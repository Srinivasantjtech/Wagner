
      <div class="col-md-9 imageblock-left">

        <div class="text-center zoomimg" id="product-image" >
          $if(TBT_NOIMAGE)$
          <img data-cloudzoom="zoomImage:&quot;$TBT_TFWEB_ZOOM$&quot;,
                     zoomSizeMode: &quot;image&quot;,
                     tintColor:&quot;#000&quot;,
                     tintOpacity:0.25,
                     captionPosition:&quot;bottom&quot;,
                     maxMagnification:4,
                     autoInside:750
                     " title="" itemprop="image" src="$TBT_TFWEB_IMAGE1$" id="zoom1" alt="$TBT_FAMILY_NAME$" class="cloudzoom" />
          $else$
          <img src="$CDNROOT$prodimages/images/noimage.gif" style="max-width:256px;max-height:256px;" class="lazy" alt="$TBT_FAMILY_NAME$"></img>
          $endif$
        </div>
        <div class="prodiv" id="productvideo" style="display:none;min-width: 100px;height:512px;max-width:512px;max-height:512px;text-align:center">
          <iframe id="ifrme" width="100%" height="450px"  src="$youtubelink$" frameborder="0" allowfullscreen="">
          </iframe>
        </div>
        <span class="gen-spacer"></span>

        <div class="zoom_thump text-center">
          $TBT_MULTIIMAGES:{
          
         <div class=" thumb_img" onclick="Imageshow();">
          <a href="$it.Image1024$" class="thumb-link">
            <img 
                   data-cloudzoom="useZoom:&quot;#zoom1&quot;,
                 image:&quot;$it.Image1024$&quot;,
                 zoomImage:&quot;$it.Image1024$&quot;"
                   title="" alt="$TBT_FAMILY_NAME$"
                   data-original="$it.Thumpnail$" class="cloudzoom-gallery lazy"/>
          </a>
        </div>

          }
          $
          $if(ISYOUTUBELIKE)$
          <div id="video"  onclick="videoshow();">
            <ul>
              <li height="112px" width="150px">
                <img data-original="$YouTubeImg$" class="lazy" style=" width:90px;height:90px;cursor:pointer;" />
                <img data-original="$CDN$images/playbtn.png" class="playbtn lazy" style="left: -69px;position: relative; top: 26px;width:30px;height:30px;cursor:pointer;"/>
              </li>
            </ul>
          </div>
          $endif$
          <div class="clear"></div>
        </div>
        <span class="gen-spacer"></span>




      </div>
  <div class="col-md-9 imagedetail-right">
    <h1 id="familyName" style="font-family:Arial, Helvetica, sans-serif !important">$TBT_FAMILY_NAME$</h1>

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
        <img class="see-catgory" src="$CDN$images/see-catgory.png"/>
          <p>
            Found in Category : <a class="" href="$TBT_CT_HREF$">$TBT_CT_NAME$</a>
          </p>
        </div>
    </div>




  </div>
  <div class="clear"></div>
