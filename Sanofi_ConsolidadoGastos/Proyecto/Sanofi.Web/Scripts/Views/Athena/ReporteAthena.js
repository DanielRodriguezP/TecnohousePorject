$(document).ready(function () {
    GenerarReporte();
    ////Asignando kendoDatePicker al input de fecha inicial
    //$('#FechaInicio').kendoDatePicker({
    //    language: "es",
    //    todayHighlight: true,
    //    autoclose: true,
    //    format: "dd/MM/yyyy"
    //});

    //////Asignando kendoDatePicker al input de fecha Final
    //$('#FechaFin').kendoDatePicker({
    //    language: "es",
    //    todayHighlight: true,
    //    autoclose: true,
    //    format: "dd/MM/yyyy"
    //});


    //Validación
    //$('#FechaFin').on('change', function () {
    //    Verificar_Fechas("Fin");
    //});

    //$('#FechaInicio').on('change', function () {
    //    Verificar_Fechas("Inicio");
    //});

    //$("#FechaFin").val('');
});

//Verificar que la fecha de Inicio siempre sea menor a la fecha Final del Evento
//function Verificar_Fechas(campo) {

//    var Fecha = $("#FechaInicio").val();
//    var Ano = parseInt(Fecha.substring(6, 10), 10);
//    var Mes = parseInt(Fecha.substring(3, 5), 10);
//    var Dia = parseInt(Fecha.substring(0, 2), 10);
//    var FechaIni = new Date(Ano, Mes, Dia);

//    Fecha = $("#FechaFin").val();
//    Ano = parseInt(Fecha.substring(6, 10), 10);
//    Mes = parseInt(Fecha.substring(3, 5), 10);
//    Dia = parseInt(Fecha.substring(0, 2), 10);
//    var FechaFin = new Date(Ano, Mes, Dia);

//    if (FechaFin < FechaIni) {
//        switch (campo) {
//            case "Inicio":
//                $("#FechaInicio").val('');
//                swal({
//                    text: "La fecha de inicio no puede ser mayor a la fecha de final.",
//                    icon: "warning",
//                });
//                break;
//            case "Fin":
//                $("#FechaFin").val('');
//                swal({
//                    text: "La fecha final no puede ser menor a la fecha de Inicio.",
//                    icon: "warning",
//                });
//                break;
//        }
//    }
//}

function GenerarReporte() {
    document.getElementById('Frame').style.display = 'block';
    $("#Frame").attr('src', '../Reportes/Athena/ReporteAthena.aspx');
};

