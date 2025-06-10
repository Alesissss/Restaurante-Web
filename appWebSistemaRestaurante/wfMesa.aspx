<%-- Archivo: wfMesa.aspx --%>
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfMesa.aspx.vb" Inherits="appWebSistemaRestaurante.wfMesa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <style>
        .card-mesa { transition: all 0.2s ease-in-out; border-width: 2px; }
        .card-mesa:hover { transform: translateY(-5px); box-shadow: 0 4px 15px rgba(0,0,0,0.15); }
        .card-mesa .card-body { padding: 1.5rem; }
        .card-mesa .icono-mesa { font-size: 3.5rem; opacity: 0.7; }
        .card-mesa .numero-mesa { font-size: 1.75rem; font-weight: bold; }
        .card-mesa .capacidad-mesa { font-size: 1rem; }
        .card-footer { background-color: rgba(0,0,0,0.03); }
    </style>

    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <div class="row">
                    <div class="col"><h1>Gestionar Mesas</h1></div>
                    <div class="col text-right align-self-center">
                        <button type="button" class="btn btn-success" onclick="fct_AbrirModalNuevaMesa()">
                            <i class="fas fa-plus-circle"></i> Nueva Mesa
                        </button>
                    </div>
                </div>
            </div>
            <div class="panel-body mt-3">
                <div class="row" id="cardContainer_Mesa" runat="server"></div>
            </div>
        </div>
    </section>

    <div class="modal fade" id="modal_Mesa" tabindex="-1" aria-labelledby="tituloModal" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModal"></h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span>&times;</span></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <input type="hidden" id="txtIdMesa">
                        <div class="col-md-6 mb-3">
                            <label for="txtNumero" class="form-label">Número de Mesa</label>
                            <input type="number" class="form-control" id="txtNumero" placeholder="Ej: 5" min="1">
                        </div>
                        <div class="col-md-6 mb-3">
                            <label for="txtCapacidad" class="form-label">Capacidad (personas)</label>
                            <input type="number" class="form-control" id="txtCapacidad" placeholder="Ej: 4" min="1">
                        </div>
                        <div class="col-md-12 form-check form-switch mt-2 ml-2">
                            <input class="form-check-input" type="checkbox" id="chkVigencia">
                            <label class="form-check-label" for="chkVigencia">Vigente / En servicio</label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" id="btnGuardar" onclick="fct_GuardarMesa()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

<script>
    function fct_AbrirModalNuevaMesa() {
        $('#tituloModal').text('Nueva Mesa');
        $('#txtIdMesa').val('0');
        $('#txtNumero').val('');
        $('#txtCapacidad').val('');
        $('#chkVigencia').prop('checked', true);
        $('#btnGuardar').text('Guardar');
        $('#modal_Mesa').modal('show');
    }

    function fct_EditarMesa(id, numero, capacidad, vigencia) {
        $('#tituloModal').text('Editar Mesa');
        $('#txtIdMesa').val(id);
        $('#txtNumero').val(numero);
        $('#txtCapacidad').val(capacidad);
        $('#chkVigencia').prop('checked', vigencia);
        $('#btnGuardar').text('Actualizar');
        $('#modal_Mesa').modal('show');
    }

    function fct_GuardarMesa() {
        let id = $('#txtIdMesa').val() || "0";
        let numero = $('#txtNumero').val();
        let capacidad = $('#txtCapacidad').val();
        let vigencia = $('#chkVigencia').is(':checked');

        if (!numero || parseInt(numero) <= 0 || !capacidad || parseInt(capacidad) <= 0) {
            toastr.warning('El número y la capacidad son obligatorios.'); return;
        }

        $.ajax({
            url: 'wfMesa.aspx/GuardarMesa', method: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ id: id, numero: numero, capacidad: capacidad, vigencia: vigencia }),
            success: function (r) { (r.d === 'success') ? (toastr.success('Guardado correctamente'), setTimeout(() => location.reload(), 800)) : toastr.error(r.d); },
            error: function () { toastr.error('Error en la solicitud.'); }
        });
    }

    function fct_EliminarMesa(id) {
        Swal.fire({
            title: "¿Eliminar permanentemente esta mesa?", text: "Esta acción no se puede revertir.",
            icon: "warning", showCancelButton: true, confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6", confirmButtonText: "Sí, eliminar"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfMesa.aspx/EliminarMesa', type: 'POST',
                    contentType: 'application/json; charset=utf-8', data: JSON.stringify({ id: id }),
                    success: function (r) { (r.d === 'success') ? (toastr.success('Eliminada'), setTimeout(() => location.reload(), 800)) : toastr.error(r.d); },
                    error: function () { toastr.error('Error en el servidor.'); }
                });
            }
        });
    }

    function fct_CambiarVigenciaMesa(id, nuevaVigencia) {
        let titulo = nuevaVigencia ? "¿Poner en servicio?" : "¿Poner en mantenimiento?";
        let texto = nuevaVigencia ? "La mesa volverá a estar activa." : "La mesa se marcará como no vigente.";
        let confirmText = nuevaVigencia ? "Sí, poner en servicio" : "Sí, dar de baja";

        Swal.fire({
            title: titulo, text: texto, icon: "question", showCancelButton: true,
            confirmButtonText: confirmText,
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'wfMesa.aspx/CambiarVigenciaMesa', type: 'POST',
                    contentType: 'application/json; charset=utf-8', data: JSON.stringify({ id: id, nuevaVigencia: nuevaVigencia }),
                    success: function (r) { (r.d === 'success') ? (toastr.success('Vigencia actualizada'), setTimeout(() => location.reload(), 800)) : toastr.error(r.d); },
                    error: function () { toastr.error('Error en el servidor.'); }
                });
            }
        });
    }
</script>
</asp:Content>