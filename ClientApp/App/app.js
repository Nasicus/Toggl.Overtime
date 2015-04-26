var togglApp = (function () {
	"use strict";

	var app = angular.module("togglApp", []);

	app.value("appConfig", {
	    dataWebServiceUrl: "api/data/",
	    directivesUrl: "ClientApp/App/directives/",
		dateFormat: "MM-dd-yyyy",
		numberOfWeeksPerBlock: 2
	});

	return app;
}());