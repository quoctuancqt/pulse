(function () {
    'use strict';

    angular
        .module('app.pulse')
        .directive('performanceGraphWidget', performanceGraphWidget);

    function performanceGraphWidget($timeout, $interval) {
        var directive = {
            templateUrl: 'app/pulse/directives/performance-graph/performance-graph-tmp.html',
            scope: {
                data: '=setting'
            },
            restrict: 'E',
            link: function (scope, elem, attr) {
                scope.settings = {
                    cpu: {
                        dataLength: 50,
                        data: [[]],
                        labels: [],
                        colours: ['#DB4437']
                    },
                    bandwidth: {
                        dataLength: 50,
                        data: [[]],
                        labels: [],
                        colours: ['#4285F4']
                    },
                    memory: {
                        dataLength: 50,
                        data: [[]],
                        labels: [],
                        colours: ['#9b59b6']
                    }
                };
                
                scope.chart = {
                    options: {
                        animation: false,
                        showTooltips: false,
                        pointDot: false,
                        datasetStrokeWidth: 0.5,
                        maintainAspectRatio: false
                    },
                };
                var interval = null;
                scope.$watch('data', function (newValue, oldValue) {
                    if (newValue) {
                        if (interval != null) $interval.cancel(interval);
                        interval = $interval(function () {
                            scope.settings = newValue;
                        }, 500);
                    }
                }, true);
            }
        };

        return directive;
    }
})();