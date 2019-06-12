(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .controller('configurationCountriesCtrl', configurationCountriesCtrl);

    function configurationCountriesCtrl($scope, $state, uiGridFactory, uiGridConstants, PUlSE_SETTINGS, commonService, configurationCountriesService, pagingService, helperFactory, notificationFactory, triSettings) {
        var vm = this;

        vm.deleteItem = {};

        vm.countries = [];

        vm.clientscountriesSchema = [
            {
                type: 'autocomplete', title: 'Country Name', value: null, data: [], id: 'countryId', properties: {
                    isVisible: true,
                    isDisable: false,
                    isRequired: 'required'
                }
            },
            {
                type: 'int', title: 'Id', value: null, id: 'id', properties: {
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
            header: 'Pulse',
            handler: 'inputForm',
            options: vm.groupSchema,
            popupType: 'confirm',
            headerLogo: triSettings.logo,
            isEditing: false,
            isAdding: false,
            submitButton: function () {
                if (vm.formSetting.isAdding) {
                    processAdd();
                }
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
                { name: 'country', field: 'Country.Name', displayName: 'Country Name'},
                { name: 'delete', field: '', displayName: '', cellTemplate: PUlSE_SETTINGS.deleteButtonTmp }
            ],
            cellEditableCondition: function (vm) {
                return (vm.col.colDef.name == 'name');
            },
            data: [],
            onRegisterApi: function (gridApi) {
                vm.gridApi = gridApi;
            }
        };

        vm.searchOptions = function () {
            return {
                skip: vm.paginationOptions.currentPage,
                take: vm.paginationOptions.pageSize
            }
        };

        vm.search = function () {
            commonService.showProgressBar();
            configurationCountriesService.searchHandler(vm.searchOptions(), function (resp) {
                commonService.hideProgressBar();
                vm.gridOptions.data = resp.data.Items;
                pagingService.processPagination(vm.paginationOptions, resp);
            })
        }

        vm.getCountries = function () {
            commonService.getCountry(function (resp) {
                
                $.each(resp.data, function (i ,v) {
                    var country = {
                        id: v.Id,
                        value: v.Name
                    };
                    vm.countries.push(country);
                });

                vm.clientscountriesSchema[0].data = vm.countries;
            });
        }

        vm.init = function () {
            vm.getCountries();
            vm.search();
        }

        vm.init();

        vm.getTableHeight = function () {
            return uiGridFactory.getTableHeight(vm.gridOptions);
        }

        vm.deleteRow = function (rowEntity) {
            vm.modelSetting.body = 'Do you want to remove ' + rowEntity.Country.Name + '?';
            vm.deleteItem = rowEntity;
            commonService.showPopup('cofirmDialog');
        }

        vm.addRow = function () {
            vm.formSetting.isAdding = true;
            vm.clientscountriesSchema[0].value =  null;
            vm.clientscountriesSchema[1].value = '';
            commonService.showPopup('inputForm');
        }

        //PRIVATE METHOD
        function processDelete() {
            commonService.showProgressBar();
            configurationCountriesService.deleteHandler(vm.deleteItem, function () {
                commonService.hideProgressBar();
                notificationFactory.show('Success', 'top right', 5000);
                vm.deleteItem = {};
                $state.reload();
            })
        }

        function processAdd() {
            var clienscountriesDto = mapEntityToDTO(vm.clientscountriesSchema);
            configurationCountriesService.createHandler(clienscountriesDto, successCallBack, errorCallBack);
        }

        function closePopup() {
            vm.isAdding = false;
            vm.isEditing = false;
            commonService.hidePopup('inputForm');
        }

        function mapEntityToDTO(entity) {
            return commonService.convertEntitytoDto(entity);
        }

        function successCallBack(status) {
            vm.formSetting.isAdding = false;
            vm.formSetting.isEditing = false;
            commonService.hideProgressBar();
            notificationFactory.show('Success');
            vm.inputForm = {};
            commonService.hidePopup('inputForm');
            $state.reload();
        }

        function errorCallBack(status) {
            commonService.hideProgressBar();
            notificationFactory.show(commonService.getErrorMsg(status));
        }
    }
})();