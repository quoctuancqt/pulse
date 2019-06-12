(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .service('configurationClientsService', configurationClientsService)

    function configurationClientsService(baseApi, commonService) {
        var me = this;

        me.api = baseApi.Api();

        me.searchHandler = function (options, callBack) {
            me.api.search('clients', options).then(function (resp) {
                commonService.fireCallBack(callBack, resp);
            }, function (error) {
                commonService.fireCallBack(callBack, error);
            });
        }

        me.getHandler = function (clientId, successCallBack, errorCallBack) {
            me.api.customGet('clients', 'byclientid/' + clientId, null).then(function (resp) {
                commonService.fireCallBack(successCallBack, resp);
            }, function (error) {
                commonService.fireCallBack(errorCallBack, error);
            }
        )}

        me.createHandler = function (entity, successCallBack, errorCallBack) {
            me.api.create('clients', entity).then(function (resp) {
                commonService.fireCallBack(successCallBack, resp);
            }, function (error) {
                commonService.fireCallBack(errorCallBack, error);
            });
        }

        me.updateHandler = function (rowEntity, successCallBack, errorCallBack) {
            me.api.update('clients', rowEntity).then(function (resp) {
                commonService.fireCallBack(successCallBack, resp);
            }, function (error) {
                commonService.fireCallBack(errorCallBack, error);
            });
        }

        me.deleteHandler = function (rowEntity, callBack) {
            me.api.remove('clients', rowEntity).then(function (resp) {
                commonService.fireCallBack(callBack, resp);
            }, function (error) {
                commonService.fireCallBack(callBack, error);
            });
        }

        return me;
    }
})();