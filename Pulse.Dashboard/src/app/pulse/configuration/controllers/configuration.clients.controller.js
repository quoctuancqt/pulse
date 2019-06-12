(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .controller('configurationClientsCtrl', configurationClientsCtrl);

    function configurationClientsCtrl($scope, $state, uiGridFactory, uiGridConstants, PUlSE_SETTINGS, commonService, configurationClientsService, pagingService, helperFactory, notificationFactory, triSettings, sessionFactory) {
        var vm = this;

        vm.deleteItem = {};

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

        vm.clientSchema = [
            {
                type: 'string', title: 'Client Id', value: '', id: 'clientId', properties: {
                    isVisible: false,
                    isDisable: true,
                    isRequired: ''
                }
            },
            {
                type: 'string', title: 'Secret', value: '', id: 'secret', properties: {
                    isVisible: false,
                    isDisable: true,
                    isRequired: ''
                }
            },
            {
                type: 'string', title: 'Secret Key', value: '', id: 'secretKey', properties: {
                    isVisible: false,
                    isDisable: true,
                    isRequired: ''
                }
            },
            {
                type: 'email', title: 'Email', value: '', id: 'email', properties: {
                    isVisible: true,
                    isDisable: false,
                    isRequired: 'required'
                }
            },
            {
                type: 'string', title: 'Client Name', value: '', id: 'name', properties: {
                    isVisible: true,
                    isDisabled: false,
                    isRequired: 'required'
                }
            },
            {
                type: 'string', title: 'Signalr Url', value: '', id: 'signalrUrl', properties: {
                    isVisible: true,
                    isDisable: false,
                    isRequired: 'required'
                }
            },
            {
                type: 'string', title: 'Mongo Name', value: '', id: 'mongoName', properties: {
                    isVisible: false,
                    isDisabled: false,
                    isRequired: ''
                }
            },
            {
                type: 'string', title: 'Mongo Connection String', value: '', id: 'mongoConnectionString', placeholder: 'Mongo Connection String (e.g.localhost:80)', properties: {
                    isVisible: true,
                    isDisable: false,
                    isRequired: 'required'
                }
            },
            {
                type: 'int', title: 'Refresh Token Lifetime', value: 0, id: 'refreshTokenLifeTime', properties: {
                    isVisible: true,
                    isDisable: false,
                    isRequired: 'required'
                }
            },
            {
                type: 'int', title: 'Token Lifetime', value: 0, id: 'tokenLifeTime', properties: {
                    isVisible: true,
                    isDisable: false,
                    isRequired: 'required'
                }
            },
            {
                type: 'select', title: 'Allowed Grant', value: 1, data: [{ id: 1, value: 'Client' }], id: 'allowedGrant', properties: {
                    isVisible: false,
                    isDisable: false,
                    isRequired: ''
                }
            },
            {
                type: 'string', title: 'Allowed Origin', value: '', id: 'allowedOrigin', properties: {
                    isVisible: false,
                    isDisable: false,
                    isRequired: 'required'
                }
            },
            {
                type: 'int', title: 'Id', value: 0, id: 'id', properties: {
                    isVisible: false,
                    isDisabled: false,
                    isRequired: ''
                }
            }
        ];

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
            enableVerticalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableColumnMenus: false,
            enableCellEdit: false,
            columnDefs: [
                { name: 'id', field: 'Id', displayName: 'ID',width:'6%' },
                { name: 'name', field: 'Name', displayName: 'Name',width:'10%' },
                { name: 'signalrUrl', field: 'SignalrUrl', displayName: 'Signalr Url', width:'15%' },
                { name: 'mongoName', field: 'MongoName', displayName: 'Mongo Name',width:'10%' },
                { name: 'mongoConnectionString', field: 'MongoConnectionString', displayName: 'Mongo Connection String', width: '15%' },
                { name: 'refreshTokenLifeTime', field: 'RefreshTokenLifeTime', displayName: 'Refresh Token LifeTime',width:'15%' },
                { name: 'tokenLifeTime', field: 'TokenLifeTime', displayName: 'Token LifeTime',width:'10%' },
                { name: 'delete', field: '', displayName: '', cellTemplate: PUlSE_SETTINGS.gridButtonTmp, width: '20%' }
            ],
            data: [],
            onRegisterApi: function (gridApi) {
                vm.gridApi = gridApi;
            }
        };

        vm.searchOptions = function (isSearch) {
            return {
                name: helperFactory.checkUndefined(vm.name),
                skip: isSearch ? 0 : vm.paginationOptions.currentPage,
                take: vm.paginationOptions.pageSize
            }
        };

        vm.search = function (isSearch) {
            commonService.showProgressBar();
            configurationClientsService.searchHandler(vm.searchOptions(isSearch), function (resp) {
                commonService.hideProgressBar();
                vm.gridOptions.data = resp.data.Items;
                pagingService.processPagination(vm.paginationOptions, resp);
            })
        }

        vm.init = function () {
            vm.search(false);
        }

        vm.init();

        vm.create = function () {
            $state.go('triangular.configuration-client', { mode: 'add', id: null });
        }

        vm.editRow = function (rowEntity) {
            $state.go('triangular.configuration-client', { mode:' edit', id: rowEntity.ClientId });
        }

        vm.deleteRow = function (rowEntity) {
            vm.modelSetting.body = 'Do you want to remove ' + rowEntity.Name + '?';
            vm.deleteItem = rowEntity;
            commonService.showPopup('cofirmDialog');
        }

        vm.getTableHeight = function () {
            return uiGridFactory.getTableHeight(vm.gridOptions);
        }

        //PRIVATE METHOD
        function closePopup() {
            vm.isEditing = false;
            vm.isAdding = false;
            commonService.hidePopup('inputForm');
        }

        function processDelete() {
            commonService.showProgressBar();
            configurationClientsService.deleteHandler(vm.deleteItem, function () {
                commonService.hideProgressBar();
                notificationFactory.show('Success');
                vm.deleteItem = {};
                $state.reload();
            })
        }

        function mapEntityToDTO(entity) {
            return commonService.convertEntitytoDto(entity);
        }

        function successCallBack(status) {
            commonService.hideProgressBar();
            notificationFactory.show('Success');
            commonService.hidePopup('inputForm');
            $state.reload();
        }

        function errorCallBack(status) {
            commonService.hideProgressBar();
            notificationFactory.show(status.data.ExceptionMessage);
        }
    }
})();