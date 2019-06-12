(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .config(moduleConfig);

    function moduleConfig($stateProvider, triMenuProvider) {
        $stateProvider
       .state('triangular.configuration', {
           url: '/configuration',
           templateUrl: '/app/pulse/configuration/views/configuration_manage.tmp.html',
           controller: 'configurationDashboardCtrl',
           controllerAs: 'vm',
       })
       .state('triangular.configuration-countries', {
           url: '/configuration-countries',
           templateUrl: 'app/pulse/configuration/views/_configuration_countries.html',
           controller: 'configurationCountriesCtrl',
           controllerAs: 'vm',
       })
       .state('triangular.configuration-kiosks', {
           url: '/configuration-kiosks',
           templateUrl: 'app/pulse/configuration/views/_configuration_kiosks.html',
           controller: 'configurationKiosksCtrl',
           controllerAs: 'vm',
       })
       .state('triangular.configuration-groups', {
           url: '/configuration-groups',
           templateUrl: 'app/pulse/configuration/views/_configuration_groups.html',
           controller: 'configurationGroupsCtrl',
           controllerAs: 'vm',
       })
       .state('triangular.configuration-licenses', {
           url: '/configuration-licenses',
           templateUrl: 'app/pulse/configuration/views/_configuration_licenses.html',
           controller: 'configurationLicensesCtrl',
           controllerAs: 'vm',
       })
        .state('triangular.configuration-users', {
            url: '/configuration-users',
            templateUrl: 'app/pulse/configuration/views/_configuration_users.html',
            controller: 'configurationUsersCtrl',
            controllerAs: 'vm',
        }).state('triangular.configuration-clients', {
            url: '/configuration-clients',
            templateUrl: 'app/pulse/configuration/views/_configuration_clients.html',
            controller: 'configurationClientsCtrl',
            controllerAs: 'vm',
        }).state('triangular.configuration-client', {
            url: '/configuration-client/:mode/:id',
            templateUrl: 'app/pulse/configuration/views/_configuration_clients_form.html',
            controller: 'configurationClientsFormCtrl',
            controllerAs: 'vm',
        });

        triMenuProvider.addMenu({
            name: 'Configuration',
            id: 4,
            state: 'triangular.configuration',
            type: 'link',
            icon: 'fa fa-cogs',
        });
    }
})();
