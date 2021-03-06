/*
* Thickbox 3.1 - One Box To Rule Them All.
* By Cody Lindley (http://www.codylindley.com)
* Copyright (c) 2007 cody lindley
* Licensed under the MIT License: http://www.opensource.org/licenses/mit-license.php
 
*
*	Modified by JIN Weijie
*	Added previous/next by clicking the current image. Please visit http://www.jinweijie.com for detail
*/

var thickboxL10n = {
    next: "Next &gt;",
    prev: "&lt; Prev",
    image: "Image",
    of: "of",
    close: "Close",
    noiframes: "This feature requires inline frames. You have iframes disabled or your browser does not support them."
};

if (typeof tb_pathToImage != 'string') {
    //var tb_pathToImage = "../../../../../../../../../../images/loadingAnimation.gif";
    var tb_pathToImage = "http://cdn.wes.com.au/wag/images/loadingAnimation.gif";
}
if (typeof tb_closeImage != 'string') {
    var tb_closeImage = "http://cdn.wes.com.au/wag/images/closebtn.png";
}

if (typeof tb_leftArrow != 'string') {
    var tb_leftArrow = "http://cdn.wes.com.au/wag/images/left.gif";
}

if (typeof tb_rightArrow != 'string') {
    var tb_rightArrow = "http://cdn.wes.com.au/wag/images/right.gif";
}


//on page load call tb_init
jQuery(document).ready(function () {
    tb_init('a.thickbox, area.thickbox, input.thickbox'); //pass where to apply thickbox
    imgLoader = new Image(); // preload image
    imgLoader.src = tb_pathToImage;
});

//add thickbox to href & area elements that have a class of .thickbox
function tb_init(domChunk) {
   // jQuery(domChunk).live('click', tb_click);
}

function tb_click() {
    var t = this.title || this.name || null;
    var a = this.href || this.alt;
    var g = this.rel || false;
    tb_show(t, a, g);
    this.blur();
    return false;
}

