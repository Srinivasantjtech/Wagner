<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="RetailerRegistration"
    Title="Untitled Page"  Culture="en-US" Codebehind="RetailerRegistration.aspx.cs" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="Server">
<%--<link rel="stylesheet" type="text/css" media="screen" href="css/All_CSS_Master.css" /> --%>
<script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/All_JS_MASTER.js" type="text/javascript"></script>



<script type="text/javascript" lang="javascript">
    function MouseHover(ID) {
        switch (parseInt(ID)) {
            case 1:
                document.getElementById("ctl00_maincontent_ForgotPassword").style.backgroundColor = "#009F00";
                break;
            case 2:
                document.getElementById("ctl00_maincontent_Close").style.backgroundColor = "red";
                break;
        }
    }

    function MouseOut(ID) {
        switch (parseInt(ID)) {
            case 1:
                document.getElementById("ctl00_maincontent_ForgotPassword").style.backgroundColor = "#1589FF";
                break;
            case 2:
                document.getElementById("ctl00_maincontent_Close").style.backgroundColor = "#1589FF";
                break;
        }
    }
    </script>
    <script language="javascript" type="text/javascript">
        function checkEnableSubmit() {

           // alert(document.getElementById("ctl00_maincontent_chkterms").checked);
            if ((document.getElementById("ctl00_maincontent_chkterms").checked == false)) {
                document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "Required";
                alert('To Submit form you must agree to the Sales Terms and Conditions');
                return false;
            }

            validateSelection();
        }

     

        function checkEnableOthers() {
            if ((document.getElementById("ctl00_maincontent_chkother").checked == true) || (document.getElementById("ctl00_maincontent_chkother").checked == 'true') || (document.getElementById("ctl00_maincontent_chkother").checked == 1)) {
                document.getElementById("ctl00_maincontent_txtothers").disabled = false;
            }
            else {
                document.getElementById("ctl00_maincontent_txtothers").disabled = true;
            }
        }

        function checkselectedlist() {
             document.getElementById("ctl00_maincontent_ErrorStatusHiddenField").value = "1";
            return true;
        }

        function validateSelection() {
            if (checkselectedlist() == true) {

                if (document.getElementById("ctl00_maincontent_chkterms").checked == true) {
                    document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                    document.getElementById("ctl00_maincontent_btnsubmit").style.fontWeight = 'bold';
                    document.getElementById("ctl00_maincontent_btnsubmit").style.backgroundColor = "#ff0000";
                  
                    document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "*";
                    return true;
                }
                else {
                    document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                  
                    document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "Required";
                    return false;
                }
            }
            else {
                document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "*";
                document.getElementById("ctl00_maincontent_chkterms").checked = false;
               
                return false;
            }
        }




       
    </script>
     <asp:Panel ID="pnlnewProfile" runat="server" DefaultButton="btnsubmit">   
        <table cellpadding="0" cellspacing="0" border="0" width="790px" valign="top">           
            <tr><td  align="left"  style="text-align:left;margin-left:0px;" >
                <div class="span9 box1" style="width: 760px;margin-left:0px;">
               <h3 class="title1" align="left">Retailer Customer Sign Up</h3>
               <div class="img_left" style="margin-top:10px;margin-bottom:10px;">
               <table cellspacing="0" cellpadding="0" border="0" valign="center">
               <tr>
                <td>
                <img width="106" height="80" alt="img" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/page_icon1.png">
                </td>           
                    <td align="left">
                       <p class="p3" style="margin-left:30px;">               
                       Please fill in form below to create a new account.<br />
                       <asp:Label ID="Label19" runat="server" Class="lblRequiredSkin" Text="*" Visible="true" Width="1px"></asp:Label>&nbsp; &nbsp;<asp:Label ID="Label20" runat="server" CssClass="red" Text="Required Fields"  Visible="true">
                        </asp:Label>
                        </p>
                    </td>
                </tr>
                </table>
                </div>
               
             

               <table cellpadding="0" cellspacing="0" border="0" width="760px" valign="top">
               <tr><td align="left" >
               <div class="box1">
               <h3 class="title3">Company Details</h3>
                        <table cellspacing="0" width="743px" cellpadding="0" >

                        <tr><td class="tx_1" align="left"><table cellpadding="0" cellspacing="0" border="0">
                        <tr><td width="170px" align="left" ><span class="form_1">
                                            Company / Account Name:<asp:Label ID="Label18" runat="server" Class="lblRequiredSkin"
                                                Visible="true" Text="" Width="1px"></asp:Label></span>
                       </td>
