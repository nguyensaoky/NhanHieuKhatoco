<%@ Control Language="C#" AutoEventWireup="true" CodeFile="importchuyenchuong.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.importchuyenchuong" %>
<div id="wrapper" runat="server">
    <b>IMPORT CHUYỂN CHUỒNG</b><br/><br/>
    <b>CHÚ Ý: BACKUP CSDL TRƯỚC KHI THỰC HIỆN THAO TÁC NÀY</b><br/><br/>
    Giờ <asp:TextBox ID="txtGio" runat="server" Text="16"></asp:TextBox>
    <br />
    File import <input id="txtFile" runat="server" name="txtFile" type="file"/>
    <asp:Button ID="cmdUpload" runat="server" CausesValidation="False" OnClick="cmdUpload_Click" CssClass="button" Text="Import"/> 
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>