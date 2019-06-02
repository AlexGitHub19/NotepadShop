var addNewItemViewModel;

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
        self.mainImage = ko.observable({ file: undefined, src: "" });
        self.onMainImageChange = onMainImageChangeCallback;
        self.additionalmages = ko.observableArray([]);
        self.onAdditinalImageChange = onAdditionalImageChangeCallback;
        self.removeAdditionalImage = removeAdditionalImage;
        self.addNewImage = addAdditionalImage;
    };

    addNewItemViewModel = new createViewModel();

    ko.applyBindings(addNewItemViewModel, document.getElementById("addNewItemContainer"));

    $('ul.tabs').tabs();
    $('select').material_select();
});

function onMainImageChangeCallback(elemet, event) {
    addNewItemViewModel.mainImage().file = event.target.files[0];
    console.log(addNewItemViewModel.mainImage().file);
};

function onAdditionalImageChangeCallback(elemet, event) {
    this.file = event.target.files[0];
};
function addAdditionalImage() {
    if (addNewItemViewModel.additionalmages().length < 5) {
        addNewItemViewModel.additionalmages.push({ file: undefined, src: "" });
    }
}
function removeAdditionalImage() {
    addNewItemViewModel.additionalmages.remove(this);
}

function createItem() {
    $.when(uploadImages())
        .done(function () {
            var dataObject =
            {
                Price: addNewItemViewModel.price(),
                Category: addNewItemViewModel.selectedCategory(),
                Names:
                    [
                        { Name: addNewItemViewModel.nameRu(), Language: 'ru' },
                        { Name: addNewItemViewModel.nameEn(), Language: 'en' },
                        { Name: addNewItemViewModel.nameUk(), Language: 'uk' }
                    ],
            };

            var createItemPromise = $.ajax({
                type: "POST",
                url: '/Items/CreateItem',
                contentType: "application/json",
                data: JSON.stringify({ item: dataObject, key: addItemKey }),
                dataType: "json"
            });

            createItemPromise.done(function () {
                alert("saved");
            });

            createItemPromise.fail(function () {
                alert("error!");
            });
        });
}

function uploadImages() {

    var d = $.Deferred();

    if (window.FormData !== undefined) {

        console.log(addNewItemViewModel.mainImage().file);
        if (addNewItemViewModel.mainImage().file != undefined) {
            console.log('inside');

            var data = new FormData();
            data.append("mainImage", addNewItemViewModel.mainImage().file);

            for (i = 0; i < addNewItemViewModel.additionalmages().length; i++) {
                data.append("image" + i, addNewItemViewModel.additionalmages()[i].file);
            }

            data.append("__RequestVerificationToken", getAntiForgeryToken());
            data.append('key', addItemKey);

            var uploadImagesromise = $.ajax({
                type: "POST",
                url: '/Items/UploadItemImages',
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
        }
    } else {
        alert("Your browser doesn't allow to upload files HTML5!");
    }

    return d.promise();
}

function changeFileName(file, newName) {
    var blob = file.slice(0, file.size, file.type);
    var newNameWithExtension = newName + file.name.substring(file.name.lastIndexOf('.'));
    return new File([blob], newNameWithExtension, { type: file.type });
}