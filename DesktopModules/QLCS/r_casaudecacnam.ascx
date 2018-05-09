<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_casaudecacnam.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_casaudecacnam" %>
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
	function openwindow(url, name, width, height)
    {
        var leftVal = (screen.width - width) / 2;
        var topVal = (screen.height - height) / 2;
        window.open(url,'','height=' + height + ',width=' + width +',toolbar=no,status=no,linemenubar=no,scrollbars=yes,resizable=yes,modal=yes,left=' + leftVal + ',top=' + topVal);
    }
</script>
<table>
    <tr><td colspan="2"><center><b>DANH SÁCH CÁ SẤU CÁI VÀ LỊCH SỬ ĐẺ THEO NĂM</b></center><br /></td></tr>
    <tr>
        <td>
            <span style="vertical-align:top;">Xem từ năm</span>
            <asp:TextBox ID="txtFromYear" runat="server" Width="100" TabIndex="6" style="vertical-align:top;"/>
            <span style="vertical-align:top;">đến năm</span>
            <asp:TextBox ID="txtYear" runat="server" Width="100" TabIndex="6" style="vertical-align:top;"/>
            <asp:ListBox ID="ddlLoaiCa" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Loại cá"></asp:ListBox>
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
            <br /><br />
            <asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdListBienDongPage" runat="server" Value="0" EnableViewState="true"/>