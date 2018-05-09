<%@ Control Language="C#" AutoEventWireup="true" CodeFile="general_settings.ascx.cs" Inherits="DotNetNuke.News.general_settings" %>

<table>
    <tr>
        <td><asp:Label ID="lblNewsPerPageAdmin" runat="server" resourcekey="lblNewsPerPageAdmin"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtLimitAdmin" runat="server" style="text-align:center">3</asp:TextBox>
        </td>
    </tr>
</table>