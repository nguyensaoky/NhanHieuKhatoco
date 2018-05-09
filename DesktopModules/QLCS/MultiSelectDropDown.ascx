<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MultiSelectDropDown.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.MultiSelectDropDown"%>
<div id="dvTop" runat="server">
	<asp:textbox id="tbm" ReadOnly="True" runat="server" onmouseover="DisplayTitle(this);"></asp:textbox>	
	<IMG id="imgm" class="DDA" onclick="SHMulSel(<%= ControlClientID %>, event)" src="<%=ResolveUrl("~/dropdown_arrow.PNG")%>">
</div>
<div id="divMain" runat="server">
	<asp:repeater id="rp1" runat="server">
		<ItemTemplate>
			<a class="LI" href="#">
			    <div id="DVLI" class="LI" onclick="event.cancelBubble=true;">
				    <input id="cb1" type="checkbox" runat="server" value='<%# DataBinder.Eval(Container, dataItem) %>' NAME="cb1">
				    <asp:Literal ID="lt1" Runat="server"></asp:Literal>
			    </div>
			</a>
		</ItemTemplate>
	</asp:repeater>
</div>
<input id="hapb" type="hidden" name="tempHiddenField" runat="server"> 
<input id="hsiv" type="hidden" name="hsiv" runat="server">
<input id="__ET1" type="hidden" name="__ET1" runat="server">
<input id="__EA1" type="hidden" name="__EA1" runat="server">
