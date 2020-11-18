<%@ Page Language="C#" MasterPageFile="~/Mainpage_Express.Master" AutoEventWireup="true" CodeBehind="Express_Checkout.aspx.cs" Inherits="WES.Express_Checkout" EnableEventValidation="false" Culture="en-US" UICulture="en-US" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="TradingBell.WebCat.CommonServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UC/OrderDetExp.ascx" TagName="OrderDet" TagPrefix="uc1" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js" ></script>
    <input type="hidden" value="true" id="chk" />
    <script type="text/javascript">
        function DRPshippment() {
            alert('Non-Standard Delivery Area. We will contact you to confirm costing');
        }
    </script>

    <script type="text/javascript">
        //<![CDATA[
        javascript: HidePanels(); dataLayer.push({ 'event': 'Checkout', 'transactionId': '121623', 'transactionAffiliation': '', 'transactionTotal': '1.50', 'transactionTax': '0.14', 'transactionShipping': '0.00', 'transactionProducts': [{ 'sku': '81776', 'name': 'ATC FUSE HOLDER KIT', 'category': 'New CBL Products', 'price': '1.3600', 'quantity': '1' }] }); $(document).ready(function () { $('html,body').animate({ scrollTop: $('#ctl00_maincontent_divl3continue').offset().top - 100 }, 0); });
        var Page_ValidationActive = false;
        if (typeof (ValidatorOnLoad) == "function") {
            ValidatorOnLoad();
        }

        function ValidatorOnSubmit() {
            if (Page_ValidationActive) {
                return ValidatorCommonOnSubmit();
            } else {
                return true;
            }
        }
        //]]>
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="success" runat="server">
    <div id="payment_whiteScreen" runat="server" class="whitescreen"></div>
    <div id="payment_popup" runat="server" class="success">
        <div class="text-center">
            <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/11.gif" style="height: 75px; margin-left: auto; margin-right: auto; display: table-cell; z-index: 1010;" />
        </div>
        <div class="successMsg">
            <%-- <h3 style="font-size:19px;font-family: Arial,Helvetica,sans-serif;"> Success !</h3>--%>
            <p style="font-size: 18px;">Thanks for your order</p>
            <p style="font-size: 16px;">You will be redirected to paypal website to complete payment transaction..</p>
        </div>
        <%--  <a class="continueShopping">Continue Shopping</a>--%>
    </div>

    <div id="payment_success_popup" runat="server" class="success">
        <div class="text-center">
            <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/11.gif" style="height: 75px; margin-left: auto; margin-right: auto; display: table-cell; z-index: 1010;" />
        </div>
        <div class="successMsg">
            <%-- <h3 style="font-size:19px;font-family: Arial,Helvetica,sans-serif;"> Success !</h3>--%>
            <p style="font-size: 16px;">Processing your order. One moment please..</p>
        </div>
        <%--  <a class="continueShopping">Continue Shopping</a>--%>
    </div>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="headerTop" runat="Server">


    <div class="topmenu-wrapper margin_btm_30">

        <div class="container">
            <div class="row">
                <div class="col-sm-4">
                    <div class="welcome">
                        <h5 runat="server" id="username"></h5>
                        <%--<%
                                    if (HttpContext.Current.Session["EXPRESS_CHECKOUT"] == null)
                                        HttpContext.Current.Session["EXPRESS_CHECKOUT"] = "False";
                                    
                                    if (HttpContext.Current.Session["USER_ID"] != "" && Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) > 0 && HttpContext.Current.Session["USER_ID"].ToString() != "999" )
                                   {
                                       string username = "";
                                       if (HttpContext.Current.Session["LOGIN_NAME_TOP"] != null && HttpContext.Current.Session["LOGIN_NAME_TOP"].ToString() != null)
                                           username = HttpContext.Current.Session["LOGIN_NAME_TOP"].ToString();
                                       else
                                           username ="";   
                                %>
                                <h5>Welcome <%= username%></h5>
                                <%
          }
                                %>--%>
                    </div>
                </div>
                <div class="col-sm-16">
                    <div class="top-menu-box">
                        <ul class="top-menu">
                            <li>
                                <a href="/onlinecatalogue.aspx">Online Catalogue</a>
                            </li>
                            <li>
                                <a href="/myaccount.aspx">My Account</a>
                            </li>
                            <li>
                                <a href="/contactus.aspx">Contact Us</a>
                            </li>
                            <li>
                                <% 
                                    if (HttpContext.Current.Session["EXPRESS_CHECKOUT"] == null)
                                        HttpContext.Current.Session["EXPRESS_CHECKOUT"] = "False";
                                %>
                                <a id="logoutLink" runat="server" href="/logout.aspx" style="display: none">Logout</a>
                                <a id="loginLink" runat="server" href="/login.aspx">Login / Register</a>

                                <%--    <% 
                                            if (HttpContext.Current.Session["EXPRESS_CHECKOUT"] == null)
                                                HttpContext.Current.Session["EXPRESS_CHECKOUT"] = "False";
                                                
                                            if (HttpContext.Current.Session["USER_ID"] != "" && Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) > 0 && HttpContext.Current.Session["USER_ID"].ToString() != "999")
                                           {
     
                                        %>
                                        <a href="/logout.aspx">Logout</a>
                                        <%
          }
         
                                        %>
                                        <%
                                           else
                                           {
                                        %>
                                        <a href="/login.aspx">Login / Register</a>
                                        <%
          } %>--%>
                            </li>

                        </ul>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>



<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="false" EnableCdn="true" EnablePageMethods="true" ScriptMode="Release" EnablePartialRendering="false"></asp:ScriptManager>


    <asp:HiddenField ID="hidSourceID" runat="server" />
    <asp:HiddenField ID="hfphonenumber" runat="server" />
    <asp:HiddenField ID="hfnothanks" runat="server" Value="0" />
    <asp:HiddenField ID="hfprodtotalprice" runat="server" />
    <asp:HiddenField ID="hfhidepaypaldiv" runat="server" Value="0" />
    <asp:HiddenField ID="hfisppp" Value="0"  runat="server"/>

    <style>
        .vldRequiredSkin {
            color: red;
            display: inline;
        }
    </style>

    <style>
        .floating-form {
            width: 100%;
            margin-top: 20px;
        }
        .profileedit_wrap {
            width:320px;
            display:block;
        }
        
	@media(max-width:767px){
		.floating-form { width:98%; }
	}

        /****  floating-Lable style start ****/
        .floating-label {
            position: relative;
            margin-bottom: 20px;
        }

        .floating-input, .floating-select {
            font-size: 14px;
            padding: 4px 4px;
            display: block;
            width: 100%;
            height: 35px;
            background-color: transparent;
            border: none;
            border: 1px solid #0072bb;
        }

            .floating-input:focus, .floating-select:focus {
                outline: none;
                border: 1px solid #0072bb;
            }
            .floating-input.error, .floating-select.error {
                border: 1px solid red;
            }

        label.fl {
            color: #999;
            font-size: 14px;
            font-weight: normal;
            position: absolute;
            pointer-events: none;
            margin-left: 10px;
            left: 5px;
            top: 8px;
            padding: 0 3px;
            background:#fff;
            transition: 0.2s ease all;
            -moz-transition: 0.2s ease all;
            -webkit-transition: 0.2s ease all;
        }

        .floating-input:focus ~ label.fl, .floating-input:not(:placeholder-shown) ~ label.fl {
            top: -10px;
            font-size: 13px;
            color: #337ab7;
            font-weight: bold;
        }
        
         .floating-input:focus ~ label.error, .floating-input:not(:placeholder-shown) ~ label.error {
            color:red ;
        }

        .floating-select:focus ~ label.fl, .floating-select:not([value=""]):valid ~ label.fl {
            top: -10px;
            font-size: 14px;
            color: #0072bb;
        }

        /* active state */
        .floating-input:focus ~ .bar:before, .floating-input:focus ~ .bar:after, .floating-select:focus ~ .bar:before, .floating-select:focus ~ .bar:after {
            width: 50%;
        }

        *, *:before, *:after {
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
        }

        .floating-textarea {
            min-height: 30px;
            max-height: 260px;
            overflow: hidden;
            overflow-x: hidden;
        }

        /* highlighter */
        .highlight {
            position: absolute;
            height: 50%;
            width: 100%;
            top: 15%;
            left: 0;
            pointer-events: none;
            opacity: 0.5;
        }

        .rgt
        {
            width: 49%; float: right;
            }
        /* active state */
        .floating-input:focus ~ .highlight, .floating-select:focus ~ .highlight {
            -webkit-animation: inputHighlighter 0.3s ease;
            -moz-animation: inputHighlighter 0.3s ease;
            animation: inputHighlighter 0.3s ease;
        }

        /* animation */
        @-webkit-keyframes inputHighlighter {
            from {
                background: #0072bb;
            }

            to {
                width: 0;
                background: transparent;
            }
        }

        @-moz-keyframes inputHighlighter {
            from {
                background: #0072bb;
            }

            to {
                width: 0;
                background: transparent;
            }
        }

        @keyframes inputHighlighter {
            from {
                background: #0072bb;
            }

            to {
                width: 0;
                background: transparent;
            }
        }

        /****  floating-Lable style end ****/
    </style>

    <style>

 #address_book ::-webkit-scrollbar {
  width: 10px;
}

/* Track */
 #address_book ::-webkit-scrollbar-track {
  background: #f1f1f1; 
  border-radius: 10px;
}

