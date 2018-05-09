<%@ Control Language="C#" AutoEventWireup="true" CodeFile="import_updatecaan.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.import_updatecaan" %>
<div id="wrapper" runat="server">
    <b>IMPORT UPDATE CÁ ĂN</b><br/><br/>
    <b>CHÚ Ý: BACKUP CSDL TRƯỚC KHI THỰC HIỆN THAO TÁC NÀY</b><br/><br/>
    <br />Số sheet: <asp:TextBox ID="txtNumSheet" runat="server">11</asp:TextBox>
    <br />Số chữ số thập phân <asp:TextBox ID="txtThapPhanTA" runat="server">1</asp:TextBox> File import thức ăn<input id="txtFileThucAn" runat="server" name="txtFileThucAn" type="file"/>
    <asp:Button ID="cmdUploadThucAn" runat="server" CausesValidation="False" OnClick="cmdUploadThucAn_Click" CssClass="button" Text="Update Thức Ăn"/>
    <br />Số chữ số thập phân <asp:TextBox ID="txtThapPhanT" runat="server">3</asp:TextBox> File import thuốc<input id="txtFileThuoc" runat="server" name="txtFileThuoc" type="file"/>
    <asp:Button ID="cmdUploadThuoc" runat="server" CausesValidation="False" OnClick="cmdUploadThuoc_Click" CssClass="button" Text="Update Thuốc"/> 
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>