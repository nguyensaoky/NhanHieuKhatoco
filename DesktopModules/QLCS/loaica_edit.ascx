﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="loaica_edit.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.loaica_edit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<table>
    <tr>
        <td width="100">Loại cá</td>
        <td>
            <asp:TextBox ID="txtLoaiCa" runat="server" style="width:400px;" TabIndex="2"/>
        </td>
    </tr>
    <tr>
        <td>Đang dùng</td>
        <td>
            <asp:CheckBox ID="chkActive" runat="server" Checked="true" TabIndex="4"/>
        </td>
    </tr>
	<tr>
		<td colspan="2" align="center">
            <asp:Button ID="Save" runat="server" OnClick="btnSave_Click" CssClass="button" tabindex="12" Text="Lưu"/>
            <asp:HyperLink ID="lnkCancel" runat="server" Font-Bold="true" tabindex="13" CssClass="button">Thoát</asp:HyperLink>
		</td>
	</tr>
</table>