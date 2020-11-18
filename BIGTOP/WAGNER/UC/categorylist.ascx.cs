using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk ;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Reflection;
using System.Linq;
//using System.Diagnostics;
public partial class UC_categorylist : System.Web.UI.UserControl
{
    string stemplatepath = string.Empty;
    ErrorHandler objErrorHandler=new ErrorHandler();
    ConnectionDB objConnectionDB = new ConnectionDB();
    Security objSecurity = new Security();
    UserServices objUserServices = new UserServices();
    ProductServices objProductServices = new ProductServices();
    string _catId = string.Empty;
    string _catName = string.Empty;
    string _catalogid = string.Empty;
  
    CategoryServices objCategoryServices = new CategoryServices();
    int iRecordsPerPage = 18;
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();

    string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
    public string WAG_Root_Path = System.Configuration.ConfigurationManager.AppSettings["EA_ROOT_CATEGORY_PATH"].ToString();
    //Stopwatch stopwatch = new Stopwatch(); 

   // int iCatalogId;
   // int iInventoryLevelCheck;
   // bool bIsStartOver = true;
   // string sSortBy = "";
   // bool bDoPaging;
    int iPageNo = 1;
   // bool bSortAsc = true;
    int iTotalPages = 0;
    int iTotalProducts = 0;
   // int iTmpProductId = 0;
   // int iPrevPgNo = 1;
    //int iNextPgNo = 1;


    string _tsb = string.Empty;
    string _tsm = string.Empty;
    string _type = string.Empty;
    string _value = string.Empty;
    string _bname = string.Empty;
    string _searchstr = string.Empty;
    string _byp = "2";
   // string _bypcat = null;
   // string _pid = "";
   // string _fid = "";
    string _cid = string.Empty;
    string _EAPath = string.Empty;
    string _ParentCatID = string.Empty;
    string _pcr = string.Empty;
    public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    string strprodimg_sepdomain = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
      
        if ((Request.RawUrl.ToLower().Contains("_")))
        {
            CT_PageLoad();

        }
        else 
        {
            CT_PageLoad_New();
        }
       
    }
    private void CT_PageLoad()
    {
        stemplatepath = Server.MapPath("Templates");
        // Page.Title = oHelper.GetOptionValues("BROWSER TITLE").ToString();
        _catalogid = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
        GetPageConfig();
        if (IsPostBack)
        {

        }
        else
        {
            if (Request.QueryString["pgno"] != null)
            {
                iPageNo = Convert.ToInt32(Request.QueryString["pgno"]);
            }
            HiddenField1.Value = "0";
            HiddenField2.Value = "0";
            hforgurl.Value = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            hfnewurl.Value = Request.RawUrl.ToString();
            hfcheckload.Value = "0";
            HFcnt.Value = "1";
            hfback.Value = "";
            hfbackdata.Value = "";
        }
       
    }
    private void CT_PageLoad_New()
    {
        stemplatepath = Server.MapPath("Templates");
        // Page.Title = oHelper.GetOptionValues("BROWSER TITLE").ToString();
        _catalogid = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
        GetPageConfig();
        if (IsPostBack)
        {

        }
        else
        {
            if (Request.QueryString["pgno"] != null)
            {
                iPageNo = Convert.ToInt32(Request.QueryString["pgno"]);
            }
            HiddenField1.Value = "0";
            HiddenField2.Value = "0";
            hforgurl.Value = HttpContext.Current.Request.Url.PathAndQuery.ToString();
          //  string StrRawurl = Request.RawUrl.ToString();
           // string NewRawUrl = objHelperServices.URlStringReverse(StrRawurl);

           // hfnewurl.Value = NewRawUrl;
                //.Replace("ct/", "/ct.aspx?");
            hfcheckload.Value = "0";
            HFcnt.Value = "1";
            hfback.Value = "";
            hfbackdata.Value = "";
        }
       
    }
    private void GetPageConfig()
    {
        try
        {
            //if (Session["PS_IS_START_OVER"].ToString() == "YES")
            //{
            //    bIsStartOver = true;
            //}
            //else
            //{
            //    bIsStartOver = false;
            //}
        
            if (HidItemPage.Value.ToString() == string.Empty || HidItemPage.Value.ToString() == null)
            {
                if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());
            }
            else
            {
                iRecordsPerPage = Convert.ToInt32(HidItemPage.Value.ToString());
                Session["RECORDS_PER_PAGE_CATEGORY_LIST"] = HidItemPage.Value.ToString();
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected string ST_CategoryList()
    {

        return (Category_RenderHTML("CATEGORY", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString())));
    }
    protected string ST_CategoryProductList()
    {
        return (CategoryProductList_RenderHTMLJson("CATEGORYPRODUCTLIST", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString())));
       // TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CATEGORYPRODUCTLIST", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        

    }
    protected string ST_newproductsnav()
    {
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();

      //TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCTHIGHLIGHTSCATLIST", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);

      //tbwtEngine.RenderHTML("Column");

       // return (tbwtEngine.RenderedHTML);
        //return (newproductsnav_RenderHTML("NEWPRODUCTHIGHLIGHTSCATLIST", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString())));
        return "";
    }
    private void SubCategoryDisplay( StringTemplate bodyST_subcategorylist, DataRow drow)
    {
        //bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_NAME", drow["CATEGORY_NAME"].ToString());
        //bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_ID", drow["CATEGORY_ID"].ToString());
        //if (drow["CUSTOM_NUM_FIELD3"] != null && drow["CUSTOM_NUM_FIELD3"].ToString() != "")
        //{
        //    if (Convert.ToInt32(drow["CUSTOM_NUM_FIELD3"]) == 2)
        //    {
        //        //bodyST_subcategorylist.SetAttribute("TBT_URL", "byproduct.aspx");
        //        bodyST_subcategorylist.SetAttribute("TBT_URL", "pl.aspx");
        //        bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(drow["CUSTOM_NUM_FIELD3"]).ToString() + "&pcr=" + _catId);
        //    }
        //    else
        //    {
        //        bodyST_subcategorylist.SetAttribute("TBT_URL", "pl.aspx");
        //        bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(drow["CUSTOM_NUM_FIELD3"]).ToString());

        //    }

        //}
        //else
        //{
        //    bodyST_subcategorylist.SetAttribute("TBT_URL", "pl.aspx");
        //    bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "1");
        //}
        //bodyST_subcategorylist.SetAttribute("divid", drow["CATEGORY_ID"].ToString());

        
        if (WesNewsCategoryId == _catId)
            bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_WITH_CAT_ID", drow["CATEGORY_ID"].ToString());



        bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_NAME", drow["CATEGORY_NAME"].ToString());
        bodyST_subcategorylist.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
        bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_ID", HttpUtility.UrlEncode(drow["CATEGORY_ID"].ToString()));
        if (drow["CUSTOM_NUM_FIELD3"] != System.DBNull.Value)
        {
            bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(drow["CUSTOM_NUM_FIELD3"]).ToString());
        }
        else
        {
            bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "");
        }
        bodyST_subcategorylist.SetAttribute("TBT_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(drow["PARENT_CATEGORY_ID"].ToString()));

        bodyST_subcategorylist.SetAttribute("TBT_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(""));
        bodyST_subcategorylist.SetAttribute("TBT_ATTRIBUTE_TYPE", "Category");
        bodyST_subcategorylist.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(drow["CATEGORY_NAME"].ToString()));
        bodyST_subcategorylist.SetAttribute("TBT_ATTRIBUTE_BRAND", "");

        bodyST_subcategorylist.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(drow["EA_PATH"].ToString())));
        bodyST_subcategorylist.SetAttribute("divid", drow["CATEGORY_ID"].ToString());

             
    }
    private void MainCategoryDisplay(StringTemplate bodyST_categorylist, DataRow _dsrow)
    {

        
        if (WesNewsCategoryId == _catId)
            bodyST_categorylist.SetAttribute("TBT_CATEGORY_WITH_CAT_ID", _dsrow["CATEGORY_ID"].ToString());
        
            bodyST_categorylist.SetAttribute("TBT_CATEGORY_NAME", _dsrow["CATEGORY_NAME"].ToString());

        bodyST_categorylist.SetAttribute("TBT_CATEGORY_ID", HttpUtility.UrlEncode(_dsrow["CATEGORY_ID"].ToString()));
        if (_dsrow["CUSTOM_NUM_FIELD3"] != System.DBNull.Value)
        {
            bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(_dsrow["CUSTOM_NUM_FIELD3"]).ToString());
        }
        else
        {
            bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "");
        }
        bodyST_categorylist.SetAttribute("TBT_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_dsrow["PARENT_CATEGORY_ID"].ToString()));

        bodyST_categorylist.SetAttribute("TBT_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(""));
        bodyST_categorylist.SetAttribute("TBT_ATTRIBUTE_TYPE", "Category");
        bodyST_categorylist.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(_dsrow["CATEGORY_NAME"].ToString()));
        bodyST_categorylist.SetAttribute("TBT_ATTRIBUTE_BRAND", "");

        bodyST_categorylist.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_dsrow["EA_PATH"].ToString())));
        bodyST_categorylist.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
        

        //bodyST_categorylist.SetAttribute("TBT_CATEGORY_NAME", _dsrow["CATEGORY_NAME"].ToString());
        //bodyST_categorylist.SetAttribute("TBT_CATEGORY_ID", _dsrow["CATEGORY_ID"].ToString());





        //FileInfo Fil = new FileInfo(strFile + _dsrow["IMAGE_FILE"].ToString());
        //if (Fil.Exists)
        //{
        //    bodyST_categorylist.SetAttribute("TBT_IMAGE_FILE", _dsrow["IMAGE_FILE"].ToString().Replace("\\", "/"));
        //}
        //else
        //{
        //    bodyST_categorylist.SetAttribute("TBT_IMAGE_FILE", "");
        //}

        //if (_dsrow["CUSTOM_NUM_FIELD3"] != null && _dsrow["CUSTOM_NUM_FIELD3"].ToString() != "")
        //{
        //    if (Convert.ToInt32(_dsrow["CUSTOM_NUM_FIELD3"]) == 2)
        //    {
        //        //bodyST_categorylist.SetAttribute("TBT_URL", "byproduct.aspx");
        //        bodyST_categorylist.SetAttribute("TBT_URL", "pl.aspx");
        //        bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(_dsrow["CUSTOM_NUM_FIELD3"]).ToString() + "&pcr=" + _catId);
        //    }
        //    else
        //    {
        //        bodyST_categorylist.SetAttribute("TBT_URL", "pl.aspx");
        //        bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(_dsrow["CUSTOM_NUM_FIELD3"]).ToString());
        //    }

        //}
        //else
        //{
        //    bodyST_categorylist.SetAttribute("TBT_URL", "pl.aspx");
        //    bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "1");
        //}
    }
//    public string CategoryProductList_RenderHTML(string package, string SkinRootPath)
//    {
//        //if (Session["PS_SEARCH_RESULTS"] == null)
//        //{
//        //    return "";
//        //}
//        FamilyServices objFamilyServices = new FamilyServices();
//        StringTemplateGroup _stg_container = null;
//        StringTemplateGroup _stg_records = null;
//        StringTemplate _stmpl_container = null;
//        StringTemplate _stmpl_records = null;
//        StringTemplate _stmpl_pages = null;
//        DataSet dsprod = new DataSet();
//        DataSet dsprodspecs = new DataSet();
//        DataSet dscat = new DataSet();
//        string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();

//        int oe = 0;
//        string category_nameh = string.Empty;
//        string sHTML = string.Empty;
//        string _pcr = string.Empty;
//        string _ViewType = string.Empty;
//        string _BCEAPath = string.Empty;
//    //string catID = "";
   
   

//        //if (Convert.ToInt32(Session["PS_SEARCH_RESULTS"].ToString()) > 0)
//        //{
//        try
//        {

//            if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
//                _tsm = Request.QueryString["tsm"];

//            if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
//                _tsb = Request.QueryString["tsb"];

//            if (Request.QueryString["type"] != null)
//                _type = Request.QueryString["type"];

//            if (Request.QueryString["value"] != null)
//                _value = Request.QueryString["value"];

//            if (Request.QueryString["bname"] != null)
//                _bname = Request.QueryString["bname"];
//            if (Request.QueryString["searchstr"] != null)
//                _searchstr = Request.QueryString["searchstr"];
//            if (Request.QueryString["srctext"] != null)
//                _searchstr = Request.QueryString["srctext"];

//            if (Request.QueryString["cid"] != null)
//                _cid = Request.QueryString["cid"];

//            if (Request.QueryString["pcr"] != null)
//                _pcr = Request.QueryString["pcr"];


//            if (Request.QueryString["ViewMode"] != null)
//            {
//                _ViewType = Request.QueryString["ViewMode"];
//               Session["PL_VIEW_MODE"] = _ViewType;
//            }
//            else if (Session["PL_VIEW_MODE"] != null && Session["PL_VIEW_MODE"].ToString() != "")
//                _ViewType = Session["PL_VIEW_MODE"].ToString();
//            else
//                _ViewType = "GV";

//            //if (Request.QueryString["path"] != null)
//            //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(Request.QueryString["path"].ToString()));

//            if (HttpContext.Current.Session["EA"] != null)
//                _EAPath = HttpContext.Current.Session["EA"].ToString();

//               if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
//            {
//                _BCEAPath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();
//                _BCEAPath = _BCEAPath.ToString().Replace("Category=", "").Replace("CATEGORY=", "");
//               }

//            stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());


                         
//                dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
        


//            if (dscat != null)
//            {

//                if (Request.QueryString["pcr"] != null)
//                    _pcr = Request.QueryString["pcr"];
//                if (Request.QueryString["type"] == null || Request.QueryString["type"] == "")
//                {
//                    //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
//                    string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
//                    if (tempstr != null && tempstr != "")
//                        category_nameh = tempstr;
//                }
//                else
//                    category_nameh = Request.QueryString["value"].ToString();

//                DataRow drpagect = dscat.Tables[0].Rows[0];
//                iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);

//                if (iPageNo > iTotalPages)
//                {
//                    iPageNo = iTotalPages;
//                    //ps.PAGE_NO = iPageNo;
//                }
//                Session["iTotalPages"] = iTotalPages;
//                DataRow drproductsct = dscat.Tables[1].Rows[0];
//                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
//                if (_cid != "")
//                    _ParentCatID = GetParentCatID(_cid);

//                if ((iTotalPages <= 0 || iTotalProducts <= 0) && WesNewsCategoryId == _ParentCatID)
//                {


//                    _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);
//                    _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "advmain");

//                   // SetEbookAndPDFLink(_stmpl_container);
//                    DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
//                    if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
//                        _stmpl_container.SetAttribute("TBT_ADV_URL_LINK", tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString());

//                    _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);
//                    return _stmpl_container.ToString();
//                }



//                TBWDataList[] lstrecords = new TBWDataList[0];
//                TBWDataList[] lstrows = new TBWDataList[0];
//                _stg_records = new StringTemplateGroup("CategoryProductList", stemplatepath);


