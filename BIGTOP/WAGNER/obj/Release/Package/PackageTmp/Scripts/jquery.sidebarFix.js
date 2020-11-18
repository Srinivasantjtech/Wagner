/**
 * jquery.sidebarFix.js
 * version 1.1.0
 * @author Tomoya Koyanagi <tomk79@gmail.com>
 */
(function($){

	var _sidebars = [];
	var lastScrollTop = 0;
	var scrollDirection = 0;
	var lastScrollDirection = 0;
	var topBuffer = 0;
	var $win = $(window);

	/**
	* sidebarFix();
	*/
	$.fn.sidebarFix = function( opt ){
		_sidebars.push(this);
		this.sidebarFixData = opt;
		this
			.css('overflow','hidden')
			.css('position','static')
			.css('left', 0 )
			.css('top', 0 )
			//.css('width', 'auto' )
		;
		this.sidebarFixData.apply = true;
		if( this.height() >= opt.frame.height() ){
			// console.log('this ã¨ frame ãŒåŒã˜é«˜ã•ã€‚');
			this.sidebarFixData.apply = false;
		}
		if( this.offset().top != opt.frame.offset().top ){
			// console.log('offsetTop ãŒ ç­‰ä¾¡ã§ã¯ãªã„ã€‚');
			this.sidebarFixData.apply = false;
		}
		if( opt.topBuffer ){
			topBuffer = opt.topBuffer;
		}

		updateStatus(this);
		
		if(window.innerWidth > 767){
				_this
				$("this").removeClass("jq_sidebar_fix");
				$("this").removeClass("middle");
			}
	}

	/**
	 * update status
	 */
	  
	 
	function updateStatus(_this){
		if( !_this.sidebarFixData.apply ){
			_this
				.css('overflow','visible')
				.css('position','static')
			;
			return;
		}

		var frameOffsetScrollTop = _this.sidebarFixData.frame.offset().top + _this.height();
		var scrollUnder = $win.height() + $win.scrollTop();
		var areaUnder = _this.offset().top + _this.height();
		var frameUnder = _this.sidebarFixData.frame.offset().top + _this.sidebarFixData.frame.height();

		// if( $win.scrollTop() >= _this.offset().top - topBuffer && scrollUnder <= areaUnder ){
		// 	console.log('é€”ä¸­ã«ã„ã¾ã™ã€‚');
		// }

		if( $win.scrollTop() < _this.sidebarFixData.frame.offset().top - topBuffer ){
			// ã‚¹ã‚¯ãƒ­ãƒ¼ãƒ«ä½ç½®ãŒ frame ã‚ˆã‚Šä¸Šãªå ´åˆ
			_this
				.css('position','static')
				.css('left', 0 )
				.css('top', 0 )
				//.css('width', 'auto' )
			;

		}else if( frameUnder <= $win.scrollTop()+_this.height() + topBuffer && frameUnder <= scrollUnder ){
			// ä¸€ç•ªä¸‹ã¾ã§ã‚¹ã‚¯ãƒ­ãƒ¼ãƒ«ã—ã¡ã‚ƒã£ã¦ã‚‹å ´åˆ
			_this
				.css('position','static')
				.css('left', 0 )
				.css('top', _this.sidebarFixData.frame.height() - _this.height() )
				//.css('width', 'auto' )
				.css('width', '19%' )
				.css('position','relative')
			;

		}else if( $win.scrollTop() >= _this.offset().top - topBuffer && scrollUnder <= areaUnder && _this.css('position') == 'relative' ){
			// console.log('ç”»é¢ã«åŽã¾ã£ã¦ãªãã¦position:relative;');

		}else if( scrollDirection < 0 && lastScrollDirection > 0 && $win.scrollTop() >= _this.offset().top - topBuffer && scrollUnder <= areaUnder ){
			// ä¸Šå‘ã(ã«ã€åˆ‡ã‚Šæ›¿ã‚ã£ã¦ä¸€ç™ºç›®)
			if( _this.css('position') != 'relative' ){
				// console.log('ä¸Šå‘ã(ã«ã€åˆ‡ã‚Šæ›¿ã‚ã£ã¦ä¸€ç™ºç›®)');
				var tmpTop = _this.offset().top - _this.sidebarFixData.frame.offset().top;
				_this
					.css('position','static')
					.css('left', 0 )
					.css('top', tmpTop )
					//.css('width', 'auto' )
					.css('width', '19%' )
					.css('position','relative')
				;
			}

		}else if( scrollDirection > 0 && lastScrollDirection < 0 && $win.scrollTop() >= _this.offset().top - topBuffer && scrollUnder <= areaUnder ){
			// ä¸‹å‘ã(ã«ã€åˆ‡ã‚Šæ›¿ã‚ã£ã¦ä¸€ç™ºç›®)
			if( _this.css('position') != 'relative' ){
				// console.log('ä¸‹å‘ã(ã«ã€åˆ‡ã‚Šæ›¿ã‚ã£ã¦ä¸€ç™ºç›®)');
				var tmpTop = _this.offset().top - _this.sidebarFixData.frame.offset().top;
				_this
					.css('position','static')
					.css('left', 0 )
					.css('top', tmpTop )
					//.css('width', 'auto' )
					.css('width', '19%' )
					.css('position','relative')
				;
			}

		}else if( scrollDirection < 0 ){
			// ä¸Šå‘ã(ç¶™ç¶šçš„)
			if( frameUnder - _this.height() - $win.scrollTop() - topBuffer >= 0 && $win.scrollTop() > _this.sidebarFixData.frame.offset().top + topBuffer ){
				if( _this.css('position') != 'fixed' ){
					_this
						.css('position','static')
						.css('left', _this.offset().left - $win.scrollLeft() )
						.css('top', topBuffer )
						//.css('width', 'auto' )
						.css('width', '19%' )
						.css('position','fixed')
					;
				}

			}

		}else if( scrollDirection > 0 ){
			// ä¸‹å‘ã(ç¶™ç¶šçš„)
			if( _this.height() <= $win.height() - topBuffer ){
				_this
					.css('position','static')
					.css('left', _this.offset().left - $win.scrollLeft() )
					.css('top', topBuffer )
					//.css('width', 'auto' )
					.css('width', '19%' )
					.css('position','fixed')
				;
				
				
				

			}else if( frameOffsetScrollTop <= scrollUnder ){
				_this
					.css('position','static')
					.css('left', _this.offset().left - $win.scrollLeft() )
					.css('top', $win.height() - _this.height() )
					//.css('width', 'auto' )
					.css('width', '19%' )
					.css('position','fixed')
				;
			}
			
			else if(window.innerWidth > 767){
				_this
					//.css('display','none');
					$("this").removeClass("jq_sidebar_fix");
					$("this").removeClass("middle");
			}
			
		}
		

	}
	
	
	if(window.innerWidth < 992){
				
		updateStatus (_this);
		updateLayout();
		$.fn.sidebarFix ()
	}
	 

	/** 
	 * update layout
	 */
	function updateLayout(){
		var tmpList = _sidebars;
		_sidebars = [];
		for( var row in tmpList ){
			tmpList[row].sidebarFix( {frame: tmpList[row].sidebarFixData.frame} );
		}
		return true;
	}

	/**
	 * On window resized.
	 */
	$win.resize(function(e){
		updateLayout();
		return true;
	});

	/**
	 * On window scrolled.
	 */
	var scrollEventHandler = function(e){
		scrollDirection = $win.scrollTop()-lastScrollTop;
		for( var row in _sidebars ){
			updateStatus(_sidebars[row]);
		}
		lastScrollTop = $win.scrollTop();
		lastScrollDirection = scrollDirection;
		return true;
	}
	$win.bind('scroll', scrollEventHandler);
	// $win.bind('touchmove', scrollEventHandler);
	// $win.bind('touchend', scrollEventHandler);
	$win.bind('gestureend', scrollEventHandler);

})(jQuery);
