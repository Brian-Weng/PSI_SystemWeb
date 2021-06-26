<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="PIS_System.LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>帳號:<asp:TextBox ID="txtAccount" runat="server"></asp:TextBox></div>
        <div>密碼:<asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></div>
        <div>
            <asp:Button ID="btnLogin" runat="server" Text="登入" OnClick="btnLogin_Click"/>
        </div>
    </form>
</body>
</html>
