<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="PO_Detail.aspx.cs" Inherits="PIS_System.PO_Detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>進貨單管理</h1>
    <div>單據編號：<asp:TextBox ID="txtPID" runat="server" CssClass="txtPID" Enabled="false"></asp:TextBox></div>
    <div>進貨時間：<asp:TextBox ID="txtDate" runat="server" CssClass="datepick"></asp:TextBox></div>
    <div>
        <button type="button" id="btnEdit">編輯</button>
    </div>
    <table class="table" id="maintable">
        <thead>
            <tr>
                <th>商品編號</th>
                <th>商品名稱</th>
                <th>單價</th>
                <th>數量</th>
                <th>小計</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="repViewPODetail" runat="server">
                <ItemTemplate>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <input type="text" name="maintableJSON"/>
    <div>
        <asp:Label ID="txtTotal" runat="server" Text="" CssClass="txtTotal"></asp:Label>
    </div>
    <div>
        <asp:Label ID="lblCreator" runat="server" Text="建立者"></asp:Label><asp:Label ID="lblCreateDate" runat="server" Text="建立時間"></asp:Label>
    </div>
    <div>
        <asp:Label ID="lblModifier" runat="server" Text="修改者"></asp:Label><asp:Label ID="lblModifyDate" runat="server" Text="修改時間"></asp:Label>
    </div>
    <div>
        <button type="button" class="btnSave">儲存</button>
        <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click"/>
    </div>
    <div>
        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
    </div>
    <button type="button" class="testbtn">test</button>
    <input id="textbox1" type="text" />


    <div id="dialog" style="display:none;">
        <table class="table" id="tableProduct" style="width:300px">
            <tr>
                <th>商品編號</th>
                <th>商品名稱</th>
                <th>單價</th>
            </tr>
            <asp:Repeater ID="repProduct" runat="server">
                <ItemTemplate>
                    <tr class="trclass">
                        <td><span class="ID"><%# Eval("ID") %></span></td>
                        <td><span class="Name"><%# Eval("Name") %></span></td>
                        <td><span class="UnitPrice"><%# Eval("UnitPrice") %></span></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        
        <div>已挑選商品：<asp:Label ID="lblProduct" runat="server" Text="請選擇商品" CssClass="lblProduct"></asp:Label></div>
        <div>數量：<asp:TextBox ID="txtQty" runat="server" TextMode="Number" Text="0" CssClass="txtQty" min="0"></asp:TextBox></div>
        <div>小計：<asp:Label ID="lblAmount" runat="server" Text="" CssClass="lblAmount"></asp:Label></div>
        <div><input type="hidden" class="unitPrice"/></div>
        <div>
            <button type="button" id="btnInsert">加入</button>
        </div>
    </div>
    <script src="javascript/js2.js"></script>
    <script>
        function GetTableValuess() {

            if ($(".txtPID").val() != "") {
                alert(123);
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
                    POdetail.PID = $(".txtPID").val();
                    POdetail.ID = row.cells[0].innerHTML;
                    POdetail.QTY = row.cells[3].innerHTML;
                    POdetail.Amount = row.cells[4].innerHTML;
                    POdetails.push(POdetail);
                }

                //Convert the JSON object to string and assign to Hidden Field.
                var x = JSON.stringify(POdetails);
                document.getElementsByName("maintableJSON")[0].value = x;
                return true;
                
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>
