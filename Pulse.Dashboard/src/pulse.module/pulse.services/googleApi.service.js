(function () {
    'use strict';

    angular
        .module('pulse.googleApi', [])
        .service('googleApiService', googleApiService);
    //To use Geocoding from Google Maps V3 you need to link https://maps.googleapis.com/maps/api/js?sensor=false
    function googleApiService() {
        var me = this;
        me.geocoder = new google.maps.Geocoder();
        me.getLatLng = function (callback, address) {
            if (me.geocoder) {
                me.geocoder.geocode({
                    'address': address
                }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        callback(results[0]);
                    }
                });
            }
        }

        return me;
    }
})();