/* Handle */
 #address_book ::-webkit-scrollbar-thumb {
  background: #b5b5b5;
  border-radius: 10px; 
}

 #address_book ::-webkit-scrollbar-thumb:hover {
  background: #555;
  border-radius: 10px; 
}


    #address_book .modal1 {
        position: fixed;
        z-index: 10000; /* 1 */
        top: 0;
        left: 0;
        visibility: hidden;
        width: 100%;
        height: 100%;
	    font-family:Arial, Helvetica, sans-serif;
    }

    #address_book .modal1.is-visible {
        visibility: visible;
    }

    #address_book .modal-overlay {
      position: fixed;
      z-index: 10;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: hsla(0, 0%, 0%, 0.5);
      visibility: hidden;
      opacity: 0;
      transition: visibility 0s linear 0.3s, opacity 0.3s;
    }

    #address_book .modal1.is-visible .modal-overlay {
      opacity: 1;
      visibility: visible;
      transition-delay: 0s;
    }

    #address_book .modal-wrapper {
      position: fixed;
      z-index: 9999;
      top: 6em;
      left: 50%;
      width: 32em;
      margin-left: -16em;
      background-color: #fff;
      box-shadow: 0 0 1.5em hsla(0, 0%, 0%, 0.35);
      border-top-radius:6px;
      border-radius:6px;
      margin-top:20px;
    }

    .modal-transition {
      transition: all 0.3s 0.12s;
      transform: translateY(-10%);
      opacity: 0;
      display:none;
    }

    .modal1.is-visible .modal-transition {
      transform: translateY(0);
      opacity: 1;
      display: block;
    }

    #address_book .modal-header,
    #address_book .modal-content {
      padding: 1em;
    }

    #address_book .modal-content {
      max-height:400px;
      overflow-y:auto;
    }

    #address_book .modal-header {
      position: relative;
      background-color: #0167b8;
      color:#fff;
      box-shadow: 0 1px 2px hsla(0, 0%, 0%, 0.06);
      border-bottom: 1px solid #e8e8e8;
      border-top-left-radius:6px;
      border-top-right-radius:6px;
    }

    #address_book .modal-close {
      position: absolute;
      top: 0;
      right: 0;
      padding: 1em;
      color: #aaa;
      background: none;
      border: 0;
    }

    #address_book .modal-close:hover {
      color: #777;
    }

    #address_book .modal-heading {
      font-size: 1.125em;
      margin: 0;
      -webkit-font-smoothing: antialiased;
      -moz-osx-font-smoothing: grayscale;
    }

    #address_book .modal-content > *:first-child {
      margin-top: 0;
    }

    #address_book .modal-content > *:last-child {
      margin-bottom: 0;
    }


    .form-col-4-8 {
        width: 50%;
    }
    .form-col-1-8, .form-col-2-8, .form-col-3-8, .form-col-4-8, .form-col-5-8, .form-col-6-8, .form-col-7-8, .form-col-8-8 {
        float: left;
        padding: 2px 1% !important;
        box-sizing: border-box;
        -moz-box-sizing: border-box;
        -ms-box-sizing: border-box;
        -webkit-box-sizing: border-box;
        -khtml-box-sizing: border-box;
    }
    #address_book .textarea1 {
        width: 99%;
    }
    #address_book .dblock {
	    display:block;
    }
    #address_book .address_label { 
	    display:block;
	    padding-bottom:6px;
	    margin-bottom:10px;
	    border-bottom:1px solid #ddd;
	
    }
    #address_book .address_label span { 
	    display:block;
	
    }
    #address_book .modal-content .dblock {
	    margin-bottom:10px;
    }
    #address_book .modal-content textarea {
	    display:inline-block;
	    width:84%;
        resize:none;
        margin: 0 10px;
    }
    #address_book .modal-content .address_radio {
	    display:inline-block;
	    width:10%;
	    vertical-align:top;
	    height:120px;
    }
    #address_book .modal-content  fieldset {
	    border:none;
    }
    #address_book .address_update {
	    float:right;
	    margin-right: 20px;
    }
    .clear {
	    clear:both;
    }
    </style>

 <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&key=AIzaSyD6KpDELswcok62eMQN_L2qBpBud3mqpGM&signed_in=true&libraries=places"></script>


        <script type="text/javascript">

            google.maps.event.addDomListener(window, 'load', initAutocomplete);

            var base64map = "eyLDgSI6IkEiLCLEgiI6IkEiLCLhuq4iOiJBIiwi4bq2IjoiQSIsIuG6sCI6IkEiLCLhurIiOiJBIiwi4bq0IjoiQSIsIseNIjoiQSIsIsOCIjoiQSIsIuG6pCI6IkEiLCLhuqwiOiJBIiwi4bqmIjoiQSIsIuG6qCI6IkEiLCLhuqoiOiJBIiwiw4QiOiJBIiwix54iOiJBIiwiyKYiOiJBIiwix6AiOiJBIiwi4bqgIjoiQSIsIsiAIjoiQSIsIsOAIjoiQSIsIuG6oiI6IkEiLCLIgiI6IkEiLCLEgCI6IkEiLCLEhCI6IkEiLCLDhSI6IkEiLCLHuiI6IkEiLCLhuIAiOiJBIiwiyLoiOiJBIiwiw4MiOiJBIiwi6pyyIjoiQUEiLCLDhiI6IkFFIiwix7wiOiJBRSIsIseiIjoiQUUiLCLqnLQiOiJBTyIsIuqctiI6IkFVIiwi6py4IjoiQVYiLCLqnLoiOiJBViIsIuqcvCI6IkFZIiwi4biCIjoiQiIsIuG4hCI6IkIiLCLGgSI6IkIiLCLhuIYiOiJCIiwiyYMiOiJCIiwixoIiOiJCIiwixIYiOiJDIiwixIwiOiJDIiwiw4ciOiJDIiwi4biIIjoiQyIsIsSIIjoiQyIsIsSKIjoiQyIsIsaHIjoiQyIsIsi7IjoiQyIsIsSOIjoiRCIsIuG4kCI6IkQiLCLhuJIiOiJEIiwi4biKIjoiRCIsIuG4jCI6IkQiLCLGiiI6IkQiLCLhuI4iOiJEIiwix7IiOiJEIiwix4UiOiJEIiwixJAiOiJEIiwixosiOiJEIiwix7EiOiJEWiIsIseEIjoiRFoiLCLDiSI6IkUiLCLElCI6IkUiLCLEmiI6IkUiLCLIqCI6IkUiLCLhuJwiOiJFIiwiw4oiOiJFIiwi4bq+IjoiRSIsIuG7hiI6IkUiLCLhu4AiOiJFIiwi4buCIjoiRSIsIuG7hCI6IkUiLCLhuJgiOiJFIiwiw4siOiJFIiwixJYiOiJFIiwi4bq4IjoiRSIsIsiEIjoiRSIsIsOIIjoiRSIsIuG6uiI6IkUiLCLIhiI6IkUiLCLEkiI6IkUiLCLhuJYiOiJFIiwi4biUIjoiRSIsIsSYIjoiRSIsIsmGIjoiRSIsIuG6vCI6IkUiLCLhuJoiOiJFIiwi6p2qIjoiRVQiLCLhuJ4iOiJGIiwixpEiOiJGIiwix7QiOiJHIiwixJ4iOiJHIiwix6YiOiJHIiwixKIiOiJHIiwixJwiOiJHIiwixKAiOiJHIiwixpMiOiJHIiwi4bigIjoiRyIsIsekIjoiRyIsIuG4qiI6IkgiLCLIniI6IkgiLCLhuKgiOiJIIiwixKQiOiJIIiwi4rGnIjoiSCIsIuG4piI6IkgiLCLhuKIiOiJIIiwi4bikIjoiSCIsIsSmIjoiSCIsIsONIjoiSSIsIsSsIjoiSSIsIsePIjoiSSIsIsOOIjoiSSIsIsOPIjoiSSIsIuG4riI6IkkiLCLEsCI6IkkiLCLhu4oiOiJJIiwiyIgiOiJJIiwiw4wiOiJJIiwi4buIIjoiSSIsIsiKIjoiSSIsIsSqIjoiSSIsIsSuIjoiSSIsIsaXIjoiSSIsIsSoIjoiSSIsIuG4rCI6IkkiLCLqnbkiOiJEIiwi6p27IjoiRiIsIuqdvSI6IkciLCLqnoIiOiJSIiwi6p6EIjoiUyIsIuqehiI6IlQiLCLqnawiOiJJUyIsIsS0IjoiSiIsIsmIIjoiSiIsIuG4sCI6IksiLCLHqCI6IksiLCLEtiI6IksiLCLisakiOiJLIiwi6p2CIjoiSyIsIuG4siI6IksiLCLGmCI6IksiLCLhuLQiOiJLIiwi6p2AIjoiSyIsIuqdhCI6IksiLCLEuSI6IkwiLCLIvSI6IkwiLCLEvSI6IkwiLCLEuyI6IkwiLCLhuLwiOiJMIiwi4bi2IjoiTCIsIuG4uCI6IkwiLCLisaAiOiJMIiwi6p2IIjoiTCIsIuG4uiI6IkwiLCLEvyI6IkwiLCLisaIiOiJMIiwix4giOiJMIiwixYEiOiJMIiwix4ciOiJMSiIsIuG4viI6Ik0iLCLhuYAiOiJNIiwi4bmCIjoiTSIsIuKxriI6Ik0iLCLFgyI6Ik4iLCLFhyI6Ik4iLCLFhSI6Ik4iLCLhuYoiOiJOIiwi4bmEIjoiTiIsIuG5hiI6Ik4iLCLHuCI6Ik4iLCLGnSI6Ik4iLCLhuYgiOiJOIiwiyKAiOiJOIiwix4siOiJOIiwiw5EiOiJOIiwix4oiOiJOSiIsIsOTIjoiTyIsIsWOIjoiTyIsIseRIjoiTyIsIsOUIjoiTyIsIuG7kCI6Ik8iLCLhu5giOiJPIiwi4buSIjoiTyIsIuG7lCI6Ik8iLCLhu5YiOiJPIiwiw5YiOiJPIiwiyKoiOiJPIiwiyK4iOiJPIiwiyLAiOiJPIiwi4buMIjoiTyIsIsWQIjoiTyIsIsiMIjoiTyIsIsOSIjoiTyIsIuG7jiI6Ik8iLCLGoCI6Ik8iLCLhu5oiOiJPIiwi4buiIjoiTyIsIuG7nCI6Ik8iLCLhu54iOiJPIiwi4bugIjoiTyIsIsiOIjoiTyIsIuqdiiI6Ik8iLCLqnYwiOiJPIiwixYwiOiJPIiwi4bmSIjoiTyIsIuG5kCI6Ik8iLCLGnyI6Ik8iLCLHqiI6Ik8iLCLHrCI6Ik8iLCLDmCI6Ik8iLCLHviI6Ik8iLCLDlSI6Ik8iLCLhuYwiOiJPIiwi4bmOIjoiTyIsIsisIjoiTyIsIsaiIjoiT0kiLCLqnY4iOiJPTyIsIsaQIjoiRSIsIsaGIjoiTyIsIsiiIjoiT1UiLCLhuZQiOiJQIiwi4bmWIjoiUCIsIuqdkiI6IlAiLCLGpCI6IlAiLCLqnZQiOiJQIiwi4rGjIjoiUCIsIuqdkCI6IlAiLCLqnZgiOiJRIiwi6p2WIjoiUSIsIsWUIjoiUiIsIsWYIjoiUiIsIsWWIjoiUiIsIuG5mCI6IlIiLCLhuZoiOiJSIiwi4bmcIjoiUiIsIsiQIjoiUiIsIsiSIjoiUiIsIuG5niI6IlIiLCLJjCI6IlIiLCLisaQiOiJSIiwi6py+IjoiQyIsIsaOIjoiRSIsIsWaIjoiUyIsIuG5pCI6IlMiLCLFoCI6IlMiLCLhuaYiOiJTIiwixZ4iOiJTIiwixZwiOiJTIiwiyJgiOiJTIiwi4bmgIjoiUyIsIuG5oiI6IlMiLCLhuagiOiJTIiwixaQiOiJUIiwixaIiOiJUIiwi4bmwIjoiVCIsIsiaIjoiVCIsIsi+IjoiVCIsIuG5qiI6IlQiLCLhuawiOiJUIiwixqwiOiJUIiwi4bmuIjoiVCIsIsauIjoiVCIsIsWmIjoiVCIsIuKxryI6IkEiLCLqnoAiOiJMIiwixpwiOiJNIiwiyYUiOiJWIiwi6pyoIjoiVFoiLCLDmiI6IlUiLCLFrCI6IlUiLCLHkyI6IlUiLCLDmyI6IlUiLCLhubYiOiJVIiwiw5wiOiJVIiwix5ciOiJVIiwix5kiOiJVIiwix5siOiJVIiwix5UiOiJVIiwi4bmyIjoiVSIsIuG7pCI6IlUiLCLFsCI6IlUiLCLIlCI6IlUiLCLDmSI6IlUiLCLhu6YiOiJVIiwixq8iOiJVIiwi4buoIjoiVSIsIuG7sCI6IlUiLCLhu6oiOiJVIiwi4busIjoiVSIsIuG7riI6IlUiLCLIliI6IlUiLCLFqiI6IlUiLCLhuboiOiJVIiwixbIiOiJVIiwixa4iOiJVIiwixagiOiJVIiwi4bm4IjoiVSIsIuG5tCI6IlUiLCLqnZ4iOiJWIiwi4bm+IjoiViIsIsayIjoiViIsIuG5vCI6IlYiLCLqnaAiOiJWWSIsIuG6giI6IlciLCLFtCI6IlciLCLhuoQiOiJXIiwi4bqGIjoiVyIsIuG6iCI6IlciLCLhuoAiOiJXIiwi4rGyIjoiVyIsIuG6jCI6IlgiLCLhuooiOiJYIiwiw50iOiJZIiwixbYiOiJZIiwixbgiOiJZIiwi4bqOIjoiWSIsIuG7tCI6IlkiLCLhu7IiOiJZIiwixrMiOiJZIiwi4bu2IjoiWSIsIuG7viI6IlkiLCLIsiI6IlkiLCLJjiI6IlkiLCLhu7giOiJZIiwixbkiOiJaIiwixb0iOiJaIiwi4bqQIjoiWiIsIuKxqyI6IloiLCLFuyI6IloiLCLhupIiOiJaIiwiyKQiOiJaIiwi4bqUIjoiWiIsIsa1IjoiWiIsIsSyIjoiSUoiLCLFkiI6Ik9FIiwi4bSAIjoiQSIsIuG0gSI6IkFFIiwiypkiOiJCIiwi4bSDIjoiQiIsIuG0hCI6IkMiLCLhtIUiOiJEIiwi4bSHIjoiRSIsIuqcsCI6IkYiLCLJoiI6IkciLCLKmyI6IkciLCLKnCI6IkgiLCLJqiI6IkkiLCLKgSI6IlIiLCLhtIoiOiJKIiwi4bSLIjoiSyIsIsqfIjoiTCIsIuG0jCI6IkwiLCLhtI0iOiJNIiwiybQiOiJOIiwi4bSPIjoiTyIsIsm2IjoiT0UiLCLhtJAiOiJPIiwi4bSVIjoiT1UiLCLhtJgiOiJQIiwiyoAiOiJSIiwi4bSOIjoiTiIsIuG0mSI6IlIiLCLqnLEiOiJTIiwi4bSbIjoiVCIsIuKxuyI6IkUiLCLhtJoiOiJSIiwi4bScIjoiVSIsIuG0oCI6IlYiLCLhtKEiOiJXIiwiyo8iOiJZIiwi4bSiIjoiWiIsIsOhIjoiYSIsIsSDIjoiYSIsIuG6ryI6ImEiLCLhurciOiJhIiwi4bqxIjoiYSIsIuG6syI6ImEiLCLhurUiOiJhIiwix44iOiJhIiwiw6IiOiJhIiwi4bqlIjoiYSIsIuG6rSI6ImEiLCLhuqciOiJhIiwi4bqpIjoiYSIsIuG6qyI6ImEiLCLDpCI6ImEiLCLHnyI6ImEiLCLIpyI6ImEiLCLHoSI6ImEiLCLhuqEiOiJhIiwiyIEiOiJhIiwiw6AiOiJhIiwi4bqjIjoiYSIsIsiDIjoiYSIsIsSBIjoiYSIsIsSFIjoiYSIsIuG2jyI6ImEiLCLhupoiOiJhIiwiw6UiOiJhIiwix7siOiJhIiwi4biBIjoiYSIsIuKxpSI6ImEiLCLDoyI6ImEiLCLqnLMiOiJhYSIsIsOmIjoiYWUiLCLHvSI6ImFlIiwix6MiOiJhZSIsIuqctSI6ImFvIiwi6py3IjoiYXUiLCLqnLkiOiJhdiIsIuqcuyI6ImF2Iiwi6py9IjoiYXkiLCLhuIMiOiJiIiwi4biFIjoiYiIsIsmTIjoiYiIsIuG4hyI6ImIiLCLhtawiOiJiIiwi4baAIjoiYiIsIsaAIjoiYiIsIsaDIjoiYiIsIsm1IjoibyIsIsSHIjoiYyIsIsSNIjoiYyIsIsOnIjoiYyIsIuG4iSI6ImMiLCLEiSI6ImMiLCLJlSI6ImMiLCLEiyI6ImMiLCLGiCI6ImMiLCLIvCI6ImMiLCLEjyI6ImQiLCLhuJEiOiJkIiwi4biTIjoiZCIsIsihIjoiZCIsIuG4iyI6ImQiLCLhuI0iOiJkIiwiyZciOiJkIiwi4baRIjoiZCIsIuG4jyI6ImQiLCLhta0iOiJkIiwi4baBIjoiZCIsIsSRIjoiZCIsIsmWIjoiZCIsIsaMIjoiZCIsIsSxIjoiaSIsIsi3IjoiaiIsIsmfIjoiaiIsIsqEIjoiaiIsIsezIjoiZHoiLCLHhiI6ImR6Iiwiw6kiOiJlIiwixJUiOiJlIiwixJsiOiJlIiwiyKkiOiJlIiwi4bidIjoiZSIsIsOqIjoiZSIsIuG6vyI6ImUiLCLhu4ciOiJlIiwi4buBIjoiZSIsIuG7gyI6ImUiLCLhu4UiOiJlIiwi4biZIjoiZSIsIsOrIjoiZSIsIsSXIjoiZSIsIuG6uSI6ImUiLCLIhSI6ImUiLCLDqCI6ImUiLCLhursiOiJlIiwiyIciOiJlIiwixJMiOiJlIiwi4biXIjoiZSIsIuG4lSI6ImUiLCLisbgiOiJlIiwixJkiOiJlIiwi4baSIjoiZSIsIsmHIjoiZSIsIuG6vSI6ImUiLCLhuJsiOiJlIiwi6p2rIjoiZXQiLCLhuJ8iOiJmIiwixpIiOiJmIiwi4bWuIjoiZiIsIuG2giI6ImYiLCLHtSI6ImciLCLEnyI6ImciLCLHpyI6ImciLCLEoyI6ImciLCLEnSI6ImciLCLEoSI6ImciLCLJoCI6ImciLCLhuKEiOiJnIiwi4baDIjoiZyIsIselIjoiZyIsIuG4qyI6ImgiLCLInyI6ImgiLCLhuKkiOiJoIiwixKUiOiJoIiwi4rGoIjoiaCIsIuG4pyI6ImgiLCLhuKMiOiJoIiwi4bilIjoiaCIsIsmmIjoiaCIsIuG6liI6ImgiLCLEpyI6ImgiLCLGlSI6Imh2Iiwiw60iOiJpIiwixK0iOiJpIiwix5AiOiJpIiwiw64iOiJpIiwiw68iOiJpIiwi4bivIjoiaSIsIuG7iyI6ImkiLCLIiSI6ImkiLCLDrCI6ImkiLCLhu4kiOiJpIiwiyIsiOiJpIiwixKsiOiJpIiwixK8iOiJpIiwi4baWIjoiaSIsIsmoIjoiaSIsIsSpIjoiaSIsIuG4rSI6ImkiLCLqnboiOiJkIiwi6p28IjoiZiIsIuG1uSI6ImciLCLqnoMiOiJyIiwi6p6FIjoicyIsIuqehyI6InQiLCLqna0iOiJpcyIsIsewIjoiaiIsIsS1IjoiaiIsIsqdIjoiaiIsIsmJIjoiaiIsIuG4sSI6ImsiLCLHqSI6ImsiLCLEtyI6ImsiLCLisaoiOiJrIiwi6p2DIjoiayIsIuG4syI6ImsiLCLGmSI6ImsiLCLhuLUiOiJrIiwi4baEIjoiayIsIuqdgSI6ImsiLCLqnYUiOiJrIiwixLoiOiJsIiwixpoiOiJsIiwiyawiOiJsIiwixL4iOiJsIiwixLwiOiJsIiwi4bi9IjoibCIsIsi0IjoibCIsIuG4tyI6ImwiLCLhuLkiOiJsIiwi4rGhIjoibCIsIuqdiSI6ImwiLCLhuLsiOiJsIiwixYAiOiJsIiwiyasiOiJsIiwi4baFIjoibCIsIsmtIjoibCIsIsWCIjoibCIsIseJIjoibGoiLCLFvyI6InMiLCLhupwiOiJzIiwi4bqbIjoicyIsIuG6nSI6InMiLCLhuL8iOiJtIiwi4bmBIjoibSIsIuG5gyI6Im0iLCLJsSI6Im0iLCLhta8iOiJtIiwi4baGIjoibSIsIsWEIjoibiIsIsWIIjoibiIsIsWGIjoibiIsIuG5iyI6Im4iLCLItSI6Im4iLCLhuYUiOiJuIiwi4bmHIjoibiIsIse5IjoibiIsIsmyIjoibiIsIuG5iSI6Im4iLCLGniI6Im4iLCLhtbAiOiJuIiwi4baHIjoibiIsIsmzIjoibiIsIsOxIjoibiIsIseMIjoibmoiLCLDsyI6Im8iLCLFjyI6Im8iLCLHkiI6Im8iLCLDtCI6Im8iLCLhu5EiOiJvIiwi4buZIjoibyIsIuG7kyI6Im8iLCLhu5UiOiJvIiwi4buXIjoibyIsIsO2IjoibyIsIsirIjoibyIsIsivIjoibyIsIsixIjoibyIsIuG7jSI6Im8iLCLFkSI6Im8iLCLIjSI6Im8iLCLDsiI6Im8iLCLhu48iOiJvIiwixqEiOiJvIiwi4bubIjoibyIsIuG7oyI6Im8iLCLhu50iOiJvIiwi4bufIjoibyIsIuG7oSI6Im8iLCLIjyI6Im8iLCLqnYsiOiJvIiwi6p2NIjoibyIsIuKxuiI6Im8iLCLFjSI6Im8iLCLhuZMiOiJvIiwi4bmRIjoibyIsIserIjoibyIsIsetIjoibyIsIsO4IjoibyIsIse/IjoibyIsIsO1IjoibyIsIuG5jSI6Im8iLCLhuY8iOiJvIiwiyK0iOiJvIiwixqMiOiJvaSIsIuqdjyI6Im9vIiwiyZsiOiJlIiwi4baTIjoiZSIsIsmUIjoibyIsIuG2lyI6Im8iLCLIoyI6Im91Iiwi4bmVIjoicCIsIuG5lyI6InAiLCLqnZMiOiJwIiwixqUiOiJwIiwi4bWxIjoicCIsIuG2iCI6InAiLCLqnZUiOiJwIiwi4bW9IjoicCIsIuqdkSI6InAiLCLqnZkiOiJxIiwiyqAiOiJxIiwiyYsiOiJxIiwi6p2XIjoicSIsIsWVIjoiciIsIsWZIjoiciIsIsWXIjoiciIsIuG5mSI6InIiLCLhuZsiOiJyIiwi4bmdIjoiciIsIsiRIjoiciIsIsm+IjoiciIsIuG1syI6InIiLCLIkyI6InIiLCLhuZ8iOiJyIiwiybwiOiJyIiwi4bWyIjoiciIsIuG2iSI6InIiLCLJjSI6InIiLCLJvSI6InIiLCLihoQiOiJjIiwi6py/IjoiYyIsIsmYIjoiZSIsIsm/IjoiciIsIsWbIjoicyIsIuG5pSI6InMiLCLFoSI6InMiLCLhuaciOiJzIiwixZ8iOiJzIiwixZ0iOiJzIiwiyJkiOiJzIiwi4bmhIjoicyIsIuG5oyI6InMiLCLhuakiOiJzIiwiyoIiOiJzIiwi4bW0IjoicyIsIuG2iiI6InMiLCLIvyI6InMiLCLJoSI6ImciLCLhtJEiOiJvIiwi4bSTIjoibyIsIuG0nSI6InUiLCLFpSI6InQiLCLFoyI6InQiLCLhubEiOiJ0IiwiyJsiOiJ0IiwiyLYiOiJ0Iiwi4bqXIjoidCIsIuKxpiI6InQiLCLhuasiOiJ0Iiwi4bmtIjoidCIsIsatIjoidCIsIuG5ryI6InQiLCLhtbUiOiJ0IiwixqsiOiJ0IiwiyogiOiJ0IiwixaciOiJ0Iiwi4bW6IjoidGgiLCLJkCI6ImEiLCLhtIIiOiJhZSIsIsedIjoiZSIsIuG1tyI6ImciLCLJpSI6ImgiLCLKriI6ImgiLCLKryI6ImgiLCLhtIkiOiJpIiwiyp4iOiJrIiwi6p6BIjoibCIsIsmvIjoibSIsIsmwIjoibSIsIuG0lCI6Im9lIiwiybkiOiJyIiwiybsiOiJyIiwiyboiOiJyIiwi4rG5IjoiciIsIsqHIjoidCIsIsqMIjoidiIsIsqNIjoidyIsIsqOIjoieSIsIuqcqSI6InR6Iiwiw7oiOiJ1Iiwixa0iOiJ1Iiwix5QiOiJ1Iiwiw7siOiJ1Iiwi4bm3IjoidSIsIsO8IjoidSIsIseYIjoidSIsIseaIjoidSIsIsecIjoidSIsIseWIjoidSIsIuG5syI6InUiLCLhu6UiOiJ1IiwixbEiOiJ1IiwiyJUiOiJ1Iiwiw7kiOiJ1Iiwi4bunIjoidSIsIsawIjoidSIsIuG7qSI6InUiLCLhu7EiOiJ1Iiwi4burIjoidSIsIuG7rSI6InUiLCLhu68iOiJ1IiwiyJciOiJ1IiwixasiOiJ1Iiwi4bm7IjoidSIsIsWzIjoidSIsIuG2mSI6InUiLCLFryI6InUiLCLFqSI6InUiLCLhubkiOiJ1Iiwi4bm1IjoidSIsIuG1qyI6InVlIiwi6p24IjoidW0iLCLisbQiOiJ2Iiwi6p2fIjoidiIsIuG5vyI6InYiLCLKiyI6InYiLCLhtowiOiJ2Iiwi4rGxIjoidiIsIuG5vSI6InYiLCLqnaEiOiJ2eSIsIuG6gyI6InciLCLFtSI6InciLCLhuoUiOiJ3Iiwi4bqHIjoidyIsIuG6iSI6InciLCLhuoEiOiJ3Iiwi4rGzIjoidyIsIuG6mCI6InciLCLhuo0iOiJ4Iiwi4bqLIjoieCIsIuG2jSI6IngiLCLDvSI6InkiLCLFtyI6InkiLCLDvyI6InkiLCLhuo8iOiJ5Iiwi4bu1IjoieSIsIuG7syI6InkiLCLGtCI6InkiLCLhu7ciOiJ5Iiwi4bu/IjoieSIsIsizIjoieSIsIuG6mSI6InkiLCLJjyI6InkiLCLhu7kiOiJ5IiwixboiOiJ6Iiwixb4iOiJ6Iiwi4bqRIjoieiIsIsqRIjoieiIsIuKxrCI6InoiLCLFvCI6InoiLCLhupMiOiJ6IiwiyKUiOiJ6Iiwi4bqVIjoieiIsIuG1tiI6InoiLCLhto4iOiJ6IiwiypAiOiJ6IiwixrYiOiJ6IiwiyYAiOiJ6Iiwi76yAIjoiZmYiLCLvrIMiOiJmZmkiLCLvrIQiOiJmZmwiLCLvrIEiOiJmaSIsIu+sgiI6ImZsIiwixLMiOiJpaiIsIsWTIjoib2UiLCLvrIYiOiJzdCIsIuKCkCI6ImEiLCLigpEiOiJlIiwi4bWiIjoiaSIsIuKxvCI6ImoiLCLigpIiOiJvIiwi4bWjIjoiciIsIuG1pCI6InUiLCLhtaUiOiJ2Iiwi4oKTIjoieCJ9";
            var Latinise = {}; Latinise.latin_map = JSON.parse(decodeURIComponent(escape(atob(base64map))));
            String.prototype.latinise = function () {
                return this.replace(/[^A-Za-z0-9\[\] ]/g, function (x) { return Latinise.latin_map[x] || x; });
            };
            String.prototype.latinize = String.prototype.latinise;
            String.prototype.isLatin = function () { return this == this.latinise() }
            var placeSearch, billingautocomplete, shippingautocomplete;
            var billingcomponentForm = {

                sublocality_level_1: 'ctl00_maincontent_txtadd2_Bill,short_name',
                locality: 'ctl00_maincontent_txttown_Bill,long_name',
                postal_town: 'ctl00_maincontent_txttown_Bill,long_name',
                administrative_area_level_1: 'ctl00_maincontent_txtstate_Bill,short_name',
                country: 'ctl00_maincontent_drpcountry_Bill,long_name',
                postal_code: 'ctl00_maincontent_txtzip_bill,short_name'
            };

            var shippingcomponentForm = {

                sublocality_level_1: 'ctl00_maincontent_txtadd2,short_name',
                locality: 'ctl00_maincontent_txttown,long_name',
                postal_town: 'ctl00_maincontent_txttown,long_name',
                administrative_area_level_1: 'ctl00_maincontent_txtstate,short_name',
                country: 'ctl00_maincontent_drpcountry,long_name',
                postal_code: 'ctl00_maincontent_txtzip,short_name'
            };

            function initAutocomplete() {
           
                shippingautocomplete = new google.maps.places.Autocomplete(
               (document.getElementById('ctl00_maincontent_txtsadd')),
              { types: ['address'] });
                shippingautocomplete.addListener('place_changed', fillInAddressShipping);


                if ($('#ctl00_maincontent_L2DivBilling').css('display') == 'block') {
             //       alert("shown");
                    billingautocomplete = new google.maps.places.Autocomplete(
                  /** @type {!HTMLInputElement} */(document.getElementById('ctl00_maincontent_txtsadd_Bill')),
                { types: ['address'] });

                    // When the user selects an address from the dropdown, populate the address
                    // fields in the form.
                    billingautocomplete.addListener('place_changed', fillInAddress);
                }

                //if (document.getElementById('ctl00_maincontent_ChkBillingAdd').checked == false) {
                //    alert("jtech");

                //    billingautocomplete = new google.maps.places.Autocomplete(
                //    /** @type {!HTMLInputElement} */(document.getElementById('ctl00_maincontent_txtsadd_Bill')),
                //  { types: ['address'] });



                //    // When the user selects an address from the dropdown, populate the address
                //    // fields in the form.
                //    billingautocomplete.addListener('place_changed', fillInAddress);
                //}

                AddKeyTrap();
            }
            function AddKeyTrap() {
                $("input[id*='txtsadd']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txtadd2']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txttown']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txtstate']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txtzip']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txtsadd_Bill']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txtadd2_Bill']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txttown_Bill']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txtstate_Bill']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txtzip_bill']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txtstate_Bill']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txtbillbusname']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txtbillname']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
                $("input[id*='txt_attnto']").keydown(function (e) {
                    if (e.keyCode == 13) {
                        e.preventDefault()
                        e.stopPropagation();
                        return false;
                    }
                });
            }
            function fillInAddress() {
                // Get the place details from the autocomplete object.

                document.getElementById("ctl00_maincontent_txtadd2_Bill").value = '';
                document.getElementById("ctl00_maincontent_txttown_Bill").value = '';
                document.getElementById("ctl00_maincontent_drpstate_Bill").value = '';
                document.getElementById("ctl00_maincontent_txtstate_Bill").value = '';

                document.getElementById("ctl00_maincontent_txtzip_bill").value = '';
                var place = billingautocomplete.getPlace();



                // Get each component of the address from the place details
                // and fill the corresponding field on the form.
                for (var i = 0; i < place.address_components.length; i++) {
                    var addressType = place.address_components[i].types[0];
                    if (billingcomponentForm[addressType]) {
                        var val = place.address_components[i][billingcomponentForm[addressType].split(',')[1]].toString().latinise();
                        var ctl = document.getElementById(billingcomponentForm[addressType].split(',')[0]);
                        if (addressType === 'country') {
                            if (val.toLowerCase() == "australia") {
                                document.getElementById('ctl00_maincontent_txtstate_Bill').style.display = 'none';
                                document.getElementById('ctl00_maincontent_drpstate_Bill').style.display = 'block';
                            } else {
                                document.getElementById('ctl00_maincontent_txtstate_Bill').style.display = 'block';
                                document.getElementById('ctl00_maincontent_drpstate_Bill').style.display = 'none';
                            }

                            if (val.toLowerCase() == "united states") {

                                document.getElementById("ctl00_maincontent_drpcountry_Bill").value = "US";
                                break;
                            }
                            else if (val.toLowerCase() == "germany") {

                                document.getElementById("ctl00_maincontent_drpcountry_Bill").value = "DE";
                                break;
                            }
                            else if (val.toLowerCase() == "vietnam") {

                                document.getElementById("ctl00_maincontent_drpcountry_Bill").value = "VN";
                                break;
                            }
                            else if (val.toLowerCase() == "hong kong") {

                                document.getElementById("ctl00_maincontent_drpcountry_Bill").value = "HK";
                                break;
                            }
                            else if (val.toLowerCase() == "tanzania") {
                                document.getElementById("ctl00_maincontent_drpcountry_Bill").value = "tanzania";
                                break;
                            }
                        }

                    }
                }

                //Fill in address line1 need to do this separately because of google autopopulating unit numbers when not entered
                document.getElementById('ctl00_maincontent_txtsadd_Bill').value =
                    document.getElementById('ctl00_maincontent_txtsadd_Bill').value.split(',')[0];
                document.getElementById('ctl00_maincontent_txtadd2_Bill').value = '';
                // Get each component of the address from the place details
                // and fill the corresponding field on the form.
                for (var i = 0; i < place.address_components.length; i++) {
                    var addressType = place.address_components[i].types[0];
                    //var state_val = place.address_components[i][shippingcomponentForm[addressType].split(',')[1]].toString().latinise();
                    //if (addressType == 'administrative_area_level_1') {
                    //    document.getElementById("ctl00_maincontent_drpstate").value = state_val;
                    //}
                    if (billingcomponentForm[addressType]) {
                        var val = place.address_components[i][billingcomponentForm[addressType].split(',')[1]];
                        // alert(shippingcomponentForm[addressType].split(',')[0]);
                        var ctl = document.getElementById(billingcomponentForm[addressType].split(',')[0]);
                        //     alert(addressType);
                        //ctl.value = '';
                 //       alert(addressType + " value " + ctl);
                        if (addressType == 'administrative_area_level_1') {
                            document.getElementById("ctl00_maincontent_drpstate_Bill").value = val;
                        }

                        if (ctl.nodeName === 'INPUT') 
                        {
                            ctl.value = '';
                            if (ctl.value === '') {
                                ctl.value = val;
                            } else {
                                if (addressType === 'street_number') {
                                    ctl.value += '/' + val;
                                } else {
                                    ctl.value += ' ' + val;
                                }
                            }
                        } else if (ctl.nodeName === 'SELECT') {
                            SetSelectedText(ctl, val);
                        }
                    }
                }
                checkIsNotEmpty($("#<%=txttown_Bill.ClientID%>").attr("name"), $("#<%=txttown_Bill.ClientID%>").val());
                checkIsNotEmpty($("#<%=txtzip_bill.ClientID%>").attr("name"), $("#<%=txtzip_bill.ClientID%>").val());
                checkIsNotEmpty($("#<%=drpcountry_Bill.ClientID%>").attr("name"), $("#<%=drpcountry_Bill.ClientID%>").val());
                if (checkIsNotEmpty($("#<%=drpcountry_Bill.ClientID%>").attr("name"), $("#<%=drpcountry_Bill.ClientID%>").val())) {
                    console.log("country " + $("#<%=drpcountry_Bill.ClientID%>").val().toLowerCase());
                    if ($("#<%=drpcountry_Bill.ClientID%>").val() == "AU") {
                        checkIsNotEmpty1($("#<%=drpstate_Bill.ClientID%>").attr("name"), $("#<%=drpstate_Bill.ClientID%>").val());
                    }
                    else {
                        checkIsNotEmpty1($("#<%=txtstate_Bill.ClientID%>").attr("name"), $("#<%=txtstate_Bill.ClientID%>").val());
                    }
                }
            }

            function fillInAddressShipping() {
                // Get the place details from the autocomplete object.
                document.getElementById("ctl00_maincontent_txtadd2").value = '';
                document.getElementById("ctl00_maincontent_txttown").value = '';
                document.getElementById("ctl00_maincontent_drpstate").value = '';
                document.getElementById("ctl00_maincontent_txtstate").value = '';
                document.getElementById("ctl00_maincontent_txtzip").value = '';
                var place = shippingautocomplete.getPlace();

                //Check that the country can be shipped to
                for (var i = 0; i < place.address_components.length; i++) {
                    var addressType = place.address_components[i].types[0];

                   

                    if (addressType === 'country') {
                        var val = place.address_components[i][shippingcomponentForm[addressType].split(',')[1]].toString().latinise();
                        var ctl = document.getElementById(shippingcomponentForm[addressType].split(',')[0]);
                        SetSelectedText(ctl, val);

                        if (val.toLowerCase() == "australia") {
                            document.getElementById('ctl00_maincontent_txtstate').style.display = 'none';
                            document.getElementById('ctl00_maincontent_drpstate').style.display = 'block';
                        } else {
                            document.getElementById('ctl00_maincontent_txtstate').style.display = 'block';
                            document.getElementById('ctl00_maincontent_drpstate').style.display = 'none';
                        }

                        if (val.toLowerCase() == "united states") {

                            document.getElementById("ctl00_maincontent_drpcountry").value = "US";
                            break;
                        }
                        else if (val.toLowerCase() == "germany") {

                            document.getElementById("ctl00_maincontent_drpcountry").value = "DE";
                            break;
                        }
                        else if (val.toLowerCase() == "vietnam") {

                            document.getElementById("ctl00_maincontent_drpcountry").value = "VN";

                            break;
                        }
                        else if (val.toLowerCase() == "hong kong") {

                            document.getElementById("ctl00_maincontent_drpcountry").value = "HK";
                            break;
                        }
                        else if (val.toLowerCase() == "tanzania") {

                            document.getElementById("ctl00_maincontent_drpcountry").value = "tanzania";
                            break;
                        }
                    }
                    
                }
                

                //for (var component in shippingcomponentForm) {
                //    alert(shippingcomponentForm[component]);
                //    var ctl = document.getElementById(shippingcomponentForm[component].spltit(',')[0]);

                //    if (ctl.nodeName === 'INPUT') {
                //        ctl.value = '';
                //        ctl.disabled = false;
                //    }
                //}

                //Fill in address line1 need to do this separately because of google autopopulating unit numbers when not entered
                document.getElementById('ctl00_maincontent_txtsadd').value =
                   document.getElementById('ctl00_maincontent_txtsadd').value.split(',')[0];
                document.getElementById('ctl00_maincontent_txtadd2').value = ''
                // Get each component of the address from the place details
                // and fill the corresponding field on the form.
                for (var i = 0; i < place.address_components.length; i++) {
                    var addressType = place.address_components[i].types[0];
                    //var state_val = place.address_components[i][shippingcomponentForm[addressType].split(',')[1]].toString().latinise();
                    //if (addressType == 'administrative_area_level_1') {
                    //    document.getElementById("ctl00_maincontent_drpstate").value = state_val;
                    //}
                    if (shippingcomponentForm[addressType]) {
                        var val = place.address_components[i][shippingcomponentForm[addressType].split(',')[1]];
                        // alert(shippingcomponentForm[addressType].split(',')[0]);
                        var ctl = document.getElementById(shippingcomponentForm[addressType].split(',')[0]);
                        //ctl.value = '';
                        
                        if (addressType == 'administrative_area_level_1') {
                            document.getElementById("ctl00_maincontent_drpstate").value = val;
                        }

                        if (ctl.nodeName === 'INPUT') 
                        {
                            ctl.value = '';
                            if (ctl.value === '') {
                                ctl.value = val;
                            } else {
                                if (addressType === 'street_number') {
                                    ctl.value += '/' + val;
                                } else {
                                    ctl.value += ' ' + val;
                                }
                            }
                        } else if (ctl.nodeName === 'SELECT') {
                            SetSelectedText(ctl, val);
                        }
                    }
                }
                checkIsNotEmpty($("#<%=txttown.ClientID%>").attr("name"), $("#<%=txttown.ClientID%>").val());
                checkIsNotEmpty($("#<%=txtzip.ClientID%>").attr("name"), $("#<%=txtzip.ClientID%>").val());
                checkIsNotEmpty($("#<%=drpcountry.ClientID%>").attr("name"), $("#<%=drpcountry.ClientID%>").val());
                if (checkIsNotEmpty($("#<%=drpcountry.ClientID%>").attr("name"), $("#<%=drpcountry.ClientID%>").val()))
                {
                    if ($("#<%=drpcountry.ClientID%>").val() == "AU") {
                        checkIsNotEmpty1($("#<%=drpstate.ClientID%>").attr("name"), $("#<%=drpstate.ClientID%>").val());
                    }
                    else {
                        checkIsNotEmpty1($("#<%=txtstate.ClientID%>").attr("name"), $("#<%=txtstate.ClientID%>").val());
                    }
                }
            }
            
            function SetSelectedText(dd, textToFind) {
                //  dd.selectedIndex = -1;
                for (var i = 0; i < dd.options.length; i++) {
                    if (dd.options[i].text.toLowerCase() === textToFind.toLowerCase()) {
                        dd.selectedIndex = i;
                        break;
                    }



                }
            }

            // Bias the autocomplete object to the user's geographical location,
            // as supplied by the browser's 'navigator.geolocation' object.
            function geolocate() {

                //if (navigator.geolocation) {
                //  navigator.geolocation.getCurrentPosition(function(position) {
                var geolocation = {
                    lat: -37.874, //position.coords.latitude,
                    lng: 145.0425 //position.coords.longitude
                };
                var circle = new google.maps.Circle({
                    center: geolocation,
                    radius: 20  //position.coords.accuracy
                });
                //billingautocomplete.setBounds(circle.getBounds());
                shippingautocomplete.setBounds(circle.getBounds());

                //});
                //}
            }
            function geolocate_bill() {
                //if (navigator.geolocation) {
                //  navigator.geolocation.getCurrentPosition(function(position) {
                var geolocation = {
                    lat: -37.874, //position.coords.latitude,
                    lng: 145.0425 //position.coords.longitude
                };
                var circle = new google.maps.Circle({
                    center: geolocation,
                    radius: 20  //position.coords.accuracy
                });
                billingautocomplete.setBounds(circle.getBounds());






                //shippingautocomplete.setBounds(circle.getBounds());
                //});
                //}
            }


        </script>
    <script type="text/javascript">

        function keyboardup(id) {
            var value = id.value
            // id.value = value.replace(/'/, '`');
            var e = e || window.event;

            if (e.keyCode == '37' || e.keyCode == '38' || e.keyCode == '39' || e.keyCode == '40' || e.keyCode == '8') {
            }
            else {
                id.value = value.replace(/'/g, "`")
            }
           return Validate(id);
        }
        function Validate(txt) {

            if (txt != null && txt.value != "" && !txt.value.match(/^[a-zA-Z0-9?=.,:*!@#$%^&*_\-\s]+$/)) {
                var lastChar = txt.value[txt.value.length - 1];
                if (!lastChar.match(/^[a-zA-Z0-9?=.,:*!@#$%^&*_\-\s]+$/)) {

                    if (!txt.value.match("/")) {
                        txt.value = txt.value.replace(lastChar, '');
                        if (!txt.value.match(/^[a-zA-Z0-9?=.,:*!@#$%^&*_\-\s]+$/)) {
                            alert("Invalid Text ': " + txt.value + " '");
                            txt.value = '';
                        }
                        else {
                            alert("Invalid Text : ' " + lastChar + " '");
                        }
                    }
                }
                else {
                    if (!txt.value.match("/")) {
                        alert("Invalid Text ': " + txt.value + " '");
                        txt.value = '';
                    }
                }
            }
            else {
                return true;
            }
        }

        function keyboardup1(id) {
            var value = id.value
            // id.value = value.replace(/'/, '`');
            var e = e || window.event;

            if (e.keyCode == '37' || e.keyCode == '38' || e.keyCode == '39' || e.keyCode == '40' || e.keyCode == '8') {
            }
            else {
                id.value = value.replace(/'/g, "`")
            }
            return Validate1(id);
        }
        function Validate1(txt) {

            if (txt != null && txt.value != "" && !txt.value.match(/^[a-zA-Z0-9.,:;\s]+$/)) {
                var lastChar = txt.value[txt.value.length - 1];
                if (!lastChar.match(/^[a-zA-Z0-9.,:;\s]+$/)) {

                    //if (!txt.value.match("/")) {
                        txt.value = txt.value.replace(lastChar, '');
                        if (!txt.value.match(/^[a-zA-Z0-9.,:;\s]+$/)) {
                            alert("Invalid Text ': " + txt.value + " '");
                            txt.value = '';
                        }
                        else {
                            alert("Invalid Text : ' " + lastChar + " '");
                        }
                    //}
                }
                else {
                    if (!txt.value.match("/")) {
                        alert("Invalid Text ': " + txt.value + " '");
                        txt.value = '';
                    }
                }
            }
            else {
                return true;
            }
        }

        function isAlphabetic() {
            var ValidChars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890/\\';
            var sText = document.getElementById("ctl00_maincontent_ttOrder").value;
            var IsAlphabetic = true;
            var Char;
            var ErrMsg = 'Sorry, but the use of the following special characters is not allowed in the Order No field:' + '\n' + '! ` & ~ ^ * %  $ @ ’ ( “ ) ; [   ] { } ! = < >  | * , . -' + '\n' + 'Please update your order no so it longer has any of these restricted characters in it to continue order.';
            var err = document.getElementById("ctl00_MainContent_txterr");
            if (err != null) {
                err.innerHTML = '';
            }
            for (i = 0; i < sText.length; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    alert(ErrMsg);
                    document.getElementById("ctl00_maincontent_ttOrder").value = '';
                    return false;
                }
            }
            checkponum();
            return isAlphabetic;
        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function CloseCC() {
            $("#ctl00_maincontent_CCPopDiv").removeClass("modal fade lgn-orderinfoscp in");
            $("#ctl00_maincontent_CCPopDiv").addClass("modal fade lgn-orderinfoscp");

            document.getElementById("ctl00_maincontent_CCPopDiv").style.display = 'none';


        }
        function CloseSCP() {
            $("#ctl00_maincontent_SCP").removeClass("modal fade lgn-orderinfoscp in");
            $("#ctl00_maincontent_SCP").addClass("modal fade lgn-orderinfoscp");
            document.getElementById("ctl00_maincontent_SCP").style.display = 'none';
        }

        function btnPayPalPayLink() {
            document.getElementById("ctl00_maincontent_ImagePay").src = "http://cdn.wes.com.au/WAG/images/paypal_check.png";
            document.getElementById("ctl00_maincontent_ImagePaySP").src = "http://cdn.wes.com.au/WAG/images/master_uncheck.png";
            document.getElementById("ctl00_maincontent_SecurePayAcc").style.display = "none";
            document.getElementById("ctl00_maincontent_PayPaypalAcc").style.display = "block";
            return false;
        }

        function btnSecurePayLink() {
            document.getElementById("ctl00_maincontent_ImagePay").src = "http://cdn.wes.com.au/WAG/images/paypal_uncheck.png";
            document.getElementById("ctl00_maincontent_ImagePaySP").src = "http://cdn.wes.com.au/WAG/images/master_check.png";
            document.getElementById("ctl00_maincontent_SecurePayAcc").style.display = "block";
            document.getElementById("ctl00_maincontent_PayPaypalAcc").style.display = "none";
            return false;
        }

        function Setinit(SourceID) {

            try {
                //       $("#payment_whiteScreen").show();
                //       $("#payment_popup").show();

                $("#<%=payment_whiteScreen.ClientID%>").show();
                $("#<%=payment_popup.ClientID%>").show();
                $("#<%=hfhidepaypaldiv.ClientID%>").val("1");
                //        tb_show(null, null, null);

                //       window.location.href = "/home.aspx";
                //       window.location.href = "/Home.aspx";
                //         window.location.assign("/Home.aspx");

                var x = document.getElementById('<%= btnPay.ClientID %>');
                // var x1 = document.getElementById('<%= btnPayApi.ClientID %>');
                var y = document.getElementById('<%= BtnProgress.ClientID %>');
                //   var z = document.getElementById('<%= ImgBtnEditShipping.ClientID %>');

                if (x != null) {
                    x.style.display = "none";
                }
                //        if (x1 != null) {
                //            x1.style.display = "none";
                //        }


                y.style.display = "block";
                y.style.visibility = "visible";
                z.style.display = "block";
                z.style.visibility = "visible";
                var hidSourceID = document.getElementById("<%=hidSourceID.ClientID%>");
                hidSourceID.value = SourceID;



            }
            catch (err)
            { }
        }


        function SetinitSP() {

            var reurnval;
            //     try {
            //          var res = Page_ClientValidate();
            //          if (res == true) {

            //               var x = document.getElementById('<%= btnSP.ClientID %>');
        //               var y = document.getElementById('<%= BtnProgressSP.ClientID %>');
        //                // var z = document.getElementById('<%= ImgBtnEditShipping.ClientID %>');
        //               x.style.display = "none";
        //                y.style.display = "block";
        //                 y.style.visibility = "visible";
        //  z.style.display = "block";
        //  z.style.visibility = "visible";

        //          }
        //           else {
        // Controlvalidate('dd');
        //                reurnval= Controlvalidate('cno');
        //                reurnval=Controlvalidate('cn');
        //                reurnval= Controlvalidate('cvv');
        //            }
        //        }
        //        catch (err)
        //       { }
        //
        if (checkIsNotEmpty($("#<%=txtCardName.ClientID%>").attr("name"), $("#<%=txtCardName.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtCardNumber.ClientID%>").attr("name"), $("#<%=txtCardNumber.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtCardCVVNumber.ClientID%>").attr("name"), $("#<%=txtCardCVVNumber.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=drpExpmonth.ClientID%>").attr("name"), $("#<%=drpExpmonth.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=drpExpyear.ClientID%>").attr("name"), $("#<%=drpExpyear.ClientID%>").val()) == true) {
            var x = document.getElementById('<%= btnSP.ClientID %>');
            var y = document.getElementById('<%= BtnProgressSP.ClientID %>');
            // var z = document.getElementById('<%= ImgBtnEditShipping.ClientID %>');
            x.style.display = "none";
            y.style.display = "block";
            y.style.visibility = "visible";
            return true;
        }
        else {
            return false;
        }

        return reurnval;
    }



    function Controlvalidate(ctype) {
        try {
            if (dd != null && dd.value == 0) {

                dd.style.border = "1px solid #FF0000";
            }
            else {
                dd.style.border = "";

            }

            if (ctype == "cno") {
                var cno = document.getElementById('<%= txtCardNumber.ClientID %>');
                     if (cno != null && cno.value == "") {
                         $("#lbrftxtCardNumber").show();
                         cno.style.border = "1px solid #FF0000";
                         return false;
                     }
                     else {
                         $("#lbrftxtCardNumber").hide();
                         cno.style.border = "";
                     }
                 }
                 if (ctype == "cn") {
                     var cn = document.getElementById('<%= txtCardName.ClientID %>');
                     if (cn != null && cn.value == "") {
                         $("#lbrftxtCardName").show();
                         cn.style.border = "1px solid #FF0000";
                         return false;
                     }
                     else {
                         $("#lbrftxtCardName").hide();
                         cn.style.border = "";
                     }
                 }
                 if (ctype == "cvv") {

                     var cvv = document.getElementById('<%= txtCardCVVNumber.ClientID %>');
                     if (cvv != null && cvv.value == "") {
                         $("#lbrftxtCardCVVNumber").show();
                         cvv.style.border = "1px solid #FF0000";
                         return false;
                     }
                     else {
                         $("#lbrftxtCardCVVNumber").hide();
                         cvv.style.border = "";
                     }
                 }
             }
             catch (err)
             { }
             return true;
         }
    </script>


    <script type="text/javascript">

        $(document).ready(function () {
//            if ($("#<%=lblErrMsg.ClientID%>").text() != "") {
//                $("#<%=letstarttxtpwd.ClientID%>").addClass('error');
//                $("#<%=letstarttxtpwd.ClientID%>").next().next("label").addClass('error');
//            }
//            else {
//                $("#<%=letstarttxtpwd.ClientID%>").removeClass('error');
//                $("#<%=letstarttxtpwd.ClientID%>").next().next("label").removeClass('error');
//            }
        });
        function checkIsNotEmpty(name, value) {
            var returnval;
            var bits = name.split("$");
            var lastOne = bits[bits.length - 1];
            var val = "lbrf" + lastOne;
            console.log(lastOne);
            if (value.toString().trim() != "") {
                $("#" + val).hide();
                $("#ctl00_maincontent_" + lastOne).removeClass('error');
                $("#ctl00_maincontent_" + lastOne).next().next("label").removeClass('error');
                returnval = true;
            }
            else {
                $("#" + val).show();
                $("#ctl00_maincontent_" + lastOne).addClass('error');
                $("#ctl00_maincontent_" + lastOne).next().next("label").addClass('error');
       //         document.getElementById("ctl00_maincontent_" + lastOne).focus();
                if ("#lbre" + lastOne != null) {
                    $("#lbre" + lastOne).hide();
                }
                returnval = false;
            }

            if (returnval == true && (lastOne == "letstarttxtemail" || lastOne == "txtregemail")) {
                returnval = checkValidEmail(name, value);
            }
            if (returnval == true && (lastOne == "txtRegpassword" || lastOne == "txtRegpassword")) {
                returnval = checkValidPassword(name, value);
            }
            if (returnval == true && (lastOne == "txtMobileNumber" || lastOne == "txtchangemobilenumber")) {
                returnval = checkValidMobilePhone(name, value);
            }
           
            if (returnval == true && lastOne == "txtRegConfirmPassword") {
                if ($("#<%=txtRegpassword.ClientID%>").val() == $("#<%=txtRegConfirmPassword.ClientID%>").val()) {
                    $("#lbretxtRegConfirmPassword").hide();
                    $("#ctl00_maincontent_" + lastOne).removeClass('error');
                    $("#ctl00_maincontent_" + lastOne).next().next("label").removeClass('error');
                    returnval = true;
            }
            else {
                $("#lbretxtRegConfirmPassword").show();
                $("#ctl00_maincontent_" + lastOne).addClass('error');
                $("#ctl00_maincontent_" + lastOne).next().next("label").addClass('error');
                returnval = false;
            }
        }
       
        return returnval;
    }
 
    function onBlurValidate(id) {
        //   var isvalidchar = keyboardup(id);
        var isNotEmpty = checkIsNotEmpty(id.name, id.value);
        //if (isvalidchar == true) {
        //    var isNotEmpty = checkIsNotEmpty(id.name, id.value);
        //}
        //var bits = id.name.split("$");
        //var lastOne = bits[bits.length - 1];
        // if (isNotEmpty==true && lastOne == "letstarttxtemail") {
        //     checkValidEmail(id.name, id.value);
        // }
    }
    function onBlurValidate1(id) {
        //  var isNotEmpty=checkIsNotEmpty1(id.name, id.value);
        var bits = id.name.split("$");
        var lastOne = bits[bits.length - 1];
        if (lastOne == "txtRegMobilePhone") {
            checkValidMobilePhone(id.name, id.value);
        }
        else {
            checkIsNotEmpty1(id.name, id.value)
        }
    }
    function checkIsNotEmpty1(name, value) {
        var returnval;
        var bits = name.split("$");
        var lastOne = bits[bits.length - 1];
        var val = "";
   //     console.log(lastOne);
        if (lastOne == "txtstate")
        {
            $("#lbrfdrpstate").hide();
        }
        else if (lastOne == "drpstate")
        {
            $("#lbrftxtstate").hide();
        }
        else if (lastOne == "txtstate_Bill") {
            $("#lbrfdrpstate_Bill").hide();
        }
        else if (lastOne == "drpstate_Bill") {
            $("#lbrftxtstate_Bill").hide();
        }
        //if (lastOne == "txtstate_Bill" || lastOne == "drpstate2") {
        //    val = "lbrftxtstate_Bill";
        //}
        //   alert(val);
        if (value.toString().trim() != "") {
        //    alert("1");
            $("#lbrf" + lastOne).hide();
            console.log(lastOne);
            $("#ctl00_maincontent_" + lastOne).removeClass('error');
            if (lastOne == "drpstate" || lastOne == "drpstate_Bill") {
                $("#ctl00_maincontent_" + lastOne).next().next("label").removeClass('error');
            }
            else{
                $("#ctl00_maincontent_" + lastOne).next().next().next("label").removeClass('error');
            }
            returnval = true;
        }
        else {
         //   alert("2");
         //   document.getElementById("ctl00_maincontent_" + lastOne).focus();
            $("#lbrf" + lastOne).show();
            $("#ctl00_maincontent_" + lastOne).addClass('error');
            if (lastOne == "drpstate" || lastOne == "drpstate_Bill") {
                $("#ctl00_maincontent_" + lastOne).next().next("label").addClass('error');
            }
            else {
                $("#ctl00_maincontent_" + lastOne).next().next().next("label").addClass('error');
            }
            returnval = false;
        }
        //    alert(returnval);
        return returnval;
    }

    function checkValidEmail(name, value) {
        var bits = name.split("$");
        var lastOne = bits[bits.length - 1];
        var emailPat = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
        if (!value.match(emailPat)) {
            $("#lbre" + lastOne).show();
      //      document.getElementById("ctl00_maincontent_" + lastOne).focus();
            $("#ctl00_maincontent_" + lastOne).addClass('error');
            $("#ctl00_maincontent_" + lastOne).next().next("label").addClass('error');
            return false;
        }
        else {
            $("#lbre" + lastOne).hide();
            $("#ctl00_maincontent_" + lastOne).removeClass('error');
            $("#ctl00_maincontent_" + lastOne).next().next("label").removeClass('error');
            return true;
        }
    }

    function checkValidPassword(name, value) {
        var bits = name.split("$");
        var lastOne = bits[bits.length - 1];
        var pswdPat = /^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$/;
        if (!value.match(pswdPat)) {
            $("#lbre" + lastOne).show();
     //       document.getElementById("ctl00_maincontent_" + lastOne).focus();
            $("#ctl00_maincontent_" + lastOne).addClass('error');
            $("#ctl00_maincontent_" + lastOne).next().next("label").addClass('error');
            return false;
        }
        else {
            $("#lbre" + lastOne).hide();
            $("#ctl00_maincontent_" + lastOne).removeClass('error');
            $("#ctl00_maincontent_" + lastOne).next().next("label").removeClass('error');
            return true;
        }
    }
    function checkValidMobilePhone(name, value) {
        var bits = name.split("$");
        var lastOne = bits[bits.length - 1];
        var mobilePat = /^(04)\d{8}$/;

        if (lastOne == "txtRegMobilePhone" && value == "") {
            $("#lbre" + lastOne).hide();
            return true;
        }
        if (!value.match(mobilePat)) {
            $("#lbre" + lastOne).show();
    //        document.getElementById("ctl00_maincontent_" + lastOne).focus();
            $("#ctl00_maincontent_" + lastOne).addClass('error');
            $("#ctl00_maincontent_" + lastOne).next().next("label").addClass('error');
            return false;
        }
        else {
            $("#lbre" + lastOne).hide();
            $("#ctl00_maincontent_" + lastOne).removeClass('error');
            $("#ctl00_maincontent_" + lastOne).next().next("label").removeClass('error');
            return true;
        }
    }

    //Check the Email whether the email already exits when we create new user 
    function CheckEmailAlreadyExists() {
        var txtregemail = ($("#<%=txtregemail.ClientID%>").val() != null) ? $("#<%=txtregemail.ClientID%>").val() : '';
        var letstarttxtemail = $("#<%=letstarttxtemail.ClientID%>").val();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/BtnGetStartContinueMethod",
            data: "{'letstarttxtemail':'" + $("#<%=letstarttxtemail.ClientID%>").val() + "','txtregemail':'" + txtregemail + "'}",
                datatype: "json",
                async: false,
                success: function (response) {
                    if (txtregemail != "" && txtregemail != letstarttxtemail) {
                        $("#<%=letstarttxtemail.ClientID%>").val(txtregemail);
                    }
                    if (response.d == "1") {
                        $("#<%=txtregemail.ClientID%>").val(letstarttxtemail);
                        //             $("#<%=txtregemail.ClientID%>").attr('ReadOnly', true);
                        $("#<%=letstartregister.ClientID%>").show();
                        $("#<%=startdivpwd.ClientID%>").hide();
                        $("#<%=getstartwelcome.ClientID%>").hide();
                        $("#<%=BtnGetStartLogin.ClientID%>").hide();
                        $("#<%=letstartdivlogin.ClientID%>").hide();
                        $("#<%=BtnRegLetStart_Edit.ClientID%>").hide();
                    } else if (response.d == "2") {
                        $("#lbreptxtregemail").show()
                        $("#<%=letstartregister.ClientID%>").hide();
                        $("#<%=startdivpwd.ClientID%>").show();
                        $("#<%=getstartwelcome.ClientID%>").show();
                        $("#<%=BtnGetStartLogin.ClientID%>").show();
                        $("#<%=letstartdivlogin.ClientID%>").show();
                        $("#<%=BtnGetStartContinue.ClientID%>").hide();
                    }
                    return true;
                },
                error: function (err) {
                    //         alert("error");
                    return false;
                }
            });
    }


    //Check Whether the Email Id is present
    function BtnGetStartContinueClick() {
        var letstarttxtemail_found = checkIsNotEmpty($("#<%=letstarttxtemail.ClientID%>").attr("name"), $("#<%=letstarttxtemail.ClientID%>").val());
        //     alert("letstarttxtemail_found" + letstarttxtemail_found);
        if (checkIsNotEmpty($("#<%=letstarttxtemail.ClientID%>").attr("name"), $("#<%=letstarttxtemail.ClientID%>").val()) == false) {
            return false;
        } else {
            var txtregemail = ($("#<%=txtregemail.ClientID%>").val() != null) ? $("#<%=txtregemail.ClientID%>").val() : '';
            var letstarttxtemail = $("#<%=letstarttxtemail.ClientID%>").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Express_Checkout.aspx/BtnGetStartContinueMethod",
                data: "{'letstarttxtemail':'" + $("#<%=letstarttxtemail.ClientID%>").val() + "','txtregemail':'" + txtregemail + "'}",
                datatype: "json",
                async: false,
                success: function (response) {
                    if (txtregemail != "" && txtregemail != letstarttxtemail) {
                        $("#<%=letstarttxtemail.ClientID%>").val(txtregemail);
                    }
                    if (response.d == "1") {
                        $("#<%=txtregemail.ClientID%>").val(letstarttxtemail);
           //             $("#<%=txtregemail.ClientID%>").attr('ReadOnly', true);
                        $("#<%=letstartregister.ClientID%>").show();
                        $("#<%=startdivpwd.ClientID%>").hide();
                        $("#<%=getstartwelcome.ClientID%>").hide();
                        $("#<%=BtnGetStartLogin.ClientID%>").hide();
                        $("#<%=letstartdivlogin.ClientID%>").hide();
                        $("#<%=BtnRegLetStart_Edit.ClientID%>").hide();
                    } else if (response.d == "2") {
                        $("#<%=letstartregister.ClientID%>").hide();
                        $("#<%=startdivpwd.ClientID%>").show();
                        $("#<%=getstartwelcome.ClientID%>").show();
                        $("#<%=BtnGetStartLogin.ClientID%>").show();
                        $("#<%=letstartdivlogin.ClientID%>").show();
                        $("#<%=BtnGetStartContinue.ClientID%>").hide();
                    }
                    return true;
                },
                error: function (err) {
           //         alert("error");
                    return false;
                }
            });
    }


}


//Signin to proceed to checkout
function BtnGetStartLoginClick() {
    if (checkIsNotEmpty($("#<%=letstarttxtpwd.ClientID%>").attr("name"), $("#<%=letstarttxtpwd.ClientID%>").val()) == true) {
            var letstarttxtemail = $("#<%=letstarttxtemail.ClientID%>").val();
            var letstarttxtpwd = $("#<%=letstarttxtpwd.ClientID%>").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Express_Checkout.aspx/BtnGetStartLoginMethod",
                data: "{'letstarttxtemail':'" + letstarttxtemail + "','letstarttxtpwd':'" + letstarttxtpwd + "'}",
                datatype: "json",
                async: false,
                success: function (response) {
       //             console.log(response);
                    if (response.d == "Invalid Password" || response.d == "Invalid Login Id" || response.d == "Email Id is associated with multiple login") {
                        $("#<%=lblErrMsg.ClientID%>").text(response.d);
                        $("#<%=letstarttxtpwd.ClientID%>").addClass('error');
                        $("#<%=letstarttxtpwd.ClientID%>").next().next("label").addClass('error');
                    } else {
                        $("#<%=letstarttxtpwd.ClientID%>").removeClass('error')
                        $("#<%=letstarttxtpwd.ClientID%>").next().next("label").removeClass('error');
                        var obj = JSON.parse(response.d);
                        getUserName();
                        cartItems();
                        if (obj.userInfo.ShipCountry != "" && obj.userInfo.ShipAddress1 != "") {
                            proceed_to_lvl3(obj);
                        } else {
                            proceed_to_lvl2(obj);
                        }
                    }
                },
                error: function (err) {
              //      alert("error");
                    //        window.location.href = "/Home.aspx";
                    return false;
                }
            });
        } else {
            return false;
        }
    }

    //Get the User Name
    function getUserName() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/getUserNameMethod",
            data: "{'str':'" + '' + "'}",
            datatype: "json",
            async: false,
            success: function (response) {
                if (response.d != "")
         //           console.log(response);
                $("#<%=username.ClientID%>").text("Welcome " + response.d);
                $("#<%=logoutLink.ClientID%>").show();
                $("#<%=loginLink.ClientID%>").hide();

            },
            error: function (err) {
          //      alert("error");
                return false;
            }
        });
    }

    //Create a new user
    function BtnRegLetStartClick() {
        if (checkIsNotEmpty($("#<%=txtregemail.ClientID%>").attr("name"), $("#<%=txtregemail.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtRegFname.ClientID%>").attr("name"), $("#<%=txtRegFname.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtRegLname.ClientID%>").attr("name"), $("#<%=txtRegLname.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtRegphone.ClientID%>").attr("name"), $("#<%=txtRegphone.ClientID%>").val()) == true && checkValidMobilePhone($("#<%=txtRegMobilePhone.ClientID%>").attr("name"), $("#<%=txtRegMobilePhone.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtRegpassword.ClientID%>").attr("name"), $("#<%=txtRegpassword.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtRegConfirmPassword.ClientID%>").attr("name"), $("#<%=txtRegConfirmPassword.ClientID%>").val()) == true) {
            var letstarttxtemail = $("#<%=letstarttxtemail.ClientID%>").val();
            var txtRegpassword = $("#<%=txtRegpassword.ClientID%>").val();
            var txtRegFname = $("#<%=txtRegFname.ClientID%>").val();
            var txtRegLname = $("#<%=txtRegLname.ClientID%>").val();
            var txtRegphone = $("#<%=txtRegphone.ClientID%>").val();
            var txtregemail = $("#<%=txtregemail.ClientID%>").val();
            var txtRegMobilePhone = $("#<%=txtRegMobilePhone.ClientID%>").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Express_Checkout.aspx/BtnRegLetStartMethod",
                data: "{'letstarttxtemail':'" + letstarttxtemail + "','txtRegpassword':'" + txtRegpassword + "','txtRegFname':'" + txtRegFname + "','txtRegLname':'" + txtRegLname + "','txtRegphone':'" + txtRegphone + "','txtregemail':'" + txtregemail + "','txtRegMobilePhone':'" + txtRegMobilePhone + "'}",
                datatype: "json",
                async: false,
                success: function (response) {
                    //           console.log(response.d);
                    if (response.d != "") {
                        $("#<%=Level1.ClientID%>").hide();
                        $("#<%=Level2.ClientID%>").show();
                        $("#<%=L2name.ClientID%>").text(response.d.FirstName.toString() + " " + response.d.LastName.toString());
                        $("#<%=L2Email.ClientID%>").text(response.d.Email);
                        $("#<%=L2Phone.ClientID%>").text(response.d.Phone);

                        if (response.d.MobilePhone.toString().trim() != "") {
                            $("#<%=L2Mobile.ClientID%>").text(response.d.MobilePhone);
                            $("#<%=P2Mobile.ClientID%>").show();
                        }
                        else {
                            $("#<%=P2Mobile.ClientID%>").hide();
                        }
                        $("#<%=txt_attnto.ClientID%>").val($("#<%=L2name.ClientID%>").text());
                        $("#<%=BtnRegLetStart.ClientID%>").hide();
                        $("#<%=L2DivBilling.ClientID%>").hide();
                        $("#<%=txtstate.ClientID%>").hide();
                        $("#<%=drpstate.ClientID%>").show();
                        getUserName();
                    }

                },
                error: function (err) {
                    //      alert("error");
                    return false;
                }
            });
        }
        else {
            return false;
        }
    }



    //Edit the Contact details
    function BtnEditLoginClick() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/BtnEditLoginMethod",
            data: "{'str':''}",
            datatype: "json",
            async: false,
            success: function (response) {
        //        console.log(response);
                if (response.d != "") {
                    $("#<%=Level1.ClientID%>").show();
                    $("#<%=Level2.ClientID%>").hide();
                    $("#<%=Level3.ClientID%>").hide();
                    $("#<%=Level4.ClientID%>").hide();
                    $("#<%=letstartregister.ClientID%>").show();
                    $("#<%=BtnRegLetStart_Edit.ClientID%>").show();
                    $("#<%=letstartdivlogin.ClientID%>").hide();
                    $("#<%=BtnRegLetStart.ClientID%>").hide();
                    $("#<%=txtregemail.ClientID%>").val(response.d.Email);
                    $("#<%=txtRegFname.ClientID%>").val(response.d.FirstName);
                    $("#<%=txtRegLname.ClientID%>").val(response.d.LastName);
                    $("#<%=txtRegphone.ClientID%>").val(response.d.Phone);
                    $("#<%=txtRegMobilePhone.ClientID%>").val(response.d.MobilePhone);
                    $("#<%=txtregemail.ClientID%>").attr('ReadOnly', true);
                    $("#<%=txtregemail.ClientID%>").prop('disabled', true);
                }

            },
            error: function (err) {
                //           alert("error");
                window.location.href = "/Home.aspx";
                //            return false;
            }
        });
    }


    //Update the contact details
    function BtnRegLetStartUpdateClick() {
        if (checkIsNotEmpty($("#<%=txtregemail.ClientID%>").attr("name"), $("#<%=txtregemail.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtRegFname.ClientID%>").attr("name"), $("#<%=txtRegFname.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtRegLname.ClientID%>").attr("name"), $("#<%=txtRegLname.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtRegphone.ClientID%>").attr("name"), $("#<%=txtRegphone.ClientID%>").val()) == true && checkValidMobilePhone($("#<%=txtRegMobilePhone.ClientID%>").attr("name"), $("#<%=txtRegMobilePhone.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtRegpassword.ClientID%>").attr("name"), $("#<%=txtRegpassword.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtRegConfirmPassword.ClientID%>").attr("name"), $("#<%=txtRegConfirmPassword.ClientID%>").val()) == true) {
            var letstarttxtemail = $("#<%=letstarttxtemail.ClientID%>").val();
            var txtRegpassword = $("#<%=txtRegpassword.ClientID%>").val();
            var txtRegFname = $("#<%=txtRegFname.ClientID%>").val();
            var txtRegLname = $("#<%=txtRegLname.ClientID%>").val();
            var txtRegphone = $("#<%=txtRegphone.ClientID%>").val();
            var txtregemail = $("#<%=txtregemail.ClientID%>").val();
            var txtRegMobilePhone = $("#<%=txtRegMobilePhone.ClientID%>").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Express_Checkout.aspx/BtnRegLetStartUpdateMethod",
                data: "{'letstarttxtemail':'" + letstarttxtemail + "','txtRegpassword':'" + txtRegpassword + "','txtRegFname':'" + txtRegFname + "','txtRegLname':'" + txtRegLname + "','txtRegphone':'" + txtRegphone + "','txtregemail':'" + txtregemail + "','txtRegMobilePhone':'" + txtRegMobilePhone + "'}",
                datatype: "json",
                async: false,
                success: function (response) {
                    var obj = JSON.parse(response.d);
       //             console.log(obj);
                    if (obj.userInfo.ShipCountry != "" && obj.userInfo.ShipAddress1 != "" && obj.userInfo.ShipState != "") {
                        proceed_to_lvl3(obj);
                    } else {
                        proceed_to_lvl2(obj);
                    }
                },
                error: function (err) {
                    //              alert("error");
                    window.location.href = "/Home.aspx";
                    //              return false;
                }
            });
        }
        else {
            return false;
        }
    }

    //Proceed to Level 2
    function proceed_to_lvl2(obj) {
        console.log(obj);
        $("#<%=Level1.ClientID%>").hide();
        $("#<%=Level2.ClientID%>").show();
        $("#<%=Level3.ClientID%>").hide();
        $("#<%=Level4.ClientID%>").hide();
        $("#<%=L2name.ClientID%>").text(obj.userInfo.Contact);
        $("#<%=L2Email.ClientID%>").text(obj.userInfo.AlternateEmail);
        $("#<%=L2Phone.ClientID%>").text(obj.userInfo.Phone);
       
        if (obj.userInfo.MobilePhone.toString().trim() != "") {
            $("#<%=L2Mobile.ClientID%>").text(obj.userInfo.MobilePhone);
            $("#<%=P2Mobile.ClientID%>").show();
        }
        else {
            $("#<%=P2Mobile.ClientID%>").hide();
        }
        $("#<%=txtComname.ClientID%>").val(obj.userInfo.COMPANY_NAME);
        if (obj.userInfo.Receiver_Contact.toString().trim() != "") {
            $("#<%=txt_attnto.ClientID%>").val(obj.userInfo.Receiver_Contact);
        }
        else {
            $("#<%=txt_attnto.ClientID%>").val(obj.userInfo.Contact);
        }
        $("#<%=txtsadd.ClientID%>").val(obj.userInfo.ShipAddress1);
        $("#<%=txtadd2.ClientID%>").val(obj.userInfo.ShipAddress2);
        $("#<%=txttown.ClientID%>").val(obj.userInfo.ShipCity);
        $("#<%=txtDELIVERYINST.ClientID%>").val(obj.userInfo.DELIVERYINST);
        $("#<%=txtzip.ClientID%>").val(obj.userInfo.ShipZip);
        $('#<%=drpcountry.ClientID%> option').map(function () {
            if ($(this).text() != obj.userInfo.ShipCountry) return this;
        }).attr('selected', false);
        $('#<%=drpcountry.ClientID%> option').map(function () {
            if ($(this).text() == obj.userInfo.ShipCountry) return this;
        }).attr('selected', 'true');

        if (obj.userInfo.ShipCountry.toString().toLowerCase() == "australia") {
            $("#<%=drpstate.ClientID%>").val(obj.userInfo.ShipState);
            $("#<%=txtstate.ClientID%>").hide();
            $("#<%=drpstate.ClientID%>").show();
        }
        else{
            $("#<%=txtstate.ClientID%>").val(obj.userInfo.ShipState);
            $("#<%=txtstate.ClientID%>").show();
            $("#<%=drpstate.ClientID%>").hide();
        }
        

        //Billing

        $("#<%=txtsadd_Bill.ClientID%>").val(obj.userInfo.BillAddress1);
        $("#<%=txtadd2_Bill.ClientID%>").val(obj.userInfo.BillAddress2);
        $("#<%=txttown_Bill.ClientID%>").val(obj.userInfo.BillCity);
        $("#<%=txtzip_bill.ClientID%>").val(obj.userInfo.BillZip);
      //  $("#<%=txtstate_Bill.ClientID%>").val(obj.userInfo.BillState);
        if (obj.userInfo.BillCountry.toString().toLowerCase() == "australia") {
            $("#<%=drpstate_Bill.ClientID%>").val(obj.userInfo.BillState);
            $("#<%=txtstate_Bill.ClientID%>").hide();
            $("#<%=drpstate_Bill.ClientID%>").show();
        }
        else {
            $("#<%=txtstate_Bill.ClientID%>").val(obj.userInfo.BillState);
            $("#<%=txtstate_Bill.ClientID%>").show();
            $("#<%=drpstate_Bill.ClientID%>").hide();
        }
        
        $('#<%=drpcountry_Bill.ClientID%> option').map(function () {
            if ($(this).text() != obj.userInfo.BillCountry) return this;
        }).attr('selected', false);
        $('#<%=drpcountry_Bill.ClientID%> option').map(function () {
            if ($(this).text() == obj.userInfo.BillCountry) return this;
        }).attr('selected', 'true');

        

     if (obj.userInfo.BillCountry.toString().toLowerCase() == obj.userInfo.ShipCountry.toString().toLowerCase() && obj.userInfo.BillZip == obj.userInfo.ShipZip && obj.userInfo.BillState == obj.userInfo.ShipState && obj.userInfo.BillAddress1 == obj.userInfo.ShipAddress1) {
         $("#<%=L2DivBilling.ClientID%>").hide();
            $('#<%=ChkBillingAdd.ClientID%>').prop('checked', true);
            $("#<%=txtbillbusname.ClientID%>").val(obj.userInfo.COMPANY_NAME);
         $("#<%=txtbillname.ClientID%>").val(obj.userInfo.Receiver_Contact);
        }
        else {
            $("#<%=L2DivBilling.ClientID%>").show();
            $('#<%=ChkBillingAdd.ClientID%>').prop('checked', false);
            $("#<%=txtbillbusname.ClientID%>").val(obj.userInfo.Bill_Company);
            $("#<%=txtbillname.ClientID%>").val(obj.userInfo.Bill_Name);
        }
        $("#<%=l2div.ClientID%>").focus();
    }

    //Validate the Level2 When continue is clicked
    function level2Validate() {
        var returnval;
        if (checkIsNotEmpty($("#<%=txtsadd.ClientID%>").attr("name"), $("#<%=txtsadd.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txttown.ClientID%>").attr("name"), $("#<%=txttown.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtzip.ClientID%>").attr("name"), $("#<%=txtzip.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=drpcountry.ClientID%>").attr("name"), $("#<%=drpcountry.ClientID%>").val()) == true) {
            if ($("#<%=drpcountry.ClientID%> option:selected").val() == "AU" && checkIsNotEmpty1($("#<%=drpstate.ClientID%>").attr("name"), $("#<%=drpstate.ClientID%>").val()) == true) {
                returnval = true;
            }
            else if ($("#<%=drpcountry.ClientID%> option:selected").val() != "AU" && checkIsNotEmpty1($("#<%=txtstate.ClientID%>").attr("name"), $("#<%=txtstate.ClientID%>").val()) == true) {
                returnval = true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
        if ($('#<%=ChkBillingAdd.ClientID%>').prop("checked")) {
            returnval = returnval;
        } else {
            if (checkIsNotEmpty($("#<%=txtsadd_Bill.ClientID%>").attr("name"), $("#<%=txtsadd_Bill.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txttown_Bill.ClientID%>").attr("name"), $("#<%=txttown_Bill.ClientID%>").val()) == true && checkIsNotEmpty($("#<%=txtzip_bill.ClientID%>").attr("name"), $("#<%=txtzip_bill.ClientID%>").val()) == true  && checkIsNotEmpty($("#<%=drpcountry_Bill.ClientID%>").attr("name"), $("#<%=drpcountry_Bill.ClientID%>").val()) == true) {
                if ($("#<%=drpcountry_Bill.ClientID%> option:selected").val() == "AU" && checkIsNotEmpty1($("#<%=drpstate_Bill.ClientID%>").attr("name"), $("#<%=drpstate_Bill.ClientID%>").val()) == true) {
                    returnval = true;
                }
                else if ($("#<%=drpcountry_Bill.ClientID%> option:selected").val() != "AU" && checkIsNotEmpty1($("#<%=txtstate_Bill.ClientID%>").attr("name"), $("#<%=txtstate_Bill.ClientID%>").val()) == true) {
                    returnval = true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        return returnval;
    }

    //Update the Shipping and Billing address
    function BtnL2ContinueClick() {
        if (level2Validate()) {
            var arForm = $("#formlevel2").serializeArray();
    //        console.log(arForm);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Express_Checkout.aspx/BtnL2ContinueMethod",
                data: JSON.stringify({ formVars: arForm }),
                datatype: "json",
                async: false,
                success: function (response) {
        //            console.log(response);
                    if (response.d != "") {
                        var obj = JSON.parse(response.d);
                        proceed_to_lvl3(obj);
                    }
                },
                error: function (err) {
                    //              alert("error");
                    window.location.href = "/Home.aspx";
                    //               return false;
                }
            });
        }
        else {
            return false;
        }
    }

    function proceed_to_lvl3(obj) {
        console.log(obj);
        $("#<%=Level1.ClientID%>").hide();
        $("#<%=Level2.ClientID%>").hide();
        $("#<%=Level3.ClientID%>").show();
        $("#<%=Level4.ClientID%>").hide();
        $("#<%=L3Name.ClientID%>").text(obj.userInfo.Contact);
        $("#<%=L3Email.ClientID%>").text(obj.userInfo.AlternateEmail);
        $("#<%=L3Phone.ClientID%>").text(obj.userInfo.Phone);
        if (obj.userInfo.MobilePhone.toString().trim() != "") {
            $("#<%=L3Mobile.ClientID%>").text(obj.userInfo.MobilePhone);
            $("#<%=P3Mobile.ClientID%>").show();
        }
        else {
            $("#<%=P3Mobile.ClientID%>").hide();
        }
        $("#<%=L3ship_company_name.ClientID%>").text(obj.userInfo.COMPANY_NAME);
        if (obj.userInfo.Receiver_Contact.toString().trim() != "") {
            $("#<%=L3ship_attn.ClientID%>").text(obj.userInfo.Receiver_Contact);
        }
        else
        {
            $("#<%=L3ship_attn.ClientID%>").text(obj.userInfo.Contact);
        }
        $("#<%=txt_attnto.ClientID%>").val(obj.userInfo.Receiver_Contact);
        $("#<%=L3Ship_Street.ClientID%>").text(obj.userInfo.ShipAddress1);
        $("#<%=L3Ship_Address.ClientID%>").text(obj.userInfo.ShipAddress2);
        $("#<%=L3Ship_Suburb.ClientID%>").text(obj.userInfo.ShipCity);
        $("#<%=L3Ship_State.ClientID%>").text(obj.userInfo.ShipState);
        $("#<%=L3Ship_Zipcode.ClientID%>").text(obj.userInfo.ShipZip);
        $("#<%=L3Ship_Country.ClientID%>").text(obj.userInfo.ShipCountry);
        $("#<%=L3Ship_DELIVERYINST.ClientID%>").text(obj.userInfo.DELIVERYINST);
        $("#<%=drpSM1.ClientID%>").focus();
        $("#smspopup").hide();
        loadShipMethod();
        loadNotificationMobileNo(obj);
    }

    function loadShipMethod() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/loadShipMethod",
            data: "{'str':'" + '' + "'}",
            datatype: "json",
            async: false,
            success: function (response) {
                if (response.d == 1) {
                    $("#<%=drpSM1.ClientID%>").empty();
                    $("#<%=drpSM1.ClientID%>").append(new Option("Please Select Shipping Method", "Please Select Shipping Method"));
                    $("#<%=drpSM1.ClientID%>").append(new Option("Standard Shipping", "Standard Shipping"));
                    $("#<%=drpSM1.ClientID%>").append(new Option("Shop Counter Pickup", "Counter Pickup"));
                    $("#<%=drpSM1.ClientID%>").prop('selectedIndex', 0);
                    $("#<%=intorder.ClientID%>").hide();
                    $("#<%=Div3.ClientID%>").hide();
                    $("#<%=SubmitText.ClientID%>").hide();
                    $("#<%=PaymentText.ClientID%>").show();
                } else {
                    $("#<%=drpSM1.ClientID%>").empty();
                    $("#<%=drpSM1.ClientID%>").append(new Option("Please Select Shipping Method", "Please Select Shipping Method"));
                    $("#<%=drpSM1.ClientID%>").append(new Option("International Shipping - TBA", "International Shipping - TBA"));
                    $("#<%=drpSM1.ClientID%>").prop('selectedIndex', 1);
                    $("#<%=intorder.ClientID%>").show();
                    $("#<%=Div3.ClientID%>").show();
                    $("#<%=SubmitText.ClientID%>").show()
                    $("#<%=PaymentText.ClientID%>").hide();
                }
                showPrice();
                return true;
            },
            error: function (err) {
        //        alert("error");
                return false;
            }
        });
    }

    function loadNotificationMobileNo(obj) {
        if ($("#<%=hfnothanks.ClientID%>").val() != 1) {
            if (obj.orderInfo.ShipPhone != "" && obj.orderInfo.ShipPhone != null && obj.orderInfo.ShipPhone.substring(0, 2) == "04" && obj.orderInfo.ShipPhone.toString().trim().length == 10) {
                $("#<%=lblorderready.ClientID%>").text(obj.orderInfo.ShipPhone.toString().trim());
            }
            else if (obj.userInfo.MobilePhone != "" && obj.userInfo.MobilePhone != null && obj.userInfo.MobilePhone.substring(0, 2) == "04" && obj.userInfo.MobilePhone.toString().trim().length == 10) {
                $("#<%=lblorderready.ClientID%>").text(obj.userInfo.MobilePhone.toString().trim());
            }
            else if (obj.userInfo.ShipPhone != "" && obj.userInfo.ShipPhone != null && obj.userInfo.ShipPhone.substring(0, 2) == "04" && obj.userInfo.ShipPhone.toString().trim().length == 10) {
                $("#<%=lblorderready.ClientID%>").text(obj.userInfo.ShipPhone.toString().trim());
            }
            else if (obj.userInfo.Phone != "" && obj.userInfo.Phone != null && obj.userInfo.Phone.substring(0, 2) == "04" && obj.userInfo.Phone.toString().trim().length == 10) {
                $("#<%=lblorderready.ClientID%>").text(obj.userInfo.Phone.toString().trim());
            }
}
    if (obj.userInfo.MobilePhone != "" && obj.userInfo.MobilePhone != null && obj.userInfo.MobilePhone.substring(0, 2) == "04" && obj.userInfo.MobilePhone.toString().trim().length == 10) {
        $("#<%=hfphonenumber.ClientID%>").val(obj.userInfo.MobilePhone.toString().trim());
        }
        else if (obj.userInfo.ShipPhone != "" && obj.userInfo.ShipPhone != null && obj.userInfo.ShipPhone.substring(0, 2) == "04" && obj.userInfo.ShipPhone.toString().trim().length == 10) {
            $("#<%=hfphonenumber.ClientID%>").val(obj.userInfo.ShipPhone.toString().trim());
        }
        else if (obj.userInfo.Phone != "" && obj.userInfo.Phone != null && obj.userInfo.Phone.substring(0, 2) == "04" && obj.userInfo.Phone.toString().trim().length == 10) {
            $("#<%=hfphonenumber.ClientID%>").val(obj.userInfo.Phone.toString().trim());
        }
}


