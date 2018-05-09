<%@ Control Language="C#" AutoEventWireup="true" CodeFile="general_admin.ascx.cs" Inherits="DotNetNuke.News.general_admin" %>
<asp:Label ID="lblRestricted" runat="server" resourcekey="lblRestricted" Visible="false"></asp:Label>
<table width="100%">
    <tr>
        <td align="left">
            <asp:HyperLink ID="lnkAdminCat" runat="server" resourcekey="lnkAdminCat"></asp:HyperLink>
            <br />
            <asp:HyperLink ID="lnkAdminGroup" runat="server" resourcekey="lnkAdminGroup"></asp:HyperLink>
            <br />
            <asp:HyperLink ID="lnkAdminNews" runat="server" resourcekey="lnkAdminNews"></asp:HyperLink>
            <br />
            <asp:HyperLink ID="lnkAdminComment" runat="server" resourcekey="lnkAdminComment"></asp:HyperLink>
            <br />
            <asp:HyperLink ID="lnkCatPermission" runat="server" resourcekey="lnkCatPermission"></asp:HyperLink>
        </td>
    </tr>
</table>