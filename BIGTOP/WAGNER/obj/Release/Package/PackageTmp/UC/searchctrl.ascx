<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_searchctrl" Codebehind="searchctrl.ascx.cs" %>
<%@ Register Src="~/search/search.ascx" TagName="search" TagPrefix="uc1" %>
<%@ Register Src="~/search/searchrsltproducts.ascx" TagName="searchrsltproducts" TagPrefix="uc4" %>
 <input type="hidden" name="__EVENTTARGET1" id="__EVENTTARGET1" value="" runat="server"/>
 <input type="hidden" name="__EVENTARGUMENT1" id="__EVENTARGUMENT1" value="" runat="server"/>
 <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>scripts/jquery-migrate-1.0.0.min.js" ></script>

<%--<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>scripts/jquery-1.4.1.min.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" ></script>--%>
 <script type="text/javascript">
     window.onbeforeunload = function (e) {
         var scrollpos = $(window).scrollTop();
         $("#" + '<%= hfscrollpos.ClientID %>').val(scrollpos);
     }

   
 </script>
 <script type="text/javascript" language="javascript">


     function SetSortOrder(orderVal) {
         var url = window.location.href;
         $.ajax({
             type: "POST",
             url: "/GblWebMethods.aspx/SetSortOrder",
             data: "{'orderVal':'" + orderVal + "','url':'" + url + "'}",
             contentType: "application/json;charset=utf-8",
             dataType: "json",
             success: function (result) {
                 if (result.d == "1") {
                     window.location = url;
                 }
             },
             error: function (result) {
                 //rtn = false;
             }


         });
         return false;
         //OnCaptchaSuccess,
     }
    
</script>



