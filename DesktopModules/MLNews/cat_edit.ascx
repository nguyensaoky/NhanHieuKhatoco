<%@ Control Language="C#" AutoEventWireup="true" CodeFile="cat_edit.ascx.cs" Inherits="DotNetNuke.News.cat_edit" %>
<asp:Label ID="lblRestricted" runat="server" resourcekey="lblRestricted" Visible="false"></asp:Label>
<table width="100%">
	<tr>
		<td></td>
		<td><asp:Label ID="lblMessage" runat="server" Visible="false" Font-Bold="true" ForeColor="red"></asp:Label></td>
	</tr>
    <tr>
        <td><asp:label ID="lblCategoryID" runat="server" resourcekey="lblCategoryID"></asp:label></td>
        <td>
            <asp:TextBox ID="txtCatID" runat="server"></asp:TextBox>
            <asp:Label ID="lblCatID" runat="server" Font-Bold="true" Visible="false"></asp:Label>
        </td>
    </tr>
	<tr>
		<td><asp:label ID="lblCategoryName" runat="server" resourcekey="lblCategoryName"></asp:label></td>
		<td><asp:TextBox ID="txtCatName" runat="server" style="width:400px;"></asp:TextBox></td>
	</tr>
	<tr>
		<td><asp:label ID="lblCategoryCode" runat="server" resourcekey="lblCategoryCode"></asp:label></td>
		<td><asp:TextBox ID="txtCatCode" runat="server" Style="width: 400px"></asp:TextBox></td>
	</tr>
	<tr>
		<td><asp:label ID="lblDescription" runat="server" resourcekey="lblDescription"></asp:label></td>
		<td><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="10" style="width:100%;"></asp:TextBox></td>
	</tr>
	<tr>
		<td><asp:label ID="lblParent" runat="server" resourcekey="lblParent"></asp:label></td>
		<td><asp:DropDownList ID="ddlParentID" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><asp:label ID="lblOrderNumber" runat="server" resourcekey="lblOrderNumber"></asp:label></td>
		<td><asp:TextBox ID="txtOrderNumber" runat="server" style="width:80px;text-align:center;">0</asp:TextBox></td>
	</tr>
	<tr>
		<td><asp:label ID="lblDesktopListID" runat="server" resourcekey="lblDesktopListID"></asp:label></td>
		<td><asp:DropDownList ID="ddlDesktopListID" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><asp:label ID="lblDesktopViewID" runat="server" resourcekey="lblDesktopViewID"></asp:label></td>
		<td><asp:DropDownList ID="ddlDesktopViewID" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
	    <td><asp:label ID="lblNewsID" runat="server" resourcekey="lblNewsID"></asp:label></td>
	    <td>
	        <asp:DropDownList ID="ddlNews" runat="server" AutoPostBack="false"></asp:DropDownList>
	    </td>
	</tr>
	<tr>
	    <td><asp:label ID="lblBookingPageID" runat="server" resourcekey="lblBookingPageID"></asp:label></td>
	    <td><asp:DropDownList ID="ddlBookingPageID" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><asp:label ID="lblVisible" runat="server" resourcekey="lblVisible"></asp:label></td>
		<td><asp:CheckBox ID="chkVisible" runat="server" Checked="true"/></td>
	</tr>
	<tr>
	    <td>
	    </td>
		<td>
		    <table width="100%">
		        <tr>
		            <td style="width:90%">
                        <asp:Button ID="btnSave" runat="server" resourcekey="btnSave" OnClick="btnSave_Click"></asp:Button>
		                <asp:Button ID="btnDelete" runat="server" resourcekey="btnDelete" OnClick="btnDelete_Click"></asp:Button>
		            </td>
		            <td align="right">
		                <asp:Button ID="btnCancel" runat="server" resourcekey="btnCancel" OnClick="btnCancel_Click"></asp:Button>
		            </td>
		        </tr>
		    </table>
		</td>
	</tr>
</table>