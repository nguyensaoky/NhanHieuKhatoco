<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_chuyendan.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_chuyendan" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<div id="wrapper" runat="server">
    <b>CHÚ Ý: BACKUP CSDL TRƯỚC KHI THỰC HIỆN THAO TÁC NÀY</b><br/><br/>
    <div style="width:250px;float:left;"><b>CHUYỂN ĐÀN TỰ ĐỘNG</b></div>
    <br />
    <div style="float:left;">Chọn loại cá</div>
    <asp:CheckBoxList id="cblLoaiCa" runat="server" RepeatColumns="10" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem ID="chkCaCon" Value="1" Selected="true">Cá con</asp:ListItem>
        <asp:ListItem ID="chkMotNam" Value="2" Selected="true">Cá một năm</asp:ListItem>
        <asp:ListItem ID="chkST1" Value="3" Selected="true">Cá ST1</asp:ListItem>
        <asp:ListItem ID="chkST2" Value="4" Selected="true">Cá ST2</asp:ListItem>
        <asp:ListItem ID="chkHB1" Value="5" Selected="true">Cá HB1</asp:ListItem>
    </asp:CheckBoxList>
    <br />
    <div style="width:200px;float:left;">Ngày giờ chuyển</div>
    <asp:TextBox ID="txtThoiDiemChuyen" runat="server"></asp:TextBox>
    <br />
    <br />
    <div style="width:200px;float:left;">Nhập mã sau vào ô để xác nhận</div>
    <asp:Label ID="lblCode" runat="server"></asp:Label>
    <asp:TextBox ID="txtCode" runat="server"></asp:TextBox><asp:Label ID="lblError" runat="server"></asp:Label>
    <br />
    <br />
    <asp:Button ID="btnChuyen" runat="server" Text="Chuyển giai đoạn cho các loại cá được chọn phía trên" OnClick="btnChuyen_Click" CssClass="button"/>
    <br />
    <br />
    <br />
</div>