//                int ictrecords = 0;
//                int icolstart = 0;
//                string trmpstr = string.Empty;
//                int icol = 1;
//                lstrows = new TBWDataList[icol];

//                if (_ViewType == "GV")
//                {
//                    icol = 4;
//                    lstrows = new TBWDataList[icol];
//                }
//                else
//                {
//                    icol = 1;
//                    lstrows = new TBWDataList[icol];
//                }

//                //if (dscat.Tables[0].Rows.Count < icol)
//                //{
//                //    icol = dscat.Tables[0].Rows.Count;
//                //}
//                string soddevenrow = "odd";

//                DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1");


//                string userid = string.Empty;
//                string PriceTable = string.Empty;
//                int pricecode = 0;
//                string tmpProds = string.Empty;
//                DataSet dsBgDisc = new DataSet();
//                DataSet dsPriceTableAll = new DataSet();
//                if (Session["USER_ID"] != null)
//                    userid = Session["USER_ID"].ToString();
//                if (userid == "")                   
//                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

//                string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
//                if (Convert.ToInt32(userid) > 0)
//                {

//                    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
//                }
//                else
//                {
//                    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
//                }
//                pricecode = objHelperDB.GetPriceCode(userid);

//                if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
//                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());
        
//                tmpProds = "";
//                if (Convert.ToInt32(userid) > 0)
//                {
//                    foreach (DataRow drpid in drprodcoll)
//                    {
//                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
//                        tmpProds = tmpProds.Replace(",,", ","); 
//                    }
//                    if (tmpProds != "")
//                    {
//                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
//                        dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
//                    }
//                }

//                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
//                lstrecords = new TBWDataList[drprodcoll.Length + 1];
//               // objErrorHandler.CreateLog(drprodcoll.Length.ToString()); 
//                foreach (DataRow drpid in drprodcoll)
//                {
//                    oe++;
//                    if ((oe % 2) == 0)
//                    {
//                        soddevenrow = "even";
//                    }
//                    else
//                    {
//                        soddevenrow = "odd";
//                    }

//                    if (_ViewType == "GV")
//                        _stmpl_records = _stg_records.GetInstanceOf("CategoryProductList" + "\\" + "productlist_GridView");
//                    else
//                        _stmpl_records = _stg_records.GetInstanceOf("CategoryProductList" + "\\" + "productlist_WES" + soddevenrow);

//                   // string urlDesc = "";

//                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());
//                    _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"].ToString());
//                    _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"].ToString());

//                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

//                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));

//                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
//                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());
//                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
//                    //objErrorHandler.CreateLog(drpid["FAMILY_ID"].ToString() + "--" + trmpstr); 
//                    if (_ViewType == "GV")
//                    {

//                        if (trmpstr.Length > 60)
//                            trmpstr = trmpstr.Substring(0, 60) + "...";
//                    }

//                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);
//                    _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", trmpstr.Replace('"', ' '));
//                    _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
//                    _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());

//                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", objHelperServices.GetIsEcomEnabled(userid));
//                    //if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
//                    //    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
//                    ////  string ValueFortag = string.Empty;
//                    ////ValueFortag = "<div id=\"pid" + dscat.Tables[2].Rows[0]["PRODUCT_ID"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
//                    //else
//                    //    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);
               
//                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
//                    {
//                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
//                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
//                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());
//                   //modified by :indu
//                        string newurl = string.Empty;
//                        newurl = _BCEAPath + "////" +
//                                             drpid["FAMILY_ID"].ToString() + "=" + _stmpl_records.GetAttribute("FAMILY_NAME");
//                        //objErrorHandler.CreateLog(newurl); 
//                        objHelperServices.Cons_NewURl(_stmpl_records, newurl, "fl.aspx", true,"");
//                    }
//                    else
//                    {
//                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
//                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);



//                        if (dsBgDisc != null)
//                        {
//                            if (dsBgDisc.Tables[0].Rows.Count > 0)
//                            {
//                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
//                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
//                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
//                                //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
//                                bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
//                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
//                                {
//                                    //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

//                                }
//                            }
//                        }
//                        if (Convert.ToInt32(userid) > 0)
//                        {

//                            PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
//                        }
//                        _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);


//                        // _stmpl_records.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));

//                        if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
//                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
//                        else
//                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

//                        DataRow[] drow = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND ATTRIBUTE_TYPE=0");
//                        if (drow.Length > 0) // Data Rows must return 1 row 
//                        {
//                            DataTable td = drow.CopyToDataTable();
//                            _stmpl_records.SetAttribute("TBT_USER_PRICE", td.Rows[0]["NUMERIC_VALUE"].ToString());

//                            if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) != -1)
//                            {
//                                if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) > 0)
//                                {
//                                    _stmpl_records.SetAttribute("TBT_QTY_AVAIL", td.Rows[0]["QTY_AVAIL"].ToString());
//                                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", td.Rows[0]["MIN_ORD_QTY"].ToString());
//                                }
//                                else
//                                {
//                                    _stmpl_records.SetAttribute("TB_DISPLAY", "none");
//                                }
//                                //_stmpl_records.SetAttribute("TBT_PRODUCT_ID", td.Rows[0]["PRODUCT_ID"].ToString());
//                            }
//                        }



//                    }
//                    dsprodspecs = new DataSet();
//                    DataRow[] drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=1 OR ATTRIBUTE_TYPE=4 OR ATTRIBUTE_TYPE=3) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
//                    if (drow1.Length > 0)
//                        dsprodspecs.Tables.Add(drow1.CopyToDataTable());
//                    else
//                        dsprodspecs = null;



//                    if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
//                    {
//                        //Indu start
//                        //foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
//                        //{
//                        //    if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
//                        //    {
//                        //        if (dr["ATTRIBUTE_ID"].ToString() == "1")
//                        //            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
//                        //        else
//                        //            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
//                        //    }
//                        //    else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
//                        //    {
//                        //        if (dr["NUMERIC_VALUE"].ToString().Length > 0)
//                        //            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString());
//                        //    }
//                        //    else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
//                        //    {
//                        //        System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
//                        //        if (Fil.Exists)
//                        //            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
//                        //        else
//                        //        {
//                        //            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
//                        //        }

//                        //    }
//                        //    else
//                        //    {
//                        //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
//                        //    }
//                        //     if (_stmpl_records.GetAttribute("TBT_SUB_FAMILY").ToString() == "False")
//                        //    {
//                        //        string newurl = string.Empty;

//                        //        newurl = _BCEAPath + "////" + drpid["FAMILY_ID"].ToString() + "=" +
//                        //                     _stmpl_records.GetAttribute("FAMILY_NAME") + "////" +
//                        //                     drpid["PRODUCT_ID"].ToString()+"="+
//                        //                     dr["PRODUCT_CODE"].ToString();
//                        //        //Added by:Indu

//                        //        _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", dr["ATTRIBUTE_TYPE"].ToString());
//                        //        objHelperServices.Cons_NewURl(_stmpl_records, newurl, "pd.aspx", true, dr["ATTRIBUTE_TYPE"].ToString());
//                        //    }
//                        //}
////End
//                        //Indu Start
//                        int cnt = dsprodspecs.Tables[0].Rows.Count;
//                        for (int i=0;i < cnt;i++)
//                        {
//                            if (dsprodspecs.Tables[0].Rows[i] ["ATTRIBUTE_TYPE"].ToString() == "1")
//                            {
//                                if (dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString() == "1")
//                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString(), 16, true));
//                                else
//                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString());
//                            }
//                            else if (dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString() == "4")
//                            {
//                                if (dsprodspecs.Tables[0].Rows[i]["NUMERIC_VALUE"].ToString().Length > 0)
//                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), Math.Round(Convert.ToDecimal(dsprodspecs.Tables[0].Rows[i]["NUMERIC_VALUE"]), 2).ToString());
//                            }
//                            else if (dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString() == "3")
//                            {
//                                System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString().Replace("/", "\\"));
//                                if (Fil.Exists)
//                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString().Replace("\\", "/"));
//                                else
//                                {
//                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
//                                }

//                            }
//                            else
//                            {
//                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString());
//                            }
//                            if (_stmpl_records.GetAttribute("TBT_SUB_FAMILY").ToString() == "False")
//                            {
//                                string newurl = string.Empty;

//                                newurl = _BCEAPath + "////" +
//                                             drpid["FAMILY_ID"].ToString() + "=" + _stmpl_records.GetAttribute("FAMILY_NAME") + "////" +
//                                             drpid["PRODUCT_ID"].ToString() + "=" +
//                                             dsprodspecs.Tables[0].Rows[i]["PRODUCT_CODE"].ToString();
//                                //Added by:Indu
//                                //objErrorHandler.CreateLog(newurl); 
//                                _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString());
//                                objHelperServices.Cons_NewURl(_stmpl_records, newurl, "pd.aspx", true, dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString());
//                            }
//                        }

////End
//                    }
//                    dsprodspecs = new DataSet();
//                    drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=7 OR ATTRIBUTE_TYPE=9) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
//                    if (drow1.Length > 0)
//                        dsprodspecs.Tables.Add(drow1.CopyToDataTable());
//                    else
//                        dsprodspecs = null;

//                    if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
//                    {
//                        string desc = "";
//                        string descattr = "";
//                        //Indu Start
//                        int cnt=dsprodspecs.Tables[0].Rows.Count ;
//                        for (int i = 0; i < cnt;i++ )
//                        {

//                            if (dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString() == "9")
//                            {
//                                System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString());
//                                if (Fil.Exists)
//                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString().Replace("\\", "/"));
//                                else
//                                {
//                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
//                                }

//                            }
//                            else if (dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString() == "7")
//                            {
//                                //string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
//                                desc = dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
//                                descattr = descattr +" "+ desc;
//                                if (desc.Length > 230 && _ViewType == "LV")
//                                {
//                                    _stmpl_records.SetAttribute("TBT_MORE_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), true);
//                                    desc = desc.Substring(0, 230).ToString();
//                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), desc);
//                                }
//                                //else if (desc.Length > 30 && _ViewType == "GV")
//                                //{
//                                //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
//                                //    desc = desc.Substring(0, 30).ToString();
//                                //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
//                                //}
//                                else
//                                {
//                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), desc);
//                                    _stmpl_records.SetAttribute("TBT_MORE_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), false);
//                                }
//                            }
//                            else
//                            {

//                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>"));
//                            }

//                        }
//                        if (descattr.Length > 86 && _ViewType == "GV")
//                        {
//                            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
//                            descattr = descattr.Substring(0, 86).ToString();
//                            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));

                          
//                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
//                        }
//                        else
//                        {
                            

//                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
//                        }
//                        }



//                    lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());

                    
//                    icolstart++;
//                    if (icolstart >= icol || oe == drprodcoll.Length)
//                    {
//                        if (icolstart == icol || iTotalProducts < 24)
//                        {
//                            _stg_container = new StringTemplateGroup("CategoryProductListcontainer", stemplatepath);
//                            _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "producttable_" + soddevenrow);
//                            _stmpl_container.SetAttribute("TBWDataList", lstrows);
//                            lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                          
//                        }
//                        balrecords_cnt.Value = icolstart.ToString();
//                        currpage.Value = iPageNo.ToString();

//                        ictrecords++;
//                        icolstart = 0;
//                        lstrows = new TBWDataList[icol];

//                    }
//                }

//                //   }

//                _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);

//                if (Request.QueryString["ViewMode"] != null)
//                {
//                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
//                    isreadonly.SetValue(this.Request.QueryString, false, null);
//                    this.Request.QueryString.Remove("ViewMode");
//                }

//                string productlistURL = Request.QueryString.ToString();
//                string powersearchURListView = string.Empty;
//                string powersearchURLGridView = string.Empty;
//                powersearchURListView = productlistURL + "&ViewMode=LV";
//                powersearchURLGridView = productlistURL + "&ViewMode=GV";


//                _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "main");
//                _stmpl_container.SetAttribute("TBW_CATEGORY_ID", _cid);
//                _stmpl_container.SetAttribute("TBW_PRODUCT_COUNT", iTotalProducts);
//                _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);

//                _stmpl_container.SetAttribute("TBW_URL", powersearchURListView);
//                _stmpl_container.SetAttribute("TBW_URL1", powersearchURLGridView);

//                if (_ViewType == "LV")
//                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
//                else
//                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);


              

//                _stmpl_container.SetAttribute("SELECT_" + iRecordsPerPage, "SELECTED=\"SELECTED\" ");

//                if (iTotalPages > 1)
//                {
//                    if (iPageNo != iTotalPages)
//                    {
//                        _stmpl_container.SetAttribute("TBW_TO_PAGE", true);
//                    }
//                    else if (iPageNo == iTotalPages)
//                    {
//                        _stmpl_container.SetAttribute("TBW_TOTAL_PAGE", true);
//                    }
//                }
//                else
//                {
//                    _stmpl_container.SetAttribute("TBW_TO_PAGE", false);
//                }


//                //if (WesNewsCategoryId == _ParentCatID) // WES NEW ONLy
//                //    SetEbookAndPDFLink(_stmpl_container);
//                //else
//                //    _stmpl_container.SetAttribute("TBT_DISPLAY_LINK", "none");



//                if (iPageNo < iTotalPages)
//                {
//                    if (iPageNo > 1)
//                    {
//                        iPrevPgNo = iPageNo - 1;
//                    }
//                    else
//                    {
//                        iPrevPgNo = 1;
//                    }
//                    iNextPgNo = iPageNo + 1;
//                }
              
//                else
//                {
//                    iNextPgNo = 1;
//                    iPrevPgNo = 1;
//                    iPageNo = iTotalPages;
//                }
//                try
//                {

//                    _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
//                    if (iPageNo > 2 && (iTotalPages >= (iPageNo + 2)))
//                    {
                       

//                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo - 2);
//                        SetQueryString(_stmpl_pages);


//                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

//                        _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

//                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo - 1);
//                        SetQueryString(_stmpl_pages);
//                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

//                        _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

//                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo);
//                        SetQueryString(_stmpl_pages);
//                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));

//                        _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

//                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo + 1);
//                        SetQueryString(_stmpl_pages);
//                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

//                        _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                        
//                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo + 2);
//                        SetQueryString(_stmpl_pages);
//                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
//                    }
//                    else if (iPageNo > 0 && iPageNo < 4 && iPageNo < iTotalPages)
//                    {

//                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", 1);
//                        SetQueryString(_stmpl_pages);

//                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                        if (iPageNo == 1)
//                        {
//                            if (1 == iTotalPages)
                               
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
//                            else
//                            {
                                
//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));

//                            }
//                        }
//                        else
//                        {
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
//                        }

//                        if (2 <= iTotalPages)
//                        {
//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 2);
//                            SetQueryString(_stmpl_pages);
                           
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            if (iPageNo == 2)
//                            {
//                                if (2 == iTotalPages)
//                                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
//                                else
//                                {
//                                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));
//                                   // _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                }
//                            }
//                            else
//                            {
                               

