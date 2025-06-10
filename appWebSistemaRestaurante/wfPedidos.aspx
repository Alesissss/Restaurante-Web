<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfPedidos.aspx.vb" Inherits="appWebSistemaRestaurante.wfPedidos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <h1>Gestión de Pedidos</h1>
                <div class="text-right mb-2">
                    <button type="button" class="btn btn-success" onclick="fct_NuevoPedido()">
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

    <!-- Modal Pedido -->
    <div class="modal fade" id="modalPedido" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Registrar Pedido</h5>
                    <button type="button" class="close" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <input type="hidden" id="txt_idPedido" />
                    <div class="form-row">
                        <div class="form-group col-md-4">
                            <label>Cliente</label>
                            <select id="select_cliente" class="form-control"></select>
                        </div>
                        <div class="form-group col-md-4">
                            <label>Mesero</label>
                            <select id="select_mesero" class="form-control"></select>
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
                        <button type="button" class="btn btn-secondary" onclick="fct_AgregarDetalle()">
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

    function fct_NuevoPedido() {
        $('#detalle_pedido tbody').html('');
        $('#txt_idPedido').val('');
        $('#select_cliente').val('');
        $('#select_mesero').val('');
        $('#select_mesa').val('');
        detallesPedido = [];
        fct_RenderizarDetalles();
        $('.modal-title').text('Registrar Pedido');
        $('#modalPedido').modal('show');
    }

    function fct_EditarPedido(id, cliente, mesero, mesa, detalles) {
        $('#txt_idPedido').val(id);
        $('#select_cliente').val(cliente);
        $('#select_mesero').val(mesero);
        $('#select_mesa').val(mesa);
        detallesPedido = detalles;
        fct_RenderizarDetalles();
        $('.modal-title').text('Editar Pedido');
        $('#modalPedido').modal('show');
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
                <button type='button' class='btn btn-sm btn-info' onclick='fct_EditarDetalle(${idx})'><i class='fas fa-edit'></i></button>
                <button type='button' class='btn btn-sm btn-danger' onclick='fct_EliminarDetalle(${idx})'><i class='fas fa-trash'></i></button>
            </td>
        </tr>`;
            tbody.append(row);
        });
    }

    function fct_AgregarDetalle() {
        const producto = $('#select_producto').val();
        const nombre = $('#select_producto option:selected').text();
        const precio = parseFloat($('#txt_precio').val());
        const cantidad = parseInt($('#txt_cantidad').val());
        const carta = $('#select_producto option:selected').data('carta');

        if (!producto || isNaN(precio) || isNaN(cantidad)) {
            toastr.warning("Complete todos los campos del detalle");
            return;
        }

        // Verificar si ya existe el producto
        const indexExistente = detallesPedido.findIndex(det => det.idProducto === parseInt(producto));
        if (indexExistente !== -1) {
            // Ya existe, sumar cantidad
            detallesPedido[indexExistente].cantidad += cantidad;
        } else {
            // No existe, agregar nuevo
            detallesPedido.push({
                idProducto: parseInt(producto),
                nombre,
                carta,
                precio,
                cantidad
            });
        }

        fct_RenderizarDetalles();
        $('#select_producto').val('');
        $('#txt_precio').val('');
        $('#txt_cantidad').val('');
    }


    function fct_EditarDetalle(idx) {
        const det = detallesPedido[idx];
        $('#select_producto').val(det.idProducto);
        $('#txt_precio').val(det.precio).prop('disabled', true);
        $('#txt_cantidad').val(det.cantidad);
        fct_EliminarDetalle(idx);
    }

    function fct_EliminarDetalle(idx) {
        detallesPedido.splice(idx, 1);
        fct_RenderizarDetalles();
    }

    function fct_GuardarPedido() {
        const id = $('#txt_idPedido').val() || 0;
        const cliente = $('#select_cliente').val();
        const mesero = $('#select_mesero').val();
        const mesa = $('#select_mesa').val();

        if (!cliente || !mesero || !mesa || detallesPedido.length === 0) {
            toastr.warning("Complete todos los campos requeridos y agregue al menos un detalle");
            return;
        }

        const metodo = id == 0 ? 'registrarPedidoWeb' : 'modificarPedidoWeb';
        const data = {
            idPedido: parseInt(id),
            idCliente: parseInt(cliente),
            idMesero: parseInt(mesero),
            idMesa: parseInt(mesa),
            detalles: detallesPedido
        };

        $.ajax({
            type: 'POST',
            url: `wfPedidos.aspx/${metodo}`,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function () {
                toastr.success("Pedido guardado exitosamente");
                $('#modalPedido').modal('hide');
                __doPostBack();
            },
            error: function () {
                toastr.error("Error al guardar el pedido");
            }
        });
    }

    function fct_LlenarCombosPedido() {
        $.ajax({
            type: "POST",
            url: "wfPedidos.aspx/listarClientes",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#select_cliente').empty().append(`<option value="" selected disabled>--Seleccione--</option>`);
                response.d.forEach(function (item) {
                    $('#select_cliente').append(`<option value="${item.id}">${item.nombre}</option>`);
                });
            },
            error: function () { toastr.error("Error al cargar clientes"); }
        });

        $.ajax({
            type: "POST",
            url: "wfPedidos.aspx/listarMeseros",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#select_mesero').empty().append(`<option value="" selected disabled>--Seleccione--</option>`);
                response.d.forEach(function (item) {
                    $('#select_mesero').append(`<option value="${item.id}">${item.nombre}</option>`);
                });
            },
            error: function () { toastr.error("Error al cargar meseros"); }
        });

        $.ajax({
            type: "POST",
            url: "wfPedidos.aspx/listarMesasDisponibles",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#select_mesa').empty().append(`<option value="" selected disabled>--Seleccione--</option>`);
                response.d.forEach(function (item) {
                    $('#select_mesa').append(`<option value="${item.id}">${item.nombre}</option>`);
                });
            },
            error: function () { toastr.error("Error al cargar mesas"); }
        });

        $.ajax({
            type: "POST",
            url: "wfPedidos.aspx/listarProductosVigentes",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#select_producto').empty().append(`<option value="">--Seleccione--</option>`);
                response.d.forEach(function (item) {
                    $('#select_producto').append(`<option value="${item.id}" data-precio="${item.precio}" data-carta="${item.carta}">${item.nombre}</option>`);
                });
            },
            error: function () { toastr.error("Error al cargar productos"); }
        });
    }
    $(document).ready(function () {
        fct_LlenarCombosPedido();
        $(document).on('change', '#select_producto', function () {
            const selected = $('#select_producto option:selected');
            const precio = selected.data('precio');
            if (precio !== undefined) {
                $('#txt_precio').val(precio).prop('disabled', true);
            } else {
                toastr.warning("No se pudo obtener el precio del producto");
            }
        });
    });

</script>
</asp:Content>