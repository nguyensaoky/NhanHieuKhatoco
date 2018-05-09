<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_chinhsuacachet.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_chinhsuacachet" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
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
<table width="100%" cellpadding="5">
    <tr>
        <td valign="top">
            <table class="FileManager_ToolBar">
                <tr>
                    <td>
                        Hiển thị cá chết
                        <asp:DropDownList ID="ddlRowStatus" runat="server">
                            <asp:ListItem Value="1">hiện hành</asp:ListItem>
                            <asp:ListItem Value="0">đã xóa</asp:ListItem>
                        </asp:DropDownList>
                        ID
                        <asp:TextBox runat="server" ID="txtID" Width="60px"></asp:TextBox>
                        <br />
                        Vi cắt
                        <asp:TextBox runat="server" ID="txtMaSo" size="20" Width="40px"></asp:TextBox>
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
                        Xuống chuồng từ
                        <asp:TextBox ID="txtFromDate" runat="server" Width="70" TabIndex="6"/>
                        <cc1:calendarextender id="calFromDate" runat="server" format="dd/MM/yyyy" popupbuttonid="txtFromDate" targetcontrolid="txtFromDate"></cc1:calendarextender>
                        đến ngày
                        <asp:TextBox ID="txtToDate" runat="server" Width="70" TabIndex="7"/>
                        <cc1:calendarextender id="calToDate" runat="server" format="dd/MM/yyyy" popupbuttonid="txtToDate" targetcontrolid="txtToDate"></cc1:calendarextender>
                        <br />
                        Ngày chết từ
                        <asp:TextBox ID="txtNgayChetFrom" runat="server" Width="70" TabIndex="6"/>
                        <cc1:calendarextender id="calNgayChetFrom" runat="server" format="dd/MM/yyyy" popupbuttonid="txtNgayChetFrom" targetcontrolid="txtNgayChetFrom"></cc1:calendarextender>
                        đến ngày
                        <asp:TextBox ID="txtNgayChetTo" runat="server" Width="70" TabIndex="7"/>
                        <cc1:calendarextender id="calNgayChetTo" runat="server" format="dd/MM/yyyy" popupbuttonid="txtNgayChetTo" targetcontrolid="txtNgayChetTo"></cc1:calendarextender>
                        <br />
                        Hình thức chết
                        <asp:DropDownList runat="server" ID="ddlHinhThucChet">
                            <asp:ListItem Value="0" Selected="true">---</asp:ListItem>
                            <asp:ListItem Value="-1">Chết</asp:ListItem>
                            <asp:ListItem Value="-4">Loại thải</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:ListBox ID="lstBienBan" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="6" ToolTip="Biên bản cá chết"></asp:ListBox>
                        <asp:ListBox ID="ddlChuong" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="6" ToolTip="Chuồng"></asp:ListBox>
                        <asp:ListBox ID="ddlCaMe" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="6" ToolTip="Cá mẹ"></asp:ListBox>
                        <asp:ListBox ID="ddlLoaiCa" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="6" ToolTip="Loại cá"></asp:ListBox>
                        <asp:Button ID="btnLoad" runat="server" Text="Xem" OnClick="btnLoad_Click" CssClass="button"/>
                    </td>
                </tr>
            </table>
            <table width="100%" class="FileManager_ToolBar">
                <tr>
                    <td align="left">
                        Tổng số cá đã chết trong danh sách: <asp:Label Font-Bold="true" ID="lblTongSo" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        Hiển thị <asp:DropDownList runat="server" ID="ddlPageSize">
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>40</asp:ListItem>
                        <asp:ListItem Selected="true">50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                        <asp:ListItem>150</asp:ListItem>
                        <asp:ListItem>200</asp:ListItem>
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
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <input type="CheckBox" name="SelectAllCheckBox" onclick="SelectAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="CheckBox" id="chkChon" runat="server" class="ChkChon" onclick = "chon_click(event, this);">
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ID" SortExpression="[IDCaSau] DESC">
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
                    <asp:BoundField DataField="TenLoaiCa" HeaderText="Loại cá" SortExpression="[LoaiCa] DESC">
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
                    <asp:TemplateField HeaderText="Biên bản" SortExpression="[BienBan] DESC">
                        <ItemTemplate>
                            <asp:TextBox ID="txtBienBan" runat="server" Width="40"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kích thước da (cm)" SortExpression="[Da_Bung] DESC">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDa_Bung" Width="25" runat="server" style="text-align:right;"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Loại" SortExpression="[Da_PhanLoai] DESC">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlDa_PhanLoai" runat="server">
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">CXĐ</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Đầu" SortExpression="[Dau] DESC">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDau" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phương pháp mổ" SortExpression="[Note] DESC">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlPPM" runat="server">
                                <asp:ListItem Value="CL">CL</asp:ListItem>
                                <asp:ListItem Value="MDL">MDL</asp:ListItem>
                                <asp:ListItem Value="CB">CB</asp:ListItem>
                                <asp:ListItem Value="CB-KDL">CB-KDL</asp:ListItem>
                                <asp:ListItem Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hình thức<br/>chết" SortExpression="[Status] DESC">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlHinhThucChet" runat="server" Enabled="false">
                                <asp:ListItem Value="-1">Chết</asp:ListItem>
                                <asp:ListItem Value="-4">Loại thải</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Lý do chết" SortExpression="[LyDoChet] DESC">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlLyDoChet" runat="server" Width="100">
                        </asp:DropDownList>
                    </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Khối lượng chết (kg)" SortExpression="[KhoiLuong] DESC">
                        <ItemTemplate>
                            <asp:TextBox ID="txtKhoiLuong" runat="server" Width="30">0</asp:TextBox>
                        </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sản phẩm khác">
                        <ItemTemplate>
                            <asp:PlaceHolder runat="server" ID="dsVatTu"></asp:PlaceHolder>
                        </ItemTemplate><ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnSave" runat="server" ToolTip="Lưu thông số cá chết mới" OnClick="btnSave_Click"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ngày chết" SortExpression="[LatestChange] DESC">
                        <ItemTemplate>
                            <asp:TextBox ID="txtNgayChet" runat="server"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnSaveNgayChet" runat="server" ToolTip="Lưu ngày chết mới" OnClick="btnSaveNgayChet_Click"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnPhucHoi" runat="server" ToolTip="Cho cá sống lại" OnClick="btnPhucHoi_Click"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnXemThayDoi" runat="server" OnClick="btnXemThayDoi_Click" ToolTip="Xem thay đổi (F1)"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsDanhSach" runat="server" EnableCaching="false" SelectMethod="GetCaChet_HienTai" EnablePaging="true"
            TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
            SortParameterName="sortBy" SelectCountMethod="CountCaChet_HienTai">
                <SelectParameters>
                    <asp:Parameter Name="WhereClause" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsDanhSachDelete" runat="server" EnableCaching="false" SelectMethod="GetCaChetDelete" EnablePaging="true"
            TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
            SelectCountMethod="CountCaChetDelete" SortParameterName="sortBy">
                <SelectParameters>
                    <asp:Parameter Name="WhereClause" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </td>
    </tr>
