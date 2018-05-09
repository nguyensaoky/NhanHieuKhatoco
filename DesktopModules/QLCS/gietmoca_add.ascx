<%@ Control Language="C#" AutoEventWireup="true" CodeFile="gietmoca_add.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.gietmoca_add" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<table width="100%">
    <tr>
        <td width="120">Thời điểm mổ</td>
        <td><asp:TextBox ID="txtNgayMo" runat="server" Width="400" TabIndex="2"/>
            <cc1:calendarextender id="calNgayMo" runat="server" format="dd/MM/yyyy 12:00:00"
                popupbuttonid="txtNgayMo" targetcontrolid="txtNgayMo"></cc1:calendarextender>
        </td>
    </tr>
    <tr>
        <td>Biên bản</td>
        <td>
            <asp:TextBox ID="txtBienBan" runat="server" Width="400" TabIndex="2"/>
        </td>
    </tr>
</table>
<br />
<div style="font-weight:bold;text-align:left;padding:10px;">Sản phẩm</div>
<asp:PlaceHolder ID="dsSanPham" runat="server"></asp:PlaceHolder>
<table width="100%">
	<tr>
		<td align="center">
            <asp:Button ID="Save" runat="server" OnClick="btnSave_Click" CssClass="button" tabindex="12" Text="Lưu"/>
            <asp:HyperLink ID="lnkCancel" runat="server" Font-Bold="true" tabindex="13" CssClass="button">Thoát</asp:HyperLink>
		</td>
	</tr>
</table>