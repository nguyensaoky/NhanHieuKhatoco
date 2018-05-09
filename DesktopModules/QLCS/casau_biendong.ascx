<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_biendong.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_biendong" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
    function KeyPress(event)
    {
        if (event.keyCode == 112)
        {
            var hdButtonID = document.getElementById('<%=hdButtonID.ClientID%>');
            document.getElementById(hdButtonID.value).click();
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
    
    function finishEdit()
    {
        jQuery('#<%= btnLoad.ClientID %>').click();
    }
    
    var showMore = false;
    function ToggleMore(link)
    {
        var tblContent= document.getElementById("tblContent");
        if(!showMore)
        {
            tblContent.style.display = 'block';
            link.innerHTML = 'Bấm vào đây để ẩn ghi chú cho cá';
            showMore = true;
        }
        else
        {
            tblContent.style.display = 'none';
            link.innerHTML = 'Bấm vào đây để nhập ghi chú cho cá';
            showMore = false;
        }
    }
</script>
<b>THÔNG TIN HIỆN TẠI</b><br />
<asp:GridView ID="grvChiTiet" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvChiTiet_RowDataBound" Width="100%">
    <Columns>
        <asp:BoundField DataField="IDCaSau" HeaderText="ID">
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
        <asp:TemplateField HeaderText="Cá mẹ" SortExpression="[CaMe] DESC">
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
        <asp:BoundField DataField="NgayNo" HeaderText="Ngày nở" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="NgayXuongChuong" HeaderText="Ngày XC" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenNguonGoc" HeaderText="Nguồn gốc">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Trạng thái">
            <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<a id="toggle" onclick="ToggleMore(this)" style="cursor:pointer;font-weight:bold;text-decoration:none;">Bấm vào đây để nhập ghi chú cho cá</a>
<div id="tblContent" style="display:none;width:100%;">
    <asp:TextBox ID="txtGhiChu" runat="server" TextMode="MultiLine" Rows="3" Width="400"></asp:TextBox>
    <span style="vertical-align:top;"><asp:Button ID="btnSaveGhiChu" runat="server" Text="Lưu ghi chú" CssClass="button" OnClick="btnSaveGhiChu_Click"/></span>
</div>
<br />
<br />
<b>BIẾN ĐỘNG</b><br/>
<asp:Button ID="btnKhoa" runat="server" Text="Khóa" OnClick="btnKhoa_Click" CssClass="button"/>
<asp:Button ID="btnMoKhoa" runat="server" Text="Mở khóa" OnClick="btnMoKhoa_Click" CssClass="button"/><br />
<asp:GridView ID="grvDanhSach" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvDanhSach_RowDataBound" Width="100%">
    <Columns>
        <asp:BoundField DataField="ThoiDiemBienDong" HeaderText="Thời điểm biến động" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenLoaiBienDong" HeaderText="Biến động">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Giá trị cũ">
            <ItemTemplate>
                <asp:Label ID="lblOldValue" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Giá trị mới">
            <ItemTemplate>
                <asp:Label ID="lblNote" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="TenNguoiThayDoi" HeaderText="Người thực hiện">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField>
            <HeaderTemplate>
                <input type="CheckBox" name="SelectAllCheckBox" onclick="SelectAll(this)"/>
            </HeaderTemplate>
            <ItemTemplate>
                <input type="CheckBox" ID="chkChon" runat="server" class="ChkChon" onclick = "chon_click(event, this);" >
            </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btnDeleteBienDong" runat="server" Text="Xóa" OnClick="btnDeleteBienDong_Click" CssClass="button"/>
                <asp:Button ID="btnEditBienDong" runat="server" Text="Sửa" OnClick="btnEditBienDong_Click" CssClass="button"/>
                <asp:Button ID="btnXemSPThuHoi" runat="server" Text="Xem SP thu hồi" OnClick="btnXemSPThuHoi_Click" CssClass="button"/>
                <asp:Button ID="btnXemThayDoi" runat="server" OnClick="btnXemThayDoi_Click" ToolTip="Xem thay đổi (F1)"/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<br />
<b>
    <asp:Label ID="lblBienDongXoa" runat="server"></asp:Label>
</b>
<br/>
<asp:GridView ID="grvDanhSachXoa" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvDanhSachXoa_RowDataBound" Width="100%">
    <Columns>
        <asp:BoundField DataField="ThoiDiemBienDong" HeaderText="Thời điểm biến động" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TenLoaiBienDong" HeaderText="Biến động">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Giá trị cũ">
            <ItemTemplate>
                <asp:Label ID="lblOldValue" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Giá trị mới">
            <ItemTemplate>
                <asp:Label ID="lblNote" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="TenNguoiThayDoi" HeaderText="Người thực hiện">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <asp:Button ID="btnXemSPThuHoiXoa" runat="server" Text="Xem SP thu hồi" OnClick="btnXemSPThuHoiXoa_Click" CssClass="button"/>
                <asp:Button ID="btnXemThayDoiXoa" runat="server" OnClick="btnXemThayDoiXoa_Click" ToolTip="Xem thay đổi (F1)"/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<br />
<b>
    <asp:Label ID="lblSinhSan" runat="server"></asp:Label>
</b>
<br/>
<asp:GridView ID="grvSinhSan" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" Width="100%" OnRowDataBound="grvSinhSan_RowDataBound">
    <Columns>
        <asp:BoundField DataField="NgayVaoAp" HeaderText="Ngày ấp" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TrungDe" HeaderText="Đẻ">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="TrungVo" HeaderText="Vỡ">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="TrungThaiLoai" HeaderText="T.loại">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="TrungKhongPhoi" HeaderText="K.phôi">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="TrungChetPhoi1" HeaderText="C.phôi1">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="TrungChetPhoi2" HeaderText="C.phôi2">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="No" HeaderText="Nở">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="NgayNo" HeaderText="Ngày nở" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="TL con BQ (g)">
            <ItemTemplate>
                <asp:Label ID="lblTrongLuongConBQ" runat="server"></asp:Label>
            </ItemTemplate>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="C.dài BQ (mm)">
            <ItemTemplate>
                <asp:Label ID="lblChieuDaiBQ" runat="server"></asp:Label>
            </ItemTemplate>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="V.bụng BQ (mm)">
            <ItemTemplate>
                <asp:Label ID="lblVongBungBQ" runat="server"></asp:Label>
            </ItemTemplate>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:HiddenField ID="hdListBienDongPage" runat="server" Value="0" EnableViewState="true"/>
<asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Xuất Excel" CssClass="button" Visible="false"/>
<asp:Button ID="btnLoad" runat="server" Width="0" BackColor="Transparent" OnClick="btnLoad_Click" style="border:none;"/>
<asp:HiddenField ID="hdOldRowID" runat="server" Value=""/>
<asp:HiddenField ID="hdOldColor" runat="server" Value=""/>
<asp:HiddenField ID="hdButtonID" runat="server" Value=""/>