//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
//                            }
//                        }

//                        if (3 <= iTotalPages)
//                        {
//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 3);
//                            SetQueryString(_stmpl_pages);
                            
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            if (iPageNo == 3)
//                            {
//                                if (3 == iTotalPages)
//                                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
//                                else
//                                {
//                                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));
//                                }
//                            }
//                            else
//                            {
                               
//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
//                            }
//                        }
//                        if (4 <= iTotalPages)
//                        {
//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 4);
//                            SetQueryString(_stmpl_pages);
                            
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            if (iPageNo == 4)
//                            {
//                                if (4 == iTotalPages)
//                                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
//                                else
//                                {
//                                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));

//                                }
//                            }
//                            else
//                            {
                               
//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
//                            }

//                        }
//                        if (5 <= iTotalPages)
//                        {
//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 5);
//                            SetQueryString(_stmpl_pages);
                           
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            if (iPageNo == 5)
//                            {
                                
//                                if (4 == iTotalPages)
//                                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("\r", "").Replace("\n", "").Replace("-</td>", "</td>"));
//                                else
//                                {
//                                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("\r", "").Replace("\n", "").Replace("-</td>", "</td>"));
//                                }
//                            }
//                            else
//                            {
//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
//                            }

//                        }
//                    }
//                    else
//                        if (iPageNo == iTotalPages && 1 <= iTotalPages - 4 && iPageNo < iTotalPages)
//                        {
                           
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 4);
//                            SetQueryString(_stmpl_pages);
                           
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
//                            SetQueryString(_stmpl_pages);
                           
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
//                            SetQueryString(_stmpl_pages);
                           
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
//                            SetQueryString(_stmpl_pages);
//                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
//                            SetQueryString(_stmpl_pages);
                           
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                            if (iPageNo == iTotalPages)
//                            {
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
//                            }
//                            else
//                            {
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
//                            }

//                        }
//                        else if (iPageNo == iTotalPages && iPageNo < iTotalPages)
//                        {
                           
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
//                            SetQueryString(_stmpl_pages);
                           
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
//                            SetQueryString(_stmpl_pages);

//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                            
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
//                            SetQueryString(_stmpl_pages);
                           
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
//                            SetQueryString(_stmpl_pages);
                            
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                            if (iPageNo == iTotalPages)
//                            {
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
//                            }
//                            else
//                            {
//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
//                            }

//                        }
//                        else if ((1 <= iTotalPages - 4 && iPageNo < iTotalPages) || (1 <= iTotalPages - 4 && iPageNo == iTotalPages))
//                        {
//                            if (iTotalPages - 4 > 0)
//                            {
                               
//                                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 4);
//                                SetQueryString(_stmpl_pages);

//                                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
//                            }


//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                            
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
//                            SetQueryString(_stmpl_pages);

//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());




//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);

//                            SetQueryString(_stmpl_pages);
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());



//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
//                            SetQueryString(_stmpl_pages);
                           
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                           
//                            if (iPageNo != iTotalPages)
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
//                            else
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                            

//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
//                            SetQueryString(_stmpl_pages);
//                            if (iPageNo == iTotalPages)
//                            {
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
//                            }
//                            else
//                            {
                               
//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
//                            }


//                        }


//                        else if (iPageNo == iTotalPages)
//                        {

//                            if (iTotalPages - 3 > 0)
//                            {
                                
//                                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
//                                SetQueryString(_stmpl_pages);
                                
//                                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
//                            }
//                            if (iTotalPages - 2 > 0)
//                            {
//                                _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                                
//                                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
//                                SetQueryString(_stmpl_pages);
                               
//                                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
//                            }
//                            if (iTotalPages - 1 > 0)
//                            {
//                                _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                                
//                                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
//                                SetQueryString(_stmpl_pages);
                                
//                                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
//                            }
//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                            
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
//                            SetQueryString(_stmpl_pages);
                           
//                            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
//                            if (iPageNo == iTotalPages)
//                            {
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
//                            }
//                            else
//                            {
//                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
//                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
//                            }

//                        }
//                    if (iTotalPages > 1 && iPageNo != iTotalPages && iPageNo < iTotalPages)
//                    {
//                         _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpagenoNext");
//                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", (iPageNo + 1));
//                        SetQueryString(_stmpl_pages);
                       
//                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);

//                        _stmpl_container.SetAttribute("TBW_NEXT_PAGE", _stmpl_pages.ToString());




//                    }
//                    else
//                    {

//                        if (iPageNo != iTotalPages)
//                        {
                           
