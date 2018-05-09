<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_thongkechuyengiaidoan_chitiet.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_thongkechuyengiaidoan_chitiet" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" type="text/css" rel="stylesheet" />
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
	} );
</script>
<table>
    <tr><td colspan="2"><center><b>THỐNG KÊ CHI TIẾT CÁ CHUYỂN GIAI ĐOẠN</b></center><br /></td></tr>
    <tr>
        <td valign="top" align="center">
            Năm (Ctrl để chọn nhiều)<br />
            <asp:ListBox ID="lstYear" runat="server" SelectionMode="Multiple" Rows="6" style="vertical-align:top;">
            </asp:ListBox>
        </td>
        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td valign="top" align="center">
            Loại cá (Ctrl để chọn nhiều)<br />
            <asp:ListBox ID="lstLoaiCa" runat="server" SelectionMode="Multiple" Rows="6" style="vertical-align:top;">
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
            <br />
            <asp:CheckBox ID="chkDuLieuSinhSan" runat="server" Text="Hiển thị dữ liệu sinh sản"/>
        </td>
        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td valign="top" align="center">
            <br />
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
        </td>
    </tr>
</table>
<br /><br /><br />
<asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>