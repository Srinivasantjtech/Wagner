<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Bottom" Codebehind="Bottom.ascx.cs" %>
<%--<%@ Import Namespace="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace="System.Diagnostics" %>--%>

<%--<%
    
    Stopwatch sw = new Stopwatch();
    sw.Start(); %>--%>

<% =ST_bottom() %>


<%--<%
    sw.Stop();
    Security objErrorHandler = new Security();
    objErrorHandler.ExeTimelog = "ST_bottom_load = " + sw.Elapsed.TotalSeconds.ToString();
    objErrorHandler.createexecutiontmielog(); 
     %>--%>
