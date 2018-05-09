<%@ Control Language="C#" AutoEventWireup="true" CodeFile="news_relative_settings.ascx.cs" Inherits="DotNetNuke.News.news_relative_settings" %>

<table>
    <tr>
        <td><asp:Label ID="lblNewsPerPage" runat="server" resourcekey="lblNewsPerPage"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtLimits" runat="server" style="text-align:center">10</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblSource" runat="server" resourcekey="lblSource"></asp:Label></td>
        <td>
            <asp:RadioButtonList ID="lstRdSource" runat="server">
                <asp:ListItem resourcekey="itemViaCategory" Value="0" Selected="True"></asp:ListItem>
                <asp:ListItem resourcekey="itemViaKeyWords" Value="1"></asp:ListItem>
            </asp:RadioButtonList></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblTemplate" runat="server" resourcekey="lblTemplate"></asp:Label></td>
        <td><asp:DropDownList ID="ddlTemplate" runat="server"></asp:DropDownList></td>
    </tr>
</table>