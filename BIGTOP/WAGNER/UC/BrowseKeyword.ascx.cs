using System;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Web;
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

namespace WES.UC
{
    
    public partial class BrowseKeyword : System.Web.UI.UserControl
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperService = new HelperServices();
        Security objSecurity = new Security();
        public string WagnerUrl = System.Configuration.ConfigurationManager.AppSettings["WagnerUrl"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
           //string key = System.Configuration.ConfigurationManager.AppSettings["SecurityKeypassword"].ToString();

            try
            {

                //tag01.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("0-9");

                //tagA.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLA");
                //tagB.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLB");
                //tagC.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLC");
                //tagD.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLD");
                //tagE.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLE");
                //tagF.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLF");
                //tagG.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLG");
                //tagH.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLH");
                //tagI.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLI");
                //tagJ.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLJ");
                //tagK.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLK");
                //tagL.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLL");
                //tagM.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLM");
                //tagN.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLN");
                //tagO.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLO");
                //tagP.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLP");
                //tagQ.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLQ");
                //tagR.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLR");
                //tagS.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLS");
                //tagT.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLT");
                //tagU.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLU");
                //tagV.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLV");
                //tagW.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLW");
                //tagX.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLX");
                //tagY.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLY");
                //tagZ.HRef = "/BrowseKeyword.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLZ");
                //if (Request.QueryString["trk"] != null)
                //{
                string[] strfilter = Request.Url.PathAndQuery.Split('?');
                string filter = strfilter[1];
                filter = filter.ToLower();
                    if (filter == "0-9")
                    {
                        tag01.Visible = false;
                        btag01.Visible = true;

                    }
                    else if (filter == "a")
                    {


                        tagA.Visible = false;
                        btagA.Visible = true;
                    }
                    else if (filter == "b")
                    {


                        tagB.Visible = false;
                        btagB.Visible = true;
                    }
                    else if (filter == "c")
                    {


                        tagC.Visible = false;
                        btagC.Visible = true;
                    }
                    else if (filter == "d")
                    {


                        tagD.Visible = false;
                        btagd.Visible = true;
                    }
                    else if (filter == "e")
                    {


                        tagE.Visible = false;
                        btagE.Visible = true;
                    }
                    else if (filter == "f")
                    {


                        tagF.Visible = false;
                        btagF.Visible = true;
                    }
                    else if (filter == "g")
                    {


                        tagG.Visible = false;
                        btagG.Visible = true;
                    }
                    else if (filter == "h")
                    {


                        tagH.Visible = false;
                        btagH.Visible = true;
                    }
                    else if (filter == "i")
                    {


                        tagI.Visible = false;
                        btagI.Visible = true;
                    }
                    else if (filter == "j")
                    {


                        tagJ.Visible = false;
                        btagJ.Visible = true;
                    }
                    else if (filter == "k")
                    {


                        tagK.Visible = false;
                        btagK.Visible = true;
                    }
                    else if (filter == "l")
                    {


                        tagL.Visible = false;
                        btagL.Visible = true;
                    }
                    else if (filter == "m")
                    {


                        tagM.Visible = false;
                        btagM.Visible = true;
                    }
                    else if (filter == "n")
                    {


                        tagN.Visible = false;
                        btagN.Visible = true;
                    }
                    else if (filter == "o")
                    {


                        tagO.Visible = false;
                        btagO.Visible = true;
                    }
                    else if (filter == "p")
                    {


                        tagP.Visible = false;
                        btagP.Visible = true;
                    }
                    else if (filter == "q")
                    {


                        tagQ.Visible = false;
                        btagQ.Visible = true;
                    }
                    else if (filter == "r")
                    {


                        tagR.Visible = false;
                        btagR.Visible = true;
                    }
                    else if (filter == "s")
                    {


                        tagS.Visible = false;
                        btagS.Visible = true;
                    }
                    else if (filter == "t")
                    {


                        tagT.Visible = false;
                        btagT.Visible = true;
                    }
                    else if (filter == "u")
                    {


                        tagU.Visible = false;
                        btagU.Visible = true;
                    }
                    else if (filter == "v")
                    {


                        tagV.Visible = false;
                        btagV.Visible = true;
                    }
                    else if (filter == "w")
                    {


                        tagW.Visible = false;
                        btagW.Visible = true;
                    }
                    else if (filter == "x")
                    {


                        tagX.Visible = false;
                        btagx.Visible = true;
                    }

                    else if (filter == "y")
                    {


                        tagY.Visible = false;
                        btagy.Visible = true;
                    }
                    else if (filter == "z")
                    {


                        tagZ.Visible = false;
                        btagz.Visible = true;
                    }
                //}
                //else
                //{
                //    tag01.Visible = false;
                //    btag01.Visible = true;

                //}
            }
            catch
            {
                Response.Redirect("/BrowseKeyword.aspx",false);

            }
            //HFcnt.Value = key;
          
            //    HiddenField1.Value = key;
            //    hftable.Value = key;
              
            //    hfcheckload.Value = key;
               
              
            //    //.Replace("pl/","pl.aspx?");

            //    hfback.Value = "";
            //    hfbackdata.Value = "";

            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "lastPostFunc();", true);
        }
        public string ST_Newproduct()
        {
            try
            {
                ConnectionDB objConnectionDB = new ConnectionDB();
                HelperServices objHelperServices = new HelperServices();

                TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCTLOGNAV", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                tbwtEngine.RenderHTML("Column");
                return tbwtEngine.ST_NewProductLogNav_Load(null, HttpContext.Current);

                //if (Session["NewProductLogNav"] != null)
                //{
                //    //return tbwtEngine.ST_NewProductLogNav_Load((DataSet)Session["NewProductLogNav"]);
                //    return Session["NewProductLogNav"].ToString();
                //}
                //else
                //    return "";

                //return "";
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtSearch.Text.Trim() != "")
                {
                    //Response.Redirect("ps.aspx?&srctext=" + txtsearchhidden.Value.ToString().Replace("#", "%23").Replace("&", "%26"), true);
                    string strsearch = txtSearch.Text;
                    //if (strsearch.Contains("_/_") == false && strsearch.Contains("_/") == false || strsearch.Contains("/_") == false)
                    //{
                    //    strsearch = strsearch.Replace("/", "`/`");


                    //}
                    //if ((strsearch == "fl") || (strsearch == "pl") || (strsearch == "ps") || (strsearch == "ct") || (strsearch == "pd") || (strsearch == "bb"))
                    //{
                    //    strsearch = strsearch + "~";
                    //}
                    strsearch = objHelperService.SimpleURL_Str(strsearch, "ps.aspx", false);
                    Response.Redirect(WagnerUrl +"/" + strsearch + "/ps/", false);
                }
                else
                {

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
        public string St_BrowseKeyword()
        {
            try
            {
        
               
                        
                string maincontent=string. Empty;
                HelperServices objHelperService = new HelperServices();
                HelperDB objhelperdb = new HelperDB();
                DataSet psds = new DataSet(); 
                //if (Request.QueryString["trk"] == null)
                //{
                //    //string filename = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["KEYWORDTXTFILE"].ToString());
                //    //using (StreamReader streamReader = new StreamReader(filename, System.Text.Encoding.UTF8))
                //    //{
                //    //    maincontent = streamReader.ReadToEnd();
                //    //}
                //    //return maincontent;
                //    psds = objhelperdb.GetPowerSearch_Keyword("0-9");                 

                //}
                //else
                //{              
                    
                //    string filter = Request.QueryString["trk"].ToString();
                //    filter = objSecurity.StringDeCrypt(filter).Replace("FL",""). ToLower();
                //     psds = objhelperdb.GetPowerSearch_Keyword(filter);
                //}
                string[] strfilter = Request.Url.PathAndQuery.Split('?');
                string filter = strfilter[1];
                filter = filter.ToLower();
                psds = objhelperdb.GetPowerSearch_Keyword(filter);
                    int j = 1;
                    DataRow[] allRows = psds.Tables[0].Select();
                string templathpath=Server.MapPath("Templates");
  
                    StringTemplateGroup group = new StringTemplateGroup("BrowseBy",templathpath);
              
              
                    TBWDataList[] lstrows = new TBWDataList[allRows.Length]; 
                int ictrows=0;
                    for (int i = 1; i <= allRows.Length - 1; i++)
                    {    
                        StringTemplate cellkeyword = group.GetInstanceOf("BrowseBy" + "\\" + "cell_Keyword");
                        string strvalue = allRows[i][0].ToString();
                      
                        string plpage = objHelperService.SimpleURL_Str( strvalue, "ps.aspx",true) + "/ps/";



                      

                        cellkeyword.SetAttribute("TBT_REWRITEURL", WagnerUrl + plpage);
                        cellkeyword.SetAttribute("strKeyword",  strvalue.ToUpper());
                        lstrows[ictrows] = new TBWDataList(cellkeyword.ToString());
                        ictrows++;
                        //if (j == 1)
                        //{
                            //maincontent = maincontent + "<a href='" + plpage + "'>" + strvalue.ToUpper() + "</a>";
                        //    j = j + 1;
                        //}
                        //else if (j == 2)
                        //{

                        //    maincontent = maincontent + "<div class='home-product'><ul><li><a href='" + plpage + "'>" + strvalue.ToUpper() + "</a></li></ul></div>";
                        //    j = j + 1;
                        //}
                        //else if (j == 3)
                        //{
                        //    maincontent = maincontent + "<div class='home-product'><ul><li><a href='" + plpage + "'>" + strvalue.ToUpper() + "</a></li></ul></div>";
                        //    j = 1;
                        //}

                    }
                   
                    StringTemplateGroup _stg_container = new StringTemplateGroup("BrowseBy", templathpath);
                    StringTemplate _stmpl_container = null;

                    _stmpl_container = _stg_container.GetInstanceOf("BrowseBy" + "\\" + "main");
                    _stmpl_container.SetAttribute("TBWDataList", lstrows);
                    return _stmpl_container.ToString();

         //       int i = 1;
         //       foreach (DataRow drpsearch in psds.Tables[0].Rows)
         //       {

         //   string strvalue = drpsearch["PSKeyword"].ToString();
         //   string href = objHelperService.Cons_NewURl_POWERSEARCH("ps.aspx?srctext=" + HttpUtility.UrlEncode(strvalue) + "", strvalue, "ps.aspx", "") + "/ps/"; ;
                  

         //          if (i % 2 != 0)
         //          {
         //              maincontent = maincontent + "<tr><td width='300px'><div class='submenu'><ul><li><a href='" + href + "'>" + strvalue.ToUpper() + "</a></li></ul></div></td>";

         //          }
         //          else

         //          {
         //              maincontent = maincontent + "<td width='300px'><div class='submenu'><ul><li><a href='" + href + "'>" + strvalue.ToUpper() + "</a></li></ul></div></td></tr>";
         //          }
         //           //if (i == 200)
         //           //{
         //           //   // HFcnt.Value = (i + 1).ToString();
         //           //    break; 
         //           //}
         //i++;
         //       }
                        //string st_end="</table></head></html>";
               

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }

        //public string Dynamic_St_BrowseKeyword(int i)
        //{
        //    try
        //    {
             
        //        //string st_head = "<html><head><table width=600px style='border=1px;align='left''>"
        //        //    + "<div class='title3' style='text-align:left'>"
        //        //    + "<h3 class='tx_6'>Browse by Keyword Index</h3></div>";

        //        HelperDB objhelperdb = new HelperDB();
        //        DataSet psds = objhelperdb.GetPowerSearch_Keyword();
        //        string maincontent = string.Empty;
        //        HelperServices objHelperService = new HelperServices();
        //        int j;
        //        for ( j = i+1; j <= i + 200;j++ )
        //        {

        //            string strvalue = psds.Tables[0].Rows[j]["PSKeyword"].ToString();
        //            string plpage = objHelperService.Cons_NewURl_bybrand("ps.aspx?srctext=" + HttpUtility.UrlEncode(strvalue) + "", strvalue, "ps.aspx", "");
        //            string href = plpage + "/ps/";

        //            if (IsOdd(j))
        //            {
        //                maincontent = maincontent + "<tr><td width='300px'><div class='submenu'><ul><li><a href='" + href + "'>" + strvalue + "</a></li></ul></div></td>";

        //            }
        //            else
        //            {
        //                maincontent = maincontent + "<td width='300px'><div class='submenu'><ul><li><a href='" + href + "'>" + strvalue + "</a></li></ul></div></td></tr>";
        //            }
                   
        //        }
        //        string hfcnt = j.ToString();
        //       // HFcnt.Value = hfcnt;
                    
             
              
        //        //string st_end = "</table></head></html>";
        //        return  maincontent ;

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        return string.Empty;
        //    }
        //}
        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }
}