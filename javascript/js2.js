$(document).ready(function () {
    //-----初始化彈出視窗的內容-----
    var id, name, price;
    var total = 0;
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

        
        if ($('#maintable tbody tr').length != 0)
            $("#divTotal").show();

        var num = $('.lblTotal').text().replace(',', '');
        total = parseInt(num, 10);
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
        },
        
    });
    //-----日曆自訂格式-----

    //-----彈出視窗按鈕-----
    $("#btnEdit").click(function () {
        $("#dialog").dialog({
            width: 350,
            modal: true,
            
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
    var price, result;
    $(".txtQty").keyup(function () {
        $(this).val($(this).val().replace('.', ''));
        qty = parseInt($(this).val(), 10);
        if (qty < 0 || qty > 101) {
            alert('數量範圍須在1~100之間');
            $(this).val('');
            $(".lblAmount").text('');
            return;
        }

        price = parseInt($(".unitPrice").val(), 10);
        result = qty * price;
        result = isNaN(result) ? '請輸入數字' : result;
        $(".lblAmount").text(result);
    });
    Math
    $(".txtQty").mouseup(function () {
        qty = parseInt($(this).val(), 10);
        price = parseInt($(".unitPrice").val(), 10);
        result = qty * price;
        result = isNaN(result) ? '請輸入數量' : result;
        $(".lblAmount").text(result);
    });
    //-----更改數量時同時計算小計-----

    //-----將挑選商品內容存入進貨單表-----
    
    $("#btnInsert").click(function () {
        qty = parseInt($(".txtQty").val(), 10);
        if (isNaN(qty) || qty == 0) {
            alert('請輸入數量');
            return;
        }
        if (qty < 0 || qty > 101) {
            alert('數量範圍須在1~99之間');
            return;
        }
        if (Math.floor(qty) != qty && $.isNumeric(id)) {
            alert('請輸入整數');
        }

        var markup = "<td>" + id + "</td><td>" + name + "</td><td>" + price + "</td><td>" + qty + "</td><td>" + result + "</td>";
        var hasvalue = $('#maintable tbody tr > td:contains(' + name + ')');
        var bool = Boolean(hasvalue.length > 0);

        if (bool) {
            total -= hasvalue.nextAll().eq(2).html();
            hasvalue.parent().html(markup);
        } else {
            $("#maintable tbody").append('<tr>' + markup + '</tr>');
        }
        total += result;
        var num = thousands(total);
        $(".lblTotal").text(num);

        $("#divHint").hide();
        $("#divTotal").show();
        sortTable();
    });
    //-----將挑選商品內容存入進貨單表-----

    //-----將挑選商品從進貨單表中移除-----
    $("#btnRemove").click(function () {
        var matchtd = $('#maintable tbody tr > td:contains(' + name + ')');
        if (matchtd.length > 0) {
            total -= matchtd.nextAll().eq(2).html();
            matchtd.parent('tr').remove();
            if ($('#maintable tbody tr').length == 0) {
                $("#divHint").show();
                $("#divTotal").hide();
            }
                
            var num = thousands(total);
            $(".lblTotal").text(num);
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
        var po = JSON.stringify(POdetails);
        var dp = $(".datepick").val();
        var pid = ($('.txtPID').val() == '儲存時產生')? 'create' : $('.txtPID').val();

        $.ajax({
            url: "/API/PO_Handler.ashx",
            type: "POST",
            dataType: "json",
            data: {"PO_Detail": po, "PID":pid, "ArrivalTime": dp}
        })
            .done(function (responseData) {
                console.log(responseData);
                var resPid = JSON.stringify(responseData.PID);
                var resCreator = JSON.stringify(responseData.Creator);
                var resCreateDate = JSON.stringify(responseData.CreateDate);

                if (resModifier == null) {
                    var formatPid = resPid.replace(/\"/g, '');
                    var formatCreator = resCreator.replace(/\"/g, '');
                    var formatCreDate = resCreateDate.slice(1, 20).replace('T', ' ');

                    $(".txtPID").val(formatPid);
                    $(".lblCreator").text("建立者" + formatCreator);
                    $(".lblCreateDate").text("建立時間" + formatCreDate);
                    $("#divCreate").show();
                    alert('新增成功');
                    //$(".btnSave").hide();
                    $(".btnSave").remove();
                    $(".btnCancel").val('回總覽頁');
                } else {
                    var resModifier = JSON.stringify(responseData.Modifier);
                    var resModifyDate = JSON.stringify(responseData.ModifyDate);

                    var formatModifier = resModifier.replace(/\"/g, '');
                    var formatModDate = resModifyDate.slice(1, 20).replace('T', ' ');

                    $(".lblModifier").text("建立者" + formatModifier);
                    $(".lblModifyDate").text("建立時間" + formatModDate);
                    $("#divModify").show();
                    alert('更新成功');
                }

            })
            .fail(function (xhr, status, errorThrown) {
                console.log("傳輸失敗");
                console.log('Error: ' + errorThrown);
                console.log('Status: ' + status);
                console.dir(xhr);
            })
    });
    //-----新增或修改進貨單到資料庫-----

    //-----數字加上千分位符號-----
    function thousands(num) {
        return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    //-----數字加上千分位符號-----

    //-----排序表格-----
    function sortTable() {
        var rows = $('#maintable tbody  tr').get();

        rows.sort(function (a, b) {

            var A = $(a).children('td').eq(0).text().toUpperCase();
            var B = $(b).children('td').eq(0).text().toUpperCase();

            if (A < B) {
                return -1;
            }

            if (A > B) {
                return 1;
            }

            return 0;

        });

        $.each(rows, function (index, row) {
            $('#maintable').children('tbody').append(row);
        });
    }
    //-----排序表格-----

});