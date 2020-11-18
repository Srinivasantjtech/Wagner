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

public partial class UC_maincategory : System.Web.UI.UserControl
{


    #region "Declarations"


    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    Security objSecurity = new Security();
    UserServices objUserServices = new UserServices();
    CategoryServices objCategoryServices = new CategoryServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
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

        if (((reqrawurl.Contains("_")) || reqrawurl.Contains("&") || reqrawurl.Contains("="))
            && !(reqrawurl.Contains("/ct/")) && !(reqrawurl.Contains("/bb/")))
        {
            pageload();
        }
        else
        {
            pageload_new();
        }

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

                    string querystring = string.Empty;

                    if ((Request.QueryString["path"] != null) && (hfisselected.Value == string.Empty))
                    {
                        querystring = Request.RawUrl.ToString();

                    }
                    else
                    {
                        querystring = Request.RawUrl.ToString();
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
    private void pageload_new()
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

            if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
            {
                _catCid = Request.QueryString["cid"];
            }


            else
            {
                string newquerystring = string.Empty;
                string requrl = Request.Url.ToString().ToLower();

                if ((requrl.Contains("pl.aspx")) ||
              (requrl.Contains("fl.aspx")) ||
               (requrl.Contains("pd.aspx")) ||
              (requrl.Contains("ct.aspx")) ||
              (requrl.Contains("microsite.aspx")) ||
              (requrl.Contains("bb.aspx"))
                || (requrl.Contains("mct.aspx") == true)
                    || (requrl.Contains("mpl.aspx"))
                    || (requrl.Contains("mps.aspx"))
                    || (requrl.Contains("mfl.aspx"))
                    || (requrl.Contains("mpd.aspx"))
                    )
                {

                    string querystring = string.Empty;

                    if ((Request.QueryString["path"] != null) && (hfisselected.Value == string.Empty))
                    {
                        querystring = Request.RawUrl.ToString().ToLower();

                        if (
                        querystring.Contains("/mfl/") || querystring.Contains("mfl.aspx")
               || querystring.Contains("/mpl/") || querystring.Contains("mpl.aspx")
               || querystring.Contains("/mpd/") || querystring.Contains("mpd.aspx")
            || querystring.Contains("/mps/") || querystring.Contains("mps.aspx"))
                        {

                            querystring = objHelperServices.URlStringReverse_MS(Request.RawUrl.ToString().ToLower());
                        }
                        else
                        {
                            querystring = objHelperServices.URlStringReverse(Request.RawUrl.ToString().ToLower());
                        }


                        newquerystring = querystring;
                        querystring = querystring.Replace("/ct/", "ct.aspx?").Replace("/pl/", "pl.aspx?").Replace("/fl/", "fl.aspx?").
                            Replace("/bb/", "bb.aspx?").Replace("/pd/", "pd.aspx?").Replace("/ps/", "ps.aspx?").
                            Replace("/microsite/", "microsite.aspx?");
                        querystring = querystring.Replace("/mct/", "mct.aspx?").Replace("/mpl/", "mpl.aspx?").Replace("/mfl/", "mfl.aspx?").
                            Replace("/mbb/", "mbb.aspx?").Replace("/mpd/", "mpd.aspx?").Replace("/mps/", "mps.aspx?");
                    }
                    //Added by indu for new urlrewrite
                    //else if (hfisselected.Value != "")
                    //{

                    //    querystring = Request.Url.PathAndQuery .ToString().ToLower();

                    //   // querystring = objHelperServices.URlStringReverse(Request.RawUrl.ToString().ToLower());
                    //    querystring = querystring.Replace("ct/", "ct.aspx?").Replace("pl/", "pl.aspx?").Replace("fl/", "fl.aspx?").Replace("bb/", "bb.aspx?").Replace("pd/", "pd.aspx?").Replace("ps/", "ps.aspx?");
                    //}
                    //
                    else
                    {
                        querystring = Request.RawUrl.ToString().ToLower();
                        if (Request.RawUrl.ToString().ToLower().Contains(".aspx?"))
                        {
                            string[] newurl = Request.Url.PathAndQuery.ToString().Split(new string[] { ".aspx?" }, StringSplitOptions.None);
                            querystring = "/" + newurl[1] + newurl[0];
                        }

                        if (
                     querystring.Contains("/mfl/") || querystring.Contains("mfl.aspx")
            || querystring.Contains("/mpl/") || querystring.Contains("mpl.aspx")
            || querystring.Contains("/mpd/") || querystring.Contains("mpd.aspx")
         || querystring.Contains("/mps/") || querystring.Contains("mps.aspx"))
                        {

                            querystring = objHelperServices.URlStringReverse_MS(querystring);
                        }
                        else
                        {
                            querystring = objHelperServices.URlStringReverse(querystring);
                        }
                        newquerystring = querystring;
                        querystring = "/" + querystring;
                        querystring = querystring.ToLower().Replace("/ct/", "ct.aspx?").Replace("/pl/", "pl.aspx?").Replace("/fl/", "fl.aspx?").Replace("/bb/", "bb.aspx?").Replace("/pd/", "pd.aspx?").Replace("/ps/", "ps.aspx?").Replace("/microsite/", "microsite.aspx?");
                        querystring = querystring.ToLower().Replace("/mct/", "mct.aspx?").Replace("/mpl/", "mpl.aspx?").Replace("/mfl/", "mfl.aspx?").Replace("/mbb/", "mbb.aspx?").Replace("/mpd/", "mpd.aspx?").Replace("/mps/", "mps.aspx?");

                        string[] qs = querystring.Split('?');
                        querystring = qs[1];
                        querystring = querystring.Replace("||", "+");
                        //.Replace("``", "\"").Replace("&srctext=", "").Replace("%E2%80%9C", "\"").Replace("%E2%80%9D", "\"").Replace("\\","/");

                    }
                    string[] ConsURL = querystring.Split('/');
                    string SB_ConsURL = string.Empty;
                    //if (ConsURL[0].Contains("_") == true)
                    //{
                    //    DataSet ds = objHelperDB.GetOrgValue("", SB_ConsURL);
                    //    if (ds != null)
                    //    {
                    //        SB_ConsURL = ds.Tables[0].Rows[0][0].ToString();
                    //    }
                    //}
                    //else

                    //{
                    //    SB_ConsURL = ConsURL[0];
                    //}
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
                    if (_catCid == "")
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

                    //added to get catcid second time
                    //if (_catCid == "")
                    //{

                    //    querystring = objHelperServices.URlStringReverse(newquerystring);
                    //    querystring = querystring.Replace("/ct/", "ct.aspx?");
                    //    querystring = querystring.Replace("||", "+");
                    //    string[] ConsURL1 = querystring.Split('/');
                    //    DataRow[] foundRows1;

                    //    foundRows1 = dt.Select("category_name = '" + ConsURL1[0].ToLower().Replace("~..~", " / ").Replace("~..", " /").Replace("..~", "/ ").Replace(".~.", "/") + "' ");
                    //    if (foundRows1.Length > 0)
                    //    {
                    //        _catCid = foundRows1[0]["category_id"].ToString();

                    //    }
                    //}
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

            Get_Value_Breadcrum();
        }
        else
        {

            Get_Value_Breadcrum_New();
        }
    }
    protected void logoutsession(object sender, EventArgs e)
    {
        objUserServices.OnLineFlag(false, objHelperServices.CI(Session["USER_ID"]));
        Session.RemoveAll();
        Session.Clear();
        Session.Abandon();
        Session["USER_ID"] = "";
        Response.Redirect("/Login.aspx", false);
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





    protected string Get_Value_Breadcrum()
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
            string requrl = HttpContext.Current.Request.Url.ToString().ToLower();
            if (requrl.Contains("pd.aspx") == true)
            {
                if (HttpContext.Current.Session["Category_Attributes"] == null && _parentCatID != string.Empty)
                {
                    EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
                }
            }

            int ckbrand = 0;

            if ((Request.QueryString["path"] != null) && (hfisselected.Value == string.Empty))
            {
                HttpContext.Current.Session["EA"] = objSecurity.StringDeCrypt(Request.QueryString["path"].ToString());
                _isorgurl = true;
            }
            else if (!(requrl.Contains("login.aspx")) && !(requrl.Contains("home.aspx")) || !(requrl.Contains(".png")))
            {
                HttpContext.Current.Session["EA"] = "";
                string querystring = string.Empty;
                if (hfisselected.Value.Equals("1"))
                {

                    if (!(hforgurl.Value.Contains("BrandModel")))
                    {
                        querystring = hforgurl.Value;
                    }
                    else
                    {
                        querystring = hforgurl.Value.Replace("bb.aspx?", "");

                        StringBuilder SBquerystring = new StringBuilder(querystring);
                        SBquerystring.Replace('_', ' ');
                        SBquerystring.Replace("||", "+");
                        SBquerystring.Replace("``", "\"");
                        SBquerystring.Replace(" / ", "~..~");
                        SBquerystring.Replace(" /", "~..");
                        SBquerystring.Replace("/ ", "..~");
                        SBquerystring.Replace("./.", ".~.");
                        querystring = SBquerystring.ToString();
                        string[] ConsURL1 = querystring.Split(new string[] { "/" }, StringSplitOptions.None);
                        _tsb = ConsURL1[1];
                        _tsm = ConsURL1[2];
                        // Context.RewritePath("bb.aspx?" + querystring); 
                    }

                }
                else
                {
                    querystring = Request.RawUrl.ToString();

                    if (querystring.Contains("&gclid="))
                    {
                        string[] querystring1 = querystring.Split(new string[] { "&gclid=" }, StringSplitOptions.None);
                        querystring = querystring1[0];
                    }
                    else if (querystring.Contains("?gclid="))
                    {
                        string[] querystring1 = querystring.Split(new string[] { "?gclid=" }, StringSplitOptions.None);
                        querystring = querystring1[0];
                    }
                    if (querystring.EndsWith("/"))
                    {

                        querystring = querystring.Substring(0, querystring.Length - 1);
                    }
                    string[] qs = querystring.Split(new string[] { "?" }, StringSplitOptions.None);
                    if (qs.Length >= 1)
                    {
                        querystring = qs[1];
                    }

                    // querystring= HttpUtility.UrlDecode(Request.Url.Query.ToString().Replace("?", ""));
                }
                string dbq = HttpUtility.UrlDecode("%E2%80%9C");
                string dbq1 = HttpUtility.UrlDecode("%E2%80%9D");
                string dbq2 = HttpUtility.UrlDecode("%C3%98");
                StringBuilder SBquerystring1 = new StringBuilder(querystring);
                SBquerystring1.Replace('_', ' ');
                SBquerystring1.Replace("``", "\"");
                SBquerystring1.Replace("&srctext=", "");
                SBquerystring1.Replace("%E2%80%9C", dbq);
                SBquerystring1.Replace("%E2%80%9D", dbq1);
                SBquerystring1.Replace("%C3%98", dbq2);
                SBquerystring1.Replace(" / ", "~..~");
                SBquerystring1.Replace(" /", "~..");
                SBquerystring1.Replace("/ ", "..~");
                SBquerystring1.Replace("./.", ".~.");

                querystring = SBquerystring1.ToString();
                string[] ConsURL = querystring.Split('/');
                string ea = string.Empty;
                if (ConsURL.Length >= 2)
                {
                    ea = querystring.Replace(ConsURL[ConsURL.Length - 1], "");
                }
                else
                {
                    ea = querystring;
                }
                StringBuilder sb_ea = new StringBuilder(ea);
                sb_ea.Replace("~..~", " / ");
                sb_ea.Replace("~..", " /");
                sb_ea.Replace("..~", "/ ");
                sb_ea.Replace(".~.", "/");
                ea = sb_ea.ToString();
                // string ea = querystring;
                string newea = string.Empty;
                if (ea.Substring(ea.Length - 1, 1).Contains("/"))
                {
                    newea = ea.Substring(0, ea.Length - 1);
                }
                else
                {

                    newea = ea;
                }
                string eapath = string.Empty;
                int fid = 0;
                if (ConsURL.Length <= 1)
                {

                    // HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////Cellular Accessories////" + newea.Replace("/", "////");
                    //if (requrl.Contains("bb.aspx"))
                    //{
                    //    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////Cellular Accessories////" + "AttribSelect=Brand='" + newea.Replace("/", "////") + "'";
                    //    _tsb = newea.Replace("/", "////");
                    //}
                    if (requrl.Contains("powersearch.aspx"))
                    {

                        HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + "UserSearch=" + newea.Replace("/", "////").Replace("&srctext=", "");
                        _searchstr = newea.Replace("/", "////").Replace("&srctext=", "");
                    }
                    if (requrl.Contains("ct.aspx") && newea.ToLower().Contains("brand").Equals(true))
                    {
                        HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
                        _tsb = ea.Replace("BRAND=", "").Replace("Brand=", "");
                        _tsbnew = ea.Replace("BRAND=", "").Replace("Brand=", "");
                        _bypcat = "2";
                    }
                }
                else
                {
                    string brand = string.Empty;
                    for (int i = 0; i <= ConsURL.Length - 2; i++)
                    {
                        //ConsURL[i] = ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" ); 
                        StringBuilder SB_ConsURL = new StringBuilder(ConsURL[i]);

                        SB_ConsURL.Replace("~..~", " / ");
                        SB_ConsURL.Replace("~..", " /");
                        SB_ConsURL.Replace("..~", "/ ");
                        SB_ConsURL.Replace(".~.", "/");
                        if (ConsURL[i].ToString().Contains("="))
                        {

                            string[] Gettype = ConsURL[i].Split(new string[] { "=" }, StringSplitOptions.None);
                            StringBuilder SB_gtype = new StringBuilder(Gettype[1]);
                            SB_gtype.Replace("~..~", " / ");
                            SB_gtype.Replace("~..", " /");
                            SB_gtype.Replace("..~", "/ ");
                            SB_gtype.Replace(".~.", "/");
                            string gettype1 = SB_gtype.ToString();

                            if (IsNumber(Gettype[0]) == false)
                            {

                                //if (requrl.Contains("ct.aspx") == true  && Gettype[0].ToLower()!="brand")
                                //{
                                //    //_catCid = Gettype[0];
                                //    //hfcid.Value = _catCid;
                                //    _catname  = Gettype[1];


                                //}
                                string gettype0 = Gettype[0].ToLower();
                                if (gettype0 == "brand")
                                {
                                    brand = gettype1;
                                    _tsb = brand;
                                    _bname = brand;
                                    ckbrand = 1;
                                    _bypcat = "2";
                                }
                                if (gettype0 == "model")
                                {
                                    if (gettype1.ToString().Contains(":"))
                                    {
                                        string[] getbname = gettype1.Split(new string[] { ":" }, StringSplitOptions.None);

                                        eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + getbname[0].Replace("'", "") + ":" + getbname[1].Replace("'", "") + "'";
                                    }
                                    else
                                    {
                                        eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + brand + ":" + gettype1 + "'";
                                        _tsm = Gettype[1];
                                    }
                                    ckbrand = 2;
                                }

                                else if (gettype0 == "category")
                                {
                                    eapath = eapath + "////" + gettype1;
                                }
                                else if (gettype0 == "usersearch")
                                {
                                    eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
                                }
                                else if (gettype0 == "usersearch1")
                                {
                                    eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
                                }
                                else if (gettype0 == "usersearch2")
                                {
                                    eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
                                }
                                else if ((gettype0 == "USER SEARCH") && ((requrl.Contains("fl.aspx")) || (requrl.Contains("pd.aspx"))))
                                {
                                    eapath = eapath + "////" + "UserSearch" + "=" + gettype1;
                                }
                                //else if ((Gettype[0].ToLower() == "user SEARCH") && (requrl.Contains("fl.aspx")) || (requrl.Contains("pd.aspx")))
                                //{
                                //    eapath = eapath + "////" + "UserSearch" + "=" + gettype1;
                                //}
                                else
                                {
                                    //if (i == 0)
                                    //{
                                    //   if (requrl.Contains("powersearch.aspx") == true)
                                    //    {
                                    eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + gettype1 + "'";
                                    //}
                                    //else
                                    //{
                                    //   eapath = eapath + "////" + gettype1 ;

                                    //        hfcid.Value = Gettype[0];
                                    //        _catCid = Gettype[0];

                                    //}
                                    //}
                                    //else
                                    //{
                                    //    eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + gettype1 + "'";

                                    //}

                                }
                            }
                            else if (fid != 1)
                            {
                                fid = 1;
                                _fid = Gettype[0];
                                eapath = eapath + "////" + "UserSearch=Family Id=" + Gettype[0];
                            }
                            else if (fid == 1)
                            {
                                eapath = eapath + "////" + "UserSearch1=Prod Id=" + Gettype[0];
                            }
                            //if((requrl.Contains("fl.aspx"))||(requrl.Contains("pd.aspx")))
                            //{
                            //    eapath = eapath.Replace("user_search", "UserSearch").Replace("user search", "UserSearch").Replace("USER_SEARCH", "UserSearch").Replace("USER SEARCH", "UserSearch").Replace("User_Search", "UserSearch").Replace("User Search", "UserSearch");   

                            //}

                            //if (requrl.Contains("ct.aspx") == true && ea.ToString().ToLower().Contains("brand")==false   )
                            //{
                            //   // _catCid = Gettype[0];
                            //    _value = Gettype[];
                            //   // hfcid.Value = _catCid;

                            //}
                        }
                        else if (requrl.Contains("bb.aspx"))
                        {



                            if (ckbrand == 0)
                            {
                                eapath = eapath + SB_ConsURL.ToString();
                                _catname = SB_ConsURL.ToString();
                                //_tsb = ConsURL[i];
                                //_bname = ConsURL[i];
                                ckbrand = 1;
                            }
                            else if (ckbrand == 1)
                            {
                                //ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" )
                                eapath = eapath + "////" + "AttribSelect=Brand='" + SB_ConsURL.ToString() + "'";
                                _tsb = SB_ConsURL.ToString();
                                _bname = SB_ConsURL.ToString();
                                ckbrand = 2;
                            }
                            else if (ckbrand == 2)
                            {
                                //ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" )
                                eapath = eapath + "////" + "AttribSelect=Model='" + _bname + ":" + SB_ConsURL.ToString() + "'";
                                _tsm = SB_ConsURL.ToString();
                                ckbrand = 2;
                            }

                            else
                            {
                                //ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" )
                                eapath = eapath + "////" + SB_ConsURL.ToString();
                            }


                        }

                        else
                        {

                            if (i == 0 && requrl.Contains("powersearch.aspx"))
                            {
                                eapath = "UserSearch=" + SB_ConsURL.ToString();
                                _searchstr = SB_ConsURL.ToString(); ;
                            }
                            else
                            {
                                eapath = eapath + "////" + SB_ConsURL.ToString();
                            }
                        }

                    }
                    StringBuilder SB_EAPATH = new StringBuilder(eapath);
                    SB_EAPATH.Replace("?", "");
                    SB_EAPATH.Replace("~..~", " / ");
                    SB_EAPATH.Replace("~..", " /");
                    SB_EAPATH.Replace("..~", "/ ");
                    SB_EAPATH.Replace(".~.", "/");
                    SB_EAPATH.Replace("||", "+");
                    SB_EAPATH.Replace("^^", "&");
                    SB_EAPATH.Replace("~`", ":");
                    SB_EAPATH.Replace("~^", ".");

                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + SB_EAPATH.ToString();

                }
                StringBuilder sb_value = new StringBuilder(ConsURL[ConsURL.Length - 1]);
                sb_value.Replace('_', ' ');
                sb_value.Replace("~..~", " / ");
                sb_value.Replace("~..", " /");
                sb_value.Replace("..~", "/ ");
                sb_value.Replace(".~.", "/");
                sb_value.Replace("||", "+");
                sb_value.Replace("^^", "&");
                sb_value.Replace("~`", ":");
                _value = sb_value.ToString();
                if (_value.Contains("="))
                {
                    string[] Gettype = _value.Split(new string[] { "=" }, StringSplitOptions.None);
                    string AttribSelect = string.Empty;
                    if (IsNumber(Gettype[0]) == false)
                    {

                        _type = Gettype[0];
                        _value = Gettype[1];

                        //AttribSelect = "AttribSelect=" + _type + "='" + _value + "'";

                        //if (requrl.Contains("ct.aspx") == true && Gettype[0].ToLower() != "brand")
                        //{
                        //    _catCid = Gettype[0];
                        //    _catname = Gettype[1];
                        //    _value = Gettype[1];
                        //    hfcid.Value = _catCid;
                        //}
                        //else
                        if ((requrl.Contains("ct.aspx")) && Gettype[0].ToLower() == "brand")
                        {
                            _tsb = Gettype[1];
                            _bypcat = "2";
                            // hidcatIds.Value = Gettype[1] + "^" + "1";
                        }
                        if (requrl.Contains("fl.aspx") == true)
                        {
                            if (IsNumber(Gettype[0]) == true)
                            {
                                _fid = Gettype[0];
                            }
                            if (Gettype[1].ToString().Contains(":"))
                            {
                                string[] getbname = _value.Split(new string[] { ":" }, StringSplitOptions.None);
                                if (Gettype[0].ToLower() == "model")
                                {
                                    _bname = getbname[0].Replace("'", "");
                                    _value = getbname[1].Replace("'", "");
                                }
                                else
                                {
                                    _bname = getbname[0];
                                    _value = getbname[1];
                                }
                            }
                        }

                        if ((requrl.Contains("pl.aspx")) || (requrl.Contains("powersearch.aspx")))
                        {

                            if (Gettype[1].ToString().Contains(":"))
                            {
                                string[] getbname = _value.Split(new string[] { ":" }, StringSplitOptions.None);
                                if (Gettype[0].ToLower() == "model")
                                {
                                    _bname = getbname[0].Replace("'", "");
                                    _value = getbname[1].Replace("'", "");
                                }
                                else
                                {
                                    _bname = getbname[0];
                                    _value = getbname[1];
                                }
                            }
                        }

                    }
                    else
                    {
                        //if (requrl.Contains("ct.aspx") == true)
                        //{
                        //    _catCid = Gettype[0];
                        //    _value = Gettype[1];
                        //    hfcid.Value = _catCid;
                        //}else
                        if ((requrl.Contains("fl.aspx")))
                        {
                            _fid = Gettype[0];


                        }
                        else if ((requrl.Contains("pd.aspx")))
                        {
                            _pid = Gettype[0];
                        }

                    }
                    if (HttpContext.Current.Session["EA"] == "")
                    {
                        HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
                    }
                    HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("USERSEARCH", "UserSearch").Replace("BRAND", "Brand").Replace("MODEL", "Model").Replace("CATEGORY", "Category");
                    HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("////////", "////");
                    // HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"] + "////" + AttribSelect;
                    orgeapath = "AllProducts////WESAUSTRALASIA////" + eapath;
                }
                else if (requrl.Contains("bb.aspx"))
                {
                    //  eapath = eapath + "////" + "AttribSelect=Model=" + newea + ":" + _value;
                    _tsm = _value;

                }
                else if (requrl.Contains("powersearch.aspx"))
                {
                    if (ConsURL.Length == 1)
                    {
                        _searchstr = _value;
                        _value = "";
                    }
                    else
                    {
                        _type = "Category";

                    }
                }
                else
                {
                    _type = "Category";

                }
                HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace('_', ' ').Replace("~..~", " / ").Replace("~..", " /").Replace("..~", "/ ").Replace(".~.", "/").Replace("||", "+").Replace("^^", "&").Replace("~`", ":"); ;
                orgeapath = HttpContext.Current.Session["EA"].ToString();
                string SessionEA = HttpContext.Current.Session["EA"].ToString();
            }



            else
                HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";

            if (HttpContext.Current.Session["MainCategory"] != null)
            {
                DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
                if (dr.Length > 0)
                    _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
            }

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

            if (_isorgurl == true)
            {
                Context.RewritePath(Request.Url.PathAndQuery.Replace("/", "") + "&ViewMode=" + _ViewType);
                if (Request.QueryString["cid"] == null || Request.QueryString["cid"] == "")
                {
                    Context.RewritePath(Request.Url.PathAndQuery.Replace("/", "") + "&ViewMode=" + _ViewType + "&cid=" + _catCid);
                }


            }
            if (requrl.Contains("ct.aspx") == true || requrl.Contains("microsite.aspx") == true || requrl.Contains("mct.aspx") == true)
            {

                if (_bypcat == null)
                {
                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
                    EasyAsk.GetMainMenuClickDetail(_catCid, "");
                    // hfcid.Value = _catCid;


                    DataTable tmptbl = null;

                    if (HttpContext.Current.Session["MainMenuClick"] != null)
                    {
                        tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0];

                        tmptbl = tmptbl.Select("CATEGORY_ID='" + _catCid + "'").CopyToDataTable();

                        if (tmptbl != null && tmptbl.Rows.Count > 0)
                        {
                            CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
                        }
                        else
                        {
                            CatName = _catname;
                        }


                    }


                    if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());
                    //if (_value != "")
                    //    _bname = _value;
                    EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                    hfcname.Value = CatName.Replace("/", "//");
                    //.Replace("+", "||").Replace("\"", "``").Replace("&", "^^").Replace(":", "~`");;

                    if (!(_isorgurl))
                    {
                        Context.RewritePath("ct.aspx?" + "&id=0&cid=" + _catCid + "&by=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                    }
                }
                else if (_tsb != string.Empty)
                {

                    string parentCatName = GetCName(_catCid);
                    EasyAsk.GetWESModel(parentCatName, iCatalogId, _tsb);
                    if (hforgurl.Value == string.Empty)
                    {
                        hforgurl.Value = parentCatName + "/" + "Brand=" + _tsb;
                    }
                    Context.RewritePath("ct.aspx?" + "&id=0&cid=" + _catCid + "&tsb=" + _tsb + "&bypcat=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                }

            }
            //Added By:Indu
            //To get EA path with out attribyte Type

            if ((requrl.Contains("bb.aspx")))
            {
                int SubCatCount = 0;
                //Added by :Indu For meta tag
                Session["prodmodel"] = _tsm + "," + _tsb;
                if (_type == null || _type == string.Empty)
                {
                    if (_tsb != null && _tsb != string.Empty && _tsm != null && _tsm != null)
                    {

                        //string parentCatName = GetCName(ParentCatID);
                        //EasyAsk.GetBrandAndModelProducts(parentCatName, _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                        if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                            iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

                        EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                        if (!(_isorgurl))
                        {
                            Context.RewritePath("bb.aspx?&id=0&cid=" + _catCid + "&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        }


                    }
                }
                else
                {
                    if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

                    if (_type != string.Empty)
                    {

                        EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                        if (_tsb == string.Empty)
                        {
                            _tsb = _bname;
                        }
                        if (!(_isorgurl))
                        {
                            Context.RewritePath("bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&type=" + _type + "&value=" + HttpUtility.UrlEncode(_value) + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        }

                    }
                    else
                    { //new open

                        EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                        if (!(_isorgurl))
                        {
                            Context.RewritePath("bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        }
                    }
                }
            }
            if ((requrl.Contains("pl.aspx")))
            {
                if (Session["RECORDS_PER_PAGE_pl"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_pl"].ToString());


                EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                if (!(_isorgurl))
                {
                    Context.RewritePath("pl.aspx?&cid=" + _catCid + "&type='" + _type + "'&value=" + _value + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                }
            }
            if ((requrl.Contains("ps.aspx")))
            {
                if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());

                EasyAsk.GetAttributeProducts("PowerSearch", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");


                if (!(_isorgurl))
                {
                    if (_value != string.Empty)
                    {
                        Context.RewritePath("ps.aspx?&id=0&searchstr=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                    }
                    else
                    {

                        Context.RewritePath("ps.aspx?&id=0&searchstr=" + _searchstr + "&srctext=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                    }
                }

            }




            if ((requrl.Contains("fl.aspx")))
            {
                if (_type == string.Empty)
                {

                    EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");
                    //objErrorHandler.CreateLog(requrl);
                    //objErrorHandler.CreateLog("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&path=" + Session["EA"].ToString());   

                    if (!(_isorgurl))
                    {
                        Context.RewritePath("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&path=" + Session["EA"].ToString());
                    }
                }
                else
                {

                    EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value, _bname, "0", "0", "");

                    if (!(_isorgurl))
                    {
                        //objErrorHandler.CreateLog("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + Session["EA"].ToString());
                        // objErrorHandler.CreateLog(requrl);
                        Context.RewritePath("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                    }
                }
            }
            if ((requrl.Contains("pd.aspx")))
            {
                if (_type == string.Empty)
                {

                    EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");

                    if (!(_isorgurl))
                    {
                        Context.RewritePath("pd.aspx?&pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        //pd.aspx?&pid=11638&fid=&cid=&path=w69oz9yt7nzLyCLCqK9wJzcf%2bCuhoudzTudtlFipxiPHz%2ft4OIyi2p2AayFoZKaJqK4bYIkLuC2qI5%2foelciRlw7Prn5b%2bAE0ihTGyTCXDjjFjPDfiZiLlZjNR9CX82k8pdtFr2loSA%3d
                    }
                }
                else
                {

                    EasyAsk.GetAttributeProducts("ProductPage", _searchstr, _type, _value, _bname, "0", "0", "");
                    if (!(_isorgurl))
                    {
                        Context.RewritePath("pd.aspx?&pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "_searchstr" + _searchstr + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        //pd.aspx?&pid=11638&fid=&cid=&path=w69oz9yt7nzLyCLCqK9wJzcf%2bCuhoudzTudtlFipxiPHz%2ft4OIyi2p2AayFoZKaJqK4bYIkLuC2qI5%2foelciRlw7Prn5b%2bAE0ihTGyTCXDjjFjPDfiZiLlZjNR9CX82k8pdtFr2loSA%3d
                    }
                }
            }
        }
        catch (Exception ex)
        {

            sHTML = ex.Message;
        }




        return sHTML;
    }

    protected string Get_Value_Breadcrum_New()
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
            string requrl = HttpContext.Current.Request.Url.ToString().ToLower();
            if ((requrl.Contains("pd.aspx")) || (requrl.Contains("mpd.aspx")))
            {
                if (HttpContext.Current.Session["Category_Attributes"] == null && _parentCatID != string.Empty)
                {
                    EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
                }
            }

            if (((requrl.Contains("mct.aspx"))) || (requrl.Contains("mpl.aspx"))
               || (requrl.Contains("mps.aspx"))
               || (requrl.Contains("mfl.aspx"))
               || (requrl.Contains("mpd.aspx")))
            {
                if (HttpContext.Current.Session["MainCategory"] == null || HttpContext.Current.Session["MainMenuClick"] == null)
                {
                    HttpCookie LoginInfoCookie = Request.Cookies["ActiveSupplier"];
                    if (LoginInfoCookie != null && LoginInfoCookie["SupplierID"] != null)
                    {
                        EasyAsk.GetMainMenuClickDetail(objSecurity.StringDeCrypt_password(LoginInfoCookie["SupplierID"]).ToString(), "");
                    }
                    else if (_catCid != string.Empty)
                    {
                        EasyAsk.GetMainMenuClickDetail(_catCid, "");
                    }
                }
            }

            int ckbrand = 0;

            if ((Request.QueryString["path"] != null) && (hfisselected.Value == ""))
            {
                HttpContext.Current.Session["EA"] = objSecurity.StringDeCrypt(Request.QueryString["path"].ToString());
                _isorgurl = true;
            }
            else if (!(requrl.Contains("/Login.aspx")) && !(requrl.Contains("/mlogin.aspx")) && !(requrl.Contains("/home.aspx")) || !(requrl.Contains(".png")))
            {
                HttpContext.Current.Session["EA"] = "";
                string querystring = string.Empty;
                if (hfisselected.Value.Equals("1") && hforgurl.Value != string.Empty)
                {

                    if (!(hforgurl.Value.Contains("BrandModel")))
                    {
                        querystring = hforgurl.Value;
                    }
                    else
                    {
                        querystring = hforgurl.Value.Replace("bb.aspx?", "");

                        //StringBuilder SBquerystring = new StringBuilder(querystring);
                        //SBquerystring.Replace('_', ' ');
                        //SBquerystring.Replace("||", "+");
                        //SBquerystring.Replace("``", "\"");
                        //SBquerystring.Replace(" / ", "~..~");
                        //SBquerystring.Replace(" /", "~..");
                        //SBquerystring.Replace("/ ", "..~");
                        //SBquerystring.Replace("`/`", ".~.");
                        //querystring = SBquerystring.ToString();
                        string[] ConsURL1 = querystring.Split(new string[] { "/" }, StringSplitOptions.None);
                        _tsb = ConsURL1[1];
                        _tsm = ConsURL1[2];

                        // Context.RewritePath("bb.aspx?" + querystring); 
                    }

                }
                else
                {
                    string rawurl = string.Empty;
                    string reqrawurl = Request.RawUrl.ToString().ToLower();
                    if (reqrawurl.Contains("ct.aspx?") && reqrawurl.Contains("/ct/"))
                    {
                        string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "ct.aspx?" }, StringSplitOptions.None);
                        rawurl = rawurl1[0];
                    }
                    else if (reqrawurl.Contains("mct.aspx?") && reqrawurl.Contains("/mct/"))
                    {
                        string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "mct.aspx?" }, StringSplitOptions.None);
                        rawurl = rawurl1[0];
                    }
                    else if (reqrawurl.Contains("microsite.aspx?") && reqrawurl.Contains("/ct/"))
                    {
                        string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "microsite.aspx?" }, StringSplitOptions.None);
                        rawurl = rawurl1[0];
                    }
                    else if (reqrawurl.Contains("bb.aspx?") && reqrawurl.Contains("/bb/"))
                    {
                        string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "bb.aspx?" }, StringSplitOptions.None);
                        rawurl = rawurl1[0];
                    }
                    else if (reqrawurl.Contains("ps.aspx?") && reqrawurl.Contains("/ps/"))
                    {
                        string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "ps.aspx?" }, StringSplitOptions.None);
                        rawurl = rawurl1[0];
                    }
                    else if (reqrawurl.Contains("mps.aspx?") && reqrawurl.Contains("/mps/"))
                    {
                        string[] rawurl1 = Request.RawUrl.ToString().Split(new string[] { "mps.aspx?" }, StringSplitOptions.None);
                        rawurl = rawurl1[0];
                    }
                    else
                    {
                        rawurl = Request.RawUrl.ToString().ToLower();
                    }
                    if (
                           rawurl.Contains("/mfl/") || rawurl.Contains("mfl.aspx")
                  || rawurl.Contains("/mpl/") || rawurl.Contains("mpl.aspx")
                  || rawurl.Contains("/mpd/") || rawurl.Contains("mpd.aspx")
               || rawurl.Contains("/mps/") || rawurl.Contains("mps.aspx"))
                    {
                        querystring = objHelperServices.URlStringReverse_MS(rawurl);

                    }
                    else
                    {
                        querystring = objHelperServices.URlStringReverse(rawurl);
                    }
                    querystring = "/" + querystring;
                    querystring = querystring.Replace("/ct/", "ct.aspx?").Replace("/pl/", "pl.aspx?").Replace("/fl/", "fl.aspx?").Replace("/bb/", "bb.aspx?").
                        Replace("/pd/", "pd.aspx?").Replace("/ps/", "ps.aspx?").Replace("/microsite/", "microsite.aspx?").Replace("/mct/", "mct.aspx?").
                        Replace("/mfl/", "mfl.aspx?").Replace("/mpl/", "mpl.aspx?").Replace("/mpd/", "mpd.aspx?").
                        Replace("/mps/", "mps.aspx?");
                    querystring = querystring.Replace("~pd~", "pd").Replace("~fl~", "fl").
                     Replace("~pl~", "pl").Replace("~ps~", "ps").Replace("~ct~", "ct").Replace("~bb~", "bb").Replace("~bk~", "bk").Replace("~bp~", "bp").
                     Replace("~mpl~", "mpl").Replace("~mps~", "mps");
                    //requrl = querystring;
                    if (querystring.Contains("&gclid="))
                    {
                        string[] querystring1 = querystring.Split(new string[] { "&gclid=" }, StringSplitOptions.None);
                        querystring = querystring1[0];
                    }
                    else if (querystring.Contains("?gclid="))
                    {
                        string[] querystring1 = querystring.Split(new string[] { "?gclid=" }, StringSplitOptions.None);
                        querystring = querystring1[0];
                    }
                    if (querystring.EndsWith("/"))
                    {

                        querystring = querystring.Substring(0, querystring.Length - 1);
                    }
                    string[] qs = querystring.Split(new string[] { "?" }, StringSplitOptions.None);
                    if (qs.Length >= 1)
                    {
                        querystring = qs[1];
                    }

                    // querystring= HttpUtility.UrlDecode(Request.Url.Query.ToString().Replace("?", ""));
                }
                string dbq = HttpUtility.UrlDecode("%E2%80%9C");
                string dbq1 = HttpUtility.UrlDecode("%E2%80%9D");
                string dbq2 = HttpUtility.UrlDecode("%C3%98");
                //StringBuilder SBquerystring1 = new StringBuilder(querystring);
                //SBquerystring1.Replace('_', ' ');
                //SBquerystring1.Replace("``", "\"");
                //SBquerystring1.Replace("&srctext=", "");
                //SBquerystring1.Replace("%E2%80%9C", dbq);
                //SBquerystring1.Replace("%E2%80%9D", dbq1);
                //SBquerystring1.Replace("%C3%98", dbq2);
                //SBquerystring1.Replace(" / ", "~..~");
                //SBquerystring1.Replace(" /", "~..");
                //SBquerystring1.Replace("/ ", "..~");
                //SBquerystring1.Replace("`/`", ".~.");

                //querystring = SBquerystring1.ToString();
                string[] ConsURL = querystring.Split('/');
                string ea = string.Empty;
                if (ConsURL.Length >= 2)
                {
                    ea = querystring.Replace(ConsURL[ConsURL.Length - 1], "");
                }
                else
                {
                    ea = querystring;
                }

                //StringBuilder sb_ea = new StringBuilder(ea);
                //sb_ea.Replace("~..~", " / ");
                //sb_ea.Replace("~..", " /");
                //sb_ea.Replace("..~", "/ ");
                //sb_ea.Replace(".~.", "/");
                //ea = sb_ea.ToString();
                // string ea = querystring;
                string newea = string.Empty;
                if (ea.Substring(ea.Length - 1, 1).Contains("/"))
                {
                    newea = ea.Substring(0, ea.Length - 1);
                }
                else
                {

                    newea = ea;
                }
                newea = newea.Replace("~~", "/");
                string eapath = string.Empty;
                int fid = 0;
                if (ConsURL.Length <= 1)
                {

                    // HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////Cellular Accessories////" + newea.Replace("/", "////");
                    //if (requrl.Contains("bb.aspx"))
                    //{
                    //    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////Cellular Accessories////" + "AttribSelect=Brand='" + newea.Replace("/", "////") + "'";
                    //    _tsb = newea.Replace("/", "////");
                    //}
                    if (requrl.Contains("ps.aspx") || requrl.Contains("mps.aspx"))
                    {

                        HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + "UserSearch=" + newea.Replace("/", "////").Replace("&srctext=", "");
                        _searchstr = newea.Replace("/", "////").Replace("&srctext=", "");
                    }
                    if (requrl.Contains("ct.aspx") && (newea.ToLower().Contains("brand")))
                    {
                        HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
                        _tsb = ea.Replace("BRAND=", "").Replace("Brand=", "");
                        _tsbnew = ea.Replace("BRAND=", "").Replace("Brand=", "");
                        _bypcat = "2";
                    }
                }
                else
                {
                    string brand = string.Empty;
                    for (int i = 0; i <= ConsURL.Length - 2; i++)
                    {
                        //ConsURL[i] = ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" ); 
                        //StringBuilder SB_ConsURL = new StringBuilder(ConsURL[i]);

                        //SB_ConsURL.Replace("~..~", " / ");
                        //SB_ConsURL.Replace("~..", " /");
                        //SB_ConsURL.Replace("..~", "/ ");
                        //SB_ConsURL.Replace(".~.", "/");
                        string SB_ConsURL = ConsURL[i].Replace("~~", "/");
                        //if (SB_ConsURL.Contains("_") == true)
                        //{
                        //    DataSet ds = objHelperDB.GetOrgValue("", SB_ConsURL);
                        //    if (ds != null)
                        //    {
                        //        SB_ConsURL = ds.Tables[0].Rows[0][0].ToString();
                        //    }
                        //}
                        if (SB_ConsURL.ToString().Contains("="))
                        {

                            string[] Gettype = SB_ConsURL.Split(new string[] { "=" }, StringSplitOptions.None);
                            //StringBuilder SB_gtype = new StringBuilder(Gettype[1]);
                            //SB_gtype.Replace("~..~", " / ");
                            //SB_gtype.Replace("~..", " /");
                            //SB_gtype.Replace("..~", "/ ");
                            //SB_gtype.Replace(".~.", "/");
                            //string gettype1 = SB_gtype.ToString();
                            string gettype1 = Gettype[1].ToString();
                            if (IsNumber(Gettype[0]) == false)
                            {

                                //if (requrl.Contains("ct.aspx") == true  && Gettype[0].ToLower()!="brand")
                                //{
                                //    //_catCid = Gettype[0];
                                //    //hfcid.Value = _catCid;
                                //    _catname  = Gettype[1];


                                //}
                                string gettype0 = Gettype[0].ToLower();
                                //if (Gettype[1].Contains("-") == true)
                                //{
                                //    DataSet ds = objHelperDB.GetOrgValue(gettype0, Gettype[1].ToLower());
                                //    if (ds != null)
                                //    {
                                //        gettype1 = ds.Tables[0].Rows[0][0].ToString();
                                //    }
                                //}
                                if (gettype0 == "brand")
                                {
                                    brand = gettype1;
                                    _tsb = brand;
                                    _bname = brand;
                                    ckbrand = 1;
                                    _bypcat = "2";
                                }
                                if (gettype0 == "model")
                                {
                                    if (gettype1.ToString().Contains(":"))
                                    {
                                        string[] getbname = gettype1.Split(new string[] { ":" }, StringSplitOptions.None);

                                        eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + getbname[0].Replace("'", "") + ":" + getbname[1].Replace("'", "") + "'";
                                    }
                                    else
                                    {
                                        eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + brand + ":" + gettype1 + "'";
                                        _tsm = Gettype[1];
                                    }
                                    ckbrand = 2;
                                }

                                else if (gettype0 == "category")
                                {
                                    eapath = eapath + "////" + gettype1;
                                }
                                else if (gettype0 == "usersearch")
                                {
                                    eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
                                }
                                else if (gettype0 == "usersearch1")
                                {
                                    eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
                                }
                                else if (gettype0 == "usersearch2")
                                {
                                    eapath = eapath + "////" + Gettype[0] + "=" + gettype1;
                                }
                                else if ((gettype0 == "USER SEARCH") && ((requrl.Contains("fl.aspx")) || (requrl.Contains("pd.aspx"))))
                                {
                                    eapath = eapath + "////" + "UserSearch" + "=" + gettype1;
                                }
                                //else if ((Gettype[0].ToLower() == "user SEARCH") && (requrl.Contains("fl.aspx")) || (requrl.Contains("pd.aspx")))
                                //{
                                //    eapath = eapath + "////" + "UserSearch" + "=" + gettype1;
                                //}
                                else
                                {
                                    //if (i == 0)
                                    //{
                                    //   if (requrl.Contains("ps.aspx") == true)
                                    //    {
                                    eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + gettype1 + "'";
                                    //}
                                    //else
                                    //{
                                    //   eapath = eapath + "////" + gettype1 ;

                                    //        hfcid.Value = Gettype[0];
                                    //        _catCid = Gettype[0];

                                    //}
                                    //}
                                    //else
                                    //{
                                    //    eapath = eapath + "////" + "AttribSelect=" + Gettype[0] + "='" + gettype1 + "'";

                                    //}

                                }
                            }
                            else if (fid != 1)
                            {
                                fid = 1;
                                _fid = Gettype[0];
                                eapath = eapath + "////" + "UserSearch=Family Id=" + Gettype[0];
                            }
                            else if (fid == 1)
                            {
                                eapath = eapath + "////" + "UserSearch1=Prod Id=" + Gettype[0];
                            }
                            //if((requrl.Contains("fl.aspx"))||(requrl.Contains("pd.aspx")))
                            //{
                            //    eapath = eapath.Replace("user_search", "UserSearch").Replace("user search", "UserSearch").Replace("USER_SEARCH", "UserSearch").Replace("USER SEARCH", "UserSearch").Replace("User_Search", "UserSearch").Replace("User Search", "UserSearch");   

                            //}

                            //if (requrl.Contains("ct.aspx") == true && ea.ToString().ToLower().Contains("brand")==false   )
                            //{
                            //   // _catCid = Gettype[0];
                            //    _value = Gettype[];
                            //   // hfcid.Value = _catCid;

                            //}
                        }
                        else if (requrl.Contains("bb.aspx"))
                        {



                            if (ckbrand == 0)
                            {
                                eapath = eapath + SB_ConsURL;
                                _catname = SB_ConsURL;
                                //_tsb = ConsURL[i];
                                //_bname = ConsURL[i];
                                ckbrand = 1;
                            }
                            else if (ckbrand == 1)
                            {
                                //ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" )
                                eapath = eapath + "////" + "AttribSelect=Brand='" + SB_ConsURL + "'";
                                _tsb = SB_ConsURL;
                                _bname = SB_ConsURL;
                                ckbrand = 2;
                            }
                            else if (ckbrand == 2)
                            {
                                //ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" )
                                eapath = eapath + "////" + "AttribSelect=Model='" + _bname + ":" + SB_ConsURL + "'";
                                _tsm = SB_ConsURL;
                                ckbrand = 2;
                            }

                            else
                            {
                                //ConsURL[i].Replace("~..~"," / " ).Replace("~.."," /" ).Replace("..~","/ " ).Replace(".~.","/" )
                                eapath = eapath + "////" + SB_ConsURL;
                            }


                        }

                        else
                        {

                            if (i == 0 && (requrl.Contains("/ps.aspx")))
                            {
                                eapath = "UserSearch=" + SB_ConsURL;
                                _searchstr = SB_ConsURL;
                            }
                            else if (i == 0 && (requrl.Contains("mps.aspx")))
                                eapath = eapath + SB_ConsURL;
                            else if (i == 1 && (requrl.Contains("mps.aspx")))
                                eapath = eapath + "////" + "UserSearch=" + SB_ConsURL;
                            else
                            {
                                eapath = eapath + "////" + SB_ConsURL;
                            }
                        }

                    }
                    //StringBuilder SB_EAPATH = new StringBuilder(eapath);
                    //SB_EAPATH.Replace("?", "");
                    //SB_EAPATH.Replace("~..~", " / ");
                    //SB_EAPATH.Replace("~..", " /");
                    //SB_EAPATH.Replace("..~", "/ ");
                    //SB_EAPATH.Replace(".~.", "/");
                    //SB_EAPATH.Replace("||", "+");
                    //SB_EAPATH.Replace("^^", "&");
                    //SB_EAPATH.Replace("~`", ":");
                    //SB_EAPATH.Replace("~^", ".");
                    eapath = eapath.Replace("~~", "/");
                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + eapath.ToString();

                }
                //StringBuilder sb_value = new StringBuilder(ConsURL[ConsURL.Length - 1]);
                //sb_value.Replace('_', ' ');
                //sb_value.Replace("~..~", " / ");
                //sb_value.Replace("~..", " /");
                //sb_value.Replace("..~", "/ ");
                //sb_value.Replace(".~.", "/");
                //sb_value.Replace("||", "+");
                //sb_value.Replace("^^", "&");
                //sb_value.Replace("~`", ":");
                //_value = sb_value.ToString();
                _value = ConsURL[ConsURL.Length - 1].Replace("~~", "/");
                if (_value.Contains("="))
                {
                    string[] Gettype = _value.Split(new string[] { "=" }, StringSplitOptions.None);
                    string AttribSelect = string.Empty;
                    if (IsNumber(Gettype[0]) == false)
                    {

                        _type = Gettype[0];
                        _value = Gettype[1];

                        //AttribSelect = "AttribSelect=" + _type + "='" + _value + "'";

                        //if (requrl.Contains("ct.aspx") == true && Gettype[0].ToLower() != "brand")
                        //{
                        //    _catCid = Gettype[0];
                        //    _catname = Gettype[1];
                        //    _value = Gettype[1];
                        //    hfcid.Value = _catCid;
                        //}
                        //else
                        if (((requrl.Contains("mct.aspx")) || (requrl.Contains("ct.aspx"))) && Gettype[0].ToLower() == "brand")
                        {
                            _tsb = Gettype[1];
                            _bypcat = "2";
                            // hidcatIds.Value = Gettype[1] + "^" + "1";
                        }
                        if ((requrl.Contains("fl.aspx")) || (requrl.Contains("mfl.aspx")))
                        {
                            if (IsNumber(Gettype[0]) == true)
                            {
                                _fid = Gettype[0];
                            }
                            if (Gettype[1].ToString().Contains(":"))
                            {
                                string[] getbname = _value.Split(new string[] { ":" }, StringSplitOptions.None);
                                if (Gettype[0].ToLower() == "model")
                                {
                                    _bname = getbname[0].Replace("'", "");
                                    _value = getbname[1].Replace("'", "");
                                }
                                else
                                {
                                    _bname = getbname[0];
                                    _value = getbname[1];
                                }
                            }
                        }

                        if (((requrl.Contains("pl.aspx"))) || ((requrl.Contains("ps.aspx")) || (requrl.Contains("mpl.aspx"))) || (requrl.Contains("mps.aspx")))
                        {

                            if (Gettype[1].ToString().Contains(":"))
                            {
                                string[] getbname = _value.Split(new string[] { ":" }, StringSplitOptions.None);
                                if (Gettype[0].ToLower() == "model")
                                {
                                    _bname = getbname[0].Replace("'", "");
                                    _value = getbname[1].Replace("'", "");
                                }
                                else
                                {
                                    _bname = getbname[0];
                                    _value = getbname[1];
                                }
                            }
                        }

                    }
                    else
                    {
                        //if (requrl.Contains("ct.aspx") == true)
                        //{
                        //    _catCid = Gettype[0];
                        //    _value = Gettype[1];
                        //    hfcid.Value = _catCid;
                        //}else
                        if ((requrl.Contains("fl.aspx")) || (requrl.Contains("mfl.aspx")))
                        {
                            _fid = Gettype[0];


                        }
                        else if ((requrl.Contains("pd.aspx")) || (requrl.Contains("mpd.aspx")))
                        {
                            _pid = Gettype[0];
                        }

                    }
                    if (HttpContext.Current.Session["EA"] == "")
                    {
                        HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
                    }
                    HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("USERSEARCH", "UserSearch").Replace("usersearch", "UserSearch").Replace("BRAND", "Brand").Replace("brand", "Brand").Replace("MODEL", "Model").Replace("model", "Model").Replace("CATEGORY", "Category").Replace("category", "Category");
                    HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("////////", "////");
                    // HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"] + "////" + AttribSelect;
                    orgeapath = "AllProducts////WESAUSTRALASIA////" + eapath;
                }
                else if (requrl.Contains("bb.aspx") || requrl.Contains("mbb.aspx"))
                {
                    //  eapath = eapath + "////" + "AttribSelect=Model=" + newea + ":" + _value;
                    _tsm = _value;

                }
                else if (requrl.Contains("/ps.aspx"))
                {
                    if (ConsURL.Length == 1)
                    {
                        _searchstr = _value;
                        _value = "";
                    }
                    else
                    {
                        _type = "Category";

                    }
                }
                else if (requrl.Contains("mps.aspx"))
                {
                    if (ConsURL.Length == 2)
                    {
                        _searchstr = ConsURL[1].ToString();
                        _value = "";
                    }
                    else
                    {
                        _type = "Category";

                    }
                }
                else
                {
                    _type = "Category";

                }
                HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace('_', ' ').Replace("~..~", " / ").Replace("~..", " /").Replace("..~", "/ ").Replace(".~.", "/").Replace("||", "+").Replace("^^", "&").Replace("~`", ":"); ;
                orgeapath = HttpContext.Current.Session["EA"].ToString();
                string SessionEA = HttpContext.Current.Session["EA"].ToString();
            }



            else
                HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";

            if (HttpContext.Current.Session["MainCategory"] != null)
            {
                DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
                if (dr.Length > 0)
                    _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
            }

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

            if ((_isorgurl))
            {
                Context.RewritePath("/" + Request.Url.PathAndQuery.Replace("/", "") + "&ViewMode=" + _ViewType);
                if (Request.QueryString["cid"] == null || Request.QueryString["cid"] == string.Empty)
                {
                    Context.RewritePath("/" + Request.Url.PathAndQuery.Replace("/", "") + "&ViewMode=" + _ViewType + "&cid=" + _catCid);
                }


            }





            if ((requrl.Contains("ct.aspx")) || (requrl.Contains("microsite.aspx")) || (requrl.Contains("mtc.aspx")))
            {

                if (_bypcat == null)
                {




                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////";
                    if (_catCid == string.Empty)
                    {
                        _catCid = "WES0830";
                    }
                    //if (_catCid == "")
                    //{
                    //    _catCid = "0";
                    //}

                    EasyAsk.GetMainMenuClickDetail(_catCid, "");
                    // hfcid.Value = _catCid;


                    DataTable tmptbl = null;
                    string SHORT_DESC = string.Empty;
                    if (HttpContext.Current.Session["MainMenuClick"] != null)
                    {
                        tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0];

                        tmptbl = tmptbl.Select("CATEGORY_ID='" + _catCid + "'").CopyToDataTable();

                        if (tmptbl != null && tmptbl.Rows.Count > 0)
                        {
                            CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
                            SHORT_DESC = tmptbl.Rows[0]["SHORT_DESC"].ToString();
                        }
                        else
                        {
                            CatName = _catname;
                        }



                    }
                    if (requrl.Contains("mct.aspx"))
                    {
                        Session["SUPPLIER_NAME"] = CatName;
                        Session["SUPPLIER_CID"] = _catCid;
                        Session["SUPPLIER_DESC"] = SHORT_DESC;
                        SetCookie(CatName, _catCid);
                    }
                    else
                    {
                        Session["SUPPLIER_NAME"] = "";
                        Session["SUPPLIER_CID"] = "";
                        Session["SUPPLIER_DESC"] = "";
                    }

                    if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());
                    //if (_value != "")
                    //    _bname = _value;

                    EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                    hfcname.Value = CatName.Replace("/", "//");
                    //.Replace("+", "||").Replace("\"", "``").Replace("&", "^^").Replace(":", "~`");;

                    if (!(_isorgurl))
                    {
                        if ((requrl.Contains("mct.aspx")))
                        {
                            Context.RewritePath("/mct.aspx?" + "&id=0&cid=" + _catCid + "&by=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        }
                        else
                            Context.RewritePath("/ct.aspx?" + "&id=0&cid=" + _catCid + "&by=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                    }



                }
                else if (_tsb != string.Empty)
                {

                    string parentCatName = GetCName(_catCid);
                    EasyAsk.GetWESModel(parentCatName, iCatalogId, _tsb);
                    if (hforgurl.Value == string.Empty)
                    {
                        hforgurl.Value = parentCatName + "/" + "Brand=" + _tsb;
                    }
                    if ((requrl.Contains("mtc.aspx")))
                        Context.RewritePath("/mct.aspx?" + "&id=0&cid=" + _catCid + "&tsb=" + _tsb + "&bypcat=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                    else
                        Context.RewritePath("/ct.aspx?" + "&id=0&cid=" + _catCid + "&tsb=" + _tsb + "&bypcat=2" + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                }

            }
            //Added By:Indu
            //To get EA path with out attribyte Type

            if ((requrl.Contains("bb.aspx")) || (requrl.Contains("mbb.aspx")))
            {
                //  int SubCatCount = 0;
                //Added by :Indu For meta tag
                Session["prodmodel"] = _tsm + "," + _tsb;
                if (_type == null || _type == string.Empty)
                {
                    if (_tsb != null && _tsb != string.Empty && _tsm != null && _tsm != null)
                    {

                        //string parentCatName = GetCName(ParentCatID);
                        //EasyAsk.GetBrandAndModelProducts(parentCatName, _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                        if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                            iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

                        EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                        if (!(_isorgurl))
                        {
                            if ((requrl.Contains("mbb.aspx")))
                                Context.RewritePath("/mbb.aspx?&id=0&cid=" + _catCid + "&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                            else
                                Context.RewritePath("/bb.aspx?&id=0&cid=" + _catCid + "&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                        }


                    }
                }
                else
                {
                    if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

                    if (_type != string.Empty)
                    {

                        EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                        if (_tsb == string.Empty)
                        {
                            _tsb = _bname;
                        }
                        if (!(_isorgurl))
                        {
                            if (requrl.Contains("mbb.aspx"))
                                Context.RewritePath("/mbb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&type=" + _type + "&value=" + HttpUtility.UrlEncode(_value) + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                            else
                                Context.RewritePath("/bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&type=" + _type + "&value=" + HttpUtility.UrlEncode(_value) + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                        }

                    }
                    else
                    { //new open

                        EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

                        if (!(_isorgurl))
                        {
                            if (requrl.Contains("mbb.aspx"))
                                Context.RewritePath("/mbb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                            else
                                Context.RewritePath("/bb.aspx?id=0&cid=" + _catCid + "&byp=2&tsm=" + HttpUtility.UrlEncode(_tsm) + "&tsb=" + _tsb + "&ViewMode=" + _ViewType + "&searchstr=&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        }
                    }
                }
            }
            if ((requrl.Contains("pl.aspx")) || (requrl.Contains("mpl.aspx")))
            {
                if (Session["RECORDS_PER_PAGE_pl"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_pl"].ToString());


                EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                if (!(_isorgurl))
                {
                    if (requrl.Contains("mpl.aspx"))
                        Context.RewritePath("/mpl.aspx?cid=" + _catCid + "&type='" + _type + "'&value=" + _value + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                    else
                        Context.RewritePath("/pl.aspx?&cid=" + _catCid + "&type='" + _type + "'&value=" + _value + "&bname=" + _bname + "&ViewMode=" + _ViewType + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                }
            }
            if ((requrl.Contains("ps.aspx")) || (requrl.Contains("mps.aspx")))
            {
                if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());
                if ((_searchstr == "~fl~") || (_searchstr == "~pl~") || (_searchstr == "~ps~") || (_searchstr == "~ct~") || (_searchstr == "~pd~") || (_searchstr == "~bb~") || (_searchstr == "~bp~") || (_searchstr == "~bk~"))
                {
                    _searchstr = _searchstr.Replace("~", "");
                }
                if ((_value == "~fl~") || (_value == "~pl~") || (_value == "~ps~") || (_value == "~ct~") || (_value == "~pd~") || (_value == "~bb~") || (_value == "~bp~") || (_value == "~bk~"))
                {
                    _value = _value.Replace("~", "");
                }
                EasyAsk.GetAttributeProducts("ps", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");


                if (!(_isorgurl))
                {
                    if (_value != "")
                    {
                        if (requrl.Contains("mps.aspx"))
                            Context.RewritePath("/mps.aspx?&id=0&searchstr=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        else
                            Context.RewritePath("/ps.aspx?&id=0&searchstr=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));

                    }
                    else
                    {
                        if (requrl.Contains("mps.aspx"))
                            Context.RewritePath("/mps.aspx?&id=0&searchstr=" + _searchstr + "&srctext=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        else
                            Context.RewritePath("/ps.aspx?&id=0&searchstr=" + _searchstr + "&srctext=" + _searchstr + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&src=" + _searchstr + "&ViewMode=" + _ViewType + "&byp=2&Path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                    }
                }

            }




            if ((requrl.Contains("fl.aspx")) || (requrl.Contains("mfl.aspx")))
            {
                if (_type == string.Empty)
                {

                    EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");
                    //objErrorHandler.CreateLog(requrl);
                    //objErrorHandler.CreateLog("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&path=" + Session["EA"].ToString());   

                    if (!(_isorgurl))
                    {
                        if (requrl.Contains("mfl.aspx"))
                            Context.RewritePath("/mfl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&path=" + Session["EA"].ToString());
                        else
                            Context.RewritePath("/fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&path=" + Session["EA"].ToString());
                    }
                }
                else
                {

                    EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value, _bname, "0", "0", "");

                    if (!(_isorgurl))
                    {
                        //objErrorHandler.CreateLog("fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + Session["EA"].ToString());
                        // objErrorHandler.CreateLog(requrl);
                        if (requrl.Contains("mfl.aspx"))
                            Context.RewritePath("/mfl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        else
                            Context.RewritePath("/fl.aspx?&fid=" + _fid + "&cid=" + _catCid + "&type=" + _type + "&value=" + _value + " &tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                    }
                }
            }
            if ((requrl.Contains("pd.aspx")) || (requrl.Contains("mpd.aspx")))
            {
                if (_type == string.Empty)
                {

                    EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");

                    if (!(_isorgurl))
                    {
                        if (requrl.Contains("mpd.aspx"))
                            Context.RewritePath("/mpd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        else
                            Context.RewritePath("/pd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        //pd.aspx?&pid=11638&fid=&cid=&path=w69oz9yt7nzLyCLCqK9wJzcf%2bCuhoudzTudtlFipxiPHz%2ft4OIyi2p2AayFoZKaJqK4bYIkLuC2qI5%2foelciRlw7Prn5b%2bAE0ihTGyTCXDjjFjPDfiZiLlZjNR9CX82k8pdtFr2loSA%3d
                    }
                }
                else
                {

                    EasyAsk.GetAttributeProducts("ProductPage", _searchstr, _type, _value, _bname, "0", "0", "");
                    if (!(_isorgurl))
                    {
                        if (requrl.Contains("mpd.aspx"))
                            Context.RewritePath("/mpd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "_searchstr" + _searchstr + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        else
                            Context.RewritePath("/pd.aspx?pid=" + _pid + "&cid=" + _catCid + "&fid=" + _fid + "&tsb=" + _tsb + "&tsm=" + _tsm + "&type=" + _type + "&value=" + _value + "&bname=" + _bname + "_searchstr" + _searchstr + "&path=" + objSecurity.StringEnCrypt(Session["EA"].ToString()));
                        //pd.aspx?&pid=11638&fid=&cid=&path=w69oz9yt7nzLyCLCqK9wJzcf%2bCuhoudzTudtlFipxiPHz%2ft4OIyi2p2AayFoZKaJqK4bYIkLuC2qI5%2foelciRlw7Prn5b%2bAE0ihTGyTCXDjjFjPDfiZiLlZjNR9CX82k8pdtFr2loSA%3d
                    }
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            sHTML = ex.Message;
        }




        return sHTML;
    }
    //-------------------------------------------------------

    protected string ST_Categories()
    {

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
            _bypcat = Request.QueryString["bypcat"];

            string Requrl = HttpContext.Current.Request.Url.ToString().ToLower();


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


            if (_catCid != string.Empty)
                _parentCatID = GetParentCatID(_catCid);
            if ((Requrl.Contains("pd.aspx")))
            {
                if (HttpContext.Current.Session["Category_Attributes"] == null && _parentCatID != string.Empty)
                {
                    EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
                }
            }


            //Modified by indu
            //modified on 9-5-1013
            //For Metatag

            //if (Request.QueryString["path"] != null)
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

            _stg_records = new StringTemplateGroup("searchrsltcategoryleftrecords", stemplatepath);
            _stg_container = new StringTemplateGroup("searchrsltcategoryleftmain", stemplatepath);
            if (dscat != null)
            {

                if (dscat.Tables.Count > 0)
                    lstrows = new TBWDataList[dscat.Tables.Count + 1];

                for (int i = 0; i < dscat.Tables.Count; i++)
                {
                    Boolean tmpallow = true;
                    //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true && Request.QueryString["tsb"]!=null  && Request.QueryString["tsb"].ToString()!="")
                    //{ 
                    //    if (dscat.Tables[i].TableName.Contains("Model"))
                    //        tmpallow = true;
                    //    else
                    //        tmpallow = false;
                    //}
                    //else 
                    if ((Requrl.Contains("ct.aspx")) || (Requrl.Contains("microsite.aspx")) || (Requrl.Contains("pd.aspx")))
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


                                if ((Requrl.Contains("ct.aspx")) || (Requrl.Contains("microsite.aspx")))
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell");
                                }

                                else if ((Requrl.Contains("bb.aspx")))
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell1");
                                }
                                else if ((Requrl.Contains("ps.aspx")))
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell2");
                                }
                                else if ((Requrl.Contains("fl.aspx")))
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell3");
                                }
                                else
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell");
                                }
                                string brandname = string.Empty;
                                if (dscat.Tables[i].TableName.Contains("Category"))
                                {
                                    _stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                    _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));

                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr["Category_Name"]);
                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
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
                                    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
                                }
                                string TBW_ATTRIBUTE_TYPE = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_TYPE").ToString().ToUpper();
                                string TBW_ATTRIBUTE_VALUE = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_VALUE").ToString();
                                if (_tsm != string.Empty)
                                {
                                    _BCEAPath = _BCEAPath.Replace(_tsm, HttpUtility.UrlEncode(_tsm));
                                }

                                if ((TBW_ATTRIBUTE_TYPE.ToUpper() == "CATEGORY") && (Requrl.Contains("bb.aspx")))
                                {
                                    objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + TBW_ATTRIBUTE_VALUE, "bb.aspx", true, TBW_ATTRIBUTE_TYPE);
                                }

                                else if (TBW_ATTRIBUTE_TYPE.ToUpper() != "CATEGORY")
                                {
                                    // objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + brandname + TBW_ATTRIBUTE_VALUE, "pl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                    if ((_BCEAPath.ToLower().Contains("brand")))
                                    {
                                        objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + TBW_ATTRIBUTE_VALUE, "pl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                    }
                                    else
                                    {
                                        objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + brandname + TBW_ATTRIBUTE_VALUE, "pl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                    }
                                }
                                else
                                {
                                    objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_VALUE, "pl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                }
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
            // You Have Select
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
                    _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "YHSCell");
                    // _stmpl_records.SetAttribute("TBT_REMOVEEAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["RemoveEAPath"].ToString())));
                    // _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"].ToString());

                    _stmpl_records.SetAttribute("TBW_ITEM_TYPE", "Item " + ritemtypetostring);
                    //    if (row["ItemType"].ToString().ToLower() == "family")
                    //        _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["FamilyName"].ToString());
                    //    else if (row["ItemType"].ToString().ToLower() == "product")
                    //        _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ProductCode"].ToString());
                    //    else
                    //        _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString());

                    //    _stmpl_records.SetAttribute("TBT_URL", row["Url"].ToString());
                    //    _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["EAPath"].ToString())));

                    //    //Modified by:indu
                    //    //Start
                    //    try
                    //    {
                    //        string[] PAGENAME = row["Url"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
                    //        string stlistprod = string.Empty;
                    //        string ItemType = row["ItemType"].ToString().ToLower();
                    //        string RevItemType = Revrow["ItemType"].ToString().ToUpper();
                    //        _stmpl_records.SetAttribute("TBT_REM_ATTRIBUTE_TYPE", RevItemType);
                    //        if (breadcrumEA == "//// //// ////")
                    //        {
                    //            //if ((ItemType == "family") || (ItemType == "product") || (ItemType == "category") || (ItemType == "model") || (ItemType == "brand"))
                    //            //{
                    //            RemovebreadcrumEA = breadcrumEA;
                    //            breadcrumEA = breadcrumEA + _stmpl_records.GetAttribute("TBW_ITEM_NAME");
                    //            //}
                    //            //else
                    //            //{
                    //            //    RemovebreadcrumEA = breadcrumEA;
                    //            //    breadcrumEA = breadcrumEA + "////" + ItemType + "/" + _stmpl_records.GetAttribute("TBW_ITEM_NAME");
                    //            //}

                    //        }
                    //        else
                    //        {
                    //            //if ((ItemType == "family") || (ItemType == "product") || (ItemType == "category") || (ItemType == "model") || (ItemType == "brand"))
                    //            //{
                    //            RemovebreadcrumEA = breadcrumEA;
                    //            breadcrumEA = breadcrumEA + "////" + _stmpl_records.GetAttribute("TBW_ITEM_NAME");
                    //            //}
                    //            //else
                    //            //{
                    //            //    RemovebreadcrumEA = breadcrumEA;
                    //            //    breadcrumEA = breadcrumEA + "////" + ItemType + "/" + _stmpl_records.GetAttribute("TBW_ITEM_NAME");
                    //            //}
                    //        }

                    //        //objeasyask.Cons_NewURl(_stmpl_records, breadcrumEA, PAGENAME[0].ToLower(), true, false);
                    //        objHelperServices.Cons_NewURl(_stmpl_records, breadcrumEA, "BC" + "-" + PAGENAME[0].ToLower(), true, ItemType);
                    //        string[] REMOVEPAGENAME = row["RemoveUrl"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
                    //        //                                objeasyask.Cons_NewURl(_stmpl_records, breadcrumEA, REMOVEPAGENAME[0].ToLower(), true, true);
                    //        objHelperServices.Cons_NewURl(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + REMOVEPAGENAME[0].ToLower(), true, RevItemType);


                    //    }
                    //    catch
                    //    { }

                    //    //End 



                    //    lstrows1[cnt] = new TBWDataList1(_stmpl_records.ToString());
                    //    cnt = cnt + 1;

                    //}
                    string[] PAGENAME = row["Url"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);

                    string pagename = string.Empty;
                    pagename = PAGENAME[0].ToLower();

                    if (rowitype == "category" && i == 0)
                    {
                        _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                        Itemvalue = HttpUtility.UrlEncode(ritemvalue);
                    }
                    else if (rowitype == "family")
                    {
                        _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["FamilyName"]);
                        Itemvalue = ritemvalue + "=" + row["FamilyName"];
                        HttpContext.Current.Session["S_FName"] = row["FamilyName"];
                    }
                    else if (rowitype == "brand" && (pagename.Contains("ct.aspx") || pagename.Contains("microsite.aspx")))
                    {
                        _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                        Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);
                    }
                    //else if ((row["ItemType"].ToString().ToLower() == "category") && i == 0)
                    //{
                    //    _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"].ToString());
                    //    if (HttpContext.Current.Request.QueryString["cid"] != null)
                    //    {
                    //        Itemvalue = HttpContext.Current.Request.QueryString["cid"].ToString() + "=" + row["ItemValue"].ToString();
                    //    }
                    //    else
                    //    {
                    //        Itemvalue = row["ItemValue"].ToString();
                    //    }
                    //}
                    else if (rowitype == "product")
                    {
                        if (row["ProductCode"].ToString() != "")
                        {
                            _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ProductCode"]);
                            Itemvalue = ritemvalue + "=" + row["ProductCode"];
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                            Itemvalue = ritemvalue + "=" + HttpUtility.UrlEncode(ritemvalue);
                        }
                    }
                    else if ((rowitype != "category")
                        && (rowitype != "model")
                        && (rowitype != "brand")
                        && (IsFromBrand == true)
                        && ((Requrl.Contains("fl.aspx")) || Requrl.Contains("pd.aspx")))
                    {
                        _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                        Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);

                    }
                    else if (((rowitype != "brand")
                       && (rowitype != "model")
                        && (rowitype != "category")
                        )
                       && ((Requrl.Contains("ct.aspx"))
                        || (Requrl.Contains("microsite.aspx"))
                       || (Requrl.Contains("ps.aspx"))

                       || (Requrl.Contains("fl.aspx"))
                       || (Requrl.Contains("pd.aspx"))))
                    {
                        _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                        Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);

                    }
                    else if ((rowitype == "brand")
                        && (!(IsFromBrand))
                        && (!(Requrl.Contains("ct.aspx")))
                        && (!(Requrl.Contains("microsite.aspx"))))
                    //(row["ItemType"].ToString().ToLower() != "model")) && 
                    //((HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true) || 
                    //(HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx")) ||
                    //(HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) || 
                    //(HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx"))))
                    // else if (((row["ItemType"].ToString().ToLower() != "brand")) && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true) && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")))
                    {
                        _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                        Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);

                    }

                    else if ((rowitype != "category")
                        && (rowitype != "model")
                        && (rowitype != "user search")
                        && (!(rowitype.Contains("usersearch")))
                        && (IsFromproductlist == true || IsFromPowersearch == true))
                    {
                        _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                        Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);
                    }
                    else if ((rowitype == "model")
                        && (IsFromBrand == false))
                    {
                        //else if ((row["ItemType"].ToString().ToLower() == "model") && (((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")) || ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))))
                        //{
                        //else if ((row["ItemType"].ToString().ToLower() == "model") && (((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) && (IsFromBrand == false)) 
                        //    || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")) || ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))))
                        //{
                        _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                        Itemvalue = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("model = ", "model=").Replace("'", "");

                        Itemvalue = HttpUtility.UrlEncode(Itemvalue);
                        Itemvalue = Itemvalue.Trim();
                    }
                    else if (((rowitype != "brand")) && ((rowitype != "model")) && ((row["Url"].ToString().Contains("bb.aspx"))))
                    {
                        _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                        Itemvalue = ritemtypetostring + "=" + HttpUtility.UrlEncode(ritemvalue);

                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBW_ITEM_NAME", ritemvalue);
                        Itemvalue = HttpUtility.UrlEncode(ritemvalue);
                    }
                    //Itemvalue = Itemvalue;
                    //.Replace("+", "||").Replace("&", "^^").Replace(":", "~`");; ;

                    try
                    {
                        string itemtitle = _stmpl_records.GetAttribute("TBW_ITEM_NAME").ToString();
                        _stmpl_records.SetAttribute("TBW_ITEM_Title", itemtitle.Replace('"', ' '));
                    }
                    catch
                    { }
                    _stmpl_records.SetAttribute("TBT_REMOVEEAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["RemoveEAPath"].ToString())));
                    _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"]);
                    _stmpl_records.SetAttribute("TBT_URL", row["Url"]);
                    _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["EAPath"].ToString())));
                    _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", ritemtypetostring);


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
                        if (pagename.Contains("pl.aspx"))
                        {
                            newpagename = PAGENAME[0].ToLower();
                            breadcrumEA = breadcrumEA.ToLower().Replace("category=", "");
                            IsFromproductlist = true;
                        }
                        else if (pagename.Contains("pd.aspx"))
                        {
                            newpagename = PAGENAME[0].ToLower();
                            if (IsFromPowersearch == true)
                            {
                                breadcrumEA = breadcrumEA.ToUpper().Replace("USERSEARCH1=", "USERSEARCH=");
                            }


                        }
                        else if (pagename.Contains("fl.aspx"))
                        {
                            newpagename = PAGENAME[0].ToLower();
                            if ((IsFromPowersearch) && (!(RemovebreadcrumEA.Contains("USERSEARCH1="))))
                            {
                                breadcrumEA = breadcrumEA.ToUpper().Replace("//// //// ////", "//// //// ////" + "USERSEARCH1=");
                            }
                            else
                            {
                                if ((rowitype == "brand") && (!(breadcrumEA.ToLower().Contains("brand="))))
                                {
                                    istsb = row["ItemValue"].ToString();
                                    breadcrumEA = breadcrumEA.ToUpper().Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
                                }
                                else if ((IsFromBrand == true) && (istsb != string.Empty) && (istsm != string.Empty))
                                {
                                    if (!(breadcrumEA.ToUpper().Contains("MODEL=")))
                                    {
                                        breadcrumEA = breadcrumEA.ToUpper().Replace(istsm.ToUpper(), "MODEL=" + istsm.ToUpper());
                                    }
                                    if (!(breadcrumEA.ToUpper().Contains("BRAND=")))
                                    {
                                        breadcrumEA = breadcrumEA.ToUpper().Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
                                    }
                                }


                            }
                        }
                        else if (pagename.Contains("ct.aspx") || pagename.Contains("microsite.aspx"))
                        {
                            newpagename = PAGENAME[0].ToLower();
                            // breadcrumEA = breadcrumEA.Replace("Brand=", "");
                            if (Itemtype.ToLower() == "brand")
                            {
                                istsb = Itemvalue.ToUpper().Replace("BRAND=", "");
                                IsFromBrand = true;
                            }

                        }
                        else if (pagename.Contains("bb.aspx"))
                        {
                            newpagename = PAGENAME[0].ToLower();
                            IsFromBrand = true;
                            if (Itemtype.ToLower() == "model")
                            {
                                istsm = Itemvalue.ToUpper().Replace("MODEL=", "");
                            }
                            breadcrumEA = breadcrumEA.ToUpper().Replace("MODEL=", "").Replace("BRAND=", "");
                        }
                        else if (pagename.Contains("ps.aspx"))
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
                        objHelperServices.Cons_NewURl(_stmpl_records, breadcrumEA, "BC" + "-" + newpagename, true, Itemtype);
                        string[] REMOVEPAGENAME = row["RemoveUrl"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
                        string Rpagename = string.Empty;
                        Rpagename = REMOVEPAGENAME[0].ToLower();
                        if (Rpagename.Contains("pl.aspx"))
                        {

                            newremovepagename = REMOVEPAGENAME[0].ToLower();
                            IsFromproductlist_RC = true;
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
                            IsFromBrand_RC = true;
                        }
                        else if (Rpagename.Contains("bb.aspx"))
                        {
                            newremovepagename = REMOVEPAGENAME[0].ToLower();
                            RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("MODEL=", "");
                            IsFromBrand_RC = true;
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
                        objHelperServices.Cons_NewURl(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + newremovepagename.ToLower(), true, RevItemType);


                    }
                    catch (Exception ex)
                    {
                        objErrorHandler.ErrorMsg = ex;
                        objErrorHandler.CreateLog();
                    }


                    lstrows1[cnt] = new TBWDataList1(_stmpl_records.ToString());
                    cnt = cnt + 1;

                } //


            }

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
            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mpd.aspx") == true)
            {
                if (HttpContext.Current.Session["Category_Attributes"] == null && _parentCatID != "")
                {
                    EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
                }
            }
            if ((Request.Url.ToString().ToLower().Contains("mct.aspx") != true) && (Request.Url.ToString().ToLower().Contains("mpl.aspx") != true)
                   && (Request.Url.ToString().ToLower().Contains("mps.aspx") != true)
                   && (Request.Url.ToString().ToLower().Contains("mfl.aspx") != true)
                   && (Request.Url.ToString().ToLower().Contains("mpd.aspx") != true))
            {
                HttpCookie LoginInfoCookie = Request.Cookies["ActiveSupplier"];
                if (LoginInfoCookie != null && LoginInfoCookie["SupplierID"] != null)
                {
                    EasyAsk.GetMainMenuClickDetail(objSecurity.StringDeCrypt_password(LoginInfoCookie["SupplierID"]).ToString(), "");
                }

            }
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
                else if (_tsb != "")
                {
                    dscat = null;

                    //dscat = (DataSet)HttpContext.Current.Session["WESBrand_Model_Attributes"];

                }

            }
            else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mpd.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("mfl.aspx") == true)
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
                HttpContext.Current.Request.Url.ToString().ToLower().Contains("mforgotpassWord.aspx") == true)
            {
                return "";
            }
            else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mct.aspx") == true)
            {

                string Mainmenu = ST_MainMenu();
                string NewProductNav = ST_NewProductNav();

                sHTML = Mainmenu + NewProductNav;
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
                sHTML = "";
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
                                    else
                                    {
                                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell");
                                    }
                                    string brandname = string.Empty;
                                    if (dscat.Tables[i].TableName.Contains("Category"))
                                    {
                                        _stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                        _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                        //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));

                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr["Category_Name"].ToString());
                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
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
                                    }
                                    string TBW_ATTRIBUTE_TYPE = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_TYPE").ToString().ToUpper();
                                    string TBW_ATTRIBUTE_VALUE = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_VALUE").ToString();
                                    if (_tsm != string.Empty)
                                    {
                                        _BCEAPath = _BCEAPath.Replace(_tsm, HttpUtility.UrlEncode(_tsm));
                                    }

                                    if ((TBW_ATTRIBUTE_TYPE.ToUpper() == "CATEGORY") && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mbb.aspx")))
                                    {
                                        objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + TBW_ATTRIBUTE_VALUE, "mbb.aspx", true, TBW_ATTRIBUTE_TYPE);
                                    }

                                    else if (TBW_ATTRIBUTE_TYPE.ToUpper() != "CATEGORY")
                                    {
                                        // objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + brandname + TBW_ATTRIBUTE_VALUE, "pl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                        if (_BCEAPath.ToLower().Contains("brand") == true)
                                        {
                                            objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + TBW_ATTRIBUTE_VALUE, "mpl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                        }
                                        else
                                        {
                                            objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + brandname + TBW_ATTRIBUTE_VALUE, "mpl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                        }
                                    }
                                    else
                                    {
                                        objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_VALUE, "mpl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                    }
                                    //  objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + _stmpl_records.GetAttribute("TBW_ATTRIBUTE_NAME"), "pl.aspx", true, "");

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
                    bool TBTSHOPBY = false;
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
                                    string model = HttpUtility.UrlEncode(dr["TOSUITE_MODEL"].ToString());
                                    //.Replace("+", "||").Replace("&", "^^").Replace(":", "~`");; ;
                                    objHelperServices.Cons_NewURl(_stmpl_records, parentcat + "////" + Request.QueryString["tsb"] + "////" + model, "bb.aspx", true, "");
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
            if (HttpContext.Current.Session["MainMenuClick"] != null)
            {
                dsCat.Tables.Add((DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"].Copy());
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
                foreach (DataRow _drow in dsCat.Tables[0].Rows)
                {
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_BRAND"].ToString());
                    if (filterval != null && _drow["TOSUITE_BRAND"].ToString().ToLower() == filterval[0].ToString().ToLower())
                    {
                        _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                        selstate = true;
                        string eapath = "";
                        //eapath = HttpContext.Current.Session["EA"].ToString();
                        //if (eapath.Contains("////AttribSelect=Brand"))
                        //{
                        //    int inx = eapath.IndexOf("////AttribSelect=Brand");

                        //    eapath = eapath.ToString().Substring(0,inx);


                        //}                            
                        //eapath = eapath + "////AttribSelect=Brand='" + _drow["TOSUITE_BRAND"].ToString() + "'";

                        eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_drow["EA_PATH"].ToString()));

                        string[] parentcategory = _drow["EA_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                        string parentcat = parentcategory[2];
                        //modified by:indu 
                        //   Response.Redirect("ct.aspx?&ld=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&bypcat=1&path=" + eapath, false);

                        //Modified by:Indu
                        string originalurl = "/ct.aspx?&ld=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&bypcat=1&path=" + eapath;

                        //string stlistprod = objHelperServices.URLRewriteToAddressBar("ct.aspx", _drow["TOSUITE_BRAND"].ToString().ToUpper(), originalurl, HttpContext.Current.Server.MapPath("URL_Rewrite_Cat.ini"), true);
                        //Modified by:Indu
                        // string stlistprod = objHelperServices.Cons_NewURl_bybrand( _drow["TOSUITE_BRAND"].ToString().ToUpper(), originalurl, "");

                        string stlistprod = objHelperServices.Cons_NewURl_bybrand(originalurl, parentcat + "////" + "Brand=" + _drow["TOSUITE_BRAND"].ToString().ToUpper(), "ct.aspx", "");
                        string NEWURL = "/" + stlistprod + "/ct/";
                        //_stmpl_pages.SetAttribute("TBT_URL", stlistprod);
                        hfisselected.Value = "0";
                        Response.Redirect(NEWURL, false);
                        Session["PageRawURL"] = NEWURL.ToLower().Replace("ct/", "ct.aspx?").Replace("pl/", "pl.aspx?").Replace("fl/", "fl.aspx?").Replace("bb/", "bb.aspx?").Replace("pd/", "pd.aspx?").Replace("ps/", "ps.aspx?");
                    }
                    else if (filterval == null && Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "" && _drow["TOSUITE_BRAND"].ToString().ToLower() == Server.UrlDecode(Request.QueryString["tsb"].ToString().ToLower()) && selstate == false)
                    {
                        filterval = new string[2];
                        filterval[0] = _drow["TOSUITE_BRAND"].ToString();
                        filterval[1] = _drow["TOSUITE_BRAND"].ToString();
                        _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                        // Response.Redirect("ct.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                    }
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_BRAND"].ToString());
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
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_MODEL"].ToString());
                            if (filterval1 != null && _drow["TOSUITE_MODEL"].ToString() == filterval1[0].ToString())
                            {
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                selstate1 = true;
                                // Response.Redirect("pl.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                string eapath = string.Empty;
                                //eapath = HttpContext.Current.Session["EA"].ToString();
                                //if (eapath.Contains("////AttribSelect=Model"))
                                //{
                                //    int inx = eapath.IndexOf("////AttribSelect=Model");

                                //    eapath = eapath.ToString().Substring(0, inx );


                                //}
                                //eapath = eapath + "////AttribSelect=Model = '" + Request.QueryString["tsb"].ToString() + ":" + _drow["TOSUITE_MODEL"].ToString() + "'";

                                eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_drow["EA_PATH"].ToString()));
                                string[] parentcategory = _drow["EA_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                string parentcat = parentcategory[2];

                                //if (Request.QueryString["cid"] != null)
                                //{
                                //    DataTable dt1 = (DataTable)HttpContext.Current.Session["dtcid"];

                                //    DataRow[] foundRows;
                                //    string cid = Request.QueryString["cid"].ToString();
                                //    foundRows = dt1.Select("cid='" + cid + "' ");
                                //    if (foundRows.Length > 0)
                                //    {
                                //        parentcat = foundRows[0][0].ToString();

                                //    }
                                //}


                                //Modified By:Indu
                                //  Response.Redirect("bb.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&bypcat=1&path=" + eapath, false);
                                //Modified by:indu
                                string originalurl = "/bb.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&bypcat=1&path=" + eapath;

                                //string stlistprod = objHelperServices.URLRewriteToAddressBar("bb.aspx", _drow["TOSUITE_MODEL"].ToString().ToUpper() + "/" + Request.QueryString["tsb"].ToString().ToUpper(), originalurl, HttpContext.Current.Server.MapPath("URL_Rewrite_Brand.ini"), true);
                                string model = HttpUtility.UrlEncode(_drow["TOSUITE_MODEL"].ToString().ToUpper());
                                //.Replace("+", "||").Replace("&", "^^").Replace(":", "~`");; 
                                string stlistprod = objHelperServices.Cons_NewURl_bybrand(originalurl, parentcat + "////" + Request.QueryString["tsb"].ToString().ToUpper() + "////" + model, "bb.aspx", "");
                                string NEWURL = "/" + stlistprod + "/bb/";
                                //_stmpl_pages.SetAttribute("TBT_URL", stlistprod);
                                Response.Redirect(NEWURL, false);
                                Session["PageRawURL"] = NEWURL.Replace("bb/", "bb.aspx?");
                            }
                            else if (filterval1 == null && Request.QueryString["tsm"] != null && _drow["TOSUITE_MODEL"].ToString() == Server.UrlDecode(Request.QueryString["tsm"].ToString()) && selstate1 == false)
                            {
                                filterval1 = new string[2];
                                filterval1[0] = _drow["TOSUITE_MODEL"].ToString();
                                filterval1[1] = _drow["TOSUITE_MODEL"].ToString();
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                            }
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

}
