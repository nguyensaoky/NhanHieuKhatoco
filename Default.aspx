<%@ Page EnableViewStateMac="true" Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.Framework.DefaultPage" CodeFile="Default.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Common.Controls" Assembly="DotNetNuke" %>
<asp:Literal id="skinDocType" runat="server"></asp:Literal>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta id="MetaRefresh" runat="server" name="Refresh" content="" />
    <meta id="MetaDescription" runat="server" name="DESCRIPTION" content="" />
    <meta id="MetaKeywords" runat="server" name="KEYWORDS" content="" />
    <meta http-equiv="PAGE-ENTER" content="RevealTrans(Duration=0,Transition=1)"/>
    <style type="text/css" id="StylePlaceholder" runat="server"></style>
    <asp:placeholder id="CSS" runat="server"></asp:placeholder>
</head>
<body id="Body" runat="server" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" onkeypress="KeyPress(event)">
    <noscript>
        <div style="margin:10px;font-family:Tahoma;font-size:13px;">
            Xin chào, Bạn không thể sử dụng chương trình này vì trình duyệt của bạn hiện đang <span style="color:Red;">tắt Javascript</span>.
            <br />Bạn hãy <span style="color:Blue;">bật Javascript</span> lên rồi refresh lại trang.
            <br />Nếu không biết cách bật Javascript lên bạn có thể làm theo hướng dẫn sau đây:
            <br />
            <br />
            <b>Đối với trình duyệt Internet Explorer, làm từng bước như sau:</b>
            <br />1. Vào menu Tools > Internet Options.
            <br />2. Chọn tab Security.
            <br />3. Chọn Internet.
            <br />4. Chọn Custom Level.
            <br />5. Kéo xuống mục Scripting (ở gần cuối).
            <br />6. Chọn Enable ở phần Active Scripting > OK.
            <br />7. Bấm Yes > OK > F5 (refresh lại trang).
            <br />
            <br />
            <b>Đối với trình duyệt Firefox, làm từng bước như sau:</b>
            <br />1. Vào menu Tools > Options.
            <br />2. Chọn tab Content.
            <br />3. Check chọn Enable Javascript.
            <br />4. OK > F5 (refresh lại trang).
            <br />
            <br />
            <b>Đối với trình duyệt Opera, làm từng bước như sau:</b>
            <br />1. Vào menu Tools > Preferences...
            <br />2. Advanced.
            <br />3. Content.
            <br />4. Enable Javascript .
            <br />5. OK > F5 (refresh lại trang). 
            <br />
            <br />
            <b>Nếu vẫn không có hiệu quả, bạn có thể liên hệ với địa chỉ mail <a href="mailto:nguyensaoky@khatoco.com">nguyensaoky@khatoco.com</a> để được hỗ trợ.</b>
        </div>
    </noscript>
    <dnn:Form id="Form" runat="server" ENCTYPE="multipart/form-data" style="height:100%;display:none;">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <cc1:alwaysvisiblecontrolextender id="avceShowWaiting" runat="server" targetcontrolid="udpWaiting" HorizontalSide="Center" HorizontalOffset="0" VerticalSide="Middle" VerticalOffset="0" ScrollEffectDuration=".1"></cc1:alwaysvisiblecontrolextender>
        <asp:UpdateProgress ID="udpWaiting" runat="server">
            <ProgressTemplate>
                <img alt="ĐANG XỬ LÝ..." src="<%=DotNetNuke.Common.Globals.ApplicationPath + "/images/loading.gif" %>" width="40"/>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:Label ID="SkinError" runat="server" CssClass="NormalRed" Visible="False"></asp:Label>
        <asp:PlaceHolder ID="SkinPlaceHolder" runat="server"/>
        <input id="ScrollTop" runat="server" name="ScrollTop" type="hidden"/>
        <input id="__dnnVariable" runat="server" name="__dnnVariable" type="hidden"/>
    </dnn:Form>
    <script type="text/javascript" language="javascript">
        var f = document.forms[0];
        f.style.display = '';
    </script>
</body>
</html>
