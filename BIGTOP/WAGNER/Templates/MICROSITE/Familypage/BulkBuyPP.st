

<h4 class=" semi_bold font_15 margin_bottom_15 margin_top_20 padding_left_right_15 line_height_25 arial">Contact us for Bulk Buy and Project Based Pricing for $TBT_FAMILY_NAME_BB$ </h4>

  <div id="BulkBuyPP" style="display:block;">


      <div class="form-group col-lg-6 form_Group_height">
        <label for="exampleInputEmail1"  class="font_normal margin_bottom">Product Code</label>
        
        <select  id="ddlprodcode" name="ddlprodcode" class="form-control"   onchange="getprodcodevalue();" onblur="Controlvalidate_BulkBuyPP('procode')" tabindex="1">
          $if(TBT_CHK_PRODCOUNT)$
          <option  SELECTED="SELECTED" value="Please Select Product" title="Please Select Product">Please Select Product</option>
          $endif$
          $TBW_DDL_VALUE$
        </select>
        <span class="error" id="Errprocode_BBPP" style="display:none;color: Red;"> Select Product code </span>
      </div>
    
      <div class="form-group col-lg-6 form_Group_height">
        <label for="exampleInputEmail1"  class="font_normal margin_bottom">
          Full Name<span class="error"> *</span>
        </label>
        <input type="text" id="txtFullname_BBPP"  class="form-control ask_question_input"  placeholder="Full Name" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')" />
        <span  class="error"  id="Errfullname_BBPP" style="display:none;color: Red;"> Enter Full Name </span>
      </div>
      <div class="form-group col-lg-6 form_Group_height">
        <label for="exampleInputPassword1"  class="font_normal margin_bottom">
          QTY Required<span class="error"> *</span>
        </label>
        <input type="text" id="txtQTY" class="form-control ask_question_input" placeholder="QTY Required"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('qty')"   onkeypress="return validateNumber(event);"/>
        <span  class="error"  id="ErrQTY" style="display:none;color: Red;"> Enter QTY </span>
      </div>
    
    <div class="form-group col-lg-6 form_Group_height">
        <label for="exampleInputPassword1"  class="font_normal margin_bottom">
          Email<span class="error"> *</span>
        </label>
        <input type="text" id="txtEmail"  class="form-control ask_question_input"  placeholder="Email"  maxlength="50"  onblur="Controlvalidate_BulkBuyPP('email')"  placeholder="Email"/>
        <span  class="error"  id="erremailadd_BBPP" style="display:none;color: Red;"> Enter Email Address </span>
        <span  class="error"  id="errvalidmail_BBPP" style="display:none;color: Red;">Enter Valid Email </span>
      </div>
    
      <div class="form-group col-lg-6 form_Group_height">
        <label for="exampleInputPassword1"  class="font_normal margin_bottom">
          Delivery Time<span class="error"> *</span>
        </label>
        <input type="text" id="txtdeliverytime"  class="form-control ask_question_input"  placeholder="Delivery time"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('deltime')" />
        <span  class="error"  id="Errdeliverytime" style="display:none;color: Red;">Enter Delivery Time </span>
      </div>
    
    
      <div class="form-group col-lg-6 form_Group_height">
        <label for="exampleInputPassword1"  class="font_normal margin_bottom">
          Phone<span class="error"> *</span>
        </label>
        <input type="text" id="txtPhone_BBPP" class="form-control ask_question_input"  placeholder="Phone"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('phone');" onkeypress="return validateNumber(event);"  />
        <span  class="error"  id="Errphone_BBPP" style="display:none;color: Red;">Enter Phone Number </span>
      </div>
    
      <div class="form-group col-lg-6 form_Group_height">
        <label for="exampleInputPassword1"  class="font_normal margin_bottom">
          Target Price
        </label>
        <input type="text" id="txttargetprice"  class="form-control ask_question_input"  placeholder="Target Price"  style="width:248px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')"  />
      </div>


      <div class="form-group col-lg-6">
        <label for="exampleInputPassword1"  class="font_normal margin_bottom">
          Notes / Addtional Info. <span class="error"> *</span>
        </label>
    
        <textarea id="txtnotesadditionalinfo" cols="34"  class="form-control ask_question_input"    rows="3" maxlength="600" 
                                            onblur="Controlvalidate_BulkBuyPP('notes')" onkeypress="textCounter_BulkBuyPP('txtnotesadditionalinfo','counter_BBPP',600);" ></textarea>
        <div class="clear">
          <input  class="char_remain" type="text" onblur="textCounter_BulkBuyPP('txtnotesadditionalinfo','counter_BBPP',600);" value="600" size="3" readonly="readonly" maxlength="3" id="counter_BBPP" name="counter_BBPP" style="color:#B2B2B2;height: 30px;padding: 6px;width: 35px !important;" /> Chars Remaining 
        </div>               
        <span  class="error"  id="errnotes" style="display:none;color: Red;">Enter The Notes / Addtional Info.</span>

      </div>

      <div class="form-group col-lg-6 form_Group_height">
        <div id="recaptcha2"></div>
        <!--<label class="col-sm-3 col-md-5 control-label font_normal" for="inputEmail3">
          <img src="$CAPTCHA_IMAGE$"   />
        </label>
        
        <input type="text" id="txtCaptchCode_BBPP"   class="form-control ask_question_input width_size mob_left"   maxlength="4"  onblur="Controlvalidate_BulkBuyPP('capcode');" onkeypress="return validateNumber(event);"   />
        

        <span  class="error mob_left mob_pad_left"  id="errCaptchCode_BBPP" style="display:none;"> Enter the code </span>
        <span  class="error mob_left mob_pad_left"  id="errCaptchInvalid_BBPP" style="display:none;"> Invalid code </span>
        <span  class="error mob_left mob_pad_left"  id="errCaptchCode1_BBPP" style="display:none;">$CC_CODE$</span>-->
      </div>
      <div class="form-group col-lg-6">

        <input type="button" value="Submit" name="" class="btn green_bg white_color border_radius_none height_34" onclick="MailSend_BulkBuyPP()"/>
        <input type="button" value="Reset" name=""  class="btn blue_color white_color border_radius_none height_34" onclick="MailReset_BulkBuyPP()"/>
      </div>
     
   



      




      



      










  </div>


<div id="BulkBuyPPSubmit"  role="alert" class="alert alert-success border_radius_none text-center" style="display:none;">
  <span aria-hidden="true" class="glyphicon glyphicon-ok-circle margin_right"></span>
  <strong>  Thanks for your Bulk Buy enquiry..! </strong> Our Customer Sales Team will be in contact with you Shortly.
</div>


