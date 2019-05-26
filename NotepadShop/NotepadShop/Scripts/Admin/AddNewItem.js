var viewModel;
$(document).ready(function () {

    function createViewModel() {
        var self = this;
        self.nameRu = ko.observable("");
        self.nameEn = ko.observable("");
        self.nameUk = ko.observable("");
        self.price = ko.observable("");
        self.availableCategories = ko.observableArray(categories);
        self.selectedCategory = ko.observable("");
        self.createItemClick = function () {
            createItem();
        };
    };

    viewModel = new createViewModel();

    ko.applyBindings(viewModel, document.getElementById("addNewItemContainer"));

    $('ul.tabs').tabs();
    $('select').material_select();
});

function createItem() {
    var dataObject =
    {
        Price: viewModel.price(),
        Category: viewModel.selectedCategory(),
        Names:
            [
                { Name: viewModel.nameRu(), Language: 'ru' },
                { Name: viewModel.nameEn(), Language: 'en' },
                { Name: viewModel.nameUk(), Language: 'uk' }
            ]
    };

    var createItemPromise = $.ajax({
        type: "POST",
        url: createItemUrl,
        contentType: "application/json",
        data: JSON.stringify({ __RequestVerificationToken: getAntiForgeryToken(), item: dataObject}),
        dataType: "json"
    });

    createItemPromise.done(function (p) {
        alert(p.firstName + " saved.");
    });

    createItemPromise.fail(function () {
        alert("error!");
    });
}