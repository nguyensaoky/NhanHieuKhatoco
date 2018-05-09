<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casauan_edit_thucan.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casauan_edit_thucan" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
    var space = '.';
    var split = ',';
    var ChuongScale = 1;
    var culture = '<%=System.Threading.Thread.CurrentThread.CurrentCulture.ToString()%>';
    if(culture == 'vi-VN')
    {   space = ',';
        split = '.';
    }
    
    function Calc(txt)
    {
        var KL = parseFloat(txt.value.replace(',','.'));
        var vitriphancach = txt.value.indexOf(space);
        if (vitriphancach >= 0) ChuongScale = txt.value.length - vitriphancach - 1;
        var SoLuong = $('#' + txt.id.replace("txtKhoiLuong", "lblSoLuong")).text();
        var SLTT = $('#' + txt.id.replace("txtKhoiLuong", "lblSLTT")).text();
        
        var rKLTT = KL * SLTT / SoLuong;
        if(rKLTT == 0) $('#' + txt.id.replace("txtKhoiLuong", "lblKLTT")).text('0');
        else{ 
            var KLTT = format_number(rKLTT, ChuongScale);
            if(parseFloat(KLTT.replace(',','.')) == 0) KLTT = format_number(rKLTT, ChuongScale + 1);
            $('#' + txt.id.replace("txtKhoiLuong", "lblKLTT")).text(KLTT);
        }
        
        var rKLG = KL - rKLTT;
        if(rKLG == 0) $('#' + txt.id.replace("txtKhoiLuong", "lblKLG")).text('0');
        else{
            var KLG = format_number(rKLG, ChuongScale);
            if(parseFloat(KLG.replace(',','.')) == 0) KLG = format_number(rKLG, ChuongScale + 1);
            $('#' + txt.id.replace("txtKhoiLuong", "lblKLG")).text(KLG);
        }
        
        var rTrungBinh = KL / SoLuong;
        var TrungBinh = format_number(rTrungBinh, ChuongScale + 1);
        if(parseFloat(TrungBinh.replace(',','.')) == 0) TrungBinh = format_number(rTrungBinh, ChuongScale + 2);
        $('#' + txt.id.replace("txtKhoiLuong", "lblTrungBinh")).text(TrungBinh);
    }
    
    function CalcTong()
    {
        var TongKhoiLuong = 0;
        var TongKLTT = 0;
        var TongKLG = 0;
        $(".TextKhoiLuong").each(function() {
            if(!isNaN(this.value.replace(',','.'))) {
                TongKhoiLuong += parseFloat(this.value.replace(',','.'));
            }
        });
        $(".lblKLG").each(function() {
            if(!isNaN(this.innerHTML.replace(',','.'))) {
                TongKLG += parseFloat(this.innerHTML.replace(',','.'));
            }
        });
        TongKLTT = TongKhoiLuong - TongKLG;

        $(".txtTongKhoiLuong").each(function() {
            $(this).val(format_thousand(format_number(TongKhoiLuong,ChuongScale)));
        });
        $(".lblTongKLTT").each(function() {
            $(this).text(format_thousand(format_number(TongKLTT,ChuongScale)));
        });
        $(".lblTongKLG").each(function() {
            $(this).text(format_thousand(format_number(TongKLG,ChuongScale)));
        });
    }
    
    function updateKhoiLuongPhanBo(txt)
    {
        var txtKhoiLuongPhanBo = document.getElementById('<%=txtKhoiLuongPhanBo.ClientID%>');
        txtKhoiLuongPhanBo.value = txt.value;
    }
    
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
            jQuery('.SChkChon').attr('checked', 'checked');
        }
        else if (CheckBoxControl.checked == false)
        {
            jQuery('.ChkChon').removeAttr('checked');
            jQuery('.SChkChon').removeAttr('checked');
        }
    }
    
    function format_number_val(dec,fix)
    {
	fixValue = parseFloat(Math.pow(10,fix));
	retValue = parseInt(Math.round(dec * fixValue)) / fixValue;
	return retValue;
    }
    
    //da format so 0 va dau thap phan nhung chua format nhom 3
    function format_number(dec,fix)
    {
	fixValue = parseFloat(Math.pow(10,fix));
	retValue = parseInt(Math.round(dec * fixValue)) / fixValue;
	retValueS = retValue.toString();
	var Scale = 0;
	var vitriphancach = retValueS.indexOf('.');
	if (vitriphancach >= 0)
		Scale = retValueS.length - vitriphancach - 1;
    
	for( i = Scale; i < fix; i++)
	{
		if(i == 0)
			retValueS = retValueS + space + "0";
		else 
			retValueS = retValueS + "0";
	}
	return retValueS.replace('.',space);
    }
    
    //format nhom 3
    function format_thousand(str)
    {
	    var phancach = str.indexOf(space);
	    var left = str.substring(0,phancach);
	    if(left.length > 3) return left.substring(0,left.length - 3) + split + left.substring(left.length - 3) + str.substring(phancach);
	    else return str;
    }
    
    function setVal(lnk, value)
    {
        $('#' + lnk.replace("lnkDelKhoiLuong", "txtKhoiLuong")).val(value);
    }
    
    function phanbothucan()
    {
        var fix = $('#txtThapPhan').val();
        var Tong = parseFloat(jQuery('#<%=txtKhoiLuongPhanBo.ClientID%>').val().replace(',','.'));
        var TongDoiChieu = 0;
        var TongCa = 0;
        var FirstTextBox;
        var FoundFirstTextBox = 0;
        var countZero = 0;
        $(".LabelSoLuong").each(function() {
            if(!isNaN(this.innerHTML) && this.innerHTML.length!=0) {
                if($('#' + $(this).attr('id').replace("lblSoLuong", "chkChon")).attr('checked'))
                    TongCa += parseInt(this.innerHTML.replace(',','').replace('.',''));
            } 
        });
        if(TongCa > 0)
        {
            $(".LabelSoLuong").each(function() {
                if($('#' + $(this).attr('id').replace("lblSoLuong", "chkChon")).attr('checked'))
                {
                    var value = format_number(Tong*this.innerHTML/TongCa, fix);
                    $('#' + $(this).attr('id').replace("lblSoLuong", "txtKhoiLuong")).val(value);
                    TongDoiChieu = TongDoiChieu + parseFloat(value.replace(',','.'));
                    if(FoundFirstTextBox == 0)
                    {
                        FirstTextBox = $(this).attr('id').replace("lblSoLuong", "txtKhoiLuong");
                        FoundFirstTextBox = 1;
                    }
                }
            });
            var oldValue = parseFloat($('#' + FirstTextBox).val().replace(',','.'));
            $('#' + FirstTextBox).val(format_number(oldValue + Tong - TongDoiChieu,fix));
            
            $(".LabelSoLuong").each(function() {
                if($('#' + $(this).attr('id').replace("lblSoLuong", "chkChon")).attr('checked'))
                {
                    var txt = document.getElementById($(this).attr('id').replace("lblSoLuong", "txtKhoiLuong"));
		    if(parseFloat(txt.value.replace(',','.'))<=0) countZero++;
                    //$('#' + $(this).attr('id').replace("lblSoLuong", "txtKhoiLuong")).val($('#' + $(this).attr('id').replace("lblSoLuong", "txtKhoiLuong")).val().replace('.',space));
                    Calc(txt);
                }
            });
            CalcTong();
        }
        jQuery('.ChkChon').removeAttr('checked');
        jQuery('.SChkChon').removeAttr('checked');
        if(countZero>0) alert("Có chuồng thức ăn không hợp lệ");
    }
    
    function openwindow(url, name, width, height)
    {
        var leftVal = (screen.width - width) / 2;
        var topVal = (screen.height - height) / 2;
        window.open(url,'','height=' + height + ',width=' + width +',toolbar=no,status=no,linemenubar=no,scrollbars=yes,resizable=yes,modal=yes,left=' + leftVal + ',top=' + topVal);
    }
    
    function DeleteItem() 
    { 
        var ddlNguoiChoAn = document.getElementById('<%= ddlNguoiChoAn.ClientID %>');
        var optionsList = ''; 
        if (ddlNguoiChoAn.value.length > 0 ) 
        {
            var itemIndex = ddlNguoiChoAn.selectedIndex;
            if (itemIndex >= 0 ) 
                ddlNguoiChoAn.remove(itemIndex); 
        } 
        for (var i=0; i<ddlNguoiChoAn.options.length; i++) 
        {
            var optionValue = ddlNguoiChoAn.options[i].value;
            optionsList += optionValue;
            optionsList += ',';
        } 
        document.getElementById('<%= hdNguoiChoAn.ClientID %>').value = optionsList; 
    } 

    function AddItem() 
    { 
        var ddlNguoiChoAn = document.getElementById('<%=ddlNguoiChoAn.ClientID %>');
        var ddlNhanVien = document.getElementById('<%=ddlNhanVien.ClientID %>'); 
        var ddlKhuChuong = document.getElementById('<%=ddlKhuChuong.ClientID %>'); 
        var optionsList = ''; 
        var idxNhanVien = ddlNhanVien.selectedIndex;
        var nhanvien = ddlNhanVien.options[idxNhanVien].text;
        var nhanvienValue = ddlNhanVien.options[idxNhanVien].value;
        var idxKhuChuong = ddlKhuChuong.selectedIndex;
        var khuchuong = ddlKhuChuong.options[idxKhuChuong].text;
        var khuchuongValue = ddlKhuChuong.options[idxKhuChuong].value;
        
        var option1 = document.createElement("option"); 
        option1.text= nhanvien + ":" + khuchuong; 
        option1.value= nhanvienValue + ":" + khuchuongValue; 
        ddlNguoiChoAn.options.add(option1); 
          
        for (var i=0; i<ddlNguoiChoAn.options.length; i++) 
        {
            var optionValue = ddlNguoiChoAn.options[i].value;
            optionsList += optionValue;
            optionsList += ',';
        } 
        document.getElementById('<%= hdNguoiChoAn.ClientID %>').value = optionsList; 
    }
    
    function loaica_change(value) 
    { 
        var ddlNhanVien = document.getElementById('<%=ddlNhanVien.ClientID %>'); 
        var ddlKhuChuong = document.getElementById('<%=ddlKhuChuong.ClientID %>'); 
        if(ddlNhanVien != null && ddlKhuChuong != null)
        {
            var s = document.getElementById('<%= hdLoaiCaNhanVienKhuChuong.ClientID %>').value;
            var temp = ";" + value + "!";
            var start = s.indexOf(temp);
            var end = s.indexOf(";", start + 1);
            var sub = s.substring(start + temp.length, end);
            var atIndex = sub.indexOf("@");
            var nv = sub.substring(0, atIndex);
            var kc = sub.substring(atIndex + 1, sub.length);    
            ddlNhanVien.value = nv;
            ddlKhuChuong.value = kc;
        }
    }
    
    function nhanvien_change(value) 
    { 
        var ddlKhuChuong = document.getElementById('<%=ddlKhuChuong.ClientID %>'); 
        var s = document.getElementById('<%= hdLoaiCaNhanVienKhuChuong.ClientID %>').value;
        var temp = "!" + value + "@";
        var start = s.indexOf(temp);
        var end = s.indexOf(";", start + 1);
        var kc = s.substring(start + temp.length, end);
        ddlKhuChuong.value = kc;
    }
    
    function finishEdit()
    {
        jQuery('#<%= btnLoad.ClientID %>').click();
    }
