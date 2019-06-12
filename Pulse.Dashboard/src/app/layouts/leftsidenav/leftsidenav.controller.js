(function () {
    'use strict';

    angular
        .module('triangular.components')
        .controller('LeftSidenavController', LeftSidenavController);

    function LeftSidenavController(triSettings, triLayout, triMenu, menuService) {

        var vm = this;
        vm.layout = triLayout.layout;
        vm.sidebarInfo = {
            appName: triSettings.name,
            appLogo: triSettings.logo
        };
        vm.toggleIconMenu = toggleIconMenu;

        function toggleIconMenu() {
            var menu = vm.layout.sideMenuSize === 'icon' ? 'full' : 'icon';
            triLayout.setOption('sideMenuSize', menu);
        };

        //menuService.getMenu(function (data) {
        //    var kiosks = triMenu.getMenu(1);
        //    kiosks.children = data;
        //});
    }
})();
