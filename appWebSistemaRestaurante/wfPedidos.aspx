<%@ Page Title="Gestión de Pedidos" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfPedidos.aspx.vb" Inherits="appWebSistemaRestaurante.wfPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <style>
        .card-mesa-pedido { cursor: pointer; transition: all 0.2s ease; border-width: 2px; }
        .card-mesa-pedido:hover { transform: scale(1.05); box-shadow: 0 5px 20px rgba(0,0,0,0.2); }
        .card-mesa-pedido .icono-mesa { font-size: 2.5rem; }
        .card-mesa-pedido .numero-mesa { font-size: 1.5rem; font-weight: 700; }
        #modalTomaPedido .modal-dialog, #modalPago .modal-dialog { max-width: 95%; }
        .menu-categorias { list-style: none; padding: 0; max-height: 45vh; overflow-y: auto; }
        .menu-categorias .nav-link { cursor: pointer; text-align: center; border: 1px solid #dee2e6; margin-bottom: 5px; }
        .menu-categorias .nav-link.active { background-color: var(--primary); color: white; }
        #menu-productos-content { max-height: 45vh; overflow-y: auto; }
        .producto-item { cursor: pointer; transition: background-color 0.2s ease; }
        .producto-item:hover { background-color: #f0f0f0; }
        .comanda-items, .pago-items { max-height: 40vh; overflow-y: auto; }
        .comanda-total, .pago-total { font-size: 1.5rem; font-weight: bold; }
    </style>

    <section class="content">
        <div class="container-fluid pt-3">
            <ul class="nav nav-pills mb-3" id="pedidosTab" role="tablist">
                <li class="nav-item" role="presentation"><a class="nav-link active" id="salon-tab" data-toggle="tab" href="#salon" role="tab"><i class="fas fa-th-large mr-1"></i> Salón Principal</a></li>
                <li class="nav-item" role="presentation"><a class="nav-link" id="historial-tab" data-toggle="tab" href="#historial" role="tab"><i class="fas fa-history mr-1"></i> Historial de Pedidos</a></li>
            </ul>
            <div class="tab-content" id="pedidosTabContent">
                <div class="tab-pane fade show active" id="salon" role="tabpanel">
                    <div class="d-flex justify-content-between align-items-center py-3">
                        <h4 class="mb-0">Mesas disponibles</h4>
                        <div><button type="button" class="btn btn-sm btn-light border" onclick="fct_CargarDatosIniciales()"><i class="fas fa-sync-alt"></i></button></div>
                    </div>
                    <div class="row" id="cardContainer_MesasPedido" runat="server">
                        <div class="col-12 text-center p-5"><div class="spinner-border text-primary" role="status"></div></div>
                    </div>
                </div>
                <div class="tab-pane fade" id="historial" role="tabpanel">
                    <div class="py-3">
                        <table id="tbl_HistorialPedidos" class="table table-bordered table-striped" style="width:100%;">
                            <thead class="bg-dark text-white"><tr><th>ID</th><th>Fecha</th><th>Mesa</th><th>Cliente</th><th>Mesero</th><th>Monto</th><th>Estado</th></tr></thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <div class="modal fade" id="modalTomaPedido" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-xl" role="document"><div class="modal-content">
            <div class="modal-header bg-primary text-white">
                <h5 class="modal-title" id="tituloModalPedido"></h5>
                <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
            </div>
            <div class="modal-body"><div class="row">
                <div class="col-md-5"><h4>Menú</h4><div class="row">
                    <div class="col-4"><ul class="nav nav-pills flex-column menu-categorias" id="menu-categorias-tabs"></ul></div>
                    <div class="col-8"><div class="list-group" id="menu-productos-content"></div></div>
                </div></div>
                <div class="col-md-7 bg-light p-3 rounded">
                    <div class="form-group"><label for="ddlPedidoMesero" class="font-weight-bold">Atendido por:</label><select id="ddlPedidoMesero" class="form-control"></select></div>
                    <hr class="mt-2 mb-2"/><h5 class="mb-3">Comanda</h5>
                    <div class="comanda-items"><table class="table table-sm">
                        <thead><tr><th>Producto</th><th style="width: 150px;">Cantidad</th><th class="text-right">Subtotal</th><th class="text-center"></th></tr></thead>
                        <tbody id="tbody_comanda"></tbody>
                    </table></div><hr />
                    <div class="d-flex justify-content-end comanda-total"><span>TOTAL: S/ </span><span id="comanda_total">0.00</span></div>
                    <div class="mt-3 text-right">
                        <button type="button" class="btn btn-warning" id="btnGuardarPedido"><i class="fas fa-save"></i> Enviar a Cocina</button>
                        <button type="button" class="btn btn-success" id="btnProcederPago"><i class="fas fa-dollar-sign"></i> Proceder al Pago</button>
                    </div>
                </div>
            </div></div>
        </div></div>
    </div>
    
    <div class="modal fade" id="modalPago" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document"><div class="modal-content">
            <div class="modal-header bg-success text-white">
                <h5 class="modal-title" id="tituloModalPago"></h5>
                <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
            </div>
            <div class="modal-body"><div class="row">
                <div class="col-md-7">
                    <h5>Resumen de Cuenta</h5>
                    <div class="pago-items border rounded bg-light p-2"><table class="table table-sm">
                        <thead><tr><th>Cant.</th><th>Producto</th><th class="text-right">Subtotal</th></tr></thead>
                        <tbody id="tbody_pago_resumen"></tbody>
                    </table></div><hr/>
                    <div class="d-flex justify-content-end pago-total"><span>TOTAL A PAGAR: S/ </span><span id="pago_total">0.00</span></div>
                </div>
                <div class="col-md-5">
                     <h5>Datos de Pago</h5>
                     <div class="form-group"><label>Cliente</label><select class="form-control" id="ddlPagoCliente"></select></div>
                     <div class="form-group"><label>Cobrado por</label><select class="form-control" id="ddlPagoUsuarioCobro"></select></div>
                     <div class="form-group"><label>Método de Pago</label><select class="form-control" id="ddlPagoMetodo"><option>Efectivo</option><option>Tarjeta</option><option>Yape/Plin</option></select></div>
                     <div class="form-group text-center">
                        <button type="button" class="btn btn-lg btn-success" id="btnConfirmarPago"><i class="fas fa-check-circle"></i> Confirmar Pago y Liberar Mesa</button>
                     </div>
                </div>
            </div></div>
        </div></div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageSpecificScripts" runat="server">
    <script>
        let pedidoActual = {};
        let itemsOriginales = [];
        let menuCompleto = [], categorias = [], clientes = [], usuariosParaCobro = [], meseros = [];
        const idContenedorMesas = '#<%= cardContainer_MesasPedido.ClientID %>';
        let tablaHistorial;

        $(document).ready(function () {
            fct_CargarDatosIniciales();
            $('#menu-categorias-tabs').on('click', '.nav-link', function (e) {
                e.preventDefault();
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
            $(idContenedorMesas).html('<div class="col-12 text-center p-5"><div class="spinner-border text-primary" role="status"></div></div>');
            $.ajax({
                type: "POST", url: "wfPedidos.aspx/GetPaginaPedidos", contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (response) {
                    if (response.d) {
                        menuCompleto = response.d.Menu || [];
                        clientes = response.d.Clientes || [];
                        usuariosParaCobro = response.d.UsuariosParaCobro || [];
                        meseros = response.d.Meseros || [];
                        const mesas = response.d.Mesas || [];
                        categorias = [...new Set(menuCompleto.map(p => p.Categoria))].filter(Boolean).map(c => ({ nombre: c }));
                        fct_RenderizarMesas(mesas);
                        fct_RenderizarHistorial(response.d.Historial || []);
                    } else { toastr.error("La respuesta del servidor está vacía."); }
                },
                error: function (xhr) {
                    toastr.error("Error crítico al cargar datos.");
                    $(idContenedorMesas).html(`<div class='col-12 alert alert-danger'>${xhr.responseText}</div>`);
                    console.error(xhr.responseText);
                }
            });
        }

        function fct_RenderizarMesas(mesas) {
            const container = $(idContenedorMesas); container.empty();
            if (!mesas || mesas.length === 0) { container.html("<div class='col-12'><p class='text-center'>No hay mesas configuradas.</p></div>"); return; }
            mesas.forEach(mesa => {
                let cardClass = mesa.EsVigente ? (mesa.EstaDisponible ? 'bg-success' : 'bg-danger') : 'bg-secondary';
                let textoEstado = mesa.EsVigente ? (mesa.EstaDisponible ? 'Disponible' : 'Ocupada') : 'Mantenimiento';
                let onClickAction = mesa.EsVigente ? `fct_AbrirPedido(${mesa.Id}, ${mesa.EstaDisponible})` : '';
                container.append(`<div class="col-xl-2 col-lg-3 col-md-4 col-sm-6 mb-4"><div class="card card-mesa-pedido text-white text-center ${cardClass}" onclick="${onClickAction}"><div class="card-body p-3"><i class="fas fa-chair icono-mesa"></i><div class="numero-mesa mt-1">${mesa.Numero}</div><div class="estado-mesa">${textoEstado}</div></div></div></div>`);
            });
        }

        function fct_RenderizarHistorial(dataHistorial) {
            if ($.fn.DataTable.isDataTable('#tbl_HistorialPedidos')) {
                tablaHistorial.clear().rows.add(dataHistorial).draw();
            } else {
                tablaHistorial = $('#tbl_HistorialPedidos').DataTable({
                    data: dataHistorial,
                    columns: [
                        { data: 'IdPedido' }, { data: 'Fecha' }, { data: 'Mesa' }, { data: 'Cliente' },
                        { data: 'Mesero' }, { data: 'Monto', render: $.fn.dataTable.render.number(',', '.', 2, 'S/ ') },
                        {
                            data: 'Estado', render: function (data) {
                                let badge = data === 'Pagado' ? 'badge-success' : 'badge-warning';
                                return `<span class="badge ${badge}">${data}</span>`;
                            }
                        }
                    ],
                    responsive: true, order: [[0, 'desc']],
                    language: { url: '//cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json' }
                });
            }
        }

        function fct_AbrirPedido(idMesa, estaDisponible) {
            const ddl = $('#ddlPedidoMesero'); ddl.empty().append('<option value="">-- Seleccione Mesero --</option>');
            meseros.forEach(m => ddl.append(`<option value="${m.Id}">${m.Nombre}</option>`));

            if (estaDisponible) {
                pedidoActual = { IdPedido: 0, IdMesa: idMesa, IdCliente: 1, IdMesero: 0, Items: [] };
                itemsOriginales = [];
                $('#tituloModalPedido').text(`Nuevo Pedido - Mesa #${idMesa}`);
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
                            $('#ddlPedidoMesero').val(pedidoActual.IdMesero);
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

            pedidoActual.IdMesero = parseInt($('#ddlPedidoMesero').val());
            if (!pedidoActual.IdMesero) { toastr.warning("Debe seleccionar un mesero."); return; }

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
                if (nuevosItems.length === 0) { toastr.info("No hay nuevos productos o cambios de cantidad para añadir."); return; }
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
            ddlCobro.empty().append('<option value="">-- Seleccione Usuario --</option>');
            usuariosParaCobro.forEach(u => ddlCobro.append(`<option value="${u.Id}">${u.Nombre}</option>`));

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