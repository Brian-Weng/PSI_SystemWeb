$(document).ready(function () {
    //-----初始化彈出視窗的內容-----
    var id, name, price;
    var fun1 = function () {
        var sndtr = $("#tableProduct").find("tr:nth-child(2)");
        sndtr.addClass("highlightRow").siblings().removeClass("highlightRow");
        id = sndtr.find(".ID").html();
        name = sndtr.find(".Name").html();
        price = sndtr.find(".UnitPrice").html();
        $(".lblProduct").html(name);
        $(".unitPrice").val(price);
        $(".txtQty").val(0);
        $(".lblAmount").text(0);
    }
    fun1();
    //-----初始化彈出視窗的內容-----

    //-----日曆自訂格式-----
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
    //-----日曆自訂格式-----

    //-----彈出視窗按鈕-----
    $("#btnEdit").click(function () {
        $("#dialog").dialog({
            width: 350,
            close: function (event, ui) {
                fun1();
            }
        });
    });
    //-----彈出視窗按鈕-----

    //-----點選表格列時將商品名稱等等值存入控制項-----
    
    $(".trclass").click(function () {
        $(this).addClass("highlightRow").siblings().removeClass("highlightRow");
        id = $(this).find(".ID").html();
        name = $(this).find(".Name").html();
        price = $(this).find(".UnitPrice").html();
        $(".lblProduct").html(name);
        $(".unitPrice").val(price);
        $(".txtQty").val(0);
        $(".lblAmount").text(0);
    });
    //-----點選表格列時將商品名稱等等值存入控制項-----

    //-----更改數量時同時計算小計-----
    var qty, price, result;
    $(".txtQty").keyup(function () {
        qty = parseInt($(this).val(), 10);
        price = parseInt($(".unitPrice").val(), 10);
        result = qty * price;
        result = isNaN(result) ? '請輸入數字' : result;
        $(".lblAmount").text(result);
    });

    $(".txtQty").mouseup(function () {
        qty = parseInt($(this).val(), 10);
        price = parseInt($(".unitPrice").val(), 10);
        result = qty * price;
        result = isNaN(result) ? '請輸入數量' : result;
        $(".lblAmount").text(result);
    });
    //-----更改數量時同時計算小計-----

    //-----將挑選商品內容存入進貨單表-----
    var total = 0;
    $("#btnInsert").click(function () {
        var markup = "<td>" + id + "</td><td>" + name + "</td><td>" + price + "</td><td>" + qty + "</td><td>" + result + "</td>";
        var hasvalue = $('#maintable tbody tr > td:contains(' + name + ')');
        var bool = Boolean(hasvalue.length > 0);

        if (bool) {
            total -= hasvalue.nextAll().eq(2).html();
            hasvalue.parent().html(markup);
            total += result;
            $(".lblTotal").text(total);
        } else {
            $("#maintable tbody").append('<tr>' + markup + '</tr>');
            total += result;
            $(".lblTotal").text(total);
        }
        $("#divTotal").show();
    });
    //-----將挑選商品內容存入進貨單表-----

    //-----將挑選商品從進貨單表中移除-----
    $("#btnRemove").click(function () {
        var matchtd = $('#maintable tbody tr > td:contains(' + name + ')');
        if (matchtd.length > 0) {
            total -= matchtd.nextAll().eq(2).html();
            matchtd.parent('tr').remove();
            if ($('#maintable tbody tr').length == 0)
                $("#divTotal").hide();
            $(".lblTotal").text(total);
        }
            
    });
    //-----將挑選商品從進貨單表中移除-----

    //-----新增或修改進貨單到資料庫-----
    $(".btnSave").click(function () {
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

        //Convert the JSON object to string and assign to Hidden Field.
        var x = JSON.stringify(POdetails);
        var y = $(".datepick").val();

        $.ajax({
            url: "/API/PO_Handler.ashx",
            method: "POST",
            dataType: "JSON",
            data: {"PO_Detail": x, "ArrivalTime": y}
        }).done(function (responseData) {
            alert("新增成功");
        });
    });
    //-----新增或修改進貨單到資料庫-----


});