<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.NewsProvider.PagingControl" %>
<div style="text-align:center;width:100%;">
<TABLE id="tblPaging" cellSpacing="2" cellPadding="1" border="0" runat="server" class="Normal" style="margin:auto;">
	<TR align="center">
		<TD><asp:hyperlink id="lnkFirst" runat="server" ForeColor="#696969">|&lt;&lt;</asp:hyperlink></TD>
		<td>&nbsp;&nbsp;</td>
		<TD><asp:hyperlink id="lnkBack" runat="server" ForeColor="#696969">&lt;</asp:hyperlink></TD>
		<td>&nbsp;&nbsp;</td>
		<TD>
			<asp:Table ID="pagingList" Runat="server" CssClass="Normal"></asp:Table>
		</TD>
		<td>&nbsp;&nbsp;</td>
		<TD><asp:hyperlink id="lnkNext" runat="server" ForeColor="#696969" >&gt;</asp:hyperlink></TD>
		<td>&nbsp;&nbsp;</td>
		<TD><asp:hyperlink id="lnkLast" runat="server" ForeColor="#696969" >&gt;&gt;|</asp:hyperlink></TD>
	</TR>
</TABLE>
</div>