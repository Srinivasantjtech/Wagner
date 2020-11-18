<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mOrderReport.aspx.cs" Inherits="WES.mOrderReport" %>

<%@ Register TagPrefix="WebCat" TagName="Invoice" Src="~/UI/mInvoice.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Order Report</title>
    <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/Micrositecss/bootstrap.min.css" rel="stylesheet" type="text/css" />
   <style type="text/css">

   .logoforORTOP1
{
     width:360px;
     height:84px;
     display:block;
     margin-top:40px;
}
   </style>
</head>
<script type="text/javascript" language="javascript">
    //  window.history.back(1);
</script>
<body>
    <form id="form1" runat="server">
    <div class="container">
    <div class="row">
    <div class="col-lg-10">
    <div class="col-lg-8 col-sm-6 padding_left_right_mob margin_top">
<p class="padding_left">
    <asp:LinkButton ID="lbtnPrint" runat="Server"  OnClientClick="javascript:window.print();return false;">
 
<%--<a href="javascript:window.print();return false;">--%>
<img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/micrositeimages/Print.png" />
<%--</a>--%>
   </asp:LinkButton> 
</p>
<h4 class="modal-title margin_top" id="myLargeModalLabel">
<img class="  img-responsive pull-left margin_top_20" alt="logo" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/micrositeimages/wagner_logo.png"/></h4>
<div class="clear"></div>
</div>
<div class="pull-right col-lg-4 col-sm-5 col-md-4 line_height_24 margin_top"><strong>Address :</strong> Wagner Electronic Services,
               138 Liverpool Road, 
               Ashfield, NSW, 2131
              Australia</br>
              <hr class="hr_margin"/>
              <strong>Telephone :</strong> +49 30 47373795</br>
              <hr class="hr_margin">
              <strong>Fax :</strong> (+61) 02 9798 0017</br>
              <hr class="hr_margin"/>
              <strong>E-mail :</strong> sales@wagneronline.com.au
              <hr class="hr_margin"/>
              </div>
 <div class="clear"></div>
    </div>
<%--<table id="tblBase" width="590px" border="0" cellpadding="3" cellspacing="0" style="border-collapse: collapse"   align="center">
 <tr>
 <td width="100%">
<table id="tblConfirm" width="100%" border="0px" cellpadding="3" cellspacing="0"  style="border-collapse: collapse">
<tr>

  <table width="28%" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse; border-width: 1px; border-style: solid;height:45px;" align="left"> 
<tr> 
<td width="35%" align="left" style="border-style: none; padding-left: 2px;">
 <img src="/images/micrositeimages/Print.png" id="imgPrint" runat="server" width="30" />
  &nbsp;&nbsp; 
  </td>
   <td width="65%" align="left">
    <asp:LinkButton ID="lbtnPrint" runat="Server" Text="Print This Page" Font-Names="Arial" Font-Size="12px" Font-Bold="true" ForeColor="#00aeef" OnClientClick="javascript:window.print();return false;">
    </asp:LinkButton> 
    </td>
     </tr>
</table>
</tr>
<tr>
<td width="50%"> 
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse;"> 
<tr> 
<td width="35%" align="center" valign="top" > 
<img src="images/WagnerOR_Logo.png" id="imgLogo" runat="server" alt="" height="84" width="360" style="margin-top:26px;"/> 
</td> 
<td width="10%"> &nbsp; </td> 
<td width="55%">
 <p align="left" class="TitleColumnStyle" style="font-family:Arial;font-size:11px;"> 
 Wagner Electronic Services <br /> 
 138 Liverpool Road, <br /> 
 Ashfield, NSW, 2131 <br />
 Australia <br />
  Phone: (+61) 02 9798 9233 <br />
   Fax: (+61) 02 9798 0017 <br />
    Email: sales@wagner.net.au </p> 
 </td> 
 </tr>
 </table> 
 </td>

 </tr>
 </table>
  </td>
  </tr>
   <tr>
         <td width="100%"> 
         <table width="100%" border="0" cellpadding="3" cellspacing="0" style="border-collapse: collapse">
          <tr>
           <td>
            <WebCat:Invoice ID="ucInvoice" runat="server" /> 
            </td>
             </tr>
              </table>
               </td> 
               </tr>
   </table>--%>
   <div class="col-lg-10">
    <WebCat:Invoice ID="Invoice1" runat="server" /> 
   </div>

   
    </div>
    </div>
    </form>
</body>
</html>
