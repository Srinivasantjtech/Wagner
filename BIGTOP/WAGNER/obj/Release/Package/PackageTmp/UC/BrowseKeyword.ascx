<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BrowseKeyword.ascx.cs" Inherits="WES.UC.BrowseKeyword" %>
<script type="text/javascript">

    function searchurl() {
        var ddlattrvalue = document.getElementById('<%=txtSearch.ClientID%>').value;
        if (ddlattrvalue != "") {
            if (ttrim(ddlattrvalue) != "") {
                document.getElementById('<%=txtsearchhidden.ClientID%>').value = ddlattrvalue;
            }
        }
        else {
            return false;
        }
    }
    function fnValidateSearchKeyword(e) {
        if (e.keyCode == 13) {

            var ddlattrvalue = document.getElementById('<%=txtSearch.ClientID%>').value;

            if (ddlattrvalue != "") {
                if (ddlattrvalue != "Search wagner! Enter Keywords or Part No's") {


                    // window.document.location="ps.aspx?&srctext=" + ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26").replace(/ /g,"%20").replace("+","%2B").replace(/\"/g, "%22");

                    //  window.location.href = "ps.aspx?" + ddlattrvalue.replace(/#/, "%23").replace(/ /g, "_").replace("+", "%2B").replace(/\"/g, "%22").replace("%20", "_").replace("  ", "_").replace("__", "_"); 
                    $.ajax({
                        type: "POST",
                        url: "/GblWebMethods.aspx/stringreplace",
                        data: '{"strvalue":"' + ddlattrvalue + '"}',
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d != "") {
                                window.location.href = "/" + data.d + "/ps/";
                            }
                        },
                        error: function (xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            alert(err.Message);
                        }



                    });


                    return false;
                }
            }
        }
    }
</script>

<div class="container">
	<div class="row">
    	<div class="col-sm-20">
        	<ul class="breadcrumb" id="Ul1">
            	<li><a href="home.aspx">Home</a></li>
                <li class="active">Browse Keyword Index</li>
            </ul>
        </div>
    </div>
    <div class="row">
 
          <div class="col-md-20">
        	  <div class="row">
                    <div class="ctgry-headpanel clearfix">
                        <div class="ctgry-headpanel clearfix">
                            <div class="categoryheading nolftpadd">
                                <h4>Browse Keyword Index</h4>
                            </div> 
                            <div class="border_btm"></div>     
                        </div>
                    </div>
               </div>
               <div class="row">
                    	<div class="subct-selection">
                        	<div class="subcategory-selection noborder clearfix">
								<div class="mainsearchbox mgntb0 clearfix">
                                	<div class="categoryheading nolftpadd">
                                        <h3 class="text-center">Popular Searches</h3>
                                    </div>
                                    <fieldset class="search-box">
                                     <asp:HiddenField ID="txtsearchhidden" runat="server" />
                                         <asp:TextBox ID="txtSearch" runat="server"   type="text"
                                          onkeypress="return fnValidateSearchKeyword(event);" 
                                         placeholder="Enter Keyword!" >
        </asp:TextBox>
           <asp:Button ID="Button1" runat="server" class="srch-btn"  OnClientClick="return searchurl();" OnClick="btnsearch_Click" Width="55px" />
           </fieldset>
                                       <%-- <fieldset class="search-box">
                                             <asp:HiddenField ID="txtsearchhidden" runat="server" />
          <asp:Label ID="lblSearchError" runat="server" ForeColor="#FF0000"></asp:Label></div>
                                          <asp:TextBox ID="txtSearch" runat="server"  CssClass="topsearch1" placeholder="Search WAGNER! Enter Keywords or Part No's" ></asp:TextBox>
        <asp:Button ID="Button1" runat="server" class="srch-btn2" style="margin-right: -282px;margin-top: 62px;" 
 OnClientClick="return searchurl();" OnClick="btnsearch_Click" />
                                           
                                        </fieldset>--%>
                                   

                            
                                </div>
                            </div>
                        </div>
                </div>
                    
                <div class="row">                    
             		
                    <div class="col-sm-20 col-md-20 nolftpadd clearfix">
                		<div class="srchbykey clearfix">
                        	<div class="alphabhet_key clearfix">
                            <a class="sli_alpha_nav" id ="tag01" runat="server" 
href="/0-9/bk/" >
0-9</a>
<b class="sli_alpha_nav_1" id ="btag01" runat="server" visible ="false" >0-9 </b>
&nbsp;
<a class="sli_alpha_nav" id ="tagA" runat="server"
href="/a/bk/" >
A</a>
<b class="sli_alpha_nav_1" id ="btagA" runat="server" visible ="false" >A </b>

