var lstMes = ['enero', 'febrero', 'marzo', 'abril', 'mayo', 'junio', 'julio', 'agosto', 'septiembre', 'octubre', 'noviembre', 'diciembre'];
var lstPCU = ['U1', 'U2', 'U3', 'FC'];
var lstSegmentoUMA = ['0 hasta 60', 'Mayor o igual 60 hasta 136', 'Mayor o igual 136 hasta 158', 'Mayor a 158 hasta 175', 'Mayor a 175 hasta 190', 'Mayor a 190'];
var limite = 4;
var anio_inicio, anio_fin;
var nota_rango_salarial = 'Rango salarial VSM cambia a UMA a partir de 2017';

$(function () {
    exportarTabla();
    mostrarDimensiones();
    $("#btn_aceptar").click(function (e) {
        mostrarCubo();
    });
    mostrarCubo(true);

    $(".ui-sortable-handle").draggable();
});

function mostrarDimensiones() {
    $('.ddl_dimensiones').select2({
        placeholder: 'Seleccione un máximo de cuatro variables',
        //maximumSelectionLength: limite,
        language: 'es'
    }).on('select2:open', function (e) {
        var dimensiones = $(this);
        anio_inicio = $('[id$=ddl_inicio]').val();
        anio_fin = $('[id$=ddl_fin]').val();
        
        if (dimensiones.val() !== null) {
            if (anio_inicio === anio_fin || anio_fin <= 12 ) {
                console.log('anio: ' + anio_inicio + ' mes: ' + anio_fin);
                if (dimensiones.val().length === limite) {
                    dimensiones.select2("close");
                    mostrarMensaje('Sólo puede seleccionar ' + limite + ' variables');
                }
            }
            else {
                if (dimensiones.val().length === (limite - 1)) {
                    console.log('anio2: ' + anio_inicio + ' mes2: ' + anio_fin+' dimensiones: '+dimensiones.val());
                    dimensiones.select2("close");
                    mostrarMensaje('Sólo puede seleccionar ' + limite + ' variables');
                }
            }
        }
    });
    //.val(['estado', 'pcu']).trigger("change");
}

function traerAnios(anio_inicio, anio_fin) {
    var anios = [];
    anios.push(anio_inicio);
    anios.push(anio_fin);
    return anios;
}

function traerDimensionesHidden(dimensionesSelect, anio_inicio, anio_fin) {
    var hidden = dimensiones.slice();
    $.each(dimensionesSelect, function (index, value) {
        hidden.splice($.inArray(value, hidden), 1);
    });
    hidden.push('acciones');
    hidden.push('monto');
    hidden.push('viviendas');
    if ((anio_inicio === anio_fin) || (anio_inicio.length !== anio_fin.length))
        hidden.push('año');
    return hidden;
}

function validarvariables() {
    if ($('.ddl_dimensiones').select2().val().length === limite) {
        mostrarMensaje('Sólo puede seleccionar ' + limite + ' variables');
        $('[id$=ddl_fin]').val($('[id$=ddl_inicio]').val());
    }
}

function mostrarMensaje(msj) {
    $('.lbl_msj').text(msj);
    $('.alert').show();
    $('.alert').delay(3000).fadeOut('slow');
}

function exportarTabla(nombre_archivo) {
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
            mostrarMensaje('Por el momento solo puede descargar tablas');
        }
    });
}