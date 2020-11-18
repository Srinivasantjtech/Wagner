


function validateform_BulkBuyPP() {
    // var captcha_response = grecaptcha.getResponse(recaptcha2);
    var captcha_response = grecaptcha.getResponse();
    if (captcha_response.length == 0) {
        return false;
    }
    else {
        return true;
    }
}





function MailSend_BulkBuyPP() {


   // var valid_g_captcha = validateform_BulkBuyPP();

   // if (valid_g_captcha == false) {
    //    alert("Invalid Captcha");
    //    return;
    // }

    var valid_g_captcha2 = document.getElementById("valrecaptcha2").value;
   // alert(valid_g_captcha2);
    if (valid_g_captcha2 == "false") {
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
         //  var caperr = $('#errCaptchCode1_BBPP').attr('innerHTML');
           var valid = true;
           Controlvalidate_BulkBuyPP("fullname"); 
           Controlvalidate_BulkBuyPP("email");
           Controlvalidate_BulkBuyPP("phone");
          // Controlvalidate_BulkBuyPP("notes");
           Controlvalidate_BulkBuyPP("deltime");
           Controlvalidate_BulkBuyPP("qty");
          // Controlvalidate_BulkBuyPP("capcode");
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
     
//           if (capcode.value != caperr) {
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
                   url: "/pd.aspx/SendBulkBuyProjectPricing",
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
                   // err1.style.display = "block";
                   document.getElementById("Errfullname_BBPP").classList.remove('dpynone');
                   document.getElementById("Errfullname_BBPP").className += " dpyblk";
               }
               else {
                   dd.style.border = "";
                   // err1.style.display = "none";
                   document.getElementById("Errfullname_BBPP").classList.remove('dpyblk');
                   document.getElementById("Errfullname_BBPP").className += " dpynone";

               }
           }
           if (ctype == "deltime") {
               var dd = document.getElementById("txtdeliverytime");
               var err1 = document.getElementById("Errdeliverytime");
               dd.setAttribute("value", dd.value);
               if (dd != null && dd.value == 0) {

                   dd.style.border = "1px solid #FF0000";
                   // err1.style.display = "block";
                   document.getElementById("Errdeliverytime").classList.remove('dpynone');
                   document.getElementById("Errdeliverytime").className += " dpyblk";
               }
               else {
                   dd.style.border = "";
                   // err1.style.display = "none";
                   document.getElementById("Errdeliverytime").classList.remove('dpyblk');
                   document.getElementById("Errdeliverytime").className += " dpynone";

               }
           }

           if (ctype == "qty") {
               var dd = document.getElementById("txtQTY");
               var err1 = document.getElementById("ErrQTY");
               dd.setAttribute("value", dd.value);
               if (dd != null && dd.value == 0) {

                   dd.style.border = "1px solid #FF0000";
                   // err1.style.display = "block";
                   document.getElementById("ErrQTY").classList.remove('dpynone');
                   document.getElementById("ErrQTY").className += " dpyblk";
               }
               else {
                   dd.style.border = "";
                   // err1.style.display = "none";
                   document.getElementById("ErrQTY").classList.remove('dpyblk');
                   document.getElementById("ErrQTY").className += " dpynone";

               }
           }
//           if (ctype == "capcode") {
//               var cn = document.getElementById("txtCaptchCode_BBPP");
//               var err1 = document.getElementById("errCaptchCode_BBPP");
//               var err2 = document.getElementById("errCaptchInvalid_BBPP");
//               // var err3 = document.getElementById("errCaptchCode1_BBPP");
//               var err3 = $('#errCaptchCode1_BBPP').attr('innerHTML');
//               cn.setAttribute("value", cn.value);
//              // alert(cn.value);
//               if (cn != null && cn.value == "") {

//                   cn.style.border = "1px solid #FF0000";
//                   err1.style.display = "block";
//               }
//               else {
//                   cn.style.border = "";
//                   err1.style.display = "none";


