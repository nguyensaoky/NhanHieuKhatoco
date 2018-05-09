<%@ Control Language="C#" AutoEventWireup="true" CodeFile="cat_permission.ascx.cs" Inherits="DotNetNuke.News.cat_permission" %>

<table width="100%">
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
        <td colspan="2">
            <asp:GridView ID="grvRoles" runat="server" AutoGenerateColumns="False" BorderStyle="None" BorderColor="White" BorderWidth="0px">
                <Columns>
                    <asp:BoundField DataField="RoleID" HeaderText="ID" />
                    <asp:BoundField DataField="RoleName" HeaderText="Role" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkChoose" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="2">
            <asp:Button ID="btnUpdate" runat="server" resourcekey="btnUpdate" Text="Update" OnClick="btnUpdate_Click" />
            <asp:HyperLink ID="lnkBack" runat="server" resourcekey="lnkBack"></asp:HyperLink>
        </td>
    </tr>
</table>