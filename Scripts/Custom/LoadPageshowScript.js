$(window).on('load pageshow', function () {
    $('body').fadeIn();
});
$("a:not([href*=javascript]):not([href*=\\#]):not(.fancybox):not([target]):not([data-fancybox])").click(function () {
    $('body').fadeOut();
    let url = $(this).attr('href');
    window.setTimeout(function () {
        window.location.href = url;
    }, 1000);
    return false;
});