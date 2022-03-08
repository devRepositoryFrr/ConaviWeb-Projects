/* Bootstrap Viewer Template */
/* __________________________________________________ */
function applyMargins() {
    var leftToggler = $(".mini-submenu-left");
    if (leftToggler.is(":visible")) {
        $("#map .ol-zoom")
            .css("margin-left", 0)
            .removeClass("zoom-top-opened-sidebar")
            .addClass("zoom-top-collapsed");
    } else {
        $("#map .ol-zoom")
            .css("margin-left", $(".sidebar-left").width())
            .removeClass("zoom-top-opened-sidebar")
            .removeClass("zoom-top-collapsed");
    }
}

function isConstrained() {
    return $(".sidebar").width() === $(window).width();
}

function applyInitialUIState() {
    if (isConstrained()) {
        $(".sidebar-left .sidebar-body").fadeOut('slide');
        $('.mini-submenu-left').fadeIn();
    }
}

function loadMenu() {
    $('.sidebar-left .slide-submenu').on('click', function () {
        var thisEl = $(this);
        thisEl.closest('.sidebar-body').fadeOut('slide', function () {
            $('.mini-submenu-left').fadeIn();
            applyMargins();
        });
    });

    $('.mini-submenu-left').on('click', function () {
        var thisEl = $(this);
        $('.sidebar-left .sidebar-body').toggle('slide');
        thisEl.hide();
        applyMargins();
    });
}

function loadLayer() {
    var url = new URL($(location).attr('href'));
    var layer = url.searchParams.get('layer');
    if (layer !== null) {
        var chkOpt;
        switch (layer) {
            case 'indigenas':
                chkOpt = $('#indigenas');
                break;
            case 'sisevive':
                chkOpt = $('#sisevive');
                break;
            case 'oferta':
                chkOpt = $('#oferta');
                break;
            case 'subsidios':
                chkOpt = $('#subsidios');
                break;
            case 'financiamientos':
                chkOpt = $('#financiamientos');
                break;
            case 'pcus':
                chkOpt = $('#pcus');
                break;
            //default:
        }
        chkOpt.prop('checked');
        chkOpt.trigger('click');
    }
}
/* __________________________________________________ */

var host_path = 'https://sniiv-cors.herokuapp.com/http://sniiv.conavi.gob.mx';//window.location.origin;//'http://172.16.250.4';

