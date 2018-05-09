<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WriteComment.ascx.cs" Inherits="DotNetNuke.News.WriteComment" %>
<table width="100%" bgcolor="#DDEEFF">
    <tr>
        <td colspan="2">
            <asp:Label ID="lblWriteComment" runat="server" resourcekey="lblWriteComment" ForeColor="ActiveCaption"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="Normal" width="10%">
            <asp:Label ID="lblName" runat="server" resourcekey="lblName"></asp:Label>    
        </td>
        <td width="90%">
            <asp:TextBox ID="txtName" runat="server" width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="Normal" width="10%">
            <asp:Label ID="lblEmail" runat="server" resourcekey="lblEmail"></asp:Label>    
        </td>
        <td width="90%">
            <asp:TextBox ID="txtEmail" runat="server" width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="Normal" width="10%">
            <asp:Label ID="lblHeadline" runat="server" resourcekey="lblHeadline"></asp:Label>    
        </td>
        <td width="90%">
            <asp:TextBox ID="txtHeadline" runat="server" width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="Normal" width="10%">
            <asp:Label ID="lblContent" runat="server" resourcekey="lblContent"></asp:Label>    
        </td>
        <td width="90%">
            <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="10" width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2" class="Normal">
            <asp:Label ID="lblSecureKey" runat="server" resourcekey="lblSecureKey"></asp:Label>
            <asp:TextBox ID="txtSecureKey" runat="server"></asp:TextBox>
            &nbsp;
            <asp:TextBox ID="txtSecure" runat="server" Enabled="False"></asp:TextBox>
            &nbsp;
            <asp:Label ID="lblNonValid" runat="server" resourcekey="lblNonValid" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="right">
            <asp:Button ID="btnSend" runat="server" resourcekey="btnSend" OnClick="btnSend_Click" CssClass="textbutton"/>
        </td>
    </tr>
</table>