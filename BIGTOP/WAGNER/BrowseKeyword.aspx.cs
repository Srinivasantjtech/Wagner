using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WES
{
    public partial class BrowseKeyword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Bigtop";
            //Page.MetaKeywords = "AboutUs,Wagner Australia";
            Page.MetaKeywords = "Bigtop, Wagneronline.com.au";
            Page.MetaDescription = "Wagner Australia is a supplier of quality Mobile Phone, Data, Media and Navigation Accessories. Our extensive range includes Parts and accessories to suit a vast range mobile phones, personal multimedia equipment and other devices, Wagner electronics, Wagneronline.com.au";
        }

        //[System.Web.Services.WebMethod]
        //public static string DynamicPag(int ipageno)
        //{
        //    try
        //    {





        //        WES.UC.BrowseKeyword objnew = new WES.UC.BrowseKeyword();
        //            System.Text.StringBuilder getPostsText = new System.Text.StringBuilder();


        //            getPostsText.Append(objnew.Dynamic_St_BrowseKeyword(ipageno));

        //            //

        //            return getPostsText.ToString();







        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }




        //}
    }
}