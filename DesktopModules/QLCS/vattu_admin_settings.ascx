<%@ Control Language="C#" AutoEventWireup="true" CodeFile="vattu_admin_settings.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.vattu_admin_settings" %>
<table width="100%">
    <tr>
        <td>Loại vật tư mặc định</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlLoaiVatTu">
                <asp:ListItem Value="TA">Thức ăn</asp:ListItem>
                    <asp:ListItem Value="TTY">Thuốc thú y</asp:ListItem>
                    <asp:ListItem Value="SPGM">Sản phẩm giết mổ</asp:ListItem>
                    <asp:ListItem Value="DCS_M">Da cá sấu</asp:ListItem>
                    <asp:ListItem Value="DCS_CBM">+ Da có bụng</asp:ListItem>
                    <asp:ListItem Value="DCS_CLM">+ Da có lưng</asp:ListItem>
                    <asp:ListItem Value="DCS_MDL">+ Da mổ dọc lưng</asp:ListItem>
                    <asp:ListItem Value="DCS_DDL">+ Đầu, Da lưng</asp:ListItem>
                    <asp:ListItem Value="DCS">Da cũ (không dùng nữa)</asp:ListItem>
                    <asp:ListItem Value="DCS_">+ Da cũ không bụng, lưng</asp:ListItem>
                    <asp:ListItem Value="DCS_CB">+ Da có bụng cũ</asp:ListItem>
                    <asp:ListItem Value="DCS_CL">+ Da có lưng cũ</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>