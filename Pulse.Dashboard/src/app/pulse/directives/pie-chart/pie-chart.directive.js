(function () {
	'use strict';

	angular
        .module('app.pulse')
        .directive('pieChartWidget', pieChartWidget);

	function pieChartWidget() {
		var directive = {
			templateUrl: 'app/pulse/directives/pie-chart/pie-chart-tmp.html',
			scope: {
				data: '=setting'
			},
			restrict: 'E',
			link: function (scope, elem, attr) {
			    //scope.$on('pieChartWidget_BindData', function (event, args) {
			    //    var item = args.chartPieData;
			    //    scope.labels = item.labels;
			    //    scope.pie_data = item.data;
			    //});
			    scope.$watch('data', function (newValue, oldValue) {
			        if (newValue) {
			            scope.labels = scope.data.labels;
			            scope.pie_data = scope.data.data;
			        };
			    }, true);
			}
		};
		return directive;
	}
})();