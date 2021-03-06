(function() {
    'use strict';

    angular
        .module('triangular.components')
        .controller('ToolbarController', DefaultToolbarController);

    /* @ngInject */
    function DefaultToolbarController($scope, $injector, $rootScope, $mdMedia, $state, $element, $filter, $mdUtil, $mdSidenav, $mdToast, $timeout, $document, triBreadcrumbsService, triSettings, triLayout, triSkins, $cookies, $window, sessionFactory, baseApi) {
        var vm = this;
        var api = baseApi.Api();
        vm.breadcrumbs = triBreadcrumbsService.breadcrumbs;
        vm.emailNew = false;
        vm.languages = triSettings.languages;
        vm.openSideNav = openSideNav;
        vm.hideMenuButton = hideMenuButton;
        vm.switchLanguage = switchLanguage;
        vm.toggleNotificationsTab = toggleNotificationsTab;
        vm.isFullScreen = false;
        vm.fullScreenIcon = 'zmdi zmdi-fullscreen';
        vm.toggleFullScreen = toggleFullScreen;
        vm.notificationCount = 0;
        //change dark mode
        vm.changeDarkMode = changeDarkMode;
        vm.isDarkMode = $scope.isDarkMode;
        vm.currentSkin = triSkins.getCurrent();

        if(vm.currentSkin.id !== 'dark-knight'){
            vm.isDarkMode = false;
        }        
        else{
            vm.isDarkMode=true;
        }

        function changeDarkMode(){
            if(vm.currentSkin.id !== 'dark-knight') {
                $cookies.put('triangular-skin',angular.toJson({
                    skin: 'dark-knight'
                }));
            }
            else{
                $cookies.put('triangular-skin',angular.toJson({
                    skin: 'cyan-cloud'
                }));
            }
            $window.location.reload();
        }

        //toggle search
        vm.showSearch = false;
        vm.toggleSearch = toggleSearch;

        function toggleSearch() {
            vm.showSearch = !vm.showSearch;
        }

        vm.currentUser = {
            fullName: sessionFactory.get('fullName'),
            avatar: sessionFactory.get('avatar')
        }

        vm.logout = function () {
            api.customPost('oauths', 'logout', { token: sessionFactory.get('access_token') }).then(function (resp) {
                sessionFactory.clearAll();
                $state.go('authentication.login');
            });
        }

        function openSideNav(navID) {
            $mdUtil.debounce(function(){
                $mdSidenav(navID).toggle();
            }, 300)();
        }

        function switchLanguage(languageCode) {
            if($injector.has('$translate')) {
                var $translate = $injector.get('$translate');
                $translate.use(languageCode)
                .then(function() {
                    $mdToast.show(
                        $mdToast.simple()
                        .content($filter('triTranslate')('Language Changed'))
                        .position('bottom right')
                        .hideDelay(500)
                    );
                    $rootScope.$emit('changeTitle');
                });
            }
        }

        function hideMenuButton() {
            switch(triLayout.layout.sideMenuSize) {
                case 'hidden':
                    // always show button if menu is hidden
                    return false;
                case 'off':
                    // never show button if menu is turned off
                    return true;
                default:
                    // show the menu button when screen is mobile and menu is hidden
                    return $mdMedia('gt-sm');
            }
        }

        function toggleNotificationsTab(tab) {
            $rootScope.$broadcast('triSwitchNotificationTab', tab);
            vm.openSideNav('notifications');
            api.update('notify', null).then(function (resp) {
                vm.notificationCount = 0;
                $state.reload();
            }, function (error) {
                console.log(error);
            });

        }

        function toggleFullScreen() {
            vm.isFullScreen = !vm.isFullScreen;
            vm.fullScreenIcon = vm.isFullScreen ? 'zmdi zmdi-fullscreen-exit':'zmdi zmdi-fullscreen';
            // more info here: https://developer.mozilla.org/en-US/docs/Web/API/Fullscreen_API
            var doc = $document[0];
            if (!doc.fullscreenElement && !doc.mozFullScreenElement && !doc.webkitFullscreenElement && !doc.msFullscreenElement ) {
                if (doc.documentElement.requestFullscreen) {
                    doc.documentElement.requestFullscreen();
                } else if (doc.documentElement.msRequestFullscreen) {
                    doc.documentElement.msRequestFullscreen();
                } else if (doc.documentElement.mozRequestFullScreen) {
                    doc.documentElement.mozRequestFullScreen();
                } else if (doc.documentElement.webkitRequestFullscreen) {
                    doc.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
                }
            } else {
                if (doc.exitFullscreen) {
                    doc.exitFullscreen();
                } else if (doc.msExitFullscreen) {
                    doc.msExitFullscreen();
                } else if (doc.mozCancelFullScreen) {
                    doc.mozCancelFullScreen();
                } else if (doc.webkitExitFullscreen) {
                    doc.webkitExitFullscreen();
                }
            }
        }

        $scope.$on('newMailNotification', function(){
            vm.emailNew = true;
        });

        $rootScope.$on('notificationChange', function (event, args) {
            vm.notificationCount = args.data;
        });
    }
})();
