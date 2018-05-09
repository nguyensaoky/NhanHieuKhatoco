<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_xuatnhapthucan.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_xuatnhapthucan" %>
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
    <tr><td colspan="2"><center><b>XUẤT NHẬP THỨC ĂN THEO NGÀY</b></center><br /><asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label></td></tr>
    <tr>
        <td valign="top">
            <table>
                <tr>
                    <td align="left">
                        <span style="vertical-align:top;">Từ ngày</span>
                    </td>
                    <td align="left">
                        <span style="vertical-align:top;">Đến ngày</span>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:TextBox ID="txtFromDate" runat="server" Width="100" TabIndex="6" style="vertical-align:top;"/>
                        <cc1:calendarextender id="calFromDate" runat="server" format="dd/MM/yyyy"
                            popupbuttonid="txtFromDate" targetcontrolid="txtFromDate"></cc1:calendarextender>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtToDate" runat="server" Width="100" TabIndex="7" style="vertical-align:top;"/>
                        <cc1:calendarextender id="calToDate" runat="server" format="dd/MM/yyyy"
                            popupbuttonid="txtToDate" targetcontrolid="txtToDate"></cc1:calendarextender>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <span style="vertical-align:top;">Loại báo cáo</span>
                    </td>
                    <td align="left">
                        <span style="vertical-align:top;">Sắp xếp thức ăn</span>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:DropDownList ID="ddlLoaiBC" runat="server" style="vertical-align:top;">
                            <asp:ListItem Value="TongHop">Tổng hợp</asp:ListItem>
                            <asp:ListItem Value="Nhap">Chi tiết nhập</asp:ListItem>
                            <asp:ListItem Value="NhapBT">+ Nhập</asp:ListItem>
                            <asp:ListItem Value="NhapDC">+ Nhập điều chỉnh</asp:ListItem>
                            <asp:ListItem Value="Xuat">Chi tiết xuất</asp:ListItem>
                            <asp:ListItem Value="XuatBT">+ Xuất</asp:ListItem>
                            <asp:ListItem Value="XuatDC">+ Xuất điều chỉnh</asp:ListItem>
                            <asp:ListItem Value="XuatH">+ Hủy</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSapXep" runat="server" style="vertical-align:top;">
                            <asp:ListItem Value="Doc">hàng dọc</asp:ListItem>
                            <asp:ListItem Value="Ngang">hàng ngang</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <asp:ListBox ID="ddlTA" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="6" ToolTip="Thức ăn"></asp:ListBox>
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>
        </td>
    </tr>
</table>