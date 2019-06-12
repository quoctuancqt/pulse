(function () {
    'use strict';

    angular
        .module('pulse.dialog', [])
        .directive('messageBox', function () {
            return {
                restrict: 'EA',
                scope: {
                    title: '=modalTitle',
                    header: '=modalHeader',
                    headerLogo:'=modalHeaderLogo',
                    body: '=modalBody',
                    footer: '=modalFooter',
                    isConfirm: '=modalIsConfirm',
                    isSuccess : '=modalIsSuccess',
                    callbackbuttonleft: '&ngClickLeftButton',
                    callbackbuttonright: '&ngClickRightButton',
                    messageType: '=messageType',
                    icon: '=messageIcon',
                    handler: '=myHandler'
                },
                templateUrl: 'pulse.module/pulse.dialog/pulse-dialog-tmp.html',
                transclude: true,
            };
    });
})();