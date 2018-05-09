<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casauan_thuoc_multi.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casauan_thuoc_multi" %>
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
</script>
<table width="100%" runat="server" id="tblThuocChuong">
    <tr>
        <td runat="server" id="tdChuong">
            Loại cá:
            <asp:DropDownList style="vertical-align:top;" ID="ddlLoaiCa" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLoaiCa_SelectedIndexChanged"/><br />
            <br />
            Thuốc 1:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThuoc1" runat="server"/>
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong1" runat="server" Width="50"></asp:TextBox>
            Thuốc 2:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThuoc2" runat="server"/>
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong2" runat="server" Width="50"></asp:TextBox>
            Thuốc 3:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThuoc3" runat="server"/>
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong3" runat="server" Width="50"></asp:TextBox>
            <br />
            Thuốc 4:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThuoc4" runat="server"/>
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong4" runat="server" Width="50"></asp:TextBox>
            Thuốc 5:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThuoc5" runat="server"/>
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong5" runat="server" Width="50"></asp:TextBox>
            Thuốc 6:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThuoc6" runat="server"/>
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong6" runat="server" Width="50"></asp:TextBox>
            <br />
            Thuốc 7:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThuoc7" runat="server"/>
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong7" runat="server" Width="50"></asp:TextBox>
            Thuốc 8:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThuoc8" runat="server"/>
            <asp:TextBox style="vertical-align:top;" ID="txtKhoiLuong8" runat="server" Width="50"></asp:TextBox>
            Thuốc 9:
            <asp:DropDownList style="vertical-align:top;" ID="ddlThuoc9" runat="server"/>
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
                <asp:Button ID="btnSaveThuoc" runat="server" OnClick="btnSaveThuoc_Click" CssClass="button" tabindex="12" Text="Lưu thuốc"/>&nbsp;&nbsp;&nbsp;&nbsp;
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
<asp:HiddenField ID="hdLoaiCa" runat="server" Value=""/>
<asp:HiddenField ID="hdThuoc" runat="server" Value=""/>
<br /><br /><br /><br />