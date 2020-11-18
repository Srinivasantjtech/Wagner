﻿using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using StringTemplate = Antlr3.ST.StringTemplate;  
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
//using TradingBell.Common;
//using TradingBell.WebServices;
using System.Xml;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.EasyAsk ;
using System.Configuration;
using System.Collections;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Web.Caching;
using System.Globalization;
using System.Net;

namespace TradingBell.WebCat.TemplateRender
{
    /*********************************** J TECH CODE ***********************************/
    public class TBWTemplateEngine
    {
        /*********************************** DECLARATION ***********************************/
        EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
        ErrorHandler objErrorHandler = new ErrorHandler();
        OrderServices objOrderServices = new OrderServices();
        HelperServices objHelperServices = new HelperServices();
        UserServices objUserServices = new UserServices();
        FamilyServices objFamilyServices = new FamilyServices();
        ProductServices objProductServices = new ProductServices();
        public string brandname = "";
        //Stopwatch stopwatch = new Stopwatch(); 
        Dictionary<string, TBWDataList[]> _dict_inner_html = new Dictionary<string, TBWDataList[]>();

        private StringTemplateGroup _stg_container = null;
        private StringTemplateGroup _stg_records = null;
        private StringTemplate _stmpl_container = null;
        private StringTemplate _stmpl_records = null;

        private string _SkinRootPath = null;
        private string _SkinRoot_container = null;
        private string _SkinRoot_records = null;

        private string _RenderedHTML = null; //for this use the big string variable
        private string _DBConnectionString = null;
        private string _Package = null;

        //Get the following from TBWC_PACKAGE table 
        private string _skin_container = null;
        private int _grid_cols = 1;
        private int _grid_rows = 1;
        private string _skin_sql_container = null;
        private string _skin_sql_type_container = null;
        private string _skin_sql_param_container = null;
        private int _package_order = 0;
        private string _skin_body_attribute = null;

        private string _skin_records = null;
        private string _skin_sql_records = null;
        private string _skin_sql_type_records = null;
        private string _skin_sql_param_records = null;
        private DataSet _GDataSet = null;
        public string paraValue = string.Empty;
        public string paraPID = string.Empty;
        public string paraFID = string.Empty;
        public string paraCID = string.Empty;
        private string _cartitem = string.Empty;
        private string _CATALOG_ID = string.Empty;
        private HelperServices objHelperService = new HelperServices();
        private Security objSecurity = new Security();
        private HelperDB objHelperDB = new HelperDB();
        private ConnectionDB objConnectionDB = new ConnectionDB();
        string _fid = string.Empty;
        string downloadST = string.Empty;
        bool isdownload = false;
        bool chkdwld = false;
        public bool isfilter = false;
        public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"];
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"];
        string strProdImages = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
        //END TBWC_PACKAGE table info
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
        string strprodimg_sepdomain = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"];
        public string strxml = HttpContext.Current.Server.MapPath("xml");
        Stopwatch sw = new Stopwatch();
        DataSet dataset = new DataSet();
        public string cartitem
        {
            get
            {
                return _cartitem;
            }
            set
            {
                _cartitem = value;
            }
        }

        public string CATALOG_ID
        {
            get
            {
                return _CATALOG_ID;
            }
            set
            {
                _CATALOG_ID = value;
            }
        }

        public string DBConnectionString
        {
            get
            {
                return _DBConnectionString;
            }
            set
            {
                _DBConnectionString = value;
            }
        }

        public string Package
        {
            get
            {
                return _Package;
            }
            set
            {
                _Package = value;
            }
        }

        public string SkinRootPath
        {
            get
            {
                return _SkinRootPath;
            }
            set
            {
                _SkinRootPath = value;
            }
        }

        public string RenderedHTML
        {
            get
            {
                return objHelperService.StripWhitespace(_RenderedHTML);
            }
        }

        public DataSet GDataSet
        {
            get
            {
                return _GDataSet;
            }
            set
            {
                _GDataSet = value;
            }
        }
        /*********************************** DECLARATION ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PACKAGE,SKIN NAME AND DATABASE CONNECTION STRING  DETAILS ***/
        /********************************************************************************/
        public TBWTemplateEngine(string Package, string SkinRootPath, string DBConnectionString)
        {
            _Package = Package;
            _SkinRootPath = SkinRootPath;
            //_DBConnectionString = DBConnectionString.Substring(DBConnectionString.IndexOf(';') + 1);
            //DataSet ds = new DataSet();
            //SqlDataAdapter da = new SqlDataAdapter("select convert(int, option_value) as OPTION_VALUE from tbwc_store_options where option_name = 'DEFAULT CATALOG'", _DBConnectionString);
            //da.Fill(ds, "generictable");
            //_CATALOG_ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            _CATALOG_ID = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"];

        }
        public TBWTemplateEngine()
        {
            _CATALOG_ID = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"];

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK HTML TEMPLATE PAGE IS GENERATED OR NOT  ***/
        /********************************************************************************/
        public bool RenderHTML(string rType)
        {
            bool _status = false;
            string _sqlpkginfo;

            //_sqlpkginfo = " SELECT * FROM TBW_PACKAGE ";
            //_sqlpkginfo = _sqlpkginfo + " WHERE IS_ROOT = 0 AND PACKAGE_NAME = '" + _Package + "' ORDER BY PROCESS_ORDER ASC ";

            DataSet dspkg = new DataSet();
            try
            {
                //dspkg = GetDataSet(_sqlpkginfo);
                dspkg = (DataSet)objHelperDB.GetGenericDataDB(_Package, "GET_PACKAGE", HelperDB.ReturnType.RTDataSet);

                if (dspkg != null)
                {
                    if (dspkg.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dspkg.Tables[0].Rows)
                        {

                            _skin_container = dr["SKIN_NAME"].ToString();
                            _grid_cols = Convert.ToInt32(dr["GRID_COLS"]);
                            _grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
                            _skin_sql_container = dr["SKIN_SQL"].ToString();
                            _skin_sql_type_container = dr["SKIN_SQL_TYPE"].ToString();
                            _skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
                            _package_order = Convert.ToInt32(dr["PROCESS_ORDER"]);
                            _skin_body_attribute = dr["SKIN_BODY_ATTRIBUTE"].ToString();
                            _skin_records = dr["LIST_SKIN_NAME"].ToString();
                            _skin_sql_records = dr["LIST_SKIN_SQL"].ToString();
                            _skin_sql_type_records = dr["LIST_SKIN_SQL_TYPE"].ToString();
                            _skin_sql_param_records = dr["LIST_SKIN_SQL_PARAM"].ToString();

                            if (rType == "Column")
                                BuildRecordsTemplateColumn();
                            else
                                BuildRecordsTemplateRow();
                        }
                    }
                }

                if (BuildMainContainer())
                {
                    _RenderedHTML = _stmpl_container.ToString().Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

                    if (_Package == "NEWPRODUCT" || _Package == "CATEGORYLISTIMG" || _Package == "CSFAMILYPAGE" || _Package == "CSFAMILYPAGEWITHSUBFAMILY" || _Package == "PRODUCT")
                    {
                        if (_stmpl_container.ToString().Contains("data-original=\"/prodimages\""))
                        {
                            if (_Package == "CSFAMILYPAGE")
                                _RenderedHTML = _stmpl_container.ToString().Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages   /images/noimage.gif\"");
                            else
                                _RenderedHTML = _stmpl_container.ToString().Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                        }
                        if (_stmpl_container.ToString().Contains("data-original=\"\""))
                        {
                            _RenderedHTML = _stmpl_container.ToString().Replace("data-original=\"\"", "data-original=\"/prodimages/images/noimage.gif\"");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                        }
                    }
                    else
                    {
                        if (_stmpl_container.ToString().Contains("src=\"/prodimages\""))
                        {
                            if (_Package == "CSFAMILYPAGE")
                                _RenderedHTML = _stmpl_container.ToString().Replace("src=\"/prodimages\"", "src=\"" + System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "\\prodimages/images/noimage.gif\" style=\"display:none;\\");
                            else
                                //              _RenderedHTML = _stmpl_container.ToString().Replace("src=\"/prodimages\"", "src=<%=System.Configuration.ConfigurationManager.AppSettings['CDN'].ToString()%>+\"/prodimages/images/noimage.gif\"");
                                _RenderedHTML = _stmpl_container.ToString().Replace("src=\"/prodimages\"", "src=\"" + System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "\\prodimages/images/noimage.gif\"\\");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                        }
                        if (_stmpl_container.ToString().Contains("src=\"\""))
                        {
                            //                _RenderedHTML = _stmpl_container.ToString().Replace("src=\"\"", "src=<%=System.Configuration.ConfigurationManager.AppSettings[\"CDN\"].ToString()%>+\"/prodimages/images/noimage.gif\"");
                            _RenderedHTML = _stmpl_container.ToString().Replace("src=\"/prodimages\"", "src=\"" + System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "\\prodimages/images/noimage.gif\"\\");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                        }

                    }

                    /*DataSet DSnaprod = new DataSet();
                    DSnaprod = GetDataSet("SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_STATUS <> 'AVAILABLE'");
                    foreach (DataRow DDR in DSnaprod.Tables[0].Rows)
                    {
                        if (_RenderedHTML.Contains("<img src=\"images/but_buy1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_"))
                            _RenderedHTML = _RenderedHTML.Replace("<img src=\"images/but_buy1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_", " <img src=\"images/but_buy1.gif\" style=\"display:none\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_");

                        if (_RenderedHTML.Contains("<img src=\"images/but_buyitem1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_"))
                            _RenderedHTML = _RenderedHTML.Replace("<img src=\"images/but_buyitem1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_", " <img src=\"images/but_buyitem1.gif\" style=\"display:none\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_");

                        if (_RenderedHTML.Contains("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\""))
                            _RenderedHTML = _RenderedHTML.Replace("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\"", "<strong style=\" display:none\">QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "\" style=\" display:none\"");

                        if (_RenderedHTML.Contains("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\" size=\"5\" type=\"text\" id=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\"/>"))
                            _RenderedHTML = _RenderedHTML.Replace("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\" size=\"5\" type=\"text\" id=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\"/>", "&nbsp;");

                    }*/
                }
                _status = true;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                _status = false;
            }

            return _status;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER THE MAIN CONTENT IS GET BUILD OR NOT  ***/
        /********************************************************************************/
        private bool BuildMainContainer()
        {
            bool _status = false;
            string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
            string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
            string strPDFFiles2 = HttpContext.Current.Server.MapPath("News update");
            // string _sqlpkginfo;
            string package_name = _Package.ToString();
            //_sqlpkginfo = " SELECT TOP 1 * FROM TBW_PACKAGE ";
            //_sqlpkginfo = _sqlpkginfo + " WHERE IS_ROOT = 1 AND PACKAGE_NAME = '" + _Package + "' ORDER BY PROCESS_ORDER ASC ";

            DataSet dspkg = new DataSet();
            try
            {
                //dspkg = GetDataSet(_sqlpkginfo);
                dspkg = (DataSet)objHelperDB.GetGenericDataDB(_Package, "GET_MAIN_PACKAGE", HelperDB.ReturnType.RTDataSet);
                if (dspkg != null)
                {
                    if (dspkg.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dspkg.Tables[0].Rows)
                        {
                            _skin_container = dr["SKIN_NAME"].ToString();
                            _grid_cols = Convert.ToInt32(dr["GRID_COLS"]);
                            _grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
                            _skin_sql_container = dr["SKIN_SQL"].ToString();
                            _skin_sql_type_container = dr["SKIN_SQL_TYPE"].ToString();
                            _skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
                            _package_order = Convert.ToInt32(dr["PROCESS_ORDER"]);
                            _skin_body_attribute = dr["SKIN_BODY_ATTRIBUTE"].ToString();
                        }
                    }
                }

                if (_skin_container == string.Empty || _SkinRootPath == string.Empty || _skin_container == null || _SkinRootPath == null)
                {
                    return false;
                }
                //Build the outer body of the HTML - for main container
                _stg_container = new StringTemplateGroup(_skin_container, _SkinRootPath);
                DataSet dscontainer = null;
                if (_GDataSet != null && _GDataSet.Tables.Count > 1)
                {
                    dscontainer = _GDataSet;
                }
                else if (package_name.Equals("CSFAMILYPAGE"))
                {
                    dscontainer = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                }
                else if (package_name.Equals("CSFAMILYPAGEWITHSUBFAMILY"))
                {
                    dscontainer = null;
                    DataSet tempDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                    if (tempDs != null && tempDs.Tables["SubFamily"] != null)
                    {
                        DataRow[] dr = tempDs.Tables["SubFamily"].Select("FAMILY_ID='" + paraValue + "'");
                        if (dr.Length > 0)
                        {
                            dscontainer = new DataSet();
                            dscontainer.Tables.Add(dr.CopyToDataTable().Copy());
                        }

                    }
                }
                else
                {
                    dscontainer = GetDataSet(_skin_sql_container, _skin_sql_type_container, _skin_sql_param_container);
                }
                if (dscontainer != null)
                {
                    if (dscontainer.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dscontainer.Tables[0].Rows)
                        {
                            if (package_name.Equals("CSFAMILYPAGE"))
                            {
                                if (dr["STATUS"].ToString().ToUpper() == "TRUE")
                                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container + "1");
                                else if (dr["STATUS"].ToString().ToUpper() == "FALSE")
                                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                                else
                                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container + "2");

                            }
                            else

                                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                            if (HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"] == "")
                            {
                            }
                            else
                            {

                                if (package_name.Equals("BOTTOM"))
                                {
                                    if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                                        _stmpl_container.SetAttribute("TBT_RET_PRO", false);
                                    else
                                        _stmpl_container.SetAttribute("TBT_RET_PRO", true);
                                }
                                if (package_name.Equals("BOTTOM") && !HttpContext.Current.Session["USER_ID"].Equals(""))
                                {
                                    if (HttpContext.Current.Session["USER_ID"].ToString().Equals(System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                                        _stmpl_container.SetAttribute("TBT_DUMUSER_CHECK", false);
                                    else
                                        _stmpl_container.SetAttribute("TBT_DUMUSER_CHECK", true);
                                }
                            }



                            if (package_name.Equals("CSFAMILYPAGE"))
                            {

                                DataSet tempdscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                                if (tempdscat != null && tempdscat.Tables.Count > 0)
                                {
                                    if (tempdscat.Tables.Count == 1 && tempdscat.Tables[0].Rows.Count > 0)
                                        _stmpl_container.SetAttribute("TBT_LHS_TIP", true);
                                    else if (tempdscat.Tables.Count > 1 && tempdscat.Tables[1].Rows.Count > 0)
                                        _stmpl_container.SetAttribute("TBT_LHS_TIP", true);
                                    else
                                        _stmpl_container.SetAttribute("TBT_LHS_TIP", false);
                                }
                                else
                                    _stmpl_container.SetAttribute("TBT_LHS_TIP", false);


                                if (HttpContext.Current.Request.QueryString["fid"] != null)
                                {
                                    _fid = HttpContext.Current.Request.QueryString["fid"].ToString();

                                    DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                                    if (tmpds != null && tmpds.Tables.Count > 0)
                                    {
                                        _stmpl_container.SetAttribute("TBT_FAMILY_ID", _fid);
                                        //  objHelperService.Cons_NewURl(_stmpl_container, _stmpl_container.GetAttribute("TBT_FAMILY_ID").ToString() + "////" + _fid, "fl.aspx", true, "", false, false);
                                        string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
                                        _stmpl_container.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                                        _stmpl_container.SetAttribute("TBT_ORGEA_PATH", eapath);
                                        string bceapath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();
                                        string[] catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                                        objHelperService.SimpleURL(_stmpl_container, "AllProducts////WESAUSTRALASIA////" + _fid + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "Family", "fl.aspx");

                                        //  objHelperService.Cons_NewURl(_stmpl_container, _stmpl_container.GetAttribute("TBT_ORGEA_PATH").ToString() + "////" + _fid+"="+"Family", "fl.aspx", true, "", false, false);
                                        // objHelperService.Cons_NewURl(_stmpl_container, bceapath + "////" + _fid, "fl.aspx", true, "", false, false);

                                    }

                                }

                            }

                            _stmpl_container.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());


                            if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].Equals("") && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
                            {
                                _stmpl_container.SetAttribute("TBT_LOGIN_NAME", GetLoginName());
                            }

                            if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                            {
                                if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString()) == 4)
                                {
                                    string ReMailLink = "<a Href=ConfirmMessage.aspx?Result=REMAILACTIVATION class=\"linkemailre\">Re-Email Activation Link Now</a>";

                                    _stmpl_container.SetAttribute("TBT_LOGIN_NAME", " Your Account Has Not Been Activated! " + ReMailLink);
                                }
                                else
                                    _stmpl_container.SetAttribute("TBT_COMPANY_NAME", "");
                            }
                            else
                            {
                                if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
                                {
                                    _stmpl_container.SetAttribute("TBT_COMPANY_NAME", GetCompanyName() + ",");
                                }
                            }

                            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "" && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
                            {
                                string lname = GetLoginName();
                                HttpContext.Current.Session["LOGIN_NAME"] = lname;
                                HttpContext.Current.Session["LOGIN_NAME_TOP"] = lname;
                            }

                            foreach (DataColumn dc in dr.Table.Columns)
                            {
                                _stmpl_container.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc.ColumnName.ToString()].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>"));
                            }


                        }


                        if (dscontainer.Tables[0].Columns.Contains("attribute_name"))
                        {
                            string descall = string.Empty;
                            // string descalltrim = "";
                            string desc1 = string.Empty;
                            string descallstring = string.Empty;
                            string Att_name = string.Empty;
                            foreach (DataRow dr in dscontainer.Tables[0].Rows)
                            {
                                desc1 = "";
                                Att_name = "";
                                Att_name = dr["ATTRIBUTE_NAME"].ToString().ToUpper();
                                if (dr["ATTRIBUTE_TYPE"].ToString().Equals("3") || dr["ATTRIBUTE_TYPE"].ToString().Equals("9"))
                                {
                                    FileInfo Fil;
                                    if (package_name.Equals("CSFAMILYPAGE"))
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                    }
                                    else
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString());
                                    }
                                    // if (Fil.Exists)
                                    // {
                                    if (package_name.Equals("CSFAMILYPAGE"))
                                    {
                                        //_stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                    }
                                    else
                                        _stmpl_container.SetAttribute("TBT_" + Att_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_"), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                    // }
                                    //else
                                    //{
                                    //    _stmpl_container.SetAttribute("TBT_" + Att_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_"), "");
                                    //}

                                }
                                else
                                {
                                    // 


                                    if (package_name.Equals("CSFAMILYPAGE"))
                                    {
                                        if (Att_name.Equals("DESCRIPTIONS") || Att_name.Equals("FEATURES") || Att_name.Equals("SPECIFICATION") || Att_name.Equals("SPECIFICATIONS") || Att_name.Equals("APPLICATIONS") || Att_name.Equals("NOTES"))
                                        {
                                            desc1 = dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                            desc1 = desc1.ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                                        }
                                        else
                                            _stmpl_container.SetAttribute("TBT_" + Att_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_"), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                                    }
                                    else
                                    {
                                        _stmpl_container.SetAttribute("TBT_" + Att_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_"), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));

                                    }
                                    //if (Att_name == "short description" || Att_name == "short description1" || Att_name == "note" || Att_name == "Notes" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "DESCRIPTIONS")
                                    //{

                                    //    desc1 = dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                    //    _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                                    //   // _stmpl_container.SetAttribute("TBT_MORE", desc1);


                                    //}
                                    //else if (dr["ATTRIBUTE_NAME"].ToString() == "Descriptions1" || dr["ATTRIBUTE_NAME"].ToString() == "DescriptionTemp")
                                    //{
                                    //    desc1 = dr["STRING_VALUE"].ToString().Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                    //}
                                    //else
                                    //{
                                    //    _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                                    //}
                                }

                                // if (dr["ATTRIBUTE_NAME"].ToString() == "Short Description" || dr["ATTRIBUTE_NAME"].ToString() == "Short Description1" || dr["ATTRIBUTE_NAME"].ToString() == "Note" || dr["ATTRIBUTE_NAME"].ToString() == "Notes" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "DESCRIPTIONS" || dr["ATTRIBUTE_NAME"].ToString() == "Descriptions1" || dr["ATTRIBUTE_NAME"].ToString() == "DescriptionTemp")
                                if (package_name.Equals("CSFAMILYPAGE"))
                                {

                                    if (!desc1.Equals(""))
                                        descall = descall + desc1 + "<br/><br/>";
                                }

                            }
                            if (package_name.Equals("CSFAMILYPAGE"))
                            {
                                if (!desc1.Equals(""))
                                {
                                    descall = descall.Trim();
                                    descall = descall.Substring(0, descall.Length - 5);
                                }


                                if (descall.Length > 1080)
                                {
                                    //  _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                                    descallstring = descall.Substring(0, 1080).ToString();
                                    _stmpl_container.SetAttribute("TBT_MORE", descallstring);
                                    _stmpl_container.SetAttribute("TBT_MENU_ID", "2");
                                    descall = descall.Substring(0, descall.Length).ToString();
                                    // descall = descall.Substring(300).ToString();

                                    _stmpl_container.SetAttribute("TBT_DESCALL", descall);

                                }
                                else
                                {
                                    _stmpl_container.SetAttribute("TBT_DESCALL", descall);
                                    _stmpl_container.SetAttribute("TBT_MENU_ID", "2");
                                    // _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);
                                    _stmpl_container.SetAttribute("TBT_MORE", descall);
                                }
                                if (descall.Length > 1080)
                                    _stmpl_container.SetAttribute("TBT_MORE_SHOW", true);
                                else
                                    _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);


                            }

                        }

                    }
                }
                if (package_name.Equals("BROWSEBYCATEGORY") || package_name.Equals("BROWSEBYBRAND") || package_name.Equals("BROWSEBYPRODUCT"))
                {
                    /*string bbvalue = "", catName = ""; int recvalue = 0; string cidvalue = "";
                    DataSet DSDR = null;
                    catName = paraValue;
                    DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catName + "'");
                    if (DSDR != null && DSDR.Tables[0].Rows.Count>=1   )
                    {
                        while (DSDR.Tables[0].Rows[0].ItemArray[1].ToString() != "0")
                        {
                            DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + DSDR.Tables[0].Rows[0].ItemArray[1].ToString() + "'");
                        }
                        bbvalue = "<h3 class=\"headerbar\"> " + DSDR.Tables[0].Rows[0]["CATEGORY_NAME"].ToString() + "</h3>";
                        cidvalue = DSDR.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                        #region comments
                        ////do
                        //{
                        //    DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraValue + "'");
                        //    foreach (DataRow DR in DSDR.Tables[0].Rows)
                        //    {
                        //        if (DR["PARENT_CATEGORY"].ToString() == "0")
                        //            if (recvalue > 0)
                        //            {
                        //                bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //                catName = DR["CATEGORY_NAME"].ToString();
                        //            }
                        //            else
                        //            {
                        //                bbvalue = "<h3 class=\"headerbar\">&raquo; " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //                catName = DR["CATEGORY_NAME"].ToString();
                        //            }
                        //        else
                        //        {
                        //            bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //            catName = DR["CATEGORY_NAME"].ToString();
                        //        }
                        //        //if (DR["PARENT_CATEGORY"].ToString() == "0")
                        //        //    if(recvalue >0)
                        //        //        bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //        //    else
                        //        //        bbvalue= "<h3 class=\"headerbar\">&raquo; " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //        //else
                        //        //    bbvalue = "<h3 class=\"headerbar2\">" + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //    }
                        //    paraValue = DSDR.Tables[0].Rows[0].ItemArray[1].ToString(); recvalue++;
                        //    DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraValue + "'");
                        //    foreach (DataRow DR in DSDR.Tables[0].Rows)
                        //    {
                        //        string Convalue = "pl";
                        //        if (_Package == "BROWSEBYBRAND")
                        //            Convalue = "bb";
                        //        if (_Package == "BROWSEBYPRODUCT")
                        //            Convalue = "byproduct";

                        //        if (catName.ToLower() == "brand" || catName.ToLower() == "product")
                        //        {
                        //            bbvalue = "<h3 class=\"headerbar\">&raquo; " + catName + "</h3>";

                        //         //   bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + catName + "</h3>";
                        //        }
                        //        else
                        //            bbvalue = "<h3 class=\"headerbar\"><A HREF=\"" + Convalue + ".aspx?&ld=0&&cid=" + DR["CATEGORY_ID"].ToString() + "\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + catName + "</a></h3>";
                        //        //bbvalue = bbvalue.Replace(catName, "<A HREF=\"pl.aspx?&ld=0&&cid=" + DR["CATEGORY_ID"].ToString() + "\">" + catName + "</a>");                            
                        //    }
                        //}//while(DSDR.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                        #endregion
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_NAME", bbvalue);
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_ID", cidvalue);
                    }
                    else
                    {
                        _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_NAME", "");
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_ID", "");
                    }*/
                }
                _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);
                //Lets get the block inner elements from the dictionary and finish building the main container

                if (package_name.Equals("CATEGORYLISTIMG"))
                {
                    DataSet DSDR = new DataSet();
                    //DSDR = GetDataSet("SELECT CATEGORY_ID,CATEGORY_NAME,IMAGE_FILE,SHORT_DESC,IMAGE_FILE2 FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraValue.ToString() + "'");

                    // DataRow[] row = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + paraValue.ToString() + "'");
                    DataSet ds = (DataSet)HttpContext.Current.Application["key_MainCategory"];
                    DataRow[] row = ds.Tables[0].Select("CATEGORY_ID='" + paraValue.ToString() + "'");
                    if (row.Length > 0)
                    {
                        DSDR.Tables.Add(row.CopyToDataTable());
                    }


                    if (DSDR != null && DSDR.Tables.Count > 0)
                    {



                        foreach (DataRow _dsrow in DSDR.Tables[0].Rows)
                        {
                            _stmpl_container.SetAttribute("TBT_CATEGORY_NAME", _dsrow["CATEGORY_NAME"].ToString());
                            _stmpl_container.SetAttribute("TBT_CATEGORY_ID", _dsrow["CATEGORY_ID"].ToString());
                            _stmpl_container.SetAttribute("TBT_SHORT_DESC", _dsrow["SHORT_DESC"].ToString());

                            string prodImage = string.Empty;

                            if (_dsrow["IMAGE_FILE"].ToString() != null && _dsrow["IMAGE_FILE"].ToString() != String.Empty && _dsrow["IMAGE_FILE"].ToString() != "")
                            {
                                String strfile1 = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + _dsrow["IMAGE_FILE"].ToString().Replace("\\", "/");
                                //bool ImageExits = objHelperServices.CheckImageExistCDN(strfile1);
                                //if (ImageExits)
                                prodImage = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + _dsrow["IMAGE_FILE"].ToString().Replace("\\", "/");
                                //else
                                //    prodImage = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";
                            }
                            else
                            {
                                prodImage = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";
                            }

                            //FileInfo Fil = new FileInfo(strFile + _dsrow["IMAGE_FILE"].ToString());
                            //if (Fil.Exists)
                            //{
                            _stmpl_container.SetAttribute("TBT_IMAGE_FILE1", prodImage);
                            //objErrorHandler.CreateLog(prodImage);
                            //}
                            //else
                            //{
                            //    _stmpl_container.SetAttribute("TBT_IMAGE_FILE1", "");
                            //}

                            /* For Category PDF file attachment (checking)  */
                            FileInfo Fil2 = new FileInfo(strPDFFiles1 + _dsrow["IMAGE_FILE2"].ToString());
                            FileInfo Fil3 = new FileInfo(strPDFFiles2 + _dsrow["IMAGE_FILE2"].ToString());
                            if (Fil2.Exists || Fil3.Exists)
                            {
                                _stmpl_container.SetAttribute("TBT_PDF_STATUS", true);
                            }
                            else
                            {
                                _stmpl_container.SetAttribute("TBT_PDF_STATUS", false);
                            }

                        }
                    }

                }

                string _Tbt_Order_Id = string.Empty;
                string _Tbt_Ship_URL = string.Empty;
                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                //Modified by:indu
                if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                {
                    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                }
                else
                {
                    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();
                    //    HttpContext.Current.Session["ORDER_ID"] = _Tbt_Order_Id;
                }

                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                {
                    _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                }
                else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
                {
                    _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                }
                else
                {
                    _Tbt_Ship_URL = "shipping.aspx";
                }

                _stmpl_container.SetAttribute("TBT_ORDER_ID", _Tbt_Order_Id);
                _stmpl_container.SetAttribute("TBT_SHIP_URL", _Tbt_Ship_URL);
                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());

                if (HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"].Equals(""))
                    _stmpl_container.SetAttribute("TBT_LOGIN", false);
                else
                    _stmpl_container.SetAttribute("TBT_LOGIN", true);

                #region "breadcrumb"
                string breadcrumb = string.Empty;
                /*if (HttpContext.Current.Request.Url.ToString().Contains("pd.aspx") == true && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request["tsb"].ToString() != "" && HttpContext.Current.Request["tsm"] != null && HttpContext.Current.Request["tsm"].ToString() != "" && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "" && HttpContext.Current.Request["sl2"] != null && HttpContext.Current.Request["sl2"].ToString() != "")
                {
                    DataSet DSBC = null;
                    if (paraCID != "")
                    {
                        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2&bypcat=1\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                            breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2&bypcat=1\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "</a>";
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&byp=2\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "</a>";
                        string sql = "";
                        if (HttpContext.Current.Request["sl2"].ToString() != "0")
                        {
                            sql = "SELECT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE TOSUITE_BRAND='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "' AND CATEGORY_ID='" + paraCID + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND PRODUCT_ID=" + paraPID.ToString();
                        }
                        else
                        {
                            sql = "SELECT SUBCATNAME_L1 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE TOSUITE_BRAND='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "' AND CATEGORY_ID='" + paraCID + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='' AND PRODUCT_ID=" + paraPID.ToString();
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2\">" + GetDataSet(sql).Tables[0].Rows[0][0].ToString() + "</a>";
                    }
                    if (paraPID != "")
                    {
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                        }
                    }
                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("pd.aspx") == true && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request["tsb"].ToString() != "" && HttpContext.Current.Request["tsm"] != null && HttpContext.Current.Request["tsm"].ToString() != "")
                {
                    DataSet DSBC = null;
                    if (paraCID != "")
                    {
                        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2&bypcat=1\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                            breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2&bypcat=1\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "</a>";
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&byp=2\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "</a>";
                    }
                    if (paraPID != "")
                    {
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                        }
                    }

                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("pd.aspx") == true && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request["tsb"].ToString() != "")
                {
                    DataSet DSBC = null;
                    if (paraCID != "")
                    {
                        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2&bypcat=1\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                            breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "</a>";
                    }
                    if (paraPID != "")
                    {
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + " and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                        }
                    }
                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("fl.aspx") == true && _Package == "CSFAMILYPAGE" && HttpContext.Current.Request.QueryString["sl1"] != null && HttpContext.Current.Request.QueryString["sl1"].ToString() != "" && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl2"].ToString() != "")
                {
                    DataSet DSBC = null;
                    DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + HttpContext.Current.Request.QueryString["cid"].ToString() + "'");
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() +"&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                    }
                    string sql = "SELECT DISTINCT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + HttpContext.Current.Request.QueryString["cid"].ToString() + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "'";
                    breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"pl.aspx?&ld=0&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + GetDataSet(sql).Tables[0].Rows[0][0].ToString() + "</a>";

                    DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"fl.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&by=2&qf=1\" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                    }

                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("pd.aspx") == true && _Package == "PRODUCT" && HttpContext.Current.Request.QueryString["sl1"] != null && HttpContext.Current.Request.QueryString["sl1"].ToString() != "" && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl2"].ToString() != "")
                {
                    DataSet DSBC = null;
                    DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + HttpContext.Current.Request.QueryString["cid"].ToString() + "'");
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                    }
                    string sql = "SELECT DISTINCT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + HttpContext.Current.Request.QueryString["cid"].ToString() + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "'";
                    breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"pl.aspx?&ld=0&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + GetDataSet(sql).Tables[0].Rows[0][0].ToString() + "</a>";
                    if (HttpContext.Current.Request.QueryString["tf"] != null && HttpContext.Current.Request.QueryString["tf"].ToString() != "")
                    {
                        DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"fl.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&by=2&qf=1\" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        }
                    }
                    DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + " and attribute_id=1");
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                    }
                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("fl.aspx") == true && HttpContext.Current.Request.QueryString["sl1"] != null && HttpContext.Current.Request.QueryString["sl1"].ToString() != "" && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl2"].ToString() != "")
                {

                }
                else
                {
                    if (paraPID != "")
                    {
                        DataSet DSBC = null;
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                            {
                                if (HttpContext.Current.Request.QueryString["byp"] != null && HttpContext.Current.Request.QueryString["byp"].ToString() != "")
                                {
                                    if (HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"].ToString() != "")
                                    {
                                        breadcrumb = "<a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                                    }
                                    else
                                        breadcrumb = "<a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                                }
                                else
                                {
                                    breadcrumb = "<a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                                }
                            }
                            else
                            {
                                breadcrumb = "<a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                            }
                        }
                        if (paraFID != "")
                        {
                            string catIDtemp = "";
                            DSBC = GetDataSet("SELECT family_name,category_id FROM TB_family WHERE family_ID = " + paraFID);
                            foreach (DataRow DR in DSBC.Tables[0].Rows)
                            {
                                if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                                {
                                    if (HttpContext.Current.Request.QueryString["byp"] != null && HttpContext.Current.Request.QueryString["byp"].ToString() != "")
                                    {
                                        if (HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"].ToString() != "")
                                            breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        else
                                            breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                    }
                                    else
                                    {
                                        breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                    }
                                }
                                else
                                {
                                    breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                }
                                catIDtemp = DR[1].ToString();
                            }
                            do
                            {
                                DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                                foreach (DataRow DR in DSBC.Tables[0].Rows)
                                {
                                    if (DR["PARENT_CATEGORY"].ToString() != "0")
                                    {
                                        if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                                        {
                                            breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                        else
                                        {
                                            breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                    }
                                    else
                                    {
                                        if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                                        {
                                            breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                        else
                                        {
                                            breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                    }
                                    catIDtemp = DR["PARENT_CATEGORY"].ToString();
                                }
                            } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                        }
                    }
                    else if (paraFID != "")
                    {
                        //DataSet DSBC = null;
                        //string catIDtemp = "";
                        //DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
                        //foreach (DataRow DR in DSBC.Tables[0].Rows)
                        //{
                        //    if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                        //    {
                        //        if (HttpContext.Current.Request.QueryString["byp"] != null && HttpContext.Current.Request.QueryString["byp"].ToString() != "")
                        //        {
                        //            if (HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"].ToString() != "")
                        //                breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //            else
                        //                breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //        }
                        //        else
                        //        {
                        //            breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //    }
                        //    catIDtemp = DR[1].ToString();
                        //}
                        //do
                        //{
                        //    DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                        //    foreach (DataRow DR in DSBC.Tables[0].Rows)
                        //    {
                        //        if (DR["PARENT_CATEGORY"].ToString() != "0")
                        //        {
                        //            if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                        //            {
                        //                breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&qf=1 \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //            else
                        //            {
                        //                breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                        //            {
                        //                //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //                breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //            else
                        //            {
                        //                breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //        }
                        //        catIDtemp = DR["PARENT_CATEGORY"].ToString();
                        //    }
                        //} while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                    }
                    else if (paraCID != "")
                    {
                        DataSet DSBC = null;
                        string catIDtemp = paraCID;
                        do
                        {
                            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                            foreach (DataRow DR in DSBC.Tables[0].Rows)
                            {
                                if (DR["PARENT_CATEGORY"].ToString() != "0")
                                {
                                    if (breadcrumb == "")
                                        breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                                    else
                                        breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                }
                                else
                                {
                                    if (breadcrumb == "")
                                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                                    else
                                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;

                                }
                                catIDtemp = DR["PARENT_CATEGORY"].ToString();
                            }
                        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                    }
                }*/
                #endregion
                _stmpl_container.SetAttribute("TBT_BREAD_CRUMBS", breadcrumb.Replace("<ars>g</ars>", "&rarr;"));

                foreach (KeyValuePair<string, TBWDataList[]> kvp in _dict_inner_html)
                {
                    _stmpl_container.SetAttribute(kvp.Key, kvp.Value);
                }

                _status = true;
                if (package_name.Equals("CSFAMILYPAGE"))
                {
                    // GetFamilyMultipleImages(Convert.ToInt32(paraFID), _stmpl_container);
                }

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                _status = false;
            }
            return _status;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO BUILD TEMPLATE RECORDS IN COLUMN WISE  ***/
        /********************************************************************************/
        private void BuildRecordsTemplateColumn()
        {
            TBWDataList[] lstrecords = new TBWDataList[0];
            string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
            //Build the cell inner body of the HTML
            _stg_records = new StringTemplateGroup(_skin_records, _SkinRootPath);
            DataSet dsrecords = null;
            string package_name = _Package.ToString();
            if (_GDataSet != null && _skin_sql_records.Length == 0)
            {
                dsrecords = _GDataSet;
            }
            else if (package_name.Equals("PRODUCT"))
            {
                dsrecords = (DataSet)HttpContext.Current.Session["FamilyProduct"];

            }
            else if (package_name.Equals("NEWPRODUCTLOGNAV"))
            {
                dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_LOG_NAV");
            }
            else if (package_name.Equals("NEWPRODUCTHIGHLIGHTSCATLIST"))
            {
                dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRO_HIGHLIGHTS_CAT_LIST");
            }
            else if (package_name.Equals("NEWPRODUCT"))
            {
                string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"];
                dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT 10" + "," + websiteid);
            }
            else if (package_name.Equals("NEWPRODUCTNAV"))
            {
                dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_NAV");
            }
            else
            {
                dsrecords = GetDataSet(_skin_sql_records, _skin_sql_type_records, _skin_sql_param_records);
            }
            _stg_container = new StringTemplateGroup(_skin_container, _SkinRootPath);
            string brandname = string.Empty;
            if (HttpContext.Current.Request.QueryString["bname"] != null)
                brandname = HttpContext.Current.Request.QueryString["bname"].ToString();
            if (dsrecords != null)
            {


                if (dsrecords.Tables[0].Rows.Count > 0)
                {
                    DataRow[] cellrow = dsrecords.Tables[0].Select("ATTRIBUTE_ID = 1");
                    //  DataRow[] cellrow_np = dsrecords.Tables[0].Select("ATTRIBUTE_ID = 450");

                    lstrecords = new TBWDataList[cellrow.Length];
                    int ictrecords = 0, ictcol = 1; string strValue = string.Empty;

                    foreach (DataRow cdr in cellrow)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records);

                        _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());

                        if (!package_name.Equals("PRODUCT"))
                            _stmpl_records.SetAttribute("TBT_YOURCOST", GetMyPrice(System.Convert.ToInt32(cdr["PRODUCT_ID"])));
                        //modify
                        if (package_name.Equals("PRODUCT"))
                        {
                            try
                            {

                                GetMultipleImages(System.Convert.ToInt32(cdr["PRODUCT_ID"]), System.Convert.ToInt32(paraFID), System.Convert.ToInt32(cdr["Family_ID"]), _stmpl_records);
                            }
                            catch
                            { }
                            if (brandname != string.Empty)
                            {
                                _stmpl_records.SetAttribute("BRAND_NAME", brandname);
                                _stmpl_records.SetAttribute("TBT_BRAND_NAME", true);
                            }
                            else
                                _stmpl_records.SetAttribute("TBT_BRAND_NAME", false);

                            if (HttpContext.Current.Request.QueryString["fid"] != null)
                            {
                                _fid = HttpContext.Current.Request.QueryString["fid"].ToString();

                                DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                                if (tmpds != null && tmpds.Tables.Count > 0)
                                {
                                    _stmpl_records.SetAttribute("TBT_CATEGORY_ID", tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());
                                    string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
                                    _stmpl_records.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                                    _stmpl_records.SetAttribute("ORG_TBT_EA_PATH", eapath);
                                }

                            }

                        }

                        // comment start
                        //  if (package_name.Equals("NEWPRODUCTLOGNAV") || package_name.Equals("NEWPRODUCTNAV") || package_name.Equals("NEWPRODUCTHIGHLIGHTSCATLIST"))
                        //  {

                        //      //Added by Indu
                        //     _stmpl_records.SetAttribute("TBT_FAMILY_ID", cdr["family_id"].ToString());
                        //     //
                        //      string espath="AllProducts////WESAUSTRALASIA////" + cdr["CATEGORY_PATH"].ToString();
                        //      _stmpl_records.SetAttribute("FAMILY_EA_PATH" , HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));
                        ////
                        //      objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" + cdr["FAMILY_ID"].ToString()+"="+cdr["FAMILY_name"].ToString() ,"fl.aspx", true, "",false,true);
                        //     //
                        //      espath = "AllProducts////WESAUSTRALASIA////" + cdr["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + cdr["FAMILY_ID"].ToString();
                        //      _stmpl_records.SetAttribute("PRODUCT_EA_PATH",  HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));
                        //     //
                        //      string pcode = string.Empty;
                        //      if (cdr["attribute_name"].ToString().ToUpper() == "CODE")
                        //      {

                        //          pcode = cdr["string_value"].ToString();
                        //      }

                        //    objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" +cdr["FAMILY_ID"].ToString()+"="+ cdr["FAMILY_name"].ToString() + "////" + cdr["product_ID"].ToString()+"="+pcode, "pd.aspx", true, "");
                        //  //
                        //  }
                        //if (package_name.Equals("NEWPRODUCT"))
                        //{
                        //    string pcode = string.Empty;
                        //    if (cdr["attribute_name"].ToString().ToUpper() == "CODE")
                        //    {
                        //        pcode = cdr["string_value"].ToString();
                        //    }
                        //    string espath = "AllProducts////WESAUSTRALASIA////" + cdr["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + cdr["FAMILY_ID"].ToString();
                        //  //  objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" + cdr["FAMILY_name"].ToString(), "fl.aspx", true, "");
                        //    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                        //}
                        // comment end

                        string dccolnameupp = string.Empty;
                        foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                        {
                            string dccolname = string.Empty;
                            dccolname = dc.ColumnName.ToString();
                            dccolnameupp = string.Empty;
                            dccolnameupp = dccolname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();
                            _stmpl_records.SetAttribute("TBT_" + dccolnameupp, cdr[dccolname].ToString().Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""));

                            if (dccolname.ToUpper() == "FAMILY_NAME" && package_name.Equals("PRODUCT"))
                            {
                                HttpContext.Current.Session["FNAME_PRODUCT"] = cdr[dccolname].ToString();
                            }
                            // comment start
                            //if (package_name.Equals("COMPARE") && dccolname.ToUpper().Equals("PRODUCT_ID"))
                            //{
                            //    //DataSet Dsfamilyname = GetDataSet("SELECT TOP(1) FAMILY_ID FROM TB_PROD_FAMILY WHERE PRODUCT_ID =" + cdr["Product_ID"].ToString() + " AND FAMILY_ID IN(SELECT DISTINCT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATEGORY_ID IN (SELECT CATEGORY_ID FROM CATEGORY_FUNCTION(" + CATALOG_ID + ", '" + cdr["CATEGORY_ID"].ToString() + "')))");
                            //    DataSet Dsfamilyname = (DataSet)objHelperDB.GetGenericDataDB(_CATALOG_ID, cdr["Product_ID"].ToString(), cdr["CATEGORY_ID"].ToString(), "GET_FAMILY_ID_COMPARE_PACKAGE", HelperDB.ReturnType.RTDataSet);
                            //    if (Dsfamilyname != null && Dsfamilyname.Tables[0].Rows.Count > 0)
                            //    {
                            //        _stmpl_records.SetAttribute("TBT_FAMILY_ID", Dsfamilyname.Tables[0].Rows[0][0].ToString());
                            //    }
                            //}
                            // comment end

                            // comment start
                            //else if ((package_name.Equals("NEWPRODUCT") || package_name.Equals("PROMOTIONS") || package_name.Equals("MOREPRODUCTS")) && dccolname.ToUpper().Equals("PRODUCT_ID"))
                            //{

                            //    DataSet Dsfamilyname = (DataSet)objHelperDB.GetGenericDataDB(_CATALOG_ID, cdr["Product_ID"].ToString(), "GET_FAMILY_ATTRIBUTE_NEWPRODUCT_PACKAGE", HelperDB.ReturnType.RTDataSet);
                            //    string pcode = string.Empty;
                            //    if (cdr["attribute_name"].ToString().ToUpper() == "CODE")
                            //    {
                            //        pcode = cdr["string_value"].ToString();
                            //    }

                            //    if (package_name.Equals("NEWPRODUCT"))
                            //    {
                            //        objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" + cdr["Family_id"].ToString() + "=" + cdr["Family_name"].ToString() + "////" + cdr["Product_ID"].ToString() + "=" + pcode, "pd.aspx", true, "");

                            //    }

                            //    if (Dsfamilyname != null && Dsfamilyname.Tables[0].Rows.Count > 0)
                            //    {

                            //        if (!package_name.Equals("NEWPRODUCT"))
                            //         _stmpl_records.SetAttribute("TBT_FAMILY_ID", Dsfamilyname.Tables[0].Rows[0]["FAMILY_ID"].ToString());


                            //         if (package_name.Equals("NEWPRODUCT"))
                            //         {

                            //         }
                            //         else
                            //         {
                            //           objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + _stmpl_records.GetAttribute("TBT_FAMILY_ID") + "=" + _stmpl_records.GetAttribute("TBT_FAMILY_NAME") + "////" + cdr["Product_ID"].ToString() + "=" + pcode, "pd.aspx", true, "");

                            //         }
                            //         if (package_name.Equals("NEWPRODUCT"))
                            //         {
                            //             foreach (DataRow Drow in Dsfamilyname.Tables[0].Rows)
                            //             {
                            //                 string desc = "";
                            //                 string string_value=Drow["STRING_VALUE"].ToString();
                            //                 //if (string_value.Length > 100)
                            //                 //{
                            //                 //    desc = string_value.Substring(0, 100) + "...";
                            //                 if (string_value.Length > 75)
                            //                 {
                            //                     desc = string_value.Substring(0, 75) + "...";   
                            //             }
                            //                 else
                            //                 {
                            //                     desc = string_value;
                            //                 }
                            //                 _stmpl_records.SetAttribute("TBT_" + Drow["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), desc.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("\n", "<br/>").Replace("\r", "&nbsp;"));



                            //             }
                            //         }
                            //         else
                            //         {

                            //             foreach (DataRow Drow in Dsfamilyname.Tables[0].Rows)
                            //             {

                            //                 string desc = string.Empty;
                            //                 string string_value = Drow["STRING_VALUE"].ToString();
                            //                 //if (string_value.Length > 80)
                            //                 if (string_value.Length > 75)
                            //                 {
                            //                     desc = string_value.Substring(0, 75) + "...";
                            //                 }
                            //                 else
                            //                 {
                            //                     desc = string_value;
                            //                 }
                            //                 _stmpl_records.SetAttribute("TBT_" + Drow["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), desc.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("\n", "<br/>").Replace("\r", "&nbsp;"));



                            //             }
                            //         }

                            //    }
                            //}
                            // comment end
                        }

                        string descall = string.Empty;
                        string desc1 = string.Empty;
                        string descallstring = string.Empty;
                        string attName = string.Empty;

                        string strurlnew = string.Empty;
                        int setatttr = 0;
                        DataRow[] PRODRows = dsrecords.Tables[0].Select("PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString());

                        foreach (DataRow dr in PRODRows)
                        {

                            desc1 = "";
                            string attr_datatype = dr["ATTRIBUTE_DATATYPE"].ToString();
                            string attr_type = dr["ATTRIBUTE_TYPE"].ToString();
                            string attr_name = dr["ATTRIBUTE_NAME"].ToString();
                            string drstrval = string.Empty;
                            drstrval = dr["STRING_VALUE"].ToString();
                            string attr_name_replace = string.Empty;
                            attr_name_replace = attr_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();
                            // comment start
                            //try
                            //{
                            //    if (package_name.Equals("NEWPRODUCTLOGNAV") && setatttr == 0)
                            //    {

                            //        string Product_ID = dr["Product_ID"].ToString();
                            //        string Attrname = "TWEB IMAGE1";
                            //        DataRow[] dr1 = dsrecords.Tables[0].Select("PRODUCT_ID='" + Product_ID + "' AND ATTRIBUTE_NAME='" + Attrname + "'");
                            //        string sSQL = string.Empty;

                            //        if (dr1.Length <= 0)
                            //        {
                            //            string family_id = dr["Family_ID"].ToString();
                            //            sSQL = "Exec Get_FamilyImage " + family_id + "," + Product_ID;
                            //            HelperDB objHelperDB = new HelperDB();
                            //            DataSet dsfamily = objHelperDB.GetDataSetDB(sSQL);

                            //            setatttr = 1;
                            //            if (dsfamily.Tables[0] != null)
                            //            {
                            //                FileInfo Fil1;
                            //                Fil1 = new FileInfo(strFile + dsfamily.Tables[0].Rows[0]["string_value"].ToString().Replace("\\", "/"));
                            //                if (Fil1.Exists)
                            //                {
                            //                    _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", dsfamily.Tables[0].Rows[0]["string_value"].ToString().Replace("\\", "/"));
                            //                }
                            //                else
                            //                {
                            //                    _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", "/images/noimage.gif");
                            //                }
                            //            }
                            //            else
                            //            {
                            //                _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", "/images/noimage.gif");
                            //            }

                            //        }
                            //    }
                            //}
                            //catch
                            //{ }
                            //// comment end
                            if (attr_datatype.ToUpper().StartsWith("TEX"))
                            {
                                if (attr_type.Equals("3") || attr_type.Equals("9"))
                                {
                                    if (!(package_name.Equals("PRODUCT")))
                                    {
                                        //FileInfo Fil;
                                        //if (package_name.Equals("PRODUCT"))
                                        //{
                                        //    Fil = new FileInfo(strFile + drstrval.Replace("_TH", "_Images_200"));
                                        //}
                                        //else
                                        //{
                                        //    Fil = new FileInfo(strFile + drstrval);
                                        //}
                                        //if (Fil.Exists)
                                        //{
                                        //if (package_name.Equals("PRODUCT"))
                                        //{
                                        //    //_stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                        //}
                                        //else
                                        //{
                                        //    _stmpl_records.SetAttribute("TBT_" + attr_name_replace, drstrval.Replace("\\", "/"));
                                        //    //set cookie value
                                        //    // comment start
                                        //    //if (package_name.Equals("PRODUCT"))
                                        //    //{
                                        //    //    string img1 = drstrval.Replace("\\", "/");
                                        //    //   // SetCookie("img1", "", "", img1,"","","");                                       
                                        //    //}
                                        //    // comment end
                                        //}
                                        if (!(package_name.Equals("PRODUCT")))
                                        {
                                            _stmpl_records.SetAttribute("TBT_" + attr_name_replace, drstrval.Replace("\\", "/"));
                                        }
                                        //}
                                        //else
                                        //{
                                        //    _stmpl_records.SetAttribute("TBT_" + attr_name_replace, "");


                                        //}
                                    }
                                }

                                else
                                {
                                    attName = dr["ATTRIBUTE_NAME"].ToString().ToUpper();

                                    if (package_name.Equals("PRODUCT"))
                                    {
                                        if (attName.Equals("SHORT_DESCRIPTION") || attName.Equals("DESCRIPTIONS") || attName.Equals("FEATURES") || attName.Equals("SPECIFICATION") || attName.Equals("SPECIFICATIONS") || attName.Equals("APPLICATIONS") || attName.Equals("NOTES"))
                                        {
                                            StringBuilder sbdrstrval = new StringBuilder(drstrval);
                                            //desc1 = drstrval.Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                                            desc1 = sbdrstrval.Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;").ToString();
                                        }
                                        else if (attName.Equals("DESCRIPTION"))
                                        {
                                            if (drstrval.Length > 0)
                                            {
                                                _stmpl_records.SetAttribute("TBT_" + attr_name_replace, drstrval);
                                                _stmpl_records.SetAttribute("PROD_DESC_ALT", drstrval);
                                                // _stmpl_records.SetAttribute("PROD_DESC_TITLE", drstrval.Replace('"', ' '));
                                            }
                                            else
                                            {
                                                _stmpl_records.SetAttribute("PROD_DESC_ALT", drstrval);
                                                // _stmpl_records.SetAttribute("PROD_DESC_TITLE", drstrval.Replace('"', ' '));
                                            }

                                        }
                                        else
                                            _stmpl_records.SetAttribute("TBT_" + attr_name_replace, drstrval);
                                    }
                                    else
                                        _stmpl_records.SetAttribute("TBT_" + attr_name_replace, drstrval);
                                }

                            }

                            if (package_name.Equals("PRODUCT"))
                            {
                                if (!desc1.Equals(""))
                                    descall = descall + desc1 + "<br/><br/>";
                            }

                            else if (attr_datatype.ToUpper().StartsWith("NUM"))
                            {
                                if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                                    _stmpl_records.SetAttribute("TBT_" + attr_name_replace, objHelperService.FixDecPlace(Convert.ToDecimal(dr["NUMERIC_VALUE"])).ToString());
                                else
                                    _stmpl_records.SetAttribute("TBT_" + attr_name_replace, "");
                            }




                        } ////

                        if (package_name.Equals("PRODUCT"))
                        {

                            if (descall == string.Empty)
                                _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", true);
                            else
                                _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", false);


                            if (descall.Length > 400)
                                _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                            else
                                _stmpl_records.SetAttribute("TBT_MORE_SHOW", false);

                            if (descall.Length > 400)
                            {
                                //  _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                                descallstring = descall.Substring(0, 400).ToString();
                                _stmpl_records.SetAttribute("TBT_MORE", descallstring);
                                _stmpl_records.SetAttribute("TBT_MENU_ID", "2");
                                descall = descall.Substring(0, descall.Length).ToString();
                                // descall = descall.Substring(300).ToString();

                                _stmpl_records.SetAttribute("TBT_DESCALL", descall);

                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_DESCALL", descall);
                                _stmpl_records.SetAttribute("TBT_MENU_ID", "2");
                                // _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);
                                _stmpl_records.SetAttribute("TBT_MORE", descall);
                            }
                        }
                        //if (package_name.Equals("PRODUCT"))
                        //{
                        //    //bool descflag = false;
                        //   // int familyrows = 0;
                        //    DataSet dsfamily = new DataSet();

                        //}
                        //else
                        //{
                        //    _stmpl_records.SetAttribute("TBT_FAMILY_ID", paraFID);
                        //}
                        if (!(package_name.Equals("PRODUCT")))
                        {
                            _stmpl_records.SetAttribute("TBT_FAMILY_ID", paraFID);
                        }
                        if (package_name.Equals("PRODUCT") || package_name.Equals("COMPARE") || package_name.Equals("NEWPRODUCT") || package_name.Equals("PROMOTIONS") || package_name.Equals("NEWPRODUCTNAV") || package_name.Equals("MOREPRODUCTS") || package_name.Equals("NEWPRODUCTHIGHLIGHTSCATLIST"))
                        {

                            if (package_name.Equals("PRODUCT"))
                            {
                                // comment start
                                //if (cdr["QTY_AVAIL"] != null && cdr["MIN_ORD_QTY"] != null)
                                //{
                                //    if (Convert.ToInt32(cdr["QTY_AVAIL"].ToString()) > 0)
                                //    {

                                //    }
                                //    else
                                //    {
                                //        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                //    }
                                //}
                                //else
                                //{
                                //    _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                //}
                                // comment end

                                if (cdr["FAMILY_PROD_COUNT"] != null && cdr["PROD_COUNT"] != null)
                                {
                                    if (!cdr["FAMILY_PROD_COUNT"].ToString().Equals(cdr["PROD_COUNT"].ToString()))
                                    {
                                        DataSet _parentFamilyds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, cdr["FAMILY_ID"].ToString(), "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
                                        string _parentFamily_Id = "0";
                                        if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                                            _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

                                        if (_parentFamily_Id.Equals("0"))
                                            _parentFamily_Id = cdr["FAMILY_ID"].ToString();

                                        _stmpl_records.SetAttribute("TBT_PARENT_FAMILY_ID", _parentFamily_Id);

                                        _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "block");
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");
                                    }
                                    //objHelperService.Cons_NewURl(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" + cdr["FAMILY_name"].ToString(), "fl.aspx", true, "");
                                }
                                else
                                    _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");


                                if (package_name.Equals("PRODUCT"))
                                {

                                    //strurlnew = objHelperService.Cons_NewURl(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" + _stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID") + "=" + cdr["FAMILY_name"].ToString().Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""), "fl.aspx", true, "");
                                    //_stmpl_records.SetAttribute("TBT_REWRITEURL_NEW", strurlnew);

                                    if (_stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID") != null)
                                    {
                                        // strurlnew = strurlnew + "-" + _stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID").ToString() + "/" + prevpath;
                                        string[] catpath = null;
                                        string categorypath = string.Empty;
                                        try
                                        {
                                            catpath = _stmpl_records.GetAttribute("ORG_TBT_EA_PATH").ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                        }
                                        catch
                                        { }
                                        categorypath = (catpath.Length >= 3 ? catpath[2] : " ") + "////" + (catpath.Length >= 4 ? catpath[3] : " ");




                                        strurlnew = objHelperService.SimpleURL(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + _stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID").ToString() + "////" + categorypath + "////" + cdr["FAMILY_name"].ToString().Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""), "fl.aspx");
                                        _stmpl_records.SetAttribute("TBT_REWRITEURL_NEW", strurlnew);
                                    }
                                }

                            }

                            else
                            {
                                DataSet dsProduct = new DataSet();
                                //dsProduct = GetDataSet("SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + cdr["PRODUCT_ID"].ToString());
                                dsProduct = (DataSet)objHelperDB.GetGenericDataDB(cdr["PRODUCT_ID"].ToString(), "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTDataSet);
                                if (dsProduct != null && dsProduct.Tables[0] != null && dsProduct.Tables[0].Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(dsProduct.Tables[0].Rows[0]["QTY_AVAIL"]) > 0)
                                    {
                                        _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dsProduct.Tables[0].Rows[0]["QTY_AVAIL"].ToString());
                                        _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dsProduct.Tables[0].Rows[0]["MIN_ORD_QTY"].ToString());
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                    }
                                }
                                else
                                {
                                    if (package_name.Equals("PRODUCT"))
                                    {
                                        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                    }
                                }
                            }
                            string prodValues = string.Empty;
                            /*foreach (DataRow dr in dsrecords.Tables[dsrecords.Tables.Count - 1].Select("ATTRIBUTE_TYPE <> 3 AND ATTRIBUTE_ID NOT IN (1,5,450,449,481,482,483,484,485,486,487,488,489,490,491,492,493,494,495,496,497,498,499,500,501,502,503) AND PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString(), "ATTRIBUTE_NAME"))
                            {
                                
                                //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());                            
                                if( dr["attribute_name"].ToString().ToUpper()!="SUIT" &&  dr["attribute_name"].ToString().ToUpper()!="BRAND")
                                if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                                {
                                    if(dr["STRING_VALUE"]!=null && dr["STRING_VALUE"].ToString() !="" && dr["ATTRIBUTE_NAME"].ToString().ToUpper() != "NEW PRODUCTS")
                                        prodValues = prodValues + "<TR><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + dr["STRING_VALUE"] + "</TD></TR>"; 
                                }
                                else if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("NUM"))
                                {
                                    if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                                        if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
                                        {
                                            prodValues = prodValues + "<TR><TD bgcolor=\"white\" style=\"border-color:Black;\" align=\"center\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + " " + objHelperService.GetOptionValues("CURRENCYFORMAT").ToString() + " " + Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString() + "</TD></TR>";
                                        }
                                        else
                                        {
                                            prodValues = prodValues + "<TR><TD bgcolor=\"white\" style=\"border-color:Black;\" align=\"center\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString() + "</TD></TR>";
                                        }
                                }

                            }*/

                            /*

                            DataRow[] ATTRows = dsrecords.Tables[0].Select("ATTRIBUTE_TYPE <> 3 AND ATTRIBUTE_ID <> 1 and  PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString(), "ATTRIBUTE_NAME");
                            //MODIFIED BY INDU FOR TUNING 05AUG2014
                            //foreach (DataRow dr in dsrecords.Tables[0].Select("ATTRIBUTE_TYPE <> 3 AND ATTRIBUTE_ID <> 1 and  PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString(), "ATTRIBUTE_NAME"))
                            //{
                            foreach (DataRow dr in ATTRows)
                            {
                                if (!dr["attribute_name"].ToString().ToUpper().Equals("SUIT") && !dr["attribute_name"].ToString().ToUpper().Equals("BRAND"))
                                    if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                                    {
                                        if (dr["STRING_VALUE"] != null && !dr["STRING_VALUE"].ToString().Equals("") && !dr["ATTRIBUTE_NAME"].ToString().ToUpper().Equals("NEW PRODUCTS"))
                                           prodValues = prodValues + "<TR><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + dr["STRING_VALUE"] + "</TD></TR>";
                                           
                                    }
                            }
                            */


                            string _sPriceTable = string.Empty;
                            string _StockStatus = string.Empty;
                            string _Eta = string.Empty;
                            string _Prod_Stock_Status = "0";
                            string _Prod_Stock_Flag = "0";
                            bool isProductReplace = true;
                            string strReplacedProduct = "";
                            string CustomerType = "";

                            if (package_name.Equals("PRODUCT"))
                            {
                                if (HttpContext.Current.Session["PRODUCT_CATEGORY_NAME_SES"] != null)
                                    _stmpl_records.SetAttribute("PRODUCT_CATEGORY_NAME", HttpContext.Current.Session["PRODUCT_CATEGORY_NAME_SES"].ToString());

                                string userid = "0";

                                _StockStatus = cdr["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
                                _Prod_Stock_Status = cdr["PROD_STOCK_STATUS"].ToString();
                                _Prod_Stock_Flag = cdr["PROD_STOCK_FLAG"].ToString();

                                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                                    userid = HttpContext.Current.Session["USER_ID"].ToString();
                                CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));



                                _StockStatus = _StockStatus.Trim().Replace("_", " ");
                                //if ((_StockStatus.ToUpper().Contains("OUT OF STOCK ITEM WILL BE BACK ORDERED") || _StockStatus.ToUpper().Contains("SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED")))//&& CustomerType.ToLower() == "dealer"
                                //    isProductReplace = false;
                                //else
                                //{
                                //    if (_Prod_Stock_Status.ToLower() == "true" || _Prod_Stock_Status.ToLower() == "1")
                                //        isProductReplace = false;
                                //    else if (_Prod_Stock_Flag.ToLower() == "0")
                                //        isProductReplace = false;
                                //}
                                if (_Prod_Stock_Flag.ToLower() == "0")
                                    isProductReplace = false;

                                if (isProductReplace == true)
                                {
                                    strReplacedProduct = GetProductReplacementDetails(_stmpl_records, cdr["STRING_VALUE"].ToString(), Convert.ToInt32(userid), _StockStatus);

                                }
                                else
                                {

                                    _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));


                                    if (cdr["ETA"].ToString() != string.Empty)
                                    {
                                        _Eta = string.Format("<tr><td><b>ETA</b></td><td colspan=\"2\"><b>" + cdr["ETA"].ToString() + "</b></td></tr>");
                                    }
                                }




                            }
                            else
                            {
                                _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));
                                _StockStatus = GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"]));
                            }
                            string _Tbt_Stock_Status = string.Empty;
                            string _Tbt_Stock_Status_1 = string.Empty;
                            bool _Tbt_Stock_Status_2 = false;
                            string _Tbt_Stock_Status_3 = string.Empty;
                            string _Colorcode1 = string.Empty;
                            string _Colorcode;
                            string _StockStatusTrim = _StockStatus.Trim();




                            //switch (_StockStatusTrim)
                            //{
                            //    case "IN STOCK":
                            //        _Tbt_Stock_Status = "INSTOCK";
                            //        _Tbt_Stock_Status_2 = true;
                            //        _Colorcode = "#43A246";
                            //        break;
                            //    case "SPECIAL ORDER":
                            //        _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILBAILTY TO BE CONFIRMED";
                            //        _Tbt_Stock_Status_2 = true;
                            //        _Colorcode = "#43A246";
                            //        break;
                            //    case "DISCONTINUED":
                            //        _Tbt_Stock_Status = "DISCONTINUED";
                            //        _Tbt_Stock_Status_2 = false;
                            //        _Colorcode = "#ED1C24";
                            //        break;
                            //    case "TEMPORARY UNAVAILBLE":
                            //        _Tbt_Stock_Status = "TEMPORARY UNAVAILABLE NO ETA";
                            //        _Tbt_Stock_Status_2 = false;
                            //        _Colorcode = "#F9A023";
                            //        break;
                            //    case "OUT OF STOCK":
                            //        _Tbt_Stock_Status = "OUT OF STOCK";
                            //        _Tbt_Stock_Status_2 = false;
                            //        _Colorcode = "#F9A023";
                            //        _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                            //        _Colorcode1 = "#43A246";
                            //        break;
                            //    default:
                            //        _Tbt_Stock_Status = _StockStatus;
                            //        _Tbt_Stock_Status_2 = true;
                            //        _Colorcode = "Black";
                            //        break;
                            //}

                            switch (_StockStatusTrim)
                            {
                                case "IN STOCK":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status = "INSTOCK";
                                    _Tbt_Stock_Status_2 = true;
                                    break;
                                case "Limited Stock, Please Call":
                                    _Colorcode = "#f69e1b";
                                    _Tbt_Stock_Status = "<span style=\"color:#43A246;\"><b>Limited Stock - Please Call</b></span>";
                                    _Tbt_Stock_Status_2 = true;
                                    break;
                                case "SPECIAL ORDER":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                                    break;
                                case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                                    break;
                                case "SPECIAL ORDER PRICE &":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                                    break;
                                case "DISCONTINUED":
                                    _Colorcode = "#ED1C24";
                                    _Tbt_Stock_Status_2 = false;
                                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                                    break;
                                case "DISCONTINUED NO LONGER AVAILABLE":
                                    _Colorcode = "#ED1C24";
                                    _Tbt_Stock_Status_2 = false;
                                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                                    break;
                                case "DISCONTINUED NO LONGER":
                                    _Colorcode = "#ED1C24";
                                    _Tbt_Stock_Status_2 = false;
                                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                                    break;
                                case "TEMPORARY UNAVAILABLE":
                                    _Colorcode = "#F9A023";
                                    //modified by indu Requirement Stock Status update date 7-Apr-2017
                                    // _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status_2 = false;
                                    //_Tbt_Stock_Status = "<span style=\"color:" + _Colorcode + "\">TEMPORARY UNAVAILABLE NO ETA</span>";
                                    _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                                    break;
                                case "TEMPORARY UNAVAILABLE NO ETA":
                                    _Colorcode = "#F9A023";
                                    //modified by indu Requirement Stock Status update date 7-Apr-2017
                                    // _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status_2 = false;
                                    _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                                    break;
                                case "OUT OF STOCK":
                                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
                                    _Tbt_Stock_Status_2 = true;
                                    _Colorcode = "#F9A023";
                                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                                    _Colorcode1 = "#43A246";
                                    break;
                                case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
                                    _Tbt_Stock_Status_2 = true;
                                    _Colorcode = "#F9A023";
                                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                                    _Colorcode1 = "#43A246";
                                    break;
                                case "OUT OF STOCK ITEM WILL":
                                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
                                    _Tbt_Stock_Status_2 = true;
                                    _Colorcode = "#F9A023";
                                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                                    _Colorcode1 = "#43A246";
                                    break;
                                default:
                                    _Colorcode = "Black";
                                    _Tbt_Stock_Status = _StockStatusTrim;
                                    break;
                            }


                            string stkstatus = string.Empty;
                            stkstatus = _Tbt_Stock_Status.ToUpper();
                            string stkstatus1 = string.Empty;
                            stkstatus1 = _Tbt_Stock_Status_1.ToUpper();



                            if (isProductReplace == true)
                            {
                                _stmpl_records.SetAttribute("TBT_REPLACED", true);
                                _stmpl_records.SetAttribute("TBT_REPLACED_DETAIL", strReplacedProduct);
                            }
                            else
                            {

                                _stmpl_records.SetAttribute("TBT_REPLACED", false);
                                _stmpl_records.SetAttribute("TBT_COLOR_CODE", _Colorcode);
                                if (!_Tbt_Stock_Status.Equals(""))
                                {
                                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
                                }
                                _stmpl_records.SetAttribute("TBT_COLOR_CODE_1", _Colorcode1);
                                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
                                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);

                                if (stkstatus == "INSTOCK" || stkstatus1 == "INSTOCK")
                                {
                                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                                }
                                else if (stkstatus == "ITEM WILL BE BACK ORDERED" || stkstatus1 == "ITEM WILL BE BACK ORDERED")
                                {
                                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/OutOfStock");
                                }
                                else if (stkstatus.Contains("SPECIAL") || stkstatus1.Contains("SPECIAL"))
                                {
                                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/LimitedAvailability");
                                }
                                else if (!(_Tbt_Stock_Status_2))
                                {
                                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/Discontinued");
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                                }




                                if (stkstatus == "DISCONTINUED NO LONGER AVAILABLE")
                                    _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                                else
                                    _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                                //_stmpl_records.SetAttribute("TBT_STOCK_STATUS", GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"])));
                                _stmpl_records.SetAttribute("TBT_PRICE_TABLE", _sPriceTable);
                                _stmpl_records.SetAttribute("TBT_PDISPLAY_MODE", _sPriceTable == "" ? "none" : "");

                            }

                            /* if (!prodValues.Equals(""))
                             {
                                 if (package_name.Equals("PRODUCT"))
                                 {
                                     try
                                     {
                                         GetProductDetails(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records);
                                     }
                                     catch
                                     { }
                                         //GetProductDesc(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records);
                                 }
                                 _stmpl_records.SetAttribute("TBT_ALL_PRODUCTVALUES", prodValues);
                                 _stmpl_records.SetAttribute("TBT_DISPLAY_MODE", "");
                             }
                             else
                             {
                                 _stmpl_records.SetAttribute("TBT_DISPLAY_MODE", "none");
                             }*/


                            if (package_name.Equals("PRODUCT"))
                            {
                                try
                                {
                                    GetProductDetails(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records, HttpContext.Current);
                                }
                                catch
                                { }
                            }
                        }
                        //if (package_name.Equals("PRODUCT")) // for download tab
                        //{
                        //    ST_Product_Download(cdr["PRODUCT_ID"].ToString());

                        //    _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", downloadST);
                        //    _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
                        //    downloadST = "";


                        //    if (isdownload == true)
                        //    {
                        //        chkdwld = true;
                        //        string dwldmrge = ST_Downloads_Update(HttpContext.Current);
                        //        dwldmrge = downloadST + dwldmrge;
                        //        _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", dwldmrge);
                        //        _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
                        //        downloadST = "";
                        //    }
                        //    else
                        //    {
                        //        isdownload = true;
                        //        chkdwld = false;
                        //        _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", ST_Downloads_Update(HttpContext.Current));
                        //        _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
                        //    }


                        //    string cthref = "";

                        //    cthref = ST_VPC();

                        //    //if (HttpContext.Current.Session["AQ_PD_CAPTCH_IMAGE"] != null)
                        //    //{
                        //    //    _stmpl_records.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_PD_CAPTCH_IMAGE"].ToString());
                        //    //    _stmpl_records.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_PD_CAPTCH_VALUE"].ToString());
                        //    //}

                        //}

                        if (_Package.ToString().Contains("FAMILYPAGE"))
                        {
                            DataSet dsProduct = new DataSet();
                            //dsProduct = GetDataSet("SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + cdr["PRODUCT_ID"].ToString());
                            dsProduct = (DataSet)objHelperDB.GetGenericDataDB(cdr["PRODUCT_ID"].ToString(), "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTDataSet);
                            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dsProduct.Tables[0].Rows[0]["QTY_AVAIL"].ToString());
                            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dsProduct.Tables[0].Rows[0]["MIN_ORD_QTY"].ToString());
                        }
                        strValue = strValue + _stmpl_records.ToString();
                        if (ictcol.Equals(_grid_cols))
                        {
                            _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                            _stmpl_container.SetAttribute("TBWDataList", strValue);
                            if (ictrecords % 2 == 0)
                                _stmpl_container.SetAttribute("BGCOLOR", "TableEvenRow");
                            else
                                _stmpl_container.SetAttribute("BGCOLOR", "TableOddRow");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                            ictrecords++; ictcol = 1; strValue = "";
                        }
                        else
                        {
                            ictcol++;
                        }
                    }//


                    if (!strValue.Equals(""))
                    {
                        _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                        _stmpl_container.SetAttribute("TBWDataList", strValue);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        ictrecords++; ictcol = 1; strValue = "";
                    }
                }
            }

            _dict_inner_html[_skin_body_attribute] = lstrecords;
        }


        //public string ST_Product_Load()
        //{
        //    string sHTML = "";

        //    try
        //    {
        //        StringTemplateGroup _stg_container = null;
        //        StringTemplateGroup _stg_records = null;
        //        StringTemplateGroup _stg_records1 = null;
        //        StringTemplate _stmpl_container = null;
        //        StringTemplate _stmpl_records = null;
        //        StringTemplate _stmpl_records1 = null;

        //        TBWDataList[] lstrecords = new TBWDataList[0];

        //        DataSet dsrecords = new DataSet();

        //        DataRow[] drs = null;

        //        string brandname = "";

        //        if (HttpContext.Current.Request.QueryString["bname"] != null)
        //            brandname = HttpContext.Current.Request.QueryString["bname"].ToString();
        //        dsrecords = (DataSet)HttpContext.Current.Session["FamilyProduct"];

        //        if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
        //            return "";

        //        int ictrows = 0;

        //        int ictrecords = 0, ictcol = 1; string strValue = "";


        //        string strurlnew = string.Empty;
        //        string attname = "";
        //        _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
        //        _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);
        //        _stg_container = new StringTemplateGroup("main", _SkinRootPath);


        //        _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "cell");
        //        _stmpl_records1 = _stg_records1.GetInstanceOf(_Package + "\\" + "row");
        //        _stmpl_container = _stg_records1.GetInstanceOf(_Package + "\\" + "main");
        //        lstrecords = new TBWDataList[2];
        //        if (dsrecords.Tables[0].Rows.Count > 0)
        //        {
        //            //DataRow[] cellrow = dsrecords.Tables[0].Select("ATTRIBUTE_ID = 1");
        //          DataRow cdr = dsrecords.Tables[0].Rows[0];



        //            _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());
        //            // foreach (DataRow cdr in cellrow)
        //            // {
        //            try
        //            {                        
        //                GetMultipleImages(System.Convert.ToInt32(cdr["PRODUCT_ID"]), System.Convert.ToInt32(paraFID), System.Convert.ToInt32(cdr["Family_ID"]), _stmpl_records);
        //            }
        //            catch
        //            { }

        //            if (brandname != "")
        //            {
        //                _stmpl_records.SetAttribute("BRAND_NAME", brandname);
        //                _stmpl_records.SetAttribute("TBT_BRAND_NAME", true);
        //            }
        //            else
        //                _stmpl_records.SetAttribute("TBT_BRAND_NAME", false);

        //            if (HttpContext.Current.Request.QueryString["fid"] != null)
        //            {
        //                _fid = HttpContext.Current.Request.QueryString["fid"].ToString();

        //                DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
        //                if (tmpds != null && tmpds.Tables.Count > 0)
        //                {
        //                    _stmpl_records.SetAttribute("TBT_CATEGORY_ID", tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());
        //                    string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
        //                    _stmpl_records.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
        //                    _stmpl_records.SetAttribute("ORG_TBT_EA_PATH", eapath);
        //                }

        //            }
        //            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", cdr["QTY_AVAIL"].ToString());
        //            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", cdr["MIN_ORD_QTY"].ToString());
        //            //if (cdr["QTY_AVAIL"] != null && cdr["MIN_ORD_QTY"] != null)
        //            //{
        //            //    if (Convert.ToInt32(cdr["QTY_AVAIL"].ToString()) > 0)
        //            //    {
        //            //        _stmpl_records.SetAttribute("TBT_QTY_AVAIL", cdr["QTY_AVAIL"].ToString());
        //            //        _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", cdr["MIN_ORD_QTY"].ToString());
        //            //    }
        //            //    else
        //            //    {
        //            //        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
        //            //    }
        //            //}
        //            //else
        //            //{
        //            //    _stmpl_records.SetAttribute("TB_DISPLAY", "none");
        //            //}
        //            _stmpl_records.SetAttribute("TBT_PRODUCT_ID", cdr["PRODUCT_ID"].ToString());
        //            _stmpl_records.SetAttribute("TBT_FAMILY_NAME", cdr["FAMILY_NAME"].ToString());
        //            _stmpl_records.SetAttribute("TBT_FAMILY_ID", cdr["FAMILY_ID"].ToString());

        //            if (cdr["FAMILY_PROD_COUNT"] != null && cdr["PROD_COUNT"] != null)
        //            {
        //                if (!cdr["FAMILY_PROD_COUNT"].ToString().Equals(cdr["PROD_COUNT"].ToString()))
        //                {
        //                    DataSet _parentFamilyds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, cdr["FAMILY_ID"].ToString(), "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
        //                    string _parentFamily_Id = "0";
        //                    if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
        //                        _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

        //                    if (_parentFamily_Id.Equals("0"))
        //                        _parentFamily_Id = cdr["FAMILY_ID"].ToString();

        //                    _stmpl_records.SetAttribute("TBT_PARENT_FAMILY_ID", _parentFamily_Id);

        //                    _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "block");
        //                }
        //                else
        //                {
        //                    _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");
        //                }
        //                //objHelperService.Cons_NewURl(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" + cdr["FAMILY_name"].ToString(), "fl.aspx", true, "");
        //            }
        //            else
        //                _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");




        //                //strurlnew = objHelperService.Cons_NewURl(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" + _stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID") + "=" + cdr["FAMILY_name"].ToString().Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""), "fl.aspx", true, "");
        //                //_stmpl_records.SetAttribute("TBT_REWRITEURL_NEW", strurlnew);

        //                if (_stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID") != null)
        //                {
        //                    // strurlnew = strurlnew + "-" + _stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID").ToString() + "/" + prevpath;
        //                    string[] catpath = null;
        //                    string categorypath = string.Empty;
        //                    try
        //                    {
        //                        catpath = _stmpl_records.GetAttribute("ORG_TBT_EA_PATH").ToString().Split(new string[] { "////" }, StringSplitOptions.None);
        //                    }
        //                    catch
        //                    { }
        //                    categorypath = (catpath.Length >= 3 ? catpath[2] : " ") + "////" + (catpath.Length >= 4 ? catpath[3] : " ");




        //                    strurlnew = objHelperService.SimpleURL(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + _stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID").ToString() + "////" + categorypath + "////" + cdr["FAMILY_name"].ToString().Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""), "fl.aspx");
        //                    _stmpl_records.SetAttribute("TBT_REWRITEURL_NEW", strurlnew);
        //                }

        //            string prodValues = "";
        //            string descall = string.Empty;
        //            string desc1 = string.Empty;
        //            string descallstring = string.Empty;
        //            string attName = string.Empty;
        //            string drstrval = string.Empty;
        //            string attr_name_replace = string.Empty;
        //            foreach (DataRow dr in dsrecords.Tables[0].Rows)
        //            {

        //                if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
        //                {
        //                    attName = dr["ATTRIBUTE_NAME"].ToString().ToUpper();
        //                    drstrval = dr["STRING_VALUE"].ToString();

        //                    attr_name_replace = attName;

        //                    if (attName.Equals("SHORT_DESCRIPTION") || attName.Equals("DESCRIPTIONS") || attName.Equals("FEATURES") || attName.Equals("SPECIFICATION") || attName.Equals("SPECIFICATIONS") || attName.Equals("APPLICATIONS") || attName.Equals("NOTES"))
        //                    {

        //                        desc1 = drstrval.Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;");
        //                    }
        //                    else if (attName.Equals("DESCRIPTION"))
        //                    {
        //                        if (drstrval.Length > 0)
        //                        {
        //                            _stmpl_records.SetAttribute("TBT_" + attr_name_replace, drstrval);
        //                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drstrval);
        //                            _stmpl_records.SetAttribute("PROD_DESC_TITLE", drstrval.Replace('"', ' '));
        //                        }
        //                        else
        //                        {
        //                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drstrval);
        //                            _stmpl_records.SetAttribute("PROD_DESC_TITLE", drstrval.Replace('"', ' '));
        //                        }

        //                    }
        //                    else
        //                        _stmpl_records.SetAttribute("TBT_" + attr_name_replace, drstrval);

        //                    if (!desc1.Equals(""))
        //                        descall = descall + desc1 + "<br/><br/>";
        //                }


        //            }

        //            if (descall == string.Empty)
        //                _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", true);
        //            else
        //                _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", false);


        //            if (descall.Length > 400)
        //                _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
        //            else
        //                _stmpl_records.SetAttribute("TBT_MORE_SHOW", false);

        //            if (descall.Length > 400)
        //            {

        //                descallstring = descall.Substring(0, 400).ToString();
        //                _stmpl_records.SetAttribute("TBT_MORE", descallstring);
        //                _stmpl_records.SetAttribute("TBT_MENU_ID", "2");
        //                descall = descall.Substring(0, descall.Length).ToString();


        //                _stmpl_records.SetAttribute("TBT_DESCALL", descall);

        //            }
        //            else
        //            {
        //                _stmpl_records.SetAttribute("TBT_DESCALL", descall);
        //                _stmpl_records.SetAttribute("TBT_MENU_ID", "2");

        //                _stmpl_records.SetAttribute("TBT_MORE", descall);
        //            }

        //            //DataRow[] DataR = dsrecords.Tables[0].Select("ATTRIBUTE_TYPE in (1,7) and  PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString());

        //            //    foreach (DataRow dr in DataR)
        //            //    {
        //            //        if (dr["ATTRIBUTE_TYPE"].ToString() == "1" || dr["ATTRIBUTE_TYPE"].ToString() == "7")
        //            //        {
        //            //            _stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString());
        //            //        }
        //            //    }

        //            string _sPriceTable = "";
        //            string _StockStatus = "";
        //            string _Prod_Stock_Status = "0";
        //            string _Prod_Stock_Flag = "0";
        //            string _Eta = "";
        //            bool isProductReplace = true;
        //            string strReplacedProduct = "";
        //            string CustomerType = "";

        //            string userid = "0";

        //            if (HttpContext.Current.Session["PRODUCT_CATEGORY_NAME_SES"] != null)
        //                _stmpl_records.SetAttribute("PRODUCT_CATEGORY_NAME", HttpContext.Current.Session["PRODUCT_CATEGORY_NAME_SES"].ToString());



        //            _StockStatus = cdr["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
        //            _Prod_Stock_Status = cdr["PROD_STOCK_STATUS"].ToString();
        //            _Prod_Stock_Flag = cdr["PROD_STOCK_FLAG"].ToString();
        //            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
        //                userid = HttpContext.Current.Session["USER_ID"].ToString();
        //            CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));

        //            if (_Prod_Stock_Flag.ToLower() == "0")
        //                isProductReplace = false;

        //            if (isProductReplace == true)
        //            {
        //                strReplacedProduct = GetProductReplacementDetails(_stmpl_records, cdr["STRING_VALUE"].ToString(), Convert.ToInt32(userid), _StockStatus);

        //            }
        //            else
        //            {
        //                _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));

        //                if (cdr["ETA"].ToString() != string.Empty)
        //                {
        //                    _Eta = string.Format("<tr><td><b>ETA</b></td><td colspan=\"2\"><b>" + cdr["ETA"].ToString() + "</b></td></tr>");
        //                }
        //            }






        //            string _Tbt_Stock_Status = string.Empty;
        //            string _Tbt_Stock_Status_1 = string.Empty;
        //            bool _Tbt_Stock_Status_2 = false;
        //            string _Tbt_Stock_Status_3 = string.Empty;
        //            string _Colorcode1 = string.Empty;
        //            string _Colorcode;
        //            string _StockStatusTrim = _StockStatus.Trim();



        //            switch (_StockStatusTrim)
        //            {
        //                case "IN STOCK":
        //                    _Colorcode = "#43A246";
        //                    _Tbt_Stock_Status = "INSTOCK";
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "SPECIAL ORDER":
        //                    _Colorcode = "#43A246";
        //                    _Tbt_Stock_Status_2 = true;
        //                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
        //                    break;
        //                case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
        //                    _Colorcode = "#43A246";
        //                    _Tbt_Stock_Status_2 = true;
        //                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
        //                    break;
        //                case "SPECIAL ORDER PRICE &":
        //                    _Colorcode = "#43A246";
        //                    _Tbt_Stock_Status_2 = true;
        //                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
        //                    break;
        //                case "DISCONTINUED":
        //                    _Colorcode = "#ED1C24";
        //                    _Tbt_Stock_Status_2 = false;
        //                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
        //                    break;
        //                case "DISCONTINUED NO LONGER AVAILABLE":
        //                    _Colorcode = "#ED1C24";
        //                    _Tbt_Stock_Status_2 = false;
        //                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
        //                    break;
        //                case "DISCONTINUED NO LONGER":
        //                    _Colorcode = "#ED1C24";
        //                    _Tbt_Stock_Status_2 = false;
        //                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
        //                    break;
        //                case "TEMPORARY UNAVAILABLE":
        //                    _Colorcode = "#F9A023";
        //                    _Tbt_Stock_Status_2 = true;
        //                    //_Tbt_Stock_Status = "<span style=\"color:" + _Colorcode + "\">TEMPORARY UNAVAILABLE NO ETA</span>";
        //                    _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
        //                    break;
        //                case "TEMPORARY UNAVAILABLE NO ETA":
        //                    _Colorcode = "#F9A023";
        //                    _Tbt_Stock_Status_2 = true;
        //                    _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
        //                    break;
        //                case "OUT OF STOCK":
        //                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
        //                    _Tbt_Stock_Status_2 = true;
        //                    _Colorcode = "#F9A023";
        //                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
        //                    _Colorcode1 = "#43A246";
        //                    break;
        //                case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
        //                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
        //                    _Tbt_Stock_Status_2 = true;
        //                    _Colorcode = "#F9A023";
        //                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
        //                    _Colorcode1 = "#43A246";
        //                    break;
        //                case "OUT OF STOCK ITEM WILL":
        //                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
        //                    _Tbt_Stock_Status_2 = true;
        //                    _Colorcode = "#F9A023";
        //                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
        //                    _Colorcode1 = "#43A246";
        //                    break;
        //                default:
        //                    _Colorcode = "Black";
        //                    _Tbt_Stock_Status = _StockStatusTrim;
        //                    break;
        //            }


        //            string stkstatus = string.Empty;
        //            stkstatus = _Tbt_Stock_Status.ToUpper();
        //            string stkstatus1 = string.Empty;
        //            stkstatus1 = _Tbt_Stock_Status_1.ToUpper();



        //            if (isProductReplace == true)
        //            {
        //                _stmpl_records.SetAttribute("TBT_REPLACED", true);
        //                // _stmpl_records.SetAttribute("TBT_REPLACED_DETAIL", strReplacedProduct);
        //            }
        //            else
        //            {

        //                _stmpl_records.SetAttribute("TBT_REPLACED", false);
        //                _stmpl_records.SetAttribute("TBT_COLOR_CODE", _Colorcode);
        //                if (!_Tbt_Stock_Status.Equals(""))
        //                {
        //                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
        //                }
        //                else
        //                {
        //                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
        //                }
        //                _stmpl_records.SetAttribute("TBT_COLOR_CODE_1", _Colorcode1);
        //                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
        //                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
        //                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);

        //                if (stkstatus == "INSTOCK" || stkstatus1 == "INSTOCK")
        //                {
        //                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
        //                }
        //                else if (stkstatus == "ITEM WILL BE BACK ORDERED" || stkstatus1 == "ITEM WILL BE BACK ORDERED")
        //                {
        //                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/OutOfStock");
        //                }
        //                else if (stkstatus.Contains("SPECIAL") || stkstatus1.Contains("SPECIAL"))
        //                {
        //                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/LimitedAvailability");
        //                }
        //                else if (!(_Tbt_Stock_Status_2))
        //                {
        //                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/Discontinued");
        //                }
        //                else
        //                {
        //                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
        //                }

        //                //_stmpl_records.SetAttribute("TBT_STOCK_STATUS", GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"])));
        //                _stmpl_records.SetAttribute("TBT_PRICE_TABLE", _sPriceTable);
        //                _stmpl_records.SetAttribute("TBT_PDISPLAY_MODE", _sPriceTable == "" ? "none" : "");

        //            }




        //            //if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
        //            //    _stmpl_records.SetAttribute("TBT_LOGIN_PRINT", true);
        //            //else
        //            //    _stmpl_records.SetAttribute("TBT_LOGIN_PRINT", false);
        //            //_stmpl_records.SetAttribute("TBT_PDISPLAY_MODE", _sPriceTable == "" ? "none" : "");

        //            try
        //            {
        //                GetProductDetails(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records);
        //            }
        //            catch
        //            { }

        //            //if (prodValues != "")
        //            //{
        //            //    if (_Package == "PRODUCT")
        //            //    {
        //            //        GetProductDetails(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records);

        //            //    }
        //            //    _stmpl_records.SetAttribute("TBT_ALL_PRODUCTVALUES", prodValues);
        //            //    _stmpl_records.SetAttribute("TBT_DISPLAY_MODE", "");
        //            //}
        //            //else
        //            //{
        //            //    _stmpl_records.SetAttribute("TBT_DISPLAY_MODE", "none");
        //            //}


        //            ST_Product_Download(cdr["PRODUCT_ID"].ToString());

        //            _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", downloadST);
        //            _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
        //            downloadST = "";


        //            if (isdownload == true)
        //            {
        //                chkdwld = true;
        //                string dwldmrge = ST_Downloads_Update();
        //                dwldmrge = downloadST + dwldmrge;
        //                _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", dwldmrge);
        //                _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
        //                downloadST = "";
        //            }
        //            else
        //            {
        //                isdownload = true;
        //                chkdwld = false;
        //                _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", ST_Downloads_Update());
        //                _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
        //            }


        //            string cthref = "";

        //            cthref = ST_VPC();
        //            _stmpl_records.SetAttribute("TBT_CT_HREF", cthref);


        //        }



        //        _stmpl_records1.SetAttribute("TBWDataList", _stmpl_records.ToString());

        //        lstrecords[ictrecords] = new TBWDataList(_stmpl_records1.ToString());
        //        //}

        //        _stmpl_container.SetAttribute("TBWDataList", lstrecords);


        //        sHTML = _stmpl_container.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        sHTML = "";
        //    }
        //    return ProdimageRreplaceImages(sHTML);
        //}

        public string ST_Product_Load()
        {
            Stopwatch stpw = new Stopwatch();
            stpw.Start();
            /// StackTrace st = new StackTrace();
            // StackFrame sf = st.GetFrame(0);          
            string sHTML = "";

            try
            {

                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplateGroup _stg_records1 = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_records1 = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                DataSet dsrecords = new DataSet();

                //  DataRow[] drs = null;



                if (HttpContext.Current.Request.QueryString["bname"] != null)
                    brandname = HttpContext.Current.Request.QueryString["bname"];
                dsrecords = (DataSet)HttpContext.Current.Session["FamilyProduct"];

                if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
                    return "";

                //  int ictrows = 0;

                int ictrecords = 0;
                // ictcol = 1; 
                // string strValue = "";


                string strurlnew = string.Empty;
                //  string attname = "";
                _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "cell");
                _stmpl_records1 = _stg_records1.GetInstanceOf(_Package + "\\" + "row");
                _stmpl_container = _stg_records1.GetInstanceOf(_Package + "\\" + "main");
                lstrecords = new TBWDataList[2];


              


                if (dsrecords.Tables[0].Rows.Count > 0)
                {
                    //DataRow[] cellrow = dsrecords.Tables[0].Select("ATTRIBUTE_ID = 1");
                    DataRow cdr = dsrecords.Tables[0].Rows[0];

                    //objErrorHandler.CreateLog("inside st_product_load"+ cdr["PRODUCT_ID"].ToString());

                    //  _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());
                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", true);
                    _stmpl_records.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"]);
                    _stmpl_records.SetAttribute("CDNROOT", System.Configuration.ConfigurationManager.AppSettings["CDNROOT"]);
                    // foreach (DataRow cdr in cellrow)
                    // {
                    // Stopwatch sw1 = new Stopwatch();
                    // sw1.Start();
                    try
                    {

                        GetMultipleImages(System.Convert.ToInt32(cdr["PRODUCT_ID"]), System.Convert.ToInt32(paraFID), System.Convert.ToInt32(cdr["Family_ID"]), _stmpl_records);

                    }
                    catch (Exception ex)
                    {
                        //objErrorHandler.CreateLog("product_id" + cdr["PRODUCT_ID"] + "--" + "paraFID" + paraFID + "--" + "Family_ID" + cdr["Family_ID"]+"--"+ex.ToString());
                    }
                    //Thread t1 = new Thread(delegate() { ST_product_Load_1(_stmpl_records, System.Convert.ToInt32(cdr["PRODUCT_ID"]), System.Convert.ToInt32(cdr["Family_ID"])); });
                    //t1.Start();
                    //  sw1.Stop();
                    //  objErrorHandler.ExeTimelog = "GetMultipleImages = " + sw1.Elapsed.TotalSeconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    // objErrorHandler.ExeTimelog = "GetMultipleImages =mins- " + sw1.Elapsed.Minutes.ToString() + ",seconds-" + sw1.Elapsed.Seconds.ToString() + ",ms-" + sw1.Elapsed.Milliseconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    //  objErrorHandler.createexecutiontmielog();



                    if (HttpContext.Current.Request.QueryString["fid"] != null)
                    {
                        _fid = HttpContext.Current.Request.QueryString["fid"];

                        DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_FAMILY_CATEGORY_BIGTOP", HelperDB.ReturnType.RTDataSet);
                        if (tmpds != null && tmpds.Tables.Count > 0)
                        {
                            // objErrorHandler.CreateLog("inside product page" + "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID" + tmpds.Tables[0].Rows.Count + "---" + HttpContext.Current.Session["EA"].ToString());

                            string checkcathpath = string.Empty;
                            //Modified by:Indu .To get same category path
                            try
                            {
                                string[] catpath = HttpContext.Current.Session["EA"].ToString().Replace("AllProducts////WESAUSTRALASIA////BigTop Store////", "").Split(new string[] { "////" }, StringSplitOptions.None);

                                //objErrorHandler.CreateLog(catpath[0] + "  " + category_nameh);
                                //if (catpath.Length >= 3)
                                //{
                                //    categorypath = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ");
                                //}
                                //else
                                //{
                                checkcathpath = (catpath.Length >= 1 ? catpath[0] : " ") + (catpath.Length >= 2 ? "////" + catpath[1] : "");

                            }
                            catch (Exception ex)
                            {


                            }

                            //objErrorHandler.CreateLog("first "+ HttpContext.Current.Session["EA"].ToString());
                            //objErrorHandler.CreateLog(checkcathpath);
                            //objErrorHandler.CreateLog("sec " + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString());

                            /////
                            _stmpl_records.SetAttribute("TBT_CATEGORY_ID", tmpds.Tables[0].Rows[0]["SubCatID"]);
                            string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Replace("Supplier Product Feed////", "");
                            string catpath1 = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Replace("Supplier Product Feed////", "");
                            //objErrorHandler.CreateLog("catpath1 "+catpath1);
                            //if (checkcathpath != catpath1)
                            //{
                            //    //objErrorHandler.CreateLog(checkcathpath + "checkcathpath");
                            //    eapath = "AllProducts////WESAUSTRALASIA////" + checkcathpath;
                            //}
                            _stmpl_records.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                            _stmpl_records.SetAttribute("ORG_TBT_EA_PATH", eapath);
                            string currenturl = System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString();
                            _stmpl_records.SetAttribute("PDURL", currenturl + HttpContext.Current.Request.RawUrl.ToString());
                        }

                    }
                    //        Thread t2=null;
                    //         if (HttpContext.Current.Request.QueryString["fid"] != null)
                    //{
                    //    _fid = HttpContext.Current.Request.QueryString["fid"].ToString();

                    //         t2 = new Thread(delegate() { ST_product_Load_2(_stmpl_records,_fid); });
                    //        t2.Start();
                    //         }
                    try
                    {
                        _stmpl_records.SetAttribute("TBT_QTY_AVAIL", cdr["QTY_AVAIL"]);
                    }
                    catch
                    { }
                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", cdr["MIN_ORD_QTY"]);

                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", cdr["PRODUCT_ID"]);






                    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", cdr["FAMILY_NAME"]);

                    HttpContext.Current.Session["FNAME_PRODUCT"] = cdr["FAMILY_NAME"];

                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", cdr["FAMILY_ID"]);
                    _stmpl_records.SetAttribute("TBT_COST", cdr["COST"]);






                    _stmpl_records.SetAttribute("TBT_META_COST", cdr["COST"].ToString().Replace("$", ""));
                    if (cdr["FAMILY_PROD_COUNT"] != null && cdr["PROD_COUNT"] != null)
                    {
                        if (!cdr["FAMILY_PROD_COUNT"].Equals(cdr["PROD_COUNT"]))
                        {
                            DataSet _parentFamilyds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, cdr["FAMILY_ID"].ToString(), "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
                            string _parentFamily_Id = "0";
                            if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                                _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

                            if (_parentFamily_Id.Equals("0"))
                                _parentFamily_Id = cdr["FAMILY_ID"].ToString();

                            _stmpl_records.SetAttribute("TBT_PARENT_FAMILY_ID", _parentFamily_Id);

                            _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "block");
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");
                        }
                        //objHelperService.Cons_NewURl(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" + cdr["FAMILY_name"].ToString(), "fl.aspx", true, "");
                    }
                    else
                        _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");


                    //Thread t3 = new Thread(delegate() { ST_product_Load_3(_stmpl_records,cdr); });
                    //t3.Start();



                    //while (t1.IsAlive ||(t2!=null && t2.IsAlive) || t3.IsAlive)
                    //    Thread.Sleep(1); 

                    //strurlnew = objHelperService.Cons_NewURl(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" + _stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID") + "=" + cdr["FAMILY_name"].ToString().Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""), "fl.aspx", true, "");
                    //_stmpl_records.SetAttribute("TBT_REWRITEURL_NEW", strurlnew);
                    // Stopwatch sw2 = new Stopwatch();
                    //  sw2.Start();

                    if (_stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID") != null)
                    {
                        // strurlnew = strurlnew + "-" + _stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID").ToString() + "/" + prevpath;
                        string[] catpath = null;
                        string categorypath = string.Empty;
                        try
                        {
                            catpath = _stmpl_records.GetAttribute("ORG_TBT_EA_PATH").ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                        }
                        catch(Exception ex)
                        {
                            objErrorHandler.ErrorMsg = ex;
                            objErrorHandler.CreateLog();
                        }
                        //objErrorHandler.CreateLog("ORG_TBT_EA_PATH "+_stmpl_records.GetAttribute("ORG_TBT_EA_PATH").ToString());
                        //categorypath = (catpath.Length >= 3 ? catpath[2] : " ") + "////" + (catpath.Length >= 4 ? catpath[3] : " ");
                        if(catpath!=null)
                        categorypath =  (catpath.Length >= 4 ? catpath[3] : "")+ (catpath.Length >= 5 ? "////"+ catpath[4] : "");



                        //objErrorHandler.CreateLog("categorypath "+ categorypath);
                        strurlnew = objHelperService.SimpleURL(_stmpl_records, "AllProducts////WESAUSTRALASIA////BigTop Store////" + _stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID") + "////" + categorypath + "////" + cdr["FAMILY_name"].ToString().Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""), "fl.aspx");
                        _stmpl_records.SetAttribute("TBT_REWRITEURL_NEW", strurlnew);
                    }
                    //   sw2.Stop();
                    //  objErrorHandler.ExeTimelog = "view related products = " + sw2.Elapsed.TotalSeconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    // objErrorHandler.ExeTimelog = "view related products =mins- " + sw2.Elapsed.Minutes.ToString() + ",seconds-" + sw2.Elapsed.Seconds.ToString() + ",ms-" + sw2.Elapsed.Milliseconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    //  objErrorHandler.createexecutiontmielog();

                    //string prodValues = "";
                    string descall = string.Empty;
                    string desc1 = string.Empty;
                    string descallstring = string.Empty;
                    string attName = string.Empty;
                    string drstrval = string.Empty;
                    string attr_name_replace = string.Empty;
                    // Stopwatch sw3 = new Stopwatch();
                    // sw3.Start();

                    DataRow[] dtrow = dsrecords.Tables[0].Select("ATTRIBUTE_DATATYPE like 'TEX%' And ATTRIBUTE_TYPE<>3 And ATTRIBUTE_TYPE<>9");
                    if (dtrow.Length > 0)
                    {
                        foreach (DataRow dr in dtrow)
                        {
                            desc1 = "";

                            if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                            {
                                attName = dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();
                                drstrval = dr["STRING_VALUE"].ToString();

                                attr_name_replace = attName;
                                if (attName.Equals("CODE"))
                                {
                                    HttpContext.Current.Session["PRODUCT_CODE"] = drstrval;
                                }

                                if (attName.Equals("DESCRIPTIONS") || attName.Equals("FEATURES") || attName.Equals("SPECIFICATION") || attName.Equals("SPECIFICATIONS") || attName.Equals("APPLICATIONS") || attName.Equals("NOTES"))
                                {

                                    desc1 = drstrval.Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                                }
                                else if (attName.Equals("DESCRIPTION"))
                                {
                                    if (drstrval.Length > 0)
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + attr_name_replace, HttpUtility.HtmlEncode(drstrval));
                                        //   _stmpl_records.SetAttribute("PROD_DESC_ALT", drstrval);
                                        //  _stmpl_records.SetAttribute("PROD_DESC_TITLE", drstrval.Replace('"', ' '));
                                    }
                                    //else
                                    //{
                                    //    _stmpl_records.SetAttribute("PROD_DESC_ALT", drstrval);
                                    //    _stmpl_records.SetAttribute("PROD_DESC_TITLE", drstrval.Replace('"', ' '));
                                    //}

                                }
                                else
                                    _stmpl_records.SetAttribute("TBT_" + attr_name_replace, HttpUtility.HtmlEncode(drstrval));

                                if (!desc1.Equals(""))
                                    descall = descall + desc1 + "<br/><br/>";
                            }


                        }

                    }


                    if (descall == string.Empty)
                        _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", true);
                    else
                        _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", false);


                    if (descall.Length > 800)
                        _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                    else
                        _stmpl_records.SetAttribute("TBT_MORE_SHOW", false);

                    if (descall.Length > 800)
                    {

                        descallstring = descall.Substring(0, 800);
                        _stmpl_records.SetAttribute("TBT_MORE", descallstring);
                        _stmpl_records.SetAttribute("TBT_MENU_ID", "2");
                        descall = descall.Substring(0, descall.Length);


                        _stmpl_records.SetAttribute("TBT_DESCALL", descall);

                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_DESCALL", descall);
                        _stmpl_records.SetAttribute("TBT_MENU_ID", "2");

                        _stmpl_records.SetAttribute("TBT_MORE", descall);
                    }
                    //Thread t4 = new Thread(delegate() { ST_product_Load_4(_stmpl_records, cdr, dsrecords); });
                    //t4.Start();
                    // sw3.Stop();
                    //  objErrorHandler.ExeTimelog = "Attributes = " + sw3.Elapsed.TotalSeconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    // objErrorHandler.ExeTimelog = "Attributes =mins- " + sw3.Elapsed.Minutes.ToString() + ",seconds-" + sw3.Elapsed.Seconds.ToString() + ",ms-" + sw3.Elapsed.Milliseconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    //   objErrorHandler.createexecutiontmielog();

                    // Stopwatch sw4 = new Stopwatch();
                    //  sw4.Start();
                    string _sPriceTable = "";
                    string _StockStatus = "";
                    string _Prod_Stock_Status = "0";
                    string _Prod_Stock_Flag = "0";
                    string _Eta = "";
                    bool isProductReplace = true;
                    string strReplacedProduct = "";
                    string CustomerType = "";
                    string Issubstitute = "";
                    string userid = "0";
                    //    _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));
                    if (HttpContext.Current.Session["PRODUCT_CATEGORY_NAME_SES"] != null)
                        _stmpl_records.SetAttribute("PRODUCT_CATEGORY_NAME", HttpContext.Current.Session["PRODUCT_CATEGORY_NAME_SES"].ToString());



                    _StockStatus = cdr["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
                    _Prod_Stock_Status = cdr["PROD_STOCK_STATUS"].ToString();
                    _Prod_Stock_Flag = cdr["PROD_STOCK_FLAG"].ToString();
                    Issubstitute = cdr["PROD_SUBSTITUTE"].ToString().Trim();

                    if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                        userid = HttpContext.Current.Session["USER_ID"].ToString();
                    CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                    //objErrorHandler.CreateLog("ETA" + cdr["ETA"].ToString());
                    if (_Prod_Stock_Flag.ToLower() == "0")

                        isProductReplace = false;
                    //    objErrorHandler.CreateLog("isProductReplace" + isProductReplace + "-----" + "Issubstitute" + Issubstitute);

                    if ((isProductReplace == true) && (Issubstitute != ""))
                    {
                        strReplacedProduct = GetProductReplacementDetails(_stmpl_records, cdr["STRING_VALUE"].ToString(), Convert.ToInt32(userid), _StockStatus);

                    }
                    else
                    { 
                        //  _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));
                       
                        if (cdr["ETA"].ToString() != string.Empty)
                        {
                            _Eta = string.Format("<span>ETA    :</span></td><td colspan=\"2\"><b>" + cdr["ETA"].ToString() + "</b>");
                            objErrorHandler.CreateLog(_Eta);
                        }
                    }
                    // sw4.Stop();
                    //  objErrorHandler.ExeTimelog = "Sub product logic = " + sw4.Elapsed.TotalSeconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    // objErrorHandler.ExeTimelog = "STOCK product logic =mins- " + sw4.Elapsed.Minutes.ToString() + ",seconds-" + sw4.Elapsed.Seconds.ToString() + ",ms-" + sw4.Elapsed.Milliseconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    //   objErrorHandler.createexecutiontmielog();

                    //   Stopwatch sw5 = new Stopwatch();
                    //   sw5.Start();

                    string _Tbt_Stock_Status = string.Empty;
                    string _Tbt_Stock_Status_1 = string.Empty;
                    bool _Tbt_Stock_Status_2 = false;
                    string _Tbt_Stock_Status_3 = string.Empty;
                    string _Colorcode1 = string.Empty;
                    string _Colorcode;
                    string _StockStatusTrim = _StockStatus.Trim();



                    // objErrorHandler.CreateLog(_StockStatusTrim+"pd");
                    if ((_StockStatusTrim == "DISCONTINUED NO LONGER AVAILABLE") || (_StockStatusTrim == "TEMPORARY UNAVAILABLE NO ETA"))
                    {
                        // objErrorHandler.CreateLog(_StockStatusTrim + "inside TBT_HIDE_BUY" + Convert.ToInt32(cdr["PRODUCT_ID"]));
                        _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                    }

                    switch (_StockStatusTrim)
                    {
                        case "IN STOCK":
                            _Colorcode = "#43A246";
                            _Tbt_Stock_Status = "INSTOCK";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "Limited Stock, Please Call":
                            _Colorcode = "#f69e1b";
                            _Tbt_Stock_Status = "Limited Stock";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "SPECIAL ORDER":
                            _Colorcode = "#43A246";
                            _Tbt_Stock_Status_2 = true;
                            _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                            break;
                        case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                            _Colorcode = "#43A246";
                            _Tbt_Stock_Status_2 = true;
                            _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                            break;
                        case "SPECIAL ORDER PRICE &":
                            _Colorcode = "#43A246";
                            _Tbt_Stock_Status_2 = true;
                            _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                            break;
                        case "DISCONTINUED":
                            _Colorcode = "#ED1C24";
                            _Tbt_Stock_Status_2 = false;
                            _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                            break;
                        case "DISCONTINUED NO LONGER AVAILABLE":
                            _Colorcode = "#ED1C24";
                            _Tbt_Stock_Status_2 = false;
                            _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                            break;
                        case "DISCONTINUED NO LONGER":
                            _Colorcode = "#ED1C24";
                            _Tbt_Stock_Status_2 = false;
                            _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                            break;
                        case "TEMPORARY UNAVAILABLE":
                            _Colorcode = "#F9A023";
                            //modified by indu Requirement Stock Status update date 7-Apr-2017
                            // _Tbt_Stock_Status_2 = true;
                            _Tbt_Stock_Status_2 = false;
                            //_Tbt_Stock_Status = "<span style=\"color:" + _Colorcode + "\">TEMPORARY UNAVAILABLE NO ETA</span>";
                            _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                            break;
                        case "TEMPORARY UNAVAILABLE NO ETA":
                            _Colorcode = "#F9A023";
                            //modified by indu Requirement Stock Status update date 7-Apr-2017
                            // _Tbt_Stock_Status_2 = true;
                            _Tbt_Stock_Status_2 = false;
                            _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                            break;
                        case "OUT OF STOCK":
                            _Tbt_Stock_Status_3 = "Out of stock. ";
                            _Tbt_Stock_Status_2 = true;
                            _Colorcode = "#F9A023";
                            _Tbt_Stock_Status_1 = "Back Order Item";
                            _Colorcode1 = "#43A246";
                            break;
                        case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                            _Tbt_Stock_Status_3 = "Out of stock. ";
                            _Tbt_Stock_Status_2 = true;
                            _Colorcode = "#F9A023";
                            _Tbt_Stock_Status_1 = "Back Order Item";
                            _Colorcode1 = "#43A246";
                            break;
                        case "OUT OF STOCK ITEM WILL":
                            _Tbt_Stock_Status_3 = "Out of stock. ";
                            _Tbt_Stock_Status_2 = true;
                            _Colorcode = "#F9A023";
                            _Tbt_Stock_Status_1 = "Back Order Item";
                            _Colorcode1 = "#43A246";
                            break;
                        default:
                            _Colorcode = "Black";
                            _Tbt_Stock_Status = _StockStatusTrim;
                            break;
                    }


                    string stkstatus = string.Empty;
                    stkstatus = _Tbt_Stock_Status.ToUpper();
                    string stkstatus1 = string.Empty;
                    stkstatus1 = _Tbt_Stock_Status_1.ToUpper();



                    if ((isProductReplace == true) && (Issubstitute != ""))
                    {
                        _stmpl_records.SetAttribute("TBT_REPLACED", true);
                        _stmpl_records.SetAttribute("TBT_REPLACED_DETAIL", strReplacedProduct);
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_REPLACED", false);
                        _stmpl_records.SetAttribute("TBT_COLOR_CODE", _Colorcode);
                        bool isSameLogic = true;

                        _stmpl_records.SetAttribute("TBT_PDISPLAY_MODE", _sPriceTable == "" ? "none" : "");

                        //objErrorHandler.CreateLog("PROD_STOCK_FLAG " + cdr["PROD_STOCK_FLAG"].ToString() + "-" + cdr["PRODUCT_ID"].ToString());
                        //objErrorHandler.CreateLog("PROD_STOCK_STATUS " + cdr["PROD_STOCK_STATUS"].ToString() + "-" + cdr["PRODUCT_ID"].ToString());
                        //objErrorHandler.CreateLog("STOCK_STATUS_DESC " + cdr["STOCK_STATUS_DESC"].ToString() + "-" + cdr["PRODUCT_ID"].ToString());


                        //          if (cdr["PROD_STOCK_FLAG"].ToString() == "-2" || cdr["PROD_STOCK_STATUS"].ToString() == "1" || (cdr["PROD_STOCK_STATUS"].ToString() == "1" && cdr["PROD_LEGISTATED_STATE"].ToString() != ""))


                        if ((cdr["PROD_STOCK_FLAG"].ToString() == "-2" && cdr["PROD_STOCK_STATUS"].ToString() == "False" && cdr["STOCK_STATUS_DESC"].ToString().Trim() == "OUT_OF_STOCK ITEM WILL BE BACK ORDERED") || (cdr["PROD_STOCK_FLAG"].ToString() == "0" && cdr["PROD_STOCK_STATUS"].ToString() == "True" && cdr["STOCK_STATUS_DESC"].ToString().Trim() == "Please_Call") || (cdr["PROD_STOCK_FLAG"].ToString() == "-2" && cdr["PROD_STOCK_STATUS"].ToString() == "False" && cdr["STOCK_STATUS_DESC"].ToString().Trim() == "SPECIAL_ORDER PRICE & AVAILABILITY TO BE CONFIRMED"))
                        {
                            //objErrorHandler.CreateLog("check"+ cdr["ETA"].ToString());
                            //if (cdr["ETA"].ToString() == "PLEASE CALL" || cdr["ETA"].ToString() == "")
                            //{
                                isSameLogic = GetStockDetails(_stmpl_records, cdr["PRODUCT_ID"].ToString(), cdr["ETA"].ToString());
                            //}
                            //else
                            //{
                            //    isSameLogic = true;
                            //}


                            //if (cdr["SUPPLIER_ITEM_CODE"] != null && cdr["SUPPLIER_ITEM_CODE"].ToString() != string.Empty)
                            //{
                            //    isSameLogic = GetStockDetails(_stmpl_records, cdr["PRODUCT_ID"].ToString());
                            //}
                            //else
                            //{
                            //    isSameLogic = true;
                            //}
                        }
                        else
                        {
                            isSameLogic = true;
                        }

                        //objErrorHandler.CreateLog("isSameLogic " + isSameLogic);
                        if (isSameLogic)
                        {
                            _stmpl_records.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);
                            if (!_Tbt_Stock_Status.Equals(""))
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                                if (_Tbt_Stock_Status == "Limited Stock")
                                {
                                    _stmpl_records.SetAttribute("TBT_PLEASE_CALL", "true");
                                }
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
                            }
                            _stmpl_records.SetAttribute("TBT_COLOR_CODE_1", _Colorcode1);
                            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
                            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                            //objErrorHandler.CreateLog("before display ETA");
                            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
                            //objErrorHandler.CreateLog("after display ETA");
                            if (stkstatus == "INSTOCK" || stkstatus1 == "INSTOCK")
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                                _stmpl_records.SetAttribute("TBT_ISINSTOCK", true);
                                //  objErrorHandler.CreateLog("Peoduct inside instock"); 
                                _stmpl_records.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                            }
                            else if (stkstatus == "ITEM WILL BE BACK ORDERED" || stkstatus1 == "ITEM WILL BE BACK ORDERED")
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/OutOfStock");
                            }
                            else if (stkstatus.Contains("SPECIAL") || stkstatus1.Contains("SPECIAL"))
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/LimitedAvailability");
                            }
                            else if (!(_Tbt_Stock_Status_2))
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/Discontinued");
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                            }

                            //_stmpl_records.SetAttribute("TBT_STOCK_STATUS", GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"])));
                            //  _stmpl_records.SetAttribute("TBT_PRICE_TABLE", _sPriceTable);

                        }



                    }
                    //  sw5.Stop();
                    //  //objErrorHandler.ExeTimelog = "STOCK Status logic =mins- " + sw5.Elapsed.Minutes.ToString() + ",seconds-" + sw5.Elapsed.Seconds.ToString() + ",ms-" + sw5.Elapsed.Milliseconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    //  objErrorHandler.ExeTimelog = "STOCK Status logic =" + sw5.Elapsed.TotalSeconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    //  objErrorHandler.createexecutiontmielog();

                    //Thread t5 = new Thread(delegate() { ST_product_Load_5(_stmpl_records, cdr, _StockStatus, isProductReplace, _sPriceTable, _Eta); });
                    //t5.Start();
                    //  Stopwatch sw6 = new Stopwatch();
                    //  sw6.Start();
                    try
                    {
                        GetProductDetails(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records, HttpContext.Current);
                    }
                    catch
                    { }
                    if (brandname != "")
                    {
                        _stmpl_records.SetAttribute("BRAND_NAME", brandname);
                        _stmpl_records.SetAttribute("TBT_BRAND_NAME", true);
                    }
                    else
                        _stmpl_records.SetAttribute("TBT_BRAND_NAME", false);

                    //  sw6.Stop();
                    // //objErrorHandler.ExeTimelog = "GetProductDetails =mins- " + sw6.Elapsed.Minutes.ToString() + ",seconds-" + sw6.Elapsed.Seconds.ToString() + ",ms-" + sw6.Elapsed.Milliseconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    // objErrorHandler.ExeTimelog = "GetProductDetails = " + sw6.Elapsed.TotalSeconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    // objErrorHandler.createexecutiontmielog();
                    // comment start 

                    //  Stopwatch swpd = new Stopwatch();
                    // swpd.Start();

                    ST_Product_Download(cdr["PRODUCT_ID"].ToString());

                    _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", downloadST);
                    _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
                    downloadST = "";


                    if (isdownload == true)
                    {
                        chkdwld = true;
                        string dwldmrge = ST_Downloads_Update(HttpContext.Current);
                        dwldmrge = downloadST + dwldmrge;
                        _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", dwldmrge);
                        _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
                        downloadST = "";
                    }
                    else
                    {
                        isdownload = true;
                        chkdwld = false;
                        _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", ST_Downloads_Update(HttpContext.Current));
                        _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
                    }
                    // comment end 


                    string cthref = "";

                    cthref = ST_VPC();
                    _stmpl_records.SetAttribute("TBT_CT_HREF", cthref);
                    if (HttpContext.Current.Session["PRO_CAT_NAME"] != null)
                        _stmpl_records.SetAttribute("TBT_CT_NAME", HttpContext.Current.Session["PRO_CAT_NAME"]);
                    else
                        _stmpl_records.SetAttribute("TBT_CT_NAME", "Category Name");

                    string userId = HttpContext.Current.Session["USER_ID"].ToString();
                    if (userId == "" || userId == null)
                        userId = ConfigurationManager.AppSettings["DUM_USER_ID"];

                    string sqlexec =  "exec GetPriceTableWagner '" + cdr["PRODUCT_ID"].ToString() + "','"+ userId + "'";
                    //objErrorHandler.CreateLog(sqlexec);
                    DataSet Dsall = objHelperDB.GetDataSetDB(sqlexec);
                    string bulkbuy = string.Empty;

                    if (Dsall != null && Dsall.Tables.Count >0)
                    {
                        DataTable Sqltb = Dsall.Tables[0];

                        if (Sqltb.Rows.Count > 0)
                        {
                            bulkbuy = string.Format("<div class=\"mt10\" style=\"font-style:italic;\"><span style=\"color:#1059a3;font-size:18px;font-weight:bold;\">Bulk Buy :</span><span style=\"color:#1059a3;font-size:18px;font-weight:normal;\"> $" + Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE1"].ToString()), 2) + " for " + Sqltb.Rows[0]["QTY"].ToString().Replace("+", "") + " or more</span><div>");
                            //objErrorHandler.CreateLog(bulkbuy);
                        }
                    }
                    _stmpl_records.SetAttribute("TBT_BULK_BUY", bulkbuy);
                    // swpd.Start();
                    // objErrorHandler.ExeTimelog = "ST_Product_Download + HREF = " + swpd.Elapsed.TotalSeconds.ToString() + "pid=" + cdr["PRODUCT_ID"];
                    // objErrorHandler.createexecutiontmielog();


                    //string usr_id = HttpContext.Current.Session["USER_ID"].ToString();
                    //if (usr_id != ConfigurationManager.AppSettings["DUM_USER_ID"].ToString() && usr_id != "")
                    //{
                    //    try
                    //    {
                    //        string img_url = "";
                    //        string raw_url = "";

                    //        if (HttpContext.Current.Session["INSERT_IMAGE_URL"] != "" && HttpContext.Current.Session["INSERT_IMAGE_URL"] != null)
                    //            img_url = HttpContext.Current.Session["INSERT_IMAGE_URL"].ToString();
                    //        else
                    //            img_url = "/images/noimages.gif";
                    //        raw_url = HttpContext.Current.Request.RawUrl.ToString();
                    //        DataSet tmpds = objHelperServices.CheckUserBrowseItem(cdr["PRODUCT_ID"].ToString(), cdr["FAMILY_ID"].ToString(), usr_id);
                    //        if (tmpds == null)
                    //            objHelperServices.User_Browse_Item(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(cdr["FAMILY_ID"]), Convert.ToInt32(usr_id), cdr["FAMILY_NAME"].ToString(), descall, "", img_url, raw_url, cdr["COST"].ToString());
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        objErrorHandler.ErrorMsg = ex;
                    //        objErrorHandler.CreateLog();
                    //    }
                    //}
                }



                _stmpl_records1.SetAttribute("TBWDataList", _stmpl_records);

                lstrecords[ictrecords] = new TBWDataList(_stmpl_records1.ToString());
                //}

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);


                sHTML = _stmpl_container.ToString();
                ErrorHandler objErrorhandler = new ErrorHandler();
                // objErrorhandler.CreateLog("after   ST_Product" + DateTime.Now.ToString("hh.mm.ss.ffffff") + HttpContext.Current.Request.RawUrl.ToString());
                stpw.Stop();

                // //objErrorHandler.ExeTimelog = "ST_Product_Load_Full = mins-" + stpw.Elapsed.Minutes.ToString() + ",seconds-" + stpw.Elapsed.Seconds.ToString() + ",mills-" + stpw.Elapsed.Milliseconds.ToString();
                objErrorHandler.ExeTimelog = "ST_Product_Load_Full = " + stpw.Elapsed.TotalSeconds.ToString();
                objErrorHandler.createexecutiontmielog();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            finally
            {





            }
            return objHelperService.ProdimageRreplaceImages(sHTML, _Package);
        }
        private void ST_product_Load_1(StringTemplate _stmpl_records, int pid, int fid)
        {
            try
            {
                GetMultipleImages(pid, System.Convert.ToInt32(paraFID), fid, _stmpl_records);
            }
            catch
            { }
        }
        private void ST_product_Load_2(StringTemplate _stmpl_records, string _fid)
        {

            DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
            if (tmpds != null && tmpds.Tables.Count > 0)
            {
                _stmpl_records.SetAttribute("TBT_CATEGORY_ID", tmpds.Tables[0].Rows[0]["CATEGORY_ID"]);
                string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"];
                _stmpl_records.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                _stmpl_records.SetAttribute("ORG_TBT_EA_PATH", eapath);
            }


        }
        private void ST_product_Load_3(StringTemplate _stmpl_records, DataRow cdr)
        {
            if (cdr["FAMILY_PROD_COUNT"] != null && cdr["PROD_COUNT"] != null)
            {
                if (!cdr["FAMILY_PROD_COUNT"].ToString().Equals(cdr["PROD_COUNT"].ToString()))
                {
                    DataSet _parentFamilyds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, cdr["FAMILY_ID"].ToString(), "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
                    string _parentFamily_Id = "0";
                    if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                        _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

                    if (_parentFamily_Id.Equals("0"))
                        _parentFamily_Id = cdr["FAMILY_ID"].ToString();

                    _stmpl_records.SetAttribute("TBT_PARENT_FAMILY_ID", _parentFamily_Id);

                    _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "block");
                }
                else
                {
                    _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");
                }
                //objHelperService.Cons_NewURl(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" + cdr["FAMILY_name"].ToString(), "fl.aspx", true, "");
            }
            else
                _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");

        }
        private void ST_product_Load_4(StringTemplate _stmpl_records, DataRow cdr, DataSet dsrecords)
        {
            string prodValues = "";
            string descall = string.Empty;
            string desc1 = string.Empty;
            string descallstring = string.Empty;
            string attName = string.Empty;
            string drstrval = string.Empty;
            string attr_name_replace = string.Empty;
            DataRow[] dtrow = dsrecords.Tables[0].Select("ATTRIBUTE_DATATYPE like 'TEX%' And ATTRIBUTE_TYPE<>3 And ATTRIBUTE_TYPE<>9");
            if (dtrow.Length > 0)
            {
                foreach (DataRow dr in dtrow)
                {
                    desc1 = "";
                    if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                    {
                        attName = dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();
                        drstrval = dr["STRING_VALUE"].ToString();

                        attr_name_replace = attName;

                        if (attName.Equals("SHORT_DESCRIPTION") || attName.Equals("DESCRIPTIONS") || attName.Equals("FEATURES") || attName.Equals("SPECIFICATION") || attName.Equals("SPECIFICATIONS") || attName.Equals("APPLICATIONS") || attName.Equals("NOTES"))
                        {

                            desc1 = drstrval.Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                        }
                        else if (attName.Equals("DESCRIPTION"))
                        {
                            if (drstrval.Length > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_" + attr_name_replace, drstrval);
                                _stmpl_records.SetAttribute("PROD_DESC_ALT", drstrval);
                                _stmpl_records.SetAttribute("PROD_DESC_TITLE", drstrval.Replace('"', ' '));
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("PROD_DESC_ALT", drstrval);
                                _stmpl_records.SetAttribute("PROD_DESC_TITLE", drstrval.Replace('"', ' '));
                            }

                        }
                        else
                            _stmpl_records.SetAttribute("TBT_" + attr_name_replace, drstrval);

                        if (!desc1.Equals(""))
                            descall = descall + desc1 + "<br/><br/>";
                    }


                }
            }
            if (descall == string.Empty)
                _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", true);
            else
                _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", false);


            if (descall.Length > 400)
                _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
            else
                _stmpl_records.SetAttribute("TBT_MORE_SHOW", false);

            if (descall.Length > 400)
            {

                descallstring = descall.Substring(0, 400).ToString();
                _stmpl_records.SetAttribute("TBT_MORE", descallstring);
                _stmpl_records.SetAttribute("TBT_MENU_ID", "2");
                descall = descall.Substring(0, descall.Length).ToString();


                _stmpl_records.SetAttribute("TBT_DESCALL", descall);

            }
            else
            {
                _stmpl_records.SetAttribute("TBT_DESCALL", descall);
                _stmpl_records.SetAttribute("TBT_MENU_ID", "2");

                _stmpl_records.SetAttribute("TBT_MORE", descall);
            }


        }
        private void ST_product_Load_5(StringTemplate _stmpl_records, DataRow cdr, string _StockStatus, bool isProductReplace, string _sPriceTable, string _Eta)
        {




            string _Tbt_Stock_Status = string.Empty;
            string _Tbt_Stock_Status_1 = string.Empty;
            bool _Tbt_Stock_Status_2 = false;
            string _Tbt_Stock_Status_3 = string.Empty;
            string _Colorcode1 = string.Empty;
            string _Colorcode;
            string _StockStatusTrim = _StockStatus.Trim();



            switch (_StockStatusTrim)
            {
                case "IN STOCK":
                    _Colorcode = "#43A246";
                    _Tbt_Stock_Status = "INSTOCK";
                    _Tbt_Stock_Status_2 = true;
                    break;
                case "Limited Stock, Please Call":
                    _Colorcode = "#f69e1b";
                    _Tbt_Stock_Status = "Limited Stock";
                    _Tbt_Stock_Status_2 = true;
                    break;
                case "SPECIAL ORDER":
                    _Colorcode = "#43A246";
                    _Tbt_Stock_Status_2 = true;
                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                    break;
                case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                    _Colorcode = "#43A246";
                    _Tbt_Stock_Status_2 = true;
                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                    break;
                case "SPECIAL ORDER PRICE &":
                    _Colorcode = "#43A246";
                    _Tbt_Stock_Status_2 = true;
                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                    break;
                case "DISCONTINUED":
                    _Colorcode = "#ED1C24";
                    _Tbt_Stock_Status_2 = false;
                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                    break;
                case "DISCONTINUED NO LONGER AVAILABLE":
                    _Colorcode = "#ED1C24";
                    _Tbt_Stock_Status_2 = false;
                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                    break;
                case "DISCONTINUED NO LONGER":
                    _Colorcode = "#ED1C24";
                    _Tbt_Stock_Status_2 = false;
                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                    break;
                case "TEMPORARY UNAVAILABLE":
                    _Colorcode = "#F9A023";
                    // _Tbt_Stock_Status_2 = true;
                    _Tbt_Stock_Status_2 = false;
                    //_Tbt_Stock_Status = "<span style=\"color:" + _Colorcode + "\">TEMPORARY UNAVAILABLE NO ETA</span>";
                    _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                    break;
                case "TEMPORARY UNAVAILABLE NO ETA":
                    _Colorcode = "#F9A023";
                    // _Tbt_Stock_Status_2 = true;
                    _Tbt_Stock_Status_2 = false;
                    _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                    break;
                case "OUT OF STOCK":
                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
                    _Tbt_Stock_Status_2 = true;
                    _Colorcode = "#F9A023";
                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                    _Colorcode1 = "#43A246";
                    break;
                case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
                    _Tbt_Stock_Status_2 = true;
                    _Colorcode = "#F9A023";
                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                    _Colorcode1 = "#43A246";
                    break;
                case "OUT OF STOCK ITEM WILL":
                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
                    _Tbt_Stock_Status_2 = true;
                    _Colorcode = "#F9A023";
                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                    _Colorcode1 = "#43A246";
                    break;
                default:
                    _Colorcode = "Black";
                    _Tbt_Stock_Status = _StockStatusTrim;
                    break;
            }


            string stkstatus = string.Empty;
            stkstatus = _Tbt_Stock_Status.ToUpper();
            string stkstatus1 = string.Empty;
            stkstatus1 = _Tbt_Stock_Status_1.ToUpper();



            if (isProductReplace == true)
            {
                _stmpl_records.SetAttribute("TBT_REPLACED", true);
                // _stmpl_records.SetAttribute("TBT_REPLACED_DETAIL", strReplacedProduct);
            }
            else
            {

                _stmpl_records.SetAttribute("TBT_REPLACED", false);
                _stmpl_records.SetAttribute("TBT_COLOR_CODE", _Colorcode);
                if (!_Tbt_Stock_Status.Equals(""))
                {
                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                    if (_Tbt_Stock_Status == "Limited Stock")
                    {
                        _stmpl_records.SetAttribute("TBT_PLEASE_CALL", "-Please Call ..");
                    }
                }
                else
                {
                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
                }
                _stmpl_records.SetAttribute("TBT_COLOR_CODE_1", _Colorcode1);
                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);

                if (stkstatus == "INSTOCK" || stkstatus1 == "INSTOCK")
                {
                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                }
                else if (stkstatus == "ITEM WILL BE BACK ORDERED" || stkstatus1 == "ITEM WILL BE BACK ORDERED")
                {
                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/OutOfStock");
                }
                else if (stkstatus.Contains("SPECIAL") || stkstatus1.Contains("SPECIAL"))
                {
                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/LimitedAvailability");
                }
                else if (!(_Tbt_Stock_Status_2))
                {
                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/Discontinued");
                }
                else
                {
                    _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                }

                //_stmpl_records.SetAttribute("TBT_STOCK_STATUS", GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"])));
                _stmpl_records.SetAttribute("TBT_PRICE_TABLE", _sPriceTable);
                _stmpl_records.SetAttribute("TBT_PDISPLAY_MODE", _sPriceTable == "" ? "none" : "");

            }



        }
        private void ST_product_Load_6(StringTemplate _stmpl_records, DataRow cdr, HttpContext ctx)
        {
            try
            {
                GetProductDetails(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records, ctx);
            }
            catch
            { }



            ST_Product_Download(cdr["PRODUCT_ID"].ToString());

            _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", downloadST);
            _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
            downloadST = "";


            if (isdownload == true)
            {
                chkdwld = true;
                string dwldmrge = ST_Downloads_Update(ctx);
                dwldmrge = downloadST + dwldmrge;
                _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", dwldmrge);
                _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
                downloadST = "";
            }
            else
            {
                isdownload = true;
                chkdwld = false;
                _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", ST_Downloads_Update(ctx));
                _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
            }




        }
        private string ST_VPC()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string vpchref = string.Empty;
            string eapath = string.Empty;
            Security objSecurity = new Security();
            try
            {
                DataSet bcvpc = new DataSet();
                if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                {
                    bcvpc = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                }

                if (bcvpc != null && bcvpc.Tables[0].Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow dr in bcvpc.Tables[0].Rows)
                    {
                        if (dr["ItemType"].ToString().ToLower() == "category")
                        {
                            vpchref = "";
                            eapath = "";
                            vpchref = dr["Url"].ToString();
                            eapath = dr["EAPath"].ToString();
                            HttpContext.Current.Session["PRO_CAT_NAME"] = dr["Itemvalue"].ToString();
                            i = i + 1;
                            if (i == 2)
                            {
                                break;
                            }
                        }
                    }
                    string NEWURL = string.Empty;
                    string HREFURL = string.Empty;
                    string urlpage = string.Empty;
                    if (vpchref.Contains("ct.aspx") == true)
                        urlpage = "ct.aspx";
                    else
                        urlpage = "pl.aspx";
                    //NEWURL = objHelperServices.Cons_NewURl_bybrand(vpchref, eapath, urlpage, "");
                    //objErrorHandler.CreateLog("vpchref eapath " + eapath);
                    NEWURL = objHelperServices.SimpleURL_Str(eapath, urlpage, true);

                    if (urlpage == "ct.aspx")
                        vpchref = NEWURL + "/ct/";
                    else
                        vpchref = NEWURL + "/pl/";

                    // vpchref = vpchref + "&path=" + eapath;
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return e.Message;
            }
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_VPC = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return vpchref;
        }

        private string GetProductReplacementDetails(StringTemplate _stmpl_records, string _CODE, int user_id, string _StockStatus)
        {
            string _catid = "", pfid = "", Ea_Path = "", wag_product_code = "", SubstuyutePid = "";
            string _sPriceTable = "";
            bool samecodesubproduct = false;
            bool samecodenotFound = false;
            DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_CODE, user_id, "pd");
            if (rtntbl != null && rtntbl.Rows.Count > 0)
            {

                _catid = rtntbl.Rows[0]["CatId"].ToString();
                pfid = rtntbl.Rows[0]["Pfid"].ToString();
                // Ea_Path = "/"+rtntbl.Rows[0]["Ea_Path"].ToString();
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
                _stmpl_records.SetAttribute("TBT_NIL_REPLACED", true);
                _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", _CODE);
                _stmpl_records.SetAttribute("TBT_REP_STATUS", _StockStatus);
                // _sPriceTable += string.Format("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _CODE);
                // _sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Temporarily Unavailable <br>Please Contact Us for more details");
            }
            else if (samecodenotFound == false && samecodesubproduct == false)
            {
                //_sPriceTable += string.Format("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _CODE);
                //_sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Not Available.");
                //_sPriceTable += "<tr class=\"success\"><td colspan=\"3\" align\"center\"><br>RECOMMENDED REPLACEMENT<br><br></td></tr>";
                //_sPriceTable += "<tr><td colspan=\"3\" align\"center\">";
                //_sPriceTable += "<br>Order Code : " + "<span  style=\"color:green;font-weight: bold;\">" + wag_product_code + "</span> <br>";
                //string strurl = "ProductDetails.aspx?Pid=" + SubstuyutePid + "&amp;fid=" + pfid + "&amp;Cid=" + _catid + "&amp;path=" + Ea_Path;
                //_sPriceTable += "<br><a href =\"" + Ea_Path + "\" style=\"font-weight: bold; text-decoration: none; color: #1589FF;\" > View Replacement Product </a>";
                //_sPriceTable += "<br><br></td></tr>";

                _stmpl_records.SetAttribute("TBT_NIL_REPLACED", false);
                _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", wag_product_code);
                _stmpl_records.SetAttribute("TBT_REP_EA_PATH", Ea_Path);

            }
            else
            {
                _stmpl_records.SetAttribute("TBT_NIL_REPLACED", true);
                _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", _CODE);
                _stmpl_records.SetAttribute("TBT_REP_STATUS", _StockStatus);
                //_sPriceTable += string.Format("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _CODE);
                //_sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Temporarily Unavailable <br>Please Contact Us for more details");
            }

            return _sPriceTable;

        }
        public string ST_Downloads_Update(HttpContext ctx)
        {
            StringTemplateGroup _stg_container = null;
            StringTemplate _stmpl_container = null;
            try
            {

                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                if (chkdwld == false)
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DowloadUpdate");
                else
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "WithDowloadUpdate");

                //if (HttpContext.Current.Session["AQ_PD_CAPTCH_IMAGE"] != null)
                //{
                //    _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_PD_CAPTCH_IMAGE"].ToString());
                //    _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_PD_CAPTCH_VALUE"].ToString());
                //}
                if (ctx.Session["FNAME_PRODUCT"] != null)
                    _stmpl_container.SetAttribute("FNAME_DU", ctx.Session["FNAME_PRODUCT"].ToString());

                return _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
            return "";
        }
        //private void SetCookie( string option_name, string fname,string code,string img1,string family_url,string family_url_org,string product_url)
        //{


        //    HttpCookie RVPCookie = HttpContext.Current.Request.Cookies["RVPCookie"];
        //    if (RVPCookie == null)
        //        RVPCookie = new HttpCookie("RVPCookie");

        //    try
        //    {
        //        if (option_name == "family_name")
        //        {
        //            if (RVPCookie != null)
        //            {

        //                if (RVPCookie["family_name1"] != null && RVPCookie["family_name2"] != null && RVPCookie["family_name3"] != null && RVPCookie["family_name1"] != fname && RVPCookie["family_name2"] != fname && RVPCookie["family_name3"] != fname)
        //                {
        //                    if (RVPCookie["family_name_lastvisit1"] != null && RVPCookie["family_name_lastvisit2"] != null && RVPCookie["family_name_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["family_name_lastvisit1"]);
        //                        nameList.Add(RVPCookie["family_name_lastvisit2"]);
        //                        nameList.Add(RVPCookie["family_name_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_family_name1") == true)
        //                        {
        //                            RVPCookie["family_name1"] = fname;
        //                            RVPCookie["family_name_lastvisit1"] = DateTime.Now.ToString() + "_family_name1";
        //                        }
        //                        else if (nameList[0].Contains("_family_name2") == true)
        //                        {
        //                            RVPCookie["family_name2"] = fname;
        //                            RVPCookie["family_name_lastvisit2"] = DateTime.Now.ToString() + "_family_name2";
        //                        }
        //                        else if (nameList[0].Contains("_family_name3") == true)
        //                        {
        //                            RVPCookie["family_name3"] = fname;
        //                            RVPCookie["family_name_lastvisit3"] = DateTime.Now.ToString() + "_family_name3";
        //                        }

        //                    }
        //                }
        //            }
        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["family_name1"] == null && RVPCookie["family_name1"] != fname)
        //                {
        //                    RVPCookie["family_name1"] = fname;
        //                    RVPCookie["family_name_lastvisit1"] = DateTime.Now.ToString() + "_family_name1";
        //                }
        //                else if (RVPCookie["family_name1"] != null && RVPCookie["family_name2"] == null && RVPCookie["family_name3"] == null && RVPCookie["family_name1"] != fname && RVPCookie["family_name2"] != fname)
        //                {
        //                    RVPCookie["family_name2"] = fname;
        //                    RVPCookie["family_name_lastvisit2"] = DateTime.Now.ToString() + "_family_name2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["family_name3"] == null && RVPCookie["family_name3"] != fname && RVPCookie["family_name1"] != null && RVPCookie["family_name2"] != null && RVPCookie["family_name1"] != fname && RVPCookie["family_name2"] != fname)
        //                    {
        //                        RVPCookie["family_name3"] = fname;
        //                        RVPCookie["family_name_lastvisit3"] = DateTime.Now.ToString() + "_family_name3";
        //                    }

        //                }
        //            }
        //            //else
        //            //{
        //            //    //HttpCookie RVPCookie = new HttpCookie("RVPCookie");
        //            //    RVPCookie["family_name1"] = fname;
        //            //}
        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }

        //        if (option_name == "code")
        //        {
        //            if (RVPCookie != null)
        //            {

        //                if (RVPCookie["code1"] != null && RVPCookie["code2"] != null && RVPCookie["code3"] != null && RVPCookie["code1"] != code && RVPCookie["code2"] != code && RVPCookie["code3"] != code)
        //                {
        //                    if (RVPCookie["code_lastvisit1"] != null && RVPCookie["code_lastvisit2"] != null && RVPCookie["code_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["code_lastvisit1"]);
        //                        nameList.Add(RVPCookie["code_lastvisit2"]);
        //                        nameList.Add(RVPCookie["code_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_code1") == true)
        //                        {
        //                            RVPCookie["code1"] = code;
        //                            RVPCookie["code_lastvisit1"] = DateTime.Now.ToString() + "_code1";
        //                        }
        //                        else if (nameList[0].Contains("_code2") == true)
        //                        {
        //                            RVPCookie["code2"] = code;
        //                            RVPCookie["code_lastvisit2"] = DateTime.Now.ToString() + "_code2";
        //                        }
        //                        else if (nameList[0].Contains("_code3") == true)
        //                        {
        //                            RVPCookie["code3"] = code;
        //                            RVPCookie["code_lastvisit3"] = DateTime.Now.ToString() + "_code3";
        //                        }

        //                    }
        //                }
        //            }

        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["code1"] == null && RVPCookie["code1"] != code)
        //                {
        //                    RVPCookie["code1"] = code;
        //                    RVPCookie["code_lastvisit1"] = DateTime.Now.ToString() + "_code1";
        //                }
        //                else if (RVPCookie["code1"] != null && RVPCookie["code2"] == null && RVPCookie["code3"] == null && RVPCookie["code1"] != code && RVPCookie["code2"] != code)
        //                {
        //                    RVPCookie["code2"] = code;
        //                    RVPCookie["code_lastvisit2"] = DateTime.Now.ToString() + "_code2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["code3"] == null && RVPCookie["code3"] != code && RVPCookie["code1"] != null && RVPCookie["code2"] != null && RVPCookie["code1"] != code && RVPCookie["code2"] != code)
        //                    {
        //                        RVPCookie["code3"] = code;
        //                        RVPCookie["code_lastvisit3"] = DateTime.Now.ToString() + "_code3";
        //                    }

        //                }
        //            }

        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }
        //        if (option_name == "img1")
        //        {
        //            if (RVPCookie != null)
        //            {

        //                if (RVPCookie["img1"] != null && RVPCookie["img2"] != null && RVPCookie["img3"] != null && RVPCookie["img1"] != img1 && RVPCookie["img2"] != img1 && RVPCookie["img3"] != img1)
        //                {
        //                    if (RVPCookie["img_lastvisit1"] != null && RVPCookie["img_lastvisit2"] != null && RVPCookie["img_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["img_lastvisit1"]);
        //                        nameList.Add(RVPCookie["img_lastvisit2"]);
        //                        nameList.Add(RVPCookie["img_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_img1") == true)
        //                        {
        //                            RVPCookie["img1"] = img1;
        //                            RVPCookie["img_lastvisit1"] = DateTime.Now.ToString() + "_img1";
        //                        }
        //                        else if (nameList[0].Contains("_img2") == true)
        //                        {
        //                            RVPCookie["img2"] = img1;
        //                            RVPCookie["img_lastvisit2"] = DateTime.Now.ToString() + "_img2";
        //                        }
        //                        else if (nameList[0].Contains("_img3") == true)
        //                        {
        //                            RVPCookie["img3"] = img1;
        //                            RVPCookie["img_lastvisit3"] = DateTime.Now.ToString() + "_img3";
        //                        }

        //                    }
        //                }
        //            }

        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["img1"] == null && RVPCookie["img1"] != img1)
        //                {
        //                    RVPCookie["img1"] = img1;
        //                    RVPCookie["img_lastvisit1"] = DateTime.Now.ToString() + "_img1";
        //                }
        //                else if (RVPCookie["img1"] != null && RVPCookie["img2"] == null && RVPCookie["img3"] == null && RVPCookie["img1"] != img1 && RVPCookie["img2"] != img1)
        //                {
        //                    RVPCookie["img2"] = img1;
        //                    RVPCookie["img_lastvisit2"] = DateTime.Now.ToString() + "_img2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["img3"] == null && RVPCookie["img3"] != img1 && RVPCookie["img1"] != null && RVPCookie["img2"] != null && RVPCookie["img1"] != img1 && RVPCookie["img2"] != img1)
        //                    {
        //                        RVPCookie["img3"] = img1;
        //                        RVPCookie["img_lastvisit3"] = DateTime.Now.ToString() + "_img3";
        //                    }

        //                }
        //            }

        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }
        //        if (option_name == "family_url")
        //        {
        //            if (RVPCookie != null)
        //            {
        //                family_url = HttpUtility.UrlEncode(family_url.Trim());
        //                family_url = family_url.Replace("%2f", "/").Replace("%3f", "?").Replace("%3d", "=").Replace("%26", "&");
        //                if (RVPCookie["family_url1"] != null && RVPCookie["family_url2"] != null && RVPCookie["family_url3"] != null && RVPCookie["family_url1"] != family_url && RVPCookie["family_url2"] != family_url && RVPCookie["family_url3"] != family_url)
        //                {
        //                    if (RVPCookie["family_url_lastvisit1"] != null && RVPCookie["family_url_lastvisit2"] != null && RVPCookie["family_url_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["family_url_lastvisit1"]);
        //                        nameList.Add(RVPCookie["family_url_lastvisit2"]);
        //                        nameList.Add(RVPCookie["family_url_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_family_url1") == true)
        //                        {
        //                            RVPCookie["family_url1"] = family_url;
        //                            RVPCookie["family_url_lastvisit1"] = DateTime.Now.ToString() + "_family_url1";
        //                        }
        //                        else if (nameList[0].Contains("_family_url2") == true)
        //                        {
        //                            RVPCookie["family_url2"] = family_url;
        //                            RVPCookie["family_url_lastvisit2"] = DateTime.Now.ToString() + "_family_url2";
        //                        }
        //                        else if (nameList[0].Contains("_family_url3") == true)
        //                        {
        //                            RVPCookie["family_url3"] = family_url;
        //                            RVPCookie["family_url_lastvisit3"] = DateTime.Now.ToString() + "_family_url3";
        //                        }

        //                    }
        //                }
        //            }

        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["family_url1"] == null && RVPCookie["family_url1"] != family_url)
        //                {
        //                    RVPCookie["family_url1"] = family_url;
        //                    RVPCookie["family_url_lastvisit1"] = DateTime.Now.ToString() + "_family_url1";
        //                }
        //                else if (RVPCookie["family_url1"] != null && RVPCookie["family_url2"] == null && RVPCookie["family_url3"] == null && RVPCookie["family_url1"] != family_url && RVPCookie["family_url2"] != family_url)
        //                {
        //                    RVPCookie["family_url2"] = family_url;
        //                    RVPCookie["family_url_lastvisit2"] = DateTime.Now.ToString() + "_family_url2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["family_url3"] == null && RVPCookie["family_url3"] != family_url && RVPCookie["family_url1"] != null && RVPCookie["family_url2"] != null && RVPCookie["family_url1"] != family_url && RVPCookie["family_url2"] != family_url)
        //                    {
        //                        RVPCookie["family_url3"] = family_url;
        //                        RVPCookie["family_url_lastvisit3"] = DateTime.Now.ToString() + "_family_url3";
        //                    }

        //                }
        //            }

        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }

        //        if (option_name == "family_url_org")
        //        {
        //            if (RVPCookie != null)
        //            {
        //                family_url_org = HttpUtility.UrlEncode(family_url_org.Trim());
        //                family_url_org = family_url_org.Replace("%2f", "/").Replace("%3f", "?").Replace("%3d", "=").Replace("%26", "&");
        //                if (RVPCookie["family_url_org1"] != null && RVPCookie["family_url_org2"] != null && RVPCookie["family_url_org3"] != null && RVPCookie["family_url_org1"] != family_url_org && RVPCookie["family_url_org2"] != family_url_org && RVPCookie["family_url_org3"] != family_url_org)
        //                {
        //                    if (RVPCookie["family_url_org_lastvisit1"] != null && RVPCookie["family_url_org_lastvisit2"] != null && RVPCookie["family_url_org_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["family_url_org_lastvisit1"]);
        //                        nameList.Add(RVPCookie["family_url_org_lastvisit2"]);
        //                        nameList.Add(RVPCookie["family_url_org_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_family_url_org1") == true)
        //                        {
        //                            RVPCookie["family_url_org1"] = family_url_org;
        //                            RVPCookie["family_url_org_lastvisit1"] = DateTime.Now.ToString() + "_family_url_org1";
        //                        }
        //                        else if (nameList[0].Contains("_family_url_org2") == true)
        //                        {
        //                            RVPCookie["family_url_org2"] = family_url_org;
        //                            RVPCookie["family_url_org_lastvisit2"] = DateTime.Now.ToString() + "_family_url_org2";
        //                        }
        //                        else if (nameList[0].Contains("_family_url_org3") == true)
        //                        {
        //                            RVPCookie["family_url_org3"] = family_url_org;
        //                            RVPCookie["family_url_org_lastvisit3"] = DateTime.Now.ToString() + "_family_url_org3";
        //                        }

        //                    }
        //                }
        //            }

        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["family_url_org1"] == null && RVPCookie["family_url_org1"] != family_url_org)
        //                {
        //                    RVPCookie["family_url_org1"] = family_url_org;
        //                    RVPCookie["family_url_org_lastvisit1"] = DateTime.Now.ToString() + "_family_url_org1";
        //                }
        //                else if (RVPCookie["family_url_org1"] != null && RVPCookie["family_url_org2"] == null && RVPCookie["family_url_org3"] == null && RVPCookie["family_url_org1"] != family_url_org && RVPCookie["family_url_org2"] != family_url_org)
        //                {
        //                    RVPCookie["family_url_org2"] = family_url_org;
        //                    RVPCookie["family_url_org_lastvisit2"] = DateTime.Now.ToString() + "_family_url_org2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["family_url_org3"] == null && RVPCookie["family_url_org3"] != family_url_org && RVPCookie["family_url_org1"] != null && RVPCookie["family_url_org2"] != null && RVPCookie["family_url_org1"] != family_url_org && RVPCookie["family_url_org2"] != family_url_org)
        //                    {
        //                        RVPCookie["family_url_org3"] = family_url_org;
        //                        RVPCookie["family_url_org_lastvisit3"] = DateTime.Now.ToString() + "_family_url_org3";
        //                    }

        //                }
        //            }

        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }
        //        if (option_name == "product_url")
        //        {
        //            if (RVPCookie != null)
        //            {
        //                product_url = HttpUtility.UrlEncode(product_url.Trim());
        //                product_url = product_url.Replace("%2f", "/").Replace("%3f", "?").Replace("%3d", "=").Replace("%26", "&");
        //                if (RVPCookie["product_url1"] != null && RVPCookie["product_url2"] != null && RVPCookie["product_url3"] != null && RVPCookie["product_url1"] != product_url && RVPCookie["product_url2"] != product_url && RVPCookie["product_url3"] != product_url)
        //                {
        //                    if (RVPCookie["product_url_lastvisit1"] != null && RVPCookie["product_url_lastvisit2"] != null && RVPCookie["product_url_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["product_url_lastvisit1"]);
        //                        nameList.Add(RVPCookie["product_url_lastvisit2"]);
        //                        nameList.Add(RVPCookie["product_url_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_product_url1") == true)
        //                        {
        //                            RVPCookie["product_url1"] = product_url;
        //                            RVPCookie["product_url_lastvisit1"] = DateTime.Now.ToString() + "_product_url1";
        //                        }
        //                        else if (nameList[0].Contains("_product_url2") == true)
        //                        {
        //                            RVPCookie["product_url2"] = product_url;
        //                            RVPCookie["product_url_lastvisit2"] = DateTime.Now.ToString() + "_product_url2";
        //                        }
        //                        else if (nameList[0].Contains("_product_url3") == true)
        //                        {
        //                            RVPCookie["product_url3"] = product_url;
        //                            RVPCookie["product_url_lastvisit3"] = DateTime.Now.ToString() + "_product_url3";
        //                        }

        //                    }
        //                }
        //            }

        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["product_url_org1"] == null && RVPCookie["product_url1"] != product_url)
        //                {
        //                    RVPCookie["product_url1"] = product_url;
        //                    RVPCookie["product_url_lastvisit1"] = DateTime.Now.ToString() + "_product_url1";
        //                }
        //                else if (RVPCookie["product_url1"] != null && RVPCookie["product_url2"] == null && RVPCookie["product_url3"] == null && RVPCookie["product_url1"] != product_url && RVPCookie["product_url2"] != product_url)
        //                {
        //                    RVPCookie["product_url2"] = product_url;
        //                    RVPCookie["product_url_lastvisit2"] = DateTime.Now.ToString() + "_product_url2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["product_url3"] == null && RVPCookie["product_url3"] != product_url && RVPCookie["product_url1"] != null && RVPCookie["product_url2"] != null && RVPCookie["product_url1"] != product_url && RVPCookie["product_url2"] != product_url)
        //                    {
        //                        RVPCookie["product_url3"] = product_url;
        //                        RVPCookie["product_url_lastvisit3"] = DateTime.Now.ToString() + "_product_url_org3";
        //                    }

        //                }
        //            }

        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //}



        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USERS PRICE IN ROUND VALUE ***/
        /********************************************************************************/
        private decimal GetMyPrice(int ProductID)
        {
            decimal retval = 0.00M;
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();

                if (userid == string.Empty)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];

                //if (!string.IsNullOrEmpty(userid))
                //{
                //    string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                //    objHelperService.SQLString = sSQL;
                //    int pricecode = objHelperService.CI(objHelperService.GetValue("price_code"));

                //    if (!string.IsNullOrEmpty(userid))
                //    {
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
                //        objHelperService.SQLString = strquery;
                //        retval = Math.Round(Convert.ToDecimal(objHelperService.GetValue("Numeric_Value")), 2);
                //    }
                //}
                retval = objHelperDB.GetProductPrice(ProductID, 1, userid);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return retval;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE STOCK STATUS USING PRODUCT ID ***/
        /********************************************************************************/
        private string GetStockStatus(int ProductID)
        {
            string Retval = "NO STATUS AVAILABLE";
            try
            {
                //string sSQL = string.Format("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = {0}", ProductID);
                //objHelperService.SQLString = sSQL;
                //Retval = objHelperService.GetValue("PROD_STK_STATUS_DSC").ToString().Replace("_", " ");
                DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTTable);
                //objErrorHandler.CreateLog("inside GetStockStatus");

                if (objrbl != null)
                {
                    Retval = objrbl.Rows[0]["STOCK_STATUS"].ToString().Replace("_", " ");
                }
                else
                {
                    objErrorHandler.CreateLog("NO STATUS AVAILABLE:" + ProductID.ToString());
                    Retval = "NO STATUS AVAILABLE";
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return Retval;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT PRICE TABLE DETAILS USING PRODUCT ID ***/
        /********************************************************************************/

        //private string GetProductPriceIncGST(int ProductID)
        //{
        //    StringBuilder _sPriceTable = new StringBuilder();
        //    try
        //    {
        //        string userid = HttpContext.Current.Session["USER_ID"].ToString();

        //        if (userid == string.Empty)
        //            userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        _sPriceTable = new StringBuilder();
        //    }
        //    return _sPriceTable.ToString();
        //}

        private string GetProductPriceTable(int ProductID)
        {

            StringBuilder _sPriceTable = new StringBuilder();
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();

                if (userid == string.Empty)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];



                if (!string.IsNullOrEmpty(userid))
                {
                    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                    //objHelperService.SQLString = sSQL;
                    //int pricecode = objHelperService.CI(objHelperService.GetValue("price_code"));
                    int pricecode = objHelperDB.GetPriceCode(userid);
                    DataSet dsPriceTable = new DataSet();
                    //SqlDataAdapter oDa = new SqlDataAdapter();
                    //oDa.SelectCommand = new SqlCommand();
                    //oDa.SelectCommand.CommandText = "GetPriceTable";
                    //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    //oDa.SelectCommand.Connection = new SqlConnection(  );
                    //oDa.SelectCommand.Parameters.Clear();
                    //oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
                    //oDa.SelectCommand.Parameters.AddWithValue("@UserID", userid);
                    //oDa.Fill(dsPriceTable, "Price");
                    dsPriceTable = objHelperDB.GetProductPriceTable(ProductID, Convert.ToInt32(userid.ToString()));
                    _sPriceTable = new StringBuilder();

                    int TotalCount = 0;
                    int RowCount = 0;
                    string[] P1 = null;
                    string[] P2 = null;
                    if (pricecode == 3)
                    {

                        //    DataRow[] dr = dsPriceTable.Tables["Price"].Select();
                        foreach (DataRow oDr in dsPriceTable.Tables["Price"].Rows)
                        {
                            //      foreach (DataRow oDr in dr)
                            //{
                            P1 = oDr["Price1"].ToString().Split('.');
                            P2 = oDr["Price2"].ToString().Split('.');

                            // comment
                            //if (P1[1].Length >= 4 && P2[1].Length >= 4)
                            //{
                            //    if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                            //        _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.0000}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.0000}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>");
                            //    else
                            //        _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>");
                            //}
                            //else
                            //    _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>");       


                            if (P1[1].Length >= 4 && P2[1].Length >= 4)
                            {
                                if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                                    _sPriceTable.AppendFormat("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.0000}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.0000}</td></tr>", oDr["QTY"], "<span>" + oDr["Price1"] + "</span><meta itemprop=\"price\" content=" + oDr["Price1"] + ">", "<span>" + oDr["Price2"] + "</span><meta itemprop=\"price\" content=" + oDr["Price2"] + ">");
                                else
                                    _sPriceTable.AppendFormat("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], "<span>" + oDr["Price1"] + "</span><meta itemprop=\"price\" content=" + oDr["Price1"] + ">", "<span>" + oDr["Price2"] + "</span><meta itemprop=\"price\" content=" + oDr["Price2"] + ">");
                            }
                            else
                                _sPriceTable.AppendFormat("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], "<span>" + oDr["Price1"] + "</span><meta itemprop=\"price\" content=" + oDr["Price1"] + ">", "<span>" + oDr["Price2"] + "</span><meta itemprop=\"price\" content=" + oDr["Price2"] + ">");

                        }
                    }
                    else
                    {
                        bool bLastRow = false;

                        TotalCount = dsPriceTable.Tables["Price"].Rows.Count;
                        RowCount = 0;
                        //DataRow[] dr = dsPriceTable.Tables["Price"].Select();

                        foreach (DataRow oDr in dsPriceTable.Tables["Price"].Rows)
                        {
                            //  foreach (DataRow oDr in dr)
                            //{
                            RowCount = RowCount + 1;
                            if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))   // check whether it is Last Row
                            {
                                bLastRow = true;
                            }

                            string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                            P1 = oDr["Price1"].ToString().Split('.');
                            P2 = oDr["Price2"].ToString().Split('.');



                            if (P1[1].Length >= 4 && P2[1].Length >= 4)
                            {
                                if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                                {
                                    if (RowCount == 1)
                                        _sPriceTable.AppendFormat("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">$<span>{1:0.0000}</span><meta itemprop=\"price\" content=\"{1:0.0000}\"/><meta itemprop=\"priceCurrency\" content=\"AUD\"/></td><td class=\"{3}\" align=\"center\">$<span>{2:0.0000}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                                    else
                                        _sPriceTable.AppendFormat("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">$<span>{1:0.0000}</span></td><td class=\"{3}\" align=\"center\">$<span>{2:0.0000}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                                }
                                else
                                {
                                    if (RowCount == 1)
                                        _sPriceTable.AppendFormat("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">$<span>{1:0.00}</span><meta itemprop=\"price\" content=\"{1:0.00}\"/><meta itemprop=\"priceCurrency\" content=\"AUD\"/></td><td class=\"{3}\" align=\"center\">$ <span>{2:0.00}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                                    else
                                        _sPriceTable.AppendFormat("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">$<span>{1:0.00}</span></td><td class=\"{3}\" align=\"center\">$ <span>{2:0.00}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                                }
                            }
                            else
                            {
                                if (RowCount == 1)
                                    _sPriceTable.AppendFormat("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">$<span>{1:0.00}</span><meta itemprop=\"price\" content=\"{1:0.00}\"/><meta itemprop=\"priceCurrency\" content=\"AUD\"/></td><td class=\"{3}\" align=\"center\">$<span>{2:0.00}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                                else
                                    _sPriceTable.AppendFormat("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">$<span>{1:0.00}</span></td><td class=\"{3}\" align=\"center\">$<span>{2:0.00}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                            }

                            // comment
                            //if (P1[1].Length >= 4 && P2[1].Length >= 4)
                            //{
                            //    if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                            //        _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>$<span itemprop=\"price\">{1:0.0000}</span></td><td class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>$<span itemprop=\"price\">{2:0.0000}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                            //    else
                            //        _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>$<span itemprop=\"price\">{1:0.00}</span></td><td class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>$ <span itemprop=\"price\">{2:0.00}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                            //}
                            //else
                            //    _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>$<span itemprop=\"price\">{1:0.00}</span></td><td class=\"{3}\" align=\"center\">$<span itemprop=\"price\">{2:0.00}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);




                            //if (P1[1].Length >= 4 && P2[1].Length >= 4)
                            //{
                            //    if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                            //        _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.0000}</td><td class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.0000}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" +oDr["Price1"] .ToString("#,#0.00") +"</span", oDr["Price2"], _color);
                            //    else
                            //        _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                            //}
                            //else
                            //    _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);






                            ////if (P1[1].Length >= 4 && P2[1].Length >= 4)
                            ////{
                            ////    if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                            ////        _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.0000}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.0000}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>");
                            ////    else
                            ////        _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>");
                            ////}
                            ////else
                            ////    _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>");

                            //_sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                            // // _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>", _color);


                            // _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                        }
                    }

                }
            }


            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                _sPriceTable = new StringBuilder();
            }
            return _sPriceTable.ToString();
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT DETAILS BASED ON FAMILY ID AND PRODUCT ID ***/
        /********************************************************************************/

        private void GetProductDetails(int ProductID, int FamilyID, StringTemplate st, HttpContext ctx)
        {
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            //SqlDataAdapter oDa = new SqlDataAdapter("GetProductDetails", oCon);
            //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            //oDa.SelectCommand.Parameters.Clear();
            //oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
            //oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
            DataSet oDs = new DataSet();
            //oDa.Fill(oDs, "PrdDetails");

            DataSet tmp = (DataSet)ctx.Session["FamilyProduct"];
            try
            {
                if (tmp != null)
                {
                    DataRow[] Dr = tmp.Tables[0].Select("ATTRIBUTE_TYPE=1");
                    if (Dr.Length > 0)
                    {
                        //oDs.Tables.Add(Dr.CopyToDataTable());
                        //oDs.Tables[0].TableName = "PrdDetails";



                        //  if (oDs != null & oDs.Tables.Count > 0)
                        {
                            // DataRow[] Familyspec2 = oDs.Tables[0].Select("ATTRIBUTE_ID in (13,4,240,241,2,51,18)");
                            //  DataRow[] dro = oDs.Tables["PrdDetails"].Select();
                            foreach (DataRow oDr in Dr)
                            {
                                //foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
                                //{
                                if (oDr["ATTRIBUTE_NAME"].ToString() != "Long Description" && oDr["ATTRIBUTE_NAME"].ToString() != "PROD_CODE")
                                {
                                    if (!string.IsNullOrEmpty(oDr["STRING_VALUE"].ToString()))
                                    {
                                        ProductDetails oPrdDet = new ProductDetails();
                                        oPrdDet.AttributeID = Convert.ToInt32(oDr["ATTRIBUTE_ID"]);
                                        if (oDr["ATTRIBUTE_NAME"].ToString().ToUpper() == "CODE")
                                        {
                                            oPrdDet.AttributeName = "Order Code";
                                        }
                                        else
                                        {
                                            oPrdDet.AttributeName = oDr["ATTRIBUTE_NAME"].ToString();
                                            oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
                                            st.SetAttribute("TBT_PRODDETAILS", oPrdDet);
                                            if (oDr["ATTRIBUTE_NAME"].ToString().ToUpper() == "MANUFACTURER")
                                            {
                                                brandname = oDr["STRING_VALUE"].ToString();
                                            }
                                        }
                                    }
                                }
                            }


                        }
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DESCRIPTION DETAILS OF PRODUCTS ***/
        /********************************************************************************/
        private void GetProductDesc(int ProductID, int FamilyID, StringTemplate st)
        {
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            //SqlDataAdapter oDa = new SqlDataAdapter("GetFamilyDetails", oCon);
            //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            //oDa.SelectCommand.Parameters.Clear();
            //oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
            DataSet oDs = new DataSet();

            DataSet tmp = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            try
            {
                if (tmp != null)
                {
                    DataRow[] Dr = tmp.Tables[0].Select("ATTRIBUTE_TYPE=7");
                    if (Dr.Length > 0)
                    {
                        //oDs.Tables.Add(Dr.CopyToDataTable());
                        //oDs.Tables[0].TableName = "PrdDetails";

                        // if (oDs != null & oDs.Tables.Count > 0)
                        {
                            // DataRow[] Familyspec2 = oDs.Tables[0].Select("ATTRIBUTE_ID in (13,4,240,241,2,51,18)");
                            //DataRow[] drproddet = oDs.Tables["PrdDetails"].Select();
                            //foreach (DataRow oDr in drproddet)
                            //{

                            foreach (DataRow oDr in Dr)
                            {
                                ProductDetails oPrdDet = new ProductDetails();
                                oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
                                //string attr = "TBT_" + oPrdDet.AttributeName;
                                st.SetAttribute("TBT_PRODDESC", oPrdDet);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }



            //oDa.Fill(oDs, "PrdDetails");
            //if (oDs != null && oDs.Tables["PrdDetails"].Rows.Count > 0 && oDs.Tables.Count > 0)
            //{
            //    StringBuilder sBuilder = new StringBuilder();
            //    DataRow[] Familyspec2 = oDs.Tables[0].Select("ATTRIBUTE_ID in (13,4,240,241,2,51,18)");
            //    int[] AttributeIdList = new int[7];
            //    AttributeIdList[0] = 13;
            //    AttributeIdList[1] = 4;
            //    AttributeIdList[2] = 240;
            //    AttributeIdList[3] = 241;
            //    AttributeIdList[4] = 2;
            //    AttributeIdList[5] = 51;
            //    AttributeIdList[6] = 18;

            //    if (Familyspec2.Length > 0)
            //    {
            //        for (int i = 0; i < AttributeIdList.Length; i++)
            //        {
            //            foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
            //            {
            //                if (oDr["STRING_VALUE"].ToString().Length > 0)
            //                {

            //                    string t = oDr["ATTRIBUTE_ID"].ToString();
            //                    if (oDr["ATTRIBUTE_ID"].ToString() == AttributeIdList[i].ToString())
            //                    {
            //                        ProductDetails oPrdDet = new ProductDetails();
            //                        oPrdDet.AttributeID = Convert.ToInt32(oDr["ATTRIBUTE_ID"]);
            //                        oPrdDet.AttributeName = oDr["ATTRIBUTE_NAME"].ToString();

            //                        oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>");
            //                        //string attr = "TBT_" + oPrdDet.AttributeName;
            //                        st.SetAttribute("TBT_PRODDESC", oPrdDet);
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    //foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
            //    //{
            //    //    ProductDetails oPrdDet = new ProductDetails();
            //    //    oPrdDet.AttributeID = Convert.ToInt32(oDr["ATTRIBUTE_ID"]);
            //    //    oPrdDet.AttributeName = oDr["ATTRIBUTE_NAME"].ToString();

            //    //    oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
            //    //    //string attr = "TBT_" + oPrdDet.AttributeName;
            //    //    st.SetAttribute("TBT_PRODDESC", oPrdDet);
            //    //}
            //}
            //else
            //{
            //    string sql_query = "select string_value from TB_PROD_SPECS where ATTRIBUTE_ID=(select ATTRIBUTE_ID from TB_ATTRIBUTE where ATTRIBUTE_NAME='Description') and product_id=" + ProductID;
            //    SqlDataAdapter oDa1 = new SqlDataAdapter(sql_query, oCon);

            //    DataSet oDs1 = new DataSet();
            //    oDa1.Fill(oDs1, "Productdetails");
            //    if (oDs1 != null)
            //    {
            //        foreach (DataRow oDr in oDs1.Tables["Productdetails"].Rows)
            //        {
            //            ProductDetails oPrdDet = new ProductDetails();
            //            oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
            //            //string attr = "TBT_" + oPrdDet.AttributeName;
            //            st.SetAttribute("TBT_PRODDESC", oPrdDet);
            //        }
            //    }
            //}

        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT IMAGES IN DIFFERENT FORMATS BASED ON PARAMETERS ***/
        /********************************************************************************/

        private void GetMultipleImages(int ProductID, int FamilyID, int SubFamilyId, StringTemplate st)
        {
            ConnectionDB objConnectionDB = new ConnectionDB();
            string strfile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            SqlDataAdapter oDa = new SqlDataAdapter("GetProductImages", objConnectionDB.GetConnection());
            oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            oDa.SelectCommand.Parameters.Clear();
            oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);

            oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);

            oDa.SelectCommand.Parameters.AddWithValue("@SubFamilyId", SubFamilyId);

            DataSet oDs = new DataSet();
            oDa.Fill(oDs, "Images");
            bool firstImg = true;
            bool imgempty = false;
            objConnectionDB.CloseConnection();
            try
            {
                if (oDs != null)
                {
                    //       st.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());

                    //DataRow[] drimages = oDs.Tables["Images"].Select() ;
                    //foreach (DataRow oDr in drimages)
                    // {
                    foreach (DataRow oDr in oDs.Tables["Images"].Rows)
                    {
                        ProductImage oPrd = new ProductImage();

                        if (oDr["Product_id"].ToString() != "0")
                        {
                            // if (File.Exists(string.Format(@"{0}/{1}", strfile, oDr["STRING_VALUE"].ToString().Replace("\\", "/"))))
                            // {
                            if (oDr["STRING_VALUE"].ToString() != "")
                            {
                                oPrd.LargeImage = strprodimg_sepdomain + oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                                string tmpimg = oDr["STRING_VALUE"].ToString().Replace("\\", "/");

                                // if ((oPrd.LargeImage.ToLower().Contains("_th")))
                                if ((tmpimg.ToLower().Contains("_th")))
                                {
                                    //string tmpimg = oDr["STRING_VALUE"].ToString().Replace("\\", "/");

                                    oPrd.LargeImage = strprodimg_sepdomain + objHelperService.SetImageFolderPath(tmpimg, "_th", "_images");
                                    oPrd.Thumpnail = strprodimg_sepdomain + objHelperService.SetImageFolderPath(tmpimg, "_th", "_th50");
                                    oPrd.SmallImage = strprodimg_sepdomain + objHelperService.SetImageFolderPath(tmpimg, "_th", "_th");
                                    oPrd.MediumImage = strprodimg_sepdomain + objHelperService.SetImageFolderPath(tmpimg, "_th", "_images_200");
                                    HttpContext.Current.Session["INSERT_IMAGE_URL"] = oPrd.MediumImage;
                                }
                                else
                                {
                                    // oPrd.LargeImage = strprodimg_sepdomain+oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                                    oPrd.Thumpnail = strprodimg_sepdomain + objHelperService.SetImageFolderPath(tmpimg.ToLower(), "_images", "_th50");
                                    oPrd.SmallImage = strprodimg_sepdomain + objHelperService.SetImageFolderPath(tmpimg.ToLower(), "_images", "_th");
                                    oPrd.MediumImage = strprodimg_sepdomain + objHelperService.SetImageFolderPath(tmpimg.ToLower(), "_images", "_images_200");
                                    HttpContext.Current.Session["INSERT_IMAGE_URL"] = oPrd.MediumImage;
                                }

                                string zoomimg = objHelperService.SetImageFolderPath(tmpimg.ToLower(), "_images", "_images_1024");

                                if (firstImg)
                                {
                                    // st.SetAttribute("TBT_NOIMAGE", true);
                                    //st.SetAttribute("TBT_TWEB_IMAGE12", oPrd.MediumImage);
                                    //st.SetAttribute("TBT_TWEB_IMAGE12_LARGE", oPrd.LargeImage);
                                    st.SetAttribute("TBT_TWEB_IMAGE12", oPrd.LargeImage);
                                    //  st.SetAttribute("TBT_TWEB_IMAGE12_LARGE", oPrd.LargeImage);
                                    //if (File.Exists(strfile + zoomimg))
                                    //{

                                    st.SetAttribute("TBT_TWEB_IMAGE12_LARGE", strprodimg_sepdomain + zoomimg);
                                    oPrd.Image1024 = strprodimg_sepdomain + zoomimg;
                                    //}
                                    //else
                                    //{

                                    //    st.SetAttribute("TBT_TWEB_IMAGE12_LARGE", oPrd.LargeImage);
                                    //    oPrd.Image1024 = oPrd.LargeImage.ToLower();
                                    //}
                                    firstImg = false;
                                    imgempty = true;
                                }
                                //Aded by indu for multiple zoom image
                                else
                                {

                                    oPrd.Image1024 = strprodimg_sepdomain + zoomimg;
                                    //if (File.Exists(strfile + zoomimg))
                                    //{

                                    //    oPrd.Image1024 = strprodimg_sepdomain + zoomimg;
                                    //}
                                    //else
                                    //{
                                    //    oPrd.Image1024 = oPrd.LargeImage.ToLower();
                                    //}
                                }
                                st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                            }
                            // }
                        }
                        else
                        {

                            string youtubeid = "";
                            string[] getid = oDr["STRING_VALUE"].ToString().Split('?');
                            string[] getyouid = getid[0].Split('/');
                            youtubeid = getyouid[getyouid.Length - 1];
                            st.SetAttribute("youtubelink", oDr["STRING_VALUE"].ToString());
                            st.SetAttribute("ISYOUTUBELIKE", true);
                            st.SetAttribute("YouTubeImg", "http://i.ytimg.com/vi/" + youtubeid + "/default.jpg");

                        }

                    }
                    if (imgempty == true)
                        st.SetAttribute("TBT_NOIMAGE", true);
                    else
                        st.SetAttribute("TBT_NOIMAGE", false);
                }
            }

            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FAMILY PAGE PRODUCT IMAGES IN DIFFERENT FORMATS ***/
        /********************************************************************************/
        private void GetFamilyMultipleImages(int FamilyID, StringTemplate st, DataSet oDs)
        {
            string strfile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
            try
            {


                // SqlConnection oCon = new SqlConnection(_DBConnectionString);
                SqlDataAdapter oDa = new SqlDataAdapter("GetFamilyImages", objConnectionDB.GetConnection());
                oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                oDa.SelectCommand.Parameters.Clear();
                oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
                oDa.SelectCommand.Parameters.AddWithValue("@Islink", 1);
                DataSet oDslink = new DataSet();
                oDa.Fill(oDslink, "Link");
                if (oDslink.Tables[0].Rows.Count > 0)
                {
                    string youtubeid = "";
                    string[] getid = oDslink.Tables[0].Rows[0]["STRING_VALUE"].ToString().Split('?');
                    string[] getyouid = getid[0].Split('/');
                    youtubeid = getyouid[getyouid.Length - 1];
                    st.SetAttribute("youtubelink", oDslink.Tables[0].Rows[0]["STRING_VALUE"].ToString());
                    st.SetAttribute("ISYOUTUBELIKE", true);
                    st.SetAttribute("YouTubeImg", "http://i.ytimg.com/vi/" + youtubeid + "/default.jpg");
                }

            }
            catch (Exception ex)

            {

                objErrorHandler.CreateLog(ex.ToString());
            }


            DataTable dt = new DataTable();
            DataRow[] dr;
            // oDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            bool firstImg = true;
            try
            {
                if (oDs != null)
                {
                    dr = oDs.Tables[0].Select("ATTRIBUTE_TYPE=9 And ATTRIBUTE_ID Not in (746, 747)", "ATTRIBUTE_ID");
                    if (dr.Length > 0)
                    {
                        // dt = dr.CopyToDataTable();
                        foreach (DataRow oDr in dr)
                        {
                            ProductImage oPrd = new ProductImage();
                            //if (File.Exists(string.Format(@"{0}/{1}", strfile, oDr["STRING_VALUE"].ToString().Replace("\\", "/"))))
                            // {
                            if (oDr["STRING_VALUE"].ToString() != "")
                            {
                                string tmpimg = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                                oPrd.LargeImage = strprodimg_sepdomain + oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                                oPrd.Thumpnail = strprodimg_sepdomain + objHelperService.SetImageFolderPath(tmpimg.ToLower(), "_Images", "_th50");
                                oPrd.SmallImage = strprodimg_sepdomain + objHelperService.SetImageFolderPath(tmpimg.ToLower(), "_Images", "_th");
                                // oPrd.MediumImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_images_200");
                                oPrd.MediumImage = strprodimg_sepdomain + objHelperService.SetImageFolderPath(tmpimg.ToLower(), "_Images", "_Images");
                                string zoomimg = objHelperService.SetImageFolderPath(tmpimg.ToLower(), "_images", "_images_1024");


                                //oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                                //oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                                //oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");

                                if (firstImg)
                                {
                                    st.SetAttribute("TBT_TFWEB_IMAGE1", oPrd.MediumImage);
                                    st.SetAttribute("TBT_TFWEB_LIMAGE", oPrd.LargeImage);

                                    if (File.Exists(strfile + zoomimg))
                                    {
                                        st.SetAttribute("TBT_TFWEB_ZOOM", strprodimg_sepdomain + zoomimg);
                                        oPrd.Image1024 = strprodimg_sepdomain + zoomimg;
                                    }
                                    else
                                    {
                                        st.SetAttribute("TBT_TFWEB_ZOOM", oPrd.LargeImage);
                                        oPrd.Image1024 = oPrd.LargeImage;
                                    }
                                    firstImg = false;
                                }
                                //Aded by indu for multiple zoom image
                                else
                                {

                                    oPrd.Image1024 = strprodimg_sepdomain + zoomimg;
                                    //if (File.Exists(strfile + zoomimg))
                                    //{

                                    //    oPrd.Image1024 = strprodimg_sepdomain + zoomimg;
                                    //}
                                    //else
                                    //{
                                    //    oPrd.Image1024 = oPrd.LargeImage.ToLower();
                                    //}
                                }
                                st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                            }
                            // }
                        }

                    }
                    else
                    {
                        // string tempstr = "";
                        // string tempstr1 = "";
                        // string[] tempstrs = null;
                        dr = oDs.Tables[0].Select("ATTRIBUTE_TYPE=9 And ATTRIBUTE_ID in (746) And STRING_VALUE<>'noimage.gif'", "ATTRIBUTE_ID");
                        if (dr.Length > 0)
                        {
                            // dt = dr.CopyToDataTable();
                            foreach (DataRow oDr in dr)
                            {

                                ProductImage oPrd = new ProductImage();
                                // if (File.Exists(string.Format(@"{0}/{1}", strfile, oDr["STRING_VALUE"].ToString().Replace("\\", "/"))))
                                // {
                                if (oDr["STRING_VALUE"].ToString() != "")
                                {
                                    string img = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                                    oPrd.LargeImage = objHelperService.SetImageFolderPath(img, "_th", "_images");

                                    oPrd.Thumpnail = objHelperService.SetImageFolderPath(img, "_th", "_th50");
                                    oPrd.SmallImage = objHelperService.SetImageFolderPath(img, "_th", "_th");
                                    oPrd.MediumImage = objHelperService.SetImageFolderPath(img, "_Th", "_images_200");
                                    string ZoomImage = objHelperService.SetImageFolderPath(img, "_Th", "_Images_1024");

                                    //oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                                    //oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                                    //oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");
                                    st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                                    if (firstImg)
                                    {
                                        st.SetAttribute("TBT_TFWEB_IMAGE1", oPrd.MediumImage);
                                        st.SetAttribute("TBT_TFWEB_LIMAGE", oPrd.LargeImage);
                                        st.SetAttribute("TBT_TFWEB_ZOOM", ZoomImage);
                                        firstImg = false;
                                    }
                                }
                                // }

                            }

                        }
                    }

                }
                if (firstImg == false)
                    st.SetAttribute("TBT_NOIMAGE", true);
                else
                    st.SetAttribute("TBT_NOIMAGE", false);



            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }

        }



        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER AN URL ALREADY EXIST OR NOT  ***/
        /********************************************************************************/
        private static bool UrlExists(string url)
        {
            bool retval = false;
            try
            {
                new System.Net.WebClient().DownloadData(url);
                retval = true;
            }
            catch (System.Net.WebException)
            {


            }
            return retval;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO BUILD ROW WISE TEMPLATE RECORDS   ***/
        /********************************************************************************/
        private void BuildRecordsTemplateRow()
        {

            try
            {
                TBWDataList[] lstrecords = new TBWDataList[0];
                string package_name = _Package.ToString();
                //Build the cell inner body of the HTML
                _stg_records = new StringTemplateGroup(_skin_records, _SkinRootPath);
                DataSet dsrecords = null;
                string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
                if (_GDataSet != null && _skin_sql_records.Length == 0)
                {
                    dsrecords = _GDataSet;
                }
                else if (package_name.Equals("TOP") || package_name.Equals("TOPLOG"))
                {
                    //DataSet tmpds=EasyAsk.GetCategoryAndBrand("MainCategory") ;
                    DataSet tmpds = (DataSet)HttpContext.Current.Application["key_MainCategory"];
                    if (tmpds != null)
                    { // remove WES NEWS MENU 
                        DataRow[] dr = tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'", "CATEGORY_NAME");
                        //TO ADD WESNEWS
                        //DataRow[] dr=tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");
                        if (dr.Length > 0)
                        {
                            dsrecords = new DataSet();
                            dsrecords.Tables.Add(dr.CopyToDataTable().Copy());
                        }
                    }
                    else
                        dsrecords = null;

                }

                else if (package_name.Equals("BOTTOM"))
                {
                    // DataSet tmpds = EasyAsk.GetCategoryAndBrand("MainCategory");
                    DataSet tmpds = (DataSet)HttpContext.Current.Application["key_MainCategory"];
                    if (tmpds != null)
                    { // remove WES NEWS MENU 
                        //DataRow[] dr = tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'", "CATEGORY_NAME");
                        //TO  ADD WESNEWS IN MENU
                        DataRow[] dr = tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");

                        if (dr.Length > 0)
                        {
                            dsrecords = new DataSet();
                            dsrecords.Tables.Add(dr.CopyToDataTable().Copy());
                        }
                    }
                    else
                        dsrecords = null;

                }

                else if (package_name.Equals("BROWSEBYCATEGORY") || package_name.Equals("BROWSEBYBRAND") || package_name.Equals("BROWSEBYPRODUCT")) // unwanted db calls
                {
                    dsrecords = null;
                }
                else
                {
                    dsrecords = GetDataSet(_skin_sql_records, _skin_sql_type_records, _skin_sql_param_records);
                }
                _stg_container = new StringTemplateGroup(_skin_container, _SkinRootPath);

                if (dsrecords != null)
                {//dsrecords.Tables.Count - 1
                    if (dsrecords.Tables[0].Rows.Count > 0)
                    {

                        lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count];
                        int ictrecords = 0, ictcol = 1; string strValue = string.Empty;
                        //DataRow[] drecords = dsrecords.Tables[0].Select() ;


                        foreach (DataRow dr in dsrecords.Tables[0].Rows)
                        {
                            if (package_name.Equals("CATEGORYLISTIMG"))
                            {
                                if (HttpContext.Current.Request.QueryString["tsb"] == null && HttpContext.Current.Request.QueryString["cid"] != null && !HttpContext.Current.Request.QueryString["cid"].Equals(""))
                                    _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records.ToString() + "1");
                                else
                                    _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records);
                            }
                            else

                                _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records);


                            _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());
                            if (HttpContext.Current.Request.QueryString["cid"] != null)
                            {
                                _stmpl_records.SetAttribute("TBT_CAT_ID", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["cid"].ToString()));
                            }
                            if (HttpContext.Current.Request.QueryString["tsb"] != null)
                            {
                                _stmpl_records.SetAttribute("TBT_TOSUITE_BRAND", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()));
                                _stmpl_records.SetAttribute("TBT_TOSUITE_BRAND1", HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()));
                            }
                            if (package_name.Equals("CATEGORYLISTIMG"))
                            {
                                if (HttpContext.Current.Session["EA"] != null)
                                {
                                    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
                                }
                            }
                            if (HttpContext.Current.Request.QueryString["sl1"] != null)
                                _stmpl_records.SetAttribute("TBT_TOSUITE_SL1", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["sl1"].ToString()));
                            if (HttpContext.Current.Request.QueryString["sl2"] != null)
                                _stmpl_records.SetAttribute("TBT_TOSUITE_SL2", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["sl2"].ToString()));

                            foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                            {
                                //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());                                                       
                                // objErrorHandler.CreateLog(dsrecords.Tables[0].Columns.Count + "colcount");
                                string dc_columnname = dc.ColumnName.ToString();

                                if ((package_name.Equals("TOP") || package_name.Equals("TOPLOG")) && dc_columnname.ToUpper().Equals("CATEGORY_NAME") && dr[dc_columnname].ToString().Length > 8)
                                {
                                    string sttr = dr[dc_columnname].ToString();
                                    int indx = sttr.IndexOf(" ", 7);
                                    if (indx >= 7)
                                        sttr = sttr.Substring(0, indx) + "<br/>" + sttr.Substring(indx + 1);
                                    else if (sttr.Equals("VCR COMPONENTS"))
                                        sttr = "VCR<br/>COMPONENTS";
                                    _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), sttr);
                                }
                                else
                                {
                                    if (dc_columnname.ToUpper().Equals("IMAGE_FILE"))
                                    {
                                        string imgfile = dr[dc_columnname].ToString().ToLower().Replace("_th", "_images_200");


                                        _stmpl_records.SetAttribute("CDNROOT", strFile);


                                        //FileInfo Fil = new FileInfo(strFile + imgfile);
                                        //if (Fil.Exists)
                                        //{
                                        //    _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), imgfile);
                                        //}
                                        //else
                                        //{
                                        //    _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "/images/noimage.gif");
                                        //}
                                        _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), imgfile);

                                        // objErrorHandler.CreateLog(strFile +"imggile"+ imgfile + "TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper());


                                        //FileInfo Fil = new FileInfo(strFile + dr[dc_columnname].ToString());
                                        //if (Fil.Exists)
                                        //{
                                        //    _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc_columnname].ToString());
                                        //}
                                        //else
                                        //{
                                        //    _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "/images/noimage.gif");
                                        //}
                                    }
                                    else
                                    {
                                        if (dc_columnname.ToUpper().Equals("CUSTOM_NUM_FIELD3") && dr[dc_columnname].ToString().Equals("2"))
                                        {
                                            _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc_columnname].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));// + "&bypcat=1");
                                        }
                                        else if (dc_columnname.ToUpper().ToString() == "EA_PATH" && (package_name.Equals("TOP") || package_name.Equals("TOPLOG")))
                                        {
                                            _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["EA_PATH"].ToString())));
                                        }
                                        else
                                        {
                                            if (dc_columnname.ToUpper().Equals("TOSUITE_MODEL") || dc_columnname.ToUpper().Equals("TOSUITE_BRAND"))
                                            {
                                                _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), HttpUtility.UrlEncode(dr[dc_columnname].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")));
                                                _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper() + "1", dr[dc_columnname].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));

                                                string parentcat = string.Empty;
                                                if (package_name.Equals("CATEGORYLISTIMG"))
                                                {
                                                    _stmpl_records.GetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper() + "1");
                                                    string[] parentcategory = HttpContext.Current.Session["EA"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                                    if (parentcategory.Length >= 3)
                                                    {
                                                        parentcat = parentcategory[2];
                                                        if (parentcat == "")
                                                        {
                                                            if (HttpContext.Current.Request.QueryString["cid"] != null)
                                                            {
                                                                DataTable dt1 = (DataTable)HttpContext.Current.Session["dtcid"];

                                                                DataRow[] foundRows;
                                                                string cid = HttpContext.Current.Request.QueryString["cid"].ToString();
                                                                foundRows = dt1.Select("cid='" + cid + "' ");
                                                                if (foundRows.Length > 0)
                                                                {
                                                                    parentcat = foundRows[0][1].ToString();

                                                                }
                                                            }


                                                        }
                                                    }
                                                    string model = _stmpl_records.GetAttribute("TBT_TOSUITE_MODEL1").ToString();
                                                    // objHelperService.Cons_NewURl(_stmpl_records, "//// ////" + parentcat + "////" + HttpContext.Current.Request.QueryString["tsb"] + "////" +model , "bb.aspx", true, "");
                                                    objHelperService.SimpleURL(_stmpl_records, "//// ////" + parentcat + "////" + HttpContext.Current.Request.QueryString["tsb"] + "////" + model, "bb.aspx");
                                                    //objErrorHandler.CreateLog("no of rows:" + dsrecords.Tables[0].Rows.Count);
                                                    //objErrorHandler.CreateLog("modelname--" + dc_columnname +dr[dc_columnname].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                                    //objErrorHandler.CreateLog(dr[0]+"--"+dr[1]+"--"+dr[2]+"--"+dr[3]);
                                                    //objErrorHandler.CreateLog("model:" + dr[dc_columnname].ToString());
                                                }
                                            }
                                            else
                                            {
                                                _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc_columnname].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                            }
                                        }

                                    }
                                }

                            }
                            if (package_name.Equals("CSFAMILYPAGE"))
                            {
                                _stmpl_records.SetAttribute("TBT_FAMILYIMAGE", "My fl");
                            }
                            //Mofified by:Indu.For URLRewrite for PRODUCT CATEGORIES
                            if (package_name.Equals("TOP") || package_name.Equals("TOPLOG") || package_name.Equals("BOTTOM"))
                            {
                                if (HttpContext.Current.Request.Url.Segments[1].ToString().ToUpper().Equals("/Home.aspx") || HttpContext.Current.Session["URLINIHOME"] == null || HttpContext.Current.Request.Url.Segments[1].ToString().ToUpper().Equals("/Login.aspx"))
                                {
                                    //objHelperService.Cons_NewURl(_stmpl_records, dr["EA_PATH"].ToString(), "ct.aspx", true, "CATEGORY");
                                    // objHelperService.Cons_NewURl(_stmpl_records,"//// ////"+  dr["Category_name"].ToString(), "ct.aspx", true, "CATEGORY");
                                    objHelperService.SimpleURL(_stmpl_records, "//// ////" + dr["Category_name"].ToString(), "ct.aspx");
                                }
                                else
                                {
                                    objHelperService.SimpleURL(_stmpl_records, "//// ////" + dr["Category_name"].ToString(), "ct.aspx");
                                    //objHelperService.Cons_NewURl(_stmpl_records, dr["EA_PATH"].ToString(), "ct.aspx", false, "CATEGORY");
                                }
                                //    if (dr["CATEGORY_ID"].ToString() != WesNewsCategoryId)
                                //{
                                //    objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["Category_name"].ToString().ToLower());

                                //    //objHelperService.Cons_NewURl(_stmpl_records, dr["EA_PATH"].ToString(), "ct.aspx", true, "CATEGORY");
                                //}

                                //else
                                //{
                                //    objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), "New Products");
                                //}
                            }

                            if (package_name.Equals("TOP") || package_name.Equals("TOPLOG"))
                            {
                                if (dr["CATEGORY_ID"].ToString() != WesNewsCategoryId)
                                {
                                    strValue = strValue + _stmpl_records.ToString();

                                }

                            }
                            else
                            {
                                strValue = strValue + _stmpl_records.ToString();

                            }
                            if (ictrecords == 10 && package_name.Equals("TOP"))
                            {
                                // strValue += "</ul></div></td><tr><td><div id=\"navcontainer\">";
                            }
                            if (ictcol.Equals(_grid_cols))
                            {
                                _stmpl_container = _stg_container.GetInstanceOf(package_name + "\\" + _skin_container);
                                _stmpl_container.SetAttribute("TBWDataList", strValue);
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                                ictrecords++; ictcol = 1; strValue = "";
                            }
                            else
                            {
                                ictcol++;
                            }
                        }
                        if (!strValue.Equals(""))
                        {
                            _stmpl_container = _stg_container.GetInstanceOf(package_name + "\\" + _skin_container);
                            _stmpl_container.SetAttribute("TBWDataList", strValue);
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                            ictrecords++; ictcol = 1; strValue = "";
                        }
                    }
                }
                _dict_inner_html[_skin_body_attribute] = lstrecords;
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
            }
        }

        //private DataSet GetDataSet(string SQLQuery)
        //{
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, _DBConnectionString);
        //    da.Fill(ds, "generictable");
        //    return ds;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE DATASET DETAILS BASED ON PARAMETERS  ***/
        /********************************************************************************/


        public string ST_Top_Cart_item()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string sHTML = "";

            try
            {

                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_container1 = null;
                StringTemplate _stmpl_records1 = null;

                TBWDataList[] lstrecords = new TBWDataList[0];
                // TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                // TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                DataSet dsrecords = new DataSet();
                DataSet dsCart = new DataSet();
                DataSet dsSubCat = new DataSet();
                DataRow[] drs = null;

                String sessionId_dum;
                // string sessionuserid_dum;
                sessionId_dum = HttpContext.Current.Session.SessionID.ToString();

                string userid = HttpContext.Current.Session["USER_ID"].ToString();

                if (userid == string.Empty)
                {
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
                }


                string _Tbt_Order_Id = "";
                string _Tbt_Ship_URL = "";
                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                {
                    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                }
                else
                {
                    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(userid), OpenOrdStatusID).ToString();

                }
                string OrderStatus = objOrderServices.GetOrderStatus(objHelperServices.CI(_Tbt_Order_Id));
                if (objHelperServices.CI(_Tbt_Order_Id) > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())    // || OrderStatus=="CAU_PENDING")
                {

                    dsCart = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_CART_ITEM_BIGTOP " + _Tbt_Order_Id + ",'" + sessionId_dum + "',''");
                }
                else
                {
                    dsCart = null;
                }
                //  GetSupplierDetail();

                int ictrows = 0;






                _stg_records = new StringTemplateGroup("count", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("cell", _SkinRootPath);






                int ictrecords = 0;
                int UlchangeCount = 0;

                string Itemslist = "";
                string newurl = "";
                //string _BCEAPath = GetEAPath();


                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                {
                    // _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                    _Tbt_Ship_URL = "/orderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + _Tbt_Order_Id;

                }
                else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
                {
                    _Tbt_Ship_URL = "/orderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + _Tbt_Order_Id;
                    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                }
                //else
                //{
                //    _Tbt_Ship_URL = "shipping.aspx";
                //}

                if (dsCart == null || dsCart.Tables.Count <= 0 || dsCart.Tables[0].Rows.Count <= 0)
                {
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");


                    _stmpl_container.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);

                    _stmpl_container.SetAttribute("SHOW_CART", false);
                    _stmpl_container.SetAttribute("TBWDataList", "");


                    _stmpl_records1 = _stg_records1.GetInstanceOf(_Package + "\\" + "cartCount");
                    _stmpl_records1.SetAttribute("Cart_Redirect", "/orderDetails.aspx?CartItem=0");
                    _stmpl_records1.SetAttribute("CART_AMOUNT", "0.00");
                    //_stmpl_records1.SetAttribute("CART_COUNT", "0.00");
                    _stmpl_records1.SetAttribute("CART_COUNT", "0");
                    _stmpl_records1.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);

                    sHTML = _stmpl_records1.ToString() + "~" + _stmpl_container.ToString() + "~" + "0" + "~" + _Tbt_Order_Id + "~" + EncryptSP(_Tbt_Order_Id.ToString());
                    sHTML = sHTML + "~" + ST_Top_Cart_itemMobile(dsCart, _Tbt_Order_Id);

                    sHTML = sHTML.Replace("data-toggle=\"dropdown\"", "");

                }
                else
                {
                    lstrecords = new TBWDataList[dsCart.Tables[0].Rows.Count + 1];
                    foreach (DataRow dr in dsCart.Tables[0].Rows)//For Records
                    {

                        ictrows = ictrows + 1;
                        _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "cell");

                        string category = dr["Category_Path"].ToString();

                        // category = strsupplierName;

                        DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, dr["product_id"].ToString(), "", "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID_BIGTOP", HelperDB.ReturnType.RTDataSet);
                        if (tmpds != null && tmpds.Tables.Count > 0)
                        {
                            string[] catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                            string familyid = tmpds.Tables[0].Rows[0]["parent_FAMILY_ID"].ToString();

                            if (familyid == "0")
                            {
                                familyid = tmpds.Tables[0].Rows[0]["FAMILY_ID"].ToString();
                            }
                            //newurl = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + tmpds.Tables[0].Rows[0]["FAMILY_name"].ToString() + "////" + tmpds.Tables[0].Rows[0]["product_ID"].ToString() + "=" + dr["Code"].ToString() + "////" + tmpds.Tables[0].Rows[0]["FAMILY_ID"].ToString(), "pd.aspx", true);
                            newurl = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["FAMILY_ID"].ToString() + "////" + tmpds.Tables[0].Rows[0]["product_ID"].ToString() + "=" + dr["Code"].ToString() + "////" + (catpath.Length >= 3 ? catpath[2] : " ") + (catpath.Length >= 4 ? "////" + catpath[3] : " ") + (catpath.Length >= 5 ? "////" + catpath[4] : " ") + (catpath.Length >= 6 ? "////" + catpath[5] : " ") + "////" + tmpds.Tables[0].Rows[0]["FAMILY_name"].ToString(), "pd.aspx", true);
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", newurl + "/pd/");
                        }

                        // _stmpl_records.SetAttribute("TBT_REWRITEURL", objHelperServices.Cons_NewURlASPX("", _BCEAPath + "////" + dr["CATEGORY_NAME"].ToString(), "mpl.aspx", true, "Category"));
                        _stmpl_records.SetAttribute("FAMILY_NAME", dr["Family_name"].ToString());
                        _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["Family_name"].ToString().Replace('"', ' '));
                        string imgpath = GetImagePath(dr["TWEB Image1"].ToString());
                        _stmpl_records.SetAttribute("TWebImage1", imgpath);


                        _stmpl_records.SetAttribute("COST", dr["Cost"]);
                        _stmpl_records.SetAttribute("CODE", dr["Code"]);





                        Itemslist = Itemslist + _stmpl_records.ToString();

                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;

                        // objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                    }

                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");

                    decimal ORDER_AMOUNT = Convert.ToDecimal(dsCart.Tables[0].Rows[0]["ORDER_AMOUNT"]);

                    if (objOrderServices.IsNativeCountry_Express(Convert.ToInt32(_Tbt_Order_Id)) == 0 && userid != "777")
                    {
                        decimal tax_amt = objOrderServices.GetTaxAmount(Convert.ToInt32(_Tbt_Order_Id));
                        ORDER_AMOUNT = ORDER_AMOUNT - tax_amt;
                    }

                    _stmpl_container.SetAttribute("CART_AMOUNT", ORDER_AMOUNT);
                    //modified by:indu
                    //_stmpl_container.SetAttribute("CART_AMOUNT", dsCart.Tables[0].Rows[0]["cost"].ToString());
                    _stmpl_container.SetAttribute("CART_COUNT", dsCart.Tables[0].Rows[0]["ITEM_COUNT"]);
                    _stmpl_container.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);

                    _stmpl_container.SetAttribute("SHOW_CART", true);
                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);

                    _stmpl_container.SetAttribute("Cart_Redirect", _Tbt_Ship_URL);
                    _stmpl_records1 = _stg_records1.GetInstanceOf(_Package + "\\" + "cartCount");

                    _stmpl_records1.SetAttribute("CART_AMOUNT", ORDER_AMOUNT);
                    _stmpl_records1.SetAttribute("CART_COUNT", dsCart.Tables[0].Rows[0]["ITEM_COUNT"]);

                    _stmpl_records1.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);

                    sHTML = _stmpl_records1.ToString() + "~" + _stmpl_container.ToString() + "~" + dsCart.Tables[0].Rows[0]["ITEM_COUNT"].ToString() + "~" + _Tbt_Order_Id + "~" + EncryptSP(_Tbt_Order_Id.ToString());


                    sHTML = sHTML + "~" + ST_Top_Cart_itemMobile(dsCart, _Tbt_Order_Id);



                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Top_Cart_item = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return sHTML;
            // return ProdimageRreplaceImages(sHTML);
        }
        public string ST_Top_Cart_itemMobile(DataSet dsCart, string _Tbt_Order_Id)
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            string sHTML = "";

            try
            {

                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_container1 = null;
                StringTemplate _stmpl_records1 = null;

                TBWDataList[] lstrecords = new TBWDataList[0];
                // TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                // TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                DataSet dsrecords = new DataSet();

                DataSet dsSubCat = new DataSet();
                DataRow[] drs = null;

                String sessionId_dum;

                string _Tbt_Ship_URL = "";
                string _Tbt_chkout_URL = "";

                int ictrows = 0;


                string userid = HttpContext.Current.Session["USER_ID"].ToString();

                if (userid == string.Empty)
                {
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
                }

                _stg_records = new StringTemplateGroup("count", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("cell", _SkinRootPath);






                int ictrecords = 0;
                int UlchangeCount = 0;

                string Itemslist = "";
                string newurl = "";
                //string _BCEAPath = GetEAPath();
                _Tbt_Ship_URL = "/orderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + _Tbt_Order_Id;
                _Tbt_chkout_URL = "/expressCheckout.aspx?" + EncryptSP(_Tbt_Order_Id.ToString());


                if (dsCart == null || dsCart.Tables.Count <= 0 || dsCart.Tables[0].Rows.Count <= 0)
                {
                    _stmpl_container = _stg_container.GetInstanceOf("CartItemsMobile" + "\\" + "Main");

                    _stmpl_container.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);

                    _stmpl_container.SetAttribute("CHECKOUT", _Tbt_chkout_URL);
                    _stmpl_container.SetAttribute("TBT_IS_CART", false);


                    _stmpl_container.SetAttribute("TBWDataList", "");




                    sHTML = _stmpl_container.ToString();

                }
                else
                {
                    lstrecords = new TBWDataList[dsCart.Tables[0].Rows.Count + 1];
                    foreach (DataRow dr in dsCart.Tables[0].Rows)//For Records
                    {

                        ictrows = ictrows + 1;
                        _stmpl_records = _stg_records.GetInstanceOf("CartItemsMobile" + "\\" + "cell");

                        string category = dr["Category_Path"].ToString();

                        // category = strsupplierName;
                        DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, dr["product_id"].ToString(), "", "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID_BIGTOP", HelperDB.ReturnType.RTDataSet);
                        if (tmpds != null && tmpds.Tables.Count > 0)
                        {

                            string[] catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                            //newurl = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + tmpds.Tables[0].Rows[0]["FAMILY_name"].ToString() + "////"
                            //    + tmpds.Tables[0].Rows[0]["product_ID"].ToString()
                            //    + "=" + dr["Code"].ToString()
                            //  + "////" + tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString()
                            //    , "pd.aspx", true);
                            newurl = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString() + "////" + tmpds.Tables[0].Rows[0]["product_ID"].ToString() + "=" + dr["Code"] + "////" + (catpath.Length >= 3 ? catpath[2] : " ") + (catpath.Length >= 4 ? "////" + catpath[3] : " ") + (catpath.Length >= 5 ? "////" + catpath[4] : " ") + (catpath.Length >= 6 ? "////" + catpath[5] : " ") + "////" + tmpds.Tables[0].Rows[0]["FAMILY_name"].ToString(), "pd.aspx", true);


                            _stmpl_records.SetAttribute("TBT_REWRITEURL", newurl + "/pd/");
                        }
                        //if (dr["Category_Path"].ToString() == "" || dr["Category_Path"].ToString().ToUpper().Contains(category.ToUpper()))
                        //{

                        //    newurl = category + "////" +
                        //             dr["FAMILY_ID"].ToString() +
                        //             "=" + dr["FAMILY_NAME"].ToString() +
                        //             "////" +
                        //              dr["PRODUCT_ID"].ToString() + "=" + dr["CODE"].ToString();

                        //    _stmpl_records.SetAttribute("TBT_REWRITEURL", "/" + objHelperServices.Cons_NewURlASPX("", newurl, "mpd.aspx", true, "") + "/mpd/");

                        //}
                        //else
                        //{
                        //    newurl = dr["Category_Path"].ToString() + "////" +
                        //                dr["FAMILY_ID"].ToString() +
                        //                "=" + dr["FAMILY_NAME"].ToString() +
                        //                "////" +
                        //                 dr["PRODUCT_ID"].ToString() + "=" + dr["CODE"].ToString();
                        //    _stmpl_records.SetAttribute("TBT_REWRITEURL", "/" + objHelperServices.Cons_NewURlASPX("", newurl, "pd.aspx", true, "") + "/pd/");
                        //}

                        // _stmpl_records.SetAttribute("TBT_REWRITEURL", objHelperServices.Cons_NewURlASPX("", _BCEAPath + "////" + dr["CATEGORY_NAME"].ToString(), "mpl.aspx", true, "Category"));
                        _stmpl_records.SetAttribute("FAMILY_NAME", dr["Family_name"].ToString());
                        _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["Family_name"].ToString().Replace('"', ' '));
                        string imgpath = GetImagePath(dr["TWEB Image1"].ToString());
                        _stmpl_records.SetAttribute("TWebImage1", imgpath);


                        _stmpl_records.SetAttribute("COST", dr["Cost"].ToString());
                        _stmpl_records.SetAttribute("CODE", dr["Code"].ToString());





                        Itemslist = Itemslist + _stmpl_records.ToString();

                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;

                        // objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                    }

                    _stmpl_container = _stg_container.GetInstanceOf("CartItemsMobile" + "\\" + "Main");

                    _stmpl_container.SetAttribute("TBT_IS_CART", true);
                    decimal ORDER_AMOUNT = Convert.ToDecimal(dsCart.Tables[0].Rows[0]["ORDER_AMOUNT"]);

                    if (objOrderServices.IsNativeCountry_Express(Convert.ToInt32(_Tbt_Order_Id)) == 0 && userid != "777")
                    {
                        decimal tax_amt = objOrderServices.GetTaxAmount(Convert.ToInt32(_Tbt_Order_Id));
                        ORDER_AMOUNT = ORDER_AMOUNT - tax_amt;
                    }

                    _stmpl_container.SetAttribute("CART_AMOUNT", ORDER_AMOUNT);
                    //modified by:indu
                    //_stmpl_container.SetAttribute("CART_AMOUNT", dsCart.Tables[0].Rows[0]["cost"].ToString());
                    _stmpl_container.SetAttribute("CART_COUNT", dsCart.Tables[0].Rows[0]["ITEM_COUNT"].ToString());
                    _stmpl_container.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);
                    _stmpl_container.SetAttribute("CHECKOUT", _Tbt_chkout_URL);

                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);




                    sHTML = _stmpl_container.ToString();


                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            //sw.Stop();
            //objErrorHandler.ExeTimelog = "ST_Top_Cart item mobile = " + sw.Elapsed.TotalSeconds.ToString();
            //objErrorHandler.createexecutiontmielog();
            return sHTML;
            // return ProdimageRreplaceImages(sHTML);
        }
        protected string EncryptSP(string ordid)
        {
            string enc = "";
            enc = objSecurity.StringEnCrypt(ordid, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            return HttpUtility.UrlEncode(enc);
        }


        private DataSet GetDataSet(string SQLQuery, string SQLType, string SQLParam)
        {
            DataSet ds = new DataSet();
            try
            {
                if (paraValue != "" && SQLParam != "")
                    SQLQuery = SQLQuery.Replace(SQLParam, paraValue);
                if (_Package.Equals("BROWSEBYCATEGORY") && HttpContext.Current.Session["PARAFILTER"].Equals("Value"))
                {
                    if (SQLQuery.Contains("order by tc.category_id"))
                        SQLQuery = SQLQuery.Replace("order by tc.category_id", " and tC.CATEGORY_ID in (select distinct category_id from tb_family where family_id in (select family_id from tb_prod_family where product_id in  (select product_id from TBWC_SEARCH_PROD_LIST where user_session_id = '" + HttpContext.Current.Session.SessionID + "')))" + " order by tc.category_id");
                }
                ConnectionDB objConnectionDB = new ConnectionDB();
                SqlDataAdapter da = new SqlDataAdapter(SQLQuery, objConnectionDB.GetConnection());
                da.Fill(ds, "generictable");
                objConnectionDB.CloseConnection();
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return ds;
        }
        //private DataSet ProductFilterFlatTable(DataSet flatDataset)
        //{
        //    {
        //        StringBuilder SQLstring = new StringBuilder();
        //        DataSet oDsProductFilter = new DataSet();
        //        string SQLString = " SELECT PRODUCT_FILTERS FROM TB_CATALOG WHERE  CATALOG_ID = " + CATALOG_ID + " ";
        //        SqlDataAdapter da = new SqlDataAdapter(SQLString, _DBConnectionString);
        //        da.Fill(oDsProductFilter);
        //        string sProductFilter = string.Empty;
        //        if (oDsProductFilter.Tables[0].Rows.Count > 0 && oDsProductFilter.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
        //        {
        //            sProductFilter = oDsProductFilter.Tables[0].Rows[0].ItemArray[0].ToString();
        //            XmlDocument xmlDOc = new XmlDocument();
        //            xmlDOc.LoadXml(sProductFilter);
        //            XmlNode rNode = xmlDOc.DocumentElement;

        //            if (rNode.ChildNodes.Count > 0)
        //            {
        //                for (int i = 0; i < rNode.ChildNodes.Count; i++)
        //                {
        //                    XmlNode TableDataSetNode = rNode.ChildNodes[i];

        //                    if (TableDataSetNode.HasChildNodes)
        //                    {
        //                        if (TableDataSetNode.ChildNodes[2].InnerText == " ")
        //                        {
        //                            TableDataSetNode.ChildNodes[2].InnerText = "=";
        //                        }
        //                        if (TableDataSetNode.ChildNodes[0].InnerText == " ")
        //                        {
        //                            TableDataSetNode.ChildNodes[0].InnerText = "0";
        //                        }
        //                        string stringval = TableDataSetNode.ChildNodes[3].InnerText.Replace("'", "''");
        //                        DataSet attribuetypeDS = new DataSet();
        //                        string sSQLString = " SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE  ATTRIBUTE_ID = " + Convert.ToInt32(TableDataSetNode.ChildNodes[0].InnerText) + " ";
        //                        SqlDataAdapter das = new SqlDataAdapter(sSQLString, _DBConnectionString);
        //                        das.Fill(attribuetypeDS);
        //                        if (attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("TEX") == true || attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("DATE") == true)
        //                        {

        //                            if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
        //                            }
        //                            else
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
        //                            }
        //                        }
        //                        else if (attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("DECI") == true || attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("NUM") == true)
        //                        {
        //                            if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE  (NUMERIC_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
        //                            }
        //                            else
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (NUMERIC_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
        //                            }
        //                        }


        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "NONE")
        //                    {
        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "AND")
        //                    {
        //                        SQLstring.Append(" INTERSECT \n");
        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "OR")
        //                    {
        //                        SQLstring.Append(" UNION \n");
        //                    }

        //                }

        //            }

        //        }
        //        string productFiltersql = SQLstring.ToString();
        //        // Boolean variableFilter = false;
        //        if (productFiltersql.Length > 0)
        //        {
        //            string s = "SELECT PRODUCT_ID FROM [PRODUCT FAMILY](" + CATALOG_ID + ") WHERE CATALOG_ID=" + CATALOG_ID + " AND PRODUCT_ID IN\n" +
        //                  "(\n";// +
        //            //"SELECT DISTINCT PRODUCT_ID\n" +
        //            //"FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") \n" +
        //            //"WHERE\n";
        //            productFiltersql = s + productFiltersql + "\n)";
        //            SqlDataAdapter dad = new SqlDataAdapter(productFiltersql, _DBConnectionString);
        //            dad.Fill(oDsProductFilter);

        //            bool available = false;

        //            for (int rowCount = 0; rowCount < flatDataset.Tables[0].Rows.Count; rowCount++)
        //            {//foreach (DataRow odr in flatDataset.Tables[0].Rows)
        //                DataRow odr = flatDataset.Tables[0].Rows[rowCount];
        //                available = false;
        //                foreach (DataRow dr in oDsProductFilter.Tables[0].Rows)
        //                {
        //                    if (dr["PRODUCT_ID"].ToString() == odr["PRODUCT_ID"].ToString())
        //                    {
        //                        available = true;
        //                    }

        //                }
        //                if (available == false)
        //                {
        //                    string cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE PRODUCT_ID = " + odr["PRODUCT_ID"].ToString() + " AND USER_SESSION_ID='" + HttpContext.Current.Session.SessionID + "'";
        //                    SqlConnection _SQLConn = new SqlConnection(_DBConnectionString);
        //                    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
        //                    pscmd.CommandType = CommandType.Text;
        //                    int valr = pscmd.ExecuteNonQuery();
        //                    odr.Delete();
        //                    flatDataset.AcceptChanges();
        //                    rowCount--;
        //                }

        //            }

        //        }
        //    }
        //    return flatDataset;
        //}

        //private bool IsPDFAttached()
        //{
        //    bool retvalue = false;

        //    if (paraCID != null && !string.IsNullOrEmpty(paraCID.Trim()))
        //    {
        //        string sSQL = "SELECT IMAGE_FILE2 FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'";
        //        objHelperService.SQLString = sSQL;


        //    }

        //    return retvalue;
        //}
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK ECOM  IS ENABLED OR NOT  ***/
        /********************************************************************************/
        private bool IsEcomenabled()
        {

            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if (userid.Equals(string.Empty))
                userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
            return objHelperService.GetIsEcomEnabled(userid);
        }

        public void ST_Product_Download(string _pid)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string rtnstr = string.Empty;
            StringTemplateGroup _stg_container = null;
            //  StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            //  StringTemplate _stmpl_records = null;
            // StringTemplate _stmpl_records1 = null;
            // StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

            // StringTemplateGroup _stg_container1 = null;
            // StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];



            DataTable dt = new DataTable();
            // DataRow[] dr = null;

            int ictrecords = 0;
            downloadST = "";
            isdownload = false;
            if (_pid != "")
            {

                DataSet TempEADs = objFamilyServices.GetFamilyPageProduct(_pid, "PRODUCT_ATTACHMENT");
                if (TempEADs != null && TempEADs.Tables.Count > 0 && TempEADs.Tables[0].Rows.Count > 0)
                {
                    //TempEADs.Tables[0].Columns.Add("FAMILY_NAME");




                    _stg_container = new StringTemplateGroup("main", SkinRootPath);
                    lstrecords = new TBWDataList[1];
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DownloadMain");


                    rtnstr = ST_Productpage_Download(TempEADs.Tables[0]);
                    if (rtnstr != "")
                    {
                        lstrecords[ictrecords] = new TBWDataList(rtnstr.ToString());
                        ictrecords = ictrecords + 1;
                    }



                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    if (ictrecords > 0)
                    {

                        downloadST = _stmpl_container.ToString();
                        isdownload = true;

                    }




                }
            }

            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Product_Download = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();


        }

        public string ST_Productpage_Download(DataTable Adt)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;

            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];


            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];


            string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
            string strImgFiles1 = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
            long FileInKB;
            string[] file = null;
            string strfile = string.Empty;
            if (Adt != null && Adt.Rows.Count > 0)
            {




                DataSet dscat = new DataSet();


                try
                {
                    _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
                    _stg_container = new StringTemplateGroup("row", _SkinRootPath);


                    lstrecords = new TBWDataList[Adt.Rows.Count + 1];



                    int ictrecords = 0;

                    foreach (DataRow dr in Adt.Rows)//For Records
                    {
                        strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");


                        FileInfo Fil;


                        string strImgFilesnew = System.Configuration.ConfigurationManager.AppSettings["VirtualPathJPG"].ToString();

                        if (dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains(".jpg") == true)
                            Fil = new FileInfo(strImgFilesnew + dr["PRODUCT_ATT_FILE"].ToString());
                        else
                            Fil = new FileInfo(strPDFFiles1 + dr["PRODUCT_ATT_FILE"].ToString());
                        //Added by :Indu
                        //Not to display wes public files and wes secure files.
                        //Date:3 Oct 2017                     
                        //  objErrorHandler.CreateLog(dr["PRODUCT_ATT_FILE"].ToString() +"pdffilename");


                        if ((Fil.Exists) && (dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains("wes_public_files") == false) && (dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains("wes_secure_files") == false))
                        {
                            //  objErrorHandler.CreateLog(dr["PRODUCT_ATT_FILE"].ToString() + "Inside pdffilename");
                            _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "DownloadCell");

                            strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");
                            strfile = strfile.Replace(@"\", "/");

                            file = strfile.Split(new string[] { @"/" }, StringSplitOptions.None);
                            if (file.Length > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_ATTACH_FILE_NAME", file[file.Length - 1].ToString());
                                if (file[file.Length - 1].ToString().ToLower().Contains(".jpg") == true)
                                    _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "prodimages");
                                else
                                    _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "attachments");
                            }

                            //  FileInBytes = Fil.Length;
                            FileInKB = Fil.Length / 1024;

                            _stmpl_records.SetAttribute("TBT_PRODUCT_ATT_DESC", dr["PRODUCT_ATT_DESC"].ToString());
                            //Modified by indu 10Sep2015
                            //_stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", dr["PRODUCT_ATT_FILE"].ToString());
                            _stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", strfile.Replace("/Attachments/", "/attachments/").Replace(".PDF", ".pdf"));
                            _stmpl_records.SetAttribute("TBT_ATTACH_SIZE", FileInKB.ToString() + " KB");


                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                            ictrecords++;
                        }
                    }

                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DownloadRow");


                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    if (ictrecords > 0)
                        return _stmpl_container.ToString();

                }
                catch (Exception ex)
                {
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    return "";
                }

                return "";
            }
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Productpage_download = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return "";
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER ECOM LOGIN NAME ***/
        /********************************************************************************/
        private string GetLoginName()
        {
            string retvalue = string.Empty;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            DataTable objDt = new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(userid))
                {
                    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"];
                    //string sSQL = "SELECT CONTACT FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
                    //objHelperService.SQLString = sSQL;
                    //string iLoginName = objHelperService.GetValue("CONTACT");
                    string iLoginName = string.Empty;
                    if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
                    {
                        objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                        if (objDt != null && objDt.Rows.Count > 0)
                        {
                            iLoginName = objDt.Rows[0]["CONTACT"].ToString();
                            HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
                        }

                    }
                    else
                    {
                        // objDt = (DataTable)HttpContext.Current.Session["ECOM_LOGIN_COMP"];
                        objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                        if (objDt != null && objDt.Rows.Count > 0)
                            iLoginName = objDt.Rows[0]["CONTACT"].ToString();
                    }
                    retvalue = iLoginName;
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retvalue = "-1";
            }
            finally
            {
                objDt.Dispose();
                objDt = null;
            }
            return retvalue;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ECOM COMPANY NAME ***/
        /********************************************************************************/
        private string GetCompanyName()
        {
            string retvalue = string.Empty;
            DataTable objDt = new DataTable();
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (!string.IsNullOrEmpty(userid))
                {
                    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"];
                    //string sSQL = "SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
                    //objHelperService.SQLString = sSQL;
                    //int iCompanyID = objHelperService.CI(objHelperService.GetValue("COMPANY_ID"));
                    //string sSQL1 = "SELECT COMPANY_NAME FROM TBWC_COMPANY WHERE WEBSITE_ID = " + websiteid + " and COMPANY_ID = " + iCompanyID;
                    //objHelperService.SQLString = sSQL1;
                    //retvalue = objHelperService.GetValue("COMPANY_NAME").ToString().Trim();
                    if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
                    {
                        objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                        if (objDt != null && objDt.Rows.Count > 0)
                        {
                            HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
                            retvalue = objDt.Rows[0]["COMPANY_NAME"].ToString();

                        }
                    }
                    else
                    {
                        //  objDt = (DataTable)HttpContext.Current.Session["ECOM_LOGIN_COMP"];
                        objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                        if (objDt != null && objDt.Rows.Count > 0)
                        {
                            retvalue = objDt.Rows[0]["COMPANY_NAME"].ToString();

                        }
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retvalue = "-1";
            }
            finally
            {
                objDt.Dispose();
                objDt = null;
            }
            return retvalue;
        }




        public string ST_BOTTOM_SUPPLIER(string templatepath)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string sHTML = "";

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();



                // _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                // _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records = new StringTemplateGroup("row", _SkinRootPath + "\\" + templatepath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath + "\\" + templatepath);


                if (_Package == "CONTACTUSPAGE_SUPPLIER1")
                {
                    _stmpl_records = _stg_records.GetInstanceOf("Contactuspage_Supplier1" + "\\" + "cell");
                    _stmpl_container = _stg_container.GetInstanceOf("Contactuspage_Supplier1" + "\\" + "Main");
                }
                else if (_Package == "ABOUTUS_SUPPLIER1")
                {
                    _stmpl_records = _stg_records.GetInstanceOf("Aboutus_Supplier1" + "\\" + "cell");
                    _stmpl_container = _stg_container.GetInstanceOf("Aboutus_Supplier1" + "\\" + "Main");
                }
                else if (_Package == "LEFTNAV_SUPPLIER1")
                {
                    _stmpl_records = _stg_records.GetInstanceOf("LeftNav_Supplier1" + "\\" + "cell");
                    _stmpl_container = _stg_container.GetInstanceOf("LeftNav_Supplier1" + "\\" + "Main");
                    _stmpl_container.SetAttribute("SUPPIER_NAME", templatepath);

                    DataSet dscatname = new DataSet();
                    dscatname = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_MICROSITE_CATEGORY_NAME '" + templatepath + "'," + "''");
                    string href = "";
                    string href1 = "";
                    string url = "";


                    string catname = "";

                    string hreffinal = "";
                    string tbt_category = "";
                    string cat_id = "";

                    if (dscatname != null && dscatname.Tables.Count > 0 && dscatname.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= dscatname.Tables[0].Rows.Count - 1; i++)
                        {
                            //catname ="";
                            //hreffinal = "";
                            //url = "";
                            //href = "";
                            //href1 = "";
                            //catname = dscatname.Tables[0].Rows[i]["CATEGORY_NAME"].ToString();
                            //cat_id = dscatname.Tables[0].Rows[i]["CATEGORY_ID"].ToString();
                            //url = "/SupplierList/Supplier1/Cl_Supplier1.aspx?supplier_name=";
                            ////href = "<li>" + "<a href=" + url + templatepath.Trim() + "&CATEGORY_ID=" + cat_id+ ">";
                            //href = "<li>" + "<a href=" + url + HttpContext.Current.Request.QueryString["supplier_name"].ToString() + "&" + "category_id=" + cat_id + ">";
                            //href1 = "</a></li>";
                            //hreffinal = href + catname + href1;
                            //tbt_category = tbt_category + hreffinal;


                            catname = "";
                            hreffinal = "";
                            url = "";
                            href = "";
                            href1 = "";
                            catname = dscatname.Tables[0].Rows[i]["CATEGORY_NAME"].ToString();
                            cat_id = dscatname.Tables[0].Rows[i]["CATEGORY_ID"].ToString();
                            url = "/SupplierList/Supplier1/Cl_Supplier1.aspx?category_id=" + cat_id;
                            //href = "<li>" + "<a href=" + url + templatepath.Trim() + "&CATEGORY_ID=" + cat_id+ ">";
                            href = "<li>" + "<a href=" + "\"" + url + "&supplier_name=" + HttpContext.Current.Request.QueryString["supplier_name"].ToString() + "\"" + ">";
                            href1 = "</a></li>";
                            hreffinal = href + catname + href1;
                            tbt_category = tbt_category + hreffinal;
                        }
                    }
                    _stmpl_container.SetAttribute("TBT_CATLIST", tbt_category.Trim());

                    // _stmpl_container.SetAttribute("TBT_CATLIST1", HttpContext.Current.Request.QueryString["supplier_name"].ToString()); 
                }
                else if (_Package == "CATEGORYLIST_SUPPLIER1")
                {
                    _stmpl_records = _stg_records.GetInstanceOf("Categorylist" + "\\" + "cell");
                    _stmpl_container = _stg_container.GetInstanceOf("Categorylist" + "\\" + "Main");
                    _stmpl_container.SetAttribute("SUPPIER_NAME", templatepath);

                    string cat_id = "";
                    if (HttpContext.Current.Request.QueryString["CATEGORY_ID"] != null)
                    {
                        cat_id = HttpContext.Current.Request.QueryString["CATEGORY_ID"].ToString();
                    }
                    DataSet dscatnamemicrosite = new DataSet();
                    dscatnamemicrosite = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_MICROSITE_CATEGORY_NAME '" + templatepath + "'" + ",'" + cat_id + "'");
                    if (dscatnamemicrosite != null && dscatnamemicrosite.Tables[0].Rows.Count > 0)
                    {
                        _stmpl_container.SetAttribute("MS_CATEGORY_NAME", dscatnamemicrosite.Tables[0].Rows[0]["CATEGORY_NAME"].ToString());
                        _stmpl_container.SetAttribute("MS_CATEGORY_DESC", dscatnamemicrosite.Tables[0].Rows[0]["CATEGORY_DESC1"].ToString() + dscatnamemicrosite.Tables[0].Rows[0]["CATEGORY_DESC"].ToString());
                    }
                }
                else
                {

                    _stmpl_records = _stg_records.GetInstanceOf("Bottom_Supplier1" + "\\" + "cell");
                    _stmpl_container = _stg_container.GetInstanceOf("Bottom_Supplier1" + "\\" + "Main");
                    _stmpl_container.SetAttribute("SUPPIER_NAME", templatepath.Trim());
                }

                string pac = _Package;

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Bottom_supplier = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return sHTML;
        }

        public string ST_TOPLOG_SUPPLIER(string templatepath)
        {
            string sHTML = "";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();



                _stg_records = new StringTemplateGroup("row", _SkinRootPath + "\\" + templatepath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath + "\\" + templatepath);

                _stmpl_records = _stg_records.GetInstanceOf("Toplog_Supplier1" + "\\" + "cell");
                _stmpl_container = _stg_container.GetInstanceOf("Toplog_Supplier1" + "\\" + "Main");


                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Toplog_supplier = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return sHTML;
        }

        //public string ST_Top_Load_Cache()
        //{
        //    string sHTML = "";

        //    try
        //    {


        //        StringTemplateGroup _stg_container = null;
        //        StringTemplateGroup _stg_records = null;
        //        StringTemplate _stmpl_container = null;
        //        StringTemplate _stmpl_records = null;
        //        StringTemplate _stmpl_records1 = null;
        //        StringTemplate _stmpl_records2 = null;
        //        TBWDataList[] lstrecords = new TBWDataList[0];
        //        StringTemplateGroup _stg_records1 = null;
        //        StringTemplateGroup _stg_records2 = null;
        //        DataSet dsSubCat = new DataSet();
        //        DataSet dsSubCat1 = new DataSet();
        //        DataSet dsrecords = new DataSet();
  
        //        if ((DataSet)HttpContext.Current.Application["key_MainCategory"] != null)
        //        {
        //            dsrecords = (DataSet)HttpContext.Current.Application["key_MainCategory"];
        //        }
        //        else
        //        {
        //            dsrecords = EasyAsk.GetCategoryAndBrand_Applicationstart("MainCategory");
        //            HttpContext.Current.Application["key_MainCategory"] = dsrecords;
        //        }
        //        dsSubCat = EasyAsk.GetCategoryAndBrand_Applicationstart("SubCategoryAll");

        //        if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
        //            return "";



        //        //string requrl = HttpContext.Current.Request.Url.ToString().ToLower();



        //        _stg_records = new StringTemplateGroup("row", _SkinRootPath);
        //        _stg_container = new StringTemplateGroup("main", _SkinRootPath);
        //        _stg_records1 = new StringTemplateGroup("li", _SkinRootPath);
        //        _stg_records2 = new StringTemplateGroup("li2", _SkinRootPath);


        //        lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];



        //        int ictrecords = 0;

        //        string chkpackage = string.Empty;
        //        string dr_cat_id_upper = string.Empty;


        //        chkpackage = "TopCache";



        //        string currenturl = objHelperService.AddDomainname();
        //        string Subcatlist = "";
        //        string Subcatlist1 = "";
        //        int subcount = 0;
        //        int Rowcount = 0;

        //        string celltop = "";

        //        celltop = chkpackage + "\\cell1";



        //        foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
        //        {

        //            DataRow[] dr1 = dsrecords.Tables[0].Select("Category_id='" + dr["CATEGORY_ID"] + "' ");



        //            dr_cat_id_upper = dr["Category_id"].ToString().ToUpper();
        //            if (dr_cat_id_upper != WesNewsCategoryId && !(dr_cat_id_upper.Contains("SPF-")) && dr1.Length > 0)
        //            {

        //                _stmpl_records = _stg_records.GetInstanceOf(celltop);

        //                string url = string.Empty;
        //                if (dr1[0]["URL_RW_PATH"].ToString().Contains("http") == false)
        //                {

        //                    url = objHelperServices.SimpleURL_Str("////" + dr["CATEGORY_NAME"].ToString(), "pl.aspx");
        //                }
        //                else
        //                {
        //                    url = objHelperServices.SimpleURL_Str("////" + dr["CATEGORY_NAME"].ToString(), "pl.aspx");

        //                }
        //                _stmpl_records.SetAttribute("TBT_REWRITEURL", url);
        //                _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME_TOP"]);

        //                Subcatlist = "";
        //                if (dsSubCat != null && dsSubCat.Tables.Count > 0)
        //                {
        //                    DataRow[] subrows = dsSubCat.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + dr["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME");

        //                    if (subrows.Length > 0)
        //                    {
        //                        Rowcount = 0;
        //                        foreach (DataRow subdr in subrows)//For Records
        //                        {
        //                            _stmpl_records1 = _stg_records1.GetInstanceOf(chkpackage + "\\" + "cellsub");

        //                            _stmpl_records1.SetAttribute("TBT_REWRITEURL", objHelperServices.SimpleURL_Str(dr["CATEGORY_NAME"].ToString() + "////" + subdr["CATEGORY_NAME"].ToString(), "mpl.aspx"));

        //                            if (subdr["CATEGORY_NAME"].ToString().Length >= 29)
        //                                _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"].ToString().Substring(0, 29) + "..");
        //                            else
        //                                _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"].ToString());

        //                            if (Rowcount == 0)
        //                                Subcatlist = Subcatlist + "<Li>";


        //                            DataRow[] subrows1 = dsSubCat.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + subdr["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME");
        //                            Subcatlist1 = "";
        //                            subcount = 0;
        //                            if (subrows1.Length > 0)
        //                            {
        //                                foreach (DataRow subdr1 in subrows1)//For Records
        //                                {
        //                                    _stmpl_records2 = _stg_records2.GetInstanceOf(chkpackage + "\\" + "cellsub1");

        //                                    _stmpl_records2.SetAttribute("TBT_REWRITEURL", objHelperServices.SimpleURL_Str(dr["CATEGORY_NAME"].ToString() + "////" + subdr1["CATEGORY_NAME"].ToString(), "mpl.aspx"));

        //                                    if (subdr1["CATEGORY_NAME"] != null)
        //                                    {
        //                                        if (subdr1["CATEGORY_NAME"].ToString().Length >= 21)
        //                                            _stmpl_records2.SetAttribute("TBT_CATEGORY_NAME", subdr1["CATEGORY_NAME"].ToString().Substring(0, 19) + "..");
        //                                        else
        //                                            _stmpl_records2.SetAttribute("TBT_CATEGORY_NAME", subdr1["CATEGORY_NAME"].ToString());
        //                                    }

        //                                    Subcatlist1 = Subcatlist1 + _stmpl_records2.ToString();
        //                                    subcount = subcount + 1;
        //                                    Rowcount = Rowcount + 1;
        //                                    if (Rowcount >= 6)
        //                                        break;

        //                                }
        //                                _stmpl_records1.SetAttribute("SUBCAT_LIST1", Subcatlist1);


        //                            }
        //                            else
        //                            {
        //                                _stmpl_records1.SetAttribute("SUBCAT_LIST1", "");
        //                                Rowcount = Rowcount + 1;

        //                            }

        //                            if (Rowcount >= 6)
        //                            {

        //                                Subcatlist = Subcatlist + _stmpl_records1.ToString() + "</li>";

        //                                Rowcount = 0;
        //                            }
        //                            else
        //                                Subcatlist = Subcatlist + _stmpl_records1.ToString();

        //                        }
        //                        _stmpl_records.SetAttribute("IS_SUBCAT", true);
        //                        _stmpl_records.SetAttribute("SUBCAT_LIST", Subcatlist.ToString());
        //                    }
        //                    else
        //                    {
        //                        _stmpl_records.SetAttribute("IS_SUBCAT", false);
        //                    }
        //                }
        //                else
        //                {
        //                    _stmpl_records.SetAttribute("IS_SUBCAT", false);
        //                }

        //                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
        //                ictrecords++;
        //            }

        //        }



        //        _stmpl_container = _stg_container.GetInstanceOf(chkpackage + "\\" + "Main");


        //        _stmpl_container.SetAttribute("TBWDataList", lstrecords);
        //        sHTML = _stmpl_container.ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        sHTML = "";
        //    }
        //    return ProdimageRreplaceImages(sHTML);
        //}


        //commented on 22-july-2020
        public string ST_Top_Load_cache(bool ishome)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //objErrorHandler.CreateLog("ST_Top_Load_cache");

            string sHTML = string.Empty;

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_records1 = null;
                StringTemplate _stmpl_records2 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                // TBWDataList[] lstrows = new TBWDataList[0];

                //StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                StringTemplateGroup _stg_records2 = null;
                //   TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                DataSet dsSubCat = new DataSet();
                DataSet dsSubCat1 = new DataSet();
                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                // DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");
                if ((DataSet)HttpContext.Current.Application["key_MainCategory"] != null)
                {
                    dsrecords = (DataSet)HttpContext.Current.Application["key_MainCategory"];
                }
                else
                {
                    dsrecords = EasyAsk.GetCategoryAndBrand_Applicationstart("MainCategory");
                    HttpContext.Current.Application["key_MainCategory"] = dsrecords;
                }
                dsSubCat = EasyAsk.GetCategoryAndBrand_Applicationstart("SubCategoryAll");
                // dsSubCat = (DataSet)HttpContext.Current.Application["key_SubCategoryAll"];

                //DataSet dsrecords_dup = new DataSet();
                //dsrecords_dup = EasyAsk.GetCategoryAndBrand_Applicationstart("Mainds_Sort");

                DataTable dt = dsSubCat.Tables[0].Copy();
                dt.Columns.Add("SUB_COUNT", typeof(int));
                //objErrorHandler.CreateLog("dsrecords table count " + dsrecords.Tables.Count);
                //objErrorHandler.CreateLog("dsrecords record count " + dsrecords.Tables[0].Rows.Count);
                //if (HttpContext.Current.Application["key_Mainds_Sort"] != null)
                //{

                //    dsrecords_dup = (DataSet)HttpContext.Current.Application["key_Mainds_Sort"];
                //}
                //if ( Fil1.Exists == true) 
                //{

                //if (HttpContext.Current.Session["Mainds_Sort"] == null)
                //{
                //    dsrecords_dup.ReadXml(strxml + "\\Mainds_Sort.xml");
                //    HttpContext.Current.Session["Mainds_Sort"] = dsrecords_dup;
                //}
                //else
                //{

                //    dsrecords_dup = (DataSet)HttpContext.Current.Session["Mainds_Sort"];
                //}


                //}

                // if (dsrecords != null && dsrecords.Tables.Count > 0 && dsrecords.Tables[0].Rows.Count>0   )
                //{ // remove WES NEWS MENU 
                //    //DataRow[] dr= tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'","CATEGORY_NAME" );
                //    //TO ADD WESNEWS
                //    //drs = tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");
                //    //DataRow[] dr = tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");
                //   // drs=tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'", "CATEGORY_NAME");



                //    //if (drs.Length > 0)
                //    //{
                //    //    dsrecords.Tables.Add(drs.CopyToDataTable().Copy());
                //    //}
                //    //else
                //    //    return "";
                //}
                //else
                //    return "";

                if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
                    return "";


                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                //  int ictrows = 0;


                //  string requrl = HttpContext.Current.Request.Url.ToString().ToLower();


                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("li", _SkinRootPath);
                _stg_records2 = new StringTemplateGroup("li2", _SkinRootPath);


                lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];



                int ictrecords = 0;

                string chkpackage = string.Empty;
                string dr_cat_id_upper = string.Empty;

                if (_Package == "TOP")
                    chkpackage = "Top";
                else
                    chkpackage = "TopLog";
                

                string currenturl = objHelperService.AddDomainname();
                string Subcatlist = "";
                string Subcatlist1 = "";
                int subcount = 0;
                int Rowcount = 0;
                //  System.IO.FileInfo subcatfile=null;
                string celltop = "";
                if (ishome)
                    celltop = chkpackage + "\\cell1";
                else
                    celltop = chkpackage + "\\cell1";
                //   objErrorHandler.CreateLog(dsrecords_dup.Tables[0].Rows.Count.ToString() );
                foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                {

                    DataRow[] dr1 = dsrecords.Tables[0].Select("Category_id='" + dr["CATEGORY_ID"] + "' ");

                    //objErrorHandler.CreateLog("CATEGORY_ID " + dr["CATEGORY_ID"]);

                    dr_cat_id_upper = dr["Category_id"].ToString().ToUpper();
                    if (dr_cat_id_upper != WesNewsCategoryId &&  dr1.Length > 0)
                    {
                        //objErrorHandler.CreateLog("length "+ dr1.Length);
                        //if (_Package == "TOP")
                        //    _stmpl_records = _stg_records.GetInstanceOf("Top" + "\\" + "cell");
                        //else
                        //    _stmpl_records = _stg_records.GetInstanceOf("TopLog" + "\\" + "cell");

                        _stmpl_records = _stg_records.GetInstanceOf(celltop);
                        //remove tostring

                        //string currenturl = System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString();
                        //  string currenturl = "http://"+HttpContext.Current.Request.Url.Host+"/";
                        //currenturl + "\\" + 
                        string url = string.Empty;
                        //if (dr1[0]["URL_RW_PATH"].ToString().Contains("http") == false)
                        //{

                        //    url = currenturl + dr1[0]["URL_RW_PATH"].ToString();

                        //}
                        //else
                        //{

                        url = currenturl + dr1[0]["URL_RW_PATH"].ToString();
                        //}
                        _stmpl_records.SetAttribute("TBT_REWRITEURL", url);
                        _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr1[0]["CATEGORY_NAME_TOP"]);
                        _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr1[0]["CATEGORY_ID"]);
                        _stmpl_records.SetAttribute("TBT_CATEGORY_NAME_ORG", dr1[0]["CATEGORY_NAME"]);
                        _stmpl_records.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());

                        //if (requrl.Contains("home.aspx") != true)
                        //{
                        Subcatlist = "";
                        if (dsSubCat != null && dsSubCat.Tables.Count > 0)
                        {

                            //for(int i=0; i<dsSubCat.Tables[0].Rows.Count; i++)//For Records
                            //{
                            //DataRow subdr in dsSubCat.Tables[0].Rows

                            //}



                            DataRow[] subrowscount = dt.Select("TBT_PARENT_CATEGORY_ID='" + dr["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME");



                            if (subrowscount.Length > 0)
                            {
                                Rowcount = 0;
                                //int listcount = 0;
                                int recordscount = 0;

                                //foreach (DataRow subdr in subrowscount)
                                //{
                                //    DataRow[] subrowsinner = dt.Select("TBT_PARENT_CATEGORY_ID='" + subdr["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME");
                                //    subdr["SUB_COUNT"] = subrowsinner.Length;
                                //    recordscount += Convert.ToInt32(subdr["SUB_COUNT"].ToString());
                                //}
                                //foreach (DataRow subdr in subrows)//For Records count
                                //{
                                //    recordscount += Convert.ToInt32(subdr["SUB_COUNT"].ToString());
                                //}

                                //listcount = recordscount / 6;
                                DataRow[] subrows = dt.Select("TBT_PARENT_CATEGORY_ID='" + dr["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME");



                                foreach (DataRow subdr in subrows)//For Records
                                {
                                    _stmpl_records1 = _stg_records1.GetInstanceOf(chkpackage + "\\" + "cellsub");
                                    _stmpl_records1.SetAttribute("TBT_REWRITEURL", subdr["URL_RW_PATH"]);

                                    if (subdr["CATEGORY_NAME"].ToString().Length >= 29)
                                        _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"].ToString().Substring(0, 29) + "..");
                                    else
                                        _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"]);

                                    //if (Rowcount == 0)
                                    Subcatlist = Subcatlist + "<Li>";


                                    DataRow[] subrows1 = dsSubCat.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + subdr["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME");
                                    Subcatlist1 = "";
                                    subcount = 0;
                                    if (subrows1.Length > 0)
                                    {
                                        foreach (DataRow subdr1 in subrows1)//For Records
                                        {
                                            _stmpl_records2 = _stg_records2.GetInstanceOf(chkpackage + "\\" + "cellsub1");
                                            _stmpl_records2.SetAttribute("TBT_REWRITEURL", subdr1["URL_RW_PATH"]);
                                            _stmpl_records2.SetAttribute("TBT_PRODUCT_COUNT", subdr1["PRODUCT_COUNT"]);
                                            //Added if to fix error in logfile 3 feb 2016
                                            if (subdr1["CATEGORY_NAME"] != null)
                                            {
                                                if (subdr1["CATEGORY_NAME"].ToString().Length >= 28)
                                                    _stmpl_records2.SetAttribute("TBT_CATEGORY_NAME", subdr1["CATEGORY_NAME"].ToString().Substring(0, 26) + "..");
                                                else
                                                    _stmpl_records2.SetAttribute("TBT_CATEGORY_NAME", subdr1["CATEGORY_NAME"]);
                                            }

                                            Subcatlist1 = Subcatlist1 + _stmpl_records2.ToString();
                                            subcount = subcount + 1;
                                            Rowcount = Rowcount + 1;
                                            //if (Rowcount >= listcount)
                                            //    break;

                                        }
                                        _stmpl_records1.SetAttribute("SUBCAT_LIST1", Subcatlist1);

                                        //if (subcount > 5)
                                        //    _stmpl_records1.SetAttribute("IS_VIEW_MORE", true);
                                        //else
                                        //    _stmpl_records1.SetAttribute("IS_VIEW_MORE", false);
                                    }
                                    else
                                    {
                                        _stmpl_records1.SetAttribute("SUBCAT_LIST1", "");
                                        Rowcount = Rowcount + 1;
                                        //_stmpl_records1.SetAttribute("IS_VIEW_MORE", false);
                                    }
                                    Subcatlist = Subcatlist + _stmpl_records1.ToString() + "</li>";
                                    //if (Rowcount >= listcount)
                                    //{
                                    //    //if (requrl.Contains("home.aspx") != true)
                                    //    //{
                                    //    Subcatlist = Subcatlist + _stmpl_records1.ToString() + "</li>";
                                    //    //}
                                    //    //else
                                    //    //{
                                    //    //    Subcatlist = Subcatlist + _stmpl_records1.ToString();
                                    //    //}
                                    //    Rowcount = 0;
                                    //}
                                    //else
                                    //    Subcatlist = Subcatlist + _stmpl_records1.ToString();

                                }
                                _stmpl_records.SetAttribute("IS_SUBCAT", true);
                                _stmpl_records.SetAttribute("SUBCAT_LIST", Subcatlist.ToString());
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("IS_SUBCAT", false);
                            }
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("IS_SUBCAT", false);
                        }
                        //}
                        //_stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME_TOP"]);
                        // _stmpl_records.SetAttribute("TBT_CURRENTURL", currenturl);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }
                    //objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                }

                //if (_Package == "TOP")
                //    _stmpl_container = _stg_container.GetInstanceOf("Top" + "\\" + "Main");
                //else
                //    _stmpl_container = _stg_container.GetInstanceOf("TopLog" + "\\" + "Main");

                _stmpl_container = _stg_container.GetInstanceOf(chkpackage + "\\" + "Main_cache");
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                sHTML = _stmpl_container.ToString();
                //   _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);

                //if (requrl.Contains("home.aspx") == true)
                //    _stmpl_container.SetAttribute("TBT_CHK_HOME", true);
                //else
                //    _stmpl_container.SetAttribute("TBT_CHK_HOME", false);
                //if (_Package == "TOP")
                //{

                //if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].Equals("") && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
                //{
                //    string lname = GetLoginName();
                //    _stmpl_container.SetAttribute("TBT_LOGIN_NAME", lname);
                //    HttpContext.Current.Session["LOGIN_NAME_TOP"] = lname;
                //}

                //if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                //{
                //    if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString()) == 4)
                //    {
                //        string ReMailLink = "<a Href=ConfirmMessage.aspx?Result=REMAILACTIVATION class=\"linkemailre\">Re-Email Activation Link Now</a>";

                //        _stmpl_container.SetAttribute("TBT_LOGIN_NAME", " Your Account Has Not Been Activated! " + ReMailLink);
                //    }
                //    else
                //        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", "");
                //}
                //else
                //{
                //    if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
                //    {
                //        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", GetCompanyName() + ",");
                //    }
                //}

                //if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "" && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
                //{
                //    string lname = GetLoginName();
                //    HttpContext.Current.Session["LOGIN_NAME"] = lname;
                //    HttpContext.Current.Session["LOGIN_NAME_TOP"] = lname;
                //}


                ////}


                //string _Tbt_Order_Id = string.Empty;
                //string _Tbt_Ship_URL = string.Empty;
                //int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                //if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                //{
                //    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                //}
                //else
                //{

                //    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();

                //    //    HttpContext.Current.Session["ORDER_ID"] = _Tbt_Order_Id;
                //}

                //if ((HttpContext.Current.Session["ORDER_ID"] != null &&
                //    Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) ||
                //    (HttpContext.Current.Request.QueryString["ViewOrder"] != null &&
                //    HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) ||
                //    (HttpContext.Current.Request.QueryString["ApproveOrder"] != null &&
                //    HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                //{
                //    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                //    _Tbt_Ship_URL = "/checkout.aspx?" + EncryptSP(_Tbt_Order_Id);
                //}
                //else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
                //{
                //    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                //    _Tbt_Ship_URL = "/checkout.aspx?" + EncryptSP(_Tbt_Order_Id);
                //}
                //else
                //{
                //    //_Tbt_Ship_URL = "shipping.aspx";
                //    _Tbt_Ship_URL = "";
                //}
                //_stmpl_container.SetAttribute("TBT_ORDER_ID", _Tbt_Order_Id);
                //_stmpl_container.SetAttribute("TBT_SHIP_URL", _Tbt_Ship_URL);
                //_stmpl_container.SetAttribute("TBT_CURRENTURL", currenturl);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }

            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Top_Load_catche = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();

            //objErrorHandler.CreateLog(sHTML);
            //return ProdimageRreplaceImages(sHTML);
            return sHTML;
        }


        //public string ST_Top_Load_cache(bool ishome)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();


        //    string sHTML = string.Empty;

        //    try
        //    {
        //        StringTemplateGroup _stg_container = null;
        //        StringTemplateGroup _stg_records = null;
        //        StringTemplate _stmpl_container = null;
        //        StringTemplate _stmpl_records = null;
        //        StringTemplate _stmpl_records1 = null;
        //        StringTemplate _stmpl_records2 = null;
        //        // StringTemplate _stmpl_recordsrows = null;
        //        TBWDataList[] lstrecords = new TBWDataList[0];
        //        // TBWDataList[] lstrows = new TBWDataList[0];

        //        //StringTemplateGroup _stg_container1 = null;
        //        StringTemplateGroup _stg_records1 = null;
        //        StringTemplateGroup _stg_records2 = null;
        //        //   TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        //        // TBWDataList1[] lstrows1 = new TBWDataList1[0];

        //        DataSet dsSubCat = new DataSet();
        //        DataSet dsSubCat1 = new DataSet();
        //        DataSet dsrecords = new DataSet();
        //        // DataTable dt = null;
        //        // DataRow[] drs = null;

        //        //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");
        //        if ((DataSet)HttpContext.Current.Application["key_MainCategory"] != null)
        //        {
        //            dsrecords = (DataSet)HttpContext.Current.Application["key_MainCategory"];
        //        }
        //        else
        //        {
        //            dsrecords = EasyAsk.GetCategoryAndBrand_Applicationstart("MainCategory");
        //            HttpContext.Current.Application["key_MainCategory"] = dsrecords;
        //        }
        //        dsSubCat = EasyAsk.GetCategoryAndBrand_Applicationstart("SubCategoryAll");
        //        // dsSubCat = (DataSet)HttpContext.Current.Application["key_SubCategoryAll"];

        //        DataSet dsrecords_dup = new DataSet();

        //        dsrecords_dup = EasyAsk.GetCategoryAndBrand_Applicationstart("Mainds_Sort");
        //        //if (HttpContext.Current.Application["key_Mainds_Sort"] != null)
        //        //{

        //        //    dsrecords_dup = (DataSet)HttpContext.Current.Application["key_Mainds_Sort"];
        //        //}
        //        //if ( Fil1.Exists == true) 
        //        //{

        //        //if (HttpContext.Current.Session["Mainds_Sort"] == null)
        //        //{
        //        //    dsrecords_dup.ReadXml(strxml + "\\Mainds_Sort.xml");
        //        //    HttpContext.Current.Session["Mainds_Sort"] = dsrecords_dup;
        //        //}
        //        //else
        //        //{

        //        //    dsrecords_dup = (DataSet)HttpContext.Current.Session["Mainds_Sort"];
        //        //}


        //        //}

        //        // if (dsrecords != null && dsrecords.Tables.Count > 0 && dsrecords.Tables[0].Rows.Count>0   )
        //        //{ // remove WES NEWS MENU 
        //        //    //DataRow[] dr= tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'","CATEGORY_NAME" );
        //        //    //TO ADD WESNEWS
        //        //    //drs = tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");
        //        //    //DataRow[] dr = tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");
        //        //   // drs=tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'", "CATEGORY_NAME");



        //        //    //if (drs.Length > 0)
        //        //    //{
        //        //    //    dsrecords.Tables.Add(drs.CopyToDataTable().Copy());
        //        //    //}
        //        //    //else
        //        //    //    return "";
        //        //}
        //        //else
        //        //    return "";

        //        if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
        //            return "";


        //        // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
        //        //  int ictrows = 0;


        //        //  string requrl = HttpContext.Current.Request.Url.ToString().ToLower();


        //        _stg_records = new StringTemplateGroup("row", _SkinRootPath);
        //        _stg_container = new StringTemplateGroup("main", _SkinRootPath);
        //        _stg_records1 = new StringTemplateGroup("li", _SkinRootPath);
        //        _stg_records2 = new StringTemplateGroup("li2", _SkinRootPath);


        //        lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];



        //        int ictrecords = 0;

        //        string chkpackage = string.Empty;
        //        string dr_cat_id_upper = string.Empty;

        //        if (_Package == "TOP")
        //            chkpackage = "Top";
        //        else
        //            chkpackage = "TopLog";



        //        string currenturl = objHelperService.AddDomainname();
        //        string Subcatlist = "";
        //        string Subcatlist1 = "";
        //        int subcount = 0;
        //        int Rowcount = 0;
        //        //  System.IO.FileInfo subcatfile=null;
        //        string celltop = "";
        //        if (ishome)
        //            celltop = chkpackage + "\\cell";
        //        else
        //            celltop = chkpackage + "\\cell1";
        //        //   objErrorHandler.CreateLog(dsrecords_dup.Tables[0].Rows.Count.ToString() );
        //        foreach (DataRow dr in dsrecords_dup.Tables[0].Rows)//For Records
        //        {

        //            DataRow[] dr1 = dsrecords.Tables[0].Select("Category_id='" + dr["CATEGORY_ID"] + "' ");



        //            dr_cat_id_upper = dr["Category_id"].ToString().ToUpper();
        //            if (dr_cat_id_upper != WesNewsCategoryId && !(dr_cat_id_upper.Contains("SPF-")) && dr1.Length > 0)
        //            {
        //                //if (_Package == "TOP")
        //                //    _stmpl_records = _stg_records.GetInstanceOf("Top" + "\\" + "cell");
        //                //else
        //                //    _stmpl_records = _stg_records.GetInstanceOf("TopLog" + "\\" + "cell");

        //                _stmpl_records = _stg_records.GetInstanceOf(celltop);
        //                //remove tostring

        //                //string currenturl = System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString();
        //                //  string currenturl = "http://"+HttpContext.Current.Request.Url.Host+"/";
        //                //currenturl + "\\" + 
        //                string url = string.Empty;
        //                //if (dr1[0]["URL_RW_PATH"].ToString().Contains("http") == false)
        //                //{

        //                //    url = currenturl + dr1[0]["URL_RW_PATH"].ToString();

        //                //}
        //                //else
        //                //{

        //                url = currenturl + dr1[0]["URL_RW_PATH"].ToString();
        //                //}
        //                _stmpl_records.SetAttribute("TBT_REWRITEURL", url);
        //                _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr1[0]["CATEGORY_NAME_TOP"]);
        //                _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr1[0]["CATEGORY_ID"]);
        //                _stmpl_records.SetAttribute("TBT_CATEGORY_NAME_ORG", dr1[0]["CATEGORY_NAME"]);
        //                _stmpl_records.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());

        //                //if (requrl.Contains("home.aspx") != true)
        //                //{
        //                Subcatlist = "";
        //                if (dsSubCat != null && dsSubCat.Tables.Count > 0)
        //                {
        //                    DataRow[] subrows = dsSubCat.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + dr["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME");

        //                    if (subrows.Length > 0)
        //                    {
        //                        Rowcount = 0;
        //                        foreach (DataRow subdr in subrows)//For Records
        //                        {
        //                            _stmpl_records1 = _stg_records1.GetInstanceOf(chkpackage + "\\" + "cellsub");
        //                            _stmpl_records1.SetAttribute("TBT_REWRITEURL", subdr["URL_RW_PATH"]);

        //                            if (subdr["CATEGORY_NAME"].ToString().Length >= 29)
        //                                _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"].ToString().Substring(0, 29) + "..");
        //                            else
        //                                _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"]);
        //                            //if (requrl.Contains("home.aspx") != true)
        //                            //{
        //                            if (Rowcount == 0)
        //                                Subcatlist = Subcatlist + "<Li>";
        //                            //}

        //                            DataRow[] subrows1 = dsSubCat.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + subdr["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME");
        //                            Subcatlist1 = "";
        //                            subcount = 0;
        //                            if (subrows1.Length > 0)
        //                            {
        //                                foreach (DataRow subdr1 in subrows1)//For Records
        //                                {
        //                                    _stmpl_records2 = _stg_records2.GetInstanceOf(chkpackage + "\\" + "cellsub1");
        //                                    _stmpl_records2.SetAttribute("TBT_REWRITEURL", subdr1["URL_RW_PATH"]);
        //                                    //Added if to fix error in logfile 3 feb 2016
        //                                    if (subdr1["CATEGORY_NAME"] != null)
        //                                    {
        //                                        if (subdr1["CATEGORY_NAME"].ToString().Length >= 21)
        //                                            _stmpl_records2.SetAttribute("TBT_CATEGORY_NAME", subdr1["CATEGORY_NAME"].ToString().Substring(0, 19) + "..");
        //                                        else
        //                                            _stmpl_records2.SetAttribute("TBT_CATEGORY_NAME", subdr1["CATEGORY_NAME"]);
        //                                    }

        //                                    Subcatlist1 = Subcatlist1 + _stmpl_records2.ToString();
        //                                    subcount = subcount + 1;
        //                                    Rowcount = Rowcount + 1;
        //                                    if (Rowcount >= 18)
        //                                        break;

        //                                }
        //                                _stmpl_records1.SetAttribute("SUBCAT_LIST1", Subcatlist1);

        //                                //if (subcount > 5)
        //                                //    _stmpl_records1.SetAttribute("IS_VIEW_MORE", true);
        //                                //else
        //                                //    _stmpl_records1.SetAttribute("IS_VIEW_MORE", false);
        //                            }
        //                            else
        //                            {
        //                                _stmpl_records1.SetAttribute("SUBCAT_LIST1", "");
        //                                Rowcount = Rowcount + 1;
        //                                //_stmpl_records1.SetAttribute("IS_VIEW_MORE", false);
        //                            }

        //                            if (Rowcount >= 18)
        //                            {
        //                                //if (requrl.Contains("home.aspx") != true)
        //                                //{
        //                                Subcatlist = Subcatlist + _stmpl_records1.ToString() + "</li>";
        //                                //}
        //                                //else
        //                                //{
        //                                //    Subcatlist = Subcatlist + _stmpl_records1.ToString();
        //                                //}
        //                                Rowcount = 0;
        //                            }
        //                            else
        //                                Subcatlist = Subcatlist + _stmpl_records1.ToString();

        //                        }
        //                        _stmpl_records.SetAttribute("IS_SUBCAT", true);
        //                        _stmpl_records.SetAttribute("SUBCAT_LIST", Subcatlist.ToString());
        //                    }
        //                    else
        //                    {
        //                        _stmpl_records.SetAttribute("IS_SUBCAT", false);
        //                    }
        //                }
        //                else
        //                {
        //                    _stmpl_records.SetAttribute("IS_SUBCAT", false);
        //                }
        //                //}
        //                //_stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME_TOP"]);
        //                // _stmpl_records.SetAttribute("TBT_CURRENTURL", currenturl);
        //                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
        //                ictrecords++;
        //            }
        //            //objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
        //        }

        //        //if (_Package == "TOP")
        //        //    _stmpl_container = _stg_container.GetInstanceOf("Top" + "\\" + "Main");
        //        //else
        //        //    _stmpl_container = _stg_container.GetInstanceOf("TopLog" + "\\" + "Main");

        //        _stmpl_container = _stg_container.GetInstanceOf(chkpackage + "\\" + "Main_cache");
        //        _stmpl_container.SetAttribute("TBWDataList", lstrecords);
        //        _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
        //        sHTML = _stmpl_container.ToString();
        //        //   _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);

        //        //if (requrl.Contains("home.aspx") == true)
        //        //    _stmpl_container.SetAttribute("TBT_CHK_HOME", true);
        //        //else
        //        //    _stmpl_container.SetAttribute("TBT_CHK_HOME", false);
        //        //if (_Package == "TOP")
        //        //{

        //        //if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].Equals("") && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
        //        //{
        //        //    string lname = GetLoginName();
        //        //    _stmpl_container.SetAttribute("TBT_LOGIN_NAME", lname);
        //        //    HttpContext.Current.Session["LOGIN_NAME_TOP"] = lname;
        //        //}

        //        //if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
        //        //{
        //        //    if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString()) == 4)
        //        //    {
        //        //        string ReMailLink = "<a Href=ConfirmMessage.aspx?Result=REMAILACTIVATION class=\"linkemailre\">Re-Email Activation Link Now</a>";

        //        //        _stmpl_container.SetAttribute("TBT_LOGIN_NAME", " Your Account Has Not Been Activated! " + ReMailLink);
        //        //    }
        //        //    else
        //        //        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", "");
        //        //}
        //        //else
        //        //{
        //        //    if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
        //        //    {
        //        //        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", GetCompanyName() + ",");
        //        //    }
        //        //}

        //        //if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "" && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
        //        //{
        //        //    string lname = GetLoginName();
        //        //    HttpContext.Current.Session["LOGIN_NAME"] = lname;
        //        //    HttpContext.Current.Session["LOGIN_NAME_TOP"] = lname;
        //        //}


        //        ////}


        //        //string _Tbt_Order_Id = string.Empty;
        //        //string _Tbt_Ship_URL = string.Empty;
        //        //int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
        //        //if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
        //        //{
        //        //    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
        //        //}
        //        //else
        //        //{

        //        //    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();

        //        //    //    HttpContext.Current.Session["ORDER_ID"] = _Tbt_Order_Id;
        //        //}

        //        //if ((HttpContext.Current.Session["ORDER_ID"] != null &&
        //        //    Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) ||
        //        //    (HttpContext.Current.Request.QueryString["ViewOrder"] != null &&
        //        //    HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) ||
        //        //    (HttpContext.Current.Request.QueryString["ApproveOrder"] != null &&
        //        //    HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
        //        //{
        //        //    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
        //        //    _Tbt_Ship_URL = "/checkout.aspx?" + EncryptSP(_Tbt_Order_Id);
        //        //}
        //        //else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
        //        //{
        //        //    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
        //        //    _Tbt_Ship_URL = "/checkout.aspx?" + EncryptSP(_Tbt_Order_Id);
        //        //}
        //        //else
        //        //{
        //        //    //_Tbt_Ship_URL = "shipping.aspx";
        //        //    _Tbt_Ship_URL = "";
        //        //}
        //        //_stmpl_container.SetAttribute("TBT_ORDER_ID", _Tbt_Order_Id);
        //        //_stmpl_container.SetAttribute("TBT_SHIP_URL", _Tbt_Ship_URL);
        //        //_stmpl_container.SetAttribute("TBT_CURRENTURL", currenturl);

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        sHTML = "";
        //    }

        //    sw.Stop();
        //    objErrorHandler.ExeTimelog = "ST_Top_Load_catche = " + sw.Elapsed.TotalSeconds.ToString();
        //    objErrorHandler.createexecutiontmielog();


        //    //return ProdimageRreplaceImages(sHTML);
        //    return sHTML;
        //}


        public string ST_Top_Load()
        {
            Stopwatch sw = new Stopwatch();
            
            sw.Start();

            string sHTML = string.Empty;

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplateGroup _stg_container1 = null;
                StringTemplate _stmpl_container1 = null;
                //StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                //  StringTemplate _stmpl_records2 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                // TBWDataList[] lstrows = new TBWDataList[0];

                //StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                StringTemplateGroup _stg_records2 = null;
                //   TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                // DataSet dsSubCat = new DataSet();
                // DataSet dsSubCat1 = new DataSet();
                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                // DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");
                //if ((DataSet)HttpContext.Current.Application["key_MainCategory"] != null)
                //{
                //    dsrecords = (DataSet)HttpContext.Current.Application["key_MainCategory"];
                //}
                //else
                //{
                //  dsrecords=  EasyAsk.GetCategoryAndBrand_Applicationstart("MainCategory");
                //  HttpContext.Current.Application["key_MainCategory"] = dsrecords;
                //}
                // dsSubCat = EasyAsk.GetCategoryAndBrand("SubCategoryAll");
                //dsSubCat = (DataSet)HttpContext.Current.Application["key_SubCategoryAll"];

                //  DataSet dsrecords_dup = new DataSet();


                //  if (HttpContext.Current.Application["key_Mainds_Sort"] != null)
                //  {

                //      dsrecords_dup = (DataSet)HttpContext.Current.Application["key_Mainds_Sort"];
                //  }
                //if ( Fil1.Exists == true) 
                //{

                //if (HttpContext.Current.Session["Mainds_Sort"] == null)
                //{
                //    dsrecords_dup.ReadXml(strxml + "\\Mainds_Sort.xml");
                //    HttpContext.Current.Session["Mainds_Sort"] = dsrecords_dup;
                //}
                //else
                //{

                //    dsrecords_dup = (DataSet)HttpContext.Current.Session["Mainds_Sort"];
                //}


                //}

                // if (dsrecords != null && dsrecords.Tables.Count > 0 && dsrecords.Tables[0].Rows.Count>0   )
                //{ // remove WES NEWS MENU 
                //    //DataRow[] dr= tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'","CATEGORY_NAME" );
                //    //TO ADD WESNEWS
                //    //drs = tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");
                //    //DataRow[] dr = tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");
                //   // drs=tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'", "CATEGORY_NAME");



                //    //if (drs.Length > 0)
                //    //{
                //    //    dsrecords.Tables.Add(drs.CopyToDataTable().Copy());
                //    //}
                //    //else
                //    //    return "";
                //}
                //else
                //    return "";

                //if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
                //    return "";


                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                //  int ictrows = 0;


                string requrl = HttpContext.Current.Request.Url.ToString().ToLower();


                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("li", _SkinRootPath);
                _stg_records2 = new StringTemplateGroup("li2", _SkinRootPath);


                //   lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];



                //   int ictrecords = 0;

                string chkpackage = "TopLog";
                string currenturl = objHelperService.AddDomainname();
                //   string dr_cat_id_upper = string.Empty;

                //   if (_Package == "TOP")
                //       chkpackage = "Top";
                //   else
                //       chkpackage = "TopLog";



                //   string currenturl = objHelperService.AddDomainname();
                //   string Subcatlist = "";
                //   string Subcatlist1 = "";
                //   int subcount = 0;
                //   int Rowcount = 0;
                // //  System.IO.FileInfo subcatfile=null;
                //   string celltop = "";
                //   if(requrl.Contains("home.aspx") == true)
                //           celltop= chkpackage + "\\cell";
                //   else
                //       celltop = chkpackage + "\\cell1";
                ////   objErrorHandler.CreateLog(dsrecords_dup.Tables[0].Rows.Count.ToString() );
                //   foreach (DataRow dr in dsrecords_dup.Tables[0].Rows)//For Records
                //   {

                //       DataRow[]  dr1 = dsrecords.Tables[0].Select("Category_id='" + dr["CATEGORY_ID"] + "' ");



                //       dr_cat_id_upper = dr["Category_id"].ToString().ToUpper();
                //       if (dr_cat_id_upper != WesNewsCategoryId && !(dr_cat_id_upper.Contains("SPF-")) && dr1.Length >0 )
                //       {
                //           //if (_Package == "TOP")
                //           //    _stmpl_records = _stg_records.GetInstanceOf("Top" + "\\" + "cell");
                //           //else
                //           //    _stmpl_records = _stg_records.GetInstanceOf("TopLog" + "\\" + "cell");

                //           _stmpl_records = _stg_records.GetInstanceOf(celltop);
                //           //remove tostring

                //           //string currenturl = System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString();
                //         //  string currenturl = "http://"+HttpContext.Current.Request.Url.Host+"/";
                //           //currenturl + "\\" + 
                //           string url = string.Empty;
                //           if (dr1[0]["URL_RW_PATH"].ToString().Contains("http") == false)
                //           {

                //               url = currenturl + dr1[0]["URL_RW_PATH"].ToString();

                //           }
                //           else
                //           {

                //               url = dr1[0]["URL_RW_PATH"].ToString();
                //           }
                //           _stmpl_records.SetAttribute("TBT_REWRITEURL",url);
                //           _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr1[0]["CATEGORY_NAME_TOP"]);
                //           _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr1[0]["CATEGORY_ID"]);
                //           _stmpl_records.SetAttribute("TBT_CATEGORY_NAME_ORG", dr1[0]["CATEGORY_NAME"]);
                //           //if (requrl.Contains("home.aspx") != true)
                //           //{
                //               Subcatlist = "";
                //               if (dsSubCat != null && dsSubCat.Tables.Count > 0)
                //               {
                //                   DataRow[] subrows = dsSubCat.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + dr["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME");

                //                   if (subrows.Length > 0)
                //                   {
                //                       Rowcount = 0;
                //                       foreach (DataRow subdr in subrows)//For Records
                //                       {
                //                           _stmpl_records1 = _stg_records1.GetInstanceOf(chkpackage + "\\" + "cellsub");
                //                           _stmpl_records1.SetAttribute("TBT_REWRITEURL", subdr["URL_RW_PATH"]);

                //                           if (subdr["CATEGORY_NAME"].ToString().Length >= 29)
                //                               _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"].ToString().Substring(0, 29) + "..");
                //                           else
                //                               _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"]);
                //                           //if (requrl.Contains("home.aspx") != true)
                //                           //{
                //                               if (Rowcount == 0)
                //                                   Subcatlist = Subcatlist + "<Li>";
                //                           //}

                //                           DataRow[] subrows1 = dsSubCat.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + subdr["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME");
                //                           Subcatlist1 = "";
                //                           subcount = 0;
                //                           if (subrows1.Length > 0)
                //                           {
                //                               foreach (DataRow subdr1 in subrows1)//For Records
                //                               {
                //                                   _stmpl_records2 = _stg_records2.GetInstanceOf(chkpackage + "\\" + "cellsub1");
                //                                   _stmpl_records2.SetAttribute("TBT_REWRITEURL", subdr1["URL_RW_PATH"]);
                //                                   //Added if to fix error in logfile 3 feb 2016
                //                                   if (subdr1["CATEGORY_NAME"] != null)
                //                                   {
                //                                       if (subdr1["CATEGORY_NAME"].ToString().Length >= 21)
                //                                           _stmpl_records2.SetAttribute("TBT_CATEGORY_NAME", subdr1["CATEGORY_NAME"].ToString().Substring(0, 19) + "..");
                //                                       else
                //                                           _stmpl_records2.SetAttribute("TBT_CATEGORY_NAME", subdr1["CATEGORY_NAME"]);
                //                                   }

                //                                   Subcatlist1 = Subcatlist1 + _stmpl_records2.ToString();
                //                                   subcount = subcount + 1;
                //                                   Rowcount = Rowcount + 1;
                //                                   if (Rowcount >=18)
                //                                       break;

                //                               }
                //                               _stmpl_records1.SetAttribute("SUBCAT_LIST1", Subcatlist1);

                //                               //if (subcount > 5)
                //                               //    _stmpl_records1.SetAttribute("IS_VIEW_MORE", true);
                //                               //else
                //                               //    _stmpl_records1.SetAttribute("IS_VIEW_MORE", false);
                //                           }
                //                           else
                //                           {
                //                               _stmpl_records1.SetAttribute("SUBCAT_LIST1", "");
                //                               Rowcount = Rowcount + 1;
                //                               //_stmpl_records1.SetAttribute("IS_VIEW_MORE", false);
                //                           }

                //                           if (Rowcount >=18)
                //                           {
                //                               //if (requrl.Contains("home.aspx") != true)
                //                               //{
                //                                   Subcatlist = Subcatlist + _stmpl_records1.ToString() + "</li>";
                //                               //}
                //                               //else
                //                               //{
                //                               //    Subcatlist = Subcatlist + _stmpl_records1.ToString();
                //                               //}
                //                               Rowcount = 0;
                //                           }
                //                           else
                //                               Subcatlist = Subcatlist + _stmpl_records1.ToString();

                //                       }
                //                       _stmpl_records.SetAttribute("IS_SUBCAT", true);
                //                       _stmpl_records.SetAttribute("SUBCAT_LIST", Subcatlist.ToString());
                //                   }
                //                   else
                //                   {
                //                       _stmpl_records.SetAttribute("IS_SUBCAT", false);
                //                   }
                //               }
                //               else
                //               {
                //                   _stmpl_records.SetAttribute("IS_SUBCAT", false);
                //               }
                //           //}
                //           //_stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME_TOP"]);
                //          // _stmpl_records.SetAttribute("TBT_CURRENTURL", currenturl);
                //           lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                //           ictrecords++;
                //       } 
                //       //objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                //   }

                //if (_Package == "TOP")
                //    _stmpl_container = _stg_container.GetInstanceOf("Top" + "\\" + "Main");
                //else
                //    _stmpl_container = _stg_container.GetInstanceOf("TopLog" + "\\" + "Main");

                _stmpl_container = _stg_container.GetInstanceOf(chkpackage + "\\" + "Main");

                _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);

                if (requrl.Contains("home.aspx") == true)
                    _stmpl_container.SetAttribute("TBT_CHK_HOME", true);
                else
                    _stmpl_container.SetAttribute("TBT_CHK_HOME", false);
                //if (_Package == "TOP")
                //{


                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                //  _stmpl_container.SetAttribute("logo_small", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/logo_small.png");
                // _stmpl_container.SetAttribute("Wagner_logo", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/Wagner-logo.png");
                //_stmpl_container.SetAttribute("wagner_tv_brackets_banner_01", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/wagner-tv-brackets-banner-01.jpg");



                if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].Equals("") && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
                {
                    string lname = GetLoginName();
                    _stmpl_container.SetAttribute("TBT_LOGIN_NAME", lname);
                    HttpContext.Current.Session["LOGIN_NAME_TOP"] = lname;
                }

                if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                {
                    if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString()) == 4)
                    {
                        string ReMailLink = "<a Href=ConfirmMessage.aspx?Result=REMAILACTIVATION class=\"linkemailre\">Re-Email Activation Link Now</a>";

                        _stmpl_container.SetAttribute("TBT_LOGIN_NAME", " Your Account Has Not Been Activated! " + ReMailLink);
                    }
                    else
                        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", "");
                }
                else
                {
                    if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
                    {
                        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", GetCompanyName() + ",");
                    }
                }

                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "" && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"]))
                {
                    string lname = GetLoginName();
                    HttpContext.Current.Session["LOGIN_NAME"] = lname;
                    HttpContext.Current.Session["LOGIN_NAME_TOP"] = lname;
                }


                //}


                string _Tbt_Order_Id = string.Empty;
                string _Tbt_Ship_URL = string.Empty;
                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                {
                    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                }
                else
                {

                    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();

                    //    HttpContext.Current.Session["ORDER_ID"] = _Tbt_Order_Id;
                }

                if ((HttpContext.Current.Session["ORDER_ID"] != null &&
                    Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) ||
                    (HttpContext.Current.Request.QueryString["ViewOrder"] != null &&
                    HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) ||
                    (HttpContext.Current.Request.QueryString["ApproveOrder"] != null &&
                    HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                {
                    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                    _Tbt_Ship_URL = "/checkout.aspx?" + EncryptSP(_Tbt_Order_Id);
                }
                else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
                {
                    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                    _Tbt_Ship_URL = "/checkout.aspx?" + EncryptSP(_Tbt_Order_Id);
                }
                else
                {
                    //_Tbt_Ship_URL = "shipping.aspx";
                    _Tbt_Ship_URL = "";
                }
                _stmpl_container.SetAttribute("TBT_ORDER_ID", _Tbt_Order_Id);
                _stmpl_container.SetAttribute("TBT_SHIP_URL", _Tbt_Ship_URL);
                _stmpl_container.SetAttribute("TBT_CURRENTURL", currenturl);
                // _stmpl_container.SetAttribute("TBWDataList", lstrecords);

                string HTMLProducts = string.Empty;
                string path = System.Configuration.ConfigurationManager.AppSettings["CDNBANNER"].ToString() + "Home" + "/";
                _stg_container1 = new StringTemplateGroup("banner", _SkinRootPath);
                try
                {
                    dataset = (DataSet)HttpContext.Current.Cache["Cache_PostLogin"];
                }
                catch (Exception ex)
                {
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                }

                _stg_container1 = new StringTemplateGroup("banner", _SkinRootPath);
               
                if (dataset != null & dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                    {
                        _stmpl_container1 = _stg_container1.GetInstanceOf(chkpackage + "\\" + "banner");
                        string image = path + dataset.Tables[0].Rows[i]["IMG_NAME"].ToString();
                        _stmpl_container1.SetAttribute("TBT_BANNER_URL", dataset.Tables[0].Rows[i]["URL"].ToString());
                        _stmpl_container1.SetAttribute("TBT_BANNER_IMAGE_NAME", image);
                        HTMLProducts += _stmpl_container1.ToString();
                    }
                }
                _stmpl_container.SetAttribute("TBT_BANNER", HTMLProducts);
               
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }


            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Top_Load = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();

            //return ProdimageRreplaceImages(sHTML);
            return sHTML;
        }

        public string ST_TOP_Category_Scroll()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string sHTML = string.Empty;
            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();
                DataTable dscatname = new DataTable();
                DataSet dscat = new DataSet();
                DataSet dssubcat = new DataSet();
                string cat_name = string.Empty;
                string searchstr = string.Empty;
                DataSet precatlist = new DataSet();
                //  DataSet dsbc = new DataSet();

                string Requesturlstring = HttpContext.Current.Request.Url.ToString().ToLower();
                if (Requesturlstring.Contains("ct.aspx") == true || Requesturlstring.Contains("pl.aspx") == true || Requesturlstring.Contains("ps.aspx") == true || Requesturlstring.Contains("bb.aspx") == true)
                {
                    if (HttpContext.Current.Request.QueryString["cid"] != null && Requesturlstring.Contains("ps.aspx") == false && Requesturlstring.Contains("bb.aspx") == false)
                    {
                        string _catId = "";
                        if (HttpContext.Current.Request.QueryString["cid"] != null)
                            _catId = HttpContext.Current.Request.QueryString["cid"];
                        if (HttpContext.Current.Session["MainMenuClick"] != null)
                        {
                            dscatname = (DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["MainCategory"];
                        }
                        if (Requesturlstring.Contains("pl.aspx") == true)
                        {
                            if (HttpContext.Current.Request.QueryString["value"] != null)
                                cat_name = HttpContext.Current.Request.QueryString["value"].ToString();

                            if (HttpContext.Current.Session["LHSAttributes"] != null)
                            {
                                dssubcat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                                // if( dssubcat != null && dssubcat.Tables[0].Rows.Count > 0)
                                //    HttpContext.Current.Session["precatlist"] = dssubcat;
                            }
                        }

                        //DataRow[] row = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + _catId + "'");
                        DataSet mainds = (DataSet)HttpContext.Current.Application["key_MainCategory"];
                        DataRow[] row = mainds.Tables[0].Select("CATEGORY_ID='" + _catId + "'");
                        if (row.Length > 0)
                        {
                            dscat.Tables.Add(row.CopyToDataTable());
                        }
                    }
                    else if (Requesturlstring.Contains("ps.aspx") == true)
                    {
                        if (HttpContext.Current.Request.QueryString["searchstr"] != null)
                            searchstr = HttpContext.Current.Request.QueryString["searchstr"];
                        if (HttpContext.Current.Session["LHSAttributes"] != null)
                        {
                            dssubcat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                        }
                        // dsbc =  (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                    }

                    else if (Requesturlstring.Contains("bb.aspx") == true)
                    {
                        if (HttpContext.Current.Session["LHSAttributes"] != null)
                        {
                            dssubcat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                        }
                    }
                }
                else
                {


                    //  dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");
                    if (HttpContext.Current.Application["key_MainCategory"] != null)
                    {
                        dsrecords = (DataSet)HttpContext.Current.Application["key_MainCategory"];
                    }
                    if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
                        return "";
                }


                if (HttpContext.Current.Session["precatlist"] != null)
                    precatlist = (DataSet)HttpContext.Current.Session["precatlist"];

                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                string chkpackage = "TopScrollCategory";
                string celltop = "";// 
                if (Requesturlstring.Contains("ct.aspx") == true)
                {
                    lstrecords = new TBWDataList[dscatname.Rows.Count + 1];
                    celltop = chkpackage + "\\cell";
                }
                else if ((Requesturlstring.Contains("pl.aspx") == true
                    || Requesturlstring.Contains("bb.aspx") == true
                    || Requesturlstring.Contains("ps.aspx") == true) && dssubcat != null && dssubcat.Tables.Count > 0 && dssubcat.Tables[0].Rows.Count > 0)
                {
                    lstrecords = new TBWDataList[dssubcat.Tables[0].Rows.Count + 1];
                    celltop = chkpackage + "\\cell1";
                }

                else
                {
                    if (dsrecords != null && dsrecords.Tables.Count > 0 && dsrecords.Tables[0].Rows.Count > 0)
                    {
                        lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];
                        celltop = chkpackage + "\\cell";
                    }
                    else
                        return "";
                }



                int ictrecords = 0;

                // string chkpackage = "TopScrollCategory";
                string dr_cat_id_upper = string.Empty;


                string currenturl = objHelperService.AddDomainname();


                // string celltop = chkpackage + "\\cell";
                string strdatalist = string.Empty;
                if (Requesturlstring.Contains("ct.aspx") == true || Requesturlstring.Contains("pl.aspx") == true || Requesturlstring.Contains("ps.aspx") == true || Requesturlstring.Contains("bb.aspx") == true)
                {
                    if (Requesturlstring.Contains("ct.aspx") == true)
                    {
                        for (int i = 0; i < dscatname.Rows.Count; i++)
                        {
                            _stmpl_records = _stg_records.GetInstanceOf(celltop);
                            _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dscatname.Rows[i][0].ToString());
                            string rewriteurl = objHelperServices.SimpleURL_Str(dscat.Tables[0].Rows[0][0].ToString() + "////" + dscatname.Rows[i][0].ToString(), "pl.aspx", true);
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", rewriteurl);
                            _stmpl_records.SetAttribute("TBT_PAGENAME", "pl");
                            _stmpl_records.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                            ictrecords++;
                        }
                    }
                    //else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true &&  HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx") == false)
                    //{
                    //    if (dssubcat != null && dssubcat.Tables[0].Rows.Count > 0)
                    //    {
                    //        foreach (DataRow drsubcat in dssubcat.Tables[0].Rows)
                    //        {
                    //            _stmpl_records = _stg_records.GetInstanceOf(celltop);
                    //            _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", drsubcat["CATEGORY_NAME"].ToString());
                    //            string rewriteurl = objHelperServices.SimpleURL_Str(dscatname.Rows[0][0].ToString() + "////" + cat_name + "////" + drsubcat["CATEGORY_NAME"].ToString(), "pl.aspx", true);
                    //            _stmpl_records.SetAttribute("TBT_REWRITEURL", rewriteurl);
                    //            _stmpl_records.SetAttribute("TBT_PAGENAME", "pl");
                    //            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    //            ictrecords++;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        for (int i = 0; i < dscatname.Rows.Count; i++)
                    //        {
                    //            _stmpl_records = _stg_records.GetInstanceOf(celltop);
                    //            _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dscatname.Rows[i][0].ToString());
                    //            string rewriteurl = objHelperServices.SimpleURL_Str(dscatname.Rows[i][2].ToString() + "////" + dscatname.Rows[i][0].ToString(), "pl.aspx", true);
                    //            _stmpl_records.SetAttribute("TBT_REWRITEURL", rewriteurl);
                    //            _stmpl_records.SetAttribute("TBT_PAGENAME", "pl");
                    //            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    //            ictrecords++;
                    //        }
                    //    }
                    //}
                    else if (Requesturlstring.Contains("ps.aspx") == true || Requesturlstring.Contains("pl.aspx") == true || Requesturlstring.Contains("bb.aspx") == true)
                    {
                        if (dssubcat != null && dssubcat.Tables[0].Rows.Count > 0)
                        {
                            string eapath = "";

                            string TBW_ATTRIBUTE_VALUE = string.Empty;
                            foreach (DataRow drsubcat in dssubcat.Tables[0].Rows)
                            {

                                _stmpl_records = _stg_records.GetInstanceOf(celltop);
                                string brandname = string.Empty;
                                if (dssubcat.Tables[0].TableName.Contains("Category"))
                                {
                                    _stmpl_records.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                                    // _stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                    _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(drsubcat["CATEGORY_ID"].ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                                    string url1 = objHelperServices.SimpleURL_Str(drsubcat["Category_Name"].ToString(), "", false);
                                    string strurl = string.Empty;
                                    string strrawurl = HttpContext.Current.Request.RawUrl;
                                    string[] checkurl = strrawurl.Split('/');

                                    if (strrawurl.Contains("/pl/"))
                                    {
                                        strurl = "/" + url1 + "/" + checkurl[checkurl.Length - 4] + "/" + checkurl[checkurl.Length - 3] + "/" + "pl/";
                                        _stmpl_records.SetAttribute("TBW_FORMNAME", "pl.aspx?");
                                    }
                                    else if (strrawurl.Contains("/bb/"))
                                    {
                                        strurl = "/" + url1 + "/" + checkurl[checkurl.Length - 5] + "/" + checkurl[checkurl.Length - 4] + "/" + checkurl[checkurl.Length - 3] + "/" + "bb/";
                                        _stmpl_records.SetAttribute("TBW_FORMNAME", "bb.aspx?");
                                    }
                                    else if (strrawurl.Contains("/ps/"))
                                    {
                                        strurl = "/" + url1 + "/" + checkurl[checkurl.Length - 3] + "/" + "ps/";
                                        _stmpl_records.SetAttribute("TBW_FORMNAME", "ps.aspx?");
                                    }
                                    _stmpl_records.SetAttribute("TBT_REWRITEURL", strurl);


                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", drsubcat["Category_Name"]);
                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(drsubcat["Category_Name"].ToString()));
                                    TBW_ATTRIBUTE_VALUE = drsubcat["Category_Name"].ToString();
                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(drsubcat["brandvalue"].ToString()));


                                    //if (HttpContext.Current.Request.QueryString["path"] != null)
                                    //{
                                    //    _stmpl_records.SetAttribute("EA_PATH$", HttpContext.Current.Request.QueryString["path"].ToString());
                                    //}
                                    //else if (HttpContext.Current.Session["EA"] != null) { 

                                    //}
                                    //  brandname = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_BRAND").ToString();
                                    //  if (brandname != "")
                                    //  {
                                    //     brandname = brandname + ":";
                                    //  }
                                    //  _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(drsubcat["SearchString"].ToString()));


                                }


                                else
                                {
                                    //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                    _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(drsubcat["CATEGORY_ID"].ToString()));

                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(drsubcat[0].ToString()));
                                    TBW_ATTRIBUTE_VALUE = drsubcat[0].ToString();
                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(drsubcat["brandvalue"].ToString()));
                                    brandname = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_BRAND").ToString();
                                    if (brandname != string.Empty)
                                    {
                                        brandname = brandname + ":";
                                    }
                                    string url1 = objHelperServices.SimpleURL_Str(drsubcat["Category_Name"].ToString(), "", false);
                                    string strurl = string.Empty;
                                    string strrawurl = HttpContext.Current.Request.RawUrl;

                                    string[] checkurl = strrawurl.Split('/');
                                    if (strrawurl.Contains("/pl/"))
                                    {
                                        strurl = "/" + url1 + "/" + checkurl[checkurl.Length - 4] + "/" + checkurl[checkurl.Length - 3] + "/" + "pl/";
                                        _stmpl_records.SetAttribute("TBW_FORMNAME", "pl.aspx?");
                                    }
                                    else if (strrawurl.Contains("/bb/"))
                                    {
                                        strurl = "/" + url1 + "/" + checkurl[checkurl.Length - 5] + "/" + checkurl[checkurl.Length - 4] + "/" + checkurl[checkurl.Length - 3] + "/" + "bb/";
                                        _stmpl_records.SetAttribute("TBW_FORMNAME", "bb.aspx?");
                                    }
                                    else if (strrawurl.Contains("/ps/"))
                                    {
                                        strurl = "/" + url1 + "/" + checkurl[checkurl.Length - 3] + "/" + "ps/";
                                        _stmpl_records.SetAttribute("TBW_FORMNAME", "ps.aspx?");
                                    }


                                    _stmpl_records.SetAttribute("TBT_REWRITEURL", strurl);

                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(drsubcat["SearchString"].ToString()));
                                    _stmpl_records.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());


                                }
                                if (HttpContext.Current.Session["EA"] != null)
                                {
                                    if (HttpContext.Current.Session["EA"].ToString() != "AllProducts////WESAUSTRALASIA////BigTop Store" || HttpContext.Current.Session["EA_URL"] == null)
                                    {
                                        _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));

                                        _stmpl_records.SetAttribute("ORG_EA_PATH", HttpContext.Current.Session["EA"].ToString());
                                    }
                                    else if (HttpContext.Current.Session["EA"].ToString() == "AllProducts////WESAUSTRALASIA////BigTop Store" && HttpContext.Current.Session["EA_URL"] != null)
                                    {

                                        _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA_URL"].ToString())));

                                        _stmpl_records.SetAttribute("ORG_EA_PATH", HttpContext.Current.Session["EA_URL"].ToString());
                                    }

                                }
                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_TYPE", dssubcat.Tables[0].TableName.ToString());
                                string TBW_ATTRIBUTE_TYPE = _stmpl_records.GetAttribute("TBW_ATTRIBUTE_TYPE").ToString().ToUpper();
                                string TBW_ATTRIBUTE_VALUE_NEW = drsubcat[0].ToString();
                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_INPUT", drsubcat[0].ToString().Replace("$", "").Replace(" ", "").Replace("\"", "dq"));


                                //if (_tsm != string.Empty)
                                //{
                                //    _BCEAPath = _BCEAPath.Replace(_tsm, HttpUtility.UrlEncode(_tsm));
                                //}


                                string TBW_ATTRIBUTE_NAME_NEW = TBW_ATTRIBUTE_VALUE;
                                if (TBW_ATTRIBUTE_TYPE == "MODEL")
                                {

                                    if (brandname != "")
                                    {
                                        TBW_ATTRIBUTE_NAME_NEW = brandname + ":" + TBW_ATTRIBUTE_VALUE_NEW.Replace(brandname, "");
                                    }
                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", drsubcat[0].ToString().Replace(brandname, ""));
                                }
                                else if (!dssubcat.Tables[0].TableName.Contains("Category"))
                                {
                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", drsubcat[0].ToString());
                                }

                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME_NEW", TBW_ATTRIBUTE_NAME_NEW);
                                _stmpl_records.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());

                                //  objHelperServices.SimpleURL(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_VALUE, "");


                                //string rewriteurl = objHelperServices.SimpleURL_Str(new_eapth, "ps.aspx", true);
                                //_stmpl_records.SetAttribute("TBT_REWRITEURL", rewriteurl);
                                //_stmpl_records.SetAttribute("TBT_PAGENAME", "ps");
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                ictrecords++;
                            }
                        }
                        else
                        {
                            sHTML = "";
                            return sHTML;
                        }
                    }

                }
                else
                {

                    foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                    {
                        dr_cat_id_upper = dr["Category_id"].ToString().ToUpper();
                        if (dr_cat_id_upper != WesNewsCategoryId && !(dr_cat_id_upper.Contains("SPF-")))
                        {

                            _stmpl_records = _stg_records.GetInstanceOf(celltop);
                            string url = string.Empty;
                            //if (dr["URL_RW_PATH"].ToString().Contains("http") == false)
                            //{

                            //    url = currenturl + dr["URL_RW_PATH"].ToString();

                            //}
                            //else
                            //{

                            url = currenturl + dr["URL_RW_PATH"].ToString();
                            //}
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", url);
                            _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME_TOP"].ToString().Replace("<br/>", " "));
                            _stmpl_records.SetAttribute("TBT_PAGENAME", "ct");
                            _stmpl_records.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                            ictrecords++;
                        }

                    }
                }

                _stmpl_container = _stg_container.GetInstanceOf(chkpackage + "\\" + "Main");

                //  _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);
                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);

                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Top_Category_Scroll = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();

            return sHTML;
        }

        public string ST_Top_Load_Mobile()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string sHTML = string.Empty;
            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");
                dsrecords = (DataSet)HttpContext.Current.Application["key_MainCategory"];

                if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
                    return "";


                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];



                int ictrecords = 0;

                string chkpackage = "TopMobile";
                string dr_cat_id_upper = string.Empty;


                string currenturl = objHelperService.AddDomainname();


                string celltop = chkpackage + "\\cell";

                foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                {
                    dr_cat_id_upper = dr["Category_id"].ToString().ToUpper();
                    if (dr_cat_id_upper != WesNewsCategoryId && !(dr_cat_id_upper.Contains("SPF-")))
                    {

                        _stmpl_records = _stg_records.GetInstanceOf(celltop);
                        string url = string.Empty;
                        //if (dr["URL_RW_PATH"].ToString().Contains("http") == false)
                        //{

                        //    url = currenturl + dr["URL_RW_PATH"].ToString();

                        //}
                        //else
                        //{

                        url = currenturl + dr["URL_RW_PATH"].ToString();
                        //}
                        _stmpl_records.SetAttribute("TBT_REWRITEURL", url);
                        _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME_TOP"]);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }

                }

                _stmpl_container = _stg_container.GetInstanceOf(chkpackage + "\\" + "Main");

                _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);


                ////if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].Equals("") && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                ////{
                ////    _stmpl_container.SetAttribute("TBT_LOGIN_NAME", GetLoginName());
                ////}

                ////if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                ////{
                ////    if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString()) == 4)
                ////    {
                ////        string ReMailLink = "<a Href=ConfirmMessage.aspx?Result=REMAILACTIVATION class=\"linkemailre\">Re-Email Activation Link Now</a>";

                ////        _stmpl_container.SetAttribute("TBT_LOGIN_NAME", " Your Account Has Not Been Activated! " + ReMailLink);
                ////    }
                ////    else
                ////        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", "");
                ////}
                ////else
                ////{
                ////    if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                ////    {
                ////        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", GetCompanyName() + ",");
                ////    }
                ////}

                ////if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "" && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                ////    HttpContext.Current.Session["LOGIN_NAME"] = GetLoginName();


                //////}


                ////string _Tbt_Order_Id = string.Empty;
                ////string _Tbt_Ship_URL = string.Empty;
                ////int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                ////if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                ////{
                ////    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                ////}
                ////else
                ////{

                ////    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();

                ////    //    HttpContext.Current.Session["ORDER_ID"] = _Tbt_Order_Id;
                ////}

                ////if ((HttpContext.Current.Session["ORDER_ID"] != null &&
                ////    Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) ||
                ////    (HttpContext.Current.Request.QueryString["ViewOrder"] != null &&
                ////    HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) ||
                ////    (HttpContext.Current.Request.QueryString["ApproveOrder"] != null &&
                ////    HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                ////{
                ////    _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                ////}
                ////else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
                ////{
                ////    _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                ////}
                ////else
                ////{
                ////    _Tbt_Ship_URL = "shipping.aspx";
                ////}
                ////_stmpl_container.SetAttribute("TBT_ORDER_ID", _Tbt_Order_Id);
                ////_stmpl_container.SetAttribute("TBT_SHIP_URL", _Tbt_Ship_URL);
                ////_stmpl_container.SetAttribute("TBT_CURRENTURL", currenturl);
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Top_Load_Mobile = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return sHTML;
        }


        public string ST_RECENT_COOKIE_PRODUCT(string currentpfid)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string sHTML = string.Empty;

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_recordstemp = new StringTemplate();

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

                string userid = HttpContext.Current.Session["USER_ID"].ToString();

                if (userid == string.Empty)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];

                HttpCookie readcookie = HttpContext.Current.Request.Cookies["recentpid"];
                string currentpfid1 = currentpfid;
                string cokkieval = readcookie["Product"].ToString();
                if (currentpfid1 != "")
                {
                    cokkieval = cokkieval.Replace(currentpfid1, "");
                }
                string[] sptcookieval = cokkieval.Split('|');

                Int64 PID1 = 0;
                Int64 FID1 = 0;

                string[] pfid = sptcookieval[0].Split(',');
                PID1 = Convert.ToInt64(pfid[0]);
                FID1 = Convert.ToInt64(pfid[1]);
                string param2 = string.Empty;
                string cookieparam = PID1 + "," + FID1;
                for (int i = 1; i < sptcookieval.Length; i++)
                {

                    param2 = param2 + "," + sptcookieval[i];

                    cookieparam = cookieparam + "|" + sptcookieval[i];
                    if (i == 3)
                    {


                        HttpCookie _userActivity = new HttpCookie("recentpid");
                        _userActivity["Product"] = cookieparam;


                        //Adding cookies to current web response
                        HttpContext.Current.Response.Cookies.Add(_userActivity);
                        break;
                    }
                }

                //DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WAG 3,0," + userid);
                DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_COOKIES_PRODUCT_WAG " + PID1 + "," + FID1 + "" + param2 + "," + userid);
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

                //string   stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                //int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];

                //   bool isenable = IsEcomenabled();

                int ictrecords = 0;
                string cellnpln = _Package + "\\cell";

                foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
                {
                    _stmpl_records = _stg_records.GetInstanceOf(cellnpln);

                    string[] catpath = dr["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                    string URL_RW_PATH = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr["FAMILY_ID"] + "////" + dr["product_ID"] + "=" + dr["Code"] + "////" + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + dr["FAMILY_name"], "pd.aspx", true);
                    string URL_RW_PATH_FAMILY = objHelperService.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr["FAMILY_ID"] + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + dr["FAMILY_name"], "fl.aspx", true);

                    //    dr["URL_RW_PATH_FAMILY"] = objHelperService.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"] + "////" + dr["FAMILY_ID"] + "=" + dr["FAMILY_name"], "fl.aspx", true, "", false, true);

                    _stmpl_records.SetAttribute("TBT_REWRITEURL", URL_RW_PATH);
                    _stmpl_records.SetAttribute("TBT_REWRITEURL_FAMILY", URL_RW_PATH_FAMILY);
                    _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString()));
                    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);
                    // _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["FAMILY_NAME"].ToString().Replace('"',' ') );
                    _stmpl_records.SetAttribute("TBT_DESCRIPTION1", dr["DESCRIPTION"].ToString());
                    _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"]);
                    _stmpl_records.SetAttribute("TBT_YOURCOST", dr["COSt"].ToString());
                    //  _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);
                    // _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                    // _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);
                    //_stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"].ToString());
                    // _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);
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
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Recent_cookie_product = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return objHelperService.ProdimageRreplaceImages(sHTML, _Package);
        }

        public string ST_POPULAR_PRODUCT()
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

                // string userid = HttpContext.Current.Session["USER_ID"].ToString();
                string userid = string.Empty;
                if (userid == string.Empty)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];

                //   DataSet tmpds = objHelperDB.GetDataSetDB("exec [STP_TBWC_PICK_POPULAR_PRODUCT] 0," + userid);




                // DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WAG_FAMPRO 16,0," + userid);
                DataSet tmpds = new DataSet();
                // JObject o1;
                // tmpds = (DataSet) HttpContext.Current.Application["key_PopularProducts"];

                //   tmpds =EasyAsk.GetCategoryAndBrand_Applicationstart("PopularProducts"); ;
                tmpds = (DataSet)HttpContext.Current.Cache["Cache_POPULARPRODUCT"];
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {


                }
                else
                    return "";

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                //int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];

                //  bool isenable = IsEcomenabled();
                bool isenable = true;
                int ictrecords = 0;
                string URL_RW_PATH = string.Empty;
                string cellnewnp = "PopularProduct\\cellnew";
                int e = 0;
                string prevprodid = string.Empty;
                DataSet tmpdsPrice = objHelperDB.GetProductPriceEA("", tmpds.Tables["AllProducts"].Rows[0][0].ToString(), userid, "New");
                // DataView view = new DataView(tmpds.Tables[0]);
                // DataTable distinctValues = view.ToTable(true, "product_id");
                //  DataTable distinctValues = tmpdsPrice.Tables[0];
                foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
                {



                    DataRow[] foundRows = tmpdsPrice.Tables[0].Select("Product_id = '" + dr["Product_id"].ToString() + "'");
                    DataRow dr1 = null;
                    if (foundRows.Length > 0)
                    {
                        dr1 = foundRows[0];
                    }
                    if (dr1 != null)
                    {
                        if (prevprodid != dr["PRODUCT_ID"].ToString())
                        {
                            prevprodid = dr["PRODUCT_ID"].ToString();
                            e = e + 1;
                            _stmpl_records = _stg_records.GetInstanceOf(cellnewnp);

                            // remove tostring

                            // URL_RW_PATH=   objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"] + "////" + dr["FAMILY_ID"] + "=" + dr["FAMILY_name"] + "////" + dr["product_ID"] + "=" + dr["Code"], "pd.aspx", true, "");
                            string[] catpath = dr["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                            URL_RW_PATH = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr["FAMILY_ID"] + "////" + dr["product_ID"] + "=" + dr["Code"] + "////" + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////BigTop Store////" + dr["FAMILY_name"], "pd.aspx", true);

                            string desc = dr["DESCRIPTION"].ToString();
                            string prodImage = string.Empty;

                            if (dr["TWEB Image1"].ToString() != null && dr["TWEB Image1"].ToString() != String.Empty && dr["TWEB Image1"].ToString() != "")
                            {
                                prodImage = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + dr["TWEB Image1"].ToString().Replace("_TH", "_images_200").Replace("\\", "/");
                                //String strfile1 = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + dr["TWEB Image1"].ToString().Replace("\\", "/");
                                //bool ImageExits = objHelperServices.CheckImageExistCDN(strfile1);
                                //if (ImageExits)
                                //    prodImage = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + dr["TWEB Image1"].ToString().Replace("_TH", "_images_200").Replace("\\", "/");
                                //else
                                //    prodImage = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";
                            }
                            else
                            {
                                prodImage = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";
                            }

                            _stmpl_records.SetAttribute("TBT_REWRITEURL", URL_RW_PATH);
                            _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", prodImage);
                            _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);
                            // _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["FAMILY_NAME"].ToString().Replace('"', ' '));
                            // _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"]);
                            if (desc.Length > 120) // && _ViewType == "GV"
                            {
                                desc = desc.Substring(0, 120).ToString();
                                desc = desc.Substring(0, desc.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", desc + "...");
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", desc);
                            }
                            _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"]);



                            if (tmpdsPrice.Tables[0] != null)
                            {
                                if (tmpdsPrice.Tables[0].Rows[0]["price"].ToString() != string.Empty)
                                {

                                    //dr1["PRODUCT_PRICE"] = dr["price"].ToString();
                                    //  string tmpprice = string.Empty;
                                    //  tmpprice = objHelperServices.CheckPriceValueDecimal(dr1["price"].ToString());
                                    //  dr["COSt"] = tmpprice;
                                    dr["COSt"] = objHelperServices.CheckPriceValueDecimal(dr1["price"].ToString());
                                }
                            }
                            //            tmpprice = objHelperServices.CheckPriceValueDecimal(dr["prod_price_1"].ToString());
                            _stmpl_records.SetAttribute("TBT_YOURCOST", dr["COSt"]);

                            //_stmpl_records.SetAttribute("TBT_YOURCOST", objHelperServices.CheckPriceValueDecimal(dr["prod_price_1"].ToString()));
                            _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);
                            // _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                            _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);
                            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"]);
                            _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);
                            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"]);
                            _stmpl_records.SetAttribute("CATEGORY_ID", dr["CATEGORY_ID"].ToString());
                            string PRODUCT_EA_PATH = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + dr["FAMILY_ID"].ToString();
                            _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(PRODUCT_EA_PATH)));

                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                            ictrecords++;
                            if (e == 16)
                            {
                                break;
                            }
                        }
                    }
                }


                _stmpl_container = _stg_container.GetInstanceOf("PopularProduct" + "\\" + "Mainnew");



                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return objHelperService.ProdimageRreplaceImages(sHTML, _Package);
        }

        public string ST_POPULAR_SEARCH()
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

                // TBWDataList[] lstrows = new TBWDataList[0];

                // StringTemplateGroup _stg_container1 = null;
                // StringTemplateGroup _stg_records1 = null;
                //   TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                //   DataSet dscat = new DataSet();
                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                // DataRow[] drs = null;

                // string userid = HttpContext.Current.Session["USER_ID"].ToString();
                string userid = string.Empty;
                if (userid == string.Empty)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];

                //   DataSet tmpds = objHelperDB.GetDataSetDB("exec [STP_TBWC_PICK_POPULAR_PRODUCT] 0," + userid);




                // DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WAG_FAMPRO 16,0," + userid);
                DataSet tmpds = new DataSet();
                // JObject o1;
                // tmpds = (DataSet) HttpContext.Current.Application["key_PopularProducts"];

                //   tmpds =EasyAsk.GetCategoryAndBrand_Applicationstart("PopularProducts"); ;
                tmpds = (DataSet)EasyAsk.GetCategoryAndBrand_Applicationstart("PopularSearch");
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {


                }
                else
                    return "";

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                //int ictrows = 0;


                TBWDataList[] lstrecords = new TBWDataList[0];



                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];

                //  bool isenable = IsEcomenabled();
                bool isenable = true;
                int ictrecords = 0;
                string URL_RW_PATH = string.Empty;
                string cellnewnp = "PopularSearch\\cell";
                int e = 0;
                string prevprodid = string.Empty;

                // DataView view = new DataView(tmpds.Tables[0]);
                // DataTable distinctValues = view.ToTable(true, "product_id");
                //  DataTable distinctValues = tmpdsPrice.Tables[0];
                foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
                {
                    e = e + 1;
                    _stmpl_records = _stg_records.GetInstanceOf(cellnewnp);



                    URL_RW_PATH = objHelperService.SimpleURL_Str(dr["FAMILY_NAME"].ToString(), "ps.aspx", true);


                    _stmpl_records.SetAttribute("TBT_REWRITEURL", URL_RW_PATH + "/ps/");
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    //          string title = textInfo.ToTitleCase(dr["FAMILY_NAME"].ToString().ToLower());
                    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"].ToString().ToLower());
                    // _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["FAMILY_NAME"].ToString().Replace('"', ' '));
                    // _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"]);



                    //_stmpl_records.SetAttribute("TBT_YOURCOST", dr["COSt"]);
                    //_stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);
                    //// _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                    //_stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);
                    //_stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"]);
                    //_stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);
                    //_stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"]);


                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                    if (e == 50)
                    {
                        break;
                    }
                }




                _stmpl_container = _stg_container.GetInstanceOf("PopularSearch" + "\\" + "Mainnew");

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return objHelperService.ProdimageRreplaceImages(sHTML, _Package);
        }

        public string ST_HOME_PRODUCT()
        {
            string sHTML = string.Empty;

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_recordstemp = new StringTemplate();
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                // TBWDataList[] lstrows = new TBWDataList[0];

                // StringTemplateGroup _stg_container1 = null;
                // StringTemplateGroup _stg_records1 = null;
                //   TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                //   DataSet dscat = new DataSet();
                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                // DataRow[] drs = null;

                // string userid = HttpContext.Current.Session["USER_ID"].ToString();
                string userid = string.Empty;
                if (userid == string.Empty)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];

                //   DataSet tmpds = objHelperDB.GetDataSetDB("exec [STP_TBWC_PICK_POPULAR_PRODUCT] 0," + userid);




                // DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WAG_FAMPRO 16,0," + userid);
                DataSet tmpds = new DataSet();
               
                tmpds = (DataSet)HttpContext.Current.Cache["Cache_HOMEPRODUCT"];
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {


                }
                else
                    return "";

                DataSet subds = (DataSet)HttpContext.Current.Cache["key_SubCategoryAll"];

                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                
                StringTemplate _stmpl_container1 = null;
                _stg_container1 = new StringTemplateGroup("main", _SkinRootPath);
                _stmpl_container1 = _stg_container1.GetInstanceOf("HomeProduct\\Main1");
                _stmpl_container1.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                sHTML += _stmpl_container1.ToString();

                //  bool isenable = IsEcomenabled();
                bool isenable = true;
               
                string URL_RW_PATH = string.Empty;
                string cellnewnp = "HomeProduct\\cell";
                int e = 0;
                string prevprodid = string.Empty;
                DataSet tmpdsPrice = objHelperDB.GetProductPriceEA("", tmpds.Tables["AllProducts"].Rows[0][0].ToString(), userid, "New");

                DataTable dt = subds.Tables[0];

                foreach (DataRow drval in dt.Rows)//For Records
                {
                    if (drval["CATEGORY_ID"] != null && drval["CATEGORY_ID"].ToString() != "SPF-BIGTOP" && drval["CATEGORY_ID"].ToString() != "CLONE3-WN1098WH")
                    { 
                        DataRow[] foundRows1 = tmpds.Tables[0].Select("Category_id = '" + drval["CATEGORY_ID"].ToString() + "'");
                    //objErrorHandler.CreateLog("Category_id " + drval["CATEGORY_ID"].ToString() + " count " + foundRows1.Length);
                    // DataRow drval = null;
                    //int ittrval = 0;
                    TBWDataList[] lstrecords = new TBWDataList[0];
                    lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];
                    int ictrecords = 0;
                    string HTMLProducts = string.Empty;
                    _stmpl_container = _stg_container.GetInstanceOf("HomeProduct" + "\\" + "Main");
                    _stmpl_container.SetAttribute("TBT_CATEGORY_NAME", drval["CATEGORY_NAME"].ToString().ToUpper());
                     string URL_RW_PATH1 = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + drval["EA_PATH"] , "pl.aspx", true);
                    _stmpl_container.SetAttribute("TBT_CATEGORY_URL", URL_RW_PATH1);
                        foreach (DataRow dr in foundRows1)//For Records
                    {



                        DataRow[] foundRows = tmpdsPrice.Tables[0].Select("Product_id = '" + dr["Product_id"].ToString() + "'");
                        DataRow dr1 = null;
                        if (foundRows.Length > 0)
                        {
                            dr1 = foundRows[0];
                        }
                        if (dr1 != null && drval["CATEGORY_ID"] != null && drval["CATEGORY_ID"].ToString() != "SPF-BIGTOP" && drval["CATEGORY_ID"].ToString() != "CLONE3-WN1098WH")
                        {
                            //if (prevprodid != dr["PRODUCT_ID"].ToString())
                            //{
                                prevprodid = dr["PRODUCT_ID"].ToString();
                                e = e + 1;
                                _stmpl_records = _stg_records.GetInstanceOf(cellnewnp);

                                // URL_RW_PATH=   objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"] + "////" + dr["FAMILY_ID"] + "=" + dr["FAMILY_name"] + "////" + dr["product_ID"] + "=" + dr["Code"], "pd.aspx", true, "");
                                string[] catpath = dr["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                    //objErrorHandler.CreateLog(dr["CATEGORY_PATH"].ToString());
                                URL_RW_PATH = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr["FAMILY_ID"] + "////" + dr["product_ID"] + "=" + dr["Code"] + "////" + (catpath.Length >= 3 ? catpath[2] : "") + (catpath.Length >= 4 ? "////" + catpath[3] : "")  + "////" + dr["FAMILY_name"], "pd.aspx", true);

                                string desc = dr["DESCRIPTION"].ToString();
                                string prodImage = string.Empty;

                                if (dr["TWEB Image1"].ToString() != null && dr["TWEB Image1"].ToString() != String.Empty && dr["TWEB Image1"].ToString() != "")
                                {
                                    prodImage = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + dr["TWEB Image1"].ToString().Replace("_TH", "_images_200").Replace("\\", "/");
                                    //String strfile1 = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + dr["TWEB Image1"].ToString().Replace("\\", "/");
                                    //bool ImageExits = objHelperServices.CheckImageExistCDN(strfile1);
                                    //if (ImageExits)
                                    //    prodImage = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + dr["TWEB Image1"].ToString().Replace("_TH", "_images_200").Replace("\\", "/");
                                    //else
                                    //    prodImage = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";
                                }
                                else
                                {
                                    prodImage = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";
                                }

                                _stmpl_records.SetAttribute("TBT_REWRITEURL", URL_RW_PATH);
                                _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", prodImage);
                                _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);
                                // _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["FAMILY_NAME"].ToString().Replace('"', ' '));
                                // _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"]);
                                if (desc.Length > 120) // && _ViewType == "GV"
                                {
                                    desc = desc.Substring(0, 120).ToString();
                                    desc = desc.Substring(0, desc.LastIndexOf(" "));
                                    _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", desc + "...");
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", desc);
                                }
                                _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"]);



                                if (tmpdsPrice.Tables[0] != null)
                                {
                                    if (tmpdsPrice.Tables[0].Rows[0]["price"].ToString() != string.Empty)
                                    {

                                        //dr1["PRODUCT_PRICE"] = dr["price"].ToString();
                                        //  string tmpprice = string.Empty;
                                        //  tmpprice = objHelperServices.CheckPriceValueDecimal(dr1["price"].ToString());
                                        //  dr["COSt"] = tmpprice;
                                        dr["COSt"] = objHelperServices.CheckPriceValueDecimal(dr1["price"].ToString());
                                    }
                                }
                                //            tmpprice = objHelperServices.CheckPriceValueDecimal(dr["prod_price_1"].ToString());
                                _stmpl_records.SetAttribute("TBT_YOURCOST", dr["COSt"]);

                                //_stmpl_records.SetAttribute("TBT_YOURCOST", objHelperServices.CheckPriceValueDecimal(dr["prod_price_1"].ToString()));
                                _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);
                                // _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                                _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);
                                _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"]);
                                _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);
                                _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"]);
                                _stmpl_records.SetAttribute("CATEGORY_ID", dr["CATEGORY_ID"].ToString());
                                string PRODUCT_EA_PATH = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + dr["FAMILY_ID"].ToString();
                                _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(PRODUCT_EA_PATH)));

                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                ictrecords++;
                                //HTMLProducts += _stmpl_records.ToString();
                                //if (e == 4)
                                //{
                                //    break;
                                //}
                           // }
                        }
                        //ittrval++;
                        //if (ittrval == 4)
                        //    break;
                    }

                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    //_stmpl_container.SetAttribute("TBWDataListValue", HTMLProducts);
                    _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                    sHTML += _stmpl_container.ToString();
                }
                }

                


                //_stmpl_container.SetAttribute("TBWDataList", lstrecords);
                //_stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                //sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return objHelperService.ProdimageRreplaceImages(sHTML, _Package);
        }


        public string ST_NewProduct_Load()
        {
            string sHTML = string.Empty;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplate _stmpl_container = null;
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stmpl_container = _stg_container.GetInstanceOf("NewProduct\\Main");
                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());

                sHTML = _stmpl_container.ToString();


            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_NewProduct_Load = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return objHelperService.ProdimageRreplaceImages(sHTML, _Package);
        }

        public string ST_NewProductLogNav_Load(DataSet tmpds, HttpContext ctx)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
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

                string userid = ctx.Session["USER_ID"].ToString();

                if (userid == string.Empty)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];

                //DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WAG 3,0," + userid);
                tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WAG 3,0," + userid);
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

                //   bool isenable = IsEcomenabled();

                int ictrecords = 0;
                string cellnpln = _Package + "\\cell";

                foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
                {
                    _stmpl_records = _stg_records.GetInstanceOf(cellnpln);

                    string[] catpath = dr["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                    string URL_RW_PATH = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr["FAMILY_ID"] + "////" + dr["product_ID"] + "=" + dr["Code"] + "////" + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + dr["FAMILY_name"], "pd.aspx", true);
                    string URL_RW_PATH_FAMILY = objHelperService.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr["FAMILY_ID"] + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + dr["FAMILY_name"], "fl.aspx", true);

                    //    dr["URL_RW_PATH_FAMILY"] = objHelperService.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"] + "////" + dr["FAMILY_ID"] + "=" + dr["FAMILY_name"], "fl.aspx", true, "", false, true);

                    _stmpl_records.SetAttribute("TBT_REWRITEURL", URL_RW_PATH);
                    _stmpl_records.SetAttribute("TBT_REWRITEURL_FAMILY", URL_RW_PATH_FAMILY);
                    _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString()));
                    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);
                    // _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["FAMILY_NAME"].ToString().Replace('"',' ') );
                    //_stmpl_records.SetAttribute("TBT_DESCRIPTION1", dr["DESCRIPTION"].ToString());
                    _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"]);
                    //  _stmpl_records.SetAttribute("TBT_YOURCOST", dr["COSt"].ToString());
                    //  _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);
                    // _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                    // _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);
                    //_stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"].ToString());
                    // _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);
                    // _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"].ToString());


                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }


                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\Main");


                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_NewProductLogNav_Load = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return objHelperService.ProdimageRreplaceImages(sHTML, _Package);
        }


        public string ST_Bottom_Load()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string sHTML = string.Empty;

            try 
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
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
                string dr_cat_id_upper = string.Empty;
                //  dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");
                dsrecords = (DataSet)HttpContext.Current.Application["key_MainCategory"];

                // if (dsrecords == null)
                //{ // remove WES NEWS MENU 
                //    //DataRow[] dr= tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'","CATEGORY_NAME" );
                //    //TO ADD WESNEWS
                //   // drs = tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");
                //    // DataRow[] dr = tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");


                //    if (drs.Length > 0)
                //    {
                //        dsrecords.Tables.Add(drs.CopyToDataTable().Copy());
                //    }
                //    else
                //        return "";
                //}
                //else
                //    return "";

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                //int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];



                int ictrecords = 0;
                string currenturl = objHelperService.AddDomainname();
                string cellbottom = "Bottom\\cell";

                string strxml = HttpContext.Current.Server.MapPath("xml_sort");
                System.IO.FileInfo Fil1 = new System.IO.FileInfo(strxml + "\\Mainds_Sort.xml");
                DataSet dsrecords_dup = new DataSet();
                // if (Fil1.Exists == true)
                // {
                dsrecords_dup.ReadXml(strxml + "\\Mainds_Sort.xml");
                // }
                foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                {

                    //DataRow[] dr1 = dsrecords.Tables[0].Select("Category_id='" + dr["CATEGORY_ID"] + "' ");

                    dr_cat_id_upper = dr["Category_id"].ToString().ToUpper();
                    if (dr_cat_id_upper != WesNewsCategoryId)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf(cellbottom);
                        // remove to string 
                        //_stmpl_records.SetAttribute("TBT_REWRITEURL", dr["URL_RW_PATH"].ToString());
                        //_stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());
                        string url = string.Empty;
                        //if (dr1[0]["URL_RW_PATH"].ToString().Contains("http") == false)
                        //{

                        //    url = currenturl + dr1[0]["URL_RW_PATH"].ToString();

                        //}
                        //else
                        //{

                        url = currenturl + dr["URL_RW_PATH"].ToString();
                        //}

                        _stmpl_records.SetAttribute("TBT_REWRITEURL", url);
                        _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"]);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }
                }

                _stmpl_container = _stg_container.GetInstanceOf("Bottom" + "\\" + "Main");

                //if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null)
                //{
                //    if (HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                //    {
                //        _stmpl_container.SetAttribute("TBT_RET_PRO", false);
                //    }
                //}
                //else
                //{
                _stmpl_container.SetAttribute("TBT_RET_PRO", true);
                //}

                //if (!HttpContext.Current.Session["USER_ID"].Equals(""))
                //{
                //    if (HttpContext.Current.Session["USER_ID"].ToString().Equals(System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"]))
                //        _stmpl_container.SetAttribute("TBT_DUMUSER_CHECK", false);
                //    else
                _stmpl_container.SetAttribute("TBT_DUMUSER_CHECK", true);
                //}
                //string Host = HttpContext.Current.Request.Url.Host;
                //string Rawurl = HttpContext.Current.Request.RawUrl;
                //_stmpl_container.SetAttribute("Rawurl", Host + Rawurl);


                //dsbotmsname = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_MICROSITE_NAME");
                //string combinelihref = "";
                //if (dsbotmsname != null)
                //{
                //    if (dsbotmsname.Tables[0].Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in dsbotmsname.Tables[0].Rows)
                //        {
                //            string href ="";
                //            string lihref="";

                //            href= "/SupplierList/Supplier1/Contactuspage_Supplier1.aspx?supplier_name=" + dr["MICROSITE_NAME"].ToString();
                //            lihref = "<li><a href=\"" + href + "\" class=\"tx_4\">" + dr["MICROSITE_NAME"].ToString() + "</a></li>";
                //            combinelihref = combinelihref + lihref;
                //        }
                //    }
                //}
                //    _stmpl_container.SetAttribute("MICSITNAME", combinelihref);

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                _stmpl_container.SetAttribute("PopularSearch", ST_POPULAR_SEARCH());
                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            // return ProdimageRreplaceImages(sHTML);
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Bottom_Load = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return sHTML;


        }
        //public string ST_PopularSearch()
        //{
        //    try
        //    {

        //            // ConnectionDB objConnectionDB = new ConnectionDB();
        //            // HelperServices objHelperServices = new HelperServices();

        //            //TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("PopularProduct", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        //            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("PopularSearch", HttpContext.Current.Server.MapPath("~\\Templates"), "");
        //            //  tbwtEngine.RenderHTML("Column");



        //            //  objErrorHandler.CreateLog("ST_PopularProduct");

        //            return tbwtEngine.ST_POPULAR_SEARCH();
        //            //string html = (string)Cache["Cache_POPULARPRODUCT"];
        //            //return html;



        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        return string.Empty;
        //    }
        //}
        public string ST_SubFamily_Load(DataSet tempDs)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string sHTML = string.Empty;

            try
            {
                StringTemplateGroup _stg_container = null;
                // StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                //  StringTemplate _stmpl_records = null;

                TBWDataList[] lstrecords = new TBWDataList[0];

                DataSet dsrecords = new DataSet();

                DataRow[] drs = null;




                //DataSet tempDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                if (tempDs != null && tempDs.Tables["FamilyPro"] != null)
                {
                    drs = tempDs.Tables["FamilyPro"].Select("FAMILY_ID='" + paraValue + "'");
                    //if (dr.Length > 0)
                    //{
                    //    dsrecords = new DataSet();
                    //    dsrecords.Tables.Add(dr.CopyToDataTable().Copy());
                    //}

                }
                if (drs == null || drs.Length <= 0)
                    return "";

                //int ictrows = 0;





                //string attname="";

                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");

                _stmpl_container.SetAttribute("TBT_FAMILY_NAME", drs[0]["FAMILY_NAME"].ToString().Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""));
                _stmpl_container.SetAttribute("TBT_FAMILY_TITLE", drs[0]["FAMILY_NAME"].ToString().Replace('"', ' '));
                _stmpl_container.SetAttribute("TBT_SHORT_DESCRIPTION", drs[0]["FAMILY_SHORT_DESC"].ToString());
                _stmpl_container.SetAttribute("TBT_DESCRIPTIONS", drs[0]["FAMILY_DESC"].ToString());
                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());

                //FileInfo Fil = new FileInfo(strProdImages + drs[0]["FAMILY_TH_IMAGE"].ToString().Replace("\\", "/"));
                if (drs[0]["FAMILY_TH_IMAGE"] != null || drs[0]["FAMILY_TH_IMAGE"].ToString() != "")
                {
                    _stmpl_container.SetAttribute("TBT_TFWEB_IMAGE1", drs[0]["FAMILY_TH_IMAGE"].ToString().Replace("\\", "/"));
                    _stmpl_container.SetAttribute("TBT_FWEB_IMAGE1", "javascript:Zoom('/prodimages" + objHelperService.SetImageFolderPath(drs[0]["FAMILY_TH_IMAGE"].ToString().Replace("\\", "/"), "_th", "_Images_200") + "')");

                }
                else
                {
                    _stmpl_container.SetAttribute("TBT_TFWEB_IMAGE1", "/images/noimage.gif");
                    _stmpl_container.SetAttribute("TBT_FWEB_IMAGE1", "");

                }


                //foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                //{

                //    attname = "TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();

                //     if (dr["ATTRIBUTE_TYPE"].ToString().Equals("3") || dr["ATTRIBUTE_TYPE"].ToString().Equals("9"))
                //     {
                //         FileInfo Fil = new FileInfo(strProdImages + dr["STRING_VALUE"].ToString().Replace("\\", "/"));

                //         if (Fil.Exists)
                //         {
                //           _stmpl_container.SetAttribute(attname , dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                //         }
                //         else
                //             _stmpl_container.SetAttribute(attname, "/images/noimage.gif");

                //     }
                //     else
                //       _stmpl_container.SetAttribute(attname, dr["STRING_VALUE"].ToString());



                //}

                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_subFamily_load = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return objHelperService.ProdimageRreplaceImages(sHTML, _Package);
        }

        public string ST_Family_Load(DataSet dsrecords)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string sHTML = string.Empty;

            try
            {
                StringTemplateGroup _stg_container = null;
                //  StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                // StringTemplate _stmpl_records = null;

                TBWDataList[] lstrecords = new TBWDataList[0];

                //DataSet dsrecords = new DataSet();

                //DataRow[] drs = null;




                // dsrecords = (DataSet)HttpContext.Current.Session["FamilyProduct"];

                if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
                    return "";

                // int ictrows = 0;





                string attname = string.Empty;

                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                if (dsrecords.Tables[0].Rows[0]["STATUS"].ToString().ToUpper() == "TRUE")
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "main" + "1");
                else if (dsrecords.Tables[0].Rows[0]["STATUS"].ToString().ToUpper() == "FALSE")
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "main");
                else
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "main" + "2");


                _stmpl_container.SetAttribute("TBT_FAMILY_NAME", dsrecords.Tables[0].Rows[0]["FAMILY_NAME"].ToString());
                _stmpl_container.SetAttribute("TBT_FAMILY_TITLE", dsrecords.Tables[0].Rows[0]["FAMILY_NAME"].ToString().Replace('"', ' '));
                HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] = "";
                HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] = dsrecords.Tables[0].Rows[0]["FAMILY_NAME"].ToString();
                _stmpl_container.SetAttribute("TBT_PROD_COUNT", dsrecords.Tables[0].Rows[0]["PROD_COUNT"].ToString());
                _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                _stmpl_container.SetAttribute("TBT_FAMILY_PROD_COUNT", dsrecords.Tables[0].Rows[0]["FAMILY_PROD_COUNT"].ToString());
                string desc1 = string.Empty;
                string descall = string.Empty;
                string descallstring = string.Empty;
                foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                {

                    attname = dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();


                    desc1 = "";
                    if (attname.Equals("DESCRIPTIONS") || attname.Equals("FEATURES") || attname.Equals("SPECIFICATION") || attname.Equals("SPECIFICATIONS") || attname.Equals("APPLICATIONS") || attname.Equals("NOTES"))
                    {
                        desc1 = dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        desc1 = desc1.ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                    }
                    //commented by indu to display short description 
                    else if (attname.Contains("SHORT_DESCRIPTION"))
                    //else
                    {
                        _stmpl_container.SetAttribute("TBT_" + attname, dr["STRING_VALUE"].ToString());
                    }

                    if (!desc1.Equals(""))
                        descall = descall + desc1 + "<br/><br/>";
                    // _stmpl_container.SetAttribute(attname, dr["STRING_VALUE"].ToString());

                    // if (attname.Contains("FAMILY_NAME"))
                    //    HttpContext.Current.Session["F_ALT_FNAME"] = dr["STRING_VALUE"].ToString();
                }

                if (!desc1.Equals(""))
                {
                    descall = descall.Trim();
                    descall = descall.Substring(0, descall.Length - 5);
                }
                if (descall.Length > 1080)
                {
                    descallstring = descall.Substring(0, 1080).ToString();
                    _stmpl_container.SetAttribute("TBT_MORE", descallstring);
                    _stmpl_container.SetAttribute("TBT_MENU_ID", "2");
                    descall = descall.Substring(0, descall.Length).ToString();
                    _stmpl_container.SetAttribute("TBT_DESCALL", descall);

                }
                else
                {
                    _stmpl_container.SetAttribute("TBT_DESCALL", descall);
                    _stmpl_container.SetAttribute("TBT_MENU_ID", "2");

                    _stmpl_container.SetAttribute("TBT_MORE", descall);
                }
                if (descall.Length > 1080)
                    _stmpl_container.SetAttribute("TBT_MORE_SHOW", true);
                else
                    _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);

                string cthref = "";
                cthref = ST_VPC();
                _stmpl_container.SetAttribute("TBT_CT_HREF", cthref);
                if (HttpContext.Current.Session["PRO_CAT_NAME"] != null)
                    _stmpl_container.SetAttribute("TBT_CT_NAME", HttpContext.Current.Session["PRO_CAT_NAME"].ToString());
                else
                    _stmpl_container.SetAttribute("TBT_CT_NAME", "Category Name");
                if (HttpContext.Current.Request.QueryString["fid"] != null)
                {
                    _fid = HttpContext.Current.Request.QueryString["fid"].ToString();

                    DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                    if (tmpds != null && tmpds.Tables.Count > 0)
                    {
                        _stmpl_container.SetAttribute("TBT_FAMILY_ID", _fid);

                        string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
                        _stmpl_container.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                        _stmpl_container.SetAttribute("TBT_ORGEA_PATH", eapath);
                        // string bceapath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();

                        //objHelperService.Cons_NewURl(_stmpl_container, _stmpl_container.GetAttribute("TBT_ORGEA_PATH").ToString() + "////" + _fid + "=" + "Family", "fl.aspx", true, "", false, false);

                        string[] catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                        objHelperService.SimpleURL(_stmpl_container, "AllProducts////WESAUSTRALASIA////" + _fid + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "Family", "fl.aspx");
                    }

                }

                try
                {
                    GetFamilyMultipleImages(Convert.ToInt32(paraFID), _stmpl_container, dsrecords);
                }
                catch
                { }

                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            sw.Stop();
            objErrorHandler.ExeTimelog = "ST_Family_Load = " + sw.Elapsed.TotalSeconds.ToString();
            objErrorHandler.createexecutiontmielog();
            return objHelperService.ProdimageRreplaceImages(sHTML, _Package);
        }


        protected string GetImagePath(object Path)
        {
            // System.IO.FileInfo Fil = null;
            string retpath = string.Empty;

            try
            {
                if (Path.ToString().Contains("noimage.gif"))
                    retpath = strprodimg_sepdomain + "/images/noimage.gif";
                else
                {
                    //Fil = new System.IO.FileInfo(strProdImages + Path.ToString().ToLower().Replace("\\", "/").Replace("_th", "_Images_200"));
                    //if (Fil.Exists)
                    //{
                    //    retpath = strprodimg_sepdomain + Path.ToString().ToLower().Replace("\\", "/").Replace("_th", "_Images_200");
                    //}
                    //else
                    //    retpath = strprodimg_sepdomain + "/images/noimage.gif";

                    //objErrorHandler.CreateLog(Path + "newproductlognav");   
                    if (Path == "")
                    {
                        retpath = strprodimg_sepdomain + "/images/noimage.gif";
                    }
                    else
                    {
                        retpath = strprodimg_sepdomain + Path.ToString().ToLower().Replace("\\", "/").Replace("_th", "_Images_200");
                    }
                }

                return retpath;
            }
            catch
            {
                return retpath;
            }
            finally
            {
                // Fil = null;
            }
        }
        public void SendMail_AfterPaymentSP(int OrderId, int OrderStatus, bool isau,string cardType)
        {

            objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP start OrderId=" + OrderId);
            string toemail = "";
            try
            {

                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner1 OrderId=" + OrderId);
                string BillAdd;
                string ShippAdd;
                string stemplatepath;
                DataSet dsOItem = new DataSet();
                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
                PaymentServices objPaymentServices = new PaymentServices();
                PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
                oPayInfo = objPaymentServices.GetPayment(OrderId);
                oOrderInfo = objOrderServices.GetOrder(OrderId);

                int UserID = oOrderInfo.UserID; //objHelperServices.CI(Session["USER_ID"].ToString());

                //oUserInfo = objUserServices.GetUserInfo(UserID);
                //   oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                dsOItem = objOrderServices.GetOrderItems(OrderId);
                BillAdd = GetBillingAddress(OrderId);
                ShippAdd = GetShippingAddress(OrderId);

                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = oOrderInfo.ShipNotes;


                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner2 OrderId=" + OrderId);
                if (oOrderInfo.CreatedUser != 777)
                {

                    oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                }
                else
                {
                    oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
                }
                // oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
                string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
                string Emailadd = oUserInfo.AlternateEmail;
                toemail = oUserInfo.AlternateEmail;

                string url = HttpContext.Current.Request.Url.Authority.ToString();
                string PendingorderURL = "";// string.Format("http://" + url + "/PendingOrder.aspx");

                int ModifiedUser = objHelperServices.CI(oOrderInfo.ModifiedUser);
                oUserInfo = objUserServices.GetUserInfo(ModifiedUser);
                string ApprovedUserEmailadd = oUserInfo.AlternateEmail;

                string SubmittedBy = "";
                switch (oOrderInfo.OrderStatus)
                {
                    case 6:
                        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                        break;
                    case 12:
                        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                        break;
                    default:
                        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                        break;
                }
                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner3 OrderId=" + OrderId);
                //string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                string sHTML = "";

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

                    stemplatepath =HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                    int ictrows = 0;

                    DataSet dscat = new DataSet();
                    DataTable dt = null;
                    _stg_records = new StringTemplateGroup("row", stemplatepath);
                    _stg_container = new StringTemplateGroup("main", stemplatepath);


                    lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];

                    int ictrecords = 0;
                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner4 OrderId=" + OrderId);
                    foreach (DataRow dr in dsOItem.Tables[0].Rows)//For Records
                    {
                        //if (websiteid == 3)
                        //   _stmpl_records = _stg_records.GetInstanceOf("mail-wagner" + "\\" + "row");
                        //   else
                        _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "row");

                        _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                        _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());

                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }

                    //if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    //if( websiteid == 3)
                    //   _stmpl_container = _stg_container.GetInstanceOf("mail-wagner" + "\\" + "OrderSubmitted");
                    //    else
                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmittedAfterPay");


                    //else
                    //    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");


                    //_stmpl_container.SetAttribute("CONNOTNO", oOrderInfo.TrackingNo);  
                    //_stmpl_container.SetAttribute("INVOICENO", oOrderInfo.InvoiceNo);
                    //_stmpl_container.SetAttribute("SHIPPEDBY", oOrderInfo.ShipCompany);
                    _stmpl_container.SetAttribute("PAY_METHOD", cardType);
                    _stmpl_container.SetAttribute("AMOUNT", oOrderInfo.TotalAmount);
                    _stmpl_container.SetAttribute("ORDER_ID", oOrderInfo.OrderID);
                    _stmpl_container.SetAttribute("OrderDate", Createdon);
                    _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                    _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                    _stmpl_container.SetAttribute("CreatedBy", Createdby);
                    // if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    _stmpl_container.SetAttribute("SubmittedBy", SubmittedBy);
                    // else
                    //    _stmpl_container.SetAttribute("SubmittedBy", "");



                    _stmpl_container.SetAttribute("ShippingMethod", ShippingMethod);
                    _stmpl_container.SetAttribute("BillingAddress", BillAdd);
                    _stmpl_container.SetAttribute("ShippingAddress", ShippAdd);
                    _stmpl_container.SetAttribute("shippingnotes", shippingnotes);

                    if (shippingnotes != "")
                        _stmpl_container.SetAttribute("TBT_shippingnotes", true);
                    else
                        _stmpl_container.SetAttribute("TBT_shippingnotes", false);

                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);

                    //Added by :Indu For View order button 
                    _stmpl_container.SetAttribute("OrderNo", oOrderInfo.OrderID);
                    string orderurl = "https://www.wagneronline.com.au/OrderReport.aspx?OrdId=" + oOrderInfo.OrderID;
                    _stmpl_container.SetAttribute("OrderURL", orderurl);
                    _stmpl_container.SetAttribute("pricecurrency", "$");

                    _stmpl_container.SetAttribute("pname", oOrderInfo.BillcompanyName);
                    _stmpl_container.SetAttribute("pstreet", oOrderInfo.BillAdd1);
                    _stmpl_container.SetAttribute("locality", oOrderInfo.BillCity);
                    _stmpl_container.SetAttribute("region", oOrderInfo.BillState);
                    _stmpl_container.SetAttribute("country", oOrderInfo.BillCountry);
                    //****************************//


                    sHTML = _stmpl_container.ToString();
                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner5 OrderId=" + OrderId);
                    //objErrorHandler.CreatePayLog(sHTML);

                }
                catch (Exception ex)
                {
                    objHelperServices.Mail_Error_Log("SP", oOrderInfo.OrderID, "", ex.Message, 0, objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]), 1);
                    objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, "", ex.Message);
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    sHTML = "";

                }
                if (sHTML != "")
                {
                    //objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
                    //System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();

                    //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                    //MessageObj.To.Add(Emailadd.ToString());
                    ////MessageObj.To.Add("jtechalert@gmail.com");
                    ////MessageObj.To.Add("mohanarangam.e.r@jtechindia.com");
                    //MessageObj.Subject = "Pending Order - WES Australasia";
                    //MessageObj.IsBodyHtml = true;
                    //MessageObj.Body = sHTML;
                    //System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    ////System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                    //smtpclient.UseDefaultCredentials = false;
                    //smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    ////smtpclient.Port = 587;
                    ////smtpclient.Credentials = new System.Net.NetworkCredential("jtechalert@gmail.com", "jtech@#$123");
                    //smtpclient.Send(MessageObj);

                    //objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
                    ////ArrayList CCList = new ArrayList();
                    ////CCList.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                    ////objNotificationServices.NotifyCC = CCList;
                    //objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                    //objNotificationServices.NotifyTo.Add(Emailadd.ToString());

                    // string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                    //EmailSubject = EmailSubject.Replace("{ORDERID}", OrderID.ToString());
                    //objNotificationServices.NotifySubject = EmailSubject;
                    //objNotificationServices.NotifyMessage = sHTML;
                    //objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
                    //objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
                    //objNotificationServices.NotifyIsHTML = true;
                    //objNotificationServices.SendMessage();

                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner6 OrderId=" + OrderId);
                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());



                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner7 OrderId=" + OrderId);

                    string emails = "";
                    string Adminemails = "";
                    string webadminmail = "";
                    webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {
                        MessageObj.To.Add(Emailadd.ToString());
                        MessageObj.Bcc.Add(webadminmail);

                        //if (isau == false)
                        //{
                        //    if (System.Configuration.ConfigurationManager.AppSettings["EasyAsk_Port"].ToString() == "9200")
                        //        Adminemails = System.Configuration.ConfigurationManager.AppSettings["ToMail"].ToString();
                        //    else
                        //        Adminemails = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                        //}

                        // Get_ADMIN_UserEmils();
                        //if (ApprovedUserEmailadd.Trim() != "" && Emailadd.ToString() != ApprovedUserEmailadd.ToString())
                        //   MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                    }
                    else
                    {
                        emails = objUserServices.Get_ADMIN_APPROVED_UserEmils(UserID.ToString());

                        MessageObj.To.Add(Emailadd.ToString());


                    }
                    string addgoogleurl = System.Configuration.ConfigurationManager.AppSettings["addgoogleurl"].ToString();
                    if (addgoogleurl == "true")
                    {
                        MessageObj.To.Add("schema.whitelisting+sample@gmail.com");
                        //MessageObj.To.Add("indumathi@jtechindia.com");
                    }
                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    // {
                    MessageObj.Subject = "Bigtop Order Payment Successful - Order No : " + CustomerOrderNo.ToString();
                    //}
                    //else
                    //{
                    //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                    // }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;
                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner8 OrderId=" + OrderId);

                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);
                    objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");

                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner9 OrderId=" + OrderId);

                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {
                        if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                        {
                            //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                            MessageObj.To.Clear();
                            MessageObj.To.Add(ApprovedUserEmailadd.ToString());

                            smtpclient.Send(MessageObj);
                            objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                            objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner10 OrderId=" + OrderId);
                        }
                        if (Adminemails != "")
                        {

                            string[] emailid = Adminemails.ToString().Split(',');
                            if (emailid.Length > 0)
                            {
                                foreach (string id in emailid)
                                {
                                    if (ApprovedUserEmailadd.ToUpper().ToString() != id.ToUpper().ToString() && Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                    {
                                        //MessageObj.CC.Add(id.ToString());
                                        MessageObj.Subject = "Bigtop Order Alert - Order No : " + CustomerOrderNo.ToString();
                                        MessageObj.To.Clear();
                                        MessageObj.To.Add(id.ToString());
                                        smtpclient.Send(MessageObj);
                                        objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                        objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner11 OrderId=" + OrderId);
                                    }
                                }
                            }
                            else
                            {
                                if (ApprovedUserEmailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString() && Emailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString())
                                {
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(Adminemails.ToString());
                                    smtpclient.Send(MessageObj);
                                    objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner12 OrderId=" + OrderId);
                                }
                                //MessageObj.CC.Add(emails.ToString());
                            }

                        }
                    }
                    else
                    {
                        if (emails != "")
                        {

                            string[] emailid = emails.ToString().Split(',');
                            if (emailid.Length > 0)
                            {
                                foreach (string id in emailid)
                                {
                                    if (Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                    {
                                        //MessageObj.CC.Add(id.ToString());
                                        MessageObj.To.Clear();
                                        MessageObj.To.Add(id.ToString());
                                        smtpclient.Send(MessageObj);
                                        objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                        objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner13 OrderId=" + OrderId);
                                    }
                                }
                            }
                            else
                            {
                                if (Emailadd.ToUpper().ToString() != emails.ToUpper().ToString())
                                {
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(emails.ToString());
                                    smtpclient.Send(MessageObj);
                                    objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner14 OrderId=" + OrderId);
                                    //MessageObj.CC.Add(emails.ToString());
                                }
                            }

                        }


                    }


                }
            }
            catch (Exception ex)
            {
                objHelperServices.Mail_Error_Log("SP", OrderId, toemail.ToString(), ex.Message, 0, objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]), 1);
                objHelperServices.Mail_Log("SP", OrderId, "", ex.Message);
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner14 OrderId=" + OrderId);

            }
            objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP end OrderId=" + OrderId);
      
    }
        public string GetShippingAddress(int OrderID)
        {
            string sShippingAddress = "";
            try
            {
                OrderServices.OrderInfo oOI = new OrderServices.OrderInfo();
                oOI = objOrderServices.GetOrder(OrderID);

                if (oOI.ShipCompName.Trim().Length > 0)
                    sShippingAddress = oOI.ShipCompName + "<BR>";
                else
                    sShippingAddress = "";



                //if (oOI.Receiver_Contact.Trim().Length > 0)
                //{
                //    sShippingAddress = sShippingAddress + "Attn To:" + oOI.Receiver_Contact + "<BR>";

                //}
                if (oOI.ShipCompName.Trim().Length > 0)
                {
                    if (oOI.ShipCompName.ToLower().Contains(oOI.ShipFName.ToLower()) == false)
                    {
                        sShippingAddress = sShippingAddress + oOI.ShipFName + "<BR>";
                    }
                }
                else
                {
                    sShippingAddress = sShippingAddress + oOI.ShipFName + "<BR>";
                }


                if (oOI.ShipAdd1.Trim().Length > 0)
                {
                    sShippingAddress = sShippingAddress + oOI.ShipAdd1.Trim() + "<BR>";
                }
                if (oOI.ShipAdd2.Trim().Length > 0)
                {
                    sShippingAddress = sShippingAddress + oOI.ShipAdd2.Trim() + "<BR>";
                }
                if (oOI.ShipAdd3.Trim().Length > 0)
                {
                    sShippingAddress = sShippingAddress + oOI.ShipAdd3.Trim() + "<BR>";
                }
                if (oOI.ShipCity.Trim().Length > 0)
                    sShippingAddress = sShippingAddress + oOI.ShipCity + "<BR>";
                if (oOI.ShipState.Trim().Length > 0)
                    sShippingAddress = sShippingAddress + oOI.ShipState + "<BR>";
                if (oOI.ShipZip.Trim().Length > 0)
                    sShippingAddress = sShippingAddress + oOI.ShipZip + "<BR>";
                if (oOI.ShipCountry.Trim().Length > 0)
                    sShippingAddress = sShippingAddress + oOI.ShipCountry + "<BR>";
                //if (oOI.ReceiverContact.Trim().Length > 0)
                //{
                //    sShippingAddress = sShippingAddress + "<BR>" + oOI.ReceiverContact + "<BR>";
                //}
                sShippingAddress = sShippingAddress + oOI.ShipPhone + "<BR>";
                if (oOI.DeliveryInstr.Trim().Length > 0)
                {
                    sShippingAddress = sShippingAddress + "<BR>" + oOI.DeliveryInstr + "<BR>";
                }
            }
            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString());
            }
            return sShippingAddress;
        }
        public string GetBillingAddress(int OrderID)
        {
            string sBillingAddress = "";
            try
            {
                OrderServices.OrderInfo oBI = new OrderServices.OrderInfo();
                oBI = objOrderServices.GetOrder(OrderID);
                if (oBI.BillcompanyName.Trim().Length > 0)
                {
                    sBillingAddress = oBI.BillcompanyName + "<BR>";
                    if ((oBI.Bill_Name.Trim().Length > 0) && (oBI.BillcompanyName.ToLower().Contains(oBI.Bill_Name.ToLower()) == false))
                    {

                        sBillingAddress = sBillingAddress + oBI.Bill_Name + "<BR>";
                    }

                    else if (oBI.BillcompanyName.ToLower().Contains(oBI.BillFName.ToLower()) == false)
                    {
                        sBillingAddress = sBillingAddress + oBI.BillFName + " " + oBI.BillLName + "<BR>";
                    }

                }
                else
                {
                    sBillingAddress = "";
                    if ((oBI.Bill_Name.Trim().Length > 0))
                    {

                        sBillingAddress = sBillingAddress + oBI.Bill_Name + "<BR>";
                    }

                    else
                    {
                        sBillingAddress = sBillingAddress + oBI.BillFName + " " + oBI.BillLName + "<BR>";
                    }
                }



                //  sBillingAddress = sBillingAddress + oBI.BillFName + oBI.BillLName + "<BR>";
                if (oBI.BillAdd1.Trim().Length > 0)
                {
                    sBillingAddress = sBillingAddress + oBI.BillAdd1.Trim() + "<BR>";
                }
                if (oBI.BillAdd2.Trim().Length > 0)
                {
                    sBillingAddress = sBillingAddress + oBI.BillAdd2.Trim() + "<BR>";
                }
                if (oBI.BillAdd3.Trim().Length > 0)
                {
                    sBillingAddress = sBillingAddress + oBI.BillAdd3.Trim() + "<BR>";
                }
                if (oBI.BillCity.Trim().Length > 0)
                    sBillingAddress = sBillingAddress + oBI.BillCity + "<BR>";
                if (oBI.BillState.Trim().Length > 0)
                    sBillingAddress = sBillingAddress + oBI.BillState + "<BR>";
                if (oBI.BillZip.Trim().Length > 0)
                    sBillingAddress = sBillingAddress + oBI.BillZip + "<BR>";
                if (oBI.BillCountry.Trim().Length > 0)
                    sBillingAddress = sBillingAddress + oBI.BillCountry + "<BR>";

                sBillingAddress = sBillingAddress + oBI.BillPhone;

                sBillingAddress = sBillingAddress + "<BR><p style='color:white'>...</p>";
            }

            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString());
            }


            return sBillingAddress;
        }
        public void SendMail_Review(int OrderId, int OrderStatus, bool isau)
         {
             string toemail = "";
             try
             {
                 //objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner1 OrderId=" + OrderId);

                 string BillAdd;
                 string ShippAdd;
                 string stemplatepath;
                 DataSet dsOItem = new DataSet();
                 OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                 UserServices objUserServices = new UserServices();
                 UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
                 PaymentServices objPaymentServices = new PaymentServices();
                 PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
                 OrderServices objOrderServices = new OrderServices();
                 oPayInfo = objPaymentServices.GetPayment(OrderId);
                 oOrderInfo = objOrderServices.GetOrder(OrderId);
                 if (oOrderInfo.TotalAmount >= 25)
                 {
                     int UserID = oOrderInfo.UserID; //objHelperServices.CI(Session["USER_ID"].ToString());

                     //oUserInfo = objUserServices.GetUserInfo(UserID);
                     oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                     dsOItem = objOrderServices.GetOrderItems(OrderId);


                     string ShippingMethod = oOrderInfo.ShipMethod;
                     string CustomerOrderNo = oPayInfo.PORelease;
                     string shippingnotes = oOrderInfo.ShipNotes;


                     if (oOrderInfo.CreatedUser != 777)
                     {

                         oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                     }
                     else
                     {
                         oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
                     }

                     //   oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                     string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
                     string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
                     string Emailadd = oUserInfo.AlternateEmail;
                     toemail = oUserInfo.AlternateEmail;
                     objErrorHandler.CreatePayLog(Emailadd);
                     string url = HttpContext.Current.Request.Url.Authority.ToString();
                     string PendingorderURL = "";// string.Format("http://" + url + "/PendingOrder.aspx");



                     string ApprovedUserEmailadd = oUserInfo.AlternateEmail;



                     //string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                     string sHTML = "";
                     try
                     {
                         StringTemplateGroup _stg_container = null;
                         StringTemplateGroup _stg_records = null;
                         StringTemplate _stmpl_container = null;
                         StringTemplate _stmpl_records = null;
                         //StringTemplate _stmpl_records1 = null;
                         //StringTemplate _stmpl_recordsrows = null;
                         TBWDataList[] lstrecords = new TBWDataList[0];
                         TBWDataList[] lstrows = new TBWDataList[0];

                         //StringTemplateGroup _stg_container1 = null;
                         //StringTemplateGroup _stg_records1 = null;
                         TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                         TBWDataList1[] lstrows1 = new TBWDataList1[0];

                         stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                         //int ictrows = 0;

                         DataSet dscat = new DataSet();
                         // DataTable dt = null;
                         _stg_records = new StringTemplateGroup("row", stemplatepath);
                         _stg_container = new StringTemplateGroup("main", stemplatepath);


                         lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];



                         int ictrecords = 0;




                         _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmittedAfterPay_Review");

                         _stmpl_container.SetAttribute("OrderDate", Createdon);

                         _stmpl_container.SetAttribute("CustName", oUserInfo.FirstName + " " + oUserInfo.LastName);
                         _stmpl_container.SetAttribute("CustEmail", Emailadd);


                         _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                         sHTML = _stmpl_container.ToString();
                         // objErrorHandler.CreatePayLog("SendMail_Review HTML");
                         objErrorHandler.CreatePayLog(sHTML);
                     }
                     catch (Exception ex)
                     {
                        objErrorHandler.CreateLog(ex.ToString());
                        objHelperServices.Mail_Error_Log("Review", oOrderInfo.OrderID, "", ex.Message, 0, objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]), 1);
                         objHelperServices.Mail_Log("Review", oOrderInfo.OrderID, "", ex.Message);
                         objErrorHandler.ErrorMsg = ex;
                         objErrorHandler.CreateLog();
                         sHTML = "";

                     }
                     if (sHTML != "")
                     {

                         //objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner3 OrderId=" + OrderId);
                         System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                         //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                         MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

                         string emails = "";

                         string webadminmail = "";
                         webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                         string Reviewmail = objHelperServices.GetOptionValues("Reviewemail").ToString();
                         objErrorHandler.CreatePayLog("Reviewmail:" + Reviewmail);
                         string Reducedemailto = objHelperServices.GetOptionValues("Reducedemailto").ToString();
                         objErrorHandler.CreatePayLog("Reducedemailto:" + Reducedemailto);
                         if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                         {
                             MessageObj.To.Add(Reducedemailto);
                             MessageObj.Bcc.Add(webadminmail);
                             MessageObj.Bcc.Add(Reviewmail);

                         }
                         else
                         {
                             //  emails = objUserServices.Get_ADMIN_APPROVED_UserEmils(UserID.ToString());

                             MessageObj.To.Add(Reducedemailto);
                             MessageObj.Bcc.Add(webadminmail);
                             MessageObj.Bcc.Add(Reviewmail);

                         }

                         //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                         //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                         // {
                         MessageObj.Subject = "Bigtop Order Information - " + oPayInfo.PORelease;
                         //}
                         //else
                         //{
                         //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                         // }

                         MessageObj.IsBodyHtml = true;
                         MessageObj.Body = sHTML;

                         //objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner4 OrderId=" + OrderId);
                         System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                         smtpclient.UseDefaultCredentials = false;
                         smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                         smtpclient.Send(MessageObj);
                         objHelperServices.Mail_Log("ReviewMail", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                         //objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner5 OrderId=" + OrderId);



                     }

                 }
                 else
                     {

                         objErrorHandler.CreateLog("Order Amount is less than 25 for Order Id" + oOrderInfo.OrderID);
                 }
             }
             catch (Exception ex)
             {
                 objHelperServices.Mail_Error_Log("Review", OrderId, toemail.ToString(), ex.Message, 0, objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]), 1);
                 objHelperServices.Mail_Log("Review", OrderId, "", ex.Message);
                 objErrorHandler.ErrorMsg = ex;
                 objErrorHandler.CreateLog();


             }
             //objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner6 OrderId=" + OrderId);
         }


         private bool GetStockDetails(StringTemplate st, string Pid,string ETA)
         {
             HelperDB objhelperDb = new HelperDB();
             try
             {
                 string user_id = string.Empty ;
                 string order_id = string.Empty;
                 int no = 0;
                 int availabilty = 0;
                 string availabilty1 = string.Empty;
                 string sqlexec =  "exec SP_CHECK_STOCK_STATUS '" + Pid.ToString() + "' ";
                 //objErrorHandler.CreateLog("sqlexec " + sqlexec);
                 DataSet Dsall = objhelperDb.GetDataSetDB(sqlexec);
                 //objErrorHandler.CreateLog("Row Count " + Dsall.Tables[0].Rows.Count);
                 //objErrorHandler.CreateLog("total " + Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"]);
              
                 //objErrorHandler.CreateLog("SUPPLIER_ITEM_CODE 1" + Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"]);
                 //objErrorHandler.CreateLog("SUPPLIER_ID 1" + Dsall.Tables[0].Rows[0]["SUPPLIER_ID"]);
                //if ((Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"] == null || Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"].ToString() == "") || (Dsall.Tables[0].Rows[0]["SUPPLIER_ID"].ToString() == null || Dsall.Tables[0].Rows[0]["SUPPLIER_ID"].ToString() == ""))
                //{
                //    objErrorHandler.CreateLog("SUPPLIER_ITEM_CODE " + Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"].ToString());
                //    if (Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"] != null )
                //    {
                //        int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString());
                //        objErrorHandler.CreateLog(" shipping_time " + shipping_time);
                //        if (shipping_time < 14)
                //        {
                //            string supplier_shipping_time = string.Empty;

                //            if (shipping_time > 1)
                //            {
                //                supplier_shipping_time = "1 - " + shipping_time + " Days";
                //            }
                //            else if (shipping_time == 1)
                //            {
                //                supplier_shipping_time = " 1 Day";
                //            }
                //            if (shipping_time <= 14)
                //            {
                //                st.SetAttribute("TBT_ISINSTOCK", true);
                //                st.SetAttribute("TBT_ISINSTOCK_STAUS", "Please Call");
                //                st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                //                st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                //            }
                //            return false;
                //        }

                //    }
                //    return true;
                //}
                string _Eta = string.Format("<tr><td><b>ETA</b></td><td colspan=\"2\"><b>" + ETA + "</b></td></tr>");
                if (Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"] != null && Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString() != "")
                 {
                     availabilty1 = Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString();
                     availabilty1 = availabilty1.Replace("+", "");
                     availabilty = Convert.ToInt32(availabilty1.ToString());
                     //objErrorHandler.CreateLog("availabilty " + availabilty);
                 }
                 if (Dsall.Tables[0].Rows[0]["product_id"] != null && Dsall.Tables[0].Rows[0]["product_id"].ToString() == string.Empty && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"] != null && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"].ToString() == string.Empty)
                 {
                     //objErrorHandler.CreateLog("availabilty " + availabilty + " SUPPLIER_SHIP_DAYS " + Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString());
                    if (Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"] != null && Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString() != "")
                    {
                        int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString());
                        //objErrorHandler.CreateLog(" shipping_time " + shipping_time);
                        string supplier_shipping_time = string.Empty;

                        if (shipping_time > 1)
                        {
                            supplier_shipping_time = "1 - " + shipping_time + " Days";
                        }
                        else if (shipping_time == 1)
                        {
                            supplier_shipping_time = " 1 Day";
                        }
                        if (shipping_time > 0 && shipping_time <= 14)

                        {
                            st.SetAttribute("TBT_ISINSTOCK", true);
                            st.SetAttribute("TBT_ISINSTOCK_STAUS", "Please Call");
                            if (ETA == "PLEASE CALL" || ETA == "")
                            {
                                st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                                st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                            }
                            else
                            {
                                st.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
                                st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);

                            }
                        }
                        else if (ETA == "PLEASE CALL" || ETA == "")
                        {
                            return true;

                        }
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                   
                 }
                 else if(availabilty > 0 )
                 {
                   //  st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                     int avail_total = Convert.ToInt32(availabilty);
                     int stock_cutoff = Convert.ToInt32(Dsall.Tables[0].Rows[0]["WEB_STOCK_CUTOFF"]);
                     int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIPPING_TIME"].ToString());
                     string supplier_shipping_time=string.Empty;
                     if(shipping_time>1)
                     {
                         supplier_shipping_time="1 - "+shipping_time + " Days";
                     }
                     else if (shipping_time == 1)
                     {
                         supplier_shipping_time = " 1 Day";
                     }
                     if (avail_total >= stock_cutoff)
                     {
                         st.SetAttribute("TBT_ISINSTOCK", true);
                         st.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                        st.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                        //if (ETA == "PLEASE CALL" || ETA == "")
                        //{
                            st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                            st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                        //}
                        //else
                        //{
                        //    st.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
                        //    st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);
                        //}
                           
                     }
                     else if (avail_total < stock_cutoff)
                     {
                        if (ETA == "PLEASE CALL" || ETA == "")
                        {
                            st.SetAttribute("TBT_PLEASE_CALL", true);
                            st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                        }
                        else
                        {
                            st.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
                            st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);

                        }
                    }
                     return false; 
                 }
             }
             catch (Exception ex)
             {
                 objErrorHandler.CreateLog(ex.ToString());
                 objErrorHandler.ErrorMsg = ex;
                 objErrorHandler.CreateLog();
             }
             return true; 
         }
    
    
    }
            //private DataSet FamilyFilterFlatTable(DataSet flatDataset)
            //{

            //    {
            //        StringBuilder SQLstring = new StringBuilder();
            //        DataSet oDsFamilyFilter = new DataSet();
            //        string SQLString = " SELECT FAMILY_FILTERS FROM TB_CATALOG WHERE  CATALOG_ID = " + CATALOG_ID + " ";
            //        SqlDataAdapter da = new SqlDataAdapter(SQLString, _DBConnectionString);
            //        da.Fill(oDsFamilyFilter);
            //        string sFamilyFilter = string.Empty;
            //        if (oDsFamilyFilter.Tables[0].Rows.Count > 0 && oDsFamilyFilter.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
            //        {
            //            sFamilyFilter = oDsFamilyFilter.Tables[0].Rows[0].ItemArray[0].ToString();
            //            XmlDocument xmlDOc = new XmlDocument();
            //            xmlDOc.LoadXml(sFamilyFilter);
            //            XmlNode rNode = xmlDOc.DocumentElement;

            //            if (rNode.ChildNodes.Count > 0)
            //            {
            //                for (int i = 0; i < rNode.ChildNodes.Count; i++)
            //                {
            //                    XmlNode TableDataSetNode = rNode.ChildNodes[i];

            //                    if (TableDataSetNode.HasChildNodes)
            //                    {
            //                        if (TableDataSetNode.ChildNodes[2].InnerText == " ")
            //                        {
            //                            TableDataSetNode.ChildNodes[2].InnerText = "=";
            //                        }
            //                        if (TableDataSetNode.ChildNodes[0].InnerText == " ")
            //                        {
            //                            TableDataSetNode.ChildNodes[0].InnerText = "0";
            //                        }
            //                        string stringval = TableDataSetNode.ChildNodes[3].InnerText.Replace("'", "''");
            //                        if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
            //                        {
            //                            SQLstring.Append("SELECT DISTINCT FAMILY_ID FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
            //                        }
            //                        else
            //                        {
            //                            SQLstring.Append("SELECT DISTINCT FAMILY_ID FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ") WHERE  (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
            //                        }


            //                    }
            //                    if (TableDataSetNode.ChildNodes[4].InnerText == "NONE")
            //                    {
            //                    }
            //                    if (TableDataSetNode.ChildNodes[4].InnerText == "AND")
            //                    {
            //                        SQLstring.Append(" INTERSECT \n");
            //                    }
            //                    if (TableDataSetNode.ChildNodes[4].InnerText == "OR")
            //                    {
            //                        SQLstring.Append(" UNION \n");
            //                    }

            //                }

            //            }

            //        }
            //        string familyFiltersql = SQLstring.ToString();

            //        if (familyFiltersql.Length > 0)
            //        {
            //            string s = "SELECT FAMILY_ID FROM FAMILY(" + CATALOG_ID + ") WHERE CATALOG_ID=" + CATALOG_ID + " AND FAMILY_ID IN\n" +
            //                  "(\n";// +
            //            //"SELECT DISTINCT FAMILY_ID\n" +
            //            //"FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ")\n" +
            //            //"WHERE\n";
            //            familyFiltersql = s + familyFiltersql + "\n)";

            //            SqlDataAdapter dda = new SqlDataAdapter(familyFiltersql, _DBConnectionString);
            //            dda.Fill(oDsFamilyFilter);

            //            bool available = false;
            //            DataSet AvailableDs = flatDataset;
            //            for (int rowCount = 0; rowCount < flatDataset.Tables[0].Rows.Count; rowCount++)
            //            {//foreach (DataRow odr in flatDataset.Tables[0].Rows)
            //                DataRow odr = flatDataset.Tables[0].Rows[rowCount];
            //                available = false;
            //                foreach (DataRow dr in oDsFamilyFilter.Tables[0].Rows)
            //                {
            //                    if (dr["FAMILY_ID"].ToString() == odr["FAMILY_ID"].ToString() || dr["FAMILY_ID"].ToString() == odr["SUBFAMILY_ID"].ToString())
            //                    {
            //                        available = true;
            //                    }

            //                }
            //                if (available == false)
            //                {
            //                    //string cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE FAMILY_ID = " + odr["FAMILY_ID"].ToString() + " OR FAMILY_ID = " + odr["SUBFAMILY_ID"].ToString() + " AND  USER_SESSION_ID='" + HttpContext.Current.Session.SessionID + "'";
            //                    //SqlConnection _SQLConn = new SqlConnection(_DBConnectionString);
            //                    //SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
            //                    //pscmd.CommandType = CommandType.Text;
            //                    //int valr = pscmd.ExecuteNonQuery();
            //                    odr.Delete();
            //                    flatDataset.AcceptChanges();
            //                    rowCount--;
            //                }

            //            }


            //        }

            //    }
            //    //ProductFilterFlatTable(flatDataset);
            //    return flatDataset;
            //}


        public class ProductImage
        {
            public string LargeImage { get; set; }
            public string Thumpnail { get; set; }
            public string MediumImage { get; set; }
            public string SmallImage { get; set; }
            public string Image1024 { get; set; }
            public string Sno { get; set; }
            public string Active { get; set; }
        }

        public class ProductDetails
        {
            public string AttributeName { get; set; }
            public string SpecValue { get; set; }
            public int AttributeID { get; set; }
            public int SortOrder { get; set; }
        }

        public class ProductDesc
        {
            public string AttributeName { get; set; }
            public string SpecValue { get; set; }
            public int AttributeID { get; set; }
        }

  

    /*********************************** J TECH CODE ***********************************/
}
