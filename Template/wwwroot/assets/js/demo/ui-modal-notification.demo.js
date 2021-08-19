/*
Template Name: ASPSTUDIO - Responsive Bootstrap 4 Outlet Template
Version: 1.1.0
Author: Sean Ngu
Website: http://www.seantheme.com/asp-studio/
*/

var handleToastsInit = function() {
	$('[data-init="toast"]').toast('show');
};

var handleToastToggle = function() {
	$(document).on('click', '[data-toggle="toast"]', function(e) {
		e.preventDefault();
		
		var targetElm = $(this).attr('data-target');
		$(targetElm).toast('show');
	});
};


/* Controller
------------------------------------------------ */
$(document).ready(function() {
	handleToastsInit();
	handleToastToggle();
});