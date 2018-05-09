<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casauan_thucan_multi.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casauan_thucan_multi" %>
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
</script>
<table width="100%" runat="server" id="tblThucAnChuong">
    <tr>
        <td runat="server" id="tdChuong">
            Loại cá:
            <asp:DropDownList style="vertical-align:top;" ID="ddlLoaiCa" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLoaiCa_SelectedIndexChanged" onchange="loaica_change(this.value);"/><br />
            <br />
            <span style="vertical-align:top;">Nhân viên</span>
            <span style="vertical-align:top;"><asp:DropDownList ID="ddlNhanVien" runat="server" onchange="nhanvien_change(this.value);"></asp:DropDownList></span>
            <span style="vertical-align:top;">Khu chuồng</span>
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
            Thức ăn 1:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThucAn1" runat="server"/>            
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong1" runat="server" Width="50"></asp:TextBox>
            Thức ăn 2:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThucAn2" runat="server"/>            
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong2" runat="server" Width="50"></asp:TextBox>
            Thức ăn 3:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThucAn3" runat="server"/>    
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong3" runat="server" Width="50"></asp:TextBox>
            <br />
            Thức ăn 4:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThucAn4" runat="server"/>            
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong4" runat="server" Width="50"></asp:TextBox>
            Thức ăn 5:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThucAn5" runat="server"/>            
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong5" runat="server" Width="50"></asp:TextBox>
            Thức ăn 6:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThucAn6" runat="server"/>            
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong6" runat="server" Width="50"></asp:TextBox>
            <br />
            Thức ăn 7:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThucAn7" runat="server"/>            
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong7" runat="server" Width="50"></asp:TextBox>
            Thức ăn 8:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThucAn8" runat="server"/>            
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong8" runat="server" Width="50"></asp:TextBox>
            Thức ăn 9:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThucAn9" runat="server"/>            
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong9" runat="server" Width="50"></asp:TextBox>
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
                </Columns>
            </asp:GridView>
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
            <br />
            <asp:Panel runat="server" ID="pnlCommand">
                <asp:Button ID="btnSaveThucAn" runat="server" OnClick="btnSaveThucAn_Click" CssClass="button" tabindex="12" Text="Lưu thức ăn"/>&nbsp;&nbsp;&nbsp;&nbsp;
                <span style="float:right;">
                    Số chữ số thập phân 
                    <asp:TextBox ID="txtThapPhan" runat="server" Width="50"></asp:TextBox>
                    <asp:CheckBox ID="chkThemVao" runat="server" Text="Thêm vào dữ liệu có sẵn" Checked="true"/>
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