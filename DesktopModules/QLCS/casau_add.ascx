<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_add.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_add" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<table>
    <tr>
        <td width="180">Số lượng cá sấu muốn thêm</td>
        <td>
            <asp:TextBox ID="txtSoLuong" runat="server" style="width:400px;" TabIndex="1" Text="1"/>
        </td>
    </tr>
    <tr>
        <td>Vi cắt</td>
        <td>
            <asp:TextBox ID="txtMaSo" runat="server" style="width:400px;" TabIndex="2"/>
        </td>
    </tr>
    <tr>
        <td>Giới tính</td>
        <td>
            <asp:DropDownList ID="ddlGioiTinh" runat="server" Style="width: 400px" TabIndex="3">
                <asp:ListItem Value="-1">CXĐ</asp:ListItem>
                <asp:ListItem Value="0">Cái</asp:ListItem>
                <asp:ListItem Value="1">Đực</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Cá giống</td>
        <td>
            <asp:CheckBox ID="chkGiong" runat="server" Checked="true" TabIndex="4"/>
        </td>
    </tr>
    <tr>
        <td>Loại cá</td>
        <td>
            <asp:DropDownList ID="ddlLoaiCa" runat="server" Style="width: 400px" TabIndex="5">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Ngày nở</td>
        <td><asp:TextBox ID="txtNgayNo" runat="server" Width="400" TabIndex="6"/>
            <cc1:calendarextender id="calNgayNo" runat="server" format="dd/MM/yyyy 12:mm:ss"
                popupbuttonid="txtNgayNo" targetcontrolid="txtNgayNo"></cc1:calendarextender>
        </td>
    </tr>
    <tr>
        <td>Ngày xuống chuồng</td>
        <td><asp:TextBox ID="txtNgayXuongChuong" runat="server" Width="400" TabIndex="7"/>
            <cc1:calendarextender id="calNgayXuongChuong" runat="server" format="dd/MM/yyyy 12:mm:ss"
                popupbuttonid="txtNgayXuongChuong" targetcontrolid="txtNgayXuongChuong"></cc1:calendarextender>
        </td>
    </tr>
    <tr>
        <td>Nguồn gốc</td>
        <td>
            <asp:DropDownList ID="ddlNguonGoc" runat="server" Style="width: 400px" TabIndex="8">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Chuồng</td>
        <td>
            <asp:DropDownList ID="ddlChuong" runat="server" Style="width: 400px" TabIndex="9">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Cá mẹ</td>
        <td>
            <asp:DropDownList ID="ddlCaMe" runat="server" Style="width: 400px" TabIndex="10">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Ghi chú</td>
        <td>
            <asp:TextBox ID="txtGhiChu" runat="server" TextMode="MultiLine" style="width:400px;" TabIndex="11"/>
        </td>
    </tr>
	<tr>
		<td colspan="2" align="center">
            <asp:Button ID="Save" runat="server" OnClick="btnSave_Click" CssClass="button" tabindex="12" Text="Lưu"/>
            <asp:HyperLink ID="lnkCancel" runat="server" Font-Bold="true" tabindex="13" CssClass="button">Thoát</asp:HyperLink>
		</td>
	</tr>
</table>