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

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
public partial class sitemap : System.Web.UI.Page
{
    Security objSecurity=new Security();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        HelperServices objHelperServices = new HelperServices();
        //Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Page.Title = "Sitemap | Bigtop"; 

        HelperDB objHelperDB =  new HelperDB();
        DataTable dt=new DataTable();
        //string ea="";
       // string eaorg="";
       // int id=0;
       // string sql="";
        int i=0;
        //dt = objHelperDB.GetDataTableDB("Select Id,eapath from TBWC_URLRW_EA");
        //    if (dt!=null && dt.Rows.Count>0) 
        //    {
        //        for( i=0;i<=dt.Rows.Count-1 ;i++)  
        //        {
        //            id=Convert.ToInt32(dt.Rows[i]["id"]);
        //            eaorg=dt.Rows[i]["eapath"].ToString();
        //            ea=GetEAPath(eaorg);
        //            sql="Insert into TBWC_URLRW_EA_INSERT(id,eapathOrg,eapath) vlaues("+ id+",'"+eaorg+"','"+ea+"')";
        //            objHelperDB.GetDataTableDB(sql);
        //        }
        //    }

    }

    private string GetEAPath(string eapath)
    {
            string[] StrValues = null;
            string EA="";
            string temp="";
            string[] tmpsplit;
        string  rtnvlaue="";
        string TempPath="";

            EA=objSecurity.StringDeCrypt(eapath,"Cellink@123@dm1n");
            if (EA == "" || EA == null)
                EA = objSecurity.StringDeCrypt(HttpUtility.UrlDecode (eapath), "Cellink@123@dm1n");
    

            if (EA != "" && EA!=null)
            {
                StrValues = EA.Split(new string[] { "////" }, StringSplitOptions.None);
                if (StrValues.Length > 0)
                {
                    for (int i = 2; i < StrValues.Length; i++)
                    {



                        temp = "";
                        for (int j = 0; j <= i; j++)
                        {
                            if (j == 0)
                            {
                                temp = temp + StrValues[j];
                            }
                            else
                            {
                                temp = temp + "////" + StrValues[j];
                            }
                        }

                        if (StrValues[i].ToUpper().Contains("ATTRIBSEL"))
                        {
                            tmpsplit = StrValues[i].Split('=');
                            if (tmpsplit.Length >= 2)
                            {

                                TempPath = TempPath + "/" + tmpsplit[1].Trim();
                            }
                        }
                        else if (StrValues[i].ToUpper().Contains("SEARCH"))
                        {
                            if (StrValues[i].ToUpper().Contains("FAMILY"))
                            {
                                tmpsplit = StrValues[i].Split('=');
                                if (tmpsplit.Length >= 2)
                                {
                                    TempPath = TempPath + "/" + "Family";

                                }
                            }
                            else if (StrValues[i].ToUpper().Contains("PROD"))
                            {
                                tmpsplit = StrValues[i].Split('=');
                                if (tmpsplit.Length >= 2)
                                {

                                    TempPath = TempPath + "/" + "Product";
                                }
                            }
                            else
                            {
                                tmpsplit = StrValues[i].Split('=');
                                if (tmpsplit.Length >= 1)
                                {

                                    TempPath = TempPath + "/" + tmpsplit[0].Trim();
                                }
                            }
                        }
                        else
                        {

                            TempPath = TempPath + "/" + "Category";

                        }


                    }

                }
            }
            rtnvlaue = TempPath;
            return rtnvlaue;

    }
    

    
}
 