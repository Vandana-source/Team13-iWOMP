// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

///<summary>
/// map libre
///</summary>
const map = new maplibregl.Map({

    // container id
    container: 'map', 

    // MapTiler style URL with key
    style: 'https://api.maptiler.com/maps/streets-v2/style.json?key=K4IpGwkgz6Fw1K2vSoDR', 

    // [longitude, latitude]
    center: [-122.3212675,47.6033376], 
    
    // starting zoom level
    zoom: 10 
});



// Adds the nav controls for the map 
map.addControl(new maplibregl.NavigationControl());