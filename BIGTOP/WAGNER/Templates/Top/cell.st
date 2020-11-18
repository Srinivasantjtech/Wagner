$if(IS_SUBCAT)$
<li >
 <a href="$TBT_REWRITEURL$/ct/"> $TBT_CATEGORY_NAME$<b class="glyphicon glyphicon-menu-down"></b></a>
<div class="drop">
<div class="drop-holder">
<div class="frame">
<ul>
	  $SUBCAT_LIST$
 </ul> </div></div></div>


</li>
$else$
<li >
  <a href="$TBT_REWRITEURL$/ct/" >
    $TBT_CATEGORY_NAME$
  </a>
</li>

$endif$