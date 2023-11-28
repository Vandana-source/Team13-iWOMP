// Create a new map instance
const map = new maplibregl.Map({
    container: 'map',
    style: 'https://api.maptiler.com/maps/30dd37d7-d50c-4872-9800-73b00ff17c52/style.json?key=K4IpGwkgz6Fw1K2vSoDR',
    center: [-122.31790014721174, 47.60930666870316],
    zoom: 11
});

// Event handler when the map style loads
map.on('load', function () {
    console.log('Map loaded');

    // Load a custom icon image
    map.loadImage('https://api.geoapify.com/v1/icon/?type=awesome&color=%233dd448&size=large&icon=restroom&scaleFactor=2&apiKey=1ca889777476478d8b34a21892745067', function (error, image) {
        if (error) {
            console.error('An error occurred while loading the image:', error);
            return; // Exit the function if the image cannot be loaded
        }

        // Add the custom icon image to the map
        map.addImage('Custom_icon', image);

        // Add the GeoJSON source
        map.addSource('myGeoJSON', {
            type: 'geojson',
            data: geojsonData
        });

        // Add a layer to display the GeoJSON features specifically for restrooms
        map.addLayer({
            id: 'Restroom',
            type: 'symbol',
            source: 'myGeoJSON',
            layout: {
                'icon-image': 'Custom_icon',
                'text-field': ['get', 'Title'], // Display the 'Title' property as text
                'text-size': 12,
                'text-offset': [0, 1.5],
                'icon-size': 0.05
            },
            filter: ['==', ['downcase', ['get', 'LocationType']], 'restroom']
        });

        // Loop through your GeoJSON data to add markers
        geojsonData.features.forEach(feature => {
            if (feature.properties && feature.properties.LocationType && feature.geometry && feature.geometry.type === "Point") {
                if (feature.properties.LocationType.toLowerCase() === 'restroom') {
                    const coords = feature.geometry.coordinates;

                    const restroomIcon = document.createElement('div');
                    restroomIcon.className = 'restroom';

                    new maplibregl.Marker(restroomIcon, {
                        anchor: 'bottom',
                        offset: [0, -34.5] // Adjust as needed for the marker's anchor point
                    })
                        .setLngLat(coords)
                        .addTo(map);
                }
            }
        });
    });
});

// Add navigation controls to the map
map.addControl(new maplibregl.NavigationControl());