//                            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpagenoNext");
//                            _stmpl_pages.SetAttribute("TBW_PAGE_NO", (iPageNo));
//                            SetQueryString(_stmpl_pages);
//                            _stmpl_container.SetAttribute("TBW_NEXT_PAGE", _stmpl_pages.ToString());
//                        }
//                        else
//                        {
//                            _stmpl_container.SetAttribute("TBW_NEXT_PAGE", "");
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {

//                }
               
//                if (iRecordsPerPage == 32767) //View All
//                {
//                    iRecordsPerPage = iTotalProducts;
//                }
//                _stmpl_container.SetAttribute("TBW_START_PAGE_NO", (iPageNo * iRecordsPerPage) - (iRecordsPerPage - 1));
//                if (((iPageNo * iRecordsPerPage) > iTotalProducts))
//                    _stmpl_container.SetAttribute("TBW_END_PAGE_NO", iTotalProducts);
//                else
//                    _stmpl_container.SetAttribute("TBW_END_PAGE_NO", (iPageNo * iRecordsPerPage));

//                if (iTotalPages > 1 && iPageNo != iTotalPages && iPageNo < iTotalPages)
//                {
//                    _stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
//                    _stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo);
//                    _stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
//                    _stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo);
//                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
//                    sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
//                }
//                else
//                {
//                    iPageNo = iTotalPages;
//                    _stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
//                    _stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo);
//                    _stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
//                    _stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo);
//                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
//                    sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
//                }
                
//                //if (sHTML.ToString().Contains("src=\"/prodimages\""))
//                //{
//                //    sHTML = sHTML.Replace("src=\"/prodimages\"", "src=\"/prodimages/images/noimage.gif\"");
//                //    sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
//                //}
//                //if (sHTML.ToString().Contains("src=\"\""))
//                //{
//                //    sHTML = sHTML.ToString().Replace("src=\"\"", "src=\"/prodimages/images/noimage.gif\"");
//                //    sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
//                //}
//                if (sHTML.ToString().Contains("data-original=\"/prodimages\""))
//                {
//                    sHTML = sHTML.Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
//                    sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
//                }
//                if (sHTML.ToString().Contains("data-original=\"\""))
//                {
//                    sHTML = sHTML.ToString().Replace("data-original=\"\"", "data-original=\"/prodimages/images/noimage.gif\"");
//                    sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
//                }

//                if (dscat.Tables[1].Rows[0].ItemArray[0].ToString() == "0")
//                    sHTML = "";
//                if (dscat.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
//                {
//                    sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1\">Previous</a>", "");
//                    sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1\">Next</a>", "");
//                }
//                DataSet DSnaprod = new DataSet();

//            }
//            string eapath = _EAPath.Replace("'", "###");
//            htmleapath.Value = eapath.ToString();
//            htmlbceapath.Value = _BCEAPath.ToString(); 
//            htmltotalpages.Value = iTotalPages.ToString();
//            htmlviewmode.Value = _ViewType;
//            if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
//            {
//                htmlirecords.Value = Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString();
//            }
//            else
//            {
//                htmlirecords.Value = "24";
//            }
//        }
//        catch (Exception ex)
//        {
//            sHTML = ex.Message;
//            objErrorHandler.ErrorMsg = ex;
//            objErrorHandler.CreateLog();
//        }
//        finally
//        {
           
//        }
       

//        return objHelperServices.StripWhitespace( sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
//    }
    public string CategoryProductList_RenderHTMLJson(string package, string SkinRootPath)
    {
       
        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
       // StringTemplate _stmpl_pages = null;
        DataSet dsprod = new DataSet();
        DataSet dsprodspecs = new DataSet();
        DataSet dscat = new DataSet();
        string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
        bool BindToST = true;
        int oe = 0;
        string category_nameh = string.Empty;
        string sHTML = string.Empty;
        string _pcr = string.Empty;
        string _ViewType = string.Empty;
        string _BCEAPath = string.Empty;
      
        try
        {

            if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != string.Empty)
                _tsm = Request.QueryString["tsm"];

            if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != string.Empty)
                _tsb = Request.QueryString["tsb"];

            if (Request.QueryString["type"] != null)
                _type = Request.QueryString["type"];

            if (Request.QueryString["value"] != null)
                _value = Request.QueryString["value"];

            if (Request.QueryString["bname"] != null)
                _bname = Request.QueryString["bname"];
            if (Request.QueryString["searchstr"] != null)
                _searchstr = Request.QueryString["searchstr"];
            if (Request.QueryString["srctext"] != null)
                _searchstr = Request.QueryString["srctext"];

            if (Request.QueryString["cid"] != null)
                _cid = Request.QueryString["cid"];

            if (Request.QueryString["pcr"] != null)
                _pcr = Request.QueryString["pcr"];


            if (Request.QueryString["ViewMode"] != null)
            {
                _ViewType = Request.QueryString["ViewMode"];
                Session["PL_VIEW_MODE"] = _ViewType;
            }
            else if (Session["PL_VIEW_MODE"] != null && Session["PL_VIEW_MODE"].ToString() != string.Empty)
                _ViewType = Session["PL_VIEW_MODE"].ToString();
            else
                _ViewType = "GV";

          
            if (HttpContext.Current.Session["EA"] != null)
                _EAPath = HttpContext.Current.Session["EA"].ToString();

            if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
            {
                _BCEAPath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();
                _BCEAPath = _BCEAPath.ToString().Replace("Category=", "").Replace("CATEGORY=", "");
            }

            stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());



            dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];



            if (dscat != null)
            {

                if (Request.QueryString["pcr"] != null)
                    _pcr = Request.QueryString["pcr"];
                if (Request.QueryString["type"] == null || Request.QueryString["type"] == "")
                {
                   
                    string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
                    if (tempstr != null && tempstr != "")
                        category_nameh = tempstr;
                }
                else
                    category_nameh = Request.QueryString["value"].ToString();

                DataRow drpagect = dscat.Tables[0].Rows[0];
                iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);

                if (iPageNo > iTotalPages)
                {
                    iPageNo = iTotalPages;
                 
                }
                Session["iTotalPages"] = iTotalPages;
                DataRow drproductsct = dscat.Tables[1].Rows[0];
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
                //if (_cid != "")
                //    _ParentCatID = GetParentCatID(_cid);

                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];
                _stg_records = new StringTemplateGroup("CategoryProductList", stemplatepath);


                int ictrecords = 0;
                int icolstart = 0;
                string trmpstr = string.Empty;
                int icol = 1;
                lstrows = new TBWDataList[24];

            
                if (dscat.Tables["FamilyPro"] == null || dscat.Tables["FamilyPro"].Rows.Count<=0) return "";

                string userid = string.Empty;
                string PriceTable = string.Empty;
        //        int pricecode = 0;
                //string tmpProds = string.Empty;
                DataSet dsBgDisc = new DataSet();
                DataSet dsPriceTableAll = new DataSet();
                if (Session["USER_ID"] != null)
                    userid = Session["USER_ID"].ToString();
                if (userid == "")
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

               
      //          pricecode = objHelperDB.GetPriceCode(userid);

                if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());

                //tmpProds = "";
                //if (Convert.ToInt32(userid) > 0)
                //{
                //    foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                //    {
                //        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                //        tmpProds = tmpProds.Replace(",,", ",");
                //    }
                //    if (tmpProds != "")
                //    {
                //        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                //        dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                //    }
                //}

      //          bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);
      //           string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count + 1];
           
        //        string newurl = string.Empty;

            

                foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                {
                   

                    //if (_ViewType == "GV")
                    _stmpl_records = _stg_records.GetInstanceOf("CategoryProductList\\productlist_GridView");
                  

            //        _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"]);
                    _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"]);
                    _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"]);

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));

                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"]);
                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"]);
                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                    //objErrorHandler.CreateLog(drpid["FAMILY_ID"].ToString() + "--" + trmpstr); 
                    if (_ViewType == "GV")
                    {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                    }

                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);
           //         _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", trmpstr.Replace('"', ' '));
                    //_stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"]);
                    //_stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"]);

                    //_stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomEnabled);
                    string[] catpath = null;
                    string CategoryPath = string.Empty;
                    //if (drpid["CATEGORY_PATH"].ToString() != null)
                    //{
                    //    catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                    //    CategoryPath =  (catpath.Length >=1 ? catpath[0] : " ") + "////" +  (catpath.Length >=2 ? catpath[1] : " ");
                    //}

                    //objErrorHandler.CreateLog(drpid["CATEGORY_PATH"].ToString() + "categorylist");
                    //objErrorHandler.CreateLog(drpid["PARENT_CATEGORY_ID"].ToString() + "PARENT_CATEGORY_ID EA");
                    //objErrorHandler.CreateLog(drpid["CATEGORY_ID"].ToString() + "CATEGORY_ID EA");
                    
                    //objErrorHandler.CreateLog(_cid + "categorylist page");
                    //objErrorHandler.CreateLog(category_nameh + "categoryname...categorylist");
                    if (drpid["CATEGORY_PATH"].ToString().StartsWith(category_nameh) || category_nameh==string.Empty)
                    {
                        //objErrorHandler.CreateLog("WesNewsCategoryId is equal");

                        if (drpid["CATEGORY_PATH"].ToString() != null)
                        {

                            catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                            //objErrorHandler.CreateLog(catpath[0] + "  " + category_nameh);
                            CategoryPath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ");
                        }
                    }
                    else
                    {
                        //objErrorHandler.CreateLog("Inside else WesNewsCategoryId is not equal" + drpid["FAMILY_ID"].ToString());
                        HelperDB objhelperDb = new HelperDB();
                       // DataTable Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, drpid["FAMILY_ID"].ToString(), "", "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                        DataTable Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, drpid["FAMILY_ID"].ToString(), _cid, "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                        if ((Sqltb != null) && (HttpContext.Current.Session["EA"] != null))
                        {
                           // objErrorHandler.CreateLog(Sqltb.Rows[0]["CATEGORY_PATH"].ToString() + "getfamily_category inside easyask");
                            //DataRow[] dr = Sqltb.Select("ParentCatID='" + _cid + "'");
                            //if (dr.Length > 0)
                            //{

                            //    objErrorHandler.CreateLog(dr.Length.ToString() + "getfamily_category inside dr");
                            //    string catpath1 = dr.CopyToDataTable().Rows[0]["CATEGORY_PATH"].ToString(); ;
                            //    catpath = catpath1.Split(new string[] { "////" }, StringSplitOptions.None);

                            //    objErrorHandler.CreateLog(catpath[0] + "  " + category_nameh);
                            string catpath1 = Sqltb.Rows[0]["CATEGORY_PATH"].ToString();
                            catpath = catpath1.Split(new string[] { "////" }, StringSplitOptions.None);
                                //if (catpath.Length >= 3)
                                //{
                                //    CategoryPath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ");
                                //}
                                //else
                                //{
                                    CategoryPath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ");

                                //}

                            //}


                        }
                        else
                        {

                            //objErrorHandler.CreateLog("else no record");
                        }
                    }



                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
              //          _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"]);
                        //modified by :indu
                       
                       
                        //newurl = WAG_Root_Path + "////" +
                        //                    drpid["FAMILY_ID"] + "////" + CategoryPath + "////" +
                        //                    drpid["FAMILY_NAME"];

                        objHelperServices.SimpleURL(_stmpl_records, WAG_Root_Path + "////" + drpid["FAMILY_ID"] + "////" + CategoryPath + "////" + drpid["FAMILY_NAME"], "fl.aspx");
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
             //           _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);



                    
                        //if (Convert.ToInt32(userid) > 0)
                        //{

                        //    PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid),  drpid["PROD_STOCK_FLAG"].ToString(),drpid["ETA"].ToString(), dsPriceTableAll);
                        //}
                        //_stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);


                        

                        //if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                        //    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                        //else
                        //    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);


                        _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"]);

                        if (drpid["QTY_AVAIL"].ToString() != string.Empty)
                        {
                            if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                            {
                                if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                                {
                                    _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"]);
                                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"]);
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                }

                            }
                        }


                   
                   

                        //newurl = WAG_Root_Path + "////" + drpid["FAMILY_ID"] + "////" + drpid["PRODUCT_ID"] + "=" + drpid["PRODUCT_CODE"] + "////" +
                        //    CategoryPath + "////" +
                        //                   drpid["FAMILY_NAME"];



                        objHelperServices.SimpleURL(_stmpl_records, WAG_Root_Path + "////" + drpid["FAMILY_ID"] + "////" + drpid["PRODUCT_ID"] + "=" + drpid["PRODUCT_CODE"] + "////" + CategoryPath + "////" + drpid["FAMILY_NAME"], "pd.aspx");

                    }

                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"]);
                   
                    //if (drpid["Prod_Thumbnail"].ToString().Contains("noimage.gif"))
                    //{
                    //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", "/images/noimage.gif");
                    //}
                    //else
                    //{
                    //    System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                    //    if (Fil.Exists)
                    //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", drpid["Prod_Thumbnail"]);
                    //    else
                    //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", "/images/noimage.gif");
                    //}


                    if (drpid["Prod_Thumbnail"].ToString().Contains("noimage.gif"))
                    {
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", strprodimg_sepdomain+"/images/noimage.gif");
                    }
                    else
                    {
                        //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                        //if (Fil.Exists)
                        //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", strprodimg_sepdomain+drpid["Prod_Thumbnail"]);
                        //else
                        //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", strprodimg_sepdomain+"/images/noimage.gif");

                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", strprodimg_sepdomain + drpid["Prod_Thumbnail"]);
                    }

                    //string desc = string.Empty;
                    string descattr = string.Empty;
                    //string prod_desc_alt = string.Empty;

                   // if (_ViewType == "LV")
                   // {

                    if (drpid["PRODUCT_COUNT"].ToString() != "1")
                    {
                        descattr = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        if (descattr.Length > 0)
                            descattr = descattr + " ";
                            //descattr = descattr + "<br/>";
                        descattr = descattr + " " + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                    }
                    else
                        descattr = drpid["Prod_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");


                        if (descattr.Length > 120) // && _ViewType == "GV"
                        {
                            descattr = descattr.Substring(0, 120).ToString();
                            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr + "...");
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                        }
                        string stk_sta_desc = "";
                        stk_sta_desc = drpid["STOCK_STATUS_DESC"].ToString().Trim();
                    //&& drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
                        BindToST = true;

                        if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                        {
                        }
                        else
                        {
                            if ((stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA"))
                            {
                                if (stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE")
                                {
                                    _stmpl_records.SetAttribute("PRODUCT_STATUS", "Product No Longer Available.</br> Please contact Us");
                                    BindToST = false;
                                    //  objErrorHandler.CreateLog(drpid["PRODUCT_ID"].ToString() + "Product No Longer Available" + "Familypage"); 
                                }
                                else if (stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA")
                                {
                                    _stmpl_records.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable.<br/>Please Contact Us for more details");
                                }
                                if (drpid["PROD_SUBSTITUTE"].ToString().Trim() != "" && drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
                                {
                                    // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid));
                                    DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PRODUCT_CODE"].ToString(), Convert.ToInt32(userid), "pd");
                                    if (rtntbl != null && rtntbl.Rows.Count > 0)
                                    {

               //                         bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

              //                          bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

                                        if ((bool)rtntbl.Rows[0]["samecodenotFound"] == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                        {
                                            if (stk_sta_desc.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
                                            {

                                                DataSet Sqltbs = objHelperDB.GetProductPriceEA("", rtntbl.Rows[0]["SubstuyutePid"].ToString(), userid);
                                                if (Sqltbs != null)
                                                {

                                        //            string stockstaus = Sqltbs.Tables[0].Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().ToUpper();
                                                    //  objErrorHandler.CreateLog("Sqltbs" + stockstaus);
                                                    if ((Sqltbs.Tables[0].Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().ToUpper() == "DISCONTINUED NO LONGER AVAILABLE"))
                                                    {

                                                        BindToST = false;
                                                    }
                                                    else
                                                    {
                                                        BindToST = true;
                                                    }
                                                }
                                            }
                                            _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                                            _stmpl_records.SetAttribute("TBT_SUB_PRODUCT", true);
                                            _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                       //                     string strurl = rtntbl.Rows[0]["ea_path"].ToString();
                                            _stmpl_records.SetAttribute("TBT_REP_EA_PATH", rtntbl.Rows[0]["ea_path"].ToString());
                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);

                                        }
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                                    }
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);

                                }

                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                                if (stk_sta_desc.ToUpper().Contains("OUT_OF_STOCK") == true && drpid["PROD_SUBSTITUTE"].ToString().Trim() != "" && drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
                                {
                                    // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid));
                                    DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PRODUCT_CODE"].ToString(), Convert.ToInt32(userid), "pd");
                                    if (rtntbl != null && rtntbl.Rows.Count > 0)
                                    {

             //                           bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

             //                           bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                                        //objErrorHandler.CreateLog("OUT_OF_STOCK" + samecodenotFound + "--" + rtntbl.Rows[0]["ea_path"].ToString());
                                        if ((bool)rtntbl.Rows[0]["samecodenotFound"] == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                        {

                                            if (stk_sta_desc.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
                                            {

                                                DataSet Sqltbs = objHelperDB.GetProductPriceEA("", rtntbl.Rows[0]["SubstuyutePid"].ToString(), userid);
                                                if (Sqltbs != null)
                                                {

               //                                     string stockstaus = Sqltbs.Tables[0].Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().ToUpper();

                                                    if ((Sqltbs.Tables[0].Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().ToUpper() == "DISCONTINUED NO LONGER AVAILABLE"))
                                                    {

                                                        BindToST = false;
                                                    }
                                                    else
                                                    {
                                                        BindToST = true;
                                                    }
                                                }
                                            }
                                            _stmpl_records.SetAttribute("TBT_SUB_PRODUCT", true);
                                            _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                //                            string strurl = rtntbl.Rows[0]["ea_path"].ToString();
                                            _stmpl_records.SetAttribute("TBT_REP_EA_PATH", rtntbl.Rows[0]["ea_path"].ToString());
                                        }
                                    }

                                }
                            }
                        }
                      //  objErrorHandler.CreateLog(stk_sta_desc + "ct");
                        //if ((stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE") || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA")
                        //    _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                        //else
                        //    _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);



                        //if ((stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA") && drpid["PROD_STOCK_STATUS"].ToString().Trim() == "0" && drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
                        //{

                        //    if ((stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE") && (drpid["PROD_SUBSTITUTE"].ToString().Trim() != ""))
                        //    {
                        //        DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid), "pd");

                        //        if (rtntbl != null && rtntbl.Rows.Count > 0)
                        //        {
                        //            _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                        //            _stmpl_records.SetAttribute("TBT_SUB_PRODUCT", true);
                        //            _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                        //            _stmpl_records.SetAttribute("TBT_REP_EA_PATH", rtntbl.Rows[0]["ea_path"].ToString());
                        //        }
                        //    }
                        //    else
                        //    {
                        //        _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);

                        //    }

                        //}
                        //else
                        //{
                        //    _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                        //}

                        //prod_desc_alt = drpid["Prod_Description"].ToString();
                        //if (prod_desc_alt.Length > 0)
                        //{
                        //    _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                        //    _stmpl_records.SetAttribute("PROD_DESC_TITLE", prod_desc_alt.Replace('"', ' '));
                        //}
                        //else
                        //{
                        //    _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"]);
                        //    _stmpl_records.SetAttribute("PROD_DESC_TITLE", prod_desc_alt.Replace('"', ' '));
                        //}
                        //if (desc.Length > 230)// && _ViewType == "LV"
                        //{
                        //    _stmpl_records.SetAttribute("TBT_MORE_13", true);
                        //    desc = desc.Substring(0, 230).ToString();
                        //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_13", desc);
                        //}
                        //else
                        //{
                        //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_13", desc);
                        //    _stmpl_records.SetAttribute("TBT_MORE_13", false);
                        //}
                        //desc = drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        //if (desc.Length > 230) //&& _ViewType == "LV"
                        //{
                        //    _stmpl_records.SetAttribute("TBT_MORE_90", true);
                        //    desc = desc.Substring(0, 230).ToString();
                        //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_90", desc);
                        //}
                        //else
                        //{
                        //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_90", desc);
                        //    _stmpl_records.SetAttribute("TBT_MORE_90", false);
                        //}


                   // }
                    //else
                    //{

                    //    descattr = drpid["Family_ShortDescription"].ToString();
                    //    if (descattr.Length > 0)
                    //        descattr = descattr + "<br/>";
                    //    descattr = descattr + drpid["Family_Description"].ToString() + " " + drpid["Prod_Description"].ToString();

                    //    prod_desc_alt = drpid["Prod_Description"].ToString();
                    //    if (prod_desc_alt.Length > 0)
                    //    {
                    //        _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                    //        _stmpl_records.SetAttribute("PROD_DESC_TITLE", prod_desc_alt.Replace('"', ' '));
                    //    }
                    //    else
                    //    {
                    //        _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"]);
                    //        _stmpl_records.SetAttribute("PROD_DESC_TITLE", drpid["FAMILY_NAME"].ToString().Replace('"', ' '));
                    //    }
                    //    if (descattr.Length > 140 && _ViewType == "GV")
                    //    {
                    //        int count = 0;
                    //        count = descattr.Count(c => char.IsUpper(c));
                    //        int count1 = 0;
                    //        count1 = descattr.Count(c => char.IsSymbol(c)) + descattr.Count(c => char.IsNumber(c));
                    //        if (count >= 35)
                    //        {
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //            descattr = descattr.Substring(0, 120).ToString();
                    //            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //        }
                    //        else if (descattr.Length > 140 && count < 35 && count1 > 10)
                    //        {
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //            descattr = descattr.Substring(0, 135).ToString();
                    //            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //        }
                    //        else if (descattr.Length > 140 && count < 35 && count1 < 10)
                    //        {
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //            descattr = descattr.Substring(0, 140).ToString();
                    //            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);

                    //        }

                    //    }
                    //    else
                    //    {
                    //        _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //    }
                    //}






                        if (BindToST == true)
                        {
                            lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                        }

                
                    icolstart++;
                  
                }

             

                _stg_container = new StringTemplateGroup("CategoryProductListcontainer", stemplatepath);
                _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "main");
              
                //if (_ViewType == "LV")
                //    _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
                //else
                //    _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);
                  

                
                _stmpl_container.SetAttribute("TBWDataList", lstrows);
                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
         //       _stmpl_container.SetAttribute("TBT_REWRITEURL_REL", "ct.aspx?" + Request.QueryString.ToString());
                sHTML = _stmpl_container.ToString();
                        //.Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
             
               
                //if (sHTML.ToString().Contains("data-original=\"/prodimages\""))
                //{
                //    sHTML = sHTML.Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
                //    sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                //}
                //if (sHTML.ToString().Contains("data-original=\"\""))
                //{
                //    sHTML = sHTML.ToString().Replace("data-original=\"\"", "data-original=\"/prodimages/images/noimage.gif\"");
                //    sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                //}

                //if (dscat.Tables[1].Rows[0].ItemArray[0].ToString() == "0")
                //    sHTML = "";
                //if (dscat.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
                //{
                //    sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1\">Previous</a>", "");
                //    sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1\">Next</a>", "");
                //}
                //DataSet DSnaprod = new DataSet();
            }
            string eapath = _EAPath.Replace("'", "###");
            htmleapath.Value = eapath.ToString();
            htmlbceapath.Value = _BCEAPath.ToString();
            htmltotalpages.Value = iTotalPages.ToString();
            htmlviewmode.Value = _ViewType;

            if (_ViewType == "LV")
            {
                sHTML = sHTML.Replace("product-grid", "product-grid list-wrapper");
                sHTML = sHTML.Replace("dpynone", "pro_discrip");
            }

            if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
            {
                htmlirecords.Value = Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString();
            }
            else
            {
               // htmlirecords.Value = "18";
                htmlirecords.Value = System.Configuration.ConfigurationManager.AppSettings["iRecordsPerPage"].ToString();
                Session["RECORDS_PER_PAGE_CATEGORY_LIST"] = htmlirecords.Value;
            }
           
        }
        catch (Exception ex)
        {
            sHTML = ex.Message;
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        finally
        {

        }

    


 
        return objHelperServices.StripWhitespace(sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
    }
    private void SetQueryString(StringTemplate _stmpl_pages)
    {
        _stmpl_pages.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_pcr));
        _stmpl_pages.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_cid));
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(_searchstr));
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_TYPE", _type);
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(_value));
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(_bname));
        _stmpl_pages.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
        _stmpl_pages.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

    }
    private string GetParentCatID(string catID)
    {
        try
        {
            DataSet DSBC = null;
            string catIDtemp = catID;
            do
            {
                //DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORYLIST_CAREGORY", HelperDB.ReturnType.RTDataSet);
                if (DSBC != null)
                {
                    //Indu Start
                    int cnt=DSBC.Tables[0].Rows.Count ;
                    for (int i = 0; i < cnt;i++ )
                    {
                        catIDtemp = DSBC.Tables[0].Rows[i]["PARENT_CATEGORY"].ToString();
                        if (catIDtemp == "0")
                        {
                            // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                            return DSBC.Tables[0].Rows[i]["CATEGORY_ID"].ToString();
                        }
                    }
                    //End
                }
            } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
            return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        }
        catch (Exception ex)
        {

        }
        return "";
    }

    public string ST_Categories()
    {
        UC_maincategory ucmain = new UC_maincategory();
     return   ucmain.ST_Categories(); 
    
    }
    public string Category_RenderHTML(string Package, string SkinRootPath)
    {
       
    
        //string skin_container = null;
        //int grid_cols = 0;
        //int grid_rows = 0;
        //string skin_sql_container = null;
        //string skin_sql_param_container = null;
        //string skin_records = null;

        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrecords1 = new TBWDataList[0];
        TBWDataList[] lstrecords2 = new TBWDataList[0];
        StringTemplateGroup stg_records = null;
        //StringTemplate bodyST_categorylist = null;
        //StringTemplate bodyST_subcategorylist = null;
        //StringTemplate bodyST = null;
        DataSet dspkg = new DataSet();
    //    string _wcat = null;

       // DataTable SubCategory = new DataTable();


        //if (Request.QueryString["wcat"] != null)
        //    _wcat = Request.QueryString["wcat"];

        try
        {


            //string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
            //string strPDFFiles2 = HttpContext.Current.Server.MapPath("News update");


            if (Request.QueryString["bypcat"] == null)
            {
               // mobpopfil.Visible = true;
                if (Request.QueryString["cid"] != null)
                {

                    //string sqlpkginfo = " SELECT * FROM TBW_PACKAGE ";
                    //sqlpkginfo = sqlpkginfo + " WHERE PACKAGE_NAME = '" + Package + "'";
                    //dspkg = GetDataSet(sqlpkginfo);
                    //dspkg = (DataSet)objHelperDB.GetGenericDataDB(Package, "GET_PACKAGE_WITHOUT_ISROOT", HelperDB.ReturnType.RTDataSet);
                    //if (dspkg != null)
                    //{
                    //    if (dspkg.Tables[0].Rows.Count > 0)
                    //    {
                    //        //Indu Start
                    //      int cnt=dspkg.Tables[0].Rows.Count;
                    //      for (int i = 0; i < cnt;i++ )
                    //      {
                    //          skin_container = dspkg.Tables[0].Rows[i]["SKIN_NAME"].ToString();
                    //          grid_cols = Convert.ToInt32(dspkg.Tables[0].Rows[i]["GRID_COLS"]);
                    //          grid_rows = Convert.ToInt32(dspkg.Tables[0].Rows[i]["GRID_ROWS"]);
                    //          skin_sql_container = dspkg.Tables[0].Rows[i]["SKIN_SQL"].ToString();
                    //          skin_sql_param_container = dspkg.Tables[0].Rows[i]["SKIN_SQL_PARAM"].ToString();
                    //          skin_records = dspkg.Tables[0].Rows[i]["SKIN_NAME"].ToString();
                    //      }
                    //        //End
                    //    }
                    //}
                    if (Request.QueryString["cid"] != null)
                        _catId = Request.QueryString["cid"].ToString();

                    stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());

                    stg_records = new StringTemplateGroup("Categorylist", stemplatepath + "\\" + "Categorylist");
                    //}
                   
                    //DataSet dscatname = GetDataSet(sqlcatquery);                

                    //SubCategory = EasyAsk.GetMainMenuClickDetail(_catId, "SubCategory");
                    DataTable dscatname = new DataTable();
                   // SubCategory = (DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["SubCategory"];

                    //dscatname = EasyAsk.GetMainMenuClickDetail(_catId, "MainCategory");
                    if (HttpContext.Current.Session["MainMenuClick"] != null)
                    {
                        dscatname = (DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["MainCategory"];
                    }
                    //if (dscatname == null)
                    //    return "";

                    DataSet dscat = new DataSet();

                   // DataRow[] row = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + _catId + "'");
                    DataSet ds = (DataSet)HttpContext.Current.Application["key_MainCategory"];
                    DataRow[] row = ds.Tables[0].Select("CATEGORY_ID='" + _catId + "'");
                    if (row.Length > 0)
                    {
                        dscat.Tables.Add(row.CopyToDataTable());
                    }
                    StringTemplate bodyST_main = stg_records.GetInstanceOf("main");
                    bodyST_main.SetAttribute("TBT_CATEGORY_NAME", dscat.Tables[0].Rows[0][0].ToString()   );



                    stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());

                   string  stmplrecords1 =stemplatepath + "\\Categorylist" + "\\" + "cell";
                 
                   StringTemplate _stmpl_records = null;
                  string TBWDataList = string.Empty;
                  string TBWDIV = string.Empty;  

                  for (int i = 0; i <dscatname.Rows.Count; i++)
                     {
                         //objErrorHandler.CreateLog(stmplrecords1);
                         _stmpl_records = stg_records.GetInstanceOf("cell");
        //                 _stmpl_records.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                         _stmpl_records.SetAttribute("SubCategory", dscatname.Rows[i][0].ToString());
                         _stmpl_records.SetAttribute("TBW_PRO_CNT", dscatname.Rows[i]["PRODUCT_COUNT"].ToString());
                         string rewriteurl = objHelperServices.SimpleURL_Str(dscat.Tables[0].Rows[0][0].ToString() + "////" + dscatname.Rows[i][0].ToString(),"pl.aspx",true);
                         _stmpl_records.SetAttribute("TBT_REWRITEURL", rewriteurl);
                         TBWDataList = TBWDataList + _stmpl_records.ToString();
                     
                             //_stmpl_records_div = stg_records.GetInstanceOf(stmplrecords_div);
                             //_stmpl_records_div.SetAttribute("TBWDataList", TBWDataList);
                             //TBWDataList = string.Empty;
                             //TBWDIV = TBWDIV + _stmpl_records_div.ToString();
                        
                     }

                  if (HttpContext.Current.Session["SortOrder"] != null && HttpContext.Current.Session["SortOrder"] != "")
                  {

                      if (HttpContext.Current.Session["SortOrder"].ToString() == "Latest")
                      {
                          bodyST_main.SetAttribute("SortBy", "Latest");
                      }

                      else if (HttpContext.Current.Session["SortOrder"].ToString() == "ltoh")
                      {
                          bodyST_main.SetAttribute("SortBy", "Price Low To High");
                      }
                      else if (HttpContext.Current.Session["SortOrder"].ToString() == "htol")
                      {
                          bodyST_main.SetAttribute("SortBy", "Price High To Low");
                      }
                      else if (HttpContext.Current.Session["SortOrder"].ToString() == "popularity")
                      {
                          bodyST_main.SetAttribute("SortBy", "Popular");
                      }
                      else
                          bodyST_main.SetAttribute("SortBy", "Latest");
                  }
                  else
                      bodyST_main.SetAttribute("SortBy", "Latest");

                  bodyST_main.SetAttribute("TBWDataList", TBWDataList);
                    //bodyST_main.SetAttribute("TBWDataList1", lstrecords1);
                   // bodyST_main.SetAttribute("TBWDataList3", ST_newproductsnav());
                  
                    string sHtmls = bodyST_main.ToString();


                  
                    
                   
                    return objHelperServices.StripWhitespace(sHtmls);
                }
            }
            else
            {
               // mobpopfil.Visible = false;
                if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
                {
            //        string tosuite_brand = Server.UrlDecode(Request.QueryString["tsb"].ToString());
                    _catId = Request.QueryString["cid"].ToString();
            //        _catName = GetCName(_catId);
                    //string sqlcatquery = "SELECT DISTINCT TOSUITE_MODEL,'/Section17_th/' + TOSUITE_MODEL_IMAGE AS IMAGE_FILE FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 <> '' AND CATEGORY_ID=N'" + _catId + "' AND CATALOG_ID=" + _catalogid + " AND TOSUITE_BRAND = '" + tosuite_brand + "' ORDER BY TOSUITE_MODEL";
                    //DataSet dscatname = GetDataSet(sqlcatquery);

                    HelperServices objHelperServices = new HelperServices();
                    
                    DataSet dscatname = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
                    TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CATEGORYLISTIMG", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                    tbwtEngine.paraValue = _catId;
                    tbwtEngine.GDataSet = dscatname;
                   // objErrorHandler.CreateLog("WESBrand_Model" + dscatname.Tables["Model"].Rows.Count);
                    //if (dscatname.Tables["Model"].Rows.Count == 1)
                    //{

                    //     dscatname = EasyAsk.GetWESModel(_catName, Convert.ToInt32(objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString()), tosuite_brand);
                    //     objErrorHandler.CreateLog("after model direct fetch:" + dscatname.Tables["Model"].Rows.Count);
                    //}
                    tbwtEngine.RenderHTML("Row");
                    return (tbwtEngine.RenderedHTML);
                }
                else if (Request.QueryString["tsb"] == null && Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                {
                    _catId = Request.QueryString["cid"].ToString();
             //       _catName = GetCName(_catId);
                    //string sqlcatquery = "SELECT  DISTINCT TOSUITE_BRAND,TOSUITE_BRAND_IMAGE AS IMAGE_FILE FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 <> '' AND CATEGORY_ID=N'" + _catId + "' AND CATALOG_ID=" + _catalogid + " ORDER BY TOSUITE_BRAND";
                    //DataSet dscatname = GetDataSet(sqlcatquery);
                    DataSet dscatname = new DataSet();
                    HelperServices objHelperServices = new HelperServices();
                    //dscatname.Tables.Add (EasyAsk.GetMainMenuClickDetail(_catalogid ,"Brand").Copy());//
                    dscatname.Tables.Add((DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"].Copy());


                    TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CATEGORYLISTIMG", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                    tbwtEngine.paraValue = _catId;
                    tbwtEngine.GDataSet = dscatname;
                    tbwtEngine.RenderHTML("Row");
                    return (tbwtEngine.RenderedHTML);
                }
                else
                {
                    _catId = Request.QueryString["cid"].ToString();
                    //old string sqlcatquery = "select tc.CATEGORY_ID,tc.CATEGORY_NAME,tc.IMAGE_FILE from tb_category tc, tb_catalog_sections tcs where tc.category_id=tcs.category_id and tc.category_name <>'Brand' and tc.Category_name <>'Product' AND ISNULL(TC.SHORT_DESC,'')<>'NOT FOR WEB' and tcs.catalog_id=" + _catalogid + " and tc.parent_category='" + _catId + "' order by tc.category_name";
                    //string sqlcatquery = "select tc.CATEGORY_ID,tc.CATEGORY_NAME,tc.IMAGE_FILE from tb_category tc, tb_catalog_sections tcs where tc.category_id=tcs.category_id and tc.category_name <>'Brand' and tc.Category_name <>'Product' AND ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3 and tcs.catalog_id=" + _catalogid + " and tc.parent_category='" + _catId + "' order by tc.category_name";
                    //DataSet dscatname = GetDataSet(sqlcatquery);
                    DataSet dscatname = (DataSet)objHelperDB.GetGenericPageDataDB("", _catalogid, _catId, "GET_CATEGORYLIST_CATEGORY_NAME", HelperDB.ReturnType.RTDataSet);

                    HelperServices objHelperServices = new HelperServices();
                    TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CATEGORYLISTIMG", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                    tbwtEngine.paraValue = _catId;
                    tbwtEngine.GDataSet = dscatname;
                    tbwtEngine.RenderHTML("Row");
                    return (tbwtEngine.RenderedHTML);
                }
            }
        }
        catch (Exception ex)
        {
            return (ex.ToString()) ;
            //objErrorHandler.ErrorMsg = ex;
            //objErrorHandler.CreateLog();
        }
       

        return "";

    }

    public string newproductsnav_RenderHTML(string Package, string SkinRootPath)
    {
        return "";
    }
    private string GetCName(string catID)
    {
        DataSet DSBC = null;
        string catIDtemp = catID;
        do
        {
            //DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
            DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORYLIST_CAREGORY", HelperDB.ReturnType.RTDataSet);
            if (DSBC != null)
            {
                int cnt=DSBC.Tables[0].Rows.Count;
                for (int i = 0; i < cnt;i++ )
                {
                    catIDtemp = DSBC.Tables[0].Rows[i]["PARENT_CATEGORY"].ToString();
                    if (catIDtemp == "0")
                    {
                        // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                        return DSBC.Tables[0].Rows[i]["CATEGORY_NAME"].ToString();
                    }
                }
            }
        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        return DSBC.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
    }
    public string Bread_Crumbs()
    {

        string breadcrumb = "", paraPID = "", paraFID = "", paraCID = "", byp = "";
        if (Request.QueryString["pid"] != null)
        {
            paraPID = Request.QueryString["pid"].ToString();
        }
        if (Request.QueryString["fid"] != null)
            paraFID = Request.QueryString["pid"].ToString();
        if (Request.QueryString["cid"] != null)
            paraCID = Request.QueryString["cid"].ToString();
        if (Request.QueryString["byp"] != null)
            byp = Request.QueryString["byp"].ToString();
        // by Jech
        //if (paraPID != "")
        //{
        //    DataSet DSBC = null;

        //    DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
        //    foreach (DataRow DR in DSBC.Tables[0].Rows)
        //    {
        //        breadcrumb = DR[0].ToString();
        //    }
        //    if (paraFID != "")
        //    {
        //        string catIDtemp = "";
        //        DSBC = GetDataSet("SELECT family_name,category_id FROM TB_family WHERE family_ID = " + paraFID);
        //        foreach (DataRow DR in DSBC.Tables[0].Rows)
        //        {
        //            breadcrumb = DR[0].ToString() + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //            catIDtemp = DR[1].ToString();
        //        }
        //        do
        //        {
        //            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
        //            foreach (DataRow DR in DSBC.Tables[0].Rows)
        //            {
        //                breadcrumb = DR["CATEGORY_NAME"].ToString() + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                catIDtemp = DR["PARENT_CATEGORY"].ToString();
        //            }
        //        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        //    }
        //}
        //else if (paraFID != "")
        //{
        //    DataSet DSBC = null;
        //    string catIDtemp = "";
        //    DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
        //    foreach (DataRow DR in DSBC.Tables[0].Rows)
        //    {
        //        breadcrumb = DR[0].ToString();
        //        catIDtemp = DR[1].ToString();
        //    }
        //    do
        //    {
        //        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
        //        foreach (DataRow DR in DSBC.Tables[0].Rows)
        //        {
        //            if (breadcrumb == "")
        //                breadcrumb = DR["CATEGORY_NAME"].ToString();
        //            else
        //                breadcrumb = DR["CATEGORY_NAME"].ToString() + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //            catIDtemp = DR["PARENT_CATEGORY"].ToString();
        //        }
        //    } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        //}
        //else if (paraCID != "")
        //{
        //    DataSet DSBC = null;
        //    string catIDtemp = paraCID;
        //    do
        //    {
        //        // Jtech mohan
        //        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
        //        //DataRow[] row = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + catIDtemp + "'");
        //        //if (row.Length > 0)
        //        //{
        //        //    DSBC.Tables.Add(row.CopyToDataTable());
        //        //}
        //        //else
        //        //{
        //        //    DataRow[] row = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + catIDtemp + "'");
        //        //}

        //        //Jech Mohan
        //        foreach (DataRow DR in DSBC.Tables[0].Rows)
        //        {
        //            if (DR["PARENT_CATEGORY"].ToString() != "0")
        //            {
        //                if (breadcrumb == "")
        //                    breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                else
        //                    breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "\"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //            }
        //            else
        //            {
        //                if (breadcrumb == "")
        //                {
        //                    if (Request.QueryString["bypcat"] != null && Request.QueryString["bypcat"].ToString() != "")
        //                    {
        //                        //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "&bypcat=" + Request.QueryString["bypcat"].ToString() + "\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                    }
        //                    else
        //                    {
        //                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                    }
        //                    if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
        //                    {
        //                        //breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "&bypcat=" + Request.QueryString["bypcat"].ToString() + "&tsb=" + Request.QueryString["tsb"].ToString() + "\" style=\"color:Black;\">" + Request.QueryString["tsb"].ToString() + "</a>";
        //                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "\" style=\"color:Black;\">" + Server.UrlDecode(Request.QueryString["tsb"].ToString()) + "</a>";
        //                    }
        //                }
        //                else
        //                {
        //                    if (Request.QueryString["bypcat"] != null && Request.QueryString["bypcat"].ToString() != "")
        //                    {
        //                        //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=" + byp + "&bypcat=" + Request.QueryString["bypcat"].ToString() + "\"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=" + byp + " \"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                    }
        //                    else
        //                    {
        //                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=" + byp + "\"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                    }
        //                    if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
        //                    {
        //                        //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "&bypcat=" + Request.QueryString["bypcat"].ToString() + "&tsb=" + Request.QueryString["tsb"].ToString() + "\" style=\"color:Black;\">" + Request.QueryString["tsb"].ToString() + "</a><font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "\" style=\"color:Black;\">" + Server.UrlDecode(Request.QueryString["tsb"].ToString()) + "</a><font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                    }
        //                }
        //            }
        //            catIDtemp = DR["PARENT_CATEGORY"].ToString();
        //        }
        //    } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");

        //}

      
        breadcrumb= EasyAsk.GetBreadCrumb(Server.MapPath("Templates"));
      
        return breadcrumb;
    }
    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery,  conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(';') + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}
    //public string ST_PDFDownload()
    //{
    //    StringTemplateGroup _stg_container = null;
    //    StringTemplateGroup _stg_records = null;
    //    StringTemplate _stmpl_container = null;
    //    StringTemplate _stmpl_records = null;
    //    TBWDataList[] lstrecords = new TBWDataList[0];
   

    //    string shtml = "";
    //    int counter = 0;

    //    if (Directory.Exists(Server.MapPath("attachments")))
    //    {

    //        //string[] fileEntries = Directory.GetFiles(Server.MapPath("attachments"), "*.pdf");
    //        //lstrecords = new TBWDataList[fileEntries.Length];
    //        //filenames = new string[fileEntries.Length];
    //        //if (fileEntries.Length > 0)

    //        DataSet dsPDFCount = new DataSet();
    //        dsPDFCount = objCategoryServices.GetCatalogPDFCount(2);

    //        //if (dsPDFCount != null)
    //        //{
    //        //    foreach (DataRow rPDF in dsPDFCount.Tables[0].Rows)
    //        //    {
    //        //        lstrecords = new TBWDataList[Convert.ToInt32(rPDF["CountFiles"].ToString())];
    //        //    }
    //        //}
    //        lstrecords = new TBWDataList[1];
    //        if (lstrecords.Length > 0)
    //        {

    //            DataSet dsCatalog = new DataSet();
    //            try
    //            {
    //                dsCatalog = objCategoryServices.GetCatalogPDFDownload1(_catId);
    //                if (dsCatalog != null)
    //                {
    //                    //Indu Start
    //                    //foreach (DataRow rCat in dsCatalog.Tables[0].Rows)
    //                    int cnt = dsCatalog.Tables[0].Rows.Count;
    //                    for(int i=0;i<cnt;i++)
    //                    {
    //                        string MyFile = Server.MapPath(string.Format("attachments/{0}", dsCatalog.Tables[0].Rows[i]["IMAGE_FILE2"].ToString()));

    //                        _stg_records = new StringTemplateGroup("Categorylist", stemplatepath);
    //                        _stmpl_records = _stg_records.GetInstanceOf("Categorylist" + "\\" + "cell3");
    //                        _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
    //                        _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "row");

    //                        if (System.IO.File.Exists(MyFile))
    //                        {
    //                            _stmpl_records.SetAttribute("PDF", dsCatalog.Tables[0].Rows[i]["IMAGE_FILE2"].ToString());
    //                          //  _stmpl_records.SetAttribute("PDFFILEDESCRIPTION", rCat["IMAGE_NAME2"].ToString());

    //                            FileInfo finfo = new FileInfo(Server.MapPath(string.Format("attachments/{0}", dsCatalog.Tables[0].Rows[i]["IMAGE_FILE2"].ToString())));
    //                            long FileInBytes = finfo.Length;
    //                            long FileInKB = finfo.Length / 1024;

    //                           // _stmpl_records.SetAttribute("PDF_SIZE", FileInKB + " KB");
    //                          //  _stmpl_records.SetAttribute("PDF_DATE", rCat["MODIFIED_DATE"].ToString());

    //                            _stmpl_container.SetAttribute("TBWDataList", _stmpl_records.ToString());
    //                            lstrecords[counter] = new TBWDataList(_stmpl_container.ToString());
    //                            counter++;
    //                        }
    //                    }
    //                    //End
    //                }
    //            }
    //            catch (Exception e)
    //            {
    //                objErrorHandler.ErrorMsg = e;
    //                objErrorHandler.CreateLog(); 
    //            }

    //            _stg_container = new StringTemplateGroup("Categorylist", stemplatepath);
    //            _stmpl_container = _stg_container.GetInstanceOf("Categorylist" + "\\" + "main2");
    //            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
    //            shtml = _stmpl_container.ToString();
    //        }
    //        else
    //            return "<table height=\"100\"><tr><td valign=\"bottom\">No PDF Catalogue found</td></tr></table>";
    //    }
    //    else
    //        return "<table height=\"100\"><tr><td valign=\"bottom\">No PDF catalogue found</td></tr></table>";
    //    return shtml;

    //}


    //public string DynamicPag_RenderHTML(string url, int ipageno, string eapath, string BCEAPath, string ViewMode, string irecords, string balrecords)
    //{
    //    //if (HttpContext.Current.Session["PS_SEARCH_RESULTS"] == null)
    //    //{
    //    //    return "";
    //    //}
    //    stemplatepath = HttpContext.Current.Server.MapPath("Templates");
    //   // string package="CATEGORYPRODUCTLIST";
    //         string SkinRootPath=HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
    //    FamilyServices objFamilyServices = new FamilyServices();
    //    StringTemplateGroup _stg_container = null;
    //    StringTemplateGroup _stg_records = null;
    //    StringTemplate _stmpl_container = null;
    //    StringTemplate _stmpl_records = null;
    //   // StringTemplate _stmpl_pages = null;
    //    DataSet dsprod = new DataSet();
    //    DataSet dsprodspecs = new DataSet();
    //    DataSet dscat = new DataSet();
    //    string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();

    //    int oe = 0;
    //    string category_nameh = string.Empty;
    //    string sHTML = string.Empty;
    //    string _pcr = string.Empty;
    //    string _ViewType = string.Empty;
    //    string _BCEAPath = string.Empty;
    //    //string catID = "";



    //    //if (Convert.ToInt32(HttpContext.Current.Session["PS_SEARCH_RESULTS"].ToString()) > 0)
    //    //{
    //    try
    //    {

    //        //balrecords.Value 
    //        //currpage.Value 
    //        //dscat = Get_Value_Breadcrum(ipageno, eapath, irecords);

    //        if (balrecords == "3" || balrecords == "2" || balrecords == "1")
    //        {
    //            irecords = "20";
    //            balrecords = "0";
    //        }
    //        else
    //        {
    //            irecords = "24";
    //            balrecords = "0";
    //        }
    //        //else if (balrecords == "2")
    //        //{
    //        //    irecords = "24";
    //        //    balrecords = "0";
    //        //}
    //        //else if (balrecords == "1")
    //        //{
    //        //    irecords = "24";
    //        //    balrecords = "0";
    //        //}
           

    //        dscat = Get_Value_Breadcrum(ipageno, eapath, irecords);

    //        //if (ipageno == 2)
    //        //{
                
    //        //    dscat = Get_Value_Breadcrum(ipageno, eapath, "21");
    //        //}
    //        //if (ipageno == 3)
    //        //{
    //        //    dscat = Get_Value_Breadcrum(ipageno, eapath, "24");
    //        //}


    //        if (HttpContext.Current.Request.QueryString["tsm"] != null && HttpContext.Current.Request.QueryString["tsm"].ToString() != "")
    //            _tsm = HttpContext.Current.Request.QueryString["tsm"];

    //        if (HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request.QueryString["tsb"].ToString() != "")
    //            _tsb = HttpContext.Current.Request.QueryString["tsb"];

    //        if (HttpContext.Current.Request.QueryString["type"] != null)
    //            _type = HttpContext.Current.Request.QueryString["type"];

    //        if (HttpContext.Current.Request.QueryString["value"] != null)
    //            _value = HttpContext.Current.Request.QueryString["value"];

    //        if (HttpContext.Current.Request.QueryString["bname"] != null)
    //            _bname = HttpContext.Current.Request.QueryString["bname"];
    //        if (HttpContext.Current.Request.QueryString["searchstr"] != null)
    //            _searchstr = HttpContext.Current.Request.QueryString["searchstr"];
    //        if (HttpContext.Current.Request.QueryString["srctext"] != null)
    //            _searchstr = HttpContext.Current.Request.QueryString["srctext"];

    //        if (HttpContext.Current.Request.QueryString["cid"] != null)
    //            _cid = HttpContext.Current.Request.QueryString["cid"];

    //        if (HttpContext.Current.Request.QueryString["pcr"] != null)
    //            _pcr = HttpContext.Current.Request.QueryString["pcr"];


           

    //        //if (HttpContext.Current.Request.QueryString["path"] != null)
    //        //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString()));
    //        //if (HttpContext.Current.Session["iPageNo"] != null)
    //        //{
    //        //    iPageNo = Convert.ToInt32(HttpContext.Current.Session["iPageNo"]);
    //        //}


    //        _ViewType = ViewMode;
    //        _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();


    //        _BCEAPath = BCEAPath.ToString().Replace("Category=","").Replace("CATEGORY=","") ;


    //        //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true)
    //        //{
    //        //    if (_bypcat == null)
    //        //    {

    //        //        EasyAsk.GetMainMenuClickDetail(_cid, "");


    //        //        string CatName = "";
    //        //        DataTable tmptbl = null;
    //        //        if (HttpContext.Current.Session["MainMenuClick"] != null)
    //        //        {
    //        //            tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0];

    //        //            tmptbl = tmptbl.Select("CATEGORY_ID='" + _cid + "'").CopyToDataTable();

    //        //            if (tmptbl != null && tmptbl.Rows.Count > 0)
    //        //            {
    //        //                CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
    //        //            }


    //        //        }


    //        //        //if (HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
    //        //        //    iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());

    //        //        EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname, irecords.ToString(), (iPageNo - 1).ToString(), "Next");

    //        //    }
    //        //    else if (_tsb != "")
    //        //    {
    //        //        string parentCatName = GetCName(_cid);
    //        //        EasyAsk.GetWESModel(parentCatName, iCatalogId, _tsb);
    //        //    }

    //        //}

    //        //if (HttpContext.Current.Session["EA"] != null)
    //        //    _EAPath = HttpContext.Current.Session["EA"].ToString();


    //        //stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());



    //        //dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];



    //        if (dscat != null)
    //        {

    //            if (HttpContext.Current.Request.QueryString["pcr"] != null)
    //                _pcr = HttpContext.Current.Request.QueryString["pcr"];
    //            if (HttpContext.Current.Request.QueryString["type"] == null || HttpContext.Current.Request.QueryString["type"] == "")
    //            {
    //                //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
    //                string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
    //                if (tempstr != null && tempstr != "")
    //                    category_nameh = tempstr;
    //            }
    //            else
    //                category_nameh = HttpContext.Current.Request.QueryString["value"].ToString();

    //            DataRow drpagect = dscat.Tables[0].Rows[0];
    //            iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);

    //            if (iPageNo > iTotalPages)
    //            {
    //                iPageNo = iTotalPages;
    //                //ps.PAGE_NO = iPageNo;
    //            }

    //            DataRow drproductsct = dscat.Tables[1].Rows[0];
    //            iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
    //            if (_cid != "")
    //                _ParentCatID = GetParentCatID(_cid);

    //            if ((iTotalPages <= 0 || iTotalProducts <= 0) && WesNewsCategoryId == _ParentCatID)
    //            {


    //                _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);
    //                _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "advmain");

    //                // SetEbookAndPDFLink(_stmpl_container);
    //                DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
    //                if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
    //                    _stmpl_container.SetAttribute("TBT_ADV_URL_LINK", tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString());

    //                _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);
    //                return _stmpl_container.ToString();
    //            }



    //            TBWDataList[] lstrecords = new TBWDataList[0];
    //            TBWDataList[] lstrows = new TBWDataList[0];
    //            _stg_records = new StringTemplateGroup("CategoryProductList", stemplatepath);


    //            int ictrecords = 0;
    //            int icolstart = 0;
    //            string trmpstr = string.Empty;
    //            int icol = 1;
    //            lstrows = new TBWDataList[icol];

    //            if (_ViewType == "GV")
    //            {
    //                icol = 4;
    //                lstrows = new TBWDataList[icol];
    //            }
    //            else
    //            {
    //                icol = 1;
    //                lstrows = new TBWDataList[icol];
    //            }

    //            //if (dscat.Tables[0].Rows.Count < icol)
    //            //{
    //            //    icol = dscat.Tables[0].Rows.Count;
    //            //}
    //            string soddevenrow = "odd";

    //            DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1");


    //            //string userid = "";
    //            string PriceTable = string.Empty;
    //            int pricecode = 0;
    //            string tmpProds = string.Empty;
    //            DataSet dsBgDisc = new DataSet();
    //            DataSet dsPriceTableAll = new DataSet();
    //            //if (HttpContext.Current.Session["USER_ID"] != null)
    //            //    userid = HttpContext.Current.Session["USER_ID"].ToString();
    //            //if (userid == "")
    //            //    userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
    //            string userid = "";
    //            if (HttpContext.Current.Session["USER_ID"] != null)
    //            {
    //                userid = HttpContext.Current.Session["USER_ID"].ToString();
    //            }
    //            //if (userid == "")
    //            //{
    //            //    userid = objHelperServices.CheckCredential();
    //            //}

    //            if (userid == "")
    //            {
    //                userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
    //            }
    //            string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
    //            if (Convert.ToInt32(userid) > 0)
    //            {

    //                dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
    //            }
    //            else
    //            {
    //                dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
    //            }
    //            pricecode = objHelperDB.GetPriceCode(userid);

    //            //if (HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
    //            //    iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());

    //            tmpProds = "";
    //            if (Convert.ToInt32(userid) > 0)
    //            {
    //                foreach (DataRow drpid in drprodcoll)
    //                {
    //                    tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
    //                    tmpProds = tmpProds.Replace(",,", ",");
    //                }
    //                if (tmpProds != "")
    //                {
    //                    tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
    //                    dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
    //                }
    //            }
    //            string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));

    //            lstrecords = new TBWDataList[drprodcoll.Length + 1];

    //            foreach (DataRow drpid in drprodcoll)
    //            {
    //                oe++;
    //                if ((oe % 2) == 0)
    //                {
    //                    soddevenrow = "even";
    //                }
    //                else
    //                {
    //                    soddevenrow = "odd";
    //                }

    //                if (_ViewType == "GV")
    //                    _stmpl_records = _stg_records.GetInstanceOf("CategoryProductList" + "\\" + "productlist_GridView");
    //                else
    //                    _stmpl_records = _stg_records.GetInstanceOf("CategoryProductList" + "\\" + "productlist_WES" + soddevenrow);

    //                string urlDesc = "";

    //                _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());
    //                _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"].ToString());
    //                _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"].ToString());

    //                _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

    //                _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));

    //                _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
    //                _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());
    //                trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
    //                if (_ViewType == "GV")
    //                {

    //                    if (trmpstr.Length > 60)
    //                        trmpstr = trmpstr.Substring(0, 60) + "...";
    //                }

    //                _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);
    //                _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", trmpstr.Replace('"', ' '));
    //                _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
    //                _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());

    //                _stmpl_records.SetAttribute("TBT_ECOMENABLED", objHelperServices.GetIsEcomEnabled(userid));
    //                //if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
    //                //    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
    //                ////  string ValueFortag = string.Empty;
    //                ////ValueFortag = "<div id=\"pid" + dscat.Tables[2].Rows[0]["PRODUCT_ID"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
    //                //else
    //                //    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

    //                if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
    //                {
    //                    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
    //                    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
    //                    _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());
    //                    //modified by :indu
    //                    string newurl = string.Empty;
    //                    newurl = _BCEAPath + "////" +
    //                                         drpid["FAMILY_ID"].ToString()
    //                                         +"="+_stmpl_records.GetAttribute("FAMILY_NAME")
    //                                         ;

    //                    objHelperServices.Cons_NewURl(_stmpl_records, newurl, "fl.aspx", true, "");
    //                }
    //                else
    //                {
    //                    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
    //                    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);



    //                    if (dsBgDisc != null)
    //                    {
    //                        if (dsBgDisc.Tables[0].Rows.Count > 0)
    //                        {
    //                            decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
    //                            DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
    //                            string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
    //                            //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
    //                            bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
    //                            if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
    //                            {
    //                                //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

    //                            }
    //                        }
    //                    }
    //                    if (Convert.ToInt32(userid) > 0)
    //                    {

    //                        PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid),  drpid["PROD_STOCK_FLAG"].ToString(),drpid["ETA"].ToString(), dsPriceTableAll);
    //                    }
    //                    _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);


    //                    // _stmpl_records.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) < 4 ? true : false));

    //                    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
    //                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
    //                    else
    //                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

    //                    DataRow[] drow = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND ATTRIBUTE_TYPE=0");
    //                    if (drow.Length > 0) // Data Rows must return 1 row 
    //                    {
    //                        DataTable td = drow.CopyToDataTable();
    //                        _stmpl_records.SetAttribute("TBT_USER_PRICE", td.Rows[0]["NUMERIC_VALUE"].ToString());

    //                        if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) != -1)
    //                        {
    //                            if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) > 0)
    //                            {
    //                                _stmpl_records.SetAttribute("TBT_QTY_AVAIL", td.Rows[0]["QTY_AVAIL"].ToString());
    //                                _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", td.Rows[0]["MIN_ORD_QTY"].ToString());
    //                            }
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("TB_DISPLAY", "none");
    //                            }
    //                            //_stmpl_records.SetAttribute("TBT_PRODUCT_ID", td.Rows[0]["PRODUCT_ID"].ToString());
    //                        }
    //                    }



    //                }
    //                dsprodspecs = new DataSet();
    //                DataRow[] drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=1 OR ATTRIBUTE_TYPE=4 OR ATTRIBUTE_TYPE=3) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
    //                if (drow1.Length > 0)
    //                    dsprodspecs.Tables.Add(drow1.CopyToDataTable());
    //                else
    //                    dsprodspecs = null;



    //                if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
    //                {
    //                    //Indu start
    //                   // foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
    //                    int cnt=dsprodspecs.Tables[0].Rows.Count ;
    //                    for (int i = 0; i < cnt;i++ )
    //                    {

    //                        if (dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString() == "1")
    //                        {
    //                            if (dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString() == "1")
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString(), 16, true));
    //                            else
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString());
    //                        }
    //                        else if (dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString() == "4")
    //                        {
    //                            if (dsprodspecs.Tables[0].Rows[i]["NUMERIC_VALUE"].ToString().Length > 0)
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), Math.Round(Convert.ToDecimal(dsprodspecs.Tables[0].Rows[i]["NUMERIC_VALUE"]), 2).ToString());
    //                        }
    //                        else if (dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString() == "3")
    //                        {
    //                            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString().Replace("/", "\\"));
    //                            if (Fil.Exists)
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString().Replace("\\", "/"));
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
    //                            }

    //                        }
    //                        else
    //                        {
    //                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString());
    //                        }
    //                        if (_stmpl_records.GetAttribute("TBT_SUB_FAMILY").ToString() == "False")
    //                        {
    //                            string newurl = string.Empty;

    //                            newurl = _BCEAPath + "////" + drpid["FAMILY_ID"].ToString() + "=" +
    //                                         _stmpl_records.GetAttribute("FAMILY_NAME") + "////" +
    //                                         drpid["PRODUCT_ID"].ToString() + "=" + dsprodspecs.Tables[0].Rows[i]["PRODUCT_CODE"].ToString() 
    //                                         ;
    //                            //Added by:Indu

    //                            _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString());
    //                            objHelperServices.Cons_NewURl(_stmpl_records, newurl, "pd.aspx", true, dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString());
    //                        }
    //                    }

    //                }
    //                dsprodspecs = new DataSet();
    //                drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=7 OR ATTRIBUTE_TYPE=9) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
    //                if (drow1.Length > 0)
    //                    dsprodspecs.Tables.Add(drow1.CopyToDataTable());
    //                else
    //                    dsprodspecs = null;

    //                if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
    //                {
    //                    string desc = "";
    //                    string descattr = "";
    //                    //foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
    //                    //Indu Start
    //                    int cnt = dsprodspecs.Tables[0].Rows.Count;
    //                    for (int i = 0; i < cnt;i++ )
    //                    {

    //                        if (dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString() == "9")
    //                        {
    //                            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString());
    //                            if (Fil.Exists)
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString().Replace("\\", "/"));
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
    //                            }

    //                        }
    //                        else if (dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_TYPE"].ToString() == "7")
    //                        {
    //                            //string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
    //                            desc = dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
    //                            descattr = descattr +" "+ desc;
    //                            if (desc.Length > 230 && _ViewType == "LV")
    //                            {
    //                                _stmpl_records.SetAttribute("TBT_MORE_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), true);
    //                                desc = desc.Substring(0, 230).ToString();
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), desc);
    //                            }
    //                            //else if (desc.Length > 30 && _ViewType == "GV")
    //                            //{
    //                            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
    //                            //    desc = desc.Substring(0, 30).ToString();
    //                            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                            //}
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), desc);
    //                                _stmpl_records.SetAttribute("TBT_MORE_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), false);
    //                            }
    //                        }
    //                        else
    //                        {

    //                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dsprodspecs.Tables[0].Rows[i]["ATTRIBUTE_ID"].ToString(), dsprodspecs.Tables[0].Rows[i]["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>"));
    //                        }

    //                    } 
    //                   // 86
    //                    if (descattr.Length > 86 && _ViewType == "GV")
    //                    {
    //                        _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
    //                        descattr = descattr.Substring(0, 86).ToString();
    //                        descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
    //                        _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
    //                    }
    //                    else
    //                        _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
    //                }



    //                lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());

                    
    //                icolstart++;
    //                if (icolstart >= icol || oe == drprodcoll.Length)
    //                {
    //                    ////_stg_container = new StringTemplateGroup("CategoryProductListcontainer", stemplatepath);
    //                    ////_stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "producttable_" + soddevenrow);
    //                    ////_stmpl_container.SetAttribute("TBWDataList", lstrows);
    //                    ////lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
    //                    ////sHTML = sHTML + _stmpl_container.ToString();
    //                    ////ictrecords++;
    //                    ////icolstart = 0;
    //                    ////lstrows = new TBWDataList[icol];
    //                    if (icolstart == icol || iTotalProducts < 24)
    //                    {
    //                        _stg_container = new StringTemplateGroup("CategoryProductListcontainer", stemplatepath);
    //                        _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "producttable_" + soddevenrow);
    //                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
    //                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
    //                        sHTML = sHTML + _stmpl_container.ToString();

    //                    }
    //                    if (icolstart == 3 || icolstart == 2 || icolstart == 1)
    //                    {
    //                        HiddenField balrecords_cnt = new HiddenField();
    //                        balrecords_cnt.Value = icolstart.ToString();
    //                       // balrecords_cnt.Value = icolstart.ToString();
    //                        //currpage.Value = iPageNo.ToString();
    //                    }

                       

    //                    ictrecords++;
    //                    icolstart = 0;
    //                    lstrows = new TBWDataList[icol];

    //                }
    //            }

    //            //   }

    //            _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);

    //            if (HttpContext.Current.Request.QueryString["ViewMode"] != null)
    //            {
    //                PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
    //                isreadonly.SetValue(HttpContext.Current.Request.QueryString, false, null);
    //                HttpContext.Current.Request.QueryString.Remove("ViewMode");
    //            }

    //            string productlistURL = HttpContext.Current.Request.QueryString.ToString();
    //            string powersearchURListView = string.Empty;
    //            string powersearchURLGridView = string.Empty;
    //            powersearchURListView = productlistURL + "&ViewMode=LV";
    //            powersearchURLGridView = productlistURL + "&ViewMode=GV";


               
               

              

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        sHTML = ex.Message;
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //    }
    //    finally
    //    {

    //    }


    //    return objHelperServices.StripWhitespace(sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
    //}
    public string DynamicPag_RenderHTMLjson(string url, int ipageno, string eapath, string BCEAPath, string ViewMode, string irecords, string balrecords)
    {
      
        
        stemplatepath = HttpContext.Current.Server.MapPath("Templates");
        //string package = "CATEGORYPRODUCTLIST";
        string SkinRootPath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
        FamilyServices objFamilyServices = new FamilyServices();
    // StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
      //  StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
       // StringTemplate _stmpl_pages = null;
        DataSet dsprod = new DataSet();
        DataSet dsprodspecs = new DataSet();
        DataSet dscat = new DataSet();
        string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
        bool BindToST = true;
        //int oe = 0;
        string category_nameh = string.Empty;
        string sHTML = string.Empty;
        string _pcr = string.Empty;
        string _ViewType = string.Empty;
        string _BCEAPath = string.Empty;
        //string catID = "";



    
        try
        {


            //if (balrecords == "3" || balrecords == "2" || balrecords == "1")
            //{
            //    irecords = "20";
            //    balrecords = "0";
            //}
            //else
            //{
            //    irecords = "18";
            //    balrecords = "0";
            //}
          


            dscat = Get_Value_Breadcrum(ipageno, eapath, irecords);

        


            if (HttpContext.Current.Request.QueryString["tsm"] != null && HttpContext.Current.Request.QueryString["tsm"].ToString() != "")
                _tsm = HttpContext.Current.Request.QueryString["tsm"];

            if (HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request.QueryString["tsb"].ToString() != "")
                _tsb = HttpContext.Current.Request.QueryString["tsb"];

            if (HttpContext.Current.Request.QueryString["type"] != null)
                _type = HttpContext.Current.Request.QueryString["type"];

            if (HttpContext.Current.Request.QueryString["value"] != null)
                _value = HttpContext.Current.Request.QueryString["value"];

            if (HttpContext.Current.Request.QueryString["bname"] != null)
                _bname = HttpContext.Current.Request.QueryString["bname"];
            if (HttpContext.Current.Request.QueryString["searchstr"] != null)
                _searchstr = HttpContext.Current.Request.QueryString["searchstr"];
            if (HttpContext.Current.Request.QueryString["srctext"] != null)
                _searchstr = HttpContext.Current.Request.QueryString["srctext"];

            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _cid = HttpContext.Current.Request.QueryString["cid"];

            if (HttpContext.Current.Request.QueryString["pcr"] != null)
                _pcr = HttpContext.Current.Request.QueryString["pcr"];



            _ViewType = ViewMode;
            _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();


            _BCEAPath = BCEAPath.ToString().Replace("Category=", "").Replace("CATEGORY=", "");


            if (dscat != null)
            {

                if (HttpContext.Current.Request.QueryString["pcr"] != null)
                    _pcr = HttpContext.Current.Request.QueryString["pcr"];
                if (HttpContext.Current.Request.QueryString["type"] == null || HttpContext.Current.Request.QueryString["type"] == string.Empty)
                {
                    //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                    string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
                    if (tempstr != null && tempstr != "")
                        category_nameh = tempstr;
                }
                else
                    category_nameh = HttpContext.Current.Request.QueryString["value"].ToString();

                DataRow drpagect = dscat.Tables[0].Rows[0];
                iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);

                if (iPageNo > iTotalPages)
                {
                    iPageNo = iTotalPages;
                    //ps.PAGE_NO = iPageNo;
                }

                DataRow drproductsct = dscat.Tables[1].Rows[0];
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
                if (_cid != "")
                    _ParentCatID = GetParentCatID(_cid);

              


                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];
                _stg_records = new StringTemplateGroup("CategoryProductList", stemplatepath);

                string TBWDataListnew = string.Empty;
                //int ictrecords = 0;
                //int icolstart = 0;
                string trmpstr = "";
                //int icol = 1;
                lstrows = new TBWDataList[24];

             

          
                string PriceTable = string.Empty;
                //int pricecode = 0;
              //  string tmpProds = string.Empty;
                DataSet dsBgDisc = new DataSet();
                DataSet dsPriceTableAll = new DataSet();
             
                string userid = string.Empty;
                if (HttpContext.Current.Session["USER_ID"] != null)
                {
                    userid = HttpContext.Current.Session["USER_ID"].ToString();
                }
               

                if (userid == "")
                {
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
                }
               
                //pricecode = objHelperDB.GetPriceCode(userid);

                //tmpProds = "";
                //if (Convert.ToInt32(userid) > 0)
                //{
                //    foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                //    {
                //        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                //        tmpProds = tmpProds.Replace(",,", ",");
                //    }
                //    if (tmpProds != "")
                //    {
                //        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                //        dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                //    }
                //}

                //string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count  + 1];
                //bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);
                foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                {



                    _stmpl_records = _stg_records.GetInstanceOf("CategoryProductList" + "\\" + "productlist_GridView");


          //          _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());
                    _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"].ToString());
                    _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"].ToString());

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));

                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());
                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                    if (_ViewType == "GV")
                    {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                    }

                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);
                    //_stmpl_records.SetAttribute("TBT_FAMILY_TITLE", trmpstr.Replace('"', ' '));
                    //_stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
                    //_stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());

                    //_stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomEnabled);

                    string[] catpath = null;
                    string categorypath = string.Empty;
                    //if (drpid["CATEGORY_PATH"].ToString() != null)
                    //{

                    //    catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                    //    categorypath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ");
                    //}
                    string parentcategoryname = string.Empty;
                    try
                    {
                        string[] getcatname = eapath.ToString().Replace("AllProducts////WESAUSTRALASIA////", "").Split(new string[] { "////" }, StringSplitOptions.None);
                        if (getcatname.Length > 0)
                        {
                            parentcategoryname = getcatname[0] + "////" + getcatname[1];
                        }
                    }
                    catch (Exception ex)
                    {
                        objErrorHandler.CreateLog(ex.ToString());
                    }
                    if (drpid["CATEGORY_PATH"].ToString().StartsWith(parentcategoryname) || parentcategoryname ==string.Empty)
                    {
                        //objErrorHandler.CreateLog("WesNewsCategoryId is equal");

                        if (drpid["CATEGORY_PATH"].ToString() != null)
                        {

                            catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                            //objErrorHandler.CreateLog(catpath[0] + "  " + category_nameh);
                            //if (catpath.Length >= 3)
                            //{
                            //    categorypath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ");
                            //}
                            //else
                            //{
                                categorypath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ");

                            //}
                        }
                    }
                    else
                    {
                        //objErrorHandler.CreateLog("Inside else WesNewsCategoryId is not equal" + drpid["FAMILY_ID"].ToString());
                        HelperDB objhelperDb = new HelperDB();
                        DataTable Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, drpid["FAMILY_ID"].ToString(), "", "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                      //  DataTable Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, drpid["FAMILY_ID"].ToString(), _cid, "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                        if ((Sqltb != null) && (HttpContext.Current.Session["EA"] != null))
                        {
                            //objErrorHandler.CreateLog(Sqltb.Rows[0]["CATEGORY_PATH"].ToString() + "getfamily_category inside easyask");
                            DataRow[] dr = Sqltb.Select("category_path like '" + parentcategoryname + "%'");
                            if (dr.Length > 0)
                            {

                                //objErrorHandler.CreateLog(dr.Length.ToString() + "getfamily_category inside dr");
                                string catpath1 = dr.CopyToDataTable().Rows[0]["CATEGORY_PATH"].ToString(); ;
                                catpath = catpath1.Split(new string[] { "////" }, StringSplitOptions.None);

                            //    objErrorHandler.CreateLog(catpath[0] + "  " + category_nameh);
                            //string catpath1 = Sqltb.Rows[0]["CATEGORY_PATH"].ToString();
                            //catpath = catpath1.Split(new string[] { "////" }, StringSplitOptions.None);
                            //if (catpath.Length >= 3)
                            //{
                            //    categorypath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ");
                            //}
                            //else
                            //{
                                categorypath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ");

                            //}

                            }


                        }
                        else
                        {

                            //objErrorHandler.CreateLog("else no record");
                        }
                    }
                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
             //           _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());
                        //modified by :indu
                        //string newurl = string.Empty;
                        //newurl = WAG_Root_Path + "////" + drpid["FAMILY_ID"] + "////" + categorypath + "////" +
                        //                  drpid["FAMILY_NAME"];


                        objHelperServices.SimpleURL(_stmpl_records, WAG_Root_Path + "////" + drpid["FAMILY_ID"] + "////" + categorypath + "////" +drpid["FAMILY_NAME"], "fl.aspx");
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
           //             _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);




                        //if (Convert.ToInt32(userid) > 0)
                        //{

                        //    PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
                        //}
                        //_stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);



                        //if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                        //    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                        //else
                        //    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);



                        _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"].ToString());

                        if (drpid["QTY_AVAIL"].ToString() != string.Empty)
                        {
                            if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                            {
                                if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                                {
                                    _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"].ToString());
                                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                }

                            }
                        }



                        //string newurl = string.Empty;


                        //newurl = WAG_Root_Path + "////" + drpid["FAMILY_ID"] + "////" + drpid["PRODUCT_ID"] + "=" + drpid["PRODUCT_CODE"] + "////" +
                        //    categorypath + "////" +
                        //                   drpid["FAMILY_NAME"];

                        objHelperServices.SimpleURL(_stmpl_records, WAG_Root_Path + "////" + drpid["FAMILY_ID"] + "////" + drpid["PRODUCT_ID"] + "=" + drpid["PRODUCT_CODE"] + "////" + categorypath + "////" + drpid["FAMILY_NAME"], "pd.aspx");



                    }

                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"].ToString());

                    
                    //if (drpid["Prod_Thumbnail"].ToString().Contains("noimage.gif"))
                    //{
                    //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", "/images/noimage.gif");
                    //}
                    //else
                    //{
                    //    System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                    //    if (Fil.Exists)
                    //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", drpid["Prod_Thumbnail"].ToString());
                    //    else
                    //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", "/images/noimage.gif");
                    //}



                    if (drpid["Prod_Thumbnail"].ToString().Contains("noimage.gif"))
                    {
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", strprodimg_sepdomain+"images/noimage.gif");
                    }
                    else
                    {
                        //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                        //if (Fil.Exists)
                        //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", strprodimg_sepdomain+drpid["Prod_Thumbnail"].ToString());
                        //else
                        //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", strprodimg_sepdomain+"/images/noimage.gif");

                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", strprodimg_sepdomain + drpid["Prod_Thumbnail"].ToString());
                    }
                    string stk_sta_desc = "";
                    stk_sta_desc = drpid["STOCK_STATUS_DESC"].ToString().Trim();
                  //  objErrorHandler.CreateLog(stk_sta_desc + "ct");
                 
                    //if ((stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE") || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA")
                    //    _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                    //else
                    //    _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                    //&& drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2"
                    BindToST = true;

                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
                    }
                    else
                    {
                        if ((stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA"))
                        {
                            if (stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE")
                            {
                                _stmpl_records.SetAttribute("PRODUCT_STATUS", "Product No Longer Available.</br> Please contact Us");
                                BindToST = false;
                                //objErrorHandler.CreateLog(drpid["PRODUCT_ID"].ToString() + "Product No Longer Available" + "Familypage"); 
                            }
                            else if (stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA")
                            {
                                _stmpl_records.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable.<br/>Please Contact Us for more details");
                            }
                            if (drpid["PROD_SUBSTITUTE"].ToString().Trim() != "" && drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
                            {
                                // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid));
                                DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PRODUCT_CODE"].ToString(), Convert.ToInt32(userid), "pd");
                                if (rtntbl != null && rtntbl.Rows.Count > 0)
                                {

                              //      bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                //    bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

                                    if ((bool)rtntbl.Rows[0]["samecodenotFound"] == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                    {
                                        //if sub product is discontinued
                                        if (stk_sta_desc.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
                                        {

                                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("", rtntbl.Rows[0]["SubstuyutePid"].ToString(), userid);
                                            if (Sqltbs != null)
                                            {

                                                string stockstaus = Sqltbs.Tables[0].Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().ToUpper();

                                                if ((stockstaus == "DISCONTINUED NO LONGER AVAILABLE"))
                                                {

                                                    BindToST = false;
                                                }
                                                else
                                                {
                                                    BindToST = true;
                                                }
                                            }
                                        }
                                        _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                                        _stmpl_records.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                           //             string strurl = rtntbl.Rows[0]["ea_path"].ToString();
                                        _stmpl_records.SetAttribute("TBT_REP_EA_PATH", rtntbl.Rows[0]["ea_path"].ToString());
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);

                                    }
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                                }
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);

                            }

                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                            if (stk_sta_desc.ToUpper().Contains("OUT_OF_STOCK") == true && drpid["PROD_SUBSTITUTE"].ToString().Trim() != "" && drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
                            {
                                // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid));
                                DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PRODUCT_CODE"].ToString(), Convert.ToInt32(userid), "pd");
                                if (rtntbl != null && rtntbl.Rows.Count > 0)
                                {

                  //                  bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                  //                  bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                                    //objErrorHandler.CreateLog("OUT_OF_STOCK" + samecodenotFound + "--" + rtntbl.Rows[0]["ea_path"].ToString());
                                    if ((bool)rtntbl.Rows[0]["samecodenotFound"] == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                    {

                                        _stmpl_records.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                         //               string strurl = rtntbl.Rows[0]["ea_path"].ToString();
                                        _stmpl_records.SetAttribute("TBT_REP_EA_PATH", rtntbl.Rows[0]["ea_path"].ToString());
                                    }
                                }

                            }
                        }
                    }
                    string descattr = "";


                    if (drpid["PRODUCT_COUNT"].ToString() != "1")
                    {
                        descattr = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        if (descattr.Length > 0)
                            descattr = descattr + " ";
                            //descattr = descattr + "<br/>";
                        descattr = descattr + " " + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                    }
                    else
                        descattr = drpid["Prod_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");


                    if (descattr.Length > 120) // && _ViewType == "GV"
                    {


                        descattr = descattr.Substring(0, 120).ToString();
                        descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                        _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr + "...");

                    }
                    else
                    {


                        _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    }

                    if (BindToST == true)
                    {
                        TBWDataListnew = TBWDataListnew + _stmpl_records.ToString();
                    }
                }
               // TBWDataListnew = TBWDataListnew + "<div class='clearfix text-center'></div>";
            //    TBWDataListnew = TBWDataListnew;

            //         if (_ViewType == "LV")
            //{
            //    TBWDataListnew = TBWDataListnew.Replace(".home-grid", ".home-grid list-wrapper");
            //    TBWDataListnew = TBWDataListnew.Replace("dpynone", "pro_discrip");
            //}
            
                return TBWDataListnew.ToString();
                     

            }
        }
        catch (Exception ex)
        {
            sHTML = ex.Message;
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        finally
        {

        }

       

        return objHelperServices.StripWhitespace(sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
    }

    protected DataSet Get_Value_Breadcrum(int ipageno, string eapath, string irecords)
    {


        string sHTML = string.Empty;
       // string sBrandAndModelHTML = "";
       // string sModelListHTML = "";
        DataSet dscat = new DataSet();
        EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
        try
        {


      //      string _catCid = string.Empty;
            string _parentCatID = string.Empty;
           // int ictrows = 0;
            string _tsb = string.Empty;
            string _tsm = string.Empty;
            string _type = string.Empty;
            string _value = string.Empty;
            string _bname = string.Empty;
            string _searchstr = string.Empty;
            string url = string.Empty;
            //string _byp = "2";
            string _bypcat = null;
            string _pid = string.Empty;
            string _fid = string.Empty;
            string _seeall = string.Empty;
            _bypcat = HttpContext.Current.Request.QueryString["bypcat"];

            if (HttpContext.Current.Request.QueryString["tsm"] != null && HttpContext.Current.Request.QueryString["tsm"].ToString() != "")
                _tsm = HttpContext.Current.Request.QueryString["tsm"];

            if (HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request.QueryString["tsb"].ToString() != "")
                _tsb = HttpContext.Current.Request.QueryString["tsb"];

            if (HttpContext.Current.Request.QueryString["type"] != null)
                _type = HttpContext.Current.Request.QueryString["type"];

            if (HttpContext.Current.Request.QueryString["value"] != null)
                _value = HttpContext.Current.Request.QueryString["value"];

            if (HttpContext.Current.Request.QueryString["bname"] != null)
                _bname = HttpContext.Current.Request.QueryString["bname"];
            if (HttpContext.Current.Request.QueryString["searchstr"] != null)
                _searchstr = HttpContext.Current.Request.QueryString["searchstr"];
            if (HttpContext.Current.Request.QueryString["srctext"] != null)
                _searchstr = HttpContext.Current.Request.QueryString["srctext"];

            if (HttpContext.Current.Request.QueryString["fid"] != null)
                _fid = HttpContext.Current.Request.QueryString["fid"];
            if (HttpContext.Current.Request.QueryString["pid"] != null)
                _pid = HttpContext.Current.Request.QueryString["pid"];

            if (HttpContext.Current.Request.QueryString["seeall"] != null)
                _seeall = HttpContext.Current.Request.QueryString["seeall"];
            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _cid = HttpContext.Current.Request.QueryString["cid"];


            //if (_catCid != string.Empty)
            //    _parentCatID = GetParentCatID(_catCid);
            string CatName = string.Empty;
            url = HttpContext.Current.Request.Url.OriginalString.ToLower();

            string EA = string.Empty;
            EA = eapath;

            //if (HttpContext.Current.Session["MainCategory"] != null)
            //{
            //    DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
            //    if (dr.Length > 0)
            //        _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
            //}


            iPageNo = ipageno;
            string[] ConsURL = eapath.Replace("AttribSelect=", "").Split(new string[] { "////" }, StringSplitOptions.None);

            _value = ConsURL[ConsURL.Length - 1].Replace('_', ' ').Replace("`/`", "/");
            CatName = _value;
            //if (_value.Contains("="))
            //{
            //    string[] Gettype = _value.Split(new string[] { "=" }, StringSplitOptions.None);
            //    //  string AttribSelect = string.Empty;
            //    _cid = Gettype[0];
            //    CatName = Gettype[1].Replace('_', ' ').Replace("||", "+").Replace("``", "\"");
            //    // AttribSelect = "AttribSelect=" + _type + "='" + _value + "'";
            //}

            //else
            //{
            //    _type = "Category";

            //}
            if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx")))
            {
                if (_bypcat == null)
                {

                   // EasyAsk.GetMainMenuClickDetail(_cid, "");


                   
                  //  DataTable tmptbl = null;
                    //if (HttpContext.Current.Session["MainMenuClick"] != null)
                    //{
                    //    tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0];

                    //    tmptbl = tmptbl.Select("CATEGORY_ID='" + _cid + "'").CopyToDataTable();

                    //    if (tmptbl != null && tmptbl.Rows.Count > 0)
                    //    {
                    //        CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
                    //    }


                    //}


                    //if (HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                    //    iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());

                    dscat = EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname, irecords, (iPageNo - 1).ToString(), "Next", EA);

                }
                //else if (_tsb != "")
                //{
                //    string parentCatName = GetCName(_cid);
                //    dscat = EasyAsk.GetWESModel(parentCatName, iCatalogId, _tsb);
                //}

            }

            //end
        }

        catch (Exception ex)
        {
            sHTML = ex.Message;
        }




        return dscat;
    }
}
