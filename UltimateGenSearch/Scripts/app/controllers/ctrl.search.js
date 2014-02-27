ugsApp.controller('SearchController', ['$scope', '$http', function ($scope, $http) {
    $scope.records = [];
    $scope.filterBy = "";
    

    $scope.setFilter = function (_filterBy) {
        $scope.filterBy = _filterBy;
    };

    $scope.search = function (_model) {
        $scope.records = [];
        $http.get('/Api/Search?Name=' + _model.who + '&Date=' + _model.when + '&Place=' + _model.where).then(function (data) {
            for (var i = 0; i < data.data.length; i++) {
                $scope.records.push(data.data[i]);
            }
        });
    };
}]);