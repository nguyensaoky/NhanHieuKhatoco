<%@ Control Language="C#" AutoEventWireup="true" CodeFile="nhanhieu_edit.ascx.cs" Inherits="DotNetNuke.Modules.NhanHieu.nhanhieu_edit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "/style.css" %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
    function openwindow(url, name, width, height)
    {
        var leftVal = (screen.width - width) / 2;
        var topVal = (screen.height - height) / 2;
        window.open(url,'','height=' + height + ',width=' + width +',toolbar=no,status=no,linemenubar=no,scrollbars=yes,resizable=yes,modal=yes,left=' + leftVal + ',top=' + topVal);
    }
    
    function finishEdit1(status, statusname, message1)
    {
        //jQuery('#<%= hdIsReferenced.ClientID %>').val('1');
        //alert(jQuery('#<%= hdIsReferenced.ClientID %>').val());
        //jQuery('#<%= hdStatusID.ClientID %>').val(status);
        //jQuery('#<%= hdStatus.ClientID %>').val(statusname);
        //jQuery('#<%= hdMessage1.ClientID %>').val(message1);
        jQuery('#<%= btnReload.ClientID %>').click();
    }
    
    function finishEdit()
    {
        jQuery('#<%= btnReload.ClientID %>').click();
    }
</script>
<asp:UpdatePanel ID="udpThongTinChung" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <b>THÔNG TIN CHUNG</b>
        <table cellspacing="5">
            <tr>
                <td style="width:200px;">Tên nhãn hiệu</td><td style="width:300px;"><asp:TextBox ID="txtTenNhanHieu" runat="server" Width="300"/></td>
                <td style="width:200px;">Nước đăng ký</td><td style="width:300px;"><asp:DropDownList ID="ddlNuocDangKy" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Nhãn hiệu gốc</td><td><asp:DropDownList ID="ddlNhanHieuGoc" runat="server"></asp:DropDownList></td>
                <td>Số đơn</td><td><asp:TextBox ID='txtSoDon' runat='server'/></td>
            </tr>
	        <tr>
	            <td>Ngày nộp đơn</td><td><asp:TextBox ID='txtNgayNopDon' runat='server'/></td>
	            <td>Ngày ưu tiên</td><td><asp:TextBox ID='txtNgayUuTien' runat='server'/></td>
	        </tr>
	        <tr>
	            <td>Số chứng nhận</td><td><asp:TextBox ID='txtSoChungNhan' runat='server'/></td>
	            <td>Ngày chứng nhận</td><td><asp:TextBox ID='txtNgayChungNhan' runat='server'/></td>
	        </tr>
	        <tr>
	            <td>Ngày công bố</td><td><asp:TextBox ID='txtNgayCongBo' runat='server'/></td>
	            <td>Số quyết định</td><td><asp:TextBox ID='txtSoQuyetDinh' runat='server'/></td>
	        </tr>
	        <tr>
	            <td>Ngày quyết định</td><td><asp:TextBox ID='txtNgayQuyetDinh' runat='server'/></td>
	            <td>Ghi chú</td><td><asp:TextBox ID='txtNote' runat='server'/></td>
	        </tr>
            <tr>
	            <td></td>
	            <td colspan='3'>
	                <asp:Button ID="btnSaveThongTinChung" runat="server" Text="Lưu thông tin chung" OnClick="btnSaveThongTinChung_Click"></asp:Button>&nbsp;
	            </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="udpNoiDung" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <b>NỘI DUNG NHÃN HIỆU</b>
        <table cellspacing="5">
	        <tr id="trCurrentFile" runat="server">
                <td style="width:200px;">Hình ảnh hiện tại</td>
                <td colspan='3'>
                    <asp:Label runat="server" ID="lblCurrentFileName"/><br />
                    <asp:Image runat="server" ID="imgCurrentFile"/>
                </td>
            </tr>
            <tr>
                <td style="width:200px;"><asp:Label runat="server" ID="lblChooseFile"/></td>
                <td colspan='3'>
                    <input id="txtFile" runat="server" name="txtFile" size="30" type="file" width="300"/>
                    <asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" EnableViewState="False"></asp:Label>
                </td>
            </tr>
	        <tr><td>Mô tả</td><td style="width:300px;"><asp:TextBox ID='txtMoTa' runat='server'/></td><td style="width:200px;">Màu sắc</td><td style="width:300px;"><asp:TextBox ID='txtMauSac' runat='server'/></td></tr>
	        <tr><td>Loại nhãn hiệu</td><td><asp:DropDownList ID="ddlLoaiNhanHieu" runat="server"></asp:DropDownList></td><td style="width:200px;">Lĩnh vực</td><td><asp:ListBox ID='lstLinhVuc' runat="server" SelectionMode = "Multiple"></asp:ListBox></td></tr>
	        <tr><td>Mô tả thay đổi</td><td colspan='3'><asp:TextBox ID='txtGhiChuThayDoi' runat='server'/></td></tr>
	        <tr><td style="width:200px;"><asp:Label ID='lblStatusText' runat='server' Text='Trạng thái'/></td><td style="width:300px;"><asp:Label ID='lblStatus' runat='server'/></td><td style="width:200px;"><asp:Label ID='lblMessage1Text' runat='server' Text="Nội dung gửi"/></td><td style="width:300px;"><asp:Literal ID='lblMessage1' runat='server'/></td></tr>
            <tr><td></td><td colspan='3'><asp:Button ID="btnSaveNoiDung" runat="server" Text="Lưu" OnClick="btnSaveNoiDung_Click"></asp:Button>&nbsp;</td></tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="udpButton" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table cellspacing="5">
	        <tr>
                <td style="width:200px;"></td>
                <td>
                    <asp:Button ID="btnDVGuiTCT_TCT" runat="server" Text="Gửi TCT" OnClick="btnDVGuiTCT_Click"></asp:Button>
                    <asp:Button ID="btnDVTrieuHoi_DV" runat="server" Text="Triệu hồi" OnClick="btnDVTrieuHoi_Click"></asp:Button>
                    <asp:Button ID="btnTCTGuiDV_DV" runat="server" Text="Gửi đơn vị" OnClick="btnTCTGuiDV_Click"></asp:Button>
                    <asp:Button ID="btnTCTChapNhan_TCT" runat="server" Text="TCT chấp nhận" OnClick="btnTCTChapNhan_Click"></asp:Button>
                    <asp:Button ID="btnCucGopYTCT_TCT" runat="server" Text="Cục góp ý TCT" OnClick="btnCucGopYTCT_Click"></asp:Button>
                    <asp:Button ID="btnCucDuyet_TCT" runat="server" Text="Cục duyệt" OnClick="btnCucDuyet_Click"></asp:Button>
                    <asp:Button ID="btnReload" runat="server" Text="" OnClick="btnReload_Click" Width="0" Height="0" BorderWidth="0" BackColor="Transparent"></asp:Button>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="udpHidden" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hdNhanHieuID" Value="0"/>
        <asp:HiddenField runat="server" ID="hdBienDongID" Value="0"/>
        <asp:HiddenField runat="server" ID="hdIsReferenced" Value="0"/>
        <asp:HiddenField runat="server" ID="hdImage" Value="0"/>
        <asp:HiddenField runat="server" ID="hdStatusID" Value="0"/>
        <asp:HiddenField runat="server" ID="hdStatusName" Value=""/>
        <asp:HiddenField runat="server" ID="hdUnit" Value=""/>
        <asp:HiddenField runat="server" ID="hdOwner" Value=""/>
        <asp:HiddenField runat="server" ID="hdStatus" Value=""/>
        <asp:HiddenField runat="server" ID="hdMessage1" Value=""/>
    </ContentTemplate>
</asp:UpdatePanel>