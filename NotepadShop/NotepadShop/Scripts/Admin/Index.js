
$(document).ready(function () {
    function createViewModel() {
        var self = this;
        self.loginEmail = ko.observable("");

    };

    layoutViewModel = new createViewModel();

    ko.applyBindings(layoutViewModel, document.getElementById("user-login-container"));
});