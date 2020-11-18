<div class="item mb_p0 mb_w100">
  <div class="fmly_mbPro_detail p5 pt10 pl10 clearfix">
    <h4>$PROD_CODE$ - $PRODUCT_NAME$</h4>
	   $if(SHOW_BUY)$ <span class="bulkbuy-smallscreen"> $TBT_BULK_BUY$</span>$endif$
    <div class="col-xs-6 pl0">
      <img  style="max-height: 100px; max-width: 100px; display: inline;"  data-original="$TWEB_Image$" src="$TWEB_Image$" class="img_75" />
    </div>
    <div class="col-xs-14 mb_p0 pr0">
      <div class="cartnPrice">
		  $if(SHOW_BUY)$
		  <h3 class="prodlprice txtGreen f-b">&#36;$COST$</h3>
		  $endif$
		  <div class="order-code">
          <b>Order Code:</b>
			  <a href="$URL_RW_PATH$" style="color: #888;"> $PROD_CODE$ </a>
        </div>
		  $if(SHOW_BUY)$
		  <div class="available">
          <b> Availability : </b>
          $if(TBT_ISINSTOCK)$
          <span class="txtGreen" style="font-weight:700;">$TBT_ISINSTOCK_STAUS$</span>
			  $elseif(TBT_PLEASE_CALL)$
			  <span style="color:#f69e1b;font-weight:700">
				  Limited Stock
			  </span>
          $else$
          $TBT_STOCK_STATUS$$TBT_STOCK_STATUS_3$$TBT_STOCK_STATUS_1$
          $endif$
        </div>
		  $TBT_STOCK_STATUS_ETA$
		  <span class="bulkbuy-bigscreen"> $TBT_BULK_BUY$ </span>
		  $endif$
	  </div>
    </div>
    <div class="col-xs-20 ft_row p0 clearfix">
      <a class="pull-left mb_info_link" href="$URL_RW_PATH$">Product Details</a>
      $if(SHOW_BUY)$
      <div class="mb0 pull-right clearfix">
        <!--<input type="text" class="fmy_tbl_input" value="1" />-->
        <input type="text" class="nmbr_input" value="1" onkeypress="return validateNumber(event);" name="txtmob$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" id="txtmob$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" style="margin-right:5px"/>
        <a id="$PRODUCT_ID$" class="btn btn-primary mob_sm_btn" onclick="productbuy('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$','$PRODUCT_ID$');" data-tooltip="#html-content$PRODUCT_ID$">Add To Cart</a>

        <!--<a href="#" class="btn btn-success mb_btn" data-tooltip="">Add To Cart</a>-->
      </div>
      $elseif(TBT_SUB_PRODUCT)$
      <div class="substitute_pro family_sub">
        <p class="text-center">
          This Product has been replaced with:
          <a href="$TBT_REP_EA_PATH$" class="green">
            $TBT_REP_NIL_CODE$
          </a>
        </p>
      </div>
      $elseif(TBT_HIDE_BUY)$
      <div class="substitute_pro family_sub">
        <p class="text-center">
          $PRODUCT_STATUS$
        </p>
      </div>
      $endif$
    </div>
  </div>
  <!--End fmly_mbPro_detail -->
  <table class="table table-bordered customTbl1  mobile_tbl">
    <tr>
      <th>Order Code</th>
      <td>$PROD_CODE$</td>
    </tr>
    $ATTRIBUTE_VALUES$
  </table>
</div>