</script>
<table width="100%">
    <tr>
        <td>Thời điểm cho ăn</td>
        <td><asp:TextBox ID="txtThoiDiem" runat="server" Width="400" TabIndex="2"/>
            <cc1:calendarextender id="calThoiDiem" runat="server" format="dd/MM/yyyy 12:00:00"
                popupbuttonid="txtThoiDiem" targetcontrolid="txtThoiDiem"></cc1:calendarextender>
        </td>
    </tr>
    <tr>
        <td></td>
        <td>
            <asp:Button ID="btnSaveCaSauAn" runat="server" OnClick="btnSaveCaSauAn_Click" CssClass="button" Text="Lưu"/>
            <asp:HyperLink ID="lnkCancel" runat="server" Font-Bold="true" tabindex="13" CssClass="button">Thoát</asp:HyperLink>            
        </td>
    </tr>
</table>
<br />
<table width="100%" runat="server" id="tblThucAnChuong">
    <tr>
        <td align="left">
            <a><b>CHI TIẾT THỨC ĂN</b></a><asp:HyperLink ID="lnkThuoc" runat="server" Font-Bold="true" tabindex="13" style="float:right;margin:0 15px;">XEM CHI TIẾT THUỐC THÚ Y</asp:HyperLink><asp:HyperLink ID="lnkThucAnMulti" runat="server" Font-Bold="true" tabindex="14" style="float:right;cursor:pointer;margin:0 15px;">CHO ĂN NHIỀU THỨC ĂN</asp:HyperLink><asp:Button ID="btnLoad" runat="server" OnClick="btnLoad_Click" Width="0" BorderWidth="0" BackColor="Transparent"/><br />
            Hiển thị thức ăn cho cá ăn 
            <asp:DropDownList ID="ddlRowStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRowStatus_SelectedIndexChanged">
                <asp:ListItem Value="1">hiện hành</asp:ListItem>
                <asp:ListItem Value="0">đã xóa</asp:ListItem>
            </asp:DropDownList>
            <asp:GridView ID="grvThucAn" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" Width="100%" OnRowDataBound="grvThucAn_RowDataBound" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Loại cá">
                        <ItemTemplate>
                            <asp:Label ID="lblTenLoaiCa" runat="server"/>
                        </ItemTemplate>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <b>Tổng cộng</b>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Thức ăn">
                        <ItemTemplate>
                            <asp:Label ID="lblTenVatTu" runat="server"/>
                        </ItemTemplate>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tổng<br/>số cá">
                        <ItemTemplate>
                            <asp:Label ID="lblSoLuongCa" runat="server"/>
                        </ItemTemplate><ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Số cá<br/>tăng trọng">
                        <ItemTemplate>
                            <asp:Label ID="lblSLTT" runat="server"/>
                        </ItemTemplate><ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Số cá<br/>giống">
                        <ItemTemplate>
                            <asp:Label ID="lblSLG" runat="server"/>
                        </ItemTemplate><ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Khối lượng<br/>thức ăn (kg)">
                        <ItemTemplate>
                            <asp:Label ID="lblKhoiLuong" runat="server"/>
                        </ItemTemplate><ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <asp:Label Font-Bold="true" ID="lblTongKhoiLuong" runat="server"></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tăng trọng<br/>ăn (kg)">
                        <ItemTemplate>
                            <asp:Label ID="lblKLTT" runat="server"/>
                        </ItemTemplate><ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <asp:Label Font-Bold="true" ID="lblTongKLTT" runat="server"></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Giống<br/>ăn (kg)">
                        <ItemTemplate>
                            <asp:Label ID="lblKLG" runat="server"/>
                        </ItemTemplate><ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <asp:Label Font-Bold="true" ID="lblTongKLG" runat="server"></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Người cho ăn">
                        <ItemTemplate>
                            <asp:Label ID="lblNguoiChoAn" runat="server"/>
                        </ItemTemplate>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnXemThayDoi" runat="server" OnClick="btnXemThayDoi_Click" ToolTip="Xem thay đổi (F1)"/>
                        </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
        </td>
    </tr>
    <tr>
        <td runat="server" id="tdChuong">
            <br />
            <b>CHI TIẾT THỨC ĂN TỪNG CHUỒNG</b><br />
            <span style="vertical-align:top;">Loại cá:</span>
            <asp:DropDownList style="vertical-align:top;" ID="ddlLoaiCa" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLoaiCa_SelectedIndexChanged" onchange="loaica_change(this.value);"/>&nbsp;&nbsp;&nbsp;
            <span style="vertical-align:top;">Thức ăn:</span>
            <asp:DropDownList style="vertical-align:top;" ID="ddlThucAn" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlThucAn_SelectedIndexChanged"/>&nbsp;&nbsp;
            <span style="vertical-align:top;">(Tồn </span><asp:Label style="vertical-align:top;" ID="lblTon" runat="server"></asp:Label><span style="vertical-align:top;"> kg)</span>&nbsp;
            <span style="vertical-align:top;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Khối lượng (kg):</span>
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong" runat="server" Width="50" onblur="updateKhoiLuongPhanBo(this);">0</asp:TextBox>
            <span style="vertical-align:top;">Số lượng cá ăn:</span>
            <asp:TextBox style="vertical-align:top;" ID="txtSoCaAn" runat="server" Width="50" Enabled="false" ForeColor="Black">0</asp:TextBox>
            <br />
            <span style="vertical-align:top;">Nhân viên</span>
            <span style="vertical-align:top;">
                <asp:DropDownList ID="ddlNhanVien" runat="server" onchange="nhanvien_change(this.value);">
                </asp:DropDownList>
            </span>
            <span style="vertical-align:top;">cho ăn khu chuồng</span>
            <span style="vertical-align:top;">
                <asp:DropDownList ID="ddlKhuChuong" runat="server">
                    <asp:ListItem Value="KSS">Sinh sản</asp:ListItem>
                    <asp:ListItem Value="DDA">Dưỡng da</asp:ListItem>
                    <asp:ListItem Value="cacon">Cá con</asp:ListItem>
                    <asp:ListItem Value="TG">Trung gian</asp:ListItem>
                    <asp:ListItem Value="TP">Thương phẩm</asp:ListItem>
                    <asp:ListItem Value="KU">Khay úm</asp:ListItem>
                    <asp:ListItem Value="LU">Lồng úm</asp:ListItem>
                </asp:DropDownList>
            </span>
            <asp:HyperLink ID="lnkAdd" runat="server" CssClass="button" onclick="AddItem();">--></asp:HyperLink>
            <asp:ListBox ID="ddlNguoiChoAn" runat="server" AutoPostBack="false" Rows="4" ToolTip="Người cho ăn" EnableViewState="true"></asp:ListBox>
            <asp:HyperLink ID="lnkDelete" runat="server" CssClass="button" onclick="DeleteItem();">Xóa</asp:HyperLink>
            <br />
            <asp:GridView ID="grvDanhSach" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvDanhSach_RowDataBound" Width="100%" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="STT">
                        <ItemTemplate>
                            <asp:Label ID="lblSTT" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <b>Tổng</b>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Chuồng">
                        <ItemTemplate>
                            <asp:Label ID="lblChuong" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tổng<br/>số cá">
                        <ItemTemplate>
                            <asp:Label ID="lblSoLuong" runat="server" class="LabelSoLuong"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <asp:Label ID="lblTongSoLuong" runat="server" Font-Bold="true"></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Số cá<br/>tăng trọng">
                        <ItemTemplate>
                            <asp:Label ID="lblSLTT" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <asp:Label ID="lblTongSLTT" runat="server" Font-Bold="true"></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Số cá<br/>giống">
                        <ItemTemplate>
                            <asp:Label ID="lblSLG" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <asp:Label ID="lblTongSLG" runat="server" Font-Bold="true"></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tăng trọng<br/>ăn(kg)">
                        <ItemTemplate>
                            <asp:Label ID="lblKLTT" runat="server" CssClass="lblKLTT"/>
                        </ItemTemplate><ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <asp:Label ID="lblTongKLTT" runat="server" Font-Bold="true" CssClass="lblTongKLTT"></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Giống<br/>ăn (kg)">
                        <ItemTemplate>
                            <asp:Label ID="lblKLG" runat="server" CssClass="lblKLG"/>
                        </ItemTemplate><ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <asp:Label ID="lblTongKLG" runat="server" Font-Bold="true" CssClass="lblTongKLG"></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Trung bình<br/>(kg/con)">
                        <ItemTemplate>
                            <asp:Label ID="lblTrungBinh" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <input type="CheckBox" name="SelectAllCheckBox" onclick="SelectAll(this);" class="SChkChon"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="CheckBox" id="chkChon" runat="server" class="ChkChon"/>
                        </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterTemplate>
                            <input type="CheckBox" name="SelectAllCheckBoxF" onclick="SelectAll(this);" class="SChkChon"/>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Khối lượng<br/>thức ăn (kg)">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkDelKhoiLuong" runat="server">Reset</asp:HyperLink>
                            <asp:TextBox ID="txtKhoiLuong" Width="50" runat="server" class="TextKhoiLuong" style="text-align:right;" onblur="Calc(this);CalcTong();">0</asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <asp:TextBox ID="txtTongKhoiLuong" ForeColor="Black" Width="50" runat="server" CssClass="txtTongKhoiLuong" style="text-align:right;" Enabled="false"></asp:TextBox>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ghi chú">
                        <ItemTemplate>
                            <asp:TextBox ID="txtGhiChu" runat="server" Width="200"></asp:TextBox>
                            <asp:Button ID="btnSaveGhiChu" runat="server" OnClick="btnSaveGhiChu_Click" CssClass="button" Text="Lưu ghi chú"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Panel runat="server" ID="pnlCommand">
                <asp:Button ID="btnSaveThucAn" runat="server" OnClick="btnSaveThucAn_Click" CssClass="button" tabindex="12" Text="Lưu thức ăn"/>&nbsp;&nbsp;&nbsp;&nbsp;
                <span style="float:right;">
                    <asp:HyperLink ID="lnkPhanBoThucAn" runat="server" CssClass="button">Phân bổ thức ăn cho các chuồng được check chọn</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
                    Khối lượng phân bổ (kg) <asp:TextBox ID="txtKhoiLuongPhanBo" runat="server" Width="50">0</asp:TextBox>
                    Số chữ số thập phân <input type="text" id="txtThapPhan" style="width:50px;" value="<%=scaleCT.ToString()%>"/>
                </span>
            </asp:Panel>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdCaSauAn" runat="server" Value=""/>
<asp:HiddenField ID="hdNgayAn" runat="server" Value=""/>
<asp:HiddenField ID="hdOldRowID" runat="server" Value=""/>
<asp:HiddenField ID="hdOldColor" runat="server" Value=""/>
<asp:HiddenField ID="hdButtonID" runat="server" Value=""/>
<asp:HiddenField ID="hdNguoiChoAn" runat="server" Value=""/>
<asp:HiddenField ID="hdLoaiCa" runat="server" Value=""/>
<asp:HiddenField ID="hdThucAn" runat="server" Value=""/>
<asp:HiddenField ID="hdLoaiCaNhanVienKhuChuong" runat="server" Value=""/>
<br /><br /><br /><br />