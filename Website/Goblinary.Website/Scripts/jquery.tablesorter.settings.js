$(function () {
	if ($(".custom-popup").length) {
		var $table = $(".custom-popup").tablesorter({
			theme: 'blue',
			widgets: ["zebra", "filter", "columnSelector", "stickyHeaders"],
			widgetOptions: {

				filter_external: '.search',
				filter_columnFilters: true,
				filter_placeholder: { search: 'Filter...' },
				filter_saveFilters: false,
				filter_reset: '.reset',
				columnSelector_container: $('#columnSelector'),
				// column status, true = display, false = hide, disable = do not display on list
				columnSelector_columns: {
					0: 'disable' /* set to disabled; not allowed to unselect it */
				},
				columnSelector_saveColumns: false,
				columnSelector_layout: '<label><input type="checkbox">{name}</label>',
				columnSelector_mediaquery: false,
				columnSelector_cssChecked: 'checked' // class name added to checked checkboxes
			}
		});
	}
});