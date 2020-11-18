<%@ Page Title="" Language="C#" MasterPageFile="~/HomePage.master" AutoEventWireup="true" Inherits="homepageST" EnableEventValidation="false" Codebehind="/Home.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style>
 .modalBackground
{
    background-color: #000000;
    filter: alpha(opacity=70);
    opacity: 0.7;
}

</style>
   <script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/MicroSitejs/jquery-1.10.2.min.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="Server"></asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="footer" runat="Server">   
     <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="false" EnableCdn="true" EnablePageMethods="true" ScriptMode="Release"  EnablePartialRendering="false" >
        </asp:ScriptManager>
    <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none;  visibility: hidden"></asp:Button>
    <div id="PopupOrderMsg" align="center" runat ="server"> 
    <asp:Panel ID="ModalPanel" runat="server" CssClass="PopUpDisplayStyle">
    <div class="modal-dialog">
                        <div class="modal-content ">
                          <div class="close-selected">
                             
      <asp:ImageButton ID="btnclose" runat="server" OnClick="btnContinueOrder_Click" />
                        
                           
                      </div>
                          <div class="modal-header green_bg">
                            <h4 id="myModalLabel" class="text-center">
                                <%
                                  string img = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString();
                                   %>
                            <img class="popsucess" alt="hii" src="<%= img %>"/>Previous Order Info</h4>
                          </div>
                          <div class="modal-body">
                            
                              <div class="col-lg-20 text-center col-sm-10 col-md-5 mgntb20">
                                <p>There has been Product items found still in your shopping cart from your last login,
Would you like to continue with this order?</p>
                              </div>                             
                            
                          </div>
                          <div class="modal-footer clear border_top_none">
                            <%--<a class="btn primary-btn-green" href="#">Yes, Continue with Previous Orde</a>--%>
                             <asp:Button ID="ContinueOrder" runat="server" Text="Yes, Continue with Previous Order"  CssClass="btn primary-btn-green" OnClick="btnContinueOrder_Click" />
                            <%--<a class="btn primary-btn-blue" href="#">No, Cancel Previous Order</a>--%>
                            <asp:Button ID="ClearOrder" runat="server" Text="No, Cancel Previous Order" CssClass="btn primary-btn-blue" OnClick="btnClearOrder_Click" />
                          </div>
                      </div>
                  </div>
   
    </asp:Panel>
    </div>
     <div id="PopupRetailerLoginMsg" align="center" runat ="server">
     <asp:Panel ID="ModalPanel1" runat="server" CssClass="PopUpDisplayStyle" style="background-color :white" >
    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"  align="center">
     <tr style="height: 5px"> <td colspan="3"> &nbsp;</td> </tr>
      <tr style="height: 10px"> <td width="100%" align="center" colspan="3"> &nbsp; </td></tr>
      <tr style="height: 10px"> <td width="100%" align="center" colspan="3" class="TextContentStyle"> Your Account Has Not Been Activated!  <br />
      Please check your email for an email from us containing an activation / confirmation link.
      <br /> If you would like us to send you the Activation Email again. <a Href="/ConfirmMessage.aspx?Result=REMAILACTIVATION" class="toplinkatest">Please Click Here</a>
        </td> </tr> <tr style="height: 10px"><td width="100%" align="center" colspan="3"> &nbsp;</td> </tr>
         <tr style="height: 5px"><td colspan="3"> &nbsp; </td> </tr>
         <tr style="height: 10px">  <td width="100%" align="center" colspan="3">
         <asp:Button ID="btnCancel" runat="server" Text="Close"   Width="205px" CssClass="ButtonStyle" OnClick="btnCancel_Click" /> </td></tr></table></asp:Panel></div>
    
<%--    <script type="text/javascript"  language="javascript" >
        function MouseHover(ID) {
            switch (parseInt(ID)) {
                case 1:
                    document.getElementById("ctl00_footer_ContinueOrder").style.backgroundColor = "#009F00";
                    break;
                case 2:
                    document.getElementById("ctl00_footer_ClearOrder").style.backgroundColor = "red";
                    break;
                case 3:
                    document.getElementById("ctl00_footer_btnCancel").style.backgroundColor = "red";
                    break;
            }
        }

        function MouseOut(ID) {
            switch (parseInt(ID)) {
                case 1:
                    document.getElementById("ctl00_footer_ContinueOrder").style.backgroundColor = "#1589FF";
                    break;
                case 2:
                    document.getElementById("ctl00_footer_ClearOrder").style.backgroundColor = "#1589FF";
                    break;
                case 3:
                    document.getElementById("ctl00_footer_btnCancel").style.backgroundColor = "#1589FF";
                    break;
            }
        }
      
    </script>--%>

</asp:Content>
