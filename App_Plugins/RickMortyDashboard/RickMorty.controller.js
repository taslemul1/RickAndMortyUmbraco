angular.module("umbraco").controller("RickMortyDashboardController", function ($scope, $http, $window) {
  var vm = this;

  vm.message = "";
  vm.showReload = false;

  vm.importCharacters = function () {
    vm.message = "Importing...";
    vm.showReload = false;

    $http.post("/umbraco/api/rickmorty/import").then(function (res) {
      vm.message = res.data;
      vm.showReload = true;
    }, function (err) {
      console.error("API error:", err);
      vm.message = err.data || "Something went wrong.";
    });
  };

  vm.reloadPage = function () {
    $window.location.reload();
  };
});
