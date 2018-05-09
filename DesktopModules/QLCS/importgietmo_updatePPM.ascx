<%@ Control Language="C#" AutoEventWireup="true" CodeFile="importgietmo_updatePPM.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.importgietmo_updatePPM" %>
<div id="wrapper" runat="server">
    <b>UPDATE PHƯƠNG PHÁP MỔ</b><br/><br/>
    <b>CHÚ Ý: BACKUP CSDL TRƯỚC KHI THỰC HIỆN THAO TÁC NÀY</b><br/><br/>
    File import <input id="txtFile" runat="server" name="txtFile" type="file"/>
    <asp:Button ID="cmdUpload" runat="server" CausesValidation="False" OnClick="cmdUpload_Click" CssClass="button" Text="Update PPM"/><br />
    Từ ngày&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="txtFromDate" runat="server" Width="100" TabIndex="6"/>
    &nbsp;&nbsp;&nbsp;đến ngày&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="txtToDate" runat="server" Width="100" TabIndex="7"/>
    &nbsp;&nbsp;&nbsp;
    <asp:Button ID="cmdTraoDoiSanPham" runat="server" CausesValidation="False" OnClick="cmdTraoDoiSanPham_Click" CssClass="button" Text="Trao đổi sản phẩm"/> 
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>