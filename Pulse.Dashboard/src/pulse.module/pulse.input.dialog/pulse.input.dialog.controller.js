(function () {
    'use strict';

    angular
        .module('pulse.input.dialog', ['ui.bootstrap'])
        .directive('inputForm', function () {
            return {
                restrict: 'EA',
                scope: {
                    header: '=modalHeader',
                    headerLogo: '=modalHeaderLogo',
                    bodyContent: '=modalBodyContent',
                    messageType: '=modalMessageType',
                    options: '=options',
                    popupType: '=modalType',
                    callbackSubmit: '&ngClickSubmitButton',
                    callbackCancel: '&ngClickCancelButton',
                    handler: '=myHandler'
                },
                templateUrl: 'pulse.module/pulse.input.dialog/pulse.input.dialog.tmp.html',
            };
        });
})();