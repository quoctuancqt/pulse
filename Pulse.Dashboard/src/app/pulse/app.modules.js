(function() {
    'use strict';

    angular
        .module('app.pulse', [
            'app.pulse.pulse_server',
            'app.pulse.kiosks',
            'app.pulse.iot',
            'app.pulse.websites',
            'app.pulse.profile',
            'app.pulse.login',
            'app.pulse.wizard',
            'app.pulse.password',
            'ui.grid',
            'ui.grid.edit',
            'ui.grid.autoResize',
            'ui.grid.rowEdit',
            'ui.grid.pagination',
            'ui.grid.cellNav',
            'pulse.grid.common',
            'pulse.googleApi',
            'pulse.paging',
            'pulse.paging.service',
            'app.pulse.configuration',
        ]);
})();