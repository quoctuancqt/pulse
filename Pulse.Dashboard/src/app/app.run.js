(function () {
    'use strict';

    angular
        .module('app')
        .run(runFunction);

    function runFunction($rootScope, $state,$window, sessionFactory, authenticateFactory, $rootElement, PUlSE_SETTINGS) {

        var access_token = sessionFactory.get('access_token');
        var refresh_token = sessionFactory.get('refresh_token');

        var currentState = $window.location.hash;

        var urlOrigin = $window.location.origin;
        PUlSE_SETTINGS.defaultAvatar = urlOrigin + PUlSE_SETTINGS.defaultAvatar;
        PUlSE_SETTINGS.defaultUrlMap = urlOrigin + PUlSE_SETTINGS.defaultUrlMap;
        PUlSE_SETTINGS.kioskGroupName = sessionFactory.get('kioskGroupName');
        if (access_token == "undefined" || access_token == null) {
            if (currentState != "" && currentState!='#/login') {
                $window.location = '/';
            }
        }

        if (access_token != "undefined" && access_token != null && access_token != "null") {
            authenticateFactory.checkTokenIsExpired(function (resp) {

            }, function () {
                updateToken();
            });
        }
        
        function updateToken() {
            authenticateFactory.refreshToken(refresh_token, function (resp) {
                sessionFactory.set('access_token', null);
                sessionFactory.set('refresh_token', null);
                sessionFactory.set('access_token', resp.data.access_token);
                sessionFactory.set('refresh_token', resp.data.refresh_token);
                $state.reload();
            }, function () {
                console.log('function RefreshToken error');
            });
        }

        // default redirect if access is denied
        function redirectError() {
            $state.go('500');
        }

        // redirect all errors to permissions to 500
        var errorHandle = $rootScope.$on('$stateChangeError', redirectError);

        // remove watch on destroy
        $rootScope.$on('$destroy', function () {
            errorHandle();
        });

        
    }
})();
