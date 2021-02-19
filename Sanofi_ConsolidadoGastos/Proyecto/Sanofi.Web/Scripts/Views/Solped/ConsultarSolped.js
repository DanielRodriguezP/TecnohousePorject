
var NombreReceptor = "";
var IdReceptor = 0;
var GridCreada = false;
$(document).ready(function () {

    ////Asignando kendoDatePicker al input de fecha inicial
    $('#FechaInicio').kendoDatePicker({
        language: "es",
        todayHighlight: true,
        autoclose: true,
        format: "dd/MM/yyyy"
    });

    ////Asignando kendoDatePicker al input de fecha Final
    $('#FechaFin').kendoDatePicker({
        language: "es",
        todayHighlight: true,
        autoclose: true,
        format: "dd/MM/yyyy"
    });


    //Validación
    $('#FechaFin').on('change', function () {
        Verificar_Fechas("Fin");
    });

    $('#FechaInicio').on('change', function () {
        Verificar_Fechas("Inicio");
    });

    $("#FechaFin").val('');
});

//Verificar que la fecha de Inicio siempre sea menor a la fecha Final del Evento
function Verificar_Fechas(campo) {

    var Fecha = $("#FechaInicio").val();
    var Ano = parseInt(Fecha.substring(6, 10), 10);
    var Mes = parseInt(Fecha.substring(3, 5), 10);
    var Dia = parseInt(Fecha.substring(0, 2), 10);
    var FechaIni = new Date(Ano, Mes, Dia);
    if (!isNaN(Ano) || !isNaN(Mes) || !isNaN(Dia) || !Fecha) {
        Fecha = $("#FechaFin").val();
        Ano = parseInt(Fecha.substring(6, 10), 10);
        Mes = parseInt(Fecha.substring(3, 5), 10);
        Dia = parseInt(Fecha.substring(0, 2), 10);
        var FechaFin = new Date(Ano, Mes, Dia);
        if (!isNaN(Ano) || !isNaN(Mes) || !isNaN(Dia) || !Fecha) {
            if (FechaFin < FechaIni) {
                switch (campo) {
                    case "Inicio":
                        $("#FechaInicio").val('');
                        swal({
                            text: "La fecha de inicio no puede ser mayor a la fecha de final.",
                            icon: "warning",
                        });
                        break;
                    case "Fin":
                        $("#FechaFin").val('');
                        swal({
                            text: "La fecha final no puede ser menor a la fecha de Inicio.",
                            icon: "warning",
                        });
                        break;
                }
            }
        } else {
            swal("El formato de fecha fin no corresponde con el establecido(DD/MM/YYYY).");
            $("#FechaFin").val('');
        }
    } else {
        swal("El formato de fecha inicio no corresponde con el establecido(DD/MM/YYYY).");
        $("#FechaInicio").val('');
    }
}

$("#BtnBuscar").on('click', function () {
    ConsultarSolped();
});

function ConsultarSolped(MostrarMensajeDeGridVacio = 1) {
    if ($("#FechaInicio").val() != "" && $("#FechaFin").val() != "") {
        var Datos = {};
        Datos.FechaInicial = $("#FechaInicio").val();
        Datos.FechaFinal = $("#FechaFin").val();
        $.ajax({
            type: "POST",
            url: sessionStorage.getItem("RutaActual") + '/Solped/ConsultarSolpedFiltros',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(Datos),
            success: function (data) {
                if (data.Resultado === true) {
                    if (data.Datos.length > 0) {

                    } else {
                        swal("No se encontrarón Resultados");

                    }
                    $("#gridLista").show();
                    CargarGrid(data.Datos);
                    var ReceptoresMostrar1 = new kendo.data.DataSource({
                        data: data.Datos,
                        pageSize: 10
                    });
                    $("#gridLista").data("kendoGrid").setDataSource(ReceptoresMostrar1);
                } else {
                    document.getElementById("gridListaPV").style.display = "none";
                    //var grid = $("#gridMenusNG").data("kendoGrid");
                    swal("No se encontrarón Resultados");
                }
            }
        });
    } else {
        swal("Los campos con (*), son de carácter obligatorio.");
    }
};

