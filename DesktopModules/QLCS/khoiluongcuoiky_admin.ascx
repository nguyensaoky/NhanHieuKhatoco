<%@ Control Language="C#" AutoEventWireup="true" CodeFile="khoiluongcuoiky_admin.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.khoiluongcuoiky_admin" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<div align="left">
    <asp:Button ID="Load" runat="server" OnClick="btnLoad_Click" CssClass="button" tabindex="12" Text="Tải"/>
    <asp:HyperLink ID="lnkAddNew" runat="server" Text="Thêm khối lượng cuối kỳ" CssClass="button"></asp:HyperLink>
</div>
<div style="width:100%;">
    <asp:GridView ID="grvDanhSach" runat="server"
        AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvDanhSach_RowDataBound"
        Width="100%" OnDataBound="grvDanhSach_DataBound">
        <Columns>
            <asp:TemplateField HeaderText="Năm cân">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkNamCan" runat="server"></asp:HyperLink>
                </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Khối lượng sơ sinh (kg)" DataField="SoSinh">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Giống" DataField="CuoiKyUm_Giong">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="TT" DataField="CuoiKyUm_TT">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Chung" DataField="CuoiKyUm">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Giống" DataField="CuoiKy1Nam_Giong">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="TT" DataField="CuoiKy1Nam_TT">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Chung" DataField="CuoiKy1Nam">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Giống" DataField="CuoiKyST1_Giong">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="TT" DataField="CuoiKyST1_TT">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Chung" DataField="CuoiKyST1">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Giống" DataField="CuoiKyST2_Giong">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="TT" DataField="CuoiKyST2_TT">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Chung" DataField="CuoiKyST2">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Giống" DataField="CuoiKyHB1_Giong">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="TT" DataField="CuoiKyHB1_TT">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Chung" DataField="CuoiKyHB1">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Giống" DataField="CuoiKyHB2_Giong">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="TT" DataField="CuoiKyHB2_TT">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Chung" DataField="CuoiKyHB2">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
        </Columns>
    </asp:GridView>
</div>