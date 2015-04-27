(function (app) {
    "use strict";

    app.directive('ncRowInput', ["appConfig", function (appConfig) {
        return {
            restrict: 'A',
            scope: {
                ngModel: '=',
                title: '@',
                inputType: '@'
            },
            templateUrl: appConfig.directivesUrl + "row-input/row-input.html"
        };
    }]);

}(togglApp));