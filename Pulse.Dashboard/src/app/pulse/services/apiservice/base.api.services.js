(function () {
    'use strict';
    angular.module('app.pulse')
        .service('baseApi', function (PUlSE_SETTINGS, $http, helperFactory) {
            var me = this;
            var apiUrl = PUlSE_SETTINGS.apiUri;
            //PRIVATE METHOD
            var $$resolveServiceEndpoint = function (route_prefix, route) {
                return apiUrl + route_prefix + '/' + helperFactory.checkUndefined(route);
            }
            //Api Obiject
            me.Api = function () {
                return {
                    get: function (route_prefix) {
                        return $http.get($$resolveServiceEndpoint(route_prefix));
                    },
                    search: function (route_prefix, params) {
                        return $http.get($$resolveServiceEndpoint(route_prefix, 'search'), { params: params });
                    },
                    create: function (route_prefix, params) {
                        return $http.post($$resolveServiceEndpoint(route_prefix), params);
                    },
                    update: function (route_prefix, params) {
                        return $http.put($$resolveServiceEndpoint(route_prefix), params);
                    },
                    remove: function (route_prefix, params) {
                        return $http.delete($$resolveServiceEndpoint(route_prefix), { data: params });
                    },
                    customGet: function (route_prefix, route, params) {
                        return $http.get($$resolveServiceEndpoint(route_prefix, route), { params: params });
                    },
                    customPost: function (route_prefix, route, params) {
                        return $http.post($$resolveServiceEndpoint(route_prefix, route), params);
                    },
                    customPut: function (route_prefix, route, params) {
                        return $http.put($$resolveServiceEndpoint(route_prefix, route), params);
                    },
                    customDelete: function (route_prefix, route, params) {
                        return $http.post($$resolveServiceEndpoint(route_prefix, route), { data: params });
                    }
                }
            }
        });
})();
