(function () {
    'use strict';
    angular.module('app.pulse')
        .factory('validationFactory', function (PUlSE_SETTINGS) {
            return {
                checkRequired: function (value) {
                    if (value == null || typeof (value) == "undefined") {
                        return true;
                    }
                    return false;
                },
                
            };
        });
})();
