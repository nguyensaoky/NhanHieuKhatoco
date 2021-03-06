<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MLCat.ascx.cs" Inherits="DotNetNuke.News.MLCat" %>
<table border="0" cellpadding="3" cellspacing="3" width="100%">
    <tr>
        <td>
            <asp:Label ID="lblLocale" runat="server" resourcekey="lblLocale" CssClass="Normal"></asp:Label></td>
        <td> 
            <asp:DropDownList ID="ddlLocale" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocale_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
</table>
<br />
<table border="0" cellpadding="3" cellspacing="3" width="100%">
    <tr>
        <td colspan="2" width="100%">
            <table border="0" cellpadding="3" cellspacing="3" width="100%">
                <tr>
                    <td style="width:80px"></td>
                    <td><asp:Label ID="lblStandardValues" runat="server" resourcekey="lblStandardValues" style="color:Gray" CssClass="NormalBold"></asp:Label></td>
                    <td><asp:Label ID="lblTranslations" runat="server" style="color:Gray" CssClass="NormalBold"></asp:Label></td>
                </tr>
                <asp:Repeater ID="repCats" runat="server" OnItemDataBound="repCats_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td><asp:Label ID="lbl_CatName" runat="server" style="color:Gray" resourcekey="lblCatName" CssClass="NormalBold"></asp:Label></td>
                            <td><asp:Label ID="lblCatName" runat="server" CssClass="NormalBold"></asp:Label></td>
                            <td><asp:TextBox ID="txtCatName" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr><td colspan="4" style="height:15px"><hr color="LightGrey" size="1" /><asp:HiddenField ID="CatId" runat="server" /></td></tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="center">
            <asp:LinkButton ID="btUpdate" runat="server" CssClass="CommandButton" resourcekey="btUpdate" OnClick="btUpdate_Click"></asp:LinkButton>
        </td>
    </tr>
</table>
