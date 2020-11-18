using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.Services;
using System.Collections;
using System.Web.Configuration;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.TemplateRender;
using System.Xml;
using Newtonsoft.Json;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

public partial class GblWebMethods : System.Web.UI.Page
{

    
    
    
  
    static int count=0;
    string _parentCatID = string.Empty;


    static string strimgPath = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
    static string EasyAsk_URL = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_URL"].ToString();
    static int EasyAsk_Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EasyAsk_Port"]);
    static string EasyAsk_WebCatDictionary = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_WebCatDictionary"].ToString();
    static string EasyAsk_WebCatPath = System.Configuration.ConfigurationManager.AppSettings["EA_NEW_PRODUCT_INIT_CATEGORY_PATH"].ToString();
    static string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    DataSet dsAutoComplete = new DataSet();
    DataSet dsAdvisor = new DataSet();


    [WebMethod]
    public static string GetSearchResultNew1(string Strvalue)
    {
        string sHTML = string.Empty;
        StringBuilder strhtml = new StringBuilder(50000);
        HelperServices objHelperServices = new HelperServices();
        //HelperServices objHelperServices = new HelperServices();
        //EasyAsk_WES ObjEasyAsk = new EasyAsk_WES();
        //HelperDB objHelperDB = new HelperDB();
        Security objSecurity = new Security();
        HelperDB objHelperDB = new HelperDB();
        DataSet dsAutoComplete = new DataSet();
        DataSet dsAdvisor = new DataSet();
        ErrorHandler objErrorHandler = new ErrorHandler();
        string temp_product_Image = string.Empty;
        string temp_fmly_Image = string.Empty;
        string image_string = string.Empty;
        try
        {


            string resultStr = string.Empty;
            string eapath = string.Empty;
            string UserId = string.Empty;
            int pricecode = -1;
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                UserId = HttpContext.Current.Session["USER_ID"].ToString();

            if (UserId == "")
                UserId = "0";
            pricecode = objHelperDB.GetPriceCode(UserId);
            try
            {
                System.Net.WebClient myWebClient = new System.Net.WebClient();
                //objErrorHandler.CreateLog("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete-1.2.1.jsp?fctn=&key=" + HttpUtility.UrlEncode(Strvalue) + "&dct=" + EasyAsk_WebCatDictionary + "&num=10");
                resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete-1.2.1.jsp?fctn=&key=" + HttpUtility.UrlEncode(Strvalue) + "&dct=" + EasyAsk_WebCatDictionary + "&num=10");
               
            }
            catch  (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString() + "Autosuggest Exception");
            
            }
            if (resultStr != "")
            {

                XmlDocument xd = new XmlDocument();
                resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
                dsAutoComplete = new DataSet();
                dsAutoComplete.ReadXml(new XmlNodeReader(xd));
                string jsoncid = JsonConvert.SerializeObject(dsAutoComplete);
                string strxml = HttpContext.Current.Server.MapPath("xml");

                System.IO.File.WriteAllText(strxml + "\\" + "autosearch.txt", jsoncid);
                //resultStr = resultStr.Replace("\"", "");

            }

            try
            {
                System.Net.WebClient myWebClient = new System.Net.WebClient();
                resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/apps/Advisor.jsp?indexed=1&oneshot=1&ie=UTF-8&disp=json&RequestAction=advisor&RequestData=CA_Search&CatPath=" + HttpUtility.UrlEncode(EasyAsk_WebCatPath) + "&defarrangeby=////NONE////&dct=" + HttpUtility.UrlEncode(EasyAsk_WebCatDictionary) + "&q=" + HttpUtility.UrlEncode(Strvalue) + "&ResultsPerPage=10&eap_PriceCode=" + pricecode.ToString());
            }
            catch { }
            if (resultStr != "")
            {

                XmlDocument xd = new XmlDocument();
                resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
                dsAdvisor = new DataSet();
                dsAdvisor.ReadXml(new XmlNodeReader(xd));
            }
            DataTable Sqltb = new DataTable();
            DataColumn dc = null;
            string tmpstr = string.Empty;
            if (dsAdvisor.Tables["items"] != null)
            {
                if (dsAdvisor.Tables["items"].Columns["CATEGORY_PATH"] == null)
                {
                    dc = new DataColumn("CATEGORY_PATH", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);
                }
                if (dsAdvisor.Tables["items"].Columns["CATEGORY_ID"] == null)
                {
                    dc = new DataColumn("CATEGORY_ID", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);
                }
                if (dsAdvisor.Tables["items"].Columns["Prod_Thumbnail"] == null)
                {
                    dc = new DataColumn("Prod_Thumbnail", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);
                }
                if (dsAdvisor.Tables["items"].Columns["Family_Thumbnail"] == null)
                {
                    dc = new DataColumn("Family_Thumbnail", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);
                }
                if (dsAdvisor.Tables["items"].Columns["Prod_Description"] == null)
                {
                    dc = new DataColumn("Prod_Description", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);
                }

                foreach (DataRow dr1 in dsAdvisor.Tables["items"].Rows)
                {
                    if (tmpstr.Contains(dr1["FAMILY_ID"].ToString().ToUpper()) == false)
                        tmpstr = tmpstr + "'" + dr1["FAMILY_ID"].ToString().ToUpper() + "',";
                }


                if (tmpstr != "")
                    tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

                if (tmpstr != "")
                {
                    //Sqltb = objhelper.GetDataTable(StrSql);
                    Sqltb = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, tmpstr, "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                    if (Sqltb != null)
                    {
                        foreach (DataRow dr in Sqltb.Rows)
                        {
                            foreach (DataRow dr1 in dsAdvisor.Tables["items"].Rows)
                            {
                                if (dr["FAMILY_ID"].ToString().ToUpper() == dr1["FAMILY_ID"].ToString().ToUpper())
                                {
                                    dr1["CATEGORY_PATH"] = dr["CATEGORY_PATH"];
                                    dr1["CATEGORY_ID"] = dr["SubCatID"];
                                }
                            }
                        }
                    }
                }
            }


            //if (dsAutoComplete != null && dsAutoComplete.Tables.Count >= 2)
            //{
            if (dsAdvisor != null && dsAdvisor.Tables.Count >= 2)
            {
                strhtml.Append("<table><tr>");
                if (dsAdvisor.Tables["categoryList"] != null || dsAdvisor.Tables["Attribute"] != null)
                    strhtml.Append("<td valign='top' style='width:75%'>");
                else
                    strhtml.Append("<td valign='top' style='width:100%'>");






                //if (dsAutoComplete != null & dsAutoComplete.Tables.Count >= 2)
                //{
                if (dsAdvisor != null & dsAdvisor.Tables.Count >= 2)
                {
                    strhtml.Append("<div class='clear'></div>");
                    strhtml.Append("<div class='viewmoresmall'><strong>Search suggestions for " + Strvalue + "</strong></div>");
                    strhtml.Append("<div class='clear'></div>");
                    strhtml.Append("<ul>");
                    int datacnt = 0;
                    string soddevenrow = "background-color:#F9F9F9;";

                    //foreach (DataRow dr in dsAutoComplete.Tables[1].Rows)
                    //{
                    foreach (DataRow dr in dsAdvisor.Tables[1].Rows)
                    {

                        datacnt++;
                        if ((datacnt % 2) == 0)
                        {
                            soddevenrow = "background-color:#fff;";
                        }
                        else
                        {
                            soddevenrow = "background-color:#F9F9F9;";
                        }


                        //tmpstr = dr["val"].ToString().ToUpper();
                        tmpstr = dr["originalquestion"].ToString().ToUpper();
                        tmpstr = tmpstr.Replace(Strvalue.ToUpper(), "<strong>" + Strvalue.ToUpper() + "</strong>");


                        //strhtml.Append("<li style=" + soddevenrow + "><a href='/ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "' >" + tmpstr + "</a></li>");

                        //Modified by :Indu Reason:For URL Rewrite


                        //string NEWURL = objHelperServices.Cons_NewURl_bybrand("ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "", Strvalue, "ps.aspx", "");
                        //string HREFURL = "ps.aspx?" + NEWURL;
                        string NEWURL = objHelperServices.SimpleURL_Str( Strvalue, "ps.aspx",false);
                        string HREFURL =  NEWURL+"/ps/";

                        strhtml.Append("<li style=" + soddevenrow + "><a href='" + HREFURL + "' rel=ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + " >" + tmpstr + "</a></li>");
                    }
                    strhtml.Append("</ul>");
                }


                //if (dsAdvisor.Tables["items"] != null)
                //{

                //    strhtml.Append("<div class='viewmoresmall'><strong>Products</strong></div>");

                //    int datacntprod = 0;
                //    string soddevenrowprod = "drop_products_odd";
                //    foreach (DataRow dr in dsAdvisor.Tables["items"].Rows)
                //    {

                //        eapath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + dr["Family_Id"].ToString();
                //        eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
                //        temp_product_Image = dr["Prod_Thumbnail"].ToString();
                //        temp_fmly_Image = dr["Family_Thumbnail"].ToString();
                //        if (temp_product_Image != "")
                //            image_string = temp_product_Image.Substring(42);
                //        else if (temp_fmly_Image != "")
                //            image_string = temp_fmly_Image.Substring(42);
                //        else
                //            image_string = "noimage.gif";
                //        //Fil = new FileInfo(strimgPath + image_string);

                //        datacntprod++;
                //        if ((datacntprod % 2) == 0)
                //        {
                //            soddevenrowprod = "drop_products_even";
                //        }
                //        else
                //        {
                //            soddevenrowprod = "drop_products_odd";
                //        }


                //        strhtml.Append("<div  class=" + soddevenrowprod + ">");
                //        strhtml.Append("<img width='80' height='80' alt='img' src='prodimages\\" + image_string + "'>");


                //        //strhtml.Append("<a href='/pd.aspx?pid=" + dr["Prod_Id"].ToString() + "&amp;fid=" + dr["Family_Id"].ToString() + "&amp;cid=" + dr["CATEGORY_ID"].ToString() + "&amp;path=" + eapath + "'><strong>" + dr["Family_name"].ToString() + "</strong></a>");
                //        //Modified by :Indu Reason:For URL Rewrite


                //        string NEWURL = objHelperServices.Cons_NewURl_bybrand("pd.aspx?pid=" + dr["Prod_Id"].ToString() + "&amp;fid=" + dr["Family_Id"].ToString() + "&amp;cid=" + dr["CATEGORY_ID"].ToString() + "&amp;path=" + eapath + "'", dr["Family_name"].ToString(), "pd.aspx", "");
                //        string HREFURL = "pd.aspx?" + NEWURL;
                //        strhtml.Append("<a href='"+ HREFURL +"'  rel=pd.aspx?pid=" + dr["Prod_Id"].ToString() + "&amp;fid=" + dr["Family_Id"].ToString() + "&amp;cid=" + dr["CATEGORY_ID"].ToString() + "&amp;path=" + eapath + "><strong>" + dr["Family_name"].ToString() + "</strong></a>");
                //        strhtml.Append("<p>" + dr["Prod_Description"].ToString() + "</p>");
                //        strhtml.Append("<div style='Color:red'>Code :<strong>" + dr["Prod_Code"].ToString() + "</strong> &nbsp;&nbsp;&nbsp;&nbsp;Price :<strong style='red'> " + dr["Price"].ToString() + "</strong>");
                //        strhtml.Append("</div><div class='clear'></div></div>");

                //    }
                //}
                strhtml.Append("</td>");
                if (dsAdvisor.Tables["categoryList"] != null || dsAdvisor.Tables["Attribute"] != null)
                {
                    strhtml.Append("<td valign='top' style='width:60%'>");

                    if (dsAdvisor.Tables["categoryList"] != null)
                    {

                        eapath = "AllProducts////WESAUSTRALASIA////UserSearch1=" + Strvalue;
                        eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
                        strhtml.Append("<div class='viewmoresmall'><strong>Category</strong></div>");
                        strhtml.Append("<ul>");
                        int i = 0;
                        int datacntcatlist = 0;
                        string soddevenrowcatlist = "catlist_li_odd";
                        foreach (DataRow dr in dsAdvisor.Tables["categoryList"].Rows)
                        {
                            i = i + 1;

                            datacntcatlist++;
                            if ((datacntcatlist % 2) == 0)
                            {
                                soddevenrowcatlist = "catlist_li_even";
                            }
                            else
                            {
                                soddevenrowcatlist = "catlist_li_odd";
                            }

                            if (i > 5)
                                break;
                            //strhtml.Append("<li class=" + soddevenrowcatlist + "><a href='ps.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=Category&amp;value=" + HttpUtility.UrlEncode(dr["name"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + "' style='font-size:10px'>" + dr["name"].ToString() + "</a></li>");
                            //Modified by :Indu Reason:For URL Rewrite

                            
                            string NEWURL = objHelperServices.SimpleURL_Str(dr["name"].ToString(), "ps.aspx",true);
                            string HREFURL =  NEWURL+"/ps/" ;
                            strhtml.Append("<li class=" + soddevenrowcatlist + "><a href='" + HREFURL + "' rel='ps.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=Category&amp;value=" + HttpUtility.UrlEncode(dr["name"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + " style='font-size:10px;'>" + dr["name"].ToString() + "</a></li>");
                        }
                        strhtml.Append("</ul>");
                        if (dsAdvisor.Tables["categoryList"].Rows.Count > 5)
                            strhtml.Append("<a style='color:#7AC943;text-decoration:none;font-size:9px;' href='ps.aspx?" + HttpUtility.UrlEncode(Strvalue) + "'>View All Results</a>");

                    }

                    if (dsAdvisor.Tables["Attribute"] != null)
                    {
                        foreach (DataRow dr1 in dsAdvisor.Tables["Attribute"].Rows)
                        {

                            //  if (dr1["name"].ToString().ToUpper().Contains("PRODUCTS TAGS") || dr1["name"].ToString().ToUpper().Contains("PRICE"))
                            if (dr1["name"].ToString().ToUpper().Contains("PRODUCTS TAGS"))
                            {
                                DataTable Datar = dsAdvisor.Tables["AttributeValueList"].Select("attribute_id='" + dr1["attribute_id"] + "'").CopyToDataTable();
                                if (Datar != null && Datar.Rows.Count > 0)
                                {
                                    eapath = "AllProducts////WESAUSTRALASIA////UserSearch1=" + Strvalue;
                                    eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
                                    strhtml.Append("<div class='viewmoresmall'><strong>" + dr1["name"].ToString() + "</strong></div>");
                                    strhtml.Append("<ul>");
                                    int i = 0;
                                    int datacntattribute = 0;
                                    string soddevenrowattlist = "catlist_li_odd";
                                    foreach (DataRow dr in Datar.Rows)
                                    {
                                        i = i + 1;
                                        datacntattribute++;
                                        if ((datacntattribute % 2) == 0)
                                        {
                                            soddevenrowattlist = "catlist_li_even";
                                        }
                                        else
                                        {
                                            soddevenrowattlist = "catlist_li_odd";
                                        }

                                        if (i > 5)
                                            break;

                                        //strhtml.Append("<li class=" + soddevenrowattlist + "><a href='ps.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=" + dr["attributeValue"].ToString() + "&amp;value=" + HttpUtility.UrlEncode(dr["Attributevalue"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + "' style='font-size:10px'>" + dr["Attributevalue"].ToString() + "</a></li>");
                                        //Modified by :Indu Reason:For URL Rewrite


                                        string NEWURL = objHelperServices.SimpleURL_Str ( dr["Attributevalue"].ToString(), "ps.aspx",true);
                                        string HREFURL =  NEWURL+"/ps/";
                                        strhtml.Append("<li class=" + soddevenrowattlist + "><a href='" + HREFURL + "' rel='ps.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=" + dr1["name"].ToString() + "&amp;value=" + HttpUtility.UrlEncode(dr["Attributevalue"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + " style='font-size:10px;'>" + dr["Attributevalue"].ToString() + "</a></li>");



                                    }

                                    strhtml.Append("</ul>");
                                    if (i > 5)
                                    {
                                        //  strhtml.Append("<a style='color:#7AC943;text-decoration:none;font-size:9px;' href='ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "'>View All Results</a>");
                                        //Modified by :Indu Reason:For URL Rewrite


                                        string NEWURL1 = objHelperServices.SimpleURL_Str( Strvalue, "ps.aspx",false);
                                        string HREFURL1 =  NEWURL1+"/ps/";
                                        strhtml.Append("<a style='color:#7AC943;text-decoration:none;font-size:9px;' href='" + HREFURL1 + "' rel='ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "'>View All Results</a>");
                                    }
                                }




                            }
                        }
                    }
                }
                strhtml.Append("</td></tr><tr>");
                if (dsAdvisor.Tables["categoryList"] != null || dsAdvisor.Tables["Attribute"] != null)
                    strhtml.Append("<td colspan='2'>");
                else
                    strhtml.Append("<td>");


                strhtml.Append("<div class='clear'></div>");
                // strhtml.Append("<a class='viewmore' href='ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "'>View All Results</a>");
                string NEWURL2 = objHelperServices.SimpleURL_Str( Strvalue, "ps.aspx",false);
                string HREFURL2 =  NEWURL2+"/ps/" ;
                if (dsAdvisor.Tables["items"] != null && dsAdvisor.Tables["Attribute"] != null)
                {
                    strhtml.Append("<a class='viewmore' href='" + HREFURL2 + "' rel='ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "'>View All Results</a>");
                }
                strhtml.Append("<div class='clear'></div>");
                strhtml.Append("</td></tr></table>");
            }

        }
        catch { }

        return strhtml.ToString().Trim();
    }



    //[WebMethod]
    //public static string GetSearchResult_auto(string Strvalue)
    //{
    //    string sHTML = string.Empty;
    //    StringBuilder strhtml = new StringBuilder(50000); 
    //    HelperServices objHelperServices = new HelperServices();
    //    //HelperServices objHelperServices = new HelperServices();
    //    //EasyAsk_WES ObjEasyAsk = new EasyAsk_WES();
    //    //HelperDB objHelperDB = new HelperDB();
    //    Security objSecurity = new Security();
    //    HelperDB objHelperDB = new HelperDB();
    //    DataSet dsAutoComplete = new DataSet();
    //    DataSet dsAdvisor = new DataSet();
    //    ErrorHandler objErrorHandler = new ErrorHandler();
    //    string temp_product_Image = string.Empty;
    //    string temp_fmly_Image = string.Empty;
    //    string image_string = string.Empty;
    //    try
    //    {


    //        string resultStr = string.Empty;
    //        string eapath = string.Empty;
    //        string UserId = string.Empty;
    //        int pricecode = -1;
    //        if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
    //            UserId = HttpContext.Current.Session["USER_ID"].ToString();

    //        if (UserId == "")
    //            UserId = "0";
    //        pricecode = objHelperDB.GetPriceCode(UserId);
    //        try
    //        {
    //            System.Net.WebClient myWebClient = new System.Net.WebClient();
    //            objErrorHandler.CreateLog("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete-1.2.1.jsp?fctn=&key=" + HttpUtility.UrlEncode(Strvalue) + "&dct=" + EasyAsk_WebCatDictionary + "&num=10");
    //            resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete-1.2.1.jsp?fctn=&key=" + HttpUtility.UrlEncode(Strvalue) + "&dct=" + EasyAsk_WebCatDictionary + "&num=10");

    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.CreateLog(ex.ToString() + "Autosuggest Exception");

    //        }
    //        if (resultStr != "")
    //        {

    //            XmlDocument xd = new XmlDocument();
    //            resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
    //            xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
    //            dsAutoComplete = new DataSet();
    //            dsAutoComplete.ReadXml(new XmlNodeReader(xd));
    //            string jsoncid = JsonConvert.SerializeObject(dsAutoComplete);
    //            string strxml = HttpContext.Current.Server.MapPath("xml");

    //            System.IO.File.WriteAllText(strxml + "\\" + "autosearch.txt", jsoncid);
    //            resultStr = resultStr.Replace("\"", "");
               
    //        }

    //        try
    //        {
    //            System.Net.WebClient myWebClient = new System.Net.WebClient();
    //            resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/apps/Advisor.jsp?indexed=1&oneshot=1&ie=UTF-8&disp=json&RequestAction=advisor&RequestData=CA_Search&CatPath=" + HttpUtility.UrlEncode(EasyAsk_WebCatPath) + "&defarrangeby=////NONE////&dct=" + HttpUtility.UrlEncode(EasyAsk_WebCatDictionary) + "&q=" + HttpUtility.UrlEncode(Strvalue) + "&ResultsPerPage=10&eap_PriceCode=" + pricecode.ToString());
    //        }
    //        catch { }
    //        if (resultStr != "")
    //        {

    //            XmlDocument xd = new XmlDocument();
    //            resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
    //            xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
    //            dsAdvisor = new DataSet();
    //            dsAdvisor.ReadXml(new XmlNodeReader(xd));
    //        }
    //        DataTable Sqltb = new DataTable();
    //        DataColumn dc = null;
    //        string tmpstr = string.Empty;
    //        if (dsAdvisor.Tables["items"] != null)
    //        {
    //            if (dsAdvisor.Tables["items"].Columns["CATEGORY_PATH"] == null)
    //            {
    //                dc = new DataColumn("CATEGORY_PATH", typeof(string));
    //                dc.DefaultValue = "";
    //                dsAdvisor.Tables["items"].Columns.Add(dc);
    //            }
    //            if (dsAdvisor.Tables["items"].Columns["CATEGORY_ID"] == null)
    //            {
    //                dc = new DataColumn("CATEGORY_ID", typeof(string));
    //                dc.DefaultValue = "";
    //                dsAdvisor.Tables["items"].Columns.Add(dc);
    //            }
    //            if (dsAdvisor.Tables["items"].Columns["Prod_Thumbnail"] == null)
    //            {
    //                dc = new DataColumn("Prod_Thumbnail", typeof(string));
    //                dc.DefaultValue = "";
    //                dsAdvisor.Tables["items"].Columns.Add(dc);
    //            }
    //            if (dsAdvisor.Tables["items"].Columns["Family_Thumbnail"] == null)
    //            {
    //                dc = new DataColumn("Family_Thumbnail", typeof(string));
    //                dc.DefaultValue = "";
    //                dsAdvisor.Tables["items"].Columns.Add(dc);
    //            }
    //            if (dsAdvisor.Tables["items"].Columns["Prod_Description"] == null)
    //            {
    //                dc = new DataColumn("Prod_Description", typeof(string));
    //                dc.DefaultValue = "";
    //                dsAdvisor.Tables["items"].Columns.Add(dc);
    //            }

    //            foreach (DataRow dr1 in dsAdvisor.Tables["items"].Rows)
    //            {
    //                if (tmpstr.Contains(dr1["FAMILY_ID"].ToString().ToUpper()) == false)
    //                    tmpstr = tmpstr + "'" + dr1["FAMILY_ID"].ToString().ToUpper() + "',";
    //            }


    //            if (tmpstr != "")
    //                tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

    //            if (tmpstr != "")
    //            {
    //                //Sqltb = objhelper.GetDataTable(StrSql);
    //                Sqltb = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, tmpstr, "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
    //                if (Sqltb != null)
    //                {
    //                    foreach (DataRow dr in Sqltb.Rows)
    //                    {
    //                        foreach (DataRow dr1 in dsAdvisor.Tables["items"].Rows)
    //                        {
    //                            if (dr["FAMILY_ID"].ToString().ToUpper() == dr1["FAMILY_ID"].ToString().ToUpper())
    //                            {
    //                                dr1["CATEGORY_PATH"] = dr["CATEGORY_PATH"];
    //                                dr1["CATEGORY_ID"] = dr["SubCatID"];
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }


    //        //if (dsAutoComplete != null && dsAutoComplete.Tables.Count >= 2)
    //        //{
    //        if (dsAdvisor != null && dsAdvisor.Tables.Count >= 2)
    //        {
    //            strhtml.Append(" <div class=srch_result>");
    //            //if (dsAdvisor.Tables["categoryList"] != null || dsAdvisor.Tables["Attribute"] != null)
    //            //    strhtml.Append("<td valign='top' style='width:75%'>");
    //            //else
    //            //    strhtml.Append("<td valign='top' style='width:100%'>");






    //            //if (dsAutoComplete != null & dsAutoComplete.Tables.Count >= 2)
    //            //{
    //            if (dsAdvisor != null & dsAdvisor.Tables.Count >= 2)
    //            {
    //               // strhtml.Append("<div class='clear'></div>");
    //                //strhtml.Append("<div class='viewmoresmall'><strong>Search suggestions for " + Strvalue + "</strong></div>");
    //                //strhtml.Append("<div class='clear'></div>");

    //                int datacnt = 0;
    //                string soddevenrow = "background-color:#F9F9F9;";
    //               // string NEWURL = objHelperServices.Cons_NewURl_bybrand("ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "", Strvalue, "ps.aspx", "");
    //                //string HREFURL = "ps.aspx?" + NEWURL;
    //                string NEWURL = objHelperServices.SimpleURL_Str(Strvalue, "ps.aspx", false);
    //                string HREFURL ="/"+ NEWURL + "/ps/";

    //                strhtml.Append("<li style=" + soddevenrow + "><a href='" + HREFURL + "' rel=ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + " ><span>" + Strvalue + "</span>"+" in all Category</a></li>");
    //                //foreach (DataRow dr in dsAutoComplete.Tables[1].Rows)
    //                //{
    //            //    foreach (DataRow dr in dsAdvisor.Tables[1].Rows)
    //            //    {

    //            //        datacnt++;
    //            //        if ((datacnt % 2) == 0)
    //            //        {
    //            //            soddevenrow = "background-color:#fff;";
    //            //        }
    //            //        else
    //            //        {
    //            //            soddevenrow = "background-color:#F9F9F9;";
    //            //        }


    //            //        //tmpstr = dr["val"].ToString().ToUpper();
    //            //        tmpstr = dr["originalquestion"].ToString().ToUpper();
    //            //        tmpstr = tmpstr.Replace(Strvalue.ToUpper(), "<strong>" + Strvalue.ToUpper() + "</strong>");


    //            //        //strhtml.Append("<li style=" + soddevenrow + "><a href='/ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "' >" + tmpstr + "</a></li>");

    //            //        //Modified by :Indu Reason:For URL Rewrite


    //            //        //string NEWURL = objHelperServices.Cons_NewURl_bybrand("ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "", Strvalue, "ps.aspx", "");
    //            //        //string HREFURL = "ps.aspx?" + NEWURL;
    //            //        string NEWURL = objHelperServices.SimpleURL_Str(Strvalue, "ps.aspx", false);
    //            //        string HREFURL = NEWURL + "/ps/";

    //            //        strhtml.Append("<li style=" + soddevenrow + "><a href='" + HREFURL + "' rel=ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + " >" + tmpstr + "</a></li>");
    //            //    }
    //            //    strhtml.Append("</ul>");
    //            //}


    //            //if (dsAdvisor.Tables["items"] != null)
    //            //{

    //            //    strhtml.Append("<div class='viewmoresmall'><strong>Products</strong></div>");

    //            //    int datacntprod = 0;
    //            //    string soddevenrowprod = "drop_products_odd";
    //            //    foreach (DataRow dr in dsAdvisor.Tables["items"].Rows)
    //            //    {

    //            //        eapath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + dr["Family_Id"].ToString();
    //            //        eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
    //            //        temp_product_Image = dr["Prod_Thumbnail"].ToString();
    //            //        temp_fmly_Image = dr["Family_Thumbnail"].ToString();
    //            //        if (temp_product_Image != "")
    //            //            image_string = temp_product_Image.Substring(42);
    //            //        else if (temp_fmly_Image != "")
    //            //            image_string = temp_fmly_Image.Substring(42);
    //            //        else
    //            //            image_string = "noimage.gif";
    //            //        //Fil = new FileInfo(strimgPath + image_string);

    //            //        datacntprod++;
    //            //        if ((datacntprod % 2) == 0)
    //            //        {
    //            //            soddevenrowprod = "drop_products_even";
    //            //        }
    //            //        else
    //            //        {
    //            //            soddevenrowprod = "drop_products_odd";
    //            //        }


    //            //        strhtml.Append("<div  class=" + soddevenrowprod + ">");
    //            //        strhtml.Append("<img width='80' height='80' alt='img' src='prodimages\\" + image_string + "'>");


    //            //        //strhtml.Append("<a href='/pd.aspx?pid=" + dr["Prod_Id"].ToString() + "&amp;fid=" + dr["Family_Id"].ToString() + "&amp;cid=" + dr["CATEGORY_ID"].ToString() + "&amp;path=" + eapath + "'><strong>" + dr["Family_name"].ToString() + "</strong></a>");
    //            //        //Modified by :Indu Reason:For URL Rewrite


    //            //        string NEWURL = objHelperServices.Cons_NewURl_bybrand("pd.aspx?pid=" + dr["Prod_Id"].ToString() + "&amp;fid=" + dr["Family_Id"].ToString() + "&amp;cid=" + dr["CATEGORY_ID"].ToString() + "&amp;path=" + eapath + "'", dr["Family_name"].ToString(), "pd.aspx", "");
    //            //        string HREFURL = "pd.aspx?" + NEWURL;
    //            //        strhtml.Append("<a href='"+ HREFURL +"'  rel=pd.aspx?pid=" + dr["Prod_Id"].ToString() + "&amp;fid=" + dr["Family_Id"].ToString() + "&amp;cid=" + dr["CATEGORY_ID"].ToString() + "&amp;path=" + eapath + "><strong>" + dr["Family_name"].ToString() + "</strong></a>");
    //            //        strhtml.Append("<p>" + dr["Prod_Description"].ToString() + "</p>");
    //            //        strhtml.Append("<div style='Color:red'>Code :<strong>" + dr["Prod_Code"].ToString() + "</strong> &nbsp;&nbsp;&nbsp;&nbsp;Price :<strong style='red'> " + dr["Price"].ToString() + "</strong>");
    //            //        strhtml.Append("</div><div class='clear'></div></div>");

    //            //    }
    //            //}
    //        //    strhtml.Append("</td>");
    //                if (dsAdvisor.Tables["categoryList"] != null || dsAdvisor.Tables["Attribute"] != null)
    //                {
    //                    //  strhtml.Append("<td valign='top' style='width:60%'>");

    //                    if (dsAdvisor.Tables["categoryList"] != null)
    //                    {

    //                        eapath = "AllProducts////WESAUSTRALASIA////UserSearch1=" + Strvalue;
    //                        eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
    //                        //strhtml.Append("<div class='viewmoresmall'><strong>Category</strong></div>");
    //                        //strhtml.Append("<ul>");
    //                        int i = 0;
                         
    //                        foreach (DataRow dr in dsAdvisor.Tables["categoryList"].Rows)
    //                        {
    //                          i = i + 1;

    //                            //datacntcatlist++;
    //                            //if ((datacntcatlist % 2) == 0)
    //                            //{
    //                            //    soddevenrowcatlist = "catlist_li_even";
    //                            //}
    //                            //else
    //                            //{
    //                            //    soddevenrowcatlist = "catlist_li_odd";
    //                            //}

    //                            if (i > 4)
    //                                break;
    //                            //strhtml.Append("<li class=" + soddevenrowcatlist + "><a href='ps.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=Category&amp;value=" + HttpUtility.UrlEncode(dr["name"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + "' style='font-size:10px'>" + dr["name"].ToString() + "</a></li>");
    //                            //Modified by :Indu Reason:For URL Rewrite


    //                             NEWURL = objHelperServices.SimpleURL_Str(dr["name"].ToString(), "ps.aspx", true);
    //                             HREFURL = NEWURL + "/ps/";
    //                            strhtml.Append("<li><a href='" + HREFURL + "' >"+"<span>"+ Strvalue+ "</span>" +" in " + dr["name"].ToString() + "</a></li>");
    //                        }

    //                        //if (dsAdvisor.Tables["categoryList"].Rows.Count > 5)
    //                        //    strhtml.Append("<a style='color:#7AC943;text-decoration:none;font-size:9px;' href='ps.aspx?" + HttpUtility.UrlEncode(Strvalue) + "'>View All Results</a>");

    //                    }
    //                }
    //                //if (dsAdvisor.Tables["Attribute"] != null)
    //                //{
    //                //    foreach (DataRow dr1 in dsAdvisor.Tables["Attribute"].Rows)
    //                //    {

    //                //        //  if (dr1["name"].ToString().ToUpper().Contains("PRODUCTS TAGS") || dr1["name"].ToString().ToUpper().Contains("PRICE"))
    //                //        if (dr1["name"].ToString().ToUpper().Contains("PRODUCTS TAGS"))
    //                //        {
    //                //            DataTable Datar = dsAdvisor.Tables["AttributeValueList"].Select("attribute_id='" + dr1["attribute_id"] + "'").CopyToDataTable();
    //                //            if (Datar != null && Datar.Rows.Count > 0)
    //                //            {
    //                //                eapath = "AllProducts////WESAUSTRALASIA////UserSearch1=" + Strvalue;
    //                //                eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
    //                //                strhtml.Append("<div class='viewmoresmall'><strong>" + dr1["name"].ToString() + "</strong></div>");
    //                //                strhtml.Append("<ul>");
    //                //                int i = 0;
    //                //                int datacntattribute = 0;
    //                //                string soddevenrowattlist = "catlist_li_odd";
    //                //                foreach (DataRow dr in Datar.Rows)
    //                //                {
    //                //                    i = i + 1;
    //                //                    datacntattribute++;
    //                //                    if ((datacntattribute % 2) == 0)
    //                //                    {
    //                //                        soddevenrowattlist = "catlist_li_even";
    //                //                    }
    //                //                    else
    //                //                    {
    //                //                        soddevenrowattlist = "catlist_li_odd";
    //                //                    }

    //                //                    if (i > 5)
    //                //                        break;

    //                //                    //strhtml.Append("<li class=" + soddevenrowattlist + "><a href='ps.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=" + dr["attributeValue"].ToString() + "&amp;value=" + HttpUtility.UrlEncode(dr["Attributevalue"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + "' style='font-size:10px'>" + dr["Attributevalue"].ToString() + "</a></li>");
    //                //                    //Modified by :Indu Reason:For URL Rewrite


    //                //                    //string NEWURL = objHelperServices.SimpleURL_Str(dr["Attributevalue"].ToString(), "ps.aspx", true);
    //                //                    //string HREFURL = NEWURL + "/ps/";
    //                //                    //strhtml.Append("<li class=" + soddevenrowattlist + "><a href='" + HREFURL + "' rel='ps.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=" + dr1["name"].ToString() + "&amp;value=" + HttpUtility.UrlEncode(dr["Attributevalue"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + " style='font-size:10px;'>" + dr["Attributevalue"].ToString() + "</a></li>");



    //                //                }

    //                //                strhtml.Append("</ul>");




    //                //                if (i > 5)
    //                //                {
    //                //                    //  strhtml.Append("<a style='color:#7AC943;text-decoration:none;font-size:9px;' href='ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "'>View All Results</a>");
    //                //                    //Modified by :Indu Reason:For URL Rewrite


    //                //                    string NEWURL1 = objHelperServices.SimpleURL_Str(Strvalue, "ps.aspx", false);
    //                //                    string HREFURL1 = NEWURL1 + "/ps/";
    //                //                    strhtml.Append("<a style='color:#7AC943;text-decoration:none;font-size:9px;' href='" + HREFURL1 + "' rel='ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "'>View All Results</a>");
    //                //                }
    //                //            }




    //                //        }
    //                //    }
    //                //}
    //            }
            
    //            strhtml.Append("</div>");
            
    //            strhtml.Append(" <div class=srch_suggestion>");
    //            int k = 0;
    //            for (int i = 0; i <= dsAdvisor.Tables["items"].Rows.Count; i++)
    //            {
    //                string familyname = dsAdvisor.Tables["items"].Rows[i]["family_name"].ToString();
    //                if (familyname != "")
    //                {
    //                    k = k + 1;
    //                    string NEWURL = objHelperServices.SimpleURL_Str(familyname, "ps.aspx", true);
    //             string   HREFURL = NEWURL + "/ps/";
    //             strhtml.Append("<li><a href='" + HREFURL + "'>" + familyname + "</a></li>");
    //                }
    //                if (k == 5)
    //                {
    //                    break;
    //                }
    //            }

    //   strhtml.Append("</div>");
    //            //if (dsAdvisor.Tables["categoryList"] != null || dsAdvisor.Tables["Attribute"] != null)
    //            //    strhtml.Append("<td colspan='2'>");
    //            //else
    //            //    strhtml.Append("<td>");


    //            //strhtml.Append("<div class='clear'></div>");
    //            // strhtml.Append("<a class='viewmore' href='ps.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "'>View All Results</a>");
    //            string NEWURL2 = objHelperServices.SimpleURL_Str(Strvalue, "ps.aspx", false);
    //            string HREFURL2 ="/"+ NEWURL2 + "/ps/";
    //            if (dsAdvisor.Tables["items"] != null && dsAdvisor.Tables["Attribute"] != null)
    //            {
    //                strhtml.Append("<a class='viewmore' href='" + HREFURL2 + "' >View All Results</a>");
    //            }
    //            //strhtml.Append("<div class='clear'></div>");
                     
    //            strhtml.Append("</div>");
    //        }

    //    }
    //    catch { }

    //    return strhtml.ToString().Trim();
    //}



    [WebMethod]
    public static string GetSearchResult_Auto_Products(string Strvalue,string Main_Micro)
    {
        string sHTML = string.Empty;
        StringBuilder strhtml = new StringBuilder(50000);
        HelperServices objHelperServices = new HelperServices();
        //HelperServices objHelperServices = new HelperServices();
        //EasyAsk_WES ObjEasyAsk = new EasyAsk_WES();
        //HelperDB objHelperDB = new HelperDB();
        Security objSecurity = new Security();
        HelperDB objHelperDB = new HelperDB();
        DataSet dsAutoComplete = new DataSet();
        DataSet dsAdvisor = new DataSet();
        ErrorHandler objErrorHandler = new ErrorHandler();
        string temp_product_Image = string.Empty;
        string temp_fmly_Image = string.Empty;
        string image_string = string.Empty;
        try
        {


            string resultStr = string.Empty;
            string eapath = string.Empty;
            string UserId = string.Empty;
            int pricecode = -1;
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                UserId = HttpContext.Current.Session["USER_ID"].ToString();

            if (UserId == "")
                UserId = "0";
            pricecode = objHelperDB.GetPriceCode(UserId);

            try
            {
                System.Net.WebClient myWebClient = new System.Net.WebClient();
               // objErrorHandler.CreateLog("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete-1.2.1.jsp?fctn=&key=" + HttpUtility.UrlEncode(Strvalue) + "&dct=" + EasyAsk_WebCatDictionary + "&num=3");
                resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete-1.2.1.jsp?fctn=&key=" + HttpUtility.UrlEncode(Strvalue) + "&dct=" + EasyAsk_WebCatDictionary + "&num=3");

            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString() + "Autosuggest Exception");

            }
            if (resultStr != "")
            {

                XmlDocument xd = new XmlDocument();
                resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
                dsAutoComplete = new DataSet();
                dsAutoComplete.ReadXml(new XmlNodeReader(xd));
                //string jsoncid = JsonConvert.SerializeObject(dsAutoComplete);
                //string strxml = HttpContext.Current.Server.MapPath("xml");

                //System.IO.File.WriteAllText(strxml + "\\" + "autosearch.txt", jsoncid);
                //resultStr = resultStr.Replace("\"", "");

            }
            string stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
            StringTemplateGroup _stg_recordsCell = null;
            StringTemplateGroup _stg_container = null;
        StringTemplate _stmpl_records_sugg = null;
                _stg_recordsCell = new StringTemplateGroup("cell_suggestion", stemplatepath);
                TBWDataList[] lstSUGG = new TBWDataList[5];
                int ictrecords_SUG = 0;
                try
                {
                    if (dsAutoComplete.Tables.Count > 1)
                    {
                        foreach (DataRow dr in dsAutoComplete.Tables[1].Rows)
                        {

                            _stmpl_records_sugg = _stg_recordsCell.GetInstanceOf("AutoSearch" + "\\" + "cell_suggestion");
                            string URL_RW_PATH = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr[0].ToString(), "PS.aspx", true);
                            _stmpl_records_sugg.SetAttribute("URL_SUGG", URL_RW_PATH + "/ps/");
                            string STRSUGG = dr[0].ToString();
                            if (STRSUGG.Length > 25)
                            {
                                STRSUGG = STRSUGG.Substring(0, 24) + "...";
                            }
                            _stmpl_records_sugg.SetAttribute("SUGGESTION", STRSUGG);

                            lstSUGG[ictrecords_SUG] = new TBWDataList(_stmpl_records_sugg.ToString());
                            if (ictrecords_SUG == 2)
                            {
                                break;
                            }
                            ictrecords_SUG++;

                        }

                    }


                }
                catch
                { }



            try
            {
                System.Net.WebClient myWebClient = new System.Net.WebClient();
                resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/apps/Advisor.jsp?indexed=1&oneshot=1&ie=UTF-8&disp=json&RequestAction=advisor&RequestData=CA_Search&CatPath=" + HttpUtility.UrlEncode(EasyAsk_WebCatPath) + "&defarrangeby=////NONE////&dct=" + HttpUtility.UrlEncode(EasyAsk_WebCatDictionary) + "&q=" + HttpUtility.UrlEncode(Strvalue) + "&ResultsPerPage=8&eap_PriceCode=" + pricecode.ToString());
            }
            catch { }
            //try
            //{
            //    System.Net.WebClient myWebClient = new System.Net.WebClient();
            //    resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/apps/Advisor.jsp?indexed=1&oneshot=1&ie=UTF-8&disp=json&RequestAction=advisor&RequestData=CA_Search&CatPath=" + HttpUtility.UrlEncode(EasyAsk_WebCatPath) + "&defarrangeby=////NONE////&dct=" + HttpUtility.UrlEncode(EasyAsk_WebCatDictionary) + "&q=" + HttpUtility.UrlEncode(Strvalue) + "&ResultsPerPage=4&eap_PriceCode=" + pricecode.ToString());
            //}
            //catch { }
            string HREFURL = string.Empty;
            if (resultStr != "")
            {

                XmlDocument xd = new XmlDocument();
                resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
                dsAdvisor = new DataSet();
                dsAdvisor.ReadXml(new XmlNodeReader(xd));
            }
            DataTable Sqltb = new DataTable();
            DataColumn dc = null;
            string tmpstr = string.Empty;
         
                if (dsAdvisor.Tables["items"] != null)
                {
                    if (dsAdvisor.Tables["items"].Columns["CATEGORY_PATH"] == null)
                    {
                        dc = new DataColumn("CATEGORY_PATH", typeof(string));
                        dc.DefaultValue = "";
                        dsAdvisor.Tables["items"].Columns.Add(dc);
                    }
                    if (dsAdvisor.Tables["items"].Columns["CATEGORY_ID"] == null)
                    {
                        dc = new DataColumn("CATEGORY_ID", typeof(string));
                        dc.DefaultValue = "";
                        dsAdvisor.Tables["items"].Columns.Add(dc);
                    }
                    if (dsAdvisor.Tables["items"].Columns["Prod_Thumbnail"] == null)
                    {
                        dc = new DataColumn("Prod_Thumbnail", typeof(string));
                        dc.DefaultValue = "";
                        dsAdvisor.Tables["items"].Columns.Add(dc);
                    }
                    if (dsAdvisor.Tables["items"].Columns["Family_Thumbnail"] == null)
                    {
                        dc = new DataColumn("Family_Thumbnail", typeof(string));
                        dc.DefaultValue = "";
                        dsAdvisor.Tables["items"].Columns.Add(dc);
                    }
                    if (dsAdvisor.Tables["items"].Columns["Prod_Description"] == null)
                    {
                        dc = new DataColumn("Prod_Description", typeof(string));
                        dc.DefaultValue = "";
                        dsAdvisor.Tables["items"].Columns.Add(dc);
                    }


                    StringTemplate _stmpl_container = null;
                    _stg_container = new StringTemplateGroup("main", stemplatepath);

                    TBWDataList[] lstrecords = new TBWDataList[6];

                    try
                    {
                        if (dsAdvisor.Tables["items"] != null)
                        {











                            string NEWURL = objHelperServices.SimpleURL_Str(Strvalue, "ps.aspx", false);
                             HREFURL = "/" + NEWURL + "/ps/";

                        
                            StringTemplate _stmpl_records = null;
                            _stg_recordsCell = new StringTemplateGroup("cell", stemplatepath);
                       
                         

                            int ictrecords = 0;

                            //  strhtml.Append("<td valign='top' style='width:60%'>");



                            eapath = "AllProducts////WESAUSTRALASIA////UserSearch1=" + Strvalue;
                            eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
                            //strhtml.Append("<div class='viewmoresmall'><strong>Category</strong></div>");
                            //strhtml.Append("<ul>");
                            int i = 0;

                            foreach (DataRow dr in dsAdvisor.Tables["items"].Rows)
                            {
                                _stmpl_records = _stg_recordsCell.GetInstanceOf("AutoSearch" + "\\" + "cell");

                                temp_product_Image = dr["Prod_Thumbnail"].ToString().Replace("http://staging.wesonline.com.au", "");
                                temp_fmly_Image = dr["Family_Thumbnail"].ToString().Replace("http://staging.wesonline.com.au", "");
                                if (temp_product_Image != "")
                                    image_string = temp_product_Image.ToUpper().Replace("_TH", "_TH50");
                                else if (temp_fmly_Image != "")
                                    image_string = temp_fmly_Image.ToUpper().Replace("_TH", "_TH50");
                                else
                                    image_string = "noimage.gif";


                                _stmpl_records.SetAttribute("TBT_PRODUCT_TH_IMAGE", image_string);

                                _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"].ToString());
                                _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"].ToString());
                                _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"].ToString());


                                string[] catpath = dr["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);


                                // string    URL_RW_PATH = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr["FAMILY_ID"] + "////" + dr["product_ID"] + "=" + dr["Prod_Code"] + "////" + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + dr["FAMILY_name"], "pd.aspx", true);
                                string URL_RW_PATH = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr["FAMILY_ID"] + "////" + dr["prod_ID"] + "=" + dr["Prod_Code"] + "////" + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + dr["FAMILY_name"], "pd.aspx", true);
                                _stmpl_records.SetAttribute("TBT_REWRITEURL", URL_RW_PATH + "/pd/");

                                _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"].ToString());

                                _stmpl_records.SetAttribute("PRODUCT_CODE", dr["Prod_Code"].ToString());
                                _stmpl_records.SetAttribute("PRODUCT_COST", dr["Price"].ToString());
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                if (ictrecords == 5)
                                {
                                    break;
                                }
                                ictrecords++;

                            }
                        }
                    }
                    catch
                    { }

                            StringTemplate _stmpl_records_price = null;
                            _stg_recordsCell = new StringTemplateGroup("cell_Price", stemplatepath);

                            TBWDataList[] lstprice=null;
                    try
                    {
                            if (dsAdvisor.Tables["AttributeValueList"] != null)
                            {
                          
                                    //  if (dr1["name"].ToString().ToUpper().Contains("PRODUCTS TAGS") || dr1["name"].ToString().ToUpper().Contains("PRICE"))

                                DataTable Datar = dsAdvisor.Tables["AttributeValueList"].Select("nodestring like 'Price-Range%'").CopyToDataTable();
                                        if (Datar != null && Datar.Rows.Count > 0)
                                        {


                                          lstprice = new TBWDataList[Datar.Rows.Count];
                                            int ictrecords_price = 0;

                                          
                                            foreach (DataRow dr in Datar.Rows)
                                            {

                                                _stmpl_records_price = _stg_recordsCell.GetInstanceOf("AutoSearch" + "\\" + "cell_price");

                                                //strhtml.Append("<li class=" + soddevenrowattlist + "><a href='ps.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=" + dr["attributeValue"].ToString() + "&amp;value=" + HttpUtility.UrlEncode(dr["Attributevalue"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + "' style='font-size:10px'>" + dr["Attributevalue"].ToString() + "</a></li>");
                                                //Modified by :Indu Reason:For URL Rewrite


                                            
                                                string    URL_RW_PATH ="AllProducts////WESAUSTRALASIA////UserSearch1=" + Strvalue + "@@Price Range@@" + dr["Attributevalue"].ToString();
                                                _stmpl_records_price.SetAttribute("TBW_ATTRIBUTE_SEARCH", URL_RW_PATH);
                                             string ID=   objHelperServices.SimpleURL_Str( dr["Attributevalue"].ToString(),"",false);
                                                      _stmpl_records_price.SetAttribute("TBW_ATTRIBUTE_NAME", ID);
                                                _stmpl_records_price.SetAttribute("TBW_ATTRIBUTE_VALUE", dr["Attributevalue"].ToString());
                                                _stmpl_records_price.SetAttribute("TBW_PRO_CNT", dr["productcount"].ToString());

                                                _stmpl_records_price.SetAttribute("STRVALUE", Strvalue);
                                                lstprice[ictrecords_price] = new TBWDataList(_stmpl_records_price.ToString());
                                                if (ictrecords_price == 6)
                                                {
                                                    break;
                                                }
                                                ictrecords_price++;

                                            }

                                         
                                        }




                                    
                                
                            }
                    }
                    catch
                    {}



                            StringTemplate _stmpl_records_Category = null;
                            _stg_recordsCell = new StringTemplateGroup("cell_Category", stemplatepath);
                            TBWDataList[] lstcategory = new TBWDataList[5];
                            int ictrecords_category = 0;

                    try
                    {
                            if (dsAdvisor.Tables["CategoryList"] != null)
                            {
                               
                                //  if (dr1["name"].ToString().ToUpper().Contains("PRODUCTS TAGS") || dr1["name"].ToString().ToUpper().Contains("PRICE"))
                                //string jsoncid = JsonConvert.SerializeObject(dsAdvisor.Tables["CategoryList"]);
                                //string strxml = HttpContext.Current.Server.MapPath("xml");

                                //System.IO.File.WriteAllText(strxml + "\\" + "categorysearch.txt", jsoncid);


                                DataTable Datar = dsAdvisor.Tables["CategoryList"].Select("ids not like '%SPF-%'").CopyToDataTable();
                                Datar.DefaultView.Sort = "productcount desc";

                                DataTable dt = Datar.DefaultView.ToTable();


                                foreach (DataRow dr in dt.Rows)
                                {
                                    _stmpl_records_Category = _stg_recordsCell.GetInstanceOf("AutoSearch" + "\\" + "cell_Category");


                                    //strhtml.Append("<li class=" + soddevenrowattlist + "><a href='ps.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=" + dr["attributeValue"].ToString() + "&amp;value=" + HttpUtility.UrlEncode(dr["Attributevalue"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + "' style='font-size:10px'>" + dr["Attributevalue"].ToString() + "</a></li>");
                                    //Modified by :Indu Reason:For URL Rewrite

                                    string URL_RW_PATH = "AllProducts////WESAUSTRALASIA////UserSearch1=" + Strvalue + "@@Category@@" + dr["name"].ToString();
                                    _stmpl_records_Category.SetAttribute("TBW_ATTRIBUTE_SEARCH", URL_RW_PATH);

                                    string ID = objHelperServices.SimpleURL_Str(dr["name"].ToString(), "", false);
                                    _stmpl_records_Category.SetAttribute("TBW_ATTRIBUTE_NAME", ID);
                                  

                                    _stmpl_records_Category.SetAttribute("TBW_ATTRIBUTE_VALUE", dr["name"].ToString());
                                    _stmpl_records_Category.SetAttribute("TBW_PRO_CNT", dr["productcount"].ToString());

                                    _stmpl_records_Category.SetAttribute("STRVALUE", Strvalue);
                                    


                                    lstcategory[ictrecords_category] = new TBWDataList(_stmpl_records_Category.ToString());
                                    if (ictrecords_category == 4)
                                    {
                                        break;
                                    }
                                    ictrecords_category++;

                                }
                            }
                    }
                    catch
                    {}

                            string totalprodcnt = "0";
                            if (dsAdvisor.Tables["ItemDescription"] != null)
                            {
                                totalprodcnt = dsAdvisor.Tables["ItemDescription"].Rows[0]["TotalItems"].ToString();
   
                            }


                _stmpl_container = _stg_container.GetInstanceOf("AutoSearch" + "\\" + "main");

                _stmpl_container.SetAttribute("STRTOTAL", totalprodcnt);
                _stmpl_container.SetAttribute("STRSEARCHURL", HREFURL);
                                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                                _stmpl_container.SetAttribute("TBWDataList_Sugg", lstSUGG);
                                _stmpl_container.SetAttribute("TBWDataList_Price", lstprice);
                                _stmpl_container.SetAttribute("TBWDataList_Category", lstcategory);
             
string shtml=_stmpl_container.ToString(); 
                          
       

strhtml.Append(shtml);
            }
            return strhtml.ToString().Trim();
        }
        catch (Exception ex) {

            return "";
        }

       
    }
        [WebMethod]
    public static string GetSearchResultMS_Auto_Products(string Strvalue, string Main_Micro)
    {
        string sHTML = string.Empty;
        StringBuilder strhtml = new StringBuilder(50000);
        HelperServices objHelperServices = new HelperServices();
        //HelperServices objHelperServices = new HelperServices();
        //EasyAsk_WES ObjEasyAsk = new EasyAsk_WES();
        //HelperDB objHelperDB = new HelperDB();
        Security objSecurity = new Security();
        HelperDB objHelperDB = new HelperDB();
        DataSet dsAutoComplete = new DataSet();
        DataSet dsAdvisor = new DataSet();
        ErrorHandler objErrorHandler = new ErrorHandler();
        string temp_product_Image = string.Empty;
        string temp_fmly_Image = string.Empty;
        string image_string = string.Empty;
        string HREFURL = string.Empty;
        try
        {


            string resultStr = string.Empty;
            string eapath = string.Empty;
            string UserId = string.Empty;
            int pricecode = -1;
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                UserId = HttpContext.Current.Session["USER_ID"].ToString();

            if (UserId == "")
                UserId = "0";
            pricecode = objHelperDB.GetPriceCode(UserId);

            try
            {
                System.Net.WebClient myWebClient = new System.Net.WebClient();
                //objErrorHandler.CreateLog("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete-1.2.1.jsp?fctn=&key=" + HttpUtility.UrlEncode(Strvalue) + "&dct=" + EasyAsk_WebCatDictionary + "&num=4");
                resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete-1.2.1.jsp?fctn=&key=" + HttpUtility.UrlEncode(Strvalue) + "&dct=" + EasyAsk_WebCatDictionary + "&num=4");

            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString() + "Autosuggest Exception");

            }
            if (resultStr != "")
            {

                XmlDocument xd = new XmlDocument();
                resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
                dsAutoComplete = new DataSet();
                dsAutoComplete.ReadXml(new XmlNodeReader(xd));
                //string jsoncid = JsonConvert.SerializeObject(dsAutoComplete);
                //string strxml = HttpContext.Current.Server.MapPath("xml");

                //System.IO.File.WriteAllText(strxml + "\\" + "autosearch.txt", jsoncid);
                //resultStr = resultStr.Replace("\"", "");

            }
            string stemplatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
            StringTemplateGroup _stg_recordsCell = null;
            StringTemplateGroup _stg_container = null;
            StringTemplate _stmpl_records_sugg = null;
            _stg_recordsCell = new StringTemplateGroup("cell_suggestion", stemplatepath);
            TBWDataList[] lstSUGG = new TBWDataList[5];
            int ictrecords_SUG = 0;
            try
            {
                if (dsAutoComplete.Tables.Count > 1)
                {

                    foreach (DataRow dr in dsAutoComplete.Tables[1].Rows)
                    {

                        _stmpl_records_sugg = _stg_recordsCell.GetInstanceOf("AutoSearch" + "\\" + "cell_suggestion");
                        string URL_RW_PATH = objHelperServices.SimpleURL_MS_Str("AllProducts////WESAUSTRALASIA////" + Main_Micro + "////" + dr[0].ToString(), "PS.aspx", true);
                        _stmpl_records_sugg.SetAttribute("URL_SUGG", URL_RW_PATH + "/mps/");
                        string STRSUGG = dr[0].ToString();
                        if (STRSUGG.Length > 25)
                        {
                            STRSUGG = STRSUGG.Substring(0, 24) + "...";
                        }
                        _stmpl_records_sugg.SetAttribute("SUGGESTION", STRSUGG);

                        lstSUGG[ictrecords_SUG] = new TBWDataList(_stmpl_records_sugg.ToString());
                        if (ictrecords_SUG == 3)
                        {
                            break;
                        }
                        ictrecords_SUG++;

                    }

                }
            }
            catch

            { }






            try
            {
                System.Net.WebClient myWebClient = new System.Net.WebClient();
                resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/apps/Advisor.jsp?indexed=1&oneshot=1&ie=UTF-8&disp=json&RequestAction=advisor&RequestData=CA_Search&CatPath=" + HttpUtility.UrlEncode(EasyAsk_WebCatPath+"////"+Main_Micro) + "&defarrangeby=////NONE////&dct=" + HttpUtility.UrlEncode(EasyAsk_WebCatDictionary) + "&q=" + HttpUtility.UrlEncode(Strvalue) + "&ResultsPerPage=7&eap_PriceCode=" + pricecode.ToString());
            }
            catch { }
            //try
            //{
            //    System.Net.WebClient myWebClient = new System.Net.WebClient();
            //    resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/apps/Advisor.jsp?indexed=1&oneshot=1&ie=UTF-8&disp=json&RequestAction=advisor&RequestData=CA_Search&CatPath=" + HttpUtility.UrlEncode(EasyAsk_WebCatPath) + "&defarrangeby=////NONE////&dct=" + HttpUtility.UrlEncode(EasyAsk_WebCatDictionary) + "&q=" + HttpUtility.UrlEncode(Strvalue) + "&ResultsPerPage=10&eap_PriceCode=" + pricecode.ToString());
            //}
            //catch { }
         
            if (resultStr != "")
            {

                XmlDocument xd = new XmlDocument();
                resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
                dsAdvisor = new DataSet();
                dsAdvisor.ReadXml(new XmlNodeReader(xd));
            }
            DataTable Sqltb = new DataTable();
            DataColumn dc = null;
            string tmpstr = string.Empty;
            if (dsAdvisor.Tables["items"] != null)
            {
                if (dsAdvisor.Tables["items"].Columns["CATEGORY_PATH"] == null)
                {
                    dc = new DataColumn("CATEGORY_PATH", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);
                }
                if (dsAdvisor.Tables["items"].Columns["CATEGORY_ID"] == null)
                {
                    dc = new DataColumn("CATEGORY_ID", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);
                }
                if (dsAdvisor.Tables["items"].Columns["Prod_Thumbnail"] == null)
                {
                    dc = new DataColumn("Prod_Thumbnail", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);
                }
                if (dsAdvisor.Tables["items"].Columns["Family_Thumbnail"] == null)
                {
                    dc = new DataColumn("Family_Thumbnail", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);
                }
                if (dsAdvisor.Tables["items"].Columns["Prod_Description"] == null)
                {
                    dc = new DataColumn("Prod_Description", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);
                }





            }
              StringTemplate _stmpl_container = null;
              TBWDataList[] lstrecords = new TBWDataList[6];
            try
            {

                if (dsAdvisor.Tables["items"] != null)
                {











                    string NEWURL = objHelperServices.SimpleURL_MS_Str(Main_Micro + "////" + Strvalue, "ps.aspx", false);
                     HREFURL = "/" + NEWURL + "/mps/";

                  
                    StringTemplate _stmpl_records = null;
                    _stg_recordsCell = new StringTemplateGroup("cell", stemplatepath);
                    _stg_container = new StringTemplateGroup("main", stemplatepath);
               

                    int ictrecords = 0;

                    //  strhtml.Append("<td valign='top' style='width:60%'>");



                    //eapath = "AllProducts////WESAUSTRALASIA////UserSearch1=" + Strvalue;
                    // eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
                    //strhtml.Append("<div class='viewmoresmall'><strong>Category</strong></div>");
                    //strhtml.Append("<ul>");
                    int i = 0;

                    foreach (DataRow dr in dsAdvisor.Tables["items"].Rows)
                    {
                        _stmpl_records = _stg_recordsCell.GetInstanceOf("AutoSearch" + "\\" + "cell");

                        temp_product_Image = dr["Prod_Thumbnail"].ToString().Replace("http://staging.wesonline.com.au", "");
                        temp_fmly_Image = dr["Family_Thumbnail"].ToString().Replace("http://staging.wesonline.com.au", "");
                        if (temp_product_Image != "")
                            image_string = temp_product_Image.ToUpper().Replace("_TH", "_TH50");
                        else if (temp_fmly_Image != "")
                            image_string = temp_fmly_Image.Replace("_TH", "_TH50");
                        else
                            image_string = "noimage.gif";


                        _stmpl_records.SetAttribute("TBT_PRODUCT_TH_IMAGE", image_string);

                        _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"].ToString());
                        _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"].ToString());
                        _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"].ToString());


                        //   string[] catpath = dr["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                        string[] catpath = dr["CATEGORY_PATH"].ToString().ToLower().Replace("supplier product feed////", "").Split(new string[] { "////" }, StringSplitOptions.None);
                        string URL_RW_PATH = objHelperServices.SimpleURL_MS_Str("AllProducts////WESAUSTRALASIA////" + "////" + Main_Micro + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + dr["FAMILY_name"].ToString() + "////" + dr["prod_ID"] + "=" + dr["Prod_Code"].ToString() + "////" + dr["FAMILY_ID"].ToString(), "mpd.aspx", true);

                        // string URL_RW_PATH = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + dr["FAMILY_ID"] + "////" + dr["product_ID"] + "=" + dr["Prod_Code"] + "////" + Main_Micro + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + dr["FAMILY_name"], "pd.aspx", true);
                        _stmpl_records.SetAttribute("TBT_REWRITEURL", URL_RW_PATH + "/mpd/");

                        _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"].ToString());

                        _stmpl_records.SetAttribute("PRODUCT_CODE", dr["Prod_Code"].ToString());
                        _stmpl_records.SetAttribute("PRODUCT_COST", dr["Price"].ToString());
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        if (ictrecords == 5)
                        {
                            break;
                        }
                        ictrecords++;

                    }
                }
            }
                catch 
            {}
                    StringTemplate _stmpl_records_price = null;
                    _stg_recordsCell = new StringTemplateGroup("cell_Price", stemplatepath);

                    TBWDataList[] lstprice = null;
            try
            {
                    if (dsAdvisor.Tables["AttributeValueList"] != null)
                    {

                        //  if (dr1["name"].ToString().ToUpper().Contains("PRODUCTS TAGS") || dr1["name"].ToString().ToUpper().Contains("PRICE"))

                        DataTable Datar = dsAdvisor.Tables["AttributeValueList"].Select("nodestring like 'Price-Range%'").CopyToDataTable();
                        if (Datar != null && Datar.Rows.Count > 0)
                        {


                            lstprice = new TBWDataList[Datar.Rows.Count];
                            int ictrecords_price = 0;


                            foreach (DataRow dr in Datar.Rows)
                            {

                                _stmpl_records_price = _stg_recordsCell.GetInstanceOf("AutoSearch" + "\\" + "cell_price");

                                //strhtml.Append("<li class=" + soddevenrowattlist + "><a href='ps.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=" + dr["attributeValue"].ToString() + "&amp;value=" + HttpUtility.UrlEncode(dr["Attributevalue"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + "' style='font-size:10px'>" + dr["Attributevalue"].ToString() + "</a></li>");
                                //Modified by :Indu Reason:For URL Rewrite



                                string URL_RW_PATH = "AllProducts////WESAUSTRALASIA////" + Main_Micro + "////" + "UserSearch1=" + Strvalue + "@@Price Range@@" + dr["Attributevalue"].ToString();
                                _stmpl_records_price.SetAttribute("TBW_ATTRIBUTE_SEARCH", URL_RW_PATH);
                                string ID = objHelperServices.SimpleURL_Str(dr["Attributevalue"].ToString(), "", false);
                                _stmpl_records_price.SetAttribute("TBW_ATTRIBUTE_NAME", ID);
                                _stmpl_records_price.SetAttribute("TBW_ATTRIBUTE_VALUE", dr["Attributevalue"].ToString());
                                _stmpl_records_price.SetAttribute("TBW_PRO_CNT", dr["productcount"].ToString());

                                _stmpl_records_price.SetAttribute("STRVALUE", Strvalue);
                                lstprice[ictrecords_price] = new TBWDataList(_stmpl_records_price.ToString());
                                if (ictrecords_price == 6)
                                {
                                    break;
                                }
                                ictrecords_price++;

                            }


                        }






                    }
                }
                catch 
            {}




                StringTemplate _stmpl_records_Category = null;
                _stg_recordsCell = new StringTemplateGroup("cell_Category", stemplatepath);
                TBWDataList[] lstcategory = new TBWDataList[5];
                int ictrecords_category = 0;

            try
            {
                if (dsAdvisor.Tables["CategoryList"] != null)
                {

                    //  if (dr1["name"].ToString().ToUpper().Contains("PRODUCTS TAGS") || dr1["name"].ToString().ToUpper().Contains("PRICE"))
                    //string jsoncid = JsonConvert.SerializeObject(dsAdvisor.Tables["CategoryList"]);
                    //string strxml = HttpContext.Current.Server.MapPath("xml");

                    //System.IO.File.WriteAllText(strxml + "\\" + "categorysearch.txt", jsoncid);


                    //DataTable Datar = dsAdvisor.Tables["CategoryList"].Select("ids  like '%SPF-%'").CopyToDataTable();
                    //Datar.DefaultView.Sort = "productcount desc";

                    DataTable dt = dsAdvisor.Tables["CategoryList"];


                    foreach (DataRow dr in dt.Rows)
                    {
                        _stmpl_records_Category = _stg_recordsCell.GetInstanceOf("AutoSearch" + "\\" + "cell_Category");


                        //strhtml.Append("<li class=" + soddevenrowattlist + "><a href='ps.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=" + dr["attributeValue"].ToString() + "&amp;value=" + HttpUtility.UrlEncode(dr["Attributevalue"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + "' style='font-size:10px'>" + dr["Attributevalue"].ToString() + "</a></li>");
                        //Modified by :Indu Reason:For URL Rewrite

                        string URL_RW_PATH = "AllProducts////WESAUSTRALASIA////"+Main_Micro+"////" +"UserSearch1=" + Strvalue + "@@Category@@" + dr["name"].ToString();
                        _stmpl_records_Category.SetAttribute("TBW_ATTRIBUTE_SEARCH", URL_RW_PATH);

                        string ID = objHelperServices.SimpleURL_Str(dr["name"].ToString(), "", false);
                        _stmpl_records_Category.SetAttribute("TBW_ATTRIBUTE_NAME", ID);


                        _stmpl_records_Category.SetAttribute("TBW_ATTRIBUTE_VALUE", dr["name"].ToString());
                        _stmpl_records_Category.SetAttribute("TBW_PRO_CNT", dr["productcount"].ToString());

                        _stmpl_records_Category.SetAttribute("STRVALUE", Strvalue);



                        lstcategory[ictrecords_category] = new TBWDataList(_stmpl_records_Category.ToString());
                        if (ictrecords_category == 4)
                        {
                            break;
                        }
                        ictrecords_category++;

                    }
                }
            }
            catch 
            {}

                string totalprodcnt = "0";
                if (dsAdvisor.Tables["ItemDescription"] != null)
                {
                    totalprodcnt = dsAdvisor.Tables["ItemDescription"].Rows[0]["TotalItems"].ToString();

                }


                _stmpl_container = _stg_container.GetInstanceOf("AutoSearch" + "\\" + "main");

                _stmpl_container.SetAttribute("STRTOTAL", totalprodcnt);
                _stmpl_container.SetAttribute("STRSEARCHURL", HREFURL);
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                _stmpl_container.SetAttribute("TBWDataList_Sugg", lstSUGG);
                _stmpl_container.SetAttribute("TBWDataList_Price", lstprice);
                _stmpl_container.SetAttribute("TBWDataList_Category", lstcategory);

                string shtml = _stmpl_container.ToString();



                strhtml.Append(shtml);
            
            return strhtml.ToString().Trim();
        }
        catch (Exception ex)
        {

            return "";
        }


    }  
    
    
    
    
    //[WebMethod]
    //public static string GetSearchResult(string Strvalue)
    //{
    //    DataSet ResultDs = new DataSet();
    //    DataSet ResultAttDs = new DataSet();
    //    string sHTML = string.Empty;

    //    string temp_product_Image = string.Empty;
    //    string temp_fmly_Image = string.Empty;
    //    string image_string = string.Empty;
    //    FileInfo Fil;
    //    string UserId = string.Empty;
    //    string tmpstr = string.Empty;
    //    string tmpstrCode = string.Empty;
        
    //    if( HttpContext.Current.Session["USER_ID"]!=null && HttpContext.Current.Session["USER_ID"]!="")
    //        UserId= HttpContext.Current.Session["USER_ID"].ToString();

    //    if (UserId == "")
    //        UserId = "0";

    //    HelperServices objHelperServices = new HelperServices();
    //    EasyAsk_WAGNER ObjEasyAsk = new EasyAsk_WAGNER(); 
    //    HelperDB objHelperDB = new HelperDB();
    //    Security objSecurity = new Security();
    //    ErrorHandler objErrorHandler = new ErrorHandler();
    //    string stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
    //    // Db calls
    //    //int priceCode = objHelperDB.GetPriceCode(UserId.ToString());
    //    //ResultDs = objHelperServices.GetPowerSearchProducts(Strvalue, priceCode, Convert.ToInt32(UserId));
    //    // Db calls
    //    Hashtable Autosuggestion = GetAutoCompleteResult(Strvalue);
       
    //    //EA Calls
    //    ResultDs = ObjEasyAsk.GetDropDownPowerSearchProducts(Strvalue,"5");
    //    ResultAttDs = (DataSet)HttpContext.Current.Session["SearchAttributes"];
    //    //EA Calls
    //    if (ResultDs != null && ResultDs.Tables.Count >= 2 && (ResultDs.Tables[0].Rows.Count > 0 || ResultDs.Tables[1].Rows.Count > 0))
    //    {
    //        try
    //        {
    //            StringTemplateGroup _stg_container = null;
    //            StringTemplateGroup _stg_recordsRow = null;
    //            StringTemplateGroup _stg_recordsCell = null;
    //            StringTemplate _stmpl_container = null;
    //            StringTemplate _stmpl_records = null;
    //            StringTemplate _stmpl_records1 = null;
    //            StringTemplate _stmpl_recordsrows = null;
    //            TBWDataList[] lstrecords = new TBWDataList[0];


    //            StringTemplateGroup _stg_container1 = null;
    //            StringTemplateGroup _stg_records1 = null;
    //            TBWDataList1[] lstrecords1 = new TBWDataList1[0];

                
    //            string SearchAttr = "";
                
    //            _stg_recordsRow = new StringTemplateGroup("row", stemplatepath);
    //            _stg_recordsCell = new StringTemplateGroup("cell", stemplatepath);
    //            _stg_container = new StringTemplateGroup("main", stemplatepath);


              



    //            int ictrecords = 0;
    //            int ictrecords1 = 0;
    //            DataRow dRow;
    //            //foreach (DataRow dr in ResultDs.Tables[1].Rows)//For Records
    //            //{
    //            //    tmpstrCode = "";
    //            //    tmpstr = dr["FAMILY_NAME"].ToString().ToUpper();
    //            //    if (tmpstrCode == "" && tmpstr.Contains(Strvalue.ToUpper()) == true)
    //            //        tmpstrCode = tmpstr;

    //            //    tmpstr = dr["ProdCode"].ToString().ToUpper();
    //            //    if (tmpstrCode == "" && tmpstr.Contains(Strvalue.ToUpper()) == true)
    //            //        tmpstrCode = tmpstr;

    //            //    tmpstr = dr["ShortDesc"].ToString().ToUpper();
    //            //    if (tmpstrCode == "" && tmpstr.Contains(Strvalue.ToUpper()) == true)
    //            //        tmpstrCode = tmpstr;

    //            //    if (tmpstrCode != "")
    //            //    {
    //            //        dRow = ResultDs.Tables[0].NewRow();
    //            //        dRow["Code"] = tmpstrCode;
    //            //        ResultDs.Tables[0].Rows.Add(dRow);
    //            //    }


    //            //}
    //            SearchAttr = GetSearchResultAttr(Strvalue, stemplatepath);
    //            foreach (string str in Autosuggestion.Values)//For Records
    //            {
                    
                   
    //                    dRow = ResultDs.Tables[0].NewRow();
    //                    dRow["Code"] = str;
    //                    ResultDs.Tables[0].Rows.Add(dRow);
                   


    //            }
    //            lstrecords = new TBWDataList[ResultDs.Tables[0].Rows.Count + 1];
    //            lstrecords1 = new TBWDataList1[ResultDs.Tables[1].Rows.Count + 1];

               


    //            foreach (DataRow dr in ResultDs.Tables[0].Rows)//For Records
    //            {

    //                _stmpl_records = _stg_recordsCell.GetInstanceOf("ToPSearch" + "\\" + "cell");
    //                if (Strvalue != "")
    //                {
    //                    tmpstr=dr["Code"].ToString().ToUpper();                        
    //                    tmpstr =tmpstr.Replace(Strvalue.ToUpper(), "<strong>" + Strvalue.ToUpper() + "</strong>");
    //                    _stmpl_records.SetAttribute("TBT_SEARCH_TEXT_FORMAT", tmpstr);
    //                }
    //                else
    //                    _stmpl_records.SetAttribute("TBT_SEARCH_TEXT_FORMAT", dr["Code"].ToString().ToUpper());

    //                _stmpl_records.SetAttribute("TBT_SEARCH_TEXT", dr["Code"].ToString().ToUpper());

    //                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
    //                ictrecords++;
    //            }
    //            foreach (DataRow dr in ResultDs.Tables[1].Rows)//For Records
    //            {

    //                _stmpl_records1 = _stg_recordsRow.GetInstanceOf("ToPSearch" + "\\" + "row");

    //                temp_product_Image = dr["ProdTh"].ToString();

    //                temp_fmly_Image = dr["FamilyTh"].ToString();



    //                if (temp_product_Image != "")
    //                    image_string = temp_product_Image;
    //                else if (temp_fmly_Image != "")
    //                    image_string = temp_fmly_Image;
    //                else
    //                    image_string = "noimage.gif";

    //                Fil = new FileInfo(strimgPath + image_string);

    //                if (Fil.Exists)
    //                    _stmpl_records1.SetAttribute("TBT_PRODUCT_TH_IMAGE", image_string);
    //                else
    //                    _stmpl_records1.SetAttribute("TBT_PRODUCT_TH_IMAGE", "/noimage.gif");


    //                _stmpl_records1.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"].ToString());
    //                _stmpl_records1.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"].ToString());
    //                _stmpl_records1.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"].ToString());

    //                string eapath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + dr["FAMILY_ID"].ToString();

    //                _stmpl_records1.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
    //                _stmpl_records1.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"].ToString());
    //                _stmpl_records1.SetAttribute("TBT_FAMILY_TITLE", dr["FAMILY_NAME"].ToString().Replace('"',' '));
    //                string desc =  dr["FamilyDesc"].ToString();
    //                if (desc.Length > 150)
    //                {
    //                    _stmpl_records1.SetAttribute("TBT_SHOW_MORE", true);
    //                    desc = desc.Substring(0, 150).ToString();
    //                    _stmpl_records1.SetAttribute("FAMILY_DESC", desc);
    //                }
    //                else
    //                {
    //                    _stmpl_records1.SetAttribute("FAMILY_DESC", desc);
    //                    _stmpl_records1.SetAttribute("TBT_SHOW_MORE", false);
    //                }

                    
    //                _stmpl_records1.SetAttribute("SHORT_DESC", dr["ShortDesc"].ToString());
    //                _stmpl_records1.SetAttribute("PRODUCT_CODE", dr["ProdCode"].ToString());
    //                _stmpl_records1.SetAttribute("PRODUCT_PRICE", dr["cost"].ToString());
    //                lstrecords1[ictrecords1] = new TBWDataList1(_stmpl_records1.ToString());
    //                ictrecords1++;
    //            }
    //            _stmpl_container = _stg_container.GetInstanceOf("ToPSearch" + "\\" + "main");

    //            if (ResultDs.Tables[0].Rows.Count > 0)
    //                _stmpl_container.SetAttribute("TBT_SEARCH_DISPLAY", true);
    //            else
    //                _stmpl_container.SetAttribute("TBT_SEARCH_DISPLAY", false);
    //            _stmpl_container.SetAttribute("TBT_SEARCH_TEXT", Strvalue);
    //            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
    //            _stmpl_container.SetAttribute("TBWDataList1", lstrecords1);
    //            _stmpl_container.SetAttribute("TBT_SEARCH_ATTR", SearchAttr);
    //            sHTML = _stmpl_container.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.ErrorMsg = ex;
    //            objErrorHandler.CreateLog();
    //            sHTML = "";
    //        }

    //    }

    //    return sHTML;
    //}

    public static string GetSearchResultAttr(string Strvalue, string stemplatepath)
    {
        DataSet ResultDs = new DataSet();

        string sHTML = string.Empty;

     
       
        //string UserId = "";
   
      
        //if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
        //    UserId = HttpContext.Current.Session["USER_ID"].ToString();

        //if (UserId == "")
        //    UserId = "0";

        HelperServices objHelperServices = new HelperServices();
        EasyAsk_WAGNER ObjEasyAsk = new EasyAsk_WAGNER();
        HelperDB objHelperDB = new HelperDB();
        Security objSecurity = new Security();
        ErrorHandler objErrorHandler = new ErrorHandler();
      
        ResultDs = (DataSet)HttpContext.Current.Session["SearchAttributes"];
        //EA Calls
        if (ResultDs != null && ResultDs.Tables.Count >0 )
        {
            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_recordsRow = null;
                StringTemplateGroup _stg_recordsCell = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_records1 = null;
                StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrows = new TBWDataList[0];


                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                TBWDataList1[] lstrecords1 = new TBWDataList1[0];



                _stg_recordsRow = new StringTemplateGroup("row", stemplatepath);
                _stg_recordsCell = new StringTemplateGroup("cell", stemplatepath);
                _stg_container = new StringTemplateGroup("main", stemplatepath);






                 int ictrows = 0;

              
              
              

                if (ResultDs != null)
                {

                    if (ResultDs.Tables.Count > 0)
                        lstrows = new TBWDataList[ResultDs.Tables.Count + 1];

                    for (int i = 0; i < ResultDs.Tables.Count; i++)
                    {
                        Boolean tmpallow = true;
                      
                            if (ResultDs.Tables[i].TableName.Contains("Category"))
                                tmpallow = true;
                            else if (ResultDs.Tables[i].TableName.Contains("Keyword"))
                                tmpallow = true;
                           // else if (ResultDs.Tables[i].TableName.Contains("Model"))
                           //     tmpallow = true;
                            else if (ResultDs.Tables[i].TableName.Contains("Price"))
                                tmpallow = true;                          
                            else
                                tmpallow = false;
                        
                        if ((tmpallow))
                        {
                            if (ResultDs.Tables[i].Rows.Count > 0)
                            {

                                lstrecords1 = new TBWDataList1[ResultDs.Tables[i].Rows.Count + 1];
                                int ictrecords = 0;

                                int j = 0;
                                foreach (DataRow dr in ResultDs.Tables[i].Rows)//For Records
                                {
                                    if (ictrecords <= 6)
                                    {
                                        _stmpl_records = _stg_recordsCell.GetInstanceOf("ToPSearch" + "\\" + "cell1");

                                        if (ResultDs.Tables[i].TableName.Contains("Category"))
                                        {
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr["Category_Name"].ToString());
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));
                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr[0].ToString());
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr[0].ToString()));
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));


                                        }


                                        _stmpl_records.SetAttribute("TBW_CUSTOM_NUM_FIELD3", 2);

                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_TYPE", ResultDs.Tables[i].TableName.ToString());
                                        if (HttpContext.Current.Session["EASearch"] != null)
                                        {
                                            _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EASearch"].ToString())));
                                        }

                                        lstrecords1[ictrecords] = new TBWDataList1(_stmpl_records.ToString());
                                    }
                                    ictrecords++;

                                }

