
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

    Fecha = $("#FechaFin").val();
    Ano = parseInt(Fecha.substring(6, 10), 10);
    Mes = parseInt(Fecha.substring(3, 5), 10);
    Dia = parseInt(Fecha.substring(0, 2), 10);
    var FechaFin = new Date(Ano, Mes, Dia);

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
}

$("#BtnBuscar").on('click', function () {
    ConsultarAthena();
});

function ConsultarAthena(MostrarMensajeDeGridVacio = 1) {
    if ($("#FechaInicio").val() != "" && $("#FechaFin").val() != "") {
        var Datos = {};
        Datos.FechaInicial = $("#FechaInicio").val();
        Datos.FechaFinal = $("#FechaFin").val();
        $.ajax({
            type: "POST",
            url: sessionStorage.getItem("RutaActual") + '/Athena/ConsultarAthenaFiltros',
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
                title: "Item",
                field: "Item",
                width: "140px"
            }, {
                title: "Purchasing Document",
                field: "Document",
                width: "140px"
            }, {
                title: "Document Date",
                field: "Date",
                width: "140px",
                template: function (dataItem) {
                    return moment(dataItem.Date).format('DD/MM/YYYY');
                }
            }, {
                title: "Material",
                field: "Material",
                width: "140px"
            }, {
                title: "Short Text",
                field: "ShortText",
                width: "140px"
            }, {
                title: "Order Quantity",
                field: "OrderQty",
                width: "140px"
            }, {
                title: "Still to be delivered (qty)",
                field: "DeliveryQty",
                width: "140px"
            }, {
                title: "Order Unit",
                field: "OrderUnit",
                width: "140px"
            }, {
                title: "Net price",
                field: "NetPrice",
                width: "140px"
            }, {
                title: "Net Order Value",
                field: "NetOrderValue",
                width: "140px"
            }, {
                title: "Vendor/supplying plant",
                field: "Vendor",
                width: "140px"
            },
            {
                title: "Currency",
                field: "Currency",
                width: "140px"
            },
            {
                title: "Release State",
                field: "ReleaseState",
                width: "140px"
            },
            {
                title: "PO history/release documentation",
                field: "POHistory",
                width: "140px"
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
            }
        });
        $('#gridLista .k-grid-header th.k-header').css('padding-left', "0.3%");
        GridCreada = true;
    } else {
        $("#gridLista").addClass('hide');
        if (MostrarMensajeDeGridVacio !== 1)
            $("#gridLista").data("kendoGrid").dataSource.data([]);
    }
};


