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
	var windowSize = $(window).width();

	/**
	* sidebarFix();
	*/
	$.fn.sidebarFix = function( opt ){
		_sidebars.push(this);
		this.sidebarFixData = opt;
		this
			//.css('overflow','hidden')
			.css('position','static')
			.css('left', 0 )
			.css('top', 0 )
			//.css('width', '230px' )
		;
		if(windowSize > 992 && windowSize < 1280) {
			 this
			//.css('overflow','hidden')
			.css('position','static')
			.css('left', 0 )
			.css('top', 0 )
			.css('width', '200px' )
			;
		}
		else if(windowSize > 1281 && windowSize < 2560) {
			 this
			//.css('overflow','hidden')
			.css('position','static')
			.css('left', 0 )
			.css('top', 0 )
			.css('width', '230px' )
			;
		}
		this.sidebarFixData.apply = true;
		if( this.height() >= opt.frame.height() ){
			// console.log('this Ã£ÂÂ¨ frame Ã£ÂÅ’Ã¥ÂÅ’Ã£ÂËœÃ©Â«ËœÃ£Ââ€¢Ã£â‚¬â€š');
			this.sidebarFixData.apply = false;
		}
		if( this.offset().top != opt.frame.offset().top ){
			// console.log('offsetTop Ã£ÂÅ’ Ã§Â­â€°Ã¤Â¾Â¡Ã£ÂÂ§Ã£ÂÂ¯Ã£ÂÂªÃ£Ââ€žÃ£â‚¬â€š');
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
		// 	console.log('Ã©â‚¬â€Ã¤Â¸Â­Ã£ÂÂ«Ã£Ââ€žÃ£ÂÂ¾Ã£Ââ„¢Ã£â‚¬â€š');
		// }
		
		
		

		if( $win.scrollTop() < _this.sidebarFixData.frame.offset().top - topBuffer ){
			// Ã£â€šÂ¹Ã£â€šÂ¯Ã£Æ’Â­Ã£Æ’Â¼Ã£Æ’Â«Ã¤Â½ÂÃ§Â½Â®Ã£ÂÅ’ frame Ã£â€šË†Ã£â€šÅ Ã¤Â¸Å Ã£ÂÂªÃ¥ Â´Ã¥ÂË†
			_this
				.css('position','static')
				.css('left', 0 )
				.css('top', 0 )
				//.css('width', 'auto' )
			;

		}else if( frameUnder <= $win.scrollTop()+_this.height() + topBuffer && frameUnder <= scrollUnder ){
			// Ã¤Â¸â‚¬Ã§â€¢ÂªÃ¤Â¸â€¹Ã£ÂÂ¾Ã£ÂÂ§Ã£â€šÂ¹Ã£â€šÂ¯Ã£Æ’Â­Ã£Æ’Â¼Ã£Æ’Â«Ã£Ââ€”Ã£ÂÂ¡Ã£â€šÆ’Ã£ÂÂ£Ã£ÂÂ¦Ã£â€šâ€¹Ã¥ Â´Ã¥ÂË†
			_this
				.css('position','static')
				.css('left', 0 )
				.css('top', _this.sidebarFixData.frame.height() - _this.height() )
				//.css('width', 'auto' )
				.css('width', '20%' )
				.css('position','relative')
			;

		}else if( $win.scrollTop() >= _this.offset().top - topBuffer && scrollUnder <= areaUnder && _this.css('position') == 'relative' ){
			// console.log('Ã§â€Â»Ã©ÂÂ¢Ã£ÂÂ«Ã¥ÂÅ½Ã£ÂÂ¾Ã£ÂÂ£Ã£ÂÂ¦Ã£ÂÂªÃ£ÂÂÃ£ÂÂ¦position:relative;');

		}else if( scrollDirection < 0 && lastScrollDirection > 0 && $win.scrollTop() >= _this.offset().top - topBuffer && scrollUnder <= areaUnder ){
			// Ã¤Â¸Å Ã¥Ââ€˜Ã£ÂÂ(Ã£ÂÂ«Ã£â‚¬ÂÃ¥Ë†â€¡Ã£â€šÅ Ã¦â€ºÂ¿Ã£â€šÂÃ£ÂÂ£Ã£ÂÂ¦Ã¤Â¸â‚¬Ã§â„¢ÂºÃ§â€ºÂ®)
			if( _this.css('position') != 'relative' ){
				// console.log('Ã¤Â¸Å Ã¥Ââ€˜Ã£ÂÂ(Ã£ÂÂ«Ã£â‚¬ÂÃ¥Ë†â€¡Ã£â€šÅ Ã¦â€ºÂ¿Ã£â€šÂÃ£ÂÂ£Ã£ÂÂ¦Ã¤Â¸â‚¬Ã§â„¢ÂºÃ§â€ºÂ®)');
				var tmpTop = _this.offset().top - _this.sidebarFixData.frame.offset().top;
				_this
					.css('position','static')
					.css('left', 0 )
					.css('top', tmpTop )
					//.css('width', 'auto' )
					.css('width', '19.5%' )
					.css('position','relative')
				;
			}

		}else if( scrollDirection > 0 && lastScrollDirection < 0 && $win.scrollTop() >= _this.offset().top - topBuffer && scrollUnder <= areaUnder ){
			// Ã¤Â¸â€¹Ã¥Ââ€˜Ã£ÂÂ(Ã£ÂÂ«Ã£â‚¬ÂÃ¥Ë†â€¡Ã£â€šÅ Ã¦â€ºÂ¿Ã£â€šÂÃ£ÂÂ£Ã£ÂÂ¦Ã¤Â¸â‚¬Ã§â„¢ÂºÃ§â€ºÂ®)
			if( _this.css('position') != 'relative' ){
				// console.log('Ã¤Â¸â€¹Ã¥Ââ€˜Ã£ÂÂ(Ã£ÂÂ«Ã£â‚¬ÂÃ¥Ë†â€¡Ã£â€šÅ Ã¦â€ºÂ¿Ã£â€šÂÃ£ÂÂ£Ã£ÂÂ¦Ã¤Â¸â‚¬Ã§â„¢ÂºÃ§â€ºÂ®)');
				var tmpTop = _this.offset().top - _this.sidebarFixData.frame.offset().top;
				_this
					.css('position','static')
					.css('left', 0 )
					.css('top', tmpTop )
					//.css('width', 'auto' )
					.css('width', '19.5%' )
					.css('position','relative')
				;
			}

		}else if( scrollDirection < 0 ){
			// Ã¤Â¸Å Ã¥Ââ€˜Ã£ÂÂ(Ã§Â¶â„¢Ã§Â¶Å¡Ã§Å¡â€ž)
			//alert('fix1');
			if( frameUnder - _this.height() - $win.scrollTop() - topBuffer >= 0 && $win.scrollTop() > _this.sidebarFixData.frame.offset().top + topBuffer ){
				if( _this.css('position') != 'fixed' ){
					_this
						.css('position','static')
						.css('left', _this.offset().left - $win.scrollLeft() )
						.css('top', topBuffer )
						//.css('width', 'auto' )
						//.css('width', '19%' )
						.css('position','fixed')
					;
				}
					
					else if (window.innerWidth < 1050){
					 _this
						.css('width', '200px' );
					}
					
					else if (window.innerWidth < 1100){
					 _this
						.css('width', '200px' );
					}
					
					else if (window.innerWidth < 1160){
					 _this
						.css('width', '200px' );
					}
					else if (window.innerWidth < 1220){
					 _this
						.css('width', '230px' );
					}
					
					
					/*else if (window.innerWidth < 1280){
					 _this
						.css('width', '245px' );
					}
					else if (window.innerWidth < 1360){
					 _this
						.css('width', '245px' );
					}
					else if (window.innerWidth < 1440){
					 _this
						.css('width', '245px' );
					}
					else if (window.innerWidth < 1520){
					 _this
						.css('width', '245px%' );
					}
					else if (window.innerWidth < 1600){
					 _this
						.css('width', '245px' );
					}
					else if (window.innerWidth < 1680){
					 _this
						.css('width', '245px' );
					}
					else if (window.innerWidth < 1760){
					 _this
						.css('width', '245px' );
					}
					else if (window.innerWidth < 1840){
					 _this
						.css('width', '245px' );
					}*/
					
					else if (window.innerWidth < 2560){
					 _this
						.css('width', '230px' );
					}

				

			}

		}
		else if( scrollDirection > 0 ){
			// Ã¤Â¸â€¹Ã¥Ââ€˜Ã£ÂÂ(Ã§Â¶â„¢Ã§Â¶Å¡Ã§Å¡â€ž)
			//alert('top');
			//if(window.innerWidth > 767){}
			
					if( _this.height() <= $win.height() - topBuffer ){
						_this
							.css('position','static')
							.css('left', _this.offset().left - $win.scrollLeft() )
							.css('top', topBuffer )
							//.css('width', 'auto' )
							//.css('width', '19%' )
							.css('position','fixed')
						;
		
					}
			
					else if (window.innerWidth < 1050){
					 _this
						.css('width', '200px' );
					}
					
					else if (window.innerWidth < 1100){
					 _this
						.css('width', '200px' );
					}
					
					else if (window.innerWidth < 1160){
					 _this
						.css('width', '200px' );
					}
					else if (window.innerWidth < 1220){
					 _this
						.css('width', '230px' );
					}
					
					
					/*else if (window.innerWidth < 1280){
					 _this
						.css('width', '230px' );
					}
					else if (window.innerWidth < 1360){
					 _this
						.css('width', '230px' );
					}
					else if (window.innerWidth < 1440){
					 _this
						.css('width', '230px' );
					}
					else if (window.innerWidth < 1520){
					 _this
						.css('width', '230px%' );
					}
					else if (window.innerWidth < 1600){
					 _this
						.css('width', '230px' );
					}
					else if (window.innerWidth < 1680){
					 _this
						.css('width', '230px' );
					}
					else if (window.innerWidth < 1760){
					 _this
						.css('width', '230px' );
					}
					else if (window.innerWidth < 1840){
					 _this
						.css('width', '230px' );
					}*/
					else if (window.innerWidth < 2560){
					 _this
						.css('width', '230px' );
						//alert('lg');
					}
					 
			
			
			
			if( frameOffsetScrollTop <= scrollUnder ){
				//alert('fix2');
					if(window.innerWidth > 767){
				_this
					
					.css('position','static')
					.css('left', _this.offset().left - $win.scrollLeft() )
					.css('top', $win.height() - _this.height() )
					//.css('width', 'auto' )
					//.css('width', '18.5%' )
					.css('position','fixed');
					}
					
					else if (window.innerWidth < 1050){
					 _this
						.css('width', '200px' );
					}
					
					else if (window.innerWidth < 1100){
					 _this
						.css('width', '200px' );
					}
					
					else if (window.innerWidth < 1160){
					 _this
						.css('width', '200px' );
					}
					else if (window.innerWidth < 1220){
					 _this
						.css('width', '230px' );
					}
					
					
					/*else if (window.innerWidth < 1280){
					 _this
						.css('width', '240px' );
					}
					else if (window.innerWidth < 1360){
					 _this
						.css('width', '240px' );
					}
					else if (window.innerWidth < 1440){
					 _this
						.css('width', '240px' );
					}
					else if (window.innerWidth < 1520){
					 _this
						.css('width', '240px%' );
					}
					else if (window.innerWidth < 1600){
					 _this
						.css('width', '240px' );
					}
					else if (window.innerWidth < 1680){
					 _this
						.css('width', '240px' );
					}
					else if (window.innerWidth < 1760){
					 _this
						.css('width', '240px' );
					}
					else if (window.innerWidth < 1840){
					 _this
						.css('width', '240px' );
					}*/
					
					else if (window.innerWidth < 2560){
					 _this
						.css('width', '230px' );
					}
					
			}
			
			else if(window.innerWidth > 767){
				//alert('mobile');
				_this
					//.css('display','none');
					$("this").removeClass("jq_sidebar_fix");
					$("this").removeClass("middle");
			}
			
		}
		

	}
	
	
	if(window.innerWidth < 992){
				//alert('largeview');
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
