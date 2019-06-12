(function () {
    'use strict';
    angular.module('app.pulse')
        .factory('notificationFactory', function (PUlSE_SETTINGS, $http, $mdToast) {
            return {
                show: function (message, potition, timeToHide) {
                    $mdToast.show({
                        template: '<md-toast><span flex>' + message + '</span></md-toast>',
                        position: typeof (postition) == "undefined" || potition == null ? 'bottom right' : position,
                        hideDelay: typeof (timeToHide) == "undefined" || timeToHide == null ? 3000 : timeToHide,
                    });
                }
            };
        });
})();
