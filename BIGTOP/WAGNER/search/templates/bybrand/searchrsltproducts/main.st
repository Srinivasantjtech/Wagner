<div class="ctgry-headpanel clearfix">
  <div class="categoryheading">
    <h4>$TBT_TOSUITE_BRAND$&nbsp;$TBT_TOSUITE_MODEL$</h4>
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
            <a onclick="SetSortOrder('latest',this);"  style="cursor:default" ping="$TBT_REWRITEURL_REL$">Latest</a>
          </li>
          <li>
            <a onclick="SetSortOrder('ltoh',this);"  style="cursor:default" ping="$TBT_REWRITEURL_REL$">Price Low to High</a>
          </li>
          <li>
            <a onclick="SetSortOrder('htol',this);"  style="cursor:default" ping="$TBT_REWRITEURL_REL$">Price High to Low</a>
          </li>
          <li>
            <a onclick="SetSortOrder('popularity',this);"  style="cursor:default" ping="$TBT_REWRITEURL_REL$">Popular</a>
          </li>
        </ul>
      </div>
    </li>
  </div>
</div>
<div class="subct-selection">
  <div class="subcategory-selection clearfix">
    <div class="bybrand_wrap">
      <div class="bybrant_img">
        <img alt="img" src="$CDNROOT$prodimages$TBT_TOSUITE_MODEL_IMAGE$"/>
  	</div>
    </div>
    <div class="bybrandtxt_wrap">
      <div class="bybrant_head clearfix">
        <h4 class="blue_text"> $TBT_TOSUITE_BRAND$&nbsp;$TBT_TOSUITE_MODEL$&nbsp;Accessories&nbsp;Product&nbsp;List</h4>
        <div class="cat_list_info tal">
          See Below for Cellink's range of Accessories to suit the $TBT_TOSUITE_BRAND$&nbsp;$TBT_TOSUITE_MODEL$
        </div>
      </div>
  </div>
  </div>
</div>
<div class="subct-selection">
  <div class="subcategory-selection clearfix">
  $if( TBT_SHOW_FT)$
  <div class="powersearch_2 text-center clearfix">
    <div id="Rcatdiv" class="powerselect-wrap">
      <label>Filter Products By Category:       </label>
      <select class="psfilter1" id="ddlcategory"  name="lst1" onchange="geturl();">
        $TBWDataList1:{$it.TBWDataListItem1$}$
      </select>
    </div>
  </div>
    $if(TBT_SHOW_FTSCat)$
    <div class="powersearch_2 text-center clearfix">
      <div id="Scatdiv"  class="powerselect-wrap" >
        <label>Filter Products By SubCategory:</label>
        <select class="psfilter2" id="ddlsubcategory" name="lst2" onchange="geturlsubcat();">
          $TBWDataList2:{$it.TBWDataListItem2$}$
        </select>
      </div>
    </div>
    $endif$
    
    <p class="powerselect-wrap hidden-xs">Note. See Addtional Filtering Options in left side Navigation.</p>

    $endif$

</div>
</div>

<div class="row">
  <div class="home-product" id="home-product">
    $TBWDataList:{$it.TBWDataListItem$}$

    <!--<div class="clearfix text-center"></div>-->
    <div class="divLoadData"></div>
    <div class="clearfix"></div>
    <div class="pro_loading clearfix">
      <div class="text-center" id="tblload" style="display:none">
        <img src="$CDN$/images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
        <br>Loading More Products.. Please Wait..</br>
      </div>
    </div>
  </div>
</div>



