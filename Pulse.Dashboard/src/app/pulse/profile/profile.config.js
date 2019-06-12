(function () {
    'use strict';

    angular
        .module('app.pulse.profile')
        .config(moduleConfig);

    function moduleConfig($stateProvider) {
        $stateProvider
        .state('triangular.profile', {
            url: '/profile',
            templateUrl: 'app/pulse/profile/views/profile.html',
            controller: 'ProfileCtrl',
            controllerAs: 'vm',
        })
    }
})();
