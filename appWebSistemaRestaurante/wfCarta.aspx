<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfCarta.aspx.vb" Inherits="appWebSistemaRestaurante.wfCarta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-12">
                        <h1>Gestionar Cartas</h1>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="row mb-2">
                    <div class="col-md-12 text-right">
                        <button type="button" class="btn btn-success" onclick="fct_AbrirModalNuevaCarta()">
                            <i class="fas fa-plus-circle"></i> Nueva Carta
                        </button>
                    </div>
                </div>

                <table class="table table-bordered table-hover text-center" id="tbl_Carta">
                    <thead class="bg-dark text-white">
                        <tr>
                            <th>ID</th>
                            <th>Nombre</th>
                            <th>Descripción</th>
                            <th>Vigencia</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody id="tbody_Carta" runat="server">
                        </tbody>
                </table>
            </div>
        </div>
    </section>

    <div class="modal fade" id="modal_Carta" tabindex="-1" aria-labelledby="tituloModal" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModal">Registrar / Editar Carta</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span>×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <input type="hidden" id="txtIdCarta">

                        <div class="col-md-12 mb-3">
                            <label for="txtNombre" class="form-label">Nombre</label>
                            <input type="text" class="form-control" id="txtNombre" placeholder="Ej: Carta de Verano, Menú Ejecutivo">
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
                    <button type="button" class="btn btn-primary" id="btnGuardar" onclick="fct_GuardarCarta()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

<script>
    function fct_AbrirModalNuevaCarta() {
        $('#tituloModal').text('Nueva Carta');
        $('#txtIdCarta').val('0');
        $('#txtNombre').val('');
        $('#txtDescripcion').val('');
        $('#chkVigencia').prop('checked', true);
        $('#btnGuardar').text('Guardar');
        $('#modal_Carta').modal('show');
    }

    function fct_EditarCarta(id, nombre, descripcion, estado) {
        $('#tituloModal').text('Editar Carta');
        
        $('#txtIdCarta').val(id);
        $('#txtNombre').val(nombre);
        $('#txtDescripcion').val(descripcion);
        
        const estadoBool = (estado.toLowerCase() === 'activo');
        $('#chkVigencia').prop('checked', estadoBool);
        
        $('#btnGuardar').text('Actualizar');
        $('#modal_Carta').modal('show');
    }

    function fct_GuardarCarta() {
        let id = $('#txtIdCarta').val() || "0";
        let nombre = $('#txtNombre').val().trim();
        let descripcion = $('#txtDescripcion').val().trim();
        let vigente = $('#chkVigencia').is(':checked');

        if (nombre === '') {
            toastr.warning('El nombre de la carta es obligatorio.');
            return;
        }

        $.ajax({
            url: 'wfCarta.aspx/GuardarCarta',
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

    function fct_EliminarCarta(id) {
        Swal.fire({
            title: "¿Deseas eliminar esta carta?",
            text: "Esta acción no se puede revertir.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, eliminar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfCarta.aspx/EliminarCarta',
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

    function fct_DarBajaCarta(id) {
        Swal.fire({
            title: "¿Dar de baja a esta carta?",
            text: "La carta se marcará como no vigente.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, dar de baja",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfCarta.aspx/DarBajaCarta',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.success('Carta dada de baja correctamente');
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

    function fct_DarAltaCarta(id) {
        Swal.fire({
            title: "¿Reactivar esta carta?",
            text: "La carta se marcará como vigente.",
            icon: "info",
            showCancelButton: true,
            confirmButtonColor: "#28a745",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, reactivar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfCarta.aspx/DarAltaCarta',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.success('Carta reactivada correctamente');
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