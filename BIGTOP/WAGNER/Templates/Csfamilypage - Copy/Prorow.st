
  <tr>
    <td>
      <!--<div class="pro_thum_outer">
        $if(SHOW_DIV)$
        <div id="pro_img_popup$PRODUCT_ID$" style=" visibility:hidden;" class="pro_img_popup">
          <img src="$TWEB_LargeImg$" alt="$PROD_CODE$ $PROD_DESCRIPTION$" />
        </div>
        $endif$
        <img  onmouseover="javascript:Moverimgtag('$PRODUCT_ID$');" onmouseout="javascript:Moutimgtag('$PRODUCT_ID$');" src="$TWEB_Image$"  class="fprorimg" />
      </div>-->
      <div class="thumb-wrapper">
        <a href="#" class="thumb">
          <img  style="width:50px;height:50px;"  src="$TWEB_Image$" />
          $if(SHOW_DIV)$
          <span>
              <img src="$TWEB_LargeImg$" alt="img" />
            </span>
          $endif$
        </a>
      </div>
      
    </td>
    <td> $PROD_CODE$ </td>
    $ATTRIBUTE_VALUES$
    <td>
      <!--<a class="tx_3" href="$URL_RW_PATH$">
        <img width="17" height="14" border="0" align="middle" src="/images/ico_details.gif" alt=""/>
        <br/>Details
      </a>-->
      <a class="green_clr semi_bold details_a" href="$URL_RW_PATH$">Details</a>
    </td>
    <td>
      <!--<div class="pricepopup">
        <div class="popupouterdiv2none" id="pid$PRODUCT_ID$">
          <div class="popupaero"></div>
          $PRICE_TABLE$
        </div>
        <a onmouseover="javascript:Moverstockstatus('$PRODUCT_ID$');" onmouseout="javascript:Moutstockstatus('$PRODUCT_ID$');" class="poppricenone tdn">
          &#36;$COST$
        </a>
      </div>-->
      <a class="price_a" style="text-decoration:none;">&#36;$COST$</a>
    </td>
    <td>
      $if(SHOW_BUY)$
      <div class="inlineblock">
      <div class="number-box">
        <!--<input type="button" onclick="decrement('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$');" value="-" id="btnDecrement"/>-->
        <a onclick="decrement('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$');" class="norgtpadd" id="btnDecrement" style="cursor:pointer;"> - </a>
        <input type="text" value="1" onkeypress="return validateNumber(event);" name="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" id="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" class="nmbr_input"/>

        <!--<input type="button" onclick="increment('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$');" value="+" id="btnIncrement"/>-->
        <a onclick="increment('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$');" class="norgtpadd" id="btnIncrement" style="cursor:pointer;"> + </a>
      </div>
      <div class="addcart-fmly">
        <a id="$PRODUCT_ID$"  onclick="productbuy('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$','$PRODUCT_ID$');" class="btn btn-primary def-html" data-tooltip="#html-content$PRODUCT_ID$">Add To Cart</a>
      </div>
      </div>
      <!--<div style="display:none;" id="html-content$PRODUCT_ID$">
        $PRICE_TABLE$
      </div>-->
      <!--<table width="100px" border="0" cellspacing="0" cellpadding="0">
        <tbody>
          <tr>
            <td>
              <div>
                <input type="text" class="fprorowinp" id="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" size="1" maxlength="6" onkeydown="return keyct(event)" name="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" value="1"  onkeyup="callproductqtykeypress(event)" />
                <div class="costable">
                  <div class="pricepopup">
                    <div id="popupouterdivpricepop$PRODUCT_ID$" class="popupouterdivnone">
                      <div class="popupaero"></div>
                      $PRICE_TABLE$
                    </div>
                    <a id="pricepop$PRODUCT_ID$"  onclick="productbuy('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$','$PRODUCT_ID$');" class="btnbuy2 button smallsiz btngreen costable fpror"  onmouseover="javascript:test('pricepop$PRODUCT_ID$');" onmouseout="javascript:Mouseout('pricepop$PRODUCT_ID$');" >Buy  </a>
                  </div>
                </div>
              </div>
            </td>
          </tr>
        </tbody>
      </table>-->
      $endif$
    </td>
  </tr>
