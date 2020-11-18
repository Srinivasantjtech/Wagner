<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mpd.aspx.cs" Inherits="WES.mpd" %>
<%@ Register Src="UC/MICROSITE/ProductMS.ascx" TagName="ProductMS" TagPrefix="uc1" %>
 <asp:Content ID="Content7" ContentPlaceHolderID="metatag" Runat="Server">
         <asp:Literal runat="server" ID="litMeta" />
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">


<!-- zoom -->
 <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/js/killercarousel.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>"></script>
        <!-- Include KillerCarousel CSS -->
        <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/css/killercarousel.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/css" rel="stylesheet" />
        
        <!-- Include Cloud Zoom CSS -->
        <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/css/cloudzoom.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/css" rel="stylesheet" />
        <!-- Include Cloud Zoom JavaScript -->
        <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/js/cloudzoom.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>"></script>
        
        <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/js/thumbelina.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>"></script>
        
        <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/css/jetzoom.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/css" rel="stylesheet" />
        <!-- Include Jet Zoom JavaScript -->
        <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/js/jetzoom.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>"></script>
        <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/js/starzoom.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>"></script>
        <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/css/starzoom.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/css" rel="stylesheet" />

        
        <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/js/jquery.fancybox.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>"></script>
        <link rel="stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/css/jquery.fancybox.css??v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/css" media="screen" />
        
                <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/zoom/css/thumbelina.css" type="text/css" rel="stylesheet" />

                <script type="text/javascript">
                    $(function () {
                        $('#zoom1').bind('click', function () {            // Bind a click event to a Cloud Zoom instance.
                            var cloudZoom = $(this).data('CloudZoom');  // On click, get the Cloud Zoom object,
                            cloudZoom.closeZoom();
                            $.fancybox.open(cloudZoom.getGalleryList()); // and pass Cloud Zoom's image list to Fancy Box.
                            return false;
                        });
                    });
</script>
        <script type="text/javascript">

            CloudZoom.quickStart(); // cz
            JetZoom.quickStart();
            StarZoom.quickStart();
        </script>

<%--<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/Micrositecss/colorbox.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>"  type="text/css">
<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/Micrositecss/smk-accordion.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css">--%>

<%--<link href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/Micrositecss/jquery.jcarousel.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" rel="stylesheet">
<link rel="Stylesheet" type="text/css" href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/Micrositecss/demo.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" >
<link rel="Stylesheet" type="text/css" href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/Micrositecss/gall.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" >
<link rel="Stylesheet" type="text/css" href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/Micrositecss/elastislide.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" >
<link href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/Micrositecss/jquery.fancybox.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" rel="Stylesheet" type="text/css" >--%>

<%--<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/Micrositecss/tabstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />--%>
<%--<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/thickboxAddtocart.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />--%>
<%--<script language="javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/thickboxaddtocart.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript" />--%>
 <link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/micrositecss/thickboxAddtocart_MS.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />
 <%--<script language="javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/micrositejs/thickboxAddtocart_MS.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript" />--%>


<%--<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/MicroSitejs/jquery.easing-1.3.pack.js" language="javascript" ></script>
<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/MicroSitejs/jquery.jcarousel.pack.js"></script>
<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/MicroSitejs/main.js"></script>
<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/MicroSitejs/jquery.tmpl.min.js"></script>
<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/MicroSitejs/jquery.easing.1.3.js"></script>
<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/MicroSitejs/jquery.elastislide.js"></script>
<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/MicroSitejs/gallery.js"></script>
<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/MicroSitejs/smk-accordion.js"></script>
<script src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/MicroSitejs/jquery.fancybox.js" type="text/javascript" ></script>
<script src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/MicroSitejs/jquery.easing-1.3.pack.js" type="text/javascript" ></script>--%>



   <script language="javascript" type="text/javascript">
       $(function () {
           $("#carousel li a").mouseover(function () {
               var mimg = $(this).children("img").attr("src");
               $("#slideshow-main  > a > img").attr("src", mimg.replace("_th50", "_images_200"));
               $("#slideshow-main  > a").attr("href", mimg.replace("_th50", "_images"));
           });
       });
    </script> 

<script type="text/javascript">
//		jQuery(document).ready(function($){
//			$(".accordion_example").smk_Accordion({
//				closeAble: true, //boolean
//			});			
//		});
</script>
<script type="text/javascript">
//    $(document).ready(function () {
//        $(".fancybox").fancybox();
//    });
    </script>

