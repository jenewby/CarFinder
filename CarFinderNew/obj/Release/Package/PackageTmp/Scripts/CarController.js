// JavaScript source code
var myApp = angular.module('myModule', ['ui.bootstrap','trNgGrid', 'td.easySocialShare' ,'ngAnimate']);

myApp.controller('myController', ['$http', '$uibModal', function ($http, $uibModal) {

    var scope = this;
    scope.years = [];
    scope.makes = [];
    scope.models = [];
    scope.trims = [];
    scope.info = [];

    scope.option = {
        year: '',
        make: '',
        model: '',
        trim: ''
    };

    scope.getYears = function () {
        $http.get("api/cars/years").then(function (response) {
            scope.years = response.data;
        });
    }

    scope.getMakes = function () {
        scope.option.make = "";
        scope.option.model = "";
        scope.option.trim = "";
        scope.cars = [];
        $http.get("api/cars/make", { params: { year: scope.option.year } }).then(function (response) {
            scope.makes = response.data;
            scope.getCars();
        })
    }

    scope.getModels = function () {
        scope.option.model = "";
        scope.option.trim = "";
        scope.cars = [];

        $http.get("api/cars/model", { params: { year: scope.option.year, make: scope.option.make } }).then(function (response) {
            scope.models = response.data;
            scope.getCars();
        })
    }


    scope.getTrims = function () {
        scope.option.trim = "";
        scope.cars = [];

        $http.get("api/cars/trim", { params: { year: scope.option.year, make: scope.option.make, model: scope.option.model, } }).then(function (response) {
            scope.trims = response.data;
            scope.getCars();
        });
    }

    scope.getCars = function () {
        $http.get("api/cars/GetCars", { params: { year: scope.option.year, make: scope.option.make, model: scope.option.model, trim: scope.option.trim } }).then(function (response) {
            scope.cars = response.data;

        });
    }

    scope.getYears();

    scope.id = {
        id: ''
    };


    //scope.getId = function (james) {
    //    scope.id.id = james;
    //    $http.get("http://RiaCar.azurewebsites.net/api/cars/GetInfo", { params: { id: scope.id.id } }).then(function (response) {
    //        scope.info = response.data;
    //    });

    //};

    scope.open = function (id) {
        scope.id.id = id;
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'carInfo.html',
            controller: 'infoController as cm',
            //size: 'lg',
            windowClass: 'app-modal-window',
            resolve: {
                car: function () {
                    return $http.get("http://RiaCar.azurewebsites.net/api/cars/GetInfo", { params: { id: scope.id.id } });
                }
            }
        })
        modalInstance.result.then(
                //close
                function (result) {
                    var a = result;
                },
                //dismiss
                function (result) {
                    var a = result;
                });
    }

}]);

angular.module('myModule').controller('infoController', function ($uibModalInstance, car){
    var self = this;
    self.car = car.data;

    self.ok= function (){
        $uibModalInstance.close();
    };

    self.cancel = function () {
        $uibModalInstance.dismiss();

    }

});

myApp.filter('capitalize', function () {
    return function (input) {
        return (!!input) ? input.charAt(0).toUpperCase() + input.substr(1).toLowerCase() : '';
    }
});

//<div class="row" style="padding-top:10px">
//               <div class="col-md-5">
//                   <table id="cartable">
//                       <thead>
//                       <tr>
//                           <th>Year</th>
//                           <th>Make</th>
//                           <th>Series</th>
//                           <th>Style</th>
//                           <th>Transmission</th>
//                       </tr>
//                           </thead>
//                       <tbody>
//                           <tr ng-repeat="car in carCtrl.cars">
//                               <td>{{car.model_year}}</td>
//                               <td>{{car.make}}</td>
//                               <td>{{car.model_name}}</td>
//                               <td>{{car.model_trim}}</td>
//                               <td>{{car.transmission_type}}</td>
//                               <td><button ng-click="carCtrl.open(car.id)">See more</button></td>
//                           </tr>
//                       </tbody>
//                   </table>
//                   </div>
//</div>