window.chartInstances = window.chartInstances || {};
window.chartjsReady = false;
window.chartjsDateAdapterFailed = false;

// Function to wait for Chart.js to be ready
window.waitForChartjsReady = () => {
    return new Promise((resolve) => {
        if (window.chartjsReady) {
            resolve();
        } else {
            const checkReady = () => {
                if (window.chartjsReady) {
                    resolve();
                } else {
                    setTimeout(checkReady, 100);
                }
            };
            checkReady();
        }
    });
};

window.getChartTheme = () => {
    // Find the FluentUI boundary element instead of document root
    const fluentProvider = document.querySelector('fluent-design-system-provider') ||
        document.querySelector('.app-container') ||
        document.documentElement;
    const rootStyles = getComputedStyle(fluentProvider);
    return {
        primaryBorder: rootStyles.getPropertyValue('--accent-fill-rest').trim(),
        primaryBackground: rootStyles.getPropertyValue('--accent-fill-rest').trim() + '30',
        gridColor: rootStyles.getPropertyValue('--neutral-stroke-rest').trim(),
        textColor: rootStyles.getPropertyValue('--neutral-foreground-rest').trim()
    };
};

window.createChart = (canvasId, config) => {
    console.log('createChart called for:', canvasId);
    const ctx = document.getElementById(canvasId);
    if (!ctx) {
        console.error('Canvas not found:', canvasId);
        return null;
    }

    // Apply FluentUI theme colors
    const theme = window.getChartTheme();
    if (config.data?.datasets) {
        config.data.datasets.forEach(dataset => {
            dataset.borderColor = theme.primaryBorder;
            dataset.backgroundColor = theme.primaryBackground;
        });
    }

    // Apply theme to grid and text
    if (config.options?.scales) {
        if (config.options.scales.x) {
            config.options.scales.x.grid = { ...config.options.scales.x.grid, color: theme.gridColor };
            config.options.scales.x.ticks = { ...config.options.scales.x.ticks, color: theme.textColor };
        }
        if (config.options.scales.y) {
            config.options.scales.y.grid = { ...config.options.scales.y.grid, color: theme.gridColor };
            config.options.scales.y.ticks = { ...config.options.scales.y.ticks, color: theme.textColor };
        }
    }

    // For WPF/WebView, we'll use linear scale instead of time scale to avoid adapter issues
    if (config.options?.scales?.x?.type === 'time') {
        console.log('Converting time scale to linear scale for WebView compatibility');
        config.options.scales.x = {
            type: 'linear',
            title: config.options.scales.x.title || { display: true, text: 'Time' },
            grid: config.options.scales.x.grid || { display: true }
        };
        
        // Convert time data to numeric timestamps
        if (config.data?.datasets) {
            config.data.datasets.forEach(dataset => {
                if (dataset.data && Array.isArray(dataset.data)) {
                    dataset.data = dataset.data.map(point => {
                        if (point.x && typeof point.x === 'string') {
                            const timestamp = new Date(point.x).getTime();
                            return {
                                x: timestamp,
                                y: point.y
                            };
                        }
                        return point;
                    });
                }
            });
        }
    }

    // Destroy existing chart if it exists
    if (window.chartInstances[canvasId]) {
        window.chartInstances[canvasId].destroy();
        delete window.chartInstances[canvasId];
    }

    try {
        window.chartInstances[canvasId] = new Chart(ctx, config);
        console.log('Chart created successfully:', canvasId);
        return window.chartInstances[canvasId];
    } catch (error) {
        console.error('Error creating chart:', error);
        return null;
    }
};

window.addDataPoint = (canvasId, datasetIndex, newDataPoint) => {
    const chart = window.chartInstances[canvasId];
    if (!chart) {
        console.warn('Chart not found:', canvasId);
        return;
    }

    if (!chart.data.datasets[datasetIndex]) {
        console.warn('Dataset not found:', datasetIndex);
        return;
    }

    chart.data.datasets[datasetIndex].data.push(newDataPoint);
    const maxPoints = 1000;
    if (chart.data.datasets[datasetIndex].data.length > maxPoints) {
        chart.data.datasets[datasetIndex].data.shift();
    }

    chart.data.datasets[datasetIndex].data.sort((a, b) => a.x - b.x);
    chart.update('none');
};

window.updateChart = (canvasId, newData) => {
    if (window.chartInstances[canvasId]) {
        window.chartInstances[canvasId].data = newData;
        window.chartInstances[canvasId].update();
    }
};

window.destroyChart = (canvasId) => {
    if (window.chartInstances[canvasId]) {
        window.chartInstances[canvasId].destroy();
        delete window.chartInstances[canvasId];
    }
};

window.replaceChartData = (canvasId, newData) => {
    const chart = window.chartInstances[canvasId];
    if (!chart) return;

    chart.data = newData;
    chart.update('active');
};

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM loaded, checking for Chart.js...');
    
    function initializeCharting() {
        if (typeof Chart !== 'undefined') {
            console.log('Chart.js is available');
            window.chartjsReady = true;
            
            // Register a simple time formatter for linear scales
            if (Chart.defaults) {
                Chart.defaults.scales = Chart.defaults.scales || {};
                Chart.defaults.scales.linear = Chart.defaults.scales.linear || {};
                Chart.defaults.scales.linear.ticks = Chart.defaults.scales.linear.ticks || {};
                Chart.defaults.scales.linear.ticks.callback = function(value, index, values) {
                    // If this looks like a timestamp, format it as time
                    if (value > 1000000000000) { // Timestamp check
                        try {
                            return new Date(value).toLocaleTimeString();
                        } catch (e) {
                            return value;
                        }
                    }
                    return value;
                };
            }
        } else {
            console.log('Chart.js not ready yet, retrying...');
            setTimeout(initializeCharting, 100);
        }
    }
    
    initializeCharting();
});

console.log('Charting script loaded');