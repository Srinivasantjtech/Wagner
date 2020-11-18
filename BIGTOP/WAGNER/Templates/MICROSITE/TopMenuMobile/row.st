 $if(TBT_SUB_MENU)$
 <li class="brands" id="SUBmenu$MENU_NO$"   >
      <h3> <a onclick="submenuClick('#SUBmenu$MENU_NO$')" > $TBT_CATEGORY_NAME$ </a> </h3>
      <div>
        <div class="grid">
          <div class="subcats">
            <ul class="main">
				$TBT_SUBCATEGORY_LIST$ 
             
             
            </ul>
       
          
          </div>
        </div>
      </div>
    </li>
$else$
	 <li class="brands1"><h3><a href="/$TBT_REWRITEURL$/mpl/"> $TBT_CATEGORY_NAME$</a> </h3></li>  	
$endif$