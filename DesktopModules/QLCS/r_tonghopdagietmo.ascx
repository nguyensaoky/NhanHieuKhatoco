<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_tonghopdagietmo.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_tonghopdagietmo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/controls/SectionHeadControl.ascx" TagName="SectionHead" TagPrefix="dnn" %>
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
    <tr><td><center><b>TỔNG HỢP DA THU HỒI TỪ CÁ GIẾT MỔ</b></center><br /></td></tr>
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
            Loại cá
            <asp:DropDownList ID="ddlLoaiCa" runat="server">
            </asp:DropDownList>
            HÌnh thức
            <asp:DropDownList ID="ddlHinhThuc" runat="server">
                <asp:ListItem Selected="True" Value="0">Giết mổ & chết</asp:ListItem>
                <asp:ListItem Value="1">Giết mổ</asp:ListItem>
                <asp:ListItem Value="2">Chết</asp:ListItem>
            </asp:DropDownList>
            <br />
            Ô chuồng (Ctrl để chọn nhiều)<br />
            <asp:PlaceHolder ID="phChuong" runat="server"></asp:PlaceHolder>
            <br /><br />Loại da (Ctrl để chọn nhiều)<br />
            <asp:ListBox ID="ddlDaCBMoi" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="6" ToolTip="Loại da CB"></asp:ListBox>
            <asp:ListBox ID="ddlDaCLMoi" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="6" ToolTip="Loại da CL"></asp:ListBox>
            <asp:ListBox ID="ddlDaMDL" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="6" ToolTip="Loại da MDL"></asp:ListBox>
            <dnn:SectionHead ID="shDaCu" runat="server" IncludeRule="True" Section="divDaCu" Text="Da loại cũ" IsExpanded="false"/>
            <div id="divDaCu" runat="server" border="0" cellpadding="2" cellspacing="0" summary="Da loại cũ">
                <asp:ListBox ID="ddlDa" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="6" ToolTip="Da không bụng, lưng cũ"></asp:ListBox>
                <asp:ListBox ID="ddlDaCB" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="6" ToolTip="Loại da CB cũ"></asp:ListBox>
                <asp:ListBox ID="ddlDaCL" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="6" ToolTip="Loại da CL cũ"></asp:ListBox>
            </div>
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/> 
            <br /><br />
            <asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdNumListChuong" runat="server" Value="0"/>