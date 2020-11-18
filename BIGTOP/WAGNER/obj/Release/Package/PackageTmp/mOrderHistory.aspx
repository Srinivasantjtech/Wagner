<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mOrderHistory.aspx.cs" Inherits="WES.mOrderHistory" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>  
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--  <script language="javascript" type="text/javascript" src="/Scripts/jquery-1.5.1.min.js"></script>--%>
    <script language="javascript" type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/jquery-ui-1.8.13.custom.min.js"></script>
    <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />
    <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/progress.js" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $(".fancybox").fancybox();

       
    });
</script>
<script language="javascript" type="text/javascript">
    window.onresize = function () {
        if ($('#mobdiv').css('display') == 'none') {
            $(".fancybox-skin").css({ 'display': 'none' });
            $(".fancybox-overlay").css({ 'display': 'none' });
        }
    } 

</script>

<script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/MicroSitejs/jquery.fancybox.js" type="text/javascript" language="javascript"></script>
<script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/MicroSitejs/jquery.easing-1.3.pack.js" type="text/javascript" language="javascript"></script>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <script language="javascript" type="text/javascript">
        var Flag = false;

        $(document).ready(function msg() {

            $("ProcessDiv").hide();

            if (Flag == false) {

            }
            $(".HyperLink2").click(function () {

                if (Flag == true) {
                    $("ProcessDiv").show();


                }
            });
        });

        function ShowDiv(obj) {
            var dataDiv = document.getElementById(obj);
            dataDiv.style.display = "block";
            alert("h2");
        }

        function countdown_clear() {
            clearTimeout(countdown);
        }


        function ShowFileMessage() {
            alert('Invalid Specification!');
        }

        function showalert(Url) {
            Flag = confirm('Do you want to proceed the download?');
            if (Flag == true) {
                //alert(Url);
                window.location.href = "OrderHistory.aspx?Key=" + Url;
            }
            return flag;
        }

        function ShowFailureMessage() {
            alert('Invoice currently Unavailable. Please try again later');
        }

        function ShowLoader() {
            setTimeout("setImage();", 500);
            window.document.getElementById('ctl00_maincontent_DivLoader').style.display = 'none';
        }

        function setImage() {
            window.document.getElementById('ctl00_maincontent_DivLoader').style.display = 'block';
        }

        function PopupOrder(url) {
            newwindow = window.open(url, 'name', 'scrollbars=1,left=130,top=50,height=530,width=700');
            if (window.focus) {
                newwindow.focus();
            }
            return false;
        }

        function payonlinepopup_open(url) {

        }





        function initinvoicereq() {
            var ret = confirm("Do you want to proceed")
            return ret;
        }
        $(function () {
            $("#<%=FromdateTextBox.ClientID %>").removeClass();
            $("#<%=FromdateTextBox.ClientID %>").datepicker({

                beforeShow: function (input, inst) {
                    var d1 = new Date();
                    d1 = d1.format('MM/dd/yyyy');
                    var sub = d1.split('/');
                    var mon = sub[0] - 1;
                    var Newdate = new Date(mon + "/" + sub[1] + "/" + sub[2]);
                    Newdate = Newdate.format('dd/MM/yyyy');
                    //alert(Newdate);
                    if (document.getElementById("ctl00_MainContent_FromdateTextBox").value == null ) {
                        document.getElementById("ctl00_MainContent_FromdateTextBox").value = Newdate;
                    }
                },
                defaultDate: '-1m',
                maxDate: "+0M +0D",
                dateFormat: 'dd/mm/yy',
                showOn: "button",     //false,                          
                buttonImage: "<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Calendar2.PNG",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true
            });

            $("#<%=TodateTextBox.ClientID %>").removeClass();
            $("#<%=TodateTextBox.ClientID %>").datepicker({
                maxDate: "+0M +0D",
                dateFormat: 'dd/mm/yy',
                showOn: "button",
                buttonImage: "<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Calendar2.PNG",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
    <script language="javascript" type="text/javascript">
        function Focus() {
            //alert('dd');
            SearchText1();
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

        });
        function SearchText() {

            $('a.HyperLink2').click({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "OrderHistory.aspx/WestestAutoCompleteData",
                        //  data: "{'username':'" + document.getElementById(ctl).value + "'}",
                        data: "{'strvalue':'A'}",
                        dataType: "json",
                        success: function (data) {
                            //alert("success");
                            response(data.d);
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                }
            });
        }
        function SearchText1() {
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "OrderHistory.aspx/WestestAutoCompleteData",
                //  data: "{'username':'" + document.getElementById(ctl).value + "'}",
                data: "{'strvalue':'A'}",
                dataType: "json",
                success: function (data) {
                    //alert("success");
                    response(data.d);
                },
                error: function (result) {
                    alert("Error");
                }
            });
        }
        function asyncServerCall(userid) {
            jQuery.ajax({
                url: 'WebForm1.aspx/GetData',
                type: "POST",
                data: "{'userid':" + userid + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    alert(data.d);
                }

            });
        }

	</script>
    <script type="text/javascript" language="javascript">
        function toggle(inv, dis) {
            var e = document.getElementById(inv);
            e.style.display = dis;
        }
        function GetInvoice(OrderInv, inv) {

            toggle(inv, "")
            PageMethods.SendInvoiceSignal(OrderInv, OnSuccess, OnFailure);
        }
        function OnSuccess(result) {
            if (result.indexOf("inv") != -1) {
                var invno = result.replace("inv", "");

                document.getElementById('<%=ImageButton1.ClientID %>').click();
                toggle(invno, "none")
            }
            else {
                var invno1 = result.replace("inv", "");
                toggle(invno1, "none")
                alert("Invoice PDF temopary not Available.Please try again later");
            }

        }
        function OnFailure(error) {
            var invno = result.replace("inv", "");
            toggle(invno, "none")
            alert("Invoice PDF temopary not Available.Please try again later");
            //alert(error);

        }
