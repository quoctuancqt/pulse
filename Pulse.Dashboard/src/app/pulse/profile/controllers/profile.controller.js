(function () {
    'use strict';

    angular
        .module('app.pulse.profile')
        .controller('ProfileCtrl', function ($scope, profileService, commonService, notificationFactory, PUlSE_SETTINGS) {
            var vm = this;

            vm.userInfo = {};

            vm.getUserInfo = function () {
                commonService.showProgressBar();
                profileService.getHandler(SuccessCallback, ErrorCallback);
            }

            vm.getUserInfo();

            vm.save = function () {
                commonService.showProgressBar();
                profileService.updateHandler(vm.userInfo, UpdateSuccessCallback, ErrorCallback);
            }

            //PRIVATE METHOD
            function SuccessCallback(resp) {
                vm.userInfo = prepareDatatoDisplay(resp.data);
                commonService.hideProgressBar();
                notificationFactory.show('Success');
            }

            function UpdateSuccessCallback(resp) {
                commonService.hideProgressBar();
                notificationFactory.show('Success');
            }

            function ErrorCallback(error) {
                commonService.hideProgressBar();
                notificationFactory.show(commonService.getErrorMsg(error));
            }

            function prepareDatatoDisplay(data) {
                if (data.AvatarPath == null) {
                    data.AvatarPath = PUlSE_SETTINGS.defaultAvatar;
                }
                data.Birthday = new Date(data.Birthday);
                return data;
            }
        });
})();