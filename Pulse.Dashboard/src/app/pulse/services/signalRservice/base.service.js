(function () {
    'use strict';

    angular
        .module('app.pulse')
        .service('baseService', baseService);

    function baseService(PUlSE_SETTINGS, sessionFactory) {
        var me = this;
        me.data = []
        me.signalR = null, me.hub = null, me.systemName = "", me.groupName = "";
        me.clientMethod = {};

        me.constructor = function (isServer, systemName, groupName) {

            me.systemName = systemName;
            me.groupName = groupName;
            me.signalR = $.hubConnection(PUlSE_SETTINGS.signalUri);
            //me.signalR.logging = true;
            me.signalR.qs = {
                'server': isServer,
                'access_token': sessionFactory.get('access_token'),
                'refresh_token': sessionFactory.get('refresh_token'),
                'clientId': sessionFactory.get('clientId'),
                'groupName': me.groupName,
                'systemName': me.systemName,
                'machineId': '',
                'role': sessionFactory.get('role')
            };
            me.hub = me.signalR.createHubProxy(PUlSE_SETTINGS.hubName);
            me.hub.on("");

            return me;
        };

        me.startConnectionToPulseServer = function (callBack) {
            me.signalR.start().done(function () {
                callBack();
                me.hub.invoke("StartConnection");
            });
        };

        me.startConnectionToKiosk = function () {
            me.signalR.start().done(function () {
            });
        };

        me.stateChange = function (callBack) {
            me.signalR.stateChanged(callBack);
        }

        me.ProcessPerfStatusMessageToPulseServer = function (callBack) {
            me.hub.on("ProcessPerfStatusMessageToPulseServer", callBack);
        };

        me.processPerfStatusMessageToKiosk = function (callBack) {
            me.hub.on("processPerfStatusMessageToKiosk", callBack);
        };

        me.receiveNotifyToPulseServer = function (callBack) {
            me.hub.on("ReceiveNotifyToPulseServer", callBack);
        }
    };

})();
