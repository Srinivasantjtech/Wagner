
<div id="downloads" >
  <p> No downloads currently available for $TBT_FAMILY_NAME_DU$</p>
  <p> Please contact us with your download requirements,e.g. User Manual, Data Sheet, etc and will get back to you.</p>
  <h3></h3>
  <form>
    <div class="row">

      <div class="col-sm-10">
        <div class="col-xs-20 col-sm-20">
          <label>
            Full Name<span class="required">*</span>
          </label>
          <input type="text" id="txtFullname_DU"  class="form-control" tabindex="1"   maxlength="30"  onblur="Controlvalidate_DU('fullname_du')" />
          <span  id="Errfullname_DU" class="mandatory" style="display:none;"> Enter Full Name </span>
                                            </div>

        <div class="col-xs-20 col-sm-20">
          <label>
            Email<span class="required">*</span>
          </label>
          <input type="email" class="form-control" id="txtEmail_DU"     tabindex="2"  maxlength="50"   onblur="Controlvalidate_DU('email_du')"/>
          <span class="mandatory" style="display:none;" id="erremailadd_DU"> Enter Email Address </span>
          <span class="mandatory" style="display:none;" id="errvalidmail_DU">Enter Valid Email </span>
                                            </div>

        <div class="col-xs-20 col-sm-20">
          <label>
            Phone<span class="required">*</span>
          </label>
          <input type="text" id="txtPhone_DU"  class="form-control" tabindex="3"   maxlength="30"  onblur="Controlvalidate_DU('phone_du');" onkeypress="return validateNumber(event);"  />
          <span class="mandatory" style="display:none;" id="Errphone_DU">Enter Phone Number </span>
                                            </div>


        <div class="col-xs-8 col-sm-8">
          <label style=" visibility: hidden;">
            Code<span class="required">*</span>
          </label>
          <div id="recaptcha3"></div>
        </div>
 

      </div>

      <div class="col-sm-10">
        <div class="col-xs-20">
          <label>
            Download Required / Comments*<span class="required">*</span>
          </label>
          <textarea id="txtdownloadre" cols="34"  class="form-control fullwidth"  tabindex="5"   rows="10" maxlength="600" onblur="Controlvalidate_DU('notes_du')" onkeypress="textCounter_DU(this,this.form.counter_DU,600);"></textarea>
          <span class="mandatory" style="display:none;" id="errdownloadre">Download Required / Comments.</span>
        </div>
        <div class="col-xs-20">

          <input type="text" onblur="textCounter_DU(this.form.counter_DU,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter_DU" class="form-control character"/>
          <span>
            <label>Chars Remaining </label>
          </span>
        </div>
      </div>

    </div>
    <br />
      <p id="review_btn">
        <button value="Submit" onclick="MailSend_DU()" type="button" class="btn btn-default">Submit</button>
        <button value="Reset" onclick="MailReset_DU()" type="reset" class="btn btn-default">Reset</button>
      </p>
    </form>


</div>
<div class="alert alert-success" id="DUSubmit" style="display:none;">
  <strong>Success!</strong> Thanks for your enquiry. Our Customer Sales Team will be in contact with you Shortly.
</div>
