(function () {
    'use strict';

    angular
        .module('pulse.config', [])
        .constant('PUlSE_SETTINGS', {
            //'signalUri': 'http://cloud.tekcent.com:9090',
            'signalUri': 'http://192.168.1.65:9090',
            'apiUri': 'http://192.168.1.65:5600/api/',
            'defaultAvatar': '/assets/images/avatars/default.png',
            'defaultUrlMap': '/assets/images/maps/',
            'hubName': 'PulseHub',
            'googleApiKey': 'AIzaSyD3yipyAvVQqN4ZzgR_wM_aNnu09k7AxI8',
            'pageSize': 6,
            'dateFormat': 'date:\'yyyy-MM-dd\'',
            'gridButtonTmp': '<div class=\'grid-button-style\'><md-button class="md-default md-warn" aria-label="flat button" ng-click=\'grid.appScope.vm.editRow(row.entity)\'">Edit</md-button>' +
            '<md-button class="md-default md-warn" aria-label="flat button" ng-disabled=\'row.entity.Kiosks>0\' ng-click=\'grid.appScope.vm.deleteRow(row.entity)\'">Delete</md-button></div>',
            'editButtonTmp': '<div class=\'grid-button-style\'><md-button class="md-default md-warn" aria-label="flat button" ng-click=\'grid.appScope.vm.editRow(row.entity)\'">Edit</md-button></div>',
            'deleteButtonTmp': '<div class=\'grid-button-style\'><md-button class="md-default md-warn" aria-label="flat button" ng-click=\'grid.appScope.vm.deleteRow(row.entity)\'">Delete</md-button></div>',
            'kioskGroupName': '',
            'serverGroupName': 'PulseServer',
            //'mongoPrefix': 'mongodb://',
            'securitiesData': [{
                "roleName": "Administrator",
                "settings": {
                    "menus": {
                        "Pulse Server": true,
                        "Kiosks": true,
                        "IoT": false,
                        "Websites": false,
                        "Configuration": true
                    },
                    "configurations": {
                        "countries": true,
                        "kiosks": true,
                        "groups": false,
                        "licenses": true,
                        "users": true,
                        "clients": true
                    }
                }
            }, {
                "roleName": "ClientAdmin",
                "settings": {
                    "menus": {
                        "Pulse Server": false,
                        "Kiosks": true,
                        "IoT": false,
                        "Websites": false,
                        "Configuration": true
                    },
                    "configurations": {
                        "countries": true,
                        "kiosks": true,
                        "groups": false,
                        "licenses": true,
                        "users": true,
                        "clients": false
                    }
                }
            }, {
                "roleName": "User",
                "settings": {
                    "menus": {
                        "Pulse Server": false,
                        "Kiosks": true,
                        "IoT": false,
                        "Websites": false,
                        "Configuration": false
                    },
                    "configurations": {
                        "countries": false,
                        "kiosks": false,
                        "groups": false,
                        "licenses": false,
                        "users": false,
                        "clients": false
                    }
                }
            }]
        });
})();