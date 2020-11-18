
$(function () {




$(document).ready(function () {
// MENU _PRADEEP								
	$(".open").pageslide();
	


// MENU _PRADEEP
	$('.sf-menu').superfish();
// TABS _PRADEEP
		$('.accordion-tab').easyResponsiveTabs({
            type: 'accordion', //Types: default, vertical, accordion           
            width: 'auto', //auto or any width like 600px
            fit: true,   // 100% fit in a container
            closed: 'accordion', // Start closed if in accordion view
        });
        $('#horizontalTab').easyResponsiveTabs({
            type: 'default', //Types: default, vertical, accordion           
            width: 'auto', //auto or any width like 600px
            fit: true,   // 100% fit in a container
            closed: 'accordion', // Start closed if in accordion view
        });

        $('#verticalTab').easyResponsiveTabs({
            type: 'vertical',
            width: 'auto',
            fit: true
        });
	});


// SLIDER _PRADEEP
		$('.flexslider').flexslider({
		animation: "fade",
		controlsContainer: ".flex-container",
		directionNav:false,
		controlNav: true,
		slideshow: true,
		slideshowSpeed: 6000,
		pauseOnHover: true
		});
// FANCY BOX _PRADEEP
		$('.fancybox').fancybox();
// To TOP _PRADEEP
		$().UItoTop({ easingType: 'easeOutQuart' });
});
