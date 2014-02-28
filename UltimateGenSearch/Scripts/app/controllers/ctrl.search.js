ugsApp.controller('SearchController', ['$scope', '$http', function ($scope, $http) {
    $scope.searchLoading = false;
    $scope.searchFailed = false;
    $scope.searchSuccess = false;

    $scope.model = {
        who: "",
        where: "",
        when: ""
    };
    $scope.filterBy = {
        Ancestry: true,
        FamilySearch: true,
        FindMyPast: true
    };

    $scope.recordsMaster = [];
    $scope.records = [];
    

    $scope.search = function (_model) {
        $scope.records = [];
        $scope.recordsMaster = [];
        $scope.searchFailed = false;
        $scope.searchSuccess = false;
        $scope.searchLoading = true;

        $http({
            method: 'GET', url: '/Api/Search?Name=' + _model.who + '&Date=' + _model.when + '&Place=' + _model.where,
            headers: { 'X-Requested-With': 'XMLHttpRequest', 'Accept': 'application/json, text/plain, */*' }
            })
		    .success(function (data, status, headers, config) {
		        for (var i = 0; i < data.length; i++) {
		            $scope.recordsMaster.push(data[i]);

		            if($scope.filterBy[data[i].Vendor] == true) {
                        $scope.records.push(data[i]);
		            }
		            
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
        $scope.model.who = "John Kennedy";
        $scope.model.where = "Boston";
        $scope.model.when = "1917";
    };

    $scope.$watch('filterBy', function () {
        $scope.records = [];
        for (var i = 0; i < $scope.recordsMaster.length; i++) {
            if ($scope.filterBy[$scope.recordsMaster[i].Vendor] == true) {
                $scope.records.push($scope.recordsMaster[i]);
            }
        }
    }, true);
}]);