
function TextoModalCarga(Texto) {
    $("#hcarga").text(Texto);
};

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

$("#BtnGenerarExcel").on('click', function () {
    ExcelConsolidado();
});

function ExcelConsolidado(MostrarMensajeDeGridVacio = 1) {
    try {
        $('#ModalCarga').modal('show');
        TextoModalCarga("Generando archivo excel");
        if ($("#FechaInicio").val() != "" && $("#FechaFin").val() != "") {
            var Datos = {};
            Datos.FechaInicial = $("#FechaInicio").val();
            Datos.FechaFinal = $("#FechaFin").val();
            $.ajax({
                type: "POST",
                url: sessionStorage.getItem("RutaActual") + '/Consolidado/ExportToExcel',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(Datos),
                success: function (data) {
                    $('#ModalCarga').modal('hide');
                    //print(data);
                    if (data.Respuesta) {
                        window.location.href = data.Resultado;
                    } else {
                       swal({
                            text: "No se encontrarón Resultados.",
                            icon: "warning",
                        });
                    }
                }, error: function (data) {
                    $('#ModalCarga').modal('hide');
                    swal({
                        text: "Se presento un problema al momento de generar el archivo de Excel",
                        icon: "warning",
                    });
                }
            });
        } else {
            $('#ModalCarga').modal('hide');
            swal({
                text: "Los campos con (*), son de carácter obligatorio.",
                icon: "warning",
            });
        }
    } catch (e) {
        $('#ModalCarga').modal('hide');
    }
    
    
};