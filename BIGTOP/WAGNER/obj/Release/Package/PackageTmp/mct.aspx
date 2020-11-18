<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mct.aspx.cs" Inherits="WES.mct" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Contentbreadcrumb" runat="server">

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Banner" runat="server">

<%GetSupplierdetail(); %>
    <div data-ride="carousel" class="carousel slide" id="myCarousel">
  <!-- Indicators -->
  <ol class="carousel-indicators hidden">
    <li class="" data-slide-to="0" data-target="#myCarousel"></li>
    <li data-slide-to="1" data-target="#myCarousel" class="active"></li>
    <li data-slide-to="2" data-target="#myCarousel" class=""></li>
  </ol>
  <div class="carousel-inner">
    <div class="item"> 
  <%--  <img alt="First slide" style="width:100%" src="/images/MicroSiteimages/ms_ banner.png"/>--%>
  <%           string bnr1href = string.Empty;
               string bnr2href = string.Empty;
               string bnr3href = string.Empty;
      if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("spf-vipvision") == true)
      {
          bnr1href = "https://www.wagneronline.com.au/vip-vision/ip-cameras-vip/919-90909/mpl/";
          bnr2href = "https://www.wagneronline.com.au/vip-vision/ip-camera-nvr-kits/919-909060909/mpl/";
          bnr3href = "https://www.wagneronline.com.au/vip-vision/ip-nvr-systems-vip/919-9090909/mpl/";
      }
      else
      {
          bnr1href = "#";
          bnr2href = "#";
          bnr3href = "#";
      }
   %>
 
  <a href="<%=bnr1href %>"> <img alt="First slide" style="width:100%" src="<%=strsupplier_bannar1 %>"/>
  </a> 
      <div class="container">
        <div class="carousel-caption hidden">
          <h1>Slide 1</h1>
          <p>Aenean a rutrum nulla. Vestibulum a arcu at nisi tristique pretium.</p>
          <p><a role="button" href="#" class="btn btn-lg btn-primary">Sign up today</a></p>
        </div>
      </div>
    </div>
    <div class="item active"> <%--<img alt="Second    slide" data-src="" style="width:100%" src="/images/MicroSiteimages/banner.png"/>--%>
   <a href="<%=bnr2href %>"> <img alt="Second slide"  style="width:100%" src="<%=strsupplier_bannar2 %>"/>
   </a>
      <div class="container">
        <div class="carousel-caption hidden">
          <h1>Slide 2</h1>
          <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vitae egestas purus. </p>
          <p><a role="button" href="#" class="btn btn-lg btn-primary">Learn more</a></p>
        </div>
      </div>
    </div>
      <%       
          if (HttpContext.Current.Request.Url.ToString().Contains("SPF-JUDGECCTV") == false)
      { 
          %>
    <div class="item"> <%--<img alt="Third slide" data-src="" style="width:100%" src="/images/MicroSiteimages/banner.png"/>--%>
    <a href="<%=bnr3href %>">
    <img alt="Third slide"  style="width:100%" src="<%=strsupplier_bannar3 %>"/>
    </a>
     
      <div class="container">
        <div class="carousel-caption hidden">
          <h1>Slide 3</h1>
          <p>Donec sit amet mi imperdiet mauris viverra accumsan ut at libero.</p>
          <p><a role="button" href="#" class="btn btn-lg btn-primary">Browse gallery</a></p>
        </div>
      </div>
    </div>
      <% } %>
  </div>
  <%--src="/images/MicroSiteimages/arrow_left.png"--%>
  <a data-slide="prev" href="#myCarousel" class="left carousel-control"><span class="glyphicon glyphicon-chevron-left">
  <img  class="sprite sprite-arrowleft"  alt=""/>
  </span></a> <a data-slide="next" href="#myCarousel" class="right carousel-control">
  <span class="glyphicon glyphicon-chevron-right">
  <img class="sprite sprite-arrowright" alt="" />
  </span></a> </div>

    <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none;  visibility: hidden"></asp:Button>
    <%--<div id="PopupOrderMsg" align="center" runat ="server" class="modal-dialog modal-lg"> 
           <asp:Panel ID="ModalPanel" runat="server" CssClass="modal-dialog modal-lg">
           <div class="modal-content">
           <div class="modal-header blue_color padding_top padding_btm">
          <h4 class=" white_color font_weight modal-title text-center" id="myLargeModalLabel">Welcome to WAGNER online store.</h4>
         
        </div>
        <div class="modal-body">
        <p class="text-center">There has been Product items found still in your shopping cart from your last login, Would you like to continue with this order?</p>
        <div class="modal-footer text-center">
        
        </div>
        </div>
    
    </div>
    </asp:Panel>
    </div>--%>

    <div id="PopupOrderMsgNew" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6); display: block;" class="modal fade bs-example-modal-lg in">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
    <div class="close-selected">
         <asp:ImageButton ID="btnclose" runat="server"   OnClick="btnContinueOrder_Click" />
                        
                           
                      </div>
        <div class="modal-header blue_color padding_top padding_btm">
          <h4 class=" white_color font_weight modal-title text-center" id="H2">Welcome to <%= strsupplierName %>  Wagner online store.</h4>
         
        </div>
        <div class="modal-body">
         <p class="text-center">There has been Product items found still in your shopping cart from your last login,