<script type="text/javascript">
    function mobilefilter() {
        $body.removeClass("modal-open");
        //alert("test"); //only-one
        //var innhtml = document.getElementById("collapse_filter").innerHTML;
        // innhtml = innhtml + document.getElementById("filter_collapse").innerHTML;
        // alert(innhtml);
        //  var innhtml = document.getElementById("filter_collapse").innerHTML;
        // alert(iii);
        //var innhtml=document.getElementById("filter_collapse").innerHTML;
        //alert(innhtml);
        // var innhtml = document.getElementsByClassName("open hidden-xs").innerHTML;
        //alert(innhtml);
        // document.getElementById("filter_collapse1").innerHTML = innhtml; 
        // document.getElementsByClassName("mobile_filter collapse test")[0].innerHTML = innhtml;
        // document.getElementsByClassName("open visible-xs")[0].innerHTML = innhtml;
        //  $('#filter_collapse').append(innhtml).accordion('destroy').accordion();
        // var options = { autoHeight: false, collapsible: true, active: false };
        //  $('#only-one').accordion('destroy').accordion(options);

        //   $('#inner')
        //.html(" \
        //   <h3><a href='#'>First</a></h3><div>First</div> \
        //   <h3><a href='#'>Second</a></h3><div>Second</div> \
        //   <h3><a href='#'>Third</a></h3><div>Third</div>")
        //.accordion({
        //    autoHeight: false,
        //    collapsible: true,
        //    active: false,
        //});
        //  $("#only-one").accordion("refresh");
        //    $("#inner")
        //.append("<h3><a href='#'>First</a></h3><div>First</div>")
        //.append("<h3><a href='#'>Second</a></h3><div>Second</div>")
        //.append("<h3><a href='#'>Third</a></h3><div>Third</div>");
    }
    function OnclickTab(Attribute) {
        var elem = document.getElementById(Attribute);
    
        var defValue = elem.value;

        document.getElementById("hfclickedattr").value = defValue;
        document.forms[0].submit();

            };

    function geturl() {
        //        var rootPath = window.location.protocol + "//" + window.location.host;
        var e = document.getElementById("ddlcategory");
        var strURL = e.options[e.selectedIndex].text;
        var strURL1 = e.options[e.selectedIndex].value;
        // var fullPath = rootPath + strURL1;

        if (e.options[e.selectedIndex].text != 'Select Category') {
            document.getElementById("hfclickedattr").value = strURL1;


            document.forms[0].submit();
            //window.location.href=fullPath;
        }
    }

    function geturlsubcat() {
        //        var rootPath = window.location.protocol + "//" + window.location.host;
        var f = document.getElementById("ddlsubcategory");
        var strURL = f.options[f.selectedIndex].text;
        var strURL1 = f.options[f.selectedIndex].value;
       
        // var fullPath = rootPath + strURL1;
        if (f.options[f.selectedIndex].text != 'Select Category') {
            document.getElementById("hfclickedattr").value = strURL1;


            document.forms[0].submit();
            //            window.location.href = fullPath;
        }
    }


    function getCookie(name) {
        var re = new RegExp(name + "=([^;]+)");
        var value = re.exec(document.cookie);
        return (value != null) ? unescape(value[1]) : null;
    }
    $(document).ready(function () {
        $(window).bind("pageshow", function () {
            // update hidden input field
            $("#" + '<%= HiddenField2.ClientID %>').val("0");
            $("#" + '<%= HiddenField1.ClientID %>').val("0");
            $("#" + '<%= hfcheckload.ClientID %>').val("0");
            var hfback = $("#" + '<%= hfback.ClientID %>').val();
        
            if ($.browser.msie) {
                if ($.browser.version != "11.0") {

                    $("#" + '<%= HFcnt.ClientID %>').val("1");
                    hfback = "0";
                }
            }
            var cookieValue = getCookie('GLVIEWMODE');
          
            if (hfback == 1) {
                var hfbackdata = $("#" + '<%= hfbackdata.ClientID %>').val();

                if (cookieValue == "LV") {
                    $('#home-product .product-grid').addClass('list-wrapper');
                    $('#home-product .product-grid #divdesc').removeClass('dpynone');
                    $('#home-product .product-grid #divdesc').addClass('pro_discrip');
                    $('#progrid').removeClass('blue_text');
                    $('#prolist').addClass('blue_text');
                    hfbackdata = hfbackdata.replace(".product-grid", ".product-grid list-wrapper");
                    hfbackdata = hfbackdata.replace("dpynone", "pro_discrip");
                }
//                else {

//                    $('#home-product .home-grid').removeClass('list-wrapper');
//                    $('#home-product .home-grid #divdesc').addClass('dpynone');
//                    $('#home-product .home-grid #divdesc').removeClass('pro_discrip');
//                    hfbackdata = hfbackdata.replace(".home-grid list-wrapper", ".home-grid");
//                    hfbackdata = hfbackdata.replace("pro_discrip", "dpynone");
//                }

                $('.divLoadData:last').before(hfbackdata);

                jQuery(document).ready(function () {
                    $("img.lazy").lazyload();
                });
                var scrollpos = $("#" + '<%= hfscrollpos.ClientID %>').val();
                $(window).scrollTop(scrollpos);
            }
            else {

                $("#" + '<%= HFcnt.ClientID %>').val("1");
                var pgno = $("#" + '<%= HFcnt.ClientID %>').val();
                var storedData = sessionStorage.getItem("S_VIEWMODE");

                if (storedData == "LV") {
                    $('#home-product .product-grid').addClass('list-wrapper');
                    $('#home-product .product-grid #divdesc').removeClass('dpynone');
                    $('#home-product .product-grid #divdesc').addClass('pro_discrip');
                    $('#progrid').removeClass('blue_text');
                    $('#prolist').addClass('blue_text');
                }
                else {

                    $('#home-product .product-grid').removeClass('list-wrapper');
                    $('#home-product .product-grid #divdesc').addClass('dpynone');
                    $('#home-product .product-grid #divdesc').removeClass('pro_discrip');
                    $('#progrid').addClass('blue_text');
                    $('#prolist').removeClass('blue_text');
                }

            }


        });
        function lastPostFunc() {
            $("#" + '<%= hfcheckload.ClientID %>').val("1");


            $('#tblload').show();

            var iTotalPages = $("#ctl00_maincontent_searchctrl1_searchrsltproducts1_htmlpstotalpages").val();
            var iTpages = parseInt(iTotalPages);
            var eapath = $("#ctl00_maincontent_searchctrl1_searchrsltproducts1_htmlpseapath").val();
            var BCEAPath = $("#ctl00_maincontent_searchctrl1_searchrsltproducts1_htmlpsbceapath").val();
            var hforgurl = $("#" + '<%= hforgurl.ClientID %>').val();
            var hfpageno = $("#" + '<%= HFcnt.ClientID %>').val();
            var ViewMode = $("#ctl00_maincontent_searchctrl1_searchrsltproducts1_htmlpsviewmode").val();
            var irecords = $("#ctl00_maincontent_searchctrl1_searchrsltproducts1_htmlpsirecords").val();
            hfpageno = parseInt(hfpageno) + 1;
            $("#" + '<%= HFcnt.ClientID %>').val(hfpageno);
            $.ajax({
                type: "POST",
                url: "/ps.aspx/DynamicPag",
                data: "{'strvalue':'" + hforgurl + "','ipageno':" + hfpageno + ",'iTotalPages':" + iTpages + ",'eapath':'" + eapath + "','BCEAPath':'" + BCEAPath + "','ViewMode':'" + ViewMode + "','irecords':'" + irecords + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {
                        $('.divLoadData:last').before(data.d);
                        $("#" + '<%= hfcheckload.ClientID %>').val("0");


                        $("#" + '<%= hfback.ClientID %>').val("1");
                        var hfpageno1 = $("#" + '<%= HFcnt.ClientID %>').val();
                        var data1 = "";
                        if (hfpageno1 > 2) {
                            var data1 = $("#" + '<%= hfbackdata.ClientID %>').val();
                            data1 = data1 + data.d;
                        }
                        else {
                            data1 = data.d;
                        }


                        $("#" + '<%= hfbackdata.ClientID %>').val(data1);
                        var className = $("#home-product .product-grid").attr('class');
                        if (className.indexOf("list-wrapper") != -1) {

                            $('#home-product .product-grid').addClass('list-wrapper');
                        }
                        else {

                            $('#products .item').removeClass('list-wrapper');
                        }
                        jQuery(document).ready(function () {
                            $("img.lazy").lazyload();
                        });
                        $("#" + '<%= HiddenField2.ClientID %>').val(0);
                    }
                    else {

                        $("#" + '<%= HiddenField1.ClientID %>').val("1");
                        $("#" + '<%= hfcheckload.ClientID %>').val("0");
                        $('#tblload').hide();
                        $('#tddata').toggle();
                        $('#tddata').show();

                    }
                    $('#divPostsLoader').empty();
                },


                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            })
        };

        //When scroll down, the scroller is at the bottom with the function below and fire the lastPostFunc function
        // var x = 1;

        $(window).scroll(function () {
            var className = $("#home-product .product-grid").attr('class');
            if (className.indexOf("list-wrapper") != -1) {

                $('#home-product .product-grid').addClass('list-wrapper'); $('#home-product .product-grid #divdesc').removeClass('dpynone');
                $('#home-product .product-grid #divdesc').addClass('pro_discrip');
            }
            else {

                $('#products .item').removeClass('list-wrapper'); $('#home-product .product-grid #divdesc').addClass('dpynone');
                $('#home-product .product-grid #divdesc').removeClass('pro_discrip');
            }
            var x = $("#" + '<%= HiddenField1.ClientID %>').val();
            var y = $(window).scrollTop();
            var z = $(document).height() - $(window).height() - 300;
            var checkload = $("#" + '<%= hfcheckload.ClientID %>').val();
            z = z - (z / 2);
           
            var varloadloader = $("#ctl00_maincontent_searchctrl1_searchrsltproducts1_loadloader").val();

            if (varloadloader == "true") {
                if (z > 3000) {
                    z = 800;
                }
                if ($(window).scrollTop() >= z) {
                     z = $(document).height() - $(window).height() - 300;
                     z = z - (z / 2);
                    var hf = $("#" + '<%= HiddenField2.ClientID %>').val();
                    if (hf != z) {
                        $("#" + '<%= HiddenField2.ClientID %>').val(z);
                        if (x == "0") {
                            lastPostFunc();
                        }
                    }
                }
                else if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                    var hf = $("#" + '<%= HiddenField2.ClientID %>').val();
                    if (hf != z) {
                        $("#" + '<%= HiddenField2.ClientID %>').val(z);
                        if (x == "0") {
                            lastPostFunc();
                        }
                    }
                }
            }
        });
    });  
