var homeIndexViewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;
        self.createItemClick = function () {
            createItem();
        };
    };

    homeIndexViewModel = new createViewModel();

    ko.applyBindings(addNewItemViewModel, document.getElementById("homeIndexContainer"));   
});