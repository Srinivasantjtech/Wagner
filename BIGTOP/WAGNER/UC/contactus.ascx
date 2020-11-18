<%@ Control Language="C#"  Inherits="UC_contactus" AutoEventWireup="true" Codebehind="contactus.ascx.cs" %>
<%--<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>--%>
<%--<script src="http://maps.googleapis.com/maps/api/js?sensor=false"></script> --%>

<%--<style>
    .google-maps {
        position: relative;
        padding-bottom: 75%; // This is the aspect ratio
        height: 0;
        overflow: hidden;
    }
    .google-maps iframe {
        position: absolute;
        top: 0;
        left: 0;
        width: 100% !important;
        height: 100% !important;
    }
</style>--%>
<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&key=AIzaSyD6KpDELswcok62eMQN_L2qBpBud3mqpGM&signed_in=true&libraries=places"></script>

<script type="text/javascript">
  google.maps.event.addDomListener(window, 'load', initialize);
    function initialize() {
        //if (navigator.geolocation) {
        //    navigator.geolocation.getCurrentPosition(function (position) {
        //        var pos = {
        //            lat: position.coords.latitude,
        //            lng: position.coords.longitude
        //        };
        //     //alert(position.coords);
        //        getAddressFromLatLang(position.coords.latitude, position.coords.longitude);
        //        //  infoWindow.setPosition(pos);
        //        //    infoWindow.setContent('Location found.');
        //        // infoWindow.open(map);
        //        //  map.setCenter(pos);
        //    }, function () {
        //        // handleLocationError(true, infoWindow, map.getCenter());
        //    });
        //}
        var options = {
         
            componentRestrictions: { country: "AU" }
        };
        var autocomplete = new google.maps.places.Autocomplete(document.getElementById("txtfromaddress"), options);
        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            
            // Get the place details from the autocomplete object.
            var place = autocomplete.getPlace();
            embedmap(document.getElementById("txtfromaddress").value);
            //var location = "<b>Address</b>: " + place.formatted_address + "<br/>";
            //location += "<b>Latitude</b>: " + place.geometry.location.A + "<br/>";
            //location += "<b>Longitude</b>: " + place.geometry.location.F;
            //document.getElementById('lblResult').innerHTML = location
        });
    }

    function initialize_buttonclick() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                //alert(position.coords);
                getAddressFromLatLang(position.coords.latitude, position.coords.longitude);
                //  infoWindow.setPosition(pos);
                //    infoWindow.setContent('Location found.');
                // infoWindow.open(map);
                //  map.setCenter(pos);
            }, function () {
                // handleLocationError(true, infoWindow, map.getCenter());
            });
        }
        //var options = {

        //    componentRestrictions: { country: "AU" }
        //};
        //var autocomplete = new google.maps.places.Autocomplete(document.getElementById("txtfromaddress"), options);
        //google.maps.event.addListener(autocomplete, 'place_changed', function () {

        //    // Get the place details from the autocomplete object.
        //    var place = autocomplete.getPlace();
        //    embedmap(document.getElementById("txtfromaddress").value);
        //    //var location = "<b>Address</b>: " + place.formatted_address + "<br/>";
        //    //location += "<b>Latitude</b>: " + place.geometry.location.A + "<br/>";
        //    //location += "<b>Longitude</b>: " + place.geometry.location.F;
        //    //document.getElementById('lblResult').innerHTML = location
        //});
    }
