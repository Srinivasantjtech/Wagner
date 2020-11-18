<div itemscope="" itemtype="http://schema.org/Product">
<div class="col-md-9 imageblock-left">

  <div class="text-center zoomimg" id="product-image" >
    $if(TBT_NOIMAGE)$
    <img class = "cloudzoom" src = "$TBT_TWEB_IMAGE12$"
            data-cloudzoom = "zoomImage: '$TBT_TWEB_IMAGE12_LARGE$'" alt="$TBT_FAMILY_NAME$" />
   
    $else$
    <img src="$CDN$prodimages/images/noimage.gif" style="max-width:256px;max-height:256px;" class="lazy" alt="$TBT_FAMILY_NAME$"></img>
    $endif$
    </div>
  <div class="prodiv" id="productvideo" style="display:none;min-width: 100px;height:512px;max-width:512px;max-height:512px;text-align:center">
    <iframe id="ifrme" width="100%" height="450px"  src="$youtubelink$" frameborder="0" allowfullscreen="">
    </iframe>
  </div>
  <span class="gen-spacer"></span>
  <div class="zoom_thump text-center">
   
      $TBT_MULTIIMAGES:{
  
    <div class=" thumb_img" onclick="Imageshow();" >
      <a href="$it.Image1024$" class="thumb-link">
        <img    data-cloudzoom="useZoom:&quot;.cloudzoom&quot;,
                 image:&quot;$it.Image1024$&quot;,
                 zoomImage:&quot;$it.Image1024$&quot;"
               title="" alt="$TBT_FAMILY_NAME$"
               src="$it.Thumpnail$" class="cloudzoom-gallery"/>
        </a>
    </div>
  
    }
    $
    $if(ISYOUTUBELIKE)$
    <div id="video"  onclick="videoshow();">
      <ul>
        <li height="112px" width="150px">
          <img data-original="$YouTubeImg$" style=" width:90px;height:75px;cursor:pointer;" class="lazy" />
          <img data-original="$CDN$images/red_playbtn.png" class="red_play lazy" style="cursor:pointer;"/>
        </li>
      </ul>
    </div>
    $endif$
    <div class="clear"></div>
  </div>
  <span class="gen-spacer"></span>
