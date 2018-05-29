<%@ Control Language="C#" AutoEventWireup="true" CodeFile="nhanhieu_admin.ascx.cs" Inherits="DotNetNuke.Modules.NhanHieu.nhanhieu_admin" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "/style.css" %>" rel="stylesheet" type="text/css" />
<table>
    <tr>
        <td>
            Ngày công bố từ
            <asp:TextBox ID="txtNgayGuiFrom" runat="server" Width="80"/>
            <cc1:calendarextender id="calNgayGuiFrom" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtNgayGuiFrom" targetcontrolid="txtNgayGuiFrom"></cc1:calendarextender>
            đến
            <asp:TextBox ID="txtNgayGuiTo" runat="server" Width="80"/>
            <cc1:calendarextender id="calNgayGuiTo" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtNgayGuiTo" targetcontrolid="txtNgayGuiTo"></cc1:calendarextender>
            <asp:DropDownList runat="server" ID="ddlStatus">
                <asp:ListItem Value="0">----</asp:ListItem>
                <asp:ListItem Value="1">Chưa gửi</asp:ListItem>
                <asp:ListItem Value="2">Đã gửi</asp:ListItem>
                <asp:ListItem Value="3">Đang cấp mã</asp:ListItem>
                <asp:ListItem Value="4">Đã cấp mã</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnLoad" runat="server" OnClick="btnLoad_Click" CssClass="btnLoad"/>
        </td>
        <td align="right">
            <div class="divAddOrder" onclick="addorder()"/>
        </td>
    </tr>
</table>
<asp:GridView ID="grvDanhSach" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvDanhSach_RowDataBound">
    <Columns>
        <asp:TemplateField HeaderText="Mã phiếu">
            <ItemTemplate>
                <asp:HyperLink ID="lnkID" runat="server" Text='<%# Eval("ID") %>'/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="ThoiDiemGui" HeaderText="Ngày Gửi" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="ThoiDiemCoKetQua" HeaderText="Ngày Kết Quả" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Trạng thái">
            <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Button Style="display:none;" ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# Eval("ID") %>' CssClass="linkbutton"/>
                <a runat="server" id="delete" class="linkbutton">Xóa</a>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
