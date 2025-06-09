<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfCliente.aspx.vb" Inherits="appWebSistemaRestaurante.wfCliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <h1>Gestionar clientes</h1>
                <div class="text-right mb-2">
                    <button class="btn btn-success" onclick="fct_NuevoCliente()">
                        <i class="fas fa-user-plus"></i> Nuevo Cliente
                    </button>
                </div>
            </div>
            <div class="panel-body">
                <table class="table table-bordered table-hover text-center">
                    <thead class="thead-dark">
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
                    <tbody id="tbody_Cliente" runat="server"></tbody>
                </table>
            </div>
        </div>
    </section>

    <!-- Modal -->
    <div class="modal fade" id="modalCliente" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Registrar Cliente</h5>
                    <button type="button" class="close" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <input type="hidden" id="txt_idCliente" />
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>DNI</label>
                            <input type="text" class="form-control" id="txt_dni" />
                        </div>
                        <div class="form-group col-md-6">
                            <label>Teléfono</label>
                            <input type="text" class="form-control" id="txt_telefono" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>Nombres</label>
                            <input type="text" class="form-control" id="txt_nombres" />
                        </div>
                        <div class="form-group col-md-6">
                            <label>Apellidos</label>
                            <input type="text" class="form-control" id="txt_apellidos" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Correo</label>
                        <input type="email" class="form-control" id="txt_correo" />
                    </div>
                    <div class="form-group">
                        <label>¿Está vigente?</label>
                        <select class="form-control" id="select_estado">
                            <option value="true">Sí</option>
                            <option value="false">No</option>
                        </select>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" onclick="fct_GuardarCliente()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        function fct_NuevoCliente() {
            $('#txt_idCliente').val('');
            $('#txt_dni').val('');
            $('#txt_nombres').val('');
            $('#txt_apellidos').val('');
            $('#txt_telefono').val('');
            $('#txt_correo').val('');
            $('#select_estado').val('true');
            $('.modal-title').text('Registrar Cliente');
            $('#modalCliente').modal('show');
        }

        function fct_EditarCliente(id, dni, nombres, apellidos, telefono, correo, estado) {
            $('#txt_idCliente').val(id);
            $('#txt_dni').val(dni);
            $('#txt_nombres').val(nombres);
            $('#txt_apellidos').val(apellidos);
            $('#txt_telefono').val(telefono);
            $('#txt_correo').val(correo);
            $('#select_estado').val(estado.toString());
            $('.modal-title').text('Editar Cliente');
            $('#modalCliente').modal('show');
        }

        function fct_GuardarCliente() {
            const id = $('#txt_idCliente').val() || 0;
            const dni = $('#txt_dni').val().trim();
            const nombres = $('#txt_nombres').val().trim();
            const apellidos = $('#txt_apellidos').val().trim();
            const telefono = $('#txt_telefono').val().trim();
            const correo = $('#txt_correo').val().trim();
            const estado = $('#select_estado').val() === "true";

            if (!dni || !nombres || !apellidos || !correo) {
                toastr.warning("Complete todos los campos requeridos");
                return;
            }

            const metodo = id == 0 ? 'guardarCliente' : 'modificarCliente';

            $.ajax({
                type: "POST",
                url: "wfCliente.aspx/" + metodo,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    id: parseInt(id),
                    dni: dni,
                    nom: nombres,
                    ape: apellidos,
                    tel: telefono,
                    cor: correo,
                    est: estado
                }),
                success: function () {
                    toastr.success("Cliente guardado correctamente");
                    $('#modalCliente').modal('hide');
                    __doPostBack();
                },
                error: function () {
                    toastr.error("Error al guardar cliente");
                }
            });
        }

        function fct_DarBajaCliente(id) {
            Swal.fire({
                title: '¿Deseas dar de baja a este cliente?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Sí, dar de baja',
                cancelButtonText: 'Cancelar',
                cancelButtonColor: '#d33',
                confirmButtonColor: '#3085d6',
                reverseButtons: true
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: "POST",
                        url: "wfCliente.aspx/darBajaCliente",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ id: id }),
                        success: function () {
                            toastr.success("Cliente dado de baja");
                            __doPostBack();
                        },
                        error: function () {
                            toastr.error("Error al dar de baja cliente");
                        }
                    });
                }
            });
        }

        function fct_EliminarCliente(id) {
            Swal.fire({
                title: '¿Eliminar cliente permanentemente?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar',
                cancelButtonColor: '#d33',
                confirmButtonColor: '#3085d6',
                reverseButtons: true
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: "POST",
                        url: "wfCliente.aspx/eliminarCliente",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ id: id }),
                        success: function () {
                            toastr.success("Cliente eliminado correctamente");
                            __doPostBack();
                        },
                        error: function () {
                            toastr.error("Error al eliminar cliente");
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
