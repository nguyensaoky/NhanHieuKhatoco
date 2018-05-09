<%@ Control Language="C#" AutoEventWireup="true" CodeFile="biendongcasau_admin.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.biendongcasau_admin"%>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
    function initGiaTriMoi()
    {
        var customarray;
        var strDataValue = '<%=dataString%>';
        if(strDataValue != '')
        {    
            customarray = strDataValue.split(";");
        }
        actb(document.getElementById('<%= txtGiaTriMoi.ClientID %>'), customarray, '<%= txtGiaTriMoi.Width.ToString() %>', '#EEEEEE', '#000000', '#C0C0EF');
    }     

    function KeyPress(event)
    {
        if (event.keyCode == 112)
        {
            var hdButtonID = document.getElementById('<%=hdButtonID.ClientID%>');
            document.getElementById(hdButtonID.value).click();
        }
        if (event.keyCode == 13)
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
    <td valign="top">
        ID
        <asp:TextBox runat="server" ID="txtID" Width="60px"></asp:TextBox>
        Vi cắt
        <asp:TextBox runat="server" ID="txtMaSo" Width="70px"></asp:TextBox>
        Giới tính
        <asp:DropDownList runat="server" ID="ddlGioiTinh">
            <asp:ListItem Value="-2" Selected="true">---</asp:ListItem>
            <asp:ListItem Value="-1">CXĐ</asp:ListItem>
            <asp:ListItem Value="1">Đực</asp:ListItem>
            <asp:ListItem Value="0">Cái</asp:ListItem>
        </asp:DropDownList>
        Giống
        <asp:DropDownList runat="server" ID="ddlGiong">
            <asp:ListItem Value="-1" Selected="true">---</asp:ListItem>
            <asp:ListItem Value="1">Giống</asp:ListItem>
            <asp:ListItem Value="0">Tăng trọng</asp:ListItem>
        </asp:DropDownList>
        <br />
        Ngày xuống chuồng từ
        <asp:TextBox ID="txtFromDate" runat="server" Width="70" TabIndex="6"/>
        <cc1:calendarextender id="calFromDate" runat="server" format="dd/MM/yyyy" popupbuttonid="txtFromDate" targetcontrolid="txtFromDate"></cc1:calendarextender>
        đến ngày
        <asp:TextBox ID="txtToDate" runat="server" Width="70" TabIndex="7"/>
        <cc1:calendarextender id="calToDate" runat="server" format="dd/MM/yyyy" popupbuttonid="txtToDate" targetcontrolid="txtToDate"></cc1:calendarextender>
        <br />
        Ngày chết (bán, GM) từ
        <asp:TextBox ID="txtDieFrom" runat="server" Width="70" TabIndex="6"/>
        <cc1:calendarextender id="calDieFrom" runat="server" format="dd/MM/yyyy" popupbuttonid="txtDieFrom" targetcontrolid="txtDieFrom"></cc1:calendarextender>
        đến ngày
        <asp:TextBox ID="txtDieTo" runat="server" Width="70" TabIndex="7"/>
        <cc1:calendarextender id="calDieTo" runat="server" format="dd/MM/yyyy" popupbuttonid="txtDieTo" targetcontrolid="txtDieTo"></cc1:calendarextender>
        <br />
    </td>
    <td rowspan="2" align="center" valign="top">
        Giá trị mới sau biến động
        <br />
        <asp:TextBox ID="txtGiaTriMoi" runat="server"></asp:TextBox>
        <br />
        Ngày biến động từ
        <br />
        <asp:TextBox ID="txtBienDongFrom" runat="server" Width="70" TabIndex="6"/>
        <cc1:calendarextender id="calBienDongFrom" runat="server" format="dd/MM/yyyy" popupbuttonid="txtBienDongFrom" targetcontrolid="txtBienDongFrom"></cc1:calendarextender>
        <br />
        đến ngày
        <br />
        <asp:TextBox ID="txtBienDongTo" runat="server" Width="70" TabIndex="7"/>
        <cc1:calendarextender id="calBienDongTo" runat="server" format="dd/MM/yyyy" popupbuttonid="txtBienDongTo" targetcontrolid="txtBienDongTo"></cc1:calendarextender>
        <br />
        Loại biến động
        <br />
        <asp:DropDownList ID="ddlLoaiBienDong" runat="server">
        </asp:DropDownList>
        <br />
        <asp:Button ID="btnLoad" runat="server" Text="Tìm kiếm" OnClick="btnLoad_Click" CssClass="button"/>
    </td>
</tr>
<tr>
    <td>
        <asp:ListBox ID="ddlChuong" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Chuồng"></asp:ListBox>
        <asp:ListBox ID="ddlCaMe" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Cá mẹ" Width="250"></asp:ListBox><br />
        <asp:ListBox ID="ddlLoaiCa" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Loại cá"></asp:ListBox>
        <asp:ListBox ID="ddlStatus" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Trạng thái"></asp:ListBox>
    </td>
</tr>
</table>
<table width="100%" class="FileManager_ToolBar">
    <tr>
        <td align="left">
            Tổng số biến động trong danh sách: <asp:Label Font-Bold="true" ID="lblTongSo" runat="server"></asp:Label>
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
            <asp:ListItem Selected="true">200</asp:ListItem>
            <asp:ListItem>500</asp:ListItem>
            <asp:ListItem>1000</asp:ListItem>
            <asp:ListItem>2000</asp:ListItem>
            <asp:ListItem>5000</asp:ListItem>
        </asp:DropDownList>dòng/trang
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
        <asp:TemplateField HeaderText="ID Cá" SortExpression="[IDCaSau] DESC">
            <ItemTemplate>
                <asp:HyperLink ID="lnkIDCaSau" runat="server" Text='<%# Eval("IDCaSau") %>' />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="MaSoGoc" HeaderText="Vi cắt">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="MaSo" HeaderText="Mã" SortExpression="[MaSo] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenChuong" HeaderText="Chuồng" SortExpression="[Chuong] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Cá mẹ" SortExpression="[CaMe] DESC">
            <ItemTemplate>
                <asp:HyperLink ID="lnkCaMe" runat="server" Text='<%# Eval("CaMe") %>' />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="GioiTinh" HeaderText="Giới tính" SortExpression="[GioiTinh] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Giong" HeaderText="Giống" SortExpression="[Giong] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenLoaiCa" HeaderText="Loại" SortExpression="[LoaiCa] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="NgayNo" HeaderText="Ngày nở" SortExpression="[NgayNo] DESC"
            DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="NgayXuongChuong" HeaderText="Ngày xuống chuồng" SortExpression="[NgayXuongChuong] DESC"
            DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenNguonGoc" HeaderText="Nguồn gốc" SortExpression="[NguonGoc] DESC">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Trạng thái" SortExpression="[Status] DESC">
            <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </ItemTemplate>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="TenLoaiBienDong" HeaderText="Loại biến động" SortExpression="[TenLoaiBienDong] DESC" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="ThoiDiemBienDong" HeaderText="Thời điểm biến động" SortExpression="[ThoiDiemBienDong] DESC"
            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Giá trị mới">
            <ItemTemplate>
                <asp:Label ID="lblNote" runat="server"></asp:Label>
            </ItemTemplate>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="TenNguoiThayDoi" HeaderText="Người thực hiện">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField>
            <HeaderTemplate>
                <input type="CheckBox" name="SelectAllCheckBox" onclick="SelectAll(this)" />
            </HeaderTemplate>
            <ItemTemplate>
                <input type="CheckBox" id="chkChon" runat="server" class="ChkChon" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnDeleteBienDong" runat="server" Text="Xóa" OnClick="btnDeleteBienDong_Click" CssClass="button"/>
                        <asp:Button ID="btnEditBienDong" runat="server" Text="Sửa" OnClick="btnEditBienDong_Click" CssClass="button"/>
                        <asp:Button ID="btnXemSPThuHoi" runat="server" Text="Xem SP thu hồi" CssClass="button"/>
                        <asp:Button ID="btnXemThayDoi" runat="server" ToolTip="Xem thay đổi (F1)"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsDanhSach" runat="server" EnableCaching="false" SelectMethod="GetBienDong" EnablePaging="true"
TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
SortParameterName="sortBy" SelectCountMethod="CountBienDong">
    <SelectParameters>
        <asp:Parameter Name="WhereClause" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:HiddenField ID="hdListBienDongPage" runat="server" Value="0" EnableViewState="true"/>
<asp:HiddenField ID="hdOldRowID" runat="server" Value=""/>
<asp:HiddenField ID="hdOldColor" runat="server" Value=""/>
<asp:HiddenField ID="hdButtonID" runat="server" Value=""/>
<br />