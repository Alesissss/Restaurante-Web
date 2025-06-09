<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfCajero.aspx.vb" Inherits="appWebSistemaRestaurante.wfCajero" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <h1>Gestionar cajeros</h1>
            </div>
            <div class="panel-body">
                <div class="row mb-2">
                    <div class="col-md-12 text-right">
                        <button class="btn btn-success" onclick="fct_AbrirModalNuevo()">
                            <i class="fas fa-plus-circle"></i> Nuevo Cajero
                        </button>
                    </div>
                </div>
                <table class="table table-bordered text-center" id="tbl_Cajero">
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
                    <tbody id="tbody_Cajero" runat="server"></tbody>
                </table>
            </div>
        </div>
    </section>

    <!-- Modal -->
    <div class="modal fade" id="modal_Cajero" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModal">Registrar / Editar Cajero</h5>
                    <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                </div>
                <div class="modal-body row">
                    <input type="hidden" id="txtIdCajero">

                    <div class="col-md-6 mb-2">
                        <label>DNI</label>
                        <input type="text" id="txtDni" class="form-control" maxlength="8">
                    </div>
                    <div class="col-md-6 mb-2">
                        <label>Nombres</label>
                        <input type="text" id="txtNombres" class="form-control">
                    </div>
                    <div class="col-md-6 mb-2">
                        <label>Apellidos</label>
                        <input type="text" id="txtApellidos" class="form-control">
                    </div>
                    <div class="col-md-6 mb-2">
                        <label>Teléfono</label>
                        <input type="text" id="txtTelefono" class="form-control">
                    </div>
                    <div class="col-md-12 mb-2">
                        <label>Correo</label>
                        <input type="email" id="txtCorreo" class="form-control">
                    </div>
                    <div class="col-md-12 form-check form-switch">
                        <input class="form-check-input" type="checkbox" id="chkEstado" checked>
                        <label class="form-check-label">Activo</label>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button class="btn btn-primary" onclick="fct_GuardarCajero()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        function fct_AbrirModalNuevo() {
            $('#tituloModal').text('Nuevo Cajero');
            $('#txtIdCajero, #txtDni, #txtNombres, #txtApellidos, #txtTelefono, #txtCorreo').val('');
            $('#chkEstado').prop('checked', true);
            $('#modal_Cajero').modal('show');
        }

        function fct_EditarCajero(id, dni, nom, ape, tel, cor, est) {
            $('#tituloModal').text('Editar Cajero');
            $('#txtIdCajero').val(id);
            $('#txtDni').val(dni);
            $('#txtNombres').val(nom);
            $('#txtApellidos').val(ape);
            $('#txtTelefono').val(tel);
            $('#txtCorreo').val(cor);
            $('#chkEstado').prop('checked', est === 'Activo');
            $('#modal_Cajero').modal('show');
        }

        function fct_GuardarCajero() {
            const data = {
                id: $('#txtIdCajero').val() || "",
                dni: $('#txtDni').val().trim(),
                nom: $('#txtNombres').val().trim(),
                ape: $('#txtApellidos').val().trim(),
                tel: $('#txtTelefono').val().trim(),
                cor: $('#txtCorreo').val().trim(),
                est: $('#chkEstado').is(':checked')
            };

            $.ajax({
                url: 'wfCajero.aspx/GuardarCajero',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(data),
                success: function (response) {
                    if (response.d === 'success') {
                        toastr.success('Guardado correctamente');
                        setTimeout(() => location.reload(), 800);
                    } else {
                        toastr.error(response.d);
                    }
                },
                error: () => toastr.error('Error en el servidor')
            });
        }

        function fct_EliminarCajero(id) {
            Swal.fire({
                title: '¿Eliminar cajero?',
                text: "Esta acción no se puede deshacer.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: "Cancelar",
                reverseButtons: true
            }).then(res => {
                if (res.isConfirmed) {
                    $.ajax({
                        url: 'wfCajero.aspx/EliminarCajero',
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify({ id }),
                        success: res => {
                            if (res.d === 'success') {
                                toastr.success('Eliminado');
                                setTimeout(() => location.reload(), 800);
                            } else {
                                toastr.error(res.d);
                            }
                        }
                    });
                }
            });
        }

        function fct_DarBajaCajero(id) {
            Swal.fire({
                title: '¿Dar de baja?',
                text: "El cajero quedará como inactivo.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: 'Sí, dar de baja',
                cancelButtonText: "Cancelar",
                reverseButtons: true
            }).then(res => {
                if (res.isConfirmed) {
                    $.ajax({
                        url: 'wfCajero.aspx/DarBajaCajero',
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify({ id }),
                        success: res => {
                            if (res.d === 'success') {
                                toastr.success('Baja exitosa');
                                setTimeout(() => location.reload(), 800);
                            } else {
                                toastr.error(res.d);
                            }
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