//Update the Shipping and Billing address
function EditAddressClick() {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "Express_Checkout.aspx/EditAddressMethod",
        data: "{'str':''}",
        datatype: "json",
        async: false,
        success: function (response) {
            if (response.d != "") {
                var obj = JSON.parse(response.d);
                proceed_to_lvl2(obj);
            }
        },
        error: function (err) {
            //            alert("error");
            window.location.href = "/Home.aspx";
            //            return false;
        }
    });
}


function BtnL3ContinueClick() {
    if (checkIsNotEmpty($("#<%=drpSM1.ClientID%>").attr("name"), $("#<%=drpSM1.ClientID%>").val()) == true) {
            var drpSM1_selectedValue = $("#<%=drpSM1.ClientID%> option:selected").val();
            var TextBox1 = $("#<%=TextBox1.ClientID%>").val();
            var lblorderready = $("#<%=lblorderready.ClientID%>").text();
            if ($('#<%=chksavemobile.ClientID%>').prop("checked")) {
                var checked_val = true;
            } else {
                var checked_val = false;
            }

            var txtMobileNumber = ($("#<%=txtMobileNumber.ClientID%>").val() != '' && $("#<%=txtMobileNumber.ClientID%>").val() != null) ? $("#<%=txtMobileNumber.ClientID%>").val() : '';
            var ttinter_order = ($("#<%=ttinter_order.ClientID%>").val() != '' && $("#<%=ttinter_order.ClientID%>").val() != null) ? $("#<%=ttinter_order.ClientID%>").val() : '';
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Express_Checkout.aspx/BtnL3ContinueMethod",
                data: "{'drpSM1':'" + drpSM1_selectedValue + "','TextBox1':'" + TextBox1 + "','ttinter_order':'" + ttinter_order + "','txtMobileNumber':'" + txtMobileNumber + "','chksavemobile':'" + checked_val + "','lblorderready':'" + lblorderready + "','hfisppp':'" + $("#<%=hfisppp.ClientID%>").val() + "'}",
                datatype: "json",
                async: true,
                success: function (response) {
            //        console.log(response);
                    if (response.d == "Order No already exists, please Re-enter Order No") {
                        $("#<%=lblerror_ttinter.ClientID%>").text(response.d);
                        $("#<%=ttinter_order.ClientID%>").focus();
                        $("#<%=BtnL3Continue.ClientID%>").val("Continue");
                        $("#<%=BtnL3Continue.ClientID%>").attr("disabled", false);
                        return false;
                    }
                    if (response.d == "ConfirmMessage.aspx?Result=QTEEMPTY") {
                        window.location.href = "/ConfirmMessage.aspx?Result=QTEEMPTY";
                    }
                    else if (response.d != "") {
                        var obj = JSON.parse(response.d);
              //          console.log(obj);
                        var modalBox = $(this).attr('data-modal-id');
                        $('#' + modalBox).fadeIn($(this).data());
                        $("[id=popup1]").hide();

                        $(".modal-box, .modal-overlay").fadeOut(500, function () {
                            $(".modal-overlay").remove();
                        });

                        $("#<%=Level3.ClientID%>").hide();
                        $("#<%=Level4.ClientID%>").show();
                        $("#<%=L4name.ClientID%>").text(obj.userInfo.Contact);
                        $("#<%=L4Email.ClientID%>").text(obj.userInfo.AlternateEmail);
                        $("#<%=L4Phone.ClientID%>").text(obj.userInfo.Phone);
                        if (obj.userInfo.MobilePhone.toString().trim() != "") {
                            $("#<%=L4Mobile.ClientID%>").text(obj.userInfo.MobilePhone);
                            $("#<%=P4Mobile.ClientID%>").show();
                        }
                        else {
                            $("#<%=P4Mobile.ClientID%>").hide();
                        }
                        $("#<%=L4Ship_Company.ClientID%>").text(obj.userInfo.COMPANY_NAME);
                        if (obj.userInfo.Receiver_Contact.toString().trim() != "") {
                            $("#<%=L4Ship_Attnto.ClientID%>").text(obj.userInfo.Receiver_Contact);
                        }
                        else {
                            $("#<%=L4Ship_Attnto.ClientID%>").text(obj.userInfo.Contact);
                        }
                        $("#<%=L4Ship_Street.ClientID%>").text(obj.userInfo.ShipAddress1);
                        $("#<%=L4Ship_Address.ClientID%>").text(obj.userInfo.ShipAddress2);
                        $("#<%=L4Ship_Suburb.ClientID%>").text(obj.userInfo.ShipCity);
                        $("#<%=L4Ship_State.ClientID%>").text(obj.userInfo.ShipState);
                        $("#<%=L4Ship_Zipcode.ClientID%>").text(obj.userInfo.ShipZip);
                        $("#<%=L4Ship_Country.ClientID%>").text(obj.userInfo.ShipCountry);
                        $("#<%=L4Ship_DELIVERYINST.ClientID%>").text(obj.userInfo.DELIVERYINST);

                        //      lblShippingMethod.Text = oOrdInfo.ShipMethod.Replace("Counter Pickup", "Shop Counter Pickup");
                        $("#<%=L4Comments.ClientID%>").text(obj.orderInfo.ShipNotes);
                        $("#<%=L4Ship_DELIVERYINST.ClientID%>").text(obj.userInfo.DELIVERYINST);
                        $("#<%=lblShippingMethod.ClientID%>").text(obj.orderInfo.ShipMethod.replace("Counter Pickup", "Shop Counter Pickup"));
                        $("#<%=lblAmount.ClientID%>").text($("#<%=totalAmount.ClientID%>").text());
                        if (obj.orderInfo.ShipMethod == "Standard Shipping") {
                            $("#<%=divshopcounter.ClientID%>").hide();
                        } else {
                            $("#<%=divshopcounter.ClientID%>").show();
                        }
                        ProceedToPayment(obj);
                    }
                },
                error: function (err) {
                    //               alert("error");
                    window.location.href = "/Home.aspx";
                    //                return false;
                }
            });
    }
    else {
        return false;
    }

}

