<div  class="item col-sm-6 col-md-4 bg_none $LISTVIEW_CLASS$" >
  <div class="thumbnail">
    <div class="thumb_img_size animateOnScroll animated zoomIn"  data-offset-top="200" data-timeout="0" data-animation-type="zoomIn">
      $if(TBT_SUB_FAMILY)$
      <a href="$TBT_REWRITEURL$/mfl/"  rel="/mfl.aspx?fid=$FAMILY_ID$&amp;cid=$CATEGORY_ID$&amp;ProductResult=SUCCESS&amp;pcr=$PARENT_CATEGORY_ID$&amp;Path=$FAMILY_EA_PATH$" class="thumb_img_width"  >
        <img class="lazy" data-original="$CDNROOT$prodimages$ATTRIBUTE_ID_452$" alt="$FAMILY_NAME$"   style="max-width:175px;max-height:175px;color:#fff;font-size:1px;" />
      </a>
      $else$
      <a href="$TBT_REWRITEURL$/mpd/"   rel="/mpd.aspx?pid=$TBT_PRODUCT_ID$&amp;fid=$FAMILY_ID$&amp;cid=$CATEGORY_ID$&amp;pcr=$PARENT_CATEGORY_ID$&amp;Path=$PRODUCT_EA_PATH$" class="thumb_img_width" >
        <img class="lazy" data-original="$CDNROOT$prodimages$ATTRIBUTE_ID_452$" alt="$ATTRIBUTE_ID_1$ $PROD_DESC_ALT$" style="max-width:175px;max-height:175px;color:#fff;font-size:1px;" />
      </a>
      $endif$
    </div>
   
    <div class="caption">
      <div class="list_details1">
        <h3 class="font_14 text-center font_weight padding_0_10 margin_top_none margin_bottom_none thumb_head line_height20">
          $if(TBT_SUB_FAMILY)$
          <a href="$TBT_REWRITEURL$/mfl/" rel="/mfl.aspx?fid=$FAMILY_ID$&amp;cid=$CATEGORY_ID$&amp;ProductResult=SUCCESS&amp;pcr=$PARENT_CATEGORY_ID$&amp;Path=$FAMILY_EA_PATH$" style="text-decoration:none;color: #404040;">$FAMILY_NAME$</a>
          $else$
          <a href="$TBT_REWRITEURL$/mpd/" rel="/mpd.aspx?pid=$TBT_PRODUCT_ID$&amp;fid=$FAMILY_ID$&amp;cid=$CATEGORY_ID$&amp;pcr=$PARENT_CATEGORY_ID$&amp;Path=$PRODUCT_EA_PATH$" style="text-decoration:none;color: #404040;">$FAMILY_NAME$</a>
          $endif$

        </h3>
        <p class="font_14 text-center padding_5 line_height19 cate_des_height">
          $if(ATTRIBUTEDESC_GV)$
          $ATTRIBUTEDESC$...
          $else$
          $ATTRIBUTEDESC$
          $endif$
        </p>
        <div class="order_quantity">
          $if(TBT_BUY_PRODUCT)$
          $if(TBT_ORG_ORDERNO)$
          <p class="text-center bolder font_1 thumb_head or_no padding_left">


            <a  href="$TBT_REWRITEURL$/mpd/" rel="/mpd.aspx?&amp;pid=$TBT_PRODUCT_ID$&amp;fid=$FAMILY_ID$&amp;cid=$CATEGORY_ID$&amp;pcr=$PARENT_CATEGORY_ID$&amp;Path=$PRODUCT_EA_PATH$" style="text-decoration:none;">

                  Order No: $ATTRIBUTE_ID_1$
                </a>
              </p>
          $else$
          
          <div class="text-center bolder font_1 thumb_head or_no1 padding_left">
          <a class="pull-left" href="$TBT_REWRITEURL$/mpd/" rel="/mpd.aspx?pid=$TBT_PRODUCT_ID$&amp;amp;fid=$FAMILY_ID$&amp;amp;cid=$CATEGORY_ID$&amp;amp;pcr=$PARENT_CATEGORY_ID$&amp;amp;Path=$PRODUCT_EA_PATH$" style="text-decoration:none;">
            <span>Order No :</span>
            <p> $ATTRIBUTE_ID_1$</p>
          </a>
        </div>
         
          $endif$
       
       
          $if(TBT_WITH_LOGIN)$
          <p class="text-center bolder font_1 thumb_head quan_right padding_right15">
            Qty : <input name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" value="1"  class=" cart_quantity " size="2" maxlength="6" type="text" onkeydown="return keyct(event)" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" onkeyup="callproductqtykeypress(event)" style="text-align: center;" />
          </p>
          $endif$

          $else$
          $if(TBT_SUB_FAMILY)$
          <p class="text-center bolder font_1 thumb_head margin_bottom_none padding_btm  padding_left">
            
            <a href="$TBT_REWRITEURL$/mfl/" rel="/mfl.aspx?fid=$FAMILY_ID$&amp;amp;cid=$CATEGORY_ID$&amp;amp;ProductResult=SUCCESS&amp;amp;pcr=$PARENT_CATEGORY_ID$&amp;amp;Path=$FAMILY_EA_PATH$"  >
            View :$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$
          </a>
        </p>
          $else$
          <p class="text-center bolder font_1 thumb_head margin_bottom_none padding_btm  padding_left">
          <a href="$TBT_REWRITEURL$/mpd/"  rel="/mpd.aspx?pid=$TBT_PRODUCT_ID$&amp;amp;fid=$FAMILY_ID$&amp;amp;cid=$CATEGORY_ID$&amp;amp;pcr=$PARENT_CATEGORY_ID$&amp;Path=$PRODUCT_EA_PATH$" >
            View:$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$
          </a>
        </p>
          $endif$
          $endif$
          <div class="clear"></div>
        </div>
      </div>
      <div class="blue_color thumb_height list_button">
        $if(TBT_BUY_PRODUCT)$
        <div class="amount_list">
        <a  href="$TBT_REWRITEURL$/mpd/" class="white_color bolder font_size_16 margin_left15 price price_width pull-left" role="button">&#36;$ATTRIBUTE_ID_5$ </a>
          <div class="clear"></div>
        </div>
        $else$
        <div class="starts_From">
          <a href="$TBT_REWRITEURL$/mfl/" role="button" class="white_color bolder font_size_16  price">Starts From  $TBT_MIN_PRICE$</a>
        </div>

        $endif$
        $if(TBT_BUY_PRODUCT)$
        $if(TBT_WITH_LOGIN)$

        $if(TBT_HIDE_BUY)$
        <!--<p class="ordercode">
          Product Temporarily Unavailable.<br /> Please Contact Us for more details
        </p>-->
        $else$

        <a data-tooltip="#html-content$TBT_PRODUCT_ID$" data-target=".bs-example-modal-lg" data-toggle="modal" role="button"  class=" white_color bolder font_size_16 pull-right green_addcart add_list_btn def-html" onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');" style="cursor: pointer;">Add to Cart </a>
        $endif$
        $else$
        $if(TBT_SUB_FAMILY)$

        <a href="$TBT_REWRITEURL$/mfl/" class=" white_color bolder font_size_16 pull-right green_addcart add_list_btn def-html"  style="cursor: pointer;margin-top:0px;">More info </a>
        $else$
        
        <a href="$TBT_REWRITEURL$/mpd/" class=" white_color bolder font_size_16 pull-right green_addcart add_list_btn def-html" style="cursor: pointer;margin-top:0px;">More info </a>
        $endif$
        $endif$
        $else$
        $if(TBT_SUB_FAMILY)$
      
        <a href="$TBT_REWRITEURL$/mfl/" class=" white_color bolder font_size_16 pull-right green_addcart add_list_btn def-html" style="cursor: pointer;">More info </a>
        $else$
       
        <a href="$TBT_REWRITEURL$/mpd/" class=" white_color bolder font_size_16 pull-right green_addcart add_list_btn def-html"  style="cursor: pointer;">More info </a>
        $endif$
        $endif$

      </div>

      <!-- tooltip starts -->
      <div style="display:none;" id="html-content$TBT_PRODUCT_ID$">
        $TBT_PRODUCT_PRICE_TABLE$
      </div>
      <!-- tooltip ends -->
    </div>
    <div class="clear"></div>
  </div>
</div>



