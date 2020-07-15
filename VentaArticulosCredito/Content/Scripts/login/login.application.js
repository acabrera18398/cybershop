var login_application = {
    config: {},
    initialize: () => { },
    application: () => { }
}

//Configuraciones globales de la aplicación
login_application.config = {
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
login_application.initialize = () => {
    "use strict";

    //Se deshabilita el caché de ajax para evitar cacheo de internet explorer.
    $.ajaxSetup({ cache: false });

    //Configuración de los botones ladda
    if (window.Ladda) Ladda.bind(".ladda-button");

    //Set configuración IziToast
    iziToast.settings(login_application.config.iziToast);

    //Set calendar para fecha de nacimiento
    $("#calendar-register").calendar({
        type: "date",
        text: {
            days: ['D', 'L', 'M', 'Mi', 'J', 'V', 'S'],
            months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthsShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dec'],
            today: 'Hoy',
            now: 'Ahora'
        }
    });

    //Valido si debo mostrar notificación de envío de correo de registro
    if (localStorage.getItem("notificar-mail-registro") != null) {
        //Notifico
        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: localStorage.getItem("notificar-mail-registro"), position: login_application.config.globals.posicion[2] });
        //Borro el localStorage
        localStorage.removeItem("notificar-mail-registro");
    }

    //Valido si tengo que notificar restauración exitosa
    if (localStorage.getItem("mensaje-reset-success") != null) {
        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: localStorage.getItem("mensaje-reset-success"), position: login_application.config.globals.posicion[2] });

        //Elimino el localStorage para no notificar de nuevo
        localStorage.removeItem("mensaje-reset-success");
    }

    //Valido si hay una sessión activa
    if (localStorage.getItem("sesion-usuario") != null){
        //Petición de ajax para recargar la sesion
        var sesionRequest = $.ajax({
            url: login_application.config.globals.root + "Login/ReloadSession",
            type: "POST",
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            data: {
                correo: localStorage.getItem("sesion-usuario")
            },
            dataType: "json"
        });

        //Petidicón satisfactoria
        sesionRequest.done((data) => {
            if (data.tipo === 1) {
                //Redirigo a la página de inicio
                location.href = login_application.config.globals.root + "Inicio/Inicio";
            }
        });

        //Petición fallida
        sesionRequest.fail(() => {
            iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: login_application.config.globals.posicion[2] });
        });
    }

    //Recupero el correo de la url
    let $email = location.href.split('=')[1];

    //Valido si debo completar un registro y notificar
    if ($email != null) {
        //Esconde los parámetros para que no sean visibles
        history.replaceState({}, null, login_application.config.globals.root + "Login/Login")

        //Petición de ajax para completar el registro
        var registerCompleteRequest = $.ajax({
            url: login_application.config.globals.root + "Login/CompleteRegister",
            type: "POST",
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            data: {
                email: $email
            },
            dataType: "json"
        });

        //Petidicón satisfactoria
        registerCompleteRequest.done((data) => {
            if (data.tipo === 1) {
                iziToast.success({ icon: "fa fa-check", title: "Correcto", message: data.mensaje, position: login_application.config.globals.posicion[2] });
            } else if (data.tipo === 2){
                //No hago nada
            } else {
                iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: login_application.config.globals.posicion[2] });
            }
        });

        //Petición fallida
        registerCompleteRequest.fail(() => {
            iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: login_application.config.globals.posicion[2] });
        });
    }
};

