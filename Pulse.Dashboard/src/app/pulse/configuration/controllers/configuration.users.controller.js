(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .controller('configurationUsersCtrl', configurationUsersCtrl);

    function configurationUsersCtrl($scope, $state, uiGridFactory, uiGridConstants, PUlSE_SETTINGS, commonService, configurationUserService, pagingService, helperFactory, notificationFactory, triSettings) {
        var vm = this;

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

        vm.formSetting = {
            header: '',
            handler: 'inputForm',
            popupType: 'confirm',
            headerLogo: triSettings.logo,
            options: vm.userSchema,
            isEditing: false,
            isAdding: false,
            submitButton: function () {
                if (vm.formSetting.isAdding) {
                    processAdd();
                }
            },
            cancelButton: function () {
                closePopup();
            }
        }

        vm.userSchema = [
            {
                type: 'email', title: 'Email', value: '', id: 'email', properties: {
                    isVisible: true,
                    isDisable: false,
                    isRequired: 'required'
                }
            }
        ];

        vm.gridOptions = {
            enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableVerticalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableColumnMenus: false,
            enableCellEdit: false,
            columnDefs: [
                { name: 'id', field: 'Id', displayName: 'ID', width:'5%' },
                { name: 'fullName', field: 'FullName', displayName: 'Full Name' },
                { name: 'gender', field: 'Gender', displayName: 'Gender', cellFilter: 'mapGender:this',width:'10%' },
                { name: 'email', field: 'Email', displayName: 'Email' },
                { name: 'address', field: 'Address', displayName: 'Address' },
                { name: 'userId', field: 'UserId', displayName: 'UserID' },
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
            configurationUserService.searchHandler(vm.searchOptions(isSearch), function (resp) {
                commonService.hideProgressBar();
                vm.gridOptions.data = prepareDatatoDisplay(resp.data.Items);
                pagingService.processPagination(vm.paginationOptions, resp);
            })
        }

        vm.init = function () {
            vm.search(false);
        }

        vm.init();

        vm.create = function () {
            vm.formSetting.isAdding = true;
            commonService.resetSchema(vm.userSchema);
            commonService.showPopup('inputForm');
        }

        vm.getTableHeight = function () {
            return uiGridFactory.getTableHeight(vm.gridOptions);
        }

        //PRIVATE METHOD
        function prepareEnity() {
            return {
                username: vm.userSchema[0].value,
                password: commonService.randomString(6),
                role: 'User'
            };
        }

        function prepareDatatoDisplay(data) {
            $.each(data, function (i, v) {
                if (v.Email == null) {
                    v.Email = 'N/A';
                }
                if (v.Address == null) {
                    v.Address = 'N/A';
                }
            })
            return data;
        }

        function closePopup() {
            vm.isAdding = false;
            commonService.hidePopup('inputForm');
        }

        function processAdd() {
            commonService.showProgressBar();
            var userDto = prepareEnity();
            configurationUserService.createHandler(userDto, SuccessCallback, ErrorCallback);
        }

        function SuccessCallback(status) {
            vm.formSetting.isAdding = false;
            commonService.hideProgressBar();
            notificationFactory.show('Success');
            commonService.hidePopup('inputForm');
            $state.reload();
        }

        function ErrorCallback(status) {
            commonService.hideProgressBar();
            notificationFactory.show(typeof status.data.ExceptionMessage != 'undefined' ? status.data.ExceptionMessage : commonService.getErrorMsg(status));
        }
    }
})();