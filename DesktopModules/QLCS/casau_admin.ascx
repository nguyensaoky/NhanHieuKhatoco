<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_admin.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_admin"%>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
    function KeyPress(event)
    {
        if (event.keyCode == 13)
        {
            var btn = document.getElementById('<%=btnLoad.ClientID%>');
            btn.click();
        }
    }
    var lastChecked = null;
    function chon_click(event, chk) {
        var chkboxes = jQuery('.ChkChon');
        if(!lastChecked) {
            lastChecked = chk;
            jQuery('#<%=lblNumChecked.ClientID%>').text(countchecked());
            return;
        }
        if (event.shiftKey)
        {
            var start = chkboxes.index(chk);
            var end = chkboxes.index(lastChecked);
            chkboxes.slice(Math.min(start,end), Math.max(start,end)+ 1).attr('checked', lastChecked.checked);
        }
        lastChecked = chk;
        jQuery('#<%=lblNumChecked.ClientID%>').text(countchecked());
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
        jQuery('#<%=lblNumChecked.ClientID%>').text(countchecked());
    }
    
    function countchecked()
    {
        return jQuery(".ChkChon:checked").length;
    }
    
    function openwindow(url, name, width, height)
    {
        var leftVal = (screen.width - width) / 2;
        var topVal = (screen.height - height) / 2;
        window.open(url,'','height=' + height + ',width=' + width +',toolbar=no,status=no,linemenubar=no,scrollbars=yes,resizable=yes,modal=yes,left=' + leftVal + ',top=' + topVal);
    }
    
    function finishEdit()
    {
        jQuery('#<%= btnLoad1.ClientID %>').click();
    }
