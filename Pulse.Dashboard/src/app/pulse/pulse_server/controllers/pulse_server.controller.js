(function () {
    'use strict';

    angular
        .module('app.pulse.pulse_server')
        .controller('pulseServerCtrl', PulseServerController);

    function PulseServerController($scope, pulseServerService, $rootScope, baseApi, $filter, settingsService, triSettings) {

        var vm = this;

        var api = baseApi.Api();

        vm.modelSetting = {};

        vm.summaryModel = {
            kiosks: {
                class: 'panel-primary',
                icon: 'zmdi zmdi-desktop-windows',
                value: 0,
                title: 'KIOSK',
                footerLeft: 'Online : ',
                footerRight: 'Offline : '
            },
            website: {
                class: 'panel-green',
                icon: 'fa fa-edge',
                value: 110,
                title: 'WEBSITE',
                footerLeft: 'Global : 55',
                footerRight: 'Internal : 55'
            },
            iot: {
                class: 'panel-red',
                icon: 'zmdi zmdi-device-hub',
                value: 120,
                title: 'IOT',
                footerLeft: 'AI : 60',
                footerRight: 'Learning: 60'
            }
        }

        vm.controllerInit = function () {
            api.search('kiosks').then(function (resp) {
                vm.summaryModel.kiosks.value = resp.data.TotalRecord;
                if (resp.data.TotalRecord > 1) {
                    vm.summaryModel.kiosks.title = "KIOSKS";
                }
                var online = $filter('filter')(resp.data.Items, { Status: 1 });
                var offline = $filter('filter')(resp.data.Items, { Status: 0 });
                vm.summaryModel.kiosks.footerLeft += online.length;
                vm.summaryModel.kiosks.footerRight += offline.length;
            });
        }

        vm.controllerInit();

        var signalR = pulseServerService.pulseServer;

        signalR.init();

        signalR.reconnect(function (change) {
            if (change.newState === $.signalR.connectionState.reconnecting) {
                vm.modelSetting.body = 'The connection to server has been disconnected. Please contact to Administrator';
                $('#pulseServerDialog').modal('show');
            }
            else if (change.newState === $.signalR.connectionState.connected) {
                $('#pulseServerDialog').modal('hide');
            }
            $scope.$apply();
        });

        vm.pulseViewModel = signalR.data;

        InitModelSetting();

        //PIRVATE METHOD
        function InitModelSetting() {
            vm.modelSetting = settingsService.getDialogSettings();
            vm.modelSetting.handler = 'pulseServerDialog';
            vm.modelSetting.headerLogo = triSettings.logo;
            vm.modelSetting.icon = 'fa fa-exclamation-circle';
            vm.modelSetting.messageType = 'text-danger';
        }
    }
})();