&nbsp;
<a class="sli_alpha_nav" id ="tagB" runat="server"
href="/b/bk/">
B</a>
<b class="sli_alpha_nav_1" id ="btagB" runat="server" visible ="false" >B</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagC" runat="server"
href="/c/bk/">
C</a>
<b class="sli_alpha_nav_1" id ="btagC" runat="server" visible ="false" >C</b>&nbsp;
<a class="sli_alpha_nav" id ="tagD" runat="server"
href="/d/bk/">
D</a>
<b class="sli_alpha_nav_1" id ="btagd" runat="server" visible ="false" >D</b>

&nbsp;
<a class="sli_alpha_nav" id ="tagE" runat="server"
href="/e/bk/">
E</a>
<b class="sli_alpha_nav_1" id ="btagE" runat="server" visible ="false" >E</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagF" runat="server"
href="/f/bk/">
F</a>
<b class="sli_alpha_nav_1" id ="btagF" runat="server" visible ="false" >F</b>
&nbsp;

<a class="sli_alpha_nav" id ="tagG" runat="server"
href="/g/bk/">
G</a>
<b class="sli_alpha_nav_1" id ="btagG" runat="server" visible ="false" >G</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagH" runat="server"
href="/h/bk/">
H</a>
<b class="sli_alpha_nav_1" id ="btagH" runat="server" visible ="false" >H</b>
&nbsp;


<a class="sli_alpha_nav" id ="tagI" runat="server"
href="/i/bk/">
I</a>
<b class="sli_alpha_nav_1" id ="btagI" runat="server" visible ="false" >I</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagJ" runat="server"
href="/j/bk/">
J</a>
<b class="sli_alpha_nav_1" id ="btagJ" runat="server" visible ="false" >J</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagK" runat="server"
href="/k/bk/">
K</a>
<b class="sli_alpha_nav_1" id ="btagK" runat="server" visible ="false" >K</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagL" runat="server"
href="/l/bk/">
L</a>
<b class="sli_alpha_nav_1" id ="btagL" runat="server" visible ="false" >L</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagM" runat="server"
href="/m/bk/">
M</a>
<b class="sli_alpha_nav_1" id ="btagM" runat="server" visible ="false" >M</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagN" runat="server"
href="/n/bk/">
N</a>
<b class="sli_alpha_nav_1" id ="btagN" runat="server" visible ="false" >N</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagO" runat="server"
href="/o/bk/">
O</a>
<b class="sli_alpha_nav_1" id ="btagO" runat="server" visible ="false" >O</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagP" runat="server"
href="/p/bk/">
P</a>
<b class="sli_alpha_nav_1" id ="btagP" runat="server" visible ="false" >P</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagQ" runat="server"
href="/q/bk/">
Q</a>
<b class="sli_alpha_nav_1" id ="btagQ" runat="server" visible ="false" >Q</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagR" runat="server"
href="/r/bk/">
R</a>
<b class="sli_alpha_nav_1" id ="btagR" runat="server" visible ="false" >R</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagS" runat="server"
href="/s/bk/">
S</a>
<b class="sli_alpha_nav_1" id ="btagS" runat="server" visible ="false" >S</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagT" runat="server"
href="/t/bk/">
T</a>
<b class="sli_alpha_nav_1" id ="btagT" runat="server" visible ="false" >T</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagU" runat="server"
href="/u/bk/">
U</a>
<b class="sli_alpha_nav_1" id ="btagU" runat="server" visible ="false" >U</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagV" runat="server"
href="/v/bk/">
V</a>&nbsp;
<b class="sli_alpha_nav_1" id ="btagV" runat="server" visible ="false" >V</b>
<a class="sli_alpha_nav" id ="tagW" runat="server"
href="/w/bk/">
W</a>
<b class="sli_alpha_nav_1" id ="btagW" runat="server" visible ="false" >W</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagX" runat="server"
href="/x/bk/">
X</a>
<b class="sli_alpha_nav_1" id ="btagx" runat="server" visible ="false" >X</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagY" runat="server"
href="/y/bk/">
Y</a>
<b class="sli_alpha_nav_1" id ="btagy" runat="server" visible ="false" >Y</b>
&nbsp;
<a class="sli_alpha_nav" id ="tagZ" runat="server"
href="/z/bk/">
Z</a>
<b class="sli_alpha_nav_1" id ="btagz" runat="server" visible ="false" >Z</b>
                            </div>
                                 
                         
                            	   <% Response.Write(St_BrowseKeyword()); %>
                          
                        </div>
                     </div>
                    
                </div>
              
             </div>
                
            </div>
        </div>


















             