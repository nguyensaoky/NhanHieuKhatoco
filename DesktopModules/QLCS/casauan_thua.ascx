<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casauan_thua.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casauan_thua" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript" src="<%= ModulePath + "jquery.js"%>"></script>
<script type="text/javascript" language="javascript">
	function addChuong()
	{
	    var numListChuong = <%=hdNumListChuong.Value%>;
	    var lstChuongIDTemplate = document.getElementById('<%=phChuong.FindControl("lstChuong0").ClientID%>').id;
	    for(i = 0; i< numListChuong; i++)
	    {
	        var newid = "lstChuong" + i.toString();
	        var lstChuongID = lstChuongIDTemplate.replace("lstChuong0",newid);
	        var x=document.getElementById(lstChuongID);
            for (var j = 0; j < x.options.length; j++) 
            {
                if(x.options[j].selected && $("#chuong" + x.options[j].value).length == 0)
                {
                    $("#ChuongDetails").append("<div id='div" + x.options[j].value + "' style='margin-bottom:5px;'><div style='width:100px;float:left;text-align:left;'>" + x.options[j].text + "</div><input type=text style='float:left;' id='chuong" + x.options[j].value + "' name='chuong" + x.options[j].value + "'/><div style='float:left;text-align:left;'>&nbsp;kg&nbsp;</div><a class='button' style='float:left;cursor:pointer;' onclick='$(this).parent().remove();'>Xóa</a><div style='clear:both;'></div></div>");
                }
            }
	    }
	}
	
	function bindChuong()
	{
	    $("#ChuongDetails").empty();
	    
	    var ListChuong = '<%=hdListChuong.Value%>';
	    var ListTenChuong = '<%=hdListTenChuong.Value%>';
	    var ListThua = '<%=hdListThua.Value%>';
	    var aChuong = ListChuong.split(";");
	    var aTenChuong = ListTenChuong.split(";");
	    var aThua = ListThua.split(";");
	    
	    $.each( aChuong, function( i, val ) {
            $("#ChuongDetails").append("<div id='div" + val + "' style='margin-bottom:5px;'><div style='width:100px;float:left;text-align:left;'>" + aTenChuong[i] + "</div><input type=text style='float:left;' id='chuong" + val + "' name='chuong" + val + "' value='" + aThua[i] + "'/><div style='float:left;text-align:left;'>&nbsp;kg&nbsp;</div><a class='button' style='float:left;cursor:pointer;' onclick='$(this).parent().remove();'>Xóa</a><div style='clear:both;'></div></div>");
        });
	}
</script>
Xem từ ngày
<asp:TextBox ID="txtTuNgay" runat="server" Width="70" TabIndex="6"/>
<cc1:calendarextender id="calTuNgay" runat="server" format="dd/MM/yyyy" popupbuttonid="txtTuNgay" targetcontrolid="txtTuNgay"></cc1:calendarextender>
đến ngày
<asp:TextBox ID="txtDenNgay" runat="server" Width="70" TabIndex="6"/>
<cc1:calendarextender id="calDenNgay" runat="server" format="dd/MM/yyyy" popupbuttonid="txtDenNgay" targetcontrolid="txtDenNgay"></cc1:calendarextender>            
<asp:Button ID="btnLoad" runat="server" Text="Tải" OnClick="btnLoad_Click" CssClass="button"/>
<br />
<br />
Danh sách ngày có thức ăn thừa <asp:DropDownList ID="ddlNgayThucAnThua" runat="server"></asp:DropDownList>
<asp:Button ID="btnEdit" runat="server" Text="Xem/Sửa" OnClick="btnEdit_Click" CssClass="button"/>
<asp:Button ID="btnDelete" runat="server" Text="Xóa" OnClick="btnDelete_Click" CssClass="button"/>
<br /><br /><br /><br />
<asp:Panel ID="pnlDetails" runat="server">
    Ngày có thức ăn thừa
    <asp:TextBox ID="txtNgayAn" runat="server" Width="70" TabIndex="6"/>
    <cc1:calendarextender id="alNgayAn" runat="server" format="dd/MM/yyyy" popupbuttonid="txtNgayAn" targetcontrolid="txtNgayAn"></cc1:calendarextender>
    <br /><br />
    Ô chuồng (Ctrl để chọn nhiều)<br />
    <asp:PlaceHolder ID="phChuong" runat="server"></asp:PlaceHolder>
    <asp:HyperLink ID="lnkAddChuong" runat="server" onclick="addChuong();" CssClass="button">Thêm chuồng</asp:HyperLink>
    <br /><br />
    <div id="ChuongDetails"></div>
    <br />
    <asp:Button ID="btnSave" runat="server" Text="Lưu" OnClick="btnSave_Click" CssClass="button"/>
</asp:Panel>
<asp:HiddenField ID="hdNumListChuong" runat="server" Value="0"/>
<asp:HiddenField ID="hdListChuong" runat="server" Value=""/>
<asp:HiddenField ID="hdListTenChuong" runat="server" Value=""/>
<asp:HiddenField ID="hdListThua" runat="server" Value=""/>