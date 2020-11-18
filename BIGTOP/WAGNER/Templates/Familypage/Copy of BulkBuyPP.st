<div id="tab4" class="tab_content tab4div">
  <br/><br/>
  <span class="prodivspan" >Contact us for Bulk Buy and Project Based Pricing for $TBT_FAMILY_NAME_BB$</span>
  <br/><br/>
  <div id="BulkBuyPP" style="display:block;">
    <table>
      <tr>
        <td valign="top" width="275px">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >Product Code*</span>
            <select  id="ddlprodcode" name="ddlprodcode" class="fbbsel"  onchange="getprodcodevalue();" onblur="Controlvalidate_BulkBuyPP('procode')" tabindex="1">
              $if(TBT_CHK_PRODCOUNT)$
              <option  selected="selected" value="Please Select Product" title="Please Select Product">Please Select Product</option>
              $endif$
              $TBW_DDL_VALUE$
            </select>
            
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text dnred" id="Errprocode_BBPP"> Select Product code </span>
          </div>
        </td>
        <td width="30px">
          
        </td>
        <td>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >Full Name*</span>

            <input type="text" id="txtFullname_BBPP"  class="cardinputAQ frmwidth"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')" tabindex="2"/>
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text dnred" id="Errfullname_BBPP"> Enter Full Name </span>

          </div>
        </td>
      </tr>
      <tr>
        <td valign="top" width="275px">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >QTY Required *</span>

            <input type="text" id="txtQTY"  class="cardinputAQ frmwidth" tabindex="3"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('qty')"   onkeypress="return validateNumber(event);"/>
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text dnred" id="ErrQTY"> Enter QTY </span>

          </div>
        </td>
        <td width="30px">

        </td>
        <td>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >Email*</span>
            <input type="text" id="txtEmail"  class="cardinputAQ frmwidth" tabindex="4"  maxlength="50"  onblur="Controlvalidate_BulkBuyPP('email')" />
          </div>
          <div class="form-col-2-81">
            <span class="error-text dnred" id="erremailadd_BBPP"> Enter Email Address </span>
            <span class="error-text dnred" id="errvalidmail_BBPP">Enter Valid Email </span>

          </div>
        </td>
      </tr>
      <tr>
        <td valign="top" width="275px">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >Delivery Time *</span>

            <input type="text" id="txtdeliverytime"  class="cardinputAQ frmwidth" tabindex="5"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('deltime')" />
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text dnred" id="Errdeliverytime">Enter Delivery Time </span>
          </div>
        </td>
        <td width="30px">

        </td>
        <td>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >Phone*</span>

            <input type="text" id="txtPhone_BBPP"  class="cardinputAQ bbppinp" tabindex="6"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('phone');" onkeypress="return validateNumber(event);"  />
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text dnred" id="Errphone_BBPP">Enter Phone Number </span>
          </div>
        </td>
      </tr>
      <tr>
        <td valign="top" width="275px" colspan="3">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >Target Price</span>

            <input type="text" id="txttargetprice"  class="cardinputAQ frmwidth"  tabindex="7" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')" />
          </div>
        </td>
      </tr>
      <tr>
        <td valign="top" width="275px" colspan="3">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >Notes / Addtional Info.</span>

            <textarea id="txtnotesadditionalinfo" cols="34" tabindex="8"  class="textarea2 prota"   rows="10" maxlength="600"
                                                onblur="Controlvalidate_BulkBuyPP('notes')" onkeypress="textCounter_BulkBuyPP(this,this.form.counter_BBPP,600);" ></textarea>
            <input class="cardinputAQ inbb" type="text" onblur="textCounter_BulkBuyPP(this.form.counter_BBPP,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter_BBPP"  />
              <span class="crspan">Chars Remaining </span>
          </div>
          <div class="form-col-2-81">
            <span class="error-text dnred" id="errnotes"></span>


          </div>
          <div class="cl"></div>
          <div class="form-col-2-81 frmwidth">
            <span class="prodivspan" >Code*</span>

            <div class="frmdivcode">

              <input type="text" id="txtCaptchCode_BBPP"  class="cardinputAQ wd50" tabindex="9"  maxlength="4"  onblur="Controlvalidate_BulkBuyPP('capcode');" onkeypress="return validateNumber(event);"   />
              <img src=""  width="120" height="40" alt="captcha" id="captcha_image_BBPP"/>

            </div>
            <div class="cl ht12"></div>
            <div class="form-col-2-81 frmwidth">
              <span class="error-text dnred" id="errCaptchCode_BBPP"> Enter the code </span>
              <span class="error-text dnred" id="errCaptchInvalid_BBPP"> Invalid code </span>
              <span class="error-text" id="errCaptchCode1_BBPP" style="display:none;"></span>
            </div>
            <div class="cl"></div>
          </div>
        </td>
      </tr>
      <tr>
        <td valign="top" width="275px" colspan="3">
          <a onclick="MailSend_BulkBuyPP()" class="Familysumit mlsnd" tabindex="10" >Submit</a>

          <a onclick="MailReset_BulkBuyPP()" class="Familysumit mlrst" tabindex="11" >Reset</a>
        </td>
      </tr>
    </table>
  </div>
  <div id="BulkBuyPPSubmit" class="bbdiv">
    <div class="bbdivinn">
      <img src="$CDN$images/tick2.png"  class="fbbimg" alt=""/>
      <span  >
        Thanks for your Bulk Buy enquiry. Our Customer Sales Team will be in contact with you Shortly.</span>
      </div>

  </div>

</div>