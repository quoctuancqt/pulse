(function () {
    'use strict';

    angular
        .module('app.pulse')
        .service('commonService', commonService);

    function commonService($timeout, baseApi, helperFactory) {

        var me = this;
        var api = baseApi.Api();

        me.startPage = 0;
        me.endPage = 0;

        me.getCountry = function (callBack) {
            api.get('countries').then(function (resp) {
                me.fireCallBack(callBack, resp);
            });
        };

        me.getClientsCountries = function (callBack) {
            api.search('clientscountries').then(function (resp) {
                me.fireCallBack(callBack, resp);
            });
        };

        me.getGroup = function (callBack) {
            return api.search('groups').then(function (resp) {
                me.fireCallBack(callBack, resp);
            });
        };

        me.getKiosks = function (options, callBack) {
            api.search('kiosks', options).then(function (resp) {
                me.fireCallBack(callBack, resp);
            }, function (error) {
                me.fireCallBack(callBack, error);
            });
        };

        me.getLicense = function (options, callBack) {
            api.customGet('kiosks', 'searchLicense', options).then(function (resp) {
                me.fireCallBack(callBack, resp);
            }, function (error) {
            });
        };

        me.getClients = function (callBack) {
            api.search('clients').then(function (resp) {
                me.fireCallBack(callBack, resp);
            }, function (error) {
                me.fireCallBack(callBack, error);
            });
        };

        me.getKiosInfo = function (machineId, successCallBack, errorCallBack) {
            api.customGet('kiosks', 'bymachineid/' + machineId, null).then(function (resp) {
                me.fireCallBack(successCallBack, resp);
            }, function (error) {
                me.fireCallBack(errorCallBack, error);
            });
        };

        me.showProgressBar = function () {
            $('#loading').show();
            $('.dashboard-container').addClass('loading');
        };

        me.hideProgressBar = function (time) {
            $timeout(function () {
                $('#loading').hide();
                $('.dashboard-container').removeClass('loading');
            }, helperFactory.checkUndefined(time, 2000));
        };

        me.initDropdownData = function (data) {
            var array = [];

            $.each(data, function (i, v) {
                var item = {
                    id: v.Id,
                    value: v.Name
                };
                array.push(item);
            })

            return array;
        };

        me.initStatusData = function () {
            var status = [];
            status.push({ label: 'Deactivate', value: 0 });
            status.push({ label: 'Activate', value: 1 });
            return status;
        };

        me.initPagesRange = function (totalPages, currentPage, pageSize) {
            var startPage, endPage;

            if (totalPages <= 10) {
                startPage = 1;
                endPage = totalPages;
            } else {
                if (currentPage <= 6) {
                    startPage = 1;
                    endPage = 10;
                } else if (currentPage + 4 >= totalPages) {
                    startPage = totalPages - 9;
                    endPage = totalPages;
                } else {
                    startPage = currentPage - 5;
                    endPage = currentPage + 4;
                }
            }

            return _.range(startPage, endPage + 1);
        };

        me.showPopup = function (id) {
            $('#' + id).modal('show');
        };

        me.hidePopup = function (id) {
            $('#' + id).modal('hide');
        };

        me.fireCallBack = function (callBack, data) {
            if (typeof (callBack) != "undefined") {
                callBack(data);
            }
        };

        me.generateKey = function (value) {
            var str = value.substring(0, 1).toUpperCase();
            return str + value.substring(1, value.length);
        };

        me.convertEntitytoDto = function (entity) {
            var obj = {};
            $.each(entity, function (i, item) {
                if (item.type == 'autocomplete') {
                    obj[item.id] = item.value.id;
                }
                else {
                    obj[item.id] = item.value;
                }
            });

            return obj;
        };

        me.convertEntitytoSchema = function (viewModel, entity) {
            $.each(viewModel, function (i, item) {
                if (item.type == 'autocomplete') {
                    if (item.id == 'countryId') {
                        item.value = { id : entity['CountryId'],value: helperFactory.checkUndefined(entity['CountryName'])};
                    }
                    if (item.id == 'groupId') {
                        item.value = { id: entity['GroupId'], value: helperFactory.checkUndefined(entity['GroupName']) };
                    }
                }
                else {
                    item.value = helperFactory.checkUndefined(entity[me.generateKey(item.id)]);
                }
            });
        };

        me.randomString = function randomString(len, charSet) {
            charSet = charSet || 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
            var randomString = '';
            for (var i = 0; i < len; i++) {
                var randomPoz = Math.floor(Math.random() * charSet.length);
                randomString += charSet.substring(randomPoz, randomPoz + 1);
            }
            return randomString;
        };

        me.resetSchema = function (schema) {
            $.each(schema, function (i, v) {
                if (v.type == 'string') {
                    v.value = '';
                }
                if (v.type == 'int') {
                    v.value = 0;
                }
                if (v.type == 'select') {
                    v.value = 0;
                }
                if (v.type == 'autocomplete') {
                    v.value = '';
                }
                if (v.type == 'email') {
                    v.value = '';
                }
            });
        };

        me.getErrorMsg = function (error) {
            var result = JSON.stringify(error.data[0]).split(":");
            return result[result.length - 1].replace(/[&\/\\#,+()$~%.'":*?<>{}]/g, '');
        };

        return me;
    }
})();
