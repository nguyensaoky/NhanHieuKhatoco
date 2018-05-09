<%@ Control Language="C#" CodeFile="Search_Rounded.ascx.cs" AutoEventWireup="true" Inherits="DotNetNuke.UI.Skins.Controls.Search" %>
<div class="loginboxdiv">
    <asp:TextBox ID="txtSearch" runat="server" MaxLength="255" EnableViewState="False" CssClass="loginbox"></asp:TextBox>&nbsp;
</div>
<div style="margin-top:3px;">
    <asp:LinkButton ID="cmdSearch" runat="server" CausesValidation="False" CssClass="SkinObject" OnClick="cmdSearch_Click"></asp:LinkButton>
</div>
