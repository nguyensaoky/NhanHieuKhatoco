var lastRan = -1;

/**
 * Since carousel.addItem uses an HTML string to create the interface
 * for each carousel item, this method formats the HTML for an LI.
 **/

var fmtItem = function(imgUrl, url, title) {

  	var innerHTML = 
  		'<a href="' + 
  		url + 
  		'"><img src="' + 
  		imgUrl +
		'" width="' +
		75 +
		'" height="' +
		75+
		'"/>' + 
  		title + 
  		'<\/a>';
  
	return innerHTML;
	
};

/**
 * Custom inital load handler. Called when the carousel loads the initial
 * set of data items. Specified to the carousel as the configuration
 * parameter: loadInitHandler
 **/
var loadInitialItems = function(type, args) {
	var start = args[0];
	var last = args[1]; 
	load(this, start, last);	
};

/**
 * Custom load next handler. Called when the carousel loads the next
 * set of data items. Specified to the carousel as the configuration
 * parameter: loadNextHandler
 **/
var loadNextItems = function(type, args) {	

	var start = args[0];
	var last = args[1]; 
	var alreadyCached = args[2];
	
	if(!alreadyCached) {
		load(this, start, last);
	}
};

/**
 * Custom load previous handler. Called when the carousel loads the previous
 * set of data items. Specified to the carousel as the configuration
 * parameter: loadPrevHandler
 **/
var loadPrevItems = function(type, args) {
	var start = args[0];
	var last = args[1]; 
	var alreadyCached = args[2];
	
	if(!alreadyCached) {
		load(this, start, last);
	}
};

var load = function(carousel, start, last) {
	for(var i=start;i<=last;i++) {
		var randomIndex = getRandom(7, lastRan);
		lastRan = randomIndex;
		carousel.addItem(i, fmtItem(imageList[randomIndex], urlList[randomIndex], "Number " + i));
/*
		// Example of an alternate way to add an item (passing an element instead of html string)
		var p = document.createElement("P");
		var t = document.createTextNode("Item"+i);
		p.appendChild(t);
		carousel.addItem(i, p );
*/
	}
};

var getRandom = function(max, last) {
	var randomIndex;
	do {
		randomIndex = Math.floor(Math.random()*max);
	} while(randomIndex == last);
	
	return randomIndex;
};