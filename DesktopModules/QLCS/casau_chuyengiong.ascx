<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_chuyengiong.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_chuyengiong" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
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
<div id="wrapper" runat="server" class="wrapper_margin">
    <div style="width:200px;float:left;">Phân loại mới</div>
    <asp:DropDownList runat="server" ID="ddlGiong">
        <asp:ListItem Value="0">Cá tăng trọng</asp:ListItem>
        <asp:ListItem Value="1">Cá giống</asp:ListItem>
    </asp:DropDownList>
    <br />
    <div style="width:200px;float:left;">Ngày giờ chuyển</div>
    <asp:TextBox ID="txtThoiDiemChuyen" runat="server"></asp:TextBox>
    <br />
    <asp:GridView ID="grvDanhSach" runat="server" AllowSorting="True"
        AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
        OnRowDataBound="grvDanhSach_RowDataBound" OnSorting="grvDanhSach_Sorting" Width="100%" Visible="false">
        <Columns>
            <asp:TemplateField HeaderText="ID" SortExpression="[IDCaSau] DESC">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkIDCaSau" runat="server" Text='<%# Eval("IDCaSau") %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
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
            <asp:BoundField DataField="NgayNo" HeaderText="Ngày nở" SortExpression="[NgayNo] DESC"
                DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField DataField="NgayXuongChuong" HeaderText="Ngày XC" SortExpression="[NgayXuongChuong] DESC"
                DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField DataField="TenNguonGoc" HeaderText="Nguồn gốc" SortExpression="[NguonGoc] DESC">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="Trạng thái" SortExpression="[Status] DESC">
                <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            </asp:TemplateField>
            <asp:BoundField DataField="ThoiDiemBienDong" HeaderText="Thời điểm chuyển" SortExpression="[ThoiDiemBienDong] DESC"
                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="False">
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField DataField="NoteVal" HeaderText="Giống mới" SortExpression="[NoteVal] DESC">
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
            <asp:TemplateField HeaderText="STT">
                <ItemTemplate>
                    <asp:Label ID="lblSTT" runat="server" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    <div style="width:200px;float:left;">&nbsp;</div>
    <asp:Button ID="btnChuyen" runat="server" Text="Chuyển" OnClick="btnChuyen_Click" CssClass="button" />
    <asp:Button ID="btnChuyenChon" runat="server" Text="Thay đổi các biến động được chọn" OnClick="btnChuyenChon_Click" CssClass="button" Visible="false"/>&nbsp;&nbsp;
    <asp:Button ID="btnDotXuat" runat="server" Text="Chuyển đột xuất" OnClick="btnChuyenDotXuat_Click" CssClass="button" Visible="false"/>
    <asp:HiddenField ID="hdIDBienDong" runat="server" Value="0"/>
    <asp:HiddenField ID="hdIDBienDongGroup" runat="server" Value="0"/>
    <asp:HiddenField ID="hdIDCaSau" runat="server" Value="0"/>
</div>