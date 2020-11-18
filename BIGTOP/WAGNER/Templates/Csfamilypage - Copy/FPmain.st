<div class="col-md-20">
  <div class="row">
    <div class="row product-info">
      $Generateparentfamilyhtml$
      <div class="col-sm-20">
        <div class="tabs">
          <ul id="myTab" class="nav nav-tabs">
            <li class="active">
              <a href="#profile">Products</a>
            </li>
            <li>
              <a href="#tab2">Ask a Question</a>
            </li>
            <li>
              <a href="#tab3">Bulk Buy / Project Price</a>
            </li>
            <li style="display:$ST_Family_Download$">
              <a href="#tab4">Downloads</a>
            </li>
            <li>
              <a href="#shipping">Shipping Info</a>
            </li>
          </ul>
          <div class="tab-content">
            <div id="profile" class="tab-pane active">
            
              <div class="table-responsive inner-table-table">
                <!--$ST_Category$-->
                $ST_Familypage$
                <!--<div class="clearfix text-center"></div>-->
              
                <div class="text-center" id="tblload" style="display:none">
                  <img src="/images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
                  <br>Loading Data.. Please Wait..</br>
                </div>
                
                
              </div>
            </div>
            $DownloadST$
            <div id="tab2" class="tab-pane">
            <div id="messages">
              <p>
                <a href="#review_btn"></a>
              </p>
              <h3></h3>
              <form>
                <div class="row">
                  <div class="col-xs-20 col-sm-10">
                    <label>
                      Full Name<span class="required">*</span>
                    </label>
                    <input type="text" id="txtFullname" maxlength="30"  onblur="Controlvalidate('fn')" class="form-control"/>
                    <span class="mandatory" id="Errfullname" style="display:none;">Enter Full Name</span>
                  </div>
                  <div class="col-xs-20 col-sm-10">
                    <label>
                      Email<span class="required">*</span>
                    </label>
                    <input type="text" class="form-control" id="txtEmailAdd" maxlength="50"  onblur="Controlvalidate('ea')" />
                    <span class="mandatory" id="erremailadd" style="display:none;"> Enter Email Address </span>
                    <span class="mandatory" id="errvalidmail" style="display:none;">Enter Valid Email </span>
                  </div>
                </div>
                <div class="row">
                  <div class="col-xs-20 col-sm-10">
                    <label>
                      Phone<span class="required">*</span>
                    </label>
                    <input type="text" class="form-control" id="txtPhone" maxlength="30"  onblur="Controlvalidate('p');" onkeypress="return validateNumber(event);"/>
                    <span class="mandatory" id="Errphone" style="display:none;">Enter Phone Number </span>
                                        </div>
                  <div class="col-xs-8 col-sm-3">
                    <label>
                      Code<span class="required">*</span>
                    </label>
                    <input type="text" class="form-control" id="txtCaptchCode" maxlength="4"  onblur="Controlvalidate('cc');" onkeypress="return validateNumber(event);" />
                    <span class="mandatory" id="errCaptchCode" style="display:none;"> Enter the code </span>
                    <span class="mandatory" id="errCaptchInvalid" style="display:none;"> Invalid code </span>
                    <span class="mandatory" id="errCaptchCode1" style="display:none;" value=""></span>
                                        </div>
                  <div class="col-xs-12 col-sm-6">
                    <img alt="captcha" src="" id="captcha_image" style="margin:25px 0 0 20px;"/>
                                        </div>
                </div>
                <br />
                  <div class="row">
                    <div class="col-xs-20">
                      <label>
                        Questions<span class="required">*</span>
                      </label>
                      <textarea  rows="10" maxlength="600"  class="form-control fullwidth" id="txtQuestionx" cols="34" onblur="Controlvalidate('q')" onkeypress="textCounter(this,this.form.counter,600);"></textarea>
                      <span class="mandatory" id="errquestion" style="display:none;">Enter The Question </span>
                    </div>
                    <div class="col-xs-20">

                      <input type="text" placeholder="600" class="form-control character"  onblur="textCounter(this.form.counter,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter"/>
                      <span>
                        <label>Chars Remaining </label>
                      </span>
                    </div>
                  </div>
                  <br />
                    <p id="review_btn">
                      <button value="Submit" type="button" onclick="MailSend()" class="btn btn-default">Submit</button>
                      <button value="Reset" type="reset" onclick="MailReset()" class="btn btn-default">Reset</button>
                    </p>
                  </form>
            </div>
            <div class="alert alert-success" id="divAskQuestionSubmit" style="display:none;">
              <strong>Success!</strong> Thanks for your enquiry. Our Customer Sales Team will be in contact with you Shortly.
            </div>
            </div>
            $ST_BulkBuyPP$
            <div id="shipping" class="tab-pane">

              <h3>&#36;12.95 Australia Wide Flat Rate Shipping*</h3>
              <p>
                No matter where you are located in Australia we offer flat rate shipping.
                Some conditions apply, <br />
                  please see our <a href="/Termsandconditions.aspx">Terms and Conditions </a>page.
              </p>

              <h3>Store Pick Up from Sydney Showroom</h3>
              <p>
                Goods can be picked from our premises from 8.30AM to 5.00PM Monday to Friday and from 9AM till 4PM on Saturday.<br />
                  In order to avoid delays it is recommended that you place your order 2 hours prior to pick up.
                  please see our <br />
Terms and Conditions page for more details.</p>

              <h3>International Order Shipping Available</h3>
              <p>
                We ship internationally. Once your order is received we will advise shipping cost to you by email.<br />
 Your order will be updated with the freight cost.</p>

            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  
</div>

