<%@ Control Language="C#" AutoEventWireup="true" CodeFile="cat_admin.ascx.cs" Inherits="DotNetNuke.News.cat_admin" %>
<asp:Label ID="lblRestricted" runat="server" resourcekey="lblRestricted" Visible="false"></asp:Label>
<table width="100%">
    <tr>
        <td align="right">
            <asp:HyperLink ID="lnkAdd" runat="server" resourcekey="lnkAddCat"></asp:HyperLink>
            <br />
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%" cellpadding="4" cellspacing="0">
              <tr style="background-color:#BBBBBB;">
                <td style="width:10%;text-align:center;"><asp:Label ID="lblOrderNumber" runat="server" resourcekey="lblOrderNumber"></asp:Label></td>
                <td style="width:10%;text-align:center;"><asp:Label ID="lblCatCode" runat="server" resourcekey="lblCatCode"></asp:Label></td>
                <td style="width:70%"><asp:Label ID="lblCatName" runat="server" resourcekey="lblCatName"></asp:Label></td>
                <td style="width:10%;text-align:center;"><asp:Label ID="lblEdit" runat="server" resourcekey="lblEdit"></asp:Label></td>
              </tr>
              <asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
            </table>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:HyperLink ID="lnkBack" runat="server" resourcekey="lnkBack"></asp:HyperLink>
        </td>
    </tr>
</table>