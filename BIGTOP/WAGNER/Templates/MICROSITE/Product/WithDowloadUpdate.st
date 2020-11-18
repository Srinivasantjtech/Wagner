
  <br/>
  <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Please contact us with your download requirements,e.g. User Manual, Data Sheet, etc and will get back to you.</span>
  <br/>
  <br/>
  <div id="DownloadUpdate" style="display:block;">

    <div class="grid4" style="width:45%;">

      <div>Full Name*</div>
      <div>
        <input type="text" id="txtFullname_DU"  class="prod-form" style="width:248px;"   maxlength="30"  onblur="Controlvalidate_DU('fullname_du')" />
        <span class="error-text" id="Errfullname_DU" style="display:none;color: Red;"> Enter Full Name </span>
      </div>
      <div>    Email*</div>
      <div>
        <input type="text" id="txtEmail_DU"  class="prod-form" style="width:248px;"   maxlength="50"  onblur="Controlvalidate_DU('email_du')" />
        <span class="error-text" id="erremailadd_DU" style="display:none;color: Red;"> Enter Email Address </span>
        <span class="error-text" id="errvalidmail_DU" style="display:none;color: Red;">Enter Valid Email </span>
      </div>
      <div>
        Phone*
      </div>
      <div>
        <input type="text" id="txtPhone_DU"  class="prod-form" style="width:248px;"   maxlength="30"  onblur="Controlvalidate_DU('phone_du');" onkeypress="return validateNumber(event);"  />
        <span class="error-text" id="Errphone_DU" style="display:none;color: Red;">Enter Phone Number </span>
      </div>
      <div>
        Code*
      </div>
      <div >
        <!--<input type="text" id="txtCaptchCode_DU"  class="prod-form" style="width:50px!important;"   maxlength="4"  onblur="Controlvalidate_DU('capcode_du');" onkeypress="return validateNumber(event);"   />
        <img src="$CAPTCHA_IMAGE$"  width="120" height="40"  />
        <span class="error-text" id="errCaptchCode_DU" style="display:none;color: Red;"> Enter the code </span>
        <span class="error-text" id="errCaptchInvalid_DU" style="display:none;color: Red;"> Invalid code </span>
        <span class="error-text" id="errCaptchCode1_DU" style="display:none;">$CC_CODE$</span>-->

        <div id="recaptcha3"></div>
      </div>




      <div style="width:225px;" >
        <input type="button" value="Submit" name="" class="subminbtn" onclick="MailSend_DU()" />
        <input type="button" value="Reset" name="" class="resetbtn" onclick="MailReset_DU()" />

      </div>
      
    </div>

    <div class="grid4" style="width:45%;">
      <div>Download Required / Comments* </div>
      <div >
        <textarea id="txtdownloadre" cols="34"   class="prod-txtarea"    rows="10" maxlength="600" style="box-shadow: 0 0 0px #CCCCCC; border:1px solid #b2b2b2;height:144px;width:390px;"
                                            onblur="Controlvalidate_DU('notes_du')" onkeypress="textCounter_DU(this,this.form.counter_DU,600);" ></textarea>
        <input class="prod-form" type="text" onblur="textCounter_DU(this.form.counter_DU,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter_DU" style="width:35px!important; color:#B2B2B2;" />
        <span style=" color: #7f7f7f;font-size: 11px;font-weight: bold;">Chars Remaining </span>
        <span class="error-text" id="errdownloadre" style="display:none;color: Red;">Download Required / Comments.</span>
      </div>

    </div>
  </div>
  <div id="DUSubmit" class="DUSubmitdiv" >
    <div class="DUdiv">
      <img src="$CDN$images/tick2.png"  class="tick2img" alt="" />
      <span>
        Thanks for your download enquiry. Our Customer Sales Team will be in contact with you Shortly.
      </span>
    </div>

  </div>

