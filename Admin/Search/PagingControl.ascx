<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.NewsProvider.PagingControl" %>
<TABLE id="tblPaging" cellSpacing="2" cellPadding="1" border="0" runat="server" class="Normal">
	<TR align="center">
		<TD><asp:hyperlink id="lnkFirst" runat="server" Font-Bold="True" ForeColor="#696969">|&lt;&lt;</asp:hyperlink></TD>
		<td>&nbsp;&nbsp;</td>
		<TD><asp:hyperlink id="lnkBack" runat="server" Font-Bold="True" ForeColor="#696969">&lt;</asp:hyperlink></TD>
		<td>&nbsp;&nbsp;</td>
		<TD>
			<asp:Table ID="pagingList" Runat="server" CssClass="Normal"></asp:Table>
		</TD>
		<td>&nbsp;&nbsp;</td>
		<TD><asp:hyperlink id="lnkNext" runat="server" Font-Bold="True" ForeColor="#696969" >&gt;</asp:hyperlink></TD>
		<td>&nbsp;&nbsp;</td>
		<TD><asp:hyperlink id="lnkLast" runat="server" Font-Bold="True" ForeColor="#696969" >&gt;&gt;|</asp:hyperlink></TD>
	</TR>
</TABLE>