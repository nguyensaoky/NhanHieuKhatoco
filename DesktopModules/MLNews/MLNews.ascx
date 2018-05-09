<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MLNews.ascx.cs" Inherits="DotNetNuke.News.MLNews" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<table border="0" cellpadding="3" cellspacing="3" width="100%">
    <tr>
        <td>
            <asp:Label ID="lblLocale" runat="server" resourcekey="lblLocale" CssClass="Normal"></asp:Label></td>
        <td> 
            <asp:DropDownList ID="ddlLocale" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocale_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblCatList" runat="server" resourcekey="lblCatList" CssClass="Normal"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlCat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCat_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblNewsList" runat="server" resourcekey="lblNewsList" CssClass="Normal"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlNews" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNews_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
</table>
<br />
<table border="0" cellpadding="3" cellspacing="3" width="100%">
    <tr>
        <td width="100%">
            <table border="0" cellpadding="3" cellspacing="3" width="100%">
                <tr>
                    <td style="width:80px">&nbsp;</td>
                    <td><asp:Label ID="lblStandardValues" runat="server" resourcekey="lblStandardValues" style="color:Gray" CssClass="NormalBold"></asp:Label></td>
                    <td><asp:Label ID="lblTranslations" runat="server" style="color:Gray" CssClass="NormalBold"></asp:Label></td>
                </tr>
                <tr>
                    <td><asp:Label ID="lbl_Headline" runat="server" style="color:Gray" resourcekey="lblHeadline" CssClass="NormalBold"></asp:Label></td>
                    <td><asp:Label ID="lblHeadline" runat="server" CssClass="NormalBold"></asp:Label></td>
                    <td><asp:TextBox ID="txtHeadline" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="3"><asp:Label ID="lbl_Description" runat="server" style="color:Gray" resourcekey="lblDescription" CssClass="NormalBold"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="pnlDescription" runat="server" Width="100%">
                            <dnn:texteditor id="txtDescription" runat="server" height="400" width="100%"></dnn:texteditor>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><asp:Label ID="lbl_Content" runat="server" style="color:Gray" resourcekey="lblContent" CssClass="NormalBold"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="pnlContent" runat="server" Width="100%">
                            <dnn:texteditor id="txtContent" runat="server" height="400" width="100%"></dnn:texteditor>
                        </asp:Panel>
                    </td>
                </tr>
                <tr><td colspan="3" style="height:15px"><hr color="LightGrey" size="1" /><asp:HiddenField ID="NewsId" runat="server" /></td></tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:LinkButton ID="btUpdate" runat="server" CssClass="CommandButton" resourcekey="btUpdate" OnClick="btUpdate_Click"></asp:LinkButton></td>
    </tr>
</table>