</script>

    <script>

 var map, infoWindow;
        //function initMap() {
        //    //map = new google.maps.Map(document.getElementById('map'), {
        //    //    center: { lat: -34.397, lng: 150.644 },
        //    //    zoom: 6
        //    //});
        //    //infoWindow = new google.maps.InfoWindow;
        //    alert("x");
        //    // Try HTML5 geolocation.
        //    if (navigator.geolocation) {
        //        navigator.geolocation.getCurrentPosition(function (position) {
        //            var pos = {
        //                lat: position.coords.latitude,
        //                lng: position.coords.longitude
        //            };
        //            console.log(position.coords);
        //            getAddressFromLatLang(position.coords.latitude, position.coords.longitude);
        //          //  infoWindow.setPosition(pos);
        //        //    infoWindow.setContent('Location found.');
        //           // infoWindow.open(map);
        //          //  map.setCenter(pos);
        //        }, function () {
        //           // handleLocationError(true, infoWindow, map.getCenter());
        //        });
        //    } else {
        //        // Browser doesn't support Geolocation
        //       // handleLocationError(false, infoWindow, map.getCenter());
        //    }
        //}
        function getAddressFromLatLang(lat,lng){
           
            var geocoder = new google.maps.Geocoder();
            var latLng = new google.maps.LatLng(lat, lng);
            geocoder.geocode( { 'latLng': latLng}, function(results, status) {
               
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[1]) {
                       // alert(results[1].formatted_address);
                      //  document.getElementById("txtfromaddress").value = results[1].formatted_address;
                    
                        embedmap(results[1].formatted_address);
                    }
                  
                }else{
                    embeddefault();
                }
            });
          
        }

        function embedmap(fromaddress) {

            $("address").each(function () {
                var tooadd = document.getElementById("ctl00_maincontent_contact1_toaddress").value;
                //   var fromadd = document.getElementById("txtfromaddress").value;
                var fromadd;
                if (fromaddress != '') {
                    fromadd = fromaddress;
                }
                else {
                    fromadd = document.getElementById("txtfromaddress").value;
                }
                //    var embed = "<div class='google-maps'><iframe width='380' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + encodeURIComponent($(this).text()) + "&amp;output=embed'></iframe></div>";

                var embed = "<div class='google-maps'><iframe width='100%' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://www.google.com/maps/embed/v1/directions?key=AIzaSyD6KpDELswcok62eMQN_L2qBpBud3mqpGM&origin=" + fromadd + "&destination=" + tooadd + "&avoid=tolls|highways'></iframe></div>";
                if (embed != null) {
                    $(this).html(embed);
                }
                else {
                    embed = "<div class='google-maps'><iframe width='100%' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + tooadd + "&amp;output=embed'></iframe></div>";
                    $(this).html(embed);
                }


            });

        }

  </script>
  <%--  <script 
    src="https://maps.googleapis.com/maps/api/js?key=AIzaSyD6KpDELswcok62eMQN_L2qBpBud3mqpGM&callback=initMap">
    </script>--%>


<script type="text/javascript">

   
   $(window).bind("pageshow", function () {
     
          $('#main_menu li a').removeClass('focused');
          $('#Contactus a').addClass('focused');
           
      });

      function ShowAddress() {
         // $('#gmap').toggle()
         // $('#gmap').show();
         
      }


    $(document).ready(function () {
        $("address").each(function () {
            var tooadd = encodeURIComponent($(this).text());
            var fromadd = document.getElementById("txtfromaddress").value;
         
   var embed = "<div class='google-maps'><iframe width='100%' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + encodeURIComponent($(this).text()) + "&amp;output=embed'></iframe></div>";
     
       $(this).html(embed);
   
        });
    });
    function embeddefault() {

        $("address").each(function () {
            var tooadd = document.getElementById("ctl00_maincontent_contact1_toaddress").value;
         
            //    var embed = "<div class='google-maps'><iframe width='380' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + encodeURIComponent($(this).text()) + "&amp;output=embed'></iframe></div>";

            var embed = "<div class='google-maps'><iframe width='100%' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + tooadd + "&amp;output=embed'></iframe></div>";
                $(this).html(embed);
          


        });

    }
   
//    var myCenter = new google.maps.LatLng(-33.890365, 151.129108);

//    function initialize() {
//        var mapProp = {
//            center: myCenter,
//            zoom: 18,
//            mapTypeId: google.maps.MapTypeId.HYBRID
//        };

//        var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
//    }

//    google.maps.event.addDomListener(window, 'load', initialize);

</script>
    <script type="text/javascript">
//      $(document).ready( function() {
//          var maph = document.getElementById("gmap");
//          maph.style.display = "none";
//            });

