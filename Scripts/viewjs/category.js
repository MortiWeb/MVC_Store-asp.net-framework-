$(function () {
    /*Add new category*/

    var newCatA = $("a#newcata");
    var newCatTextInput = $("#newcatname");
    var ajaxText = $("span.ajax-text");
    var table = $("table#categories tbody");

    newCatTextInput.keyup(function (e) {
        if (e.keyCode == 13) {
            newCatA.click();
        }
    });

    newCatA.click(function (e) {
        e.preventDefault();

        var catName = newCatTextInput.val();

        if (catName.length < 3) {
            alert("Category name must be at least 3 characters long.");
            return false;
        }

        ajaxText.show();

        var url = "/admin/shop/AddNewCategory";

        $.post(url, { catName: catName }, function (data) {
            var response = data.trim();

            if (response == "titletaken") {
                ajaxText.html("<span class='alert alert-danger'>That title is already taken!</span>");
                setTimeout(function () {
                    ajaxText.fadeOut("fast", function () {
                        ajaxText.html("<img src='/Content/img/ajax-loader.gif' height='50' />");
                    });
                }, 3000);
                return false;
            }
            else {
                if (!$("table#categories").length) {
                    location.reload();
                }
                else {
                    ajaxText.html("<span class='alert alert-success'>The category has been added!</span>");
                    setTimeout(function () {
                        ajaxText.fadeOut("fast", function () {
                            ajaxText.html("<img src='/Content/img/ajax-loader.gif' height='50' />");
                        });
                    }, 3000);

                    newCatTextInput.val("");

                    var toAppend = $("table#categories tbody tr:last").clone();
                    toAppend.attr("id", "id_" + data);
                    toAppend.find("input.form-control").attr("value", catName);
                    toAppend.find("a.delete").attr("href", "/admin/shop/DeleteCategory/" + data);
                    table.append(toAppend);
                    table.sortable("refresh");
                }
            }
        });
    });

    /* Rename category script*/
    var originalTextBoxValue;

    $("table#categories button.edit").click(function () {
        $("table#categories input.text-box").attr("readonly", true);
        $("table#categories div.ajax-icon-save").css('display', 'none');
        $(this).parent().parent().find("div.ajax-icon-save").css('display', 'inline-block');
        var textBox = $(this).parent().parent().find("input.text-box");
        textBox.attr("readonly", false);
        textBox.focus();
        originalTextBoxValue = textBox.val();
    });

    $("table#categories input.text-box").blur(function () {
        $(this).val(originalTextBoxValue);
    });

    /*$("table#categories input.text-box").keyup(function (e) {
        if (e.keyCode == 13) {
            $(this).blur();
        }
    });*/

    $("table#categories div.ajax-icon-save").click(function () {
        var $this = $(this);
        var textBox = $this.parent().find("input.text-box");
        var ajaxdiv = $this.parent().parent().parent().parent().find(".ajaxdivtd");
        var newCatName = textBox.val();
        var id = $this.parent().parent().parent().parent().parent().attr("id").substring(3);
        var url = "/admin/shop/RenameCategory";

        if (newCatName.length < 3) {
            alert("Category name must be at least 3 characters long.");
            textBox.val(originalTextBoxValue);
            textBox.attr("readonly", true);
            return false;
        }

        $.post(url, { newCatName: newCatName, id: id }, function (data) {
            var response = data.trim();

            if (response == "titletaken") {
                textBox.val(originalTextBoxValue);
                ajaxdiv.html("<div class='alert alert-danger'>That title is taken!</div>").show();
            }
            else {
                ajaxdiv.html("<div class='alert alert-success'>The category name has been changed!</div>").show();
                originalTextBoxValue = newCatName;
            }

            setTimeout(function () {
                ajaxdiv.fadeOut("fast", function () {
                    ajaxdiv.html("");
                });
            }, 3000);
        }).done(function () {
            textBox.attr("readonly", true);
            $this.css('display', 'none');
        });
    });


    /* Confirm category deletion */
    $("body").on("click", "a.delete", function () {
        if (!confirm("Confirm category deletion")) return false;
    });

    /*Sorting script*/
    $("table#categories tbody").sortable({
        placeholder: "ui-state-highlight",
        update: function () {
            var ids = $("table#categories tbody").sortable("serialize");
            var url = "/Admin/Shop/ReorderCategories";
            $.post(url, ids, function (data) {
            });
        }
    });
});