</script>
<table width="100%" cellpadding="5">
    <tr>
        <td style="width:69%;" valign="top">
            <asp:UpdatePanel ID="udpDanhSach" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
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
                            Ngày nhập chuồng từ
                            <asp:TextBox ID="txtNhapChuongFrom" runat="server" Width="70" TabIndex="6"/>
                            <cc1:calendarextender id="calNhapChuongFrom" runat="server" format="dd/MM/yyyy" popupbuttonid="txtNhapChuongFrom" targetcontrolid="txtNhapChuongFrom"></cc1:calendarextender>
                            đến ngày
                            <asp:TextBox ID="txtNhapChuongTo" runat="server" Width="70" TabIndex="7"/>
                            <cc1:calendarextender id="calNhapChuongTo" runat="server" format="dd/MM/yyyy" popupbuttonid="txtNhapChuongTo" targetcontrolid="txtNhapChuongTo"></cc1:calendarextender>
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
                            <asp:Button ID="btnLoad" runat="server" Text="Xem trước thời điểm" OnClick="btnLoad_Click" CssClass="button"/>
                            <asp:Button ID="btnLoad1" runat="server" OnClick="btnLoad1_Click" Width="0" BorderWidth="0" BackColor="Transparent"/><br />
                            <asp:TextBox ID="txtTruocNgay" runat="server" Width="100"/>
                            <cc1:calendarextender id="calTruocNgay" runat="server" format="dd/MM/yyyy" popupbuttonid="txtTruocNgay" targetcontrolid="txtTruocNgay"></cc1:calendarextender><br /><br />
                            <asp:HyperLink ID="lnkAddCaSau" runat="server" CssClass="button" Text="Thêm cá"></asp:HyperLink><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="ddlChuong" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Chuồng"></asp:ListBox>
                            <asp:ListBox ID="ddlCaMe" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Cá mẹ"></asp:ListBox><br />
                            <asp:ListBox ID="ddlLoaiCa" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Loại cá"></asp:ListBox>
                            <asp:ListBox ID="ddlStatus" runat="server" SelectionMode="Multiple" AutoPostBack="false" Rows="4" ToolTip="Trạng thái"></asp:ListBox>
                        </td>
                    </tr>
                    </table>
                    <table width="100%" class="FileManager_ToolBar">
                        <tr>
                            <td align="left">
                                Tổng số cá trong danh sách: <asp:Label Font-Bold="true" ID="lblTongSo" runat="server"></asp:Label>
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
                    <asp:GridView ID="grvDanhSach" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="grvDanhSach_PageIndexChanging" 
                        OnRowDataBound="grvDanhSach_RowDataBound"
                        PageSize="50" OnSorting="grvDanhSach_Sorting" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="ID" SortExpression="[IDCaSau] DESC">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkIDCaSau" runat="server" Text='<%# Eval("IDCaSau") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MaSoGoc" HeaderText="Vi cắt" SortExpression="[MaSoGoc] DESC">
                                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="MaSo" HeaderText="Mã" SortExpression="[MaSo] DESC">
                                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="TenChuong" HeaderText="Chuồng" SortExpression="[Chuong] DESC">
                                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NgayNhapChuong" SortExpression="[NgayNhapChuong] DESC" HeaderText="Ngày nhập chuồng" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
                                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
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
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input type="CheckBox" name="SelectAllCheckBox" onclick="SelectAll(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input type="CheckBox" id="chkChon" runat="server" class="ChkChon" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="GhiChu" HeaderText="Ghi chú" SortExpression="[GhiChu] DESC">
                                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="STT">
                                <ItemTemplate>
                                    <asp:Label ID="lblSTT" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsDanhSach" runat="server" EnableCaching="false" SelectMethod="Get" EnablePaging="true"
                    TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
                    SortParameterName="sortBy" SelectCountMethod="Count">
                        <SelectParameters>
                            <asp:Parameter Name="WhereClause" />
                            <asp:Parameter Name="Date"/>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td valign="top" style="width:32%;" runat="server" id="tdSub">
            <asp:UpdatePanel ID="udpDanhSachChon" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="float:left;padding-top:3px;">
                        <asp:Button ID="btnChon" runat="server" Text=">" OnClick="btnChon_Click" Width="40px" CssClass="button" />
                        <br />
                        <br />
                        <asp:Button ID="btnBo" runat="server" Text="<" OnClick="btnBo_Click" Width="40px" CssClass="button"/>
                        <br />
                        <br />
                        <asp:Button ID="btnBoToanBo" runat="server" Text="<<" OnClick="btnBoToanBo_Click" Width="40px" CssClass="button"/>
                        <br />
                        <br />
                        <center><asp:Label ID="lblNumChecked" runat="server" Font-Bold="true" ForeColor="Red" Text="0"/></center>
                    </div>
                    <table>
                        <tr>
                            <td style="width:100%;" valign="top">
                                <asp:ListBox runat="server" ID="lstChon" Rows="32" Width="100%" SelectionMode="Multiple" Font-Names="Arial" style="font-family:Arial Unicode MS"></asp:ListBox>
                            </td>
                            <td style="width:30%;" valign="top">
                                <asp:Button id="btnChuyenChuong" runat="server" Text="Đổi chuồng" OnClick="btnChuyenChuong_Click" Width="110px" CssClass="button"></asp:Button><br /><br />
                                <asp:Button id="btnChuyenMaSo" runat="server" Text="Đổi vi cắt" OnClick="btnChuyenMaSo_Click" Width="110px" CssClass="button"></asp:Button><br /><br />
                                <asp:Button id="btnChuyenGioiTinh" runat="server" Text="Đổi giới tính" OnClick="btnChuyenGioiTinh_Click" Width="110px" CssClass="button"></asp:Button><br /><br />
                                <asp:Button id="btnChuyenLoaiCa" runat="server" Text="Đổi loại cá" OnClick="btnChuyenLoaiCa_Click" Width="110px" CssClass="button"></asp:Button><br /><br />
                                <asp:Button id="btnChuyenTrangThai" runat="server" Text="Đổi trạng thái" OnClick="btnChuyenTrangThai_Click" Width="110px" CssClass="button"></asp:Button><br /><br />
                                <asp:Button id="btnChuyenGiong" runat="server" Text="Đổi phân loại" OnClick="btnChuyenGiong_Click" Width="110px" CssClass="button"></asp:Button><br /><br />
                                <br />
                                <hr />
                                <br />
                                <asp:Button id="btnChuyenNguonGoc" runat="server" Text="Đổi nguồn gốc" OnClick="btnChuyenNguonGoc_Click" Width="110px" CssClass="button"></asp:Button><br /><br />
                                <asp:Button id="btnChuyenNgayNo" runat="server" Text="Đổi ngày nở" OnClick="btnChuyenNgayNo_Click" Width="110px" CssClass="button"></asp:Button><br /><br />
                                <asp:Button id="btnChuyenNgayXuongChuong" runat="server" Text="Đổi ngày XC" OnClick="btnChuyenNgayXuongChuong_Click" Width="110px" CssClass="button"></asp:Button><br /><br />
                                <asp:Button id="btnChuyenCaMe" runat="server" Text="Đổi cá mẹ" OnClick="btnChuyenCaMe_Click" Width="110px" CssClass="button"></asp:Button><br /><br />
                                <br />
                                <hr />
                                <br />
                                <asp:Button id="btnXoaCa" runat="server" Text="Xóa cá" OnClick="btnXoaCa_Click" Width="110px" CssClass="button"></asp:Button>                               
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<asp:GridView ID="grvHidden" runat="server"
    AutoGenerateColumns="False"
    OnRowDataBound="grvHidden_RowDataBound"
    Width="100%" BorderStyle="Solid" HeaderStyle-BackColor="#999999">
    <Columns>
        <asp:TemplateField HeaderText="ID">
            <ItemTemplate>
                <asp:HyperLink ID="lnkIDCaSau" runat="server" Text='<%# Eval("IDCaSau") %>' />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="MaSoGoc" HeaderText="Vi cắt">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="MaSo" HeaderText="Mã">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenChuong" HeaderText="Chuồng">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Cá mẹ">
            <ItemTemplate>
                <asp:HyperLink ID="lnkCaMe" runat="server" Text='<%# Eval("CaMe") %>' />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="GioiTinh" HeaderText="Giới tính">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Giong" HeaderText="Giống">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenLoaiCa" HeaderText="Loại">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="NgayNo" HeaderText="Ngày nở"
            DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="NgayXuongChuong" HeaderText="Ngày xuống chuồng"
            DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenNguonGoc" HeaderText="Nguồn gốc">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Trạng thái">
            <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </ItemTemplate>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:Button ID="btnExcel" runat="server" Text="Xuất Excel" OnClick="btnExcel_Click" CssClass="button"/>
<asp:HiddenField ID="hdListBienDongPage" runat="server" Value="0" EnableViewState="true"/>
<asp:HiddenField ID="hdOrderBy" runat="server" Value="" EnableViewState="true"/>
<br />