using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
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
using System.Reflection;
using System.Linq;
using System;
using System.Web;

namespace WES.UC
{
    public partial class bs : System.Web.UI.UserControl
    {

        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        CategoryServices objCategoryServices = new CategoryServices();
        UserServices objUserServices = new UserServices();
        Security objSecurity = new Security();
        HelperDB objHelperDB = new HelperDB();
        //SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString());
        DataSet dscat = new DataSet();
        //ProductFamily oPF = new ProductFamily();
        ProductServices objProductServices = new ProductServices();
        ConnectionDB objConnectionDB = new ConnectionDB();
        int iCatalogId;
        int iInventoryLevelCheck;
        int iRecordsPerPage = 24;
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
        string _pcr = "";
        string _EAPath = "";
        string _ParentCatID = "";
        public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

        string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
        string _AttType = "";
        string _AttValue = "";
        int _resultpage = 12;
        int _PageNo = 1;
        string ParentCatID = "";
        string _Brand = "";
        public string WAG_Root_Path = System.Configuration.ConfigurationManager.AppSettings["EA_ROOT_CATEGORY_PATH"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                string hfclickedattr = Request.Form["hfclickedattr"];

                if (hfclickedattr != null)
                {

                    string[] url = Request.RawUrl.ToString().Split(new string[] { "mpl.aspx?" }, StringSplitOptions.None);
                    Session["hfclickedattr_mpl"] = hfclickedattr.Replace("doublequot", @""""); 
                    // objErrorHandler.CreateLog(hfclickedattr);  
                    Response.Redirect(url[0].ToLower().Replace("undefined", ""));
                }
                //if (Request.RawUrl.ToString().ToLower().Contains("undefined"))
                //{
                //    Response.Redirect(Request.RawUrl.ToString().ToLower().Replace("undefined", ""));
                //}

                if (Request.QueryString["type"] != null)
                    _AttType = Request.QueryString["Type"];
                if (Request.QueryString["value"] != null)
                    _AttValue = Request.QueryString["Value"];
                if (Request.QueryString["bname"] != null)
                    _Brand = Request.QueryString["bname"];
                if (Request.QueryString["pgno"] != null)
                    _PageNo = Convert.ToInt32(Request.QueryString["pgno"]);

                if (Request.QueryString["searchstr"] != null)
                    _searchstr = Request.QueryString["searchstr"];

                if (Session["RECORDS_PER_PAGE"] != null)
                    _resultpage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE"]);

                if (Request.QueryString["pcr"] != null)
                {
                    ParentCatID = Request.QueryString["pcr"];
                }

                if (!IsPostBack)
                {
                    HiddenField1.Value = "0";
                    HiddenField2.Value = "0";
                    hforgurl.Value = HttpContext.Current.Request.Url.PathAndQuery.ToString();
                    hfcheckload.Value = "0";
                    HFcnt.Value = "1";
                    hfnewurl.Value = Request.RawUrl.ToString();
                    //.Replace("pl/","pl.aspx?");

                    hfback.Value = "";
                    hfbackdata.Value = "";
                }

            }
            catch (Exception ex)
            {
                //objErrorHandler.ErrorMsg = ex;
                //objErrorHandler.CreateLog();

            }


            try
            {




                stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
                try
                {
                    //if (Request.Url.OriginalString.ToString().ToUpper().Contains("MPL.ASPX"))
                    //    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\product_list\\";
                    //if (Request.Url.OriginalString.ToString().ToUpper().Contains("MBB.ASPX"))
                    //    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";
                    //if (Request.Url.OriginalString.ToString().ToUpper().Contains("MBYPRODUCT.ASPX"))
                    //    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\byproduct\\";
                }
                catch (Exception ec)
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
                // GetStoreConfig();
                //GetPageConfig();

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }

        //private void GetStoreConfig()
        //{
        //    if (Session["CATALOG_ID"] != null && Session["INVENTORY_LEVEL_CHECK"] != null)
        //    {
        //        iCatalogId = Convert.ToInt32(Session["CATALOG_ID"].ToString());
        //        iInventoryLevelCheck = Convert.ToInt32(Session["INVENTORY_LEVEL_CHECK"].ToString());
        //    }
        //}

        //private void GetPageConfig()
        //{
        //    try
        //    {
        //        //if (Session["PS_IS_START_OVER"].ToString() == "YES")
        //        //{
        //        //    bIsStartOver = true;
        //        //}
        //        //else
        //        //{
        //        //    bIsStartOver = false;
        //        //}
        //        bDoPaging = Convert.ToBoolean(Session["DO_PAGING"].ToString());
        //        if (HidItemPage.Value.ToString() == string.Empty || HidItemPage.Value.ToString() == null)
        //        {
        //            if (Session["RECORDS_PER_PAGE_PRODUCT_LIST"] != null)
        //                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_PRODUCT_LIST"].ToString());
        //        }
        //        else
        //        {
        //            iRecordsPerPage = Convert.ToInt32(HidItemPage.Value.ToString());
        //            Session["RECORDS_PER_PAGE_PRODUCT_LIST"] = HidItemPage.Value.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //}
        //private void SetQueryString(StringTemplate _stmpl_pages)
        //{
        //    _stmpl_pages.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_pcr));
        //    _stmpl_pages.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_cid));
        //    _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(_searchstr));
        //    _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_TYPE", _type);
        //    _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(_value));
        //    _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(_bname));
        //    _stmpl_pages.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
        //    _stmpl_pages.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

        //}

        //public string ST_ProductList()
        //{
        //    //if (Session["PS_SEARCH_RESULTS"] == null)
        //    //{
        //    //    return "";
        //    //}
        //    FamilyServices objFamilyServices = new FamilyServices();
        //    StringTemplateGroup _stg_container = null;
        //    StringTemplateGroup _stg_records = null;
        //    StringTemplate _stmpl_container = null;
        //    StringTemplate _stmpl_records = null;
        //    StringTemplate _stmpl_pages = null;
        //    DataSet dsprod = new DataSet();
        //    DataSet dsprodspecs = new DataSet();
        //    DataSet subcatds = new DataSet();
        //    string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();

        //    int oe = 0;
        //    string category_nameh = "";
        //    string sHTML = "";
        //    string _pcr = "";
        //    string _ViewType = "";
        //    string _BCEAPath="";

        //    string lstcarecord = "";
        //    string lstprerecord = "";
        //    string lstvperrecord = "";
        //    DataSet breadcrumb = new DataSet();
        //    HelperServices objHelperService = new HelperServices();
        //    string CID_new = "";
        //    string PCID_new = "";
        //    //if (Convert.ToInt32(Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        //    //{
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
        //           // Session["PL_VIEW_MODE"] = _ViewType;
        //        }
        //        //else if (Session["PL_VIEW_MODE"] != null && Session["PL_VIEW_MODE"].ToString() != "")
        //        //    _ViewType = Session["PL_VIEW_MODE"].ToString();
        //        else
        //            _ViewType = "GV";




        //        //if (Request.QueryString["path"] != null)
        //        //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(Request.QueryString["path"].ToString()));

        //        if (HttpContext.Current.Session["EA"] != null)
        //        {
        //            _EAPath = HttpContext.Current.Session["EA"].ToString();
        //        }
        //          if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
        //        {
        //            _BCEAPath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();   
        //        }



        //        if (Request.Url.ToString().ToLower().Contains("pl.aspx"))
        //        {
        //            //ps.ClearFilterproduct("DELETE TBWC_SEARCH_PROD_LIST WHERE PRODUCT_ID IN(SELECT DISTINCT F.FAMILY_ID FROM TB_FAMILY F,TB_FAMILY_SPECS FS WHERE F.FAMILY_ID=FS.FAMILY_ID AND ISNULL(FS.STRING_VALUE,'')='NOT FOR WEB' AND F.CATEGORY_ID='" + ps.CATEGORY_ID + "') AND USER_SESSION_ID='" + Session.SessionID + "'");
        //            // dscat = ps.GetFamilies();
        //            dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
        //        }
        //        else
        //        {
        //            // dscat = ps.GetProducts();
        //        }



        //        if (dscat != null)
        //        {

        //            if (Request.QueryString["pcr"] != null)
        //                _pcr = Request.QueryString["pcr"];
        //            if (Request.QueryString["type"] == null)
        //            {
        //                //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
        //                string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
        //                if (tempstr != null && tempstr != "")
        //                    category_nameh = tempstr;
        //            }
        //            else
        //                category_nameh = Request.QueryString["value"].ToString();

        //            DataRow drpagect = dscat.Tables[0].Rows[0];
        //            iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);

        //            if (iPageNo > iTotalPages)
        //            {
        //                iPageNo = iTotalPages;
        //                //ps.PAGE_NO = iPageNo;
        //            }
        //            Session["iTotalPages"] = iTotalPages;
        //            DataRow drproductsct = dscat.Tables[1].Rows[0];
        //            iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
        //            if (_cid != "")
        //                _ParentCatID = GetParentCatID(_cid);

        //            if ((iTotalPages <= 0 || iTotalProducts <= 0) && WesNewsCategoryId == _ParentCatID)
        //            {


        //                _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);
        //                _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "advmain");

        //                SetEbookAndPDFLink(_stmpl_container);
        //                DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
        //                if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
        //                    _stmpl_container.SetAttribute("TBT_ADV_URL_LINK", tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString());

        //                _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh.ToUpper());
        //                return _stmpl_container.ToString();
        //            }



        //            TBWDataList[] lstrecords = new TBWDataList[0];
        //            TBWDataList[] lstrows = new TBWDataList[0];
        //            _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);


        //            int ictrecords = 0;
        //            int icolstart = 0;
        //            string trmpstr = "";
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

        //            DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1", "SNO");


        //            string userid = "";
        //            string PriceTable = "";
        //            int pricecode = 0;
        //            string tmpProds = "";
        //            DataSet dsBgDisc = new DataSet();
        //            DataSet dsPriceTableAll = new DataSet();
        //            if (Session["USER_ID"] != null)
        //                userid = Session["USER_ID"].ToString();
        //            if (userid == "")
        //               // userid = "0";
        //                userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

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


        //            lstrecords = new TBWDataList[drprodcoll.Length + 1];
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
        //                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_WES" + soddevenrow);

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

        //                string[] catpath = null;
        //                string categorypath = string.Empty;
        //                if (drpid["CATEGORY_PATH"].ToString() != null)
        //                {

        //                    catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
        //                    categorypath = catpath[0] + "////" + catpath[1];
        //                }
        //                if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
        //                {
        //                    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
        //                    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
        //                    _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());

        //                    //modified by :indu
        //                    string newurl = string.Empty;
        //                    newurl = WAG_Root_Path + "////" + categorypath + "////" +
        //                                      drpid["FAMILY_NAME"]+"////" + drpid["FAMILY_ID"] ;


        //                    objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "fl.aspx");

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

        //                        PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
        //                    }
        //                    _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);


        //                    // _stmpl_records.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));

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
        //                    foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
        //                    {
        //                        if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
        //                        {
        //                            if (dr["ATTRIBUTE_ID"].ToString() == "1")
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
        //                            else
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
        //                        }
        //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
        //                        {
        //                            if (dr["NUMERIC_VALUE"].ToString().Length > 0)
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString());
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
        //                        else
        //                        {
        //                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
        //                        }

        //                        if (_stmpl_records.GetAttribute("TBT_SUB_FAMILY").ToString() == "False")
        //                        {
        //                            string newurl = string.Empty;


        //                            newurl = WAG_Root_Path + "////" + categorypath + "////" +
        //                                               drpid["FAMILY_NAME"] + "////" +
        //                                             drpid["PRODUCT_ID"] + "=" + drpid["PRODUCT_CODE"] + "////"
        //                                             + drpid["FAMILY_ID"];
        //                                               ;

        //                            objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "mpd.aspx");

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
        //                    string descattr = "";
        //                    string desc = "";
        //                    foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
        //                    {

        //                        if (dr["ATTRIBUTE_TYPE"].ToString() == "9")
        //                        {
        //                            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString());
        //                            if (Fil.Exists)
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
        //                            else
        //                            {
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
        //                            }

        //                        }
        //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
        //                        {
        //                            //string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
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
        //                        }
        //                        else
        //                        {

        //                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>"));
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

        //                //lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
        //                icolstart++;
        //                if (icolstart >= icol || oe == drprodcoll.Length)
        //                {
        //                   // if (icolstart == icol || iTotalProducts < 24)
        //                   // {
        //                        _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
        //                        _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "producttable_" + soddevenrow);
        //                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
        //                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
        //                  //  }
        //                    //if (icolstart == 3 || icolstart == 2 || icolstart == 1)
        //                    //{
        //                    //  balrecords_cnt.Value = icolstart.ToString();
        //                    //}

        //                    ictrecords++;
        //                    icolstart = 0;
        //                    lstrows = new TBWDataList[icol];

        //                }
        //            }

        //            //   }

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
        //            powersearchURListView = productlistURL + "&ViewMode=LV";
        //            powersearchURLGridView = productlistURL + "&ViewMode=GV";


        //            _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "main");
        //            _stmpl_container.SetAttribute("TBW_CATEGORY_ID", _cid);
        //            _stmpl_container.SetAttribute("TBW_PRODUCT_COUNT", iTotalProducts);
        //           // _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);



        //            breadcrumb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
        //            DataRow[] _DCRow = null;
        //            _DCRow = breadcrumb.Tables[0].Select("ItemType='Category'");
        //            if (_DCRow != null && _DCRow.Length > 0)
        //            {
        //                foreach (DataRow _drow in _DCRow)
        //                {
        //                    if (_DCRow.Length == 3)
        //                    {
        //                        if (_drow == _DCRow[_DCRow.Length - 1])
        //                        {
        //                            lstcarecord = _drow["ItemValue"].ToString();
        //                        }
        //                        if (_drow == _DCRow[_DCRow.Length - 2])
        //                        {
        //                            lstprerecord = _drow["ItemValue"].ToString();
        //                        }

        //                        if (_drow == _DCRow[_DCRow.Length - 3])
        //                        {
        //                            lstvperrecord = _drow["ItemValue"].ToString();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (_drow == _DCRow[_DCRow.Length - 1])
        //                        {
        //                            lstprerecord = _drow["ItemValue"].ToString();
        //                        }
        //                        if (_drow == _DCRow[_DCRow.Length - 2])
        //                        {
        //                            lstvperrecord = _drow["ItemValue"].ToString();
        //                        }

        //                    }
        //                }
        //            }

        //            if (lstcarecord != null && lstcarecord != "" && lstprerecord != null && lstprerecord != "" && lstvperrecord != null && lstvperrecord != "")

        //                subcatds = (DataSet)objHelperDB.GetGenericDataDB("", lstvperrecord, lstprerecord, lstcarecord, "GET_CATEGORY_DETAILS", HelperDB.ReturnType.RTDataSet);
        //            else
        //                subcatds = (DataSet)objHelperDB.GetGenericDataDB("", lstvperrecord, lstprerecord, lstcarecord, "GET_CATEGORY_DETAILS_BR", HelperDB.ReturnType.RTDataSet);

        //            string catdesc = "";
        //            string catename = "";
        //            if (subcatds != null && subcatds.Tables.Count >= 1)
        //            {
        //                //string catdesc = "";
        //                //string catename = "";
        //                catdesc = subcatds.Tables[0].Rows[0]["SHORT_DESC"].ToString();
        //                catename = subcatds.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
        //                _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", catdesc);
        //                _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", catename);
        //                // System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString());
        //                string imgpath = subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().Replace("\\", "/");

        //                if (subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().ToLower().Contains("_th") == false)
        //                {
        //                    imgpath = objHelperService.SetImageFolderPath(imgpath, "_images", "_th");
        //                }
        //                System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + imgpath);

        //                if (Fil.Exists)
        //                    // _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().Replace("\\", "/"));
        //                    _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", imgpath);
        //                else
        //                {
        //                    _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
        //                }

        //            }
        //            else
        //            {
        //                _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
        //                _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", "");
        //                _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", "");
        //            }

        //            if (catename !="")
        //                _stmpl_container.SetAttribute("TBWC_CATEGORY_NAME", catename);
        //            else
        //                _stmpl_container.SetAttribute("TBWC_CATEGORY_NAME", category_nameh);

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


        //            if (WesNewsCategoryId == _ParentCatID) // WES NEW ONLy
        //            {
        //                SetEbookAndPDFLink(_stmpl_container);
        //                // _stmpl_container.SetAttribute("TBT_DISPLAY_DIV_PAGE", false);
        //            }
        //            else
        //            {
        //                _stmpl_container.SetAttribute("TBT_DISPLAY_LINK", "none");
        //                _stmpl_container.SetAttribute("TBT_DISPLAY_DIV_PAGE", true);
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

        //            else
        //            {
        //                iNextPgNo = 1;
        //                iPrevPgNo = 1;
        //                iPageNo = iTotalPages;
        //            }
        //            //try
        //            //{

        //            //    _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
        //            //    if (iPageNo > 2 && (iTotalPages >= (iPageNo + 2)))
        //            //    {


        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo - 2);
        //            //        SetQueryString(_stmpl_pages);


        //            //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

        //            //        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo - 1);
        //            //        SetQueryString(_stmpl_pages);
        //            //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

        //            //        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo);
        //            //        SetQueryString(_stmpl_pages);
        //            //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));

        //            //        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo + 1);
        //            //        SetQueryString(_stmpl_pages);
        //            //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

        //            //        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo + 2);
        //            //        SetQueryString(_stmpl_pages);
        //            //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
        //            //    }
        //            //    else if (iPageNo > 0 && iPageNo < 4 && iPageNo < iTotalPages)
        //            //    {

        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", 1);
        //            //        SetQueryString(_stmpl_pages);

        //            //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //        if (iPageNo == 1)
        //            //        {
        //            //            if (1 == iTotalPages)

        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
        //            //            else
        //            //            {

        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));

        //            //            }
        //            //        }
        //            //        else
        //            //        {
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
        //            //        }

        //            //        if (2 <= iTotalPages)
        //            //        {
        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 2);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            if (iPageNo == 2)
        //            //            {
        //            //                if (2 == iTotalPages)
        //            //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
        //            //                else
        //            //                {
        //            //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));
        //            //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                }
        //            //            }
        //            //            else
        //            //            {


        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
        //            //            }
        //            //        }

        //            //        if (3 <= iTotalPages)
        //            //        {
        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 3);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            if (iPageNo == 3)
        //            //            {
        //            //                if (3 == iTotalPages)
        //            //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
        //            //                else
        //            //                {
        //            //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));
        //            //                }
        //            //            }
        //            //            else
        //            //            {

        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
        //            //            }
        //            //        }
        //            //        if (4 <= iTotalPages)
        //            //        {
        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 4);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            if (iPageNo == 4)
        //            //            {
        //            //                if (4 == iTotalPages)
        //            //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
        //            //                else
        //            //                {
        //            //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));

        //            //                }
        //            //            }
        //            //            else
        //            //            {

        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
        //            //            }

        //            //        }
        //            //        if (5 <= iTotalPages)
        //            //        {
        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 5);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            if (iPageNo == 5)
        //            //            {

        //            //                if (4 == iTotalPages)
        //            //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("\r", "").Replace("\n", "").Replace("-</td>", "</td>"));
        //            //                else
        //            //                {
        //            //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("\r", "").Replace("\n", "").Replace("-</td>", "</td>"));
        //            //                }
        //            //            }
        //            //            else
        //            //            {
        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
        //            //            }

        //            //        }
        //            //    }
        //            //    else
        //            //        if (iPageNo == iTotalPages && 1 <= iTotalPages - 4 && iPageNo < iTotalPages)
        //            //        {

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 4);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
        //            //            SetQueryString(_stmpl_pages);
        //            //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //            if (iPageNo == iTotalPages)
        //            //            {
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
        //            //            }
        //            //            else
        //            //            {
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
        //            //            }

        //            //        }
        //            //        else if (iPageNo == iTotalPages && iPageNo < iTotalPages)
        //            //        {

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //            if (iPageNo == iTotalPages)
        //            //            {
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
        //            //            }
        //            //            else
        //            //            {
        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
        //            //            }

        //            //        }
        //            //        else if ((1 <= iTotalPages - 4 && iPageNo < iTotalPages) || (1 <= iTotalPages - 4 && iPageNo == iTotalPages))
        //            //        {
        //            //            if (iTotalPages - 4 > 0)
        //            //            {

        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 4);
        //            //                SetQueryString(_stmpl_pages);

        //            //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
        //            //            }


        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());




        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);

        //            //            SetQueryString(_stmpl_pages);
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());



        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");

        //            //            if (iPageNo != iTotalPages)
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
        //            //            else
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");


        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
        //            //            SetQueryString(_stmpl_pages);
        //            //            if (iPageNo == iTotalPages)
        //            //            {
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
        //            //            }
        //            //            else
        //            //            {

        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
        //            //            }


        //            //        }


        //            //        else if (iPageNo == iTotalPages)
        //            //        {

        //            //            if (iTotalPages - 3 > 0)
        //            //            {

        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
        //            //                SetQueryString(_stmpl_pages);

        //            //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
        //            //            }
        //            //            if (iTotalPages - 2 > 0)
        //            //            {
        //            //                _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
        //            //                SetQueryString(_stmpl_pages);

        //            //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
        //            //            }
        //            //            if (iTotalPages - 1 > 0)
        //            //            {
        //            //                _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
        //            //                SetQueryString(_stmpl_pages);

        //            //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
        //            //            }
        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
        //            //            SetQueryString(_stmpl_pages);

        //            //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
        //            //            if (iPageNo == iTotalPages)
        //            //            {
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
        //            //            }
        //            //            else
        //            //            {
        //            //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
        //            //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
        //            //            }

        //            //        }
        //            //    if (iTotalPages > 1 && iPageNo != iTotalPages && iPageNo < iTotalPages)
        //            //    {


        //            //        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpagenoNext");
        //            //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", (iPageNo + 1));
        //            //        SetQueryString(_stmpl_pages);
        //            //         _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);

        //            //        _stmpl_container.SetAttribute("TBW_NEXT_PAGE", _stmpl_pages.ToString());




        //            //    }
        //            //    else
        //            //    {

        //            //        if (iPageNo != iTotalPages)
        //            //        {

        //            //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpagenoNext");
        //            //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", (iPageNo));
        //            //            SetQueryString(_stmpl_pages);
        //            //            _stmpl_container.SetAttribute("TBW_NEXT_PAGE", _stmpl_pages.ToString());
        //            //        }
        //            //        else
        //            //        {
        //            //            _stmpl_container.SetAttribute("TBW_NEXT_PAGE", "");
        //            //        }
        //            //    }
        //            //}
        //            //catch (Exception ex)
        //            //{

        //            //}
        //            //if (iRecordsPerPage == 32767) //View All
        //            //{
        //            //    iRecordsPerPage = iTotalProducts;
        //            //}
        //            _stmpl_container.SetAttribute("TBW_START_PAGE_NO", (iPageNo * iRecordsPerPage) - (iRecordsPerPage - 1));
        //            if (((iPageNo * iRecordsPerPage) > iTotalProducts))
        //                _stmpl_container.SetAttribute("TBW_END_PAGE_NO", iTotalProducts);
        //            else
        //                _stmpl_container.SetAttribute("TBW_END_PAGE_NO", (iPageNo * iRecordsPerPage));

        //            if (iTotalPages > 1 && iPageNo != iTotalPages && iPageNo < iTotalPages)
        //            {
        //                _stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
        //                _stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo);
        //                _stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
        //                _stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo);
        //                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
        //                sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
        //            }
        //            else
        //            {
        //                iPageNo = iTotalPages;
        //                _stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
        //                _stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo);
        //                _stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
        //                _stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo);
        //                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
        //                sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
        //            }

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
        //                sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1\">Previous</a>", "");
        //                sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1\">Next</a>", "");
        //            }
        //            DataSet DSnaprod = new DataSet();


        //        }

        //        string orgurl = HttpContext.Current.Request.Url.PathAndQuery.ToString();
        //        string eapath = _EAPath.Replace("'", "###");
        //        string bceapath = _BCEAPath.Replace("'", "###");  

        //        htmleapath.Value = eapath.ToString();
        //        htmlbceapath.Value = bceapath.ToString();   
        //        htmltotalpages.Value = iTotalPages.ToString();
        //        htmlviewmode.Value = _ViewType;
        //        if (Session["RECORDS_PER_PAGE_PRODUCT_LIST"] != null)
        //        {
        //            htmlirecords.Value = Session["RECORDS_PER_PAGE_PRODUCT_LIST"].ToString();
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

        //    }


        //    sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
        //    return objHelperServices.StripWhitespace(sHTML);
        //}
        public string ST_ProductListjson()
        {

            FamilyServices objFamilyServices = new FamilyServices();
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_pages = null;
            DataSet dsprod = new DataSet();
            DataSet dsprodspecs = new DataSet();
            DataSet subcatds = new DataSet();
            string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();

            int oe = 0;
            string category_nameh = "";
            string sHTML = "";
            string _pcr = "";
            string _ViewType = "";
            string _BCEAPath = "";

            string lstcarecord = "";
            string lstprerecord = "";
            string lstvperrecord = "";
            DataSet breadcrumb = new DataSet();
            HelperServices objHelperService = new HelperServices();
            //string CID_new = "";
            //string PCID_new = "";

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

                if (Request.QueryString["pcr"] != null)
                    _pcr = Request.QueryString["pcr"];



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




                //if (Request.QueryString["path"] != null)
                //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(Request.QueryString["path"].ToString()));

                if (HttpContext.Current.Session["EA"] != null)
                {
                    _EAPath = HttpContext.Current.Session["EA"].ToString();
                }
                if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
                {
                    _BCEAPath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();
                }



                if (Request.Url.ToString().ToLower().Contains("mpl.aspx"))
                {
                    //ps.ClearFilterproduct("DELETE TBWC_SEARCH_PROD_LIST WHERE PRODUCT_ID IN(SELECT DISTINCT F.FAMILY_ID FROM TB_FAMILY F,TB_FAMILY_SPECS FS WHERE F.FAMILY_ID=FS.FAMILY_ID AND ISNULL(FS.STRING_VALUE,'')='NOT FOR WEB' AND F.CATEGORY_ID='" + ps.CATEGORY_ID + "') AND USER_SESSION_ID='" + Session.SessionID + "'");
                    // dscat = ps.GetFamilies();
                    dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                }
                else
                {
                    // dscat = ps.GetProducts();
                }



                if (dscat != null)
                {

                    if (Request.QueryString["pcr"] != null)
                        _pcr = Request.QueryString["pcr"];
                    if (Request.QueryString["type"] == null)
                    {
                        //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
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
                        //ps.PAGE_NO = iPageNo;
                    }
                    Session["iTotalPages"] = iTotalPages;
                    DataRow drproductsct = dscat.Tables[1].Rows[0];
                    iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
                    if (_cid != "")
                        _ParentCatID = GetParentCatID(_cid);

                    if ((iTotalPages <= 0 || iTotalProducts <= 0) && WesNewsCategoryId == _ParentCatID)
                    {


                        _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("ProductList" + "\\" + "advmain");

                        SetEbookAndPDFLink(_stmpl_container);
                        DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
                        if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
                            _stmpl_container.SetAttribute("TBT_ADV_URL_LINK", tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString());

                        _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh.ToUpper());
                        return _stmpl_container.ToString();
                    }



                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];
                    _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);


                    int ictrecords = 0;
                    int icolstart = 0;
                    string trmpstr = "";
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

                    // DataRow[] drprodcoll = dscat.Tables["FamilyPro"].Select("", "SNO");


                    string userid = "";
                    string PriceTable = "";
                    int pricecode = 0;
                    string tmpProds = "";
                    DataSet dsBgDisc = new DataSet();
                    DataSet dsPriceTableAll = new DataSet();
                    if (Session["USER_ID"] != null)
                        userid = Session["USER_ID"].ToString();
                    if (userid == "")
                        // userid = "0";
                        userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

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
                    bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);

                    lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count + 1];
                    string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
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
                        _stmpl_records = _stg_records.GetInstanceOf("ProductList" + "\\" + "productlist_GridView");
                        //else
                        //    _stmpl_records = _stg_records.GetInstanceOf("ProductList" + "\\" + "productlist_WES" + soddevenrow);

                        string urlDesc = "";

                        if (_ViewType == "GV")
                            _stmpl_records.SetAttribute("LISTVIEW_CLASS", "");
                        else
                            _stmpl_records.SetAttribute("LISTVIEW_CLASS", "list-group-item");



                        _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());
                        _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"].ToString());
                        _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"].ToString());

                        _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                        _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));

                        _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
                        _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());

                        trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                        //if (_ViewType == "GV")
                        // {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                        //}

                        _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);
                        _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", trmpstr.Replace('"', ' '));
                        _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
                        _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());

                        _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomEnabled);

                        string[] catpath = null;
                        string categorypath = string.Empty;
                        //if (drpid["CATEGORY_PATH"].ToString() != null)
                        //{

                        //    catpath = drpid["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                        //    categorypath = catpath[0] + "////" + catpath[1];
                        //}
                        catpath = HttpContext.Current.Request.RawUrl.Split('/');
                        if (catpath.Length > 2)
                        {
                            categorypath = (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ");
                        }
                        else
                        {
                            categorypath = (catpath.Length >= 2 ? catpath[1] : " ");
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


                            // _stmpl_records.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));

                            if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                                _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);

                            else
                                _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);


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


                            //DataRow[] drow = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND ATTRIBUTE_TYPE=0");
                            //if (drow.Length > 0) // Data Rows must return 1 row 
                            //{
                            //    DataTable td = drow.CopyToDataTable();
                            //    _stmpl_records.SetAttribute("TBT_USER_PRICE", td.Rows[0]["NUMERIC_VALUE"].ToString());

                            //    if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) != -1)
                            //    {
                            //        if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) > 0)
                            //        {
                            //            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", td.Rows[0]["QTY_AVAIL"].ToString());
                            //            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", td.Rows[0]["MIN_ORD_QTY"].ToString());
                            //        }
                            //        else
                            //        {
                            //            _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                            //        }
                            //        //_stmpl_records.SetAttribute("TBT_PRODUCT_ID", td.Rows[0]["PRODUCT_ID"].ToString());
                            //    }
                            //}

                            string newurl = string.Empty;


                            newurl = WAG_Root_Path + "////" + categorypath + "////" +
                                               drpid["FAMILY_NAME"] + "////" +
                                             drpid["PRODUCT_ID"] + "=" + drpid["PRODUCT_CODE"] + "////"
                                             + drpid["FAMILY_ID"];


                            objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "mpd.aspx");

                        }


                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"].ToString());
                        //modified by indu:To minimize orderno length grater than 7" 
                        if (drpid["PRODUCT_CODE"].ToString().Length <= 8)
                        {
                            _stmpl_records.SetAttribute("TBT_ORG_ORDERNO", true);
                        }
                        //else
                        //{
                        //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1_min", drpid["PRODUCT_CODE"].ToString().Substring (0,7)+"..");
                        //}



                        // System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                        // if (Fil.Exists)
                        if (!drpid["Prod_Thumbnail"].ToString().Contains("_images_200"))
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", objHelperService.SetImageFolderPath(drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"), "_th", "_images_200"));
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                        }
                        //  else
                        //     _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", "/images/noimage.gif");


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
                        //        _stmpl_records.SetAttribute("TBT_MORE_13", true);
                        //        desc = desc.Substring(0, 230).ToString();
                        //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_13", desc);
                        //    }
                        //    else
                        //    {
                        //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_13", desc);
                        //        _stmpl_records.SetAttribute("TBT_MORE_13", false);
                        //    }
                        //    desc = drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        //    if (desc.Length > 250 && _ViewType == "LV")
                        //    {
                        //        _stmpl_records.SetAttribute("TBT_MORE_90", true);
                        //        desc = desc.Substring(0, 230).ToString();
                        //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_90", desc);
                        //    }
                        //    else
                        //    {
                        //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_90", desc);
                        //        _stmpl_records.SetAttribute("TBT_MORE_90", false);
                        //    }


                        //}
                        //else
                        {

                            descattr = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                            if (descattr.Length > 0)
                                descattr = descattr + "<br/>";
                            descattr = descattr + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
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
                                _stmpl_records.SetAttribute("PROD_DESC_TITLE", prod_desc_alt.Replace('"', ' '));
                            }
                            if (descattr.Length > 140) // && _ViewType == "GV"
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


                        lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());

                        //lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                        icolstart++;
                        if (icolstart >= icol || oe == dscat.Tables["FamilyPro"].Rows.Count)
                        {
                            // if (icolstart == icol || iTotalProducts < 24)
                            // {
                            _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
                            _stmpl_container = _stg_container.GetInstanceOf("ProductList" + "\\" + "producttable_" + soddevenrow);
                            _stmpl_container.SetAttribute("TBWDataList", lstrows);
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                            //  }
                            //if (icolstart == 3 || icolstart == 2 || icolstart == 1)
                            //{
                            //  balrecords_cnt.Value = icolstart.ToString();
                            //}

                            ictrecords++;
                            icolstart = 0;
                            lstrows = new TBWDataList[icol];

                        }
                    }

                    //   }

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


                    _stmpl_container = _stg_container.GetInstanceOf("ProductList" + "\\" + "main");
                    _stmpl_container.SetAttribute("TBW_CATEGORY_ID", _cid);
                    _stmpl_container.SetAttribute("TBW_PRODUCT_COUNT", iTotalProducts);

                    _stmpl_container.SetAttribute("ST_TOP_FILTER", ST_Top_filter(stemplatepath));
                    // _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);


                    /*
                                    breadcrumb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                                    DataRow[] _DCRow = null;
                                    _DCRow = breadcrumb.Tables[0].Select("ItemType='Category'");
                                    if (_DCRow != null && _DCRow.Length > 0)
                                    {
                                        foreach (DataRow _drow in _DCRow)
                                        {
                                            if (_DCRow.Length == 3)
                                            {
                                                if (_drow == _DCRow[_DCRow.Length - 1])
                                                {
                                                    lstcarecord = _drow["ItemValue"].ToString();
                                                }
                                                if (_drow == _DCRow[_DCRow.Length - 2])
                                                {
                                                    lstprerecord = _drow["ItemValue"].ToString();
                                                }

                                                if (_drow == _DCRow[_DCRow.Length - 3])
                                                {
                                                    lstvperrecord = _drow["ItemValue"].ToString();
                                                }
                                            }
                                            else if (_DCRow.Length == 2)
                                            {
                                                if (_drow == _DCRow[_DCRow.Length - 1])
                                                {
                                                    lstprerecord = _drow["ItemValue"].ToString();
                                                }
                                                if (_drow == _DCRow[_DCRow.Length - 2])
                                                {
                                                    lstvperrecord = _drow["ItemValue"].ToString();
                                                }

                                            }
                                            else
                                            {
                                                if (_drow == _DCRow[_DCRow.Length - 1])
                                                {
                                                    lstprerecord = _drow["ItemValue"].ToString();
                                                }
                           
                                            }
                                        }
                                    }

                                    if (lstcarecord != null && lstcarecord != "" && lstprerecord != null && lstprerecord != "" && lstvperrecord != null && lstvperrecord != "")

                                        subcatds = (DataSet)objHelperDB.GetGenericDataDB("", lstvperrecord, lstprerecord, lstcarecord, "GET_CATEGORY_DETAILS", HelperDB.ReturnType.RTDataSet);
                                    else
                                        subcatds = (DataSet)objHelperDB.GetGenericDataDB("", lstvperrecord, lstprerecord, lstcarecord, "GET_CATEGORY_DETAILS_BR", HelperDB.ReturnType.RTDataSet);

                                    string catdesc = "";
                                    string catename = "";
                                    if (subcatds != null && subcatds.Tables.Count >= 1)
                                    {
                                        //string catdesc = "";
                                        //string catename = "";
                                        catdesc = subcatds.Tables[0].Rows[0]["SHORT_DESC"].ToString();
                                        catename = subcatds.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                                        _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", catdesc);
                                        _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", catename);
                                        // System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString());
                                        string imgpath = subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().Replace("\\", "/");

                                        if (subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().ToLower().Contains("_th") == false)
                                        {
                                            imgpath = objHelperService.SetImageFolderPath(imgpath, "_images", "_200");
                                        }
                                        System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + imgpath);

                                        if (Fil.Exists)
                                            // _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().Replace("\\", "/"));
                                            _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", imgpath);
                                        else
                                        {
                                            _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
                                        }

                                    }
                                    else
                                    {
                                        _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
                                        _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", "");
                                        _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", "");
                                    }

                                    if (catename != "")
                                        _stmpl_container.SetAttribute("TBWC_CATEGORY_NAME", catename);
                                    else
                                        _stmpl_container.SetAttribute("TBWC_CATEGORY_NAME", category_nameh);

                                    _stmpl_container.SetAttribute("TBW_URL", powersearchURListView);
                                    _stmpl_container.SetAttribute("TBW_URL1", powersearchURLGridView);



                                    if (_ViewType == "LV")
                                        _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
                                    else
                                        _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);




                                    _stmpl_container.SetAttribute("SELECT_" + iRecordsPerPage, "SELECTED=\"SELECTED\" ");

                                    if (iTotalPages > 1)
                                    {
                                        if (iPageNo != iTotalPages)
                                        {
                                            _stmpl_container.SetAttribute("TBW_TO_PAGE", true);
                                        }
                                        else if (iPageNo == iTotalPages)
                                        {
                                            _stmpl_container.SetAttribute("TBW_TOTAL_PAGE", true);
                                        }
                                    }
                                    else
                                    {
                                        _stmpl_container.SetAttribute("TBW_TO_PAGE", false);
                                    }


                                    if (WesNewsCategoryId == _ParentCatID) // WES NEW ONLy
                                    {
                                        SetEbookAndPDFLink(_stmpl_container);
                                        // _stmpl_container.SetAttribute("TBT_DISPLAY_DIV_PAGE", false);
                                    }
                                    else
                                    {
                                        _stmpl_container.SetAttribute("TBT_DISPLAY_LINK", "none");
                                        _stmpl_container.SetAttribute("TBT_DISPLAY_DIV_PAGE", true);
                                    }


                                    */

                    if (iPageNo < iTotalPages)
                    {
                        if (iPageNo > 1)
                        {
                            iPrevPgNo = iPageNo - 1;
                        }
                        else
                        {
                            iPrevPgNo = 1;
                        }
                        iNextPgNo = iPageNo + 1;
                    }

                    else
                    {
                        iNextPgNo = 1;
                        iPrevPgNo = 1;
                        iPageNo = iTotalPages;
                    }

                    // _stmpl_container.SetAttribute("TBW_START_PAGE_NO", (iPageNo * iRecordsPerPage) - (iRecordsPerPage - 1));
                    //  if (((iPageNo * iRecordsPerPage) > iTotalProducts))
                    //   _stmpl_container.SetAttribute("TBW_END_PAGE_NO", iTotalProducts);
                    // else
                    //  _stmpl_container.SetAttribute("TBW_END_PAGE_NO", (iPageNo * iRecordsPerPage));

                    //if (iTotalPages > 1 && iPageNo != iTotalPages && iPageNo < iTotalPages)
                    //{
                    //    _stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
                    //    _stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo);
                    //    _stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
                    //    _stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo);
                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
                    //}
                    //else
                    //{
                    //    iPageNo = iTotalPages;
                    //    _stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
                    //    _stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo);
                    //    _stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
                    //    _stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo);
                    //    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    //    sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
                    //}

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
                        sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1\">Previous</a>", "");
                        sHTML = sHTML.Replace("<a href=\"ps.aspx?pgno=1\">Next</a>", "");
                    }
                    DataSet DSnaprod = new DataSet();


                }

                string orgurl = HttpContext.Current.Request.Url.PathAndQuery.ToString();
                string eapath = _EAPath.Replace("'", "###");
                string bceapath = _BCEAPath.Replace("'", "###");

                htmleapath.Value = eapath.ToString();
                htmlbceapath.Value = bceapath.ToString();
                htmltotalpages.Value = iTotalPages.ToString();
                htmlviewmode.Value = _ViewType;
                if (Session["RECORDS_PER_PAGE_PRODUCT_LIST"] != null)
                {
                    htmlirecords.Value = Session["RECORDS_PER_PAGE_PRODUCT_LIST"].ToString();
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

            }


            sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
            return objHelperServices.StripWhitespace(sHTML);
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

        //private decimal GetMyPrice(int ProductID)
        //{
        //    decimal retval = 0.00M;
        //    string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //    if (!string.IsNullOrEmpty(userid))
        //    {

        //        string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
        //        oHelper.SQLString = sSQL;
        //        int pricecode = oHelper.CI(oHelper.GetValue("price_code"));

        //        string strquery = "";
        //        if (pricecode == 1)
        //        {
        //            strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
        //        }
        //        else
        //        {
        //            strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
        //        }

        //        DataSet DSprice = new DataSet();
        //        oHelper.SQLString = strquery;
        //        retval = Math.Round(Convert.ToDecimal(oHelper.GetValue("Numeric_Value")), 2);
        //    }
        //    return retval;
        //}
        //private DataSet GetDataSet(string SQLQuery)
        //{
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(';') + 1));
        //    da.Fill(ds, "generictable");
        //    return ds;
        //}

        private string GetParentCatID(string catID)
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

        private void SetEbookAndPDFLink(StringTemplate _stmpl_container)
        {
            DataSet dsCatalog = objCategoryServices.GetCatalogPDFDownload1(_cid);

            if (dsCatalog != null && dsCatalog.Tables.Count > 0 && dsCatalog.Tables[0].Rows.Count > 0)
            {

                FileInfo Fil1 = new FileInfo(strPDFFiles1 + dsCatalog.Tables[0].Rows[0]["IMAGE_FILE2"].ToString());

                if (dsCatalog.Tables[0].Rows[0]["IMAGE_NAME"].ToString() != "" || Fil1.Exists)
                {
                    _stmpl_container.SetAttribute("TBT_DISPLAY_LINK", "block");



                    if (dsCatalog.Tables[0].Rows[0]["IMAGE_NAME"].ToString() != "")
                    {
                        _stmpl_container.SetAttribute("TBT_EBOOK_LINK", dsCatalog.Tables[0].Rows[0]["IMAGE_NAME"].ToString());
                        _stmpl_container.SetAttribute("TBT_DISPLAY_EBOOK_LINK", "block");
                    }
                    else
                    {
                        _stmpl_container.SetAttribute("TBT_EBOOK_LINK", "");
                        _stmpl_container.SetAttribute("TBT_DISPLAY_EBOOK_LINK", "none");
                    }

                    if (dsCatalog.Tables[0].Rows[0]["CUSTOM_TEXT_FIELD3"].ToString() != "")
                    {
                        _stmpl_container.SetAttribute("TBT_HTML_LINK", dsCatalog.Tables[0].Rows[0]["CUSTOM_TEXT_FIELD3"].ToString());
                        _stmpl_container.SetAttribute("TBT_DISPLAY_HTML_LINK", "block");
                    }
                    else
                    {
                        _stmpl_container.SetAttribute("TBT_HTML_LINK", "");
                        _stmpl_container.SetAttribute("TBT_DISPLAY_HTML_LINK", "none");
                    }




                    if (Fil1.Exists)
                    {
                        _stmpl_container.SetAttribute("PDF", dsCatalog.Tables[0].Rows[0]["IMAGE_FILE2"].ToString());
                        _stmpl_container.SetAttribute("TBT_DISPLAY_PDF_LINK", "block");
                    }
                    else
                        _stmpl_container.SetAttribute("TBT_DISPLAY_PDF_LINK", "none");

                }
                else
                    _stmpl_container.SetAttribute("TBT_DISPLAY_LINK", "none");


            }
            else
                _stmpl_container.SetAttribute("TBT_DISPLAY_LINK", "none");
        }


        //public string DynamicPag(string url, int ipageno, string eapath, string bceapath, string ViewMode, string irecords)
        //{
        //    //if (HttpContext.Current.Session["PS_SEARCH_RESULTS"] == null)
        //    //{
        //    //    return "";
        //    //}
        //    HelperServices objHelperServices = new HelperServices();
        //    ErrorHandler objErrorHandler = new ErrorHandler();
        //    CategoryServices objCategoryServices = new CategoryServices();
        //    Security objSecurity = new Security();
        //    HelperDB objHelperDB = new HelperDB();
        //    FamilyServices objFamilyServices = new FamilyServices();
        //    StringTemplateGroup _stg_container = null;
        //    StringTemplateGroup _stg_records = null;
        //    StringTemplate _stmpl_container = null;
        //    StringTemplate _stmpl_records = null;
        //    StringTemplate _stmpl_pages = null;
        //    DataSet dsprod = new DataSet();
        //    DataSet dsprodspecs = new DataSet();
        //    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
        //    ProductServices objProductServices = new ProductServices();
        //    string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
        //    string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
        //    string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        //    stemplatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
        //    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\product_list\\";
        //    string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
        //    string _BCEAPath=string.Empty ; 

        //    int oe = 0;
        //    string category_nameh = "";
        //    string sHTML = "";
        //    string _pcr = "";
        //    string _ViewType = "";

        //    //if (Convert.ToInt32(HttpContext.Current.Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        //    //{
        //    try
        //    {
        //        //int balrecords_int = 0;

        //        //if (balrecords == "3" || balrecords == "2" || balrecords == "1")
        //        //{
        //        //    irecords = "20";
        //        //    balrecords = "0";
        //        //   // balrecords_cnt.Value = balrecords_int.ToString();
        //        //}
        //        //else
        //        //{
        //        //    irecords = "24";
        //        //    balrecords = "0";
        //        //   // balrecords_cnt.Value = balrecords_int.ToString();
        //        //}


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



        //        _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();


        //        _BCEAPath = bceapath;





        //        if (dscat != null)
        //        {

        //            if (HttpContext.Current.Request.QueryString["pcr"] != null)
        //                _pcr = HttpContext.Current.Request.QueryString["pcr"];
        //            if (HttpContext.Current.Request.QueryString["type"] == null)
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


        //                _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);
        //                _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "advmain");

        //                SetEbookAndPDFLink(_stmpl_container);
        //                DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
        //                if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
        //                    _stmpl_container.SetAttribute("TBT_ADV_URL_LINK", tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString());

        //                _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh.ToUpper());
        //                return _stmpl_container.ToString();
        //            }



        //            TBWDataList[] lstrecords = new TBWDataList[0];
        //            TBWDataList[] lstrows = new TBWDataList[0];
        //            _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);


        //            int ictrecords = 0;
        //            int icolstart = 0;
        //            string trmpstr = "";
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


        //            string soddevenrow = "odd";

        //            DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1", "SNO");


        //           // string userid = "";
        //            string PriceTable = "";
        //            int pricecode = 0;
        //            string tmpProds = "";
        //            DataSet dsBgDisc = new DataSet();
        //            DataSet dsPriceTableAll = new DataSet();
        //            //if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"]!="")
        //            //    userid = HttpContext.Current.Session["USER_ID"].ToString();
        //            //if (userid == "")
        //            //{
        //               // userid = System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
        //            //}

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


        //            lstrecords = new TBWDataList[drprodcoll.Length + 1];
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
        //                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_WES" + soddevenrow);

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


        //                if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
        //                {
        //                    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
        //                    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
        //                    _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());
        //                    // EA Price Call    
        //                    //modified by :indu
        //                    string newurl = string.Empty;
        //                    newurl = _BCEAPath + "////" +
        //                                        drpid["FAMILY_ID"].ToString() + 
        //                                         "="+_stmpl_records.GetAttribute("FAMILY_NAME");

        //                    objHelperServices.Cons_NewURl(_stmpl_records, newurl, "fl.aspx", true, "", true);

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
        //                    foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
        //                    {
        //                        if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
        //                        {
        //                            if (dr["ATTRIBUTE_ID"].ToString() == "1")
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
        //                            else
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
        //                        }
        //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
        //                        {
        //                            if (dr["NUMERIC_VALUE"].ToString().Length > 0)
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString());
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
        //                        else
        //                        {
        //                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
        //                        }
        //                        if (_stmpl_records.GetAttribute("TBT_SUB_FAMILY").ToString() == "False")
        //                        {
        //                            string newurl = string.Empty;

        //                            newurl = _BCEAPath + "////" + 
        //                                        drpid["FAMILY_ID"].ToString()  +
        //                                          "=" + _stmpl_records.GetAttribute("FAMILY_NAME") +
        //                                         "////" +
        //                                          drpid["PRODUCT_ID"].ToString() + "=" + dr["PRODUCT_CODE"].ToString()
        //                                        ;
        //                            //Added by:Indu

        //                            _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", dr["ATTRIBUTE_TYPE"].ToString());
        //                            objHelperServices.Cons_NewURl(_stmpl_records, newurl, "pd.aspx", true, dr["ATTRIBUTE_TYPE"].ToString(), true);
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
        //                    string descattr = "";
        //                    string desc = "";
        //                    foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
        //                    {

        //                        if (dr["ATTRIBUTE_TYPE"].ToString() == "9")
        //                        {
        //                            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString());
        //                            if (Fil.Exists)
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
        //                            else
        //                            {
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
        //                            }

        //                        }
        //                        else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
        //                        {

        //                            // desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;");
        //                             desc = dr["STRING_VALUE"].ToString().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
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
        //                        }
        //                        else
        //                        {

        //                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>"));
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
        //                    {
        //                        _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
        //                    }
        //                }




        //                lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());

        //                //lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
        //                icolstart++;
        //                if (icolstart >= icol || oe == drprodcoll.Length)
        //                {
        //                    _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
        //                    _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "producttable_" + soddevenrow);
        //                    _stmpl_container.SetAttribute("TBWDataList", lstrows);
        //                    lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
        //                    sHTML = sHTML + _stmpl_container.ToString();
        //                    ictrecords++;
        //                    icolstart = 0;
        //                    lstrows = new TBWDataList[icol];
        //                    //if (icolstart == icol || iTotalProducts < 24)
        //                    //{
        //                    //    _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
        //                    //    _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "producttable_" + soddevenrow);
        //                    //    _stmpl_container.SetAttribute("TBWDataList", lstrows);
        //                    //    lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
        //                    //    sHTML = sHTML + _stmpl_container.ToString();
        //                    //}
        //                    //if (icolstart == 3 || icolstart == 2 || icolstart == 1)
        //                    //{
        //                    //    HiddenField balrecords_cnt = new HiddenField();
        //                    //    balrecords_cnt.Value = icolstart.ToString();
        //                    //}
        //                    //ictrecords++;
        //                    //icolstart = 0;
        //                    //lstrows = new TBWDataList[icol];
        //                }
        //            }

        //            //   }

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


        //    sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
        //    return objHelperServices.StripWhitespace(sHTML);
        //}
        public string DynamicPagJson(string url, int ipageno, string eapath, string bceapath, string ViewMode, string irecords)
        {

            HelperServices objHelperServices = new HelperServices();
            ErrorHandler objErrorHandler = new ErrorHandler();
            CategoryServices objCategoryServices = new CategoryServices();
            Security objSecurity = new Security();
            HelperDB objHelperDB = new HelperDB();
            FamilyServices objFamilyServices = new FamilyServices();
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_pages = null;
            DataSet dsprod = new DataSet();
            DataSet dsprodspecs = new DataSet();
            EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
            ProductServices objProductServices = new ProductServices();
            string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
            HelperServices objHelperService = new HelperServices();
            string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
            string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
            // stemplatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());

            stemplatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
            string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
            string _BCEAPath = string.Empty;

            int oe = 0;
            string category_nameh = "";
            string sHTML = "";
            string _pcr = "";
            string _ViewType = "";


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

                if (HttpContext.Current.Request.QueryString["pcr"] != null)
                    _pcr = HttpContext.Current.Request.QueryString["pcr"];








                _ViewType = ViewMode;



                _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();


                _BCEAPath = bceapath;





                if (dscat != null)
                {

                    if (HttpContext.Current.Request.QueryString["pcr"] != null)
                        _pcr = HttpContext.Current.Request.QueryString["pcr"];
                    if (HttpContext.Current.Request.QueryString["type"] == null)
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

                    if ((iTotalPages <= 0 || iTotalProducts <= 0) && WesNewsCategoryId == _ParentCatID)
                    {


                        _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("ProductList" + "\\" + "advmain");

                        SetEbookAndPDFLink(_stmpl_container);
                        DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
                        if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
                            _stmpl_container.SetAttribute("TBT_ADV_URL_LINK", tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString());

                        _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh.ToUpper());
                        return _stmpl_container.ToString();
                    }



                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];
                    _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);


                    int ictrecords = 0;
                    int icolstart = 0;
                    string trmpstr = "";
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


                    string soddevenrow = "odd";

                    // DataRow[] drprodcoll = dscat.Tables["FamilyPro"].Select("", "SNO");


                    // string userid = "";
                    string PriceTable = "";
                    int pricecode = 0;
                    string tmpProds = "";
                    DataSet dsBgDisc = new DataSet();
                    DataSet dsPriceTableAll = new DataSet();
                    //if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"]!="")
                    //    userid = HttpContext.Current.Session["USER_ID"].ToString();
                    //if (userid == "")
                    //{
                    // userid = System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
                    //}

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

                    bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);
                    lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count + 1];
                    string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
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

                        //  if (_ViewType == "GV")
                        _stmpl_records = _stg_records.GetInstanceOf("ProductList" + "\\" + "productlist_GridView");
                        //  else
                        //     _stmpl_records = _stg_records.GetInstanceOf("ProductList" + "\\" + "productlist_WES" + soddevenrow);

                        string urlDesc = "";


                        if (_ViewType == "GV")
                            _stmpl_records.SetAttribute("LISTVIEW_CLASS", "");
                        else
                            _stmpl_records.SetAttribute("LISTVIEW_CLASS", "list-group-item");

                        _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());
                        _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"].ToString());
                        _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"].ToString());

                        _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                        _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch=Family Id=" + drpid["FAMILY_ID"].ToString())));

                        _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
                        _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());
                        trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                        //if (_ViewType == "GV")
                        {

                            if (trmpstr.Length > 60)
                                trmpstr = trmpstr.Substring(0, 60) + "...";
                        }

                        _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);
                        _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", trmpstr.Replace('"', ' '));
                        _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
                        _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());

                        _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomEnabled);

                        string[] catpath = null;
                        string categorypath = string.Empty;
                        //catpath = HttpContext.Current.Request.RawUrl.Split('/');
                        //if (catpath.Length > 2)
                        //{
                        //    categorypath = (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ");
                        //}
                        //else
                        //{
                        //    categorypath = (catpath.Length >= 2 ? catpath[1] : " ");
                        //}
                        if (drpid["CATEGORY_PATH"].ToString() != null)
                        {

                            string catpath1 = drpid["CATEGORY_PATH"].ToString().ToLower().Replace("supplier product feed////", "");


                            catpath = catpath1.Split(new string[] { "////" }, StringSplitOptions.None);
                            categorypath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ");
                        }

                        //catpath = HttpContext.Current.Request.RawUrl.Split('/');
                        //if (catpath.Length > 2)
                        //{
                        //    categorypath =  (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ");
                        //}
                        //else
                        //{
                        //    categorypath =  (catpath.Length >= 2 ? catpath[1] : " ");
                        //}

                        if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                        {
                            _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
                            _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
                            _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());
                            // EA Price Call    
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


                            // _stmpl_records.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) < 4 ? true : false));

                            if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                                _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);

                            else
                                _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);


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


                            string newurl = string.Empty;


                            newurl = WAG_Root_Path + "////" + categorypath + "////" +
                                               drpid["FAMILY_NAME"] + "////" +
                                             drpid["PRODUCT_ID"] + "=" + drpid["PRODUCT_CODE"] + "////"
                                             + drpid["FAMILY_ID"];


                            objHelperServices.SimpleURL_MS(_stmpl_records, newurl, "mpd.aspx");

                        }
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"].ToString());
                        //modified by indu:To minimize orderno length grater than 7" 
                        if (drpid["PRODUCT_CODE"].ToString().Length <= 8)
                        {
                            _stmpl_records.SetAttribute("TBT_ORG_ORDERNO", true);
                        }
                        //else
                        //{
                        //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1_min", drpid["PRODUCT_CODE"].ToString().Substring(0, 7) + "..");
                        //}
                        //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                        //if (Fil.Exists)
                        //{
                        if (!drpid["Prod_Thumbnail"].ToString().Contains("_images_200"))
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", objHelperService.SetImageFolderPath(drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"), "_th", "_images_200"));
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                        }
                        //}
                        //else
                        //{
                        //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", "/images/noimage.gif");
                        //}


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
                        //        _stmpl_records.SetAttribute("TBT_MORE_13", true);
                        //        desc = desc.Substring(0, 230).ToString();
                        //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_13", desc);
                        //    }
                        //    else
                        //    {
                        //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_13", desc);
                        //        _stmpl_records.SetAttribute("TBT_MORE_13", false);
                        //    }
                        //    desc = drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        //    if (desc.Length > 250 && _ViewType == "LV")
                        //    {
                        //        _stmpl_records.SetAttribute("TBT_MORE_90", true);
                        //        desc = desc.Substring(0, 230).ToString();
                        //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_90", desc);
                        //    }
                        //    else
                        //    {
                        //        _stmpl_records.SetAttribute("ATTRIBUTE_ID_90", desc);
                        //        _stmpl_records.SetAttribute("TBT_MORE_90", false);
                        //    }


                        //}
                        //else
                        {

                            descattr = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                            if (descattr.Length > 0)
                                descattr = descattr + "<br/>";
                            descattr = descattr + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
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
                                _stmpl_records.SetAttribute("PROD_DESC_TITLE", prod_desc_alt.Replace('"', ' '));
                            }
                            if (descattr.Length > 140) //.&& _ViewType == "GV"
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



                        lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());

                        //lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                        icolstart++;
                        if (icolstart >= icol || oe == dscat.Tables["FamilyPro"].Rows.Count)
                        {
                            _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
                            _stmpl_container = _stg_container.GetInstanceOf("ProductList" + "\\" + "producttable_" + soddevenrow);
                            _stmpl_container.SetAttribute("TBWDataList", lstrows);
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                            sHTML = sHTML + _stmpl_container.ToString();
                            ictrecords++;
                            icolstart = 0;
                            lstrows = new TBWDataList[icol];

                        }
                    }

                    //   }

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


            sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
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


                if (_catCid != "")
                    _parentCatID = GetParentCatID(_catCid);

                url = HttpContext.Current.Request.Url.OriginalString.ToLower();

                string EA = string.Empty;
                EA = eapath;

                //if (HttpContext.Current.Session["MainCategory"] != null)
                //{
                //    DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
                //    if (dr.Length > 0)
                //        _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
                //}
                string[] ConsURL = eapath.Replace("AttribSelect=", "").Split(new string[] { "////" }, StringSplitOptions.None);

                _value = ConsURL[ConsURL.Length - 1].Replace('_', ' ').Replace("\\", "/");
                if (_value.Contains("="))
                {
                    string[] Gettype = _value.Split(new string[] { "=" }, StringSplitOptions.None);
                    //  string AttribSelect = string.Empty;
                    _type = Gettype[0];
                    _value = Gettype[1].Replace('_', ' ').Replace("./.", "/");
                    // AttribSelect = "AttribSelect=" + _type + "='" + _value + "'";
                }

                else
                {
                    _type = "Category";

                }

                iPageNo = ipageno;

                if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true)
                {
                    //if (HttpContext.Current.Session["RECORDS_PER_PAGE_PRODUCT_LIST"] != null)
                    //    iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_PRODUCT_LIST"].ToString());

                    EA = eapath;

                    dscat = EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, irecords, (iPageNo - 1).ToString(), "Next", EA);
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
}