</script>
<script type="text/javascript">
    function __doPostBack2(eventTarget, eventArgument) {
        document.getElementById("<%=__EVENTTARGET1.ClientID%>").value = eventTarget;
        document.getElementById("<%=__EVENTARGUMENT1.ClientID%>").value = eventArgument;
        document.forms[0].submit();
        return true;
    }
   

    function CheckAddCount(field, ctlid) {
        var count = 1;
        for (var j = 0; j < field.length; j++) {
            if (field[j].checked) {
                count = count + 1;
            }
        }
    }
    function GetAddItems(field) {
        var count = 0;
        var st = "";
        if (field.length > 0) {
            for (var j = 0; j < field.length; j++) {
                if (field[j].checked) {
                    st += field[j].value + ",";
                    count = count + 1;
                }
            }
        }
        else {
            if (field.checked) {
                st += field.value + ",";
                count = count + 1;
            }
        }
        if (count >= 1) {
            return __doPostBack2('OrderDetails', st);
        }
        else
            return false;
    }

    function changeLview() {
        $('#home-product .product-grid').addClass('list-wrapper');
        $('#progrid').removeClass('blue_text');
        $('#prolist').addClass('blue_text');
        $('#home-product .product-grid #divdesc').removeClass('dpynone');
        $('#home-product .product-grid #divdesc').addClass('pro_discrip');
        window.document.cookie = "GLVIEWMODE" + "=" + "LV";
        sessionStorage.setItem("S_VIEWMODE", "LV");
    }
    function changeGview() {
        $('#home-product .product-grid').removeClass('list-wrapper');
        $('#prolist').removeClass('blue_text');
        $('#progrid').addClass('blue_text');
        $('#home-product .product-grid #divdesc').removeClass('pro_discrip');
        $('#home-product .product-grid #divdesc').addClass('dpynone');
        window.document.cookie = "GLVIEWMODE" + "=" + "GV";
        sessionStorage.setItem("S_VIEWMODE", "GV");
    }

    </script>   


