<%@ Control Language="C#" AutoEventWireup="true" CodeFile="URLControl_Small_Unzip.ascx.cs" Inherits="DotNetNuke.UI.UserControls.UrlControl_Small_Unzip" %>
<asp:Label ID="lblFileName" runat="server" CssClass="Normal" Visible="true"></asp:Label><br />
<input id="txtFile" runat="server" name="txtFile" size="30" type="file" width="300"/>
<asp:LinkButton ID="cmdSave" runat="server" CausesValidation="False" CssClass="CommandButton" resourcekey="Save">Save</asp:LinkButton>
<asp:Label ID="lblUnzipFileIDs" runat="server" CssClass="Normal" Visible="false"></asp:Label>
<asp:Label ID="lblUnzipFileNames" runat="server" CssClass="Normal" Visible="false"></asp:Label>
<asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" EnableViewState="False"></asp:Label>
