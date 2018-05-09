<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_gietmocatheovicat.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_gietmocatheovicat" %>
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
		
		var table1 = $('#thongke1').DataTable( {
			scrollY:        "500px",
			scrollX:        true,
			scrollCollapse: true,
			paging:         false,
			bFilter:        false,
			bInfo:          false,
			bSort:          false
		} );
		new $.fn.dataTable.FixedColumns( table1, {
			leftColumns: 1
		} );
	} );
</script>
<table>
    <tr><td><center><b>THỐNG KÊ GIẾT MỔ CÁ THEO VI CẮT</b></center><br /></td></tr>
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
            Hiển thị loại sản phẩm phụ (Ctrl để chọn nhiều)
            <asp:ListBox ID="lstVatTu" runat="server" SelectionMode="Multiple" Rows="4" style="vertical-align:top;">
            </asp:ListBox>
            Chuẩn loại thải
            <asp:DropDownList ID="ddlChuan" runat="server">
                <asp:ListItem Value="1">Chuẩn cũ (15kg)</asp:ListItem>
                <asp:ListItem Value="2">Chuẩn mới</asp:ListItem>
            </asp:DropDownList>
            Năm nở
            <asp:DropDownList ID="ddlNamNo" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
            <br /><br />
            <asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>
        </td>
    </tr>
</table>