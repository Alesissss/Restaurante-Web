<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfCajero.aspx.vb" Inherits="appWebSistemaRestaurante.wfCajero" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-12">
                        <h1>Gestionar Cajeros</h1>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="row mb-2">
                    <div class="col-md-12 text-right">
                        <button type="button" class="btn btn-success" onclick="fct_AbrirModalNuevoCajero()">
                            <i class="fas fa-plus-circle"></i> Nuevo Cajero
                        </button>
                    </div>
                </div>

                <table class="table table-bordered table-hover text-center" id="tbl_Cajero">
                    <thead class="bg-dark text-white">
                        <tr>
                            <th>ID</th>
                            <th>DNI</th>
                            <th>Nombres</th>
                            <th>Apellidos</th>
                            <th>Teléfono</th>
                            <th>Correo</th>
                            <th>Estado</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody id="tbody_Cajero" runat="server">
                        </tbody>
                </table>
            </div>
        </div>
    </section>

    <div class="modal fade" id="modal_Cajero" tabindex="-1" aria-labelledby="tituloModal" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModal">Registrar / Editar Cajero</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span>×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <input type="hidden" id="txtIdCajero">

                        <div class="col-md-6 mb-3">
                            <label for="txtDni" class="form-label">DNI</label>
                            <input type="text" class="form-control" id="txtDni" placeholder="Ingrese el DNI" maxlength="8">
                        </div>
                        
                        <div class="col-md-6 mb-3">
                            <label for="txtCorreo" class="form-label">Correo Electrónico</label>
                            <input type="email" class="form-control" id="txtCorreo" placeholder="ejemplo@correo.com">
                        </div>

                        <div class="col-md-6 mb-3">
                            <label for="txtNombres" class="form-label">Nombres</label>
                            <input type="text" class="form-control" id="txtNombres" placeholder="Ingrese los nombres">
                        </div>

                        <div class="col-md-6 mb-3">
                            <label for="txtApellidos" class="form-label">Apellidos</label>
                            <input type="text" class="form-control" id="txtApellidos" placeholder="Ingrese los apellidos">
                        </div>

                        <div class="col-md-6 mb-3">
                            <label for="txtTelefono" class="form-label">Teléfono</label>
                            <input type="text" class="form-control" id="txtTelefono" placeholder="Ingrese el teléfono (opcional)" maxlength="9">
                        </div>

                        <div class="col-md-6 mb-3 align-self-center">
                             <div class="form-check form-switch">
                                <input class="form-check-input" type="checkbox" id="chkEstado" checked>
                                <label class="form-check-label" for="chkEstado">Activo</label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" id="btnGuardar" onclick="fct_GuardarCajero()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

<script>
    function fct_AbrirModalNuevoCajero() {
        $('#tituloModal').text('Nuevo Cajero');
        $('#txtIdCajero').val('0');
        $('#txtDni').val('');
        $('#txtNombres').val('');
        $('#txtApellidos').val('');
        $('#txtTelefono').val('');
        $('#txtCorreo').val('');
        $('#chkEstado').prop('checked', true);
        $('#btnGuardar').text('Guardar');
        $('#modal_Cajero').modal('show');
    }

    function fct_EditarCajero(id, dni, nombres, apellidos, telefono, correo, estado) {
        $('#tituloModal').text('Editar Cajero');

        $('#txtIdCajero').val(id);
        $('#txtDni').val(dni);
        $('#txtNombres').val(nombres);
        $('#txtApellidos').val(apellidos);
        $('#txtTelefono').val(telefono);
        $('#txtCorreo').val(correo);

        const estadoBool = (estado.toLowerCase() === 'activo');
        $('#chkEstado').prop('checked', estadoBool);

        $('#btnGuardar').text('Actualizar');
        $('#modal_Cajero').modal('show');
    }

    function fct_GuardarCajero() {
        let id = $('#txtIdCajero').val() || "0";
        let dni = $('#txtDni').val().trim();
        let nombres = $('#txtNombres').val().trim();
        let apellidos = $('#txtApellidos').val().trim();
        let telefono = $('#txtTelefono').val().trim();
        let correo = $('#txtCorreo').val().trim();
        let estado = $('#chkEstado').is(':checked');

        if (dni === '' || nombres === '' || apellidos === '' || correo === '') {
            toastr.warning('DNI, Nombres, Apellidos y Correo son obligatorios.');
            return;
        }

        if (dni.length !== 8) {
            toastr.warning('El DNI debe tener 8 dígitos.');
            return;
        }

        $.ajax({
            url: 'wfCajero.aspx/GuardarCajero',
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                id: id,
                dni: dni,
                nombres: nombres,
                apellidos: apellidos,
                telefono: telefono,
                correo: correo,
                estado: estado
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

    function fct_EliminarCajero(id) {
        Swal.fire({
            title: "¿Deseas eliminar este cajero?",
            text: "Esta acción no se puede revertir.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, eliminar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfCajero.aspx/EliminarCajero',
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
                    error: function () { toastr.error('Error en el servidor'); }
                });
            }
        });
    }

    function fct_DarBajaCajero(id) {
        Swal.fire({
            title: "¿Dar de baja a este cajero?",
            text: "El cajero se marcará como inactivo.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, dar de baja"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfCajero.aspx/DarBajaCajero',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.success('Cajero dado de baja correctamente');
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

    function fct_DarAltaCajero(id) {
        Swal.fire({
            title: "¿Reactivar a este cajero?",
            text: "El cajero se marcará como activo.",
            icon: "info",
            showCancelButton: true,
            confirmButtonColor: "#28a745",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, reactivar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfCajero.aspx/DarAltaCajero',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.success('Cajero reactivado correctamente');
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