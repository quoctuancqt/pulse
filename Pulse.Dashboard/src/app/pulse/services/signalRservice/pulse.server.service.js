(function () {
    'use strict';

    angular
        .module('app.pulse')
        .service('pulseServerService', pulseServerService);

    function pulseServerService($state, $timeout, baseService, collectorFactory, PUlSE_SETTINGS, notificationFactory, $rootScope, baseApi, helperFactory, sessionFactory) {

        var me = this;

        var api = baseApi.Api();

        var obj = Object.create(baseService);

        var groupName = PUlSE_SETTINGS.serverGroupName + "#" + sessionFactory.get('clientId');

        me.pulseServer = obj.constructor(true, "Dashboard", groupName);

        me.pulseServer.data = null;

        var isStart = false;
        me.pulseServer.isCallStartConnection = false;
        me.pulseServer.init = function () {
            me.pulseServer.data = collectorFactory.initPulseViewModel({
                chartPieData: {
                    'labels': ['Error', 'Information', 'Warning'],
                    'data': []
                }
            });
            isStart = true;
            getUserActivities();
            getSystemEvents();
        };

        me.pulseServer.startConnectionToPulseServer(function () {
            me.pulseServer.isCallStartConnection = true;
        });

        me.pulseServer.reconnect = function (callBack) {
            me.pulseServer.stateChange(callBack);
        };

        me.pulseServer.ProcessPerfStatusMessageToPulseServer(function (json) {
            if ($state.current.controller == "pulseServerCtrl" && isStart) {
                var obj = JSON.parse(json);
                me.pulseServer.data = collectorFactory.dataBinding(obj, me.pulseServer.data);
            } else {
                isStart = false;
            }
        });

        me.pulseServer.receiveNotifyToPulseServer(function (json) {
            var obj = JSON.parse(json);
            notificationFactory.show(obj.content);
            var notificationItem = {
                machineId: obj.machineId,
                content: obj.content,
                status: obj.status,
                countDate: obj.countDate,
                isRead: obj.isRead == "true" ? true : false,
                icon: 'zmdi zmdi-desktop-windows',
                iconColor: '#55acee',
            };
            $rootScope.$broadcast("pushNotification", { data: notificationItem });
            $state.reload();
        });

        function getUserActivities() {
            api.get('useractivities').then(function (resp) {
                $.each(resp.data, function (i, v) {
                    var item = { timeLog: helperFactory.checkUndefined(v.CountDate) + ' ago', contentLog: helperFactory.checkUndefined(v.Name) + ' ' + helperFactory.checkUndefined(v.ActionName) };
                    me.pulseServer.data.user_activities_data.push(item);
                });
            });
        }

        function getSystemEvents() {
            api.get('systemevents').then(function (resp) {
                $.each(resp.data, function (i, v) {
                    var item = { timeLog: helperFactory.checkUndefined(v.CountDate) + ' ago', contentLog: helperFactory.checkUndefined(v.MachineName) + ' ' + helperFactory.checkUndefined(v.ActionName) };
                    me.pulseServer.data.system_event_data.push(item);
                });
            });
        }
    }
})();
