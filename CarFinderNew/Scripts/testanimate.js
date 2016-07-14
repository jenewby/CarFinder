// JavaScript source code
var myApp = angular.module('thisone', ['ngAnimate']);

myApp.controller('testController', function ($scope) {

    $scope.title = "hope this works";

    var employee = [
        { name: 'CF', city: 'Greensboro' },
        { name: 'CF1', city: 'High Point' }
    ];
    $scope.employees = employee;

});