(function () {
    'use strict';

    angular
        .module('app.pulse.kiosks')
        .controller('kiosksDetailCtrl', kiosksDetailCtrl);

    function kiosksDetailCtrl($rootScope, $scope, $stateParams, collectorFactory, pulseKiosksService, commonService, PUlSE_SETTINGS, helperFactory, baseApi, notificationFactory, triSettings, sessionFactory, settingsService, $interval) {

        var vm = this;
        vm.startDate = new Date();
        vm.endDate = new Date();
        vm.endDate.setDate(vm.startDate.getDate() + 1);
        vm.pulsePlaybackModel = {};
        vm.modelSetting = {};
        vm.playbackData = [];
        vm.skip = 0;
        vm.take = 10;
        var api = baseApi.Api();

        vm.machineId = $stateParams.id;

        vm.kioskInfo = {
            name: '',
            address: '',
            ip: '',
            status: 0,
            id: 0
        };

        vm.historySchema = [
            {
                type: 'richtext', title: 'Comment', value: '', id: 'comment', properties: {
                    isVisible: true,
                    isDisabled: false,
                    isRequired: 'required'
                }
            }
        ];

        vm.formSetting = {
            handler: 'inputForm',
            headerLogo: triSettings.logo,
            bodyContent: '',
            messageType: 'pulse-text-error',
            options: vm.historySchema,
            popupType: 'confirm',
            isRestart: false,
            submitButton: function () {
                processAdd(vm.formSetting.isRestart);
            },
            cancelButton: function () {
                closePopup();
            }
        }

        vm.pulseViewModel = collectorFactory.initPulseViewModel({
            chartPieData: {
                'labels': ['Error', 'Information', 'Warning'],
                'data': []
            }
        });

        var signalR = pulseKiosksService.pulsekiosk;

        signalR.clearEvent();

        vm.getKioskInfo = function () {
            commonService.getKiosInfo(vm.machineId, successCallBack, errorCallBack);
        }

        vm.getKioskInfo();

        getSystemEvents();

        vm.bindingdata = function (obj, machineId) {
            if (vm.machineId == machineId) {
                vm.status = "Online";
                vm.pulseViewModel = collectorFactory.dataBinding(obj, vm.pulseViewModel);
                $scope.$digest();
            }
        };

        vm.onRestartMachine = function () {
            vm.formSetting.isRestart = true;
            vm.formSetting.bodyContent = 'Do you want to restart this machine ?';
            showInputForm();
        };

        vm.onShutDownMachine = function () {
            vm.formSetting.isRestart = false;
            vm.formSetting.bodyContent = 'Do you want to shutdown this machine ?';
            showInputForm();
        };

        $rootScope.$on("shareDataKiosks", function (event, args) {
            vm.bindingdata(args.obj, args.machineId);
        });

        InitModelSetting();

        vm.view = function () {
            if (!CheckValidatePlayback()) {
                vm.modelSetting.body = 'End date must be greater than start date';
                $('#pulseKioskDialog').modal('show');
            }
            else {
                InitPlaybackModel();
                GetData(vm.machineId, vm.startDate, vm.endDate, vm.skip, vm.take);
            }
        }

        vm.bindingPlayback = function (obj, machineId) {
            vm.pulsePlaybackModel = collectorFactory.dataBinding(obj, vm.pulsePlaybackModel);
        };

        //PRIVATE METHOD
        function successCallBack(resp) {
            vm.kioskInfo.name = helperFactory.checkUndefined(resp.data.Name, 'Unknown');
            vm.kioskInfo.address = helperFactory.checkUndefined(resp.data.Address, 'Unknown');
            vm.kioskInfo.ip = helperFactory.checkUndefined(resp.data.IpAddress, 'Unknown');
            vm.kioskInfo.status = resp.data.Status;
            vm.kioskInfo.id = resp.data.Id;
        }

        function errorCallBack(error) {
            notificationFactory.show(commonService.getErrorMsg(error));
        }

        function showInputForm() {
            commonService.showPopup('inputForm');
        }

        function processAdd() {
            if (vm.formSetting.isRestart) {
                vm.kioskInfo.status = 0;
                signalR.SendDeviceControl(vm.machineId, "restart");
            }
            else {
                vm.kioskInfo.status = 1;
                signalR.SendDeviceControl(vm.machineId, "shutdown");
            }

            closePopup();
        }

        function closePopup() {
            vm.formSetting.isRestart = false;
            vm.formSetting.header = '';
            vm.formSetting.bodyContent = '';
            commonService.hidePopup('inputForm');
        }

        function getSystemEvents() {
            api.get('systemevents').then(function (resp) {
                $.each(resp.data, function (i, v) {
                    var item = { timeLog: helperFactory.checkUndefined(v.CountDate) + ' ago', contentLog: helperFactory.checkUndefined(v.Description) + ' ' + helperFactory.checkUndefined(v.ActionName) };
                    vm.pulseViewModel.system_event_data.push(item);
                });
            });
        }

        function InitModelSetting() {
            vm.modelSetting = settingsService.getDialogSettings();
            vm.modelSetting.handler = 'pulseKioskDialog';
            vm.modelSetting.headerLogo = triSettings.logo;
            vm.modelSetting.icon = 'fa fa-exclamation-circle';
            vm.modelSetting.messageType = 'text-danger';
        }

        function CheckValidatePlayback() {
            if (vm.endDate <= vm.startDate) {
                return false;
            }
            return true;
        }

        function InitPlaybackModel() {
            vm.pulsePlaybackModel = collectorFactory.initPulseViewModel({
                chartPieData: {
                    'labels': ['Error', 'Information', 'Warning'],
                    'data': []
                }
            });
        }

        function GetData(machineId, startDate, endDate, skip, take) {
            api.customGet('kiosks', machineId + '/search', { machineId: machineId, startDate: startDate, endDate: endDate, skip: skip, take: take }).then(function (resp) {
                if (resp.data.Items.length > 0) {
                    CopyData(resp.data.Items);
                    if (vm.skip == -0) BindChartData();
                    vm.skip += 1;
                    if (vm.skip < resp.data.ToTalPage) {
                        GetData(machineId, startDate, endDate, vm.skip, take);
                    }
                }
                
            }, function (error) {
                console.log(error);
            });
        }

        function BindChartData() {
            var index = 0;
            var promise = $interval(function () {
                var json = vm.playbackData[index].Json;
                var obj = JSON.parse(json);
                vm.bindingPlayback(obj, vm.machineId);
                index++;
                if (vm.playbackData.length - 1 == index) {
                    $interval.cancel(promise);
                }
            }, 500);
        }

        function CopyData(data) {
            $.each(data, function (i, v) {
                vm.playbackData.push(v);
            });
        }
    }
})();