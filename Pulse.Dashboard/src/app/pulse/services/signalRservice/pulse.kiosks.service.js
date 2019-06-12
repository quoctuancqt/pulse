(function () {
    'use strict';

    angular
        .module('app.pulse')
        .service('pulseKiosksService', pulseKiosksService);

    function pulseKiosksService($rootScope, baseService, PUlSE_SETTINGS) {

        var me = this;

        var obj = Object.create(baseService);

        me.pulsekiosk = obj.constructor(false, "Dashboard", PUlSE_SETTINGS.kioskGroupName);

        me.pulsekiosk.kiosksData = [];

        if (typeof ($rootScope.isKioskStart) == "undefined") {
            me.pulsekiosk.startConnectionToKiosk();
        }

        me.pulsekiosk.clearEvent = function () {
            $rootScope.$$listeners["shareDataKiosks"] = [];
        };

        me.pulsekiosk.SendDeviceControl = function (machineId, controlType) {
            me.pulsekiosk.hub.invoke("SendDeviceControl", machineId, controlType);
        };

        me.pulsekiosk.SendUpdateUserData = function (connectionId, systemName) {
            me.pulsekiosk.hub.invoke("SendUpdateUserData", connectionId, systemName);
        };
    }
})();
