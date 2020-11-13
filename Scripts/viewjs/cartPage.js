$(function () {
    /* Decriment product */
    $("a.incProduct").click(function (e) {
        e.preventDefault();

        var productId = $(this).data("id");
        var url = "/cart/IncrementProduct/";

        $.getJSON(url, { id: productId }, function (data) {
            $("td.qty" + productId).html(data.qty);
            $("span.topQty").html(data.totalqty);
            var price = data.qty * data.price;
            var priceHtml = price.toFixed(2);

            $("td.total" + productId + " span").html(priceHtml);
            
            var grandtotal = (data.grandtotal).toFixed(2);

            $("td.grandtotal span").html(grandtotal);
            $("span.topTotal").html(grandtotal);
        });
    });
/*-----------------------------------------------------------*/

    /* Decriment product */
    $("a.decProduct").click(function (e) {
        e.preventDefault();

        var $this = $(this);
        var productId = $(this).data("id");
        var url = "/cart/DecrementProduct/";

        $.getJSON(url, { id: productId }, function (data) {
            if (data.qty == 0) {
                $this.parent().fadeOut("fast", function () {
                    location.reload();
                });
            }
            else {
                $("td.qty" + productId).html(data.qty);
                $("span.topQty").html(data.totalqty);
                var price = data.qty * data.price;
                var priceHtml = price.toFixed(2);

                $("td.total" + productId + " span").html(priceHtml);

                var grandtotal = (data.grandtotal).toFixed(2);

                $("td.grandtotal span").html(grandtotal);
                $("span.topTotal").html(grandtotal);
            }
        });
    });
    /*-----------------------------------------------------------*/

    /* Remove product */
    $(function () {

        $("a.removeProduct").click(function (e) {
            e.preventDefault();

            var $this = $(this);
            var productId = $(this).data("id");
            var url = "/cart/RemoveProduct";

            $.get(url, { id: productId }, function (data) {
                location.reload();
            });
        });
    });
     /*-----------------------------------------------------------*/


});