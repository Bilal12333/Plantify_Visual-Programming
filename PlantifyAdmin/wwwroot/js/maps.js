window.plantifyMaps = {
    map: null,
    routingControl: null,

    initMap: function (mapId, lat, lng, title) {
        if (window.plantifyMaps.map) {
            window.plantifyMaps.map.remove();
            window.plantifyMaps.map = null;
        }

        setTimeout(() => {
            const map = L.map(mapId).setView([lat, lng], 13);

            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '© OpenStreetMap contributors'
            }).addTo(map);

            const marker = L.marker([lat, lng]).addTo(map);
            marker.bindPopup(`<b>${title}</b>`).openPopup();

            window.plantifyMaps.map = map;
        }, 100);
    },

    getDirections: function (destLat, destLng) {
        if (!navigator.geolocation) {
            alert("Geolocation is not supported by your browser.");
            return;
        }

        navigator.geolocation.getCurrentPosition((position) => {
            const userLat = position.coords.latitude;
            const userLng = position.coords.longitude;

            if (window.plantifyMaps.routingControl) {
                window.plantifyMaps.map.removeControl(window.plantifyMaps.routingControl);
            }

            window.plantifyMaps.routingControl = L.Routing.control({
                waypoints: [
                    L.latLng(userLat, userLng),
                    L.latLng(destLat, destLng)
                ],
                routeWhileDragging: false,
                addWaypoints: false,
                draggableWaypoints: false,
                fitSelectedRoutes: true,
                lineOptions: {
                    styles: [{ color: '#2e7d32', weight: 5 }]
                },
                createMarker: function (i, waypoint) {
                    const icon = L.divIcon({
                        html: i === 0 ? '📍' : '🌱',
                        className: '',
                        iconSize: [30, 30]
                    });
                    return L.marker(waypoint.latLng, { icon });
                }
            }).addTo(window.plantifyMaps.map);

        }, () => {
            alert("Unable to get your location. Please allow location access.");
        });
    },

    destroyMap: function () {
        if (window.plantifyMaps.map) {
            window.plantifyMaps.map.remove();
            window.plantifyMaps.map = null;
            window.plantifyMaps.routingControl = null;
        }
    }
};