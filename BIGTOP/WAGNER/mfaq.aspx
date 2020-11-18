<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mfaq.aspx.cs" Inherits="WES.WebForm1" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="metatag" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"> 
    
<%--<script type="text/javascript">
    $(document).ready(function () {

        $('.panel-collapse.in').collapse('hide');

        $('.openall').click(function () {

            var expval = $("#reelplay_faq").text();
            if (expval == "Expand all") {
                $("#reelplay_faq").text("Collapse all");
               $('#accordion div div h4 a').removeClass();
                $('#accordion div div h4 a').addClass("accordion-toggle");
               $('.panel-collapse:not(".in")').collapse('show');

              //  alert("1");
            }
            else if (expval = "Collapse all") {
                $("#reelplay_faq").text("Expand all");
                $('#accordion div div h4 a').removeClass();
                $('#accordion div div h4 a').addClass("accordion-toggle collapsed");
                $('.panel-collapse.in').collapse('hide');
               // alert("2");
            }
            else {
              //  alert("3");
                $("#reelplay_faq").text("Collapse all");
                $('#accordion div div h4 a').removeClass();
                $('#accordion div div h4 a').addClass("accordion-toggle");
            }
            //if ($('.panel-collapse.in').collapse('hide')) {
            //    $('.panel-collapse:not(".in")').collapse('show');
            //    alert("1111");
            //   // $(".accordion-toggle").parent().find(".glyphicon-plus").removeClass("glyphicon-plus").addClass("glyphicon-minus");
            //}

        });

    });


    function updowncss(obj) {
        var id = obj;
        // alert(id);

        //if ($('.panel-collapse.in').collapse('hide')) {
        //    // $('.panel-collapse:not(".in")').collapse('show');
        //    $('#accordion div div h4 a').removeClass();
        //    $('#accordion div div h4 a').addClass("accordion-toggle collapsed");
        //    $('#' + obj).removeClass();
        //    $('#' + obj).addClass("accordion-toggle");
        //}
       // alert($('#' + obj).attr("class"));

        //for (i = 1; i <= 11; i++) {
        //    if (obj == i) {
        //        if ($('#' + obj).attr("class") == "accordion-toggle") {
        //            $('#' + obj).removeClass();
        //            $('#' + obj).addClass("accordion-toggle collapsed");
        //        }
        //        else {
        //            $('#' + obj).removeClass();
        //            $('#' + obj).addClass("accordion-toggle");
        //        }
        //    }
        //    else {
        //        if ($('#' + obj).attr("class") == "accordion-toggle collapsed") {
        //            $('#' + obj).removeClass();
        //            $('#' + obj).addClass("accordion-toggle");
        //        }
        //        else {
        //            $('#' + obj).removeClass();
        //            $('#' + obj).addClass("accordion-toggle collapsed");
        //        }
        //    }
        //}

        //if ($('.panel-collapse.in').collapse('hide')) {
        //    $('.panel-collapse:not(".in")').collapse('show');
        //   // $(".accordion-toggle").parent().find(".glyphicon-plus").removeClass("glyphicon-plus").addClass("glyphicon-minus");
        //}
    }
</script>--%>
    <script type="text/javascript">
        $(document).ready(function () {

            $('.panel-collapse.in').collapse('hide');

            $('.openall').click(function () {

                var expval = $("#reelplay_faq").text();
                if (expval == "Expand all") {
                    $("#reelplay_faq").text("Collapse all");
                    $('#accordion div div h4 a i').removeClass();
                    $('#accordion div div h4 a i').addClass("indicator glyphicon glyphicon-chevron-down");
                   // $('.panel-collapse:not(".in")').collapse('show');

                    for (i = 1 ; i <= 10 ; i++) {
                        var id = "#collapse" + i;
                        $(id).removeClass();
                        $(id).addClass("panel-collapse collapse in");
                       $(id).css('height', 'auto');
                        //  $(id).animate({  }, "slow");
                      //  $(id).removeAttr("style");
                        // $(id).animate({ 'max-height': '1000px' }, 5000);
                        // $(id).animate({ 'max-height': '1000px' }, "slow");
                        //  $(id).slideToggle(1000);
                    }
                    //  alert("1");
                }
                else if (expval = "Collapse all") {
                    $("#reelplay_faq").text("Expand all");
                    $('#accordion div div h4 a i').removeClass();
                    $('#accordion div div h4 a i').addClass("indicator glyphicon glyphicon-chevron-up");
                   // $('.panel-collapse.in').collapse('hide');
                    for (i = 1 ; i <= 10 ; i++) {
                        var id = "#collapse" + i;
                        $(id).removeClass();
                        $(id).addClass("panel-collapse collapse");
                        $(id).css('height', '0px');
                     
                     }
                }
                else {
                    //  alert("3");
                    $("#reelplay_faq").text("Collapse all");
                    $('#accordion div div h4 a').removeClass();
                    $('#accordion div div h4 a').addClass("accordion-toggle");
                }
                //if ($('.panel-collapse.in').collapse('hide')) {
                //    $('.panel-collapse:not(".in")').collapse('show');
                //    alert("1111");
                //   // $(".accordion-toggle").parent().find(".glyphicon-plus").removeClass("glyphicon-plus").addClass("glyphicon-minus");
                //}

            });

            function toggleChevron(e) {
                $(e.target)
					.prev('.panel-heading')
					.find("i.indicator")
					.toggleClass('glyphicon-chevron-down glyphicon-chevron-up');
            }
            $('#accordion').on('hidden.bs.collapse', toggleChevron);
            $('#accordion').on('shown.bs.collapse', toggleChevron);

        });



