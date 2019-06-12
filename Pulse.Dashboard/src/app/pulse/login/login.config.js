(function () {
    'use strict';

    angular
        .module('app.pulse.login')
        .config(moduleConfig);

    function moduleConfig($stateProvider, triMenuProvider) {
        $stateProvider
        .state('authentication', {
            abstract: true,
            views: {
                'root': {
                    templateUrl: 'app/pulse/login/views/authentication.tmpl.html'
                }
            }
        }).state('authentication.login', {
            url: '/login',
            templateUrl: 'app/pulse/login/views/login.html',
            controller: 'LoginCtrl',
            controllerAs: 'vm',
        }).state('authentication.wizard', {
            url: '/wizard-configuration',
            templateUrl: 'app/pulse/login/views/profileWizard.html',
            controller: 'WizardCtrl',
            controllerAs: 'vm',
        });
    }
})();
