<%@ Application Language="C#" %>
<%@ Import Namespace="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace="TradingBell.WebCat.CommonServices" %>
<%@ Import Namespace="TradingBell.WebCat.Helpers" %>

<%@ Import Namespace="System.Windows.Forms" %>
<%@ Import Namespace="System.Data" %>
<script RunAt="server">
       
    public System.Data.SqlClient.SqlConnection Gcon = new System.Data.SqlClient.SqlConnection();

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Application["WebsiteID"] = 7;
        // RegisterBundles(BundleTable.Bundles);
          WES.App_Code.CL.StaticCache.LoadStaticCache();
    }

    

   
    void Session_Start(object sender, EventArgs e)
    {
        Session["USER_ID"] = "";
    }

    void Session_End(object sender, EventArgs e)
    {
        if (Session["pdffile"] != null && Session["pdffile"].ToString() != string.Empty)
        {
            //string filename = Session["PdfFileName"].ToString();
            System.IO.FileInfo filin = new System.IO.FileInfo(Session["pdffile"].ToString());
            filin.Delete();
            
        }
        HelperServices objHelperServices = new HelperServices();
        if (objHelperServices.CI(Session["USER_ID"]) > 0)
        {
            // Set the User Online Flag to false
            UserServices objUserServices = new UserServices();
            objUserServices.OnLineFlag(false, objHelperServices.CI(Session["USER_ID"]));

        }
        //System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>();
        //IDictionaryEnumerator enumerator = System.Web.Caching.Cache.GetEnumerator();
        //while (enumerator.MoveNext())
        //{
        //    keys.Add(enumerator.Key.ToString());
        //}
        //for (int i = 0; i < keys.Count; i++)
        //{

        //    Cache.Remove(keys[i]);
        //}



    }

    // protected void Application_BeginRequest(object sender, EventArgs e)
    //  {


    //string url = Request.Url.ToString().Replace(Request.Url.Authority, "").Replace(Request.Url.Scheme, "").Replace("://","");


    //if (url.ToLower().Contains("/pay/do.dll/") && url.ToLower().Contains("payonlinecc.aspx"))
    //{

    //    url = url.Replace("/pay/do.dll/PayOnlineCC.aspx", "/PayOnlineCC.aspx?");            
    //    Context.RewritePath(url);
    //}
    //else if (url.ToLower().Contains("/pay/do.dll/") && url.ToLower().Contains(".aspx"))
    //{
    //    url = url.Replace(".aspx","").Replace("/pay/do.dll/", "/PayOnlineCC.aspx?");
    //    Context.RewritePath(url);
    //}
    //else if (url.Contains("/pay/do.dll/"))
    //{
    //    url = url.Replace("/pay/do.dll/", "/");
    //    Context.RewritePath(url);
    //}


    //  }



    //getting root     
    string GetAppRoot(string sRequestedPath)
    {
        return sRequestedPath.Replace(System.IO.Path.GetFileName(sRequestedPath), "");
    }


    private bool CheckCredential()
    {
        bool returnvalue = false;
        bool validUser;
        string username;
        string password;
        string login;
        int UserID;
        string strUrl = string.Empty;
        strUrl = HttpContext.Current.Request.Url.ToString().ToLower();

        TradingBell.WebCat.Helpers.Security objSecurity = new TradingBell.WebCat.Helpers.Security();
        UserServices objUserServices = new UserServices();
       
        CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();

        if (Request.Cookies["BigtopLoginInfo"] != null && Request.Cookies["BigtopLoginInfo"].Value.ToString().Trim() != "")
        {
            try
            {
                login = "False";
                HttpCookie LoginInfoCookie = Request.Cookies["BigtopLoginInfo"];
                username = objSecurity.StringDeCrypt_password(LoginInfoCookie["UserName"].ToString());
                password = objSecurity.StringDeCrypt_password(LoginInfoCookie["Password"].ToString());
                if (LoginInfoCookie["Login"] != null)
                    login = objSecurity.StringDeCrypt_password(LoginInfoCookie["Login"].ToString());
                validUser = objUserServices.CheckUserName(username);
                UserID = objUserServices.GetUserID(username);
                if (UserID != -1 && username != string.Empty && login == "True")
                {
                    if (objCompanyGroupServices.CheckCompanyStatus(UserID) == CompanyGroupServices.CompanyStatus.ACTIVE.ToString())
                    {
                        if ((validUser))
                        {
                            bool HasAdminUser = objUserServices.HasAdmin(UserID);
                            if (objUserServices.IsUserActive(UserID.ToString()))
                            {
                                password = objSecurity.StringEnCrypt_password(password);
                                if (objUserServices.CheckUser(username, password))
                                {
                                    string Role;
                                    Role = objUserServices.GetRole(UserID);

                                    if (Role != null)
                                    {
                                        returnvalue = true;
                                        objUserServices.OnLineFlag(true, UserID);
                                        Session["USER_NAME"] = username;
                                        Session["USER_ID"] = UserID;
                                        Session["USER_ROLE"] = Role;
                                        Session["COMPANY_ID"] = objUserServices.GetCompanyID(UserID);
                                        Session["CUSTOMER_TYPE"] = objUserServices.GetCustomerType(UserID);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Session["USER_NAME"] = string.Empty;
                    Session["USER_ID"] = string.Empty;
                    Session["USER_ROLE"] = "0";
                    Session["COMPANY_ID"] = string.Empty;
                    returnvalue = true;
                }

            }
            catch (Exception ex)
            {
                //objErrorHandler.ErrorMsg = ex;
                //objErrorHandler.CreateLog();
            }

        }
        else
        {
            Session["USER_NAME"] = string.Empty;
            Session["USER_ID"] = string.Empty;
            Session["USER_ROLE"] = "0";
            Session["COMPANY_ID"] = string.Empty;
            returnvalue = true;
        }


        //if (returnvalue == false && strUrl.ToLower().Contains("orderdetail") == true && strUrl.ToLower().Contains("popup=true") == true)
        //    returnvalue = true;

        if (!(returnvalue) && (strUrl.Contains("orderdetail")) && (strUrl.Contains("popup=true")))
            returnvalue = true;

        return returnvalue;

    }
   
    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        //string lurl1 = HttpContext.Current.Request.RawUrl.ToString().Replace("\\", "");
        HelperServices objError = new HelperServices();
        ErrorHandler objErrorhandler = new ErrorHandler();
       
        //objError.writelog(HttpContext.Current.Request.RawUrl.ToString());
        //objError.writelog(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() + "," + HttpContext.Current.Request.Url.ToString().ToLower() + "," + HttpContext.Current.Request.UserAgent);
        string strRawUrl = HttpContext.Current.Request.RawUrl.ToString().ToLower();
        if (((strRawUrl.Contains("?gclid=")) || (strRawUrl.Contains("?utm"))) &&
            (
            (strRawUrl.Contains("/ct/")) ||
            (strRawUrl.Contains("/pl/")) ||
             (strRawUrl.Contains("/fl/")) ||
              (strRawUrl.Contains("/ps/")) ||
               (strRawUrl.Contains("/pd/")) ||
                (strRawUrl.Contains("/bb/")) ||
                (strRawUrl.Contains("/mct/")) ||
                 (strRawUrl.Contains("/mpd/")) ||
                  (strRawUrl.Contains("/mfl/")) ||
                  (strRawUrl.Contains("/mpl/")) ||
                  (strRawUrl.Contains("/bk/")) ||
                    (strRawUrl.Contains("/bp/")) ||
                     (strRawUrl == "/")
                  )
            )
        {

         //   objErrorhandler.CreateLog(strRawUrl);
            string[] newurl = new string[2];
            newurl = strRawUrl.Split(new string[] { "?" }, StringSplitOptions.None);
            Response.Redirect(newurl[0], false);
        }


        //if (strRawUrl.Contains("/pd/"))
        //{
        //   if ((strRawUrl.Contains("?gclid=")) ==true)
        //   {
        //       objErrorhandler.CreateLog(strRawUrl);
        //       string[] newurl = new string[2];
        //       newurl = strRawUrl.Split(new string[] { "?" }, StringSplitOptions.None);
        //       Response.Redirect(newurl[0], false);
        //   }
        //   // objErrorhandler.CreateLog("inside    global" + DateTime.Now.ToString("hh.mm.ss.ffffff") + HttpContext.Current.Request.RawUrl.ToString());
        //}
        if (strRawUrl.Contains("/mpd/"))
        {
            if (strRawUrl.Contains("/cypconverters/") || strRawUrl.Contains("/electrical-wholesalers-online/") || strRawUrl.Contains("/vip-vision/"))
            {

            }
            else
            {

                string[] mainstrrawurl = strRawUrl.Split(new string[] { "/" }, StringSplitOptions.None);
                string url = string.Empty;
                try
                {
                    Response.Redirect("/"+mainstrrawurl[mainstrrawurl.Length-3]+"/ps/");
                  //  objError.writelog(mainstrrawurl.Length.ToString());
                    
                    //if (mainstrrawurl.Length == 8)
                    //{
                    //    url = "/" + mainstrrawurl[3] +"/" +  mainstrrawurl[2] + "/" + mainstrrawurl[1] + "/" +mainstrrawurl[4] + "/" + mainstrrawurl[5] + "/" + "pd" + "/";
                    //   // objError.writelog(url);
                    //    //url = "/" + mainstrrawurl[3] + "/" + mainstrrawurl[4] + "/" + mainstrrawurl[5] + "/" + "pd" + "/";
                    //    //objError.writelog(url);
                    //    url = url.Replace("/mpd", "/pd/"); 
                    //    Response.Redirect(url);
                    //}
                    //else if (mainstrrawurl.Length == 7)
                    //{

                    //    url = "/" + mainstrrawurl[2] + "/" + mainstrrawurl[1] + "/" + mainstrrawurl[3] + "/" + mainstrrawurl[4] + "/" + mainstrrawurl[5] + "/" + "pd" + "/";
                    //   // objError.writelog(url);
                    //    url = url.Replace("/mpd", "/pd/"); 
                    //    Response.Redirect(url);
                    //}
                }
                catch (Exception ex)
                {
                    //objErrorHandler.CreateLog(ex.ToString());
                }

            }
        }

        if (HttpContext.Current.Request.RawUrl.ToString().ToUpper() == "/PRODUCT_LIST.ASPX?AUDIO_COMPONENTS/SPEAKER_DRIVERS_AUDIOPHILE/PEERLESS")
        {
            Response.Redirect("/speakers-peerless/speaker-drivers-audiophile/audio-speakers-pa/pl/");
        }
        if (HttpContext.Current.Request.RawUrl.ToString().ToUpper() == "/PRODUCT_LIST.ASPX?AUDIO_COMPONENTS/SPEAKER_DRIVERS_AUDIOPHILE/VIFA")
        {
            Response.Redirect("/speakers-vifa/speaker-drivers-audiophile/audio-speakers-pa/pl/");
        }
       else if (HttpContext.Current.Request.RawUrl.ToString().ToUpper () == "/PRODUCT_LIST.ASPX?AUDIO_COMPONENTS/SPEAKER_DRIVERS_AUDIOPHILE/DAYTON_AUDIO")
        {
            Response.Redirect("/speakers-dayton-audio/speaker-drivers-audiophile/audio-speakers-pa/pl/");
        }
        else if (HttpContext.Current.Request.RawUrl.ToString().ToUpper() == "/PRODUCT_LIST.ASPX?AUDIO_COMPONENTS/SPEAKER_DRIVERS_AUDIOPHILE/SB_ACOUSTICS")
        {
            Response.Redirect("/speakers-sb-acoustics/speaker-drivers-audiophile/audio-speakers-pa/pl/");
        }
        else if (HttpContext.Current.Request.RawUrl.ToString().ToUpper() == "/PRODUCT_LIST.ASPX?AUDIO_COMPONENTS/SPEAKER_DRIVERS_AUDIOPHILE/SCAN-SPEAK")
        {
            Response.Redirect("/speakers-scan-speak/speaker-drivers-audiophile/audio-speakers-pa/pl/");
        }
        else if (HttpContext.Current.Request.RawUrl.ToString().ToUpper() == "/PRODUCT_LIST.ASPX?AUDIO_COMPONENTS/SPEAKER_DRIVERS_AUDIOPHILE/RIBBON_TWEETERS_FOUNTEK")
        {
            Response.Redirect("/ribbon-tweeters-fountek/speaker-drivers-audiophile/audio-speakers-pa/pl/");
        }
        else if (HttpContext.Current.Request.RawUrl.ToString().ToUpper() == "/PRODUCT_LIST.ASPX?AUDIO_COMPONENTS/SPEAKER_DRIVERS_AUDIOPHILE/VIFA")
        {
            Response.Redirect("/speakers-vifa/speaker-drivers-audiophile/audio-speakers-pa/pl/");
        }
        
        
        if ((HttpContext.Current.Request.RawUrl.ToString().ToLower().Contains("reelplay-tv-australia/")) 
            ||(HttpContext.Current.Request.RawUrl.ToString().ToLower().Contains("/hd110-ita-69195/976331/pd/"))
                ||(HttpContext.Current.Request.RawUrl.ToString().ToLower().Contains("/hd110-gre-69197/976332/pd/"))
                ||(HttpContext.Current.Request.RawUrl.ToString().ToLower().Contains("/hd110-ara-69194/976309/pd/"))
            || (HttpContext.Current.Request.RawUrl.ToString().ToLower().Contains("/hd110-ara-69194/976333/pd/")))
            
        {
            Response.Redirect("/home.aspx");
        }
      
        if (HttpContext.Current.Request.RawUrl.ToString().Contains("ps.aspx?keyword="))
        {
            string url = HttpContext.Current.Request.RawUrl.ToString().Replace("ps.aspx?keyword=", "") + "/ps/";
            Response.Redirect(url);
        }
        if (HttpContext.Current.Request.UrlReferrer == null || HttpContext.Current.Request.RawUrl.ToString().Contains(".pdf")==true)
        {
        
     
        //HelperServices objHelperService = new HelperServices();
  
        if (strRawUrl.EndsWith("/reelplay") || (strRawUrl.EndsWith("/reelplay/")))
        {

            Response.Redirect("reelplay-tv-australia/ps/");
        }
        if (((strRawUrl.Contains("?gclid=")) || (strRawUrl.Contains("?utm"))) &&
           ((strRawUrl.Contains("/reelplay/"))))
        {
            Response.Redirect("reelplay-tv-australia/ps/");
        }

        //if (strRawUrl.Contains("u00252Fhd110-ita-69195") == true)
        //{
        //    Response.Redirect("/home.aspx");
        //}
        //if (strRawUrl.EndsWith("69195/976309/pd/") || (strRawUrl.EndsWith("69195/976309/pd")))
        //{
        //    Response.Redirect("/home.aspx");
        //}
        //if (strRawUrl.EndsWith("69195/976309/mpd/") || (strRawUrl.EndsWith("69195/976309/mpd")))
        //{
        //    Response.Redirect("/home.aspx");
        //}
        //if (strRawUrl.EndsWith("69195/976331/pd/") || (strRawUrl.EndsWith("69195/976331/pd")))
        //{
        //    Response.Redirect("/home.aspx");
        //}
    // objHelperService.writelog  ("strRawUrl"+ strRawUrl);

        //if (!strRawUrl.Contains("pl/")  &&
        //    !strRawUrl.Contains("ps/") &&
        //     !strRawUrl.Contains("bb/") &&
        //      !strRawUrl.Contains("pl/")  &&
        //     !strRawUrl.Contains("ct/") &&
        //      !strRawUrl.Contains("microsite/")  &&
        //       !strRawUrl.Contains("bk/") &&
        //       !strRawUrl.Contains("bp/") &&
        //       !strRawUrl.Contains(".aspx") && !strRawUrl.Contains(".html")
        //       && !strRawUrl.Contains(".htm") && !strRawUrl.Contains(".pdf") && strRawUrl!="http://www.wagneronline.com.au/")
        //{

        //    Response.RedirectPermanent("/404new.htm");  
        //}
        //if (strRawUrl.Contains("404new.htm") && strRawUrl.EndsWith("404new.htm") == false)
        //{
        //    Response.Redirect("404new.htm");  

        //}
        
        string[] getfirstelement = strRawUrl.Split('/');

        string ele1 = getfirstelement[1];
        if (strRawUrl.StartsWith("/9") && ele1.Split('9').Length > 4)

        {
            Response.RedirectPermanent("/home.aspx",false);  
        }

        

        if
   ((strRawUrl.EndsWith("/ct")) ||
     (strRawUrl.EndsWith("/pl")) ||
      (strRawUrl.EndsWith("/fl")) ||
       (strRawUrl.EndsWith("/ps")) ||
        (strRawUrl.EndsWith("/pd")) ||
         (strRawUrl.EndsWith("/bb")) ||
         (strRawUrl.EndsWith("/mct")) ||
          (strRawUrl.EndsWith("/mpd")) ||
           (strRawUrl.EndsWith("/mfl")) ||
           (strRawUrl.EndsWith("/mpl"))

           )
        {

            Response.Redirect(strRawUrl + "/"); 
        }
       
  

        //Added By Indu for url crawl issue as on 9-Dec-2015
        if
  ((strRawUrl.ToLower().Contains("categorylist.aspx?")) ||
   // (strRawUrl.ToLower().Contains("product_list.aspx?")) ||
     (strRawUrl.ToLower().Contains("family.aspx?")) ||
      (strRawUrl.ToLower().Contains("powersearch.aspx")) ||
       (strRawUrl.ToLower().Contains("productdetails.aspx?")) ||
        (strRawUrl.ToLower().Contains("bybrand.aspx?")) 
          )
        {
            HelperServices objHelperServices = new HelperServices();
            objHelperServices.writelog("strRawUrl12345 "+strRawUrl);
            Response.RedirectPermanent("404New.htm");
        }



        if (
!((strRawUrl.Contains(".")) ||
(strRawUrl.Contains("/ct/")) ||
(strRawUrl.Contains("/pl/")) ||
(strRawUrl.Contains("/fl/")) ||
(strRawUrl.Contains("/ps/")) ||
(strRawUrl.Contains("/pd/")) ||
(strRawUrl.Contains("/bb/")) ||
(strRawUrl.Contains("/mct/")) ||
 (strRawUrl.Contains("/mpd/")) ||
  (strRawUrl.Contains("/mfl/")) ||
  (strRawUrl.Contains("/mpl/")) ||
(strRawUrl.Contains("/mps/")) ||
  (strRawUrl.Contains("/bk/")) ||
    (strRawUrl.Contains("/bp/")) ||
     (strRawUrl.Contains("/electronics-news/")) ||
     (strRawUrl.Contains("/reelplay")) ||
       (strRawUrl == "/") ||
          (strRawUrl.ToLower().Contains("/404new.htm")) ||
                  (strRawUrl.ToLower().Contains("/vip-vision/")) ||
              (strRawUrl.ToLower().Contains("/electrical-wholesalers-online/")) ||
                (strRawUrl.ToLower().Contains("/cypconverters/")) ||
                  (strRawUrl.ToLower().Contains("/reelplay-tv-australia/")) ||
                    (strRawUrl.ToLower().Contains("/wagner-catalogue/")) ||
                     (strRawUrl.ToLower().Contains("/wagner-product-update")) ||
           (strRawUrl.ToLower().Contains("/catalogue/"))
  )
)
        {
           // Response.RedirectPermanent("/404new.htm");
            Response.RedirectPermanent("home.aspx");
        }
        
        //end
        if
     (!((strRawUrl.EndsWith("/ct/")) ||
       (strRawUrl.EndsWith("/pl/")) ||
        (strRawUrl.EndsWith("/fl/")) ||
         (strRawUrl.EndsWith("/ps/")) ||
          (strRawUrl.EndsWith("/pd/")) ||
           (strRawUrl.EndsWith("/bb/")) ||
           (strRawUrl.EndsWith("/mct/")) ||
            (strRawUrl.EndsWith("/mpd/")) ||
             (strRawUrl.EndsWith("/mfl/")) ||
             (strRawUrl.EndsWith("/mpl/")) ||
             (strRawUrl.EndsWith("home.aspx")) ||
             (strRawUrl.EndsWith("wagcaptcha.aspx")) ||
              (strRawUrl.EndsWith("contactus.aspx")) ||
               (strRawUrl.EndsWith("login.aspx"))
             ))
        {


            string strUrlPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery.ToString().ToLower();

            string strUrl = HttpContext.Current.Request.Url.ToString().ToLower();

            if ((strRawUrl.Contains("?gclid=")) &&
            (
            (strRawUrl.Contains("/ct/")) ||
            (strRawUrl.Contains("/pl/")) ||
             (strRawUrl.Contains("/fl/")) ||
              (strRawUrl.Contains("/ps/")) ||
               (strRawUrl.Contains("/pd/")) ||
                (strRawUrl.Contains("/bb/")) ||
                (strRawUrl.Contains("/mct/")) ||
                 (strRawUrl.Contains("/mpd/")) ||
                  (strRawUrl.Contains("/mfl/")) ||
                  (strRawUrl.Contains("/mpl/")) ||
                  (strRawUrl.Contains("/bk/")) ||
                    (strRawUrl.Contains("/bp/")) ||
                     (strRawUrl == "/")
                  )
            )
            {

                string[] newurl = new string[2];
                newurl = strRawUrl.Split(new string[] { "?gclid=" }, StringSplitOptions.None);
                Response.Redirect(newurl[0],false);
            }


           
            if ((strRawUrl.Contains("?mc_cid="))
                &&
          (
          (strRawUrl.Contains("/ct/")) ||
          (strRawUrl.Contains("/pl/")) ||
           (strRawUrl.Contains("/fl/")) ||
            (strRawUrl.Contains("/ps/")) ||
             (strRawUrl.Contains("/pd/")) ||
              (strRawUrl.Contains("/bb/")) ||
              (strRawUrl.Contains("/mct/")) ||
               (strRawUrl.Contains("/mpd/")) ||
                (strRawUrl.Contains("/mfl/")) ||
                (strRawUrl.Contains("/mpl/")) ||
                (strRawUrl.Contains("/bk/")) ||
                  (strRawUrl.Contains("/bp/")) ||
                   (strRawUrl == "/")
                )
          )
            {
               
                string[] newurl = new string[2];
                newurl = strRawUrl.Split(new string[] { "?mc_cid=" }, StringSplitOptions.None);
                Response.Redirect(newurl[0], false);
               
            }
            if (strRawUrl.Contains("captchaimage.aspx"))
            {
                //HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] = "/"+strRawUrl.Substring(strRawUrl.ToLower().IndexOf("captchaimage.axd"));
                HttpContext.Current.RewritePath("/" + strRawUrl.Substring(strRawUrl.IndexOf("captchaimage.aspx")));
            }
            //if (strRawUrl.ToLower().Contains("onlinecatalogue.aspx") == true)
            else if (strRawUrl.Contains("onlinecatalogue.aspx"))
            {
                strRawUrl = "/catalogue/";
                Response.RedirectPermanent(strRawUrl);
            }
            //else if (strRawUrl.Contains("contactus.aspx/sendenquirymail"))
            //{
            //    strRawUrl = "/contactus.aspx/SendEnquiryMail";
            //    Response.RedirectPermanent(strRawUrl);
            //}
           
            //else if (strUrlPathAndQuery.EndsWith("product_list.aspx"))
            //{

            //    Response.RedirectPermanent("/404New.htm");
            //}
            //else if (strUrlPathAndQuery.EndsWith("pl.aspx"))
            //{

            //    Response.RedirectPermanent("/404New.htm");
            //}


            //if (!(strUrlPathAndQuery.Contains(".")) &&
            //     !(strUrlPathAndQuery.Contains("/ps.aspx")) &&
            //     !(strUrlPathAndQuery.Contains("/bb.aspx")) &&
            //     !(strUrlPathAndQuery.Contains("/pd.aspx")) &&
            //     !(strUrlPathAndQuery.Contains("/fl.aspx")) &&
            //     !(strUrlPathAndQuery.Contains("/pl.aspx")) &&
            //      !(strUrlPathAndQuery.Contains("/ct.aspx")) &&
            //      !(strUrlPathAndQuery.Contains("/mct.aspx")) &&
            //    !(strUrlPathAndQuery.Contains("/electronics-news/")) &&

            //      strUrlPathAndQuery != "/")
            //{
            //    string[] querystring = strUrlPathAndQuery.Split('/');

            //    string plpage = objHelperService.SimpleURL_Str(querystring[1].Replace("-", " "), "ps.aspx",true) + "/ps/";
            //    Response.RedirectPermanent(plpage,false);
            //}

            //else if (strUrlPathAndQuery.EndsWith("aspx/"))
            //{

            //    string querystring = string.Empty;

            //    querystring = strUrlPathAndQuery;
            //    querystring = querystring.Substring(0, querystring.Length - 1);
            //    Response.Redirect(querystring,false);
            //}
            //else if (strUrlPathAndQuery.EndsWith("family.aspx"))
            //{


            //    Response.RedirectPermanent("/404New.htm");
            //}

            //else if (strUrlPathAndQuery.EndsWith("family.aspx"))
            //{


            //    Response.RedirectPermanent("/404New.htm");
            //}
            //string formname=HttpContext.Current.Request.RawUrl.ToString().StartsWith("  

            //if ((!(strUrlPathAndQuery.Contains("_")))
            //    && (!(strUrlPathAndQuery.Contains("&")))
            //    && (!(strUrlPathAndQuery.Contains("=")))
            //    )
            //{
            //    if ((strRawUrl.StartsWith("/ct.aspx?"))
            //        || (strRawUrl.StartsWith("/microsite.aspx?"))
            //        || (strRawUrl.StartsWith("/fl.aspx?"))
            //        || (strRawUrl.StartsWith("/pl.aspx?"))
            //|| (strRawUrl.StartsWith("/pd.aspx?"))
            //|| (strRawUrl.StartsWith("/ps.aspx?"))
            //|| (strRawUrl.StartsWith("/bb.aspx?")))
            //    {
            //        string formname = HttpContext.Current.Request.Path + "?";
            //        string newformname = HttpContext.Current.Request.Path.Replace(".aspx", "/");
            //        string RAWURL = string.Empty;
            //        if (!(strUrlPathAndQuery.Contains("-")) && !(strUrlPathAndQuery.Contains("9")) && !(strUrlPathAndQuery.Contains("=")) && !(strUrlPathAndQuery.Contains("/")))
            //        {
            //            RAWURL = strUrlPathAndQuery + "/9";

            //        }
            //        else
            //        {
            //            if (!(strUrlPathAndQuery.Contains("9")))
            //            {
            //                RAWURL = strUrlPathAndQuery + "/9";
            //            }
            //            else
            //            {
            //                RAWURL = strUrlPathAndQuery;

            //            }
            //        }
            //        string newurl = "/" + RAWURL.Replace(formname, "") + newformname;

            //        Response.Redirect(newurl,false);
            //    }

            //}






            //if (strRawUrl.ToLower().Replace("categorylist.aspx_supplier1.aspx", "categorylist_supplier1.aspx").Contains("categorylist_supplier1.aspx?"))
            //{

            //    Response.RedirectPermanent(strRawUrl);
            //}

            //if (strUrlPathAndQuery.Replace(".aspx.aspx", ".aspx").Contains("categorylist.aspx?"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.RedirectPermanent(newurl.ToLower().Replace(".aspx.aspx", ".aspx").Replace("categorylist.aspx?", "ct.aspx?").Replace("??", "?"), false);
            //}

            //else if (strUrlPathAndQuery.Replace(".aspx.aspx", ".aspx").Contains("family.aspx?"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.RedirectPermanent(newurl.ToLower().Replace(".aspx.aspx", ".aspx").Replace("family.aspx?", "fl.aspx?").Replace("??", "?"), false);
            //}
            //else if (strUrlPathAndQuery.Contains("familydet?"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.RedirectPermanent(newurl.ToLower().Replace(".aspx.aspx", ".aspx").Replace("familydet?", "fl.aspx?").Replace("??", "?"), false);
            //}
            //else if (strUrlPathAndQuery.Replace(".aspx.aspx", ".aspx").Contains("product_list.aspx?"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.RedirectPermanent(newurl.ToLower().Replace(".aspx.aspx", ".aspx").Replace("product_list.aspx?", "pl.aspx?").Replace("??", "?"), false);
            //}


            //else if (strUrlPathAndQuery.Contains("productlist?"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.RedirectPermanent(newurl.ToLower().Replace(".aspx.aspx", ".aspx").Replace("productlist?", "pl.aspx?").Replace("??", "?"), false);
            //}
            //else if (strUrlPathAndQuery.Replace(".aspx.aspx", ".aspx").Contains("productdetails.aspx?"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.RedirectPermanent(newurl.ToLower().Replace(".aspx.aspx", ".aspx").Replace("productdetails.aspx?", "pd.aspx?").Replace("??", "?"), false);
            //}
            //else if (strUrlPathAndQuery.Contains("ProdDetails?"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.RedirectPermanent(newurl.ToLower().Replace(".aspx.aspx", ".aspx").Replace("proddetails?", "pd.aspx?").Replace("??", "?"), false);
            //}

            //else if (strUrlPathAndQuery.Replace(".aspx.aspx", ".aspx").Contains("powersearch.aspx?"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.RedirectPermanent(newurl.ToLower().Replace(".aspx.aspx", ".aspx").Replace("powersearch.aspx?", "ps.aspx?").Replace("??", "?"), false);
            //}

            //else if (strUrlPathAndQuery.Contains("psearch?"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.RedirectPermanent(newurl.ToLower().Replace(".aspx.aspx", ".aspx").Replace("psearch?", "ps.aspx?").Replace("??", "?"));
            //}

            //else if (strUrlPathAndQuery.Replace(".aspx.aspx", ".aspx").Contains("bybrand.aspx?"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.RedirectPermanent(newurl.ToLower().Replace(".aspx.aspx", ".aspx").Replace("bybrand.aspx?", "bb.aspx?").Replace("??", "?"));
            //}
            ////if (strUrlPathAndQuery.ToLower().Replace(".aspx.aspx", ".aspx").Contains("onlinecatalogue.aspx"))
            ////{
            ////    string newurl = strUrlPathAndQuery.Replace("onlinecatalogue.aspx", "catalogue/");
            ////    Response.RedirectPermanent(newurl.ToLower());
            ////}

            //else if (strUrlPathAndQuery.Contains("bybrand?"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.RedirectPermanent(newurl.ToLower().Replace("bybrand?", "bb.aspx?").Replace("??", "?"));
            //}

            if (((strUrl.ToLower().Contains(".pdf")) || (strUrl.Contains(".zip")) 
                || (strUrl.Contains(".htm"))
                || (strUrl.Contains(".html"))) &&
                !(strUrl.Contains("404new.htm")))
            {
                
              string lurl = HttpContext.Current.Request.RawUrl.ToString().Replace("\\","") ;
              HelperServices objHelperServices = new HelperServices();
              objHelperServices.writelog(lurl); 
              if (((lurl.ToLower().Contains(".pdf") || lurl.Contains(".zip")) && !lurl.ToLower().Contains("attachments/attachments") && lurl.ToLower().Contains("/attachments/")))
               {
                   lurl = lurl.ToLower().Replace("/Attachments/", "/Attachments/attachments/").Replace("/attachments/", "/Attachments/attachments/");
                   objHelperServices.writelog("b4 context rewrite"+lurl); 
                  Context.RewritePath(lurl);
              }
                // string[] localhost = strUrl.Split(new string[] { "/" }, StringSplitOptions.None);

                // string querypath=strUrlPathAndQuery.Replace ("/","\\");
                if (!(strUrl.Contains("google82359746d10d964f.html")))
                {
                    string newurl = string.Empty;
                    if (strUrlPathAndQuery.Contains("attachments"))
                    {
                        newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
                    }
                    else
                    {
                        if (!(strUrl.Contains("404new.htm")))
                        {
                            newurl = "/attachments" + HttpUtility.UrlDecode(strUrlPathAndQuery);
                        }

                    }


                    //string newurl =  HttpUtility.UrlDecode(strUrlPathAndQuery);

                   // objHelperServices.writelog(strUrl);
                    //objHelperServices.writelog(newurl);
                    //Context.RewritePath(newurl);
                }

            }

            //if (strUrlPathAndQuery.ToLower().Contains(".") == false)
            //{
            //    string[] strurl = strUrlPathAndQuery.ToLower().Split('/');

            //    string strvalue = objHelperServices.Cons_NewURl_bybrand("ps.aspx?srctext=" + HttpUtility.UrlEncode(strurl[1]) + "", strurl[1], "ps.aspx", "");
            //    strvalue = "/" + strvalue + "/ps/";
            //    Response.RedirectPermanent(strvalue);
            //}

            //if (strUrlPathAndQuery.ToLower().Contains("ct/"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Context.RewritePath(newurl.ToLower().Replace("ct/", "ct.aspx?"));
            //    Response.Redirect(newurl.ToLower().Replace("ct.aspx", "ct.aspx"));
            //}
            //if (strUrlPathAndQuery.ToLower().Contains("fl/"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Context.RewritePath(newurl.ToLower().Replace("fl/", "fl.aspx?"));
            //}

            //if (strUrlPathAndQuery.ToLower().Contains("pl/"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Context.RewritePath(newurl.ToLower().Replace("pl/", "pl.aspx?"));
            //}

            //if (strUrlPathAndQuery.ToLower().Contains("pd/"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Context.RewritePath(newurl.ToLower().Replace("pd/", "pd.aspx?"));
            //}

            //if (strUrlPathAndQuery.ToLower().Contains("ps/"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Context.RewritePath(newurl.ToLower().Replace("ps/", "ps.aspx?"));
            //}


            //if (strUrlPathAndQuery.ToLower().Contains("bb/"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Context.RewritePath(newurl.ToLower().Replace("bb/", "bb.aspx?"));
            //}
            /*************/
            //if (strUrlPathAndQuery.ToLower().Contains("categorybr"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.Redirect(newurl.ToLower().Replace("categorybr/", "ct.aspx"));
            //}
            //if (strUrlPathAndQuery.ToLower().Contains("familydet"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.Redirect(newurl.ToLower().Replace("familydet/", "fl.aspx?"));
            //}

            //if (strUrlPathAndQuery.ToLower().Contains("productlist"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.Redirect(newurl.ToLower().Replace("productlist/", "pl.aspx?"));
            //}

            //if (strUrlPathAndQuery.ToLower().Contains("proddetails"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.Redirect(newurl.ToLower().Replace("proddetails/", "pd.aspx?"));
            //}

            //if (strUrlPathAndQuery.ToLower().Contains("psearch"))
            //{
            //    string newurl = HttpUtility.UrlDecode(strUrlPathAndQuery);
            //    Response.Redirect(newurl.ToLower().Replace("psearch/", "ps.aspx?"));
            //}  
        }
        }
            
        else
        {
          

            string reqrawurl = Request.RawUrl.ToLower();
            if (reqrawurl.Contains("/ct/") || reqrawurl.Contains("/pl/") || reqrawurl.Contains("/bb/") )
            {

                //Get_Value_Breadcrum_New();

                if (reqrawurl.Contains("/pno-") )
                {
                    //HelperServices objHelperServices = new HelperServices();
                    //objHelperServices.writelog(HttpContext.Current.Request.UrlReferrer.ToString().ToLower());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();

                }
            }
        }
            
    
    }


    protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {



        string strUrl = string.Empty;

        strUrl = HttpContext.Current.Request.Url.ToString().ToLower();
        //if (System.Web.HttpContext.Current.Session != null)
        //{
        //    if (HttpContext.Current.Session["dtcid"] != null)
        //    {
        //        DataTable dt = (DataTable)HttpContext.Current.Session["dtcid"];
        //        if (dt.Select("cname='new Products'").Length <= 0)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = "WESNEWS";
        //            dr[1] = "New Products";
        //            dt.Rows.Add(dr);
        //            HttpContext.Current.Session["dtcid"] = dt;
        //        }
        //    }
        //}
        TradingBell.WebCat.Helpers.Security objSecurity = new TradingBell.WebCat.Helpers.Security();
        //if (ConfigurationManager.AppSettings["SSL_ACTIVE"].ToString().Equals("1"))
        //{
        //    if (!Request.IsLocal)
        //    {

        //        if ((strUrl.Contains("/login.aspx")) || (strUrl.Contains("newcustomerregistration.aspx")) || (strUrl.Contains("dealerregistration.aspx")) || (strUrl.Contains("retailerregistration.aspx")) || (strUrl.Contains("createanaccount.aspx")) || (strUrl.Contains("payonlinecc.aspx")))
        //            objSecurity.RedirectSSL(HttpContext.Current, true, true);
        //        else
        //        {
        //            if (System.Web.HttpContext.Current.Session != null) //if (HttpContext.Current.Session.Count > 0)
        //            {
        //                if (System.Web.HttpContext.Current.Session["USER_ID"] != null && System.Web.HttpContext.Current.Session["USER_ID"].ToString() != "" && Convert.ToInt32(System.Web.HttpContext.Current.Session["USER_ID"].ToString()) > 0 && Convert.ToInt32(System.Web.HttpContext.Current.Session["USER_ID"].ToString()) != 777)
        //                {
        //                    objSecurity.RedirectSSL(HttpContext.Current, true, true);
        //                }
        //                else
        //                {
        //                    objSecurity.RedirectSSL(HttpContext.Current, true, false);
        //                }
        //            }
        //           // else
        //            //    objSecurity.RedirectSSL(HttpContext.Current, true, false);

        //        }

        //    }
        //}
        if (HttpContext.Current.Request.RawUrl.ToLower().Contains("captchaimage.aspx"))
        {
           // HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] = "/" + HttpContext.Current.Request.RawUrl.Substring(HttpContext.Current.Request.RawUrl.ToLower().IndexOf("captchaimage.axd"));
            HttpContext.Current.RewritePath("/" + HttpContext.Current.Request.RawUrl.Substring(HttpContext.Current.Request.RawUrl.ToLower().IndexOf("captchaimage.aspx")));
        }
        
        if (!(strUrl.Contains("userbasicinfo.aspx")) &&
            !(strUrl.Contains("companyprofile.aspx")) && !(strUrl.Contains("userprofile.aspx")) &&
            !(strUrl.Contains("forgotpassword.aspx")) && !(strUrl.Contains("confirmmessage.aspx")) &&
            !(strUrl.Contains("aboutus.aspx")) && !(strUrl.Contains("contactus.aspx")) &&
            !(strUrl.Contains("newcustomerregistration.aspx")) && !(strUrl.Contains("existcustomerregistration.aspx")) &&
            !(strUrl.Contains("dealerregistration.aspx")) && !(strUrl.Contains("retailerregistration.aspx")) &&
            !(strUrl.Contains("activation.aspx"))
            && !(strUrl.Contains("getdeal.aspx"))
            && !(strUrl.Contains("termsandconditions.aspx")) && !(strUrl.Contains("resetpassword.aspx")) && !(strUrl.Contains("forgotusername.aspx"))
            && !(strUrl.Contains("createanaccount.aspx"))
            && !(strUrl.Contains("makeopenordermail.aspx"))
            && !(strUrl.Contains("gblwebmethods.aspx"))
               && !(strUrl.Contains("popchangepwd.aspx")) && !(strUrl.Contains("makepaymentmail.aspx"))
              
            )
        {
            if (ConfigurationManager.AppSettings["REQUIRE_LOGIN"].ToString().Equals("YES"))
            {
                if (((strUrl.Contains(".aspx"))) && HttpContext.Current.Session != null && HttpContext.Current.Session["USER_ID"] != null)
                {
                    if (!HttpContext.Current.Session["USER_ID"].ToString().Equals(string.Empty) && Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) > 0)
                    {
                        string sRequestedPath, sPageExtention = "";
                        HelperServices objHelperServices = new HelperServices();
                        sRequestedPath = HttpContext.Current.Request.Path.ToLower();
                        string _sAppRoot = GetAppRoot(sRequestedPath);
                        //if (sRequestedPath.Contains(".aspx"))
                        //    Session["PageUrl"] = sRequestedPath.Substring(sRequestedPath.IndexOf("/", 1) + 1).ToString();
                        // generating relevant url
                        if (sRequestedPath.IndexOf("c-", StringComparison.InvariantCultureIgnoreCase) > -1)
                        {
                            sPageExtention = Convert.ToString(objHelperServices.GetOptionValues("PAGE EXTENTION"));
                            if (sRequestedPath.EndsWith(sPageExtention))
                            {
                                string[] sArrCustomUrlParts = sRequestedPath.Substring(sRequestedPath.LastIndexOf("/")).Split('-');
                                string sCatId = sArrCustomUrlParts[1].ToString();
                                sRequestedPath = sRequestedPath.Substring(0, sRequestedPath.LastIndexOf("/"));
                                sRequestedPath = sRequestedPath + "//Home.aspx?CatID=" + sCatId;
                                Context.RewritePath(sRequestedPath, false);
                            }
                        }
                        else if (sRequestedPath.IndexOf("f-", StringComparison.InvariantCultureIgnoreCase) > -1)
                        {
                            sPageExtention = Convert.ToString(objHelperServices.GetOptionValues("PAGE EXTENTION"));
                            if (sRequestedPath.EndsWith(sPageExtention))
                            {
                                string[] sArrCustomUrlParts = sRequestedPath.Substring(sRequestedPath.LastIndexOf("/")).Split('-');
                                string sCatId = sArrCustomUrlParts[1].ToString();
                                sRequestedPath = sRequestedPath.Substring(0, sRequestedPath.LastIndexOf("/"));
                                sRequestedPath = sRequestedPath + "/fl.aspx?Cat=" + sCatId;
                                Context.RewritePath(sRequestedPath, false);
                            }
                        }
                    }
                    else if ((strUrl.Contains(".aspx")) && !(strUrl.Contains("/Login.aspx")))
                    {
                        if (HttpContext.Current.Session == null || HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"].ToString().Equals(string.Empty))
                            if (HttpContext.Current.Session["USER_ID"].ToString().Equals(string.Empty) || Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) < 1)
                            {

                                if ((strUrl.Contains("mmyaccount.aspx")) || (strUrl.Contains("mchangepassword.aspx")) || (strUrl.Contains("mchangeusername.aspx")) || (strUrl.Contains("morderhistory.aspx")))
                                {
                                    HttpContext.Current.Response.Redirect("/mlogin.aspx");
                                }
                                //HttpContext.Current.Session["PageUrl"] = HttpContext.Current.Request.Url.AbsoluteUri;
                                if (!(CheckCredential()))
                                    HttpContext.Current.Response.Redirect("/Login.aspx");
                            }
                    }

                }
                else if ((strUrl.Contains(".aspx")) && (strUrl.Contains("/Login.aspx")))
                {
                    if (HttpContext.Current.Session == null || HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"].ToString() == string.Empty)
                        if (HttpContext.Current.Session["USER_ID"].ToString() == string.Empty || Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) < 1)
                        {
                           // if ((CheckCredential()))
                              //  HttpContext.Current.Response.Redirect("/Home.aspx");

                        }
                }
                /*                else if (strUrl.ToLower().Contains("cataloguedownload.aspx") == true && !strUrl.ToLower().Contains("cataloguedownload.aspx"))
                                {
                                    HttpContext.Current.Response.Redirect("login.aspx?URL=cataloguedownload.aspx");
                                }*/
                //else if (strUrl.ToLower().Contains("catalogue download") == true && !strUrl.ToLower().Contains("cataloguedownload.aspx"))
                //{
                //    if (HttpContext.Current.Request.UrlReferrer == null)
                //    HttpContext.Current.Response.Redirect("/login.aspx?URL=cataloguedownload.aspx");
                //}

                else if ((strUrl.Contains("attachments")) && !(strUrl.Contains("cataloguedownload.aspx")) &&  !(strUrl.Contains(".zip")))
                {
                    HelperServices objHelperServices = new HelperServices();
                    objHelperServices.writelog("strUrl "+strUrl);
                    if ((strUrl.Contains("404.htm")) || strUrl.Contains("404new.htm"))
                    {
                        HttpContext.Current.Response.Redirect("/404New.htm");
                    }
                        
                    //else if (HttpContext.Current.Request.UrlReferrer == null)
                    //{
                    //    HttpContext.Current.Response.Redirect("/login.aspx?URL=cataloguedownload.aspx");
                    //}
                }
                else if ((strUrl.Contains(".aspx")) && !(strUrl.Contains("/Login.aspx")))
                {
                    {
                        try
                        {
                            //HttpContext.Current.Session["PageUrl"] = HttpContext.Current.Request.Url.AbsoluteUri;

                        }
                        catch
                        {
    HelperServices objHelperServices = new HelperServices();
                            objHelperServices.writelog("strUrl1 "+strUrl);
                            HttpContext.Current.Response.Redirect("404New.htm");
                        }
                        if (!(CheckCredential()))
                            HttpContext.Current.Response.Redirect("/Login.aspx");
                    }
                }

            }
        }

    }
</script>
