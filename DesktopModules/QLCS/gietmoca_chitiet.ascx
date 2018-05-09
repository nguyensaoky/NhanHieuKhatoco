<%@ Control Language="C#" AutoEventWireup="true" CodeFile="gietmoca_chitiet.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.gietmoca_chitiet" %>
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
</script>
<table class="FileManager_ToolBar">
    <tr>
        <td align="left">
            Hiển thị cá giết mổ
            <asp:DropDownList ID="ddlRowStatus" runat="server">
                <asp:ListItem Value="1">hiện hành</asp:ListItem>
                <asp:ListItem Value="0">đã xóa</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td align="right">
            <asp:Button ID="btnLoad" runat="server" OnClick="btnLoad_Click" CssClass="button" Text="Tải"/>
            <asp:HyperLink ID="btnAddGietMoCaChiTiet" runat="server" Text="Thêm cá giết mổ" CssClass="button"></asp:HyperLink>     
        </td>
    </tr>
</table>
<table width="100%" cellpadding="5">
    <tr>
        <td style="width:100%;" valign="top">
            <asp:Label ID="lblNoData" runat="server" Text="Chưa có cá giết mổ" ForeColor="red" Visible="false"></asp:Label>
            <asp:GridView ID="grvDanhSach" runat="server"
                AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvDanhSach_RowDataBound" PageSize="50" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="STT">
                        <ItemTemplate>
                            <asp:Label ID="lblSTT" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cá giết mổ">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkCaSau" runat="server" Text='<%# Eval("TenCa") %>' />
                        </ItemTemplate>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Trọng lượng da (kg)">
                        <ItemTemplate>
                            <asp:Label ID="lblDa_TrongLuong" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Da_Bung" HeaderText="Kích thước da (cm)">
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Da_PhanLoai" HeaderText="Phân loại da">
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Lấy đầu">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDau" runat="server" Enabled="false"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TenNguoiMo" HeaderText="Người mổ">
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Trọng lượng hơi (kg)">
                        <ItemTemplate>
                            <asp:Label ID="lblTrongLuongHoi" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Trọng lượng móc hàm (kg)">
                        <ItemTemplate>
                            <asp:Label ID="lblTrongLuongMocHam" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PhuongPhapMo" HeaderText="Phương pháp mổ">
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Dị tật">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDiTat" runat="server" Enabled="false"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sản phẩm khác">
                        <ItemTemplate>
                            <asp:Label ID="lblVatTu" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnXemThayDoi" runat="server" OnClick="btnXemThayDoi_Click" ToolTip="Xem thay đổi (F1)"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
<table class="FileManager_ToolBar">
    <tr>
        <td align="center">
            <b>Tổng trọng lượng hơi:</b> <asp:Label ID="lblTongHoi" runat="server" Text="0"></asp:Label> kg
            <br />
            <b>Tổng trọng lượng móc hàm:</b> <asp:Label ID="lblTongMocHam" runat="server" Text="0"></asp:Label> kg
            <br /><br />
            <asp:HyperLink ID="lnkCancel" runat="server" Font-Bold="true" tabindex="13" CssClass="button">Trở lại</asp:HyperLink>
        </td>
    </tr>
</table>
<asp:Label ID="lblGMC" runat="server" Text="0" Visible="false"></asp:Label>
<asp:HiddenField ID="hdGietMoCaChonCaPage" runat="server" Value="0" EnableViewState="true"/>
<asp:HiddenField ID="hdOldRowID" runat="server" Value=""/>
<asp:HiddenField ID="hdOldColor" runat="server" Value=""/>
<asp:HiddenField ID="hdButtonID" runat="server" Value=""/>