
<div class="box1 fbox1">
  
  $Generateparentfamilyhtml$
<!--<div class="target label label-info">target element</div>-->
  <!--<a  class="avpc" href="$TBT_CT_HREF$" style="margin-right:5px;float:right!important;"></a>-->
  <div  class="button-group fbg">
    <a href="$TBT_CT_HREF$" class="vproduct-btn">
      View Product Category<br />
      <span>See All products in the same Product category</span>
    </a>
  </div>
  <!--<div style="height:6px;" class="clear"></div>-->
  <div class="tabbable">
    <ul class="tabs">
      <li>
        <a href="#tab1" >Products</a>
        
      </li>
      <li  >
        <a href="#tab3" >Ask a Question</a>
      </li>
      <li>
        <a href="#tab4">Bulk Buy / Project Pricing</a>
      </li>
      <li style="display:$ST_Family_Download$" >
        <a href="#tab2" >Downloads</a>
      </li>
      <li>
        <a href="#tab5">Shipping Info</a>
      </li>
    </ul>
    <div class="clear"></div>
    <div class="tab-content">
      <div class="fpscroll">
       
        <div id="tab1" class="tab_content" >
          <p style="color:#222222;">Products for</p><h2 class="fh2">$TBT_FAMILY_NAME_PSPECS$</h2>
          $ST_Familypage$
        </div>
      </div>
      $DownloadST$
      <div id="tab3" class="tab_content" >
      <div id="divaskquestion" style="display:block;">
        <table >
          <tr>
            <td valign="top" width="275px;" height="270px">
              <div class="cl"></div>

              <div class="form-col-2-81">
                <span class="prodivspan" >Full Name*</span>

                <input type="text" id="txtFullname"  class="cardinputAQ frmwidth"  maxlength="30"  onblur="Controlvalidate('fn')" />

              </div>
              <div class="cl"></div>
              <div class="form-col-2-81">
                <span class="error-text dnred" id="Errfullname"> Enter Full Name </span>

              </div>
              <div class="cl"></div>
              <div class="form-col-2-81">
                <span class="prodivspan" >Email*</span>


                <input type="text" id="txtEmailAdd"  class="cardinputAQ frmwidth"   maxlength="50"  onblur="Controlvalidate('ea')" />
              </div>
              <div class="form-col-2-81">
                <span class="error-text dnred" id="erremailadd" > Enter Email Address </span>
                <span class="error-text dnred" id="errvalidmail">Enter Valid Email </span>

              </div>
              <div class="cl"></div>
              <div class="form-col-2-81">
                <span class="prodivspan" >Phone*</span>

                <input type="text" id="txtPhone"  class="cardinputAQ frmwidth"  maxlength="30"  onblur="Controlvalidate('p');" onkeypress="return validateNumber(event);"  />
              </div>
              <div class="cl"></div>
              <div class="form-col-2-81">
                <span class="error-text dnred" id="Errphone">Enter Phone Number </span>
              </div>
              <div class="cl"></div>              
              <div class="form-col-2-81 frmwidth">
                <span class="prodivspan" >Code*</span>
                <div>
                <input type="text" id="txtCaptchCode"  class="cardinputAQ wd50"  maxlength="4"  onblur="Controlvalidate('cc');" onkeypress="return validateNumber(event);"   />
                  <img src=""  width="120" height="40" alt="captcha" id="captcha_image"/>
                </div>
               
              </div>
              <div class="form-col-2-81 frmwidth">
                <span class="error-text dnred" id="errCaptchCode" > Enter the code </span>
                <span class="error-text dnred" id="errCaptchInvalid"> Invalid code </span>
                <span class="error-text" id="errCaptchCode1" style="display:none;"></span>
              </div>
              <div class="cl"></div>
              <div >
                <a onclick="MailSend()" class="Familysumit mlsnd">Submit</a>

                <a onclick="MailReset()" class="Familysumit mlrst">Reset</a>

              </div>
            </td>
            <td valign="top">
              <div class="cl"></div>
              <div class="form-col-2-81">
                <span class="prodivspan" >Question*</span>


    
                <textarea id="txtQuestionx" cols="34"   class="textarea2 prota1"   rows="10" maxlength="600" 
                                                      onblur="Controlvalidate('q')" onkeypress="textCounter(this,this.form.counter,600);" ></textarea>
                <input class="cardinputAQ incarinn" type="text" onblur="textCounter(this.form.counter,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter" />
                  <span class="spcolbbpp">Chars Remaining </span>
                </div>
              <div class="form-col-2-81">
                <span class="error-text dnred" id="errquestion">Enter The Question </span>


              </div>
            </td>
          </tr>
        </table>
      </div>
      <div id="divAskQuestionSubmit" class="divaq" >
        <div class="divaqinn">
          <img src="$CDN$images/tick2.png" class="divaqimg" alt=""/>
          <span>
            Thanks for your enquiry. Our Customer Sales Team will be in contact with you Shortly.</span>
          </div>

      </div>
    </div>
      $ST_BulkBuyPP$
      <div id="tab5" class="tab_content tcmar">
      <p class="ftab5">&#36;12.95 Australia Wide Flat Rate Shipping*</p>
      <p class="pptab5">
        No matter where you are located in Australia we offer flat rate shipping.<br />Some conditions apply,
        please see our <a class="psop" target="_blank" href="/Termsandconditions.aspx">Terms and Conditions </a>page.
      </p>
      <p class="ftab5">Store Pick Up from Sydney Showroom</p>
      <p class="pptab5">
        Goods can be picked from our premises from 8.30AM to 5.00PM Monday to Friday and from 9AM till 4PM on<br />Saturday. In order to avoid delays it is recommended that you place your order 2 hours prior to pick up.<br />please see our <a class="psop" target="_blank" href="/Termsandconditions.aspx">Terms and Conditions </a> page for more details.
      </p>

      <p class="ftab5">International Order Shipping Available</p>
      <p class="pptab5">
        We ship internationally. Once your order is received we will advise shipping cost to you by email. <br /> Your order will be updated with the freight cost.
      </p>
    </div>
    </div>
  </div >
</div>