function ProceedToPayment(obj) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "Express_Checkout.aspx/loadShipMethod",
        data: "{'str':'" + '' + "'}",
        datatype: "json",
        async: false,
        success: function (response) {
            if (response.d == 1 && ($("#<%=hfisppp.ClientID%>").val() == "0" || ($("#<%=hfisppp.ClientID%>").val() == "1" && $("#<%=drpSM1.ClientID%> option:selected").val() != "Standard Shipping"))) {
                var encryptVal = obj.orderInfo.OrderID.toString() + "#####" + "Pay";
                //       EncryptSP(encryptVal);
           //     alert("smith");
                $("#Level4_Payment").show();
                $("#Level4_Payment").focus();
                $("#<%=div2.ClientID%>").hide();
                    $("#<%=Div3.ClientID%>").hide();
                    $("#<%=Level4_Submit.ClientID%>").hide();
                    //         $("#<%=ImageButton1.ClientID%>").hide();
                    $("#<%=PayType.ClientID%>").show();
                    $("#<%=lblpaypaltotamt.ClientID%>").text($("#<%=lblAmount.ClientID%>").text());
                    //           $("#<%=ttOrder.ClientID%>").focus();
                    $("#<%=PHOrderConfirm.ClientID%>").hide();
                    ScrollToControl("ctl00_maincontent_divl4payment");
                    if ($("#<%=lblAmount.ClientID%>").text() == "") {
                        var totamt = obj.orderInfo.TotalAmount;
                    }
                    else {
                        var totamt = $("#<%=lblAmount.ClientID%>").text();
                    }

                    if (totamt <= 300) {
                        $("#<%=lbcard.ClientID%>").show();

                    }
                    else {
                        $("#<%=lbcard.ClientID%>").hide();
                        $("#<%=SecurePayAcc.ClientID%>").hide();
                        $("#<%=PayPaypalAcc.ClientID%>").show();
                        $("#<%=ImagePay.ClientID%>").attr("src", "http://cdn.wes.com.au/WAG/images/paypal_check.png");
                    }
                    PHOrderConfirm(obj);
            }
            else {
        //        alert("enter")

                if ($("#<%=hfisppp.ClientID%>").val() == "1" && $("#<%=drpSM1.ClientID%> option:selected").val() == "Standard Shipping") {
                    if ($("#<%=ttOrder.ClientID%>").val().toString().trim() == "") {
                        $("#<%=txtintporelease_No.ClientID%>").val("WAG" + obj.orderInfo.OrderID);
                        $("#<%=ttOrder.ClientID%>").val("WAG" + obj.orderInfo.OrderID);
                        $("#<%=lblPPPorderid.ClientID%>").text("WAG" + obj.orderInfo.OrderID);
                    }
                    else {
                        $("#<%=txtintporelease_No.ClientID%>").val($("#<%=ttOrder.ClientID%>").val().toString());
                        $("#<%=lblPPPorderid.ClientID%>").text($("#<%=ttOrder.ClientID%>").val().toString());
                    }
                }
                else {
                    if ($("#<%=ttinter_order.ClientID%>").val().toString().trim() == "") {
                        $("#<%=txtintporelease_No.ClientID%>").val("WAG" + obj.orderInfo.OrderID);
                        $("#<%=ttinter_order.ClientID%>").val("WAG" + obj.orderInfo.OrderID);
                        $("#<%=lblintorderid.ClientID%>").text("WAG" + obj.orderInfo.OrderID);
                    }
                    else {
                        $("#<%=txtintporelease_No.ClientID%>").val($("#<%=ttinter_order.ClientID%>").val().toString());
                        $("#<%=lblintorderid.ClientID%>").text($("#<%=ttOrder.ClientID%>").val().toString());
                    }
                }
                    $("#<%=ImageButton1.ClientID%>").hide();
                    $("#<%=divshopcounter.ClientID%>").hide();
                    $("#<%=Div3.ClientID%>").show();
                    $("#Level4_Payment").hide();
                    $("#<%=Level4_Submit.ClientID%>").show();
                    $("#<%=checkoutrightL1.ClientID%>").show();
                    $("#<%=checkoutrightL4.ClientID%>").hide();
                    $("#<%=L4AEditAddress.ClientID%>").hide();
                    $("#<%=BtnEditAddress.ClientID%>").hide();
                    $("#<%=L3AEditAddress.ClientID%>").hide();
                    $("#<%=BtnL3EditAddress.ClientID%>").hide();
                    $("#<%=L4AEditShippingMethod.ClientID%>").hide();
                    $("#<%=BtnL4EditShippingMethod.ClientID%>").hide();
                    $("#<%=L4AEditAddress.ClientID%>").hide();
                    $("#<%=btneditlogin4.ClientID%>").hide();
                    $("#<%=PayType.ClientID%>").show();
                    $("#<%=SecurePayAcc.ClientID%>").hide();
                    $("#<%=divl4payment.ClientID%>").addClass('headingwrap visited clearfix').removeClass('headingwrap active clearfix mt20 mb20');
                    $("#Level4_Payment").addClass('col-sm-19 pv15 br_dark m10').removeClass('checkoutleft');
                    PHOrderConfirm(obj);
                }
                return true;
            },
            error: function (err) {
         //       alert("error");
                return false;
            }
        });
    }

    function EncryptSP(orderid) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/EncryptSPMethod",
            data: "{'ordid':'" + orderid + "'}",
            datatype: "json",
            async: false,
            success: function (response) {
                redirectPay(response.d);
            },
            error: function (err) {
            //    alert("error");
                return false;
            }
        });
    }
    function redirectPay(url) {
    //    alert(url);
        window.location.href = "/express_checkout.aspx?" + url;
        //      Response.Redirect("expresscheckout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay"), false);
    }

    function PHOrderConfirm(obj) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/IsNativeCountry_Express",
            data: "{'str':'" + obj.orderInfo.OrderID + "'}",
            datatype: "json",
            async: false,
            success: function (response) {
                var USER_ROLE = '<%=Session["USER_ROLE"]%>';

                if (obj.userInfo.USERROLE.toString().trim() == "3") {
                    $("#pendingOrderText").show();
                }
                else if (obj.userInfo.USERROLE.toString().trim() == "1" || obj.userInfo.USERROLE.toString().trim() == "2") {
             //       alert("responsed " + response.d);
                    if (response.d == 0) {
                    //    alert("inter");
                        $("#submitOrderText").show();
                        $("#InternationalOrderText").show();
                        $("#<%=lblintorderid.ClientID%>").text($("#<%=txtintporelease_No.ClientID%>").val().toString());
                        $("#submitOrderText").focus();
                        //    $("#<%=PayOkDiv.ClientID%>").focus();
                        $("#<%=PHOrderConfirm.ClientID%>").show();
                        $(document).ready(function () {
                            $('html,body').animate({
                                scrollTop: $('#ctl00_maincontent_PHOrderConfirm').offset().top - 100
                            }, 0);
                        });
                        //                    ScrollToControl("ctl00_maincontent_PHOrderConfirm", true);
                    }
                    else if ($("#<%=hfisppp.ClientID%>").val() == "1" && $("#<%=drpSM1.ClientID%> option:selected").val() == "Standard Shipping") {
                   //     alert("PHOrderConfirm");
                        $("#<%=PHOrderConfirm.ClientID%>").show();
                        $("#submitOrderText").show();
                        $("#PPPOrderText").show();
                        $(document).ready(function () {
                            $('html,body').animate({
                                scrollTop: $('#ctl00_maincontent_PHOrderConfirm').offset().top - 100
                            }, 0);
                        });
                    }
                }
                if (response.d == 0) {
                    $("#<%=SubmitText.ClientID%>").show()
                    $("#<%=PaymentText.ClientID%>").hide();

                }
                else if ($("#<%=hfisppp.ClientID%>").val() == "1" && $("#<%=drpSM1.ClientID%> option:selected").val() == "Standard Shipping") {
                    $("#<%=SubmitText.ClientID%>").show()
                    $("#<%=PaymentText.ClientID%>").hide();
                }
                else {
                    $("#<%=SubmitText.ClientID%>").hide()
                    $("#<%=PaymentText.ClientID%>").show();
                    $("#Level4_Payment").focus();
                }

                return true;
            },
            error: function (err) {
           //     alert("error");
                return false;
            }
        });
    }

        function level3Continue() {
            if ($("#<%=drpSM1.ClientID%> option:selected").text() == "International Shipping - TBA" || ($("#<%=drpSM1.ClientID%> option:selected").text() == "Standard Shipping" && $("#<%=hfisppp.ClientID%>").val() == "1")) {
                $("#<%=BtnL3Continue.ClientID%>").val("Processing Order.. Please Wait");
                $("#<%=BtnL3Continue.ClientID%>").attr("disabled", true);
            }

        var mobileno = $("#<%=lblorderready.ClientID%>").text();
        if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Please Select Shipping Method") {
            $("#<%=drpSM1.ClientID%>").focus();
            $("#lbrfdrpSM1").show();
            return false;
        } else if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup") {
     //       console.log("mobileno" + mobileno + "no thanks " + $("#<%=hfnothanks.ClientID%>").val());
            if (($("#<%=lblorderready.ClientID%>").text() == "" || $("#<%=lblorderready.ClientID%>").text().substring(0, 2) != 04 || $("#<%=lblorderready.ClientID%>").text().toString().trim().length != 10) && $("#<%=hfnothanks.ClientID%>").val() == "0") {

                var appendthis = ("<div class='modal-overlay js-modal-close'></div>");
                $("body").append(appendthis);
                $(".modal-overlay").fadeTo(500, 0.7);
                $(".popup1").css({
                    display: "block",
                    top: ($(window).height() - $(".modal-box").outerHeight()) / 2,
                    left: ($(window).width() - $(".modal-box").outerWidth()) / 2
                });
                //$(".js-modalbox").fadeIn(500);
                //        var modalBox = $(this).attr('data-modal-id');
                //               $('#' + modalBox).fadeIn($(this).data());

                //$(window).resize(function () {
                //    $(".modal-box").css({
                //        top: ($(window).height() - $(".modal-box").outerHeight()) / 2,
                //        left: ($(window).width() - $(".modal-box").outerWidth()) / 2
                //    });
                //});
                $("#<%=txtMobileNumber.ClientID%>").focus();
                 $("#popup1").show();
                 $("[id=divchangepopup]").hide();
                 return false;
             } else {
                 BtnL3ContinueClick();
                 return true;
             }
         } else {
             BtnL3ContinueClick();
             return true;
         }
 }

 function ScrollToControl(controlId) {
     $(document).ready(function () {
         $('html,body').animate({
             scrollTop: $('#' + controlId).offset().top - 100
         }, 0);
     });
 }

 function noThanksBtnClick() {
     $("#<%=txtMobileNumber.ClientID%>").val('');
        $("#<%=lblorderreadytext.ClientID%>").text("SMS Order ready notification will NOT be sent.");
        $("#<%=hfnothanks.ClientID%>").val('1');
        BtnL3ContinueClick();
    }

    function notifiyMeClick() {
        if (checkIsNotEmpty($("#<%=txtMobileNumber.ClientID%>").attr("name"), $("#<%=txtMobileNumber.ClientID%>").val()) == true) {
            BtnL3ContinueClick();
        }
        else {
            return false;
        }
    }

    function btnMobileNoChangeClick() {
        if (checkIsNotEmpty($("#<%=txtchangemobilenumber.ClientID%>").attr("name"), $("#<%=txtchangemobilenumber.ClientID%>").val()) == true) {
            if ($('#<%=cbmobilechange.ClientID%>').prop("checked")) {
                var checked_val = true;
            } else {
                var checked_val = false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Express_Checkout.aspx/btnMobileNoChangeMethod",
                data: "{'mobilenumber':'" + $("#<%=txtchangemobilenumber.ClientID%>").val() + "','checked_val':'" + checked_val + "'}",
                datatype: "json",
                async: false,
                success: function (response) {
         //           console.log(response.d);
                    if (response.d != 0) {
                        $("#<%=lblorderready.ClientID%>").text($("#<%=txtchangemobilenumber.ClientID%>").val());
                        $("#<%=lblorderreadytext.ClientID%>").text("SMS Order ready notification message will be sent to:");
                        $('#<%=hfnothanks.ClientID %>').attr('value', '0');
                        $("[id=divchangepopup]").hide();
                        $(".modal-box, .modal-overlay").fadeOut(500, function () {
                            $(".modal-overlay").remove();
                        });
                        LoadMobileNoChange();
                        return true;
                    }
                },
                error: function (err) {
                    //              alert("error");
                    window.location.href = "/Home.aspx";
                    //              return false;
                }
            });
        }
    }

    //NO thanks while Changing mobile No.
    function btnNoThanksChangeClick() {


        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/btnNoThanksChangeMethod",
            data: "{'str':'" + '' + "'}",
            datatype: "json",
            async: false,
            success: function (response) {
      //          console.log(response.d);
                if (response.d != 0) {
                    $("#<%=txtchangemobilenumber.ClientID%>").val($("#<%=hfphonenumber.ClientID%>").val());
                    $('#<%=cbmobilechange.ClientID%>').prop('checked', true);
                    $("#<%=lblorderready.ClientID%>").text('');
                    $("#<%=lblorderreadytext.ClientID%>").text("SMS Order ready notification will NOT be sent.");
                    //        $("#<%=hfnothanks.ClientID%>").val('1');
                    $('#<%=hfnothanks.ClientID %>').attr('value', '1');
                    $("[id=divchangepopup]").hide();
                    $(".modal-box, .modal-overlay").fadeOut(500, function () {
                        $(".modal-overlay").remove();
                    });
                    LoadMobileNoChange();
                    return true;
                }
            },
             error: function (err) {
                 //         alert("error");
                 window.location.href = "/Home.aspx";
                 //         return false;
             }
         });
    }

    //Load Notification MobileNo
    function LoadMobileNoChange() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/LoadMobileNoChangeMethod",
            data: "{'str':'" + '' + "'}",
            datatype: "json",
            async: false,
            success: function (response) {
                var obj = JSON.parse(response.d);
                loadNotificationMobileNo(obj);
            },
            error: function (err) {
        //        alert("error");
                return false;
            }
        });
    }

    //Edit the Shipping method
    function BtnL4EditShippingMethodClick() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/BtnL4EditShippingClickMethod",
            data: "{'str':'" + '' + "'}",
            datatype: "json",
            async: false,
            success: function (response) {
                var obj = JSON.parse(response.d);
                proceed_to_lvl3(obj);
                return true;
            },
            error: function (err) {
                //        alert("error");
                window.location.href = "/Home.aspx";
                //         return false;
            }
        });
    }


    //Show the Cart items
    function cartItems() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/cartItemsMethod",
            data: "{'str':'" + '' + "'}",
            datatype: "json",
            async: false,
            success: function (response) {
                var obj = JSON.parse(response.d);
                $("#orderitemlist").empty();
                var orderDetails = "";
                $(obj).each(function (index, res) {
                    orderDetails += '<div class="clearfix br_b pb15 pt25" >';
                    orderDetails += '<div class="col-xs-3">';
                    orderDetails += '<img  src="' + res.imageSrc + '" class="img_h50" Style="max-width: 60px;"> </div>';
                    orderDetails += '<div class="col-xs-17 pr0 ">';
                    orderDetails += '<div class="dblock pb10"><b class="f14">' + res.description + '</b></div>';
                    orderDetails += '<div class="os_itemdetail">'
                    orderDetails += '<div class="col-xs-6 pl0"> <span class="dblock pb5">Qty:</span>' + res.Qty + '</div>';
                    orderDetails += '<div class="col-xs-7"> <span class="dblock pb5">Item Cost:</span>' + "$ " + res.ProductUnitPrice + ' Ex GST</div>';
                    orderDetails += '<div class="col-xs-7 text-right"> <span class="dblock pb5">Sub-Total:</span>' + "$ " + res.Amt + ' Ex GST</div>';
                    orderDetails += '</div> </div> </div>';
                   
                    if (res.isppp != null && res.isppp != "") {
                        $("#<%=hfisppp.ClientID%>").val(res.isppp);
                    }
                    $("#<%=hfprodtotalprice.ClientID%>").val(res.subtot);                                       //want to change
                });
                $("#orderitemlist").append(orderDetails);
                if ($("#<%=hfisppp.ClientID%>").val() == "1") {
                    $("#<%=lblppp.ClientID%>").text("P");
                }
                else {
                    $("#<%=lblppp.ClientID%>").text("");
                }
               
                showPrice();
                return true;
            },
            error: function (err) {
                //          alert("error12");
                window.location.href = "/Home.aspx";
                //          return false;
            }
        });
    }

    //Get the optionValue
    function GetOptionValue() {
        var data = "COURIER CHARGE";
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/GetOptionValuesMethod",
            data: "{'oName':'" + data + "'}",
            datatype: "json",
            async: false,
            success: function (response) {
                //              document.getElementById("ctl00_MainContent_lblshipcost").innerText = response.d;
                return true;
            },
            error: function (err) {
         //       alert("error");
                return false;
            }
        });
    }

    //Show the Price of the Order
    function showPrice() {
        var ProdTotalPrice = $("#<%=hfprodtotalprice.ClientID%>").val();
        // var txtMobileNumber = ($("#<%=txtMobileNumber.ClientID%>").val() != '' && $("#<%=txtMobileNumber.ClientID%>").val() != null) ? $("#<%=txtMobileNumber.ClientID%>").val() : '';
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/showPriceMethod",
            data: "{'drpSM1':'" + $("#<%=drpSM1.ClientID%> option:selected").text() + "','ProdTotalPrice':'" + ProdTotalPrice + "','hfisppp':'" + $("#<%=hfisppp.ClientID%>").val() + "'}",
            datatype: "json",
            async: false,
            success: function (response) {
  //              console.log(response.d);
                $("#<%=taxAmount.ClientID%>").text(response.d.taxAmount);
               $("#<%=totalAmount.ClientID%>").text(response.d.totalAmount);
                $("#<%=lblAmount.ClientID%>").text(response.d.totalAmount);
                $("#<%=lblpaypaltotamt.ClientID%>").text(response.d.totalAmount);

                if ($("#<%=hfisppp.ClientID%>").val() == "1" && $("#<%=drpSM1.ClientID%> option:selected").val() != "Counter Pickup") {
                    $("#<%=lbDeliveryText.ClientID%>").text("Shipping Charge");
                    $("#<%=lbEstimateAmountText.ClientID%>").text("Est. Total ");
                    $("#<%=lbDelivery.ClientID%>").text("To Be Advised");
                }
                else if (response.d.orderType == "Domestic") {
                    $("#<%=lbDeliveryText.ClientID%>").text("Delivery (Ex GST)");
                    $("#<%=lbEstimateAmountText.ClientID%>").text("Est. Total Inc GST");
                    $("#<%=lbDelivery.ClientID%>").text("$ " + response.d.deliveryCharge);

                } else {
                    $("#<%=lbDeliveryText.ClientID%>").text("Shipping Charge");
                    $("#<%=lbEstimateAmountText.ClientID%>").text("Est. Total ");
                    $("#<%=lbDelivery.ClientID%>").text("To Be Advised");
                }
                return true;
            },
            error: function (err) {
        //        alert("error");
                return false;
            }
        });
    }

    //Paypal payment

    function btnPayClick() {
       
        var ttOrder = $("#<%=ttOrder.ClientID%>").val();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Express_Checkout.aspx/btnPayMethod",
            data: "{'ttOrder':'" + ttOrder + "'}",
            datatype: "json",
            async: false,
            success: function (response) {
       //         console.log(response.d);
                if (response.d == "ConfirmMessage.aspx?Result=QTEEMPTY") {
                    window.location.href = "/ConfirmMessage.aspx?Result=QTEEMPTY";
                }
                else if (response.d == "Order No already exists, please Re-enter Order No") {
                    $("#<%=txterr.ClientID%>").val(response.d);
                    $("#<%=txterr.ClientID%>").focus();
                    //          $("#<%=ImagePay.ClientID%>").attr("src", "http://cdn.wes.com.au/WAG/images/paypal_check.png");

                } else if (response.d != "") {
                    //          window.location.href = "";
             //       alert("smith");
                }
             //   alert("smith1");
                return true;
            },
            error: function (err) {
            //    alert("error");
                return false;
            }
        });
}


