//$(function () {
//    // ======================= imagesLoaded Plugin ===============================
//    // https://github.com/desandro/imagesloaded

//    // $('#my-container').imagesLoaded(myFunction)
//    // execute a callback when all images have loaded.
//    // needed because .load() doesn't work on cached images

//    // callback function gets image collection as argument
//    //  this is the container

//    // original: mit license. paul irish. 2010.
//    // contributors: Oren Solomianik, David DeSandro, Yiannis Chatzikonstantinou

//    $.fn.imagesLoaded = function (callback) {
//        var $images = this.find('img'),
//		len = $images.length,
//		_this = this,
//		blank = 'data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==';

//        function triggerCallback() {
//            callback.call(_this, $images);
//        }

//        function imgLoaded() {
//            if (--len <= 0 && this.src !== blank) {
//                setTimeout(triggerCallback);
//                $images.off('load error', imgLoaded);
//            }
//        }

//        if (!len) {
//            triggerCallback();
//        }

//        $images.on('load error', imgLoaded).each(function () {
//            // cached images don't fire load sometimes, so we reset src.
//            if (this.complete || this.complete === undefined) {
//                var src = this.src;
//                // webkit hack from http://groups.google.com/group/jquery-dev/browse_thread/thread/eee6ab7b2da50e1f
//                // data uri bypasses webkit log warning (thx doug jones)
//                this.src = blank;
//                this.src = src;
//            }
//        });

//        return this;
//    };

//    // gallery container
//    var $rgGallery = $('#rg-gallery'),
//    // carousel container
//	$esCarousel = $rgGallery.find('div.es-carousel-wrapper'),
//    // the carousel items
//	$items = $esCarousel.find('ul > li'),
//    // total number of items
//	itemsCount = $items.length;

//    Gallery = (function () {
//        // index of the current item
//        var current = 0,
//        // mode : carousel || fullview
//			mode = 'carousel',
//        // control if one image is being loaded
//			anim = false,
//			init = function () {

//			    // (not necessary) preloading the images here...
//			    $items.add('<img src="/images/ajax-loader.gif"/><img src="/images/black.png"/>').imagesLoaded(function () {
//			        // add options
//			        _addViewModes();

//			        // add large image wrapper
//			        _addImageWrapper();

//			        // show first image
//			        _showImage($items.eq(current));

//			    });

//			    // initialize the carousel
//			    if (mode === 'carousel')
//			        _initCarousel();

//			},
//			_initCarousel = function () {

//			    // we are using the elastislide plugin:
//			    // http://tympanus.net/codrops/2011/09/12/elastislide-responsive-carousel/
//			    $esCarousel.show().elastislide({
//			        imageW: 65,
//			        onClick: function ($item) {
//			            if (anim) return false;
//			            anim = true;
//			            // on click show image
//			            _showImage($item);
//			            // change current
//			            current = $item.index();
//			        }
//			    });

//			    // set elastislide's current to current
//			    $esCarousel.elastislide('setCurrent', current);

//			},
//			_addViewModes = function () {

//			    // top right buttons: hide / show carousel

//			    var $viewfull = $('<a href="#" class="rg-view-full"></a>'),
//					$viewthumbs = $('<a href="#" class="rg-view-thumbs rg-view-selected"></a>');

//			    $rgGallery.prepend($('<div class="rg-view"/>').append($viewfull).append($viewthumbs));

//			    $viewfull.on('click.rgGallery', function (event) {
//			        if (mode === 'carousel')
//			            $esCarousel.elastislide('destroy');
//			        $esCarousel.hide();
//			        $viewfull.addClass('rg-view-selected');
//			        $viewthumbs.removeClass('rg-view-selected');
//			        mode = 'fullview';
//			        return false;
//			    });

//			    $viewthumbs.on('click.rgGallery', function (event) {
//			        _initCarousel();
//			        $viewthumbs.addClass('rg-view-selected');
//			        $viewfull.removeClass('rg-view-selected');
//			        mode = 'carousel';
//			        return false;
//			    });

//			    if (mode === 'fullview')
//			        $viewfull.trigger('click');

//			},
//			_addImageWrapper = function () {

//			    // adds the structure for the large image and the navigation buttons (if total items > 1)
//			    // also initializes the navigation events

//			    $('#img-wrapper-tmpl').tmpl({ itemsCount: itemsCount }).appendTo($rgGallery);

//			  

//			    if (itemsCount > 1) {
//			        // addNavigation
//			        var $navPrev = $rgGallery.find('a.rg-image-nav-prev'),
//						$navNext = $rgGallery.find('a.rg-image-nav-next'),
//						$imgWrapper = $rgGallery.find('div.rg-image');

//			        $navPrev.on('click.rgGallery', function (event) {
//			            _navigate('left');
//			            return false;
//			        });

//			        $navNext.on('click.rgGallery', function (event) {
//			            _navigate('right');
//			            return false;
//			        });

//			        // add touchwipe events on the large image wrapper
//			        $imgWrapper.touchwipe({
//			            wipeLeft: function () {
//			                _navigate('right');
//			            },
//			            wipeRight: function () {
//			                _navigate('left');
//			            },
//			            preventDefaultEvents: false
//			        });

//			        $(document).on('keyup.rgGallery', function (event) {
//			            if (event.keyCode == 39)
//			                _navigate('right');
//			            else if (event.keyCode == 37)
//			                _navigate('left');
//			        });

//			    }

//			},
//			_navigate = function (dir) {

//			    // navigate through the large images

//			    if (anim) return false;
//			    anim = true;

//			    if (dir === 'right') {
//			        if (current + 1 >= itemsCount)
//			            current = 0;
//			        else
//			            ++current;
//			    }
//			    else if (dir === 'left') {
//			        if (current - 1 < 0)
//			            current = itemsCount - 1;
//			        else
//			            --current;
//			    }

//			    _showImage($items.eq(current));

//			},
//			_showImage = function ($item) {

//			    // shows the large image that is associated to the $item

