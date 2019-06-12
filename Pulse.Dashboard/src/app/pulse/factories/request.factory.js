(function () {
    'use strict';
    angular.module('app')
        .factory('httpRequestInterceptor', httpRequestInterceptor);
    function httpRequestInterceptor(sessionFactory, $window, $q, $injector) {
        var isRefresh = false;
        var option = null;
        
        return {
            request: function (config) {

                if (config.url.indexOf(".html") == -1) {
                    config.headers['Authorization'] = 'bearer ' + (typeof (sessionFactory.get('access_token')) != "undefined" && sessionFactory.get('access_token') != null ? sessionFactory.get('access_token') : '');
                    config.headers['Content-Type'] = 'application/json';
                }
                return config;
            },
            responseError: function (rejection) {
                var authenticateFactory = $injector.get('authenticateFactory');
                if (rejection.status === 401 && isRefresh == false) {
                    isRefresh = true;
                    var refreshToken = sessionFactory.get('refresh_token');
                    if (rejection.config.method != "GET") {
                        option = rejection.config;
                    }
                    authenticateFactory.refreshToken(refreshToken, SuccessCallBack, ErrorCallBack);
                }
                else {
                    return $q.reject(rejection);
                }
            }
        };

        function RetryAjax() {
            if (option != null) {
                var http = $injector.get('$http');
                http(option);
                option == null;
            }

            $window.location.reload();

        };

        function SuccessCallBack(resp) {
            var timeout = $injector.get('$timeout');
            sessionFactory.set('access_token', null);
            sessionFactory.set('refresh_token', null);
            sessionFactory.set('access_token', resp.data.access_token);
            sessionFactory.set('refresh_token', resp.data.refresh_token);
            isRefresh = false;
            timeout(function () {
                RetryAjax();
            }, 1000);
        }

        function ErrorCallBack(error) {
            //console.log(error);
        }
    };
})();