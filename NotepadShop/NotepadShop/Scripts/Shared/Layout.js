﻿var layoutViewModel;

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

        //self.ruLanguageSelected = ko.observable(true);
        //self.ukLanguageSelected = ko.observable(false);
        //self.enLanguageSelected = ko.observable(false);
    };

    layoutViewModel = new createViewModel();

    ko.applyBindings(layoutViewModel, document.getElementById("layoutConainer"));

    $('#logInBtn').leanModal({
        ready: function () {
            $('ul.tabs').tabs();
        }
    });

    registerEvents();
});

//function selectLanguage() {
//    var languageCookieValue = getCookieValueByName('ns-language');
//    if (languageCookieValue) {
//        if (languageCookieValue === 'ru') {
//            viewModel.ruLanguageSelected(true);
//            viewModel.ukLanguageSelected(false);
//            viewModel.enLanguageSelected(false);
//        } else if (languageCookieValue === 'uk') {
//            viewModel.ruLanguageSelected(false);
//            viewModel.ukLanguageSelected(true);
//            viewModel.enLanguageSelected(false);
//        } else if (languageCookieValue === 'en') {
//            viewModel.ruLanguageSelected(false);
//            viewModel.ukLanguageSelected(false);
//            viewModel.enLanguageSelected(true);
//        }
//    }
//}

//function getCookieValueByName(name) {
//    var matches = document.cookie.match(new RegExp(
//        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
//    ));
//    return matches ? decodeURIComponent(matches[1]) : undefined;
//}

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

        var date = new Date();layoutViewModel
        date.setDate(date.getDate() + 2);
        document.cookie = 'ns-language=' + language + '; path=/; expires=' + date.toUTCString();
        $('#languagePopup').css('display', 'none');
        location.reload();
    });
};

function getAntiForgeryToken() {
    return $(".tokenContainer > input[type='hidden'][name$='RequestVerificationToken']").val();
};