//                   if (cn.value != err3) {
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
                   //  err1.style.display = "block";
                   document.getElementById("erremailadd_BBPP").classList.remove('dpynone');
                   document.getElementById("erremailadd_BBPP").className += " dpyblk";
               }
               else {

                   cno.style.border = "";
                   // err1.style.display = "none";
                   document.getElementById("erremailadd_BBPP").classList.remove('dpyblk');
                   document.getElementById("erremailadd_BBPP").className += " dpynone";

                   var vaildemail = checkEmail(cno.value.trim());
                   if (vaildemail == false) {
                       // err2.style.display = "block";
                       document.getElementById("errvalidmail_BBPP").classList.remove('dpynone');
                       document.getElementById("errvalidmail_BBPP").className += " dpyblk";
                   } else {
                       // err2.style.display = "none";
                       document.getElementById("errvalidmail_BBPP").classList.remove('dpyblk');
                       document.getElementById("errvalidmail_BBPP").className += " dpynone";
                   }
               }

           }
           if (ctype == "phone") {
               var cn = document.getElementById("txtPhone_BBPP");
               var err1 = document.getElementById("Errphone_BBPP");
               cn.setAttribute("value", cn.value);
               if (cn != null && cn.value == "") {

                   cn.style.border = "1px solid #FF0000";
                   // err1.style.display = "block";
                   document.getElementById("Errphone_BBPP").classList.remove('dpynone');
                   document.getElementById("Errphone_BBPP").className += " dpyblk";
               }
               else {
                   cn.style.border = "";
                   // err1.style.display = "none";
                   document.getElementById("Errphone_BBPP").classList.remove('dpyblk');
                   document.getElementById("Errphone_BBPP").className += " dpynone";
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
                   // err1.style.display = "none";
                   document.getElementById("errnotes").classList.remove('dpyblk');
                   document.getElementById("errnotes").className += " dpynone";
               }
           }

       }

