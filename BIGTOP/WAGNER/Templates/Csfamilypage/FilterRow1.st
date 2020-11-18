$if(ATTRIBUTE_SELECTED)$
<option value="$ATTRIBUTE_VALUE$" selected="selected">$ATTRIBUTE_NAME$ ($ATTRIBUTE_VALUE_CNT$)</option>
$else$
$if(ATTRIBUTE_DISABLED)$
<option value="$ATTRIBUTE_VALUE$" disabled="disabled">$ATTRIBUTE_NAME$ ($ATTRIBUTE_VALUE_CNT$)</option>
$else$
<option value="$ATTRIBUTE_VALUE$" >$ATTRIBUTE_NAME$ ($ATTRIBUTE_VALUE_CNT$)</option>
$endif$
$endif$

