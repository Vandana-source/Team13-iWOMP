// Function to create and add custom markers
// Global variables to store start and end points
let startPoint = null;
let endPoint = null;

// Create a new map instance
const map = new maplibregl.Map({
    container: 'map',
    style: 'https://api.maptiler.com/maps/30dd37d7-d50c-4872-9800-73b00ff17c52/style.json?key=K4IpGwkgz6Fw1K2vSoDR',
    center: [-122.31790014721174, 47.60930666870316],
    zoom: 11
});

// Function to create and add custom markers
function addCustomMarkers(geojsonData) {
    geojsonData.features.forEach(feature => {
        const type = feature.properties.LocationType;
        const coordinates = feature.geometry.coordinates;

        // Create a div element for the marker
        const markerElement = document.createElement('div');
        markerElement.className = type.toLowerCase() + '-icon'; // e.g., 'bench-icon'

        // Create the marker and set its properties
        new maplibregl.Marker(markerElement, { anchor: 'bottom' })
            .setLngLat(coordinates)
            .addTo(map);

        // Add click event listener to each marker
        markerElement.addEventListener('click', () => {
            if (!startPoint) {
                // Set the start point
                startPoint = coordinates;
            } else {
                // Set the end point and draw the route
                endPoint = coordinates;
                drawRoute(startPoint, endPoint);

                // Reset the start and end points for the next route
                startPoint = null;
                endPoint = null;
            }
        });
    });
}
// Function to update the location type filter
function updateLocationType(locationType) {
    // Select all markers
    const allMarkers = document.querySelectorAll('.bench-icon, .restroom-icon, .table-icon, .other-icon');

    allMarkers.forEach(marker => {
        if (locationType === null) {
            // Show all markers
            marker.style.display = 'block';
        } else {
            // Show markers based on the selected type
            if (marker.classList.contains(locationType.toLowerCase() + '-icon')) {
                marker.style.display = 'block';
            } else {
                marker.style.display = 'none';
            }
        }
    });

    // Update the button text
    const selectedLocationTypeText = locationType === null ? 'All' : locationType;
    document.getElementById('selectedLocationType').innerText = selectedLocationTypeText;
}

// Event handler when the map style loads
map.on('load', function () {
    // Assuming geojsonData is already defined and available
    addCustomMarkers(geojsonData);

    // Add navigation controls to the map
    map.addControl(new maplibregl.NavigationControl());
});

// Function to draw a route on the map
// Function to draw a route on the map using Geoapify API
// Function to draw a route on the map using Geoapify API
function drawRoute(start, end) {
    const requestOptions = {
        method: 'GET',
    };

    const startCoords = `${start[1]},${start[0]}`; // Latitude, Longitude
    const endCoords = `${end[1]},${end[0]}`; // Latitude, Longitude

    fetch(`https://api.geoapify.com/v1/routing?waypoints=${startCoords}%7C${endCoords}&mode=drive&apiKey=1ca889777476478d8b34a21892745067`, requestOptions)
        .then(response => response.json())
        .then(result => {
            const route = result.features[0]; // Assuming the route is the first feature
            if (map.getSource('route')) {
                map.getSource('route').setData(route);
            } else {
                map.addLayer({
                    id: 'route',
                    type: 'line',
                    source: {
                        type: 'geojson',
                        data: route
                    },
                    layout: {
                        'line-join': 'round',
                        'line-cap': 'round'
                    },
                    paint: {
                        'line-color': '#1DB7DD',
                        'line-width': 5
                    }
                });
            }
        })
        .catch(error => console.error('Error fetching route:', error));
}

// Function to clear the route from the map
map.getContainer().addEventListener('click', (e) => {
    if (e.target.className.includes('-icon')) return; // Ignore clicks on icons
    if (map.getSource('route')) {
        map.removeLayer('route');
        map.removeSource('route');
    }
});
