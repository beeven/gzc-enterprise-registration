var app = angular.module('app', ["ui.grid","ui.grid.pagination", "ui.bootstrap", "ngResource"]);

angular.element(document).ready(function () {
    angular.bootstrap(document, ['app']);
});

app.factory("MessageService", ["$resource", function ($resource) {
    var msgService = $resource("api/Messages/:id", { id: "@id" }, {
        query: { method: "GET", params: { id: "", pageSize: 10, offset: 0 }, isArray: true }
    });
    return msgService;
}]);

app.controller("MainCtrl", ["$scope", 'uiGridConstants', "MessageService",function ($scope,uiGridConstants, msgSvc) {
    var paginationOptions = {
        pageNumber: 1,
        pageSize: 25,
        sort: null
    };

    $scope.gridOptions = {
        paginationPageSizes: [10,25,50,75,100],
        paginationPageSize: 10,
        useExternalPagination: true,
        useExternalSorting: true,
        columnDefs: [
            { name: "From", field: "FromAddress" },
            { name: "Subject", field: "Subject" },
            { name: "Body", field: "Body" },
            { name: "Sent", field: "DateSent" }
        ],
        onRegisterApi: function(gridApi)  {
            $scope.gridApi = gridApi;
            $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                if (sortColumns.length == 0) {
                    paginationOptions.sort = null;
                } else {
                    paginationOptions.sort = sortColumns[0].sort.direction;
                }
            });

            gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                paginationOptions.pageNumber = newPage;
                paginationOptions.pageSize = pageSize;
                getPage(pageSize, (newPage - 1) * pageSize);
            });
        }
    }

    var getPage = function (pageSize, offset) {
        $scope.gridOptions.data = msgSvc.query(pageSize, offset);
    }

    getPage(paginationOptions.pageSize, 0);
}]);