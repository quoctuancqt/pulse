(function () {
    'use strict';

    angular
        .module('pulse.grid.common',[])
        .factory('uiGridFactory', uiGridFactory);

    function uiGridFactory() {

        var me = this;
        
        me.getTableHeight = function (gridOptions) {
            var rowHeight = 30;
            var headerHeight = 30;
            return {
                height: (gridOptions.data.length * rowHeight + headerHeight) + "px"
            }
        }

        return me;
    }
})();