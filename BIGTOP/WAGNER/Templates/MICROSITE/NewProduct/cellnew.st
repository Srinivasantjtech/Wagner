<div class="col-sm-6 col-md-4">
  <div class="thumbnail">
    <!--<img  class="padding_15 thumb_img_width"  src="/prodimages$TBT_TWEB_IMAGE1$"  alt="$TBT_CODE$ $TBT_FAMILY_NAME$" />-->
    <div class="thumb_img_size animateOnScroll animated zoomIn"  data-offset-top="200" data-timeout="0" data-animation-type="zoomIn">
      <!--<img src="/images/MicroSiteimages/por1.png" class="padding_15 thumb_img_width"></img>-->
      <a  href="$TBT_REWRITEURL$/mpd/"   class="thumb_img_width">
        <img class="lazy" data-original="$CDNROOT$prodimages$TBT_TWEB_IMAGE1$" style="max-width:175px;max-height:175px;color:#fff;font-size:1px;" alt="$TBT_FAMILY_NAME$" />      
      </a >
    </div>
      <div class="caption">
        <div class="list_details">
          <h3 class="font_14 text-center font_weight padding_0_10 margin_top_none margin_bottom_none thumb_head line_height20">
          <a  href="$TBT_REWRITEURL$/mpd/"     style="text-decoration:none;" >
            $TBT_FAMILY_NAME$ 
          </a>
          </h3>
          <p class="font_14 text-center padding_5 line_height19 cate_des_height">$TBT_DESCRIPTIONS$</p>
          <div class="order_quantity">
            <p class="text-center bolder font_1 thumb_head margin_bottom_none padding_btm  padding_left">
              <a  href="$TBT_REWRITEURL$/mpd/">Order No : $TBT_CODE$</a>
            </p>
           
            <div class="clear"></div>
          </div>
          
       
        </div>
     
        <div class="blue_color thumb_height list_button">
          <a role="button" class="white_color bolder font_size_16 mar_left_30 price price_width pull-left" href="#">&#36;$TBT_YOURCOST$</a>
          $if(TBT_ECOMENABLED)$$if(TBT_STOCK_STATUS_2)$
          <input type="text" style="display:none;text-align: center;"   id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"  name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" value="1" onkeyup="callproductqtykeypress(event)" />
          <a data-target=".bs-example-modal-lg" data-toggle="modal" 
             class=" white_color bolder font_size_16 pull-right green_addcart add_list_btn def-html" 
             onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');"
             style="cursor: pointer;">Add to Cart </a>
          $endif$$endif$
        </div>
      </div>
    </div>
</div>
