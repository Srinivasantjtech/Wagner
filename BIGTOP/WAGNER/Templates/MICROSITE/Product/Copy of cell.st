<div class="grid12">
  <div class="grid6">
    <div class="product-gallery">
      <div class="visible-desktop" id="slideshow-main">

        <ul>
          $TBT_MULTIIMAGES:{
         <li class="p1">
           <a href="#rg-gallery" rel="group" class="fancybox">            
              <img data-original="$CDNROOT$prodimages$it.LargeImage$" alt="$TBT_CODE$ $PROD_DESC_ALT$" class="lazy" width="60%" />
            </a>
          </li>}
          $
        </ul>
      
      </div>
      <div class="visible-desktop" id="slideshow-carousel">
        <div class=" jcarousel-skin-tango">
          <div class="jcarousel-container jcarousel-container-vertical" style="display: block;">
            <div class="jcarousel-prev jcarousel-prev-vertical jcarousel-prev-disabled jcarousel-prev-disabled-vertical" style="display: block;" disabled="disabled"></div>
            <div class="jcarousel-next jcarousel-next-vertical" style="display: block;"></div>
            <div class="jcarousel-clip jcarousel-clip-vertical">
              <ul class="jcarousel jcarousel-list jcarousel-list-vertical" id="carousel" style="height: 445px; top: 0px;">

                $TBT_MULTIIMAGES:{
              <li class="jcarousel-item jcarousel-item-vertical jcarousel-item-1 jcarousel-item-1-vertical" jcarouselindex="1">
                  <a rel="p1" href="#">
                  
                    <img data-original="$CDNROOT$prodimages$it.Thumpnail$" alt="$TBT_CODE$ $PROD_DESC_ALT$" class="lazy" style="max-width:50px;max-height:50px"/>
                  </a>
                </li>}
                $
                
              </ul>
            </div>
          </div>
        </div>
      </div>
      <div class="clear"></div>
    </div>
  </div>
  <div class="grid6">
    <div class="gal-prod-content">
      <div class="gal-prod-title">$TBT_FAMILY_NAME$ </div>
      <div class="gal-prod-desc-title">DETAILS:</div>
      <div class="gal-prod-desc">
        <!--<div class="item_txt">
        
          <span itemprop="description">
            <p align="left">
              $TBT_SHORT_DESCRIPTION$
            </p>
            <div id="div1" style="color: #222222;font-size: 11px;line-height: 14px;margin: 5px 0 0 0px;overflow:hidden;text-align:left;min-height:160px;">
              $if(TBT_PROD_DESC_SHOW)$
              <p>$TBT_DESCRIPTION$</p>
              $endif$
              <p>$TBT_MORE$</p>
            </div>
            <div id="div2" style="display:none;color: #222222;font-size: 11px;line-height: 14px;margin: 5px 0 0 0px;overflow:hidden;text-align:left;min-height:160px;">
              <p>$TBT_DESCALL$</p>
            </div>
            $if(TBT_MORE_SHOW)$
            <div id="preview" >
              <div id="smo" class="show_hide12" style="">
                <a class="redmoew"  >Read More! Show Full Description</a>
                <br/>
              </div>
              <div id="lmo" class="show_hide12" style="">
                <a class="redmoewless">Show Less Description</a>
                <br/>
              </div>
            </div>$endif$
            <a href="$TBT_REWRITEURL_NEW$/fl/" rel="/fl.aspx?fid=$TBT_PARENT_FAMILY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;Path=$TBT_EA_PATH$" class="btnfamily" style="display:$TBT_DISPLAY_TIP$;margin:69px 10px 0px 1px;">
              View All $TBT_FAMILY_PROD_COUNT$ Products in this Family
            </a>
          </span>
        </div>

        <div class="item_txt1">
          <div class="box5">
            <div style="width:110px; margin:auto">
              $if(TBT_ECOMENABLED)$$if(TBT_STOCK_STATUS_2)$
              <input name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" style="height: 21px; margin:7px 0; width: 40px; float:left;background-color:white;border: 1px solid #86B7CD;" size="5"  maxlength="6" type="text" onkeydown="return keyct(event)" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" />
              <a class="btnbuy2 button smallsiz btngreen costable" onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');">Buy</a>$endif$ $endif$
            </div>
            <div class="clear"></div>
            --><!--<div style="width:110px; margin:auto">
        $if(TBT_STOCK_STATUS_2)$
        <input name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$" style="height: 21px; margin:7px 0; width: 40px; float:left;background-color:white;border: 1px solid #86B7CD;" size="5" type="text" onkeydown="return keyct(event)" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$" />
        <a class="btnbuy2 button smallsiz btngreen costable" onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$','$TBT_PRODUCT_ID$');">Buy</a>$endif$ 
      </div>--><!--
            <div class="clear"></div>
          </div>
          <div style="border-top:none;" class="box5">
            <table style="margin:0" class="table table-striped  table-bordered table-condensed">
              <tbody>
                <tr class="success">
                  <td width="28%" >ORDER CODE</td>
                  <td colspan="2">STOCK STATUS</td>
                </tr>
                <tr>
                  <td>
                    <span id="ProductCode">$TBT_CODE$</span>
                  </td>
                  <td colspan="2">$TBT_STOCK_STATUS$$TBT_STOCK_STATUS_3$$TBT_STOCK_STATUS_1$</td>
                </tr>
                $TBT_STOCK_STATUS_ETA$
                <tr class="success">
                  <td>QTY</td>
                  <td width="38%">Cost INC GST</td>
                  <td width="34%">Cost EX GST</td>
                </tr>
                <tr>$TBT_PRICE_TABLE$</tr>
              </tbody>
            </table>
          </div>
        </div>-->
      </div>
      <input type="button" value="See Products Below.." id="click" class="btn-see-prod"/>
          </div>
  </div>
  <div class="cl"></div>
