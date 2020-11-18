<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Products" Codebehind="Product.ascx.cs" %>
<%--<%@ Register Assembly="CustomCaptcha" Namespace="CustomCaptcha" TagPrefix="cc1" %>--%>
  <script type="text/javascript" src="/scripts/product.js"></script>
  
   <script  type="text/javascript">
         var recaptcha1;
         var recaptcha2;
         var recaptcha3;
         var myCallBack = function () {
             recaptcha_p1 = grecaptcha.render('recaptcha1', {
                 'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', //Replace this with your Site key
                 'theme': 'light'
             });

             recaptcha2 = grecaptcha.render('recaptcha2', {
                 'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', //Replace this with your Site key
                 'theme': 'light'
             });

             recaptcha3 = grecaptcha.render('recaptcha3', {
                 'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', //Replace this with your Site key
                 'theme': 'light'
             });
         };
    </script>




<%--<script type="text/javascript">
    function test() {
        //var recaptcha_p1;
        //var myCallBack = function () {
        //    recaptcha_p1 = grecaptcha.render('recaptcha_p1', {
        //        'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', //Replace this with your Site key
        //        'theme': 'light'
        //    });
        //  };
      //  alert("1111");
        var rec_p1 = document.getElementById("recaptcha_p1");
        rec_p1.style.display = "block";
        var rec_p2 = document.getElementById("recaptcha_p2");
        rec_p2.style.display = "none";
    }
    function test1() {
        //var recaptcha2;
        //var myCallBack = function () {
        //     recaptcha2 = grecaptcha.render('recaptcha_p2', {
        //        'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', //Replace this with your Site key
        //        'theme': 'light'
        //    });
        //};
        // alert("2222");

        var rec_p1 = document.getElementById("recaptcha_p1");
        rec_p1.style.display = "none";
        var rec_p2 = document.getElementById("recaptcha_p2");
        rec_p2.style.display = "block";
    }

</script>--%>
 
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




    function validateform() {
        var captcha_response = grecaptcha.getResponse(recaptcha1);
        if (captcha_response.length == 0) {
            // Captcha is not Passed
            return false;
        }
        else {
            // Captcha is Passed
            return true;
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
           // var err3 = document.getElementById("errCaptchCode1");
           // alert(cn.value);
           // alert(err3.innerHTML);
            // alert($('#errCaptchCode1').attr('innerHTML'));
            var err3 = $('#errCaptchCode1').attr('innerHTML');
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                cn.style.border = "";
                err1.style.display = "none";

               
                if (cn.value != err3 && cn.value != null) {
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
       // document.getElementById("txtCaptchCode").value = "";
    }
    function strtrim(s) {
        s = s.replace(/(^\s*)|(\s*$)/gi, "");
        s = s.replace(/[ ]{2,}/gi, " ");
        s = s.replace(/\n /, "\n");
        return s;
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
        //var cc = document.getElementById("txtCaptchCode");
        var pcode = document.getElementById("ProductCode");
        //var err3 = document.getElementById("errCaptchCode1");
       // var err3 = $('#errCaptchCode1').attr('innerHTML');
        var valid = true;
        Controlvalidate("fn");
        Controlvalidate("ea");
        Controlvalidate("p");
        Controlvalidate("q");
       // Controlvalidate("cc");
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
//        if (cc.value != err3) {
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
                url: "/pd.aspx/SendAskQuestionMail",
                data: "{'fromid':'" + ma.value.trim() + "','fname':'" + fn.value.trim() + "','phone':'" + p.value.trim() + "','qustion':'" + q.value.trim() + "','productcode':'" + pcode.innerHTML + "'}",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: OnmailSuccess,
                error: OnmailFailure


            });
        }



    }

//    function createcaptcha() {
//        $.ajax({
//            type: "GET",
//            url: "/wagcaptcha.aspx",
//            data: "{}",
//            contentType: "application/json;charset=utf-8",
//            dataType: "json",
//            success: function (data) {
//            }
//        });
//    }

