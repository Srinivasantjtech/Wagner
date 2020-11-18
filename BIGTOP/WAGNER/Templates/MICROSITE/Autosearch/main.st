


  
    <div class="col-sm-7 bdr_right">
    
      <div class="srch_suggestion clearfix">
        <div class="heading_box clearfix">
          <h3 class="srch_heading">Products</h3>
          <p class="srch_total">Total $STRTOTAL$</p>
        </div>
        <hr>
          $TBWDataList:{$it.TBWDataListItem$}$
          <li class="text-center">
            <a href="$STRSEARCHURL$" class="">View All</a>
          </li>
        </hr>
      </div>
    </div>
 
  <div class="col-sm-5">
    <div class="srch_category">
      <div class="heading_box">
        <h3 class="srch_heading">Category</h3>
      </div>
      $TBWDataList_Category:{$it.TBWDataListItem$}$
    </div>
    <div class="srch_result">
      <div class="heading_box">
        <h3 class="srch_heading">Suggested Key Words</h3>
      </div>
      $TBWDataList_Sugg:{$it.TBWDataListItem$}$
    </div>
    <div class="srch_price">
      <div class="heading_box">
        <h3 class="srch_heading">Price</h3>
      </div>
      $TBWDataList_Price:{$it.TBWDataListItem$}$
    </div>
  </div>

