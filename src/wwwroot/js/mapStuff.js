// Create a new map instance
const map = new maplibregl.Map({
    container: 'map',
    style: 'https://api.maptiler.com/maps/30dd37d7-d50c-4872-9800-73b00ff17c52/style.json?key=K4IpGwkgz6Fw1K2vSoDR',
    center: [-122.31790014721174, 47.60930666870316],
    zoom: 11
});

// Event handler when the map loads
map.on('load', function () {
    // Load a custom icon image
    map.loadImage('https://cdn-icons-png.flaticon.com/512/684/684908.png', function (error, image) {
        if (error) {
            throw error;
        }

        // Add the custom icon image to the map
        map.addImage('Custom_icon', image);

        // Use the GeoJSON data passed from the Razor Page
        // Make sure the geojsonData variable is declared in the Razor Page
        map.addSource('myGeoJSON', {
            type: 'geojson',
            data: geojsonData
        });

        // Add a layer to display the GeoJSON features
        map.addLayer({
            id: 'Restrooms',
            type: 'symbol',
            source: 'myGeoJSON',
            layout: {
                'icon-image': 'Custom_icon',
                'icon-size': 0.05
            }
        });
    });
});

// Add navigation controls to the map
map.addControl(new maplibregl.NavigationControl());
