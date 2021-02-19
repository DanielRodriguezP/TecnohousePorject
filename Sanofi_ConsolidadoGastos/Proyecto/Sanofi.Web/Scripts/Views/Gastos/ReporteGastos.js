
var IdReceptor = 0;
var GridCreada = false;
var ListaCuentas = "";
var ListaTipos = "";
var ListaConcatenar = "";
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
    if (FechaSplit[0] > 0 && FechaSplit[0] < 13 && FechaSplit[2] > 1900 && FechaSplit[2] < 2500) {
        CargarComboCuentas();
        CargarComboTipos();
        CargarComboConcatenar();
        $("#DivCuenta").prop("style", "display:block");
        $("#DivTipo").prop("style", "display:block");
        $("#DivConcatenar").prop("style", "display:block");
        $("#DivBtnBuscar").prop("style", "display:block");
    } else {
        swal("El formato de fecha no corresponde con el establecido(MM/DD/YYYY).");
        $("#Fecha").val('');
        $("#DivCuenta").prop("style", "display:none");
        $("#DivTipo").prop("style", "display:none");
        $("#DivConcatenar").prop("style", "display:none");
        $("#DivBtnBuscar").prop("style", "display:none");
    }
}

$("#BtnBuscar").on('click', function () {
    //alert(ListaCuentas.substr(1, ListaCuentas.length));
    //alert(ListaTipos.substr(1, ListaTipos.length));
    //alert(ListaConcatenar.substr(1, ListaConcatenar.length));
    document.getElementById('Frame').style.display = 'block';
    var Datos = {};
    Datos.Cuentas = ListaCuentas.substr(1, ListaCuentas.length);
    Datos.Tipos = ListaTipos.substr(1, ListaTipos.length);
    Datos.Concatenar = ListaConcatenar.substr(1, ListaConcatenar.length);
    //$.ajax({
    //    type: "POST",
    //    url: sessionStorage.getItem("RutaActual") + '../Reportes/Gastos/ReporteGastos.aspx',
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    data: JSON.stringify(Datos),
    //    success: function (data) {
    //        $("#Frame").html(data);
    //    }
    //});
    $("#Frame").attr('src', '../Reportes/Gastos/ReporteGastos.aspx?Cuentas=' + ListaCuentas.substr(1, ListaCuentas.length) + '&Tipos=' + ListaTipos.substr(1, ListaTipos.length) + '&Concatenar=' + ListaConcatenar.substr(1, ListaConcatenar.length));
});

