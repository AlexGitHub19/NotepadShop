$(document).ready(function () {

    $('body').on('paste', '.ns-phone-input', function (e) {
        e.preventDefault();
    });

    const pfoneInputsStateBeforeKeydown = new Map();
    const phonePrefix = '+38(0';

    $('body').on('input', '.ns-phone-input', function (e) {
        const value = $(this).val();
        if (!value.startsWith(phonePrefix)) {
            $(this).val((pfoneInputsStateBeforeKeydown.get(this)));
            return;
        }

        if (value.length === 5) {
            return;
        }

        const lasSymbolBeforeKeyUp = pfoneInputsStateBeforeKeydown.get(this)[pfoneInputsStateBeforeKeydown.get(this).length - 1];
        const postfix = value.slice(5, value.length);
        let formattedValue = postfix.replace(/-/g, "").replace(/\)/g, "");
        if (formattedValue.length === 1) {
            $(this).val(phonePrefix + formattedValue);
        } else if (formattedValue.length === 2) {
            if (lasSymbolBeforeKeyUp === '-') {
                $(this).val(phonePrefix + formattedValue[0]);
            } else {
                $(this).val(phonePrefix + formattedValue + ')-');
            }
        } else if (formattedValue.length === 3) {
            $(this).val(phonePrefix + formattedValue.slice(0, 2) + ')-' + formattedValue[2]);
        } else if (formattedValue.length === 4) {
            if (lasSymbolBeforeKeyUp === '-') {
                $(this).val(phonePrefix + formattedValue.slice(0, 2) + ')-' + formattedValue[2]);
            } else {
                $(this).val(phonePrefix + formattedValue.slice(0, 2) + ')-' + formattedValue.slice(2, 4) + '-');
            }
        } else if (formattedValue.length === 5) {
            $(this).val(phonePrefix + formattedValue.slice(0, 2) + ')-' + formattedValue.slice(2, 4) + '-' + formattedValue[4]);
        } else if (formattedValue.length === 6) {
            if (lasSymbolBeforeKeyUp === '-') {
                $(this).val(phonePrefix + formattedValue.slice(0, 2) + ')-' + formattedValue.slice(2, 4) + '-' + formattedValue[4]);
            } else {
                $(this).val(phonePrefix + formattedValue.slice(0, 2) + ')-' + formattedValue.slice(2, 4) + '-' + formattedValue.slice(4, 6) + '-');
            }
        } else if (formattedValue.length === 7) {
            $(this).val(phonePrefix + formattedValue.slice(0, 2) + ')-' + formattedValue.slice(2, 4) + '-' + formattedValue.slice(4, 6) + '-' + formattedValue[6]);
        } else if (formattedValue.length === 8) {
            $(this).val(phonePrefix + formattedValue.slice(0, 2) + ')-' + formattedValue.slice(2, 4) + '-' + formattedValue.slice(4, 6) + '-' + + formattedValue.slice(6, 8));
        } else if (formattedValue.length === 9) {
            $(this).val(phonePrefix + formattedValue.slice(0, 2) + ')-' + formattedValue.slice(2, 4) + '-' + formattedValue.slice(4, 6) + '-' + + formattedValue.slice(6, 9));
        }
    });

    $('body').on('keydown', '.ns-phone-input', function (e) {
        if (e.shiftKey || (!isSymbolNumber(e.keyCode) && !isSymbolAdditionalSymbol(e.keyCode))) {
            e.preventDefault();
            return false;
        }

        const value = $(this).val();
        pfoneInputsStateBeforeKeydown.set(this, value);

        if (isSymbolNumber(e.keyCode) && value.length === 18) {
            e.preventDefault();
            return false;
        }
    });

    $('body').on('focusin', '.ns-phone-input', function (e) {
        if (!$(this).val()) {
            $(this).val(phonePrefix);
        }
    });

    $('body').on('focusout', '.ns-phone-input', function (e) {
        if ($(this).val() === phonePrefix) {
            $(this).val('');
        }
    });
});