//       function textCounter_BulkBuyPP(field, countfield, maxlimit) {
//           
//           var tf = document.getElementById(field);
//           var cf = document.getElementById(countfield);
//           tf.setAttribute("value", tf.value);
//           if (tf.value.length > maxlimit) {
//               tf.value = tf.value.substring(0, maxlimit);
//               alert('Enquiry/Comments maximum allowed 600 characters.');
//               return false;
//           }
//           else {
//               cf.value = maxlimit - tf.value.length;
//           }
//       }


       function textCounter_BulkBuyPP(field, countfield, maxlimit) {
           if (field.value.length > maxlimit) {
               field.value = field.value.substring(0, maxlimit);
               alert('Notes / Addtional Info maximum allowed 600 characters.');
               return false;
           }
           else {
               countfield.value = maxlimit - field.value.length;
           }
       }

       function MailReset_BulkBuyPP() {
       
         
          

           document.getElementById("txtFullname_BBPP").value = "";
           document.getElementById("txtFullname_BBPP").defaultValue = "";
           document.getElementById("txtQTY").value = "";
           document.getElementById("txtQTY").defaultValue = "";
           document.getElementById("txtEmail").value = "";
           document.getElementById("txtEmail").defaultValue = "";
           document.getElementById("txtdeliverytime").value = "";
           document.getElementById("txtdeliverytime").defaultValue = "";
           document.getElementById("txtPhone_BBPP").value = "";
           document.getElementById("txtPhone_BBPP").defaultValue = "";
           document.getElementById("txttargetprice").value = "";
           document.getElementById("txttargetprice").defaultValue = "";
           document.getElementById("txtnotesadditionalinfo").value = "";
           document.getElementById("Errfullname_BBPP").style.display = "none";
           document.getElementById("ErrQTY").style.display = "none";
           document.getElementById("erremailadd_BBPP").style.display = "none";
           document.getElementById("Errdeliverytime").style.display = "none";
           document.getElementById("Errphone_BBPP").style.display = "none";
           document.getElementById("errnotes").style.display = "none";
           document.getElementById("errvalidmail_BBPP").style.display = "none";

           document.getElementById("txtFullname_BBPP").style.border = "";
           document.getElementById("txtQTY").style.border = "";
           document.getElementById("txtEmail").style.border = "";
           document.getElementById("txtdeliverytime").style.border = "";
           document.getElementById("txtPhone_BBPP").style.border = "";
           document.getElementById("txtnotesadditionalinfo").style.border = "";
       

       }

       function OnmailSuccess_BBPP(result) {

           var dt;
           if (result.d != null && result.d != "-1") {

               var bbpp = document.getElementById("bulkbuy");
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

       function ValidateCaptcha_BBPP() {
           var rtn = false;
           var p = document.getElementById("txtCaptchCode_BBPP");
           $.ajax({
               type: "POST",
               url: "/pd.aspx/ValidateCaptcha_BBPP",
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

          // var valid_g_captcha = validateform_DU();

          // if (valid_g_captcha == false) {
          //     alert("Invalid Captcha");
          //     return;
           //  }

           var valid_g_captcha3 = document.getElementById("valrecaptcha3").value;
           if (valid_g_captcha3 == "false") {
               alert("Invalid Captcha");
               return;
           }


           var fullname_du = document.getElementById("txtFullname_DU");
           var email_du = document.getElementById("txtEmail_DU");
           var phone_du = document.getElementById("txtPhone_DU");
           var notes_du = document.getElementById("txtdownloadre");
           var procode_du = document.getElementById("ProductCode");
       
           var valid = true;
           Controlvalidate_DU("fullname_du");
           Controlvalidate_DU("email_du");
           Controlvalidate_DU("phone_du");
           Controlvalidate_DU("notes_du");
          // Controlvalidate_DU("capcode_du");
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
//           if (capcode_du.value != caperr_du) {
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
                   url: "/pd.aspx/DownloadUpdate",
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
                   // err1.style.display = "block";
                   document.getElementById("Errfullname_DU").classList.remove('dpynone');
                   document.getElementById("Errfullname_DU").className += " dpyblk";
               }
               else {
                   dd.style.border = "";
                   // err1.style.display = "none";
                   document.getElementById("Errfullname_DU").classList.remove('dpyblk');
                   document.getElementById("Errfullname_DU").className += " dpynone";

               }
           }

           if (ctype == "email_du") {
               var cno = document.getElementById("txtEmail_DU");
               var err1 = document.getElementById("erremailadd_DU");
               var err2 = document.getElementById("errvalidmail_DU");
               cno.setAttribute("value", cno.value);
               if (cno != null && cno.value == "") {
                   cno.style.border = "1px solid #FF0000";
                   //err1.style.display = "block";
                   document.getElementById("erremailadd_DU").classList.remove('dpynone');
                   document.getElementById("erremailadd_DU").className += " dpyblk";
               }
               else {

                   cno.style.border = "";
                   // err1.style.display = "none";
                   document.getElementById("erremailadd_DU").classList.remove('dpyblk');
                   document.getElementById("erremailadd_DU").className += " dpynone";

                   var vaildemail = checkEmail(cno.value.trim());
                   if (vaildemail == false) {
                       //err2.style.display = "block";
                       document.getElementById("errvalidmail_DU").classList.remove('dpynone');
                       document.getElementById("errvalidmail_DU").className += " dpyblk";
                   } else {
                       // err2.style.display = "none";
                       document.getElementById("errvalidmail_DU").classList.remove('dpyblk');
                       document.getElementById("errvalidmail_DU").className += " dpynone";
                   }
               }

           }
//           if (ctype == "capcode_du") {
//               var cn = document.getElementById("txtCaptchCode_DU");
//               var err1 = document.getElementById("errCaptchCode_DU");
//               var err2 = document.getElementById("errCaptchInvalid_DU");
//              // var err3 = document.getElementById("errCaptchCode1_DU");
//               var err3 = $('#errCaptchCode1_DU').attr('innerHTML');
//               cn.setAttribute("value", cn.value);
//               if (cn != null && cn.value == "") {

//                   cn.style.border = "1px solid #FF0000";
//                   err1.style.display = "block";
//               }
//               else {
//                   cn.style.border = "";
//                   err1.style.display = "none";


//                   if (cn.value != err3 && cn.value != null) {
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
                   // err1.style.display = "block";
                   document.getElementById("Errphone_DU").classList.remove('dpynone');
                   document.getElementById("Errphone_DU").className += " dpyblk";
               }
               else {
                   cn.style.border = "";
                   // err1.style.display = "none";
                   document.getElementById("Errphone_DU").classList.remove('dpyblk');
                   document.getElementById("Errphone_DU").className += " dpynone";
               }
           }
           if (ctype == "notes_du") {
               var cn = document.getElementById("txtdownloadre");
               var err1 = document.getElementById("errdownloadre");
               cn.setAttribute("value", cn.value);
               if (cn != null && cn.value == "") {

                   cn.style.border = "1px solid #FF0000";
                   // err1.style.display = "block";
                   document.getElementById("errdownloadre").classList.remove('dpynone');
                   document.getElementById("errdownloadre").className += " dpyblk";
               }
               else {
                   cn.style.border = "";
                   // err1.style.display = "none";
                   document.getElementById("errdownloadre").classList.remove('dpyblk');
                   document.getElementById("errdownloadre").className += " dpynone";
               }
           }

       }