function ConsultarGastos() {
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

function ConsultarCuentas() {
    if ($("#Fecha").val() != "") {
        var Datos = {};
        fecha = $("#Fecha").val().split("/");
        Datos.FechaInicial = fecha[0];
        Datos.FechaFinal = fecha[2];
        $.ajax({
            type: "POST",
            url: sessionStorage.getItem("RutaActual") + '/Gastos/ConsultarCuentas',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(Datos),
            success: function (data) {
                if (data.Resultado === true) {
                    if (data.Datos.length > 0) {
                        $("#gridLista").show();
                        CargarGrid(data.Datos);
                        var Mostrar1 = new kendo.data.DataSource({
                            data: data.Datos,
                            pageSize: 10
                        });
                        $("#gridLista").data("kendoGrid").setDataSource(Mostrar1);
                        AbrirModal(item);
                    } else {
                        swal("No se encontrarón Resultados");

                    }
                    
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

function AbrirModal(item) {
    
    $('#modal').modal('show');
    $('#Filtro').val("");
    document.getElementById("gridMenusNG").style.display = "none";
    if (item == 1) {
        $("#myModalLabel").text("Cuenta");
    } else if (item == 2) {
        $("#myModalLabel").text("Tipo");
    } else if (item == 3) {
        $("#myModalLabel").text("Concatenar");
    }

}

function CargarComboCuentas() {
    $("#dialogCuentas").kendoDialog({
        visible: false,
        title: "Cuentas",
        closable: true,
        modal: true,
        content: "<div><div id='listViewCuentas' style='width: 93% ;height: 170px'><div class='row' style='margin-top:- 5 %; margin-left: auto;'><input type='checkbox' id='Todos' style='margin-top: 0;'/>< span class='checkbox' style = 'margin-top:0;' >Todos</span></div></div></div>"
        ,
        actions: [
            { text: 'Aceptar', primary: true, action: OkCuentas },
            { text: 'Cancelar' }
        ]
    });
    $('.k-dialog').prop("style", "width:20% !important;");
    $("#Btncuentas").on("click", function () {
        $("#dialogCuentas").data("kendoDialog").open();
        //$('input[type="checkbox"]').prop("checked", true);
    });
    
    if ($("#Fecha").val() != "") {
        var Datos = {};
        fecha = $("#Fecha").val().split("/");
        Datos.FechaInicial = fecha[0];
        Datos.FechaFinal = fecha[2];
        $.ajax({
            url: sessionStorage.getItem("RutaActual") + '/Gastos/ConsultarCuentas',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(Datos),
            success: function (Dato) {
                if (Dato.Resultado === true) {
                    $("#listViewCuentas").kendoListView({
                        dataSource: Dato.Datos,
                        template: kendo.template($("#templateCuenta").html())
                    });
                    var cantidad = Dato.Datos.length;
                    OkCuentas();
                    $('.checkboxCuenta:checkbox').change(function () {
                        $('#CTodos').prop("checked", false);
                        var num = CantidadMarcados(1);
                        if (num == cantidad - 1) {
                            $('#CTodos').prop("checked", true);
                        }
                    });

                    $("#CTodos").on("click", function () {
                        if ($("#CTodos").length == $("#CTodos:checked").length) {
                            $('.checkboxCuenta:checkbox').prop("checked", true);
                        } else {
                            $('.checkboxCuenta:checkbox').prop("checked", false);
                        }
                    });
                } else {
                    swal("", Dato.Mensaje.Texto, "warning");
                }
            }
        });
    } else {
        swal("Los campos con (*), son de carácter obligatorio.");
    }
}

function CargarComboTipos() {
    $("#dialogTipo").kendoDialog({
        visible: false,
        title: "Tipos",
        closable: true,
        modal: true,
        content: "<div id='listViewTipos' style='width: 93% ;height: 170px'></div>"//"<div><div id='listViewTipos' style='width: 93% ;height: 170px'></div></div> "
        ,
        actions: [
            { text: 'Aceptar', primary: true, action: OkTipos },
            { text: 'Cancelar' }
        ]
    });
    $('.k-dialog').prop("style", "width:20% !important;");
    $("#Btntipos").on("click", function () {
        $("#dialogTipo").data("kendoDialog").open();
        //$('input[type="checkbox"]').prop("checked", true);
    });

    if ($("#Fecha").val() != "") {
        var Datos = {};
        fecha = $("#Fecha").val().split("/");
        Datos.FechaInicial = fecha[0];
        Datos.FechaFinal = fecha[2];
        $.ajax({
            url: sessionStorage.getItem("RutaActual") + '/Gastos/ConsultarTipos',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(Datos),
            success: function (Dato) {
                if (Dato.Resultado === true) {
                    $("#listViewTipos").kendoListView({
                        dataSource: Dato.Datos,
                        template: kendo.template($("#templateTipo").html())
                    });
                    //$("#listViewTipos").html()
                    OkTipos();
                    var cantidad = Dato.Datos.length;
                    $('.checkboxTipo:checkbox').change(function () {
                        $('#TTodos').prop("checked", false);
                        var num = CantidadMarcados(2);
                        if (num == cantidad - 1) {
                            $('#TTodos').prop("checked", true);
                        }
                    });

                    $("#TTodos").on("click", function () {
                        if ($("#TTodos").length == $("#TTodos:checked").length) {
                            $('.checkboxTipo:checkbox').prop("checked", true);
                        } else {
                            $('.checkboxTipo:checkbox').prop("checked", false);
                        }
                    });
                } else {
                    swal("", Dato.Mensaje.Texto, "warning");
                }
            }
        });
    } else {
        swal("Los campos con (*), son de carácter obligatorio.");
    }
}

function CargarComboConcatenar() {
    $("#dialogConcatenar").kendoDialog({
        visible: false,
        title: "Concatenar",
        closable: true,
        modal: true,
        content: "<div><div id='listViewConcatenar' style='width: 93% ;height: 170px'></div></div> "
        ,
        actions: [
            { text: 'Aceptar', primary: true, action: OkConcatenar },
            { text: 'Cancelar' }
        ]
    });
    $('.k-dialog').prop("style", "width:20% !important;");
    $("#BtnConcatenar").on("click", function () {
        $("#dialogConcatenar").data("kendoDialog").open();
        //$('input[type="checkbox"]').prop("checked", true);
    });

    if ($("#Fecha").val() != "") {
        var Datos = {};
        fecha = $("#Fecha").val().split("/");
        Datos.FechaInicial = fecha[0];
        Datos.FechaFinal = fecha[2];
        $.ajax({
            url: sessionStorage.getItem("RutaActual") + '/Gastos/ConsultarConcatenar',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(Datos),
            success: function (Dato) {
                if (Dato.Resultado === true) {
                    $("#listViewConcatenar").kendoListView({
                        dataSource: Dato.Datos,
                        template: kendo.template($("#templateConcatenar").html())
                    });
                    var cantidad = Dato.Datos.length;
                    OkConcatenar();
                    $('.checkboxConcatenar:checkbox').change(function () {
                        $('#RTodos').prop("checked", false);
                        var num = CantidadMarcados(3);
                        if (num == cantidad - 1) {
                            $('#RTodos').prop("checked", true);
                        }
                    });

                    $("#RTodos").on("click", function () {
                        if ($("#RTodos").length == $("#RTodos:checked").length) {
                            $('input[type="checkbox"]').prop("checked", true);
                        } else {
                            $('input[type="checkbox"]').prop("checked", false);
                        }
                    });
                } else {
                    swal("", Dato.Mensaje.Texto, "warning");
                }
            }
        });
    } else {
        swal("Los campos con (*), son de carácter obligatorio.");
    }
}

function OkCuentas() {
    ListaCuentas = "";
    $(".checkboxCuenta:checkbox:checked").each(
        function () {
            ListaCuentas = ListaCuentas + "," + $(this).prop("id").substr(1, $(this).prop("id").length);
        }
    );
}
function OkTipos() {
    ListaTipos = "";
    $(".checkboxTipo:checkbox:checked").each(
        function () {
            ListaTipos = ListaTipos + "," + $(this).prop("id").substr(1, $(this).prop("id").length);
        }
    );
}
function OkConcatenar() {
    ListaConcatenar = "";
    $(".checkboxConcatenar:checkbox:checked").each(
        function () {
            ListaConcatenar = ListaConcatenar + "," + $(this).prop("id").substr(1, $(this).prop("id").length);
        }
    );
}

function CantidadMarcados(clase) {
    cantidadMarcadas = 0;
    if (clase == 1) {
        $(".checkboxCuenta:checkbox:checked").each(
            function () {
                cantidadMarcadas++;
            }
        );
    } else if (clase == 2) {
        $(".checkboxTipo:checkbox:checked").each(
            function () {
                cantidadMarcadas++;
            }
        );
    } else if (clase == 3) {
        $(".checkboxConcatenar:checkbox:checked").each(
            function () {
                cantidadMarcadas++;
            }
        );
    }
    return cantidadMarcadas;
}