
$if(TBT_SUB_MENU)$
<li class="category">  <a href="/$TBT_REWRITEURL$/mpl/"   style="Color:#fff;padding-left:0px; transition:all ease 1s;" > <span>$TBT_CATEGORY_NAME$</span>        </a>
<ul style="display: $ISFIRST_MENU$;" class="sub-menu">
    <p class="green_clr bolder font_15 sub_menu_head"> 
	<a href="/$TBT_REWRITEURL$/mpl/"  style=" color: #3ab54a; padding-left:0px; transition:all ease 1s;" > $TBT_CATEGORY_NAME$  </a> </p>
	<li class="col-sm-4 col-lg-3">
<ul class="d_menu">
$TBT_SUBCATEGORY_LIST$ 
</ul>
</li>
 <li class="col-sm-4 col-lg-3 margin_left_right_50">
                  <ul class="d_menu">
				  $TBT_SUBCATEGORY_LIST2$ 
				  </ul>
				  </li>
				<li class="col-sm-4 col-lg-3">
                  <ul class="d_menu">
				  $TBT_SUBCATEGORY_LIST3$ 
				  </ul>
				  </li>
				  </ul>
				  </li>
$else$
  	  <!--<li class="category hidemenu-list" id="acc">  <a href="/$TBT_REWRITEURL$/mpl/"   style="Color:#fff;padding-left:0px; transition:all ease 1s;" > <span>$TBT_CATEGORY_NAME$</span>        </a>
	</li>-->

	<li class="category hidemenu-list" >  <a href="/$TBT_REWRITEURL$/mpl/"   style="Color:#fff;padding-left:0px; transition:all ease 1s;" > <span>$TBT_CATEGORY_NAME$</span>        </a>
	</li>
$endif$