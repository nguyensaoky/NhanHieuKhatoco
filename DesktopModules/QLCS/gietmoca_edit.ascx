﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="gietmoca_edit.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.gietmoca_edit" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
    function KeyPress(event)
    {
        if (event.keyCode == 112)
        {
            var hdButtonID = document.getElementById('<%=hdButtonID.ClientID%>');
            document.getElementById(hdButtonID.value).click();
        }
    }
    function setSelectedRow(row, buttonid)
    {
        var hdOldRowID = document.getElementById('<%=hdOldRowID.ClientID%>');
        var hdOldColor = document.getElementById('<%=hdOldColor.ClientID%>');
        var hdButtonID = document.getElementById('<%=hdButtonID.ClientID%>');
        
        var or = document.getElementById(hdOldRowID.value);
        if(or != null) or.style.backgroundColor = hdOldColor.value;
        
        hdButtonID.value = buttonid;
        hdOldRowID.value = row.id;
        hdOldColor.value = row.style.backgroundColor;
        
        row.style.backgroundColor = "#ffffaa";
    }
    function openwindow(url, name, width, height)
    {
        var leftVal = (screen.width - width) / 2;
        var topVal = (screen.height - height) / 2;
        window.open(url,'','height=' + height + ',width=' + width +',toolbar=no,status=no,linemenubar=no,scrollbars=yes,resizable=yes,modal=yes,left=' + leftVal + ',top=' + topVal);
    }
</script>
<table width="100%">
    <tr>
        <td width="200">Thời điểm mổ</td>
        <td><asp:TextBox ID="txtNgayMo" runat="server" Width="400" TabIndex="2"/>
            <cc1:calendarextender id="calNgayMo" runat="server" format="dd/MM/yyyy 12:00:00"
                popupbuttonid="txtNgayMo" targetcontrolid="txtNgayMo"></cc1:calendarextender>
        </td>
    </tr>
    <tr>
        <td>Biên bản</td>
        <td>
            <asp:TextBox ID="txtBienBan" runat="server" Width="400" TabIndex="2" onkeydown="enterToTab(this,event);"/>
        </td>
    </tr>
    <tr>
        <td>Trọng lượng hơi (kg)</td>
        <td>
            <asp:TextBox ID="txtTrongLuongHoi" runat="server" Width="400" TabIndex="2" Enabled="false"/>
        </td>
    </tr>
    <tr>
        <td>Trọng lượng móc hàm (kg)</td>
        <td>
            <asp:TextBox ID="txtTrongLuongMocHam" runat="server" Width="400" TabIndex="2" Enabled="false"/>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td><asp:Button ID="Save" runat="server" OnClick="btnSave_Click" CssClass="button" tabindex="12" Text="Lưu"/></td>
    </tr>
</table>
<br />
<table width="100%">
    <tr>
        <td width="50%" valign="top">
            <div style="font-weight:bold;text-align:left;">SẢN PHẨM GIẾT MỔ:</div><br />
            <asp:PlaceHolder ID="dsSanPham" runat="server"></asp:PlaceHolder>
        </td>
        <td width="50%" valign="top">
            <div style="font-weight:bold;text-align:left;">DA CÁ SẤU:</div><br />
            <asp:PlaceHolder ID="dsDCS" runat="server"></asp:PlaceHolder>
        </td>
    </tr>
</table>
<table width="100%">
	<tr>
		<td align="center">
		    <asp:HyperLink ID="lnkChiTiet" runat="server" Font-Bold="true" tabindex="13" CssClass="button">Danh sách cá giết mổ</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:HyperLink ID="lnkCancel" runat="server" Font-Bold="true" tabindex="13" CssClass="button">Thoát</asp:HyperLink>
		</td>
	</tr>
</table>
<asp:Label ID="lblCanEdit" runat="server" Visible="false"></asp:Label>
<asp:HiddenField ID="hdSanPham" runat="server" />
<asp:HiddenField ID="hdOldRowID" runat="server" Value=""/>
<asp:HiddenField ID="hdOldColor" runat="server" Value=""/>
<asp:HiddenField ID="hdButtonID" runat="server" Value=""/>