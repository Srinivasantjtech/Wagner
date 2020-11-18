using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using CustomCaptcha;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;

public partial class familyMS : System.Web.UI.UserControl
{

    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    FamilyServices objFamilyServices = new FamilyServices();
    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER(); 
    string category_id = "1";
    int iRecordsPerPage = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["iRecordsPerPage"].ToString());
    string templatename = "";
    string contentvalue = "";
    string CScontentvalue = "";
    string subfamtemplate = "";
    DataSet Ds = new DataSet();
    DataSet EADs = new DataSet();
    DataSet Dsall = new DataSet();
    DataTable Famtb = new DataTable();
    DataTable SFamtb = new DataTable();
    DataTable EASubFamtb = new DataTable();
    DataSet dsPriceTableAll = new DataSet();
    DataSet DDS = null;
    TBWTemplateEngineMS tbwtEngine;
    FamilyServices ObjFamilyPage = new FamilyServices();
    EasyAsk_WAGNER objEasyAsk = new EasyAsk_WAGNER();
    string breadcrumb = "";
    string _Familyids = "";
    string _Fid = "";
    string micro_stemplatepath = "";
    string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
    string strImgFiles1 = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
    public string DownloadST = "";
    public bool isDownload = false;
    bool isdownload_product = false;
    bool chkdwld = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        iRecordsPerPage = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["familypagecnt"].ToString());   
        micro_stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());


    }
    //protected void cVerify_preRender(object sender, EventArgs e)
    //{
    //    Session["AQ_FL_CAPTCH_VALUE"] = cVerify.Text;
    //    Session["AQ_FL_CAPTCH_IMAGE"] = cVerify.GuidPath;

    //}

    public string Bread_Crumbs()
    {

        breadcrumb = objEasyAsk.GetBreadCrumb_Simple(Server.MapPath("Templates"));
        return breadcrumb;
    }
    private void ConstructFamilyData(string familyid, DataRow[] SourceFtb, DataSet tempDs, int tableinx)
    {
        try
        {
            DataTable dt = new DataTable();
            //DataSet tempDs = new DataSet();
            //tempDs = GetFamilyPageProduct(familyid, "PRODUCT"); // Get Other Attribute from Db
            if (tempDs != null && tempDs.Tables.Count > 0 && tempDs.Tables[tableinx].Rows.Count > 0)
            {
                Ds.Tables.Add(familyid);

                Ds.Tables[familyid].Columns.Add("FAMILY_ID", typeof(string));
                Ds.Tables[familyid].Columns.Add("PRODUCT_ID", typeof(string));
                Ds.Tables[familyid].Columns.Add("TWeb Image1", typeof(string));
                Ds.Tables[familyid].Columns.Add("Code", typeof(string));
                // Ds.Tables[familyid].Columns.Add("PROD_CODE", typeof(string));
                for (int i = 0; i <= tempDs.Tables[tableinx].Columns.Count - 1; i++)
                {
                    if (tempDs.Tables[tableinx].Columns[i].ColumnName.ToUpper() != "FAMILY_ID" && tempDs.Tables[tableinx].Columns[i].ColumnName.ToUpper() != "PRODUCT_ID")
                    {
                        Ds.Tables[familyid].Columns.Add(tempDs.Tables[tableinx].Columns[i].ColumnName.ToUpper(), typeof(string));
                    }
                }

                DataRow[] tempDr = null;
                foreach (DataRow tdr in SourceFtb)
                {
                    DataRow Dsdr = Ds.Tables[familyid].NewRow();
                    Dsdr["FAMILY_ID"] = tdr["FAMILY_ID"];
                    Dsdr["PRODUCT_ID"] = tdr["PRODUCT_ID"];
                    Dsdr["TWeb Image1"] = tdr["PRODUCT_TH_IMAGE"];
                    Dsdr["CODE"] = tdr["PRODUCT_CODE"];
                    Dsdr["COST"] = tdr["PRODUCT_PRICE"];
                    // Dsdr["PROD_CODE"] = tdr["PROD_CODE"];
                    tempDr = null;

                    tempDr = tempDs.Tables[tableinx].Select("FAMILY_ID='" + familyid + "' And PRODUCT_ID='" + tdr["PRODUCT_ID"] + "'");
                    if (tempDr.Length > 0)
                    {
                        var Dr = tempDr[0].Table.Columns;

                        // DataTable temptb = tempDr.CopyToDataTable();

                        for (int i = 0; i <= Dr.Count - 1; i++)
                        {
                            if (Dr[i].ColumnName.ToUpper() != "FAMILY_ID" && Dr[i].ColumnName.ToUpper() != "PRODUCT_ID" && Dr[i].ColumnName.ToUpper() != "COST")
                            {
                                try
                                {
                                    Dsdr[Dr[i].ColumnName.ToUpper()] = tempDr[0][Dr[i].ColumnName.ToUpper()];
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    Ds.Tables[familyid].Rows.Add(Dsdr);
                }

                //tempDs.Tables[0].TableName = familyid.ToString();
                //tempDs.Tables[0].Columns.Add("TWeb Image1", typeof(string)).SetOrdinal(2);
                //tempDs.Tables[0].Columns.Add("Code", typeof(string)).SetOrdinal(3);

                //dt = tempDs.Tables[0].Clone();

                ////Ds.Tables.Add(tempDs.Tables[0].Copy());


                //DataRow[] tempDr = null;
                //foreach (DataRow tdr in SourceFtb)
                //{
                //    tempDr = tempDs.Tables[0].Select("FAMILY_ID='" + familyid + "' And PRODUCT_ID='" + tdr["PRODUCT_ID"] + "'");
                //    if (tempDr.Length > 0)
                //    {
                //        tempDr[0]["TWeb Image1"] = tdr["PRODUCT_TH_IMAGE"];
                //        tempDr[0]["Code"] = tdr["PRODUCT_CODE"];
                //        tempDr[0]["COST"] = tdr["PRODUCT_PRICE"];
                //        dt.ImportRow(tempDr[0]);
                //    }


                //}

                //dt.TableName = familyid.ToString();
                //Ds.Tables.Add(dt);


            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
           // objErrorHandler.CreateLog(ex.ToString() + "," + familyid + "," + tableinx);

        }
    }
    private void ConstructSubFamilyHeader(DataSet EADs, DataTable SFtb)
    {
        try
        {
            if (SFtb != null)
            {
                //DataTable tempDs = new DataTable();  
                string image_string = "";
                DataRow dRow;
                EADs.Tables.Add(EASubFamtb);
                EASubFamtb.Columns.Add("FAMILY_ID", typeof(string));
                EASubFamtb.Columns.Add("FAMILY_NAME", typeof(string));
                EASubFamtb.Columns.Add("STRING_VALUE", typeof(string));
                EASubFamtb.Columns.Add("NUMERIC_VALUE", typeof(string));
                EASubFamtb.Columns.Add("ATTRIBUTE_ID", typeof(string));
                EASubFamtb.Columns.Add("ATTRIBUTE_NAME", typeof(string));
                EASubFamtb.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
                EASubFamtb.Columns.Add("OBJECT_TYPE", typeof(string));
                EASubFamtb.Columns.Add("OBJECT_NAME", typeof(string));
                EASubFamtb.TableName = "SubFamily";

                foreach (DataRow tdr in SFtb.Rows)
                {
                    DataRow[] dr = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + tdr["FAMILY_ID"] + "'");
                    if (dr.Length > 0)
                    {
                        //tempDs=dr.CopyToDataTable();

                        dRow = EASubFamtb.NewRow();
                        dRow["FAMILY_ID"] = dr[0]["FAMILY_ID"];// tempDs.Rows[0]["FAMILY_ID"];
                        dRow["FAMILY_NAME"] = dr[0]["FAMILY_NAME"];

                        //------------------
                        dRow["ATTRIBUTE_ID"] = "13";
                        dRow["STRING_VALUE"] = dr[0]["FAMILY_SHORT_DESC"];//fl Description
                        dRow["NUMERIC_VALUE"] = "0";
                        dRow["OBJECT_TYPE"] = "NULL";
                        dRow["OBJECT_NAME"] = "NULL";
                        dRow["ATTRIBUTE_NAME"] = "SHORT_DESCRIPTION";
                        dRow["ATTRIBUTE_TYPE"] = "7";
                        EASubFamtb.Rows.Add(dRow.ItemArray);

                        //------------------
                        dRow["ATTRIBUTE_ID"] = "4";
                        dRow["STRING_VALUE"] = dr[0]["FAMILY_DESC"];//fl Description
                        dRow["NUMERIC_VALUE"] = "0";
                        dRow["OBJECT_TYPE"] = "NULL";
                        dRow["OBJECT_NAME"] = "NULL";
                        dRow["ATTRIBUTE_NAME"] = "DESCRIPTION";
                        dRow["ATTRIBUTE_TYPE"] = "7";
                        EASubFamtb.Rows.Add(dRow.ItemArray);



                        //------------------
                        image_string = dr[0]["FAMILY_TH_IMAGE"].ToString();
                        if (image_string.ToLower().Contains("noimage.gif") == false)
                            image_string = image_string.ToLower().Replace("_th", "_Images_200");

                        dRow["ATTRIBUTE_ID"] = "746";
                        dRow["STRING_VALUE"] = image_string;
                        dRow["NUMERIC_VALUE"] = "0";
                        dRow["OBJECT_TYPE"] = "jpg";
                        dRow["OBJECT_NAME"] = image_string;
                        dRow["ATTRIBUTE_NAME"] = "FWeb Image1";
                        dRow["ATTRIBUTE_TYPE"] = "9";
                        EASubFamtb.Rows.Add(dRow.ItemArray);



                        //------------------
                        dRow["ATTRIBUTE_ID"] = "747";
                        dRow["STRING_VALUE"] = dr[0]["FAMILY_TH_IMAGE"];
                        dRow["NUMERIC_VALUE"] = "0";
                        dRow["OBJECT_TYPE"] = "jpg";
                        dRow["OBJECT_NAME"] = dr[0]["FAMILY_TH_IMAGE"];
                        dRow["ATTRIBUTE_NAME"] = "TFWeb Image1";
                        dRow["ATTRIBUTE_TYPE"] = "9";
                        EASubFamtb.Rows.Add(dRow.ItemArray);

                    }

                }

                HttpContext.Current.Session["FamilyProduct"] = EADs;
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }
    private string ST_VPC()
    {
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
                        i = i + 1;
                        if (i == 2)
                        {
                            break;
                        }
                    }
                }
                string NEWURL = string.Empty;
                string HREFURL = string.Empty;
                //  string urlpage = string.Empty;
                //if (vpchref.Contains("ct.aspx") == true)
                //    urlpage = "ct.aspx";
                //else
                //    urlpage = "pl.aspx";
                //NEWURL = objHelperServices.Cons_NewURl_bybrand(vpchref, eapath, urlpage, "");
                string[] catpath = HttpContext.Current.Request.RawUrl.Split('/');
                 eapath = "AllProducts////WESAUSTRALASIA////" +  (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ");
                NEWURL = objHelperServices.SimpleURL_MS_Str(eapath, "mpl.aspx",true);

                //if (urlpage == "ct.aspx")
                //    vpchref = "/" + NEWURL + "/ct/";
                //else
                vpchref =  NEWURL + "/mpl/";

                // vpchref = vpchref + "&path=" + eapath;
            }
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return e.Message;
        }
        return vpchref;
    }
    public string ST_Family_Download(DataSet TmpDs, DataSet TempEADs, int tableinx)
    {
        try
        {
            string rtnstr = "";
            StringTemplateGroup _stg_container = null;
           // StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
           // StringTemplate _stmpl_records = null;
           // StringTemplate _stmpl_records1 = null;
           // StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

           // StringTemplateGroup _stg_container1 = null;
           // StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];


            DataTable dt = new DataTable();
            DataRow[] dr = null;

            int ictrecords = 0;

            if (Request.QueryString["fid"] != null)
                _Fid = Request.QueryString["fid"].ToString();

            //  _Familyids = _Fid;

            ////  DataSet TmpDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];

            //  dr = TmpDs.Tables["FamilyPro"].Select("FAMILY_ID<>'" + _Fid + "'");
            //  if (dr.Length > 0)
            //  {
            //      SFamtb = dr.CopyToDataTable().DefaultView.ToTable(true, "FAMILY_ID").Copy();
            //      if (SFamtb != null)
            //      {
            //          for (int i = 0; i <= SFamtb.Rows.Count - 1; i++)
            //          {
            //              _Familyids = _Familyids + "," + SFamtb.Rows[i]["FAMILY_ID"].ToString();
            //              _Familyids = _Familyids.Replace(",,", ",");
            //          }
            //      }
            //  }

            _stg_container = new StringTemplateGroup("main", micro_stemplatepath);

            _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "DownloadMain");



            if (_Familyids != "")
            {

                // DataSet TempEADs = GetFamilyPageProduct(_Familyids, "ATTACHMENT");
                if (TempEADs != null && TempEADs.Tables.Count > 0 && TempEADs.Tables[tableinx].Rows.Count > 0)
                {
                    //TempEADs.Tables[0].Columns.Add("FAMILY_NAME");



                    string[] strf = _Familyids.Split(new string[] { "," }, StringSplitOptions.None);
                    if (strf.Length > 0)
                    {
                        lstrecords = new TBWDataList[strf.Length];
                        for (int intfam = 0; intfam <= strf.Length - 1; intfam++)
                        {
                            dr = null;
                            dr = TempEADs.Tables[tableinx].Select("FAMILY_ID='" + strf[intfam] + "'", "Sno");
                            if (dr.Length > 0)
                            {
                                //dt = new DataTable();
                                //dt = dr.CopyToDataTable();
                                rtnstr = ST_Familypage_Download(_Fid, strf[intfam], dr);
                                if (rtnstr != "")
                                {
                                    lstrecords[ictrecords] = new TBWDataList(rtnstr.ToString());
                                    ictrecords = ictrecords + 1;
                                }

                            }
                        }





                    }
                }





            }
            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
            //string DownloadST_Product = ST_Product_Download(TmpDs);
            //_stmpl_container.SetAttribute("PRODUCT_DOWNLOAD", DownloadST_Product);
//|| DownloadST_Product != ""
            if (ictrecords > 0 )
            {

                DownloadST = _stmpl_container.ToString();
                isDownload = true;
                return "block";
            }


            DownloadST = "";
            isDownload = false;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
        return "none";


    }

    public string ST_Product_Download(DataSet TmpDs)
    {
        string rtnstr = "";
        // StringTemplateGroup _stg_container = null;
      //  StringTemplateGroup _stg_records = null;
       // StringTemplate _stmpl_container = null;
       // StringTemplate _stmpl_records = null;
       // StringTemplate _stmpl_records1 = null;
      //  StringTemplate _stmpl_recordsrows = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];

      //  StringTemplateGroup _stg_container1 = null;
       // StringTemplateGroup _stg_records1 = null;
        TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        TBWDataList1[] lstrows1 = new TBWDataList1[0];
        string downloadst_pro = string.Empty;


        DataTable dt = new DataTable();
      //  DataRow[] dr = null;

       // int ictrecords = 0;



        string DownloadST_Product = "";




        string _pid_multiple = string.Empty;
        string pcode_multiple = string.Empty;
        for (int prd = 0; prd <= TmpDs.Tables["FamilyPro"].Rows.Count - 1; prd++)
        {

            string _pid = TmpDs.Tables["FamilyPro"].Rows[prd]["PRODUCT_ID"].ToString();
            string prodcode = TmpDs.Tables["FamilyPro"].Rows[prd]["PRODUCT_CODE"].ToString() + " - Product Downloads";
            if (_pid != "")
            {
                if (_pid_multiple == string.Empty)
                {
                    _pid_multiple = _pid;
                    pcode_multiple = prodcode;
                }
                else
                {
                    _pid_multiple = _pid_multiple + "," + _pid;
                    pcode_multiple = pcode_multiple + "," + prodcode;

                }
            }

        }

        DataSet TempEADs_pid = objFamilyServices.GetFamilyPageProduct(_pid_multiple, "PRODUCT_ATTACHMENT");
        string[] pid = _pid_multiple.Split(',');
        string[] pcode = pcode_multiple.Split(',');
        //Added if to fix error in logfile 3 feb 2016
        if (TempEADs_pid != null)
        {
            for (int i = 0; i <= pid.Length - 1; i++)
            {
                DataRow[] drpid = TempEADs_pid.Tables[0].Select("PRODUCT_ID='" + pid[i] + "'");
                if (drpid.Length > 0)
                {

                    rtnstr = ST_Productpage_Download(drpid, pcode[i].ToString());
                    if (rtnstr != "")
                    {
                        DownloadST_Product = DownloadST_Product + rtnstr;
                    }
                }

            }
        }
        return DownloadST_Product;

    }

    public string ST_Productpage_Download(DataRow[] Adt, string Protitle)
    {

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
        string strfile = "";
        if (Adt != null && Adt.Length > 0)
        {




            DataSet dscat = new DataSet();


            try
            {
                _stg_records = new StringTemplateGroup("cell", micro_stemplatepath);
                _stg_container = new StringTemplateGroup("row", micro_stemplatepath);

                // _stg_container = new StringTemplateGroup("row", _SkinRootPath);


                lstrecords = new TBWDataList[Adt.Length + 1];



                int ictrecords = 0;

                foreach (DataRow dr in Adt)//For Records
                {
                    strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");


                    FileInfo Fil;

                    string strImgFilesnew = System.Configuration.ConfigurationManager.AppSettings["VirtualPathJPG"].ToString(); 


                    if (dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains(".jpg") == true)
                        Fil = new FileInfo(strImgFilesnew + dr["PRODUCT_ATT_FILE"].ToString());
                    else
                        Fil = new FileInfo(strPDFFiles1 + dr["PRODUCT_ATT_FILE"].ToString());


                    if (Fil.Exists)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf("Familypage" + "\\" + "DownloadCell_Product");

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

                        _stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", dr["PRODUCT_ATT_FILE"].ToString().Replace("/Attachments/", "/attachments/").Replace(".PDF", ".pdf"));
                        _stmpl_records.SetAttribute("TBT_ATTACH_SIZE", FileInKB.ToString() + " KB");


                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }
                }

                _stmpl_container = _stg_container.GetInstanceOf("FamilyPage" + "\\" + "DownloadRow_Product");

                _stmpl_container.SetAttribute("TBT_PRODUCT_CODE", Protitle);
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                if (ictrecords > 0)
                {
                    //isdownload_product = true;
                    return _stmpl_container.ToString();
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "";
            }

            return "";
        }
        return "";
    }

    public string ST_FamilypageALLData()
    {
        StringTemplateGroup _stg_container = null;
        //StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
      //  StringTemplate _stmpl_records = null;

        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];


        TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        TBWDataList1[] lstrows1 = new TBWDataList1[0];


        //micro_stemplatepath = micro_stemplatepath + "\\" + "templates\\microsite\\";
        _stg_container = new StringTemplateGroup("main", micro_stemplatepath);

        _stmpl_container = _stg_container.GetInstanceOf("CSFAMILYPAGE" + "\\" + "FPmain");
        //EADs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
        int rtn = GetFamilyAllData("");
        if (rtn == -1)
            return "";

        _stmpl_container.SetAttribute("Generateparentfamilyhtml", Generateparentfamilyhtml());
        _stmpl_container.SetAttribute("FAMILY_ATT_FILTER", ST_Top_filter());
        _stmpl_container.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());

        string cthref = "";

        cthref = ST_VPC();
        _stmpl_container.SetAttribute("TBT_CT_HREF", cthref);
        HttpContext.Current.Session["hfprevfid"] = null;
        _stmpl_container.SetAttribute("ST_Familypage", ST_Familypage("",HttpContext.Current.Request.RawUrl.ToString().Replace("/fl/", "")));

        hfeapath.Value = HttpContext.Current.Session["EA"].ToString();
        //Session["EA"].ToString();
        HFcnt.Value = "0";
        if (_Fid != "")
        {
            hffid.Value = _Fid;
        }
        else
        {
            if (HttpContext.Current.Request.QueryString["fid"] != null)
                hffid.Value = HttpContext.Current.Request.QueryString["fid"].ToString();
        }
        hfrawurl.Value = HttpContext.Current.Request.RawUrl.ToString().Replace("/fl/", "");

        if (HttpContext.Current.Session[hffid.Value + "Icnt"] != null)
        {
            itotalrecords.Value = HttpContext.Current.Session[hffid.Value + "Icnt"].ToString();
            if (Convert.ToInt16(HttpContext.Current.Session[hffid.Value + "Icnt"].ToString()) > 1)
            {
                hfcheckload.Value = "1";
            }
            else
            {
                hfcheckload.Value = "0";
            }
        }
         
        
        _stmpl_container.SetAttribute("ST_Family_Download", ST_Family_Download(EADs, Dsall, Dsall.Tables.Count - 1));

        if (_Fid != "")
        {
            hffid.Value = _Fid;
        }
        else
        {
            if (HttpContext.Current.Request.QueryString["fid"] != null)
                hffid.Value = HttpContext.Current.Request.QueryString["fid"].ToString();
        }
       
        if (DownloadST == "")
        {
            chkdwld = false;
            _stmpl_container.SetAttribute("DownloadST", ST_Downloads_Update());
            _stmpl_container.SetAttribute("ST_Family_Download", "block");

        }
        else
        {
            chkdwld = true;
            string dwldmrge = ST_Downloads_Update();
            dwldmrge = DownloadST + dwldmrge;
            _stmpl_container.SetAttribute("DownloadST", dwldmrge);
        }
       _stmpl_container.SetAttribute("ST_BulkBuyPP", ST_BulkBuyPP());
       if (HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] != null)
       {
           _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"].ToString());
           _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString());
       }
        return _stmpl_container.ToString();

    }
    public string ST_Top_filter()
    {
        try
        {
            HelperServices objHelperServices = new HelperServices();
            ConnectionDB objConnectionDB = new ConnectionDB();

            TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("searchrsltcategorytop", micro_stemplatepath, objConnectionDB.ConnectionString);

            return tbwtMSEngine.ST_Top_attribute_Filter();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }
    public string ST_Downloads_Update()
    {
        StringTemplateGroup _stg_container = null;
        StringTemplate _stmpl_container = null;
        try
        {

            _stg_container = new StringTemplateGroup("main", micro_stemplatepath);

            if (chkdwld == false)
                _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "DowloadUpdate");
            else
                _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "WithDowloadUpdate");

            if (HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] != null)
            {
                _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"].ToString());
                _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString());
            }

            if (HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] != null)
                _stmpl_container.SetAttribute("TBT_FAMILY_NAME_DU", HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"].ToString());
            //if (chkdwld == false)
            //    _stmpl_container.SetAttribute("IS_DWLD_MRGE", true);
            //  else
            //    _stmpl_container.SetAttribute("IS_DWLD_MRGE", false);

            return _stmpl_container.ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
        return "";
    }
    public string ST_BulkBuyPP()
    {
        StringTemplateGroup _stg_container = null;
        StringTemplate _stmpl_container = null;
        StringTemplateGroup _stg_records1 = null;
        StringTemplate _stg_container_records1 = null;
        try
        {

            _stg_container = new StringTemplateGroup("main", micro_stemplatepath);

            _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "BulkBuyPP");


            _stg_records1 = new StringTemplateGroup("row", micro_stemplatepath);
            // _stg_container_records1 = _stg_records1.GetInstanceOf("Familypage" + "\\" + "multilistitem");
            string shtml = "";
            if (HttpContext.Current.Session["prodcodedesc"] != null)
            {
                string codedescall = HttpContext.Current.Session["prodcodedesc"].ToString();
                string[] codedesc = codedescall.Split('|');
                for (int i = 0; i < codedesc.Length - 1; i++)
                {
                    _stg_container_records1 = _stg_records1.GetInstanceOf("Familypage" + "\\" + "multilistitem");
                    string prodcode = "";
                    prodcode = codedesc[i].Trim();
                    //_stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
                    //if (i == 0)
                    //    _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");

                    if (codedesc.Length > 2)
                    {
                        _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
                        _stmpl_container.SetAttribute("TBT_CHK_PRODCOUNT", true);
                    }
                    else
                    {
                        _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
                        _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");
                        _stmpl_container.SetAttribute("TBT_CHK_PRODCOUNT", false);
                    }


                    //if (i == 0)
                    //{
                    //    _stg_container_records1.SetAttribute("TBW_LIST_VAL", "Please Select Product");
                    //    _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");
                    //}
                    //else
                    //    _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);

                    shtml = shtml + _stg_container_records1.ToString();
                }
                if (HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] != null)
                {
                    _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"].ToString());
                    _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString());
                }

                if (HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] != null)
                    _stmpl_container.SetAttribute("TBT_FAMILY_NAME_BB", HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"].ToString());
                // _stmpl_container.SetAttribute("TBW_DDL_VALUE", _stg_container_records1.ToString());
                _stmpl_container.SetAttribute("TBW_DDL_VALUE", shtml.ToString());
            }
            else
            {
                Response.Redirect("home.aspx",false);
            }
            return _stmpl_container.ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
        return "";
    }
    public string ST_Familypage_Download(string pFamilyid, string Familyid, DataRow[] Adt)
    {
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;

        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];


        TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        TBWDataList1[] lstrows1 = new TBWDataList1[0];



        long FileInKB;
        string[] file = null;
        string strfile = "";
        if (Adt.Length > 0)  //if (Adt != null && Adt.Rows.Count > 0)
        {




            DataSet dscat = new DataSet();


            try
            {
                _stg_records = new StringTemplateGroup("cell", micro_stemplatepath);
                _stg_container = new StringTemplateGroup("row", micro_stemplatepath);


                lstrecords = new TBWDataList[Adt.Length + 1];



                int ictrecords = 0;

                foreach (DataRow dr in Adt)//For Records
                {
                    strfile = dr["FAMILY_ATT_FILE"].ToString().Replace(@"\\", "/");


                    FileInfo Fil;
                    string strImgFilesnew = System.Configuration.ConfigurationManager.AppSettings["VirtualPathJPG"].ToString(); 


                    if (dr["FAMILY_ATT_FILE"].ToString().ToLower().Contains(".jpg") == true)
                        Fil = new FileInfo(strImgFilesnew + dr["FAMILY_ATT_FILE"].ToString());
                    else
                        Fil = new FileInfo(strPDFFiles1 + dr["FAMILY_ATT_FILE"].ToString());


                    if (Fil.Exists)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf("Familypage" + "\\" + "DownloadCell");

                        strfile = dr["FAMILY_ATT_FILE"].ToString().Replace(@"\\", "/");
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


                        _stmpl_records.SetAttribute("TBT_FAMILY_ATT_DESC", dr["FAMILY_ATT_DESC"].ToString());

                        //Modified by indu 10Sep2015
                        //_stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", dr["FAMILY_ATT_FILE"].ToString());
                        _stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", strfile.Replace("/Attachments/", "/attachments/").Replace(".PDF", ".pdf"));
                        _stmpl_records.SetAttribute("TBT_ATTACH_SIZE", FileInKB.ToString() + " KB");


                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }
                }

                _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "DownloadRow");

                _stmpl_container.SetAttribute("TBT_FAMILY_NAME", Adt[0]["FAMILY_NAME"].ToString());
                if (pFamilyid.ToLower() == Familyid.ToLower())
                {
                    _stmpl_container.SetAttribute("TBT_FAMILY_HEAD", false);
                }
                else
                {
                    _stmpl_container.SetAttribute("TBT_FAMILY_HEAD", true);
                }



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
        return "";
    }

    public DataSet ConvertXMLToDataSet(string xmlData)
    {
        StringReader stream = null;
        XmlTextReader reader = null;
        try
        {
            DataSet xmlDS = new DataSet();
            stream = new StringReader(xmlData);
            // Load the XmlTextReader from the stream
            reader = new XmlTextReader(stream);
            xmlDS.ReadXml(reader);
            return xmlDS;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (reader != null) reader.Close();
        }
    }

  
    public int GetFamilyAllData(string _Fid)
    {

        //have to get the id
        EADs = (DataSet)HttpContext.Current.Session["FamilyProduct"];

        DataSet tempDs = new DataSet();
        DataTable Ftb = new DataTable();


        DataTable Atttbl = new DataTable();

        DataRow[] DrMain = null;
        DataRow[] Drsub = null;
        DataRow[] Dr = null;
        string _UserID = "";

        string _cid = "";
        string _pcr = "";

        if (HttpContext.Current.Request.QueryString["fid"] != null)
            _Fid = HttpContext.Current.Request.QueryString["fid"].ToString();
        if (HttpContext.Current.Request.QueryString["cid"] != null)
            _cid = HttpContext.Current.Request.QueryString["cid"].ToString();
        if (HttpContext.Current.Request.QueryString["pcr"] != null)
            _pcr = HttpContext.Current.Request.QueryString["pcr"].ToString();

        if (HttpContext.Current.Session["USER_ID"] != null)
            _UserID = HttpContext.Current.Session["USER_ID"].ToString();


        if (_UserID == "" || _UserID == null)
            _UserID = System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

        string ExecString = "";



        int tblinx = 0;


        _Familyids = _Fid;

        string x ="";
      
        if (EADs != null)
        {

            if (EADs.Tables["FamilyPro"].Rows.Count > 0)
            {

             //   Modified By :Indu 11-Dec-2017

                //ExecString = "exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + _Fid + "','PRODUCT'";
                x = objFamilyServices.getleftattribute(_Fid);
                if (x != "")
                {
                    ExecString = "exec STP_TBWC_PICKFAMILYPAGEPRODUCT_TABLEDESIGNER '" + _Fid + "','PRODUCT','" + x + "'";
                }
                else
                {
                    ExecString = "exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + _Fid + "','PRODUCT'";
                }

                DrMain = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + _Fid + "'");

                Drsub = EADs.Tables["FamilyPro"].Select("FAMILY_ID<>'" + _Fid + "'");
                if (Drsub.Length > 0)
                {
                    SFamtb = Drsub.CopyToDataTable().DefaultView.ToTable(true, "FAMILY_ID").Copy();
                    foreach (DataRow dr in SFamtb.Rows)
                    {
                      //  ExecString = ExecString + ";exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + dr["FAMILY_ID"].ToString() + "','PRODUCT'";

                        x = objFamilyServices.getleftattribute(dr["FAMILY_ID"].ToString());

                        if (x != "")
                        {
                            ExecString = ExecString + ";exec STP_TBWC_PICKFAMILYPAGEPRODUCT_TABLEDESIGNER '" + dr["FAMILY_ID"].ToString() + "','PRODUCT','" + x + "'";

                        }
                        else
                        {
                            ExecString = ExecString + ";exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + dr["FAMILY_ID"].ToString() + "','PRODUCT'";

                        }
                            _Familyids = _Familyids + "," + dr["FAMILY_ID"].ToString();
                        _Familyids = _Familyids.Replace(",,", ",");
                    }

                }
            }
            //ExecString=ExecString+";exec STP_TBWC_PICKFPRODUCTPRICE '"+ _Familyids +"','','"+_UserID+"'";

            //ExecString=ExecString+";exec STP_TBWC_PICKGENERICDATA '2',"+EADs.Tables[0].Rows[0]["Family_id"].ToString()+",'2','','','GET_FAMILY_ATTRIBUTE'";

            ExecString = ExecString + ";exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + _Familyids + "','ATTACHMENT'";




            string tmpProds = "";
            if (Convert.ToInt32(_UserID) > 0)
            {
                foreach (DataRow drpid in EADs.Tables["FamilyPro"].Rows)
                {
                    tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                    tmpProds = tmpProds.Replace(",,", ",");
                }
                if (tmpProds != "")
                {
                    tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);

                    dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(_UserID));
                }
            }


            Dsall = objHelperDB.GetDataSetDB(ExecString);

            if (Dsall == null)
            {
                return -1;
            }
            else if (Dsall.Tables.Count <= 0)
            {
                return -1;
            }



            // Main fl
            if ((EADs.Tables["FamilyPro"] != null) && (EADs.Tables["FamilyPro"].Rows.Count >0))
            {
                DrMain = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + _Fid + "'");
                if (DrMain.Length > 0)
                {
                    // Famtb = Dr.CopyToDataTable();

                    ConstructFamilyData(_Fid, DrMain, Dsall, tblinx);
                }
       
            // Sub fl


            if (Drsub.Length > 0)
            {

                if (SFamtb != null)
                {
                    for (int i = 0; i <= SFamtb.Rows.Count - 1; i++)
                    {
                        Dr = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + SFamtb.Rows[i]["FAMILY_ID"].ToString() + "'");
                        if (Dr.Length > 0)
                        {
                            // Ftb = Dr.CopyToDataTable();
                            tblinx = tblinx + 1;
                            ConstructFamilyData(SFamtb.Rows[i]["FAMILY_ID"].ToString(), Dr, Dsall, tblinx);
                        }
                        //_Familyids = _Familyids + "," + SFamtb.Rows[i]["FAMILY_ID"].ToString();
                        //_Familyids = _Familyids.Replace(",,", ",");
                    }

                }

            }
            }
            //tblinx = tblinx + 1;
            //Dsall.Tables[tblinx].TableName = "ProductPrice"; 
            //Ds.Tables.Add(Dsall.Tables[tblinx].Copy());




            // Get fl Attribute
            //if (_Familyids != "")
            //    tempDs = GetFamilyPageProduct(_Familyids, "ATTRIBUTE");
            //if (tempDs != null && tempDs.Tables.Count > 0)
            //{

            //    tempDs.Tables[0].TableName = "Attribute";
            //    Ds.Tables.Add(tempDs.Tables[0].Copy());
            //}




            //DataTable Sqltb = new DataTable();


            //Sqltb = objhelper.GetDataTable(StrSql);
            // Sqltb = (DataTable)objHelperDB.GetGenericDataDB("2", EADs.Tables[0].Rows[0]["Family_id"].ToString(), "2", "GET_FAMILY_ATTRIBUTE", HelperDB.ReturnType.RTTable);

            //  DataRow dRow=null;
            //  dRow = EADs.Tables[0].Rows[0];
            //  tblinx = tblinx + 1;
            ////  Ds.Tables.Add(Dsall.Tables[tblinx].Copy());

            //  if (Dsall.Tables[tblinx] != null)
            //   {
            //       foreach (DataRow dr in Dsall.Tables[tblinx].Rows)
            //       {

            //           dRow["ATTRIBUTE_ID"] = dr["ATTRIBUTE_ID"];
            //           dRow["STRING_VALUE"] = dr["STRING_VALUE"];
            //           dRow["NUMERIC_VALUE"] = dr["NUMERIC_VALUE"];
            //           dRow["OBJECT_TYPE"] = dr["OBJECT_TYPE"];
            //           dRow["OBJECT_NAME"] = dr["OBJECT_NAME"];
            //           dRow["ATTRIBUTE_NAME"] = dr["ATTRIBUTE_NAME"];
            //           dRow["ATTRIBUTE_TYPE"] = dr["ATTRIBUTE_TYPE"];
            //           EADs.Tables[0].Rows.Add(dRow.ItemArray);
            //       }
            //   }





            //// Main fl
            //DrMain = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + _Fid + "'");
            //if (DrMain.Length > 0)
            //{
            //    // Famtb = Dr.CopyToDataTable();
            //    ConstructFamilyData(_Fid, DrMain);
            //}
            //// Sub fl

            //SFamtb = new DataTable();
            //Drsub = EADs.Tables["FamilyPro"].Select("FAMILY_ID<>'" + _Fid + "'");
            //if (Drsub.Length > 0)
            //{
            //    SFamtb = Drsub.CopyToDataTable().DefaultView.ToTable(true, "FAMILY_ID").Copy();
            //    if (SFamtb != null)
            //    {
            //        for (int i = 0; i <= SFamtb.Rows.Count - 1; i++)
            //        {
            //            Dr = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + SFamtb.Rows[i]["FAMILY_ID"].ToString() + "'");
            //            if (Dr.Length > 0)
            //            {
            //                // Ftb = Dr.CopyToDataTable();
            //                ConstructFamilyData(SFamtb.Rows[i]["FAMILY_ID"].ToString(), Dr);
            //            }
            //            _Familyids = _Familyids + "," + SFamtb.Rows[i]["FAMILY_ID"].ToString();
            //            _Familyids = _Familyids.Replace(",,", ",");
            //        }

            //    }

            //}
            //// Get Product Price & Inventory
            //tempDs = GetProductPrice(_Familyids, "", _UserID);
            //if (tempDs != null && tempDs.Tables.Count > 0)
            //{
            //    tempDs.Tables[0].TableName = "ProductPrice";
            //    Ds.Tables.Add(tempDs.Tables[0].Copy());
            //}
            // Construct Sub FamilyData
            // ConstructSubFamilyHeader(EADs, SFamtb); 
            return 1;


        }

        return -1;

    }
    public string Dynamic_pagination(string pagno, string _fid, string eapath, string Rawurl)
    {

        Get_Value_Breadcrum(pagno, _fid, eapath, Rawurl);
        int rtn = GetFamilyAllData(_fid);
        return ST_Familypage(_fid, Rawurl);

    }
    public void Get_Value_Breadcrum(string pagno, string _fid, string eapath, string Rawurl)
    {

      
        if (HttpContext.Current.Session["EA"] != null)
        {
            if (HttpContext.Current.Session["EA"].ToString().Contains(_fid))
            {

                EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", iRecordsPerPage.ToString(), pagno, "Next");
            }
            else
            {

                AssignSubdsEApath(_fid, Rawurl);
                EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", iRecordsPerPage.ToString(), pagno, "Next");
            }
        }
       
    }
    public void AssignSubdsEApath(string FamilyId, string Rawurl)
    {
        DataSet subds = new DataSet();
        try
        {
            //if (HttpContext.Current.Session["SubCategory"] != null)
            //{

            //    subds = (DataSet)HttpContext.Current.Session["SubCategory"];
            //}

            //else
            //{
            //    string strxml = HttpContext.Current.Server.MapPath("xml");

            //   // subds.ReadXml(strxml + "\\" + "subds.xml");
            //   // JObject o1 = JObject.Parse(File.ReadAllText(strxml + "\\" + "subds.txt"));
            //    //subds = JsonConvert.DeserializeObject<DataSet>(o1.ToString());

            //    using (StreamReader subdstxt = File.OpenText(strxml + "\\" + "subds.txt"))
            //    {
            //        using (JsonReader reader = new JsonTextReader(subdstxt))
            //        {
            //            JsonSerializer serializer = new JsonSerializer();
            //            subds = (DataSet)serializer.Deserialize(subdstxt, typeof(DataSet));
            //            reader.Close();
            //        }
            //        subdstxt.Dispose();
            //    }

            //    HttpContext.Current.Session["SubCategory"] = subds;

            //}

            subds = (DataSet)HttpContext.Current.Application["key_SubCategory"];
            DataTable dt = subds.Tables[0];
            DataRow[] foundRows;

            //string querystring = HttpContext.Current.Request.RawUrl.ToString().ToLower();
            string[] ConsURL = null;

            ConsURL = (Rawurl + "/fl/").Split('/');

            Array.Reverse(ConsURL);
            string urlpath = string.Empty;



           
                urlpath = ConsURL[6] + "/" + ConsURL[7];
       


            foundRows = dt.Select("URL_RW_PATH='" + urlpath + "' ");

            string prevEA = string.Empty;

            if (foundRows.Length > 0)
            {
                HttpContext.Current.Session["EA"] = foundRows[0]["EA_PATH"].ToString() + "////" + foundRows[0]["TBT_PARENT_CATEGORY_NAME"].ToString() + "////" + foundRows[0]["CATEGORY_NAME"].ToString() + "////" + "UserSearch1=Family Id=" + FamilyId;
                // _catCid = foundRows[0]["CATEGORY_ID"].ToString();

            }




        }
        catch
        { }
    }

   
    public string ST_Familypage(string _fid,string Rawurl)
    {

       // objErrorHandler.CreateLog("Inside ST_Family_page");  
        string _cid = "";
        string _pcr = "";
        string _UserID = "";
        _Fid = _fid;
        if (HttpContext.Current.Request.QueryString["fid"] != null)
            _Fid = HttpContext.Current.Request.QueryString["fid"].ToString();
        if (HttpContext.Current.Request.QueryString["cid"] != null)
            _cid = HttpContext.Current.Request.QueryString["cid"].ToString();
        if (HttpContext.Current.Request.QueryString["pcr"] != null)
            _pcr = HttpContext.Current.Request.QueryString["pcr"].ToString();

        if (HttpContext.Current.Session["USER_ID"].ToString() != null)
            _UserID = HttpContext.Current.Session["USER_ID"].ToString();
      
        _Familyids = _Fid;
        int tablecount = 0;
        HttpContext.Current.Session["prodcodedesc"] = null;
        try
        {
            contentvalue = "";
            if (HttpContext.Current.Request.QueryString["fid"] != null || (_Fid != ""))
            {

                #region comments

                #endregion
                //by jtech
                tablecount = tablecount + 1;
                int clmcnt = 0;
                int clmrowcnt = 0;
                //Added if to fix error in logfile 3 feb 2016
                if ( (Ds != null) && (Ds.Tables.Count>0)  )
                {
                    if (Ds.Tables[_Fid] == null)
                    {
                        clmcnt = Ds.Tables[0].Columns.Count;
                        clmrowcnt = Ds.Tables[0].Rows.Count;
                    }
                    else
                    {
                        clmcnt = Ds.Tables[_Fid].Columns.Count;
                        clmrowcnt = Ds.Tables[_Fid].Rows.Count;
                    }


                    //if (clmcnt <= 8)
                    //{
                    //    CScontentvalue = ObjFamilyPage.GenerateHorizontalHTMLJson(_Fid, Ds, dsPriceTableAll, EADs, Rawurl); 
                    //}

                    //else
                    //{
                    //    if (clmrowcnt <= 5)
                    //    {
                    //        CScontentvalue = ObjFamilyPage.GenerateHorizontalHTMLJson(_Fid, Ds, dsPriceTableAll, EADs, Rawurl); 
                    //    }
                    //    else
                    //    {
                            for (int i = 0; i < clmrowcnt; i++)
                            {
                                int endcount = i + 5;

                                if (endcount > clmrowcnt - 1)
                                {
                                    endcount = clmrowcnt;
                                }

                                string strCScontentvalue = ObjFamilyPage.GenerateHorizontalHTMLJsonMS(_Fid, Ds, dsPriceTableAll, EADs, tablecount, i, endcount, Rawurl);
                                if (i == 0)
                                {
                                    CScontentvalue = strCScontentvalue;
                                }
                                else
                                {
                                    CScontentvalue = CScontentvalue + "<div></div>" + strCScontentvalue;

                                }
                                i = i + 4;
                            }


                        //}

                  //  }
                    if (Famtb.Rows.Count == 1 && SFamtb.Rows.Count == 0 && (HttpContext.Current.Request.QueryString["ProductResult"] != null && HttpContext.Current.Request.QueryString["ProductResult"].ToString().Equals("SUCCESS")))
                    {
                        Response.Redirect("/mpd.aspx?pid=" + Famtb.Rows[0]["Product_ID"].ToString() + "&fid=" + HttpContext.Current.Request.QueryString["fid"].ToString() + "&cid=" + _cid + "&pcr=" + _pcr + "byp=2", false);
                        return "";
                    }
                    else if (SFamtb != null)
                    {
                        string subfamproduct = "";
                        if (HttpContext.Current.Session["prodcodedesc"] != null)
                            subfamproduct = HttpContext.Current.Session["prodcodedesc"].ToString();
                        foreach (DataRow DR in SFamtb.Rows)
                        {
                            string cssubfamilycontent = "";
                            //cssubfamilycontent = getcstable(DR["Family_id"].ToString());
                            tablecount = tablecount + 1;
                            // cssubfamilycontent = ObjFamilyPage.GenerateHorizontalHTMLJsonMS(DR["Family_id"].ToString(), Ds, dsPriceTableAll, EADs, tablecount);
                            //modified  by indu for vertical view
                            string subfid = DR["Family_id"].ToString();
                            if (Ds.Tables[subfid].Columns.Count <= 8)
                            {
                                cssubfamilycontent = ObjFamilyPage.GenerateHorizontalHTMLJsonMS(DR["Family_id"].ToString(), Ds, dsPriceTableAll, EADs, tablecount, 0, Ds.Tables[subfid].Rows.Count, Rawurl);
                            }

                            else
                            {
                                if (Ds.Tables[subfid].Rows.Count <= 5)
                                {
                                    cssubfamilycontent = ObjFamilyPage.GenerateHorizontalHTMLJsonMS(DR["Family_id"].ToString(), Ds, dsPriceTableAll, EADs, tablecount, 0, Ds.Tables[DR["Family_id"].ToString()].Rows.Count, Rawurl);
                                }
                                else
                                {
                                    for (int i = 0; i < Ds.Tables[subfid].Rows.Count; i++)
                                    {
                                        int endcount = i + 5;

                                        if (endcount > Ds.Tables[subfid].Rows.Count - 1)
                                        {
                                            endcount = Ds.Tables[subfid].Rows.Count;
                                        }

                                        string strCScontentvalue = ObjFamilyPage.GenerateHorizontalHTMLJsonMS(_Fid, Ds, dsPriceTableAll, EADs, tablecount, i, endcount, Rawurl);
                                        if (i == 0)
                                        {
                                            cssubfamilycontent = strCScontentvalue;
                                        }
                                        else
                                        {
                                            cssubfamilycontent = cssubfamilycontent + "<div></div>" + strCScontentvalue;

                                        }
                                        i = i + 4;
                                    }


                                }
                            }










                            if (cssubfamilycontent != "" && cssubfamilycontent.Length > 336)
                            {
                                templatename = "CSFAMILYPAGEWITHSUBFAMILY";
                                tbwtEngine = new TBWTemplateEngineMS(templatename, micro_stemplatepath, objConnectionDB.ConnectionString);
                                tbwtEngine.paraValue = DR["Family_id"].ToString();
                                tbwtEngine.paraFID = DR["Family_id"].ToString();

                                //tbwtEngine.RenderHTML("Row");
                                //subfamtemplate = subfamtemplate + tbwtEngine.RenderedHTML;
                                subfamtemplate = subfamtemplate + tbwtEngine.ST_SubFamily_Load(EADs);
                                subfamtemplate = subfamtemplate + cssubfamilycontent;
                            }
                            else
                            {
                                cssubfamilycontent = "";
                            }
                            if (HttpContext.Current.Session["prodcodedesc"] != null)
                            {
                                subfamproduct = subfamproduct + HttpContext.Current.Session["prodcodedesc"].ToString();
                            }
                        }
                        HttpContext.Current.Session["prodcodedesc"] = subfamproduct;
                    }
                    //templatename = "CSFAMILYPAGELOGO";
                    //tbwtEngine = new TBWTemplateEngine(templatename, Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                    //tbwtEngine.RenderHTML("Row");
                    //  if (subfamtemplate != "")
                    //     subfamtemplate = "<div class=\"title7\">Related Products</div>" + subfamtemplate;
                    //contentvalue = contentvalue + "<div style=\"overflow:auto; width:780px; height:100%;\">" + CScontentvalue + subfamtemplate + "</div>" + tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                    contentvalue = contentvalue + "<div style=\" height:100%;\">" + CScontentvalue + subfamtemplate + "</div>"; //+ tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

                }
            }
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return e.Message;
        }
        return objHelperServices.StripWhitespace(contentvalue.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));

    }
    // by Jtech
    //private DataSet GetDataSetX(string category_id)
    //{

    //    DataSet catid = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter("select convert(int, option_value) as OPTION_VALUE from tbwc_store_options where option_name = 'DEFAULT CATALOG'", conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
    //    da.Fill(catid, "generictable");
    //    int[] AttributeIdList = new int[0];
    //    CatalogXfunction oProdTable = new CatalogXfunction();
    //    DataSet dds = oProdTable.WebCatalogFamily(Convert.ToInt32(catid.Tables[0].Rows[0].ItemArray[0].ToString()), 178966, AttributeIdList, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
    //    return dds;
    //}

    //private DataSet GetDataSetFX(string familyid)
    //{
    //    DataSet prodtable = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter("select distinct tps.PRODUCT_ID from tb_prod_specs tps,tb_prod_family tpf, tbwc_inventory ti where ti.product_id= tps.product_id and ti.product_status <> 'DISABLE' and tps.product_id=tpf.product_id and tpf.family_id=" + familyid, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
    //    da.Fill(prodtable, "Producttable");
    //    DataSet subfamtable = new DataSet();
    //    DataTable DT = new DataTable("generictable");
    //    da = new SqlDataAdapter("select sf.family_id,sf.subfamily_id from tb_subfamily sf,tb_catalog_family cf where cf.family_id=sf.subfamily_id and cf.catalog_id=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " and  sf.family_id=" + familyid, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
    //    da.Fill(DT);
    //    prodtable.Tables.Add(DT);
    //    return prodtable;
    //}

    //private DataSet GetDataSetSFX(string familyid)
    //{
    //    DataSet prodtable = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter("select distinct tps.PRODUCT_ID from tb_prod_specs tps,tb_prod_family tpf, tbwc_inventory ti where ti.product_id = tps.product_id and ti.product_status <> 'DISABLE' and tps.product_id=tpf.product_id and tpf.family_id=" + familyid, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
    //    da.Fill(prodtable, "Producttable");
    //    return prodtable;
    //}


    private DataSet GetFamilyPageProduct(string familyid, string Option)
    {
        return objFamilyServices.GetFamilyPageProduct(familyid, Option);
    }
    private DataSet GetProductPrice(string familyids, string productids, string UserID)
    {
        DataSet ds = new DataSet();
        SqlCommand objSqlCommand;
        SqlDataAdapter da;
        //SqlConnection Gcon = new SqlConnection();
        //Gcon.ConnectionString = conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1);
        try
        {


            // objErrorHandler.CreateLog("Prod & family :" + familyids+ "-"+productids );

            if (familyids.StartsWith(",") == true)
                familyids = familyids.Substring(1);

            if (productids.StartsWith(",") == true)
                productids = productids.Substring(1);

            objSqlCommand = new SqlCommand("STP_TBWC_PICKFPRODUCTPRICE", objConnectionDB.GetConnection());
            objSqlCommand.CommandType = CommandType.StoredProcedure;
            objSqlCommand.Parameters.Add("@FamilyIDs", familyids);
            objSqlCommand.Parameters.Add("@ProductIDs", productids);
            objSqlCommand.Parameters.Add("@UserID", UserID);
            da = new SqlDataAdapter(objSqlCommand);
            da.Fill(ds);
        }

        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
        }
        finally
        {
            objSqlCommand = null;
            da = null;
        }

        return ds;
    }
    //public string getcstable(string familyid)
    //{
    //    CSProductTable oCSProdTab = new CSProductTable(familyid, oHelper.GetOptionValues("DEFAULT CATALOG"));
    //    oCSProdTab.UserID = oHelper.CI(Session["USER_ID"]);
    //    oCSProdTab.DesciptionAttributeWidth = 300;
    //    oCSProdTab.DesciptionHighAttributeWidth = 180;
    //    oCSProdTab.DesciptionMidumAttributeWidth = 80;
    //    oCSProdTab.DesciptionNormalAttributeWidth = 440;
    //    oCSProdTab.ProductImgHeight = 100;
    //    oCSProdTab.ProductImgWidth = 100;
    //    oCSProdTab.IsVisibleShipping = true;
    //    return (oCSProdTab.GenerateFamilyPreview().Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">&rarr;</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
    //}

    public string Generateparentfamilyhtml()
    {
        try
        {
            contentvalue = "";
            if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "")
            {
                //DDS = GetDataSetFX(Request.QueryString["fid"].ToString());
                templatename = "CSFAMILYPAGE";
                tbwtEngine = new TBWTemplateEngineMS(templatename, micro_stemplatepath, objConnectionDB.ConnectionString);
                if (Request.QueryString["fid"] != null)
                {
                    tbwtEngine.paraValue = Request.QueryString["fid"].ToString();
                    tbwtEngine.paraFID = Request.QueryString["fid"].ToString();
                }
                //tbwtEngine.RenderHTML("Row");            
                //if(tbwtEngine.RenderedHTML!=null)
                //contentvalue = tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

                contentvalue = tbwtEngine.ST_Family_Load(EADs);
                if (contentvalue != null)
                    contentvalue = contentvalue.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
            }
            return objHelperServices.StripWhitespace(contentvalue);

        }


        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }

    }
}
