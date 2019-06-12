(function () {
    'use strict';

    angular
        .module('app.pulse.configuration')
        .controller('configurationClientsFormCtrl', configurationClientsFormCtrl);

    function configurationClientsFormCtrl($scope, $state, $stateParams, PUlSE_SETTINGS, commonService, configurationClientsService, helperFactory, notificationFactory, triSettings, sessionFactory) {
        var vm = this;
        commonService.hideProgressBar();
        vm.mode = $stateParams.mode.toString().trim();
        vm.clientId = $stateParams.id;
        vm.client = {
        	SignalrUrl : PUlSE_SETTINGS.signalUri,
			MongoConnectionString : 'localhost:27017',
			RefreshTokenLifeTime : 7,
			TokenLifeTime : 20,
			AllowedOrigin : '*',
			AllowedGrant : 1
        };

        vm.cancel = function () {
        	$state.go('triangular.configuration-clients');
        }

        vm.getData = function () {
        	commonService.showProgressBar();
        	configurationClientsService.getHandler(vm.clientId, SuccessCallBack, ErrorCallBack);
        }

        if (vm.mode == 'edit') {
        	vm.getData();
        }
        else {
        	$('#pulse_container').addClass(' pulse-dashboard-style');
        }

        vm.submit = function () {
        	switch (vm.mode) {
        		case 'edit': {
        			configurationClientsService.updateHandler(vm.client, SuccessCallBack, ErrorCallBack);
        			break;
        		};
        		default: {
        			configurationClientsService.createHandler(vm.client, SuccessCallBack, ErrorCallBack);
        			break;
        		}
        	}
        	$state.go('triangular.configuration-clients');
        }

    	//PRIVATE FUNCTION
        function SuccessCallBack(resp) {
        	vm.client = resp.data;
        	commonService.hideProgressBar();
        	notificationFactory.show('Success');
        }

        function ErrorCallBack(error) {
        	commonService.hideProgressBar();
        	notificationFactory.show(error.data.ExceptionMessage);
        }
    }
})();