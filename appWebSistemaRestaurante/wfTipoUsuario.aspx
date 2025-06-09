<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfTipoUsuario.aspx.vb" Inherits="appWebSistemaRestaurante.wfTipoUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-12">
                        <h1>Gestionar tipos de usuario</h1>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                 <div class="row mb-2">
                    <div class="col-md-12 text-right">
                        <button type="button" class="btn btn-success" onclick="fct_AbrirModalNuevo()">
                            <i class="fas fa-plus-circle"></i> Nuevo Tipo Usuario
                        </button>
                    </div>
                </div>

                <table class="table table-bordered table-hover text-center" id="tbl_TipoUsuario">
                    <thead class="bg-dark text-white">
                        <tr>
                            <th>ID</th>
                            <th>Nombre</th>
                            <th>Descripción</th>
                            <th>Vigencia</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody id="tbody_TipoUsuario" runat="server">
                        <!-- Se llenará desde el backend -->
                    </tbody>
                </table>
            </div>                       
        </div>
    </section>

    <!-- Modal -->
    <div class="modal fade" id="modal_TipoUsuario" tabindex="-1" aria-labelledby="tituloModal" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModal">Registrar / Editar Tipo Usuario</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span>×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <!-- Campo oculto para ID -->
                        <input type="hidden" id="txtIdTipoUsuario">

                        <!-- Nombre -->
                        <div class="col-md-12 mb-3">
                            <label for="txtNombre" class="form-label">Nombre</label>
                            <input type="text" class="form-control" id="txtNombre" placeholder="Ingrese el nombre">
                        </div>

                        <!-- Descripción -->
                        <div class="col-md-12 mb-3">
                            <label for="txtDescripcion" class="form-label">Descripción</label>
                            <textarea class="form-control" id="txtDescripcion" rows="3" placeholder="Ingrese la descripción"></textarea>
                        </div>

                        <!-- Vigencia -->
                        <div class="col-md-12 form-check form-switch">
                            <input class="form-check-input" type="checkbox" id="chkVigencia" checked>
                            <label class="form-check-label" for="chkVigencia">Vigente</label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" id="btnGuardar" onclick="fct_GuardarTipoUsuario()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

<script>
    function fct_AbrirModalNuevo() {
        $('#tituloModal').text('Nuevo Tipo Usuario');
        $('#hdnIdTipoUsuario').val('');
        $('#txtNombre').val('');
        $('#txtDescripcion').val('');
        $('#chkVigencia').prop('checked', true);
        $('#modal_TipoUsuario').modal('show');
    }

    function fct_EditarTipoUsuario(id, nombre, descripcion, estado) {
        $('#tituloModal').text('Editar Tipo Usuario');

        // Llenamos los campos del modal
        document.getElementById("txtIdTipoUsuario").value = id;
        document.getElementById("txtNombre").value = nombre;
        document.getElementById("txtDescripcion").value = descripcion;

        // Convertimos a booleano por si viene como texto
        const estadoBool = (estado === true || estado === "true");
        document.getElementById("chkVigencia").checked = estadoBool;

        // Cambiamos el texto del botón
        document.getElementById("btnGuardar").innerText = "Actualizar";

        // Mostramos el modal
        const modal = new bootstrap.Modal(document.getElementById("modal_TipoUsuario"));
        modal.show();
    }

    function fct_GuardarTipoUsuario() {
        let id = $('#txtIdTipoUsuario').val() || "";
        let nombre = $('#txtNombre').val().trim();
        let descripcion = $('#txtDescripcion').val().trim();
        let vigente = $('#chkVigencia').is(':checked');

        if (nombre === '') {
            toastr.warning('Ingrese un nombre');
            return;
        }

        $.ajax({
            url: 'wfTipoUsuario.aspx/GuardarTipoUsuario',
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
                toastr.error('Error en la solicitud');
            }
        });
    }

    function fct_EliminarTipoUsuario(id) {
        Swal.fire({
            title: "¿Deseas eliminar este tipo usuario?",
            text: "El registro no se podrá recuperar.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, eliminar",
            cancelButtonText: "Cancelar",
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfTipoUsuario.aspx/EliminarTipoUsuario',
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

    function fct_DarBajaTipoUsuario(id) {
        Swal.fire({
            title: "¿Dar de baja?",
            text: "El registro se marcará como no vigente.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, dar de baja",
            cancelButtonText: "Cancelar",
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfTipoUsuario.aspx/DarBajaTipoUsuario',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.success('Tipo usuario dado de baja correctamente');
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