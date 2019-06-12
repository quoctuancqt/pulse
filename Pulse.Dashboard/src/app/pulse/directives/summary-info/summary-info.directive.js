(function () {
    'use strict';

    angular
        .module('app.pulse')
        .directive('summaryInfoWidget', summaryInfoWidget);

    function summaryInfoWidget() {
        var directive = {
            templateUrl: 'app/pulse/directives/summary-info/summary-info-tmp.html',
            scope: {
                data: '=setting'
            },
            restrict: 'E',
            link: function (scope, elem, attr) {
                scope.settings = scope.data;
            }
        };
        return directive;
    }
})();