</div>
<div class="col-md-9 imagedetail-right">
  $if(TBT_BRAND_NAME)$
  
  <meta itemprop="brand" content="$BRAND_NAME$" />
  $endif$
  <h1 itemprop="name">$TBT_FAMILY_NAME$</h1>

  <div class="line"></div>

  $if(TBT_REPLACED)$
  $if(TBT_NIL_REPLACED)$
      <div class="col-lg-20">
    <div class="col-lg-5 col-sm-7" itemprop="offers" itemscope="" itemtype="http://schema.org/Offer">
      <meta itemprop="category" content="$PRODUCT_CATEGORY_NAME$" />
      <ul>
        <meta itemprop="priceCurrency" content="AUD"/>
        <h2 class="prodlprice">$TBT_COST$</h2>  
        <meta itemprop="price" content="$TBT_META_COST$"/>
        <link itemprop="availability" href="$TBT_STOCK_HREF$"/>
        
      </ul>

    </div>

    <div class="col-lg-15 col-sm-13 paddrt0">


      <div class="col-lg-20 paddrt0">
        <div class="pro-discrip-cartbtn alignboth">

          <div class="form-inline">

            <div class="form-group">
              <div class="number-box">

                <a onclick="decrement('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$');" id="btnDecrement" style="cursor:pointer;"> - </a>
                <input id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"  size="5"  maxlength="6" type="text" class="nmbr_input" onkeydown="return keyct(event)"  value="1"  onkeypress="return validateNumber(event);"/>


                <a onclick="increment('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$');" id="btnIncrement" style="cursor:pointer;"> + </a>
              </div>
             
              <button type="button" style="cursor:pointer;" class="btn btn-primary" onclick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');">Add to Cart</button>
            
            </div>
          </div>
        </div>
      </div>



    </div>
  </div>
      <div class="col-lg-20 paddt_b">
        <div class="col-lg-12">
          <li class="">
            <span>Order Code:</span> $TBT_REP_NIL_CODE$
          </li>
        </div>
        <div class="col-lg-8">
          <li>
            <span style="margin-left:-8px;">Availability: </span><!--<link itemprop="availability" href="$TBT_STOCK_HREF$"/>-->
            $TBT_REP_STATUS$ 
           
          </li>
        </div>
      </div>
      $else$
      <div class="pro-discrip col-lg-20">
        <div class="col-lg-20 nopadd clearfix">
          <div class="col-lg-13 col-md-13 nolftpadd">
            <p>
              $TBT_SHORT_DESCRIPTION$
            </p>
            <li id="div1" style="display:block;">
              $if(TBT_PROD_DESC_SHOW)$
              $TBT_DESCRIPTION$
              $endif$
              $TBT_MORE$
            </li>
            <li id="div2" style="display:none;">
              $TBT_DESCALL$
            </li>
          </div>
          <div class="col-lg-7 col-md-7 norgtpadd">
        <div class="substitute-wrap">

          <div class="substitute_head">
            <div class="clearfix">
              <img src="$CDN$images/subproduct_icon.jpg">
                <a href="#">SUBSTITUTED PRODUCT ITEM</a>
              </img>
              </div>
          </div>


          <div class="substitute_body">
            <p>
              <span>NOTICE! </span> This product is currently unavailable and has been replaced with the product below
            </p>

          </div>

          <div class="substitute_rgt">
            <a href="$TBT_REP_EA_PATH$">$TBT_REP_NIL_CODE$</a>
            <a href="$TBT_REP_EA_PATH$">View Substitute Product</a>
          </div>

        </div>
      </div>
          <div class="col-lg-20">
            $if(TBT_MORE_SHOW)$
            <a class="fmly-showmore" id="smlm" style="display:block;cursor:pointer;"  onclick="showmoredes();">Read / Show More</a>
            <a class="fmly-showmore"  id="lessmore" style="display:none;cursor:pointer;"  onclick="showmoredes();">Read / Less More</a>
            $endif$
            <div class="founded-ctgory" style="display:$TBT_DISPLAY_TIP$;">
              <div>
                <img src="$CDN$images/view-catgory.png" class="see-catgory"/>
                <p>
                  Related Products : <a href="$TBT_REWRITEURL_NEW$/fl/" class="">View Product Group Page</a>
                </p>
              </div>
            </div>
            <div class="founded-ctgory">
              <div>
                <img src="$CDN$images/see-catgory.png" class="see-catgory"/>
                <p>
                  Found in Category : <a href="$TBT_CT_HREF$" class="">$TBT_CT_NAME$ </a>
                </p>
              </div>
            </div>
          </div>
       </div>
      </div>
      $endif$
      $else$
      <div class="col-lg-20">
        <div class="col-lg-5 col-sm-7" itemprop="offers" itemscope="" itemtype="http://schema.org/Offer">
          <meta itemprop="category" content="$PRODUCT_CATEGORY_NAME$" />
          <ul>
            <meta itemprop="priceCurrency" content="AUD"/>
            <h2 class="prodlprice" >$TBT_COST$</h2>
            <meta itemprop="price" content="$TBT_META_COST$"/>
            <link itemprop="availability" href="$TBT_STOCK_HREF$"/>
            
          </ul>

        </div>

        <div class="col-lg-15 col-sm-13 paddrt0">


          <div class="col-lg-20 paddrt0">
            <div class="pro-discrip-cartbtn alignboth">
            
              <div class="form-inline">

                <div class="form-group">
                  <div class="number-box">
                   
                    <a onclick="decrement('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$');" id="btnDecrement" style="cursor:pointer;"> - </a>
                    <input id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"  size="5"  maxlength="6" type="text" class="nmbr_input" onkeydown="return keyct(event)"  value="1"  onkeypress="return validateNumber(event);"/>

                   
                    <a onclick="increment('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$');" id="btnIncrement" style="cursor:pointer;"> + </a>
                  </div>
                  $if(TBT_HIDE_BUY)$
                  $else$
                  <button type="button" class="btn btn-primary" onclick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');">Add to Cart</button>
                  $endif$
                </div>
              </div>
            </div>
          </div>



        </div>
      </div>
      <div class="col-lg-20 paddt_b">
        <div class="col-lg-12">
          <li class="">
            <span>Order Code:</span>$TBT_CODE$
          </li>
        </div>
        <div class="col-lg-8">
          <li>
            <span style="margin-left:-8px;">Availability: </span><!--<link itemprop="availability" href="$TBT_STOCK_HREF$"/>-->$TBT_STOCK_STATUS$$TBT_STOCK_STATUS_3$$TBT_STOCK_STATUS_1$
            $if(TBT_HIDE_BUY)$ PLEASE CONTACT US  $endif$
          </li>
        </div>
      </div>
      <div class="pro-discrip col-lg-20 nolftpadd">
    <div class="col-lg-20 nolftpadd productdetail" itemprop="description">
      <p>
        $TBT_SHORT_DESCRIPTION$
      </p>
      <li id="div1" style="display:block;">
        $if(TBT_PROD_DESC_SHOW)$
        $TBT_DESCRIPTION$
        $endif$
        $TBT_MORE$
      </li>
      <li id="div2" style="display:none;">
        $TBT_DESCALL$
      </li>
    </div>
    <div class="col-lg-20 nolftpadd">
      $if(TBT_MORE_SHOW)$
      <a class="fmly-showmore" id="smlm" style="display:block;cursor:pointer;"  onclick="showmoredes();">Read / Show More</a>
      <a class="fmly-showmore"  id="lessmore" style="display:none;cursor:pointer;"  onclick="showmoredes();">Read / Less More</a>
      $endif$

      <div class="founded-ctgory" style="display:$TBT_DISPLAY_TIP$;">
        <div>
          <img src="$CDN$images/view-catgory.png" class="see-catgory"/>
          <p>
            Related Products : <a href="$TBT_REWRITEURL_NEW$/fl/" class="">View Product Group Page</a>
          </p>
        </div>
      </div>
      <div class="founded-ctgory">
        <div>
          <img src="$CDN$images/see-catgory.png" class="see-catgory"/>
          <p>
            Found in Category : <a href="$TBT_CT_HREF$" class="">$TBT_CT_NAME$ </a>
          </p>
        </div>
      </div>
     
    </div>
  </div>
      $endif$
    </div>
