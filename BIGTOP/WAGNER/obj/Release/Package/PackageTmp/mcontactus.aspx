<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mcontactus.aspx.cs" Inherits="WES.mcontactus" %>
<%@ Register Assembly="CustomCaptcha" Namespace="CustomCaptcha" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contentbreadcrumb" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="leftnav" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<script  type="text/javascript" src="https://www.google.com/recaptcha/api.js?onload=myCallBack&render=explicit" async defer></script>
<script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/MicroSitejs/site.js" type="text/javascript"></script>
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
        //        //alert(position.coords);
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

    <script type="text/javascript">

        var map, infoWindow;
    
        function getAddressFromLatLang(lat, lng) {

            var geocoder = new google.maps.Geocoder();
            var latLng = new google.maps.LatLng(lat, lng);
            geocoder.geocode({ 'latLng': latLng }, function (results, status) {

                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[1]) {
                        // alert(results[1].formatted_address);
                        //  document.getElementById("txtfromaddress").value = results[1].formatted_address;

                        embedmap(results[1].formatted_address);
                    }

                } else {
                    embeddefault();
                }
            });

        }

        function embedmap(fromaddress) {

            $("address").each(function () {
                var tooadd = document.getElementById("ctl00_MainContent_toaddress").value;
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
                var tooadd = document.getElementById("ctl00_MainContent_toaddress").value;

                //    var embed = "<div class='google-maps'><iframe width='380' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + encodeURIComponent($(this).text()) + "&amp;output=embed'></iframe></div>";

                var embed = "<div class='google-maps'><iframe width='100%' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + tooadd + "&amp;output=embed'></iframe></div>";
                $(this).html(embed);



            });

        }
  </script>
<%--<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/thickboxAddtocart.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />
<script language="javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/thickboxaddtocart.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript" ></script>
<script type="text/javascript" type="text/javascript">

    /*accordion for filter -End*/
    $(document).ready(function () {
        $("#footeraccordian h3").click(function () {
            //slide up all the link lists
            $("#footeraccordian ul ul").slideUp();
            //slide down the link list below the h3 clicked - only if its closed
            if (!$(this).next().is(":visible")) {
                $(this).next().slideDown();
            }
        })
    })


    function productbuy(buyvalue, pid) {
        try {


            var qtyval = 1;
            var qtyavail = 2;
            qtyavail = buyvalue.toString().split('_')[1];
            var minordqty = buyvalue.toString().split('_')[2];
            //          minordqty = minordqty.toString().split('_')[2];

            var fid = buyvalue.toString().split('_')[3];


            var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";


            if (isNaN(qtyval) || qtyval == "" || qtyval <= 0) {
                alert('Invalid Quantity!');
                //  window.document.forms[0].elements[buyvalue].style.borderColor = "red";
                //  document.forms[0].elements[buyvalue].focus();
                return false;
            }
            else {
                var tOrderID = '<%=Session["ORDER_ID"]%>';

                if (tOrderID != null && parseInt(tOrderID) > 0) {

                    CallProductPopup(orgurl, buyvalue, pid, qtyval, tOrderID, fid);
                }
                else {

                    CallProductPopup(orgurl, buyvalue, pid, qtyval, tOrderID, fid);
                }
            }
        }
        catch (e) {
            alert(e);
        }
    }

  </script>--%>
    <script language="javascript" type="text/javascript">
        var recaptcha1;

        var myCallBack = function () {
            recaptcha1 = grecaptcha.render('recaptcha1', {
                // 'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', //Replace this with your Site key
                'sitekey': '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>',
                'theme': 'light'
            });
        };


       

    </script>

