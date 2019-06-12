(function () {
    'use strict';

    angular
        .module('triangular.components')
        .controller('RightSidenavController', RightSidenavController);

    /* @ngInject */
    function RightSidenavController($scope, $http, $mdSidenav, $state, PUlSE_SETTINGS, $rootScope, baseApi, $filter) {
        var vm = this;
        var api = baseApi.Api();
        // sets the current active tab
        vm.close = close;
        vm.currentTab = 0;
        vm.notificationGroups = [{
            name: 'Kiosk',
            notifications: []
        }];

        vm.go = function (groupName, machineId, status) {
            $.each(vm.notificationGroups, function (i, g) {
                if (g.name == groupName) {
                    $.each(g.notifications, function (j, n) {
                        if (n.machineId == machineId) {
                            g.notifications.splice(j, 1);
                        }
                    })
                }
            });
            close();
            $state.go('triangular.detail', { id: machineId });
        }

        $scope.$on('triSwitchNotificationTab', function ($event, tab) {
            vm.currentTab = tab;
        });

        vm.getNotify = function () {
            api.get('notify').then(function (resp) {
                $.each(resp.data.Items, function (i, v) {
                    var notificationItem = {
                        machineId: v.MachineId,
                        content: v.Content,
                        status: v.Status,
                        countDate: v.CountDate,
                        isRead : v.IsRead,
                        icon: 'zmdi zmdi-desktop-windows',
                        iconColor: '#55acee',
                    };
                    vm.notificationGroups[0].notifications.push(notificationItem);
                });
                var count = $filter('filter')(vm.notificationGroups[0].notifications, { isRead: false });
                $rootScope.$broadcast("notificationChange", { data: count.length });
            }, function (error) {
                console.log(error);
            });
        }

        vm.getNotify();

        function close() {
            $mdSidenav('notifications').close();
        }

        $rootScope.$on('pushNotification', function (event, args) {
            vm.notificationGroups[0].notifications.push(args.data);
            var count = $filter('filter')(vm.notificationGroups[0].notifications, { isRead: false });
            $rootScope.$broadcast("notificationChange", { data: count.length });
        });
    }
})();
