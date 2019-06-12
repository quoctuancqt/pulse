(function () {
    'use strict';

    angular
        .module('app.pulse')
        .service('menuService', menuService);

    function menuService(baseApi) {
        var me = this;
        
        me.getMenu = function (callBack) {
            baseApi.Api().search('countries').then(function (data) {
                var menu = [];

                $.each(data.data.Items, function (i, item) {
                    menu.push(me.mapItemMenu(item));
                });

                callBack(menu);
            }, function (error) {

            });
        };

        me.mapItemMenu = function (item) {
            return {
                name: item.Name,
                state: 'triangular.kiosk',
                type: 'dropdown',
                children: me.mapItemSubMenu(item.Kiosks)
            };
        };

        me.mapItemSubMenu = function (data) {
            var children = [];
            $.each(data, function (i, item) {
                if (item.Status != 0) {
                    children.push({
                        name: item.Name,
                        state: 'triangular.detail',
                        type: 'link',
                        params: {
                            id: item.MachineId
                        }
                    });
                }
            });
            return children;
        };
    }
})();
