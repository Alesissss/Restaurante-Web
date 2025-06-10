<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfMesero.aspx.vb" Inherits="appWebSistemaRestaurante.wfMesero" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-12">
                        <h1>Gestionar Meseros</h1>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="row mb-2">
                    <div class="col-md-12 text-right">
                        <button type="button" class="btn btn-success" onclick="fct_AbrirModalNuevoMesero()">
                            <i class="fas fa-plus-circle"></i> Nuevo Mesero
                        </button>
                    </div>
                </div>

                <table class="table table-bordered table-hover text-center" id="tbl_Mesero">
                    <thead class="bg-dark text-white">
                        <tr>
                            <th>ID</th>
                            <th>DNI</th>
                            <th>Nombres y Apellidos</th>
                            <th>Teléfono</th>
                            <th>Correo</th>
                            <th>Estado</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody id="tbody_Mesero" runat="server">
                        </tbody>
                </table>
            </div>
        </div>
    </section>

    <div class="modal fade" id="modal_Mesero" tabindex="-1" aria-labelledby="tituloModal" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModal">Registrar / Editar Mesero</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span>×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <input type="hidden" id="txtIdMesero">

                        <div class="col-md-6 mb-3">
                            <label for="txtDni" class="form-label">DNI</label>
                            <input type="text" class="form-control" id="txtDni" placeholder="DNI (8 dígitos)" maxlength="8">
                        </div>

                        <div class="col-md-6 mb-3">
                            <label for="txtFechaNac" class="form-label">Fecha de Nacimiento</label>
                            <input type="date" class="form-control" id="txtFechaNac">
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
                            <label for="ddlSexo" class="form-label">Sexo</label>
                            <select id="ddlSexo" class="form-control">
                                <option value="Masculino">Masculino</option>
                                <option value="Femenino">Femenino</option>
                            </select>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label for="txtTelefono" class="form-label">Teléfono</label>
                            <input type="text" class="form-control" id="txtTelefono" placeholder="Teléfono (9 dígitos)" maxlength="9">
                        </div>

                        <div class="col-md-12 mb-3">
                            <label for="txtCorreo" class="form-label">Correo Electrónico</label>
                            <input type="email" class="form-control" id="txtCorreo" placeholder="ejemplo@correo.com">
                        </div>

                        <div class="col-md-12 mb-3">
                            <label for="txtDireccion" class="form-label">Dirección</label>
                            <input type="text" class="form-control" id="txtDireccion" placeholder="Dirección completa">
                        </div>

                        <div class="col-md-12 form-check form-switch">
                            <input class="form-check-input" type="checkbox" id="chkEstado" checked>
                            <label class="form-check-label" for="chkEstado">Activo</label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" id="btnGuardar" onclick="fct_GuardarMesero()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

<script>
    function fct_AbrirModalNuevoMesero() {
        $('#tituloModal').text('Nuevo Mesero');
        $('#txtIdMesero').val('0');
        $('#txtDni').val('');
        $('#txtFechaNac').val('');
        $('#txtNombres').val('');
        $('#txtApellidos').val('');
        $('#ddlSexo').val('Masculino');
        $('#txtTelefono').val('');
        $('#txtCorreo').val('');
        $('#txtDireccion').val('');
        $('#chkEstado').prop('checked', true);
        $('#btnGuardar').text('Guardar');
        $('#modal_Mesero').modal('show');
    }

    function fct_EditarMesero(id, dni, nombres, apellidos, sexo, fechaNac, direccion, telefono, correo, estado) {
        $('#tituloModal').text('Editar Mesero');

        $('#txtIdMesero').val(id);
        $('#txtDni').val(dni);
        $('#txtNombres').val(nombres);
        $('#txtApellidos').val(apellidos);
        $('#ddlSexo').val(sexo);
        $('#txtFechaNac').val(fechaNac);
        $('#txtDireccion').val(direccion);
        $('#txtTelefono').val(telefono);
        $('#txtCorreo').val(correo);

        const estadoBool = (estado.toLowerCase() === 'activo');
        $('#chkEstado').prop('checked', estadoBool);

        $('#btnGuardar').text('Actualizar');
        $('#modal_Mesero').modal('show');
    }

    function fct_GuardarMesero() {
        let id = $('#txtIdMesero').val() || "0";
        let dni = $('#txtDni').val().trim();
        let nombres = $('#txtNombres').val().trim();
        let apellidos = $('#txtApellidos').val().trim();
        let sexo = $('#ddlSexo').val();
        let fechaNac = $('#txtFechaNac').val();
        let direccion = $('#txtDireccion').val().trim();
        let telefono = $('#txtTelefono').val().trim();
        let correo = $('#txtCorreo').val().trim();
        let estado = $('#chkEstado').is(':checked');

        if (dni === '' || nombres === '' || apellidos === '' || fechaNac === '' || direccion === '' || correo === '') {
            toastr.warning('Todos los campos son obligatorios, excepto el teléfono.');
            return;
        }

        if (dni.length !== 8) {
            toastr.warning('El DNI debe tener 8 dígitos.');
            return;
        }

        $.ajax({
            url: 'wfMesero.aspx/GuardarMesero',
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                id: id, dni: dni, nom: nombres, ape: apellidos, sex: sexo,
                fec: fechaNac, dir: direccion, tel: telefono, cor: correo, est: estado
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

    function fct_EliminarMesero(id) {
        Swal.fire({
            title: "¿Deseas eliminar este mesero?",
            icon: "warning", showCancelButton: true, confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6", confirmButtonText: "Sí, eliminar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfMesero.aspx/EliminarMesero', type: 'POST',
                    contentType: 'application/json; charset=utf-8', data: JSON.stringify({ id: id }),
                    success: function (r) { (r.d === 'success') ? (toastr.success('Eliminado'), setTimeout(() => location.reload(), 800)) : toastr.error(r.d); },
                    error: function () { toastr.error('Error en el servidor'); }
                });
            }
        });
    }

    function fct_DarBajaMesero(id) {
        Swal.fire({
            title: "¿Dar de baja a este mesero?", icon: "warning", showCancelButton: true,
            confirmButtonText: "Sí, dar de baja"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfMesero.aspx/DarBajaMesero', type: 'POST',
                    contentType: 'application/json; charset=utf-8', data: JSON.stringify({ id: id }),
                    success: function (r) { (r.d === 'success') ? (toastr.info('Mesero dado de baja'), setTimeout(() => location.reload(), 800)) : toastr.error(r.d); },
                    error: function () { toastr.error('Error en el servidor'); }
                });
            }
        });
    }

    function fct_DarAltaMesero(id) {
        Swal.fire({
            title: "¿Reactivar a este mesero?", icon: "info", showCancelButton: true,
            confirmButtonColor: "#28a745", confirmButtonText: "Sí, reactivar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfMesero.aspx/DarAltaMesero', type: 'POST',
                    contentType: 'application/json; charset=utf-8', data: JSON.stringify({ id: id }),
                    success: function (r) { (r.d === 'success') ? (toastr.success('Mesero reactivado'), setTimeout(() => location.reload(), 800)) : toastr.error(r.d); },
                    error: function () { toastr.error('Error en el servidor'); }
                });
            }
        });
    }
</script>
</asp:Content>