 $Generateparentfamilyhtml$
<div class="col-lg-12 margin_top_20 clear padding_left_right">
    <div class="button-group" style="float:right;margin-bottom:-18px;">
        <a href="$TBT_CT_HREF$" class="vproduct-btn">
            View Product Category<br />
            <span>See All products in the same Product category</span>
          </a>

    </div>
</div>
<div class="clear"></div>
<div class="col-lg-12 margin_top_20 clear padding_left_right">
    <!-- tabs -->
    <div class="tabbable">
        <ul class="nav nav-tabs">
            <li class="active col-lg-3-1 col-xs-12 col-md-3 col-sm-3 padding_left_right_mob tab_head">
                <a href="#one" data-toggle="tab" class="bolder">Product Specs</a>
            </li>
            <li class="col-lg-3-1 col-xs-12 col-md-3 col-sm-3 padding_left_right_mob tab_head">
                <a href="#two" data-toggle="tab" class="bolder">Ask A Questions</a>
            </li>
            <li class="col-lg-3-1 col-xs-12 col-md-3 col-sm-3 padding_left_right_mob tab_head">
                <a href="#twee" data-toggle="tab" class="bolder">Bulk Buy / Project Pricing </a>
            </li>
            <li class="col-lg-3-1 col-xs-12 col-md-3 col-sm-3 padding_left_right_mob tab_head">
                <a href="#four" data-toggle="tab" class="bolder">Downloads</a>
            </li>
            <li class="col-lg-3-1 col-xs-12 col-md-3 col-sm-3 padding_left_right_mob tab_head">
                <a href="#five" data-toggle="tab" class="bolder">Shipping Info</a>
            </li>
        </ul>
        <div class="tab-content margin_top margin_bottom_15">

            <div class="tab-pane col-lg-12 active padding_left_right_mob" id="one">

                $FAMILY_ATT_FILTER$ $ST_Familypage$
                <div class="divLoadData"></div>

                <table id="tblload" style="display:none;" width="325px" align="center">
                    <tr>
                        <td>
                            <div style="width:300px" align="center">
                                <img src="$CDN$images/bigLoader.gif" alt="" width="12%" height="12%" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <Label ID="Label1" runat="server" Text="LOADING DATA...PLEASE WAIT" Font-Bold="True" Font-Names="Arial" Font-Size="X-Small"></Label>
                        </td>
                    </tr>
                </table>

            </div>
            <div class="tab-pane border_gray" id="two">
                <div id="divaskquestion" style="display:block;">
                    <!--<form class="margin_top_20">-->
                    <div class="form-group col-lg-6  form_Group_height" style="margin-top:20px;">
                        <label for="exampleInputEmail1" class="font_normal margin_bottom">Full Name <span class="error"> *</span>
                        </label>

                        <input type="text" id="txtFullname" class="form-control ask_question_input" placeholder="Full Name" maxlength="30" onblur="Controlvalidate('fn')" />
                        <span class="error" id="Errfullname" style="display:none;color: Red;"> Enter Full Name </span>

                    </div>
                    <div class="form-group col-lg-6 form_Group_height" style="margin-top:20px;">
                        <label for="exampleInputEmail1" class="font_normal margin_bottom">
                            Email address <span class="error"> *</span>
                        </label>
                        <input type="email" id="txtEmailAdd" class="form-control ask_question_input" placeholder="Enter email" maxlength="50" onblur="Controlvalidate('ea')" />
                        <span class="error" id="erremailadd" style="display:none;color: Red;"> Enter Email Address </span>
                        <span class="error" id="errvalidmail" style="display:none;color: Red;">Enter Valid Email </span>

                    </div>
                    <div class="form-group col-lg-6">
                        <label for="exampleInputPassword1" class="font_normal margin_bottom">
                            Phone <span class="error"> *</span>
                        </label>

                        <input type="text" id="txtPhone" class="form-control ask_question_input" placeholder="Phone" maxlength="30" onblur="Controlvalidate('p');" onkeypress="return validateNumber(event);" />
                        <span class="error" id="Errphone" style="display:none;color: Red;">Enter Phone Number </span>
                    </div>

                    <div class="form-group col-lg-6">
                        <label for="exampleInputPassword1" class="font_normal margin_bottom">
                            Questions <span class="error"> *</span>
                        </label>

                        <textarea id="txtQuestionx" rows="4" onblur="Controlvalidate('q')" onkeypress="textCounter('txtQuestionx','countfield',600);" maxlength="600" class="form-control ask_question_input"></textarea>
                        <span class="error" id="errquestion" style="display:none;">Enter The Question </span>
                        <div class="clear">
                            <input type="text" id="countfield" onblur="textCounter('txtQuestionx','countfield',600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter" class="char_remain readonly" /> Chars Remaining
                        </div>

                    </div>
                    <div class="form-group col-lg-6  form_Group_height">
                        <div id="recaptcha1"></div>
                        <!--     <label for="inputEmail3" class="col-sm-3 col-md-5 control-label font_normal">
              <img src="$CAPTCHA_IMAGE$"    />
              </label>
            <input type="text" id="txtCaptchCode" class="form-control ask_question_input width_size mob_left"   maxlength="4"  onblur="Controlvalidate('cc');" onkeypress="return validateNumber(event);"   />

            <span class="error mob_left mob_pad_left" id="errCaptchCode" style="display:none;"> Enter the code </span>
            <span class="error mob_left mob_pad_left" id="errCaptchInvalid" style="display:none;"> Invalid code </span>
            <span  class="error mob_left mob_pad_left"  id="errCaptchCode1" style="display:none;">$CC_CODE$</span>-->
                    </div>
                    <div class="form-group col-lg-6">
                        <input type="button" value="Submit" name="" class="btn green_bg white_color border_radius_none height_34" onclick="MailSend()" />
                        <input type="button" value="Reset" name="" class="btn blue_color white_color border_radius_none height_34" onclick="MailReset()" />
                    </div>
                    <!--</form>-->
                </div>

                <div id="divAskQuestionSubmit" class="alert alert-success border_radius_none text-center" role="alert" style="display:none;">
                    <span class="glyphicon glyphicon-ok-circle margin_right" aria-hidden="true"></span>
                    <strong> Thanks for your enquiry..! </strong> Our Customer Sales Team will be in contact with you Shortly.

                </div>
                <div class="clear"></div>
            </div>
            <div class="tab-pane border_gray" id="twee">
                $ST_BulkBuyPP$
                <div class="clear"></div>
            </div>
            <div class="tab-pane border_gray" id="four">
                $DownloadST$
                <div class="clear"></div>
            </div>
            <div class="tab-pane border_gray" id="five">
                <div style="margin-left:10px;margin-top:10px;">
                    <p style="font-family:Arial, Helvetica, sans-serif; font-size:16px;font-weight:bold; color:#007BDB; margin-bottom: 5px;margin-top: 20px;">&#36;9.90 Australia Wide Flat Rate Shipping*</p>
                    <p style="color:#818181; text-align:left; font-size:12px; font-family:Arial, Helvetica, sans-serif;margin:0;">
                        No matter where you are located in Australia we offer flat rate shipping.Some conditions apply, please see our <a style="text-decoration:none; color:#00a2e8; font-size:13px;" target="_blank" href="/Termsandconditions.aspx">Terms and Conditions </a>page.
                    </p>
                    <p style="font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;color:#007BDB; margin-bottom: 5px;margin-top: 20px;">Store Pick Up from Sydney Showroom</p>
                    <p style="color:#818181; text-align:left; font-size:12px; font-family:Arial, Helvetica, sans-serif;margin:0;">
                        Goods can be picked from our premises from 8.30AM to 5.00PM Monday to Friday and from 9AM till 4PM on Saturday. In order to avoid delays it is recommended that you place your
                        <br /> order 2 hours prior to pick up. please see our <a style="text-decoration:none; color:#00a2e8; font-size:13px;" target="_blank" href="/Termsandconditions.aspx">Terms and Conditions </a> page for more details.
                    </p>

                    <p style="font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;color:#007BDB; margin-bottom: 5px;margin-top: 20px;">International Order Shipping Available</p>
                    <p style="color:#818181; text-align:left; font-size:12px; font-family:Arial, Helvetica, sans-serif;">
                        We ship internationally. Once your order is received we will advise shipping cost to you by email. Your order will be updated with the freight cost.
                    </p>
                    <div class="clear"></div>
                </div>
            </div>
        </div>
    </div>
    <!-- /tabs -->

</div>
<div class="clear"></div>