<%@ Control Language="C#" AutoEventWireup="true" CodeFile="comment_admin.ascx.cs" Inherits="DotNetNuke.News.comment_admin" %>
<%@ Register Src="PagingControl.ascx" TagName="PagingControl" TagPrefix="pg" %>

<asp:Label ID="lblRestricted" runat="server" resourcekey="lblRestricted" Visible="false"></asp:Label>
<table width="100%">
    <tr>
        <td>
            <asp:Label ID="lblCatList" runat="server" resourcekey="lblCatList" CssClass="Normal"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlCat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCat_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblNewsList" runat="server" resourcekey="lblNewsList" CssClass="Normal"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlNews" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNews_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblStatusList" runat="server" resourcekey="lblStatusList" CssClass="Normal"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                <asp:ListItem Selected="True" Text="" Value="-1"></asp:ListItem>
                <asp:ListItem resourcekey="lblStatusDisable" Value="0"></asp:ListItem>
                <asp:ListItem resourcekey="lblStatusEnable" Value="1"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0">
              <tr style="background-color:#BBBBBB;">
                <td style="width:10%"><asp:Label ID="lblAuthor" runat="server" resourcekey="lblAuthor"></asp:Label></td>
                <td style="width:50%"><asp:Label ID="lblComment" runat="server" resourcekey="lblComment"></asp:Label></td>
                <td style="width:10%"><asp:Label ID="lblIPAddress" runat="server" resourcekey="lblIPAddress"></asp:Label></td>
                <td style="width:10%"><asp:Label ID="lblStatus" runat="server" resourcekey="lblStatus"></asp:Label></td>
                <td style="width:10%;text-align:center;"><asp:Label ID="lblDelete" runat="server" resourcekey="lblDelete"></asp:Label></td>
                <td style="width:10%;text-align:center;"><asp:Label ID="lblChangeStatus" runat="server" resourcekey="lblChangeStatus"></asp:Label></td>
              </tr>
              <asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
            </table>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="2">
            <pg:PagingControl id="paging" runat="server" Visible="false" />
        </td>
    </tr>
    <tr>
        <td colspan="2" align="right">
            <asp:HyperLink ID="lnkBack" runat="server" resourcekey="lnkBack"></asp:HyperLink>
        </td>
    </tr>
</table>
