<%@ Control Language="C#" AutoEventWireup="true" CodeFile="news_view_settings.ascx.cs" Inherits="DotNetNuke.News.news_view_settings" %>
<table width="100%">
    <tr>
        <td><asp:Label ID="lblNewsSource" runat="server" resourcekey="lblNewsSource"></asp:Label></td>
        <td>
            <asp:RadioButtonList ID="radSource" runat="server">
                <asp:ListItem resourcekey="itemViaUrl" Value="0"  Selected="True"></asp:ListItem>
                <asp:ListItem resourcekey="itemViaAssign" Value="1"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblNewsChoose" runat="server" resourcekey="lblNewsChoose"></asp:Label></td>
        <td>
	        <asp:DropDownList ID="ddlNews" runat="server" AutoPostBack="false"></asp:DropDownList>
	    </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblTemplate" runat="server" resourcekey="lblTemplate"></asp:Label></td>
        <td><asp:DropDownList ID="ddlTemplate" runat="server"></asp:DropDownList></td>
    </tr>
</table>