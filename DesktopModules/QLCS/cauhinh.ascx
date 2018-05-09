<%@ Control Language="C#" AutoEventWireup="true" CodeFile="cauhinh.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.cauhinh" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<link href="<%= ModulePath + "style_main.css" %>" type="text/css" rel="stylesheet" />
<script type="text/javascript" language="javascript" src="<%= ModulePath + "script_main.js"%>"></script>
<style type="text/css">
    .TextButton
    {
        border: none;
        cursor: pointer;
        text-decoration: none;
        font-weight: bold;
        margin: 5px 0;
    }
</style>
<br />
<center><b>Danh sách các tham số hệ thống</b></center>
<br />
<table>
    <tr>
        <td>
            Thư mục sao lưu dữ liệu:
        </td>
        <td>
            <asp:TextBox ID="txtBackupFolderName" runat="server" TabIndex="6" Width="300"/>
        </td>
        <td>
            <asp:Button ID="btnBackupFolderName" runat="server" Text="Lưu" OnClick="btnBackupFolderName_Click" CssClass="button"/>
        </td>
    </tr>
    <tr>
        <td>
            Số chữ số thập phân mặc định cho thức ăn cá:
        </td>
        <td>
            <asp:TextBox ID="txtTAScale" runat="server" TabIndex="6" Width="30"/>
        </td>
        <td>
            <asp:Button ID="btnTAScale" runat="server" Text="Lưu" OnClick="btnTAScale_Click" CssClass="button"/>
        </td>
    </tr>
    <tr>
        <td>
            Số chữ số thập phân mặc định cho thuốc thú y:
        </td>
        <td>
            <asp:TextBox ID="txtTTYScale" runat="server" TabIndex="6" Width="30"/>
        </td>
        <td>
            <asp:Button ID="btnTTYScale" runat="server" Text="Lưu" OnClick="btnTTYScale_Click" CssClass="button"/>
        </td>
    </tr>
    <tr>
        <td>
            Số chữ số thập phân mặc định cho sản phẩm giết mổ:
        </td>
        <td>
            <asp:TextBox ID="txtSPGMScale" runat="server" TabIndex="6" Width="30"/>
        </td>
        <td>
            <asp:Button ID="btnSPGMScale" runat="server" Text="Lưu" OnClick="btnSPGMScale_Click" CssClass="button"/>
        </td>
    </tr>
    <tr>
        <td>
            Số chữ số thập phân mặc định cho da cá sấu:
        </td>
        <td>
            <asp:TextBox ID="txtDCSScale" runat="server" TabIndex="6" Width="30"/>
        </td>
        <td>
            <asp:Button ID="btnDCSScale" runat="server" Text="Lưu" OnClick="btnDCSScale_Click" CssClass="button"/>
        </td>
    </tr>
</table>
<br /><br /><br /><br />