function tb_show(caption, url, imageGroup) {//function called when the user clicks on a thickbox link

    try {
        if (typeof document.body.style.maxHeight === "undefined") {//if IE 6
            jQuery("body", "html").css({ height: "100%", width: "100%" });
            jQuery("html").css("overflow", "hidden");
            if (document.getElementById("TB_HideSelect") === null) {//iframe to hide select elements in ie6
                jQuery("body").append("<iframe id='TB_HideSelect'>" + thickboxL10n.noiframes + "</iframe><div id='TB_overlay' ></div><div id='TB_window'></div>");
                jQuery("#TB_overlay").click(tb_remove);
            }
        } else {//all others
            if (document.getElementById("TB_overlay") === null) {
                var ovstyle = "display: block; background: none repeat scroll 0px 0px rgba(0, 0, 0, 0.6);";

                jQuery("body").append("<div id='TB_overlay' style='" + ovstyle + "'><div id='TB_window' ></div></div>");
                jQuery("#TB_overlay").click(tb_remove);
            }
        }

        if (tb_detectMacXFF()) {
            jQuery("#TB_overlay").addClass("TB_overlayMacFFBGHack"); //use png overlay so hide flash
        } else {
            jQuery("#TB_overlay").addClass("modal fade bs-example-modal-lg in"); //use background and opacity 
            //  jQuery("#TB_window").addClass("modal-dialog modal-lg");  
            jQuery("#TB_window").addClass("modal-dialog");
        }

        if (caption === null) { caption = ""; }
        jQuery("body").append("<div id='TB_load'> <span class='TB_load_content'> Please Wait...<br/><br/>Adding Item Cart </span><br/><br/><br/><br/><img src='" + imgLoader.src + "' /></div>"); //add loader to the page
        jQuery('#TB_load').show(); //show loader

        //jQuery("#TB_window").append("<img src='" + tb_closeImage + "' style='position:absolute;right:-20px;top:-22px;cursor: pointer; cursor: hand;' onClick='tb_remove()' />"); //add loader to the page

       // return;

        var baseURL;
        if (url.indexOf("?") !== -1) { //ff there is a query string involved
            baseURL = url.substr(0, url.indexOf("?"));
        } else {
            baseURL = url;
        }

        var urlString = /\.jpg$|\.jpeg$|\.png$|\.gif$|\.bmp$/;
        var urlType = baseURL.toLowerCase().match(urlString);

        if (urlType == '.jpg' || urlType == '.jpeg' || urlType == '.png' || urlType == '.gif' || urlType == '.bmp') {//code to show images

            TB_PrevCaption = "";
            TB_PrevURL = "";
            TB_PrevHTML = "";
            TB_NextCaption = "";
            TB_NextURL = "";
            TB_NextHTML = "";
            TB_imageCount = "";
            TB_FoundURL = false;
            if (imageGroup) {
                TB_TempArray = jQuery("a[rel=" + imageGroup + "]").get();
                for (TB_Counter = 0; ((TB_Counter < TB_TempArray.length) && (TB_NextHTML === "")); TB_Counter++) {
                    var urlTypeTemp = TB_TempArray[TB_Counter].href.toLowerCase().match(urlString);
                    if (!(TB_TempArray[TB_Counter].href == url)) {
                        if (TB_FoundURL) {
                            TB_NextCaption = TB_TempArray[TB_Counter].title;
                            TB_NextURL = TB_TempArray[TB_Counter].href;
                            TB_NextHTML = "<span id='TB_next'>&nbsp;&nbsp;<a href='#'>" + thickboxL10n.next + "</a></span>";
                        } else {
                            TB_PrevCaption = TB_TempArray[TB_Counter].title;
                            TB_PrevURL = TB_TempArray[TB_Counter].href;
                            TB_PrevHTML = "<span id='TB_prev'>&nbsp;&nbsp;<a href='#'>" + thickboxL10n.prev + "</a></span>";
                        }
                    } else {
                        TB_FoundURL = true;
                        TB_imageCount = thickboxL10n.image + ' ' + (TB_Counter + 1) + ' ' + thickboxL10n.of + ' ' + (TB_TempArray.length);
                    }
                }
            }


            imgPreloader = new Image();
            prevImg = new Image();
            nextImg = new Image();
            imgPreloader.onload = function () {
                imgPreloader.onload = null;

                var tb_links = jQuery('a[class="thickbox"]');
                var i = -1;
                tb_links.each(function (n) { if (this.href == imgPreloader.src) { i = n; } });


                if (i != -1) {
                    if (i > 0) { prevImg.src = tb_links[i - 1].href; }
                    if (i + 1 < tb_links.length) {

                        var imgTemp1 = new Image();
                        imgTemp1.src = tb_links[i + 1].href;

                        if (tb_links[i + 2]) {
                            var imgTemp2 = new Image();
                            imgTemp2.src = tb_links[i + 2].href;
                        }

                        if (tb_links[i + 3]) {
                            var imgTemp3 = new Image();
                            imgTemp3.src = tb_links[i + 3].href;
                        }
                    }
                }


                // imgPreloader = new Image();
                // imgPreloader.onload = function(){
                //  imgPreloader.onload = null;

                // Resizing large images - orginal by Christian Montoya edited by me.
                var pagesize = tb_getPageSize();
                var x = pagesize[0] - 150;
                var y = pagesize[1] - 150;
                var imageWidth = imgPreloader.width;
                var imageHeight = imgPreloader.height;
                if (imageWidth > x) {
                    imageHeight = imageHeight * (x / imageWidth);
                    imageWidth = x;
                    if (imageHeight > y) {
                        imageWidth = imageWidth * (y / imageHeight);
                        imageHeight = y;
                    }
                } else if (imageHeight > y) {
                    imageWidth = imageWidth * (y / imageHeight);
                    imageHeight = y;
                    if (imageWidth > x) {
                        imageHeight = imageHeight * (x / imageWidth);
                        imageWidth = x;
                    }
                }
                // End Resizing

                TB_WIDTH = imageWidth + 30;
                TB_HEIGHT = imageHeight + 60;
                jQuery("#TB_window").append("<img id='imgLeftArrow' src='" + tb_leftArrow + "' border='0' style='display:none;'/><img id='imgRightArrow' src='" + tb_rightArrow + "' border='0' style='display:none;'/><div id='divNavControl'><div id='divPre'></div><div id='divNext'></div></div><a href='#' id='TB_nextPIC' title='" + thickboxL10n.next + "'><img id='TB_Image' src='" + url + "' width='" + imageWidth + "' height='" + imageHeight + "' alt='" + caption + "'/></a>" + "<div id='TB_caption'>" + caption + "<div id='TB_secondLine'>" + TB_imageCount + TB_PrevHTML + TB_NextHTML + "</div></div><div id='TB_closeWindow'><a href='#' id='TB_closeWindowButton' title='" + thickboxL10n.close + "'><img src='" + tb_closeImage + "' /></a></div>");

                jQuery("#TB_closeWindowButton").click(tb_remove);

                function hideArrows() {
                    jQuery("#imgLeftArrow").css({ display: 'none' });
                    jQuery("#imgRightArrow").css({ display: 'none' });
                }

                jQuery("#divNavControl").css({ height: imageHeight + 'px', width: imageWidth + 'px', left: '15px', top: '15px' });

                if (!(TB_PrevHTML === "")) {
                    function goPrev() {
                        if (jQuery(document).unbind("click", goPrev)) { jQuery(document).unbind("click", goPrev); }
                        jQuery("#TB_window").remove();
                        jQuery("body").append("<div id='TB_window'></div>");
                        tb_show(TB_PrevCaption, TB_PrevURL, imageGroup);
                        return false;
                    }
                    jQuery("#TB_prev").click(goPrev);

                    function showLeftArrow() {
                        jQuery("#imgLeftArrow").css({ display: '' });
                        jQuery("#imgRightArrow").css({ display: 'none' });
                    }


                    jQuery("#divPre").click(goPrev);
                    jQuery("#divPre").mouseover(showLeftArrow);
                    jQuery("#divPre").mouseout(hideArrows);
                }

                if (!(TB_NextHTML === "")) {
                    function goNext() {
                        jQuery("#TB_window").remove();
                        jQuery("body").append("<div id='TB_window'></div>");
                        tb_show(TB_NextCaption, TB_NextURL, imageGroup);
                        return false;
                    }
                    jQuery("#TB_next").click(goNext);
                    jQuery("#TB_nextPIC").click(goNext);


                    jQuery("#divNext").click(goNext);

                    function showRightArrow() {
                        jQuery("#imgLeftArrow").css({ display: 'none' });
                        jQuery("#imgRightArrow").css({ display: '' });
                    }
                    jQuery("#divNext").mouseover(showRightArrow);
                    jQuery("#divNext").mouseout(hideArrows);

                }


                document.onkeydown = function (e) {
                    if (e == null) { // ie
                        keycode = event.keyCode;
                    } else { // mozilla
                        keycode = e.which;
                    }
                    if (keycode == 27) { // close
                        tb_remove();
                    } else if (keycode == 190) { // display previous image
                        if (!(TB_NextHTML == "")) {
                            document.onkeydown = "";
                            goNext();
                        }
                    } else if (keycode == 188) { // display next image
                        if (!(TB_PrevHTML == "")) {
                            document.onkeydown = "";
                            goPrev();
                        }
                    }
                };

                tb_position();
                jQuery("#TB_load").remove();
                jQuery("#TB_ImageOff").click(tb_remove);
                jQuery("#TB_window").css({ display: "block" }); //for safari using css instead of show
            };

            imgPreloader.src = url;
        } else {//code to show html

            var queryString = url.replace(/^[^\?]+\??/, '');
            var params = tb_parseQuery(queryString);

            TB_WIDTH = (params['width'] * 1) + 30 || 630; //defaults to 630 if no paramaters were added to URL
            TB_HEIGHT = (params['height'] * 1) + 40 || 440; //defaults to 440 if no paramaters were added to URL
            ajaxContentW = TB_WIDTH - 30;
            ajaxContentH = TB_HEIGHT - 45;

            if (url.indexOf('TB_iframe') != -1) {// either iframe or ajax window
                urlNoQuery = url.split('TB_');
                jQuery("#TB_iframeContent").remove();
                if (params['modal'] != "true") {//iframe no modal
                    jQuery("#TB_window").append("<div id='TB_title'><div id='TB_ajaxWindowTitle'>" + caption + "</div><div id='TB_closeAjaxWindow'><a href='#' id='TB_closeWindowButton' title='" + thickboxL10n.close + "'><img src='" + tb_closeImage + "' /></a></div></div><iframe frameborder='0' hspace='0' src='" + urlNoQuery[0] + "' id='TB_iframeContent' name='TB_iframeContent" + Math.round(Math.random() * 1000) + "' onload='tb_showIframe()' style='width:" + (ajaxContentW + 29) + "px;height:" + (ajaxContentH + 17) + "px;' >" + thickboxL10n.noiframes + "</iframe>");
                } else {//iframe modal
                    jQuery("#TB_overlay").unbind();
                    jQuery("#TB_window").append("<iframe frameborder='0' hspace='0' src='" + urlNoQuery[0] + "' id='TB_iframeContent' name='TB_iframeContent" + Math.round(Math.random() * 1000) + "' onload='tb_showIframe()' style='width:" + (ajaxContentW + 29) + "px;height:" + (ajaxContentH + 17) + "px;'>" + thickboxL10n.noiframes + "</iframe>");
                }
            } else {// not an iframe, ajax
                if (jQuery("#TB_window").css("display") != "block") {
                    if (params['modal'] != "true") {//ajax no modal
                        jQuery("#TB_window").append("<div id='TB_title'><div id='TB_ajaxWindowTitle'>" + caption + "</div><div id='TB_closeAjaxWindow'><a href='#' id='TB_closeWindowButton'><img src='" + tb_closeImage + "' /></a></div></div><div id='TB_ajaxContent' style='width:" + ajaxContentW + "px;height:" + ajaxContentH + "px'></div>");
                    } else {//ajax modal
                        jQuery("#TB_overlay").unbind();
                        jQuery("#TB_window").append("<div id='TB_ajaxContent' class='modal-content' ></div>");
                    }
                } else {//this means the window is already up, we are just loading new content via ajax
                    // jQuery("#TB_ajaxContent")[0].style.width = ajaxContentW + "px";
                    //jQuery("#TB_ajaxContent")[0].style.height = ajaxContentH + "px";
                    //jQuery("#TB_ajaxContent")[0].scrollTop = 0;
                    jQuery("#TB_ajaxWindowTitle").html(caption);
                }
            }

            jQuery("#TB_closeWindowButton").click(tb_remove);

            if (url.indexOf('TB_inline') != -1) {
                jQuery("#TB_ajaxContent").append(jQuery('#' + params['inlineId']).children());
                jQuery("#TB_window").unload(function () {
                    jQuery('#' + params['inlineId']).append(jQuery("#TB_ajaxContent").children()); // move elements back when you're finished
                });
                tb_position();
                jQuery("#TB_load").remove();
                jQuery("#TB_window").css({ display: "block" });
            } else if (url.indexOf('TB_iframe') != -1) {
                tb_position();
                if (jQuery.browser.safari) {//safari needs help because it will not fire iframe onload
                    jQuery("#TB_load").remove();
                    jQuery("#TB_window").css({ display: "block" });
                }
            } else {
                jQuery("#TB_ajaxContent").load(url += "&random=" + (new Date().getTime()), function () {//to do a post change this load method
                    tb_position();
                    jQuery("#TB_load").remove();
                    tb_init("#TB_ajaxContent a.thickbox");
                    jQuery("#TB_window").css({ display: "block" });
                });
            }

        }

        if (!params['modal']) {
            document.onkeyup = function (e) {
                if (e == null) { // ie
                    keycode = event.keyCode;
                } else { // mozilla
                    keycode = e.which;
                }
                if (keycode == 27) { // close
                    tb_remove();
                }
            };
        }

    } catch (e) {
        //nothing here
    }
}