<td width="250px" align="left" ><span class="form_2"><asp:TextBox runat="server" ID="txtcompname" Text=""   CssClass="input_dr"  MaxLength="50"></asp:TextBox></span> </td>
<td align="left" width="150px"></td>
<td align="right" width="100px"> 
<%----%>
</td></tr>              
<tr><td align="left"><span class="form_1">ABN / ACN / Company Number:
<asp:Label ID="Label21" runat="server" Class="lblRequiredSkin" Visible="true" Text="" Width="1px"></asp:Label></span></td>
<td align="left"><span class="form_2"> <asp:TextBox runat="server" ID="txtcompno" Text="" CssClass="input_dr"  MaxLength="20"></asp:TextBox></span></td>
 <td align="left">&nbsp;&nbsp;</td></tr> </table></td></tr></table>
 </div> 
 </td>
 </tr>
<tr>
<td align="left">
<div class="box1"><h3 class="title3">Contact Details</h3>
<table cellspacing="0" width="714px" cellpadding="0" > <tr><td class="tx_1" align="left">
<table cellpadding="0" cellspacing="0" border="0"> <tr>
<td width="150px"><span class="form_1">First Name:<asp:Label ID="Label9" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label></span> </td>
 <td width="250px"><span class="form_2"><asp:TextBox runat="server" ID="txtfname" Text="" CssClass="input_dr"  MaxLength="20" ></asp:TextBox></span> </td> <td align="left" width="140px">
&nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvfname" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter First Name"  ControlToValidate="txtfname"></asp:RequiredFieldValidator></td>
<td align="right" width="110px">
<%--<asp:Label ID="Label16" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
&nbsp;<asp:Label ID="Label17" runat="server" Visible="true" Text="Required Fields" CssClass="red"></asp:Label>--%></td> </tr>                                 
<tr> <td><span class="form_1">Last Name:<asp:Label ID="Label10" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label></span> </td>
<td><span class="form_2"> <asp:TextBox runat="server" ID="txtlname" Text="" CssClass="input_dr" MaxLength="20"></asp:TextBox></span>  </td> <td align="left">
&nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvlname" runat="server" Class="vldRequiredSkin"  ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Last Name"   ControlToValidate="txtlname"></asp:RequiredFieldValidator> </td></tr>
<tr><td><span class="form_1">Phone:<asp:Label ID="Label12" runat="server" Class="lblRequiredSkin" Visible="true"  Text="" Width="1px">*</asp:Label></span>  </td>
  <td><span class="form_2"> <asp:TextBox runat="server" ID="txtphone" Text="" CssClass="input_dr"
MaxLength="16"></asp:TextBox></span>  </td>
<td align="left">&nbsp;&nbsp; <asp:RequiredFieldValidator ID="rfvphone" runat="server" Class="vldRequiredSkin"  ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Phone"  ControlToValidate="txtphone"></asp:RequiredFieldValidator>&nbsp;&nbsp;
<asp:FilteredTextBoxExtender ID="ftePhone" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtphone" /></td></tr>
 <tr><td><span class="form_1">Mobile/Cell Phone:</span> </td>
<td><span class="form_2"><asp:TextBox runat="server" ID="txtMobile" Text="" CssClass="input_dr"
MaxLength="16"></asp:TextBox></span> </td> <td align="left">&nbsp;&nbsp;<asp:FilteredTextBoxExtender ID="fteMobile" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtMobile" /> </td></tr>
 <tr><td><span class="form_1"> Fax:<asp:Label ID="Label13" runat="server" Class="lblRequiredSkin" Visible="true"  Text="" Width="1px"></asp:Label></span> </td>
 <td><span class="form_2"><asp:TextBox runat="server" ID="txtfax" Text="" CssClass="input_dr" MaxLength="16"></asp:TextBox> </span> </td>
  <td align="left">&nbsp;&nbsp;<asp:FilteredTextBoxExtender ID="ftefax" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtfax" /> </td></tr>                                  
 <tr><td><span class="form_1">Email:<asp:Label ID="Label14" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label></span></td>
 <td><span class="form_2"> <asp:TextBox runat="server" ID="txtemail" Text="" CssClass="input_dr"  MaxLength="55"></asp:TextBox></span></td>
