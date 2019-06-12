(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .service('configurationGroupsService', configurationGroupsService)

    function configurationGroupsService(baseApi, commonService) {
        var me = this;

        me.api = baseApi.Api();

        me.searchHandler = function (options, callBack) {
            me.api.search('groups', options).then(function (resp) {
                commonService.fireCallBack(callBack, resp);
            }, function (error) {
                commonService.fireCallBack(callBack, error);
            });
        }

        me.updateHandler = function (rowEntity, successCallBack, errorCallBack) {
            me.api.update('groups', rowEntity).then(function (resp) {
                commonService.fireCallBack(successCallBack, resp);
            }, function (error) {
                commonService.fireCallBack(errorCallBack, error);
            });
        }

        me.deleteHandler = function (rowEntity, callBack) {
            me.api.remove('groups', rowEntity).then(function (resp) {
                commonService.fireCallBack(callBack, resp);
            }, function (error) {
                commonService.fireCallBack(callBack, error);
            });
        }

        me.createHandler = function (rowEntity, successCallBack, errorCallBack) {
            me.api.create('groups', rowEntity).then(function (resp) {
                commonService.fireCallBack(successCallBack, resp);
            }, function (error) {
                commonService.fireCallBack(errorCallBack, error);
            });
        }

        return me;
    }
})();