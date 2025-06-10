<%@ Page Title="Gestión de Pedidos" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfPedidos.aspx.vb" Inherits="appWebSistemaRestaurante.wfPedidos" %>

<%-- BLOQUE 1: Contenido Visual (HTML y Estilos) --%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <style>
        .card-mesa-pedido { cursor: pointer; transition: all 0.2s ease; border-width: 2px; }
        .card-mesa-pedido:hover { transform: scale(1.05); box-shadow: 0 5px 20px rgba(0,0,0,0.2); }
        .card-mesa-pedido .icono-mesa { font-size: 2.5rem; }
        .card-mesa-pedido .numero-mesa { font-size: 1.5rem; font-weight: 700; }
        #modalTomaPedido .modal-dialog, #modalPago .modal-dialog { max-width: 95%; }
        .menu-categorias { list-style: none; padding: 0; }
        .menu-categorias .nav-link { cursor: pointer; text-align: center; border: 1px solid #dee2e6; margin-bottom: 5px; }
        .menu-categorias .nav-link.active { background-color: var(--primary); color: white; }
        .producto-item { cursor: pointer; transition: background-color 0.2s ease; }
        .producto-item:hover { background-color: #f0f0f0; }
        .comanda-items, .pago-items { max-height: 40vh; overflow-y: auto; }
        .comanda-total, .pago-total { font-size: 1.5rem; font-weight: bold; }
    </style>

    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <h1>Gestión de Pedidos</h1>
                <div class="text-right mb-2">
                    <button class="btn btn-success" onclick="fct_NuevoPedido()">
                        <i class="fas fa-plus"></i> Nuevo Pedido
                    </button>
                </div>
            </div>
            <div class="panel-body">
                <table class="table table-bordered table-hover text-center">
                    <thead class="thead-dark">
                        <tr>
                            <th>ID</th>
                            <th>Fecha</th>
                            <th>Monto</th>
                            <th>Estado</th>
                            <th>Mesa</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody id="tbody_Pedido" runat="server"></tbody>
                </table>
            </div>
        </div>
    </section>

    <div class="modal fade" id="modalTomaPedido" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModalPedido"></h5>
                    <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-5">
                            <h4>Menú</h4>
                            <div class="row">
                                <div class="col-4"><ul class="nav nav-pills flex-column menu-categorias" id="menu-categorias-tabs"></ul></div>
                                <div class="col-8"><div class="tab-content" id="menu-productos-content"></div></div>
                            </div>
                        </div>
                        <div class="form-group col-md-4">
                            <label>Mesa</label>
                            <select id="select_mesa" class="form-control"></select>
                        </div>
                    </div>
                    <hr>
                    <h5>Detalles del Pedido</h5>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>Producto</label>
                            <select id="select_producto" class="form-control"></select>
                        </div>
                        <div class="form-group col-md-3">
                            <label>Precio</label>
                            <input type="number" class="form-control" id="txt_precio" step="0.01">
                        </div>
                        <div class="form-group col-md-3">
                            <label>Cantidad</label>
                            <input type="number" class="form-control" id="txt_cantidad">
                        </div>
                    </div>
                    <div class="text-right">
                        <button class="btn btn-secondary" onclick="fct_AgregarDetalle()">
                            <i class="fas fa-plus-circle"></i> Agregar Detalle
                        </button>
                    </div>
                    <table class="table table-sm mt-3">
                        <thead>
                            <tr>
                                <th>Producto</th>
                                <th>Precio</th>
                                <th>Cantidad</th>
                                <th>Subtotal</th>
                                <th>Acción</th>
                            </tr>
                        </thead>
                        <tbody id="tbody_Detalles"></tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" onclick="fct_GuardarPedido()">Guardar</button>
                </div>
            </div>
        </div>
    </div>
<script>
    let detallesPedido = [];

        // Variable para guardar el ID del contenedor de mesas generado por ASP.NET
        const idContenedorMesas = '#<%= cardContainer_MesasPedido.ClientID %>';

        // Función que se ejecuta cuando el documento está listo
        $(document).ready(function () {
            fct_CargarDatosIniciales();
            // Asignación de eventos a los elementos de la página
            $('#menu-categorias-tabs').on('click', '.nav-link', function () {
                const categoria = $(this).data('categoria');
                $('#menu-categorias-tabs .nav-link').removeClass('active');
                $(this).addClass('active');
                fct_MostrarProductosPorCategoria(categoria);
            });
            $('#menu-productos-content').on('click', '.producto-item', function () { fct_AgregarItem($(this).data('id')); });
            $('#tbody_comanda').on('click', '.btn-ajustar-cantidad', function () { fct_AjustarCantidad($(this).data('index'), parseInt($(this).data('cambio'))); });
            $('#btnGuardarPedido').on('click', fct_GuardarPedido);
            $('#btnProcederPago').on('click', fct_AbrirModalPago);
            $('#btnConfirmarPago').on('click', fct_ConfirmarPago);
        });

        // Carga todos los datos necesarios para la página en una sola llamada AJAX
        function fct_CargarDatosIniciales() {
            $(idContenedorMesas).html('<div class="col-12 text-center"><div class="spinner-border text-primary" role="status"><span class="sr-only">Cargando...</span></div></div>');
            $.ajax({
                type: "POST", url: "wfPedidos.aspx/GetDatosIniciales", contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (response) {
                    if (response.d) {
                        // Guardar datos en variables globales (con protección por si vienen nulos)
                        menuCompleto = response.d.Menu || [];
                        clientes = response.d.Clientes || [];
                        cajeros = response.d.Cajeros || [];
                        const mesas = response.d.Mesas || [];

                        categorias = [...new Set(menuCompleto.map(p => p.Categoria))].map(c => ({ nombre: c }));

                        fct_RenderizarMesas(mesas);
                    } else {
                        toastr.error("La respuesta del servidor está vacía o es inválida.");
                    }
                },
                error: function (xhr) {
                    toastr.error("Error crítico al cargar datos. Revisa la consola (F12).");
                    console.error(xhr.responseText); // Muestra el error detallado del servidor en consola
                    $(idContenedorMesas).html(`<div class='col-12 alert alert-danger'>Error al cargar: ${xhr.responseText}</div>`);
                }
            });
        }

        // Dibuja las tarjetas de las mesas en la pantalla
        function fct_RenderizarMesas(mesas) {
            const container = $(idContenedorMesas);
            container.empty();
            if (!mesas || mesas.length === 0) {
                container.html("<div class='col-12'><p class='text-center'>No hay mesas configuradas en el sistema.</p></div>"); return;
            }
            mesas.forEach(mesa => {
                let cardClass = mesa.EsVigente ? (mesa.EstaDisponible ? 'bg-success' : 'bg-danger') : 'bg-secondary';
                let textoEstado = mesa.EsVigente ? (mesa.EstaDisponible ? 'Disponible' : 'Ocupada') : 'Mantenimiento';
                let onClickAction = mesa.EsVigente ? `fct_AbrirPedido(${mesa.Id}, ${mesa.EstaDisponible})` : '';
                container.append(`<div class="col-lg-2 col-md-3 col-sm-4 mb-4">
                    <div class="card card-mesa-pedido text-white text-center ${cardClass}" onclick="${onClickAction}">
                        <div class="card-body p-3"><i class="fas fa-chair icono-mesa"></i>
                            <div class="numero-mesa mt-1">${mesa.Numero}</div><div class="estado-mesa">${textoEstado}</div>
                        </div></div></div>`);
            });
        }

    function fct_RenderizarDetalles() {
        const tbody = $('#tbody_Detalles');
        tbody.empty();
        detallesPedido.forEach((det, idx) => {
            const row = `<tr>
            <td>${det.nombre}</td>
            <td>${det.precio}</td>
            <td>${det.cantidad}</td>
            <td>${(det.precio * det.cantidad).toFixed(2)}</td>
            <td>
                <button class='btn btn-sm btn-info' onclick='fct_EditarDetalle(${idx})'><i class='fas fa-edit'></i></button>
                <button class='btn btn-sm btn-danger' onclick='fct_EliminarDetalle(${idx})'><i class='fas fa-trash'></i></button>
            </td>
        </tr>`;
            tbody.append(row);
        });
    }

        // --- Funciones para manejar el menú y la comanda dentro del modal ---

        function fct_RenderizarCategorias() {
            const container = $('#menu-categorias-tabs'); container.empty();
            categorias.forEach((cat, index) => {
                container.append(`<li class="nav-item"><a class="nav-link ${index === 0 ? 'active' : ''}" data-categoria="${cat.nombre}">${cat.nombre}</a></li>`);
            });
            if (categorias.length > 0) fct_MostrarProductosPorCategoria(categorias[0].nombre);
        }

        function fct_MostrarProductosPorCategoria(categoria) {
            const container = $('#menu-productos-content'); container.empty();
            const productosFiltrados = menuCompleto.filter(p => p.Categoria === categoria);
            let productosHtml = '<div class="list-group">';
            productosFiltrados.forEach(p => {
                productosHtml += `<a class="list-group-item list-group-item-action producto-item" data-id="${p.IdProducto}">${p.Nombre} <span class="float-right font-weight-bold">S/ ${p.Precio.toFixed(2)}</span></a>`;
            });
            container.html(productosHtml + '</div>');
        }

        function fct_AgregarItem(idProducto) {
            if (pedidoActual.IdPedido > 0) { toastr.info("Para añadir items a un pedido ya enviado, se necesita una lógica de 'Actualizar Pedido'."); return; }
            const producto = menuCompleto.find(p => p.IdProducto === idProducto);
            if (!producto) return;
            const itemExistente = pedidoActual.Items.find(item => item.IdProducto === idProducto);
            if (itemExistente) itemExistente.Cantidad++;
            else pedidoActual.Items.push({ IdProducto: producto.IdProducto, Nombre: producto.Nombre, Precio: producto.Precio, Cantidad: 1 });
            fct_RenderizarComanda();
        }

        function fct_AjustarCantidad(index, cambio) {
            if (pedidoActual.IdPedido > 0) { toastr.info("No se puede modificar un pedido ya enviado desde esta interfaz."); return; }
            const item = pedidoActual.Items[index];
            if (item) {
                item.Cantidad += cambio;
                if (item.Cantidad <= 0) pedidoActual.Items.splice(index, 1);
            }
            fct_RenderizarComanda();
        }

        function fct_RenderizarComanda() {
            const tbody = $('#tbody_comanda'); tbody.empty();
            let total = 0;
            if (!pedidoActual.Items || pedidoActual.Items.length === 0) {
                tbody.html('<tr><td colspan="4" class="text-center text-muted">Seleccione productos del menú</td></tr>');
            } else {
                pedidoActual.Items.forEach((item, index) => {
                    const subtotal = item.Precio * item.Cantidad;
                    total += subtotal;
                    tbody.append(`<tr><td>${item.Nombre}</td><td><div class="input-group"><div class="input-group-prepend"><button class="btn btn-sm btn-outline-danger btn-ajustar-cantidad" type="button" data-index="${index}" data-cambio="-1">-</button></div><input type="text" class="form-control-sm text-center" value="${item.Cantidad}" readonly><div class="input-group-append"><button class="btn btn-sm btn-outline-success btn-ajustar-cantidad" type="button" data-index="${index}" data-cambio="1">+</button></div></div></td><td class="text-right">S/ ${subtotal.toFixed(2)}</td><td class="text-center"><button class="btn btn-xs btn-danger btn-ajustar-cantidad" data-index="${index}" data-cambio="${-item.Cantidad}"><i class="fas fa-times"></i></button></td></tr>`);
                });
            }
            $('#comanda_total').text(total.toFixed(2));
        }

        // --- Funciones para guardar y pagar ---

        function fct_GuardarPedido() {
            if (!pedidoActual.Items || pedidoActual.Items.length === 0) { toastr.warning("Debe agregar productos al pedido."); return; }
            const boton = $('#btnGuardarPedido');
            boton.prop('disabled', true).text('Guardando...');
            $.ajax({
                type: "POST", url: "wfPedidos.aspx/RegistrarOModificarPedido", contentType: "application/json; charset=utf-8",
                dataType: "json", data: JSON.stringify({ pedido: pedidoActual }),
                success: function (r) {
                    if (r.d.startsWith("Error")) { toastr.error(r.d); }
                    else {
                        toastr.success("Pedido registrado exitosamente!");
                        $('#modalTomaPedido').modal('hide');
                        fct_CargarDatosIniciales();
                    }
                },
                error: function (xhr) { toastr.error("Error crítico al guardar el pedido."); console.error(xhr.responseText); },
                complete: function () { boton.prop('disabled', false).text('Registrar Pedido'); }
            });
        }

        function fct_AbrirModalPago() {
            $('#modalTomaPedido').modal('hide');
            $('#tituloModalPago').text(`Procesar Pago - Pedido #${pedidoActual.IdPedido}`);
            $('#pago_total').text($('#comanda_total').text());

            const tbody = $('#tbody_pago_resumen'); tbody.empty();
            pedidoActual.Items.forEach(item => {
                tbody.append(`<tr><td>${item.Cantidad}</td><td>${item.Nombre}</td><td class="text-right">S/ ${(item.Cantidad * item.Precio).toFixed(2)}</td></tr>`);
            });

            $('#ddlPagoCliente').empty();
            clientes.forEach(c => $('#ddlPagoCliente').append(`<option value="${c.Id}">${c.Nombre}</option>`));
            $('#ddlPagoCajero').empty();
            cajeros.forEach(c => $('#ddlPagoCajero').append(`<option value="${c.Id}">${c.Nombre}</option>`));

            $('#ddlPagoCliente').val(pedidoActual.IdCliente);
            $('#modalPago').modal('show');
        }

        function fct_ConfirmarPago() {
            const idPedido = pedidoActual.IdPedido;
            const idCajero = $('#ddlPagoCajero').val();
            if (!idCajero) { toastr.warning("Debe seleccionar un cajero."); return; }

            const boton = $('#btnConfirmarPago');
            boton.prop('disabled', true).html('<span class="spinner-border spinner-border-sm"></span> Procesando...');

            $.ajax({
                type: "POST", url: "wfPedidos.aspx/ProcesarPago", contentType: "application/json; charset=utf-8",
                dataType: "json", data: JSON.stringify({ idPedido: idPedido, idCajero: idCajero }),
                success: function (r) {
                    if (r.d.startsWith("Error")) { toastr.error(r.d); }
                    else {
                        Swal.fire('¡Pago Exitoso!', 'La mesa ha sido liberada.', 'success');
                        $('#modalPago').modal('hide');
                        fct_CargarDatosIniciales();
                    }
                },
                error: function (xhr) { toastr.error("Error crítico al procesar el pago."); console.error(xhr.responseText); },
                complete: function () { boton.prop('disabled', false).html('<i class="fas fa-check-circle"></i> Confirmar Pago y Liberar Mesa'); }
            });
        }
    </script>
</asp:Content>