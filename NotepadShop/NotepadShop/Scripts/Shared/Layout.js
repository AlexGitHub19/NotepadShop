var layoutViewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;
        self.email = ko.observable(email == "" ? undefined : email);
        self.isLoginModalVisible = ko.observable(false);
        self.loginBtnClick = loginBtnClickCallback;
        self.closeloginPopupClick = closeloginPopupClickCallback;
        self.loginEmail = ko.observable("");
        self.loginPasword = ko.observable("");
        self.submitLoginBtnEnabled = ko.computed(function () {
            return (self.loginEmail() !== undefined && self.loginEmail() !== "") &&
                   (self.loginPasword() !== undefined && self.loginPasword() !== "");
        });
        self.displayLogInPreloader = ko.observable(false);
        self.isLoggedIn = ko.computed(function () {
            return self.email() !== undefined && self.email() !== "";
        });
        self.loginFailedMessage = ko.observable("");
        self.showLoginFailedMessage = ko.observable(false);
        self.registerEmail = ko.observable("");
        self.registerPasword = ko.observable("");
        self.registerFailedMessage = ko.observable("");
        self.showRegisterFailedMessage = ko.observable(false);
        self.displayRegisterPreloader = ko.observable(false);

        self.languageCookieName = 'ns-language';
        self.shoppingCartCookieName = 'ns-shopping-cart';
        self.shoppingCartItems = ko.observableArray();
        self.isShoppingCartPopupVisible = ko.observable(false);
        self.shoppingCartTimer;
        self.showShoppingCartPopup = () => self.isShoppingCartPopupVisible(true);
        self.hideShoppingCartPopup = () => self.isShoppingCartPopupVisible(false);
        self.deleteItemFromShoppingCart = (item) => deleteItemFromShoppingCart(item);
        self.totaShoppingCartlPrice = ko.pureComputed(function () {
            let result = 0;
            this.shoppingCartItems().forEach(item => result += item.Price * item.Quantity());
            return result.toFixed(2);
        }, self);
        self.plusQuantity = (item) => {
            if (!item.Quantity()) {
                item.Quantity(1);
            } else if (Number.parseInt(item.Quantity()) < 99) {
                item.Quantity(Number.parseInt(item.Quantity()) + 1); 
                setNewItemQuantityToCookie(item.Code, item.Quantity());
            }
        };
        self.minusQuantity = (item) => {
            if (Number.parseInt(item.Quantity()) > 1) {
                item.Quantity(Number.parseInt(item.Quantity()) - 1); 
                setNewItemQuantityToCookie(item.Code, item.Quantity());
            }
        };
        self.isShoppingCartVisible = ko.observable(false);
        self.shoppingCartImageClick = function () {
            if (this.shoppingCartItems().length > 0) {
                $("body").css("overflow", "hidden");
                self.isShoppingCartVisible(true);
            }
        };
        self.countInputKeyDown = countInputKeyDownCallback;
        self.countInputInput = countInputInputCallback;
        self.shoppingCartCloseBtnClick = closeShoppingCartCallBack;

        self.shoppingCartName = ko.observable('');
        self.shoppingCartSurname = ko.observable('');
        self.shoppingCartPhone = ko.observable('');
        self.shoppingCartEmail = ko.observable('');
        self.shoppingCartCity = ko.observable('');
        self.shoppingCartPostDepartment = ko.observable('');
        self.shoppingCartPaymentType = ko.observable('cart');
        self.shoppingCartDeliveryType = ko.observable('novaPoshta');
        self.makeOrderClick = makeOrderClickCallBack;

        self.displaySessionExpiredWindow = ko.observable(false);
    };

    layoutViewModel = new createViewModel();


    createShoppingCartCookie();
    setShoppingCartValuesFromCookie();
    startShoppingCartCookieTimer();

    reSetLanguageCookieIfExists();

    ko.applyBindings(layoutViewModel, document.getElementById("layoutConainer"));

    registerEvents();

});

function loginBtnClickCallback() {
    layoutViewModel.isLoginModalVisible(true);
}