</script>
  <style type="text/css">
      /*
.reelplay_faq .panel-body {
	padding:20px;
}

.reelplay_faq .panel-heading .collapsed[data-toggle="collapse"]::after {
    color: #454444;
    transform: rotate(90deg);
}
.reelplay_faq .panel-heading .collapsed[data-toggle="collapse"]::after {
    color: #454444;
    transform: rotate(90deg);
}

.reelplay_faq .panel-heading [data-toggle="collapse"]:after {
        font-family: 'Glyphicons Halflings';
        content: "\e072"; 
        float: right;
        color: #F58723;
        font-size: 14px;
        line-height: 22px;
        -webkit-transform: rotate(-90deg);
        -moz-transform: rotate(-90deg);
        -ms-transform: rotate(-90deg);
        -o-transform: rotate(-90deg);
        transform: rotate(-90deg);
}

.reelplay_faq .panel-heading [data-toggle="collapse"].collapsed:after {
        -webkit-transform: rotate(90deg);
        -moz-transform: rotate(90deg);
        -ms-transform: rotate(90deg);
        -o-transform: rotate(90deg);
        transform: rotate(90deg);
        color: #454444;
}
*/
      .reelplay_faq .panel-body {
	padding:20px;
}
.panel-title { position:relative; }
.panel-title i { position:absolute; right:0; margin-right:10px; top:35%;cursor:pointer; } 
</style>

</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="Banner" runat="server">
</asp:Content>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentbreadcrumb" runat="server">

</asp:Content>
<%--<asp:Content ID="Content5" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content6" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container margin_top">
  <div class="row">

   
    <div class="col-lg-12">
    <h4 class="bolder"><span class="gray_40">Reel Play</span>  <span class="blue_color_text">FAQ</span></h4>
    
    	<div class="reelplay_faq" >
            <div class="block text-right">
          <a class="btn btn-default openall" id="reelplay_faq">Expand all</a>
                </div>
            <hr/>
          <div id="accordion" class="panel-group">
            <div class="panel panel-default">
              <div class="panel-heading">
                <h4 class="panel-title">
                  <a href="#collapse1" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle collapsed" id="1" >
                  
                    How can I contact Reelplay?
                       <i class="indicator glyphicon glyphicon-chevron-down"></i>
                  </a>
                      
                </h4>
              </div>
              <div class="panel-collapse collapse" id="collapse1" style="height: 0px;">
                <div class="panel-body">
                   You can chat with us on the website or call us:
					<br/>

                Europe: +49 32 2142 19984<br/>

                USA: 1888 2169 054<br/>

                Canada: 1888 2169 054<br/>

                Australia: 1800 580 997


                </div>
              </div>
            </div>
            <div class="panel panel-default">
              <div class="panel-heading">
                <h4 class="panel-title">
                  <a href="#collapse2" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle collapsed"  id="2" >
                
                   Do I need to install a satellite dish?
                        <i class="indicator glyphicon glyphicon-chevron-down"></i>
                  </a>
                     
                </h4>
              </div>
              <div class="panel-collapse collapse" id="collapse2" style="height: 0px;">
                <div class="panel-body">
                  No, Reelplay is an IPTV receiver which means (Internet Protocol Television), it uses internet technology to stream the channels to your TV without a satellite dish. 
                </div>
              </div>
            </div>
            <div class="panel panel-default">
              <div class="panel-heading">
                <h4 class="panel-title">
                  <a href="#collapse3" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle collapsed"  id="3" >
            
                   What internet speed do I need?
                        <i class="indicator glyphicon glyphicon-chevron-down"></i>
                  </a>
                      
                </h4>
              </div>
              <div class="panel-collapse collapse" id="collapse3" style="height: 0px;">
                <div class="panel-body">
                  You will need 2 Mbps internet speed or higher. You can use this site to test your speed.<a href="www.fast.com" target="_blank" >www.fast.com</a>
                </div>
              </div>
            </div>
              <div class="panel panel-default">
              <div class="panel-heading">
                <h4 class="panel-title">
                  <a href="#collapse4" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle collapsed"  id="4" >
            
                   What's the internet data usage when watching on Reelplay? 
                        <i class="indicator glyphicon glyphicon-chevron-down"></i>
                  </a>
                    
                </h4>
              </div>
              <div class="panel-collapse collapse" id="collapse4" style="height: 0px;">
                <div class="panel-body">
                 Based on average client watching 8 hours per day, Reelplay will use 100GB monthly. 
                </div>
              </div>
            </div>
              <div class="panel panel-default">
              <div class="panel-heading">
                <h4 class="panel-title">
                  <a href="#collapse5" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle collapsed"  id="5" >
            
                   Is Reelplay HD-110 IPTV Box Wireless? 
                        <i class="indicator glyphicon glyphicon-chevron-down"></i>
                  </a>
                      
                </h4>
              </div>
              <div class="panel-collapse collapse" id="collapse5" style="height: 0px;">
                <div class="panel-body">
                 Yes Reelplay HD-110 IPTV Box supports Wifi. 
                </div>
              </div>
            </div>
              <div class="panel panel-default">
              <div class="panel-heading">
                <h4 class="panel-title">
                  <a href="#collapse6" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle collapsed"  id="6" >
            
                  If I purchase Reelpay today How long will it take to deliver? 
                        <i class="indicator glyphicon glyphicon-chevron-down"></i>
                  </a>
                      
                </h4>
              </div>
              <div class="panel-collapse collapse" id="collapse6" style="height: 0px;">
                <div class="panel-body">
                 Once you have completed your online order on our website, you will receive your Reelplay box within 3 business days.<br />
