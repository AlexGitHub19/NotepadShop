$(document).ready(function () {
    $('body').on('click', '.loader-btn-contaner', function (e) {
        $(this).siblings('input').trigger('click');
    });

    $('body').on('change', '.loader-image-input', function (e) {
        var jInput = $(this);
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            var imageFile = this.files[0];

            reader.onload = function (e) {
                jInput.siblings('.loader-image-replacement').css('display', 'none');
                jInput.siblings('.loader-image').attr('src', e.target.result);
                jInput.siblings('.loader-image').css('display', 'unset');
                jInput.siblings('.loader-btn-contaner').find('.loader-btn').html('edit');

                var sizeString = createImageSizeString(imageFile.size);
                jInput.siblings('.image-size-container').html(sizeString);
                jInput.siblings('.image-size-container').css('display', 'block');
            };

            reader.readAsDataURL(imageFile);
        }
    });

    function createImageSizeString(imageSize) {
        var sizeString;
        if (imageSize < 1000) {
            sizeString = imageSize.toString().substring(0, 4) + " bytes"
        } else if (imageSize < 1000000) {
            sizeString = (imageSize / 1000).toPrecision(3) + " Kb"
        }
        else {
            sizeString = (imageSize / 1000000).toPrecision(3) + " MB"
        }

        return sizeString;
    }
});