function closeloginPopupClickCallback() {
    layoutViewModel.isLoginModalVisible(false);
}

function registerEvents() {
    $('body').on('click', '#sumbitLogInBtn', function (e) {
        layoutViewModel.displayLogInPreloader(true);
        layoutViewModel.loginFailedMessage("");
        layoutViewModel.registerFailedMessage(false);
        $.ajax({
            type: "POST",
            url: logInUrl,
            data: { __RequestVerificationToken: getAntiForgeryToken(), Email: layoutViewModel.loginEmail(), Password: layoutViewModel.loginPasword() },
            success: function (result) {
                if (result.Email) {
                    if (result.Language) {
                        setLanguageCookieIfNotExists(result.Language);
                    }
                    location.reload();
                } else {
                    layoutViewModel.displayLogInPreloader(false);
                    layoutViewModel.loginFailedMessage(result.ErrorMessage);
                    layoutViewModel.showLoginFailedMessage(true);
                }
            },
            error: function () {
                layoutViewModel.displayLogInPreloader(false);
            }
        });
    });

    $('body').on('click', '#sumbitRegisterBtn', function (e) {

        if ($('#registerEmailInput').hasClass('valid') && $('#registerPasswordInput').hasClass('valid')) {
            layoutViewModel.displayRegisterPreloader(true);
            layoutViewModel.registerFailedMessage("");
            layoutViewModel.showLoginFailedMessage(false);

            $.ajax({
                type: "POST",
                url: registerUrl,
                data: { __RequestVerificationToken: getAntiForgeryToken(), Email: layoutViewModel.registerEmail(), Password: layoutViewModel.registerPasword() },
                success: function (result) {
                    if (result.Email) {
                        location.reload();
                    } else {
                        layoutViewModel.displayRegisterPreloader(false);
                        layoutViewModel.registerFailedMessage(result.ErrorMessage);
                        layoutViewModel.showRegisterFailedMessage(true);
                    }
                },
                error: function () {
                    layoutViewModel.displayRegisterPreloader(false);
                }
            });
        }
    });

    $('body').on('click', '#logOutBtn', function (e) {
        $.ajax({
            type: "POST",
            url: logOutUrl,
            data: { __RequestVerificationToken: getAntiForgeryToken()},
            success: function (result) {
                location.reload();
            }
        });
    });

    $('body').on('mouseenter', '#userInfoContainer', function (e) {
        if (layoutViewModel.isLoggedIn()) {
            $('#userInfoPopup').css('left', $('#userInfoContainer').offset().left);
            $('#userInfoPopup').css('width', $('#userInfoContainer').width());
            $('#userInfoPopup').css('display', 'block');
        }
    });

    $('body').on('mouseleave', '#userInfoContainer', function (e) {
        $('#userInfoPopup').css('display', 'none');
    });

    $('body').on('mouseenter', '#userInfoPopup', function (e) {
        $('#userInfoPopup').css('display', 'block');
    });

    $('body').on('mouseleave', '#userInfoPopup', function (e) {
        $('#userInfoPopup').css('display', 'none');
    });


    $('body').on('mouseenter', '#languageContainer', function (e) {
        $('#languagePopup').css('left', $('#languageContainer').offset().left);
        $('#languagePopup').css('display', 'block');
    });

    $('body').on('mouseleave', '#languageContainer', function (e) {
        $('#languagePopup').css('display', 'none');
    });

    $('body').on('mouseenter', '#languagePopup', function (e) {
        $('#languagePopup').css('display', 'block');
    });

    $('body').on('mouseleave', '#languagePopup', function (e) {
        $('#languagePopup').css('display', 'none');
    });


    //$('#userInfoContainer').on('focusout', '#passwordInput', function (e) {
    //    viewModel.showPasswordValidation($("#passwordInput").hasClass("invalid"));
    //})


    $('#languagePopup').on('click', '.popup-language-item', function (e) {
        var itemId = $(this).attr('id');
        var language = 'ru';
        if (itemId === 'popup-language-ru') {
            language = 'ru';
        } else if (itemId === 'popup-language-uk') {
            language = 'uk';
        } else if (itemId === 'popup-language-en') {
            language = 'en';
        }


        setLanguageCookie(language);
        $('#languagePopup').css('display', 'none');
        location.reload();
    });
};

