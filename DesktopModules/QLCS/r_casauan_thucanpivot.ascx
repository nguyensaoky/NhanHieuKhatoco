﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_casauan_thucanpivot.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_casauan_thucanpivot" %>
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
    <tr><td><center><b>TỔNG HỢP THỨC ĂN THEO NGÀY</b></center><br /></td></tr>
    <tr>
        <td>
            <span style="vertical-align:top;">Từ ngày</span>
            <asp:TextBox ID="txtFromDate" runat="server" Width="100" TabIndex="6" style="vertical-align:top;"/>
            <cc1:calendarextender id="calFromDate" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtFromDate" targetcontrolid="txtFromDate"></cc1:calendarextender>
            <span style="vertical-align:top;">đến </span>
            <asp:TextBox ID="txtToDate" runat="server" Width="100" TabIndex="7" style="vertical-align:top;"/>
            <cc1:calendarextender id="calToDate" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtToDate" targetcontrolid="txtToDate"></cc1:calendarextender>&nbsp;&nbsp;&nbsp;
            <asp:ListBox ID="ddlLoaiCa" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Loại cá"></asp:ListBox>
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
        </td>
    </tr>
</table>