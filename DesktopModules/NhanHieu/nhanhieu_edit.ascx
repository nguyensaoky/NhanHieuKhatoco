<%@ Control Language="C#" AutoEventWireup="true" CodeFile="nhanhieu_edit.ascx.cs" Inherits="DotNetNuke.Modules.NhanHieu.nhanhieu_edit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "/style.css" %>" rel="stylesheet" type="text/css" />
<asp:UpdatePanel ID="udpThongTinChung" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <b>THÔNG TIN CHUNG</b>
        <table width="100%" cellspacing="5">
            <tr>
                <td style="width:300px;">Tên nhãn hiệu</td>
                <td><asp:TextBox ID="txtTenNhanHieu" runat="server" Width="400"/>
                </td>
            </tr>
            <tr><td>Nước đăng ký</td><td><asp:DropDownList ID="ddlNuocDangKy" runat="server"></asp:DropDownList></td></tr>
            <tr><td>Nhãn hiệu gốc</td><td><asp:DropDownList ID="ddlNhanHieuGoc" runat="server"></asp:DropDownList></td></tr>
            <tr><td>Số đơn</td><td><asp:TextBox ID='txtSoDon' runat='server'/></td></tr>
	        <tr><td>Ngày nộp đơn</td><td><asp:TextBox ID='txtNgayNopDon' runat='server'/></td></tr>
	        <tr><td>Ngày ưu tiên</td><td><asp:TextBox ID='txtNgayUuTien' runat='server'/></td></tr>
	        <tr><td>Số chứng nhận</td><td><asp:TextBox ID='txtSoChungNhan' runat='server'/></td></tr>
	        <tr><td>Ngày chứng nhận</td><td><asp:TextBox ID='txtNgayChungNhan' runat='server'/></td></tr>
	        <tr><td>Ngày công bố</td><td><asp:TextBox ID='txtNgayCongBo' runat='server'/></td></tr>
	        <tr><td>Số quyết định</td><td><asp:TextBox ID='txtSoQuyetDinh' runat='server'/></td></tr>
	        <tr><td>Ngày quyết định</td><td><asp:TextBox ID='txtNgayQuyetDinh' runat='server'/></td></tr>
	        <tr><td>Ghi chú</td><td><asp:TextBox ID='txtNote' runat='server'/></td></tr>
            <tr>
	            <td></td>
	            <td>
	                <asp:Button ID="btnSaveThongTinChung" runat="server" Text="Lưu thông tin chung" OnClick="btnSaveThongTinChung_Click"></asp:Button>&nbsp;
	            </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="udpNoiDung" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <b>NỘI DUNG NHÃN HIỆU</b>
        <table width="100%" cellspacing="5">
	        <tr id="trCurrentFile" runat="server">
                <td style="width:300px;">Hình ảnh hiện tại</td>
                <td>
                    <asp:Label runat="server" ID="lblCurrentFileName"/>
                </td>
            </tr>
            <tr>
                <td style="width:300px;"><asp:Label runat="server" ID="lblChooseFile"/></td>
                <td>
                    <input id="txtFile" runat="server" name="txtFile" size="30" type="file" width="300"/>
                    <asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" EnableViewState="False"></asp:Label>
                </td>
            </tr>
	        <tr><td>Mô tả</td><td><asp:TextBox ID='txtMoTa' runat='server'/></td></tr>
	        <tr><td>Màu sắc</td><td><asp:TextBox ID='txtMauSac' runat='server'/></td></tr>
	        <tr><td>Loại nhãn hiệu</td><td><asp:DropDownList ID="ddlLoaiNhanHieu" runat="server"></asp:DropDownList></td></tr>
	        <tr><td>Lĩnh vực</td><td><asp:ListBox ID='lstLinhVuc' runat="server" SelectionMode = "Multiple"></asp:ListBox></td></tr>
	        <tr><td>Mô tả thay đổi</td><td><asp:TextBox ID='txtGhiChuThayDoi' runat='server'/></td></tr>
            <tr>
	            <td></td>
	            <td>
	                <asp:Button ID="btnSaveNoiDung" runat="server" Text="Lưu" OnClick="btnSaveNoiDung_Click"></asp:Button>&nbsp;
	            </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="udpButton" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Button ID="btnDVGuiTCT_TCT" runat="server" Text="Gửi TCT" OnClick="btnDVGuiTCT_Click"></asp:Button>
        <asp:Button ID="btnDVTrieuHoi_DV" runat="server" Text="Triệu hồi" OnClick="btnDVTrieuHoi_Click"></asp:Button>
        <asp:Button ID="btnTCTGuiDV_DV" runat="server" Text="Gửi đơn vị" OnClick="btnTCTGuiDV_Click"></asp:Button>
        <asp:Button ID="btnTCTChapNhan_TCT" runat="server" Text="TCT chấp nhận" OnClick="btnTCTChapNhan_Click"></asp:Button>
        <asp:Button ID="btnCucGopYTCT_TCT" runat="server" Text="Cục góp ý TCT" OnClick="btnCucGopYTCT_Click"></asp:Button>
        <asp:Button ID="btnCucDuyet_TCT" runat="server" Text="Cục duyệt" OnClick="btnCucDuyet_Click"></asp:Button>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField runat="server" ID="hdNhanHieuID" Value="0"/>
<asp:HiddenField runat="server" ID="hdBienDongID" Value="0"/>
<asp:HiddenField runat="server" ID="hdIsReferenced" Value="0"/>
<asp:HiddenField runat="server" ID="hdImage" Value="0"/>
<asp:HiddenField runat="server" ID="hdStatusID" Value="0"/>
<asp:HiddenField runat="server" ID="hdStatusName" Value=""/>
<asp:HiddenField runat="server" ID="hdUnit" Value=""/>
<asp:HiddenField runat="server" ID="hdOwner" Value=""/>