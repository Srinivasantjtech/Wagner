<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_newproductlognav" Codebehind="newproductlognav.ascx.cs" %>
<%    if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == false && 
          HttpContext.Current.Request.Url.ToString().ToLower().Contains("forgotpassword.aspx") == false && 
          HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx") == false && 
          HttpContext.Current.Request.Url.ToString().ToLower().Contains("orderdetails.aspx") == false 
          && HttpContext.Current.Request.Url.ToString().ToLower().Contains("myaccount.aspx") == false
          && HttpContext.Current.Request.Url.ToString().ToLower().Contains("resetpassword.aspx") == false )
      {
          Response.Write(ST_Newproduct());  
      }
     %>