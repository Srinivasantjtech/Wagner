<div id="bulkbuy" >
          <p> Contact us for Bulk Buy and Project Based Pricing for $TBT_FAMILY_NAME$</p>
          <h3></h3>
          <form>
            <div class="row">
              <div class="col-xs-20 col-sm-10">
                <label>
                  Product Code<span class="required">*</span>
                </label>
                <input type="text" class="form-control" id="txtproductcode" readonly="true" value="$TBT_CODE$"  maxlength="30" />
              </div>
              <div class="col-xs-20 col-sm-10">
                <label>
                  Full Name<span class="required">*</span>
                </label>
                <input type="text" class="form-control" id="txtFullname_BBPP" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')"/>
                <span class="mandatory dpynone" id="Errfullname_BBPP" > Enter Full Name </span>
              </div>
            </div>
            <div class="row">
              <div class="col-xs-20 col-sm-10">
                <label>
                  QTY Required<span class="required">*</span>
                </label>
                <input type="text" class="form-control" id="txtQTY" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('qty')"   onkeypress="return validateNumber(event);"/>
                <span class="mandatory dpynone" id="ErrQTY"> Enter QTY </span>
              </div>
              <div class="col-xs-20 col-sm-10">
                <label>
                  Email<span class="required">*</span>
                </label>
                <input type="email" class="form-control" id="txtEmail" maxlength="50"  onblur="Controlvalidate_BulkBuyPP('email')" />
                <span class="mandatory dpynone" id="erremailadd_BBPP"> Enter Email Address </span>
                <span class="mandatory dpynone" id="errvalidmail_BBPP">Enter Valid Email </span>
              </div>
            </div>
            <div class="row">
              <div class="col-xs-20 col-sm-10">
                <label>
                  Delivery Time<span class="required">*</span>
                </label>
                <input type="text" class="form-control" id="txtdeliverytime" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('deltime')"/>
                <span class="mandatory dpynone" id="Errdeliverytime">Enter Delivery Time </span>
              </div>
              <div class="col-xs-20 col-sm-10">
                <label>
                  Phone<span class="required">*</span>
                </label>
                <input type="text" class="form-control" id="txtPhone_BBPP" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('phone');" onkeypress="return validateNumber(event);" />
                <span class="mandatory dpynone" id="Errphone_BBPP">Enter Phone Number </span>
              </div>
            </div>
            <div class="row">
              <div class="col-xs-20 col-sm-10">
                <label>
                  Target Price<span class="required"></span>
                </label>
                <input type="text" class="form-control" id="txttargetprice" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')"/>
              </div>
              <div class="col-xs-8 col-sm-3">
                <label class="vishid">
                  Code<span class="required">*</span>
                </label>
                <div id="recaptcha2"></div>
           
              </div>
    
            </div>
            <br />
            <div class="row">
              <div class="col-xs-20">
                <label>
                  Notes / Addtional Info.<span class="required">*</span>
                </label>
                <textarea id="txtnotesadditionalinfo" cols="34"  class="form-control fullwidth" rows="10" maxlength="600"
                                                    onblur="Controlvalidate_BulkBuyPP('notes')" onkeypress="textCounter_BulkBuyPP(this,this.form.counter_BBPP,600);" ></textarea>
                <span class="mandatory dpynone" id="errnotes">Enter The Notes / Addtional Info.</span>
              </div>
              <div class="col-xs-20">

                <input type="text" onblur="textCounter_BulkBuyPP(this.form.counter_BBPP,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter_BBPP"  class="form-control character"/>
                <span>
                  <label>Chars Remaining </label>
                </span>
              </div>
            </div>
            <br />
            <p id="review_btn">
              <button value="Submit" type="button" onclick="MailSend_BulkBuyPP()" class="btn btn-default">Submit</button>
              <button value="Reset" type="reset"  onclick="MailReset_BulkBuyPP()" class="btn btn-default">Reset</button>
            </p>
          </form>


        </div>
        <div class="alert alert-success" id="BulkBuyPPSubmit" style="display:none;">
          <strong>Success!</strong> Thanks for your Bulk Buy enquiry. Our Customer Sales Team will be in contact with you Shortly.
        </div>