<div class="container">
	<div class="row">
    	
         <% Response.Write(Bread_Crumbs());   %>
        	
   
    </div>
  <%-- <div class="row middle">--%>
    <div class="row">
    	<div class="col-md-4 col-lg-4 leftsidebar pl0" id="sidebar">
        	
           
                
           <%  Response.Write(ST_Categories());  %>
            	
          
       	</div>
       
          
  <div class="col-md-16 rightgrid pr0" id="content">
                   
            	<div class="row">
                <div class="col-sm-20 col-md-20 clearfix mpad0">
                <div class="ctgry-headpanel clearfix">
  <div class="categoryheading">
    <h4>Search Results</h4>
  </div>
<%--  <div class="sort-btns">
    <div id="grid-btn">
      <a href="#">
        <img alt="Grid" src="/images/grid_btn.png"/>
      </a>
    </div>
    <div id="list-btn">
      <a href="#">
        <img alt="Grid" src="/images/list_btn.png"/>
      </a>
    </div>

  </div>--%>
  <div class="sort-btns hidden-xs">
      <div id="grid-btn">
        <a  id="progrid" class="blue_text" onclick="changeGview();" style="cursor:pointer;">
          <span class="glyphicon glyphicon-th"></span>Grid
        </a>
      </div>
      <div id="list-btn">
        <a  id="prolist" class="" onclick="changeLview();" style="cursor:pointer;">
          <span class="glyphicon glyphicon-list"></span>List
        </a>
      </div>
    </div>
  <div class="pricesorter_wrap subject hidden-xs">   
    <input type="hidden" value="sort" name="subject"/>
      <li id="pricesorter" class="dropdown">
             <span style="vertical-align: middle; line-height: 30px">Sort By :</span>
        <a data-toggle="dropdown" class="dropdown-toggle" href="#" aria-expanded="false" style="display:block;"><%=strsortval%></a>
        <div class="dropdown-menu">
          <ul class="">
            <li>
              <a onclick="SetSortOrder('latest');">Latest</a>
            </li>
            <li>
              <a onclick="SetSortOrder('ltoh');">Price Low to High</a>
            </li>
            <li>
              <a onclick="SetSortOrder('htol');">Price High to Low</a>
            </li>
              <li>
                <a onclick="SetSortOrder('relevance');">Relevance</a>            
            </li>
                <li>
                <a onclick="SetSortOrder('popularity');">Popular</a>            
            </li>
          </ul>
        </div>
      </li>
    </div>
