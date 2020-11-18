<div itemscope="" itemtype="http://schema.org/Product">
<div class="col-md-9 imageblock-left">
  <div class="text-center zoomimg" id="product-image" >
    $if(TBT_NOIMAGE)$
    <img class = "cloudzoom" src = "$TBT_TWEB_IMAGE12$"
            data-cloudzoom = "zoomImage: '$TBT_TWEB_IMAGE12_LARGE$'" alt="$TBT_FAMILY_NAME$" />
   
    $else$
    <img src="$CDN$prodimages/images/noimage.gif" style="max-width:256px;max-height:256px;" class="lazy" alt="$TBT_FAMILY_NAME$"></img>
    $endif$
    </div>
  <div class="prodiv" id="productvideo" style="display:none;min-width: 100px;height:512px;max-width:512px;max-height:512px;text-align:center">
    <iframe id="ifrme" width="100%" height="450px"  src="$youtubelink$" frameborder="0" allowfullscreen="">
    </iframe>
  </div>
  <span class="gen-spacer"></span>
  <div class="zoom_thump text-center">
   
      $TBT_MULTIIMAGES:{
  
    <div class=" thumb_img" onclick="Imageshow();" >
      <a href="$it.Image1024$" class="thumb-link">
        <img    data-cloudzoom="useZoom:&quot;.cloudzoom&quot;,
                 image:&quot;$it.Image1024$&quot;,
                 zoomImage:&quot;$it.Image1024$&quot;"
               title="" alt="$TBT_FAMILY_NAME$"
               src="$it.Thumpnail$" class="cloudzoom-gallery"/>
        </a>
    </div>
  
    }
    $
    $if(ISYOUTUBELIKE)$
    <div id="video"  onclick="videoshow();">
      <ul>
        <li height="112px" width="150px">
          <img data-original="$YouTubeImg$" style=" width:90px;height:75px;cursor:pointer;" class="lazy" />
          <img data-original="$CDN$images/red_playbtn.png" class="red_play lazy" style="cursor:pointer;"/>
        </li>
      </ul>
    </div>
    $endif$
    <div class="clear"></div>
  </div>
  <span class="gen-spacer"></span>
