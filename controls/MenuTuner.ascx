<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.UI.UserControls.MenuTuner" %>
<script type="text/javascript" src="<%= ResolveUrl("~/js/MenuTuner/dmenu.js") %>"></script>
<script type="text/javascript">
/*
   Deluxe Menu Data File
   Created by Deluxe Tuner v2.0
   http://deluxe-menu.com
*/


// -- Deluxe Tuner Style Names
var itemStylesNames=[];
var menuStylesNames=[];
// -- End of Deluxe Tuner Style Names

//--- Common
var isHorizontal=0;
var smColumns=1;
var smOrientation=0;
var smViewType=0;
var dmRTL=0;
var pressedItem=-2;
var itemCursor="pointer";
var itemTarget="_self";
var statusString="link";
var blankImage="blank.gif";

//--- Dimensions
var menuWidth="95%";
var menuHeight="22";
var smWidth="";
var smHeight="";

//--- Positioning
var absolutePos=0;
var posX="0";
var posY="0";
var topDX=0;
var topDY=2;
var DX=0;
var DY=2;

//--- Font
var fontStyle="bold 9pt Verdana";
var fontColor=["#FFFFFF","#FFFFFF"];
var fontDecoration=["none","none"];
var fontColorDisabled="#AAAAAA";

//--- Appearance
var menuBackColor="";
var menuBackImage="";
var menuBackRepeat="no-repeat";
var menuBorderColor="";
var menuBorderWidth="";
var menuBorderStyle="none";

//--- Item Appearance
var itemBackColor=["#FFFFFF","#C0C0C0"];
var itemBackImage=["btn_black_blue.gif","btn_green.gif"];
var itemBorderWidth=0;
var itemBorderColor=["#6655FF","#665500"];
var itemBorderStyle=["none","none"];
var itemSpacing=2;
var itemPadding="4px";
var itemAlignTop="left";
var itemAlign="left";
var subMenuAlign="";

//--- Icons
var iconTopWidth=24;
var iconTopHeight=24;
var iconWidth=16;
var iconHeight=16;
var arrowWidth=9;
var arrowHeight=9;
var arrowImageMain=["arr_black.gif",""];
var arrowImageSub=["arrv_black.gif",""];

//--- Separators
var separatorImage="";
var separatorWidth="100%";
var separatorHeight="3";
var separatorAlignment="left";
var separatorVImage="";
var separatorVWidth="3";
var separatorVHeight="100%";
var separatorPadding="0px";

//--- Floatable Menu
var floatable=0;
var floatIterations=6;
var floatableX=1;
var floatableY=1;

//--- Movable Menu
var movable=0;
var moveWidth=12;
var moveHeight=20;
var moveColor="#AA0000";
var moveImage="";
var moveCursor="default";
var smMovable=0;
var closeBtnW=15;
var closeBtnH=15;
var closeBtn="";

//--- Transitional Effects & Filters
var transparency="100";
var transition=24;
var transOptions="";
var transDuration=200;
var transDuration2=200;
var shadowLen=3;
var shadowColor="#777777";
var shadowTop=1;

//--- CSS Support (CSS-based Menu)
var cssStyle=0;
var cssSubmenu="";
var cssItem=["",""];
var cssItemText=["",""];

//--- Advanced
var dmObjectsCheck=0;
var saveNavigationPath=1;
var showByClick=0;
var noWrap=1;
var pathPrefix_img="<%= this.StrImgPath %>";
var pathPrefix_link="";
var smShowPause=100;
var smHidePause=100;
var smSmartScroll=1;
var smHideOnClick=1;
var dm_writeAll=0;

//--- AJAX-like Technology
var dmAJAX=0;
var dmAJAXCount=0;

//--- Dynamic Menu
var dynamic=0;

//--- Keystrokes Support
var keystrokes=0;
var dm_focus=1;
var dm_actKey=113;


<asp:Literal ID="ltScript" runat="server"></asp:Literal>


dm_init();
</script>