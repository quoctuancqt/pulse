(function () {
    'use strict';

    angular
        .module('app.pulse')
        .service('settingsService', settingsService);

    function settingsService(triSettings) {
        var me = this;

        me.getInputFormSettings = function () {
            return {
                header: '',
                headerLogo: triSettings.logo,
                bodyContent: '',
                messageType: 'pulse-text-default',
                options: [],
                popupType: 'alert',
                callbackSubmit: null,
                callbackCancel: null,
                handler: ''
            }
        }

        me.getDialogSettings = function () {
            return {
                header: '',
                headerLogo: triSettings.logo,
                body: '',
                footer: '',
                isConfirm: false,
                handler: '',
                messageType: 'text-info',
                icon: 'fa fa-info-circle',
                myRightButton: null
            }
        }

        return me;
    }
})();
