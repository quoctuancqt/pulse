(function () {
    'use strict';

    angular
        .module('app.pulse.kiosks')
        .config(moduleConfig);

    function moduleConfig($stateProvider, triMenuProvider) {
        $stateProvider
            .state('triangular.kiosk', {
                url: '/kiosks',
                templateUrl: 'app/pulse/kiosks/views/Dashboard/kiosks_manage.tmp.html',
                controller: 'kiosksDashboardCtrl',
                controllerAs: 'vm',
            }).state('triangular.detail', {
                url: '/detail/:id',
                templateUrl: 'app/pulse/kiosks/views/Detail/kiosks_detail.html',
                controller: 'kiosksDetailCtrl',
                controllerAs: 'vm'
            });

        triMenuProvider.addMenu({
            name: 'Kiosks',
            id: 1,
            state: 'triangular.kiosk',
            icon: 'zmdi zmdi-desktop-windows',
            type: 'link',
            action: '#kiosks'
        });
    }
})();
