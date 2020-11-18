
//$(function () {




//$(document).ready(function () {
//// MENU _PRADEEP								
//	$(".open").pageslide();
//	


//// MENU _PRADEEP
//	$('.sf-menu').superfish();
//// TABS _PRADEEP
//		$('.accordion-tab').easyResponsiveTabs({
//            type: 'accordion', //Types: default, vertical, accordion           
//            width: 'auto', //auto or any width like 600px
//            fit: true,   // 100% fit in a container
//            closed: 'accordion', // Start closed if in accordion view
//        });
//        $('#horizontalTab').easyResponsiveTabs({
//            type: 'default', //Types: default, vertical, accordion           
//            width: 'auto', //auto or any width like 600px
//            fit: true,   // 100% fit in a container
//            closed: 'accordion', // Start closed if in accordion view
//        });

//        $('#verticalTab').easyResponsiveTabs({
//            type: 'vertical',
//            width: 'auto',
//            fit: true
//        });
//	});


//// SLIDER _PRADEEP
//		$('.flexslider').flexslider({
//		animation: "fade",
//		controlsContainer: ".flex-container",
//		directionNav:false,
//		controlNav: true,
//		slideshow: true,
//		slideshowSpeed: 6000,
//		pauseOnHover: true
//		});
//// FANCY BOX _PRADEEP
//		$('.fancybox').fancybox();
//// To TOP _PRADEEP
//		$().UItoTop({ easingType: 'easeOutQuart' });
//});

  function ttrim(stringToTrim) {
	   return stringToTrim.replace(" ","");
    }

 function urlredirectKMS(e) {

        var keynum
        var keychar
        var numcheck
        if (window.event) {
            keynum = e.keyCode
        }
        else if (e.which) {
            keynum = e.which
        }
        keychar = String.fromCharCode(keynum)
        /*if (keychar == "@" || keychar == "!" || keychar == "~" || keychar == "`"  || keychar == "%" || keychar == "^"  || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" || keychar == ";" || keychar == ":" || keychar == "," || keychar == "?" || keychar == "/" || keychar == "[" || keychar == "{" || keychar == "]" || keychar == "}" || keychar == "." || keychar == "*" || keychar == "\#" || keychar == "\$" || keychar == "\%" )
        {
        e.keyCode = '';
        return false;
        }
        else if (e.which == 64 || e.which == 126 || e.which == 33 || e.which == 64 || e.which == 35  || e.which == 36 || e.which == 37 || e.which == 94 || e.which == 38 || e.which == 42 || e.which == 40 || e.which == 39 || e.which == 44 || e.which == 45 || e.which == 46 || e.which == 47 || e.which == 58 || e.which == 61 || e.which == 91 || e.which == 92 || e.which == 93 || e.which == 96 || e.which == 95 || e.which == 43 || e.which == 123 || e.which == 124 || e.which == 125 || e.which == 59 || e.which == 34 || e.which == 60 || e.which == 62 || e.which == 63 || e.which == 126)
        {

        return false;
        }*/

        var ddlattrvalue = document.getElementById('srcfield').value.replace("\"", "`~");
         var supName = document.getElementById('SupplierName').value;
        if (ddlattrvalue != "") {
            if (ddlattrvalue != "Search") {

                if (e.keyCode == 13) {
                    // window.document.location="ps.aspx?&srctext=" + ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26").replace(/ /g,"%20").replace("+","%2B").replace(/\"/g, "%22");

                    //  window.location.href = "ps.aspx?" + ddlattrvalue.replace(/#/, "%23").replace(/ /g, "_").replace("+", "%2B").replace(/\"/g, "%22").replace("%20", "_").replace("  ", "_").replace("__", "_"); 
                    $.ajax({
                        type: "POST",
                        url: "/GblWebMethods.aspx/stringreplaceMS",                        
                        data: "{'strvalue':'" + ddlattrvalue + "','suppName':'" + supName + "'}",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d != "") {
                                window.location.href = "/" + data.d + "/mps/";
                            }
                        },
                        error: function (xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            alert(err.Message);
                        }



                    });


                    return false;
                }
            }
        }
    }

     function urlredirectMS() {
    try{
        var ddlattrvalue = document.getElementById('srcfield').value.replace("\"", "`~");
        var supName = document.getElementById('SupplierName').value;
//        .replace("%20", "_").replace(" ", "_").replace(/ /, '_').replace("/", "./.").replace("__", "_").replace("  ", "_");
       // alert(ddlattrvalue);

        if (ddlattrvalue != "") {
            if (ddlattrvalue != "Search..") {
                if (ttrim(ddlattrvalue) != "") {




                 
                    $.ajax({
                        type: "POST",
                        url: "/GblWebMethods.aspx/stringreplaceMS",                        
                        data: "{'strvalue':'" + ddlattrvalue + "','suppName':'" + supName + "'}",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d != "") {
                                
                                window.location.href = "/" + data.d + "/mps/";

                            }
                        },
                        error: function (xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            alert(err.Message);
                        }



                    });

                   

                   
                }
            }
        }
    }
    catch (e) {
        alert(e.ToString());
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


    function MailSend_mscontactus()
    {
        var valid_g_captcha = validateform();
        if (valid_g_captcha == false) {
            alert("Invalid Captcha");
            return false;
        }
           var fullname = document.getElementById("mstxtfullname");
           var email = document.getElementById("mstxtemail");
           var comments = document.getElementById("mstxtenquirycomments");
           var msphone = document.getElementById("mstxtphone");
          // var ccode =  document.getElementById("txtcapcode");
           //var caperr = document.getElementById("errcapcode1");
         //  var caperr = document.getElementById("ctl00_MainContent_capchacode");
           var valid = true;
           Controlvalidate_mscontactus("fullname");
            Controlvalidate_mscontactus("email");
            Controlvalidate_mscontactus("comments");
           //   Controlvalidate_mscontactus("ccode");
             
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

        
           //if (ccode.value != caperr.innerHTML) {
           //    valid = false;
           //    // err2.style.display = "block";
           //    ccode.focus();
           //    return;
           //}

           var s = fullname.value;
        
           fullname.value = s.replace(/'/g, "`");
           s = comments.value;
           comments.value = s.replace(/'/g, "`");
       
          
            if (valid == true) {
               $.ajax({
                   type: "POST",
                   url: "/mcontactus.aspx/SendmscontactusMail",
                   data: "{'fullname':'" + fullname.value.trim() + "','fromid':'" + email.value.trim() + "','phone':'" + msphone.value.trim() +"','notesandaddtionalinfo':'" + comments.value.trim() + "'}",
                   contentType: "application/json;charset=utf-8",
                   dataType: "json",
                   success: OnmailSuccess_mscontactus,
                   error: OnmailFailure_mscontactus


               });
           }
    }

           function OnmailSuccess_mscontactus(result) {

           var dt;
           if (result.d != null && result.d != "-1") {

              // var bbpp = document.getElementById("mscontactsuccessmessage");
              // var bbppsub = document.getElementById("BulkBuyPPSubmit");
              // bbpp.style.display = "none";
               //bbpp.style.display = "block";
               window.location.href = "mConfirmMessage.aspx?Result=MESSAGESENT"
           }
           else {
               alert("Unable to send your message. Please contact The Wes at Info@wes.com");
           }

       }
       function OnmailFailure_mscontactus(result) {
           alert("Unable to send your message. Please contact The Wes at Info@wes.com");
       }
        function Controlvalidate_mscontactus(ctype) {
           if (ctype == "fullname") {
               var dd = document.getElementById("mstxtfullname");
               var err1 = document.getElementById("errfullname");
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
                if (ctype == "ccode") {
               var cn = document.getElementById("txtcapcode");
               var err1 = document.getElementById("errcapcode");
               var err2 = document.getElementById("errcapcodeinvalid");
               // var err3 = document.getElementById("errcapcode1");
               var err3 = document.getElementById("ctl00_MainContent_capchacode");
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
           if (ctype == "email") {
               var cno = document.getElementById("mstxtemail");
               var err1 = document.getElementById("erremailaddress");
               var err2 = document.getElementById("errvalidemail");
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

           if (ctype == "comments") {
               var cn = document.getElementById("mstxtenquirycomments");
               var err1 = document.getElementById("errenquiry");
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


   