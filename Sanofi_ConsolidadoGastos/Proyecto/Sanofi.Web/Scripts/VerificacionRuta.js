var URLactual = "";
var CodPerfilUsuario = 0;
var EstadoSincronizacion = false;

$(document).ready(function () {
    var VerificacionRuta = sessionStorage.getItem("RutaActual");
    if (VerificacionRuta !== null && VerificacionRuta!=="") {
        URLactual = sessionStorage.getItem("RutaActual");
    } else {
        URLactual = window.location.href;
        URLactual = URLactual.replace("/Home/Index", "");
        sessionStorage.setItem("RutaActual", URLactual);
    }

    //var VerificacionPerfilUsuario = sessionStorage.getItem("PerfilUsuario");
    //if (VerificacionRuta !== null) {
    //    CodPerfilUsuario = sessionStorage.getItem("PerfilUsuario");
    //}

});

