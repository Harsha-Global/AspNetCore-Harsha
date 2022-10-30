const prices = []; //new values will be added dynamically from server feed
const labels = []; //new values will be added dynamically from server feed

//create chart date
const chartData = {
    labels: labels,
    datasets: [{
        label: 'Stock price',
        data: prices,
        fill: false,
        borderColor: '#0a8ac6',
        tension: 0.1
    }]
};

//create chart options
const chartOptions = {
    scales: {
        x: {
            ticks: { display: false } //don't show x axis labels
        }
    },
    plugins: { legend: { display: false } }, //don't show legend for x axis
    tooltips: { mode: 'index' } //show price value as tooltip on hover on data points in the chart
};

const ctx = document.getElementById('stock-chart').getContext('2d');

//line chart
const chart = new Chart(ctx, { type: 'line', data: chartData, options: chartOptions });

//display current date
var d = new Date();
document.querySelector(".date").innerText = d.toLocaleDateString();
