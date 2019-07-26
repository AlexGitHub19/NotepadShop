var homeShopViewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;
    };

    homeIndexViewModel = new createViewModel();

    ko.applyBindings(homeShopViewModel, document.getElementById("homeShopContainer"));   
});