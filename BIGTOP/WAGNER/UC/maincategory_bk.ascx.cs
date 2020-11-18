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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using TradingBell5.CatalogX;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Text;
using System.Diagnostics;
using System.Threading; 
public partial class UC_maincategory : System.Web.UI.UserControl
{
  

    #region "Declarations"    
    
   // Stopwatch stopwatch = new Stopwatch(); 
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    Security objSecurity = new Security();
    UserServices objUserServices = new UserServices();
    CategoryServices objCategoryServices = new CategoryServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    string CategoryName = string.Empty; 
    int iCatalogId;
   // int iInventoryLevelCheck;
    int iRecordsPerPage = 24;
   // bool bIsStartOver = true;
   // bool bDoPaging;
    int iPageNo = 1;
   // bool bSortAsc = true;
   // string _SearchString = "";
    string tempCID = string.Empty;
    string tempCName = string.Empty;
    string _tsbnew = string.Empty;
    string querystring = HttpContext.Current.Request.RawUrl.ToString().ToLower();
    //TreeView TVCategory = new TreeView();
  
   // string _TvrSkin;
    //String _CategoryText;
   // int _CatalogId=1;
   // string _ImagePath;
   // string _CategoryLogo;
   // string _FamilyLogo;
   // string _DisplayCategoryMode = "FLAT";
   // string _DisplayFamilyCount="NO";
   // string _DisplayProductCount="YES";
   // string _DisplayFamilyLogo="NO";
   // string _DisplayCategoryLogo="NO";
    //string _NaviWidth;
    //string _NaviHeight;
    //string _NodeExpanded;
   // string _CategoryHeaderText;
   // string _HeaderCssClass;
    string MCID = string.Empty;
    //string CID = "";
    //string ParentCatID = "";
   // string valuepath = "";
    string stemplatepath = string.Empty;
    string _catCid = string.Empty;
    string _parentCatID = string.Empty;
    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    DataSet dscat = new DataSet();
    string MicroSiteTemplate = string.Empty;
    public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
    public string EA_ROOT_CATEGORY_PATH = System.Configuration.ConfigurationManager.AppSettings["EA_ROOT_CATEGORY_PATH"].ToString();
    
    #endregion

    public enum DisplayMode
    {
        No = 0,
        YES = 1
    }
    DisplayMode _CategoryHeaderVisible;

    ConnectionDB conStrr = new ConnectionDB();


    //public string ST_Browsebycategory()
    //{

    //    if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "" && Request.QueryString["fid"].ToString() != "List all products")
    //    {
    //        CID = GetCIDD(Request.QueryString["fid"].ToString());
    //    }
    //    if ((Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "List all models") || CID != "" || Request.Url.ToString().ToLower().Contains("ps.aspx") == true)
    //    {
    //        HelperDB objHelperDB = new HelperDB();
    //        ConnectionDB objConnectionDB = new ConnectionDB();

    //        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BROWSEBYCATEGORY", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
    //        if (Request.QueryString["cid"] == null)
    //            tbwtEngine.paraValue = CID;
    //        else
    //            tbwtEngine.paraValue = Request.QueryString["cid"].ToString();
    //        tbwtEngine.RenderHTML("Row");
    //        return (tbwtEngine.RenderedHTML);
    //    }
    //    return "";
    //}

