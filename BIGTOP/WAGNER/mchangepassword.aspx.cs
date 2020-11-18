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
    public partial class mchangepassword : System.Web.UI.Page
    {
        Security objcrpengine = new Security();
        UserServices objUserServices = new UserServices();
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        string OldPwd = "";
        public string micrositeurl = "";
        public string strsupplierName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["SUPPLIER_NAME"] != null)
            {
                strsupplierName = Session["SUPPLIER_NAME"].ToString();
                micrositeurl ="/"+  objHelperServices.SimpleURL_MS_Str(strsupplierName, "mct.aspx",false) + "/mct/";
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            int UsrID = objHelperServices.CI(Session["USER_ID"].ToString());
            try
            {
                if (Request["ChangePass"] != null)
                {
                    txtOldPassword.Text = objUserServices.GetPassword(objHelperServices.CI(Session["USER_ID"].ToString()));
                 }
                if (txtOldPassword.Text != "" && txtNewPassword.Text != "" && txtNewConfirmPassword.Text != "") //txtOldPassword.Text 
                {
                    OldPwd = objUserServices.GetPassword(UsrID);
        
                    string cibertext = objcrpengine.StringEnCrypt_password(txtOldPassword.Text);
                    if (OldPwd != cibertext)
                    // if (OldPwd != txtOldPassword.Text.Trim().ToString())
                    {
                        //lblError.Text = (string)GetLocalResourceObject("ErrMsgInValid");
                        lblError.Text = "Invalid old password";
                    }
                    else if (txtNewPassword.Text.Trim().ToString() != txtNewConfirmPassword.Text.Trim().ToString())
                    {
                       // lblError.Text = (string)GetLocalResourceObject("ErrMsgPwdSame");
                        lblError.Text = "New password and confirm password are not same";
                     
                    }
                    else if (txtNewPassword.Text.Trim().ToString() != txtOldPassword.Text.Trim().ToString())
                    {
                        if (OldPwd == cibertext)
                    
                        {
    
                            string cipherText = objcrpengine.StringEnCrypt_password(txtNewPassword.Text);
                            if (txtNewPassword.Text.Trim().Length < 6)
                            {
                               // lblError.Text = (string)GetLocalResourceObject("ErrMsgPwdLimit");
                                lblError.Text = "Password must be 6 or more characters";
                                //"Password must be 6 or more characters.";
                            }

                            else if (objUserServices.ChangePassword(UsrID, cipherText) > 0)
                  
                            {
                  
                                Session["USER_ID"] = "";
                                Session["USER_NAME"] = null;
                                if (Request.Cookies["BigtopLoginInfo"] != null && Request.Cookies["BigtopLoginInfo"].Value.ToString().Trim() != "")
                                {
                                    HttpCookie LoginInfoCookie = new HttpCookie("BigtopLoginInfo");
                                    LoginInfoCookie["UserName"] = objUserServices.GetUserEmailAdd(UsrID);
                                    LoginInfoCookie["Password"] = txtNewPassword.Text;
                                    LoginInfoCookie.Expires = DateTime.Now.AddDays(1);
                                    HttpContext.Current.Response.AppendCookie(LoginInfoCookie);
                                }
                                Response.Redirect("ConfirmMessage.aspx?Result=PASSWORDCHANGED",false);
                            }
                        }
                        else
                        {
                           // lblError.Text = (string)GetLocalResourceObject("ErrMsgInValid");
                            lblError.Text = "Invalid old password";
                        }
                    }
                    else
                    {
                        //lblError.Text = (string)GetLocalResourceObject("ErrMsgNoPwdSame");
                        lblError.Text = "Old password and new password should not be same";
                        if (Request["ChangePass"] != null && Request["ChangePass"] == "True")
                        {
                            Session.RemoveAll();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
    }
}