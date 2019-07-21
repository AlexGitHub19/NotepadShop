var adminOrdersViewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;

        self.orders = ko.observableArray([]);
        self.dateFrom = ko.observable("");
        self.dateTo = ko.observable("");
        self.searchOrdersClick = loadOrdersClickCallack;
    };

    adminOrdersViewModel = new createViewModel();
    const dateFrom = new Date();
    dateFrom.setHours(0, 0, 0, 0);
    const dateTo = new Date();
    dateTo.setDate(dateTo.getDate() + 1);
    dateTo.setHours(0, 0, 0, 0)
    loadOrders(dateFrom, dateTo);

    ko.applyBindings(adminOrdersViewModel, document.getElementById("adminOrdersContainer"));

    $("#datepickerFrom").datepicker();
    $("#datepickerTo").datepicker();
});

function loadOrdersClickCallack() {
    loadOrders(new Date(adminOrdersViewModel.dateFrom()), new Date(adminOrdersViewModel.dateTo()));
}

function loadOrders(dateFrom, dateTo) {
    var loadOrdersPromise = $.ajax({
        type: "GET",
        url: '/api/orders-by-date-range',
        contentType: "application/json",
        data: { dateFrom: dateFrom.toUTCString(), dateTo: dateTo.toUTCString() },
        dataType: "json"
    });

    loadOrdersPromise.done(function (result) {
        var orders = result.map(item => new Order(item));
        adminOrdersViewModel.orders.removeAll();
        ko.utils.arrayPushAll(adminOrdersViewModel.orders, orders);
        //processPagination(Number(result.TotalCount));
    });

    loadOrdersPromise.fail(function () {
        alert("error!");
    });
}

function Order(order) {
    this.number = order.Number;
    this.creatingDateTime = order.CreatingDateTime.slice(0, order.CreatingDateTime.lastIndexOf(':'));
    this.status = order.OrderStatus;
}
