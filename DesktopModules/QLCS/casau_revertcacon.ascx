<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_revertcacon.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_revertcacon" %>
<div id="wrapper" runat="server">
    <b>REVERT CÁ CON</b><br/><br/>
    <b>CHÚ Ý: BACKUP CSDL TRƯỚC KHI THỰC HIỆN THAO TÁC NÀY</b><br/><br/>
    Người thực hiện <asp:DropDownList runat="server" ID="ddlUser"/>
    File import <input id="txtFile" runat="server" name="txtFile" type="file"/>
    <asp:Button ID="cmdUpload" runat="server" CausesValidation="False" OnClick="cmdUpload_Click" CssClass="button" Text="Import"/> 
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>