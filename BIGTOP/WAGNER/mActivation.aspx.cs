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
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class mActivation : System.Web.UI.Page
{
    #region "Declarations"
   // int OrderId = 0;
    HelperServices objHelperServices = new HelperServices();
    
    ErrorHandler objErrorHandler = new ErrorHandler();
    UserServices objUserServices = new UserServices();
    Security objSecurity = new Security();
    ActiveCampaignService objActiveCampaignService = new ActiveCampaignService();
    UserServices.UserInfo objuserinfo = new UserServices.UserInfo();
    
    
    
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string reg_id = "";
            int i=0;
            if( Request["id"]!=null)
            {
                Page.MetaDescription = "Registerd User Activation";
                reg_id = Request["id"].ToString();
                reg_id= objSecurity.StringDeCrypt(reg_id);
                i = objUserServices.SetUserRole(reg_id, 2);
                objuserinfo = objUserServices.GetUserInfo(Convert.ToInt32(reg_id) );
                if (i > 0)
                {                   
                    objActiveCampaignService.SetContact_Subscribe(objuserinfo.AlternateEmail, objuserinfo.FirstName, "", (objuserinfo.Subscribe == 1? 1:2) , false);  

                    Session["USER_ROLE"] = objUserServices.GetRole(Convert.ToInt32(reg_id));
                    Response.Redirect("mConfirmMessage.aspx?Result=ACTIVATED");
                }
                else
                    Response.Redirect("mConfirmMessage.aspx?Result=ACTIVATION_FAILED" );
            }
            if (Request["ac_id"] != null)
            {
                Page.MetaDescription = "Subscribe Activation";
                reg_id = Request["ac_id"].ToString();
                DataSet ds = null;
               ds= objActiveCampaignService.SetContact_Subscribe_active(Convert.ToInt32(reg_id),1);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["result_code"]) > 0)
                     Response.Redirect("mConfirmMessage.aspx?Result=SUBSCRIBE_ACTIVATED" );
                else
                    Response.Redirect("mConfirmMessage.aspx?Result=SUBSCRIBE_ACTIVATION_FAILED");
                }
                else
                 Response.Redirect("mConfirmMessage.aspx?Result=SUBSCRIBE_ACTIVATION_FAILED");

                                
            }
            Page.Title = "Bigtop-Activation";
            Page.MetaKeywords = "Activation,Wagner Australia";
            
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