                                j++;

                                _stmpl_recordsrows = _stg_recordsRow.GetInstanceOf("ToPSearch" + "\\" + "row1");
                                
                                _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", ResultDs.Tables[i].TableName.ToString());
                                _stmpl_recordsrows.SetAttribute("TBT_SEARCH_TEXT", Strvalue);
                                _stmpl_recordsrows.SetAttribute("TBWDataList1", lstrecords1);

                                if (ictrecords > 6)
                                    _stmpl_recordsrows.SetAttribute("TBT_DISPLAY_LINK", true);
                                else
                                    _stmpl_recordsrows.SetAttribute("TBT_DISPLAY_LINK", false);

                                lstrows[ictrows] = new TBWDataList(_stmpl_recordsrows.ToString());
                                ictrows++;
                            }
                        }
                    }
                    _stmpl_container = _stg_container.GetInstanceOf("ToPSearch" + "\\" + "main1");
                    
                    _stmpl_container.SetAttribute("TBWDataList", lstrows); 
                    sHTML = _stmpl_container.ToString();
                }

                
                
              
                
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }

        }

        return sHTML;
    }
    private static string GetParentCatID(string catID)
    {
        try
        {
            HelperDB objHelperDB = new HelperDB();
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
       [System.Web.Services.WebMethod()]
    public static string stringreplace(string strvalue)
    {
        if ((strvalue.Contains("`~")))
        {
            strvalue = strvalue.Replace("`~", "\"");
        }
    HelperServices objHelperServices=new HelperServices();
    //if ((strvalue == "fl") || (strvalue == "pl") || (strvalue == "ps") || (strvalue == "ct") || (strvalue == "pd") || (strvalue == "bb"))
    //{
    //    strvalue = strvalue + "~";
    //}
  string  strvaluenew = objHelperServices.SimpleURL_Str ( strvalue, "ps.aspx",false);

           //if(strvaluenew.Contains("-"))
           //  {
                 HttpContext.Current.Session["CurrSearch"] = strvalue; 
           //}
           return strvaluenew;
    
    }
       [System.Web.Services.WebMethod()]
       public static string stringreplaceMS(string strvalue, string suppName)
       {
           if ((strvalue.Contains("`~")))
           {
               strvalue = strvalue.Replace("`~", "\"");
           }
           HttpContext.Current.Session["SUPPLIER_NAME"] = suppName;   
           HelperServices objHelperServices = new HelperServices();
           //if ((strvalue == "fl") || (strvalue == "pl") || (strvalue == "ps") || (strvalue == "ct") || (strvalue == "pd") || (strvalue == "bb"))
           //{
           //    strvalue = strvalue + "~";
           //}
           if (suppName == "")
           {
               suppName =  HttpContext.Current.Session["mssuppliername"].ToString() ;
           }
           string strvaluenew = objHelperServices.SimpleURL_MS_Str("AllProducts////WESAUSTRALASIA////" + suppName + "////" + strvalue, "mps.aspx",false);

           if (strvaluenew.Contains("-"))
           {
               HttpContext.Current.Session["CurrSearch_MS"] = strvalue;
           }
           return strvaluenew;

       }
   
    public static Hashtable GetAutoCompleteResult(string Strvalue)
    {
        string resultStr = string.Empty;
        Hashtable Autosuggestion = new Hashtable();
        try
        {
            System.Net.WebClient myWebClient = new System.Net.WebClient();
            resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete.jsp?fctn=&key=" + Strvalue + "&dct=" + EasyAsk_WebCatDictionary + "&num=5");
        }
        catch { }
        if (resultStr != "")
        {

            resultStr = resultStr.Replace("\"", "");
            BuildResult(resultStr, Autosuggestion);
        }
        return Autosuggestion;
    }
    public static void BuildResult(string resultStr, Hashtable Autosuggestion)
    {
        int s = 0;
        int e = 0;
        int s1 = 0;
        int e1 = 0;

        string tmpstr = string.Empty;
        if (resultStr.Contains("[{"))
            s = resultStr.IndexOf("[{");
        else
            s = resultStr.IndexOf(",{");

        if (resultStr.Contains("},"))
            e = resultStr.IndexOf("},");
        else
            e = resultStr.IndexOf("}]");
        if (s >= 0 && e >= 0)
        {
            count = count + 1;
            s = s + 2;


            tmpstr = resultStr.Substring(s, e - s);
            if (tmpstr.Contains("val:"))
                s1 = tmpstr.IndexOf("val:") + 4;
            if (tmpstr.Contains(",start"))
                e1 = tmpstr.IndexOf(",start");

            Autosuggestion.Add(count, tmpstr.Substring(s1, e1 - s1));



            if (resultStr.Substring(e + 1).Length > 0)
                BuildResult(resultStr.Substring(e + 1), Autosuggestion);
        }



    }
    [System.Web.Services.WebMethod()]
    public static string cartcount(string Strvalue)
    {
        try
        {


            if ((Strvalue.ToLower().Contains("/mct") == true) || (Strvalue.ToLower().Contains("/mpl") == true)
                   || (Strvalue.ToLower().Contains("/mps") == true)
                   || (Strvalue.ToLower().Contains("/mfl") == true)
                   || (Strvalue.ToLower().Contains("/mpd") == true)
          || (Strvalue.ToLower().Contains("/mcontactus") == true)
         || (Strvalue.ToLower().Contains("/maboutus") == true)
                  || (Strvalue.ToLower().Contains("/mmyaccount") == true)
                      || (Strvalue.ToLower().Contains("/mchangepassword") == true)
                  || (Strvalue.ToLower().Contains("/mlogin") == true)
                   )
            {
                return cartcountMS(Strvalue);
            }
            else
            {
                try
                {

                    HelperServices objHelperServices = new HelperServices();
                    ErrorHandler oErr = new ErrorHandler();
                    OrderServices objOrderServices = new OrderServices();
                    ConnectionDB objConnectionDB = new ConnectionDB();

                    TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CartItems", HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                    string formname = HttpContext.Current.Request.Url.ToString();
                    return "WA"+tbwtEngine.ST_Top_Cart_item();

                }
                catch
                {
                    return "0,0";
                }

            }

            //HelperServices objHelperServices = new HelperServices();
            //ErrorHandler oErr = new ErrorHandler();
            //OrderServices objOrderServices = new OrderServices();
            //string cartitem = "0";
            //string cartOrderid = "0";
            //int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
            //if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].ToString().Equals("") && !HttpContext.Current.Session["USER_ID"].ToString().Equals(System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))            
            //{

            //    int OrderID = 0;

            //    if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
            //    {
            //        OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
            //    }
            //    else
            //    {
            //        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID);
            //    }

            //    string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
            //    if (OrderID > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())    // || OrderStatus=="CAU_PENDING")
            //    {
            //        if (objOrderServices.GetOrderItemCount(OrderID) == 0)
            //            cartitem = "0";
            //        else
            //            cartitem = objOrderServices.GetOrderItemCount(OrderID).ToString();

            //        cartOrderid = OrderID.ToString();
            //    }
            //    else
            //    {
            //        cartOrderid = "0";
            //        cartitem = "0";
            //    }
            //}
            //else
            //{
            //    int OrderID = 0;
            //    String sessionId;
            //    sessionId =HttpContext.Current.Session.SessionID;

            //    if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
            //    {
            //        OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
            //    }
            //    else
            //    {
            //        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"]), OpenOrdStatusID);
            //    }

               
            //    string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
            //    if (OrderID > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())        //  || OrderStatus == "CAU_PENDING")
            //    {
            //        if (objOrderServices.GetOrderItemCount_BL(OrderID, sessionId) == 0)
            //            cartitem = "0";
            //        else
            //            cartitem = objOrderServices.GetOrderItemCount_BL(OrderID, sessionId).ToString();

            //        cartOrderid = OrderID.ToString();
            //    }
            //    else
            //    {
            //        cartOrderid = "0";
            //        cartitem = "0";
            //    }
            //}
            //return cartitem + "," + cartOrderid;
        }


        catch
        {
            return "0,0";
        }
    }


    public  static string cartcountMS(string Strvalue)
    {
        try
        {

            HelperServices objHelperServices = new HelperServices();
            ErrorHandler oErr = new ErrorHandler();
            OrderServices objOrderServices = new OrderServices();
            ConnectionDB objConnectionDB = new ConnectionDB();
            string MicroSiteTemplate = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());

            TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("CartItems", MicroSiteTemplate, objConnectionDB.ConnectionString);
            string formname =HttpContext.Current.Request.Url.ToString();
            return "MS" + tbwtMSEngine.ST_Top_Cart_item();

        }
        catch
        {
            return "MS";
        }
    
    }


    [System.Web.Services.WebMethod]
    public static string SetSortOrder(string orderVal, string url)
    {

        string rtn = "-1";
       
        if (orderVal.ToLower() == "latest")
        {
            if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "Latest")
            {
                HttpContext.Current.Session["SortOrder"] = "Latest";
                rtn = "1";
            }

        }
        else if (orderVal.ToLower() == "ltoh")
        {
            if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "ltoh")
            {
                HttpContext.Current.Session["SortOrder"] = "ltoh";
                rtn = "1";
            }
            
        }
        else if (orderVal.ToLower() == "htol")
        {
            if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "htol")
            {

                HttpContext.Current.Session["SortOrder"] = "htol";
                rtn = "1";
            }
        }

        else if (orderVal.ToLower() == "relevance")
        {
            if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "relevance")
            {

                HttpContext.Current.Session["SortOrder"] = "relevance";
                rtn = "1";
            }
        }

        else if (orderVal.ToLower() == "popularity")
        {
            if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "popularity")
            {
                HttpContext.Current.Session["SortOrder"] = "popularity";
                rtn = "1";
            }
        }
        else if (orderVal.ToLower() == "catalog")
        {
            if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "catalog")
            {
                HttpContext.Current.Session["SortOrder"] = "catalog";
                rtn = "1";
            }
        }


        if (url.Contains("/ps/") == true)
        {
            HttpContext.Current.Session["SortOrder_ps"] = "SortOrder_ps";
        }
        else
        {
            HttpContext.Current.Session["SortOrder_ps"] = "SortOrder_op";
        }

        return rtn;
    }


    [System.Web.Services.WebMethod]
    public static string Assignhfclickattr(string hfclickattr,string strvalue)
    {

        HttpContext.Current.Session["hfclickedattr_ps"] = hfclickattr;
        HelperServices objHelperServices = new HelperServices();
        string strvaluenew = objHelperServices.SimpleURL_Str(strvalue, "ps.aspx", false);

       
        return strvaluenew;
        
    }
    [System.Web.Services.WebMethod]
    public static string Assignhfclickattrms(string hfclickattr, string strvalue)
    {

        HttpContext.Current.Session["hfclickedattr_mps"] = hfclickattr;
        HelperServices objHelperServices = new HelperServices();
        string strvaluenew = objHelperServices.SimpleURL_MS_Str(strvalue, "mps.aspx", false);


        return strvaluenew;

    }


    public class Marker
    {
        public string endpoint { get; set; }
        // public string keys { get; set; }

    }

    [System.Web.Services.WebMethod]
    public static string endpoint(object Markers)
    {

        try
        {
            string subscription = JsonConvert.SerializeObject(Markers);
            string[] x = subscription.Split(new string[] { "," }, StringSplitOptions.None);
            string endpointjs = x[0] + "}";
            //   Marker x = JsonConvert.SerializeObject( Markers);
          Marker user = JsonConvert.DeserializeObject<Marker>(endpointjs);

            string endpoint = user.endpoint;
            ConnectionDB objConnection = new ConnectionDB();
            DataSet ds = new DataSet();
            HelperServices objHelperService = new HelperServices();
            string querystr = "insert into TB_NOTIFICATION_SUB_DETAILS (currentSubscription,[ENDPOINT],[MessageStatus],Website_id) ";
            querystr = querystr + "values ('" + subscription + "','" + endpoint + "',0,3)";

            SqlCommand pscmd = new SqlCommand(querystr, objConnection.GetConnection());
            pscmd.ExecuteNonQuery();
            //string strxml = HttpContext.Current.Server.MapPath("Notification") + "\\" + "Notification.txt";
            ////      System.IO.File.WriteAllText(strxml + "\\" + "Mainds.txt",  JsonConvert.SerializeObject(Markers));

            //if (Markers != null)
            //{


            //    StreamWriter writer2 = new StreamWriter(strxml, true);
            //    writer2.WriteLine(JsonConvert.SerializeObject(Markers));
            //    writer2.Flush();
            //    writer2.Close();
            //}

            //ConnectionDB objConnection = new ConnectionDB();      


            //string str = "insert into TB_NOTIFICATION_SUB_DETAILS values('"+ Markers +"','endpoint')";

            //SqlCommand objSqlCommand = new SqlCommand(str,   objConnection.GetConnection());
            //int r = objSqlCommand.ExecuteNonQuery();
            //if (r > 0)
            //{

            //}
            return "";
        }
        catch (Exception ex)
        {

            return "";

        }
    }


    [System.Web.Services.WebMethod]
    public static string setsession(string urlpath, string strpath)
    {
        try
        {
            //string[] x = strpath.Split(new string[] { "value=" }, StringSplitOptions.None);
            //string[] y = x[1].Split(new string[] { "&" }, StringSplitOptions.None);
            //HelperServices objHelperServices = new HelperServices();
            //string z = objHelperServices.URLstringreplace_Simple(y[0]);
            string[] x = urlpath.Split('/');
               string svalue = "hfclickedattr_pl" +x[0];
               //ErrorHandler objerr = new ErrorHandler();
               //objerr.CreateLog(svalue + "webmethod");
            HttpContext.Current.Session[svalue] = strpath;
            return strpath;
        }
        catch (Exception ex)
        {
        
         return "1";
        }
    }
    [System.Web.Services.WebMethod]
    public static string setsession_product(string strpath)
    {

      
        ErrorHandler objErrorHandler = new ErrorHandler();
        objErrorHandler.CreateLog("setsession_product"+strpath);
        HttpContext.Current.Session["hfclickedattr_pd"] = strpath;
        return "1";
    }
    [System.Web.Services.WebMethod]
    public static string setsession_family(string strpath)
    {
        HttpContext.Current.Session["hfclickedattr_fl"] = strpath;
        return "1";
    }
    [System.Web.Services.WebMethod]
    public static string setsession_brand_dd(string urlpath, string strpath)
    {
        try
        {
            //string[] x = strpath.Split(new string[] { "value=" }, StringSplitOptions.None);
            //string[] y = x[1].Split(new string[] { "&" }, StringSplitOptions.None);
            HelperServices objHelperServices = new HelperServices();
            //string z = objHelperServices.URLstringreplace_Simple(y[0]);
         //   string[] x = urlpath.Split('/');
            string z = objHelperServices.URLstringreplace_Simple(urlpath);
            string svalue = "hfclickedattr_bb" + z;
            HttpContext.Current.Session[svalue] = strpath;
            return z;
        }
        catch (Exception ex)
        {
            return "1";
        }
    }
    [System.Web.Services.WebMethod]
    public static string setsession_brand(string urlpath, string strpath)
    {
        try
        {
            //string[] x = strpath.Split(new string[] { "value=" }, StringSplitOptions.None);
            //string[] y = x[1].Split(new string[] { "&" }, StringSplitOptions.None);
            HelperServices objHelperServices = new HelperServices();
            //string z = objHelperServices.URLstringreplace_Simple(y[0]);
            string[] x = urlpath.Split('/');
            string z = objHelperServices.URLstringreplace_Simple(x[1]);
            string svalue = "hfclickedattr_bb" +z;
            HttpContext.Current.Session[svalue] = strpath;
            return z;
        }
        catch (Exception ex)
        {
            return "1";
        }
    }
    [System.Web.Services.WebMethod]
    public static string setsession_ps(string urlpath, string strpath)
    {
        try
        {
            //string[] x = strpath.Split(new string[] { "value=" }, StringSplitOptions.None);
            //string[] y = x[1].Split(new string[] { "&" }, StringSplitOptions.None);
        

            string[] x = urlpath.Split('/');
            HelperServices objHelperServices = new HelperServices();
            string z = objHelperServices.URLstringreplace_Simple(x[1]);
            string svalue = "hfclickedattr_ps" +z;
            HttpContext.Current.Session[svalue] = strpath;
            HttpContext.Current.Session["hfclickedattr_ps_view"] = strpath;

            return z;
        }
        catch (Exception ex)
        {
            return "1";
        }
    }
    [System.Web.Services.WebMethod]
    public static string setsession_breadcrumb(string strpath)
    {
        if (strpath.ToLower().Contains("pl.aspx"))
        {
            HttpContext.Current.Session["hfclickedattr_pl"] = strpath;
        }
        else if (strpath.ToLower().Contains("bb.aspx"))
        {
            HttpContext.Current.Session["hfclickedattr_bb"] = strpath;
        }

        else if (strpath.ToLower().Contains("pd.aspx"))
        {
            HttpContext.Current.Session["hfclickedattr_pd"] = strpath;
        }
        else if (strpath.ToLower().Contains("fl.aspx"))
        {
          //  HttpContext.Current.Session["hfclickedattr_fl"] = strpath;
        }
        else if (strpath.ToLower().Contains("ps.aspx"))
        {
            HttpContext.Current.Session["hfclickedattr_ps"] = strpath;
        }
        return "1";
    }


    [System.Web.Services.WebMethod]
    public static string SearchCtr_str(string strpath)
    {
        try
        {

          
                HelperServices objHelperServices = new HelperServices();
                string z = objHelperServices.URLstringreplace_Simple(strpath);
                // objErrorHandler.CreateLog(strvalue);
                //if (strpath.Contains("-"))
                //{
                    HttpContext.Current.Session["CurrSearch"] = strpath;
                //}

                return z;
        }
       
        catch (Exception ex)
        {
            return strpath;
        }
    }


    [System.Web.Services.WebMethod]
    public static string setsession_ps_bb(string strpath)
    {
        try
        {


            HelperServices objHelperServices = new HelperServices();
            string z = objHelperServices.URLstringreplace_Simple(strpath);
            // objErrorHandler.CreateLog(strvalue);
            //if (strpath.Contains("-"))
            //{
            HttpContext.Current.Session["CurrSearch"] = strpath;
            //}

            return z;
        }

        catch (Exception ex)
        {
            return strpath;
        }
    }
}
