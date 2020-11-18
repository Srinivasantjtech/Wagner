<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td>
      <div class="topdiv">
        <div class="tdidiv">
          <table width="997" border="0" cellpadding="0" cellspacing="0">
            <tr>
              <td width="361" rowspan="3" valign="bottom" class="logo">
                <a href="$TBT_CURRENTURL$" class="toplogowag">
                </a>
              </td>
              <td width="102" rowspan="3" valign="middle">
                <table cellpadding="0" cellspacing="0" border="0">
                  <tr>
                    <td height="24" align="right" class="txt_1">
                    </td>
                    <td width="26" align="center" class=""></td>
                  </tr>
                  <tr>
                    <td height="24" align="right" class="txt_1">
                    </td>
                    <td align="center"  class=""> </td>
                  </tr>
                  <tr>
                    <td height="24" align="right" class="txt_1">
                    </td>
                    <td align="center"  class=""></td>
                  </tr>
                </table>
              </td>
              <td width="534" height="61" align="right" valign="middle" style="padding-right:22px;">
                <table border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td colspan="12" class="txt_1">
                      <div class="welcommsg" style="Color:white;">Welcome, &nbsp; $TBT_COMPANY_NAME$ &nbsp; $TBT_LOGIN_NAME$ &nbsp;</div>
                    </td>
                  </tr>
                  <tr>
                    <td colspan="12" class="txt_1" height="5px"></td>
                  </tr>
                  <tr class="txt_1">
                    <td>
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="right">
                      <a href="$TBT_CURRENTURL$catalogue/">Online Catalogue</a>
                    </td>
                    <td width="18" align="center">&nbsp;</td>
                    <td align="right">
                      <a href="$TBT_CURRENTURL$MyAccount.aspx">My Account</a>
                    </td>
                    <td width="22" align="center" class="topmyacc">
                    </td>
                    <td align="right">
                      <a href="$TBT_CURRENTURL$AboutUs.aspx" >About Us</a>
                    </td>
                    <td width="22" align="center" class="topabout"></td>
                    <td align="right">
                      <a href="$TBT_CURRENTURL$ContactUs.aspx" >Contact Us </a>
                    </td>
                    <td width="22" align="center" class="topcontact"></td>
                    <td align="right">
                      <a href="$TBT_CURRENTURL$Logout.aspx" >Logout</a>
                    </td>
                    <td width="22" align="center" class="toplogin"></td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr>
              <td height="30" valign="top">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="17" height="30">&nbsp;</td>
                    <td width="167" align="center" class="txt_2 inn">
                      <strong>
                        <span id="cartqty"> Items in Cart: $TBW_CART_ITEM$</span>
                      </strong>
                    </td>
                    <td width="33" align="center" valign="middle"  class="topviewcart">
                    </td>
                    <td width="100" valign="middle" >
                      <a href="$TBT_CURRENTURL$orderDetails.aspx?&amp;bulkorder=1&amp;Pid=0&amp;ORDER_ID=$TBT_ORDER_ID$" class="viewcart tvcart" id="cartviewcart">VIEW CART</a>
                    </td>
                    <td width="30" align="center" valign="middle" class="topcheckout">
                    </td>
                    <td width="87" valign="middle">
                      <a id="cartcheckout" href="$TBT_SHIP_URL$" class="txt_1" style="Color:#FFFFFF;">
                        <strong>CHECK OUT</strong>
                      </a>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr>
              <td height="32" valign="top">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="208" height="32" align="right" valign="middle" >
                      <a href="$TBT_CURRENTURL$sitemap.aspx" class="topdisallcat">
                      </a>
                    </td>
                    <td width="79" align="right" valign="middle" class="txt_3">
                      <strong>Search :</strong>
                    </td>
                    <td width="166">
                      <input type="text" name="srcfield" id="srcfield" onkeypress="return urlredirectK(event)" style="width:166px;" class="inputtxtlogps"/>
                     
                    </td>
                    <td width="81" align="left">

                      
                      <a class="btn2" alt=""    style="cursor:pointer;display:block;" onclick="urlredirect();"  />
                      </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </div>
        <div class="divsmoke"></div>
      </div>
    </td>
  </tr>
  <tr>
    <td align="center">

      <table width="995" border="0" cellpadding="0" cellspacing="0" >
        <tr>
          <td width="995" valign="top"  >
            <div id="navcontainer">
              <ul>
                $TBWDataList:{$it.TBWDataListItem$}$

                <li class="default">
                  <a href="$TBT_CURRENTURL$new-products/ct/" target="_top" class="last redbtn">
                    New Products
                    <b class="glyphicon glyphicon-menu-down"></b>
                  </a>
                </li>

              </ul>
            </div>
          </td>
        </tr>
      </table>
    </td>
  </tr>
</table>