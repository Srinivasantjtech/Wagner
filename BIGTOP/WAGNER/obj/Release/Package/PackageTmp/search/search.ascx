<%@ Control Language="C#" AutoEventWireup="true" Inherits="search_search" Codebehind="search.ascx.cs" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<input id="HidItemPage" type="hidden" runat="server" />
<script language="javascript" type="text/javascript">
    function fnSetEvent() {
        document.getElementById("<%=hdnForClear.ClientID%>").value = "CLEAR";
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
    function trim(str, chars) {
        return ltrim(rtrim(str, chars), chars);
    }
    function ltrim(str, chars) {
        chars = chars || "\\s";
        return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
    }
    function rtrim(str, chars) {
        chars = chars || "\\s";
        return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
    }
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
    function ttrim(stringToTrim) {
        return stringToTrim.replace(" ", "");
    }
    function GetSelectedIts1() {
        var mySplitResult = "pppopt";
        var SelAttrStr = '';
        for (var j = 0; j < document.getElementById(mySplitResult).options.length; j++) {
            if (document.getElementById(mySplitResult).options[j].selected) {
                temp = document.getElementById(mySplitResult).options[j].value;
                document.getElementById("<%=HidItemPage.ClientID%>").value = temp;
            }
        }
        document.forms[0].submit();
    }
</script>

<input type="hidden" id="hdnForClear" runat="server" />
             <%
                Response.Write(Spell_Correction());
            %>

            
            
            <div class="powersearchbox clearfix">
                                   <%-- <form class="form-horizontal" method="get" action="#">--%>
                                        <fieldset class="search-box home">
                                         <asp:TextBox ID="txtSearch" runat="server"   type="text"
                                          onkeypress="return fnValidateSearchKeyword(event);" 
                                         placeholder="Enter Keyword!" >
        </asp:TextBox>
           <asp:Button ID="Button1" runat="server" class="srch-btn"  OnClientClick="return searchurl();" OnClick="btnsearch_Click" Width="55px" />
<%--           <button value="Search" class="srch-btn"   runat="server" type="submit" onclientclick="return searchurl();" onclick="btnsearch_Click"></button>--%>
                        <% if (txtSearch.Text != "")
           {  if (Session["totalitemcount"] !=null) { %> 
        <p> Search Results for <%=txtSearch.Text %>  has <%= Session["totalitemcount"] %>  items</p>
                                            <% }  else { %>
 <p> Search Results for <%=txtSearch.Text %> </p>
<% } %>
       <%} %>                    
                                          
                                        </fieldset>
                                   <%-- </form>--%>
                                </div>

<asp:HiddenField ID="txtsearchhidden" runat="server" />
        
        <%--  <div class="clearfix text-center"></div>--%>