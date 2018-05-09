<%@ Control Language="C#" CodeFile="Search_New.ascx.cs" AutoEventWireup="true" 
    Inherits="DotNetNuke.UI.Skins.Controls.Search_New" %>
<table width="100%">
    <tr>
        <td align="center">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="NormalTextBox" Columns="30" MaxLength="255" EnableViewState="False"></asp:TextBox>        
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:LinkButton ID="cmdSearch" runat="server" CausesValidation="False" CssClass="SkinObject" OnClick="cmdSearch_Click"></asp:LinkButton>
        </td>
    </tr>
</table>