function checkponum() {
    var data = document.getElementById("ctl00_maincontent_ttOrder").value;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "Express_Checkout.aspx/GetData",
        data: "{'PO':'" + data + "'}",
        datatype: "json",
        async: false,
        success: function (response) {
            if (response.d == "2") {
                document.getElementById("ctl00_maincontent_txterr").innerHTML = "Purchase Order No  already exists, please Re-enter Purchase Order No ";
                return false;
            }
            else {
                document.getElementById("ctl00_maincontent_txterr").innerHTML = "";
                return true;
            }
        },
        error: function (err) {
            return false;
        }
    });
}
function checkponum_international() {

    var data = document.getElementById("ctl00_maincontent_ttinter_order").value;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "Express_Checkout.aspx/GetData",
        //data: JSON.stringify({ "data": data }),
        data: "{'PO':'" + data + "'}",
        datatype: "json",
        async: false,
        success: function (response) {
            //alert("C# method calling : " + response.d);
            if (response.d == "2") {
                document.getElementById("ctl00_maincontent_lblerror_ttinter").innerHTML = "Purchase Order No  already exists, please Re-enter Purchase Order No ";
                $("#<%=BtnL3Continue.ClientID%>").val("Continue");
                $("#<%=BtnL3Continue.ClientID%>").attr("disabled", false);
                //document.getElementById("ctl00_maincontent_ttinter_order").value = '';
                // document.forms[0].elements["<%=ttinter_order.ClientID%>"].focus();
                    return false;
                }

                else {
                    document.getElementById("ctl00_maincontent_lblerror_ttinter").innerHTML = "";
                    return true;
                }
            },
            error: function (err) {
                //alert(err.responseText);
                return false;
            }
        });
    }

    </script>

    <script type="text/javascript">
        function ShowModal() {
            $("#divchangepopup").show();
            $(window).resize(function () {
                $(".modal-box").css({
                    top: 0,
                    left: 0
                });
            });

            var appendthis = ("<div class='modal-overlay js-modal-close'></div>");
            $("body").append(appendthis);
            $(".modal-overlay").fadeTo(500, 0.7);
            //$(".js-modalbox").fadeIn(500);
            //        var modalBox = $(this).attr('data-modal-id');
            //       $('#' + modalBox).fadeIn($(this).data());


            var input = $("#<%=txtchangemobilenumber.ClientID%>");
                var len = input.val().length;
                input[0].focus();
                input[0].setSelectionRange(len, len);
                if ($("#<%=lblorderready.ClientID%>").text() != "") {
                 $("#<%=txtchangemobilenumber.ClientID%>").val($("#<%=lblorderready.ClientID%>").text());
             }
             else {
                 $("#<%=txtchangemobilenumber.ClientID%>").val($("#<%=hfphonenumber.ClientID%>").val());
             }
            //        $('html,body').animate({ scrollTop: $('#<%=divl3continue.ClientID%>').offset().top - 100 }, 0);
            return true;
        }
    </script>

    <script src="https://terrylinooo.github.io/jquery.disableAutoFill/assets/js/jquery.disableAutoFill.min.js";></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //      $('#form1').disableAutoFill();
