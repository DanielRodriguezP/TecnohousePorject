
var NombreReceptor = "";
var IdReceptor = 0;
var GridCreada = false;
$(document).ready(function () {

    ////Asignando kendoDatePicker al input de fecha
    $("#Fecha").kendoDatePicker({
        depth: "year",
        start: "year"
    });


    //Validación
    $('#Fecha').on('change', function () {
        Verificar_Fechas();
    });

    $("#Fecha").val('');
});

//Verificar que la fecha de Inicio siempre sea menor a la fecha Final del Evento
function Verificar_Fechas() {

    var Fecha = $("#Fecha").val();
    var FechaSplit = Fecha.split("/")
    if (FechaSplit[0] > 0 && FechaSplit[0] < 13 && FechaSplit[2] > 1900 && FechaSplit[2]<2500) {
    } else {
        swal("El formato de fecha no corresponde con el establecido(MM/DD/YYYY).");
        $("#Fecha").val('');
    }
}

$("#BtnBuscar").on('click', function () {
    ConsultarGastos();
});

function ConsultarGastos(MostrarMensajeDeGridVacio = 1) {
    if ($("#Fecha").val() != "") {
        var Datos = {};
        fecha = $("#Fecha").val().split("/");
        Datos.FechaInicial = fecha[0];
        Datos.FechaFinal = fecha[2];
        $.ajax({
            type: "POST",
            url: sessionStorage.getItem("RutaActual") + '/Gastos/ConsultarGastosFiltros',
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
                    var Mostrar1 = new kendo.data.DataSource({
                        data: data.Datos,
                        pageSize: 10
                    });
                    $("#gridLista").data("kendoGrid").setDataSource(Mostrar1);
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
                title: "CUENTA",
                field: "NomCuenta"
            }, {
                title: "Clases de coste/Centros de cos",
                field: "NomCentroC"
            }, {
                title: "CUENTA",
                field: "NroCuenta",

            }, {
                title: "CC",
                field: "CentroCoste"
            }, {
                title: "CONCATENAR",
                field: "Concatenar"
            }, {
                title: "TIPO",
                field: "Tipo"
            }, {
                title: "MES",
                field: "Mes"
            }, {
                title: "AJUSTES",
                field: "Ajustes"
            }, {
                title: "TOTAL MES",
                field: "TotalMes"
            }],
            sortable: true,
            scrollable: false,
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
                }
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


