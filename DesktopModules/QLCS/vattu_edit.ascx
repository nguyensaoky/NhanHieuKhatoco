<%@ Control Language="C#" AutoEventWireup="true" CodeFile="vattu_edit.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.vattu_edit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
    function initDonViTinh()
    {
        var customarray;
        var strDataValue = '<%=dataString%>';
        if(strDataValue != '')
        {
            customarray = strDataValue.split(";");
        }
        actb(document.getElementById('<%= txtDonViTinh.ClientID %>'), customarray, '<%= txtDonViTinh.Width.ToString() %>', '#EEEEEE', '#000000', '#C0C0EF');
    }
</script>
<table>
    <tr>
        <td>Tên vật tư</td>
        <td><asp:TextBox ID="txtTenVatTu" runat="server" Width="400" TabIndex="2"/>
        </td>
    </tr>
    <tr>
        <td>Đơn vị tính</td>
        <td>
            <asp:TextBox ID="txtDonViTinh" runat="server" Width="400" TabIndex="2"/>
        </td>
    </tr>
	<tr>
		<td colspan="2" align="center">
            <asp:Button ID="Save" runat="server" OnClick="btnSave_Click" CssClass="button" tabindex="12" Text="Lưu"/>
            <asp:HyperLink ID="lnkCancel" runat="server" Font-Bold="true" tabindex="13" CssClass="button">Thoát</asp:HyperLink>
		</td>
	</tr>
</table>