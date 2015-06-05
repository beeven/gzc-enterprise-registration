var app = angular.module('app', ["ui.grid", "ui.bootstrap", "ngResource"]);

angular.element(document).ready(function () {
    console.log("hello bootstrap");
    angular.bootstrap(document, ['app']);
});

app.factory("MessageService", ["$resource", function ($resource) {
    var msgService = $resource("api/Messages/:id", { id: "@id" }, {
        query: {method:"GET", params: {id:"", pageSize:10,offset:0}, isArray:true}
    });
    return msgService;
}]);

app.controller("MainCtrl", ["$scope", "MessageService", function ($scope, msgSvc) {
    console.log("Hello");
    $scope.gridData = msgSvc.query();
}]);