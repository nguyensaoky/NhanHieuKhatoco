<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Controls.TransMenu"
    CodeFile="transmenu.ascx.vb" %>
<!-- Begin menu -->
<div id="transmenu">
    <link href="<%=AppPath%>/admin/skins/transmenu/transmenu.css" type="text/css"
        rel="stylesheet" xmlns:fo="http://www.w3.org/1999/XSL/Format">

    <script language="javascript" src="<%=AppPath%>/admin/skins/transmenu/transmenu.js"
        xmlns:fo="http://www.w3.org/1999/XSL/Format"></script>

    <div id="wrap">
        <div id="menu">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <%=MenuRootItems%>
                </tr>
            </table>
        </div>
    </div>

    <script language="javascript" type="text/javascript">
		if (TransMenu.isSupported())
		{
			//TransMenu.updateImgPath('http://<%=Request.ServerVariables("SERVER_NAME")%><%=Request.ApplicationPath%>/admin/skins/transmenu');
			TransMenu.updateImgPath('http://<%=Request.ServerVariables("SERVER_NAME")%><%=AppPath%>/admin/skins/transmenu');
	 		TransMenu.subpad_x = 0;
			TransMenu.subpad_y = 0;
	  		var ms = new TransMenuSet(TransMenu.direction.down, 0, 0, TransMenu.reference.bottomLeft);

            <%=MenuJavaScript%>
            TransMenu.renderAll();
		}
		AttachEvent=function(){TransMenu.initialize();}

		if (window.attachEvent)
		{
			window.attachEvent("onload", AttachEvent);
		}
		else
		{
			TransMenu.initialize();
		}
    </script>

</div>
