ugsApp.controller('SearchController', ['$scope', '$http', function ($scope, $http) {
    $scope.records = [];
    $scope.filterBy = "";
    

    $scope.setFilter = function (_filterBy) {
        $scope.filterBy = _filterBy;
    };

    $scope.search = function (_model) {
        $http.get('/Api/Search?Name=' + _model.Who + '&Date=' + _model.When + '&Place=' + _model.Where).then(function (data) {
            for (var i = 0; i < data.data.length; i++) {
                $scope.records.push(data.data[i]);
            }
        });
    };
}]);