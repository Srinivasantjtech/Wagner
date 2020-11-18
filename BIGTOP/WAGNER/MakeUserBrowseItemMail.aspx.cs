using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Data;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Text;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;


    public partial class MakeUserBrowseItemMail : System.Web.UI.Page
    {
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        OrderServices objOrderServices = new OrderServices();
        UserServices objUserServices = new UserServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            int rtnmail = 0;
            DataTable temptbl = objHelperServices.CheckUserBI();
            if (temptbl != null && temptbl.Rows.Count > 0)
            {
                foreach (DataRow dr in temptbl.Rows)
                {
                    rtnmail = SendMail(dr["USER_ID"].ToString());
                }
            }
        }

        public string GetLoginName_top(string userid)
        {
            string retvalue = string.Empty;
            //string userid = HttpContext.Current.Session["USER_ID"].ToString();
            DataTable objDt = new DataTable();
            string _CATALOG_ID = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"];
            try
            {
                if (!string.IsNullOrEmpty(userid))
                {
                    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"];
                    string iLoginName = string.Empty;

                        objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                        if (objDt != null && objDt.Rows.Count > 0)
                            iLoginName = objDt.Rows[0]["CONTACT"].ToString();

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
        private int SendMail(string User_id)
        {
            int rtnval = 0;
            string sHTML = "";
            string stemplatepath;
            string Emailadd ="";
            try
            {
                DataSet dsOItem = new DataSet();
                dsOItem = objHelperServices.GetBrowseItem(User_id);
                if (dsOItem == null || dsOItem.Tables.Count <= 0)
                    return 0;
                try
                {
                    //UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
                    StringTemplateGroup _stg_container = null;
                    StringTemplateGroup _stg_records = null;
                    StringTemplate _stmpl_container = null;
                    StringTemplate _stmpl_records = null;

                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];


                    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                    TBWDataList1[] lstrows1 = new TBWDataList1[0];

                    stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                    _stg_records = new StringTemplateGroup("row", stemplatepath);
                    _stg_container = new StringTemplateGroup("main", stemplatepath);
                    lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];
                    Emailadd = objUserServices.GetUserEmailAdd(Convert.ToInt32(User_id.ToString()));
                    int ictrecords = 0;

                    foreach (DataRow dr in dsOItem.Tables[0].Rows)//For Records
                    {
                        string descall = dr["DESCRIPTION"].ToString();
                        _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "browserow");

                        _stmpl_records.SetAttribute("Image", dr["IMAGE_PATH"].ToString());

                        if (descall.Length > 100)
                        {
                            descall = descall.Substring(0, 100);
                            descall = descall + "...";
                        }

                      //  _stmpl_records.SetAttribute("Desc", dr["DESCRIPTION"].ToString());
                        _stmpl_records.SetAttribute("Desc", descall);
                        _stmpl_records.SetAttribute("Price", dr["COST"].ToString());
                        _stmpl_records.SetAttribute("Fname", dr["FAMILY_NAME"].ToString());
                        _stmpl_records.SetAttribute("url", dr["URL"].ToString());
                        _stmpl_records.SetAttribute("siteurl", HttpContext.Current.Request.Url.Authority.ToString());
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }
                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "browse");

                    _stmpl_container.SetAttribute("ORDER_ID", "");

                    string contact_name = GetLoginName_top(User_id);

                    _stmpl_container.SetAttribute("CONTACT", contact_name);

                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);

                    //string url1 = HttpContext.Current.Request.Url.Authority.ToString();
                    // url1 = url1+ "/checkout.aspx?" +EncryptSP(oOrderInfo.OrderID.ToString());

                    //url1 = url1 + "/OrderDetails.aspx?ORDER_ID=" + oOrderInfo.OrderID.ToString() + "&bulkorder=1&ViewOrder=View&OrdFlg=" + objSecurity.StringEnCrypt_password("OPENORDER");


                   // _stmpl_container.SetAttribute("TBT_CHKOUT_URL", url1);

                    sHTML = _stmpl_container.ToString();
                }
                catch (Exception ex)
                {
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    sHTML = "";
                }
                if (sHTML != "")
                {

                   System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                    MessageObj.Subject = "Your Browse Items - WAGNER";
                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;
                    MessageObj.To.Add(Emailadd.ToString());

                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);
                    return 1;
                }

            }
            catch (Exception ex)
            {

            }
            return rtnval;

        }
    }
