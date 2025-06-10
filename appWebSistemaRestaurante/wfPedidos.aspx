<%@ Page Title="Gestión de Pedidos" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfPedidos.aspx.vb" Inherits="appWebSistemaRestaurante.wfPedidos" %>

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
            <div id="vistaMesas">
                <div class="d-flex justify-content-between align-items-center mb-3">
                    <h1>Salón Principal</h1>
                    <div>
                        <button class="btn btn-sm btn-light" onclick="fct_CargarDatosIniciales()"><i class="fas fa-sync-alt"></i> Actualizar</button>
                    </div>
                </div>
                <hr />
                <div class="row" id="cardContainer_MesasPedido" runat="server">
                    <div class="col-12 text-center"><div class="spinner-border text-primary" role="status"><span class="sr-only">Cargando...</span></div></div>
                </div>
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
                        <div class="col-md-7 bg-light p-3 rounded">
                            <h5 class="mb-3">Comanda</h5>
                            <div class="comanda-items">
                                <table class="table table-sm">
                                    <thead><tr><th>Producto</th><th style="width: 120px;">Cantidad</th><th class="text-right">Subtotal</th><th class="text-center"></th></tr></thead>
                                    <tbody id="tbody_comanda"></tbody>
                                </table>
                            </div><hr />
                            <div class="d-flex justify-content-end comanda-total"><span>TOTAL: S/ </span><span id="comanda_total">0.00</span></div>
                            <div class="mt-3 text-right">
                                <button class="btn btn-warning" id="btnGuardarPedido"><i class="fas fa-save"></i> Enviar a Cocina</button>
                                <button class="btn btn-success" id="btnProcederPago"><i class="fas fa-dollar-sign"></i> Proceder al Pago</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="modal fade" id="modalPago" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
             <div class="modal-content">
                <div class="modal-header bg-success text-white">
                    <h5 class="modal-title" id="tituloModalPago"></h5>
                    <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                </div>
                <div class="modal-body">
                     <div class="row">
                        <div class="col-md-7">
                            <h5>Resumen de Cuenta</h5>
                            <div class="pago-items border rounded bg-light p-2">
                                <table class="table table-sm">
                                    <thead><tr><th>Cant.</th><th>Producto</th><th class="text-right">Subtotal</th></tr></thead>
                                    <tbody id="tbody_pago_resumen"></tbody>
                                </table>
                            </div>
                             <hr/>
                            <div class="d-flex justify-content-end pago-total"><span>TOTAL A PAGAR: S/ </span><span id="pago_total">0.00</span></div>
                        </div>
                        <div class="col-md-5">
                             <h5>Datos de Pago</h5>
                             <div class="form-group">
                                 <label>Cliente</label>
                                 <select class="form-control" id="ddlPagoCliente"></select>
                             </div>
                             <div class="form-group">
                                 <label>Cobrado por</label>
                                 <select class="form-control" id="ddlPagoUsuarioCobro"></select>
                             </div>
                             <div class="form-group">
                                 <label>Método de Pago</label>
                                 <select class="form-control" id="ddlPagoMetodo">
                                     <option>Efectivo</option>
                                     <option>Tarjeta</option>
                                     <option>Yape/Plin</option>
                                 </select>
                             </div>
                             <div class="form-group text-center">
                                <button class="btn btn-lg btn-success" id="btnConfirmarPago"><i class="fas fa-check-circle"></i> Confirmar Pago y Liberar Mesa</button>
                             </div>
                        </div>
                     </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageSpecificScripts" runat="server">
    <script>
        let pedidoActual = {};
        let itemsOriginales = [];
        let menuCompleto = [], categorias = [], clientes = [], usuariosParaCobro = [];
        let usuarioLogueado = {};

        const idContenedorMesas = '#<%= cardContainer_MesasPedido.ClientID %>';

        $(document).ready(function () {
            fct_CargarDatosIniciales();
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

        function fct_CargarDatosIniciales() {
            $(idContenedorMesas).html('<div class="col-12 text-center"><div class="spinner-border text-primary" role="status"></div></div>');
            $.ajax({
                type: "POST", url: "wfPedidos.aspx/GetSessionInfo", contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (sessionResponse) {
                    if (sessionResponse.d) { usuarioLogueado = sessionResponse.d; }
                    else {
                        toastr.warning("No se pudo identificar al usuario. Se usará un mesero por defecto.");
                        usuarioLogueado = { IdUsuario: 1, NombreUsuario: "Mesero Default" };
                    }
                    $.ajax({
                        type: "POST", url: "wfPedidos.aspx/GetDatosIniciales", contentType: "application/json; charset=utf-8", dataType: "json",
                        success: function (response) {
                            if (response.d) {
                                menuCompleto = response.d.Menu || [];
                                clientes = response.d.Clientes || [];
                                usuariosParaCobro = response.d.UsuariosParaCobro || [];
                                const mesas = response.d.Mesas || [];
                                categorias = [...new Set(menuCompleto.map(p => p.Categoria))].map(c => ({ nombre: c }));
                                fct_RenderizarMesas(mesas);
                            } else { toastr.error("La respuesta del servidor está vacía."); }
                        },
                        error: function (xhr) { toastr.error("Error crítico al cargar datos."); console.error(xhr.responseText); }
                    });
                },
                error: function () { toastr.error("Fallo al obtener la sesión."); }
            });
        }

        function fct_RenderizarMesas(mesas) {
            const container = $(idContenedorMesas); container.empty();
            if (!mesas || mesas.length === 0) { container.html("<div class='col-12'><p class='text-center'>No hay mesas configuradas.</p></div>"); return; }
            mesas.forEach(mesa => {
                let cardClass = mesa.EsVigente ? (mesa.EstaDisponible ? 'bg-success' : 'bg-danger') : 'bg-secondary';
                let textoEstado = mesa.EsVigente ? (mesa.EstaDisponible ? 'Disponible' : 'Ocupada') : 'Mantenimiento';
                let onClickAction = mesa.EsVigente ? `fct_AbrirPedido(${mesa.Id}, ${mesa.EstaDisponible})` : '';
                container.append(`<div class="col-lg-2 col-md-3 col-sm-4 mb-4"><div class="card card-mesa-pedido text-white text-center ${cardClass}" onclick="${onClickAction}"><div class="card-body p-3"><i class="fas fa-chair icono-mesa"></i><div class="numero-mesa mt-1">${mesa.Numero}</div><div class="estado-mesa">${textoEstado}</div></div></div></div>`);
            });
        }

        function fct_AbrirPedido(idMesa, estaDisponible) {
            if (estaDisponible) {
                pedidoActual = { IdPedido: 0, IdMesa: idMesa, IdCliente: 1, IdMesero: usuarioLogueado.IdUsuario, Items: [] };
                itemsOriginales = [];
                $('#tituloModalPedido').text(`Nuevo Pedido - Mesa #${idMesa} (Atendido por: ${usuarioLogueado.NombreUsuario})`);
                $('#btnProcederPago').hide();
                $('#btnGuardarPedido').text('Registrar Pedido').show();
                fct_RenderizarCategorias(); fct_RenderizarComanda();
                $('#modalTomaPedido').modal('show');
            } else {
                $.ajax({
                    type: "POST", url: "wfPedidos.aspx/GetPedidoPorMesa", contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ idMesa: idMesa }), dataType: "json",
                    success: function (r) {
                        if (r.d) {
                            pedidoActual = r.d;
                            itemsOriginales = JSON.parse(JSON.stringify(r.d.Items));
                            $('#tituloModalPedido').text(`Viendo Pedido #${pedidoActual.IdPedido} - Mesa #${idMesa}`);
                            $('#btnProcederPago').show();
                            $('#btnGuardarPedido').text('Añadir y Enviar').show();
                            fct_RenderizarCategorias(); fct_RenderizarComanda();
                            $('#modalTomaPedido').modal('show');
                        } else { toastr.error("No se pudo cargar el pedido de esta mesa."); }
                    },
                    error: function (xhr) { toastr.error("Error al obtener el pedido."); console.error(xhr.responseText); }
                });
            }
        }

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
            const producto = menuCompleto.find(p => p.IdProducto === idProducto);
            if (!producto) return;
            const itemExistente = pedidoActual.Items.find(item => item.IdProducto === idProducto);
            if (itemExistente) { itemExistente.Cantidad++; }
            else { pedidoActual.Items.push({ IdProducto: producto.IdProducto, Nombre: producto.Nombre, Precio: producto.Precio, Cantidad: 1 }); }
            fct_RenderizarComanda();
        }

        function fct_AjustarCantidad(index, cambio) {
            const item = pedidoActual.Items[index];
            if (item) {
                item.Cantidad += cambio;
                if (item.Cantidad <= 0) { pedidoActual.Items.splice(index, 1); }
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

        function fct_GuardarPedido() {
            let datosParaEnviar = {};
            let esNuevo = (pedidoActual.IdPedido === 0);

            if (esNuevo) {
                if (pedidoActual.Items.length === 0) { toastr.warning("Debe agregar productos al pedido."); return; }
                datosParaEnviar = pedidoActual;
            } else {
                const nuevosItems = [];
                pedidoActual.Items.forEach(itemActual => {
                    const itemOriginal = itemsOriginales.find(i => i.IdProducto === itemActual.IdProducto);
                    if (!itemOriginal) { nuevosItems.push(itemActual); }
                    else if (itemActual.Cantidad > itemOriginal.Cantidad) { nuevosItems.push({ ...itemActual, Cantidad: itemActual.Cantidad - itemOriginal.Cantidad }); }
                });
                if (nuevosItems.length === 0) { toastr.info("No hay nuevos productos para añadir."); return; }
                datosParaEnviar = { IdPedido: pedidoActual.IdPedido, Items: nuevosItems };
            }

            const boton = $('#btnGuardarPedido');
            boton.prop('disabled', true).text('Enviando...');
            $.ajax({
                type: "POST", url: "wfPedidos.aspx/RegistrarOModificarPedido", contentType: "application/json; charset=utf-8",
                dataType: "json", data: JSON.stringify({ pedido: datosParaEnviar }),
                success: function (r) {
                    if (r.d.startsWith("Error")) { toastr.error(r.d); }
                    else {
                        toastr.success("Operación realizada con éxito!");
                        $('#modalTomaPedido').modal('hide');
                        fct_CargarDatosIniciales();
                    }
                },
                error: function (xhr) { toastr.error("Error crítico al guardar."); console.error(xhr.responseText); },
                complete: function () { boton.prop('disabled', false).text(esNuevo ? 'Registrar Pedido' : 'Añadir y Enviar'); }
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

            const ddlCobro = $('#ddlPagoUsuarioCobro');
            ddlCobro.empty();
            usuariosParaCobro.forEach(u => ddlCobro.append(`<option value="${u.Id}">${u.Nombre}</option>`));
            ddlCobro.val(usuarioLogueado.IdUsuario);

            $('#ddlPagoCliente').empty();
            clientes.forEach(c => $('#ddlPagoCliente').append(`<option value="${c.Id}">${c.Nombre}</option>`));
            $('#ddlPagoCliente').val(pedidoActual.IdCliente);
            $('#modalPago').modal('show');
        }

        function fct_ConfirmarPago() {
            const idPedido = pedidoActual.IdPedido;
            const idCajero = $('#ddlPagoUsuarioCobro').val();
            const idCliente = $('#ddlPagoCliente').val();

            if (!idCajero) { toastr.warning("Debe seleccionar un usuario para el cobro."); return; }

            const boton = $('#btnConfirmarPago');
            boton.prop('disabled', true).html('<span class="spinner-border spinner-border-sm"></span> Procesando...');

            $.ajax({
                type: "POST", url: "wfPedidos.aspx/ProcesarPago", contentType: "application/json; charset=utf-8",
                dataType: "json", data: JSON.stringify({ idPedido: idPedido, idCajero: idCajero, idCliente: idCliente }),
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