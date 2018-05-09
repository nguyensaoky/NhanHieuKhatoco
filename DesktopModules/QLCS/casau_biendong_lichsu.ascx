<%@ Control Language="C#" AutoEventWireup="true" CodeFile="casau_biendong_lichsu.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.casau_biendong_lichsu" %>
<link href="<%= ModulePath + "style.css" %>" rel="stylesheet" type="text/css" />
<div class="lichsubiendong">
    <b>LỊCH SỬ THAY ĐỔI BIẾN ĐỘNG NÀY</b><br/>
    <b>LOẠI BIẾN ĐỘNG:&nbsp;</b><asp:Label ID="lblLBD" runat="server"></asp:Label><br/>
    <asp:Literal ID="lt" runat="server"></asp:Literal>
</div>