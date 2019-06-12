(function () {
    'use strict';

    angular
        .module('app.pulse.password')
        .config(moduleConfig);

    function moduleConfig($stateProvider) {
        $stateProvider
        .state('triangular.password', {
            url: '/password-management',
            templateUrl: 'app/pulse/password_management/views/password.html',
            controller: 'PasswordCtrl',
            controllerAs: 'vm',
        })
    }
})();
