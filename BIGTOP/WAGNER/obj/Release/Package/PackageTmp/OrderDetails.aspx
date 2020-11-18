<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="OrderDetails" Title="Untitled Page"
     Culture="en-US" UICulture="en-US" Codebehind="OrderDetails.aspx.cs" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="GCheckout" Namespace="GCheckout.Checkout" TagPrefix="GCCheckout" %>
<%@ Register Src="UC/BulkOrder.ascx" TagName="BulkOrder" TagPrefix="uc1" %>

 
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="false" EnableCdn="true" EnablePageMethods="true" ScriptMode="Release"  EnablePartialRendering="false" >
        </asp:ScriptManager>
<%if (Request.QueryString["popup"] == null)
  { %>
  <div class="container">
  <div class="row hidden-xs hidden-sm">
    	<div class="col-sm-20">
        	<ul id="mainbredcrumb" class="breadcrumb">
            	<li><a href="/home.aspx">Home</a></li>
                <li class="active">View Cart</li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlEnter" runat="server" DefaultButton="lblOrderProceed">
       <%-- <link rel="Stylesheet" href="css/thickboxNew.css" type="text/css" />
        <script language="javascript" src="Scripts/thickboxNew.js" type="text/javascript" />
        <script language="javascript" src="Scripts/jquery-Thickbox-New.js" type="text/javascript" />--%>
        <script language="javascript" type="text/javascript">
            var pid = "";
            var ProdPrice = "";
            var UntPrice = 0;
            var pgValidated = "no";

            //window.history.forward(1);
            function PopupAdd(FamilyID, ProductID, MinQty, MaxQty, refocus) {
                var Qty = window.open("ProductFeatures.aspx?Fid=" + FamilyID + "&Pid=" + ProductID, "Qty", "width=350,height=250,left=150,top=200,resizable=yes,scrollbars=1");
                if (Qty >= MinQty) {
                   
                    var href = window.parent.location = "/OrderDetails.aspx?&bulkorder=1&Pid="+ProductID+"&Qty="+document.forms[0].elements["Returnname"].value;
                }
            }

            function CheckDelete() {
                //  alert("Wanna Remove?");
                document.getElementById('ImageButton2').click();
            }

            function confirmDelete(e) {
                var targ;

                if (!e) var e = window.event;
                targ = (e.target) ? e.target : e.srcElement;
                if (targ.nodeType == 3) targ = targ.parentNode;

                if (targ.id.toLowerCase().indexOf("delete") >= 0) {
                    return confirm("Do you want to delete this item?");
                }
                //routeEvent(e);
            }
            document.onclick = confirmDelete;

            function BuildValue(Productid, Qty1, Pprice, Id) {
                var rst;
                if (document.forms[0].elements["Chk" + Id] != null) {

                    if (document.forms[0].elements["Chk" + Id].checked) {
                        pid = pid + "," + Productid;
                        ProdPrice = ProdPrice + "," + Pprice;
                        UntPrice += Pprice;
                        var elem = document.forms[0].elements
                        var c = 0;
                        for (var i = 0; i < elem.length; i++) {
                            var ObjName = "Chk" + i;
                            if (elem[i].type == "checkbox") {
                                if (elem[i].name == "Chk" + c) {
                                    c += 1
                                }
                            }
                        }

                        var ObjChkStatus = 0;
                        for (var j = 0; j < c; j++) {
                            var Oname = "Chk" + j;
                            if (document.forms[0].elements[Oname].checked == true) {
                                ObjChkStatus += 1;
                            }
                        }

                        if (c == ObjChkStatus) {
                            document.forms[0].elements["ChkAllSel"].checked = true;
                        }
                    }
                    else {

                        pid = pid.replace("," + Productid, "");
                        ProdPrice = ProdPrice.replace("," + Pprice, "");
                        if (UntPrice > 0) {
                            UntPrice -= Pprice;
                        }
                        if (document.forms[0].elements["ChkAllSel"].checked == true) {
                            document.forms[0].elements["ChkAllSel"].checked = false;
                        }
                    }
                }
            }
            function Send() {
                window.location.href = "/OrderDetails.aspx";
                if (pid != "") {
                    pid = pid.substr(1, pid.length);
                }
                else {
                    pid = 'AllProd';
                    UntPrice = '0';
                    ProdPrice = "";
                }
                window.location.href = '/OrderDetails.aspx?&bulkorder=1&SelPid='+pid+'&SelProdPrice='+UntPrice;
            }
            function validateNumber(event) {
                var key = window.event ? event.keyCode : event.which;

                if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 9
                               || event.keyCode == 37 || event.keyCode == 39) {
                    return true;
                }
                else if (key < 48 || key > 57) {
                    return false;
                }
                else return true;
            }
            function UpdQuantity() {
                document.getElementById('updCart').click();
            }

          

            function SendRemoveProducts() {
             
                if (String(pid) != "") {
                    pid = pid.substr(1, pid.length);
                    ProdPrice = ProdPrice.substr(1, ProdPrice.length);
                    window.location.href = "/OrderDetails.aspx?&bulkorder=1&SelPid="+pid+"&SelProdPrice="+UntPrice+"&ProdPrice="+ProdPrice;
                    return true;
                }
                else {
                    UntPrice = 0;
                    if (document.forms[0].elements["ChkAllSel"].checked == true) {
                        pid = "AllProd";
                        SelProdPrice = 0;
                        ProdPrice = "";
                        window.location.href = "/OrderDetails.aspx?&bulkorder=1&SelPid=AllProd";
                        return true;
                    }
                    else {
                        var elem = document.forms[0].elements;
                        var c = 0;
                        //Get the check box count 
                        for (var i = 0; i < elem.length; i++) {
                            if (elem[i].type == "checkbox") {
                                if (elem[i].name == "Chk" + c) {
                                    c += 1;
                                }
                            }
                        }
                        for (var j = 0; j < c; j++) {
                            var Oname = "Chk" + j;
                            if (document.forms[0].elements[Oname].checked == true) {
                                pid = pid + "," + parseInt(document.forms[0].elements[Oname].value);
                                UntPrice += parseFloat(document.forms[0].elements["txtPrdTprice" + j].value);
                                ProdPrice = ProdPrice + "," + parseFloat(document.forms[0].elements["txtPrdTprice" + j].value);
                            }
                        }
                        if (String(pid) != "" && document.forms[0].elements["ChkAllSel"].checked == false) {
                            pid = pid.substr(1, pid.length);
                            ProdPrice = ProdPrice.substr(1, ProdPrice.length);
                            window.location.href = "/OrderDetails.aspx?&bulkorder=1&SelPid="+pid+ "&SelProdPrice="+UntPrice+"&ProdPrice="+ProdPrice;
                            return true;
                        }
                    }
                }
            }

            function DelayR() {
                setTimeout('SendRemoveProducts()', 6000);
            }

            function CheckSelectAll() {
                var elem = document.forms[0].elements
                var c = 0;
                for (var i = 0; i < elem.length; i++) {
                    var ObjName = "Chk" + i;
                    if (elem[i].type == "checkbox") {
                        if (elem[i].name == "Chk" + c) {
                            c += 1
                        }
                    }
                }

                for (var j = 0; j < c; j++) {
                    var Oname = "Chk" + j;
                    if (document.forms[0].elements["ChkAllSel"].checked == true) {
                        if (document.forms[0].elements[Oname].checked == false) {
                            document.forms[0].elements[Oname].checked = true;
                        }
                        pid = "";
                        ProdPrice = "";
                    }
                    else {
                        document.forms[0].elements[Oname].checked = false;
                        UntPrice = 0;
                        ProdPrice = "";
                    }
                }
            }
            
            //$("#ctl00_Popupcontent_txtQty").focusout(function () {
            //    alert("UpdateOrderQty");
            //});
     

        </script>
        <script language="javascript" type="text/javascript">
            function test() {
                jQuery("#TB_window").append("<div id='TB_load'><img src='<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/closebtn.png' /></div>"); //add loader to the page
                jQuery('#TB_load').show(); //show loader
            }

            function ShowAlert(e) {
                alert("dsf");
            }
            function keyct(e) {               
                var keyCode = (e.keyCode ? e.keyCode : e.which);
                if (keyCode == 8 || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) {

                }
                else {
                    e.preventDefault();
                }
            }
          </script>
          <div class="col-sm-20">
          <div class="row">
  <%--      <table align="center" width="780" border="0" cellspacing="0" style="margin-left: 10px;" >
    <tr>
        <td align="left" >--%>
      <%-- <div class="box1" style="width:761px;">--%>
      <%if (Request.QueryString["CartItems"] == null)
      { %>
        <asp:UpdatePanel ID="updorddet" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <% 
              
                       
                    HelperServices objHelperServices = new HelperServices();
                    HelperDB objHelperDB = new HelperDB();                    
                    OrderServices objOrderServices = new OrderServices();
                    ProductServices objProductServices = new ProductServices();
                    Security objSecurity = new Security();
                    
                    //ProductFamily oProdFam = new ProductFamily();
                    DataSet dsOItem = new DataSet();
                    int OrderID = 0;
                    int Userid;
                    bool UserCheckout=false;
                    int ProductId;
                    string OrdStatus = "";
                    int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;

                    Userid = objHelperServices.CI(Session["USER_ID"]);

                    if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                    {
                        OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                    }
                    else
                    {
                        OrderID = objOrderServices.GetOrderID(Userid, OpenOrdStatusID);
                    }

                    
                    OrdStatus = objOrderServices.GetOrderStatus(OrderID);
                    ProductId = objHelperServices.CI(Request.QueryString["Pid"]);

                    decimal subtot = 0.00M;
                    decimal taxamt = 0.00M;
                    decimal Total = 0.00M;

                    string SelProductId = "";
                %>


                <%
                                             int DupProdCount = 0;
                                            string LeaveDuplicateProds = "";
                                            if (Request.QueryString["bulkorder"] != null && Request.QueryString["bulkorder"].ToString() == "1")
                                            {
                                                if (Request["rma"] != null)
                                                {
                                                    string _rma = Request["rma"].ToString();
                                                    string _rmitem = Request["Item"].ToString();
                                                    string _rmqty = "";
                                                    double CalItem_ID = 0;
                                                    if (Request.QueryString["DelQty"] != null)
                                                    {
                                                        _rmqty = Request["DelQty"].ToString();
                                                    }
                                                    if (Request.QueryString["cla_id"] != null)
                                                    {
                                                        CalItem_ID = Convert.ToDouble(Request["cla_id"].ToString());
                                                    }
                                                    if (_rma == "NF")
                                                    {
                                                      
                                                        objOrderServices.Remove_Clarification_item(CalItem_ID);  
                                                    }
                                                    if (_rma == "CI")
                                                    {
                                                      
                                                        objOrderServices.Remove_Clarification_item(CalItem_ID); 
                                                    }
                                                }
                                                
                                            
                                                LeaveDuplicateProds = GetLeaveDuplicateProducts();
                                                String sessionId;
                                          
                                                sessionId = Session.SessionID;
                                                DataSet dsDuplicateItem = objOrderServices.GetOrderItemsWithDuplicate(OrderID, LeaveDuplicateProds, sessionId);
                                                DataTable tbErrorItem = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_ERROR", sessionId);
                                               
                                               DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_CHK","");
                                               DataTable tbErrorReplace = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_REPLACE", sessionId);

                                               DataSet dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds, sessionId);
                                                
                                                if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
                                                {                                                   
                                                    DupProdCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
                                                }
                                                
                                               
                                                if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0) || (DupProdCount > 0) || (tbErrorReplace != null && tbErrorReplace.Rows.Count > 0))                                                    
                                                {%>


                 <div class="categoryheading">
  		             <h4>Order Clarification/Errors</h4>
                 </div>     
                 <div class="col-sm-20 orangebg-heading">
        	      <h4>Please Check Below</h4>
                 </div>                                         
                 <div class="table-responsive col-sm-20">
                     <table class="table table-bordered tabletype">
                      <thead class="view_Cart_head_color">
                         <tr>
                           <th>ITEMCODE</th>
                            <th> CLARIFICATION REQUIRED </th>
                           <th></th>
                         </tr>
                    </thead>
                    <tbody>
                    <% string TempNotFoundItem = "";
                                                           string _NotFoundItem = "";
                                                           string _ClaItem_ID = "0";
                                                           if (tbErrorItem != null && tbErrorItem.Rows.Count>0)
                                                           {

                                                               foreach (DataRow RItem in tbErrorItem.Rows)
                                                               {
                                                                   _NotFoundItem = RItem["PRODUCT_DESC"].ToString();
                                                                   _ClaItem_ID = RItem["CLARIFICATION_ID"].ToString();
                                                                   if (_NotFoundItem.Trim() != "")
                                                                   {
                                                                       if (_NotFoundItem.Trim() != TempNotFoundItem.Trim())
                                                                       {
                                                        %>
                                                        <tr>
                                                            <td align="left" >
                                                                <%=_NotFoundItem%>
                                                            </td>
                                                            <td align="left">
                                                                <font color="red" style="font-weight: bold;">Not Found / Incorrect Code</font>
                                                            </td>
                                                            <td align="left">
                                                                <a href="#bulkorder" style="font-weight: bold; text-decoration: none; color: #1589FF;">
                                                                    Please Re-Enter Below</a>
                                                            </td>
                                                            <td align="left">
                                                                <a href="/Orderdetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_NotFoundItem%>&amp;cla_id=<%=_ClaItem_ID%>" style="font-weight: bold;
                                                                    text-decoration: none; color: #1589FF;">Delete Item</a>
                                                            </td>
                                                            </tr>
                                                            <%
                                                                        }
                                                                       
                                                                    }
                                                                       TempNotFoundItem = _NotFoundItem;
                                                                       }
                                                                   }
                                                           
                                                                
                                                           string TempreplaceItem = "";
                                                           string _replaceItem = "";
                                                           int _ordQty = 1;
                                                           string EA_root_Cat_path1 = System.Configuration.ConfigurationManager.AppSettings["EA_ROOT_CATEGORY_PATH"].ToString();
                                                           string EA_New_Product_init_cat_path1 = System.Configuration.ConfigurationManager.AppSettings["EA_NEW_PRODUCT_INIT_CATEGORY_PATH"].ToString();
                                                           string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

                                                           string Ea_Path = "";
                                                           string neweapath="";
                                                           string pfid = "";
                                                           string _catid = "";
                                                    
                                                           if (tbErrorReplace != null && tbErrorReplace.Rows.Count > 0)
                                                           {

                                                               foreach (DataRow RItem in tbErrorReplace.Rows)
                                                               {
                                                                   _replaceItem = RItem["PRODUCT_DESC"].ToString();
                                                                   _ClaItem_ID = RItem["CLARIFICATION_ID"].ToString();
                                                                   _ordQty = (RItem["QTY"].ToString() != "" && Convert.ToInt32(RItem["QTY"].ToString()) > 0) ? Convert.ToInt32(RItem["QTY"].ToString()) : 1;
                                                                   if (_replaceItem.Trim() != "")
                                                                   {
                                                                       if (_replaceItem.Trim() != TempreplaceItem.Trim())
                                                                       {
                                                                           
                                                                           DataTable substituteproduct = new DataTable();
                                                                           DataTable rtntbl = new DataTable();
                                                                           DataTable wag_product_code_substituteproduct = new DataTable();
                                                                           bool samecodesubproduct= false;
                                                                           bool samecodenotFound = false;
                                                                           string wag_product_code = "";
                                                                           string SubstuyutePid = "";
                                                                           int checksetsub = -1;
                                                                           string product_status = "";
                                                                           int StrProductStatusSub = -1;
                                                                           string StrStockStatus = "";
                                                                           int ProdCodeId = 0;
                                                                           
                                                                            
                                                                           rtntbl = objProductServices.GetSubstituteProductDetails(_replaceItem, Userid,"pd");
                                                                           if (rtntbl != null && rtntbl.Rows.Count > 0)
                                                                           {

                                                                               _catid = rtntbl.Rows[0]["CatId"].ToString();
                                                                               pfid = rtntbl.Rows[0]["Pfid"].ToString();
                                                                               Ea_Path = rtntbl.Rows[0]["Ea_Path"].ToString();
                                                                               samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];
                                                                               samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                                                                               wag_product_code = rtntbl.Rows[0]["wag_product_code"].ToString();
                                                                               SubstuyutePid = rtntbl.Rows[0]["SubstuyutePid"].ToString();
                                                                           }
                                                                           else
                                                                           {
                                                                               samecodesubproduct = true;
                                                                               samecodenotFound = false;
                                                                           }

                                                                           if (samecodenotFound==false && samecodesubproduct == true)
                                                                           {
                                                                               
                                                                               
                                                              %>
                                                <tr>
                                                                 <td align="left">
                                                                <%=_replaceItem%>
                                                            </td>
                                                              <td align="left" >

                                                              <font color="orange" style="font-weight: bold;">Product Temporarily Unavailable.&nbsp;Please Contact Us for more details</font>
                                                            
                                                               
                                                               </td>
                                                              
                                                              <td>
                                                                <a href="/Orderdetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>" class="btn primary-btn-blue">Delete Item</a>
                                                            </td>
                                                            </tr>
                                    <%
                                                                    }
                                                                           else if (samecodenotFound == false && samecodesubproduct == false)
                                                                           {
                                                                        %>
                        <tr>
                                                              <td>
                                                                <%=_replaceItem%>
                                                            </td>
                                                              <td align="left">

                                                              <font color="orange" style="font-weight: bold;">Product Not Available.</font>
                                                               <font color="#009900" style="font-weight: bold;">Replaced With:&nbsp;<%=wag_product_code%></font></td>
                                                              <td>
                                                           <%--    <table>
                                                                 <tbody>
                                                                  <tr>--%>
                                                                 <%--   <td>--%>
                                                                     <a href ="<%=Ea_Path%> " class="btn primary-btn-blue">
                                                                     View Item
                                                                     </a>
                                                                   <%-- </td>
                                                                    <td align="left">--%>
                                                                <a href="/Orderdetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>" class="btn primary-btn-blue">Delete Item
                                                                </a>
                                                                 <%-- </td>--%>
                                                                     
                                                                 <%-- </tr>
                                                                 </tbody>
                                                               </table>--%>
                                                              
                                                            </td>
                                                           
                                                             <td>
                                                                
                                                                      <a href="/OrderDetails.aspx?bulkorder=1&amp;flgAddItem=chkAddItem&amp;rma=NF&amp;ORDER_ID=<%=OrderID%>&amp;Pid=<%=SubstuyutePid.ToString().Trim()%>&amp;Qty=<%=_ordQty %>&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>"  class="btn primary-btn-green">
                                                                      Add Item
                                                                      </a>
                                                                    </td>
                                                                    </tr>
                        <%}
                                                                           else
                                                                           {                                                                               
                                                                           %>
                        <tr>
                                                            <td align="left" >
                                                                <%=_replaceItem%>
                                                            </td>
                                                            <td align="left">
                                                                <font color="red" style="font-weight: bold;">Not Found / Incorrect Code</font>
                                                            </td>
                                                            <td align="left">
                                                                <a href="#bulkorder" style="font-weight: bold; text-decoration: none; color: #1589FF;">
                                                                    Please Re-Enter Below</a>
                                                            </td>
                                                            <td align="left">
                                                                <a href="/Orderdetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>" style="font-weight: bold;
                                                                    text-decoration: none; color: #1589FF;">Delete Item</a>
                                                            </td>
                                                            </tr>
                                                                <%
                                                                    }
                                                                        }
                                                                       TempNotFoundItem = _NotFoundItem;
                                                                       }
                                                                   }
                                                           }
                                                          
                                                             string TempClarifyItem = "";
                                                           string _ClarifyItem = "";
                                                           Int32  _orderQty = 0;
                                                           
                                                           if (tbErrorChk!=null && tbErrorChk.Rows.Count > 0)
                                                           {
                                                               foreach (DataRow RItem in tbErrorChk.Rows)
                                                               {
                                                                   _ClarifyItem = RItem["PRODUCT_DESC"].ToString();
                                                                   _orderQty = Convert.ToInt32(RItem["QTY"].ToString());
                                                                   _ClaItem_ID = RItem["CLARIFICATION_ID"].ToString();
                                                                   if (_ClarifyItem.Trim() != "")
                                                                   {
                                                                       if (_ClarifyItem.Trim() != TempClarifyItem.Trim())
                                                                       {      
                                                        %>
                                                        <tr>
                                                            <td align="left">
                                                                <% =_ClarifyItem%>
                                                            </td>
                                                            <td align="left">
                                                                <font color="#ff9900" style="font-weight: bold">Not unique Code</font>
                                                            </td>
                                                            <td align="left">
                                                                <a class="thickbox" href="SubProducts.aspx?Item=<%=_ClarifyItem %>&amp;height=400&amp;width=600&amp;modal=true&amp;OrderID=<%=OrderID%>&amp;ClrQty=<%=_orderQty%>&amp;cla_id=<%=_ClaItem_ID%>"
                                                                     style="font-weight: bold; text-decoration: none; color: #1589FF;">Clarify Now</a>
                                                            </td>
                                                            <td align="left">
                                                                <a href="/OrderDetails.aspx?bulkorder=1&amp;rma=CI&amp;item=<%= _ClarifyItem %>&amp;DelQty=<%=_orderQty%>&amp;cla_id=<%=_ClaItem_ID%> "
                                                                    style="font-weight: bold; text-decoration: none; color: #1589FF;">Delete Item</a>
                                                            </td>
                                                        </tr>
                                                        <%  
                                                                 
                                                                       }

                                                                       TempClarifyItem = _ClarifyItem;
                                                                    
                                                                   }
                                                               }
                                                            }
                                                            string dupItem ="";
                                                            int pid1;
                                                            int maxqty1;
                                                            int minQty1;
                                                            int FId1 = 0;
                                                            double  orderItemId = 0;
                                                            decimal ProductUnitPrice1;
                                                            decimal ProdTotal1;
                                                            int Qty1;
                                                            int rowcnt1 = 0;
                                                            
                                                            DataTable temptbl = null;
                                                            if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)                                                                
                                                            {
                                                                foreach (DataRow drItem in dsDuplicateItem_Prod_id.Tables[0].Rows)
                                                                {
                                                                    DataRow[] Dr = dsDuplicateItem.Tables[0].Select("PRODUCT_ID='" + drItem["PRODUCT_ID"].ToString() + "'");
                                                                    if (Dr.Length > 0)
                                                                    {
                                                                        temptbl = Dr.CopyToDataTable();
                                                                        if (temptbl != null && temptbl.Rows.Count > 0)                                                                            
                                                                        { %>
                                                                            
                                                                          <tr>
                                                                            <td >
                                                                                <% =temptbl.Rows[0]["CATALOG_ITEM_NO"].ToString().ToUpper() %>
                                                                            </td>
                                                                            <td>
                                                                                <font color="#ff9900" style="font-weight: bold">Duplicate/Multiple Product Entries Found</font>
                                                                           
                                                                            <table cellspacing="0" cellpadding="0" border="0px" class="table table-bordered" style="padding-top: 0px; padding-bottom: 0px; width:345px;">
                                                                             <tbody>
                                                                             <tr>                                                                           
                                                                                    <td width="100">
                                                                                        
                                                                                          <strong> Order Code </strong>
                                                                                        
                                                                                    </td>
                                                                                     <td width="100" align ="right" >
                                                                                     
                                                                                     <strong>  QTY</strong>
                                                                                     
                                                                                    </td>  
                                                                                     <td width="100" align="center"  >
                                                                                     <font  style="font-weight: bold" >
                                                                                    
                                                                                       </font>
                                                                                    </td>                                                                             
                                                                                  </tr>
                                                                                                          
                                                                             <%
                                                                            pid1 = objHelperServices.CI(drItem["PRODUCT_ID"].ToString());
                                                                            foreach (DataRow tmpItem in temptbl.Rows)
                                                                            {   
                                                                               
                                                                                ProductUnitPrice1 = objHelperServices.CDEC(tmpItem["PRICE_EXT_APPLIED"].ToString());
                                                                                ProductUnitPrice1 = objHelperServices.CDEC(ProductUnitPrice1.ToString("N2"));
                                                                                Qty1 = objHelperServices.CI(tmpItem["QTY"].ToString());
                                                                                ProdTotal1 = Qty1 * ProductUnitPrice1;
                                                                                orderItemId = Convert.ToDouble(tmpItem["ORDER_ITEM_ID"].ToString());
                                                                                %>
                                                                             <tr>                                                                           
                                                                                    <td width="100" align ="left" >
                                                                                        <font  style="font-weight: bold">
                                                                                           <% =tmpItem["CATALOG_ITEM_NO"].ToString().ToUpper() %>
                                                                                        </font>
                                                                                    </td>
                                                                                    <td width="100" align ="right" >
                                                                                        <font  style="font-weight: bold;font-size:12px;">
                                                                                           <% =tmpItem["QTY"].ToString().ToUpper() %>
                                                                                        </font>
                                                                                    </td>
                                                                                      <td width="100" align="center">
                                                                                        <a href="/OrderDetails.aspx?&bulkorder=1&SelPid=<%=pid1%>&amp;SelProdPrice=<%=ProdTotal1%>&amp;ProdPrice=<%=ProductUnitPrice1%>&amp;ORDER_ID=<%=OrderID%>&amp;ORDER_ITEM_ID=<%=orderItemId%> " style="color: #0099ff">Remove</a>
                                                                                    </td>                                                                            
                                                                                  </tr>
                                                                             <%
                                                                                
                                                                            }
                                                                            
                                                                            %>

                                                                            <tr>
                                                                            
                                                                            <td colspan="3" align="left"  >
                                                                                <a href="/OrderDetails.aspx?&bulkorder=1&amp;ORDER_ID=<%=OrderID%>&amp;CombainProd_id=<%=pid1%>"style="color: #0099ff">Combine all into one (QTY discount may apply)</a>
                                                                                
                                                                                <br />
                                                                                 <a href="/OrderDetails.aspx?&bulkorder=1&amp;ORDER_ID=<%=OrderID%>&amp;LeaveProd_id=<%=pid1%>"style="color: #0099ff">Leave as multiple lines</a>
                                                                            </td>
                                                                            
                                                                            </tr>
                                                                            </tbody>
                                                                             </table>
                                                                               
                                                                            </td>
                                                                            <td>
                                                                                
                                                                            </td>
                                                                         <%--   <td>
                                                                                
                                                                            </td>--%>
                                                                            </tr>
                                                                            


                                                                            <%
                                                                                
                                                                        }
                                                                    }
                                                                         
                                                                }
                                                                
                                                            }                                                                                                                                         
                                                        %>
                       </tbody>
                     </table>
                 </div>
                          <%   }
                                            }
                                            else
                                            {
                                                Session["ITEM_ERROR"] = "";
                                               Session["ITEM_CHK"] = "";
                                               Session["QTY_CHK"] = "";
                                            }
          
                                        %>              
                          <%--   <tr>
                        <td align="left" colspan="2" width="100%">
                          <div class="quickorder3">        
                            <H3 class="title2">Order Clarification/Errors</H3>
                            <table id="SiteMapTable0"  class="orderdettable">
                                  <tr>
                                    <td align="left" colspan="4" bgColor="#cccccc">
                                        <b>Please Check Below</b>
                                    </td>
                                </tr>
                                 <tr>
                                        <td  align="left" bgcolor="#f2f2f2"   >
                                            ITEMCODE
                                        </td>
                                        <td align="left" bgcolor="#f2f2f2" >
                                            CLARIFICATION REQUIRED
                                        </td>
                                        <td colspan="2" bgcolor="#f2f2f2" >
                                            &nbsp;
                                        </td>
                                    </tr>  
                                   
                                   
                                                         

                                                        
                                                        
                                                              
                                                             
                                                                        
                                                                  
                                                                            

                                             
                                                   </table>    
                            </div>
                            <br />
                        </td>
                    </tr>--%>
                                        
               <div class="categoryheading">
           		<h4>Shopping cart contents</h4>
               </div>
               <div class="col-sm-20 bluebg-heading pl0">
        	    <h4>Order Contents</h4>
               </div>
               <div class="table-responsive col-sm-20 inner-table pl0">
               <table  id="BaseTable0" class="table table-bordered tabletype">                       
                   <%-- <tr>
                        <td align="left" colspan="2"  valign="top" style="background-color: #F2F2F2; border: thin solid #E7E7E7">--%>
                          <%-- <div class="quickorder3">  --%>      
                            <%--<H3 class="title1">Shopping cart contents</H3>--%>
                            <asp:Label ID="Label1" runat="server" Text="Order No : " Visible="false"></asp:Label>
                             <asp:Label ID="lblOrdNo" runat="server" Visible="false"></asp:Label>
                           <%-- <table border="0px" cellpadding="0" cellspacing="0" class="orderdettable">--%>
                             
                             
                                <%--<tr>
                                    <td align="left" colspan="7" bgColor="#cccccc">
                                        <b>Order Contents</b>
                                    </td>
                                </tr>--%>
                                <thead class="view_Cart_head_color">
                                <tr>
                                    <th>
                                        Order Code
                                    </th>
                                    <th>
                                        Quantity
                                    </th>
                                    <th>
                                        Description
                                    </th>
    
                                    <th>
                                        Cost (Ex. GST)
                                    </th>
                                    <th>
                                        Extension Amount (Ex. GST)
                                    </th>
                                </tr>
                                </thead>
                         <%--         <% 
                                      string chkuserID = HttpContext.Current.Session["USER_ID"].ToString();
                                      if (System.Convert.ToString(Session["CUSTOMER_TYPE"]).Contains("Retailer") == true)
                                           {                                               
                                            
                                            %>

                                            
                                          
                                          <%}
                                           else
                                           { %>
                                    
                                              
                                          
                                          <% }
                                             %>--%>

                                 <tbody>

                                <%   	   
                                     string EA_root_Cat_path = System.Configuration.ConfigurationManager.AppSettings["EA_ROOT_CATEGORY_PATH"].ToString();
                                     string EA_New_Product_init_cat_path = System.Configuration.ConfigurationManager.AppSettings["EA_NEW_PRODUCT_INIT_CATEGORY_PATH"].ToString();


                                   //  String sessionId;
                                   //  sessionId = Session.SessionID;
                                     String sessionId_dum;
                                     // string sessionuserid_dum;
                                     sessionId_dum = Session.SessionID;
                                    
                                     string chkuserID = HttpContext.Current.Session["USER_ID"].ToString();


                                     if (chkuserID != ConfigurationManager.AppSettings["DUM_USER_ID"].ToString())
                                     {
                                         //dsOItem = objOrderServices.GetOrderItems(OrderID);
                                         dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, LeaveDuplicateProds,"");
                                     }
                                     else
                                     {
                                         //dsOItem = objOrderServices.GetOrderItems(OrderID);
                                        
                                         dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, LeaveDuplicateProds, sessionId_dum);
                                     }   
                                    string cSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                                    decimal ProdShippCost = 0.00M;
                                    decimal TotalShipCost = 0.00M;

                                     
                                    SelProductId = "";
                                    if (OrdStatus == OrderServices.OrderStatus.OPEN.ToString() || OrdStatus == "CAU_PENDING")
                                    {
                                        if (dsOItem != null)
                                        {
                                            int i = 0;
                                            
                                            string prdsessionid = "";
                                           
                                            bool flgchkdummy = false;
                                            bool chkopenurl = false;

                                           // string id;
                                           // string strchkopenurl = "";
                                            //if (Request.Url.Query != null && Request.Url.Query != "")
                                            //{
                                               
                                            //    if (Request.Url.Query.Contains("key=") == false)
                                            //    {
                                            //        id = Request.Url.Query.Replace("?", "");
                                            //        id = objSecurity.StringDeCrypt_password(id);

                                            //        if (id != null)
                                            //        {
                                            //            string[] ids = id.Split(new string[] { "OrdFlg=" }, StringSplitOptions.None);
                                            //            strchkopenurl = ids[1].ToString();
                                            //        }
                                            //    }

                                               
                                            //}

                                            if (Request.QueryString["OrdFlg"] != null)
                                            {
                                                string strchkopenurl = objSecurity.StringDeCrypt_password(Request.QueryString["OrdFlg"].ToString());
                                                if(strchkopenurl == "OPENORDER")
                                                      chkopenurl = true;
                                              
                                            }
                                           
                                            foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                                            {
                                             flgchkdummy = false;
                                                 if(chkuserID != ConfigurationManager.AppSettings["DUM_USER_ID"].ToString())
                                                     flgchkdummy =true;
                                                 else
                                                 {
                                                   if (chkuserID == ConfigurationManager.AppSettings["DUM_USER_ID"].ToString() && sessionId_dum == dsOItem.Tables[0].Rows[i]["PRD_SESSION_ID"].ToString())
                                                    flgchkdummy =true;
                                                   else if(chkopenurl == true)
                                                      flgchkdummy = true;
                                                else
                                                    flgchkdummy =false;
                                                 }
                                               if (flgchkdummy) 
                                                                                                                                      
                                               {
                                                decimal ProductUnitPrice;
                                                int pid;
                                                int maxqty;
                                                int minQty;
                                                int FId = 0;
                                                double OrderItemId1 = 0;
                                                string sty = "style=\"border-style: none solid none none; border-width: thin; border-color: #E7E7E7\" ";
                                                string styl = "style=\"border-style: none none none none; border-width: thin; border-color: #E7E7E7\" ";
                                                if (rItem["PRODUCT_ID"].ToString() == dsOItem.Tables[0].Rows[dsOItem.Tables[0].Rows.Count - 1]["PRODUCT_ID"].ToString())
                                                {
                                                    sty = "style=\"border-style: none solid none none; border-width: thin; border-color: #E7E7E7\" ";
                                                    styl = "style=\"border-style: none none none none; border-width: thin; border-color: #E7E7E7\" ";
                                                }
                                                pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                                                OrderItemId1 = objHelperServices.CD(rItem["ORDER_ITEM_ID"].ToString());
                                                //FId = oHelper.CI(rItem["FAMILY_ID"].ToString()); 
                                                FId = objProductServices.GetFamilyID(pid);
                                                int pQty = objOrderServices.GetOrderItemQty(pid, OrderID, OrderItemId1);
                                                maxqty = objHelperServices.CI(rItem["QTY_AVAIL"].ToString());
                                                maxqty = maxqty + objHelperServices.CI(Request.Form["txtQty"] + pQty);
                                                minQty = objHelperServices.CI(rItem["MIN_ORD_QTY"].ToString());
                                                ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                                                ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N2"));
                                                
                                                //ProdShippCost = CalculateShippingCost(OrderID, pid, ProductUnitPrice, pQty);
                                                //TotalShipCost = oHelper.CDEC(TotalShipCost + ProdShippCost); 
                                                int Qty = objHelperServices.CI(rItem["QTY"].ToString());
                                                decimal ProdTotal = Qty * ProductUnitPrice;
                                                subtot = subtot + ProdTotal;
                                                string Desc = rItem["DESCRIPTION"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                                                string Available = rItem["PRODUCT_STATUS"].ToString();

                                                string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
                                                
                                                string Ea_Path = "";
                                                string pfid = "";
                                                string _catid = "";
                                                string neweapath = "";
                                                DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, pid.ToString(), "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID", HelperDB.ReturnType.RTDataSet);
                                                if (tmpds != null && tmpds.Tables.Count > 0)
                                                {

                                                    if (objHelperServices.CI(tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"]) > 0)
                                                    {
                                                        pfid = tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();
 string[] catpath=null;
                                                        if (tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() != "")
                                                        {
                                                            catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                                        
                                                            Ea_Path = EA_New_Product_init_cat_path +"////"+ tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString()+"////"+ pid + "=" + rItem["CATALOG_ITEM_NO"].ToString()+ "////" + (catpath.Length >= 1 ? catpath[0] : " ")+"////"+ (catpath.Length >= 2 ? catpath[1] : " ")   + "////" + tmpds.Tables[0].Rows[0]["FAMILY_NAME"].ToString() ;
                                                            Ea_Path = objHelperServices.SimpleURL_Str(Ea_Path, "pd.aspx", true) + "/pd/";
                                                        }
                                                        
                                                       


                                                        //Ea_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                    }
                                                    else
                                                    {
                                                        pfid = tmpds.Tables[0].Rows[0]["FAMILY_ID"].ToString();
                                                        string[] catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                                        Ea_Path = EA_New_Product_init_cat_path +"////"+tmpds.Tables[0].Rows[0]["FAMILY_ID"].ToString()+"////"+ pid + "=" + rItem["CATALOG_ITEM_NO"].ToString()+ "////" +(catpath.Length >= 1 ? catpath[0] : " ")+"////"+ (catpath.Length >= 2 ? catpath[1] : " ")   + "////" + tmpds.Tables[0].Rows[0]["FAMILY_NAME"].ToString();
                                                       
                                                        Ea_Path = objHelperServices.SimpleURL_Str( Ea_Path, "pd.aspx",true) + "/pd/";
                                                        //Ea_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                    }
                                                    
                                                    //_catid = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                                                    //pfid = tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();
                                                    //Ea_Path = EA_New_Product_init_cat_path + "////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();
                                                    //neweapath = Ea_Path.Replace(" ", "_"); ;                                                    
                                                    //Ea_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                               
                                                }
                                                
                                                // int FId=oProd.getpr
                                                if (Request["SelAll"] != "1")
                                                {
                                                    SelProductId = "";
                                                    Session["SelProduct"] = null;
                                                    CheckBox chk = new CheckBox();
                                %>
                                <tr>
                                    <td>
                                        <%="<a href ="+ Ea_Path + ">" + rItem["CATALOG_ITEM_NO"].ToString() + " </a>"%>
                                    </td>
                                    <td>
                                        <%="<input type =\"Text\" Id=\"txtQtyId" + OrderItemId1 + "_" + maxqty + "\" Name =\"txtQty" + OrderItemId1 + "\" size=\"4\" onkeypress=\"return validateNumber(event)\" onkeydown=\"return keyct(event)\"  maxlength=\"6\" class=\"qty_input\" value =\"" + Qty + "\">"%>
                                        &nbsp;&nbsp;<a href="/OrderDetails.aspx?&bulkorder=1&SelPid=<%=pid%>&amp;SelProdPrice=<%=ProdTotal%>&amp;ProdPrice=<%=ProductUnitPrice%>&amp;ORDER_ID=<%=OrderID%>&amp;ORDER_ITEM_ID=<%=OrderItemId1%>" class="gray_40">Remove</a>&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <%=Desc%>
                                    </td>
                                   
                                    <td>
                                        <%=cSymbol + " " + ProductUnitPrice.ToString("#,#0.00")%>
                                    </td>
                                    <td>
                                        <%=cSymbol + " " + ProdTotal.ToString("#,#0.00")%>
                                    </td>
                                    <%="<input type=\"hidden\" Name=\"txtPid" + OrderItemId1 + "\" runat=\"server\" value=\"" + pid + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtCatItem" + OrderItemId1 + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtMaxQty" + OrderItemId1 + "\" runat=\"server\" value=\"" + maxqty + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtMinQty" + OrderItemId1 + "\" runat=\"server\" value=\"" + minQty + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtUntPrice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtPrdTprice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"%>
                                </tr>
                                <%  
                                                
                                                     prdsessionid = dsOItem.Tables[0].Rows[i]["PRD_SESSION_ID"].ToString();
                                                       i = i + 1;
                                                       
                                                   
                                                  
                                                }
                                                else if (Request["SelAll"] == "0")
                                                {
                                                    SelProductId = "";
                                                    Session["SelProduct"] = null;
                                %>
                                <tr>
                                    <td align="center" bgcolor="White" class="style24OrdDetail" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<input type =\"CheckBox\" style=\"border-style:none;\" Name =\"Chk" + OrderItemId1 + "\" value =" + pid + "\"  onclick =\"javascript:Click(" + pid + "," + Qty + "," + OrderItemId1 + ");\">"%>
                                    </td>
                                    <td align="left" bgcolor="White" class="style19OrdDetail" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<a href =ProductFeatures.aspx?Fid=" + FId + "&Pid" + pid + "&Min=" + minQty + "&Max" + maxqty + ");>" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>"%>
                                    </td>
                                    <td align="left" bgcolor="White" class="Numeric" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<input type =\"Text\" Id=\"txtQtyId" + OrderItemId1 + "_" + maxqty + "\" Name =\"txtQty" + OrderItemId1 + "\" size=\"7\"  runat =\"server\" onBlur=\"javascript:Check(" + OrderItemId1 + "," + maxqty + ");\" value =\"" + Qty + "\">"%>
                                    </td>
                                    <td bgcolor="White" class="style21OrdDetail" style="border-style: none none none solid; border-width: thin;
                                        border-color: #E7E7E7">
                                        <%=Desc %>
                                    </td>
                                    <td bgcolor="White" class="style20OrdDetail" style="border-style: none none none solid; border-width: thin;
                                        border-color: #E7E7E7">
                                        <%=Available%>
                                    </td>
                                    <td align="center" bgcolor="White" class="style23OrdDetail" style="width: 130px" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%=cSymbol + " " + ProductUnitPrice.ToString("#,#0.00")%>
                                    </td>
                                    <%--								                                <td class="NumericField" align="center"><%Response.Write(cSymbol + ProdShippCost.ToString("#,#0.00")); %></td>
                                    --%>
                                    <td align="center" bgcolor="White" class="NumericField" style="border-style: none solid none solid;
                                        border-width: thin; border-color: #E7E7E7" width="20%">
                                        <%=cSymbol + " " + ProdTotal.ToString("#,#0.00") %>
                                    </td>
                                    <%="<input type=\"hidden\" Name=\"txtPid" + OrderItemId1 + "\" runat=\"server\" value=\"" + pid + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtCatItem" + OrderItemId1 + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtMaxQty" + OrderItemId1 + "\" runat=\"server\" value=\"" + maxqty + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtsPrdId" + OrderItemId1 + "\" runat=\"server\" value=\"" + pid + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtUntPrice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtPrdTprice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"%>
                                </tr>
                                <%
                                                    prdsessionid = dsOItem.Tables[0].Rows[i]["PRD_SESSION_ID"].ToString();
                                                    i = i + 1;
                                                    
                                                }
                                                else
                                                { 
                                %>
                                <tr>
                                    <td align="center" bgcolor="White" class="style24OrdDetail" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<input type =\"CheckBox\" style=\"border-style:none;\" Name =\"Chk" + OrderItemId1 + "\" value =" + pid + "\" checked=\"checked\" onclick =\"javascript:Click(" + pid + "," + Qty + "," + OrderItemId1 + ");\">"%>
                                    </td>
                                    <td align="left" bgcolor="White" class="style19OrdDetail" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<a href =ProductFeatures.aspx?Fid=" + FId + "&Pid=" + pid + "&Min=" + minQty + "&Max=" + maxqty + ");>" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>"%>
                                    </td>
                                    <td align="left" bgcolor="White" class="Numeric" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%="<input type =\"Text\" Id=\"txtQtyId" + OrderItemId1 + "_" + maxqty + "\" Name =\"txtQty" + OrderItemId1 + "\" size=\"7\"    runat =\"server\" onBlur=\"javascript:Check(" + OrderItemId1 + "," + maxqty + ");\" value =\"" + Qty + "\">"%>
                                    </td>
                                    <td bgcolor="White" class="style21OrdDetail" style="border-style: none none none solid; border-width: thin;
                                        border-color: #E7E7E7">
                                        <%=Desc %>
                                    </td>
                                    <td bgcolor="White" class="style20OrdDetail" style="border-style: none none none solid; border-width: thin;
                                        border-color: #E7E7E7">
                                        <%=Available%>
                                    </td>
                                    <td align="center" bgcolor="White" class="style23OrdDetail" style="width: 130px" style="border-style: none none none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%=cSymbol + " " + ProductUnitPrice.ToString("#,#0.00")%>
                                    </td>
                                    <%--								                                <td class="NumericField" align="center"><%Response.Write(cSymbol + ProdShippCost.ToString("#,#0.00")); %></td>
                                    --%>
                                    <td align="center" bgcolor="White" class="NumericField" style="border-style: none solid none solid;
                                        border-width: thin; border-color: #E7E7E7">
                                        <%=cSymbol + " " + ProdTotal.ToString("#,#0.00") %>
                                    </td>
                                    <%="<input type=\"hidden\" Name=\"txtPid" + OrderItemId1 + "\" runat=\"server\" value=\"" + pid + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtCatItem" + OrderItemId1 + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtMaxQty" + OrderItemId1 + "\" runat=\"server\" value=\"" + maxqty + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtsPrdId" + OrderItemId1 + "\" runat=\"server\" value=\"" + pid + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtUntPrice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"%>
                                    <%="<input type=\"hidden\" Name=\"txtPrdTprice" + OrderItemId1 + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"%>
                                </tr>
                                <%   
                                                    SelProductId = SelProductId + "," + pid;
                                                    prdsessionid = dsOItem.Tables[0].Rows[i]["PRD_SESSION_ID"].ToString();
                                                    i = i + 1;
                                              
                                                } //End of SelAll
                                               } //end if

                                                //if (sessionId != "" && sessionId != prdsessionid && chkuserID == ConfigurationManager.AppSettings["DUM_USER_ID"].ToString())
                                                if (sessionId_dum != "" &&  flgchkdummy == false)
                                                    i = i + 1;
                                            } //End of for each.
                                            dsOItem.Dispose();
                                        }//End of dataset empty. 
                                    } // End Of Order Status Check
                                    if (SelProductId != "")
                                    {
                                        SelProductId = SelProductId.Substring(1, SelProductId.Length - 1);
                                        Session["SelProduct"] = SelProductId;
                                    }
                                %>
                                      
                                <tr class="border_none">
                                    <td colspan="3">
                                          
                                        <asp:ImageButton ID="ImageButton2" runat="server" ClientIDMode="Static"
                                            OnClientClick="javascript:return  setTimeout('SendRemoveProducts()',100);" />
                                    </td>
                                   
                                    <td  colspan="1">
                                        <strong>Sub Total</strong>
                                    </td>
                                    <td>
                                        <strong class="font_18"><%=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(subtot))%></strong>
                                    </td>
                                </tr>
                                <tr class="border_none">
                                <td colspan="3">
                                </td>
                                    <td>
                                       <strong> Tax Amount(GST)</strong><br />
                                        <span style="font-size: 4"></span>
                                    </td>
                                    <td>
                                        <%       
                                            
                                            //string sSQL = string.Format("SELECT isnull(TAX_AMOUNT,0) [TAX_AMOUNT] FROM TBWC_ORDER WHERE ORDER_ID = {0}", OrderID);
                                            //objHelperServices.SQLString = sSQL;
                                            //taxamt = System.Convert.ToDecimal(oHelper.GetValue("TAX_AMOUNT"));
                                            if (subtot > 0)
                                            {
                                                taxamt = objOrderServices.CalculateTaxAmount_Express(subtot, OrderID.ToString());   //Math.Round((subtot * 10 / 100),2,MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                taxamt = 0;
                                            }
                                            Total = subtot + taxamt + TotalShipCost;
                                            Total = objHelperServices.CDEC(objHelperServices.FixDecPlace(Total));%>
                                       <strong class="font_18"> <%=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(taxamt))%></strong>
                                    </td>
                                </tr>
                                <tr class="border_none blue_color_text">
                                <td colspan="3">
                                </td>
                                    <td colspan="1">  <%--bgColor="#dff0d8"--%>
                                    <% 
                                          
                                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                                    {                                        
                                     %>                                        
                                         <strong>Total</strong><br />
                                     <%
                                    }
                                    else
                                    {
                                         %>
                                        <strong>Total Inc GST</strong><br />
                                         <%
                                    } %>

                                      
                                        <%--<font size="1">(Freight not included)</font>--%>
                                    </td>
                                    <td> <%--bgColor="#dff0d8"--%>
                                        <strong class="font_18">
                                            <%=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(Total))%>
                                        </strong>
                                    </td>
                                </tr>
                                <tr>
                <%--    <td colspan="6">
                    <P class="p2 fright">*Please Note: Above prices are not final and subject to our confirmation. Total price shown above does not include freight.</P>
                    </td>
                    </tr>--%>
                   
                    
                           
                   
                    
                        
