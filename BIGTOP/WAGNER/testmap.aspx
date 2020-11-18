 <html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>Google Places Autocomplete textbox using google maps api</title>
</head>
<body>
<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&key=AIzaSyD6KpDELswcok62eMQN_L2qBpBud3mqpGM&signed_in=true&libraries=places"></script>
<script type="text/javascript">
    google.maps.event.addDomListener(window, 'load', initialize);
    function initialize() {
        var autocomplete = new google.maps.places.Autocomplete(document.getElementById('txtAutocomplete'));
        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            // Get the place details from the autocomplete object.
            var place = autocomplete.getPlace();
            var location = "<b>Address</b>: " + place.formatted_address + "<br/>";
            location += "<b>Latitude</b>: " + place.geometry.location.A + "<br/>";
            location += "<b>Longitude</b>: " + place.geometry.location.F;
            document.getElementById('lblResult').innerHTML = location
        });
    }
</script>
<span>Location:</span>
<input type="text" id="txtAutocomplete" style="width: 300px" placeholder="Enter your address" /><br /><br />
<label id="lblResult" />
</body>
</html>

