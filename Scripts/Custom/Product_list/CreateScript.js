$(document).ready(function () {
    var productId = $('#id_товара').val();
    $.ajax({
        type: "POST",
        url: "/Список_товаров/GetProductPrice",
        data: { ProductId: productId },
        success: function (data) {
            $('#Цена').val(data.price);
            $('#productImage').attr('src', data.image).show();
            $('#productCount').val(data.count);
            limitOrderQuantity(data.count);
            imgExists();
        }
    });
    $('#id_товара').change(function () {
        var productId = $(this).val();
        $.ajax({
            type: "POST",
            url: "/Список_товаров/GetProductPrice",
            data: { ProductId: productId },
            success: function (data) {
                $('#Цена').val(data.price);
                $('#productImage').attr('src', data.image).show();
                $('#productCount').val(data.count);
                limitOrderQuantity(data.count);
                imgExists();
            }
        });
    });
});

function limitOrderQuantity(maxQuantity) {
    $('#count').on('input', function () {
        var desiredQty = parseInt($(this).val());
        if (isNaN(desiredQty) || desiredQty > maxQuantity) {
            $(this).val(maxQuantity);
        }
    });
}

function checkImageExists(image_url, callback) {
    var img = new Image();
    img.onerror = function () {
        callback(false);
    };
    img.onload = function () {
        callback(true);
    };
    img.src = image_url;
}

function imgExists() {
    let img = document.getElementById('productImage').src;
    console.log(img.src);

    checkImageExists(img, function (exists) {
        if (exists) {
            document.getElementById('imgDiv').style.display = 'block';
        } else {
            document.getElementById('imgDiv').style.display = 'none';
        }
    });
}