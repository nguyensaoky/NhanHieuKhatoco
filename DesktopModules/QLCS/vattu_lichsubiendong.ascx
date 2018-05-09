<%@ Control Language="C#" AutoEventWireup="true" CodeFile="vattu_lichsubiendong.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.vattu_lichsubiendong" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
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
</script>
<b>CHI TIẾT BIẾN ĐỘNG - <asp:Label ID="lblVatTu" runat="server"/></b><br/>
<table class="FileManager_ToolBar">
    <tr>
        <td>
            Hiển thị biến động
            <asp:DropDownList ID="ddlRowStatus" runat="server">
                <asp:ListItem Value="1">hiện hành</asp:ListItem>
                <asp:ListItem Value="0">đã xóa</asp:ListItem>
            </asp:DropDownList>
            Từ ngày
            <asp:TextBox ID="txtFromDate" runat="server" Width="100" TabIndex="6"/>
            <cc1:calendarextender id="calFromDate" runat="server" format="dd/MM/yyyy" popupbuttonid="txtFromDate" targetcontrolid="txtFromDate"></cc1:calendarextender>
            đến ngày
            <asp:TextBox ID="txtToDate" runat="server" Width="100" TabIndex="7"/>
            <cc1:calendarextender id="calToDate" runat="server" format="dd/MM/yyyy" popupbuttonid="txtToDate" targetcontrolid="txtToDate"></cc1:calendarextender>
            <asp:Button ID="btnLoad" runat="server" Text="         Xem         " OnClick="btnLoad_Click" CssClass="button"/>
            <br />
            <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Red"/>
        </td>
    </tr>
</table>
<span style="float:left;">
    <asp:Button ID="btnKhoa" runat="server" Text="Khóa" OnClick="btnKhoa_Click" CssClass="button"/>
    <asp:Button ID="btnMoKhoa" runat="server" Text="Mở khóa" OnClick="btnMoKhoa_Click" CssClass="button"/>
</span>
<br />
<asp:GridView ID="grvDanhSach" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvDanhSach_RowDataBound" Width="100%">
    <Columns>
        <asp:BoundField DataField="NgayBienDong" HeaderText="Thời điểm biến động" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="False">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Số lượng trước đó">
            <ItemTemplate>
                <asp:Label ID="lblSoLuongTruocDo" runat="server"></asp:Label>
            </ItemTemplate><ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="TenLoaiBienDong" HeaderText="Loại biến động">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Số lượng biến động">
            <ItemTemplate>
                <asp:Label ID="lblSoLuongBienDong" runat="server"></asp:Label>
            </ItemTemplate><ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Số lượng sau đó">
            <ItemTemplate>
                <asp:Label ID="lblSoLuongSauDo" runat="server"></asp:Label>
            </ItemTemplate><ItemStyle HorizontalAlign="Right"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Nguồn">
            <ItemTemplate>
                <asp:Label ID="lblNguon" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:BoundField DataField="Note" HeaderText="Ghi chú">
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Sửa số lượng">
            <ItemTemplate>
                <asp:TextBox ID="txtSoLuongBienDong" runat="server"></asp:TextBox>
                <asp:DropDownList ID="ddlNhaCungCap" runat="server"></asp:DropDownList>
                <asp:Button ID="btnSave" runat="server" Text="Lưu" OnClick="btnSave_Click" CssClass="button"/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <input type="CheckBox" name="SelectAllCheckBox" onclick="SelectAll(this)" />
            </HeaderTemplate>
            <ItemTemplate>
                <input type="CheckBox" id="chkChon" runat="server" class="ChkChon" onclick = "chon_click(event, this);" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Xem thay đổi">
            <ItemTemplate>
                <asp:Button ID="btnXemThayDoi" runat="server" OnClick="btnXemThayDoi_Click" ToolTip="Xem thay đổi (F1)"/>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle ForeColor="Yellow"></HeaderStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<br />
<asp:HyperLink ID="lnkCancel" runat="server" CssClass="button">Trở lại</asp:HyperLink>
<asp:HiddenField ID="hdOldRowID" runat="server" Value=""/>
<asp:HiddenField ID="hdOldColor" runat="server" Value=""/>
<asp:HiddenField ID="hdButtonID" runat="server" Value=""/>