<%@ Control Language="C#" AutoEventWireup="true" CodeFile="nhanhieu_admin.ascx.cs" Inherits="DotNetNuke.Modules.NhanHieu.nhanhieu_admin" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "/style.css" %>" rel="stylesheet" type="text/css" />
<<<<<<< HEAD
<script language="javascript" type="text/javascript">
    function addnhanhieu()
    {
        self.location='<%=DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "nhanhieu_edit", "mid/" + this.ModuleId.ToString())%>';
    }
</script>
<table>
    <tr>
        <td>
            <asp:TextBox ID="txtSearch" runat="server" Width="200" PlaceHolder="tìm kiếm"/>
            Ngày chứng nhận từ<asp:TextBox ID="txtNgayChungNhanFrom" runat="server" Width="80"/>
            <cc1:calendarextender id="calNgayChungNhanFrom" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtNgayChungNhanFrom" targetcontrolid="txtNgayChungNhanFrom"></cc1:calendarextender>
=======
<table>
    <tr>
        <td>
            Ngày công bố từ
            <asp:TextBox ID="txtNgayGuiFrom" runat="server" Width="80"/>
            <cc1:calendarextender id="calNgayGuiFrom" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtNgayGuiFrom" targetcontrolid="txtNgayGuiFrom"></cc1:calendarextender>
>>>>>>> 60f830380f14438990695216c6b4c6f073a69a66
            đến
            <asp:TextBox ID="txtNgayChungNhanTo" runat="server" Width="80"/>
            <cc1:calendarextender id="calNgayChungNhanTo" runat="server" format="dd/MM/yyyy"
                popupbuttonid="txtNgayChungNhanTo" targetcontrolid="txtNgayChungNhanTo"></cc1:calendarextender>
            Nhãn hiệu gốc
            <asp:DropDownList runat="server" ID="ddlNhanHieuGoc">
            </asp:DropDownList>
            <asp:DropDownList runat="server" ID="ddlNuocDangKy">
            </asp:DropDownList>
            <asp:DropDownList runat="server" ID="ddlDonVi">
            </asp:DropDownList>
            <asp:DropDownList runat="server" ID="ddlLinhVuc">
            </asp:DropDownList>
            <asp:DropDownList runat="server" ID="ddlStatus">
            </asp:DropDownList>
            <asp:Button ID="btnLoad" runat="server" OnClick="btnLoad_Click" CssClass="btnLoad" Text="Tải dữ liệu"/>
        </td>
        <td align="right">
            <div class="divAddOrder" onclick="addnhanhieu()"/>
        </td>
    </tr>
</table>
<asp:GridView ID="grvDanhSach" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvDanhSach_RowDataBound">
    <Columns>
        <asp:TemplateField HeaderText="Mã">
            <ItemTemplate>
                <asp:HyperLink ID="lnkID" runat="server" Text='<%# Eval("ID") %>'/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="TenNhanHieu" HeaderText="Tên nhãn hiệu" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="NuocDangKy" HeaderText="Nước" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="SoChungNhan" HeaderText="CN" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="NgayChungNhan" HeaderText="Ngày CN" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Gốc">
            <ItemTemplate>
                <asp:HyperLink ID="lnkGoc" runat="server"/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Hình ảnh">
            <ItemTemplate>
                <asp:Image ID="imgHinhAnh" runat="server"/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="LinhVuc" HeaderText="Lĩnh vực" HtmlEncode="False">
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
