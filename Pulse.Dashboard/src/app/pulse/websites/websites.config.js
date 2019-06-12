(function() {
    'use strict';

    angular
        .module('app.pulse.websites')
        .config(moduleConfig);

    function moduleConfig($stateProvider, triMenuProvider) {
         $stateProvider
        .state('triangular.websites', {
            url: '/websites',
            templateUrl: 'app/pulse/websites/views/_websites_manage.tmp.html',
        });

        triMenuProvider.addMenu({
            name: 'Websites',
            id: 3,
            state: 'triangular.websites',
            type: 'link',
            icon: 'fa fa-edge',
         });
    }
})();