//            $('#form1').disableAutoFill({
//                callback: function () {
//                    return true;
//                },
//            });
        });
    </script>
    <script type="text/javascript">
        $(window).on("load", function () {
            window.history.forward(1);
        });

        $(document).ready(function () {
            cartItems();
            $("#smspopup").hide();
            var mobileno = $("#<%=lblorderready.ClientID%>").text();
        var nothanks = $("#<%=hfnothanks.ClientID%>").val();
        //Mobile number description 
        if ($("#<%=lblorderready.ClientID%>").text() != "" && nothanks == "0") {
            $("#<%=lblorderreadytext.ClientID%>").text("SMS Order ready notification message will be sent to:");
            $("#<%=txtchangemobilenumber.ClientID%>").val($("#<%=lblorderready.ClientID%>").text());
        } else if (nothanks == "1") {
            $("#<%=lblorderready.ClientID%>").text('');
            $("#<%=lblorderreadytext.ClientID%>").text("SMS Order ready notification will NOT be sent.");
            $("#<%=txtchangemobilenumber.ClientID%>").val($("#<%=hfphonenumber.ClientID%>").val());
        }

        $('#<%=drpcountry.ClientID%>').on("change", function () {
            $("#<%=txtsadd.ClientID%>").val('');
            $("#<%=txtadd2.ClientID%>").val('');
            $("#<%=txttown.ClientID%>").val('');
            $("#<%=txtstate.ClientID%>").val('');
            $("#<%=drpstate.ClientID%>").val('');
            $("#<%=txtzip.ClientID%>").val('');

            $('#<%=drpcountry.ClientID%> option').map(function () {
                if ($(this).val() != $("#<%=drpcountry.ClientID%>").val()) return this;
            }).attr('selected', false);
            $('#<%=drpcountry.ClientID%> option').map(function () {
                if ($(this).val() == $("#<%=drpcountry.ClientID%>").val()) {
                    shippingautocomplete.setComponentRestrictions({ 'country': $(this).val() });
                    return this;
                }     
            }).attr('selected', 'true');
        });

        $('#<%=drpcountry_Bill.ClientID%>').on("change", function () {
            $("#<%=txtsadd_Bill.ClientID%>").val('');
            $("#<%=txtadd2_Bill.ClientID%>").val('');
            $("#<%=txttown_Bill.ClientID%>").val('');
            $("#<%=txtstate_Bill.ClientID%>").val('');
            $("#<%=drpstate_Bill.ClientID%>").val('');
            $("#<%=txtzip_bill.ClientID%>").val('');
            $('#<%=drpcountry_Bill.ClientID%> option').map(function () {
                if ($(this).val() != $("#<%=drpcountry_Bill.ClientID%>").val()) return this;
            }).attr('selected', false);
            $('#<%=drpcountry_Bill.ClientID%> option').map(function () {
                if ($(this).val() == $("#<%=drpcountry_Bill.ClientID%>").val()) {
                    billingautocomplete.setComponentRestrictions({ 'country': $(this).val() });
                    return this;
                }
        }).attr('selected', 'true');
        });
       
        //When shipping address checkbox is checked or unchecked
        $('#<%=ChkBillingAdd.ClientID%>').on("change", function () {
            if ($(this).prop("checked")) {
                $("#<%=L2DivBilling.ClientID%>").hide();
            } else {
                $("#<%=L2DivBilling.ClientID%>").show();
                $("#<%=txtsadd_Bill.ClientID%>").val('');
                $("#<%=txtadd2_Bill.ClientID%>").val('');
                $("#<%=txttown_Bill.ClientID%>").val('');
                $("#<%=txtstate_Bill.ClientID%>").val('');
                $("#<%=drpstate_Bill.ClientID%>").val('');
                $("#<%=txtzip_bill.ClientID%>").val('');
                $('#<%=drpcountry_Bill.ClientID%> option').map(function () {
                    if ($(this).val() != $("#<%=drpcountry.ClientID%> option:selected").val()) return this;
                }).attr('selected', false);
                $('#<%=drpcountry_Bill.ClientID%> option').map(function () {
                    if ($(this).val() == $("#<%=drpcountry.ClientID%> option:selected").val()) return this;
                }).attr('selected', 'true');
                if ($("#<%=drpcountry_Bill.ClientID%> option:selected").val() == "AU") {
                    $("#<%=drpstate_Bill.ClientID%>").show();
                    $("#<%=txtstate_Bill.ClientID%>").hide();
                } else {
                    $("#<%=txtstate_Bill.ClientID%>").show();
                    $("#<%=drpstate_Bill.ClientID%>").hide();
             
                }
            }
        });


        //When shipping method changed
        $("#<%=drpSM1.ClientID %>").on("change", function (event) {
            //       cartItems();
            showPrice();
            $("#smspopup").hide();
            if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Standard Shipping") {
                $("#<%=SCP.ClientID%>").hide();
                //         GetOptionValue();
            }
            else if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup") {
                if ($("#<%=lblorderready.ClientID%>").text().substring(0, 2) == "04" && $("#<%=lblorderready.ClientID%>").text().toString().trim().length == 10) {
                    $("#smspopup").show();
                } else if ($("#<%=hfnothanks.ClientID%>").val() == "1") {
                    $("#smspopup").show();
                }
                $("#<%=SCP.ClientID%>").show();
                $("#<%=SCP.ClientID%>").removeClass("modal fade lgn-orderinfoscp");
                $("#<%=SCP.ClientID%>").addClass("modal fade lgn-orderinfoscp in");
            }
            else {
                $("#<%=SCP.ClientID%>").hide();
            }
        });

        //Shipping method 
        if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup") {
            if ($("#<%=lblorderready.ClientID%>").text().substring(0, 2) == "04" && $("#<%=lblorderready.ClientID%>").text().toString().trim().length == 10) {
                $("#smspopup").show();
            } else if (nothanksvalue == "1") {
                $("#smspopup").show();
            }
        }

            // When Ship Country is Changed
            $("#<%=drpcountry.ClientID %>").on("change", function (event) {
                if ($("#<%=drpcountry.ClientID%> option:selected").val() == "AU") {
                    $("#<%=drpstate.ClientID%>").show();
                    $("#<%=txtstate.ClientID%>").hide();
                    //         checkIsNotEmpty($("#<%=drpstate.ClientID%>").attr("name"), $("#<%=drpstate.ClientID%>").val());
                } else {
                    $("#<%=txtstate.ClientID%>").show();
                    $("#<%=drpstate.ClientID%>").hide();
                    //         checkIsNotEmpty($("#<%=txtstate.ClientID%>").attr("name"), $("#<%=txtstate.ClientID%>").val());

                }
            });

            // When Bill Country is Changed
            $("#<%=drpcountry_Bill.ClientID %>").on("change", function (event) {
                if ($("#<%=drpcountry_Bill.ClientID%> option:selected").val() == "AU") {
                    $("#<%=drpstate_Bill.ClientID%>").show();
                    $("#<%=txtstate_Bill.ClientID%>").hide();
                } else {
                    $("#<%=txtstate_Bill.ClientID%>").show();
                    $("#<%=drpstate_Bill.ClientID%>").hide();
               }
            });

            //$("#ctl00_maincontent_drpcountry").autocomplete({
            //    source: function (request, response) {
            //        $.ajax({
            //            type: "POST",
            //            contentType: "application/json; charset=utf-8",
            //            url: "Express_Checkout.aspx/GetCountryList",
            //            data: "{'str':'" + document.getElementById('ctl00_maincontent_drpcountry').value + "'}",
            //            dataType: "json",
            //            success: function (data) {
            //                //console.log(data.d);
            //                response($.map(data.d, function (item) {
            //                    return {
            //                        label: item.split('/')[0],
            //                        val: item.split('/')[1]
            //                    }
            //                }));
            //            },
            //            error: function (result) {
            //                //alert("Error");
            //            }
            //        });
            //    },
            //    select: function (event, ui) {
            //        var str_esc = escape(ui.item.val);
            //        shippingautocomplete.setComponentRestrictions({ 'country': str_esc });
            //    }

            //});

            //$("#ctl00_maincontent_drpcountry_Bill").autocomplete({
            //    source: function (request, response) {
            //        $.ajax({
            //            type: "POST",
            //            contentType: "application/json; charset=utf-8",
            //            url: "Express_Checkout.aspx/GetCountryList",
            //            data: "{'str':'" + document.getElementById('ctl00_maincontent_drpcountry_Bill').value + "'}",
            //            dataType: "json",
            //            success: function (data) {
            //                //console.log(data.d);
            //                response($.map(data.d, function (item) {
            //                    return {
            //                        label: item.split('/')[0],
            //                        val: item.split('/')[1]
            //                    }
            //                }));
            //            },
            //            error: function (result) {
            //                //alert("Error");
            //            }
            //        });
            //    },
            //    select: function (event, ui) {
            //        var str_esc = escape(ui.item.val);
            //        billingautocomplete.setComponentRestrictions({ 'country': str_esc });
            //    }

            //});

           

   
    });

    </script>
    <div class="container">
        <div class="row hidden-xs hidden-sm">
            <div class="col-sm-20">
                <ul id="mainbredcrumb" class="breadcrumb">
                    <li><a href="/home.aspx">Home</a></li>
                    <li class="active">Checkout</li>
                </ul>
            </div>
        </div>


        <div id="PHOrderConfirm" runat="server" style="display: none">
            <div class="accordion_head_green white_color" id="pendingOrderText" style="display: none">
                <h3 style="font-size: 16px;">Order Now Pending Approval</h3>
                <p class="p2">
                    Your order is now pending approval form your company supervisor/s before it can be submitted to us for processing.
                    <br />
                    The following member/s in your company will be able to authorise and submite your order:
                    <br />
                    <asp:Label ID="lblUserRoleName" runat="server" Visible="true" Font-Names="Arial" Font-Size="11px" ForeColor="Black" Font-Bold="true" Text=""></asp:Label>
                </p>
            </div>

            <div class="accordion_head_green white_color clearfix" style="text-align: center; display: none" id="submitOrderText">


                <div class="col-xs-19" style="text-align: center">
                    <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/checkout_tick.png" class="mr15" alt="" />
                    Your order has been successfully submitted to us for processing. Thank You!
                </div>

            </div>

            <div class="accordion_head_yellow gray_40" style="background: #FFF200; padding: 12px 17px; text-align: center; display: none" id="InternationalOrderText">
                <strong>Please Note :</strong>
                As you are purchasing from outside of Australia, We will calculate shipping charges and advise you shortly by email with further details.
                    <br />
                <p>
                    Your Reference Order Id is
                <asp:Label ID="lblintorderid" runat="server" Text="" Font-Bold="true"></asp:Label>
                </p>
            </div>

            <div class="accordion_head_yellow gray_40" style="font-size:14px; background: #FFF200; padding: 12px 17px; text-align: center;display: none" id="PPPOrderText">
                <strong>Please Note :</strong>  We will calculate shipping charges and advise you shortly by email with further details.
                    <br />
                <p>Your Reference Order Id is
                    <asp:Label ID="lblPPPorderid" runat="server" Text="" Font-Bold="true"></asp:Label></p>
            </div>

        </div>

        <div runat="server" id="PayOkDiv" style="text-align: center">

            <div id="divOk" runat="server" class="accordion_head_green clear" style="text-align: center">
            </div>

        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="titleblock text_black mb10">
                    <h4 class="main_heading">Order Checkout</h4>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="" id="accordion" role="tablist" aria-multiselectable="true">

                <div class="">
                    <div class="row clearfix">


                        <div class="col-md-9 col-md-push-11 ml-15">
                            <div class="checkoutright ml20 mb30" id="checkoutrightL1" runat="server">
                                <div class="brb">
                                    <div class="pv5 ph10" style="background: #f0f0f0;">
                                        <h4 class="heading_2 inlineblk">Order Summary</h4>
                                        <asp:Label ID="lblppp" runat="server" Text="" style="float:right;color:grey"></asp:Label>
                                        <asp:Button ID="ImageButton1" runat="server" Text="Edit Cart" OnClick="ImageButton1_Click" class="smp_btn pull-right" />
                                    </div>
                                    <div id="orderitemlist">
                                    </div>
                                    <%--      <%
                       HelperServices objHelperServices = new HelperServices();
                       
                        OrderServices objOrderServices = new OrderServices();
                        ProductServices objProductServices = new ProductServices();
                        DataSet dsOItem = new DataSet();

                         int OrderID = 0;
                        int Userid;
                        int ProductId;
                        decimal subtot = 0.00M;
                        decimal taxamt = 0.00M;
                        decimal Total = 0.00M;

                        string SelProductId = "";
                        string OrdStatus = "";

                        int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                        UserServices objUserServices = new UserServices();

                        Userid = objHelperServices.CI(Session["USER_ID"]);
                        if (Userid <= 0)
                            Userid = objHelperServices.CI(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString());
                        if (!string.IsNullOrEmpty(Request["OrderID"]))
                        {
                            OrderID = Convert.ToInt32(Request["OrderID"].ToString());
                        }
                        else
                        {
                            OrderID = objOrderServices.GetOrderID(Userid, OpenOrdStatusID);
                        }

                        OrdStatus = objOrderServices.GetOrderStatus(OrderID);
                        ProductId = objHelperServices.CI(Request.QueryString["Pid"]);
                    %>
                        <%   	  
                            String sessionId_dum;
                            sessionId_dum = Session.SessionID;
                                             	     
                          
                            if (Userid != 999)
                            {
                                    dsOItem = objOrderServices.GetOrderItems(OrderID);
                            }
                            else
                            {
                                dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, "", sessionId_dum);
                            }

                            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                            oOrderInfo = objOrderServices.GetOrder(OrderID);

                            UserServices.UserInfo oOrdBillInfo1 = objUserServices.GetUserBillInfo(Userid);
                            UserServices.UserInfo oOrdShippInfo1 = objUserServices.GetUserShipInfo(Userid);


                            string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                            decimal ProdShippCost = 0.00M;
                            decimal TotalShipCost = 0.00M;
                            decimal ShippingValue = 0;
                            string catlogitem = "";
                            SelProductId = "";
                            if (OrdStatus == OrderServices.OrderStatus.OPEN.ToString() || OrdStatus == "CAU_PENDING")
                            {
                                if (dsOItem != null)
                                {
                                    int i = 0;
                                    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                                    {
                                        decimal ProductUnitPrice;
                                        int pid;
                                        decimal Amt = 0;

                                        pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                                        catlogitem = rItem["CATALOG_ITEM_NO"].ToString();
                                        ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                                        ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N2"));
                                        Amt = objHelperServices.CDEC(objHelperServices.CI(rItem["QTY"].ToString()) * ProductUnitPrice);

                                        int Qty = objHelperServices.CI(rItem["QTY"].ToString());
                                        decimal ProdTotal = Qty * ProductUnitPrice;
                                        subtot = subtot + ProdTotal;
                                      
                        %>
                        <div class="clearfix br_b pb15 pt25">
                        	<div class="col-xs-3">
                                <% GetImage(pid); %>

                                <asp:Image ID="lblProImage" class="img_h50" Style="max-width: 60px;" runat="server" alt="Loading..."  />
                            </div>
                            <div class="col-xs-17 pr0 ">
                            	<div class="dblock pb10"><b class="f14"><% Response.Write(rItem["DESCRIPTION"].ToString()); %></b></div>
                                <div class="os_itemdetail">
                                	<div class="col-xs-6 pl0"> <span class="dblock pb5">Qty:</span> <% Response.Write(rItem["QTY"].ToString()); %> </div>
                                    <div class="col-xs-7"> <span class="dblock pb5">Item Cost:</span><% Response.Write(CurSymbol + " " + ProductUnitPrice.ToString("#,#0.00")); %> Ex GST</div>
                                    <div class="col-xs-7 text-right"> <span class="dblock pb5">Sub-Total:</span> <% Response.Write(CurSymbol + " " + Amt.ToString("#,#0.00")); %> Ex GST</div>
                                </div>
                            </div>
                        </div>

                         <%  
                               i = i + 1;
                                       
                                        
                                    } 
                                  
                                }  
                            } 
                          
                        %>--%>

                                    <%--    <div class="clearfix br_b pb15 pt25">
                        	<div class="col-xs-16">
                        		<p class="mb0 mt10">
                                      <%  
                                          HelperServices objHelperServices = new HelperServices();

                                          OrderServices objOrderServices = new OrderServices();
                                          ProductServices objProductServices = new ProductServices();
                                          OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                                          string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                                          decimal ShippingValue = 0;
                                          decimal subtot = 0;
                                          if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                                          {
                                              Response.Write("Shipping Charge");
                                          }
                                          else
                                          {
                                              Response.Write("Delivery (Ex GST)");
                                          }
                                 %>
                        		</p>
                            </div>
                            <div class="col-xs-4 text-right ital">
                                        <%
                                        
                                            if (objHelperServices.GetOptionValues("COURIER CHARGE") != "" && (drpSM1.SelectedValue == "Standard Shipping" || lblShippingMethod.Text == "Standard Shipping"))


                                             ShippingValue = Convert.ToDecimal(objHelperServices.GetOptionValues("COURIER CHARGE").ToString());
                            
                        %>

                                <%
                                    if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                                    {

                                        Response.Write("To Be Advised");
                                        } 
                                    else
                                    {
                            
                        %>
                                   <span style="display:none;" id="shipping_charge_domestic_value">
                                     <asp:Label ID="lblshipcost" runat="server" Text="" style="display:none;"></asp:Label>
                                
                                 
                                      
                                <%  Response.Write(CurSymbol + " " + objHelperServices.GetOptionValues("COURIER CHARGE").ToString());
                                    lblshipcost.Text = objHelperServices.GetOptionValues("COURIER CHARGE").ToString();
                                    }   %>
                                  </span>
                                 <span style="display:none;" id="shipping_charge_domestic_zero">
                                     
                                      $ <asp:Label ID="Label1" runat="server" Text="0.00"></asp:Label>
                                 
                                 
                                           
                                       <% if (lblshipcost.Text != "")
                                          {  %>
                                     
                                       <% 
                                              oOrderInfo.ShipCost = Convert.ToDecimal( lblshipcost.Text);
                                              ShippingValue=Convert.ToDecimal( lblshipcost.Text);
                                          }   %>
                                 </span>
                            </div>
                        </div>--%>

                                    <div class="clearfix br_b pb15 pt25">
                                        <div class="col-xs-16">
                                            <p class="mb0 mt10">
                                                <asp:Label ID="lbDeliveryText" runat="server" Text=""></asp:Label>
                                            </p>
                                        </div>
                                        <div class="col-xs-4 text-right ital">
                                            <asp:Label ID="lbDelivery" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>

                                    <%--  <% } %>--%>
                                    <div class="clearfix br_b pb15 pt25">
                                        <div class="col-xs-16">
                                            Total Tax Amount (GST) 
                        		<p class="mb0 mt10">
                                    <asp:Label ID="lbEstimateAmountText" runat="server" Text="" Style="font-weight: bold"></asp:Label>
                                    <%--          <%
                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                        {                                        
                            %>                                        
                              <b>Est. Total </b> 
                            <%
                        }
                        else
                        {
                                %> <b>Est. Total Inc GST</b>
                       <%
                        } %>--%>
                                </p>
                                        </div>
                                        <div class="col-xs-4 text-right">
                                            $
                                            <asp:Label ID="taxAmount" runat="server" Text=""></asp:Label>
                                            <p class="mb0 mt10">
                                                $
                                                <asp:Label ID="totalAmount" runat="server" Text="" Style="font-weight: bold"></asp:Label>
                                            </p>
                                        </div>
                                        <%--  <div class="col-xs-4 text-right">
                                <strong id="Ictaxamount" style="display: none;">
                                    <%  Response.Write(CurSymbol + " 0.00");  %>
                                </strong>
                                  <span style="display:block;" id="TaxAmount">
                                       
                                <%       
                                  
                                    decimal ICtaxamt = 0.00M;
                                    if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                                    {
                                        Response.Write(CurSymbol + " " + ICtaxamt);
                                    }
                                    else
                                    {
                                        decimal totamtanor = subtot + ShippingValue;
                                        decimal taxa_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTaxa_nor = objHelperServices.CDEC((totamtanor) * (taxa_nor / 100));
                                        RetTaxa_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTaxa_nor));
                                        
                                        Response.Write(CurSymbol + " " + RetTaxa_nor);
                                    }
                                    
                                    %></span>

                                  <span style="display:none;" id="taxamtcoupickup">
                                   <%  decimal totamtTaxCP = subtot;
                                        decimal tax_CP = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_CP = objHelperServices.CDEC(totamtTaxCP * (tax_CP / 100));
                                        RetTax_CP = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_CP));
                                        Response.Write(CurSymbol + " " + RetTax_CP); 
                                    %>
                                  </span>
                                    <span style="display:none;" id="taxamtwithcouriercrge">
                                       <%  
                                           decimal totamtTax = ShippingValue + subtot;
                                        decimal tax_WC = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_WC = objHelperServices.CDEC(totamtTax * (tax_WC / 100));
                                        RetTax_WC = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_WC));
                                        Response.Write(CurSymbol + " " + RetTax_WC); 
                                    %></span>
                            	
                            	<p class="mb0 mt10">
                                    <strong id="ICtotalamount" style="display:none;">

                                       
                                      
                                  <% 
                                      Response.Write(CurSymbol + " " + oOrderInfo.ProdTotalPrice);  
                                    %>
                                </strong>
                                <strong id="totalamt" style="display:block;">
                                    <b>
                                    <%
                                        
                                        
                                        //decimal totnor = oOrderInfo.ProdTotalPrice;
                                        decimal totnor = subtot+ShippingValue;
                                        decimal tax_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_nor = objHelperServices.CDEC(totnor * (tax_nor / 100));
                                        RetTax_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
                                        
                                        decimal totamtnor = RetTax_nor + totnor;
                                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                                        {
                                            //Response.Write(CurSymbol + " " + oOrderInfo.ProdTotalPrice);
                                            Response.Write(CurSymbol + " " + subtot);
                                        }
                                        else
                                        {
                                            Response.Write(CurSymbol + " " + totamtnor);
                                        }
                                        %>
                                        </b>
                                </strong>
                                   <strong id="totalcoupickup" style="display:none;">
                                    <%
                                        
                                        //decimal totcp = oOrderInfo.ProdTotalPrice;
                                        decimal totcp = subtot;
                                        decimal tax_tcp = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_tcp = objHelperServices.CDEC(totcp * (tax_CP / 100));
                                        // totamtTax = RetTax_WC + ShippingValue + oOrderInfo.ProdTotalPrice;
                                        RetTax_tcp = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_tcp));


                                       // decimal totcoupu = RetTax_tcp + oOrderInfo.ProdTotalPrice;
                                        decimal totcoupu = RetTax_tcp + subtot;
                                        totcoupu = objHelperServices.CDEC(objHelperServices.FixDecPlace(totcoupu));
                                        Response.Write(CurSymbol + " " + totcoupu);    
                                        %>
                                </strong>
                                    <strong id="totamtwithcouriercrge" style="display:none;">

                                    <%
                                        
                                        decimal tocouamt = 0.00M; 
                                        //tocouamt = ShippingValue + oOrderInfo.ProdTotalPrice;
                                        tocouamt = ShippingValue + subtot;
                                        decimal tax = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax = objHelperServices.CDEC(tocouamt * (tax / 100));
                                       // tocouamt = RetTax + ShippingValue + oOrderInfo.ProdTotalPrice;
                                        tocouamt = RetTax + ShippingValue + subtot;
                                       tocouamt = objHelperServices.CDEC(objHelperServices.FixDecPlace(tocouamt));
                                       if (lblAmount.Text != "")
                                       {
                                           Response.Write(CurSymbol + " " + lblAmount.Text);
                                       }
                                       else
                                       {
                                           Response.Write(CurSymbol + " " + tocouamt);
                                       }   
                                        %>
                                </strong>

                            	</p>
                            </div>--%>
                                    </div>

                                </div>

                            </div>
                            <div class="checkoutright ml20 mb30" runat="server" id="checkoutrightL4">
                                <uc1:OrderDet ID="OrderDet1" runat="server" />
                            </div>
                        </div>

                        <div class="col-md-11 col-md-pull-9">

                            <div class="wrapleft pr40 mpr0 pb20" runat="server" id="Level1">
                                <div class="headingwrap active clearfix">
                                    <div class="no_circle">1</div>
                                    <h4 class="numaric_heading inlineblk">Lets Get Started</h4>
                                </div>
                                <div class="checkoutleft" id="letstartdivlogin" runat="server">
                                    <div id="getstartwelcome" runat="server">
                                        <p class="caption mb20 mt10">Welcome Back! Please sign in to continue check out.</p>
                                    </div>
                                    <form id="form1">
                                        <div class="form-group mb10 clearfix">
                                            <%--  <label class="col-md-5 pl0">Email<span class="required">*</span></label>--%>
                                            <%--<div class="col-md-15 mpl0">
                                      <asp:TextBox runat="server" ID="letstarttxtemail" Text="" class="form-control" MaxLength="55" autocomplete="off" onkeyup="javascript:keyboardup(this);" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                      <span id="lbrfletstarttxtemail" class="mandatory" style="display:none">Enter Email</span>
                                      <span id="lbreletstarttxtemail" class="mandatory" style="display:none">Enter Valid Email</span>
                                      <asp:HiddenField ID="rfletstarttxtemail" runat="server" />
                                     <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="Mandatory_Express1" Display="Dynamic" Text="Enter Email" ControlToValidate="letstarttxtemail" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                            <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="letstarttxtemail" ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory_Express1" ForeColor="Red"></asp:RegularExpressionValidator>-%>
                                  </div>--%>
                                            <div class="floating-form">
                                                <div class="floating-label">
                                                <%--<input style="display:none" type="text" name="fakeusernameremembered" />
                                                <input style="display:none" type="password" name="fakepasswordremembered" />--%>
                                                    <asp:TextBox runat="server" ID="letstarttxtemail" Text="" class="floating-input" MaxLength="55" placeholder=" " autocomplete="off" TextMode="search" onkeyup="javascript:keyboardup(this);" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                    <span class="highlight"></span>
                                                    <label class="fl">Email</label>
                                                    <span id="lbrfletstarttxtemail" class="mandatory" style="display: none">Enter Email</span>
                                                    <span id="lbreletstarttxtemail" class="mandatory" style="display: none">Enter Valid Email</span>
                                                     <span id="lbreptxtregemail" class="mandatory" style="display: none">Email Already Exists</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group mb30 clearfix" id="startdivpwd" runat="server">
                                            <%--<label class="col-md-5 pl0">Password<span class="required">*</span></label>
                                  <div class="col-md-15 mpl0">
                                      <asp:TextBox runat="server" ID="letstarttxtpwd" Text="" class="form-control" TextMode="Password" MaxLength="55" onkeyup="javascript:keyboardup(this);" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                      <span id="lbrfletstarttxtpwd" class="mandatory" style="display:none">Enter Password</span>
                                    <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="Mandatory_Express2" Display="Dynamic" Text="Enter Password" ControlToValidate="letstarttxtpwd" ForeColor="Red"></asp:RequiredFieldValidator>-%>
                                      <a class="fplink pull-right" target="_blank" href="ForgotPassword.aspx">Forgot Password</a>
                                      <asp:Label ID="lblErrMsg" runat="server" Text="" Class="mandatory"></asp:Label>
                                  </div>--%>
                                            <div class="floating-form">
                                                <div class="floating-label">
                                                    <asp:TextBox runat="server" ID="letstarttxtpwd" Text="" TextMode="Password" class="floating-input" MaxLength="55" placeholder=" " autocomplete="off"  onkeyup="javascript:onBlurValidate(this);"></asp:TextBox>
                                                    <span class="highlight"></span>
                                                    <label class="fl">Password</label>
                                                    <span id="lbrfletstarttxtpwd" class="mandatory" style="display: none">Enter Password</span>
                                                    <a class="fplink pull-right" target="_blank" href="ForgotPassword.aspx">Forgot Password</a>
                                                    <asp:Label ID="lblErrMsg" runat="server" Text="" Class="mandatory"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                        <p class="text-center">
                                            <asp:Button runat="server" ID="BtnGetStartContinue" Text="Continue" class="ph30 btn primary-btn-green f16" ValidationGroup="Mandatory_Express1" OnClientClick="return BtnGetStartContinueClick();" CausesValidation="true" UseSubmitBehavior="false" Visible="true" />
                                            <asp:Button runat="server" ID="BtnGetStartLogin" Text="Sign In" class="ph30 btn primary-btn-green f16" OnClientClick="return BtnGetStartLoginClick();" CausesValidation="true" ValidationGroup="Mandatory_Express2" UseSubmitBehavior="false" Visible="true" />
                                        </p>
                                    </form>
                                 
                                </div>
                                <div class="checkoutleft" id="letstartregister" runat="server">
                                    <p class="caption mb20">Please enter details below to continue check out</p>
                                   
                                        <div class="form-group mb10 clearfix">
                                            <%-- <label class="col-md-7 pl0">Email<span class="required">*</span></label>
                                  <div class="col-md-13 mpl0">
                                      <asp:TextBox runat="server" ID="txtregemail" Text="" class="form-control" MaxLength="55" onkeyup="javascript:keyboardup(this);" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                       <span id="lbrftxtregemail" class="mandatory" style="display:none">Enter Email</span>
                                       <span id="lbretxtregemail" class="mandatory" style="display:none">Enter Valid Email</span>
                                     <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="Mandatory_Express31_New" Display="Dynamic" Text="Enter Email" ControlToValidate="txtregemail" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                            <%--  <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtregemail" ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory_Express31_New" ForeColor="Red"></asp:RegularExpressionValidator>-%>
                                  </div>--%>
                                            <div class="floating-form">
                                                <div class="floating-label">
                                                    <asp:TextBox runat="server" ID="txtregemail" Text="" class="floating-input" MaxLength="55" placeholder=" "  TextMode="search" onkeyup="javascript:keyboardup(this);" autocomplete="off" onblur="return CheckEmailAlreadyExists()"></asp:TextBox>
                                                    <span class="highlight"></span>
                                                    <label class="fl">Email</label>
                                                    <span id="lbrftxtregemail" class="mandatory" style="display: none">Enter Email</span>
                                                    <span id="lbretxtregemail" class="mandatory" style="display: none">Enter Valid Email</span>
                                                   
                                                </div>
                                            </div>
                                        </div>

                                        <hr class="mv15">

                                        <div class="form-group mb10 clearfix">
                                            <%--<label class="col-md-7 pl0">First Name<span class="required">*</span></label>
                                  <div class="col-md-13 mpl0">
                                      <asp:TextBox runat="server" ID="txtRegFname" Text="" class="form-control" MaxLength="50" onkeyup="javascript:keyboardup(this);" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                       <span id="lbrftxtRegFname" class="mandatory" style="display:none">Enter First Name</span>
                                  </div>--%>
                                            <div class="floating-form">
                                                <div class="floating-label">
                                                    <asp:TextBox runat="server" ID="txtRegFname" Text="" class="floating-input" MaxLength="50" placeholder=" "  TextMode="search" onkeyup="javascript:keyboardup(this);" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                    <span class="highlight"></span>
                                                    <label class="fl">First Name</label>
                                                    <span id="lbrftxtRegFname" class="mandatory" style="display: none">Enter First Name</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group mb10 clearfix">
                                            <%--<label class="col-md-7 pl0">Last Name<span class="required">*</span></label>
                                  <div class="col-md-13 mpl0">
                                      <asp:TextBox runat="server" ID="txtRegLname" Text="" class="form-control" MaxLength="30" onkeyup="javascript:keyboardup(this);" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                       <span id="lbrftxtRegLname" class="mandatory" style="display:none">Enter Last Name</span>
                                  </div>--%>
                                            <div class="floating-form">
                                                <div class="floating-label">
                                                    <asp:TextBox runat="server" ID="txtRegLname" Text="" class="floating-input" MaxLength="30" placeholder=" "  TextMode="search" autocomplete="off" onkeyup="javascript:keyboardup(this);" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                    <span class="highlight"></span>
                                                    <label class="fl">Last Name</label>
                                                    <span id="lbrftxtRegLname" class="mandatory" style="display: none">Enter Last Name</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group mb10 clearfix">
                                            <%-- <label class="col-md-7 pl0">Phone Number<span class="required">*</span></label>
                                  <div class="col-md-13 mpl0">
                                      <asp:TextBox runat="server" ID="txtRegphone" Text="" class="form-control" MaxLength="16" onkeyup="javascript:keyboardup(this);" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                       <span id="lbrftxtRegphone" class="mandatory" style="display:none">Enter Phone</span>
                                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtRegphone" />
                                  </div>--%>
                                            <div class="floating-form">
                                                <div class="floating-label">
                                                    <asp:TextBox runat="server" ID="txtRegphone" Text="" class="floating-input" placeholder=" "  TextMode="search" MaxLength="16" onkeyup="javascript:keyboardup(this);" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                    <span class="highlight"></span>
                                                    <label class="fl">Phone Number</label>
                                                    <span id="lbrftxtRegphone" class="mandatory" style="display: none">Enter Phone</span>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtRegphone" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-group mb30 clearfix">
                                            <%--<label class="col-md-7 pl0">Mobile Phone<span class="required"></span></label>
                                  <div class="col-md-13 mpl0">
                                      <asp:TextBox ID="txtRegMobilePhone" CssClass="form-control" runat="server" MaxLength="10" autocomplete="off" onblur="javascript:onBlurValidate1(this);"></asp:TextBox>
                                       <span id="lbretxtRegMobilePhone" class="mandatory" style="display:none">Mobile No. must start with 04 and must be 10 digit</span>
                                      <asp:FilteredTextBoxExtender ID="feMobilePhone2" runat="server" FilterMode="ValidChars"
                                          ValidChars="1234567890" TargetControlID="txtRegMobilePhone" />
                                  </div>--%>
                                            <div class="floating-form">
                                                <div class="floating-label">
                                                    <asp:TextBox ID="txtRegMobilePhone" CssClass="floating-input" runat="server" MaxLength="10" placeholder=" "  TextMode="search" autocomplete="off" onblur="javascript:onBlurValidate1(this);"></asp:TextBox>
                                                    <span class="highlight"></span>
                                                    <label class="fl">Mobile Phone</label>
                                                    <span id="lbretxtRegMobilePhone" class="mandatory" style="display: none">Mobile No. must start with 04 and must be 10 digit</span>
                                                    <asp:FilteredTextBoxExtender ID="feMobilePhone2" runat="server" FilterMode="ValidChars"
                                                        ValidChars="1234567890" TargetControlID="txtRegMobilePhone" />
                                                </div>
                                            </div>
                                        </div>

                                        <hr class="mv15">

                                        <div class="form-group mb30 clearfix">
                                            <%--<label class="col-md-7 pl0">Password<span class="required">*</span></label>
                                  <div class="col-md-13 mpl0">
                                      <asp:TextBox runat="server" ID="txtRegpassword" Text="" class="form-control" TextMode="Password" MaxLength="15" onkeydown="CheckTextPassMaxLength(this,event,'15');" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                       <span id="lbrftxtRegpassword" class="mandatory" style="display:none">Enter Password</span>
                                      <span id="lbretxtRegpassword" class="mandatory" style="display:none">Password must contain letters and numbers. Length needs to be between 6 and 15 characters.</span>
                                  </div>--%>
                                            <div class="floating-form">
                                                <div class="floating-label">
                                                    <asp:TextBox runat="server" ID="txtRegpassword" Text="" class="floating-input" TextMode="Password" placeholder=" " MaxLength="15" onkeydown="CheckTextPassMaxLength(this,event,'15');" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                    <span class="highlight"></span>
                                                    <label class="fl">Password</label>
                                                    <span id="lbrftxtRegpassword" class="mandatory" style="display: none">Enter Password</span>
                                                    <span id="lbretxtRegpassword" class="mandatory" style="display: none">Password must contain letters and numbers. Length needs to be between 6 and 15 characters.</span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-group mb30 clearfix">
                                            <%-- <label class="col-md-7 pl0">Confirm Password<span class="required">*</span></label>
                                  <div class="col-md-13 mpl0">
                                      <asp:TextBox runat="server" ID="txtRegConfirmPassword" Text="" class="form-control" TextMode="Password" autocomplete="off"
                                          MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                       <span id="lbrftxtRegConfirmPassword" class="mandatory" style="display:none">Enter Confirm Password</span>
                                       <span id="lbretxtRegConfirmPassword" class="mandatory" style="display:none">Confirm Password and Password should be same</span>
                                  </div>--%>
                                            <div class="floating-form">
                                                <div class="floating-label">
                                                    <asp:TextBox runat="server" ID="txtRegConfirmPassword" Text="" class="floating-input" TextMode="Password" placeholder=" " autocomplete="off"
                                                        MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                    <span class="highlight"></span>
                                                    <label class="fl">Confirm Password</label>
                                                    <span id="lbrftxtRegConfirmPassword" class="mandatory" style="display: none">Enter Confirm Password</span>
                                                    <span id="lbretxtRegConfirmPassword" class="mandatory" style="display: none">Confirm Password and Password should be same</span>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Label ID="lblerror_l1update" runat="server" Text="" Class="mandatory"></asp:Label>
                                        <p class="text-center">
                                            <asp:Button runat="server" ID="BtnRegLetStart" Text="Continue" class="ph20 btn primary-btn-green" ValidationGroup="Mandatory_Express31_New" OnClientClick="return BtnRegLetStartClick();" UseSubmitBehavior="false" Visible="true" />
                                            <asp:Button runat="server" ID="BtnRegLetStart_Edit" Text="Continue" class="ph20 btn primary-btn-green" ValidationGroup="Mandatory_Express31_New" OnClientClick="return BtnRegLetStartUpdateClick();" UseSubmitBehavior="false" Visible="true" />
                                        </p>
                                  
                                </div>
                                <div class="headingwrap clearfix mt20">
                                    <div class="no_circle">2</div>
                                    <h4 class="numaric_heading inlineblk">Shipping Address</h4>
                                </div>

                                <div class="headingwrap clearfix mt20">
                                    <div class="no_circle">3</div>
                                    <h4 class="numaric_heading inlineblk">Shipping Method</h4>
                                </div>

                                <div class="headingwrap clearfix mt20 mb20">
                                    <div class="no_circle">4</div>
                                    <h4 class="numaric_heading inlineblk">Payment</h4>
                                </div>

                            </div>


                            <div id="Level2" class="wrapleft pr40 mpr0 pb20" runat="server">
                                <div class="headingwrap visited clearfix">
                                    <div class="no_circle">1</div>
                                    <h4 class="numaric_heading inlineblk">Contact Details</h4>
                                    <asp:Button ID="btnl2Editlogin" runat="server" Text="Edit" class="smp_btn pull-right" OnClientClick="BtnEditLoginClick();return true;" UseSubmitBehavior="false" />
                                </div>

                                <div class="form-group row p15 text-left clearfix">
                                    <div class="col-sm-20 pv15 br_dark">
                                        <p class="mb0">
                                            <b>Name  : </b>
                                            <asp:Label runat="server" ID="L2name" />
                                        </p>
                                        <p class="mb0">
                                            <b>Email : </b>
                                            <asp:Label runat="server" ID="L2Email" />
                                        </p>
                                        <p class="mb0">
                                            <b>Phone : </b>
                                            <asp:Label runat="server" ID="L2Phone" />
                                        </p>
                                        <p class="mb0" id="P2Mobile" runat="server"><b>Mobile : </b>
                                            <asp:Label runat="server" ID="L2Mobile" /></p>
                                    </div>
                                </div>
                                <div id="l2div" runat="server"></div>
                                <div class="headingwrap active clearfix mt20">
                                    <div class="no_circle">2</div>
                                    <h4 class="numaric_heading inlineblk">Shipping &amp; Billing Address</h4>
                                </div>

                                <div class="checkoutleft">
                                    <form id="formlevel2">
                                        <div class="cmn_wrap">
                                            <h2 class="text-center sub_heading2">Shipping Address</h2>
                                            <div runat="server">
                                              <%--  <div class="form-group mb10 clearfix">
                                                    <a id="shipAddressBook" href="#" class="modal-toggle">
                                                        <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/address_icon.png" style="float:right" /></a>
                                                    <asp:HiddenField ID="hfShipAddress" runat="server" Value="0"></asp:HiddenField>
                                                </div>--%>
                                                <div class="form-group mb0 clearfix">
                                                    <%--<label class="col-md-8 pl0">Bussiness Name</label>
                                          <div class="col-md-12 mpl0">
                                              <asp:TextBox runat="server" ID="txtComname" Text="" class="form-control" MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                          </div>--%>
                                                    <div class="floating-form" style="width: 100%;">
                                                        <div class="floating-label mb10">
                                                            <asp:TextBox runat="server" ID="txtComname" Text="" class="floating-input" MaxLength="30" placeholder=" " onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                                            <span class="highlight"></span>
                                                            <label class="fl">Bussiness Name</label>
                                                        </div>
                                                    </div>
                                                    <label style="font-size: 12px; font-weight: lighter;" class="mb15">A business name is required if your order is being delivered to a non-residential address.</label>
                                                </div>
                                                <div class="form-group mb5 clearfix">
                                                    <%--<label class="col-md-8 pl0">Receivers Name</label>
                                          <div class="col-md-12 mpl0">
                                              <asp:TextBox runat="server" ID="txt_attnto" Text="" class="form-control" MaxLength="250"></asp:TextBox>
                                          </div>--%>
                                                    <div class="floating-form" style="width: 100%;">
                                                        <div class="floating-label">
                                                            <asp:TextBox runat="server" ID="txt_attnto" Text="" class="floating-input"  TextMode="search" placeholder=" " MaxLength="250"></asp:TextBox>
                                                            <span class="highlight"></span>
                                                            <label class="fl">Receivers Name</label>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-group mb0 clearfix">
                                                <%--<label class="col-md-8 pl0">Street Address<span class="required">*</span></label>
                                      <div class="col-md-12 mpl0">
                                          <asp:TextBox runat="server" ID="txtsadd" Text="" class="form-control" MaxLength="30" autocomplete="off" onkeyup="javascript:keyboardup(this);"  onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                          <span id="lbrftxtsadd" class="vldRequiredSkin" style="display:none">Enter Street Address</span>
                                      </div>--%>
                                                <div class="floating-form" style="width: 100%;">
                                                    <div class="floating-label">
                                                         <asp:TextBox runat="server" ID="txtsadd" Text="" onfocus= "geolocate()" class="floating-input" placeholder=" "  TextMode="search" MaxLength="100" autocomplete="off" onkeyup="javascript:keyboardup(this);"  onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Street Address</label>
                                                        <span id="lbrftxtsadd" class="vldRequiredSkin" style="display:none">Enter Street Address</span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group mb5 clearfix">
                                                <%--<label class="col-md-8 pl0">Address Line 2</label>
                                      <div class="col-md-12 mpl0">
                                          <asp:TextBox runat="server" ID="txtadd2" Text="" class="form-control" MaxLength="30" autocomplete="off" onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
                                      </div>--%>
                                                <div class="floating-form mt0" style="width: 100%;">
                                                    <div class="floating-label">
                                                         <asp:TextBox runat="server" ID="txtadd2" Text="" class="floating-input" MaxLength="30" placeholder=" "  TextMode="search" autocomplete="off" onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Address Line 2</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group mb20 clearfix">
                                                <div class="floating-form mb0" style="width: 49%; float: left;">
                                                    <div class="floating-label mb0">
                                                       <asp:TextBox runat="server" ID="txttown" Text="" class="floating-input" MaxLength="30" placeholder=" "  TextMode="search" autocomplete="off" onkeyup="javascript:keyboardup(this);"  onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Suburb / Town</label>
                                                         <span id="lbrftxttown" class="vldRequiredSkin" style="display:none">Enter Suburb/Town</span>
                                                    </div>
                                                </div>

                                                <div class="floating-form mb0" style="width: 49%; float: right;">
                                                   <div class="floating-label mb0">
                                                         <asp:TextBox runat="server" ID="txtzip" Text="" class="floating-input" placeholder=" " MaxLength="10"  TextMode="search" onkeyup="javascript:keyboardup(this);" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Postal Zipcode</label>
                                                         <span id="lbrftxtzip" class="vldRequiredSkin" style="display:none">Enter Post/Zip Code</span>
                                                       <asp:FilteredTextBoxExtender ID="ftezip" runat="server" FilterMode="ValidChars" ValidChars="1234567890" TargetControlID="txtzip" />
                                                    </div>
                                                </div>

                                               <%-- <div class="floating-form rgt" id="intercust" runat="server" >
                                                    <div class="floating-label">
                                                       <asp:TextBox runat="server" ID="txtzip_inter" Text="" class="floating-input" placeholder=" "  TextMode="search" MaxLength="10" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Postal Zipcode</label>
                                                         <span id="lbrftxtzip_inter" class="vldRequiredSkin" style="display:none">Enter Post/Zip Code</span>
                                                    </div>
                                                </div>--%>

                                            </div>
                                           <%-- <div class="form-group mb10 clearfix">
                                                <div class="floating-form">
                                                    <div class="floating-label">
                                                        <asp:TextBox runat="server" ID="txtstate" Text="" CssClass="floating-input" MaxLength="20" placeholder=" " style="display:none" autocomplete="off"></asp:TextBox>
                                                         <asp:DropDownList ID="drpstate1" runat="server" class="floating-input" placeholder=" " Visible="true"></asp:DropDownList>
                                                        <span class="highlight"></span>
                                                        <label class="fl">State Province</label>
                                                        <span id="lbrftxtstate" class="vldRequiredSkin" style="display:none">Enter State/Province</span>
                                                        <span id="lbrfdrpstate1" class="vldRequiredSkin" style="display:none">Select State/Province</span>
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <%--<div class="form-group mb10 clearfix" id="aucust" runat="server">
                                     
                                                <div class="floating-form">
                                                    <div class="floating-label">
                                                         <asp:TextBox runat="server" ID="txtzip" Text="" class="floating-input" placeholder=" " MaxLength="10" onkeyup="javascript:keyboardup(this);" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Postal Zipcode</label>
                                                         <span id="lbrftxtzip" class="vldRequiredSkin" style="display:none">Enter Post/Zip Code</span>
                                                       <asp:FilteredTextBoxExtender ID="ftezip" runat="server" FilterMode="ValidChars" ValidChars="1234567890" TargetControlID="txtzip" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group mb10 clearfix" id="intercust" runat="server">
                                     
                                                <div class="floating-form">
                                                    <div class="floating-label">
                                                       <asp:TextBox runat="server" ID="txtzip_inter" Text="" class="floating-input" placeholder=" " MaxLength="10" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Postal Zipcode</label>
                                                         <span id="lbrftxtzip_inter" class="vldRequiredSkin" style="display:none">Enter Post/Zip Code</span>
                                                    </div>
                                                </div>

                                            </div>--%>

                                            <div class="form-group mb20 clearfix">

                                                <div class="floating-form mb0 mt0" style="width: 49%; float: left;">
                                                    <div class="floating-label mb0">
                                                        <asp:TextBox runat="server" ID="txtstate" Text="" CssClass="floating-input" MaxLength="20" TextMode="search" placeholder=" " onkeyup="javascript:keyboardup(this);" onblur="javascript:onBlurValidate1(this);" autocomplete="off"></asp:TextBox>
                                                         <asp:DropDownList ID="drpstate" runat="server" class="floating-input" placeholder=" "  onblur="javascript:onBlurValidate1(this);" style="display:none" Visible="true"></asp:DropDownList>
                                                        <span class="highlight"></span>
                                                        <label class="fl">State Province</label>
                                                        <span id="lbrftxtstate" class="vldRequiredSkin" style="display:none">Enter State/Province</span>
                                                        <span id="lbrfdrpstate" class="vldRequiredSkin" style="display:none">Select State/Province</span>
                                                    </div>
                                                </div>
                                  
                                                <div class="floating-form mb0 mt0" style="width: 49%; float: right;">
                                                    <div class="floating-label mb0">
                                                      <%--  <asp:TextBox runat="server" ID="txtcountry" Text="" CssClass="floating-input" MaxLength="20" TextMode="search" placeholder=" " onkeyup="javascript:keyboardup(this);" onblur="javascript:onBlurValidate(this);" autocomplete="off"></asp:TextBox>--%>
                                                        <asp:DropDownList ID="drpcountry" runat="server" class="floating-input" placeholder=" "> </asp:DropDownList>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Country</label>
                                                         <span id="lbrfdrpcountry" class="vldRequiredSkin" style="display: none">Enter Country</span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group mb5 clearfix">
                                                <%-- <label class="col-md-8 pl0">Delivery Instructions</label>
                                      <div class="col-md-12 mpl0">
                                          <asp:TextBox runat="server" ID="txtDELIVERYINST" Text="" class="form-control" MaxLength="30"></asp:TextBox>
                                      </div>--%>
                                                <div class="floating-form" style="width: 100%;">
                                                    <div class="floating-label">
                                                        <asp:TextBox runat="server" ID="txtDELIVERYINST" Text="" TextMode="search" class="floating-input" MaxLength="30" placeholder=" "></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Delivery Instructions</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>



                                        <div class="cmn_wrap" id="L2DivBilling" runat="server">
                                            <h2 class="text-center sub_heading2">Billing Address</h2>
                                           <%-- <div class="form-group mb10 clearfix">
                                                    <a id="A1" href="#" class="modal-toggle">
                                                        <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/address_icon.png" style="float:right"/></a>
                                                    <asp:HiddenField ID="hfBillAddress" runat="server" Value="0"></asp:HiddenField>
                                                </div>--%>
                                            <div class="form-group mb5 clearfix">
                                                <div class="floating-form" style="width: 100%;">
                                                    <div class="floating-label">
                                                        <asp:TextBox runat="server" ID="txtbillbusname" Text="" class="floating-input" MaxLength="30" TextMode="search" placeholder=" " onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Bussiness Name</label>
                                                    </div>
                                                </div>
                                                <label style="font-size: 12px; font-weight: lighter;" class="mb15">A business name is required if your order is being billed to a non-residential address.</label>
                                            </div>
                                            <div class="form-group mb5 clearfix">
                                                <div class="floating-form" style="width: 100%;">
                                                    <div class="floating-label">
                                                        <asp:TextBox runat="server" ID="txtbillname" Text="" class="floating-input" placeholder=" " TextMode="search" MaxLength="250"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Bill Name</label>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="form-group mb0 clearfix">
                                                <div class="floating-form" style="width: 100%;">
                                                    <div class="floating-label">
                                                        <asp:TextBox runat="server" ID="txtsadd_Bill" Text="" class="floating-input"  onfocus= "geolocate_bill()" placeholder=" " TextMode="search" MaxLength="30" autocomplete="off" onkeyup="javascript:keyboardup(this);" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Street Address</label>
                                                        <span id="lbrftxtsadd_Bill" class="vldRequiredSkin" style="display: none">Enter Street Address</span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group mb5 clearfix">
                                                <div class="floating-form mt0" style="width: 100%;">
                                                    <div class="floating-label">
                                                        <asp:TextBox runat="server" ID="txtadd2_Bill" Text="" class="floating-input" autocomplete="off" TextMode="search" placeholder=" " MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Address Line 2</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group mb20 clearfix">
                                                <div class="floating-form mb0" style="width: 49%; float: left;">
                                                    <div class="floating-label mb0">
                                                        <asp:TextBox runat="server" ID="txttown_Bill" Text="" class="floating-input" MaxLength="30" TextMode="search" placeholder=" " autocomplete="off" onkeyup="javascript:keyboardup(this);" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Suburb / Town</label>
                                                        <span id="lbrftxttown_Bill" class="vldRequiredSkin" style="display: none">Enter Suburb/Town</span>
                                                    </div>
                                                </div>

                                                <div class="floating-form mb0" style="width: 49%; float: right;">
                                                    <div class="floating-label mb0">
                                                        <asp:TextBox runat="server" ID="txtzip_bill" Text="" class="floating-input" MaxLength="10" TextMode="search" placeholder=" " onkeyup="javascript:keyboardup(this);" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Postal Zipcode</label>
                                                        <span id="lbrftxtzip_bill" class="vldRequiredSkin" style="display: none">Enter Post/Zip Code</span>
                                                    </div>
                                                </div>

                                            </div>
                                           <%-- <div class="form-group mb10 clearfix">
                                                 <div class="floating-form">
                                            <div class="floating-label">      
                                               <asp:DropDownList ID="drpstate2" runat="server" class="floating-input" placeholder=" " Visible="true"></asp:DropDownList>
                                                 <asp:TextBox runat="server" ID="txtstate_Bill" Text="" CssClass="floating-input" MaxLength="20" autocomplete="off" placeholder=" " Style="display: none" Visible="true"></asp:TextBox>
                                              <span class="highlight"></span>
                                              <label class="fl">State Province</label>
                                              <span id="lbrfdrpstate2" class="vldRequiredSkin" style="display: none">Select State/Province</span>
                                              <span id="lbrftxtstate_Bill" class="vldRequiredSkin" style="display: none">Enter State/Province</span>
                                            </div>
                                           </div>
                                            </div>--%>
                                           <%-- <div class="form-group mb10 clearfix" id="aucust_bill" runat="server">
                                                 <div class="floating-form">
                                            <div class="floating-label">      
                                                <asp:TextBox runat="server" ID="txtzip_bill" Text="" class="floating-input" MaxLength="10" placeholder=" " onkeyup="javascript:keyboardup(this);" autocomplete="off" onblur="javascript:onBlurValidate(this);"></asp:TextBox>
                                              <span class="highlight"></span>
                                              <label class="fl">Postal Zipcode</label>
                                              <span id="lbrftxtzip_bill" class="vldRequiredSkin" style="display: none">Enter Post/Zip Code</span>
                                            </div>
                                           </div>
                                            </div>--%>
                                            <div class="form-group mb20 clearfix">
                                                <div class="floating-form mb0 mt0" style="width: 49%; float: left;">
                                                    <div class="floating-label mb0">
                                                        <asp:TextBox runat="server" ID="txtstate_Bill" Text="" CssClass="floating-input" TextMode="search" MaxLength="20" autocomplete="off" onkeyup="javascript:keyboardup(this);" onblur="javascript:onBlurValidate1(this);" placeholder=" "></asp:TextBox>
                                                        <asp:DropDownList ID="drpstate_Bill" runat="server" class="floating-input" placeholder=" " Visible="true" onblur="javascript:onBlurValidate1(this);" style="display:none"></asp:DropDownList>
                                                        <span class="highlight"></span>
                                                        <label class="fl">State Province</label>
                                                        <span id="lbrftxtstate_Bill" class="vldRequiredSkin" style="display: none">Enter State/Province</span>
                                                        <span id="lbrfdrpstate_Bill" class="vldRequiredSkin" style="display: none">Select State/Province</span>
                                                    </div>
                                                </div>
                                                <div class="floating-form mb0 mt0" style="width: 49%; float: right;">
                                                    <div class="floating-label mb0">
                                                     <%--  <asp:TextBox runat="server" ID="txtcountry_Bill" Text="" CssClass="floating-input" TextMode="search" MaxLength="20" autocomplete="off" onkeyup="javascript:keyboardup(this);" onblur="javascript:onBlurValidate(this);" placeholder=" "></asp:TextBox>--%>
                                                          <asp:DropDownList ID="drpcountry_Bill" runat="server" class="floating-input" placeholder=" "> </asp:DropDownList>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Country</label>
                                                         <span id="lbrfdrpcountry_Bill" class="vldRequiredSkin" style="display: none">Enter Country</span>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <asp:FilteredTextBoxExtender ID="ftezip_bill" runat="server" FilterMode="ValidChars" ValidChars="1234567890" TargetControlID="txtzip_bill" />
                                        <div class="form-group col-lg-20 center-block text-center">
                                            <label class="mf12 mmt15">
                                                Billing address same as shipping address</label>
                                            <asp:CheckBox ID="ChkBillingAdd" runat="server" Visible="true"
                                                Class="CheckBoxSkin" Checked="true" />
                                        </div>

                                        <p class="text-center">
                                            <asp:Button runat="server" ID="BtnL2Continue" Text="Continue" class="ph30 f16 btn primary-btn-green" ValidationGroup="Mandatory_Express_Level2" OnClientClick="return BtnL2ContinueClick();" UseSubmitBehavior="false" />
                                        </p>

                                    </form>
                                </div>

                                <div id="address_book" class="modal1 address_book">
                                    <div class="modal-overlay modal-toggle"></div>
                                    <div class="modal-wrapper modal-transition">
                                        <div class="modal-header">
                                            <a href="#" class="modal-close modal-toggle">
                                                <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/close_popup.png" class="close-icon icon" />
                                            </a>
                                            <h2 class="modal-heading">Select Your Address</h2>
                                        </div>

                                        <div class="modal-body" style="padding: 0px;">
                                            <div class="modal-content">
                                                <fieldset class="dblock" id="radio_group">
                                                    <label class="address_label" for="address1">Address 1 </label>

                                                    <input type="radio" name="radio_group" id="address1" class="address_radio" />

                                                    <textarea class="dblock"></textarea>

                                                    <label class="address_label" for="address2">Address 2</label>

                                                    <input type="radio" name="radio_group" id="address2" class="address_radio" />

                                                    <textarea class="dblock"></textarea>

                                                    <label class="address_label" for="address3">Address 3</label>

                                                    <input type="radio" name="radio_group" id="Radio1" class="address_radio" />

                                                    <textarea class="dblock"></textarea>
                                                </fieldset>
                                                <div class="dblock">
                                                </div>
                                                <div class="dblock">
                                                </div>

                                                <button id="updateBtn" class="modal-toggle address_update ph30 f16 btn primary-btn-green">Update</button>
                                                <div class="clear"></div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="headingwrap clearfix mt20">
                                    <div class="no_circle">3</div>
                                    <h4 class="numaric_heading inlineblk">Shipping Method</h4>
                                </div>

                                <div class="headingwrap clearfix mt20 mb20">
                                    <div class="no_circle">4</div>
                                    <h4 class="numaric_heading inlineblk">Payment</h4>
                                </div>
                            </div>


                            <div class="wrapleft pr40 mpr0 pb20" id="Level3" runat="server">

                                <div class="clearfix">
                                    <div class="headingwrap visited clearfix">
                                        <div class="no_circle">1</div>
                                        <h4 class="numaric_heading inlineblk" id="hl1" runat="server">Contact Details</h4>

                                        <asp:Button ID="btnl3editlogin" runat="server" Text="Edit" class="smp_btn pull-right" OnClientClick="BtnEditLoginClick();return true;" UseSubmitBehavior="false" />
                                    </div>
                                    <div class="form-group row p15 text-left clearfix">
                                        <div class="col-sm-20 pv15 br_dark">
                                            <p class="mb0">
                                                <b>Name : </b>
                                                <asp:Label runat="server" ID="L3Name" />
                                            </p>
                                            <p class="mb0">
                                                <b>Email : </b>
                                                <asp:Label runat="server" ID="L3Email" />
                                            </p>
                                            <p class="mb0">
                                                <b>Phone : </b>
                                                <asp:Label runat="server" ID="L3Phone" />
                                            </p>
                                            <p class="mb0" id="P3Mobile" runat="server"><b>Mobile : </b>
                                                <asp:Label runat="server" ID="L3Mobile" /></p>
                                        </div>
                                    </div>
                                </div>



                                <div class="clearfix">
                                    <div class="headingwrap visited clearfix">
                                        <div class="no_circle">2</div>
                                        <h4 class="numaric_heading inlineblk">Shipping Address</h4>

                                        <a href="#" class="pull-right mt5" id="L3AEditAddress" runat="server">Edit Address</a>
                                        <asp:Button ID="BtnL3EditAddress" runat="server" Text="Edit Address" class="smp_btn pull-right" OnClientClick="EditAddressClick();return true;" UseSubmitBehavior="false" />
                                    </div>
                                    <div class="form-group row p15 text-left clearfix">
                                        <div class="col-sm-20 pv15 br_dark">
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L3ship_company_name" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L3ship_attn" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L3Ship_Street" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L3Ship_Address" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L3Ship_Suburb" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L3Ship_State" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L3Ship_Zipcode" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L3Ship_Country" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L3Ship_DELIVERYINST" />
                                            </p>


                                        </div>
                                    </div>
                                </div>

                                <div id="divl3continue" runat="server"></div>

                                <div class="headingwrap active clearfix mt20 ship_method">
                                    <div class="no_circle">3</div>
                                    <h4 class="numaric_heading inlineblk">Shipping Method</h4>
                                </div>

                                <div class="checkoutleft">
                                    <form>

                                        <div class="form-group pt10 mb30 clearfix">
                                            <div class="col-md-14 col-xs-17 pl0 mp0">
                                                <%--  <div class="floating-form">
                                            <div class="floating-label">      
                                              <asp:DropDownList NAME="drpSM1" ID="drpSM1" runat="server" class="form-control" onblur="javascript:onBlurValidate(this);">
                                                    <asp:ListItem Text="Please Select Shipping Method" Value="Please Select Shipping Method">Please Select Shipping Method</asp:ListItem>
                                                    <asp:ListItem Text="Standard Shipping" Value="Standard Shipping">Standard Shipping</asp:ListItem>
                                                    <asp:ListItem Text="Shop Counter Pickup" Value="Counter Pickup">Shop Counter Pickup</asp:ListItem>
                                                </asp:DropDownList>
                                              <span class="highlight"></span>
                                               <span id="lbrfdrpSM1" class="vldRequiredSkin" style="display: none">Please Select Shipping Method</span>
                                            </div>
                                           </div>--%>
                                                <asp:DropDownList NAME="drpSM1" ID="drpSM1" runat="server" class="floating-input" onblur="javascript:onBlurValidate(this);">
                                                    <asp:ListItem Text="Please Select Shipping Method" Value="Please Select Shipping Method">Please Select Shipping Method</asp:ListItem>
                                                    <asp:ListItem Text="Standard Shipping" Value="Standard Shipping">Standard Shipping</asp:ListItem>
                                                    <asp:ListItem Text="Shop Counter Pickup" Value="Counter Pickup">Shop Counter Pickup</asp:ListItem>
                                                </asp:DropDownList>
                                                <span id="lbrfdrpSM1" class="vldRequiredSkin" style="display: none">Please Select Shipping Method</span>
                                                <%-- <asp:RequiredFieldValidator Display="Dynamic" ID="rfvdrpSM1" InitialValue="Please Select Shipping Method" ControlToValidate="drpSM1" runat="server" ErrorMessage="Please Select Shipping Method" ValidationGroup="Mandatory_Express_Level3" SetFocusOnError="true" ForeColor="Red" />--%>
                                            </div>
                                            <div class="col-md-2 col-xs-2 mpr0 list-inline">
                                                <li>
                                                    <a href="#" data-target=".lgn-orderinfo" data-toggle="modal" role="button">
                                                        <img class="mt5" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/info1_Exp.png" />

                                                    </a>
                                                </li>
                                            </div>
                                        </div>
                                        <div class="form-group mb30 clearfix">
                                            <label>Order special notes / Comments</label>
                                            <asp:TextBox ID="TextBox1" runat="server" Rows="5" Columns="30" Font-Size="12px" CssClass="form-control" MaxLength="240"
                                                TextMode="MultiLine" onkeyup="javascript:keyboardup1(this);">
                                            </asp:TextBox>      

                                        </div>
                                        <div class="form-group mb10 clearfix" id="intorder" runat="server" style="display: none">
                                            <%--<label class="col-md-7 pl0">Purchase Order No :</label>
                                            <div class="col-md-10 mpl0">
                                                <asp:TextBox Width="100%" MaxLength="12" autocomplete="off" ID="ttinter_order" runat="server" onblur="checkponum_international()" placeholder="Order No" class="form-control" />
                                                <asp:Label Width="250px" ID="lblerror_ttinter" runat="server" ForeColor="red" />
                                            </div>--%>
                                             <div class="floating-form">
                                            <div class="floating-label">      
                                                <asp:TextBox Width="100%" MaxLength="12" autocomplete="off" ID="ttinter_order" runat="server" onblur="checkponum_international()" placeholder=" " class="floating-input" />
                                              <span class="highlight"></span>
                                              <label class="fl">Purchase Order No :</label>
                                                  <asp:Label Width="250px" ID="lblerror_ttinter" runat="server" ForeColor="red" />
                                            </div>
                                           </div>
                                            <div class="col-md-20 mt10 pl0">
                                                <p id="P1" runat="server">Optional field. Enter your own order reference ID.</p>
                                            </div>
                                        </div>
                                        <p class="text-center">

                                            <asp:Button runat="server" ID="BtnL3Continue" Text="Continue" class="ph30 f16 btn primary-btn-green" ValidationGroup="Mandatory_Express_Level3" OnClientClick="level3Continue(); return true;" UseSubmitBehavior="false" />
                                             <asp:Button runat="server" ID="BtnL3ContinueProcessing" Text="Processing Order.. Please Wait" Style="display: none;" class="ph30 f16 btn primary-btn-green" UseSubmitBehavior="false" />
                                                   
                                        </p>
                                        <div style="text-align: center; margin: 0 auto; display: block;">
                                            <div id="smspopup" class="chechout_notify">
                                                <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/info_icon.png" />
                                                <asp:Label ID="lblorderreadytext" runat="server" Text="SMS Order ready notification message will be sent to:" Style="margin-top: 4px; display: inline-block;"></asp:Label>
                                                <asp:Label ID="lblorderready" runat="server" Text=""
                                                    Style="font-weight: bolder; font-size: 14px; vertical-align: middle; color: #666;"></asp:Label>

                                                <a class="js-open-modal" onclick="ShowModal();" style="cursor: pointer;">Change</a>
                                            </div>
                                        </div>
                                        <div id="popup1" class="modal-box popup1">

                                            <div class="modal-body">
                                                <h2 class="pophead">Get notified by SMS when your </h2>
                                                <h2 class="pophead">Order is Ready for Pick Up</h2>
                                                <div class="entermobile">
                                                    <label>Enter Your Mobile Number:</label>
                                                    <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="pp_input" MaxLength="10"></asp:TextBox>
                                                    <div class="clearfix"></div>
                                                    <span id="lbrftxtMobileNumber" class="vldRequiredSkin" style="font-size: 12px; padding-top: 3px; display: none">Enter Mobile Number</span>
                                                    <span id="lbretxtMobileNumber" class="vldRequiredSkin" style="font-size: 12px; padding-top: 3px; display: none">Mobile No. must start with 04 and must be 10 digit</span>
                                                    <%-- <asp:RequiredFieldValidator ID="rfMobileNumber" runat="server" Class="vldRequiredSkin"
                                              ValidationGroup="x" Display="Dynamic" ErrorMessage="Enter Mobile Number" ControlToValidate="txtMobileNumber" Style="color: red"></asp:RequiredFieldValidator>
                                          <asp:RegularExpressionValidator ID="reMobileNumber" runat="server" ControlToValidate="txtMobileNumber"
                                              ValidationExpression="^(04)\d{8}$"
                                              Class="vldRegExSkin" Display="Dynamic" ValidationGroup="x" ErrorMessage="Mobile No. must start with 04 and must be 10 digit" Style="color: red"></asp:RegularExpressionValidator>--%>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtMobileNumber" />

                                                </div>

                                                <div class="btns">
                                                    <asp:CheckBox ID="chksavemobile" runat="server" Checked="true" />
                                                    <label class="lbl">Save number for future orders</label>
                                                    <asp:Button ID="notifymeBtn" runat="server" Text="Ok, Notify Me" CssClass="notifyme" ValidationGroup="x" OnClientClick="return notifiyMeClick();" UseSubmitBehavior="false" />
                                                    <asp:Button ID="noThanksBtn" runat="server" Text="No, Thanks" CssClass="nothanks" OnClientClick="noThanksBtnClick();return true;" UseSubmitBehavior="false" />
                                                </div>
                                            </div>

                                        </div>
                                        <div id="divchangepopup" class="modal-box sms" style="top: 0px; left: 0px">

                                            <div class="modal-body">
                                                <h2 class="pophead">Get notified by SMS when your </h2>
                                                <h2 class="pophead">Order is Ready for Pick Up</h2>
                                                <div class="entermobile">
                                                    <label>Enter New Number:</label>
                                                    <asp:TextBox ID="txtchangemobilenumber" runat="server" CssClass="pp_input" MaxLength="10" onkeypress="return isNumber(event)"></asp:TextBox>
                                                    <div class="clearfix"></div>
                                                    <span id="lbrftxtchangemobilenumber" class="vldRequiredSkin" style="font-size: 12px; padding-top: 3px; display: none">Enter Mobile Number</span>
                                                    <span id="lbretxtchangemobilenumber" class="vldRequiredSkin" style="font-size: 12px; padding-top: 3px; display: none">Mobile No. must start with 04 and must be 10 digit</span>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" Class="vldRequiredSkin"
                                              ValidationGroup="y" Display="Dynamic" ErrorMessage="Enter Mobile Number" ControlToValidate="txtchangemobilenumber" Style="color: red"></asp:RequiredFieldValidator>
                                          <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtchangemobilenumber"
                                              ValidationExpression="^(04)\d{8}$"
                                              Class="vldRegExSkin" Display="Dynamic" ValidationGroup="y" ErrorMessage="Mobile No. must start with 04 and must be 10 digit" Style="color: red"></asp:RegularExpressionValidator>--%>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtMobileNumber" />

                                                </div>

                                                <div class="btns">
                                                    <asp:CheckBox ID="cbmobilechange" runat="server" Checked="true" />
                                                    <label class="lbl">Save number for future orders</label>
                                                    <asp:Button ID="btnMobileNoChange" runat="server" Text="Ok, Notify Me" CssClass="notifyme" ValidationGroup="y" OnClientClick="return btnMobileNoChangeClick()" UseSubmitBehavior="false" />
                                                    <asp:Button ID="btnNoThanksChange" runat="server" Text="No, Thanks" CssClass="nothanks" OnClientClick="btnNoThanksChangeClick();return true;" UseSubmitBehavior="false" />
                                                </div>
                                            </div>


                                        </div>
                                    </form>
                                </div>


                                <div class="headingwrap clearfix mt20 mb20">
                                    <div class="no_circle">4</div>
                                    <h4 class="numaric_heading inlineblk">Payment</h4>
                                </div>

                            </div>


                            <div class="wrapleft pr40 mpr0 pb20 clearfix" id="Level4" runat="server">

                                <div class="clearfix">
                                    <div class="headingwrap visited clearfix">
                                        <div class="no_circle">1</div>
                                        <h4 class="numaric_heading inlineblk">Contact Details</h4>
                                        <asp:Button ID="btneditlogin4" runat="server" Text="Edit" class="smp_btn pull-right" OnClientClick="BtnEditLoginClick();return true;" UseSubmitBehavior="false" />
                                    </div>
                                    <div class="form-group row p15 text-left clearfix">
                                        <div class="col-sm-20 pv15 br_dark">
                                            <p class="mb0">
                                                <b>Name : </b>
                                                <asp:Label runat="server" ID="L4name" />
                                            </p>
                                            <p class="mb0">
                                                <b>Email : </b>
                                                <asp:Label runat="server" ID="L4Email" />
                                            </p>
                                            <p class="mb0">
                                                <b>Phone : </b>
                                                <asp:Label runat="server" ID="L4Phone" />
                                            </p>
                                            <p class="mb0" id="P4Mobile" runat="server"><b>Mobile : </b>
                                                <asp:Label runat="server" ID="L4Mobile" /></p>
                                        </div>
                                    </div>
                                </div>


                                <div class="clearfix">
                                    <div class="headingwrap visited clearfix">
                                        <div class="no_circle">2</div>
                                        <h4 class="numaric_heading inlineblk">Shipping Address</h4>

                                        <a href="#" class="pull-right mt5" id="L4AEditAddress" runat="server">Edit Address</a>
                                        <asp:Button ID="BtnEditAddress" runat="server" Text="Edit Address" class="smp_btn pull-right" OnClientClick="EditAddressClick();return true;" UseSubmitBehavior="false" />
                                    </div>
                                    <div class="form-group row p15 text-left clearfix">
                                        <div class="col-sm-20 pv15 br_dark">
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L4Ship_Company" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L4Ship_Attnto" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L4Ship_Street" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L4Ship_Address" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L4Ship_Suburb" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L4Ship_State" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L4Ship_Zipcode" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L4Ship_Country" />
                                            </p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L4Ship_DELIVERYINST" />
                                            </p>

                                        </div>
                                    </div>
                                </div>


                                <div class="clearfix">
                                    <div class="headingwrap visited clearfix">
                                        <div class="no_circle">3</div>
                                        <h4 class="numaric_heading inlineblk">Shipping Method</h4>
                                        <a href="#" class="pull-right mt5" id="L4AEditShippingMethod" runat="server">Edit Shipping Method</a>
                                        <asp:Button ID="BtnL4EditShippingMethod" runat="server" Text="Edit Shipping Method" OnClientClick="BtnL4EditShippingMethodClick();return true;" class="smp_btn pull-right" UseSubmitBehavior="false" />
                                    </div>
                                    <div class="form-group row p15 text-left clearfix">
                                        <div class="col-sm-20 pv15 br_dark">
                                            <div id="divshopcounter" runat="server">
                                                <p class="mb0"><b>Click and Collect </b></p>
                                                <p class="mb0">
                                                    Pickup from wagner electronic warehouse
                                          <br />
                                                    84-90, Paramtta Road,
                            	Summer Hill, NSW 2130
                                                </p>
                                                <br />
                                            </div>
                                            <div id="divstandardship" runat="server">
                                                <p class="mb0"><b>Shipping Method</b> </p>
                                                <p class="mb0">
                                                    <asp:Label ID="lblShippingMethod" runat="server" Text=""></asp:Label>
                                                </p>
                                                <br />
                                            </div>
                                            <p class="mb0"><b>Order special notes / Comments </b></p>
                                            <p class="mb0">
                                                <asp:Label runat="server" ID="L4Comments" />
                                            </p>
                                        </div>
                                    </div>
                                </div>


                                <div class="headingwrap active clearfix mt20 mb20" id="divl4payment" runat="server">
                                    <div class="no_circle">4</div>
                                    <%-- <% 
                              OrderServices objOrderServices = new OrderServices();
                              if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                              {
                          %>--%>
                                    <h4 id="PaymentText" runat="server" class="numaric_heading inline">Payment</h4>

                                    <%-- <% }
                         else
                         { %>--%>
                                    <h4 id="SubmitText" runat="server" class="numaric_heading inline">Submit Order</h4>

                                    <div id="Div3" class="col-sm-19 pv15 br_dark m10" runat="server">
                                        <div class="form-group mb10 clearfix">
                                            <label class="col-md-7 pl0">Purchase Order No :</label>
                                            <div class="col-md-10 mpl0">
                                                <asp:TextBox Width="100%" MaxLength="12" ID="txtintporelease_No" runat="server"
                                                    placeholder="Order No" class="form-control" autocomplete="off" ReadOnly="true" Enabled="false" />
                                            </div>
                                        </div>
                                        <div id="div_linkint">
                                            Payment Method :To Be Advised
                                        </div>
                                    </div>
                                    <%-- <% } %>--%>
                                </div>


                                <div class="checkoutleft" id="Level4_Payment">


                                    <div class="form-group pb20 br_b1 clearfix">
                                        <label class="col-md-7 pl0">Purchase Order No :</label>
                                        <div class="col-md-10 mpl0">
                                            <asp:TextBox Width="100%" MaxLength="12" ID="ttOrder" runat="server" placeholder="Order No" class="form-control" autocomplete="off" onblur="checkponum()" />
                                            <asp:Label Width="250px" ID="txterr" runat="server" ForeColor="red" />
                                        </div>
                                      <%--  <div class="floating-form">
                                            <div class="floating-label">      
                                                <asp:TextBox Width="100%" MaxLength="12" ID="ttOrder" runat="server" placeholder=" " class="floating-input" autocomplete="off" onblur="checkponum()" />
                                              <span class="highlight"></span>
                                              <label class="fl">Purchase Order No :</label>
                                                 <asp:Label Width="250px" ID="txterr" runat="server" ForeColor="red" />
                                            </div>
                                           </div>--%>
                                        <div class="col-md-20 mt10 pl0">
                                            <p id="of_ptag" runat="server">Optional field. Enter your own order reference ID.</p>
                                        </div>
                                    </div>
                                    <label id="label1" runat="server"></label>
                                    <div id="divError" runat="server" style="color: Red;">
                                    </div>

                                    <div id="divlink" runat="server">
                                    </div>

                                   

                                    <div runat="server" id="div2" class="accordion_head_yellow gray_40" style="font-size: 12px; text-align: center; font-weight: bold; margin-bottom: 12px; background: #FFF200; padding: 12px 17px;">
                                    </div>
                                    <h4 class="panel-title" id="h3Pay1" runat="server"><span class="collapsed"></span></h4>

                                    <div id="divpaid" runat="server"></div>

                                    <div class="form-group dblock" id="PayType" runat="server" visible="true">
                                        <%-- <% 
                                  OrderServices objOrderServices = new OrderServices();

                                  if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                                  {
                                      decimal totamt;
                                      if (lblAmount.Text == "")
                                      {
                                          OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                                          oOrderInfo = objOrderServices.GetOrder(OrderID);
                                          totamt = oOrderInfo.TotalAmount;
                                      }
                                      else
                                      {
                                          totamt = Convert.ToDecimal(lblAmount.Text);
                                      }

                                      if (totamt <= 300)
                                      {
                               %> --%>

                                        <label class="checkbox-inline lspace0" runat="server" style="color: white" id="lbcard">
                                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="btnSecurePayLink();return false;" CausesValidation="false">
                                                <asp:Image ID="ImagePaySP" runat="server" Style="margin-top: -5px; cursor: pointer;" alt="cc" />
                                            </asp:LinkButton>
                                        </label>


                                        <%--   <%} %>--%>
                                        <label class="checkbox-inline lspace0" runat="server" style="color: white" id="lbPaypal">
                                            .
                               <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="true"
                                   OnClientClick="btnPayPalPayLink();return false;">

                                   <asp:Image ID="ImagePay" runat="server" Style="margin-top: -5px; cursor: pointer;" alt="cc" />
                                            </asp:LinkButton>
                                        </label>

                                        <label style="color: White">.</label>

                                        <%-- <%} %>  --%>
                                    </div>

                                    <div id="SecurePayAcc" runat="server" visible="false">
                                        <div runat="server" id="PaySPDiv" class="payment_form clearfix">
                                            <asp:DropDownList ID="drppaymentmethod" runat="server" Width="200px" CssClass="cardinput" onchange="Controlvalidate('dd')" Visible="false" />

                                              <div class="form-group mb0 clearfix">
                                                <div class="floating-form mb0">
                                                    <div class="floating-label">
                                                        <asp:TextBox runat="server" ID="txtCardNumber" CssClass="floating-input" MaxLength="19" placeholder=" " OnBlur="Controlvalidate('cno')" ValidationGroup="sp" />
                                                        <span class="highlight"></span>
                                                        <label class="fl">Card Number</label>
                                                        <span id="lbrftxtCardNumber" class="error-text" style="display: none; color: red">Enter Card Number</span>
                                                        <asp:CustomValidator ID="CustomValidator1" Display="Dynamic" ValidationGroup="sp" CssClass="error-text" ErrorMessage="Please Check Credit Card Number" ControlToValidate="txtCardNumber" runat="server">
                                                        </asp:CustomValidator>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group mb10 clearfix">
                                                 <div class="floating-form mt5">
                                                        <div class="floating-label">
                                                            <asp:TextBox runat="server" ID="txtCardName" CssClass="floating-input" MaxLength="50" placeholder=" " OnBlur="Controlvalidate('cn')" />
                                                            <span class="highlight"></span>
                                                            <label class="fl">Name on Card</label>
                                                            <span id="lbrftxtCardName" class="error-text" style="display: none; color: red">Enter Name on Card</span>
                                                        </div>
                                                    </div>
                                            </div>

                                          

                                            <div class="form-group mb10 clearfix">
                                                <div class="floating-form mt5">
                                                    <div class="floating-label clearfix" style="float:left">
                                                        <asp:TextBox runat="server" ID="txtCardCVVNumber" Width="100px" Style="float: left" CssClass="floating-input" MaxLength="4" placeholder=" " OnBlur="Controlvalidate('cvv')" Text="" />
                                                        <span class="highlight"></span>
                                                        <label class="fl">CVV</label>

                                                      
                                                        <span id="lbrftxtCardCVVNumber" class="error-text" style="display: none; color: red; float: left;">Enter Card Security Code</span>
                                                    </div>

                                                      <div class="cvv-right" style="float:left">
                                                            <span class="mandatory checkouterror" style="margin-top: 0px;">
                                                                <a href="#" class="checkoutlink" data-target=".bs-example-modal-lg1" data-toggle="modal" role="button">
                                                                    <img class=" margin_rgt15" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/question-icon.png" /></a>
                                                            </span>
                                                        </div>
                                                </div>
                                            </div>

                                             <div class="form-group mb10 clearfix">
                                                 <div class="floating-form">
                                                     <div class="floating-label clearfix">
                                                         <asp:DropDownList NAME="drpExpmonth" ID="drpExpmonth" runat="server" placeholder=" " Style="width: 35%; float: left; margin-right: 6%;" CssClass="floating-input" onblur="javascript:onBlurValidate(this);">
                                                             <asp:ListItem Selected="true" Text="01" Value="01"></asp:ListItem>
                                                             <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                             <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                             <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                             <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                             <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                             <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                             <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                             <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                             <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                             <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                             <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                         </asp:DropDownList>
                                                         <span class="highlight"></span>
                                                         <label class="fl">Card Expiry</label>
                                                         <asp:DropDownList NAME="drpExpyear" ID="drpExpyear" runat="server" placeholder=" " Style="width: 35%; float: left" CssClass="floating-input" onblur="javascript:onBlurValidate(this);">
                                                         </asp:DropDownList>
                                                         <span id="lbrfdrpExpmonth" class="error-text" style="display: none; color: red">Select Month</span>
                                                         <span id="lbrfdrpExpyear" class="error-text" style="display: none; color: red">Select Year</span>
                                                     </div>
                                                 </div>
                                            </div>

                                         <%--   <div class="col-sm-20 mp0 nolftpadd">
                                                <div class="form-group col-lg-20 mp0 nolftpadd">
                                                    <%--<div class="col-lg-20 mp0 nolftpadd">
                                                        <label>
                                                            Name on Card   
                                                    <span class="required">*</span>
                                                        </label>
                                                        <asp:TextBox runat="server" ID="txtCardName" CssClass="form-control checkout_input" MaxLength="50" OnBlur="Controlvalidate('cn')" />
                                                    </div>--%>
                                                   <%-- <div class="floating-form">
                                                        <div class="floating-label">
                                                            <asp:TextBox runat="server" ID="txtCardName" CssClass="floating-input" MaxLength="50" placeholder=" " OnBlur="Controlvalidate('cn')" />
                                                            <span class="highlight"></span>
                                                            <label class="fl">Name on Card</label>
                                                            <span id="lbrftxtCardName" class="error-text" style="display: none; color: red">Enter Name on Card</span>
                                                        </div>
                                                    </div>--%>

                                                    <%--  <div class="col-lg-8">
                                                        <p class="mandatory m0 checkouterror">
                                                            <span id="lbrftxtCardName" class="error-text" style="display: none; color: red">Enter Name on Card</span>
                                                        </p>
                                                    </div>-%>
                                                </div>
                                            </div>--%>

                                           <%-- <div class="col-sm-20 mp0 nolftpadd">
                                                <div class="form-group col-lg-20 mp0 nolftpadd">
                                                    <%--<div class="col-lg-20 mp0 nolftpadd">
                                                        <label>Card Number &nbsp;&nbsp;<span class="required">*</span></label>
                                                        <asp:TextBox runat="server" ID="txtCardNumber" CssClass="form-control checkout_input" MaxLength="19" OnBlur="Controlvalidate('cno')" ValidationGroup="sp" />
                                                    </div>
                                                    <div class="col-lg-8">
                                                        <p class="mandatory m0 checkouterror">
                                                            <span id="lbrftxtCardNumber" class="error-text" style="display: none; color: red">Enter Card Number</span>
                                                            <asp:CustomValidator ID="CustomValidator1" Display="Dynamic" ValidationGroup="sp" CssClass="error-text" ErrorMessage="Please Check Credit Card Number" ControlToValidate="txtCardNumber" runat="server">
                                                            </asp:CustomValidator>
                                                        </p>
                                                    </div>-%>
                                                     <div class="floating-form">
                                                    <div class="floating-label">
                                                         <asp:TextBox runat="server" ID="txtCardNumber" CssClass="floating-input" MaxLength="19" placeholder=" " OnBlur="Controlvalidate('cno')" ValidationGroup="sp" />
                                                        <span class="highlight"></span>
                                                        <label class="fl">Card Number</label>
                                                          <span id="lbrftxtCardNumber" class="error-text" style="display: none; color: red">Enter Card Number</span>
                                                            <asp:CustomValidator ID="CustomValidator1" Display="Dynamic" ValidationGroup="sp" CssClass="error-text" ErrorMessage="Please Check Credit Card Number" ControlToValidate="txtCardNumber" runat="server">
                                                            </asp:CustomValidator>
                                                    </div>
                                                </div>
                                                </div>
                                            </div>--%>

                                            <%--<div class="col-sm-20 mp0 nolftpadd clearfix">
                                                <div class="form-group col-lg-10 col-sm-20 mp0 nolftpadd">
                                                    <%--<div class="cvv-left">
                                                        <label>CVV &nbsp;&nbsp;<span class="required">*</span></label>
                                                        <asp:TextBox runat="server" ID="txtCardCVVNumber" Width="100px" CssClass="form-control" MaxLength="4" OnBlur="Controlvalidate('cvv')" Text="" />
                                                    </div>
                                                    <div class="cvv-right">
                                                        <span class="mandatory checkouterror">
                                                            <a href="#" class="checkoutlink" data-target=".bs-example-modal-lg1" data-toggle="modal" role="button">
                                                                <img class=" margin_rgt15" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/question-icon.png" /></a>
                                                        </span>
                                                    </div>-%>
                                                     <div class="floating-form">
                                                    <div class="floating-label clearfix">
                                                        <asp:TextBox runat="server" ID="txtCardCVVNumber" Width="100px" style="float:left" CssClass="floating-input" MaxLength="4" placeholder=" " OnBlur="Controlvalidate('cvv')" Text="" />
                                                        <span class="highlight"></span>
                                                        <label class="fl">CVV</label>
                                                        
                                                         <div class="cvv-right">
                                                        <span class="mandatory checkouterror" style=" margin-top: 0px;">
                                                            <a href="#" class="checkoutlink" data-target=".bs-example-modal-lg1" data-toggle="modal" role="button">
                                                                <img class=" margin_rgt15" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/question-icon.png" /></a>
                                                        </span>
                                                    </div>
                                                        <span id="lbrftxtCardCVVNumber" class="error-text" style="display: none; color: red;float: left;">Enter Card Security Code</span>
                                                    </div>
                                                </div>
                                                </div>
                                               <%-- <div class="col-lg-8">
                                                    <p class="mandatory m0 checkouterror">
                                                        <span id="lbrftxtCardCVVNumber" class="error-text" style="display: none; color: red">Enter Card Security Code</span>
                                                    </p>
                                                </div>-%>
                                            </div>--%>


                                          <%--  <div class="col-sm-20 nolftpadd clearfix margin_top15 mp0">
                                                <div class="form-group col-lg-20 mp0 nolftpadd clearfix">
                                                  <%--  <div class="col-lg-8 col-xs-10 mp0 nolftpadd">
                                                        <label>Card Expiry &nbsp;&nbsp;<span class="required">*</span></label>

                                                        <asp:DropDownList NAME="drpExpmonth" ID="drpExpmonth" runat="server" CssClass="form-control checkout_input" onblur="javascript:onBlurValidate(this);">
                                                            <asp:ListItem Selected="true" Text="01" Value="01"></asp:ListItem>
                                                            <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                            <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                            <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                            <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                            <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                            <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                            <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-8 col-xs-10 mp0">
                                                        <label>&nbsp;&nbsp;<span class="required">&nbsp;&nbsp;</span></label>
                                                        <asp:DropDownList NAME="drpExpyear" ID="drpExpyear" runat="server" CssClass="form-control checkout_input" onblur="javascript:onBlurValidate(this);">
                                                        </asp:DropDownList>
                                                    </div>--%>
                                                    <%--  <div class="floating-form">
                                                    <div class="floating-label clearfix">
                                                          <asp:DropDownList NAME="drpExpmonth" ID="drpExpmonth" runat="server" placeholder=" " style="width: 35%;float: left;margin-right: 6%;" CssClass="floating-input" onblur="javascript:onBlurValidate(this);">
                                                            <asp:ListItem Selected="true" Text="01" Value="01"></asp:ListItem>
                                                            <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                            <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                            <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                            <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                            <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                            <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                            <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span class="highlight"></span>
                                                        <label class="fl">Card Expiry</label>
                                                         <asp:DropDownList NAME="drpExpyear" ID="drpExpyear" runat="server" placeholder=" "  style="width: 35%;float: left" CssClass="floating-input" onblur="javascript:onBlurValidate(this);">
                                                        </asp:DropDownList>
                                                          <span id="lbrfdrpExpmonth" class="error-text" style="display: none; color: red">Select Month</span>
                                                            <span id="lbrfdrpExpyear" class="error-text" style="display: none; color: red">Select Year</span>
                                                    </div>
                                                </div>--%>

                                                  <%--  <div class="col-lg-8 col-xs-20">
                                                        <p class="mandatory m0 checkouterror">
                                                            <span id="lbrfdrpExpmonth" class="error-text" style="display: none; color: red">Select Month</span>
                                                            <span id="lbrfdrpExpyear" class="error-text" style="display: none; color: red">Select Year</span>
                                                            <%-- <asp:RequiredFieldValidator ID="rfmonthsp" runat="server"
                                                        ControlToValidate="drpExpmonth" Display="Dynamic" CssClass="error-text"
                                                        ErrorMessage="Select Month"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server"
                                                        ControlToValidate="drpExpyear" Display="Dynamic" CssClass="error-text"
                                                        ErrorMessage="Select Year" ValidationGroup="sp"></asp:RequiredFieldValidator>-%>
                                                        </p>
                                                    </div>-%>
                                                </div>
                                            </div>--%>

                                            <div class="col-sm-20 nolftpadd">
                                                <div class="form-group col-lg-20 nolftpadd">
                                                    <h3 class="green_clr" style="font-weight: bold">Total Amount &nbsp $
                                                <asp:Label runat="server" ID="lblAmount" CssClass="totalamt" />
                                                    </h3>

                                                </div>
                                            </div>



                                            <div class="cl"></div>
                                            <div class="col-sm-20 nolftpadd margin_btm_30">
                                                <div class="form-group col-lg-8 nolftpadd">
                                                    <asp:Button runat="server" ID="btnSP" ValidationGroup="sp" Text="Pay Now" class="btn btn-primary paynow_lg" OnClick="btnSecurePay_Click" OnClientClick="return SetinitSP();" />
                                                    <asp:Button runat="server" ID="BtnProgressSP" Text="Processing Payment. Please Wait" Style="display: none; visibility: visible; float: left;" class="btn btn-primary" Enabled="false" />
                                                    <div id="div6" style="font-size: 12px; margin-left: 30px; color: Red" runat="server"></div>
                                                </div>
                                            </div>

                                            <div class="cl"></div>
                                        </div>

                                    </div>


                                    <div id="PayPaypalAcc" runat="server" style="display: none" >

                                        <%  
                                            Boolean blnpp = true; 
                                        %>
                                        <% if (blnpp == false)
                                           { %>
                                        <div style="padding: 16px 2px 16px 174px; background-color: #FFD52B" class="alert yellowbox icon_4">
                                            <h3 style="font-size: 16px; color: Black;">Paypal Payment Option is currently unavailable.
                                        <br />
                                                Please kindly proceed to check out with Standard Credit Card payment option.</h3>
                                        </div>
                                        <%}
                                           else
                                           {     %>
                                        <div class="col-sm-20 mp0 nolftpadd" style="display: block; text-align: center; margin-bottom: 15px; border: 1px solid #ccc;">
                                            <div class="form-group text-center">
                                                <div id="divpaypalaccount" runat="server">
                                                    <p class="blue_text margin_top15"><strong>Pay using your Paypal Account</strong></p>
                                                    <p class="margin_bott15">You will be redirected to paypal website to complete payment transaction</p>
                                                </div>

                                                <h3 class="blue_text"><b>Total Amount &nbsp $
                                            <asp:Label runat="server" ID="lblpaypaltotamt" CssClass="totalamt" />
                                                </b></h3>
                                            </div>
                                        </div>
                                        <%} %>

                                        <p class="text-center">
                                            <%-- <asp:Button runat="server" ID="btnPay" Text="Make Payment" class="ph30 f16 btn primary-btn-green " OnClientClick="btnPayClick();return true;" UseSubmitBehavior="false" />--%>
                                            <%--<asp:Button runat="server" ID="btnPay" Text="Make Payment" class="ph30 f16 btn primary-btn-green " OnClientClick="btnPayClick();return true;"/>--%>
                                            <asp:Button runat="server" ID="btnPay" Text="Make Payment" class="ph30 f16 btn primary-btn-green " OnClick="btnPay_Click" OnClientClick=" Setinit(this.id)" />
                                            <%-- Setinit(this.id)--%>
                                            <br />
                                            <asp:Button runat="server" ID="BtnProgress" Text="Processing Payment. Please Wait" Style="display: none; visibility: visible; float: left;" class="btn btn-primary " Enabled="false" />
                                            <br />
                                        </p>
                                        <br />
                                        <br />
                                    </div>

                            
                                    <div class="row">
    <div class="col-xs-20" >
      <div id="drop-in"></div>
    </div>
  </div>
 <div class="row">
      <asp:HiddenField ID="HiddenField1" runat="server" />

    <div class="col-xs-6">
   <%--   <div class="input-group nonce-group hidden">
        <span class="input-group-addon">nonce</span>
        <input readonly name="nonce" class="form-control">
      </div>--%>
      <div class="input-group pay-group">
        <input disabled id="pay-btn" class="btn btn-success" type="submit" value="Loading...">
      </div>
    </div>
  </div>
                                </div>



                                <div class="checkoutleft nopadd" id="Level4_Submit" runat="server">
                                    <div class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" id="divship" runat="server">
                                        <div class="panel-body">
                                            <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none; visibility: hidden"></asp:Button>
                                            <div id="divonlinesubmitordererror" visible="false" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6); display: block;" class="modal fade bs-example-modal-lg in">
                                                <div class="modal-dialog modal-lg">
                                                    <div class="modal-content">
                                                        <div class="modal-header blue_color padding_top padding_btm" style="background-color: #0069b2; color: #fff;">
                                                            <h4 id="H5" class=" white_color font_weight modal-title">Invalid Login Details for Checkout! </h4>
                                                        </div>
                                                        <div class="modal-body">
                                                            <p>
                                                                User Id does not matche with online submit order.
                            <br />
                                                                <a href="/logout.aspx" style="color: #15c;">Please Click Here</a>
                                                            </p>


                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <asp:Button ID="btnHiddenpopuploginreg" runat="server" Style="display: none; visibility: hidden"></asp:Button>
                                            <div id="Hiddenpopuploginreg" align="center" runat="server">
                                                <asp:Panel ID="HiddenpopuploginregPanel" runat="server" CssClass="PopUpDisplayStyleship">
                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                                                        align="center">
                                                        <tr style="height: 5px">
                                                            <td colspan="3">&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px">
                                                            <td width="100%" align="center" colspan="3">&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px">
                                                            <td width="100%" align="center" colspan="3" class="TextContentStyleship">Continue to Checkout Please  Login Or Register
                            <br />
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px">
                                                            <td width="100%" align="center" colspan="3">&nbsp;
                                                            </td>
                                                        </tr>

                                                        <tr style="height: 10px">
                                                            <td width="35%" align="right"></td>
                                                            <td width="30%">
                                                                <asp:Button ID="loginregrdir" runat="server" Text="Login Or Register"
                                                                    Width="205px" CssClass="button normalsiz btnblue" />
                                                            </td>
                                                            <td width="35%" align="left"></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>

                                            <asp:Button ID="btnitemerrors" runat="server" Style="display: none; visibility: hidden"></asp:Button>
                                            <div id="PopupMsg" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6); display: block;" class="modal fade bs-example-modal-lg in">
                                                <div class="modal-dialog  margin_top_popup20 ">
                                                    <div class="modal-content  border_radius_none">
                                                        <div class="modal-body">
                                                            <p class="alert-red">Please review and correct Order Clarification / Errors before proceeding to Check Out!  </p>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <asp:Button ID="btnCancelerroritems" runat="server" Text="Close" CssClass="btn-lg padding_top btn-danger border_none border_radius_none white_color semi_bold font_14 mar_right_30  mob_100 margin_left" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                </div>

                            </div>



                        </div>

                    </div>
                </div>

                <div class="" id="liPayOption" runat="server">
                    <div id="headingtwo" role="tab" class="">
                        <h4 class="panel-title" onclick="OnclickTab('Pay')" style="cursor: pointer;" id="h3Pay" runat="server">
                            <span class="collapsed" id="spanPay" runat="server"></span>
                        </h4>

                    </div>

                    <div runat="server" id="Paydiv">
                        <div class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo" id="divPay" runat="server">
                            <div class="panel-body">
                                <div runat="server" id="divCC" visible="false">
                                    <div class="col-lg-20" runat="server" id="div1">
                                        <div runat="server" id="Div5">
                                            <div runat="server" id="PaypalApiDiv">
                                                <div class="form-group dblock">
                                                    <%  
                                                        Boolean blnpp = true;
                                                        if (blnpp == false)
                                                        { %>
                                                    <div style="background-color: #FFD52B; display: block; height: 100px; text-align: center; margin-bottom: 15px; border: 1px solid #ccc;" class="alert yellowbox icon_4">
                                                        <h3 style="font-size: 16px; color: Black;">Paypal Payment Option is currently unavailable.
                                                  <br />
                                                            Please kindly proceed to check out with Standard Credit Card payment option.</h3>
                                                    </div>
                                                    <%}
                                            else
                                            {     %>

                                                    <p class="blue_text margin_top15"><strong>Pay using your Paypal Account</strong></p>
                                                    <p class="margin_bott15">You will be redirected to paypal website to complete payment transaction</p>
                                                    <div class="col-sm-20 nolftpadd">
                                                        <div class="form-group col-lg-20 nolftpadd">
                                                            <h3 class="green_clr" style="font-weight: bold">Total Amount $  
                         
                                                            </h3>
                                                        </div>
                                                    </div>
                                                    <asp:Button runat="server" ID="btnPayApi" Text="Pay Now" Style="width: 100px;" class="btn btn-primary " Visible="false" OnClick="btnPayApi_Click" OnClientClick="Setinit(this.id)" />
                                                    <div class="cl"></div>
                                                    <%} %>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-20 col-xs-20">
                                            <div class="table-responsive col-lg-20 col-sm-20">
                                                <div class="col-lg-20 text-right">
                                                    <asp:Button ID="ImgBtnEditShipping" runat="server" Text="Edit / Update Order" Style="width: auto !important" CssClass="btn btn-primary" CausesValidation="false" Visible="false" />
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="grid12" runat="server" id="divTimeout" visible="false">
                                        <fieldset>
                                            <div style="text-align: center; padding: 130px;">
                                                <span style="font-size: 21px;">Your session has timed out</span><br />
                                                <span style="font-size: 14px;"><a href="/Login.aspx" class="para pad10-0" style="font-size: 11px; color: #0033cc; font-weight: bold;">Click here</a> to log in again </span>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="cl"></div>
                                </div>
                            </div>
                        </div>
                        <div id="divContent" style="font-size: 12px; margin-left: 30px; color: Red" runat="server"></div>
                    </div>

                </div>

                <div class="" id="liFinalReview" runat="server">
                    <div class="" role="tab">
                        <h4 class="panel-title" onclick="OnclickTab('paid')" style="cursor: pointer;" id="hpaid" runat="server">
                            <span class="collapsed">
                                <span id="spanpaid" runat="server"></span></span>
                        </h4>

                        <h4 class="panel-title" id="hpaid1" runat="server"></h4>
                    </div>
                    <div runat="server" id="paiddiv">
                    </div>
                </div>

            </div>
        </div>

    </div>

    <div id="PopDiv" runat="server" style="background: rgba(0, 0, 0, 0.6) none repeat scroll 0% 0%; display: none; padding-right: 17px;" aria-hidden="true" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" class="modal fade lgn-orderinfo">
        <div class="modal-dialog custom">
            <div class="modal-content">
                <div class="close-selected">
                    <asp:ImageButton ID="btnclose1" runat="server" data-dismiss="modal" style="margin:10px" />
                </div>
                <div class="modal-header green_bg">
                    <h4 id="H2" class="text-center">
                        <img class="popsucess" alt="img" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Order__info.png" />Shipment Method Options</h4>
                </div>
                <div class="modal-body">
                    <div class="col-lg-20 col-sm-20">
                        <div class="popuptext">
                            <p style="font-size: 12px;">
                                <b style="font-size: 16px;">Courier Service</b>
                                <br />
                                Australia wide flat rate delivery charge of $9.90*
                            <br />
                                Goods will be sent using Courier Service if under Gross /Cubic weight of 3Kg.                                 
                            <br />
                                Parcels over 3Kg will be sent by most economical means, eg Road, E-Parcel Service etc. 
                            <br />
                                Exemptions apply to heavy / large items (e.g Server Racks) will occur higher delivery charges our sales team will contact you prior to shipping. 
                            <br />
                                Note. Dangerous goods cannot be sent by air, road service only.
                            <br />


                                <b style="font-size: 16px;">Shop Counter Pick Up</b>
                                <br />
                                Place your order online and pick up goods from our Sydney Showroom.
                            <br>
                                Important Details For Store Pick Up:
                            <br>
                                1. Please bring a printed copy of invoice when coming into store.
                            <br />
                                2. Proof of Identity is required when picking up goods from our store. Please ensure you have the Credit Card you made the purchase with and your Driver’s 
                            <br />
                                License ID when coming into the store to pick up goods purchased online. The Name on the Credit Card Must Match Your Drivers License ID.
                            <br />
                                You will not be able to pick up the goods from our store unless the Credit Card you made the purchase with and your Drivers License are shown OR if there if 
                            <br />
                                there is a mismatch in details.  
                            </p>
                        </div>
                    </div>
                </div>
                <div class="modal-footer clear border_top_none">
                    <asp:Button ID="btnOk" runat="server" Text="Close" CssClass="btn button-close" data-dismiss="modal" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>

    <div id="SCP" runat="server" style="background: rgba(0, 0, 0, 0.6) none repeat scroll 0% 0%; display: none; padding-right: 17px;" aria-hidden="true" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" class="modal fade lgn-orderinfoscp">
        <div class="modal-dialog custom">
            <div class="modal-content">
                <div class="close-selected">
                    <asp:ImageButton ID="ImageButton3" runat="server" OnClientClick="CloseSCP();" style="margin:10px"/>
                </div>
                <div class="modal-header green_bg">
                    <h4 id="H4" class="text-center">
                        <img class="popsucess" alt="img" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Order__info.png" />Shop Counter Pickup</h4>
                </div>
                <div class="modal-body">
                    <div class="col-lg-20 col-sm-20">
                        <div class="popuptext">
                            <p style="font-size: 12px;">
                                <b style="font-size: 13px;">IMPORTANT DETAILS FOR WHEN PICKING UP GOODS FROM STORE</b>
                                <br />
                                <br />
                                1. Please being a printed copy of invoice when coming into store.
                            <br />
                                2. Proof of Identity is required. When picking up goods from our store you will be required to show proof of identity and the credit card you made the purchase with.
                            <br />
                                The Name on the Credit Card Must Match Your Drivers License ID. Please ensure you have the Credit Card you made the purchase with and your Driver’s License ID when coming into the store to pick up goods purchased online.
                            <br />
                                You will not be able to pick up the goods from our store unless the Credit Card you made the purchase with and your Drivers License are shown OR if there if there is a mismatch in details.
                            <br />
                                When picking up goods from our store you will be required to show proof of identity and the credit card you made the purchase with.  
                            </p>
                        </div>
                    </div>
                </div>
                <div class="modal-footer clear border_top_none">
                    <asp:Button ID="Button1" runat="server" Text="Close" CssClass="btn button-close" OnClientClick="CloseSCP();" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>

    <div id="CCPopDiv" runat="server" style="background: rgba(0, 0, 0, 0.6) none repeat scroll 0% 0%; display: none; padding-right: 17px;" aria-hidden="true" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" class="modal fade bs-example-modal-lg1">

        <div class="modal-dialog custom">
            <div class="modal-content">
                 <div class="close-selected">
                   <asp:ImageButton ID="ImageButton2" runat="server" src="https://cdn.wes.com.au/WAG/Images/close_btn.png" data-dismiss="modal" style="margin:10px" />     
                </div>
                <div class="modal-header green_bg">
                    <h4 id="H1" class="text-center">
                        <img class="popsucess" alt="img" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Order__info.png" />CVV Help</h4>
                                 
                </div>
           
                <div class="modal-body">
                    <h1>How to find the security code on a credit card</h1>
                    <p>Find out where to locate the security code on your credit card.</p>
                    <p><strong>Visa, MasterCard, Discover, JCB, and Diners Club</strong></p>
                    <p>The security code is a three-digit number on the back of your credit card, immediately following your main card number.</p>
                    <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Mcard.jpg" />
                    <p><strong>American Express</strong></p>
                    <p>The security code is a four-digit number located on the front of your credit card, to the right above your main credit card number.</p>
                    <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Acard.jpg" />
                    <p>If your security code is missing or illegible, call the bank or credit card establishment referenced on your card for assistance.</p>
                </div>
                <div class="modal-footer clear border_top_none">
                </div>
            </div>
        </div>
    </div>
            <script src="https://js.braintreegateway.com/web/3.47.0/js/client.min.js"></script>
        <script src="https://js.braintreegateway.com/web/dropin/1.18.0/js/dropin.min.js"></script>
       <script src="https://js.braintreegateway.com/web/3.47.0/js/three-d-secure.min.js"></script>
      <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
     <script>



        var payBtn = document.getElementById('pay-btn');