<%--                    <tr>
                        <td colspan="6" align="right">
                           

                                   
                        </td>
                    </tr>--%>
                    </tbody>
                   <%-- </table>  --%>                          
                           <%-- </div>  --%>                   
                       <%-- </td>
                    </tr>--%>
                </table>
              
               </div>
                 <div class="col-sm-20 text-right">
        
                       *Please Note: Above prices are not final and subject to our confirmation. Total price shown above does not include freight.
               </div>
               <div class="col-sm-20 text-right viewcart_btns inner-table">
                                                      <asp:Button ID="BtnOrdTemplate" runat="server" OnClick="btnSaveOrdTemplate_Click" Text="Save as Template" style="margin: 10px 0px 10px 200px; "  class="buttongray normalsiz btngray fleft" Visible="false"   />

                                        <asp:Button ID="updCart" runat="server" OnClick="btnUpdateCart_Click" Text="Update Order Qty"  class="btn btn-primary"   />
                                        <%
                                            UserCheckout = objHelperServices.GetIsEcomEnabled(Userid.ToString());
                                                
                                             %>
                                        <% if (UserCheckout == true)
                                           { %>
                                        
                                             <asp:Button ID="ImageButton4" runat="server"   OnClick="btnContinueNext_Click" Text="Continue Shopping"   class="btn btn-primary"   />

                                        <%} %>
                                  
                                        <% if (UserCheckout == true)
                                           { %>                                        
                                        <asp:Button ID="lblOrderProceed" runat="server"   OnClick="btnNext_Click" Text="Go to Check Out"  class="btn primary-btn-green" Visible="false"   />
                                        <%}
                                           else
                                           { %>
                                  
                                             <asp:Button ID="ImageButton5" runat="server"   OnClick="btnContinueNext_Click" Text="Continue Shopping"   class="btn btn-primary"   />

                                        <%} %>
                    <asp:Button ID="BtnExpressCheckout" runat="server" Text="Go To Checkout" class="btn primary-btn-green" OnClick="BtnExpressCheckout_Click" />
                  
                   <%  if (HttpContext.Current.Session["USER_ID"] != "" && Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) > 0 && HttpContext.Current.Session["USER_ID"].ToString() == "999" ){ %>
                  <%} %>
              </div>

               <div class="clearfix"></div>
               
                <div id="PopupMsg" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel"  role="dialog" tabindex="-1" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6);display: block;" class="modal fade bs-example-modal-lg in">
                   
                   <div class="modal-dialog">
                    <div class="modal-content"> 
                    <div class="modal-body"> 
                         <div class="warning-info clearfix">
                             <div class="close-selected">
                                    <asp:ImageButton
                ID="ImageButton3"                runat="server"
                OnClick="btnCancel_Click"   CausesValidation="false"/>
               </div>
                              <div class="col-lg-20 text-center col-sm-10 col-md-5 mgntb20">
                                <p>Please review and correct order Clarification / Error before proceeding to Checkout!</p>
                              </div> 
                            </div>
                            <div class="modal-footer">
                                 <asp:Button ID="btnCancel" runat="server" Text="Close"  class="btn primary-btn-blue" OnClick="btnCancel_Click" />                              
                              </div>
                  </div> 
                     
                  </div>
                 </div>
                  
                    <%--<asp:Panel ID="ModalPanel" runat="server" Style="display: none" BackColor="White"
                        Height="50px" Width="650px" BorderStyle="Solid" BorderWidth="2px" BorderColor="#b81212">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
                            font-family: Arial; font-size: 12px; font-weight: bold; color: #FF0000;" align="center">
                            <tr style="height: 15px">
                                <td colspan="4">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td width="5%">
                                    &nbsp;
                                </td>
                                <td width="80%" align="left">
                                    Please review and correct Order Clarifications / Errors before proceeding to Check
                                    Out!
                                </td>
                                <td width="10%" align="right">
                                    <asp:Button ID="btnCancel" runat="server" Text="Close" Width="55px" Font-Bold="true" ForeColor="#1589FF" OnClick="btnCancel_Click" />
                                </td>
                                <td width="5%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
                </div>
                <div id="DivAlert">
                    <asp:Panel ID="pnlAlert" runat="server" Style="display: none" BackColor="White"
                        Height="100px" Width="350px" BorderStyle="Solid" BorderWidth="2px" BorderColor="#0077cc">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
                            font-family: Arial; font-size: 12px; font-weight: bold; color: #FF0000;" align="center">
                            <tr style="height: 15px">
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 50px">
                                <td width="5%">
                                    &nbsp;
                                </td>
                                <td width="80%" align="center">
                                    
                                    <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
                                </td>                               
                                <td width="5%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                             <td colspan="3" align="center">
                                  <asp:Button ID="btnok" runat="server" Text="Ok" Width="55px" Font-Bold="true"
                                        ForeColor="#1589FF" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <%
                    if (objOrderServices.GetOrderItemCount(OrderID) == 0)
                    {
                        if (Session["NOITEMADDED"] != null && Session["NOITEMADDED"].ToString() != "")
                        {
                            Response.Redirect("ConfirmMessage.aspx?Result=NOPRICEAMT",false);
                        }
                        else
                        {
                            if (Request.QueryString["bulkorder"] != null && Request.QueryString["bulkorder"].ToString() == "1")
                            {

                            }
                            else
                            {
                                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY",false);
                            }
                        }
                    }

                    if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
                    {
                        OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                    }
                    else
                    {
                        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                    }

                    decimal totalcost = objOrderServices.GetOrderTotalCost(OrderID);

                    if (totalcost <= 0)
                    {
                        lblOrderProceed.Enabled = false;
                        //lblOrderProceed1.Enabled = false;
                    }
                %>
                <div id="Div2">
                    <asp:Panel ID="MessageboxPanel" runat="server" Style="display: none" BackColor="White"
                        Height="65px" Width="450px" BorderStyle="Solid" BorderWidth="2px" BorderColor="#b81212">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
                            font-family: Arial; font-size: 12px; font-weight: bold; color: #FF0000;" align="center">
                            <tr style="height: 5px;">
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td>
                                    Invalid Quantity! Minimum Ordered Quantity Should be Equal/Greater than 1
                                </td>
                            </tr>
                            <tr style="height: 5px;">
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 15px;">
                                <td>
                                    <asp:Button ID="CloseButton" runat="server" Text="Close" Width="55px" Font-Bold="true"
                                        ForeColor="#1589FF" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="updCart" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ImageButton2" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
       
      <%} else if(Request.QueryString["CartItems"].ToString()  == "0")
        { %>


      <div  tabindex="0" style="overflow: hidden; outline: none; ">
<div class="mini-empty-cart">
<div class="col-sm-5"><img alt="" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/empty_cart.png"  class="img-responsive"/></div>
<div class="col-sm-15 mgntop50">
 
<p style="font-weight:bold"   >
You have no items in your shopping cart</p>
 <asp:Button ID="Button1" runat="server"  
 OnClick="btnContinueNext_Click" Text="Start Shopping"   class="btn btn-primary"   />
 </div>
 </div></div>
  <%} %>
  <%--<br />--%>
    <%
        if (Request.QueryString["bulkorder"] != null && Request.QueryString["bulkorder"].ToString() == "1")
        {
    %>

    <%} %>
   <%-- </div>--%>
   <%-- </td>
