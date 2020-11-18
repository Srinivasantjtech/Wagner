<div class="product-grid">
  <div class="grid-img animateOnScroll animated zoomIn" data-offset-top="200" data-timeout="0" data-animation-type="zoomIn">
    <a onclick="getValue_pd(this);" href="$TBT_REWRITEURL$/pd/" rev="pd.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$">
      <img class="img-responsive lazy" data-original="$TBT_TWEB_IMAGE1$" alt="$TBT_FAMILY_NAME$"/>
    </a>
  </div>
  <div class="grid-detail">
    <div class="productname">      
      <h5>
        <a onclick="getValue_pd(this);" href="$TBT_REWRITEURL$/pd/" rev="pd.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$">$TBT_FAMILY_NAME$</a>
      </h5>
    </div>
    <div class="grid_discrip">
      <p> $TBT_DESCRIPTIONS$</p>
    </div>
    <div class="ordercode">
      Order Code : $TBT_CODE$
    </div>
    <div class="grd_cartwrap">
    <div class="product-prize">
      <span></span>
       <div class="grid-prize">
        &#36;$TBT_YOURCOST$
      </div>
      </div>
    <div class="grid-cart">
        $if(TBT_ECOMENABLED)$
        <a style="cursor:pointer;" onclick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');">Add to Cart</a>
        $endif$
      </div>
    </div>
  </div>
</div>
 