<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_family" Codebehind="family.ascx.cs" %>
<%@ Register Src="searchby.ascx" TagName="searchby" TagPrefix="uc5" %>
  <input type="hidden" value="false" id="valrecaptcha1" />
<input type="hidden" value="false" id="valrecaptcha2" />
<input type="hidden" value="false" id="valrecaptcha3" />
<script language="javascript" type="text/javascript" >
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
    function OnclickTab(Attribute) {
        var elem = document.getElementById(Attribute);
        var defValue = elem.value;

        document.getElementById("hfclickedattr").value = defValue;


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
          //  var cn = document.getElementById("txtCaptchCode");
            var err1 = document.getElementById("errCaptchCode");
            var err2 = document.getElementById("errCaptchInvalid");
            //var err3 = document.getElementById("errCaptchCode1");
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
      //  document.getElementById("txtCaptchCode").value = "";
        document.getElementById("Errfullname").style.display = "none";
        document.getElementById("erremailadd").style.display = "none";
        document.getElementById("Errphone").style.display = "none";
//        document.getElementById("errCaptchCode").style.display = "none";
//        document.getElementById("errCaptchInvalid").style.display = "none";
        document.getElementById("errquestion").style.display = "none";
        document.getElementById("txtFullname").style.border = "";
        document.getElementById("txtEmailAdd").style.border = "";
        document.getElementById("txtPhone").style.border = "";
        document.getElementById("txtQuestionx").style.border = "";
       // document.getElementById("txtCaptchCode").style.border = "";
    }
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

       // var valid_g_captcha = validateform();
      //  if (valid_g_captcha == false) {
      //      alert("Invalid Captcha");
      //      return;
      //  }

        var valid_g_captcha = document.getElementById("valrecaptcha1").value;
        if (valid_g_captcha == "false") {
            alert("Invalid Captcha");
            return;
        }

        var ma = document.getElementById("txtEmailAdd");
        var fn = document.getElementById("txtFullname");
        var p = document.getElementById("txtPhone");
        var q = document.getElementById("txtQuestionx");
      //  var cc = document.getElementById("txtCaptchCode");
        var fname = document.getElementById("familyName");
       // var err3 = document.getElementById("errCaptchCode1");
      //  var err3 = $('#errCaptchCode1').attr('innerHTML');
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
//        if (cc.value != err3) {
//            valid = false;
//           // err2.style.display = "block";
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
                url: "/fl.aspx/SendAskQuestionMail",
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
            url: "/fl.aspx/ValidateCaptcha",
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
        //var qtyval = document.forms[0].elements[buyvalue].value.trim();
        var qtyval = document.getElementById(buyvalue).value.trim();
        // var qtyavail = document.forms[0].elements[buyvalue].name;
        var qtyavail = buyvalue;
        qtyavail = qtyavail.toString().split('_')[1];
        //var minordqty = document.forms[0].elements[buyvalue].name;
        var minordqty = buyvalue;
        minordqty = minordqty.toString().split('_')[2];
        //var fid = document.forms[0].elements[buyvalue].name;
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
            //window.document.location = '/OrderDetails.aspx?&amp;bulkorder=1&amp;Pid=' + pid + '&amp;Qty=' + qtyval;
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
</script>
<script type="text/javascript">
    function test(id) {

        var testid = "popupouterdiv" + id;   
        var objDiv = document.getElementById(testid);
        var pidr = objDiv.id;
        document.getElementById(pidr).style.visibility = 'hidden';      
        var podv = $(objDiv).html();
        var objDiva = document.getElementById(id);      
        var d = document.getElementById(pidr);      
        var menuDiv1 = document.getElementById('testdiv');
       var menuDiv = document.getElementById('testdiv');
       $(menuDiv).html('');
       $(menuDiv).css("width", "220px");
       $(menuDiv).css("visibility", "visible");
       $(menuDiv).css("padding", "5px");
       $(menuDiv).append(podv);
       var IE = document.all ? true : false;
       if (IE) {
          
           tempX = event.clientX + document.documentElement.scrollLeft;
           tempY = event.clientY + document.documentElement.scrollTop;
           MouseX = tempX;
           MouseY = tempY;
           $(menuDiv).css(
        {
           position: "absolute",         
             left : MouseX - (menuDiv.clientWidth -120) + 'px',
              top : MouseY - (menuDiv.clientHeight + 35) + 'px'       
        }
        );
       }
      else {
          
           $(menuDiv).css(
        {
            position: "absolute",
            left: MouseX - (menuDiv.clientWidth - 120) + 'px',
            top: (MouseY - (menuDiv.offsetHeight + 35)) + 'px'
        }
         );
       }
    }
    function Mouseout(id) {
        var testid = "popupouterdiv" + id;
        var objDiv = document.getElementById(testid);
        var pidr = objDiv.id;
        document.getElementById(pidr).style.visibility = 'hidden';
        document.getElementById('testdiv').style.visibility = 'hidden';
    }
    function createMenuDiv() {
        var menuDiv = document.createElement("div");
        $(menuDiv).css("background-color", "yellow");
        $(menuDiv).css("z-index", "1");
        $(menuDiv).css("width", "220px");
        $(menuDiv).css("height", "70px");
        $(menuDiv).css("visibility", "visible");       
        return menuDiv;

    }
    function Moverstockstatus(id) {
        var testid = "pid" + id;
        var objDiv = document.getElementById(testid);
        var pidr = objDiv.id;
        document.getElementById(pidr).style.visibility = 'hidden';
        var podv = $(objDiv).html();
        var objDiva = document.getElementById(id);

        if (objDiva!=null)
            var pida = objDiva.id;

        var d = document.getElementById(pidr);
          var menuDiv1 = document.getElementById('testdiv');
        var menuDiv = document.getElementById('testdiv');
        $(menuDiv).html('');
        $(menuDiv).css("width", "220px");
        $(menuDiv).css("visibility", "visible");       
        $(menuDiv).css("padding", "5px");      
        $(menuDiv).append(podv);
        var IE = document.all ? true : false;
        if (IE) {
            tempX = event.clientX + document.documentElement.scrollLeft;
            tempY = event.clientY + document.documentElement.scrollTop;
            MouseX = tempX;
            MouseY = tempY;
            $(menuDiv).css(
        {
                    position: "absolute",         
            left: MouseX - (menuDiv.clientWidth - 120) + 'px',
            top: MouseY - (menuDiv.clientHeight + 35) + 'px'
        }
        );
        }
        else {
            $(menuDiv).css(
        {
            position: "absolute",
            left: MouseX - (menuDiv.clientWidth - 120) + 'px',
            top: (MouseY - (menuDiv.offsetHeight + 35)) + 'px'
        }
        );
        }
    }
    function Moutstockstatus(id) {
        var testid = "pid" + id;
        var objDiv = document.getElementById(testid);
        var pidr = objDiv.id;
        document.getElementById(pidr).style.visibility = 'hidden';
        document.getElementById('testdiv').style.visibility = 'hidden';
    }

    //    new script
    function Moverimgtag(id) {
        var testid = "pro_img_popup" + id;
        var objDiv = document.getElementById(testid);
        var pidrnew = objDiv.id;
       // document.getElementById(pidrnew).style.visibility = 'hidden';
        var podvimg = $(objDiv).html();
        var objDiva = document.getElementById(id);
        var d = document.getElementById(pidrnew);
        var menuDiv1 = document.getElementById('testdivimg');
        var menuDiv = document.getElementById('testdivimg');
        $(menuDiv).html('');
        $(menuDiv).css("visibility", "visible");
        $(menuDiv).append(podvimg);
        var IE = document.all ? true : false;
        if (IE) {
            tempX = event.clientX + document.documentElement.scrollLeft;
            tempY = event.clientY + document.documentElement.scrollTop;
            MouseX = tempX;
            MouseY = tempY;
            $(menuDiv).css(
        {

            position: "absolute",
            left: MouseX - (menuDiv.clientWidth - 200) + 'px',
            top: MouseY - (menuDiv.clientHeight + 35) + 'px'
        }
        );
        }
    else {

            $(menuDiv).css(
        {
              
            position: "absolute",
            left: MouseX - (menuDiv.clientWidth - 200) + 'px',
            top: (MouseY - (menuDiv.offsetHeight + 35)) + 'px'
        }
         
        );
        }
    }
    function Moutimgtag(id) {
        var testid = "pro_img_popup" + id;

        var objDivnew = document.getElementById(testid);
        var pidrnew = objDivnew.id;

        document.getElementById(pidrnew).style.visibility = 'hidden';
        document.getElementById('testdivimg').style.visibility = 'hidden';
    }  