//helper functions below
function tb_showIframe() {
    jQuery("#TB_load").remove();
    jQuery("#TB_window").css({ display: "block" });

}

function tb_remove() {
    jQuery("body").removeClass("modal-open");
    jQuery("#TB_imageOff").unbind("click");
    jQuery("#TB_closeWindowButton").unbind("click");
    jQuery("#TB_window").fadeOut("fast", function () { jQuery('#TB_window,#TB_overlay,#TB_HideSelect').trigger("unload").unbind().remove(); });
    jQuery("#TB_load").remove();
    if (typeof document.body.style.maxHeight == "undefined") {//if IE 6
        jQuery("body", "html").css({ height: "auto", width: "auto" });
        jQuery("html").css("overflow", "");
    }
    document.onkeydown = "";
    document.onkeyup = "";
    return false;
}

function tb_position() {
    var isIE6 = typeof document.body.style.maxHeight === "undefined";
    //  jQuery("#TB_window").css({ marginLeft: '-' + parseInt((TB_WIDTH / 2), 10) + 'px', width: TB_WIDTH + 'px' });
    if (!isIE6) { // take away IE6
        // jQuery("#TB_window").css({ marginTop: '-' + parseInt((TB_HEIGHT / 2), 10) + 'px' });
    }
}

function tb_parseQuery(query) {
    var Params = {};
    if (!query) { return Params; } // return empty object
    var Pairs = query.split(/[;&]/);
    for (var i = 0; i < Pairs.length; i++) {
        var KeyVal = Pairs[i].split('=');
        if (!KeyVal || KeyVal.length != 2) { continue; }
        var key = unescape(KeyVal[0]);
        var val = unescape(KeyVal[1]);
        val = val.replace(/\+/g, ' ');
        Params[key] = val;
    }
    return Params;
}

