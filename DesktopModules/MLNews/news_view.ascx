<%@ Control Language="C#" AutoEventWireup="true" CodeFile="news_view.ascx.cs" Inherits="DotNetNuke.News.news_view" %>
<%@ Register TagPrefix="dnn" TagName="WriteComment" Src="~/DesktopModules/MLNews/WriteComment.ascx" %>
<table width="100%" ID="tblContent" runat="server">
    <tr>
        <td class="NewsTitle">
            <asp:Label ID="lblHeadline" runat="server"></asp:Label><br />
        </td>
    </tr>
    <tr>
        <td class="NewsDescription">
           <asp:Label ID="lblDescription" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="NewsContent">
            <asp:Label ID="lblContent" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="Normal" align="right">
            <asp:HyperLink ID="lnkBooking" runat="server"></asp:HyperLink>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="Normal">
            <dnn:WriteComment ID="writeComment" runat="server"/>
        </td>
    </tr>
    <tr>
        <td class="ReaderComment">
            <asp:Label ID="lblReaderComment" runat="server" resourcekey="lblReaderComment"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
        </td>
    </tr>
    <tr>
        <td class="Normal" style="text-align:right;" >
            <asp:HiddenField ID="hdAllowComment" runat="server" />
        </td>
    </tr>
</table>