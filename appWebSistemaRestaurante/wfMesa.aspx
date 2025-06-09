<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfMesa.aspx.vb" Inherits="appWebSistemaRestaurante.wfMesa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<section class="content">
    <div class="container-fluid p-4">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-12">
                    <h1>Gestionar mesas</h1>
                </div>
            </div>
        </div>
        <div class="panel-body">
             <div class="row mb-2">
                <div class="col-md-12 text-right">
                    <button type="button" class="btn btn-success" onclick="fct_AbrirModalNuevo()">
                        <i class="fas fa-plus-circle"></i> Nueva mesa
                    </button>
                </div>
            </div>
            <table class="table table-bordered table-hover text-center">
                <thead class="bg-dark text-white">
                    <tr>
                        <th>ID</th>
                        <th>Número</th>
                        <th>Capacidad</th>
                        <th>Estado</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody id="tbody_Mesa" runat="server"></tbody>
            </table>
        </div>
    </div>
</section>

<!-- Modal -->
<div class="modal fade" id="modal_Mesa" tabindex="-1">
    <div class="modal-dialog modal-md">
        <div class="modal-content">
            <div class="modal-header bg-primary text-white">
                <h5 class="modal-title" id="tituloModal">Registrar / Editar Mesa</h5>
                <button type="button" class="close" data-bs-dismiss="modal"><span>&times;</span></button>
            </div>
            <div class="modal-body">
                <input type="hidden" id="txtIdMesa">
                <div class="mb-3">
                    <label>Número</label>
                    <input type="number" class="form-control" id="txtNumero" min="1">
                </div>
                <div class="mb-3">
                    <label>Capacidad</label>
                    <input type="number" class="form-control" id="txtCapacidad" min="1">
                </div>
                <div class="form-check form-switch mb-3">
                    <input class="form-check-input" type="checkbox" id="chkEstado" checked>
                    <label class="form-check-label">Activo</label>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                <button class="btn btn-primary" onclick="fct_GuardarMesa()">Guardar</button>
            </div>
        </div>
    </div>
</div>

<script>
    function fct_AbrirModalNuevo() {
        $('#tituloModal').text('Nueva Mesa');
        $('#txtIdMesa').val('');
        $('#txtNumero').val('');
        $('#txtCapacidad').val('');
        $('#chkEstado').prop('checked', true);
        $('#modal_Mesa').modal('show');
    }

    function fct_EditarMesa(id, numero, capacidad, estado) {
        $('#tituloModal').text('Editar Mesa');
        $('#txtIdMesa').val(id);
        $('#txtNumero').val(numero);
        $('#txtCapacidad').val(capacidad);
        $('#chkEstado').prop('checked', estado === true || estado === "true");
        $('#modal_Mesa').modal('show');
    }

    function fct_GuardarMesa() {
        let id = $('#txtIdMesa').val() || "";
        let numero = $('#txtNumero').val().trim();
        let capacidad = $('#txtCapacidad').val().trim();
        let estado = $('#chkEstado').is(':checked');

        if (!numero || !capacidad) {
            toastr.warning('Complete todos los campos.');
            return;
        }

        $.ajax({
            url: 'wfMesa.aspx/GuardarMesa',
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ id, numero, capacidad, estado }),
            success: function (res) {
                if (res.d === 'success') {
                    toastr.success('Guardado correctamente');
                    setTimeout(() => location.reload(), 800);
                } else {
                    toastr.error(res.d);
                }
            }
        });
    }

    function fct_EliminarMesa(id) {
        Swal.fire({
            title: "¿Eliminar mesa?",
            text: "Esta acción no se puede deshacer.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, eliminar",
            cancelButtonText: "Cancelar",
            reverseButtons: true
        }).then(result => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfMesa.aspx/EliminarMesa',
                    method: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id }),
                    success: function (res) {
                        if (res.d === 'success') {
                            toastr.success('Eliminado correctamente');
                            setTimeout(() => location.reload(), 800);
                        } else {
                            toastr.error(res.d);
                        }
                    }
                });
            }
        });
    }

    function fct_DarBajaMesa(id) {
        Swal.fire({
            title: "¿Dar de baja mesa?",
            text: "La mesa quedará como inactiva.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, dar de baja",
            cancelButtonText: "Cancelar",
            reverseButtons: true
        }).then(result => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfMesa.aspx/DarBajaMesa',
                    method: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id }),
                    success: function (res) {
                        if (res.d === 'success') {
                            toastr.success('Mesa dada de baja');
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