//Funciones y métodos a utilizar
login_application.application = () => {
    "use strict"

    //Lista de funciones y métodos vacía
    let functions = {
        components: {}
    }

    //Declaración de funcioens y métodos
    functions: {
        let components = {};

        //Handlers para la vista del login
        components.login_view = ((data) => {
            //Componentes de la vista
            let component = {};

            //Click del botón registrarse
            $("#show-register-form").click(() => {
                //Esconde el contenedor del login
                document.querySelector("#login-container").style.display = "none";
                //Muestra el contenedor de registro
                document.querySelector("#register-container").style.display = "flex";
            });

            //Click del boton cancelar registro
            $("#cancel-register-button").click(() => {
                let $loginContainer = document.querySelector("#login-container");

                //Esconde el contenedor de registro
                document.querySelector("#register-container").style.display = "none";
                //Cambia la clase del contenedor del login
                $loginContainer.className = "ui middle aligned center aligned grid animated flipInY";
                //Muestra el contenedor del login
                $loginContainer.style.display = "";
                //Reinicia el formulario
                document.querySelector("#register-form").reset();
            });

            //Click del boton olvide mi contraseña
            $("#show-forgot-password").click(() => {
                $("#reset-password-modal")
                    .modal({
                        onHide: () => {
                            //Reinicia el formulario
                            document.querySelector("#forgot-password-form").reset();
                        }
                    })
                    .modal("setting", "transition", "vertical flip")
                    .modal("show");   
            });

            //Click del boton cancelar olvide mi contraseña
            $("#cancel-forgot-password-button").click(() => {
                //Reinicia el formulario
                document.querySelector("#forgot-password-form").reset();
            });

            //Click del boton iniciar sesion
            $("#login-form").submit((e) => {
                e.preventDefault();
                e.stopPropagation();

                //Set componentes del login
                component.$loginForm = $("#login-form");
                component.$buttonLogin = window.Ladda && Ladda.create(component.$loginForm.find(".ladda-button")[0]);

                //Petición al controlador
                let loginRequest = $.ajax({
                    url: login_application.config.globals.root + "Login/Login",
                    type: "POST",
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    data: component.$loginForm.serialize(),
                    dataType: "json",
                    beforeSend: () => {
                        component.$buttonLogin.start();
                    }
                });

                //Petición satisfactoria
                loginRequest.done((data) => {
                    if (data.tipo === 1) {
                        //Guardo el correo del usuario en un local storage
                        localStorage.setItem("sesion-usuario", data.correo);
                        location.href = login_application.config.globals.root + "Inicio/Inicio";
                    } else if (data.tipo === 2) {
                        iziToast.warning({ icon: "fa fa-exclamation-triangle", title: "Advertencia", message: data.mensaje, position: login_application.config.globals.posicion[2] });
                    } else {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: login_application.config.globals.posicion[2] });
                    }
                });

                //Petición fallida
                loginRequest.fail(() => {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: login_application.config.globals.posicion[2] });
                });

                //Petición siempre
                loginRequest.always(() => {
                    component.$buttonLogin.stop();
                });
            });

            //Submit del formulario registrarse
            $("#register-form").submit((e) => {
                e.preventDefault();
                e.stopPropagation();

                //Set componentes del formulario
                component.$registerForm = $("#register-form");
                component.$buttonRegister = window.Ladda && Ladda.create(component.$registerForm.find(".ladda-button")[0]);

                //Petición al controlador
                let registerRequest = $.ajax({
                    url: login_application.config.globals.root + "Login/Register",
                    type: "POST",
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    data: component.$registerForm.serialize(),
                    dataType: "json",
                    beforeSend: () => {
                        component.$buttonRegister.start();
                    }
                });

                //Petición satisfactoria
                registerRequest.done((data) => {
                    if (data.tipo === 1) {
                        localStorage.setItem("notificar-mail-registro", data.mensaje);
                        location.href = login_application.config.globals.root + "Login/Login";
                    } else {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: login_application.config.globals.posicion[2] });
                    }
                });

                //Petición fallida
                registerRequest.fail(() => {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: login_application.config.globals.posicion[2] });
                });

                //Petición siempre
                registerRequest.always(() => {
                    component.$buttonRegister.stop();
                });
            });

            //Submit del formulario olvide mi contraseña
            $("#button-forgot-password").click((e) => {
                e.preventDefault();
                e.stopPropagation();

                //Set componentes
                component.$forgotForm = $("#forgot-password-form");
                component.$forgotButton = window.Ladda && Ladda.create($("#reset-password-modal").find(".ladda-button")[0]);

                let requestSendForgot = $.ajax({
                    url: login_application.config.globals.root + "Login/SendForgotPassword",
                    type: "POST",
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    data: component.$forgotForm.serialize(),
                    dataType: "json",
                    beforeSend: () => {
                        component.$forgotButton.start();
                    }
                });

                //Petición satisfactoria
                requestSendForgot.done((data) => {
                    if (data.tipo === 1) {
                        document.querySelector("#cancel-forgot-password-button").click();
                        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: data.mensaje, position: login_application.config.globals.posicion[2] });
                    } else {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: login_application.config.globals.posicion[2] });
                    }
                });

                //Petición fallida
                requestSendForgot.fail(() => {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: login_application.config.globals.posicion[2] });
                });

                //Petición siempre
                requestSendForgot.always(() => {
                    component.$forgotButton.stop();
                });
            });
        })();

        return components;
    }
};

//Inicializar la aplicación
$(document).ready(() => {
    login_application.initialize();
    login_application.application();
});