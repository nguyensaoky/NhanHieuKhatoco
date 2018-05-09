$(document).ready(function () {
    var $map, default_options;
    $map = $('#map');

	function buildToolTipArea(name, content) {
        return $('<div>' + name + '/' + content + '</div>');
    }

    function buildAreas() {
        var items = $('#RealMap').find('area');
        var areaArray = [];

        items.each(function () {
            var name = $(this).attr('alt');
            var content = $(this).attr('content');
            areaArray.push({ key: name, toolTip: buildToolTipArea(name, content) });
        });
        return areaArray;
    }

    default_options =
	{
		fillOpacity: 0.5,
		render_highlight: {
			fillColor: '22ff00',
			stroke: true
		},
		render_select: {
			fillColor: 'ff000c',
			stroke: false
		},
		fadeInterval: 50,
		isSelectable: false,
		mapKey: 'alt',
		mapValue: 'content',
		showToolTip: true,
		toolTipClose: ["tooltip-click", "area-click", "area-mouseout"],
		areas: buildAreas()
	};

    $map.mapster(default_options);
	$map.mapster('resize', window.innerWidth - 20, 0, 1000);
});