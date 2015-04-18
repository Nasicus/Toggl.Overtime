(function (app) {
	"use strict";

	app.factory("webApi", ["$http", "appConfig", function ($http, appConfig) {

		function callServer(url, method, postData, success) {
			$http({ url: url, method: method, data: postData })
				.success(function (data) {
					success(data);
				})
				.error(function (data, status) {
					alert(status + ": " + data.ExceptionMessage);
				});
		}

		return {
			getData: function (apiToken, regularWorkingHours, startDate, endDate, success) {
				callServer(appConfig.dataWebServiceUrl + apiToken + "/" + regularWorkingHours + "/" + startDate + "/" + endDate, "get", null, success);
			}
		};
	}]);

}(togglApp));