function CargarGrid(data) {
    if (data !== null) {
        $("#gridLista").removeClass('hide');
        $("#gridLista").kendoGrid({
            columns: [{
                title: "Sol. nro.",
                field: "SolNro"
            }, {
                title: "Número de OC",
                field: "NroOC"
            }, {
                title: "Línea de pedido nro.",
                field: "LineaPedido",

            }, {
                title: "Núm. artículos solic.",
                field: "NroArticulos"
            }, {
                title: "Número SPE (Encabezado)",
                field: "NroSPE"
            }, {
                title: "HACAT",
                field: "HACAT"
            }, {
                title: "Solicitado por (encabezado)",
                field: "PersonaSolicitud"
            }, {
                title: "Enviado el (encabezado)",
                field: "FechaEnvio",
                template: function (dataItem) {
                    if (moment(dataItem.FechaEnvio).format('DD/MM/YYYY') == '01/01/0001') {
                        return '';
                    }
                    return moment(dataItem.FechaEnvio).format('DD/MM/YYYY');
                }
            }, {
                title: "Estado (encabezado)",
                field: "Estado"
            }, {
                title: "Tipo",
                field: "Tipo"
            }, {
                title: "Artículo",
                field: "Articulo"
            },
            {
                title: "Cantidad",
                field: "Cantidad"
            },
            {
                title: "Total de la línea:",
                field: "TotalLinea"
            },
            {
                title: "Código IVA",
                field: "CodigoIVA"
            },

            {
                title: "Cuadro de cuentas",
                field: "CuadroCuentas"
            }, {
                title: "Creador (encabezado)",
                field: "PersonaCreador"
            }, {
                title: "Fecha del pedido",
                field: "FechaPedido",
                template: function (dataItem) {
                    if (moment(dataItem.FechaPedido).format('DD/MM/YYYY') == '01/01/0001') {
                        return '';
                    }
                    return moment(dataItem.FechaPedido).format('DD/MM/YYYY');
                }
            }, {
                title: "Fecha de creación (encabezado)",
                field: "FechaCreacion",
                template: function (dataItem) {
                    return moment(dataItem.FechaCreacion).format('DD/MM/YYYY');
                }
            }, {
                title: "Fecha Entrega",
                field: "FechaEntrega",
                template: function (dataItem) {
                    if (moment(dataItem.FechaEntrega).format('DD/MM/YYYY') == '01/01/0001') {
                        return '';
                    }
                    return moment(dataItem.FechaEntrega).format('DD/MM/YYYY');
                }
            }, {
                title: "Fecha de caducidad (encabezado)",
                field: "FechaCaducidad",
                template: function (dataItem) {
                    if (moment(dataItem.FechaCaducidad).format('DD/MM/YYYY') == '01/01/0001') {
                        return '';
                    }
                    return moment(dataItem.FechaCaducidad).format('DD/MM/YYYY');
                }
            }, {
                title: "Proveedor",
                field: "Proveedor"
            }, {
                title: "Cuenta",
                field: "Cuenta"
            },
            {
                title: "Id. de OC",
                field: "IdOC"
            },
            {
                title: "Leading Cost Center (Encabezado)",
                field: "LeandingCostC"
            },
            {
                title: "Estado de la línea",
                field: "EstadoLinea"
            },
            {
                title: "Divisa",
                field: "Divisa"
            },
            {
                title: "Estado",
                field: "Recent",
                template: function (dataItem) {
                    var Valor = "";
                    if (dataItem.Recent == 1) {
                        Valor = "Nuevo";
                    } else {
                        Valor = "Modificado";
                    }
                    return Valor;
                }
            }],

            sortable: true,
            scrollable: false,
            resizable: true,
            reorderable: true,
            columnMenu: true,

            filterable: {
                mode: "row"
            },
            pageable: {
                refresh: false,
                buttonCount: 5,
                filterable: {
                    mode: "row"
                },
                messages: {
                    display: "Mostrando de {0} a {1} de {2} registros",
                    empty: "No hay registros",
                    next: "Página Siguente",
                    last: "Página Final",
                    first: "Página De Inicio",
                    previous: "Página Anterior"
                },
            },
        });
        $('#gridLista .k-grid-header th.k-header').css('padding-left', "0.3%");
        GridCreada = true;
    } else {
        $("#gridLista").addClass('hide');
        if (MostrarMensajeDeGridVacio !== 1)
            $("#gridLista").data("kendoGrid").dataSource.data([]);
    }
};


