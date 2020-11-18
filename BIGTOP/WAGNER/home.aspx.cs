using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Diagnostics;
using System.Configuration;
using System.Text;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.EasyAsk;

public partial class homepageST : System.Web.UI.Page
{
    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    ErrorHandler objErrorHandler = new ErrorHandler();
    OrderServices objOrderServices = new OrderServices();
    OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
    HelperServices objHelperServices = new HelperServices();
    UserServices objUserServices = new UserServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperDB objHelperDB = new HelperDB();
    Security objSecurity = new Security();
    ProductServices objProductServices = new ProductServices();

    string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
    string performance = System.Configuration.ConfigurationManager.AppSettings["performance"];

    DataSet dsrecordsS = new DataSet();
    DataSet dsrecordsM = new DataSet();
    ErrorHandler objErrorhandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {
       
   
        //Stopwatch sw = new Stopwatch();
        if (Request.QueryString["code"]!=null)
        { 
            PayPalService paysc = new PayPalService();
            // paysc.();
            PayPalService.AccountInfo ac = new PayPalService.AccountInfo();
       ac=paysc.getuserinfo(Request.QueryString["code"]  );
           
foreach(var itm in ac.emails)
            {

                objErrorhandler.CreateLog(itm.Value+ ac.address.street_address+ac.address.country );
               
            }
            //if (objUserServices.CheckUserName(ac.emails[0].Value))
            //{
            //    loginpaypalUser(ac.emails[0].Value);
            //}
            //else
            //{
            //    string firstname = string.Empty;
            //    string lastname = string.Empty;
            //    string[] name= ac.name.Split(' ');
            //    firstname = name[0];
            //    if (name.Length > 0)
            //    {
            //        lastname = name[1];
            //    }
            //    registerpaypaluser(ac.emails[0].Value, firstname, lastname,"");
            //}
            objErrorhandler.CreateLog();
            //foreach (var itm in ac.address)
            //{

            //    objErrorhandler.CreateLog(itm.StreetAddress);
            //}
        }
        HelperServices objhelp = new HelperServices();
        btnclose.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/close_btn.png";
        try
        {
          //  objhelp.writelog(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() + "," + HttpContext.Current.Request.Url.ToString().ToLower() + "," + HttpContext.Current.Request.UserAgent + "," + ((HttpContext.Current.Session["USER_ID"] != null) ? HttpContext.Current.Session["USER_ID"].ToString() : "0"));
            //objErrorHandler.CreateLog("Home Page");

            if (Session["IP_ADDR"] == null)
            {
                Session["IP_ADDR"] = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }

                
        HelperServices objHelperServices = new HelperServices();
        //sw.Start();
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        //Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
       // Page.Title = "Wagner Online Super Store";
      //  Page.Title = "Wagner Online Store. Audio Speakers, Automotive, AV TV SAT Installation, Cables, Connectors, Cellular Accessories and More";
        //Page.Title = "Wagner Online Store | Wide range of products and accessories";

            string sPageExtention = objHelperServices.GetOptionValues("PAGE EXTENTION");
        Session["PageExtention"] = sPageExtention == "-1" ? "" : sPageExtention;
      
           // objHelperServices.Createnewdt_Home();

            objOrderServices.ChkDelete_PreviousDupOrderItem(Convert.ToInt32(ConfigurationManager.AppSettings["DUM_USER_ID"])); 

        if (!IsPostBack)
        {
          
            if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
            {
                //string strHostName = System.Net.Dns.GetHostName();
                //string clientIPAddress = string.Empty;
                //if (System.Net.Dns.GetHostAddresses(strHostName) != null)
                //{
                //    if (System.Net.Dns.GetHostAddresses(strHostName).Length <= 1)
                //        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                //    else
                //        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
                //}

                DataSet ordds = new DataSet();
                //int orduseridupdate = 0;
                int orditemuseridupdate = 0;
                int newordid = 0;
                int getnewordid = 0;
                int order_id = 0;
                //int sessionflgupdate = 0;
                int chkcurordid = 0;
                string userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
            
                if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString()) == 4)
                {
                  
                    order_id = objOrderServices.GetOrderID(Convert.ToInt32(userid));
                
                    if (order_id > 0)
                    {
                        ordds = objOrderServices.GetOrderItemsAmtDetails(order_id);
                      
                        String sessionId;
                        sessionId = Session.SessionID;
                     
                        if ( sessionId != "" && HttpContext.Current.Session["DUMMY_FLAG"] == "1")
                        { 
                            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "" && HttpContext.Current.Session["DUMMY_FLAG"].Equals("1"))
                            {
                                Session["DUMMY_FLAG"] = "0";
                                oOrdInfo.UserID = Convert.ToInt32(HttpContext.Current.Session["USER_ID"]);
                                chkcurordid = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), 1);
                                if (chkcurordid == 0)
                                    newordid = objOrderServices.InitilizeOrder(oOrdInfo);
                                getnewordid = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), 1);
                             
                                orditemuseridupdate = objOrderServices.OrderItemsUpdate_UserId(order_id, Convert.ToInt32(HttpContext.Current.Session["USER_ID"]), sessionId, getnewordid, sessionId);
                                if (getnewordid > 0)
                                {
                                    Session["ORDER_ID"] = getnewordid;
                                    oOrdInfo.OrderID = getnewordid;
                                    objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                                }
                            

                                if (orditemuseridupdate > 0)
                                    Response.Redirect("/OrderDetails.aspx?ORDER_ID=" + getnewordid + "&bulkorder=1&ViewOrder=View",false);

                            }

                        }
                    }
                    CallRetailerMsg();
                }
                else
                {                                                   
                    order_id = objOrderServices.GetOrderID(Convert.ToInt32(userid));
               
                    if (order_id > 0)
                    {
                        ordds = objOrderServices.GetOrderItemsAmtDetails(order_id);
                       
                        String sessionId;
                        sessionId = Session.SessionID;
                     
                        if (sessionId != "" && HttpContext.Current.Session["DUMMY_FLAG"] == "1")
                        {
                            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "" && HttpContext.Current.Session["DUMMY_FLAG"] == "1")
                            {

                             
                                Session["DUMMY_FLAG"] = "0";
                                //Session["PrevOrderID"] = "0";
                                
                                oOrdInfo.UserID = Convert.ToInt32(HttpContext.Current.Session["USER_ID"]);

                               // chkcurordid = objOrderServices.OrderId_checkcurruserid(Convert.ToInt32(HttpContext.Current.Session["USER_ID"]));
                                chkcurordid = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), 1);
                             
                                if (chkcurordid == 0)
                                 newordid = objOrderServices.InitilizeOrder(oOrdInfo);                            

                                getnewordid = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()),1);
                            
                                orditemuseridupdate = objOrderServices.OrderItemsUpdate_UserId(order_id, Convert.ToInt32(HttpContext.Current.Session["USER_ID"]), sessionId, getnewordid, sessionId);
                             
                                if (getnewordid > 0 )
                                {
                                    Session["ORDER_ID"] = getnewordid;
                                    oOrdInfo.OrderID = getnewordid;
                                    objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                                }
                            

                                if(orditemuseridupdate > 0)
                                Response.Redirect("/OrderDetails.aspx?ORDER_ID=" + getnewordid + "&bulkorder=1&ViewOrder=View",false);
                               
                            }
                            
                        }

                      
                    }

                    CallOrderMsg();
                }
            }
            else
            {
              
                CallOrderMsg();

            }

        }
        
       // ContinueOrder.Attributes.Add("onmouseover","javascipt:MouseHover(1);");
       // ContinueOrder.Attributes.Add("onmouseout","javascipt:MouseOut(1);");

       // ClearOrder.Attributes.Add("onmouseover", "javascipt:MouseHover(2);");
       // ClearOrder.Attributes.Add("onmouseout", "javascipt:MouseOut(2);");

        btnCancel.Attributes.Add("onmouseover", "javascipt:MouseHover(3);");
        btnCancel.Attributes.Add("onmouseout", "javascipt:MouseOut(3);");

        }
        catch (System.Threading.ThreadAbortException)
        {
            // ignore it
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        //sw.Stop();
        //Console.WriteLine("Elapsed={0}", sw.Elapsed);

        //StackTrace st = new StackTrace();
        //StackFrame sf = st.GetFrame(0);

        //objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed.TotalSeconds.ToString();
        //// objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed;
        //objErrorHandler.CreateTimeLog(); 
        Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();

        //if (performance == "1")
        //{
        //    DataSet dsnewProduct = new DataSet();

        //    if (IsPostBack == false)
        //    {
        //        dsnewProduct = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_ASPX 10 ");
        //        if (dsnewProduct != null && dsnewProduct.Tables[0].Rows.Count > 0)
        //        {
        //            dsnewProduct.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
        //            foreach (DataRow dr in dsnewProduct.Tables[0].Rows)
        //            {
        //                dr["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"].ToString() + "////" + dr["FAMILY_ID"].ToString() + "=" + dr["FAMILY_name"].ToString() + "////" + dr["product_ID"].ToString() + "=" + dr["Code"].ToString(), "pd.aspx", true, "");

        //            }
        //        }

        //        Repeater1.DataSource = dsnewProduct;
        //        Repeater1.DataBind();
        //    }



            
            
        //     dsrecordsM = EasyAsk.GetCategoryAndBrand("MainCategory");
        //     dsrecordsS = EasyAsk.GetCategoryAndBrand("SubCategory");
             
        //    double cnt=0;
        //    int cntchk=0;
        //    string sttr = "";
        //    //if (dsrecordsM != null && dsrecordsM.Tables[0].Rows.Count > 0 && dsrecordsM.Tables[0].Columns.Contains("URL_RW_PATH")==false )
        //    // {
        //    //     dsrecordsM.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
        //    //     DataColumn dc = new DataColumn("SUB_COUNT", typeof(int));
                 
        //    //     dc.DefaultValue = 0;
        //    //     dsrecordsM.Tables[0].Columns.Add(dc);
        //    //     dsrecordsM.Tables[0].Columns.Add("CATEGORY_NAME_TOP", typeof(string));
        //    // }
        //    //if (dsrecordsS != null && dsrecordsS.Tables[0].Rows.Count > 0 && dsrecordsS.Tables[0].Columns.Contains("URL_RW_PATH") == false)
        //    // {
        //    //     dsrecordsS.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
        //    //     dsrecordsS.Tables[0].Columns.Add("PART1", typeof(string));                 
        //    // }
        //    //if (dsrecordsM != null && dsrecordsM.Tables[0].Rows.Count > 0)
        //    //{
                
        //    //    foreach (DataRow dr in dsrecordsM.Tables[0].Rows)
        //    //    {                    
        //    //        dr["URL_RW_PATH"]=objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_NAME"].ToString(), "ct.aspx", true, "");
        //    //        sttr = dr["CATEGORY_NAME"].ToString();
        //    //        int indx = sttr.IndexOf(" ", 7);
        //    //        if (indx >= 7)
        //    //            sttr = sttr.Substring(0, indx) + "<br/>" + sttr.Substring(indx + 1);
        //    //        else if (sttr.Equals("VCR COMPONENTS"))
        //    //            sttr = "VCR<br/>COMPONENTS";
        //    //        dr["CATEGORY_NAME_TOP"] = sttr;


        //    //        cntchk = 0;
        //    //        if (dsrecordsS != null && dsrecordsS.Tables[0].Rows.Count > 0)
        //    //        {

        //    //            DataRow[] drs = dsrecordsS.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + dr["CATEGORY_ID"].ToString() + "'");
        //    //            if (drs.Length > 0)
        //    //            {
        //    //                dr["SUB_COUNT"] = drs.Length;
        //    //                cnt = drs.Length;
        //    //                if (drs.Length > 6)
        //    //                {
        //    //                    cnt = 6;   
        //    //                }
        //    //                foreach (DataRow dr1 in drs)
        //    //                {
        //    //                    cntchk=cntchk+1;
        //    //                    dr1["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr1["EA_PATH"].ToString() + "////" + dr1["TBT_PARENT_CATEGORY_NAME"].ToString() + "////" + dr1["CATEGORY_NAME"].ToString(), "pl.aspx", true, "");
        //    //                    if (cntchk <= cnt)
        //    //                    {
        //    //                        dr1["PART1"] = "1";
        //    //                    }
        //    //                    else
        //    //                        dr1["PART1"] = "2";
        //    //                }
        //    //            }
        //    //        }

        //    //    }
        //    //}


        //    Label1.Text = cartcountlog();

        //    RepeaterTop.DataSource = dsrecordsM.Tables[0].Select("CATEGORY_ID<>'WESNEWS'", "CATEGORY_NAME").CopyToDataTable();
        //        RepeaterTop.DataBind();

        //        RepeaterBottom.DataSource = dsrecordsM.Tables[0].Select("CATEGORY_ID<>'WESNEWS'", "CATEGORY_NAME").CopyToDataTable();
        //        RepeaterBottom.DataBind();

        //    RepeaterCat.DataSource = dsrecordsM.Tables[0].Select("SUB_COUNT>0","CATEGORY_NAME").CopyToDataTable() ;
        //    RepeaterCat.DataBind();
        //}

    }
    //protected object UrlEncode(object obj, Boolean Encrypt, Boolean urlencode)
    //{
    //    return objSecurity.UrlEncode(obj, Encrypt, urlencode);
    //}
    //protected DataTable GetSubCategory(object obj,object td)
    //{
    //    string caid = obj.ToString();
    //    DataRow[] dr = dsrecordsS.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + caid + "' And PART1='" + td.ToString() +"'") ;
    //    if (dr.Length > 0)
    //    {
    //        return dr.Take(6).CopyToDataTable();
    //    }
    //    return null;

    //}
    //protected string GetImagePath(object Path)
    //{
    //    string retpath;
    //    //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + Path.ToString());
    //    //if (Fil.Exists)
    //    //{
    //    //    retpath = Path.ToString();
    //    //    //retpath = objHelperServices.SetImageFolderPath(Path.ToString().Replace("\\", "/"), "_th", "_Images_200");
    //    //}
    //    //else
    //    //    retpath = "/images/noimage.gif";
    //    retpath = Path.ToString();
    //    return retpath;
    //}
    private void CallOrderMsg()
    {
      
        this.ModalPanel1.Visible = false;
        this.modalPop.Hide();
        modalPop = new AjaxControlToolkit.ModalPopupExtender();
       
        if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
        {
            ModalPanel.Visible = true;
            imgclose.Src = System.Configuration.ConfigurationManager.AppSettings["cdn"].ToString()+"/images/Order__info.png"; 
            modalPop.ID = "popUp";
            modalPop.PopupControlID = "ModalPanel";
            modalPop.BackgroundCssClass = "modalBackground";
            modalPop.DropShadow = false;
            modalPop.TargetControlID = "btnHiddenTestPopupExtender";
            this.ModalPanel.Controls.Add(modalPop);
            this.modalPop.Show();
        }
        else
        {
            this.ModalPanel.Visible = false;            
            this.modalPop.Hide();
        }
    }
    private void CallRetailerMsg()
    {
        this.ModalPanel.Visible = false;
        this.modalPop.Hide();
        modalPop = new AjaxControlToolkit.ModalPopupExtender();
        
        if (Session["RetailerMsg"] == null)
        {

     
          
            ModalPanel1.Visible = true;
            modalPop.ID = "popUp1";
            modalPop.PopupControlID = "ModalPanel1";
            modalPop.BackgroundCssClass = "modalBackground";
            modalPop.DropShadow = false;
            modalPop.TargetControlID = "btnHiddenTestPopupExtender";
            this.ModalPanel1.Controls.Add(modalPop);
            this.modalPop.Show();
        }
        else
        {
           
            this.ModalPanel1.Visible = false;
            this.modalPop.Hide();
        }
    }
    protected void btnContinueOrder_Click(object sender, EventArgs e)
    {
        try
        {
            //objErrorHandler.CreateLog("inside btncondi");

        if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
        {
            this.modalPop.Hide();
            Session["ORDER_ID"] = Session["PrevOrderID"];
            Session["PrevOrderID"] = "0";
            int OrderId = Convert.ToInt32(Session["ORDER_ID"]);
            Session["PrevOrderID"] = "0";

            //Added By indu to get upfdated price and stock in orderItem
            try
            {
                if (objOrderServices.GetOrderItemCount(OrderId) > 0)
                {
                    ErrorHandler objErrorHandler = new ErrorHandler();
                    objErrorHandler.CreateLog("inside continue");
                    DataSet oDSOrderItems = objOrderServices.GetOrderItems(OrderId);
                    decimal TempShipCost = 0;
                    for (int i = 0; i < oDSOrderItems.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            int PrdId = objHelperServices.CI(oDSOrderItems.Tables[0].Rows[i]["product_id"].ToString());
                            double order_item_id = objHelperServices.CD(oDSOrderItems.Tables[0].Rows[i]["order_item_id"].ToString());

                            int pQty = objHelperServices.CI(oDSOrderItems.Tables[0].Rows[i]["QTY"].ToString());

                            objErrorHandler.CreateLog("Remove continue" + PrdId);

                            objOrderServices.RemoveItem(PrdId.ToString(), OrderId, objHelperServices.CI(Session["USER_ID"]), order_item_id.ToString());

                            OrderDetails frmord = new OrderDetails();
                            frmord.NewProduct_id = PrdId;
                            frmord.NewQty = pQty;

                            frmord.AddOrderItem(OrderId, objHelperServices.CI(Session["USER_ID"]));
                        }
                        catch (Exception ex)
                        {

                            objErrorHandler.CreateLog(ex.ToString());
                        }

                    }


                    DataTable tbErrorItem = objOrderServices.GetOrder_Clarification_Items(OrderId, "","");
                    if (tbErrorItem != null)
                    {
                        for (int i = 0; i < tbErrorItem.Rows.Count; i++)
                        {
                            try
                            {
                                string product_code = tbErrorItem.Rows[i]["product_desc"].ToString();
                                int PrdId = objHelperServices.CI(objProductServices.GetProductID_code(product_code));
                                double CalItem_ID = objHelperServices.CD(tbErrorItem.Rows[i]["CLARIFICATION_ID"].ToString());

                                int pQty = objHelperServices.CI(tbErrorItem.Rows[i]["QTY"].ToString());

                                objErrorHandler.CreateLog("Remove continue inside order clarification" + PrdId);

                                objOrderServices.Remove_Clarification_item(CalItem_ID);
                                //cA.AddToCart(pQty, PrdId);

                                OrderDetails frmord = new OrderDetails();
                                frmord.NewProduct_id = PrdId;
                                frmord.NewQty = pQty;

                                frmord.AddOrderItem(OrderId, objHelperServices.CI(Session["USER_ID"]));
                            }
                            catch (Exception ex)
                            {

                                objErrorHandler.CreateLog(ex.ToString());
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString());
            }



            Response.Redirect("/OrderDetails.aspx?ORDER_ID=" + Convert.ToInt32(Session["ORDER_ID"]) + "&bulkorder=1&ViewOrder=View",false);
        }
        }
        catch (System.Threading.ThreadAbortException)
        {
            // ignore it
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

            this.modalPop.Hide();
            PopupRetailerLoginMsg.Visible = false;
            Session["RetailerMsg"] = "true";
            CallOrderMsg();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected void btnClearOrder_Click(object sender, EventArgs e)
    {
        try
        {
        this.modalPop.Hide();
        PopupOrderMsg.Visible = false;
      if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
        {
            OrderServices objOrderServices = new OrderServices();
            objOrderServices.RemoveItem("AllProd", Convert.ToInt32(Session["PrevOrderID"]), Convert.ToInt32(Session["USER_ID"]),"");
            objOrderServices.RemoveOrder(Convert.ToInt32(Session["PrevOrderID"]), Convert.ToInt32(Session["USER_ID"]));

            Session["PrevOrderID"] = "0";
            Session["ORDER_ID"] = "0";
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
     
    }


    [System.Web.Services.WebMethod]
    public static string ST_Newproduct(string newprod)
    {

        try
        {
            ConnectionDB objConnectionDB = new ConnectionDB();
            HelperServices objHelperServices = new HelperServices();

            //TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCT",HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCT", HttpContext.Current.Server.MapPath("~\\Templates"), "");
            //tbwtEngine.RenderHTML("Column");
            //return (tbwtEngine.RenderedHTML);
            // objErrorHandler.CreateLog("ST_RecentProduct"); 
            //return tbwtEngine.ST_NewProduct_Load();
            return tbwtEngine.ST_HOME_PRODUCT();
        }
        catch (Exception e)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }

    #region "Top"

    //public string ST_top()
    //{
    //    try
    //    {
    //        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("TOP", Server.MapPath("~\\Templates"), objConnectionDB.ConnectionString);
    //        tbwtEngine.cartitem = cartcount();
    //        tbwtEngine.RenderHTML("Row");
    //        return (tbwtEngine.RenderedHTML);
    //    }


    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //        return string.Empty;
    //    }
    //}

    //public string cartcount()
    //{
    //    try
    //    {
    //        HelperServices objHelperServices = new HelperServices();
    //        ErrorHandler oErr = new ErrorHandler();
    //        OrderServices objOrderServices = new OrderServices();
    //        string cartitem = "0";
    //        int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
    //        if (Session["USER_ID"] != null && !Session["USER_ID"].ToString().Equals(""))
    //        {

    //            int OrderID = 0;

    //            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].Equals("View")))
    //            {
    //                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
    //            }
    //            else
    //            {
    //                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);
    //            }

    //            string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
    //            if (OrderID > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())    // || OrderStatus=="CAU_PENDING")
    //            {
    //                if (objOrderServices.GetOrderItemCount(OrderID) == 0)
    //                    cartitem = "0";
    //                else
    //                    cartitem = objOrderServices.GetOrderItemCount(OrderID).ToString();

    //            }
    //            else
    //            {
    //                cartitem = "0";
    //            }
    //        }
    //        return cartitem;
    //    }


    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //        return string.Empty;
    //    }
    //}

    protected void logoutsession(object sender, EventArgs e)
    {
        objUserServices.OnLineFlag(false, objHelperServices.CI(Session["USER_ID"]));
        Session.RemoveAll();
        Session.Clear();
        Session.Abandon();
        Session["USER_ID"] = "";
        HttpCookie LoginInfoCookie = Request.Cookies["BigtopLoginInfo"];
        if (LoginInfoCookie != null && LoginInfoCookie["Password"] != null)
            LoginInfoCookie["Password"] = "";

        Response.Cookies["BigtopLoginInfo"].Expires = DateTime.Now.AddDays(-665);
        Response.Redirect("/Login.aspx");
    }
    #endregion


    #region "TopLog"
    //public string ST_toplog()
    //{
    //    try
    //    {
    //        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("TOPLOG", Server.MapPath("~\\Templates"), objConnectionDB.ConnectionString);
    //        tbwtEngine.cartitem = cartcountlog();
    //        tbwtEngine.RenderHTML("Row");
    //        return (tbwtEngine.RenderedHTML);
    //    }


    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //        return string.Empty;
    //    }
    //}

    //public string cartcountlog()
    //{

    //    try
    //    {
    //        HelperDB objHelperDB = new HelperDB();
    //        ErrorHandler objErrorHandler = new ErrorHandler();
    //        OrderServices objOrderServices = new OrderServices();
    //        string cartitem = "0";
    //        int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
    //        if (Session["USER_ID"] != null && !Session["USER_ID"].ToString().Equals("") && !Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
    //        {
    //            int OrderID = 0;

    //            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].Equals("View")))
    //            {
    //                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
    //            }
    //            else
    //            {
    //                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);
    //            }

    //            string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
    //            if (OrderID > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())        //  || OrderStatus == "CAU_PENDING")
    //            {
    //                if (objOrderServices.GetOrderItemCount(OrderID) == 0)
    //                    cartitem = "0";
    //                else
    //                    cartitem = objOrderServices.GetOrderItemCount(OrderID).ToString();

    //            }
    //            else
    //            {
    //                cartitem = "0";
    //            }
    //        }
    //        else
    //        {
    //            int OrderID = 0;
    //            String sessionId;
    //            sessionId = Session.SessionID;
    //            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].Equals("View")))
    //            {
    //                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
    //            }
    //            else
    //            {
    //                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(ConfigurationManager.AppSettings["DUM_USER_ID"]), OpenOrdStatusID);
    //            }
    //            string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
    //            if (OrderID > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())        //  || OrderStatus == "CAU_PENDING")
    //            {
    //                if (objOrderServices.GetOrderItemCount_BL(OrderID, sessionId) == 0)
    //                    cartitem = "0";
    //                else
    //                    cartitem = objOrderServices.GetOrderItemCount_BL(OrderID, sessionId).ToString();

    //            }

    //        }
    //        return cartitem;
    //    }


    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //        return string.Empty;
    //    }
    //}
    #endregion

    //#region "Newproduct"
    //public string ST_Newproduct()
    //{

    //    try
    //    {
    //        ConnectionDB objConnectionDB = new ConnectionDB();
    //        HelperServices objHelperServices = new HelperServices();

    //        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCT", Server.MapPath("~\\Templates"), objConnectionDB.ConnectionString);
    //        tbwtEngine.RenderHTML("Column");
    //        return (tbwtEngine.RenderedHTML);
    //    }
    //    catch (Exception e)
    //    {
    //        objErrorHandler.ErrorMsg = e;
    //        objErrorHandler.CreateLog();
    //        return string.Empty;
    //    }
    //}
    //#endregion
  
    //#region "Category"

    //public string ST_Category()
    //{
    //    HelperServices objHelperServices = new HelperServices();
    //    return (Category_RenderHTML("CATEGORY", Server.MapPath("~\\Templates")));
    //}


    //public string Category_RenderHTML(string Package, string SkinRootPath)
    //{
    //    try
    //    {
    //        string skin_container = null;

    //        int grid_cols = 0;
    //        int grid_rows = 0;
    //        string skin_sql_container = null;
    //        string skin_sql_param_container = null;
    //        string skin_records = null;
    //        TBWDataList[] lstrecords = new TBWDataList[0];
    //        StringTemplateGroup stg_records = null;
    //        StringTemplate bodyST = null;
    //        StringTemplate bodyST_categorylist = null;
    //        StringTemplate bodyST_categorylistnew = null;
    //        StringTemplate bodyST_head = null;
    //        StringTemplate bodyST_list1 = null;
    //        string firstval = null;
    //        //List<string> name = new List<string>();
    //        StringBuilder name = new StringBuilder();
    //        System.Text.StringBuilder ct = new StringBuilder();
    //        System.Text.StringBuilder categoryrowlist = new StringBuilder();
    //        System.Text.StringBuilder categorylistnew = new StringBuilder();
    //        int indV = 0;
    //        int bodyValue = 0;
    //        EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    //        DataSet dspkg = new DataSet();
    //        string _tsb = string.Empty; ;
    //        string _tsm = string.Empty;
    //        string _searchstr = string.Empty;
    //       // string _byp = "2";
    //       // string _bypcat = null;
    //        //string _pid = "";
    //       // string _fid = "";
    //        //string _Ea_path = "";



    //        if (Request.QueryString["tsm"] != null)
    //            _tsm = Request.QueryString["tsm"];

    //        if (Request.QueryString["tsb"] != null)
    //            _tsb = Request.QueryString["tsb"];

    //        if (Request.QueryString["searchstr"] != null)
    //            _searchstr = Request.QueryString["searchstr"];
    //        if (Request.QueryString["srctext"] != null)
    //            _searchstr = Request.QueryString["srctext"];



    //        // old Code  by Jtech
    //        //string sqlpkginfo = " SELECT * FROM TBW_PACKAGE ";
    //        //sqlpkginfo = sqlpkginfo + " WHERE PACKAGE_NAME = '" + Package + "'";

    //        //dspkg = GetDataSet(sqlpkginfo);
    //        // old Code commant by Jtech
    //        dspkg = (DataSet)objHelperDB.GetGenericDataDB(Package, "GET_PACKAGE_WITHOUT_ISROOT", HelperDB.ReturnType.RTDataSet);

    //        if (dspkg != null)
    //        {
    //            if (dspkg.Tables[0].Rows.Count > 0)
    //            {
    //                foreach (DataRow dr in dspkg.Tables[0].Rows)
    //                {
    //                    skin_container = dr["SKIN_NAME"].ToString();
    //                    grid_cols = Convert.ToInt32(dr["GRID_COLS"]);
    //                    grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
    //                    skin_sql_container = dr["SKIN_SQL"].ToString();
    //                    skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
    //                    skin_records = dr["SKIN_NAME"].ToString();
    //                }
    //            }
    //        }

    //        //Build the inner body of the HTML
    //        stg_records = new StringTemplateGroup(skin_records, SkinRootPath + "\\" + skin_records);
    //        //DataSet dsrecords = GetDataSet(skin_sql_container);
    //        DataSet dsrecords = EasyAsk.GetCategoryAndBrand("SubCategory");
    //        if (dsrecords != null && dsrecords.Tables[0].Rows.Count != 0)
    //        {
    //            lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count * 2];
    //            int catno = 0;
    //            if (dsrecords.Tables[0].Rows[0]["TBT_PARENT_CATEGORY_NAME"].ToString() != null && dsrecords.Tables[0].Rows[0]["TBT_PARENT_CATEGORY_NAME"].ToString() != string.Empty)
    //            {
    //                //Build the Sub heading 
    //                firstval = dsrecords.Tables[0].Rows[0]["TBT_PARENT_CATEGORY_NAME"].ToString().ToUpper();

    //                if (firstval.ToUpper().ToString() != "WESNEWS" && firstval != "")
    //                    bodyST_categorylist = stg_records.GetInstanceOf("cell");
    //                // objHelperServices.Cons_NewURl(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + firstval, "ct.aspx", true, "");
    //                DataRow ddr = dsrecords.Tables[0].Rows[0];

    //                foreach (DataColumn dc in dsrecords.Tables[0].Columns)
    //                {

    //                    if (dc.ColumnName.ToString().Contains("CUSTOM_NUM"))
    //                        if (ddr[dc.ColumnName].ToString().Length > 0)
    //                            bodyST_categorylist.SetAttribute(dc.ColumnName, Convert.ToDouble(ddr[dc.ColumnName].ToString()));
    //                        else
    //                            bodyST_categorylist.SetAttribute(dc.ColumnName, "0");

    //                    else
    //                    {
    //                        if ("TBT_PARENT_CATEGORY_IMAGE" == dc.ColumnName)
    //                        {
    //                            bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString().Replace("\\", "/"));
    //                            //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + ddr[dc.ColumnName].ToString().Replace("/", "\\"));
    //                            //if (Fil.Exists)
    //                            //{
    //                            //    bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString().Replace("\\", "/"));
    //                            //}
    //                            //else
    //                            //{
    //                            //    bodyST_categorylist.SetAttribute(dc.ColumnName, "/images/noimage.gif");

    //                            //}
    //                            //bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString().Replace('\\', '/'));
    //                        }
    //                        else if ("EA_PATH" == dc.ColumnName)
    //                            bodyST_categorylist.SetAttribute(dc.ColumnName, HttpUtility.UrlEncode(objSecurity.StringEnCrypt(ddr[dc.ColumnName].ToString())));
    //                        else
    //                        {
    //                            if (ddr[dc.ColumnName].ToString() != "")
    //                                bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString());
    //                        }
    //                        // objHelperServices.Cons_NewURl(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + ddr[dc.ColumnName].ToString(), "ct.aspx", true, ""); 
    //                    }
    //                }
    //                if (firstval != "")
    //                    objHelperServices.SimpleURL(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + firstval, "ct.aspx");
    //            }

    //            //if (i == 0)
    //            //{
    //            //    objHelperServices.Cons_NewURl(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + firstval, "ct.aspx", true, "");
    //            //}
    //            int i = 1;
    //            foreach (DataRow dr in dsrecords.Tables[0].Rows)
    //            {

    //                if (dr["TBT_PARENT_CATEGORY_NAME"].ToString() != null && dr["TBT_PARENT_CATEGORY_NAME"].ToString() != string.Empty && dr["TBT_PARENT_CATEGORY_NAME"].ToString().ToLower() != "wesnews" && dr["TBT_PARENT_CATEGORY_ID"].ToString().ToLower() != "wesnews")
    //                {


    //                    if (firstval != dr["TBT_PARENT_CATEGORY_NAME"].ToString().ToUpper() && firstval != "")
    //                    {

    //                        //Build the category 
    //                        bodyST_categorylist.SetAttribute("TBT_CATEGORY_ORDER", i);
    //                        i++;
    //                        bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", ct.ToString());
    //                        bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST1", categorylistnew.ToString());

    //                        name.Append(bodyST_categorylist.ToString());
    //                        indV++; catno = 0;
    //                        if (indV == grid_cols)
    //                        {
    //                            bodyST = stg_records.GetInstanceOf("row");
    //                            bodyST.SetAttribute("TBWDataList", name);

    //                            lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
    //                            bodyValue++;
    //                            indV = 0;
    //                            name = new StringBuilder();
    //                        }

    //                        if (firstval.ToUpper().ToString() != "WESNEWS")
    //                        {
    //                            //Build the sub heading
    //                            ct = new StringBuilder();
    //                            bodyST_categorylist = stg_records.GetInstanceOf("cell");

    //                            categorylistnew = new StringBuilder();
    //                            bodyST_categorylistnew = stg_records.GetInstanceOf("cell");
    //                        }

    //                        // objHelperServices.Cons_NewURl(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + dr["TBT_PARENT_CATEGORY_NAME"].ToString(), "ct.aspx",, "");
    //                        foreach (DataColumn dc in dsrecords.Tables[0].Columns)
    //                        {
    //                            if (dc.ColumnName.ToString().Contains("CUSTOM_NUM"))
    //                                if (dr[dc.ColumnName].ToString().Length > 0)
    //                                    bodyST_categorylist.SetAttribute(dc.ColumnName, Convert.ToDouble(dr[dc.ColumnName].ToString()));
    //                                else
    //                                    bodyST_categorylist.SetAttribute(dc.ColumnName, "0");
    //                            else
    //                            {
    //                                if ("TBT_PARENT_CATEGORY_IMAGE" == dc.ColumnName)
    //                                {

    //                                    bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString().Replace("\\", "/"));
    //                                    //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr[dc.ColumnName].ToString().Replace("/", "\\"));
    //                                    //if (Fil.Exists)
    //                                    //{
    //                                    //    bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString().Replace("\\", "/"));
    //                                    //}
    //                                    //else
    //                                    //{
    //                                    //    bodyST_categorylist.SetAttribute(dc.ColumnName, "/images/noimage.gif");

    //                                    //}

    //                                    // bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString().Replace('\\', '/'));
    //                                }
    //                                else if ("EA_PATH" == dc.ColumnName)
    //                                    bodyST_categorylist.SetAttribute(dc.ColumnName, HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr[dc.ColumnName].ToString())));
    //                                else
    //                                {
    //                                    if (dr[dc.ColumnName].ToString() != "")
    //                                        bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());
    //                                }
    //                            }

    //                        }

    //                        firstval = dr[1].ToString().ToUpper();

    //                        if (firstval != "")
    //                            objHelperServices.SimpleURL(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + firstval, "ct.aspx");

    //                    }





    //                    //Build the Content
    //                    if (catno < 12 && firstval != "")
    //                    {
    //                        if (dr["TBT_SHORT_DESC"].ToString().ToLower() != "not for web" && dr["CATEGORY_ID"].ToString().ToLower() != "wesnews")
    //                        {
    //                            bodyST_list1 = stg_records.GetInstanceOf("cell1");
    //                            bodyST_list1.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());
    //                            bodyST_list1.SetAttribute("TBT_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
    //                            if (dr["TBT_CUSTOM_NUM_FIELD3"] != System.DBNull.Value)
    //                            {
    //                                bodyST_list1.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(dr["TBT_CUSTOM_NUM_FIELD3"]).ToString());
    //                            }
    //                            else
    //                            {
    //                                bodyST_list1.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "");
    //                            }
    //                            bodyST_list1.SetAttribute("TBT_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(dr["TBT_PARENT_CATEGORY_ID"].ToString()));

    //                            bodyST_list1.SetAttribute("TBT_BRAND", HttpUtility.UrlEncode(_tsb));
    //                            bodyST_list1.SetAttribute("TBT_MODEL", HttpUtility.UrlEncode(_tsm));
    //                            bodyST_list1.SetAttribute("TBT_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(_searchstr));
    //                            bodyST_list1.SetAttribute("TBT_ATTRIBUTE_TYPE", "Category");
    //                            bodyST_list1.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["CATEGORY_NAME"].ToString()));
    //                            bodyST_list1.SetAttribute("TBT_ATTRIBUTE_BRAND", _tsb);

    //                            bodyST_list1.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["EA_PATH"].ToString() + "////" + dr["TBT_PARENT_CATEGORY_NAME"].ToString())));
    //                            objHelperServices.SimpleURL(bodyST_list1, "AllProducts////WESAUSTRALASIA////" + "////" + dr["EA_PATH"].ToString() + "////" + dr["TBT_PARENT_CATEGORY_NAME"].ToString() + "////" + bodyST_list1.GetAttribute("TBT_ATTRIBUTE_VALUE"), "pl.aspx");

    //                            if (catno < 6)
    //                            {
    //                                ct.Append(bodyST_list1.ToString()); catno++;
    //                            }
    //                            else
    //                            {
    //                                categorylistnew.Append(bodyST_list1.ToString()); catno++;
    //                            }
    //                        }
    //                    }



    //                }
    //            }
    //        }
    //        if (dsrecords == null || dsrecords.Tables[0].Rows.Count == 0)
    //        {
    //            bodyST_categorylist = stg_records.GetInstanceOf("cell");
    //        }
    //        if (indV < grid_cols)
    //        {

    //            bodyST_categorylist.SetAttribute("TBT_CATEGORY_ORDER", "21");
    //            bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", ct.ToString());
    //            bodyST_categorylistnew.SetAttribute("TBT_SUB_CATEGORY_LIST1", categorylistnew.ToString());
    //            name.Append(bodyST_categorylist.ToString());
    //            // name.Append(bodyST_categorylistnew.ToString());
    //            bodyST = stg_records.GetInstanceOf("row");
    //            bodyST.SetAttribute("TBWDataList", name);
    //            if (lstrecords.Length != 0)
    //            {
    //                lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
    //            }
    //            bodyValue++;
    //        }
    //        StringTemplate bodyST_main = stg_records.GetInstanceOf("main");
    //        bodyST_main.SetAttribute("TBWDataList", lstrecords);
    //        string sHtmls = bodyST_main.ToString();
    //        if (sHtmls.Contains("src=\"/prodimages\""))
    //            sHtmls = sHtmls.Replace("src=\"/prodimages\"", "src=\"/images/noimage.gif\"");
    //        if (sHtmls.Contains("src=\"\""))
    //            sHtmls = sHtmls.Replace("src=\"\"", "src=\"/images/noimage.gif\"");

    //        return objHelperServices.StripWhitespace(sHtmls);
    //    }
    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //        return string.Empty;
    //    }

    //}

    //#endregion

    //#region "bottom"
    //public string ST_bottom()
    //{
    //    try
    //    {
    //        HelperServices objHelperServices = new HelperServices();
    //        ConnectionDB objConnectionDB = new ConnectionDB();
    //        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BOTTOM", Server.MapPath("~\\Templates"), objConnectionDB.ConnectionString);
    //        tbwtEngine.RenderHTML("Row");
    //        return (tbwtEngine.RenderedHTML);
    //    }
    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //        return string.Empty;
    //    }
    //}
    // #endregion


}
