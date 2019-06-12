(function () {
    'use strict';

    angular
        .module('app.pulse.kiosks')
        .controller('kiosksDashboardCtrl', kiosksDashboardController);

    function kiosksDashboardController($state, pulseKiosksService, $rootScope, pagingService, PUlSE_SETTINGS, helperFactory, commonService, baseApi, collectorFactory) {
        var vm = this;

        var api = baseApi.Api();

        var signalR = pulseKiosksService.pulsekiosk;

        vm.groups = [];

        vm.countries = [];

        $rootScope.isReloadMap = false;

        vm.kiosksData = [];

        signalR.clearEvent();

        vm.paginationOptions = pagingService.getSetting();

        vm.paginationOptions.callBack = function (value) {
            vm.paginationOptions.currentPage = value - 1;
            vm.paginationOptions.activePage = value;
            vm.search(false);
        };

        vm.paginationOptions.pageSizeCallBack = function (value) {
            PUlSE_SETTINGS.pageSize = value;
            $state.reload();
        };

        vm.searchOptions = function (isSearch) {
            return {
                name: helperFactory.checkUndefined(vm.searchNameKey),
                address: helperFactory.checkUndefined(vm.searchAddressKey),
                countryId: vm.searchCountry == null ? -1 : vm.searchCountry,
                groupId: vm.searchGroup == null ? -1 : vm.searchGroup,
                skip: isSearch ? 0 : vm.paginationOptions.currentPage,
                take: vm.paginationOptions.pageSize
            }
        };

        vm.search = function (isSearch) {
            commonService.showProgressBar();
            api.search('kiosks', vm.searchOptions(isSearch)).then(function (resp) {
                commonService.hideProgressBar();
                mapData(resp.data.Items);
                $rootScope.isReloadMap = true;
                pagingService.processPagination(vm.paginationOptions, resp);
            });
        }

        vm.search(false);

        vm.goToDetail = function (machineId) {
            $state.go('triangular.detail', { id: machineId });
        }

        signalR.hub.on("OnDisconnected", function (machineId) {
            updateKioskInfo(machineId, null);
        });

        signalR.processPerfStatusMessageToKiosk(function (machineId, json) {
            console.log(json);
            if (vm.kiosksData.length > 0) {
                var obj = JSON.parse(json);
                updateKioskInfo(machineId, obj);
                $rootScope.isKioskStart = true;
                if (typeof ($rootScope.$$listeners["shareDataKiosks"]) != "undefined") {
                    $rootScope.$broadcast("shareDataKiosks", { machineId: machineId, obj: obj });
                };
            }
        });

        $rootScope.$on("kiosksChange", function (event, args) {
            vm.search(false);
            signalR.SendUpdateUserData(args.entity.connectionId, args.entity.name);
        });

        //PRIVATE METHOD
        function InitController() {
            vm.searchCountry = -1;
            vm.searchGroup = -1;

            commonService.getCountry(function (resp) {
                vm.countries.push({ id: -1, name: 'Select Country' });
                $.each(resp.data.Items, function (i, v) {
                    var country = {
                        id: v.Id,
                        name: v.Name
                    };
                    vm.countries.push(country);
                });
            });

            commonService.getGroup(function (resp) {
                vm.groups.push({ id: -1, name: 'Select Group' });
                $.each(resp.data.Items, function (i, v) {
                    var group = {
                        id: v.Id,
                        name: v.Name
                    };
                    vm.groups.push(group);
                });
            });
        }

        InitController();

        function getPulseViewModel() {
            return collectorFactory.initPulseViewModel({
                chartPieData: {
                    'labels': ['Error', 'Information', 'Warning'],
                    'data': []
                }
            });
        };

        function mapData(kiosksArray) {
            vm.kiosksData = [];
            $.each(kiosksArray, function (i, item) {
                var pulseViewModel = getPulseViewModel();
                var kiosk = {};
                kiosk.Index = i;
                kiosk.MachineId = item.MachineId;
                kiosk.Info = {
                    Name: item.Name,
                    Address: item.Address,
                    CountryName: item.CountryName,
                    GroupName: item.GroupName,
                    Long: item.Long,
                    Lat: item.Lat,
                    Status: item.Status
                };

                kiosk.Signal = pulseViewModel;
                if (item.DefaultValue != null) {
                    kiosk.Signal = collectorFactory.dataBinding(JSON.parse(item.DefaultValue), kiosk.Signal);
                }
                vm.kiosksData.push(kiosk);
            });
        };

        function updateKioskInfo(machineId, jsonObj) {
            for (var i = 0; i < vm.kiosksData.length; i++) {
                if (vm.kiosksData[i].MachineId == machineId) {
                    if (jsonObj != null) {
                        vm.kiosksData[i].Info.Status = 1;
                        vm.kiosksData[i].Signal = collectorFactory.dataBinding(jsonObj, vm.kiosksData[i].Signal);
                    }
                    else {
                        vm.kiosksData[i].Info.Status = 0;
                        $rootScope.$apply();
                    }
                    return;
                }
            }
        };

        function onDisconnected(machineId) {
            debugger;
            me.updateKioskInfo(machineId, null)
        }
    }
})();