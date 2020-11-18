
$if(SHOW_CART)$
 
  $TBWDataList:{$it.TBWDataListItem$}$
  <li class="padding_10 width_100 text-center green_bg btn_absolute ">
    <a href="$VIEW_ORDER$" style="color:#fff;" class="white_color font_weight" rel="nofollow">View Cart</a>
  </li>
  $else$
    <!-- <li class="padding_10 border_bottom padding_bottom15 padding_top15" style=" padding-bottom: 15px; padding-top: 15px;padding: 10px;border-bottom: thin solid #ddd;">
  <div class="pull-left margin_left15 drop_width">
      <div>
        <strong style="color:#434a4f;"> No Items Added </strong>
      </div>
  </div>
  <div class="clear"></div>
</li> -->
  $endif$
       
    
   