Would you like to continue with this order?</p>
         
<div class="modal-footer text-center">

       <%--  <input type="submit" value="Yes, Continue with Previous Order" class="btn-lg  padding_top green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub">
         <input type="submit" value="No, Cancel Previous Order" class="btn-lg  padding_top btn-danger border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub">--%>
         <asp:Button ID="ContinueOrder" runat="server" Text="Yes, Continue with Previous Order"  CssClass="btn-lg  padding_top green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mob_100" OnClick="btnContinueOrder_Click" />
    <asp:Button ID="ClearOrder" runat="server" Text="No, Cancel Previous Order"   CssClass="btn-lg  padding_top btn-danger border_none border_radius_none white_color semi_bold font_14 mar_right_30  margin_top mob_100" OnClick="btnClearOrder_Click" />
         </div>
        </div>
      </div>
  </div>
</div>

 <div id="PopupRetailerLoginMsg" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6); display: block;" class="modal fade bs-example-modal-lg in">
    <%-- <asp:Panel ID="ModalPanel1" runat="server" CssClass="PopUpDisplayStyle" Visible="false">--%>
   <%-- <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"  align="center">
     <tr style="height: 5px"> <td colspan="3"> &nbsp;</td> </tr>
      <tr style="height: 10px"> <td width="100%" align="center" colspan="3"> &nbsp; </td></tr>
      <tr style="height: 10px"> <td width="100%" align="center" colspan="3" class="TextContentStyle"> Your Account Has Not Been Activated!  <br />
      Please check your email for an email from us containing an activation / confirmation link.
      <br /> If you would like us to send you the Activation Email again. <a Href="/ConfirmMessage.aspx?Result=REMAILACTIVATION" class="toplinkatest">Please Click Here</a>
        </td> </tr> <tr style="height: 10px"><td width="100%" align="center" colspan="3"> &nbsp;</td> </tr>
         <tr style="height: 5px"><td colspan="3"> &nbsp; </td> </tr>
         <tr style="height: 10px">  <td width="100%" align="center" colspan="3">
         <asp:Button ID="btnCancel" runat="server" Text="Close"   Width="205px" CssClass="ButtonStyle" OnClick="btnCancel_Click" /> </td></tr></table>--%>
           <%-- <div style="left: -632px; top: -200px; position: absolute; width: 1390px; height: 1800px; background-color: rgb(0, 0, 0); opacity: 0;"></div>
             <div  style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6);" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
 --%>
  <div class="modal-dialog modal-lg">
    <div class="modal-content">

        <div class="modal-header blue_color padding_top padding_btm">

     <%--     <button aria-label="Close" data-dismiss="modal" class="close white_bg" type="button">
          <span aria-hidden="true">×</span></button>--%>
          <h4 id="H1" class=" white_color font_weight modal-title">Your Account Has Not Been Activated! </h4>
         
        </div>
        <div class="modal-body">

     
         <p>  Please check your email for an email from us containing an activation / confirmation link.
      <br /> If you would like us to send you the Activation Email again. <a Href="/mConfirmMessage.aspx?Result=REMAILACTIVATION" style="color:#15c;" >Please Click Here</a></p>
        
<div class="modal-footer">
         <asp:Button ID="btnCancel" runat="server" Text="Close"   Width="205px" class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100" OnClick="btnCancel_Click" />
      
         </div>
        </div>
      </div>
  </div>
</div>
         
         
       <%--  </asp:Panel>--%>
         
         </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
 
   <div class="col-lg-9 col-md-9 col-sm-8 col-xs-12 padding_left_right">

<%--productbuy code moved to MS_Alljs--%>
    
          <% =ST_NewProduct()%>


     
      </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="footer" runat="server">

    <link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/micrositecss/thickboxAddtocart_MS.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />
<%--<script language="javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/thickboxaddtocart.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript" />--%>
</asp:Content>