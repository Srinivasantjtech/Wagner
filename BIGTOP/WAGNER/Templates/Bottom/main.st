<footer class="footer">
  <div class="container">
    <div class="row">
      <div id="no-footerlist" class="col-sm-10 col-md-5">
        <div class="footer-list">
          <h3>Product Categories</h3>
          $TBWDataList:{$it.TBWDataListItem$}$
        </div>
      </div>
      <div id="no-footerlist" class="col-sm-10 col-md-4">
        <!--<div class="footer-list">
          <h3>Wagner Mini Stores</h3>
          <li>
            <a href="/cypconverters/mct/">CYP Converters Online </a>
          </li>
          <li>
            <a href="/electrical-wholesalers-online/mct/" >Electrical Wholesalers Online Store</a>
          </li>
          <li>
            <a href="/vip-vision/mct/" >VIP-Vision Online Store</a>
          </li>
          <li>

            <a href="/judge-cctv-surveillance/mct/"> Judge CCTV Surveillance</a>
          </li>-->
           <!--<li>
            <a href="/reelplay-tv-australia/mct/" >ReelPlay TV Australia</a>
          </li>-->
          <!--<li>
          <a href="/Ourbrands/" >Our Brands</a>
          </li>
          <li>
          <a href="/mbrand.aspx?type=1" >Our Brands1</a>
          </li>
        </div>-->
        $if(TBT_DUMUSER_CHECK)$
        <div class="footer-list footer-myaccount-hide">
          <h3>MyAccount</h3>
          <li>
          <!--$if(TBT_RET_PRO)$-->
          <!--<a href="/myaccount.aspx" >View Profile</a>-->
          <!--$else$-->
          <a href="/myaccount.aspx"  title="View Profile" >View Profile</a>
          <!--$endif$-->
        </li>
          <li >
            <a href="/myaccount.aspx#messages">Change Password</a>
          </li>
          <li >
            <a href="/myaccount.aspx#home">Change Username</a>
          </li>
          <li >
            <a href="/myaccount.aspx#settings">Order History</a>
          </li>
        </div>
        $endif$
      </div>
       <div id="no-footerlist" class="col-sm-10 col-md-5">
        <div class="footer-list">
          <h3>COMPANY INFO</h3>
          <li>
            <a href="/aboutus.aspx"> About Us</a>
          </li>
          <li>
            <a href="/contactus.aspx"> Contact Us</a>
          </li>
          <li>
            <a href="/Termsandconditions.aspx">Terms and Conditions</a>
          </li>

          <li >
            <a href="/privacepolicy.aspx" >Privacy Policy</a>
          </li>
          <li >
            <a href="/sitemap.aspx">Site Map</a>
          </li>
          <li>
            <a href="/0-9/bk/" >Browse Keyword Index</a>

          </li>
          <li>
            <a href="/0-9/bp/">Browse Product Tag Index</a>
          </li>
          <li>
            <a href="/electronics-news/">Wagner Electronics News and Updates</a>

          </li>
          <li>
            <a href="/Review.aspx">Reviews</a>

          </li>
        </div>
      </div>
      <div id="footer-accordian">
        <div class="categoryselectmenu">
          <section data-accordion-group="" id="only-one">
            <section data-accordion="">
              <button data-control="" onclick="return false">Product Catagories</button>
              <div data-content="" style="max-height: 0px; overflow: hidden;">
                $TBWDataList:{$it.TBWDataListItem$}$
              </div>
            </section>

            <!--   <section data-accordion="">
              <button data-control="" onclick="return false">Wagner Mini Stores</button>
              <div data-content="" style="max-height: 0px; overflow: hidden;">
                <a href="/cypconverters/mct/">CYP Converters Online </a>
                <a href="/electrical-wholesalers-online/mct/" >Electrical Wholesalers Online Store</a>
                <a href="/vip-vision/mct/" >VIP-Vision Online Store</a>
              

                  <a href="/judge-cctv-surveillance/mct/"> Judge CCTV Surveillance</a>
               
                 <!-<a href="/the-loudspeaker-kit/mct/" >The LoudSpeaker Kit</a>->    
            <!-<a href="/reelplay-tv-australia/mct/" >ReelPlay TV Australia</a>->
                <a href="/Ourbrands/" >Our Brands</a>
                <a href="/mbrand.aspx?type=1" >Our Brands1</a>
              </div>
            </section>-->
        $if(TBT_DUMUSER_CHECK)$
        <section data-accordion="">
              <button data-control="" onclick="return false">MyAccount</button>
              <div data-content="" style="max-height: 0px; overflow: hidden;">
                $if(TBT_RET_PRO)$
                <a href="/myaccount.aspx" >View Profile</a>
                $else$
                <a href="/myaccount.aspx#profile"  title="View Profile" >View Profile</a>
                $endif$
                <a href="/myaccount.aspx#home">Change Password</a>
                <a href="/myaccount.aspx#messages">Change Username</a>
                <a href="/myaccount.aspx#settings">Order History</a>
              </div>
            </section>
            $endif$
            <section data-accordion="">
              <button data-control="" onclick="return false">Bigtop Info</button>
              <div data-content="" style="max-height: 0px; overflow: hidden;">
   
                  <a href="/aboutus.aspx"> About Us</a>
         
                  <a href="/contactus.aspx"> Contact Us</a>
           
                  <a href="/Termsandconditions.aspx">Terms and Conditions</a>
          
                  <a href="/privacepolicy.aspx" >Privacy Policy</a>
            
                  <a href="/sitemap.aspx">Site Map</a>
            
                  <a href="/0-9/bk/" >Browse Keyword Index</a>

           
                  <a href="/0-9/bp/">Browse Product Tag Index</a>
            
                  <a href="/electronics-news/">Wagner Electronics News and Updates</a>
                <a href="/Review.aspx">Reviews</a>
              </div>
            </section>

          </section>
        </div>
      </div>
      <div class="col-sm-10 col-md-6">

        <div class="footer-list">
          <div class="social-list">
            <h3>Connect With Us</h3>
            <li>
              
              <a href="https://www.facebook.com/wagner.electronics/" class="fb_footer" title="share on facebook" target="_blank" >
                <!--<img class="fb_footer" alt="Wagner Electronics"/>-->
              </a>
            </li>
            <li>
              <a href="https://twitter.com/WagnerOnlineAU" class="tw_footer" target="_blank" >
                <!--<img class="tw_footer" alt="Wagner Electronics"/>-->
              </a>
            </li>
            <li>
              <a href="https://plus.google.com/101848865943067222284" class="google_footer" target="_blank" >
                <!--<img class="tw_footer" src="$CDN$images/wagner-g.png" alt="Wagner Electronics"/>-->
              </a>
            </li>
            <li>
              <a href="https://www.instagram.com/wagner_electronics/" class="insta_footer" target="_blank" >
                <!--<img src="$CDN$images/instagram.png" alt="Wagner Electronics"/>-->
              </a>
            </li>
         
          </div>
          <div class="payment-list">
            <h3>Payment Options</h3>
            <li>
              <a href="#" class="visa_footer">
                <!--<img src="$CDN$images/wagner-visa.png" alt="Wagner Electronics"/>-->
              </a>
            </li>
            <li>
              <a href="#" class="master_footer">
                <!--<img src="$CDN$images/wagner-master.png" alt="Wagner Electronics"/>-->
              </a>
            </li>
            <li>
              <a href="#" class="ae_footer">
                <!--<img src="$CDN$images/wagner-ae.png" alt="Wagner Electronics"/>-->
              </a>
            </li>
            <li>
              <a href="#" class="payp_footer">
                <!--<img src="$CDN$images/wagner-payp.png" alt="Wagner Electronics"/>-->
              </a>
            </li>
          </div>
          <div class="newsletter">
            <h3>Newsletter Sign Up</h3>
            <p>Receive Latest Product Updates and releases</p>
            <div class="col-sm-14 col-xs-14">
              <input type="email" placeholder="Enter Your Email Address" id="txtMail1" class="foot_subscribe"/>
            </div>
            <div class="col-sm-6 col-xs-6">
              <input type="button" class="sub_submit" onclick="GetDeal(); return false;" value="Sign Up"/>
            </div>
          </div>
        </div>
      </div>
    </div>
    $PopularSearch$
  </div>
  <div class="scrolltop">
    <a style="cursor:pointer;" >
      <i class="glyphicon glyphicon-chevron-up"></i>
    </a>
  </div>
  <div class="reviewmgr-stream" data-show-aggregate-rating="true" data-show-reviews="false" data-include-empty="false" data-review-limit="1" data-url="https://reviewr.app/wagneronline/"></div>
  <script>!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0];if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src="//platform.reviewmgr.com/widgets.js";fjs.parentNode.insertBefore(js,fjs);}}(document, "script", "reviewmgr-wjs");</script>


</footer>
<div class="copy_write"> &copy; 2015 Wagner Online Store. All Rights Reserved </div>