function setLanguageCookie(language) {
    var date = new Date();
    date.setDate(date.getDate() + 2);
    document.cookie = layoutViewModel.languageCookieName + '=' + language + '; path=/; expires=' + date.toUTCString();
}

function reSetLanguageCookieIfExists() {
    if (isCookieExists(layoutViewModel.languageCookieName)) {
        setLanguageCookie(getCookie(layoutViewModel.languageCookieName));
    }
}

function setLanguageCookieIfNotExists(language) {
    if (!isCookieExists(layoutViewModel.languageCookieName)) {
        setLanguageCookie(language);
    }
}

function createShoppingCartCookie() {
    if (!isCookieExists(layoutViewModel.shoppingCartCookieName)) {
        setCartCookieValue('[]');
    } else {
        setCartCookieValue(getCookie(layoutViewModel.shoppingCartCookieName));
    }
}

function addItemToShoppingCart(itemCode, itemPrice, itemName, imageName) {
    const itemToAdd = new ShoppingCartItem(itemCode, itemPrice, itemName, 1, imageName);
    layoutViewModel.shoppingCartItems.push(itemToAdd);

    var cartCookieValue = JSON.parse(getCookie(layoutViewModel.shoppingCartCookieName));
    newCookieValue = cartCookieValue;
    newCookieValue.push(new CookieShoppingCartItem(itemToAdd.Code, itemToAdd.Price, itemToAdd.Name, itemToAdd.Quantity(), itemToAdd.ImageName));
    setCartCookieValue(JSON.stringify(newCookieValue));
}

function deleteItemFromShoppingCart(item) {
    layoutViewModel.shoppingCartItems.remove(item);

    if (layoutViewModel.shoppingCartItems().length === 0) {
        closeShoppingCartCallBack();
    }

    var cartCookieValue = JSON.parse(getCookie(layoutViewModel.shoppingCartCookieName));
    var newCookieValue = cartCookieValue.filter(cookieItem => cookieItem.Code !== item.Code);
    setCartCookieValue(JSON.stringify(newCookieValue));
}

function setCartCookieValue(newValue) {
    var date = new Date();
    date.setMinutes(date.getMinutes() + 5);
    document.cookie = layoutViewModel.shoppingCartCookieName + '=' + newValue + '; path=/; expires=' + date.toUTCString();
}

function setNewItemQuantityToCookie(itemCode, newQuantity) {
    var cartCookieValue = JSON.parse(getCookie(layoutViewModel.shoppingCartCookieName));
    cartCookieValue.filter(cookieItem => cookieItem.Code === itemCode)[0].Quantity = newQuantity;
    setCartCookieValue(JSON.stringify(cartCookieValue));
}

function isCookieExists(name) {
    return getCookie(name) !== undefined;
}

function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function setShoppingCartValuesFromCookie() {
    var items = JSON.parse(getCookie(layoutViewModel.shoppingCartCookieName));
    layoutViewModel.shoppingCartItems.removeAll();
    ko.utils.arrayPushAll(layoutViewModel.shoppingCartItems,
        items.map(item => new ShoppingCartItem(item.Code, item.Price, item.Name, item.Quantity, item.ImageName)));
}

function startShoppingCartCookieTimer() {
    let currentCookie = getCookie(layoutViewModel.shoppingCartCookieName);
    const checkCookiechanged = () => {
        if (!isCookieExists(layoutViewModel.shoppingCartCookieName)) {
            clearTimeout(layoutViewModel.shoppingCartTimer);
            layoutViewModel.displaySessionExpiredWindow(true);
        } else {
            const shoppingCartCookieValue = getCookie(layoutViewModel.shoppingCartCookieName);
            if (currentCookie !== shoppingCartCookieValue) {
                if (wasShoppingCartChangedInAnotherPage()) {
                    setShoppingCartValuesFromCookie();
                }
            }
            currentCookie = shoppingCartCookieValue;
        }
    }

    layoutViewModel.shoppingCartTimer = setInterval(checkCookiechanged, 1000);
}

