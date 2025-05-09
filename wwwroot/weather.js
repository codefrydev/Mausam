// Geolocation wrapper
window.getCurrentPosition = () => {
    return new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(
            position => resolve({
                coords: {
                    latitude: position.coords.latitude,
                    longitude: position.coords.longitude
                }
            }),
            error => reject(error)
        );
    });
};

// Chart initialization
window.initializeChart = (data) => {
    const canvas = document.getElementById('temperatureChart');
    const existing = Chart.getChart(canvas);
    if (existing) {
        existing.destroy();
    }
    const ctx = canvas.getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.labels,
            datasets: [{
                label: 'Temperature (°C)',
                data: data.data,
                borderColor: 'white',
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {legend: {display: false}},
            scales: {
                x: {ticks: {color: 'white'}},
                y: {ticks: {color: 'white'}}
            }
        }
    });
};