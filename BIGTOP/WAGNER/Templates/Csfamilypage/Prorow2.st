<div class="panel panel-default">
  <div class="panel-heading p10 clearfix">
	  $PROD_CODE$ - $PRODUCT_NAME$
	  <a href="$URL_RW_PATH$" class="pull-right mr15">More Info</a>
  </div>
  <div class="panel-body p10">
    <div class="row">
      <!--<div class="col-sm-1 w10">
        <img  style="max-height: 75px; max-width: 75px; display: inline;" data-original="$TWEB_Image$" src="$TWEB_Image$" class="img_75 thumb_img_fl" />
      </div>-->
      <div class="col-sm-20 pl0 family_prod_spec">
		  <div class="w10" style="margin: auto;text-align: center;">
			  <img  style="max-height: 100px; max-width: 100px; display: inline;" data-original="$TWEB_Image$" src="$TWEB_Image$" class="img_75" />
		  </div>
        <div class="table-responsive hidden-xs" style="width:89%">
          <table class="table table-bordered customTbl1 margin_top mb10 text-center w89">
            <tr>
              <th width="18%">Order Code</th>
              <td>$PROD_CODE$</td>
            </tr>
            $ATTRIBUTE_VALUES$
          </table>
        </div>
      </div>
    </div>
    <!-- End Row -->

    <div class="row">
      <div class="col-sm-2 w10 p0">
      </div>
      <div class="col-sm-12 pl10 pr0">
        <div class="inlineblk f12 mr15">
          <a href="$URL_RW_PATH$"> Product Details </a>
        </div>
        <div class="inlineblk mob_sm_txt" style="margin-right: 15px;">
          <b>Order Code :</b><a href="$URL_RW_PATH$" style="color: #888;"> $PROD_CODE$ </a>
        </div>
		  $if(SHOW_BUY)$
		  <div class="inlineblk mob_sm_txt" style="margin-right: 15px;">
          <b>Availability: </b>
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
		  $endif$
	  </div>
      <div class="col-sm-6 pl0 pr15">
        $if(SHOW_BUY)$
        <div class="inlineblk" style="float:right">
          <div class="number-box">
            <a onclick="decrement('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$');" class="norgtpadd" id="btnDecrement" style="cursor:pointer;"> - </a>
            <input type="text" value="1" onkeypress="return validateNumber(event);" name="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" id="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" class="nmbr_input"/>
            <a onclick="increment('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$');" class="norgtpadd" id="btnIncrement" style="cursor:pointer;"> + </a>
          </div>
          <button type="button" class="btn btn-primary mob_sm_btn"  onclick="productbuy('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$','$PRODUCT_ID$');">Add to Cart</button>
        </div>
		<div class="inlineblk vcenter mr20" style="float:right; margin-top: 8px;">
			<h3 class="prodlprice txtGreen f-b mb0">&#36;$COST$</h3>
		</div>
        $TBT_BULK_BUY$
        $elseif(TBT_SUB_PRODUCT)$
        <div class="substitute_pro family_sub" style="float:right">
          <p class="text-center">
            This Product has been replaced with:
            <a href="$TBT_REP_EA_PATH$" class="green">
              $TBT_REP_NIL_CODE$
            </a>
          </p>
        </div>
        $elseif(TBT_HIDE_BUY)$
        <div class="substitute_pro family_sub" style="float:right">
          <p class="text-center">
            $PRODUCT_STATUS$
          </p>
        </div>
        $endif$
      </div>
    </div>
    <!-- End Row -->

  </div>
</div>