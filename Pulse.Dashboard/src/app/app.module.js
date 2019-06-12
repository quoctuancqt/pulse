(function () {
    'use strict';

    angular
        .module('app', [
            'ui.router',
            'triangular',
            'ngAnimate', 'ngCookies', 'ngSanitize', 'ngMessages', 'ngMaterial',
            'googlechart', 'chart.js', 'linkify', 'ui.calendar', 'angularMoment', 'textAngular', 'uiGmapgoogle-maps', 'hljs', 'md.data.table', angularDragula(angular), 'ngFileUpload',
            'app.pulse', 'ng.epoch', 'n3-pie-chart', 'pulse.config', 'pulse.session', 'pulse.authenticate', 'pulse.dialog', 'ngLoadingSpinner', 'pulse.input.dialog', 'pulse.filter', 'ngAutocomplete',
        ])
        .config(function ($httpProvider) {
            $httpProvider.interceptors.push('httpRequestInterceptor');
        });
})();