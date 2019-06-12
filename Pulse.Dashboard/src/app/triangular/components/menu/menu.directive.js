(function() {
    'use strict';

    angular
        .module('triangular.components')
        .directive('triMenu', triMenuDirective);

    /* @ngInject */
    function triMenuDirective($location, $mdTheming, triTheming) {
        var directive = {
            restrict: 'E',
            template: '<md-content><tri-menu-item permission permission-only="item.permission" ng-repeat="item in triMenuController.menu | orderBy:\'priority\'" item="::item"></tri-menu-item></md-content>',
            scope: {},
            controller: triMenuController,
            controllerAs: 'triMenuController',
            link: link
        };
        return directive;

        function link($scope, $element) {
            $mdTheming($element);
            var $mdTheme = $element.controller('mdTheme'); //eslint-disable-line

            var menuColor = triTheming.getThemeHue($mdTheme.$mdTheme, 'primary', 'default');
            var menuColorRGBA = triTheming.rgba(menuColor.value);
            $element.css({ 'background-color': "#009ecf" });
            $element.children('md-content').css({ 'background-color': "#009ecf" });
        }
    }

    /* @ngInject */
    function triMenuController(triMenu) {
        var triMenuController = this;
        // get the menu and order it
        triMenuController.menu = triMenu.menu;
    }
})();
