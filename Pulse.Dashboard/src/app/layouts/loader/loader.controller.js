(function() {
    'use strict';

    angular
        .module('app')
        .controller('LoaderController', LoaderController);

    /* @ngInject */
    function LoaderController(triSettings) {
        var vm = this;
        vm.triSettings = triSettings;
    }
})();
