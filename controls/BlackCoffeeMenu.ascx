<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.UI.UserControls.BlackCoffeeMenu" %>
<script type="text/javascript">
    function toggleMenu(objID) 
    {
        if (!document.getElementById) return;
        var ob = document.getElementById(objID).style;
        ob.display = (ob.display == 'block')?'none': 'block';
    }
    function expandMenu(objID) 
    {
        if (!document.getElementById) return;
        var ob = document.getElementById(objID).style;
        ob.display = 'block';
    }
    function collapseMenu(objID) 
    {
        if (!document.getElementById) return;
        var ob = document.getElementById(objID).style;
        ob.display = 'none';
    }
</script>
<div style="width:<%=MenuWidth%>px;">
    <asp:Literal ID="ltScript" runat="server"></asp:Literal>
</div>