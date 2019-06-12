(function () {
    'use strict';

    angular
        .module('app.pulse')
        .directive('gaugeChartWidget', gaugeChartWidget);

    function gaugeChartWidget() {
        var directive = {
            templateUrl: 'app/pulse/directives/gauge-chart/gauge-chart-tmp.html',
            scope: {
                data: '=setting'
            },
            restrict: 'E',
            link: function (scope, elem, attr) {
                scope.options = { thickness: 10, mode: "gauge", total: 100 };
                scope.datas = scope.data;
            }
        };
        return directive;
    }
})();