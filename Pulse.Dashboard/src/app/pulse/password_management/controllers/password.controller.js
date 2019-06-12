(function () {
    'use strict';

    angular
        .module('app.pulse.password')
        .controller('PasswordCtrl', function (  profileService, commonService, notificationFactory, $state) {
            var vm = this;
            vm.password = {
                currentPassword: '',
                newPassword: ''
            }
            commonService.hideProgressBar();

            vm.update = function () {
                commonService.showProgressBar();
                profileService.changePassword(vm.password, function (successData) {
                    commonService.hideProgressBar();
                    notificationFactory.show('Password has been changed');
                    $state.reload();
                }, function (errorData) {
                    commonService.hideProgressBar();
                    notificationFactory.show(errorData.data.ExceptionMessage);
                });
            }
        });
})();