</div>
            
                    	<div class="subct-selection">
                        	<div class="subcategory-selection clearfix">
                            	
								
                                 <uc1:search ID="search1" runat="server" />    
                          
                        </div>
                        </div>
                          <uc4:searchrsltproducts ID="searchrsltproducts1" runat="server" />
                        
                 
                       </div>
	              </div>
           </div>

</div>






    <table align="center" width="100%" border="0" cellpadding="0" cellspacing="0" ><tr><td width="70%">&nbsp;</td>
    <td width="28%" align="right"><a href="" class="scrollup" >Top</a></td><td width="2%">&nbsp;</td></tr></table>







    <div>
    
      <asp:HiddenField ID="HiddenField2" runat="server" />
       <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="HFcnt" runat="server" />
        <asp:HiddenField ID="hfcheckload" runat="server" />
        <asp:HiddenField ID="hforgurl" runat="server" />
         <asp:HiddenField ID="hfnewurl" runat="server" />
         <asp:HiddenField ID="hfback" runat="server" />
         <asp:HiddenField ID="hfbackdata" runat="server" />
           <asp:HiddenField ID="hfscrollpos" runat="server" />
             <input type="hidden" id="hfclickedattr" name="hfclickedattr" value="" />

    
    </div>
    </div>

<div class="fixed_bottom visible-xs clearfix">
    <div class="fixed_filter">
        <a href="#popup_filter" class="" data-toggle="modal" data-backdrop="">FILTER</a>
    </div>
    <div class="fixed_addtocart">
        <a href="#popup_sorter" class="" data-toggle="modal" >SORT BY</a>
    </div>
