var application = {
    config: {},
    initialize: function () { },
    application: function () { }
}

//Configuraciones globales de la aplicación
application.config = {
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
        onOpening: function () { },
        onOpened: function () { },
        onClosing: function () { },
        onClosed: function () { }
    },

    //Declaraciones globales
    globals: {
        root: $("html").attr("data-root") + "/",
        posicion: ["bottomRight", "bottomLeft", "topRight", "topLeft", "topCenter", "bottomCenter", "center"]
    }
}

//Inicializa los elmenetos a utilizar
application.initialize = () => {
    "use strict";

    //Acordeon
    $('.ui.accordion').accordion("close", 0);

    //Se deshabilita el caché de ajax para evitar cacheo de internet explorer.
    $.ajaxSetup({ cache: false });

    //Configuración de los botones ladda
    if (window.Ladda) Ladda.bind(".ladda-button");

    //Set configuración IziToast
    iziToast.settings(application.config.iziToast);

    //Escondo el filtro si no estoy en el inicio
    if (window.location.href != (application.config.globals.root + "Inicio/Inicio")) {
        $("#search-input-menu")[0].style.display = "none";
    } else {
        $("#search-input-menu")[0].style.display = "";
    }

    //Si se desea ver una orden se esconde el parámetro
    if (window.location.href.indexOf("orden") > -1) {
        //Reemplazo el link
        history.replaceState({}, null, application.config.globals.root + "Compras/Compra");
    }

    //Elimino localstorage
    localStorage.removeItem("filtro-subcategoria");

    //Actualiza el indicador del carrito
    let actualizarCarrito = () => {
        let updateRequest = $.ajax({
            url: application.config.globals.root + "Inicio/ActualizarIndicadorCarrito",
            type: "GET",
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            dataType: "json"
        });

        //Petición satisfactoria
        updateRequest.done((data) => {
            if (data.tipo === 1) {
                let $itemCarrito = $("#item-indicador-carrito")[0],
                    $labelIndicador = $("#live_message_badge_main_header", $itemCarrito)[0],
                    $indicador = $(".live_message_badge", $itemCarrito)[0];

                if (data.cantidad > 0) {
                    $indicador.innerText = data.cantidad;
                    $labelIndicador.style.display = "";
                } else {
                    $labelIndicador.style.display = "none";
                }
            } else {
                iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
            }
        });

        //Petición fallida
        updateRequest.fail(() => {
            iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
        });
    }

    actualizarCarrito();

    //Valido si debo notificar mensaje de orden confirmada
    if (localStorage.getItem("mensaje-confirmacíon-orden") != null) {
        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: localStorage.getItem("mensaje-confirmacíon-orden"), position: application.config.globals.posicion[2] });

        //Elimino el localStorage para no notificar de nuevo
        localStorage.removeItem("mensaje-confirmacíon-orden");
    }

    //Valido si debo notificar mensaje de orden cancelada
    if (localStorage.getItem("mensaje-cancelar-orden") != null){
        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: localStorage.getItem("mensaje-cancelar-orden"), position: application.config.globals.posicion[2] });

        //Elimino el localStorage para no notificar de nuevo
        localStorage.removeItem("mensaje-cancelar-orden");
    }

    //Valido si debo notificar información actualizada
    if (localStorage.getItem("mensaje-update-info") != null){
        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: localStorage.getItem("mensaje-update-info"), position: application.config.globals.posicion[2] });

        //Elimino el localStorage para no notificar de nuevo
        localStorage.removeItem("mensaje-update-info");
    }

    //Valido si debo notificar cambio de contraseña
    if (localStorage.getItem("mensaje-change-password") != null) {
        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: localStorage.getItem("mensaje-change-password"), position: application.config.globals.posicion[2] });

        //Elimino el localStorage para no notificar de nuevo
        localStorage.removeItem("mensaje-change-password");
    }

    //Valido si debo mostrar mensaje de crear dirección
    if (localStorage.getItem("mensaje-crear-direccion") != null) {
        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: localStorage.getItem("mensaje-crear-direccion"), position: application.config.globals.posicion[2] });

        //Elimino el localStorage para no notificar de nuevo
        localStorage.removeItem("mensaje-crear-direccion");
    }

    //Valido y muestro mensaje de update directino
    if (localStorage.getItem("mensaje-update-direction") != null) {
        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: localStorage.getItem("mensaje-update-direction"), position: application.config.globals.posicion[2] });

        //Elimino el localStorage para no notificar de nuevo
        localStorage.removeItem("mensaje-update-direction");
    }

    //Valido y muestro mensaje de delete direction
    if (localStorage.getItem("mensaje-delete-direction") != null) {
        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: localStorage.getItem("mensaje-delete-direction"), position: application.config.globals.posicion[2] });

        //Elimino el localStorage para no notificar de nuevo
        localStorage.removeItem("mensaje-delete-direction");
    }

    //Valido si tengo que mostar mensaje add stock
    if (localStorage.getItem("mensaje-add-stock") != null) {
        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: localStorage.getItem("mensaje-add-stock"), position: application.config.globals.posicion[2] });

        //Elimino el localStorage para no notificar de nuevo
        localStorage.removeItem("mensaje-add-stock");
    }

    //Valido si tengo que mostar mensaje crear artículo
    if (localStorage.getItem("mensaje-create-article") != null) {
        iziToast.success({ icon: "fa fa-check", title: "Correcto", message: localStorage.getItem("mensaje-create-article"), position: application.config.globals.posicion[2] });

        //Elimino el localStorage para no notificar de nuevo
        localStorage.removeItem("mensaje-create-article");
    }    

    //Dropdownlist
    $("#lista-departamentos").dropdown({
        clearable: true,
        placeholder: 'Departamento'
    });

    //Dropdosnlist
    $("#lista-municipios").dropdown({
        clearable: true,
        placeholder: 'Municipio'
    });
};

