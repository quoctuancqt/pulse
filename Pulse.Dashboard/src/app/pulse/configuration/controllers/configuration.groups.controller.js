(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .controller('configurationGroupsCtrl', configurationGroupsCtrl);

    function configurationGroupsCtrl($scope, $state, uiGridFactory, uiGridConstants, PUlSE_SETTINGS, commonService, configurationGroupsService, pagingService, helperFactory, notificationFactory, triSettings) {
        var vm = this;

        vm.deleteItem = {};

        vm.groupSchema = [
            {
                type: 'string', title: 'Group Name', value: '', id: 'name', properties: {
                    isVisible: true,
                    isDisabled: false,
                    isRequired: 'required'
                }
            },
            {
                type: 'string', title: 'Created Date', value: null, id: 'createdAt', properties: {
                    isVisible: false,
                    isDisable: false,
                    isRequired: ''
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
            header: '',
            handler: 'inputForm',
            popupType: 'confirm',
            headerLogo: triSettings.logo,
            options: vm.groupSchema,
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
                { name: 'name', field: 'Name', displayName: 'Groups Name' },
                { name: 'createdAt', field: 'CreatedAt', displayName: 'Create At', cellFilter: PUlSE_SETTINGS.dateFormat },
                { name: 'kiosks', field: 'Kiosks', displayName: 'Kiosks Count' },
                { name: 'delete', field: '', displayName: '', cellTemplate: PUlSE_SETTINGS.gridButtonTmp }
            ],
            cellEditableCondition: function (vm) {
                return (vm.col.colDef.name == 'name');
            },
            data: [],
            onRegisterApi: function (gridApi) {
                vm.gridApi = gridApi;
            }
        };

        vm.searchOptions = function (isSearch) {
            return {
                name: helperFactory.checkUndefined(vm.searchGroupName),
                skip: isSearch ? 0 : vm.paginationOptions.currentPage,
                take: vm.paginationOptions.pageSize
            }
        };

        vm.search = function (isSearch) {
            commonService.showProgressBar();
            configurationGroupsService.searchHandler(vm.searchOptions(isSearch), function (resp) {
                commonService.hideProgressBar();
                vm.gridOptions.data = prepareData(resp.data.Items);
                pagingService.processPagination(vm.paginationOptions, resp);
            })
        }

        vm.init = function () {
            vm.search(false);
        }

        vm.init();

        vm.getTableHeight = function () {
            return uiGridFactory.getTableHeight(vm.gridOptions);
        }

        vm.deleteRow = function (rowEntity) {
            vm.modelSetting.body = 'Do you want to remove ' + rowEntity.Name + '?';
            vm.deleteItem = rowEntity;
            commonService.showPopup('cofirmDialog');
        }

        vm.addRow = function () {
            vm.formSetting.isAdding = true;
            vm.groupSchema[0].value = '';
            vm.groupSchema[1].value = new Date();
            vm.groupSchema[2].value = null;
            commonService.showPopup('inputForm');
        }

        vm.editRow = function (rowEntity) {
            vm.formSetting.isEditing = true;
            commonService.convertEntitytoSchema(vm.groupSchema, rowEntity);
            commonService.showPopup('inputForm');
        }

        //PRIVATE METHOD
        function processDelete() {
            commonService.showProgressBar();
            configurationGroupsService.deleteHandler(vm.deleteItem, function () {
                commonService.hideProgressBar();
                notificationFactory.show('Success', 'top right', 5000);
                vm.deleteItem = {};
                $state.reload();
            })
        }

        function processAdd() {
            var groupDto = mapEntityToDTO(vm.groupSchema);
            configurationGroupsService.createHandler(groupDto, successCallBack, errorCallBack);
        }

        function processEdit() {
            var groupDto = mapEntityToDTO(vm.groupSchema);
            configurationGroupsService.updateHandler(groupDto, successCallBack, errorCallBack);
        }

        function closePopup() {
            vm.isAdding = false;
            vm.isEditing = false;
            commonService.hidePopup('inputForm');
        }

        function mapEntityToDTO(entity) {
            return commonService.convertEntitytoDto(entity);
        }

        function prepareData(data) {
            for (var i = 0; i < data.length; i++) {
                var kiosks = data[i].Kiosks.length;
                data[i].Kiosks = kiosks;
            }
            return data;
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