</script>
  
    <div class="grid12 hidden-phone">
    <%  %>
        <div class="mar5 breadcrumb"> <a style="color:#089fd6 !important" href="<%=micrositeurl%>">Home</a> &gt;<a style="color:#089fd6 !important" href="/morderhistory.aspx"> OrderHistory</a></div>
      </div>
    <div class="grid12">
    <div class="mar5">
    <div class="signup blue border-rad5 ourcont">
    <h3>Order History</h3>
<h5>Filter Orders By:</h5>
<div class="fltrcont">
<div class="grid3">
              	<div class="grid12">Order No or Invoice No</div>
                <div class="cl"></div> 
             
                <div>
            <%--    <input type="text" class="field" name="">--%>
             <asp:TextBox runat="server" ID="OrderNo" Width="72%" BackColor="White" CssClass="field" 
                                 BorderWidth="1px"></asp:TextBox>
                </div>
              </div>
              <div class="grid2">
              	<div class="grid12">From Date</div>
                <div class="cl"></div> 
                <div>
               <asp:TextBox runat="server" ID="FromdateTextBox" Width="72%"  BackColor="White" Height="25px" CssClass="field" 
                                BorderWidth="1px"></asp:TextBox>
                </div>
              </div>
              <div class="grid2">
              	<div class="grid12">To Date</div>
                <div class="cl"></div> 
                <div>
                    <asp:TextBox runat="server" ID="TodateTextBox" Width="72%"  BackColor="White" Height="25px" CssClass="field" 
                                 BorderWidth="1px"></asp:TextBox>
                </div>
              </div>
              <div class="grid2">
              	<div class="grid12">Created User</div>
                <div class="cl"></div> 
                <div>
                    <asp:DropDownList runat="server" ID="CreatedUserDropDownlist" BackColor="White"  CssClass="inputtxt" Width="100%">
                            </asp:DropDownList>
                </div>
              </div>
              <div class="btnalign">
                   <asp:Button runat="server" ID="SearchButton"  CssClass="btn"
                                Text="Search" OnClick="SearchButton_Click" />
                    <asp:Button runat="server" ID="ResetButton"  CssClass="btn"
                                Text="Reset"  OnClick="ResetButton_Click" />    
                              <%--   <asp:Button ID="btnHidden" runat="server" CssClass="HiddenButtonOrdhis" />--%>
                <div class="cl"></div>
              </div>
              <div class="cl"></div>
              <div class="records" style="overflow:hidden;">
              <div class="desk">
             
                       <% 
                    try
                    {
                        HelperServices objHelperServices = new HelperServices();
                        Security objSecurity = new Security();
                        
                        OrderServices objOrderServices = new OrderServices();
                        DataTable oDt = null;
                        //int Companyid = Convert.ToInt16(Session["COMPANY_ID"]);
                        int Companyid = Convert.ToInt32(Session["COMPANY_ID"]);

                        if (OrderNoHiddenField.Value.Trim() != "" || FromDateHiddenField.Value.Trim() != "" || ToDateHiddenField.Value.Trim() != "" || UserHiddenField.Value.Trim() != "")
                        {
                            DateTime Fromdate1 = new DateTime();
                            DateTime Todate1 = new DateTime();

                            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US", true);

                            if (FromDateHiddenField.Value.Trim() != "")
                            {
                               
                            }
                            else
                            {
                                FromDateHiddenField.Value = null;
                            }

                            if (ToDateHiddenField.Value.Trim() != "")
                            {
                               
                            }
                            else
                            {
                                ToDateHiddenField.Value = null;
                            }

                            if (OrderNoHiddenField.Value.Trim() == "")
                            {
                                OrderNoHiddenField.Value = null;
                            }

                            if (UserHiddenField.Value.Trim() == "")
                            {
                                UserHiddenField.Value = null;
                            }
                                                                                    
                            oDt = objOrderServices.GetFilteredOrderHistory(OrderNoHiddenField.Value, FromDateHiddenField.Value, ToDateHiddenField.Value, UserHiddenField.Value, Companyid);                            
                        }
                        else
                        {
                            oDt = objOrderServices.GetOrderHistory();
                        }

                        if (oDt != null && oDt.Rows.Count > 0)
                        {
                %>
              <table class="tblbdr" width="100%">
              <thead>
              <tr>
              <th>Cust. Order Date</th>
                  <th>Cust. Order No.</th>
                  <th>Invoice Date</th>
                  <th>Invoice No</th>
                  <th>User</th>
                  <th>Order Status</th>
                  <th>Shipping Track &amp; Trace</th>
                  <th>Submitted Order</th>
                  <th>View Invoice</th>
                </tr>
              </thead>
               <tbody>
                 <%  
                            string bgcolor = "";
                            bool bodrow = true;
                            int i = 1;
                            foreach (DataRow oDr in oDt.Rows)
                            {
                               // bgcolor = bodrow ? "white" : "#e7efef";
                               // bodrow = !bodrow;
                                if (i % 2 == 1)
                                    bgcolor = "tdwhite";
                                else
                                    bgcolor = "tdgrey";
                                i++;
                    %>
             
              <tr>
               <td class="<%=bgcolor%>">
                            <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["Order Date"])%>
                        </td>
                        <td class="<%=bgcolor%>">
                            <%=oDr["Cust.Order No"].ToString()%>
                        </td>
                        <td class="<%=bgcolor%>">
                            <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["Modified Date"])%>
                        </td>
                        <td class="<%=bgcolor%>">
                            <%=oDr["Invoice No"].ToString()%>
                        </td>
                        <td class="<%=bgcolor%>">
                            <%=oDr["User"].ToString()%>
                        </td>
                        <td class="<%=bgcolor%>">
                            <%=oDr["Order Status"].ToString()%>
                        </td>
                        <td class="<%=bgcolor%>">
                            <font color="#3399FF">
                            <% 
                                if (!string.IsNullOrEmpty(oDr["SHIPTRACKURL"].ToString().Trim()))
                                { %>
                                <img id="ExternalLinkImg" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/External_Link.gif" height="12px" width="12px" alt="" /> 
                                &nbsp;
                                <a href="<% = oDr["SHIPTRACKURL"].ToString() %>" style="color: #0099da; text-decoration:none;"
                                    onclick="window.open('<% = oDr["SHIPTRACKURL"].ToString() %>','popup','width=800,height=600,scrollbars=yes,resizable=yes,toolbar=no,directories=no,location=no,menubar=no,modal=yes,status=yes,left=150,top=25'); return false">
                                    <%=oDr["Shipping Track & Trace"].ToString().Trim() %> </a>
                                 <% }
                                else if (!string.IsNullOrEmpty(oDr["Shipping Track & Trace"].ToString().Trim()))
                                {%>
                                   <%=oDr["Shipping Track & Trace"].ToString().Trim()%>
                                  <%}
                                else if (oDr["CC_PAY_RESPONSE"].ToString().ToLower() != "yes" && (oDr["Order Status"].ToString().Trim() == "Payment Required" || oDr["Order Status"].ToString().Trim() == "Proforma Payment Required"))
                                {                                    
                                    %>
                                      
                                    <a id="A1" style="color: #3399FF" href="mcheckout.aspx?<%= EncryptSP(oDr["ORDERID"].ToString() + "#####" + "Pay" ) %>">
                                          <img id="Img1" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/makepayment.png" height="25px" width="100px" alt="" />
                                     </a>
                                 <%                                 
                                }
                                    %>
                            </font>
                        </td>
                        <td class="<%=bgcolor%>">
                            <a id="HyperLink1" style="color: #3399FF" href="OrderReport.aspx?OrdId=<% = oDr["ORDERID"].ToString() %>"
                                onclick="return PopupOrder('OrderReport.aspx?OrdId=<% = oDr["ORDERID"].ToString() %>')">
                                View Order </a>
                        </td>
                        <td class="<%=bgcolor%>">
                            <%
                               
                                if (oDr["Invoice No"].ToString() != "" && (oDr["Order Status"].ToString().Trim() == "Payment Required" || oDr["Order Status"].ToString().Trim() == "Payment Successful" || oDr["Order Status"].ToString().Trim() == "Processing Order" || oDr["Order Status"].ToString().Trim() == "Order Shipped" || oDr["Order Status"].ToString().Trim() == "Completed" || oDr["Order Status"].ToString().Trim() == "Shipped" || oDr["Order Status"].ToString().Trim() == "Proforma Payment Required"))
                                {
                                    string InvNo= oDr["Invoice No"].ToString();
                                    string EncryptKeyString = string.Format("{0}|{1}", oDr["ORDERID"].ToString(), oDr["Invoice No"].ToString().Trim());
                                    string PdfNameValue = objSecurity.Encrypt(EncryptKeyString, Session.SessionID);
                                    string pdfn = Server.HtmlEncode(PdfNameValue).ToString();
                            %>
                        

                                
                         
                                <a class="HyperLink2" id="A2" style="color: #3399FF; cursor: pointer;" onclick="GetInvoice('<%=EncryptKeyString%>','<%=InvNo%>');" >
                                 <%
                                     if (oDr["Order Status"].ToString().Trim() != "Proforma Payment Required")
                                     {
                                     %>
                                 View Invoice 
                                 <%
                                     }
                                     else
                                     { %>
                                     View Proforma Invoice
                                     <%} %>
                                 </a>                               
                                <div id="<%=InvNo%>" style="display:none;" ><img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Invloading.gif"/ ></div>
                                <asp:ImageButton ID="ImageButton1" runat="server" onclick="cmdUpdateField_Click"    style="display:none;" ></asp:ImageButton>

                               
                            
                            <% } %>
                        </td>




                        </tr>
                          <% } %>
              </tbody>
              </table>
              

                   <%
                        }
                        else
                        {
                            MsgLabel.Text = "No Records Found!";
                            MsgLabel.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgLabel.Text = ex.Message;
                        MsgLabel.Visible = true;
                    }
                %>

              </div>
              </div>
                <div class="cl"></div>
                 <div class="mob" id="mobdiv">
                                <% 
                    try
                    {
                        HelperServices objHelperServices = new HelperServices();
                        Security objSecurity = new Security();
                        
                        OrderServices objOrderServices = new OrderServices();
                        DataTable oDt = null;
                        //int Companyid = Convert.ToInt16(Session["COMPANY_ID"]);
                        int Companyid = Convert.ToInt32(Session["COMPANY_ID"]);

                        if (OrderNoHiddenField.Value.Trim() != "" || FromDateHiddenField.Value.Trim() != "" || ToDateHiddenField.Value.Trim() != "" || UserHiddenField.Value.Trim() != "")
                        {
                            DateTime Fromdate1 = new DateTime();
                            DateTime Todate1 = new DateTime();

                            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US", true);

                            if (FromDateHiddenField.Value.Trim() != "")
                            {
                               
                            }
                            else
                            {
                                FromDateHiddenField.Value = null;
                            }

                            if (ToDateHiddenField.Value.Trim() != "")
                            {
                               
                            }
                            else
                            {
                                ToDateHiddenField.Value = null;
                            }

                            if (OrderNoHiddenField.Value.Trim() == "")
                            {
                                OrderNoHiddenField.Value = null;
                            }

                            if (UserHiddenField.Value.Trim() == "")
                            {
                                UserHiddenField.Value = null;
                            }
                                                                                    
                            oDt = objOrderServices.GetFilteredOrderHistory(OrderNoHiddenField.Value, FromDateHiddenField.Value, ToDateHiddenField.Value, UserHiddenField.Value, Companyid);                            
                        }
                        else
                        {
                            oDt = objOrderServices.GetOrderHistory();
                        }

                        if (oDt != null && oDt.Rows.Count > 0)
                        {
                %>
              <table class="tblbdr" width="100%">
              <thead>
              <tr><th>Cust. Order Date</th>
                  <th>Order Status</th>
                  <th>View More Details</th>  
                </tr>
              </thead>
               <tbody>
                 <%  
                            string bgcolor = "";
                            bool bodrow = true;
                            int i = 1;
                            foreach (DataRow oDr in oDt.Rows)
                            {
                               // bgcolor = bodrow ? "white" : "#e7efef";
                               // bodrow = !bodrow;
                                if (i % 2 == 1)
                                    bgcolor = "tdwhite";
                                else
                                    bgcolor = "tdgrey";
                                i++;
                    %>
             
              <tr>
               <td class="<%=bgcolor%>">
                            <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["Order Date"])%>
                        </td>
                        <td class="<%=bgcolor%>">
                            <%=oDr["Order Status"].ToString()%>
                        </td>
                       	<td class="tdwhite">
                         <a class="fancybox" rel="group" href="#pop<%= oDr["Cust.Order No"].ToString()%>">
                        <%-- <a class="fancybox" rel="group" href="#pop">--%>
                           <input type="button" class="btn widh" value="View More" name="" />
                         </a>                        
                        </td>
                        </tr>
                          <% } %>
              </tbody>
              </table>
              

                   <%
                        }
                        else
                        {
                            MsgLabel.Text = "No Records Found!";
                            MsgLabel.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgLabel.Text = ex.Message;
                        MsgLabel.Visible = true;
                    }
                %>
                 </div>
