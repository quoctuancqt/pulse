(function() {
    'use strict';

    angular
        .module('app')
        .controller('AppFooterController', FooterController);

    /* @ngInject */
    function FooterController(triLayout, triSettings) {
        var vm = this;

        vm.layout = triLayout;
        vm.settings = triSettings;
    }
})();
