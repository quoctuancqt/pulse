(function () {
    'use strict';

    angular.module('app.pulse')
           .factory('helperFactory', helperFactory);

    function helperFactory() {
        var helper = {};

        helper.checkUndefined= function(val, defaultVal) {
            return (typeof (val) == "undefined" || val == null) ? (typeof (defaultVal) == "undefined" || defaultVal == null ? "" : defaultVal) : val;
        }
        
        return helper;
    }
})();