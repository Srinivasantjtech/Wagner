<%@ Control Language="C#" AutoEventWireup="true" Inherits="bybrandMS" Codebehind="bybrandMS.ascx.cs" %>
<%@ Register Src="~/search/searchrsltproducts.ascx" TagName="searchrsltproducts" TagPrefix="uc4" %>
<input id="HidItemPage" type="hidden" runat="server" />
<input id="Hidcat" type="hidden" runat="server" />
<%--<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>scripts/jquery-1.4.1.min.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" ></script>--%>
<script type="text/javascript">
    window.onbeforeunload = function (e) {
        var scrollpos = $(window).scrollTop();
        $("#" + '<%= hfscrollpos.ClientID %>').val(scrollpos);
    }
 </script>

<script type="text/javascript">
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


            if (hfback == 1) {
                var hfbackdata = $("#" + '<%= hfbackdata.ClientID %>').val();

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


            }

        });
        function lastPostFunc() {
            $("#" + '<%= hfcheckload.ClientID %>').val("1");
            $('#tblload').show();
            var iTotalPages = $("#ctl00_maincontent_bybrand1_searchrsltproducts2_htmltotalpage").val();
            var iTpages = parseInt(iTotalPages);
            var eapath = $("#ctl00_maincontent_bybrand1_searchrsltproducts2_htmleapath").val();
            var BCEAPath = $("#ctl00_maincontent_bybrand1_searchrsltproducts2_htmlbceapth").val();
            var hforgurl = $("#" + '<%= hforgurl.ClientID %>').val();
            var hfpageno = $("#" + '<%= HFcnt.ClientID %>').val();
            var ViewMode = $("#ctl00_maincontent_bybrand1_searchrsltproducts2_htmlviewmode").val();
            var irecords = $("#ctl00_maincontent_bybrand1_searchrsltproducts2_htmlirecords").val();
            var prevpageno = hfpageno;
            hfpageno = parseInt(hfpageno) + 1;
            $("#" + '<%= HFcnt.ClientID %>').val(hfpageno);
            $.ajax({
                type: "POST",
                url: "/bb.aspx/DynamicPag",
                data: "{'strvalue':'" + hforgurl + "','ipageno':" + hfpageno + ",'iTotalPages':" + iTpages + ",'eapath':'" + eapath + "','BCEAPath':'" + BCEAPath + "','ViewMode':'" + ViewMode + "','irecords':'" + irecords + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {

                        $('.divLoadData:last').before(data.d);
                        $('#tblload').hide();
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
                        jQuery(document).ready(function () {
                            $("img.lazy").lazyload();
                        });

                    }
                    else {
                        $("#" + '<%= HiddenField1.ClientID %>').val("1");
                        $("#" + '<%= hfcheckload.ClientID %>').val("0");
                        $('#tblload').hide();
                        $('#databottom').toggle();
                        $('#databottom').show();
                    }
                    $('#divPostsLoader').empty();
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            })
            $('#tblload').hide();
        };
        $(window).scroll(function () {
            var x = $("#" + '<%= HiddenField1.ClientID %>').val();
            var y = $(window).scrollTop();
            var z = $('#tblmain').outerHeight();
            var checkload = $("#" + '<%= hfcheckload.ClientID %>').val();
            z = z - (z / 2);
            if ($(window).scrollTop() >= z) {
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
        });
    });   
