var profileOrdersViewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;

        self.orders = ko.observableArray([]);
    };

    profileOrdersViewModel = new createViewModel();
    ko.applyBindings(profileOrdersViewModel, document.getElementById("profileOrdersContainer"));
    loadOrders();
});

function loadOrders() {
    var loadOrdersPromise = $.ajax({
        type: "GET",
        url: '/profile/api/user-orders',
        contentType: "application/json",
        dataType: "json"
    });

    loadOrdersPromise.done(function (result) {
        var orders = result.map(item => new Order(item));
        profileOrdersViewModel.orders.removeAll();
        ko.utils.arrayPushAll(profileOrdersViewModel.orders, orders);
        //processPagination(Number(result.TotalCount));
    });

    loadOrdersPromise.fail(function () {
        alert("error!");
    });
}

function Order(order) {
    this.number = order.Number;
    this.creatingDateTime = new Date(order.CreatingDateTime).toLocaleString();
    this.status = order.OrderStatus;
}
