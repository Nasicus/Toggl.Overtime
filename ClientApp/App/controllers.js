(function (app) {
	"use strict";

	app.controller("togglRootController", ["$scope", "webApi", "$filter", "appConfig", function ($scope, webApi, $filter, appConfig)
	{
		$scope.apiToken = localStorage.apiToken;
		$scope.startDate = localStorage.startDate ? new Date(localStorage.startDate) : null;
		$scope.endDate = new Date();
		$scope.regularWorkingHours = localStorage.regularWorkingHours ? parseInt(localStorage.regularWorkingHours) : null;

		$scope.calculateOvertime = function()
		{
			var formatDate = function (date) { return $filter('date')(date, appConfig.dateFormat); };

			$scope.showLoadingImage = true;

			localStorage.apiToken = $scope.apiToken;
			localStorage.startDate = formatDate($scope.startDate);
			localStorage.regularWorkingHours = $scope.regularWorkingHours;

			webApi.getData($scope.apiToken, $scope.regularWorkingHours, formatDate($scope.startDate), formatDate($scope.endDate), function (data)
			{
				$scope.data = data;
				$scope.showLoadingImage = false;
			});
		};

		$scope.formatSeconds = function(s)
		{
			if (isNaN(s))
			{
				return null;
			}

			var isNegative = s < 0;
			s = Math.abs(s);

			var hours = Math.floor(s / 3600);
			var minutes = Math.floor((s / 60) % 60);
			
			return (isNegative ? "-" : "") + (hours == 0 ? "" : hours + "h ") + minutes + " min";
		};
	}]);

}(togglApp));