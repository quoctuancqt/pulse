(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .service('configurationKiosksService', configurationKiosksService)

    function configurationKiosksService(googleApiService, $filter, $interval, notificationFactory, $timeout, baseApi, commonService) {
        var me = this;

        me.api = baseApi.Api();

        me.searchHandler = function (options, callBack) {
            me.api.search('kiosks', options).then(function (resp) {
                commonService.fireCallBack(callBack, resp);
            }, function (error) {
                commonService.fireCallBack(callBack, error);
            });
        }

        me.updateHandler = function (rowEntity, successCallBack, errorCallBack) {
            prepareEntity(rowEntity, function (resp) {
                rowEntity.long = resp.geometry.location.lng().toString();
                rowEntity.lat = resp.geometry.location.lat().toString();
                rowEntity.status = 'online';
                me.api.update('kiosks', rowEntity).then(function (resp) {
                    commonService.fireCallBack(successCallBack, rowEntity);
                }, function (error) {
                    commonService.fireCallBack(errorCallBack, error);
                });
            });
        }

        //PRIVATE METHOD
        var prepareEntity = function (rowEntity, callBack) {
            googleApiService.getLatLng(function (resp) {
                commonService.fireCallBack(callBack, resp);
            }, rowEntity.address);
        };

        return me;
    }
})();