(function () {
    'use strict';

    angular
        .module('app.pulse.login')
        .controller('LoginCtrl', function ($scope, $http, $state, PUlSE_SETTINGS, sessionFactory, triSettings, settingsService, commonService, $injector, $interval) {
            var vm = $scope;

            commonService.hideProgressBar(500);

            vm.name = triSettings.name;

            vm.copyright = triSettings.copyright;

            vm.version = "v" + triSettings.version;

            vm.modelSetting = {};

            var webapi_base = PUlSE_SETTINGS.apiUri;

            var successCallback = function (resp) {
                commonService.hideProgressBar(0);
                if (typeof (resp.data.error) == "undefined") {
                    sessionFactory.set('access_token', resp.data.access_token);
                    sessionFactory.set('refresh_token', resp.data.refresh_token);
                    sessionFactory.set('clientName', resp.data.clientName);
                    sessionFactory.set('clientId', resp.data.clientId);
                    sessionFactory.set('fullName', resp.data.fullName);
                    sessionFactory.set('role', resp.data.role);
                    sessionFactory.set('avatar', resp.data.avatarPath == "" ? PUlSE_SETTINGS.defaultAvatar : resp.data.avatarPath);
                    sessionFactory.set('emailConfirm', resp.data.emailConfirm);
                    sessionFactory.set('kioskGroupName', resp.data.clientName.replace(/ /g, '').toLowerCase() + resp.data.clientId);
                    sessionFactory.set('clientId', resp.data.clientId);
                    vm.modelSetting.body = 'Login Success!';
                    vm.modelSetting.isSuccess = true;
                    vm.modelSetting.isConfirm = false;
                    checkUserIsActive(resp.data.emailConfirm, resp.data.role);
                }
                else {
                    vm.modelSetting.body = resp.data.error;
                    vm.modelSetting.isSuccess = false;
                    vm.modelSetting.isConfirm = false;
                    $('#loginDialog').modal('show');
                }
            }

            var errorCallback = function () {
                commonService.hideProgressBar(0);
                vm.modelSetting.body = 'Login Fail!';
                vm.modelSetting.isSuccess = false;
                vm.modelSetting.isConfirm = false;
                $('#loginDialog').modal('show');
            }

            vm.prepareUserDto = function () {
                var user = {};
                user.username = vm.username;
                user.password = vm.password;
                return user;
            };

            vm.submitForm = function (form) {
                if (form.$valid) {
                    commonService.showProgressBar();
                    var userDto = vm.prepareUserDto();
                    $http.post(webapi_base + 'oauths', userDto).then(successCallback, errorCallback);
                }
            }

            InitModelSetting();
            //PRIVATE METHOD
            function InitModelSetting() {
                vm.modelSetting = settingsService.getDialogSettings();
                vm.modelSetting.handler = 'loginDialog';
                vm.modelSetting.myRightButton = function () {
                    $('div.modal-backdrop').hide();
                }
            }


            function setDefaultPageByRole(role) {
                switch (role) {
                    case 'Administrator':
                        {
                            $state.go('triangular.sales-layout');
                            break;
                        }
                    default:
                        {
                            var pulseServerService = $injector.get('pulseServerService')
                            var signalR = pulseServerService.pulseServer;

                            var interval = $interval(function () {
                                if (signalR.isCallStartConnection) {
                                    $interval.cancel(interval);
                                    $state.go('triangular.kiosk');
                                }
                            }, 500);
                            
                            break;
                        }
                }
            }

            function checkUserIsActive(isActive, role) {
                if (isActive == 0) {
                    $state.go('authentication.wizard');
                }
                else {
                    setDefaultPageByRole(role);
                }
            }
        });
})();