</div>
<div class="col-md-9 imagedetail-right">
  $if(TBT_BRAND_NAME)$ 
  <meta itemprop="brand" content="$BRAND_NAME$" />
  $endif$
  <h1 itemprop="name">$TBT_FAMILY_NAME$</h1>
  <input id="TBT_PRODUCT_ID" value="$TBT_PRODUCT_ID$" style="display:none;" readonly="readonly"></input>
  <div class="line"></div>

  $if(TBT_REPLACED)$
  $if(TBT_NIL_REPLACED)$
      <div class="col-lg-20">
    <div class="col-lg-5 col-sm-7" itemprop="offers" itemscope="" itemtype="http://schema.org/Offer">
      <meta itemprop="category" content="$PRODUCT_CATEGORY_NAME$" />
      <ul>
        <meta itemprop="priceCurrency" content="AUD"/>
        <h2 class="prodlprice">$TBT_COST$</h2>  
        <meta itemprop="price" content="$TBT_META_COST$"/>
        <link itemprop="availability" href="$TBT_STOCK_HREF$"/>
        
      </ul>

    </div>

    <div class="col-lg-15 col-sm-13 paddrt0">


      <div class="col-lg-20 paddrt0">
        <div class="pro-discrip-cartbtn alignboth">

          <div class="form-inline">

            $if(TBT_HIDE_BUY)$
          


            $else$
            <div class="form-group">
              <div class="number-box">

                <a onclick="decrement('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$');" id="btnDecrement" style="cursor:pointer;"> - </a>
                <input id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"  size="5"  maxlength="6" type="text" class="nmbr_input" onkeydown="return keyct(event)"  value="1"  onkeypress="return validateNumber(event);"/>


                <a onclick="increment('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$');" id="btnIncrement" style="cursor:pointer;"> + </a>
              </div>
             
              <button type="button" style="cursor:pointer;" class="btn btn-primary" onclick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');">Add to Cart</button>
            
            </div>
          </div>
        </div>
      </div>



    </div>
  </div>
      <div class="col-lg-20 paddt_b">
        <div class="col-lg-12">
          <li class="">
            <span>Order Code:</span> $TBT_REP_NIL_CODE$
          </li>
        </div>
        <div class="col-lg-8">
          <li>
            <span style="margin-left:-8px;">Availability: </span><!--<link itemprop="availability" href="$TBT_STOCK_HREF$"/>-->
            $TBT_REP_STATUS$ 
           
          </li>
        </div>
      </div>
      $else$
      <div class="pro-discrip col-lg-20">
        <div class="col-lg-20 nopadd clearfix">
          <div class="col-lg-13 col-md-13 nolftpadd">
            <p>
              $TBT_SHORT_DESCRIPTION$
            </p>
            <li id="div1" style="display:block;">
              $if(TBT_PROD_DESC_SHOW)$
              $TBT_DESCRIPTION$
              $endif$
              $TBT_MORE$
            </li>
            <li id="div2" style="display:none;">
              $TBT_DESCALL$
            </li>
          </div>
          <div class="col-lg-7 col-md-7 norgtpadd">
        <div class="substitute-wrap">

          <div class="substitute_head">
            <div class="clearfix">
              <img src="$CDN$images/subproduct_icon.jpg">
                <a href="#">SUBSTITUTED PRODUCT ITEM</a>
              </img>
              </div>
          </div>


          <div class="substitute_body">
            <p>
              <span>NOTICE! </span> This product is currently unavailable and has been replaced with the product below
            </p>

          </div>

          <div class="substitute_rgt">
            <a href="$TBT_REP_EA_PATH$">$TBT_REP_NIL_CODE$</a>
            <a href="$TBT_REP_EA_PATH$">View Substitute Product</a>
          </div>

        </div>
      </div>
          <div class="col-lg-20">
            $if(TBT_MORE_SHOW)$
            <a class="fmly-showmore" id="smlm" style="display:block;cursor:pointer;"  onclick="showmoredes();">Read / Show More</a>
            <a class="fmly-showmore"  id="lessmore" style="display:none;cursor:pointer;"  onclick="showmoredes();">Read / Less More</a>
            $endif$
            <div class="founded-ctgory" style="display:$TBT_DISPLAY_TIP$;">
              <div>
                <img src="$CDN$images/view-catgory.png" class="see-catgory"/>
                <p>
                  Related Products : <a href="$TBT_REWRITEURL_NEW$/fl/" class="">View Product Group Page</a>
                </p>
              </div>
            </div>
            <div class="founded-ctgory">
              <div>
                <img src="$CDN$images/see-catgory.png" class="see-catgory"/>
                <p>
                  Found in Category : <a href="$TBT_CT_HREF$" class="">$TBT_CT_NAME$ </a>
                </p>
              </div>
            </div>
          </div>
       </div>
      </div>
      $endif$
      $else$
      <div class="col-lg-20">
        <div class="col-lg-5 col-sm-7" itemprop="offers" itemscope="" itemtype="http://schema.org/Offer">
          <meta itemprop="category" content="$PRODUCT_CATEGORY_NAME$" />
          <ul>
            <meta itemprop="priceCurrency" content="AUD"/>
            <h2 class="prodlprice" >$TBT_COST$</h2>
            <meta itemprop="price" content="$TBT_META_COST$"/>
            <link itemprop="availability" href="$TBT_STOCK_HREF$"/>
            
          </ul>

        </div>

        <div class="col-lg-15 col-sm-13 paddrt0">


          <div class="col-lg-20 paddrt0">
            <div class="pro-discrip-cartbtn alignboth">
            
              <div class="form-inline">
                $if(TBT_HIDE_BUY)$


                $else$
                <div class="form-group">
                  <div class="number-box">
                   
                    <a onclick="decrement('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$');" id="btnDecrement" style="cursor:pointer;"> - </a>
                    <input id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"  size="5"  maxlength="6" type="text" class="nmbr_input" onkeydown="return keyct(event)"  value="1"  onkeypress="return validateNumber(event);"/>

                   
                    <a onclick="increment('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$');" id="btnIncrement" style="cursor:pointer;"> + </a>
                  </div>
                
                  <button type="button" class="btn btn-primary" onclick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');">Add to Cart</button>
                
                $endif$
                </div>
                 $endif$
              </div>
            </div>
          </div>



        </div>
      </div>
      <div class="col-lg-20 paddt_b">
        <div class="col-lg-12">
          <li class="">
            <span>Order Code:</span>$TBT_CODE$
          </li>
        </div>
        <div class="col-lg-8">
          <li>
            <span style="margin-left:-8px;">Availability: </span><!--<link itemprop="availability" href="$TBT_STOCK_HREF$"/>-->$TBT_STOCK_STATUS$$TBT_STOCK_STATUS_3$$TBT_STOCK_STATUS_1$
            $if(TBT_HIDE_BUY)$ PLEASE CONTACT US  $endif$
          </li>
        </div>
      </div>
      <div class="pro-discrip col-lg-20 nolftpadd">
    <div class="col-lg-20 nolftpadd productdetail" itemprop="description">
      <p>
        $TBT_SHORT_DESCRIPTION$
      </p>
      <li id="div1" style="display:block;">
        $if(TBT_PROD_DESC_SHOW)$
        $TBT_DESCRIPTION$
        $endif$
        $TBT_MORE$
      </li>
      <li id="div2" style="display:none;">
        $TBT_DESCALL$
      </li>
    </div>
    <div class="col-lg-20 nolftpadd">
      $if(TBT_MORE_SHOW)$
      <a class="fmly-showmore" id="smlm" style="display:block;cursor:pointer;"  onclick="showmoredes();">Read / Show More</a>
      <a class="fmly-showmore"  id="lessmore" style="display:none;cursor:pointer;"  onclick="showmoredes();">Read / Less More</a>
      $endif$

      <div class="founded-ctgory" style="display:$TBT_DISPLAY_TIP$;">
        <div>
          <img src="$CDN$images/view-catgory.png" class="see-catgory"/>
          <p>
            Related Products : <a href="$TBT_REWRITEURL_NEW$/fl/" class="">View Product Group Page</a>
          </p>
        </div>
      </div>
      <div class="founded-ctgory">
        <div>
          <img src="$CDN$images/see-catgory.png" class="see-catgory"/>
          <p>
            Found in Category : <a href="$TBT_CT_HREF$" class="">$TBT_CT_NAME$ </a>
          </p>
        </div>
      </div>
     
    </div>
  </div>
      $endif$
    </div>
