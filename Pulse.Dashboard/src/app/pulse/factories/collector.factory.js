(function () {
    'use strict';

    angular
        .module('app.pulse')
        .factory('collectorFactory', collectorFactory);

    function collectorFactory($rootScope) {

        var collectorFactory = {};
        /* publish method */
        collectorFactory.dataBinding = function (jsonObject, model) {
            getLiveChartData(model.cpu, jsonObject.cpu.value);
            getLiveChartData(model.bandwidth, jsonObject.network.total / 1024);
            getLiveChartData(model.memory, jsonObject.memory.value);
            model.dataMemory[0].value = jsonObject.memory.value;
            model.dataDisk[0].value = jsonObject.disk.value;
            model.dataCPU[0].value = jsonObject.cpu.value;
            model.temperature.data = jsonObject.temperature.value;
            model.networkIn.data = jsonObject.network.received;
            model.networkOut.data = jsonObject.network.sent;
            model.ipAddress = jsonObject.network.ipAddress;
            model.systemName = jsonObject.systemName;
            model.details = collectorFactory.prepareDetailDisk(jsonObject.details);
            if (typeof (jsonObject.eventLog.Error) != "undefined") {
                model.chartPieData.data = prepareEventLogData(jsonObject.eventLog);
            }
            return model;
        }

        collectorFactory.initPulseViewModel = function (options) {
            var settings = {};
            settings.ipAddress = '';
            settings.systemName = '';
            settings.address = '';
            settings.networkIn = {
                color: 'deep-orange:500',
                icon: 'fa fa-sign-in',
                title: 'Network in ( k/b)',
                data: 0
            };
            settings.networkOut = {
                color: 'triCyan:500',
                icon: 'fa fa-sign-out',
                title: 'Network out ( k/b)',
                data: 0
            };
            settings.temperature = {
                color: 'indigo:500',
                icon: 'fa fa-tint',
                title: 'Temperature (<sup>o</sup>C)',
                data: 0
            };
            settings.website = {
                color: 'teal:500',
                icon: 'zmdi zmdi-globe',
                title: 'Website Request',
                data: 120
            };
            settings.chartPieData = {};
            settings.details = {};
            settings.dataDisk = [
                { label: "Disk", value: 0, color: "#2980b9", suffix: "%" }
            ];
            settings.dataMemory = [
                { label: "Memory", value: 0, color: "#8e44ad", suffix: "%" }
            ];
            settings.dataCPU = [
                { label: "CPU", value: 0, color: "#DB4437", suffix: "%" }
            ];
            settings.user_activities_data = [];
            settings.system_event_data = [];
            settings.cpu = {
                dataLength: 50,
                data: [[]],
                labels: [],
                colours: ['#DB4437']
            };
            settings.bandwidth = {
                dataLength: 50,
                data: [[]],
                labels: [],
                colours: ['#4285F4']
            };
            settings.memory = {
                dataLength: 50,
                data: [[]],
                labels: [],
                colours: ['#9b59b6']
            };

            return $.extend(settings, options);
        }

        collectorFactory.sendDataForGeoChart = function (data) {
            $rootScope.$broadcast('geoChartWidget_BindData', { data: data });
        }

        collectorFactory.prepareDetailDisk = function (obj) {
            var data = [];
            $.each(obj, function (i, item) {
                data.push({
                    label: i,
                    value: item
                });
            });
            for (var i = 0; i < data.length; i++) {
                data[i].class_color = getClassColorDisk(i);
            }
            return data;
        }
        /* private method*/

        function getLiveChartData(chart, value) {
            if (chart.data[0].length) {
                chart.labels = chart.labels.slice(1);
                chart.data[0] = chart.data[0].slice(1);
            }

            while (chart.data[0].length < chart.dataLength) {
                chart.labels.push(' ');
                chart.data[0].push(value);
            }
        }

        function prepareEventLogData(obj) {
            var output = [];
            output.push(obj.Error);
            output.push(obj.Information);
            output.push(obj.Warning);
            return output;
        }

        function getClassColorDisk(value) {
            if (value == 0) {
                return 'md-default';
            }
            if (value == 1) {
                return 'md-accent';
            }
            if (value == 2) {
                return 'md-warn';
            }
            return 'md-default'
        }

        return collectorFactory;
    }
})();
