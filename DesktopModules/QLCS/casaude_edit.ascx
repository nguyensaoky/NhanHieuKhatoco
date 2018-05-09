<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casaude_edit.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casaude_edit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script src="<%= ResolveUrl("~/js/json2.js") %>" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    function CountSLNo()
    {
        var td = document.getElementById('<%=txtTrungDe.ClientID%>').value;
        var TrungBo = 0;
        $(".TrungBo").each(function() {
            if(!isNaN(this.value) && this.value.length!=0) {
                TrungBo += parseFloat(this.value);
            } 
        });
        document.getElementById('<%=txtConLai.ClientID%>').value = td - TrungBo;
    }
    
    function CountThaiLoai()
    {
        var TrungThaiLoai = 0;
        $(".TrungThaiLoai").each(function() {
            if(!isNaN(this.value) && this.value.length!=0) {
                TrungThaiLoai += parseFloat(this.value);
            } 
        });
        $(".lblTongThaiLoai").each(function() {
            this.innerHTML = TrungThaiLoai;
        });
    }
    
    function confirmDelete()
    {
        var r=confirm("Bạn có chắc bạn muốn xóa?");
        if (r==true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    $(document).ready(function() { 
        CountSLNo();
        if($("#<%=hdDaLoadKhayAp.ClientID%>").val() == "0")
            getAvailableKhayAp();
    });
    
    var DataCheck = function(type, date, idtheodoide) {
        this.Type = type;
        this.Date = date;
        this.IDTheoDoiDe = idtheodoide;
    }
    
    function getAvailableKhayUm()
    {
        var date = $("#<%=txtNgayNo.ClientID%>").val();
        var idtheodoide = $("#<%=hdIDTDD.ClientID%>").val();
        var jsDataCheck = new DataCheck(2, date, idtheodoide);
        var jsonText = JSON.stringify(jsDataCheck);
        $.ajax({
            type: "POST",
            url: location.href,
            data: jsonText,
            dataType: "json",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            success: function (data) {
                $('#<%=ddlKhayUm.ClientID%>').empty();
		        var count = 1;
                $.each(data, function (key, value) {
                    $('#<%=ddlKhayUm.ClientID%>').append($("<option></option>").val(value.ID).html(value.Name));
		            if(count == 1) {$("#<%=hdKhayUm.ClientID%>").val(value.ID);}
		            count=count+1;
                });
            },
            error: function (data) {
            },
            beforeSend: function(xhr) {
                xhr.setRequestHeader("X-OFFICIAL-REQUEST", "TRUE");//Used to mark it as an AJAX Request
            },
            complete: function(XMLHttpRequest, textStatus) {
            }
        });
    }
    
    function getAvailableKhayAp()
    {
        var date = $("#<%=txtNgayVaoAp.ClientID%>").val();
        var idtheodoide = $("#<%=hdIDTDD.ClientID%>").val();
        var jsDataCheck = new DataCheck(1, date, idtheodoide);
        var jsonText = JSON.stringify(jsDataCheck);
        $.ajax({
            type: "POST",
            url: location.href,
            data: jsonText,
            dataType: "json",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            success: function (data) {
                $('#<%=ddlKhayAp.ClientID%>').empty();
		        var count = 1;
                $.each(data, function (key, value) {
                    $('#<%=ddlKhayAp.ClientID%>').append($("<option></option>").val(value.ID).html(value.Name));
		            if(count == 1) {$("#<%=hdKhayAp.ClientID%>").val(value.ID);}
		            count=count+1;
                });
            },
            error: function (data) {
            },
            beforeSend: function(xhr) {
                xhr.setRequestHeader("X-OFFICIAL-REQUEST", "TRUE");//Used to mark it as an AJAX Request
            },
            complete: function(XMLHttpRequest, textStatus) {
            }
        });
    }
    
    function xacdinh(sel)
    {
        $("#<%=hdKhayUm.ClientID%>").val(sel.value);
    }
    function xacdinh1(sel)
    {
        $("#<%=hdKhayAp.ClientID%>").val(sel.value);
    }
</script>
<table>
    <tr>
        <td>Ngày vào ấp</td>
        <td>
            <asp:TextBox ID="txtNgayVaoAp" runat="server" Width="150" TabIndex="1" onblur="getAvailableKhayAp()"/>
            <cc1:calendarextender id="calNgayVaoAp" runat="server" format="dd/MM/yyyy 12:00:00"
            popupbuttonid="txtNgayVaoAp" targetcontrolid="txtNgayVaoAp"></cc1:calendarextender>
        </td>
    </tr>
    <tr>
        <td>
            Chuồng
        </td>
        <td>
            <asp:UpdatePanel ID="udpCaDe" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:DropDownList runat="server" ID="ddlChuong" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddlChuong_SelectedIndexChanged"></asp:DropDownList>
                    Cá đẻ
                    <asp:DropDownList runat="server" ID="ddlCaMe" TabIndex="1"></asp:DropDownList>
                    <asp:Button ID="btnRefreshCaDe" runat="server" OnClick="btnRefreshCaDe_Click" Text="Refresh"/>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>Người thu trứng</td>
        <td>
            <asp:ListBox ID="lstNhanVien" runat="server" SelectionMode="Multiple"></asp:ListBox>
        </td>
    </tr>
    <tr>
        <td>Khay ấp</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlKhayAp" TabIndex="3" onchange="xacdinh1(this)"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Phòng ấp</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlPhongAp" TabIndex="3"></asp:DropDownList>
        </td>
    </tr>
	<tr>
		<td>TL trứng bình quân (g)</td>
		<td><asp:TextBox ID="txtTLTBQ" runat="server" style="width:400px;" TabIndex="4"></asp:TextBox>
        </td>
	</tr>
	<tr>
		<td>Số trứng đẻ</td>
		<td><asp:TextBox ID="txtTrungDe" runat="server" style="width:400px;" TabIndex="5" onblur="CountSLNo();">0</asp:TextBox></td>
	</tr>
	<tr>
		<td>Số trứng vỡ</td>
		<td><asp:TextBox ID="txtTrungVo" runat="server" style="width:400px;" TabIndex="6" CssClass="TrungBo" onblur="CountSLNo();">0</asp:TextBox></td>
	</tr>
	<tr>
	    <td>Số trứng thải loại</td>
	    <td>
	        <asp:GridView ID="grvDanhSach" runat="server" AutoGenerateColumns="False" CssClass="mGrid" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grvDanhSach_RowDataBound" Width="100%" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Lý do thải loại trứng">
                        <ItemTemplate>
                            <asp:Label ID="lblLyDoThaiLoai" runat="server" CssClass="lblLyDoThaiLoai"></asp:Label>
                        </ItemTemplate><ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <b>Tổng thải loại</b>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Số lượng trứng thải loại">
                        <ItemTemplate>
                            <asp:TextBox ID="txtSoLuong" runat="server" CssClass="TrungBo TrungThaiLoai" onblur="CountSLNo();CountThaiLoai();" TabIndex="7"></asp:TextBox>
                            <asp:Button ID="btnSaveThaiLoaiTrung" runat="server" Enabled="false" BorderStyle="None" BackColor="Transparent" TabIndex="7"/>
                        </ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle ForeColor="Yellow"></HeaderStyle>
                        <FooterTemplate>
                            <asp:Label ID="lblTongThaiLoai" runat="server" Font-Bold="true" CssClass="lblTongThaiLoai"></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center"/>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="pgr" />
                <AlternatingRowStyle CssClass="alt" />
            </asp:GridView>
	    </td>
	</tr>
	<tr>
		<td>Số trứng không phôi</td>
		<td><asp:TextBox ID="txtTrungKhongPhoi" runat="server" style="width:400px;" TabIndex="8" CssClass="TrungBo" onblur="CountSLNo();">0</asp:TextBox></td>
	</tr>
	<tr>
		<td>Số trứng chết phôi 1</td>
		<td><asp:TextBox ID="txtTrungChetPhoi1" runat="server" style="width:400px;" TabIndex="9" CssClass="TrungBo" onblur="CountSLNo();">0</asp:TextBox></td>
	</tr>
	<tr>
		<td>Số trứng chết phôi 2</td>
		<td><asp:TextBox ID="txtTrungChetPhoi2" runat="server" style="width:400px;" TabIndex="10" CssClass="TrungBo" onblur="CountSLNo();">0</asp:TextBox></td>
	</tr>
	<tr>
		<td>Còn lại</td>
		<td><asp:TextBox ID="txtConLai" runat="server" style="width:400px;" TabIndex="10" Enabled="false">0</asp:TextBox></td>
	</tr>
	<tr>
        <td>Ngày nở</td>
        <td>
            <asp:TextBox ID="txtNgayNo" runat="server" Width="400" TabIndex="11" onblur="getAvailableKhayUm()"/>
            <cc1:calendarextender id="calNgayNo" runat="server" format="dd/MM/yyyy 12:00:00" popupbuttonid="txtNgayNo" targetcontrolid="txtNgayNo"></cc1:calendarextender>
        </td>
    </tr>
    <tr>
        <td>Khay úm</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlKhayUm" TabIndex="12" EnableViewState="true" onchange="xacdinh(this)"></asp:DropDownList>
        </td>
    </tr>
    <tr>
		<td>TL con bình quân (g)</td>
		<td><asp:TextBox ID="txtTLCBQ" runat="server" style="width:400px;" TabIndex="13"></asp:TextBox></td>
	</tr>
	<tr>
		<td>Chiều dài bình quân (cm)</td>
		<td><asp:TextBox ID="txtCDBQ" runat="server" style="width:400px;" TabIndex="14"></asp:TextBox></td>
	</tr>
	<tr>
		<td>Vòng bụng bình quân (cm)</td>
		<td><asp:TextBox ID="txtVBBQ" runat="server" style="width:400px;" TabIndex="15"></asp:TextBox></td>
	</tr>
	<tr>
		<td>Ghi chú</td>
		<td><asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" style="width:400px;" Rows="3" TabIndex="16"></asp:TextBox></td>
	</tr>
	<tr>
		<td colspan="2" align="center">
            <asp:Button ID="Save" runat="server" OnClick="btnSave_Click" CssClass="button" tabindex="17" Text="Lưu"/>
            <asp:Button ID="Delete" runat="server" OnClick="btnDelete_Click" CssClass="button" tabindex="18" Text="Xóa" OnClientClick="return confirmDelete()"/>
            <asp:HyperLink ID="lnkCancel" runat="server" Font-Bold="true" CssClass="button" tabindex="19">Thoát</asp:HyperLink>
		</td>
	</tr>
</table>
<asp:HiddenField ID="hdIDTDD" runat="server" Value="0"/>
<asp:HiddenField ID="hdSoTtrungNoCu" runat="server" Value="0"/>
<asp:HiddenField ID="hdKhayUm" runat="server" Value="0"/>
<asp:HiddenField ID="hdKhayAp" runat="server" Value="0"/>
<asp:HiddenField ID="hdDaLoadKhayAp" runat="server" Value="0"/>