var nonceGroup = document.querySelector('.nonce-group');
var nonceInput = document.querySelector('.nonce-group input');
var payGroup = document.querySelector('.pay-group');
         var dropin;
        
         var totamt = $("#<%=lblAmount.ClientID%>").text();
         alert(totamt);
         var amt = parseFloat(totamt);
         alert(amt);
  braintree.dropin.create({
    authorization:"<%= this.ClientToken %>",
    container: '#drop-in',
    threeDSecure: {
      amount: amt
    }
  }, function(err, instance) {
    if (err) {
      console.log('component error:', err);
      return;
    }
    
    dropin = instance;

    setupForm();
  });


function setupForm() {
  enablePayNow();
}

function enablePayNow() {
  payBtn.value = 'Pay Now';
  payBtn.removeAttribute('disabled');
}

function showNonce(payload) {
    nonceInput.value = payload.nonce;

    document.getElementById("HiddenField1").value = payload.nonce; 
  payGroup.classList.add('hidden');
  payGroup.style.display = 'none';
    nonceGroup.classList.remove('hidden');


               
}

payBtn.addEventListener('click', function(event) {
  payBtn.setAttribute('disabled', 'disabled');
  payBtn.value = 'Processing...';

  dropin.requestPaymentMethod(function(err, payload) {
    if (err) {
      console.log('tokenization error:', err);
      dropin.clearSelectedPaymentMethod();
      enablePayNow();
      return;
    } else {
      console.log('initial tokenization success:', payload);
    }

    if (payload.type !== 'CreditCard') {
      // if not a credit card, skip 3ds and send nonce to server
      return;
    }
      
    if (!payload.liabilityShifted) {
      dropin.clearSelectedPaymentMethod();
      console.log('Liability did not shift', payload);
      enablePayNow();
      return;
    }

    console.log('verification success:', payload);
   
      SaleTrans(payload.nonce);
    // send nonce and verification data to your server
  });
});


    </script> 
     <script type="text/javascript" language="javascript">


            function SaleTrans(a) {
                try { 
          alert(a);

                $.ajax({
                    type: "POST",
                    url: "bt.aspx/SaleTrans",
                    data: "{'nounce':'" + a + "','Amount':'100'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        alert("x");
                       // showNonce(payload);
                    },
                    error: function (xhr, status, error) {

                        var err = eval("(" + xhr.responseText + ")");
                        alert(err);
                    }
                })
            }
            catch (err) {
alert(err)
            }
      }

      
