<div id="tab3" class="tab-pane">
<div id="bulkbuy" >
  <p> Contact us for Bulk Buy and Project Based Pricing for $TBT_FAMILY_NAME_BB$</p>
  <h3></h3>
  <form>
    <div class="row">
      <div class="col-xs-20 col-sm-10">
        <label>
          Product Code<span class="required">*</span>
        </label>
        <select class="form-control" id="ddlprodcode" name="ddlprodcode" onchange="getprodcodevalue();" onblur="Controlvalidate_BulkBuyPP('procode')" tabindex="1">
          $if(TBT_CHK_PRODCOUNT)$
          <option  selected="selected" value="Please Select Product" title="Please Select Product">Please Select Product</option>
          $endif$
          $TBW_DDL_VALUE$
        </select>
        <span class="mandatory" style="display:none;" id="Errprocode_BBPP"> Select Product code </span>
      </div>
      <div class="col-xs-20 col-sm-10">
        <label>
          Full Name<span class="required">*</span>
        </label>
        <input type="text" class="form-control" id="txtFullname_BBPP" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')" tabindex="2"/>
        <span class="mandatory" style="display:none;" id="Errfullname_BBPP"> Enter Full Name </span>
                                        </div>
    </div>
    <div class="row">
      <div class="col-xs-20 col-sm-10">
        <label>
          QTY Required<span class="required">*</span>
        </label>
        <input type="text" id="txtQTY"   tabindex="3"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('qty')"   onkeypress="return validateNumber(event);" class="form-control"/>
        <span class="mandatory" style="display:none;" id="ErrQTY"> Enter QTY </span>
      </div>
      <div class="col-xs-20 col-sm-10">
        <label>
          Email<span class="required">*</span>
        </label>
        <input type="email" class="form-control" id="txtEmail"  tabindex="4"  maxlength="50"  onblur="Controlvalidate_BulkBuyPP('email')" />
        <span class="mandatory" style="display:none;" id="erremailadd_BBPP"> Enter Email Address </span>
        <span class="mandatory" style="display:none;" id="errvalidmail_BBPP">Enter Valid Email </span>
      </div>
    </div>
    <div class="row">
      <div class="col-xs-20 col-sm-10">
        <label>
          Delivery Time<span class="required">*</span>
        </label>
        <input type="text" class="form-control" id="txtdeliverytime"   tabindex="5"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('deltime')"/>
        <span class="mandatory" style="display:none;" id="Errdeliverytime">Enter Delivery Time </span>
      </div>
      <div class="col-xs-20 col-sm-10">
        <label>
          Phone<span class="required">*</span>
        </label>
        <input type="text" class="form-control" id="txtPhone_BBPP"  tabindex="6"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('phone');" onkeypress="return validateNumber(event);" />
        <span class="mandatory" style="display:none;" id="Errphone_BBPP">Enter Phone Number </span>
      </div>
    </div>
    <div class="row">
      <div class="col-xs-20 col-sm-10">
        <label>
          Target Price<span class="required"></span>
        </label>
        <input type="text" class="form-control" id="txttargetprice"   tabindex="7" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')" />
                                        </div>
      <div class="col-xs-8 col-sm-3">
        <label style=" visibility: hidden;">
          Code<span class="required">*</span>
        </label>
        <div id="recaptcha2"></div>
        <!--<input type="text" class="form-control"  id="txtCaptchCode_BBPP"  tabindex="9"  maxlength="4"  onblur="Controlvalidate_BulkBuyPP('capcode');" onkeypress="return validateNumber(event);"/>
        <span class="mandatory" style="display:none;" id="errCaptchCode_BBPP"> Enter the code </span>
        <span class="mandatory" style="display:none;" id="errCaptchInvalid_BBPP"> Invalid code </span>
        <span class="mandatory" id="errCaptchCode1_BBPP" style="display:none;"></span>-->
                                        </div>
      <!--<div class="col-xs-12 col-sm-6">
        <img alt="captcha" src="" id="captcha_image_BBPP" style="margin:25px 0 0 20px;" class="img-responsive"/>
                                        </div>-->
    </div>
    <br />
      <div class="row">
        <div class="col-xs-20">
          <label>
            Notes / Addtional Info.<span class="required">*</span>
          </label>
          <textarea id="txtnotesadditionalinfo" cols="34" tabindex="8" class="form-control fullwidth"  rows="10" maxlength="600"  onblur="Controlvalidate_BulkBuyPP('notes')" onkeypress="textCounter_BulkBuyPP(this,this.form.counter_BBPP,600);"></textarea>
          <span class="mandatory" style="display:none;" id="errnotes"></span>
        </div>
        <div class="col-xs-20">

          <input type="text" onblur="textCounter_BulkBuyPP(this.form.counter_BBPP,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter_BBPP" class="form-control character"/>
          <span>
            <label>Chars Remaining </label>
          </span>
        </div>
      </div>
      <br />
        <p id="review_btn">
          <button value="Submit" onclick="MailSend_BulkBuyPP()" type="button" class="btn btn-default">Submit</button>
          <button value="Reset" onclick="MailReset_BulkBuyPP()" type="reset" class="btn btn-default">Reset</button>
        </p>
      </form>


</div>
<div class="alert alert-success" id="bulkbuysubmit" style="display:none;">
  <strong>Success!</strong> Thanks for your enquiry. Our Customer Sales Team will be in contact with you Shortly.
</div>

</div>