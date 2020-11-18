$if(TBT_DOWNLOAD)$
<h4 class=" semi_bold font_15 margin_bottom_15 margin_top_20 padding_left_right_15 line_height_25 arial">
  No downloads currently available for $FNAME_DU$
</h4>
$endif$
<h4 class=" semi_bold font_15 margin_bottom_15 padding_left_right_15 line_height_25 arial">
  Please contact us with your download requirements,e.g. User Manual, Data Sheet, etc and will get back to you.
</h4>
<div class="form-group col-lg-6">
  <label class="margin_bottom font_normal" for="exampleInputEmail1">Full Name</label>
  <input type="text" placeholder="Full Name" id="txtFullname_DU" class="form-control ask_question_input" maxlength="30"  onblur="Controlvalidate_DU('fullname_du')"/>
  <span class="error" id="Errfullname_DU" style="display:none;color: Red;"> Enter Full Name </span>
</div>
<div class="form-group col-lg-6">
  <label class="margin_bottom font_normal" for="exampleInputEmail1">Email address</label>
  <input type="email" placeholder="Enter email" id="txtEmail_DU" maxlength="50"  onblur="Controlvalidate_DU('email_du')"  class="form-control ask_question_input"/>
  <span class="error" id="erremailadd_DU" style="display:none;color: Red;"> Enter Email Address </span>
  <span class="error" id="errvalidmail_DU" style="display:none;color: Red;">Enter Valid Email </span>
</div>
<div class="form-group col-lg-6">
  <label class="margin_bottom font_normal" for="exampleInputPassword1">Phone</label>
  <input type="text" placeholder="Phone" id="txtPhone_DU" class="form-control ask_question_input" maxlength="30"  onblur="Controlvalidate_DU('phone_du');" onkeypress="return validateNumber(event);"/>
  <span class="error" id="Errphone_DU" style="display:none;color: Red;">Enter Phone Number </span>
</div>

<div class="form-group col-lg-6">
  <label class="margin_bottom font_normal" for="exampleInputPassword1">Download Required / Comments*</label>

  <textarea rows="3" id="txtdownloadre"  maxlength="600" class="form-control ask_question_input"  onblur="Controlvalidate_DU('notes_du')" onkeypress="textCounter_DU('txtdownloadre','counter_DU',600);"></textarea>
  <span class="error" id="errdownloadre" style="display:none;color: Red;">Download Required / Comments.</span>
  <div class="clear">
    <input  class="char_remain readonly" type="text" onblur="textCounter_DU('txtdownloadre','counter_DU',600);" value="600" size="3" readonly="readonly" maxlength="3" id="counter_DU" name="counter_DU"/> Chars Remaining
  </div>
</div>
<div class="form-group col-lg-6 form_Group_height">
    <!--<label for="inputEmail3" class="col-sm-3 col-md-5 control-label font_normal">
    <img src="$CAPTCHA_IMAGE$" />
  </label>
  <input type="text" class="form-control ask_question_input width_size mob_left" id="txtCaptchCode_DU" maxlength="4"  onblur="Controlvalidate_DU('capcode_du');" onkeypress="return validateNumber(event);"/>
  <span class="error mob_left mob_pad_left" id="errCaptchCode_DU" style="display:none;color: Red;"> Enter the code </span>
  <span class="error mob_left mob_pad_left" id="errCaptchInvalid_DU" style="display:none;color: Red;"> Invalid code </span>
  <span class="error mob_left mob_pad_left" id="errCaptchCode1_DU" style="display:none;">$CC_CODE$</span>-->
   <div id="recaptcha3"></div>
</div>
<div class="form-group col-lg-6">
  <input type="button" value="Submit" name="" class="btn green_bg white_color border_radius_none height_34" onclick="MailSend_DU()" />
  <input type="button" value="Reset" name="" class="btn blue_color white_color border_radius_none height_34" onclick="MailReset_DU()" />
              </div>
