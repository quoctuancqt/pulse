(function () {
    'use strict';
    angular.module('pulse.paging.service', [])
    .service('pagingService', pagingService);

    function pagingService(PUlSE_SETTINGS, commonService) {
        var me = this;

        var paginationOptions = function () {
            return {
                currentPage: 0,
                pageSize: PUlSE_SETTINGS.pageSize,
                pageSizeRange: [2, 4, 6, 8, 10],
                activePage: 0,
                totalPage: 0,
                pages: 0,
                totalItems: 0,
                startIndex: 0,
                endIndex: 0,
                callBack: null,
                pageSizeCallBack: null
            }
        }

        me.getSetting = function () {
            return paginationOptions();
        }

        me.processPagination = function (paginationOptions, resp) {
            paginationOptions.totalPage = resp.data.ToTalPage;
            paginationOptions.totalItems = resp.data.TotalRecord;
            paginationOptions.currentPage = paginationOptions.currentPage == 0 ? 1 : paginationOptions.currentPage;
            paginationOptions.activePage = paginationOptions.activePage == 0 ? 1 : paginationOptions.activePage;
            paginationOptions.pages = commonService.initPagesRange(paginationOptions.totalPage, paginationOptions.currentPage, paginationOptions.pageSize);
            paginationOptions.startIndex = resp.data.TotalRecord > 0 ? me.getStartIndex(paginationOptions.activePage, paginationOptions.pageSize) : 0;
            paginationOptions.endIndex = resp.data.TotalRecord > 0 ? me.getEndIndex(paginationOptions.startIndex, paginationOptions.pageSize, paginationOptions.totalItems) : 0;
        }

        me.getStartIndex = function (currentPage, pageSize) {
            return (currentPage - 1) * pageSize + 1;
        };

        me.getEndIndex = function (startIndex, pageSize, totalItems) {
            return Math.min(startIndex + pageSize - 1, totalItems);
        };

        return me;
    }
})();