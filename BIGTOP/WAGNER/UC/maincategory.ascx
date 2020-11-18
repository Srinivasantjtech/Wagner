<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_maincategory" EnableTheming="True" Codebehind="maincategory.ascx.cs" %>
 <%-- <input type="hidden" name="hidcatIds" runat="server" id="hidcatIds" />--%>
   <%--  <input type="hidden" name="HidsubcatIds" runat="server" id="HidsubcatIds" />--%>
    <%-- <input type="hidden" name="HidsubcatIds1" runat="server" id="HidsubcatIds1" />--%>
  <%--   <input id="HidItemPage" type="hidden" runat="server" />--%>
<%--<input id="Hidcat" type="hidden" runat="server" />--%>

 <script language="javascript" type="text/javascript">
     function getValue_breadcrumb(a) {

         var relval = a.rev;
       //  alert(relval);
          
         var url = a.ping.replace('//', '/');

         $.ajax({
             type: "POST",
             url: "/GblWebMethods.aspx/setsession_breadcrumb",
             data: "{'strpath':'" + relval + "'}",
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (data) {
                  
                      
                 window.location = url;
                   
             },
             error: function (xhr, status, error) {
                 var err = eval("(" + xhr.responseText + ")");
                 // alert(err);
             }
         })
     }

     function getValue_pd(a) {

         var relval = a.rev;


         var url = a.href;

         $.ajax({
             type: "POST",
             url: "/GblWebMethods.aspx/setsession_product",
             data: "{'strpath':'" + relval + "'}",
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (data) {
                 if (result.d == "1") {
                 
                     window.location = url;
                 }
             },
             error: function (xhr, status, error) {
                 var err = eval("(" + xhr.responseText + ")");
                 // alert(err);
             }
         })
     }


     function getValue_fl(a) {

         var relval = a.rev;


         var url = a.href;

         $.ajax({
             type: "POST",
             url: "/GblWebMethods.aspx/setsession_family",
             data: "{'strpath':'" + relval + "'}",
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (data) {
                 if (result.d == "1") {
                   
                     window.location = url;
                 }
             },
             error: function (xhr, status, error) {
                 var err = eval("(" + xhr.responseText + ")");
                 // alert(err);
             }
         })
     }
     function getValue_brand(a) {

         var relval = a.rel;


         var url = a.href;
        
         $.ajax({  
             type: "POST",
             url: "/GblWebMethods.aspx/setsession_brand",
             data: "{'urlpath':'" + url + "','strpath':'" + relval + "'}",
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (data) {
                 if (result.d == "1") {
                  
                     window.location = url;
                 }
             },
             error: function (xhr, status, error) {
                 var err = eval("(" + xhr.responseText + ")");
                 // alert(err);
             }
         })
     }
     </script>
  

  <% if ((Request.Url.ToString().ToLower().Contains("mct.aspx") == true) || (Request.Url.ToString().ToLower().Contains("mpl.aspx") == true)
                    || (Request.Url.ToString().ToLower().Contains("mps.aspx") == true)
                    || (Request.Url.ToString().ToLower().Contains("mfl.aspx") == true)
                    || (Request.Url.ToString().ToLower().Contains("mpd.aspx") == true)
           || (Request.Url.ToString().ToLower().Contains("mcontactus.aspx") == true)
          || (Request.Url.ToString().ToLower().Contains("maboutus.aspx") == true)
           || (Request.Url.ToString().ToLower().Contains("mlogin.aspx") == true)
         || (Request.Url.ToString().ToLower().Contains("mmyaccount.aspx") == true)
          || (Request.Url.ToString().ToLower().Contains("mchangepassword.aspx") == true)
          || (Request.Url.ToString().ToLower().Contains("mchangeusername.aspx") == true)
          || (Request.Url.ToString().ToLower().Contains("morderhistory.aspx") == true)
         || (Request.Url.ToString().ToLower().Contains("mconfirmmessage.aspx") == true)
              || (Request.Url.ToString().ToLower().Contains("mforgotpassWord.aspx") == true)
              || (Request.Url.ToString().ToLower().Contains("muserprofile.aspx") == true)
              || (Request.Url.ToString().ToLower().Contains("morderdetails.aspx") == true)
         || (Request.Url.ToString().ToLower().Contains("mcheckout.aspx") == true)
         )
                    
         {
             if ((Request.Url.ToString().ToLower().Contains("mfl.aspx") == true)
                     || (Request.Url.ToString().ToLower().Contains("mpd.aspx") == true)
                 || Request.Url.ToString().ToLower().Contains("mmyaccount.aspx") == true
                   || (Request.Url.ToString().ToLower().Contains("mcontactus.aspx") == true)
          || (Request.Url.ToString().ToLower().Contains("maboutus.aspx") == true)
                 )
             {
                    
         %>
       
              <% Response.Write(ST_CategoriesMS());  %>
           
             <%}
             else
             { %>
                  
                 <div class="col-lg-3 col-md-3 col-sm-4 col-xs-12 padding_left_zero_fil">
              <% Response.Write(ST_CategoriesMS());  %>
             </div>
           
              <%}%>

              




              <%
                
             } 
            else
            { %>
<%-- <script language="javascript" type="text/javascript">   
      function GetSelectedItems(field) {

         var SelAttrStr = '';
         var hfcid = document.getElementById("<%=hfcid.ClientID%>").value;
         var hfcatname = document.getElementById("<%=hfcname.ClientID%>").value;
         for (var j = 0; j < document.getElementById(field).options.length; j++) {
             if (document.getElementById(field).options[j].selected) {
                 if (document.getElementById(field).options[j].value != 'Select Brand' && document.getElementById(field).options[j].value != 'List all models' && document.getElementById(field).options[j].value != 'List all products') {
                     SelAttrStr = document.getElementById(field).options[j].value + '^' + field;
                     if (field == 1) {

                         document.getElementById("<%=hidcatIds.ClientID%>").value = SelAttrStr;
                         document.getElementById("<%=hforgurl.ClientID%>").value = hfcatname;
                         window.location.href = document.getElementById(field).options[j].value;


                     }
                     else if (field == 2) {

                         document.getElementById("<%=HidsubcatIds.ClientID%>").value = SelAttrStr;
                         window.location.href = document.getElementById(field).options[j].value;
                     
                     }
                   
                 }
             }

         }

     }
    </script>--%>
<%--<table width="180" border="0" cellspacing="0" cellpadding="0" ><tr><td>--%>
<%--<input type="text" name="pcid" id="hfcid" runat="server" style="display:none;"/>--%>
<%--<input type="text" name="pcname" id="hfcname" runat="server" style="display:none;"/>
<input type="text" name="hforgurl" id="hforgurl" runat="server" style="display:none;"/>--%>
<%--<input type="text" name="hfisselected" id="hfisselected"  runat="server" style="display:none;"/>--%>

   
      <%  if(HttpContext.Current.Request.Url.ToString().ToLower().Contains("mlogin.aspx")==false &&
              HttpContext.Current.Request.Url.ToString().ToLower().Contains("mconfirmmessage.aspx") == false &&
             HttpContext.Current.Request.Url.ToString().ToLower().Contains("mforgotpassword.aspx") == false &&
               HttpContext.Current.Request.Url.ToString().ToLower().Contains("mcontactus.aspx") == false &&
                    HttpContext.Current.Request.Url.ToString().ToLower().Contains("maboutus.aspx") == false &&
                         HttpContext.Current.Request.Url.ToString().ToLower().Contains("mmyaccount.aspx") == false &&
                HttpContext.Current.Request.Url.ToString().ToLower().Contains("mgetdeal.aspx") == false &&
                 HttpContext.Current.Request.Url.ToString().ToLower().Contains("mfaq.aspx") == false &&
                HttpContext.Current.Request.Url.ToString().ToLower().Contains("mresetpwd.aspx") == false
               )      
         {
             YHSCell_Bind(false); 
           }  %>
         
       
   <%-- </td></tr></table><br />  --%>     
                   <%}%>
       


<%--  <% if ((Request.Url.ToString().ToLower().Contains("mpl.aspx") == true) || (Request.Url.ToString().ToLower().Contains("mps.aspx") == true)) { %>
   <% Response.Write(ST_CategoriesMS_mobile());  %>
    <%}%>--%>