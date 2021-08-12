/*
Template Name: ASPSTUDIO - Responsive Bootstrap 4 Outlet Template
Version: 1.1.0
Author: Sean Ngu
Website: http://www.seantheme.com/asp-studio/
*/

var handleRenderSummernote = function() {
	var totalHeight = ($(window).width() >= 767) ? $(window).height() - $('.summernote').offset().top - 63 : 400;
	$('.summernote').summernote({
		height: totalHeight
	});
};


/* Controller
------------------------------------------------ */
$(document).ready(function() {
	handleRenderSummernote();
});