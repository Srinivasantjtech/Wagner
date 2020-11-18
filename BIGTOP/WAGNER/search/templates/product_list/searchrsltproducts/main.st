  <div class="ctgry-headpanel clearfix">
    <div class="categoryheading">
      <h4>$TBTC_CATEGORY_NAME$</h4>
      <p class="p2 push_left" > $TBTC_SHORT_DESC_CAT$ </p>
    </div>
    <div class="sort-btns hidden-xs">
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
        <span style="vertical-align: middle; line-height: 30px;">Sort By :</span>
        <a data-toggle="dropdown" class="dropdown-toggle" href="#" aria-expanded="false" style="display:block;">$SortBy$</a>
        <div class="dropdown-menu">
          <ul class="">
              <li>
              <a onclick="SetSortOrder('catalog');"  style="cursor:default" >Relevance</a>
            </li>
            <li>
              <a onclick="SetSortOrder('latest');"  style="cursor:default"  >Latest</a>
            </li>
            <li>
              <a onclick="SetSortOrder('ltoh');" style="cursor:default">Price Low to High</a>
            </li>
            <li>
              <a onclick="SetSortOrder('htol');" style="cursor:default" >Price High to Low</a>
            </li>
            <li>
              <a onclick="SetSortOrder('popularity');" style="cursor:default" ping="$TBT_REWRITEURL_REL$" >Popular</a>
            </li>
          </ul>
        </div>
      </li>
    </div>
  </div>

  <div class="row">
    
    <div class="home-product" id="home-product" style="padding:9px 0 0;" >
      $TBWDataList:{$it.TBWDataListItem$}$

      <!--<div class="clearfix text-center"></div>-->
      <div class="divLoadData"></div>
          <div class="clearfix"></div>
      <div class="pro_loading clearfix">
      <div class="text-center" id="tblload" style="display:none">
    
        <img src="$CDN$images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
        <br>Loading More Products.. Please Wait..</br>
      </div>
      </div>
    </div>
  </div>





