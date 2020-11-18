<div class="col-lg-4 col-sm-6 margin_top">
  <div class="text-center margin_bottom_15">
    $if(ISImageexist)$
    <img style="width:250px;min-width: 100px;" class="cloudzoom" alt ="$TBT_FAMILY_NAME$" id ="zoom1" src="$CDNROOT$prodimages$TBT_TFWEB_IMAGE1$"
                 title=""
                 data-cloudzoom='
                     zoomImage:"$CDNROOT$prodimages$TBT_TFWEB_LIMAGE$",
                     zoomSizeMode: "image",
                     tintColor:"#000",
                     tintOpacity:0.25,
                     captionPosition:"bottom",
                     maxMagnification:4,
                     autoInside:750
                     ' />
      $else$

      <img class="lazy" src="$CDNROOT$prodimages$TBT_TFWEB_IMAGE1$" alt="$TBT_FAMILY_NAME$" style="max-width:100px;max-height:100px;color:#fff;font-size:1px;" />
      $endif$
    </div>
  <span class="gen-spacer"></span>

  <div class="text-center">
    $if(ISImageexist)$
    $TBT_MULTIIMAGES:{
    <div class=" thumb_img">
      <a href="$CDNROOT$prodimages$it.LargeImage$" class="thumb-link">
        <img class="cloudzoom-gallery" width="50" height="50" src="$CDNROOT$prodimages$it.Thumpnail$"
             title=""
             alt ="$TBT_FAMILY_NAME$"
             data-cloudzoom='
                         useZoom:"#zoom1",
                         image:"$CDNROOT$prodimages$it.LargeImage$",
                         zoomImage:"$CDNROOT$prodimages$it.LargeImage$"' />

        </a>
    </div>
    }$
    $endif$

    <div class="clear"></div>
  </div>

  <span class="gen-spacer"></span>

</div>
<div class="col-lg-8 col-sm-6">
  <h3 class="bolder blue_color_text font_size_16" id="familyName">  $TBT_FAMILY_NAME$</h3>
  <div class="divider_details"></div>
  <span itemprop="description">
    <p class="details_para margin_top">
      $TBT_SHORT_DESCRIPTION$
    </p>
    <div id="div1" >

      <p class="details_para margin_top">$TBT_MORE$</p>
    </div>
    <div id="div2" style="display:none;" >

      <p class="details_para margin_top">$TBT_DESCALL$ </p>
    </div>

    <br/>
    $if(TBT_MORE_SHOW)$
    <!--<div id="preview" >
      <div id="smo" class="show_hide12">
        <a class="redmoew" >Read More! Show Full Description </a>
      </div>
      <div id="lmo" class="show_hide12" style="margin:10px 0px;">
        <a class="redmoewless"  >Show Less Description</a>
      </div>
    </div>-->
    $endif$
  </span>
  <a class="btnfamily"  title="$TBT_FAMILY_NAME$"  href="$TBT_REWRITEURL$/mfl/" rel="/mfl.aspx?fid=$TBT_FAMILY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;Path=$TBT_EA_PATH$">
    $TBT_PROD_COUNT$ of $TBT_FAMILY_PROD_COUNT$ Products Shown | View All Products in this Family
  </a>
  <br/>
</div>




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
  objDiv.style.left = MouseX - (objDiv.clientWidth - 100) + 'px';//alert(objDiv.style.left);
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
</script>