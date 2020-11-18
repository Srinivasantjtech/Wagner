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
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_CatalogueDownload : System.Web.UI.UserControl
{
    string stemplatepath = "";
    ErrorHandler objErrorHandler=new ErrorHandler();
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    CategoryServices objCategoryServices = new CategoryServices();
    Security objSecurity = new Security();
    string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
    string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    string _Action = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        stemplatepath = Server.MapPath("Templates");
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();

        if (Request.QueryString["ActionResult"] != null)
            _Action = Request.QueryString["ActionResult"].ToString();


        //DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, WesNewsCategoryId, "GET_CATEGORY", HelperDB.ReturnType.RTDataSet);
        //string custom_num_f = null;
        //if (tmpds != null)
        //{
        //    custom_num_f = tmpds.Tables[0].Rows[0]["CUSTOM_NUM_FIELD3"].ToString();
        //}
    }
    public string ST_PDFDownload()
    {
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        //StringTemplateGroup _stg_pages = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
       // StringTemplate _stmpl_pages = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
       // string[] filenames = null;

        string shtml = "";
        int counter = 0;
        int cntoddeven = 0;
        DataRow[] dr=null;

        //if (Directory.Exists(Server.MapPath("attachments")))
        //{

        //    //string[] fileEntries = Directory.GetFiles(Server.MapPath("attachments"), "*.pdf");
        //    //lstrecords = new TBWDataList[fileEntries.Length];
        //    //filenames = new string[fileEntries.Length];
        //    //if (fileEntries.Length > 0)

        //    //DataSet dsPDFCount = new DataSet();
        //    //dsPDFCount = objCategoryServices.GetCatalogPDFCount(2);

        //    //if (dsPDFCount != null)
        //    //{
        //    //    foreach (DataRow rPDF in dsPDFCount.Tables[0].Rows)
        //    //    {
        //    //        lstrecords = new TBWDataList[Convert.ToInt32(rPDF["CountFiles"].ToString())];
        //    //    }
        //    //}

        //    //if (lstrecords.Length > 0)
        //    //{

        //        DataSet dsCatalog = new DataSet();
        //        try
        //        {
        //            dsCatalog = objCategoryServices.GetCatalogPDFDownload(2);
        //            if (dsCatalog != null)
        //            {
        //                if (_Action == "CATALOGUE")
        //                {
        //                    dr = dsCatalog.Tables[0].Select("PARENT_CATEGORY<>'" + WesNewsCategoryId + "' And CUSTOM_NUM_FIELD1=1 And CUSTOM_NUM_FIELD2=0");
        //                }
        //                else if (_Action == "NEWS")
        //                {
        //                    dr = dsCatalog.Tables[0].Select("PARENT_CATEGORY='" + WesNewsCategoryId + "' And CUSTOM_NUM_FIELD1=1 And CUSTOM_NUM_FIELD2=0");
        //                }

        //                else if (_Action == "FORMS")
        //                {
        //                    dr = dsCatalog.Tables[0].Select("CUSTOM_NUM_FIELD2=1");
        //                }

        //                if (dr.Length > 0)
        //                {
        //                    lstrecords = new TBWDataList[dr.Length + 1];
        //                    dsCatalog=new DataSet();
        //                    dsCatalog.Tables.Add(dr.CopyToDataTable().Copy());

        //                    foreach (DataRow rCat in dsCatalog.Tables[0].Rows)
        //                    {
        //                        string MyFile = Server.MapPath(string.Format("attachments{0}", rCat["IMAGE_FILE2"].ToString()));

                                
                               
        //                        if (System.IO.File.Exists(MyFile) || rCat["IMAGE_NAME"].ToString() != "")
        //                        {

        //                            _stg_records = new StringTemplateGroup("cataloguedownload", stemplatepath);
        //                            _stmpl_records = _stg_records.GetInstanceOf("cataloguedownload" + "\\" + "cell");

                                   


        //                            cntoddeven++;
        //                            if ((cntoddeven % 2) == 0)
        //                            {
        //                                _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
        //                                _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "row1");
        //                            }
        //                            else
        //                            {
        //                                _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
        //                                _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "row");
        //                            }

        //                            if (System.IO.File.Exists(MyFile))
        //                            {
        //                                _stmpl_records.SetAttribute("TBT_PDF_DESCRIPTION", rCat["IMAGE_NAME2"].ToString());

        //                                string[] file = rCat["IMAGE_FILE2"].ToString().Split(new string[] { @"/" }, StringSplitOptions.None);

        //                                _stmpl_records.SetAttribute("PDF_FILE_NAME", file[file.Length-1].ToString());

        //                                _stmpl_records.SetAttribute("PDF", rCat["IMAGE_FILE2"].ToString());


        //                                FileInfo finfo = new FileInfo(Server.MapPath(string.Format("attachments{0}", rCat["IMAGE_FILE2"].ToString())));
        //                                long FileInBytes = finfo.Length;
        //                                long FileInKB = finfo.Length / 1024;


        //                                DateTime time = finfo.LastWriteTime;

        //                                _stmpl_records.SetAttribute("PDF_SIZE", FileInKB + " KB");
        //                                _stmpl_records.SetAttribute("PDF_DATE", time.Date.ToShortDateString());

        //                            }
        //                            else
        //                            {
        //                                _stmpl_records.SetAttribute("TBT_PDF_DESCRIPTION", "");
        //                                _stmpl_records.SetAttribute("PDF_FILE_NAME", "");
        //                            }
        //                            //_stmpl_records.SetAttribute("PDF_EBOOK",);
        //                            //_stmpl_records.SetAttribute("PDF_BROWSE_ONLINE", time.Date.ToString());

        //                            if (_Action == "NEWS" || _Action == "CATALOGUE")
        //                            {
        //                                if (rCat["IMAGE_NAME"].ToString() != "")
        //                                {
        //                                    _stmpl_records.SetAttribute("PDF_EBOOK", rCat["IMAGE_NAME"].ToString());
        //                                    _stmpl_records.SetAttribute("EBOOK_DISPLAY", true);
        //                                }
        //                                else
        //                                {
        //                                    _stmpl_records.SetAttribute("PDF_EBOOK", "");
        //                                    _stmpl_records.SetAttribute("EBOOK_DISPLAY", false);
        //                                }
        //                                string Ea_Path="";
        //                                if (rCat["CATEGORY_PATH"].ToString().Contains("////") == true)
        //                                {
        //                                    if (rCat["CATEGORY_PATH"].ToString().EndsWith("////" + rCat["CATEGORY_NAME"].ToString()) == true)
        //                                    {
        //                                        string[] str = rCat["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
        //                                        Ea_Path = "AllProducts////WESAUSTRALASIA";
        //                                        for (int i = 0; i < str.Length - 1; i++)
        //                                        {
        //                                            Ea_Path = Ea_Path + "////" + str[i].ToString();
        //                                        }
        //                                        _stmpl_records.SetAttribute("TBT_URL_PATH", "pl.aspx");
        //                                    }
        //                                    else
        //                                    {

        //                                        Ea_Path = "AllProducts////WESAUSTRALASIA////" + rCat["CATEGORY_PATH"].ToString();
        //                                        _stmpl_records.SetAttribute("TBT_URL_PATH", "pl.aspx");
        //                                    }

        //                                }
        //                                else
        //                                {
        //                                    _stmpl_records.SetAttribute("TBT_URL_PATH", "ct.aspx");                                            
        //                                    Ea_Path = "AllProducts////WESAUSTRALASIA";
        //                                }
                                        
        //                                _stmpl_records.SetAttribute("TBT_PARENT_CATEGORY_ID",HttpUtility.UrlEncode( rCat["PARENT_CATEGORY"].ToString()));
        //                                _stmpl_records.SetAttribute("TBT_CATEGORY_ID", HttpUtility.UrlEncode(rCat["CATEGORY_ID"].ToString()));
        //                                _stmpl_records.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(rCat["CATEGORY_NAME"].ToString()));
        //                                _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path)));


        //                            }
        //                            if (_Action == "CATALOGUE")
        //                            {
        //                                _stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", true);
        //                                _stmpl_records.SetAttribute("TBT_PDF_NEWS", false);
        //                                _stmpl_records.SetAttribute("TBT_PDF_FORMS", false);
        //                            }
        //                            else if (_Action == "NEWS")
        //                            {
        //                                _stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", false);
        //                                _stmpl_records.SetAttribute("TBT_PDF_NEWS", true);
        //                                _stmpl_records.SetAttribute("TBT_PDF_FORMS", false);
        //                            }

        //                            else if (_Action == "FORMS")
        //                            {
        //                                _stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", false);
        //                                _stmpl_records.SetAttribute("TBT_PDF_NEWS", false);
        //                                _stmpl_records.SetAttribute("TBT_PDF_FORMS", true);
        //                            }
        //                            _stmpl_container.SetAttribute("TBWDataList", _stmpl_records.ToString());
        //                            lstrecords[counter] = new TBWDataList(_stmpl_container.ToString());
        //                            counter++;
        //                        }
        //                    }
        //                }
        //            }
        //        }
                
        //        catch (Exception e)
        //        {
        //            objErrorHandler.ErrorMsg = e;
        //            objErrorHandler.CreateLog(); 
        //        }

        //        _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
        //        _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "main");
        //         if(_Action=="CATALOGUE")
        //        {
        //            _stmpl_container.SetAttribute("TBT_PDF_CATALOGUE" ,true);
        //            _stmpl_container.SetAttribute("TBT_PDF_NEWS" ,false);
        //            _stmpl_container.SetAttribute("TBT_PDF_FORMS" ,false);                                 
        //        }
        //        else if(_Action=="NEWS")
        //        {
        //            _stmpl_container.SetAttribute("TBT_PDF_CATALOGUE" ,false);
        //            _stmpl_container.SetAttribute("TBT_PDF_NEWS" ,true);
        //            _stmpl_container.SetAttribute("TBT_PDF_FORMS" ,false);                    
        //        }

        //        else if(_Action=="FORMS")
        //        {
        //            _stmpl_container.SetAttribute("TBT_PDF_CATALOGUE" ,false);
        //            _stmpl_container.SetAttribute("TBT_PDF_NEWS" ,false);
        //            _stmpl_container.SetAttribute("TBT_PDF_FORMS" ,true);
        //        }
        //        _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                
        //        shtml = _stmpl_container.ToString();
        // }
        ////    else
        ////        return "<table height=\"100\"><tr><td valign=\"bottom\">No PDF Catalogue found</td></tr></table>";
        ////}
        ////else
        ////    return "<table height=\"100\"><tr><td valign=\"bottom\">No PDF catalogue found</td></tr></table>";
        //return objHelperServices.StripWhitespace(shtml) ;

        if (Directory.Exists(Server.MapPath("attachments")))
        {

             DataSet dsCatalog = new DataSet();
            try
            {
                dsCatalog = objCategoryServices.GetCatalogPDFDownload(2);
                if (dsCatalog != null)
                {
                    if (_Action == "CATALOGUE")
                    {
                        dr = dsCatalog.Tables[0].Select("PARENT_CATEGORY<>'" + WesNewsCategoryId + "' And CUSTOM_NUM_FIELD1=1 And CUSTOM_NUM_FIELD2=0");
                    }
                    else if (_Action == "NEWS")
                    {
                        dr = dsCatalog.Tables[0].Select("PARENT_CATEGORY='" + WesNewsCategoryId + "' And CUSTOM_NUM_FIELD1=1 And CUSTOM_NUM_FIELD2=0");
                    }

                    else if (_Action == "FORMS")
                    {
                        dr = dsCatalog.Tables[0].Select("CUSTOM_NUM_FIELD2=1");
                    }
                    //Added if to fix error in logfile 3 feb 2016
                    if (dr != null)
                    {
                        if (dr.Length > 0)
                        {
                            lstrecords = new TBWDataList[dr.Length + 1];
                            dsCatalog = new DataSet();
                            dsCatalog.Tables.Add(dr.CopyToDataTable().Copy());

                            foreach (DataRow rCat in dsCatalog.Tables[0].Rows)
                            {
                                string MyFile = Server.MapPath(string.Format("attachments{0}", rCat["IMAGE_FILE2"].ToString()));



                                if (System.IO.File.Exists(MyFile) || rCat["IMAGE_NAME"].ToString() != "")
                                {

                                    _stg_records = new StringTemplateGroup("cataloguedownload", stemplatepath);
                                    _stmpl_records = _stg_records.GetInstanceOf("cataloguedownload" + "\\" + "cell");




                                    cntoddeven++;
                                    if ((cntoddeven % 2) == 0)
                                    {
                                        _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
                                        _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "row1");
                                    }
                                    else
                                    {
                                        _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
                                        _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "row");
                                    }

                                    if (System.IO.File.Exists(MyFile))
                                    {
                                        _stmpl_records.SetAttribute("TBT_PDF_DESCRIPTION", rCat["IMAGE_NAME2"].ToString());

                                        string[] file = rCat["IMAGE_FILE2"].ToString().Split(new string[] { @"/" }, StringSplitOptions.None);

                                        _stmpl_records.SetAttribute("PDF_FILE_NAME", file[file.Length - 1].ToString());
                                        //Modified By :indu :Added replace function to remove  catelogdowload
                                        _stmpl_records.SetAttribute("PDF", rCat["IMAGE_FILE2"].ToString().Replace("\\", "/"));


                                        FileInfo finfo = new FileInfo(Server.MapPath(string.Format("attachments{0}", rCat["IMAGE_FILE2"].ToString())));
                                        long FileInBytes = finfo.Length;
                                        long FileInKB = finfo.Length / 1024;


                                        DateTime time = finfo.LastWriteTime;

                                        _stmpl_records.SetAttribute("PDF_SIZE", FileInKB + " KB");
                                        _stmpl_records.SetAttribute("PDF_DATE", time.Date.ToShortDateString());

                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_PDF_DESCRIPTION", "");
                                        _stmpl_records.SetAttribute("PDF_FILE_NAME", "");
                                    }

                                    if (_Action == "NEWS" || _Action == "CATALOGUE")
                                    {
                                        if (rCat["IMAGE_NAME"].ToString() != "")
                                        {
                                            string ebookpath = objHelperServices.viewebook(rCat["IMAGE_NAME"].ToString());

                                            _stmpl_records.SetAttribute("PDF_EBOOK", ebookpath);
                                            _stmpl_records.SetAttribute("EBOOK_DISPLAY", true);
                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("PDF_EBOOK", "");
                                            _stmpl_records.SetAttribute("EBOOK_DISPLAY", false);
                                        }
                                        string Ea_Path = "";
                                        if (rCat["CATEGORY_PATH"].ToString().Contains("////") == true)
                                        {
                                            if (rCat["CATEGORY_PATH"].ToString().EndsWith("////" + rCat["CATEGORY_NAME"].ToString()) == true)
                                            {
                                                string[] str = rCat["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                                Ea_Path = "AllProducts////WESAUSTRALASIA";
                                                for (int i = 0; i < str.Length - 1; i++)
                                                {
                                                    Ea_Path = Ea_Path + "////" + str[i].ToString();
                                                }
                                                _stmpl_records.SetAttribute("TBT_URL_PATH", "pl.aspx");
                                                _stmpl_records.SetAttribute("TBT_ISCAT", false);
                                            }
                                            else
                                            {

                                                Ea_Path = "AllProducts////WESAUSTRALASIA////" + rCat["CATEGORY_PATH"].ToString();
                                                _stmpl_records.SetAttribute("TBT_URL_PATH", "pl.aspx");
                                                _stmpl_records.SetAttribute("TBT_ISCAT", false);
                                            }

                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("TBT_URL_PATH", "ct.aspx");
                                            Ea_Path = "AllProducts////WESAUSTRALASIA";
                                            _stmpl_records.SetAttribute("TBT_ISCAT", true);
                                        }

                                        _stmpl_records.SetAttribute("TBT_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(rCat["PARENT_CATEGORY"].ToString()));
                                        _stmpl_records.SetAttribute("TBT_CATEGORY_ID", HttpUtility.UrlEncode(rCat["CATEGORY_ID"].ToString()));
                                        _stmpl_records.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(rCat["CATEGORY_NAME"].ToString()));
                                        _stmpl_records.SetAttribute("TBT_CUSTOM_NUM_FIELD3", HttpUtility.UrlEncode(rCat["CUSTOM_NUM_FIELD3"].ToString()));
                                        _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path)));


                                    }
                                    if (_Action == "CATALOGUE")
                                    {
                                        _stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", true);
                                        _stmpl_records.SetAttribute("TBT_PDF_NEWS", false);
                                        _stmpl_records.SetAttribute("TBT_PDF_FORMS", false);
                                    }
                                    else if (_Action == "NEWS")
                                    {
                                        _stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", false);
                                        _stmpl_records.SetAttribute("TBT_PDF_NEWS", true);
                                        _stmpl_records.SetAttribute("TBT_PDF_FORMS", false);
                                    }

                                    else if (_Action == "FORMS")
                                    {
                                        _stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", false);
                                        _stmpl_records.SetAttribute("TBT_PDF_NEWS", false);
                                        _stmpl_records.SetAttribute("TBT_PDF_FORMS", true);
                                    }
                                    _stmpl_container.SetAttribute("TBWDataList", _stmpl_records.ToString());
                                    lstrecords[counter] = new TBWDataList(_stmpl_container.ToString());
                                    counter++;
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

            _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
            _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "main");
            if (_Action == "CATALOGUE")
            {
                _stmpl_container.SetAttribute("TBT_PDF_CATALOGUE", true);
                _stmpl_container.SetAttribute("TBT_PDF_NEWS", false);
                _stmpl_container.SetAttribute("TBT_PDF_FORMS", false);
            }
            else if (_Action == "NEWS")
            {
                _stmpl_container.SetAttribute("TBT_PDF_CATALOGUE", false);
                _stmpl_container.SetAttribute("TBT_PDF_NEWS", true);
                _stmpl_container.SetAttribute("TBT_PDF_FORMS", false);
            }

            else if (_Action == "FORMS")
            {
                _stmpl_container.SetAttribute("TBT_PDF_CATALOGUE", false);
                _stmpl_container.SetAttribute("TBT_PDF_NEWS", false);
                _stmpl_container.SetAttribute("TBT_PDF_FORMS", true);
            }
            _stmpl_container.SetAttribute("TBWDataList", lstrecords);

            shtml = _stmpl_container.ToString();
        }
        return objHelperServices.StripWhitespace(shtml);

    }
}