$(function () {
    //console.log(host_path);
    loadMenu();

    /* Map CONAVI */
    /* __________________________________________________ */
    var view = new ol.View({
        center: ol.proj.transform([-101.5, 24.5], 'EPSG:4326', 'EPSG:3857'),
        //center: [-11312551, 2542295],
        zoom: 6,
        minZoom: 6,
        maxZoom: 16
    });

    var wmsSourceIndigenas = new ol.source.TileWMS({
        url: host_path + ':8080/geoserver/sniiv/wms',
        params: { 'LAYERS': 'sniiv:municipios_indigenas' },
        /*url: 'https://ide.sedatu.gob.mx' + ':8080/ows',
        params: {
            'LAYERS': 'geonode:marginacion_Edo_15',
            'SRS': 'EPSG:3857',
            'CRS': 'EPSG:3857',
            'TILED':'true' ,
            'WIDTH':'256' ,
            'HEIGHT':'256' ,
            'BBOX' : '- 12523442.714243278, 3757032.814272985, - 11271098.44281895, 5009377.085697313 ',
        },*/
        serverType: 'geoserver'
    });

    var wmsSourcePcus = new ol.source.TileWMS({
        url: host_path + ':8080/geoserver/sniiv/wms',
        params: { 'LAYERS': 'sniiv:pcus_2015_shp_4326' },
        serverType: 'geoserver'
    });

    var wmsSourceOferta = new ol.source.TileWMS({
        url: host_path + ':8080/geoserver/sniiv/wms',
        params: { 'LAYERS': 'sniiv:oferta201606' },
        serverType: 'geoserver'
    });

    var wmsSourceSisevive = new ol.source.TileWMS({
        url: host_path + ':8080/geoserver/sniiv/wms',
        params: { 'LAYERS': 'sniiv:sisevive' },
        serverType: 'geoserver'
    });

    /*var wmsSourceConabio = new ol.source.TileWMS({
        url: host_path + ':8080/geoserver/Conabio-Rep-Mex/wms',
        params: { 'LAYERS': 'Conabio-Rep-Mex:dest_2010gw' },
        serverType: 'geoserver'
    });*/

    var iconGeometry = new ol.geom.Point([0, 0]);
    var iconFeature = new ol.Feature({
        id: 'icon_location',
        geometry: iconGeometry
    });
    iconFeature.setStyle(new ol.style.Style({
        image: new ol.style.Icon(/** @type {olx.style.IconOptions} */({
            anchor: [0.5, 0.96],
			src: 'http://sniiv.conavi.gob.mx/img/location_prueba.png'
            //src: host_path + '/img/location_prueba.png'
        }))
    }));

    var attribution = new ol.Attribution({
        html: 'Tiles © <a href="http://services.arcgisonline.com/ArcGIS/' +
            'rest/services/World_Topo_Map/MapServer">ArcGIS</a>'
    });

    var map = new ol.Map({
        target: "map",
        layers: [
            new ol.layer.Tile({
                name: 'openstreetmap',
                visible: false,
                source: new ol.source.OSM()
            }),
            new ol.layer.Tile({
                name: 'xyxesri',
                source: new ol.source.XYZ({
                    attributions: [attribution],
                    url: 'http://server.arcgisonline.com/ArcGIS/rest/services/' +
                        'World_Topo_Map/MapServer/tile/{z}/{y}/{x}'
                })
            }),
            new ol.layer.Tile({
                name: 'bingmaproad',
                visible: false,
                source: new ol.source.BingMaps({
                    key: 'amfAnRjTIm48e7NZMldv~TuD6dffwFNPxlLOamTq9LA~ArK4pT3FVt09LAWns-qeuTYH1yEaIaUu2DsH1ua9yb92hKVdb_R7twdVina_X9gv',
                    imagerySet: 'Road'
                })
            }),
            new ol.layer.Tile({
                name: 'bingmapaerial',
                visible: false,
                source: new ol.source.BingMaps({
                    key: 'amfAnRjTIm48e7NZMldv~TuD6dffwFNPxlLOamTq9LA~ArK4pT3FVt09LAWns-qeuTYH1yEaIaUu2DsH1ua9yb92hKVdb_R7twdVina_X9gv',
                    imagerySet: 'AerialWithLabels'
                })
            }),

            new ol.layer.Group({
                layers: [
                    new ol.layer.Tile({
                        name: 'indigenas',
                        visible: false,
                        opacity: 0.8,
                        source: wmsSourceIndigenas
                    }),
                    new ol.layer.Tile({
                        name: 'pcus',
                        visible: false,
                        opacity: 0.6,
                        source: wmsSourcePcus
                    }),
                    new ol.layer.Tile({
                        name: 'oferta',
                        visible: false,
                        source: wmsSourceOferta
                    }),
                    new ol.layer.Tile({
                        name: 'sisevive',
                        visible: false,
                        source: wmsSourceSisevive
                    }),
                    /*new ol.layer.Tile({
                        name: 'conabio',
                        visible: true,
                        source: wmsSourceConabio
                    })*/
                    new ol.layer.Vector({
                        name: 'icon_location',
                        source: new ol.source.Vector({ features: [iconFeature] })
                    })
                ]
            })
        ],
        view: view
    });

    /* Geocoder Instantiate with some options and add the Control */
    /*
    - Nominatim not suport autocomplate
    - Countrycodes only valid for osm and mapquest

    provider: 'mapquest',
    key: 'odQ5hwRs5RWZO1b1GEGECnKkZ4tpBp7S',

    provider: 'photon',

    provider: 'bing',
    key: 'amfAnRjTIm48e7NZMldv~TuD6dffwFNPxlLOamTq9LA~ArK4pT3FVt09LAWns-qeuTYH1yEaIaUu2DsH1ua9yb92hKVdb_R7twdVina_X9gv',

    provider: 'pelias',
    key: 'mapzen-Veo8bp7',
    */
    /*var geocoder = new Geocoder('nominatim', {
        provider: 'google',
        key: 'AIzaSyC09E3_m_YiP5yaRMfUPoXwG6AIB9Kg5X8',
        lang: 'es',
        countrycodes: 'mx',
        placeholder: 'Buscar',
        targetType: 'text-input',
        limit: 5,
        debug: true,
        autoComplete: true,
        keepOpen: true
    });
    map.addControl(geocoder);*/

    /* Options Bootstrap Viewer Template */
    $(window).on("resize", applyMargins);
    //applyInitialUIState();
    applyMargins();

    /* Popup */
    var popup = new ol.Overlay({
        element: document.getElementById('popup')
    });
    map.addOverlay(popup);

    $.get(path + 'CatalogoAPI/GetFechasMapa').done(function (json) {
        //console.log(json);
        $('#lbl-indigenas').popover({
            trigger: 'hover',
            placement: 'top',
            title: json[0].descripcion,
            html: true,
            content: '<blockquote>'
                + '<span class="glyphicon glyphicon-stop" style="color:#f8d7da" aria-hidden="true"></span> 10% o menos<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#e9c2c5" aria-hidden="true"></span> 10.1% - 20%<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#daadb1" aria-hidden="true"></span> 20.1% - 30%<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#cb989d" aria-hidden="true"></span> 30.1% - 40%<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#bc8389" aria-hidden="true"></span> 40.1% - 50%<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#ad6f74" aria-hidden="true"></span> 50.1% - 60%<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#9e5a60" aria-hidden="true"></span> 60.1% - 70%<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#8f454c" aria-hidden="true"></span> 70.1% - 80%<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#803038" aria-hidden="true"></span> 80.1% - 90%<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#721c24" aria-hidden="true"></span> Más de 90%'
                + '</blockquote>'
        });
        $('#lbl-pcu').popover({
            trigger: 'hover',
            placement: 'top',
            title: json[1].descripcion,
            html: true,
            content: '<blockquote>'
                + '<span class="glyphicon glyphicon-stop" style="color:#FF0000" aria-hidden="true"></span> U1A<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#730000" aria-hidden="true"></span> U1B<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#4CE600" aria-hidden="true"></span> U2A<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#267300" aria-hidden="true"></span> U2B<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#0070FF" aria-hidden="true"></span> U3'
                + '</blockquote>'
        });
        $('#lbl-oferta').popover({
            trigger: 'hover',
            placement: 'top',
            title: json[2].descripcion,
            html: true,
            content: '<blockquote>'
                + '<span class="glyphicon glyphicon-stop" style="color:#268fff" aria-hidden="true"></span> Hasta 99 viviendas<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#004085" aria-hidden="true"></span> 100 o más viviendas'
                + '</blockquote>'
        });
        $('#lbl-sisevive').popover({
            trigger: 'hover',
            placement: 'top',
            title: json[3].descripcion,
            html: true,
            content: '<blockquote>'
                + '<span class="glyphicon glyphicon-stop" style="color:#48b461" aria-hidden="true"></span> Hasta 99 viviendas<br />'
                + '<span class="glyphicon glyphicon-stop" style="color:#155724" aria-hidden="true"></span> 100 o más viviendas'
                + '</blockquote>'
        });
    });

    /*
    #004085
    #007bff
    #268fff

    #155724
    #28a745
    #48b461
    */

    /* __________________________________________________ */

    /* Map Events */
    /* __________________________________________________ */
    //Search location
    $("#geocomplete").geocomplete({
        //types: ['establishment'],
        country: 'mx'
    });
    $("#geocomplete").bind("geocode:result", function (event, result) {
        //console.log(result.geometry.location.lat());
        var point = ol.proj.transform([result.geometry.location.lng(), result.geometry.location.lat()], 'EPSG:4326', 'EPSG:3857');
        iconGeometry.setCoordinates(point);
        map.getView().setCenter(point);
        map.getView().setZoom(14);
    });

    //Change Map
    $('.list-group-item').on('click', function () {
        var $this = $(this);
        var visible;
        $('.active').removeClass('active');
        $this.toggleClass('active');
        map.getLayers().forEach(function (layer) {
            if (!(layer instanceof ol.layer.Group)) {
                visible = layer.get('name') === $this.attr('id');
                layer.setVisible(visible);
            }
        });
    });

    //Change Layer
    $('#capas :checkbox').click(function () {
        var $this = $(this);
        var visible;
        map.getLayers().forEach(function (layer) {
            if (layer instanceof ol.layer.Group) {
                layer.getLayers().forEach(function (sublayer) {
                    if (sublayer.get('name') === $this.attr('id')) {
                        visible = $this.is(':checked');
                        sublayer.setVisible(visible);
                    }
                });
            }
        });
    });

    //Click Layer Point
    map.on('singleclick', function (evt) {
        var element = popup.getElement();
        $(element).popover('destroy');

        var displayedLayers = [];
        var responses = 0;
        var coordinate, url;
        var icon;

        //Cambiar a[x] en caso de quitar o agregar layers
        if (map.getLayers().a[4].U.layers.a[2].U.visible) {
            displayedLayers = [];
            responses = 0;
            coordinate = evt.coordinate;
            url = wmsSourceOferta.getGetFeatureInfoUrl(
                coordinate,
                map.getView().getResolution(),
                map.getView().getProjection(), {
                    'INFO_FORMAT': 'text/javascript',
                    'format_options': 'callback:parseResponse',
                    'propertyName': 'nombre_ofe,precio,precio_min,precio_max,viviendas'
                });
            reqwest({
                url: url,
                type: 'jsonp',
                jsonpCallbackName: 'parseResponse'
            }).then(function (data) {
                var feature = data.features[0];
                if (feature !== undefined) {
                    var props = feature.properties;
                    var content = '<div class="row">'
                        + '<div class="col-md-7"><strong><small>Precio mínimo: </small></strong></div>'
                        + '<div class="col-md-5"><small>' + formatCurrency(props.precio_min) + '</small></div>'
                        + '</div>'
                        + '<div class="row">'
                        + '<div class="col-md-7"><strong><small>Precio promedio: </small></strong></div>'
                        + '<div class="col-md-5"><small>' + formatCurrency(props.precio) + '</small></div>'
                        + '</div>'
                        + '<div class="row">'
                        + '<div class="col-md-7"><strong><small>Precio máximo: </small></strong></div>'
                        + '<div class="col-md-5"><small>' + formatCurrency(props.precio_max) + '</small></div>'
                        + '</div>'
                        + '<div class="row">'
                        + '<div class="col-md-7"><strong><small>Viviendas: </small></strong></div>'
                        + '<div class="col-md-5"><small>' + props.viviendas + '</small></div>'
                        + '</div>';
                    popup.setPosition(coordinate);

                    icon = '<span class="glyphicon glyphicon-home" aria-hidden="true"></span> ';
                    $('#popup').prop('title', icon + props.nombre_ofe);
                    $(element).popover({
                        'placement': 'top',
                        'animation': false,
                        'html': true,
                        'content': content
                    });
                    $(element).popover('show');
                }
            });
        }
        else if (map.getLayers().a[4].U.layers.a[3].U.visible) {
            displayedLayers = [];
            responses = 0;
            coordinate = evt.coordinate;
            url = wmsSourceSisevive.getGetFeatureInfoUrl(
                coordinate,
                map.getView().getResolution(),
                map.getView().getProjection(), {
                    'INFO_FORMAT': 'text/javascript',
                    'format_options': 'callback:parseResponse',
                    'propertyName': 'nombre_ofe,programa,tipologia,idg,co2,viviendas'
                });
            reqwest({
                url: url,
                type: 'jsonp',
                jsonpCallbackName: 'parseResponse'
            }).then(function (data) {
                var feature = data.features[0];
                if (feature !== undefined) {
                    var props = feature.properties;
                    var content = '<div class="row">'
                        + '<div class="col-md-4"><strong><small>Programa:</small></strong></div>'
                        + '<div class="col-md-8" style="font-size: 75%;">' + props.programa + '</div>'
                        + '</div>'
                        + '<div class="row">'
                        + '<div class="col-md-4"><strong><small>Tipologia:</small></strong></div>'
                        + '<div class="col-md-8"><small>' + props.tipologia + '</small></div>'
                        + '</div>'
                        + '<div class="row">'
                        + '<div class="col-md-4"><strong><small>IDG:</small></strong></div>'
                        + '<div class="col-md-8"><small>' + props.idg + '</small></div>'
                        + '</div>'
                        + '<div class="row">'
                        + '<div class="col-md-4"><strong><small>CO2:</small></strong></div>'
                        + '<div class="col-md-8"><small>' + props.co2 + '</small></div>'
                        + '</div>'
                        + '<div class="row">'
                        + '<div class="col-md-4"><strong><small>Viviendas:</small></strong></div>'
                        + '<div class="col-md-8"><small>' + props.viviendas + '</small></div>'
                        + '</div>';
                    popup.setPosition(coordinate);

                    icon = '<span class="glyphicon glyphicon-leaf" aria-hidden="true"></span> ';
                    $('#popup').prop('title', icon + props.nombre_ofe);
                    $(element).popover({
                        'placement': 'top',
                        'animation': false,
                        'html': true,
                        'content': content
                    });
                    $(element).popover('show');
                }
            });
        }
    });

    /*map.on('pointermove', function (evt) {
        if (evt.dragging) {
            return;
        }
        var pixel = map.getEventPixel(evt.originalEvent);
        var hit = map.forEachLayerAtPixel(pixel, function () {
            return true;
        });
        map.getTargetElement().style.cursor = hit ? 'pointer' : '';
    });*/
    /* __________________________________________________ */

    /* Functions */
    function formatCurrency(value) {
        return '$' + value.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    }

    loadLayer();
});