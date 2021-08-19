/*
Template Name: ASPSTUDIO - Responsive Bootstrap 4 Outlet Template
Version: 1.1.0
Author: Sean Ngu
Website: http://www.seantheme.com/asp-studio/
	----------------------------
		APPS CONTENT TABLE
	----------------------------

	<!-- ======== GLOBAL SCRIPT SETTING ======== -->
	01. Global Variable
	02. Handle Scrollbar
	03. Handle Header Search Bar
	04. Handle Sidebar Menu
	05. Handle Sidebar Minify
	06. Handle Sidebar Minify Float Menu
	07. Handle Dropdown Close Option
	08. Handle Panel - Remove / Reload / Collapse / Expand
	09. Handle Tooltip & Popover Activation
	10. Handle Scroll to Top Button Activation
	11. Handle hexToRgba
	12. Handle Scroll to
	
	<!-- ======== APPLICATION SETTING ======== -->
	Application Controller
*/



/* 01. Global Variable
------------------------------------------------ */
var FONT_FAMILY    = '-apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,"Noto Sans",sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol","Noto Color Emoji"';
var COLOR_BLUE     = '#1f6bff';
var COLOR_GREEN    = '#1abd36';
var COLOR_ORANGE   = '#ff9500';
var COLOR_RED      = '#ff3b30';
var COLOR_AQUA     = '#30beff';
var COLOR_PURPLE   = '#5b2e91';
var COLOR_YELLOW   = '#ffcc00';
var COLOR_INDIGO   = '#640df3';
var COLOR_PINK     = '#ff2d55';
var COLOR_TEAL     = '#0cd096';
var COLOR_BLACK    = '#000000';
var COLOR_WHITE    = '#FFFFFF';
var COLOR_GRAY_100 = '#ebeef4';
var COLOR_GRAY_200 = '#dae0ec';
var COLOR_GRAY_300 = '#c9d2e3';
var COLOR_GRAY_400 = '#a8b6d1';
var COLOR_GRAY_500 = '#869ac0';
var COLOR_GRAY_600 = '#657eae';
var COLOR_GRAY_700 = '#4d6593';
var COLOR_GRAY_800 = '#3c4e71';
var COLOR_GRAY_900 = '#212837';


/* 02. Handle Scrollbar
------------------------------------------------ */
var handleSlimScroll = function() {
	"use strict";
	$.when($('[data-scrollbar=true]').each( function() {
		generateSlimScroll($(this));
	})).done(function() {
		$('[data-scrollbar="true"]').mouseover();
	});
};
var generateSlimScroll = function(element) {
	var isMobile = (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent));
	
	if ($(element).attr('data-init') || (isMobile && $(element).attr('data-skip-mobile'))) {
		return;
	}
	var dataHeight = $(element).attr('data-height');
		dataHeight = (!dataHeight) ? $(element).height() : dataHeight;

	var scrollBarOption = {
		height: dataHeight, 
		alwaysVisible: false
	};
	if(isMobile) {
		$(element).css('height', dataHeight);
		$(element).css('overflow-x','scroll');
	} else {
		$(element).slimScroll(scrollBarOption);
		$(element).closest('.slimScrollDiv').find('.slimScrollBar').hide();
	}
	$(element).attr('data-init', true);
};


/* 04. Handle Sidebar Menu
------------------------------------------------ */
var handleSidebarMenu = function() {
	"use strict";
	$(document).on('click', '.app-sidebar .menu > .menu-item.has-sub > .menu-link', function(e) {
		e.preventDefault();
		
		var target = $(this).next('.menu-submenu');
		var otherMenu = $('.app-sidebar .menu > .menu-item.has-sub > .menu-submenu').not(target);

		if ($('.app-sidebar-minified').length === 0) {
			$(otherMenu).slideUp(250);
			$(otherMenu).closest('.menu-item').removeClass('expand');
			
			$(target).slideToggle(250);
			var targetElm = $(target).closest('.menu-item');
			if ($(targetElm).hasClass('expand')) {
				$(targetElm).removeClass('expand');
			} else {
				$(targetElm).addClass('expand');
			}
		}
	});
	$(document).on('click', '.app-sidebar .menu > .menu-item.has-sub .menu-submenu .menu-item.has-sub > .menu-link', function(e) {
		e.preventDefault();
		
		if ($('.app-sidebar-minified').length === 0) {
			var target = $(this).next('.menu-submenu');
			$(target).slideToggle(250);
		}
	});
};


