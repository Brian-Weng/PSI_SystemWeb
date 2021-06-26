$(document).ready(function () {
    $(".datepick").datepicker({
        dateFormat: "yy-mm-dd",
        onSelect: function (datetext) {
            var d = new Date(); // for now

            var h = d.getHours();
            h = (h < 10) ? ("0" + h) : h;

            var m = d.getMinutes();
            m = (m < 10) ? ("0" + m) : m;

            datetext = datetext + " " + h + ":" + m;

            $(".datepick").val(datetext);
        }
    });

    $("#btnEdit").click(function () {
        $("#dialog").dialog({
            width: 350,
            close: function (event, ui) {
                $(".trclass").removeClass("highlightRow");
                $(".lblProduct").html('請選擇商品');
                $(".txtQty").attr('disabled', true);
                $(".txtQty").val(0);
                $(".lblAmount").text('');

            }
        });
    });
    $(".trclass").click(function () {
        $(this).addClass("highlightRow").siblings().removeClass("highlightRow");
        $(".txtQty").attr('disabled', false);
        var name = $(this).find(".Name").html();
        var price = $(this).find(".UnitPrice").html();
        $(".lblProduct").html(name);
        $(".unitPrice").val(price);
        $(".lblAmount").text(0);
    });
    $(".txtQty").keyup(function () {
        var qty = parseInt($(this).val(), 10);
        var price = parseInt($(".unitPrice").val(), 10);
        var result = qty * price;
        result = isNaN(result) ? '請輸入數量' : result;
        $(".lblAmount").text(result);
    });
    $(".txtQty").mouseup(function () {
        var qty = parseInt($(this).val(), 10);
        var price = parseInt($(".unitPrice").val(), 10);
        var result = qty * price;
        result = isNaN(result) ? '請輸入數量' : result;
        $(".lblAmount").text(result);
    });
});