(function(app) {
    "use strict";

    app.factory("webApi", ["$http", "appConfig", function($http, appConfig) {

        function callServer(url, method, postData, success, fail) {
            $http({ url: url, method: method, data: postData })
                .success(function(data) {
                    success(data);
                })
                .error(function(data, status) {
                    fail(status, data.ExceptionMessage);
                });
        }

        return {
            getData: function(apiToken, regularWorkingHours, startDate, endDate, success, fail) {
                var dataUrl = appConfig.dataWebServiceUrl + apiToken + "/" + regularWorkingHours + "/" + startDate + "/" + endDate;
                callServer(dataUrl,"get", null, success, fail);
            }
        };
    }]);

}(togglApp));