function tb_getPageSize() {
    var de = document.documentElement;
    var w = window.innerWidth || self.innerWidth || (de && de.clientWidth) || document.body.clientWidth;
    var h = window.innerHeight || self.innerHeight || (de && de.clientHeight) || document.body.clientHeight;
    arrayPageSize = [w, h];
    return arrayPageSize;
}

function tb_detectMacXFF() {
    var userAgent = navigator.userAgent.toLowerCase();
    if (userAgent.indexOf('mac') != -1 && userAgent.indexOf('firefox') != -1) {
        return true;
    }
}


function CallProductPopup(orgurl,buyvalue, pid, qtyval, tOrderID, fid) {

    var url = "";
    if (orgurl.toLowerCase().indexOf("/mpl") != -1 || orgurl.toLowerCase().indexOf("/mpd") != -1 || orgurl.toLowerCase().indexOf("/mct") != -1 || orgurl.toLowerCase().indexOf("/mps") != -1 || orgurl.toLowerCase().indexOf("/mfl") != -1) {
        
        if (tOrderID != null && parseInt(tOrderID) > 0) {
            url = orgurl + "mOrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval.trim() + "&ORDER_ID=" + tOrderID + "&popup=true&modal=true&width=350&height=380&fid=" + fid;

        }
        else {
            url = orgurl + "mOrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval.trim() + "&popup=true&modal=true&width=350&height=380&fid=" + fid;

        }
    }
    else {
       
        if (tOrderID != null && parseInt(tOrderID) > 0) {
            url = orgurl + "OrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval + "&ORDER_ID=" + tOrderID + "&popup=true&modal=true&width=350&height=380&fid=" + fid;
           // alert(url);
        }
        else {
            url = orgurl + "OrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval + "&popup=true&modal=true&width=350&height=380&fid=" + fid;
           
        }
    }
 
    tb_show(null, url, null);
    this.$("body").addClass("modal-open");

   // document.forms[0].elements[buyvalue].style.borderColor = "#86b7cd";
    
    //document.forms[0].elements[buyvalue].value = "";

    //        var cart = document.getElementById("cartqty");
    //        if (cart != null) {
    //            var cnt = cart.innerHTML;
    //            cnt = cnt.replace("(", "");
    //            cnt = cnt.replace(")", "");
    //            cnt = Number(cnt) + 1;
    //            cart.innerHTML = "(" + cnt + ")";

    //        }
    // GetCartCount();

}
function callproductqtykeypress(e) {
    var bv = document.getElementById(e.target.id);
    if (bv != null) {
        if (bv.value == "") {
            bv.style.borderColor = "red";
        }
        else {
            bv.style.borderColor = "#86b7cd";
        }
    }
}
function CallProductPopupMS(orgurl, buyvalue, pid, qtyval, tOrderID, fid) {

    var url = "";

        if (tOrderID != null && parseInt(tOrderID) > 0) {
            url = orgurl + "mOrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval.trim() + "&ORDER_ID=" + tOrderID + "&popup=true&modal=true&width=350&height=380&fid=" + fid;

        }
        else {
            url = orgurl + "mOrderDetails.aspx?bulkorder=1&Pid=" + pid + "&Qty=" + qtyval.trim() + "&popup=true&modal=true&width=350&height=380&fid=" + fid;

        }

        tb_show(null, url, null);
      
        document.forms[0].elements[buyvalue].style.borderColor = "#86b7cd";
    //var bv = document.getElementById(buyvalue);
    //bv.value="";
    //document.forms[0].elements[buyvalue].value = "";
    

    //        var cart = document.getElementById("cartqty");
    //        if (cart != null) {
    //            var cnt = cart.innerHTML;
    //            cnt = cnt.replace("(", "");
    //            cnt = cnt.replace(")", "");
    //            cnt = Number(cnt) + 1;
    //            cart.innerHTML = "(" + cnt + ")";

    //        }
    // GetCartCount();

}
function GetCartCount() {
   
    var orgurl = window.location.href;
 
     $.ajax({
        type: "POST",
        url: "/GblWebMethods.aspx/cartcount",
        data: "{'Strvalue':'"+ orgurl +"'}",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: OncartSuccess,
        error: OncartFailure


    });

}

