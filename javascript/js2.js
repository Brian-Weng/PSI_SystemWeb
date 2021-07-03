$(document).ready(function () {
    //-----初始化-----
    var id, name, price;
    var total = 0;
    const urlParams = new URLSearchParams(window.location.search);
    const pidQueryStr = urlParams.get('PID');
    var fun1 = function () {
        var secondTr = $("#tableProduct").find("tr:nth-child(2)");
        secondTr.addClass("highlightRow").siblings().removeClass("highlightRow");
        id = secondTr.find(".ID").html();
        name = secondTr.find(".Name").html();
        price = secondTr.find(".UnitPrice").html();
        $(".lblProduct").html(name);
        $(".unitPrice").val(price);
        $(".txtQty").val(0);
        $(".lblAmount").text(0);

        
        if ($('#maintable tbody tr').length != 0) {
            $("#divHint").hide();
            $("#divTotal").show();
        }
            
        var num = $('.lblTotal').text().replace(',', '');
        total = parseInt(num, 10);
    }
    fun1();
    //-----初始化-----

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
        var pid = $('.txtPID').val();
        if (pid == '儲存時產生' && !pidQueryStr) {
            pid = 'create';
        } else if (pid == pidQueryStr) {
            pid = pid;
        } else {
            alert('票據編號發生異常');
            return;
        }

        var date = $(".datepick").val()
        if (!isValidDate(date)) {
            alert('日期格式不正確或異常');
            return;
        }

        var tableTr = $('#maintable tbody tr');
        if ( tableTr.length == 0) {
            alert('明細表中必須至少有一樣商品');
            return;
        }

        

        //Create an Array to hold the Table values.
        var POdetails = new Array();

        for (var i = 0; i < tableTr.length; i++) {
            //Reference the Table Row.
            var row = tableTr.eq(i);

            //Copy values from Table Cell to JSON object.
            var POdetail = {};
            
            POdetail.ID = row.find('td').eq(0).html();
            POdetail.QTY = row.find('td').eq(3).html();
            POdetail.Amount = row.find('td').eq(4).html();
            POdetails.push(POdetail);
        }

        //Convert the JSON object to string and assign to Hidden Field.
        var PODetailData = JSON.stringify(POdetails);


        $.ajax({
            url: "/API/PO_Handler.ashx",
            type: "POST",
            dataType: "json",
            data: {"PO_Detail": PODetailData, "PID":pid, "ArrivalTime": date}
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


    //-----時間格式-----
    function isValidDate(datetimeString) {
        var reg = /^[1-2]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])\s+(20|21|22|23|[0-1]\d|[0-9]):([0-5]\d|[0-9])$/;
        if (!datetimeString.match(reg)) return false;  // Invalid format
        var dateStr = datetimeString.slice(0, 10);
        var d = new Date(dateStr);
        var dNum = d.getTime();
        if (!dNum && dNum !== 0) return false; // NaN value, Invalid date
        return d.toISOString().slice(0, 10) === dateStr;
    }
    //-----時間格式-----
});