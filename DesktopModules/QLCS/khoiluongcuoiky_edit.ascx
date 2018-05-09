<%@ Control Language="C#" AutoEventWireup="true" CodeFile="khoiluongcuoiky_edit.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.khoiluongcuoiky_edit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<table>
    <tr>
        <td width="300">Năm cân</td>
        <td>
            <asp:TextBox ID="txtNamCan" runat="server" style="width:400px;" TabIndex="2"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng sơ sinh (kg)</td>
        <td>
            <asp:TextBox ID="txtSoSinh" runat="server" style="width:400px;" TabIndex="3" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ úm <b>GIỐNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyUm_Giong" runat="server" style="width:400px;" TabIndex="4" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ úm <b>TĂNG TRỌNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyUm_TT" runat="server" style="width:400px;" TabIndex="4" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ úm (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyUm" runat="server" style="width:400px;" TabIndex="4" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ 1 năm <b>GIỐNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKy1Nam_Giong" runat="server" style="width:400px;" TabIndex="4" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ 1 năm <b>TĂNG TRỌNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKy1Nam_TT" runat="server" style="width:400px;" TabIndex="4" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ 1 năm (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKy1Nam" runat="server" style="width:400px;" TabIndex="4" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ ST1 <b>GIỐNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyST1_Giong" runat="server" style="width:400px;" TabIndex="5" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ ST1 <b>TĂNG TRỌNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyST1_TT" runat="server" style="width:400px;" TabIndex="5" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ ST1 (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyST1" runat="server" style="width:400px;" TabIndex="5" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ ST2 <b>GIỐNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyST2_Giong" runat="server" style="width:400px;" TabIndex="6" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ ST2 <b>TĂNG TRỌNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyST2_TT" runat="server" style="width:400px;" TabIndex="6" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ ST2 (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyST2" runat="server" style="width:400px;" TabIndex="6" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ HB1 - 4 tuổi <b>GIỐNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyHB1_Giong" runat="server" style="width:400px;" TabIndex="7" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ HB1 - 4 tuổi <b>TĂNG TRỌNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyHB1_TT" runat="server" style="width:400px;" TabIndex="7" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ HB1 - 4 tuổi (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyHB1" runat="server" style="width:400px;" TabIndex="7" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ HB2 - 5 tuổi <b>GIỐNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyHB2_Giong" runat="server" style="width:400px;" TabIndex="8" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ HB2 - 5 tuổi <b>TĂNG TRỌNG</b> (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyHB2_TT" runat="server" style="width:400px;" TabIndex="8" Text="0"/>
        </td>
    </tr>
    <tr>
        <td>Khối lượng cuối kỳ HB2 - 5 tuổi (kg)</td>
        <td>
            <asp:TextBox ID="txtCuoiKyHB2" runat="server" style="width:400px;" TabIndex="8" Text="0"/>
        </td>
    </tr>
	<tr>
		<td colspan="2" align="center">
            <asp:Button ID="Save" runat="server" OnClick="btnSave_Click" CssClass="button" tabindex="12" Text="Lưu"/>
            <asp:Button ID="Delete" runat="server" OnClick="btnDelete_Click" CssClass="button" tabindex="13" Text="Xóa"/>
            <asp:HyperLink ID="lnkCancel" runat="server" Font-Bold="true" tabindex="13" CssClass="button">Thoát</asp:HyperLink>
		</td>
	</tr>
</table>