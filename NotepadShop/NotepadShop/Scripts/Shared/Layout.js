var layoutViewModel;

$(document).ready(function () {

    function createViewModel() {
        var self = this;
        self.email = ko.observable(email == "" ? undefined : email);
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

        self.shoppingCartCookieName = 'ns-shopping-cart';
        self.shoppingCartItems = ko.observableArray();
        self.showShoppingCart = ko.observable(false);
        self.showShoppingCartPopup = () => self.showShoppingCart(true);
        self.hideShoppingCartPopup = () => self.showShoppingCart(false);
        self.deleteItemFromShoppingCart = (item) => deleteItemFromShoppingCart(item);
        self.totaShoppingCartlPrice = ko.pureComputed(function () {
            let result = 0;
            this.shoppingCartItems().forEach(item => result += item.Price * item.Quantity());
            return result.toFixed(2);
        }, self);
        self.plusQuantity = (item) => {
            if (item.Quantity() < 99) {
                item.Quantity(item.Quantity() + 1); 
                setNewItemQuantityToCookie(item.Code, item.Quantity());
            }
        };
        self.minusQuantity = (item) => {
            if (item.Quantity() > 1) {
                item.Quantity(item.Quantity() - 1); 
                setNewItemQuantityToCookie(item.Code, item.Quantity());
            }
        };
    };

    layoutViewModel = new createViewModel();

    $('#logInBtn').leanModal({
        ready: function () {
            $('ul.tabs').tabs();
        }
    });

    createShoppingCartCookie();
    var items = JSON.parse(getCookie(layoutViewModel.shoppingCartCookieName));
    ko.utils.arrayPushAll(layoutViewModel.shoppingCartItems,
        items.map(item => new ShoppingCartItem(item.Code, item.Price, item.Name, item.Quantity, item.ImageName)));
    startShoppingCartCookieTimer();

    ko.applyBindings(layoutViewModel, document.getElementById("layoutConainer"));

    registerEvents();

});

function startShoppingCartCookieTimer() {
    let currentCookie = getCookie(layoutViewModel.shoppingCartCookieName);
    const checkCookiechanged = () => {
        const shoppingCartCookieValue = getCookie(layoutViewModel.shoppingCartCookieName);
        if (currentCookie !== shoppingCartCookieValue) {
            console.log('changed');
        }
        currentCookie = shoppingCartCookieValue;
    }
    setInterval(checkCookiechanged, 1000);
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
        console.log(getAntiForgeryToken());
        $.ajax({
            type: "POST",
            url: logOutUrl,
            data: { __RequestVerificationToken: getAntiForgeryToken()},
            success: function (result) {
                location.reload();
            }
        });
    });

    $('body').on('click', '#logInBtn', function (e) {
        $('#loginModal').openModal();
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

    $('body').on('click', '#logInBtn', function (e) {
        $('#loginModal').openModal();
    });

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

        var date = new Date();
        date.setDate(date.getDate() + 2);
        document.cookie = 'ns-language=' + language + '; path=/; expires=' + date.toUTCString();
        $('#languagePopup').css('display', 'none');
        location.reload();
    });
};

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