//        function showmap() {
//            var mapshow = document.getElementById("gmap");
//            mapshow.style.display = "block";
//        }
        function SendEnquiryMail() {
            var t1 = document.getElementById("ctl00_maincontent_contact1_T1");
            var email = document.getElementById("ctl00_maincontent_contact1_T2");
            var t3 = document.getElementById("ctl00_maincontent_contact1_T3");
            var s1 = document.getElementById("ctl00_maincontent_contact1_S1");
            var valid = true;
           Controlvalidate("t1");
            Controlvalidate("email");
            Controlvalidate("t3");
            Controlvalidate("s1");
            if (t1 == null || t1.value.trim() == "") {
                valid = false;
                // alert("enter Full Name")
                Controlvalidate("t1");
                t1.focus();
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

            if (s1 == null || s1.value.trim() == "") {
                valid = false;
                s1.focus();
                return;
            }
            if (t3 == null || t3.value.trim() == "") {
                valid = false;
                t3.focus();
                return;
            }






            var s = t1.value;
            t1.value = s.replace(/'/g, "`");
            s = s1.value;
            s1.value = s.replace(/'/g, "`");
          

            if (valid == true) {
                $.ajax({
                    type: "POST",
                    url: "/contactus.aspx/SendEnquiryMail",
                    data: "{'fullname':'" + t1.value.trim() + "','email':'" + email.value.trim() + "','phone':'" + t3.value.trim() + "','enquiry':'" + s1.value.trim() + "'}",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: OnmailSuccess,
                    error: OnmailFailure
                });
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
        function Controlvalidate(ctype) {
            if (ctype == "t1") {
                var dd = document.getElementById("ctl00_maincontent_contact1_T1");
                var err1 = document.getElementById("errfirstname");
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
                var cno = document.getElementById("ctl00_maincontent_contact1_T2");
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
            if (ctype == "t3") {
                var cn = document.getElementById("ctl00_maincontent_contact1_T3");
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

            if (ctype == "s1") {
                var cn = document.getElementById("ctl00_maincontent_contact1_S1");
                var err1 = document.getElementById("errenquiry");
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
            function OnmailSuccess(result) {

        var dt;
        if (result.d != null && result.d != "-1") {
        //alert(result.d);
          window.location.href="ConfirmMessage.aspx?Result=MESSAGESENT";
        }
        else {
            window.location.href="ConfirmMessage.aspx?Result=MESSAGENOTSENT";
        }

    }
    function OnmailFailure(result) {
        window.location.href="ConfirmMessage.aspx?Result=MESSAGENOTSENT";
    }
    </script>
   <%--       <script type="text/javascript">
              function textCounter(field, countfield, maxlimit) {
                  if (field.value.length > maxlimit) {
                      field.value = field.value.substring(0, maxlimit);
                      alert('Enquiry/Comments maximum allowed 320 characters.');
                      return false;
                  }
                  else {
                      countfield.value = maxlimit - field.value.length;
                  }
              }
</script>

<table width="806" border="0" cellpadding="0" cellspacing="0">

  <tr>
    <td width="778" height="27" valign="top" align="left">
      <table width="778" border="0" cellpadding="0" cellspacing="0">
      
        <tr>
          <td width="761" height="24" valign="top">
            <span class="txt_11">
              <strong>
                <a href="/Home.aspx" class="txt_11">Home</a>
              </strong>
            </span> / Contact Us
          </td>
        </tr>

      </table>
    </td>
  </tr>
  <tr>
    <td valign="top" align="left">
    <div class="box1" style="margin:0 0 0 5px;width:774px;">
      <table width="765" border="0" cellpadding="0" cellspacing="0">
 
        <tr>
          <td height="48" colspan="3" valign="top">
            <h1 class="bold_h1tag" >Contact Us</h1>
          </td>
        </tr>
        <tr>
         <td  width="20px">
          </td>
          <td width="409px" rowspan="2" valign="top">

           <p class="txt_aboutus"> Full Name:<span style="color: red">*</span><br />
              <input type="text" name="T1" size="20" id="T1" runat="server" class="inputtxtcu" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="* Required Field"
                                                ControlToValidate="T1" ValidationGroup="Mandatory" SetFocusOnError="true" SkinID="lblRequiredSkin"></asp:RequiredFieldValidator>
         <span  style="display: inline-block; color: red;width:100px;text-align:right  ">*Required</span>
          </p>
          
            <p class="txt_aboutus">
                                            Email Address:<span style="color: red">*</span>
                                            <br/>
                                            <input type="text" name="T2" size="20" id="T2" runat="server" class="inputtxtcu"/>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Required Field"
                                                ControlToValidate="T2" ValidationGroup="Mandatory" SetFocusOnError="true" SkinID="lblRequiredSkin"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="T2"
                                                ErrorMessage="Require Valid Email" SkinID="lblRequiredSkin" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Width="128px" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                                        </p>
                                           <p class="txt_aboutus">
                                            Contact Number:<br/>
                                            <input type="text" name="T3" size="20" id="T3" runat="server" class="inputtxtcu"/>
                                           
                                            
                                            <br />
                                        </p>
                                         <p class="txt_aboutus">
                                         Enquiry/Comments:<span style="color: red">*</span>
                                        <br/>
                                        <textarea rows="18" name="S1" id="S1" cols="50" runat="server" class="textarea" onkeypress="textCounter(this,this.form.counter,320);" ></textarea>
                                        <input class="inputtxtta"  style="width:35px; color:#B2B2B2;"	type="text"	name="counter"	maxlength="3" readonly="readonly"	size="3"	value="320"	onblur="textCounter(this.form.counter,this,320);"> Chars Remaining
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=""
                                            ControlToValidate="S1" ValidationGroup="Mandatory" SetFocusOnError="true" SkinID="lblRequiredSkin"></asp:RequiredFieldValidator>
                                        <br/>
                                        <br/>
                                         </p>
                                          <p class="txt_aboutus"><p class="txt_aboutus"> Form Verify. Please enter text code shown below:</p><br />
                                            <asp:Label ID="lblresult" runat="server" Text="" style="COLOR: red;"></asp:Label>
                                            <br />
                                           
                                            <cc1:CaptchaControl ID="cVerify" CaptchaChars="23456789" CaptchaLength="4" CaptchaBackgroundNoise="Low" runat="server" />
                                          <p class="txt_aboutus">Text Code <span style="color: red">*</span> <asp:TextBox ID="cText" runat="server" value="" class="inputtxtcu" Width="130px"></asp:TextBox> 
                                         
                                           <p class="txt_aboutus" align="right"></p></p>

                                        </p>
                                        
                                           <p>
                                         </p>
                                      
          </td>
          <td  width="20px">
          </td>
          <td width="6" height="722px" valign="top" class="bdr_left">
          </td>
          <td  width="40px">
          </td>
          <td width="270px" rowspan="2" valign="top" align="left">
          <%
    Response.Write(ST_contactus());
%>
</td>
        </tr>

      </table>
      </div>
    </td>

  </tr>
  <tr>
    <td>&nbsp;</td>
  </tr>
</table>
--%>
<div class="container">
<div class="row hidden-xs hidden-sm">
    	<div class="col-sm-20">
        	<ul id="mainbredcrumb" class="breadcrumb">
            	<li><a href="/home.aspx">Home</a></li>
                <li class="active">Contact Us</li>  
            </ul>
        </div>
    </div>
    <div class="row">
    <div class="col-md-4 main-left">
    <%   
          Response.Write(ST_Newproduct());
     %>
        	<%--<div class="categorysearch categoryheading clearfix hidden-xs hidden-sm">
            <h4>New Products</h4>
            </div>
            <div id="innerpgearo" class="categoryselectmenu hidden-xs hidden-sm">

            	<div class="product-left">
                          	<div class="newpro_name">
                            	<h4>Product Name</h4>
                            </div>
                            <div class="newpro-img">
                            	<img class="img-responsive" src="images/wagner_product1.png"/>
                            </div>
                            <div class="newpro_code">
                            	<p>Order Code</p>
                            </div>
                      </div>
    
                     <div class="product-left">
                          	<div class="newpro_name">
                            	<h4>Product Name</h4>
                            </div>
                            <div class="newpro-img">
                            	<img class="img-responsive" src="images/wagner_product1.png"/>
                            </div>
                            <div class="newpro_code">
                            	<p>Order Code</p>
                            </div>
                         </div>
        
            </div>--%>
       	</div>
        <div class="col-md-16">
        <div class="row">
        <div class="col-sm-20 col-md-20 clearfix">
         <div class="ctgry-headpanel clearfix">
                        <div class="categoryheading">
                            <h4>Contact Us</h4>
                        </div>
                        
                    </div>
                    <div class="contact-pge">
                     <div class="row">
                          <div class="col-sm-10">
                      <%
    Response.Write(ST_contactus());
%>
                     </div>
                     <div class="col-sm-10">
                                <div class="contacpge_form">
                                    <div class="mobileform-head">
                                         <h4>Contact Form</h4>
                                     </div>
                                     <div>
                                     <form class="form-horizontal">
                                           <div class="form-group">
                                                        <label for="Name">Full Name:</label>
                                                        <input type="text" class="form-control" id="T1" runat="server" onblur="Controlvalidate('t1')"/>
                                                        <span id="errfirstname" style="display:none;" class="mandatory">Enter First Name</span>
                                                         
                                           </div>
                                            <div class="form-group">
                                                        <label for="Name">Email Address:</label>
                                                        <input type="text" class="form-control"  size="20" id="T2" runat="server" onblur="Controlvalidate('email')"/>
                                                        <span id="erremailadd" style="display: none;" class="mandatory"> Enter Email Address </span>
                                                        <span id="errvalidmail" style="display: none;" class="mandatory">Enter Valid Email </span>
                                            </div>
                                            <div class="form-group">
                                                        <label for="Name">Contact Number:</label>
                                                        <input type="text" class="form-control"  size="20" id="T3" runat="server" onblur="Controlvalidate('t3')" onkeypress="return validateNumber(event);"/>
                                                        <span id="Errphone" style="display: none;" class="mandatory">Enter Contact Number </span>
                                            </div>
                                            <div class="form-group">
                                                        <label for="Name">Enquiry / Questions / Comments:</label>
                                                        <textarea class="form-control" rows="2"  id="S1" cols="50" runat="server" type="text" onblur="Controlvalidate('s1')"></textarea>
                                                        <span id="errenquiry" style="display: none;" class="mandatory">Enter  Questions / Comments.</span>
                                            </div>
                                            <div class="form-group">
      <%-- <asp:Button ID="BtnRequest"  runat="server"  OnClick="BtnRequest_Click"  Text="Login" /> --%>
      <button value="Submit" class="btn" type="button" onclick="SendEnquiryMail();">Submit</button>
                                            </div>
                                       
                                       </form>
                                       </div>
                                   </div>
                            </div>
                
                     </div>
                        <div class="row">
                            <div class="direction">
<div class="form-group clearfix">
                          <div class="col-sm-20">
  <h4 style="color: rgb(0, 0, 0); font-weight: bold; letter-spacing: 1px; font-size: 24px; margin-bottom:0px;">Directions to our store</h4>
  <span style="padding-bottom: 10px; display: block;">Enter the address you are leaving from to get directions to our store</span>
</div>
     <div class="col-sm-20">
        <label>Enter your Departing Address / Location</label>
                                
         </div>
                                  <div class="col-sm-12">
                                     
                                     <input type="text" class="form-control col-md-9" id="txtfromaddress" placeholder="Enter your address" /><br /><br />

                                    
                                      <asp:HiddenField ID="toaddress" runat="server" Value="Wagner Electronic Services, 84-90 Parramatta Road, Ashfield NSW 2130,Australia" />
                                             </div> 
    <div class="col-sm-8 text-right">
         <button value="Submit" style="font-size:13px" class="btn btn-primary" type="button" onclick="embedmap('');">Find Route</button>
              <button value="Submit" style="font-size:13px" class="btn btn-primary"  type="button" onclick="initialize_buttonclick('');">Route From Current Location</button>

        </div>
                                
</div>
                                <div class="contactinfo-box clearfix" id="gmap" style="display:block;">
  <address>
    Wagner Electronic Services,84-90 Parramatta Road, Ashfield NSW 2130,Australia
  </address>
</div>
</div>
                            </div>
                        </div>
                    </div>
        </div>
        </div>
        </div>
    </div>
