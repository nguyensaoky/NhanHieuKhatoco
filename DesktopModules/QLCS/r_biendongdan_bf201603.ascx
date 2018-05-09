<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_biendongdan_bf201603.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_biendongdan_bf201603" %>
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
    <tr><td><center><b>BIẾN ĐỘNG ĐÀN</b></center><br /></td></tr>
    <tr>
        <td>
            Từ ngày&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtFromDate" runat="server" Width="100" TabIndex="6"/>
            <cc1:calendarextender id="calFromDate" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtFromDate" targetcontrolid="txtFromDate"></cc1:calendarextender>
            &nbsp;&nbsp;&nbsp;đến ngày&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtToDate" runat="server" Width="100" TabIndex="7"/>
            <cc1:calendarextender id="calToDate" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtToDate" targetcontrolid="txtToDate"></cc1:calendarextender>&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
            <br /><br />
            <asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>
        </td>
    </tr>
</table>