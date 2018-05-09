<%@ Control Language="C#" AutoEventWireup="true" CodeFile="news_list_yahoo_settings.ascx.cs" Inherits="DotNetNuke.News.news_list_yahoo_settings" %>

<table>
    <tr>
        <td><asp:Label ID="lblNewsPerPage" runat="server" resourcekey="lblNewsPerPage"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtLimits" runat="server" style="text-align:center">3</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblNewsSource" runat="server" resourcekey="lblNewsSource"></asp:Label></td>
        <td>
            <asp:RadioButtonList ID="radSource" runat="server">
                <asp:ListItem resourcekey="itemViaCategoryUrl" Value="0" Selected="True"></asp:ListItem>
                <asp:ListItem resourcekey="itemViaNewsGroupUrl" Value="1"></asp:ListItem>
                <asp:ListItem resourcekey="itemViaCategoryAssign" Value="2"></asp:ListItem>
                <asp:ListItem resourcekey="itemViaNewsGroupAssign" Value="3"></asp:ListItem>
                <asp:ListItem resourcekey="itemViaCategoryCode" Value="4"></asp:ListItem>
                <asp:ListItem resourcekey="itemViaNewsGroupCode" Value="5"></asp:ListItem>
                <asp:ListItem resourcekey="itemViaMostRead" Value="6"></asp:ListItem>
                <asp:ListItem resourcekey="itemViaNewest" Value="7"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblCategory" runat="server" resourcekey="lblCategory"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlCategory" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblNewsGroup" runat="server" resourcekey="lblNewsGroup"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlNewsGroup" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblCategoryCode" runat="server" resourcekey="lblCategoryCode"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtCategoryCode" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblNewsGroupCode" runat="server" resourcekey="lblNewsGroupCode"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtNewsGroupCode" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblSlideHeight" runat="server" resourcekey="lblSlideHeight"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtSlideHeight" runat="server">300</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblLeftPartWidth" runat="server" resourcekey="lblLeftPartWidth"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtLeftPartWidth" runat="server">400</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblRightPartWidth" runat="server" resourcekey="lblRightPartWidth"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtRightPartWidth" runat="server">250</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblImageWidth" runat="server" resourcekey="lblImageWidth"></asp:Label></td>
        <td><asp:TextBox ID="txtImageWidth" runat="server">100</asp:TextBox>&nbsp;px</td>
    </tr>
    <tr>
        <td><asp:Label ID="lblImageHeight" runat="server" resourcekey="lblImageHeight"></asp:Label></td>
        <td><asp:TextBox ID="txtImageHeight" runat="server">100</asp:TextBox>&nbsp;px</td>
    </tr>
    <tr>
        <td><asp:Label ID="lblTitleLength" runat="server" resourcekey="lblTitleLength"></asp:Label></td>
        <td><asp:TextBox ID="txtTitleLength" runat="server">100</asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblDescLength" runat="server" resourcekey="lblDescLength"></asp:Label></td>
        <td><asp:TextBox ID="txtDescLength" runat="server">100</asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblDisplayTitle" runat="server" resourcekey="lblDisplayTitle"></asp:Label></td>
        <td><asp:CheckBox ID="chkDisplayTitle" runat="server" Checked="true" /></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblTitleCSSClass" runat="server" resourcekey="lblTitleCSSClass"></asp:Label></td>
        <td><asp:TextBox ID="txtTitleCSSClass" runat="server">NewsListTitle</asp:TextBox></td>
    </tr>
</table>