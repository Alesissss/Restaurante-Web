<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfTipoProducto.aspx.vb" Inherits="appWebSistemaRestaurante.wfTipoProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-12">
                        <h1>Gestionar Tipos de Producto</h1>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="row mb-2">
                    <div class="col-md-12 text-right">
                        <button type="button" class="btn btn-success" onclick="fct_AbrirModalNuevoTipoProducto()">
                            <i class="fas fa-plus-circle"></i> Nuevo Tipo de Producto
                        </button>
                    </div>
                </div>

                <table class="table table-bordered table-hover text-center" id="tbl_TipoProducto">
                    <thead class="bg-dark text-white">
                        <tr>
                            <th>ID</th>
                            <th>Nombre</th>
                            <th>Descripción</th>
                            <th>Vigencia</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody id="tbody_TipoProducto" runat="server">
                        </tbody>
                </table>
            </div>
        </div>
    </section>

    <div class="modal fade" id="modal_TipoProducto" tabindex="-1" aria-labelledby="tituloModal" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModal">Registrar / Editar Tipo de Producto</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span>×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <input type="hidden" id="txtIdTipoProducto">

                        <div class="col-md-12 mb-3">
                            <label for="txtNombre" class="form-label">Nombre</label>
                            <input type="text" class="form-control" id="txtNombre" placeholder="Ej: Entradas, Bebidas, Postres">
                        </div>

                        <div class="col-md-12 mb-3">
                            <label for="txtDescripcion" class="form-label">Descripción</label>
                            <textarea class="form-control" id="txtDescripcion" rows="3" placeholder="Ingrese una descripción opcional"></textarea>
                        </div>

                        <div class="col-md-12 form-check form-switch">
                            <input class="form-check-input" type="checkbox" id="chkVigencia" checked>
                            <label class="form-check-label" for="chkVigencia">Vigente</label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" id="btnGuardar" onclick="fct_GuardarTipoProducto()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

<script>
    function fct_AbrirModalNuevoTipoProducto() {
        $('#tituloModal').text('Nuevo Tipo de Producto');
        $('#txtIdTipoProducto').val('0');
        $('#txtNombre').val('');
        $('#txtDescripcion').val('');
        $('#chkVigencia').prop('checked', true);
        $('#btnGuardar').text('Guardar');
        $('#modal_TipoProducto').modal('show');
    }

    function fct_EditarTipoProducto(id, nombre, descripcion, estado) {
        $('#tituloModal').text('Editar Tipo de Producto');

        $('#txtIdTipoProducto').val(id);
        $('#txtNombre').val(nombre);
        $('#txtDescripcion').val(descripcion);

        const estadoBool = (estado.toLowerCase() === 'activo');
        $('#chkVigencia').prop('checked', estadoBool);

        $('#btnGuardar').text('Actualizar');
        $('#modal_TipoProducto').modal('show');
    }

    function fct_GuardarTipoProducto() {
        let id = $('#txtIdTipoProducto').val() || "0";
        let nombre = $('#txtNombre').val().trim();
        let descripcion = $('#txtDescripcion').val().trim();
        let vigente = $('#chkVigencia').is(':checked');

        if (nombre === '') {
            toastr.warning('El nombre del tipo de producto es obligatorio.');
            return;
        }

        $.ajax({
            url: 'wfTipoProducto.aspx/GuardarTipoProducto',
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ id: id, nombre: nombre, descripcion: descripcion, vigente: vigente }),
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

    function fct_EliminarTipoProducto(id) {
        Swal.fire({
            title: "¿Deseas eliminar este tipo?",
            text: "No podrás eliminarlo si está siendo usado por algún producto.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6",
            confirmButtonText: "Sí, eliminar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfTipoProducto.aspx/EliminarTipoProducto',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.success('Eliminado correctamente');
                            setTimeout(() => location.reload(), 800);
                        } else {
                            toastr.error(response.d);
                        }
                    },
                    error: function () {
                        toastr.error('Error en el servidor');
                    }
                });
            }
        });
    }

    function fct_DarBajaTipoProducto(id) {
        Swal.fire({
            title: "¿Dar de baja a este tipo?",
            text: "Se marcará como no vigente.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Sí, dar de baja"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfTipoProducto.aspx/DarBajaTipoProducto',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.info('Tipo de producto dado de baja');
                            setTimeout(() => location.reload(), 800);
                        } else {
                            toastr.error(response.d);
                        }
                    },
                    error: function () {
                        toastr.error('Error en el servidor');
                    }
                });
            }
        });
    }

    function fct_DarAltaTipoProducto(id) {
        Swal.fire({
            title: "¿Reactivar este tipo?",
            text: "Se marcará como vigente.",
            icon: "info",
            showCancelButton: true,
            confirmButtonColor: "#28a745",
            confirmButtonText: "Sí, reactivar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfTipoProducto.aspx/DarAltaTipoProducto',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.success('Tipo de producto reactivado');
                            setTimeout(() => location.reload(), 800);
                        } else {
                            toastr.error(response.d);
                        }
                    },
                    error: function () {
                        toastr.error('Error en el servidor');
                    }
                });
            }
        });
    }
</script>
</asp:Content>