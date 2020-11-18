<script language="javascript" type="text/javascript">
  var IE = document.all?true:false;
  var MouseX = 0;
  var MouseY = 0;
  if (!IE)
  {
  document.captureEvents(Event.MOUSEMOVE);
  document.onmousemove = getMouseXY;
  }
  function ShowPriceTable(objectid)
  {
  var objDiv = document.getElementById(objectid);
  objDiv.style.visibility = "visible";
  if (IE)
  {
  getMouseXY();
  objDiv.style.left = MouseX - (objDiv.clientWidth + 5) + 'px';
  objDiv.style.top = MouseY - (objDiv.clientHeight + 25) + 'px';
  }
  else
  {
  objDiv.style.left = MouseX - (objDiv.clientWidth + 5) + 'px';
  objDiv.style.top = (MouseY - (objDiv.offsetHeight + 50)) + 'px';
  }
  objDiv.style.zIndex = 1;
  objDiv.style.padding = "5px";
  }
  function getMouseXY(e)
  {
  if (IE) { // grab the x-y pos.s if browser is IE
  tempX = event.clientX + document.documentElement.scrollLeft;
  tempY = event.clientY + document.documentElement.scrollTop;
  } else {  // grab the x-y pos.s if browser is NS
  tempX = e.pageX;
  tempY = e.pageY;
  }
  MouseX = tempX;
  MouseY = tempY;
  }
  function ClosePriceTable(objectid)
  {
  var objDiv = document.getElementById(objectid);
  objDiv.style.visibility = "hidden";
  }
  function GridviewOnclick() {
 
  var ur = document.getElementById("ctl00_MainContent_Productlist1_hfnewurl");

  var url=ur.value;

  window.document.cookie= "GLVIEWMODE" + "=" + "GV";
  // alert(url);
  window.location.href =  url;
  }

  function ListviewOnclick() {
  // alert("y");
  var ur = document.getElementById("ctl00_MainContent_Productlist1_hfnewurl");
  var url=ur.value;

  window.document.cookie = "GLVIEWMODE" + "=" + "LV";
  //alert(url);
  window.location.href = url;
  }

</script>

<div class="col-lg-9 col-md-9 col-sm-8 col-xs-12 padding_left_right">
  <div class="col-lg-12 blue_color our_pro_head white_color margin_bottom_15 clearfix hidden-xs">
    $TBWC_CATEGORY_NAME$
    
    <div class=" pull-right sort">
      <a  id="list"  class=" font_size_16 pointer">
        <span class="glyphicon glyphicon-th-list color_change1"></span>
      </a>
      <a  id="grid" class=" font_size_16 margin_left pointer  yellow">
        <span class="glyphicon glyphicon-th color_change"></span>
      </a>
      <span style="float: left; vertical-align: middle; line-height: 25px; margin-right:10px;">Sort By :</span>
      <div class="dropdown_filter subject">
        <span class="selected">$SortBy$</span>

        <ul style="display: none;">
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
    </div>
  </div>
  $ST_TOP_FILTER$
  <div class="row">
    <div id="products" class="list-group">
      $TBWDataList:{ $it.TBWDataListItem$}$
      <div class="divLoadData" style="float:left; margin-left:300px;" ></div>
   
    </div>
  </div>
  <div class="text-center" id="tblload">
    <img src="$CDN$images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
    <br>Loading Data.. Please Wait..</br>
  </div>
  <div class="text-right" >
    <a class="scrollup gray_40 " href="">Top</a>
  </div>
</div>