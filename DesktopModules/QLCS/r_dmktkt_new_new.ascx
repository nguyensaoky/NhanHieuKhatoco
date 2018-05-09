<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_dmktkt_new_new.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_dmktkt_new_new" %>
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
    <tr><td><center><b>CÁC CHỈ TIÊU ĐỊNH MỨC KINH TẾ KỸ THUẬT (ÁP DỤNG TỪ 2016)</b></center><br /></td></tr>
    <tr>
        <td>
            <asp:Label runat="server" style="vertical-align:top;">Chọn năm xem</asp:Label>
            <asp:ListBox ID="lstYear" runat="server" SelectionMode="Multiple"></asp:ListBox>
            <span style="vertical-align:top;">
                Chuẩn loại thải
                <asp:DropDownList ID="ddlChuan" runat="server" style="vertical-align:top;">
                    <asp:ListItem Value="1">Chuẩn cũ (15kg)</asp:ListItem>
                    <asp:ListItem Value="2">Chuẩn mới</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span style="vertical-align:top;">
                Cách tính số lượng cá trung bình
                <asp:DropDownList ID="ddlCachTinh" runat="server" style="vertical-align:top;">
                    <asp:ListItem Value="1">Bình quân gia quyền</asp:ListItem>
                    <asp:ListItem Value="2">Theo ngày ăn (cách cũ)</asp:ListItem>
                </asp:DropDownList>
            </span>
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
            <br />
            <asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>
        </td>
    </tr>
</table>