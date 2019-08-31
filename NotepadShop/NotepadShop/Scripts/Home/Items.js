var homeItemsViewModel; 

$(document).ready(function () {

    recalculateItemsLoadingContianerHeight();

    function createViewModel() {
        var self = this;
        self.items = ko.observableArray();
        self.availableItemsCountOnPage = [3, 2, 1, 10];
        self.itemsCountOnPage = ko.observable();
        self.selectedPageNumber = ko.observable(1);
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
        self.addItemToCartClick = onAddItemToCartClick;
    };

    homeItemsViewModel = new createViewModel();

    ko.applyBindings(homeItemsViewModel, document.getElementById("homeItemsContainer"));

    loadItems();

    $(window).resize(() => recalculateItemsLoadingContianerHeight());
});

let itemsLoading = false;
let scollingAnimationContinues = false;
let shouldPaginationBeShown = false;

function loadItems() {

    beforeLoadingCallback();

    var loadItemsPromise = $.ajax({
        type: "GET",
        url: '/items/api/get-items',
        contentType: "application/json",
        data: { category: category, countOnPage: homeItemsViewModel.itemsCountOnPage(), page: homeItemsViewModel.selectedPageNumber() },
        dataType: "json"
    });

    loadItemsPromise.done(function (result) {
        itemLoadingCompletedCallback();
        var items = result.Items.map(item => new Item(item));
        homeItemsViewModel.items.removeAll();
        ko.utils.arrayPushAll(homeItemsViewModel.items, items);
        processPagination(Number(result.TotalCount));
        showItemsContainer();
    });

    loadItemsPromise.fail(function () {
        itemLoadingCompletedCallback();
        showItemsContainer();
        alert("error!");
    });
}

function beforeLoadingCallback() {
    itemsLoading = true;
    scollingAnimationContinues = true;

    $('html').animate({
        scrollTop: 0
    }, 200, () => {
        scollingAnimationContinues = false;
        if (itemsLoading) {
            $('.items-loading-container').fadeIn(200);
        } else {
            if ($('.items-loading-container').css('display') != 'none') {
                $('.items-loading-container').fadeOut(200, () => {
                    displayItemsContainer();
                });
            } else {
                displayItemsContainer();
            }
        }
    });
    $('.items-container').fadeOut(200);
    $('.pagination-container').fadeOut(200);
}

function showItemsContainer() {
    if (!scollingAnimationContinues) {
        $('.items-loading-container').fadeOut(200, () => {
            displayItemsContainer();
        });
    }
}

function displayItemsContainer() {
    $('.items-container').fadeIn(200);
    if (shouldPaginationBeShown) {
        $('.pagination-container').fadeIn(200);
    }
}

function itemLoadingCompletedCallback() {
    itemsLoading = false;
}

function onItemsCountOnPageChange() {
    homeItemsViewModel.selectedPageNumber(1);
    loadItems();
}

function onPaginationElementClick(pageNumber) {
    homeItemsViewModel.selectedPageNumber(pageNumber);
    loadItems();
}

function onLeftArrowClick() {
    homeItemsViewModel.selectedPageNumber(homeItemsViewModel.selectedPageNumber() - 1);
    loadItems();
}

function onRightArrowClick() {
    homeItemsViewModel.selectedPageNumber(homeItemsViewModel.selectedPageNumber() + 1);
    loadItems();
}

function processPagination(itemsTotalCount) {
    var pagesCount = getPagesCount(itemsTotalCount);
    if (pagesCount >= 5) {
        var selectedPage = homeItemsViewModel.selectedPageNumber();
        if (homeItemsViewModel.selectedPageNumber() > 3) {
            if (pagesCount - homeItemsViewModel.selectedPageNumber() >= 2) {
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

function resetPaginationVisibility(pagesCount) {
    shouldPaginationBeShown = pagesCount > 1;
}

function resetPaginationElementsNumbers(rightNumber) {
    homeItemsViewModel.firstPaginationElement().number(rightNumber - 4);
    homeItemsViewModel.secondPaginationElement().number(rightNumber - 3);
    homeItemsViewModel.thirdPaginationElement().number(rightNumber - 2);
    homeItemsViewModel.thourthPaginationElement().number(rightNumber - 1);
    homeItemsViewModel.fifthPaginationElement().number(rightNumber);
}

function resetPaginationElementsNumbersIfPagesCountIsLessThen5(pagesCount) {
    homeItemsViewModel.firstPaginationElement().number(pagesCount >= 1 ? 1 : undefined);
    homeItemsViewModel.secondPaginationElement().number(pagesCount >= 2 ? 2 : undefined);
    homeItemsViewModel.thirdPaginationElement().number(pagesCount >= 3 ? 3 : undefined);
    homeItemsViewModel.thourthPaginationElement().number(pagesCount >= 4 ? 4 : undefined);
    homeItemsViewModel.fifthPaginationElement().number(pagesCount >= 5 ? 5 : undefined);
}

function resetArrowEnabling(pagesCount) {
    if (homeItemsViewModel.selectedPageNumber() === 1) {
        homeItemsViewModel.isLeftArrowDisabled(true);
    } else if (homeItemsViewModel.isLeftArrowDisabled()) {
        homeItemsViewModel.isLeftArrowDisabled(false);
    }

    if (homeItemsViewModel.selectedPageNumber() === pagesCount) {
        homeItemsViewModel.isRightArrowDisabled(true);
    } else if (homeItemsViewModel.isRightArrowDisabled()) {
        homeItemsViewModel.isRightArrowDisabled(false);
    }
}

function resetPaginationElementsMarkedState() {
    resetPaginationElemensMarkedState(homeItemsViewModel.firstPaginationElement());
    resetPaginationElemensMarkedState(homeItemsViewModel.secondPaginationElement());
    resetPaginationElemensMarkedState(homeItemsViewModel.thirdPaginationElement());
    resetPaginationElemensMarkedState(homeItemsViewModel.thourthPaginationElement());
    resetPaginationElemensMarkedState(homeItemsViewModel.fifthPaginationElement());
}

function resetPaginationElemensMarkedState(paginationElement) {
    if (paginationElement.number() === homeItemsViewModel.selectedPageNumber()) {
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
    return Math.ceil(itemsTotalCount / Number(homeItemsViewModel.itemsCountOnPage()));
}

function onAddItemToCartClick(item) {
    addItemToShoppingCart(item.Code, item.Price, item.Name, item.MainImageName);
}

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


function recalculateItemsLoadingContianerHeight() {
    const pageContainerOffsetTop = $('#homeItemsContainer').offset().top;
    const categoryContainerHeight = $('.categories-container').outerHeight(true);
    const itemsCountContainerHeight = $('.items-count-container ').outerHeight(true);

    const itemsLoadingSpiinnerHeight = $(window).height() - pageContainerOffsetTop - categoryContainerHeight - itemsCountContainerHeight - 7;

    $('.items-loading-container').height(itemsLoadingSpiinnerHeight + 'px');
}