//    function getcaptcha() {
//        createcaptcha();
//        $.ajax({
//            type: "POST",
//            url: "/wagcaptcha.aspx/getcaptchaimage",
//            data: "{}",
//            contentType: "application/json;charset=utf-8",
//            dataType: "json",
//            success: function (data) {
//                //alert(data.d.toString().split(',,')[0]);
//                $('#captcha_image').attr('src', data.d.toString().split(',,')[0]);
//                $('#errCaptchCode1').attr('innerHTML', data.d.toString().split(',,')[1]);

//                $('#captcha_image_BBPP').attr('src', data.d.toString().split(',,')[0]);
//                $('#errCaptchCode1_BBPP').attr('innerHTML', data.d.toString().split(',,')[1]);

//                $('#captcha_image_DU').attr('src', data.d.toString().split(',,')[0]);
//                $('#errCaptchCode1_DU').attr('innerHTML', data.d.toString().split(',,')[1]);
//            }
//        });
//    }

    function OnmailSuccess(result) {

        var dt;
        if (result.d != null && result.d != "-1") {

            var q = document.getElementById("messages");
            var qs = document.getElementById("divAskQuestionSubmit");
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
        if (field.value.length > maxlimit) {
            field.value = field.value.substring(0, maxlimit);
            alert('Enquiry/Comments maximum allowed 600 characters.');
            return false;
        }
        else {
            countfield.value = maxlimit - field.value.length;
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

<script language="javascript" type="text/javascript">
    function productbuy(buyvalue, pid) {

        //  var qtyval = document.forms[0].elements[buyvalue].value.trim();
        var qtyval = document.getElementById(buyvalue).value.trim();
        //var qtyavail = document.forms[0].elements[buyvalue].name;
        //qtyavail = qtyavail.toString().split('_')[1];
        var qtyavail = buyvalue.toString().split('_')[1];
       // var minordqty = document.forms[0].elements[buyvalue].name;
       // minordqty = minordqty.toString().split('_')[2];
        var minordqty = buyvalue;
        minordqty = minordqty.toString().split('_')[2];

        //var fid = document.forms[0].elements[buyvalue].name;
        // fid = fid.toString().split('_')[3];
        var fid = buyvalue;
        fid = fid.toString().split('_')[3];

        var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";


        if (isNaN(qtyval) || qtyval == "" || qtyval <= 0 || qtyval.indexOf(".") != -1) {
            alert('Invalid Quantity!');
            window.document.forms[0].elements[buyvalue].style.borderColor = "red";
            document.forms[0].elements[buyvalue].focus();
            return false;
        }
             else {
                 // window.document.location = '/OrderDetails.aspx?&amp;bulkorder=1&amp;Pid=' + pid + '&amp;Qty=' + qtyval;
                 CallProductPopup(orgurl, buyvalue, pid, qtyval, 0, fid);
        }
    }
    function keyct(e) {        
        var keyCode = (e.keyCode ? e.keyCode : e.which);
        if (keyCode == 8 || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) {

        }
        else {
            e.preventDefault();
        }
    }

    function videoshow() {

        $("#productvideo").css({ 'display': 'block' });
        $("#product-image").css({ 'display': 'none' });

        $("#ifrme")[0].src += "&autoplay=1";
        ev.preventDefault();
    }
    function Imageshow() {

        $("#productvideo").css({ 'display': 'none' });
        $("#product-image").css({ 'display': 'block' });

    }
</script>
<%-- <script language="javascript" type="text/javascript" src="/scripts/jquery172.js"></script>--%>
 <script type="text/javascript">
     $(document).ready(function () {
        // $("#lmo").hide();
      
         $("#preview").toggle(function () {
             $("#div1").hide();
             $("#div2").show();
             // $("#lmo").show();
            //  $("#smo").hide();
         }, function () {
             $("#div1").show();
             $("#div2").hide();
             // $("#smo").show();
             // $("#lmo").hide();
         });
     });

     function showmoredes() {
         var div1dis = $("#div1").css("display");
         var div2dis = $("#div2").css("display");
          if (div1dis == "block") {
             $("#div1").removeAttr('style');
             $("#div1").css({ 'display': 'none' });
             $("#div2").removeAttr('style');
             $("#div2").css({ 'display': 'block' });

             if (div1dis == "block") {

                 $("#lessmore").removeAttr('style');
                 $("#lessmore").css({ 'display': 'block' });
                 $("#lessmore").css({ 'cursor': 'pointer' });

                 $("#smlm").removeAttr('style');
                 $("#smlm").css({ 'display': 'none' });
                 $("#smlm").css({ 'cursor': 'pointer' });

             }
         }
         else {
             $("#div1").removeAttr('style');
             $("#div1").css({ 'display': 'block' });
             $("#div2").removeAttr('style');
             $("#div2").css({ 'display': 'none' });

             $("#lessmore").removeAttr('style');
             $("#lessmore").css({ 'display': 'none' });
             $("#lessmore").css({ 'cursor': 'pointer' });

             $("#smlm").removeAttr('style');
             $("#smlm").css({ 'display': 'block' });
             $("#smlm").css({ 'cursor': 'pointer' });
         }
     }

</script>
   <script type="text/javascript">
       function increment(id) {
           var input = document.getElementById(id);
           input.value = parseInt(input.value) + 1;
       }
       function decrement(id) {
           var input = document.getElementById(id);
           if (parseInt(input.value) - 1 < 1) {
               input.value = 1;
           }
           else {
               input.value = parseInt(input.value) - 1;
           }
       }
</script>
<%-- <script type="text/javascript">
     $(document).ready(function () {
         //Default Action
         $(".tab_content").hide(); //Hide all content
         $("ul.tabs li:first").addClass("active").show(); //Activate first tab
         $(".tab_content:first").show(); //Show first tab content
         $(".withDownupdateproduct").hide();
         //On Click Event
         $(".tabs ul li a").click(function () {
            // alert("11");
             //             $("ul.tabs li").removeClass("active"); //Remove any "active" class
             //             $(this).addClass("active"); //Add "active" class to selected tab
             //             $(".tab_content").hide(); //Hide all tab content
             //             $(".withDownupdateproduct").hide();
             var activeTab = $(this).attr("href"); //Find the rel attribute value to identify the active tab + content
             //  $(activeTab).fadeIn(); //Fade in the active content
             if (activeTab == "#messages" || activeTab == "#bulkbuy" || activeTab == "#downloads") {
                 getcaptcha();
             }
             if (activeTab == "#downloads") {
                 $(".withDownupdateproduct").show();
             }

             return false;
         });
     });
</script>--%>

<%-- <script type="text/javascript">
     $(document).ready(function () {
         //Default Action
         //$(".tab_content").hide(); //Hide all content
         //$("ul.tabs li:first").addClass("active").show(); //Activate first tab
         //$(".tab_content:first").show(); //Show first tab content
         //$(".withDownupdateproduct").hide();
         //On Click Event
         $(".tabs ul li a").click(function () {
            // alert("11");
             //             $("ul.tabs li").removeClass("active"); //Remove any "active" class
             //             $(this).addClass("active"); //Add "active" class to selected tab
             //             $(".tab_content").hide(); //Hide all tab content
             //             $(".withDownupdateproduct").hide();
             var activeTab = $(this).attr("href"); //Find the rel attribute value to identify the active tab + content
             //  $(activeTab).fadeIn(); //Fade in the active content
             alert(activeTab);
             if (activeTab == "#tab2") {
                 test();
             }
             if (activeTab == "#tab3") {
                 test1();
             }
             if (activeTab == "#tab4") {

             }
             //if (activeTab == "#downloads") {
             //    $(".withDownupdateproduct").show();
             //}

             //return false;
         });
     });
</script>--%>
<script type="text/javascript">
    $(document).ready(function () {
        //Default Action
        // $(".tab-content").hide();
        $("#myTab li:first").addClass("active").show();
        $(".tab_content:first").show();
        $(".withDownupdate").hide();
        //On Click Event
        $("#myTab li").click(function () {
            $("#myTab li").removeClass("active");
            $(this).addClass("active");
            // $(".tab-content").hide();
            $(".withDownupdate").hide();
            var activeTab = $(this).find("a").attr("href");
            $(activeTab).fadeIn();
//            if (activeTab == "#tab2" || activeTab == "#tab3" || activeTab == "#tab4") {
//                getcaptcha();
//            }
            if (activeTab == "#tab4") {
                $(".withDownupdate").show();
            }

            return false;
        });
    });
</script>
<%--<script type="text/javascript" language="javascript">
    $('html').click(function (e) {
        if (e.target.id == 'jquery-lightbox') {
            $("#jquery-lightbox").remove();
            $("#jquery-overlay").remove();
            // $("#imageinnerdiv").css('margin-top', '0px');
            $(document).unbind();
        }
    });

    function btn_popupclose() {
        $("#jquery-lightbox").remove();
        $("#jquery-overlay").remove();
        $(document).unbind();
    }
</script>--%>

<%--<script type="text/javascript" language="javascript">
    function scrolldown() {

        // $("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() - 140 }, 500);
        $("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() - 400 }, 500);
        var stop = $("#lightbox-image-details").scrollLeft();
        if (stop == '0') {

            $("#lightbox-image-up").css({ 'visibility': 'hidden' });
            $("#lightbox-image-up").css({ 'cursor': 'default' });
        }
        else {
            $("#lightbox-image-down").css({ 'visibility': 'visible' });
            $("#lightbox-image-down").css({ 'cursor': 'pointer' });
        }
    }
    var j = 0;
    function scrollup() {

        //$("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() + 140 }, 500);
        $("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() + 400 }, 500);
        $("#lightbox-image-up").css({ 'visibility': 'visible' });
        $("#lightbox-image-up").css({ 'cursor': 'pointer' });
        var sbot = $(window).scrollLeft() + $(window).width();
        // if ($("#lightbox-image-details").scrollLeft() + 100 >= $("#lightbox-image-details-caption").innerWidth()) {

        // alert($("#lightbox-image-details").scrollLeft());
        // alert($("#lightbox-image-details-caption").innerWidth());
        var ua = window.navigator.userAgent;
        var msiechk = ua.indexOf("MSIE ");
        var lidcwidth = 0;
        if (navigator.userAgent.toLowerCase().indexOf('chrome') >= 64 || msiechk > 0) {
            lidcwidth = $("#lightbox-image-details-caption").innerWidth() + 1370;
        }
        else {
            lidcwidth = $("#lightbox-image-details-caption").innerWidth()
        }
        if ($("#lightbox-image-details").scrollLeft() + 400 >= lidcwidth) {
            $("#lightbox-image-details").animate({ "scrollLeft": 0 }, 500);
            $("#lightbox-image-up").css({ 'visibility': 'hidden' });
            $("#lightbox-image-up").css({ 'cursor': 'default' });
        }

        //alert($("#lightbox-image-details-caption").scrollWidth)
        //alert(document.getElementById('lightbox-image-details-caption').scrollWidth);

    }


    $(document).ready(function () {
        var dpy = $("#avrp").css("display");
        if (dpy == "none") {
            $("#avpc").removeAttr('style');
            $("#avpc").css({ 'float': 'right' });
        }
        else {
            $("#avpc").css({ 'float': 'left !important' });
        }
    });

</script>--%>
  <div class="container">
<%--<table align="center" width="100%" border="0" cellspacing="0" cellpadding="0" class="padL">
     <tr><td align="center"><table align="center" width="100%" border="0" cellspacing="0" cellpadding="0" >
                 <tr><td>--%>
                 <div class="row">
                 <%Response.Write(Bread_Crumbs());  %>
                 </div>
                <%-- </td></tr>
                  <tr><td height="4px"></td></tr></table>                 
      <table width="100%"  border="0" cellspacing="0" cellpadding="0">
        <tr><td >  <div class="box1 bx1div">--%>
                   
  
                  <% Response.Write(ST_Product()); %> 
                  </div>
                  
               <%-- <input type="text" name="g_captcha_val_f_aqt" id="g_captcha_val_f_aqt" runat="server" class="H1Tag" style="display:none;"/> --%> 
                  <%--</div> </td></tr>  </table></td> </tr></table>--%>



