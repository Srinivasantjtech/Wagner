<div class="product-grid">
  <div class="grid-img animateOnScroll animated zoomIn" data-offset-top="200" data-timeout="0" data-animation-type="zoomIn">
    $if(TBT_SUB_FAMILY)$


    <a onclick="getValue_fl(this);" href="$TBT_REWRITEURL$/fl/" rev="fl.aspx?&fid=$TBT_FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&path=$FAMILY_EA_PATH$">
       <img data-original="$ATTRIBUTE_ID_747$" class="img-responsive lazy" alt="$FAMILY_NAME$"/>
    </a>
    $else$
    <a onclick="getValue_pd(this);" href="$TBT_REWRITEURL$/pd/"  rev="pd.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">
      <img data-original="$ATTRIBUTE_ID_747$" class="img-responsive lazy" alt="$FAMILY_NAME$"/>
    </a>
    $endif$
  </div>
  <div class="grid-detail">
    <div class="productname">
      <h5>
        $if(TBT_SUB_FAMILY)$
        <a onclick="getValue_fl(this);" href="$TBT_REWRITEURL$/fl/" rev="fl.aspx?&fid=$TBT_FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&path=$FAMILY_EA_PATH$">
          $FAMILY_NAME$
        </a>
        $else$
        <a onclick="getValue_pd(this);" href="$TBT_REWRITEURL$/pd/" rev="pd.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">
          $FAMILY_NAME$
        </a>
        $endif$
      </h5>
    </div>
    <div class="grid_discrip">
      <p>
        $ATTRIBUTEDESC$
      </p>
    </div>
    $if(TBT_SUB_FAMILY)$
    <div class="ordercode">
    </div>
    $else$
    <div class="ordercode">
      Order Code : $ATTRIBUTE_ID_1$
    </div>
    $endif$
    <div class="grd_cartwrap">
    
      $if(TBT_SUB_FAMILY)$
     <div class="product-prize">
      <div class="grid-prize">
        <span>Starts From: </span>
        $TBT_MIN_PRICE$
      </div>
    </div>
    <div class="grid-moreinfo">
      <a onclick="getValue_fl(this);" href="$TBT_REWRITEURL$/fl/" rev="fl.aspx?&fid=$TBT_FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&path=$FAMILY_EA_PATH$"> More Info </a>
    </div>

      $else$
      $if(TBT_SUB_PRODUCT)$
      $else$
      <div class="product-prize">
      <div class="grid-prize">
        <span> </span>
        &#36;$TBT_USER_PRICE$
      </div>
    </div>
      $endif$

      $if(TBT_HIDE_BUY)$
      <div class="product-gridunavail">
        <p class="product-gridunavail">
          $PRODUCT_STATUS$
        </p>
      </div>
      $elseif(TBT_SUB_PRODUCT)$
      <div class="substitute_wrap">

        <div class="substitute_details">

          <p> Product is unavailable and replaced with code: </p>
          <a href="$TBT_REP_EA_PATH$">
            <b class="green">$TBT_REP_NIL_CODE$</b>
          </a>
          <a href="$TBT_REP_EA_PATH$">View Substitute Product</a>
        </div>

      </div>


      $else$
      <div class="grid-cart">
      <a id="$PRODUCT_ID$"  style="cursor:pointer;"  onclick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');"
              class="def-html" data-tooltip="#html-content$TBT_PRODUCT_ID$">Add To Cart</a>
   
    </div>
  $endif$ 
      $endif$
   
</div>
  
  </div>
</div>


