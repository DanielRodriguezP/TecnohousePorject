function Mensajes() {

    this.MostrarMensaje = function (Mensaje) {
        $('body').css('padding-right', '');
        $('.modal-body').css('padding-right', '');

        //if ($('.modal-backdrop').is(':visible')) {
        //  $('body').removeClass('modal-open'); 
        //  $('.modal-backdrop').remove(); 
        //};

        /*el Tipo 6 es exclusivo para redireccionar al usuario al Logout
         *de lo contrario se mostrara un mensaje tipo modal*/
        if (Mensaje.Tipo != 6) {
            $('#divMenAdvertencia').css('display', 'none');
            $('#divMenCheck').css('display', 'none');
            $('#divMenError').css('display', 'none');
            $('#diMenInformacion').css('display', 'none');
            $('#diMenConfirmacion').css('display', 'none');
            $('#divMenProgress').css('display', 'none');
            $('#BotoneraConfirmacion').css('display', 'none');
            $('#BotoneraMensajes').css('display', 'block');
            $("#titulomensaje").text("Mensaje");
            $('#codigomensaje').attr('value', Mensaje.Codigo);
            switch (Mensaje.Tipo) {
                case 1:
                    $('#divMenAdvertencia').css('display', 'block');
                    $("#lblMenAdvertencia").text(Mensaje.Texto);
                    break;
                case 2:
                    $('#divMenCheck').css('display', 'block');
                    $("#lblMenCheck").text(Mensaje.Texto);
                    break;
                case 3:
                    $('#divMenError').css('display', 'block');
                    $("#lblMenError").text(Mensaje.Texto);
                    break;
                case 4:
                    $('#diMenInformacion').css('display', 'block');
                    $("#lblMenInformacion").text(Mensaje.Texto);
                    break;
                case 5:
                    $('#diMenConfirmacion').css('display', 'block');
                    $('#BotoneraConfirmacion').css('display', 'block');
                    $('#BotoneraMensajes').css('display', 'none');
                    $("#lblMenConfirmacion").text(Mensaje.Texto);

                    break;
                case 7:
                    $('#divMenProgress').css('display', 'block');
                    $('#BotoneraConfirmacion').css('display', 'none');
                    $('#BotoneraMensajes').css('display', 'none');
                    $("#titulomensaje").text(Mensaje.Texto);
                    break;
                case 8:
                    $('#diMenConfirmacion').css('display', 'block');
                    $('#BotoneraConfirmacion').css('display', 'block');
                    $('#BotoneraMensajes').css('display', 'none');
                    $("#lblMenConfirmacion").text(Mensaje.Texto);
                    $("#btnCancelar").val('NO');
                    $("#btnAceptar").val('SI');
                    break;
            }
            //if (!$("body").hasClass('modal-open')) {
            $('#modal-mensajes').modal('show');
            //}
        }
        else {
            /*se redirecciona al usuario al logout con el mensaje a mostrar*/
            window.location.replace("/Login/Logout/" + objEncryptMp.EncryptValor(Mensaje.Codigo));
        }
    };

    this.OcultarMensaje = function () {
        $('body').css('padding-right', '');
        $('#modal-mensajes').modal('hide');

        if ($('.modal-backdrop').is(':visible')) {
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        };

    };
    this.MensajeProgreso = function (Texto) {
        var Mensaje = new Object();
        Mensaje.Tipo = 7;
        Mensaje.Texto = Texto;
        objMensajes.MostrarMensaje(Mensaje);
    };

    this.ObtenerMensaje = function (Mensaje) {
        var mensajes;
        $.ajax({
            async: false,
            url: '/Mensajes/ObtenerMensaje',
            type: "GET",
            dataType: "JSON",
            data: { Filtro: objEncryptMp.EncryptValor(JSON.stringify(Mensaje)) },
            success: function (data) {
                if (data.Respuesta) {
                    mensajes = data.Mensaje;
                } else {
                    objMensajes.MostrarMensaje(data.Mensaje);
                }
            }
        });
        return mensajes;
    };
}

var objMensajes = new Mensajes();

var pageInitialized = false;
$(function () {
    if (pageInitialized) return;
    pageInitialized = true;
    // Put your init logic here.
    $(document).ready(function () {
        $("#modal-mensajes").modal({
            show: false,
            backdrop: 'static',
            keyboard: false
        });
        $("#btnCancelar").click(function () {
            RespuestaMensaje(false);
        });
        $("#btnAceptar").click(function () {
            RespuestaMensaje(true);
        });
        $("#btnCerrar").click(function () {
            RespuestaMensaje(true);
            $('#modal-mensajes').modal('hide');
        });
    });
});