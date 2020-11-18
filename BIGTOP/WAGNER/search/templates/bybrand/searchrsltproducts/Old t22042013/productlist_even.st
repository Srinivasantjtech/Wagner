<tr>
  <td height="26px"  align="center" valign="middle" width="100px">
    <a class="Zoomer" href="#thumb">
      <img src="/ProdImages$ATTRIBUTE_ID_453$"  style="max-height:100px;max-width:100px;cursor:pointer;" alt=""/>
      <div>
        <img src="/ProdImages$ATTRIBUTE_ID_453_LARGE$" alt=""></img>
      </div>
    </a>
  </td>
  <td width="100px" class="prodlisttext">$ATTRIBUTE_ID_1$</td>
  <td width="350px"  class="prodlisttext" valign="middle" align="center">
    <a href="/pd/&amp;pid=$PRODUCT_ID$&amp;fid=$FAMILY_ID$&amp;cid=$CATEGORY_ID$&amp;byp=2&amp;qf=1&amp;pcr=$PARENT_CATEGORY_ID$&amp;path=$PRODUCT_EA_PATH$" class="click_here_pd1" style="color:#008DEC">
      $ATTRIBUTE_ID_449$
    </a>
    <br/>
    $FAMILY_NAME$
  </td>
  <td width="150px"  valign="middle" align="center">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="37" valign="top" class="tx_1" colspan="3">
          $if(TBT_WITH_LOGIN)$
          <div id="pid$PRODUCT_ID$" style="background-color:#ffffff;visibility:hidden;position:absolute" > $TBT_PRODUCT_PRICE_TABLE$</div>
          <strong>
            <div onMouseOver="javascript:ShowPriceTable('pid$PRODUCT_ID$')" onMouseOut="javascript:ClosePriceTable('pid$PRODUCT_ID$')" style="position:relative">
            Price: &#36; $ATTRIBUTE_ID_5$<br/>
          </strong>
          $endif$
        </td>
      </tr>
      <tr>
        <td width="30px">
          $if(TBT_ECOMENABLED)$
          <input name="txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$" style="background-color:#FFFFFF;color: #000000;width:20px;" size="2" maxlength="6" type="text" onkeydown="return keyct(event)" id="txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$"/>
          $endif$
        </td>
        <td align="right" width="76px">
          $if(TBT_ECOMENABLED)$
          &nbsp;&nbsp;<a style="cursor:pointer;" onMouseOut="MM_swapImgRestore();ClosePriceTable('pid$PRODUCT_ID$')" onMouseOver="MM_swapImage('Image$PRODUCT_ID$_$FAMILY_ID$_pp','','images/but_buy2.gif',1);ShowPriceTable('pid$PRODUCT_ID$')">
            <!--<img src="images/but_buy1.gif" style="display:$TB_DISPLAY$" name="Image$PRODUCT_ID$_$FAMILY_ID$_pp" width="76px" height="28" border="0" onClick="productbuy('txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$','$PRODUCT_ID$');"/>-->
            <img class="but_buy1" style="display:$TB_DISPLAY$" name="Image$PRODUCT_ID$_$FAMILY_ID$_pp" width="76px" height="28" border="0" onClick="productbuy('txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$','$PRODUCT_ID$');"/>
          </a>
       
          $endif$
        </td>
      </tr>
    </table>    
  </td>
</tr>
