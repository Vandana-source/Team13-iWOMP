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
    style: 'https://api.maptiler.com/maps/30dd37d7-d50c-4872-9800-73b00ff17c52/style.json?key=K4IpGwkgz6Fw1K2vSoDR',

    // [longitude, latitude]
    center: [-122.31790014721174, 47.60930666870316], 
    
    // starting zoom level
    zoom: 11 
});



// Adds the nav controls for the map 
map.addControl(new maplibregl.NavigationControl());