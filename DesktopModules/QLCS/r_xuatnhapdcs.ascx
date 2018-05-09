<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_xuatnhapdcs.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_xuatnhapdcs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<table>
    <tr><td><center><b>XUẤT NHẬP DA CÁ SẤU</b></center><br /></td></tr>
    <tr>
        <td>
            <span style="vertical-align:top;">Từ ngày</span>
            <asp:TextBox ID="txtFromDate" runat="server" Width="100" TabIndex="6" style="vertical-align:top;"/>
            <cc1:calendarextender id="calFromDate" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtFromDate" targetcontrolid="txtFromDate"></cc1:calendarextender>
            <span style="vertical-align:top;">đến ngày</span>
            <asp:TextBox ID="txtToDate" runat="server" Width="100" TabIndex="7" style="vertical-align:top;"/>
            <cc1:calendarextender id="calToDate" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtToDate" targetcontrolid="txtToDate"></cc1:calendarextender>&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>   
            <br /><br />
            <asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>
        </td>
    </tr>
</table>