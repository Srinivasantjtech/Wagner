using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
namespace WES
{
    public partial class PopChangePwd : System.Web.UI.Page
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        Security objSecurity = new Security();
        UserServices objUserServices = new UserServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Change Password";
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Text != "" || txtConfirmPassword.Text != "")
            {
                string pass = txtNewPassword.Text.Trim();
                string Cpass = txtConfirmPassword.Text.Trim();
                pass = objSecurity.StringEnCrypt_password(txtNewPassword.Text);
                Cpass = objSecurity.StringEnCrypt_password(txtConfirmPassword.Text);
                if (pass != "")
                {
                    if (Cpass == pass)
                    {
                        // string userid = Request.QueryString["Uname"].ToString();
                        string userid = "";
                        if (Session["USER_ID_Ch"] != null)
                        {
                            userid = Session["USER_ID_Ch"].ToString();
                        }

                        //userid = objSecurity.StringDeCrypt(HttpUtility.UrlDecode(Request.QueryString["Uname"]));
                        int uid = Convert.ToInt32(userid);

                        string username = objUserServices.GetUserLoginName(uid);

                        int UserID = uid;
                        string Role = objUserServices.GetRole(UserID);

                        objUserServices.ChangePassword(UserID, pass);
                        Session["USER_ID_Ch"] = "";
                        Session["USER_NAME"] = username;
                        Session["USER_ID"] = UserID;
                        Session["USER_ROLE"] = Role;
                        Session["COMPANY_ID"] = objUserServices.GetCompanyID(UserID);
                        Session["CUSTOMER_TYPE"] = objUserServices.GetCustomerType(UserID);

                        LogSession();
                        HttpContext.Current.Response.Redirect("/Home.aspx");
                        HttpContext.Current.Response.Close();
                    }
                    else
                    {
                        txtConfirmPassword.Text = "";

                        lblError.Text = "Confirm password does not match";
                        lblError.Text = "Confirm password does not match";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script language='javascript' type='text/javascript'> alert('Confirm password does not match')</script>", true);
                    }
                }
                else
                {
                    lblError.Text = "Invalid Password";
                }
            }
            else
            {
               // lblError.Text = "Please Enter Password";
            
            }
        }

        private void LogSession()
        {
            try
            {
                UserSessionServices objUserSession = new UserSessionServices();
                UserSessionServices.UserSessionInfo oUSI = new UserSessionServices.UserSessionInfo();
                string sReferralURL = Request.ServerVariables["HTTP_REFERER"].ToString();
                oUSI.Session_ID = Session.SessionID;
                oUSI.Referal_URL = sReferralURL;
                oUSI.User_ID = objHelperServices.CI(Session["USER_ID"]);
                oUSI.Last_IP = Request.ServerVariables["REMOTE_ADDR"].ToString();
                if (oUSI.User_ID > 0)
                {
                    objUserSession.TrackSession(oUSI);
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