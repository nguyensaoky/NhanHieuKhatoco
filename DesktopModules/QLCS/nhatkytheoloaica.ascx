<%@ Control Language="C#" AutoEventWireup="true" CodeFile="nhatkytheoloaica.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.nhatkytheoloaica" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="<%= ModulePath + "jquery.dataTables.css" %>"/>
<link rel="stylesheet" type="text/css" href="<%= ModulePath + "dataTables.fixedColumns.css" %>"/>
<style type="text/css" class="init">
	th, td { white-space: nowrap; }
	div.dataTables_wrapper {
		width: 1000px;
		margin: 0 auto;
	}
</style>

<script type="text/javascript" language="javascript" src="<%= ModulePath + "jquery.js"%>"></script>
<script type="text/javascript" language="javascript" src="<%= ModulePath + "jquery.dataTables.js"%>"></script>
<script type="text/javascript" language="javascript" src="<%= ModulePath + "dataTables.fixedColumns.js"%>"></script>
<script type="text/javascript" language="javascript" class="init">
	$(document).ready(function() {
		var table = $('#thongke').DataTable( {
			scrollY:        "500px",
			scrollX:        true,
			scrollCollapse: true,
			paging:         false,
			bFilter:        false,
			bInfo:          false,
			bSort:          false
		} );
		new $.fn.dataTable.FixedColumns( table, {
			leftColumns: 1
		} );
		//new $.fn.dataTableExt.sErrMode = 'throw';
	} );
</script>
<table>
    <tr><td colspan="2"><center><b>NHẬT KÝ CHĂN NUÔI THEO LOẠI CÁ</b></center><br /></td></tr>
    <tr>
        <td valign="top" align="left">
            Từ ngày<br />
            <asp:TextBox ID="txtTuNgay" runat="server" Width="100" TabIndex="6"/>
            <cc1:calendarextender id="calTuNgay" runat="server" format="dd/MM/yyyy" popupbuttonid="txtTuNgay" targetcontrolid="txtTuNgay"></cc1:calendarextender>
        </td>
        <td valign="top" align="left">
            Đến ngày<br />
            <asp:TextBox ID="txtDenNgay" runat="server" Width="100" TabIndex="6"/>
            <cc1:calendarextender id="calDenNgay" runat="server" format="dd/MM/yyyy" popupbuttonid="txtDenNgay" targetcontrolid="txtDenNgay"></cc1:calendarextender>            
        </td>
        <td valign="top" align="center">
            Loại cá (Ctrl để chọn nhiều)<br />
            <asp:ListBox ID="lstLoaiCa" runat="server" SelectionMode="Multiple" Rows="6" style="vertical-align:top;">
            </asp:ListBox>
        </td>
        <td valign="top" align="center">
            Khu chuồng (Ctrl để chọn nhiều)<br />
            <asp:ListBox ID="lstKhuChuong" runat="server" SelectionMode="Multiple" Rows="6" style="vertical-align:top;">
            </asp:ListBox>
        </td>
        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td valign="top" align="center">
            Phân loại cá<br />
            <asp:DropDownList ID="ddlPhanLoai" runat="server" AutoPostBack="false">
                <asp:ListItem Value="-1">-----</asp:ListItem>
                <asp:ListItem Value="1">Giống</asp:ListItem>
                <asp:ListItem Value="0">Tăng trọng</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td valign="top" align="center">
            Số chữ số thập phân<br />
            Thức ăn<br />
            <asp:DropDownList ID="ddlScaleTA" runat="server">
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="3">3</asp:ListItem>
            </asp:DropDownList><br />
            Thuốc thú y<br />
            <asp:DropDownList ID="ddlScaleTTY" runat="server">
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="3">3</asp:ListItem>
            </asp:DropDownList>
            <br />
        </td>
        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td valign="top" align="center">
            Sắp xếp theo<br />
            <asp:DropDownList ID="ddlOrderBy" runat="server">
                <asp:ListItem Value="Chuong">Ô chuồng</asp:ListItem>
                <asp:ListItem Value="Ngay">Ngày</asp:ListItem>
            </asp:DropDownList><br /><br />
            <asp:CheckBox ID="chkShowAll" Checked="true" runat="server" Text="Hiển thị tất cả<br/>chuồng có cá"/>
        </td>
        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td valign="top" align="center">
            Chuẩn loại thải<br />
            <asp:DropDownList ID="ddlChuan" runat="server">
                <asp:ListItem Value="1">Chuẩn cũ (15kg)</asp:ListItem>
                <asp:ListItem Value="2">Chuẩn mới</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td valign="top" align="center">
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/> 
        </td>
    </tr>
</table>
<br /><br /><br />
<asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>