function wasShoppingCartChangedInAnotherPage() {
    const shoppingCartCookieValue = getCookie(layoutViewModel.shoppingCartCookieName);
    const shoppingCartItemsValue = JSON.stringify(layoutViewModel.shoppingCartItems()
        .map(item => new CookieShoppingCartItem(item.Code, item.Price, item.Name, item.Quantity(), item.ImageName)));

    return shoppingCartCookieValue !== shoppingCartItemsValue;
}

function countInputKeyDownCallback(element, event) {
    if (event.shiftKey || (!isSymbolNumber(event.keyCode) && !isSymbolAdditionalSymbol(event.keyCode))) {
        event.preventDefault();
        return false;
    }
    return true;
}

function countInputInputCallback(element) {
    let inputValue = Number.parseInt(element.Quantity());
    if (!inputValue) {
        inputValue = '';
    }
    let newValue = inputValue;

    if (newValue > 99) {
        newValue = 99;
    }

    element.Quantity(newValue);
    setNewItemQuantityToCookie(element.Code, newValue);
    return true;
}

function closeShoppingCartCallBack() {
    $("body").css("overflow", "initial");
    layoutViewModel.isShoppingCartVisible(false);
}

function makeOrderClickCallBack() {
    var dataObject =
    {
        CustomerName: layoutViewModel.shoppingCartName(),
        CustomerSurname: layoutViewModel.shoppingCartSurname(),
        CustomerPhone: layoutViewModel.shoppingCartPhone(),
        CustomerEmail: layoutViewModel.shoppingCartEmail(),
        City: layoutViewModel.shoppingCartCity(),
        PostDepartment: layoutViewModel.shoppingCartPostDepartment(),
        PaymentType: layoutViewModel.shoppingCartPaymentType(),
        DeliveryType: layoutViewModel.shoppingCartDeliveryType(),
        Items: layoutViewModel.shoppingCartItems().map(cartItem => new CreateOrderItem(cartItem.Code, cartItem.Quantity()))
    };

    function CreateOrderItem(code, count) {
        this.Code = code;
        this.Count = count;
    }

    console.log(dataObject);
    console.log(JSON.stringify(dataObject));

    var createItemPromise = $.ajax({
        type: "POST",
        url: '/api/create-order',
        contentType: "application/json",
        data: JSON.stringify(dataObject),
        dataType: "json"
    });

    createItemPromise.done(function () {
        alert("saved");
    });

    createItemPromise.fail(function () {
        alert("error!");
    });
}

const numbers = [];
const keypadZero = 48;
const numpadZero = 96;

//add key codes for digits 0 - 9 into this filter
for (var i = 0; i <= 9; i++) {
    numbers.push(i + keypadZero);
    numbers.push(i + numpadZero);
}

//add other keys for editing the keyboard input
const additionalSymbol = [];
additionalSymbol.push(8);     //backspace
additionalSymbol.push(46);    //delete
additionalSymbol.push(37);    //left arrow
additionalSymbol.push(39);    //right arrow

function isSymbolNumber(keyCode) {
    return numbers.indexOf(keyCode) >= 0;
}

function isSymbolAdditionalSymbol(keyCode) {
    return additionalSymbol.indexOf(keyCode) >= 0;
}

function ShoppingCartItem(code, price, name, quantity, imageName) {
    this.Code = code;
    this.Price = price;
    this.Name = name;
    this.Quantity = ko.observable(quantity);
    this.ImageName = imageName;
}

function CookieShoppingCartItem(code, price, name, quantity, imageName) {
    this.Code = code;
    this.Price = price;
    this.Name = name;
    this.Quantity = quantity;
    this.ImageName = imageName;
}

function getAntiForgeryToken() {
    return $(".tokenContainer > input[type='hidden'][name$='RequestVerificationToken']").val();
};