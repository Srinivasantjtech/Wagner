<%@ Page Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" Inherits="MForgotPassWord"  Culture="en-US"
    UICulture="en-US" Codebehind="MForgotPassWord.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    


<script type="text/javascript">
    function CheckCusType() {

        switch (document.getElementById("ctl00_maincontent_CusType").value) {
            case 'Retailer':
                ShowRetailer();
                break;
            case 'Dealer':
                ShowDealer();
                break;

        }
    }

    function ShowDealer() {
        document.getElementById("ctl00_maincontent_txtLoginName").style.display = "block";
        document.getElementById("ctl00_maincontent_lblLoginName").style.display = "block";
    }

    function ShowRetailer() {
        document.getElementById("ctl00_maincontent_txtLoginName").style.display = "none";
        document.getElementById("ctl00_maincontent_lblLoginName").style.display = "none";
    }
   
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server"></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">

<div class="container margin_top">
  <div class="row">
 
    <div class="col-lg-8 margin_top_20 col-sm-12 col-lg-push-2">
      
      <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/MicroSiteimages/forgot.png" class="col-lg-4 col-sm-4 for_img">
      <div class="col-lg-8 margin_top_20 margin_bottom_30 col-sm-8">
      <p><strong>Password Assistance</strong></p>
     <p> To start resetting your password, type your email</p>
    
     <div class="form-group">
        <label for="exampleInputEmail3" class="sr-only">Email address</label>
       <asp:TextBox placeholder="Enter email" autocomplete="off" ID="txtUserMail" runat="server"  class="form-control border_radius_none height_49" AutoCompleteType="Disabled"></asp:TextBox> 
 <asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin"   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email" ControlToValidate="txtUserMail"></asp:RequiredFieldValidator>               
 <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtUserMail"   ErrorMessage="Required" 
Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
  
<%--        <input type="email" placeholder="Enter email" id="exampleInputEmail3" class="form-control border_radius_none height_49">--%>
         <asp:Button ID="btnSubmit" runat="server" meta:resourcekey="btnSubmit" OnClick="btnSubmit_Click" ValidationGroup="Mandatory" Class="btn green_bg white_color border_radius_none height_49 margin_top_20 width_100 font_weight" />
      <asp:Label ID="lblError" runat="server" Class="lblErrorSkin" ForeColor="#FF3300"></asp:Label>
      </div>
     
   
      </div>
    </div>
    
    <div class="clear"></div>
  </div>
</div>


<%--    <table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
 <tr> <td align="left" class=""> <a href="/Home.aspx" style="color: #0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight: bolder; font-size: small; font-style: normal"> / </font>Forgot Password </td> </tr>
 <tr> <td class="tx_3"> <hr> </td> </tr>
  <tr> <td> 
  <table id="tblError" width="558" runat="server" align="left"> <tr> <td align="left"> <asp:Label ID="lblError" runat="server" Class="lblErrorSkin"></asp:Label> </td> </tr> </table> </td> </tr></table>
   
     <asp:Panel ID="pnlUser" runat="server" DefaultButton="btnSubmit">
<table id="tblUserID" runat="server" class="BaseTblBorder" width="558" border="0" align="center">
  <tr> <td class="tx_6" height="20px" background="images/17.gif" align="left"> <asp:Label ID="lblAssistance" runat="server" meta:resourcekey="lblAssistance" ></asp:Label> </td> </tr>          
  <tr> <td height="40" align="left"> 
  <asp:Label ID="lblAssistanceMessage" runat="server" Class="lblNormalSkin" meta:resourcekey="lblAssistanceMessage"></asp:Label>
   </td> 
   </tr>
<tr>
 <td> 
 <table width="558">        
       
             <tr> 
             <td style="width: 77px" align="left"> 
             <asp:Label ID="lblUserID" runat="server" meta:resourcekey="lblUserID" Class="lblStaticSkin" Width="140px"></asp:Label> 
             </td> 
             <td align="left"> 
             &nbsp;<asp:TextBox autocomplete="off" ID="txtUserMail" runat="server" Width="160px" Class="textSkin" AutoCompleteType="Disabled"></asp:TextBox> 
             </td> 
             </tr>
                <tr style="visibility:hidden;"> 
          <td style="width: 77px" align="left">
           <asp:Label ID="lblLoginName" runat="server" meta:resourcekey="lblLoginName" Class="lblStaticSkin" Width="140px"></asp:Label> 
           </td> 
           <td align="left"> 
           &nbsp;<asp:TextBox autocomplete="off" ID="txtLoginName" runat="server" Width="160px" MaxLength="10" Class="textSkin" AutoCompleteType="Disabled"></asp:TextBox>
            </td> 
            </tr>
  <tr style="visibility:hidden;"> 
  <td align="left" style="width: 77px">
   <asp:Label ID="lblcustype" runat="server" Text="CustomerType :" Class="lblStaticSkin"></asp:Label>
    </td>
     <td align="left">
      <asp:DropDownList NAME="CusType" Width="165px" ID="CusType" runat="server" CssClass="inputtxt" >
       <asp:ListItem Text="Retailer" Value="Retailer">Retailer</asp:ListItem>
        </asp:DropDownList>
         </td> 
  </tr>
              </table> 
              </td>
               </tr></table>
<table width="558" border="0" id="tblusedidbtn" runat="server"> <tr> <td align="right" colspan="2"> <asp:Button ID="btnUser" runat="server" meta:resourcekey="btnUser" OnClick="btnUser_Click" Class="btnNormalSkin" /> </td> </tr> </table>
</asp:Panel>
<asp:Panel ID="pnlSQ" runat="server" DefaultButton="btnSubmit">
<table id="tblSecurityQuestion" runat="Server" visible="false" class="BaseTblBorder"  width="558" cellspacing="1">
  <tr> <td colspan="2" class="TableRowHead" align="left">
   <asp:Label ID="lblSecurityHeader" runat="server" meta:resourcekey="lblSecurityHeader"></asp:Label>
    </td> </tr>
  <tr> 
  <td colspan="2" height="40" align="left"> <asp:Label ID="lblSecurityQuestion" runat="server" Class="lblNormalSkin"></asp:Label> </td> </tr>
 <tr> <td style="width: 107px" align="left"> <asp:Label ID="lblYourAnswer" runat="server" meta:resourcekey="lblYourAnswer" Class="lblStaticSkin"></asp:Label> </td> <td align="left"> <asp:TextBox autocomplete="off" ID="txtYourAnswer" runat="server" Class="textSkin" AutoCompleteType="Disabled"></asp:TextBox> </td> </tr> </table>
 <table id="tblSecurityQuestionbtn" runat="server" width="558px" border="0" visible="false"><tr> <td colspan="2" align="left"> 
 <asp:Button ID="btnSubmit" runat="server" meta:resourcekey="btnSubmit" OnClick="btnSubmit_Click" Class="btnNormalSkin" /> </td> </tr></table></asp:Panel>
 --%>
 </asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server"></asp:Content>
