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
public partial class GetDeal : System.Web.UI.Page
{
    UserServices objUserServices = new UserServices();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    ActiveCampaignService objActiveCampaignService = new ActiveCampaignService();
    string mail = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
       //        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Page.Title = "Wagner-GetDeal";
        HtmlMeta meta = new HtmlMeta();
        meta.ID = "1";
        meta.Name = "keywords";
        meta.Content = objHelperServices.GetOptionValues("Meta keyword").ToString();
        //this.Header.Controls.Add(meta);

        // Render: <meta name="Description" content="noindex" />
        meta = new HtmlMeta();
        meta.ID = "2";
        meta.Name = "Description";
        meta.Content = objHelperServices.GetOptionValues("Meta Description").ToString();
      // this.Header.Controls.Add(meta);

        // Render: <meta name="Abstraction" content="Some words listed here" />

        meta.Name = "Abstraction";
        meta.Content = objHelperServices.GetOptionValues("Meta Abstraction").ToString();
        //this.Header.Controls.Add(meta);
        meta.ID = "3";
        // Render: <meta name="Distribution" content="noindex" />
        meta = new HtmlMeta();
        meta.Name = "Distribution";
        meta.Content = objHelperServices.GetOptionValues("Meta Distribution").ToString();
       // this.Header.Controls.Add(meta);
        try
        {
            if (!IsPostBack)
            {
                if (Request["mail"] != null)
                {

                    mail = Request["mail"].ToString();
                    txtmail.Text = mail;
                }
            }

            lblError.Text = "";
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        txtmail.Attributes.Add("onkeypress", "javascript:return Email(event);");
        //txtmail.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
        txtFName.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
        txtLName.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
    }
    #region "Control events"
    protected void btnSubscribe_Click(object sender, EventArgs e)
    {
        MailChimpManagerServices ObjMailChimService = new MailChimpManagerServices();
        try
        {
            //DataSet ds = null;
            //lblError.Text = "";
            //string rtnstr = "";
            //rtnstr=objActiveCampaignService.GetContact_Exists(txtmail.Text);
            //if (rtnstr == "")
            //{
            //    ds = objActiveCampaignService.SetContact_Subscribe(txtmail.Text, txtFName.Text, txtLName.Text, 0, true);
            //    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            //    {
            //        if (Convert.ToInt32(ds.Tables[0].Rows[0]["result_code"]) > 0)
            //            Response.Redirect("ConfirmMessage.aspx?Result=GETDEAL", true);
            //        else
            //            lblError.Text = ds.Tables[0].Rows[0]["result_message"].ToString();
            //    }
            //    else
            //        lblError.Text = "Subscribe Failed ,try again";
            //}
            //else
            //    lblError.Text = rtnstr;

            string rtnstr = "";
            rtnstr=ObjMailChimService.GetCheckList(txtmail.Text);
            if (rtnstr == "")
            {

                ObjMailChimService.CreateMailChimpMemberDeal("", txtmail.Text, txtFName.Text, txtLName.Text, "", "", "", "", "");
                Response.Redirect("ConfirmMessage.aspx?Result=GETDEAL", true);
            }
            else
            {
                lblError.Text = "Subscribe Email Address Already Exist.";
            }

        }
        catch (Exception ex)
        {
            lblError.Text = "Subscribe Failed ,try again";
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    #endregion
}