</tr>
</table>--%>
          </div>
          </div>
       </asp:Panel>
  </div>
  <%} %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Popupcontent" runat="Server" >
    <%--   <asp:HiddenField runat="server" ID="txtordidpop" />
      <asp:HiddenField runat="server" ID="txtpidpop" />--%>
<script type="text/javascript">

    //$(document).ready(function () {
    //    //called when key is pressed in textbox
    //    $("#ctl00_Popupcontent_txtQtypop").keypress(function (e) {
    //        //if the letter is not digit then display error and don't type anything
    //        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
    //            //display error message
    //          //  $("#errmsg").html("Digits Only").show().fadeOut("slow");
    //            return false;
    //        }
    //    });
    //});

    document.onkeyup = function (e) {
        if (e == null) { // ie
            keycode = event.keyCode;
        } else { // mozilla
            keycode = e.which;
        }
        if (keycode == 27) { // close
            GetCartCount();
            tb_remove();
        }
    }

    //$(document).ready(function () {
    //    $("#ctl00_Popupcontent_txtQtypop").focusout(function () {
    //        var qty =document.getElementById("ctl00_Popupcontent_txtQtypop").value;
    //        var ordid = document.getElementById("ctl00_Popupcontent_txtordidpop").value;
    //        var pid = document.getElementById("ctl00_Popupcontent_txtpidpop").value;

    //        $.ajax({
    //            type: "POST",
    //            url: "/OrderDetails.aspx/UpdateOrderQty",
    //            data: "{'Qty':'" + qty + "','OrdId':'" + ordid + "','pid':'" + pid + "'}",
    //            contentType: "application/json;charset=utf-8",
    //            dataType: "json"
    //            //success: OnmailSuccess,
    //           // error: OnmailFailure


    //        });


    //    });
    //});

    //function OnmailSuccess(result) {
    //}