</table>
<asp:GridView ID="grvHidden" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvHidden_RowDataBound" Width="100%" BorderStyle="Solid" HeaderStyle-BackColor="#999999">
    <Columns>
        <asp:BoundField HeaderText="ID" DataField="IDCaSau" >
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="MaSoGoc" HeaderText="Vi cắt">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="MaSo" HeaderText="Mã">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenChuong" HeaderText="Chuồng">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="GioiTinh" HeaderText="Giới tính">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Giong" HeaderText="Giống">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenLoaiCa" HeaderText="Loại cá">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="NgayNo" HeaderText="Ngày nở" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="NgayXuongChuong" HeaderText="Ngày xuống<br/>chuồng" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenNguonGoc" HeaderText="Nguồn gốc">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="BienBan" HeaderText="Biên bản">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Da_Bung" HeaderText="Kích thước<br/>da">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Loại">
            <ItemTemplate>
                <asp:Label ID="lblDa_PhanLoai" runat="server"/>
            </ItemTemplate>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="Dau" HeaderText="Đầu">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Note" HeaderText="PPM">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Hình thức<br/>chết">
            <ItemTemplate>
                <asp:Label ID="lblHinhThucChet" runat="server"/>
            </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="LyDoChet" HeaderText="Lý do chết">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="KhoiLuong" HeaderText="Khối lượng<br/>chết (kg)">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="LatestChange" HeaderText="Ngày chết" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
    </Columns>
</asp:GridView>
<asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
<asp:HiddenField ID="hdListBienDongPage" runat="server" Value="0" EnableViewState="true"/>
<asp:HiddenField ID="hdCanEdit" runat="server" Value="0" EnableViewState="true"/>
<asp:HiddenField ID="hdOldRowID" runat="server" Value=""/>
<asp:HiddenField ID="hdOldColor" runat="server" Value=""/>
<asp:HiddenField ID="hdButtonID" runat="server" Value=""/>
<asp:HiddenField ID="hdOrderBy" runat="server" Value="" EnableViewState="true"/>
<br />