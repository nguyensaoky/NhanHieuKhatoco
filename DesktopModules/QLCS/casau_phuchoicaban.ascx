<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_phuchoicaban.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_phuchoicaban" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
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
</script>
<table width="100%" cellpadding="5">
    <tr>
        <td style="width:79%;" valign="top">
            <asp:UpdatePanel ID="udpDanhSach" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="FileManager_ToolBar">
                    <tr><td>
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
                        Loại cá
                        <asp:DropDownList runat="server" ID="ddlLoaiCa"></asp:DropDownList>
                        <br />
                        Chuồng
                        <asp:DropDownList runat="server" ID="ddlChuong"></asp:DropDownList>
                        Cá mẹ
                        <asp:DropDownList runat="server" ID="ddlCaMe"></asp:DropDownList>
                        <br />
                        Ngày xuống chuồng từ
                        <asp:TextBox ID="txtFromDate" runat="server" Width="100" TabIndex="6"/>
                        <cc1:calendarextender id="calFromDate" runat="server" format="dd/MM/yyyy" popupbuttonid="txtFromDate" targetcontrolid="txtFromDate"></cc1:calendarextender>
                        đến ngày
                        <asp:TextBox ID="txtToDate" runat="server" Width="100" TabIndex="7"/>
                        <cc1:calendarextender id="calToDate" runat="server" format="dd/MM/yyyy" popupbuttonid="txtToDate" targetcontrolid="txtToDate"></cc1:calendarextender>
                        <br />
                        Ngày bán từ
                        <asp:TextBox ID="txtNgayBanFrom" runat="server" Width="100" TabIndex="6"/>
                        <cc1:calendarextender id="calNgayBanFrom" runat="server" format="dd/MM/yyyy" popupbuttonid="txtNgayBanFrom" targetcontrolid="txtNgayBanFrom"></cc1:calendarextender>
                        đến ngày
                        <asp:TextBox ID="txtNgayBanTo" runat="server" Width="100" TabIndex="7"/>
                        <cc1:calendarextender id="calNgayBanTo" runat="server" format="dd/MM/yyyy" popupbuttonid="txtNgayBanTo" targetcontrolid="txtNgayBanTo"></cc1:calendarextender>
                        </td>
                        <td align="right" valign="top">
                            <asp:Button ID="btnLoad" runat="server" Text="         Xem         " OnClick="btnLoad_Click" CssClass="button" style="float:right;"/>
                        </td>
                        </tr>
                    </table>
                    <table width="100%" class="FileManager_ToolBar">
                        <tr>
                            <td align="left">
                                Tổng số cá đã bán trong danh sách: <asp:Label Font-Bold="true" ID="lblTongSo" runat="server"></asp:Label>
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
                    <asp:GridView ID="grvDanhSach" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="grvDanhSach_PageIndexChanging" 
                        OnRowDataBound="grvDanhSach_RowDataBound"
                        PageSize="50" OnSorting="grvDanhSach_Sorting" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="ID" SortExpression="[IDCaSau] DESC">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkIDCaSau" runat="server" Text='<%# Eval("IDCaSau") %>'/>
                                </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>

<HeaderStyle ForeColor="Yellow"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MaSo" HeaderText="Vi cắt" SortExpression="[MaSo] DESC">
<HeaderStyle ForeColor="Yellow"></HeaderStyle>
</asp:BoundField>
                            <asp:BoundField DataField="TenChuong" HeaderText="Chuồng" SortExpression="[Chuong] DESC">
<HeaderStyle ForeColor="Yellow"></HeaderStyle>
</asp:BoundField>
                            <asp:BoundField DataField="CaMe" HeaderText="Mẹ" SortExpression="[CaMe] DESC">
<HeaderStyle ForeColor="Yellow"></HeaderStyle>
</asp:BoundField>
                            <asp:BoundField DataField="GioiTinh" HeaderText="Giới tính" SortExpression="[GioiTinh] DESC">
<HeaderStyle ForeColor="Yellow"></HeaderStyle>
</asp:BoundField>
                            <asp:BoundField DataField="Giong" HeaderText="Giống" SortExpression="[Giong] DESC">
<HeaderStyle ForeColor="Yellow"></HeaderStyle>
</asp:BoundField>
                            <asp:BoundField DataField="TenLoaiCa" HeaderText="Loại" SortExpression="[LoaiCa] DESC">
<HeaderStyle ForeColor="Yellow"></HeaderStyle>
</asp:BoundField>
                            <asp:BoundField DataField="NgayNo" HeaderText="Ngày nở" SortExpression="[NgayNo] DESC" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
                            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NgayXuongChuong" HeaderText="Ngày XC" SortExpression="[NgayXuongChuong] DESC" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
                            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="TenNguonGoc" HeaderText="Nguồn gốc" SortExpression="[NguonGoc] DESC">
<HeaderStyle ForeColor="Yellow"></HeaderStyle>
</asp:BoundField>
                            <asp:BoundField HeaderText="Ngày bán" DataField="LatestChange" SortExpression="[LatestChange] DESC" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
                                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input type="CheckBox" name="SelectAllCheckBox" onclick="SelectAll(this)"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input type="CheckBox" ID="chkChon" runat="server" class="ChkChon">
                                </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsDanhSach" runat="server" EnableCaching="false" SelectMethod="Get_HienTai" EnablePaging="true"
                    TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
                    SortParameterName="sortBy" SelectCountMethod="Count_HienTai">
                        <SelectParameters>
                            <asp:Parameter Name="WhereClause" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td valign="top" style="width:21%;" runat="server" id="tdSub">
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
                    <table width="80%">
                        <tr>
                            <td style="width:100%;" valign="top">
                                <asp:ListBox runat="server" ID="lstChon" Rows="20" Width="100%" SelectionMode="Multiple" Font-Names="Arial" style="font-family:Arial Unicode MS"></asp:ListBox>
                                <br /><br />
                                <asp:Button id="btnPhucHoi" runat="server" Text="Phục hồi cá bán được chọn" OnClick="btnPhucHoi_Click" CssClass="button"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdListBienDongPage" runat="server" Value="0" EnableViewState="true"/>
<br />