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

// Responsive helper functions
window.getWindowWidth = () => {
    return window.innerWidth;
};

window.addEventListener('resize', () => {
    // Dispatch custom event for responsive updates
    window.dispatchEvent(new CustomEvent('responsivechange', {
        detail: { width: window.innerWidth }
    }));
});

// Chart initialization functions
window.initializeTemperatureChart = (data) => {
    const canvas = document.getElementById('temperatureChart');
    if (!canvas || typeof Chart === 'undefined') return;
    
    const existing = Chart.getChart(canvas);
    if (existing) existing.destroy();
    
    new Chart(canvas.getContext('2d'), {
        type: 'line',
        data: {
            labels: data.labels,
            datasets: [{
                label: 'Temperature (°C)',
                data: data.data,
                borderColor: '#ef4444',
                backgroundColor: 'rgba(239, 68, 68, 0.1)',
                borderWidth: 2,
                tension: 0.4,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: {
                x: { ticks: { color: '#64748b', maxTicksLimit: 8 }, grid: { color: 'rgba(148, 163, 184, 0.1)' } },
                y: { ticks: { color: '#64748b' }, grid: { color: 'rgba(148, 163, 184, 0.1)' } }
            }
        }
    });
};

window.initializePrecipitationChart = (data) => {
    const canvas = document.getElementById('precipitationChart');
    if (!canvas || typeof Chart === 'undefined') return;
    
    const existing = Chart.getChart(canvas);
    if (existing) existing.destroy();
    
    new Chart(canvas.getContext('2d'), {
        type: 'bar',
        data: {
            labels: data.labels,
            datasets: [{
                label: 'Precipitation (mm)',
                data: data.data,
                backgroundColor: '#3b82f6',
                borderColor: '#1d4ed8',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: {
                x: { ticks: { color: '#64748b', maxTicksLimit: 8 }, grid: { color: 'rgba(148, 163, 184, 0.1)' } },
                y: { ticks: { color: '#64748b' }, grid: { color: 'rgba(148, 163, 184, 0.1)' } }
            }
        }
    });
};

window.initializeWindChart = (data) => {
    const canvas = document.getElementById('windChart');
    if (!canvas || typeof Chart === 'undefined') return;
    
    const existing = Chart.getChart(canvas);
    if (existing) existing.destroy();
    
    new Chart(canvas.getContext('2d'), {
        type: 'radar',
        data: {
            labels: data.labels,
            datasets: [{
                label: 'Wind Speed (km/h)',
                data: data.data,
                borderColor: '#10b981',
                backgroundColor: 'rgba(16, 185, 129, 0.2)',
                borderWidth: 2,
                pointBackgroundColor: '#10b981'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: {
                r: {
                    ticks: { color: '#64748b', stepSize: 5 },
                    grid: { color: 'rgba(148, 163, 184, 0.1)' },
                    pointLabels: { color: '#64748b' }
                }
            }
        }
    });
};

window.initializeHumidityChart = (data) => {
    const canvas = document.getElementById('humidityChart');
    if (!canvas || typeof Chart === 'undefined') return;
    
    const existing = Chart.getChart(canvas);
    if (existing) existing.destroy();
    
    new Chart(canvas.getContext('2d'), {
        type: 'doughnut',
        data: {
            labels: ['Humidity', 'Dry Air'],
            datasets: [{
                data: [data.humidity, 100 - data.humidity],
                backgroundColor: ['#8b5cf6', '#e5e7eb'],
                borderWidth: 0
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            cutout: '70%'
        }
    });
};

// Legacy function for backward compatibility
window.initializeChart = window.initializeTemperatureChart;