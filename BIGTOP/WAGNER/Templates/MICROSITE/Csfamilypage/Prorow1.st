<tr>
  <td style="width:200px">
    <img style="max-height: 50px; max-width: 50px; display: inline;"  data-original="$TWEB_Image$" class="lazy" alt="$PROD_CODE$ $PROD_DESCRIPTION$" />
  </td>
  <td>$PROD_CODE$</td>
  $ATTRIBUTE_VALUES$
  <td>
    <a href="$URL_RW_PATH$" rel="pd.aspx?pid=$PRODUCT_ID$&amp;fid=$PARENT_FAMILY_ID$&amp;cid=$CAT_ID$&amp;path=$EA_PATH$" class="blue_color_text semi_bold details_a">Details</a>
  </td>
  <td>
    <!--<a href="$URL_RW_PATH$" class="blue_color_text semi_bold details_a"  >  &#36;$COST$ price / Stock Status</a>-->
    <a href="$URL_RW_PATH$" class="blue_color_text semi_bold details_a"  >  &#36;$COST$</a>
  </td>
  <td >
    $if(SHOW_BUY)$
    <input id="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" type="text" class="quantity " value="1" onkeydown="return keyct(event)" name="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$"  maxlength="6">
      <input id="$PRODUCT_ID$" 
             onclick="productbuy('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$','$PRODUCT_ID$');" 
             type="button" 
             class="addcart_btn_tabview green_bg add_btn hover_a bolder margin_left border_none white_color def-html" 
             value="Add To Cart" data-tooltip="#html-content$PRODUCT_ID$" style="cursor: pointer;">


        <!-- tooltip starts -->
        <div id = "html-content$PRODUCT_ID$" style="display:none;">
          $PRICE_TABLE$
        </div>
        <!-- tooltip ends -->
        $endif$
      </td>


</tr>