//			    var $loader = $rgGallery.find('div.rg-loading').show();

//			    $items.removeClass('selected');
//			    $item.addClass('selected');

//			    var $thumb = $item.find('img'),
//					largesrc = $thumb.data('large'),
//					title = $thumb.data('description');

//			    $('<img/>').load(function () {

//			        $rgGallery.find('div.rg-image').empty().append('<img src="' + largesrc + '"/>');

//			        if (title)
//			            $rgGallery.find('div.rg-caption').show().children('p').empty().text(title);

//			        $loader.hide();

//			        if (mode === 'carousel') {
//			            $esCarousel.elastislide('reload');
//			            $esCarousel.elastislide('setCurrent', current);
//			        }

//			        anim = false;

//			    }).attr('src', largesrc);

//			},
//			addItems = function ($new) {

//			    $esCarousel.find('ul').append($new);
//			    $items = $items.add($($new));
//			    itemsCount = $items.length;
//			    $esCarousel.elastislide('add', $new);

//			};

//        return {
//            init: init,
//            addItems: addItems
//        };

//    })();

//    Gallery.init();

//    /*
//    Example to add more items to the gallery:
//	
//    var $new  = $('<li><a href="#"><img src="images/thumbs/1.jpg" data-large="images/1.jpg" alt="image01" data-description="From off a hill whose concave womb reworded" /></a></li>');
//    Gallery.addItems( $new );
//    */
//});





