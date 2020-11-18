<!--<div class="col-lg-8 margin_top_20 col-sm-8">
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
        <h4 class="font_14 col-lg-12 margin_top">Form Verify. Please enter text shown below:</h4>
        <div class="form-group">
          <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top col-md-4 ">
            <img src="$CAPTCHA_IMAGE$"/>
          </label>
          <div class="col-sm-4 col-md-3 ">
            <input type="text" class="form-control width_size margin_top_20 checkout_input" id="txtcapcode" maxlength="4"   onblur="Controlvalidate_mscontactus('ccode');" onkeypress="return validateNumber(event);"/>
            <span class="error-text" id="errcapcode" style="display:none;color: Red;"> Enter Code </span>
            <span class="error-text" id="errcapcodeinvalid" style="display:none;color: Red;"> Invalid code </span>
            <span class="error-text" id="errcapcode1" style="display:none;">$CC_CODE$</span>
            </div>
          <div class="col-lg-4 col-md-4 ">
         
            <button class="btn-lg margin_top_20 padding_top green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub" onclick="MailSend_mscontactus();return false;" autopostback="false">Submit</button>
          </div>
          <div class="clear"></div>
        </div>
      </div>
    </form>
  </div>-->
<div itemtype="http://schema.org/LocalBusiness" itemscope="" class="col-lg-4 col-sm-4">
  <h4 class="bolder blue_color_text font_18">
    <span itemprop="name">Wagner Electronic Services</span>
  </h4>
  <ul class="list_style_none list-group address">
    <div itemtype="http://schema.org/PostalAddress" itemscope="" itemprop="address">
      <li>
        <img class=" margin_right16" src="$CDN$images/micrositeimages/pointer.png"/>
          <span itemprop="streetAddress">84-90 Parramatta Road,</span>
          </br>
            <span itemprop="addressLocality" class="mar_left_30">Summer Hill</span>, <span itemprop="Addressregion">NSW</span>, <span itemprop="postalCode">2130</span></br>
              <span itemprop="Addresscountry" class="mar_left_30">Australia</span>
      </li>
    </div>
    <li>
      <img class=" margin_right16" src="$CDN$images/micrositeimages/phone.png" alt=""/>
        <span itemprop="telephone">(+61) 02 9798 9233</span>
    </li>
    <li>
      <img class=" margin_right16" src="$CDN$images/micrositeimages/fax.png"/>
        <span itemprop="faxNumber">(+61) 02 9798 0017</span>
    </li>
    <li>
      <img class=" margin_right16" src="$CDN$images/micrositeimages/mail_add.png"/>
        <a class="gray_40" href="mailto:sales@wagneronline.com.au">sales@wagneronline.com.au</a>
    </li>
    <li>
      <img class=" margin_right16" src="$CDN$images/micrositeimages/web.png"/>
        <a class="gray_40" href="https://www.wagneronline.com.au" target="_blank">www.wagneronline.com.au</a>
    </li>
  </ul>
  <h4 class="bolder blue_color_text font_18">Business Hours </h4>
  <ul class="list_style_none list-group address">
    <li>8.30AM - 5PM Monday to Friday</li>
    <li>9.00AM - 4PM Saturday</li>
  </ul>
</div>
  <div class="clear"></div>
<div id="mscontactsuccessmessage" style="display:none;margin-left:10px;background-color: #e6f7ee;border: thin solid #85d6ad;border-radius: 6px;color: #339966;font-size: 12px;font-weight: bold;padding: 14px;width: 800px;">
  <img style="margin-right: 10px;vertical-align: middle;" src="$CDN$images/tick2.png"/>
  <span>
    Thanks for your enquiry. Our Customer Sales Team will be in contact with you Shortly.
  </span>
</div>


