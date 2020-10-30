$(function () {
    function readURL(input) {
        var url = input.value;
        var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
        if (input.files && input.files[0] && (ext == "png" || ext == "jpeg" || ext == "jpg")) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('.imgpreview').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        } else {
            $('.imgpreview').attr('src', '~/Content/img/NO_IMG_600x600.png');
        }
    }

    $("#imageUpload").change(function () {
        readURL(this);
    });
});