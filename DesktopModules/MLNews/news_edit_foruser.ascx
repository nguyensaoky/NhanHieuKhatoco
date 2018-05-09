<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="news_edit_foruser.ascx.cs" Inherits="DotNetNuke.News.news_edit_foruser" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<asp:Label ID="lblRestricted" runat="server" resourcekey="lblRestricted" Visible="false"></asp:Label>
<table width="100%">
	<tr>
		<td style="width:140px;"></td>
		<td><asp:Label ID="lblMessage" runat="server" Visible="false" Font-Bold="true" ForeColor="red"></asp:Label></td>
	</tr>
	<tr>
		<td><asp:Label ID="lblCategory" runat="server" resourcekey="lblCategory"></asp:Label></td>
		<td><asp:DropDownList ID="ddlCategory" runat="server"></asp:DropDownList></td>
	</tr>
	<tr><td colspan="2">&nbsp;</td></tr>
	<tr>
		<td colspan="2" align="left"><asp:Label ID="lblHeadline" runat="server" resourcekey="lblHeadline" Font-Bold="true"></asp:Label></td>
	</tr>
	<tr>
	    <td colspan="2" align="left"><asp:TextBox ID="txtHeadline" runat="server" style="width:100%"></asp:TextBox></td>
	</tr>
	<tr><td colspan="2">&nbsp;</td></tr>
    <tr>
        <td colspan="2" align="left"><asp:Label ID="lblDescription" runat="server" resourcekey="lblDescription" Font-Bold="true"></asp:Label></td>
    </tr>	
	<tr>
		<td colspan="2" align="left">
		    <dnn:texteditor id="txtDescription" runat="server" height="400" width="100%" ></dnn:texteditor>
		</td>
	</tr>
	<tr><td colspan="2">&nbsp;</td></tr>
	<tr>
	    <td colspan="2" align="left"><asp:Label ID="lblContent" runat="server" resourcekey="lblContent" Font-Bold="true"></asp:Label></td>
	</tr>
	<tr>
		<td colspan="2" align="left">
		    <dnn:texteditor id="teContent" runat="server" height="400" width="100%" ></dnn:texteditor>
		</td>
	</tr>
	<tr>
	    <td><asp:CheckBox ID="chkImageURL" runat="server" resourcekey="lblImageURL"></asp:CheckBox></td>
	    <td>
	        <portal:url id="ctlURL" runat="server" width="300" showfiles="true" ShowTabs="false" ShowUrls="false" urltype="F" showlog="False" shownewwindow="False" showtrack="False" />
	    </td>
	</tr>
	<tr>
		<td><asp:Label ID="lblAllowComment" runat="server" resourcekey="lblAllowComment"></asp:Label></td>
		<td><asp:CheckBox ID="chkAllowComment" runat="server"></asp:CheckBox></td>
	</tr>
	<tr>
		<td><asp:Label ID="lblPublish" runat="server" resourcekey="lblPublish"></asp:Label></td>
		<td><asp:CheckBox ID="chkPublished" runat="server"/></td>
	</tr>
	<tr>
		<td><asp:Label ID="lblKeyWords" runat="server" resourcekey="lblKeyWords"></asp:Label></td>
		<td><asp:TextBox ID="txtKeyWords" runat="server" style="width:300px;"></asp:TextBox></td>
	</tr>
	<tr>
	    <td><asp:Label ID="lblModifyDate" runat="server" resourcekey="lblModifyDate"></asp:Label></td>
	    <td>
            <asp:TextBox ID="txtModifyDate" runat="server"></asp:TextBox>
            <asp:HyperLink ID="cmdModifyDate" runat="server" CssClass="CommandButton" resourcekey="Calendar"></asp:HyperLink>
	    </td>
	</tr>
	<tr>
	    <td><asp:Label ID="lblStartDate" runat="server" resourcekey="lblStartDate"></asp:Label></td>
	    <td>
            <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
            <asp:HyperLink ID="cmdStartDate" runat="server" CssClass="CommandButton" resourcekey="Calendar"></asp:HyperLink>
	    </td>
	</tr>
	<tr>
	    <td><asp:Label ID="lblEndDate" runat="server" resourcekey="lblEndDate"></asp:Label></td>
	    <td>
            <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
            <asp:HyperLink ID="cmdEndDate" runat="server" CssClass="CommandButton" resourcekey="Calendar"></asp:HyperLink>
	    </td>
	</tr>
	<tr>
	    <td><asp:Label ID="lblFeature" runat="server" resourcekey="lblFeature"></asp:Label></td>
	    <td>
	        <asp:CheckBox ID="chkNew" runat="server" resourcekey="chkNew"/>
	        <asp:CheckBox ID="chkHot" runat="server" resourcekey="chkHot"/>
	    </td>
	</tr>
    <tr>
		<td colspan="2">
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
<asp:Label ID="lblCatString" runat="server" Visible="false"></asp:Label>