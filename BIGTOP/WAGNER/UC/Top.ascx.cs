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
using System.Data.SqlClient;

using TradingBell5.CatalogX;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

using System.Web.Services;
using System.Web.Configuration;

public partial class UC_Top : System.Web.UI.UserControl
{
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    UserServices objUserServices = new UserServices();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ST_top()
    {
        try
        {
        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("TOP", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);        
        tbwtEngine.cartitem = cartcount();
        //tbwtEngine.RenderHTML("Row");
        //return (tbwtEngine.RenderedHTML);
        return tbwtEngine.ST_Top_Load(); 
        }


        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }

    public string cartcount()
    {
        try
        {
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler oErr = new ErrorHandler();
        OrderServices objOrderServices = new OrderServices();
        string cartitem = "0";
        int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
        if (Session["USER_ID"] != null && !Session["USER_ID"].ToString().Equals(""))
        {

            int OrderID = 0;

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);
            }

            string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
            if (OrderID > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())    // || OrderStatus=="CAU_PENDING")
            {
                if (objOrderServices.GetOrderItemCount(OrderID) == 0)
                    cartitem = "0";
                else
                    cartitem = objOrderServices.GetOrderItemCount(OrderID).ToString();

            }
            else
            {
                cartitem = "0";
            }
        }
        return cartitem;
        }


        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty ;
        }
    }

    protected void logoutsession(object sender, EventArgs e)
    {
        objUserServices.OnLineFlag(false, objHelperServices.CI(Session["USER_ID"]));
        Session.RemoveAll();
        Session.Clear();
        Session.Abandon();
        Session["USER_ID"] = "";
        HttpCookie LoginInfoCookie = Request.Cookies["BigtopLoginInfo"];
        if (LoginInfoCookie != null && LoginInfoCookie["Password"] != null)
            LoginInfoCookie["Password"] = "";

        Response.Cookies["BigtopLoginInfo"].Expires = DateTime.Now.AddDays(-665);
        Response.Redirect("/Login.aspx",false);
    }

    }
