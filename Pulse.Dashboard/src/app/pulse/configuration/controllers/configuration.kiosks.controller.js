(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .controller('configurationKiosksCtrl', configurationKiosksCtrl)

    function configurationKiosksCtrl($rootScope, $scope, $state, uiGridFactory, uiGridConstants, PUlSE_SETTINGS, commonService, configurationKiosksService, pagingService, helperFactory, notificationFactory, triSettings) {
        var vm = this;

        vm.kioskSchema = [
            {
                type: 'int', title: 'Id', value: null, id: 'id', properties: {
                    isVisible: false,
                    isDisabled: false,
                    isRequired: ''
                }
            },
            {
                type: 'string', title: 'Kiosk Name', value: '', id: 'name', properties: {
                    isVisible: true,
                    isDisabled: false,
                    isRequired: 'required'
                }
            },
            {
                type: 'select', title: 'Group Name', value: null, data: [], id: 'groupId', properties: {
                    isVisible: false,
                    isDisable: false,
                    isRequired: ''
                }
            },
            {
                type: 'autocomplete', title: 'Country Name', value: null, data: [], id: 'countryId', properties: {
                    isVisible: true,
                    isDisable: false,
                    isRequired: 'required'
                }
            },
            {
                type: 'string', title: 'Address', value: null, id: 'address', properties: {
                    isVisible: true,
                    isDisabled: false,
                    isRequired: ''
                }
            },
            {
                type: 'string', title: 'Longitude', value: null, id: 'long', properties: {
                    isVisible: false,
                    isDisabled: false,
                    isRequired: ''
                }
            }
            ,
            {
                type: 'string', title: 'Latitude', value: null, id: 'lat', properties: {
                    isVisible: false,
                    isDisabled: false,
                    isRequired: ''
                }
            },
            {
                type: 'string', title: 'Machine Id', value: null, id: 'machineId', properties: {
                    isVisible: false,
                    isDisabled: false,
                    isRequired: ''
                }
            }
            ,
            {
                type: 'string', title: 'Connection Id', value: null, id: 'connectionId', properties: {
                    isVisible: false,
                    isDisabled: false,
                    isRequired: ''
                }
            }
            ,
            {
                type: 'string', title: 'Ip Address', value: null, id: 'ipAddress', properties: {
                    isVisible: false,
                    isDisabled: false,
                    isRequired: ''
                }
            }
            ,
            {
                type: 'string', title: 'Status', value: null, id: 'status', properties: {
                    isVisible: false,
                    isDisabled: false,
                    isRequired: ''
                }
            }
        ];

        vm.modelSetting = {
            header: '',
            headerLogo: triSettings.logo,
            body: 'Put here your body',
            isConfirm: true,
            handler: 'cofirmDialog',
            messageType: 'text-info',
            icon: 'fa fa-warning-circle',
            myRightButton: function (bool) {
                processDelete();
            }
        }

        vm.formSetting = {
            header: '',
            handler: 'inputForm',
            popupType: 'confirm',
            headerLogo: triSettings.logo,
            options: vm.kioskSchema,
            isEditing: false,
            isAdding: false,
            submitButton: function () {
                if (vm.formSetting.isEditing) {
                    processEdit();
                }
            },
            cancelButton: function () {
                closePopup();
            }
        }

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

        vm.gridOptions = {
            enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableVerticalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'id', field: 'Id', display: 'ID' },
                { name: 'machineId', field: 'MachineId', display: 'Machine ID' },
                { name: 'name', field: 'Name', display: 'Kiosk Name' },
                { name: 'countryName', field: 'CountryName', display: 'Country' },
                //{ name: 'groupName', field: 'GroupName', display: 'Group' },
                { name: 'address', field: 'Address' },
                { name: 'delete', field: '', displayName: '', cellTemplate: PUlSE_SETTINGS.editButtonTmp }
            ],
            data: [],
            onRegisterApi: function (gridApi) {
                vm.gridApi = gridApi;
            }
        };

        vm.searchOptions = function (isSearch) {
            return {
                name: helperFactory.checkUndefined(vm.searchNameKey),
                address: helperFactory.checkUndefined(vm.searchAddressKey),
                skip: isSearch ? 0 : vm.paginationOptions.currentPage,
                take: vm.paginationOptions.pageSize
            }
        };

        vm.search = function (isSearch) {
            commonService.showProgressBar();
            configurationKiosksService.searchHandler(vm.searchOptions(isSearch), function (resp) {
                commonService.hideProgressBar();
                vm.gridOptions.data = resp.data.Items;
                pagingService.processPagination(vm.paginationOptions, resp);
            })
        }

        vm.init = function () {
            commonService.getClientsCountries(function (resp) {
                var clientscountries = [];
                $.each(resp.data.Items, function (i, item) {
                    var clientscountry = {
                        id: item.CountryId,
                        value: item.Country.Name
                    };
                    clientscountries.push(clientscountry);
                });

                vm.kioskSchema[3].data = clientscountries;
            });

            commonService.getGroup(function (resp) {
                vm.kioskSchema[2].data = commonService.initDropdownData(resp.data.Items);
            });

            vm.search(false);
        }

        vm.init();

        vm.getTableHeight = function () {
            return uiGridFactory.getTableHeight(vm.gridOptions);
        }

        vm.editRow = function (rowEntity) {
            vm.formSetting.isEditing = true;
            commonService.convertEntitytoSchema(vm.kioskSchema, rowEntity);
            commonService.showPopup('inputForm');
        }

        //PRIVATE METHOD
        function processEdit() {
            var kioskDto = mapEntityToDTO(vm.kioskSchema);
            configurationKiosksService.updateHandler(kioskDto, successCallBack, errorCallBack);
        }

        function closePopup() {
            vm.isEditing = false;
            commonService.hidePopup('inputForm');
        }

        function mapEntityToDTO(entity) {
            return commonService.convertEntitytoDto(entity);
        }

        function successCallBack(entity) {
            vm.formSetting.isEditing = false;
            commonService.hideProgressBar();
            notificationFactory.show('Success');
            vm.inputForm = {};
            commonService.hidePopup('inputForm');
            $rootScope.$broadcast("kiosksChange", { entity: entity });
            $state.reload();
        }

        function errorCallBack(status) {
            commonService.hideProgressBar();
            notificationFactory.show(commonService.getErrorMsg(status));
        }
    }
})();