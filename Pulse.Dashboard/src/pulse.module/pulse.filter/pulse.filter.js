(function () {
    'use strict';

    angular
        .module('pulse.filter', [])
        .filter('mapCountry', function () {
            return function (input, scope) {
                if (!input) {
                    return '';
                } else {
                    for (var i = 0; i < scope.vm.countries.length; i++) {
                        if (scope.vm.countries[i].id == input) {
                            return scope.vm.countries[i].name;
                        }
                    }
                }
            };
        })
        .filter('mapGroup', function () {
            return function (input, scope) {
                if (!input) {
                    return '';
                } else {
                    for (var i = 0; i < scope.vm.groups.length; i++) {
                        if (scope.vm.groups[i].id == input) {
                            return scope.vm.groups[i].name;
                        }
                    }
                }
            };
        })
    .filter('mapGender', function () {
        return function (input) {
            if (input == 0) {
                return 'Male';
            } else {
                return 'Female';
            }
        };
    })
    .filter('mapAllowedGrant', function () {
        return function (input) {
            if (input == 0) {
                return 'SystemAdmin';
            } else {
                return 'Client';
            }
        };
    })
})();