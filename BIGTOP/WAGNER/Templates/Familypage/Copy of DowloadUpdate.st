
<div id="tab2" class="tab_content tab4div">
  <br/><br/>
  <span class="prodivspan" >No downloads currently available for $TBT_FAMILY_NAME_DU$</span>
  <br/><br/>
   <span class="prodivspan" >Please contact us with your download requirements,e.g. User Manual, Data Sheet, etc and will get back to you.</span>
  <br/>
  <br/>
  <div id="DownloadUpdate" style="display:block;">
    <table>
      <tr>
        <td valign="top" width="275px" >
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >Full Name*</span>
            <input type="text" id="txtFullname_DU"  class="cardinputAQ frmwidth" tabindex="1"   maxlength="30"  onblur="Controlvalidate_DU('fullname_du')" />
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text dnred" id="Errfullname_DU"> Enter Full Name </span>

          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >Email*</span>
            <input type="text" id="txtEmail_DU"  class="cardinputAQ frmwidth"  tabindex="2"  maxlength="50"  onblur="Controlvalidate_DU('email_du')" />
          </div>
          <div class="form-col-2-81">
            <span class="error-text dnred" id="erremailadd_DU"> Enter Email Address </span>
            <span class="error-text dnred" id="errvalidmail_DU">Enter Valid Email </span>

          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >Phone*</span>

            <input type="text" id="txtPhone_DU"  class="cardinputAQ bbppinp" tabindex="3"   maxlength="30"  onblur="Controlvalidate_DU('phone_du');" onkeypress="return validateNumber(event);"  />
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text dnred" id="Errphone_DU">Enter Phone Number </span>
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81 frmwidth">
            <span class="prodivspan" >Code*</span>

            <div class="frmdivcode">

              <input type="text" id="txtCaptchCode_DU"  class="cardinputAQ wd50"  maxlength="4" tabindex="4"  onblur="Controlvalidate_DU('capcode_du');" onkeypress="return validateNumber(event);"   />
              <img src=""  width="120" height="40" alt="captcha" id="captcha_image_DU" />
            </div>

          </div>
          <div class="cl ht12"></div>
          <div class="form-col-2-81 frmwidth">
            <span class="error-text dnred" id="errCaptchCode_DU"> Enter the code </span>
            <span class="error-text dnred" id="errCaptchInvalid_DU"> Invalid code </span>
             <span class="error-text" id="errCaptchCode1_DU" style="display:none;"></span>
          </div>
          <div class="cl ht26"></div>
        </td>
        <td valign="top">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="prodivspan" >Download Required / Comments*</span>

            <textarea id="txtdownloadre" cols="34"   class="textarea2 duta" tabindex="5"   rows="10" maxlength="600" 
                                                onblur="Controlvalidate_DU('notes_du')" onkeypress="textCounter_DU(this,this.form.counter_DU,600);" ></textarea>
            <input class="cardinputAQ inbb" type="text" onblur="textCounter_DU(this.form.counter_DU,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter_DU"  />
            <span class="crspan">Chars Remaining </span>
          </div>
          <div class="form-col-2-81">
            <span class="error-text dnred" id="errdownloadre">Download Required / Comments.</span>


          </div>
        </td>     
      </tr>   
      <tr>
        <td valign="top" width="275px" height="49px" colspan="2">
          <a onclick="MailSend_DU()" class="Familysumit btndusnd" tabindex="6" >Submit</a>

          <a onclick="MailReset_DU()" class="Familysumit btndu" tabindex="7" >Reset</a>
        </td>
      </tr>
    </table>
  </div>
  <div id="DUSubmit" class="DUSubmitdiv" >
    <div class="DUdiv">
      <img src="$CDN$images/tick2.png"  class="duimg" alt=""/>
      <span>
        Thanks for your download enquiry. Our Customer Sales Team will be in contact with you Shortly.</span>
      </div>

  </div>
 
</div>
