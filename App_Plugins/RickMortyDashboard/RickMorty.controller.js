angular.module("umbraco").controller("RickMortyDashboardController", function ($http) {
  var vm = this;

  vm.importCharacters = function () {
    vm.message = "Importing...";

    $http.post("/umbraco/api/rickmorty/import").then(function (res) {
      vm.message = res.data;
    }, function (err) {
      vm.message = "Something went wrong.";
    });
  };
});