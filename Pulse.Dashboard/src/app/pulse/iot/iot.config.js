(function() {
    'use strict';

    angular
        .module('app.pulse.iot')
        .config(moduleConfig);

    function moduleConfig($stateProvider, triMenuProvider) {
         $stateProvider
        .state('triangular.iot', {
            url: '/iot',
            templateUrl: 'app/pulse/iot/views/_iot_manage.tmp.html',
        });

        triMenuProvider.addMenu({
            name: 'IoT',
            id: 2,
            state: 'triangular.iot',
            type: 'link',
            icon: 'zmdi zmdi-device-hub',
        });
    }
})();
