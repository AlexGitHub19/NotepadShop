var adminOrdersViewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;

        self.orders = ko.observableArray([]);
        self.dateFrom = ko.observable('');
        self.dateTo = ko.observable('');
        self.searchOrdersClick = loadOrdersClickCallack;
        self.phoneNumberToSearch = ko.observable('');
        self.ordersSearchType = ko.observable('Orders for today');
        self.isNoOrdersFoundShown = ko.observable(false);
        self.isSearchByPhoneNumberBtnEnabled = ko.computed(function () {
            return self.phoneNumberToSearch() && isPhoneNumberValid(self.phoneNumberToSearch());
        });

        self.orderNumberToSearch = ko.observable('');
        self.searchOrdersByOrderNumberClick = loadOrdersByOrderNumberClickCallback;

        self.searchOrdersByPhoneClick = loadOrdersByPhoneNumber;
    };

    adminOrdersViewModel = new createViewModel();
    const dateFrom = new Date();
    dateFrom.setHours(0, 0, 0, 0);
    const dateTo = new Date();
    dateTo.setDate(dateTo.getDate() + 1);
    dateTo.setHours(0, 0, 0, 0);
    loadOrders(dateFrom, dateTo, 'Orders for today');

    ko.applyBindings(adminOrdersViewModel, document.getElementById("adminOrdersContainer"));

    $("#datepickerFrom").datepicker();
    $("#datepickerTo").datepicker();
});

function loadOrdersClickCallack() {
    loadOrders(new Date(adminOrdersViewModel.dateFrom()), new Date(adminOrdersViewModel.dateTo()), 'Search by date range');
}

function loadOrders(dateFrom, dateTo, searchType) {
    hideOrdersNotFoundMessage();
    var loadOrdersPromise = $.ajax({
        type: "GET",
        url: '/api/orders-by-date-range',
        contentType: "application/json",
        data: { dateFrom: dateFrom.toUTCString(), dateTo: dateTo.toUTCString() },
        dataType: "json"
    });

    loadOrdersPromise.done(function (loadedOrders) {
        addOrdersToList(loadedOrders, searchType);
    });

    loadOrdersPromise.fail(function () {
        alert("error!");
    });
}

function loadOrdersByPhoneNumber() {
    hideOrdersNotFoundMessage();
    var loadOrdersByPhoneNumberPromise = $.ajax({
        type: "GET",
        url: '/api/orders-by-phone-number',
        contentType: "application/json",
        data: { phoneNumber: adminOrdersViewModel.phoneNumberToSearch() },
        dataType: "json"
    });

    loadOrdersByPhoneNumberPromise.done(function (loadedOrders) {
        addOrdersToList(loadedOrders, 'Search by phone number');
    });

    loadOrdersByPhoneNumberPromise.fail(function () {
        alert("error!");
    });
}

function loadOrdersByOrderNumberClickCallback() {
    hideOrdersNotFoundMessage();
    var loadOrdersByOrderNumbePromise = $.ajax({
        type: "GET",
        url: '/api/order-by-number',
        contentType: "application/json",
        data: { number: adminOrdersViewModel.orderNumberToSearch() },
        dataType: "json"
    });

    loadOrdersByOrderNumbePromise.done(function (loadedOrder) {
        const orders = [];
        if (loadedOrder != 'not exists') {
            orders.push(loadedOrder);
        }
        addOrdersToList(orders, 'Search by order number');
    });

    loadOrdersByOrderNumbePromise.fail(function () {
        alert("error!");
    });
}

function showOrdersNotFoundMessage() {
    if (!adminOrdersViewModel.isNoOrdersFoundShown()) {
        adminOrdersViewModel.isNoOrdersFoundShown(true);
    }
}

function hideOrdersNotFoundMessage() {
    if (adminOrdersViewModel.isNoOrdersFoundShown()) {
        adminOrdersViewModel.isNoOrdersFoundShown(false);
    }
}

function addOrdersToList(loadedOrders, searchType) {
    if (loadedOrders.length == 0) {
        showOrdersNotFoundMessage();
    }

    var orders = loadedOrders.map(item => new Order(item));
    adminOrdersViewModel.orders.removeAll();
    ko.utils.arrayPushAll(adminOrdersViewModel.orders, orders);
    adminOrdersViewModel.ordersSearchType(searchType);
        //processPagination(Number(result.TotalCount))
}

function Order(order) {
    this.number = order.Number;
    this.creatingDateTime = new Date(order.CreatingDateTime).toLocaleString();
    this.status = order.OrderStatus;
}
