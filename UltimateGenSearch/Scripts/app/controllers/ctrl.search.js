ugsApp.controller('SearchController', ['$scope', '$http', function ($scope, $http) {
    $scope.model = {
        who: "",
        where: "",
        when: ""
    }
    $scope.records = [];
    $scope.filterBy = "";
    $scope.searchLoading = false;
    $scope.searchFailed = false;
    $scope.searchSuccess = false;
    

    $scope.setFilter = function (_filterBy) {
        $scope.filterBy = _filterBy;
    };

    $scope.search = function (_model) {
        $scope.records = [];
        $scope.searchFailed = false;
        $scope.searchSuccess = false;
        $scope.searchLoading = true;

        $http({
            method: 'GET', url: '/Api/Search?Name=' + _model.who + '&Date=' + _model.when + '&Place=' + _model.where,
            headers: { 'X-Requested-With': 'XMLHttpRequest', 'Accept': 'application/json, text/plain, */*' }
            })
		    .success(function (data, status, headers, config) {
		        for (var i = 0; i < data.length; i++) {
		            $scope.records.push(data[i]);
		        }
		        $scope.searchLoading = false;
		        $scope.searchSuccess = true;
		        $scope.searchFailed = false;
		    })
		    .error(function (data, status, headers, config) {
		        $scope.searchLoading = false;
		        $scope.searchSuccess = false;
		        $scope.searchFailed = true;
		    });
    };

    $scope.populateForm = function () {
        $scope.model.who = "John Smith";
        $scope.model.where = "New York";
        $scope.model.when = "1920";
    };
}]);