var reset_application = {
    config: {},
    initialize: () => { },
    application: () => { }
}

//Configuraciones globales de la aplicación
reset_application.config = {
    //Configuración de iziToast
    iziToast: {
        title: '',
        titleColor: '',
        message: '',
        messageColor: '',
        messageSize: 14,
        theme: 'light',
        icon: '',
        iconText: '',
        iconColor: '',
        close: true,
        closeOnEscape: true,
        closeOnClick: true,
        displayMode: 0,
        timeout: 4000,
        animateInside: false,
        pauseOnHover: true,
        resetOnHover: false,
        progressBar: true,
        progressBarColor: '',
        progressBarEasing: 'linear',
        transitionIn: 'bounceInLeft',
        transitionOut: 'fadeOutRight',
        buttons: {},
        inputs: {},
        onOpening: () => { },
        onOpened: () => { },
        onClosing: () => { },
        onClosed: () => { }
    },

    //Declaraciones globales
    globals: {
        root: $("html").attr("data-root") + "/",
        posicion: ["bottomRight", "bottomLeft", "topRight", "topLeft", "topCenter", "bottomCenter", "center"]
    }
}

//Inicializa los elmenetos a utilizar
reset_application.initialize = () => {
    "use strict";

    //Recupera la url para extraer el token y el usuario
    let token = window.location.href.split('?')[1].split('=')[1].split('&')[0],
        correo = window.location.href.split('?')[1].split('=')[2];

    //Guarda el token y el usuario en un localStorage
    localStorage.setItem("correo-reset", correo);
    localStorage.setItem("token-reset", token);

    //Esconde los parámetros para que no sean visibles
    history.replaceState({}, null, reset_application.config.globals.root + "Login/ResetPassword");

    //Se deshabilita el caché de ajax para evitar cacheo de internet explorer.
    $.ajaxSetup({ cache: false });

    //Configuración de los botones ladda
    if (window.Ladda) Ladda.bind(".ladda-button");

    //Set configuración IziToast
    iziToast.settings(reset_application.config.iziToast);
};

//Funciones y métodos a utilizar
reset_application.application = () => {
    "use strict"

    //Lista de funciones y métodos vacía
    var functions = {
        components: {}
    }

    //Declaración de funcioens y métodos
    functions: {
        var components = {};

        //Handlers para la vista reset password
        components.reset_view = ((data) => {
            //Componentes de la vista
            var component = {};

            //Click del boton restaurar del formlario reset-password-form
            $("#reset-password-form").submit((e) => {
                e.preventDefault();
                e.stopPropagation();

                //Componentes del formularios
                component.$resetForm = $("#reset-password-form");
                component.$buttonReset = window.Ladda && Ladda.create(component.$resetForm.find(".ladda-button")[0]);
                component.$password = $('input[name="password"]', component.$resetForm).val();
                component.$confirmPassword = $('input[name="confirm-password"]', component.$resetForm).val();
                component.$token = $('input[name="__RequestVerificationToken"]', component.$resetForm).val();

                //Petición al controlador
                var resetRequest = $.ajax({
                    url: reset_application.config.globals.root + "Login/ResetPassword",
                    type: "POST",
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    data: {
                        __RequestVerificationToken: component.$token,
                        password: component.$password,
                        confirmPassword: component.$confirmPassword,
                        correo: localStorage.getItem("correo-reset"),
                        token: localStorage.getItem("token-reset")
                    },
                    dataType: "json",
                    beforeSend: () => {
                        component.$buttonReset.start();
                        //Remuevo los localStorage
                        localStorage.removeItem("correo-reset");
                        localStorage.removeItem("token-reset");
                    }
                });

                //Sólo si petición es satisfactoria
                resetRequest.done((data) => {
                    if (data.tipo === 0) {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: reset_application.config.globals.posicion[2] });
                    } else {
                        //Guardo la notificación en un localStorage
                        localStorage.setItem("mensaje-reset-success", data.mensaje);

                        //Redirijo al login
                        window.location.href = reset_application.config.globals.root + "Login/Login";
                    }
                });

                //Sólo si peticicón es fallida
                resetRequest.fail(() => {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: reset_application.config.globals.posicion[2] });
                });

                //Siempre
                resetRequest.always(() => {
                    component.$buttonReset.stop();
                });
            });
        })();

        return components;
    }
};

//Inicializar la aplicación
$(document).ready(() => {
    reset_application.initialize();
    reset_application.application();
});
