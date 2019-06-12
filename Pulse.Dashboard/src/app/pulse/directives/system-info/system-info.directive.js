(function () {
    'use strict';

    angular
        .module('app.pulse')
        .directive('systemInfoWidget', systemInfoWidget);

    function systemInfoWidget($timeout, $interval) {
        var directive = {
            templateUrl: 'app/pulse/directives/system-info/system-info-tmp.html',
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