</div>
<!--<div class="cd-tabs grid12 is-ended">
  <nav>
    <ul class="cd-tabs-navigation">
      <li>
        <a href="#0" class="selected" data-content="id1">Product Specs</a>
      </li>
      <li>
        <a href="#0" data-content="id2">Ask a Question</a>
      </li>
      <li>
        <a href="#0" data-content="id3">Bulk Buy / Project Pricing</a>
      </li>$if(TBT_DOWNLOAD)$
      <li>
        <a href="#0" data-content="id4">Downloads</a>
      </li>$endif$
    </ul>
    <div class="cl"></div>
  </nav>
  <ul id="movetotable" class="cd-tabs-content">
    <li class="selected" data-content="id1">
      <div class="mar-top10 prod-info-table">
        <table width="100%" cellspacing="5" cellpadding="5" class="prod-item-tbl">
          <tbody>
            $TBT_PRODDETAILS:{<tr>
              <td width="30%" class="tdfirst">$it.AttributeName$</td>
              <td width="70%">$it.SpecValue$</td>
            </tr>}$
           
          </tbody>
        </table>
      </div>
    </li>
    <li data-content="id2">
      <div class="mar-top10 prod-info-table">
      
            <div class="grid4">
                <div>Full Name*</div>
                <div>
                <input type="text" id="txtFullname"  class="prod-form"    maxlength="30"  onblur="Controlvalidate('fn')" />
                  <span class="error-text" id="Errfullname" style="display:none;color: Red;"> Enter Full Name </span>
                </div>


              <div>Email*</div>

              <div>
                <input type="text" id="txtEmailAdd" class="prod-form"   maxlength="50"  onblur="Controlvalidate('ea')" />
                <span class="error-text" id="erremailadd" style="display:none;color: Red;"> Enter Email Address </span>
                <span class="error-text" id="errvalidmail" style="display:none;color: Red;">Enter Valid Email </span>
              </div>


              <div>Phone*</div>
              <div>
                <input type="text" id="txtPhone"   class="prod-form"   maxlength="30"  onblur="Controlvalidate('p');" onkeypress="return validateNumber(event);"  />
                <span class="error-text" id="Errphone" style="display:none;color: Red;">Enter Phone Number </span>
              </div>

              <div>Code*</div>
                <div>
                  <input type="text" id="txtCaptchCode"  class="cardinputAQ" style="width:50px;"   maxlength="4"  onblur="Controlvalidate('cc');" onkeypress="return validateNumber(event);"   />
                  <img src="$CAPTCHA_IMAGE$"  width="120" height="40"  />
                  <span class="error-text" id="errCaptchCode" style="display:none;color: Red;"> Enter the code </span>
                  <span class="error-text" id="errCaptchInvalid" style="display:none;color: Red;"> Invalid code </span>
                  <span class="error-text" id="errCaptchCode1" style="display:none;">$CC_CODE$</span>
                </div>

             
           
              <div >
                <a onclick="MailSend()" class="subminbtn" style="width:87px;height:27px; ">Submit</a>
              </div>
              <div >
                <a onclick="MailReset()" class="resetbtn" style="width:68px;height:27px; ">Reset</a>
              </div>
            </div>
            <div class="grid8 mgn">
              <div> Question* </div>


              <div>
                <textarea id="txtQuestionx" cols="34"   class="prod-txtarea"   rows="10" maxlength="600" style="box-shadow: 0 0 0px #CCCCCC; border:1px solid #b2b2b2;"
                                                      onblur="Controlvalidate('q')" onkeypress="textCounter(this,this.form.counter,600);" ></textarea>
                <span class="error-text" id="errquestion" style="display:none;color: Red;">Enter The Question </span>
               </div>
              <div class="cl"></div>
              <div>
                <input class="cardinputAQ" type="text" onblur="textCounter(this.form.counter,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter" style="width:35px; color:#B2B2B2;height:10px;"/>
                  <span style=" color: #7f7f7f;">Chars Remaining </span>
                </div>
              
               


              
            </div>
            <div class="cl"></div>
        
      </div>
      <div class="cl"></div>
    </li>
    <li data-content="id3">
      <div class="mar-top10 prod-info-table">
        <div id="tab4" class="tab_content" style="text-align: left;margin-left: 6px;">
          <br/>
          <br/>
          <br/>
          <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Contact us for Bulk Buy and Project Based Pricing for $TBT_FAMILY_NAME$</span>
          <br/>
          <br/>
          <div id="BulkBuyPP" style="display:block;">
            <table>
              <tr>
                <td valign="top" width="275px">
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Product Code</span>

                    <input type="text" id="txtproductcode"  class="cardinputAQ" style="width:248px;"  readonly="true" value="$TBT_CODE$"  maxlength="30"   />
                    --><!--<select  id="ddlprodcode" name="ddlprodcode" style="font-size: 12px;height: 30px;padding: 4px;width: 257px;border: 1px solid #b2b2b2;"  onchange="getprodcodevalue();">
                              $TBW_DDL_VALUE$
                            </select>--><!--

                  </div>
                </td>
                <td width="30px">

                </td>
                <td>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Full Name*</span>

                    <input type="text" id="txtFullname_BBPP"  class="cardinputAQ" style="width:248px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')" />
                  </div>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="Errfullname_BBPP" style="display:none;color: Red;"> Enter Full Name </span>

                  </div>
                </td>
              </tr>
              <tr>
                <td valign="top" width="275px">
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >QTY Required *</span>

                    <input type="text" id="txtQTY"  class="cardinputAQ" style="width:248px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('qty')"   onkeypress="return validateNumber(event);"/>
                  </div>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="ErrQTY" style="display:none;color: Red;"> Enter QTY </span>

                  </div>
                </td>
                <td width="30px">

                </td>
                <td>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Email*</span>
                    <input type="text" id="txtEmail"  class="cardinputAQ" style="width:248px;"   maxlength="50"  onblur="Controlvalidate_BulkBuyPP('email')" />
                  </div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="erremailadd_BBPP" style="display:none;color: Red;"> Enter Email Address </span>
                    <span class="error-text" id="errvalidmail_BBPP" style="display:none;color: Red;">Enter Valid Email </span>

                  </div>
                </td>
              </tr>
              <tr>
                <td valign="top" width="275px">
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Delivery Time *</span>

                    <input type="text" id="txtdeliverytime"  class="cardinputAQ" style="width:248px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('deltime')" />
                  </div>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="Errdeliverytime" style="display:none;color: Red;">Enter Delivery Time </span>
                  </div>
                </td>
                <td width="30px">

                </td>
                <td>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Phone*</span>

                    <input type="text" id="txtPhone_BBPP"  class="cardinputAQ" style="width:248px;height:12px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('phone');" onkeypress="return validateNumber(event);"  />
                  </div>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="Errphone_BBPP" style="display:none;color: Red;">Enter Phone Number </span>
                  </div>
                </td>
              </tr>
              <tr>
                <td valign="top" width="275px" colspan="3">
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Target Price</span>

                    <input type="text" id="txttargetprice"  class="cardinputAQ" style="width:248px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')"  />
                  </div>
                </td>
              </tr>
              <tr>
                <td valign="top" width="275px" colspan="3">
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Notes / Addtional Info.</span>

                    <textarea id="txtnotesadditionalinfo" cols="34"   class="textarea2"   rows="10" maxlength="600" style="box-shadow: 0 0 0px #CCCCCC; border:1px solid #b2b2b2;height:100px;width:435px;"
                                                        onblur="Controlvalidate_BulkBuyPP('notes')" onkeypress="textCounter_BulkBuyPP(this,this.form.counter_BBPP,600);" ></textarea>
                    <input class="cardinputAQ" type="text" onblur="textCounter_BulkBuyPP(this.form.counter_BBPP,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter_BBPP" style="width:35px; color:#B2B2B2;height:12px;" />
                    <span style=" color: #7f7f7f;font-size: 11px;font-weight: bold;">Chars Remaining </span>
                  </div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="errnotes" style="display:none;color: Red;">Enter The Notes / Addtional Info.</span>


                  </div>
                  <div class="cl"></div>
                  <div class="form-col-2-81" style="width:248px;">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Code*</span>
                    <div style="display: flex;height: 29px;">
                      <input type="text" id="txtCaptchCode_BBPP"  class="cardinputAQ" style="width:50px;height:12px;"   maxlength="4"  onblur="Controlvalidate_BulkBuyPP('capcode');" onkeypress="return validateNumber(event);"   />
                      <img src="$CAPTCHA_IMAGE$"  width="120" height="40"  />
                    </div>

                  </div>
                  <div class="cl" style="height:12px;"></div>
                  <div class="form-col-2-81" style="width:248px;">
                    <span class="error-text" id="errCaptchCode_BBPP" style="display:none;color: Red;"> Enter the code </span>
                    <span class="error-text" id="errCaptchInvalid_BBPP" style="display:none;color: Red;"> Invalid code </span>
                    <span class="error-text" id="errCaptchCode1_BBPP" style="display:none;">$CC_CODE$</span>
                  </div>
                  <div class="cl"></div>
                </td>
              </tr>
              <tr>
                <td valign="top" width="275px" colspan="3">
                  <a onclick="MailSend_BulkBuyPP()" Class="Familysumit" style="width:87px;height:27px; ">Submit</a>

                  <a onclick="MailReset_BulkBuyPP()" Class="Familysumit" style="width:68px;height:27px; ">Reset</a>
                </td>
              </tr>
            </table>
          </div>
          <div id="BulkBuyPPSubmit" style="text-align:center;display:none; height: 160px; padding-bottom: 135px;padding-top: 33px;" >
            <div style="background-color: #e6f7ee;border: thin solid #85d6ad;border-radius: 6px;color: #339966;font-size: 12px;font-weight: bold;padding: 14px;width: 663px;">
              <img src="/images/tick2.png"  style="margin-left: -20px;margin-right: 10px;vertical-align: middle;" />
              <span  >
                Thanks for your Bulk Buy enquiry. Our Customer Sales Team will be in contact with you Shortly.
              </span>
            </div>

          </div>

        </div>
        <div class="cl"></div>
      </div>
      <div class="cl"></div>
    </li>
    <li data-content="id4">
      $TBT_DOWNLOAD_DATA$
    </li>
  </ul>
