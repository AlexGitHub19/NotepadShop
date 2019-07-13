var homeItemsiewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;
        self.addItemToCartClick = onAddItemToCartClick;
    };

    homeItemsiewModel = new createViewModel();

    ko.applyBindings(homeItemsiewModel, document.getElementById("homeItemContainer"));
});

function onAddItemToCartClick() {
    addItemToShoppingCart(homeItemCode, homeItemPrice, homeItemName, homeItemMainImgName);
}