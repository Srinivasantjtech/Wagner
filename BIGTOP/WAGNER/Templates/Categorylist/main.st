<div class="ctgry-headpanel clearfix">
  <div class="categoryheading">
    <h4>$TBT_CATEGORY_NAME$</h4>
  </div>
  
  <div class="sort-btns">
    <div id="grid-btn">
      <a  id="progrid" class="blue_text" onclick="changeGview();" style="cursor:pointer;">
        <span class="glyphicon glyphicon-th"></span>Grid
      </a>
    </div>
    <div id="list-btn">
      <a  id="prolist" class="" onclick="changeLview();" style="cursor:pointer;">
        <span class="glyphicon glyphicon-list"></span>List
      </a>
    </div>
 
    
  </div>
  <div class="pricesorter_wrap subject hidden-xs">   
    <input type="hidden" value="sort" name="subject"/>
      <li id="pricesorter" class="dropdown">
        <span style="vertical-align: middle; line-height: 30px">Sort By :</span>
        <a data-toggle="dropdown" class="dropdown-toggle" href="#" aria-expanded="false" style="display:block;">$SortBy$</a>
        <div class="dropdown-menu">
          <ul class="">
            <li>
              <a onclick="SetSortOrder('latest');">Latest</a>
            </li>
            <li>
              <a onclick="SetSortOrder('ltoh');">Price Low to High</a>
            </li>
            <li>
              <a onclick="SetSortOrder('htol');">Price High to Low</a>
            </li>
            <li>
              <a onclick="SetSortOrder('popularity');">Popular</a>
            </li>
          </ul>
        </div>
      </li>
    </div>
</div>
<div class="row">
  <div class="dummy" style="margin: 15px 15px 0 4px;">
    <div class="subcategory-selection clearfix hidden-sm hidden-xs" style="width:100%;">
      <div class="breadcrumb clearfix">
        <li>Sub Category Selection</li>
      </div>
      $TBWDataList$
    </div>
  </div>
</div>
