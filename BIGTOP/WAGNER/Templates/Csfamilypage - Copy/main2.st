<div class="col-md-9 imageblock-left">

  <div class="text-center zoomimg">
    $if(TBT_NOIMAGE)$
    <img data-cloudzoom="zoomImage:&quot;/prodimages$TBT_TFWEB_LIMAGE$&quot;,
                     zoomSizeMode: &quot;image&quot;,
                     tintColor:&quot;#000&quot;,
                     tintOpacity:0.25,
                     captionPosition:&quot;bottom&quot;,
                     maxMagnification:4,
                     autoInside:750
                     " title="" itemprop="image" src="/prodimages$TBT_TFWEB_IMAGE1$" id="zoom1" alt="Cloud Zoom small image" class="cloudzoom" style="width:256px;min-width: 100px;max-width:256px;max-height:256px;"/>
    $else$
    <img src="/prodimages/images/noimage.gif" style="max-width:256px;max-height:256px;"></img>
    $endif$
  </div>
  <span class="gen-spacer"></span>

  <div class="zoom_thump text-center">
    $TBT_MULTIIMAGES:{

    <div class=" thumb_img">
      <a href="/prodimages$it.LargeImage$" class="thumb-link">
        <img 
               data-cloudzoom="useZoom:&quot;#zoom1&quot;,
                 image:&quot;/prodimages$it.LargeImage$&quot;,
                 zoomImage:&quot;/prodimages$it.LargeImage$&quot;"
               title="" alt="Cloud Zoom thumb image"
               src="/prodimages$it.Thumpnail$" class="cloudzoom-gallery"/>
      </a>
    </div>
    }
    $
    <div class="clear"></div>
  </div>
  <span class="gen-spacer"></span>




</div>
<div class="col-md-9 imagedetail-right">
  <h1 id="familyName">$TBT_FAMILY_NAME$</h1>

  <div class="line"></div>

  <div class="pro-discrip col-lg-20 ">
    <p>
      $TBT_SHORT_DESCRIPTION$<br />
    </p>
    <p id="div1" style="display:block;">
      $TBT_MORE$
    </p>
    <p id="div2" style="display:none;">
      $TBT_DESCALL$
    </p>
    $if(TBT_MORE_SHOW)$
    <a class="fmly-showmore"  id="smlm" style="display:block;cursor:pointer;"  onclick="showmoredes();">Read / Show More</a>
    <a class="fmly-showmore"  id="lessmore" style="display:none;cursor:pointer;"  onclick="showmoredes();">Read / Less More</a>
    $endif$
    <div class="solid-border"></div>
    <div class="founded-ctgory">
      <img class="see-catgory" src="/images/see-catgory.png"/>
      <p>
        Found in Category : <a class="" href="$TBT_CT_HREF$">$TBT_CT_NAME$</a>
      </p>
    </div>
  </div>




</div>
<div class="clear"></div>