<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true"  Inherits="RegistrationResult"
    Culture="en-US" UICulture="en-US" Codebehind="ConfirmMessage.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript">    window.history.forward(1);

    window.location.hash = "1";
    window.location.hash = "1"; //again because google chrome don't insert first hash into history
    window.onhashchange = function () { window.location.hash = "1"; }
    
     </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server"></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
 <%--   <table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
    <tr><td align="left" class="tx_1"><a href="/Home.aspx" style="color: #0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif;
                    font-weight: bolder; font-size: small; font-style: normal"> / </font>Confirm Message     </td> </tr>
        <tr><td class="tx_3"> <hr>  </td> </tr> </table>  <br />--%>
    <% if (Request.QueryString["Result"] != null)
       {
           if (Request.QueryString["Result"] == "NOECOMMERCE")
           {%>
    <asp:Label ID="lblmsg" runat="server" meta:resourcekey="lblmsg"></asp:Label>
    <% }
             if (Request["Result"] == "REGISTRATION")
             {
                 string regHtml = "<table cellspacing=\"0\" width=\"588px\" cellpadding=\"0\" style=\"background-color: #F2F2F2; border: thin solid #E7E7E7\"><tr><td  class=\"tx_7_blue\" align=\"left\" colspan=\"2\">Registration Confirmation.</td></tr><tr><td  class=\"tx_7_blue\" align=\"left\" colspan=\"2\">&nbsp;</td></tr><tr><td width=\"2%\">&nbsp;</td><td class=\"tx_1\" align=\"left\"><b>Your Details have been received for processing.</b><br /><b>You should receive an email / phone call from us within the next 1-2 business days.</b><br /><br /><b>Should you not hear from us in this time frame please contact us on:</b><br /><br /><b>Email: <a href=\"mailto:sales@wes.net.au\" class=\"tx_3\">sales@wes.net.au</a></b><br /><b>Phone: +61 2 9797-9866</b><br /></td></tr></table>";
                 Response.Write(regHtml);
             }
         }%>


         <div class="container">
	<div class="row hidden-xs hidden-sm">
    	<div class="col-sm-20">
        	<ul id="mainbredcrumb" class="breadcrumb">
            	<li><a href="home.aspx">Home</a></li>
                <li class="active">Confirm Message</li>
            </ul>
        </div>
    </div>
    <!--<div class="row" >
    	<div class="col-md-20 main-left hidden-sm hidden-xs">
        	<div class="categorysearch categoryheading clearfix">
                <h4>New Products</h4>
            </div>
       	</div>
    </div> -->
    
    <div class="row">
    	<div class="categoryheading">
        </div>
		
        <div class="col-sm-20">
          <% if (Request.QueryString["Result"] != null)
             {
                 if (Request.QueryString["Result"] == "QTEEMPTY")
                 {%>
          <img class="col-lg-4 col-sm-4 for_img nolftpadd" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/empty_cart.png"/>
          <% } 
              %>
              <%else
                 { %>
             <img class="col-lg-4 col-sm-4 for_img nolftpadd" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/confirmation.png"/>
             <% }
             }%>
          <div class="col-lg-8 margin_top_20 margin_bottom_30 col-sm-8">
          <p></p>
          <p class="margin_top_30"> <b>
           <asp:Image ID="Imgstat" runat="server" Visible="false" />
    <asp:Label ID="lblConfirmmsg" runat="Server" Class="lblResultSkin"></asp:Label>
    <asp:Label ID="lblErrormsg" runat="Server" Class="lblErrorSkin"></asp:Label>
    <asp:Label ID="lblCartEmpty" runat="Server" Class="lblErrorSkin"></asp:Label>


    <asp:HyperLink ID="lnkResult" Class="HLCommonLinkSkin" runat="server" NavigateUrl="/Login.aspx"></asp:HyperLink>

          </b> </p>
              <asp:Button ID="btnContinueNext" runat="server"  
 OnClick="btnContinueNext_Click" Text="Start Shopping"   class="btn btn-primary"  visible="false" />
          </div>
        </div>
        
        <div class="clearfix"></div>
          </div>
          
</div>

   
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
