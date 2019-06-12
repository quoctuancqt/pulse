(function () {
    'use strict';

    angular
        .module('app.pulse')
        .directive('userActivitiesWidget', userActivitiesWidget);

    function userActivitiesWidget($timeout, $interval, triSkins) {
        var directive = {
            templateUrl: 'app/pulse/directives/user-activities/user-activities-tmp.html',
            scope: {
                data: '=setting'
            },
            restrict: 'E',
            link: function (scope, elem, attr) {
                scope.array = scope.data;
                //set background-color for dark theme
                if (triSkins.getCurrent().id == 'dark-knight') {
                    elem[0].children[0].className += " dark-theme-background";
                    elem[0].children[0].lastElementChild.lastElementChild.className = "user-activities-dark"
                }
                scope.currentDate = new Date();
            }
        };
        return directive;
    }
})();