<td align="left"> &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin"   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email" ControlToValidate="txtemail"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtemail"   ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator></td> </tr>
<tr><td><span class="form_1"> Confirm Email:<asp:Label ID="Label15" runat="server" Class="lblRequiredSkin" Visible="true"  Text="*" Width="1px"></asp:Label></span>  </td>
 <td><span class="form_2"><asp:TextBox runat="server" ID="txtcemail" Text="" CssClass="input_dr"  MaxLength="55"></asp:TextBox></span> </td>
   <td align="left"> &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvcemail" runat="server" Class="vldRequiredSkin"
ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Confirm Email"
 ControlToValidate="txtcemail"></asp:RequiredFieldValidator>                                         
 <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtcemail" ControlToCompare="txtemail" runat="server" ErrorMessage="Confirm Email and Email should be same" ValidationGroup="Mandatory" Display="Dynamic" ></asp:CompareValidator>
 </td></tr>
 <tr><td><span class="form_1">Password:<asp:Label ID="Label1" runat="server" Class="lblRequiredSkin" Visible="true"
Text="*" Width="1px"></asp:Label></span></td><td><span class="form_2"> <asp:TextBox runat="server" ID="TxtPassword" Text="" CssClass="input_dr"  TextMode="Password" 
 MaxLength="10"></asp:TextBox></span> </td><td align="left">
&nbsp;&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Class="vldRequiredSkin"
ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Password" ControlToValidate="TxtPassword"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic"  ControlToValidate="TxtPassword"  ValidationGroup="Mandatory"  ValidationExpression="^[a-zA-Z0-9\s]{6,10}$"  runat="server" ErrorMessage="Password must contain alphabet and numeric,and length should be 6 to 10"></asp:RegularExpressionValidator>
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic"  ControlToValidate="TxtPassword"  ValidationGroup="Mandatory"  ValidationExpression=".*[0-9].*"  runat="server" ErrorMessage="Password must contain one numeric"></asp:RegularExpressionValidator>
<asp:RegularExpressionValidator ID="RegularExpressionValidator3" Display="Dynamic"  ControlToValidate="TxtPassword"  ValidationGroup="Mandatory"  ValidationExpression=".*[a-zA-Z].*"   runat="server" ErrorMessage="Password must contain one alphabet"></asp:RegularExpressionValidator>                   
 </td></tr>
 <tr><td><span class="form_1">Confirm Password:<asp:Label ID="Label3" runat="server" Class="lblRequiredSkin" Visible="true"
Text="*" Width="1px"></asp:Label></span></td>
<td><span class="form_2"><asp:TextBox runat="server" ID="TxtConfirmPassword" Text="" CssClass="input_dr" TextMode="Password" 
 MaxLength="10"></asp:TextBox></span> </td>
   <td align="left"> &nbsp;&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Class="vldRequiredSkin"
ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Confirm Password" ControlToValidate="TxtConfirmPassword"></asp:RequiredFieldValidator>
<asp:CompareValidator ID="CompareValidator2" ControlToValidate="TxtConfirmPassword" ControlToCompare="TxtPassword"  runat="server" ErrorMessage="Confirm Password and Password should be same" ValidationGroup="Mandatory"  Display="Dynamic"></asp:CompareValidator>
  </td></tr>

  <tr><td><span class="form_1">Subscribe mail: </span></td>
<td><span class="form_1" style="text-align:left;" >&nbsp;
    <asp:CheckBox ID="chkissubscribe" runat="server" Checked="true" style="width:10px;"    TextAlign="Right"/>
    
</span> </td>
   <td align="left">
  </td></tr>

  </table> </td></tr></table>
  </div> 
   </td>
   </tr>


 <tr>
 <td align="left">
<div class="box1">
 <h3 class="title3">Address Details</h3>
 <table cellspacing="0" width="714px" cellpadding="0" ><tr><td class="tx_1" align="left"><table cellpadding="0" cellspacing="0" border="0">
   <tr><td width="180px"><span class="form_1">  Street Address:<asp:Label ID="Label5" runat="server" Class="lblRequiredSkin" Visible="true"   Text="*" Width="1px"></asp:Label></span> </td>
 <td width="250px"><span class="form_2"><asp:TextBox runat="server" ID="txtsadd" Text="" CssClass="input_dr"   MaxLength="30"></asp:TextBox></span> </td>
