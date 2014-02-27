ugsApp.controller('SearchController', ['$scope', '$http', function ($scope, $http) {
    $scope.records = [];
    $scope.filterBy = "";
    $http.get('/Api/Search').then(function (data) {
        for (var i = 0; i < data.data.length; i++) {
            $scope.records.push(data.data[i]);
        }
    });

    $scope.setFilter = function (_filterBy) {
        $scope.filterBy = _filterBy;
    }
}]);