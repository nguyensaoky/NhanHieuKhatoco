<%@ Control Language="c#" Codebehind="~/Admin/Skins/Skin.cs" AutoEventWireup="true" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="CURRENTDATE" Src="~/Admin/Skins/CurrentDate.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SOLPARTMENU" Src="~/Admin/Skins/SolPartMenu.ascx" %>
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
<table id="wrapper">
	<tr>
		<td rowspan="3" class="left_side">
		&nbsp;
		</td>
		<td class="top">
		</td>
		<td rowspan="3" class="right_side">
		&nbsp;
		</td>
	</tr>
	<tr>			
		<td class="menu">
		</td>			
	</tr>
	<tr>
		<td>
			<table class="mtable">
				<tr>
					<td class="mtable_lr" rowspan="8">
					</td>
					<td class="mtable_sp"> 
					</td >
					<td>
					</td>
					<td class="mtable_lr"  rowspan="8">
					</td>
				</tr>
				<tr>						
					<td class="mtable_gt">
					</td>
					<td rowspan="5" class="mtable_p">
					</td>						
				</tr>
				<tr>						
					<td class="mtable_sp">
					</td>
				</tr>
				<tr>
					<td class="mtable_login" runat="server" id="ContentPane">
					</td>
				</tr>
				<tr>
					<td class="mtable_sp">
					</td>
				</tr>
				<tr>
					<td class="mtable_d">
					</td>
				</tr>	
				<tr>						
					<td colspan="2">							
					</td>						
				</tr>						
				<tr>						
					<td colspan="2" class="copyright">	
					</td>						
				</tr>											
			</table>				
		</td>
	</tr>
</table>

