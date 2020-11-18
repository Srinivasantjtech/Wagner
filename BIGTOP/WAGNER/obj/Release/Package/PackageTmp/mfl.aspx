<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mfl.aspx.cs" Inherits="WES.mfl" %>
<%@ Register Src="UC/MICROSITE/familyMS.ascx" TagName="familyMS" TagPrefix="uc1" %>
<asp:Content ID="Content7" ContentPlaceHolderID="metatag" Runat="Server">
         <asp:Literal runat="server" ID="litMeta" />
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

<script  type="text/javascript" src="https://www.google.com/recaptcha/api.js?onload=myCallBack&render=explicit"></script>
<%--Validation code moved to MS_Alljs--%>

<%--productbuy code moved to MS_Alljs--%>
 <script language="javascript" type="text/javascript">
     var recaptcha1;
     var recaptcha2;
     var recaptcha3;
     var myCallBack = function () {
         recaptcha1 = grecaptcha.render('recaptcha1', {
             // 'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', 
             'sitekey': '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>',
             'theme': 'light'
         });

         recaptcha2 = grecaptcha.render('recaptcha2', {
             // 'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', 
             'sitekey': '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>',
             'theme': 'light'
         });

         recaptcha3 = grecaptcha.render('recaptcha3', {
             //'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', 
             'sitekey': '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>',
             // 'callback': verifyCallback2,
             'theme': 'light'
         });
     };
    </script>