</script>
<script language="javascript" type="text/javascript">

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

       // var valid_g_captcha = validateform_BulkBuyPP();
       // if (valid_g_captcha == false) {
      //      alert("Invalid Captcha");
      //      return;
      //  }

        var valid_g_captcha2 = document.getElementById("valrecaptcha2").value;
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
      
       // var capcode = document.getElementById("txtCaptchCode_BBPP");
 
       // var caperr = $('#errCaptchCode1_BBPP').attr('innerHTML');
        var valid = true;
        Controlvalidate_BulkBuyPP("fullname");
        Controlvalidate_BulkBuyPP("email");
        Controlvalidate_BulkBuyPP("phone");
        // Controlvalidate_BulkBuyPP("notes");
        Controlvalidate_BulkBuyPP("deltime");
        Controlvalidate_BulkBuyPP("qty");
      //  Controlvalidate_BulkBuyPP("capcode");
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
//        if (capcode.value != caperr) {
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
                url: "/fl.aspx/SendBulkBuyProjectPricing",
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
//            // var err3 = document.getElementById("errCaptchCode1_BBPP");
//            var err3 = $('#errCaptchCode1_BBPP').attr('innerHTML');
//            if (cn != null && cn.value == "") {

//                cn.style.border = "1px solid #FF0000";
//                err1.style.display = "block";
//            }
//            else {
//                cn.style.border = "";
//                err1.style.display = "none";


