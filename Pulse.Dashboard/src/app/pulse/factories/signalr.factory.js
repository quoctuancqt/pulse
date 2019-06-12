(function () {
    'use strict';

    angular
        .module('app.pulse')
        .factory('signalrFactory', signalrFactory);

    function signalrFactory(PUlSE_SETTINGS) {
        function SignalrInstance(isServer) {
            var signalrInstance = {};
            signalrInstance.hubProperties = initHubProperties(isServer);
            signalrInstance.startConnection = startConnection;
            signalrInstance.sendDeviceControl1 = sendDeviceControl1;
            signalrInstance.processPerfStatusMessageToKiosk = processPerfStatusMessageToKiosk;
            signalrInstance.processPerfStatusMessageToPulseServer = processPerfStatusMessageToPulseServer;
            signalrInstance.stopConnection = stopConnection;
            return {
                signalrInstance
            };
        };

        function initHubProperties(isServer) {
            var signalrOptions = {};
            signalrOptions.connection = $.hubConnection(PUlSE_SETTINGS.signalUri);
            signalrOptions.connection.logging = true;
            signalrOptions.connection.qs = { 'server': isServer };
            signalrOptions.proxy = signalrOptions.connection.createHubProxy(PUlSE_SETTINGS.hubName);
            signalrOptions.hub = signalrOptions.proxy.connection;
            signalrOptions.proxy.on("");
            return signalrOptions;
        }

        function startConnection(groupName, _connection, _hub, _proxy) {
            _connection.start().done(function () {
                _proxy.invoke("StartConnection", _hub.id,'Dashboard', groupName, '');
            });
        };

        function sendDeviceControl1(machineId, controlType, _connection, _proxy) {
            _connection.start().done(function () {
                _proxy.invoke("SendDeviceControl", machineId, controlType);
            });
        };


        function processPerfStatusMessageToKiosk(callback, _proxy) {
            _proxy.on("processPerfStatusMessageToKiosk", callback);
        };

        function processPerfStatusMessageToPulseServer(callback, _proxy) {
            _proxy.on("processPerfStatusMessageToPulseServer", callback);
        };

        function stopConnection(sirnalrObj) {
            sirnalrObj.hubProperties.hub.stop();
        };

        return {
            createNew: function (isServer) {
                return new SignalrInstance(isServer);
            }
        };
    }
})();