//Funciones y métodos a utilizar
application.application = () => {
    "use strict"

    //Lista de funciones y métodos vacía
    let functions = {
        components: {}
    }

    //Declaración de funcioens y métodos
    functions: {
        let components = {};

        //Filtro los artículos por el texto del input
        let filtro_input = ($filter, encontrado) => {
            //Recorre los artículos buscando coincidencias en el nombre
            $(".four.wide.column").each((index, element) => {
                let titulo = $(".ui.centered.small.header", element)[0].innerText,
                    filtros = $filter.split(' ');

                for (var ele in filtros) {
                    if (titulo.toUpperCase().indexOf(filtros[ele].toUpperCase()) > -1) {
                        encontrado = true;
                        element.style.display = "";
                        document.querySelector("#mensaje-sin-coincidencias").style.display = "none";
                    } else {
                        element.style.display = "none";
                    }
                }
            });

            //Muestro mensaje si no hay coincidencias
            if (!encontrado)
                document.querySelector("#mensaje-sin-coincidencias").style.display = "inherit";
        }

        //Filtro los artículos por sub categoría
        let filtro_subCategoria = (subCategoria, encontrado) => {
            //Recorre los artículos buscando coincidencias con la categoría
            $(".four.wide.column").each((index, element) => {
                let $subCat = element.attributes.getNamedItem("data-subCategorie").value;

                if ($subCat === subCategoria) {
                    encontrado = true;
                    element.style.display = "";
                    document.querySelector("#mensaje-sin-coincidencias").style.display = "none";
                } else {
                    element.style.display = "none";
                }
            });

            //Muestro mensaje si no hay coincidencias
            if (!encontrado)
                document.querySelector("#mensaje-sin-coincidencias").style.display = "inherit";
        }

        //Filtro los artículos por el texto del input y por sub categoría
        let filtro_input_subCategoria = ($filtro, subCategoria, encontrado) => {
            let encontrados = [],
                no_encontrados = [];

            //Recorre los artículos buscando coincidencias con la categoría
            $(".four.wide.column").each((index, element) => {
                let $subCat = element.attributes.getNamedItem("data-subCategorie").value;

                if ($subCat === subCategoria) {
                    encontrados.push(element);
                } else {
                    no_encontrados.push(element);   
                }
            });

            //Recorro los elementos que no son de la categoría y los escondo
            no_encontrados.forEach((elemento) => {
                elemento.style.display = "none";
            });

            //Recorro los elementos encontrados para filtrarlos por el filtro del input
            encontrados.forEach((elemento) => {
                let titulo = $(".ui.centered.small.header", elemento)[0].innerText,
                    filtros = $filtro.split(' ');

                for (var ele in filtros) {
                    if (titulo.toUpperCase().indexOf(filtros[ele].toUpperCase()) > -1) {
                        encontrado = true;
                        elemento.style.display = "";
                        document.querySelector("#mensaje-sin-coincidencias").style.display = "none";
                    } else {
                        elemento.style.display = "none";
                    }
                }
            });

            //Muestro mensaje si no hay coincidencias
            if (!encontrado)
                document.querySelector("#mensaje-sin-coincidencias").style.display = "inherit";
        }

        //Actualiza el indicador del carrito
        let actualizarCarrito = () => {
            let updateRequest = $.ajax({
                url: application.config.globals.root + "Inicio/ActualizarIndicadorCarrito",
                type: "GET",
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                dataType: "json"
            });

            //Petición satisfactoria
            updateRequest.done((data) => {
                if (data.tipo === 1) {
                    let $itemCarrito = $("#item-indicador-carrito")[0],
                        $labelIndicador = $("#live_message_badge_main_header", $itemCarrito)[0],
                        $indicador = $(".live_message_badge", $itemCarrito)[0];

                    if (data.cantidad > 0) {
                        $indicador.innerText = data.cantidad;
                        $labelIndicador.style.display = "";
                    } else {
                        $labelIndicador.style.display = "none";
                    }
                } else {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                }
            });

            //Petición fallida
            updateRequest.fail(() => {
                iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
            });
        }

        //Handlers para la vista de inicio y el menú 
        components.handlers = ((data) => {
            //Componentes de la vista
            let component = {};

            //Click del del botón cerrar sesión
            $("#close-session-button").click(() => {
                //Petición al controlador
                let closeSessionRequest = $.ajax({
                    url: application.config.globals.root + "Inicio/CloseSession",
                    type: "GET",
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    dataType: "json"
                });

                //Petición satisfactoria
                closeSessionRequest.done((data) => {
                    if (data.tipo === 1) {
                        //Elimino local storage del usuario
                        localStorage.removeItem("sesion-usuario");
                        //Redirigo al login
                        location.href = application.config.globals.root + "Login/Login";
                    } else {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                    }
                });

                //Petición fallida
                closeSessionRequest.fail(() => {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                });
            });

            //Enter del filtro
            $("#filter-input-menu").keypress((k) => {
                //Sólo si es enter
                if (k.which === 13) {
                    let $filter = $("#filter-input-menu").val(),
                        subCategoria = localStorage.getItem("filtro-subcategoria"),
                        encontrado = false;

                    if (subCategoria === null) {
                        filtro_input($filter, encontrado);
                    } else {
                        filtro_input_subCategoria($filter, subCategoria, encontrado);
                    }
                }
            });

            //Click de alguna categoria
            $(".filter-subcategorie").click((sc) => {
                let subCategoria = sc.currentTarget.attributes.getNamedItem("data-filter").value,
                    $filtro = $("#filter-input-menu").val(),
                    encontrado = false;

                if ($filtro === "") {
                    filtro_subCategoria(subCategoria, encontrado);
                } else {
                    filtro_input_subCategoria($filtro, subCategoria, encontrado);
                }

                //Guardo categoría en localstorage
                localStorage.setItem("filtro-subcategoria", subCategoria);

                //Elimina el active de la subcategoria si es que hay
                if ($(".active", $(".ui.vertical.menu")).length > 0)
                    $(".active", $(".ui.vertical.menu"))[0].className = "item filter-subcategorie";

                //Selecciona la categoría a la que se le dió clic
                sc.currentTarget.className = "item filter-subcategorie active";
            });

            //Click del botón ver más
            $(".show.more.button").each((index, element) => {
                $(element).click(() => {
                    let correlativo = element.attributes.getNamedItem("data-inventario").value;

                    //Animo y desactivo el botón
                    element.className = "fluid ui animated fade red loading disabled button show more button";

                    //Cargo la información en la ventana modal
                    $("#articulo-modal-view").load(application.config.globals.root + "Inicio/ArticuloModalPartialView?correlativoInventario=" + correlativo,
                        () => {
                            //Desanimo y activo el botón
                            element.className = "fluid ui animated fade red button show more button";

                            //Muesro la ventana modal
                            $("#articulo-modal-view")
                                .modal({
                                    onVisible: () => {
                                        //Slide fotos
                                        new Glide($(".glide", $("#articulo-modal-view"))[0]).mount({
                                            type: 'carousel'
                                        })
                                    }
                                })
                                .modal("show")

                            //Agrego evento al botón agregar al carrito
                            //Click del botón agregar al carrito
                            $("#btnAgregarACarrito").click((button) => {
                                let $cantidad = $("#txt_cantidad").val(),
                                    $inventario = $("#inventario-para-carrito").val();

                                if ($cantidad === "") {
                                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Ingrese una cantidad", position: application.config.globals.posicion[2] });
                                } else {
                                    let carritoRequest = $.ajax({
                                        url: application.config.globals.root + "Inicio/GuardarEnCarrito",
                                        type: "POST",
                                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                                        data: {
                                            inventario: $inventario,
                                            cantidad: $cantidad
                                        },
                                        dataType: "json",
                                        beforeSend: () => {
                                            //Animo y desactivo el botón
                                            button.currentTarget.className = "fluid ui animated fade orange loading disabled button";
                                        }
                                    });

                                    //Petición satisfactoria
                                    carritoRequest.done((data) => {
                                        if (data.tipo === 1) {
                                            //Escondo la ventana modal
                                            $("#articulo-modal-view").modal("hide");
                                            //Modifico el contador de artículos en el carrito
                                            actualizarCarrito();
                                        } else {
                                            iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                                        }
                                    });

                                    //Petición fallida
                                    carritoRequest.fail(() => {
                                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                                    });

                                    //Petición siempre
                                    carritoRequest.always(() => {
                                        //Desanimo y activo el botón
                                        button.currentTarget.className = "fluid ui animated fade orange button";
                                    });
                                }
                            });
                        });
                })
            });

            //Click del botón limpiar filtros
            $("#limpiar-filtro").click(() => {
                //Recargo la página para borra los filtros
                window.location.href = application.config.globals.root + "Inicio/Inicio";
            });

            //Click del botón del carrito
            $("#item-indicador-carrito").click(() => {
                window.location.href = application.config.globals.root + "Carrito/Carrito";
            });

            //Click del botón compras
            $("#item-menu-compras").click(() => {
                window.location.href = application.config.globals.root + "Compras/Compras";
            });

            //Click del botón perfil
            $("#button-view-profile").click(() => {
                //Redirigo
                window.location.href = application.config.globals.root + "Perfil/Perfil";
            });

            //Click del botón eliminar artículo del carrito
            $(".boton.eliminar.articulo.carrito").each((index, element) => {
                $(element).click(() => {
                    let correlativo = element.attributes.getNamedItem("data-inventario").value;

                    let removeRequest = $.ajax({
                        url: application.config.globals.root + "Carrito/EliminarArticulo",
                        type: "POST",
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        data: {
                            correlativo: correlativo
                        },
                        dataType: "json",
                        beforeSend: () => {
                            //Animo y desactivo el botón
                            element.className = "ui animated fade red loading disabled button boton eliminar articulo carrito";
                        }
                    });

                    //Petición satisfactoria
                    removeRequest.done((data) => {
                        if (data.tipo === 1) {
                            window.location.href = application.config.globals.root + "Carrito/Carrito";
                        } else {
                            iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                        }
                    });

                    //Petición fallida
                    removeRequest.fail(() => {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                    });

                    //Petición siempre
                    removeRequest.always(() => {
                        //Desanimo y activo
                        element.className = "ui animated fade red button boton eliminar articulo carrito";
                    });
                });
            });

            //Click del botón ordenar
            $("#btn-ordenar-compra").click(() => {
                $("#confirm-order-modal")
                    .modal({
                        onHidden: () => {
                            //Deselecciono las direcciones
                            $(".tarjeta.direccion").each((index, element) => {
                                element.className = "ui raised centered link card tarjeta direccion";
                                element.style.border = "";
                                element.style.borderRadius = "";
                            });
                        }
                    })
                    .modal("show");
            });

            //Click de las tarjetas de dirección
            $(".tarjeta.direccion").each((index, element) => {
                $(element).click(() => {
                    let tarjeta = $(".tarjeta.direccion.active");

                    if (tarjeta.length > 0) {
                        //Deselecciono si es que hay alguna seleccionada
                        tarjeta[0].className = "ui raised centered link card tarjeta direccion";
                        tarjeta[0].style.border = "";
                        tarjeta[0].style.borderRadius = "";
                    }

                    //Selecciono la tarjeta
                    element.className = "ui raised centered link card tarjeta direccion active";
                    element.style.border = "3px solid green";
                    element.style.borderRadius = "5px";
                });
            });

            //Click del botón confirmar orden
            $("#btn-confirmar-orden-compra").click((element) => {
                let seleccionada = false,
                    $direccion = 0;

                $(".tarjeta.direccion").each((index, element) => {
                    if (element.className.indexOf("active") > -1) {
                        seleccionada = true;
                        $direccion = element.attributes.getNamedItem("data-direccion").value;
                    }
                });

                if (!seleccionada) {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Seleccione una dirección", position: application.config.globals.posicion[2] });
                } else {
                    let confirmRequest = $.ajax({
                        url: application.config.globals.root + "Compras/ConfirmarOrden",
                        type: "POST",
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        data: {
                            direccionEntrega: $direccion
                        },
                        dataType: "json",
                        beforeSend: () => {
                            //Animo y desactivo
                            element.currentTarget.className = "fluid ui animated fade olive loading disabled button";
                        }
                    });

                    //Petición satisfactoria
                    confirmRequest.done((data) => {
                        if (data.tipo === 1) {
                            //Guardo mensaje en localstorage
                            localStorage.setItem("mensaje-confirmacíon-orden", data.mensaje);
                            //Redirigo
                            window.location.href = application.config.globals.root + "Compras/Compra?orden=" + data.orden;
                        } else {
                            iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                        }
                    });

                    //Petición fallida
                    confirmRequest.fail(() => {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                    });

                    //Petición siempre
                    confirmRequest.always(() => {
                        //Desanimo y activo
                        element.currentTarget.className = "fluid ui animated fade olive button";
                    });
                }
            });

            //Click del botón cancelar orden
            $("#btn-cancelar-orden").click((button) => {
                let orden = button.currentTarget.attributes.getNamedItem("data-orden").value;

                let cancelRequest = $.ajax({
                    url: application.config.globals.root + "Compras/CancelarOrden",
                    type: "POST",
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    data: {
                        orden: orden
                    },
                    dataType: "json",
                    beforeSend: () => {
                        //Animo y desactivo
                        button.currentTarget.className = "fluid ui animated fade orange loading disabled button";
                    }
                });

                //Petición satisfactoria
                cancelRequest.done((data) => {
                    if (data.tipo === 1) {
                        //Guardo mensaje en localstorage
                        localStorage.setItem("mensaje-cancelar-orden", data.mensaje);
                        //Redirigo
                        window.location.href = application.config.globals.root + "Compras/Compra?orden=" + orden;
                    } else {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                    }
                });

                //Petición fallida
                cancelRequest.fail(() => {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                });

                //Petición siempre
                cancelRequest.always(() => {
                    //Desanimo y activo
                    button.currentTarget.className = "fluid ui animated fade orange button";
                });
            });

            //Click de la fila de una orden
            $(".fila.orden.compra").each((index, element) => {
                $(element).dblclick(() => {
                    let $orden = element.attributes.getNamedItem("data-orden-compra").value;

                    //Redirigo
                    window.location.href = application.config.globals.root + "Compras/Compra?orden=" + $orden;
                });
            });

            //Datatable a tabla de ordenes
            $("#tabla-ordenes-compra").DataTable({
                "language": {
                    "lengthMenu": "Mostrar _MENU_ registros",
                    "zeroRecords": "0 ordenes encontradas",
                    "info": "Página _PAGE_ de _PAGES_",
                    "infoEmpty": "No se encontraron ordenes",
                    "infoFiltered": "(filtradas de _MAX_ ordenes totales)",
                    "decimal": ".",
                    "thousands": ",",
                    "search": "Buscar:",
                    "paginate": {
                        "first": "Primera",
                        "last": "Ultima",
                        "next": "Siguiente",
                        "previous": "Anterior"
                    },
                    "loadingRecords": "Cargando...",
                    "processing": "Procesando...",
                    "paging": "false"
                }
            });

            //DataTable a tabla de artículos
            $("#tabla-articulos-admin").DataTable({
                "language": {
                    "lengthMenu": "Mostrar _MENU_ registros",
                    "zeroRecords": "0 artículos encontrados",
                    "info": "Página _PAGE_ de _PAGES_",
                    "infoEmpty": "No se encontraron artículos",
                    "infoFiltered": "(filtrados de _MAX_ artículos totales)",
                    "decimal": ".",
                    "thousands": ",",
                    "search": "Buscar:",
                    "paginate": {
                        "first": "Primera",
                        "last": "Ultima",
                        "next": "Siguiente",
                        "previous": "Anterior"
                    },
                    "loadingRecords": "Cargando...",
                    "processing": "Procesando...",
                    "paging": "false"
                }
            });

            //Click del botón actualizar información
            $("#btn-show-update-info").click(() => {
                $("#update-info-modal")
                    .modal({
                        onVisible: () => {
                            //Set calendar para fecha de nacimiento
                            $("#calendar-update-info").calendar({
                                type: "date",
                                text: {
                                    days: ['D', 'L', 'M', 'Mi', 'J', 'V', 'S'],
                                    months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                                    monthsShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dec'],
                                    today: 'Hoy',
                                    now: 'Ahora'
                                }
                            });
                        },
                        onHidden: () => {
                            //Reinicia el formulario
                            document.querySelector("#update-info-form").reset();
                            //Set calendar para fecha de nacimiento
                            $("#calendar-update-info").calendar({
                                type: "date",
                                text: {
                                    days: ['D', 'L', 'M', 'Mi', 'J', 'V', 'S'],
                                    months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                                    monthsShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dec'],
                                    today: 'Hoy',
                                    now: 'Ahora'
                                }
                            });
                        }
                    })
                    .modal("show");
            });

            //Click del botón cambiar contraseña
            $("#btn-show-change-password").click(() => {
                $("#change-password-modal")
                    .modal({
                        onHidden: () => {
                            //Reinicia el formulario
                            document.querySelector("#change-password-form").reset();
                        }
                    })
                    .modal("show");
            });

            //Click del botón agregar dirección
            $("#btn-show-add-direction").click(() => {
                $("#add-direction-modal")
                    .modal({
                        onHidden: () => {
                            //Reinicia el formulario
                            document.querySelector("#create-direction-form").reset();
                            //Reinicia dropdown
                            $("#lista-departamentos").dropdown("clear");
                            //Reinicia dropdown
                            $("#lista-municipios").dropdown("clear");
                        }
                    })
                    .modal("show");
            });

            //Submit form update info
            $("#update-info-form").submit((e) => {
                e.preventDefault();
                e.stopPropagation();

                let $form = $("#update-info-form"),
                    $button = $(".boton.update", $form)[0];

                let updateRequest = $.ajax({
                    url: application.config.globals.root + "Perfil/UpdateInfo",
                    type: "POST",
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    data: $form.serialize(),
                    dataType: "json",
                    beforeSend: () => {
                        //Animo y desactivo
                        $button.className = "fluid ui animated fade large orange submit loading disabled button boton update";
                    }
                });

                //Petición satisfactoria
                updateRequest.done((data) => {
                    if (data.tipo === 1) {
                        //Set del local storage del usuario y el mensaje
                        localStorage.setItem("sesion-usuario", data.correo);
                        localStorage.setItem("mensaje-update-info", data.mensaje);
                        //Redirigo
                        window.location.href = application.config.globals.root + "Perfil/Perfil";
                    } else {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                    }
                });

                //Petición fallida
                updateRequest.fail(() => {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                });

                //Petición siempre
                updateRequest.always(() => {
                    //Desanimo y activo
                    $button.className = "fluid ui animated fade large orange submit button boton update";
                });
            });

            //Submit formulario change password
            $("#change-password-form").submit((e) => {
                e.preventDefault();
                e.stopPropagation();

                let $form = $("#change-password-form"),
                    $button = $(".boton.change", $form)[0];

                let changeRequest = $.ajax({
                    url: application.config.globals.root + "Perfil/ChangePassword",
                    type: "POST",
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    data: $form.serialize(),
                    dataType: "json",
                    beforeSend: () => {
                        //Animo y desactivo
                        $button.className = "fluid ui animated fade large orange submit loading disabled button boton change";
                    }
                });

                //Petición satisfactoria
                changeRequest.done((data) => {
                    if (data.tipo === 1) {
                        //Set del local storage del usuario y el mensaje
                        localStorage.setItem("mensaje-change-password", data.mensaje);
                        //Redirigo
                        window.location.href = application.config.globals.root + "Perfil/Perfil";
                    } else {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                    }
                });

                //Petición fallida
                changeRequest.fail(() => {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                });

                //Petición siempre
                changeRequest.always(() => {
                    //Desanimo y activo
                    $button.className = "fluid ui animated fade large orange submit button boton change";
                });
            });

            //Onchange departamento add direction
            $("#lista-departamentos").change(() => {
                let $depto = $("#lista-departamentos").val();

                if ($depto != "") {
                    let getRequest = $.ajax({
                        url: application.config.globals.root + "Perfil/GetMunicipios",
                        type: "POST",
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        data: {
                            departamento: $depto
                        },
                        dataType: "json"
                    });

                    //Petición satisfactoria
                    getRequest.done((data) => {
                        if (data.tipo === 1) {
                            let html = '<option value="">Municipio</option>';

                            for (var index = 0; index < data.listaMunicipios.length; index++) {
                                html += '<option value="' + data.listaMunicipios[index].Value + '">' + data.listaMunicipios[index].Text + '</option>';
                            }

                            //Clear
                            $("#lista-municipios").dropdown("clear");
                            //Set html
                            $("#lista-municipios").html(html).show()
                        } else {
                            iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                        }
                    });

                    //Petición fallida
                    getRequest.fail(() => {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                    });
                } else {
                    $("#lista-municipios").dropdown("clear");
                }
            });

            //Submit formulario crear dirección
            $("#create-direction-form").submit((e) => {
                e.preventDefault();
                e.stopPropagation()

                let $form = $("#create-direction-form"),
                    $button = $(".boton.crear.direccion", $form)[0];

                let createRequest = $.ajax({
                    url: application.config.globals.root + "Perfil/CreateDirection",
                    type: "POST",
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    data: $form.serialize(),
                    dataType: "json",
                    beforeSend: () => {
                        //Animo y desactivo
                        $button.className = "fluid ui animated fade large orange submit loading disabled button boton crear direccion";
                    }
                });

                //Petición satisfactoria
                createRequest.done((data) => {
                    if (data.tipo === 1) {
                        //Guardo mensaje en localstorage
                        localStorage.setItem("mensaje-crear-direccion", data.mensaje);
                        //Redirigo
                        window.location.href = application.config.globals.root + "Perfil/Perfil";
                    } else {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                    }
                });

                //Petición fallida
                createRequest.fail(() => {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                });

                //Petición siempre
                createRequest.always(() => {
                    //Desanimo y activo
                    $button.className = "fluid ui animated fade large orange submit button boton crear direccion";
                });
            });

            //Click del botón para ir al perfil y agregar direcciónes
            $("#btn-redirigir-agregar-direccion").click(() => {
                window.location.href = application.config.globals.root + "Perfil/Perfil";
            }); 

            //Click del botón editar dirección
            $("#boton-editar-direccion").click(() => {
                let seleccionada = false,
                    $direccion = 0,
                    $departamento = 0,
                    $municipio = 0;

                $(".tarjeta.direccion").each((index, element) => {
                    if (element.className.indexOf("active") > -1) {
                        seleccionada = true;
                        $direccion = element.attributes.getNamedItem("data-direccion").value;
                        $departamento = element.attributes.getNamedItem("data-departamento").value;
                        $municipio = element.attributes.getNamedItem("data-municipio").value;
                    }
                });

                if (!seleccionada) {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Seleccione una dirección", position: application.config.globals.posicion[2] });
                } else {
                    $("#edit-direction-modal").load(application.config.globals.root + "Perfil/EditDirectionPartialView?direccion=" + $direccion, () => {
                        $("#edit-direction-modal")
                            .modal({
                                onHidden: () => {
                                    //Deselecciono las direcciones
                                    $(".tarjeta.direccion").each((index, element) => {
                                        element.className = "ui raised centered link card tarjeta direccion";
                                        element.style.border = "";
                                        element.style.borderRadius = "";
                                    }); 
                                },
                                onVisible: () => {
                                    //Dropdownlist
                                    $("#lista-departamentos-edit")
                                        .dropdown('set selected', $departamento)
                                        .dropdown({
                                            clearable: true,
                                            placeholder: 'Departamento'
                                        });

                                    //Dropdosnlist
                                    $("#lista-municipios-edit")
                                        .dropdown('set selected', $municipio)
                                        .dropdown({
                                            clearable: true,
                                            placeholder: 'Municipio'
                                        });

                                    $("#lista-departamentos-edit").change(() => {
                                        let $depto = $("#lista-departamentos-edit").val();

                                        if ($depto != "") {
                                            let getRequest = $.ajax({
                                                url: application.config.globals.root + "Perfil/GetMunicipios",
                                                type: "POST",
                                                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                                                data: {
                                                    departamento: $depto
                                                },
                                                dataType: "json"
                                            });

                                            //Petición satisfactoria
                                            getRequest.done((data) => {
                                                if (data.tipo === 1) {
                                                    let html = '<option value="">Municipio</option>';

                                                    for (var index = 0; index < data.listaMunicipios.length; index++) {
                                                        html += '<option value="' + data.listaMunicipios[index].Value + '">' + data.listaMunicipios[index].Text + '</option>';
                                                    }

                                                    //Clear
                                                    $("#lista-municipios-edit").dropdown("clear");
                                                    //Set html
                                                    $("#lista-municipios-edit").html(html).show();
                                                } else {
                                                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                                                }
                                            });

                                            //Petición fallida
                                            getRequest.fail(() => {
                                                iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                                            });
                                        } else {
                                            $("#lista-municipios").dropdown("clear");
                                        }
                                    });

                                    //Submit form editar dirección
                                    $("#edit-direction-form").submit((e) => {
                                        e.preventDefault();
                                        e.stopPropagation();

                                        let $form = $("#edit-direction-form"),
                                            $button = $(".boton.editar.direccion", $form)[0];

                                        let editRequest = $.ajax({
                                            url: application.config.globals.root + "Perfil/EditDirection",
                                            type: "POST",
                                            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                                            data: $form.serialize(),
                                            dataType: "json",
                                            beforeSend: () => {
                                                //Animo y desactivo
                                                $button.className = "fluid ui animated fade large orange submit loading disabled button boton editar direccion";
                                            }
                                        });

                                        //Petición satisfactoria
                                        editRequest.done((data) => {
                                            if (data.tipo === 1) {
                                                //Set del local storage del usuario y el mensaje
                                                localStorage.setItem("mensaje-update-direction", data.mensaje);
                                                //Redirigo
                                                window.location.href = application.config.globals.root + "Perfil/Perfil";
                                            } else {
                                                iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                                            }
                                        });

                                        //Petición fallida
                                        editRequest.fail(() => {
                                            iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                                        });

                                        //Petición siempre
                                        editRequest.always(() => {
                                            //Desanimo y activo
                                            $button.className = "fluid ui animated fade large orange submit button boton editar direccion";
                                        });
                                    });
                                }
                            })
                            .modal("show");
                    });
                }
            });

            //Click botón elimiar dirección
            $("#boton-eliminar-direccion").click(() => {
                let seleccionada = false,
                    $direccion = 0;

                $(".tarjeta.direccion").each((index, element) => {
                    if (element.className.indexOf("active") > -1) {
                        seleccionada = true;
                        $direccion = element.attributes.getNamedItem("data-direccion").value;
                    }
                });

                if (!seleccionada) {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Seleccione una dirección", position: application.config.globals.posicion[2] });
                } else {
                    let $button = $("#boton-eliminar-direccion");

                    let deleteRequest = $.ajax({
                        url: application.config.globals.root + "Perfil/DeleteDirection",
                        type: "POST",
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        data: {
                            direccion: $direccion
                        },
                        dataType: "json",
                        beforeSend: () => {
                            //Animo y desactivo
                            $button.className = "fluid ui animated fade large red submit loading disabled button";
                        }
                    });

                    //Petición satisfactoria
                    deleteRequest.done((data) => {
                        if (data.tipo === 1) {
                            //Set del local storage del usuario y el mensaje
                            localStorage.setItem("mensaje-delete-direction", data.mensaje);
                            //Redirigo
                            window.location.href = application.config.globals.root + "Perfil/Perfil";
                        } else {
                            iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                        }
                    });

                    //Petición fallida
                    deleteRequest.fail(() => {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                    });

                    //Petición siempre
                    deleteRequest.always(() => {
                        //Desanimo y activo
                        $button.className = "fluid ui animated fade large red submit button";
                    });
                }
            });

            //Click del botón administrar artículos
            $("#button-admin-articles").click(() => {
                //Redirigo
                window.location.href = application.config.globals.root + "Articulos/Articulos";
            });

            //Click del botón agregar stock
            $(".boton.agregar.stock.articulo.admin").each((index, element) => {
                $(element).click(() => {
                    let $modal = $("#add-stock-modal"),
                        $form = $('form[name="add-stock-form"]', $modal),
                        $input = $('input[name="articulo"]'),
                        $article = element.attributes.getNamedItem("data-articulo").value,
                        $name = element.attributes.getNamedItem("data-nombre-articulo").value,
                        $span = $('span[name="nombre-articulo-span"]', $modal);

                    //Set value article
                    $($input).val($article);
                    $span[0].innerText = $name;

                    $($modal).modal({
                            onHidden: () => {
                                //Reinicio formulario
                                document.querySelector("#add-stock-form").reset();
                            },
                            onVisible: () => {
                                //Calendar
                                $("#calendario-add-stock").calendar({
                                    type: "date",
                                    text: {
                                        days: ['D', 'L', 'M', 'Mi', 'J', 'V', 'S'],
                                        months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                                        monthsShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dec'],
                                        today: 'Hoy',
                                        now: 'Ahora'
                                    }
                                });
                            }
                        })
                        .modal("show");
                });
            });

            //Submit form add stock
            $("#add-stock-form").submit((e) => {
                e.preventDefault();
                e.stopPropagation();

                let $form = $("#add-stock-form"),
                    $button = $(".boton.add.stock", $form);

                let addRequest = $.ajax({
                    url: application.config.globals.root + "Articulos/AddStock",
                    type: "POST",
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    data: $form.serialize(),
                    dataType: "json",
                    beforeSend: () => {
                        //Animo y desactivo
                        $button.className = "fluid ui animated fade large orange submit loading disabled button boton add stock";
                    }
                });

                //Petición satisfactoria
                addRequest.done((data) => {
                    if (data.tipo === 1) {
                        //Set del local storage del usuario y el mensaje
                        localStorage.setItem("mensaje-add-stock", data.mensaje);
                        //Redirigo
                        window.location.href = application.config.globals.root + "Articulos/Articulos";
                    } else {
                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                    }
                });

                //Petición fallida
                addRequest.fail(() => {
                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                });

                //Petición siempre
                addRequest.always(() => {
                    //Desanimo y activo
                    $button.className = "fluid ui animated fade large orange submit button boton add stock";
                });
            });

            //Click botón agregar artículo
            $("#boton-agregar-articulo").click(() => {
                $("#add-article-modal")
                    .modal({
                        onHidden: () => {
                            //Reinicio
                            document.querySelector("#create-article-form").reset();
                            //Reinicio
                            $("#lista-sub-categorias").dropdown("clear");
                            //Reinicio
                            $("#lista-categorias").dropdown("clear");
                            //Reset
                            $("#create-article-form").unbind("submit");
                            //Reset
                            $("#lista-categorias").unbind("change");
                            //Reset
                            $("input:text").unbind("click");
                            //Reset
                            $('input:file', '.ui.action.input').unbind("change");
                        },
                        onVisible: () => {
                            //Submit crear articulo
                            $("#create-article-form").submit((e) => {
                                e.stopPropagation();
                                e.preventDefault();

                                let $form = $("#create-article-form"),
                                    $button = $(".boton.crear.articulo", $form)[0],
                                    $data = $form.serialize(),
                                    formData = new FormData(),
                                    $imagenes = $('input[name="imagenes"]', $form)[0].files;

                                $($imagenes).each((index, element) => {
                                    let size = element.size;

                                    if (size > 4000000) {
                                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Las imágenes deben pesar como máximo 4MB", position: application.config.globals.posicion[2] });
                                        return;
                                    }

                                    formData.append("imagenes", element);
                                });

                                let createRequest = $.ajax({
                                    url: application.config.globals.root + "Articulos/CreateArticle?" + $data,
                                    type: "POST",
                                    data: formData,
                                    contentType: false,
                                    processData: false,
                                    dataType: "json",
                                    beforeSend: () => {
                                        //Animo y desactivo
                                        $button.className = "fluid ui animated fade large orange submit loading disabled button boton crear articulo";
                                    }
                                });

                                //Petición satisfactoria
                                createRequest.done((data) => {
                                    if (data.tipo === 1) {
                                        //Set del local storage del usuario y el mensaje
                                        localStorage.setItem("mensaje-create-article", data.mensaje);
                                        //Redirigo
                                        window.location.href = application.config.globals.root + "Articulos/Articulos";
                                    } else {
                                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                                    }
                                });

                                //Petición fallida
                                createRequest.fail(() => {
                                    iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                                });

                                //Petición siempre
                                createRequest.always(() => {
                                    //Desanimo y activo
                                    $button.className = "fluid ui animated fade large orange submit button boton crear articulo";
                                });
                            });

                            //Dropdown
                            $("#lista-categorias").dropdown({
                                clearable: true,
                                placeholder: 'Categoria'
                            });

                            //Dropdown
                            $("#lista-sub-categorias").dropdown({
                                clearable: true,
                                placeholder: 'Subcategoria'
                            });

                            //File
                            $("input:text").click((element) => {
                                $(element.currentTarget).parent().find("input:file").click();
                            });

                            //File
                            $('input:file', '.ui.action.input')
                                .on('change', (e) => {
                                    let value = "";

                                    for (var index = 0; index < e.target.files.length; index++) {
                                        if (index < e.target.files.length - 1) {
                                            value += e.target.files[index].name + ", ";
                                        } else {
                                            value += e.target.files[index].name;
                                        }
                                    }

                                    $('input:text', $(e.target).parent()).val(value);
                                });

                            $("#lista-categorias").change(() => {
                                let $cat = $("#lista-categorias").val();

                                if ($cat != "") {
                                    let getRequest = $.ajax({
                                        url: application.config.globals.root + "Articulos/GetSubCategorias",
                                        type: "POST",
                                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                                        data: {
                                            categoria: $cat
                                        },
                                        dataType: "json"
                                    });

                                    //Petición satisfactoria
                                    getRequest.done((data) => {
                                        if (data.tipo === 1) {
                                            let html = '<option value="">Subcategoria</option>';

                                            for (var index = 0; index < data.listaSub.length; index++) {
                                                html += '<option value="' + data.listaSub[index].Value + '">' + data.listaSub[index].Text + '</option>';
                                            }

                                            //Clear
                                            $("#lista-sub-categorias").dropdown("clear");
                                            //Set html
                                            $("#lista-sub-categorias").html(html).show();
                                        } else {
                                            iziToast.error({ icon: "fa fa-ban", title: "Error", message: data.mensaje, position: application.config.globals.posicion[2] });
                                        }
                                    });

                                    //Petición fallida
                                    getRequest.fail(() => {
                                        iziToast.error({ icon: "fa fa-ban", title: "Error", message: "Error en la llamada al servidor", position: application.config.globals.posicion[2] });
                                    });
                                } else {
                                    $("#lista-sub-categorias").dropdown("clear");
                                }
                            })
                        }
                    })
                    .modal("show");
            });
        })();

        return components;
    }
};

//Inicializar la aplicación
$(document).ready(() => {
    application.initialize();
    application.application();
});