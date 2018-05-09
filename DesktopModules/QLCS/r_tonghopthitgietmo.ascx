<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_tonghopthitgietmo.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_tonghopthitgietmo" %>
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
			leftColumns: 2
		} );
	} );
</script>
<table>
    <tr><td colspan="2"><center><b>TỔNG HỢP THỊT THU HỒI TỪ CÁ GIẾT MỔ</b></center><br /></td></tr>
    <tr>
        <td>
            <b>Theo ngày</b>
        </td>
        <td>
            <span style="vertical-align:top;">Từ ngày</span>
            <asp:TextBox ID="txtFromDate" runat="server" Width="100" TabIndex="6" style="vertical-align:top;"/>
            <cc1:calendarextender id="calFromDate" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtFromDate" targetcontrolid="txtFromDate"></cc1:calendarextender>
            <span style="vertical-align:top;">đến </span>
            <asp:TextBox ID="txtToDate" runat="server" Width="100" TabIndex="7" style="vertical-align:top;"/>
            <cc1:calendarextender id="calToDate" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtToDate" targetcontrolid="txtToDate"></cc1:calendarextender>
            Loại cá
            <asp:DropDownList ID="ddlLoaiCaNgay" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btnViewNgay" runat="server" Text="Xem" OnClick="btnViewNgay_Click" CssClass="button"/>    
            <asp:Button ID="btnExcelNgay" runat="server" Text="Xuất Excel" OnClick="btnExcelNgay_Click" CssClass="button"/> 
        </td>
    </tr>
    <tr><td colspan="2"><br /></td></tr>
    <tr>
        <td width="100">
            <b>Theo tháng</b>
        </td>
        <td>
            Từ tháng&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="ddlFromMonth" runat="server">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
            </asp:DropDownList> năm
            <asp:TextBox ID="txtFromYear" runat="server" Width="100" TabIndex="6"/>
            &nbsp;&nbsp;&nbsp;đến tháng&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="ddlToMonth" runat="server">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
            </asp:DropDownList> năm
            <asp:TextBox ID="txtToYear" runat="server" Width="100" TabIndex="6"/>
            Loại cá
            <asp:DropDownList ID="ddlLoaiCaThang" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btnViewThang" runat="server" Text="Xem" OnClick="btnViewThang_Click" CssClass="button"/>    
            <asp:Button ID="btnExcelThang" runat="server" Text="Xuất Excel" OnClick="btnExcelThang_Click" CssClass="button"/> 
        </td>
    </tr>
    <tr><td colspan="2"><br /></td></tr>
    <tr>
        <td>
            <b>Theo quý</b>
        </td>
        <td>
            Năm&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtYear" runat="server" Width="100" TabIndex="6"/>
            Loại cá
            <asp:DropDownList ID="ddlLoaiCaQuy" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btnViewQuy" runat="server" Text="Xem" OnClick="btnViewQuy_Click" CssClass="button"/>
            <asp:Button ID="btnExcelQuy" runat="server" Text="Xuất Excel" OnClick="btnExcelQuy_Click" CssClass="button"/>
        </td>
    </tr>
</table>
<asp:Literal ID="ltNgay" runat="server" EnableViewState="false"></asp:Literal>  
<asp:Literal ID="ltThang" runat="server" EnableViewState="false"></asp:Literal>  
<asp:Literal ID="ltQuy" runat="server" EnableViewState="false"></asp:Literal>    