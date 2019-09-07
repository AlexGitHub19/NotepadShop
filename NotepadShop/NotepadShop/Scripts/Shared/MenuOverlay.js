let leftTranslateXValue;
let rightTranslateXValue;

let centralElementWidth;

$(document).ready(function () {
    recalculateRatio();

    var nsMenu = $('.ns-menu');


    //$(window).on('resize', function () {
    //    if (!resize) {
    //        resize = true;
    //        (!window.requestAnimationFrame) ? setTimeout(setMenuOverlayDimensions, 300) : window.requestAnimationFrame(setMenuOverlayDimensions);
    //    }
    //});

    $('.menu-btn').on('click', function (event) {
        event.preventDefault();
        $('body').addClass('menu-opened');
        openMenu();
        setTimeout(function () {
            nsMenu.addClass('visible');
            setTimeout(function () {
                $('.menu-close-btn').addClass('visible');
            }, 1300);
        }, 600);
    });

    $('body').on('click', '.menu-close-btn', function (event) {
        event.preventDefault();
        nsMenu.removeClass('visible');
        closeMenu();
        $(this).removeClass('visible');
    });

    nsMenu.on("click", "a", function () {
        $('.menu-close-btn').removeClass('visible');
        var newUrl = $(this).attr("href");
        $("html").fadeOut(function () {
            location = newUrl;
        });
        return false;
    });


    $(window).resize(function () {
        recalculateRatio();
    })
});


function closeMenu() {
    $('.overlay-center').addClass('color');
    $('.overlay-left').addClass('color');
    $('.overlay-right').addClass('color');


    if ($('.overlay-center').first().hasClass('open')) {
        $('.overlay-center').removeClass('open');
        $('.overlay-left').removeClass('open');
        $('.overlay-right').removeClass('open');
    } else {
        $('.overlay-center').removeClass('small-open');
        $('.overlay-left').removeClass('small-open');
        $('.overlay-right').removeClass('small-open');
    }

    setTimeout(() => {
        $('.overlay-center').removeClass('color');
        $('.overlay-left').removeClass('color');
        $('.overlay-right').removeClass('color');
    }, 700);

    setTimeout(() => {
        $('.overlay-background').fadeOut();
        setTimeout(() => {
            $('body').removeClass('menu-opened');
        }, 200);
    }, 500);
}

function openMenu() {
    if (centralElementWidth == 'normal') {
        $('.overlay-center').addClass('open');
        $('.overlay-left').addClass('open');
        $('.overlay-right').addClass('open');
    } else {
        $('.overlay-center').addClass('small-open');
        $('.overlay-left').addClass('small-open');
        $('.overlay-right').addClass('small-open');
    }

    setTimeout(() => {
        $('.overlay-background').css('display', 'unset');
    }, 700);
}

function recalculateRatio() {
    leftTranslateXValue = -188.2;
    rightTranslateXValue = 88.2;
    centralElementWidth = 'normal';
    let windowRatio = $(window).width() / $(window).height();
    if (windowRatio < 0.7) {
        leftTranslateXValue = -242;
        rightTranslateXValue = 142;
        centralElementWidth = 'small';
    } else if (windowRatio < 1.082) {
        leftTranslateXValue = -222;
        rightTranslateXValue = 122;
        centralElementWidth = 'small';
    } else if (windowRatio < 1.62) {
        leftTranslateXValue = -201;
        rightTranslateXValue = 101;
        centralElementWidth = 'small';
    }

    $('.overlay-left').css('transform', 'translateY(-50%) translateX(' + leftTranslateXValue + '%) rotate(45deg)');
    $('.overlay-right').css('transform', 'translateY(-50%) translateX(' + rightTranslateXValue + '%) rotate(45deg)');
}



//function closeMenuOverlay() {
//    $('.menu-overlay-container').addClass('closing');
//    $('.menu-overlay-container').one('webkitAnimationEnd oanimationend msAnimationEnd animationend', function () {
//        $('.menu-overlay-container').removeClass('closing opening visible');
//        $('.menu-overlay-container').off('webkitAnimationEnd oanimationend msAnimationEnd animationend');
//    });
//}
