<div id="messages">
  <p>
    <a href="#review_btn"></a>
  </p>
  <h3></h3>
  <form id="askaquestion">
	  <div class="row">
		  <div class="col-sm-12 mb15 clearfix">
			  <label class="askq" style="margin-bottom:0px;line-height:1.2;">
				  Full Name<span class="required">*</span>
			  </label>
			  <input type="text" id="txtFullname"  maxlength="30" onblur="Controlvalidate('fn')" class="form-control"/>
			  <span class="mandatory dpynone" id="Errfullname">Enter Full Name</span>
			  <span id="ProductCode" class="dpynone">$TBT_CODE$</span>
		  </div>
	  </div>
	  <div class="row">
      <div class="col-sm-12 mb15 clearfix">
        <label class="askq" style="margin-bottom:0px;line-height:1.2;">
          Email<span class="required">*</span>
        </label>
        <input type="text" id="txtEmailAdd" maxlength="50"  onblur="Controlvalidate('ea')" class="form-control"/>
        <span class="mandatory dpynone" id="erremailadd"> Enter Email Address </span>
        <span class="mandatory dpynone" id="errvalidmail">Enter Valid Email </span>
      </div>
    </div>
	  <div class="row">
		  <div class="col-sm-12 mb15 clearfix">
			  <label class="askq" style="margin-bottom:0px;line-height:1.2;">
				  Phone<span class="required">*</span>
			  </label>
			  <input type="text" id="txtPhone"  maxlength="30"  onblur="Controlvalidate('p');" onkeypress="return validateNumber(event);" class="form-control"/>
			  <span class="mandatory dpynone" id="Errphone">Enter Phone Number </span>
		  </div>
	  </div>
	
	 
   
    <div class="row">
		<div class="col-sm-12 mb15 clearfix">
        <label class="askq" style="margin-bottom:0px;line-height:1.2;">
          Questions<span class="required">*</span>
        </label>
        <textarea  id="txtQuestionx" rows="10" maxlength="600" cols="34"  onblur="Controlvalidate('q')" onkeypress="textCounter(this,this.form.counter,600);" class="form-control fullwidth"></textarea>
        <span class="mandatory dpynone" id="errquestion">Enter The Question </span>
      </div>
      <div class="col-xs-20">

        <input type="text" placeholder="600" class="form-control character"  onblur="textCounter(this.form.counter,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter"/>
        <span>
          <label>Chars Remaining </label>
        </span>
      </div>
    </div>
 
	  <div class="row">

		  <div class="col-sm-12 mb15 clearfix">

			  <label class="vishid">
				  Code<span class="required">*</span>
			  </label>
			  <div id="recaptcha1" ></div>

		  </div>

	  </div>
	  <br />
	  <div class="row">
		  <div class="col-sm-12 clearfix">
			  <p id="review_btn">
				  <button value="Submit" type="button" onclick="MailSend()" class="btn btn-default">Submit</button>
				  <button value="Reset" onclick="MailReset()" type="reset" class="btn btn-default">Reset</button>
			  </p>
		  </div>
	  </div>
  </form>


</div>
<div class="alert alert-success" id="divAskQuestionSubmit" style="display:none;">
  <strong>Success!</strong> Thanks for your enquiry. Our Customer Sales Team will be in contact with you Shortly.
</div>