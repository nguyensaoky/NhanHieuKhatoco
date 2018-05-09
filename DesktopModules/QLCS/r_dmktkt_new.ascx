<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_dmktkt_new.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_dmktkt_new" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<table>
    <tr><td colspan="2"><center><b>CÁC CHỈ TIÊU ĐỊNH MỨC KINH TẾ KỸ THUẬT</b></center><br /></td></tr>
    <tr>
        <td valign="top">
            <asp:Label runat="server" style="vertical-align:top;">Chọn năm xem</asp:Label>
            <asp:ListBox ID="lstYear" runat="server" SelectionMode="Multiple"></asp:ListBox>
            <asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>    
            <asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
            <br />
            <asp:Literal ID="lt" runat="server" EnableViewState="false"></asp:Literal>
        </td>
    </tr>
</table>