//                if (cn.value != err3 && cn.value != null) {
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
        document.getElementById("errvalidmail_BBPP").style.display = "none";
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

            var bbpp = document.getElementById("bulkbuy");
            var bbppsub = document.getElementById("bulkbuysubmit");
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
            url: "/fl.aspx/ValidateCaptcha_BBPP",
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
       // var capcode_du = document.getElementById("txtCaptchCode_DU");
        // var caperr_du = document.getElementById("errCaptchCode1_DU");
       // var caperr_du = $('#errCaptchCode1_DU').attr('innerHTML');
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
//        if (capcode_du.value != caperr_du) {
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
                url: "/fl.aspx/DownloadUpdate",
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
//            //  var err3 = document.getElementById("errCaptchCode1_DU");
//            var err3 = $('#errCaptchCode1_DU').attr('innerHTML');
//            if (cn != null && cn.value == "") {

//                cn.style.border = "1px solid #FF0000";
//                err1.style.display = "block";
//            }
//            else {
//                cn.style.border = "";
//                err1.style.display = "none";


//                if (cn.value != err3 && cn.value != null) {
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
        document.getElementById("txtEmail_DU").value = "";
        document.getElementById("txtPhone_DU").value = "";
        document.getElementById("txtdownloadre").value = "";
       // document.getElementById("txtCaptchCode_DU").value = "";
        document.getElementById("Errfullname_DU").style.display = "none";
        document.getElementById("erremailadd_DU").style.display = "none";
        document.getElementById("Errphone_DU").style.display = "none";
        document.getElementById("errdownloadre").style.display = "none";
        document.getElementById("errvalidmail_DU").style.display = "none";
