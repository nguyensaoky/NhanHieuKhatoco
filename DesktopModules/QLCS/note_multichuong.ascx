<%@ Control Language="C#" AutoEventWireup="true" CodeFile="note_multichuong.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.note_multichuong" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<link href="<%= ModulePath + "style_main.css" %>" type="text/css" rel="stylesheet" />
<script type="text/javascript" language="javascript" src="<%= ModulePath + "script_main.js"%>"></script>
<script language="javascript" type="text/javascript">
    function confirmDelete()
    {
        var r=confirm("Bạn có chắc bạn muốn xóa?");
        if (r==true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
</script>
<table>
    <tr>
        <td>Ngày</td>
        <td>
            <asp:TextBox ID="txtNgay" runat="server" Width="70" TabIndex="2"/>
            <cc1:calendarextender id="calNgay" runat="server" format="dd/MM/yyyy"
            popupbuttonid="txtNgay" targetcontrolid="txtNgay"></cc1:calendarextender>
        </td>
    </tr>
    <tr>
        <td>Chuồng</td>
        <td>
            <asp:ListBox ID="ddlChuong" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="5" ToolTip="Chuồng"></asp:ListBox>
        </td>
    </tr>
	<tr>
		<td>Ghi chú</td>
		<td><asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Rows="4" TabIndex="4" Width="300"></asp:TextBox>
        </td>
	</tr>
	<tr>
		<td colspan="2" align="center">
		    <br />
            <asp:CheckBox ID="chkReplace" runat="server" Text="Thay thế"/>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Save" runat="server" OnClick="btnSave_Click" CssClass="button" tabindex="17" Text="Lưu"/>
            <asp:HyperLink ID="lnkCancel" runat="server" Font-Bold="true" CssClass="button" tabindex="18">Thoát</asp:HyperLink>
		</td>
	</tr>
</table>
<br /><br /><br /><br /><br />