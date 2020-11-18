<footer class="footer">
  <div class="container-fluid flwdh_2">
    <div class="row">
      <div id="no-footerlist" class="col-sm-10 col-md-5">
        <div class="footer-list">
          <h3>Product Catagories</h3>
          $TBWDataList:{$it.TBWDataListItem$}$
        </div>
      </div>
      <div id="no-footerlist" class="col-sm-10 col-md-4">
        <div class="footer-list">
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
        </div>
        $if(TBT_DUMUSER_CHECK)$
        <div class="footer-list">
          <h3>MyAccount</h3>
          <li >
          $if(TBT_RET_PRO)$
          <a href="/myaccount.aspx" >View Profile</a>
          $else$<a href="/myaccount.aspx"  title="View Profile" >View Profile</a>
          $endif$ </li>
          <li >
            <a href="/myaccount.aspx">Change Password</a>
          </li>
          <li >
            <a href="/myaccount.aspx">Change Username</a>
          </li>
          <li >
            <a href="/myaccount.aspx">Order History</a>
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
        </div>
      </div>
      <div id="footer-accordian">
        <div class="categoryselectmenu">
          <section data-accordion-group="" id="only-one">
            <section data-accordion="#">
              <button data-control="#">Product Catagories</button>
              <div data-content="#" style="max-height: 0px; overflow: hidden;">
                $TBWDataList:{$it.TBWDataListItem$}$
              </div>
            </section>

            <section data-accordion="">
              <button data-control="">Wagner Mini Stores</button>
              <div data-content="" style="max-height: 0px; overflow: hidden;">
                <a href="/cypconverters/mct/">CYP Converters Online </a>
                <a href="/electrical-wholesalers-online/mct/" >Electrical Wholesalers Online Store</a>
                <a href="/vip-vision/mct/" >VIP-Vision Online Store</a>
              </div>
            </section>
            $if(TBT_DUMUSER_CHECK)$
            <section data-accordion="">
              <button data-control="">MyAccount</button>
              <div data-content="" style="max-height: 0px; overflow: hidden;">
                $if(TBT_RET_PRO)$
                <a href="/myaccount.aspx" >View Profile</a>
                $else$
                <a href="/myaccount.aspx"  title="View Profile" >View Profile</a>
                $endif$
                <a href="/myaccount.aspx">Change Password</a>
                <a href="/myaccount.aspx">Change Username</a>
                <a href="/myaccount.aspx">Order History</a>
              </div>
            </section>
            $endif$
            <section data-accordion="">
              <button data-control="">Wagner Info</button>
              <div data-content="" style="max-height: 0px; overflow: hidden;">
   
                  <a href="/aboutus.aspx"> About Us</a>
         
                  <a href="/contactus.aspx"> Contact Us</a>
           
                  <a href="/Termsandconditions.aspx">Terms and Conditions</a>
          
                  <a href="/privacepolicy.aspx" >Privacy Policy</a>
            
                  <a href="/sitemap.aspx">Site Map</a>
            
                  <a href="/0-9/bk/" >Browse Keyword Index</a>

           
                  <a href="/0-9/bp/">Browse Product Tag Index</a>
            
                  <a href="/electronics-news/">Wagner Electronics News and Updates</a>

              </div>
            </section>

          </section>
        </div>
      </div>
      <div class="col-sm-10 col-md-6">

        <div class="footer-list">
          <div class="social-list">
            <h3>Share</h3>
            <li>
              
              <a href="https://www.facebook.com/sharer/sharer.php?u=http://$Rawurl$" title="share on facebook" target="_blank">
                <img src="$CDN$images/wagner-fb.png"/>
              </a>
            </li>
            <li>
              <a href="http://twitter.com/share?url=http://$Rawurl$" target="_blank">
                <img src="$CDN$images/wagner-twit.png"/>
              </a>
            </li>
            <li>
              <a href="https://plus.google.com/share?url=http://$Rawurl$" target="_blank">
                <img src="$CDN$images/wagner-g.png"/>
              </a>
            </li>
            <li>
              <a href="https://pinterest.com/pin/create/button/?url=&amp;media=http://$Rawurl$" target="_blank">
                <img src="$CDN$images/wagner-pin.png"/>
              </a>
            </li>
            <li>
              <a href="http://www.linkedin.com/shareArticle?mini=true&amp;url=http://$Rawurl$" target="_blank">
                <img src="$CDN$images/wagner-in.png"/>
              </a>
            </li>
          </div>
          <div class="payment-list">
            <h3>Payment Options</h3>
            <li>
              <a href="#">
                <img src="$CDN$images/wagner-visa.png"/>
              </a>
            </li>
            <li>
              <a href="#">
                <img src="$CDN$images/wagner-master.png"/>
              </a>
            </li>
            <li>
              <a href="#">
                <img src="$CDN$images/wagner-ae.png"/>
              </a>
            </li>
            <li>
              <a href="#">
                <img src="$CDN$images/wagner-payp.png"/>
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
  </div>
  <div class="scrolltop">
    <a href="#">
      <i class="glyphicon glyphicon-chevron-up"></i>
    </a>
  </div>
</footer>
<div class="copy_write"> &copy; 2015 Wagner Online Store. All Rights Reserved </div>


