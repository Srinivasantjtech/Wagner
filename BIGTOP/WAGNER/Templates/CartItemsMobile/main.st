

$if(TBT_IS_CART)$
<div id="cartSummaryPanel" style="display:block">
  <div class="summary">
    <h2>Cart Summary</h2>
    <a class="btn alt-2 padding_none" href="$VIEW_ORDER$">
      View Cart
    </a>
  </div>
  <ul data-bind="foreach: LineItems">
    $TBWDataList:{$it.TBWDataListItem$}$
  </ul>
  <div class="total">
    <h3>
      Subtotal <span>(excluding delivery)</span>
    </h3>
    <p>
      <span class="currency">&#36; $CART_AMOUNT$</span>

      <sup>
        <!--ko text: TotalDecimalPrice-->
        <!--/ko-->
      </sup>
    </p>
    <a href="$CHECKOUT$" class="chkout" >
      Checkout
    </a>
  </div>
</div>
$else$
<div id="emptyCart" class="emptycart" style="display:block;">
  <h2>No Items Added</h2>
  <div class="clear"></div>
</div>
$endif$