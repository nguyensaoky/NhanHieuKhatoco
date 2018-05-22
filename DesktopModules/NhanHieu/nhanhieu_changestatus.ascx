<%@ Control Language="C#" AutoEventWireup="true" CodeFile="nhanhieu_changestatus.ascx.cs" Inherits="DotNetNuke.Modules.NhanHieu.nhanhieu_changestatus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="<%= ModulePath + "/style.css" %>" rel="stylesheet" type="text/css" />

<asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" EnableViewState="False"></asp:Label>
<br />
<asp:Label ID="lblDonVi" runat="server" Text="Đơn vị"></asp:Label>
<asp:DropDownList ID="ddlDonVi" runat="server"></asp:DropDownList>
Nội dung tin nhắn <asp:TextBox ID='txtMessage' runat='server'/>
File đính kèm <input id="txtFile" runat="server" name="txtFile" size="30" type="file" width="300"/>
<br />
<asp:Button ID="btnThucHien" runat="server" Text="Thực hiện" OnClick="btnThucHien_Click"></asp:Button>