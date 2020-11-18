
<div class="mar5">
  <div class="fea-sell-w">
    <div class="fea-sell-img">
      <a href="$TBT_REWRITEURL$/mpd/" rel="/mpd.aspx?pid=$TBT_PRODUCT_ID$&amp;fid=$TBT_FAMILY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;path=$PRODUCT_EA_PATH$">        
        <img class="lazy" data-original="$CDNROOT$prodimages$TBT_TWEB_IMAGE1$"  alt="$TBT_CODE$ $TBT_FAMILY_NAME$" />
      </a>
    </div>
    <div class="fea-selr">
      <span class="fea-sell-n">
        <a href="$TBT_REWRITEURL$/mpd/" rel="/mpd.aspx?pid=$TBT_PRODUCT_ID$&amp;fid=$TBT_FAMILY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;path=$PRODUCT_EA_PATH$"  style="text-decoration:none;color: #404040;">
          $TBT_FAMILY_NAME$
        </a>
      </span>
      <div class="fea-sell-n">
        <a href="$TBT_REWRITEURL$/mpd/" rel="/mpd.aspx?pid=$TBT_PRODUCT_ID$&amp;fid=$TBT_FAMILY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;path=$PRODUCT_EA_PATH$"  style="text-decoration:none;color: ##404040;">
          <strong>Order No: $TBT_CODE$</strong>
        </a>
        </div>
      <div class="prod-pricef">
        &#36;$TBT_YOURCOST$        
      </div>
      <input type="text" style="display:none;"   id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" value="1" />
      $if(TBT_ECOMENABLED)$$if(TBT_STOCK_STATUS_2)$
      
      <a class="btn-cart" onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$">
        <!--<img src="/images/MicroSiteimages/btn-add-cart.png" />-->
        <img  class="mar_left5 margin_right sprite sprite-cart" alt=""/>
      </a>
      $endif$$endif$
    </div>
  </div>
  <div class="cl"></div>
</div>