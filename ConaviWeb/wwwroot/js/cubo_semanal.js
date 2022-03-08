var lstMes = ['enero', 'febrero', 'marzo', 'abril', 'mayo', 'junio', 'julio', 'agosto', 'septiembre', 'octubre', 'noviembre', 'diciembre'];
var lstPCU = ['U1', 'U2', 'U3', 'FC'];
var lstSegmentoUMA = ['0 hasta 60', 'Mayor o igual 60 hasta 136', 'Mayor o igual 136 hasta 158', 'Mayor a 158 hasta 175', 'Mayor a 175 hasta 190', 'Mayor a 190'];
var limite = 5;
var anio_inicio, anio_fin;
var clave_estado, clave_municipio;
var variables = [];
var dimensionesSelect = [];
var digitsAfterDecimal = 0;
var prefix = '$';
var estadoControl = 1;

var nota_rango_salarial = 'Rango salarial VSM cambia a UMA a partir de 2017';

$(function () {
    mostrarEntidadFederativa(estadoControl);
    crearEventos();
    $("#btn_aceptar").click(function (e) {
        mostrarCubo();
    });
    mostrarCubo(true);

    $(".ui-sortable-handle").draggable();
});

function crearEventos() {
    /*
    $('.btn-export').click(function () {
        var file_name = $(this).attr('id');
        var opc = $('.pvtRenderer').val();
        if (opc.indexOf('Tabla') >= 0 || opc.indexOf('Mapa') >= 0) {
            $('.pvtTable').table2excel({
                //exclude: '',
                name: file_name,
                filename: file_name + '_' + new Date().toLocaleString().replace(/[\/\:\.\ ]/g, ''),
                exclude_img: true,
                exclude_links: true,
                exclude_inputs: true
            });
        }
        else {
            mostrarMensaje('Solo puede descargar tablas');
        }
    });
    */

    $('.btn-export').click(function () {
        var file_name = $(this).attr('id');
        var opc = $('.pvtRenderer').val();
        if (opc.indexOf('Tabla') >= 0 || opc.indexOf('Mapa') >= 0) {
            //window.open('data:application/vnd.ms-excel;base64;filename=exportData.xls,' + $.base64.encode($('.pvtRendererArea').html()));
            //e.preventDefault();
            var str = $.base64.encode($('.pvtRendererArea').html());
            var uri = 'data:application/vnd.ms-excel;base64,' + str;

            var downloadLink = document.createElement('a');
            downloadLink.href = uri;
            downloadLink.download = file_name + '_' + new Date().toLocaleString().replace(/[\/\:\.\ ]/g, '');

            document.body.appendChild(downloadLink);
            downloadLink.click();
            document.body.removeChild(downloadLink);
        }
        else
            mostrarMensaje('Solo puede descargar tablas');
    });

    $('[id$=dv_municipio]').hide();
    $('[id$=ddl_estado]').change(function () {
        var clave_estado = $(this).val();
        if (clave_estado === '0' || clave_estado === '00')
            $('[id$=dv_municipio]').hide();
        else
            $('[id$=dv_municipio]').show();
    });
}

function mostrarEntidadFederativa(desa) {
    $.getJSON(path + 'CatalogoAPI/GetEntidadFederativa' + '/' + desa, function (json) {
        $('#ddl_estado').empty();
        $.each(json, function (key, obj) {
            $('#ddl_estado').append($('<option></option>').attr('value', obj.id).text(obj.descripcion));
        });
    });
}

function traerAnios(anio_inicio, anio_fin) {
    var aux;
    if (anio_inicio > anio_fin) {
        aux = anio_inicio;
        anio_inicio = anio_fin;
        anio_fin = aux;
    }

    var anios = [];
    anios.push(anio_inicio);
    anios.push(anio_fin);
    return anios;
}

function traerDimensionesHidden(anio_inicio, anio_fin) {
    var hidden = [];
    hidden.push('acciones');
    hidden.push('monto');
    hidden.push('viviendas');
    hidden.push('trabajadores');
    if ((anio_inicio === anio_fin) || (anio_inicio.length !== anio_fin.length))
        hidden.push('año');
    return hidden;
}

function mostrarMensaje(msj) {
    $('.lbl_msj').text(msj);
    $('.alert').show();
    $('.alert').delay(3000).fadeOut('slow');
}