<script language="javascript" type="text/javascript">
    var captchavalid;
    function checkEmail(inputvalue) {
        var pattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
        if (pattern.test(inputvalue)) {
            return true;
        } else {
            return false;
        }
    }

    function validateNumber(event) {
        var key = window.event ? event.keyCode : event.which;

        if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 9
     || event.keyCode == 37 || event.keyCode == 39) {
            return true;
        }
        else if (key < 48 || key > 57) {
            return false;
        }
        else return true;
    }
    function Controlvalidate(ctype) {
        if (ctype == "fn") {

            var dd = document.getElementById("txtFullname");
            var err1 = document.getElementById("Errfullname");
            dd.setAttribute("value", dd.value);
            if (dd != null && dd.value == 0) {

                dd.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                dd.style.border = "";
                err1.style.display = "none";

            }
        }
        if (ctype == "ea") {
            var cno = document.getElementById("txtEmailAdd");
            var err1 = document.getElementById("erremailadd");
            var err2 = document.getElementById("errvalidmail");
            cno.setAttribute("value", cno.value);
            if (cno != null && cno.value == "") {
                cno.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {

                cno.style.border = "";
                err1.style.display = "none";

                var vaildemail = checkEmail(cno.value.trim());
                if (vaildemail == false) {
                    err2.style.display = "block";
                } else {
                    err2.style.display = "none";
                }
            }

        }
        if (ctype == "p") {
            var cn = document.getElementById("txtPhone");
            var err1 = document.getElementById("Errphone");
            cn.setAttribute("value", cn.value);
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                cn.style.border = "";
                err1.style.display = "none";
            }
        }
        if (ctype == "q") {
            var cn = document.getElementById("txtQuestionx");
            var err1 = document.getElementById("errquestion");
            cn.setAttribute("value", cn.value);
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                cn.style.border = "";
                err1.style.display = "none";
            }
        }
        if (ctype == "cc") {
            var cn = document.getElementById("txtCaptchCode");
            var err1 = document.getElementById("errCaptchCode");
            var err2 = document.getElementById("errCaptchInvalid");
            var err3 = document.getElementById("errCaptchCode1");
            cn.setAttribute("value", cn.value);
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                cn.style.border = "";
                err1.style.display = "none";


                if (cn.value != err3.innerHTML) {
                    err2.style.display = "block";
                } else {
                    err2.style.display = "none";
                }

            }
        }
    }

    function MailReset() {
        document.getElementById("txtEmailAdd").value = "";
        document.getElementById("txtFullname").value = "";
        document.getElementById("txtPhone").value = "";
        document.getElementById("txtQuestionx").value = "";
      //  document.getElementById("txtCaptchCode").value = "";
    }
    function strtrim(s) {
        s = s.replace(/(^\s*)|(\s*$)/gi, "");
        s = s.replace(/[ ]{2,}/gi, " ");
        s = s.replace(/\n /, "\n");
        return s;
    }

    function validateform() {
        var captcha_response = grecaptcha.getResponse(recaptcha1);
        if (captcha_response.length == 0) {
            return false;
        }
        else {
            return true;
        }
    }

    function MailSend() {


        var valid_g_captcha = validateform();
        if (valid_g_captcha == false) {
            alert("Invalid Captcha");
            return;
        }
       
        var ma = document.getElementById("txtEmailAdd");
        var fn = document.getElementById("txtFullname");
        var p = document.getElementById("txtPhone");
        var q = document.getElementById("txtQuestionx");
       // var cc = document.getElementById("txtCaptchCode");
        var pcode = document.getElementById("ProductCode");
        //var err3 = document.getElementById("errCaptchCode1");

        var valid = true;
        Controlvalidate("fn");
        Controlvalidate("ea");
        Controlvalidate("p");
        Controlvalidate("q");
      //  Controlvalidate("cc");
        if (fn == null || fn.value.trim() == "") {
            valid = false;
            // alert("enter Full Name")
            Controlvalidate("fn");
            fn.focus();
            return;
        }

        if (ma == null || ma.value.trim() == "") {
            valid = false;
            //  alert("enter Email id")
            ma.focus();
            return;
        }

        var vaildemail = checkEmail(ma.value.trim());
        if (vaildemail == false) {
            valid = false;
            //  alert("enter valid Email id")
            ma.focus();
            return;
        }
        if (p == null || p.value.trim() == "") {
            valid = false;
            //  alert("enter Phone Numbar")
            p.focus();
            return;
        }

        if (q == null || q.value.trim() == "") {
            valid = false;
            //   alert("enter Question")
            q.focus();
            return;
        }
//        if (cc == null || cc.value.trim() == "") {
//            valid = false;
//            //   alert("enter Question")
//            cc.focus();
//            return;
//        }
//        if (cc.value != err3.innerHTML) {
//            valid = false;
//            // err2.style.display = "block";
//            cc.focus();
//            return;
//        }


        //        var err2 = document.getElementById("errCaptchInvalid");
        //        err2.style.display = "none";
        //        if (captchavalid == false) {
        //            valid = false;
        //            err2.style.display = "block";
        //            return;
        //        }        




        var s = fn.value;
        fn.value = s.replace(/'/g, "`");
        s = q.value;
        q.value = s.replace(/'/g, "`");


        if (valid == true) {
            $.ajax({
                type: "POST",
                url: "/mpd.aspx/SendAskQuestionMail",
                data: "{'fromid':'" + ma.value.trim() + "','fname':'" + fn.value.trim() + "','phone':'" + p.value.trim() + "','qustion':'" + q.value.trim() + "','productcode':'" + pcode.innerHTML + "'}",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: OnmailSuccess,
                error: OnmailFailure


            });
        }



    }
    function OnmailSuccess(result) {

        var dt;
        if (result.d != null && result.d != "-1") {

            //var q = document.getElementById("divaskquestion");
            //var qs = document.getElementById("divAskQuestionSubmit");
            var q = document.getElementById("show_hide");
            var qs = document.getElementById("AQTSM");
            q.style.display = "none";
            qs.style.display = "block";
            //MailReset();
            //alert("Thank you for contacting The Wes, Inc. We will be contact you as soon as possible.");
        }
        else {
            alert("Unable to send your message. Please contact The Wes at Info@wes.com");
        }

    }
    function OnmailFailure(result) {
        alert("Unable to send your message. Please contact The Wes at Info@wes.com");
    }

    function textCounter(field, countfield, maxlimit) {
        var tf = document.getElementById(field);
        var cf = document.getElementById(countfield);
        tf.setAttribute("value", tf.value);
        if (tf.value.length > maxlimit) {
            tf.value = tf.value.substring(0, maxlimit);
            alert('Enquiry/Comments maximum allowed 600 characters.');
            return false;
        }
        else {
            cf.value = maxlimit - tf.value.length;
        }
    }

    function ValidateCaptcha() {
        var rtn = false;
        var p = document.getElementById("txtCaptchCode");
        $.ajax({
            type: "POST",
            url: "/pd.aspx/ValidateCaptcha",
            data: "{'secCode':'" + p.value.trim() + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d == -2) {
                    rtn = false;

                } else if (result.d == -1) {
                    rtn = false;
                }
            },
            error: function (result) {
                rtn = false;
            }


        });
        return rtn;
        //OnCaptchaSuccess,
    }
    function OnCaptchaSuccess(result) {
        if (result.d == -2) {
            captchavalid = false;
            //alert("Session time out");
        } else if (result.d == -1) {
            captchavalid = false;
            // alert("Invalid Code");
        }

    }
    function OnCaptchaFailure(result) {

        captchavalid = false;

    }


    