//       function textCounter_DU(field, countfield, maxlimit) {
//           var tf = document.getElementById(field);
//           var cf = document.getElementById(countfield);
//           tf.setAttribute("value", tf.value);
//           if (tf.value.length > maxlimit) {
//               tf.value = tf.value.substring(0, maxlimit);
//               alert('Enquiry/Comments maximum allowed 600 characters.');
//               return false;
//           }
//           else {
//               cf.value = maxlimit - tf.value.length;
//           }
//       }


       function textCounter_DU(field, countfield, maxlimit) {
           if (field.value.length > maxlimit) {
               field.value = field.value.substring(0, maxlimit);
               alert('Notes / Addtional Info maximum allowed 600 characters.');
               return false;
           }
           else {
               countfield.value = maxlimit - field.value.length;
           }
       }


       function MailReset_DU() {
           document.getElementById("txtFullname_DU").value = "";
           document.getElementById("txtFullname_DU").defaultValue = "";
           document.getElementById("txtEmail_DU").value = "";
           document.getElementById("txtEmail_DU").defaultValue = "";
           document.getElementById("txtPhone_DU").value = "";
           document.getElementById("txtPhone_DU").defaultValue = "";
           document.getElementById("txtdownloadre").value = "";
 
           document.getElementById("Errfullname_DU").style.display = "none";
           document.getElementById("erremailadd_DU").style.display = "none";
           document.getElementById("Errphone_DU").style.display = "none";
           document.getElementById("errdownloadre").style.display = "none";
           document.getElementById("errvalidmail_DU").style.display = "none";

           document.getElementById("txtFullname_DU").style.border = "";
           document.getElementById("txtEmail_DU").style.border = "";
           document.getElementById("txtPhone_DU").style.border = "";
           document.getElementById("txtdownloadre").style.border = "";
  
       }

       function OnmailSuccess_DU(result) {

           var dt;
           if (result.d != null && result.d != "-1") {

               var bbpp = document.getElementById("downloads");
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




       function DownloadLoad_ST() {
           var productID = document.getElementById("TBT_PRODUCT_ID").value;
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
       function DownloadFailure(result) {
           alert("Unable to send your message. Please contact The Wes at sales@wagneronline.com.au");
       }

       var verifyCallbackDownload = function (response) {
           if (response.length !== 0) {
               document.getElementById("valrecaptcha3").value = "true";
           }
       };

       var expCallbackDownload = function () {
           document.getElementById("valrecaptcha3").value = "false";
       };


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

