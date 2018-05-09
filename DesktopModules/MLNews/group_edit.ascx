<%@ Control Language="C#" AutoEventWireup="true" CodeFile="group_edit.ascx.cs" Inherits="DotNetNuke.News.group_edit" %>
<asp:Label ID="lblRestricted" runat="server" resourcekey="lblRestricted" Visible="false"></asp:Label>
<table width="100%">
	<tr>
		<td></td>
		<td><asp:Label ID="lblMessage" runat="server" Visible="false" Font-Bold="true" ForeColor="red"></asp:Label></td>
	</tr>
    <tr>
        <td><asp:label ID="lblGroup" runat="server" resourcekey="lblGroupID"></asp:label></td>
        <td>
            <asp:TextBox ID="txtGroupID" runat="server"></asp:TextBox>
            <asp:Label ID="lblGroupID" runat="server" Font-Bold="true" Visible="false"></asp:Label>
        </td>
    </tr>
	<tr>
		<td><asp:label ID="lblGroupName" runat="server" resourcekey="lblGroupName"></asp:label></td>
		<td><asp:TextBox ID="txtGroupName" runat="server" style="width:400px;"></asp:TextBox></td>
	</tr>
	<tr>
		<td><asp:label ID="lblGroupCode" runat="server" resourcekey="lblGroupCode"></asp:label></td>
		<td><asp:TextBox ID="txtGroupCode" runat="server" Style="width: 400px"></asp:TextBox></td>
	</tr>
	<tr>
		<td><asp:label ID="lblDescription" runat="server" resourcekey="lblDescription"></asp:label></td>
		<td><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="10" style="width:100%;"></asp:TextBox></td>
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
	    <td>
	    </td>
		<td>
		    <table width="100%">
		        <tr>
		            <td style="width:90%">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" resourcekey="btnSave"></asp:Button>
		                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" resourcekey="btnDelete"></asp:Button>
		            </td>
		            <td align="right">
		                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" resourcekey="btnCancel"></asp:Button>
		            </td>
		        </tr>
		    </table>
		</td>
	</tr>
</table>