<div class="col-xs-10 w50  mb_p0 mb_w100">
  <div class="fmly_mbPro_detail clearfix">
    <h4 data-title="Order Code">$PROD_CODE$</h4>
    <div class="col-xs-12 pl0">
      <div class="cartnPrice">
        <h3 class="txtGreen f-b" style="font-weight: 700;">&#36;$COST$</h3>
        <div data-title="Cart">
          $if(SHOW_BUY)$
          <div class="inlineblock clearfix">
            <div class="number-box" style="margin-bottom:10px">

              <a onclick="decrement('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$');" class="norgtpadd" id="btnDecrement" style="cursor:pointer;"> - </a>
              <input type="text" value="1" onkeypress="return validateNumber(event);" name="txtmob$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" id="txtmob$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" class="nmbr_input"/>


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
        </div>
        
      </div>
      <a data-title="Details" href="$URL_RW_PATH$">View more info</a>
    </div>
    <div class="col-xs-8 mb_p0 pr0"  data-title="Image">
      <div class="thumb-wrapper">
        <a href="$URL_RW_PATH$" class="thumb">                   
          <img  style="max-height: 75px; max-width: 75px; display: inline;"  data-original="$TWEB_Image$" class="lazy" />
          $if(SHOW_DIV)$
          <!--<span>
            <img style="display: inline;" data-original="$TWEB_LargeImg$" alt="img" class="lazy" />
          </span>-->
          $endif$
        </a>
      </div>
    </div>
    <!--End fmly_mbPro_detail -->
    
  </div>
  <table class="table table-bordered mobile_tbl">
    <tr>
      <th>ORDER CODE</th>
      <td data-title="Order Code">$PROD_CODE$</td>
    </tr>
  <!-- <tr>
      <th>AVAILABILITY</th>
      <td >$STOCK_STATUS$</td>
    </tr> -->
    $ATTRIBUTE_VALUES$

    <!--<tr>
        <th>Availability</th>
        <td>{{val.producT_STATUS}}</td>
      </tr>
      <ng-container *ngFor="let val1 of val.dynamiC_VALUE">
        <tr *ngIf="val1.attributE_TITLE!='More_Info'">
          <th>{{val1.attributE_TITLE}}</th>
          <td>{{val1.attributE_VALUE}}</td>
        </tr>
      </ng-container>-->
  </table>
</div>