</div>
<div class="cl"></div>
<div class="accord-hold">
  <ul class="accordion_example smk_accordion acc_with_icon">
    --><!-- Section 1 --><!--
    <li class="accordion_in">
      <div class="acc_head">
        <div class="acc_icon_expand"></div>Product Specs
      </div>
      --><!-- Head --><!--
      <div class="mar-top10 prod-info-table acc_content" style="display: none;">
    
          <table width="100%" cellspacing="5" cellpadding="5" class="prod-item-tbl">
            <tbody>
              $TBT_PRODDETAILS:{<tr>
                <td width="30%" class="tdfirst">$it.AttributeName$</td>
                <td width="70%">$it.SpecValue$</td>
              </tr>}$

            </tbody>
          </table>
  
      </div>
    </li>
    --><!-- Section 2 --><!--
    <li class="accordion_in">
      <div class="acc_head">
        <div class="acc_icon_expand"></div>Ask a Question
      </div>
      --><!-- Head --><!--
      <div class="mar-top10 prod-info-table acc_content" style="display: none;">


          <div class="grid4">
            <div>Full Name*</div>
            <div>
              <input type="text" id="txtFullname"  class="prod-form"    maxlength="30"  onblur="Controlvalidate('fn')" />
              <span class="error-text" id="Errfullname" style="display:none;color: Red;"> Enter Full Name </span>
            </div>


            <div>Email*</div>

            <div>
              <input type="text" id="txtEmailAdd" class="prod-form"   maxlength="50"  onblur="Controlvalidate('ea')" />
              <span class="error-text" id="erremailadd" style="display:none;color: Red;"> Enter Email Address </span>
              <span class="error-text" id="errvalidmail" style="display:none;color: Red;">Enter Valid Email </span>
            </div>


            <div>Phone*</div>
            <div>
              <input type="text" id="txtPhone"   class="prod-form"   maxlength="30"  onblur="Controlvalidate('p');" onkeypress="return validateNumber(event);"  />
              <span class="error-text" id="Errphone" style="display:none;color: Red;">Enter Phone Number </span>
            </div>

            <div>Code*</div>
            <div>
              <input type="text" id="txtCaptchCode"  class="cardinputAQ" style="width:50px;"   maxlength="4"  onblur="Controlvalidate('cc');" onkeypress="return validateNumber(event);"   />
              <img src="$CAPTCHA_IMAGE$"  width="120" height="40"  />
              <span class="error-text" id="errCaptchCode" style="display:none;color: Red;"> Enter the code </span>
              <span class="error-text" id="errCaptchInvalid" style="display:none;color: Red;"> Invalid code </span>
              <span class="error-text" id="errCaptchCode1" style="display:none;">$CC_CODE$</span>
            </div>



            <div >
              <a onclick="MailSend()" class="subminbtn" style="width:87px;height:27px; ">Submit</a>
            </div>
            <div >
              <a onclick="MailReset()" class="resetbtn" style="width:68px;height:27px; ">Reset</a>
            </div>
          </div>
          <div class="grid8 mgn">
            <div> Question* </div>


            <div>
              <textarea id="txtQuestionx" cols="34"   class="prod-txtarea"   rows="10" maxlength="600" style="box-shadow: 0 0 0px #CCCCCC; border:1px solid #b2b2b2;"
                                                    onblur="Controlvalidate('q')" onkeypress="textCounter(this,this.form.counter,600);" ></textarea>
              <span class="error-text" id="errquestion" style="display:none;color: Red;">Enter The Question </span>
            </div>
            <div class="cl"></div>
            <div>
              <input class="cardinputAQ" type="text" onblur="textCounter(this.form.counter,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter" style="width:35px; color:#B2B2B2;height:10px;"/>
              <span style=" color: #7f7f7f;">Chars Remaining </span>
            </div>





          </div>
          <div class="cl"></div>


        <div class="cl"></div>
      </div>
    </li>
    --><!-- Section 3 --><!--
    <li class="accordion_in">
      <div class="acc_head">
        <div class="acc_icon_expand"></div>Bulk Buy / Project Pricing
      </div>
      --><!-- Head --><!--
      <div class="mar-top10 prod-info-table acc_content" style="display: none;">
        <div id="tab4" class="tab_content" style="text-align: left;margin-left: 6px;">
          <br/>
          <br/>
          <br/>
          <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Contact us for Bulk Buy and Project Based Pricing for $TBT_FAMILY_NAME$</span>
          <br/>
          <br/>
          <div id="BulkBuyPP" style="display:block;">
            <table>
              <tr>
                <td valign="top" width="275px">
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Product Code</span>

                    <input type="text" id="txtproductcode"  class="cardinputAQ" style="width:248px;"  readonly="true" value="$TBT_CODE$"  maxlength="30"   />
                    --><!--<select  id="ddlprodcode" name="ddlprodcode" style="font-size: 12px;height: 30px;padding: 4px;width: 257px;border: 1px solid #b2b2b2;"  onchange="getprodcodevalue();">
                              $TBW_DDL_VALUE$
                            </select>--><!--

                  </div>
                </td>
                <td width="30px">

                </td>
                <td>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Full Name*</span>

                    <input type="text" id="txtFullname_BBPP"  class="cardinputAQ" style="width:248px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')" />
                  </div>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="Errfullname_BBPP" style="display:none;color: Red;"> Enter Full Name </span>

                  </div>
                </td>
              </tr>
              <tr>
                <td valign="top" width="275px">
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >QTY Required *</span>

                    <input type="text" id="txtQTY"  class="cardinputAQ" style="width:248px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('qty')"   onkeypress="return validateNumber(event);"/>
                  </div>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="ErrQTY" style="display:none;color: Red;"> Enter QTY </span>

                  </div>
                </td>
                <td width="30px">

                </td>
                <td>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Email*</span>
                    <input type="text" id="txtEmail"  class="cardinputAQ" style="width:248px;"   maxlength="50"  onblur="Controlvalidate_BulkBuyPP('email')" />
                  </div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="erremailadd_BBPP" style="display:none;color: Red;"> Enter Email Address </span>
                    <span class="error-text" id="errvalidmail_BBPP" style="display:none;color: Red;">Enter Valid Email </span>

                  </div>
                </td>
              </tr>
              <tr>
                <td valign="top" width="275px">
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Delivery Time *</span>

                    <input type="text" id="txtdeliverytime"  class="cardinputAQ" style="width:248px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('deltime')" />
                  </div>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="Errdeliverytime" style="display:none;color: Red;">Enter Delivery Time </span>
                  </div>
                </td>
                <td width="30px">

                </td>
                <td>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Phone*</span>

                    <input type="text" id="txtPhone_BBPP"  class="cardinputAQ" style="width:248px;height:12px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('phone');" onkeypress="return validateNumber(event);"  />
                  </div>
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="Errphone_BBPP" style="display:none;color: Red;">Enter Phone Number </span>
                  </div>
                </td>
              </tr>
              <tr>
                <td valign="top" width="275px" colspan="3">
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Target Price</span>

                    <input type="text" id="txttargetprice"  class="cardinputAQ" style="width:248px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')"  />
                  </div>
                </td>
              </tr>
              <tr>
                <td valign="top" width="275px" colspan="3">
                  <div class="cl"></div>
                  <div class="form-col-2-81">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Notes / Addtional Info.</span>

                    <textarea id="txtnotesadditionalinfo" cols="34"   class="textarea2"   rows="10" maxlength="600" style="box-shadow: 0 0 0px #CCCCCC; border:1px solid #b2b2b2;height:100px;width:435px;"
                                                        onblur="Controlvalidate_BulkBuyPP('notes')" onkeypress="textCounter_BulkBuyPP(this,this.form.counter_BBPP,600);" ></textarea>
                    <input class="cardinputAQ" type="text" onblur="textCounter_BulkBuyPP(this.form.counter_BBPP,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter_BBPP" style="width:35px; color:#B2B2B2;height:12px;" />
                    <span style=" color: #7f7f7f;font-size: 11px;font-weight: bold;">Chars Remaining </span>
                  </div>
                  <div class="form-col-2-81">
                    <span class="error-text" id="errnotes" style="display:none;color: Red;">Enter The Notes / Addtional Info.</span>


                  </div>
                  <div class="cl"></div>
                  <div class="form-col-2-81" style="width:248px;">
                    <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Code*</span>
                    <div style="display: flex;height: 29px;">
                      <input type="text" id="txtCaptchCode_BBPP"  class="cardinputAQ" style="width:50px;height:12px;"   maxlength="4"  onblur="Controlvalidate_BulkBuyPP('capcode');" onkeypress="return validateNumber(event);"   />
                      <img src="$CAPTCHA_IMAGE$"  width="120" height="40"  />
                    </div>

                  </div>
                  <div class="cl" style="height:12px;"></div>
                  <div class="form-col-2-81" style="width:248px;">
                    <span class="error-text" id="errCaptchCode_BBPP" style="display:none;color: Red;"> Enter the code </span>
                    <span class="error-text" id="errCaptchInvalid_BBPP" style="display:none;color: Red;"> Invalid code </span>
                    <span class="error-text" id="errCaptchCode1_BBPP" style="display:none;">$CC_CODE$</span>
                  </div>
                  <div class="cl"></div>
                </td>
              </tr>
              <tr>
                <td valign="top" width="275px" colspan="3">
                  <a onclick="MailSend_BulkBuyPP()" Class="Familysumit" style="width:87px;height:27px; ">Submit</a>

                  <a onclick="MailReset_BulkBuyPP()" Class="Familysumit" style="width:68px;height:27px; ">Reset</a>
                </td>
              </tr>
            </table>
          </div>
          <div id="BulkBuyPPSubmit" style="text-align:center;display:none; height: 160px; padding-bottom: 135px;padding-top: 33px;" >
            <div style="background-color: #e6f7ee;border: thin solid #85d6ad;border-radius: 6px;color: #339966;font-size: 12px;font-weight: bold;padding: 14px;width: 663px;">
              <img src="/images/tick2.png"  style="margin-left: -20px;margin-right: 10px;vertical-align: middle;" />
              <span  >
                Thanks for your Bulk Buy enquiry. Our Customer Sales Team will be in contact with you Shortly.
              </span>
            </div>

          </div>

        </div>
        <div class="cl"></div>
      </div>
    </li>
    --><!-- Section 4 --><!--
    <li class="accordion_in">
      <div class="acc_head">
        <div class="acc_icon_expand"></div>Downloads
      </div>
      --><!-- Head --><!--
      <div class="mar-top10 prod-info-table acc_content" style="display: none;">
        $TBT_DOWNLOAD_DATA$
      </div>
    </li>
  </ul>
</div>
<div class="cl"></div>-->



