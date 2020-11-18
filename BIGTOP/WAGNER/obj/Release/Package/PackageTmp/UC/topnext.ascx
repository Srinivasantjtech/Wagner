<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_topnext" Codebehind="topnext.ascx.cs" %>
<%@ Register Src="Quickbuy.ascx" TagName="QuickOrder" TagPrefix="uc2" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<div class="container-fluid zerospace">
	<div class="container">
    	<div class="main-banner animateOnScroll" data-animation-type="fadeInUp" data-timeout="0" data-offset-top="200">
            <%
                string image = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/banner_New.jpg";
                 %>
        	<img class="img-responsive" src="<%=image %>"/>
        </div>	
    </div>
</div>
<%--<script type="text/javascript">
    function GetDeal() {
        var m=document.getElementById('txtMail').value;
        window.location.href = 'GetDeal.aspx?&amp;mail=' + m;
    }
   
  </script>
<table cellpadding="0" cellspacing="0" border="0"><tr><td colspan="1" height="6"></td></tr>
<tr>
<td height="200">
<div class="anythingSlider">
<%
    HelperDB objHelperDB = new HelperDB(); 
    DataSet dsbannerlink = new DataSet();
    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
    dsbannerlink = (DataSet)objHelperDB.GetGenericDataDB("", "GET_BANNER_LINK", HelperDB.ReturnType.RTDataSet);
    string Banner1_Link = "";
    string Banner2_Link = "";
    string Banner3_Link = "";
    if (dsbannerlink != null && dsbannerlink.Tables[0].Rows.Count > 0)
    {
        Banner1_Link = "http://www.wagneronline.com.au/electronics-news/casio-lamp-free-projectors/";//dsbannerlink.Tables[0].Rows[0]["BANNER1_LINK_NEW"].ToString();
        Banner2_Link = dsbannerlink.Tables[0].Rows[0]["BANNER2_LINK_NEW"].ToString();
        Banner3_Link = dsbannerlink.Tables[0].Rows[0]["BANNER3_LINK_NEW"].ToString();
    }
    
     %>
<div class="wrapper">
<ul>
<li>
<a href="<%=Banner1_Link%>" target="_blank"><img src="images/Wagner-Banner-Casio.jpg" border="0" alt="Casio Projectors Australia" width="760px" height="200px" /> </a>
</li>
<li>
<a href="<%=Banner2_Link%>"><img src="images/Wagner_Banner_2.jpg" border="0" alt="Banner2" width="760px" height="200px" /></a>
</li>
<li>
<a href="<%=Banner3_Link%>"><img src="images/Wagner_Banner_3.jpg" border="0" alt="Banner3" width="760px" height="200px"/></a>
</li>
</ul>
 </div>
 </div>
</td>
<td valign="top" height="203" style="background:url(images/emaildeals1.jpg); background-repeat:no-repeat; background-position:3px 0px; padding-left:18px;" >
<table width="200" cellpadding="0" cellspacing="0" border="0" ><tr><td height="73" colspan="2"></td></tr>
<tr><td height="24" width="138"><input id="txtMail" name="" type="text" class="inputtxtlogps" /></td>
<td width="62px" >

<a href="#" onclick="GetDeal(); return false;" class="homesgnup"></a>
</td> </tr>
<tr><td height="76" colspan="2"></td></tr><tr><td colspan="2"><a href="/new-products/909/ct/" class="pink">See our latest New Products &amp; Specials</a></td></tr></table></td> </tr> </table>--%>