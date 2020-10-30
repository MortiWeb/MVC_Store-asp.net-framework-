$(function () {
    /* Confirm page deletion */
    $("a.delete-page").click(function () {
        if (!confirm("Confirm page deletion")) return false;
    });

    /*Sorting script*/
    $("table#pages tbody").sortable({
        placeholder: "ui-state-highlight",
        update: function () {
            var ids = $("table#pages tbody").sortable("serialize");
            var url = "/Admin/Pages/ReoderPages";
            $.post(url, ids, function (data) {
            });
        }
    });
});