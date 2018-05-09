<%@ Control Language="C#" AutoEventWireup="true" CodeFile="saoluudulieu.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.saoluudulieu" %>
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
            Sao lưu dữ liệu với tên:
        </td>
        <td>
            <asp:TextBox ID="txtBackupFileName" runat="server" TabIndex="6" Width="300"/>
        </td>
        <td>
            <asp:Button ID="btnBackup" runat="server" Text="Lưu dữ liệu" OnClick="btnBackup_Click" CssClass="button"/>
        </td>
    </tr>
</table>
<br /><br /><br /><br />