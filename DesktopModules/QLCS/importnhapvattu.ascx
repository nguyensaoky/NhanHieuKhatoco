<%@ Control Language="C#" AutoEventWireup="true" CodeFile="importnhapvattu.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.importnhapvattu" %>
<div id="wrapper" runat="server">
    <b>IMPORT NHẬP VẬT TƯ</b><br/><br/>
    <b>CHÚ Ý: BACKUP CSDL TRƯỚC KHI THỰC HIỆN THAO TÁC NÀY</b><br/><br/>
    File import <input id="txtFile" runat="server" name="txtFile" type="file"/>
    <asp:Button ID="cmdUpload" runat="server" CausesValidation="False" OnClick="cmdUpload_Click" CssClass="button" Text="Import"/> 
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>