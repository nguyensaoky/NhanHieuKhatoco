<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_vattu_nhacungcap.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_vattu_nhacungcap" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/controls/SectionHeadControl.ascx" TagName="SectionHead" TagPrefix="dnn" %>
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
	
	function selectDeselect(listid, status)
	{  
        var listb = document.getElementById(listid);  
        var len = listb.options.length;  
        for (var i = 0; i < len; i++) {  
            listb.options[i].selected = status;  
        }  
    }  
</script>
<table>
    <tr><td><center><b>THEO DÕI NHẬP VẬT TƯ</b></center><br /><asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label></td></tr>
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
                    <td align="left">
                        <span style="vertical-align:top;">Loại vật tư</span>
                    </td>
                    <td></td>
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
                    <td align="left">
                        <asp:DropDownList ID="ddlLoaiVatTu" runat="server" style="vertical-align:top;" Enabled="false">
                            <asp:ListItem Value="TA" Selected="True">Thức ăn</asp:ListItem>
                            <asp:ListItem Value="TTY">Thuốc thú y</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
                        <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            Xuất Excel loại vật tư
            <asp:DropDownList ID="ddlLoaiVatTu1" runat="server" style="vertical-align:top;" Enabled="false">
                <asp:ListItem Value="TA" Selected="True">Thức ăn</asp:ListItem>
                <asp:ListItem Value="TTY">Thuốc thú y</asp:ListItem>
            </asp:DropDownList>
            từ tháng
            <asp:DropDownList ID="ddlThangFrom" runat="server" style="vertical-align:top;" AutoPostBack="false">
                <asp:ListItem Value="1" Selected="True">1</asp:ListItem>
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
            năm
            <asp:DropDownList ID="ddlNamFrom" runat="server" style="vertical-align:top;" AutoPostBack="false">
            </asp:DropDownList>
            đến tháng
            <asp:DropDownList ID="ddlThangTo" runat="server" style="vertical-align:top;" AutoPostBack="false">
                <asp:ListItem Value="1" Selected="True">1</asp:ListItem>
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
            năm
            <asp:DropDownList ID="ddlNamTo" runat="server" style="vertical-align:top;" AutoPostBack="false">
            </asp:DropDownList>
            <asp:Button ID="btnExcelThang" runat="server" Text="Xuất Excel" OnClick="btnExcelThang_Click" CssClass="button"/>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>
        </td>
    </tr>
</table>