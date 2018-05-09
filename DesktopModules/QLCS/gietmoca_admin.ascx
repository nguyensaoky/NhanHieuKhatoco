<%@ Control Language="C#" AutoEventWireup="true" CodeFile="gietmoca_admin.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.gietmoca_admin" %>
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
</script>
<table class="FileManager_ToolBar">
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Ngày giết mổ từ" style="vertical-align:top;"></asp:Label>
            <asp:TextBox ID="txtNgayGietMoFrom" runat="server" Width="70" style="vertical-align:top;"/>
            <cc1:CalendarExtender ID="calNgayGietMoFrom" runat="server" Format="dd/MM/yyyy" PopupButtonID="txtNgayGietMoFrom"
                TargetControlID="txtNgayGietMoFrom">
            </cc1:CalendarExtender>
            <asp:Label ID="Label2" runat="server" Text="đến" style="vertical-align:top;"></asp:Label>
            <asp:TextBox ID="txtNgayGietMoTo" runat="server" Width="70" style="vertical-align:top;"/>
            <cc1:CalendarExtender ID="calNgayGietMoTo" runat="server" Format="dd/MM/yyyy" PopupButtonID="txtNgayGietMoTo"
                TargetControlID="txtNgayGietMoTo">
            </cc1:CalendarExtender>
            <asp:Label ID="Label3" runat="server" Text="Số biên bản" style="vertical-align:top;"></asp:Label>
            <asp:TextBox ID="txtBienBan" runat="server" Width="50" style="vertical-align:top;"/>
            <asp:ListBox ID="ddlLoaiCa" runat="server" SelectionMode="Multiple" AutoPostBack="false"
                Rows="4" ToolTip="Loại cá"></asp:ListBox>
            <asp:Button ID="btnLoad" runat="server" Text="Tải" OnClick="btnLoad_Click" CssClass="button" />
            <asp:HyperLink ID="lnkAddGietMoCa" runat="server" CssClass="button" style="float:right;">Thêm mới giết mổ cá</asp:HyperLink>
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
<span style="float:left;">
    <asp:Button ID="btnKhoa" runat="server" Text="Khóa" OnClick="btnKhoa_Click" CssClass="button"/>
    <asp:Button ID="btnMoKhoa" runat="server" Text="Mở khóa" OnClick="btnMoKhoa_Click" CssClass="button"/>
</span>
<br />
<asp:GridView ID="grvDanhSach" runat="server" AllowPaging="True" AllowSorting="True"
    AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="grvDanhSach_PageIndexChanging" 
    OnRowDataBound="grvDanhSach_RowDataBound"
    PageSize="50" OnSorting="grvDanhSach_Sorting" Width="100%">
    <Columns>
        <asp:BoundField DataField="NgayMo" HeaderText="Ngày Mổ" SortExpression="[NgayMo]" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="BienBan" HeaderText="Biên bản" SortExpression="[BienBan]">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Trọng Lượng Hơi (kg)" SortExpression="[TrongLuongHoi]">
            <ItemTemplate>
                <asp:Label ID="lblTrongLuongHoi" runat="server" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Trọng Lượng Móc Hàm (kg)" SortExpression="[TrongLuongMocHam]">
            <ItemTemplate>
                <asp:Label ID="lblTrongLuongMocHam" runat="server" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Loại cá" SortExpression="[LoaiCa]">
            <ItemTemplate>
                <asp:Label ID="lblLoaiCa" runat="server" />
            </ItemTemplate>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
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
                <asp:HyperLink ID="lnkEdit" runat="server" CssClass="button">Xem chi tiết</asp:HyperLink><br />
                <asp:Button ID="btnXemThayDoi" runat="server" OnClick="btnXemThayDoi_Click" ToolTip="Xem thay đổi (F1)"/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btnBBMoKham" runat="server" CssClass="button" Text="Xuất biên bản mổ khám"
                    OnClick="btnBBMoKham_Click"></asp:Button>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btnBBSPThuHoi" runat="server" CssClass="button" Text="Xuất biên bản sản phẩm thu hồi"
                    OnClick="btnBBSPThuHoi_Click"></asp:Button>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsDanhSach" runat="server" EnableCaching="false" SelectMethod="GetGietMoCa" EnablePaging="true"
TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
SelectCountMethod="CountGietMoCa" SortParameterName="sortBy">
    <SelectParameters>
        <asp:Parameter Name="WhereClause" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:Button ID="btnExcel" runat="server" Text="Xuất Excel trang này" OnClick="btnExcel_Click" CssClass="button" Visible="false"/>
<asp:HiddenField ID="hdOldRowID" runat="server" Value=""/>
<asp:HiddenField ID="hdOldColor" runat="server" Value=""/>
<asp:HiddenField ID="hdButtonID" runat="server" Value=""/>