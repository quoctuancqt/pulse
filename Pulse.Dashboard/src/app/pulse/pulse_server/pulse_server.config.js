(function() {
    'use strict';

    angular
        .module('app.pulse.pulse_server')
        .config(moduleConfig);

    function moduleConfig($stateProvider, triMenuProvider) {
        $stateProvider
        .state('triangular.sales-layout', {
            url: '/pulse_server',
            templateUrl: 'app/pulse/pulse_server/views/_pulse_server.tmpl.html',
            controller: 'pulseServerCtrl',
            controllerAs: 'vm',
        });

        triMenuProvider.addMenu({
            name: 'Pulse Server',
            id: 0,
            state: 'triangular.sales-layout',
            type: 'link',
            icon: 'fa fa-line-chart',
            priority: 1.1,
        });
    }
})();
