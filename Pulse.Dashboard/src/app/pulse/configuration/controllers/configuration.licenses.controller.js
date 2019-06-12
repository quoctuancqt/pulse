(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .controller('configurationLicensesCtrl', configurationLicensesCtrl);

    function configurationLicensesCtrl($scope, $state, uiGridFactory, uiGridConstants, PUlSE_SETTINGS, commonService, triSettings, configurationKiosksService, pagingService, helperFactory, baseApi, sessionFactory) {
        var vm = this;
        var api = baseApi.Api();
        vm.status = [];
        vm.searchLicenseKey = '';
        vm.searchClientId = sessionFactory.get('clientId');
        vm.isAdmin = sessionFactory.get('role') == 'Administrator' ? true : false;
        vm.searchStatus = 0;
        vm.clients = [];
        vm.isAdminstrator = sessionFactory.get('role') == 'Administrator' ? true : false;

        vm.licenseSchema = [
            {
                type: 'int', title: 'Amout', value: 1, id: 'amount', properties: {
                    isVisible: true,
                    isDisabled: false,
                    isRequired: 'required',
                    min: 1,
                    max: 20
                }
            },
            {
                type: 'select', title: 'Client Name', value: 0, data: [], id: 'clientName', properties: {
                    isVisible: true,
                    isDisabled: false,
                    isRequired: 'required'
                }
            }
        ];

        vm.formSetting = {
            header: '',
            handler: 'inputForm',
            popupType: 'confirm',
            headerLogo: triSettings.logo,
            options: vm.licenseSchema,
            isEditing: false,
            isAdding: false,
            submitButton: function () {
                if (vm.formSetting.isAdding) {
                    processGenerate();
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
            enableCellEdit: false,
            columnDefs: [
                { name: 'id', field: 'Id', displayName: 'ID', width: '1%' },
                { name: 'machineId', field: 'MachineId', displayName: 'Machine ID', width: '24%' },
                { name: 'licenseKey', field: 'LicenseKey', displayName: 'License Key', width: '24%' },
                { name: 'clientId', field: 'ClientId', displayName: 'Client ID', width: '*' },
                {
                    name: 'isActive', field: 'IsActive', displayName: 'Activate', type: 'boolean',
                    cellTemplate: '<div><span ng-class="{\'label label-success\': row.entity.IsActive , \'label label-warning\': !row.entity.IsActive}">{{row.entity.IsActive ? \'Activate\' : \'Deactivate\'}}</span></div>', width: '10%'
                },
                { name: 'expiryDate', field: 'ExpiryDate', displayName: 'Expire Date', cellFilter: PUlSE_SETTINGS.dateFormat, width: '*' },
                //{ name: 'createdAt', field: 'CreatedAt', displayName: 'Created At', cellFilter: PUlSE_SETTINGS.dateFormat, width: '*' },
                //{ name: 'createdBy', field: 'CreatedBy', displayName: 'Created By', width: '*' },
                //{ name: 'updatedAt', field: 'UpdatedAt', displayName: 'Updated At', cellFilter: PUlSE_SETTINGS.dateFormat, width: '*' },
                //{ name: 'updatedBy', field: 'UpdatedBy', displayName: 'Updated By', width: '*' },
            ],
            data: [],
            onRegisterApi: function (gridApi) {
                vm.gridApi = gridApi;
            }
        };

        vm.searchOptions = function (isSearch) {
            return {
                licenseKey: helperFactory.checkUndefined(vm.searchLicenseKey),
                clientId : helperFactory.checkUndefined(vm.searchClientId),
                isActive: vm.searchStatus == 0 ? false : true,
                skip: isSearch ? 0 : vm.paginationOptions.currentPage,
                take: vm.paginationOptions.pageSize
            }
        };

        vm.search = function (isSearch) {
            commonService.showProgressBar();
            commonService.getLicense(vm.searchOptions(isSearch), function (resp) {
                commonService.hideProgressBar();
                vm.gridOptions.data = vm.prepareDataDisplay(resp.data.Items);
                pagingService.processPagination(vm.paginationOptions, resp);
            });
        }

        vm.getClients = function () {
            commonService.getClients(function (resp) {
                $.each(resp.data.Items, function (i, v) {
                    var client = {
                        id: v.ClientId,
                        value: v.Name
                    };
                    vm.clients.push(client);
                });
                vm.licenseSchema[1].data = vm.clients;
            })
        }

        vm.init = function () {
            vm.status = commonService.initStatusData();
            vm.getClients();
            vm.search(false);
        }

        vm.init();

        vm.generate = function () {
            vm.formSetting.isAdding = true;
            vm.licenseSchema[0].value = 1;
            vm.licenseSchema[1].value = null;
            commonService.showPopup('inputForm');
        }

        vm.getTableHeight = function () {
            return uiGridFactory.getTableHeight(vm.gridOptions);
        }

        vm.prepareDataDisplay = function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].IsActive == false) {
                    data[i].MachineId = 'N/A';
                }
                if (data[i].ExpiryDate == null) {
                    data[i].ExpiryDate = 'N/A';
                }
                if (data[i].CreatedAt == null) {
                    data[i].CreatedAt = 'N/A';
                }
                if (data[i].CreatedBy == null) {
                    data[i].CreatedBy = 'N/A';
                }
                if (data[i].UpdatedAt == null) {
                    data[i].UpdatedAt = 'N/A';
                }
                if (data[i].UpdatedBy == null) {
                    data[i].UpdatedBy = 'N/A';
                }
            }
            return data;
        }

        //PRIVATE METHOD
        function processGenerate() {
            var params = {
                number : vm.licenseSchema[0].value,
                clientId: vm.licenseSchema[1].value
            };
            commonService.showProgressBar();
            api.customPost('kiosks', 'generatelicensekey', params).then(function () {
                $state.reload();
                commonService.hideProgressBar();
            }, function (error) {
                console.log(error);
            });
        }

        function closePopup() {
            vm.isAdding = false;
            commonService.hidePopup('inputForm');
        }
    }
})();