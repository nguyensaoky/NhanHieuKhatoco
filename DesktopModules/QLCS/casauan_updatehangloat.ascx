<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casauan_updatehangloat.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casauan_updatehangloat" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<table width="100%">
    <tr>
        <td>
            Update cá ăn bắt đầu từ ngày
            <asp:TextBox ID="txtThoiDiemFrom" runat="server" Width="100" TabIndex="2"/>
            <cc1:calendarextender id="calThoiDiemFrom" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtThoiDiemFrom" targetcontrolid="txtThoiDiemFrom"></cc1:calendarextender>
            đến ngày
            <asp:TextBox ID="txtThoiDiemTo" runat="server" Width="100" TabIndex="3"/>
            <cc1:calendarextender id="calThoiDiemTo" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtThoiDiemTo" targetcontrolid="txtThoiDiemTo"></cc1:calendarextender>
            Loại cá
            <asp:ListBox ID="ddlLoaiCa" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Loại cá"></asp:ListBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnUpdateThucAn" runat="server" OnClick="btnUpdateThucAn_Click" CssClass="button" Text="Update Thức ăn"/>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnUpdateThuoc" runat="server" OnClick="btnUpdateThuoc_Click" CssClass="button" Text="Update Thuốc"/>
        </td>
    </tr>
</table>
