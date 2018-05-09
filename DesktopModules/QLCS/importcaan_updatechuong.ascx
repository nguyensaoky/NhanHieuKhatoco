<%@ Control Language="C#" AutoEventWireup="true" CodeFile="importcaan_updatechuong.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.importcaan_updatechuong" %>
<div id="wrapper" runat="server">
    <b>UPDATE CÁ ĂN</b><br/><br/>
    <b>CHÚ Ý: BACKUP CSDL TRƯỚC KHI THỰC HIỆN THAO TÁC NÀY</b><br/><br/>
    <br />Số sheet: <asp:TextBox ID="txtNumSheet" runat="server">1</asp:TextBox>
    <br />File update thức ăn<input id="txtFileThucAn" runat="server" name="txtFileThucAn" type="file"/>
    <asp:Button ID="cmdUploadThucAn" runat="server" CausesValidation="False" OnClick="cmdUploadThucAn_Click" CssClass="button" Text="Update Thức Ăn"/> 
    <br />File update thuốc<input id="txtFileThuoc" runat="server" name="txtFileThuoc" type="file"/>
    <asp:Button ID="cmdUploadThuoc" runat="server" CausesValidation="False" OnClick="cmdUploadThuoc_Click" CssClass="button" Text="Update Thuốc"/> 
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>