Once the order is shipped out you will get an email with the DHL tracking number. 
                </div>
              </div>
            </div>
              <div class="panel panel-default">
              <div class="panel-heading">
                <h4 class="panel-title">
                  <a href="#collapse7" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle collapsed"  id="7" >
            
                 Is it easy to connect Reelplay to my TV and how do I do it? 
                        <i class="indicator glyphicon glyphicon-chevron-down"></i>
                  </a>
                    
                </h4>
              </div>
              <div class="panel-collapse collapse" id="collapse7" style="height: 0px;">
                <div class="panel-body">
           All you have to do is to connect your Reelplay HD-110 IPTV Box to the internet using the Ethernet cable or wifi, connect it to your tv using HDMI cable or the RCA cable and plug the power. All cables are included in the box. Once it is connected and turned ON you can proceed to activate by scratching the panel on the plastic activation card and your account gets automatically activated and your subscription starts from that date. 
                </div>
              </div>
            </div>
              <div class="panel panel-default">
              <div class="panel-heading">
                <h4 class="panel-title">
                  <a href="#collapse8" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle collapsed"  id="8" >
            
                Does Reelplay HD-110 record? 
                        <i class="indicator glyphicon glyphicon-chevron-down"></i>
                  </a>
                       
                </h4>
              </div>
              <div class="panel-collapse collapse" id="collapse8" style="height: 0px;">
                <div class="panel-body">
          Yes, Reelplay Hd-110 can record if you plug a USB stick in.<br />
You can also create multiple recording schedules so you can record your shows when you are away from home.<br />
And you can record two channels and watch a third channel simultaneously. 
                </div>
              </div>
            </div>
              <div class="panel panel-default">
              <div class="panel-heading">
                <h4 class="panel-title">
                  <a href="#collapse9" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle collapsed"  id="9" >
            
                What is the warranty on the Reelplay Box? 
                        <i class="indicator glyphicon glyphicon-chevron-down"></i>
                  </a>
                      
                </h4>
              </div>
              <div class="panel-collapse collapse" id="collapse9" style="height: 0px;">
                <div class="panel-body">
         The Reelplay Hd-110 Box comes with a one year hardware warranty.<br />
If you have problems contact us so we can help you to diagnose the problem and fix it. 
              </div>
            </div>

          </div>
              <div class="panel panel-default">
              <div class="panel-heading">
                <h4 class="panel-title">
                  <a href="#collapse10" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle collapsed"  id="10" >
            
               What happens when my Reelplay subscription expires and How does the renewal work? 
                        <i class="indicator glyphicon glyphicon-chevron-down"></i>
                  </a>
                       
                </h4>
              </div>
              <div class="panel-collapse collapse" id="collapse10" style="height: 0px;">
                <div class="panel-body">
    Once it comes time to renew you can buy a renewal on our website for $99 USD for a 2 year subscription, You will receive a 2 year activation code. By entering the code in your Reelplay activation menu your account automatically will be activated for 2 years. 
              </div>
            </div>

          </div>
        </div>
    
    </div>
 

  </div>
</div>
        </div>
    
</asp:Content>
<%--<asp:Content ID="Content7" ContentPlaceHolderID="Popupcontent" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="footer" runat="server">
</asp:Content>--%>