</div>
<div class="clear"></div>
<div class="col-sm-20">
  <div class="tabs">
    <ul id="myTab" class="nav nav-tabs">
      <li class="active">
        <a href="#profile">Product Specs</a>
      </li>
      <li class="">
        <a href="#tab2">Ask a Question</a>
      </li>
      <li class="">
        <a href="#tab3">Bulk Buy / Project Price</a>
      </li>
      <li>
        <a href="#tab4">Downloads</a>
      </li>
      <li>
        <a href="#shipping">Shipping Info</a>
      </li>
    </ul>
    <div class="tab-content">
      <div id="profile" class="tab-pane active">
        <p>
          Product Specs for <a style="text-decoration:none !important;">$TBT_FAMILY_NAME$</a>
        </p>
        <table class="table specs">
          <tbody>
             $TBT_PRODDETAILS:{<tr>
              <th>$it.AttributeName$</th>
              <td>$it.SpecValue$</td>
            </tr>}$
          </tbody>
        </table>
      </div>
      <div id="tab4" class="tab-pane">
        <div class="text-center" id="DownloadLoad" style="display:none">
          <img src="$CDN$images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
          <br>Loading Data.. Please Wait..</br>
        </div>
      </div>
      <div id="tab2" class="tab-pane">
       <div class="text-center" id="askaquestionLoad" style="display:none">
         <img src="$CDN$images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
         <br>Loading Data.. Please Wait..</br>
       </div>
     
      </div>
      <div id="tab3" class="tab-pane">
        <div class="text-center" id="bulkbuyLoad" style="display:none">
          <img src="$CDN$images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
          <br>Loading Data.. Please Wait..</br>
        </div>
      
      </div>
      
      <div id="shipping" class="tab-pane">
        <div class="text-center" id="shippingLoad" style="display:none">
          <img src="$CDN$images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
          <br>Loading Data.. Please Wait..</br>
        </div>
    
      </div>
    </div>
  </div>
</div>

