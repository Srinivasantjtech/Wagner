using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using System.Data;
namespace WES.App_Code.CL
{
    [System.ComponentModel.DataObject]
    public class StaticCache
    {
        public static void LoadStaticCache()
       {
            // Get suppliers - cache using application state
             EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();

            HttpContext.Current.Application["key_AttributeId"] = EasyAsk.GetCategoryAndBrand_Applicationstart("AttributeIDJson");
            HttpContext.Current.Application["key_AttributeValue"] = EasyAsk.GetCategoryAndBrand_Applicationstart("AttributeValueJson");
         
            HttpContext.Current.Application["key_PTag"] = EasyAsk.GetCategoryAndBrand_Applicationstart("ProductTagJson");
           
            HttpContext.Current.Application["key_MainCategory"] = EasyAsk.GetCategoryAndBrand_Applicationstart("MainCategory");
           
            HttpContext.Current.Application["key_SubCategory"] = EasyAsk.GetCategoryAndBrand_Applicationstart("SubCategory");
            HttpContext.Current.Application["Key_Model"] = EasyAsk.GetCategoryAndBrand_Applicationstart("Models");
            HttpContext.Current.Application["Key_Brand"] = EasyAsk.GetCategoryAndBrand_Applicationstart("Brands");

            HttpContext.Current.Application["All_Category"] = EasyAsk.GetCategoryAndBrand_Applicationstart("AllCategory");

     //  HttpContext.Current.Application["key_SubCategoryAll"] = EasyAsk.GetCategoryAndBrand_Applicationstart("SubCategoryAll");
            DataSet key_SubCategoryAll = EasyAsk.GetCategoryAndBrand_Applicationstart("SubCategoryAll");
            HttpRuntime.Cache.Insert(
                "key_SubCategoryAll",
              key_SubCategoryAll,
              null,
            Cache.NoAbsoluteExpiration,
             Cache.NoSlidingExpiration,
             CacheItemPriority.NotRemovable,
             null);
            DataSet Cache_NEWPRODUCT = EasyAsk.GetCategoryAndBrand_Applicationstart("New Products");
            HttpRuntime.Cache.Insert(
                          "Cache_NEWPRODUCT",
                      Cache_NEWPRODUCT,
                   null,
              Cache.NoAbsoluteExpiration,
               Cache.NoSlidingExpiration,
                        CacheItemPriority.NotRemovable,
               null);
            DataSet Cache_POPULARPRODUCT = EasyAsk.GetCategoryAndBrand_Applicationstart("PopularProducts"); ;
            HttpRuntime.Cache.Insert(
                       "Cache_POPULARPRODUCT",
                        Cache_POPULARPRODUCT,
                null,
              Cache.NoAbsoluteExpiration,
              Cache.NoSlidingExpiration,
                      CacheItemPriority.NotRemovable,
               null);
            DataSet Cache_HOMEPRODUCT = EasyAsk.GetCategoryAndBrand_Applicationstart("Home Products"); ;
            HttpRuntime.Cache.Insert(
                       "Cache_HOMEPRODUCT",
                        Cache_HOMEPRODUCT,
                null,
              Cache.NoAbsoluteExpiration,
              Cache.NoSlidingExpiration,
                      CacheItemPriority.NotRemovable,
               null);
            //DataSet Cache_POPULARSEARCH = EasyAsk.GetCategoryAndBrand_Applicationstart("PopularSearch"); ;
            //HttpRuntime.Cache.Insert(
            //           "Cache_POPULARSEARCH",
            //            Cache_POPULARSEARCH,
            //    null,
            //  Cache.NoAbsoluteExpiration,
            //  Cache.NoSlidingExpiration,
            //          CacheItemPriority.NotRemovable,
            //   null);
            // HttpContext.Current.Application["key_Mainds_Sort"] = EasyAsk.GetCategoryAndBrand_Applicationstart("Mainds_Sort");
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
            tbwtEngine = new TBWTemplateEngine("TOPLOG", HttpContext.Current.Server.MapPath("~\\Templates"), "");

            string html_top_home = "before bind";
            if(tbwtEngine.ST_Top_Load_cache(true)!="")
            {
                html_top_home = tbwtEngine.ST_Top_Load_cache(true); 
            }

             HttpRuntime.Cache.Insert(
                   "Cache_Top_Home",
                 html_top_home,
                 null,
               Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable,
                null);

             tbwtEngine = new TBWTemplateEngine("TOPLOG", HttpContext.Current.Server.MapPath("~\\Templates"), "");

             string html_top = string.Empty;
             if (tbwtEngine.ST_Top_Load_cache(false) != "")
             {
                 html_top = tbwtEngine.ST_Top_Load_cache(false);
             }

             HttpRuntime.Cache.Insert(
                   "Cache_Top",
                 html_top,
                 null,
               Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable,
                null);
            

            //tbwtEngine = new TBWTemplateEngine("NEWPRODUCT", HttpContext.Current.Server.MapPath("~\\Templates"), "");
            //string html_NEWPRODUCT = tbwtEngine.ST_NewProduct_Load(); ;
            //HttpRuntime.Cache.Insert("Cache_NEWPRODUCT",
            //            html_NEWPRODUCT,
            //         null,
            //    Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(60));


          
            //tbwtEngine = new TBWTemplateEngine("PopularProduct", HttpContext.Current.Server.MapPath("~\\Templates"), "");
            //string html_POPULARPRODUCT = tbwtEngine.ST_POPULAR_PRODUCT(); ;

            //HttpRuntime.Cache.Insert(
            //              "Cache_POPULARPRODUCT",
            //               html_POPULARPRODUCT,
            //       null,
            //     Cache.NoAbsoluteExpiration,
            //     Cache.NoSlidingExpiration,
            //             CacheItemPriority.NotRemovable,
            //      null);

       //     HttpContext.Current.Application["key_NewProducts"] = 
          //  HttpContext.Current.Application["key_PopularProducts"] = 


             tbwtEngine = new TBWTemplateEngine("BOTTOM", HttpContext.Current.Server.MapPath("~\\Templates"), "");
          string html=tbwtEngine.ST_Bottom_Load();
          HttpRuntime.Cache.Insert( "Cache_bottom", html, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
          CacheItemPriority.NotRemovable, null);
            DataSet PostLogin = EasyAsk.LoadLinkTXT("postlogin");
            HttpRuntime.Cache.Insert(
                          "Cache_PostLogin",
                           PostLogin,
                   null,
                 Cache.NoAbsoluteExpiration,
                 Cache.NoSlidingExpiration,
                         CacheItemPriority.NotRemovable,
                  null);
            //DataSet HomeProducts = EasyAsk.LoadHomeProducts();
            //HttpRuntime.Cache.Insert(
            //              "Cache_HomeProducts",
            //               HomeProducts,
            //       null,
            //     Cache.NoAbsoluteExpiration,
            //     Cache.NoSlidingExpiration,
            //             CacheItemPriority.NotRemovable,
            //      null);


            //DataSet AutoSearch = EasyAsk.GetCategoryAndBrand_Applicationstart("AutoSearch");
            //HttpRuntime.Cache.Insert("Cache_AutoSearch", AutoSearch, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
            //CacheItemPriority.NotRemovable, null);

        }
    }
}