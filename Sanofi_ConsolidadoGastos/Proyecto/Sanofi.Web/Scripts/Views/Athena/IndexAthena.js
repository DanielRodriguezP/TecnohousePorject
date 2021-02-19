//Variables Globales
const realInput = document.getElementById('real-input');
const uploadButton = document.querySelector('.browse-btn');
const fileInfo = document.querySelector('.file-info');
var spreadsheet = "";
var NombreDocumento = "";
var ObjHome = new Home();
this.GridCreadaError = false;

$(document).ready(function () {

    $("#spreadsheet").kendoSpreadsheet({
        excel: {
            proxyURL: "https://demos.telerik.com/kendo-ui/service/export"
        }
    });

    if (!ObjHome.GridCreada) {
        ObjHome.CargarGrid();
        ObjHome.ConsultarRegistrosExitosos(1, true);
    }
    else {
        ObjHome.ConsultarRegistrosExitosos(1, true);
    }

    if (!GridCreadaError) {
        CargarGridErrores();
        ConsultarRegistrosconErrores(1, true);
    }
    else {
        ConsultarRegistrosconErrores(1, true);
    }
    

});

$("#lbDescargarPlantilla").click(function () {
    DescargarPlantilla();
});

function DescargarPlantilla() {
    $.ajax({
        url: sessionStorage.getItem("RutaActual") +'/Athena/DescargarPlantilla',
        type: "GET",
        dataType: "JSON",
        data: null,
        success: function (data) {
            if (data.Respuesta) {
                window.location.href = sessionStorage.getItem("RutaActual") + data.Resultado;
            }
            else {
                swal("Señor Usuario", "" + data.Mensaje.Texto + "", "info");
            }
        }
    });
}

uploadButton.addEventListener('click', (e) => {
    realInput.click(e);
});

