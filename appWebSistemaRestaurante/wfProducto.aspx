<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfProducto.aspx.vb" Inherits="appWebSistemaRestaurante.wfProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-12">
                        <h1>Gestionar Productos</h1>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="row mb-2">
                    <div class="col-md-12 text-right">
                        <button type="button" class="btn btn-success" onclick="fct_AbrirModalNuevoProducto()">
                            <i class="fas fa-plus-circle"></i> Nuevo Producto
                        </button>
                    </div>
                </div>

                <table class="table table-bordered table-hover text-center" id="tbl_Producto">
                    <thead class="bg-dark text-white">
                        <tr>
                            <th>ID</th>
                            <th>Nombre</th>
                            <th>Precio</th>
                            <th>Tipo</th>
                            <th>Carta</th>
                            <th>Estado</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody id="tbody_Producto" runat="server">
                        </tbody>
                </table>
            </div>
        </div>
    </section>

    <div class="modal fade" id="modal_Producto" tabindex="-1" aria-labelledby="tituloModal" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModal">Registrar / Editar Producto</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span>×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <input type="hidden" id="txtIdProducto">

                        <div class="col-md-8 mb-3">
                            <label for="txtNombre" class="form-label">Nombre del Producto</label>
                            <input type="text" class="form-control" id="txtNombre" placeholder="Ej: Lomo Saltado">
                        </div>
                        
                        <div class="col-md-4 mb-3">
                            <label for="txtPrecio" class="form-label">Precio (S/.)</label>
                            <input type="number" step="0.10" min="0" class="form-control" id="txtPrecio" placeholder="Ej: 25.50">
                        </div>

                        <div class="col-md-6 mb-3">
                            <label for="ddlTipoProducto" class="form-label">Tipo de Producto</label>
                            <asp:DropDownList ID="ddlTipoProducto" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label for="ddlCarta" class="form-label">Carta</label>
                            <asp:DropDownList ID="ddlCarta" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        
                        <div class="col-md-12 mb-3">
                            <label for="txtDescripcion" class="form-label">Descripción</label>
                            <textarea class="form-control" id="txtDescripcion" rows="2" placeholder="Ingrese una descripción (opcional)"></textarea>
                        </div>

                        <div class="col-md-12 form-check form-switch">
                            <input class="form-check-input" type="checkbox" id="chkVigencia" checked>
                            <label class="form-check-label" for="chkVigencia">Vigente / Disponible</label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" id="btnGuardar" onclick="fct_GuardarProducto()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

<script>
    function fct_AbrirModalNuevoProducto() {
        $('#tituloModal').text('Nuevo Producto');
        $('#txtIdProducto').val('0');
        $('#txtNombre').val('');
        $('#txtDescripcion').val('');
        $('#txtPrecio').val('');
        $('#<%= ddlTipoProducto.ClientID %>').prop('selectedIndex', 0);
        $('#<%= ddlCarta.ClientID %>').prop('selectedIndex', 0);
        $('#chkVigencia').prop('checked', true);
        $('#btnGuardar').text('Guardar');
        $('#modal_Producto').modal('show');
    }

    function fct_EditarProducto(id, nombre, descripcion, precio, idTipo, idCarta, estado) {
        $('#tituloModal').text('Editar Producto');

        $('#txtIdProducto').val(id);
        $('#txtNombre').val(nombre);
        $('#txtDescripcion').val(descripcion);
        $('#txtPrecio').val(precio);
        $('#<%= ddlTipoProducto.ClientID %>').val(idTipo);
        $('#<%= ddlCarta.ClientID %>').val(idCarta);

        const estadoBool = (estado.toLowerCase() === 'activo');
        $('#chkVigencia').prop('checked', estadoBool);
        
        $('#btnGuardar').text('Actualizar');
        $('#modal_Producto').modal('show');
    }

    function fct_GuardarProducto() {
        let id = $('#txtIdProducto').val() || "0";
        let nombre = $('#txtNombre').val().trim();
        let descripcion = $('#txtDescripcion').val().trim();
        let precio = $('#txtPrecio').val();
        let idTipo = $('#<%= ddlTipoProducto.ClientID %>').val();
        let idCarta = $('#<%= ddlCarta.ClientID %>').val();
        let vigente = $('#chkVigencia').is(':checked');

        if (nombre === '' || precio === '' || parseFloat(precio) <= 0) {
            toastr.warning('Nombre y un precio válido son obligatorios.');
            return;
        }

        if (!idTipo || !idCarta) {
            toastr.warning('Debe seleccionar un Tipo y una Carta.');
            return;
        }

        $.ajax({
            url: 'wfProducto.aspx/GuardarProducto',
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                id: id,
                nombre: nombre,
                descripcion: descripcion,
                precio: parseFloat(precio),
                idTipo: parseInt(idTipo),
                idCarta: parseInt(idCarta),
                vigente: vigente
            }),
            success: function (response) {
                if (response.d === 'success') {
                    toastr.success('Guardado correctamente');
                    setTimeout(() => location.reload(), 800);
                } else {
                    toastr.error(response.d);
                }
            },
            error: function () {
                toastr.error('Error en la solicitud al servidor.');
            }
        });
    }

    function fct_EliminarProducto(id) {
        Swal.fire({
            title: "¿Deseas eliminar este producto?",
            text: "Esta acción no se puede revertir.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6",
            confirmButtonText: "Sí, eliminar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfProducto.aspx/EliminarProducto',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.success('Producto eliminado');
                            setTimeout(() => location.reload(), 800);
                        } else {
                            toastr.error(response.d);
                        }
                    },
                    error: function () { toastr.error('Error en el servidor'); }
                });
            }
        });
    }

    function fct_DarBajaProducto(id) {
        Swal.fire({
            title: "¿Marcar como No Disponible?",
            text: "El producto se mostrará como inactivo.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Sí, marcar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfProducto.aspx/DarBajaProducto',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.info('Producto marcado como No Disponible');
                            setTimeout(() => location.reload(), 800);
                        } else {
                            toastr.error(response.d);
                        }
                    },
                    error: function () { toastr.error('Error en el servidor'); }
                });
            }
        });
    }

    function fct_DarAltaProducto(id) {
        Swal.fire({
            title: "¿Marcar como Disponible?",
            text: "El producto se mostrará como activo.",
            icon: "info",
            showCancelButton: true,
            confirmButtonColor: "#28a745",
            confirmButtonText: "Sí, marcar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfProducto.aspx/DarAltaProducto',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.success('Producto marcado como Disponible');
                            setTimeout(() => location.reload(), 800);
                        } else {
                            toastr.error(response.d);
                        }
                    },
                    error: function () { toastr.error('Error en el servidor'); }
                });
            }
        });
    }
</script>
</asp:Content>