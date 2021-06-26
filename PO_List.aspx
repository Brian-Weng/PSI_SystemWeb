<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="PO_List.aspx.cs" Inherits="PIS_System.PO_List" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>進貨單總覽</h1>
    <table class="table">
        <tr>
            <th>單據編號</th>
            <th>貨物種類</th>
            <th>進貨數量</th>
            <th>預計進貨時間</th>
            <th>進貨金額</th>
            <th>修改/刪除</th>
        </tr>
        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("PID") %></td>
                    <td><%# Eval("Items") %></td>
                    <td><%# Eval("QTY") %></td>
                    <td><%# Eval("ArrivalTime", "{0:yyyy/MM/dd hh:mm}") %></td>
                    <td><%# Eval("Total") %></td>
                    <td>
                        <asp:Button ID="btnDelete" runat="server" Text="刪除" CommandName="DeleteItem" CommandArgument='<%# Eval("PID") %>' OnClientClick='<%# Eval("PID", "return confirm(\"確定要刪除進貨單{0}?\");") %>'/>
                        <asp:Button ID="btnUpdate" runat="server" Text="修改" CommandName="UpdateItem" CommandArgument='<%# Eval("PID") %>'/>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <div class="row">
        <div>
            <asp:Button ID="btnInsert" runat="server" Text="新增" CssClass="button button2" OnClick="btnInsert_Click"/>
            <asp:Button ID="btnPrint" runat="server" Text="列印" CssClass="button button2" OnClick="btnPrint_Click"/>
        </div>
        <div class="pagebox">
            <asp:Repeater ID="repPage" runat="server">
                <ItemTemplate>
                    <a href="<%# Eval("Link") %>" title="<%# Eval("Title") %>" class="page"><%# Eval("Name") %></a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</asp:Content>
