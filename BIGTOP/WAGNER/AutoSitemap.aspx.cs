using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TradingBell.WebCat.EasyAsk;
using System.Xml.XPath;
public partial class AutoSitemap : System.Web.UI.Page
{
    //XmlWriter writer;
    //string txtUrl = "";
    //// string DistinctColumn = "URL";
    //ArrayList UniqueRecords = new ArrayList();
    //ArrayList DuplicateRecords = new ArrayList();
    //DataTable Dtable = new DataTable();
    //string sturl = "";
    //string dval = "";
    //EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    //DataSet dsrecords = new DataSet();
    //DataSet dsrecords1 = new DataSet();
    //HelperServices objHelperServices = new HelperServices();
    protected void Page_Load(object sender, EventArgs e)
    {
      ////  string sitename = string.Empty;
      ////  string _bname = "";
      ////  string CatName = "";
      ////  string subCatName = "";
      ////  string familyname = "";
      ////  string PRODDESC = "";
      ////  int iRecordsPerPage = 24;
      ////  int iPageNo = 1;
      ////  int familyid = 0;
      ////  int productid = 0;
      ////  string href = "";
      ////  try
      ////  {
      ////      Dtable.Columns.Add("URL");
      ////      sitename = Request.Url.GetLeftPart(UriPartial.Authority);      
      ////      sitename = "http://staging.wagneronline.com.au/";
      ////      writer = XmlWriter.Create(Server.MapPath("SiteMap.xml"));
      ////      writer.WriteStartDocument();
      ////      writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
      ////      WebClient WClient = new WebClient();



      ////      if (HttpContext.Current.Session["MainCategory"] == null)
      ////      {
      ////          EasyAsk.GetCategoryAndBrand("MainCategory");
      ////          dsrecords = (DataSet)HttpContext.Current.Session["MainCategory"];
      ////      }
      ////      else
      ////      {
      ////          dsrecords = (DataSet)HttpContext.Current.Session["MainCategory"];
      ////      }
           
      ////      foreach (DataRow dr1 in dsrecords.Tables[0].Rows)
      ////      {
      ////          CatName = dr1["CATEGORY_NAME"].ToString().ToUpper();
      ////          // iRecordsPerPage = 32767;
      ////          iRecordsPerPage = 1000;
      ////          // EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
      ////          // DataSet dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
      ////          DataSet dscat = EasyAsk.GetAttributeProductstoxml("CategoryProductList", "", "Category", CatName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
      ////          CatName = CatName.Replace(" ", "_");
      ////          CatName = CatName.Replace(" / ", "~..~");
      ////          CatName = CatName.Replace(" /", "~..");
      ////          CatName = CatName.Replace("/ ", "..~");
      ////          CatName = CatName.Replace("/", ".~.");
      ////          CatName = CatName.Replace("+", "||");
      ////          CatName = CatName.Replace("&", "^^");
      ////          CatName = CatName.Replace(":", "~`");
      ////          if (dscat != null && dscat.Tables["FamilyProxml"].Rows.Count > 0)
      ////          {
      ////              foreach (DataRow dr in dscat.Tables[2].Rows)
      ////              {
      ////                  //subCatName = dscat.Tables["FamilyPro"].Rows[j]["SUBCATNAME_L1"].ToString().ToUpper();
      ////                  subCatName = dr["SUBCATNAME_L1"].ToString().ToUpper();
      ////                  subCatName = subCatName.Replace(" ", "_");
      ////                  subCatName = subCatName.Replace(" / ", "~..~");
      ////                  subCatName = subCatName.Replace(" /", "~..");
      ////                  subCatName = subCatName.Replace("/ ", "..~");
      ////                  subCatName = subCatName.Replace("/", ".~.");
      ////                  subCatName = subCatName.Replace("+", "||");
      ////                  subCatName = subCatName.Replace("&", "^^");
      ////                  subCatName = subCatName.Replace(":", "~`");
      ////                  int productcount = objHelperServices.CI(dr["PRODUCT_COUNT"].ToString());
      ////                  familyid = objHelperServices.CI(dr["FAMILY_ID"].ToString());
      ////                  productid = objHelperServices.CI(dr["PRODUCT_ID"].ToString());
      ////                  familyname = dr["FAMILY_NAME"].ToString().ToUpper();
      ////                  familyname = familyname.Replace(" ", "_");
      ////                  familyname = familyname.Replace(" / ", "~..~");
      ////                  familyname = familyname.Replace(" /", "~..");
      ////                  familyname = familyname.Replace("/ ", "..~");
      ////                  familyname = familyname.Replace("/", ".~.");
      ////                  familyname = familyname.Replace("+", "||");
      ////                  familyname = familyname.Replace("&", "^^");
      ////                  familyname = familyname.Replace(":", "~`");
      ////                  if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
      ////                      PRODDESC = dr["STRING_VALUE"].ToString();

      ////                  if (productcount > 1)
      ////                  {
      ////                      //href = href + "," + "href=" + "\"" + sitename + "fl.aspx?" + CatName + "/" + subCatName + "/" + familyid + "=" + familyname + "\"";
      ////                      if (subCatName == "")
      ////                          href = "fl.aspx?" + CatName + "/" + familyid + "=" + familyname;
      ////                      else
      ////                          href = "fl.aspx?" + CatName + "/" + subCatName + "/" + familyid + "=" + familyname;

      ////                      Dtable.Rows.Add(href);
      ////                  }
      ////                  else
      ////                  {
      ////                      // href = href + "," + "href="+"\""+ sitename + "pd.aspx?" + CatName + "/" + subCatName + "/" + familyid + "=" + familyname + "/" + productid + "=" + PRODDESC +"\"";
      ////                      if (subCatName == "")
      ////                          href = "pd.aspx?" + CatName + "/" + familyid + "=" + familyname + "/" + productid + "=" + PRODDESC;
      ////                      else
      ////                          href = "pd.aspx?" + CatName + "/" + subCatName + "/" + familyid + "=" + familyname + "/" + productid + "=" + PRODDESC;
      ////                      Dtable.Rows.Add(href);
      ////                  }
      ////              }
                   
      ////              dscat = null;
      ////              HttpContext.Current.Session["FamilyProductxml"] = null;
      ////          }


      ////      }




       

      ////  //    string _tsm = "";
      ////  //    string _tsb = "";
      ////  //    string _parentCatID = "WES0830";
      ////  //    EasyAsk.GetBrandListxml(_parentCatID, "");
      ////  //    DataTable dsbrandlist = new DataTable();
      ////  //    DataSet dsmodelist = new DataSet();
      ////  //    if (HttpContext.Current.Session["brand_xml"] != null)
      ////  //        dsbrandlist = (DataTable)HttpContext.Current.Session["brand_xml"];
           
        
      ////  //    foreach (DataRow drbrlst in dsbrandlist.Rows)
      ////  //    {
      ////  //        _tsb = drbrlst["TOSUITE_BRAND"].ToString().ToUpper();
      ////  //        dsmodelist = EasyAsk.getwesmodeltoxml("CELLULAR ACCESSORIES", 2, _tsb);

      ////  //        if (HttpContext.Current.Session["WESBrand_Model_xml"] != null)
      ////  //            dsmodelist = (DataSet)HttpContext.Current.Session["WESBrand_Model_xml"];
              


      ////  //    }
      ////  //    EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
      ////  //  //  EasyAsk.GetWESModel("CELLULAR ACCESSORIES", 2, _tsb);



      ////  //   // WClient.Credentials = new NetworkCredential("staginguser","staging@1234", sitename);
      ////  //  //  WClient.Credentials = new NetworkCredential("staginguser", "staging@1234");
      ////  //  //  string URL = WClient.DownloadString(sitename);

      ////  //    string URL = "";

      ////  //    foreach (LinkItem i in LinkFinder.Find(URL))
      ////  //    {
      ////  //        Debug.WriteLine(i);
      ////  //        txtUrl = i.ToString();
      ////  //        txtUrl = txtUrl.Replace("amp;", "");
      ////  //        Dtable.Rows.Add(txtUrl);
      ////  //    }
      ////  //    //foreach (LinkItem i in LinkFinder.Find(href))
      ////  //    //{
      ////  //    //    Debug.WriteLine(i);
      ////  //    //    txtUrl = i.ToString();
      ////  //    //    txtUrl = txtUrl.Replace("amp;", "");
      ////  //    //    Dtable.Rows.Add(txtUrl);
      ////  //    //}
      ////  //    foreach (DataRow Drow in Dtable.Rows)
      ////  //    {
      ////  //        dval = Drow[0].ToString().ToLower();
      ////  //        if (UniqueRecords.Contains(dval))
      ////  //        {
      ////  //            DuplicateRecords.Add(Drow);
      ////  //        }
      ////  //        else
      ////  //        {
      ////  //            UniqueRecords.Add(dval);
      ////  //            sturl = Drow[0].ToString();
      ////  //            WriteTag("0.6", "Always", sitename + "/" + sturl, writer);
      ////  //        }
      ////  //    }

      ////  //    writer.WriteEndDocument();
      ////  //    writer.Close();
      ////  //    // UpdateProgressxml.Visible = false;
      ////      //Response.Redirect("SiteMap.xml");
      //// // }
      //////  catch (Exception ex)
      //////  {
      ////     // lblerror.Visible = true;
      ////     // lblerror.Text = ex.ToString() + sitename;

      //// // }

      ////  //Response.Clear();
      ////  //Response.Buffer = true;
      ////  //Response.Charset = "";
      ////  //Response.Cache.SetCacheability(HttpCacheability.NoCache);
      ////  //Response.ContentType = "application/xml";
      ////  //Response.WriteFile(Server.MapPath("~/SiteMap.xml"));
      ////  //Response.Flush();
      ////  //Response.End();
        Response.Redirect("/SiteMap.xml",false);

    }
    public struct LinkItem
    {
        public string Href;
        public string Text;
        public override string ToString()
        {
            return Href + "\n\t" + Text;
        }
    }
    static class LinkFinder
    {
        public static List<LinkItem> Find(string file)
        {
            List<LinkItem> list = new List<LinkItem>();
            MatchCollection m1 = Regex.Matches(file, @"(<a.*?>.*?</a>)",
            RegexOptions.Singleline);

            foreach (Match m in m1)
            {
                string value = m.Groups[1].Value;
                LinkItem i = new LinkItem();

                Match m2 = Regex.Match(value, @"href=\""(.*?)\""",
                RegexOptions.Singleline);
                if (m2.Success)
                {
                    i.Href = m2.Groups[1].Value;
                }
                list.Add(i);
            }
            return list;
        }
    }
    private void WriteTag(string Priority, string freq, string Navigation, XmlWriter UrlWriter)
    {
        UrlWriter.WriteStartElement("url");

        UrlWriter.WriteStartElement("loc");
        UrlWriter.WriteValue(Navigation);
        UrlWriter.WriteEndElement();

        UrlWriter.WriteStartElement("lastmod");
        UrlWriter.WriteValue(DateTime.Now.ToString("yyyy-MM-dd"));
        UrlWriter.WriteEndElement();

        UrlWriter.WriteStartElement("changefreq");
        UrlWriter.WriteValue(freq);
        UrlWriter.WriteEndElement();

        UrlWriter.WriteStartElement("priority");
        UrlWriter.WriteValue(Priority);
        UrlWriter.WriteEndElement();

        UrlWriter.WriteEndElement();
    }
}
