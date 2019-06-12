(function () {
    'use strict';

    angular
        .module('pulse.authenticate', ['pulse.config'])
        .factory('authenticateFactory', authenticateFactory);

    function authenticateFactory($http, PUlSE_SETTINGS) {
        var me = this;

        me.getToken = function (userDto, successCallback, errorCallback) {
            $http.post(PUlSE_SETTINGS.apiUri + 'oauths', userDto).then(successCallback, errorCallback);
        };

        me.refreshToken = function (refressId, successCallback, errorCallback) {
            $http.get(PUlSE_SETTINGS.apiUri + 'oauths?refreshId=' + refressId).then(successCallback, errorCallback);
        };

        me.checkTokenIsExpired = function (successCallback, errorCallback) {
            $http.get(PUlSE_SETTINGS.apiUri + 'oauths/checktoken').then(successCallback, errorCallback);
        };

        return me;
    }
})();
