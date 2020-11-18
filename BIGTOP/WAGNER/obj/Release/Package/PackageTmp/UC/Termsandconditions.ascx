<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Termsandconditions" Codebehind="Termsandconditions.ascx.cs" %>


 <div class="container">
<div class="row hidden-xs hidden-sm">
    	<div class="col-sm-20">
        	<ul id="mainbredcrumb" class="breadcrumb">
            	<li><a href="/home.aspx">Home</a></li>
                <li class="active">Terms and Conditions</li>  
            </ul>
        </div>
    </div>
    <div class="row">
  <%--  <div class="col-md-4 main-left">
    <%   
          Response.Write(ST_Newproduct());
     %>
        
       	</div>--%>
        <div class="col-sm-20 margin_lft15">
        <div class="privacypolicy">
       
               <% Response.Write(ST_termsandconditions()); %>    
               </div>
</div>
</div>
</div>