</script>
<script type="text/javascript">
        function __doPostBack(eventTarget, eventArgument) {
            document.getElementById("__EVENTTARGET").value = eventTarget;
            document.getElementById("__EVENTARGUMENT").value = eventArgument;
            document.forms[0].submit();
        }
        
       
      
      
    </script>  
    <input type="hidden" id="hdnFamilyId" runat="server"/>   
    <input type="hidden" name="__EVENTTARGET" value=""/>
    <input type="hidden" name="__EVENTARGUMENT" value=""/>
    <table id="tblmain" align="center" style="width: 558">
    <tr><td align="center">
    <% if (Request.Url.OriginalString.ToString().ToUpper().Contains("BB.ASPX")) %>
        <table width="780" border="0" cellspacing="0" cellpadding="0" onload="Getfidfromurl();">
    <% else{%>        
        <table width="558" border="0" cellspacing="0" cellpadding="0" onload="Getfidfromurl();">   
        <tr><td align="left" class="tx_1"><div class="" style="margin:0 0 0 10px;">          
               <%
                   Response.Write(Bread_Crumbs());
                %>
                 </div>  </td></tr><tr><td height="3px"> </td></tr>  </table> </td></tr>
        <tr><td>  <div class="box1" style=" margin:0 0 0 10px; height:100%;" >    
                <uc4:searchrsltproducts ID="searchrsltproducts2" runat="server" /> 
               <table> <tr><td><div class="divLoadData"></div> </td></tr>
               <tr><td align="center"><table id="tblload" style="display:none;" width="760px" ><tr><td align="center">      
      <div width="300px" align="center"><img src="$CDN$images/bigLoader.gif" width="6%" height ="6%"></div></td></tr>
      <tr><td align="center"><asp:Label ID="Label1" runat="server" Text="LOADING DATA...PLEASE WAIT " Font-Bold="True" Font-Names="Arial" Font-Size="X-Small" ></asp:Label> 
      </td> </tr> </table>  </td> </tr>
               <tr><td><table id="databottom" width="760px" bgColor="#f2f2f2" border="0" cellSpacing="0" cellPadding="0" style="display:none;" class="box2">
          <tr><td height="35" width="156" align="left"></td><td width="303" align="middle"></td><td width="175" align="right"></td></tr>
        </table></td>  </tr>  </table> </div>
                 <asp:HiddenField ID="HiddenField2" runat="server" /><asp:HiddenField ID="HiddenField1" runat="server" /><asp:HiddenField ID="HFcnt" runat="server" />
                <asp:HiddenField ID="hfcheckload" runat="server" /><asp:HiddenField ID="hforgurl" runat="server" />
              <asp:HiddenField ID="hfnewurl" runat="server" />
                    <asp:HiddenField ID="hfback" runat="server" />
                       <asp:HiddenField ID="hfbackdata" runat="server" />
              <asp:HiddenField ID="hfscrollpos" runat="server" />
              </td></tr></table>   
   <table align="center" width="100%" border="0" cellpadding="0" cellspacing="0" ><tr><td width="70%">&nbsp;</td>
    <td width="28%" align="right"> <a href="" class="scrollup" >Top</a></td><td width="2%">&nbsp;</td></tr></table>


      <script type="text/javascript" language="javascript">
          (function ($, window, document, undefined) { var $window = $(window); $.fn.lazyload = function (options) { var elements = this; var $container; var settings = { threshold: 0, failure_limit: 0, event: "scroll", effect: "show", container: window, data_attribute: "original", skip_invisible: true, appear: null, load: null, placeholder: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCC" }; function update() { var counter = 0; elements.each(function () { var $this = $(this); if (settings.skip_invisible && !$this.is(":visible")) { return } if ($.abovethetop(this, settings) || $.leftofbegin(this, settings)) { } else { if (!$.belowthefold(this, settings) && !$.rightoffold(this, settings)) { $this.trigger("appear"); counter = 0 } else { if (++counter > settings.failure_limit) { return false } } } }) } if (options) { if (undefined !== options.failurelimit) { options.failure_limit = options.failurelimit; delete options.failurelimit } if (undefined !== options.effectspeed) { options.effect_speed = options.effectspeed; delete options.effectspeed } $.extend(settings, options) } $container = (settings.container === undefined || settings.container === window) ? $window : $(settings.container); if (0 === settings.event.indexOf("scroll")) { $container.bind(settings.event, function () { return update() }) } this.each(function () { var self = this; var $self = $(self); self.loaded = false; if ($self.attr("src") === undefined || $self.attr("src") === false) { if ($self.is("img")) { $self.attr("src", settings.placeholder) } } $self.one("appear", function () { if (!this.loaded) { if (settings.appear) { var elements_left = elements.length; settings.appear.call(self, elements_left, settings) } $("<img src='' />").bind("load", function () { var original = $self.attr("data-" + settings.data_attribute); $self.hide(); if ($self.is("img")) { $self.attr("src", original) } else { $self.css("background-image", "url('" + original + "')") } $self[settings.effect](settings.effect_speed); self.loaded = true; var temp = $.grep(elements, function (element) { return !element.loaded }); elements = $(temp); if (settings.load) { var elements_left = elements.length; settings.load.call(self, elements_left, settings) } }).attr("src", $self.attr("data-" + settings.data_attribute)) } }); if (0 !== settings.event.indexOf("scroll")) { $self.bind(settings.event, function () { if (!self.loaded) { $self.trigger("appear") } }) } }); $window.bind("resize", function () { update() }); if ((/(?:iphone|ipod|ipad).*os 5/gi).test(navigator.appVersion)) { $window.bind("pageshow", function (event) { if (event.originalEvent && event.originalEvent.persisted) { elements.each(function () { $(this).trigger("appear") }) } }) } $(document).ready(function () { update() }); return this }; $.belowthefold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = (window.innerHeight ? window.innerHeight : $window.height()) + $window.scrollTop() } else { fold = $(settings.container).offset().top + $(settings.container).height() } return fold <= $(element).offset().top - settings.threshold }; $.rightoffold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.width() + $window.scrollLeft() } else { fold = $(settings.container).offset().left + $(settings.container).width() } return fold <= $(element).offset().left - settings.threshold }; $.abovethetop = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollTop() } else { fold = $(settings.container).offset().top } return fold >= $(element).offset().top + settings.threshold + $(element).height() }; $.leftofbegin = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollLeft() } else { fold = $(settings.container).offset().left } return fold >= $(element).offset().left + settings.threshold + $(element).width() }; $.inviewport = function (element, settings) { return !$.rightoffold(element, settings) && !$.leftofbegin(element, settings) && !$.belowthefold(element, settings) && !$.abovethetop(element, settings) }; $.extend($.expr[":"], { "below-the-fold": function (a) { return $.belowthefold(a, { threshold: 0 }) }, "above-the-top": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-screen": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-screen": function (a) { return !$.rightoffold(a, { threshold: 0 }) }, "in-viewport": function (a) { return $.inviewport(a, { threshold: 0 }) }, "above-the-fold": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-fold": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-fold": function (a) { return !$.rightoffold(a, { threshold: 0 }) } }) })(jQuery, window, document);
</script>