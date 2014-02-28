ugsApp.controller('SearchController', ['$scope', '$http', function ($scope, $http) {
    $scope.searchLoading = false;
    $scope.searchFailed = false;

    $scope.showSearchPage = false;
    $scope.showAccountPage = false;

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
    $scope.myAccounts = [];
    

    $scope.search = function (_model) {
        $scope.showAccountPage = false;

        $scope.records = [];
        $scope.recordsMaster = [];
        $scope.searchFailed = false;
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
		        $scope.showSearchPage = true;
		        $scope.searchFailed = false;
		    })
		    .error(function (data, status, headers, config) {
		        $scope.searchLoading = false;
		        $scope.showSearchPage = false;
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

    $scope.showAccountPageFn = function () {
        $scope.showSearchPage = false;
        $scope.showAccountPage = true;
        $scope.searchLoading = false;
        $scope.searchFailed = false;

        $scope.myAccounts = [];
        $http({
            method: 'GET', url: '/Api/Account',
            headers: { 'X-Requested-With': 'XMLHttpRequest', 'Accept': 'application/json, text/plain, */*' }
        })
		    .success(function (data, status, headers, config) {
		        for (var i = 0; i < data.length; i++) {
		            $scope.myAccounts.push(data[i]);
		        }
		    })
		    .error(function (data, status, headers, config) {
		    });
    };

    $scope.connectAccount = function (_model, _name) {
        _model.Name = _name;
        var json = JSON.stringify(_model);
        $http({
            url: '/Api/Account',
            method: "POST",
            data: json,
            headers: { 'X-Requested-With': 'XMLHttpRequest', 'Accept': 'application/json, text/plain, */*' }
        }).success(function (data, status, headers, config) {
            $scope.showAccountPageFn();
        }).error(function (data, status, headers, config) {
            $scope.showAccountPageFn();
        });
    };
}]);