<%@ Page Language="C#" AutoEventWireup="true"  Inherits="OrderReport" Codebehind="OrderReport.aspx.cs" %>
<%@ Register TagPrefix="WebCat" TagName="Invoice" Src="~/UI/Invoice.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Order Report</title>
   <style type="text/css">
   .logoforORTOP1
{
           /* background: url(../Images/loginmerge.png);
       background-position: -14px -358px; 
     background-repeat: no-repeat;*/
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
<table id="tblBase" width="590px" border="0" cellpadding="3" cellspacing="0" style="border-collapse: collapse"   align="center"> <tr><td width="100%">
<table id="tblConfirm" width="100%" border="0px" cellpadding="3" cellspacing="0"  style="border-collapse: collapse">
<tr>

  <table width="28%" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse; border-width: 1px; border-style: solid;height:45px;" align="left"> 
<tr> 
<td width="35%" align="left" style="border-style: none; padding-left: 2px;">
 <img src="images/Printer-Image2.jpg" id="imgPrint" runat="server" width="30" />
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
<%--<td width="38%">
<table width="50%" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse; border-width: 1px; border-style: solid" align="center"> 

<tr> 

   <td width="65%" align="left">
    <asp:LinkButton ID="lbtnPrint" runat="Server" Text="Print This Page" Font-Names="Arial" Font-Size="12px" Font-Bold="true" ForeColor="#00aeef" OnClientClick="javascript:window.print();return false;">
    </asp:LinkButton> 
    </td>
     </tr>
</table>
 </td>--%>
<td width="50%"> 
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse;"> 
<tr> 
<td width="35%" align="center" valign="top" > 
<img src="images/Wagner-logo.png" id="imgLogo" runat="server" alt="" height="84" width="360" style="margin-top:26px;"/> 
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
    Email: sales@wagneronline.com.au </p> 
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
    </table>
    </form>
</body>
</html>
