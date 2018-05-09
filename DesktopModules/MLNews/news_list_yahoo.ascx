<%@ Control Language="C#" AutoEventWireup="true" CodeFile="news_list_yahoo.ascx.cs" Inherits="DotNetNuke.News.news_list_yahoo" %>
<script type="text/javascript" src='<%= ResolveUrl("~/js/NewYahooSlideshow/script.js") %>'></script>
<link rel="stylesheet" type="text/css" href='<%= ResolveUrl("~/js/NewYahooSlideshow/style.css") %>' title="default" />
<asp:Literal ID="ltTitle" runat="server"></asp:Literal>
<asp:Literal ID="ltNewsList" runat="server"></asp:Literal>
<div id="wrapper">
	<div id="fullsize" style="width:<%=leftPartWidth%>px;height:<%=slideHeight%>px;text-align:left;">
		<div id="imgprev" class="imgnav" style="height:<%=slideHeight%>px;" title="Previous Image"></div>
		<div id="imglink" style="height:<%=slideHeight%>px;"></div>
		<div id="imgnext" class="imgnav" style="height:<%=slideHeight%>px;" title="Next Image"></div>
		<div id="image" style="width:<%=leftPartWidth%>px;height:<%=slideHeight%>px;text-align:left;"></div>
		<div id="information" style="width:<%=leftPartWidth%>px;text-align:left;">
			<h3 style="text-align:justify;"></h3>
			<p style="text-align:justify;"></p>
			<a>Chi tiết</a>
		</div>
	</div>
	<div id="thumbnails">
		<div id="slidearea" style="width:<%=rightPartWidth%>px;height:<%=slideHeight%>px;">
			<div id="slider" style="text-align:left;"></div>
		</div>
	</div>
	<div id="slidebar" style="height:<%=slideHeight%>px;">
		<div id="slideleft" title="Slide Left""></div>
		<div id="slideright" title="Slide Right" style="top:<%=scrollDownPosition%>px;"></div>
	</div>
</div>
<script type="text/javascript">
	$('slideshow').style.display='none';
	$('wrapper').style.display='block';
	var slideshow=new TINY.slideshow("slideshow");
	window.onload=function(){
		slideshow.auto=true;
		slideshow.speed=5;
		slideshow.link="linkhover";
		slideshow.info="information";
		slideshow.thumbs="slider";
		slideshow.left="slideleft";
		slideshow.right="slideright";
		slideshow.scrollSpeed=4;
		slideshow.spacing=5;
		slideshow.active="#fff";
		slideshow.init("slideshow","image","imgprev","imgnext","imglink");
	}
</script>