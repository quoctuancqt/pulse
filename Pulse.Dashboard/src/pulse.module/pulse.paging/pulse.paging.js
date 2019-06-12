(function () {
    'use strict';
    angular.module('pulse.paging', [])
    .directive('paging', function () {
        return {
            restrict: 'E',
            scope: {
                options: '=options'
            },
            templateUrl: 'pulse.module/pulse.paging/pulse.paging.tmp.html'
        }
    });
})();