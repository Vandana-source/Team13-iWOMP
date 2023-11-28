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
        new maplibregl.Marker(markerElement, {
            anchor: 'bottom'
        })
            .setLngLat(coordinates)
            .addTo(map);

        // Optional: Add popup or event listeners to the marker here
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