function GetCartCountpop() {
    var orgurl = window.location.href;
    var delay = 12000;
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/GblWebMethods.aspx/cartcount",
            data: "{'Strvalue':'" + orgurl + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: OncartSuccess,
            error: OncartFailure


        });

    }, delay)

}
function OncartSuccess(result) {
  
    var dt;
    if (result.d != null) {

        if (result.d.substring(0, 2) == "WA") {

            dt = result.d.split("~");

            var cart = document.getElementById("navcart");
            var carttop = document.getElementById("navcart-top");
            var carttopddiv = document.getElementById("navcart-top-div");
            var cartmaindiv = document.getElementById("navcart-div");
             try
                {
            var atagtog = document.getElementById("atagtoogle");        
       
            if ( atagtog != null) {
               
               
                atagtog.innerHTML= atagtog.innerHTML.replace("<a class=", "<a data-toggle=dropdown class=").replace("/orderDetails.aspx?CartItems=0","#");
            } 
            }
        catch (e) {

        }
           
            var cartmob = document.getElementById("cart_mop");
            if (carttop != null) {
              
                carttop.innerHTML = dt[0].substring(2) + "<div id='navcart-top-div' class='dropdown-menu'>" + dt[1] + "</div>";
               // carttopddiv.innerHTML = innerdata;
            }
            if (cart != null) {
                cart.innerHTML = dt[0].substring(2) + "<div id='navcart-div' class='dropdown-menu'>" + dt[1] + "</div>";
            }
            var mobcart = document.getElementById("itemsCount");
            if (mobcart != null) {
              //  var dupcnt = dt[0].substring(70);
               //  var orgprocnt = dupcnt.substring(0,2);
                // mobcart.innerHTML = orgprocnt; 
                mobcart.innerHTML = dt[2]; 
            }
            if (cartmob != null ) {
                // cartmob.innerHTML = "<div >";
               // alert(cartmob.innerHTML);
                // alert(dt[5]);
                if ($('#cart_mop').hasClass('dropdown-menu')) {
                    cartmob.innerHTML = "<div id='cart'>" + dt[5] + "</div>";
                }
                else {
                    document.getElementById("cart_mop").className = "dropdown-menu";
                    cartmob.innerHTML = "<div id='cart'>" + dt[5] + "</div>";
                }
            }

            //alert(cartmob.innerHTML);
            var itms = "";//  document.getElementById("CartItems_head");
            
            if (itms != null) {
               
           
                itms.innerHTML = dt[1];
            }
            var itms1 = "";// document.getElementById("CartItems");
            if (itms1 != null) {
               
                itms1.innerHTML = dt[1];
            }
        }
        else {
            dt = result.d.split(",");

            var cart = document.getElementById("cartqty");
            if (cart != null) {
                cart.innerHTML = "Items in Cart: " + dt[0] + "";
            }
            var checkout = document.getElementById("cartcheckout");
            if (checkout != null) {
                checkout.href = "/checkout.aspx?OrderID=" + dt[1] + "&ApproveOrder=Approve"
            }
            var viewcart = document.getElementById("cartviewcart");
            if (viewcart != null) {
                viewcart.href = "/orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + dt[1]
            }
        }
    }
}
function OncartFailure(result) {
    alert("failure");
}