<script language="javascript" type="text/javascript">
    function OnclickTab(Attribute) {
        var elem = document.getElementById(Attribute);
        var defValue = elem.value;
      
        document.getElementById("hfclickedattr").value = defValue;
        
        document.forms[0].submit();

    }
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
    function textCounter(field, countfield, maxlimit) {
        //  alert(maxlimit);
        field = document.getElementById("txtQuestionx").value.length;
        //alert(field);
        var field_value = document.getElementById("txtQuestionx").value;
        var cf_value = document.getElementById("countfield").value;

        if (field > 600) {
            document.getElementById("txtQuestionx").value = document.getElementById("txtQuestionx").value.substring(0, maxlimit);
            alert('Enquiry/Comments maximum allowed 600 characters.');
            return false;
        }
        else {
            document.getElementById("countfield").value = maxlimit - field;
        }
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
        var fname = document.getElementById("familyName");
    
    //var err3 = document.getElementById("errCaptchCode1");
       // var err3 = $('#errCaptchCode1').attr('innerHTML');
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
        //if (cc == null || cc.value.trim() == "") {
          
        //    valid = false;
        //    //   alert("enter Question")
        //    cc.focus();
        //    return;
        //}

        //if (cc.value != err3.innerHTML) {
         
        //    valid = false;
        //    // err2.style.display = "block";
        //    cc.focus();
        //    return;
        //}


      




        var s = fn.value;
        fn.value = s.replace(/'/g, "`");
        s = q.value;
        q.value = s.replace(/'/g, "`");

       
        if (valid == true) {
      
            $.ajax({
                type: "POST",
                url: "/mfl.aspx/SendAskQuestionMail",
                data: "{'fromid':'" + ma.value.trim() + "','fname':'" + fn.value.trim() + "','phone':'" + p.value.trim() + "','qustion':'" + q.value.trim() + "','familyName':'" + fname.innerHTML + "'}",
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

            var q = document.getElementById("divaskquestion");
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
    function validateform_BulkBuyPP() {
        var captcha_response = grecaptcha.getResponse(recaptcha2);
        if (captcha_response.length == 0) {
            return false;
        }
        else {
            return true;
        }
    }
    function MailSend_BulkBuyPP() {
        var valid_g_captcha = validateform_BulkBuyPP();

        if (valid_g_captcha == false) {
            alert("Invalid Captcha");
            return;
        }

        var email = document.getElementById("txtEmail");
        var fullname = document.getElementById("txtFullname_BBPP");
        var deltime = document.getElementById("txtdeliverytime");
        var phone = document.getElementById("txtPhone_BBPP");
        var notes = document.getElementById("txtnotesadditionalinfo");
        var fname_bbpp = document.getElementById("familyName");
        var tarprice = document.getElementById("txttargetprice");
        var qty = document.getElementById("txtQTY");
        // var procode = document.getElementById("txtproductcode");
      //  var capcode = document.getElementById("txtCaptchCode_BBPP");
      //  var caperr = document.getElementById("errCaptchCode1_BBPP");
        var valid = true;
        Controlvalidate_BulkBuyPP("fullname");
        Controlvalidate_BulkBuyPP("email");
        Controlvalidate_BulkBuyPP("phone");
        // Controlvalidate_BulkBuyPP("notes");
        Controlvalidate_BulkBuyPP("deltime");
        Controlvalidate_BulkBuyPP("qty");
       // Controlvalidate_BulkBuyPP("capcode");
        var e = document.getElementById("ddlprodcode");
        var procode = e.options[e.selectedIndex].text;
        Controlvalidate_BulkBuyPP("procode");
        if (fullname == null || fullname.value.trim() == "") {
            valid = false;
            // alert("enter Full Name")
            Controlvalidate_BulkBuyPP("fullname");
            fullname.focus();
            return;
        }

        if (procode == "Please Select Product") {
            valid = false;
            procode.focus();
            return;
        }
        if (email == null || email.value.trim() == "") {
            valid = false;
            //  alert("enter Email id")
            email.focus();
            return;
        }

        var vaildemail = checkEmail(email.value.trim());
        if (vaildemail == false) {
            valid = false;
            email.focus();
            return;
        }
//        if (capcode.value != caperr.innerHTML) {
//            valid = false;
//            // err2.style.display = "block";
//            capcode.focus();
//            return;
//        }
        if (phone == null || phone.value.trim() == "") {
            valid = false;
            phone.focus();
            return;
        }
        if (deltime == null || deltime.value.trim() == "") {
            valid = false;
            deltime.focus();
            return;
        }

        //        if (notes == null || notes.value.trim() == "") {
        //            valid = false;
        //            notes.focus();
        //            return;
        //        }
        if (qty == null || qty.value.trim() == "") {
            valid = false;
            notes.focus();
            return;
        }






        var s = fullname.value;
        fullname.value = s.replace(/'/g, "`");
        s = notes.value;
        notes.value = s.replace(/'/g, "`");


        if (valid == true) {
            $.ajax({
                type: "POST",
                url: "/mfl.aspx/SendBulkBuyProjectPricing",
                data: "{'productcode':'" + procode + "','fullname':'" + fullname.value.trim() + "','qty':'" + qty.value.trim() + "','fromid':'" + email.value.trim() + "','deliverytime':'" + deltime.value.trim() + "','phone':'" + phone.value.trim() + "','targetprice':'" + tarprice.value.trim() + "','notesandaddtionalinfo':'" + notes.value.trim() + "','familyName':'" + fname_bbpp.innerHTML + "'}",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: OnmailSuccess_BBPP,
                error: OnmailFailure_BBPP


            });
        }



    }

    function Controlvalidate_BulkBuyPP(ctype) {
        if (ctype == "fullname") {
            var dd = document.getElementById("txtFullname_BBPP");
            var err1 = document.getElementById("Errfullname_BBPP");
            if (dd != null && dd.value == 0) {

                dd.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                dd.style.border = "";
                err1.style.display = "none";

            }
        }
        if (ctype == "deltime") {
            var dd = document.getElementById("txtdeliverytime");
            var err1 = document.getElementById("Errdeliverytime");
            if (dd != null && dd.value == 0) {

                dd.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                dd.style.border = "";
                err1.style.display = "none";

            }
        }

        if (ctype == "procode") {

            var dd = document.getElementById("ddlprodcode");
            var err1 = document.getElementById("Errprocode_BBPP");
            if (dd != null && dd.value == "Please Select Product") {

                dd.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                dd.style.border = "";
                err1.style.display = "none";

            }
        }
        if (ctype == "qty") {
            var dd = document.getElementById("txtQTY");
            var err1 = document.getElementById("ErrQTY");
            if (dd != null && dd.value == 0) {

                dd.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                dd.style.border = "";
                err1.style.display = "none";

            }
        }
        if (ctype == "email") {
            var cno = document.getElementById("txtEmail");
            var err1 = document.getElementById("erremailadd_BBPP");
            var err2 = document.getElementById("errvalidmail_BBPP");
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
        if (ctype == "phone") {
            var cn = document.getElementById("txtPhone_BBPP");
            var err1 = document.getElementById("Errphone_BBPP");
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                cn.style.border = "";
                err1.style.display = "none";
            }
        }
//        if (ctype == "capcode") {
//            var cn = document.getElementById("txtCaptchCode_BBPP");
//            var err1 = document.getElementById("errCaptchCode_BBPP");
//            var err2 = document.getElementById("errCaptchInvalid_BBPP");
//            var err3 = document.getElementById("errCaptchCode1_BBPP");
//            if (cn != null && cn.value == "") {

//                cn.style.border = "1px solid #FF0000";
//                err1.style.display = "block";
//            }
//            else {
//                cn.style.border = "";
//                err1.style.display = "none";


//                if (cn.value != err3.innerHTML) {
//                    err2.style.display = "block";
//                } else {
//                    err2.style.display = "none";
//                }

//            }

//        }
        if (ctype == "notes") {
            var cn = document.getElementById("txtnotesadditionalinfo");
            var err1 = document.getElementById("errnotes");
            if (cn != null && cn.value == "") {

                //cn.style.border = "1px solid #FF0000";
                // err1.style.display = "block";
            }
            else {
                cn.style.border = "";
                err1.style.display = "none";
            }
        }

    }
    function textCounter_BulkBuyPP(field, countfield, maxlimit) {
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
    function MailReset() {
        document.getElementById("txtFullname").value = "";
        document.getElementById("txtEmailAdd").value = "";
        document.getElementById("txtPhone").value = "";
        document.getElementById("txtQuestionx").value = "";
       // document.getElementById("txtCaptchCode").value = "";
        document.getElementById("Errfullname").style.display = "none";
        document.getElementById("erremailadd").style.display = "none";
        document.getElementById("Errphone").style.display = "none";
        document.getElementById("errquestion").style.display = "none";
//        document.getElementById("errCaptchCode").style.display = "none";
//        document.getElementById("errCaptchCode1").style.display = "none";
//        document.getElementById("errCaptchInvalid").style.display = "none";
        document.getElementById("errvalidmail").style.display = "none";
        document.getElementById("txtFullname").style.border = "";
        document.getElementById("txtEmailAdd").style.border = "";
        document.getElementById("txtPhone").style.border = "";
        document.getElementById("txtQuestionx").style.border = "";
        //document.getElementById("txtCaptchCode").style.border = "";
    }
    function MailReset_BulkBuyPP() {
        // document.getElementById("txtproductcode").value = "";
        document.getElementById("txtFullname_BBPP").value = "";
        document.getElementById("txtQTY").value = "";
        document.getElementById("txtEmail").value = "";
        document.getElementById("txtdeliverytime").value = "";
        document.getElementById("txtPhone_BBPP").value = "";
        document.getElementById("txttargetprice").value = "";
        document.getElementById("txtnotesadditionalinfo").value = "";
      //  document.getElementById("txtCaptchCode_BBPP").value = "";
        document.getElementById("Errfullname_BBPP").style.display = "none";
        document.getElementById("ErrQTY").style.display = "none";
        document.getElementById("erremailadd_BBPP").style.display = "none";
        document.getElementById("Errdeliverytime").style.display = "none";
        document.getElementById("Errphone_BBPP").style.display = "none";
        document.getElementById("errnotes").style.display = "none";
//        document.getElementById("errvalidmail_BBPP").style.display = "none";
//        document.getElementById("errCaptchInvalid_BBPP").style.display = "none";
//        document.getElementById("errCaptchCode_BBPP").style.display = "none";
//        document.getElementById("errCaptchCode1_BBPP").style.display = "none";
        document.getElementById("Errprocode_BBPP").style.display = "none";
        document.getElementById("txtFullname_BBPP").style.border = "";
        document.getElementById("txtQTY").style.border = "";
        document.getElementById("txtEmail").style.border = "";
        document.getElementById("txtdeliverytime").style.border = "";
        document.getElementById("txtPhone_BBPP").style.border = "";
        document.getElementById("txtnotesadditionalinfo").style.border = "";
      //  document.getElementById("txtCaptchCode_BBPP").style.border = "";
        document.getElementById("ddlprodcode").style.border = "";
    }

    function OnmailSuccess_BBPP(result) {

        var dt;
        if (result.d != null && result.d != "-1") {

            var bbpp = document.getElementById("BulkBuyPP");
            var bbppsub = document.getElementById("BulkBuyPPSubmit");
            bbpp.style.display = "none";
            bbppsub.style.display = "block";
        }
        else {
            alert("Unable to send your message. Please contact The Wes at Info@wes.com");
        }

    }
    function OnmailFailure_BBPP(result) {
        alert("Unable to send your message. Please contact The Wes at Info@wes.com");
    }

    function getprodcodevalue() {
        var e = document.getElementById("ddlprodcode");
        var strURL = e.options[e.selectedIndex].text;
        var strURL1 = e.options[e.selectedIndex].value;
        // alert(strURL);
    }

    function ValidateCaptcha_BBPP() {
        var rtn = false;
        var p = document.getElementById("txtCaptchCode_BBPP");
        $.ajax({
            type: "POST",
            url: "/mfl.aspx/ValidateCaptcha_BBPP",
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
    function OnCaptchaSuccess_BBPP(result) {
        if (result.d == -2) {
            captchavalid = false;
        } else if (result.d == -1) {
            captchavalid = false;
        }

    }
    function OnCaptchaFailure_BBPP(result) {

        captchavalid = false;

    }

</script>

<script language="javascript" type="text/javascript">
    function validateform_DU() {
        var captcha_response = grecaptcha.getResponse(recaptcha3);
        if (captcha_response.length == 0) {
            return false;
        }
        else {
            return true;
        }
    }


    function MailSend_DU() {

        var valid_g_captcha = validateform_DU();

        if (valid_g_captcha == false) {
            alert("Invalid Captcha");
            return;
        }
        var fullname_du = document.getElementById("txtFullname_DU");
        var email_du = document.getElementById("txtEmail_DU");
        var phone_du = document.getElementById("txtPhone_DU");
        var notes_du = document.getElementById("txtdownloadre");
        var fname_du = document.getElementById("familyName");
//        var capcode_du = document.getElementById("txtCaptchCode_DU");
//        var caperr_du = document.getElementById("errCaptchCode1_DU");
        var valid = true;
        Controlvalidate_DU("fullname_du");
        Controlvalidate_DU("email_du");
        Controlvalidate_DU("phone_du");
        Controlvalidate_DU("notes_du");
       // Controlvalidate_DU("capcode_du");

        if (fullname_du == null || fullname_du.value.trim() == "") {
            valid = false;
            // alert("enter Full Name")
            Controlvalidate_DU("fullname_du");
            fullname_du.focus();
            return;
        }

        if (email_du == null || email_du.value.trim() == "") {
            valid = false;
            //  alert("enter Email id")
            email_du.focus();
            return;
        }

        var vaildemail = checkEmail(email_du.value.trim());
        if (vaildemail == false) {
            valid = false;
            email_du.focus();
            return;
        }
        if (phone_du == null || phone_du.value.trim() == "") {
            valid = false;
            phone_du.focus();
            return;
        }
//        if (capcode_du.value != caperr_du.innerHTML) {
//            valid = false;
//            // err2.style.display = "block";
//            capcode_du.focus();
//            return;
//        }
        if (notes_du == null || notes_du.value.trim() == "") {
            valid = false;
            notes_du.focus();
            return;
        }

        var s = fullname_du.value;
        fullname_du.value = s.replace(/'/g, "`");
        s = notes_du.value;
        notes_du.value = s.replace(/'/g, "`");


        if (valid == true) {
            $.ajax({
                type: "POST",
                url: "/mfl.aspx/DownloadUpdate",
                data: "{'fullname':'" + fullname_du.value.trim() + "','fromid':'" + email_du.value.trim() + "','phone':'" + phone_du.value.trim() + "','downloadrequire':'" + notes_du.value.trim() + "','familyName':'" + fname_du.innerHTML + "'}",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: OnmailSuccess_DU,
                error: OnmailFailure_DU


            });
        }



    }

    function Controlvalidate_DU(ctype) {
        if (ctype == "fullname_du") {
            var dd = document.getElementById("txtFullname_DU");
            var err1 = document.getElementById("Errfullname_DU");
            if (dd != null && dd.value == 0) {

                dd.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                dd.style.border = "";
                err1.style.display = "none";

            }
        }

        if (ctype == "email_du") {
            var cno = document.getElementById("txtEmail_DU");
            var err1 = document.getElementById("erremailadd_DU");
            var err2 = document.getElementById("errvalidmail_DU");
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
        if (ctype == "phone_du") {
            var cn = document.getElementById("txtPhone_DU");
            var err1 = document.getElementById("Errphone_DU");
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                cn.style.border = "";
                err1.style.display = "none";
            }
        }


//        if (ctype == "capcode_du") {
//            var cn = document.getElementById("txtCaptchCode_DU");
//            var err1 = document.getElementById("errCaptchCode_DU");
//            var err2 = document.getElementById("errCaptchInvalid_DU");
//            var err3 = document.getElementById("errCaptchCode1_DU");
//            if (cn != null && cn.value == "") {

//                cn.style.border = "1px solid #FF0000";
//                err1.style.display = "block";
//            }
//            else {
//                cn.style.border = "";
//                err1.style.display = "none";


//                if (cn.value != err3.innerHTML) {
//                    err2.style.display = "block";
//                } else {
//                    err2.style.display = "none";
//                }

//            }

//        }
        if (ctype == "notes_du") {
            var cn = document.getElementById("txtdownloadre");
            var err1 = document.getElementById("errdownloadre");
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                cn.style.border = "";
                err1.style.display = "none";
            }
        }

    }
    function textCounter_DU(field, countfield, maxlimit) {
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

    function MailReset_DU() {
        document.getElementById("txtFullname_DU").value = "";
        document.getElementById("txtEmail_DU").value = "";
        document.getElementById("txtPhone_DU").value = "";
        document.getElementById("txtdownloadre").value = "";
      //  document.getElementById("txtCaptchCode_DU").value = "";
        document.getElementById("Errfullname_DU").style.display = "none";
        document.getElementById("erremailadd_DU").style.display = "none";
        document.getElementById("Errphone_DU").style.display = "none";
        document.getElementById("errdownloadre").style.display = "none";
        document.getElementById("errvalidmail_DU").style.display = "none";
     //   document.getElementById("errCaptchInvalid_DU").style.display = "none";
       // document.getElementById("errCaptchCode_DU").style.display = "none";
       // document.getElementById("errCaptchCode1_DU").style.display = "none";
        document.getElementById("txtFullname_DU").style.border = "";
        document.getElementById("txtEmail_DU").style.border = "";
        document.getElementById("txtPhone_DU").style.border = "";
        document.getElementById("txtdownloadre").style.border = "";
       // document.getElementById("txtCaptchCode_DU").style.border = "";
    }

    function OnmailSuccess_DU(result) {

        var dt;
        if (result.d != null && result.d != "-1") {

            var bbpp = document.getElementById("DownloadUpdate");
            var bbppsub = document.getElementById("DUSubmit");
            bbpp.style.display = "none";
            bbppsub.style.display = "block";
        }
        else {
            alert("Unable to send your message. Please contact The Wes at Info@wes.com");
        }

    }
    function OnmailFailure_DU(result) {
        alert("Unable to send your message. Please contact The Wes at Info@wes.com");
    }


    function ValidateCaptcha_DU() {
        var rtn = false;
        var p = document.getElementById("txtCaptchCode_DU");
        $.ajax({
            type: "POST",
            url: "/mfl.aspx/ValidateCaptcha_DU",
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
    function OnCaptchaSuccess_DU(result) {
        if (result.d == -2) {
            captchavalid = false;
        } else if (result.d == -1) {
            captchavalid = false;
        }

    }
    function OnCaptchaFailure_DU(result) {

        captchavalid = false;

    }

</script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
      
       <div class=" col-lg-12 breadcrambs"> <%=Bread_Crumbs_MS(true) %> </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/darktooltip.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />
<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/micrositecss/thickboxAddtocart_MS.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />

<%--<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/thickboxAddtocart.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />--%>


  <input type="hidden" id="hfclickedattr" name="hfclickedattr" value="" />

        
      
        <uc1:familyMS ID="familyMS1" runat="server" />



   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">





<!-- zoom -->
 <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>zoom/js/killercarousel.js"></script>
        <!-- Include KillerCarousel CSS -->
        <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/zoom/css/killercarousel.css" type="text/css" rel="stylesheet" />
        
        <!-- Include Cloud Zoom CSS -->
        <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/zoom/js/cloudzoom.css" type="text/css" rel="stylesheet" />
        <!-- Include Cloud Zoom JavaScript -->
        <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/zoom/js/cloudzoom.js"></script>
        
        <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/zoom/js/thumbelina.js"></script>
        
        <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/zoom/css/jetzoom.css?v=3" type="text/css" rel="stylesheet" />
        <!-- Include Jet Zoom JavaScript -->
        <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/zoom/js/jetzoom.js"></script>
        <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/zoom/js/starzoom.js"></script>
        <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/zoom/css/starzoom.css?v=3" type="text/css" rel="stylesheet" />

        
        <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/zoom/js/jquery.fancybox.js"></script>
        <link rel="stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/zoom/css/jquery.fancybox.css" type="text/css" media="screen" />
        
                <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/zoom/css/thumbelina.css" type="text/css" rel="stylesheet" />


                

<!-- Optional theme -->

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
        


<script  language="javascript" type="text/javascript">
    $(".amenu .category").mouseover(function () {
        $(".amenu .sub-menu").hide();
        $($(this).children(".sub-menu")).show();
    });


    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
</script>

<script language="javascript" type="text/javascript">
    $('.def-html').darkTooltip({
        opacity: 1,
        gravity: 'south'
    });
</script>

<script language="javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/MicroSitejs/thegoods.js" type="text/javascript" />


 
</asp:Content>
