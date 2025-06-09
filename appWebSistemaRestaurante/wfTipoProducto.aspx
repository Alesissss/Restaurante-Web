<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfTipoProducto.aspx.vb" Inherits="appWebSistemaRestaurante.wfTipoProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <section class="content">
        <div class="container-fluid p-4">
            <h1>Gestionar tipos de producto</h1>

            <div class="text-right mb-2">
                <button type="button" class="btn btn-success" onclick="fct_AbrirModalNuevo()">
                    <i class="fas fa-plus-circle"></i> Nuevo Tipo de Producto
                </button>
            </div>

            <table class="table table-bordered table-hover text-center">
                <thead class="thead-dark text-white">
                    <tr>
                        <th>ID</th>
                        <th>Nombre</th>
                        <th>Descripción</th>
                        <th>Vigencia</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody id="tbody_TipoProducto" runat="server">
                    <!-- Llenado desde backend -->
                </tbody>
            </table>
        </div>
    </section>

    <!-- Modal -->
    <div class="modal fade" id="modal_TipoProducto" tabindex="-1" aria-labelledby="tituloModal" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModal">Registrar / Editar Tipo de Producto</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span>×</span></button>
                </div>
                <div class="modal-body">
                    <input type="hidden" id="txtIdTipoProducto">
                    <div class="form-group">
                        <label for="txtNombre">Nombre</label>
                        <input type="text" class="form-control" id="txtNombre" placeholder="Ingrese nombre">
                    </div>
                    <div class="form-group">
                        <label for="txtDescripcion">Descripción</label>
                        <textarea class="form-control" id="txtDescripcion" rows="3" placeholder="Ingrese descripción"></textarea>
                    </div>
                    <div class="form-check form-switch">
                        <input class="form-check-input" type="checkbox" id="chkVigencia" checked>
                        <label class="form-check-label" for="chkVigencia">Vigente</label>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" id="btnGuardar" onclick="fct_GuardarTipoProducto()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Script -->
    <script>
        function fct_AbrirModalNuevo() {
            $('#tituloModal').text('Nuevo Tipo de Producto');
            $('#txtIdTipoProducto').val('');
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
            $('#chkVigencia').prop('checked', estado === true || estado === "true");
            $('#btnGuardar').text('Actualizar');
            $('#modal_TipoProducto').modal('show');
        }

        function fct_GuardarTipoProducto() {
            let id = $('#txtIdTipoProducto').val() || "";
            let nombre = $('#txtNombre').val().trim();
            let descripcion = $('#txtDescripcion').val().trim();
            let vigente = $('#chkVigencia').is(':checked');

            if (nombre === "") {
                toastr.warning("Ingrese el nombre del tipo de producto");
                return;
            }

            $.ajax({
                url: 'wfTipoProducto.aspx/GuardarTipoProducto',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ id: id, nombre: nombre, descripcion: descripcion, vigente: vigente }),
                success: function (response) {
                    if (response.d === 'success') {
                        toastr.success("Guardado correctamente");
                        setTimeout(() => location.reload(), 800);
                    } else {
                        toastr.error(response.d);
                    }
                },
                error: function () {
                    toastr.error("Error en la solicitud");
                }
            });
        }

        function fct_EliminarTipoProducto(id) {
            Swal.fire({
                title: "¿Eliminar este tipo de producto?",
                text: "No se podrá recuperar.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminar",
                cancelButtonText: "Cancelar",
                reverseButtons: true
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: 'wfTipoProducto.aspx/EliminarTipoProducto',
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ id: id }),
                        success: function (response) {
                            if (response.d === 'success') {
                                toastr.success("Eliminado correctamente");
                                setTimeout(() => location.reload(), 800);
                            } else {
                                toastr.error(response.d);
                            }
                        },
                        error: function () {
                            toastr.error("Error en el servidor");
                        }
                    });
                }
            });
        }

        function fct_DarBajaTipoProducto(id) {
            Swal.fire({
                title: "¿Dar de baja?",
                text: "El registro se marcará como inactivo.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, dar de baja",
                cancelButtonText: "Cancelar",
                reverseButtons: true
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: 'wfTipoProducto.aspx/DarBajaTipoProducto',
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ id: id }),
                        success: function (response) {
                            if (response.d === 'success') {
                                toastr.success("Tipo de producto dado de baja");
                                setTimeout(() => location.reload(), 800);
                            } else {
                                toastr.error(response.d);
                            }
                        },
                        error: function () {
                            toastr.error("Error en el servidor");
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
