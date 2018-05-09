<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_chuyenngayxuongchuong.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_chuyenngayxuongchuong" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<div id="wrapper" runat="server" class="wrapper_margin">
    <div style="width:200px;float:left;">Thời điểm xuống chuồng mới</div>
    <asp:TextBox ID="txtNgayXuongChuong" runat="server"></asp:TextBox>
    <br /><br />
    <div style="width:200px;float:left;">&nbsp;</div>
    <asp:Button ID="btnChuyen" runat="server" Text="Chuyển" OnClick="btnChuyen_Click" CssClass="button"/>
</div>