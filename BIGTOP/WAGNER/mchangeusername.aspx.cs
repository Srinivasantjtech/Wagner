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

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

namespace WES
{
    public partial class mchangeusername : System.Web.UI.Page
    {
        UserServices objUserServices = new UserServices();
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        string OldUserName = "";
        public string micrositeurl = "";
        public string strsupplierName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
         
            try
            {
                if (Session["SUPPLIER_NAME"] != null)
                {
                    strsupplierName = Session["SUPPLIER_NAME"].ToString();
                    micrositeurl = objHelperServices.SimpleURL_MS_Str(strsupplierName, "mct.aspx",false) + "/mct/";
                }
                // Page.Title = Helper.oHelper.GetOptionValues("BROWSER TITLE"].ToString();
                if (!IsPostBack)
                {


                    txtOldUserName.Text = objUserServices.GetUserLoginName(objHelperServices.CI(Session["USER_ID"].ToString()));
                    //txtOldUserName.BackColor = System.Drawing.Color.DarkGray;
                    txtOldUserName.ReadOnly = true;
                    txtNewUserName.Focus();
                    OldUserName = objUserServices.GetUserLoginName(objHelperServices.CI(Session["USER_ID"].ToString()));
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            txtOldUserName.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
            txtNewUserName.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
            txtConfirmUserName.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
        }
        protected void btnChange_Click(object sender, EventArgs e)
        {
            int UsrID = objHelperServices.CI(Session["USER_ID"].ToString());
            int cmpid = objUserServices.GetCompanyID(UsrID);
            try
            {
                if (Request["ChangePass"] != null)
                {
                    //txtOldUserName.Text = objUserServices.GetPassword(objHelperServices.CI(Session["USER_ID"].ToString()));
                    //rfvOldPwd.Visible = false;
                }
                if (txtOldUserName.Text != "" && txtNewUserName.Text != "" && txtConfirmUserName.Text != "") //txtOldUserName.Text 
                {
                    OldUserName = objUserServices.GetUserLoginName(UsrID);

                    if (OldUserName.ToUpper() != txtOldUserName.Text.ToUpper().Trim().ToString())
                    {
                        lblError.Text = (string)GetLocalResourceObject("ErrMsgInValid");
                    }
                    else if (txtNewUserName.Text.Trim().ToString() != txtConfirmUserName.Text.Trim().ToString())
                    {
                        lblError.Text = (string)GetLocalResourceObject("ErrMsgPwdSame");
                        //"New UserName and confirm UserName are not same"
                    }
                    else if (txtNewUserName.Text.ToString().ToUpper().StartsWith(cmpid.ToString().ToUpper()) == true)
                    {
                        lblError.Text = "User name should not contain " + cmpid.ToString() + ",try with different user name";
                    }
                    else if (txtNewUserName.Text.ToString().ToUpper().StartsWith("WES") == true || txtNewUserName.Text.ToString().ToUpper().StartsWith("CELL") == true || txtNewUserName.Text.ToString().ToUpper().StartsWith("WAG") == true)
                    {
                        lblError.Text = "User name should not contain `WES` or `CELL` or `WAG`,try with different user name";
                    }
                    else if (txtNewUserName.Text.Trim().ToString() != txtOldUserName.Text.Trim().ToString())
                    {
                        if (OldUserName.ToUpper() == txtOldUserName.Text.ToUpper().Trim().ToString())
                        {

                            DataSet tmpdb = objUserServices.CheckMultipleLoginName(txtNewUserName.Text, "");
                            if (txtNewUserName.Text.Trim().Length < 6)
                            {
                                lblError.Text = (string)GetLocalResourceObject("ErrMsgPwdLimit");
                                //"UserName must be 6 or more characters.";
                            }
                            else if (tmpdb != null && tmpdb.Tables.Count > 0 && tmpdb.Tables[0].Rows.Count > 0)
                            {
                                lblError.Text = (string)GetLocalResourceObject("ErrMsgExists");
                            }
                            else if (objUserServices.ChangeLoginName(UsrID, txtNewUserName.Text) > 0)
                            {
                                // Session.RemoveAll();
                                Session["USER_ID"] = "";
                                Session["USER_NAME"] = null;
                                if (Request.Cookies["BigtopLoginInfo"] != null && Request.Cookies["BigtopLoginInfo"].Value.ToString().Trim() != "")
                                {
                                    HttpCookie LoginInfoCookie = new HttpCookie("BigtopLoginInfo");
                                    LoginInfoCookie["UserName"] = txtNewUserName.Text;
                                    LoginInfoCookie["Password"] = objUserServices.GetPassword(UsrID);
                                    LoginInfoCookie.Expires = DateTime.Now.AddDays(1);
                                    HttpContext.Current.Response.AppendCookie(LoginInfoCookie);
                                }
                                Response.Redirect("ConfirmMessage.aspx?Result=LOGINNAMECHANGED",false);
                            }
                        }
                        else
                        {
                            lblError.Text = (string)GetLocalResourceObject("ErrMsgInValid");
                        }
                    }
                    else
                    {
                        lblError.Text = (string)GetLocalResourceObject("ErrMsgNoPwdSame");
                        if (Request["ChangePass"] != null && Request["ChangePass"] == "True")
                        {
                            Session.RemoveAll();
                        }
                    }
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
    }
}