<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfMesero.aspx.vb" Inherits="appWebSistemaRestaurante.wfMesero" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-12">
                        <h1>Gestionar meseros</h1>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                 <div class="row mb-2">
                    <div class="col-md-12 text-right">
                        <button class="btn btn-success" onclick="fct_AbrirModalNuevo()">
                            <i class="fas fa-plus-circle"></i> Nuevo Mesero
                        </button>
                    </div>
                </div>
                <table class="table table-bordered text-center" id="tbl_Mesero">
                    <thead class="bg-dark text-white">
                        <tr>
                            <th>ID</th>
                            <th>DNI</th>
                            <th>Nombres</th>
                            <th>Apellidos</th>
                            <th>Sexo</th>
                            <th>F. Nac</th>
                            <th>Teléfono</th>
                            <th>Correo</th>
                            <th>Estado</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody id="tbody_Mesero" runat="server"></tbody>
                </table>
            </div>
        </div>
    </section>

    <!-- Modal -->
    <div class="modal fade" id="modal_Mesero" tabindex="-1" aria-labelledby="tituloModal" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModal">Registrar / Editar Mesero</h5>
                    <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                </div>
                <div class="modal-body row">
                    <input type="hidden" id="txtIdMesero">

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
                        <label>Sexo</label>
                        <select id="cboSexo" class="form-control">
                            <option>Masculino</option>
                            <option>Femenino</option>
                        </select>
                    </div>
                    <div class="col-md-6 mb-2">
                        <label>Fecha Nacimiento</label>
                        <input type="date" id="txtFechaNac" class="form-control">
                    </div>
                    <div class="col-md-6 mb-2">
                        <label>Teléfono</label>
                        <input type="text" id="txtTelefono" class="form-control">
                    </div>
                    <div class="col-md-12 mb-2">
                        <label>Dirección</label>
                        <input type="text" id="txtDireccion" class="form-control">
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
                    <button class="btn btn-primary" onclick="fct_GuardarMesero()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

<script>
    function fct_AbrirModalNuevo() {
        $('#tituloModal').text('Nuevo Mesero');
        $('#txtIdMesero, #txtDni, #txtNombres, #txtApellidos, #txtFechaNac, #txtDireccion, #txtTelefono, #txtCorreo').val('');
        $('#cboSexo').val('Masculino');
        $('#chkEstado').prop('checked', true);
        $('#modal_Mesero').modal('show');
    }

    function fct_EditarMesero(id, dni, nom, ape, sex, fec, dir, tel, cor, est) {
        $('#tituloModal').text('Editar Mesero');
        $('#txtIdMesero').val(id);
        $('#txtDni').val(dni);
        $('#txtNombres').val(nom);
        $('#txtApellidos').val(ape);
        $('#cboSexo').val(sex);
        $('#txtFechaNac').val(fec);
        $('#txtDireccion').val(dir);
        $('#txtTelefono').val(tel);
        $('#txtCorreo').val(cor);
        $('#chkEstado').prop('checked', est === 'Activo');
        $('#modal_Mesero').modal('show');
    }

    function fct_GuardarMesero() {
        const data = {
            id: $('#txtIdMesero').val() || "",
            dni: $('#txtDni').val().trim(),
            nom: $('#txtNombres').val().trim(),
            ape: $('#txtApellidos').val().trim(),
            sex: $('#cboSexo').val(),
            fec: $('#txtFechaNac').val(),
            dir: $('#txtDireccion').val().trim(),
            tel: $('#txtTelefono').val().trim(),
            cor: $('#txtCorreo').val().trim(),
            est: $('#chkEstado').is(':checked')
        };

        $.ajax({
            url: 'wfMesero.aspx/GuardarMesero',
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

    function fct_EliminarMesero(id) {
        Swal.fire({
            title: '¿Eliminar mesero?',
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
                    url: 'wfMesero.aspx/EliminarMesero',
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

    function fct_DarBajaMesero(id) {
        Swal.fire({
            title: '¿Dar de baja?',
            text: "El mesero quedará como inactivo.",
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
                    url: 'wfMesero.aspx/DarBajaMesero',
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