<td align="left" width="140px">  &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvsadd" runat="server" Class="vldRequiredSkin"
   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Street Address"   ControlToValidate="txtsadd"></asp:RequiredFieldValidator></td>
   <td align="right" width="110px"><%--<asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"  Width="1px"></asp:Label>
  &nbsp;<asp:Label ID="Label4" runat="server" Visible="true" Text="Required Fields" CssClass="red"></asp:Label>--%></td> </tr>
                                    
<tr><td><span class="form_1"> Address Line 2</span> </td>
<td><span class="form_2"><asp:TextBox runat="server" ID="txtadd2" Text="" CssClass="input_dr"  MaxLength="30"></asp:TextBox></span>  </td><td></td></tr>                                  
<tr><td><span class="form_1">  Suburb/Town:<asp:Label ID="Label6" runat="server" Class="lblRequiredSkin" Visible="true"   Text="*" Width="1px"></asp:Label></span>  </td>
 <td><span class="form_2"><asp:TextBox runat="server" ID="txttown" Text="" CssClass="input_dr"  MaxLength="30"></asp:TextBox></span> </td>
 <td align="left">&nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvtown" runat="server" Class="vldRequiredSkin"   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Suburb/Town"  ControlToValidate="txttown"></asp:RequiredFieldValidator></td></tr><tr>
<td><span class="form_1"> State/Province:<asp:Label ID="Label7" runat="server" Class="lblRequiredSkin" Visible="true"   Text="*" Width="1px"></asp:Label></span> </td>
<td><span class="form_2"><asp:TextBox runat="server" ID="txtstate" Text="" CssClass="input_dr" MaxLength="20"></asp:TextBox></span> </td>
 <td align="left">&nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvstate" runat="server" Class="vldRequiredSkin"
   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter State/Province"  ControlToValidate="txtstate"></asp:RequiredFieldValidator></td></tr>                               
<tr><td><span class="form_1">Post/Zip Code:<asp:Label ID="Label8" runat="server" Class="lblRequiredSkin" Visible="true"  Text="*" Width="1px"></asp:Label></span> </td>
<td><span class="form_2"> <asp:TextBox runat="server" ID="txtzip" Text="" CssClass="input_dr" MaxLength="10" ></asp:TextBox></span> </td>
<td align="left">&nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvzip" runat="server" Class="vldRequiredSkin"
 ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Post/Zip Code"  ControlToValidate="txtzip"></asp:RequiredFieldValidator>&nbsp;&nbsp;
 <asp:FilteredTextBoxExtender ID="ftezip" runat="server" FilterMode="ValidChars" ValidChars="1234567890"  TargetControlID="txtzip" /> </td> </tr>                                  
    <tr><td><span class="form_1"> Country:</span> </td>
  <td><span class="form_2"> <asp:DropDownList ID="drpCountry" runat="server" Width="250px" Class="DropdownlistSkin"> </asp:DropDownList></span> </td>
   <td></td></tr></table></td></tr> </table>
   </div>
   </td>
   </tr>
 <tr> 
 <td align="left">
  <div class="box1"><h3 class="title3">Submit Form</h3>
  <table cellpadding="10px" cellspacing="0" border="0">
 <tr><td class="tx_1" colspan="2" style="font-size: 12px;" > Form Verify. Please enter text code shown below:</td></tr>
 <tr><td colspan="2"><cc1:CaptchaControl ID="cVerify" CaptchaChars="23456789" CaptchaLength="4" CaptchaBackgroundNoise="Low"  runat="server" CustomValidatorErrorMessage="Invalid Verification Code" CaptchaMaxTimeout="300" CaptchaMinTimeout="2" /></td></tr>
 <tr><td colspan="2"><table cellpadding="0" cellspacing="0" border="0"><tr><td  style="font-size: 12px;" >
                                            Text Code: <span style="color: Red">*</span> &nbsp;
                                            <asp:TextBox ID="cText" runat="server" value="" CssClass="input_dr" ></asp:TextBox></td>
  <td> &nbsp; <asp:Label ID="cVerifyMsg" runat="server" ForeColor="Red" Text="" Visible="false" /></td></tr></table></td> </tr></table>
                         <table cellpadding="2" cellspacing="3" border="0">
  <tr><td align="center"> <input id="chkterms" type="checkbox" runat="server" onclick="javascript:checkEnableSubmit();" /></td>
  <td class="tx_1"> <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse"><tr> <td>
                                          
  <div style="line-height: 14px;color:#333"><span class="txtnorm">I have read and understand<asp:LinkButton ID="myLink" CssClass="modal" style="color: #01AEF0;text-decoration: none;" ToolTip="Sales Terms and Conditions" Text="Sales Terms and Conditions" runat="server"  /></span>

    </div>  </td><td>
   &nbsp;
