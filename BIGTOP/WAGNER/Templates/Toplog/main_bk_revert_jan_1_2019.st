$if(TBT_CHK_HOME)$ 
<div class="headerbg">
 <div class="container">
  <div class="row"> <div class="col-sm-20"> <div id="logo"> <a href="$TBT_CURRENTURL$">
   <img class="img-responsive col-lg-20" alt="Wagner Electronics" src="/images/Wagner-logo.png"/> </a> 
   <a href="$TBT_CURRENTURL$"> <img class="img-responsive" id="logo-small" alt="Wagner Electronics" src="/images/logo_small.png"/> </a> </div> </div> <div class="col-sm-20"> <div class="mainsearchbox clearfix"> <div  class="form-horizontal"> <fieldset class="search-box"> <input id="txtsear" value="" placeholder="Quick Product Search" type="text" name="txtsear" onkeypress="return urlredirect_enterK(event)"/> <button value="Search" class="srch-btn" type="submit"  onclick="return urlredirectK(event)"  ></button> </fieldset>
    </div> </div> </div> </div> 
	<div class="row float_wrapper">
            	
          <div class="container float_catg">
   <div id="circle-btn" class="row"> <div class="col-sm-20 clearfix"> <ul id="top-icons" class="clearfix">  <!-- $TBWDataList:{$it.TBWDataListItem$}$-->
           Cache_Top_Replace </ul> </div></div></div>
		   
		    <div class="container-fluid float_banner zerospace">
	<div class="container">
    	<div class="main-banner animateOnScroll" 
data-animation-type="fadeInUp" data-timeout="0" data-offset-top="200">
        
             <a href="#" >
                 <img class="img-responsive" alt="x-mas" src="images/wagner-xmas-banner-2018-main-1.png"/>

             </a>

   
        </div>	
    </div>
	</div>
		   
		   
		   
		    </div> </div> </div>
		   
		    $else$ <div class="container hidden-xs hidden-sm"> <div class="row"> <div class="col-md-8"> <div id="innerlogo"> <a href="$TBT_CURRENTURL$"> <img class="img-responsive col-lg-20" alt="Wagner Electronics" src="/images/Wagner-logo.png"/> </a> <a href="$TBT_CURRENTURL$"> <img class="img-responsive" id="logo-small" alt="Wagner Electronics" src="/images/logo_small.png"/> </a> </div> </div> <div class="col-md-12"> <div class="ctgorysearchbox clearfix"> <fieldset class="search-box"> <input type="text" placeholder="Quick Product Search! Enter key words or product Codes!"  name="txtsear1" id="txtsear1" onkeypress="return urlredirect_enter(event)"/> <button value="Search" class="srch-btn" type="submit" onclick="return urlredirectK_src(event)"  ></button> </fieldset> </div> </div> </div> </div> <div class="container-fluid"> <div class="navbg hidden-sm hidden-xs"> <div class="container"> <ul id="categorynav" class="nav clearfix">  <!-- $TBWDataList:{$it.TBWDataListItem$}$-->
           Cache_Top_Replace </ul>  </div> </div> </div> $endif$