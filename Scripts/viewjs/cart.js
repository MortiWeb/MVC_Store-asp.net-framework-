$(function () {
    $("a.addtocart").click(function (e) {
        e.preventDefault();
        var clickId = $(this).attr('id');
        var url = "/cart/CartPartial/";
        $(this).next(".ajaxmsg").addClass("ib");
        $.get(url, { id: clickId }, function myfunction(data) {
            $(".ajaxcart").html(data);
        }).done(function () {
            setTimeout(function () {
                $("div.ajaxmsg").fadeOut("fast");
                $("div.ajaxmsg").removeClass("ib");
            }, 2000);
        });
    });
});