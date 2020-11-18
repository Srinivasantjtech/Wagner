using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_byproduct : System.Web.UI.UserControl
{
    ConnectionDB conStr = new ConnectionDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    //public string Bread_Crumbs()
    //{
    //    string breadcrumb = "", paraPID = "", paraFID = "", paraCID = "";
    //    if (Request.QueryString["pid"] != null)
    //    {
    //        paraPID = Request.QueryString["pid"].ToString();
    //    }
    //    if (Request.QueryString["fid"] != null)
    //        paraFID = Request.QueryString["fid"].ToString();
    //    if (Request.QueryString["cid"] != null)
    //        paraCID = Request.QueryString["cid"].ToString();


    //    if (paraPID != "")
    //    {
    //        DataSet DSBC = null;

    //        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
    //        foreach (DataRow DR in DSBC.Tables[0].Rows)
    //        {
    //            breadcrumb = "<a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
    //        }
    //        if (paraFID != "")
    //        {
    //            string catIDtemp = "";
    //            DSBC = GetDataSet("SELECT family_name,category_id FROM TB_family WHERE family_ID = " + paraFID);
    //            foreach (DataRow DR in DSBC.Tables[0].Rows)
    //            {
    //                breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString() + "</a> / " + breadcrumb;
    //                catIDtemp = DR[1].ToString();
    //            }
    //            do
    //            {
    //                DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
    //                foreach (DataRow DR in DSBC.Tables[0].Rows)
    //                {
    //                    if (DR["PARENT_CATEGORY"].ToString() != "0")
    //                    {
    //                        breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> / " + breadcrumb;
    //                    }
    //                    else
    //                    {
    //                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> / " + breadcrumb;
    //                    }
    //                    catIDtemp = DR["PARENT_CATEGORY"].ToString();
    //                }
    //            } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
    //        }
    //    }
    //    else if (paraFID != "")
    //    {
    //        DataSet DSBC = null;
    //        string catIDtemp = "";
    //        DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
    //        foreach (DataRow DR in DSBC.Tables[0].Rows)
    //        {
    //            breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
    //            catIDtemp = DR[1].ToString();
    //        }
    //        do
    //        {
    //            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
    //            foreach (DataRow DR in DSBC.Tables[0].Rows)
    //            {
    //                if (DR["PARENT_CATEGORY"].ToString() != "0")
    //                {
    //                    if (breadcrumb == "")
    //                        breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
    //                    else
    //                        breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> / " + breadcrumb;
    //                }
    //                else
    //                {
    //                    if (breadcrumb == "")
    //                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
    //                    else
    //                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> / " + breadcrumb;

    //                }
    //                catIDtemp = DR["PARENT_CATEGORY"].ToString();
    //            }
    //        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
    //    }
    //    else if (paraCID != "")
    //    {
    //        DataSet DSBC = null;
    //        string catIDtemp = paraCID;
    //        do
    //        {
    //            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
    //            foreach (DataRow DR in DSBC.Tables[0].Rows)
    //            {
    //                if (DR["PARENT_CATEGORY"].ToString() != "0")
    //                {
    //                    if (breadcrumb == "")
    //                        breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
    //                    else
    //                        breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> / " + breadcrumb;
    //                }
    //                else
    //                {
    //                    if (breadcrumb == "")
    //                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
    //                    else
    //                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> / " + breadcrumb;

    //                }
    //                catIDtemp = DR["PARENT_CATEGORY"].ToString();
    //            }
    //        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
    //    }
    //    return breadcrumb;
    //}

    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(';') + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}
}
