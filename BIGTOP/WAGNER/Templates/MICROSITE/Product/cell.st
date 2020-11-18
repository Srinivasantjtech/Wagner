<div class="container margin_top" itemscope="" itemtype="http://schema.org/Product">
  <div class="row">
    <div class="col-lg-4 col-sm-6 margin_top">
      <div class="text-center margin_bottom_15">
        $if(TBT_NOIMAGE)$
        <img data-cloudzoom="zoomImage:&quot;$CDNROOT$prodimages$TBT_TWEB_IMAGE12_LARGE$&quot;,
                     zoomSizeMode: &quot;image&quot;,
                     tintColor:&quot;#000&quot;,
                     tintOpacity:0.25,
                     captionPosition:&quot;bottom&quot;,
                     maxMagnification:4,
                     autoInside:750
                     " title="" itemprop="image" src="$CDNROOT$prodimages$TBT_TWEB_IMAGE12$" id="zoom1" alt="$TBT_FAMILY_NAME$" class="cloudzoom" style="width:256px;min-width: 100px;"/>
       $else$
          <img src="$CDNROOT$prodimages$TBT_TWEB_IMAGE12$" style="max-width:256px;max-height:256px;" alt="$TBT_FAMILY_NAME$"></img>
       $endif$
      </div>
      <span class="gen-spacer"></span>
      <div class="text-center">
        $TBT_MULTIIMAGES:{
        <div class=" thumb_img">
          $if(TBT_NOIMAGE)$
          <a  href="$CDNROOT$prodimages$it.LargeImage$" class="thumb-link">
          <!--href="/prodimages$it.LargeImage$"-->
          <!--<img src="/prodimages$it.Thumpnail$" alt="$TBT_CODE$ $PROD_DESC_ALT$"  class="cloudzoom-gallery" width="50" data-cloudzoom="useZoom:"#zoom1=", image:"zoom="/prodimages$it.Thumpnail$",zoomImage:"/prodimages$it.LargeImage$"" />-->
            <img width="50" 
                 data-cloudzoom="useZoom:&quot;#zoom1&quot;,
                 image:&quot;$CDNROOT$prodimages$it.LargeImage$&quot;,
                 zoomImage:&quot;$CDNROOT$prodimages$it.LargeImage$&quot;" 
                 title="" alt="$TBT_FAMILY_NAME$" 
                 src="$CDNROOT$prodimages$it.Thumpnail$" class="cloudzoom-gallery"></img>
          <!--<img src="/prodimages$it.Thumpnail$" alt="$TBT_CODE$ $PROD_DESC_ALT$" class="cloudzoom-gallery" width="50"/>-->
        </a>
          $else$
          <!--<img width="50"  title=""  src="/prodimages$it.Thumpnail$" ></img>-->
          $endif$
        </div>
        
      }
        $
        <div class="clear"></div>
      </div>
      <span class="gen-spacer"></span>
    </div>
    <div >
    <div class="col-lg-8 col-sm-6">
      <h3 class="bolder blue_color_text font_size_16" itemprop="name"> $TBT_FAMILY_NAME$</h3> 
      <div class="divider_details" style="display:$TBT_DIS_BLK$"></div>
      <span itemprop="description">
        <p class="details_para margin_top">$TBT_SHORTDESCRIPTION$</p>
        <p class="details_para margin_top">$TBT_DESCALL$</p>
      </span>
      <div class="divider_details"></div>
      $if(TBT_REPLACED)$
      <div class="margin_top">
      <p class="pull-left width_size">Order Code </p>
      <span class="mar_left_30 mar_right_30"> : </span>
      <span class="font_15 bolder red_txt" id="ProductCode">$TBT_NIL_CODE$</span>
    </div>
      <div class="margin_top">
        <p class="pull-left width_size">Stock Status</p>
        <p class="mar_left_30 mar_right_30 pull-left"> : </p>
        <p class="font_15 mobile_size red_txt">
          <img class="margin_right" src="$CDN$images/micrositeimages/cross.png" alt=""/>


          $TBT_STOCK_STATUS$$TBT_STOCK_STATUS_3$$TBT_STOCK_STATUS_1$
          
          
        </p>               
        <div class="clear"></div>
      </div>
      $if(TBT_REPLACED_EXISTS)$
      <h3 class="bolder blue_color_text" style="font-size:18px !important"> Substituted Product Item</h3>
      <span itemprop="description">
        <h4>Notice</h4>
        <p class="details_para margin_top">
          This product is currently unavailable and has been replaced with the product below
        </p>
      </span>
      <a href="#">
        <span class="font_15 bolder green_clr" id="ProductCode">
          <a style="text-decoration:none;font-weight: bold;font-size: 14px;" class="green_clr" href="$TBT_REPLACED_URL$">$TBT_REPLACED_CODE$</a>
        </span>
      </a>
      <div class="margin_top padding_bottom15 border_bottom">

        <a style="text-decoration:none;" href="$TBT_REPLACED_URL$">
          <span class="font_18 bolder blue_color_text">  View Replacement Product </span>
        </a>
      </div>
      <!--<div class="margin_top">
        <span class="font_15 bolder">RECOMMENDED REPLACEMENT </span>
      </div>
      <div class="margin_top">
        <p class="pull-left width_size">Order Code </p>
        <span class="mar_left_30 mar_right_30"> : </span>
        <span id="ProductCode" class="font_15 bolder green_clr">$TBT_REPLACED_CODE$</span>

      </div>
      <div class="margin_top">

        <a href="$TBT_REPLACED_URL$" style="text-decoration:none;" >
          <span class="font_18 bolder blue_color_text">  View Replacement Product </span>
        </a>
      </div>-->
      $endif$
      
      $else$
      <div class="margin_top">
        <p class="pull-left width_size">Order Code </p>
        <span class="mar_left_30 mar_right_30"> : </span>
        <span class="font_15 bolder" id="ProductCode">$TBT_CODE$</span>
      </div>
      
      <div class="margin_top">

        <p class="pull-left width_size">Stock Status</p>
        <p class="mar_left_30 mar_right_30 pull-left"> : </p>
       
        
            $if(TBT_STKSTATUS)$
                    <p class="font_15 mobile_size green_clr">         
          <img class="margin_right" src="$CDN$images/micrositeimages/tick.png" alt=""/>$TBT_STOCK_STATUS$$TBT_STOCK_STATUS_3$$TBT_STOCK_STATUS_1$
                      
        </p>
            $else$
            <p class="font_15 mobile_size">
              <img class="margin_right" src="$CDN$images/micrositeimages/cross.png" alt=""/>
              <meta link="" itemprop="availability" HREF="$TBT_STOCK_HREF$"/>
              <a class="gray_40" href="#ETA_link" id="ETALink" >
                $TBT_STOCK_STATUS$$TBT_STOCK_STATUS_3$$TBT_STOCK_STATUS_1$</a>          
            </p>
            $endif$
        <div class="clear"></div>
      </div>
     
      <div class="margin_top">
        $TBT_STOCK_STATUS_ETA$
      </div>
      <!--  <div class="col-lg-8 clear  padding_left_right_mob margin_top">
        <table class="table table-bordered">
          <thead>
            <tr>
              <th class="text-center">QTY</th>
              <th class="text-center">Cost Inc GST</th>
              <th class="text-center">Cost EX GST</th>
            </tr>
          </thead>      
         $TBT_PRICE_TABLE$
        </table>
      </div> -->
      $if(TBT_ECOMENABLED)$$if(TBT_STOCK_STATUS_2)$
      <div class="margin_top clear col-lg-6 padding_left_zero" >
        <p class="pull-left width_size">Quantity </p>
        <span class="mar_left_30 mar_right_30"> : </span>
        <span class="font_15 bolder">
          <input type="text" name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" length="6" value="1" class="quantity" onkeyup="callproductqtykeypress(event)" style="text-align: center;"/>
        </span>
      </div>
      $endif$ $endif$
    
      <div class="col-lg-12 padding_left_zero " itemprop="offers" itemscope="" itemtype="http://schema.org/Offer">
        <meta itemprop="category" content="$PRODUCT_CATEGORY_NAME$" />
        <link itemprop="availability" href="$TBT_STOCK_HREF$"/>
        <div class="margin_top col-lg-6 padding_left_zero">
          <p class="pull-left width_size padd_zero">Price </p>
          <span class="mar_left_30 mar_right_30"> : </span>
          <span class="font_18 bolder blue_color_text">$TBT_COST$</span>
          <meta itemprop="price" content="$TBT_COST$"/>
            <meta itemprop="priceCurrency" content="AUD"/>
        </div>
        $if(TBT_ECOMENABLED)$$if(TBT_STOCK_STATUS_2)$
        <div class="margin_top col-lg-6 padding_left_zero mob_margin_Top">
          <!--<a class="green_bg padding_10 hover_a bolder" href="" onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');">Add to Cart</a>-->

          <input type="button" name="" value="Add to Cart" 
                  style="cursor: pointer;"
                 class="green_bg padding_10 hover_a bolder border_none white_color" 
                 onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');" />
        </div>
        <div class="fixed_bottom visible-xs clearfix">
          <div class="fixed_buy">
            <a onclick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');" href="#" class="" data-toggle="modal">BUY NOW</a>

          </div>
        </div>
        $endif$ $endif$
      </div>
      $endif$
    </div>

      <div class="col-lg-12 clear margin_top_20 mob_pad_top_40">
        <div  class="button-group" style="float:right;margin-bottom:-18px;">
          <a id="avrp" style="display:$TBT_DISPLAY_TIP$;" href="$TBT_REWRITEURL_NEW$/mfl/"  class="vrelated-btn">
            View Product Group<br />
            <span>See All products in the same Product Group</span>
          </a>
          <a href="$TBT_CT_HREF$" class="vproduct-btn">
            View Product Category<br />
            <span>See All products in the same Product category</span>
          </a>

        </div>
      </div>
      <div class="clear"></div>
      
    <div class="col-lg-12 clear margin_top_20 mob_pad_top_40">
      <!-- tabs -->
      <div class="tabbable mob_top_40">
        <ul class="nav nav-tabs">
          <li class="active col-lg-3-1 col-xs-12 col-sm-3 col-md-3 padding_left_right_mob tab_head">
            <a class="bolder" data-toggle="tab" href="#one">Product Specs</a>
          </li>
          <li class="col-lg-3-1 col-xs-12 col-sm-3 col-md-3 padding_left_right_mob tab_head">
            <a class="bolder" data-toggle="tab" href="#two" id="ETA_link">Ask A Questions</a>
          </li>
          <li class="col-lg-3-1  col-xs-12 col-sm-3 col-md-3 padding_left_right_mob tab_head">
            <a class="bolder" data-toggle="tab" href="#twee">Bulk Buy / Project Pricing </a>
          </li>
          $if(TBT_DOWNLOAD)$
          <li class="col-lg-3-1 col-xs-12 col-sm-3 col-md-3  padding_left_right_mob tab_head">
            <a class="bolder" data-toggle="tab" href="#four">Downloads</a>
          </li>
          $endif$
        <li class="col-lg-3-1 col-xs-12 col-md-3 col-sm-3 padding_left_right_mob tab_head">
          <a href="#five" data-toggle="tab" class="bolder">Shipping Info</a>
        </li>
        </ul>
        <div class="tab-content margin_top margin_bottom_15 ">
          <div id="one" class="tab-pane col-lg-6 col-xs-12 active padding_left_right_mob padding_top  ">
            <table class=" table table-bordered ">
              <!--<thead>
                <tr>
                  <th>Order Code</th>
                  <th> DVR 520</th>

                </tr>
              </thead>
              <tbody>
                <tr>
                  <th scope="row">Channel</th>
                  <td>16ch</td>

                </tr>
                <tr>
                  <th scope="row">Hard Disk</th>
                  <td>2Tb</td>
                </tr>
                <tr>
                  <th scope="row">Monitor</th>
                  <td>No</td>
                </tr>
              </tbody>-->
              $TBT_PRODDETAILS:{<tr>
                <th scope="row" class="table_bg">$it.AttributeName$</th>
                <td>$it.SpecValue$</td>
              </tr>}$
            </table>

          </div>
          <div id="two" class="tab-pane border_gray">
            <div class="alert alert-success border_radius_none text-center" id="AQTSM" role="alert" style="display:none;">
              <span class="glyphicon glyphicon-ok-circle margin_right" aria-hidden="true"></span>
              <strong> Thanks for your enquiry..! </strong> Our Customer Sales Team will be in contact with you Shortly.
            </div>
            <div id="show_hide">
              <!--<form class="margin_top_20">-->
                <div class="form-group col-lg-6 form_Group_height" style="margin-top:20px;">
                  <label class="font_normal margin_bottom" for="exampleInputEmail1">
                    Full Name<span class="error"> *</span>
                  </label>
                  <input type="text" placeholder="Full Name" id="txtFullname" maxlength="30"  onblur="Controlvalidate('fn')" class="form-control ask_question_input"/>
                  <span  id="Errfullname" class="error" style="display:none;"> Enter Full Name </span>
                </div>
                <div class="form-group col-lg-6 form_Group_height" style="margin-top:20px;">
                  <label class="font_normal margin_bottom" for="exampleInputEmail1">
                    Email address<span class="error"> *</span>
                  </label>
                  <input type="email" placeholder="Enter email" id="txtEmailAdd" class="form-control ask_question_input" maxlength="50"  onblur="Controlvalidate('ea')"/>
                  <span class="error" id="erremailadd" style="display:none;"> Enter Email Address </span>
                  <span class="error" id="errvalidmail" style="display:none;">Enter Valid Email </span>
                </div>
                <div class="form-group col-lg-6 form_Group_height">
                  <label class="font_normal margin_bottom" for="exampleInputPassword1">
                    Phone<span class="error"> *</span>
                  </label>
                  <input type="text" placeholder="Phone" id="txtPhone" maxlength="30"  onblur="Controlvalidate('p');" onkeypress="return validateNumber(event);"  class="form-control ask_question_input"/>
                  <span class="error" id="Errphone" style="display:none;">Enter Phone Number </span>
                </div>

                <div class="form-group col-lg-6">
                  <label class="font_normal margin_bottom" for="exampleInputPassword1">
                    Questions<span class="error"> *</span>
                  </label>
                  <textarea  id="txtQuestionx"   rows="4" onblur="Controlvalidate('q')" onkeypress="textCounter('txtQuestionx','countfield',600);" maxlength="600"  class="form-control ask_question_input"></textarea>
                  <span class="error" id="errquestion" style="display:none;">Enter The Question </span>
                  <div class="clear">
                    <input type="text" id="countfield" onblur="textCounter('txtQuestionx','countfield',600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter" class="char_remain readonly"/> Chars Remaining
                  </div>
                </div>
                <div class="form-group col-lg-6 form_Group_height">
                  <!--<label for="inputEmail3" class="col-sm-3 col-md-5 control-label font_normal">
                    <img src="$CAPTCHA_IMAGE$" />
                  </label>
                  <input type="text" maxlength="4" id="txtCaptchCode" onblur="Controlvalidate('cc');" onkeypress="return validateNumber(event);"  class="form-control ask_question_input width_size mob_left"/>
                  <span class="error mob_left mob_pad_left" id="errCaptchCode" style="display:none;"> Enter the code </span>
                  <span class="error mob_left mob_pad_left" id="errCaptchInvalid" style="display:none;"> Invalid code </span>
                  <span class="error mob_left mob_pad_left" id="errCaptchCode1" style="display:none;">$CC_CODE$</span>-->
                  <div id="recaptcha1"></div>
                </div>
                <div class="form-group col-lg-6">
                   <input type="button" class="btn green_bg white_color border_radius_none height_34" value="Submit" onclick="MailSend()"/>
                  <input type="button" class="btn blue_color white_color border_radius_none height_34" value="Reset" onclick="MailReset()"/>
                </div>
            </div>
            <div class="clear"></div>
          </div>
          <div id="twee" class="tab-pane border_gray">
            <div class="alert alert-success border_radius_none text-center" role="alert" id="BBSM" style="display:none;">
              <span class="glyphicon glyphicon-ok-circle margin_right" aria-hidden="true"></span>
              <strong>  Thanks for your Bulk Buy enquiry..! </strong> Our Customer Sales Team will be in contact with you Shortly.
            </div>
            <div id="show_hide_BB">
              <h4 class=" semi_bold font_15 margin_bottom_15 margin_top_20 padding_left_right_15 line_height_25 arial">Contact us for Bulk Buy and Project Based Pricing for $FNAME_BBPP$ </h4>
              <!--<form class="margin_top_20">-->
              <div class="form-group col-lg-6">
                <label class="font_normal margin_bottom" for="exampleInputEmail1">Product Code</label>
                <input type="text" readonly="true" value="$TBT_CODE$"  maxlength="30"   id="txtproductcode"  class="form-control ask_question_input readonly"/>
              </div>
              <div class="form-group col-lg-6">
                <label class="font_normal margin_bottom" for="exampleInputEmail1">Full Name</label>
                <input type="text" placeholder="Full Name" id="txtFullname_BBPP" onblur="Controlvalidate_BulkBuyPP('fullname')" class="form-control ask_question_input"/>
                <span class="error" id="Errfullname_BBPP" style="display:none;color: Red;"> Enter Full Name </span>
              </div>
              <div class="form-group col-lg-6">
                <label class="font_normal margin_bottom" for="exampleInputPassword1">QTY Required</label>
                <input type="text" placeholder="QTY Required" id="txtQTY"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('qty')"   onkeypress="return validateNumber(event);" class="form-control ask_question_input"/>
                <span class="error" id="ErrQTY" style="display:none;color: Red;"> Enter QTY </span>
              </div>

              <div class="form-group col-lg-6">
                <label class="font_normal margin_bottom" for="exampleInputPassword1">Email</label>
                <input type="text" placeholder="Email" id="txtEmail" class="form-control ask_question_input" maxlength="50"  onblur="Controlvalidate_BulkBuyPP('email')"/>
                <span class="error" id="erremailadd_BBPP" style="display:none;color: Red;"> Enter Email Address </span>
                <span class="error" id="errvalidmail_BBPP" style="display:none;color: Red;">Enter Valid Email </span>
              </div>
              <div class="form-group col-lg-6">
                <label class="font_normal margin_bottom" for="exampleInputPassword1">Delivery Time</label>
                <input type="text" placeholder="Delivery time" id="txtdeliverytime" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('deltime')" class="form-control ask_question_input"/>
                <span class="error" id="Errdeliverytime" style="display:none;color: Red;">Enter Delivery Time </span>
              </div>
              <div class="form-group col-lg-6">
                <label class="font_normal margin_bottom" for="exampleInputPassword1">Phone</label>
                <input type="text" placeholder="Phone" id="txtPhone_BBPP" class="form-control ask_question_input" onblur="Controlvalidate_BulkBuyPP('phone');" onkeypress="return validateNumber(event);" />
                <span class="error" id="Errphone_BBPP" style="display:none;color: Red;">Enter Phone Number </span>
              </div>
              <div class="form-group col-lg-6">
                <label class="font_normal margin_bottom" for="exampleInputPassword1">Target Price</label>
                <input type="text" placeholder="Target Price" id="txttargetprice" class="form-control ask_question_input" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')" />
              </div>
              <div class="form-group col-lg-6">
                <label class="font_normal margin_bottom" for="exampleInputPassword1">Notes / Addtional Info.</label>
                <textarea rows="3" id="txtnotesadditionalinfo" maxlength="600" class="form-control ask_question_input" onblur="Controlvalidate_BulkBuyPP('notes')" onkeypress="textCounter_BulkBuyPP('txtnotesadditionalinfo','counter_BBPP',600);"></textarea>
                <span class="error" id="errnotes" style="display:none;color: Red;">Enter The Notes / Addtional Info.</span>
                <div class="clear">
                  <input type="text" class="char_remain readonly" onblur="textCounter_BulkBuyPP('txtnotesadditionalinfo','counter_BBPP',600);" value="600" size="3" readonly="readonly" maxlength="3" id="counter_BBPP" name="counter_BBPP"/> Chars Remaining
                </div>
              </div>
              <div class="form-group col-lg-6 form_Group_height">

                <div id="recaptcha2"></div>
                <!--<label for="inputEmail3" class="col-sm-3 col-md-5 control-label font_normal">
                  <img src="$CAPTCHA_IMAGE$"/>
                </label>
                <input type="text" class="form-control ask_question_input width_size mob_left" id="txtCaptchCode_BBPP" maxlength="4"  onblur="Controlvalidate_BulkBuyPP('capcode');" onkeypress="return validateNumber(event);"/>
                <span class="error mob_left mob_pad_left" id="errCaptchCode_BBPP" style="display:none;color: Red;"> Enter the code </span>
                <span class="error mob_left mob_pad_left" id="errCaptchInvalid_BBPP" style="display:none;color: Red;"> Invalid code </span>
                <span class="error mob_left mob_pad_left" id="errCaptchCode1_BBPP" style="display:none;">$CC_CODE$</span>-->
              </div>
              <div class="form-group col-lg-6">
               

                <input type="button" value="Submit" name="" class="btn green_bg white_color border_radius_none height_34" onclick="MailSend_BulkBuyPP()"/>
                <input type="button" value="Reset" name="" class="btn blue_color white_color border_radius_none height_34" onclick="MailReset_BulkBuyPP()"/>
              </div>
              <!--</form>-->
            </div>
            <div class="clear"></div>
          </div>
          <div id="four" class="tab-pane border_gray">
            <div class="alert alert-success border_radius_none text-center" role="alert" id="DUSM" style="display:none;">
              <span class="glyphicon glyphicon-ok-circle margin_right" aria-hidden="true"></span>
              <strong> Thanks for your download enquiry..! </strong> Our Customer Sales Team will be in contact with you Shortly.
            </div>
            <div id="show_hide_DU">
              $TBT_DOWNLOAD_DATA$
              <!--<h4 class=" semi_bold font_15 margin_bottom_15 margin_top_20 padding_left_right_15 line_height_25 arial">
              No downloads currently available for $FNAME_DU$
            </h4>
            <h4 class=" semi_bold font_15 margin_bottom_15 padding_left_right_15 line_height_25 arial">
              Please contact us with your download requirements,e.g. User Manual, Data Sheet, etc and will get back to you.
            </h4>
              <div class="form-group col-lg-6">
                <label class="margin_bottom font_normal" for="exampleInputEmail1">Full Name</label>
                <input type="text" placeholder="Full Name" id="txtFullname_DU" class="form-control ask_question_input" maxlength="30"  onblur="Controlvalidate_DU('fullname_du')"/>
                <span class="error" id="Errfullname_DU" style="display:none;color: Red;"> Enter Full Name </span>
  </div>
              <div class="form-group col-lg-6">
                <label class="margin_bottom font_normal" for="exampleInputEmail1">Email address</label>
                <input type="email" placeholder="Enter email" id="txtEmail_DU" maxlength="50"  onblur="Controlvalidate_DU('email_du')"  class="form-control ask_question_input"/>
                <span class="error" id="erremailadd_DU" style="display:none;color: Red;"> Enter Email Address </span>
                <span class="error" id="errvalidmail_DU" style="display:none;color: Red;">Enter Valid Email </span>
  </div>
              <div class="form-group col-lg-6">
                <label class="margin_bottom font_normal" for="exampleInputPassword1">Phone</label>
                <input type="text" placeholder="Phone" id="txtPhone_DU" class="form-control ask_question_input" maxlength="30"  onblur="Controlvalidate_DU('phone_du');" onkeypress="return validateNumber(event);"/>
                <span class="error" id="Errphone_DU" style="display:none;color: Red;">Enter Phone Number </span>
  </div>

              <div class="form-group col-lg-6">
                <label class="margin_bottom font_normal" for="exampleInputPassword1">Download Required / Comments*</label>
                
                <textarea rows="3" id="txtdownloadre"  maxlength="600" class="form-control ask_question_input"  onblur="Controlvalidate_DU('notes_du')" onkeypress="textCounter_DU('txtdownloadre','counter_DU',600);"></textarea>
                <span class="error" id="errdownloadre" style="display:none;color: Red;">Download Required / Comments.</span>
                <div class="clear">
                  <input  class="char_remain readonly" type="text" onblur="textCounter_DU('txtdownloadre','counter_DU',600);" value="600" size="3" readonly="readonly" maxlength="3" id="counter_DU" name="counter_DU"/> Chars Remaining 
                </div>   
              </div>
              <div class="form-group col-lg-6 form_Group_height">
                <label for="inputEmail3" class="col-sm-3 col-md-5 control-label font_normal">
                  <img src="$CAPTCHA_IMAGE$" />
                </label>
                <input type="text" class="form-control ask_question_input width_size mob_left" id="txtCaptchCode_DU" maxlength="4"  onblur="Controlvalidate_DU('capcode_du');" onkeypress="return validateNumber(event);"/>
                <span class="error mob_left mob_pad_left" id="errCaptchCode_DU" style="display:none;color: Red;"> Enter the code </span>
                <span class="error mob_left mob_pad_left" id="errCaptchInvalid_DU" style="display:none;color: Red;"> Invalid code </span>
                <span class="error mob_left mob_pad_left" id="errCaptchCode1_DU" style="display:none;">$CC_CODE$</span>
  </div>
              <div class="form-group col-lg-6">
                --><!--<button class="btn green_bg white_color border_radius_none" type="submit">Submit</button>
                <button class="btn blue_color white_color border_radius_none" type="reset">Reset</button>--><!--
                <input type="button" value="Submit" name="" class="btn green_bg white_color border_radius_none height_34" onclick="MailSend_DU()" />
                <input type="button" value="Reset" name="" class="btn blue_color white_color border_radius_none height_34" onclick="MailReset_DU()" />
              </div>-->
          
            </div>
            <div class="clear"></div>
          </div>
          <div class="tab-pane border_gray" id="five">
            <div style="margin-left:10px;margin-top:10px;">
              <p style="font-family:Arial, Helvetica, sans-serif; font-size:16px;font-weight:bold; color:#007BDB; margin-bottom: 5px;margin-top: 20px;">&#36;9.90 Australia Wide Flat Rate Shipping*</p>
              <p style="color:#818181; text-align:left; font-size:12px; font-family:Arial, Helvetica, sans-serif;margin:0;">
                No matter where you are located in Australia we offer flat rate shipping.Some conditions apply,
                please see our <a style="text-decoration:none; color:#00a2e8; font-size:13px;" target="_blank" href="/Termsandconditions.aspx">Terms and Conditions </a>page.
              </p>
              <p style="font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;color:#007BDB; margin-bottom: 5px;margin-top: 20px;">Store Pick Up from Sydney Showroom</p>
              <p style="color:#818181; text-align:left; font-size:12px; font-family:Arial, Helvetica, sans-serif;margin:0;">
                Goods can be picked from our premises from 8.30AM to 5.00PM Monday to Friday and from 9AM till 4PM on Saturday. In order to avoid delays it is recommended that you place your   <br /> order 2 hours prior to pick up. please see our <a style="text-decoration:none; color:#00a2e8; font-size:13px;" target="_blank" href="/Termsandconditions.aspx">Terms and Conditions </a> page for more details.
              </p>

              <p style="font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;color:#007BDB; margin-bottom: 5px;margin-top: 20px;">International Order Shipping Available</p>
              <p style="color:#818181; text-align:left; font-size:12px; font-family:Arial, Helvetica, sans-serif;">
                We ship internationally. Once your order is received we will advise shipping cost to you by email. Your order will be updated with the freight cost.
              </p>
              <div class="clear"></div>
            </div>
          </div>
        </div>
      </div>
      <!-- /tabs -->

    </div>
    </div>
    <div class="clear"></div>
  </div>
  <div class="clear"></div>
</div>



<!--<div id="DUSubmit" style="text-align:center;display:none; height: 160px; padding-bottom: 135px;padding-top: 33px;" >
  <div style="background-color: #e6f7ee;border: thin solid #85d6ad;border-radius: 6px;color: #339966;font-size: 12px;font-weight: bold;padding: 14px;width: 664px;">
    <img src="/images/tick2.png"  style="margin-left: -20px;margin-right: 10px;vertical-align: middle;" />
    <span>
      Thanks for your download enquiry. Our Customer Sales Team will be in contact with you Shortly.
    </span>
  </div>

</div>-->


