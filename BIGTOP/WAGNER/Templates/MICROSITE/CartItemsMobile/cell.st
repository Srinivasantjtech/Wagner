<li>
  <a href="$TBT_REWRITEURL$" data-bind="attr: { href: Product ? Product.SEOUrl : &#39;&#39; }">
    <img class="product" alt="" data-bind="attr: { src: Product ? Product.PrimaryProductImage.Thumbnail_LowRes.Uri : &#39;&#39;, alt: Product ? Product.ProductName : &#39;&#39; }" />
    <p class="title">
      Order-Code : $CODE$</br>
      $FAMILY_NAME$
     
    </p>
    <span class="cost">
      <span class="currency">&#36;</span>$COST$
      <sup>

      </sup>
    </span>
  </a>
  <img class="product" alt="" data-bind="attr: { src: Product ? Product.PrimaryProductImage.Thumbnail_LowRes.Uri : &#39;&#39;, alt: Product ? Product.ProductName : &#39;&#39; }" />
</li>