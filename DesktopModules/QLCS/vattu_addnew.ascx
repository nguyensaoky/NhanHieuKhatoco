<%@ Control Language="C#" AutoEventWireup="true" CodeFile="vattu_addnew.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.vattu_addnew" %>
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
        <td width="150">Loại vật tư</td>
        <td>
            <asp:DropDownList ID="ddlLoaiVatTu" runat="server" Style="width: 400px" TabIndex="1">
                <asp:ListItem Value="TA">Thức ăn</asp:ListItem>
                <asp:ListItem Value="TTY">Thuốc thú y</asp:ListItem>
                <asp:ListItem Value="SPGM">Sản phẩm giết mổ</asp:ListItem>
                <asp:ListItem Value="DCS">Da cá sấu</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
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
        <td>Tồn đầu</td>
        <td>
            <asp:TextBox ID="txtSoLuongHienTai" runat="server" Width="400" TabIndex="2"/>
        </td>
    </tr>
    <tr style="visibility:hidden;">
        <td visible="false">Tháng</td>
        <td visible="false">
            <asp:DropDownList ID="ddlThang" runat="server" TabIndex="1">
                <asp:ListItem Value="1">1</asp:ListItem>
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
            <asp:DropDownList ID="ddlNam" runat="server" TabIndex="1"/>
        </td>
    </tr>
	<tr>
		<td colspan="2" align="center">
            <asp:Button ID="Save" runat="server" OnClick="btnSave_Click" CssClass="button" tabindex="12" Text="Lưu"/>
            <asp:HyperLink ID="lnkCancel" runat="server" Font-Bold="true" tabindex="13" CssClass="button">Thoát</asp:HyperLink>
		</td>
	</tr>
</table>