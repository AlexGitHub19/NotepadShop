var adminItemsViewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;
        self.items = ko.observableArray();
        self.deleteItem = onDeleteItemCallback;
        self.availableItemsCountOnPage = [2, 3, 10];
        self.itemsCountOnPage = ko.observable();
        self.selectedPageNumber = ko.observable(1);
        self.isPaginationVisible = ko.observable(false);
        self.firstPaginationElement = ko.observable(new PaginationElement(undefined, true));
        self.secondPaginationElement = ko.observable(new PaginationElement(undefined, false));
        self.thirdPaginationElement = ko.observable(new PaginationElement(undefined, false));
        self.thourthPaginationElement = ko.observable(new PaginationElement(undefined, false));
        self.fifthPaginationElement = ko.observable(new PaginationElement(undefined, false));
        self.isLeftArrowDisabled = ko.observable(true);
        self.isRightArrowDisabled = ko.observable(false);
        self.paginationElementClick = onPaginationElementClick;
        self.leftArrowClick = onLeftArrowClick;
        self.rightArrowClick = onRightArrowClick;
        self.itemsCountOnPageChange = onItemsCountOnPageChange;
    };

    adminItemsViewModel = new createViewModel();

    ko.applyBindings(adminItemsViewModel, document.getElementById("adminItemsContainer"));
    $('select').material_select();

    loadItems();
});

function loadItems() {
    var loadItemsPromise = $.ajax({
        type: "GET",
        url: '/Items/GetItems',
        contentType: "application/json",
        data: { category: category, countOnPage: adminItemsViewModel.itemsCountOnPage(), page: adminItemsViewModel.selectedPageNumber() },
        dataType: "json"
    });

    loadItemsPromise.done(function (result) {
        var items = result.Items.map(item => new Item(item));
        adminItemsViewModel.items.removeAll();
        ko.utils.arrayPushAll(adminItemsViewModel.items, items);
        processPagination(Number(result.TotalCount));
    });

    loadItemsPromise.fail(function () {
        alert("error!");
    });
}

function onItemsCountOnPageChange() {
    adminItemsViewModel.selectedPageNumber(1);
    loadItems();
}

function onPaginationElementClick(pageNumber) {
    adminItemsViewModel.selectedPageNumber(pageNumber);
    loadItems();
}

function onLeftArrowClick() {
    adminItemsViewModel.selectedPageNumber(adminItemsViewModel.selectedPageNumber() - 1);
    loadItems();
}

function onRightArrowClick() {
    adminItemsViewModel.selectedPageNumber(adminItemsViewModel.selectedPageNumber() + 1);
    loadItems();
}

function processPagination(itemsTotalCount) {
    var pagesCount = getPagesCount(itemsTotalCount);
    if (pagesCount >= 5) {
        var selectedPage = adminItemsViewModel.selectedPageNumber();
        if (adminItemsViewModel.selectedPageNumber() > 3) {
            if (pagesCount - adminItemsViewModel.selectedPageNumber() >= 2) {
                resetPaginationElementsNumbers(selectedPage + 2);
            } else {
                resetPaginationElementsNumbers(pagesCount);
            }
        } else {
            resetPaginationElementsNumbers(5);
        }
    } else {
        resetPaginationElementsNumbersIfPagesCountIsLessThen5(pagesCount);
    }
    resetArrowEnabling(pagesCount);
    resetPaginationElementsMarkedState();

    resetPaginationVisibility(pagesCount);
}

function resetPaginationElementsNumbers(rightNumber) {
    adminItemsViewModel.firstPaginationElement().number(rightNumber - 4);
    adminItemsViewModel.secondPaginationElement().number(rightNumber - 3);
    adminItemsViewModel.thirdPaginationElement().number(rightNumber - 2);
    adminItemsViewModel.thourthPaginationElement().number(rightNumber - 1);
    adminItemsViewModel.fifthPaginationElement().number(rightNumber);
}

function resetPaginationElementsNumbersIfPagesCountIsLessThen5(pagesCount) {
    adminItemsViewModel.firstPaginationElement().number(pagesCount >= 1 ? 1 : undefined);
    adminItemsViewModel.secondPaginationElement().number(pagesCount >= 2 ? 2 : undefined);
    adminItemsViewModel.thirdPaginationElement().number(pagesCount >= 3 ? 3 : undefined);
    adminItemsViewModel.thourthPaginationElement().number(pagesCount >= 4 ? 4 : undefined);
    adminItemsViewModel.fifthPaginationElement().number(pagesCount >= 5 ? 5 : undefined);
}

function resetArrowEnabling(pagesCount) {
    if (adminItemsViewModel.selectedPageNumber() === 1) {
        adminItemsViewModel.isLeftArrowDisabled(true);
    } else if (adminItemsViewModel.isLeftArrowDisabled()) {
        adminItemsViewModel.isLeftArrowDisabled(false);
    }

    if (adminItemsViewModel.selectedPageNumber() === pagesCount) {
        adminItemsViewModel.isRightArrowDisabled(true);
    } else if (adminItemsViewModel.isRightArrowDisabled()) {
        adminItemsViewModel.isRightArrowDisabled(false);
    }
}
function resetPaginationVisibility(pagesCount) {
    adminItemsViewModel.isPaginationVisible(pagesCount > 1);
}

function resetPaginationElementsMarkedState() {
    resetPaginationElemensMarkedState(adminItemsViewModel.firstPaginationElement());
    resetPaginationElemensMarkedState(adminItemsViewModel.secondPaginationElement());
    resetPaginationElemensMarkedState(adminItemsViewModel.thirdPaginationElement());
    resetPaginationElemensMarkedState(adminItemsViewModel.thourthPaginationElement());
    resetPaginationElemensMarkedState(adminItemsViewModel.fifthPaginationElement());
}

function resetPaginationElemensMarkedState(paginationElement) {
    if (paginationElement.number() === adminItemsViewModel.selectedPageNumber()) {
        if (!paginationElement.isMarked()) {
            paginationElement.isMarked(true);
        }
    } else {
        if (paginationElement.isMarked()) {
            paginationElement.isMarked(false);
        }
    }
}

function getPagesCount(itemsTotalCount) {
    return Math.ceil(itemsTotalCount / Number(adminItemsViewModel.itemsCountOnPage()));
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

function Item(item) {
    this.Code = item.Code;
    this.Price = item.Price;
    this.Name = item.Name;
    this.MainImageName = item.MainImageName;
}

function PaginationElement(number, isMarked) {
    this.number = ko.observable(number);
    this.isMarked = ko.observable(isMarked);
}