
$if(TBT_SUB_MENU)$
<li>
  <a href="$TBT_REWRITEURL$/mpl/"  ><h3 onclick="func_showChild($MENU_NO$)" class="h3_$MENU_NO$"><span></span>$TBT_CATEGORY_NAME$ ($TBW_PRO_CNT$)
  </h3>
</a>
  <ul>
    $TBT_SUBCATEGORY_LIST$ 
  </ul>
</li>
$else$
  <a href="$TBT_REWRITEURL$/mpl/" class="list-group-item" > $TBT_CATEGORY_NAME$ ($TBW_PRO_CNT$) </a>
$endif$