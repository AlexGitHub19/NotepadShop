var adminItemsViewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;
        self.items = ko.observableArray();
        self.deleteItem = onDeleteItemCallback;
    };

    adminItemsViewModel = new createViewModel();

    ko.applyBindings(adminItemsViewModel, document.getElementById("adminItemsContainer"));

    loadItems();
});

function loadItems() {

    var createItemPromise = $.ajax({
        type: "GET",
        url: '/Items/GetItems',
        contentType: "application/json",
        data: { category: category, countOnPage: 20, page: 1 },
        dataType: "json"
    });

    createItemPromise.done(function (result) {
        var items = result.map(item => new Item(item));
        ko.utils.arrayPushAll(adminItemsViewModel.items, items);
    });

    createItemPromise.fail(function () {
        alert("error!");
    });
}

function Item(item) {
    this.Code = item.Code;
    this.Price = item.Price;
    this.Name = item.Name;
    this.MainImageName = item.MainImageName;
}

function onDeleteItemCallback(element, event) {

    adminItemsViewModel.items.remove(element);

    var deleteItemPromise = $.ajax({
        type: "POST",
        url: '/Items/DeleteItem',
        data: { __RequestVerificationToken: getAntiForgeryToken(), code: element.Code }
    });

    deleteItemPromise.fail(function () {
        alert("error!");
    });
};