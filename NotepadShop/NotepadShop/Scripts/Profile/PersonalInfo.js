var profilePersonalInfoModel;
const notFilled = 'Not заполнено';

$(document).ready(function () {

    function createViewModel() {
        var self = this;
        self.userFirstName = ko.observable('');
        self.userSurnameName = ko.observable('');
        self.userPhone = ko.observable('');
        self.userEmail = ko.observable('');
        self.userCity = ko.observable('');
        self.userPostDepartment = ko.observable('');
        self.userLanguage = ko.observable('none');
        self.userLanguageString = ko.observable('');
        self.changeUserInfoClick = changeUserInfoClickCalback;
        self.saveUserInfoClick = saveUserInfoClickCallback;
        self.cancelUserInfoClick = cancelUserInfoClickCallback;

        self.isLabelPhoneIsNotValidShown = ko.observable(false);
        self.phoneInputFocusIn = () => self.isLabelPhoneIsNotValidShown(false);

        self.postDepartmentInputKeyDown = numberInputKeyDownCallback;

        self.isChangeModeEnabled = ko.observable(false);
        self.isUserDataLoaded = ko.observable(false);
        self.loadedUserData = undefined;

        self.isChangePasswordContainerVisible = ko.observable(false);
        self.isUserDataContainerVisible = ko.computed(function () {
            return self.isUserDataLoaded() && !self.isChangePasswordContainerVisible();
        });

        self.isUserActionsContainerVisible = ko.computed(function () {
            return self.isUserDataLoaded() && !self.isChangePasswordContainerVisible() && !self.isChangeModeEnabled();
        });

        self.oldPassword = ko.observable('');
        self.newPassword = ko.observable('');
        self.newPasswordConfirm = ko.observable('');
        self.showOldPasswordIsEmpty = ko.observable(false);
        self.showNewPasswordIsEmpty = ko.observable(false);
        self.showNewPasswordConfirmIsEmpty = ko.observable(false);
        self.showNePasswordIsNotValid = ko.observable(false);
        self.showNewPasswordAndConfirmAreNotMatched = ko.observable(false);
        self.changePasswordClick = changePasswordClickCallback;
        self.oldPasswordFocusIn = () => self.showOldPasswordIsEmpty(false);
        self.newPasswordFocusIn = () => { self.showNewPasswordIsEmpty(false); self.showNePasswordIsNotValid(false); self.showNewPasswordAndConfirmAreNotMatched(false); }
        self.newPasswordConfirmFocusIn = () => { self.showNewPasswordConfirmIsEmpty(false); self.showNewPasswordAndConfirmAreNotMatched(false); }
        self.savePasswordChangingClick = savePasswordChangingClickCallback;
        self.cancelPasswordChangingClick = cancelPasswordChangingClickCallback;

        self.passwordInputPaste = (element, event) => event.preventDefault();

        self.logoutClick = logout;
    };

    profilePersonalInfoModel = new createViewModel();
    ko.applyBindings(profilePersonalInfoModel, document.getElementById("profilePersonalInfoContainer"));
    loadUserInfo();
});

function loadUserInfo() {
    var loadUserInfoPromise = $.ajax({
        type: "GET",
        url: '/profile/api/personal-info',
        contentType: "application/json",
        dataType: "json"
    });

    loadUserInfoPromise.done(function (result) {
        profilePersonalInfoModel.loadedUserData = result;
        setUserDataFromLoadedData();

        profilePersonalInfoModel.isUserDataLoaded(true);
    });

    loadUserInfoPromise.fail(function () {
        alert("error!");
    });
}

function changeUserInfoClickCalback() {
    profilePersonalInfoModel.isChangeModeEnabled(true);
}

function saveUserInfoClickCallback() {
    defaultValuesIfItIsEmpty();

    const data = profilePersonalInfoModel.loadedUserData;

    const isFirstNameChanged = profilePersonalInfoModel.userFirstName() !== data.FirstName;
    const newName = new ChangePersonalInfoItemData(profilePersonalInfoModel.userFirstName(), isFirstNameChanged);

    const isSurnameChanged = profilePersonalInfoModel.userSurnameName() !== data.Surname;
    const newSurname = new ChangePersonalInfoItemData(profilePersonalInfoModel.userSurnameName(), isSurnameChanged);

    const isPhoneChanged = profilePersonalInfoModel.userPhone() !== data.Phone;
    const newPhone = new ChangePersonalInfoItemData(profilePersonalInfoModel.userPhone(), isPhoneChanged);

    if (profilePersonalInfoModel.userPhone() && !isPhoneNumberValid(profilePersonalInfoModel.userPhone())) {
        profilePersonalInfoModel.isLabelPhoneIsNotValidShown(true);
        return;
    }

    const isCityChanged = profilePersonalInfoModel.userCity() !== data.City;
    const newCity = new ChangePersonalInfoItemData(profilePersonalInfoModel.userCity(), isCityChanged);

    const isPostDepartmentChanged = profilePersonalInfoModel.userPostDepartment() !== data.PostDepartment;
    const newPostDepartment = new ChangePersonalInfoItemData(profilePersonalInfoModel.userPostDepartment(), isPostDepartmentChanged);

    const isLanguageChanged = isLanguageWasChanged();
    const assembledNewLanguage = assembleLanguageInputValue(profilePersonalInfoModel.userLanguage());
    const newLanguage = new ChangePersonalInfoItemData(assembledNewLanguage, isLanguageChanged);

    var isSomeFieldChanged = isFirstNameChanged || isSurnameChanged || isPhoneChanged || isCityChanged || isPostDepartmentChanged || isLanguageChanged;

    if (isSomeFieldChanged) {
        var dataObject =
        {
            NewFirstName: newName,
            NewSurname: newSurname,
            NewCity: newCity,
            NewPostDepartment: newPostDepartment,
            NewPhone: newPhone,
            NewLanguage: newLanguage
        };

        var createItemPromise = $.ajax({
            type: "POST",
            url: '/profile/api/change-personal-info',
            contentType: "application/json",
            data: JSON.stringify({ data: dataObject }),
            dataType: "json"
        });

        createItemPromise.done(function () {
            alert("saved");
            profilePersonalInfoModel.isChangeModeEnabled(false);
        });

        createItemPromise.fail(function () {
            alert("error!");
            profilePersonalInfoModel.isChangeModeEnabled(false);
        });
    } else {
        profilePersonalInfoModel.isChangeModeEnabled(false);
    }
}

