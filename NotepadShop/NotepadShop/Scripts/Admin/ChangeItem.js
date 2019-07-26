var changeItemViewModel;

$(document).ready(function () {

    console.log(inputData);

    function createViewModel() {
        var self = this;
        self.nameRu = ko.observable(new inputItem(inputData.ruName));
        self.nameEn = ko.observable(new inputItem(inputData.engName));
        self.nameUk = ko.observable(new inputItem(inputData.ukrName));
        self.price = ko.observable(new inputItem(inputData.price));
        self.availableCategories = ko.observableArray(inputData.allCategories);
        self.selectedCategory = ko.observable(new inputItem(inputData.category));
        self.onInputChange = onInputChangeCallback;
        self.saveItem = function () {
            saveItem();
        };
        self.mainImage = ko.observable(new imageItem(undefined, inputData.mainImageName, '/Content/Item/Images/' + inputData.mainImageName));
        self.onMainImageChange = onMainImageChangeCallback;
        self.additionalmages = ko.observableArray(inputData.additionalImages.map(imageName => new imageItem(undefined, imageName, '/Content/Item/Images/' + imageName)));
        self.onAdditinalImageChange = onAdditionalImageChangeCallback;
        self.removeAdditionalImage = removeAdditionalImage;
        self.addNewImage = addAdditionalImage;

        self.additionalImagesToDelete = [];

        self.isChangeDetectionOn = false;
    };

    changeItemViewModel = new createViewModel();

    ko.applyBindings(changeItemViewModel, document.getElementById("changeItemContainer"));

    $('ul.tabs').tabs();

    changeItemViewModel.isChangeDetectionOn = true;
});

function onMainImageChangeCallback(element, event) {
    changeItemViewModel.mainImage().file = event.target.files[0];
    changeItemViewModel.mainImage().changed = true;
};

function onInputChangeCallback(observableElement) {
    if (changeItemViewModel.isChangeDetectionOn) {
        observableElement().changed = true;
    }
};

function onAdditionalImageChangeCallback(element, event) {
    this.file = event.target.files[0];
    element.changed = true;
    if (element.name) {
        changeItemViewModel.additionalImagesToDelete.push(this.name);
    }
};
function addAdditionalImage() {
    if (changeItemViewModel.additionalmages().length < 5) {
        changeItemViewModel.additionalmages.push(new imageItem(undefined, '', ''));
    }
}
function removeAdditionalImage(element) {
    changeItemViewModel.additionalmages.remove(element);
    if (element.name) {
        changeItemViewModel.additionalImagesToDelete.push(this.name);
    }
}

function saveItem() {
    if (!canItemBeSaved()) {
        alert("Item can't be saved because no field is changed");
        return;
    }

    if (changeItemViewModel.mainImage().changed || areAdditionalImagesAdded()) {
        $.when(uploadItemImages(), uploadMainImage())
            .done(() => saveChanges());
    } else {
        saveChanges();
    }
}

function canItemBeSaved() {
    return changeItemViewModel.price().changed || changeItemViewModel.selectedCategory().changed || changeItemViewModel.nameRu().changed ||
        changeItemViewModel.nameUk().changed || changeItemViewModel.nameEn().changed || changeItemViewModel.mainImage().changed ||
        areAdditionalImagesAdded() || (changeItemViewModel.additionalImagesToDelete !== undefined && changeItemViewModel.additionalImagesToDelete.length > 0);
}

function saveChanges() {
    var dataObject =
    {
        Code: inputData.code,
        NewPrice: changeItemViewModel.price().changed ? changeItemViewModel.price().value() : null,
        NewCategory: changeItemViewModel.selectedCategory().changed ? changeItemViewModel.selectedCategory().value() : null,
        NewRuName: changeItemViewModel.nameRu().changed ? changeItemViewModel.nameRu().value() : null,
        NewUkName: changeItemViewModel.nameUk().changed ? changeItemViewModel.nameUk().value() : null,
        NewEnName: changeItemViewModel.nameEn().changed ? changeItemViewModel.nameEn().value() : null,
        IsMainImageChanged: changeItemViewModel.mainImage().changed,
        AreAdditionalImagesAdded: areAdditionalImagesAdded(),
        AdditionalImagesToDeleteNames: changeItemViewModel.additionalImagesToDelete
    };

    var createItemPromise = $.ajax({
        type: "POST",
        url: '/items/api/change-item',
        contentType: "application/json",
        data: JSON.stringify({ itemData: dataObject, key: inputData.key }),
        dataType: "json"
    });

    createItemPromise.done(function () {
        alert("saved");
    });

    createItemPromise.fail(function () {
        alert("error!");
    });
}

function uploadItemImages() {
    var d = $.Deferred();
    if (window.FormData !== undefined) {
        if (areAdditionalImagesAdded()) {
            var data = new FormData();
            changeItemViewModel.additionalmages().filter(img => img.changed).forEach(function (item) {
                data.append("image", item.file);
            });

            data.append("__RequestVerificationToken", getAntiForgeryToken());
            data.append('key', inputData.key);

            var uploadImagesromise = $.ajax({
                type: "POST",
                url: '/items/api/change-item-upload-images',
                contentType: false,
                processData: false,
                data: data
            });

            uploadImagesromise.done(function () {
                d.resolve();
            });

            uploadImagesromise.fail(function () {
                alert("error!");
                d.reject();
            });
        } else {
            d.resolve();
        }
    } else {
        alert("Your browser doesn't allow to upload files HTML5!");
    }

    return d.promise();
}


function uploadMainImage() {
    var d = $.Deferred();
    if (window.FormData !== undefined) {
        if (changeItemViewModel.mainImage().changed) {
            var data = new FormData();
            data.append("mainImage", changeItemViewModel.mainImage().file);

            data.append("__RequestVerificationToken", getAntiForgeryToken());
            data.append('key', inputData.key);

            var uploadImagesromise = $.ajax({
                type: "POST",
                url: '/items/api/change-item-upload-main-image',
                contentType: false,
                processData: false,
                data: data
            });

            uploadImagesromise.done(function () {
                d.resolve();
            });

            uploadImagesromise.fail(function () {
                alert("error!");
                d.reject();
            });
        } else {
            d.resolve();
        }
    } else {
        alert("Your browser doesn't allow to upload files HTML5!");
    }

    return d.promise();
}

function areAdditionalImagesAdded() {
    return changeItemViewModel.additionalmages().some(img => img.changed);
}

function imageItem(file, name, src) {
    this.file = file;
    this.name = name;
    this.src = ko.observable(src);
    this.changed = false;
}

function inputItem(value) {
    this.value = ko.observable(value);
    this.changed = false;
}