</script>
    <script type="text/javascript">
        $(function () {

            var appendthis = ("<div class='modal-overlay js-modal-close'></div>");

            $('a[data-modal-id]').click(function (e) {
                e.preventDefault();
                $("body").append(appendthis);
                $(".modal-overlay").fadeTo(500, 0.7);
                //$(".js-modalbox").fadeIn(500);
                var modalBox = $(this).attr('data-modal-id');
                $('#' + modalBox).fadeIn($(this).data());
            });



            $(".js-modal-close, .modal-overlay").click(function () {
                $(".modal-box, .modal-overlay").fadeOut(500, function () {
                    $(".modal-overlay").remove();
                });

            });

            $(window).resize(function () {
                $(".modal-box").css({
                    //            top: ($(window).height() - $(".modal-box").outerHeight()) / 2,
                    //           left: ($(window).width() - $(".modal-box").outerWidth()) / 2
                });
            });

            $(window).resize();

        });
    </script>
  <%--  <script type="text/javascript">
        $(".address_update").on('click', function (e) {
            console.log("update buttton clicked");
            var btntype = $(this)[0].id;
            $.each($("input[name='radio_group']:checked"), function () {
                if (btntype == "shipAddress_updateBtn") {
                    $("#<%=hfShipAddress.ClientID%>").val($(this).val());
               }
               else if (btntype == "billAddress_updateBtn") {
                   $("#<%=hfBillAddress.ClientID%>").val($(this).val());
               }
           });

         });
        // Quick & dirty toggle to demonstrate modal toggle behavior
        $('.modal-toggle').on('click', function (e) {
            e.preventDefault();
            var addressType = "";
            var checkval = "";
            if ($(this)[0].id == "shipAddressBook") {
                addressType = "shipAddress";
                checkval = $("#<%=hfShipAddress.ClientID%>").val();
           }
           else if ($(this)[0].id == "billAddressBook") {
               addressType = "billAddress";
               checkval = $("#<%=hfBillAddress.ClientID%>").val();
           }
            if (addressType == "shipAddress" || addressType == "billAddress") {
                $(".address_update").attr('id', addressType + '_updateBtn');
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Express_Checkout.aspx/GetAddress",
                    data: "{'addressType':'" + addressType + "'}",
                    datatype: "json",
                    async: false,
                    success: function (response) {
                        if (response.d != null && response.d != "") {
                            var obj = JSON.parse(response.d);
                            $("#radio_group").empty();
                            var str = ""
                            console.log(obj.company);
                            if (obj.address.Table != null) {
                                $(obj.address.Table).each(function (index, res) {
                                    var address = "";
                                    if ($.trim(obj.company) != "") {
                                        address += $.trim(obj.company) + '\n';
                                    }
                                    if ($.trim(res.ADDRESS_LINE1) != "") {
                                        address += $.trim(res.ADDRESS_LINE1) + '\n';
                                    }
                                    if ($.trim(res.ADDRESS_LINE2) != "") {
                                        address += $.trim(res.ADDRESS_LINE2) + '\n';
                                    }
                                    if ($.trim(res.ADDRESS_LINE3) != "") {
                                        address += $.trim(res.ADDRESS_LINE3) + '\n';
                                    }
                                    if (res.CITY != "") {
                                        address += res.CITY + '\n';
                                    }
                                    if (res.STATE != "") {
                                        address += res.STATE + ' - ';
                                    }
                                    if (res.ZIP != "") {
                                        address += res.ZIP + '\n';
                                    }
                                    if (res.COUNTRY != "") {
                                        address += res.COUNTRY + '\n';
                                    }
                                    if (res.PHONE != "") {
                                        address += 'Phone No: ' + res.PHONE;
                                    }
                                    if (res.PRIMARY_ADDRESS == 1) {
                                        checkval = (index + 1);
                                    }
                                  
                                    str += '<label class="address_label" for="address' + (index + 1) + '">Address ' + (index + 1) + ' </label>';
                                    str += '<input type="radio" name="radio_group" id="address' + (index + 1) + '" class="address_radio" value="' + (index + 1) + '" />';
                                    str += '<textarea class="dblock" rows="6" readonly="true">' + address + '</textarea>';
                                    //if (checkval == (index + 1))
                                    //{
                                    //    alert(checkval);
                                    //    $('#address'+ (index + 1)).prop('checked', true);
                                    //}
                                    //<label class="address_label" for="address1">Address 1 </label>
                                    //  <input type="radio" name="radio_group" id="address1" class="address_radio" />
                                    //  <textarea class="dblock"></textarea>
                                    //          console.log(res.ADDRESS_LINE1);
                                });
                                //      console.log(str);
                                $("#radio_group").append(str);

                            }
                        }
                    },
                    error: function (err) {
               //         alert("error");
                        //        window.location.href = "/Home.aspx";
                        return false;
                    }
                });
            }
            $('#address' + checkval).prop('checked', true);
            $('.address_book').toggleClass('is-visible');
            if ($(".address_book").hasClass('is-visible')) {
                var appendthis = ("<div class='modal-overlay js-modal-close'></div>");
                $("body").append(appendthis);
                $(".modal-overlay").fadeTo(500, 0.7);
            }
            else {
                $(".modal-box, .modal-overlay").fadeOut(500, function () {
                    $(".modal-overlay").remove();
                });

            }
            
        });
</script>--%>


</asp:Content>
