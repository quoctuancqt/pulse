(function () {
    'use strict';

    angular
        .module('pulse.session', [])
        .factory('sessionFactory', SessionFactory);

    function SessionFactory($window) {
        var me = this;

        me.set = function (key, value) {
            $window.sessionStorage.setItem(key, value);
        }

        me.get = function (key) {
            return $window.sessionStorage.getItem(key);
        }

        me.remove = function (key) {
            $window.sessionStorage.removeItem(key);
        }

        me.clearAll = function () {
            $window.sessionStorage.clear();
        }

        return me;
    }
})();