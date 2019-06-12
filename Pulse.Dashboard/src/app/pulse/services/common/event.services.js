(function () {
    'use strict';

    angular
        .module('app.pulse')
        .service('eventService', eventService);

    function eventService($rootScope) {

        var me = this;

        me.broadcast = function (eventName, args) {
            $rootScope.$broadcast(eventName, args);
        };

        me.listen = function (handler) {
            $rootScope.$on(handler, function (event, args) {
                args.stopConnection(args);
                console.log(args);
                $rootScope.$$listeners[event.name] = [];
            });
        };
    }
})();