</div>
<div id="popup_sorter" class="modal fade in" style="display: none;z-index:9999 !important;" aria-hidden="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">�</button>
                    <h3 class="filter_title">Product Sorter</h3>
                </div>
                <div class="modal-body">
                    <div class="container">
                        <div class="mob_sort">
                        	<li>
                                 <a onclick="SetSortOrder('latest');" style="cursor:pointer;">Latest</a>

                        	</li>
                            <li>
                                <a onclick="SetSortOrder('ltoh');" style="cursor:pointer;">Price Low to High</a>

                            </li>
                            <li>
                            <a onclick="SetSortOrder('relevance');" style="cursor:pointer;">Relevance</a> 
                            </li> 
                            <li>
                                <a onclick="SetSortOrder('htol');" style="cursor:pointer;">Price High to Low</a>

                            </li>
                            <li>
                               <a onclick="SetSortOrder('popularity');" style="cursor:pointer;">Popular</a>
                            </li>
                        </div>
                    </div>
                    
                </div>

            </div>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        (function ($, window, document, undefined) { var $window = $(window); $.fn.lazyload = function (options) { var elements = this; var $container; var settings = { threshold: 0, failure_limit: 0, event: "scroll", effect: "show", container: window, data_attribute: "original", skip_invisible: true, appear: null, load: null, placeholder: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCC" }; function update() { var counter = 0; elements.each(function () { var $this = $(this); if (settings.skip_invisible && !$this.is(":visible")) { return } if ($.abovethetop(this, settings) || $.leftofbegin(this, settings)) { } else { if (!$.belowthefold(this, settings) && !$.rightoffold(this, settings)) { $this.trigger("appear"); counter = 0 } else { if (++counter > settings.failure_limit) { return false } } } }) } if (options) { if (undefined !== options.failurelimit) { options.failure_limit = options.failurelimit; delete options.failurelimit } if (undefined !== options.effectspeed) { options.effect_speed = options.effectspeed; delete options.effectspeed } $.extend(settings, options) } $container = (settings.container === undefined || settings.container === window) ? $window : $(settings.container); if (0 === settings.event.indexOf("scroll")) { $container.bind(settings.event, function () { return update() }) } this.each(function () { var self = this; var $self = $(self); self.loaded = false; if ($self.attr("src") === undefined || $self.attr("src") === false) { if ($self.is("img")) { $self.attr("src", settings.placeholder) } } $self.one("appear", function () { if (!this.loaded) { if (settings.appear) { var elements_left = elements.length; settings.appear.call(self, elements_left, settings) } $("<img src='' />").bind("load", function () { var original = $self.attr("data-" + settings.data_attribute); $self.hide(); if ($self.is("img")) { $self.attr("src", original) } else { $self.css("background-image", "url('" + original + "')") } $self[settings.effect](settings.effect_speed); self.loaded = true; var temp = $.grep(elements, function (element) { return !element.loaded }); elements = $(temp); if (settings.load) { var elements_left = elements.length; settings.load.call(self, elements_left, settings) } }).attr("src", $self.attr("data-" + settings.data_attribute)) } }); if (0 !== settings.event.indexOf("scroll")) { $self.bind(settings.event, function () { if (!self.loaded) { $self.trigger("appear") } }) } }); $window.bind("resize", function () { update() }); if ((/(?:iphone|ipod|ipad).*os 5/gi).test(navigator.appVersion)) { $window.bind("pageshow", function (event) { if (event.originalEvent && event.originalEvent.persisted) { elements.each(function () { $(this).trigger("appear") }) } }) } $(document).ready(function () { update() }); return this }; $.belowthefold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = (window.innerHeight ? window.innerHeight : $window.height()) + $window.scrollTop() } else { fold = $(settings.container).offset().top + $(settings.container).height() } return fold <= $(element).offset().top - settings.threshold }; $.rightoffold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.width() + $window.scrollLeft() } else { fold = $(settings.container).offset().left + $(settings.container).width() } return fold <= $(element).offset().left - settings.threshold }; $.abovethetop = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollTop() } else { fold = $(settings.container).offset().top } return fold >= $(element).offset().top + settings.threshold + $(element).height() }; $.leftofbegin = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollLeft() } else { fold = $(settings.container).offset().left } return fold >= $(element).offset().left + settings.threshold + $(element).width() }; $.inviewport = function (element, settings) { return !$.rightoffold(element, settings) && !$.leftofbegin(element, settings) && !$.belowthefold(element, settings) && !$.abovethetop(element, settings) }; $.extend($.expr[":"], { "below-the-fold": function (a) { return $.belowthefold(a, { threshold: 0 }) }, "above-the-top": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-screen": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-screen": function (a) { return !$.rightoffold(a, { threshold: 0 }) }, "in-viewport": function (a) { return $.inviewport(a, { threshold: 0 }) }, "above-the-fold": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-fold": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-fold": function (a) { return !$.rightoffold(a, { threshold: 0 }) } }) })(jQuery, window, document);
</script>
    