//        document.getElementById("errCaptchInvalid_DU").style.display = "none";
//        document.getElementById("errCaptchCode_DU").style.display = "none";
//        document.getElementById("errCaptchCode1_DU").style.display = "none";
        document.getElementById("txtFullname_DU").style.border = "";
        document.getElementById("txtEmail_DU").style.border = "";
        document.getElementById("txtPhone_DU").style.border = "";
        document.getElementById("txtdownloadre").style.border = "";
       // document.getElementById("txtCaptchCode_DU").style.border = "";
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
            url: "/fl.aspx/ValidateCaptcha_DU",
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
<script type="text/javascript" language="javascript">
    $('html').click(function (e) {
        if (e.target.id == 'jquery-lightbox') {
            // $("#jquery-lightbox").remove();
            //$("#jquery-overlay").remove();
            // $("#imageinnerdiv").css('margin-top', '0px');
            $("#jquery-lightbox").remove(); $("#jquery-overlay").fadeOut(function () { $("#jquery-overlay").remove(); }); $("embed, object, select").css({ visibility: "visible" })
            $(document).unbind();
        }
    });

                function btn_popupclose() {
                   // $("#jquery-lightbox").remove();
                    //$("#jquery-overlay").remove();
                    $("#jquery-lightbox").remove(); $("#jquery-overlay").fadeOut(function () { $("#jquery-overlay").remove(); }); $("embed, object, select").css({ visibility: "visible" })
                    $(document).unbind();
                }
</script>

<script type="text/javascript" language="javascript">
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


</script>

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
            if (activeTab == "#tab4") {
                $(".withDownupdate").show();
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
            if (activeTab == "#shipping") {
                var shipload = document.getElementById("shippingLoad");
                if (shipload != null) {
                    shipload.style.display = "block";
                }
                shippinginfoload();
            }
            return false;
        });
    });
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
    window.onbeforeunload = function (e) {
        $("#" + '<%= hfcheckload.ClientID %>').val("1");
        $("#" + '<%= HFcnt.ClientID %>').val("0");
    }         
    </script>
