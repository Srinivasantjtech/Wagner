<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Privacepolicy" Codebehind="Privacepolicy.ascx.cs" %>
 <div class="container">
<div class="row hidden-xs hidden-sm">
    	<div class="col-sm-20">
        	<ul id="mainbredcrumb" class="breadcrumb">
            	<li><a href="/home.aspx">Home</a></li>
                <li class="active">Privacy Policy</li>
            </ul>
        </div>
    </div>
    <div class="row">
<%--    <div class="col-md-4 main-left">
    <%   
          Response.Write(ST_Newproduct());
     %>
        
       	</div>--%>
        <div class="col-sm-20">
        <div class="privacypolicy">
       
               <% Response.Write(ST_privacepolicy()); %>    
               </div>
</div>
</div>
</div>
