<%@ Control Language="C#" AutoEventWireup="true" CodeFile="foruser_settings.ascx.cs" Inherits="DotNetNuke.News.foruser_settings" %>

<table>
    <tr>
        <td><asp:Label ID="lblNewsPerPageAdmin" runat="server" resourcekey="lblNewsPerPageAdmin"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtLimitAdmin" runat="server" style="text-align:center">3</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Chuyên mục</td>
	    <td>
            <asp:CheckBoxList ID="lstChkCats" runat="server">
            </asp:CheckBoxList>
	    </td>
    </tr>
</table>