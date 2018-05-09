<%@ Control Language="C#" AutoEventWireup="true" CodeFile="note_admin.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.note_admin" %>
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
    <td valign="top">
        <asp:Label runat="server" style="vertical-align:top;">Hiển thị các ghi chú</asp:Label>
        <asp:DropDownList ID="ddlRowStatus" runat="server" style="vertical-align:top;">
            <asp:ListItem Value="1">hiện hành</asp:ListItem>
            <asp:ListItem Value="0">đã xóa</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="Label1" runat="server" style="vertical-align:top;">Từ ngày</asp:Label>
        <asp:TextBox ID="txtNgayFrom" runat="server" Width="80" style="vertical-align:top;"/>
        <cc1:calendarextender id="calNgayFrom" runat="server" format="dd/MM/yyyy"
            popupbuttonid="txtNgayFrom" targetcontrolid="txtNgayFrom"></cc1:calendarextender>
        <asp:Label ID="Label2" runat="server" style="vertical-align:top;">đến</asp:Label>
        <asp:TextBox ID="txtNgayTo" runat="server" Width="80" style="vertical-align:top;"/>
        <cc1:calendarextender id="calNgayTo" runat="server" format="dd/MM/yyyy"
            popupbuttonid="txtNgayTo" targetcontrolid="txtNgayTo"></cc1:calendarextender>
        <asp:ListBox ID="ddlChuong" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="5" ToolTip="Chuồng"></asp:ListBox>
        <asp:Button ID="btnLoad" runat="server" Text="Tải" OnClick="btnLoad_Click" CssClass="button" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="lnkAddNote" runat="server" CssClass="button">Thêm ghi chú</asp:HyperLink>
        <asp:HyperLink ID="lnkMultiChuong" runat="server" CssClass="button">Ghi chú nhiều chuồng</asp:HyperLink>
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
        <asp:BoundField DataField="Ngay" HeaderText="Ngày" SortExpression="Ngay" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow" HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenChuong" HeaderText="Chuồng" SortExpression="TenChuong">
            <HeaderStyle ForeColor="Yellow" HorizontalAlign="Center"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Note" HeaderText="Ghi chú" SortExpression="Note">
            <HeaderStyle ForeColor="Yellow" HorizontalAlign="Center"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField>
             <HeaderStyle ForeColor="Yellow" HorizontalAlign="Center"></HeaderStyle>
            <HeaderTemplate>
                <input type="CheckBox" name="SelectAllCheckBox" onclick="SelectAll(this)" />
            </HeaderTemplate>
            <ItemTemplate>
                <input type="CheckBox" id="chkChon" runat="server" class="ChkChon" onclick = "chon_click(event, this);" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HyperLink ID="lnkEdit" runat="server" CssClass="button">Sửa</asp:HyperLink>
                <asp:Button ID="btnDelete" runat="server" Text="Xóa" CssClass="button" OnClick="btnDelete_Click" OnClientClick="return confirmDelete()" />
                <asp:Button ID="btnXemThayDoi" runat="server" OnClick="btnXemThayDoi_Click" ToolTip="Xem thay đổi (F1)"/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsDanhSach" runat="server" EnableCaching="false" SelectMethod="GetNote" EnablePaging="true"
TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
SelectCountMethod="CountNote" SortParameterName="sortBy">
    <SelectParameters>
        <asp:Parameter Name="WhereClause" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsDanhSachDelete" runat="server" EnableCaching="false" SelectMethod="GetNoteDelete" EnablePaging="true"
TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
SelectCountMethod="CountNoteDelete" SortParameterName="sortBy">
    <SelectParameters>
        <asp:Parameter Name="WhereClause" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:GridView ID="grvHidden" runat="server"
    AutoGenerateColumns="False"
    OnRowDataBound="grvHidden_RowDataBound"
    Width="100%" BorderStyle="Solid" HeaderStyle-BackColor="#999999" Font-Names="Arial" HeaderStyle-Font-Names="Arial" RowStyle-Font-Names="Arial">
    <Columns>
        <asp:BoundField DataField="Ngay" HeaderText="Ngày"
            DataFormatString="{0:dd/MM/yyyy}">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenChuong" HeaderText="Chuồng">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
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