<script type="text/javascript">


    $(document).ready(function () {






        function lastPostFunc() {

            try {
                $('#tblload').toggle();
                $('#tblload').show();

                var irecords = '11';
                var hfpageno = $("#" + '<%= HFcnt.ClientID %>').val();

                var fid = $("#" + '<%= hffid.ClientID %>').val();

                hfpageno = parseInt(hfpageno) + 1;

                var eapath = $("#" + '<%= hfeapath.ClientID %>').val();


                $("#" + '<%= HFcnt.ClientID %>').val(hfpageno)
                var pagecount = $("#" + '<%= itotalrecords.ClientID %>').val();

                var Rawurl = $("#" + '<%= hfrawurl.ClientID %>').val();

                $.ajax({
                    type: "POST",
                    url: "/fl.aspx/DynamicPag",
                    data: "{'ipageno':" + hfpageno + ",'_Fid':'" + fid + "','eapath':'" + eapath + "','Rawurl':'" + Rawurl + "','Pagecnt':'" + pagecount + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (data.d != "") {

                            $('.divLoadData:last').before(data.d);

                            // $("#ctl00_maincontent_Productlist1_searchrsltproductfamily1_balrecords_cnt").val("0");


                            $("#" + '<%= hfcheckload.ClientID %>').val("1");



                            jQuery(document).ready(function () {
                                $("img.lazy").lazyload();
                            });

                        }
                        else {


                            $("#" + '<%= hfcheckload.ClientID %>').val("0");
                            $('#tblload').hide();
                            $('#data').toggle();
                            $('#data').show();
                        }
                        $('#divPostsLoader').empty();
                    },
                    error: function (xhr, status, error) {
                        $('#tblload').hide();
                        //                        var err = eval("(" + xhr.responseText + ")");
                        //                        alert(err);
                    }
                })
            }

            catch (err) {
                alert(err.Message);
            }
        };
        $(window).scroll(function () {

            var checkload = $("#" + '<%= hfcheckload.ClientID %>').val();
    
            if (checkload == 1 && $(window).scrollTop() > 800) {

                $("#" + '<%= hfcheckload.ClientID %>').val("0");
                lastPostFunc();
            }
            else {
                //$('#tblload').hide();
            }


        });
    });



    function shippinginfoload() {

        var testval = "shippinginfo";
        var shipload = document.getElementById("shippingLoad");
        if (shipload != null) {
            $.ajax({
                type: "POST",
                url: "/fl.aspx/Shiipinginfo",
                data: "{'value':'" + testval + "'}",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: OnShippingSuccess,
                error: OnShippingFailure


            });
        }
    }
    function OnShippingSuccess(result) {
        if (result.d != null && result.d != "-1") {

            var q = document.getElementById("shipping");
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
                url: "/fl.aspx/BulkBuyLoad",
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
</script>
<%--<div class="addthis_toolbox addthis_default_style addthis_16x16_style" style="text-align:right;float: right;" >

<a class="addthis_button_facebook" ></a>
<a class="addthis_button_twitter" style="text-align:right"></a>
<a class="addthis_button_linkedin" style="text-align:right"></a>
<a class="addthis_button_email" style="text-align:right"></a>
<a class="addthis_button_compact" style="text-align:right"></a>
<a class="addthis_counter addthis_bubble_style" style="text-align:right"></a>

</div>

<script type="text/javascript">    var addthis_config = { "data_track_addressbar": true };</script>
<script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-531947a24f1f0d18"></script>
--%>
<%--<div class="br" style="clear:both;">--%>
<div class="container">
 <div class="row">
 <%
                Response.Write(Bread_Crumbs());
  %>
  </div>
<%--</div> --%>
<%--<div class="clear"></div>--%>

<%--
<%=Generateparentfamilyhtml()%>

<div class="tabbable">
  <ul class="tabs">
    <li>
      <a href="#tab1" >Products</a>
    </li>
    <li style=display:<%= ST_Family_Download() %>; >
      <a href="#tab2" >Downloads</a>
    </li>
  </ul>
  <div class="clear"></div>
  <div class="tab-content">
    <div id="tab1" class="tab_content" >
      <%=ST_Familypage()%>
    </div>
    <%=DownloadST %> 
  </div>
</div >
</div> --%>
<%--<div class="log">log</div>--%>



   <%--<cc1:CaptchaControl ID="cVerify" CaptchaChars="23456789"  CaptchaLength="4" CaptchaBackgroundNoise="Low" runat="server"   OnPreRender="cVerify_preRender"    style="display:none;" />--%>


   <div class="row">
<%=ST_FamilypageALLData()%>
</div>
</div>
  <input type="hidden" id="hfclickedattr" name="hfclickedattr" value="" />
  <input type="hidden" id="Hidden1" name="hfclickedattr" value="" />
  
            <asp:HiddenField ID="HFcnt" runat="server" />
            <asp:HiddenField ID="hfcheckload" runat="server"  />
              <asp:HiddenField ID="itotalrecords" runat="server" />
               <asp:HiddenField ID="hffid" runat="server" />
                    <asp:HiddenField ID="hfeapath" runat="server" />
         <asp:HiddenField ID="hfrawurl" runat="server" />
          
<div id="testdiv" class="ppdivtest" ></div>
<div id="testdivimg" class="testdivimgcss" ></div>
<%--<script type="text/javascript" language="javascript">
    jQuery(document).ready(function () {
        $("img.lazy").lazyload();
    });
    (function ($, window, document, undefined) { var $window = $(window); $.fn.lazyload = function (options) { var elements = this; var $container; var settings = { threshold: 0, failure_limit: 0, event: "scroll", effect: "show", container: window, data_attribute: "original", skip_invisible: true, appear: null, load: null, placeholder: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCC" }; function update() { var counter = 0; elements.each(function () { var $this = $(this); if (settings.skip_invisible && !$this.is(":visible")) { return } if ($.abovethetop(this, settings) || $.leftofbegin(this, settings)) { } else { if (!$.belowthefold(this, settings) && !$.rightoffold(this, settings)) { $this.trigger("appear"); counter = 0 } else { if (++counter > settings.failure_limit) { return false } } } }) } if (options) { if (undefined !== options.failurelimit) { options.failure_limit = options.failurelimit; delete options.failurelimit } if (undefined !== options.effectspeed) { options.effect_speed = options.effectspeed; delete options.effectspeed } $.extend(settings, options) } $container = (settings.container === undefined || settings.container === window) ? $window : $(settings.container); if (0 === settings.event.indexOf("scroll")) { $container.bind(settings.event, function () { return update() }) } this.each(function () { var self = this; var $self = $(self); self.loaded = false; if ($self.attr("src") === undefined || $self.attr("src") === false) { if ($self.is("img")) { $self.attr("src", settings.placeholder) } } $self.one("appear", function () { if (!this.loaded) { if (settings.appear) { var elements_left = elements.length; settings.appear.call(self, elements_left, settings) } $("<img />").bind("load", function () { var original = $self.attr("data-" + settings.data_attribute); $self.hide(); if ($self.is("img")) { $self.attr("src", original) } else { $self.css("background-image", "url('" + original + "')") } $self[settings.effect](settings.effect_speed); self.loaded = true; var temp = $.grep(elements, function (element) { return !element.loaded }); elements = $(temp); if (settings.load) { var elements_left = elements.length; settings.load.call(self, elements_left, settings) } }).attr("src", $self.attr("data-" + settings.data_attribute)) } }); if (0 !== settings.event.indexOf("scroll")) { $self.bind(settings.event, function () { if (!self.loaded) { $self.trigger("appear") } }) } }); $window.bind("resize", function () { update() }); if ((/(?:iphone|ipod|ipad).*os 5/gi).test(navigator.appVersion)) { $window.bind("pageshow", function (event) { if (event.originalEvent && event.originalEvent.persisted) { elements.each(function () { $(this).trigger("appear") }) } }) } $(document).ready(function () { update() }); return this }; $.belowthefold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = (window.innerHeight ? window.innerHeight : $window.height()) + $window.scrollTop() } else { fold = $(settings.container).offset().top + $(settings.container).height() } return fold <= $(element).offset().top - settings.threshold }; $.rightoffold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.width() + $window.scrollLeft() } else { fold = $(settings.container).offset().left + $(settings.container).width() } return fold <= $(element).offset().left - settings.threshold }; $.abovethetop = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollTop() } else { fold = $(settings.container).offset().top } return fold >= $(element).offset().top + settings.threshold + $(element).height() }; $.leftofbegin = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollLeft() } else { fold = $(settings.container).offset().left } return fold >= $(element).offset().left + settings.threshold + $(element).width() }; $.inviewport = function (element, settings) { return !$.rightoffold(element, settings) && !$.leftofbegin(element, settings) && !$.belowthefold(element, settings) && !$.abovethetop(element, settings) }; $.extend($.expr[":"], { "below-the-fold": function (a) { return $.belowthefold(a, { threshold: 0 }) }, "above-the-top": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-screen": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-screen": function (a) { return !$.rightoffold(a, { threshold: 0 }) }, "in-viewport": function (a) { return $.inviewport(a, { threshold: 0 }) }, "above-the-fold": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-fold": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-fold": function (a) { return !$.rightoffold(a, { threshold: 0 }) } }) })(jQuery, window, document);
</script>--%>


