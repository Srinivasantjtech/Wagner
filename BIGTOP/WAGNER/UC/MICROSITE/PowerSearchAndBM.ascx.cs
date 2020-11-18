using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat;
using System.Reflection;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Linq;
public partial class PowerSearchAndBM : System.Web.UI.UserControl
{
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    ProductServices objProductServices = new ProductServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    Security objSecurity = new Security();
    HelperServices objhelper = new HelperServices();
    UserServices objUserServices = new UserServices();
    //SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString());
    public string WAG_Root_Path = System.Configuration.ConfigurationManager.AppSettings["EA_ROOT_CATEGORY_PATH"].ToString();
    DataSet dscat = new DataSet();
    ConnectionDB objConnectionDB = new ConnectionDB();
    int iCatalogId;
    int iInventoryLevelCheck;
    int iRecordsPerPage = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["iRecordsPerPage"].ToString());
    bool bIsStartOver = true;
    string sSortBy = "";
    bool bDoPaging;
    int iPageNo = 1;
    bool bSortAsc = true;
    int iTotalPages = 0;
    int iTotalProducts = 0;
    int iTmpProductId = 0;
    int iPrevPgNo = 1;
    int iNextPgNo = 1;
    string stemplatepath = "";
    //string catID = "";
    //string catname = "";
    string _SearchString = "";
    string _tsb = "";
    string _tsm = "";
    string _type = "";
    string _value = "";
    string _bname = "";
    string _searchstr = "";
    string _byp = "2";
    string _bypcat = null;
    string _pid = "";
    string _fid = "";
    string _cid = "";
   // string _pcr = "";
    string _EAPath = "";
    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //added string template path M/A

            stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
            try
            {
                //if (Request.Url.OriginalString.ToString().ToUpper().Contains("PL.ASPX"))
                //    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\product_list\\";
                //if (Request.Url.OriginalString.ToString().ToUpper().Contains("BB.ASPX"))
                //    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";
                //if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX"))
                //    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\byproduct\\";
                if (Request.QueryString["cid"] != null)
                {
                    Session["Closelos"] = Request.QueryString["cid"].ToString();
                }
            }
            catch (Exception ex)
            {
            }
            if (IsPostBack)
            {

            }
            else
            {
                if (Request.QueryString["pgno"] != null)
                {
                    iPageNo = Convert.ToInt32(Request.QueryString["pgno"]);
                }
            }
            GetStoreConfig();
            GetPageConfig();

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    private void GetStoreConfig()
    {
        if (Session["CATALOG_ID"] != null && Session["INVENTORY_LEVEL_CHECK"] != null)
        {
            iCatalogId = Convert.ToInt32(WesCatalogId); //Convert.ToInt32(Session["CATALOG_ID"].ToString());
            iInventoryLevelCheck = Convert.ToInt32(Session["INVENTORY_LEVEL_CHECK"].ToString());
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
            //bDoPaging = Convert.ToBoolean(Session["DO_PAGING"].ToString());


            if (HidItemPage.Value.ToString() == string.Empty || HidItemPage.Value.ToString() == null)
            {
                if (Request.Url.ToString().ToLower().Contains("mbb.aspx") == true)
                {

                    if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());
                }
                else
                {
                    if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
                        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());
                }
            }
            else
            {
                if (Request.Url.ToString().ToLower().Contains("mbb.aspx") == true)
                {
                    iRecordsPerPage = Convert.ToInt32(HidItemPage.Value.ToString());
                    Session["RECORDS_PER_PAGE_BYBRAND"] = HidItemPage.Value.ToString();
                }
                else
                {
                    iRecordsPerPage = Convert.ToInt32(HidItemPage.Value.ToString());
                    Session["RECORDS_PER_PAGE_POWERSEARCH"] = HidItemPage.Value.ToString();
                }
            }

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    private void SetQueryString(StringTemplate _stmpl_pages)
    {

        _stmpl_pages.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_cid));
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(_searchstr));
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_TYPE", _type);
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(_value));
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(_bname));
        _stmpl_pages.SetAttribute("TBW_CUSTOM_NUM_FIELD3", HttpUtility.UrlEncode(_byp));
        _stmpl_pages.SetAttribute("TBT_TOSUITE_BRAND", HttpUtility.UrlEncode(_tsb));
        _stmpl_pages.SetAttribute("TBT_TOSUITE_MODEL", HttpUtility.UrlEncode(_tsm));
        _stmpl_pages.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

    }


    public string ST_Top_filter(string MicroSiteTemplate)
    {
        try
        {
            HelperServices objHelperServices = new HelperServices();
            ConnectionDB objConnectionDB = new ConnectionDB();

            TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("searchrsltcategorytop", MicroSiteTemplate, objConnectionDB.ConnectionString);

            return tbwtMSEngine.ST_Top_attribute_Filter();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }
    protected string ST_ProductListJson()
    {
        string _tempfid = "";
        string category_nameh = "";
        string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();

        if (Request["srctext"] != null && Request["srctext"].ToString() == "")
        {
            return "CLEAR";
        }
        else if (Request["srctext"] != null)
        {
            _SearchString = Request["srctext"].ToString();

        }
        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        //StringTemplate _stmpl_pages = null;
        //StringTemplateGroup _stg_records1 = null;
        //string subcatname_l1 = "";
        //string subcatname_l2 = "";
     string subcatname_l1_l2 = "";
        //string catname = "";
        string catheader = "";
        //bool subcatdispflag = false;
        //bool catdispflag = false;
        int oe = 0;
        string _ViewType = "";

        string sHTML = "";

        //if (Convert.ToInt32(Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        // {
        try
        {


            if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
                _tsm = Request.QueryString["tsm"];

            if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
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

            //if (Request.QueryString["pcr"] != null)
            //    _pcr = Request.QueryString["pcr"];




            if (Session["PL_VIEW_MODE"] != null && Session["PL_VIEW_MODE"].ToString() != "")
                _ViewType = Session["PL_VIEW_MODE"].ToString();
            else if (Request.QueryString["ViewMode"] != null)
            {
                _ViewType = Request.QueryString["ViewMode"];
                Session["PL_VIEW_MODE"] = _ViewType;
            }            
            else
            {
                _ViewType = "GV";
                Session["PL_VIEW_MODE"] = _ViewType;
            }


            string _BCEAPath = string.Empty;
            if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
            {
                _BCEAPath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();


                string _newBCEAPath = string.Empty;
                string[] ConsURL = _BCEAPath.Split(new string[] { "////" }, StringSplitOptions.None);
                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 2 && ConsURL.Length == 3)
                    {
                        _newBCEAPath = "UserSearch1=" + ConsURL[i].ToString();

                    }
                    else if (i == 3)
                    {
                        _newBCEAPath = "UserSearch=" + ConsURL[i].ToString();
                    }
                    else
                    {
                        _newBCEAPath = _newBCEAPath + "////" + ConsURL[i].ToString();
                    }

                }
                _BCEAPath = _newBCEAPath;






            }

            DataSet ds = new DataSet();
            string _CategorySelect = string.Empty;
            if (HttpContext.Current.Session["BreadCrumbDS"] != null)
            {
                ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                _CategorySelect = ds.Tables[0].Rows[0]["EAPath"].ToString();
            }
            else
            {

                _CategorySelect = WAG_Root_Path + "////" + "UserSearch=" + _searchstr;
            }


            if (_EAPath == "")
                _EAPath = HttpContext.Current.Session["EA"].ToString();
            //string _BCEAPath = string.Empty;
            //if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
            //{
            //    _BCEAPath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();
            //}

            DataSet tmp = (DataSet)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTDataSet);
            if (tmp != null && tmp.Tables.Count >= 1 && tmp.Tables[0].Rows.Count >= 1)
            {

                category_nameh = tmp.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
            }
            DataSet dsfamprod = new DataSet();

            dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            //dscat = ps.getwesproducts();
            if (dscat == null || dscat.Tables["FamilyPro"].Rows.Count ==0)
            {
                return "";
            }
            
            DataRow drpagect = dscat.Tables[0].Rows[0];
            DataRow drproductsct = dscat.Tables[1].Rows[0];
            if (dsfamprod.Tables != null && dsfamprod.Tables.Count > 0 && Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "" && Request.Url.ToString().ToLower().Contains("byproduct.aspx"))
            {
                iTotalProducts = dsfamprod.Tables[0].Rows.Count;
                if (iRecordsPerPage >= iTotalProducts)
                {
                    iTotalPages = 1;
                }
                else
                {
                    double recordpg = iRecordsPerPage;
                    double totalpg = (iTotalProducts / recordpg);
                    if (totalpg > Convert.ToInt32(totalpg))
                        iTotalPages = Convert.ToInt32(totalpg) + 1;
                    else
                        iTotalPages = Convert.ToInt32(totalpg);
                }
            }
            else
            {
                iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
            }
            Session["iTotalPages"] = iTotalPages;

            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];
            _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);

            if (Request.Url.ToString().ToLower().Contains("byproduct.aspx") && _tempfid.Length > 0)
            {
                lstrecords = new TBWDataList[dsfamprod.Tables[0].Rows.Count + 1];
            }
            else
            {
                lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count + 1];
            }
            int ictrecords = 0;
            int icolstart = 0;
            int icol = 1;
            lstrows = new TBWDataList[icol];

            if (_ViewType == "GV")
            {
                icol = 4;
                lstrows = new TBWDataList[icol];
            }
            else
            {
                icol = 1;
                lstrows = new TBWDataList[icol];
            }
            //if (dscat.Tables[0].Rows.Count < icol)
            //{
            //    icol = dscat.Tables[0].Rows.Count;
            //}
            string soddevenrow = "odd";
           // DataRow[] drprodcoll = null;


            //drprodcoll = dscat.Tables["FamilyPro"].Select("", "SNO");

            string trmpstr = "";
        //    int ProductTotalcount = 0;
            int pricecode = 0;
            string userid = "";
            if (Session["USER_ID"] != null)
                userid = Session["USER_ID"].ToString();

            if (userid == "")
                // userid = "0";
                userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
            DataSet dsBgDisc = new DataSet();
            DataSet dsPriceTableAll = new DataSet();
            string tmpProds = "";
            bool BindToST = true; 
            //string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
            //if (Convert.ToInt32(userid) > 0)
            //{

            //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
            //}
            //else
            //{
            //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
            //}

            pricecode = objHelperDB.GetPriceCode(userid);

            if (dscat.Tables["FamilyPro"].Rows.Count  > 0)
            {

                tmpProds = "";
                if (Convert.ToInt32(userid) > 0)
                {
                    foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                        tmpProds = tmpProds.Replace(",,", ",");
                    }
                    if (tmpProds != "")
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                        dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                    }
                }
                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);
                foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                {
                    oe++;
                    if ((oe % 2) == 0)
                    {
                        soddevenrow = "even";
                    }
                    else
                    {
                        soddevenrow = "odd";
                    }


                    //if (_ViewType == "GV")
                        _stmpl_records = _stg_records.GetInstanceOf("PowerSearch" + "\\" + "productlist_GridView");
                    //else
                    //    _stmpl_records = _stg_records.GetInstanceOf("PowerSearch" + "\\" + "productlist_" + soddevenrow);

                    if (_ViewType == "GV")
                        _stmpl_records.SetAttribute("LISTVIEW_CLASS", "");
                    else
                        _stmpl_records.SetAttribute("LISTVIEW_CLASS", "list-group-item");

                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomEnabled);

                    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                    else
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

                    _stmpl_records.SetAttribute("PRODUCT_ID", drpid["PRODUCT_ID"].ToString());

                    //_stmpl_records.SetAttribute("MIN_ORD_QTY", oProd.GetMinOrdQty(Convert.ToInt32(drpid["PRODUCT_ID"].ToString())).ToString());
                    _stmpl_records.SetAttribute("MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());



                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                    if (_ViewType == "GV")
                    {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                    }

                    _stmpl_records.SetAttribute("CDNROOT", System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString());
                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);
                    _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", trmpstr.Replace('"', ' '));
                    _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_CODE"].ToString());
                    int _pid;
                    if (drpid["PRODUCT_ID"].ToString() != null && drpid["PRODUCT_ID"].ToString() != "")
                    {

                        _pid = System.Convert.ToInt32(drpid["PRODUCT_ID"].ToString());
                    }
                    //_stmpl_records.SetAttribute("TBT_USER_PRICE", GetMyPrice(_pid));
                    //_stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["NUMERIC_VALUE"].ToString());
                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());

                    _stmpl_records.SetAttribute("CATEGORY_ID", HttpUtility.UrlEncode(drpid["CATEGORY_ID"].ToString()));

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));



                    string PriceTable = "";




                    //if (dsBgDisc != null)
                    //{
                    //    if (dsBgDisc.Tables[0].Rows.Count > 0)
                    //    {
                    //        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                    //        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                    //        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                    //        //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                    //        bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                    //        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                    //        {
                    //            //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                    //        }
                    //    }
                    //}
                    if (Convert.ToInt32(userid) > 0)
                    {

                        PriceTable = objFamilyServices.AssemblePriceTableMS(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
                    }
                    _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);


                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_5", drpid["PRODUCT_PRICE"].ToString());

                  
                    _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"].ToString());
                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());

                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());

                    string stk_sta_desc = drpid["STOCK_STATUS_DESC"].ToString().Trim().ToUpper();
                    BindToST = true;
                    //objErrorHandler.CreateLog(stk_sta_desc + "microsite");
                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
                    }
                    else
                    {
                        if ((stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE") || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA")
                        {


                            if (_SearchString.ToUpper() != drpid["PRODUCT_CODE"].ToString().ToUpper() &&
                            drpid["PROD_SUBSTITUTE"].ToString().Trim() == "" && stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE")
                            {
                                BindToST = false;

                            }

                            _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                        }
                    }
                    string[] catpath = null;
                    string categorypath = string.Empty;
                    catpath = HttpContext.Current.Request.RawUrl.Split('/');
                    if (catpath.Length > 1)
                    {
                        categorypath =  (catpath.Length >= 2 ? catpath[1] : " ");
                    }
                 
                    if (drpid["CATEGORY_PATH"].ToString() != null)
                    {

                        catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                        categorypath = categorypath + "////" +  (catpath.Length >= 2 ? catpath[1] : " ");
                    }
                  
                    ////if (drpid["CATEGORY_PATH"].ToString() != null)
                    ////{

                    ////    catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                    ////    categorypath = catpath[0] + "////" + catpath[1];
                    ////}
                 
                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());

                        string newurl = string.Empty;
                        newurl = WAG_Root_Path + "////" + categorypath + "////" +
                                          drpid["FAMILY_NAME"] + "////" + drpid["FAMILY_ID"];


                        objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "fl.aspx");


                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);


                        string newurl = string.Empty;


                        newurl = WAG_Root_Path + "////" + categorypath + "////" +
                                           drpid["FAMILY_NAME"] + "////" +
                                         drpid["PRODUCT_ID"] + "=" + drpid["PRODUCT_CODE"] + "////"
                                         + drpid["FAMILY_ID"];
                       

                        objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "mpd.aspx");
                    }

                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"].ToString());

                    if (drpid["PRODUCT_CODE"].ToString().Length <= 8)
                    {
                        _stmpl_records.SetAttribute("TBT_ORG_ORDERNO", true);
                    }
                   

                   // System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                   // if (Fil.Exists)
                   // {
                        if (!drpid["Prod_Thumbnail"].ToString().Contains("_images_200"))
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", objhelper.SetImageFolderPath(drpid["Prod_Thumbnail"].ToString().Replace("\\", "/"), "_th", "_Images_200"));
                        }
                        else
                        {

                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", drpid["Prod_Thumbnail"].ToString().Replace("\\", "/"));
                        }
                 //  }
                  //  else
                   //     _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", "/images/noimage.gif");



                    if (Request.QueryString["byp"] != null && (Request.QueryString["byp"].ToString() == "2" || Request.QueryString["byp"].ToString() == "3"))
                    {
                        _stmpl_records.SetAttribute("BYP", Request.QueryString["byp"].ToString());
                    }



                    string desc = "";
                    string descattr = "";
                    string prod_desc_alt = "";
                    if (_ViewType == "LV")
                    {
                        desc = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        prod_desc_alt = drpid["Prod_Description"].ToString();
                        if (prod_desc_alt.Length > 0)
                        {
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                            _stmpl_records.SetAttribute("PROD_DESC_TITLE", prod_desc_alt.Replace('"', ' '));
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"].ToString());
                            _stmpl_records.SetAttribute("PROD_DESC_TITLE", drpid["FAMILY_NAME"].ToString().Replace('"', ' '));
                        }
                        if (desc.Length > 250 && _ViewType == "LV")
                        {
                            _stmpl_records.SetAttribute("TBT_MORE_62", true);
                            desc = desc.Substring(0, 230).ToString();
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_62", desc);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_62", desc);
                            _stmpl_records.SetAttribute("TBT_MORE_62", false);
                        }
                        desc = drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        if (desc.Length > 250 && _ViewType == "LV")
                        {
                            _stmpl_records.SetAttribute("TBT_MORE_4", true);
                            desc = desc.Substring(0, 230).ToString();
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_4", desc);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_4", desc);
                            _stmpl_records.SetAttribute("TBT_MORE_4", false);
                        }


                    }
                    else
                    {
                        descattr = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        if (descattr.Length > 0)
                            descattr = descattr + "<br/>";
                        descattr = descattr + " " + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        descattr = descattr + " " + drpid["Prod_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        prod_desc_alt = drpid["Prod_Description"].ToString();
                        if (prod_desc_alt.Length > 0)
                        {
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                            _stmpl_records.SetAttribute("PROD_DESC_TITLE", prod_desc_alt.Replace('"', ' '));
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"].ToString());
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"].ToString().Replace('"', ' '));
                        }
                        if (descattr.Length > 140 )
                            //&& _ViewType == "GV")
                        {
                            int count = 0;
                            count = descattr.Count(c => char.IsUpper(c));
                            int count1 = 0;
                            count1 = descattr.Count(c => char.IsSymbol(c)) + descattr.Count(c => char.IsNumber(c));
                            if (count >= 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 120).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35 && count1 > 10)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 135).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35 && count1 < 10)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 140).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);

                            }
                        }
                        else
                        {


                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                        }
                    }

                    if (BindToST == true)
                    {
                        lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                    }

                    icolstart++;

                    if (icolstart >= icol || oe == dscat.Tables["FamilyPro"].Rows.Count)
                    {
                        _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("PowerSearch" + "\\" + "producttable_" + soddevenrow);
                        _stmpl_container.SetAttribute("TBT_SUBCATNAME", subcatname_l1_l2);
                        _stmpl_container.SetAttribute("TBT_CATEGORY_NAME", catheader);
                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        ictrecords++;
                        icolstart = 0;
                        lstrows = new TBWDataList[icol];

                    }

                }

                _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);

                if (Request.QueryString["ViewMode"] != null)
                {
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    isreadonly.SetValue(this.Request.QueryString, false, null);
                    this.Request.QueryString.Remove("ViewMode");
                }
                string productlistURL = Request.QueryString.ToString();
                string powersearchURListView = "";
                string powersearchURLGridView = "";
                powersearchURListView = productlistURL + "&ViewMode=LV";
                powersearchURLGridView = productlistURL + "&ViewMode=GV";




                _stmpl_container = _stg_container.GetInstanceOf("PowerSearch" + "\\" + "main");

                if (HttpContext.Current.Session["SortOrder_mps"] != null && HttpContext.Current.Session["SortOrder_mps"] != "")
                {
                    if (HttpContext.Current.Session["SortOrder_mps"].ToString() != "SortOrder_mps")
                    {
                        HttpContext.Current.Session["SortOrder"] = null;
                    }
                }



                if (HttpContext.Current.Session["SortOrder"] != null && HttpContext.Current.Session["SortOrder"] != "")
                {

                    if ( HttpContext.Current.Session["SortOrder"].ToString() == "Latest")
                        _stmpl_container.SetAttribute("SortBy", "Latest");

                    else if ( HttpContext.Current.Session["SortOrder"].ToString() == "ltoh")
                        _stmpl_container.SetAttribute("SortBy", "Price Low To High");
                    else if ( HttpContext.Current.Session["SortOrder"].ToString() == "htol")
                        _stmpl_container.SetAttribute("SortBy", "Price High To Low");
                    else if ( HttpContext.Current.Session["SortOrder"].ToString() == "relevance")
                        _stmpl_container.SetAttribute("SortBy", "Relevance");
                    else if (HttpContext.Current.Session["SortOrder"].ToString() == "popularity")
                        _stmpl_container.SetAttribute("SortBy", "Popular");
                    else
                        _stmpl_container.SetAttribute("SortBy", "Relevance");

                    HttpContext.Current.Session["SortOrder_mps"] = "SortOrder_mop";

                }
                else
                {
                    _stmpl_container.SetAttribute("SortBy", "Relevance");
                    HttpContext.Current.Session["SortOrder_mps"] = "SortOrder_mop";
                }


                _stmpl_container.SetAttribute("TBW_PRODUCT_COUNT", iTotalProducts);
                _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh.ToUpper());

                _stmpl_container.SetAttribute("TBW_URL", powersearchURListView);
                _stmpl_container.SetAttribute("TBW_URL1", powersearchURLGridView);
                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                if (_ViewType == "LV")
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
                else
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);


                _stmpl_container.SetAttribute("ST_TOP_FILTER", ST_Top_filter(stemplatepath));

                /*

                //Added By :Indu
                //For Category and subcategory in powersearch page
                DataSet dscategory = new DataSet();
                DataSet dssubcategory = new DataSet();
                DataSet breadcrumb_ct = new DataSet();
                DataRow[] _DCRow = null;
                DataRow[] _DCRow1 = null;
                StringTemplateGroup _stg_records_container = null;
                StringTemplate _stmpl_records_tmpl = null;
                StringTemplateGroup _stg_records_container1 = null;
                StringTemplate _stmpl_records_tmpl1 = null;
                TBWDataList1[] lstrecords11 = new TBWDataList1[0];
                TBWDataList1[] lstrows11 = new TBWDataList1[0];
                TBWDataList2[] lstrecords12 = new TBWDataList2[0];
                int icolstart11 = 0;
                int ictrecords11;
                int ictrecords12;
                if (HttpContext.Current.Session["LHScatPS"] != null)
                {
                    // dscategory= (DataSet)HttpContext.Current.Session["LHSAttributes"];
                    dscategory = (DataSet)HttpContext.Current.Session["LHScatPS"];
                }
                if (HttpContext.Current.Session["LHSsubcatPS"] != null)
                {
                    dssubcategory = (DataSet)HttpContext.Current.Session["LHSsubcatPS"];
                }
                if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                {
                    breadcrumb_ct = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                }
                if (dscategory != null && dscategory.Tables.Count > 0)
                {
                    _DCRow = dscategory.Tables[0].Select();
                    if (_DCRow != null && _DCRow.Length > 0)
                    {
                        ictrecords11 = 0;
                        lstrecords11 = new TBWDataList1[_DCRow.Length + 1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("PowerSearch" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Category");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Category");
                        _stmpl_records_tmpl.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
                        lstrecords11[ictrecords11] = new TBWDataList1(_stmpl_records_tmpl.ToString());
                        ictrecords11++;
                        foreach (DataRow _drow in _DCRow)
                        {
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("PowerSearch" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["Category_Name"].ToString());
                            _stmpl_records_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                            string eapathfil = "";
                            if (breadcrumb_ct.Tables[0].Rows[0]["ItemType"].ToString().ToLower().Contains("usersearch"))
                            {
                                if (breadcrumb_ct.Tables[0].Rows.Count > 0 && breadcrumb_ct.Tables[0].Rows.Count > 1)
                                {
                                    string catvalue = "";
                                    bool chkmc = false;
                                    if (breadcrumb_ct.Tables[0].Rows.Count > 2)
                                    {
                                        for (int i = 0; i < breadcrumb_ct.Tables[0].Rows.Count; i++)
                                        {
                                            if (breadcrumb_ct.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "category" && chkmc == false)
                                            {
                                                catvalue = breadcrumb_ct.Tables[0].Rows[i]["ItemValue"].ToString();
                                                chkmc = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        catvalue = breadcrumb_ct.Tables[0].Rows[1]["ItemValue"].ToString();
                                    }
                                    if (catvalue.ToLower() == _drow["Category_Name"].ToString().ToLower())
                                    {
                                        _stmpl_records_tmpl.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
                                    }
                                }

                                if (breadcrumb_ct != null)
                                {
                                    eapathfil = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb_ct.Tables[0].Rows[0]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
                                    //if (breadcrumb_ct.Tables.Count > 0 && breadcrumb_ct.Tables[0].Rows.Count == 1)
                                    //{

                                    //    eapathfil = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb_ct.Tables[0].Rows[0]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
                                    //}
                                    //else
                                    //{
                                    //    string seseapath = HttpContext.Current.Session["EA"].ToString();
                                    //     if( seseapath != "")
                                    //         eapathfil = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(seseapath + "////" + _drow["Category_Name"].ToString()));
                                    //         else
                                    //        eapathfil = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb_ct.Tables[0].Rows[0]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
                                    //}
                                }
                            }
                            else
                            {
                                if (breadcrumb_ct.Tables[0].Rows.Count > 0 && breadcrumb_ct.Tables[0].Rows.Count > 3)
                                {
                                    string catvalue = breadcrumb_ct.Tables[0].Rows[3]["ItemValue"].ToString();
                                    if (catvalue.ToLower() == _drow["Category_Name"].ToString().ToLower())
                                    {
                                        _stmpl_records_tmpl.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
                                    }
                                }

                                if (breadcrumb_ct != null)
                                    eapathfil = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb_ct.Tables[0].Rows[0]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
                                //if (breadcrumb_ct != null)
                                //{
                                //    if (breadcrumb_ct.Tables.Count > 0 && breadcrumb_ct.Tables[0].Rows.Count > 0)
                                //    {
                                //        string seseapath = HttpContext.Current.Session["EA"].ToString();
                                //        if (seseapath != "")
                                //            eapathfil = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(seseapath + "////" + _drow["Category_Name"].ToString()));
                                //        else
                                //            eapathfil = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb_ct.Tables[0].Rows[0]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
                                //    }
                                //}
                            }
                            //  string href = "powersearch.aspx?&id=0&searchstr=" + HttpUtility.UrlEncode(_drow["SearchString"].ToString()) + "&type=" + dscategory.Tables[0].TableName.ToString() + "&bname=" + HttpUtility.UrlEncode(_bname.ToString()) + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&byp=2&Path=" + eapathfil;
                            string href = "ps.aspx?&id=0&searchstr=" + HttpUtility.UrlEncode(_drow["SearchString"].ToString()) + "&type=" + dscategory.Tables[0].TableName.ToString() + "&bname=" + HttpUtility.UrlEncode(_bname.ToString()) + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&byp=2&Path=" + eapathfil;

                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", href);

                            lstrecords11[ictrecords11] = new TBWDataList1(_stmpl_records_tmpl.ToString());
                            ictrecords11++;
                        }
                        _stmpl_container.SetAttribute("TBWDataList1", lstrecords11);
                        _stmpl_container.SetAttribute("TBW_SELECTED_ID", "1");

                    }
                }

                if (dssubcategory != null && dssubcategory.Tables.Count > 0 && dssubcategory.Tables[0].Rows.Count > 0)
                {
                    _DCRow1 = dssubcategory.Tables[0].Select();
                    if (_DCRow1 != null && _DCRow1.Length > 0)
                    {
                        ictrecords12 = 0;
                        lstrecords12 = new TBWDataList2[_DCRow1.Length + 1];
                        _stg_records1 = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container1 = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl1 = _stg_records.GetInstanceOf("PowerSearch" + "\\" + "multilistitem1");
                        _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL1", "Select Category");
                        _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL", "Select Category");
                        _stmpl_records_tmpl1.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
                        lstrecords12[ictrecords12] = new TBWDataList2(_stmpl_records_tmpl1.ToString());
                        ictrecords12++;
                        foreach (DataRow _drow in _DCRow1)
                        {
                            _stg_records1 = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container1 = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl1 = _stg_records.GetInstanceOf("PowerSearch" + "\\" + "multilistitem1");
                            _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL", _drow["Category_Name"].ToString());
                            // _stmpl_records_tmpl1.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            _stmpl_records_tmpl1.SetAttribute("TBW_SELECTED_ID", "1");
                            string eapathsubcat = "";
                            if (breadcrumb_ct.Tables[0].Rows[1]["ItemType"].ToString().ToLower().Contains("category"))
                            {
                                if (breadcrumb_ct.Tables[0].Rows.Count > 0 && breadcrumb_ct.Tables[0].Rows.Count > 2)
                                {
                                    string subcatvalue = breadcrumb_ct.Tables[0].Rows[2]["ItemValue"].ToString();
                                    if (subcatvalue.ToLower() == _drow["Category_Name"].ToString().ToLower())
                                    {
                                        _stmpl_records_tmpl1.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
                                    }
                                }
                                if (breadcrumb_ct != null)
                                    eapathsubcat = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb_ct.Tables[0].Rows[1]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
                            }
                            else
                            {
                                if (breadcrumb_ct.Tables[0].Rows.Count > 0 && breadcrumb_ct.Tables[0].Rows.Count > 3)
                                {
                                    string subcatvalue = breadcrumb_ct.Tables[0].Rows[3]["ItemValue"].ToString();
                                    if (subcatvalue == _drow["Category_Name"].ToString())
                                    {
                                        _stmpl_records_tmpl1.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
                                    }
                                }
                                if (breadcrumb_ct != null)
                                    eapathsubcat = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb_ct.Tables[0].Rows[1]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));

                            }

                            string strtype = "";
                            if (dscategory.Tables.Count > 0)
                                strtype = dscategory.Tables[0].TableName.ToString();

                            string href = "ps.aspx?&id=0&searchstr=" + HttpUtility.UrlEncode(_drow["SearchString"].ToString()) + "&type=" + strtype + "&bname=" + HttpUtility.UrlEncode(_bname.ToString()) + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&byp=2&Path=" + eapathsubcat;
                            //_stmpl_records_tmpl1.SetAttribute("href", href);

                            ////   _stmpl_records_tmpl1.SetAttribute("href", "bybrand.aspx?"+href);
                            _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL1", href);
                            lstrecords12[ictrecords12] = new TBWDataList2(_stmpl_records_tmpl1.ToString());
                            ictrecords12++;
                        }
                        _stmpl_container.SetAttribute("TBWDataList2", lstrecords12);
                        _stmpl_container.SetAttribute("TBW_SELECTED_ID", "2");

                    }
                }


                if (dscategory != null && dscategory.Tables.Count > 0)
                {
                    if (dscategory.Tables[0].Rows.Count > 0)
                    {
                        _stmpl_container.SetAttribute("TBT_SHOW_FT", true);
                        Session["MainCat"] = "true";
                    }
                    else
                    {
                        _stmpl_container.SetAttribute("TBT_SHOW_FT", false);
                        Session["MainCat"] = "false";
                    }
                }
                if (breadcrumb_ct != null)
                {
                    int i;
                    string chk = "";
                    bool flgcheck = false;
                    //for (i = 0; i < breadcrumb_ct.Tables[0].Rows.Count; i++)
                    //{
                    //    chk = breadcrumb_ct.Tables[0].Rows[i]["ItemType"].ToString();
                    //    if (chk == "Type")
                    //    {
                    //        flgcheck = true;
                    //    }
                    //}
                    if (dssubcategory != null && dssubcategory.Tables.Count > 0)
                    {
                        if (flgcheck == false && dssubcategory.Tables[0].Rows.Count > 0 && Session["subcatshow"] != null && Session["subcatshow"].ToString() == "true")
                        {
                            _stmpl_container.SetAttribute("TBT_SHOW_FTSCat", true);
                            Session["subcatshow"] = "true";
                        }
                        else
                        {
                            _stmpl_container.SetAttribute("TBT_SHOW_FTSCat", false);
                            Session["subcatshow"] = "false";
                        }
                    }
                }
                //End
              // _stmpl_container.SetAttribute("SELECT_" + iRecordsPerPage, "SELECTED=\"SELECTED\" ");


                */


                string nextstring = "";
                try
                {
                    if (Request.QueryString["srctext"].ToString() != "")
                    {
                        nextstring = "&srctext=" + Request.QueryString["srctext"].ToString().Replace("\"", "%22").Replace("&", "%26").Replace("#", "%23");

                    }
                }
                catch (Exception) { }
                //_stmpl_container.SetAttribute("TBW_START_PAGE_NO", (iPageNo * iRecordsPerPage) - (iRecordsPerPage - 1));

                //if ((iPageNo * iRecordsPerPage) > iTotalProducts)
                //    _stmpl_container.SetAttribute("TBW_END_PAGE_NO", iTotalProducts);
                //else
                //    _stmpl_container.SetAttribute("TBW_END_PAGE_NO", (iPageNo * iRecordsPerPage));

                //_stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
                //_stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo + nextstring);
                //_stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
                //_stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo + nextstring);
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");



                //if (sHTML.ToString().Contains("src=\"/prodimages\""))
                //{
                //    sHTML = sHTML.Replace("src=\"/prodimages\"", "src=\"/prodimages/images/noimage.gif\"");
                //    sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                //}
                //if (sHTML.ToString().Contains("src=\"\""))
                //{
                //    sHTML = sHTML.ToString().Replace("src=\"\"", "src=\"/prodimages/images/noimage.gif\"");
                //    sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                //}

                if (sHTML.ToString().Contains("data-original=\"/prodimages\""))
                {
                    sHTML = sHTML.Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
                    sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                }
                if (sHTML.ToString().Contains("data-original=\"\""))
                {
                    sHTML = sHTML.ToString().Replace("data-original=\"\"", "data-original=\"/prodimages/images/noimage.gif\"");
                    sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                }
                if (dscat.Tables[1].Rows[0].ItemArray[0].ToString() == "0")
                    sHTML = "";
                if (dscat.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
                {
                    sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1" + nextstring + "\">Previous</a>", "");
                    sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1" + nextstring + "\">Next</a>", "");
                }
                if (Convert.ToInt32(dscat.Tables[0].Rows[0].ItemArray[0].ToString()) == iPageNo)
                {
                    sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1" + nextstring + "\">Next</a>", "");
                }


            }
            string eapath = _EAPath.Replace("'", "###");
            string BCEAPath = _BCEAPath.Replace("'", "###");

            htmlpseapath.Value = eapath;
            htmlpsbceapath.Value = BCEAPath;
            htmlpstotalpages.Value = iTotalPages.ToString();
            htmlpsviewmode.Value = _ViewType;
            if (HttpContext.Current.Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
            {

                htmlpsirecords.Value = HttpContext.Current.Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString();
            }
            else
            {
                htmlpsirecords.Value = "24";
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
            //if (oCon != null)
            //{
            //oCon.Close();
            //}
        }
        //}

        sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
        return objHelperServices.StripWhitespace(sHTML);
    }
    //protected string ST_BrandAndModelProductListNew()
    //{
    //    string _tempfid = "";
    //    string category_nameh = "";
    //    string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
    //    //if (Session["PS_SEARCH_RESULTS"] == null)
    //    //{
    //    //    return "";
    //    //}
    //    if (Request["srctext"] != null && Request["srctext"].ToString() == "")
    //    {
    //        return "CLEAR";
    //    }
    //    else if (Request["srctext"] != null)
    //    {
    //        _SearchString = Request["srctext"].ToString();

    //    }
    //    FamilyServices objFamilyServices = new FamilyServices();
    //    StringTemplateGroup _stg_container = null;
    //    StringTemplateGroup _stg_records = null;
    //    StringTemplateGroup _stg_records1 = null;
    //    StringTemplate _stmpl_container = null;
    //    StringTemplate _stmpl_records = null;
    //    StringTemplate _stmpl_pages = null;
    //    string subcatname_l1 = "";
    //    string subcatname_l2 = "";
    //    string subcatname_l1_l2 = "";
    //    string catname = "";
    //    string catheader = "";
    //    bool subcatdispflag = false;
    //    bool catdispflag = false;
    //    int oe = 0;
    //    string _ViewType = "";

    //    string sHTML = "";
    //    string _ParentCatID = "";
    //    //if (Convert.ToInt32(Session["PS_SEARCH_RESULTS"].ToString()) > 0)
    //    // {
    //    try
    //    {


    //        if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
    //            _tsm = Request.QueryString["tsm"];

    //        if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
    //            _tsb = Request.QueryString["tsb"];

    //        if (Request.QueryString["type"] != null)
    //            _type = Request.QueryString["type"];

    //        if (Request.QueryString["value"] != null)
    //            _value = Request.QueryString["value"];

    //        if (Request.QueryString["bname"] != null)
    //            _bname = Request.QueryString["bname"];
    //        if (Request.QueryString["searchstr"] != null)
    //            _searchstr = Request.QueryString["searchstr"];
    //        if (Request.QueryString["srctext"] != null)
    //            _searchstr = Request.QueryString["srctext"];

    //        if (Request.QueryString["cid"] != null)
    //            _cid = Request.QueryString["cid"];

    //        if (Request.QueryString["pcr"] != null)
    //            _pcr = Request.QueryString["pcr"];




    //        if (Request.QueryString["ViewMode"] != null)
    //        {
    //            _ViewType = Request.QueryString["ViewMode"];
    //            Session["PL_VIEW_MODE"] = _ViewType;
    //        }
    //        else if (Session["PL_VIEW_MODE"] != null && Session["PL_VIEW_MODE"].ToString() != "")
    //            _ViewType = Session["PL_VIEW_MODE"].ToString();
    //        else
    //            _ViewType = "GV";

    //        //if (_ViewType != "")
    //        //    Session["BM_VIEW_MODE"] = _ViewType;

    //        //if (Session["BM_VIEW_MODE"] != null && Session["BM_VIEW_MODE"] != "")
    //        //    _ViewType = Session["BM_VIEW_MODE"].ToString();

    //        //if (Request.QueryString["path"] != null)
    //        //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(Request.QueryString["path"].ToString()));
    //        string _BCEAPath = string.Empty;
    //        string _realBCEAPath = string.Empty;
    //        if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
    //        {
    //            _BCEAPath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();
    //            _realBCEAPath = _BCEAPath;
    //            string _newBCEAPath = string.Empty;
    //            string _newrealBCEAPath = string.Empty;  
    //            string[] ConsURL = _BCEAPath.Split(new string[] { "////" }, StringSplitOptions.None);
    //            for (int i = 0; i <= ConsURL.Length - 1; i++)
    //            {
    //                if (i == 0)
    //                {
    //                    _newBCEAPath = ConsURL[i].ToString();
    //                  //  _newrealBCEAPath = ConsURL[i].ToString();
    //                }
    //                else if (i == 4)
    //                {
                   
    //                    _newBCEAPath = _newBCEAPath + "////" + "AttribSelect=Brand=" + ConsURL[i].ToString();
    //                   // _newrealBCEAPath = _newrealBCEAPath + "////" +  ConsURL[i].ToString();
    //                }
    //                else if (i == 5)
    //                {
    //                    //_newBCEAPath = _newBCEAPath + "////" + "AttribSelect=Model=" +HttpUtility.UrlEncode(ConsURL[i].ToString());
    //                    //_newrealBCEAPath = _newrealBCEAPath + "////" + HttpUtility.UrlEncode(ConsURL[i].ToString());
    //                    _newBCEAPath = _newBCEAPath + "////" + "AttribSelect=Model=" + ConsURL[i].ToString();
    //                    //_newrealBCEAPath = _newrealBCEAPath + "////" + ConsURL[i].ToString();
    //                } 
    //                else
    //                {
    //                    _newBCEAPath = _newBCEAPath + "////" + ConsURL[i].ToString();
    //                    //_newrealBCEAPath = _newrealBCEAPath + "////" + ConsURL[i].ToString();
    //                }

    //            }
    //            _BCEAPath = _newBCEAPath;
    //           // _realBCEAPath = _newrealBCEAPath;
    //        }
    //        if (_EAPath == "")
    //        {
    //            _EAPath = HttpContext.Current.Session["EA"].ToString();
    //        }
    //        //string _BCEAPath = string.Empty;
    //        //if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
    //        //{
    //        //    _BCEAPath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();
    //        //}




    //        if (Request.QueryString["type"] == null)
    //        {
    //            DataSet tmp = (DataSet)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTDataSet);
    //            if (tmp != null && tmp.Tables.Count >= 1 && tmp.Tables[0].Rows.Count >= 1)
    //            {

    //                category_nameh = tmp.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
    //            }
    //        }
    //        else
    //        {
    //            //category_nameh = Request.QueryString["value"].ToString();
    //            category_nameh = HttpUtility.HtmlDecode(Request.QueryString["value"].ToString());
    //        }



     
              


    //        DataSet dsfamprod = new DataSet();

    //        string Tosuit_ModelImage = "";
    //        DataSet dsModel = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
    //        if (dsModel != null)
    //        {
    //            DataRow[] dr = dsModel.Tables[0].Select("TOSUITE_MODEL='" + _tsm + "'");
    //            if (dr.Length > 0)
    //                Tosuit_ModelImage = dr.CopyToDataTable().Rows[0]["IMAGE_FILE"].ToString();

    //        }
    //        else
    //            Tosuit_ModelImage = (string)objHelperDB.GetGenericPageDataDB("", _tsb, _tsm, "GET_BYBRAND_MODEL_IMAGE", HelperDB.ReturnType.RTString);


    //        dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
    //        //dscat = ps.getwesproducts();
    //        if (dscat == null)
    //        {
    //            return "";
    //        }
    //        else if (dscat.Tables[2].Rows.Count == 0)
    //        {
    //            return "";
    //        }

    //        DataRow drpagect = dscat.Tables[0].Rows[0];
    //        DataRow drproductsct = dscat.Tables[1].Rows[0];
    //        if (dsfamprod.Tables != null && dsfamprod.Tables.Count > 0 && Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "" && Request.Url.ToString().ToLower().Contains("byproduct.aspx"))
    //        {
    //            iTotalProducts = dsfamprod.Tables[0].Rows.Count;
    //            if (iRecordsPerPage >= iTotalProducts)
    //            {
    //                iTotalPages = 1;
    //            }
    //            else
    //            {
    //                double recordpg = iRecordsPerPage;
    //                double totalpg = (iTotalProducts / recordpg);
    //                if (totalpg > Convert.ToInt32(totalpg))
    //                    iTotalPages = Convert.ToInt32(totalpg) + 1;
    //                else
    //                    iTotalPages = Convert.ToInt32(totalpg);
    //            }
    //        }
    //        else
    //        {
    //            iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);
    //            iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
    //        }
    //        Session["iTotalPages"] = iTotalPages;

    //        TBWDataList[] lstrecords = new TBWDataList[0];
    //        TBWDataList[] lstrows = new TBWDataList[0];
    //        _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);

    //        if (Request.Url.ToString().ToLower().Contains("byproduct.aspx") && _tempfid.Length > 0)
    //        {
    //            lstrecords = new TBWDataList[dsfamprod.Tables[0].Rows.Count + 1];
    //        }
    //        else
    //        {
    //            lstrecords = new TBWDataList[dscat.Tables[2].Rows.Count + 1];
    //        }
    //        int ictrecords = 0;
    //        int icolstart = 0;
    //        int icol = 1;
    //        lstrows = new TBWDataList[icol];

    //        if (_ViewType == "GV")
    //        {
    //            icol = 4;
    //            lstrows = new TBWDataList[icol];
    //        }
    //        else
    //        {
    //            icol = 1;
    //            lstrows = new TBWDataList[icol];
    //        }
    //        //if (dscat.Tables[0].Rows.Count < icol)
    //        //{
    //        //    icol = dscat.Tables[0].Rows.Count;
    //        //}
    //        string soddevenrow = "odd";
    //        DataRow[] drprodcoll = null;


    //        drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1", "SNO");


    //        int ProductTotalcount = 0;
    //        string trmpstr = "";
    //        int pricecode = 0;
    //        string userid = "";
    //        if (Session["USER_ID"] != null)
    //            userid = Session["USER_ID"].ToString();

    //        if (userid == "")
    //           // userid = "0";
    //            userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
    //        DataSet dsBgDisc = new DataSet();
    //        DataSet dsPriceTableAll = new DataSet();
    //        string tmpProds = "";

    //        string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
    //        if (Convert.ToInt32(userid) > 0)
    //        {

    //            dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
    //        }
    //        else
    //        {
    //            dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
    //        }

    //        pricecode = objHelperDB.GetPriceCode(userid);

    //        if (drprodcoll.Length > 0)
    //        {

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
    //                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_GridView");
    //                else
    //                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_" + soddevenrow);


    //                _stmpl_records.SetAttribute("TBT_ECOMENABLED", objHelperServices.GetIsEcomEnabled(userid));



    //                _stmpl_records.SetAttribute("PRODUCT_ID", drpid["PRODUCT_ID"].ToString());

    //                //_stmpl_records.SetAttribute("MIN_ORD_QTY", oProd.GetMinOrdQty(Convert.ToInt32(drpid["PRODUCT_ID"].ToString())).ToString());
    //                _stmpl_records.SetAttribute("MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());
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
    //                _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_code"].ToString());
                    
    //                int _pid;
    //                if (drpid["PRODUCT_ID"].ToString() != null && drpid["PRODUCT_ID"].ToString()!="")
    //                {
    //                    _pid = System.Convert.ToInt32(drpid["PRODUCT_ID"].ToString());
    //                }
    //                //_stmpl_records.SetAttribute("TBT_USER_PRICE", GetMyPrice(_pid));
    //                _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["NUMERIC_VALUE"].ToString());
    //                _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());

    //                _stmpl_records.SetAttribute("CATEGORY_ID", HttpUtility.UrlEncode(drpid["CATEGORY_ID"].ToString()));

    //                _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

    //                _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));



    //                string PriceTable = "";




    //                if (dsBgDisc != null)
    //                {
    //                    if (dsBgDisc.Tables[0].Rows.Count > 0)
    //                    {
    //                        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
    //                        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
    //                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
    //                        //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
    //                        bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
    //                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
    //                        {
    //                            //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

    //                        }
    //                    }
    //                }
    //                if (Convert.ToInt32(userid) > 0)
    //                {

    //                    PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid),  drpid["PROD_STOCK_FLAG"].ToString(),drpid["ETA"].ToString(), dsPriceTableAll);
    //                }
    //                _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);




    //                if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
    //                {
    //                    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
    //                    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
    //                    _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());

    //                    string newurl = _BCEAPath + "////" + drpid["FAMILY_ID"].ToString() + "=" +
    //                                         _stmpl_records.GetAttribute("FAMILY_NAME");
    //                    if (newurl.ToString().EndsWith(".") == true)
    //                    {
    //                        newurl = newurl.Substring(0, newurl.Length - 1);
    //                    }
    //                    objHelperServices.Cons_NewURl(_stmpl_records, newurl, "fl.aspx", true, "");
    //                }
    //                else
    //                {
    //                    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
    //                    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);
    //                }


    //                if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
    //                    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
    //                else
    //                    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

    //                if (Request.QueryString["byp"] != null && (Request.QueryString["byp"].ToString() == "2" || Request.QueryString["byp"].ToString() == "3"))
    //                {
    //                    _stmpl_records.SetAttribute("BYP", Request.QueryString["byp"].ToString());
    //                }
    //                DataRow[] drAry = dscat.Tables[2].Select("FAMILY_ID ='" + drpid["FAMILY_ID"].ToString() + "' AND " + "PRODUCT_ID ='" + drpid["PRODUCT_ID"].ToString() + "'");
    //                if (drAry.Length > 0)
    //                {
    //                    string descattr = "";
    //                    string desc = "";
    //                    string proddesc = string.Empty;
    //                    foreach (DataRow dr in drAry.CopyToDataTable().Rows)
    //                    {

                            
    //                        if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
    //                        {
    //                            if (dr["ATTRIBUTE_ID"].ToString() == "1")
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
    //                            }
    //                                else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
    //                            }

    //                            if (dr["ATTRIBUTE_TYPE"].ToString() == "1" && dr["ATTRIBUTE_NAME"].ToString() == "PROD_DSC")
    //                            {
    //                            proddesc=dr["STRING_VALUE"].ToString();
    //                            }
    //                        }

    //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
    //                        {
                                
    //                            desc = dr["STRING_VALUE"].ToString().Replace("\r\n", "").Replace("\r", "&nbsp;").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
    //                            descattr = descattr + " "+desc;
    //                            if (proddesc.Trim() == "")
    //                            {
    //                                proddesc = descattr;
    //                            }
    //                            if (desc.Length > 250 && _ViewType == "LV")
    //                            {
    //                                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
    //                                desc = desc.Substring(0, 250).ToString();

    //                                if(drpid["PRODUCT_COUNT"].ToString()!="1")
    //                                {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                                }
    //                                else
    //                                {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), proddesc);
    //                                }
    //                            }
    //                            //else if (desc.Length > 30 && _ViewType == "GV")
    //                            //{
    //                            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
    //                            //    desc = desc.Substring(0, 30).ToString();
    //                            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                            //}
    //                            else
    //                            {
    //                                if (drpid["PRODUCT_COUNT"].ToString() != "1")
    //                                {
    //                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                                }
    //                                else
    //                                {
    //                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), proddesc);
    //                                }
    //                                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
    //                            }

    //                            //_stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"]);
    //                        }
    //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
    //                        {
    //                            if (dr["NUMERIC_VALUE"].ToString().Length > 0)
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.FixDecPlace(Convert.ToDecimal(dr["NUMERIC_VALUE"])).ToString());
    //                        }
    //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
    //                        {
    //                            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
    //                            if (Fil.Exists)
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
    //                            }

    //                        }
                         
    //                    }
    //                    if (drpid["PRODUCT_COUNT"].ToString() != "1")
    //                    {

    //                        if (descattr.Length > 86 && _ViewType == "GV")
    //                        {
    //                            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
    //                            descattr = descattr.Substring(0, 86).ToString();
    //                            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
    //                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
    //                        }
    //                        else
    //                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
    //                    }
    //                    else
    //                    {
    //                        if (proddesc.Trim() != "")
    //                        {
    //                            if (proddesc.Length > 86 && _ViewType == "GV")
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
    //                                proddesc = proddesc.Substring(0, 86).ToString();
    //                                proddesc = proddesc.Substring(0, proddesc.LastIndexOf(" "));
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", proddesc);
    //                            }
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", proddesc);
    //                            }
    //                        }
    //                        else
    //                        {
    //                            if (descattr.Length > 86 && _ViewType == "GV")
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
    //                                descattr = descattr.Substring(0, 86).ToString();
    //                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
    //                            }
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
    //                            }
    //                        }
                        
    //                    }
    //                }
    //                //ADDED BY :INDU
    //                if (_stmpl_records.GetAttribute("TBT_SUB_FAMILY").ToString() == "False")
    //                {
    //                    string newurl = string.Empty;

    //                    newurl = _BCEAPath + "////" + drpid["FAMILY_ID"].ToString() + "=" + _stmpl_records.GetAttribute("FAMILY_NAME") + "////" +
    //                       drpid["PRODUCT_ID"].ToString() + "=" + _stmpl_records.GetAttribute("PRODUCT_CODE");
    //                    if (newurl.ToString().EndsWith(".") == true)
    //                    {
    //                        newurl = newurl.Substring(0, newurl.Length - 1);
    //                    }
    //                    objHelperServices.Cons_NewURl(_stmpl_records, newurl, "pd.aspx", true, "");
    //                }
    //                //END

    //                lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());

    //                icolstart++;

    //                if (icolstart >= icol || oe == drprodcoll.Length)
    //                {
    //                    _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
    //                    _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "producttable_" + soddevenrow);
    //                    _stmpl_container.SetAttribute("TBT_SUBCATNAME", subcatname_l1_l2);
    //                    _stmpl_container.SetAttribute("TBT_CATEGORY_NAME", catheader);
    //                    _stmpl_container.SetAttribute("TBWDataList", lstrows);
    //                    lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
    //                    ictrecords++;
    //                    icolstart = 0;
    //                    lstrows = new TBWDataList[icol];

    //                }

    //            }

    //            _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);

    //            if (Request.QueryString["ViewMode"] != null)
    //            {
    //                PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
    //                isreadonly.SetValue(this.Request.QueryString, false, null);
    //                this.Request.QueryString.Remove("ViewMode");
    //            }
    //            string productlistURL = Request.QueryString.ToString();
    //            string powersearchURListView = "";
    //            string powersearchURLGridView = "";
    //            powersearchURListView = "/"+productlistURL + "&ViewMode=LV";
    //            powersearchURLGridView = "/" + productlistURL + "&ViewMode=GV";




    //            _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "main");



    //            _stmpl_container.SetAttribute("TBT_TOSUITE_BRAND", _tsb);
    //            _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL", _tsm);


    //            //modify by palani
    //            DataSet dscatebybrand = new DataSet();
    //            DataSet dssubcatbybrand = new DataSet();
    //            DataSet breadcrumb = new DataSet();
    //            StringTemplateGroup _stg_records_container = null;
    //            StringTemplate _stmpl_records_tmpl = null;
    //            StringTemplateGroup _stg_records_container1 = null;
    //            StringTemplate _stmpl_records_tmpl1 = null;
    //            TBWDataList1[] lstrecords11 = new TBWDataList1[0];
    //            TBWDataList1[] lstrows11 = new TBWDataList1[0];
    //            TBWDataList2[] lstrecords12 = new TBWDataList2[0];
    //            int icolstart11 = 0;
    //            int ictrecords11;
    //            int ictrecords12;
    //            string tempCID = "";
    //            string subrealBCEAPath = "";
    //            dssubcatbybrand = (DataSet)HttpContext.Current.Session["dssubcatbybrand"];
    //            dscatebybrand = (DataSet)HttpContext.Current.Session["dscatbybrand"];
    //            breadcrumb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
    //            string catvalue = "";
              

    //            if (dscatebybrand != null && dscatebybrand.Tables.Count > 0 && dscatebybrand.Tables[0].TableName.Contains("Category") && dscatebybrand.Tables[0].Rows.Count > 0 && breadcrumb.Tables[0].Rows.Count > 2)
    //            {
    //                if (Request.QueryString["cid"] == null)
    //                    tempCID = "WES0830";
    //                else
    //                    tempCID = Request.QueryString["cid"].ToString();
    //                if (_cid != "")
    //                    _ParentCatID = GetParentCatID(_cid);

    //                DataRow[] _DCRow = null;
    //                _DCRow = dscatebybrand.Tables[0].Select();

    //                if (_DCRow != null && _DCRow.Length > 0)
    //                {
    //                    ictrecords11 = 0;
    //                    lstrecords11 = new TBWDataList1[_DCRow.Length + 1];
    //                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
    //                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
    //                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "multilistitem");
    //                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Category");
    //                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Category");
    //                    _stmpl_records_tmpl.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
    //                    lstrecords11[ictrecords11] = new TBWDataList1(_stmpl_records_tmpl.ToString());
    //                    ictrecords11++;
    //                    //_stg_container1 = new StringTemplateGroup("searchrsltproductmain", stemplatepath);
    //                    //_stmpl_container1 = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "main_sub");
                      
    //                    foreach (DataRow _drow in _DCRow)
    //                    {
    //                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
    //                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
    //                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "multilistitem");
    //                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["Category_Name"].ToString());
    //                        //  _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
    //                        _stmpl_records_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
    //                        string eapath = "";
    //                        string subcatvalue = "";
    //                        if (breadcrumb.Tables[0].Rows.Count > 0 && breadcrumb.Tables[0].Rows.Count > 3)
    //                        {
    //                             catvalue = breadcrumb.Tables[0].Rows[3]["ItemValue"].ToString();
    //                            if (catvalue.ToUpper() == _drow["Category_Name"].ToString().ToUpper())
    //                            {
    //                                _stmpl_records_tmpl.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
    //                                // new line added
    //                                //_stmpl_container.SetAttribute("TBW_CATEGORY_NAME", catvalue);
    //                            }
    //                        }

    //                        if (breadcrumb != null)
    //                            eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb.Tables[0].Rows[2]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));

    //                        //Modified by:indu
    //                        //DataTable dt = breadcrumb.Tables[0];
    //                        //DataRow[] foundRows;
    //                        //string cat = "Category";
                        
    //                        //foundRows = dt.Select("ItemType='" + cat + "' ");
    //                        //string parentcat = string.Empty;
    //                        //if (foundRows.Length > 0)
    //                        //{
    //                        //    parentcat = foundRows[0]["Itemvalue"].ToString();
    //                        //    if (subcatvalue == "")
    //                        //    {
    //                        //        parentcat = "////Category=" + parentcat;
    //                        //    }
    //                        //    else
    //                        //    {
    //                        //        parentcat = "////Category=" + subcatvalue;
    //                        //    }
    //                        //}
    //                        //string href = _realBCEAPath + parentcat + "////" + "Category=" + _drow["Category_Name"].ToString();

    //                        string href="";
    //                        if (catvalue == "")
    //                            href = _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();
    //                        else
    //                        {
    //                            //href = "";
    //                            string _realBCEAPathmc = "////" + " " + "////" + " " + "////" + "CELLULAR ACCESSORIES" + "////" + _drow["brandvalue"].ToString().ToUpper() + "////" + _tsm.ToUpper();
    //                               //.Replace("+","||");
    //                           href =  _realBCEAPathmc + "////" + "Category=" + _drow["Category_Name"].ToString();
    //                        }
    //                        subrealBCEAPath = _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();
    //                      //  subrealBCEAPath = _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();
    //                        string rel = "bb.aspx?&Id=0&pcr=" + HttpUtility.UrlEncode(tempCID) + "&cid=" + HttpUtility.UrlEncode(_drow["CATEGORY_ID"].ToString()) + "&searchstr=&bname=" + HttpUtility.UrlEncode(_drow["brandvalue"].ToString()) + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&type=Category&tsm=" + HttpUtility.UrlEncode(_tsm) + "&byp=2&path=" + eapath;
    //                        href = objHelperServices.Cons_NewURl_bybrand_MS(rel, href, "bb.aspx", "Category");
    //                        //_stmpl_records_tmpl.SetAttribute("href", "bb.aspx?"+href);
    //                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "/"+ href +"/bb/");

    //                        lstrecords11[ictrecords11] = new TBWDataList1(_stmpl_records_tmpl.ToString());
    //                        ictrecords11++;
    //                    }
    //                    _stmpl_container.SetAttribute("TBWDataList1", lstrecords11);
    //                    _stmpl_container.SetAttribute("TBW_SELECTED_ID", "1");

    //                }

    //            }
    //            if (dssubcatbybrand != null && dssubcatbybrand.Tables.Count > 0 && dssubcatbybrand.Tables[0].TableName.Contains("Category") && dssubcatbybrand.Tables[0].Rows.Count > 0 && breadcrumb.Tables[0].Rows.Count > 3)
    //            {
    //                if (dssubcatbybrand != null && dssubcatbybrand.Tables.Count > 0 && dssubcatbybrand.Tables[0].TableName.Contains("Category"))
    //                {
    //                    if (Request.QueryString["cid"] == null)
    //                        tempCID = "WES0830";
    //                    else
    //                        tempCID = Request.QueryString["cid"].ToString();
    //                    if (_cid != "")
    //                        _ParentCatID = GetParentCatID(_cid);

    //                    DataRow[] _DCRow1 = null;
    //                    _DCRow1 = dssubcatbybrand.Tables[0].Select();

    //                    if (_DCRow1 != null && _DCRow1.Length > 0)
    //                    {
    //                        ictrecords12 = 0;
    //                        lstrecords12 = new TBWDataList2[_DCRow1.Length + 1];
    //                        _stg_records1 = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
    //                        _stg_records_container1 = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
    //                        _stmpl_records_tmpl1 = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "multilistitem1");
    //                        _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL1", "Select Category");
    //                        _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL", "Select Category");
    //                        _stmpl_records_tmpl1.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
    //                        lstrecords12[ictrecords12] = new TBWDataList2(_stmpl_records_tmpl1.ToString());
    //                        ictrecords12++;
    //                        foreach (DataRow _drow in _DCRow1)
    //                        {
    //                            _stg_records1 = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
    //                            _stg_records_container1 = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
    //                            _stmpl_records_tmpl1 = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "multilistitem1");
    //                            _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL", _drow["Category_Name"].ToString());
    //                            // _stmpl_records_tmpl1.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
    //                            _stmpl_records_tmpl1.SetAttribute("TBW_SELECTED_ID", "1");
    //                            string eapath = "";
    //                            string subcatvalue = "";
    //                            if (breadcrumb.Tables[0].Rows.Count > 0 && breadcrumb.Tables[0].Rows.Count > 4)
    //                            {
    //                                 subcatvalue = breadcrumb.Tables[0].Rows[4]["ItemValue"].ToString();
    //                                if (subcatvalue.ToUpper() == _drow["Category_Name"].ToString().ToUpper())
    //                                {
    //                                    _stmpl_records_tmpl1.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
    //                                    // new line added
    //                                   // _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", subcatvalue);
    //                                }
    //                            }

    //                            if (breadcrumb != null)
    //                                eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb.Tables[0].Rows[3]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
    //                            //string href = "bb.aspx?&Id=0&pcr=" + HttpUtility.UrlEncode(tempCID) + "&cid=" + HttpUtility.UrlEncode(_drow["CATEGORY_ID"].ToString()) + "&searchstr=&bname=" + HttpUtility.UrlEncode(_drow["brandvalue"].ToString()) + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&type=Category&tsm=" + HttpUtility.UrlEncode(_tsm) + "&byp=2&path=" + eapath;
    //                            ////_stmpl_records_tmpl1.SetAttribute("href", href);

    //                            //////   _stmpl_records_tmpl1.SetAttribute("href", "bb.aspx?"+href);
    //                            //_stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL1", href);
    //                            DataTable dt = breadcrumb.Tables[0];
    //                            DataRow[] foundRows;
    //                            string cat = "Category";
    //                            string nocat = "Cellular Accessories";
    //                            foundRows = dt.Select("ItemType='" + cat + "' and Itemvalue<>'" + nocat + "'");
    //                            string parentcat = string.Empty;
    //                            if (foundRows.Length > 0)
    //                            {
    //                                parentcat = foundRows[0]["Itemvalue"].ToString();
    //                                if (subcatvalue == "")
    //                                {
    //                                    parentcat = "////Category=" + parentcat;
    //                                }
    //                                else
    //                                {
    //                                    parentcat = "";
    //                                    parentcat = "////Category=" + subcatvalue;
    //                                }
    //                            }

    //                           // string href = _realBCEAPath + parentcat +"////" + "Category=" + _drow["Category_Name"].ToString();
    //                            //string href = _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();
                              
    //                            //_realBCEAPath= "AllProducts" + "////" + "WESAUSTRALASIA" + "////" + "CELLULAR ACCESSORIES" + "////" + "AttribSelect=Brand=" + _drow["brandvalue"].ToString().ToUpper() + "////" + "AttribSelect=Model=" + _tsm.ToUpper();
    //                            //string href = _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();
    //                             string href="";
    //                             if (subcatvalue == "")
    //                                 href =  _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();
    //                             else
    //                             {
    //                                 //.Replace("+","||")
    //                             string    _realBCEAPath1 = "////" + " " + "////" + " " + "////" + "CELLULAR ACCESSORIES" + "////" + _drow["brandvalue"].ToString().ToUpper() + "////" + HttpUtility.UrlEncode (_tsm.ToUpper()) + "////" + "Category=" + catvalue.ToUpper();
    //                             href =  _realBCEAPath1 + "////" + "Category=" + _drow["Category_Name"].ToString();
    //                             }
    //                                string rel = "bb.aspx?&Id=0&pcr=" + HttpUtility.UrlEncode(tempCID) + "&cid=" + HttpUtility.UrlEncode(_drow["CATEGORY_ID"].ToString()) + "&searchstr=&bname=" + HttpUtility.UrlEncode(_drow["brandvalue"].ToString()) + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&type=Category&tsm=" + HttpUtility.UrlEncode(_tsm) + "&byp=2&path=" + eapath;
    //                                href = objHelperServices.Cons_NewURl_bybrand(rel, href, "bb.aspx", "Category");
    //                                //   _stmpl_records_tmpl1.SetAttribute("href", "bb.aspx?"+href);
    //                                _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL1",  "/"+href+"/bb/" );
                               

    //                            lstrecords12[ictrecords12] = new TBWDataList2(_stmpl_records_tmpl1.ToString());
    //                            ictrecords12++;
    //                        }
    //                        _stmpl_container.SetAttribute("TBWDataList2", lstrecords12);
    //                        _stmpl_container.SetAttribute("TBW_SELECTED_ID", "2");

    //                        //if (dssubcatbybrand != null)
    //                        //    dssubcatbybrand = null;
    //                    }

    //                }

    //            }
    //            if (dscatebybrand != null)
    //            {
    //                if (dscatebybrand.Tables[0].Rows.Count > 0)
    //                    _stmpl_container.SetAttribute("TBT_SHOW_FT", true);
    //                else
    //                    _stmpl_container.SetAttribute("TBT_SHOW_FT", false);
    //            }
    //            if (breadcrumb != null)
    //            {
    //                int i;
    //                string chk = "";
    //                bool flgcheck = false;
    //                for (i = 0; i < breadcrumb.Tables[0].Rows.Count; i++)
    //                {
    //                    chk = breadcrumb.Tables[0].Rows[i]["ItemType"].ToString();
    //                    if (chk == "Type")
    //                    {
    //                        flgcheck = true;
    //                    }
    //                }
    //                if (dssubcatbybrand != null)
    //                {
    //                    if (flgcheck == false && dssubcatbybrand.Tables[0].Rows.Count > 0)
    //                        _stmpl_container.SetAttribute("TBT_SHOW_FTSCat", true);
    //                    else
    //                        _stmpl_container.SetAttribute("TBT_SHOW_FTSCat", false);
    //                }
    //            }
    //            //end


    //            if (Tosuit_ModelImage != "")
    //            {
    //                System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + Tosuit_ModelImage.Replace("/", "\\"));
    //                if (Fil.Exists)
    //                {
    //                    _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL_IMAGE", Tosuit_ModelImage.ToString().Replace("\\", "/"));
    //                }
    //                else
    //                {
    //                    _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL_IMAGE", "/images/noimage.gif");
    //                }
    //            }
    //            else
    //            {

    //                _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL_IMAGE", "/images/noimage.gif");
    //            }
    //            _stmpl_container.SetAttribute("TBW_PRODUCT_COUNT", iTotalProducts);

    //            _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh.ToUpper());

    //            _stmpl_container.SetAttribute("TBW_URL", powersearchURListView);
    //            _stmpl_container.SetAttribute("TBW_URL1", powersearchURLGridView);

    //            if (_ViewType == "LV")
    //                _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
    //            else
    //                _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);


    //            _stmpl_container.SetAttribute("SELECT_" + iRecordsPerPage, "SELECTED=\"SELECTED\" ");



    //            if (iTotalPages > 1)
    //            {
    //                if (iPageNo != iTotalPages)
    //                {
    //                    _stmpl_container.SetAttribute("TBW_TO_PAGE", true);
    //                }
    //                else if (iPageNo == iTotalPages)
    //                {
    //                    _stmpl_container.SetAttribute("TBW_TOTAL_PAGE", true);
    //                }
    //            }
    //            else
    //            {
    //                _stmpl_container.SetAttribute("TBW_TO_PAGE", false);
    //            }


    //            if (iRecordsPerPage == 32767) //View All
    //            {
    //                iRecordsPerPage = iTotalProducts;
    //            }
    //            if (iPageNo < iTotalPages)
    //            {
    //                if (iPageNo > 1)
    //                {
    //                    iPrevPgNo = iPageNo - 1;
    //                }
    //                else
    //                {
    //                    iPrevPgNo = 1;
    //                }
    //                iNextPgNo = iPageNo + 1;
    //            }
    //            else if (iPageNo == iTotalPages && iTotalPages != 1)
    //            {
    //                iPrevPgNo = iPageNo - 1;
    //                iNextPgNo = iPageNo;
    //            }
    //            else
    //            {
    //                iNextPgNo = 1;
    //                iPrevPgNo = 1;
    //            }
    //            try
    //            {
    //                bool rUrl = false;
    //                if (Request.Url.ToString().ToLower().Contains("bb.aspx") == true || Request.Url.ToString().ToLower().Contains("byproduct.aspx") == true)
    //                {
    //                    rUrl = true;
    //                }
    //                string sl2 = "";
    //                string sl1 = "";
    //                string sl3 = "";
    //                string tosuite_brand = "";
    //                string tosuite_model = "";
    //                if (rUrl)
    //                {
    //                    if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")
    //                        sl1 = Request.QueryString["cid"].ToString();
    //                    if (Request.QueryString["sl1"] != null && Request.QueryString["sl1"].ToString() != "")
    //                        sl2 = Request.QueryString["sl1"].ToString();
    //                    if (Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "")
    //                        sl3 = Request.QueryString["sl2"].ToString();
    //                    if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
    //                        tosuite_brand = Server.UrlEncode(Request.QueryString["tsb"].ToString());
    //                    if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
    //                        tosuite_model = Server.UrlEncode(Request.QueryString["tsm"].ToString());
    //                }
    //                _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
    //                if (iPageNo > 2 && (iTotalPages >= (iPageNo + 2)))
    //                {

    //                    _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo - 2);
    //                    SetQueryString(_stmpl_pages);


    //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                    _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);


    //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

    //                    _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                    _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo - 1);
    //                    SetQueryString(_stmpl_pages);

    //                    _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");

    //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

    //                    _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                    _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo);
    //                    SetQueryString(_stmpl_pages);

    //                    _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));

    //                    _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                    _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo + 1);
    //                    SetQueryString(_stmpl_pages);

    //                    _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");

    //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

    //                    _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
    //                    _stmpl_pages.SetAttribute("TBW_CATEGORY_ID", _cid);
    //                    _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo + 2);
    //                    SetQueryString(_stmpl_pages);

    //                    _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");

    //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
    //                }
    //                else if (iPageNo > 0 && iPageNo < 4)
    //                {

    //                    _stmpl_pages.SetAttribute("TBW_PAGE_NO", 1);
    //                    SetQueryString(_stmpl_pages);

    //                    _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);


    //                    if (iPageNo == 1)
    //                    {
    //                        if (1 == iTotalPages)

    //                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
    //                        else
    //                        {
    //                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));
    //                        }
    //                    }
    //                    else
    //                    {
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
    //                    }

    //                    if (2 <= iTotalPages)
    //                    {
    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", 2);
    //                        SetQueryString(_stmpl_pages);


    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);


    //                        if (iPageNo == 2)
    //                        {
    //                            if (2 == iTotalPages)
    //                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
    //                            else
    //                            {
    //                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));
    //                            }
    //                        }
    //                        else
    //                        {
    //                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));                                
    //                        }
    //                    }

    //                    if (3 <= iTotalPages)
    //                    {
    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", 3);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);

    //                        if (iPageNo == 3)
    //                        {
    //                            if (3 == iTotalPages)
    //                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
    //                            else
    //                            {
    //                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));
    //                            }
    //                        }
    //                        else
    //                        {
    //                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));

    //                        }
    //                    }
    //                    if (4 <= iTotalPages)
    //                    {
    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", 4);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);

    //                        if (iPageNo == 4)
    //                        {
    //                            if (4 == iTotalPages)
    //                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("\n", "").Replace("\r", "").Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
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

    //                    }
    //                    if (5 <= iTotalPages)
    //                    {
    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", 5);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);

    //                        if (iPageNo == 5)
    //                        {
    //                            if (5 == iTotalPages)
    //                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
    //                            else
    //                            {
    //                                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
    //                            }
    //                        }
    //                        else
    //                        {
    //                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
    //                        }

    //                    }
    //                }
    //                else
    //                    if (iPageNo == iTotalPages && 1 <= iTotalPages - 4)
    //                    {

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 4);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);

    //                        if (iPageNo == iTotalPages)
    //                        {
    //                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
    //                        }
    //                        else
    //                        {
    //                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
    //                        }

    //                    }
    //                    else if (iPageNo == iTotalPages)
    //                    {


    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);

    //                        if (iPageNo == iTotalPages)
    //                        {
    //                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", "").Replace("-</td>", "</td>"));
    //                        }
    //                        else
    //                        {
    //                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("\n", "").Replace("\r", "").Replace("-</td>", "</td>"));
    //                        }

    //                    }
    //                    else if (1 <= iTotalPages - 4)
    //                    {

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 4);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());



    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());




    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());



    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                        //  _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
    //                        //new line
    //                        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));


    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        if (iPageNo == iTotalPages)
    //                        {
    //                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
    //                        }
    //                        else
    //                        {
    //                            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
    //                            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("</td>", "</td>"));
    //                            // _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("-</td>", "</td>"));

    //                        }


    //                    }
    //                if (iTotalPages > 1 && iPageNo != iTotalPages)
    //                {
    //                    // _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
    //                    //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", _cid);
    //                    // _stmpl_pages.SetAttribute("TBW_PAGE_NO", (iPageNo+1));
    //                    if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX"))
    //                        _stmpl_container.SetAttribute("TBW_NEXT_PAGE", "<td align=\"left\" class=\"tx_10A\"><a href=\"byproduct.aspx?pgno=" + (iPageNo + 1) + "&cid=" + sl1 + "&sl2=" + sl2 + "&fid=" + "" + "&byp=2\" >Next Page</a></td>");
    //                    else if (Request.Url.OriginalString.ToString().ToUpper().Contains("BB.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("PS.ASPX"))
    //                    {
    //                        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpagenoNext");
    //                        _stmpl_pages.SetAttribute("TBW_PAGE_NO", (iPageNo + 1));
    //                        SetQueryString(_stmpl_pages);
    //                        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
    //                        _stmpl_container.SetAttribute("TBW_NEXT_PAGE", _stmpl_pages.ToString());
    //                        //_stmpl_container.SetAttribute("TBW_NEXT_PAGE", "<td align=\"left\" class=\"tx_10A\"><a href=\"bb.aspx?&pgno=" + (iPageNo + 1) + "&cid=" + sl1 + "&sl1=" + sl2 + "&sl2=" + sl3 + "&tsb=" + tosuite_brand + "&tsm=" + tosuite_model + "&byp=2\" >Next Page</a></td>");
    //                    }

    //                }
    //            }
    //            catch (Exception ex)
    //            {

    //            }
    //            string nextstring = "";
    //            try
    //            {
    //                if (Request.QueryString["srctext"].ToString() != "")
    //                {
    //                  //  nextstring = "&srctext=" + Request.QueryString["srctext"].ToString().Replace("\"", "%22").Replace("&", "%26").Replace("#", "%23");
    //                    nextstring = Request.QueryString["srctext"].ToString().Replace("\"", "%22").Replace("&", "%26").Replace("#", "%23");
    //                }
    //            }
    //            catch (Exception) { }
    //            _stmpl_container.SetAttribute("TBW_START_PAGE_NO", (iPageNo * iRecordsPerPage) - (iRecordsPerPage - 1));

    //            if ((iPageNo * iRecordsPerPage) > iTotalProducts)
    //                _stmpl_container.SetAttribute("TBW_END_PAGE_NO", iTotalProducts);
    //            else
    //                _stmpl_container.SetAttribute("TBW_END_PAGE_NO", (iPageNo * iRecordsPerPage));
    //            _stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
    //            _stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo + nextstring);
    //            _stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
    //            _stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo + nextstring);
    //            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
    //            sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");


    //            //if (sHTML.ToString().Contains("src=\"/prodimages\""))
    //            //{
    //            //    sHTML = sHTML.Replace("src=\"/prodimages\"", "src=\"/prodimages/images/noimage.gif\"");
    //            //    sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
    //            //}
    //            //if (sHTML.ToString().Contains("src=\"\""))
    //            //{
    //            //    sHTML = sHTML.ToString().Replace("src=\"\"", "src=\"/prodimages/images/noimage.gif\"");
    //            //    sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
    //            //}

    //            if (sHTML.ToString().Contains("data-original=\"/prodimages\""))
    //            {
    //                sHTML = sHTML.Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
    //                sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
    //            }
    //            if (sHTML.ToString().Contains("data-original=\"\""))
    //            {
    //                sHTML = sHTML.ToString().Replace("data-original=\"\"", "data-original=\"/prodimages/images/noimage.gif\"");
    //                sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
    //            }
    //            if (dscat.Tables[1].Rows[0].ItemArray[0].ToString() == "0")
    //                sHTML = "";
    //            if (dscat.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
    //            {
    //                sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1" + nextstring + "\">Previous</a>", "");
    //                sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1" + nextstring + "\">Next</a>", "");
    //            }
    //            if (Convert.ToInt32(dscat.Tables[0].Rows[0].ItemArray[0].ToString()) == iPageNo)
    //            {
    //                sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1" + nextstring + "\">Next</a>", "");
    //            }


    //        }
    //        string eapath1 = _EAPath.Replace("'", "###");
    //        string BCEAPath = _BCEAPath.Replace("'", "###");
    //        htmleapath.Value = eapath1;
    //        htmlbceapth.Value = BCEAPath;
    //        htmltotalpage.Value = iTotalPages.ToString();
    //        htmlviewmode.Value = _ViewType;
    //        if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
    //        {
    //            htmlirecords.Value = Session["RECORDS_PER_PAGE_BYBRAND"].ToString();
    //        }
    //        else
    //        {
    //            htmlirecords.Value = "24";
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
    //        //if (oCon != null)
    //        //{
    //        //oCon.Close();
    //        //}
    //    }
    //    //}

    //    sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
    //    return objHelperServices.StripWhitespace(sHTML);
        
    //}


    protected string ST_BrandAndModelProductListNewJson()
    {
        string _tempfid = "";
        string category_nameh = "";
        string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
      
        if (Request["srctext"] != null && Request["srctext"].ToString() == "")
        {
            return "CLEAR";
        }
        else if (Request["srctext"] != null)
        {
            _SearchString = Request["srctext"].ToString();

        }
        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplateGroup _stg_records1 = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        //StringTemplate _stmpl_pages = null;
        //string subcatname_l1 = "";
        //string subcatname_l2 = "";
        string subcatname_l1_l2 = "";
        //string catname = "";
        string catheader = "";
        //bool subcatdispflag = false;
        //bool catdispflag = false;
        int oe = 0;
        string _ViewType = "";

        string sHTML = "";
        string _ParentCatID = "";
        //if (Convert.ToInt32(Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        // {
        try
        {


            if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
                _tsm = Request.QueryString["tsm"];

            if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
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

            //if (Request.QueryString["pcr"] != null)
            //    _pcr = Request.QueryString["pcr"];




            if (Request.QueryString["ViewMode"] != null)
            {
                _ViewType = Request.QueryString["ViewMode"];
                Session["PL_VIEW_MODE"] = _ViewType;
            }
            else if (Session["PL_VIEW_MODE"] != null && Session["PL_VIEW_MODE"].ToString() != "")
                _ViewType = Session["PL_VIEW_MODE"].ToString();
            else
                _ViewType = "GV";

            //if (_ViewType != "")
            //    Session["BM_VIEW_MODE"] = _ViewType;

            //if (Session["BM_VIEW_MODE"] != null && Session["BM_VIEW_MODE"] != "")
            //    _ViewType = Session["BM_VIEW_MODE"].ToString();

            //if (Request.QueryString["path"] != null)
            //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(Request.QueryString["path"].ToString()));
            string _BCEAPath = string.Empty;
            string _realBCEAPath = string.Empty;

            DataSet ds = new DataSet();
            string _realBCEAPathmc = string.Empty; 
            if (HttpContext.Current.Session["BreadCrumbDS"] != null)
            {
                ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                _realBCEAPathmc = ds.Tables[0].Rows[2]["EAPath"].ToString();
                //if (_tsb == null)
                //{
                //    _tsb = ds.Tables[0].Rows[1]["Itemvalue"].ToString();
                //}
                if ((_tsm == null) || (_tsb == ""))
                {
                    _tsm = ds.Tables[0].Rows[2]["Itemvalue"].ToString();
                }
            }
            if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
            {
                _BCEAPath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();
                _realBCEAPath = _BCEAPath;
                string _newBCEAPath = string.Empty;
                string _newrealBCEAPath = string.Empty;
                string[] ConsURL = _BCEAPath.Split(new string[] { "////" }, StringSplitOptions.None);
                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        _newBCEAPath = ConsURL[i].ToString();
                        //  _newrealBCEAPath = ConsURL[i].ToString();
                    }
                    else if (i == 4)
                    {

                        _newBCEAPath = _newBCEAPath + "////" + "AttribSelect=Brand=" + ConsURL[i].ToString();
                        // _newrealBCEAPath = _newrealBCEAPath + "////" +  ConsURL[i].ToString();
                    }
                    else if (i == 5)
                    {
                        //_newBCEAPath = _newBCEAPath + "////" + "AttribSelect=Model=" +HttpUtility.UrlEncode(ConsURL[i].ToString());
                        //_newrealBCEAPath = _newrealBCEAPath + "////" + HttpUtility.UrlEncode(ConsURL[i].ToString());
                        _newBCEAPath = _newBCEAPath + "////" + "AttribSelect=Model=" + ConsURL[i].ToString();
                        //_newrealBCEAPath = _newrealBCEAPath + "////" + ConsURL[i].ToString();
                    }
                    else
                    {
                        _newBCEAPath = _newBCEAPath + "////" + ConsURL[i].ToString();
                        //_newrealBCEAPath = _newrealBCEAPath + "////" + ConsURL[i].ToString();
                    }

                }
                _BCEAPath = _newBCEAPath;
                // _realBCEAPath = _newrealBCEAPath;
            }
            if (_EAPath == "")
            {
                _EAPath = HttpContext.Current.Session["EA"].ToString();
            }
            //string _BCEAPath = string.Empty;
            //if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
            //{
            //    _BCEAPath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();
            //}




            if (Request.QueryString["type"] == null)
            {
                DataSet tmp = (DataSet)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTDataSet);
                if (tmp != null && tmp.Tables.Count >= 1 && tmp.Tables[0].Rows.Count >= 1)
                {

                    category_nameh = tmp.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                }
            }
            else
            {
                //category_nameh = Request.QueryString["value"].ToString();
                category_nameh = HttpUtility.HtmlDecode(Request.QueryString["value"].ToString());
            }







            DataSet dsfamprod = new DataSet();

            string Tosuit_ModelImage = "";
            DataSet dsModel = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
            if (dsModel != null)
            {
                DataRow[] dr = dsModel.Tables[0].Select("TOSUITE_MODEL='" + _tsm + "'");
                if (dr.Length > 0)
                    Tosuit_ModelImage = dr.CopyToDataTable().Rows[0]["IMAGE_FILE"].ToString();

            }
            else
                Tosuit_ModelImage = (string)objHelperDB.GetGenericPageDataDB("", _tsb, _tsm, "GET_BYBRAND_MODEL_IMAGE", HelperDB.ReturnType.RTString);


            dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            //dscat = ps.getwesproducts();
            if (dscat == null || dscat.Tables["FamilyPro"].Rows.Count == 0)
            {
                return "";
            }
            
            DataRow drpagect = dscat.Tables[0].Rows[0];
            DataRow drproductsct = dscat.Tables[1].Rows[0];
            if (dsfamprod.Tables != null && dsfamprod.Tables.Count > 0 && Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "" && Request.Url.ToString().ToLower().Contains("byproduct.aspx"))
            {
                iTotalProducts = dsfamprod.Tables[0].Rows.Count;
                if (iRecordsPerPage >= iTotalProducts)
                {
                    iTotalPages = 1;
                }
                else
                {
                    double recordpg = iRecordsPerPage;
                    double totalpg = (iTotalProducts / recordpg);
                    if (totalpg > Convert.ToInt32(totalpg))
                        iTotalPages = Convert.ToInt32(totalpg) + 1;
                    else
                        iTotalPages = Convert.ToInt32(totalpg);
                }
            }
            else
            {
                iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
            }
            Session["iTotalPages"] = iTotalPages;

            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];
            _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);

            if (Request.Url.ToString().ToLower().Contains("byproduct.aspx") && _tempfid.Length > 0)
            {
                lstrecords = new TBWDataList[dsfamprod.Tables[0].Rows.Count + 1];
            }
            else
            {
                lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count + 1];
            }
            int ictrecords = 0;
            int icolstart = 0;
            int icol = 1;
            lstrows = new TBWDataList[icol];

            if (_ViewType == "GV")
            {
                icol = 4;
                lstrows = new TBWDataList[icol];
            }
            else
            {
                icol = 1;
                lstrows = new TBWDataList[icol];
            }
            //if (dscat.Tables[0].Rows.Count < icol)
            //{
            //    icol = dscat.Tables[0].Rows.Count;
            //}
            string soddevenrow = "odd";
            //DataRow[] drprodcoll = null;


            //drprodcoll = dscat.Tables["FamilyPro"].Select("", "SNO");


            int ProductTotalcount = 0;
            string trmpstr = "";
            int pricecode = 0;
            string userid = "";
            if (Session["USER_ID"] != null)
                userid = Session["USER_ID"].ToString();

            if (userid == "")
                // userid = "0";
                userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
            DataSet dsBgDisc = new DataSet();
            DataSet dsPriceTableAll = new DataSet();
            string tmpProds = "";

            //string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
            //if (Convert.ToInt32(userid) > 0)
            //{

            //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
            //}
            //else
            //{
            //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
            //}

            pricecode = objHelperDB.GetPriceCode(userid);

            if (dscat.Tables["FamilyPro"].Rows.Count > 0)
            {

                tmpProds = "";
                if (Convert.ToInt32(userid) > 0)
                {
                    foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                        tmpProds = tmpProds.Replace(",,", ",");
                    }
                    if (tmpProds != "")
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                        dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                    }
                }
                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);
                foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                {
                    oe++;
                    if ((oe % 2) == 0)
                    {
                        soddevenrow = "even";
                    }
                    else
                    {
                        soddevenrow = "odd";
                    }


                    if (_ViewType == "GV")
                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_GridView");
                    else
                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_" + soddevenrow);


                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomEnabled);



                    _stmpl_records.SetAttribute("PRODUCT_ID", drpid["PRODUCT_ID"].ToString());

                    //_stmpl_records.SetAttribute("MIN_ORD_QTY", oProd.GetMinOrdQty(Convert.ToInt32(drpid["PRODUCT_ID"].ToString())).ToString());
                    _stmpl_records.SetAttribute("MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());
                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                    if (_ViewType == "GV")
                    {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                    }

                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);
                    _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", trmpstr.Replace('"', ' '));

                    _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_code"].ToString());

                    int _pid;
                    if (drpid["PRODUCT_ID"].ToString() != null && drpid["PRODUCT_ID"].ToString() != "")
                    {
                        _pid = System.Convert.ToInt32(drpid["PRODUCT_ID"].ToString());
                    }
                    //_stmpl_records.SetAttribute("TBT_USER_PRICE", GetMyPrice(_pid));
                    //_stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["NUMERIC_VALUE"].ToString());
                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());

                    _stmpl_records.SetAttribute("CATEGORY_ID", HttpUtility.UrlEncode(drpid["CATEGORY_ID"].ToString()));

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));



                    string PriceTable = "";




                    //if (dsBgDisc != null)
                    //{
                    //    if (dsBgDisc.Tables[0].Rows.Count > 0)
                    //    {
                    //        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                    //        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                    //        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                    //        //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                    //        bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                    //        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                    //        {
                    //            //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                    //        }
                    //    }
                    //}
                    if (Convert.ToInt32(userid) > 0)
                    {

                        PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
                    }
                    _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);


                    string[] catpath = null;
                    string categorypath = string.Empty;
                    if (drpid["CATEGORY_PATH"].ToString() != null)
                    {

                        catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                        categorypath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >= 2 ? catpath[1] : " ");
                    }


                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());


                        //modified by :indu
                        string newurl = string.Empty;
                        newurl = WAG_Root_Path + "////" + categorypath + "////" +
                                          drpid["FAMILY_NAME"] + "////" + drpid["FAMILY_ID"];


                        objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "fl.aspx");


                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);

                        string newurl = string.Empty;


                        newurl = WAG_Root_Path + "////" + categorypath + "////" +
                                           drpid["FAMILY_NAME"] + "////" +
                                         drpid["PRODUCT_ID"] + "=" + drpid["PRODUCT_CODE"] + "////"
                                         + drpid["FAMILY_ID"];
                      

                        objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "mpd.aspx");

                    }


                    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                    else
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

                    if (Request.QueryString["byp"] != null && (Request.QueryString["byp"].ToString() == "2" || Request.QueryString["byp"].ToString() == "3"))
                    {
                        _stmpl_records.SetAttribute("BYP", Request.QueryString["byp"].ToString());
                    }

                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"].ToString());

                    if (drpid["PRODUCT_CODE"].ToString().Length <= 8)
                    {
                        _stmpl_records.SetAttribute("TBT_ORG_ORDERNO", true);
                    }
                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", objhelper.SetImageFolderPath(drpid["Prod_Thumbnail"].ToString().Replace("\\", "/"), "_th", "_Images_200"));
                    //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                    //if (Fil.Exists)
                    //{
                    //    //Modified by:Indu
                    //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", objhelper.SetImageFolderPath(drpid["Prod_Thumbnail"].ToString().Replace("\\", "/"), "_th", "_Images_200"));
                    //    //_stmpl_records.SetAttribute("ATTRIBUTE_ID_452", drpid["Prod_Thumbnail"].ToString());
                    //}
                    //else
                    //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", "/images/noimage.gif");


                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_5", drpid["PRODUCT_PRICE"].ToString());


                    _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"].ToString());
                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());



                    string desc = "";
                    string descattr = "";
                    string prod_desc_alt = "";
                    if (_ViewType == "LV")
                    {
                        if (drpid["PRODUCT_COUNT"].ToString() != "1")
                        {
                            desc = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                            desc = desc + " " + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        }
                        else
                            desc = drpid["Prod_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        prod_desc_alt = drpid["Prod_Description"].ToString();
                        if (prod_desc_alt.Length > 0)
                        {
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                            _stmpl_records.SetAttribute("PROD_DESC_TITLE", prod_desc_alt.Replace('"', ' '));
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"].ToString());
                            _stmpl_records.SetAttribute("PROD_DESC_TITLE", drpid["FAMILY_NAME"].ToString().Replace('"', ' '));
                        }
                        if (desc.Length > 250)
                        {
                            _stmpl_records.SetAttribute("TBT_MORE_62", true);
                            desc = desc.Substring(0, 230).ToString();
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_62", desc);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_62", desc);
                            _stmpl_records.SetAttribute("TBT_MORE_62", false);
                        }

                   

                    }
                    else
                    {
                        if (drpid["PRODUCT_COUNT"].ToString() != "1")
                        {
                            descattr = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                            if (descattr.Length > 0)
                                descattr = descattr + "<br/>";
                            descattr = descattr + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        }
                        else
                            descattr = drpid["Prod_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        prod_desc_alt = drpid["Prod_Description"].ToString();
                        if (prod_desc_alt.Length > 0)
                        {
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                            _stmpl_records.SetAttribute("PROD_DESC_TITLE", prod_desc_alt.Replace('"', ' '));
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"].ToString());
                            _stmpl_records.SetAttribute("PROD_DESC_TITLE", drpid["FAMILY_NAME"].ToString().Replace('"', ' '));
                        }
                        if (descattr.Length > 140 )
                        {
                            int count = 0;
                            count = descattr.Count(c => char.IsUpper(c));
                            if (count >= 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 120).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 140).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                        }
                        else
                        {


                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                        }
                    }



                    //DataRow[] drAry = dscat.Tables[2].Select("FAMILY_ID ='" + drpid["FAMILY_ID"].ToString() + "' AND " + "PRODUCT_ID ='" + drpid["PRODUCT_ID"].ToString() + "'");
                    //if (drAry.Length > 0)
                    //{
                    //    string descattr = "";
                    //    string desc = "";
                    //    string proddesc = string.Empty;
                    //    foreach (DataRow dr in drAry.CopyToDataTable().Rows)
                    //    {


                    //        if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
                    //        {
                    //            if (dr["ATTRIBUTE_ID"].ToString() == "1")
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
                    //            }
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
                    //            }

                    //            if (dr["ATTRIBUTE_TYPE"].ToString() == "1" && dr["ATTRIBUTE_NAME"].ToString() == "PROD_DSC")
                    //            {
                    //                proddesc = dr["STRING_VALUE"].ToString();
                    //            }
                    //        }

                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
                    //        {

                    //            desc = dr["STRING_VALUE"].ToString().Replace("\r\n", "").Replace("\r", "&nbsp;").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                    //            descattr = descattr + " " + desc;
                    //            if (proddesc.Trim() == "")
                    //            {
                    //                proddesc = descattr;
                    //            }
                    //            if (desc.Length > 250 && _ViewType == "LV")
                    //            {
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //                desc = desc.Substring(0, 250).ToString();

                    //                if (drpid["PRODUCT_COUNT"].ToString() != "1")
                    //                {
                    //                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //                }
                    //                else
                    //                {
                    //                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), proddesc);
                    //                }
                    //            }
                    //            //else if (desc.Length > 30 && _ViewType == "GV")
                    //            //{
                    //            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //            //    desc = desc.Substring(0, 30).ToString();
                    //            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            //}
                    //            else
                    //            {
                    //                if (drpid["PRODUCT_COUNT"].ToString() != "1")
                    //                {
                    //                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //                }
                    //                else
                    //                {
                    //                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), proddesc);
                    //                }
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
                    //            }

                    //            //_stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"]);
                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
                    //        {
                    //            if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.FixDecPlace(Convert.ToDecimal(dr["NUMERIC_VALUE"])).ToString());
                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
                    //        {
                    //            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
                    //            if (Fil.Exists)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                    //            }

                    //        }

                    //    }
                    //    if (drpid["PRODUCT_COUNT"].ToString() != "1")
                    //    {

                    //        if (descattr.Length > 86 && _ViewType == "GV")
                    //        {
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //            descattr = descattr.Substring(0, 86).ToString();
                    //            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //        }
                    //        else
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //    }
                    //    else
                    //    {
                    //        if (proddesc.Trim() != "")
                    //        {
                    //            if (proddesc.Length > 86 && _ViewType == "GV")
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //                proddesc = proddesc.Substring(0, 86).ToString();
                    //                proddesc = proddesc.Substring(0, proddesc.LastIndexOf(" "));
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC", proddesc);
                    //            }
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC", proddesc);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (descattr.Length > 86 && _ViewType == "GV")
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //                descattr = descattr.Substring(0, 86).ToString();
                    //                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //            }
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //            }
                    //        }

                    //    }
                    //}
                    ////ADDED BY :INDU
                    //if (_stmpl_records.GetAttribute("TBT_SUB_FAMILY").ToString() == "False")
                    //{
                    //    string newurl = string.Empty;

                    //    newurl = _BCEAPath + "////" + drpid["FAMILY_ID"].ToString() + "=" + _stmpl_records.GetAttribute("FAMILY_NAME") + "////" +
                    //       drpid["PRODUCT_ID"].ToString() + "=" + _stmpl_records.GetAttribute("PRODUCT_CODE");
                    //    if (newurl.ToString().EndsWith(".") == true)
                    //    {
                    //        newurl = newurl.Substring(0, newurl.Length - 1);
                    //    }
                    //    objHelperServices.Cons_NewURl(_stmpl_records, newurl, "pd.aspx", true, "");
                    //}
                    ////END

                    lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());

                    icolstart++;

                    if (icolstart >= icol || oe == dscat.Tables["FamilyPro"].Rows.Count)
                    {
                        _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "producttable_" + soddevenrow);
                        _stmpl_container.SetAttribute("TBT_SUBCATNAME", subcatname_l1_l2);
                        _stmpl_container.SetAttribute("TBT_CATEGORY_NAME", catheader);
                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        ictrecords++;
                        icolstart = 0;
                        lstrows = new TBWDataList[icol];

                    }

                }

                _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);

                if (Request.QueryString["ViewMode"] != null)
                {
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    isreadonly.SetValue(this.Request.QueryString, false, null);
                    this.Request.QueryString.Remove("ViewMode");
                }
                string productlistURL = Request.QueryString.ToString();
                string powersearchURListView = "";
                string powersearchURLGridView = "";
                powersearchURListView = "/" + productlistURL + "&ViewMode=LV";
                powersearchURLGridView = "/" + productlistURL + "&ViewMode=GV";




                _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "main");



                _stmpl_container.SetAttribute("TBT_TOSUITE_BRAND", _tsb);
                _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL", _tsm);


                //modify by palani
                DataSet dscatebybrand = new DataSet();
                DataSet dssubcatbybrand = new DataSet();
                DataSet breadcrumb = new DataSet();
                StringTemplateGroup _stg_records_container = null;
                StringTemplate _stmpl_records_tmpl = null;
                StringTemplateGroup _stg_records_container1 = null;
                StringTemplate _stmpl_records_tmpl1 = null;
                TBWDataList1[] lstrecords11 = new TBWDataList1[0];
                TBWDataList1[] lstrows11 = new TBWDataList1[0];
                TBWDataList2[] lstrecords12 = new TBWDataList2[0];
                int icolstart11 = 0;
                int ictrecords11;
                int ictrecords12;
                string tempCID = "";
                string subrealBCEAPath = "";
                dssubcatbybrand = (DataSet)HttpContext.Current.Session["dssubcatbybrand"];
                dscatebybrand = (DataSet)HttpContext.Current.Session["dscatbybrand"];
                breadcrumb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                string catvalue = "";


                if (dscatebybrand != null && dscatebybrand.Tables.Count > 0 && dscatebybrand.Tables[0].TableName.Contains("Category") && dscatebybrand.Tables[0].Rows.Count > 0 && breadcrumb.Tables[0].Rows.Count > 2)
                {
                    if (Request.QueryString["cid"] == null)
                        tempCID = "WES0830";
                    else
                        tempCID = Request.QueryString["cid"].ToString();
                    if (_cid != "")
                        _ParentCatID = GetParentCatID(_cid);

                    DataRow[] _DCRow = null;
                    _DCRow = dscatebybrand.Tables[0].Select();

                    if (_DCRow != null && _DCRow.Length > 0)
                    {
                        ictrecords11 = 0;
                        lstrecords11 = new TBWDataList1[_DCRow.Length + 1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Category");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Category");
                        _stmpl_records_tmpl.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
                        lstrecords11[ictrecords11] = new TBWDataList1(_stmpl_records_tmpl.ToString());
                        ictrecords11++;
                        //_stg_container1 = new StringTemplateGroup("searchrsltproductmain", stemplatepath);
                        //_stmpl_container1 = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "main_sub");

                        foreach (DataRow _drow in _DCRow)
                        {
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["Category_Name"].ToString());
                            //  _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            _stmpl_records_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                            string eapath = "";
                            string subcatvalue = "";
                            if (breadcrumb.Tables[0].Rows.Count > 0 && breadcrumb.Tables[0].Rows.Count > 3)
                            {
                                catvalue = breadcrumb.Tables[0].Rows[3]["ItemValue"].ToString();
                                if (catvalue.ToUpper() == _drow["Category_Name"].ToString().ToUpper())
                                {
                                    _stmpl_records_tmpl.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
                                    // new line added
                                    //_stmpl_container.SetAttribute("TBW_CATEGORY_NAME", catvalue);
                                }
                            }

                            if (breadcrumb != null)
                                eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb.Tables[0].Rows[2]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));

                            //Modified by:indu
                            //DataTable dt = breadcrumb.Tables[0];
                            //DataRow[] foundRows;
                            //string cat = "Category";

                            //foundRows = dt.Select("ItemType='" + cat + "' ");
                            //string parentcat = string.Empty;
                            //if (foundRows.Length > 0)
                            //{
                            //    parentcat = foundRows[0]["Itemvalue"].ToString();
                            //    if (subcatvalue == "")
                            //    {
                            //        parentcat = "////Category=" + parentcat;
                            //    }
                            //    else
                            //    {
                            //        parentcat = "////Category=" + subcatvalue;
                            //    }
                            //}
                            //string href = _realBCEAPath + parentcat + "////" + "Category=" + _drow["Category_Name"].ToString();

                            //string href = "";
                            //if (catvalue == "")
                            //    href = _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();
                            //else
                            //{
                            //    //href = "";
                            //    string _realBCEAPathmc = "////" + " " + "////" + " " + "////" + "CELLULAR ACCESSORIES" + "////" + _drow["brandvalue"].ToString().ToUpper() + "////" + _tsm.ToUpper();
                            //    //.Replace("+","||");
                            //    href = _realBCEAPathmc + "////" + "Category=" + _drow["Category_Name"].ToString();
                            //}
                            //subrealBCEAPath = _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();
                            //  subrealBCEAPath = _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _realBCEAPathmc + "@@category@@" + _drow["Category_Name"]);
                            //_stmpl_records_tmpl.SetAttribute("href", "bb.aspx?"+href);
                          //  _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "/" + href + "/bb/");

                            lstrecords11[ictrecords11] = new TBWDataList1(_stmpl_records_tmpl.ToString());
                            ictrecords11++;
                        }
                        _stmpl_container.SetAttribute("TBWDataList1", lstrecords11);
                        _stmpl_container.SetAttribute("TBW_SELECTED_ID", "1");

                    }

                }
                if (dssubcatbybrand != null && dssubcatbybrand.Tables.Count > 0 && dssubcatbybrand.Tables[0].TableName.Contains("Category") && dssubcatbybrand.Tables[0].Rows.Count > 0 && breadcrumb.Tables[0].Rows.Count > 3)
                {
                    if (dssubcatbybrand != null && dssubcatbybrand.Tables.Count > 0 && dssubcatbybrand.Tables[0].TableName.Contains("Category"))
                    {
                        if (Request.QueryString["cid"] == null)
                            tempCID = "WES0830";
                        else
                            tempCID = Request.QueryString["cid"].ToString();
                        if (_cid != "")
                            _ParentCatID = GetParentCatID(_cid);

                        DataRow[] _DCRow1 = null;
                        _DCRow1 = dssubcatbybrand.Tables[0].Select();

                        if (_DCRow1 != null && _DCRow1.Length > 0)
                        {
                            ictrecords12 = 0;
                            lstrecords12 = new TBWDataList2[_DCRow1.Length + 1];
                            _stg_records1 = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container1 = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl1 = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "multilistitem1");
                            _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL1", "Select Category");
                            _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL", "Select Category");
                            _stmpl_records_tmpl1.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
                            lstrecords12[ictrecords12] = new TBWDataList2(_stmpl_records_tmpl1.ToString());
                            ictrecords12++;
                            foreach (DataRow _drow in _DCRow1)
                            {
                                _stg_records1 = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                                _stg_records_container1 = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                                _stmpl_records_tmpl1 = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "multilistitem1");
                                _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL", _drow["Category_Name"].ToString());
                                // _stmpl_records_tmpl1.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                _stmpl_records_tmpl1.SetAttribute("TBW_SELECTED_ID", "1");
                                string eapath = "";
                                string subcatvalue = "";
                                if (breadcrumb.Tables[0].Rows.Count > 0 && breadcrumb.Tables[0].Rows.Count > 4)
                                {
                                    subcatvalue = breadcrumb.Tables[0].Rows[4]["ItemValue"].ToString();
                                    if (subcatvalue.ToUpper() == _drow["Category_Name"].ToString().ToUpper())
                                    {
                                        _stmpl_records_tmpl1.SetAttribute("TBW_SELECTED", "SELECTED=\"SELECTED\" ");
                                        // new line added
                                        // _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", subcatvalue);
                                    }
                                }

                                if (breadcrumb != null)
                                    eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb.Tables[0].Rows[3]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
                                //string href = "bb.aspx?&Id=0&pcr=" + HttpUtility.UrlEncode(tempCID) + "&cid=" + HttpUtility.UrlEncode(_drow["CATEGORY_ID"].ToString()) + "&searchstr=&bname=" + HttpUtility.UrlEncode(_drow["brandvalue"].ToString()) + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&type=Category&tsm=" + HttpUtility.UrlEncode(_tsm) + "&byp=2&path=" + eapath;
                                ////_stmpl_records_tmpl1.SetAttribute("href", href);

                                //////   _stmpl_records_tmpl1.SetAttribute("href", "bb.aspx?"+href);
                                //_stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL1", href);
                                DataTable dt = breadcrumb.Tables[0];
                                DataRow[] foundRows;
                                string cat = "Category";
                                string nocat = "Cellular Accessories";
                                foundRows = dt.Select("ItemType='" + cat + "' and Itemvalue<>'" + nocat + "'");
                                string parentcat = string.Empty;
                                if (foundRows.Length > 0)
                                {
                                    parentcat = foundRows[0]["Itemvalue"].ToString();
                                    if (subcatvalue == "")
                                    {
                                        parentcat = "////Category=" + parentcat;
                                    }
                                    else
                                    {
                                        parentcat = "";
                                        parentcat = "////Category=" + subcatvalue;
                                    }
                                }
                                string _realBCEAPath1 = _realBCEAPathmc + "////" + catvalue.ToUpper();
                                // href = _realBCEAPath1 + "////" + "Category=" + _drow["Category_Name"].ToString();
                                _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL1", _realBCEAPath1 + "@@Category@@" + _drow["Category_Name"].ToString());
                                // string href = _realBCEAPath + parentcat +"////" + "Category=" + _drow["Category_Name"].ToString();
                                //string href = _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();

                                //_realBCEAPath= "AllProducts" + "////" + "WESAUSTRALASIA" + "////" + "CELLULAR ACCESSORIES" + "////" + "AttribSelect=Brand=" + _drow["brandvalue"].ToString().ToUpper() + "////" + "AttribSelect=Model=" + _tsm.ToUpper();
                                //string href = _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();
                                //string href = "";
                                //if (subcatvalue == "")
                                //    href = _realBCEAPath + "////" + "Category=" + _drow["Category_Name"].ToString();
                                //else
                                //{
                                //    //.Replace("+","||")
                                //    string _realBCEAPath1 = "////" + " " + "////" + " " + "////" + "CELLULAR ACCESSORIES" + "////" + _drow["brandvalue"].ToString().ToUpper() + "////" + HttpUtility.UrlEncode(_tsm.ToUpper()) + "////" + "Category=" + catvalue.ToUpper();
                                //    href = _realBCEAPath1 + "////" + "Category=" + _drow["Category_Name"].ToString();
                                //}
                               // string rel = "bb.aspx?&Id=0&pcr=" + HttpUtility.UrlEncode(tempCID) + "&cid=" + HttpUtility.UrlEncode(_drow["CATEGORY_ID"].ToString()) + "&searchstr=&bname=" + HttpUtility.UrlEncode(_drow["brandvalue"].ToString()) + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&type=Category&tsm=" + HttpUtility.UrlEncode(_tsm) + "&byp=2&path=" + eapath;
                               // href = objHelperServices.Cons_NewURl_bybrand_MS(rel, href, "bb.aspx", "Category");
                                //   _stmpl_records_tmpl1.SetAttribute("href", "bb.aspx?"+href);
                              
                              //  _stmpl_records_tmpl1.SetAttribute("TBW_LIST_VAL1", "/" + href + "/bb/");


                                lstrecords12[ictrecords12] = new TBWDataList2(_stmpl_records_tmpl1.ToString());
                                ictrecords12++;
                            }
                            _stmpl_container.SetAttribute("TBWDataList2", lstrecords12);
                            _stmpl_container.SetAttribute("TBW_SELECTED_ID", "2");

                            //if (dssubcatbybrand != null)
                            //    dssubcatbybrand = null;
                        }

                    }

                }
                if (dscatebybrand != null)
                {
                    if (dscatebybrand.Tables[0].Rows.Count > 0)
                        _stmpl_container.SetAttribute("TBT_SHOW_FT", true);
                    else
                        _stmpl_container.SetAttribute("TBT_SHOW_FT", false);
                }
                if (breadcrumb != null)
                {
                    int i;
                    string chk = "";
                    bool flgcheck = false;
                    for (i = 0; i < breadcrumb.Tables[0].Rows.Count; i++)
                    {
                        chk = breadcrumb.Tables[0].Rows[i]["ItemType"].ToString();
                        if (chk == "Type")
                        {
                            flgcheck = true;
                        }
                    }
                    if (dssubcatbybrand != null)
                    {
                        if (flgcheck == false && dssubcatbybrand.Tables[0].Rows.Count > 0)
                            _stmpl_container.SetAttribute("TBT_SHOW_FTSCat", true);
                        else
                            _stmpl_container.SetAttribute("TBT_SHOW_FTSCat", false);
                    }
                }
                //end


                if (Tosuit_ModelImage != "")
                {
                    _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL_IMAGE", Tosuit_ModelImage.ToString().Replace("\\", "/"));
                    //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + Tosuit_ModelImage.Replace("/", "\\"));
                    //if (Fil.Exists)
                    //{
                    //    _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL_IMAGE", Tosuit_ModelImage.ToString().Replace("\\", "/"));
                    //}
                    //else
                    //{
                    //    _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL_IMAGE", "/images/noimage.gif");
                    //}
                }
                else
                {

                    _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL_IMAGE", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()+"images/noimage.gif");
                }
                _stmpl_container.SetAttribute("TBW_PRODUCT_COUNT", iTotalProducts);

                _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh.ToUpper());

                _stmpl_container.SetAttribute("TBW_URL", powersearchURListView);
                _stmpl_container.SetAttribute("TBW_URL1", powersearchURLGridView);

                if (_ViewType == "LV")
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
                else
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);


              
                string nextstring = "";
                try
                {
                    if (Request.QueryString["srctext"].ToString() != "")
                    {
                        //  nextstring = "&srctext=" + Request.QueryString["srctext"].ToString().Replace("\"", "%22").Replace("&", "%26").Replace("#", "%23");
                        nextstring = Request.QueryString["srctext"].ToString().Replace("\"", "%22").Replace("&", "%26").Replace("#", "%23");
                    }
                }
                catch (Exception) { }
                //_stmpl_container.SetAttribute("TBW_START_PAGE_NO", (iPageNo * iRecordsPerPage) - (iRecordsPerPage - 1));

                //if ((iPageNo * iRecordsPerPage) > iTotalProducts)
                //    _stmpl_container.SetAttribute("TBW_END_PAGE_NO", iTotalProducts);
                //else
                //    _stmpl_container.SetAttribute("TBW_END_PAGE_NO", (iPageNo * iRecordsPerPage));
                //_stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
                //_stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo + nextstring);
                //_stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
                //_stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo + nextstring);
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");


                //if (sHTML.ToString().Contains("src=\"/prodimages\""))
                //{
                //    sHTML = sHTML.Replace("src=\"/prodimages\"", "src=\"/prodimages/images/noimage.gif\"");
                //    sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                //}
                //if (sHTML.ToString().Contains("src=\"\""))
                //{
                //    sHTML = sHTML.ToString().Replace("src=\"\"", "src=\"/prodimages/images/noimage.gif\"");
                //    sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                //}

                if (sHTML.ToString().Contains("data-original=\"/prodimages\""))
                {
                    sHTML = sHTML.Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
                    sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                }
                if (sHTML.ToString().Contains("data-original=\"\""))
                {
                    sHTML = sHTML.ToString().Replace("data-original=\"\"", "data-original=\"/prodimages/images/noimage.gif\"");
                    sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                }
                if (dscat.Tables[1].Rows[0].ItemArray[0].ToString() == "0")
                    sHTML = "";
                if (dscat.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
                {
                    sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1" + nextstring + "\">Previous</a>", "");
                    sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1" + nextstring + "\">Next</a>", "");
                }
                if (Convert.ToInt32(dscat.Tables[0].Rows[0].ItemArray[0].ToString()) == iPageNo)
                {
                    sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1" + nextstring + "\">Next</a>", "");
                }


            }
            string eapath1 = _EAPath.Replace("'", "###");
            string BCEAPath = _BCEAPath.Replace("'", "###");
            htmleapath.Value = eapath1;
            htmlbceapth.Value = BCEAPath;
            htmltotalpage.Value = iTotalPages.ToString();
            htmlviewmode.Value = _ViewType;
            if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
            {
                htmlirecords.Value = Session["RECORDS_PER_PAGE_BYBRAND"].ToString();
            }
            else
            {
                htmlirecords.Value = "24";
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
            //if (oCon != null)
            //{
            //oCon.Close();
            //}
        }
        //}

        sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
        return objHelperServices.StripWhitespace(sHTML);

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
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        catIDtemp = DR["PARENT_CATEGORY"].ToString();
                        if (catIDtemp == "0" || catIDtemp == "WES-SPF")
                        {
                            // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                            return DR["CATEGORY_ID"].ToString();
                        }
                    }
                }
            } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
            return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        }
        catch (Exception ex)
        {
            return "";
        }
        return "";
    }
    //protected string ST_BrandModelProductList()
    //{
    //    string _tempfid = "";
    //    string category_nameh = "";
    //    string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
    //    //if (Session["PS_SEARCH_RESULTS"] == null)
    //    //{
    //    //    return "";
    //    //}
    //    if (Request["srctext"] != null && Request["srctext"].ToString() == "")
    //    {
    //        return "CLEAR";
    //    }
    //    else if (Request["srctext"] != null)
    //    {
    //        _SearchString = Request["srctext"].ToString();

    //    }
    //    FamilyServices objFamilyServices = new FamilyServices();
    //    StringTemplateGroup _stg_container = null;
    //    StringTemplateGroup _stg_records = null;
    //    StringTemplate _stmpl_container = null;
    //    StringTemplate _stmpl_records = null;
    //    StringTemplate _stmpl_pages = null;
    //    string subcatname_l1 = "";
    //    string subcatname_l2 = "";
    //    string subcatname_l1_l2 = "";
    //    string catname = "";
    //    string catheader = "";
    //    bool subcatdispflag = false;
    //    bool catdispflag = false;
    //    int oe = 0;

    //    string sHTML = "";

    //    //if (Convert.ToInt32(Session["PS_SEARCH_RESULTS"].ToString()) > 0)
    //    // {
    //    try
    //    {


    //        if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
    //            _tsm = Request.QueryString["tsm"];

    //        if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
    //            _tsb = Request.QueryString["tsb"];

    //        if (Request.QueryString["type"] != null)
    //            _type = Request.QueryString["type"];

    //        if (Request.QueryString["value"] != null)
    //            _value = Request.QueryString["value"];

    //        if (Request.QueryString["bname"] != null)
    //            _bname = Request.QueryString["bname"];
    //        if (Request.QueryString["searchstr"] != null)
    //            _searchstr = Request.QueryString["searchstr"];
    //        if (Request.QueryString["srctext"] != null)
    //            _searchstr = Request.QueryString["srctext"];

    //        if (Request.QueryString["cid"] != null)
    //            _cid = Request.QueryString["cid"];

    //        if (Request.QueryString["pcr"] != null)
    //            _pcr = Request.QueryString["pcr"];

    //        //if (Request.QueryString["path"] != null)
    //        //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(Request.QueryString["path"].ToString()));


    //        if (HttpContext.Current.Session["EA"] != null)
    //            _EAPath = HttpContext.Current.Session["EA"].ToString();


    //        dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
    //        if (dscat == null)
    //        {
    //            return "";
    //        }
    //        else if (dscat.Tables[2].Rows.Count == 0)
    //        {
    //            return "";
    //        }
    //        string Tosuit_ModelImage = "";
    //        DataSet dsModel = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
    //        if (dsModel != null)
    //        {
    //            DataRow[] dr = dsModel.Tables[0].Select("TOSUITE_MODEL='" + _tsm + "'");
    //            if (dr.Length > 0)
    //                Tosuit_ModelImage = dr.CopyToDataTable().Rows[0]["IMAGE_FILE"].ToString();

    //        }
    //        else
    //            Tosuit_ModelImage = (string)objHelperDB.GetGenericPageDataDB("", _tsb, _tsm, "GET_BYBRAND_MODEL_IMAGE", HelperDB.ReturnType.RTString);


    //        TBWDataList1[] lstrecords = new TBWDataList1[0];
    //        TBWDataList[] lstrows = new TBWDataList[0];
    //        _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);




    //        int ictrecords = 0;
    //        int icolstart = 0;
    //        int icol = 1;
    //        lstrows = new TBWDataList[icol];
    //        if (dscat.Tables[0].Rows.Count < icol)
    //        {
    //            icol = dscat.Tables[0].Rows.Count;
    //        }
    //        string soddevenrow = "odd";
    //        DataTable dtCatcoll = null;
    //        DataRow[] drprodcoll = null;


    //        int ProductTotalcount = 0;
    //        int pricecode = 0;
    //        string userid = "";
    //        DataSet dsBgDisc = new DataSet();
    //        DataSet dsPriceTableAll = new DataSet();
    //        string tmpProds = "";

    //        dtCatcoll = dscat.Tables[3].Copy();

    //        lstrecords = new TBWDataList1[dtCatcoll.Rows.Count + 1];
    //        if (Session["USER_ID"] != null)
    //            userid = Session["USER_ID"].ToString();
    //        if (userid == "")
    //            userid = "0";

    //        string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
    //        if (Convert.ToInt32(userid) > 0)
    //        {

    //            dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
    //        }
    //        else
    //        {
    //            dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
    //        }
    //        pricecode = objHelperDB.GetPriceCode(userid);

    //        foreach (DataRow Catdr in dtCatcoll.Rows)
    //        {

    //            drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1 And CATEGORY_NAME='" + Catdr["CATEGORY_NAME"].ToString() + "'");





    //            if (drprodcoll.Length > 0)
    //            {
    //                lstrows = new TBWDataList[drprodcoll.Length + 1];

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

    //                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_" + soddevenrow);


    //                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));

    //                    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
    //                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
    //                    else
    //                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

    //                    _stmpl_records.SetAttribute("PRODUCT_ID", drpid["PRODUCT_ID"].ToString());

    //                    //_stmpl_records.SetAttribute("MIN_ORD_QTY", oProd.GetMinOrdQty(Convert.ToInt32(drpid["PRODUCT_ID"].ToString())).ToString());
    //                    _stmpl_records.SetAttribute("MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());

    //                    _stmpl_records.SetAttribute("FAMILY_NAME", drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
    //                    _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace('"', ' '));
    //                    _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
    //                    _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());
    //                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());
    //                    _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"].ToString());
    //                    _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"].ToString());
    //                    int _pid = System.Convert.ToInt32(drpid["PRODUCT_ID"].ToString());

    //                    //_stmpl_records.SetAttribute("TBT_USER_PRICE", GetMyPrice(_pid));
    //                    _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["NUMERIC_VALUE"].ToString());

    //                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));

    //                    string PriceTable = "";




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

    //                        PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
    //                    }
    //                    _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);



    //                    DataRow[] drAry = dscat.Tables[2].Select("FAMILY_ID ='" + drpid["FAMILY_ID"].ToString() + "' AND " + "PRODUCT_ID ='" + drpid["PRODUCT_ID"].ToString() + "' AND " + "SUB_FAMILY_ID ='" + drpid["SUB_FAMILY_ID"].ToString() + "'");
    //                    if (drAry.Length > 0)
    //                    {

    //                        foreach (DataRow dr in drAry.CopyToDataTable().Rows)
    //                        {
    //                            if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"]);
    //                            }
    //                            else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
    //                            {
    //                                if (dr["NUMERIC_VALUE"].ToString().Length > 0)
    //                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.FixDecPlace(Convert.ToDecimal(dr["NUMERIC_VALUE"])).ToString());
    //                            }
    //                            else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
    //                            {
    //                                System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString());
    //                                if (Fil.Exists)
    //                                {
    //                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));//);
    //                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString() + "_LARGE", objHelperServices.SetImageFolderPath(dr["STRING_VALUE"].ToString(), "_Th", "_Images").ToString().Replace("\\", "/"));//);
    //                                }
    //                                else
    //                                {
    //                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
    //                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString() + "_LARGE", "");//);
    //                                }

    //                            }
    //                        }
    //                    }


    //                    lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());

    //                    icolstart++;
    //                    if (icolstart == drprodcoll.Length)
    //                    {

    //                        ProductTotalcount = ProductTotalcount + drprodcoll.Length;
    //                        _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
    //                        _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "Catmain");
    //                        _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", drpid["SUBCATNAME_L1"].ToString().ToUpper());

    //                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
    //                        if (drprodcoll.Length == 10)
    //                        {
    //                            _stmpl_container.SetAttribute("TBT_DISPLAY_MORE_DIV", "block");
    //                            _stmpl_container.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(drpid["CATEGORY_ID"].ToString()));
    //                            _stmpl_container.SetAttribute("TBW_BRAND", HttpUtility.UrlEncode(_tsb));
    //                            _stmpl_container.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(_tsm));
    //                            _stmpl_container.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(_searchstr));
    //                            _stmpl_container.SetAttribute("TBW_ATTRIBUTE_TYPE", "Category");
    //                            _stmpl_container.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(drpid["SUBCATNAME_L1"].ToString()));
    //                            _stmpl_container.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(_tsb));
    //                            _stmpl_container.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
    //                            _stmpl_container.SetAttribute("TBT_PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());
    //                            _stmpl_container.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));
    //                        }
    //                        else
    //                            _stmpl_container.SetAttribute("TBT_DISPLAY_MORE_DIV", "none");

    //                        lstrecords[ictrecords] = new TBWDataList1(_stmpl_container.ToString());
    //                        ictrecords++;
    //                        icolstart = 0;

    //                    }

    //                }
    //            }
    //        }
    //        _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);
    //        _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "main");

    //        _stmpl_container.SetAttribute("TBT_TOSUITE_BRAND", _tsb);
    //        _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL", _tsm);


    //        if (Tosuit_ModelImage != "")
    //        {
    //            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + Tosuit_ModelImage.Replace("/", "\\"));
    //            if (Fil.Exists)
    //            {
    //                _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL_IMAGE", Tosuit_ModelImage.ToString().Replace("\\", "/"));
    //            }
    //            else
    //            {
    //                _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL_IMAGE", "/images/noimage.gif");
    //            }
    //        }
    //        else
    //        {

    //            _stmpl_container.SetAttribute("TBT_TOSUITE_MODEL_IMAGE", "/images/noimage.gif");
    //        }



    //        _stmpl_container.SetAttribute("TBT_COUNT", ProductTotalcount.ToString());
    //        _stmpl_container.SetAttribute("TBWDataList1", lstrecords);
    //        sHTML = _stmpl_container.ToString();



    //    }
    //    catch (Exception ex)
    //    {
    //        sHTML = ex.Message;
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //    }
    //    finally
    //    {
    //        //if (oCon != null)
    //        //{
    //        //oCon.Close();
    //        //}
    //    }
    //    //}

    //    return sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
    //}


    //public string DynamicPag(string url, int ipageno, string eapath, string BCEAPath, string ViewMode, string irecords)
    //{
    //    HelperDB objHelperDB = new HelperDB();
    //    HelperServices objHelperServices = new HelperServices();
    //    ProductServices objProductServices = new ProductServices();
    //    ErrorHandler objErrorHandler = new ErrorHandler();
    //    Security objSecurity = new Security();
    //    //SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString());
    //    DataSet dscat = new DataSet();
    //    ConnectionDB objConnectionDB = new ConnectionDB();
    //    string _tempfid = "";
    //    string category_nameh = "";
    //    string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
    //    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    //    string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    //    stemplatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
    //  //  string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
    //    //if (HttpContext.Current.Session["PS_SEARCH_RESULTS"] == null)
    //    //{
    //    //    return "";
    //    //}
    //    if (HttpContext.Current.Request["srctext"] != null && HttpContext.Current.Request["srctext"].ToString() == "")
    //    {
    //        return "CLEAR";
    //    }
    //    else if (HttpContext.Current.Request["srctext"] != null)
    //    {
    //        _SearchString = HttpContext.Current.Request["srctext"].ToString();

    //    }
    //    FamilyServices objFamilyServices = new FamilyServices();
    //    StringTemplateGroup _stg_container = null;
    //    StringTemplateGroup _stg_records = null;
    //    StringTemplate _stmpl_container = null;
    //    StringTemplate _stmpl_records = null;
    //    StringTemplate _stmpl_pages = null;
    //    string subcatname_l1 = "";
    //    string subcatname_l2 = "";
    //    string subcatname_l1_l2 = "";
    //    string catname = "";
    //    string catheader = "";
    //    bool subcatdispflag = false;
    //    bool catdispflag = false;
    //    int oe = 0;
    //    string _ViewType = "";

    //    string sHTML = "";

    //    //if (Convert.ToInt32(HttpContext.Current.Session["PS_SEARCH_RESULTS"].ToString()) > 0)
    //    // {
    //    try
    //    {
    //        dscat = Get_Value_Breadcrum(ipageno, eapath, irecords);

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



    //        _ViewType = ViewMode;

    //        //if (HttpContext.Current.Request.QueryString["ViewMode"] != null)
    //        //{
    //        //    _ViewType = HttpContext.Current.Request.QueryString["ViewMode"];
    //        //    HttpContext.Current.Session["PL_VIEW_MODE"] = _ViewType;
    //        //}
    //        //else if (HttpContext.Current.Session["PL_VIEW_MODE"] != null && HttpContext.Current.Session["PL_VIEW_MODE"].ToString() != "")
    //        //    _ViewType = HttpContext.Current.Session["PL_VIEW_MODE"].ToString();
    //        //else
    //        //    _ViewType = "GV";




    //        //if (_ViewType != "")
    //        //    HttpContext.Current.Session["PS_VIEW_MODE"] = _ViewType;

    //        //if (HttpContext.Current.Session["PS_VIEW_MODE"] != null && HttpContext.Current.Session["PS_VIEW_MODE"] != "")
    //        //    _ViewType = HttpContext.Current.Session["PS_VIEW_MODE"].ToString();

    //        //if (HttpContext.Current.Request.QueryString["path"] != null)
    //        //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString()));

    //        _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();
    //        string _BCEAPath = string.Empty;
    //        _BCEAPath = BCEAPath;

    //        DataSet tmp = (DataSet)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTDataSet);
    //        if (tmp != null && tmp.Tables.Count >= 1 && tmp.Tables[0].Rows.Count >= 1)
    //        {

    //            category_nameh = tmp.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
    //        }
    //        DataSet dsfamprod = new DataSet();

    //      //  dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
    //        //dscat = ps.getwesproducts();
    //        if (dscat == null)
    //        {
    //            return "";
    //        }
    //        else if (dscat.Tables[2].Rows.Count == 0)
    //        {
    //            return "";
    //        }

    //        DataRow drpagect = dscat.Tables[0].Rows[0];
    //        DataRow drproductsct = dscat.Tables[1].Rows[0];
    //        if (dsfamprod.Tables != null && dsfamprod.Tables.Count > 0 && HttpContext.Current.Request.QueryString["fid"] != null && HttpContext.Current.Request.QueryString["fid"].ToString() != "" && HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx"))
    //        {
    //            iTotalProducts = dsfamprod.Tables[0].Rows.Count;
    //            if (iRecordsPerPage >= iTotalProducts)
    //            {
    //                iTotalPages = 1;
    //            }
    //            else
    //            {
    //                double recordpg = iRecordsPerPage;
    //                double totalpg = (iTotalProducts / recordpg);
    //                if (totalpg > Convert.ToInt32(totalpg))
    //                    iTotalPages = Convert.ToInt32(totalpg) + 1;
    //                else
    //                    iTotalPages = Convert.ToInt32(totalpg);
    //            }
    //        }
    //        else
    //        {
    //            iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);
    //            iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
    //        }
    //        HttpContext.Current.Session["iTotalPages"] = iTotalPages;

    //        TBWDataList[] lstrecords = new TBWDataList[0];
    //        TBWDataList[] lstrows = new TBWDataList[0];
    //        _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);

    //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx") && _tempfid.Length > 0)
    //        {
    //            lstrecords = new TBWDataList[dsfamprod.Tables[0].Rows.Count + 1];
    //        }
    //        else
    //        {
    //            lstrecords = new TBWDataList[dscat.Tables[2].Rows.Count + 1];
    //        }
    //        int ictrecords = 0;
    //        int icolstart = 0;
    //        int icol = 1;
    //        lstrows = new TBWDataList[icol];

    //        if (_ViewType == "GV")
    //        {
    //            icol = 4;
    //            lstrows = new TBWDataList[icol];
    //        }
    //        else
    //        {
    //            icol = 1;
    //            lstrows = new TBWDataList[icol];
    //        }
    //        //if (dscat.Tables[0].Rows.Count < icol)
    //        //{
    //        //    icol = dscat.Tables[0].Rows.Count;
    //        //}
    //        string soddevenrow = "odd";
    //        DataRow[] drprodcoll = null;


    //        drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1", "SNO");

    //        string trmpstr = "";
    //        int ProductTotalcount = 0;
    //        int pricecode = 0;
        
    //        ////if (HttpContext.Current.Session["USER_ID"] != null)
    //        //    userid = HttpContext.Current.Session["USER_ID"].ToString();

    //        //if (userid == "")

          
    //        string userid = "";
    //        if (HttpContext.Current.Session["USER_ID"] != null)
    //        {
    //            userid = HttpContext.Current.Session["USER_ID"].ToString();
    //        }
    //        //if (userid == "")
    //        //{
    //        //    userid = objHelperServices.CheckCredential();
    //        //}

    //        if (userid == "")
    //        {
    //            userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
    //        }
    //             DataSet dsBgDisc = new DataSet();
    //        DataSet dsPriceTableAll = new DataSet();
    //        string tmpProds = "";

    //        string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
    //        if (Convert.ToInt32(userid) > 0)
    //        {

    //            dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
    //        }
    //        else
    //        {
    //            dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
    //        }

    //        pricecode = objHelperDB.GetPriceCode(userid);

    //        if (drprodcoll.Length > 0)
    //        {

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
    //                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_GridView");
    //                else
    //                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_" + soddevenrow);


    //                _stmpl_records.SetAttribute("TBT_ECOMENABLED", objHelperServices.GetIsEcomEnabled(userid));

    //                if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
    //                    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
    //                else
    //                    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

    //                _stmpl_records.SetAttribute("PRODUCT_ID", drpid["PRODUCT_ID"].ToString());

    //                //_stmpl_records.SetAttribute("MIN_ORD_QTY", oProd.GetMinOrdQty(Convert.ToInt32(drpid["PRODUCT_ID"].ToString())).ToString());
    //                _stmpl_records.SetAttribute("MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());



    //                trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
    //                if (_ViewType == "GV")
    //                {

    //                    if (trmpstr.Length > 60)
    //                        trmpstr = trmpstr.Substring(0, 60) + "...";
    //                }
    //                _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);

    //                _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
    //                _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());
    //                _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_CODE"].ToString());
    //                int _pid;
    //                if (drpid["PRODUCT_ID"].ToString() != null && drpid["PRODUCT_ID"].ToString() != "")
    //                {

    //                    _pid = System.Convert.ToInt32(drpid["PRODUCT_ID"].ToString());
    //                }

    //                //_stmpl_records.SetAttribute("TBT_USER_PRICE", GetMyPrice(_pid));
    //                _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["NUMERIC_VALUE"].ToString());
    //                _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());

    //                _stmpl_records.SetAttribute("CATEGORY_ID", HttpUtility.UrlEncode(drpid["CATEGORY_ID"].ToString()));

    //                _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

    //                _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));



    //                string PriceTable = "";




    //                if (dsBgDisc != null)
    //                {
    //                    if (dsBgDisc.Tables[0].Rows.Count > 0)
    //                    {
    //                        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
    //                        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
    //                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
    //                        //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
    //                        bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
    //                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
    //                        {
    //                            //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

    //                        }
    //                    }
    //                }
    //                if (Convert.ToInt32(userid) > 0)
    //                {

    //                    PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
    //                }
    //                _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);




    //                if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
    //                {
    //                    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
    //                    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
    //                    _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());

    //                    //Added by :indu
    //                    string newurl = _BCEAPath + "////" + drpid["FAMILY_ID"].ToString() + "=" + _stmpl_records.GetAttribute("FAMILY_NAME").ToString();

    //                    objHelperServices.Cons_NewURl(_stmpl_records, newurl, "fl.aspx", true, "", true);
    //                }
    //                else
    //                {
    //                    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
    //                    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);
    //                    //Added by :indu
    //                    string newurl = _BCEAPath + "////" + drpid["FAMILY_ID"].ToString() + "=" + _stmpl_records.GetAttribute("FAMILY_NAME").ToString() + "////" + drpid["PRODUCT_ID"].ToString() + "=" + _stmpl_records.GetAttribute("PRODUCT_CODE").ToString();
    //                    objHelperServices.Cons_NewURl(_stmpl_records, newurl, "pd.aspx", true, "", true);
    //                }



    //                if (HttpContext.Current.Request.QueryString["byp"] != null && (HttpContext.Current.Request.QueryString["byp"].ToString() == "2" || HttpContext.Current.Request.QueryString["byp"].ToString() == "3"))
    //                {
    //                    _stmpl_records.SetAttribute("BYP", HttpContext.Current.Request.QueryString["byp"].ToString());
    //                }
    //                DataRow[] drAry = dscat.Tables[2].Select("FAMILY_ID ='" + drpid["FAMILY_ID"].ToString() + "' AND " + "PRODUCT_ID ='" + drpid["PRODUCT_ID"].ToString() + "'");
    //                if (drAry.Length > 0)
    //                {
    //                    string descattr = "";
    //                    string desc = "";
    //                    foreach (DataRow dr in drAry.CopyToDataTable().Rows)
    //                    {
    //                        if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
    //                        {
    //                            if (dr["ATTRIBUTE_ID"].ToString() == "1")
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
    //                            else
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());

    //                        }
    //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
    //                        {
    //                            desc = dr["STRING_VALUE"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
    //                             descattr = descattr + " "+desc;
    //                            if (desc.Length > 250 && _ViewType == "LV")
    //                            {
    //                                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
    //                                desc = desc.Substring(0, 250).ToString();
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                            }
    //                            //else if (desc.Length > 30 && _ViewType == "GV")
    //                            //{
    //                            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
    //                            //    desc = desc.Substring(0, 30).ToString();
    //                            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                            //}
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
    //                            }

    //                            //_stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"]);
    //                        }
    //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
    //                        {
    //                            if (dr["NUMERIC_VALUE"].ToString().Length > 0)
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.FixDecPlace(Convert.ToDecimal(dr["NUMERIC_VALUE"])).ToString());
    //                        }
    //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
    //                        {
    //                            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
    //                            if (Fil.Exists)
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/NoImage.gif");
    //                            }

    //                        }
    //                    }
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
    //                    _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
    //                    _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "producttable_" + soddevenrow);
    //                    _stmpl_container.SetAttribute("TBT_SUBCATNAME", subcatname_l1_l2);
    //                    _stmpl_container.SetAttribute("TBT_CATEGORY_NAME", catheader);
    //                    _stmpl_container.SetAttribute("TBWDataList", lstrows);
    //                    lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
    //                    sHTML = sHTML + _stmpl_container.ToString(); 
    //                    ictrecords++;
    //                    icolstart = 0;
    //                    lstrows = new TBWDataList[icol];

    //                }

    //            }

    //            _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);

    //            if (HttpContext.Current.Request.QueryString["ViewMode"] != null)
    //            {
    //                PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
    //                isreadonly.SetValue(HttpContext.Current.Request.QueryString, false, null);
    //           HttpContext.Current.Request.QueryString.Remove("ViewMode");
    //            }
    //            string productlistURL = HttpContext.Current.Request.QueryString.ToString();
    //            string powersearchURListView = "";
    //            string powersearchURLGridView = "";
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
    //        //if (oCon != null)
    //        //{
    //        //oCon.Close();
    //        //}
    //    }
    //    //}

    //    sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
    //    //objErrorHandler.CreateLog(sHTML); 
    //    return objHelperServices.StripWhitespace(sHTML);
    //}

    public string DynamicPagJson(string url, int ipageno, string eapath, string BCEAPath, string ViewMode, string irecords)
    {
        HelperDB objHelperDB = new HelperDB();
        HelperServices objHelperServices = new HelperServices();
        ProductServices objProductServices = new ProductServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        Security objSecurity = new Security();
        //SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString());
        DataSet dscat = new DataSet();
        ConnectionDB objConnectionDB = new ConnectionDB();
        string _tempfid = "";
        string category_nameh = "";
        string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
        EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        stemplatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
        //  string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
        //if (HttpContext.Current.Session["PS_SEARCH_RESULTS"] == null)
        //{
        //    return "";
        //}
        if (HttpContext.Current.Request["srctext"] != null && HttpContext.Current.Request["srctext"].ToString() == "")
        {
            return "CLEAR";
        }
        else if (HttpContext.Current.Request["srctext"] != null)
        {
            _SearchString = HttpContext.Current.Request["srctext"].ToString();

        }
        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        StringTemplate _stmpl_pages = null;
        string subcatname_l1 = "";
        string subcatname_l2 = "";
        string subcatname_l1_l2 = "";
        string catname = "";
        string catheader = "";
        bool subcatdispflag = false;
        bool catdispflag = false;
        int oe = 0;
        string _ViewType = "";

        string sHTML = "";

        //if (Convert.ToInt32(HttpContext.Current.Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        // {
        try
        {
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

            //if (HttpContext.Current.Request.QueryString["pcr"] != null)
            //    _pcr = HttpContext.Current.Request.QueryString["pcr"];

            bool BindToST = true;

            _ViewType = ViewMode;

            //if (HttpContext.Current.Request.QueryString["ViewMode"] != null)
            //{
            //    _ViewType = HttpContext.Current.Request.QueryString["ViewMode"];
            //    HttpContext.Current.Session["PL_VIEW_MODE"] = _ViewType;
            //}
            //else if (HttpContext.Current.Session["PL_VIEW_MODE"] != null && HttpContext.Current.Session["PL_VIEW_MODE"].ToString() != "")
            //    _ViewType = HttpContext.Current.Session["PL_VIEW_MODE"].ToString();
            //else
            //    _ViewType = "GV";




            //if (_ViewType != "")
            //    HttpContext.Current.Session["PS_VIEW_MODE"] = _ViewType;

            //if (HttpContext.Current.Session["PS_VIEW_MODE"] != null && HttpContext.Current.Session["PS_VIEW_MODE"] != "")
            //    _ViewType = HttpContext.Current.Session["PS_VIEW_MODE"].ToString();

            //if (HttpContext.Current.Request.QueryString["path"] != null)
            //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString()));

            _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();
            string _BCEAPath = string.Empty;
            _BCEAPath = BCEAPath;

            DataSet tmp = (DataSet)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTDataSet);
            if (tmp != null && tmp.Tables.Count >= 1 && tmp.Tables[0].Rows.Count >= 1)
            {

                category_nameh = tmp.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
            }
            DataSet dsfamprod = new DataSet();

            //  dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            //dscat = ps.getwesproducts();
            if (dscat == null || dscat.Tables["FamilyPro"].Rows.Count==0)
            {
                return "";
            }
            //else if (dscat.Tables[2].Rows.Count == 0)
            //{
            //    return "";
            //}

            DataRow drpagect = dscat.Tables[0].Rows[0];
            DataRow drproductsct = dscat.Tables[1].Rows[0];
            if (dsfamprod.Tables != null && dsfamprod.Tables.Count > 0 && HttpContext.Current.Request.QueryString["fid"] != null && HttpContext.Current.Request.QueryString["fid"].ToString() != "" && HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx"))
            {
                iTotalProducts = dsfamprod.Tables[0].Rows.Count;
                if (iRecordsPerPage >= iTotalProducts)
                {
                    iTotalPages = 1;
                }
                else
                {
                    double recordpg = iRecordsPerPage;
                    double totalpg = (iTotalProducts / recordpg);
                    if (totalpg > Convert.ToInt32(totalpg))
                        iTotalPages = Convert.ToInt32(totalpg) + 1;
                    else
                        iTotalPages = Convert.ToInt32(totalpg);
                }
            }
            else
            {
                iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
            }
            HttpContext.Current.Session["iTotalPages"] = iTotalPages;

            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];
            _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);

            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx") && _tempfid.Length > 0)
            {
                lstrecords = new TBWDataList[dsfamprod.Tables[0].Rows.Count + 1];
            }
            else
            {
                lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count + 1];
            }
            int ictrecords = 0;
            int icolstart = 0;
            int icol = 1;
            lstrows = new TBWDataList[icol];

            if (_ViewType == "GV")
            {
                icol = 4;
                lstrows = new TBWDataList[icol];
            }
            else
            {
                icol = 1;
                lstrows = new TBWDataList[icol];
            }
            //if (dscat.Tables[0].Rows.Count < icol)
            //{
            //    icol = dscat.Tables[0].Rows.Count;
            //}
            string soddevenrow = "odd";
            //DataRow[] drprodcoll = null;


            //drprodcoll = dscat.Tables["FamilyPro"].Select("", "SNO");

            string trmpstr = "";
            int ProductTotalcount = 0;
            int pricecode = 0;

            ////if (HttpContext.Current.Session["USER_ID"] != null)
            //    userid = HttpContext.Current.Session["USER_ID"].ToString();

            //if (userid == "")


            string userid = "";
            if (HttpContext.Current.Session["USER_ID"] != null)
            {
                userid = HttpContext.Current.Session["USER_ID"].ToString();
            }
            //if (userid == "")
            //{
            //    userid = objHelperServices.CheckCredential();
            //}

            if (userid == "")
            {
                userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
            }
            DataSet dsBgDisc = new DataSet();
            DataSet dsPriceTableAll = new DataSet();
            string tmpProds = "";

            //string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
            //if (Convert.ToInt32(userid) > 0)
            //{

            //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
            //}
            //else
            //{
            //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
            //}

            pricecode = objHelperDB.GetPriceCode(userid);

            if (dscat.Tables["FamilyPro"].Rows.Count  > 0)
            {

                tmpProds = "";
                if (Convert.ToInt32(userid) > 0)
                {
                    foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                        tmpProds = tmpProds.Replace(",,", ",");
                    }
                    if (tmpProds != "")
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                        dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                    }
                }
                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                        bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);
                foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                {
                    oe++;
                    if ((oe % 2) == 0)
                    {
                        soddevenrow = "even";
                    }
                    else
                    {
                        soddevenrow = "odd";
                    }


                    //if (_ViewType == "GV")
                    //{
                        _stmpl_records = _stg_records.GetInstanceOf("PowerSearch" + "\\" + "productlist_GridView");
                    //}
                    //else
                    //{
                    //    _stmpl_records = _stg_records.GetInstanceOf("PowerSearch" + "\\" + "productlist_" + soddevenrow);
                    //}
                        if (_ViewType == "GV")
                            _stmpl_records.SetAttribute("LISTVIEW_CLASS", "");
                        else
                            _stmpl_records.SetAttribute("LISTVIEW_CLASS", "list-group-item");


                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomEnabled);

                    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                    else
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

                    _stmpl_records.SetAttribute("PRODUCT_ID", drpid["PRODUCT_ID"].ToString());

                    //_stmpl_records.SetAttribute("MIN_ORD_QTY", oProd.GetMinOrdQty(Convert.ToInt32(drpid["PRODUCT_ID"].ToString())).ToString());
                    _stmpl_records.SetAttribute("MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());



                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                    if (_ViewType == "GV")
                    {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                    }
                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);
                    _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", trmpstr.Replace('"', ' '));
                    _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_CODE"].ToString());
                    _stmpl_records.SetAttribute("CDNROOT", System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString());

                    int _pid;
                    if (drpid["PRODUCT_ID"].ToString() != null && drpid["PRODUCT_ID"].ToString() != "")
                    {

                        _pid = System.Convert.ToInt32(drpid["PRODUCT_ID"].ToString());
                    }

                    //_stmpl_records.SetAttribute("TBT_USER_PRICE", GetMyPrice(_pid));
                    //_stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["NUMERIC_VALUE"].ToString());
                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());

                    _stmpl_records.SetAttribute("CATEGORY_ID", HttpUtility.UrlEncode(drpid["CATEGORY_ID"].ToString()));

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));

                    string stk_sta_desc = drpid["STOCK_STATUS_DESC"].ToString().Trim().ToUpper();
                //objErrorHandler.CreateLog(stk_sta_desc + "microsite");
                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
                    }
                    else
                    {
                        if ((stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE") || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA")
                        {
                            _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                            if (_SearchString.ToUpper() != drpid["PRODUCT_CODE"].ToString().ToUpper() &&
                       drpid["PROD_SUBSTITUTE"].ToString().Trim() == "" && stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE")
                            {
                                BindToST = false;
                            }

                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                        }
                    }
                    string PriceTable = "";




                    //if (dsBgDisc != null)
                    //{
                    //    if (dsBgDisc.Tables[0].Rows.Count > 0)
                    //    {
                    //        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                    //        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                    //        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                    //        //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                    //        bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                    //        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                    //        {
                    //            //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                    //        }
                    //    }
                    //}
                    if (Convert.ToInt32(userid) > 0)
                    {

                        PriceTable = objFamilyServices.AssemblePriceTableMS(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
                    }
                    _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);

                    string[] catpath = null;
                string categorypath = string.Empty;
                    //catpath = HttpContext.Current.Request.RawUrl.Split('/');
                    //if (catpath.Length > 1)
                    //{
                    //    categorypath = catpath[1];
                    //}

                    if (drpid["CATEGORY_PATH"].ToString() != null)
                    {

                        catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                        categorypath = (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ");
                    }
                    //if (drpid["CATEGORY_PATH"].ToString() != null)
                    //{

                    //    catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                    //    categorypath = catpath[0] + "////" + catpath[1];
                    //}


                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());

                        //Added by :indu

                        string newurl = string.Empty;
                        newurl = WAG_Root_Path + "////" + categorypath + "////" +
                                          drpid["FAMILY_NAME"] + "////" + drpid["FAMILY_ID"];


                        objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "fl.aspx");


                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);

                        string newurl = string.Empty;


                        newurl = WAG_Root_Path + "////" + categorypath + "////" +
                                           drpid["FAMILY_NAME"] + "////" +
                                         drpid["PRODUCT_ID"] + "=" + drpid["PRODUCT_CODE"] + "////"
                                         + drpid["FAMILY_ID"];


                        objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "mpd.aspx");
                    }

                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_5", drpid["PRODUCT_PRICE"].ToString());
                 //Added By Indu
                    _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"].ToString());
                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());

                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());
                 

                    //
                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"].ToString());

                    if (drpid["PRODUCT_CODE"].ToString().Length <= 8)
                    {
                        _stmpl_records.SetAttribute("TBT_ORG_ORDERNO", true);
                    }
                  

                   // System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                   // if (Fil.Exists)
                   // {
                        if (!drpid["Prod_Thumbnail"].ToString().Contains("_images_200"))
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", objhelper.SetImageFolderPath(drpid["Prod_Thumbnail"].ToString().Replace("\\", "/"), "_th", "_Images_200"));
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", drpid["Prod_Thumbnail"].ToString());
                        }
                   // }
                  //  else
                   //     _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", "/images/noimage.gif");



                    string desc = "";
                    string descattr = "";
                    string prod_desc_alt = "";
                    //if (_ViewType == "LV")
                    //{
                    //    desc = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                    //    prod_desc_alt = drpid["Prod_Description"].ToString();
                    //    if (prod_desc_alt.Length > 0)
                    //        _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                    //    else
                    //        _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"].ToString());
                    //    if (desc.Length > 250 && _ViewType == "LV")
                    //    {
                    //        _stmpl_records.SetAttribute("TBT_MORE_62", true);
                    //        desc = desc.Substring(0, 230).ToString();
                    //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_62", desc);
                    //    }
                    //    else
                    //    {
                    //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_62", desc);
                    //        _stmpl_records.SetAttribute("TBT_MORE_62", false);
                    //    }
                    //    desc = drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                    //    if (desc.Length > 250 && _ViewType == "LV")
                    //    {
                    //        _stmpl_records.SetAttribute("TBT_MORE_4", true);
                    //        desc = desc.Substring(0, 230).ToString();
                    //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_4", desc);
                    //    }
                    //    else
                    //    {
                    //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_4", desc);
                    //        _stmpl_records.SetAttribute("TBT_MORE_4", false);
                    //    }


                    //}
                    //else
                    {

                        descattr = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        if (descattr.Length > 0)
                            descattr = descattr + "<br/>";
                        descattr = descattr + " " + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        descattr = descattr + " " + drpid["Prod_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        prod_desc_alt = drpid["Prod_Description"].ToString();
                        if (prod_desc_alt.Length > 0)
                        {
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                            _stmpl_records.SetAttribute("PROD_DESC_TITLE", prod_desc_alt.Replace('"', ' '));
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"].ToString());
                            _stmpl_records.SetAttribute("PROD_DESC_TITLE", drpid["FAMILY_NAME"].ToString().Replace('"', ' '));
                        }
                        if (descattr.Length > 140 )
                            //&& _ViewType == "GV")
                        {
                            int count = 0;
                            count = descattr.Count(c => char.IsUpper(c));
                            int count1 = 0;
                            count1 = descattr.Count(c => char.IsSymbol(c)) + descattr.Count(c => char.IsNumber(c));
                            if (count >= 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 120).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35 && count1 > 10)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 135).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35 && count1 < 10)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 140).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);

                            }
                        }
                        else
                        {


                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                        }
                    }


                    if (HttpContext.Current.Request.QueryString["byp"] != null && (HttpContext.Current.Request.QueryString["byp"].ToString() == "2" || HttpContext.Current.Request.QueryString["byp"].ToString() == "3"))
                    {
                        _stmpl_records.SetAttribute("BYP", HttpContext.Current.Request.QueryString["byp"].ToString());
                    }
                    //DataRow[] drAry = dscat.Tables[2].Select("FAMILY_ID ='" + drpid["FAMILY_ID"].ToString() + "' AND " + "PRODUCT_ID ='" + drpid["PRODUCT_ID"].ToString() + "'");
                    //if (drAry.Length > 0)
                    //{
                    //    string descattr = "";
                    //    string desc = "";
                    //    foreach (DataRow dr in drAry.CopyToDataTable().Rows)
                    //    {
                    //        if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
                    //        {
                    //            if (dr["ATTRIBUTE_ID"].ToString() == "1")
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
                    //            else
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());

                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
                    //        {
                    //            desc = dr["STRING_VALUE"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                    //            descattr = descattr + " " + desc;
                    //            if (desc.Length > 250 && _ViewType == "LV")
                    //            {
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //                desc = desc.Substring(0, 250).ToString();
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            }
                    //            //else if (desc.Length > 30 && _ViewType == "GV")
                    //            //{
                    //            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //            //    desc = desc.Substring(0, 30).ToString();
                    //            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            //}
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
                    //            }

                    //            //_stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"]);
                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
                    //        {
                    //            if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.FixDecPlace(Convert.ToDecimal(dr["NUMERIC_VALUE"])).ToString());
                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
                    //        {
                    //            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
                    //            if (Fil.Exists)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/NoImage.gif");
                    //            }

                    //        }
                    //    }
                    //    if (descattr.Length > 86 && _ViewType == "GV")
                    //    {
                    //        _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //        descattr = descattr.Substring(0, 86).ToString();
                    //        descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //        _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //    }
                    //    else
                    //        _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //}

                    if (BindToST == true)
                    {
                        lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                    }

                    icolstart++;

                    if (icolstart >= icol || oe == dscat.Tables["FamilyPro"].Rows.Count)
                    {
                        _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("PowerSearch" + "\\" + "producttable_" + soddevenrow);
                        _stmpl_container.SetAttribute("TBT_SUBCATNAME", subcatname_l1_l2);
                        _stmpl_container.SetAttribute("TBT_CATEGORY_NAME", catheader);
                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        sHTML = sHTML + _stmpl_container.ToString();
                        ictrecords++;
                        icolstart = 0;
                        lstrows = new TBWDataList[icol];

                    }

                }

                _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);

                if (HttpContext.Current.Request.QueryString["ViewMode"] != null)
                {
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    isreadonly.SetValue(HttpContext.Current.Request.QueryString, false, null);
                    HttpContext.Current.Request.QueryString.Remove("ViewMode");
                }
                string productlistURL = HttpContext.Current.Request.QueryString.ToString();
                string powersearchURListView = "";
                string powersearchURLGridView = "";
                powersearchURListView = productlistURL + "&ViewMode=LV";
                powersearchURLGridView = productlistURL + "&ViewMode=GV";





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
            //if (oCon != null)
            //{
            //oCon.Close();
            //}
        }
        //}

        sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
        //objErrorHandler.CreateLog(sHTML); 
        return objHelperServices.StripWhitespace(sHTML);
    }

    protected DataSet Get_Value_Breadcrum(int ipageno, string eapath, string irecords)
    {

        string sHTML = "";
        string sBrandAndModelHTML = "";
        string sModelListHTML = "";
        DataSet dscat = new DataSet();
        EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
        try
        {


            string _catCid = "";
            string _parentCatID = "";
            int ictrows = 0;
            string _tsb = "";
            string _tsm = "";
            string _type = "";
            string _value = "";
            string _bname = "";
            string _searchstr = "";
            string url = "";
            string _byp = "2";
            string _bypcat = null;
            string _pid = "";
            string _fid = "";
            string _seeall = "";
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


            //if (_catCid != "")
            //    _parentCatID = GetParentCatID(_catCid);
          
            url = HttpContext.Current.Request.Url.OriginalString.ToLower();

            string EA = string.Empty;
            EA = eapath;


          
            string[] ConsURL = eapath.Replace("AttribSelect=", "").Split(new string[] { "////" }, StringSplitOptions.None);


            _value = ConsURL[ConsURL.Length - 1].Replace('_', ' ').Replace("`/`", "/");
            if (_value.Contains("="))
            {
                string[] Gettype = _value.Split(new string[] { "=" }, StringSplitOptions.None);
                string AttribSelect = string.Empty;
                if (HttpContext.Current.Request.Url.ToString().Contains("bb.aspx"))
                {
                    if (Gettype[0].ToString().ToLower().Trim() != "brand")
                    {
                        if ((Gettype[0].ToString().ToLower().Trim() != "model"))
                        {
                            _type = Gettype[0];
                            _value = Gettype[1].Replace('_', ' ');
                        }

                    }

                    if (Gettype[0].ToString().ToLower().Trim() == "model")
                    {
                        string[] Gettype1 = _value.Split(new string[] { ":" }, StringSplitOptions.None);
                        _tsb = Gettype1[0];
                        _tsm = Gettype1[1];
                        _type = "";
                    }
                }
                else
                {
                    if (Gettype[0].ToString().ToLower().Contains("usersearch") == false)
                    {
                        _type = Gettype[0];
                        _value = Gettype[1].Replace('_', ' ');
                        _searchstr = _value;
                    }
                    else
                    {
                        _searchstr = _value;
                    }
                    if (Gettype[0].ToString().ToLower().Trim() == "brand")
                    {
                        _bname = _value;
                    }



                }

            }
            else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx") == true)
            {
                _searchstr = _value;
            }
            else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx"))
            {
                _type = "Category";
                // _value = _value;
            }









                //if (HttpContext.Current.Session["MainCategory"] != null)
                //{
                //    DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
                //    if (dr.Length > 0)
                //        _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
                //}


            iPageNo = ipageno;

            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true)
            {
                int SubCatCount = 0;
                if (_type == null)
                {
                    if (_tsb != null && _tsb != "" && _tsm != null && _tsm != null)
                    {

                        //string parentCatName = GetCName(ParentCatID);
                        //EasyAsk.GetBrandAndModelProducts(parentCatName, _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                        //if (HttpContext.Current.Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                        //    iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

                        dscat = EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, irecords, (iPageNo - 1).ToString(), "Next", EA);

                    }
                }
                else
                {
                    //if (HttpContext.Current.Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                    //    iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

                    if (_type != "")
                    {

                        dscat = EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, irecords, (iPageNo - 1).ToString(), "Next", EA);

                    }
                    else
                    { //new open

                        dscat = EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, irecords, (iPageNo - 1).ToString(), "Next", EA);

                    }
                }
            }
            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx") == true)
            {
                //if (HttpContext.Current.Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
                //    iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());
              

                dscat = EasyAsk.GetAttributeProducts("ps", _searchstr, _type, _value, _bname, irecords, (iPageNo - 1).ToString(), "Next", EA);
               
            }

            //end
        }

        catch (Exception ex)
        {
            sHTML = ex.Message;
        }




        return dscat;
    }


    //public string DynamicPag_BrandAndModelProductListNew(string url, int ipageno, string eapath, string BCEAPath, string ViewMode, string irecords)
    //{
    //    HelperDB objHelperDB = new HelperDB();
    //    HelperServices objHelperServices = new HelperServices();
    //    ProductServices objProductServices = new ProductServices();
    //    ErrorHandler objErrorHandler = new ErrorHandler();
    //    Security objSecurity = new Security();
    //    //SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString());
    //    DataSet dscat = new DataSet();
    //    ConnectionDB objConnectionDB = new ConnectionDB();
    //    string _tempfid = "";
    //    string category_nameh = "";
    //    string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
    //    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    //    string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    //    stemplatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
       
    //    //if (HttpContext.Current.Session["PS_SEARCH_RESULTS"] == null)
    //    //{
    //    //    return ""; 
    //    //}
    //    if (HttpContext.Current.Request["srctext"] != null && HttpContext.Current.Request["srctext"].ToString() == "")
    //    {
    //        return "CLEAR";
    //    }
    //    else if (HttpContext.Current.Request["srctext"] != null)
    //    {
    //        _SearchString = HttpContext.Current.Request["srctext"].ToString();

    //    }
    //    FamilyServices objFamilyServices = new FamilyServices();
    //    StringTemplateGroup _stg_container = null;
    //    StringTemplateGroup _stg_records = null;
    //    StringTemplate _stmpl_container = null;
    //    StringTemplate _stmpl_records = null;
    //    StringTemplate _stmpl_pages = null;
    //    string subcatname_l1 = "";
    //    string subcatname_l2 = "";
    //    string subcatname_l1_l2 = "";
    //    string catname = "";
    //    string catheader = "";
    //    bool subcatdispflag = false;
    //    bool catdispflag = false;
    //    int oe = 0;
    //    string _ViewType = "";

    //    string sHTML = "";

    //    //if (Convert.ToInt32(HttpContext.Current.Session["PS_SEARCH_RESULTS"].ToString()) > 0)
    //    // {
    //    try
    //    {
    //        dscat = Get_Value_Breadcrum(ipageno, eapath,irecords);

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





    //        _ViewType = ViewMode;
           
    //        _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();
    //        string _BCEAPath = string.Empty;
    //        _BCEAPath = BCEAPath;
    //        DataSet tmp = (DataSet)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTDataSet);
    //        if (tmp != null && tmp.Tables.Count >= 1 && tmp.Tables[0].Rows.Count >= 1)
    //        {

    //            category_nameh = tmp.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
    //        }
    //        DataSet dsfamprod = new DataSet();

    //        string Tosuit_ModelImage = "";
    //        DataSet dsModel = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
    //        if (dsModel != null)
    //        {
    //            DataRow[] dr = dsModel.Tables[0].Select("TOSUITE_MODEL='" + _tsm + "'");
    //            if (dr.Length > 0)
    //                Tosuit_ModelImage = dr.CopyToDataTable().Rows[0]["IMAGE_FILE"].ToString();

    //        }
    //        else
    //            Tosuit_ModelImage = (string)objHelperDB.GetGenericPageDataDB("", _tsb, _tsm, "GET_BYBRAND_MODEL_IMAGE", HelperDB.ReturnType.RTString);


    //        dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
    //        //dscat = ps.getwesproducts();
    //        if (dscat == null)
    //        {
    //            return "";
    //        }
    //        else if (dscat.Tables[2].Rows.Count == 0)
    //        {
    //            return "";
    //        }

    //        DataRow drpagect = dscat.Tables[0].Rows[0];
    //        DataRow drproductsct = dscat.Tables[1].Rows[0];
    //        if (dsfamprod.Tables != null && dsfamprod.Tables.Count > 0 && HttpContext.Current.Request.QueryString["fid"] != null && HttpContext.Current.Request.QueryString["fid"].ToString() != "" && HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx"))
    //        {
    //            iTotalProducts = dsfamprod.Tables[0].Rows.Count;
    //            if (iRecordsPerPage >= iTotalProducts)
    //            {
    //                iTotalPages = 1;
    //            }
    //            else
    //            {
    //                double recordpg = iRecordsPerPage;
    //                double totalpg = (iTotalProducts / recordpg);
    //                if (totalpg > Convert.ToInt32(totalpg))
    //                    iTotalPages = Convert.ToInt32(totalpg) + 1;
    //                else
    //                    iTotalPages = Convert.ToInt32(totalpg);
    //            }
    //        }
    //        else
    //        {
    //            iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);
    //            iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
    //        }
    //        HttpContext.Current.Session["iTotalPages"] = iTotalPages;

    //        TBWDataList[] lstrecords = new TBWDataList[0];
    //        TBWDataList[] lstrows = new TBWDataList[0];
    //        _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);

    //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx") && _tempfid.Length > 0)
    //        {
    //            lstrecords = new TBWDataList[dsfamprod.Tables[0].Rows.Count + 1];
    //        }
    //        else
    //        {
    //            lstrecords = new TBWDataList[dscat.Tables[2].Rows.Count + 1];
    //        }
    //        int ictrecords = 0;
    //        int icolstart = 0;
    //        int icol = 1;
    //        lstrows = new TBWDataList[icol];

    //        if (_ViewType == "GV")
    //        {
    //            icol = 4;
    //            lstrows = new TBWDataList[icol];
    //        }
    //        else
    //        {
    //            icol = 1;
    //            lstrows = new TBWDataList[icol];
    //        }
    //        //if (dscat.Tables[0].Rows.Count < icol)
    //        //{
    //        //    icol = dscat.Tables[0].Rows.Count;
    //        //}
    //        string soddevenrow = "odd";
    //        DataRow[] drprodcoll = null;


    //        drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1", "SNO");


    //        int ProductTotalcount = 0;
    //        string trmpstr = "";
    //        int pricecode = 0;
    //        //string userid = "";
    //        //if (HttpContext.Current.Session["USER_ID"] != null)
    //        //    userid = HttpContext.Current.Session["USER_ID"].ToString();

    //        //if (userid == "")
    //            //userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

    //        string userid = "";
    //        if (HttpContext.Current.Session["USER_ID"] != null)
    //        {
    //            userid = HttpContext.Current.Session["USER_ID"].ToString();
    //        }
    //        //if (userid == "")
    //        //{
    //        //    userid = objHelperServices.CheckCredential();
    //        //}

    //        if (userid == "")
    //        {
    //            userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
    //        }
    //        DataSet dsBgDisc = new DataSet();
    //        DataSet dsPriceTableAll = new DataSet();
    //        string tmpProds = "";

    //        string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
    //        if (Convert.ToInt32(userid) > 0)
    //        {

    //            dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
    //        }
    //        else
    //        {
    //            dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
    //        }

    //        pricecode = objHelperDB.GetPriceCode(userid);

    //        if (drprodcoll.Length > 0)
    //        {

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
    //                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_GridView");
    //                else
    //                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_" + soddevenrow);


    //                _stmpl_records.SetAttribute("TBT_ECOMENABLED", objHelperServices.GetIsEcomEnabled(userid));



    //                _stmpl_records.SetAttribute("PRODUCT_ID", drpid["PRODUCT_ID"].ToString());

    //                //_stmpl_records.SetAttribute("MIN_ORD_QTY", oProd.GetMinOrdQty(Convert.ToInt32(drpid["PRODUCT_ID"].ToString())).ToString());
    //                _stmpl_records.SetAttribute("MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());
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

                    
    //                   int _pid;
    //                   if (drpid["PRODUCT_ID"].ToString() != null && drpid["PRODUCT_ID"].ToString() != "")
    //                   {
    //                       _pid = System.Convert.ToInt32(drpid["PRODUCT_ID"].ToString());
    //                   }
    //                //_stmpl_records.SetAttribute("TBT_USER_PRICE", GetMyPrice(_pid));
    //                _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["NUMERIC_VALUE"].ToString());
    //                _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());

    //                _stmpl_records.SetAttribute("CATEGORY_ID", HttpUtility.UrlEncode(drpid["CATEGORY_ID"].ToString()));

    //                _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

    //                _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));



    //                string PriceTable = "";




    //                if (dsBgDisc != null)
    //                {
    //                    if (dsBgDisc.Tables[0].Rows.Count > 0)
    //                    {
    //                        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
    //                        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
    //                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
    //                        //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
    //                        bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
    //                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
    //                        {
    //                            //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

    //                        }
    //                    }
    //                }
    //                if (Convert.ToInt32(userid) > 0)
    //                {

    //                    PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
    //                }
    //                _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);




    //                if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
    //                {
    //                    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
    //                    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
    //                    _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());

    //                    string newurl = _BCEAPath + "////" +
    //                                         drpid["FAMILY_ID"].ToString() + "=" + _stmpl_records.GetAttribute("FAMILY_NAME");

    //                    objHelperServices.Cons_NewURl(_stmpl_records, newurl, "fl.aspx", true, "");
    //                }
    //                else
    //                {
    //                    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
    //                    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);
    //                }


    //                if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
    //                    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
    //                else
    //                    _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

    //                if (HttpContext.Current.Request.QueryString["byp"] != null && (HttpContext.Current.Request.QueryString["byp"].ToString() == "2" || HttpContext.Current.Request.QueryString["byp"].ToString() == "3"))
    //                {
    //                    _stmpl_records.SetAttribute("BYP", HttpContext.Current.Request.QueryString["byp"].ToString());
    //                }
    //                DataRow[] drAry = dscat.Tables[2].Select("FAMILY_ID ='" + drpid["FAMILY_ID"].ToString() + "' AND " + "PRODUCT_ID ='" + drpid["PRODUCT_ID"].ToString() + "'");
    //                if (drAry.Length > 0)
    //                {
    //                    string desc = "";
    //                    string descattr = "";
    //                    string proddesc = "";
    //                    foreach (DataRow dr in drAry.CopyToDataTable().Rows)
    //                    {
    //                        if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
    //                        {
    //                            if (dr["ATTRIBUTE_ID"].ToString() == "1")
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
    //                            else
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());

    //                        }
    //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
    //                        {
    //                            desc = dr["STRING_VALUE"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
    //                            descattr = descattr + " " + desc;
    //                            if (proddesc.Trim() == "")
    //                            {
    //                                proddesc = descattr;
    //                            }
    //                            if (desc.Length > 250 && _ViewType == "LV")
    //                            {
    //                                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
    //                                desc = desc.Substring(0, 250).ToString();

    //                                if (drpid["PRODUCT_COUNT"].ToString() != "1")
    //                                {
    //                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                                }
    //                                else
    //                                {
    //                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), proddesc);
    //                                }
    //                            }
    //                            //else if (desc.Length > 30 && _ViewType == "GV")
    //                            //{
    //                            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
    //                            //    desc = desc.Substring(0, 30).ToString();
    //                            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                            //}
    //                            else
    //                            {
    //                                if (drpid["PRODUCT_COUNT"].ToString() != "1")
    //                                {
    //                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                                }
    //                                else
    //                                {
    //                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), proddesc);
    //                                }
    //                                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
    //                            }
    //                            //if (desc.Length > 250 && _ViewType == "LV")
    //                            //{
    //                            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
    //                            //    desc = desc.Substring(0, 250).ToString();
    //                            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                            //}
    //                            ////else if (desc.Length > 30 && _ViewType == "GV")
    //                            ////{
    //                            ////    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
    //                            ////    desc = desc.Substring(0, 30).ToString();
    //                            ////    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                            ////}
    //                            //else
    //                            //{
    //                            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
    //                            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
    //                            //}

    //                            //_stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"]);
    //                        }
    //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
    //                        {
    //                            if (dr["NUMERIC_VALUE"].ToString().Length > 0)
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.FixDecPlace(Convert.ToDecimal(dr["NUMERIC_VALUE"])).ToString());
    //                        }
    //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
    //                        {
    //                            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
    //                            if (Fil.Exists)
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/NoImage.gif");
    //                            }

    //                        }
    //                    }
    //                    //if (descattr.Length > 86 && _ViewType == "GV")
    //                    //{
    //                    //    _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
    //                    //    descattr = descattr.Substring(0, 86).ToString();
    //                    //    descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
    //                    //    _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
    //                    //}
    //                    //else
    //                    //    _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);


    //                    //Modified by:indu
    //                    if (drpid["PRODUCT_COUNT"].ToString() != "1")
    //                    {

    //                        if (descattr.Length > 86 && _ViewType == "GV")
    //                        {
    //                            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
    //                            descattr = descattr.Substring(0, 86).ToString();
    //                            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
    //                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
    //                        }
    //                        else
    //                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
    //                    }
    //                      else
    //                    {
    //                        if (proddesc.Trim() != "")
    //                        {
    //                            if (proddesc.Length > 86 && _ViewType == "GV")
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
    //                                proddesc = proddesc.Substring(0, 86).ToString();
    //                                proddesc = proddesc.Substring(0, proddesc.LastIndexOf(" "));
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", proddesc);
    //                            }
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", proddesc);
    //                            }
    //                        }
    //                        else
    //                        {
    //                            if (descattr.Length > 86 && _ViewType == "GV")
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
    //                                descattr = descattr.Substring(0, 86).ToString();
    //                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
    //                            }
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
    //                            }
    //                        }
                        
    //                    }
    //                    //end
    //                }
    //                //ADDED BY :INDU
    //                if (_stmpl_records.GetAttribute("TBT_SUB_FAMILY").ToString() == "False")
    //                {
    //                    string newurl = string.Empty;

    //                    newurl = _BCEAPath + "////" + drpid["FAMILY_ID"].ToString() + "=" + _stmpl_records.GetAttribute("FAMILY_NAME") + "////" +
    //                       drpid["PRODUCT_ID"].ToString() + "=" + drpid["PRODUCT_CODE"].ToString();

    //                    objHelperServices.Cons_NewURl(_stmpl_records, newurl, "pd.aspx", true, "");
    //                }
    //                //END

    //                lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());

    //                icolstart++;

    //                if (icolstart >= icol || oe == drprodcoll.Length)
    //                {
    //                    _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
    //                    _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "producttable_" + soddevenrow);
    //                    _stmpl_container.SetAttribute("TBT_SUBCATNAME", subcatname_l1_l2);
    //                    _stmpl_container.SetAttribute("TBT_CATEGORY_NAME", catheader);
    //                    _stmpl_container.SetAttribute("TBWDataList", lstrows);
    //                    lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
    //                    sHTML = sHTML + _stmpl_container.ToString();
    //                    ictrecords++;
    //                    icolstart = 0;
    //                    lstrows = new TBWDataList[icol];

    //                }

    //            }

    //            _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);

    //            if (HttpContext.Current.Request.QueryString["ViewMode"] != null)
    //            {
    //                PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
    //                isreadonly.SetValue(HttpContext.Current.Request.QueryString, false, null);
    //                HttpContext.Current.Request.QueryString.Remove("ViewMode");
    //            }
    //            string productlistURL = HttpContext.Current.Request.QueryString.ToString();
    //            string powersearchURListView = "";
    //            string powersearchURLGridView = "";
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
        

    //    sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
    //    string strtop = "<tr><td height=\"126\" align=\"center\" ><TABLE border=\"0\" cellSpacing=\"0\" cellPadding=\"0\"   width=\"760px\"><tr><td>";
    //    string strbottom = "</td></tr></TABLE></td></tr>";
    //    sHTML = strtop + sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>") + strbottom;
    //    return objHelperServices.StripWhitespace(sHTML);

    //}

    public string DynamicPag_BrandAndModelProductListNewJson(string url, int ipageno, string eapath, string BCEAPath, string ViewMode, string irecords)
    {
        HelperDB objHelperDB = new HelperDB();
        HelperServices objHelperServices = new HelperServices();
        ProductServices objProductServices = new ProductServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        Security objSecurity = new Security();
        //SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString());
        DataSet dscat = new DataSet();
        ConnectionDB objConnectionDB = new ConnectionDB();
        string _tempfid = "";
        string category_nameh = "";
        string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
        EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        stemplatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());

        //if (HttpContext.Current.Session["PS_SEARCH_RESULTS"] == null)
        //{
        //    return ""; 
        //}
        if (HttpContext.Current.Request["srctext"] != null && HttpContext.Current.Request["srctext"].ToString() == "")
        {
            return "CLEAR";
        }
        else if (HttpContext.Current.Request["srctext"] != null)
        {
            _SearchString = HttpContext.Current.Request["srctext"].ToString();

        }
        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        StringTemplate _stmpl_pages = null;
        string subcatname_l1 = "";
        string subcatname_l2 = "";
        string subcatname_l1_l2 = "";
        string catname = "";
        string catheader = "";
        bool subcatdispflag = false;
        bool catdispflag = false;
        int oe = 0;
        string _ViewType = "";

        string sHTML = "";

        //if (Convert.ToInt32(HttpContext.Current.Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        // {
        try
        {
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

            //if (HttpContext.Current.Request.QueryString["pcr"] != null)
            //    _pcr = HttpContext.Current.Request.QueryString["pcr"];





            _ViewType = ViewMode;

            _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();
            string _BCEAPath = string.Empty;
            _BCEAPath = BCEAPath;
            DataSet tmp = (DataSet)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTDataSet);
            if (tmp != null && tmp.Tables.Count >= 1 && tmp.Tables[0].Rows.Count >= 1)
            {

                category_nameh = tmp.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
            }
            DataSet dsfamprod = new DataSet();

            string Tosuit_ModelImage = "";
            DataSet dsModel = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
            if (dsModel != null)
            {
                DataRow[] dr = dsModel.Tables[0].Select("TOSUITE_MODEL='" + _tsm + "'");
                if (dr.Length > 0)
                    Tosuit_ModelImage = dr.CopyToDataTable().Rows[0]["IMAGE_FILE"].ToString();

            }
            else
                Tosuit_ModelImage = (string)objHelperDB.GetGenericPageDataDB("", _tsb, _tsm, "GET_BYBRAND_MODEL_IMAGE", HelperDB.ReturnType.RTString);


            dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            //dscat = ps.getwesproducts();
            if (dscat == null || dscat.Tables["FamilyPro"].Rows.Count==0)
            {
                return "";
            }
            

            DataRow drpagect = dscat.Tables[0].Rows[0];
            DataRow drproductsct = dscat.Tables[1].Rows[0];
            if (dsfamprod.Tables != null && dsfamprod.Tables.Count > 0 && HttpContext.Current.Request.QueryString["fid"] != null && HttpContext.Current.Request.QueryString["fid"].ToString() != "" && HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx"))
            {
                iTotalProducts = dsfamprod.Tables[0].Rows.Count;
                if (iRecordsPerPage >= iTotalProducts)
                {
                    iTotalPages = 1;
                }
                else
                {
                    double recordpg = iRecordsPerPage;
                    double totalpg = (iTotalProducts / recordpg);
                    if (totalpg > Convert.ToInt32(totalpg))
                        iTotalPages = Convert.ToInt32(totalpg) + 1;
                    else
                        iTotalPages = Convert.ToInt32(totalpg);
                }
            }
            else
            {
                iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
            }
            HttpContext.Current.Session["iTotalPages"] = iTotalPages;

            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];
            _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);

            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx") && _tempfid.Length > 0)
            {
                lstrecords = new TBWDataList[dsfamprod.Tables[0].Rows.Count + 1];
            }
            else
            {
                lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count + 1];
            }
            int ictrecords = 0;
            int icolstart = 0;
            int icol = 1;
            lstrows = new TBWDataList[icol];

            if (_ViewType == "GV")
            {
                icol = 4;
                lstrows = new TBWDataList[icol];
            }
            else
            {
                icol = 1;
                lstrows = new TBWDataList[icol];
            }
            //if (dscat.Tables[0].Rows.Count < icol)
            //{
            //    icol = dscat.Tables[0].Rows.Count;
            //}
            string soddevenrow = "odd";
           //DataRow[] drprodcoll = null;


            //drprodcoll = dscat.Tables["FamilyPro"].Select("", "SNO");


            int ProductTotalcount = 0;
            string trmpstr = "";
            int pricecode = 0;
            //string userid = "";
            //if (HttpContext.Current.Session["USER_ID"] != null)
            //    userid = HttpContext.Current.Session["USER_ID"].ToString();

            //if (userid == "")
            //userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

            string userid = "";
            if (HttpContext.Current.Session["USER_ID"] != null)
            {
                userid = HttpContext.Current.Session["USER_ID"].ToString();
            }
            //if (userid == "")
            //{
            //    userid = objHelperServices.CheckCredential();
            //}

            if (userid == "")
            {
                userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
            }
            DataSet dsBgDisc = new DataSet();
            DataSet dsPriceTableAll = new DataSet();
            string tmpProds = "";

            //string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
            //if (Convert.ToInt32(userid) > 0)
            //{

            //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
            //}
            //else
            //{
            //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
            //}

            pricecode = objHelperDB.GetPriceCode(userid);

            if (dscat.Tables["FamilyPro"].Rows.Count > 0)
            {

                tmpProds = "";
                if (Convert.ToInt32(userid) > 0)
                {
                    foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                        tmpProds = tmpProds.Replace(",,", ",");
                    }
                    if (tmpProds != "")
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                        dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                    }
                }
                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);
                foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                {
                    oe++;
                    if ((oe % 2) == 0)
                    {
                        soddevenrow = "even";
                    }
                    else
                    {
                        soddevenrow = "odd";
                    }


                    if (_ViewType == "GV")
                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_GridView");
                    else
                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_" + soddevenrow);


                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomEnabled);



                    _stmpl_records.SetAttribute("PRODUCT_ID", drpid["PRODUCT_ID"].ToString());

                    //_stmpl_records.SetAttribute("MIN_ORD_QTY", oProd.GetMinOrdQty(Convert.ToInt32(drpid["PRODUCT_ID"].ToString())).ToString());
                    _stmpl_records.SetAttribute("MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());
                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                    if (_ViewType == "GV")
                    {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                    }

                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);
                    _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", trmpstr.Replace('"', ' '));

                    _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());


                    int _pid;
                    if (drpid["PRODUCT_ID"].ToString() != null && drpid["PRODUCT_ID"].ToString() != "")
                    {
                        _pid = System.Convert.ToInt32(drpid["PRODUCT_ID"].ToString());
                    }
                    //_stmpl_records.SetAttribute("TBT_USER_PRICE", GetMyPrice(_pid));
                   // _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["NUMERIC_VALUE"].ToString());
                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());

                    _stmpl_records.SetAttribute("CATEGORY_ID", HttpUtility.UrlEncode(drpid["CATEGORY_ID"].ToString()));

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));



                    string PriceTable = "";




                    //if (dsBgDisc != null)
                    //{
                    //    if (dsBgDisc.Tables[0].Rows.Count > 0)
                    //    {
                    //        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                    //        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                    //        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                    //        //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                    //        bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                    //        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                    //        {
                    //            //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                    //        }
                    //    }
                    //}
                    if (Convert.ToInt32(userid) > 0)
                    {

                        PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
                    }
                    _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);


                    string[] catpath = null;
                    string categorypath = string.Empty;
                    if (drpid["CATEGORY_PATH"].ToString() != null)
                    {

                        catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                        categorypath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >= 2 ? catpath[1] : " ");
                    }

                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());

                        string newurl = string.Empty;
                        newurl = WAG_Root_Path + "////" + categorypath + "////" +
                                          drpid["FAMILY_NAME"] + "////" + drpid["FAMILY_ID"];


                        objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "fl.aspx");

                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);


                        string newurl = string.Empty;


                        newurl = WAG_Root_Path + "////" + categorypath + "////" +
                                           drpid["FAMILY_NAME"] + "////" +
                                         drpid["PRODUCT_ID"] + "=" + drpid["PRODUCT_CODE"] + "////"
                                         + drpid["FAMILY_ID"];


                        objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "mpd.aspx");
                    }


                    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                    else
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

                    if (HttpContext.Current.Request.QueryString["byp"] != null && (HttpContext.Current.Request.QueryString["byp"].ToString() == "2" || HttpContext.Current.Request.QueryString["byp"].ToString() == "3"))
                    {
                        _stmpl_records.SetAttribute("BYP", HttpContext.Current.Request.QueryString["byp"].ToString());
                    }

                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"].ToString());

                    if (drpid["PRODUCT_CODE"].ToString().Length <= 8)
                    {
                        _stmpl_records.SetAttribute("TBT_ORG_ORDERNO", true);
                    }

                    //modified by smith
                    if (drpid["Prod_Thumbnail"].ToString() != null && drpid["Prod_Thumbnail"].ToString() != String.Empty && drpid["Prod_Thumbnail"].ToString() != "")
                    {

                         if (!drpid["Prod_Thumbnail"].ToString().Contains("_images_200"))
                         {
                             _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", objhelper.SetImageFolderPath(drpid["Prod_Thumbnail"].ToString().Replace("\\", "/"), "_th", "_Images_200"));
                         }
                         else
                         {
                             _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", drpid["Prod_Thumbnail"].ToString().Replace("\\", "/"));
                         }
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", "images/noimage.gif");
                    }


                    //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                    //if (Fil.Exists)
                    //{
                    //    //Modified by:Indu
                    //    if (!drpid["Prod_Thumbnail"].ToString().Contains("_images_200"))
                    //    {
                    //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", objhelper.SetImageFolderPath(drpid["Prod_Thumbnail"].ToString().Replace("\\", "/"), "_th", "_Images_200"));
                    //    }
                    //    else
                    //    {
                    //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", drpid["Prod_Thumbnail"].ToString().Replace("\\", "/"));
                    //    }
                           
                    //}
                    //else
                    //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_452", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()+"images/noimage.gif");


                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_5", drpid["PRODUCT_PRICE"].ToString());


                    _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"].ToString());
                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());


                    string desc = "";
                    string descattr = "";

                    if (_ViewType == "LV")
                    {
                        if (drpid["PRODUCT_COUNT"].ToString() != "1")
                        {
                            desc = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                            desc = desc + " " + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        }
                        else
                            desc = drpid["Prod_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        if (desc.Length > 250)
                        {
                            _stmpl_records.SetAttribute("TBT_MORE_62", true);
                            desc = desc.Substring(0, 230).ToString();
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_62", desc);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_62", desc);
                            _stmpl_records.SetAttribute("TBT_MORE_62", false);
                        }



                    }
                    else
                    {
                        if (drpid["PRODUCT_COUNT"].ToString() != "1")
                        {
                            descattr = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                            if (descattr.Length > 0)
                                descattr = descattr + "<br/>";
                            descattr = descattr + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        }
                        else
                            descattr = drpid["Prod_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        if (descattr.Length > 140)
                        {
                            int count = 0;
                            count = descattr.Count(c => char.IsUpper(c));
                            if (count >= 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 120).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 140).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                        }
                        else
                        {


                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                        }
                    }




                    //DataRow[] drAry = dscat.Tables[2].Select("FAMILY_ID ='" + drpid["FAMILY_ID"].ToString() + "' AND " + "PRODUCT_ID ='" + drpid["PRODUCT_ID"].ToString() + "'");
                    //if (drAry.Length > 0)
                    //{
                    //    string desc = "";
                    //    string descattr = "";
                    //    string proddesc = "";
                    //    foreach (DataRow dr in drAry.CopyToDataTable().Rows)
                    //    {
                    //        if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
                    //        {
                    //            if (dr["ATTRIBUTE_ID"].ToString() == "1")
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
                    //            else
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());

                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
                    //        {
                    //            desc = dr["STRING_VALUE"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                    //            descattr = descattr + " " + desc;
                    //            if (proddesc.Trim() == "")
                    //            {
                    //                proddesc = descattr;
                    //            }
                    //            if (desc.Length > 250 && _ViewType == "LV")
                    //            {
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //                desc = desc.Substring(0, 250).ToString();

                    //                if (drpid["PRODUCT_COUNT"].ToString() != "1")
                    //                {
                    //                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //                }
                    //                else
                    //                {
                    //                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), proddesc);
                    //                }
                    //            }
                    //            //else if (desc.Length > 30 && _ViewType == "GV")
                    //            //{
                    //            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //            //    desc = desc.Substring(0, 30).ToString();
                    //            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            //}
                    //            else
                    //            {
                    //                if (drpid["PRODUCT_COUNT"].ToString() != "1")
                    //                {
                    //                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //                }
                    //                else
                    //                {
                    //                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), proddesc);
                    //                }
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
                    //            }
                    //            //if (desc.Length > 250 && _ViewType == "LV")
                    //            //{
                    //            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //            //    desc = desc.Substring(0, 250).ToString();
                    //            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            //}
                    //            ////else if (desc.Length > 30 && _ViewType == "GV")
                    //            ////{
                    //            ////    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //            ////    desc = desc.Substring(0, 30).ToString();
                    //            ////    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            ////}
                    //            //else
                    //            //{
                    //            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
                    //            //}

                    //            //_stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"]);
                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
                    //        {
                    //            if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.FixDecPlace(Convert.ToDecimal(dr["NUMERIC_VALUE"])).ToString());
                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
                    //        {
                    //            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
                    //            if (Fil.Exists)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/NoImage.gif");
                    //            }

                    //        }
                    //    }
                    //    //if (descattr.Length > 86 && _ViewType == "GV")
                    //    //{
                    //    //    _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //    //    descattr = descattr.Substring(0, 86).ToString();
                    //    //    descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //    //    _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //    //}
                    //    //else
                    //    //    _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);


                    //    //Modified by:indu
                    //    if (drpid["PRODUCT_COUNT"].ToString() != "1")
                    //    {

                    //        if (descattr.Length > 86 && _ViewType == "GV")
                    //        {
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //            descattr = descattr.Substring(0, 86).ToString();
                    //            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //        }
                    //        else
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //    }
                    //    else
                    //    {
                    //        if (proddesc.Trim() != "")
                    //        {
                    //            if (proddesc.Length > 86 && _ViewType == "GV")
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //                proddesc = proddesc.Substring(0, 86).ToString();
                    //                proddesc = proddesc.Substring(0, proddesc.LastIndexOf(" "));
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC", proddesc);
                    //            }
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC", proddesc);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (descattr.Length > 86 && _ViewType == "GV")
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //                descattr = descattr.Substring(0, 86).ToString();
                    //                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //            }
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //            }
                    //        }

                    //    }
                    //    //end
                    //}
                    ////ADDED BY :INDU
                    //if (_stmpl_records.GetAttribute("TBT_SUB_FAMILY").ToString() == "False")
                    //{
                    //    string newurl = string.Empty;

                    //    newurl = _BCEAPath + "////" + drpid["FAMILY_ID"].ToString() + "=" + _stmpl_records.GetAttribute("FAMILY_NAME") + "////" +
                    //       drpid["PRODUCT_ID"].ToString() + "=" + drpid["PRODUCT_CODE"].ToString();

                    //    objHelperServices.Cons_NewURl(_stmpl_records, newurl, "pd.aspx", true, "");
                    //}
                    ////END

                    lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());

                    icolstart++;

                    if (icolstart >= icol || oe == dscat.Tables["FamilyPro"].Rows.Count)
                    {
                        _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "producttable_" + soddevenrow);
                        _stmpl_container.SetAttribute("TBT_SUBCATNAME", subcatname_l1_l2);
                        _stmpl_container.SetAttribute("TBT_CATEGORY_NAME", catheader);
                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        sHTML = sHTML + _stmpl_container.ToString();
                        ictrecords++;
                        icolstart = 0;
                        lstrows = new TBWDataList[icol];

                    }

                }

                _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);

                if (HttpContext.Current.Request.QueryString["ViewMode"] != null)
                {
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    isreadonly.SetValue(HttpContext.Current.Request.QueryString, false, null);
                    HttpContext.Current.Request.QueryString.Remove("ViewMode");
                }
                string productlistURL = HttpContext.Current.Request.QueryString.ToString();
                string powersearchURListView = "";
                string powersearchURLGridView = "";
                powersearchURListView = productlistURL + "&ViewMode=LV";
                powersearchURLGridView = productlistURL + "&ViewMode=GV";







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


        sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
        string strtop = "<tr><td height=\"126\" align=\"center\" ><TABLE border=\"0\" cellSpacing=\"0\" cellPadding=\"0\"   width=\"775px\"><tr><td>";
        string strbottom = "</td></tr></TABLE></td></tr>";
        sHTML = strtop + sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>") + strbottom;
        return objHelperServices.StripWhitespace(sHTML);

    }
}
