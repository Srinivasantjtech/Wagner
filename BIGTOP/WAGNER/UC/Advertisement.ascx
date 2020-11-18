<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Advertisement" Codebehind="Advertisement.ascx.cs" %>
<%
    if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx") == false && HttpContext.Current.Request.Url.ToString().ToLower().Contains("forgotpassword.aspx") == false && HttpContext.Current.Request.Url.ToString().ToLower().Contains("resetpassword.aspx") == false)
    {
        Response.Write(ST_Advertisement());
    }
%>