(function() {
    'use strict';

    angular
        .module('app')
        .config(translateConfig);

    /* @ngInject */
    function translateConfig(triSettingsProvider, triRouteProvider) {
        var now = new Date();
        // set app name & logo (used in loader, sidemenu, footer, login pages, etc)
        triSettingsProvider.setName('PULSE');
        triSettingsProvider.setCopyright('&copy;TEKCENT ' + now.getFullYear());
        triSettingsProvider.setLogo('assets/images/logo.png');
        // set current version of app (shown in footer)
        triSettingsProvider.setVersion('0.0.2');
        // set the document title that appears on the browser tab
        triRouteProvider.setTitle('PULSE - TEKCENT');
        triRouteProvider.setSeparator('|');
    }
})();
