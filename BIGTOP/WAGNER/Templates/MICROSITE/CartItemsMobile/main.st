

$if(TBT_IS_CART)$
<div id="cartSummaryPanel" style="display:block">
  <div class="summary">
    <h2>Cart Summary</h2>
    <a class="btn alt-2 padding_none" href="$VIEW_ORDER$">
      <span>View Cart</span>
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
      <span class="currency">&#36;</span>$CART_AMOUNT$

      <sup>
      
      </sup>
    </p>
    <a href="$CHECKOUT$" class="btn icon arrow padding_none alt-2">
      <span>Checkout</span>
    </a>
  </div>
</div>
$else$
<div id="emptyCart" class="emptycart" style="display:block">
  <h2>There are no items in your cart</h2>
  <div class="clear"></div>
</div>
$endif$