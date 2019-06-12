(function() {
    'use strict';

    angular
        .module('triangular.components')
        .factory('triBreadcrumbsService', BreadcrumbsService);

    /* @ngInject */
    function BreadcrumbsService($rootScope) {
        var crumbs = [];

        return {
            breadcrumbs: {
                crumbs: crumbs
            },
            addCrumb: addCrumb,
            reset: reset
        };

        ////////////////

        function addCrumb(item) {
            this.breadcrumbs.crumbs.unshift(item);
            $rootScope.$emit('changeTitle');
        }

        function reset() {
            this.breadcrumbs.crumbs = [];
        }
    }
})();
