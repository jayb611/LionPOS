"use strict";

/*----------------------------------------------------------------------------*/
/*      Google Map settings - EDIT ONLY THIS
/*----------------------------------------------------------------------------*/
var latitude    = 36.174208,
    longitude   = -115.142728,
    zoom        = 14;

/*----------------------------------------------------------------------------*/
/*      Function to create your custom Google Map
/*----------------------------------------------------------------------------*/
function init() {

        // Google Map options
        // For more options see: https://developers.google.com/maps/documentation/javascript/reference#MapOptions
    var mapOptions = {
            disableDefaultUI        : true,
            draggable               : false,
            disableDoubleClickZoom  : true,
            scrollwheel             : false,
            zoom                    : zoom,
            center                  : new google.maps.LatLng(latitude,longitude),
            styles: [{"featureType":"water","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":17}]},{"featureType":"landscape","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":20}]},{"featureType":"road.highway","elementType":"geometry.fill","stylers":[{"color":"#000000"},{"lightness":17}]},{"featureType":"road.highway","elementType":"geometry.stroke","stylers":[{"color":"#000000"},{"lightness":29},{"weight":0.2}]},{"featureType":"road.arterial","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":18}]},{"featureType":"road.local","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":16}]},{"featureType":"poi","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":21}]},{"elementType":"labels.text.stroke","stylers":[{"visibility":"on"},{"color":"#000000"},{"lightness":16}]},{"elementType":"labels.text.fill","stylers":[{"saturation":36},{"color":"#000000"},{"lightness":40}]},{"elementType":"labels.icon","stylers":[{"visibility":"off"}]},{"featureType":"transit","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":19}]},{"featureType":"administrative","elementType":"geometry.fill","stylers":[{"color":"#000000"},{"lightness":20}]},{"featureType":"administrative","elementType":"geometry.stroke","stylers":[{"color":"#000000"},{"lightness":17},{"weight":1.2}]}]
        },

        // Create the map container
        mapContainer = document.createElement('div');
        mapContainer.id = 'map';

    // Add the map container inside the contacts section
    document.getElementById('contacts').appendChild(mapContainer);

    // Create the Google Map using the map container and options defined above
    var map = new google.maps.Map(document.getElementById('map'), mapOptions);

}

/*----------------------------------------------------------------------------*/
/*      When the window has finished loading create your Google Map
/*----------------------------------------------------------------------------*/
google.maps.event.addDomListener(window, 'load', init);