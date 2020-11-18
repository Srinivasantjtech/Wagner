<td bgcolor="">
  <div class="cat_list even">
    $if(TBT_SUB_FAMILY)$<div class="cat_list_img">
      <a href="$TBT_REWRITEURL$/fl/"   class="lvatag">
        <!--<img  alt="" src="/prodimages$ATTRIBUTE_ID_747$">-->
        <img class="lazy lvimg"    data-original="$CDN$prodimages$ATTRIBUTE_ID_747$" alt="$FAMILY_NAME$" />
      </a>
    </div >$else$
    <div class="cat_list_img">
      <a href="$TBT_REWRITEURL$/pd/"   class="lvatag">
        <!--<img    alt="" src="/prodimages$ATTRIBUTE_ID_747$">-->
        <img class="lazy lvimg"  data-original="$CDN$prodimages$ATTRIBUTE_ID_747$" alt="$ATTRIBUTE_ID_1$ $PROD_DESC_ALT$"  />
      </a>
    </div >$endif$<div class="cat_list_txt">
      $if(TBT_SUB_FAMILY)$<strong>
        <a href="$TBT_REWRITEURL$/fl/"  class="tdn">$FAMILY_NAME$</a>
      </strong>$else$<strong>
        <a href="$TBT_REWRITEURL$/pd/"  class="tdn">$FAMILY_NAME$</a>
      </strong>$endif$<p>
        $ATTRIBUTE_ID_13$$if(TBT_MORE_13)$$if(TBT_SUB_FAMILY)$...<a href="$TBT_REWRITEURL$/fl/"   class="tx_3">More Information</a>$else$...
        <a href="$TBT_REWRITEURL$/pd/"  class="tx_3">More Information</a>$endif$$endif$
      </p>
      <p>
        $ATTRIBUTE_ID_90$$if(TBT_MORE_90)$$if(TBT_SUB_FAMILY)$...<a href="$TBT_REWRITEURL$/fl/"   class="tx_3">More Information</a>$else$...
        <a href="$TBT_REWRITEURL$/pd/"  class="tx_3">More Information</a>$endif$ $endif$
      </p>
      <p>
        $ATTRIBUTE_ID_91$$if(TBT_MORE_91)$$if(TBT_SUB_FAMILY)$...<a href="$TBT_REWRITEURL$/fl/" class="ctdn">More Information</a>$else$...
        <a href="$TBT_REWRITEURL$/pd/"  class="ctdn">More Information</a> $endif$$endif$
      </p>
      <p>
        $ATTRIBUTE_ID_377$$if(TBT_MORE_377)$$if(TBT_SUB_FAMILY)$...<a href="$TBT_REWRITEURL$/fl/"   class="ctdn">More Information</a>$else$...
        <a href="$TBT_REWRITEURL$/pd/"   class="ctdn">More Information</a>$endif$$endif$
      </p>
      <p>
        $ATTRIBUTE_ID_379$$if(TBT_MORE_379)$$if(TBT_SUB_FAMILY)$...<a href="$TBT_REWRITEURL$/fl/"  class="ctdn">More Information</a>$else$...
        <a href="$TBT_REWRITEURL$/pd/"  class="ctdn">More Information</a>
        $endif$$endif$
      </p><p>
        $ATTRIBUTE_ID_4$$if(TBT_MORE_4)$$if(TBT_SUB_FAMILY)$...<a href="$TBT_REWRITEURL$/fl/"   class="ctdn">More Information</a>$else$ ...
        <a href="$TBT_REWRITEURL$/pd/"   class="ctdn">More Information</a>$endif$$endif$
      </p>
      <div class="test123">
        $if(TBT_SUB_FAMILY)$<a href="$TBT_REWRITEURL$/fl/"    class="tx_3new">
          <img src="$CDN$images/ico_details.gif"  border="0" align="absmiddle" />&nbsp;&nbsp;<strong class="tcfw">View:$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products</strong>
        </a>$else$<a href="$TBT_REWRITEURL$/pd/"    class="tx_3new">
          <img src="$CDN$images/ico_details.gif"  border="0" align="absmiddle" />&nbsp;&nbsp;<strong class="tcfw">View:$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products</strong>
        </a>$endif$
      </div>
    </div ><div class="cat_list_right">
      $if(TBT_BUY_PRODUCT)$<div class="cat_list_cost">
        <a class="hoverblack_1 tcfw"  href="$TBT_REWRITEURL$/pd/">
          <strong>Cost:</strong>&#36;$TBT_USER_PRICE$<br/><strong> Order No:</strong> $ATTRIBUTE_ID_1$
        </a>
        <br/>
      </div > $if(TBT_ECOMENABLED)$<div >
        <input name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" class="lvinput" maxlength="6" size="2" type="text" onkeydown="return keyct(event)" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" value="1" onkeyup="callproductqtykeypress(event)"/>
        <div class="costable">
          <div class="pricepopup">
            <div class="popupouterdiv1">
              <div class="popupaero"></div>$TBT_PRODUCT_PRICE_TABLE$
            </div>
            <a  class="btnbuy button smallsiz btngreen costable curpoi" onclick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');">Buy</a>
          </div>
        </div>
      </div>$else$$if(TBT_SUB_FAMILY)$ <a  href="$TBT_REWRITEURL$/fl/"  class="btnbuy button smallsiz btnblue"> More Info </a>$else$<a href="$TBT_REWRITEURL$/pd/" class="btnbuy button smallsiz btnblue"> More Info  </a>$endif$$endif$$else$<div class="cat_list_cost">
        <strong>
          Starting From:
        </strong>$TBT_MIN_PRICE$<br/><strong>Order No: </strong>$ATTRIBUTE_ID_1$ <br/>
      </div> $if(TBT_SUB_FAMILY)$<a href="$TBT_REWRITEURL$/fl/"    class="btnbuy button smallsiz btnblue">
        More Info
      </a>$else$<a href="$TBT_REWRITEURL$/pd/"    class="btnbuy button smallsiz btnblue"> More Info   </a>$endif$$endif$
    </div>
  </div >
</td>