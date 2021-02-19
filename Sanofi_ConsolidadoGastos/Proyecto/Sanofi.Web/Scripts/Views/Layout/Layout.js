var ObjLayout = new Layout();
var URLactual2 = null;
$(document).ready(function () {

    ObjLayout.ConsultarVersion();

    //var array = [];

    //for (var i = 0; i < 19; i++) {
    //    var data = {
    //        IdModalidad: 0
    //    }
    //    data.IdModalidad = i+1;
    //    array[i]= data;
    //}

    if (sessionStorage.getItem("Usuario") === null || sessionStorage.getItem("Usuario") === "") {
        URLactual2 = window.location.href;
        ObjLayout.ConsultarUsuarioWindows();
        //$("#page-top").prop("style", "display:block");
        //ObjLayout.VisualizacionMenu(array);

    } else {
        URLactual2 = window.location.href;
        ObjLayout.ConsultarRolModalidadUsuario();
        //$("#page-top").prop("style", "display:block");
        //ObjLayout.VisualizacionMenu(array);
    }
});



function Layout() {

    this.ConsultarVersion = function () {
        $.ajax({
            type: "GET",
            url: sessionStorage.getItem("RutaActual") + "/Layout/ConsultarVersion",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {},

            success: function (data) {
                $("#Version").text("Versión " + data);

            }
        });
    }
    //***************************************
    //Se consulta los roles y modalidades a las que tiene permiso el usuario
    //****************************************
    this.ConsultarRolModalidadUsuario = function () {
        var _Usuario = sessionStorage.getItem("Usuario");
        $.ajax({
            type: "GET",
            url: sessionStorage.getItem("RutaActual") + "/Layout/ConsultarRolModalidadUsuario",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {
                _Usuario: _Usuario
            },

            success: function (data) {
                if (data.Respuesta) {
                    if (URLactual2 == sessionStorage.getItem("RutaActual") || URLactual2 == sessionStorage.getItem("RutaActual") + "/Home/Index") {
                        $("#User").text(sessionStorage.getItem("Nombre"));
                        $("#page-top").prop("style", "display:block");

                        ObjLayout.VisualizacionMenu(data.Resultado);
                    } else {
                        var Acceso = 0;
                        $.each(data.Resultado, function (key, item) {

                            if (URLactual2.indexOf(item.RutaAcceso) > -1) {
                                Acceso = Acceso + 1;
                            }

                        });
                        if (Acceso != 0) {
                            $("#User").text(data.Resultado[0].Nombre);
                            $("#page-top").prop("style", "display:block");

                            ObjLayout.VisualizacionMenu(data.Resultado);
                        } else {
                            window.location.href = sessionStorage.getItem("RutaActual") + "/Home/Index";

                        }

                    }

                }
                else {
                    console.log(data.Mensaje)
                }

            }
        });
    };


    //***************************************
    //Se consulta usuario que tiene la sesión de windows iniciada
    //****************************************
    this.ConsultarUsuarioWindows = function () {

        $.ajax({
            type: "GET",
            url: sessionStorage.getItem("RutaActual") + "/Layout/ConsultarUsuario",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {},
            success: function (data) {
                if (data.Respuesta) {
                    sessionStorage.setItem("Usuario", data.Resultado.Usuario);
                    sessionStorage.setItem("Nombre", data.Resultado.Nombre);
                    var user = data.Resultado.Usuario;
                    if (user !== null && user !== "") {
                        $("#page-top").prop("style", "display:block");
                        ObjLayout.ConsultarRolModalidadUsuario();
                    }
                }
                else {

                    window.location.href = sessionStorage.getItem("RutaActual") + "/ErrorUsuario/ErrorUsuario";

                }
            },
            error: function () {

            }
        });
    };

    //***************************************
    //Filtra las modalidades que puede visualizar el usuario
    //***************************************

    this.VisualizacionMenu = function (data) {
        $.each(data, function (key, item) {
            switch (item.IdModalidad) {
                case 1:
                    $("#Athena").prop("style", "display:block");
                    $("#IndexAthena").prop("style", "display:block");
                    break;
                case 2:
                    $("#Athena").prop("style", "display:block");
                    $("#ConsultaAthena").prop("style", "display:block");
                    break;
                case 3:
                    $("#Usuario").prop("style", "display:block");
                    $("#MUser").prop("style", "display:block");
                    break;
                default:
                    //Declaraciones ejecutadas cuando ninguno de los valores coincide con el valor de la expresión

                    break;
            };
        });
    };
}
