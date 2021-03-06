<%@ Page Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true"
    Inherits="MOrderDetails" Title="Untitled Page" Culture="en-US" UICulture="en-US"
    CodeBehind="MOrderDetails.aspx.cs" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace="TradingBell.WebCat.CommonServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="GCheckout" Namespace="GCheckout.Checkout" TagPrefix="GCCheckout" %>
<%@ Register Src="UC/BulkOrder.ascx" TagName="BulkOrder" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
    <%if (Request.QueryString["popup"] == null)
      { %>
    <%--<div class="grid12 hidden-phone">
        <div class="mar5 breadcrumb">          
          <a href="<%=micrositeurl %>" style="color: #089fd6 !important;"> Home </a>  &gt; <a href="<%=HttpContext.Current.Request.Url.ToString() %>" style="color: #089fd6 !important;"> Order Details </a>
        </div>
      </div>--%>
    <%} %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="false" EnableCdn="true" EnablePageMethods="true" ScriptMode="Release"  EnablePartialRendering="false" >
        </asp:ScriptManager>
    <%if (Request.QueryString["popup"] == null)
      { %>
    <asp:Panel ID="pnlEnter" runat="server" DefaultButton="lblOrderProceed">
        <%--<link rel="Stylesheet" href="css/thickboxNew.css" type="text/css" />

        <script language="javascript" src="Scripts/thickboxNew.js" type="text/javascript" />
        <script language="javascript" src="Scripts/jquery-Thickbox-New.js" type="text/javascript" />
        --%>
        <script language="javascript" type="text/javascript">
            var pid = "";
            var ProdPrice = "";
            var UntPrice = 0;
            var pgValidated = "no";

            //window.history.forward(1);
            function PopupAdd(FamilyID, ProductID, MinQty, MaxQty, refocus) {
                var Qty = window.open("ProductFeatures.aspx?Fid=" + FamilyID + "&Pid=" + ProductID, "Qty", "width=350,height=250,left=150,top=200,resizable=yes,scrollbars=1");
                if (Qty >= MinQty) {
                    alert(document.forms[0].elements["Returnname"].value);
                    var href = window.parent.location = "/MOrderDetails.aspx?bulkorder=1&amp;Pid=" + ProductID + "&amp;Qty=" + document.forms[0].elements["Returnname"].value;
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
                window.location.href = "/MOrderDetails.aspx";
                if (pid != "") {
                    pid = pid.substr(1, pid.length);
                }
                else {
                    pid = 'AllProd';
                    UntPrice = '0';
                    ProdPrice = "";
                }
                window.location.href = '/MOrderDetails.aspx?bulkorder=1&amp;SelPid=' + pid + '&amp;SelProdPrice=' + UntPrice;
            }

            function UpdQuantity() {
                document.getElementById('updCart').click();
            }



            function SendRemoveProducts() {
                if (String(pid) != "") {
                    pid = pid.substr(1, pid.length);
                    ProdPrice = ProdPrice.substr(1, ProdPrice.length);
                    window.location.href = "/MOrderDetails.aspx?bulkorder=1&amp;SelPid=" + pid + "&amp;SelProdPrice=" + UntPrice + "&amp;ProdPrice=" + ProdPrice;
                    return true;
                }
                else {
                    UntPrice = 0;
                    if (document.forms[0].elements["ChkAllSel"].checked == true) {
                        pid = "AllProd";
                        SelProdPrice = 0;
                        ProdPrice = "";
                        window.location.href = "/MOrderDetails.aspx?bulkorder=1&amp;SelPid=AllProd";
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
                            window.location.href = "/MOrderDetails.aspx?bulkorder=1&amp;SelPid=" + pid + "&amp;SelProdPrice=" + UntPrice + "&amp;ProdPrice=" + ProdPrice;
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
            
        </script>
        <script language="javascript" type="text/javascript">
            function test() {
                jQuery("#TB_window").append("<div id='TB_load'><img src='images/closebtn.png' /></div>"); //add loader to the page
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
                    bool UserCheckout = false;
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
                                //Session["ITEM_ERROR"] = Session["ITEM_ERROR"].ToString().Replace(_rmitem + ",", "");
                                objOrderServices.Remove_Clarification_item(CalItem_ID);
                            }
                            if (_rma == "CI")
                            {
                                //Session["ITEM_CHK"] = Session["ITEM_CHK"].ToString().Replace(_rmitem + ",", "");
                                //Session["QTY_CHK"] = Session["QTY_CHK"].ToString().Replace(_rmqty + ",", "");
                                objOrderServices.Remove_Clarification_item(CalItem_ID);
                            }
                        }


                        LeaveDuplicateProds = GetLeaveDuplicateProducts();
                        String sessionId;
                        // string sessionuserid;
                        sessionId = Session.SessionID;

                        // string dumuserid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
                        DataSet dsDuplicateItem = objOrderServices.GetOrderItemsWithDuplicate(OrderID, LeaveDuplicateProds, sessionId);
                        DataTable tbErrorItem = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_ERROR", sessionId);

                        DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_CHK", "");
                        DataTable tbErrorReplace = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_REPLACE", sessionId);

                        DataSet dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds, sessionId);

                        if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
                        {
                            DupProdCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
                        }

                        //if (Session["ITEM_ERROR"] != null && (Session["ITEM_ERROR"].ToString().Replace(",", "") != "" || Session["ITEM_CHK"].ToString() != "") ||( DupProdCount>0))
                        if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0) || (DupProdCount > 0) || (tbErrorReplace != null && tbErrorReplace.Rows.Count > 0))
                        {%>
                <h4 class="blue_color_text bolder padding_left_right_15 col-lg-12 col-md-12 col-sm-12 col-xs-12 padding_left_right_mob ">
                    Order Clarification/Errors</h4>
                <div class="col-lg-12 col-xs-12 orange_color white_color bolder font_size_16 padding_top_btm_8 ">
                    Please Check Below</div>
                <div class="table-responsive col-lg-12 col-md-12 col-sm-12 col-xs-12 padding_left_right_mob">
                    <table class="table table-bordered">
                        <thead class="view_Cart_head_color">
                            <tr>
                                <th>
                                    ITEMCODE
                                </th>
                                <th>
                                    CLARIFICATION REQUIRED
                                </th>
                                <th>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <% string TempNotFoundItem = "";
                               string _NotFoundItem = "";
                               string _ClaItem_ID = "0";
                               if (tbErrorItem != null && tbErrorItem.Rows.Count > 0)
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
                                <td>
                                    <%=_NotFoundItem%>
                                </td>
                                <td style="color: #92cd01;">
                                    <font color="red" style="font-weight: bold;">Not Found / Incorrect Code</font>
                                </td>
                                <td style="border-right: 0">
                                    <a class="detele_item" href="/MOrderdetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_NotFoundItem%>&amp;cla_id=<%=_ClaItem_ID%>">
                                        Delete Item</a>
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
                                                           string neweapath = "";
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
                                                                           bool samecodesubproduct = false;
                                                                           bool samecodenotFound = false;
                                                                           string wag_product_code = "";
                                                                           string SubstuyutePid = "";
                                                                           int checksetsub = -1;
                                                                           string product_status = "";
                                                                           int StrProductStatusSub = -1;
                                                                           string StrStockStatus = "";
                                                                           int ProdCodeId = 0;

                                                                          /* substituteproduct = objProductServices.GetSubstituteProduct(_replaceItem);


                                                                           if (substituteproduct != null && substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString() != "Not Found" && substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString() != "")
                                                                           {
                                                                               DataSet tmpds1 = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, substituteproduct.Rows[0]["SUBSTITUTEPID"].ToString(), strsupplierId, "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID", HelperDB.ReturnType.RTDataSet);
                                                                               if (tmpds1 != null && tmpds1.Tables.Count > 0)
                                                                               {
                                                                                   _catid = tmpds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                                                                                   pfid = tmpds1.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();
                                                                                   //Ea_Path = EA_New_Product_init_cat_path1 + "////" + tmpds1.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + tmpds1.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();
                                                                                   //neweapath = Ea_Path;
                                                                                   //Ea_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                                                   neweapath = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + tmpds1.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////" + tmpds1.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString() + "=" + tmpds1.Tables[0].Rows[0]["FAMILY_name"].ToString() + "////" + tmpds1.Tables[0].Rows[0]["product_ID"].ToString() + "=" + substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString(), "mpd.aspx", true, "");

                                                                               }
                                                                               if (RItem["PRODUCT_DESC"].ToString() == substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString() || substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString().Trim() == "")
                                                                                   samecodesubproduct = true;
                                                                               else
                                                                               {
                                                                                   wag_product_code = substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString();
                                                                                   DataTable protblsub = (DataTable)objHelperDB.GetGenericPageDataDB("'" + wag_product_code + "'", "GET_BULKORDER_INVENTORY_COUNT_ALL", HelperDB.ReturnType.RTTable);

                                                                                   if (protblsub != null && protblsub.Rows.Count > 0)
                                                                                   {

                                                                                       foreach (DataRow drow in protblsub.Rows)
                                                                                       {
                                                                                           if (drow["product_status"].ToString().ToUpper() == "AVAILABLE")
                                                                                           {
                                                                                               checksetsub = 1;
                                                                                               ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
                                                                                               StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
                                                                                               if (objProductServices.GetStockStatusDesc(StrStockStatus, Userid) == 1)
                                                                                                   StrProductStatusSub = 1;
                                                                                               else
                                                                                                   StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));
                                                                                               product_status = drow["product_status"].ToString();
                                                                                               break;
                                                                                           }
                                                                                       }
                                                                                       if (product_status == "")
                                                                                       {
                                                                                           foreach (DataRow drow in protblsub.Rows)
                                                                                           {
                                                                                               if (drow["product_status"].ToString().ToUpper() != "AVAILABLE")
                                                                                               {
                                                                                                   checksetsub = 2;
                                                                                                   ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
                                                                                                   StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
                                                                                                   if (objProductServices.GetStockStatusDesc(StrStockStatus, Userid) == 1)
                                                                                                       StrProductStatusSub = 1;
                                                                                                   else
                                                                                                       StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));

                                                                                                   product_status = drow["product_status"].ToString();
                                                                                                   break;
                                                                                               }
                                                                                           }
                                                                                       }
                                                                                   }
                                                                                   if (checksetsub == 1 && StrProductStatusSub == 1)
                                                                                   {
                                                                                       samecodesubproduct = false;
                                                                                   }
                                                                                   else
                                                                                       samecodesubproduct = true;

                                                                               }
                                                                           }
                                                                           else
                                                                           {
                                                                               wag_product_code = RItem["PRODUCT_DESC"].ToString();
                                                                               DataTable protblsub = (DataTable)objHelperDB.GetGenericPageDataDB("'" + wag_product_code + "'", "GET_BULKORDER_INVENTORY_COUNT_ALL", HelperDB.ReturnType.RTTable);

                                                                               if (protblsub != null && protblsub.Rows.Count > 0)
                                                                               {
                                                                                   samecodesubproduct = true;
                                                                               }
                                                                               else
                                                                                   samecodenotFound = true;

                                                                           }
                                                                           */
                                                                           rtntbl = objProductServices.GetSubstituteProductDetails(_replaceItem, Userid, "mpd");
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
                                                                           if (samecodenotFound == false && samecodesubproduct == true)
                                                                           {
                                                                               
                                                                               
                            %>
                            <tr>
                                <td>
                                    <%=_replaceItem%>
                                </td>
                                <td style="color: #404040;">
                                    Product Temporarily Unavailable. Please Contact Us for more details
                                </td>
                                <td style="border-right: 0">
                                    <a class="detele_item" href="/MOrderDetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>">
                                        Delete Item</a>
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
                                <td style="color: #92cd01;">
                                    <font color="orange" style="font-weight: bold;">Product Not Available.</font> <font
                                        color="#009900" style="font-weight: bold;">Replaced With:&nbsp;<%=wag_product_code%></font>
                                </td>
                                <td style="border-right: 0">
                                    <a href='<%=Ea_Path %>' class="detele_item">View Item</a>
                                </td>
                                <td style="border-right: 0">
                                    <a href="/MOrderDetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>"
                                        class="detele_item">Delete Item</a>
                                </td>
                                <td style="border-right: 0">
                                    <a href="/MOrderDetails.aspx?bulkorder=1&amp;flgAddItem=chkAddItem&amp;rma=NF&amp;ORDER_ID=<%=OrderID%>&amp;Pid=<%=SubstuyutePid.ToString().Trim()%>&amp;Qty=<%=_ordQty %>&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>"
                                        class="detele_item">Add Item</a>
                                </td>
                            </tr>
                            <%}
                                                                           else
                                                                           {                                                                               
                            %>
                            <tr>
                                <td>
                                    <%=_replaceItem%>
                                </td>
                                <td style="color: #92cd01;">
                                    <font color="red" style="font-weight: bold;">Not Found / Incorrect Code</font>
                                </td>
                                <td style="border-right: 0">
                                    <font color="red" style="font-weight: bold;">Not Found / Incorrect Code</font>
                                </td>
                                <td style="border-right: 0">
                                    <a href="/MOrderDetails.aspx?bulkorder=1&amp;rma=NF&amp;item=<%=_replaceItem%>&amp;cla_id=<%=_ClaItem_ID%>"
                                        class="detele_item">Delete Item</a>
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
                                                           Int32 _orderQty = 0;

                                                           if (tbErrorChk != null && tbErrorChk.Rows.Count > 0)
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
                                <td>
                                    <% =_ClarifyItem%>
                                </td>
                                <td style="color: #92cd01;">
                                    <font color="#ff9900" style="font-weight: bold">Not unique Code</font>
                                </td>
                                <td style="border-right: 0">
                                    <a class="thickbox" href="SubProducts.aspx?Item=<%=_ClarifyItem %>&amp;height=400&amp;width=600&amp;modal=true&amp;OrderID=<%=OrderID%>&amp;ClrQty=<%=_orderQty%>&amp;cla_id=<%=_ClaItem_ID%>"
                                        style="font-weight: bold; text-decoration: none; color: #1589FF;">Clarify Now</a>
                                </td>
                                <td style="border-right: 0">
                                    <a href="/MOrderDetails.aspx?bulkorder=1&amp;rma=CI&amp;item=<%= _ClarifyItem %>&amp;DelQty=<%=_orderQty%>&amp;cla_id=<%=_ClaItem_ID%> "
                                        class="detele_item">Delete Item</a>
                                </td>
                            </tr>
                            <%  
                                                                 
}

                                                                       TempClarifyItem = _ClarifyItem;

                                                                   }
                                                               }
                                                           }
                                                           string dupItem = "";
                                                           int pid1;
                                                           int maxqty1;
                                                           int minQty1;
                                                           int FId1 = 0;
                                                           double orderItemId = 0;
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
                                <td>
                                    <% =temptbl.Rows[0]["CATALOG_ITEM_NO"].ToString().ToUpper() %>
                                </td>
                                <td>
                                    <font color="#ff9900" style="font-weight: bold">Duplicate/Multiple Product Entries Found</font>
                                    <table border="0px" cellpadding="0" cellspacing="0" style="padding-top: 0px; padding-bottom: 0px;
                                        width: 300px;" class="table table-bordered">
                                        <tr>
                                            <td width="100">
                                                <strong>Order Code </strong>
                                            </td>
                                            <td width="100" align="right">
                                                <strong>QTY</strong>
                                            </td>
                                            <td width="100" align="center">
                                                <font style="font-weight: bold"></font>
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
                                            <td width="100" align="left">
                                                <font style="font-weight: bold">
                                                    <% =tmpItem["CATALOG_ITEM_NO"].ToString().ToUpper() %>
                                                </font>
                                            </td>
                                            <td width="100" align="right">
                                                <font style="font-weight: bold; font-size: 12px;">
                                                    <% =tmpItem["QTY"].ToString().ToUpper() %>
                                                </font>
                                            </td>
                                            <td width="100" align="center">
                                                <a href="/MOrderDetails.aspx?bulkorder=1&amp;SelPid=<%=pid1%>&amp;SelProdPrice=<%=ProdTotal1%>&amp;ProdPrice=<%=ProductUnitPrice1%>&amp;ORDER_ID=<%=OrderID%>&amp;ORDER_ITEM_ID=<%=orderItemId%> "
                                                    style="color: #0099ff">Remove</a>
                                            </td>
                                        </tr>
                                        <%
                                                                                
}
                                                                            
                                        %>
                                        <tr>
                                            <td colspan="3" align="left">
                                                <a href="/MOrderDetails.aspx?bulkorder=1&amp;ORDER_ID=<%=OrderID%>&amp;CombainProd_id=<%=pid1%>"
                                                    style="color: #0099ff">Combine all into one (QTY discount may apply)</a>
                                                <br />
                                                <a href="/MOrderDetails.aspx?bulkorder=1&amp;ORDER_ID=<%=OrderID%>&amp;LeaveProd_id=<%=pid1%>"
                                                    style="color: #0099ff">Leave as multiple lines</a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
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
                <h4 class="blue_color_text bolder padding_left_right_15 col-lg-12 col-md-12 col-sm-12 col-xs-12  padding_left_right_mob">
                    Shopping cart contents</h4>
                <div class="col-lg-12 col-xs-12 blue_color white_color bolder font_size_16 padding_top_btm_8">
                    Order Contents</div>
                <div class="table-responsive col-lg-12 col-md-12 col-sm-12 col-xs-12 padding_left_right_mob">
                    <asp:Label ID="Label1" runat="server" Text="Order No : " Visible="false"></asp:Label>
                    <asp:Label ID="lblOrdNo" runat="server" Visible="false"></asp:Label>
                    <table class="table table-bordered">
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
                                    Cost(EX.GST)
                                </th>
                                <th>
                                    Extention Amount (EX.GST)
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <%   	   
                                string EA_root_Cat_path = System.Configuration.ConfigurationManager.AppSettings["EA_ROOT_CATEGORY_PATH"].ToString();
                                string EA_New_Product_init_cat_path = System.Configuration.ConfigurationManager.AppSettings["EA_NEW_PRODUCT_INIT_CATEGORY_PATH"].ToString();



                                //dsOItem = objOrderServices.GetOrderItems(OrderID);
                                String sessionId_dum;
                                // string sessionuserid_dum;
                                sessionId_dum = Session.SessionID;
                                dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, LeaveDuplicateProds, sessionId_dum);

                                string cSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                                decimal ProdShippCost = 0.00M;
                                decimal TotalShipCost = 0.00M;


                                SelProductId = "";
                                if (OrdStatus == OrderServices.OrderStatus.OPEN.ToString() || OrdStatus == "CAU_PENDING")
                                {
                                    if (dsOItem != null)
                                    {
                                        int i = 0;
                                        String sessionId;
                                        sessionId = Session.SessionID;
                                        string prdsessionid = "";
                                        string chkuserID = HttpContext.Current.Session["USER_ID"].ToString();
                                        bool flgchkdummy = false;


                                        foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                                        {
                                            flgchkdummy = false;
                                            if (chkuserID != ConfigurationManager.AppSettings["DUM_USER_ID"].ToString())
                                                flgchkdummy = true;
                                            else
                                            {
                                                if (chkuserID == ConfigurationManager.AppSettings["DUM_USER_ID"].ToString() && sessionId == dsOItem.Tables[0].Rows[i]["PRD_SESSION_ID"].ToString())
                                                    flgchkdummy = true;
                                                else
                                                    flgchkdummy = false;
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
                                                DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, pid.ToString(), strsupplierId, "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID", HelperDB.ReturnType.RTDataSet);
                                                if (tmpds != null && tmpds.Tables.Count > 0)
                                                {
                                                    //_catid = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                                                    //pfid = tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();
                                                    // Ea_Path = EA_New_Product_init_cat_path + "////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();
                                                    // neweapath = Ea_Path.Replace(" ", "_"); ;                                                    Ea_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                                                    string familyname = string.Empty;
                                                    if (tmpds.Tables[0].Columns.Contains("FAMILY_name") == true)
                                                    {
                                                        familyname = tmpds.Tables[0].Rows[0]["FAMILY_name"].ToString();
                                                    }
                                                    string productcode = string.Empty;
                                                    if (tmpds.Tables[0].Columns.Contains("product_ID") == true)
                                                    {
                                                        productcode = tmpds.Tables[0].Rows[0]["product_ID"].ToString();
                                                    }

                                                    string[] catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                                                    neweapath = objHelperServices.SimpleURL_MS_Str( "AllProducts////WESAUSTRALASIA////" + "////"
                                                        + (catpath.Length >= 2 ? catpath[1] : " ") +"////"+  (catpath.Length >= 3 ? catpath[2] : " ")+ 
                                                        "////" + familyname + "////"
                                                        + productcode + "=" + rItem["CATALOG_ITEM_NO"].ToString() + "////" + tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString(), "mpd.aspx",true);

                                                    neweapath = neweapath + "/mpd/";
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
                              
                                    <%=
                                        
                                        "<a href =\""+ neweapath +"\"  style=\"color: #666;\">" + rItem["CATALOG_ITEM_NO"].ToString() + " </a>"
                                        
                                        
                                        %>
                                </td>
                              
                                <td>
                                    <%="<input type =\"Text\" Id=\"txtQtyId" + OrderItemId1 + "_" + maxqty + "\" Name =\"txtQty" + OrderItemId1 + "\" size=\"4\" onkeydown=\"return keyct(event)\"  maxlength=\"6\" class=\"inpt-qty\" value =\"" + Qty + "\">"%>
                                    &nbsp;&nbsp;<a href="/MOrderDetails.aspx?bulkorder=1&amp;SelPid=<%=pid%>&amp;SelProdPrice=<%=ProdTotal%>&amp;ProdPrice=<%=ProductUnitPrice%>&amp;ORDER_ID=<%=OrderID%>&amp;ORDER_ITEM_ID=<%=OrderItemId1%>"
                                        class="toplinkatest" style="color: #666;">Remove</a>&nbsp;&nbsp;
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
                                <td bgcolor="White" class="style21OrdDetail" style="border-style: none none none solid;
                                    border-width: thin; border-color: #E7E7E7">
                                    <%=Desc %>
                                </td>
                                <td bgcolor="White" class="style20OrdDetail" style="border-style: none none none solid;
                                    border-width: thin; border-color: #E7E7E7">
                                    <%=Available%>
                                </td>
                                <td align="center" bgcolor="White" class="style23OrdDetail" style="width: 130px"
                                    style="border-style: none none none solid; border-width: thin; border-color: #E7E7E7">
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
                                <td bgcolor="White" class="style21OrdDetail" style="border-style: none none none solid;
                                    border-width: thin; border-color: #E7E7E7">
                                    <%=Desc %>
                                </td>
                                <td bgcolor="White" class="style20OrdDetail" style="border-style: none none none solid;
                                    border-width: thin; border-color: #E7E7E7">
                                    <%=Available%>
                                </td>
                                <td align="center" bgcolor="White" class="style23OrdDetail" style="width: 130px"
                                    style="border-style: none none none solid; border-width: thin; border-color: #E7E7E7">
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
                                               if (sessionId != "" && flgchkdummy == false)
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
                                <td colspan="3" rowspan="3">
                                    <asp:ImageButton ID="ImageButton2" runat="server" ClientIDMode="Static"
                                        OnClientClick="javascript:return  setTimeout('SendRemoveProducts()',100);" />
                                </td>
                                <td>
                                    <strong>Sub Total</strong>
                                </td>
                                <td style="border-right: 0">
                                    <%=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(subtot))%>
                                </td>
                            </tr>
                            <tr class="border_none">
                                <td>
                                    Tax Amount(GST)<br />
                                    <span style="font-size: 4"></span>
                                </td>
                                <td>
                                    <%       
                                            
                                        //string sSQL = string.Format("SELECT isnull(TAX_AMOUNT,0) [TAX_AMOUNT] FROM TBWC_ORDER WHERE ORDER_ID = {0}", OrderID);
                                        //objHelperServices.SQLString = sSQL;
                                        //taxamt = System.Convert.ToDecimal(oHelper.GetValue("TAX_AMOUNT"));
                                        if (subtot > 0)
                                        {
                                            taxamt = objOrderServices.CalculateTaxAmount(subtot, OrderID.ToString());   //Math.Round((subtot * 10 / 100),2,MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            taxamt = 0;
                                        }
                                        Total = subtot + taxamt + TotalShipCost;
                                        Total = objHelperServices.CDEC(objHelperServices.FixDecPlace(Total));%>
                                    <%=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(taxamt))%>
                                </td>
                            </tr>
                            <tr class="border_none">
                                <td class="pdglft ttlamt">
                                    <% 
                                          
                                        if (objOrderServices.IsNativeCountry(OrderID) == 0)
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
                                <td class="pdglft ttlamt" style="border-right: 0">
                                    <strong>
                                        <%=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(Total))%>
                                    </strong>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="cl">
                    </div>
                </div>
                <div class="col-lg-12 text-right clear">
                    *Please Note: Above prices are not final and subject to our confirmation. Total
                    price shown above does not include freight.
                </div>
                <div class="col-lg-12 text-right margin_top margin_bottom_30">
                    <asp:Button ID="BtnOrdTemplate" runat="server" OnClick="btnSaveOrdTemplate_Click"
                        Text="Save as Template" Style="margin: 10px 0px 10px 200px;" class="buttongray normalsiz btngray fleft"
                        Visible="false" />
                    <asp:Button ID="updCart" runat="server" OnClick="btnUpdateCart_Click" Text="Update Order Qty"
                        class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14" />
                    <%
                        UserCheckout = objHelperServices.GetIsEcomEnabled(Userid.ToString());
                                                
                    %>
                    <% if (UserCheckout == true)
                       { %>
                    <asp:Button ID="ImageButton4" runat="server" OnClick="btnContinueNext_Click" Text="Continue Shopping"
                        class="btn-lg gray_40_bg border_none border_radius_none white_color semi_bold font_14" />
                    <%} %>
                    <% if (UserCheckout == true)
                       { %>
                    <asp:Button ID="lblOrderProceed" runat="server" OnClick="btnNext_Click" Text="Go to Check Out"
                        class="btn-lg blue_color border_none border_radius_none white_color semi_bold font_14" />
                    <%}
                       else
                       { %>
                    <asp:Button ID="ImageButton5" runat="server" OnClick="btnContinueNext_Click" Text="Continue Shopping"
                        class="btn-lg gray_40_bg border_none border_radius_none white_color semi_bold font_14" />
                    <%} %>
                </div>
                <div class="cl">
                </div>
                <div id="PopupMsg" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel"
                    role="dialog" tabindex="-1" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6);
                    display: block;" class="modal fade bs-example-modal-lg in">
                    <div class="modal-dialog  margin_top_popup20 ">
                        <div class="modal-content  border_radius_none">
                            <div class="modal-body">
                                <p class="alert-red">
                                    Please review and correct Order Clarification / Errors before proceeding to Check
                                    Out!
                                </p>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="btn-lg padding_top btn-danger border_none border_radius_none white_color semi_bold font_14 mar_right_30  mob_100 margin_left"
                                    OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <%--<div id="PopupMsg">
                    <asp:Panel ID="ModalPanel" runat="server" Style="display: none" BackColor="White"
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
                                    
                                </td>
                                <td width="5%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>--%>
                <div id="DivAlert">
                    <asp:Panel ID="pnlAlert" runat="server" Style="display: none" BackColor="White" Height="100px"
                        Width="350px" BorderStyle="Solid" BorderWidth="2px" BorderColor="#0077cc">
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
                                    <asp:Button ID="btnok" runat="server" Text="Ok" Width="55px" Font-Bold="true" ForeColor="#1589FF" />
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
                            Response.Redirect("ConfirmMessage.aspx?Result=NOPRICEAMT", false);
                        }
                        else
                        {
                            if (Request.QueryString["bulkorder"] != null && Request.QueryString["bulkorder"].ToString() == "1")
                            {

                            }
                            else
                            {
                                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", false);
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
    </asp:Panel>
    <%} %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Popupcontent" runat="Server">
    <%-- <asp:HiddenField runat="server" ID="txtordidpop" />
      <asp:HiddenField runat="server" ID="txtpidpop" />--%>
    <script type="text/javascript">

        //$(document).ready(function () {
        //     $("#ctl00_Popupcontent_txtQtypop").keypress(function (e) {
        //        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        //            return false;
        //        }
        //    });
        //});
        //$(document).ready(function () {
        //    $("#ctl00_Popupcontent_txtQtypop").focusout(function () {
        //        var qty = document.getElementById("ctl00_Popupcontent_txtQtypop").value;
        //        var ordid = document.getElementById("ctl00_Popupcontent_txtordidpop").value;
        //        var pid = document.getElementById("ctl00_Popupcontent_txtpidpop").value;

        //        $.ajax({
        //            type: "POST",
        //            url: "/MOrderDetails.aspx/UpdateOrderQty",
        //            data: "{'Qty':'" + qty + "','OrdId':'" + ordid + "','pid':'" + pid + "'}",
        //            contentType: "application/json;charset=utf-8",
        //            dataType: "json"
        //            //success: OnmailSuccess,
        //            // error: OnmailFailure


        //        });


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
    </script>
    <%if (Request.QueryString["popup"] != null)
      { %>
    <%--<div align="center" runat="server" id="divAddtocart" >
        
        <div><span style="color: #7DBC20; font-family: arial; font-size: 22px; font-weight: bold;"><img src="/images/tick.png" alt="tick" style="margin-top: -9px;" /> Item Added to Cart! </span></div>
        <div class=clear/>
          <div style="display: table-cell;height: 100px;text-align: center;vertical-align: middle;">
          <asp:Image ID="lblProImage"  runat="server"  style="vertical-align: middle;padding:2px;" alt="Loading..."  />
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
        
         <a id="A1" runat="server"  class="button normalsiz btngreen"  style="width:200px;line-height: 2.5;" href="/MOrderDetails.aspx?&bulkorder=1&pid=0"  >View Order and Check Out </a>

         </div>

  </div>--%>
    <div runat="server" id="divAddtocart">
  
        <div class="modal-header green_bg">
            <h4 id="myModalLabel" class="modal-titl white_color bolder font_15 text-center">
                <img class=" margin_right" alt="iatc" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/MicroSiteimages/popup_tick.png" />Item
                Added to Cart</h4>
        </div>
        <div class="modal-body">
            <div class="col-lg-12">
                <div class="col-lg-12 text-center col-sm-6 col-md-4">
                    <div class="thumb_img_size_atcp">
                        <asp:Image ID="lblProImage" runat="server" Style="max-width: 175px; max-height: 175px;"
                            CssClass="img-responsive" alt="Loading..." />
                    </div>
                    <%-- </br>--%>
                    <p class="font_14 semi_bold gray_40 margin_top_20">
                        <asp:Label ID="lblFamilyName" runat="server" Text="Label"></asp:Label>
                    </p>
                    <%--<p class="font_14 semi_bold gray_40 margin_top_20">Copy of Propack 16 channel kit</p>--%>
                </div>
                <div class="col-lg-12 col-sm-6 col-xs-12 col-md-8">
                    <div>
                        <%--class="table-responsive"--%>
                        <table class="table table-bordered">
                            <thead>
                                <tr class="gray_table">
                                    <th>
                                        Order Code
                                    </th>
                                    <th>
                                        QTY
                                    </th>
                                    <th >
                                        Description
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblordercode" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblQty" runat="server" Text=""></asp:Label>
                                         <%--<input type="text" id="txtQtypop" runat="server" class="add_input"  style="width:40px; border:1px solid #ccc;"  maxlength="6"/>--%>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblDesc" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="modal-footer clear border_top_none crt_popup">
            <%-- <button class="btn-lg blue_color border_none border_radius_none white_color semi_bold font_14 mar_right_30"  onclick="GetCartCount();tb_remove();">Continue Shopping</button>
 <button class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub">View Order &amp; Checkout</button>--%>
            <%--       <a class="btn-lg blue_color border_none border_radius_none white_color semi_bold font_14 mar_right_30" onclick="GetCartCount();tb_remove();">Continue Shopping</a>
       <a id="A1" class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub" href="/MOrderDetails.aspx?&bulkorder=1&pid=0">View Order &amp; Checkout</a>--%>
            <asp:Button runat="server" class="btn-lg mob_100 blue_color border_none border_radius_none white_color semi_bold font_14 mar_right_15 mob_btn con_shopping"
                Text="Continue Shopping" OnClientClick="GetCartCount();tb_remove();return false;"
                AutoPostBack="FALSE" />
            <%--       <asp:Button runat="server" id="A1" class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub" Text="View Order & Checkout" OnClientClick="MSChkoutRedirect();" />--%>
            <%-- <input type="button" value="Continue Shopping" class="btn-lg blue_color border_none border_radius_none white_color semi_bold font_14 mar_right_30"  onclick ="GetCartCount();tb_remove();"/>
              <input runat="server"  class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub" value="View Order & Checkout" onclick="MSChkoutRedirect();" />--%>
            <input runat="server" type="submit" class="btn-lg mob_100 border_none border_radius_none white_color semi_bold font_14  reg_sub cart_bg_btn"
                value="View Order & Check Out" onclick="MSChkoutRedirect();return false;" style="cursor: pointer;" />
        </div>
    </div>
    <%-- </div>--%>
    <div class="grid12" runat="server" id="divTimeout" visible="false">
        <fieldset>
            <div style="text-align: center; padding-top: 120px; padding-bottom: 120px;">
                <span style="font-size: 21px;">Your session has timed out</span><br />
                <span style="font-size: 14px;"><a href="/Login.aspx" class="para pad10-0" style="font-size: 11px;
                    color: #0033cc; font-weight: bold;">Click here</a> to log in again </span>
            </div>
        </fieldset>
    </div>
    <%--</div>--%>
    <%} %>
</asp:Content>