</div>
<div class="clear"></div>
<div class="col-sm-20">
  <div class="tabs">
    <ul id="myTab" class="nav nav-tabs">
      <li class="active">
        <a href="#profile">Product Specs</a>
      </li>
      <li class="">
        <a href="#tab2">Ask a Question</a>
      </li>
      <li class="">
        <a href="#tab3">Bulk Buy / Project Price</a>
      </li>
      <li>
        <a href="#tab4">Downloads</a>
      </li>
      <li>
        <a href="#shipping">Shipping Info</a>
      </li>
    </ul>
    <div class="tab-content">
      <div id="profile" class="tab-pane active">
        <p>
          Product Specs for <a style="text-decoration:none !important;">$TBT_FAMILY_NAME$</a>
        </p>
        <table class="table specs">
          <tbody>
             $TBT_PRODDETAILS:{<tr>
              <th>$it.AttributeName$</th>
              <td>$it.SpecValue$</td>
            </tr>}$
          </tbody>
        </table>
      </div>
      $TBT_DOWNLOAD_DATA$
     <div id="tab2" class="tab-pane">

       <div class="text-center" id="askaquestionLoad" style="display:none">
         <img src="$CDN$images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
         <br>Loading Data.. Please Wait..</br>
       </div>
      <!--<div id="messages">
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
              <input type="text" id="txtFullname"  maxlength="30" onblur="Controlvalidate('fn')" class="form-control"/>
              <span class="mandatory dpynone" id="Errfullname">Enter Full Name</span>
              <span id="ProductCode" class="dpynone">$TBT_CODE$</span>
                                        </div>
            <div class="col-xs-20 col-sm-10">
              <label>
                Email<span class="required">*</span>
              </label>
              <input type="text" id="txtEmailAdd" maxlength="50"  onblur="Controlvalidate('ea')" class="form-control"/>
              <span class="mandatory dpynone" id="erremailadd"> Enter Email Address </span>
              <span class="mandatory dpynone" id="errvalidmail">Enter Valid Email </span>
                                        </div>
          </div>
          <div class="row">
            <div class="col-xs-20 col-sm-10">
              <label>
                Phone<span class="required">*</span>
              </label>
              <input type="text" id="txtPhone"  maxlength="30"  onblur="Controlvalidate('p');" onkeypress="return validateNumber(event);" class="form-control"/>
              <span class="mandatory dpynone" id="Errphone">Enter Phone Number </span>
                                        </div>
            <div class="col-xs-8 col-sm-3">
              
              <label class="vishid">
                Code<span class="required">*</span>
              </label>
              <div id="recaptcha1"></div>
       
            </div>
    
          </div>
          <br />
            <div class="row">
              <div class="col-xs-20">
                <label>
                  Questions<span class="required">*</span>
                </label>
                <textarea  id="txtQuestionx" rows="10" maxlength="600" cols="34"  onblur="Controlvalidate('q')" onkeypress="textCounter(this,this.form.counter,600);" class="form-control fullwidth"></textarea>
                <span class="mandatory dpynone" id="errquestion">Enter The Question </span>
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
                <button value="Reset" onclick="MailReset()" type="reset" class="btn btn-default">Reset</button>
              </p>
            </form>


      </div>
      <div class="alert alert-success" id="divAskQuestionSubmit" style="display:none;">
        <strong>Success!</strong> Thanks for your enquiry. Our Customer Sales Team will be in contact with you Shortly.
      </div>-->
      </div>
      <div id="tab3" class="tab-pane">
        <div class="text-center" id="bulkbuyLoad" style="display:none">
          <img src="$CDN$images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
          <br>Loading Data.. Please Wait..</br>
        </div>
        <!--<div id="bulkbuy" >
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
        </div>-->
      </div>
      
      <div id="shipping" class="tab-pane">
        <div class="text-center" id="shippingLoad" style="display:none">
          <img src="$CDN$images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
          <br>Loading Data.. Please Wait..</br>
        </div>
        <!--<h3>&#36;9.90 Australia Wide Flat Rate Shipping*</h3>
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
 Your order will be updated with the freight cost.</p>-->

      </div>
    </div>
  </div>
</div>