<cc1:CaptchaControl ID="cVerify" CaptchaChars="23456789" CaptchaLength="4" CaptchaBackgroundNoise="Low" runat="server"   OnPreRender="cVerify_preRender"    style="display:none;"  />
<div class="col-lg-8 margin_top_20 col-sm-8">
    <form>
      <div class="form-group col-lg-6">
        <label class="font_normal" for="exampleInputEmail1">Full Name<span class="error"> *</span>
      </label>
        <input type="text" id="mstxtfullname" class="form-control checkout_input" onblur="Controlvalidate_mscontactus('fullname');"/>
        <span class="error-text" id="errfullname" style="display:none;color: Red;"> Enter Full Name </span>
        </div>
      <div class="form-group col-lg-6">
        <label class="font_normal" for="exampleInputPassword1">E-Mail address<span class="error"> *</span>
      </label>
        <input type="email"  id="mstxtemail" onblur="Controlvalidate_mscontactus('email');" class="form-control checkout_input"/>
        <span class="error-text" id="erremailaddress" style="display:none;color: Red;"> Enter Email Address </span>
        <span class="error-text" id="errvalidemail" style="display:none;color: Red;">Enter Valid Email </span>
        </div>
      <div class="form-group col-lg-6">
        <label class="font_normal" for="exampleInputPassword1">Contact Number</label>
        <input type="text" onkeypress="return validateNumber(event);"  id="mstxtphone" class="form-control checkout_input"/>
        </div>
      <div class="form-group col-lg-12">
        <label class="font_normal" for="exampleInputPassword1">Enquiry / Comments <span class="error"> *</span>
      </label>
        <textarea rows="6" class="resize width_100" id="mstxtenquirycomments" onblur="Controlvalidate_mscontactus('comments');"></textarea>
        <span class="error-text" id="errenquiry" style="display:none;color: Red;"> Enter Enquiry/Comments </span>
      </div>
      <div class="form-group col-lg-12">
        <h4 class="font_14 col-lg-12 margin_top">Form Verify.</h4>
        <div class="form-group">
         <%-- <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top col-md-4 ">
            <img src="" runat="server" id="contactcaptcha"/>
          </label>--%>
          <%--<div class="col-sm-4 col-md-3 ">
            <input type="text" class="form-control width_size margin_top_20 checkout_input" id="txtcapcode" maxlength="4"   onblur="Controlvalidate_mscontactus('ccode');" onkeypress="return validateNumber(event);"/>
            <span class="error-text" id="errcapcode" style="display:none;color: Red;"> Enter Code </span>
            <span class="error-text" id="errcapcodeinvalid" style="display:none;color: Red;"> Invalid code </span>
            <span class="error-text" id="errcapcode1" style="display:none;"><asp:Label ID="capchacode" runat="server" Text=""></asp:Label></span>
            </div>--%>
             <div class="form-group col-lg-12">
              <div id="recaptcha1"></div>
              </div>
          <div class="col-lg-4 col-md-4 ">
         
            <button class="btn-lg margin_top_20 padding_top green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub" onclick="MailSend_mscontactus();return false;" autopostback="false" >Submit</button>
          </div>
          <div class="clear"></div>
        </div>
      </div>
    </form>
  </div>
  <% =ST_mcontactus()%>

              <div class="row">
                            <div class="direction">
<div class="form-group clearfix">
                          <div class="col-sm-12">
  <h4 style="color: rgb(0, 0, 0); font-weight: bold; letter-spacing: 1px; font-size: 24px; margin-bottom:0px;">Directions to our store</h4>
  <span style="padding-bottom: 10px; display: block;">Enter the address you are leaving from to get directions to our store</span>
</div>
     <div class="col-sm-12">
        <label>Enter your Departing Address / Location</label>
                                
         </div>
                                  <div class="col-sm-6">
                                     
                                     <input type="text" class="form-control col-md-9" id="txtfromaddress" placeholder="Enter your address" /><br /><br />

                                    
                                      <asp:HiddenField ID="toaddress" runat="server" Value="Wagner Electronic Services,84-90 Parramatta Road, Ashfield NSW 2130,Australia" />
                                             </div> 
    <div class="col-sm-4">
         <button value="Submit"  style="font-size:13px"  class="btn btn-primary" type="button" onclick="embedmap('');">Find Route</button>
         <button value="Submit" style="font-size:13px" class="btn btn-primary"  type="button" onclick="initialize_buttonclick('');">Route From Current Location</button>

         </div>
                                
</div>
                                <div class="contactinfo-box clearfix" id="gmap" style="display:block;">
  <address>
    Wagner Electronic Services, 84-90 Parramatta Road, Ashfield NSW 2130,Australia
  </address>
</div>
</div>
                            </div>
</asp:Content>
