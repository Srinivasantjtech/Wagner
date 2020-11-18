$if(TBT_CHK_HOME)$ 
<div class="headerbg">
 <div class="container">
  <div class="row"> 

         <div class="col-md-4 col-xs-7 col-sm-10">
                <div id="innerlogo">
                    <a href="/home.aspx">
                        <img src="$CDN$images/bigtop.png" class="img-responsive" width="100px" height="100px"></img></a>
                    <img src="$CDN$images/bigtop.png" id="logo-small" class="img-responsive" width="100px" height="70px"></img>
                </div>
            </div>
            <div class="col-md-12 hidden-sm hidden-xs">
                <div class="ctgorysearchbox clearfix">

                    <div  class="form-horizontal">
                        <fieldset class="search-box"> 
                            <input id="txtsear" value="" placeholder="Quick Product Search" type="text" name="txtsear" onkeypress="return urlredirect_enterK(event)"/> 
                            <button value="Search" class="srch-btn" type="submit"  onclick="return urlredirectK(event)"  ></button> 
                        </fieldset>
                    </div>

                </div>
            </div>
            <div class="col-md-4 col-xs-13 col-sm-10 pull-right clearfix">
                <a href="/home.aspx">
                    <img src="$CDN$images/bigtop.png" class="img-responsive mgn_top5 pull-right mob_rgtspace " width="100px" height="100px"></img>
                </a>
            </div>
      
     <!-- <div class="col-sm-20"> <div id="logo"> <a href="$TBT_CURRENTURL$">
   <img class="img-responsive col-lg-20" alt="Big Top" src="$CDN$images/Wagner-logo.png"/>  </a> 
   <a href="$TBT_CURRENTURL$"> <img class="img-responsive" id="logo-small" alt="Big Top" src="$CDN$images/logo_small.png"/> </a> </div> </div> <div class="col-sm-20"> <div class="mainsearchbox clearfix"> <div  class="form-horizontal"> <fieldset class="search-box"> <input id="txtsear" value="" placeholder="Quick Product Search" type="text" name="txtsear" onkeypress="return urlredirect_enterK(event)"/> <button value="Search" class="srch-btn" type="submit"  onclick="return urlredirectK(event)"  ></button> </fieldset>
    </div> </div> </div> -->


  </div> 
	<div class="row float_wrapper">
         
		    </div> </div> </div>
	    <div class="container-fluid" style="background-color: #dcdcdc;"><div class="row"><div class="container float_catg">
   <div id="circle-btn" class="row"> <div class="col-sm-20 clearfix"> <ul id="categorynav" class="clearfix">  <!-- $TBWDataList:{$it.TBWDataListItem$}$-->
           Cache_Top_Replace </ul> </div></div></div></div></div>
		
	<div class="container-fluid zerospace">
	<div class="container mb20 mt20">
    	<div class="main-banner slides">
			$TBT_BANNER$
		</div>
	</div>
	</div>	   
		    $else$ <div class="container hidden-xs hidden-sm"> <div class="row" style="margin-bottom: 15px;"> <div class="col-md-4"> <div id="innerlogo">
			 <a href="/home.aspx" class="wagner_logo">
		 	 <img class="img-responsive" alt="Big Top" src="$CDN$images/bigtop.png" width="100px" height="100px"/>
			  </a> <a href="/home.aspx"> <img class="img-responsive" id="logo-small" alt="Big Top" src="$CDN$images/bigtop.png"/> </a> </div> </div> 
			  <div class="col-md-12"> <div class="ctgorysearchbox clearfix"> <fieldset class="search-box"> <input type="text" placeholder="Quick Product Search!"  name="txtsear1" id="txtsear1" onkeypress="return urlredirect_enter(event)"/> <button value="Search" class="srch-btn" type="submit" onclick="return urlredirectK_src(event)"  ></button> </fieldset> </div> </div> 
			  <div class="col-md-4"> <a href="/home.aspx"> <img src="$CDN$images/bigtop.png" class="img-responsive mgn_top5 pull-right mob_rgtspace " width="100px" height="100px" /></a></div>
			  </div> </div>
			   <div class="container-fluid"> <div class="navbg hidden-sm hidden-xs"> <div class="container"> <ul id="categorynav" class="nav clearfix">  <!-- $TBWDataList:{$it.TBWDataListItem$}$-->
           Cache_Top_Replace </ul>  </div> </div> </div> $endif$