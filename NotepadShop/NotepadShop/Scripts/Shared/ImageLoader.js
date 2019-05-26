$(document).ready(function () {
    $('body').on('click', '.loader-btn-contaner', function (e) {
        $(this).siblings('input').trigger('click');
    });

    $('body').on('change', '.loader-image-input', function (e) {
        var jInput = $(this);
        if (this.files && this.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                jInput.siblings('.loader-image-replacement').css('display', 'none');
                jInput.siblings('.loader-image').attr('src', e.target.result);
                jInput.siblings('.loader-image').css('display', 'unset');
            };

            reader.readAsDataURL(this.files[0]);
        }
    });
});
