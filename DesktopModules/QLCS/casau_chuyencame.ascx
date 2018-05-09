<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_chuyencame.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_chuyencame" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<div id="wrapper" runat="server" class="wrapper_margin">
    <div style="width:200px;float:left;">Chọn cá mẹ mới</div>
    <asp:DropDownList runat="server" ID="ddlCaMe">
    </asp:DropDownList>
    <br /><br />
    <div style="width:200px;float:left;">&nbsp;</div>
    <asp:Button ID="btnChuyen" runat="server" Text="Chuyển" OnClick="btnChuyen_Click" CssClass="button"/>
</div>