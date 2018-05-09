/* This script and many more are available free online at
The JavaScript Source!! http://javascript.internet.com
Created by: Sandeep Gangadharan | http://www.sivamdesign.com/scripts/ */

var exp = 5;

function newCookie(name,value,days) {
  if (days) {
    var date = new Date();
    date.setTime(date.getTime()+(days*24*60*60*1000));
    var expires = "; expires="+date.toGMTString();
  } else var expires = "";
  document.cookie = name+"="+value+expires+"; path=/";
}

function readCookie(name) {
  var nameSG = name + "=";
  var nuller = '';
  if (document.cookie.indexOf(nameSG) == -1)
  return nuller;
  var ca = document.cookie.split(';');
  for(var i=0; i<ca.length; i++) {
    var c = ca[i];
    while (c.charAt(0)==' ') c = c.substring(1,c.length);
    if (c.indexOf(nameSG) == 0) return c.substring(nameSG.length,c.length);
  }
  return null;
}

function eraseCookie(name) {
  newCookie(name,"",-1);
}

if (document.getElementById) {
  document.writeln('<style type="text/css">')
  document.writeln('.main {text-decoration:none; color:blue; cursor:hand; cursor:pointer}')
  document.writeln('span:hover.mainLink {text-decoration:underline; color:red}')
  document.writeln('.sublinks1 {display:none; padding-left:14px}')
  document.writeln('.link2 {text-decoration:none; color:blue}')
  document.writeln('a:hover.link2 {text-decoration:underline; color:red}')
  document.writeln('</style>') }

    // Below you should add a1, a2 etc. for each main link you wish to include
    // so if you want 3 main links you should add a1, a2, a3 in the format shown
    // enclosed in double quotes
  var mainNum = new Array("a1","a2","a3");

    // Below you should add b1, b2 etc. for each sub link you wish to include
    // under one main link, here the first main link. so if you want 4 sub links you
    // should add b1, b2, b3, b4 in the format shown enclosed in double quotes
  var subNum1 = new Array("b1","b2");

    // Below, this is for sub links under the second main link. there are 3 sub links
    // in the given example
  var subNum2 = new Array("c1","c2","c3");
 

function openClose(theName, menuArray, theID) {
  for(var i=0; i < menuArray.length; i++) {
    if (menuArray[i] == theID) {
      if (document.getElementById(theID).style.display == "block") {
        document.getElementById(theID).style.display = "none";
        document.getElementById("tick_"+menuArray[i]).innerHTML = "+";
        eraseCookie(theName); }
      else {
        document.getElementById(theID).style.display = "block";
        document.getElementById("tick_"+menuArray[i]).innerHTML = "-";
        newCookie(theName,menuArray[i],exp); }
      }
    else {
      document.getElementById(menuArray[i]).style.display = "none";
      document.getElementById("tick_"+menuArray[i]).innerHTML = "+";
    }
  }
}

function memStatus() {
  var num = readCookie("MN");
  if (num) {
    document.getElementById(num).style.display = "block";
    document.getElementById("tick_"+num).innerHTML = "-"; }
  var num1 = readCookie("SB");
  if (num1) {
    document.getElementById(num1).style.display = "block";
    document.getElementById("tick_"+num1).innerHTML = "-"; }
}

// Multiple onload function created by: Simon Willison
// http://simonwillison.net/2004/May/26/addLoadEvent/
function addLoadEvent(func) {
  var oldonload = window.onload;
  if (typeof window.onload != 'function') {
    window.onload = func;
  } else {
    window.onload = function() {
      if (oldonload) {
        oldonload();
      }
      func();
    }
  }
}

addLoadEvent(function() {
  memStatus();
});

