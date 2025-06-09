<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfUsuario.aspx.vb" Inherits="appWebSistemaRestaurante.wfUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-12">
                        <h1>Gestionar Usuarios</h1>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="row mb-2">
                    <div class="col-md-12 text-right">
                        <button type="button" class="btn btn-success" onclick="fct_AbrirModalNuevoUsuario()">
                            <i class="fas fa-plus-circle"></i> Nuevo Usuario
                        </button>
                    </div>
                </div>

                <table class="table table-bordered table-hover text-center" id="tbl_Usuario">
                    <thead class="bg-dark text-white">
                        <tr>
                            <th>ID</th>
                            <th>Nombres Completos</th>
                            <th>Usuario</th>
                            <th>Tipo Usuario</th>
                            <th>Vigencia</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody id="tbody_Usuario" runat="server">
                        </tbody>
                </table>
            </div>
        </div>
    </section>

    <div class="modal fade" id="modal_Usuario" tabindex="-1" aria-labelledby="tituloModal" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="tituloModal">Registrar / Editar Usuario</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span>×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <input type="hidden" id="txtIdUsuario">

                        <div class="col-md-6 mb-3">
                            <label for="ddlTipoUsuario" class="form-label">Tipo de Usuario</label>
                            <asp:DropDownList ID="ddlTipoUsuario" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                        </div>
                        
                        <div class="col-md-6 mb-3">
                            <label for="txtNombresCompletos" class="form-label">Nombres Completos</label>
                            <input type="text" class="form-control" id="txtNombresCompletos" placeholder="Ingrese los nombres completos">
                        </div>
                        
                        <div class="col-md-6 mb-3">
                            <label for="txtNombreUsuario" class="form-label">Nombre de Usuario</label>
                            <input type="text" class="form-control" id="txtNombreUsuario" placeholder="Ingrese el nombre de usuario">
                        </div>

                        <div class="col-md-6 mb-3">
                            <label for="txtContrasena" class="form-label">Contraseña</label>
                            <input type="password" class="form-control" id="txtContrasena" placeholder="Dejar en blanco para no cambiar">
                        </div>

                        <div class="col-md-12 mb-3">
                            <label for="txtPreguntaSecreta" class="form-label">Pregunta Secreta</label>
                            <input type="text" class="form-control" id="txtPreguntaSecreta" placeholder="Ej: ¿Nombre de mi primera mascota?">
                        </div>
                        
                        <div class="col-md-12 mb-3">
                            <label for="txtRespuesta" class="form-label">Respuesta a la Pregunta</label>
                            <input type="text" class="form-control" id="txtRespuesta" placeholder="Ingrese la respuesta secreta">
                        </div>

                        <div class="col-md-12 form-check form-switch ml-2">
                            <input class="form-check-input" type="checkbox" id="chkVigencia" checked>
                            <label class="form-check-label" for="chkVigencia">Vigente</label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" id="btnGuardar" onclick="fct_GuardarUsuario()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

<script>
    function fct_AbrirModalNuevoUsuario() {
        $('#tituloModal').text('Nuevo Usuario');
        $('#txt').val('0'); // Se enviará 0 para nuevos registros
        $('#ddlTipoUsuario').val('1'); // Seleccionar el primer tipo por defecto
        $('#txtNombresCompletos').val('');
        $('#txtNombreUsuario').val('');
        $('#txtContrasena').val('');
        $('#txtContrasena').attr('placeholder', 'Ingrese la contraseña');
        $('#txtPreguntaSecreta').val('');
        $('#txtRespuesta').val('');
        $('#chkVigencia').prop('checked', true);
        $('#btnGuardar').text('Guardar');
        $('#modal_Usuario').modal('show');
    }

    // Nota: Los parámetros de esta función deben coincidir con los datos que envías desde el backend en el botón "Editar"
    function fct_EditarUsuario(id, idTipoUsuario, nombresCompletos, nombreUsuario, preguntaSecreta, respuesta, estado) {
        $('#tituloModal').text('Editar Usuario');

        // Llenamos los campos del modal
        $("#txtIdUsuario").val(id);
        $("#<%= ddlTipoUsuario.ClientID %>").val(idTipoUsuario); // Se usa ClientID por ser control de ASP.NET
        $("#txtNombresCompletos").val(nombresCompletos);
        $("#txtNombreUsuario").val(nombreUsuario);
        $("#txtPreguntaSecreta").val(preguntaSecreta);
        $("#txtRespuesta").val(respuesta);
        $('#txtContrasena').attr('placeholder', 'Dejar en blanco para no cambiar');

        // Convertimos a booleano
        const estadoBool = (estado === true || estado.toLowerCase() === "true");
        $("#chkVigencia").prop('checked', estadoBool);

        $("#btnGuardar").text("Actualizar");
        $('#modal_Usuario').modal('show');
    }

    function fct_GuardarUsuario() {
        let id = $('#txtIdUsuario').val() || "0";
        let idTipoUsuario = $("#<%= ddlTipoUsuario.ClientID %>").val();
        let nombresCompletos = $('#txtNombresCompletos').val().trim();
        let nombreUsuario = $('#txtNombreUsuario').val().trim();
        let contrasena = $('#txtContrasena').val(); // No usar trim() en contraseñas
        let preguntaSecreta = $('#txtPreguntaSecreta').val().trim();
        let respuesta = $('#txtRespuesta').val().trim();
        let vigente = $('#chkVigencia').is(':checked');

        if (nombresCompletos === '' || nombreUsuario === '' || (id === '0' && contrasena === '')) {
            toastr.warning('Nombres, Usuario y Contraseña son obligatorios para nuevos registros.');
            return;
        }

        let usuario = {
            id: parseInt(id),
            idTipoUsuario: parseInt(idTipoUsuario),
            nombresCompletos: nombresCompletos,
            nombre: nombreUsuario,
            contrasena: contrasena,
            preguntaSecreta: preguntaSecreta,
            respuesta: respuesta,
            vigente: vigente
        };

        $.ajax({
            url: 'wfUsuario.aspx/GuardarUsuario',
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ oUsuario: usuario }), // Se envía el objeto completo
            success: function (response) {
                if (response.d === 'success') {
                    toastr.success('Guardado correctamente');
                    setTimeout(() => location.reload(), 800);
                } else {
                    toastr.error(response.d);
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
                toastr.error('Error en la solicitud al servidor.');
            }
        });
    }

    function fct_EliminarUsuario(id) {
        Swal.fire({
            title: "¿Deseas eliminar este usuario?",
            text: "El registro se eliminará permanentemente.",
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
                    url: 'wfUsuario.aspx/EliminarUsuario',
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

    function fct_DarBajaUsuario(id) {
        Swal.fire({
            title: "¿Dar de baja a este usuario?",
            text: "El usuario no podrá iniciar sesión.",
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
                    url: 'wfUsuario.aspx/DarBajaUsuario',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ id: id }),
                    success: function (response) {
                        if (response.d === 'success') {
                            toastr.success('Usuario dado de baja correctamente');
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