<%@ Control Language="C#" AutoEventWireup="true" CodeFile="group_menu_settings.ascx.cs" Inherits="DotNetNuke.News.group_menu_settings" %>
<table width="100%">
    <tr>
        <td><asp:Label ID="lblSource" runat="server" resourcekey="lblSource"></asp:Label></td>
        <td>
            <asp:RadioButtonList ID="lstRdSource" runat="server">
                <asp:ListItem resourcekey="itemAll" Value="0" Selected="True"></asp:ListItem>
                <asp:ListItem resourcekey="itemViaCode" Value="1"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblCode" runat="server" resourcekey="lblCode"></asp:Label></td>
        <td><asp:TextBox ID="txtCode" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblExpandAll" runat="server" resourcekey="lblExpandAll"></asp:Label></td>
        <td><asp:CheckBox ID="chkExpandAll" runat="server" Checked="false" /></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblMenuWidth" runat="server" resourcekey="lblMenuWidth"></asp:Label></td>
        <td><asp:TextBox ID="txtMenuWidth" runat="server" Text="220"></asp:TextBox>&nbsp;px</td>
    </tr>
    <tr>
        <td><asp:Label ID="lblCSSPrefix" runat="server" resourcekey="lblCSSPrefix"></asp:Label></td>
        <td><asp:TextBox ID="txtCSSPrefix" runat="server"></asp:TextBox></td>
    </tr>
</table>