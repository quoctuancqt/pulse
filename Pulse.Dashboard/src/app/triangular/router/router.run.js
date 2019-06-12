(function() {
    'use strict';

    angular
        .module('triangular')
        .run(runFunction);

    /* @ngInject */
    function runFunction($rootScope, $window, $state, $filter, $timeout, $injector, triRoute, triBreadcrumbsService) {
        var breadcrumbs = triBreadcrumbsService.breadcrumbs;

        // change title when language changes - when a menu item is clicked - on app init
        var menuTitleHandler = $rootScope.$on('changeTitle', function(){
            setFullTitle();
        });

        $rootScope.$on('$destroy', function(){
            menuTitleHandler();
        });

        function setFullTitle() {
            $timeout(function(){
                var title = triRoute.title;
                angular.forEach(breadcrumbs.crumbs, function(crumb){
                    var name = crumb.name;
                    if($injector.has('translateFilter')) {
                        name = $filter('translate')(crumb.name);
                    }
                    title +=' ' + triRoute.separator + ' ' + name;
                });
                $window.document.title = title;
            });
        }
    }
})();
