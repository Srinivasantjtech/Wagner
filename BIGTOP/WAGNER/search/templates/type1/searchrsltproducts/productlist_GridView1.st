<div class="product-grid">
  <div class="grid-img">
    $if(TBT_SUB_FAMILY)$


    <a href="$TBT_REWRITEURL$/fl/">
      <img data-original="$ATTRIBUTE_ID_452$" class="img-responsive lazy"/>
    </a>
    $else$
    <a href="$TBT_REWRITEURL$/pd/">
      <img data-original="$ATTRIBUTE_ID_452$" class="img-responsive lazy"/>
    </a>
    $endif$
  </div>
  <div class="grid-detail">
    <div class="productname">
      <h5>
        $if(TBT_SUB_FAMILY)$
        <a  href="$TBT_REWRITEURL$/fl/">
          $FAMILY_NAME$
        </a>
        $else$
        <a href="$TBT_REWRITEURL$/pd/">
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
    <!--<div class="dpynone" id="divdesc">
      <p>
        $ATTRIBUTEDESC$
       
      </p>
    </div>-->
    
    $if(TBT_SUB_FAMILY)$
<<<<<<< .mine
    <div class="grid-moreinfo">
        <a href="$TBT_REWRITEURL$/fl/"> More Info </a>
      </div>
    
      <div class="product-prize">
=======
   <div class="product-prize">
      <div class="grid-prize">
        <span>Starts From: </span>
        $TBT_MIN_PRICE$
      </div>
    </div>
    <div class="grid-moreinfo">
      <a href="$TBT_REWRITEURL$/fl/"> More Info </a>
    </div>
      <!--<div class="product-prize">
>>>>>>> .r4417
    
          <span>Starts From</span>
          <div class="grid-prize">
            $TBT_MIN_PRICE$
          </div>
     
      </div>
     
    $else$
      <div class="grid-cart">
        <a id="$TBT_PRODUCT_ID$"  onclick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$','$TBT_PRODUCT_ID$');"
              class="def-html" data-tooltip="#html-content$TBT_PRODUCT_ID$">Add To Cart</a>
      </div>-->
<<<<<<< .mine
      <div class="product-prize">
=======
    
    $else$
   <div class="product-prize">
      <div class="grid-prize">
        <span> </span>
        &#36;$ATTRIBUTE_ID_5$
      </div>
    </div>
    <div class="grid-cart">
      <a id="$TBT_PRODUCT_ID$"  onclick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$','$TBT_PRODUCT_ID$');"
     class="def-html" data-tooltip="#html-content$TBT_PRODUCT_ID$">Add To Cart</a>
    </div>
      <!--<div class="product-prize">
>>>>>>> .r4417
        <span></span>
        <div class="grid-prize">
          &#36;$ATTRIBUTE_ID_5$
        </div>
      </div>
<<<<<<< .mine
   
=======
      <div class="grid-cart">
        <a id="$TBT_PRODUCT_ID$"  onclick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$','$TBT_PRODUCT_ID$');"
              class="def-html" data-tooltip="#html-content$TBT_PRODUCT_ID$">Add To Cart</a>
      </div>-->
>>>>>>> .r4417

       $endif$
    </div>
  </div>
</div>


