<%@ Control Language="C#" AutoEventWireup="true" CodeFile="khoaso.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.khoaso" %>
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
<table width="100%">
    <tr>
        <td width="200">
            Khóa dữ liệu trước thời điểm
        </td>
        <td width="130">
            <asp:TextBox ID="txtDate" runat="server" Width="120" TabIndex="6"/>
            <cc1:calendarextender id="calDate" runat="server" format="dd/MM/yyyy 12:00:00" popupbuttonid="txtDate" targetcontrolid="txtDate"></cc1:calendarextender>
        </td>
        <td>
            <asp:Button ID="btnKhoaSo" runat="server" Text="Khóa dữ liệu" OnClick="btnKhoaSo_Click" CssClass="button"/>
        </td>
    </tr>
    <tr><td colspan="3"><br /></td></tr>
    <tr>
        <td>
            Mở các loại dữ liệu đã bị khóa
        </td>
        <td>
            <asp:ListBox ID="lstLoaiDuLieu" runat="server" SelectionMode="Multiple" Rows="6">
                <asp:ListItem Selected="True" Value="CaSau_BienDong">Biến động cá</asp:ListItem>
                <asp:ListItem Selected="True" Value="CaSauDe">Cá đẻ</asp:ListItem>
                <asp:ListItem Selected="True" Value="CaSauAn">Cá ăn</asp:ListItem>
                <asp:ListItem Selected="True" Value="GietMoCa">Giết mổ cá</asp:ListItem>
                <asp:ListItem Selected="True" Value="ThuHoiDa">Thu hồi da</asp:ListItem>
                <asp:ListItem Selected="True" Value="VatTu_BienDong">Biến động vật tư</asp:ListItem>
            </asp:ListBox>
        </td>
        <td>
            <asp:Button ID="btnMoKhoa" runat="server" Text="Mở khóa" OnClick="btnMoKhoa_Click" CssClass="button"/>
        </td>
    </tr>
</table>
<br /><br /><br /><br />