<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfProducto.aspx.vb" Inherits="appWebSistemaRestaurante.wfProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-12">
                    <h1>Gestionar productos</h1>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="row mb-2">
                    <div class="col-md-12 text-right">
                            <button class="btn btn-success" onclick="fct_NuevoProducto()">
                                <i class="fas fa-plus-circle"></i> Nuevo Producto
                            </button>
                    </div>
                </div>
                    <table class="table table-bordered table-hover text-center">
                        <thead class="thead-dark">
                            <tr>
                                <th>ID</th>
                                <th>Nombre</th>
                                <th>Descripción</th>
                                <th>Precio</th>
                                <th>Tipo</th>
                                <th>Carta</th>
                                <th>Estado</th>
                                <th>Acciones</th>
                            </tr>
                        </thead>
                        <tbody id="tbody_Producto" runat="server"></tbody>
                    </table>
            </div>
        </div>
    </section>

    <!-- Modal Producto -->
    <div class="modal fade" id="modalProducto" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Registrar Producto</h5>
                    <button type="button" class="close" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <input type="hidden" id="txt_idProducto" />
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>Nombre</label>
                            <input type="text" class="form-control" id="txt_nombre" />
                        </div>
                        <div class="form-group col-md-6">
                            <label>Precio</label>
                            <input type="number" class="form-control" id="txt_precio" step="0.01" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Descripción</label>
                        <textarea class="form-control" id="txt_descripcion"></textarea>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>Tipo de Producto</label>
                            <select class="form-control" id="select_tipo" runat="server"></select>
                        </div>
                        <div class="form-group col-md-6">
                            <label>Carta</label>
                            <select class="form-control" id="select_carta" runat="server"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label>¿Está vigente?</label>
                        <select class="form-control" id="select_vigencia">
                            <option value="true">Sí</option>
                            <option value="false">No</option>
                        </select>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" onclick="fct_GuardarProducto()">Guardar</button>
                </div>
            </div>
        </div>
    </div>
<script>
    function fct_NuevoProducto() {
        $('#txt_idProducto').val('');
        $('#txt_nombre').val('');
        $('#txt_precio').val('');
        $('#txt_descripcion').val('');
        $('#select_tipo').val('');
        $('#select_carta').val('');
        $('#select_vigencia').val('true');
        $('.modal-title').text('Registrar Producto');
        $('#modalProducto').modal('show');
    }

    function fct_EditarProducto(id, nombre, descripcion, precio, idTipo, idCarta, vigencia) {
        $('#txt_idProducto').val(id);
        $('#txt_nombre').val(nombre);
        $('#txt_precio').val(precio);
        $('#txt_descripcion').val(descripcion);
        $('#select_tipo').val(idTipo);
        $('#select_carta').val(idCarta);
        $('#select_vigencia').val(vigencia.toString());
        $('.modal-title').text('Editar Producto');
        $('#modalProducto').modal('show');
    }

    function fct_GuardarProducto() {
        const id = $('#txt_idProducto').val() || 0;
        const nombre = $('#txt_nombre').val().trim();
        const precio = parseFloat($('#txt_precio').val());
        const descripcion = $('#txt_descripcion').val().trim();
        const idTipo = $('#select_tipo').val();
        const idCarta = $('#select_carta').val();
        const vigencia = $('#select_vigencia').val() === "true";

        if (!nombre || isNaN(precio) || !idTipo || !idCarta) {
            toastr.warning('Complete todos los campos requeridos');
            return;
        }

        const metodo = id == 0 ? 'guardarProducto' : 'modificarProducto';

        $.ajax({
            type: "POST",
            url: "wfProducto.aspx/" + metodo,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                id: parseInt(id),
                nombre: nombre,
                descripcion: descripcion,
                precio: precio,
                idTipo: parseInt(idTipo),
                idCarta: parseInt(idCarta),
                vigencia: vigencia
            }),
            success: function () {
                toastr.success("Producto guardado exitosamente");
                $('#modalProducto').modal('hide');
                __doPostBack(); // puedes reemplazar esto por recarga dinámica si deseas
            },
            error: function () {
                toastr.error("Error al guardar el producto");
            }
        });
    }

    function fct_DarBajaProducto(id) {
        Swal.fire({
            title: '¿Deseas dar de baja este producto?',
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
                    url: "wfProducto.aspx/darBajaProducto",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ id: id }),
                    success: function () {
                        toastr.success("Producto dado de baja");
                        __doPostBack();
                    },
                    error: function () {
                        toastr.error("Error al dar de baja el producto");
                    }
                });
            }
        });
    }

    function fct_EliminarProducto(id) {
        Swal.fire({
            title: '¿Estás seguro de eliminar este producto permanentemente?',
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
                    url: "wfProducto.aspx/eliminarProducto",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ id: id }),
                    success: function () {
                        toastr.success("Producto eliminado correctamente");
                        __doPostBack();
                    },
                    error: function () {
                        toastr.error("Error al eliminar el producto");
                    }
                });
            }
        });
    }
</script>
</asp:Content>
