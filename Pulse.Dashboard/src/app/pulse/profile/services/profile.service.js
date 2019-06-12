(function () {
    'use strict';

    angular
        .module('app.pulse.profile')
        .service('profileService', profileService)

    function profileService(baseApi, commonService) {
        var me = this;

        me.api = baseApi.Api();

        me.getHandler = function (successCallBack, errorCallBack) {
            me.api.customGet('oauths', 'getuserprofile', null).then(function (resp) {
                commonService.fireCallBack(successCallBack, resp);
            }, function (error) {
                commonService.fireCallBack(errorCallBack, error);
            });
        }

        me.updateHandler = function (entity, successCallBack, errorCallBack) {
            me.api.customPut('oauths', 'updateuserprofile', entity).then(function (resp) {
                commonService.fireCallBack(successCallBack, resp);
            }, function (error) {
                commonService.fireCallBack(errorCallBack, error);
            });
        }

        me.changePassword = function (entity, successCallBack, errorCallBack) {
            me.api.customPost('oauths', 'changepassword', entity).then(function (resp) {
                commonService.fireCallBack(successCallBack, resp);
            }, function (error) {
                commonService.fireCallBack(errorCallBack, error);
            });
        }

        return me;
    }
})();