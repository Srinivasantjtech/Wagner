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
           var procode = document.getElementById("ProductCode");
          // var capcode = document.getElementById("txtCaptchCode_BBPP"); 
          // var caperr = document.getElementById("errCaptchCode1_BBPP");
           var valid = true;
           Controlvalidate_BulkBuyPP("fullname"); 
           Controlvalidate_BulkBuyPP("email");
           Controlvalidate_BulkBuyPP("phone");
          // Controlvalidate_BulkBuyPP("notes");
           Controlvalidate_BulkBuyPP("deltime");
           Controlvalidate_BulkBuyPP("qty");
         //  Controlvalidate_BulkBuyPP("capcode");
           if (fullname == null || fullname.value.trim() == "") {
               valid = false;
               // alert("enter Full Name")
               // Controlvalidate_BulkBuyPP("fullname");
               fullname.focus();
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

//           if (notes == null || notes.value.trim() == "") {
//               valid = false;
//               notes.focus();
//               return;
//           }
           if (qty == null || qty.value.trim() == "") {
               valid = false;
               notes.focus();
               return;
           }

//           if (capcode.value != caperr.innerHTML) {
//               valid = false;
//               // err2.style.display = "block";
//               capcode.focus();
//               return;
//           }




           var s = fullname.value;
           fullname.value = s.replace(/'/g, "`");
           s = notes.value;
           notes.value = s.replace(/'/g, "`");


           if (valid == true) {
               $.ajax({
                   type: "POST",
                   url: "/mpd.aspx/SendBulkBuyProjectPricing",
                   data: "{'productcode':'" + procode.innerHTML + "','fullname':'" + fullname.value.trim() + "','qty':'" + qty.value.trim() + "','fromid':'" + email.value.trim() + "','deliverytime':'" + deltime.value.trim() + "','phone':'" + phone.value.trim() + "','targetprice':'" + tarprice.value.trim() + "','notesandaddtionalinfo':'" + notes.value.trim() + "'}",
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
           if (ctype == "deltime") {
               var dd = document.getElementById("txtdeliverytime");
               var err1 = document.getElementById("Errdeliverytime");
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

           if (ctype == "qty") {
               var dd = document.getElementById("txtQTY");
               var err1 = document.getElementById("ErrQTY");
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
//           if (ctype == "capcode") {
//               var cn = document.getElementById("txtCaptchCode_BBPP");
//               var err1 = document.getElementById("errCaptchCode_BBPP");
//               var err2 = document.getElementById("errCaptchInvalid_BBPP");
//               var err3 = document.getElementById("errCaptchCode1_BBPP");
//               cn.setAttribute("value", cn.value);
//               if (cn != null && cn.value == "") {

//                   cn.style.border = "1px solid #FF0000";
//                   err1.style.display = "block";
//               }
//               else {
//                   cn.style.border = "";
//                   err1.style.display = "none";


//                   if (cn.value != err3.innerHTML) {
//                       err2.style.display = "block";
//                   } else {
//                       err2.style.display = "none";
//                   }

//               }

//           }
           if (ctype == "email") {
               var cno = document.getElementById("txtEmail");
               var err1 = document.getElementById("erremailadd_BBPP");
               var err2 = document.getElementById("errvalidmail_BBPP");
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
           if (ctype == "phone") {
               var cn = document.getElementById("txtPhone_BBPP");
               var err1 = document.getElementById("Errphone_BBPP");
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
           if (ctype == "notes") {
               var cn = document.getElementById("txtnotesadditionalinfo");
               var err1 = document.getElementById("errnotes");
               cn.setAttribute("value", cn.value);
               if (cn != null && cn.value == "") {

                  // cn.style.border = "1px solid #FF0000";
                   //err1.style.display = "block";
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
               // document.getElementById("errCaptchCode").style.display = "none";
                //document.getElementById("errCaptchCode1").style.display = "none";
               // document.getElementById("errCaptchInvalid").style.display = "none";
                document.getElementById("errvalidmail").style.display = "none";
                document.getElementById("txtFullname").style.border = "";
                document.getElementById("txtEmailAdd").style.border = "";
                document.getElementById("txtPhone").style.border = "";
                document.getElementById("txtQuestionx").style.border = "";
                //document.getElementById("txtCaptchCode").style.border = "";  
       }

       function MailReset_BulkBuyPP() {
           //document.getElementById("txtproductcode").value = "";
           document.getElementById("txtFullname_BBPP").value = "";
           document.getElementById("txtQTY").value = "";
           document.getElementById("txtEmail").value = "";
           document.getElementById("txtdeliverytime").value = "";
           document.getElementById("txtPhone_BBPP").value = "";
           document.getElementById("txttargetprice").value = "";
          // document.getElementById("txtCaptchCode_BBPP").value = "";
           document.getElementById("txtnotesadditionalinfo").value = "";
           document.getElementById("Errfullname_BBPP").style.display = "none";
           document.getElementById("ErrQTY").style.display = "none";
           document.getElementById("erremailadd_BBPP").style.display = "none";
           document.getElementById("Errdeliverytime").style.display = "none";
           document.getElementById("Errphone_BBPP").style.display = "none";
           document.getElementById("errnotes").style.display = "none";
           document.getElementById("errvalidmail_BBPP").style.display = "none";
//           document.getElementById("errCaptchInvalid_BBPP").style.display = "none";
//           document.getElementById("errCaptchCode_BBPP").style.display = "none";
//           document.getElementById("errCaptchCode1_BBPP").style.display = "none";
           document.getElementById("txtFullname_BBPP").style.border = "";
           document.getElementById("txtQTY").style.border = "";
           document.getElementById("txtEmail").style.border = "";
           document.getElementById("txtdeliverytime").style.border = "";
           document.getElementById("txtPhone_BBPP").style.border = "";
           document.getElementById("txtnotesadditionalinfo").style.border = "";
         //  document.getElementById("txtCaptchCode_BBPP").style.border = "";

       }

       function OnmailSuccess_BBPP(result) {

           var dt;
           if (result.d != null && result.d != "-1") {

               var bbpp = document.getElementById("show_hide_BB");
               var bbppsub = document.getElementById("BBSM");
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

       function ValidateCaptcha_BBPP() {
           var rtn = false;
           var p = document.getElementById("txtCaptchCode_BBPP");
           $.ajax({
               type: "POST",
               url: "/mpd.aspx/ValidateCaptcha_BBPP",
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
           var procode_du = document.getElementById("ProductCode");
         //  var capcode_du = document.getElementById("txtCaptchCode_DU");
         //  var caperr_du = document.getElementById("errCaptchCode1_DU");
           var valid = true;
           Controlvalidate_DU("fullname_du");
           Controlvalidate_DU("email_du");
           Controlvalidate_DU("phone_du");
           Controlvalidate_DU("notes_du");
         //  Controlvalidate_DU("capcode_du");
           if (fullname_du == null || fullname_du.value.trim() == "") {
               valid = false;
               // alert("enter Full Name")
               // Controlvalidate_BulkBuyPP("fullname");
               fullname_du.focus();
               return;
           }

           if (email_du == null || email_du.value.trim() == "") {
               valid = false;
               //  alert("enter Email id")
               email_du.focus();
               return;
           }
//           if (capcode_du.value != caperr_du.innerHTML) {
//               valid = false;
//               // err2.style.display = "block";
//               capcode_du.focus();
//               return;
//           }
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
                   url: "/mpd.aspx/DownloadUpdate",
                   data: "{'fullname':'" + fullname_du.value.trim() + "','fromid':'" + email_du.value.trim() + "','phone':'" + phone_du.value.trim() + "','downloadrequire':'" + notes_du.value.trim() + "','productcode':'" + procode_du.innerHTML + "'}",
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

           if (ctype == "email_du") {
               var cno = document.getElementById("txtEmail_DU");
               var err1 = document.getElementById("erremailadd_DU");
               var err2 = document.getElementById("errvalidmail_DU");
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
//           if (ctype == "capcode_du") {
//               var cn = document.getElementById("txtCaptchCode_DU");
//               var err1 = document.getElementById("errCaptchCode_DU");
//               var err2 = document.getElementById("errCaptchInvalid_DU");
//               var err3 = document.getElementById("errCaptchCode1_DU");
//               cn.setAttribute("value", cn.value);
//               if (cn != null && cn.value == "") {

//                   cn.style.border = "1px solid #FF0000";
//                   err1.style.display = "block";
//               }
//               else {
//                   cn.style.border = "";
//                   err1.style.display = "none";


//                   if (cn.value != err3.innerHTML) {
//                       err2.style.display = "block";
//                   } else {
//                       err2.style.display = "none";
//                   }

//               }

//           }
           if (ctype == "phone_du") {
               var cn = document.getElementById("txtPhone_DU");
               var err1 = document.getElementById("Errphone_DU");
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
           if (ctype == "notes_du") {
               var cn = document.getElementById("txtdownloadre");
               var err1 = document.getElementById("errdownloadre");
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
          // document.getElementById("txtCaptchCode_DU").value = "";
           document.getElementById("Errfullname_DU").style.display = "none";
           document.getElementById("erremailadd_DU").style.display = "none";
           document.getElementById("Errphone_DU").style.display = "none";
           document.getElementById("errdownloadre").style.display = "none";
           document.getElementById("errvalidmail_DU").style.display = "none";
//           document.getElementById("errCaptchInvalid_DU").style.display = "none";
//           document.getElementById("errCaptchCode_DU").style.display = "none";
//           document.getElementById("errCaptchCode1_DU").style.display = "none";
           document.getElementById("txtFullname_DU").style.border = "";
           document.getElementById("txtEmail_DU").style.border = "";
           document.getElementById("txtPhone_DU").style.border = "";
           document.getElementById("txtdownloadre").style.border = "";
         //  document.getElementById("txtCaptchCode_DU").style.border = "";
       }

       function OnmailSuccess_DU(result) {

           var dt;
           if (result.d != null && result.d != "-1") {

               var bbpp = document.getElementById("show_hide_DU");
               var bbppsub = document.getElementById("DUSM");
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
               url: "/pd.aspx/ValidateCaptcha_DU",
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