    private string GetCIDD(string familyid)
    {
        try
        {
            DataSet prodtable = new DataSet();
            prodtable = (DataSet)objHelperDB.GetGenericPageDataDB(familyid, "GET_MAINCATEGORY_CAREGORY_ID", HelperDB.ReturnType.RTDataSet);
            //SqlDataAdapter da = new SqlDataAdapter("select CATEGORY_ID from tb_family where family_id=" + familyid, conStrr.ConnectionString.ToString().Substring(conStrr.ConnectionString.ToString().IndexOf(';') + 1));
            //da.Fill(prodtable, "Producttable");
            //return prodtable.Tables[0].Rows[0].ItemArray[0].ToString();

            if (prodtable != null && prodtable.Tables.Count > 0)
                return prodtable.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            else
                return "";
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        MicroSiteTemplate = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
       // if (Request.RawUrl.ToLower().Contains("ct.aspx?") == true
       //    || Request.RawUrl.ToLower().Contains("fl.aspx?") == true
       //    || Request.RawUrl.ToLower().Contains("pl.aspx?") == true
       //    || Request.RawUrl.ToLower().Contains("pd.aspx?") == true
       //|| Request.RawUrl.ToLower().Contains("powersearch.aspx?") == true
       //    || Request.RawUrl.ToLower().Contains("bb.aspx?") == true)
        string reqrawurl = Request.RawUrl.ToLower();

        //if (((reqrawurl.Contains("_")) || reqrawurl.Contains("&") || reqrawurl.Contains("="))
        //    && !(reqrawurl.Contains("/ct/")) && !(reqrawurl.Contains("/bb/")))
        //{
        //    pageload();
        //}
        //else
        //{
            pageload_simple();
        //}
       
    }
    private void pageload()
    {
        try
        {

            //TVCategory.ID = "TVCategory";


            //TVCategory.class= _TvrSkin;
            //TVCategory.NodeStyle.BorderColor = System.Drawing.Color.White;
            //TVCategory.NodeStyle.BorderWidth =2;

            //TVCategory.CollapseAll();

            iCatalogId = Convert.ToInt32(WesCatalogId);

            stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());

            //if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "" && Request.QueryString["fid"].ToString() != "List all products")
            //{
            //    if(!Request.QueryString["fid"].ToString().Contains("~"))
            //    CID = GetCID(Request.QueryString["fid"].ToString());
            //}
            // if (Request.QueryString["cid"] != null || CID.ToString() != "")
            // {
            //     if (Request.QueryString["cid"] != null)
            //     {
            //         CID = GetMCatID(Request.QueryString["cid"].ToString());
            //     }
            // }

            string reqrawurl = Request.Url.ToString().ToLower();

            if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
            {
                _catCid = Request.QueryString["cid"];
            }


            else
            {

                if ((reqrawurl.Contains("pl.aspx")) ||
              (reqrawurl.Contains("fl.aspx")) ||
               (reqrawurl.Contains("pd.aspx")) ||
              (reqrawurl.Contains("ct.aspx")) ||
              (reqrawurl.Contains("microsite.aspx")) ||
              (reqrawurl.Contains("bb.aspx")))
                {

                   // string querystring = string.Empty;

                    if ((Request.QueryString["path"] != null) && (hfisselected.Value == string.Empty))
                    {
                       // querystring = Request.RawUrl.ToString();

                    }
                    else
                    {
                       // querystring = Request.RawUrl.ToString();
                        string[] qs = querystring.Split('?');

                        querystring = qs[1];
                        querystring = querystring.Replace('_', ' ').Replace("||", "+").Replace("``", "\"").Replace("&srctext=", "").Replace("%E2%80%9C", "\"").Replace("%E2%80%9D", "\"").Replace("\\", "/");

                    }
                    string[] ConsURL = querystring.Split('/');

                    DataTable dt;
                    if (HttpContext.Current.Session["dtcid"] != null)
                    {
                        dt = (DataTable)HttpContext.Current.Session["dtcid"];
                        DataRow[] foundRows;

                        foundRows = dt.Select("cname='" + ConsURL[0].ToLower().Replace("~..~", " / ").Replace("~..", " /").Replace("..~", "/ ").Replace(".~.", "/") + "' ");
                        if (foundRows.Length > 0)
                        {
                            _catCid = foundRows[0][0].ToString();

                        }
                    }
                    else
                    {
                        DataSet tmpds = EasyAsk.GetCategoryAndBrand("MainCategory");
                        dt = tmpds.Tables[0];
                        DataRow[] foundRows;

                        foundRows = dt.Select("category_name = '" + ConsURL[0].ToLower().Replace("~..~", " / ").Replace("~..", " /").Replace("..~", "/ ").Replace(".~.", "/") + "' ");
                        if (foundRows.Length > 0)
                        {
                            _catCid = foundRows[0]["category_id"].ToString();

                        }
                    }


                }
            }

            //if (HidItemPage.Value.ToString() == string.Empty || HidItemPage.Value.ToString() == null)
            //{
            //    if (Session["RECORDS_PER_PAGE"] != null)
            //        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE"].ToString());
            //}
            //else
            //{
            //    iRecordsPerPage = Convert.ToInt32(HidItemPage.Value.ToString());
            //    Session["RECORDS_PER_PAGE"] = HidItemPage.Value.ToString();
            //}
            if (Request.QueryString["pgno"] != null)
            {
                iPageNo = Convert.ToInt32(Request.QueryString["pgno"]);
            }
            if (!IsPostBack)
            {
                // LoadCategoryTree(CID);
                //System.Web.UI.WebControls.TreeNode parentNode;
                //string[] IDS = MCID.Split('>');
                //string valuepath1 = "";
                //for (int idValue = 2; idValue < IDS.Length - 1; idValue++)
                //{
                //    if (valuepath1 == "")
                //        valuepath1 = IDS[idValue].ToString();
                //    else
                //        valuepath1 = valuepath1 + "/" + IDS[idValue].ToString();
                //    parentNode = new TreeNode();
                //    parentNode = TVCategory.FindNode(valuepath1);
                //    if (parentNode != null)
                //    {
                //        parentNode.Expand();
                //        if (idValue == IDS.Length - 2)
                //        {
                //            parentNode.Selected = true;
                //        }
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }
    //private void pageload_new()
    //{
    //    try
    //    {

    //        //TVCategory.ID = "TVCategory";


    //        //TVCategory.class= _TvrSkin;
    //        //TVCategory.NodeStyle.BorderColor = System.Drawing.Color.White;
    //        //TVCategory.NodeStyle.BorderWidth =2;

    //        //TVCategory.CollapseAll();

    //        iCatalogId = Convert.ToInt32(WesCatalogId);

    //        stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());

    //        //if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "" && Request.QueryString["fid"].ToString() != "List all products")
    //        //{
    //        //    if(!Request.QueryString["fid"].ToString().Contains("~"))
    //        //    CID = GetCID(Request.QueryString["fid"].ToString());
    //        //}
    //        // if (Request.QueryString["cid"] != null || CID.ToString() != "")
    //        // {
    //        //     if (Request.QueryString["cid"] != null)
    //        //     {
    //        //         CID = GetMCatID(Request.QueryString["cid"].ToString());
    //        //     }
    //        // }

    //        if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
    //        {
    //            _catCid = Request.QueryString["cid"];
    //        }


    //        else
    //        {
    //            string newquerystring = string.Empty;
    //            string requrl = Request.Url.ToString().ToLower();

    //            if ((requrl.Contains("pl.aspx")) ||
    //          (requrl.Contains("fl.aspx")) ||
    //           (requrl.Contains("pd.aspx")) ||
    //          (requrl.Contains("ct.aspx")) ||
    //          (requrl.Contains("microsite.aspx")) ||
    //          (requrl.Contains("bb.aspx"))
    //            || (requrl.Contains("mct.aspx") == true)
    //                || (requrl.Contains("mpl.aspx"))
    //                || (requrl.Contains("mps.aspx"))
    //                || (requrl.Contains("mfl.aspx"))
    //                || (requrl.Contains("mpd.aspx"))
    //                )
    //            {

    //                string querystring = string.Empty;

    //                if ((Request.QueryString["path"] != null) && (hfisselected.Value == string.Empty))
    //                {
    //                    querystring = Request.RawUrl.ToString().ToLower();

    //                    if (
    //                    querystring.Contains("/mfl/") || querystring.Contains("mfl.aspx")
    //           || querystring.Contains("/mpl/") || querystring.Contains("mpl.aspx")
    //           || querystring.Contains("/mpd/") || querystring.Contains("mpd.aspx")
    //        || querystring.Contains("/mps/") || querystring.Contains("mps.aspx"))
    //                    {

    //                        querystring = objHelperServices.URlStringReverse_MS(Request.RawUrl.ToString().ToLower());
    //                    }
    //                    else 
    //                    {
    //                        querystring = objHelperServices.URlStringReverse(Request.RawUrl.ToString().ToLower());
    //                    }
                        
                        
    //                    newquerystring = querystring;
    //                    querystring = querystring.Replace("/ct/", "ct.aspx?").Replace("/pl/", "pl.aspx?").Replace("/fl/", "fl.aspx?").
    //                        Replace("/bb/", "bb.aspx?").Replace("/pd/", "pd.aspx?").Replace("/ps/", "ps.aspx?").
    //                        Replace("/microsite/", "microsite.aspx?");
    //                    querystring = querystring.Replace("/mct/", "mct.aspx?").Replace("/mpl/", "mpl.aspx?").Replace("/mfl/", "mfl.aspx?").
    //                        Replace("/mbb/", "mbb.aspx?").Replace("/mpd/", "mpd.aspx?").Replace("/mps/", "mps.aspx?");                            
    //                }
    //                //Added by indu for new urlrewrite
    //                //else if (hfisselected.Value != "")
    //                //{

    //                //    querystring = Request.Url.PathAndQuery .ToString().ToLower();

    //                //   // querystring = objHelperServices.URlStringReverse(Request.RawUrl.ToString().ToLower());
    //                //    querystring = querystring.Replace("ct/", "ct.aspx?").Replace("pl/", "pl.aspx?").Replace("fl/", "fl.aspx?").Replace("bb/", "bb.aspx?").Replace("pd/", "pd.aspx?").Replace("ps/", "ps.aspx?");
    //                //}
    //                //
    //                else
    //                {
    //                    querystring = Request.RawUrl.ToString().ToLower();
    //                    if (Request.RawUrl.ToString().ToLower().Contains(".aspx?"))
    //                    {
    //                        string[] newurl = Request.Url.PathAndQuery.ToString().Split(new string[] { ".aspx?" }, StringSplitOptions.None);
    //                        querystring = "/" + newurl[1] + newurl[0];
    //                    }

    //                    if (
    //                 querystring.Contains("/mfl/") || querystring.Contains("mfl.aspx")
    //        || querystring.Contains("/mpl/") || querystring.Contains("mpl.aspx")
    //        || querystring.Contains("/mpd/") || querystring.Contains("mpd.aspx")
    //     || querystring.Contains("/mps/") || querystring.Contains("mps.aspx"))
    //                    {

    //                        querystring = objHelperServices.URlStringReverse_MS(querystring);
    //                    }
    //                    else
    //                    {
    //                        querystring = objHelperServices.URlStringReverse(querystring);  
    //                    }
    //                           newquerystring = querystring;
    //                    querystring = "/" + querystring;
    //                    querystring = querystring.ToLower().Replace("/ct/", "ct.aspx?").Replace("/pl/", "pl.aspx?").Replace("/fl/", "fl.aspx?").Replace("/bb/", "bb.aspx?").Replace("/pd/", "pd.aspx?").Replace("/ps/", "ps.aspx?").Replace("/microsite/", "microsite.aspx?");
    //                    querystring = querystring.ToLower().Replace("/mct/", "mct.aspx?").Replace("/mpl/", "mpl.aspx?").Replace("/mfl/", "mfl.aspx?").Replace("/mbb/", "mbb.aspx?").Replace("/mpd/", "mpd.aspx?").Replace("/mps/", "mps.aspx?");
                      
    //                    string[] qs = querystring.Split('?');
    //                    querystring = qs[1];
    //                    querystring = querystring.Replace("||", "+");
    //                    //.Replace("``", "\"").Replace("&srctext=", "").Replace("%E2%80%9C", "\"").Replace("%E2%80%9D", "\"").Replace("\\","/");

    //                }
    //                string[] ConsURL = querystring.Split('/');
    //                string SB_ConsURL = string.Empty;
    //                //if (ConsURL[0].Contains("_") == true)
    //                //{
    //                //    DataSet ds = objHelperDB.GetOrgValue("", SB_ConsURL);
    //                //    if (ds != null)
    //                //    {
    //                //        SB_ConsURL = ds.Tables[0].Rows[0][0].ToString();
    //                //    }
    //                //}
    //                //else

    //                //{
    //                //    SB_ConsURL = ConsURL[0];
    //                //}
    //                DataTable dt;
    //                if (HttpContext.Current.Session["dtcid"] != null)
    //                {
    //                    dt = (DataTable)HttpContext.Current.Session["dtcid"];
    //                    DataRow[] foundRows;

    //                    foundRows = dt.Select("cname='" + ConsURL[0].ToLower().Replace("~..~", " / ").Replace("~..", " /").Replace("..~", "/ ").Replace(".~.", "/") + "' ");
    //                    if (foundRows.Length > 0)
    //                    {
    //                        _catCid = foundRows[0][0].ToString();

    //                    }
    //                }
    //                if (_catCid == "")
    //                {
    //                    DataSet tmpds = EasyAsk.GetCategoryAndBrand("MainCategory");
    //                    dt = tmpds.Tables[0];
    //                    DataRow[] foundRows;

    //                    foundRows = dt.Select("category_name = '" + ConsURL[0].ToLower().Replace("~..~", " / ").Replace("~..", " /").Replace("..~", "/ ").Replace(".~.", "/") + "' ");
    //                    if (foundRows.Length > 0)
    //                    {
    //                        _catCid = foundRows[0]["category_id"].ToString();

    //                    }
    //                }

    //                //added to get catcid second time
    //                //if (_catCid == "")
    //                //{

    //                //    querystring = objHelperServices.URlStringReverse(newquerystring);
    //                //    querystring = querystring.Replace("/ct/", "ct.aspx?");
    //                //    querystring = querystring.Replace("||", "+");
    //                //    string[] ConsURL1 = querystring.Split('/');
    //                //    DataRow[] foundRows1;

    //                //    foundRows1 = dt.Select("category_name = '" + ConsURL1[0].ToLower().Replace("~..~", " / ").Replace("~..", " /").Replace("..~", "/ ").Replace(".~.", "/") + "' ");
    //                //    if (foundRows1.Length > 0)
    //                //    {
    //                //        _catCid = foundRows1[0]["category_id"].ToString();

    //                //    }
    //                //}
    //            }
    //        }

    //        //if (HidItemPage.Value.ToString() == string.Empty || HidItemPage.Value.ToString() == null)
    //        //{
    //        //    if (Session["RECORDS_PER_PAGE"] != null)
    //        //        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE"].ToString());
    //        //}
    //        //else
    //        //{
    //        //    iRecordsPerPage = Convert.ToInt32(HidItemPage.Value.ToString());
    //        //    Session["RECORDS_PER_PAGE"] = HidItemPage.Value.ToString();
    //        //}
    //        if (Request.QueryString["pgno"] != null)
    //        {
    //            iPageNo = Convert.ToInt32(Request.QueryString["pgno"]);
    //        }
    //        if (!IsPostBack)
    //        {
    //            // LoadCategoryTree(CID);
    //            //System.Web.UI.WebControls.TreeNode parentNode;
    //            //string[] IDS = MCID.Split('>');
    //            //string valuepath1 = "";
    //            //for (int idValue = 2; idValue < IDS.Length - 1; idValue++)
    //            //{
    //            //    if (valuepath1 == "")
    //            //        valuepath1 = IDS[idValue].ToString();
    //            //    else
    //            //        valuepath1 = valuepath1 + "/" + IDS[idValue].ToString();
    //            //    parentNode = new TreeNode();
    //            //    parentNode = TVCategory.FindNode(valuepath1);
    //            //    if (parentNode != null)
    //            //    {
    //            //        parentNode.Expand();
    //            //        if (idValue == IDS.Length - 2)
    //            //        {
    //            //            parentNode.Selected = true;
    //            //        }
    //            //    }
    //            //}
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();

    //    }
    //}


    private void pageload_simple()
    {
        DataSet mainds = new DataSet();
        DataTable dt = new DataTable();
        try
        {



            iCatalogId = Convert.ToInt32(WesCatalogId);

            stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());



            if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
            {
                _catCid = Request.QueryString["cid"];
            }


            else
            {
                string newquerystring = string.Empty;
                string requrl = Request.Url.ToString().ToLower();

                if (requrl.Contains("ct.aspx") || requrl.Contains("mps.aspx") || requrl.Contains("mct.aspx") || requrl.Contains("mpl.aspx") || requrl.Contains("mfl.aspx") || requrl.Contains("mpd.aspx"))
                {


                    //   string querystring = string.Empty;

                    //if ((Request.QueryString["path"] != null) && (hfisselected.Value == string.Empty))
                    //{
                    querystring = querystring.ToLower();
                    string[] ConsURL = querystring.Split('/');

                    if (HttpContext.Current.Session["MainCategory"] != null)
                    {
                        mainds = (DataSet)HttpContext.Current.Session["MainCategory"];
                    }
                    else
                    {
                        string strxml = HttpContext.Current.Server.MapPath("xml");

                        mainds.ReadXml(strxml + "\\" + "mainds.xml");
                        HttpContext.Current.Session["MainCategory"] = mainds;
                    }
                    dt = mainds.Tables[0];
                    DataRow[] foundRows;
                    //if (ConsURL.Length == 4)
                    //{
                    //    foundRows = dt.Select("URL_RW_PATH='" + ConsURL[1] + "' ");
                    //}


                    if (ConsURL.Length == 5 && requrl.Contains("ct.aspx"))
                    {
                        foundRows = dt.Select("URL_RW_PATH='" + ConsURL[2] + "' ");

                    }
                    else
                    {

                        foundRows = dt.Select("URL_RW_PATH='" + ConsURL[1] + "' ");
                    }

                    if (foundRows.Length > 0)
                    {
                        _catCid = foundRows[0]["CATEGORY_ID"].ToString();
                        CategoryName = foundRows[0]["CATEGORY_NAME"].ToString();

                    }
                    else
                    {
                        // objErrorHandler.CreateLog_new("category is empty" + querystring);  
                        if (ConsURL[1] != "")
                        {
                            Response.RedirectPermanent("/" + ConsURL[1] + "/ps/");
                        }
                        else
                        {

                            Response.RedirectPermanent("/404new.html");
                        }
                    }
                    Session["mssuppliername"] = CategoryName;

                    //}
                }



            }

        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
        finally 
        {
    mainds.Dispose() ;
            dt.Dispose();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (Request.RawUrl.ToLower().Contains("ct.aspx?") == true
        //    || Request.RawUrl.ToLower().Contains("fl.aspx?") == true
        //    || Request.RawUrl.ToLower().Contains("pl.aspx?") == true
        //    || Request.RawUrl.ToLower().Contains("pd.aspx?") == true
        //|| Request.RawUrl.ToLower().Contains("powersearch.aspx?") == true
        //    || Request.RawUrl.ToLower().Contains("bb.aspx?") == true)

        string reqrawurl = Request.RawUrl.ToLower();
        if (((reqrawurl.Contains("_")) || reqrawurl.Contains("&") || reqrawurl.Contains("="))
             && !(reqrawurl.Contains("/ct/")) && !(reqrawurl.Contains("/bb/")))
        {

          //  Get_Value_Breadcrum();
        }
        else if (reqrawurl.Contains("/ct/") || reqrawurl.Contains("/pl/") || reqrawurl.Contains("/bb/") || reqrawurl.Contains("/ps/"))
        {

            //Get_Value_Breadcrum_New();
          
   
            
            if(!reqrawurl.Contains("/9") || (reqrawurl.StartsWith("/9")))
            {
                Get_Value_Breadcrum_Simple();
            }
            else
            {
                Redirect_To_New_URL(reqrawurl);
                //Get_Value_Breadcrum_New();
            }
        }
        else if (reqrawurl.Contains("/fl/") || reqrawurl.Contains("/pd/") || (reqrawurl.Contains("/ps/")))
        {
            if (!reqrawurl.Contains("xx"))
            {
                Get_Value_Breadcrum_Simple();
            }
            else
            {
                Redirect_To_New_URL(reqrawurl);

                // Get_Value_Breadcrum_New();
            }
        }

        else if (reqrawurl.Contains("/mct/") || reqrawurl.Contains("/mpl/")
            || reqrawurl.Contains("/mbb/") || reqrawurl.Contains("/mps/"))
        {

            if (!reqrawurl.Contains("/9") || (reqrawurl.StartsWith("/9") ))
            {
                Get_Value_Breadcrum_Simple_MS();
            }
            else
            {
                Redirect_To_New_URL_MS(reqrawurl);
                //Get_Value_Breadcrum_New();
            }

        }
        else if (reqrawurl.Contains("/mfl/") || reqrawurl.Contains("/mpd/") || (reqrawurl.Contains("/mps/")))
        {
            if (!reqrawurl.Contains("xx"))
            {
                Get_Value_Breadcrum_Simple_MS();
            }
            else
            {
                Redirect_To_New_URL_MS(reqrawurl);

                // Get_Value_Breadcrum_New();
            }
        }

    }
    protected string Get_Value_Breadcrum_Simple()
    {
        //stopwatch.Start();
        string sHTML = string.Empty;

        try
        {



            string _tsb = string.Empty;
            string _tsm = string.Empty;
            string _type = string.Empty;
            string _value = string.Empty;
            string _bname = string.Empty;
            string _searchstr = string.Empty;
            string _byp = "2";
            string _bypcat = null;
            string orgeapath = string.Empty;
            bool _isorgurl = false;
            string _pid = string.Empty;
            string _fid = string.Empty;
            string _seeall = string.Empty;
            string _catname = string.Empty;
            _bypcat = Request.QueryString["bypcat"];

            string CatName = string.Empty;


            if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != string.Empty)
                _tsm = Request.QueryString["tsm"];

            if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != string.Empty)
                _tsb = Request.QueryString["tsb"];



            if (_catCid != "")
                _parentCatID = GetParentCatID(_catCid);

            string _ViewType = string.Empty;
            if (Request.Cookies["GLVIEWMODE"] != null)
            {
                if (Request.Cookies["GLVIEWMODE"].Value != null)
                {
                    _ViewType = Request.Cookies["GLVIEWMODE"].Value;
                    Response.Cookies["GLVIEWMODE"].Expires = DateTime.Now.AddDays(-1);
                    Session["GL_VIEWMODE"] = _ViewType;
                    //  Response.Cookies["CLVIEWMODE"].Value = _ViewType;
                    //  Response.Cookies["GLVIEWMODE"].Value = _ViewType;
                    //  Response.Cookies["PLVIEWMODE"].Expires = DateTime.Now.AddDays(1);
                }
                else if (Session["GL_VIEWMODE"] != null)
                {
                    _ViewType = Session["GL_VIEWMODE"].ToString();
                }
                else
                {
                    _ViewType = "GV";
                }
            }
            else if (Session["GL_VIEWMODE"] != null)
            {
                _ViewType = Session["GL_VIEWMODE"].ToString();
            }
            else
            {
                _ViewType = "GV";
            }

            string requrl = Request.Url.ToString().ToLower();

           // string rawurl = Request.RawUrl.ToString().ToLower();
          //  objErrorHandler.CreateLog_new("Get_value_breadcrumb " + querystring);  
            if (_catCid == string.Empty)
            {
                _catCid = "WES0830";
            }
            string[] checkurl = querystring.Split('/');
            if ((requrl.Contains("ct.aspx")) )
            {
              
                if (checkurl.Length == 4)
                {




                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
                    //if (_catCid == string.Empty)
                    //{
                    //    _catCid = "WES0830";
                    //}

   EasyAsk.GetMainMenuClickDetail(_catCid, "");


                    //if (requrl.Contains("mct.aspx"))


                    if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());
                   
                    EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CategoryName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                  
                    hfcname.Value = CatName.Replace("/", "//");

                    if (!(_isorgurl))
                    {
                       
                            Context.RewritePath("/ct.aspx?" + "&id=0&cid=" + _catCid + "&by=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                    }

                    
                }
                else
                {

                    //  string parentCatName = GetCName(_catCid);

                    if (checkurl[1].Contains("-"))
                    {

                        _tsb = GetCorrectBrand(checkurl[1]);
                    }
                    else
                    {
                        _tsb = checkurl[1];

                    }
                    if (_tsb == "")
                    {
                        Response.RedirectPermanent("/404New.htm");
                    }
                    EasyAsk.GetWESModel(CategoryName, iCatalogId, _tsb);
                    if (hforgurl.Value == string.Empty)
                    {
                        hforgurl.Value = CategoryName + "/" + "Brand=" + _tsb;
                    }
                   
                        Context.RewritePath("/ct.aspx?" + "&id=0&cid=" + _catCid + "&tsb=" + _tsb + "&bypcat=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                }


            }
            //Added By:Indu
            //To get EA path with out attribyte Type

            else if ((requrl.Contains("bb.aspx")))
            {


                if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

                bool checksession = false;
                string[] gettype = null;
                if (Session["hfclickedattr_bb"] != null)
                {
                    gettype = Session["hfclickedattr_bb"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                    if (gettype.Length > 2)
                    {
                        checksession = true;

                    }
                }


                if (Session["hfclickedattr_bb"] == null || checksession==false)
                {
                    if (checkurl[2].Contains("-"))
                    {

                        _tsb = GetCorrectBrand(checkurl[2]);
                    }
                    else
                    {
                        _tsb = checkurl[2];

                    }
                    if (checkurl[1].Contains("-"))
                    {

                        _tsm = GetCorrectModel(checkurl[2] + "-" + checkurl[1]);
                        _tsm = _tsm.ToLower().Replace(_tsb.ToLower() + ":", "");
                    }
                    else
                    {
                        _tsm = checkurl[1];

                    }
                    if (_tsm == "")
                    {
                        Response.RedirectPermanent("/404New.htm");
                    }

                    if (checkurl.Length == 6)
                    {
                        Session["EA"] = EA_ROOT_CATEGORY_PATH + "////" + "CELLULAR ACCESSORIES" + "////" + "AttribSelect=Brand='" + _tsb + "'";
                        EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                    }
                    else
                    {


                        string[] getEA = querystring.Split(new string[] { "/wa-" }, StringSplitOptions.None);
                        if (getEA.Length > 1)
                        {
                            string hffield = getEA[1];
                            string[] hffield1 = hffield.Split('/');

                            string EA = string.Empty;
                            string hffieldfinal = _tsm + "wa-" + hffield1[0];
                            string _value1 = string.Empty;
                            if (Session[hffieldfinal] != null)
                            {
                                string hfvalue = Session[hffieldfinal].ToString();
                                string[] setEA = hfvalue.Split(new string[] { "////" }, StringSplitOptions.None);
                                string attr = setEA[setEA.Length - 1];

                               // string[] gettype = null;

                                gettype = attr.Split('=');
                                if (gettype.Length > 1)
                                {
                                    _type = gettype[1];
                                    _value = gettype[2];
                                    _value = _value.Substring(2, _value.Length - 3);
                                }
                                else
                                {
                                    _type = "category";
                                    _value = gettype[0];
                                }

                                Session["EA"] = hfvalue.Replace("////" + attr, "");
                                Session[hffieldfinal] = null;
                                EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                            }
                            else
                            {
                                Session["EA"] = EA_ROOT_CATEGORY_PATH + "////" + "CELLULAR ACCESSORIES" + "////" + "AttribSelect=Brand='" + _tsb + "'";
                                EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                            }
                        }
                        else
                        {

                            Session["EA"] = EA_ROOT_CATEGORY_PATH + "////" + "CELLULAR ACCESSORIES" + "////" + "AttribSelect=Brand='" + _tsb + "'";
                                EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                           
                        
                        }
                    }
                    // EasyAsk.GetBrandAndModelProducts(CategoryName, _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                 //   Context.RewritePath("/bb.aspx?&id=0&cid=" + _catCid + "&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                    if (!(_isorgurl))
                    {
                        if (Session["EA"] == null)
                        {
                            // objErrorHandler.CreateLog("EA is null");
                            Session["EA"] = "AllProducts////WESAUSTRALASIA";

                        }
                            Context.RewritePath("/bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                    }
                }

                else
                {

                   // string[] gettype = null;
                    if (Session["hfclickedattr_bb"] != null)
                    {
                        gettype = Session["hfclickedattr_bb"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                    }

                    if (gettype[0] != "")
                    {
                        Session["EA"] = gettype[0];
                    }

                    _type = gettype[1];
                    _value = gettype[2];
                    if (_value.Contains("::"))
                    {
                        gettype = _value.Split(new string[] { "::" }, StringSplitOptions.None);
                        _bname = gettype[0];
                        _value = gettype[1];
                    }
                    Session["hfclickedattr_bb"] = null;

                    EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                    if (!(_isorgurl))
                    {
                        if (Session["EA"] == null)
                        {
                            // objErrorHandler.CreateLog("EA is null");
                            Session["EA"] = "AllProducts////WESAUSTRALASIA";

                        }
                            Context.RewritePath("/bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&type=" + _type + "&value=" + HttpUtility.UrlEncode(_value) + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                    }
                }


            }
            else if (requrl.Contains("pl.aspx"))
            {
               
                if (Session["RECORDS_PER_PAGE_pl"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_pl"].ToString());
                bool checksession = false;
                   string[] gettype = null;
                   if (Session["hfclickedattr_pl"] != null)
                   {
                       gettype = Session["hfclickedattr_pl"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                       if (gettype.Length > 2)
                       {
                           checksession = true;

                       }
                   }
                if (Session["hfclickedattr_pl"] == null ||  checksession==false)
                {
                    _value = GetSubCategoryEA();
                     string[] getEA = querystring.Split(new string[] { "/wa-" }, StringSplitOptions.None);
                    if ((checkurl.Length == 5) || (getEA.Length==1) )
                    {
                        EasyAsk.GetAttributeProducts("ProductList", "", "Category", _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                    }
                    else
                    {
                       
                        string hffield = getEA[1];
                        string[] hffield1 = hffield.Split('/');

                        string EA = string.Empty;
                        string hffieldfinal = _value + "wa-" + hffield1[0];
                        string _value1 = string.Empty;
                        if (Session[hffieldfinal] != null)
                        {
                            string hfvalue = Session[hffieldfinal].ToString();
                            string[] setEA = hfvalue.Split(new string[] { "////" }, StringSplitOptions.None);
                            string attr = setEA[setEA.Length - 1];

                            //string[] gettype = null;

                            gettype = attr.Split('=');
                            if (gettype.Length > 1)
                            {
                                _type = gettype[1];
                                _value = gettype[2];
                                _value = _value.Substring(2, _value.Length - 3);
                            }
                            else
                            {
                                _type = "category";
                                _value = gettype[0];
                            }

                            Session["EA"] = hfvalue.Replace("////" + attr, "");
                            Session[hffieldfinal] = null;
                            EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                        }
                        else
                        {
                            EasyAsk.GetAttributeProducts("ProductList", "", "Category", _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                        }

                    }





                }
                else
                {

                 
                    if (Session["hfclickedattr_pl"] != null)
                    {
                        gettype = Session["hfclickedattr_pl"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                    }

                    if (gettype[0] != "")
                    {
                        Session["EA"] = gettype[0];
                       // Session["EA_F_P"] = gettype[0];
                    }

                    _type = gettype[1];
                    _value = gettype[2];
                    if (_value.Contains("::"))
                    {
                        gettype = _value.Split(new string[] { "::" }, StringSplitOptions.None);
                        _bname = gettype[0];
                        _value = gettype[1];
                    }
                    Session["hfclickedattr_pl"] = null;
                    
                    EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                }
                if (!(_isorgurl))
                {
                if(Session["EA"]==null)
                {
                   // objErrorHandler.CreateLog("EA is null");
                    Session["EA"] = "AllProducts////WESAUSTRALASIA";
                  
                }
                    
                        Context.RewritePath("/pl.aspx?&cid=" + _catCid + "&type='" + _type + "'&value=" + _value + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                   
                   }
            }
            else if ((requrl.Contains("ps.aspx")))
            {
                if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());
                 string[] gettype = null;
                string[] strps = requrl.Split('?');
                 

                   bool checksession = false;
                   if (Session["hfclickedattr_ps"] != null)
                   {
                       gettype = Session["hfclickedattr_ps"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                if(   gettype.Length > 2)
                {
                checksession=true;
                
                }
                   }

                   if (Session["hfclickedattr_ps"] == null || checksession==false)
                {
                    if (strps[1].Contains("-"))
                    {
                        if (HttpContext.Current.Session["CurrSearch"] != null)
                        {
                            _searchstr = HttpContext.Current.Session["CurrSearch"].ToString().ToLower();

                            string[] checkifsameurl = strps[1].Split('-');
                            if (_searchstr.Contains(checkifsameurl[0].ToLower()) == false)
                            {

                                _searchstr = strps[1].Replace("-", " "); 
                            }

                        }
                        else
                        { 

                            _searchstr = strps[1].Replace("-"," ") ; 
                        }
                    }
                    else
                    {
                        _searchstr = strps[1].ToString(); 
                    }
               
                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + "UserSearch=" + _searchstr.Replace("/", "////").Replace("&srctext=", "");

                    if ((_searchstr == "~fl~") || (_searchstr == "~pl~") || (_searchstr == "~ps~") || (_searchstr == "~ct~") || (_searchstr == "~pd~") || (_searchstr == "~bb~") || (_searchstr == "~bp~") || (_searchstr == "~bk~"))
                    {
                        _searchstr = _searchstr.Replace("~", "");
                    }
                    if ((_value == "~fl~") || (_value == "~pl~") || (_value == "~ps~") || (_value == "~ct~") || (_value == "~pd~") || (_value == "~bb~") || (_value == "~bp~") || (_value == "~bk~"))
                    {
                        _value = _value.Replace("~", "");
                    }
                    string[] getEA = querystring.Split(new string[] { "/wa-" }, StringSplitOptions.None);
                    if( (checkurl.Length == 4) || (getEA.Length==1)) 
                    {
                        Session["EA"] = EA_ROOT_CATEGORY_PATH + "////";
                        EasyAsk.GetAttributeProducts("ps", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                    }

                    else
                    {

                       
                        string hffield = getEA[1];
                        string[] hffield1 = hffield.Split('/');

                        string[] searchstr = _searchstr.Split('/');
                        _searchstr = searchstr[0].Replace("-", " ");
                        string EA = string.Empty;
                        string hffieldfinal = _searchstr + "wa-" + hffield1[0];
                        string _value1 = string.Empty;
                        if (Session[hffieldfinal] != null)
                        {
                            string hfvalue = Session[hffieldfinal].ToString();
                            string[] setEA = hfvalue.Split(new string[] { "////" }, StringSplitOptions.None);
                            string attr = setEA[setEA.Length - 1];

                      

                            gettype = attr.Split('=');
                            if (gettype.Length > 1)
                            {
                                _type = gettype[1];
                                _value = gettype[2];
                                _value = _value.Substring(2, _value.Length - 3);
                            }
                            else
                            {
                                _type = "category";
                                _value = gettype[0];
                            }
                          
                            Session["EA"] = hfvalue.Replace("////" + attr, "");
                            Session[hffieldfinal] = null;

                        }
                        EasyAsk.GetAttributeProducts("ps", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");




                    }
                }
                else
                {

                   
                 
                        if (gettype[0] != "")
                        {
                            Session["EA"] = gettype[0];
                        }

                        _type = gettype[1];
                        _value = gettype[2];
                        if (_value.Contains("::"))
                        {
                            gettype = _value.Split(new string[] { "::" }, StringSplitOptions.None);
                            _bname = gettype[0];
                            _value = gettype[1];
                        }
                        if (_type.Contains("USERSEARCH"))
                        {
                            _searchstr = _value;
                            _type = "";
                            _value = "";
                        }
                        Session["hfclickedattr_ps"] = null;
                        EasyAsk.GetAttributeProducts("ps", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                   
                }


                if (!(_isorgurl))
                {
                    if (_value != "")
                    {
                       
                            Context.RewritePath("/ps.aspx?&id=0&searchstr=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                    }
                    else
                    {
                            Context.RewritePath("/ps.aspx?&id=0&searchstr=" + _searchstr + "&srctext=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                    }
                }

            }




            else if ((requrl.Contains("fl.aspx")) || (requrl.Contains("mfl.aspx")))
            {
             
                // string[] Consurl = rawurl.Split('/');


                //string family = checkurl[1];
                //string[] fid = family.Split('-');
                //_fid = fid[fid.Length - 1];


                if (requrl.Contains("/wa-"))
                {
                    _fid = checkurl[4];
                }
                else
                {

                    _fid = checkurl[4];
                }


                bool checksession = false;
                string[] gettype = null;
                if (Session["hfclickedattr"] != null)
                {
                    gettype = Session["hfclickedattr"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                    if (gettype.Length > 2)
                    {
                        checksession = true;

                    }
                }

                if (Session["hfclickedattr"] == null || checksession == false) 
                {
                    //if (Request.Form["hfclickedattr"] == null)
                    //{
                    if (_fid == "")
                    {
                       
                        Response.RedirectPermanent("/404New.htm");
                    }
                    else if(IsNumber(_fid) == false)
                        {
                            Response.RedirectPermanent("/404New.htm");
                        }
                    Get2SubCatEA(_fid);
                    string[] getEA = querystring.Split(new string[] { "/wa-" }, StringSplitOptions.None);
                    if ((checkurl.Length == 7) || (getEA.Length==1))
                    {
                        EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");
                    }
                    else
                    {


                      
                        string hffield = getEA[1];
                        string[] hffield1 = hffield.Split('/');

                        string EA = string.Empty;
                        string hffieldfinal = _fid + "wa-" + hffield1[0];
                        string _value1 = string.Empty;
                        if (Session[hffieldfinal] != null)
                        {
                            string hfvalue = Session[hffieldfinal].ToString();
                            string[] setEA = hfvalue.Split(new string[] { "////" }, StringSplitOptions.None);
                            string attr = setEA[setEA.Length - 1];

                            

                            gettype = attr.Split('=');
                            _type = gettype[1];
                            _value = gettype[2];
                            _value1 = _value.Substring(2, _value.Length - 3);

                            Session["EA"] = hfvalue.Replace("////" + attr, "");
                            Session[hffieldfinal] = null;
                            EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value1, _bname, "0", "0", "");

                        }
                        else
                        {

                            EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");

                        }



                    }



                }
                else
                {

                 
                    if (Session["hfclickedattr"] != null)
                    {
                        gettype = Session["hfclickedattr"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                    }

                    if (gettype[0] != "")
                    {
                        Session["EA"] = gettype[0];
                    }

                    _type = gettype[1];
                    _value = gettype[2];
                    if (_value.Contains("::"))
                    {
                        gettype = _value.Split(new string[] { "::" }, StringSplitOptions.None);
                        _bname = gettype[0].ToLower();
                        _value = gettype[1].ToLower();
                    }
                    Session["hfclickedattr"] = null;
                    EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value, _bname, "0", "0", "");

                    //    Request.Form["hfclickedattr"] = Session["hfclickedattr"].ToString();
                    //  Session["hfclickedattr"] = null;

                }









                if (!(_isorgurl))
                {
                    //objErrorHandler.CreateLog("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + Session["EA"].ToString());
                    // objErrorHandler.CreateLog(requrl);
                    if (Session["EA"] == null)
                    {
                        //objErrorHandler.CreateLog_new("EA is null Family");
                        Session["EA"] = "AllProducts////WESAUSTRALASIA";

                    }
                        Context.RewritePath("/fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                }



              
        

            }
            //else if ((requrl.Contains("pd.aspx")) || (requrl.Contains("mpd.aspx")))
            // {

            //         //string[] getvalue = rawurl.Split('/');
            //     string family = checkurl[2];
            //         string[] fid = family.Split('-');
            //         _fid = fid[fid.Length - 1];
            //         Get2SubCatEA(_fid);



            //         Session["EA"] = Session["EA"] + "////" + "UserSearch=Family Id=" + _fid;
            //         string product = checkurl[1];
            //         string[] pid = product.Split('-');
            //         _pid = pid[pid.Length - 1];
            //         EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");

            //         if (!(_isorgurl))
            //         {
            //             if (requrl.Contains("mpd.aspx"))
            //                 Context.RewritePath("/mpd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
            //             else
            //                 Context.RewritePath("/pd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

            //         }

            // }
            else if ((requrl.Contains("pd.aspx")) || (requrl.Contains("mpd.aspx")))
            {


                //HttpContext ctx = HttpContext.Current;
                //Thread t1 = new Thread(delegate()
                //    {
                //        ST_NewProductLogNav_Thread(ctx); 
                //    });
                //t1.Start();

                //string[] getvalue = rawurl.Split('/');
                string family = checkurl[checkurl.Length - 3];
                string[] fid = family.Split('-');
                _fid = family;
                Get2SubCatEA(_fid);

                if (_fid == ""  )
                {

                    Response.RedirectPermanent("/404New.htm");
                }
                else if (IsNumber(_fid) == false)
                {
                    Response.RedirectPermanent("/404New.htm");
                }
                //Session["EA"] = Session["EA"] + "////" + "UserSearch=Family Id=" + _fid;
                string product = checkurl[checkurl.Length - 4];
                string[] pid = product.Split('-');
             
                _pid = pid[pid.Length - 1];
                if (_pid == "")
                {

                    Response.RedirectPermanent("/404New.htm");
                }
                else if (IsNumber(_pid) == false)
                {
                    Response.RedirectPermanent("/404New.htm");
                }
              //  stopwatch.Start();
                EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");
               // stopwatch.Stop();
              //  objErrorHandler.CreateLog("GetAttributeProducts -" + stopwatch.Elapsed);
                if (!(_isorgurl))
                {
                    if (Session["EA"] == null)
                    {
                       // objErrorHandler.CreateLog_new("EA is null proddet");
                        Session["EA"] = "AllProducts////WESAUSTRALASIA";

                    }
                        Context.RewritePath("/pd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                }

            }

        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            sHTML = ex.Message;
        }
       // stopwatch.Stop();
       // objErrorHandler.CreateLog("get_value_breadcrumb_simple -" + stopwatch.Elapsed);

        return sHTML;
    }

    protected string Get_Value_Breadcrum_Simple_MS()
    {

        string sHTML = string.Empty;

        try
        {



            string _tsb = string.Empty;
            string _tsm = string.Empty;
            string _type = string.Empty;
            string _value = string.Empty;
            string _bname = string.Empty;
            string _searchstr = string.Empty;
            string _byp = "2";
            string _bypcat = null;
            string orgeapath = string.Empty;
            bool _isorgurl = false;
            string _pid = string.Empty;
            string _fid = string.Empty;
            string _seeall = string.Empty;
            string _catname = string.Empty;
            _bypcat = Request.QueryString["bypcat"];

            string CatName = string.Empty;


            if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != string.Empty)
                _tsm = Request.QueryString["tsm"];

            if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != string.Empty)
                _tsb = Request.QueryString["tsb"];



            if (_catCid != "")
                _parentCatID = GetParentCatID(_catCid);

            string _ViewType = string.Empty;
            if (Request.Cookies["GLVIEWMODE"] != null)
            {
                if (Request.Cookies["GLVIEWMODE"].Value != null)
                {
                    _ViewType = Request.Cookies["GLVIEWMODE"].Value;
                    Response.Cookies["GLVIEWMODE"].Expires = DateTime.Now.AddDays(-1);
                    Session["GL_VIEWMODE"] = _ViewType;
                    //  Response.Cookies["CLVIEWMODE"].Value = _ViewType;
                    //  Response.Cookies["GLVIEWMODE"].Value = _ViewType;
                    //  Response.Cookies["PLVIEWMODE"].Expires = DateTime.Now.AddDays(1);
                }
                else if (Session["GL_VIEWMODE"] != null)
                {
                    _ViewType = Session["GL_VIEWMODE"].ToString();
                }
                else
                {
                    _ViewType = "GV";
                }
            }
            else if (Session["GL_VIEWMODE"] != null)
            {
                _ViewType = Session["GL_VIEWMODE"].ToString();
            }
            else
            {
                _ViewType = "GV";
            }

            string requrl = Request.Url.ToString().ToLower();

           string rawurl = querystring;
            if (rawurl.Contains("undefined"))
            {

              rawurl=  rawurl.ToString().ToLower().Replace("undefined", "");
            }
            if (_catCid == string.Empty)
            {
                _catCid = "WES0830";
            }
            string[] checkurl = rawurl.Split('/');
            if ((requrl.Contains("microsite.aspx")) || (requrl.Contains("mct.aspx")))
            {

                if (checkurl.Length == 4)
                {




                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
                    if (_catCid == string.Empty)
                    {
                        _catCid = "WES0830";
                    }

                    EasyAsk.GetMainMenuClickDetail(_catCid, "");


                    if (requrl.Contains("mct.aspx"))


                        if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                            iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());

                    EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CategoryName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                    hfcname.Value = CatName.Replace("/", "//");

                    if (!(_isorgurl))
                    {

                        Context.RewritePath("/mct.aspx?" + "&id=0&cid=" + _catCid + "&by=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));


                    }



                }
                else
                {

                    //  string parentCatName = GetCName(_catCid);

                    if (checkurl[1].Contains("-"))
                    {

                        _tsb = GetCorrectBrand(checkurl[1]);
                    }
                    else
                    {
                        _tsb = checkurl[1];

                    }
                    if (_tsb == "")
                    {

                        Response.RedirectPermanent("/404New.htm");  
                    }
                    EasyAsk.GetWESModel(CategoryName, iCatalogId, _tsb);
                    if (hforgurl.Value == string.Empty)
                    {
                        hforgurl.Value = CategoryName + "/" + "Brand=" + _tsb;
                    }
                   
                        Context.RewritePath("/mct.aspx?" + "&id=0&cid=" + _catCid + "&tsb=" + _tsb + "&bypcat=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                  

                }

            }
            //Added By:Indu
            //To get EA path with out attribyte Type

            else if ((requrl.Contains("bb.aspx")) || (requrl.Contains("mbb.aspx")))
            {
                //To be used later

                //callms_bybrand()
              
            }
            else if ((requrl.Contains("mpl.aspx")))
            {
                if (Session["RECORDS_PER_PAGE_pl"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_pl"].ToString());

                bool checksession = false;
                string[] gettype = null;
                if (Session["hfclickedattr_top"] != null)
                {
                    gettype = Session["hfclickedattr_top"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                    if (gettype.Length > 1)
                    {
                        checksession = true;

                    }
                }

                if (Session["hfclickedattr_top"] != null && checksession==true)
                {
                    //string[] gettype = null;
                    gettype = Session["hfclickedattr_top"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);

                    if (gettype.Length > 0)
                    {
                        if (gettype[0] != "")
                        {
                         string tempcatname=   GetSubCategoryEA_MS();
                         Session["EA"] = Session["EA"] + "////" + tempcatname; 

                        }

                        _type = gettype[1];

                        _value = gettype[2];

                        if (_type.ToLower() == "model")
                        {
                            if (_value.Contains(":"))
                            {
                                string[] val = _value.Split(':');
                                if (val.Length == 3)
                                {
                                    _bname = val[1];
                                    _value = val[2];
                                }
                                else if (val.Length == 2)
                                {
                                    _bname = val[0];
                                    _value = val[1];
                                }
                            }

                        }
                    }
                    Session["hfclickedattr_top"] = null;
                 

                    EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                }
                 checksession = false;

                 if (Session["hfclickedattr_mpl"] != null)
                {
                    gettype = Session["hfclickedattr_mpl"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                    if (gettype.Length > 1)
                    {
                        checksession = true;

                    }
                }

                 if (Session["hfclickedattr_mpl"] == null || Session["hfclickedattr_mpl"] == "" || checksession==false)


                {

                    _value = GetSubCategoryEA_MS();

                    string[] getEA = rawurl.Split(new string[] { "/mwa-" }, StringSplitOptions.None);
                    if ( (checkurl.Length == 5) || (getEA.Length==1))
                    {
                        if (_value == "" || _value == string.Empty)
                        {
                            Response.RedirectPermanent("/404New.htm");
                        }
                        EasyAsk.GetAttributeProducts("ProductList", "", "Category", _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                    }
                    else
                    {
                      
                        string hffield = getEA[1];
                        string[] hffield1 = hffield.Split('/');

                        string EA = string.Empty;
                        string hffieldfinal = _value + "mwa-" + hffield1[0];
                        string _value1 = string.Empty;
                        if (Session[hffieldfinal] != null)
                        {
                            string hfvalue = Session[hffieldfinal].ToString();
                            string[] setEA = hfvalue.Split(new string[] { "////" }, StringSplitOptions.None);
                            string attr = setEA[setEA.Length - 1];

                            //string[] gettype = null;

                            gettype = attr.Split('=');
                            if (gettype.Length > 1)
                            {
                                _type = gettype[1];
                                _value = gettype[2];
                                _value = _value.Substring(2, _value.Length - 3);
                            }
                            else
                            {
                                _type = "category";
                                _value = gettype[0];
                            }
                            Session["EA"] = hfvalue.Replace("////" + attr, "");
                            Session[hffieldfinal] = null;
                            if (_type.ToLower() == "model")
                            {
                                if (_value.Contains(":"))
                                {
                                    string[] val = _value.Split(':');
                                    if (val.Length == 3)
                                    {
                                        _bname = val[1];
                                        _value = val[2];
                                    }
                                    else if (val.Length == 2)
                                    {
                                        _bname = val[0];
                                        _value = val[1];
                                    }
                                }

                            }
                            EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                        }
                        else
                        {
                            if (_value == "" || _value == string.Empty)
                            {
                                Response.RedirectPermanent("/404New.htm");
                            }
                            EasyAsk.GetAttributeProducts("ProductList", "", "Category", _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                        }

                    }





                }
                else
                {
                   // objErrorHandler.CreateLog(Session["hfclickedattr_mpl"].ToString());  
                   // string[] gettype = null;
                    if (Session["hfclickedattr_mpl"] != null)
                    {
                        gettype = Session["hfclickedattr_mpl"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);

                        if (gettype.Length > 0)
                        {
                            if (gettype[0] != "")
                            {
                                Session["EA"] = gettype[0];
                            }

                            _type = gettype[1];
                            _value = gettype[2];

                             string[] EAsplit = gettype[0].Split(new string[] { "////" }, StringSplitOptions.None);
                             if (EAsplit.Length > 4)
                             {

                                 if (gettype[0].Contains(_type))
                                 {

                                     string[] EA = gettype[0].Split(new string[] { _type }, StringSplitOptions.None);
                                     Session["EA"] = EA[0].Replace("AttribSelect=","");
                                 }
                                 else if (_type.ToUpper() == "CATEGORY")
                                 {

                                     Session["EA"] = EAsplit[0] + "////" + EAsplit[1] + "////" + EAsplit[2] + "////" + EAsplit[3] + "////";
                                 }
                             }
                            if (_value.Contains("::"))
                            {
                                gettype = _value.Split(new string[] { "::" }, StringSplitOptions.None);
                                _bname = gettype[0];
                                _value = gettype[1];
                            }

                            if (_type.ToLower() == "model")
                            {
                                if (_value.Contains(":"))
                                {
                                    string[] val = _value.Split(':');
                                    if (val.Length == 3)
                                    {
                                        _bname = val[1];
                                        _value = val[2];
                                    }
                                    else if (val.Length == 2)
                                    {
                                        _bname = val[0];
                                        _value = val[1];
                                    }
                                }

                            }
                            Session["hfclickedattr_mpl"] = null;
                        }
                        else
                        {
                            _value = GetSubCategoryEA_MS();
                        }
                  
                    }

                    EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                }
                if (!(_isorgurl))
                {
                   
                        Context.RewritePath("/mpl.aspx?cid=" + _catCid + "&type='" + _type + "'&value=" + _value + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                   
                }
            }
            else if ((requrl.Contains("mps.aspx")))
            {
                if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());
                string[] strps = requrl.Split('?');
                bool checksession = false;
                string[] gettype = null;
                if (Session["hfclickedattr_mps"] != null)
                {
                    gettype = Session["hfclickedattr_mps"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                    if (gettype.Length > 2)
                    {
                        checksession = true;

                    }
                }

                if (Session["hfclickedattr_mps"] == null || checksession==false)
                {

                    string[] strps1 = strps[1].ToString().Split('/');


                    if (strps[1].Contains("-"))
                    {
                        if (HttpContext.Current.Session["CurrSearch_MS"] != null)
                        {
                            _searchstr = HttpContext.Current.Session["CurrSearch_MS"].ToString().ToLower();

                            string[] checkifsameurl = strps1[1].Split('-');
                            if (_searchstr.Contains(checkifsameurl[0].ToLower()) == false)
                            {

                                _searchstr = strps1[1].Replace("-", " ");
                            }

                        }
                        else
                        {

                            _searchstr = strps1[1].Replace("-", " ");
                        }
                    }
                    else
                    {
                        _searchstr = strps1[1].ToString();
                    }
                  //  HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" +CategoryName+"////"+ "UserSearch=" + _searchstr.Replace("/", "////").Replace("&srctext=", "");

                    if ((_searchstr == "~fl~") || (_searchstr == "~pl~") || (_searchstr == "~ps~") || (_searchstr == "~ct~") || (_searchstr == "~pd~") || (_searchstr == "~bb~") || (_searchstr == "~bp~") || (_searchstr == "~bk~"))
                    {
                        _searchstr = _searchstr.Replace("~", "");
                    }
                    if ((_value == "~fl~") || (_value == "~pl~") || (_value == "~ps~") || (_value == "~ct~") || (_value == "~pd~") || (_value == "~bb~") || (_value == "~bp~") || (_value == "~bk~"))
                    {
                        _value = _value.Replace("~", "");
                    }
                    string[] getEA = rawurl.Split(new string[] { "/mwa-" }, StringSplitOptions.None);
                    if ( (checkurl.Length == 5) || (getEA.Length==1)) 
                    {

                        //DataSet mainds = new DataSet();
                        //DataTable dt;
                        //if (HttpContext.Current.Session["MainCategory"] != null)
                        //{
                        //    mainds = (DataSet)HttpContext.Current.Session["MainCategory"];
                        //    dt = mainds.Tables[0];
                        //    DataRow[] foundRows;

                        //    foundRows = dt.Select("URL_RW_PATH='" + strps1[0] + "' ");

                        //    if (foundRows.Length > 0)
                        //    {
                        //        _catCid = foundRows[0]["CATEGORY_ID"].ToString();
                        //        CategoryName = foundRows[0]["CATEGORY_NAME"].ToString();

                        //    }
                        //}


                       

                        Session["EA"] = EA_ROOT_CATEGORY_PATH + "////" + CategoryName;
                        EasyAsk.GetAttributeProducts("ps", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                    }

                    else
                    {

                    
                        string hffield = getEA[1];
                        string[] hffield1 = hffield.Split('/');

                        string[] searchstr = _searchstr.Split('/');
                        _searchstr = searchstr[0].Replace("-", " ");
                        string EA = string.Empty;
                        string hffieldfinal = _searchstr + "mwa-" + hffield1[0];
                        string _value1 = string.Empty;
                        if (Session[hffieldfinal] != null)
                        {
                            string hfvalue = Session[hffieldfinal].ToString();
                            string[] setEA = hfvalue.Split(new string[] { "////" }, StringSplitOptions.None);
                            string attr = setEA[setEA.Length - 1];

                            //string[] gettype = null;

                            gettype = attr.Split('=');
                            if (gettype.Length > 1)
                            {
                                _type = gettype[1];
                                _value = gettype[2];
                                _value = _value.Substring(2, _value.Length - 3);
                            }
                            else
                            {
                                _type = "category";
                                _value = gettype[0];
                            }

                            Session["EA"] = hfvalue.Replace("////" + attr, "");
                            Session[hffieldfinal] = null;

                        }
                        else
                        {
                            Session["EA"] = EA_ROOT_CATEGORY_PATH + "////" + CategoryName;
                        }
                        EasyAsk.GetAttributeProducts("ps", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");




                    }
                }
                else
                {

                  //  string[] gettype = null;
                    if (Session["hfclickedattr_mps"] != null)
                    {
                        gettype = Session["hfclickedattr_mps"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                    }

                    if (gettype[0] != "")
                    {
                        Session["EA"] = gettype[0];
                    }

                    _type = gettype[1];
                    _value = gettype[2];
                    string[] EAsplit = gettype[0].Split(new string[] { "////" }, StringSplitOptions.None);
                    if (EAsplit.Length > 4)
                    {

                        if (gettype[0].Contains(_type))
                        {

                            string[] EA = gettype[0].Split(new string[] { _type }, StringSplitOptions.None);
                            Session["EA"] = EA[0].Replace("AttribSelect=", "");
                        }
                        else if (_type.ToUpper() == "CATEGORY")
                        {

                            Session["EA"] = EAsplit[0] + "////" + EAsplit[1] + "////" + EAsplit[2] + "////" + EAsplit[3] + "////";
                        }
                    }
                    if (_value.Contains("::"))
                    {
                        gettype = _value.Split(new string[] { "::" }, StringSplitOptions.None);
                        _bname = gettype[0];
                        _value = gettype[1];
                    }
                    Session["hfclickedattr_mps"] = null;
                    EasyAsk.GetAttributeProducts("ps", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                }


                if (!(_isorgurl))
                {
                    if (_value != "")
                    {
                        if (requrl.Contains("mps.aspx"))
                            Context.RewritePath("/mps.aspx?&id=0&searchstr=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                       

                    }
                    else
                    {
                       
                            Context.RewritePath("/mps.aspx?&id=0&searchstr=" + _searchstr + "&srctext=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                       
                    }
                }

            }




            else if ((requrl.Contains("mfl.aspx")))
            {

                // string[] Consurl = rawurl.Split('/');


                //string family = checkurl[1];
                //string[] fid = family.Split('-');
                //_fid = fid[fid.Length - 1];


                if (requrl.Contains("/mwa-"))
                {
                    _fid = checkurl[4];
                }
                else
                {

                    _fid = checkurl[4];
                }


                bool checksession = false;
                string[] gettype = null;
                if (Session["hfclickedattr_mfl"] != null)
                {
                    gettype = Session["hfclickedattr_mfl"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
                    if (gettype.Length > 2)
                    {
                        checksession = true;

                    }
                }

                if (Session["hfclickedattr_mfl"] == null || checksession==false)
                {
                    //if (Request.Form["hfclickedattr"] == null)
                    //{
                    if (_fid == "")
                    {

                        Response.RedirectPermanent("/404New.htm");
                    }
                    else if (IsNumber(_fid) == false)
                    {
                        Response.RedirectPermanent("/404New.htm");
                    }
                    Get2SubCatEA_MS(_fid);
                    string[] getEA = rawurl.Split(new string[] { "/mwa-" }, StringSplitOptions.None);
                    if ((checkurl.Length == 7)||(getEA.Length==1) )
                    {
                        EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");
                    }
                    else
                    {


                       
                        string hffield = getEA[1];
                        string[] hffield1 = hffield.Split('/');

                        string EA = string.Empty;
                        string hffieldfinal = _fid + "mwa-" + hffield1[0];
                        string _value1 = string.Empty;
                        if (Session[hffieldfinal] != null)
                        {
                            string hfvalue = Session[hffieldfinal].ToString();
                            string[] setEA = hfvalue.Split(new string[] { "////" }, StringSplitOptions.None);
                            string attr = setEA[setEA.Length - 1];

                           // string[] gettype = null;

                            gettype = attr.Split('=');
                            _type = gettype[1];
                            _value = gettype[2];
                            _value1 = _value.Substring(2, _value.Length - 3);

                            Session["EA"] = hfvalue.Replace("////" + attr, "");
                            Session[hffieldfinal] = null;
                            EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value1, _bname, "0", "0", "");

                        }
                        else
                        {

                            EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");

                        }



                    }



                }
                else
                {

                  //  string[] gettype = null;
                    if (Session["hfclickedattr_mfl"] != null)
                    {
                        gettype = Session["hfclickedattr_mfl"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);

                        if (gettype.Length  > 0)
                        {
                            if (gettype[0] != "")
                            {
                                Session["EA"] = gettype[0];
                            }

                            _type = gettype[1];
                            _value = gettype[2];
                            if (_value.Contains("::"))
                            {
                                gettype = _value.Split(new string[] { "::" }, StringSplitOptions.None);
                                _bname = gettype[0].ToLower();
                                _value = gettype[1].ToLower();
                            }
                            Session["hfclickedattr_mfl"] = null;
                            EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value, _bname, "0", "0", "");
                           
                        }
                        else
                        {
                            if (_fid == "" || IsNumber(_fid) == false)
                            {

                                Response.RedirectPermanent("/404New.htm");
                            }
                            Get2SubCatEA_MS(_fid);

                            EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");
                        }
                    }
                  
                    //    Request.Form["hfclickedattr"] = Session["hfclickedattr"].ToString();
                    //  Session["hfclickedattr"] = null;

                }









                if (!(_isorgurl))
                {
                    //objErrorHandler.CreateLog("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + Session["EA"].ToString());
                    // objErrorHandler.CreateLog(requrl);
                   
                        Context.RewritePath("/mfl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                   
                }





            }
            //else if ((requrl.Contains("pd.aspx")) || (requrl.Contains("mpd.aspx")))
            // {

            //         //string[] getvalue = rawurl.Split('/');
            //     string family = checkurl[2];
            //         string[] fid = family.Split('-');
            //         _fid = fid[fid.Length - 1];
            //         Get2SubCatEA(_fid);



            //         Session["EA"] = Session["EA"] + "////" + "UserSearch=Family Id=" + _fid;
            //         string product = checkurl[1];
            //         string[] pid = product.Split('-');
            //         _pid = pid[pid.Length - 1];
            //         EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");

            //         if (!(_isorgurl))
            //         {
            //             if (requrl.Contains("mpd.aspx"))
            //                 Context.RewritePath("/mpd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
            //             else
            //                 Context.RewritePath("/pd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

            //         }

            // }
            else if ((requrl.Contains("mpd.aspx")))
            {
                Array.Reverse(checkurl);  
                //string[] getvalue = rawurl.Split('/');
               // string family = checkurl[5];
                //string[] fid = family.Split('-');
                string family = checkurl[2];
                _fid = family;
                Get2SubCatEA_MS(_fid);
                if (_fid == "")
                {
                    Array.Reverse(checkurl);
                    for (int i = 0; i < checkurl.Length; i++)
                    {
                        if (IsNumber(checkurl[2]))
                        {
                            _fid = checkurl[2];
                            break;
                        }
                    }
                }


                Session["EA"] = Session["EA"] + "////" + "UserSearch=Family Id=" + _fid;
                string product = checkurl[3];
                string[] pid = product.Split('-');
                _pid = pid[pid.Length - 1];
                if (_pid == "" || IsNumber(_pid) == false)
                {

                    Response.RedirectPermanent("/404New.htm");
                }
                EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");

                if (!(_isorgurl))
                {

                    Context.RewritePath("/mpd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                }

            }
        }
        catch (ThreadAbortException)
        {
           //s objErrorHandler.CreateLog_new("MainCategory----- " + HttpContext.Current.Request.RawUrl + "------- " );    
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            sHTML = ex.Message;
        }




        return sHTML;
    }
    public void Redirect_To_New_URL(string url)
    {
        try
        {
            string ConsNewUrl = string.Empty;
            if (url.Contains("/ct/"))
            {
                string[] spliturl = url.Split('/');

                if (spliturl.Length == 6)
                {

                    ConsNewUrl = "/" + spliturl[1] + "/" + spliturl[2] + "/" + spliturl[4] + "/";
                    Response.RedirectPermanent(ConsNewUrl.Replace("//", "/"));

                }
                else
                {

                    ConsNewUrl = "/" + spliturl[1] + "/" + spliturl[3] + "/";
                    Response.RedirectPermanent(ConsNewUrl.Replace("//", "/"));


                }

            }
            else if (url.Contains("/bb/"))
            {

                string[] spliturl = url.Split('/');
                Array.Reverse(spliturl);


                ConsNewUrl = "/" + spliturl[5] + "/" + spliturl[4] + "/" + spliturl[3] + "/" + "bb" + "/";
                Response.RedirectPermanent(ConsNewUrl.Replace("//", "/"));


            }
            else if (url.Contains("/pl/"))
            {
                string[] spliturl = url.Split('/');
                Array.Reverse(spliturl);


                ConsNewUrl = "/" + spliturl[4] + "/" + spliturl[3] + "/" + "pl" + "/";

                Response.RedirectPermanent(ConsNewUrl.Replace("//","/"));








            }
            else if (url.Contains("/ps/"))
            {
                string[] spliturl = url.Split('/');
                Array.Reverse(spliturl);

                if (spliturl[3] != "")
                {
                    ConsNewUrl = "/" + spliturl[3] + "/ps" + "/";
                    Response.RedirectPermanent(ConsNewUrl);
                }
                else
                {
                    Response.RedirectPermanent("/404New.html");
                }
                

            }
            else if (url.Contains("/fl/"))
            {
                // string rawurl = Request.RawUrl.ToString();
                string x = objHelperServices.URlStringReverse(querystring);
                string[] oldurl = x.Split('/');
                string familyname = string.Empty;
                string fid = string.Empty;
                for (int i = 0; i < oldurl.Length; i++)
                {


                    if (oldurl[i].Contains("="))
                    {

                        string[] Gettype = oldurl[i].Split(new string[] { "=" }, StringSplitOptions.None);


                        if (IsNumber(Gettype[0]) == true)
                        {
                            fid = Gettype[0];
                            familyname = Gettype[1];
                          
                        }
                       
                    }


                }

                if (fid != string.Empty)
                {

                    string CATEGORY_PATH = string.Empty;
                    DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                    if (tmpds != null && tmpds.Tables.Count > 0)
                    {


                        string[] catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                        CATEGORY_PATH = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////";
                    }
                    ConsNewUrl = "AllProducts////WESAUSTRALASIA////" + fid + "////" + CATEGORY_PATH + "////" + familyname;
                    ConsNewUrl = objHelperServices.SimpleURL_Str(ConsNewUrl, "fl.aspx", false);
                    Response.RedirectPermanent("/" + ConsNewUrl + "/fl/");
                }
                else
                {

                    Response.RedirectPermanent("/home.aspx");
                }

            }
            else if (url.Contains("/pd/"))
            {
                // string rawurl = Request.RawUrl.ToString();
                string x = objHelperServices.URlStringReverse(querystring);
                string[] oldurl = x.Split('/');
                string familyname = string.Empty;
                string fid = string.Empty;
                string pid = string.Empty;
                string pcode = string.Empty;
                for (int i = 0; i < oldurl.Length; i++)
                {


                    if (oldurl[i].Contains("="))
                    {

                        string[] Gettype = oldurl[i].Split(new string[] { "=" }, StringSplitOptions.None);


                        if (IsNumber(Gettype[0]) == true)
                        {
                            if (fid == string.Empty)
                            {
                                fid = Gettype[0];
                                familyname = Gettype[1];
                            }
                            else
                            {
                                pid = Gettype[0];
                                pcode = Gettype[1];
                            }

                        }
                    }


                }
                if (fid != string.Empty)
                {
                    string CATEGORY_PATH = string.Empty;
                    DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                    if (tmpds != null && tmpds.Tables.Count > 0)
                    {


                        string[] catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                        CATEGORY_PATH = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////";
                    }
                    ConsNewUrl = "AllProducts////WESAUSTRALASIA////" + fid + "////" + pid + "=" + pcode + "////" + CATEGORY_PATH + "////" + familyname;
                    ConsNewUrl = objHelperServices.SimpleURL_Str(ConsNewUrl, "pd.aspx", false);
                    Response.RedirectPermanent("/" + ConsNewUrl + "/pd/");
                }
                else
                {
                    Response.Redirect("/home.aspx");  
                }

            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            //handle
           Response.RedirectPermanent("/home.aspx");
        }

    }
    public void Redirect_To_New_URL_MS(string url)
    {
        try
        {
            string ConsNewUrl = string.Empty;
            if (url.Contains("/mct/"))
            {
                string[] spliturl = url.Split('/');

                if (spliturl.Length == 6)
                {

                    ConsNewUrl = "/" + spliturl[1] + "/" + spliturl[2] + "/" + spliturl[4] + "/";
                    Response.RedirectPermanent(ConsNewUrl.Replace("//", "/"));

                }
                else
                {

                    ConsNewUrl = "/" + spliturl[1] + "/" + spliturl[3] + "/";
                    Response.RedirectPermanent(ConsNewUrl.Replace("//", "/"));


                }

            }
            else if (url.Contains("/mbb/"))
            {

                string[] spliturl = url.Split('/');
                Array.Reverse(spliturl);


                ConsNewUrl = "/" + spliturl[5] + "/" + spliturl[4] + "/" + spliturl[3] + "/" + "mbb" + "/";
                Response.RedirectPermanent(ConsNewUrl);


            }
            else if (url.Contains("/mpl/"))
            {
                string[] spliturl = url.Split('/');



                ConsNewUrl = "/" + spliturl[1] + "/" + spliturl[2] + "/" + "mpl" + "/";
                Response.RedirectPermanent(ConsNewUrl.Replace("//", "/"));

            }
            else if (url.Contains("/mps/"))
            {
                string[] spliturl = url.Split('/');
                Array.Reverse(spliturl);

                if (spliturl[3] != "")
                {
                    ConsNewUrl = "/" + spliturl[3] + "mps" + "/";
                    Response.RedirectPermanent(ConsNewUrl);
                }
                else
                {
                    Response.RedirectPermanent("/home.aspx");
                }
             

            }
            else if (url.Contains("/mfl/"))
            {
                //string rawurl = Request.RawUrl.ToString();
                string x = objHelperServices.URlStringReverse_MS(querystring);
                string[] oldurl = x.Split('/');
                string familyname = string.Empty;
                string fid = string.Empty;
                for (int i = 0; i < oldurl.Length; i++)
                {


                    if (oldurl[i].Contains("="))
                    {

                        string[] Gettype = oldurl[i].Split(new string[] { "=" }, StringSplitOptions.None);


                        if (IsNumber(Gettype[0]) == true)
                        {
                            fid = Gettype[0];
                            familyname = Gettype[1];
                         
                        }
                       
                    }

                 
                }
                if (fid != string.Empty)
                {
                    string CATEGORY_PATH = string.Empty;
                    DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, fid, _catCid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                    if (tmpds != null && tmpds.Tables.Count > 0)
                    {


                        string[] catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                        CATEGORY_PATH = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////";
                    }
                    ConsNewUrl = "AllProducts////WESAUSTRALASIA////" + CATEGORY_PATH + "////" + familyname + "////" + fid;
                    ConsNewUrl = objHelperServices.SimpleURL_MS_Str(ConsNewUrl, "fl.aspx", false);
                    Response.RedirectPermanent("/" + ConsNewUrl + "/mfl/");
                }
                else
                {
                    Response.Redirect("/home.aspx"); 
                }

            }
            else if (url.Contains("/mpd/"))
            {
                //string rawurl = Request.RawUrl.ToString();
                string x = objHelperServices.URlStringReverse_MS(querystring);
                string[] oldurl = x.Split('/');
                string familyname = string.Empty;
                string fid = string.Empty;
                string pid = string.Empty;
                string pcode = string.Empty;
                for (int i = 0; i < oldurl.Length; i++)
                {


                    if (oldurl[i].Contains("="))
                    {

                        string[] Gettype = oldurl[i].Split(new string[] { "=" }, StringSplitOptions.None);


                        if (IsNumber(Gettype[0]) == true)
                        {
                            if (fid == string.Empty)
                            {
                                fid = Gettype[0];
                                familyname = Gettype[1];
                            }
                            else
                            {
                                pid = Gettype[0];
                                pcode = Gettype[1];
                            }

                        }
                    }


                }
                if (fid != string.Empty)
                {
                    string CATEGORY_PATH = string.Empty;
                    string[] sptrawurl = querystring.Split('/');

                    CATEGORY_PATH = sptrawurl[1] + "////" + sptrawurl[2];
                    //DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, fid,_catCid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                    //if (tmpds != null && tmpds.Tables.Count > 0)
                    //{


                    //    string[] catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                    //   //+ "////" + catpath[1] + "////";
                    //    CATEGORY_PATH = CategoryName + "////" + (catpath.Length >= 1 ? catpath[0] : " ");
                    //}
                    ConsNewUrl = "AllProducts////WESAUSTRALASIA////" + CATEGORY_PATH + "////" + familyname + "////" + pid + "=" + pcode + "////" + fid;
                    ConsNewUrl = objHelperServices.SimpleURL_MS_Str(ConsNewUrl, "mpd.aspx", false);
                    Response.RedirectPermanent("/" + ConsNewUrl + "/mpd/");
                }
                else
                {
                    Response.RedirectPermanent("/home.aspx");
                }

            }

        }
        catch
        {

            Response.Redirect("/home.aspx");
        }
    }
    protected void logoutsession(object sender, EventArgs e)
    {
        objUserServices.OnLineFlag(false, objHelperServices.CI(Session["USER_ID"]));
        Session.RemoveAll();
        Session.Clear();
        Session.Abandon();
        Session["USER_ID"] = "";
         Response.Redirect("/Login.aspx",false);
    }

    
     //private DataSet GetDataSet(string SQLQuery)
     //{
     //    DataSet ds = new DataSet();
     //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
     //    da.Fill(ds, "generictable");
     //    return ds;
     //}
    bool IsNumber(string text)
    {
        Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
        return regex.IsMatch(text);
    }
     private string GetCID(string familyid)
     {
         try
         {


             DataSet DSBC = null;
             string catIDtemp = string.Empty;
             //DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + familyid);
             //foreach (DataRow DR in DSBC.Tables[0].Rows)
             //{
             //catIDtemp = DR[1].ToString();
             //}
             DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(familyid, "GET_MAINCATEGORY_CAREGORY_ID", HelperDB.ReturnType.RTDataSet);

             if (DSBC != null && DSBC.Tables.Count > 0)
                 catIDtemp = DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
             else
                 catIDtemp = "";
             do
             {
                 //DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                 DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORYLIST_CAREGORY", HelperDB.ReturnType.RTDataSet);
                 if (DSBC != null)
                 {
                     foreach (DataRow DR in DSBC.Tables[0].Rows)
                     {
                         catIDtemp = DR["PARENT_CATEGORY"].ToString();
                         MCID = DR["CATEGORY_ID"].ToString() + ">" + MCID;
                     }
                 }
             } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
             MCID = DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString() + ">" + MCID;
             return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
         }
         catch (Exception ex)
         {

         }
         return "";

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

         }
         return "";
     }
     private string GetMCatID(string MainCategoryid)
     {
         DataSet DSBC = null;
         string catIDtemp = string.Empty;
         catIDtemp = MainCategoryid;
         MCID = MainCategoryid + ">" + MCID;
         do
         {
             //DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
             DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORYLIST_CAREGORY", HelperDB.ReturnType.RTDataSet);
             if (DSBC != null)
             {
                 foreach (DataRow DR in DSBC.Tables[0].Rows)
                 {
                     catIDtemp = DR["PARENT_CATEGORY"].ToString();
                     MCID = DR["PARENT_CATEGORY"].ToString() + ">" + MCID;
                 }
                 if (DSBC.Tables[0].Rows.Count <= 0)
                     return "0";
             }
         } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
         return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
     }





     //protected string Get_Value_Breadcrum()
     //{

     //    string sHTML = string.Empty;

     //    try
     //    {



     //        string _tsb = string.Empty;
     //        string _tsm = string.Empty;
     //        string _type = string.Empty;
     //        string _value = string.Empty;
     //        string _bname = string.Empty;
     //        string _searchstr = string.Empty;
     //        string _byp = "2";
     //        string _bypcat = null;
     //        string orgeapath = string.Empty;
     //        bool _isorgurl = false;
     //        string _pid = string.Empty;
     //        string _fid = string.Empty;
     //        string _seeall = string.Empty;
     //        string _catname = string.Empty;
     //        _bypcat = Request.QueryString["bypcat"];

     //        string CatName = string.Empty;


     //        if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != string.Empty)
     //            _tsm = Request.QueryString["tsm"];

     //        if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != string.Empty)
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

     //        if (Request.QueryString["fid"] != null)
     //            _fid = Request.QueryString["fid"];
     //        if (Request.QueryString["pid"] != null)
     //            _pid = Request.QueryString["pid"];

     //        if (Request.QueryString["seeall"] != null)
     //            _seeall = Request.QueryString["seeall"];


     //        if (_catCid != "")
     //            _parentCatID = GetParentCatID(_catCid);
     //        string requrl = HttpContext.Current.Request.Url.ToString().ToLower();
     //        if (requrl.Contains("pd.aspx") == true)
     //        {
     //            if (HttpContext.Current.Session["Category_Attributes"] == null && _parentCatID != string.Empty)
     //            {
     //                EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
     //            }
     //        }

     //        int ckbrand = 0;

     //        if ((Request.QueryString["path"] != null) && (hfisselected.Value == string.Empty))
     //        {
     //            HttpContext.Current.Session["EA"] = objSecurity.StringDeCrypt(Request.QueryString["path"].ToString());
     //            _isorgurl = true;
     //        }
     //        else if (!(requrl.Contains("login.aspx")) && !(requrl.Contains("home.aspx")) || !(requrl.Contains(".png")))
     //        {
     //            HttpContext.Current.Session["EA"] = "";
     //            string querystring = string.Empty;
     //            if (hfisselected.Value.Equals("1"))
     //            {

     //                if (!(hforgurl.Value.Contains("BrandModel")))
     //                {
     //                    querystring = hforgurl.Value;
     //                }
     //                else
     //                {
     //                    querystring = hforgurl.Value.Replace("bb.aspx?", "");

     //                    StringBuilder SBquerystring = new StringBuilder(querystring);
     //                    SBquerystring.Replace('_', ' ');
     //                    SBquerystring.Replace("||", "+");
     //                    SBquerystring.Replace("``", "\"");
     //                    SBquerystring.Replace(" / ", "~..~");
     //                    SBquerystring.Replace(" /", "~..");
     //                    SBquerystring.Replace("/ ", "..~");
     //                    SBquerystring.Replace("./.", ".~.");
     //                    querystring = SBquerystring.ToString();
     //                    string[] ConsURL1 = querystring.Split(new string[] { "/" }, StringSplitOptions.None);
     //                    _tsb = ConsURL1[1];
     //                    _tsm = ConsURL1[2];
     //                    // Context.RewritePath("bb.aspx?" + querystring); 
     //                }

     //            }
     //            else
     //            {
     //                querystring = Request.RawUrl.ToString();

     //                if (querystring.Contains("&gclid="))
     //                {
     //                    string[] querystring1 = querystring.Split(new string[] { "&gclid=" }, StringSplitOptions.None);
     //                    querystring = querystring1[0];
     //                }
     //                else if (querystring.Contains("?gclid="))
     //                {
     //                    string[] querystring1 = querystring.Split(new string[] { "?gclid=" }, StringSplitOptions.None);
     //                    querystring = querystring1[0];
     //                }
     //                if (querystring.EndsWith("/"))
     //                {

     //                    querystring = querystring.Substring(0, querystring.Length - 1);
     //                }
     //                string[] qs = querystring.Split(new string[] { "?" }, StringSplitOptions.None);
     //                if (qs.Length >= 1)
     //                {
     //                    querystring = qs[1];
     //                }

     //                // querystring= HttpUtility.UrlDecode(Request.Url.Query.ToString().Replace("?", ""));
     //            }
     //            string dbq = HttpUtility.UrlDecode("%E2%80%9C");
     //            string dbq1 = HttpUtility.UrlDecode("%E2%80%9D");
     //            string dbq2 = HttpUtility.UrlDecode("%C3%98");
     //            StringBuilder SBquerystring1 = new StringBuilder(querystring);
     //            SBquerystring1.Replace('_', ' ');
     //            SBquerystring1.Replace("``", "\"");
     //            SBquerystring1.Replace("&srctext=", "");
     //            SBquerystring1.Replace("%E2%80%9C", dbq);
     //            SBquerystring1.Replace("%E2%80%9D", dbq1);
     //            SBquerystring1.Replace("%C3%98", dbq2);
     //            SBquerystring1.Replace(" / ", "~..~");
     //            SBquerystring1.Replace(" /", "~..");
     //            SBquerystring1.Replace("/ ", "..~");
     //            SBquerystring1.Replace("./.", ".~.");

     //            querystring = SBquerystring1.ToString();
     //            string[] ConsURL = querystring.Split('/');
     //            string ea = string.Empty;
     //            if (ConsURL.Length >= 2)
     //            {
     //                ea = querystring.Replace(ConsURL[ConsURL.Length - 1], "");
     //            }
     //            else
     //            {
     //                ea = querystring;
     //            }
     //            StringBuilder sb_ea = new StringBuilder(ea);
     //            sb_ea.Replace("~..~", " / ");
     //            sb_ea.Replace("~..", " /");
     //            sb_ea.Replace("..~", "/ ");
     //            sb_ea.Replace(".~.", "/");
     //            ea = sb_ea.ToString();
     //            // string ea = querystring;
     //            string newea = string.Empty;
     //            if (ea.Substring(ea.Length - 1, 1).Contains("/"))
     //            {
     //                newea = ea.Substring(0, ea.Length - 1);
     //            }
     //            else
     //            {

     //                newea = ea;
     //            }
     //            string eapath = string.Empty;
     //            int fid = 0;
     //            if (ConsURL.Length <= 1)
     //            {

     //                // HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////Cellular Accessories////" + newea.Replace("/", "////");
     //                //if (requrl.Contains("bb.aspx"))
     //                //{
     //                //    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////Cellular Accessories////" + "AttribSelect=Brand='" + newea.Replace("/", "////") + "'";
     //                //    _tsb = newea.Replace("/", "////");
     //                //}
     //                if (requrl.Contains("powersearch.aspx"))
     //                {

     //                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + "UserSearch=" + newea.Replace("/", "////").Replace("&srctext=", "");
     //                    _searchstr = newea.Replace("/", "////").Replace("&srctext=", "");
     //                }
     //                if (requrl.Contains("ct.aspx") && newea.ToLower().Contains("brand").Equals(true))
     //                {
     //                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
     //                    _tsb = ea.Replace("BRAND=", "").Replace("Brand=", "");
     //                    _tsbnew = ea.Replace("BRAND=", "").Replace("Brand=", "");
     //                    _bypcat = "2";
     //                }
     //            }
     //            else
     //            {
     //                string brand = string.Empty;
     //                for (int i = 0; i <= ConsURL.Length - 2; i++)
     //                {
     //                    //ConsURL[i] = ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" ); 
     //                    StringBuilder SB_ConsURL = new StringBuilder(ConsURL[i]);

     //                    SB_ConsURL.Replace("~..~", " / ");
     //                    SB_ConsURL.Replace("~..", " /");
     //                    SB_ConsURL.Replace("..~", "/ ");
     //                    SB_ConsURL.Replace(".~.", "/");
     //                    if (ConsURL[i].ToString().Contains("="))
     //                    {

     //                        string[] Gettype = ConsURL[i].Split(new string[] { "=" }, StringSplitOptions.None);
     //                        StringBuilder SB_gtype = new StringBuilder(Gettype[1]);
     //                        SB_gtype.Replace("~..~", " / ");
     //                        SB_gtype.Replace("~..", " /");
     //                        SB_gtype.Replace("..~", "/ ");
     //                        SB_gtype.Replace(".~.", "/");
     //                        string gettype1 = SB_gtype.ToString();

     //                        if (IsNumber(Gettype[0]) == false)
     //                        {

     //                            //if (requrl.Contains("ct.aspx") == true  && Gettype[0].ToLower()!="brand")
     //                            //{
     //                            //    //_catCid = Gettype[0];
     //                            //    //hfcid.Value = _catCid;
     //                            //    _catname  = Gettype[1];


     //                            //}
     //                            string gettype0 = Gettype[0].ToLower();
     //                            if (gettype0 == "brand")
     //                            {
     //                                brand = gettype1;
     //                                _tsb = brand;
     //                                _bname = brand;
     //                                ckbrand = 1;
     //                                _bypcat = "2";
     //                            }
     //                            if (gettype0 == "model")
     //                            {
     //                                if (gettype1.ToString().Contains(":"))
     //                                {
     //                                    string[] getbname = gettype1.Split(new string[] { ":" }, StringSplitOptions.None);

     //                                    eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + getbname[0].Replace("'", "") + ":" + getbname[1].Replace("'", "") + "'";
     //                                }
     //                                else
     //                                {
     //                                    eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + brand + ":" + gettype1 + "'";
     //                                    _tsm = Gettype[1];
     //                                }
     //                                ckbrand = 2;
     //                            }

     //                            else if (gettype0 == "category")
     //                            {
     //                                eapath = eapath + "////" + gettype1;
     //                            }
     //                            else if (gettype0 == "usersearch")
     //                            {
     //                                eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
     //                            }
     //                            else if (gettype0 == "usersearch1")
     //                            {
     //                                eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
     //                            }
     //                            else if (gettype0 == "usersearch2")
     //                            {
     //                                eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
     //                            }
     //                            else if ((gettype0 == "USER SEARCH") && ((requrl.Contains("fl.aspx")) || (requrl.Contains("pd.aspx"))))
     //                            {
     //                                eapath = eapath + "////" + "UserSearch" + "=" + gettype1;
     //                            }
     //                            //else if ((Gettype[0].ToLower() == "user SEARCH") && (requrl.Contains("fl.aspx")) || (requrl.Contains("pd.aspx")))
     //                            //{
     //                            //    eapath = eapath + "////" + "UserSearch" + "=" + gettype1;
     //                            //}
     //                            else
     //                            {
     //                                //if (i == 0)
     //                                //{
     //                                //   if (requrl.Contains("powersearch.aspx") == true)
     //                                //    {
     //                                eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + gettype1 + "'";
     //                                //}
     //                                //else
     //                                //{
     //                                //   eapath = eapath + "////" + gettype1 ;

     //                                //        hfcid.Value = Gettype[0];
     //                                //        _catCid = Gettype[0];

     //                                //}
     //                                //}
     //                                //else
     //                                //{
     //                                //    eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + gettype1 + "'";

     //                                //}

     //                            }
     //                        }
     //                        else if (fid != 1)
     //                        {
     //                            fid = 1;
     //                            _fid = Gettype[0];
     //                            eapath = eapath + "////" + "UserSearch=Family Id=" + Gettype[0];
     //                        }
     //                        else if (fid == 1)
     //                        {
     //                            eapath = eapath + "////" + "UserSearch1=Prod Id=" + Gettype[0];
     //                        }
     //                        //if((requrl.Contains("fl.aspx"))||(requrl.Contains("pd.aspx")))
     //                        //{
     //                        //    eapath = eapath.Replace("user_search", "UserSearch").Replace("user search", "UserSearch").Replace("USER_SEARCH", "UserSearch").Replace("USER SEARCH", "UserSearch").Replace("User_Search", "UserSearch").Replace("User Search", "UserSearch");   

     //                        //}

     //                        //if (requrl.Contains("ct.aspx") == true && ea.ToString().ToLower().Contains("brand")==false   )
     //                        //{
     //                        //   // _catCid = Gettype[0];
     //                        //    _value = Gettype[];
     //                        //   // hfcid.Value = _catCid;

     //                        //}
     //                    }
     //                    else if (requrl.Contains("bb.aspx"))
     //                    {



     //                        if (ckbrand == 0)
     //                        {
     //                            eapath = eapath + SB_ConsURL.ToString();
     //                            _catname = SB_ConsURL.ToString();
     //                            //_tsb = ConsURL[i];
     //                            //_bname = ConsURL[i];
     //                            ckbrand = 1;
     //                        }
     //                        else if (ckbrand == 1)
     //                        {
     //                            //ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" )
     //                            eapath = eapath + "////" + "AttribSelect=Brand='" + SB_ConsURL.ToString() + "'";
     //                            _tsb = SB_ConsURL.ToString();
     //                            _bname = SB_ConsURL.ToString();
     //                            ckbrand = 2;
     //                        }
     //                        else if (ckbrand == 2)
     //                        {
     //                            //ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" )
     //                            eapath = eapath + "////" + "AttribSelect=Model='" + _bname + ":" + SB_ConsURL.ToString() + "'";
     //                            _tsm = SB_ConsURL.ToString();
     //                            ckbrand = 2;
     //                        }

     //                        else
     //                        {
     //                            //ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" )
     //                            eapath = eapath + "////" + SB_ConsURL.ToString();
     //                        }


     //                    }

     //                    else
     //                    {

     //                        if (i == 0 && requrl.Contains("powersearch.aspx"))
     //                        {
     //                            eapath = "UserSearch=" + SB_ConsURL.ToString();
     //                            _searchstr = SB_ConsURL.ToString(); ;
     //                        }
     //                        else
     //                        {
     //                            eapath = eapath + "////" + SB_ConsURL.ToString();
     //                        }
     //                    }

     //                }
     //                StringBuilder SB_EAPATH = new StringBuilder(eapath);
     //                SB_EAPATH.Replace("?", "");
     //                SB_EAPATH.Replace("~..~", " / ");
     //                SB_EAPATH.Replace("~..", " /");
     //                SB_EAPATH.Replace("..~", "/ ");
     //                SB_EAPATH.Replace(".~.", "/");
     //                SB_EAPATH.Replace("||", "+");
     //                SB_EAPATH.Replace("^^", "&");
     //                SB_EAPATH.Replace("~`", ":");
     //                SB_EAPATH.Replace("~^", ".");

     //                HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + SB_EAPATH.ToString();

     //            }
     //            StringBuilder sb_value = new StringBuilder(ConsURL[ConsURL.Length - 1]);
     //            sb_value.Replace('_', ' ');
     //            sb_value.Replace("~..~", " / ");
     //            sb_value.Replace("~..", " /");
     //            sb_value.Replace("..~", "/ ");
     //            sb_value.Replace(".~.", "/");
     //            sb_value.Replace("||", "+");
     //            sb_value.Replace("^^", "&");
     //            sb_value.Replace("~`", ":");
     //            _value = sb_value.ToString();
     //            if (_value.Contains("="))
     //            {
     //                string[] Gettype = _value.Split(new string[] { "=" }, StringSplitOptions.None);
     //                string AttribSelect = string.Empty;
     //                if (IsNumber(Gettype[0]) == false)
     //                {

     //                    _type = Gettype[0];
     //                    _value = Gettype[1];

     //                    //AttribSelect = "AttribSelect=" + _type + "='" + _value + "'";

     //                    //if (requrl.Contains("ct.aspx") == true && Gettype[0].ToLower() != "brand")
     //                    //{
     //                    //    _catCid = Gettype[0];
     //                    //    _catname = Gettype[1];
     //                    //    _value = Gettype[1];
     //                    //    hfcid.Value = _catCid;
     //                    //}
     //                    //else
     //                    if ((requrl.Contains("ct.aspx")) && Gettype[0].ToLower() == "brand")
     //                    {
     //                        _tsb = Gettype[1];
     //                        _bypcat = "2";
     //                        // hidcatIds.Value = Gettype[1] + "^" + "1";
     //                    }
     //                    if (requrl.Contains("fl.aspx") == true)
     //                    {
     //                        if (IsNumber(Gettype[0]) == true)
     //                        {
     //                            _fid = Gettype[0];
     //                        }
     //                        if (Gettype[1].ToString().Contains(":"))
     //                        {
     //                            string[] getbname = _value.Split(new string[] { ":" }, StringSplitOptions.None);
     //                            if (Gettype[0].ToLower() == "model")
     //                            {
     //                                _bname = getbname[0].Replace("'", "");
     //                                _value = getbname[1].Replace("'", "");
     //                            }
     //                            else
     //                            {
     //                                _bname = getbname[0];
     //                                _value = getbname[1];
     //                            }
     //                        }
     //                    }

     //                    if ((requrl.Contains("pl.aspx")) || (requrl.Contains("powersearch.aspx")))
     //                    {

     //                        if (Gettype[1].ToString().Contains(":"))
     //                        {
     //                            string[] getbname = _value.Split(new string[] { ":" }, StringSplitOptions.None);
     //                            if (Gettype[0].ToLower() == "model")
     //                            {
     //                                _bname = getbname[0].Replace("'", "");
     //                                _value = getbname[1].Replace("'", "");
     //                            }
     //                            else
     //                            {
     //                                _bname = getbname[0];
     //                                _value = getbname[1];
     //                            }
     //                        }
     //                    }

     //                }
     //                else
     //                {
     //                    //if (requrl.Contains("ct.aspx") == true)
     //                    //{
     //                    //    _catCid = Gettype[0];
     //                    //    _value = Gettype[1];
     //                    //    hfcid.Value = _catCid;
     //                    //}else
     //                    if ((requrl.Contains("fl.aspx")))
     //                    {
     //                        _fid = Gettype[0];


     //                    }
     //                    else if ((requrl.Contains("pd.aspx")))
     //                    {
     //                        _pid = Gettype[0];
     //                    }

     //                }
     //                if (HttpContext.Current.Session["EA"] == "")
     //                {
     //                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
     //                }
     //                HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("USERSEARCH", "UserSearch").Replace("BRAND", "Brand").Replace("MODEL", "Model").Replace("CATEGORY", "Category");
     //                HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("////////", "////");
     //                // HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"] + "////" + AttribSelect;
     //                orgeapath = "AllProducts////WESAUSTRALASIA////" + eapath;
     //            }
     //            else if (requrl.Contains("bb.aspx"))
     //            {
     //                //  eapath = eapath + "////" + "AttribSelect=Model=" + newea + ":" + _value;
     //                _tsm = _value;

     //            }
     //            else if (requrl.Contains("powersearch.aspx"))
     //            {
     //                if (ConsURL.Length == 1)
     //                {
     //                    _searchstr = _value;
     //                    _value = "";
     //                }
     //                else
     //                {
     //                    _type = "Category";

     //                }
     //            }
     //            else
     //            {
     //                _type = "Category";

     //            }
     //            HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace('_', ' ').Replace("~..~", " / ").Replace("~..", " /").Replace("..~", "/ ").Replace(".~.", "/").Replace("||", "+").Replace("^^", "&").Replace("~`", ":"); ;
     //            orgeapath = HttpContext.Current.Session["EA"].ToString();
     //            string SessionEA = HttpContext.Current.Session["EA"].ToString();
     //        }



     //        else
     //            HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";

     //        if (HttpContext.Current.Session["MainCategory"] != null)
     //        {
     //            DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
     //            if (dr.Length > 0)
     //                _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
     //        }

     //        string _ViewType = string.Empty;
     //        if (Request.Cookies["GLVIEWMODE"] != null)
     //        {
     //            if (Request.Cookies["GLVIEWMODE"].Value != null)
     //            {
     //                _ViewType = Request.Cookies["GLVIEWMODE"].Value;
     //                Response.Cookies["GLVIEWMODE"].Expires = DateTime.Now.AddDays(-1);
     //                Session["GL_VIEWMODE"] = _ViewType;
     //                //  Response.Cookies["CLVIEWMODE"].Value = _ViewType;
     //                //  Response.Cookies["GLVIEWMODE"].Value = _ViewType;
     //                //  Response.Cookies["PLVIEWMODE"].Expires = DateTime.Now.AddDays(1);
     //            }
     //            else if (Session["GL_VIEWMODE"] != null)
     //            {
     //                _ViewType = Session["GL_VIEWMODE"].ToString();
     //            }
     //            else
     //            {
     //                _ViewType = "GV";
     //            }
     //        }
     //        else if (Session["GL_VIEWMODE"] != null)
     //        {
     //            _ViewType = Session["GL_VIEWMODE"].ToString();
     //        }
     //        else
     //        {
     //            _ViewType = "GV";
     //        }

     //        if (_isorgurl == true)
     //        {
     //            Context.RewritePath(Request.Url.PathAndQuery.Replace("/", "") + "&ViewMode=" + _ViewType);
     //            if (Request.QueryString["cid"] == null || Request.QueryString["cid"] == "")
     //            {
     //                Context.RewritePath(Request.Url.PathAndQuery.Replace("/", "") + "&ViewMode=" + _ViewType + "&cid=" + _catCid);
     //            }


     //        }
     //        if (requrl.Contains("ct.aspx") == true || requrl.Contains("microsite.aspx") == true || requrl.Contains("mct.aspx") == true)
     //        {

     //            if (_bypcat == null)
     //            {
     //                HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
     //                EasyAsk.GetMainMenuClickDetail(_catCid, "");
     //                // hfcid.Value = _catCid;


     //                DataTable tmptbl = null;

     //                if (HttpContext.Current.Session["MainMenuClick"] != null)
     //                {
     //                    tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0];

     //                    tmptbl = tmptbl.Select("CATEGORY_ID='" + _catCid + "'").CopyToDataTable();

     //                    if (tmptbl != null && tmptbl.Rows.Count > 0)
     //                    {
     //                        CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
     //                    }
     //                    else
     //                    {
     //                        CatName = _catname;
     //                    }


     //                }

                    
     //                if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
     //                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());
     //                //if (_value != "")
     //                //    _bname = _value;
     //                EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
     //                hfcname.Value = CatName.Replace("/", "//");
     //                //.Replace("+", "||").Replace("\"", "``").Replace("&", "^^").Replace(":", "~`");;

     //                if (!(_isorgurl))
     //                {
     //                    Context.RewritePath("ct.aspx?" + "&id=0&cid=" + _catCid + "&by=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

     //                }
     //            }
     //            else if (_tsb != string.Empty)
     //            {

     //                string parentCatName = GetCName(_catCid);
     //                EasyAsk.GetWESModel(parentCatName, iCatalogId, _tsb);
     //                if (hforgurl.Value == string.Empty)
     //                {
     //                    hforgurl.Value = parentCatName + "/" + "Brand=" + _tsb;
     //                }
     //                Context.RewritePath("ct.aspx?" + "&id=0&cid=" + _catCid + "&tsb=" + _tsb + "&bypcat=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

     //            }

     //        }
     //        //Added By:Indu
     //        //To get EA path with out attribyte Type

     //        if ((requrl.Contains("bb.aspx")))
     //        {
     //            int SubCatCount = 0;
     //            //Added by :Indu For meta tag
     //            Session["prodmodel"] = _tsm + "," + _tsb;
     //            if (_type == null || _type == string.Empty)
     //            {
     //                if (_tsb != null && _tsb != string.Empty && _tsm != null && _tsm != null)
     //                {

     //                    //string parentCatName = GetCName(ParentCatID);
     //                    //EasyAsk.GetBrandAndModelProducts(parentCatName, _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
     //                    if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
     //                        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

     //                    EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //                    if (!(_isorgurl))
     //                    {
     //                        Context.RewritePath("bb.aspx?&id=0&cid=" + _catCid + "&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    }


     //                }
     //            }
     //            else
     //            {
     //                if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
     //                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

     //                if (_type != string.Empty)
     //                {

     //                    EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //                    if (_tsb == string.Empty)
     //                    {
     //                        _tsb = _bname;
     //                    }
     //                    if (!(_isorgurl))
     //                    {
     //                        Context.RewritePath("bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&type=" + _type + "&value=" + HttpUtility.UrlEncode(_value) + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    }

     //                }
     //                else
     //                { //new open

     //                    EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //                    if (!(_isorgurl))
     //                    {
     //                        Context.RewritePath("bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    }
     //                }
     //            }
     //        }
     //        if ((requrl.Contains("pl.aspx")))
     //        {
     //            if (Session["RECORDS_PER_PAGE_pl"] != null)
     //                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_pl"].ToString());


     //            EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
     //            if (!(_isorgurl))
     //            {
     //                Context.RewritePath("pl.aspx?&cid=" + _catCid + "&type='" + _type + "'&value=" + _value + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //            }
     //        }
     //        if ((requrl.Contains("ps.aspx")))
     //        {
     //            if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
     //                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());

     //            EasyAsk.GetAttributeProducts("PowerSearch", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");


     //            if (!(_isorgurl))
     //            {
     //                if (_value != string.Empty)
     //                {
     //                    Context.RewritePath("ps.aspx?&id=0&searchstr=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                }
     //                else
     //                {

     //                    Context.RewritePath("ps.aspx?&id=0&searchstr=" + _searchstr + "&srctext=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                }
     //            }

     //        }




     //        if ((requrl.Contains("fl.aspx")))
     //        {
     //            if (_type == string.Empty)
     //            {

     //                EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");
     //                //objErrorHandler.CreateLog(requrl);
     //                //objErrorHandler.CreateLog("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&path=" + Session["EA"].ToString());   

     //                if (!(_isorgurl))
     //                {
     //                    Context.RewritePath("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&path=" + Session["EA"].ToString());
     //                }
     //            }
     //            else
     //            {

     //                EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value, _bname, "0", "0", "");

     //                if (!(_isorgurl))
     //                {
     //                    //objErrorHandler.CreateLog("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + Session["EA"].ToString());
     //                    // objErrorHandler.CreateLog(requrl);
     //                    Context.RewritePath("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                }
     //            }
     //        }
     //        if ((requrl.Contains("pd.aspx")))
     //        {
     //            if (_type == string.Empty)
     //            {

     //                EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");

     //                if (!(_isorgurl))
     //                {
     //                    Context.RewritePath("pd.aspx?&pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    //pd.aspx?&pid=11638&fid=&cid=&path=w69oz9yt7nzLyCLCqK9wJzcf%2bCuhoudzTudtlFipxiPHz%2ft4OIyi2p2AayFoZKaJqK4bYIkLuC2qI5%2foelciRlw7Prn5b%2bAE0ihTGyTCXDjjFjPDfiZiLlZjNR9CX82k8pdtFr2loSA%3d
     //                }
     //            }
     //            else
     //            {

     //                EasyAsk.GetAttributeProducts("ProductPage", _searchstr, _type, _value, _bname, "0", "0", "");
     //                if (!(_isorgurl))
     //                {
     //                    Context.RewritePath("pd.aspx?&pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "_searchstr" + _searchstr + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    //pd.aspx?&pid=11638&fid=&cid=&path=w69oz9yt7nzLyCLCqK9wJzcf%2bCuhoudzTudtlFipxiPHz%2ft4OIyi2p2AayFoZKaJqK4bYIkLuC2qI5%2foelciRlw7Prn5b%2bAE0ihTGyTCXDjjFjPDfiZiLlZjNR9CX82k8pdtFr2loSA%3d
     //                }
     //            }
     //        }
     //    }
     //    catch (Exception ex)
     //    {

     //        sHTML = ex.Message;
     //    }




     //    return sHTML;
     //}

     //protected string Get_Value_Breadcrum_New()
     //{

     //    string sHTML = string.Empty;

     //    try
     //    {



     //        string _tsb = string.Empty;
     //        string _tsm = string.Empty;
     //        string _type = string.Empty;
     //        string _value = string.Empty;
     //        string _bname = string.Empty;
     //        string _searchstr = string.Empty;
     //        string _byp = "2";
     //        string _bypcat = null;
     //        string orgeapath = string.Empty;
     //        bool _isorgurl = false;
     //        string _pid = string.Empty;
     //        string _fid = string.Empty;
     //        string _seeall = string.Empty;
     //        string _catname = string.Empty;
     //        _bypcat = Request.QueryString["bypcat"];

     //        string CatName = string.Empty;


     //        if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != string.Empty)
     //            _tsm = Request.QueryString["tsm"];

     //        if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != string.Empty)
     //            _tsb = Request.QueryString["tsb"];

     //        //if (Request.QueryString["type"] != null)
     //        //    _type = Request.QueryString["type"];

     //        //if (Request.QueryString["value"] != null)
     //        //    _value = Request.QueryString["value"];

     //        //if (Request.QueryString["bname"] != null)
     //        //    _bname = Request.QueryString["bname"];
     //        //if (Request.QueryString["searchstr"] != null)
     //        //    _searchstr = Request.QueryString["searchstr"];
     //        //if (Request.QueryString["srctext"] != null)
     //        //    _searchstr = Request.QueryString["srctext"];

     //        //if (Request.QueryString["fid"] != null)
     //        //    _fid = Request.QueryString["fid"];
     //        //if (Request.QueryString["pid"] != null)
     //        //    _pid = Request.QueryString["pid"];

     //        //if (Request.QueryString["seeall"] != null)
     //        //    _seeall = Request.QueryString["seeall"];


     //        if (_catCid != "")
     //            _parentCatID = GetParentCatID(_catCid);
     //        string requrl = HttpContext.Current.Request.Url.ToString().ToLower();
     //        if ((requrl.Contains("pd.aspx")) || (requrl.Contains("mpd.aspx")))
     //        {
     //            if (HttpContext.Current.Session["Category_Attributes"] == null && _parentCatID != string.Empty)
     //            {
     //                EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
     //            }
     //        }

     //        if (((requrl.Contains("mct.aspx"))) || (requrl.Contains("mpl.aspx"))
     //           || (requrl.Contains("mps.aspx"))
     //           || (requrl.Contains("mfl.aspx"))
     //           || (requrl.Contains("mpd.aspx")))
     //        {
     //            if (HttpContext.Current.Session["MainCategory"] == null || HttpContext.Current.Session["MainMenuClick"] == null)
     //            {
     //                HttpCookie LoginInfoCookie = Request.Cookies["ActiveSupplier"];
     //                if (LoginInfoCookie != null && LoginInfoCookie["SupplierID"] != null)
     //                {
     //                    EasyAsk.GetMainMenuClickDetail(objSecurity.StringDeCrypt_password(LoginInfoCookie["SupplierID"]).ToString(), "");
     //                }
     //                else if (_catCid != string.Empty)
     //                {
     //                    EasyAsk.GetMainMenuClickDetail(_catCid, "");
     //                }
     //            }
     //        }

     //        int ckbrand = 0;

     //        if ((Request.QueryString["path"] != null) && (hfisselected.Value == ""))
     //        {
     //            HttpContext.Current.Session["EA"] = objSecurity.StringDeCrypt(Request.QueryString["path"].ToString());
     //            _isorgurl = true;
     //        }
     //        else if (!(requrl.Contains("/Login.aspx")) && !(requrl.ToLower() .Contains("/mlogin.aspx")) && !(requrl.Contains("/Home.aspx")) || !(requrl.Contains(".png")))
     //        {
     //            HttpContext.Current.Session["EA"] = "";
     //            string querystring = string.Empty;
     //            if (hfisselected.Value.Equals("1") && hforgurl.Value != string.Empty)
     //            {

     //                if (!(hforgurl.Value.Contains("BrandModel")))
     //                {
     //                    querystring = hforgurl.Value;
     //                }
     //                else
     //                {
     //                    querystring = hforgurl.Value.Replace("bb.aspx?", "");

     //                    string[] ConsURL1 = querystring.Split(new string[] { "/" }, StringSplitOptions.None);
     //                    _tsb = ConsURL1[1];
     //                    _tsm = ConsURL1[2];
     //                }

     //            }
     //            else
     //            {
     //                string rawurl = string.Empty;
     //                string reqrawurl = Request.RawUrl.ToString().ToLower();
     //                if (reqrawurl.Contains("ct.aspx?") && reqrawurl.Contains("/ct/"))
     //                {
     //                    string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "ct.aspx?" }, StringSplitOptions.None);
     //                    rawurl = rawurl1[0];
     //                }
     //                else if (reqrawurl.Contains("mct.aspx?") && reqrawurl.Contains("/mct/"))
     //                {
     //                    string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "mct.aspx?" }, StringSplitOptions.None);
     //                    rawurl = rawurl1[0];
     //                }
     //                else if (reqrawurl.Contains("microsite.aspx?") && reqrawurl.Contains("/ct/"))
     //                {
     //                    string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "microsite.aspx?" }, StringSplitOptions.None);
     //                    rawurl = rawurl1[0];
     //                }
     //                else if (reqrawurl.Contains("bb.aspx?") && reqrawurl.Contains("/bb/"))
     //                {
     //                    string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "bb.aspx?" }, StringSplitOptions.None);
     //                    rawurl = rawurl1[0];
     //                }
     //                else if (reqrawurl.Contains("ps.aspx?") && reqrawurl.Contains("/ps/"))
     //                {
     //                    string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "ps.aspx?" }, StringSplitOptions.None);
     //                    rawurl = rawurl1[0];
     //                }
     //                else if (reqrawurl.Contains("mps.aspx?") && reqrawurl.Contains("/mps/"))
     //                {
     //                    string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "mps.aspx?" }, StringSplitOptions.None);
     //                    rawurl = rawurl1[0];
     //                }   
     //                else
     //                {
     //                    rawurl = Request.RawUrl.ToString().ToLower();
     //                }
     //                if (
     //                       rawurl.Contains("/mfl/") || rawurl.Contains("mfl.aspx")
     //              || rawurl.Contains("/mpl/") || rawurl.Contains("mpl.aspx")
     //              || rawurl.Contains("/mpd/") || rawurl.Contains("mpd.aspx")
     //           || rawurl.Contains("/mps/") || rawurl.Contains("mps.aspx"))
     //                {
     //                    querystring = objHelperServices.URlStringReverse_MS(rawurl);

     //                }
     //                else
     //                {
     //                    querystring = objHelperServices.URlStringReverse(rawurl);
     //                }
     //                querystring = "/" + querystring;

     //                StringBuilder querystring_new = new StringBuilder(querystring);
     //                querystring_new.Replace("/ct/", "ct.aspx?");
     //                querystring_new.Replace("/pl/", "pl.aspx?");
     //                querystring_new.Replace("/fl/", "fl.aspx?");
     //                querystring_new.Replace("/bb/", "bb.aspx?");
     //                querystring_new.Replace("/pd/", "pd.aspx?");
     //                querystring_new.Replace("/ps/", "ps.aspx?");
     //                querystring_new.Replace("/microsite/", "microsite.aspx?");
     //                querystring_new.Replace("/mct/", "mct.aspx?");
     //                  querystring_new.Replace("/mfl/", "mfl.aspx?");
     //                  querystring_new.Replace("/mpl/", "mpl.aspx?");
     //                  querystring_new.Replace("/mpd/", "mpd.aspx?");
     //                   querystring_new.Replace("/mps/", "mps.aspx?");
     //                   querystring_new.Replace("~pd~", "pd");
     //                   querystring_new.Replace("~fl~", "fl");
     //                   querystring_new.Replace("~pl~", "pl");
     //                   querystring_new.Replace("~ps~", "ps");
     //                   querystring_new.Replace("~ct~", "ct");
     //                   querystring_new.Replace("~bb~", "bb");
     //                   querystring_new.Replace("~bk~", "bk");
     //                   querystring_new.Replace("~bp~", "bp");
     //                   querystring_new.Replace("~mpl~", "mpl");
     //                   querystring_new.Replace("~mps~", "mps");

     //                   querystring = querystring_new.ToString(); 
     //                //requrl = querystring;
     //                if (querystring.Contains("&gclid="))
     //                {
     //                    string[] querystring1 = querystring.Split(new string[] { "&gclid=" }, StringSplitOptions.None);
     //                    querystring = querystring1[0];
     //                }
     //                else if (querystring.Contains("?gclid="))
     //                {
     //                    string[] querystring1 = querystring.Split(new string[] { "?gclid=" }, StringSplitOptions.None);
     //                    querystring = querystring1[0];
     //                }
     //                if (querystring.EndsWith("/"))
     //                {

     //                    querystring = querystring.Substring(0, querystring.Length - 1);
     //                }
     //                string[] qs = querystring.Split(new string[] { "?" }, StringSplitOptions.None);
     //                if (qs.Length >= 1)
     //                {
     //                    querystring = qs[1];
     //                }

                   
     //            }
     //            string dbq = HttpUtility.UrlDecode("%E2%80%9C");
     //            string dbq1 = HttpUtility.UrlDecode("%E2%80%9D");
     //            string dbq2 = HttpUtility.UrlDecode("%C3%98");
                
     //            string[] ConsURL = querystring.Split('/');
     //            string ea = string.Empty;
     //            if (ConsURL.Length >= 2)
     //            {
     //                ea = querystring.Replace(ConsURL[ConsURL.Length - 1], "");
     //            }
     //            else
     //            {
     //                ea = querystring;
     //            }
               
               
     //            string newea = string.Empty;
     //            if (ea.Substring(ea.Length - 1, 1).Contains("/"))
     //            {
     //                newea = ea.Substring(0, ea.Length - 1);
     //            }
     //            else
     //            {

     //                newea = ea;
     //            }
     //            newea = newea.Replace("~~", "/"); 
     //            string eapath = string.Empty;
     //            int fid = 0;
     //            if (ConsURL.Length <= 1)
     //            {

                    
     //                if (requrl.Contains("ps.aspx") || requrl.Contains("mps.aspx"))
     //                {

     //                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + "UserSearch=" + newea.Replace("/", "////").Replace("&srctext=", "");
     //                    _searchstr = newea.Replace("/", "////").Replace("&srctext=", "");
     //                }
     //                if (requrl.Contains("ct.aspx") && newea.ToLower().Contains("brand").Equals(true))
     //                {
     //                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
     //                    _tsb = ea.Replace("BRAND=", "").Replace("Brand=", "");
     //                    _tsbnew = ea.Replace("BRAND=", "").Replace("Brand=", "");
     //                    _bypcat = "2";
     //                }
     //            }
     //            else
     //            {
     //                string brand = string.Empty;
     //                for (int i = 0; i <= ConsURL.Length - 2; i++)
     //                {
                      
     //                    string SB_ConsURL = ConsURL[i].Replace("~~","/");
                      
     //                    if (SB_ConsURL.ToString().Contains("="))
     //                    {

     //                        string[] Gettype = SB_ConsURL.Split(new string[] { "=" }, StringSplitOptions.None);
                         
     //                        string gettype1 = Gettype[1].ToString();
     //                        if (IsNumber(Gettype[0]) == false)
     //                        {

                              
     //                            string gettype0 = Gettype[0].ToLower();
                              
     //                            if (gettype0 == "brand")
     //                            {
     //                                brand = gettype1;
     //                                _tsb = brand;
     //                                _bname = brand;
     //                                ckbrand = 1;
     //                                _bypcat = "2";
     //                            }
     //                            if (gettype0 == "model")
     //                            {
     //                                if (gettype1.ToString().Contains(":"))
     //                                {
     //                                    string[] getbname = gettype1.Split(new string[] { ":" }, StringSplitOptions.None);

     //                                    eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + getbname[0].Replace("'", "") + ":" + getbname[1].Replace("'", "") + "'";
     //                                }
     //                                else
     //                                {
     //                                    eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + brand + ":" + gettype1 + "'";
     //                                    _tsm = Gettype[1];
     //                                }
     //                                ckbrand = 2;
     //                            }

     //                            else if (gettype0 == "category")
     //                            {
     //                                eapath = eapath + "////" + gettype1;
     //                            }
     //                            else if (gettype0 == "usersearch")
     //                            {
     //                                eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
     //                            }
     //                            else if (gettype0 == "usersearch1")
     //                            {
     //                                eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
     //                            }
     //                            else if (gettype0 == "usersearch2")
     //                            {
     //                                eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
     //                            }
     //                            else if ((gettype0 == "USER SEARCH") && ((requrl.Contains("fl.aspx")) || (requrl.Contains("pd.aspx"))))
     //                            {
     //                                eapath = eapath + "////" + "UserSearch" + "=" + gettype1;
     //                            }
                              
     //                            else
     //                            {
                                    
     //                                eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + gettype1 + "'";
                                  
     //                            }
     //                        }
     //                        else if (fid != 1)
     //                        {
     //                            fid = 1;
     //                            _fid = Gettype[0];
     //                            eapath = eapath + "////" + "UserSearch=Family Id=" + Gettype[0];
     //                        }
     //                        else if (fid == 1)
     //                        {
     //                            eapath = eapath + "////" + "UserSearch1=Prod Id=" + Gettype[0];
     //                        }
     //                        //if((requrl.Contains("fl.aspx"))||(requrl.Contains("pd.aspx")))
     //                        //{
     //                        //    eapath = eapath.Replace("user_search", "UserSearch").Replace("user search", "UserSearch").Replace("USER_SEARCH", "UserSearch").Replace("USER SEARCH", "UserSearch").Replace("User_Search", "UserSearch").Replace("User Search", "UserSearch");   

     //                        //}

     //                        //if (requrl.Contains("ct.aspx") == true && ea.ToString().ToLower().Contains("brand")==false   )
     //                        //{
     //                        //   // _catCid = Gettype[0];
     //                        //    _value = Gettype[];
     //                        //   // hfcid.Value = _catCid;

     //                        //}
     //                    }
     //                    else if (requrl.Contains("bb.aspx"))
     //                    {



     //                        if (ckbrand == 0)
     //                        {
     //                            eapath = eapath + SB_ConsURL;
     //                            _catname = SB_ConsURL;
                            
     //                            ckbrand = 1;
     //                        }
     //                        else if (ckbrand == 1)
     //                        {
     //                            //ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" )
     //                            eapath = eapath + "////" + "AttribSelect=Brand='" + SB_ConsURL + "'";
     //                            _tsb = SB_ConsURL;
     //                            _bname = SB_ConsURL;
     //                            ckbrand = 2;
     //                        }
     //                        else if (ckbrand == 2)
     //                        {
                                
     //                            eapath = eapath + "////" + "AttribSelect=Model='" + _bname + ":" + SB_ConsURL + "'";
     //                            _tsm = SB_ConsURL;
     //                            ckbrand = 2;
     //                        }

     //                        else
     //                        {
                              
     //                            eapath = eapath + "////" + SB_ConsURL;
     //                        }


     //                    }

     //                    else
     //                    {

     //                        if (i == 0 && (requrl.Contains("/ps.aspx")))
     //                        {
     //                            eapath = "UserSearch=" + SB_ConsURL;
     //                            _searchstr = SB_ConsURL; 
     //                        }             
     //                        else if (i == 0 && (requrl.Contains("mps.aspx")))
     //                            eapath = eapath + SB_ConsURL;
     //                        else if (i == 1 && (requrl.Contains("mps.aspx")))
     //                            eapath =eapath + "////"+  "UserSearch=" + SB_ConsURL;                                 
     //                        else
     //                        {
     //                            eapath = eapath + "////" + SB_ConsURL;
     //                        }
     //                    }

     //                }
               
     //                eapath = eapath.Replace("~~", "/");
     //                HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + eapath.ToString();

     //            }
               
     //            _value = ConsURL[ConsURL.Length - 1].Replace("~~","/");
     //            if (_value.Contains("="))
     //            {
     //                string[] Gettype = _value.Split(new string[] { "=" }, StringSplitOptions.None);
     //                string AttribSelect = string.Empty;
     //                if (IsNumber(Gettype[0]) == false)
     //                {

     //                    _type = Gettype[0];
     //                    _value = Gettype[1];

                     
     //                    if (((requrl.Contains("mct.aspx"))|| (requrl.Contains("ct.aspx"))) && Gettype[0].ToLower() == "brand")
     //                    {
     //                        _tsb = Gettype[1];
     //                        _bypcat = "2";
                            
     //                    }
     //                    if ((requrl.Contains("fl.aspx")) || (requrl.Contains("mfl.aspx")))
     //                    {
     //                        if (IsNumber(Gettype[0]) == true)
     //                        {
     //                            _fid = Gettype[0];
     //                        }
     //                        if (Gettype[1].ToString().Contains(":"))
     //                        {
     //                            string[] getbname = _value.Split(new string[] { ":" }, StringSplitOptions.None);
     //                            if (Gettype[0].ToLower() == "model")
     //                            {
     //                                _bname = getbname[0].Replace("'", "");
     //                                _value = getbname[1].Replace("'", "");
     //                            }
     //                            else
     //                            {
     //                                _bname = getbname[0];
     //                                _value = getbname[1];
     //                            }
     //                        }
     //                    }

     //                    if (((requrl.Contains("pl.aspx"))) || ((requrl.Contains("ps.aspx")) || (requrl.Contains("mpl.aspx"))) || (requrl.Contains("mps.aspx")))
     //                    {

     //                        if (Gettype[1].ToString().Contains(":"))
     //                        {
     //                            string[] getbname = _value.Split(new string[] { ":" }, StringSplitOptions.None);
     //                            if (Gettype[0].ToLower() == "model")
     //                            {
     //                                _bname = getbname[0].Replace("'", "");
     //                                _value = getbname[1].Replace("'", "");
     //                            }
     //                            else
     //                            {
     //                                _bname = getbname[0];
     //                                _value = getbname[1];
     //                            }
     //                        }
     //                    }

     //                }
     //                else
     //                {
     //                    //if (requrl.Contains("ct.aspx") == true)
     //                    //{
     //                    //    _catCid = Gettype[0];
     //                    //    _value = Gettype[1];
     //                    //    hfcid.Value = _catCid;
     //                    //}else
     //                    if ((requrl.Contains("fl.aspx")) || (requrl.Contains("mfl.aspx")))
     //                    {
     //                        _fid = Gettype[0];


     //                    }
     //                    else if ((requrl.Contains("pd.aspx")) || (requrl.Contains("mpd.aspx")))
     //                    {
     //                        _pid = Gettype[0];
     //                    }

     //                }
     //                if (HttpContext.Current.Session["EA"] == null )
     //                {
     //                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
     //                }
     //                HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("USERSEARCH", "UserSearch").Replace("usersearch", "UserSearch").Replace("BRAND", "Brand").Replace("brand", "Brand").Replace("MODEL", "Model").Replace("model", "Model").Replace("CATEGORY", "Category").Replace("category", "Category");
     //                HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("////////", "////");
     //                // HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"] + "////" + AttribSelect;
     //                orgeapath = "AllProducts////WESAUSTRALASIA////" + eapath;
     //            }
     //            else if (requrl.Contains("bb.aspx") || requrl.Contains("mbb.aspx"))
     //            {
     //                //  eapath = eapath + "////" + "AttribSelect=Model=" + newea + ":" + _value;
     //                _tsm = _value;

     //            }
     //            else if (requrl.Contains("/ps.aspx"))
     //            {
     //                if (ConsURL.Length == 1)
     //                {
     //                    _searchstr = _value;
     //                    _value = "";
     //                }
     //                else
     //                {
     //                    _type = "Category";

     //                }
     //            }
     //            else if (requrl.Contains("mps.aspx"))
     //            {
     //                if (ConsURL.Length == 2)
     //                {
     //                    _searchstr = ConsURL[1].ToString();
     //                    _value = "";
     //                }
     //                else
     //                {
     //                    _type = "Category";

     //                }
     //            }
     //            else
     //            {
     //                _type = "Category";

     //            }
     //            HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace('_', ' ').Replace("~..~", " / ").Replace("~..", " /").Replace("..~", "/ ").Replace(".~.", "/").Replace("||", "+").Replace("^^", "&").Replace("~`", ":"); ;
     //            orgeapath = HttpContext.Current.Session["EA"].ToString();
     //            string SessionEA = HttpContext.Current.Session["EA"].ToString();
     //        }



     //        else
     //            HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";

     //        if (HttpContext.Current.Session["MainCategory"] != null)
     //        {
     //            DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
     //            if (dr.Length > 0)
     //                _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
     //        }

     //        string _ViewType = string.Empty;
     //        if (Request.Cookies["GLVIEWMODE"] != null)
     //        {
     //            if (Request.Cookies["GLVIEWMODE"].Value != null)
     //            {
     //                _ViewType = Request.Cookies["GLVIEWMODE"].Value;
     //                Response.Cookies["GLVIEWMODE"].Expires = DateTime.Now.AddDays(-1);
     //                Session["GL_VIEWMODE"] = _ViewType;
     //                //  Response.Cookies["CLVIEWMODE"].Value = _ViewType;
     //                //  Response.Cookies["GLVIEWMODE"].Value = _ViewType;
     //                //  Response.Cookies["PLVIEWMODE"].Expires = DateTime.Now.AddDays(1);
     //            }
     //            else if (Session["GL_VIEWMODE"] != null)
     //            {
     //                _ViewType = Session["GL_VIEWMODE"].ToString();
     //            }
     //            else
     //            {
     //                _ViewType = "GV";
     //            }
     //        }
     //        else if (Session["GL_VIEWMODE"] != null)
     //        {
     //            _ViewType = Session["GL_VIEWMODE"].ToString();
     //        }
     //        else
     //        {
     //            _ViewType = "GV";
     //        }

     //        if ((_isorgurl))
     //        {
     //            Context.RewritePath("/"+Request.Url.PathAndQuery.Replace("/", "") + "&ViewMode=" + _ViewType);
     //            if (Request.QueryString["cid"] == null || Request.QueryString["cid"] == string.Empty)
     //            {
     //                Context.RewritePath("/" + Request.Url.PathAndQuery.Replace("/", "") + "&ViewMode=" + _ViewType + "&cid=" + _catCid);
     //            }


     //        }

          
             


     //        if ((requrl.Contains("ct.aspx")) || (requrl.Contains("microsite.aspx")) || (requrl.Contains("mtc.aspx")))
     //        {

     //            if (_bypcat == null)
     //            {
                   
                   
                     
                     
     //                HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
     //                if (_catCid == string.Empty)
     //                {
     //                    _catCid = "WES0830";
     //                }
                    
     //                    EasyAsk.GetMainMenuClickDetail(_catCid, "");
                 
     //                DataTable tmptbl = null;
     //                string SHORT_DESC = string.Empty;
     //                if (HttpContext.Current.Session["MainMenuClick"] != null)
     //                {
     //                    tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0];

     //                    tmptbl = tmptbl.Select("CATEGORY_ID='" + _catCid + "'").CopyToDataTable();

     //                    if (tmptbl != null && tmptbl.Rows.Count > 0)
     //                    {
     //                        CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
     //                        SHORT_DESC = tmptbl.Rows[0]["SHORT_DESC"].ToString();
     //                    }
     //                    else
     //                    {
     //                        CatName = _catname;
     //                    }
                         


     //                }
     //                if (requrl.Contains("mct.aspx"))
     //                {
     //                    Session["SUPPLIER_NAME"] = CatName;
     //                    Session["SUPPLIER_CID"] = _catCid;
     //                    Session["SUPPLIER_DESC"] = SHORT_DESC;
     //                    SetCookie(CatName, _catCid);
     //                }
     //                else
     //                {
     //                    Session["SUPPLIER_NAME"] = "";
     //                    Session["SUPPLIER_CID"] = "";
     //                    Session["SUPPLIER_DESC"] = "";
     //                }

     //                if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
     //                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());
                    
     //                    EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //                hfcname.Value = CatName.Replace("/", "//");
             
     //                if (!(_isorgurl))
     //                {
     //                    if ((requrl.Contains("mct.aspx")))
     //                    {
     //                        Context.RewritePath("/mct.aspx?" + "&id=0&cid=" + _catCid + "&by=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    }
     //                    else
     //                        Context.RewritePath("/ct.aspx?" + "&id=0&cid=" + _catCid + "&by=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

     //                }
                     
                     

     //            }
     //            else if (_tsb != string.Empty)
     //            {

     //                string parentCatName = GetCName(_catCid);
     //                EasyAsk.GetWESModel(parentCatName, iCatalogId, _tsb);
     //                if (hforgurl.Value == string.Empty)
     //                {
     //                    hforgurl.Value = parentCatName + "/" + "Brand=" + _tsb;
     //                }
     //                if ((requrl.Contains("mtc.aspx")))
     //                       Context.RewritePath("/mct.aspx?" + "&id=0&cid=" + _catCid + "&tsb=" + _tsb + "&bypcat=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                else
     //                    Context.RewritePath("/ct.aspx?" + "&id=0&cid=" + _catCid + "&tsb=" + _tsb + "&bypcat=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

     //            }

     //        }
     //        //Added By:Indu
     //        //To get EA path with out attribyte Type

     //        if ((requrl.Contains("bb.aspx")) || (requrl.Contains("mbb.aspx")))
     //        {
     //            int SubCatCount = 0;
     //            //Added by :Indu For meta tag
     //            Session["prodmodel"] = _tsm + "," + _tsb;
     //            if (_type == null || _type == string.Empty)
     //            {
     //                if (_tsb != null && _tsb != string.Empty && _tsm != null && _tsm != null)
     //                {

     //                    //string parentCatName = GetCName(ParentCatID);
     //                    //EasyAsk.GetBrandAndModelProducts(parentCatName, _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
     //                    if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
     //                        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

     //                    EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //                    if (!(_isorgurl))
     //                    {
     //                        if ((requrl.Contains("mbb.aspx")))
     //                           Context.RewritePath("/mbb.aspx?&id=0&cid=" + _catCid + "&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                        else
     //                            Context.RewritePath("/bb.aspx?&id=0&cid=" + _catCid + "&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

     //                    }


     //                }
     //            }
     //            else
     //            {
     //                if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
     //                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

     //                if (_type != string.Empty)
     //                {

     //                    EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //                    if (_tsb == string.Empty)
     //                    {
     //                        _tsb = _bname;
     //                    }
     //                    if (!(_isorgurl))
     //                    {
     //                        if (requrl.Contains("mbb.aspx"))
     //                            Context.RewritePath("/mbb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&type=" + _type + "&value=" + HttpUtility.UrlEncode(_value) + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                        else
     //                            Context.RewritePath("/bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&type=" + _type + "&value=" + HttpUtility.UrlEncode(_value) + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

     //                    }

     //                }
     //                else
     //                { //new open

     //                    EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //                    if (!(_isorgurl))
     //                    {
     //                        if (requrl.Contains("mbb.aspx"))
     //                           Context.RewritePath("/mbb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                        else
     //                            Context.RewritePath("/bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    }
     //                }
     //            }
     //        }
     //        if ((requrl.Contains("pl.aspx")) || (requrl.Contains("mpl.aspx")))
     //        {
     //            if (Session["RECORDS_PER_PAGE_pl"] != null)
     //                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_pl"].ToString());


     //            EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
     //            if (!(_isorgurl))
     //            {
     //                if (requrl.Contains("mpl.aspx"))
     //                   Context.RewritePath("/mpl.aspx?cid=" + _catCid + "&type='" + _type + "'&value=" + _value + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                else
     //                    Context.RewritePath("/pl.aspx?&cid=" + _catCid + "&type='" + _type + "'&value=" + _value + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //            }
     //        }
     //        if ((requrl.Contains("ps.aspx")) || (requrl.Contains("mps.aspx")))
     //        {
     //            if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
     //                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());
     //            if ((_searchstr == "~fl~") || (_searchstr == "~pl~") || (_searchstr == "~ps~") || (_searchstr == "~ct~") || (_searchstr == "~pd~") || (_searchstr == "~bb~") || (_searchstr == "~bp~") || (_searchstr == "~bk~"))
     //            {
     //                _searchstr = _searchstr.Replace("~", ""); 
     //            }
     //            if ((_value == "~fl~") || (_value == "~pl~") || (_value == "~ps~") || (_value == "~ct~") || (_value == "~pd~") || (_value == "~bb~") || (_value == "~bp~") || (_value == "~bk~"))
     //            {
     //                _value = _value.Replace("~", "");
     //            }
     //            EasyAsk.GetAttributeProducts("ps", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");


     //            if (!(_isorgurl))
     //            {
     //                if (_value != "")
     //                {
     //                    if (requrl.Contains("mps.aspx"))
     //                       Context.RewritePath("/mps.aspx?&id=0&searchstr=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    else
     //                        Context.RewritePath("/ps.aspx?&id=0&searchstr=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

     //                }
     //                else
     //                {
     //                    if (requrl.Contains("mps.aspx"))
     //                       Context.RewritePath("/mps.aspx?&id=0&searchstr=" + _searchstr + "&srctext=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    else
     //                        Context.RewritePath("/ps.aspx?&id=0&searchstr=" + _searchstr + "&srctext=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                }
     //            }

     //        }




     //        if ((requrl.Contains("fl.aspx"))||(requrl.Contains("mfl.aspx"))) 
     //        {
     //            if (_type == string.Empty)
     //            {

     //                EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");
     //                //objErrorHandler.CreateLog(requrl);
     //                //objErrorHandler.CreateLog("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&path=" + Session["EA"].ToString());   

     //                if (!(_isorgurl))
     //                {
     //                    if (requrl.Contains("mfl.aspx"))
     //                       Context.RewritePath("/mfl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&path=" + Session["EA"].ToString());
     //                    else
     //                        Context.RewritePath("/fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&path=" + Session["EA"].ToString());
     //                }
     //            }
     //            else
     //            {

     //                EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value, _bname, "0", "0", "");

     //                if (!(_isorgurl))
     //                {
     //                    //objErrorHandler.CreateLog("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + Session["EA"].ToString());
     //                    // objErrorHandler.CreateLog(requrl);
     //                    if (requrl.Contains("mfl.aspx"))
     //                        Context.RewritePath("/mfl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    else
     //                        Context.RewritePath("/fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                }
     //            }
     //        }
     //        if ((requrl.Contains("pd.aspx")) || (requrl.Contains("mpd.aspx")))
     //        {
     //            if (_type == string.Empty)
     //            {

     //                EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");

     //                if (!(_isorgurl))
     //                {
     //                    if (requrl.Contains("mpd.aspx"))
     //                       Context.RewritePath("/mpd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    else
     //                        Context.RewritePath("/pd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    //pd.aspx?&pid=11638&fid=&cid=&path=w69oz9yt7nzLyCLCqK9wJzcf%2bCuhoudzTudtlFipxiPHz%2ft4OIyi2p2AayFoZKaJqK4bYIkLuC2qI5%2foelciRlw7Prn5b%2bAE0ihTGyTCXDjjFjPDfiZiLlZjNR9CX82k8pdtFr2loSA%3d
     //                }
     //            }
     //            else
     //            {

     //                EasyAsk.GetAttributeProducts("ProductPage", _searchstr, _type, _value, _bname, "0", "0", "");
     //                if (!(_isorgurl))
     //                {
     //                    if (requrl.Contains("mpd.aspx"))
     //                       Context.RewritePath("/mpd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "_searchstr" + _searchstr + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    else
     //                        Context.RewritePath("/pd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "_searchstr" + _searchstr + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
     //                    //pd.aspx?&pid=11638&fid=&cid=&path=w69oz9yt7nzLyCLCqK9wJzcf%2bCuhoudzTudtlFipxiPHz%2ft4OIyi2p2AayFoZKaJqK4bYIkLuC2qI5%2foelciRlw7Prn5b%2bAE0ihTGyTCXDjjFjPDfiZiLlZjNR9CX82k8pdtFr2loSA%3d
     //                }
     //            }
     //        }
     //    }
     //    catch (Exception ex)
     //    {
     //        objErrorHandler.ErrorMsg = ex;
     //        objErrorHandler.CreateLog();
     //        sHTML = ex.Message;
     //    }




     //    return sHTML;
     //}
    //-------------------------------------------------------

     public string ST_Categories()
     {
         //stopwatch.Start();
         string sHTML = string.Empty;
         string sBrandAndModelHTML = string.Empty;
         string sModelListHTML = string.Empty;
         try
         {


             StringTemplateGroup _stg_container = null;
             StringTemplateGroup _stg_records = null;
             StringTemplate _stmpl_container = null;
             StringTemplate _stmpl_records = null;
             //  StringTemplate _stmpl_records1 = null;
             StringTemplate _stmpl_recordsrows = null;
             TBWDataList[] lstrecords = new TBWDataList[0];
             TBWDataList[] lstrows = new TBWDataList[0];

             // StringTemplateGroup _stg_container1 = null;
             //   StringTemplateGroup _stg_records1 = null;
             TBWDataList1[] lstrecords1 = new TBWDataList1[0];
             TBWDataList1[] lstrows1 = new TBWDataList1[0];
             int ictrows = 0;
             string _tsb = string.Empty;
             string _tsm = string.Empty;
             string _type = string.Empty;
             string _value = string.Empty;
             string _bname = string.Empty;
             string _searchstr = string.Empty;
             string _byp = "2";
             string _bypcat = null;


             string _pid = string.Empty;
             string _fid = string.Empty;
             string _seeall = string.Empty;
             _bypcat = HttpContext.Current.Request.QueryString["bypcat"];

             string Requrl = HttpContext.Current.Request.Url.ToString().ToLower();


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


             if (_catCid != string.Empty)
                 _parentCatID = GetParentCatID(_catCid);
             //if ((Requrl.Contains("pd.aspx")))
             //{
             //    if (HttpContext.Current.Session["Category_Attributes"] == null && _parentCatID != string.Empty)
             //    {
             //        EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
             //    }
             //}


             //Modified by indu
             //modified on 9-5-1013
             //For Metatag

             //if (HttpContext.Current.Request.QueryString["path"] != null)
             //    HttpContext.Current.Session["EA"] = objSecurity.StringDeCrypt(Request.QueryString["path"].ToString());

             if (HttpContext.Current.Session["MainCategory"] != null)
             {
                 DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
                 if (dr.Length > 0)
                     _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
             }

             //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true )
             //{
             //    if (_bypcat == null)
             //    {

             //        EasyAsk.GetMainMenuClickDetail(_catCid, "");


             //        string CatName = ""; 
             //        DataTable tmptbl=null;
             //        if (HttpContext.Current.Session["MainMenuClick"]!=null)
             //        {
             //            tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0] ;

             //             tmptbl = tmptbl.Select("CATEGORY_ID='" + _catCid + "'").CopyToDataTable();

             //            if (tmptbl != null && tmptbl.Rows.Count > 0)
             //            {                              
             //                CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
             //            }


             //        }           


             //        if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
             //            iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());

             //        EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

             //    }
             //    else if (_tsb != "")
             //    {
             //        string parentCatName = GetCName(_catCid);
             //        EasyAsk.GetWESModel(parentCatName, iCatalogId, _tsb);
             //    }

             //}
             ////Added By:Indu
             ////To get EA path with out attribyte Type

             //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true)
             //{
             //    int SubCatCount=0;
             //    if (Request.QueryString["type"] == null )
             //    {
             //        if (_tsb != null && _tsb != "" && _tsm != null && _tsm != null)
             //        {

             //            //string parentCatName = GetCName(ParentCatID);
             //            //EasyAsk.GetBrandAndModelProducts(parentCatName, _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
             //            if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
             //                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

             //            EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

             //        }
             //    }
             //    else
             //    {
             //        if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
             //            iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

             //        if (_type != "")
             //        {

             //            EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

             //        }
             //        else
             //        { //new open

             //            EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

             //        }
             //    }
             //}
             // if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true)
             //{
             //    if (Session["RECORDS_PER_PAGE_pl"] != null)
             //         iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_pl"].ToString());


             //     EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

             //}
             //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx") == true)
             //{
             //    if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
             //        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());

             //    EasyAsk.GetAttributeProducts("ps", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

             //}
             //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx") == true)
             //{
             //    if (Request.QueryString["type"] == null)
             //    {

             //        EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");

             //    }
             //    else
             //    {

             //        EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value, _bname, "0", "0", "");

             //    }
             //}
             //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)
             //{
             //    if (Request.QueryString["type"] == null)
             //    {

             //        EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");

             //    }
             //    else
             //    {

             //        EasyAsk.GetAttributeProducts("ProductPage", _searchstr, _type, _value, _bname, "0", "0", "");

             //    }
             //}
             //--------End
             //if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "" && (Request.QueryString["tsm"].ToString() != null && Request.QueryString["tsm"] != null))
             //{
             //    string category_nameh = "";
             //    DataSet tmp = GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'");
             //    if (tmp != null && tmp.Tables.Count >= 1 && tmp.Tables[0].Rows.Count >= 1)
             //    {

             //        category_nameh = tmp.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
             //    }

             //    EasyAsk.GetBrandAndModelProducts(category_nameh, Request.QueryString["tsm"].ToString(), Request.QueryString["tsb"].ToString(), iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
             //}

             if ((Requrl.Contains("ct.aspx")) || (Requrl.Contains("microsite.aspx")))
             {
                 if (_bypcat == null)
                 {
                     dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
                     DataTable result = null; //Declare a dataSet to be filled.

                     //Sort data
                     if (HttpContext.Current.Request.QueryString["cid"].ToString().ToUpper() == "WESNEWS")
                     {
                         try
                         {
                             dscat.Tables[0].DefaultView.Sort = "CUSTOM_TEXT_FIELD1 desc";

                             // Store in new Dataset
                             // result.Tables.Add(dscat.Tables[0].DefaultView.ToTable());
                             result = dscat.Tables[0].DefaultView.ToTable();
                             dscat.Tables.Remove("Category");
                             dscat.Tables.Add(result);
                         }
                         catch
                         {

                         }
                     }
                 }
                 else if (_tsb != string.Empty)
                 {
                     dscat = null;

                     //dscat = (DataSet)HttpContext.Current.Session["WESBrand_Model_Attributes"];

                 }

             }
             else if ((Requrl.Contains("pd.aspx")))
             {
                 //dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
                 //if (dscat == null)
                 //{
                 //    EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
                 //    dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
                 //}
                 dscat = null;

             }
             else
             {
                 dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
             }
             //Modified by Indu
             //Reason:To display category and subcategory drop down details in powersearch page
             if ((Requrl.Contains("ps.aspx")) && dscat != null && dscat.Tables.Count > 0)
             {
                 if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                 {
                     DataSet breadcrumbPS = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

                     if (breadcrumbPS != null && breadcrumbPS.Tables.Count > 0 && breadcrumbPS.Tables[0].Rows.Count == 1)
                     {
                         Session["LHScatPS"] = dscat;
                         Session["LHSsubcatPS"] = null;

                     }
                     else if (breadcrumbPS != null && breadcrumbPS.Tables.Count > 0 && breadcrumbPS.Tables[0].Rows.Count == 2 && breadcrumbPS.Tables[0].Rows[1]["ItemType"].ToString().ToLower().Contains("category"))
                     {
                         Session["LHSsubcatPS"] = dscat;
                         Session["subcatshow"] = "true";
                     }
                     else if (breadcrumbPS != null && breadcrumbPS.Tables.Count > 0 && breadcrumbPS.Tables[0].Rows.Count == 3 && breadcrumbPS.Tables[0].Rows[1]["ItemType"].ToString().ToLower().Contains("category") && breadcrumbPS.Tables[0].Rows[2]["ItemType"].ToString().ToLower().Contains("category"))
                     {
                         Session["LHSsubcatPS"] = (DataSet)HttpContext.Current.Session["LHSsubcatPS"];
                         // Session["LHSsubcatPS"] = dscat;
                         Session["subcatshow"] = "true";
                     }
                     else
                     {
                         // Session["LHSsubcatPS"] = null;  
                         Session["subcatshow"] = "false";
                     }

                 }
             }
             //End

             // modify by palani
             if ((Requrl.Contains("bb.aspx")) && dscat != null)
             {
                 if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                 {
                     DataSet breadcrumb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                     if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count == 3)
                     {
                         Session["dscatBrandModel"] = _tsb + "," + _tsm;
                         Session["dscatname"] = _value;
                         Session["dscatbybrand"] = dscat;
                         Session["dssubcatbybrand"] = null;
                     }
                     if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count == 4)
                     {
                         if (breadcrumb.Tables[0].Rows[3]["ItemType"].ToString() == "Category")
                             Session["dssubcatbybrand"] = dscat;
                     }
                 }


             }


             _stg_records = new StringTemplateGroup("searchrsltcategoryleftrecords", stemplatepath);
             _stg_container = new StringTemplateGroup("searchrsltcategoryleftmain", stemplatepath);

             if (!(Requrl.Contains("pd.aspx")))
             {
                 DataSet dsbc = null;
                 if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                 {
                     dsbc = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

                 }
                 string _BCEAPath = "//// //// ////";
                 for (int i = 0; i <= dsbc.Tables[0].Rows.Count - 1; i++)
                 {
                     string TBW_ITEM_NAME = string.Empty;
                     DataRow row = dsbc.Tables[0].Rows[i];

                     string rowItemType = string.Empty;

                     string rowItemValue = string.Empty;
                     string rowitemtypetostring = string.Empty;

                     rowItemType = row["ItemType"].ToString().ToLower();
                     rowItemValue = row["ItemValue"].ToString();
                     rowitemtypetostring = row["ItemType"].ToString();

                     //if (row["ItemType"].ToString().ToLower() == "family")
                     //    TBW_ITEM_NAME = row["FamilyName"].ToString();
                     //else if (row["ItemType"].ToString().ToLower() == "product")
                     //    TBW_ITEM_NAME = row["ProductCode"].ToString();
                     //else
                     //    TBW_ITEM_NAME = row["ItemValue"].ToString();

                     string _istsb = string.Empty;

                     if (rowItemType == "brand")
                     {

                         _istsb = rowItemValue;// row["ItemValue"].ToString();
                     }

                     if (rowItemType == "family")
                     {
                         TBW_ITEM_NAME = rowItemValue + "=" + row["FamilyName"];
                     }
                     //else if (row["ItemType"].ToString().ToLower() == "category" && i == 0)
                     //{
                     //    TBW_ITEM_NAME = hfcid.Value + "=" + row["ItemValue"].ToString();

                     //}
                     else if (rowItemType == "product")
                     {
                         if (row["ProductCode"].ToString() != string.Empty)
                         {
                             TBW_ITEM_NAME = rowItemValue + "=" + row["ProductCode"];
                         }
                         else
                         {
                             TBW_ITEM_NAME = rowitemtypetostring + "=" + rowItemValue;

                         }
                     }
                     //(row["ItemType"].ToString().ToLower() != "category") && 
                     else if ((rowItemType == "category" && i != 0) && (rowItemType != "model") && (rowItemType != "brand") && ((Requrl.Contains("bb.aspx"))))
                     {
                         TBW_ITEM_NAME = rowitemtypetostring + "=" + rowItemValue;

                     }

                     //else if ((row["ItemType"].ToString().ToLower() == "brand") || (row["ItemType"].ToString().ToLower() == "model") && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx") == true)||(HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)))
                     //{
                     //    TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                     //}
                     else if ((rowItemType != "category") && (rowItemType != "model") && ((Requrl.Contains("fl.aspx")) || (Requrl.Contains("pl.aspx")) || (Requrl.Contains("pd.aspx")) || (Requrl.Contains("ps.aspx")) && ((rowItemType != "user search") && !(rowItemType.Contains("usersearch")))))
                     {
                         TBW_ITEM_NAME = rowitemtypetostring + "=" + rowItemValue;

                     }

                     else if ((rowItemType != "brand") && (rowItemType != "category") && ((Requrl.Contains("ct.aspx")) || (Requrl.ToLower().Contains("microsite.aspx"))))
                     {
                         TBW_ITEM_NAME = rowitemtypetostring + "=" + rowItemValue;

                     }
                     else if ((rowItemType == "model") && ((Requrl.Contains("fl.aspx")) || (Requrl.Contains("pl.aspx")) || (Requrl.Contains("pd.aspx")) || (Requrl.Contains("ps.aspx"))))
                     {
                         if (_istsb != string.Empty)
                         {
                             TBW_ITEM_NAME = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("model = ", "model=").Replace("model = ", "model=").Replace("'", "").Replace(_istsb + ":", "");
                         }
                         else
                         {
                             TBW_ITEM_NAME = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("MODEL = ", "MODEL=").Replace("model = ", "model=").Replace("'", "");
                         }
                     }
                     else if ((rowItemType != "category") && (rowItemType != "user search") && (!(rowItemType.Contains("usersearch"))) && (rowItemType != "model") && (Requrl.Contains("ps.aspx") || Requrl.Contains("pl.aspx")))
                     {
                         TBW_ITEM_NAME = rowitemtypetostring + "=" + rowItemValue;
                     }
                     else if ((rowItemType == "user search") && (row["url"].ToString().ToLower().Contains("ps.aspx")) && (Requrl.Contains("fl.aspx")))
                     {
                         TBW_ITEM_NAME = "UserSearch" + "=" + rowItemValue;
                     }
                     else if (((rowItemType != "brand")) && ((rowItemType != "model")) && ((row["Url"].ToString().Contains("bb.aspx"))))
                     {

                         TBW_ITEM_NAME = rowitemtypetostring + "=" + rowItemValue;

                     }
                     else
                     {
                         TBW_ITEM_NAME = rowItemValue;
                     }
                     TBW_ITEM_NAME = HttpUtility.UrlEncode(TBW_ITEM_NAME);
                     //.Replace("+", "||").Replace("&", "^^").Replace(":", "~`");;

                     if (_BCEAPath == "//// //// ////")
                     {
                         _BCEAPath = _BCEAPath + TBW_ITEM_NAME;
                     }
                     else
                     {

                         _BCEAPath = _BCEAPath + "////" + TBW_ITEM_NAME;


                     }
                 }
                 HttpContext.Current.Session["breadcrumEAPATH"] = _BCEAPath;
            





             string stmplrecords = string.Empty;
             if ((Requrl.Contains("ct.aspx")) || (Requrl.Contains("microsite.aspx")))
             {
                 stmplrecords = "searchrsltcategoryleft" + "\\" + "cell";
             }

             else if ((Requrl.Contains("bb.aspx")))
             {
                 stmplrecords = "searchrsltcategoryleft" + "\\" + "cell1";
             }
             else if ((Requrl.Contains("ps.aspx")))
             {
                 stmplrecords = "searchrsltcategoryleft" + "\\" + "cell2";
             }
             else if ((Requrl.Contains("fl.aspx")))
             {
                 stmplrecords = "searchrsltcategoryleft" + "\\" + "cell3";
             }
             else if ((Requrl.Contains("pl.aspx")))
             {
                 stmplrecords = "searchrsltcategoryleft" + "\\" + "cell4";
             }
             else
             {
                 stmplrecords = "searchrsltcategoryleft" + "\\" + "cell";
             }
             if (dscat != null)
             {

                 if (dscat.Tables.Count > 0)
                     lstrows = new TBWDataList[dscat.Tables.Count + 1];

                 for (int i = 0; i < dscat.Tables.Count; i++)
                 {
                     Boolean tmpallow = true;
                     if ((Requrl.Contains("ct.aspx")) || (Requrl.Contains("microsite.aspx")) || (Requrl.Contains("pd.aspx")))
                     {
                         if (dscat.Tables[i].TableName.Contains("Category"))
                             tmpallow = true;
                         else if (dscat.Tables[i].TableName.Contains("Brand"))
                             tmpallow = false;
                         else if (HttpContext.Current.Request.QueryString["byp"] == "2")
                             tmpallow = true;
                         else
                             tmpallow = false;
                     }
                     if (tmpallow == true)
                     {
                         if (dscat.Tables[i].Rows.Count > 0)
                         {
                             lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];
                             lstrecords1 = new TBWDataList1[dscat.Tables[i].Rows.Count + 1];
                             int ictrecords = 0;

                             int j = 0;
                             string TBW_ATTRIBUTE_VALUE = string.Empty;




                             string brandvalue = string.Empty;
                            

                             foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
                             {
                                 _stmpl_records = _stg_records.GetInstanceOf(stmplrecords);

                                 string brandname = string.Empty;
                                 if (dscat.Tables[i].TableName.Contains("Category"))
                                 {
                                     _stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                     _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                     //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));

                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr["Category_Name"]);
                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
                                     TBW_ATTRIBUTE_VALUE = dr["Category_Name"].ToString();
                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                     brandname = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_BRAND").ToString();
                                     if (brandname != "")
                                     {
                                         brandname = brandname + ":";
                                     }
                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));

                                     if (_parentCatID == WesNewsCategoryId)
                                         _stmpl_records.SetAttribute("TBW_OPTION_CATEGORY_ID", "<br/>" + dr["CATEGORY_ID"].ToString());
                                     else
                                         _stmpl_records.SetAttribute("TBT_CATEGORY_ID_DIV", "");
                                 }
                                

                                 else
                                 {
                                     //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                                     //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                     _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_catCid.ToString()));
                                   
                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr[0].ToString()));
                                     TBW_ATTRIBUTE_VALUE = dr[0].ToString();
                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                     brandname = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_BRAND").ToString();
                                     if (brandname != string.Empty)
                                     {
                                         brandname = brandname + ":";
                                     }

                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));


                                 }


                                 _stmpl_records.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
                                 _stmpl_records.SetAttribute("TBW_BRAND", HttpUtility.UrlEncode(_tsb));
                                 _stmpl_records.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(_tsm));
                                 _stmpl_records.SetAttribute("TBW_FAMILY_ID", _fid);

                                 _stmpl_records.SetAttribute("TBW_ATTRIBUTE_TYPE", dscat.Tables[i].TableName.ToString());



                                 if (HttpContext.Current.Session["EA"] != null)
                                 {
                                     if (HttpContext.Current.Session["EA"].ToString() != "AllProducts////WESAUSTRALASIA" || HttpContext.Current.Session["EA_URL"] == null)
                                     {
                                         _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));

                                         _stmpl_records.SetAttribute("ORG_EA_PATH", HttpContext.Current.Session["EA"].ToString());
                                     }
                                     else if (HttpContext.Current.Session["EA"].ToString() == "AllProducts////WESAUSTRALASIA" && HttpContext.Current.Session["EA_URL"] != null)
                                     {

                                         _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA_URL"].ToString())));

                                         _stmpl_records.SetAttribute("ORG_EA_PATH", HttpContext.Current.Session["EA_URL"].ToString());
                                     }

                                 }

                                 string TBW_ATTRIBUTE_TYPE = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_TYPE").ToString().ToUpper();
                                 string TBW_ATTRIBUTE_VALUE_NEW = dr[0].ToString();
                                 _stmpl_records.SetAttribute("TBW_ATTRIBUTE_INPUT", dr[0].ToString().Replace("$", "").Replace(" ", ""));


                                 if (_tsm != string.Empty)
                                 {
                                     _BCEAPath = _BCEAPath.Replace(_tsm, HttpUtility.UrlEncode(_tsm));
                                 }


                                 string TBW_ATTRIBUTE_NAME_NEW = TBW_ATTRIBUTE_VALUE;
                                 if (TBW_ATTRIBUTE_TYPE == "MODEL")


                                 {

                                     if (brandname != "")
                                     {
                                         TBW_ATTRIBUTE_NAME_NEW = brandname + ":" + TBW_ATTRIBUTE_VALUE_NEW.Replace(brandname, "");
                                     }
                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr[0].ToString().Replace(brandname,""));
                                 }
                                 else if(!dscat.Tables[i].TableName.Contains("Category"))
                                 {
                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr[0].ToString());
                                 }

                                 _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME_NEW", TBW_ATTRIBUTE_NAME_NEW);

                                 objHelperServices.SimpleURL(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_VALUE, "");
                                 //if ((TBW_ATTRIBUTE_TYPE.ToUpper() == "CATEGORY") && (Requrl.Contains("bb.aspx")))
                                 //{
                                 //    objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" +TBW_ATTRIBUTE_VALUE, "bb.aspx", true, TBW_ATTRIBUTE_TYPE);
                                 //}

                                 //else if (TBW_ATTRIBUTE_TYPE.ToUpper() != "CATEGORY" )
                                 //{

                                 //    if((_BCEAPath.ToLower().Contains("brand")) )
                                 //    {
                                 //    objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + TBW_ATTRIBUTE_VALUE, "pl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                 //    }
                                 //    else
                                 //    {
                                 //    objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + brandname + TBW_ATTRIBUTE_VALUE, "pl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                 //    }
                                 //    }
                                 //else
                                 //{
                                 //    objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_VALUE, "pl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                 //}
                                 //  objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + _stmpl_records.GetAttribute("TBW_ATTRIBUTE_NAME"), "pl.aspx", true, "");


                                 if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)
                                 {
                                     lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                 }
                                 else
                                 {
                                     if ((dscat.Tables[i].TableName.Contains("Category")))
                                     {

                                         lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                     }
                                     else
                                     {
                                         if (ictrecords <= 4)
                                         {
                                             lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                         }
                                         else
                                         {
                                             lstrecords1[ictrecords] = new TBWDataList1(_stmpl_records.ToString());
                                         }
                                     }

                                 }
                                 ictrecords++;
                             }

                             j++;
                             //if (dscat_full.Tables[i].Rows.Count > 0)
                             //{
                             //    _stmpl_recordsrows.SetAttribute("TBW_LINK", "<h3 class=expand id='" + dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", "") + "1' onclick=showHide('" + dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", "") + "');return false;>Show More Options</h3>");
                             //}
                             if ((Requrl.Contains("ct.aspx")) || (Requrl.Contains("microsite.aspx")) || (Requrl.Contains("pd.aspx")))
                                 _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
                             else
                             {
                                 if ((dscat.Tables[i].TableName.Contains("Category")))
                                 {
                                     _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
                                 }
                                 else
                                 {
                                     if (dscat.Tables[i].Rows.Count > 5)
                                         _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row");
                                     else
                                         _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
                                 }
                             }

                             //}
                             //else
                             //{
                             //    _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
                             //}
                             _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", dscat.Tables[i].TableName.ToString());
                             _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_ID", dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", ""));
                             _stmpl_recordsrows.SetAttribute("TBT_MENU_ID", (i + 1).ToString());
                             _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);
                             _stmpl_recordsrows.SetAttribute("TBWDataList1", lstrecords1);
                             lstrows[ictrows] = new TBWDataList(_stmpl_recordsrows.ToString());
                             ictrows++;
                         }
                     }
                 }
             }
           }

             // You Have Select
             //DataSet ds = new DataSet();
             //int cnt = 0;
             //ds = null;
             //if (HttpContext.Current.Session["BreadCrumbDS"] != null)
             //{
             //    ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

             //}
             //string YHSCell = "searchrsltcategoryleft" + "\\" + "YHSCell";
             //if (ds != null && ds.Tables[0].Rows.Count > 0)
             //{
             //    lstrows1 = new TBWDataList1[ds.Tables[0].Rows.Count + 1];

             //    //foreach (DataRow row in ds.Tables[0].Rows)
             //    //{
             //    string breadcrumEA = "//// //// ////";
             //    string RemovebreadcrumEA = "//// //// ////";
             //    bool IsFromPowersearch = false;
             //    bool IsFromPowersearch_RC = false;

             //    bool IsFromBrand = false;

             //    string istsm = string.Empty;
             //    string istsb = string.Empty;
             //    bool familybrandmodelset = false;
             //    string Familyname = string.Empty;
             //    string fid = string.Empty;
             //    int a = 0;
             //    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
             //    {
             //        DataRow row = ds.Tables[0].Rows[i];
             //        DataRow Revrow;
             //        string Itemvalue = string.Empty;
             //        string newpagename = string.Empty;
             //        string newremovepagename = string.Empty;
             //        string rowitype = string.Empty;
             //        rowitype = row["ItemType"].ToString().ToLower();
             //        string ritemvalue = string.Empty;
             //        ritemvalue = row["ItemValue"].ToString();

             //        string ritemtypetostring = string.Empty;
             //        ritemtypetostring = row["ItemType"].ToString();
             //        if (i == 0)
             //        {
             //            Revrow = ds.Tables[0].Rows[0];
             //        }
             //        else
             //        {
             //            Revrow = ds.Tables[0].Rows[i - 1];
             //        }
             //        _stmpl_records = _stg_records.GetInstanceOf(YHSCell);

             //        _stmpl_records.SetAttribute("TBW_ITEM_TYPE", "Item " + ritemtypetostring);

             //        string[] PAGENAME = row["Url"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);

             //        string pagename = string.Empty;
             //        pagename = PAGENAME[0].ToLower();

             //        if (rowitype == "category" && i == 0)
             //        {
             //            _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
             //            Itemvalue = HttpUtility.UrlEncode(ritemvalue);
             //        }
             //        else if (rowitype == "family")
             //        {
             //            _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["FamilyName"]);
             //            Itemvalue = ritemvalue + "=" + row["FamilyName"];
             //            HttpContext.Current.Session["S_FName"] = row["FamilyName"];
             //        }
             //        //else if (rowitype == "brand" && (pagename.Contains("ct.aspx") || pagename.Contains("microsite.aspx")))
             //        //{
             //        //    _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
             //        //    Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);
             //        //}

             //        else if (rowitype == "product")
             //        {
             //            if (row["ProductCode"].ToString() != "")
             //            {
             //                _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ProductCode"]);
             //                Itemvalue = ritemvalue + "=" + row["ProductCode"];
             //            }
             //            else
             //            {
             //                _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
             //                Itemvalue = ritemvalue + "=" + HttpUtility.UrlEncode(ritemvalue);
             //            }
             //        }
             //        // else if ((rowitype != "category")
             //        //     && (rowitype != "model")
             //        //     && (rowitype != "brand") 
             //        //     && (IsFromBrand == true)
             //        //     && ((Requrl.Contains("fl.aspx")) || Requrl.Contains("pd.aspx")))
             //        // {
             //        //     _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
             //        //     Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);

             //       // }
             //        // else if (((rowitype != "brand")
             //        //    && (rowitype != "model")
             //        //     && (rowitype != "category") 
             //        //     )
             //        //    && ((Requrl.Contains("ct.aspx"))
             //        //     || (Requrl.Contains("microsite.aspx"))
             //        //    || (Requrl.Contains("ps.aspx"))

             //       //    || (Requrl.Contains("fl.aspx"))
             //        //    || (Requrl.Contains("pd.aspx"))))
             //        //{
             //        //    _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
             //        //    Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);

             //       //}
             //        // else if ((rowitype == "brand") 
             //        //     && (!(IsFromBrand))
             //        //     && (!(Requrl.Contains("ct.aspx")))
             //        //     && (!(Requrl.Contains("microsite.aspx"))))
             //        //     //(row["ItemType"].ToString().ToLower() != "model")) && 
             //        //     //((HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true) || 
             //        //     //(HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx")) ||
             //        //     //(HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) || 
             //        //     //(HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx"))))
             //        // // else if (((row["ItemType"].ToString().ToLower() != "brand")) && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true) && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")))
             //        // {
             //        //     _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
             //        //     Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);

             //       // }

             //       // else if ((rowitype != "category")
             //        //     && (rowitype != "model")
             //        //     && (rowitype != "user search")
             //        //     && (!(rowitype.Contains("usersearch"))) 
             //        //     && (IsFromproductlist == true || IsFromPowersearch == true))
             //        // {
             //        //     _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
             //        //     Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);
             //        // }
             //        // else if ((rowitype == "model") 
             //        //     && (IsFromBrand == false))
             //        // {

             //       //     _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
             //        //     Itemvalue = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("model = ", "model=").Replace("'", "");

             //       //     Itemvalue = HttpUtility.UrlEncode(Itemvalue);
             //        //     Itemvalue = Itemvalue.Trim();
             //        // }
             //        // else if (((rowitype != "brand")) && ((rowitype != "model")) && ((row["Url"].ToString().Contains("bb.aspx"))))
             //        // {
             //        //     _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
             //        //     Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);

             //       // }
             //        else
             //        {
             //            _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
             //            Itemvalue = HttpUtility.UrlEncode(ritemvalue);
             //        }
             //        //Itemvalue = Itemvalue;
             //        //.Replace("+", "||").Replace("&", "^^").Replace(":", "~`");; ;

             //        try
             //        {
             //            string itemtitle = _stmpl_records.GetAttribute("TBW_ITEM_NAME").ToString();
             //            _stmpl_records.SetAttribute("TBW_ITEM_Title", itemtitle.Replace('"', ' '));
             //        }
             //        catch
             //        { }
             //        _stmpl_records.SetAttribute("TBT_REMOVEEAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["RemoveEAPath"].ToString())));
             //        _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"]);
             //        _stmpl_records.SetAttribute("TBT_URL", row["Url"]);
             //        _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["EAPath"].ToString())));
             //        _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", ritemtypetostring);


             //        //modified by :indu
             //        try
             //        {


             //            //TBWTemplateEngine objTempengine = new TBWTemplateEngine("", "", "");
             //            string Itemtype = row["ItemType"].ToString().ToUpper();
             //            string RevItemType = Revrow["ItemType"].ToString().ToUpper();

             //            _stmpl_records.SetAttribute("TBT_REM_ATTRIBUTE_TYPE", RevItemType);
             //            string[] catpath = ds.Tables[0].Rows[i]["EAPATH"].ToString().ToUpper().Split(new string[] { "////" }, StringSplitOptions.None);
             //            newpagename = PAGENAME[0].ToLower();
             //            if (breadcrumEA == "//// //// ////")
             //            {

             //                RemovebreadcrumEA = breadcrumEA;
             //                breadcrumEA = breadcrumEA + Itemvalue;

             //            }
             //            else
             //            {

             //                RemovebreadcrumEA = breadcrumEA;
             //                breadcrumEA = breadcrumEA + "////" + ds.Tables[0].Rows[i]["Itemvalue"].ToString();

             //            }
             //            newpagename = PAGENAME[0].ToLower();
             //            if ((pagename.Contains("pl.aspx")) && (catpath.Length >= 5))
             //            {

             //                a = a + 1;

             //                //breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + catpath[1] + "////" + "wa-" + a + "////" + catpath[2] + "////" + catpath[3];

             //                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
             //                    (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" +
             //                    (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
             //                       (catpath.Length >= 4 ? catpath[3] : " ");
             //                HttpContext.Current.Session[(catpath.Length >= 4 ? catpath[3] : " ") + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();


             //            }

             //            else if (pagename.Contains("pd.aspx"))
             //            {

             //                //if ((IsFromps))
             //                //{
             //                //    breadcrumEA = breadcrumEA.ToUpper().Replace("USERSEARCH1=", "USERSEARCH=").Replace("USER_SEARCH=", "USERSEARCH=");
             //                //    breadcrumEA = breadcrumEA.Replace("USERSEARCH1=USERSEARCH1=", "USERSEARCH=").Replace("USER_SEARCH=USER_SEARCH=", "USERSEARCH=");
             //                //}


             //                if (catpath.Length == 7)
             //                {

             //                    //breadcrumEA = catpath[0]+ "////" + catpath[1] + "////" +
             //                    //     catpath[5].Replace("USERSEARCH=FAMILY ID=", "") + "////" +
             //                    //      catpath[6].Replace("USERSEARCH1=PROD ID=", "") + "=" +
             //                    //      ds.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper() +
             //                    //        "////" + catpath[2] + "////" + catpath[3] + "////" +
             //                    //            Familyname;
             //                    breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
             //                       (catpath.Length >= 2 ? catpath[1] : " ") + "////" +
             //                       (catpath.Length >= 6 ? catpath[5] : " ").Replace("USERSEARCH=FAMILY ID=", "") + "////" +
             //                          (catpath.Length >= 7 ? catpath[6] : " ").Replace("USERSEARCH1=PROD ID=", "") + "=" +
             //                         ds.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper() +
             //                           "////" + (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
             //                          (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
             //                               Familyname;
             //                }
             //                else
             //                {

             //                    //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" +
             //                    //    catpath[4].Replace("USERSEARCH=FAMILY ID=", "")
             //                    //    + "////" +
             //                    //     catpath[5].Replace("USERSEARCH1=PROD ID=", "") + "=" + ds.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper()
             //                    //     + "////" + catpath[2] + "////" +
             //                    //    catpath[3] + "////" +
             //                    //             Familyname;

             //                    breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
             //                        (catpath.Length >= 2 ? catpath[1] : " ") + "////" +
             //                      (catpath.Length >= 5 ? catpath[4] : " ").Replace("USERSEARCH=FAMILY ID=", "")
             //                      + "////" +
             //                       (catpath.Length >= 6 ? catpath[5] : " ").Replace("USERSEARCH1=PROD ID=", "") + "=" + ds.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper()
             //                       + "////" + (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
             //                     (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
             //                               Familyname;
             //                }
             //            }
             //            else if (pagename.Contains("fl.aspx"))
             //            {
             //                // string[] catpath = ds.Tables[0].Rows[i] ["EAPATH"].ToString().ToUpper().Split(new string[] { "////" }, StringSplitOptions.None);



             //                if (Itemtype == "FAMILY")
             //                {

             //                    Familyname = ds.Tables[0].Rows[i]["FAMILYNAME"].ToString();
             //                    _fid = ds.Tables[0].Rows[i]["Itemvalue"].ToString();

             //                    //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" + _fid + "////" + catpath[2] + "////" + catpath[3] + "////" +
             //                    //        Familyname;
             //                    breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////"
             //                        + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + _fid + "////" +
             //                        (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
             //                       (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
             //                           Familyname;

             //                }
             //                else
             //                {

             //                    a = a + 1;
             //                    //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" + "wa-" + a + "////" + _fid + "////" + catpath[2] + "////" + catpath[3] + "////" +
             //                    //          Familyname;
             //                    breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" + _fid + "////"
             //                        + (catpath.Length >= 3 ? catpath[2] : " ") + "////" + (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
             //                             Familyname;
             //                    HttpContext.Current.Session[_fid + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();

             //                }


             //            }

             //            else if (pagename.Contains("ct.aspx"))
             //            {

             //                //breadcrumEA = breadcrumEA.Replace("Brand=", "");
             //                if (Itemtype.ToLower() == "brand")
             //                {
             //                    istsb = ds.Tables[0].Rows[i]["Itemvalue"].ToString();

             //                }

             //            }
             //            else if (pagename.Contains("bb.aspx"))
             //            {

             //                if (catpath.Length <= 5)
             //                {
             //                    IsFromBrand = true;
             //                    if (Itemtype.ToLower() == "model")
             //                    {
             //                        istsm = ds.Tables[0].Rows[i]["Itemvalue"].ToString().ToLower().Replace("model=", "");
             //                    }
             //                    breadcrumEA = breadcrumEA.ToUpper().Replace("MODEL=", "").Replace("BRAND=", "");
             //                }
             //                else
             //                {
             //                    a = a + 1;
             //                    //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" + "wa-" + a + "////" + catpath[2] + "////" + istsb + "////" + istsm;
             //                    breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" +
             //                        (catpath.Length >= 3 ? catpath[2] : " ") + "////" + istsb + "////" + istsm;


             //                    HttpContext.Current.Session[istsm + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();



             //                }
             //            }
             //            else if ((pagename.Contains("ps.aspx")) || pagename == "")
             //            {

             //                if (catpath.Length > 3)
             //                {

             //                    a = a + 1;
             //                    //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" + "wa-" + a + "////" + ds.Tables[0].Rows[0]["ItemValue"].ToString();

             //                    breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
             //                         (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" + ds.Tables[0].Rows[0]["ItemValue"].ToString();

             //                    HttpContext.Current.Session[ds.Tables[0].Rows[0]["ItemValue"].ToString() + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();

             //                }

             //                // IsFromps = true;


             //                breadcrumEA = breadcrumEA.ToUpper().Replace("USER SEARCH=", "").Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "").Replace("USERSEARCH2=", "").Replace("CATEGORY=", "");
             //                //  breadcrumEA = breadcrumEA.Replace("//// //// ////", "//// //// ////" + "UserSearch=");
             //            }


             //            //  breadcrumEA = breadcrumEA.Replace("+", "||"); 
             //            //Cons_NewURl(_stmpl_records, breadcrumEA, PAGENAME[0].ToLower(), true, false);                     
             //            if (newpagename == "")
             //            {
             //                newpagename = "ps.aspx";
             //            }
             //            objHelperServices.SimpleURL(_stmpl_records, breadcrumEA, "BC" + "-" + newpagename);
             //            string[] REMOVEPAGENAME = row["RemoveUrl"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
             //            string Rpagename = string.Empty;
             //            Rpagename = REMOVEPAGENAME[0].ToLower();
             //            if (Rpagename.Contains("pl.aspx"))
             //            {

             //                newremovepagename = REMOVEPAGENAME[0].ToLower();

             //            }
             //            else if (Rpagename.Contains("pd.aspx"))
             //            {
             //                newremovepagename = REMOVEPAGENAME[0].ToLower();
             //                if (IsFromPowersearch_RC == true)
             //                {
             //                    RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USERSEARCH1=", "USERSEARCH=");
             //                }
             //            }
             //            else if (Rpagename.Contains("fl.aspx"))
             //            {
             //                newremovepagename = REMOVEPAGENAME[0].ToLower().Replace("fl.aspx", "fl");
             //                if ((IsFromPowersearch) && (!(RemovebreadcrumEA.Contains("USERSEARCH1="))))
             //                {
             //                    RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("//// //// ////", "//// //// ////" + "USERSEARCH1=");
             //                }
             //                else
             //                {
             //                    if ((IsFromBrand == true) && (istsb != string.Empty) && (istsm != string.Empty) && (familybrandmodelset == false))
             //                    {
             //                        RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace(istsm.ToUpper(), "MODEL=" + istsm.ToUpper()).Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
             //                        familybrandmodelset = true;
             //                    }

             //                }
             //            }
             //            else if (Rpagename.Contains("ct.aspx") || Rpagename.Contains("microsite.aspx"))
             //            {
             //                newremovepagename = REMOVEPAGENAME[0].ToLower();
             //                //RemovebreadcrumEA = RemovebreadcrumEA;

             //            }
             //            else if (Rpagename.Contains("bb.aspx"))
             //            {
             //                newremovepagename = REMOVEPAGENAME[0].ToLower();
             //                RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("MODEL=", "");

             //            }
             //            else if (Rpagename.Contains("ps.aspx"))
             //            {
             //                newremovepagename = REMOVEPAGENAME[0].ToLower();
             //                IsFromPowersearch_RC = true;
             //                RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USER SEARCH=", "").Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "").Replace("USERSEARCH2=", "").Replace("CATEGORY=", "");
             //            }

             //            else
             //            {
             //                newremovepagename = REMOVEPAGENAME[0].ToLower();

             //            }
             //            //added by:indu
             //            RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USERSEARCH1 = USERSEARCH =", "USERSEARCH =").Replace("USERSEARCH1=USERSEARCH1=", "USERSEARCH1=")
             //                .Replace("MODEL=MODEL=", "MODEL=").Replace("BRAND=BRAND=", "BRAND=");
             //            // Cons_NewURl(_stmpl_records, breadcrumEA, REMOVEPAGENAME[0].ToLower(), true, true);
             //            // RemovebreadcrumEA = RemovebreadcrumEA.Replace("+", "||"); 
             //            if (newremovepagename == "")
             //            {
             //                newremovepagename = "ps.aspx";
             //            }
             //            objHelperServices.SimpleURL(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + newremovepagename.ToLower());


             //        }
             //        catch (Exception ex)
             //        {
             //            objErrorHandler.ErrorMsg = ex;
             //            objErrorHandler.CreateLog();
             //        }


             //        lstrows1[cnt] = new TBWDataList1(_stmpl_records.ToString());
             //        cnt = cnt + 1;

             //    } //


             //}
             lstrows1=YHSCell_Bind();
             string reqorgquerystring = string.Empty;
             reqorgquerystring = Request.Url.OriginalString.ToLower();

             if (reqorgquerystring.Contains("ct.aspx") || reqorgquerystring.Contains("microsite.aspx"))
             {
                 //if (_parentCatID != WesNewsCategoryId && _byp == "2") // Not WES News
                 if (_parentCatID != string.Empty && _byp == "2")
                     sBrandAndModelHTML = ST_bybrand();

             }
             if ((reqorgquerystring.Contains("ct.aspx") || reqorgquerystring.Contains("microsite.aspx")) && _tsb != string.Empty && _bypcat != null)
             {
                 //if (_parentCatID != WesNewsCategoryId && _byp == "2") // Not WES News

                 if (_parentCatID != string.Empty && _byp == "2")
                     sModelListHTML = ST_BrandAndModel();
             }



             // You Have Select
             _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "main");
             //_stmpl_container.SetAttribute("Selection", updateNavigation());
             _stmpl_container.SetAttribute("TBWDataList", lstrows);
             _stmpl_container.SetAttribute("BRAND_AND_MODEL_HTML", sBrandAndModelHTML);
             _stmpl_container.SetAttribute("MODEL_HTML", sModelListHTML);
             _stmpl_container.SetAttribute("TBWDataList1", lstrows1);  //youer current Selection  
             sHTML += _stmpl_container.ToString();

             if (Requrl.Contains("pd.aspx") == true || Requrl.Contains("fl.aspx") == true)
                 sHTML = "";
            // stopwatch.Stop();
            // objErrorHandler.CreateLog("Stcategories-" + stopwatch.Elapsed);
         }

         catch (Exception ex)
         {
             sHTML = ex.Message;
         }
         finally
         {

         }



         return objHelperServices.StripWhitespace(sHTML);

     }


     private TBWDataList1[]  YHSCell_Bind()
     {

         try
         {

             StringTemplateGroup _stg_container = null;
             StringTemplateGroup _stg_records = null;
             StringTemplate _stmpl_container = null;
             StringTemplate _stmpl_records = null;
             TBWDataList1[] lstrecords1 = new TBWDataList1[0];
             TBWDataList1[] lstrows1 = new TBWDataList1[0];
             DataSet ds = new DataSet();
             int cnt = 0;
             ds = null;
             if (HttpContext.Current.Session["BreadCrumbDS"] != null)
             {
                 ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

             }
             _stg_records = new StringTemplateGroup("searchrsltcategoryleftrecords", stemplatepath);
             _stg_container = new StringTemplateGroup("searchrsltcategoryleftmain", stemplatepath);
             string YHSCell = "searchrsltcategoryleft" + "\\" + "YHSCell";
             if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
             {
                 DataTable dtconsbc = new DataTable();
                 dtconsbc.Columns.Add("TBW_ITEM_NAME");
                 dtconsbc.Columns.Add("TBW_ITEM_TYPE");
                 dtconsbc.Columns.Add("TBT_REWRITEURL");
                 dtconsbc.Columns.Add("TBT_REMOVEREWRITEURL");
                 dtconsbc.Columns.Add("lastvalue");
                     



                 lstrows1 = new TBWDataList1[ds.Tables[0].Rows.Count + 1];


                 string breadcrumEA = "//// //// ////";
                 string RemovebreadcrumEA = "//// //// ////";
                 bool IsFromPowersearch = false;
                 bool IsFromPowersearch_RC = false;

                 bool IsFromBrand = false;

                 string istsm = string.Empty;
                 string istsb = string.Empty;
                 bool familybrandmodelset = false;
                 string Familyname = string.Empty;
                 string _fid = string.Empty;
                 int a = 0;
                 for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                 {
                     DataRow drconsbc = dtconsbc.NewRow();
                     DataRow row = ds.Tables[0].Rows[i];
                     DataRow Revrow;
                     string Itemvalue = string.Empty;
                     string newpagename = string.Empty;
                     string newremovepagename = string.Empty;
                     string rowitype = string.Empty;
                     rowitype = row["ItemType"].ToString().ToLower();
                     string ritemvalue = string.Empty;
                     ritemvalue = row["ItemValue"].ToString();

                     string ritemtypetostring = string.Empty;
                     ritemtypetostring = row["ItemType"].ToString();
                     if (i == 0)
                     {
                         Revrow = ds.Tables[0].Rows[0];
                     }
                     else
                     {
                         Revrow = ds.Tables[0].Rows[i - 1];
                     }
                     _stmpl_records = _stg_records.GetInstanceOf(YHSCell);

                     _stmpl_records.SetAttribute("TBW_ITEM_TYPE", "Item " + ritemtypetostring);
                     drconsbc["TBW_ITEM_TYPE"] = "Item " + ritemtypetostring;

                     string[] PAGENAME = row["Url"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);

                     string pagename = string.Empty;
                     pagename = PAGENAME[0].ToLower();

                     if (rowitype == "category" && i == 0)
                     {
                         _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                         drconsbc["TBW_ITEM_NAME"] = ritemvalue;
                         Itemvalue = HttpUtility.UrlEncode(ritemvalue);
                     }
                     else if (rowitype == "family")
                     {
                         _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["FamilyName"]);
                         drconsbc["TBW_ITEM_NAME"] = row["FamilyName"];
                         Itemvalue = ritemvalue + "=" + row["FamilyName"];
                         //HttpContext.Current.Session["S_FName"] = row["FamilyName"];
                     }

                     else if (rowitype == "product")
                     {
                         if (row["ProductCode"].ToString() != "")
                         {
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ProductCode"]);
                             drconsbc["TBW_ITEM_NAME"] = row["ProductCode"];
                             Itemvalue = ritemvalue + "=" + row["ProductCode"];
                         }
                         else
                         {
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                             drconsbc["TBW_ITEM_NAME"] = ritemvalue;
                             Itemvalue = ritemvalue + "=" + HttpUtility.UrlEncode(ritemvalue);
                         }
                     }

                     else
                     {
                         _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                         drconsbc["TBW_ITEM_NAME"] = ritemvalue;
                         Itemvalue = HttpUtility.UrlEncode(ritemvalue);
                     }

                     try
                     {
                         string itemtitle = _stmpl_records.GetAttribute("TBW_ITEM_NAME").ToString();

                     }
                     catch
                     { }

                     _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"]);

                     _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", ritemtypetostring);


                     //modified by :indu
                     try
                     {


                         //TBWTemplateEngine objTempengine = new TBWTemplateEngine("", "", "");
                         string Itemtype = row["ItemType"].ToString().ToUpper();
                         string RevItemType = Revrow["ItemType"].ToString().ToUpper();

                         _stmpl_records.SetAttribute("TBT_REM_ATTRIBUTE_TYPE", RevItemType);
                         string[] catpath = ds.Tables[0].Rows[i]["EAPATH"].ToString().ToUpper().Split(new string[] { "////" }, StringSplitOptions.None);
                         newpagename = PAGENAME[0].ToLower();
                         if (breadcrumEA == "//// //// ////")
                         {

                             RemovebreadcrumEA = breadcrumEA;
                             breadcrumEA = breadcrumEA + Itemvalue;

                         }
                         else
                         {

                             RemovebreadcrumEA = breadcrumEA;
                             breadcrumEA = breadcrumEA + "////" + ds.Tables[0].Rows[i]["Itemvalue"].ToString();

                         }
                         newpagename = PAGENAME[0].ToLower();
                         if ((pagename.Contains("pl.aspx")) && (catpath.Length >= 5))
                         {

                             a = a + 1;



                             breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
                                 (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" +
                                 (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
                                    (catpath.Length >= 4 ? catpath[3] : " ");
                             HttpContext.Current.Session[(catpath.Length >= 4 ? catpath[3] : " ") + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();


                         }

                         else if (pagename.Contains("pd.aspx"))
                         {




                             if (catpath.Length == 7)
                             {


                                 breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
                                    (catpath.Length >= 2 ? catpath[1] : " ") + "////" +
                                    (catpath.Length >= 6 ? catpath[5] : " ").Replace("USERSEARCH=FAMILY ID=", "") + "////" +
                                       (catpath.Length >= 7 ? catpath[6] : " ").Replace("USERSEARCH1=PROD ID=", "") + "=" +
                                      ds.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper() +
                                        "////" + (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
                                       (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
                                            Familyname;
                             }
                             else
                             {



                                 breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
                                     (catpath.Length >= 2 ? catpath[1] : " ") + "////" +
                                   (catpath.Length >= 5 ? catpath[4] : " ").Replace("USERSEARCH=FAMILY ID=", "")
                                   + "////" +
                                    (catpath.Length >= 6 ? catpath[5] : " ").Replace("USERSEARCH1=PROD ID=", "") + "=" + ds.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper()
                                    + "////" + (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
                                  (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
                                            Familyname;
                             }
                         }
                         else if (pagename.Contains("fl.aspx"))
                         {



                             if (Itemtype == "FAMILY")
                             {

                                 Familyname = ds.Tables[0].Rows[i]["FAMILYNAME"].ToString();
                                 _fid = ds.Tables[0].Rows[i]["Itemvalue"].ToString();


                                 breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////"
                                     + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + _fid + "////" +
                                     (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
                                    (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
                                        Familyname;

                             }
                             else
                             {

                                 a = a + 1;

                                 breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" + _fid + "////"
                                     + (catpath.Length >= 3 ? catpath[2] : " ") + "////" + (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
                                          Familyname;
                                 HttpContext.Current.Session[_fid + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();

                             }


                         }

                         else if (pagename.Contains("ct.aspx"))
                         {


                             if (Itemtype.ToLower() == "brand")
                             {
                                 istsb = ds.Tables[0].Rows[i]["Itemvalue"].ToString();

                             }

                         }
                         else if (pagename.Contains("bb.aspx"))
                         {

                             if (catpath.Length <= 5)
                             {
                                 IsFromBrand = true;
                                 if (Itemtype.ToLower() == "model")
                                 {
                                     istsm = ds.Tables[0].Rows[i]["Itemvalue"].ToString().ToLower().Replace("model=", "");
                                 }
                                 breadcrumEA = breadcrumEA.ToUpper().Replace("MODEL=", "").Replace("BRAND=", "");
                             }
                             else
                             {
                                 a = a + 1;

                                 breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" +
                                     (catpath.Length >= 3 ? catpath[2] : " ") + "////" + istsb + "////" + istsm;


                                 HttpContext.Current.Session[istsm + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();



                             }
                         }
                         else if ((pagename.Contains("ps.aspx")) || pagename == "")
                         {

                             if (catpath.Length > 3)
                             {

                                 a = a + 1;


                                 breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
                                      (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" + ds.Tables[0].Rows[0]["ItemValue"].ToString();

                                 HttpContext.Current.Session[ds.Tables[0].Rows[0]["ItemValue"].ToString() + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();

                             }

                             // IsFromps = true;


                             breadcrumEA = breadcrumEA.ToUpper().Replace("USER SEARCH=", "").Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "").Replace("USERSEARCH2=", "").Replace("CATEGORY=", "");
                             //  breadcrumEA = breadcrumEA.Replace("//// //// ////", "//// //// ////" + "UserSearch=");
                         }


                         //  breadcrumEA = breadcrumEA.Replace("+", "||"); 
                         //Cons_NewURl(_stmpl_records, breadcrumEA, PAGENAME[0].ToLower(), true, false);                     
                         if (newpagename == "")
                         {
                             newpagename = "ps.aspx";
                         }
                         //objHelperServices.SimpleURL(_stmpl_records, breadcrumEA, "BC" + "-" + newpagename);

                         string TBT_REWRITEURL = "/" + objHelperServices.SimpleURL_Str(breadcrumEA, "BC" + "-" + newpagename, false) + "/" + newpagename.Replace(".aspx", "/");
                         _stmpl_records.SetAttribute("TBT_REWRITEURL", TBT_REWRITEURL);
                         drconsbc["TBT_REWRITEURL"] = TBT_REWRITEURL;
                         string[] REMOVEPAGENAME = row["RemoveUrl"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
                         string Rpagename = string.Empty;
                         Rpagename = REMOVEPAGENAME[0].ToLower();
                         if (Rpagename.Contains("pl.aspx"))
                         {

                             newremovepagename = REMOVEPAGENAME[0].ToLower();

                         }
                         else if (Rpagename.Contains("pd.aspx"))
                         {
                             newremovepagename = REMOVEPAGENAME[0].ToLower();
                             if (IsFromPowersearch_RC == true)
                             {
                                 RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USERSEARCH1=", "USERSEARCH=");
                             }
                         }
                         else if (Rpagename.Contains("fl.aspx"))
                         {
                             newremovepagename = REMOVEPAGENAME[0].ToLower().Replace("fl.aspx", "fl");
                             if ((IsFromPowersearch) && (!(RemovebreadcrumEA.Contains("USERSEARCH1="))))
                             {
                                 RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("//// //// ////", "//// //// ////" + "USERSEARCH1=");
                             }
                             else
                             {
                                 if ((IsFromBrand == true) && (istsb != string.Empty) && (istsm != string.Empty) && (familybrandmodelset == false))
                                 {
                                     RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace(istsm.ToUpper(), "MODEL=" + istsm.ToUpper()).Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
                                     familybrandmodelset = true;
                                 }

                             }
                         }
                         else if (Rpagename.Contains("ct.aspx") || Rpagename.Contains("microsite.aspx"))
                         {
                             newremovepagename = REMOVEPAGENAME[0].ToLower();
                             //RemovebreadcrumEA = RemovebreadcrumEA;

                         }
                         else if (Rpagename.Contains("bb.aspx"))
                         {
                             newremovepagename = REMOVEPAGENAME[0].ToLower();
                             RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("MODEL=", "");

                         }
                         else if (Rpagename.Contains("ps.aspx"))
                         {
                             newremovepagename = REMOVEPAGENAME[0].ToLower();
                             IsFromPowersearch_RC = true;
                             RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USER SEARCH=", "").Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "").Replace("USERSEARCH2=", "").Replace("CATEGORY=", "");
                         }

                         else
                         {
                             newremovepagename = REMOVEPAGENAME[0].ToLower();

                         }
                         //added by:indu
                         RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USERSEARCH1 = USERSEARCH =", "USERSEARCH =").Replace("USERSEARCH1=USERSEARCH1=", "USERSEARCH1=")
                             .Replace("MODEL=MODEL=", "MODEL=").Replace("BRAND=BRAND=", "BRAND=");
                         // Cons_NewURl(_stmpl_records, breadcrumEA, REMOVEPAGENAME[0].ToLower(), true, true);
                         // RemovebreadcrumEA = RemovebreadcrumEA.Replace("+", "||"); 
                         if (newremovepagename == "")
                         {
                             newremovepagename = "ps.aspx";
                         }
                         // objHelperServices.SimpleURL(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + newremovepagename.ToLower());

                         string TBT_REMOVEREWRITEURL = "/" + objHelperServices.SimpleURL_Str(RemovebreadcrumEA, "BC" + "-" + newpagename, false) + "/" + newremovepagename.Replace(".aspx", "/");
                         if (TBT_REMOVEREWRITEURL == "//home/")
                         {
                             TBT_REMOVEREWRITEURL = "/Home.aspx";
                         }
                         _stmpl_records.SetAttribute("TBT_REMOVEREWRITEURL", TBT_REMOVEREWRITEURL);

                         drconsbc["TBT_REMOVEREWRITEURL"] = TBT_REMOVEREWRITEURL;
                         if (i == ds.Tables[0].Rows.Count - 1 && (pagename.Contains("ps.aspx") || pagename.Contains("pl.aspx") || pagename.Contains("bb.aspx") || pagename.Contains("")))
                         {
                             drconsbc["lastvalue"] = ds.Tables[0].Rows[i]["RemoveEAPATH"].ToString() + "@@" + Itemtype + "@@" + ds.Tables[0].Rows[i]["Itemvalue"].ToString();
                           
                             // _stmpl_records.SetAttribute("lastvalue", newurl);

                         }
                     }
                     catch (Exception ex)
                     {
                         objErrorHandler.ErrorMsg = ex;
                         objErrorHandler.CreateLog();
                     }
                     dtconsbc.Rows.Add(drconsbc);

                     lstrows1[cnt] = new TBWDataList1(_stmpl_records.ToString());
                     cnt = cnt + 1;

                 } //
                 Session.Add("YHSCell_dt", dtconsbc);
             }

             return lstrows1;
         }
         catch(Exception ex)
         {
             objErrorHandler.ErrorMsg = ex;
             objErrorHandler.CreateLog();
             return null;
  
         }
     }












     protected string ST_CategoriesMS()
     {

         string sHTML = "";
         string sBrandAndModelHTML = "";
         string sModelListHTML = "";
         bool isrecordexists = false;
         try
         {


             StringTemplateGroup _stg_container = null;
             StringTemplateGroup _stg_records = null;
             StringTemplate _stmpl_container = null;
             StringTemplate _stmpl_records = null;
             StringTemplate _stmpl_records1 = null;
             StringTemplate _stmpl_recordsrows = null;
             TBWDataList[] lstrecords = new TBWDataList[0];
             TBWDataList[] lstrows = new TBWDataList[0];

             StringTemplateGroup _stg_container1 = null;
             StringTemplateGroup _stg_records1 = null;
             TBWDataList1[] lstrecords1 = new TBWDataList1[0];
             TBWDataList1[] lstrows1 = new TBWDataList1[0];
             int ictrows = 0;
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
             string _seeall = "";
             _bypcat = Request.QueryString["bypcat"];




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

             if (Request.QueryString["fid"] != null)
                 _fid = Request.QueryString["fid"];
             if (Request.QueryString["pid"] != null)
                 _pid = Request.QueryString["pid"];

             if (Request.QueryString["seeall"] != null)
                 _seeall = Request.QueryString["seeall"];


             if (_catCid != "")
                 _parentCatID = GetParentCatID(_catCid);
             //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mpd.aspx") == true)
             //{
             //    if (HttpContext.Current.Session["Category_Attributes"] == null && _parentCatID != "")
             //    {
             //        EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
             //    }
             //}
             //if ((Request.Url.ToString().ToLower().Contains("mct.aspx") != true) && (Request.Url.ToString().ToLower().Contains("mpl.aspx") != true)
             //       && (Request.Url.ToString().ToLower().Contains("mps.aspx") != true)
             //       && (Request.Url.ToString().ToLower().Contains("mfl.aspx") != true)
             //       && (Request.Url.ToString().ToLower().Contains("mpd.aspx") != true))
             //{
             //    HttpCookie LoginInfoCookie = Request.Cookies["ActiveSupplier"];
             //    if (LoginInfoCookie != null && LoginInfoCookie["SupplierID"] != null)
             //    {
             //        EasyAsk.GetMainMenuClickDetail(objSecurity.StringDeCrypt_password(LoginInfoCookie["SupplierID"] ).ToString(), "");
             //    }
             
             //}
             //if (HttpContext.Current.Session["MainCategory"] == null || HttpContext.Current.Session["MainMenuClick"] == null)
             //{
             //    HttpCookie LoginInfoCookie = Request.Cookies["ActiveSupplier"];
             //    if (LoginInfoCookie != null && LoginInfoCookie["SupplierID"] != null)
             //    {
             //        EasyAsk.GetMainMenuClickDetail(objSecurity.StringDeCrypt_password(LoginInfoCookie["SupplierID"]).ToString(), "");
             //    }
             //}
                    

             if (HttpContext.Current.Session["MainCategory"] != null)
             {
                 DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
                 if (dr.Length > 0)
                     _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
             }

           

             if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx") == true)
             {
                 if (_bypcat == null)
                 {
                     dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
                     DataTable result = null; //Declare a dataSet to be filled.

                     //Sort data
                     if (HttpContext.Current.Request.QueryString["cid"] != null)
                     {
                         if (HttpContext.Current.Request.QueryString["cid"].ToString().ToUpper() == "WESNEWS")
                         {
                             try
                             {
                                 dscat.Tables[0].DefaultView.Sort = "CUSTOM_TEXT_FIELD1 desc";

                                 // Store in new Dataset
                                 // result.Tables.Add(dscat.Tables[0].DefaultView.ToTable());
                                 result = dscat.Tables[0].DefaultView.ToTable();
                                 dscat.Tables.Remove("Category");
                                 dscat.Tables.Add(result);
                             }
                             catch
                             {

                             }
                         }
                     }
                 }
                 else if (_tsb != "")
                 {
                     dscat = null;

                     //dscat = (DataSet)HttpContext.Current.Session["WESBrand_Model_Attributes"];

                 }

             }
             //else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mpd.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("mfl.aspx") == true)
             //{
             //    //dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
             //    //if (dscat == null)
             //    //{
             //    //    EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
             //    //    dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
             //    //}
             //   // dscat = null;

             //}
             else
             {
                 dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
             }
             //Modified by Indu
             //Reason:To display category and subcategory drop down details in powersearch page
             if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mps.aspx") == true && dscat != null && dscat.Tables.Count > 0)
             {
                 if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                 {
                     DataSet breadcrumbPS = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

                     if (breadcrumbPS != null && breadcrumbPS.Tables.Count > 0 && breadcrumbPS.Tables[0].Rows.Count == 1)
                     {
                         Session["LHScatPS"] = dscat;
                         Session["LHSsubcatPS"] = null;

                     }
                     else if (breadcrumbPS != null && breadcrumbPS.Tables.Count > 0 && breadcrumbPS.Tables[0].Rows.Count == 2 && breadcrumbPS.Tables[0].Rows[1]["ItemType"].ToString().ToLower().Contains("category"))
                     {
                         Session["LHSsubcatPS"] = dscat;
                         Session["subcatshow"] = "true";
                     }
                     else if (breadcrumbPS != null && breadcrumbPS.Tables.Count > 0 && breadcrumbPS.Tables[0].Rows.Count == 3 && breadcrumbPS.Tables[0].Rows[1]["ItemType"].ToString().ToLower().Contains("category") && breadcrumbPS.Tables[0].Rows[2]["ItemType"].ToString().ToLower().Contains("category"))
                     {
                         Session["LHSsubcatPS"] = (DataSet)HttpContext.Current.Session["LHSsubcatPS"];
                         // Session["LHSsubcatPS"] = dscat;
                         Session["subcatshow"] = "true";
                     }
                     else
                     {
                         // Session["LHSsubcatPS"] = null;  
                         Session["subcatshow"] = "false";
                     }

                 }
             }
             //End

             // modify by palani
             if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mbb.aspx") == true && dscat != null)
             {
                 if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                 {
                     DataSet breadcrumb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                     if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count == 3)
                     {
                         Session["dscatBrandModel"] = _tsb + "," + _tsm;
                         Session["dscatname"] = _value;
                         Session["dscatbybrand"] = dscat;
                         Session["dssubcatbybrand"] = null;
                     }
                     if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count == 4)
                     {
                         if (breadcrumb.Tables[0].Rows[3]["ItemType"].ToString() == "Category")
                             Session["dssubcatbybrand"] = dscat;
                     }
                 }


             }




             DataSet dsbc = null;
             if (HttpContext.Current.Session["BreadCrumbDS"] != null)
             {
                 dsbc = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

             }

             string _BCEAPath = "//// //// ////";
             if (dsbc != null)
             {
                 for (int i = 0; i <= dsbc.Tables[0].Rows.Count - 1; i++)
                 {
                     string TBW_ITEM_NAME = string.Empty;
                     DataRow row = dsbc.Tables[0].Rows[i];
                     //if (row["ItemType"].ToString().ToLower() == "family")
                     //    TBW_ITEM_NAME = row["FamilyName"].ToString();
                     //else if (row["ItemType"].ToString().ToLower() == "product")
                     //    TBW_ITEM_NAME = row["ProductCode"].ToString();
                     //else
                     //    TBW_ITEM_NAME = row["ItemValue"].ToString();

                     string _istsb = string.Empty;

                     if (row["ItemType"].ToString().ToLower() == "brand")
                     {

                         _istsb = row["ItemValue"].ToString();
                     }

                     if (row["ItemType"].ToString().ToLower() == "family")
                     {
                         TBW_ITEM_NAME = row["ItemValue"].ToString() + "=" + row["FamilyName"].ToString();
                     }
                     //else if (row["ItemType"].ToString().ToLower() == "category" && i == 0)
                     //{
                     //    TBW_ITEM_NAME = hfcid.Value + "=" + row["ItemValue"].ToString();

                     //}
                     else if (row["ItemType"].ToString().ToLower() == "product")
                     {
                         if (row["ProductCode"].ToString() != "")
                         {
                             TBW_ITEM_NAME = row["ItemValue"].ToString() + "=" + row["ProductCode"].ToString();
                         }
                         else
                         {
                             TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                         }
                     }
                     //(row["ItemType"].ToString().ToLower() != "category") && 
                     else if ((row["ItemType"].ToString().ToLower() == "category" && i != 0) && (row["ItemType"].ToString().ToLower() != "model") && (row["ItemType"].ToString().ToLower() != "brand") && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true))
                     {
                         TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                     }

                     //else if ((row["ItemType"].ToString().ToLower() == "brand") || (row["ItemType"].ToString().ToLower() == "model") && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx") == true)||(HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)))
                     //{
                     //    TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                     //}
                     else if ((row["ItemType"].ToString().ToLower() != "category") && (row["ItemType"].ToString().ToLower() != "model") && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx")) && ((row["ItemType"].ToString().ToLower() != "user search") && row["ItemType"].ToString().ToLower().Contains("usersearch") == false)))
                     {
                         TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                     }

                     else if ((row["ItemType"].ToString().ToLower() != "brand") && (row["ItemType"].ToString().ToLower() != "category") && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx") == true))
                     {
                         TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                     }
                     else if ((row["ItemType"].ToString().ToLower() == "model") && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))))
                     {
                         if (_istsb != "")
                         {
                             TBW_ITEM_NAME = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("model = ", "model=").Replace("model = ", "model=").Replace("'", "").Replace(_istsb + ":", "");
                         }
                         else
                         {
                             TBW_ITEM_NAME = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("MODEL = ", "MODEL=").Replace("model = ", "model=").Replace("'", "");
                         }
                     }
                     else if ((row["ItemType"].ToString().ToLower() != "category") && (row["ItemType"].ToString().ToLower() != "user search") && (row["ItemType"].ToString().ToLower().Contains("usersearch") == false) && (row["ItemType"].ToString().ToLower() != "model") && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx") || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")))
                     {
                         TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();
                     }
                     else if ((row["ItemType"].ToString().ToLower() == "user search") && (row["url"].ToString().ToLower().Contains("ps.aspx")) && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")))
                     {
                         TBW_ITEM_NAME = "UserSearch" + "=" + row["ItemValue"].ToString();
                     }
                     else if (((row["ItemType"].ToString().ToLower() != "brand")) && ((row["ItemType"].ToString().ToLower() != "model")) && (row["Url"].ToString().Contains("bb.aspx") == true))
                     {

                         TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                     }
                     else
                     {
                         TBW_ITEM_NAME = row["ItemValue"].ToString();
                     }
                     TBW_ITEM_NAME = HttpUtility.UrlEncode(TBW_ITEM_NAME);
                     //.Replace("+", "||").Replace("&", "^^").Replace(":", "~`");;

                     if (_BCEAPath == "//// //// ////")
                     {
                         _BCEAPath = _BCEAPath + TBW_ITEM_NAME;
                     }
                     else
                     {

                         _BCEAPath = _BCEAPath + "////" + TBW_ITEM_NAME;


                     }
                 }
                 HttpContext.Current.Session["breadcrumEAPATH"] = _BCEAPath;
             }
             if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mpd.aspx") == true              
                 || HttpContext.Current.Request.Url.ToString().ToLower().Contains("mfl.aspx") == true                 
                 || Request.Url.ToString().ToLower().Contains("mlogin.aspx") == true
                    
                || HttpContext.Current.Request.Url.ToString().ToLower().Contains("mcontactus.aspx") == true
                 || HttpContext.Current.Request.Url.ToString().ToLower().Contains("maboutus.aspx") == true || 
                 HttpContext.Current.Request.Url.ToString().ToLower().Contains("mmyaccount.aspx") == true || 
                 HttpContext.Current.Request.Url.ToString().ToLower().Contains("mconfirmmessage.aspx") == true || 
                 HttpContext.Current.Request.Url.ToString().ToLower().Contains("mforgotpassWord.aspx") == true )
             {
                 return "";
             }
             else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mct.aspx") == true)
             {

                 string Mainmenu = ST_MainMenu();
                 string NewProductNav = ST_NewProductNav();

                 sHTML= Mainmenu + NewProductNav;
             }
             //else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mcontactus.aspx") == true || 
             //    HttpContext.Current.Request.Url.ToString().ToLower().Contains("maboutus.aspx") == true || 
             //    HttpContext.Current.Request.Url.ToString().ToLower().Contains("mmyaccount.aspx") == true || 
             //    HttpContext.Current.Request.Url.ToString().ToLower().Contains("mchangepassword.aspx") == true || 
             //    HttpContext.Current.Request.Url.ToString().ToLower().Contains("mchangeusername.aspx") == true || 
             //    HttpContext.Current.Request.Url.ToString().ToLower().Contains("mconfirmmessage.aspx") == true || 
             //    HttpContext.Current.Request.Url.ToString().ToLower().Contains("mforgotpassWord.aspx") == true ||
             //    HttpContext.Current.Request.Url.ToString().ToLower().Contains("muserprofile.aspx") == true
            
             //    ) 
             //{

             //    sHTML = ST_NewProductNavContactus();
             //   // return objHelperServices.StripWhitespace(sHTML);

             //}
             else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mcheckout.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("morderdetails.aspx") == true)
             {
                 sHTML="";
             }
             else
             {


                 _stg_records = new StringTemplateGroup("searchrsltcategoryleftrecords", MicroSiteTemplate);
                 _stg_container = new StringTemplateGroup("searchrsltcategoryleftmain", MicroSiteTemplate);
                 if (dscat != null)
                 {

                     if (dscat.Tables.Count > 0)
                         lstrows = new TBWDataList[dscat.Tables.Count + 1];

                     for (int i = 0; i < dscat.Tables.Count; i++)
                     {
                         Boolean tmpallow = true;

                         if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)
                         {
                             if (dscat.Tables[i].TableName.Contains("Category"))
                                 tmpallow = true;
                             else if (dscat.Tables[i].TableName.Contains("Brand"))
                                 tmpallow = false;
                             else if (Request.QueryString["byp"] == "2")
                                 tmpallow = true;
                             else
                                 tmpallow = false;
                         }
                         if (tmpallow == true)
                         {
                             if (dscat.Tables[i].Rows.Count > 0)
                             {
                                 lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];
                                 lstrecords1 = new TBWDataList1[dscat.Tables[i].Rows.Count + 1];
                                 int ictrecords = 0;

                                 int j = 0;
                                 foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
                                 {

                                     isrecordexists = true;
                                     if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx") == true)
                                     {
                                         _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell");
                                     }

                                     else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mbb.aspx") == true)
                                     {
                                         _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell1");
                                     }
                                     else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mps.aspx") == true)
                                     {
                                         _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell2");
                                     }
                                     else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mfl.aspx") == true)
                                     {
                                         _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell3");
                                     }
                                     else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mpl.aspx") == true)
                                     {
                                         _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell4");
                                     }
                                     else
                                     {
                                         _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell");
                                     }
                                     string brandname = string.Empty;
                                     string TBW_ATTRIBUTE_VALUE = string.Empty;
                                     if (dscat.Tables[i].TableName.Contains("Category"))
                                     {
                                         _stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                         _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                         //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));

                                         _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr["Category_Name"].ToString());
                                         _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
                                         TBW_ATTRIBUTE_VALUE = dr["Category_Name"].ToString();
                                         _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                         brandname = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_BRAND").ToString();
                                         if (brandname != "")
                                         {
                                             brandname = brandname + ":";
                                         }
                                         _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));

                                         if (_parentCatID == WesNewsCategoryId)
                                             _stmpl_records.SetAttribute("TBW_OPTION_CATEGORY_ID", "<br/>" + dr["CATEGORY_ID"].ToString());
                                         else
                                             _stmpl_records.SetAttribute("TBT_CATEGORY_ID_DIV", "");
                                     }
                                     else
                                     {
                                         //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                                         //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                         _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_catCid.ToString()));
                                         _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr[0].ToString());
                                         _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr[0].ToString()));
                                         TBW_ATTRIBUTE_VALUE = dr[0].ToString();
                                         _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                         brandname = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_BRAND").ToString();
                                         if (brandname != "")
                                         {
                                             brandname = brandname + ":";
                                         }

                                         _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));


                                     }

                                     try
                                     {
                                         string itemtitle = _stmpl_records.GetAttribute("TBW_ITEM_NAME").ToString();
                                         _stmpl_records.SetAttribute("TBW_ITEM_Title", itemtitle.Replace('"', ' '));
                                     }
                                     catch
                                     { }
                                     _stmpl_records.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
                                     _stmpl_records.SetAttribute("TBW_BRAND", HttpUtility.UrlEncode(_tsb));
                                     _stmpl_records.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(_tsm));
                                     _stmpl_records.SetAttribute("TBW_FAMILY_ID", _fid);

                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_TYPE", dscat.Tables[i].TableName.ToString());
                                     if (HttpContext.Current.Session["EA"] != null)
                                     {
                                         _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
                                         if (HttpContext.Current.Session["EA_URL"] == null)
                                         {
                                             string eapath = string.Empty;
                                             if (HttpContext.Current.Session["EA"].ToString() == "AllProducts////WESAUSTRALASIA")
                                             {
                                                 eapath = _BCEAPath.Replace("//// //// ////", "AllProducts////WESAUSTRALASIA////");
                                             }
                                             else
                                             {
                                                 eapath = HttpContext.Current.Session["EA"].ToString();
                                             }
                                             _stmpl_records.SetAttribute("ORG_EA_PATH", eapath);
                                         }
                                         else
                                         {
                                             _stmpl_records.SetAttribute("ORG_EA_PATH", HttpContext.Current.Session["EA_URL"].ToString());
                                         }
                                       }
                                     string TBW_ATTRIBUTE_TYPE = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_TYPE").ToString().ToUpper();
                                     string TBW_ATTRIBUTE_VALUE_NEW = dr[0].ToString();
                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_INPUT", dr[0].ToString().Replace("$", "").Replace(" ", ""));


                                     if (_tsm != string.Empty)
                                     {
                                         _BCEAPath = _BCEAPath.Replace(_tsm, HttpUtility.UrlEncode(_tsm));
                                     }


                                     string TBW_ATTRIBUTE_NAME_NEW = TBW_ATTRIBUTE_VALUE;
                                     if (TBW_ATTRIBUTE_TYPE == "MODEL")
                                     {
                                         if (brandname != "")
                                         {
                                             TBW_ATTRIBUTE_NAME_NEW = brandname + ":" + TBW_ATTRIBUTE_VALUE_NEW;
                                         }
                                        
                                     }
                                     _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME_NEW", TBW_ATTRIBUTE_NAME_NEW);

                                     objHelperServices.SimpleURL_MS(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_VALUE, "");
                                     lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());

                                     //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("mpd.aspx") == true)
                                     //{
                                     //    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                     //}
                                     //else
                                     //{
                                     //    if (dscat.Tables[i].TableName.Contains("Category") == true)
                                     //    {

                                     //        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                     //    }
                                     //    else
                                     //    {
                                     //        if (ictrecords <= 4)
                                     //        {
                                     //            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                     //        }
                                     //        else
                                     //        {
                                     //            lstrecords1[ictrecords] = new TBWDataList1(_stmpl_records.ToString());
                                     //        }
                                     //    }

                                     //}
                                     ictrecords++;
                                 }

                                 j++;
                                 _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
                                 //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("mpd.aspx") == true)
                                 //    _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
                                 //else
                                 //{
                                 //    if (dscat.Tables[i].TableName.Contains("Category") == true)
                                 //    {
                                 //        _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
                                 //    }
                                 //    else
                                 //    {
                                 //        if (dscat.Tables[i].Rows.Count > 5)
                                 //            _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row");
                                 //        else
                                 //            _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
                                 //    }
                                 //}


                                 _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", dscat.Tables[i].TableName.ToString());
                                 _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_ID", dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", ""));
                                 _stmpl_recordsrows.SetAttribute("TBT_MENU_ID", (i + 1).ToString());
                                 _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);
                                 //_stmpl_recordsrows.SetAttribute("TBWDataList1", lstrecords1);
                                 lstrows[ictrows] = new TBWDataList(_stmpl_recordsrows.ToString());
                                 ictrows++;
                             }
                         }
                     }
                 }


            /*     // You Have Select
                 DataSet ds = new DataSet();
                 int cnt = 0;
                 ds = null;
                 if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                 {
                     ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

                 }
                 if (ds != null && ds.Tables[0].Rows.Count > 0)
                 {
                     lstrows1 = new TBWDataList1[ds.Tables[0].Rows.Count + 1];

                     //foreach (DataRow row in ds.Tables[0].Rows)
                     //{
                     string breadcrumEA = "//// //// ////";
                     string RemovebreadcrumEA = "//// //// ////";
                     bool IsFromPowersearch = false;
                     bool IsFromPowersearch_RC = false;
                     bool IsFromproductlist = false;
                     bool IsFromproductlist_RC = false;
                     bool IsFromBrand = false;
                     bool IsFromBrand_RC = false;
                     string istsm = string.Empty;
                     string istsb = string.Empty;
                     bool familybrandmodelset = false;
                   
                     for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                     {
                         DataRow row = ds.Tables[0].Rows[i];
                         DataRow Revrow;
                         string Itemvalue = string.Empty;
                         string newpagename = string.Empty;
                         string newremovepagename = string.Empty;
                         if (i == 0)
                         {
                             Revrow = ds.Tables[0].Rows[0];
                         }
                         else
                         {
                             Revrow = ds.Tables[0].Rows[i - 1];
                         }
                         _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "YHSCell");


                         _stmpl_records.SetAttribute("TBW_ITEM_TYPE", "Item " + row["ItemType"].ToString());

                         string[] PAGENAME = row["Url"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);

                         if (row["ItemType"].ToString().ToLower() == "category" && i == 0)
                         {
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString().ToLower());
                             Itemvalue = HttpUtility.UrlEncode(row["ItemValue"].ToString());
                         }
                         else if (row["ItemType"].ToString().ToLower() == "family")
                         {
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["FamilyName"].ToString().ToLower());
                             Itemvalue = row["ItemValue"].ToString() + "=" + row["FamilyName"].ToString();
                             HttpContext.Current.Session["S_FName"] = row["FamilyName"].ToString();
                         }
                         else if (row["ItemType"].ToString().ToLower() == "brand" && (PAGENAME[0].ToLower().Contains("ct.aspx") || PAGENAME[0].ToLower().Contains("microsite.aspx")))
                         {
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString());
                             Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());
                         }

                         else if (row["ItemType"].ToString().ToLower() == "product")
                         {
                             if (row["ProductCode"].ToString() != "")
                             {
                                 _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ProductCode"].ToString().ToLower());
                                 Itemvalue = row["ItemValue"].ToString() + "=" + row["ProductCode"].ToString();
                             }
                             else
                             {
                                 _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString().ToLower());
                                 Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());
                             }
                         }
                         else if ((row["ItemType"].ToString().ToLower() != "category")
                             && (row["ItemType"].ToString().ToLower() != "model")
                             && (row["ItemType"].ToString().ToLower() != "brand")
                             && (IsFromBrand == true)
                             && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")))
                         {
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString());
                             Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());

                         }
                         else if (((row["ItemType"].ToString().ToLower() != "brand")
                       && (row["ItemType"].ToString().ToLower() != "model")
                        && (row["ItemType"].ToString().ToLower() != "category")
                        )
                       && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true)
                        || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx"))
                       || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))

                       || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx"))
                       || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx"))))
                         {
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString());
                             Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());

                         }
                         else if ((row["ItemType"].ToString().ToLower() == "brand")
                             && (IsFromBrand == false)
                             && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == false)
                             && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx") == false))
                         //(row["ItemType"].ToString().ToLower() != "model")) && 
                         //((HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true) || 
                         //(HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx")) ||
                         //(HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) || 
                         //(HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx"))))
                         // else if (((row["ItemType"].ToString().ToLower() != "brand")) && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true) && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")))
                         {
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString());
                             Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());

                         }

                         else if ((row["ItemType"].ToString().ToLower() != "category")
                             && (row["ItemType"].ToString().ToLower() != "model")
                             && (row["ItemType"].ToString().ToLower() != "user search")
                             && (row["ItemType"].ToString().ToLower().Contains("usersearch") == false)
                             && (IsFromproductlist == true || IsFromPowersearch == true))
                         {
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString());
                             Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());
                         }
                         else if ((row["ItemType"].ToString().ToLower() == "model")
                             && (IsFromBrand == false))
                         {
                             //else if ((row["ItemType"].ToString().ToLower() == "model") && (((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")) || ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))))
                             //{
                             //else if ((row["ItemType"].ToString().ToLower() == "model") && (((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) && (IsFromBrand == false)) 
                             //    || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")) || ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))))
                             //{
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString());
                             Itemvalue = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("model = ", "model=").Replace("'", "");

                             Itemvalue = HttpUtility.UrlEncode(Itemvalue);
                             Itemvalue = Itemvalue.Trim();
                         }
                         else if (((row["ItemType"].ToString().ToLower() != "brand")) && ((row["ItemType"].ToString().ToLower() != "model")) && (row["Url"].ToString().Contains("mbb.aspx") == true))
                         {
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString());
                             Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());

                         }
                         else
                         {
                             _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString().ToLower());
                             Itemvalue = HttpUtility.UrlEncode(row["ItemValue"].ToString());
                         }
                         //Itemvalue = Itemvalue;
                         //.Replace("+", "||").Replace("&", "^^").Replace(":", "~`");; ;
                         _stmpl_records.SetAttribute("TBT_REMOVEEAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["RemoveEAPath"].ToString())));
                         _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"].ToString());
                         _stmpl_records.SetAttribute("TBT_URL", row["Url"].ToString());
                         _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["EAPath"].ToString())));
                         _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", row["ItemType"].ToString());
                         //modified by :indu
                         try
                         {


                             //TBWTemplateEngine objTempengine = new TBWTemplateEngine("", "", "");
                             string Itemtype = row["ItemType"].ToString().ToUpper();
                             string RevItemType = Revrow["ItemType"].ToString().ToUpper();

                             _stmpl_records.SetAttribute("TBT_REM_ATTRIBUTE_TYPE", RevItemType);

                             if (breadcrumEA == "//// //// ////")
                             {

                                 RemovebreadcrumEA = breadcrumEA;
                                 breadcrumEA = breadcrumEA + Itemvalue;



                             }
                             else
                             {

                                 RemovebreadcrumEA = breadcrumEA;
                                 breadcrumEA = breadcrumEA + "////" + Itemvalue;


                             }
                             if (PAGENAME[0].ToLower().Contains("pl.aspx"))
                             {
                                 newpagename = PAGENAME[0].ToLower();
                                 breadcrumEA = breadcrumEA.ToLower().Replace("category=", "");
                                 IsFromproductlist = true;
                             }
                             else if (PAGENAME[0].ToLower().Contains("pd.aspx"))
                             {
                                 newpagename = PAGENAME[0].ToLower();
                                 if (IsFromPowersearch == true)
                                 {
                                     breadcrumEA = breadcrumEA.ToUpper().Replace("USERSEARCH1=", "USERSEARCH=");
                                 }


                             }
                             else if (PAGENAME[0].ToLower().Contains("fl.aspx"))
                             {
                                 newpagename = PAGENAME[0].ToLower();
                                 if ((IsFromPowersearch == true) && (RemovebreadcrumEA.Contains("USERSEARCH1=") == false))
                                 {
                                     breadcrumEA = breadcrumEA.ToUpper().Replace("//// //// ////", "//// //// ////" + "USERSEARCH1=");
                                 }
                                 else
                                 {
                                     if ((row["ItemType"].ToString().ToLower() == "brand") && (breadcrumEA.ToLower().Contains("brand=") == false))
                                     {
                                         istsb = row["ItemValue"].ToString();
                                         breadcrumEA = breadcrumEA.ToUpper().Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
                                     }
                                     else if ((IsFromBrand == true) && (istsb != string.Empty) && (istsm != string.Empty))
                                     {
                                         if (breadcrumEA.ToUpper().Contains("MODEL=") == false)
                                         {
                                             breadcrumEA = breadcrumEA.ToUpper().Replace(istsm.ToUpper(), "MODEL=" + istsm.ToUpper());
                                         }
                                         if (breadcrumEA.ToUpper().Contains("BRAND=") == false)
                                         {
                                             breadcrumEA = breadcrumEA.ToUpper().Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
                                         }
                                     }


                                 }
                             }
                             else if (PAGENAME[0].ToLower().Contains("ct.aspx") || PAGENAME[0].ToLower().Contains("microsite.aspx"))
                             {
                                 newpagename = PAGENAME[0].ToLower();
                                 // breadcrumEA = breadcrumEA.Replace("Brand=", "");
                                 if (Itemtype.ToLower() == "brand")
                                 {
                                     istsb = Itemvalue.ToUpper().Replace("BRAND=", "");
                                     IsFromBrand = true;
                                 }

                             }
                             else if (PAGENAME[0].ToLower().Contains("bb.aspx"))
                             {
                                 newpagename = PAGENAME[0].ToLower();
                                 IsFromBrand = true;
                                 if (Itemtype.ToLower() == "model")
                                 {
                                     istsm = Itemvalue.ToUpper().Replace("MODEL=", "");
                                 }
                                 breadcrumEA = breadcrumEA.ToUpper().Replace("MODEL=", "").Replace("BRAND=", "");
                             }
                             else if (PAGENAME[0].ToLower().Contains("ps.aspx"))
                             {
                                 newpagename = PAGENAME[0].ToLower();
                                 IsFromPowersearch = true;
                                 breadcrumEA = breadcrumEA.ToUpper().Replace("USER SEARCH=", "").Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "").Replace("USERSEARCH2=", "").Replace("CATEGORY=", "");
                             }

                             else
                             {
                                 newpagename = PAGENAME[0].ToLower();

                             }
                             //  breadcrumEA = breadcrumEA.Replace("+", "||"); 
                             //Cons_NewURl(_stmpl_records, breadcrumEA, PAGENAME[0].ToLower(), true, false);                     
                             objHelperServices.Cons_NewURl(_stmpl_records, breadcrumEA, "BC" + "-" + "m"+newpagename, true, Itemtype);
                             string[] REMOVEPAGENAME = row["RemoveUrl"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
                             if (REMOVEPAGENAME[0].ToLower().Contains("pl.aspx"))
                             {

                                 newremovepagename = REMOVEPAGENAME[0].ToLower();
                                 IsFromproductlist_RC = true;
                             }
                             else if (REMOVEPAGENAME[0].ToLower().Contains("pd.aspx"))
                             {
                                 newremovepagename = REMOVEPAGENAME[0].ToLower();
                                 if (IsFromPowersearch_RC == true)
                                 {
                                     RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USERSEARCH1=", "USERSEARCH=");
                                 }
                             }
                             else if (REMOVEPAGENAME[0].ToLower().Contains("fl.aspx"))
                             {
                                 newremovepagename = REMOVEPAGENAME[0].ToLower().Replace("fl.aspx", "fl");
                                 if ((IsFromPowersearch == true) && (RemovebreadcrumEA.Contains("USERSEARCH1=") == false))
                                 {
                                     RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("//// //// ////", "//// //// ////" + "USERSEARCH1=");
                                 }
                                 else
                                 {
                                     if ((IsFromBrand == true) && (istsb != string.Empty) && (istsm != string.Empty) && (familybrandmodelset == false))
                                     {
                                         RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace(istsm.ToUpper(), "MODEL=" + istsm.ToUpper()).Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
                                         familybrandmodelset = true;
                                     }

                                 }
                             }
                             else if (REMOVEPAGENAME[0].ToLower().Contains("ct.aspx") || REMOVEPAGENAME[0].ToLower().Contains("microsite.aspx"))
                             {
                                 newremovepagename = REMOVEPAGENAME[0].ToLower();
                                 //RemovebreadcrumEA = RemovebreadcrumEA;
                                 IsFromBrand_RC = true;
                             }
                             else if (REMOVEPAGENAME[0].ToLower().Contains("bb.aspx"))
                             {
                                 newremovepagename = REMOVEPAGENAME[0].ToLower();
                                 RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("MODEL=", "");
                                 IsFromBrand_RC = true;
                             }
                             else if (REMOVEPAGENAME[0].ToLower().Contains("ps.aspx"))
                             {
                                 newremovepagename = REMOVEPAGENAME[0].ToLower();
                                 IsFromPowersearch_RC = true;
                                 RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USER SEARCH=", "").Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "").Replace("USERSEARCH2=", "").Replace("CATEGORY=", "");
                             }

                             else
                             {
                                 newremovepagename = REMOVEPAGENAME[0].ToLower();

                             }
                             //added by:indu
                             RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USERSEARCH1 = USERSEARCH =", "USERSEARCH =").Replace("USERSEARCH1=USERSEARCH1=", "USERSEARCH1=")
                                 .Replace("MODEL=MODEL=", "MODEL=").Replace("BRAND=BRAND=", "BRAND=");
                             // Cons_NewURl(_stmpl_records, breadcrumEA, REMOVEPAGENAME[0].ToLower(), true, true);
                             // RemovebreadcrumEA = RemovebreadcrumEA.Replace("+", "||"); 
                             objHelperServices.Cons_NewURl(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + "m"+newremovepagename.ToLower(), true, RevItemType);


                         }
                         catch (Exception ex)
                         {
                             objErrorHandler.ErrorMsg = ex;
                             objErrorHandler.CreateLog();
                         }

                         if (i!=0) // remove first category name
                            lstrows1[cnt] = new TBWDataList1(_stmpl_records.ToString());

                         cnt = cnt + 1;

                     }


                 } */
                 /*   if (Request.Url.OriginalString.ToLower().Contains("ct.aspx") || Request.Url.OriginalString.ToLower().Contains("microsite.aspx"))
                    {
                        //if (_parentCatID != WesNewsCategoryId && _byp == "2") // Not WES News
                        if (_parentCatID != "" && _byp == "2")
                            sBrandAndModelHTML = ST_bybrand();

                    }
                    if ((Request.Url.OriginalString.ToLower().Contains("ct.aspx") || Request.Url.OriginalString.ToLower().Contains("microsite.aspx")) && _tsb != "" && _bypcat != null)
                    {
                        //if (_parentCatID != WesNewsCategoryId && _byp == "2") // Not WES News

                        if (_parentCatID != "" && _byp == "2")
                            sModelListHTML = ST_BrandAndModel();
                    }
                    */


                 // You Have Select
                 _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "main");
                 //_stmpl_container.SetAttribute("Selection", updateNavigation());
                 if (isrecordexists == false)
                 {
                     _stmpl_container.SetAttribute("TBT_PROD_FILTER", false);
                 }
                 else
                 {
                     _stmpl_container.SetAttribute("TBWDataList", lstrows);
                     _stmpl_container.SetAttribute("TBT_PROD_FILTER", true);
                 }
                  
                 //_stmpl_container.SetAttribute("BRAND_AND_MODEL_HTML", sBrandAndModelHTML);
                 //_stmpl_container.SetAttribute("MODEL_HTML", sModelListHTML);
                // _stmpl_container.SetAttribute("TBWDataList1", lstrows1);  //youer current Selection  
                 _stmpl_container.SetAttribute("NEWPRODUCTLIST", ST_NewProductNav());

                 if (Request.Url.OriginalString.ToLower().Contains("mpd.aspx"))
                 {
                     _stmpl_container.SetAttribute("TBT_SHOP_BY", false);
                 }
                 else
                 {
                     //Added by Indu
                     bool TBTSHOPBY=false;
                     for (int i = 0; i <= lstrows.Length - 1; i++)
                     {
                         if (lstrows[i] != null)
                         {
                             TBTSHOPBY = true;
                             break;
                         }
                     }


                     if (TBTSHOPBY == true)
                     {
                         _stmpl_container.SetAttribute("TBT_SHOP_BY", true);
                     }
                     else
                     {
                         _stmpl_container.SetAttribute("TBT_SHOP_BY", false);
                     }

                 }

                 sHTML += _stmpl_container.ToString();
             }
         }

         catch (Exception ex)
         {
             objErrorHandler.ErrorMsg = ex;
             objErrorHandler.CreateLog();
             sHTML = ex.Message;
         }
         finally
         {

         }


         return objHelperServices.StripWhitespace(sHTML);

     }
     public string ST_NewProductNav()
     {
         try
         {
             TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("NewProductNav", MicroSiteTemplate, objConnectionDB.ConnectionString);

             return tbwtMSEngine.ST_NewProductLogNav_Load();
         }


         catch (Exception ex)
         {
             objErrorHandler.ErrorMsg = ex;
             objErrorHandler.CreateLog();
             return string.Empty;
         }
     }

     public void  ST_NewProductLogNav_Thread(HttpContext ctx  )
     {
         try
         {
             string userid = ctx.Session["USER_ID"].ToString();
             if (userid == string.Empty)
                 userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

             DataSet tmps = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WAG 3,0," + userid);

            // stemplatepath = Server.MapPath("templates");
             //TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCTLOGNAV", Server.MapPath("templates"), objConnectionDB.ConnectionString);
            // tbwtEngine.RenderHTML("Column");
          // TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
             ctx.Session["NewProductLogNav"] = ST_NewProductLogNav_Load(tmps, userid, Server.MapPath("templates"),"NEWPRODUCTLOGNAV");
             
         }


         catch (Exception ex)
         {
             objErrorHandler.ErrorMsg = ex;
             objErrorHandler.CreateLog();
             
         }
     }
     public string ST_NewProductLogNav_Load(DataSet tmpds, string userid, string _SkinRootPath, string _Package)
     {
         string sHTML = string.Empty;

         try
         {
             StringTemplateGroup _stg_container = null;
             StringTemplateGroup _stg_records = null;
             StringTemplate _stmpl_container = null;
             StringTemplate _stmpl_records = null;
             StringTemplate _stmpl_recordstemp = new StringTemplate();
             //  StringTemplate _stmpl_records1 = null;
             // StringTemplate _stmpl_recordsrows = null;
             TBWDataList[] lstrecords = new TBWDataList[0];
             // TBWDataList[] lstrows = new TBWDataList[0];

             // StringTemplateGroup _stg_container1 = null;
             // StringTemplateGroup _stg_records1 = null;
             //   TBWDataList1[] lstrecords1 = new TBWDataList1[0];
             // TBWDataList1[] lstrows1 = new TBWDataList1[0];

             //   DataSet dscat = new DataSet();
             DataSet dsrecords = new DataSet();
             // DataTable dt = null;
             // DataRow[] drs = null;

            // string userid = ctx.Session["USER_ID"].ToString();

             if (userid == string.Empty)
                 userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

             //DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WAG 3,0," + userid);

             //if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
             //{
             //    //Commented by indu to reduce for loop moved to next for loop
             //    //tmpds.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
             //    //tmpds.Tables[0].Columns.Add("URL_RW_PATH_FAMILY", typeof(string));
             //    //foreach (DataRow dr in tmpds.Tables[0].Rows)
             //    //{
             //    //    dr["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"] + "////" + dr["FAMILY_ID"] + "=" + dr["FAMILY_name"] + "////" + dr["product_ID"] + "=" + dr["Code"], "pd.aspx", true, "");                          
             //    //    dr["URL_RW_PATH_FAMILY"] = objHelperService.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"] + "////" + dr["FAMILY_ID"] + "=" + dr["FAMILY_name"], "fl.aspx", true, "", false, true);

             //    //}

             //}
             //else
             //    return "";

             if (tmpds == null)
                 return "";

             // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
             //int ictrows = 0;






             _stg_records = new StringTemplateGroup("row", _SkinRootPath);
             _stg_container = new StringTemplateGroup("main", _SkinRootPath);


             lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];

             bool isenable = true;//objHelperServices.GetIsEcomEnabled(userid);

             int ictrecords = 0;
             string cellnpln = _Package + "\\cell";

             foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
             {
                 _stmpl_records = _stg_records.GetInstanceOf(cellnpln);

                 string[] catpath = dr["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                 string URL_RW_PATH = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr["FAMILY_ID"] + "////" + dr["product_ID"] + "=" + dr["Code"] + "////" + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + dr["FAMILY_name"], "pd.aspx", true);
                 string URL_RW_PATH_FAMILY = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr["FAMILY_ID"] + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + dr["FAMILY_name"], "fl.aspx", true);

                 //    dr["URL_RW_PATH_FAMILY"] = objHelperService.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"] + "////" + dr["FAMILY_ID"] + "=" + dr["FAMILY_name"], "fl.aspx", true, "", false, true);

                 _stmpl_records.SetAttribute("TBT_REWRITEURL", URL_RW_PATH);
                 _stmpl_records.SetAttribute("TBT_REWRITEURL_FAMILY", URL_RW_PATH_FAMILY);
                 _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString()));
                 _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);
                 _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["FAMILY_NAME"].ToString().Replace('"', ' '));
                 //_stmpl_records.SetAttribute("TBT_DESCRIPTION1", dr["DESCRIPTION"].ToString());
                 _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"]);
                 //  _stmpl_records.SetAttribute("TBT_YOURCOST", dr["COSt"].ToString());
                 _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);
                 // _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                 _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);
                 //_stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"].ToString());
                 _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);
                 // _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"].ToString());


                 lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                 ictrecords++;
             }


             _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\Main");



             _stmpl_container.SetAttribute("TBWDataList", lstrecords);
             sHTML = _stmpl_container.ToString();
         }
         catch (Exception ex)
         {
             objErrorHandler.ErrorMsg = ex;
             objErrorHandler.CreateLog();
             sHTML = "";
         }
         return objHelperServices.ProdimageRreplaceImages(sHTML,_Package);
     }
     protected string GetImagePath(object Path)
     {
         System.IO.FileInfo Fil = null;
         string retpath = string.Empty;
         try
         {
             string strProdImages = HttpContext.Current.Server.MapPath("ProdImages");
             if (Path.ToString().Contains("noimage.gif"))
                 retpath = "/images/noimage.gif";
             else
             {
                 Fil = new System.IO.FileInfo(strProdImages + Path.ToString());
                 if (Fil.Exists)
                 {
                     //retpath = Path.ToString();
                     retpath = Path.ToString().Replace("\\", "/");
                     //retpath = objHelperServices.SetImageFolderPath(Path.ToString().Replace("\\", "/"), "_th", "_Images_200");
                 }
                 else
                     retpath = "/images/noimage.gif";
             }

             return retpath;
         }
         catch
         {
             return retpath;
         }
         finally
         {
             Fil = null;
         }
     }
     public string ST_MainMenu()
     {
         try
         {
             TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("MAINMENU", MicroSiteTemplate, objConnectionDB.ConnectionString);

             return tbwtMSEngine.ST_MainMenu();
         }


         catch (Exception ex)
         {
             objErrorHandler.ErrorMsg = ex;
             objErrorHandler.CreateLog();
             return string.Empty;
         }
     }
     public string ST_NewProductNavContactus()
     {
         try
         {
             TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("NewProductNavContactus", MicroSiteTemplate, objConnectionDB.ConnectionString);

             return tbwtMSEngine.ST_NewProductLogNav_Load();
         }


         catch (Exception ex)
         {
             objErrorHandler.ErrorMsg = ex;
             objErrorHandler.CreateLog();
             return string.Empty;
         }
     }
     //protected string ST_Categories()
     //{

     //    string sHTML = "";
     //    string sBrandAndModelHTML = "";
     //    string sModelListHTML = "";
     //    try
     //    {


     //        StringTemplateGroup _stg_container = null;
     //        StringTemplateGroup _stg_records = null;
     //        StringTemplate _stmpl_container = null;
     //        StringTemplate _stmpl_records = null;
     //        StringTemplate _stmpl_records1 = null;
     //        StringTemplate _stmpl_recordsrows = null;
     //        TBWDataList[] lstrecords = new TBWDataList[0];
     //        TBWDataList[] lstrows = new TBWDataList[0];

     //        StringTemplateGroup _stg_container1 = null;
     //        StringTemplateGroup _stg_records1 = null;
     //        TBWDataList1[] lstrecords1 = new TBWDataList1[0];
     //        TBWDataList1[] lstrows1 = new TBWDataList1[0];
     //        int ictrows = 0;
     //        string _tsb = "";
     //        string _tsm = "";
     //        string _type = "";
     //        string _value = "";
     //        string _bname = "";
     //        string _searchstr = "";
     //        string _byp = "2";
     //        string _bypcat = null;


     //        string _pid = "";
     //        string _fid = "";
     //        string _seeall = "";
     //        _bypcat = Request.QueryString["bypcat"];




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

     //        if (Request.QueryString["fid"] != null)
     //            _fid = Request.QueryString["fid"];
     //        if (Request.QueryString["pid"] != null)
     //            _pid = Request.QueryString["pid"];

     //        if (Request.QueryString["seeall"] != null)
     //            _seeall = Request.QueryString["seeall"];


     //        if (_catCid != "")
     //            _parentCatID = GetParentCatID(_catCid);
     //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)
     //        {
     //            if (HttpContext.Current.Session["Category_Attributes"] == null)
     //            {
     //                EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
     //            }
     //        }




     //        if (Request.QueryString["path"] != null)
     //            HttpContext.Current.Session["EA"] = objSecurity.StringDeCrypt(Request.QueryString["path"].ToString());

     //        if (HttpContext.Current.Session["MainCategory"] != null)
     //        {
     //            DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
     //            if (dr.Length > 0)
     //                _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
     //        }

     //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true)
     //        {
     //            if (_bypcat == null)
     //            {

     //                EasyAsk.GetMainMenuClickDetail(_catCid, "");


     //                string CatName = "";
     //                DataTable tmptbl = null;
     //                if (HttpContext.Current.Session["MainMenuClick"] != null)
     //                {
     //                    tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0];

     //                    tmptbl = tmptbl.Select("CATEGORY_ID='" + _catCid + "'").CopyToDataTable();

     //                    if (tmptbl != null && tmptbl.Rows.Count > 0)
     //                    {
     //                        CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
     //                    }


     //                }


     //                if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
     //                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());

     //                EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //            }
     //            else if (_tsb != "")
     //            {
     //                string parentCatName = GetCName(_catCid);
     //                EasyAsk.GetWESModel(parentCatName, iCatalogId, _tsb);
     //            }

     //        }
     //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true)
     //        {
     //            int SubCatCount = 0;
     //            if (Request.QueryString["type"] == null)
     //            {
     //                if (_tsb != null && _tsb != "" && _tsm != null && _tsm != null)
     //                {

     //                    //string parentCatName = GetCName(ParentCatID);
     //                    //EasyAsk.GetBrandAndModelProducts(parentCatName, _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
     //                    if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
     //                        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

     //                    EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //                }
     //            }
     //            else
     //            {
     //                if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
     //                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

     //                if (_type != "")
     //                {

     //                    EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //                }
     //                else
     //                { //new open

     //                    EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //                }
     //            }
     //        }
     //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true)
     //        {
     //            if (Session["RECORDS_PER_PAGE_pl"] != null)
     //                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_pl"].ToString());


     //            EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //        }
     //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx") == true)
     //        {
     //            if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
     //                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());

     //            EasyAsk.GetAttributeProducts("ps", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

     //        }
     //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx") == true)
     //        {
     //            if (Request.QueryString["type"] == null)
     //            {

     //                EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");

     //            }
     //            else
     //            {

     //                EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value, _bname, "0", "0", "");

     //            }
     //        }
     //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)
     //        {
     //            if (Request.QueryString["type"] == null)
     //            {

     //                EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");

     //            }
     //            else
     //            {

     //                EasyAsk.GetAttributeProducts("ProductPage", _searchstr, _type, _value, _bname, "0", "0", "");

     //            }
     //        }
     //        //if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "" && (Request.QueryString["tsm"].ToString() != null && Request.QueryString["tsm"] != null))
     //        //{
     //        //    string category_nameh = "";
     //        //    DataSet tmp = GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'");
     //        //    if (tmp != null && tmp.Tables.Count >= 1 && tmp.Tables[0].Rows.Count >= 1)
     //        //    {

     //        //        category_nameh = tmp.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
     //        //    }

     //        //    EasyAsk.GetBrandAndModelProducts(category_nameh, Request.QueryString["tsm"].ToString(), Request.QueryString["tsb"].ToString(), iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
     //        //}

     //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true)
     //        {
     //            if (_bypcat == null)
     //                dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
     //            else if (_tsb != "")
     //            {
     //                dscat = null;

     //                //dscat = (DataSet)HttpContext.Current.Session["WESBrand_Model_Attributes"];

     //            }

     //        }
     //        else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)
     //        {
     //            //dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
     //            //if (dscat == null)
     //            //{
     //            //    EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
     //            //    dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
     //            //}
     //            dscat = null;

     //        }
     //        else
     //        {
     //            dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
     //        }
     //        // modify by palani
     //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true && dscat != null)
     //        {
     //            if (HttpContext.Current.Session["BreadCrumbDS"] != null)
     //            {
     //                DataSet breadcrumb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
     //                if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count == 3)
     //                {
     //                    Session["dscatBrandModel"] = _tsb + "," + _tsm;
     //                    Session["dscatname"] = _value;
     //                    Session["dscatbybrand"] = dscat;
     //                    Session["dssubcatbybrand"] = null;
     //                }
     //                if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count == 4)
     //                {
     //                    if (breadcrumb.Tables[0].Rows[3]["ItemType"].ToString() == "Category")
     //                        Session["dssubcatbybrand"] = dscat;
     //                }
     //            }


     //        }
     //        _stg_records = new StringTemplateGroup("searchrsltcategoryleftrecords", stemplatepath);
     //        _stg_container = new StringTemplateGroup("searchrsltcategoryleftmain", stemplatepath);
     //        if (dscat != null)
     //        {

     //            if (dscat.Tables.Count > 0)
     //                lstrows = new TBWDataList[dscat.Tables.Count + 1];

     //            for (int i = 0; i < dscat.Tables.Count; i++)
     //            {
     //                Boolean tmpallow = true;
     //                //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true && Request.QueryString["tsb"]!=null  && Request.QueryString["tsb"].ToString()!="")
     //                //{ 
     //                //    if (dscat.Tables[i].TableName.Contains("Model"))
     //                //        tmpallow = true;
     //                //    else
     //                //        tmpallow = false;
     //                //}
     //                //else 
     //                if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)
     //                {
     //                    if (dscat.Tables[i].TableName.Contains("Category"))
     //                        tmpallow = true;
     //                    else if (dscat.Tables[i].TableName.Contains("Brand"))
     //                        tmpallow = false;
     //                    else if (Request.QueryString["byp"] == "2")
     //                        tmpallow = true;
     //                    else
     //                        tmpallow = false;
     //                }
     //                if (tmpallow == true)
     //                {
     //                    if (dscat.Tables[i].Rows.Count > 0)
     //                    {
     //                        lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];
     //                        lstrecords1 = new TBWDataList1[dscat.Tables[i].Rows.Count + 1];
     //                        int ictrecords = 0;

     //                        int j = 0;
     //                        foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
     //                        {


     //                            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true)
     //                            {
     //                                _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell");
     //                            }
     //                            else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true)
     //                            {
     //                                _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell1");
     //                            }
     //                            else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx") == true)
     //                            {
     //                                _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell2");
     //                            }
     //                            else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx") == true)
     //                            {
     //                                _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell3");
     //                            }
     //                            else
     //                            {
     //                                _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell");
     //                            }
     //                            if (dscat.Tables[i].TableName.Contains("Category"))
     //                            {
     //                                _stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
     //                                _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
     //                                //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));

     //                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr["Category_Name"].ToString());
     //                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
     //                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
     //                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));

     //                                if (_parentCatID == WesNewsCategoryId)
     //                                    _stmpl_records.SetAttribute("TBW_OPTION_CATEGORY_ID", "<br/>" + dr["CATEGORY_ID"].ToString());
     //                                else
     //                                    _stmpl_records.SetAttribute("TBT_CATEGORY_ID_DIV", "");
     //                            }
     //                            else
     //                            {
     //                                //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
     //                                //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
     //                                _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_catCid.ToString()));
     //                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr[0].ToString());
     //                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr[0].ToString()));
     //                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
     //                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));


     //                            }


     //                            _stmpl_records.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
     //                            _stmpl_records.SetAttribute("TBW_BRAND", HttpUtility.UrlEncode(_tsb));
     //                            _stmpl_records.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(_tsm));
     //                            _stmpl_records.SetAttribute("TBW_FAMILY_ID", _fid);

     //                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_TYPE", dscat.Tables[i].TableName.ToString());
     //                            if (HttpContext.Current.Session["EA"] != null)
     //                            {
     //                                _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
     //                            }
     //                            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)
     //                            {
     //                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
     //                            }
     //                            else
     //                            {
     //                                if (dscat.Tables[i].TableName.Contains("Category") == true)
     //                                {

     //                                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
     //                                }
     //                                else
     //                                {
     //                                    if (ictrecords <= 4)
     //                                    {
     //                                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
     //                                    }
     //                                    else
     //                                    {
     //                                        lstrecords1[ictrecords] = new TBWDataList1(_stmpl_records.ToString());
     //                                    }
     //                                }

     //                            }
     //                            ictrecords++;
     //                        }

     //                        j++;
     //                        //if (dscat_full.Tables[i].Rows.Count > 0)
     //                        //{
     //                        //    _stmpl_recordsrows.SetAttribute("TBW_LINK", "<h3 class=expand id='" + dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", "") + "1' onclick=showHide('" + dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", "") + "');return false;>Show More Options</h3>");
     //                        //}
     //                        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)
     //                            _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
     //                        else
     //                        {
     //                            if (dscat.Tables[i].TableName.Contains("Category") == true)
     //                            {
     //                                _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
     //                            }
     //                            else
     //                            {
     //                                if (dscat.Tables[i].Rows.Count > 5)
     //                                    _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row");
     //                                else
     //                                    _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
     //                            }
     //                        }

     //                        //}
     //                        //else
     //                        //{
     //                        //    _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
     //                        //}
     //                        _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", dscat.Tables[i].TableName.ToString());
     //                        _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_ID", dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", ""));
     //                        _stmpl_recordsrows.SetAttribute("TBT_MENU_ID", (i + 1).ToString());
     //                        _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);
     //                        _stmpl_recordsrows.SetAttribute("TBWDataList1", lstrecords1);
     //                        lstrows[ictrows] = new TBWDataList(_stmpl_recordsrows.ToString());
     //                        ictrows++;
     //                    }
     //                }
     //            }
     //        }
     //        // You Have Select
     //        DataSet ds = new DataSet();
     //        int cnt = 0;
     //        ds = null;
     //        if (HttpContext.Current.Session["BreadCrumbDS"] != null)
     //        {
     //            ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

     //        }
     //        if (ds != null && ds.Tables[0].Rows.Count > 0)
     //        {
     //            lstrows1 = new TBWDataList1[ds.Tables[0].Rows.Count + 1];

     //            foreach (DataRow row in ds.Tables[0].Rows)
     //            {
     //                _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "YHSCell");
     //                _stmpl_records.SetAttribute("TBT_REMOVEEAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["RemoveEAPath"].ToString())));
     //                _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"].ToString());

     //                _stmpl_records.SetAttribute("TBW_ITEM_TYPE", "Item " + row["ItemType"].ToString());
     //                if (row["ItemType"].ToString().ToLower() == "family")
     //                    _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["FamilyName"].ToString());
     //                else if (row["ItemType"].ToString().ToLower() == "product")
     //                    _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ProductCode"].ToString());
     //                else
     //                    _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString());

     //                _stmpl_records.SetAttribute("TBT_URL", row["Url"].ToString());
     //                _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["EAPath"].ToString())));

     //                lstrows1[cnt] = new TBWDataList1(_stmpl_records.ToString());
     //                cnt = cnt + 1;

     //            }

     //        }
     //        if (Request.Url.OriginalString.ToLower().Contains("ct.aspx"))
     //        {
     //            if (_parentCatID != WesNewsCategoryId && _byp == "2") // Not WES News
     //                sBrandAndModelHTML = ST_bybrand();

     //        }
     //        if (Request.Url.OriginalString.ToLower().Contains("ct.aspx") && _tsb != "" && _bypcat != null)
     //        {
     //            if (_parentCatID != WesNewsCategoryId && _byp == "2") // Not WES News
     //                sModelListHTML = ST_BrandAndModel();
     //        }



     //        // You Have Select
     //        _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "main");
     //        //_stmpl_container.SetAttribute("Selection", updateNavigation());
     //        _stmpl_container.SetAttribute("TBWDataList", lstrows);
     //        _stmpl_container.SetAttribute("BRAND_AND_MODEL_HTML", sBrandAndModelHTML);
     //        _stmpl_container.SetAttribute("MODEL_HTML", sModelListHTML);
     //        _stmpl_container.SetAttribute("TBWDataList1", lstrows1);  //youer current Selection  
     //        sHTML += _stmpl_container.ToString();
     //    }

     //    catch (Exception ex)
     //    {
     //        sHTML = ex.Message;
     //    }
     //    finally
     //    {

     //    }


     //    return objHelperServices.StripWhitespace(sHTML);

     //}
   
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
                foreach (DataRow DR in DSBC.Tables[0].Rows)
                {
                    catIDtemp = DR["PARENT_CATEGORY"].ToString();
                    if (catIDtemp == "0")
                    {
                        // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                        return DR["CATEGORY_NAME"].ToString();
                    }
                }
            }
        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        return DSBC.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
    }


    protected string ST_BrandAndModel()
    {

        string sHTML = string.Empty;
        try
        {


            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
          //  StringTemplate _stmpl_records1 = null;
            StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

          //  StringTemplateGroup _stg_container1 = null;
           // StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];

            stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
            int ictrows = 0;
            string _bypcat = null;
            _bypcat = Request.QueryString["bypcat"];
            DataSet dscat = new DataSet();
           // DataTable dt = null;
            //if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true && _bypcat == null) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true)
            //{
            //    dt = (DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"];
            //    if (dt == null)
            //        dscat = null;
            //    else
            //        dscat.Tables.Add(dt.Copy());
            //}
            //else
            //{
                dscat = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
            //}






            if (dscat == null)
                return "";
            _stg_records = new StringTemplateGroup("searchrsltcategoryleftrecords", stemplatepath);
            _stg_container = new StringTemplateGroup("searchrsltcategoryleftmain", stemplatepath);
            if (dscat.Tables.Count > 0)
                lstrows = new TBWDataList[dscat.Tables.Count + 1];

            for (int i = 0; i < dscat.Tables.Count; i++)
            {
                Boolean tmpallow = true;
                //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true)
                //{
                //if (dscat.Tables[i].TableName.Contains("Brand") == true && Request.QueryString["byp"] == "2")
                //    tmpallow = true;
                //else
                //    tmpallow = false;
                //}
                if (dscat.Tables[i].TableName.Contains("Model") == true)
                    tmpallow = true;
                else
                    tmpallow = false;

                if (tmpallow == true)
                {
                    if (dscat.Tables[i].Rows.Count > 0)
                    {
                        lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];

                        int ictrecords = 0;

                        int j = 0;
                        foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
                        {


                            //if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true && _bypcat == null) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true)
                            //{
                            //    _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryright" + "\\" + "cell");
                            //    _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                            //    _stmpl_records.SetAttribute("TBW_BRAND", dr["TOSUITE_BRAND"].ToString());

                            //}
                            //else
                            //{
                                _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryright" + "\\" + "cell2");
                                _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                _stmpl_records.SetAttribute("TBW_BRAND", Server.UrlEncode(Request.QueryString["tsb"]));
                                _stmpl_records.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(dr["TOSUITE_MODEL"].ToString()));
                                _stmpl_records.SetAttribute("TBW_MODEL_NAME", dr["TOSUITE_MODEL"].ToString());

                            //}
                            if (HttpContext.Current.Session["EA"] != null)
                            {
                                _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
                                //ADDED bY INDU
                                if ((Request.Url.OriginalString.ToString().ToUpper().Contains("CT.ASPX")))
                                {
                                    string[] parentcategory = HttpContext.Current.Session["EA"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                    string parentcat = parentcategory[2];

                                    if (parentcat == string.Empty)
                                    {
                                        if (Request.QueryString["cid"] != null)
                                        {
                                            DataTable dt1 = (DataTable)HttpContext.Current.Session["dtcid"];

                                            DataRow[] foundRows;
                                            string cid = Request.QueryString["cid"].ToString();
                                            foundRows = dt1.Select("cid='" + cid + "' ");
                                            if (foundRows.Length > 0)
                                            {
                                                parentcat = foundRows[0][1].ToString();

                                            }
                                        }
                                    }
                                    string model =HttpUtility.UrlEncode (  dr["TOSUITE_MODEL"].ToString());
                                        //.Replace("+", "||").Replace("&", "^^").Replace(":", "~`");; ;
                                    objHelperServices.SimpleURL(_stmpl_records, parentcat + "////" + Request.QueryString["tsb"] + "////" + model, "bb.aspx");
                                }
                            
                            
                            }

                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                            ictrecords++;
                        }

                        j++;

                        _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryright" + "\\" + "row1");
                        //if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true && _bypcat == null) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true)
                         //   _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", dscat.Tables[i].TableName.ToString());
                        //else
                            _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", "MODEL");

                        _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);
                        lstrows[ictrows] = new TBWDataList(_stmpl_recordsrows.ToString());
                        ictrows++;
                    }
                }
            }

            _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategoryright" + "\\" + "main");
            //_stmpl_container.SetAttribute("Selection", updateNavigation());
            _stmpl_container.SetAttribute("TBWDataList", lstrows);
            sHTML += _stmpl_container.ToString();
        }

        catch (Exception ex)
        {
            sHTML = ex.Message;
        }
        finally
        {

        }


        return sHTML;
    }

    //   protected string ST_bybrand()
    //{
    //    StringTemplateGroup _stg_main_container = null;
    //    StringTemplateGroup _stg_records_container = null;
    //    StringTemplateGroup _stg_records = null;
    //    StringTemplate _stmpl_main_container_tmpl = null;
    //    StringTemplate _stmpl_records_container_tmpl = null;
    //    StringTemplate _stmpl_records_tmpl = null;
    //    //StringTemplate _stmpl_records_tmpl2 = null;
    //    //StringTemplate _stmpl_records_tmpl3 = null;
    //    TBWDataList[] lstrecords = new TBWDataList[0];
    //    TBWDataList[] lstrows = new TBWDataList[0];
    //    TBWDataList[] lstcontainers = new TBWDataList[3];
    //    DataSet dsCat;
    //    string[] filterval = null;
    //    string[] filterval1 = null;
    //    string[] filterval2 = null;
        
    //    //oPR = new ProductRender();
    //    string sHTML = string.Empty;
    //   // string dropdowncatid = "";
    //   // string _catid = "";
    //   // string _fid = "";
    //    int ictrecords = 0;


    //    try
    //    {
    //        stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";


    //        if (hidcatIds.Value != string.Empty && hidcatIds.Value != null)
    //        {
    //            filterval = hidcatIds.Value.Split('^');
    //        }
    //        if (HidsubcatIds.Value != string.Empty && HidsubcatIds.Value != null)
    //        {
    //            filterval1 = HidsubcatIds.Value.Split('^');
    //        }
    //        if (HidsubcatIds1.Value != string.Empty && HidsubcatIds1.Value != null)
    //        {
    //            filterval2 = HidsubcatIds1.Value.Split('^');
    //        }

    //       // if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")//&& Request.QueryString["cid"].ToString() == "WES210582")
    //        //{
    //            //string cid = Request.QueryString["cid"].ToString();

    //            //tempCID = Request.QueryString["cid"].ToString();
    //            //tempCName = GetCName(tempCID);
    //            dsCat = new DataSet();
    //            if (HttpContext.Current.Session["MainMenuClick"] != null)
    //            {
    //                dsCat.Tables.Add((DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"].Copy());
    //            }
    //            if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables[0].Rows.Count == 0)
    //                return "";


    //            if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
    //            {
    //                tempCID = Request.QueryString["cid"].ToString();
    //                ictrecords = 0;
    //                lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
    //                _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
    //                _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
    //                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");

    //                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
    //                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
    //                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
    //                ictrecords++;
    //                bool selstate = false;
    //                foreach (DataRow _drow in dsCat.Tables[0].Rows)
    //                {
    //                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
    //                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_BRAND"].ToString());
    //                    if (filterval != null && _drow["TOSUITE_BRAND"].ToString().ToLower() == filterval[0].ToString().ToLower())
    //                    {
    //                        _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
    //                        selstate = true;
    //                        string eapath="";
    //                        //eapath = HttpContext.Current.Session["EA"].ToString();
    //                        //if (eapath.Contains("////AttribSelect=Brand"))
    //                        //{
    //                        //    int inx = eapath.IndexOf("////AttribSelect=Brand");

    //                        //    eapath = eapath.ToString().Substring(0,inx);

                                
    //                        //}                            
    //                        //eapath = eapath + "////AttribSelect=Brand='" + _drow["TOSUITE_BRAND"].ToString() + "'";

    //                        eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_drow["EA_PATH"].ToString()));

    //                        string[] parentcategory = _drow["EA_PATH"].ToString().Split (new string[] { "////" }, StringSplitOptions.None);
    //                        string parentcat = parentcategory[2];
    //                        //modified by:indu 
    //                        //   Response.Redirect("ct.aspx?&ld=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&bypcat=1&path=" + eapath, false);

    //                        //Modified by:Indu
    //                        string originalurl = "/ct.aspx?&ld=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&bypcat=1&path=" + eapath;

    //                        //string stlistprod = objHelperServices.URLRewriteToAddressBar("ct.aspx", _drow["TOSUITE_BRAND"].ToString().ToUpper(), originalurl, HttpContext.Current.Server.MapPath("URL_Rewrite_Cat.ini"), true);
    //                        //Modified by:Indu
    //                        // string stlistprod = objHelperServices.Cons_NewURl_bybrand( _drow["TOSUITE_BRAND"].ToString().ToUpper(), originalurl, "");

    //                        string stlistprod = objHelperServices.Cons_NewURl_bybrand(originalurl, parentcat+"////"+"Brand="+ _drow["TOSUITE_BRAND"].ToString().ToUpper(), "ct.aspx", "");
    //                        string NEWURL = "/" + stlistprod + "/ct/";
    //                        //_stmpl_pages.SetAttribute("TBT_URL", stlistprod);
    //                        hfisselected.Value = "0"; 
    //                         Response.Redirect(NEWURL,false);
    //                        Session["PageRawURL"] = NEWURL.ToLower().Replace("ct/","ct.aspx?").Replace("pl/","pl.aspx?").Replace("fl/","fl.aspx?").Replace("bb/","bb.aspx?").Replace("pd/","pd.aspx?").Replace("ps/","ps.aspx?")    ;
    //                    }
    //                    else if (filterval == null && Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "" && _drow["TOSUITE_BRAND"].ToString().ToLower() == Server.UrlDecode(Request.QueryString["tsb"].ToString().ToLower()) && selstate == false)
    //                    {
    //                        filterval = new string[2];
    //                        filterval[0] = _drow["TOSUITE_BRAND"].ToString();
    //                        filterval[1] = _drow["TOSUITE_BRAND"].ToString();
    //                        _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
    //                        // Response.Redirect("ct.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
    //                    }
    //                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_BRAND"].ToString());
    //                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
    //                    ictrecords++;

    //                }
    //                _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
    //                _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
    //                _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
    //                lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

    //            }
    //            if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
    //            {
    //                tempCID = Request.QueryString["cid"].ToString();
    //                bool selstate1 = false;
    //                DataSet dsCat1 = new DataSet();
    //                //dsCat1 = oCat.GetWESModel(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString()));
    //                //dsCat1 = EasyAsk.GetWESModel(tempCName, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString())); 
    //                dsCat1 = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
    //                if (dsCat1 != null && dsCat1.Tables.Count > 0 && (dsCat1.Tables[0].Rows.Count > 0))
    //                {
    //                    ictrecords = 0;
    //                    DataRow[] _DCRow = null;
    //                    _DCRow = dsCat1.Tables[0].Select();
    //                    if (_DCRow != null && _DCRow.Length > 0)
    //                    {
    //                        lstrecords = new TBWDataList[_DCRow.Length + 1];
    //                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
    //                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
    //                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
    //                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
    //                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
    //                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
    //                        ictrecords++;
    //                        foreach (DataRow _drow in _DCRow)
    //                        {
    //                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
    //                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_MODEL"].ToString());
    //                            if (filterval1 != null && _drow["TOSUITE_MODEL"].ToString() == filterval1[0].ToString())
    //                            {
    //                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
    //                                selstate1 = true;
    //                                // Response.Redirect("pl.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
    //                                string eapath = string.Empty;
    //                                //eapath = HttpContext.Current.Session["EA"].ToString();
    //                                //if (eapath.Contains("////AttribSelect=Model"))
    //                                //{
    //                                //    int inx = eapath.IndexOf("////AttribSelect=Model");

    //                                //    eapath = eapath.ToString().Substring(0, inx );

                                        
    //                                //}
    //                                //eapath = eapath + "////AttribSelect=Model = '" + Request.QueryString["tsb"].ToString() + ":" + _drow["TOSUITE_MODEL"].ToString() + "'";

    //                                eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_drow["EA_PATH"].ToString()));
    //                                string[] parentcategory = _drow["EA_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
    //                                string parentcat = parentcategory[2];

    //                                //if (Request.QueryString["cid"] != null)
    //                                //{
    //                                //    DataTable dt1 = (DataTable)HttpContext.Current.Session["dtcid"];

    //                                //    DataRow[] foundRows;
    //                                //    string cid = Request.QueryString["cid"].ToString();
    //                                //    foundRows = dt1.Select("cid='" + cid + "' ");
    //                                //    if (foundRows.Length > 0)
    //                                //    {
    //                                //        parentcat = foundRows[0][0].ToString();

    //                                //    }
    //                                //}


    //                                //Modified By:Indu
    //                                //  Response.Redirect("bb.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&bypcat=1&path=" + eapath, false);
    //                                //Modified by:indu
    //                                string originalurl = "/bb.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&bypcat=1&path=" + eapath;

    //                                //string stlistprod = objHelperServices.URLRewriteToAddressBar("bb.aspx", _drow["TOSUITE_MODEL"].ToString().ToUpper() + "/" + Request.QueryString["tsb"].ToString().ToUpper(), originalurl, HttpContext.Current.Server.MapPath("URL_Rewrite_Brand.ini"), true);
    //                                string model =HttpUtility.UrlEncode(_drow["TOSUITE_MODEL"].ToString().ToUpper());
    //                                    //.Replace("+", "||").Replace("&", "^^").Replace(":", "~`");; 
    //                                string stlistprod = objHelperServices.Cons_NewURl_bybrand(originalurl, parentcat + "////" + Request.QueryString["tsb"].ToString().ToUpper() + "////" + model, "bb.aspx", "");
    //                                string NEWURL = "/" + stlistprod + "/bb/";
    //                                //_stmpl_pages.SetAttribute("TBT_URL", stlistprod);
    //                                 Response.Redirect(NEWURL,false);
    //                                Session["PageRawURL"] = NEWURL.Replace("bb/","bb.aspx?");
    //                            }
    //                            else if (filterval1 == null && Request.QueryString["tsm"] != null && _drow["TOSUITE_MODEL"].ToString() == Server.UrlDecode(Request.QueryString["tsm"].ToString()) && selstate1 == false)
    //                            {
    //                                filterval1 = new string[2];
    //                                filterval1[0] = _drow["TOSUITE_MODEL"].ToString();
    //                                filterval1[1] = _drow["TOSUITE_MODEL"].ToString();
    //                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

    //                            }
    //                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_MODEL"].ToString());
    //                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
    //                            ictrecords++;

    //                        }
    //                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
    //                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
    //                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
    //                        lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
    //                    }
    //                }
    //                dsCat1.Dispose();
    //            }
    //            else
    //            {
    //                lstrecords = new TBWDataList[1];
    //                _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
    //                _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
    //                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
    //                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
    //                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
    //                lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
    //                _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
    //                _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
    //                _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
    //                lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
    //            }

              

    //       // }
    //        //}
    //        _stg_main_container = new StringTemplateGroup("searchrsltmultilistmain", stemplatepath);
    //        _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistmain");
    //        _stmpl_main_container_tmpl.SetAttribute("TBW_CATEGORY_NAME", tempCName);
    //        _stmpl_main_container_tmpl.SetAttribute("TBWDataList", lstcontainers);
    //        sHTML = _stmpl_main_container_tmpl.ToString();
    //    }
    //    catch (System.Threading.ThreadAbortException)
    //    {
    //        // ignore it
    //    }
    //    catch (Exception ex)
    //    {
    //        sHTML = ex.Message;
    //    }
    //    return sHTML;
    //}


    protected string ST_bybrand()
    {
       
        StringTemplateGroup _stg_main_container = null;
        StringTemplateGroup _stg_records_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_main_container_tmpl = null;
        StringTemplate _stmpl_records_container_tmpl = null;
        StringTemplate _stmpl_records_tmpl = null;
        //StringTemplate _stmpl_records_tmpl2 = null;
        //StringTemplate _stmpl_records_tmpl3 = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];
        TBWDataList[] lstcontainers = new TBWDataList[3];
        DataSet dsCat;
        string[] filterval = null;
        string[] filterval1 = null;
        string[] filterval2 = null;

        //oPR = new ProductRender();
        string sHTML = string.Empty;
        // string dropdowncatid = "";
        // string _catid = "";
        // string _fid = "";
        int ictrecords = 0;


        try
        {
            stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";


            if (hidcatIds.Value != string.Empty && hidcatIds.Value != null)
            {
                filterval = hidcatIds.Value.Split('^');
            }
            if (HidsubcatIds.Value != string.Empty && HidsubcatIds.Value != null)
            {
                filterval1 = HidsubcatIds.Value.Split('^');
            }
            if (HidsubcatIds1.Value != string.Empty && HidsubcatIds1.Value != null)
            {
                filterval2 = HidsubcatIds1.Value.Split('^');
            }

            // if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")//&& Request.QueryString["cid"].ToString() == "WES210582")
            //{
            //string cid = Request.QueryString["cid"].ToString();

            //tempCID = Request.QueryString["cid"].ToString();
            //tempCName = GetCName(tempCID);
            dsCat = new DataSet();
            //if (HttpContext.Current.Session["MainMenuClick"] != null)
            //{
            //    dsCat.Tables.Add((DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"].Copy());
            //}
            if (HttpContext.Current.Session["Category_Attributes"] != null)
            {
                dsCat.Tables.Add((DataTable)((DataSet)HttpContext.Current.Session["Category_Attributes"]).Tables["Brand"].Copy());
            }
            if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables[0].Rows.Count == 0)
                return "";


            if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
            {
                tempCID = Request.QueryString["cid"].ToString();
                ictrecords = 0;
                lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");

                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                ictrecords++;
                bool selstate = false;
                string TOSUITE_BRAND = string.Empty;
                foreach (DataRow _drow in dsCat.Tables[0].Select("eapath <>''")  )
                {
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    //  _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_BRAND"].ToString());
                    // _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_BRAND"].ToString());
                    TOSUITE_BRAND=_drow["eapath"].ToString();
                 
                    //string stlistprodm = objHelperServices.SimpleURL_Str( CategoryName + "////" + _drow["TOSUITE_BRAND"].ToString().ToUpper(),"ct.aspx",false);
                    string stlistprodm = objHelperServices.SimpleURL_Str(CategoryName + "////" + TOSUITE_BRAND, "ct.aspx", false);
                    string NEWURL1 ="/"+stlistprodm + "/ct/";
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", NEWURL1);

                    if (Request.QueryString["tsb"] != null)
                    {
                        if (TOSUITE_BRAND.ToLower()  == Request.QueryString["tsb"].ToLower())
                        {
                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                        }
                    }
                    //if (filterval != null && _drow["TOSUITE_BRAND"].ToString().ToLower() == filterval[0].ToString().ToLower())
                    //{
                    //    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                    //    selstate = true;
                    //    string eapath="";
                    //    //eapath = HttpContext.Current.Session["EA"].ToString();
                    //    //if (eapath.Contains("////AttribSelect=Brand"))
                    //    //{
                    //    //    int inx = eapath.IndexOf("////AttribSelect=Brand");

                    //    //    eapath = eapath.ToString().Substring(0,inx);


                    //    //}                            
                    //    //eapath = eapath + "////AttribSelect=Brand='" + _drow["TOSUITE_BRAND"].ToString() + "'";

                    //    eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_drow["EA_PATH"].ToString()));

                    //    string[] parentcategory = _drow["EA_PATH"].ToString().Split (new string[] { "////" }, StringSplitOptions.None);
                    //    string parentcat = parentcategory[2];
                    //    //modified by:indu 
                    //    //   Response.Redirect("ct.aspx?&ld=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&bypcat=1&path=" + eapath, false);

                    //    //Modified by:Indu
                    //    string originalurl = "/ct.aspx?&ld=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&bypcat=1&path=" + eapath;

                    //    //string stlistprod = objHelperServices.URLRewriteToAddressBar("ct.aspx", _drow["TOSUITE_BRAND"].ToString().ToUpper(), originalurl, HttpContext.Current.Server.MapPath("URL_Rewrite_Cat.ini"), true);
                    //    //Modified by:Indu
                    //    // string stlistprod = objHelperServices.Cons_NewURl_bybrand( _drow["TOSUITE_BRAND"].ToString().ToUpper(), originalurl, "");

                    //    //string stlistprod = objHelperServices.Cons_NewURl_bybrand(originalurl, parentcat+"////"+"Brand="+ _drow["TOSUITE_BRAND"].ToString().ToUpper(), "ct.aspx", "");

                    //    string stlistprod = objHelperServices.SimpleURL (originalurl, parentcat + "////" + _drow["TOSUITE_BRAND"].ToString().ToUpper(),"ct.aspx",false,"");
                    //    string NEWURL = "/" + stlistprod + "/ct/";
                    //   // _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", NEWURL);
                    //    //_stmpl_pages.SetAttribute("TBT_URL", stlistprod);
                    //    hfisselected.Value = "0"; 
                    //     Response.Redirect(NEWURL,false);
                    //    Session["PageRawURL"] = NEWURL.ToLower().Replace("ct/","ct.aspx?").Replace("pl/","pl.aspx?").Replace("fl/","fl.aspx?").Replace("bb/","bb.aspx?").Replace("pd/","pd.aspx?").Replace("ps/","ps.aspx?")    ;
                    //}
                    //else if (filterval == null && Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "" && _drow["TOSUITE_BRAND"].ToString().ToLower() == Server.UrlDecode(Request.QueryString["tsb"].ToString().ToLower()) && selstate == false)
                    //{
                    //    filterval = new string[2];
                    //    filterval[0] = _drow["TOSUITE_BRAND"].ToString();
                    //    filterval[1] = _drow["TOSUITE_BRAND"].ToString();
                    //    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                    //    // Response.Redirect("ct.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                    //}
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", TOSUITE_BRAND);
                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    ictrecords++;

                }
                _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

            }
            if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
            {
                tempCID = Request.QueryString["cid"].ToString();
                bool selstate1 = false;
                DataSet dsCat1 = new DataSet();
                //dsCat1 = oCat.GetWESModel(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString()));
                //dsCat1 = EasyAsk.GetWESModel(tempCName, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString())); 
                dsCat1 = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
                if (dsCat1 != null && dsCat1.Tables.Count > 0 && (dsCat1.Tables[0].Rows.Count > 0))
                {
                    ictrecords = 0;
                    DataRow[] _DCRow = null;
                    _DCRow = dsCat1.Tables[0].Select();
                    if (_DCRow != null && _DCRow.Length > 0)
                    {
                        lstrecords = new TBWDataList[_DCRow.Length + 1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;
                        foreach (DataRow _drow in _DCRow)
                        {
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                            string stlistprodm = objHelperServices.SimpleURL_Str( CategoryName + "////" + Request.QueryString["tsb"] + "////" + _drow["TOSUITE_MODEL"].ToString(), "bb.aspx",false);
                            string NEWURL1 ="/"+ stlistprodm + "/bb/";
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", NEWURL1);
                            //  _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_MODEL"].ToString());
                            //if (filterval1 != null && _drow["TOSUITE_MODEL"].ToString() == filterval1[0].ToString())
                            //{
                            //    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            //    selstate1 = true;
                            //    // Response.Redirect("pl.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                            //    string eapath = string.Empty;
                            //    //eapath = HttpContext.Current.Session["EA"].ToString();
                            //    //if (eapath.Contains("////AttribSelect=Model"))
                            //    //{
                            //    //    int inx = eapath.IndexOf("////AttribSelect=Model");

                            //    //    eapath = eapath.ToString().Substring(0, inx );


                            //    //}
                            //    //eapath = eapath + "////AttribSelect=Model = '" + Request.QueryString["tsb"].ToString() + ":" + _drow["TOSUITE_MODEL"].ToString() + "'";

                            //    eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_drow["EA_PATH"].ToString()));
                            //    string[] parentcategory = _drow["EA_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                            //    string parentcat = parentcategory[2];

                            //    //if (Request.QueryString["cid"] != null)
                            //    //{
                            //    //    DataTable dt1 = (DataTable)HttpContext.Current.Session["dtcid"];

                            //    //    DataRow[] foundRows;
                            //    //    string cid = Request.QueryString["cid"].ToString();
                            //    //    foundRows = dt1.Select("cid='" + cid + "' ");
                            //    //    if (foundRows.Length > 0)
                            //    //    {
                            //    //        parentcat = foundRows[0][0].ToString();

                            //    //    }
                            //    //}


                            //    //Modified By:Indu
                            //    //  Response.Redirect("bb.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&bypcat=1&path=" + eapath, false);
                            //    //Modified by:indu
                            //    string originalurl = "/bb.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&bypcat=1&path=" + eapath;

                            //    //string stlistprod = objHelperServices.URLRewriteToAddressBar("bb.aspx", _drow["TOSUITE_MODEL"].ToString().ToUpper() + "/" + Request.QueryString["tsb"].ToString().ToUpper(), originalurl, HttpContext.Current.Server.MapPath("URL_Rewrite_Brand.ini"), true);
                            //    string model =HttpUtility.UrlEncode(_drow["TOSUITE_MODEL"].ToString().ToUpper());
                            //        //.Replace("+", "||").Replace("&", "^^").Replace(":", "~`");; 
                            //   // string stlistprod = objHelperServices.Cons_NewURl_bybrand(originalurl, parentcat + "////" + Request.QueryString["tsb"].ToString().ToUpper() + "////" + model, "bb.aspx", "");
                            //    string stlistprod = objHelperServices.SimpleURL (originalurl, parentcat + "////" + Request.QueryString["tsb"].ToString().ToUpper() , "bb.aspx", false,"");
                            //    string NEWURL = "/" + stlistprod + "/bb/";
                            //    //_stmpl_pages.SetAttribute("TBT_URL", stlistprod);
                            //     Response.Redirect(NEWURL,false);
                            //    Session["PageRawURL"] = NEWURL.Replace("bb/","bb.aspx?");
                            //}
                            //else if (filterval1 == null && Request.QueryString["tsm"] != null && _drow["TOSUITE_MODEL"].ToString() == Server.UrlDecode(Request.QueryString["tsm"].ToString()) && selstate1 == false)
                            //{
                            //    filterval1 = new string[2];
                            //    filterval1[0] = _drow["TOSUITE_MODEL"].ToString();
                            //    filterval1[1] = _drow["TOSUITE_MODEL"].ToString();
                            //    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                            //}
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_MODEL"].ToString());
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;

                        }
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                        lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    }
                }
                dsCat1.Dispose();
            }
            else
            {
                lstrecords = new TBWDataList[1];
                _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
            }



            // }
            //}
            _stg_main_container = new StringTemplateGroup("searchrsltmultilistmain", stemplatepath);
            _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistmain");
            _stmpl_main_container_tmpl.SetAttribute("TBW_CATEGORY_NAME", tempCName);
            _stmpl_main_container_tmpl.SetAttribute("TBWDataList", lstcontainers);
            sHTML = _stmpl_main_container_tmpl.ToString();
        }
        catch (System.Threading.ThreadAbortException)
        {
            // ignore it
        }
        catch (Exception ex)
        {
            sHTML = ex.Message;
        }
       


        return sHTML;
    }
    private void SetCookie(string supplierName, string supplierId)
       {
         
               HttpCookie LoginInfoCookie = new HttpCookie("ActiveSupplier");
               LoginInfoCookie["SupplierName"] = objSecurity.StringEnCrypt_password(supplierName);
               LoginInfoCookie["SupplierID"] = objSecurity.StringEnCrypt_password(supplierId);
               
               
               LoginInfoCookie.Expires = DateTime.Now.AddDays(999);
               HttpContext.Current.Response.AppendCookie(LoginInfoCookie);
         
       }
       //public void AssignCategoryEA()
       //{

        

       //    //string querystring = string.Empty;

       //    //if ((Request.QueryString["path"] != null) && (hfisselected.Value == string.Empty))
       //    //{
       //   // querystring = Request.RawUrl.ToString().ToLower();
       //    DataSet mainds = new DataSet();
       //    DataTable dt=new DataTable() ;
       //    try
       //    {
       //        string[] ConsURL = querystring.Split('/');
              
            
       //        if (HttpContext.Current.Session["MainCategory"] != null)
       //        {
       //            mainds = (DataSet)HttpContext.Current.Session["MainCategory"];
       //        }
       //        else
       //        {
       //            string strxml = HttpContext.Current.Server.MapPath("xml");

       //            mainds.ReadXml(strxml + "\\" + "mainds.xml");
       //            HttpContext.Current.Session["MainCategory"] = mainds;
       //        }
       //        dt = mainds.Tables[0];
       //        DataRow[] foundRows;

       //        foundRows = dt.Select("URL_RW_PATH='" + ConsURL[ConsURL.Length - 3] + "' ");
       //        if (foundRows.Length > 0)
       //        {
       //            _catCid = foundRows[0]["CATEGORY_ID"].ToString();
       //            CategoryName = foundRows[0]["CATEGORY_NAME"].ToString();
       //            Session["EA"] = foundRows[0]["EA_PATH"].ToString();
       //        }

       //    }
       //    catch
       //    { }
       //    finally
       //    {
       //        mainds.Dispose();
       //        dt.Dispose(); 
       //    }
          

       //}
    public void AssignSubdsEApath(string FamilyId)
       {
           DataSet subds = new DataSet();
           try
           {
               if (HttpContext.Current.Session["SubCategory"] != null)
               {

                   subds = (DataSet)HttpContext.Current.Session["SubCategory"];
               }

               else
               {
                   string strxml = HttpContext.Current.Server.MapPath("xml");

                   subds.ReadXml(strxml + "\\" + "subds.xml");
                   HttpContext.Current.Session["SubCategory"] = subds;

               }
               DataTable dt = subds.Tables[0];
               DataRow[] foundRows;
               //string querystring = Request.RawUrl.ToString().ToLower();
               string[] ConsURL = querystring.Split('/');
               Array.Reverse(ConsURL);
               string urlpath = string.Empty;
               if (Request.RawUrl.Contains("/pd/"))
               {

                   urlpath = ConsURL[5] + "/" + ConsURL[4];
               }
               else if (Request.RawUrl.Contains("/fl/"))
               {

                   if (ConsURL[2].Contains("wa"))
                   {
                       urlpath = ConsURL[5] + "/" + ConsURL[4];
                   }
                   else
                   {
                       urlpath = ConsURL[4] + "/" + ConsURL[3];
                   }
               }

               foundRows = dt.Select("URL_RW_PATH='" + urlpath + "' ");

               string prevEA = string.Empty;
               if (Session["EA"] != null)
               {
               prevEA = Session["EA"].ToString();
               }
                
               if (foundRows.Length > 0)
               {
                   Session["EA"] = foundRows[0]["EA_PATH"].ToString() + "////" + foundRows[0]["TBT_PARENT_CATEGORY_NAME"].ToString() + "////" + foundRows[0]["CATEGORY_NAME"].ToString();
                   // _catCid = foundRows[0]["CATEGORY_ID"].ToString();





                   if (querystring.Contains("/fl/") && prevEA.Contains(Session["EA"].ToString()) && !prevEA.ToLower().Contains("family id=") && !prevEA.ToLower().Contains("prod id="))
                       {
                           
                           Session["EA"] = prevEA;
                       }
                   else if (querystring.Contains("/pd/") )
                       {

                      
                           if (prevEA.Contains(Session["EA"].ToString()) && prevEA.Contains(FamilyId))
                           {
                               Session["EA"] = prevEA;
                               //objErrorHandler.CreateLog_new("AssignSubdsEApath" + prevEA);
                           }
                           else if (prevEA.Contains(Session["EA"].ToString()) && !prevEA.ToLower().Contains("family id="))
                           {
                               Session["EA"] = prevEA + "UserSearch=Family Id=" + FamilyId;
                               //objErrorHandler.CreateLog_new("AssignSubdsEApath1" + Session["EA"]);
                           }
                           else
                           {
                               Session["EA"] = Session["EA"] + "////" + "UserSearch=Family Id=" + FamilyId;
                              // objErrorHandler.CreateLog_new("AssignSubdsEApath2" + Session["EA"]);
                           }

                             
                       }

                  
               }
               else
               {

                   if (querystring.Contains("/pd/") && (prevEA.Contains(FamilyId)))
                   {
                      
                       Session["EA"] = prevEA;
                   }
                   else
                   {
                       Session["EA"] = "AllProducts////WESAUSTRALASIA////";
                   }
                  

               }

           }
           catch
           {
               Session["EA"] = "AllProducts////WESAUSTRALASIA////";
           }
           finally
           {
               subds.Dispose(); 
           }


       }
       public string GetSubCategoryEA()
       {
           // string querystring = Request.RawUrl.ToString().ToLower(); 
           DataSet subds = new DataSet();
           try
           {

             
               if (HttpContext.Current.Session["SubCategory"] != null)
               {

                   subds = (DataSet)HttpContext.Current.Session["SubCategory"];
               }
               else
               {
                   string strxml = HttpContext.Current.Server.MapPath("xml");

                   subds.ReadXml(strxml + "\\" + "subds.xml");
                   HttpContext.Current.Session["SubCategory"] = subds;
               }
               DataTable dt = subds.Tables[0];
               DataRow[] foundRows;
              
               string[] ConsURL = querystring.Split('/');
               Array.Reverse(ConsURL);
               string urlpath = string.Empty;
               if (ConsURL.Length == 5)
               {
                   urlpath = ConsURL[3] + "/" + ConsURL[2];
               }
               else if (ConsURL.Length > 5)
               {
                   urlpath = ConsURL[4] + "/" + ConsURL[3];
               }
              
               foundRows = dt.Select("URL_RW_PATH='" + urlpath + "' ");
               if (foundRows.Length > 0)
               {
                   Session["EA"] = foundRows[0]["EA_PATH"].ToString() + "////" + foundRows[0]["TBT_PARENT_CATEGORY_NAME"].ToString();
                   _catCid = foundRows[0]["CATEGORY_ID"].ToString();

                   return foundRows[0]["CATEGORY_NAME"].ToString();
               }
               else
               {

                 
                    if(ConsURL.Length < 5)
               {
                   Response.RedirectPermanent("/" + ConsURL[2] + "/ps/");
               }
                   if (ConsURL[3] != "")
                   {
                       Response.RedirectPermanent("/" + ConsURL[3] + "/ps/");
                   }
                   else
                   {
                       Response.RedirectPermanent("/404New.html" + "/ps/");
                   }
                   return "";
               }

           }
           catch (ThreadAbortException)
           {
               return "";
           }
           catch
           {
               //objErrorHandler.CreateLog("Error_getSubCategoryEA" + querystring);
               Response.RedirectPermanent("/404New.htm");   
               return "";

           }
            
           finally
           {
               subds.Dispose(); 
           }
       }


       public void AssignSubdsEApath_MS(string FamilyId)
       {
         //  string querystring = Request.RawUrl.ToString().ToLower();
           DataSet subds = new DataSet();
           try
           {
               
               if (HttpContext.Current.Session["SubCategory"] != null)
               {

                   subds = (DataSet)HttpContext.Current.Session["SubCategory"];
               }

               else
               {
                   string strxml = HttpContext.Current.Server.MapPath("xml");

                   subds.ReadXml(strxml + "\\" + "subds.xml");
                   HttpContext.Current.Session["SubCategory"] = subds;
               }
               DataTable dt = subds.Tables[0];
               DataRow[] foundRows;
           
               string[] ConsURL = querystring.Split('/');
               //  Array.Reverse(ConsURL);
               string urlpath = string.Empty;
               //if (Request.RawUrl.Contains("/pd/"))
               //{

               urlpath = ConsURL[2] + "/" + ConsURL[1];
               //}
               //else if (Request.RawUrl.Contains("/fl/"))
               //{

               //    if (ConsURL[2].Contains("wa"))
               //    {
               //        urlpath = ConsURL[5] + "/" + ConsURL[4];
               //    }
               //    else
               //    {
               //        urlpath = ConsURL[4] + "/" + ConsURL[3];
               //    }
               //}
               string prevEA = string.Empty;
               if (Session["EA"] != null)
               {
                   prevEA = Session["EA"].ToString();
               }
                
               foundRows = dt.Select("URL_RW_PATH='" + urlpath + "' ");
               if (foundRows.Length > 0)
               {
                   Session["EA"] = foundRows[0]["EA_PATH"].ToString() + "////" + foundRows[0]["TBT_PARENT_CATEGORY_NAME"].ToString() + "////" + foundRows[0]["CATEGORY_NAME"].ToString();
                   // _catCid = foundRows[0]["CATEGORY_ID"].ToString();

                   if (prevEA.Contains(Session["EA"].ToString()) && !prevEA.ToLower().Contains("family id") && !prevEA.ToLower().Contains("prod id="))
                   {

                       Session["EA"] = prevEA;
                   }
                  else if (querystring.Contains("/pd/") )
                       {

                          
                                   if( prevEA.ToLower().Contains("family id") && prevEA.Contains(Session["EA"].ToString()))
                                   {
                                       Session["EA"] = prevEA;
                                   }
                                   else 
                                   {
                                    Session["EA"] =  Session["EA"]     + "////" + "UserSearch=Family Id=" + FamilyId; 
                                   }
                             
                       }


               }
               else
               {

                   Session["EA"] = "AllProducts////WESAUSTRALASIA////"+CategoryName;
               }

           }
           catch (Exception ex)
           {
  //objErrorHandler.CreateLog("Error_AssignSubdsEAPath_MS" + querystring +ex.ToString() );
              
           }
          
           finally
           {
               subds.Dispose();
           }



       }
       public string GetSubCategoryEA_MS()
       {
          DataSet subds = new DataSet();
           try
           {
               
               if (HttpContext.Current.Session["SubCategory"] != null)
               {

                   subds = (DataSet)HttpContext.Current.Session["SubCategory"];
               }
               else
               {
                   string strxml = HttpContext.Current.Server.MapPath("xml");

                   subds.ReadXml(strxml + "\\" + "subds.xml");
                   HttpContext.Current.Session["SubCategory"] = subds;
               }
               DataTable dt = subds.Tables[0];
               DataRow[] foundRows;
             
               string[] ConsURL = querystring.Split('/');
               // Array.Reverse(ConsURL);

               string urlpath = string.Empty;
               //if (ConsURL.Length == 5)
               //{
               urlpath = ConsURL[2] + "/" + ConsURL[1];
               //}
               //else
               //{
               //urlpath = ConsURL[5] + "/" + ConsURL[4];
               //}
               foundRows = dt.Select("URL_RW_PATH='" + urlpath + "' ");
               if (foundRows.Length > 0)
               {
                   Session["EA"] = foundRows[0]["EA_PATH"].ToString() + "////" + foundRows[0]["TBT_PARENT_CATEGORY_NAME"].ToString();
                   _catCid = foundRows[0]["TBT_PARENT_CATEGORY_ID"].ToString();

                   return foundRows[0]["CATEGORY_NAME"].ToString();
               }
               else
               {
                 
                   return "";
               
               }
              

           }
           catch (Exception ex)
           {
               //objErrorHandler.CreateLog("GetSubCategoryEA_MS" + querystring + ex.ToString());
            
               return "";
           }
         
           finally
           {
               subds.Dispose();
           }

       }


       public void Get2SubCatEA_MS(string FamilyId)
       {

           try
           {
               //if (Session["FamilyProduct"] != null)
               //{
               //    DataSet familyds = (DataSet)HttpContext.Current.Session["FamilyProduct"];

               //    DataTable dt = familyds.Tables["FamilyPro"];
               //    if (dt.Columns.Contains("Category_Path"))
               //    {
               //        DataRow[] foundRows;

               //        foundRows = dt.Select("Family_Id='" + FamilyId + "' ");
               //        if (foundRows.Length > 0)
               //        {
               //            string[] catpath = foundRows[0]["Category_Path"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
               //            if (catpath.Length >= 2)
               //            {
               //                Session["EA"] = EA_ROOT_CATEGORY_PATH + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >= 2 ? catpath[1] : " ");
               //            }
               //            else
               //            {
               //                AssignSubdsEApath_MS();
               //            }

               //        }
               //        else
               //        {
               //            AssignSubdsEApath_MS();

               //        }
               //    }
               //    else
               //    {
               //        AssignSubdsEApath_MS();
               //    }

               //}

               //else
               //{
                   AssignSubdsEApath_MS(FamilyId);
                   //DataTable Sqltb = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, FamilyId, "", "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                   //if (Sqltb.Rows.Count > 0)
                   //{

                   //    Session["EA"] = EA_ROOT_CATEGORY_PATH +"////"+ Sqltb.Rows[0]["Category_Path"] .ToString();
                   //}
               //}
           }
           catch
           {
           
           }
       }

       public void Get2SubCatEA(string FamilyId)
       {
           try
           {
               if (Session["FamilyProduct"] != null)
               {
                  
                   DataSet familyds = (DataSet)HttpContext.Current.Session["FamilyProduct"];

                   DataTable dt = familyds.Tables["FamilyPro"];
                   if (dt.Columns.Contains("Category_Path"))
                   {
                       DataRow[] foundRows;

                       foundRows = dt.Select("Family_Id='" + FamilyId + "' ");
                       if (foundRows.Length > 0)
                       {
                           //string[] catpath= foundRows[0]["Category_Path"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);


                           if (foundRows[0]["Category_Path"].ToString() != "")
                           {
                               string prevEA = string.Empty;
                               if (Session["EA"] != null)
                               {
                               prevEA = Session["EA"].ToString();
                               }
                             //  Session["EA"] = EA_ROOT_CATEGORY_PATH + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ");

                               Session["EA"] = EA_ROOT_CATEGORY_PATH + "////" + foundRows[0]["Category_Path"].ToString();
                              // objErrorHandler.CreateLog_new("inside familyds" + foundRows[0]["Category_Path"].ToString());
                               if (querystring.Contains("/fl/") && prevEA.Contains(Session["EA"].ToString()) && !prevEA.ToLower().Contains("family id=") && !prevEA.ToLower().Contains("prod id="))
                               {

                                   Session["EA"] = prevEA;
                               }
                               else if (querystring.Contains("/pd/"))
                               {


                                   if (prevEA.Contains(Session["EA"].ToString()) && prevEA.Contains(FamilyId))
                                   {
                                      // objErrorHandler.CreateLog_new("inside prevEA" + prevEA);
                                       Session["EA"] = prevEA;
                                   }
                                   else if (prevEA.Contains(Session["EA"].ToString()) && !prevEA.ToLower().Contains("family id="))
                                   {
                                       Session["EA"] = prevEA + "////" + "UserSearch=Family Id=" + FamilyId;
                                      // objErrorHandler.CreateLog_new("inside prevEA1" + prevEA);
                                   }
                                   else
                                   {
                                       Session["EA"] = Session["EA"] + "////" + "UserSearch=Family Id=" + FamilyId;
                                      // objErrorHandler.CreateLog_new("inside session" + Session["EA"]);
                                   }

                               }
                                    
                               }
                              
                             
                               
                         
                           else
                           {
                               AssignSubdsEApath(FamilyId);
                           }


                          
                       }
                       else
                       {
                           AssignSubdsEApath(FamilyId);

                       }
                   }
                   else
                   {
                       AssignSubdsEApath(FamilyId);
                   }

               }

               else
               {
                   AssignSubdsEApath(FamilyId);
                   //DataTable Sqltb = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, FamilyId, "", "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                   //if (Sqltb.Rows.Count > 0)
                   //{

                   //    Session["EA"] = EA_ROOT_CATEGORY_PATH +"////"+ Sqltb.Rows[0]["Category_Path"] .ToString();
                   //}
               }
           }
           catch
           { }
       }

       public string GetCorrectBrand(string tsb)
       {
 DataSet subds = new DataSet();
           try
           {
              
               if (HttpContext.Current.Session["Category_Attributes"] != null)
               {

                   subds = (DataSet)HttpContext.Current.Session["Category_Attributes"];
               }
               else
               {
                   string strxml = HttpContext.Current.Server.MapPath("xml");

                   subds.ReadXml(strxml + "\\" + _catCid + "_Att.xml");

               }

               DataTable dt = subds.Tables["brand"];
               DataRow[] foundRows;

               foundRows = dt.Select("URL_RW_PATH='" + tsb + "' ");
               if (foundRows.Length > 0)
               {
                   return foundRows[0]["eaPath"].ToString();
               }
               else
               {
                   return tsb;
               }
           }
           catch (Exception ex)
           {

               return tsb;
           }
          
           finally
           {
               subds.Dispose();
           }
       }

       public string GetCorrectModel(string tsm)
       {
 DataSet subds = new DataSet();
           try
           {
              
               //if (HttpContext.Current.Session["Category_Attributes"] != null)
               //{

               //    subds = (DataSet)HttpContext.Current.Session["Category_Attributes"];
               //}
               //else
               //{
                   string strxml = HttpContext.Current.Server.MapPath("xml");

                   subds.ReadXml(strxml + "\\" + _catCid + "_Att.xml");

               //}

               DataTable dt = subds.Tables["models"];
               DataRow[] foundRows;

               foundRows = dt.Select("URL_RW_PATH='" + tsm + "' ");
               if (foundRows.Length > 0)
               {
                   return foundRows[0]["eaPath"].ToString();
               }
               else
               {
                   return tsm;
               }
           }
           catch (Exception ex)
           {

               return tsm;
           }
        
           finally
           {
               subds.Dispose();
           }
       }

       //public void callms_bybrand()
       //{
       //    if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
       //        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());




       //    if (Session["hfclickedattr_bb"] == null)
       //    {
       //        if (checkurl[2].Contains("-"))
       //        {

       //            _tsb = GetCorrectBrand(checkurl[2]);
       //        }
       //        else
       //        {
       //            _tsb = checkurl[2];

       //        }
       //        if (checkurl[1].Contains("-"))
       //        {

       //            _tsm = GetCorrectModel(checkurl[2] + "-" + checkurl[1]);
       //            _tsm = _tsm.ToLower().Replace(_tsb.ToLower() + ":", "");
       //        }
       //        else
       //        {
       //            _tsm = checkurl[1];

       //        }


       //        if (checkurl.Length == 6)
       //        {
       //            Session["EA"] = EA_ROOT_CATEGORY_PATH + "////" + "CELLULAR ACCESSORIES" + "////" + "AttribSelect=Brand='" + _tsb + "'";
       //            EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
       //        }
       //        else
       //        {


       //            string[] getEA = rawurl.Split(new string[] { "/mwa-" }, StringSplitOptions.None);
       //            string hffield = getEA[1];
       //            string[] hffield1 = hffield.Split('/');

       //            string EA = string.Empty;
       //            string hffieldfinal = _tsm + "mwa-" + hffield1[0];
       //            string _value1 = string.Empty;
       //            if (Session[hffieldfinal] != null)
       //            {
       //                string hfvalue = Session[hffieldfinal].ToString();
       //                string[] setEA = hfvalue.Split(new string[] { "////" }, StringSplitOptions.None);
       //                string attr = setEA[setEA.Length - 1];

       //                string[] gettype = null;

       //                gettype = attr.Split('=');
       //                if (gettype.Length > 1)
       //                {
       //                    _type = gettype[1];
       //                    _value = gettype[2];
       //                    _value = _value.Substring(2, _value.Length - 3);
       //                }
       //                else
       //                {
       //                    _type = "category";
       //                    _value = gettype[0];
       //                }

       //                Session["EA"] = hfvalue.Replace("////" + attr, "");
       //                Session[hffieldfinal] = null;
       //                EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

       //            }
       //            else
       //            {

       //                EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
       //            }
       //        }
       //        // EasyAsk.GetBrandAndModelProducts(CategoryName, _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
       //        Context.RewritePath("/bb.aspx?&id=0&cid=" + _catCid + "&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

       //        if (!(_isorgurl))
       //        {
       //            if (requrl.Contains("mbb.aspx"))
       //                Context.RewritePath("/mbb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
       //            else
       //                Context.RewritePath("/bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
       //        }
       //    }

       //    else
       //    {

       //        string[] gettype = null;
       //        if (Session["hfclickedattr_bb"] != null)
       //        {
       //            gettype = Session["hfclickedattr_bb"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
       //        }

       //        if (gettype[0] != "")
       //        {
       //            Session["EA"] = gettype[0];
       //        }

       //        _type = gettype[1];
       //        _value = gettype[2];
       //        if (_value.Contains("::"))
       //        {
       //            gettype = _value.Split(new string[] { "::" }, StringSplitOptions.None);
       //            _bname = gettype[0];
       //            _value = gettype[1];
       //        }
       //        Session["hfclickedattr_bb"] = null;

       //        EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
       //        if (!(_isorgurl))
       //        {
       //            if (requrl.Contains("mbb.aspx"))
       //                Context.RewritePath("/mbb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&type=" + _type + "&value=" + HttpUtility.UrlEncode(_value) + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
       //            else
       //                Context.RewritePath("/bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&type=" + _type + "&value=" + HttpUtility.UrlEncode(_value) + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

       //        }
       //    }



       //}

}

