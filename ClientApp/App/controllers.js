(function(app)
{
	"use strict";

	app.controller("togglRootController", ["$scope", "webApi", "$filter", "appConfig", function($scope, webApi, $filter, appConfig)
	{
		$scope.appConfig = appConfig;
		$scope.apiToken = localStorage.apiToken;
		$scope.startDate = localStorage.startDate ? new Date(localStorage.startDate) : null;
		$scope.endDate = new Date();
		$scope.regularWorkingHours = localStorage.regularWorkingHours ? parseFloat(localStorage.regularWorkingHours) : null;

		$scope.calculateOvertime = function()
		{
			$scope.showLoadingImage = true;

			var formattedStartDate = formatDate($scope.startDate);
			var formattedEndDate = formatDate($scope.endDate);

			localStorage.apiToken = $scope.apiToken;
			localStorage.startDate = formattedStartDate;
			localStorage.regularWorkingHours = $scope.regularWorkingHours;

			webApi.getData($scope.apiToken, $scope.regularWorkingHours, formattedStartDate, formattedEndDate, function(data)
			{
				$scope.data = data;
				$scope.weeksInBlocks = getDataInBlocks(appConfig.numberOfWeeksPerBlock, data.Weeks);
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

		$scope.formatDateToLocale = function(date)
		{
			return new Date(date).toLocaleDateString();
		};

		function formatDate(date)
		{
			return $filter('date')(date, appConfig.dateFormat);
		};

		function getDataInBlocks(blockSize, data)
		{
			var returnValues = [];
			var counter = 0;
			var currentResults = [];
			for (var i = 0; i < data.length; i++)
			{
				if (counter == blockSize)
				{
					counter = 0;
					returnValues.push(currentResults);
					currentResults = [];
				}

				currentResults.push(data[i]);
				counter++;
			}

			return returnValues;
		};
	}]);
}(togglApp));