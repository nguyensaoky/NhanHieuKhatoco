<%@ Control Language="c#" Codebehind="~/Admin/Skins/Skin.cs" AutoEventWireup="true" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="CURRENTDATE" Src="~/Admin/Skins/CurrentDate.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="MENU" Src="~/Admin/Skins/superfishmenu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LINKS" Src="~/Admin/Skins/Links.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<script runat="server">
    protected void Page_Load( Object sender, EventArgs e )
    {
       Control skinDocType = this.Page.FindControl("skinDocType");
       if (skinDocType != null)
           ((System.Web.UI.WebControls.Literal)skinDocType).Text = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">";
    }    
</script>
<link rel="stylesheet" href="<%=SkinPath %>css/style.css" type="text/css" media="screen" />
<link rel="stylesheet" href="<%=SkinPath %>css/superfish.css" media="screen">
<script type="text/javascript" src="<%=SkinPath %>js/jquery.js"></script>
<script type="text/javascript" src="<%=SkinPath %>js/jquery-1.7.1.min.js"></script>
<script type="text/javascript" src="<%=SkinPath %>js/superfish.js"></script>
<script type="text/javascript" language="javascript">
	jQuery(function($) {
		$('#menu').superfish({
		});
	});
</script>
<table class="mtable_2">
	<tr>			
		<td class="top_2">
			<div class="banner_2"></div>				
		</td>			
	</tr>
	<tr>
	    <td>
	        <div class="menu_2">
	            <table class="table_menu_2" cellpadding="0" cellspacing="0">
		            <tr>
			            <td class="menu_2_left">&nbsp;	
			            </td>
			            <td class="menu_2_center">
                            <div class="menutop">
                                <ul class="sf-menu" id="menu">
                                    <dnn:MENU ID="Menu1" runat="server"/>
                                </ul>
                            </div>
			            </td>
			            <td class="menu_2_center_1">
                            <dnn:USER runat="server" id="dnnUSER" CssClass="user" />&nbsp;&bull;&nbsp;
                            <dnn:LOGIN runat="server" id="dnnLOGIN" CssClass="user" />
                        </td>
			            <td class="menu_2_right">&nbsp;	
			            </td>
		            </tr>
	            </table>
            </div>	
	    </td>
	</tr>
	<tr>			
		<td runat="server" id="ContentPane" valign="top" style="padding-top:15px;">
		</td>			
	</tr>							
	<tr>
		<td class="copyright">	
		&nbsp;
		</td>
	</tr>
</table>

