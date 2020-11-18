<html><head><meta http-equiv="Content-Language" content="en-us">
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<title>E-mail template</title>
</head>

<body style=" font-family:Arial, Helvetica, sans-serif; color:#333333;">
<div style="width:600px;">
<table width="600" height="" cellspacing="0" cellpadding="0" border="0">
  <tbody>

   <tr>
    <td width=""><img width="330" src="https://www.wagneronline.com.au/images/wagner-logo-email.gif" alt="wagner-logo"/></td>
    <td style=" font-size:14px; line-height:25px;color:#333333 !important;text-decoration:none !important;"><strong>Web :</strong> <a target="_blank" style="color:#333333 !important;text-decoration:none !important;" href="https://www.wagneronline.com.au">www.wagneronline.com.au</a><br>
      <p style="border-bottom:solid thin #333; margin:3px 0;"></p>
      <strong style="color:#333333 !important;text-decoration:none !important;">E-mail :</strong> <a  style="color:#333333 !important;text-decoration:none !important;" href="">sales@wagneronline.com.au</a><br>
      <p style="border-bottom:solid thin #333; margin:3px 0;"></p>
      <strong>Telephone :</strong> +61 2 9798 9233 <br>
      <p style="border-bottom:solid thin #333; margin:3px 0;"></p>
      <strong>&nbsp;Address 
      :</strong> Wagner Online,&nbsp; 138 Liverpool Road,         
      Ashfield, NSW, 2131, Australia
      <p style="border-bottom:solid thin #333; margin:3px 0 20px; "></p></td>
  </tr>
 <tr style="background:#449D44;height:50px;text-align:left;">
    <td colspan="3"><strong style="color:#fff; padding-left:10px;font-size:15px;">Order Confirmation</strong></td>
  </tr>
  <tr>
    <td style="padding:15px 10px;line-height:24px;font-size:14px;" colspan="3">Thanks for your order. Your Order has now been submitted to us for processing. Please see below for your submitted order details. Please note this is not the final invoice and only a confirmation we have received your order. </td>
  </tr>
  <tr>
    <td style="padding:5px 10px 10px;line-height:24px;font-size:14px;" colspan="3">If there is any incorrect details or concerns please contact us ASAP: </td>
  </tr>
  <tr>
    <td style="height:50px; background:#0069B3; color:#fff; font-size:14px;" colspan="3"><strong style="padding-left:10px;">ORDER DETAILS</strong></td>
  </tr>
  </tbody></table>
  <div>
   <div style="float:left; width:50%;">
 <div style="background:#F4F4F4; padding:10px; margin:20px; width:85%; float:left"><strong style="padding-left:10px; color:#404040; font-size:13px;">SHIPPING &amp; ORDER DETAILS</strong></div>
 <table width="90%" cellspacing="0" cellpadding="0" style="margin-left:20px; margin-right:20px; border:solid thin #ddd; border-collapse: collapse;">
 <tbody><tr>
 <tr>
 <td style=" border:solid thin #ddd;  padding:10px ; font-size:12px; width:50%""><strong>SHIPPED METHOD</strong></td>
 <td style=" border:solid thin #ddd; padding:10px">$ShippingMethod$</td>
 </tr>
 <tr>
 <td style="border:solid thin #ddd; padding:10px ; font-size:12px;"><strong>ORDER NO</strong></td>
 <td style=" border:solid thin #ddd; padding:10px ; font-size:12px;">$CustOrderNo$</td>
 </tr>
  <tr>
 <td style="border:solid thin #ddd; padding:10px ; font-size:12px;"><strong>INVOICE NO</strong></td>
 <td style=" border:solid thin #ddd; padding:10px ; font-size:12px;">$INVOICENO$</td>
 </tr>
    $if(TBT_shippingnotes)$
    <tr>
	<td style="border:solid thin #ddd; padding:10px ; font-size:12px;"><strong>ORDER COMMENTS</strong></td>
	 <td style="border:solid thin #ddd; padding:10px ; font-size:12px;">$shippingnotes$</td>
	</tr>
    $endif$
  </tbody></table>
 </div>
 <div style="float:left; width:50%;">
 <div style="background:#F4F4F4; padding:10px; margin:20px; width:85%; float:left"><strong style="padding-left:10px; color:#404040; font-size:14px;">USER</strong></div>
 <table width="90%" cellspacing="0" cellpadding="0" style="margin-left:20px; margin-right:20px;">
 <tbody><tr>
 <td style="font-size:12px;"><strong>CREATED BY USER</strong></td>
 </tr>
 <tr>
 <td style="padding-left:10px; font-size:12px; padding-top:10px">$CreatedBy$</td>
 </tr>
  <tr>
 <td style="font-size:12px; padding-top:10px"><strong>APPROVED BY USER</strong></td>
 </tr><tr>
 <td style="padding-left:10px; font-size:12px; padding-top:10px">$SubmittedBy$</td>
 </tr>
 </tbody></table>
 </div>
 <div style="clear:both"></div>
 </div>
  <div style="clear:both;">
  <div style="float:left; width:50%;line-height:24px;">
 <div style="background:#F4F4F4; padding:10px; margin:20px; width:85%; float:left"><strong style="padding-left:10px; color:#404040; font-size:14px;">BILL TO</strong></div>
 <ul style="list-style:none;padding-left:23px; font-size:14px; line-height:24px;">
<li>$BillingAddress$</li>
 </ul>
 </div>
  
<div style="float:left; width:50%;font-size:14px; line-height:24px;">
 <div style="background:#F4F4F4; padding:10px; margin:20px; width:85%; float:left"><strong style="padding-left:10px; color:#404040; font-size:14px;">SHIP TO</strong></div>
 <ul style="list-style:none; padding-left:23px;">
<li>$ShippingAddress$</li>
 </ul>
 </div>
 </div>
 <table width="100%" cellspacing="0" cellpadding="0" style="  border:solid thin #ddd; border-collapse: collapse;">
 <tbody><tr style="background:#0069B3;">
 <td style=" border:solid thin #ddd;  padding:10px ; font-size:14px; color:#fff;" colspan="2"><strong>ORDER CONTENTS</strong></td>
 </tr>
 <tr>
 <td style="border:solid thin #ddd; padding:10px ; width:50%; font-size:13px;"><strong>ORDER CODE</strong></td>
 <td style=" border:solid thin #ddd; padding:10px ; font-size:12px;"><strong>QTY</strong></td>
 </tr>

  $TBWDataList:{
   $it.TBWDataListItem$}$
 </tbody></table>
</div>
</body>
</html>