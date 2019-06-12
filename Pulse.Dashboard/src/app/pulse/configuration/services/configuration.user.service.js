(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .service('configurationUserService', configurationUserService)

    function configurationUserService(baseApi, commonService) {
        var me = this;

        me.api = baseApi.Api();

        me.searchHandler = function (options, callBack) {
            me.api.search('oauths', options).then(function (resp) {
                commonService.fireCallBack(callBack, resp);
            }, function (error) {
                commonService.fireCallBack(callBack, error);
            });
        }

        me.createHandler = function (entity, successCallBack, errorCallBack) {
            me.api.customPost('oauths', 'register', entity).then(function (resp) {
                commonService.fireCallBack(successCallBack, resp);
            }, function (error) {
                commonService.fireCallBack(errorCallBack, error);
            });
        }

        return me;
    }
})();