<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VerticalMenu.ascx.cs" Inherits="DotNetNuke.UI.UserControls.VerticalMenu" %>
<script type="text/javascript">
    function toggleMenu_(objID) 
    {
        if (!document.getElementById) return;
        var ob = document.getElementById(objID).style;
        ob.display = (ob.display == 'block')?'none': 'block';
    }
    
    function toggleMenu(objID, objIcon, plusImg, minusImg) 
    {
        if (!document.getElementById) return;
        var icon = document.getElementById(objIcon);
        var ob = document.getElementById(objID).style;
        ob.display = (ob.display == 'block')?'none': 'block';
        if(ob.display == 'block')
        {
            icon.src = minusImg;
        }
        else if(ob.display == 'none')
        {
            icon.src = plusImg;
        }
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
    function downloadallfile(path)
    {
        var iframe = document.createElement("iframe");
        iframe.src = path;
        iframe.style.display = "none";
        document.body.appendChild(iframe); 
    }
</script>
<div style="width:<%=MenuWidth%>;">
    <asp:Literal ID="ltScript" runat="server"></asp:Literal>
</div>