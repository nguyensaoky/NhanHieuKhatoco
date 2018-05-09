var FadeDurationMS=1000;
function SetOpacity(object,opacityPct)
{
    object.style.filter = 'alpha(opacity=' + opacityPct + ')';
    object.style.MozOpacity = opacityPct/100;
    object.style.opacity = opacityPct/100;
}
function ChangeOpacity(id,msDuration,msStart,fromO,toO)
{
    var element=document.getElementById(id);
    var msNow = (new Date()).getTime();
    var opacity = fromO + (toO - fromO) * (msNow - msStart) / msDuration;
    if (opacity>=100)
    {
        SetOpacity(element,100);
        element.timer = undefined;
    }
    else if (opacity<=0)
    {
        SetOpacity(element,0);
        element.timer = undefined;
    }
    else 
    {
        SetOpacity(element,opacity);
        element.timer = window.setTimeout("ChangeOpacity('" + id + "'," + msDuration + "," + msStart + "," + fromO + "," + toO + ")",10);
    }
}
function FadeInImage(foregroundID,linkID,newImage,backgroundID)
{
    var foreground=document.getElementById(foregroundID);
    var link=document.getElementById(linkID);
    if (foreground.timer) window.clearTimeout(foreground.timer);
    if (link.timer) window.clearTimeout(link.timer);
    
    var Separator = newImage.indexOf("@");
    var image = newImage.substring(0,Separator);
    var linkURL = newImage.substring(Separator+1);
    
    if (backgroundID)
    {
        var background=document.getElementById(backgroundID);
        if (background)
        {
            if (background.src)
            {
                foreground.src = background.src; 
                SetOpacity(foreground,100);
            }
            background.src = image;
            link.href = linkURL;
			link.target='blank';
            background.style.backgroundImage = 'url(' + image + ')';
            background.style.backgroundRepeat = 'no-repeat';
            var startMS = (new Date()).getTime();
            foreground.timer = window.setTimeout("ChangeOpacity('" + foregroundID + "'," + FadeDurationMS + "," + startMS + ",100,0)",10);
        }
    } 
    else 
    {
        foreground.src = image;
        link.href = linkURL;
    }
}
function RunSlideShow(pictureID,linkID,backgroundID,imageFiles,displaySecs)
{
    var imageSeparator = imageFiles.indexOf(";");
	if(imageSeparator > -1)
	{
		var nextImage = imageFiles.substring(0,imageSeparator);
		FadeInImage(pictureID,linkID,nextImage,backgroundID);
		var futureImages = imageFiles.substring(imageSeparator+1,imageFiles.length)+ ';' + nextImage;
		setTimeout("RunSlideShow('"+pictureID+"','"+linkID+"','"+backgroundID+"','"+futureImages+"',"+displaySecs+")",displaySecs*1000);	
	}
	else
	{
		FadeInImage(pictureID,linkID,imageFiles,backgroundID);
		setTimeout("RunSlideShow('"+pictureID+"','"+linkID+"','"+backgroundID+"','"+imageFiles+"',"+displaySecs+")",displaySecs*1000);	
	}
}