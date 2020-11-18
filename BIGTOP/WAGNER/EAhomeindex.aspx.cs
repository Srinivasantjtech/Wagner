﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.EasyAsk;
using System.Data;
using System.Configuration;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using Newtonsoft.Json;
namespace WES
{
    public partial class EAhomeindex : System.Web.UI.Page
    {
        EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
        ErrorHandler objErrorhandler = new ErrorHandler();
        private HelperDB objHelperDB = new HelperDB();
        public string strxml = HttpContext.Current.Server.MapPath("xml");
        protected void Page_Load(object sender, EventArgs e)
        {
            try 
            {
                EasyAsk.CreateCategory_JSON();
                EasyAsk.CreateProductTag_Json();
                EasyAsk.CreateAttributeID_JSON();
                EasyAsk.CreateAttributeDetails_JSON();
                DataSet main = EasyAsk.GetCategoryAndBrand("", true);

                if (main != null && main.Tables.Count > 0 && main.Tables[0].Rows.Count > 0)
                {
                    //foreach (DataRow dr in main.Tables[0].Rows)
                    for (int i = 0; i <= main.Tables[0].Rows.Count - 1; i++)
                    {
                        EasyAsk.GetMainMenuClickDetailJson(main.Tables[0].Rows[i]["CATEGORY_ID"].ToString(), "", false);

                    }
                }
                EasyAsk.Create_NewProducts_Json();
                EasyAsk.Create_PopularProducts_Json();
                EasyAsk.Create_HomeProducts_Json();
               // EasyAsk.Create_PRICE_EA_Json();
            }
            catch (Exception ex)
            {
                objErrorhandler.CreateLog(ex.ToString());
            
            }



        }
       
     
    }
}