(function () {
    'use strict';

    angular
        .module('app.pulse.wizard', [])
        .controller('WizardCtrl', function (profileService, commonService, notificationFactory, $timeout, sessionFactory, $state, baseApi) {
            var vm = this;
            var api = baseApi.Api();
            vm.isPasswordChanged = false;
            commonService.hideProgressBar();
            var userRole = sessionFactory.get('role');

            vm.account = {
                currentPassword: '',
                newPassword: ''
            };

            vm.userInfo = {};

            vm.getUserProfile = function () {
                profileService.getHandler(function (successData) {
                    vm.userInfo = prepareDatatoDisplay(successData.data);
                    notificationFactory.show('Success');
                }, function (errorData) {
                    notificationFactory.show(errorData.data.ExceptionMessage);
                });
            }

            vm.getUserProfile();

            vm.changePassword = function () {
                commonService.showProgressBar();
                profileService.changePassword(vm.data.account, function (successData) {
                    vm.isPasswordChanged = true;
                    commonService.hideProgressBar();
                    notificationFactory.show('Password has been changed');
                }, function (errorData) {
                    commonService.hideProgressBar();
                    notificationFactory.show(errorData.data.ExceptionMessage);
                });
            }

            vm.updateUserProfile = function () {
                commonService.showProgressBar();
                profileService.updateHandler(vm.userInfo, function () {
                    vm.addClientUser();
                }, function (errorData) {
                    commonService.hideProgressBar();
                    notificationFactory.show(errorData.data.ExceptionMessage);
                });
            }

            vm.submit = function () {
                commonService.showProgressBar();
                vm.updateUserProfile();
            }

            vm.addClientUser = function () {
                api.create('clientuser', null).then(function (resp) {
                    commonService.hideProgressBar();
                    notificationFactory.show('Update Completed');
                    setDefaultPageByRole(userRole);
                }, function (error) {
                    commonService.hideProgressBar();
                    notificationFactory.show(error.data.ExceptionMessage);
                });
            }

            //PRIVATE METHOD
            function prepareDatatoDisplay(data) {
                if (data.Birthday != null) {
                    data.Birthday = new Date(data.Birthday);
                }
                return data;
            }

            function setDefaultPageByRole(role) {
                commonService.hideProgressBar();
                switch (role) {
                    case 'Administrator': $state.go('triangular.sales-layout'); break;
                    default: $state.go('triangular.kiosk'); break;
                }
            }
        });
})();