/* 05. Handle Sidebar Minify
------------------------------------------------ */
var MOBILE_SIDEBAR_TOGGLE_CLASS = 'app-sidebar-mobile-toggled';
var MOBILE_SIDEBAR_CLOSED_CLASS = 'app-sidebar-mobile-closed';
var handleSidebarMinify = function() {
	$('[data-toggle="sidebar-minify"]').on('click', function(e) {
		e.preventDefault();
		
		var targetElm = '#app';
		var targetClass = 'app-sidebar-minified';
		
		if ($(targetElm).hasClass(targetClass)) {
			$(targetElm).removeClass(targetClass);
			localStorage.removeItem('appSidebarMinified');
		} else {
			$(targetElm).addClass(targetClass);
			localStorage.setItem('appSidebarMinified', true);
		}
	});
	
	if (typeof(Storage) !== 'undefined') {
		if (localStorage.appSidebarMinified) {
			$('#app').addClass('app-sidebar-minified');
		}
	}
};
var handleSidebarMobileToggle = function() {
	$(document).on('click', '[data-toggle="sidebar-mobile"]', function(e) {
		e.preventDefault();
		
		var targetElm = '#app';
		
		$(targetElm).removeClass(MOBILE_SIDEBAR_CLOSED_CLASS).addClass(MOBILE_SIDEBAR_TOGGLE_CLASS);
	});
};
var handleSidebarMobileDismiss = function() {
	$(document).on('click', '[data-dismiss="sidebar-mobile"]', function(e) {
		e.preventDefault();
		
		var targetElm = '#app';
		
		$(targetElm).removeClass(MOBILE_SIDEBAR_TOGGLE_CLASS).addClass(MOBILE_SIDEBAR_CLOSED_CLASS);
		setTimeout(function() {
			$(targetElm).removeClass(MOBILE_SIDEBAR_CLOSED_CLASS);
		}, 250);
	});
};


/* 06. Handle Sidebar Minify Float Menu
------------------------------------------------ */
var floatSubMenuTimeout;
var targetFloatMenu;
var handleMouseoverFloatSubMenu = function(elm) {
	clearTimeout(floatSubMenuTimeout);
};
var handleMouseoutFloatSubMenu = function(elm) {
	floatSubMenuTimeout = setTimeout(function() {
		$('.app-float-submenu').remove();
	}, 250);
};
var handleSidebarMinifyFloatMenu = function() {
	$(document).on('click', '.app-float-submenu .menu-item.has-sub > .menu-link', function(e) {
		e.preventDefault();
		
		var target = $(this).next('.menu-submenu');
		$(target).slideToggle(250, function() {
			var targetMenu = $('.app-float-submenu');
			var targetHeight = $(targetMenu).height() + 20;
			var targetOffset = $(targetMenu).offset();
			var targetTop = $(targetMenu).attr('data-offset-top');
			var windowHeight = $(window).height();
			if ((windowHeight - targetTop) > targetHeight) {
				$('.app-float-submenu').css({
					'top': targetTop,
					'bottom': 'auto',
					'overflow': 'initial'
				});
			} else {
				$('.app-float-submenu').css({
					'bottom': 0,
					'overflow': 'scroll'
				});
			}
		});
	});
	$(document).on('mouseover', '.app-sidebar-minified .app-sidebar .menu .menu-item.has-sub > .menu-link', function() {
		clearTimeout(floatSubMenuTimeout);
		
		var targetMenu = $(this).closest('.menu-item').find('.menu-submenu').first();
		if (targetFloatMenu == this) {
			return false;
		} else {
			targetFloatMenu = this;
		}
		var targetMenuHtml = $(targetMenu).html();
		
		if (targetMenuHtml) {
			var targetHeight = $(targetMenu).height() + 20;
			var targetOffset = $(this).offset();
			var targetTop = targetOffset.top - $(window).scrollTop();
			var targetLeft = (!$('#app').hasClass('app-sidebar-right')) ? $('#sidebar').width() + $('#sidebar').offset().left : 'auto';
			var targetRight = (!$('#app').hasClass('app-sidebar-right')) ? 'auto' : $('#sidebar').width();
			var windowHeight = $(window).height();
			var submenuHeight = 0;
			
			if ($('.app-float-submenu').length == 0) {
				targetMenuHtml = '<div class="app-float-submenu" data-offset-top="'+ targetTop +'" onmouseover="handleMouseoverFloatSubMenu(this)" onmouseout="handleMouseoutFloatSubMenu(this)">' + targetMenuHtml + '</div>';
				$('body').append(targetMenuHtml);
			} else {
				$('.app-float-submenu').html(targetMenuHtml);
			}
			submenuHeight = $('.app-float-submenu').height();
			if ((windowHeight - targetTop) > targetHeight && ((targetTop + submenuHeight) < windowHeight)) {
				$('.app-float-submenu').css({
					'top': targetTop,
					'left': targetLeft,
					'bottom': 'auto',
					'right': targetRight
				});
			} else {
				$('.app-float-submenu').css({
					'bottom': 0,
					'top': 'auto',
					'left': targetLeft,
					'right': targetRight
				});
			}
		} else {
			$('.app-float-submenu').remove();
			targetFloatMenu = '';
		}
	});
	$(document).on('mouseout', '.app-sidebar-minified .app-sidebar .menu > .menu-item.has-sub > .menu-link', function() {
		floatSubMenuTimeout = setTimeout(function() {
			$('.app-float-submenu').remove();
			targetFloatMenu = '';
		}, 250);
	});
}


