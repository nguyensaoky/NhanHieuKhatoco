<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_chuyentrangthai.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_chuyentrangthai" %>
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
    <div style="width:200px;float:left;">Trạng thái mới</div>
    <asp:DropDownList ID="ddlTrangThai" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTrangThai_SelectedIndexChanged">
        <asp:ListItem Value="-3" Text="Cá bán"></asp:ListItem>
        <asp:ListItem Value="-1" Text="Cá chết"></asp:ListItem>
        <asp:ListItem Value="-4" Text="Loại thải"></asp:ListItem>
        <asp:ListItem Value="0" Text="Cá bình thường"></asp:ListItem>
        <asp:ListItem Value="1" Text="Cá bệnh"></asp:ListItem>
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
            <asp:BoundField DataField="NoteVal" HeaderText="TT mới" SortExpression="[NoteVal] DESC">
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
    <div id="CaChet" runat="server" visible="false">
        Biên bản cá chết<asp:TextBox ID="txtBienBan" runat="server" Width="100"></asp:TextBox>
        <br />
        Sản phẩm thu hồi:<br />
        <asp:GridView ID="grvSPTH" runat="server"
            AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" Width="100%" OnRowDataBound="grvSPTH_RowDataBound" >
            <Columns>
                <asp:BoundField HeaderText="Cá" DataField="Ca">
                    <HeaderStyle ForeColor="Yellow" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Kích thước da (cm)">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDaBung" runat="server" Width="30"></asp:TextBox>
                    </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phân loại da">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlDaPhanLoai" runat="server">
                            <asp:ListItem Value="1">Loại 1</asp:ListItem>
                            <asp:ListItem Value="2">Loại 2</asp:ListItem>
                            <asp:ListItem Value="3">Loại 3</asp:ListItem>
                            <asp:ListItem Value="4" Selected="True">Loại 4</asp:ListItem>
                            <asp:ListItem Value="5">Loại 5</asp:ListItem>
                            <asp:ListItem Value="6">Loại CXĐ</asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lấy đầu">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkDau" runat="server"/>
                    </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phương pháp mổ">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlPPM" runat="server">
                            <asp:ListItem Value="CL">CL</asp:ListItem>
                            <asp:ListItem Value="MDL">MDL</asp:ListItem>
                            <asp:ListItem Value="CB">CB</asp:ListItem>
                            <asp:ListItem Value="CB-KDL">CB-KDL</asp:ListItem>
                            <asp:ListItem Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lý do chết">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlLyDoChet" runat="server">
                        </asp:DropDownList>
                    </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Khối lượng khi chết (kg)">
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
            </Columns>
            <PagerStyle CssClass="pgr" />
            <AlternatingRowStyle CssClass="alt" />
        </asp:GridView>
    </div>
    <br />
    <div style="width:200px;float:left;">&nbsp;</div>
    <asp:Button ID="btnChuyen" runat="server" Text="Chuyển" OnClick="btnChuyen_Click" CssClass="button"/>
    <asp:Button ID="btnChuyenChon" runat="server" Text="Thay đổi các biến động được chọn" OnClick="btnChuyenChon_Click" CssClass="button" Visible="false"/>&nbsp;&nbsp;
    <asp:HiddenField ID="hdIDBienDong" runat="server" Value="0"/>
    <asp:HiddenField ID="hdIDBienDongGroup" runat="server" Value="0"/>
    <asp:HiddenField ID="hdIDCaSau" runat="server" Value="0"/>
</div>