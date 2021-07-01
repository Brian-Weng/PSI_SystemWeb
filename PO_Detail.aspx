<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="PO_Detail.aspx.cs" Inherits="PIS_System.PO_Detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="javascript/js2.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>進貨單管理</h1>
    <h2 id="h2Title" runat="server"></h2>
    <div>單據編號：<asp:TextBox ID="txtPID" runat="server" CssClass="txtPID" Enabled="false">儲存時產生</asp:TextBox></div>
    <div>到達時間：<asp:TextBox ID="txtDate" runat="server" CssClass="datepick" AutoCompleteType="Disabled"></asp:TextBox></div>
    <div style="margin-top:20px">
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
                        <td><%# Eval("ID") %></td>
                        <td><%# Eval("Name") %></td>
                        <td><%# Eval("UnitPrice") %></td>
                        <td><%# Eval("Qty") %></td>
                        <td><%# Eval("Amount") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <input type="hidden" name="maintableJSON"/>
    <div id="divTotal" style="display:none;">
        <span style="margin-left:37%;">
            總計:<asp:Label ID="lblTotal" runat="server" Text="0" CssClass="lblTotal"></asp:Label>
        </span>
    </div>
    <div id="divCreate" style="display:none;"  runat="server">
        <asp:Label ID="lblCreator" runat="server" Text="建立者" CssClass="lblCreator">

        </asp:Label><asp:Label ID="lblCreateDate" runat="server" Text="建立時間" CssClass="lblCreateDate"></asp:Label>
    </div>
    <div id="divModify" style="display:none;">
        <asp:Label ID="lblModifier" runat="server" Text="修改者" CssClass="lblModifier">
        </asp:Label><asp:Label ID="lblModifyDate" runat="server" Text="修改時間" CssClass="lblModifyDate"></asp:Label>
    </div>
    <div>
        <button type="button" class="btnSave">儲存</button>
        <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click"/>
    </div>
    <div>
        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
    </div>


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
            <button type="button" id="btnRemove" class="btnRemove">移除</button>
        </div>
    </div>
</asp:Content>
