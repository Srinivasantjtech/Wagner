<h4 class=" semi_bold font_15 margin_bottom_15 margin_top_20 padding_left_right_15 line_height_25 arial">
  No downloads currently available for $TBT_FAMILY_NAME_DU$
</h4>
<h4 class=" semi_bold font_15 margin_bottom_15 padding_left_right_15 line_height_25 arial"> Please contact us with your download requirements,e.g. User Manual, Data Sheet, etc and will get back to you. </h4>

<div id="DownloadUpdate" style="display:block;">



    <div class="form-group form_Group_height col-lg-6">
      <label for="exampleInputEmail1" class="margin_bottom font_normal">
        Full Name<span class="error"> *</span>
      </label>
      
        <input type="text" id="txtFullname_DU"  class="form-control ask_question_input"  placeholder="Full Name" maxlength="30"  onblur="Controlvalidate_DU('fullname_du')" />
        
      <span class="error" id="Errfullname_DU" style="display:none;color: Red;"> Enter Full Name </span>
      </div>


    <div class="form-group col-lg-6 form_Group_height">
      <label for="exampleInputEmail1" class="margin_bottom font_normal">
        Email address<span class="error"> *</span>
      </label>
      <input id="txtEmail_DU"  type="email" class="form-control ask_question_input"  maxlength="50"  onblur="Controlvalidate_DU('email_du')"  placeholder="Enter email" />
      <span class="error" id="erremailadd_DU" style="display:none;color: Red;"> Enter Email Address </span>
      <span class="error" id="errvalidmail_DU" style="display:none;color: Red;">Enter Valid Email </span>
    </div>

    
    

    <div class="form-group col-lg-6 form_Group_height">
      <label for="exampleInputPassword1" class="margin_bottom font_normal">
        Phone<span class="error"> *</span>
      </label>
      <input type="text" id="txtPhone_DU"   class="form-control ask_question_input" placeholder="Phone"   maxlength="30"  onblur="Controlvalidate_DU('phone_du');" onkeypress="return validateNumber(event);"  />
      <span  class="error"  id="Errphone_DU" style="display:none;color: Red;">Enter Phone Number </span>
    </div>
    
    <div class="form-group col-lg-6">
    
    <label for="exampleInputPassword1" class="margin_bottom font_normal">
      Download Required / Comments<span class="error"> *</span>
    </label>    
      <textarea id="txtdownloadre" cols="34"   class="form-control ask_question_input"  rows="3" maxlength="600" onblur="Controlvalidate_DU('notes_du')" onkeypress="textCounter_DU('txtdownloadre','counter_DU',600);" ></textarea>
    <div class="clear">
      <input class="char_remain" type="text" onblur="textCounter_DU('txtdownloadre','counter_DU',600);" value="600" size="3" readonly="readonly" maxlength="3" id="counter_DU" name="counter_DU" style="color:#B2B2B2;height: 30px;padding: 6px;width: 35px !important;" />
      Chars Remaining
    </div>      
      <span  class="error"  id="errdownloadre" style="display:none;color: Red;">Download Required / Comments.</span>
   
  </div>

    <div class="form-group col-lg-6 form_Group_height">
      <div id="recaptcha3"></div>
      <!--<label class="col-sm-3 col-md-5 control-label font_normal" for="inputEmail3">
        <img src="$CAPTCHA_IMAGE$"   />
      </label>
      <input type="text" id="txtCaptchCode_DU"  class="form-control ask_question_input width_size mob_left"    maxlength="4"  onblur="Controlvalidate_DU('capcode_du');" onkeypress="return validateNumber(event);"  />

      <span  class="error mob_left mob_pad_left"  id="errCaptchCode_DU" style="display:none;"> Enter the code </span>
      <span  class="error mob_left mob_pad_left"  id="errCaptchInvalid_DU" style="display:none;"> Invalid code </span>
      <span  class="error mob_left mob_pad_left"  id="errCaptchCode1_DU" style="display:none;">$CC_CODE$</span>-->
    </div>




  <div class="form-group col-lg-6">
      <input type="button" value="Submit" name="" class="btn green_bg white_color border_radius_none height_34" onclick="MailSend_DU()" />
      <input type="button" value="Reset" name="" class="btn blue_color white_color border_radius_none height_34" onclick="MailReset_DU()" />
    </div>
   


  
</div>

<div id="DUSubmit" role="alert" class="alert alert-success border_radius_none text-center" style="display:none;">
  <span aria-hidden="true" class="glyphicon glyphicon-ok-circle margin_right"></span>
  <strong> Thanks for your download enquiry..! </strong> Our Customer Sales Team will be in contact with you Shortly.
</div>



