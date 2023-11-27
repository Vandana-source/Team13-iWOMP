// Create a new map instance
const map = new maplibregl.Map({
    // Container ID where the map will be displayed
    container: 'map',

    // MapTiler style URL with API key
    style: 'https://api.maptiler.com/maps/30dd37d7-d50c-4872-9800-73b00ff17c52/style.json?key=K4IpGwkgz6Fw1K2vSoDR',

    // Initial center coordinates [longitude, latitude]
    center: [-122.31790014721174, 47.60930666870316],

    // Initial zoom level
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

        // Fetch GeoJSON data from your ASP.NET Core API endpoint
        fetch('/api/Map/geojson')
            .then(response => response.json())
            .then(geoJSONData => {
                // Add the retrieved GeoJSON data as a source
                map.addSource('myGeoJSON', {
                    type: 'geojson',
                    data: geoJSONData // Use the fetched GeoJSON data here
                });

                // Add a layer to display the GeoJSON features
                map.addLayer({
                    id: 'Restrooms',
                    type: 'symbol',
                    source: 'myGeoJSON',
                    layout: {
                        'icon-image': 'Custom_icon',
                        'icon-size': 0.05 // Adjust size as needed
                    }
                });
            })
            .catch(error => {
                console.error('Failed to fetch GeoJSON data:', error);
            });
    });
});

// Add navigation controls to the map
map.addControl(new maplibregl.NavigationControl());
