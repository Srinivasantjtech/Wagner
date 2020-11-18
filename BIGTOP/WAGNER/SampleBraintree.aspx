<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SampleBraintree.aspx.cs" Inherits="WES.SampleBraintree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div>
  <label>Amount:</label>
  <asp:TextBox ID="txtAmount" runat="server" />
</div>
<div id="dropin-container"></div>
<asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
        <button id="payButton"></button> 
<script src="https://js.braintreegateway.com/v2/braintree.js"></script>
      <%--  <script src="https://js.braintreegateway.com/web/3.47.0/js/client.min.js"></script>
        <script src="https://js.braintreegateway.com/web/dropin/1.18.0/js/dropin.min.js"></script>
       <script src="https://js.braintreegateway.com/web/3.47.0/js/three-d-secure.min.js"></script>--%>
<script>
  braintree.setup("<%= this.ClientToken %>", "dropin", { container: "dropin-container" });
//   braintree.threeDSecure.create({
//  client: this.Cl,
//  version: '2-bootstrap3-modal'
//}, function (createError, threeDSecure) {
//  // set up lookup-complete listener
//  threeDSecure.on('lookup-complete', function (data, next) {
//    // check lookup data

//    next();
//  });

//  // using Hosted Fields, use `tokenize` to get back a credit card nonce

//  // challenge will be presented in a bootstrap 3 modal
//  threeDSecure.verifyCard({
//    nonce:  Request.Form["payment_method_nonce"],
//    bin: binFromTokenizationPayload,
//    amount: '100.00'
//  }, function (verifyError, payload) {
//    // inspect payload
//    // send payload.nonce to your server
//  });
//});


   
</script>

     <%--   <script>

            braintree.dropin.create({
                authorization: "<%= this.ClientToken %>",
                container: "#dropin-container",threeDSecure: true
}, function (err, dropinInstance) {
    if (err) {
        // Handle any errors that might've occurred when creating Drop-in
        console.error(err);
        return;
    }

    payButton.addEventListener('click', function (e) {
        e.preventDefault();
        dropinInstance.requestPaymentMethod({
          threeDSecure: threeDSecureParameters
        }, function (err, payload) {
            if (err) {
                // Handle errors in requesting payment method
            }
            // Send payload.nonce to your server

        });
    });
});
braintree.threeDSecure.create({
  authorization:  "<%= this.ClientToken %>",
  version: 2
},
function (threeDSecureError, threeDSecureInstance) {
  if (threeDSecureError) {
    // Handle initialization errors here
    return;
  }// Use the instance here
                });
           

            threeDSecureInstance.verifyCard({
  amount: 15,
  nonce: payload.nonce,
  bin: payload.details.bin,
  email: "customer_email",
  billingAddress: { /* address */ },
  additionalInformation: { /* more info */ },
  onLookupComplete: function (data, next) {
    // Use look-up data here
    next();
  }
},
function (verifyError, verifyResponse) {
  if (verifyError) {
    // Handle errors here
    return;
  }// Submit verifyResponse.nonce to your server
});
        </script>--%>
        </form>
</body>
</html>
