﻿var viewModel;

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
    };

    viewModel = new createViewModel();
    ko.applyBindings(viewModel, document.getElementById("user-login-container"));

    $('#logInBtn').leanModal({
        ready: function () {
            $('ul.tabs').tabs();
        }
    });

    registerEvents();
});

function registerEvents() {
    $('body').on('click', '#sumbitLogInBtn', function (e) {
        viewModel.displayLogInPreloader(true);
        viewModel.loginFailedMessage("");
        viewModel.registerFailedMessage(false);
        $.ajax({
            type: "POST",
            url: logInUrl,
            data: { __RequestVerificationToken: getAntiForgeryToken(), Email: viewModel.loginEmail(), Password: viewModel.loginPasword() },
            success: function (result) {
                if (result.Email) {
                    location.reload();
                } else {
                    viewModel.displayLogInPreloader(false);
                    viewModel.loginFailedMessage(result.ErrorMessage);
                    viewModel.showLoginFailedMessage(true);
                }
            },
            error: function () {
                viewModel.displayLogInPreloader(false);
            }
        });
    });

    $('body').on('click', '#sumbitRegisterBtn', function (e) {

        if ($('#registerEmailInput').hasClass('valid') && $('#registerPasswordInput').hasClass('valid')) {
            viewModel.displayRegisterPreloader(true);
            viewModel.registerFailedMessage("");
            viewModel.showLoginFailedMessage(false);

            $.ajax({
                type: "POST",
                url: registerUrl,
                data: { __RequestVerificationToken: getAntiForgeryToken(), Email: viewModel.registerEmail(), Password: viewModel.registerPasword() },
                success: function (result) {
                    if (result.Email) {
                        location.reload();
                    } else {
                        viewModel.displayRegisterPreloader(false);
                        viewModel.registerFailedMessage(result.ErrorMessage);
                        viewModel.showRegisterFailedMessage(true);
                    }
                },
                error: function () {
                    viewModel.displayRegisterPreloader(false);
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

    $('body').on('click', '#logInBtn', function (e) {
        $('#loginModal').openModal();
    });

    $('body').on('mouseover', '#userLoginContainer', function (e) {
        $('#userInfoPopUp').css('display', 'block');
    });

    $('body').on('mouseleave', '#userLoginContainer', function (e) {
        $('#userInfoPopUp').css('display', 'none');
    });

    $('body').on('mouseover', '#userInfoPopUp', function (e) {
        $('#userInfoPopUp').css('display', 'block');
    });

    $('body').on('mouseleave', '#userInfoPopUp', function (e) {
        $('#userInfoPopUp').css('display', 'none');
    });


    //$('#userInfoContainer').on('focusout', '#passwordInput', function (e) {
    //    viewModel.showPasswordValidation($("#passwordInput").hasClass("invalid"));
    //})
};

function getAntiForgeryToken() {
    return $(".tokenContainer > input[type='hidden'][name$='RequestVerificationToken']").val();
};