function isLanguageWasChanged() {
    if (profilePersonalInfoModel.loadedUserData.Language == null && profilePersonalInfoModel.userLanguage() === 'none') {
        return false;
    }
    return profilePersonalInfoModel.userLanguage() !== profilePersonalInfoModel.loadedUserData.Language;
}

function defaultValuesIfItIsEmpty() {
    if (profilePersonalInfoModel.userFirstName() == '') {
        profilePersonalInfoModel.userFirstName(null);
    }

    if (profilePersonalInfoModel.userSurnameName() == '') {
        profilePersonalInfoModel.userSurnameName(null);
    }

    if (profilePersonalInfoModel.userPhone() == '') {
        profilePersonalInfoModel.userPhone(null);
    }

    if (profilePersonalInfoModel.userCity() == '') {
        profilePersonalInfoModel.userCity(null);
    }

    if (profilePersonalInfoModel.userPostDepartment() == '') {
        profilePersonalInfoModel.userPostDepartment(null);
    }
}

function cancelUserInfoClickCallback() {
    setUserDataFromLoadedData();
    profilePersonalInfoModel.isChangeModeEnabled(false);
}

function setUserDataFromLoadedData() {
    const data = profilePersonalInfoModel.loadedUserData;
    profilePersonalInfoModel.userFirstName(data.FirstName);
    profilePersonalInfoModel.userSurnameName(data.Surname);
    profilePersonalInfoModel.userPhone(data.Phone);
    profilePersonalInfoModel.userEmail(email);
    profilePersonalInfoModel.userCity(data.City);
    profilePersonalInfoModel.userPostDepartment(data.PostDepartment);
    profilePersonalInfoModel.userLanguageString(assembleLanguage(data.Language));
    if (data.Language) {
        profilePersonalInfoModel.userLanguage(data.Language);
    }
}

function assembleLanguage(shortLanguage) {
    if (!shortLanguage) {
        return undefined;
    }

    let result;
    switch (shortLanguage) {
        case 'ru':
            result = "Русский";
            break;
        case 'en':
            result = "English";
            break;
        case 'uk':
            result = "Українська";
            break;
        default:
            alert('ERRROR! type ' + shortLanguage + 'is not suppoted');
            break;
    }

    return result;
}

function assembleLanguageInputValue(languageInputValue) {
    if (languageInputValue == 'none') {
        return null;
    } else {

        return languageInputValue;
    }
}

function changePasswordClickCallback() {
    profilePersonalInfoModel.isChangePasswordContainerVisible(true);
}

function savePasswordChangingClickCallback() {
    if (!validatePasswordChanging()) {
        return
    }

    var changePasswordPromise = $.ajax({
        type: "POST",
        url: '/account/api/change-password',
        data: { __RequestVerificationToken: getAntiForgeryToken(), oldPassword: profilePersonalInfoModel.oldPassword(), newPassword: profilePersonalInfoModel.newPassword() },
    });

    changePasswordPromise.done(function (result) {
        if (result != true) {
            alert("fail");
        } else {
            closeChangePasswordContainer();
        }
    });

    changePasswordPromise.fail(function () {
        alert("error!");
    });
}

function validatePasswordChanging() {
    let result = true;
    if (!profilePersonalInfoModel.oldPassword()) {
        profilePersonalInfoModel.showOldPasswordIsEmpty(true);
        result = false;
    }

    if (!profilePersonalInfoModel.newPassword()) {
        profilePersonalInfoModel.showNewPasswordIsEmpty(true);
        result = false;
    } else if (!isPasswordValid(profilePersonalInfoModel.newPassword())) {
        profilePersonalInfoModel.showNePasswordIsNotValid(true);
        result = false;
    }

    if (!profilePersonalInfoModel.newPasswordConfirm()) {
        profilePersonalInfoModel.showNewPasswordConfirmIsEmpty(true);
        result = false;
    }

    if (profilePersonalInfoModel.newPassword() !== profilePersonalInfoModel.newPasswordConfirm()) {
        profilePersonalInfoModel.showNewPasswordAndConfirmAreNotMatched(true);
        result = false;
    }

    return result;
}

function isPasswordValid(password) {
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/.test(password);
}

function cancelPasswordChangingClickCallback() {
    closeChangePasswordContainer();
}

function closeChangePasswordContainer() {
    profilePersonalInfoModel.isChangePasswordContainerVisible(false);
    profilePersonalInfoModel.oldPassword('');
    profilePersonalInfoModel.newPassword('');
    profilePersonalInfoModel.newPasswordConfirm('');
}

function ChangePersonalInfoItemData(newValue, isChanged) {
    this.NewValue = newValue;
    this.IsChanged = isChanged;
}