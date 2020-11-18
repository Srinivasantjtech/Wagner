using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



    public partial class wagcaptcha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string captchaval =  cVerify.GuidPath;
            Session["Captcha_url"] = captchaval;
            string captcha_code = string.Empty;
            Session["Captcha_code"] = cVerify.Text;
           
        }

        private void captchacontrol()
        {
            string cc = string.Empty;
            cc = cVerify.GuidPath;
            Session["Captcha_url"] = cc;
           
        }
        protected void cVerify_preRender(object sender, EventArgs e)
        {
            Session["AQ_PD_CAPTCH_VALUE"] = cVerify.Text;
            Session["AQ_PD_CAPTCH_IMAGE"] = cVerify.GuidPath;

        }
        [System.Web.Services.WebMethod]
        public static string getcaptchaimage()
        {
            string capval = string.Empty;
            string capcode = string.Empty;
            try
            {
                
                capval = HttpContext.Current.Session["Captcha_url"].ToString();
                capcode = HttpContext.Current.Session["Captcha_code"].ToString();
                return capval + ",," + capcode;
            }
            catch (Exception ex)
            {
            }
            return capval;
        }
    }
