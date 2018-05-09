<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_biendongdan.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_biendongdan" %>
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
            Chuẩn loại thải
            <asp:DropDownList ID="ddlChuan" runat="server">
                <asp:ListItem Value="1">Chuẩn cũ (15kg)</asp:ListItem>
                <asp:ListItem Value="2">Chuẩn mới</asp:ListItem>
            </asp:DropDownList>
            <cc1:calendarextender id="calToDate" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtToDate" targetcontrolid="txtToDate"></cc1:calendarextender>&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>             
        </td>
    </tr>
    <tr>
        <td>
            Xem chuyển giai đoạn năm 
            <asp:DropDownList ID="ddlNam" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btnViewCGD" runat="server" Text="Xem" OnClick="btnViewCGD_Click" CssClass="button"/>
            <asp:Button ID="btnExcelCGD" runat="server" Text="Xuất Excel" OnClick="btnExcelCGD_Click" CssClass="button"/>
        </td>
    </tr>
    <tr>
        <td>
            Xem biến động đàn bao gồm chuyển giai đoạn năm 
            <asp:DropDownList ID="ddlNam_GomCGD" runat="server">
            </asp:DropDownList>&nbsp;&nbsp;&nbsp;
            tính đến ngày
            <asp:TextBox ID="txtToDate_GomCGD" runat="server" Width="100" TabIndex="7"/>
            <cc1:calendarextender id="claToDate_GomCGD" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtToDate_GomCGD" targetcontrolid="txtToDate_GomCGD"></cc1:calendarextender>
            Chuẩn loại thải
            <asp:DropDownList ID="ddlChuan_GomCGD" runat="server">
                <asp:ListItem Value="1">Chuẩn cũ (15kg)</asp:ListItem>
                <asp:ListItem Value="2">Chuẩn mới</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnView_GomCGD" runat="server" Text="Xem" OnClick="btnView_GomCGD_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel_GomCGD" runat="server" Text="Xuất Excel" OnClick="btnExcel_GomCGD_Click" CssClass="button"/> 
        </td>
    </tr>
    <tr>
        <td>
            <br /><br />
            <asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>
        </td>
    </tr>
</table>