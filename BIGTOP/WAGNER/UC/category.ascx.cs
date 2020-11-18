using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

using System.Diagnostics;  
public partial class UC_category : System.Web.UI.UserControl
{
    ConnectionDB objConnectionDB = new ConnectionDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    Security objSecurity = new Security();
    string strFile = HttpContext.Current.Server.MapPath("ProdImages");
    public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ST_Category()
    {
        HelperServices objHelperServices = new HelperServices();
        //return (Category_RenderHTML("CATEGORY", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString())));
       // Stopwatch sw = Stopwatch.StartNew();
        //string ss=ST_Category_Load("CATEGORY", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()));
        //sw.Stop();
        //return sw.Elapsed.ToString() + ss;

        return ST_Category_Load("CATEGORY", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()));
    }
    //public string ST_Categoryold()
    //{
    //    HelperServices objHelperServices = new HelperServices();
    //    Stopwatch sw = Stopwatch.StartNew();
    //    string ss = (Category_RenderHTML("CATEGORY", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString())));
    //    sw.Stop();
    //    return sw.Elapsed.ToString() + ss;
    //}
    public string ST_Category_Load(string Package, string _SkinRootPath)
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
           //  TBWDataList[] lstrows = new TBWDataList[0];

            //StringTemplateGroup _stg_container1 = null;
            // StringTemplateGroup _stg_records1 = null;
             //TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            // TBWDataList1[] lstrows1 = new TBWDataList1[0];

            //   DataSet dscat = new DataSet();
            EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
            DataSet dsrecordsM = new DataSet();
            DataSet dsrecordsS = new DataSet();
           // DataTable dt = null;
          //  DataRow[] drs = null;

            dsrecordsM = EasyAsk.GetCategoryAndBrand("MainCategory");
            dsrecordsS = EasyAsk.GetCategoryAndBrand("SubCategory"); 
            // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
           // int ictrows = 0;

            //if (dsrecordsM != null && dsrecordsM.Tables.Count > 0 && dsrecordsM.Tables[0].Rows.Count > 0)
            //{
                
            //}




            _stg_records = new StringTemplateGroup("row", _SkinRootPath);
            _stg_container = new StringTemplateGroup("main", _SkinRootPath);


            lstrecords = new TBWDataList[dsrecordsM.Tables[0].Rows.Count + 1];


            string dr_cat_id_upper = string.Empty;

            int ictrecords = 0;
           // int ictrecords1 = 0;
            string currenturl = objHelperServices.AddDomainname();
            foreach (DataRow dr in dsrecordsM.Tables[0].Rows)//For Records
            {
               dr_cat_id_upper = dr["Category_id"].ToString().ToUpper();
               if (dr_cat_id_upper != WesNewsCategoryId && !(dr_cat_id_upper.Contains("SPF-")))
               {
                   if (Convert.ToInt32(dr["SUB_COUNT"]) > 0)
                   {
                       _stmpl_records = _stg_records.GetInstanceOf("Category" + "\\" + "cellnew");
                       // remove tostring
                       // _stmpl_records.SetAttribute("TBT_REWRITEURL", dr["URL_RW_PATH"].ToString());
                       // _stmpl_records.SetAttribute("TBT_PARENT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());

                       
                       _stmpl_records.SetAttribute("TBT_REWRITEURL", dr["URL_RW_PATH"]);
                      
                       _stmpl_records.SetAttribute("TBT_PARENT_CATEGORY_NAME", dr["CATEGORY_NAME"]);
                       _stmpl_records.SetAttribute("TBT_SUB_CATEGORY_LIST", getSubcategoryList(dsrecordsS, dr["CATEGORY_ID"].ToString(), 1, _SkinRootPath));
                       _stmpl_records.SetAttribute("TBT_SUB_CATEGORY_LIST1", getSubcategoryList(dsrecordsS, dr["CATEGORY_ID"].ToString(), 2, _SkinRootPath));
                      
                       lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                       ictrecords++;
                   }
               }
            }


            _stmpl_container = _stg_container.GetInstanceOf("Category" + "\\" + "mainnew");


            _stmpl_container.SetAttribute("TBT_CURRENTURL", currenturl);
            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
            sHTML = _stmpl_container.ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            sHTML = "";
        }
        return sHTML;
    }
    public string getSubcategoryList(DataSet subdata, string catid,int part,string _SkinRootPath)
    {
        //StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
       // StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        StringTemplate _stmpl_recordstemp = new StringTemplate();
        //  StringTemplate _stmpl_records1 = null;
        // StringTemplate _stmpl_recordsrows = null;
        TBWDataList[] lstrecords = new TBWDataList[0];

        _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
       // string subhtml = "";
        StringBuilder strsubhtml = new StringBuilder();
      
      //  DataTable dt = null;
        try
        {
            DataRow[] drs = subdata.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + catid + "' And PART1='" + part + "'");
            string currenturl = objHelperServices.AddDomainname();   
            if (drs.Length > 0)
            {
                
                foreach (DataRow dr in drs.Take(6).CopyToDataTable().Rows)
                {
                    _stmpl_records = _stg_records.GetInstanceOf("Category" + "\\" + "cell1");

                    // remove tostring
                   // _stmpl_records.SetAttribute("TBT_REWRITEURL", dr["URL_RW_PATH"].ToString());
                   // _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());

                    string url = string.Empty;
                    if (dr["URL_RW_PATH"].ToString().Contains("http") == false)
                    {

                        url = currenturl + dr["URL_RW_PATH"].ToString();

                    }
                    else
                    {

                        url = dr["URL_RW_PATH"].ToString();
                    }
                    _stmpl_records.SetAttribute("TBT_REWRITEURL", url);
                    _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"]);
                    // subhtml = subhtml + _stmpl_records.ToString();
                  
                    strsubhtml = strsubhtml.Append(_stmpl_records.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            return "";
        }
       // return subhtml;
        return strsubhtml.ToString();
    }
    public string Category_RenderHTML(string Package, string SkinRootPath)
    {
        System.IO.FileInfo Fil = null;
        try
        {
            string skin_container = null;

            int grid_cols = 0;
            int grid_rows = 0;
            string skin_sql_container = null;
            string skin_sql_param_container = null;
            string skin_records = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            StringTemplateGroup stg_records = null;
            StringTemplate bodyST = null;
            StringTemplate bodyST_categorylist = null;
            StringTemplate bodyST_categorylistnew = null;
            StringTemplate bodyST_head = null;
            StringTemplate bodyST_list1 = null;
            string firstval = null;
            //List<string> name = new List<string>();
            StringBuilder name = new StringBuilder();
            System.Text.StringBuilder ct = new StringBuilder();
            System.Text.StringBuilder categoryrowlist = new StringBuilder();
            System.Text.StringBuilder categorylistnew = new StringBuilder();
            int indV = 0;
            int bodyValue = 0;
            EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
            DataSet dspkg = new DataSet();
            string _tsb = "";
            string _tsm = "";
            string _searchstr = "";
            string _byp = "2";
            string _bypcat = null;
            string _pid = "";
            string _fid = "";
            string _Ea_path = "";

           


            if (Request.QueryString["tsm"] != null)
                _tsm = Request.QueryString["tsm"];

            if (Request.QueryString["tsb"] != null)
                _tsb = Request.QueryString["tsb"];

            if (Request.QueryString["searchstr"] != null)
                _searchstr = Request.QueryString["searchstr"];
            if (Request.QueryString["srctext"] != null)
                _searchstr = Request.QueryString["srctext"];



            // old Code  by Jtech
            //string sqlpkginfo = " SELECT * FROM TBW_PACKAGE ";
            //sqlpkginfo = sqlpkginfo + " WHERE PACKAGE_NAME = '" + Package + "'";

            //dspkg = GetDataSet(sqlpkginfo);
            // old Code commant by Jtech
            dspkg = (DataSet)objHelperDB.GetGenericDataDB(Package, "GET_PACKAGE_WITHOUT_ISROOT", HelperDB.ReturnType.RTDataSet);

            if (dspkg != null)
            {
                if (dspkg.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dspkg.Tables[0].Rows)
                    {
                        skin_container = dr["SKIN_NAME"].ToString();
                        grid_cols = Convert.ToInt32(dr["GRID_COLS"]);
                        grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
                        skin_sql_container = dr["SKIN_SQL"].ToString();
                        skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
                        skin_records = dr["SKIN_NAME"].ToString();
                    }
                }
            }

            //Build the inner body of the HTML
            stg_records = new StringTemplateGroup(skin_records, SkinRootPath + "\\" + skin_records);
            //DataSet dsrecords = GetDataSet(skin_sql_container);
            DataSet dsrecords = EasyAsk.GetCategoryAndBrand("SubCategory");
            if (dsrecords != null && dsrecords.Tables[0].Rows.Count != 0)
            {
                lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count * 2];
                int catno = 0;
                if (dsrecords.Tables[0].Rows[0]["TBT_PARENT_CATEGORY_NAME"].ToString() != null && dsrecords.Tables[0].Rows[0]["TBT_PARENT_CATEGORY_NAME"].ToString() != string.Empty)
                {
                    //Build the Sub heading 
                    firstval = dsrecords.Tables[0].Rows[0]["TBT_PARENT_CATEGORY_NAME"].ToString().ToUpper();

                    //if (firstval.ToUpper().ToString() != "WESNEWS" && firstval != "")
                    //TO INCLUDE WESNEWS
                    if (firstval.ToUpper().ToString() != "" && firstval != "")
                        bodyST_categorylist = stg_records.GetInstanceOf("cell");
                    // objHelperServices.Cons_NewURl(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + firstval, "ct.aspx", true, "");
                    DataRow ddr = dsrecords.Tables[0].Rows[0];

                    foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                    {

                        if (dc.ColumnName.ToString().Contains("CUSTOM_NUM"))
                            if (ddr[dc.ColumnName].ToString().Length > 0)
                                bodyST_categorylist.SetAttribute(dc.ColumnName, Convert.ToDouble(ddr[dc.ColumnName].ToString()));
                            else
                                bodyST_categorylist.SetAttribute(dc.ColumnName, "0");

                        else
                        {
                            if ("TBT_PARENT_CATEGORY_IMAGE" == dc.ColumnName)
                            {
                                bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString().Replace("\\", "/"));
                                //Fil = new System.IO.FileInfo(strFile + ddr[dc.ColumnName].ToString().Replace("/", "\\"));
                                //if (Fil.Exists)
                                //{
                                //    bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString().Replace("\\", "/"));
                                //}
                                //else
                                //{
                                //    bodyST_categorylist.SetAttribute(dc.ColumnName, "/images/noimage.gif");

                                //}
                                //bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString().Replace('\\', '/'));
                            }
                            else if ("EA_PATH" == dc.ColumnName)
                                bodyST_categorylist.SetAttribute(dc.ColumnName, HttpUtility.UrlEncode(objSecurity.StringEnCrypt(ddr[dc.ColumnName].ToString())));
                            else
                            {
                                if (ddr[dc.ColumnName].ToString() != "")
                                    bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString());
                            }
                            // objHelperServices.Cons_NewURl(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + ddr[dc.ColumnName].ToString(), "ct.aspx", true, ""); 
                        }
                    }
                    if (firstval != "")
                        objHelperServices.SimpleURL(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + firstval, "ct.aspx");
                }

                //if (i == 0)
                //{
                //    objHelperServices.Cons_NewURl(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + firstval, "ct.aspx", true, "");
                //}
                int i = 1;
                foreach (DataRow dr in dsrecords.Tables[0].Rows)
                {

                    //if (dr["TBT_PARENT_CATEGORY_NAME"].ToString() != null && dr["TBT_PARENT_CATEGORY_NAME"].ToString() != string.Empty && dr["TBT_PARENT_CATEGORY_NAME"].ToString().ToLower() != "wesnews" && dr["TBT_PARENT_CATEGORY_ID"].ToString().ToLower() != "wesnews")
                    //{
                    //TO ADD WESNEWS
                    if (dr["TBT_PARENT_CATEGORY_NAME"].ToString() != null && dr["TBT_PARENT_CATEGORY_NAME"].ToString() != string.Empty && dr["TBT_PARENT_CATEGORY_NAME"].ToString().ToLower() != "" && dr["TBT_PARENT_CATEGORY_ID"].ToString().ToLower() != "")
                    {

                        if (firstval != dr["TBT_PARENT_CATEGORY_NAME"].ToString().ToUpper() && firstval != "")
                        {

                            //Build the category 
                            bodyST_categorylist.SetAttribute("TBT_CATEGORY_ORDER", i);
                            i++;
                            bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", ct.ToString());
                            bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST1", categorylistnew.ToString());

                            name.Append(bodyST_categorylist.ToString());
                            indV++; catno = 0;
                            if (indV == grid_cols)
                            {
                                bodyST = stg_records.GetInstanceOf("row");
                                bodyST.SetAttribute("TBWDataList", name);

                                lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
                                bodyValue++;
                                indV = 0;
                                name = new StringBuilder();
                            }

                            //if (firstval.ToUpper().ToString() != "WESNEWS")
                            //{
                            //TO ADD WESNEWS
                            if (firstval.ToUpper().ToString() != "")
                            {
                                //Build the sub heading
                                ct = new StringBuilder();
                                bodyST_categorylist = stg_records.GetInstanceOf("cell");

                                categorylistnew = new StringBuilder();
                                bodyST_categorylistnew = stg_records.GetInstanceOf("cell");
                            }

                            // objHelperServices.Cons_NewURl(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + dr["TBT_PARENT_CATEGORY_NAME"].ToString(), "ct.aspx",, "");
                            foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                            {
                                if (dc.ColumnName.ToString().Contains("CUSTOM_NUM"))
                                    if (dr[dc.ColumnName].ToString().Length > 0)
                                        bodyST_categorylist.SetAttribute(dc.ColumnName, Convert.ToDouble(dr[dc.ColumnName].ToString()));
                                    else
                                        bodyST_categorylist.SetAttribute(dc.ColumnName, "0");
                                else
                                {
                                    if ("TBT_PARENT_CATEGORY_IMAGE" == dc.ColumnName)
                                    {

                                        bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString().Replace("\\", "/"));
                                        // Fil = new System.IO.FileInfo(strFile + dr[dc.ColumnName].ToString().Replace("/", "\\"));
                                        //if (Fil.Exists)
                                        //{
                                        //    bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString().Replace("\\", "/"));
                                        //}
                                        //else
                                        //{
                                        //    bodyST_categorylist.SetAttribute(dc.ColumnName, "/images/noimage.gif");

                                        //}

                                        // bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString().Replace('\\', '/'));
                                    }
                                    else if ("EA_PATH" == dc.ColumnName)
                                        bodyST_categorylist.SetAttribute(dc.ColumnName, HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr[dc.ColumnName].ToString())));
                                    else
                                    {
                                        if (dr[dc.ColumnName].ToString() != "")
                                            bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());
                                    }
                                }

                            }

                            firstval = dr[1].ToString().ToUpper();

                            if (firstval != "")
                                objHelperServices.SimpleURL(bodyST_categorylist, "AllProducts////WESAUSTRALASIA////" + "////" + firstval, "ct.aspx");

                        }





                        //Build the Content
                        if (catno < 12 && firstval != "")
                        {
                            //if (dr["TBT_SHORT_DESC"].ToString().ToLower() != "not for web" && dr["CATEGORY_ID"].ToString().ToLower() != "wesnews")
                            //{
                            //DISPLAY WESNEWS IN WEB
                            if (dr["TBT_SHORT_DESC"].ToString().ToLower() != "not for web" && dr["CATEGORY_ID"].ToString().ToLower() != "")
                            {
                                bodyST_list1 = stg_records.GetInstanceOf("cell1");
                                bodyST_list1.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());
                                bodyST_list1.SetAttribute("TBT_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                if (dr["TBT_CUSTOM_NUM_FIELD3"] != System.DBNull.Value)
                                {
                                    bodyST_list1.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(dr["TBT_CUSTOM_NUM_FIELD3"]).ToString());
                                }
                                else
                                {
                                    bodyST_list1.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "");
                                }
                                bodyST_list1.SetAttribute("TBT_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(dr["TBT_PARENT_CATEGORY_ID"].ToString()));

                                bodyST_list1.SetAttribute("TBT_BRAND", HttpUtility.UrlEncode(_tsb));
                                bodyST_list1.SetAttribute("TBT_MODEL", HttpUtility.UrlEncode(_tsm));
                                bodyST_list1.SetAttribute("TBT_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(_searchstr));
                                bodyST_list1.SetAttribute("TBT_ATTRIBUTE_TYPE", "Category");
                                bodyST_list1.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["CATEGORY_NAME"].ToString()));
                                bodyST_list1.SetAttribute("TBT_ATTRIBUTE_BRAND", _tsb);

                                bodyST_list1.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["EA_PATH"].ToString() + "////" + dr["TBT_PARENT_CATEGORY_NAME"].ToString())));

                                objHelperServices.SimpleURL(bodyST_list1, "AllProducts////WESAUSTRALASIA////" + "////" + dr["EA_PATH"].ToString() + "////" + dr["TBT_PARENT_CATEGORY_NAME"].ToString() + "////" + bodyST_list1.GetAttribute("TBT_ATTRIBUTE_VALUE"), "pl.aspx");

                                if (catno < 6)
                                {
                                    ct.Append(bodyST_list1.ToString()); catno++;
                                }
                                else
                                {
                                    categorylistnew.Append(bodyST_list1.ToString()); catno++;
                                }
                            }
                        }



                    }
                }
            }
            if (dsrecords == null || dsrecords.Tables[0].Rows.Count == 0)
            {
                bodyST_categorylist = stg_records.GetInstanceOf("cell");
            }
            if (indV < grid_cols)
            {

                bodyST_categorylist.SetAttribute("TBT_CATEGORY_ORDER", "21");
                bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", ct.ToString());
                bodyST_categorylistnew.SetAttribute("TBT_SUB_CATEGORY_LIST1", categorylistnew.ToString());
                name.Append(bodyST_categorylist.ToString());
                // name.Append(bodyST_categorylistnew.ToString());
                bodyST = stg_records.GetInstanceOf("row");
                bodyST.SetAttribute("TBWDataList", name);
                if (lstrecords.Length != 0)
                {
                    lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
                }
                bodyValue++;
            }
            StringTemplate bodyST_main = stg_records.GetInstanceOf("main");
            bodyST_main.SetAttribute("TBWDataList", lstrecords);
            string sHtmls = bodyST_main.ToString();
            if (sHtmls.Contains("src=\"/prodimages\""))
                sHtmls = sHtmls.Replace("src=\"/prodimages\"", "src=\"/images/noimage.gif\"");
            if (sHtmls.Contains("src=\"\""))
                sHtmls = sHtmls.Replace("src=\"\"", "src=\"/images/noimage.gif\"");

            return objHelperServices.StripWhitespace(sHtmls);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
        finally
        {
            Fil = null; 
        }

        //if (dsrecords.Tables[0].Rows[0][1].ToString() != null && dsrecords.Tables[0].Rows[0][1].ToString() != string.Empty)
        //{
        //    //Build the Sub heading 
        //    firstval = dsrecords.Tables[0].Rows[0][1].ToString().ToUpper();
        //    bodyST_head = stg_records.GetInstanceOf("cell_head");
        //    bodyST_head.SetAttribute("TBT_PARENT_CATEGORY_NAME", firstval);
        //    ct.Append(bodyST_head.ToString());
          
        //}
        //foreach (DataRow dr in dsrecords.Tables[0].Rows)
        //{
        //    if (dr[1].ToString() != null && dr[1].ToString() != string.Empty)
        //    {
        //        if (firstval != dr[1].ToString().ToUpper())
        //        {

        //            //Build the category 
        //            bodyST_categorylist = stg_records.GetInstanceOf("cell");
        //            bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", ct.ToString());
        //            name.Append(bodyST_categorylist.ToString());
        //            indV++;
        //            if (indV == grid_cols)
        //            {
        //                bodyST = stg_records.GetInstanceOf("row");
        //                bodyST.SetAttribute("TBWDataList", name);
        //                lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
        //                bodyValue++;
        //                indV = 0;
        //                name = new StringBuilder();
        //            }

        //            //Build the sub heading
        //            ct = new StringBuilder();
        //            bodyST_head = stg_records.GetInstanceOf("cell_head");
        //            bodyST_head.SetAttribute("TBT_PARENT_CATEGORY_NAME", dr[1].ToString().ToUpper());
        //            ct.Append(bodyST_head.ToString());
        //            firstval = dr[1].ToString().ToUpper();
                   
        //        }
        //        //Build the Content
        //        bodyST_list1 = stg_records.GetInstanceOf("cell1");
        //        bodyST_list1.SetAttribute(dr.Table.Columns[3].ColumnName.ToString(), dr[3].ToString());
        //        ct.Append(bodyST_list1.ToString());
                
        //    }
        //}      
        
        //if (indV < grid_cols)
        //{
        //    bodyST = stg_records.GetInstanceOf("row");
        //    bodyST.SetAttribute("TBWDataList", name);
        //    lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
        //    bodyValue++;            
        //}
        //StringTemplate bodyST_main = stg_records.GetInstanceOf("main");
        //bodyST_main.SetAttribute("TBWDataList", lstrecords);
        //return bodyST_main.ToString();
    }



    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(";") + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}
}
