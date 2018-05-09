<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_dinhmuc_thang.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_dinhmuc_thang" %>
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
    <tr><td><center><b>BÁO CÁO ĐỊNH MỨC THÁNG</b></center><br /></td></tr>
    <tr>
        <td>
            <span style="vertical-align:top;">Tháng</span>
            <asp:DropDownList ID="ddlThang" runat="server" style="vertical-align:top;">
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="3">3</asp:ListItem>
                <asp:ListItem Value="4">4</asp:ListItem>
                <asp:ListItem Value="5">5</asp:ListItem>
                <asp:ListItem Value="6">6</asp:ListItem>
                <asp:ListItem Value="7">7</asp:ListItem>
                <asp:ListItem Value="8">8</asp:ListItem>
                <asp:ListItem Value="9">9</asp:ListItem>
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="11">11</asp:ListItem>
                <asp:ListItem Value="12">12</asp:ListItem>
            </asp:DropDownList>
            <span style="vertical-align:top;">Năm</span>
            <asp:DropDownList ID="ddlNam" runat="server" style="vertical-align:top;">
            </asp:DropDownList>
            <asp:ListBox ID="ddlLoaiCa" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Loại cá"></asp:ListBox>
            <asp:Button ID="btnTyLeNuoiSong" runat="server" Text="Báo cáo tỷ lệ nuôi sống" OnClick="btnTyLeNuoiSong_Click" CssClass="button"/> 
            <asp:Button ID="btnTieuTonThucAn" runat="server" Text="Báo cáo tiêu tốn thức ăn" OnClick="btnTieuTonThucAn_Click" CssClass="button"/> 
            <asp:Button ID="btnTyLeDe" runat="server" Text="Báo cáo tỷ lệ đẻ" OnClick="btnTyLeDe_Click" CssClass="button"/> 
            <asp:Button ID="btnTyLeNo" runat="server" Text="Báo cáo tỷ lệ nở" OnClick="btnTyLeNo_Click" CssClass="button"/> 
            <asp:Button ID="btnAll" runat="server" Text="Báo cáo tổng hợp" OnClick="btnAll_Click" CssClass="button"/> 
        </td>
    </tr>
</table>