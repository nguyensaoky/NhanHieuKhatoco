<%@ Control Language="C#" AutoEventWireup="true" CodeFile="news_list.ascx.cs" Inherits="DotNetNuke.News.news_list" %>
<%@ Register Src="PagingControl.ascx" TagName="PagingControl" TagPrefix="pg" %>
<asp:Literal ID="ltTitle" runat="server"></asp:Literal>
<asp:Literal ID="ltNewsList" runat="server"></asp:Literal>
<div align="center"><pg:PagingControl id="paging" runat="server" Visible="false" /></div>
<div class="clear"></div>