<asp:Label ID="lblCheckTerms" runat="server" ForeColor="Red" Text="*" Visible="false" />
   </td></tr></table></td></tr></table> </div>
   </td>
   </tr>
                     <tr>
                     <td align="center" >
                     <asp:Button ID="btnsubmit" class="button normalsiz btngreen btnmain" Text="Submit" runat="server" ValidationGroup="Mandatory"  OnClick="btnsubmit_Click" OnClientClick="return checkEnableSubmit()" />                
              </td> 
              </tr>
              </table>
                </div> 
                </td> 
                </tr>
            <tr><td align="left"></td></tr>
            <tr><td align="center"></td></tr>
            <tr><td align="center"></td></tr>
            <tr><td align="center">
            <table cellpadding="0" cellspacing="0" border="0">
            <tr>
            <td>
            </td>
            </tr>
            </table>
            </td> 
            </tr>
            </table>
        <asp:HiddenField ID="ErrorStatusHiddenField" runat="server" />
    </asp:Panel>
<asp:Panel ID="pnlTAC" runat="server" Width="650px"  Style="display:none; "    >
<a href = "javascript:Hidepopup()" class="testbutton" ></a>
<div class="boxfull" style="width:575px;height:500px;overflow:scroll;">
<table width="96%" border="0" cellpadding="0" cellspacing="0">
  <tr><td align="center" valign="top"> <table width="100%" border="0" cellpadding="0" cellspacing="0">
  <!--DWLayoutTable-->
  <tr><td height="85" valign="top" align="left"><table width="100%" border="0" cellpadding="0" cellspacing="0">
        <!--DWLayoutTable-->
        <tr><td width="100%" height="85" valign="top">
            <p class="bold_txt2">Terms &amp; Conditions</p>
            <p>Find out more about Wagner. If you can't find the information you need, please use the Make an Enquiry page or phone 1100 13 933 and get help from a Customer Service representative.</p>
          </td></tr> </table></td></tr>
  <tr><td height="100%" valign="top"><table width="100%" border="0" cellpadding="0" cellspacing="0">
        <!--DWLayoutTable-->
        <tr><td height="28" colspan="3" class="tab6" align="left"><strong>GENERAL INFORMATION AND CONDITIONS </strong> </td></tr>
        <tr><td width="6" height="6" valign="top" class="cr_11"></td> <td width="708" valign="top" class="bdr_top" height="6"></td><td width="6" valign="top" height="6" class="cr_13"></td></tr>
        <tr><td height="314" valign="top" class="bdr_left">
      <%--  <img src="images/Cr_14.png" width="6" height="10" />--%>
       <%--  <img class="Cr_14" width="6" height="10" />--%>
        </td>
          <td valign="top" style="text-align:justify">
            <p><strong>WAGNER ELECTRONICS:</strong>  Is an independent Australian organisation dedicated to the Electronics Industry. Our aim is to give you the BEST possible service at COMPETITIVE prices.</p>
            <p><strong>THE PRODUCTS</strong>:  Goods offered are from reliable and internationally known manufacturers and are of the highest quality. Data and specifications are those quoted by our suppliers which we believe to be correct.</p>
            <p><strong>PRICES:</strong>  This price list supersedes all previous price lists and although correct at the time of printing, prices are subject to change without notice. All prices INCLUDE GST.</p>
            <p><strong>DELIVERY</strong>:  A delivery and handling charge applies to all orders. Please note that goods are sent at the purchasers risk. Minimum surface mail charge is &#36;6.90. Parcels over 500g attract parcel rates and in most cases the mail charges are more than courier. Therefore, for heavier or urgent orders we recommend that the courier service be used. In most cases they deliver on the next working day, thus courier can be both convenient and cost effective.</p>
            <p><strong>PICKUP</strong>:  Wagner Electronics is open 8:30 to 5.00 Mon-Fri, 9:00 to 4:00 on Sat.</p>
            <p><strong>LOCAL:</strong>  (Sydney Metropolitan Road Service) &#36;6.90. This is an overnight service.</p>
            <p><strong>LOCAL SAME DAY:</strong>  (Sydney Metro') &#36;8.50. Orders must be received before 10AM. Delivery is between 2PM and 5.30PM. Please state clearly on your order &quot;SAME DAY DELIVERY&quot;.</p>
            <p><strong>COUNTRY and INTERSTATE:</strong>  Air service is &#36;9.90 up to 3Kg.</p>
            <p><strong>HEAVY PARCELS:</strong>  (Above 3Kg) will be sent by road freight at no extra charge. Please allow 2-3 working days for delivery. Some country areas may take up to 7 days.</p>
            <p><strong>DANGEROUS GOODS:</strong>   Orders with aerosols or isopropanol products must be sent by road.</p>
            <p><strong>BACK ORDERS:</strong>   Goods not in stock will be automatically back ordered unless otherwise requested.</p>
            <p><strong>CLAIMS: </strong>   The use of our  products is totally beyond our control or supervision. We therefore  cannot accept any responsibility for losses or consequential damage to  any goods or equipment. A general Three month warranty applies to most  products from date of invoice. Manufacturers, however, may vary this  period at their discretion. PLEASE NOTE. No warranty applies to some  parts as faults not correctly diagnosed or other faults in associated  circuitry can damage newly fitted parts, e.g. semiconductors, etc. <br /><br />
              <strong>GOODS RETURNED: </strong>   Please, it is essential you obtain authorization before returning any  goods. All goods to be returned must be freight prepaid and accompanied  with a copy of the original purchase invoice. Faulty goods will be  replaced only. Not credited. Goods purchased incorrectly or no longer  required will not be accepted for credit. <br />
              <br />
              <strong>OWNERSHIP OF GOODS: </strong>   Ownership of all goods and materials supplied by WAGNER Components  remains the property of WAGNER Components, in accordance with the  retention of title clause, until all goods supplied are paid for by the  customer. <br />
              <br /> <strong>ERRORS/OMISSIONS: </strong>   Due to the size of this catalogue and the thousands of products it  contains, it is possible that mistakes exist. All care has been taken  to eliminate errors, but no responsibility can be accepted. Therefore,  we recommend that this catalogue be used as a guide only. All prices  are correct at the time of printing, but may change without notice.  Please tell us if you discover any discrepancies. We will advise you of  corrections as they become available. See WAGNER NEWS</p></td>
          <td valign="top" class="bdr_right"> <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Cr_15.png" width="6" height="10" /> </td></tr>
        <tr><td height="6" valign="top" class="cr_16"></td><td valign="top" class="bdr_bottom" width="10" height="6"></td>
          <td valign="top" class="cr_18" width="6" height="6"></td></tr></table></td></tr><tr><td>&nbsp;</td></tr></table></td></tr></table></div> </asp:Panel>
<asp:ModalPopupExtender ID="TACpopup" PopupControlID="pnlTAC" BackgroundCssClass="modalBackgroundpopup"  BehaviorID="testTACpopup"
    DropShadow="true" runat="server" TargetControlID="myLink" RepositionMode="None"   >
</asp:ModalPopupExtender>

 <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none; visibility: hidden">
 </asp:Button>
    <div id="PopupOrderMsg" align="center" runat ="server">
        <asp:Panel ID="ModalPanel" runat="server" CssClass="PopUpDisplayStyle">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;" align="center">
                <tr style="height: 5px"><td colspan="3">&nbsp;</td> </tr>
                <tr style="height: 10px"> <td width="100%" align="center" colspan="3">&nbsp;</td></tr>
                <tr style="height: 10px"> <td width="100%" align="center" colspan="3" class="TextContentStyle">
                       The email address you are registering your account with exists already.
                        <br /> Would you like to continue with ForgotPassword?</td></tr>
                <tr style="height: 10px"><td width="100%" align="center" colspan="3">&nbsp;</td> </tr>
                <tr style="height: 5px"><td colspan="3"> &nbsp;</td></tr>
                <tr style="height: 10px"> <td width="45%" align="right"><asp:Button ID="ForgotPassword" runat="server" Text="Forgotten Password"
                            Width="205px"  CssClass="ButtonStyle" OnClick="btnForgotPassword_Click" /></td>
                    <td width="10%">&nbsp;</td>
                    <td width="45%" align="left"><asp:Button ID="Close" runat="server" Text="Close" Width="165px"
                            CssClass="ButtonStyle" OnClick="btnClose_Click" />
                    </td></tr></table> </asp:Panel></div>
</asp:Content>
