var togglApp = (function () {
	"use strict";

	var app = angular.module("togglApp", []);

	// "static" configuration object containig values to be used where ever appConfig is injected
	app.value("appConfig", {
		dataWebServiceUrl: "/api/data/",
		dateFormat: "MM-dd-yyyy",
		numberOfWeeksPerBlock: 2
	});

	// export the app which is stored in a global variable, we need this all over the place,
	// e.g. to register directives, services, etc.
	return app;
}());