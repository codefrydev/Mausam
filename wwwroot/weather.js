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
    console.log('[Chart] Initializing temperature chart with data:', data);
    
    const canvas = document.getElementById('temperatureChart');
    if (!canvas) {
        console.error('[Chart] Temperature chart canvas not found');
        return;
    }
    
    if (typeof Chart === 'undefined') {
        console.error('[Chart] Chart.js not loaded');
        return;
    }
    
    try {
        const existing = Chart.getChart(canvas);
        if (existing) {
            console.log('[Chart] Destroying existing temperature chart');
            existing.destroy();
        }
        
        const chart = new Chart(canvas.getContext('2d'), {
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
        
        console.log('[Chart] Temperature chart initialized successfully');
    } catch (error) {
        console.error('[Chart] Error initializing temperature chart:', error);
    }
};

window.initializePrecipitationChart = (data) => {
    console.log('[Chart] Initializing precipitation chart with data:', data);
    
    const canvas = document.getElementById('precipitationChart');
    if (!canvas) {
        console.error('[Chart] Precipitation chart canvas not found');
        return;
    }
    
    if (typeof Chart === 'undefined') {
        console.error('[Chart] Chart.js not loaded');
        return;
    }
    
    try {
        const existing = Chart.getChart(canvas);
        if (existing) {
            console.log('[Chart] Destroying existing precipitation chart');
            existing.destroy();
        }
        
        const chart = new Chart(canvas.getContext('2d'), {
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
        
        console.log('[Chart] Precipitation chart initialized successfully');
    } catch (error) {
        console.error('[Chart] Error initializing precipitation chart:', error);
    }
};

window.initializeWindChart = (data) => {
    console.log('[Chart] Initializing wind chart with data:', data);
    
    const canvas = document.getElementById('windChart');
    if (!canvas) {
        console.error('[Chart] Wind chart canvas not found');
        return;
    }
    
    if (typeof Chart === 'undefined') {
        console.error('[Chart] Chart.js not loaded');
        return;
    }
    
    try {
        const existing = Chart.getChart(canvas);
        if (existing) {
            console.log('[Chart] Destroying existing wind chart');
            existing.destroy();
        }
        
        const chart = new Chart(canvas.getContext('2d'), {
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
        
        console.log('[Chart] Wind chart initialized successfully');
    } catch (error) {
        console.error('[Chart] Error initializing wind chart:', error);
    }
};

window.initializeHumidityChart = (data) => {
    console.log('[Chart] Initializing humidity chart with data:', data);
    
    const canvas = document.getElementById('humidityChart');
    if (!canvas) {
        console.error('[Chart] Humidity chart canvas not found');
        return;
    }
    
    if (typeof Chart === 'undefined') {
        console.error('[Chart] Chart.js not loaded');
        return;
    }
    
    try {
        const existing = Chart.getChart(canvas);
        if (existing) {
            console.log('[Chart] Destroying existing humidity chart');
            existing.destroy();
        }
        
        const chart = new Chart(canvas.getContext('2d'), {
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
        
        console.log('[Chart] Humidity chart initialized successfully');
    } catch (error) {
        console.error('[Chart] Error initializing humidity chart:', error);
    }
};

// Chart.js loading utility
window.waitForChartJs = () => {
    return new Promise((resolve) => {
        if (typeof Chart !== 'undefined') {
            resolve();
            return;
        }
        
        const checkInterval = setInterval(() => {
            if (typeof Chart !== 'undefined') {
                clearInterval(checkInterval);
                resolve();
            }
        }, 100);
        
        // Timeout after 5 seconds
        setTimeout(() => {
            clearInterval(checkInterval);
            resolve();
        }, 5000);
    });
};

// Legacy function for backward compatibility
window.initializeChart = window.initializeTemperatureChart;