/* 07. Handle Dropdown Close Option
------------------------------------------------ */
var handleDropdownClose = function() {
	$(document).on('click', '[data-dropdown-close="false"]', function(e) {
		e.stopPropagation();
	});
};


/* 08. Handle Card - Remove / Reload / Collapse / Expand
------------------------------------------------ */
var cardActionRunning = false;
var handleCardAction = function() {
	"use strict";

	if (cardActionRunning) {
		return false;
	}
	cardActionRunning = true;

	// expand
	$(document).on('mouseover', '[data-toggle=card-expand]', function(e) {
		if (!$(this).attr('data-init')) {
			$(this).tooltip({
				title: 'Expand / Compress',
				placement: 'bottom',
				trigger: 'hover',
				container: 'body'
			});
			$(this).tooltip('show');
			$(this).attr('data-init', true);
		}
	});
	$(document).on('click', '[data-toggle=card-expand]', function(e) {
		e.preventDefault();
		var target = $(this).closest('.card');
		var targetBody = $(target).find('.card-body');
		var targetClass = 'card-expand';
		var targetTop = 40;
		if ($(targetBody).length !== 0) {
			var targetOffsetTop = $(target).offset().top;
			var targetBodyOffsetTop = $(targetBody).offset().top;
			targetTop = targetBodyOffsetTop - targetOffsetTop;
		}

		if ($('body').hasClass(targetClass) && $(target).hasClass(targetClass)) {
			$('body, .card').removeClass(targetClass);
			$('.card').removeAttr('style');
			$(targetBody).removeAttr('style');
		} else {
			$('body').addClass(targetClass);
			$(this).closest('.card').addClass(targetClass);
		}
		$(window).trigger('resize');
	});
};


/* 09. Handle Tooltip & Popover Activation
------------------------------------------------ */
var handelTooltipPopoverActivation = function() {
	"use strict";
	if ($('[data-toggle="tooltip"]').length !== 0) {
		$('[data-toggle=tooltip]').tooltip();
	}
	if ($('[data-toggle="popover"]').length !== 0) {
		$('[data-toggle=popover]').popover();
	}
};


/* 10. Handle Scroll to Top Button Activation
------------------------------------------------ */
var handleScrollToTopButton = function() {
	"use strict";
	$(document).on('scroll', function() {
		var totalScroll = $(document).scrollTop();

		if (totalScroll >= 200) {
			$('[data-click=scroll-top]').addClass('show');
		} else {
			$('[data-click=scroll-top]').removeClass('show');
		}
	});
	$('[data-click=scroll-top]').on('click', function(e) {
		e.preventDefault();
		$('html, body, .content').animate({
			scrollTop: $("body").offset().top
		}, 500);
	});
};


/* 11. Handle hexToRgba
------------------------------------------------ */
var hexToRgba = function(hex, transparent = 1) {
	var c;
	if(/^#([A-Fa-f0-9]{3}){1,2}$/.test(hex)){
			c= hex.substring(1).split('');
			if(c.length== 3){
					c= [c[0], c[0], c[1], c[1], c[2], c[2]];
			}
			c= '0x'+c.join('');
			return 'rgba('+[(c>>16)&255, (c>>8)&255, c&255].join(',')+','+ transparent +')';
	}
  throw new Error('Bad Hex');
};


/* 12. Handle Scroll to
------------------------------------------------ */
var handleScrollTo = function() {
	$(document).on('click', '[data-toggle="scroll-to"]', function(e) {
		e.preventDefault();
		
		var targetElm = ($(this).attr('data-target')) ? $(this).attr('data-target') : $(this).attr('href');
		if (targetElm) {
			$('html, body').animate({
				scrollTop: $(targetElm).offset().top - $('#header').height() - 24
			}, 0);
		}
	});
};


/* Application Controller
------------------------------------------------ */
var App = function () {
	"use strict";
	
	return {
		//main function
		init: function () {
			this.initSidebar();
			this.initHeader();
			this.initComponent();
		},
		initSidebar: function() {
			handleSidebarMinifyFloatMenu();
			handleSidebarMenu();
			handleSidebarMinify();
			handleSidebarMobileToggle();
			handleSidebarMobileDismiss();
		},
		initHeader: function() {
		},
		initComponent: function() {
			handleSlimScroll();
			handleCardAction();
			handelTooltipPopoverActivation();
			handleScrollToTopButton();
			handleDropdownClose();
			handleScrollTo();
		},
		scrollTop: function() {
			$('html, body, .content').animate({
				scrollTop: $('body').offset().top
			}, 0);
		}
	};
}();

$(document).ready(function() {
	App.init();
});