<%@ Control Language="C#" AutoEventWireup="true" CodeFile="importcade_chuacothailoai.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.importcade_chuacothailoai" %>
<div id="wrapper" runat="server">
    <b>IMPORT CÁ ĐẺ</b><br/><br/>
    <b>CHÚ Ý: BACKUP CSDL TRƯỚC KHI THỰC HIỆN THAO TÁC NÀY</b><br/><br/>
    File import <input id="txtFile" runat="server" name="txtFile" type="file"/>
    <asp:Button ID="cmdUpload" runat="server" CausesValidation="False" OnClick="cmdUpload_Click" CssClass="button" Text="Import"/> 
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>