(function (app) {
    "use strict";
    
    app.directive('ngRowInput', ["appConfig", function (appConfig) {
        return {
            restrict: 'A',
            require: '^ngModel',
            scope: {
                ngModel: '=',
                title: '@',
                inputType: '@'
            },
            templateUrl: appConfig.directivesUrl + "row-input/row-input.html"
        };
    }]);

}(togglApp));