(function (a) {
    a.fn.lightBox = function (s) {
        s = jQuery.extend({ overlayBgColor: "#000", overlayOpacity: 0.4, hoverNavigation: false, imageWidth: 550, imageHeight: 430, /*imageLoading: "",*/ imageLoading: "images/lightbox-ico-loading.gif",  /*imageLoading:"images/ajax-loader-pp.gif",*/

            imageBtnPrev: "/images/aero-btn-Left.png",
            imageBtnNext: "/images/aero-btn-right.png",
            imageBtnPrevHover: "/images/aero-btn-Left.png",
            imageBtnNextHover: "/images/aero-btn-right.png",
            imgbtnnextdis: "/images/rightarrow.png",
            imgbtnpredis: "/images/leftarrow.png",


            /*  imageBtnPrev:"",
            imageBtnPrevHover:"",
            imageBtnNext:"",
            imageBtnNextHover:"",*/

            imageBtnClose: "/images/lightbox-btn-close1.png",
            preclick: false,
            btnnextclick: false,
            boolresize: false,
            /*imageBtnClose : "",*/
            imageBlank: "themes/default/img/lightbox-blank.gif", containerBorderSize: 10, containerResizeSpeed: 400, txtImage: "Image", txtOf: "of", keyToClose: "c", keyToPrev: "p", keyToNext: "n", imageArray: [], activeImage: 0
        }, s); var j = this; function v() { r(this, j); return false } function t() {
        }
        function r(z, y) {

            a("embed, object, select").css({ visibility: "hidden" }); c();
            s.imageArray.length = 0;
            s.activeImage = 0;
            var ylen = y.length;
            var pylen = ylen - 1;
            var targetlen = ylen - pylen;

            if (y.length == 1) { if (a(z).attr("href") == "/prodimages") { a("#jquery-lightbox").css({ 'visibility': 'hidden' }); b(); } s.imageArray.push(new Array(a(z).attr("href"), a(z).children("img").attr("src"))) } else { a(y).each(function () { var imgid_pp = a(this).children("img").attr("id"); if (imgid_pp != "popupmaindel") { s.imageArray.push(new Array(a(this).attr("href"), a(this).children("img").attr("src"))) } }) }
            t();
            //var indexToRemove = 0;
            // var numberToRemove = 1;

            // s.imageArray.splice(indexToRemove, numberToRemove);
            // s.imageArray.pop(0,1);
            //s.imageArray.shift();
            a("#lightbox-loading").css({ 'visibility': 'hidden' });
            for (var x = 0; x < s.imageArray.length; x++) { if (s.imageArray[x][0].toString().toLowerCase() == a(z).attr("href").toString().toLowerCase()) { s.activeImage = x; break } } o()
            if (s.imageArray.length > 11) {
                a("#lightbox-image-down").css({ 'visibility': 'visible' });
                a("#lightbox-image-down").css({ 'cursor': 'pointer' });
            }

            // alert(s.imageArray.length);

            if (s.activeImage > 11) {
                //alert("sss");
                a("#lightbox-image-up").css({ 'visibility': 'visible' });
                a("#lightbox-image-up").css({ 'cursor': 'pointer' });
                // a("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() + 400 }, 500);
            }
            //if( s.activeImage == s.imageArray.length - 1 && s.activeImage > 23 )
            if (s.activeImage > 11) {
                //alert("fff");
                a("#lightbox-image-down").css({ 'visibility': 'visible' });
                a("#lightbox-image-down").css({ 'cursor': 'pointer' });
                // a("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() + 400 }, 500);
            }

            // if (s.activeImage != s.imageArray.length - 1 && s.activeImage != 0) {
            if (s.activeImage != s.imageArray.length - 1) {
                s.btnnextclick = true;
            }
            //alert( s.btnnextclick);
            if (s.activeImage > 11 && s.activeImage <= 16) {

                a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details").scrollLeft() + 300 }, 500);
            }
            if (s.activeImage > 16 && s.activeImage <= 20) {

                a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details").scrollLeft() + 600 }, 500);
            }
            if (s.activeImage > 20 && s.activeImage <= 24) {
                a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details").scrollLeft() + 900 }, 500);
            }
            if (s.activeImage > 24 && s.activeImage <= 28) {
                a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details").scrollLeft() + 1200 }, 500);
            }
            if (s.activeImage > 28 && s.activeImage <= 32) {
                a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details").scrollLeft() + 1500 }, 500);
            }
            if (s.activeImage > 32 && s.activeImage <= 36) {
                a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details").scrollLeft() + 1800 }, 500);
            }
            if (s.activeImage > 36 && s.activeImage <= 40) {
                a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details").scrollLeft() + 2100 }, 500);
            }
            if (s.activeImage > 40 && s.activeImage <= 44) {
                a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details").scrollLeft() + 2400 }, 500);
            }
            if (s.activeImage > 44 && s.activeImage <= 48) {
                a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details").scrollLeft() + 2700 }, 500);
            }
            if (s.activeImage > 48 && s.activeImage <= 52) {
                a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details").scrollLeft() + 2900 }, 500);
            }



            if (s.imageArray.length == 1) {

                a("#lightbox-nav-btnPrev").css({ 'visibility': 'hidden' });
                a("#lightbox-nav-btnNext").css({ 'visibility': 'hidden' });
                a("#lightbox-nav-btnPrev").css({ 'display': 'none' });
                a("#lightbox-nav-btnNext").css({ 'display': 'none' });
            }

            if (s.imageArray.length > 1) {
                a("#lightbox-nav-btnPrev").css({ 'visibility': 'visible' });
                a("#lightbox-nav-btnNext").css({ 'visibility': 'visible' });
                a("#lightbox-nav-btnPrev").css({ 'display': 'block' });
                a("#lightbox-nav-btnNext").css({ 'display': 'block' });
            }




        } function c() {

            a("body").append('<div id="jquery-overlay" ></div><div id="jquery-lightbox" ><div id="lightbox-outer-container" class="loc"  ><div id="lightbox-container-image-box"><div id="lightbox-container-image"><img id="lightbox-image"><div id="lightbox-loading"><a href="#" id="lightbox-loading-link"><img src="/' + s.imageLoading + '"></a></div></div></div><div id="lightbox-container-image-data-box"><div id="lightbox-container-image-data"><div id="lightbox-image-up" style="visibility:hidden;" onclick="scrolldown();" ><img src="/images/leftarrow.png" alt="" /></div><div id="lightbox-image-details"><div id="imageinnerdiv" ><span id="lightbox-image-details-caption"></span><span id="lightbox-image-details-currentNumber"></span></div></div><div id="lightbox-image-down" style="visibility:hidden;" onclick="scrollup();"><img src="/images/rightarrow.png" alt="" /></div></div></div><div id="lightbox-secNav"><a  id="lightbox-secNav-btnClose" onclick="btn_popupclose();"><img src="' + s.imageBtnClose + '" width="36px" height="38px" /></a></div><a href="#" id="lightbox-nav-btnPrev" ></a><a href="#" id="lightbox-nav-btnNext"></a></div></div>');
            var x = f(); a("#jquery-overlay").css({ backgroundColor: s.overlayBgColor, opacity: s.overlayOpacity, width: x[0], height: x[1] }).fadeIn(); var y = h(); a("#jquery-lightbox").css({ top: (y[1] + (x[3] / 10)) - 45, left: y[0] }).show(); a("#jquery-overlay").click(function () { b() }); a("#lightbox-loading-link,#lightbox-secNav-btnClose").click(function () { b(); return false }); a(window).resize(function () { var z = f(); a("#jquery-overlay").css({ width: z[0], height: z[1] }); var A = h(); a("#jquery-lightbox").css({ top: A[1] + (z[3] / 10), left: A[0] }) })




            a("#lightbox-container-image-data-box").css({ 'display': 'none' });
            a("#jquery-lightbox").css({ 'display': 'none' });
            a("#jquery-overlay").css({ 'display': 'none' });

            a("#lightbox-secNav-btnClose").css({ 'display': 'none' });
            //alert("first");


            // alert(s.activeImage);
            // a(".thumb hover").focus();
            //a("#imageinnerdiv").css({ 'margin-top' : '0px'});
        } function o() {
            a("#lightbox-loading").show(); a("#lightbox-image").hide(); k(); w(); var x = new Image(); x.onload = function () {
                a("#lightbox-image").attr("src", s.imageArray[s.activeImage][0]); l(x.width, x.height);

                /* if(s.activeImage <= 0){ 
                objPrev = new Image(); 
                objPrev.src = s.imageArray[s.imageArray.length - 1][0] ;  
                // a("#lightbox-image").attr("src", objPrev.src);
                //alert(s.activeImage.length);
                }*/
                //alert("main");
                // alert(objPrev.src);
                // alert(objNext.src);
                //alert(x.height); 
                if (x.height < 100) {
                    var mleft = '3px'
                    var mtop = '140px';
                    a("#lightbox-image").css({ 'margin-top': mtop });
                    a("#lightbox-image").css({ 'margin-left': mleft });
                }
                else {
                    mtop = '';
                    mleft = '';
                    a("#lightbox-image").css({ 'margin-top': mtop });
                    a("#lightbox-image").css({ 'margin-left': mleft });
                }

                if (x.height > 400) {
                    a("#lightbox-container-image").css({ 'margin-top': '-63px' });
                }
                else {
                    a("#lightbox-container-image").css({ 'margin-top': '0px' });
                }
                // alert(s.imageArray.length);
                if (s.imageArray.length < 12) {
                    a("#lightbox-image-up").css({ 'visibility': 'hidden' });
                    a("#lightbox-image-down").css({ 'visibility': 'hidden' });
                    // a("#lightbox-image-details").css({ 'overflow-x' : 'auto'}); 
                    /*a("#lightbox-image-details").css({ 'max-height' : '528px'}); */
                    a("#lightbox-image-details").css({ 'height': '72px' });
                    a("#jquery-lightbox").css({ 'display': 'block' });
                    a("#jquery-overlay").css({ 'display': 'block' });
                    a("#lightbox-image-details").css({ 'max-width': '98%' });
                    a("#imageinnerdiv").css({ 'max-width': '3000px' });

                }
                else {
                    //a("#lightbox-image-details").css({ 'margin-top' : mtop}); 
                    //a("#lightbox-image-up").css({ 'visibility' : 'visible'}); 
                    //           a("#lightbox-image-down").css({ 'visibility' : 'visible'});
                    //           a("#lightbox-image-down").css({ 'cursor' : 'pointer'});
                    /* a("#lightbox-image-details").css({ 'height' : '528px'}); */
                    a("#lightbox-image-details").css({ 'height': '72px' });
                    //a("#lightbox-image-up").css({'visibility' : 'hidden'});
                    a("#jquery-lightbox").css({ 'display': 'block' });
                    a("#jquery-overlay").css({ 'display': 'block' });
                    a("#lightbox-image-details").css({ 'width': '98%' });
                    a("#imageinnerdiv").css({ 'width': '3000px' });

                }
                if (s.imageArray.length == 1) {
                    a("#lightbox-nav-btnPrev").css({ 'visibility': 'hidden' });
                    a("#lightbox-nav-btnNext").css({ 'visibility': 'hidden' });
                    a("#lightbox-nav-btnPrev").css({ 'display': 'none' });
                }

                if (s.activeImage == 0 && s.imageArray.length > 11) {
                    //alert("yes");
                    if (a("#lightbox-image-details").scrollLeft() > 0) {
                        a("#lightbox-image-details").animate({ "scrollLeft": 0 }, 500);
                    }
                }

                if (s.activeImage == s.imageArray.length - 1 && s.imageArray.length > 11 && s.btnnextclick == false) {

                    a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details-caption").innerWidth() + 1000 }, 500);
                }



                x.onload = function () { }
            }; if (s.imageArray[s.activeImage] != null) { x.src = s.imageArray[s.activeImage][0]; a("#lightbox-nav-btnPrev").attr("href", "##"); a("#lightbox-nav-btnNext").attr("href", "##"); } else { if (s.activeImage == s.imageArray.length || s.activeImage > s.imageArray.length || s.activeImage == 0 || s.activeImage < 0 || s.activeImage == -1) { if (s.activeImage < 0) { /* s.activeImage = 0;*/s.activeImage = s.imageArray.length - 1; } else {  /*s.activeImage = s.imageArray.length - 1 ; */s.activeImage = 0; } a("#lightbox-loading").css({ 'visibility': 'hidden' }); a("#lightbox-image").css({ 'display': 'inline' }); a("#lightbox-image-details-caption").css({ 'display': 'block' }); /*return false; */ }  /* b() */ }
        } function l(A, D) {
            var x = a("#lightbox-container-image-box").width();
            var C = a("#lightbox-container-image-box").height();

            var B = (A + (s.containerBorderSize * 2));
            var z = (D + (s.containerBorderSize * 2));
            var y = x - B; var E = C - z;

            z = 512;
            B = 512;


//            a("#lightbox-container-image-box").removeAttr('style');
//            a("#lightbox-container-image-box").css({ 'height': '520px' });

            a("#lightbox-container-image").removeAttr('style');
            a("#lightbox-container-image").css({ 'max-height': '100%' });
            a("#lightbox-container-image").css({ 'max-width': '100%' });

            a("#lightbox-image").removeAttr('style');
            a("#lightbox-image").css({ 'max-height': '100%' });
            a("#lightbox-image").css({ 'max-width': '100%' });

            a("#lightbox-nav-btnPrev").css({ 'top': '256px' });
            a("#lightbox-nav-btnNext").css({ 'top': '256px' });

//            $(window).resize(function () {
//                var arrowtop = a("#lightbox-outer-container").height();
//                // z = arrowtop;
//                //alert(z);
//                arrowtop = arrowtop / 2;
//                arrowtop = arrowtop + 10;
//                z = 0;
//                z = $(window).height() / 2;
//                z = z + 100;
//                if ($(window).height() < 520 && $(window).height() < 380) {
//                    //alert($(window).width());
//                    z = z + 128;
//                    a("#lightbox-container-image-box").removeAttr('style');
//                    a("#lightbox-container-image-box").css({ 'height': z });
//                }
//                else {
//                    a("#lightbox-container-image-box").removeAttr('style');
//                    a("#lightbox-container-image-box").css({ 'max-height': '520px' });
//                }
//                //alert(z);
//                // a("#lightbox-nav-btnPrev").removeAttr('style');
//                a("#lightbox-nav-btnPrev").css({ 'top': arrowtop });
//                a("#lightbox-nav-btnNext").css({ 'top': arrowtop });


//            });


        
            a("#lightbox-container-image-box").animate({ 'max-width': '100%',height:z }, s.containerResizeSpeed, function () { g(); a("#lightbox-secNav-btnClose").css({ 'display': 'block' }); /*a("#lightbox-nav-btnPrev").css({ 'visibility': 'visible' });*/a("#lightbox-nav-btnNext").css({ 'visibility': 'visible' }); a("#lightbox-loading").css({ 'visibility': 'hidden' }); }); if ((y == 0) && (E == 0)) { if (a.browser.msie) { q(250) } else { q(100); } }
            // a("#lightbox-container-image-box").animate({ width: B, height: z }, s.containerResizeSpeed, function () { g(); a("#lightbox-secNav-btnClose").css({ 'display': 'block' }); /*a("#lightbox-nav-btnPrev").css({ 'visibility': 'visible' });*/a("#lightbox-nav-btnNext").css({ 'visibility': 'visible' }); a("#lightbox-loading").css({ 'visibility': 'visible' }); }); if ((y == 0) && (E == 0)) { if (a.browser.msie) { q(250) } else { q(100); } }

            ////////            $(window).resize(function () {
            ////////                if ($(window).width() < 822 && $(window).height() < 522) {
            ////////                    //&& $(window).height() < 522
            ////////                    y = a("#lightbox-container-image-box").width() - 100;
            ////////                    E = a("#lightbox-container-image-box").height() - 100;

            ////////                    // alert($(window).height());
            ////////                    var wid = $(window).width() - 100;
            ////////                    var hig = $(window).height() - 100;

            ////////                    var imgwid = $(window).width() - 200;
            ////////                    var imghig = $(window).height() - 200;

            ////////                    var wid1 = $(window).width() - 150;
            ////////                    var hig1 = $(window).height() - 150;

            ////////                    a("#lightbox-container-image").removeAttr('style');
            ////////                    a("#lightbox-container-image").css({ 'max-width': wid });
            ////////                    a("#lightbox-container-image").css({ 'max-height': hig });

            ////////                    // a("#lightbox-outer-container").removeAttr('height');
            ////////                    //a("#lightbox-outer-container").removeAttr('width');

            ////////                    // a("#lightbox-outer-container").removeProperty('height');
            ////////                    // a("#lightbox-outer-container").removeProperty('width');
            ////////                    // $('#lightbox-outer-container').removeCss('width');


            ////////                    a("#lightbox-image").removeAttr('style');
            ////////                    a("#lightbox-image").css({ 'max-height': hig1 });
            ////////                    a("#lightbox-image").css({ 'max-width': wid1 });


            ////////                    a("#lightbox-outer-container").removeAttr('style');
            ////////                    a("#lightbox-outer-container").css({ 'max-width': wid });
            ////////                    a("#lightbox-outer-container").css({ 'max-height': hig });
            ////////                    //alert("inner yes")
            ////////                    a("#lightbox-container-image-box").removeAttr('style');
            ////////                    a("#lightbox-container-image-box").animate({ 'max-width': imgwid, 'max-height': imghig }, s.containerResizeSpeed, function () { g(); a("#lightbox-secNav-btnClose").css({ 'display': 'block' }); /*a("#lightbox-nav-btnPrev").css({ 'visibility': 'visible' });*/a("#lightbox-nav-btnNext").css({ 'visibility': 'visible' }); a("#lightbox-loading").css({ 'visibility': 'visible' }); }); if ((y == 0) && (E == 0)) { if (a.browser.msie) { q(250) } else { q(100); } }
            ////////                }
            ////////                else {
            ////////                  //  s.boolresize = true;
            ////////                                        if ($(window).width() >= 822 && $(window).height() >= 522) {
            ////////                    //                      
            ////////                                          

            ////////                                            a("#lightbox-outer-container").removeAttr('style');
            ////////                                            a("#lightbox-outer-container").css({ 'width': '822px' });
            ////////                                            a("#lightbox-outer-container").css({ 'height': '541px' });

            ////////                                            a("#lightbox-image").removeAttr('style');
            ////////                                            a("#lightbox-image").css({ 'max-height': '512px' });
            ////////                                            a("#lightbox-image").css({ 'max-width': '512px' });

            ////////                                            a("#lightbox-container-image").removeAttr('style');
            ////////                                            a("#lightbox-container-image").css({ 'width': '512px' });
            ////////                                            a("#lightbox-container-image").css({ 'height': '512px' });
            ////////                                            a("#lightbox-container-image-box").removeAttr('style');
            ////////                                            a("#lightbox-container-image-box").animate({ width: B, height: z }, s.containerResizeSpeed, function () { g(); a("#lightbox-secNav-btnClose").css({ 'display': 'block' }); /*a("#lightbox-nav-btnPrev").css({ 'visibility': 'visible' });*/a("#lightbox-nav-btnNext").css({ 'visibility': 'visible' }); a("#lightbox-loading").css({ 'visibility': 'visible' }); }); if ((y == 0) && (E == 0)) { if (a.browser.msie) { q(250) } else { q(100); } }
            ////////                                      }
            ////////                }
            ////////            });
            ////////            // alert($(window).width());
            ////////            // alert($(window).height());

            ////////            if ($(window).width() > 822 && $(window).height() > 522 ) {


            ////////                var wid = $(window).width() - 100;
            ////////                var hig = $(window).height() - 100;

            ////////                var imgwid = $(window).width() - 200;
            ////////                var imghig = $(window).height() - 200;

            ////////                var wid1 = $(window).width() - 150;
            ////////                var hig1 = $(window).height() - 150;


            ////////               
            ////////                a("#lightbox-outer-container").removeAttr('style');
            ////////                a("#lightbox-outer-container").css({ 'max-width': '822px' });
            ////////                a("#lightbox-outer-container").css({ 'max-height': '541px' });

            ////////                a("#lightbox-image").removeAttr('style');
            ////////                a("#lightbox-image").css({ 'max-height': '512px' });
            ////////                a("#lightbox-image").css({ 'max-width': '512px' });

            ////////                a("#lightbox-container-image").removeAttr('style');
            ////////                a("#lightbox-container-image").css({ 'width': '512px' });
            ////////                a("#lightbox-container-image").css({ 'height': '512px' });

            ////////                a("#lightbox-container-image-box").removeAttr('style');
            ////////                a("#lightbox-container-image-box").animate({ width: B, height: z }, s.containerResizeSpeed, function () { g(); a("#lightbox-secNav-btnClose").css({ 'display': 'block' }); /*a("#lightbox-nav-btnPrev").css({ 'visibility': 'visible' });*/a("#lightbox-nav-btnNext").css({ 'visibility': 'visible' }); a("#lightbox-loading").css({ 'visibility': 'visible' }); }); if ((y == 0) && (E == 0)) { if (a.browser.msie) { q(250) } else { q(100); } }

            ////////                //                a("#lightbox-container-image-box").animate({ width: B, height: z }, s.containerResizeSpeed, function () { g(); a("#lightbox-secNav-btnClose").css({ 'display': 'block' }); /*a("#lightbox-nav-btnPrev").css({ 'visibility': 'visible' });*/a("#lightbox-nav-btnNext").css({ 'visibility': 'visible' }); a("#lightbox-loading").css({ 'visibility': 'visible' }); }); if ((y == 0) && (E == 0)) { if (a.browser.msie) { q(250) } else { q(100); } }

            ////////                //                a("#lightbox-outer-container").removeAttr('style');
            ////////                //                a("#lightbox-outer-container").css({ 'width': '822px' });
            ////////                //                a("#lightbox-outer-container").css({ 'height': '541px' });

            ////////                //                a("#lightbox-image").removeAttr('style');
            ////////                //                a("#lightbox-image").css({ 'max-height': '512px' });
            ////////                //                a("#lightbox-image").css({ 'max-width': '512px' });

            ////////                //                a("#lightbox-container-image").removeAttr('style');
            ////////                //                a("#lightbox-container-image").css({ 'width': '512px' });
            ////////                //                a("#lightbox-container-image").css({ 'height': '512px' });
            ////////            }

            //////////            if ($(window).width() > 822 && $(window).height() > 522 && s.boolresize == false) {
            //////////                a("#lightbox-container-image-box").animate({ width: B, height: z }, s.containerResizeSpeed, function () { g(); a("#lightbox-secNav-btnClose").css({ 'display': 'block' }); /*a("#lightbox-nav-btnPrev").css({ 'visibility': 'visible' });*/a("#lightbox-nav-btnNext").css({ 'visibility': 'visible' }); a("#lightbox-loading").css({ 'visibility': 'visible' }); }); if ((y == 0) && (E == 0)) { if (a.browser.msie) { q(250) } else { q(100); } }

            //////////                a("#lightbox-outer-container").removeAttr('style');
            //////////                a("#lightbox-outer-container").css({ 'width': '822px' });
            //////////                a("#lightbox-outer-container").css({ 'height': '541px' });

            //////////                a("#lightbox-image").removeAttr('style');
            //////////                a("#lightbox-image").css({ 'max-height': '512px' });
            //////////                a("#lightbox-image").css({ 'max-width': '512px' });

            //////////                a("#lightbox-container-image").removeAttr('style');
            //////////                a("#lightbox-container-image").css({ 'width': '512px' });
            //////////                a("#lightbox-container-image").css({ 'height': '512px' });
            //////////            }


            ////////            //            else {
            ////////            //                a("#lightbox-container-image-box").animate({ 'max-width': B, 'max-height': z }, s.containerResizeSpeed, function () { g(); a("#lightbox-secNav-btnClose").css({ 'display': 'block' }); /*a("#lightbox-nav-btnPrev").css({ 'visibility': 'visible' });*/a("#lightbox-nav-btnNext").css({ 'visibility': 'visible' }); a("#lightbox-loading").css({ 'visibility': 'visible' }); }); if ((y == 0) && (E == 0)) { if (a.browser.msie) { q(250) } else { q(100); } }
            ////////            //            }

        } function g() { a("#lightbox-loading").hide(); a("#lightbox-image").fadeIn(function () { }); u(); } function k() {
            a("#lightbox-container-image-data-box").slideDown("slow"); a("#lightbox-image-details-caption").hide(); if (s.imageArray[s.activeImage] != undefined) {

                var x = ""; for (var y = 0; y < s.imageArray.length; y++) {

                    (s.activeImage == y) ? hover = "hover" : hover = ""; x += '<a href="' + s.imageArray[y][0] + '" id="' + y + '" class="thumb ' + hover + '"   ><img src="' +
s.imageArray[y][1] + '"  style="max-width:50px;max-height:50px;"/></a>'

                }

                a(".thumb").live("click", function () { a("#lightbox-loading").css({ 'visibility': 'hidden' }); s.activeImage = a(this).attr("id") * 1; o(); return false }); a("#lightbox-image-details-caption").html(x).show()
            }

        }

        function w() {

            a("#lightbox-nav").show(); a("#lightbox-nav-btnPrev").css({ background: "transparent url(" + s.imageBtnPrev + ") 10px 48% no-repeat" }); a("#lightbox-nav-btnNext").css({ background: "transparent url(" + s.imageBtnNext + ") 0px 48% no-repeat" }); if (s.activeImage == 0) { objPrev = new Image(); objPrev.src = s.imageArray[s.imageArray.length - 1][0]; a("#lightbox-image-up").css({ 'visibility': 'hidden' }); a("#lightbox-image-up").css({ 'cursor': 'default' }); } if (s.activeImage != 0) {
                a("#lightbox-nav-btnPrev").show();




                var deaimg = 13;
                var cuaimg = s.activeImage - deaimg;

                if (s.activeImage < 0) {
                    s.activeImage = s.imageArray.length - 1;
                    k();
                    //  a("#lightbox-image-details-caption").show();
                    if (s.imageArray.length > 11) {
                        //alert("aaa");
                        a("#lightbox-image-details").animate({ "scrollLeft": a("#lightbox-image-details-caption").innerWidth() + 1000 }, 500);
                    }

                }
                if (s.activeImage == s.imageArray.length) {
                    s.activeImage = 0;
                    k();
                    // a("#lightbox-image-details-caption").show();
                    a("#lightbox-image-details").animate({ "scrollLeft": 0 }, 500);
                }



                if (s.activeImage == 11) {
                    s.preclick = false;
                }

                if (s.activeImage == s.imageArray.length - 1) {
                    a("lightbox-image-down").css({ 'visibility': 'hidden' });
                    a("lightbox-image-down").css({ 'cursor': 'pointer' });
                }



                //           if( s.activeImage == s.imageArray.length - 1)
                //           {
                //            
                //            $("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() }, 0);
                //           }

                if (s.activeImage > 11 && s.preclick == false && s.activeImage < s.imageArray.length - 1) {
                    // alert("next");
                    //a("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() + 90 }, 500);

                    if (s.activeImage > 11 && s.activeImage <= 15) { a("#lightbox-image-details").animate({ "scrollLeft": 300 }, 500); }
                    if (s.activeImage > 15 && s.activeImage <= 20) { a("#lightbox-image-details").animate({ "scrollLeft": 650 }, 500); }
                    if (s.activeImage > 20 && s.activeImage <= 25) {
                        a("#lightbox-image-details").animate({ "scrollLeft": 1000 }, 500);
                    }
                    if (s.activeImage > 25 && s.activeImage <= 30) {
                        a("#lightbox-image-details").animate({ "scrollLeft": 1350 }, 500);
                    }
                    if (s.activeImage > 30 && s.activeImage <= 35) {
                        a("#lightbox-image-details").animate({ "scrollLeft": 1700 }, 500);
                    }
                    if (s.activeImage > 35 && s.activeImage <= 40) {
                        a("#lightbox-image-details").animate({ "scrollLeft": 2050 }, 500);
                    }
                    if (s.activeImage > 40 && s.activeImage <= 45) {
                        a("#lightbox-image-details").animate({ "scrollLeft": 2400 }, 500);
                    }
                    if (s.activeImage > 50 && s.activeImage <= 55) {
                        a("#lightbox-image-details").animate({ "scrollLeft": 2750 }, 500);
                    }
                    if (s.activeImage > 55) {
                        a("#lightbox-image-details").animate({ "scrollLeft": 3000 }, 500);
                    }
                }

                if (s.activeImage > 11 && s.imageArray.length > 11) {
                    a("#lightbox-image-up").css({ 'visibility': 'visible' });
                    a("#lightbox-image-up").css({ 'cursor': 'pointer' });
                }
                if (s.activeImage <= 11 && s.imageArray.length > 11) {
                    a("#lightbox-image-up").css({ 'visibility': 'hidden' });
                    a("#lightbox-image-up").css({ 'cursor': 'default' });

                    if (s.activeImage <= 5) {
                        a("#lightbox-image-details").animate({ "scrollLeft": 0 }, 500);
                    }
                    if (s.activeImage > 5 && s.activeImage <= 11) {
                        //a("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() - 90 }, 500);
                        a("#lightbox-image-details").animate({ "scrollLeft": 0 }, 500);
                    }
                }



                if (s.hoverNavigation) { a("#lightbox-nav-btnPrev").css({ background: "url(" + s.imageBtnPrev + ") 10px 48% no-repeat" }).unbind().bind("click", function () { s.activeImage = s.activeImage - 1; o(); return false }) } else {
                    a("#lightbox-nav-btnPrev").unbind().hover(function () { a(this).css({ background: "url(" + s.imageBtnPrevHover + ") 10px 48% no-repeat" }) },
function () { a(this).css({ background: "transparent url(" + s.imageBtnPrev + ") 10px 48% no-repeat" }) }).show().bind("click", function () { if (s.activeImage > 11) { s.preclick = true; /*a("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() - 90 }, 500); */if (s.activeImage > 11 && s.activeImage <= 15) { a("#lightbox-image-details").animate({ "scrollLeft": 300 }, 500); } if (s.activeImage > 15 && s.activeImage <= 20) { a("#lightbox-image-details").animate({ "scrollLeft": 650 }, 500); } if (s.activeImage > 20 && s.activeImage <= 25) { a("#lightbox-image-details").animate({ "scrollLeft": 1000 }, 500); } if (s.activeImage > 25 && s.activeImage <= 30) { a("#lightbox-image-details").animate({ "scrollLeft": 1350 }, 500); } if (s.activeImage > 30 && s.activeImage <= 35) { a("#lightbox-image-details").animate({ "scrollLeft": 1700 }, 500); } if (s.activeImage > 35 && s.activeImage <= 40) { a("#lightbox-image-details").animate({ "scrollLeft": 2050 }, 500); } if (s.activeImage > 40 && s.activeImage <= 45) { a("#lightbox-image-details").animate({ "scrollLeft": 2400 }, 500); } if (s.activeImage > 50 && s.activeImage <= 55) { a("#lightbox-image-details").animate({ "scrollLeft": 2750 }, 500); } if (s.activeImage > 55) { a("#lightbox-image-details").animate({ "scrollLeft": 3000 }, 500); } } s.activeImage = s.activeImage - 1; o(); return false })
                }
            } else {
                if (s.imageArray.length == 1) { a("#lightbox-nav-btnPrev").css({ 'visibility': 'hidden' }); a("#lightbox-nav-btnPrev").css({ 'display': 'none' }); a("#lightbox-nav-btnNext").css({ 'display': 'none' }); } /*a("#lightbox-nav-btnPrev").hide()*/a("#lightbox-nav-btnPrev").css({ 'cursor': 'pointer' });
                /*a("#lightbox-nav-btnPrev").css({ background: "transparent url(" + s.imgbtnpredis + ") 0px 48% no-repeat" }); */
            } /*if(s.activeImage == (s.imageArray.length - 1)){alert("iii");}*/if (s.activeImage != (s.imageArray.length - 1)) {

                a("#lightbox-nav-btnNext").css({ 'cursor': 'pointer' });
                a("#lightbox-nav-btnNext").css({ 'visibility': 'visible' });
                a("#lightbox-nav-btnNext").show();

                if (s.hoverNavigation) { a("#lightbox-nav-btnNext").css({ background: "url(" + s.imageBtnNext + ") 0px 48% no-repeat" }).unbind().bind("click", function () { s.activeImage = s.activeImage + 1; o(); return false }) } else { a("#lightbox-nav-btnNext").unbind().hover(function () { a(this).css({ background: "url(" + s.imageBtnNextHover + ") 0px 48% no-repeat" }) }, function () { a(this).css({ background: "transparent url(" + s.imageBtnNext + ") 0px 48% no-repeat" }) }).show().bind("click", function () { s.preclick = false;  /*alert("inner next"); */s.activeImage = s.activeImage + 1; o(); return false }) }
            }
            else {
                /* a("#lightbox-nav-btnNext").hide() */a("#lightbox-nav-btnNext").css({ 'cursor': 'pointer' });
                /* a("#lightbox-nav-btnNext").css({ background: "transparent url(" + s.imgbtnnextdis + ") 0px 48% no-repeat" }); */
            } p()
        } function p() { a(document).keydown(function (x) { d(x) }) } function e() { a(document).unbind() } function d(x) { if (x == null) { keycode = event.keyCode; escapeKey = 27 } else { keycode = x.keyCode; escapeKey = x.DOM_VK_ESCAPE } key = String.fromCharCode(keycode).toLowerCase(); if ((key == s.keyToClose) || (key == "x") || (keycode == escapeKey)) { /* alert("1"); */b() } if ((key == s.keyToPrev) || (keycode == 37)) { if (s.activeImage != 0) {  s.activeImage = s.activeImage - 1; o(); e() } } if ((key == s.keyToNext) || (keycode == 39)) { if (s.activeImage != (s.imageArray.length - 1)) {  s.activeImage = s.activeImage + 1; o(); e() } } } function u() {
            if (s.activeImage == (s.imageArray.length - 1)) { /*alert("last");*/ /*objNext = new Image(); objNext.src = s.imageArray[0][0]; alert(objNext.src);*/ } if ((s.imageArray.length - 1) > s.activeImage) { /* alert("next"); objNext = new Image(); objNext.src = s.imageArray[s.activeImage + 1][0];*/ }
            if (s.activeImage > 0) { /*alert("1");*/ /*  objPrev = new Image(); objPrev.src = s.imageArray[s.activeImage - 1][0] ;*/ /*alert(s.activeImage);*/enex();  /*s.activeImage = s.activeImage ; o(); return false;*/ }
            else { /*alert("2");*/ /* objPrev = new Image(); objPrev.src = s.imageArray[s.imageArray.length - 1][0] ;*/epre(); }
        }


        function enex() {
            //alert(s.btnnextclick);
            // alert(s.activeImage);
            if (s.btnnextclick == false) {

                s.btnnextclick = true;
                a("#lightbox-nav-btnNext").show().bind("click", function () { s.activeImage = 0; o(); return false });
            }
        }
        function epre() {
            a("#lightbox-nav-btnPrev").show().bind("click", function () { s.activeImage = s.imageArray.length - 1; o(); return false });
        }

        function b() {
            a("#jquery-lightbox").remove(); a("#jquery-overlay").fadeOut(function () { a("#jquery-overlay").remove(); }); a("embed, object, select").css({ visibility: "visible" })
        } function f() {
            /* alert("11");*/s.btnnextclick = false;
            var z, x; if (window.innerHeight && window.scrollMaxY) { z = window.innerWidth + window.scrollMaxX; x = window.innerHeight + window.scrollMaxY } else { if (document.body.scrollHeight > document.body.offsetHeight) { z = document.body.scrollWidth; x = document.body.scrollHeight } else { z = document.body.offsetWidth; x = document.body.offsetHeight } } var y, A; if (self.innerHeight) { if (document.documentElement.clientWidth) { y = document.documentElement.clientWidth } else { y = self.innerWidth } A = self.innerHeight } else {
                if (document.documentElement && document.documentElement.clientHeight) { y = document.documentElement.clientWidth; A = document.documentElement.clientHeight } else
                { if (document.body) { y = document.body.clientWidth; A = document.body.clientHeight } }
            } if (x < A) { pageHeight = A } else { pageHeight = x } if (z < y) { pageWidth = z } else { pageWidth = y } arrayPageSize = new Array(pageWidth, pageHeight, y, A); return arrayPageSize
        } function h() { var y, x; if (self.pageYOffset) { x = self.pageYOffset; y = self.pageXOffset } else { if (document.documentElement && document.documentElement.scrollTop) { x = document.documentElement.scrollTop; y = document.documentElement.scrollLeft } else { if (document.body) { x = document.body.scrollTop; y = document.body.scrollLeft } } } arrayPageScroll = new Array(y, x); return arrayPageScroll } function q(z) { var y = new Date(); x = null; do { var x = new Date() } while (x - y < z) } return this.unbind("click").click(v)
    }; a(function () { a("#lboximgpopup a").lightBox();  /* a("#gallery A").lightBox();a("#product-image a").lightBox();*//*$(window).resize(function () { if ($(window).width() < 822 && $(window).height() < 522) { alert("yes") } });*/ });
})(jQuery);                                                    (function (a) {
    a.fn.hoverIntent = function (l, k) {
        var o = { sensitivity: 7, interval: 100, timeout: 0 }; o = a.extend(o, k ? { over: l, out: k} : l); var q, p, h, d; var e = function (f) { q = f.pageX; p = f.pageY }; var c = function (g, f) { f.hoverIntent_t = clearTimeout(f.hoverIntent_t); if ((Math.abs(h - q) + Math.abs(d - p)) < o.sensitivity) { a(f).unbind("mousemove", e); f.hoverIntent_s = 1; return o.over.apply(f, [g]) } else { h = q; d = p; f.hoverIntent_t = setTimeout(function () { c(g, f) }, o.interval) } }; var j = function (g, f) { f.hoverIntent_t = clearTimeout(f.hoverIntent_t); f.hoverIntent_s = 0; return o.out.apply(f, [g]) }; var b = function (s) {
            s.type = (s.type == "mouseenter") ? "mouseover" : s.type; s.type = (s.type == "mouseleave") ? "mouseout" : s.type; var r = (s.type == "mouseover" ? s.fromElement : s.toElement) || s.relatedTarget; while (r && r != this) { try { r = r.parentNode } catch (s) { r = this } } if (r == this) { return false } var g = jQuery.extend({}, s);
            var f = this; if (f.hoverIntent_t) { f.hoverIntent_t = clearTimeout(f.hoverIntent_t) } if (s.type == "mouseover") { h = g.pageX; d = g.pageY; a(f).bind("mousemove", e); if (f.hoverIntent_s != 1) { f.hoverIntent_t = setTimeout(function () { c(g, f) }, o.interval) } } else { a(f).unbind("mousemove", e); if (f.hoverIntent_s == 1) { f.hoverIntent_t = setTimeout(function () { j(g, f) }, o.timeout) } }
        }; return this.mouseover(b).mouseout(b)
    }
})(jQuery);








