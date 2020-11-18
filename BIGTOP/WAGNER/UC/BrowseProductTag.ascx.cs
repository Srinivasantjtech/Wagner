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
    
    public partial class BrowseProductTag : System.Web.UI.UserControl
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        Security objSecurity = new Security();
        HelperServices objHelperService = new HelperServices();
        public string WagnerUrl = System.Configuration.ConfigurationManager.AppSettings["WagnerUrl"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //tag01.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("0-9");

                //tagA.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLA");
                //tagB.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLB");
                //tagC.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLC");
                //tagD.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLD");
                //tagE.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLE");
                //tagF.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLF");
                //tagG.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLG");
                //tagH.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLH");
                //tagI.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLI");
                //tagJ.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLJ");
                //tagK.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLK");
                //tagL.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLL");
                //tagM.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLM");
                //tagN.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLN");
                //tagO.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLO");
                //tagP.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLP");
                //tagQ.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLQ");
                //tagR.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLR");
                //tagS.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLS");
                //tagT.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLT");
                //tagU.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLU");
                //tagV.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLV");
                //tagW.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLW");
                //tagX.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLX");
                //tagY.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLY");
                //tagZ.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLZ");

                //tag01.HRef = "/0-9/bt/");

                //tagA.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLA");
                //tagB.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLB");
                //tagC.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLC");
                //tagD.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLD");
                //tagE.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLE");
                //tagF.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLF");
                //tagG.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLG");
                //tagH.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLH");
                //tagI.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLI");
                //tagJ.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLJ");
                //tagK.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLK");
                //tagL.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLL");
                //tagM.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLM");
                //tagN.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLN");
                //tagO.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLO");
                //tagP.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLP");
                //tagQ.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLQ");
                //tagR.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLR");
                //tagS.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLS");
                //tagT.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLT");
                //tagU.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLU");
                //tagV.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLV");
                //tagW.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLW");
                //tagX.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLX");
                //tagY.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLY");
                //tagZ.HRef = "/BrowseProductTag.aspx" + "?&trk=" + objSecurity.StringEnCrypt("FLZ");


                //if (Request.QueryString["trk"] != null)
                //{
                    string[] strfilter =Request.Url.PathAndQuery.Split('?')    ;
                  string  filter = strfilter[1];
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
                Response.Redirect("/BrowseProductTag.aspx",false);
            
            }
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
        public string St_BrowseKeyword()
        {
            try
            {
        
                 
                         HelperDB objhelperdb = new HelperDB();
                 DataSet psds=new DataSet();
              
                
                 //if (Request.QueryString["trk"] == null)
                 //{
                 //    psds = objhelperdb.GetProductTag("0-9");
                    
                 //}
                 //else
                 //{
                    
                 //    filter = Request.QueryString["trk"].ToString();
                 //    filter = objSecurity.StringDeCrypt(filter).Replace("FL","").ToLower();
                 //    psds = objhelperdb.GetProductTag(filter);
                 //}
                 string[] strfilter = Request.Url.PathAndQuery.Split('?');
                 string filter = strfilter[1];
                 filter = filter.ToLower();
                 
                 psds = objhelperdb.GetProductTag(filter);
                string maincontent=string. Empty;
                HelperServices objHelperService = new HelperServices();
                //int i = 1;
                //foreach (DataRow drpsearch in psds.Tables[0].Rows)
                //{
                int j = 1;
                DataRow[] allRows = psds.Tables[0].Select();


                string templathpath = Server.MapPath("Templates");

                StringTemplateGroup group = new StringTemplateGroup("BrowseBy", templathpath);


                TBWDataList[] lstrows = new TBWDataList[allRows.Length];
                int ictrows = 0;

                for (int i = 1; i <= allRows.Length - 1; i++)
                {
                    string strvalue = allRows[i][0].ToString();
                    string plpage = objHelperService.SimpleURL_Str(strvalue, "ps.aspx", true) + "/ps/";
                    StringTemplate cellproduct = group.GetInstanceOf("BrowseBy" + "\\" + "cell_product");



                    cellproduct.SetAttribute("TBT_REWRITEURL", WagnerUrl + plpage);
                    cellproduct.SetAttribute("strKeyword", strvalue.ToUpper());
                    lstrows[ictrows] = new TBWDataList(cellproduct.ToString());
                    ictrows++;
                    //maincontent = maincontent + "<a href='" + plpage + "'>" + strvalue.ToUpper() + "</a>";
                }

                StringTemplateGroup _stg_container = new StringTemplateGroup("BrowseBy", templathpath);
                StringTemplate _stmpl_container = null;

                _stmpl_container = _stg_container.GetInstanceOf("BrowseBy" + "\\" + "main");
                _stmpl_container.SetAttribute("TBWDataList", lstrows); 
                     // if(j==1)
                     //{
                        // maincontent = maincontent + "<div class='home-grid'><ul><li><a href='" + plpage + "'>" + strvalue.ToUpper() + "</a></li></ul></div>";
                   //    j = j + 1;
                   //  }
                   //   else  if (j  == 2)
                   //  {

                   //      maincontent = maincontent + "<td width='250px'><div class='keymenu'><ul><li><a href='" + plpage + "'>" + strvalue.ToUpper() + "</a></li></ul></div></td>";
                   //      j = j + 1;
                   //  }
                   //  else if (j  == 3)
                   //  {
                   //      maincontent = maincontent + "<td width='250px'><div class='keymenu'><ul><li><a href='" + plpage + "'>" + strvalue.ToUpper() + "</a></li></ul></div></td></tr>";
                   //      j = 1;
                   //  }

                   //}
                return _stmpl_container.ToString();
                   //else

                   //{
                   //    maincontent = maincontent + "<td width='300px'><div class='submenu'><ul><li><a href='" + plpage + "'>" + strvalue.ToUpper() + "</a></li></ul></div></td></tr>";
                   //}
                    
         //i++;
          
                        //string st_end="</table></head></html>";
                      

            }
            catch (Exception ex)
            {
           
                objErrorHandler.ErrorMsg = ex;
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
                    strsearch = objHelperService.SimpleURL_Str( strsearch, "ps.aspx",false);
                    Response.Redirect(WagnerUrl + "/" + strsearch + "/ps/",false);
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
        //public static bool IsOdd(int value)
        //{
        //    return value % 2 != 0;
        //}
    }
}