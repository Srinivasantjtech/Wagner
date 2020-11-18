<div class="col-lg-12 padding_left_right text-center margin_top_20 ">
  <a href="$TBT_REWRITEURL$/mpd/" rel="/mpd.aspx?pid=$TBT_PRODUCT_ID$&amp;fid=$TBT_FAMILY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;path=$PRODUCT_EA_PATH$">
    <img class="padding_15" src="$CDNROOT$prodimages$TBT_TWEB_IMAGE1$"   style="max-width:175px;max-height:175px;" alt="$TBT_FAMILY_NAME$"/>
  </a>
  <!--<img class="padding_15 thumb_img_width" src="/images/MicroSiteimages/por1.png"  alt="$TBT_CODE$ $TBT_FAMILY_NAME$"/>-->
    <h3 class="font_14 text-center font_weight padding_5 margin_bottom_none margin_top_none">
      <a href="$TBT_REWRITEURL$/mpd/" rel="/mpd.aspx?pid=$TBT_PRODUCT_ID$&amp;fid=$TBT_FAMILY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;path=$PRODUCT_EA_PATH$"  style="text-decoration:none;color: #404040;">
        $TBT_FAMILY_NAME$
      </a>
    </h3>
    <p class="font_14 text-center padding_5 margin_bottom_none">
      <a href="$TBT_REWRITEURL$/mpd/" rel="/mpd.aspx?pid=$TBT_PRODUCT_ID$&amp;fid=$TBT_FAMILY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;path=$PRODUCT_EA_PATH$"  style="text-decoration:none;color: #404040;">
       Order No: $TBT_CODE$
      </a>
    </p>
    <p class="bolder font_18">&#36;$TBT_YOURCOST$ </p>
  <input type="text" style="display:none;"  value="1"  id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" />
  $if(TBT_ECOMENABLED)$$if(TBT_STOCK_STATUS_2)$
  <a class="padding_10 font_14 bolder blue_color fea"
     style="cursor: pointer;"
     onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$">Add To Cart</a>
  $endif$$endif$
</div>
