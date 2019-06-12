(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .controller('configurationDashboardCtrl', configurationDashboardCtrl);

    function configurationDashboardCtrl($state, $filter, PUlSE_SETTINGS, sessionFactory) {

        var vm = this;

        var securitiesData = $filter('filter')(PUlSE_SETTINGS.securitiesData, { roleName: sessionFactory.get('role') });

        vm.securities = securitiesData[0].settings.configurations;

        vm.go = function (key) {
            switch (key) {
                case 'kiosks': {
                    $state.go('triangular.configuration-kiosks');
                    break;
                }
                case 'groups': {
                    $state.go('triangular.configuration-groups');
                    break;
                }
                case 'licenses': {
                    $state.go('triangular.configuration-licenses');
                    break;
                }
                case 'users': {
                    $state.go('triangular.configuration-users');
                    break;
                }
                case 'client': {
                    $state.go('triangular.configuration-clients');
                    break;
                }
                default: {
                    $state.go('triangular.configuration-countries');
                    break;
                }
            }
        }
    }
})();