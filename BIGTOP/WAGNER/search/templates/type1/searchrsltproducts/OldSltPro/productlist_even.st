﻿<TD align="middle">  
  <input id="Chkbox_$PRODUCT_ID$" type="checkbox" name="AddItem" value="$PRODUCT_ID$"  onclick="CheckAddCount(document.forms[0].AddItem,'Chkbox_$PRODUCT_ID$');" />
</TD>
<TD>
  <DIV style="TEXT-ALIGN: center">
    <a href="/pd/&pid=$PRODUCT_ID$&fid=$FAMILY_ID$">
      <img src="$CDNROOT$prodimages$ATTRIBUTE_ID_452$"/>
    </a>
  </DIV>
</TD>
<TD class ="tx_searchfamily">
  <DIV style="TEXT-ALIGN: left">
    <a href="/flFid=$FAMILY_ID$">
      <b>$FAMILY_NAME$</b>
    </a>
    <br/>
    $ATTRIBUTE_ID_4$
  </DIV>
</TD>
<TD>
  <DIV id="ctl00_ContentMain_SearchResultsGrid_grid_ctl03_ctl08_pnlNotCompliant">
    <a href="/pd/&pid=$PRODUCT_ID$&fid=$FAMILY_ID$">$ATTRIBUTE_ID_1$</a>
  </DIV>
</TD>
<TD>
  <DIV
  id="ctl00_ContentMain_SearchResultsGrid_grid_ctl03_ctl08_pnlNotCompliant">
    &#36; $TBT_USER_PRICE$
  </DIV>
</TD>
<TD class="SearchResultsBuyColumn">
  <DIV id="ctl00_ContentMain_SearchResultsGrid_grid_ctl03_ctl07_pnlQuantity">
    <BR/>
    <a href="/OrderDetails.aspx?&bulkorder=1&Pid=$PRODUCT_ID$&Qty=$MIN_ORD_QTY$&ORDER_ID=$TBT_ORDER_ID$" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image$PRODUCT_ID$_$FAMILY_ID$aps','','images/but_buy2.gif',1)">
      <!--<img src="images/but_buy1.gif" name="Image$PRODUCT_ID$_$FAMILY_ID$aps" width="76" height="25" border="0"/>-->
      <img class="but_buy1" name="Image$PRODUCT_ID$_$FAMILY_ID$aps" width="76" height="25" border="0"/>
    </a>
  </DIV>
</TD>
<TD>
  <DIV
  id="ctl00_ContentMain_SearchResultsGrid_grid_ctl03_ctl08_pnlNotCompliant">
    $ATTRIBUTE_ID_3$
  </DIV>
</TD>