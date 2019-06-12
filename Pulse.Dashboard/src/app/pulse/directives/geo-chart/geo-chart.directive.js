(function () {
    'use strict';

    angular
        .module('app.pulse')
        .directive('geoChartWidget', geoChartWidget);

    function geoChartWidget($rootScope, PUlSE_SETTINGS) {
        var directive = {
            templateUrl: 'app/pulse/directives/geo-chart/geo-chart-tmp.html',
            restrict: 'E',
            scope: {
                data: '=setting'
            },
            link: function (scope, elem, attr) {
                scope.$watch('data', function (newValue, oldValue) {
                    if (newValue && $rootScope.isReloadMap) {
                        init(newValue);
                        $rootScope.isReloadMap = false;
                    } 
                }, true);

                var init = function (data) {
                    
                    navigator.geolocation.getCurrentPosition(function (position) {
                        
                        scope.mapOptions = {
                            zoom: 5,
                            center: new google.maps.LatLng(position.coords.latitude, position.coords.longitude),
                            mapType: 'normal',
                        }

                        scope.map = new google.maps.Map(document.getElementById('map'), scope.mapOptions);

                        scope.markers = [];

                        scope.infoWindow = new google.maps.InfoWindow();

                        bindData(data);
                    });
                    
                }
                
                var createMarker = function (info) {
                    
                    var marker = new google.maps.Marker({
                        map: scope.map,
                        position: new google.maps.LatLng(info.lat, info.long),
                        title: info.name,
                        icon: info.status == 'Offline' ? PUlSE_SETTINGS.defaultUrlMap + 'offline.png' : PUlSE_SETTINGS.defaultUrlMap + 'online.png'
                    });

                    marker.content = '<div class="infoWindowContent">' + info.address + '</div>';

                    google.maps.event.addListener(marker, 'click', function () {
                        scope.infoWindow.setContent('<h2 class="infoWindowTitle">' + marker.title + '</h2>' + marker.content);
                        scope.infoWindow.open(scope.map, marker);
                    });

                    scope.markers.push(marker);

                }

                var bindData = function (data) {
                    scope.markers = [];
                    $.each(data, function (i, v) {
                        var obj = {
                            name: v.Info.Name,
                            address: v.Info.Address,
                            status: v.Info.Status == 0 ? 'Offline' : 'Online',
                            lat: v.Info.Lat,
                            long: v.Info.Long
                        };
                        createMarker(obj);
                    })
                }

            }
        };
        return directive;
    }
})();