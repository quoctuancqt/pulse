(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .service('configurationCountriesService', configurationCountriesService)

    function configurationCountriesService(baseApi, commonService) {
        var me = this;

        me.api = baseApi.Api();

        me.searchHandler = function (options, callBack) {
            me.api.search('clientscountries', options).then(function (resp) {
                commonService.fireCallBack(callBack, resp);
            }, function (error) {
                commonService.fireCallBack(callBack, error);
            });
        }

        me.deleteHandler = function (rowEntity, callBack) {
            me.api.remove('clientscountries', rowEntity).then(function (resp) {
                commonService.fireCallBack(callBack, resp);
            }, function (error) {
                commonService.fireCallBack(callBack, error);
            });
        }

        me.createHandler = function (rowEntity, successCallBack, errorCallBack) {
            me.api.customPost('clientscountries', 'addcountry', rowEntity).then(function (resp) {
                commonService.fireCallBack(successCallBack, resp);
            }, function (error) {
                commonService.fireCallBack(errorCallBack, error);
            });
        }

        return me;
    }
})();