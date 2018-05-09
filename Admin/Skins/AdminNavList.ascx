<%@ Control Language="C#" AutoEventWireup="true"  Inherits="DotNetNuke.UI.Skins.Controls.AdminNavList" CodeFile="AdminNavList.ascx.cs" %>
<asp:Label CSSClass="SubHead" ID="lblTitle" runat="server" Text=""></asp:Label>&nbsp;
<asp:DropDownList ID="ddlAdminNavList" runat="server">
</asp:DropDownList>
<asp:ImageButton ID="btnGo" runat="server" Height="21px" ImageUrl="~/images/fwd.gif"
    OnClick="btnGo_Click" Width="21px" ImageAlign="Top" />