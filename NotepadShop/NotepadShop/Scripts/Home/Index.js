$(document).ready(function () {
    $('.notepad-container').css('width', Math.trunc($('.notepad-container').innerWidth()) + 'px');

    startSvgAnimation();

    openMenuOverlay();

    setTimeout(function () {
        $('.main-header-overlay').addClass('hidden');
        closeMenuOverlay();
    }, 7000);
});

$('.cover').click(function () {
    $('body').addClass('open');
    $('.notepad-container').addClass('open');
    $('.cover').addClass('open');
    $('.left-part').addClass('open');
    $('.right-part').addClass('open');
});

$('.notepad-close-btn').click(function () {
    $('body').removeClass('open');
    $('.cover').removeClass('open');
    $('.right-part').removeClass('open');
    $('.left-part').removeClass('open');
    $('.notepad-container').removeClass('open');
})

//svg animation
function startSvgAnimation() {
    var tmax_optionsGlobal = {
        repeat: 1,
        repeatDelay: 1.65,
        yoyo: true
    };

    CSSPlugin.useSVGTransformAttr = true;

    var timeLine = new TimelineMax(tmax_optionsGlobal),
        stagger_val = 0.01,
        duration = 1;

    var itemsArray = [];
    $.each($('.main-header-svg *'), function (i, el) {
        itemsArray.push($(this));
    });
    var newArray = itemsArray.reverse();

    $('.main-header-svg').css('display', 'unset');

    $.each(newArray, function (i, el) {
        timeLine.set($(this), {
            x: '+=' + getRandomValue(-5500, 5500),
            y: '+=' + getRandomValue(-5500, 5500),
            rotation: '+=' + getRandomValue(-420, 420),
            scale: 0,
            opacity: 0
        });
    });

    function getRandomValue(min, max) {
        return Math.random() * (max - min) + min;
    }

    var stagger_opts_to = {
        x: 0,
        y: 0,
        opacity: 1,
        scale: 1,
        rotation: 0,
        ease: Power4.easeInOut
    };


    timeLine.staggerTo(newArray, duration, stagger_opts_to, stagger_val);

    setTimeout(function () {
        timeLine.timeScale(0.15);
    }, 5000);

    setTimeout(function () {
        timeLine.timeScale(1);
    }, 6500);
}