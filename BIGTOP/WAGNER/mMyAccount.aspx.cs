﻿using System;
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
using System.Web.Configuration;
using System.Web.Services;
using System.Data.SqlClient;







namespace WES
{
    public partial class mMyAccount : System.Web.UI.Page
    {
        public string micrositeurl = "";
        HelperServices objHelperServices = new HelperServices();
        public string strsupplierName = "";
        OrderServices objOrderServices = new OrderServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        UserServices objUserServices = new UserServices();
        UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
        CountryServices objCountryServices = new CountryServices();
        ConnectionDB objConnectionDB = new ConnectionDB();
        ActiveCampaignService objActiveCampaignService = new ActiveCampaignService();
        Security objSecurity = new Security();
        string oldmail = "";
        int userid = 0;
        string MicroSiteTemplate = "";
        public string strsupplierDesc = "";
        public string strsupplierId = "";
        string OldPwd = "";
        string OldUserName = "";
        NotificationServices objNotificationServices = new NotificationServices();

        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
        public static int tick_count = 0;
        AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
        int Userid;
        MailChimpManagerServices objmcms = new MailChimpManagerServices();
        public static System.Web.UI.Control GetPostBackControl(System.Web.UI.Page page)
        {
            Control control = null;
            string ctrlname = page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            // if __EVENTTARGET is null, the control is a button type and we need to 
            // iterate over the form collection to find it
            else
            {
                string ctrlStr = String.Empty;
                Control c = null;
                foreach (string ctl in page.Request.Form)
                {
                    // handle ImageButton controls ...
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        ctrlStr = ctl.Substring(0, ctl.Length - 2);
                        c = page.FindControl(ctrlStr);
                    }
                    else
                    {
                        c = page.FindControl(ctl);
                    }
                    if (c is System.Web.UI.WebControls.Button ||
                                c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            tabcontrolHiddenField.Value = "false";
            tabcontrolprofileHiddenField.Value = "false";
            tabcontrolprosuccessField.Value = "false";
            if (Session["SUPPLIER_NAME"] != null)
            {
                strsupplierName = Session["SUPPLIER_NAME"].ToString();
                micrositeurl = "/" + objHelperServices.SimpleURL_MS_Str( strsupplierName, "mct.aspx",false) + "/mct/";
            }
            try
            {
                if (!IsPostBack)
                {
                    tabcontrolHiddenField.Value = "true";

                    txtOldUserName.Text = objUserServices.GetUserLoginName(objHelperServices.CI(Session["USER_ID"].ToString()));
                    //txtOldUserName.BackColor = System.Drawing.Color.DarkGray;
                    txtOldUserName.ReadOnly = true;
                    txtNewUserName.Focus();
                    OldUserName = objUserServices.GetUserLoginName(objHelperServices.CI(Session["USER_ID"].ToString()));
                }

                if (!IsPostBack)
                {
                    if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
                    {
                        LoadCountryList();

                        AssignUserData(Session["USER_ID"].ToString());

                        string countryCode = drpCountry.SelectedValue.ToString();
                        //LoadStates(countryCode);
                        string ShipcountryCode = drpShipCountry.SelectedValue.ToString();
                        //ShipLoadStates(ShipcountryCode);
                        string BillcountryCode = drpBillCountry.SelectedValue.ToString();
                        //BillLoadStates(BillcountryCode);
                    }
                    else
                    {
                        Response.Redirect("/mLogin.aspx", false);
                    }

                }
                if (Session["SUPPLIER_NAME"] != null)
                {
                    strsupplierName = Session["SUPPLIER_NAME"].ToString();
                    micrositeurl =  objHelperServices.SimpleURL_MS_Str( strsupplierName, "mct.aspx",true) + "/mct/";
                }

                if (Session["USER_NAME"] == null || Session["USER_NAME"] == "")
                {
                    Response.Redirect("/Login.aspx",false);
                }
                if (Convert.ToInt16(Session["USER_ROLE"]) == 4)
                {
                    Response.Redirect("/Home.aspx",false);
                }
                if (Request.QueryString["PDFSession"] != null)
                {
                    if (Request.QueryString["PDFSession"].ToString() == "true")
                    {

                        //string tmppath = HttpContext.Current.Request.Url.OriginalString.Replace(HttpContext.Current.Request.Url.Query, "");
                        //HtmlMeta hm1 = new HtmlMeta();

                        //hm1.HttpEquiv = "refresh";
                        //hm1.Content = "5;" + tmppath;
                        //HtmlHead head = (HtmlHead)Page.Header;
                        //head.Controls.Add(hm1);


                        ShowProcessSignalMessage1();
                    }
                    else
                    {
                        ShowProcessSignalMessage();
                    }
                }

                if (IsPostBack)
                {

                    if ((Session["tickcheck"] != null && Session["tickcheck"].ToString() != "") && (Convert.ToInt32(Session["tickcheck"]) >= 30))
                    {

                        Timer1.Enabled = false;
                        if (Convert.ToInt32(Session["tickcheck"]) > 30)
                        {
                            Session["tickcheck"] = null;
                            this.modalPop.Hide();
                            Response.Redirect("Orderhistory.aspx?PDFSession=false",false );
                            ShowProcessSignalMessage();
                        }
                        else
                        {

                            Session["tickcheck"] = null;


                            Response.Redirect("Orderhistory.aspx?PDFSession=true",false);
                            this.modalPop.Hide();



                            //string pdffile = Session["pdffile"].ToString();
                            //string PdfFileName = Session["PdfFileName"].ToString();
                            //if (System.IO.File.Exists(pdffile))
                            //{

                            //    Session["pdffile"] = null;
                            //    Session["PdfFileName"] = null;
                            //    Response.AppendHeader("refresh", "1");
                            //    Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
                            //    Response.ContentType = "application/pdf";
                            //    Response.WriteFile(pdffile);
                            //    Response.End();
                            //}                   
                        }
                    }
                }
                //  Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
                Page.Title = "Wagner-Order History";
                if (Session["USER_NAME"] == null)
                {
                    Session["USER"] = "";
                    Session["COUNT"] = "0";
                    Response.Redirect("/Login.aspx",false);
                }

                if (!Page.IsPostBack)
                {

                    if (Request["Key"] != null)
                    {
                        tick_count = 0;
                        string _Key = Server.HtmlDecode(Request["Key"].ToString());
                        string DecryptedValueString = objSecurity.Decrypt(_Key, Session.SessionID);

                        if (!string.IsNullOrEmpty(DecryptedValueString))
                        {
                            string[] PdfFileName = DecryptedValueString.Split('|');
                            string pdffile = Server.MapPath(string.Format("~/Invoices/In{0}", PdfFileName[1] + ".pdf"));
                            Session["PdfFileName"] = PdfFileName[1].ToString();
                            Session["pdffile"] = pdffile;
                            //System.Threading.Thread.Sleep(100000);
                            if (System.IO.File.Exists(pdffile))
                            {
                                //Timer1.Enabled = false;
                                Session["tickcheck"] = null;
                                //System.Threading.Thread.Sleep(20000);
                                //ShowProcessSignalMessage();
                                Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(pdffile);
                                Response.End();

                            }
                            else
                            {
                                if (PdfFileName[0] != null && PdfFileName[0].ToString().Trim() != string.Empty)
                                {
                                    int cStatus = 0;
                                    string cOrderNo = PdfFileName[0].ToString();
                                    cStatus = objOrderServices.SentSignalInvoiceNotification(cOrderNo);
                                    if (cStatus > 0)
                                    {
                                        if (tick_count == 0)
                                        {
                                            //Response.Write("<script>alert('timer started');</script>");
                                            ShowProcessMessage();
                                            //ShowProcessSignalMessage();
                                            //Timer1.Interval = 3000;
                                            //Timer1.Elapsed += new ElapsedEventHandler(timer1_Tick);
                                            //// Only raise the event the first time Interval elapses.
                                            //Timer1.AutoReset = false;
                                            Timer1.Enabled = true;
                                            Timer1.Interval = 500;
                                            tick_count = 1;
                                        }
                                    }
                                    else
                                    {
                                        ShowProcessSignalMessage();
                                    }

                                }
                            }
                        }
                        else
                        {
                            ShowProcessSignalMessage();
                        }

                    }

                    OrderNoHiddenField.Value = "";
                    FromDateHiddenField.Value = "";
                    ToDateHiddenField.Value = "";
                    UserHiddenField.Value = "";
                    Userid = Convert.ToInt32(Session["User_id"]);
                    UserList(Userid);

                    CreatedUserDropDownlist.SelectedIndex = 0;
                    UserHiddenField.Value = CreatedUserDropDownlist.SelectedItem.Text;
                    OrderHistory();

                    //if (Session["pdffile"] != null && Request["Key"] == null)
                    //{

                    //    string pdffile = Session["pdffile"].ToString();
                    //    string PdfFileName = Session["PdfFileName"].ToString();
                    //    if (System.IO.File.Exists(pdffile))
                    //    {

                    //        Session["pdffile"] = null;
                    //        Session["PdfFileName"] = null;
                    //        //Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName + ".pdf"));
                    //        //Response.ContentType = "application/pdf";
                    //        //Response.WriteFile(pdffile);
                    //        //Response.Write("<script type='text/javascript'> window.open('"+pdffile+"','_blank'); </script>");
                    //        //Response.End();
                    //        //Response.Redirect("Orderhistory.aspx");

                    //        Response.Clear();
                    //        Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName + ".pdf"));
                    //        Response.ContentType = "application/pdf";

                    //        //Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
                    //        //Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
                    //        //Response.ContentType = "application/pdf";
                    //        Response.Flush();
                    //        Response.WriteFile(pdffile);
                    //        //Response.Write("<script type='text/javaScript'> window.location.href ='Orderhistory.aspx'; </script>");



                    //    }
                    //}
                }
                else
                {
                    OrderNoHiddenField.Value = OrderNo.Text;
                    FromDateHiddenField.Value = FromdateTextBox.Text;
                    ToDateHiddenField.Value = TodateTextBox.Text;
                    UserHiddenField.Value = CreatedUserDropDownlist.SelectedItem.Text;
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
            txtOldUserName.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
            txtNewUserName.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
            txtConfirmUserName.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
        }
  protected void btnUpdate_Click(object sender, EventArgs e)
    {
       try
       {
        
        DataSet dsMail =new DataSet(); 
        if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
        {
           userid = Convert.ToInt32(Session["USER_ID"].ToString().Trim());
           oldmail=objUserServices.GetUserEmailAdd(userid);

            if (oldmail != txtAltEmail.Text.Trim().ToString())   
            {
                dsMail = objUserServices.CheckCustomerRegistrationExists(txtAltEmail.Text.Trim().ToString(), "Retailer");
                   if (dsMail!=null && dsMail.Tables.Count>0 && dsMail.Tables[0].Rows.Count>0)
                   {
                       ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Mail address already exists');", true);                
                       return;
                   }
            }

            if (UpdateUserData() > 0)
            {
                if (oldmail != txtAltEmail.Text.Trim().ToString())
                {
                    if (chkissubscribe.Checked == false)
                    {
                        objmcms.UnSubscribeChimpMember("", oldmail.ToString());
                    }
                  //  objActiveCampaignService.SetContact_Subscribe(oldmail.ToString(), txtFname.Text.Trim(), "", 2, false);
                  //  objUserServices.SetUserRole(userid.ToString(), 4);
                    SendUpdateCustomer(userid);
                }
                else
                {
                    
                   // Response.Redirect("mConfirmMessage.aspx?Result=UPDATE");
                    tabcontrolprosuccessField.Value = "true";
                }
            }
        }
        else
        {
            Response.Redirect("/mLogin.aspx",false );
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

              string cibertext = objSecurity.StringEnCrypt_password(txtOldPassword.Text);
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

                      string cipherText = objSecurity.StringEnCrypt_password(txtNewPassword.Text);
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
                          Response.Redirect("mConfirmMessage.aspx?Result=PASSWORDCHANGED",false);
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
  protected void btnChangeLoginName_Click(object sender, EventArgs e)
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
                  lblErrorpwd.Text = (string)GetLocalResourceObject("ErrMsgInValid");
              }
              else if (txtNewUserName.Text.Trim().ToString() != txtConfirmUserName.Text.Trim().ToString())
              {
                  lblErrorpwd.Text = (string)GetLocalResourceObject("ErrMsgPwdSame");
                  //"New UserName and confirm UserName are not same"
              }
              else if (txtNewUserName.Text.ToString().ToUpper().StartsWith(cmpid.ToString().ToUpper()) == true)
              {
                  lblErrorpwd.Text = "User name should not contain " + cmpid.ToString() + ",try with different user name";
              }
              else if (txtNewUserName.Text.ToString().ToUpper().StartsWith("WES") == true || txtNewUserName.Text.ToString().ToUpper().StartsWith("CELL") == true || txtNewUserName.Text.ToString().ToUpper().StartsWith("WAG") == true)
              {
                  lblErrorpwd.Text = "User name should not contain `WES` or `CELL` or `WAG`,try with different user name";
              }
              else if (txtNewUserName.Text.Trim().ToString() != txtOldUserName.Text.Trim().ToString())
              {
                  if (OldUserName.ToUpper() == txtOldUserName.Text.ToUpper().Trim().ToString())
                  {

                      DataSet tmpdb = objUserServices.CheckMultipleLoginName(txtNewUserName.Text, "");
                      if (txtNewUserName.Text.Trim().Length < 6)
                      {
                          lblErrorpwd.Text = (string)GetLocalResourceObject("ErrMsgPwdLimit");
                          //"UserName must be 6 or more characters.";
                      }
                      else if (tmpdb != null && tmpdb.Tables.Count > 0 && tmpdb.Tables[0].Rows.Count > 0)
                      {
                          lblErrorpwd.Text = (string)GetLocalResourceObject("ErrMsgExists");
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
                          Response.Redirect("mConfirmMessage.aspx?Result=LOGINNAMECHANGED",false);
                      }
                  }
                  else
                  {
                      lblErrorpwd.Text = (string)GetLocalResourceObject("ErrMsgInValid");
                  }
              }
              else
              {
                  lblErrorpwd.Text = (string)GetLocalResourceObject("ErrMsgNoPwdSame");
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
    public void LoadShippingInfo(string sUserID)
    {
        try
        {

            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oUserinfo = objUserServices.GetUserInfo(_UserID);
            //txtShipCompName.Text = objUserServices.GetCompanyName(_UserID);
            //cmbPrefix.Text = oUserinfo.Prefix;
           // txtShipFname.Text = oUserinfo.FirstName;
            txtshipadd1.Text = oUserinfo.ShipAddress1;
            txtshipadd2.Text = oUserinfo.ShipAddress2;
            txtshipadd3.Text = oUserinfo.ShipAddress3;
            txtshipcity.Text = oUserinfo.ShipCity;
            drpShipState.Text = oUserinfo.ShipState;
            txtshipzip.Text = oUserinfo.ShipZip;
            drpShipCountry.SelectedValue = oUserinfo.ShipCountry;
            Setdrpdownlistvalue(drpShipCountry, oUserinfo.ShipCountry.ToString());
            if (drpShipCountry.SelectedValue == "AU")
            {
                Setdrpdownlistvalue(ddlshipstate, oUserinfo.ShipState.ToString());
                drpShipState.Visible = false;
                ddlshipstate.Visible = true;
                RVshipddlstate.Enabled = false;
                RVshiptxtstate.Enabled = true;
            }
            else
            {

                drpShipState.Visible = true;
                ddlshipstate.Visible = false;
                RVshipddlstate.Enabled = true;
                RVshiptxtstate.Enabled = false;
            }
            txtshipphone.Text = oUserinfo.ShipPhone;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    private string Setdrpdownlistvalue(DropDownList d, string val)
    {
        ListItem li;
        string returnselected = "";
        for (int i = 0; i < d.Items.Count; i++)
        {
            li = d.Items[i];
            if (li.Text.ToUpper() == val.ToUpper())
            {
                d.SelectedIndex = i;
                returnselected = li.Text.ToUpper();
                break;
            }
        }
        return returnselected;
    }
    public void LoadBillInfo(string sUserID)
    {
        try
        {
            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oUserinfo = objUserServices.GetUserBillInfo(_UserID);

            //txtbillCompanyName.Text = objUserServices.GetBillToCompanyName(_UserID);
            //txtbillFName.Text = oUserinfo.FirstName;
            txtbilladd1.Text = oUserinfo.BillAddress1;
            txtbilladd2.Text = oUserinfo.BillAddress2;
            txtbilladd3.Text = oUserinfo.BillAddress3;
            txtbillcity.Text = oUserinfo.BillCity;
            drpBillState.Text = oUserinfo.BillState;
            txtbillzip.Text = oUserinfo.BillZip;
            drpBillCountry.SelectedValue = oUserinfo.BillCountry;
            Setdrpdownlistvalue(drpBillCountry, oUserinfo.BillCountry.ToString());
            if (drpBillCountry.SelectedValue == "AU")
            {
                Setdrpdownlistvalue(ddlbillstate, oUserinfo.BillState.ToString());
                drpBillState.Visible = false;
                ddlbillstate.Visible = true;
                RVddlBillstate.Enabled = true;
                RVtxtBillstate.Enabled = false;
            }
            else
            {
                drpBillState.Visible = true;
                ddlbillstate.Visible = false;
                RVddlBillstate.Enabled = false;
                RVtxtBillstate.Enabled = true;
            }
            txtbillphone.Text = oUserinfo.BillPhone;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void AssignUserData(string sUserID)
    {
        try
        {
            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oUserinfo = objUserServices.GetUserInfo(_UserID);
            txtcompname.Text = objUserServices.GetCompanyName(_UserID);
            //cmbPrefix.Text = oUserinfo.Prefix;
            txtFname.Text = oUserinfo.FirstName;
            txtLname.Text = oUserinfo.LastName;
            //txtSuffix.Text = oUserinfo.Suffix;
            //txtMName.Text = oUserinfo.MiddleName;
           
            txtFax.Text = oUserinfo.Fax;

            txtMobile.Text = oUserinfo.MobilePhone;
            txtPhone.Text = oUserinfo.Phone;
            drpState.Text = oUserinfo.State;

            txtAdd1.Text = oUserinfo.Address1;
            txtAdd2.Text = oUserinfo.Address2;
            txtAdd3.Text = oUserinfo.Address3;
            txtCity.Text = oUserinfo.City;
            txtAltEmail.Text = oUserinfo.AlternateEmail;
            txtZip.Text = oUserinfo.Zip;
            chkissubscribe.Checked = oUserinfo.Subscribe==1? true :false;
            drpCountry.SelectedValue = objUserServices.GetUserCountryCode(oUserinfo.Country);
            if (drpCountry.SelectedValue == "AU")
            {
                Setdrpdownlistvalue(ddlstate, oUserinfo.State.ToString());
                drpState.Visible = false;
                ddlstate.Visible = true;
                rfvddlstate.Enabled = true;
                RVtxtBillstate.Enabled = false; 
            }
            else
            {
                drpState.Visible = true;
                ddlstate.Visible = false;
                RVtxtBillstate.Enabled = true;
                rfvddlstate.Enabled = false;
            }
            
            //For Shipping Details
            LoadShippingInfo(Session["USER_ID"].ToString());
            //For Billing Details
            LoadBillInfo(Session["USER_ID"].ToString());


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void ClearShippingInfo()
    {
        txtshipadd1.Text = "";
        txtshipadd2.Text = "";
        txtshipadd3.Text = "";
        txtshipcity.Text = "";
        //txtshipcountry.Text = "";
        txtshipphone.Text = "";
        //txtshipstate.Text = "";
        txtshipzip.Text = "";
    }
    public void ClearBillingInfo()
    {
        txtbilladd1.Text = "";
        txtbilladd2.Text = "";
        txtbilladd3.Text = "";
        txtbillcity.Text = "";
        //txtbillcountry.Text = "";
        txtbillphone.Text = "";
        //txtbillstate.Text = "";
        txtbillzip.Text = "";
    }
    public void GetExistingShipAddr()
    {
        txtshipadd1.Text = "";
        txtshipadd2.Text = "";
        txtshipadd3.Text = "";
        txtshipcity.Text = "";
        //txtshipcountry.Text = "";
        txtshipphone.Text = "";
        //txtshipstate.Text = "";
        txtshipzip.Text = "";
    }
    public void GetExistingBillAddr()
    {
    }
    private void SendUpdateCustomer(int User_id)
    {
        try
        {
            string url = HttpContext.Current.Request.Url.Authority.ToString();
          //  string activeLink = string.Format("https://" + url + "/mActivation.aspx?id={0}", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(User_id.ToString())));
            objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
            string strstate = string.Empty;
            if (drpCountry.SelectedValue == "AU")
            {
                strstate = ddlstate.SelectedValue;
            }
            else
            {
                strstate = drpState.Text; 
            }
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());                 
            MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            MessageObj.To.Add(txtAltEmail.Text.ToString());
            string message = "";
            MessageObj.Subject = "WESonline-Form-Update Profile";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td>First Name</td><td>&nbsp;</td><td>" + txtFname.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Last Name</td><td>&nbsp;</td><td>" + txtLname.Text.ToString() + "</td></tr>";                        
            message = message + "<tr><td>Phone</td><td>&nbsp;</td><td>" + txtPhone.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Mobile/Cell Phone</td><td>&nbsp;</td><td>" + txtMobile.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Fax</td><td>&nbsp;</td><td>" + txtFax.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Email</td><td>&nbsp;</td><td>" + txtAltEmail.Text.ToString() + "</td></tr>";


            message = message + "<tr><td>Street Address</td><td>&nbsp;</td><td>" + txtAdd1.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Address Line2</td><td>&nbsp;</td><td>" + txtAdd2.Text.ToString() + " </td></tr>";
            message = message + "<tr><td>Suburp/Town</td><td>&nbsp;</td><td>" + txtCity.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>State/Province</td><td>&nbsp;</td><td>" + strstate + "</td></tr>";
            message = message + "<tr><td>Postcode/ZipCode</td><td>&nbsp;</td><td>" + txtZip.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Country</td><td>&nbsp;</td><td>" + drpCountry.SelectedItem.ToString() + "</td></tr>";            
           // MessageObj.Body = "<html><body><table>" + message + "</table>" + "</br> <font color=\"Red\">Activation Link :</font></br><font color=\"Blue\">" + activeLink + "</font></body></html>";

            MessageObj.Body = "<html><body><table>" + message + "</table></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            Response.Redirect("mConfirmMessage.aspx?Result=MESSAGESENT",false);
        }
        catch (System.Threading.ThreadAbortException)
        {
            // ignore it
        }
        catch (Exception)
        {            
            Response.Redirect("mConfirmMessage.aspx?Result=MESSAGENOTSENT",false);
        }
    }

    public int UpdateUserData()
    {
        try
        {
            oUserinfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
            //oUserinfo.Prefix = cmbPrefix.Text;   
            oUserinfo.COMPANY_NAME = txtcompname.Text; 
            oUserinfo.FirstName = txtFname.Text;
            oUserinfo.LastName = txtLname.Text;
            //oUserinfo.MiddleName = txtMName.Text;
            //oUserinfo.Suffix = txtSuffix.Text;
            oUserinfo.Address1 = txtAdd1.Text;
            oUserinfo.Address2 = txtAdd2.Text;
            oUserinfo.Address3 = txtAdd3.Text;
            oUserinfo.Email  = txtAltEmail.Text;
            oUserinfo.City = txtCity.Text;
            if (drpCountry.SelectedValue == "AU")
            {
                oUserinfo.State = ddlstate.SelectedValue;  
            }
            else
            {
                oUserinfo.State = drpState.Text;
            }
           // oUserinfo.Country = drpCountry.Text;
            oUserinfo.Country = drpCountry.SelectedItem.ToString();
            oUserinfo.Zip = txtZip.Text;
            oUserinfo.Fax = txtFax.Text;
            oUserinfo.Phone = txtPhone.Text;
            oUserinfo.MobilePhone = txtMobile.Text;
            //For Shipping Details
            oUserinfo.ShipAddress1 = txtshipadd1.Text;
            oUserinfo.ShipAddress2 = txtshipadd2.Text;
            oUserinfo.ShipAddress3 = txtshipadd3.Text;
            oUserinfo.ShipCity = txtshipcity.Text;
            if (drpShipCountry.SelectedValue == "AU")
            {
                oUserinfo.ShipState = ddlshipstate.SelectedValue; 
            }
            else
            {
                oUserinfo.ShipState = drpShipState.Text;
            }
            oUserinfo.ShipZip = txtshipzip.Text;
           // oUserinfo.ShipCountry = drpShipCountry.Text;
            oUserinfo.ShipCountry = drpShipCountry.SelectedItem.ToString();
            oUserinfo.ShipPhone = txtshipphone.Text;
            //For Billing Details
            oUserinfo.BillAddress1 = txtbilladd1.Text;
            oUserinfo.BillAddress2 = txtbilladd2.Text;
            oUserinfo.BillAddress3 = txtbilladd3.Text;
            oUserinfo.BillCity = txtbillcity.Text;
            if (drpBillCountry.SelectedValue == "AU")
            {
                oUserinfo.BillState = ddlbillstate.SelectedValue ;
            }
            else
            {
                oUserinfo.BillState = drpBillState.Text;
            }
            oUserinfo.BillZip = txtbillzip.Text;
            oUserinfo.BillCountry = drpBillCountry.SelectedItem.ToString();
            oUserinfo.BillPhone = txtbillphone.Text;

            objUserServices.SetUserSubscriber(oUserinfo.UserID.ToString(), (chkissubscribe.Checked == true ? 1 : 0));

          //  objActiveCampaignService.SetContact_Subscribe(txtAltEmail.Text.Trim().ToString(), txtFname.Text.Trim(), "", (chkissubscribe.Checked == true ? 1 : 2), false);

            try
            {
                if (chkissubscribe.Checked == false)
                {
                    objmcms.UnSubscribeChimpMember("", txtAltEmail.Text.Trim());
                }
                else
                {
                    //objmcms.CreateMailChimpMember("", txtAltEmail.Text.Trim().ToString(), txtFname.Text.Trim(), txtLname.Text.Trim(), txtAdd1.Text.Trim(), txtCity.Text.Trim(), oUserinfo.State, oUserinfo.Zip, oUserinfo.Country);
                    objmcms.ResubscribeMailChimpMember("", txtAltEmail.Text.Trim().ToString(), txtFname.Text.Trim(), txtLname.Text.Trim(), txtAdd1.Text.Trim(), txtCity.Text.Trim(), oUserinfo.State, oUserinfo.Zip, oUserinfo.Country);
                    //objmcms.ResubscribeMailChimpMember("", txtAltEmail.Text.Trim().ToString(), txtFname.Text.Trim(), txtLname.Text.Trim(), "", txtCity.Text.Trim(), oUserinfo.State, oUserinfo.Zip, oUserinfo.Country);
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }




            return objUserServices.UpdateUserInfo(oUserinfo);

              
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }
    }
    protected void ChkShippingAdd_CheckedChanged(object sender, EventArgs e)
    {
        tabcontrolprofileHiddenField.Value = "true";
        if (ChkShippingAdd.Checked == true)
        {
            txtshipadd1.Text = txtbilladd1.Text;
            txtshipadd2.Text = txtbilladd2.Text;
            txtshipadd3.Text = txtbilladd3.Text;
            txtshipcity.Text = txtbillcity.Text;
            if (drpBillCountry.SelectedValue == "AU")
            {
                ddlshipstate.SelectedValue = ddlbillstate.SelectedValue;
                drpShipState.Visible = false;
                ddlshipstate.Visible = true;
                RVshipddlstate.Enabled = true;
                RVshipddlstate.Enabled = false; 
            }
            else
            {
                drpShipState.Text = drpBillState.Text;
                drpShipState.Visible = true;
                ddlshipstate.Visible = false;
                RVshipddlstate.Enabled = false;
                RVshiptxtstate.Enabled = true;
            }
            drpShipCountry.Text = drpBillCountry.SelectedValue;
            txtshipphone.Text = txtbillphone.Text;
            txtshipzip.Text = txtbillzip.Text;
        }
        else
        {
            LoadShippingInfo(Session["USER_ID"].ToString());
        }
    }
    protected void ChkBillingAdd_CheckedChanged1(object sender, EventArgs e)
    {
        tabcontrolprofileHiddenField.Value = "true";
        if (ChkBillingAdd.Checked == true)
        {
            txtbilladd1.Text = txtAdd1.Text;
            txtbilladd2.Text = txtAdd2.Text;
            txtbilladd3.Text = txtAdd3.Text;
            txtbillcity.Text = txtCity.Text;
            if (drpCountry.SelectedValue == "AU")
            {
                ddlbillstate.SelectedValue = ddlstate.SelectedValue;
                drpBillState.Visible = false;
                ddlbillstate.Visible = true;
                RVddlBillstate.Enabled = true;
                RVtxtBillstate.Enabled = false; 
            }
            else
            {
                drpBillState.Text = drpState.Text;
                drpBillState.Visible = true;
                ddlbillstate.Visible = false;
                RVddlBillstate.Enabled = false;
                RVtxtBillstate.Enabled = true; 
            }
            drpBillCountry.Text = drpCountry.SelectedValue;
            txtbillphone.Text = txtPhone.Text;
            txtbillzip.Text = txtZip.Text;
        }
        else
        {
            LoadBillInfo(Session["USER_ID"].ToString());
        }
    }
    public void LoadCountryList()
    {
        try
        {
            DataSet oDs = new DataSet();
            oDs = objCountryServices.GetCountries();
            drpCountry.Items.Clear();
            drpCountry.DataSource = oDs.Tables[0];
            drpCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpCountry.DataBind();
            drpCountry.SelectedIndex = drpCountry.Items.IndexOf(new ListItem("Australia", "AU"));
            LoadStates(drpCountry.SelectedValue, ddlstate);
           
            oDs = new DataSet();
            oDs = objCountryServices.GetCountries();
            drpShipCountry.Items.Clear();
            drpShipCountry.DataSource = oDs;
            drpShipCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpShipCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpShipCountry.DataBind();
            drpShipCountry.SelectedIndex = drpCountry.Items.IndexOf(new ListItem("Australia", "AU"));
            LoadStates(drpShipCountry.SelectedValue, ddlshipstate);
           
            oDs = new DataSet();
            oDs = objCountryServices.GetCountries();
            drpBillCountry.Items.Clear();
            drpBillCountry.DataSource = oDs;
            drpBillCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpBillCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpBillCountry.DataBind();
            drpBillCountry.SelectedIndex = drpCountry.Items.IndexOf(new ListItem("Australia", "AU"));
            LoadStates(drpBillCountry.SelectedValue, ddlbillstate);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void LoadStates(String conCode,DropDownList ddlcommonstate)
    {
        try
        {
            DataSet oDs = new DataSet();
            oDs = objCountryServices.GetStates(conCode);
            ddlcommonstate.Items.Clear();
            ddlcommonstate.DataSource = oDs;
            ddlcommonstate.DataTextField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            ddlcommonstate.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            ddlcommonstate.DataBind();
            ddlcommonstate.Items.Insert(0, new ListItem("Select", ""));
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void ShipLoadStates(String conCode)
    {
        try
        {

            //DataSet oDs = new DataSet();
            //    oDs = oCountry.GetStates(conCode);
            //    drpShipState.Items.Clear();
            //    drpShipState.DataSource = oDs;
            //    drpShipState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
            //    drpShipState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            //   drpShipState.DataBind();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void BillLoadStates(String conCode)
    {
        try
        {
            //DataSet oDs = new DataSet();
            //oDs = oCountry.GetStates(conCode);
            //drpBillState.Items.Clear();
            //drpBillState.DataSource = oDs;
            //drpBillState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
            //drpBillState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            //drpBillState.DataBind();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        tabcontrolprofileHiddenField.Value = "true";
        if (drpCountry.SelectedValue == "AU")
        {
            ddlstate.Visible = true;
            LoadStates(drpCountry.SelectedValue,ddlstate);
            drpState.Text = ""; 
            ddlstate.Focus();
          drpState.Visible = false;
          rfvtxtstate.Enabled = false;
          rfvddlstate.Enabled = true;  
        }
        else
        {
            drpState.Text = ""; 
            drpState.Visible = true;
            ddlstate.Visible = false;
            rfvtxtstate.Enabled = true;
            rfvddlstate.Enabled = false;  
        }
    }

    protected void drpBillCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        tabcontrolprofileHiddenField.Value = "true";
        if (drpBillCountry.SelectedValue == "AU")
        {
            ddlbillstate.Visible = true;
            LoadStates(drpBillCountry.SelectedValue, ddlbillstate);
            drpBillState.Text = ""; 
            ddlbillstate.Focus();
            drpBillState.Visible = false;
            RVtxtBillstate.Enabled = false;
            RVddlBillstate.Enabled = true;
          
        }
        else
        {
            drpBillState.Text = ""; 
           drpBillState.Visible = true;
           ddlbillstate.Visible = false;
           RVtxtBillstate.Enabled = true;
           RVddlBillstate.Enabled = false;  
        }
    }

    protected void drpShipCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        tabcontrolprofileHiddenField.Value = "true";
        if (drpShipCountry.SelectedValue == "AU")
        {
            ddlshipstate.Visible = true;
            LoadStates(drpShipCountry.SelectedValue, ddlshipstate);
            drpShipState.Text = "";

            RVshiptxtstate.Enabled = false;
            RVshipddlstate.Enabled = true;
            ddlshipstate.Focus();
            drpShipState.Visible = false;
             

        }
        else
        {
            drpShipState.Text = "";
            drpShipState.Visible = true;
            ddlshipstate.Visible = false;
            RVshiptxtstate.Enabled = true;
            RVshipddlstate.Enabled = false;
           
        }
    }

    
    

    //public void GetSupplierdetail()
    //{
    //    if (strsupplierName == "")
    //    {
    //        TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("NEWPRODUCT", MicroSiteTemplate, objConnectionDB.ConnectionString);
    //        DataTable ttbl = tbwtMSEngine.GetSupplierDetail();
    //        if (ttbl != null && ttbl.Rows.Count > 0)
    //        {
    //            strsupplierName = ttbl.Rows[0]["CATEGORY_NAME"].ToString();
    //            strsupplierDesc = ttbl.Rows[0]["SHORT_DESC"].ToString();
    //            strsupplierId = ttbl.Rows[0]["CATEGORY_ID"].ToString();
    //            micrositeurl = "/" + objHelperServices.Cons_NewURl_bybrand("", strsupplierName, "mct.aspx", "") + "/mct/";
    //        }
    //    }



    //}


    public void timer1_Tick(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < 2; i++)
            {
                if (System.IO.File.Exists(Session["pdffile"].ToString()))
                {
                    Timer1.Enabled = false;

                    i = 2;
                    tick_count = 29;
                    //Session["tickcheck"] = tick_count;
                    //Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + Session["PdfFileName"].ToString() + " .pdf"));
                    //Response.ContentType = "application/pdf";
                    //Response.WriteFile(Session["pdffile"].ToString());
                    //Response.End();                
                }
                tick_count = tick_count + 1;
                Session["tickcheck"] = tick_count;
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }


    //public void timer1_Tick(object sender, EventArgs e)
    //{

    //    // message.Attributes["style"] = "display:none;";
    //    //  messagediv.Visible = true;
    //    if (tick_count == 0)
    //    {
    //        // messagediv.Visible = false;
    //        // Response.Write("time out " + tick_count + " ...........");
    //        tick_count = tick_count + 1;
    //        //timer1.Stop();
    //        //ShowProcessSignalMessage();

    //        if (Request["Key"] != null)
    //        {

    //            string _Key = Server.HtmlDecode(Request["Key"].ToString());
    //            string DecryptedValueString = oHelper.Decrypt(_Key, Session.SessionID);

    //            if (!string.IsNullOrEmpty(DecryptedValueString))
    //            {
    //                string[] PdfFileName = DecryptedValueString.Split('|');
    //                string pdffile = Server.MapPath(string.Format("Invoices/In{0}", PdfFileName[1] + ".pdf"));
    //                //System.Threading.Thread.Sleep(100000);
    //                if (System.IO.File.Exists(pdffile))
    //                {
    //                    ProcessDiv.Visible = false;
    //                    // messagediv.Visible = false;
    //                    Timer1.Enabled = false;

    //                    Session["tickcheck"] = tick_count.ToString();
    //                    // tick_count = 0;
    //                    //timer1.Stop();
    //                    //System.Threading.Thread.Sleep(20000);
    //                    //ShowProcessSignalMessage();
    //                    Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
    //                    Response.ContentType = "application/pdf";
    //                    Response.WriteFile(pdffile);
    //                    Response.End();

    //                }
    //                //else
    //                //{
    //                //   // messagediv.Visible = false;
    //                //    ProcessDiv.Visible = false;
    //                //    Timer1.Enabled = false;

    //                //    if (PdfFileName[0] != null && PdfFileName[0].ToString().Trim() != string.Empty)
    //                //    {
    //                //        int cStatus = 0;
    //                //        string cOrderNo = PdfFileName[0].ToString();
    //                //        cStatus = oOrder.SentSignalInvoiceNotification(cOrderNo);
    //                //        if (cStatus >= 0)
    //                //        {

    //                //            ShowPopUpSignalMessage();
    //                //        }
    //                //        else
    //                //        {

    //                //            ShowProcessSignalMessage();
    //                //        }

    //                //    }
    //                //}
    //            }

    //        }


    //    }
    //    else if (tick_count == 3)
    //    {
    //        tick_count = tick_count + 1;
    //        //messagediv.Visible = false;
    //        ProcessDiv.Visible = false;
    //        Timer1.Enabled = false;

    //        if (Request["Key"] != null)
    //        {

    //            string _Key = Server.HtmlDecode(Request["Key"].ToString());
    //            string DecryptedValueString = oHelper.Decrypt(_Key, Session.SessionID);

    //            if (!string.IsNullOrEmpty(DecryptedValueString))
    //            {
    //                string[] PdfFileName = DecryptedValueString.Split('|');
    //                string pdffile = Server.MapPath(string.Format("Invoices/In{0}", PdfFileName[1] + ".pdf"));
    //                //System.Threading.Thread.Sleep(100000);
    //                if (System.IO.File.Exists(pdffile))
    //                {
    //                    //messagediv.Visible = false;
    //                    ProcessDiv.Visible = false;
    //                    Timer1.Enabled = false;

    //                    Session["tickcheck"] = tick_count.ToString();
    //                    // tick_count = 0;
    //                    //timer1.Stop();
    //                    //System.Threading.Thread.Sleep(20000);
    //                    //ShowProcessSignalMessage();
    //                    Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
    //                    Response.ContentType = "application/pdf";
    //                    Response.WriteFile(pdffile);
    //                    Response.End();

    //                }
    //                //else
    //                //{
    //                //    //messagediv.Visible = false;
    //                //    ProcessDiv.Visible = false;
    //                //    Timer1.Enabled = false;

    //                //    if (PdfFileName[0] != null && PdfFileName[0].ToString().Trim() != string.Empty)
    //                //    {
    //                //        ShowProcessSignalMessage();
    //                //        //int cStatus = 0;
    //                //        //string cOrderNo = PdfFileName[0].ToString();
    //                //        //cStatus = oOrder.SentSignalInvoiceNotification(cOrderNo);
    //                //        //if (cStatus >= 0)
    //                //        //{

    //                //        //    ShowPopUpSignalMessage();
    //                //        //}
    //                //        //else
    //                //        //{

    //                //        //    ShowProcessSignalMessage();
    //                //        //}

    //                //    }
    //                //}
    //            }
    //            //else
    //            //{
    //            //   // messagediv.Visible = false;
    //            //    ProcessDiv.Visible = false;
    //            //    Timer1.Enabled = false;
    //            //    ShowProcessSignalMessage();
    //            //}
    //        }

    //    }

    //    else
    //    {
    //        // messagediv.Visible = false;
    //        ProcessDiv.Visible = false;
    //        Timer1.Enabled = false;

    //        tick_count = tick_count + 1;
    //        if (Request["Key"] != null)
    //        {

    //            string _Key = Server.HtmlDecode(Request["Key"].ToString());
    //            string DecryptedValueString = oHelper.Decrypt(_Key, Session.SessionID);

    //            if (!string.IsNullOrEmpty(DecryptedValueString))
    //            {
    //                string[] PdfFileName = DecryptedValueString.Split('|');
    //                string pdffile = Server.MapPath(string.Format("Invoices/In{0}", PdfFileName[1] + ".pdf"));
    //                //System.Threading.Thread.Sleep(100000);
    //                if (System.IO.File.Exists(pdffile))
    //                {
    //                    ProcessDiv.Visible = false;
    //                    //messagediv.Visible = false;
    //                    Timer1.Enabled = false;

    //                    Session["tickcheck"] = tick_count.ToString();
    //                    //tick_count = 0;
    //                    //timer1.Stop();
    //                    //System.Threading.Thread.Sleep(20000);
    //                    //ShowProcessSignalMessage();
    //                    Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
    //                    Response.ContentType = "application/pdf";
    //                    Response.WriteFile(pdffile);
    //                    Response.End();

    //                }

    //            }

    //        }

    //    }

    //}

    //public void invoiceprocessstart()
    //{
    //    if (Request["Key"] != null)
    //    {
    //        string _Key = Server.HtmlDecode(Request["Key"].ToString());
    //        string DecryptedValueString = oHelper.Decrypt(_Key, Session.SessionID);

    //        if (!string.IsNullOrEmpty(DecryptedValueString))
    //        {
    //            string[] PdfFileName = DecryptedValueString.Split('|');
    //            string pdffile = Server.MapPath(string.Format("Invoices/In{0}", PdfFileName[1] + ".pdf"));
    //            if (System.IO.File.Exists(pdffile))
    //            {
    //                System.Windows.Forms.DialogResult result1 = System.Windows.Forms.MessageBox.Show("Please wait for a moment. Invoice will be available shortly", "View Invoice", System.Windows.Forms.MessageBoxButtons.OKCancel);
    //                if (result1 == System.Windows.Forms.DialogResult.OK)
    //                {
    //                    Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + PdfFileName[1] + ".pdf"));
    //                    Response.ContentType = "application/pdf";
    //                    Response.WriteFile(pdffile);
    //                    Response.End();
    //                }
    //            }
    //            else
    //            {
    //                if (PdfFileName[0] != null && PdfFileName[0].ToString().Trim() != string.Empty)
    //                {
    //                    int cStatus = 0;
    //                    string cOrderNo = PdfFileName[0].ToString();
    //                    cStatus = oOrder.SentSignalInvoiceNotification(cOrderNo);
    //                    if (cStatus >= 0)
    //                    {
    //                        ShowPopUpSignalMessage();
    //                    }
    //                    else
    //                    {
    //                        ShowProcessSignalMessage();
    //                    }

    //                }
    //            }
    //        }
    //        else
    //        {
    //            ShowProcessSignalMessage();
    //        }
    //    }
    //}
    public void SendInvoiceNotification(string InvoiceNo)
    {
        //oHelper.SQLString = "exec stp_reqinvoice";
        //oHelper.ExecuteSQLQuery();
    }

    private void OrderHistory()
    {
        //Order History Display    
        //Connection oConStr = new Connection();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        string websiteid = ConfigurationManager.AppSettings["WEBSITEID"];
        ConnectionDB objConnectionDB = new ConnectionDB();
        SqlDataAdapter da = new SqlDataAdapter("STP_TBWC_PICK_GetOrderHistory", objConnectionDB.GetConnection());
        da.SelectCommand.Parameters.AddWithValue("@WEBSITE_ID", websiteid);
        if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
        {
            da.SelectCommand.Parameters.AddWithValue("@User_id", Userid);
        }

        da.SelectCommand.CommandType = CommandType.StoredProcedure;

        DataTable dt = new DataTable();
        da.Fill(dt);
        objConnectionDB.CloseConnection();
        //OrderHistoryGridView.DataSource = dt;
        //OrderHistoryGridView.DataBind();
    }

    private void UserList(int Userid)
    {
        //All Users Display in Drop down 
        //Connection oConStr = new Connection();
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));      
        //SqlDataAdapter Sqlda = new SqlDataAdapter("SELECT	distinct substring(TBCB.CONTACT,0,15)  [User], 'View Order' as [Submitted Order] FROM TBWC_ORDER AS TBO INNER JOIN TBWC_COMPANY_BUYERS AS TBCB ON TBO.USER_ID = TBCB.USER_ID and TBCB.CONTACT != '' ", oCon);        
        //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        ConnectionDB objConnectionDB = new ConnectionDB();
        SqlDataAdapter da = new SqlDataAdapter("STP_TBWC_PICK_GetUserList", objConnectionDB.GetConnection());
        da.SelectCommand.Parameters.AddWithValue("@Userid", Userid);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        DataTable Sqldt = new DataTable();
        da.Fill(Sqldt);
        objConnectionDB.CloseConnection();


        // if (!Page.IsPostBack)
        // {
        CreatedUserDropDownlist.DataSource = Sqldt;
        CreatedUserDropDownlist.DataTextField = Sqldt.Columns["User"].ColumnName.ToString();
        CreatedUserDropDownlist.DataValueField = Sqldt.Columns["User"].ColumnName.ToString();
        CreatedUserDropDownlist.DataBind();
        if ((Convert.ToInt32(Session["USER_ROLE"]) == 1 && Session["CUSTOMER_TYPE"].ToString() == "Retailer") || Session["CUSTOMER_TYPE"].ToString() != "Retailer")
            CreatedUserDropDownlist.Items.Insert(0, "All Users");

        //}
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        try
        {

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-GB", true);

            if (FromdateTextBox.Text.Trim() != "" && TodateTextBox.Text.Trim() != "")
            {
                DateTime _mFromDate = DateTime.Parse(FromdateTextBox.Text, culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                DateTime _mToDate = DateTime.Parse(TodateTextBox.Text, culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

                if (DateTime.Compare(_mFromDate, _mToDate) > 0)
                {
                    MsgLabel.Text = "To Date Should be Greater than From Date";
                    MsgLabel.Visible = true;
                }
                else
                {
                    MsgLabel.Text = "  .";
                    MsgLabel.Visible = false;
                }
            }

            //String InvoiceNo = (OrderNo.Text.Trim() == string.Empty ? null : OrderNo.Text);
            //String FromDate = (FromdateTextBox.Text.Trim() == string.Empty ? null : FromdateTextBox.Text);
            //String Todate = (TodateTextBox.Text.Trim() == string.Empty ? null : TodateTextBox.Text);
            //String Users = (CreatedUserDropDownlist.SelectedItem.Text.Trim() == string.Empty ? null : CreatedUserDropDownlist.SelectedItem.Text);

            //Order oOrder1 = new Order();
            //DataTable Dt = new DataTable();
            //Dt = oOrder1.GetFilteredOrderHistory(InvoiceNo, FromDate, Todate, Users);

            //if (Dt.Rows.Count > 0)
            //{
            //    MsgLabel.ForeColor = System.Drawing.Color.White;
            //    MsgLabel.BackColor = System.Drawing.Color.White;
            //    MsgLabel.Text = "   .";
            //    //MsgLabel.Text = "No of Records Available : " + Dt.Rows.Count;
            //}
            //else
            //{
            //    MsgLabel.ForeColor = System.Drawing.Color.Red;
            //    MsgLabel.Text = "No Records Found!";
            //}

            //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
            //SqlDataAdapter da = new SqlDataAdapter("GetOrderHistory_Search", oCon);
            //da.SelectCommand.CommandType = CommandType.StoredProcedure;
            ////da.SelectCommand.Parameters.Clear();
            //da.SelectCommand.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            //da.SelectCommand.Parameters.AddWithValue("@Users", Users);
            //da.SelectCommand.Parameters.AddWithValue("@Fromdate1", FromDate);
            //da.SelectCommand.Parameters.AddWithValue("@Todate1", Todate);

            //DataSet ds = new DataSet();
            //DataTable Dt = new DataTable();
            //da.Fill(ds);
            ////OrderHistoryGridView.DataSource = Dt;
            ////OrderHistoryGridView.DataBind();

            //if (Dt.Rows.Count == 0)
            //{
            //    MsgLabel.Text = "Record Not Found!";
            //}

        }
        catch (Exception ex)
        {
            MsgLabel.Text = ex.Message;
        }

    }



    protected void ResetButton_Click(object sender, EventArgs e)
    {
        OrderNoHiddenField.Value = "";
        FromDateHiddenField.Value = "";
        ToDateHiddenField.Value = "";
        CreatedUserDropDownlist.SelectedIndex = 0;
        UserHiddenField.Value = CreatedUserDropDownlist.SelectedItem.Text;
        MsgLabel.Text = "  .";
        MsgLabel.Visible = false;

        OrderNo.Text = "";
        FromdateTextBox.Text = "";
        TodateTextBox.Text = "";
        OrderHistory();
        //CreatedUserDropDownlist.Items.Insert(0, "All Users");

    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        btnClose_Click(sender, e);

    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        HidePopUpSignalMessage();
        Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + Session["PdfFileName"].ToString() + " .pdf"));
        Response.ContentType = "application/pdf";
        Response.WriteFile(Session["pdffile"].ToString());
        Response.End();
    }
    private void ShowPopUpSignalMessage()
    {

        modalPop.ID = "popUp";
        modalPop.PopupControlID = "SignalPopupPanel";
        modalPop.BackgroundCssClass = "modalBackground";
        modalPop.TargetControlID = "btnHidden";
        modalPop.DropShadow = false;
        modalPop.CancelControlID = "btnCancel";
        // this.SignalPopupPanel.Controls.Add(modalPop);

        this.modalPop.Show();
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        ShowProcessSignalMessage();
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        //Response.Redirect("Orderhistory.aspx");
        //this.modalPop.Hide();
    }
    private void ShowProcessSignalMessage()
    {
        modalPop.ID = "PopDiv1";
        modalPop.PopupControlID = "SignalProcessPanel";
        modalPop.BackgroundCssClass = "modalBackground";
        modalPop.TargetControlID = "btnHidden";
        modalPop.DropShadow = false;
        modalPop.CancelControlID = "ExitButton";

        // this.SignalPopupPanel.Controls.Add(modalPop);
        this.modalPop.Show();
    }
    private void ShowProcessSignalMessage1()
    {
        modalPop.ID = "PopDiv";
        modalPop.PopupControlID = "SignalPopupPanel";
        modalPop.BackgroundCssClass = "modalBackground";
        modalPop.TargetControlID = "btnHidden";
        modalPop.DropShadow = false;
        modalPop.CancelControlID = "ExitButton";
        //  this.SignalPopupPanel.Controls.Add(modalPop);
        this.modalPop.Show();
    }
    private void ShowProcessMessage()
    {
        //modalPop.TargetControlID = "timer";
        // modalPop.PopupControlID = "timer";
        this.modalPop.ID = "popUp";
        modalPop.ID = "popUp";
        modalPop.PopupControlID = "ShowProcessPanel";
        modalPop.BackgroundCssClass = "modalBackground";
        modalPop.TargetControlID = "btnHidden";
        modalPop.DropShadow = false;
        modalPop.CancelControlID = "ExitButton";

        // this.ShowProcessPanel.Controls.Add(modalPop);
        this.modalPop.Show();
        //this.message.Visible=true;
    }


    private void HidePopUpSignalMessage()
    {
        this.modalPop.Hide();
    }



    [WebMethod]
    public static string SendInvoiceSignal(string strvalue)
    {

        string DecryptedValueString = strvalue;
        string invno = "-1";
        OrderServices objOrderServices = new OrderServices();
        HelperServices objHelperServices = new HelperServices();
        if (!string.IsNullOrEmpty(DecryptedValueString))
        {
            string[] PdfFileName = DecryptedValueString.Split('|');
            string pdffile = HttpContext.Current.Server.MapPath(string.Format("~/Invoices/In{0}", PdfFileName[1] + ".pdf"));
            invno = PdfFileName[1].ToString();
            HttpContext.Current.Session["PdfFileName"] = PdfFileName[1].ToString();
            HttpContext.Current.Session["pdffile"] = pdffile;
            HttpContext.Current.Session["invno"] = PdfFileName[1].ToString();


            string INVOICE_NO_OF_TIME_TRY = System.Configuration.ConfigurationManager.AppSettings["INVOICE_NO_OF_TIME_TRY"];
            string INVOICE_WAIT_TIME = System.Configuration.ConfigurationManager.AppSettings["INVOICE_WAIT_TIME"];

            //System.Threading.Thread.Sleep(100000);
            if (System.IO.File.Exists(pdffile))
            {
                return "inv" + invno;
            }
            else
            {
                if (PdfFileName[0] != null && PdfFileName[0].ToString().Trim() != string.Empty)
                {
                    int cStatus = 0;
                    int invTry = objHelperServices.CI(INVOICE_NO_OF_TIME_TRY);
                    int invWaitTime = objHelperServices.CI(INVOICE_WAIT_TIME);
                    string cOrderNo = PdfFileName[0].ToString();
                    //cStatus = objOrderServices.SentSignalInvoiceNotification(cOrderNo);
                    if (objOrderServices.GetOrderStatus(objHelperServices.CI(cOrderNo)).ToLower() == (Enum.GetName(typeof(OrderServices.OrderStatus), OrderServices.OrderStatus.Proforma_Payment_Required)).ToLower())
                        cStatus = objOrderServices.SentSignal("0", cOrderNo, "201");
                    else
                        cStatus = objOrderServices.SentSignal("0", cOrderNo, "200");

                    if (cStatus >= 0)
                    {

                        for (int i = 0; i < invTry; i++)
                        {
                            if (System.IO.File.Exists(HttpContext.Current.Session["pdffile"].ToString()))
                            {



                                return "inv" + invno;
                            }
                            System.Threading.Thread.Sleep(invWaitTime);
                        }
                    }
                    else
                    {
                        HttpContext.Current.Session["pdffile"] = "";
                        HttpContext.Current.Session["PdfFileName"] = "";

                        return invno;
                    }

                }
            }
        }
        else
        {
            HttpContext.Current.Session["pdffile"] = "";
            HttpContext.Current.Session["PdfFileName"] = "";

            return invno;
        }
        HttpContext.Current.Session["pdffile"] = "";
        HttpContext.Current.Session["PdfFileName"] = "";
        return invno;


    }


    protected void cmdUpdateField_Click(object sender, EventArgs e)
    {

        try
        {
            if (HttpContext.Current.Session["PdfFileName"] != null && HttpContext.Current.Session["pdffile"] != "" && HttpContext.Current.Session["PdfFileName"].ToString() != "")
            {
                HttpContext.Current.Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", "In" + HttpContext.Current.Session["PdfFileName"].ToString() + " .pdf"));
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(HttpContext.Current.Session["pdffile"].ToString());
                HttpContext.Current.Response.End();
            }
            else
            {
                ShowProcessSignalMessage();
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected string EncryptSP(string ordid)
    {
        string enc = "";
        enc = objSecurity.StringEnCrypt(ordid, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        return HttpUtility.UrlEncode(enc);
    }
    }
}