</script>
 <%if (Request.QueryString["popup"] != null)
      { %>
      <div align="center" runat="server" id="divAddtocart" class="popuptable">
               
      <div class="modal-header green_bg">
                            <h4 id="myModalLabel" class="text-center">
                            <img class="popsucess" alt="" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/popup_tick.png"/>Item Added to Cart</h4>
                          </div>
      <div class="modal-body">
                            <div class="col-lg-20">
                              <div class="col-lg-20 text-center nopadd">
                                <div class="pop_img_size">
                               <%-- <img class="img-responsive" src="/images/newproduct-2.jpg"/>--%>
                                  <asp:Image ID="lblProImage" class="img-responsive" Style="margin:0 auto;" runat="server" alt="Loading..."  />
                                </div>
                                <p class="boldtext">
                                <asp:Label ID="lblFamilyName" runat="server" Text="Label"></asp:Label>
                                </p>
                              </div>
                              <div class="col-lg-20">
                                <div class="table-responsive">
                                  <table class="table table-bordered boldtxt">
                                    <thead>
                                      <tr class="gray_table">
                                        <th>Order Code</th>
                                        <th>QTY</th>
                                        <th >Description</th>
                                      </tr>
                                    </thead>
                                    <tbody>
                                      <tr>
                                        <td><asp:Label ID="lblordercode" runat="server" Text=""></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblQty" runat="server" Text=""></asp:Label>
                                          <%--  <input type="text" id="txtQtypop" runat="server" class="add_input"  style="width:40px; border:1px solid #ccc;"  maxlength="6"/>     --%>                                    
                                                       
                                        </td>
                                        <td ><asp:Label ID="lblDesc" runat="server" Text=""></asp:Label></td>
                                      </tr>
                                    </tbody>
                                  </table>
                                </div>
                              </div>
                              <div class="clear"></div>
                            </div>
                          </div>
      <div class="modal-footer clear border_top_none centered-block">
                            <a class="btn primary-btn-blue ico_continue" onclick="tb_remove();GetCartCount();">Continue Shopping</a>
                            <a class="btn primary-btn-green ico_checkout" href="/OrderDetails.aspx?&bulkorder=1&pid=0">View Order & Check out</a>
                          </div>

      </div>
   <%--<div align="center" runat="server" id="divAddtocart" >
        
        <div>
        <span style="color: #7DBC20; font-family: arial; font-size: 22px; font-weight: bold;">
        <img src="/images/tick.png" alt="tick" style="margin-top: -9px;" /> Item Added to Cart! </span>
        </div>
        <div class="clear"/>
          <div style="display: table-cell;height: 100px;text-align: center;vertical-align: middle;">
          <asp:Image ID="lblProImage"  runat="server"  style="vertical-align: middle;padding:2px;max-width:100px;max-height:100px;" alt="Loading..."  />
          </div>
           <div class=clear/>
           <div><asp:Label ID="lblFamilyName" runat="server" style="color: #222222;font-family: Arial ;font-size: 11px; font-weight: bold;" Text="Label"></asp:Label></div>
         
         <div><table class="orderdettable" style="font-size:9px;height:90px;" cellspacing="0" cellpadding="0" border="0px">
         <tr style="background-color:#E8EDF1;">
         <td>Order Code</td>
         <td>QTY</td>
         <td>Description</td>
         </tr>
         <tr>
         <td><asp:Label ID="lblordercode" runat="server" Text=""></asp:Label></td>
         <td><asp:Label ID="lblQty" runat="server" Text=""></asp:Label></td>
         <td><asp:Label ID="lblDesc" runat="server" Text=""></asp:Label></td>
         </tr>
         </table>
         </div>
         <div class=clear/>
         <div style="padding: 4px; margin-top:3px;">                 
         <a class="button normalsiz btnblue" style="width:200px;line-height: 2.5;" onclick="GetCartCount();tb_remove();"  >Continue Shopping </a>                  
         </div>
         <div class=clear/>
         <div>
        
         <a id="A1" runat="server"  class="button normalsiz btngreen"  style="width:200px;line-height: 2.5;" href="/OrderDetails.aspx?&bulkorder=1&pid=0"  >View Order and Check Out </a>

         </div>

  </div>--%>
   <div>
    <div class="grid12" runat="server" id="divTimeout"  visible="false"    >
    <fieldset>
    <div style="text-align:center;padding-top:120px;Padding-bottom:120px;" >
    <span style="font-size:21px;"  > Your session has timed out</span><br />
    <span style="font-size:14px;"> <a href="/Login.aspx" class="para pad10-0" style="font-size:11px; color:#0033cc; font-weight:bold;">Click here</a> to log in again </span>
    </div>
    </fieldset>
    </div>
</div>
         <%} %>
</asp:Content>
