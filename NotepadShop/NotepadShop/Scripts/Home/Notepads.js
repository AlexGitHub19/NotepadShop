var homeItemsViewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;
        self.items = ko.observableArray();
    };

    homeItemsViewModel = new createViewModel();

    ko.applyBindings(homeItemsViewModel, document.getElementById("homeItemsContainer"));

    loadItems();
});

function loadItems() {

    var createItemPromise = $.ajax({
        type: "GET",
        url: '/Items/GetItems',
        contentType: "application/json",
        data: { category: 'Notepad', countOnPage: 20, page: 1 },
        dataType: "json"
    });

    createItemPromise.done(function (result) {
        var items = result.map(item => new Item(item));
        ko.utils.arrayPushAll(homeItemsViewModel.items, items);
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