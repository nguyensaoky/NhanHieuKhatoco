<%@ Control Language="C#" AutoEventWireup="true" CodeFile="phuchoidulieu.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.phuchoidulieu" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<link href="<%= ModulePath + "style_main.css" %>" type="text/css" rel="stylesheet" />
<script type="text/javascript" language="javascript" src="<%= ModulePath + "script_main.js"%>"></script>
<style type="text/css">
    .TextButton
    {
        border: none;
        cursor: pointer;
        text-decoration: none;
        font-weight: bold;
        margin: 5px 0;
    }
</style>
<table>
    <tr>
        <td>
            Chọn file để phục hồi dữ liệu:
        </td>
        <td>
            <asp:DropDownList ID="ddlBackupFile" runat="server">
            </asp:DropDownList>
        </td>
        <td>
            <asp:Button ID="btnRestore" runat="server" Text="Phục hồi dữ liệu" OnClick="btnRestore_Click" CssClass="button"/>
        </td>
    </tr>
</table>
<br /><br /><br /><br />