</div>
    <asp:Label runat="server" ID="MsgLabel" Width="250px"> &nbsp; </asp:Label>
    
<%--<div id="pop">--%>


                   <% 
                    try
                    {
                        HelperServices objHelperServices = new HelperServices();
                        Security objSecurity = new Security();
                        
                        OrderServices objOrderServices = new OrderServices();
                        DataTable oDt = null;
                        //int Companyid = Convert.ToInt16(Session["COMPANY_ID"]);
                        int Companyid = Convert.ToInt32(Session["COMPANY_ID"]);

                        if (OrderNoHiddenField.Value.Trim() != "" || FromDateHiddenField.Value.Trim() != "" || ToDateHiddenField.Value.Trim() != "" || UserHiddenField.Value.Trim() != "")
                        {
                            DateTime Fromdate1 = new DateTime();
                            DateTime Todate1 = new DateTime();

                            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US", true);

                            if (FromDateHiddenField.Value.Trim() != "")
                            {
                               
                            }
                            else
                            {
                                FromDateHiddenField.Value = null;
                            }

                            if (ToDateHiddenField.Value.Trim() != "")
                            {
                               
                            }
                            else
                            {
                                ToDateHiddenField.Value = null;
                            }

                            if (OrderNoHiddenField.Value.Trim() == "")
                            {
                                OrderNoHiddenField.Value = null;
                            }

                            if (UserHiddenField.Value.Trim() == "")
                            {
                                UserHiddenField.Value = null;
                            }
                                                                                    
                            oDt = objOrderServices.GetFilteredOrderHistory(OrderNoHiddenField.Value, FromDateHiddenField.Value, ToDateHiddenField.Value, UserHiddenField.Value, Companyid);                            
                        }
                        else
                        {
                            oDt = objOrderServices.GetOrderHistory();
                        }

                        if (oDt != null && oDt.Rows.Count > 0)
                        {
                %>
                 <%  
                            string bgcolor = "";
                            bool bodrow = true;
                            int i = 1;
                            int j = 1;
                            string strheading = "";
                            string strrowvalue = "";
                            foreach (DataRow oDr in oDt.Rows)
                            {
                              
                                //if (i % 2 == 1)
                                //    bgcolor = "tdwhite";
                                //else
                                //    bgcolor = "tdgrey";
                                //i++;

                                
                                 
                    %>
                    <div id="pop<%= oDr["Cust.Order No"].ToString()%>" style="display:none;">
                          <table width="100%" class="tblbdr">
                    <thead>
                      <th class="alnpdg">Cust. Order Date</th>                  
                    </thead>
                        <tr>
                            <td class="tdwhite alnpdg" height="10px"><%= string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["Order Date"])%></td>                       
                        </tr>  
                        </table>
                        <table width="100%" class="tblbdr">
                          <thead>
                      <th class="alnpdg">Cust. Order No.</th>                  
                    </thead>
                        <tr>
                            <td class="tdwhite alnpdg" height="10px"><%= oDr["Cust.Order No"].ToString()%></td>                       
                        </tr>                       
                  </table>
                      <table width="100%" class="tblbdr">
                          <thead>
                      <th class="alnpdg" >Invoice Date</th>                  
                    </thead>
                        <tr>
                            <td class="tdwhite alnpdg" height="10px"><%= string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["Modified Date"])%></td>                       
                        </tr>                       
                  </table>
                      <table width="100%" class="tblbdr">
                          <thead>
                      <th class="alnpdg">Invoice No</th>                  
                    </thead>
                        <tr>
                            <td class="tdwhite alnpdg" height="10px"><%= oDr["Invoice No"].ToString()%></td>                       
                        </tr>                       
                  </table>
                     <table width="100%" class="tblbdr">
                          <thead>
                      <th class="alnpdg">User</th>                  
                    </thead>
                        <tr>
                            <td class="tdwhite alnpdg" height="10px"><%= oDr["User"].ToString()%></td>                       
                        </tr>                       
                  </table>
                      <table width="100%" class="tblbdr">
                          <thead>
                      <th class="alnpdg">Order Status</th>                  
                    </thead>
                        <tr>
                            <td class="tdwhite alnpdg" height="10px"><%= oDr["Order Status"].ToString()%></td>                       
                        </tr>                       
                  </table>
                      <table width="100%" class="tblbdr">
                          <thead>
                      <th class="alnpdg">Shipping Track &amp; Trace</th>                  
                    </thead>
                        <tr>
                            <td class="tdwhite alnpdg" height="10px"><%= oDr["Shipping Track & Trace"].ToString().Trim()%></td>                       
                        </tr>                       
                  </table>
                      <table width="100%" class="tblbdr">
                          <thead>
                      <th class="alnpdg">Submitted Order</th>                  
                    </thead>
                        <tr>
                            <td class="tdwhite alnpdg" height="10px">
                            <%--View Order--%>
                             <a id="A3" style="color: #3399FF" href="OrderReport.aspx?OrdId=<% = oDr["ORDERID"].ToString() %>"
                                onclick="return PopupOrder('OrderReport.aspx?OrdId=<% = oDr["ORDERID"].ToString() %>')">
                                View Order </a>
                            
                            </td>                       
                        </tr>                       
                  </table>
                       <table width="100%" class="tblbdr">
                          <thead>
                      <th class="alnpdg">View Invoice</th>                  
                    </thead>
                        <tr>
                            <td class="tdwhite alnpdg" height="10px">
                           <%-- View Invoice--%>
                             <%
                               
                                if (oDr["Invoice No"].ToString() != "" && (oDr["Order Status"].ToString().Trim() == "Payment Required" || oDr["Order Status"].ToString().Trim() == "Payment Successful" || oDr["Order Status"].ToString().Trim() == "Processing Order" || oDr["Order Status"].ToString().Trim() == "Order Shipped" || oDr["Order Status"].ToString().Trim() == "Completed" || oDr["Order Status"].ToString().Trim() == "Shipped" || oDr["Order Status"].ToString().Trim() == "Proforma Payment Required"))
                                {
                                    string InvNo= oDr["Invoice No"].ToString();
                                    string EncryptKeyString = string.Format("{0}|{1}", oDr["ORDERID"].ToString(), oDr["Invoice No"].ToString().Trim());
                                    string PdfNameValue = objSecurity.Encrypt(EncryptKeyString, Session.SessionID);
                                    string pdfn = Server.HtmlEncode(PdfNameValue).ToString();
                            %>
                        

                                
                         
                                <a class="HyperLink2" id="A4" style="color: #3399FF; cursor: pointer;" onclick="GetInvoice('<%=EncryptKeyString%>','<%=InvNo%>');" >
                                 <%
                                     if (oDr["Order Status"].ToString().Trim() != "Proforma Payment Required")
                                     {
                                     %>
                                 View Invoice 
                                 <%
                                     }
                                     else
                                     { %>
                                     View Proforma Invoice
                                     <%} %>
                                 </a>                               
                                <div id="Div1" style="display:none;" ><img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Invloading.gif"/ ></div>
                                <asp:ImageButton ID="ImageButton2" runat="server"     style="display:none;" ></asp:ImageButton>

                               
                            
                            <% } %>
                            
                            
                            </td>                       
                        </tr>                       
                  </table>
                           </div>
                    <% } %>
         
              

                   <%
                        }
                        else
                        {
                            MsgLabel.Text = "No Records Found!";
                            MsgLabel.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgLabel.Text = ex.Message;
                        MsgLabel.Visible = true;
                    }
                %>


                               	
            <%--  </div>--%>
    </div>
       <div class="cl"></div>
    </div>
    </div>

    
     
   


    <%--<div id="PopDiv" class="containerOrdhis">
        <asp:Panel ID="SignalPopupPanel" runat="server" Style="display: none;" CssClass="ModalPopupStyleOrdhis">
            <div class="containerOrdhis">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                        <td width="10px">
                        <asp:ImageButton ID="ImageButton2" runat="server" OnClick="btncancel_Click" ImageUrl="~/images/btn_images/11_32_2.png" ImageAlign="Right" />                        
                         </td>
                        
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOrdhis" colspan="2" style="font-size: medium; font-weight: bold">
                          &nbsp;&nbsp;Your Invoice has been generated.
                        <br/>
                        </td>
                        
                        
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOrdhis" colspan="3">
                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Click here to Download.." OnClick="btnClose_Click"  Font-Size="Medium" Font-Underline="True" ForeColor="Maroon" Font-Bold="False" />
                           
                        </td>
                        
                        
                    </tr>
           
                </table>
            </div>
        </asp:Panel>
    </div>
    <div id="PopDiv1" class="containerOrdhis">
        <asp:Panel ID="SignalProcessPanel" runat="server" Style="display: none;" CssClass="ModalPopupStyleOrdhis">
            <div class="containerOrdhis">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
                    <tr class="TableColumnStyleOrdhis">
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOrdhis">
                           Invoice PDF temopary not Available. 
                            <br />
                            Please try again later
                        </td>
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOrdhis">
                         
                            <asp:Button ID="Button1" runat="server" Text="Close"  OnClick="btncancel_Click" CssClass="ButtonStyleOrdhis"/>                            
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>

    
    </div>
     
     <div id="Div1" class="containerOrdhis">
        <asp:Panel ID="Panel1" runat="server" Style="display: none;" CssClass="ModalPopupStyleOrdhis">
            <div class="containerOrdhis">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">                   
                    <tr>
                        <td class="TableColumnStyleOrdhis">
                            <asp:Button ID="ExitButton" runat="server" Text="Close"   OnClick="btncancel_Click" CssClass="ButtonStyleOrdhis"/>                            
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel> 
    </div>
    <div id="ProcessDiv" class="containerOrdhis" runat="server"> 
         <asp:Panel ID="ShowProcessPanel" runat="server" Style="display: none;"  CssClass="ModalPopupStyleOrdhis">
         <div class="containerOrdhis">
                   <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
                    <tr class="TableColumnStyleOrdhis">
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOrdhis">
                           Please wait for a moment.
                            <br />
                            Invoice will be available shortly
                        </td>
                    </tr>
                   </table>          
    </div>
        </asp:Panel>
       
    </div>--%>
    <asp:HiddenField ID="FromDateHiddenField" runat="server"/>
    <asp:HiddenField ID="ToDateHiddenField" runat="server" />
    <asp:HiddenField ID="UserHiddenField" runat="server" />
    <asp:HiddenField ID="OrderNoHiddenField" runat="server" />


       <asp:Timer ID="Timer1" runat="server" OnTick="timer1_Tick" Interval="3000" Enabled="false" />
  <asp:updatepanel id="updatepaneltimer" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick"/>
         </Triggers>
   
    
       </asp:updatepanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
