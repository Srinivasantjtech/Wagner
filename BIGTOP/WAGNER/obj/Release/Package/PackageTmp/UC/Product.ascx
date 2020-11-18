<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Products" Codebehind="Product.ascx.cs" %>
<%--<%@ Register Assembly="CustomCaptcha" Namespace="CustomCaptcha" TagPrefix="cc1" %>--%>
  <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() %>scripts/product.js"></script>
  <input type="hidden" value="false" id="valrecaptcha1" />
<input type="hidden" value="false" id="valrecaptcha2" />
<input type="hidden" value="false" id="valrecaptcha3" />

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
            if (dd != null && dd.value == 0) {

                dd.style.border = "1px solid #FF0000";
             
                document.getElementById("Errfullname").classList.remove('dpynone');
                document.getElementById("Errfullname").className += " dpyblk";
            }
            else {
                dd.style.border = "";
             
                document.getElementById("Errfullname").classList.remove('dpyblk');
                document.getElementById("Errfullname").className += " dpynone";

            }
        }
        if (ctype == "ea") {
            var cno = document.getElementById("txtEmailAdd");
            var err1 = document.getElementById("erremailadd");
            var err2 = document.getElementById("errvalidmail");
            if (cno != null && cno.value == "") {
                cno.style.border = "1px solid #FF0000";
                
                document.getElementById("erremailadd").classList.remove('dpynone');
                document.getElementById("erremailadd").className += " dpyblk";
            }
            else {

                cno.style.border = "";
               
                document.getElementById("erremailadd").classList.remove('dpyblk');
                document.getElementById("erremailadd").className += " dpynone";

                var vaildemail = checkEmail(cno.value.trim());
                if (vaildemail == false) {
                  
                    document.getElementById("errvalidmail").classList.remove('dpynone');
                    document.getElementById("errvalidmail").className += " dpyblk";
                } else {
                  
                    document.getElementById("errvalidmail").classList.remove('dpyblk');
                    document.getElementById("errvalidmail").className += " dpynone";
                }
            }

        }
        if (ctype == "p") {
            var cn = document.getElementById("txtPhone");
            var err1 = document.getElementById("Errphone");
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                
                document.getElementById("Errphone").classList.remove('dpynone');
                document.getElementById("Errphone").className += " dpyblk";
            }
            else {
                cn.style.border = "";
               
                document.getElementById("Errphone").classList.remove('dpyblk');
                document.getElementById("Errphone").className += " dpynone";
            }
        }
        if (ctype == "q") {
            var cn = document.getElementById("txtQuestionx");
            var err1 = document.getElementById("errquestion");
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
               
                document.getElementById("errquestion").classList.remove('dpynone');
                document.getElementById("errquestion").className += " dpyblk";
            }
            else {
                cn.style.border = "";
             
                document.getElementById("errquestion").classList.remove('dpyblk');
                document.getElementById("errquestion").className += " dpynone";
            }
        }
        if (ctype == "cc") {
      
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
      
    }
    function strtrim(s) {
        s = s.replace(/(^\s*)|(\s*$)/gi, "");
        s = s.replace(/[ ]{2,}/gi, " ");
        s = s.replace(/\n /, "\n");
        return s;
    }

    function validateform() {
        //alert(captchaid);
       // var cap_id = captchaid;
        // var captcha_response = grecaptcha.getResponse(recaptcha1);
        var captcha_response = grecaptcha.getResponse(recaptcha1);
       // alert(captcha_response);
        if (captcha_response.length == 0) {

            return false;
        }
        else {

            return true;
        }
    }

    function MailSend() {

       // var valid_g_captcha = validateform();

        //if (valid_g_captcha == false) {
        //    alert("Invalid Captcha");
        //    return;
       // }

        //var id = document.getElementById("g-recaptcha-response");
       // alert(id.response);
        // alert(document.getElementById("valrecaptcha1").value);

       // var x = document.getElementsByClassName("rc-anchor-error-msg-container");

      

        var valid_g_captcha = document.getElementById("valrecaptcha1").value;
        if (valid_g_captcha == "false") {
            alert("Invalid Captcha");
            return;
        }

       // var response = grecaptcha.getResponse(recaptcha1);
       // alert(response);

        var ma = document.getElementById("txtEmailAdd");
        var fn = document.getElementById("txtFullname");
        var p = document.getElementById("txtPhone");
        var q = document.getElementById("txtQuestionx");
      
        var pcode = document.getElementById("ProductCode");
     
        var valid = true;
        Controlvalidate("fn");
        Controlvalidate("ea");
        Controlvalidate("p");
        Controlvalidate("q");
      
        if (fn == null || fn.value.trim() == "") {
            valid = false;
          
            Controlvalidate("fn");
            fn.focus();
            return;
        }

        if (ma == null || ma.value.trim() == "") {
            valid = false;
         
            ma.focus();
            return;
        }

        var vaildemail = checkEmail(ma.value.trim());
        if (vaildemail == false) {
            valid = false;
         
            ma.focus();
            return;
        }
        if (p == null || p.value.trim() == "") {
            valid = false;
          
            p.focus();
            return;
        }

        if (q == null || q.value.trim() == "") {
            valid = false;
           
            q.focus();
            return;
        }
       




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



    function OnmailSuccess(result) {

        var dt;
        if (result.d != null && result.d != "-1") {

            var q = document.getElementById("messages");
            var qs = document.getElementById("divAskQuestionSubmit");
            q.style.display = "none";
            qs.style.display = "block";
           
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
       
    }
    function OnCaptchaSuccess(result) {
        if (result.d == -2) {
            captchavalid = false;
           
        } else if (result.d == -1) {
            captchavalid = false;
           
        }

    }
    function OnCaptchaFailure(result) {

        captchavalid = false;

    }

    function shippinginfoload() {
      
        var testval = "shippinginfo";
        var shipload = document.getElementById("shippingLoad");
        if (shipload != null) {
            $.ajax({
                type: "POST",
                url: "/pd.aspx/Shiipinginfo",
                data: "{'value':'" + testval + "'}",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: OnShippingSuccess,
                error: OnShippingFailure


            });
        }
    }
    function OnShippingSuccess(result) {

        //var dt;
        //alert(result.d);
        // alert(result);
        if (result.d != null && result.d != "-1") {

            var q = document.getElementById("shipping");

            // q.style.display = "none";
            // qs.style.display = "block";
            q.innerHTML = result.d;
            var shipload = document.getElementById("shippingLoad");
            if (shipload != null) {
                shipload.style.display = "none";
            }

        }
        else {
            alert("Try after some time...");
        }

    }
    function OnShippingFailure(result) {
        alert("Unable to send your message. Please contact The Wes at sales@wagneronline.com.au");
    }
    function AskaQuestionLoad() {

        var testval = "AskaQuestion";
        var shipload = document.getElementById("askaquestionLoad");
        if (shipload != null) {
            $.ajax({
                type: "POST",
                url: "/pd.aspx/AskaQuestionLoad",
                data: "{'value':'" + testval + "'}",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: OnAskqQuestionSuccess,
                error: OnAskqQuestionFailure


            });
        }
    }

    var verifyCallbackAsk = function (response) {
        if (response.length !== 0) {
            document.getElementById("valrecaptcha1").value = "true";
        }
    };
    var expCallbackAsk = function () {
        // grecaptcha.reset();
        document.getElementById("valrecaptcha1").value = "false";
    };
    var verifyCallbackBulkBuy = function (response) {
        if (response.length !== 0) {
            document.getElementById("valrecaptcha2").value = "true";
        }
    };
    
    var expCallbackBulkBuy = function () {
        // grecaptcha.reset();
        document.getElementById("valrecaptcha2").value = "false";
    };

    function OnAskqQuestionSuccess(result) {

        if (result.d != null && result.d != "-1") {
            var q = document.getElementById("tab2");
            q.innerHTML = result.d;
            var askaqload = document.getElementById("askaquestionLoad");
            if (askaqload != null) {
                askaqload.style.display = "none";
            }

            var recaptcha1;
            var myCallBack = function () {
                recaptcha1 = grecaptcha.render('recaptcha1', {
                    'sitekey': '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>',
                    'theme': 'light',
                    'callback': verifyCallbackAsk,
                    'expired-callback': expCallbackAsk
                });


              };
             myCallBack();

            //grecaptcha.render($('#recaptcha1')[0], { sitekey: '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>' });

        }
        else {
            alert("Try after some time...");
        }

    }
    function OnAskqQuestionFailure(result) {
        alert("Unable to send your message. Please contact The Wes at sales@wagneronline.com.au");
    }
    function BulkBuyLoad() {
        var testval = "bulkbuy";
        var bulkbuyLoad = document.getElementById("bulkbuyLoad");
        if (bulkbuyLoad != null) {
            $.ajax({
                type: "POST",
                url: "/pd.aspx/BulkBuyLoad",
                data: "{'value':'" + testval + "'}",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: BulkBuySuccess,
                error: BulkBuyFailure


            });
        }
    }

    function BulkBuyFailure(result) {
        alert("Unable to send your message. Please contact The Wes at sales@wagneronline.com.au");
    }

    function BulkBuySuccess(result) {

        if (result.d != null && result.d != "-1") {
            var q = document.getElementById("tab3");
            q.innerHTML = result.d;
            var bulkbuyLoad = document.getElementById("bulkbuyLoad");
            if (bulkbuyLoad != null) {
                bulkbuyLoad.style.display = "none";
            }
            var recaptcha2;
            var myCallBack = function () {
                recaptcha2 = grecaptcha.render('recaptcha2', {
                    'sitekey': '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>',
                    'theme': 'light',
                    'callback': verifyCallbackBulkBuy,
                    'expired-callback': expCallbackBulkBuy
                });


            };
            myCallBack();


        }
        else {
            alert("Try after some time...");
        }

    }


    function DownloadLoad_ST() {
        var productID = document.getElementById("TBT_PRODUCT_ID").value;
        // alert(productID);
        //var testval = "sk";
        var DownloadLoad = document.getElementById("DownloadLoad");
        if (DownloadLoad != null) {
            $.ajax({
                type: "POST",
                url: "/pd.aspx/ProductDownloadLoad",
                data: "{'value':'" + productID + "'}",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: DownloadSuccess,
                error: DownloadFailure


            });
        }
    }
    function DownloadFailure(result) {
        alert("Unable to send your message. Please contact The Wes at sales@wagneronline.com.au");
    }
    function DownloadSuccess(result) {

        if (result.d != null && result.d != "-1") {
            var q = document.getElementById("tab4");
            q.innerHTML = result.d;
            var DownloadLoad = document.getElementById("DownloadLoad");
            if (DownloadLoad != null) {
                DownloadLoad.style.display = "none";
            }
            var recaptcha3;
            var myCallBack = function () {
                recaptcha3 = grecaptcha.render('recaptcha3', {
                    'sitekey': '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>',
                    'theme': 'light',
                    'callback': verifyCallbackDownload,
                    'expired-callback': expCallbackDownload
                });


            };
            myCallBack();


        }
        else {
            alert("Try after some time...");
        }

    }
    var verifyCallbackDownload = function (response) {
        if (response.length !== 0) {
            document.getElementById("valrecaptcha3").value = "true";
        }
    };

    var expCallbackDownload = function () {
        document.getElementById("valrecaptcha3").value = "false";
    };
</script>

<script language="javascript" type="text/javascript">
    function productbuy(buyvalue, pid) {

      
        var qtyval = document.getElementById(buyvalue).value.trim();
    
        var qtyavail = buyvalue.toString().split('_')[1];
  
        var minordqty = buyvalue;
        minordqty = minordqty.toString().split('_')[2];

  
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
 <script type="text/javascript">
     $(document).ready(function () {
         
         $("#preview").toggle(function () {
             $("#div1").hide();
             $("#div2").show();
   
         }, function () {
             $("#div1").show();
             $("#div2").hide();
 
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

<script type="text/javascript">
    $(document).ready(function () {
   
        $("#myTab li:first").addClass("active").show();
        $(".tab_content:first").show();
        $(".withDownupdate").hide();
       
        $("#myTab li").click(function () {
            $("#myTab li").removeClass("active");
            $(this).addClass("active");
          
            $(".withDownupdate").hide();
            var activeTab = $(this).find("a").attr("href");
            $(activeTab).fadeIn();

            if (activeTab == "#tab4") {
                $(".withDownupdate").show();
            }
            if (activeTab == "#shipping") {
                var shipload = document.getElementById("shippingLoad");
                if (shipload != null) {
                    shipload.style.display = "block";
                }
                shippinginfoload();
            }
            if (activeTab == "#tab2") {
                var askload = document.getElementById("askaquestionLoad");
                if (askload != null) {
                    askload.style.display = "block";
                }
                AskaQuestionLoad();
            }
            if (activeTab == "#tab3") {
                var bulkbuyLoad = document.getElementById("bulkbuyLoad");
                if (bulkbuyLoad != null) {
                    bulkbuyLoad.style.display = "block";
                }
                BulkBuyLoad();
            }

            if (activeTab == "#tab4") {
                var DownloadLoad = document.getElementById("DownloadLoad");
                if (DownloadLoad != null) {
                    DownloadLoad.style.display = "block";
                }
                DownloadLoad_ST();
            }
            return false;
        });
    });
</script>

<div class="container">
    <div class="row">
        <% Response.Write(Bread_Crumbs()); %>
    </div>               
    <div class="row">
        <% Response.Write(ST_Product()); %> 
    </div>                
</div>
                  




