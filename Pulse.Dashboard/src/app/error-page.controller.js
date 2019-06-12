(function() {
    angular
        .module('app')
        .controller('ErrorPageController', ErrorPageController);

    /* @ngInject */
    function ErrorPageController($state) {
        var vm = this;

        vm.goHome = goHome;

        /////////

        function goHome() {
            $state.go('triangular.dashboard-analytics');
        }
    }
})();
