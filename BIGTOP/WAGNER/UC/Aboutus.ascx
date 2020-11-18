<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Aboutus" Codebehind="Aboutus.ascx.cs" %>
<div class="container">
<div class="row hidden-xs hidden-sm">
    <div class="col-sm-20">
      <ul id="mainbredcrumb" class="breadcrumb">
        <li>
          <a href="/home.aspx">Home</a>
        </li>
        <li class="active">About Us</li>
      </ul>
    </div>
  </div>
  <div class="row">
  <div class="col-md-4 main-left hidden-sm hidden-xs">
   <%   
          Response.Write(ST_Newproduct());
     %>
  </div>
  <div class="col-md-16">
  <% Response.Write(ST_aboutus()); %>
  </div>
  </div>
</div>



