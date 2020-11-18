
  <tr>
    <td  data-title="Image">
     
      <div class="thumb-wrapper">
        <a href="$URL_RW_PATH$" class="thumb">
          <img  style="max-height: 50px; max-width: 50px; display: inline;"  data-original="$TWEB_Image$" class="lazy" />
          $if(SHOW_DIV)$
          <span>
              <img data-original="$TWEB_LargeImg$" alt="img" class="lazy" />
            </span>
          $endif$
        </a>
      </div>
      
    </td>
    <td data-title="Order Code"> $PROD_CODE$</td>
    $ATTRIBUTE_VALUES$
    <td data-title="Details">
      
      <a class="green_clr semi_bold details_a" href="$URL_RW_PATH$">Details</a>
    </td>
    <td data-title="Cost inc GST">
     
      <a class="price_a" style="text-decoration:none;">&#36;$COST$</a>
    </td>
    <td data-title="Cart">
      $if(SHOW_BUY)$
      <div class="inlineblock clearfix">
      <div class="number-box">
       
        <a onclick="decrement('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$');" class="norgtpadd" id="btnDecrement" style="cursor:pointer;"> - </a>
        <input type="text" value="1" onkeypress="return validateNumber(event);" name="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" id="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" class="nmbr_input"/>

       
        <a onclick="increment('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$');" class="norgtpadd" id="btnIncrement" style="cursor:pointer;"> + </a>
      </div>
      <div class="addcart-fmly">
        <a id="$PRODUCT_ID$"  onclick="productbuy('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$','$PRODUCT_ID$');" class="btn btn-primary def-html" data-tooltip="#html-content$PRODUCT_ID$">Add To Cart</a>
      </div>
      </div>
      $elseif(TBT_SUB_PRODUCT)$
      <div class="substitute_pro family_sub">
        <p>
          This Product has been replaced with:<br/>
          <a href="$TBT_REP_EA_PATH$" class="green">
            $TBT_REP_NIL_CODE$
          </a>

        </p>

        
      </div>
      $elseif(TBT_HIDE_BUY)$
      <div class="substitute_pro family_sub">
        <p>
          $PRODUCT_STATUS$

        </p>


      </div>
      $endif$
    </td>
  </tr>