realInput.addEventListener('change', (e) => {
    const urll = realInput.value;
    const name = realInput.value.split(/\\|\//).pop();
    fileInfo.innerHTML = name;
    ValidarExtensionArchivo(name);
    spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
    var InfoFile = $('#real-input').get(0).files[0];
    spreadsheet.fromFile(InfoFile);
    NombreDocumento = name;
});

function ValidarExtensionArchivo(NombreArchivo) {
    var respuesta;
    var cadena = NombreArchivo.toString();
    var longitud = NombreArchivo.length;
    for (i = longitud; i >= 0; i--) {
        if (NombreArchivo.charAt(i) === '.') {
            var posicion = i;
            break;
        }
    }
    if (posicion > 0) {
        extension = cadena.substring(posicion + 1, longitud);
        if (extension == "xls" || extension == "xlsx" || extension == "XLS" || extension == "XLSX") {
            respuesta = true;
            $('#BtnSubirInfo').show();
        } else {
            swal("Señor Usuario", "El formato " + extension + " es incorrecto, por favor seleccione un archivo con formato .xls o .xlsx");
            $('#BtnSubirInfo').hide();
            $('#NombreArchivo').html("");
        }
    } else {
        swal("Archivo, no seleccionado.");
        $('#BtnSubirInfo').hide();
        $('#NombreArchivo').html("");
    }
    return respuesta;
}

function MensajeExitoso(Mensaje) {
    swal("Archivo procesado!", Mensaje, "success").then(function () {
        window.location.reload();
    });

}

function CargarInformacionExcel(conf) {
    try {
        $('#ModalCarga').modal('show');
        TextoModalCarga("Validando la estructura del archivo y almacenando la información.");
        var file = JSON.stringify(spreadsheet.toJSON());
        $.ajax({
            url: sessionStorage.getItem("RutaActual") + '/Athena/ValidarArchivo',
            type: "POST",
            dataType: "JSON",
            data: {
                Plantilla: file,
                NombreArchivo: NombreDocumento,
                Conf: conf
            },
            success: function (data) {
                $('#ModalCarga').modal('hide');
                if (data.Respuesta) {
                    ObjHome.ConsultarRegistrosExitosos(0, false);
                }
                else {
                    if (data.Resultado != null) {

                        ConfirmarUsuario(data.Mensaje.Texto, NombreArchivo)

                    } else {
                        
                        swal("Señor Usuario", "" + data.Mensaje.Texto + "", "warning")
                    }
                }
            },
            error: function () {
                $('#ModalCarga').modal('hide');
            }
        });
    } catch (error) {
        $('#ModalCarga').modal('hide');
    }
}

function ConfirmarUsuario(mensaje, fileName) {

    swal({
        title: "Confirmar",
        text: mensaje,
        icon: "warning",
        buttons: true,
        dangerMode: true
    })
        .then((willDelete) => {
            if (willDelete) {
                //El usuario confirmó la Actualización. Proceder:
                CargarInformacionExcel(1);

            } else {

                swal("No se actualizaron los registros!");

            }
        });

}

function Home() {
    this.GridCreada = false;

    this.ConsultarRegistrosExitosos = function (MostrarMensajeDeGridVacio = 1, ValidarExitosos) {
        $.ajax({
            type: "POST",
            url: sessionStorage.getItem("RutaActual") +"/Athena/ConsultarregistrosExitosos",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: 0,
            success: function (data) {
                if (data.Resultado === true) {
                    if (ValidarExitosos) {
                        LLenarGrid(data.Datos);
                        if (ObjHome.GridCreada)
                            $("#SpamSucess").html(data.Datos.length);
                    } else {
                        LLenarGrid(data.Datos);
                        if (ObjHome.GridCreada)
                            $("#SpamSucess").html(data.Datos.length);
                        ConsultarRegistrosconErrores(0, false);
                    }


                }
                else {
                    ObjHome.CargarGrid(data.Datos, MostrarMensajeDeGridVacio);
                    if (MostrarMensajeDeGridVacio === 1)
                        console.log(data.Mensaje)
                }
            }
        });
    };


    function LLenarGrid(dataSource) {
        var grid = $("#gridExitosos").data("kendoGrid");
        var dataSource = new kendo.data.DataSource({
            type: "json",
            data: dataSource,
            pageSize: 10,
            cache: false,
            async: false
        });
        grid.setDataSource(dataSource);
    };
    //***************************************
    //Cargar los datos en la GRID.
    //****************************************
    this.CargarGrid = function (data, MostrarMensajeDeGridVacio = 1) {
        var exportFlag = false;
        var EstadoCarga = false;
        if (data !== null) {
            $("#gridExitosos").removeClass('hide');
            $("#gridExitosos").kendoGrid({
                columns: [{
                    title: "Item",
                    field: "Item"
                }, {
                    title: "Purchasing Document",
                    field: "Document"
                }, {
                    title: "Document Date",
                    field: "Date",
                    template: function (dataItem) {
                        return moment(dataItem.Date).format('DD/MM/YYYY');
                    }
                }, {
                    title: "Material",
                    field: "Material"
                }, {
                    title: "Short Text",
                    field: "ShortText"
                }, {
                    title: "Order Quantity",
                    field: "OrderQty"
                }, {
                    title: "Still to be delivered (qty)",
                    field: "DeliveryQty"
                }, {
                    title: "Order Unit",
                    field: "OrderUnit"
                }, {
                    title: "Net price",
                    field: "NetPrice"
                }, {
                    title: "Net Order Value",
                    field: "NetOrderValue"
                }, {
                    title: "Vendor/supplying plant",
                    field: "Vendor"
                },
                {
                    title: "Currency",
                    field: "Currency"
                },
                {
                    title: "Release State",
                    field: "ReleaseState"
                },
                {
                    title: "PO history/release documentation",
                    field: "POHistory"
                },
                {
                    title: "Status",
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
            ObjHome.GridCreada = true;
        } else {
            $("#gridExitosos").addClass('hide');
            if (MostrarMensajeDeGridVacio !== 1)
                $("#gridExitosos").data("kendoGrid").dataSource.data([]);
        }
    };
}

function ConsultarRegistrosconErrores(MostrarMensajeDeGridVacio = 1, activarMensaje) {
    $.ajax({
        type: "POST",
        url: sessionStorage.getItem("RutaActual") +"/Athena/ConsultarErroresExcel",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: 0,
        success: function (data) {
            if (data.Resultado === true) {
                if (activarMensaje) {
                    LLenarGridErrores(data.Datos);
                    if (GridCreadaError)
                        $("#SpanError").html(data.Datos.length);
                } else {
                    LLenarGridErrores(data.Datos);
                    if (GridCreadaError)
                        $("#SpanError").html(data.Datos.length);
                    MensajeExitoso("Registros almacenados correctamente: " + $("#SpamSucess").html() + ". \n Celdas que no cumplen las especificaciones: " + $("#SpanError").html() + ".");
                }
            }
            else {
                CargarGridErrores(data.Datos, MostrarMensajeDeGridVacio);
                if (MostrarMensajeDeGridVacio === 1)
                    console.log(data.Mensaje)
            }
        }
    });
};

function LLenarGridErrores(dataSource) {
    var grid = $("#gridErorres").data("kendoGrid");
    var dataSource = new kendo.data.DataSource({
        type: "json",
        data: dataSource,
        pageSize: 10,
        cache: false,
        async: false
    });
    grid.setDataSource(dataSource);
};

function CargarGridErrores(data, MostrarMensajeDeGridVacio = 1) {
    if (data !== null) {
        $("#gridErorres").removeClass('hide');
        $("#gridErorres").kendoGrid({
            columns: [{
                title: "Mensaje",
                field: "Mensaje"
            }, {
                title: "Campo",
                field: "Campo"
            }, {
                title: "Valor",
                field: "Valor"
            }, {
                title: "Fila",
                field: "Fila"
            }, {
                title: "Fecha Registro",
                field: "Fecha",
                template: function (dataItem) {
                    return moment(dataItem.Fecha).format('DD/MM/YYYY');
                }
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
        GridCreadaError = true;
    } else {
        $("#gridErorres").addClass('hide');
        if (MostrarMensajeDeGridVacio !== 1)
            $("#gridErorres").data("kendoGrid").dataSource.data([]);
    }
};

function TextoModalCarga(Texto) {
    $("#hcarga").text(Texto);
};
