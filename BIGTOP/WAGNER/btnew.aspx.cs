using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Braintree;
namespace WES
{
    public partial class btnew : System.Web.UI.Page
    {
        public string ClientToken = string.Empty;
        protected void Page_Load(object sender, EventArgs e)

        {
            if (!Page.IsPostBack)
            {
                GenerateClientToken();

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
                    Amount = 10.00M,
                    PaymentMethodNonce = Request["PaymentMethodNonce"],
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    },
                    BillingAddress = new AddressRequest
                    {
                        PostalCode = "641021"
                    },
                };

                Result<Transaction> result = gateway.Transaction.Sale(request);
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

        [System.Web.Services.WebMethod]
        public static string SaleTrans(string nounce, string Amount)
        {
            try
            {
                string x = "";
                var gateway = new BraintreeGateway
                {
                    Environment = Braintree.Environment.SANDBOX,
                    MerchantId = "mjff7p7mgb4qmp77",
                    PublicKey = "p78kxf6s7zhb8z8x",
                    PrivateKey = "c747edab3bb504dc5fb9606a1716ccd7"
                };
                var request = new TransactionRequest
                {
                    Amount = Convert.ToDecimal(Amount),

                    PaymentMethodNonce = nounce,

                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }

                };

                Result<Transaction> result = gateway.Transaction.Sale(request);
                if (result.IsSuccess() == true)
                {
                    x = "true";
                }

                return x;
            }

            catch (Exception ex)
            {
                return "";
            }
        }
    }
}