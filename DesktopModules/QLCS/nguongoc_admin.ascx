﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="nguongoc_admin.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.nguongoc_admin" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<div align="left">
    &nbsp;Trạng thái
    <asp:DropDownList ID="ddlActive" runat="server">
        <asp:ListItem Value="-1">-----</asp:ListItem>
        <asp:ListItem Value="1">Đang dùng</asp:ListItem>
        <asp:ListItem Value="0">Không dùng</asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="Load" runat="server" OnClick="btnLoad_Click" CssClass="button" tabindex="12" Text="Tải"/>
    <asp:HyperLink ID="lnkAddNew" runat="server" Text="Thêm nguồn gốc mới" CssClass="button"></asp:HyperLink>
</div>
<div style="width:100%;">
    <asp:GridView ID="grvDanhSach" runat="server"
        AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvDanhSach_RowDataBound"
        Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="Nguồn gốc">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkNguonGoc" runat="server"></asp:HyperLink>
                </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Đang dùng">
                <ItemTemplate>
                    <asp:CheckBox ID="chkActive" runat="server" Enabled="false"/>
                </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
        