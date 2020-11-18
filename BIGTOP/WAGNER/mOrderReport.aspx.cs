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
using TradingBell.WebCat.CommonServices;

namespace WES
{
    public partial class mOrderReport : System.Web.UI.Page
    {
        HelperServices objHelperServices = new HelperServices();
        OrderServices objOrderServices = new OrderServices();
        UserServices objUserServices = new UserServices();
        DataSet ds = new DataSet();
        ErrorHandler objErrorHandler = new ErrorHandler();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                imgPrint.Src = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/micrositeimages/Print.png";
                imgLogo.Src = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/micrositeimages/wagner_logo.png";

                if (!IsPostBack)
                {


                    if (Request["ViewType"] == "Confirm")
                    {
                        decimal CCTotal = objOrderServices.GetOrderTotalCost(objHelperServices.CI(Request["OrdId"]));
                        string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                    }
                    else
                    {
                        //lblBill.Visible = false;
                        // lblCheck.Visible = false;
                        //lblConfirmMsg1.Visible = false;
                        //   lblConfirmMsg.Visible = false;
                        //  lblReviewOrder.Visible = false;
                        //  lblShip.Visible = false;
                        // lblShoppingCart.Visible = false;
                        // lblPageHead.Visible = true;
                    }
                }
                if (Session["USER_NAME"] == null)
                {
                    Session["USER"] = "";
                    Session["COUNT"] = "0";
                    Response.Redirect("/mLogin.aspx");
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