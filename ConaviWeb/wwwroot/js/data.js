var clave_nacional = '00';
var buttons = ['excel', 'pdf'];

// setOption map
var tooltip = {
    trigger: 'item',
    formatter: '{b}<br/>{c}'
};
var toolbox = {
    show: true,
    orient: 'vertical',
    left: 'right',
    top: 'center',
    showTitle: false,
    feature: {
        saveAsImage: {}
    }
};

// setOption pie chart
var tooltip2 = {
    trigger: 'item'
};
var toolbox2 = {
    show: true,
    orient: 'vertical',
    left: 'right',
    top: 'center',
    showTitle: false,
    feature: {
        magicType: { show: true, type: ['pie', 'funnel'] },
        restore: { show: true },
        saveAsImage: { show: true }
    }
};

// functions
Array.prototype.sum = Array.prototype.sum || function () {
    return this.reduce(function (sum, a) { return sum + Number(a) }, 0);
}
Array.prototype.average = Array.prototype.average || function () {
    return this.sum() / (this.length || 1);
}

function percentageChange(v1, v2) {
    return (((v1 - v2) / v2) * 100).toFixed(2);
}
