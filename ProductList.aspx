<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="PIS_System.ProductList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>商品管理</h1>
    <table class="table">
        <tr>
            <th>商品編號</th>
            <th>分類</th>
            <th>商品名稱</th>
            <th>單價</th>
        </tr>
        <asp:Repeater ID="repProduct" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("ID") %></td>
                    <td><%# Eval("Category") %></td>
                    <td><%# Eval("Name") %></td>
                    <td><%# Eval("UnitPrice") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
