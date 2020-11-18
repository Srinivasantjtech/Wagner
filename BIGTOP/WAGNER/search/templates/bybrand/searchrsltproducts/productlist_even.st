<td>
  <div class="cat_list even">
    <div class="cat_list_img">
      $if(TBT_SUB_FAMILY)$<a href="$TBT_REWRITEURL$/fl/" 
 class="lvatag">
        <img class="lazy lvimg" data-original="$CDNROOT$prodimages$ATTRIBUTE_ID_453$" src="" alt="$FAMILY_NAME$"/>
      </a>$else$  <a href="$TBT_REWRITEURL$/pd/"  class="lvatag">
        <img class="lazy lvimg" data-original="$CDNROOT$prodimages$ATTRIBUTE_ID_452$" src="" alt="$ATTRIBUTE_ID_1$ $PROD_DESC_ALT$"/>
      </a>$endif$
    </div >
    <div class="cat_list_txt">
      <h6>
        $if(TBT_SUB_FAMILY)$<a href="$TBT_REWRITEURL$/fl/" class="tdn">
          $FAMILY_NAME$
        </a>$else$<a href="$TBT_REWRITEURL$/pd/"  class="tdn">$FAMILY_NAME$</a>$endif$
      </h6 ><p>
        $ATTRIBUTE_ID_62$$if(TBT_MORE_62)$$if(TBT_SUB_FAMILY)$...<a href="$TBT_REWRITEURL$/fl/"  class="tx_3">More Information</a>$else$ ...
        <a href="$TBT_REWRITEURL$/pd/"    class="tx_3">More Information</a>$endif$$endif$$ATTRIBUTE_ID_4$$if(TBT_MORE_4)$$if(TBT_SUB_FAMILY)$...<a href="$TBT_REWRITEURL$/fl/"  class="tx_3">More Information</a>$else$...<a href="$TBT_REWRITEURL$/pd/"  class="tx_3">More Information</a>$endif$ $endif$
      </p>$if(TBT_SUB_FAMILY)$<a href="$TBT_REWRITEURL$/fl/"  
       class="tx_3new">
        <img src="$CDN$images/ico_details.gif"  border="0" align="absmiddle" alt=""/>&nbsp;&nbsp;<strong  class="tcfw">View:$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products </strong>
      </a>$else$<a href="$TBT_REWRITEURL$/pd/"  
  class="tx_3new">
        <img src="$CDN$images/ico_details.gif"  border="0" align="absmiddle" alt=""/>&nbsp;&nbsp;<strong  class="tcfw">View:$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products </strong>
      </a>$endif$
    </div >
    <div class="cat_list_right11">
      $if(TBT_BUY_PRODUCT)$<div class="cat_list_cost">
        <a href="$TBT_REWRITEURL$/pd/" 
           class="hoverblack_1">
          <strong>Cost:</strong>&#36; $ATTRIBUTE_ID_5$<br/> <strong> Order No: </strong>$ATTRIBUTE_ID_1$
        </a>
      </div>$if(TBT_WITH_LOGIN)$<div class="divfw">
        <input name="txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$"  
               size="2" maxlength="6" type="text" 
               class="bbbuy" onkeydown="return keyct(event)" id="txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$" value="1" onkeyup="callproductqtykeypress(event)"/>
        <div class="costable">
          <div class="pricepopup">
            <div class="popupouterdiv1" id="pid$PRODUCT_ID$">
              <div class="popupaero"></div>$TBT_PRODUCT_PRICE_TABLE$
            </div>
            <a onclick="productbuy('txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$','$PRODUCT_ID$');" class="btnbuy button smallsiz btngreen costable curpoi"> Buy </a>
          </div>
        </div>
      </div>$else$$if(TBT_SUB_FAMILY)$
      <a href="$TBT_REWRITEURL$/fl/" 
         class="btnbuy button smallsiz btnblue">More Info</a>$else$<a href="$TBT_REWRITEURL$/pd/"  class="btnbuy button smallsiz btnblue">More Info</a>$endif$$endif$$else$<div class="cat_list_cost">
        <strong>Starting From:</strong>$TBT_MIN_PRICE$<br/><strong> Order No: </strong>$ATTRIBUTE_ID_1$
      </div> $if(TBT_SUB_FAMILY)$<a href="$TBT_REWRITEURL$/fl/" class="btnbuy button smallsiz btnblue">
        More Info
      </a>$else$<a href="$TBT_REWRITEURL$/pd/"  class="btnbuy button smallsiz btnblue">More Info</a>$endif$$endif$
    </div >
  </div >
</td>