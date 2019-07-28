$(document).ready(function () {
    var menuOverlayConatainer = $('.menu-overlay-container'),
        menuOverlay = $('.menu-overlay');
        nsMenu = $('.ns-menu');

    var frameProportion = 1.78,
        frames = 25,
        resize = false;

    setMenuOverlayDimensions();
    $(window).on('resize', function () {
        if (!resize) {
            resize = true;
            (!window.requestAnimationFrame) ? setTimeout(setMenuOverlayDimensions, 300) : window.requestAnimationFrame(setMenuOverlayDimensions);
        }
    });

    $('.menu-btn').on('click', function (event) {
        event.preventDefault();
        menuOverlayConatainer.addClass('visible opening');
        setTimeout(function () {
            nsMenu.addClass('visible');
            setTimeout(function () {
                $('.menu-close-btn').addClass('visible');
            }, 1300);
        }, 600);
    });

    $('#layoutConainer').on('click', '.menu-close-btn', function (event) {
        event.preventDefault();
        menuOverlayConatainer.addClass('closing');
        nsMenu.removeClass('visible');
        $(this).removeClass('visible');
        menuOverlay.one('webkitAnimationEnd oanimationend msAnimationEnd animationend', function () {
            menuOverlayConatainer.removeClass('closing opening visible');
            menuOverlay.off('webkitAnimationEnd oanimationend msAnimationEnd animationend');
        });
    });

    function setMenuOverlayDimensions() {
        var windowWidth = $(window).width(),
            windowHeight = $(window).height(),
            layerHeight, layerWidth;

        if (windowWidth / windowHeight > frameProportion) {
            layerWidth = windowWidth;
            layerHeight = layerWidth / frameProportion;
        } else {
            layerHeight = windowHeight * 1.2;
            layerWidth = layerHeight * frameProportion;
        }

        menuOverlay.css({
            'width': layerWidth * frames + 'px',
            'height': layerHeight + 'px',
        });

        resize = false;
    }


    nsMenu.on("click", "a", function () {
        $('.menu-close-btn').removeClass('visible');
        var newUrl = $(this).attr("href");
        $("html").fadeOut(function () {
            location = newUrl;
        });
        return false;
    });
});
