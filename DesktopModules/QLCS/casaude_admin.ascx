<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casaude_admin.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casaude_admin" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
    function KeyPress(event)
    {
        if (event.keyCode == 112)
        {
            var hdButtonID = document.getElementById('<%=hdButtonID.ClientID%>');
            document.getElementById(hdButtonID.value).click();
        }
        else if (event.keyCode == 13)
        {
            var btn = document.getElementById('<%=btnLoad.ClientID%>');
            btn.click();
        }
    }
    function setSelectedRow(row, buttonid)
    {
        var hdOldRowID = document.getElementById('<%=hdOldRowID.ClientID%>');
        var hdOldColor = document.getElementById('<%=hdOldColor.ClientID%>');
        var hdButtonID = document.getElementById('<%=hdButtonID.ClientID%>');
        
        var or = document.getElementById(hdOldRowID.value);
        if(or != null) or.style.backgroundColor = hdOldColor.value;
        
        hdButtonID.value = buttonid;
        hdOldRowID.value = row.id;
        hdOldColor.value = row.style.backgroundColor;
        
        row.style.backgroundColor = "#ffffaa";
    }
    var lastChecked = null;
    function chon_click(event, chk) {
        var chkboxes = jQuery('.ChkChon');
        if(!lastChecked) {
            lastChecked = chk;
            return;
        }
        if (event.shiftKey)
        {
            var start = chkboxes.index(chk);
            var end = chkboxes.index(lastChecked);
            chkboxes.slice(Math.min(start,end), Math.max(start,end)+ 1).attr('checked', lastChecked.checked);
        }
        lastChecked = chk;
    }
    function SelectAll(CheckBoxControl)
    {
        if (CheckBoxControl.checked == true)
        {
            jQuery('.ChkChon').attr('checked', 'checked');
        }
        else if (CheckBoxControl.checked == false)
        {
            jQuery('.ChkChon').removeAttr('checked');
        }
    }
    
    function openwindow(url, name, width, height)
    {
        var leftVal = (screen.width - width) / 2;
        var topVal = (screen.height - height) / 2;
        window.open(url,'','height=' + height + ',width=' + width +',toolbar=no,status=no,linemenubar=no,scrollbars=yes,resizable=yes,modal=yes,left=' + leftVal + ',top=' + topVal);
    }
    
    function confirmDelete()
    {
        var r=confirm("Bạn có chắc bạn muốn xóa?");
        if (r==true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
</script>
<table class="FileManager_ToolBar">
<tr>
    <td>
        Hiển thị lượt cá đẻ 
        <asp:DropDownList ID="ddlRowStatus" runat="server">
            <asp:ListItem Value="1">hiện hành</asp:ListItem>
            <asp:ListItem Value="0">đã xóa</asp:ListItem>
        </asp:DropDownList>
        Trạng thái
        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="false">
            <asp:ListItem Value="-1">---</asp:ListItem>
            <asp:ListItem Value="0">Chưa/không nở</asp:ListItem>
            <asp:ListItem Value="2">Chưa nở</asp:ListItem>
            <asp:ListItem Value="3">Không nở</asp:ListItem>
            <asp:ListItem Value="1">Đã nở</asp:ListItem>
        </asp:DropDownList>
        Ngày ấp từ
        <asp:TextBox ID="txtNgayApFrom" runat="server" Width="80"/>
        <cc1:calendarextender id="calNgayApFrom" runat="server" format="dd/MM/yyyy"
            popupbuttonid="txtNgayApFrom" targetcontrolid="txtNgayApFrom"></cc1:calendarextender>
        đến
        <asp:TextBox ID="txtNgayApTo" runat="server" Width="80"/>
        <cc1:calendarextender id="calNgayApTo" runat="server" format="dd/MM/yyyy"
            popupbuttonid="txtNgayApTo" targetcontrolid="txtNgayApTo"></cc1:calendarextender>
        <br />
        <asp:ListBox ID="ddlChuong" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Chuồng"></asp:ListBox>
        <asp:ListBox ID="ddlCaMe" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Cá mẹ" Width="250"></asp:ListBox>
        <asp:ListBox ID="ddlLoaiCa" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Loại cá"></asp:ListBox>
        <asp:ListBox ID="ddlPhongAp" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Phòng ấp"></asp:ListBox>
        <asp:ListBox ID="ddlNhanVien" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Người thu thứng"></asp:ListBox>
        <asp:Button ID="btnLoad" runat="server" Text="Tải" OnClick="btnLoad_Click" CssClass="button" />
        <asp:HyperLink ID="lnkAddCaSauDe" runat="server" CssClass="button" style="float:right;">Thêm cá đẻ</asp:HyperLink>
    </td>
</tr>
</table>
<table width="100%" class="FileManager_ToolBar">
    <tr>
        <td align="left">
            Tổng số trong danh sách: <asp:Label Font-Bold="true" ID="lblTongSo" runat="server"></asp:Label>
        </td>
        <td align="right">
            Hiển thị <asp:DropDownList runat="server" ID="ddlPageSize">
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem Selected="true">50</asp:ListItem>
            <asp:ListItem>100</asp:ListItem>
            <asp:ListItem>150</asp:ListItem>
            <asp:ListItem>200</asp:ListItem>
            <asp:ListItem>500</asp:ListItem>
            <asp:ListItem>1000</asp:ListItem>
            <asp:ListItem>2000</asp:ListItem>
            <asp:ListItem>5000</asp:ListItem>
        </asp:DropDownList>mục/trang
        </td>
    </tr>
</table>
<asp:Button ID="btnKhoa" runat="server" Text="Khóa" OnClick="btnKhoa_Click" CssClass="button"/>
<asp:Button ID="btnMoKhoa" runat="server" Text="Mở khóa" OnClick="btnMoKhoa_Click" CssClass="button"/><br />
<asp:GridView ID="grvDanhSach" runat="server" AllowPaging="True" AllowSorting="True"
    AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="grvDanhSach_PageIndexChanging" 
    OnRowDataBound="grvDanhSach_RowDataBound"
    PageSize="50" OnSorting="grvDanhSach_Sorting" Width="100%">
    <Columns>
        <asp:TemplateField HeaderText="Cá mẹ" SortExpression="[CaMe1] DESC">
            <ItemTemplate>
                <asp:Label ID="lblCaMe" runat="server"/>
            </ItemTemplate>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Người thu trứng" SortExpression="[NhanVien] DESC">
            <ItemTemplate>
                <asp:Label ID="lblNhanVien" runat="server"/>
            </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="NgayVaoAp" HeaderText="Ngày Ấp" SortExpression="[NgayVaoAp] DESC" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"/>
        </asp:BoundField>
        <asp:BoundField DataField="TenKhayAp" HeaderText="Khay Ấp" SortExpression="[KhayAp] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenPhongAp" HeaderText="Phòng Ấp" SortExpression="[PhongAp] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TrongLuongTrungBQ" HeaderText="Khối lượng trứng (g)" SortExpression="[TrongLuongTrungBQ] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungDe" HeaderText="Đẻ (trứng)" SortExpression="[TrungDe] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungVo" HeaderText="Vỡ (trứng)" SortExpression="[TrungVo] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungThaiLoai" HeaderText="Thải loại (trứng)" SortExpression="[TrungThaiLoai] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungKhongPhoi" HeaderText="Không phôi (trứng)" SortExpression="[TrungKhongPhoi] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungChetPhoi1" HeaderText="Chết phôi 1 (trứng)" SortExpression="[TrungChetPhoi1] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungChetPhoi2" HeaderText="Chết phôi 2 (trứng)" SortExpression="[TrungChetPhoi2] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="NgayNo" HeaderText="Ngày Nở" SortExpression="[NgayNo] DESC" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"/>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Số lượng nở" SortExpression="[TrungNo] DESC">
            <ItemTemplate>
                <asp:Label ID="lblSLNo" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Số ngày ấp"  SortExpression="[SoNgayAp] DESC">
            <ItemTemplate>
                <asp:Label ID="lblSoNgayAp" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="TenKhayUm" HeaderText="Khay Úm" SortExpression="[KhayUm] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TrongLuongConBQ" HeaderText="Khối lượng con (g)" SortExpression="[TrongLuongConBQ] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Dài (cm)" SortExpression="[ChieuDaiBQ] DESC">
            <ItemTemplate>
                <asp:Label ID="lblChieuDaiBQ" runat="server" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Bụng (cm)" SortExpression="[VongBungBQ] DESC">
            <ItemTemplate>
                <asp:Label ID="lblVongBungBQ" runat="server" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Trạng thái" SortExpression="[NewStatus] DESC">
            <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server" /><br />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="Note" HeaderText="Ghi chú" SortExpression="[Note] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField>
            <HeaderTemplate>
                <input type="CheckBox" name="SelectAllCheckBox" onclick="SelectAll(this)" />
            </HeaderTemplate>
            <ItemTemplate>
                <input type="CheckBox" id="chkChon" runat="server" class="ChkChon" onclick = "chon_click(event, this);"/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HyperLink ID="lnkCaMe" runat="server" Text="Xem chi tiết" CssClass="button" /><br />
                <asp:Button ID="btnDelete" runat="server" Text="Xóa" CssClass="button" OnClick="btnDelete_Click" OnClientClick="return confirmDelete()" />
                <asp:Button ID="btnXemThayDoi" runat="server" OnClick="btnXemThayDoi_Click" ToolTip="Xem thay đổi (F1)"/><br />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsDanhSach" runat="server" EnableCaching="false" SelectMethod="GetCaSauDe" EnablePaging="true"
TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
SelectCountMethod="CountCaSauDe" SortParameterName="sortBy">
    <SelectParameters>
        <asp:Parameter Name="WhereClause" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsDanhSachDelete" runat="server" EnableCaching="false" SelectMethod="GetCaSauDeDelete" EnablePaging="true"
TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
SelectCountMethod="CountCaSauDeDelete" SortParameterName="sortBy">
    <SelectParameters>
        <asp:Parameter Name="WhereClause" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:GridView ID="grvHidden" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvHidden_RowDataBound" Width="100%" BorderStyle="Solid" HeaderStyle-BackColor="#999999" Font-Names="Arial" HeaderStyle-Font-Names="Arial" RowStyle-Font-Names="Arial">
    <Columns>
        <asp:BoundField DataField="CaMe" HeaderText="Cá mẹ">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Người thu trứng">
            <ItemTemplate>
                <asp:Label ID="lblNhanVien" runat="server"/>
            </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="NgayVaoAp" HeaderText="Ngày Ấp" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"/>
        </asp:BoundField>
        <asp:BoundField DataField="TenKhayAp" HeaderText="Khay Ấp">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenPhongAp" HeaderText="Phòng Ấp">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TrongLuongTrungBQ" HeaderText="Khối lượng trứng (g)">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungDe" HeaderText="Đẻ (trứng)">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungVo" HeaderText="Vỡ (trứng)">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungThaiLoai" HeaderText="Thải loại (trứng)">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungKhongPhoi" HeaderText="Không phôi (trứng)">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungChetPhoi1" HeaderText="Chết phôi 1 (trứng)">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="TrungChetPhoi2" HeaderText="Chết phôi 2 (trứng)">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:BoundField DataField="NgayNo" HeaderText="Ngày Nở" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"/>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Số lượng nở">
            <ItemTemplate>
                <asp:Label ID="lblSLNo" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Số ngày ấp">
            <ItemTemplate>
                <asp:Label ID="lblSoNgayAp" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="TenKhayUm" HeaderText="Khay Úm">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TrongLuongConBQ" HeaderText="Khối lượng con (g)">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Dài (cm)">
            <ItemTemplate>
                <asp:Label ID="lblChieuDaiBQ" runat="server" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Bụng (cm)">
            <ItemTemplate>
                <asp:Label ID="lblVongBungBQ" runat="server" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Trạng thái">
            <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server" /><br />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="Note" HeaderText="Ghi chú">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
    </Columns>
</asp:GridView>
<asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
<asp:HiddenField ID="hdOrderBy" runat="server" Value="" EnableViewState="true"/>
<asp:HiddenField ID="hdOldRowID" runat="server" Value=""/>
<asp:HiddenField ID="hdOldColor" runat="server" Value=""/>
<asp:HiddenField ID="hdButtonID" runat="server" Value=""/>