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


map.on('load', function() {
    map.loadImage('https://cdn-icons-png.flaticon.com/512/684/684908.png', function (error, image) {
        if (error) throw error;
        map.addImage('Custom_icon', image);

        // Assuming 'data' is in the 'wwwroot' directory and is accessible
        map.addSource('myGeoJSON', {
            type: 'geojson',
            data: 'data/locations.geojson'
        });

        map.addLayer({
            id: 'Restrooms',
            type: 'symbol',
            source: 'myGeoJSON',
            layout: {
                'icon-image': 'Custom_icon',
                'icon-size': 0.05 // Adjust size as needed
            }
        });
    });
});



// Adds the nav controls for the map 
map.addControl(new maplibregl.NavigationControl());