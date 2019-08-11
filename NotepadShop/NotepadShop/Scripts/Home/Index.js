$(document).ready(function () {
   $('.notepad-container').css('width', Math.trunc($('.notepad-container').innerWidth()) + 'px');
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