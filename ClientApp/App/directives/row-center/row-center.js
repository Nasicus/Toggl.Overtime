(function (app) {
    "use strict";

    app.directive('ncRowCenter', ["appConfig", function (appConfig) {
        return {
            restrict: 'A',
            transclude: true,
            scope: {
                additionalClass: '@'
            },
            templateUrl: appConfig.directivesUrl + "row-center/row-center.html"
        };
    }]);

}(togglApp));