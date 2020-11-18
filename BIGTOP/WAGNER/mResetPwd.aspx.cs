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
using TradingBell.WebCat;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;

    public partial class mResetPwd : System.Web.UI.Page
    {
        private static int _UserID;
        Security objcrpengine = new Security();
        UserServices objUserServices = new UserServices();
        CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
        NotificationServices objNotificationServices = new NotificationServices();
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
        AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
        int usercheck;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                Page.Title = "Wagner-ResetPassword";
                if (Request.QueryString["pwdKey"] != null && Request.QueryString["pwdKey"].ToString().Trim() != "" && Request.QueryString["loginName"] != null && Request.QueryString["loginName"].ToString().Trim() != "")
                {
                    bool validUser;
                    string username;
                    int UserID;
                    // int usercheck;
                    txtLoginName.Text = Request.QueryString["loginName"];
                    username = txtLoginName.Text;
                    validUser = objUserServices.CheckUserName(username);
                    UserID = objUserServices.GetUserID(username);
                    //txtCompanyID.Text = objUserServices.GetCompanyID(UserID).ToString();

                    if (Request.QueryString["UserId"] != null)
                    {
                        usercheck = Convert.ToInt32(Request.QueryString["UserId"].ToString());
                        if (objUserServices.GetUserStatus(objHelperServices.CI(Request.QueryString["UserId"].ToString())) == 1)
                        {
                            Response.Redirect("mLogin.aspx");
                        }
                        oUserInfo = objUserServices.GetUserInfo(usercheck);
                        if (oUserInfo.CUSTOMER_TYPE == "Retailer")
                        {

                            string retailermailladdress = objUserServices.GetUserEmailAdd(usercheck);
                            //Modified by:Indu
                            //Modified on:11-Apr-2013
                            //Mofified reason:To Build encrption logic
                            string temppwd = objcrpengine.StringEnCrypt_password(Request.QueryString["pwdKey"]);
                            if (objUserServices.CheckValidUserForForgetPassword(txtLoginName.Text, retailermailladdress, temppwd))
                            {
                                lblErrorMessage.Text = "";
                                lblErrorMessage.Visible = false;
                                Session["RESETSTATUS"] = "RESETVALID";
                                if (Session["RESETSTATUS"] != null && Session["RESETSTATUS"].ToString().Trim().Equals("RESETVALID"))
                                {
                                    divemail.Visible = false ;
                                   
                                        ShowPopUpReset();
                                   
                                }
                            }
                            else
                            {
                                lblErrorMessage.Text = "Invalid Account Information! Contact Wes Admin and then Try again";
                                divemail.Visible = true;
                                lblErrorMessage.Visible = true;
                            }
                        }
                    }

                    if (UserID == -1 || username == string.Empty || validUser == false)
                    {
                        Session["ForgotAction"] = "Invalid User Name!";
                        Response.Redirect("/mLogin.aspx", true);
                    }

                    Session["RESETSTATUS"] = "RESETVALID";
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {


                //Modified by:Indu
                //Modified on:11-Apr-2013
                //Mofified reason:To Build encrption logic

                string cipherText = objcrpengine.StringEnCrypt_password(Request.QueryString["pwdKey"]);
                int UserID;
                UserID = objUserServices.GetUserID(txtLoginName.Text);

                // if (objUserServices.CheckValidUserForForgetPassword(txtLoginName.Text, txtEmailAddress.Text, Request.QueryString["pwdKey"]))
                if (objUserServices.CheckValidUserForForgetPassword(txtLoginName.Text, txtEmailAddress.Text, cipherText))
                {
                    lblErrorMessage.Text = "";
                    lblErrorMessage.Visible = false;

                    if (Session["RESETSTATUS"] != null && Session["RESETSTATUS"].ToString().Trim().Equals("RESETVALID"))
                    {
                        //Code Modified by:palani

                      
                            ShowPopUpReset();
                       

                    }
                }
                else
                {
                    lblErrorMessage.Text = "Invalid Account Information! Contact Wes Admin and then Try again";
                    lblErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
        }

        private void ShowPopUpReset()
    {
        popupreset.Visible = true;
        
    }

        private void HidePopUpReset()
        {
            btnSubmit.UseSubmitBehavior = true;
            btnUpdate.UseSubmitBehavior = false;
            this.modalPop.Hide();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                bool validUser;
                string username;
                int UserID;

                lblPwdErrorMessage.Visible = false;
                username = txtLoginName.Text;
                validUser = objUserServices.CheckUserName(username);
                UserID = objUserServices.GetUserID(username);
                string TempPwd = Request.QueryString["pwdKey"].ToString();

               
                    txtCompanyID.Text = oUserInfo.CompanyID;
                    string retailermailladdress = objUserServices.GetUserEmailAdd(usercheck);
                    if (System.Convert.ToInt32(txtCompanyID.Text) == objUserServices.GetCompanyID(UserID) && objUserServices.CheckForgotUserEmail(username, retailermailladdress))
                    {

                        string cipherText = objcrpengine.StringEnCrypt_password(txtConfirmPassword.Text);
                        TempPwd = objcrpengine.StringEnCrypt_password(Request.QueryString["pwdKey"].ToString());
                        int chkvalue = objUserServices.UpdateNewPassword(txtLoginName.Text, UserID, System.Convert.ToInt32(txtCompanyID.Text), retailermailladdress, TempPwd, cipherText);

                        if (chkvalue <= 0)
                        {
                            lblPwdErrorMessage.Text = "Unable to Proceed! Check Company AccountNo and Email";
                            lblPwdErrorMessage.Visible = true;
                            return;
                        }

                        Response.Redirect("mConfirmMessage.aspx?Result=RESETPASSWORD", false);
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
