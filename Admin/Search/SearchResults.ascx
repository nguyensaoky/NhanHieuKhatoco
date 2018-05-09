<%@ Control AutoEventWireup="true" CodeFile="SearchResults.ascx.cs"  Inherits="DotNetNuke.Modules.SearchResults.SearchResults" Language="C#" %>
<asp:DataGrid ID="dgResults" runat="server" AllowPaging="True" AutoGenerateColumns="False" BorderStyle="None" CellPadding="4" GridLines="None" OnPageIndexChanged="dgResults_PageIndexChanged"
    PagerStyle-CssClass="NormalBold" ShowHeader="False">
    <Columns>
        <asp:TemplateColumn ItemStyle-VerticalAlign="Top">
            <ItemTemplate>
                <asp:Label ID="lblNo" runat="server" CssClass="SubHead" Text='<%# ((int)(Convert.ToInt32(DataBinder.Eval(Container, "ItemIndex")) + 1)).ToString() + "." %>'>
                </asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:HyperLink ID="lnkTitle" runat="server" CssClass="SubHead" NavigateUrl='<%# FormatURL(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"TabId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"Guid")), Convert.ToString(DataBinder.Eval(Container.DataItem,"SearchKey"))) %>'
                    Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'>
                </asp:HyperLink>
                <br/>
                <asp:Label ID="lblSummary" runat="server" CssClass="Normal" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Description")) %>' Visible="<%# Convert.ToBoolean(ShowDescription()) %>">
                </asp:Label>
                <asp:HyperLink ID="lnkLink" runat="server" CssClass="CommandButton" NavigateUrl='<%# FormatURL(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"TabId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"Guid")), Convert.ToString(DataBinder.Eval(Container.DataItem,"SearchKey"))) %>'
                    Text='<%# FormatURL(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"TabId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"Guid")), Convert.ToString(DataBinder.Eval(Container.DataItem,"SearchKey"))) %>'>
                </asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
    <PagerStyle CssClass="NormalBold" Mode="NumericPages" />
</asp:DataGrid>