</script>
<%--productbuy code moved to MS_Alljs--%>
<%-- <script language="javascript" type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>scripts/jquery172.js"></script>--%>
 <script type="text/javascript">
     $(document).ready(function () {
         $("#lmo").hide();
         $("#preview").toggle(function () {
             $("#div1").hide();
             $("#div2").show();
             $("#lmo").show();
             $("#smo").hide();
         }, function () {
             $("#div1").show();
             $("#div2").hide();
             $("#smo").show();
             $("#lmo").hide();
         });
     });
</script>
 <script type="text/javascript">
     jQuery(document).ready(function () {
         $("img.lazy").lazyload();
     });
     $(document).ready(function () {
         //Default Action
         $(".tab_content").hide(); //Hide all content
         $("ul.tabs li:first").addClass("active").show(); //Activate first tab
         $(".tab_content:first").show(); //Show first tab content
         $(".withDownupdateproduct").hide();
         //On Click Event
         $("ul.tabs li").click(function () {
             $("ul.tabs li").removeClass("active"); //Remove any "active" class
             $(this).addClass("active"); //Add "active" class to selected tab
             $(".tab_content").hide(); //Hide all tab content
             $(".withDownupdateproduct").hide();
             var activeTab = $(this).find("a").attr("href"); //Find the rel attribute value to identify the active tab + content
             $(activeTab).fadeIn(); //Fade in the active content
             if (activeTab == "#tab2") {
                 $(".withDownupdateproduct").show();
             }
             return false;
         });
     });

</script>


</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
     
             <%=Bread_Crumbs_MS(true) %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     

      
        <uc1:ProductMS ID="ProductMS1" runat="server" />
        <script language="javascript" type="text/javascript">
            $("#ETALink").click(function (e) {

                e.preventDefault();
              
                var href = $(this).attr("href");
                window.location = href;
                $("#ETA_link").click();
            });
        </script>
<%--<script language="javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/micrositejs/thickboxAddtocart_MS.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript" />
--%>

<script  type="text/javascript" src="https://www.google.com/recaptcha/api.js?onload=myCallBack&render=explicit" async defer></script>

  <script language="javascript" type="text/javascript">
      var recaptcha1;
      var recaptcha2;
      var recaptcha3;
      var myCallBack = function () {
          recaptcha1 = grecaptcha.render('recaptcha1', {
              // 'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', //Replace this with your Site key
              'sitekey': '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>',
              'theme': 'light'
          });

          recaptcha2 = grecaptcha.render('recaptcha2', {
              //   'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', //Replace this with your Site key
              'sitekey': '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>',
              'theme': 'light'
          });

          recaptcha3 = grecaptcha.render('recaptcha3', {
              //'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', //Replace this with your Site key
              'sitekey': '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>',
              // 'callback': verifyCallback2,
              'theme': 'light'
          });
      };
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">


</asp:Content>

