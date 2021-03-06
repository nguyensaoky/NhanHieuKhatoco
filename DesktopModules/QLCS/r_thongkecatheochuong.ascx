﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_thongkecatheochuong.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_thongkecatheochuong" %>
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
	} );
</script>
<table>
    <tr><td><center><b>THỐNG KÊ CÁ THEO CHUỒNG</b></center><br /></td></tr>
    <tr>
        <td valign="top" align="center">
            Trước ngày<br />
            <asp:TextBox ID="txtDate" runat="server" Width="100" TabIndex="6"/>
            <cc1:calendarextender id="calDate" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtDate" targetcontrolid="txtDate"></cc1:calendarextender>
        </td>
        <td valign="top" align="center">
            Ô chuồng (Ctrl để chọn nhiều)<br />
            <asp:PlaceHolder ID="phChuong" runat="server"></asp:PlaceHolder>
        </td>
        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td valign="top" align="center">
            Loại cá (Ctrl để chọn nhiều)<br />
            <asp:ListBox ID="lstLoaiCa" runat="server" SelectionMode="Multiple" Rows="6" style="vertical-align:top;">
            </asp:ListBox>
        </td>
        <td>
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/> 
        </td>
    </tr>
</table>
<br />
<asp:HiddenField ID="hdNumListChuong" runat="server" Value="0"/>
<asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>