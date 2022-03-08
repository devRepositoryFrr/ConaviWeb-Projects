/*
 Funciones utiles
 */
var host_path = window.location.origin + '/';
var path = window.location.origin + '/api/';
var mapPath = window.location.origin + '/doc';
//['#e41a1c', '#377eb8', '#4daf4a', '#984ea3', '#ff7f00', '#ffff33', '#a65628']
//var baseColor = ['#c23531', '#2f4554', '#61a0a8', '#d48265', '#91c7ae', '#749f83', '#ca8622', '#bda29a', '#6e7074', '#546570', '#c4ccd3'];
//var baseColor = ['#447c69', '#74c493', '#8e8c6d', '#e4bf80', '#e9d78e', '#e2975d', '#f19670', '#e16552', '#c94a53', '#be5168', '#a34974', '#993767', '#65387d', '#4e2472', '#9163b6', '#e279a3', '#e0598b', '#7c9fb0', '#5698c4', '#9abf88'];
/*var baseColor = [
    '#c23531',
    '#2f4554',
    '#61a0a8',
    '#d48265',
    '#91c7ae',
    '#749f83',
    '#ca8622',
    '#bda29a',
    '#6e7074',
    '#546570',
    '#c4ccd3'
];*/

var baseColor = [
    '#dd6b66',
    '#759aa0',
    '#e69d87',
    '#8dc1a9',
    '#ea7e53',
    '#eedd78',
    '#73a373',
    '#73b9bc',
    '#7289ab',
    '#91ca8c',
    '#f49f42'
];

/*
var baseColor = [
    '#37A2DA',
    '#32C5E9',
    '#67E0E3',
    '#9FE6B8',
    '#FFDB5C',
    '#ff9f7f',
    '#fb7293',
    '#E062AE',
    '#E690D1',
    '#e7bcf3',
    '#9d96f5',
    '#8378EA',
    '#96BFFF'
];
*/

/*
var baseColor = [
    "#5470c6",
    "#91cc75",
    "#fac858",
    "#ee6666",
    "#73c0de",
    "#3ba272",
    "#fc8452",
    "#9a60b4",
    "#ea7ccc"
];
*/

/*
var baseColor = [
    "#3fb1e3",
    "#6be6c1",
    "#626c91",
    "#a0a7e6",
    "#c4ebad",
    "#96dee8"
];
*/

//var baseColor = ['#4bb573', '#f79b92', '#f3c88f', '#f0e275', '#5beedc', '#c5c5c5', '#8db1c3', '#c5b591', '#f178a9'];
//var rangeColor = ['#f6f9c2', '#74c493', '#447C69'];
var rangeColor = ['#EDF8E9', '#41AB5D', '#005A32'];
/*var rangeColor = [
    "#bf444c",
    "#d88273",
    "#f6efa6"
];*/
//var rangeColor = ["#f7fcf0", "#e0f3db", "#ccebc5", "#a8ddb5", "#7bccc4", "#4eb3d3", "#5470c6"];
//['#fff3cd', '#dc3545'];
//'#51574a', '#447c69', '#74c493', '#8e8c6d', '#e4bf80', '#e9d78e', '#e2975d', '#f19670', '#e16552', '#c94a53', '#be5168', '#a34974', '#993767', '#65387d', '#4e2472', '#9163b6', '#e279a3', '#e0598b', '#7c9fb0', '#5698c4', '#9abf88'
// #85ffd0 #6ce6c6 #acf5a8 #95fed0

var width_map = 700;
var height_map = 420;
var center_map = [-99.5, 22.5];
var color_map = 'sniiv';
var quantiles_map = 7;

var colorOn = '#333';
var colorOff = '#808080';

function changeTitle(text) {
    $('#lbl_titulo').text(text);
}

function changeSubtitle() {
    $(".lst").change(function () {
        $("#lbl_subtitulo").text($(this).find("option:selected").text());
    });
}

function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
}

function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function limitText(limitField, limitNum) {
    if (limitField.value.length > limitNum)
        limitField.value = limitField.value.substring(0, limitNum);
}

function existsUrl(url) {
    var http = new XMLHttpRequest();
    http.open('HEAD', url, false);
    http.send();
    return http.status !== 404;
}

function divToPrint(div) {
    var newWin = window.open('', 'Print-Window');
    newWin.document.open();
    newWin.document.write('<html><body onload="window.print()">' + div.innerHTML + '</body></html>');
    newWin.document.close();
    setTimeout(function () { newWin.close(); }, 10);
}

function getURLParameter(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] === sParam)
            return sParameterName[1];
    }
}