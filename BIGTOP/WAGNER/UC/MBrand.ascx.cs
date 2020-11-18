using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using Antlr3.ST;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace WES.UC
{
      
    public partial class MBrand : System.Web.UI.UserControl
    {
        EasyAsk_WAGNER ObjEasyAsk = new EasyAsk_WAGNER();
        ErrorHandler objErrorHandler = new ErrorHandler();
        string catID = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string Bread_Crumbs()
        {
            string breadcrumb = "", paraPID = "", paraFID = "", paraCID = "", byp = "";
            //if (Request.QueryString["pid"] != null)
            //{
            //    paraPID = Request.QueryString["pid"].ToString();
            //}
            //if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "List all products")
            //    paraFID = Request.QueryString["fid"].ToString();
            //if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "List all models")
            //    paraCID = Request.QueryString["cid"].ToString();
            //if (Request.QueryString["byp"] != null && Request.QueryString["byp"].ToString() != "")
            //    byp = Request.QueryString["byp"].ToString();

            //if (Request.QueryString["cid"] != null)
            //    catID = Request.QueryString["cid"].ToString();
            try
            {
                breadcrumb = ObjEasyAsk.GetBreadCrumb_Simple_MS(Server.MapPath("Templates"), true);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            return breadcrumb;

        }


        protected string ST_BrandAndModel()
        {

            string sHTML = string.Empty;
            try
            {


                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_container_slide = null;
                StringTemplate _stmpl_container_slide_active = null;
                StringTemplate _stmpl_container_main = null;
                StringTemplate _stmpl_records = null;
                StringTemplateGroup _stg_container_slide = null;
                StringTemplateGroup _stg_records_slide = null;


                StringTemplate _stmpl_records_slide = null;
                //  StringTemplate _stmpl_records1 = null;

                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];
                TBWDataList[] lstrows_slide_active = new TBWDataList[0];
                TBWDataList[] lstrows_slide = new TBWDataList[0];
                //  StringTemplateGroup _stg_container1 = null;
                // StringTemplateGroup _stg_records1 = null;
                TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                TBWDataList1[] lstrows1 = new TBWDataList1[0];
                string _SkinRootPath = Server.MapPath("Templates");

                int ictrows = 0;
                string _bypcat = null;
                _bypcat = HttpContext.Current.Request.QueryString["bypcat"];
                DataSet dscat = new DataSet();


                HelperDB objHelperDB = new HelperDB();
                dscat = (DataSet)objHelperDB.GetGenericDataDB("Brand", "GET_OURBRAND", HelperDB.ReturnType.RTDataSet);



                HelperServices objHelperServices = new HelperServices();




                if (dscat == null)
                    return "";

                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                if (dscat.Tables.Count > 0)
                    lstrows = new TBWDataList[dscat.Tables[0].Rows.Count];

                for (int i = 0; i < dscat.Tables[0].Rows.Count; i++)
                {

                    if (Request.QueryString["Type"] == null)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf("mbrand" + "\\" + "cell");
                    }
                    else
                    {
                        _stmpl_records = _stg_records.GetInstanceOf("mbrand" + "\\" + "cell1");
                    }

                    string url = dscat.Tables[0].Rows[i]["brand"].ToString();
                    url = objHelperServices.SimpleURL_Str(url, "PL", false);
                    _stmpl_records.SetAttribute("TBT_REWRITEURL", url);
                    _stmpl_records.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
               
                    _stmpl_records.SetAttribute("TBT_TOSUITE_Brand", dscat.Tables[0].Rows[i]["brand"].ToString());
                    _stmpl_records.SetAttribute("TBT_IMAGE_FILE", dscat.Tables[0].Rows[i]["LOGO_IMAGE"].ToString());
                    lstrows[ictrows] = new TBWDataList(_stmpl_records.ToString());
                    ictrows++;
                }
                if (Request.QueryString["Type"] == null)
                {
                    _stmpl_container = _stg_container.GetInstanceOf("mbrand" + "\\" + "main");
                }
                else
                {
                    _stmpl_container = _stg_container.GetInstanceOf("mbrand" + "\\" + "main1");
                }
                //_stmpl_container.SetAttribute("Selection", updateNavigation());
                _stmpl_container.SetAttribute("TBWDataList", lstrows);
                _stg_records_slide = new StringTemplateGroup("rowslideactive", _SkinRootPath);
                _stg_container_slide = new StringTemplateGroup("mainslide", _SkinRootPath);
                ictrows = 0;
                DataTable DtTOPBRANDS = new DataTable();
                DataRow[] row = dscat.Tables[0].Select("IMAGE_PATH_SLIDE <>''");
                if (row.Length > 0)
                {
                    DtTOPBRANDS = row.CopyToDataTable();
                    lstrows_slide_active = new TBWDataList[6];
                    lstrows_slide = new TBWDataList[6];
                }
                for (int i = 0; i < 6; i++)
                {


                    _stmpl_records_slide = _stg_records_slide.GetInstanceOf("mbrand" + "\\" + "cellslide");

                    string url = DtTOPBRANDS.Rows[i]["brand"].ToString();
                    url = objHelperServices.SimpleURL_Str(url, "PL", false);
                    _stmpl_records_slide.SetAttribute("TBT_REWRITEURL", url);
                    _stmpl_records_slide.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                    _stmpl_records_slide.SetAttribute("TBT_TOSUITE_Brand", DtTOPBRANDS.Rows[i]["brand"].ToString());
                    _stmpl_records_slide.SetAttribute("TBT_IMAGE_FILE", DtTOPBRANDS.Rows[i]["IMAGE_PATH_SLIDE"].ToString());

                    lstrows_slide_active[ictrows] = new TBWDataList(_stmpl_records_slide.ToString());


                    ictrows++;
                }
                _stmpl_container_slide_active = _stg_container.GetInstanceOf("mbrand" + "\\" + "rowslideactive");

                _stmpl_container_slide_active.SetAttribute("TBWDataList_active", lstrows_slide_active);
                _stg_records_slide = new StringTemplateGroup("rowslide", _SkinRootPath);
                _stmpl_container_slide = _stg_container.GetInstanceOf("mbrand" + "\\" + "rowslide");
                string rowitem = "";
                ictrows = 0;
                for (int i = 6; i < DtTOPBRANDS.Rows.Count; i++)
                {


                    _stmpl_records_slide = _stg_records_slide.GetInstanceOf("mbrand" + "\\" + "cellslide");

                    string url = DtTOPBRANDS.Rows[i]["brand"].ToString();
                    url = objHelperServices.SimpleURL_Str(url, "PL", false);
                    _stmpl_records_slide.SetAttribute("TBT_REWRITEURL", url);
                    _stmpl_records_slide.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                    _stmpl_records_slide.SetAttribute("TBT_TOSUITE_Brand", DtTOPBRANDS.Rows[i]["brand"].ToString());
                    _stmpl_records_slide.SetAttribute("TBT_IMAGE_FILE", DtTOPBRANDS.Rows[i]["IMAGE_PATH_SLIDE"].ToString());

                    lstrows_slide[ictrows] = new TBWDataList(_stmpl_records_slide.ToString());
                    ictrows++;
                    if ((i != 6) && ((i + 1) % 6 == 0))
                    {
                        _stmpl_container_slide.SetAttribute("TBWDataList", lstrows_slide);
                        rowitem = rowitem + _stmpl_container_slide.ToString();
                        lstrows_slide = new TBWDataList[6];
                        ictrows = 0;
                        _stmpl_container_slide = null;
                        _stmpl_container_slide = _stg_container.GetInstanceOf("mbrand" + "\\" + "rowslide");
                    }

                }





                _stmpl_container_main = _stg_container.GetInstanceOf("mbrand" + "\\" + "mainslide");
                _stmpl_container_main.SetAttribute("TBWDataList_active", _stmpl_container_slide_active.ToString());
                _stmpl_container_main.SetAttribute("TBWDataList", rowitem);
                sHTML += _stmpl_container_main.ToString() + _stmpl_container.ToString();

            }
            catch (Exception ex)
            {
                sHTML = ex.Message;
                objErrorHandler.CreateLog();
            }
            finally
            {

            }


            return sHTML;
        }
    }
}