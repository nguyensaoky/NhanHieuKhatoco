<%@ Control Language="C#" AutoEventWireup="true" CodeFile="gietmoca_chonca.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.gietmoca_chonca" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
    function chon_click(id)
    {
        var t = document.getElementById('<%= txtIDCa.ClientID%>');
        t.value = id;
        scroll(0,0);
    }
    
    function chonall()
    {
        l = <%=cblStatus.Items.Count%>;
        lnk = jQuery('#<%= lnkCheckAll.ClientID%>');
        
        if(lnk.text() == 'Chọn tất cả')
        {
            for(i = 0; i<l; i++)
            {
                jQuery('#<%= cblStatus.ClientID%>_' + i).attr('checked', true);
            }
            lnk.text('Bỏ tất cả');
        }
        else
        {
            for(i = 0; i<l; i++)
            {
                jQuery('#<%= cblStatus.ClientID%>_' + i).attr('checked', false);
            }
            lnk.text('Chọn tất cả');
        }
    }
</script>
<table width="100%" cellpadding="5">
    <tr>
        <td style="width:75%;" valign="top" runat="server" id="SearchCa">
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
                Ngày nhập chuồng từ
                <asp:TextBox ID="txtNhapChuongFrom" runat="server" Width="100" TabIndex="6"/>
                <cc1:calendarextender id="calNhapChuongFrom" runat="server" format="dd/MM/yyyy" popupbuttonid="txtNhapChuongFrom" targetcontrolid="txtNhapChuongFrom"></cc1:calendarextender>
                đến ngày
                <asp:TextBox ID="txtNhapChuongTo" runat="server" Width="100" TabIndex="7"/>
                <cc1:calendarextender id="calNhapChuongTo" runat="server" format="dd/MM/yyyy" popupbuttonid="txtNhapChuongTo" targetcontrolid="txtNhapChuongTo"></cc1:calendarextender>
                <br />
                Trạng thái :
                <asp:CheckBoxList id="cblStatus" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem ID="chkBT" Value="0" Selected="true">Bình thường</asp:ListItem>
                    <asp:ListItem ID="chkBenh" Value="1">Bệnh</asp:ListItem>
                </asp:CheckBoxList>
                <asp:HyperLink ID="lnkCheckAll" runat="server" CssClass="button" onclick="chonall()">Chọn tất cả</asp:HyperLink>
            </td>
            <td align="right" valign="top">
                <asp:Button ID="btnLoad" runat="server" Text="Tìm" OnClick="btnLoad_Click" CssClass="button"/>
            </td>
            </tr>
            </table>
            <table class="FileManager_ToolBar">
                <tr>
                    <td align="left">
                        Tổng số cá trong danh sách: <asp:Label Font-Bold="true" ID="lblTongSo" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        Hiển thị
                        <asp:DropDownList runat="server" ID="ddlPageSize">
                            <asp:ListItem>15</asp:ListItem>
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
                    <asp:BoundField DataField="IDCaSau" HeaderText="ID" SortExpression="[IDCaSau] DESC">
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
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
                    <asp:BoundField DataField="NgayXuongChuong" HeaderText="Ngày XC" SortExpression="[NgayXuongChuong] DESC"
                        DataFormatString="{0:dd/MM/yyyy}">
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TenNguonGoc" HeaderText="Nguồn gốc" SortExpression="[NguonGoc] DESC">
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TrangThai" HeaderText="Trạng thái" SortExpression="[Status] DESC">
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink Font-Bold="true" ID="btnChon" runat="server" Text="Chọn" CssClass="button"></asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsDanhSach" runat="server" EnableCaching="false" SelectMethod="Get_HienTai" EnablePaging="true"
            TypeName="DotNetNuke.Modules.QLCS.CaSauDataObject" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
            SelectCountMethod="Count_HienTai" SortParameterName="sortBy">
                <SelectParameters>
                    <asp:Parameter Name="WhereClause" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </td>
        <td valign="top" style="width:25%;">
            <div style="width:200px;float:left;">ID Cá:</div>
            <div style="width:200px;float:left;"><asp:TextBox runat="server" ID="txtIDCa" Width="200" BackColor="LightGray"></asp:TextBox></div>
            <div style="clear:both;"></div>
            <div style="width:200px;float:left;">Trọng lượng da (kg)</div>
            <div style="width:200px;float:left;"><asp:TextBox runat="server" ID="txtDa_TrongLuong" Width="200" AutoComplete="Off">0</asp:TextBox></div>
            <div style="clear:both;"></div>
            <div style="width:200px;float:left;">Kích thước da (cm)</div>
            <div style="width:200px;float:left;"><asp:TextBox runat="server" ID="txtDa_Bung" Width="200" AutoComplete="Off"></asp:TextBox></div>
            <div style="clear:both;"></div>
            <div style="width:200px;float:left;">Phân loại da</div>
            <div style="width:200px;float:left;">
                <asp:DropDownList ID="ddlDa_PhanLoai" runat="server" Width="200">
                    <asp:ListItem Value="1">Loại 1</asp:ListItem>
                    <asp:ListItem Value="2">Loại 2</asp:ListItem>
                    <asp:ListItem Value="3">Loại 3</asp:ListItem>
                    <asp:ListItem Value="4" Selected="True">Loại 4</asp:ListItem>
                    <asp:ListItem Value="5">Loại 5</asp:ListItem>
                    <asp:ListItem Value="6">Loại CXĐ</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div style="clear:both;"></div>
            <div style="width:200px;float:left;">&nbsp;</div><div style="width:200px;float:left;"><asp:CheckBox ID="chkDau" runat="server" Text="Thu hồi đầu"/></div>
            <div style="clear:both;"></div>
            <div style="width:200px;float:left;">Người mổ</div>
            <div style="width:200px;float:left;">
                <asp:DropDownList ID="ddlNguoiMo" runat="server">
                </asp:DropDownList>
            </div>
            <div style="clear:both;"></div>
            <div style="width:200px;float:left;">Trọng lượng hơi (kg)</div>
            <div style="width:200px;float:left;"><asp:TextBox runat="server" ID="txtTrongLuongHoi" Width="200" AutoComplete="Off"></asp:TextBox></div>
            <div style="clear:both;"></div>
            <div style="width:200px;float:left;">Trọng lượng móc hàm (kg)</div>
            <div style="width:200px;float:left;"><asp:TextBox runat="server" ID="txtTrongLuongMocHam" Width="200" AutoComplete="Off"></asp:TextBox></div>
            <div style="clear:both;"></div>
            <div style="width:200px;float:left;">Phương pháp mổ</div>
            <div style="width:200px;float:left;"><asp:DropDownList ID="ddlPhuongPhapMo" runat="server" Width="200">
                    <asp:ListItem Value="CL">CL</asp:ListItem>
                    <asp:ListItem Value="MDL">MDL</asp:ListItem>
                    <asp:ListItem Value="CB">CB</asp:ListItem>
                    <asp:ListItem Value="CB-KDL">CB-KDL</asp:ListItem>
                    <asp:ListItem Value=""></asp:ListItem>
                </asp:DropDownList></div>
            <div style="clear:both;"></div>
            <div style="width:200px;float:left;">&nbsp;</div><div style="width:200px;float:left;"><asp:CheckBox ID="chkDiTat" runat="server" Text="Dị tât"/></div>
            <div style="clear:both;"></div>
            <asp:PlaceHolder ID="dsVatTu" runat="server" EnableViewState="true"></asp:PlaceHolder>
            <div style="width:200px;float:left;">
                <asp:Label runat="server" Text="0" ID="lblGMCCT" Visible="false"></asp:Label>
                <asp:Label runat="server" Text="0" ID="lblGMC" Visible="false"></asp:Label>
            </div>
            <div style="width:200px;float:left;"><asp:Button id="btnSave" runat="server" Text="Lưu" OnClick="btnSave_Click" CssClass="button"></asp:Button><asp:Button id="btnDelete" runat="server" Text="Xóa" OnClick="btnDelete_Click" CssClass="button" Visible="false"></asp:Button></div>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdListBienDongPage" runat="server" Value="0" EnableViewState="true"/>