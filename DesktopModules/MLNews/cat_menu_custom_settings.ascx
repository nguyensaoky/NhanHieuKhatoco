<%@ Control Language="C#" AutoEventWireup="true" CodeFile="cat_menu_custom_settings.ascx.cs" Inherits="DotNetNuke.News.cat_menu_custom_settings" %>
<table width="100%">
    <tr>
        <td>Tiêu đề cột 1</td>
        <td>
            <asp:TextBox ID="txtTieuDe1" runat="server" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Số tin đặc biệt cột 1</td>
        <td>
            <asp:TextBox ID="txtNumShortNews1" runat="server" Width="100%">0</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Chiều ngang hình ảnh thu nhỏ cột 1</td>
        <td>
            <asp:TextBox ID="txtImageWidth1" runat="server" Width="100%">100</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Mẫu giao diện cột 1</td>
        <td><asp:DropDownList ID="ddlTemplate1" runat="server"></asp:DropDownList></td>
    </tr>
    
    <tr>
        <td valign="top">Bài viết cột 1</td>
        <td>
            <asp:ListBox ID="lstBaiViet1" runat="server" SelectionMode="Multiple" Width="100%" Height="50"></asp:ListBox>
            <br /><br /><br />
        </td>
    </tr>
    <tr>
        <td>Tiêu đề cột 2</td>
        <td>
            <asp:TextBox ID="txtTieuDe2" runat="server" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Số tin đặc biệt cột 2</td>
        <td>
            <asp:TextBox ID="txtNumShortNews2" runat="server" Width="100%">0</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Chiều ngang hình ảnh thu nhỏ cột 2</td>
        <td>
            <asp:TextBox ID="txtImageWidth2" runat="server" Width="100%">100</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Mẫu giao diện cột 2</td>
        <td><asp:DropDownList ID="ddlTemplate2" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
        <td valign="top">Bài viết cột 2</td>
        <td>
            <asp:ListBox ID="lstBaiViet2" runat="server" SelectionMode="Multiple" Width="100%"></asp:ListBox>
            <br /><br /><br />
        </td>
    </tr>
    <tr>
        <td>Tiêu đề cột 3</td>
        <td>
            <asp:TextBox ID="txtTieuDe3" runat="server" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Số tin đặc biệt cột 3</td>
        <td>
            <asp:TextBox ID="txtNumShortNews3" runat="server" Width="100%">0</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Chiều ngang hình ảnh thu nhỏ cột 3</td>
        <td>
            <asp:TextBox ID="txtImageWidth3" runat="server" Width="100%">100</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Mẫu giao diện cột 3</td>
        <td><asp:DropDownList ID="ddlTemplate3" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
        <td valign="top">Bài viết cột 3</td>
        <td>
            <asp:ListBox ID="lstBaiViet3" runat="server" SelectionMode="Multiple" Width="100%"></asp:ListBox>
        </td>
    </tr>
</table>