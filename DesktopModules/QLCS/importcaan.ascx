<%@ Control Language="C#" AutoEventWireup="true" CodeFile="importcaan.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.importcaan" %>
<div id="wrapper" runat="server">
    <b>IMPORT CÁ ĂN</b><br/><br/>
    <b>CHÚ Ý: BACKUP CSDL TRƯỚC KHI THỰC HIỆN THAO TÁC NÀY</b><br/><br/>
    <br /><asp:CheckBox ID="chkValueAdd" runat="server" Text="Là giá trị thêm vào" Checked="true"/>
    <br />Số sheet: <asp:TextBox ID="txtNumSheet" runat="server">11</asp:TextBox>
    <br />Số chữ số thập phân <asp:TextBox ID="txtThapPhanTA" runat="server">1</asp:TextBox> File import thức ăn<input id="txtFileThucAn" runat="server" name="txtFileThucAn" type="file"/>
    <asp:Button ID="cmdUploadThucAn" runat="server" CausesValidation="False" OnClick="cmdUploadThucAn_Click" CssClass="button" Text="Import Thức Ăn"/>
    <asp:Button ID="cmdUpdateThucAnKhoiLuongChuong" runat="server" CausesValidation="False" OnClick="cmdUpdateThucAnKhoiLuongChuong_Click" CssClass="button" Text="Update Thức Ăn" Visible="false"/> 
    <br />Số chữ số thập phân <asp:TextBox ID="txtThapPhanT" runat="server">3</asp:TextBox> File import thuốc<input id="txtFileThuoc" runat="server" name="txtFileThuoc" type="file"/>
    <asp:Button ID="cmdUploadThuoc" runat="server" CausesValidation="False" OnClick="cmdUploadThuoc_Click" CssClass="button" Text="Import Thuốc"/> 
    <asp:Button ID="cmdUpdateThuocKhoiLuongChuong" runat="server" CausesValidation="False" OnClick="cmdUpdateThuocKhoiLuongChuong_Click" CssClass="button" Text="Update Thuốc" Visible="false"/> 
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>