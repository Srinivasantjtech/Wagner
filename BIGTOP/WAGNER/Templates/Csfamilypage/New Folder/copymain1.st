<div class="product_top">
  <div id="lboximgpopup">
  <div class="family_img">
  <div id="product-image" class="gallery-view fimggv">
    <a href="$CDN$prodimages$TBT_TFWEB_LIMAGE$" >
      <img data-original="$CDN$prodimages$TBT_TFWEB_IMAGE1$" class="lazy flimg" alt="$TBT_FAMILY_NAME$"   id="popupmaindel"/>
    </a>
  </div>
  <div class="product-info">
    <div class="brand"></div>
    <div class="gallery-thumbnails">
      <div id="gallery">
        <ul style="list-style: none outside none;">$TBT_MULTIIMAGES:{
        <li style="height:50;width:50;"><a href="$CDN$prodimages$it.LargeImage$">
           <img data-original="$CDN$prodimages$it.Thumpnail$"  class="lazy fimg" alt="$TBT_FAMILY_NAME$" />
        </a> </li> 
          }$</ul></div>  </div>
  </div></div>
  </div>
  <div class="family_txt">
    <h1 itemprop="name"  class="pad10 proh1" id="familyName">$TBT_FAMILY_NAME$</h1>
    <span itemprop="description">
      <p> $TBT_SHORT_DESCRIPTION$</p>
      <div id="div1" class="fdes1">
        <p> $TBT_MORE$</p>
      </div>
      <div id="div2" class="fdes1" style="display:none;">
        <p>$TBT_DESCALL$</p>
      </div>$if(TBT_MORE_SHOW)$<div id="preview" >
        <div id="smo" class="show_hide12">
          <a class="redmoew" >Read More! Show Full Description </a>
        </div>
        <div id="lmo" class="show_hide12" style="margin:10px 0px;">
          <a class="redmoewless"  >Show Less Description</a>
        </div>
      </div> $endif$
    </span>
      <br/></div><div class="clear"></div></div>
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
  objDiv.style.left = MouseX - (objDiv.clientWidth -100) + 'px';
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