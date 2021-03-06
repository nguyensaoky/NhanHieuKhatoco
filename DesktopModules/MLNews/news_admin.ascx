﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="news_admin.ascx.cs" Inherits="DotNetNuke.News.news_admin" %>
<%@ Register Src="PagingControl.ascx" TagName="PagingControl" TagPrefix="pg" %>
<script type="text/javascript" language="javascript">
    function downloadfile(path)
    {
        var iframe = document.createElement("iframe");
        iframe.src = path;
        iframe.style.display = "none";
        document.body.appendChild(iframe); 
    }
</script>
<asp:Label ID="lblRestricted" runat="server" resourcekey="lblRestricted" Visible="false"></asp:Label>
<table width="100%">
    <tr>
        <td align="right" colspan="2">
            <asp:HyperLink ID="lnkAdd" runat="server" resourcekey="lnkAddNews"></asp:HyperLink>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="Label1" runat="server" Text="Từ khóa tìm kiếm" CssClass="Normal"></asp:Label><asp:TextBox ID="txtSearch" runat="server" Width="70"/>
            <asp:Label ID="lblCatList" runat="server" resourcekey="lblCatList" CssClass="Normal"></asp:Label>
            <asp:DropDownList ID="ddlCat" runat="server" AutoPostBack="false"></asp:DropDownList><br />
            <asp:Label ID="lblFromDate" runat="server" resourcekey="lblFromDate" CssClass="Normal"></asp:Label>&nbsp;
            <asp:TextBox ID="txtFromDate" runat="server" Width="70"/>&nbsp;<asp:HyperLink ID="lnkFromDate" runat="server" CssClass="CommandButton" resourcekey="lnkLich"></asp:HyperLink>&nbsp;
            <asp:Label ID="lblToDate" runat="server" resourcekey="lblToDate" CssClass="Normal"></asp:Label>&nbsp;
            <asp:TextBox ID="txtToDate" runat="server" Width="70"/>&nbsp;<asp:HyperLink ID="lnkToDate" runat="server" CssClass="CommandButton" resourcekey="lnkLich"></asp:HyperLink>&nbsp;
            <asp:Button ID="btnLoad" runat="server" resourcekey="btnLoad" OnClick="btnLoad_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0">
              <tr style="background-color:#BBBBBB;">
                <td style="width:20%"><asp:Label ID="lblCatName" runat="server" resourcekey="lblCatName"></asp:Label></td>
                <td style="width:70%"><asp:Label ID="lblHeadline" runat="server" resourcekey="lblHeadline"></asp:Label></td>
                <td><asp:Label ID="lblNew" runat="server" resourcekey="lblNew"></asp:Label></td>
                <td><asp:Label ID="lblHot" runat="server" resourcekey="lblHot"></asp:Label></td>
                <td><asp:Label ID="lblDate" runat="server" resourcekey="lblDate"></asp:Label></td>
                <td style="width:10%;text-align:center;"><asp:Label ID="lblEdit" runat="server" resourcekey="lblEdit"></asp:Label></td>
              </tr>
              <asp:Xml ID="xmlTransformer" runat="server"></asp:Xml></table>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="2">
            <pg:PagingControl id="paging" runat="server" Visible="false" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Excel</asp:LinkButton>
        </td>
        <td align="right">
            <asp:HyperLink ID="lnkBack" runat="server" resourcekey="lnkBack"></asp:HyperLink>
        </td>
    </tr>
</table>