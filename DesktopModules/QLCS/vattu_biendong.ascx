<%@ Control Language="C#" AutoEventWireup="true" CodeFile="vattu_biendong.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.vattu_biendong" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<div>
    <div style="background-color:#CCCCFF;">
    Tên vật tư: 
    <asp:Label ID="lblTenVatTu" runat="server"></asp:Label> - Đơn vị tính: <asp:Label ID="lblDonViTinh" runat="server"></asp:Label>
    </div>
    <br />
    <asp:Label ID="lblError" ForeColor="Red" runat="server"></asp:Label>
    <div style="width:200px;float:left;">Số lượng biến động</div>
    <asp:TextBox ID="txtSoLuongBienDong" runat="server"></asp:TextBox>
    <br />
    <div style="width:200px;float:left;">Ngày giờ biến động</div>
    <asp:TextBox ID="txtThoiDiemBienDong" runat="server"></asp:TextBox>
    <br />
    <div style="width:200px;float:left;">Loại biến động</div>
    <asp:DropDownList runat="server" ID="ddlLoaiBienDong" OnSelectedIndexChanged="ddlLoaiBienDong_SelectedIndexChanged" AutoPostBack="true">
    </asp:DropDownList>
    <br />
    <div runat="server" id="divNhaCungCap">
        <div style="width:200px;float:left;">Nhà cung cấp</div>
        <asp:DropDownList runat="server" ID="ddlNhaCungCap">
        </asp:DropDownList>
    </div>
    <br />
    <div style="width:200px;float:left;">Ghi chú</div>
    <asp:TextBox ID="txtNote" runat="server"></asp:TextBox><br /><br /><br />
    <div style="width:200px;float:left;">&nbsp;</div>
    <asp:Button ID="btnSave" runat="server" Text="Lưu biến động" OnClick="btnSave_Click" CssClass="button"/>
</div>