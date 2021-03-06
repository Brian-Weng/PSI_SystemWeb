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

    //Create an Array to hold the Table values.
    var POdetails = new Array();

    //Reference the Table.
    var table = document.getElementById("maintable");

    //Loop through Table Rows.
    for (var i = 1; i < table.rows.length; i++) {
        //Reference the Table Row.
        var row = table.rows[i];

        //Copy values from Table Cell to JSON object.
        var POdetail = {};

        POdetail.ID = row.cells[0].innerHTML;
        POdetail.QTY = row.cells[3].innerHTML;
        POdetail.Amount = row.cells[4].innerHTML;
        POdetails.push(POdetail);
    }
});