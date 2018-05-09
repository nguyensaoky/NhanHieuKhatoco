<%@ Control Language="C#" AutoEventWireup="true" CodeFile="URLControl_Small.ascx.cs" Inherits="DotNetNuke.UI.UserControls.UrlControl_Small" %>
<asp:Label ID="lblFileName" runat="server" CssClass="Normal" Visible="true"></asp:Label><br />
<asp:Label ID="lblFileID" runat="server" CssClass="Normal" Visible="false"></asp:Label>
<input id="txtFile" runat="server" name="txtFile" size="30" type="file" width="300"/>*
<asp:LinkButton ID="cmdSave" runat="server" CausesValidation="False" CssClass="CommandButton" resourcekey="Save">Save</asp:LinkButton>
<asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" EnableViewState="False"></asp:Label>
