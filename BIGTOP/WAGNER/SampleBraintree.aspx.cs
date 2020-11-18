using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Braintree;
namespace WES
{
    public partial class SampleBraintree : System.Web.UI.Page
    {
        protected string ClientToken = string.Empty;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GenerateClientToken();
                Pay();
               
            }
            else
            {
                var gateway = new BraintreeGateway
                {
                    Environment = Braintree.Environment.SANDBOX,
                    MerchantId = "mjff7p7mgb4qmp77",
                    PublicKey = "p78kxf6s7zhb8z8x",
                    PrivateKey = "c747edab3bb504dc5fb9606a1716ccd7"
                };
                var request = new TransactionRequest
                {
                    Amount = Convert.ToDecimal(txtAmount.Text),

                    PaymentMethodNonce = Request.Form["payment_method_nonce"],

                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }

                };

                Result<Transaction> result = gateway.Transaction.Sale(request);
                if (result.IsSuccess() == true)
                {

                }
            }

        }

        protected void GenerateClientToken()
        {
            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "mjff7p7mgb4qmp77",
                PublicKey = "p78kxf6s7zhb8z8x",
                PrivateKey = "c747edab3bb504dc5fb9606a1716ccd7"
            };
           

            this.ClientToken = gateway.ClientToken.Generate();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //var gateway = new BraintreeGateway
            //{
            //    Environment = Braintree.Environment.SANDBOX,
            //    MerchantId = "mjff7p7mgb4qmp77",
            //    PublicKey = "p78kxf6s7zhb8z8x",
            //    PrivateKey = "c747edab3bb504dc5fb9606a1716ccd7"
            //};

            if (Request.Form["payment_method_nonce"]==null)
            {

                Pay();
            }
            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(this.txtAmount.Text),
                PaymentMethodNonce = Request.Form["payment_method_nonce"],
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                },
                ThreeDSecurePassThru=new TransactionThreeDSecurePassThruRequest
                {


                }
            };
            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "mjff7p7mgb4qmp77",
                PublicKey = "p78kxf6s7zhb8z8x",
                PrivateKey = "c747edab3bb504dc5fb9606a1716ccd7"
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
        }

        protected void Pay()
        {
           string  PaymentMethodNonce_str = Request.Form["payment_method_nonce"];
            //    PaymentMethodNonce paymentMethodNonce = gateway.PaymentMethodNonce.Find("nonce_string");
            //var gateway = new BraintreeGateway
            //{
            //    Environment = Braintree.Environment.SANDBOX,
            //    MerchantId = "mjff7p7mgb4qmp77",
            //    PublicKey = "p78kxf6s7zhb8z8x",
            //    PrivateKey = "c747edab3bb504dc5fb9606a1716ccd7"
            //};
            //try
            //{
            //    PaymentMethodNonce paymentMethodNonce = gateway.PaymentMethodNonce.Find(PaymentMethodNonce_str);
            //    ThreeDSecureInfo info = paymentMethodNonce.ThreeDSecureInfo;
            //    if (info == null)
            //    {
            //        return; // This means that the nonce was not 3D Secured
            //    }

            //}
            //catch 
            //{ }

            //Build request string etc.
        }
    }
}