<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_biendonggroup.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_biendonggroup" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
    function openwindow(url, name, width, height)
    {
        var leftVal = (screen.width - width) / 2;
        var topVal = (screen.height - height) / 2;
        window.open(url,'','height=' + height + ',width=' + width +',toolbar=no,status=no,linemenubar=no,scrollbars=yes,resizable=yes,modal=yes,left=' + leftVal + ',top=' + topVal);
    }
    
    function finishEdit()
    {
        jQuery('#<%= btnLoad.ClientID %>').click();
    }
</script>
<table class="FileManager_ToolBar">
    <tr>
        <td>
            Biến động từ ngày
            <asp:TextBox ID="txtFromDate" runat="server" Width="100" TabIndex="6"/>
            đến ngày
            <asp:TextBox ID="txtToDate" runat="server" Width="100" TabIndex="7"/>
            Loại biến động
            <asp:DropDownList runat="server" ID="ddlLoaiBienDong">
                <asp:ListItem Text="-----" Value="0"></asp:ListItem>
                <asp:ListItem Text="Đổi chuồng" Value="1"></asp:ListItem>
                <asp:ListItem Text="Đổi giới tính" Value="2"></asp:ListItem>
                <asp:ListItem Text="Đổi loại cá" Value="4"></asp:ListItem>
                <asp:ListItem Text="Đổi vi cắt" Value="5"></asp:ListItem>
                <asp:ListItem Text="Đổi trạng thái" Value="6"></asp:ListItem>
                <asp:ListItem Text="Đổi phân loại" Value="7"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnLoad" runat="server" OnClick="btnLoad_Click" CssClass="button" Text="Tìm"/>
        </td>
    </tr>
</table>
<table width="100%" class="FileManager_ToolBar">
    <tr>
        <td align="left">
            Tổng số biến động trong danh sách: <asp:Label Font-Bold="true" ID="lblTongSo" runat="server"></asp:Label>
            <asp:Label Font-Bold="true" ForeColor="Red" ID="lblMessage" runat="server"></asp:Label>
        </td>
        <td align="right">
            Hiển thị <asp:DropDownList runat="server" ID="ddlPageSize">
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
            <asp:ListItem>40</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>100</asp:ListItem>
            <asp:ListItem>150</asp:ListItem>
            <asp:ListItem>200</asp:ListItem>
            <asp:ListItem Selected="true">500</asp:ListItem>
            <asp:ListItem>1000</asp:ListItem>
            <asp:ListItem>2000</asp:ListItem>
            <asp:ListItem>5000</asp:ListItem>
        </asp:DropDownList>mục/trang
        </td>
    </tr>
</table>
<asp:GridView ID="grvDanhSach" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" 
    AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvDanhSach_RowDataBound" Width="100%"
    AllowPaging="True" AllowSorting="True" OnPageIndexChanging="grvDanhSach_PageIndexChanging"
    PageSize="50" OnSorting="grvDanhSach_Sorting">
    <Columns>
        <asp:BoundField DataField="ThoiDiemBienDong" HeaderText="Thời điểm biến động" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="False" SortExpression="[ThoiDiemBienDong] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenLoaiBienDong" HeaderText="Biến động" SortExpression="[TenLoaiBienDong] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Giá trị mới">
            <ItemTemplate>
                <asp:Label ID="lblNote" runat="server"></asp:Label>
            </ItemTemplate>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Số cá biến động<br/>lý thuyết">
            <ItemTemplate>
                <asp:Label ID="lblSoCa" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Số cá biến động<br/>thực tế">
            <ItemTemplate>
                <asp:Label ID="lblSoCaThucTe" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Số biến động cá thực tế<br/>chưa bị thay đổi">
            <ItemTemplate>
                <asp:Label ID="lblSoCaThucTe_Origin" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="TenNguoiThayDoi" HeaderText="Người thực hiện">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Xóa">
            <ItemTemplate>
                <asp:Button ID="btnDeleteBienDong" runat="server" Text="Xóa" OnClick="btnDeleteBienDong_Click" CssClass="button"/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Sửa">
            <ItemTemplate>
                <asp:Button ID="btnEditBienDong" runat="server" Text="Sửa" OnClick="btnEditBienDong_Click" CssClass="button"/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsDanhSach" runat="server" EnableCaching="false" SelectMethod="GetBienDongGroup" EnablePaging="true"
    TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
    SortParameterName="sortBy" SelectCountMethod="CountBienDongGroup">
    <SelectParameters>
        <asp:Parameter Name="WhereClause" />
    </SelectParameters>
</asp:ObjectDataSource>
