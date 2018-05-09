<%@ Control Language="c#" Codebehind="~/Admin/Skins/Skin.cs" AutoEventWireup="true" Inherits="DotNetNuke.UI.Skins.Skin" %>
<script runat="server">
    protected void Page_Load( Object sender, EventArgs e )
    {
       Control skinDocType = this.Page.FindControl("skinDocType");
       if (skinDocType != null)
           ((System.Web.UI.WebControls.Literal)skinDocType).Text = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">";
    }    
</script>
<div style="width:100%;text-align:center;" id="ContentPane" runat="server"></div>