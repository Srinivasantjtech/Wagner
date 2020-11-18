<head>
<div id="paypal-button"></div>
<div d="paypal-credit-button"></div>
<!-- Load PayPal's checkout.js Library. -->i
<script src="https://www.paypalobjects.com/api/checkout.js" data-version-4 log-level="warn"></script>

<!-- Load the client component. -->
<script src="https://js.braintreegateway.com/web/3.59.0/js/client.min.js"></script>

<!-- Load the PayPal Checkout component. -->
<script src="https://js.braintreegateway.com/web/3.59.0/js/paypal-checkout.min.js"></script>
</head>
<body>
    <form>

  <script>
 



 // Create a client.
braintree.client.create({
  authorization: "eyJ2ZXJzaW9uIjoyLCJhdXRob3JpemF0aW9uRmluZ2VycHJpbnQiOiJleUowZVhBaU9pSktWMVFpTENKaGJHY2lPaUpGVXpJMU5pSXNJbXRwWkNJNklqSXdNVGd3TkRJMk1UWXRjMkZ1WkdKdmVDSXNJbWx6Y3lJNklrRjFkR2g1SW4wLmV5SmxlSEFpT2pFMU9EUXdPVGd6Tmpnc0ltcDBhU0k2SWpWaFl6STJOREl3TFRBME1UQXRORGMwTmkxaE1qQmxMV0V5WWpVeFpHUXhORFZqTWlJc0luTjFZaUk2SW0xcVptWTNjRGR0WjJJMGNXMXdOemNpTENKcGMzTWlPaUpCZFhSb2VTSXNJbTFsY21Ob1lXNTBJanA3SW5CMVlteHBZMTlwWkNJNkltMXFabVkzY0RkdFoySTBjVzF3TnpjaUxDSjJaWEpwWm5sZlkyRnlaRjlpZVY5a1pXWmhkV3gwSWpwMGNuVmxmU3dpY21sbmFIUnpJanBiSW0xaGJtRm5aVjkyWVhWc2RDSmRMQ0p2Y0hScGIyNXpJanA3ZlgwLkN5cVNRanpiWTlLc0puQjdpbDY1VEdkZ2tYNlN5dGs2Z2Zac1NNazNjNGp6SFhrUzdNb2JaaGRZU3lRbWZNM0tQOGI2RU1ENlBVZG5lVFBhUndRVEVBIiwiY29uZmlnVXJsIjoiaHR0cHM6Ly9hcGkuc2FuZGJveC5icmFpbnRyZWVnYXRld2F5LmNvbTo0NDMvbWVyY2hhbnRzL21qZmY3cDdtZ2I0cW1wNzcvY2xpZW50X2FwaS92MS9jb25maWd1cmF0aW9uIiwiZ3JhcGhRTCI6eyJ1cmwiOiJodHRwczovL3BheW1lbnRzLnNhbmRib3guYnJhaW50cmVlLWFwaS5jb20vZ3JhcGhxbCIsImRhdGUiOiIyMDE4LTA1LTA4In0sImNoYWxsZW5nZXMiOlsiY3Z2Il0sImVudmlyb25tZW50Ijoic2FuZGJveCIsImNsaWVudEFwaVVybCI6Imh0dHBzOi8vYXBpLnNhbmRib3guYnJhaW50cmVlZ2F0ZXdheS5jb206NDQzL21lcmNoYW50cy9tamZmN3A3bWdiNHFtcDc3L2NsaWVudF9hcGkiLCJhc3NldHNVcmwiOiJodHRwczovL2Fzc2V0cy5icmFpbnRyZWVnYXRld2F5LmNvbSIsImF1dGhVcmwiOiJodHRwczovL2F1dGgudmVubW8uc2FuZGJveC5icmFpbnRyZWVnYXRld2F5LmNvbSIsImFuYWx5dGljcyI6eyJ1cmwiOiJodHRwczovL29yaWdpbi1hbmFseXRpY3Mtc2FuZC5zYW5kYm94LmJyYWludHJlZS1hcGkuY29tL21qZmY3cDdtZ2I0cW1wNzcifSwidGhyZWVEU2VjdXJlRW5hYmxlZCI6dHJ1ZSwicGF5cGFsRW5hYmxlZCI6dHJ1ZSwicGF5cGFsIjp7ImRpc3BsYXlOYW1lIjoiV0VTIEFsbGlhbmNlIiwiY2xpZW50SWQiOiJBU3ZPVnF5cXhjLXl1TVhhWUZCYzNLeXFpeGZrVWkxa2FZdWx0UV82dkYwT2wyZU5ZZkh1ejZUQXNiRXNjUHZONFlzcDRJa0x6THNtWkVUNyIsInByaXZhY3lVcmwiOiJodHRwOi8vZXhhbXBsZS5jb20vcHAiLCJ1c2VyQWdyZWVtZW50VXJsIjoiaHR0cDovL2V4YW1wbGUuY29tL3RvcyIsImJhc2VVcmwiOiJodHRwczovL2Fzc2V0cy5icmFpbnRyZWVnYXRld2F5LmNvbSIsImFzc2V0c1VybCI6Imh0dHBzOi8vY2hlY2tvdXQucGF5cGFsLmNvbSIsImRpcmVjdEJhc2VVcmwiOm51bGwsImFsbG93SHR0cCI6dHJ1ZSwiZW52aXJvbm1lbnROb05ldHdvcmsiOmZhbHNlLCJlbnZpcm9ubWVudCI6Im9mZmxpbmUiLCJ1bnZldHRlZE1lcmNoYW50IjpmYWxzZSwiYnJhaW50cmVlQ2xpZW50SWQiOiJtYXN0ZXJjbGllbnQzIiwiYmlsbGluZ0FncmVlbWVudHNFbmFibGVkIjp0cnVlLCJtZXJjaGFudEFjY291bnRJZCI6Indlc2FsbGlhbmNlIiwiY3VycmVuY3lJc29Db2RlIjoiQVVEIn0sIm1lcmNoYW50SWQiOiJtamZmN3A3bWdiNHFtcDc3IiwidmVubW8iOiJvZmYifQ=="
}, function (clientErr, clientInstance) {

  // Stop if there was a problem creating the client.
  // This could happen if there is a network error or if the authorization
  // is invalid.
  if (clientErr) {
    console.error('Error creating client:', clientErr);
    return;
  }

  // Create a PayPal Checkout component.
  braintree.paypalCheckout.create({
    client: clientInstance
  }, function (paypalCheckoutErr, paypalCheckoutInstance) {

    // Stop if there was a problem creating PayPal Checkout.
    // This could happen if there was a network error or if it's incorrectly
    // configured.
    if (paypalCheckoutErr) {
      console.error('Error creating PayPal Checkout:', paypalCheckoutErr);
      return;
    }

    // Set up PayPal with the checkout.js library
    paypal.Button.render({
      env: 'sandbox', // or 'sandbox'

      payment: function () {
        return paypalCheckoutInstance.createPayment({
          flow: 'checkout', // Required
          amount: 10, // Required
          currency: 'USD' // Required
        });
      },

      onAuthorize: function (data, actions) {
        return paypalCheckoutInstance.tokenizePayment(data, function (err, payload) {
          // Submit `payload.nonce` to your server.
        });
      },

      onCancel: function (data) {
        console.log('checkout.js payment cancelled', JSON.stringify(data, 0, 2));
      },

      onError: function (err) {
        console.error('checkout.js error', err);
      }
    }, '#paypal-button').then(function () {
      // The PayPal button will be rendered in an html element with the id
      // `paypal-button`. This function will be called when the PayPal button
      // is set up and ready to be used.
    });

  });

});








  
  </script>
        </form> 
</body>