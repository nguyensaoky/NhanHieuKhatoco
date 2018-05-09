<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_dexuatthuoctheongay.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_dexuatthuoctheongay" %>
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
			leftColumns: 0
		} );
		//new $.fn.dataTableExt.sErrMode = 'throw';
	} );
</script>
<table>
    <tr><td><center><b>ĐỀ XUẤT THUỐC THEO NGÀY</b></center><br /></td></tr>
    <tr>
        <td valign="top" align="center">
            Từ ngày<br />
            <asp:TextBox ID="txtTuNgay" runat="server" TabIndex="6"/>
            <cc1:calendarextender id="calTuNgay" runat="server" format="dd/MM/yyyy" popupbuttonid="txtTuNgay" targetcontrolid="txtTuNgay"></cc1:calendarextender>
        </td>
        <td valign="top" align="center">
            Đến ngày<br />
            <asp:TextBox ID="txtDenNgay" runat="server" TabIndex="6"/>
            <cc1:calendarextender id="calDenNgay" runat="server" format="dd/MM/yyyy" popupbuttonid="txtDenNgay" targetcontrolid="txtDenNgay"></cc1:calendarextender>            
        </td>
        <td valign="top" align="center">
            Loại cá (Ctrl để chọn nhiều)<br />
            <asp:ListBox ID="lstLoaiCa" runat="server" SelectionMode="Multiple" Rows="6" style="vertical-align:top;">
            </asp:ListBox>
        </td>
        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td valign="top" align="center">
            Khu chuồng (Ctrl để chọn nhiều)<br />
            <asp:ListBox ID="lstKhuChuong" runat="server" SelectionMode="Multiple" Rows="6" style="vertical-align:top;">
            </asp:ListBox>
        </td>
        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td valign="top" align="center">
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
        </td>
    </tr>
</table>
<br /><br />
<asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>
<br />