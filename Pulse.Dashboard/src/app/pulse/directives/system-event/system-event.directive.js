(function () {
    'use strict';

    angular
        .module('app.pulse')
        .directive('systemEventWidget', systemEventWidget);

    function systemEventWidget($timeout, $interval, triSkins) {
        var directive = {
            templateUrl: 'app/pulse/directives/system-event/system-event-tmp.html',
            scope: {
                data: '=setting'
            },
            restrict: 'E',
            link: function (scope, elem, attr) {
                scope.array = scope.data;
                //set background-color for dark theme
                if (triSkins.getCurrent().id == 'dark-knight') {
                    elem[0].children[0].className += " dark-theme-background";
                }
                scope.currentDate = new Date();
            }
        };
        return directive;
    }
})();