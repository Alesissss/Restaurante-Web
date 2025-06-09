<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogin.aspx.vb" Inherits="appWebSistemaRestaurante.wfLogin" %>

<!DOCTYPE html>
<html lang="es">

<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Iniciar Sesión</title>
    <link rel="icon" type="image/x-icon" href="/static/img/dashboard.ico">

     <!-- Favicon -->
    <link rel="icon" type="image/x-icon" href="">

    <!-- AdminLTE -->
    <link rel="stylesheet" href="/utilities/css/adminlte.min.css">
    <link rel="stylesheet" href="/utilities/css/all.min.css">

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css">

    <!-- SweetAlert2 -->
    <link rel="stylesheet" href="/utilities/plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css">
    <!-- Toastr -->
    <link rel="stylesheet" href="/utilities/plugins/toastr/toastr.min.css">
    <!-- Select2 -->
    <link rel="stylesheet" href="/utilities/plugins/select2/css/select2.min.css">
    <link rel="stylesheet" href="/utilities/plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css">
    <!-- Tempusdominus Bootstrap 4 -->
    <link rel="stylesheet" href="/utilities/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css">
    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700">
</head>

<body class="hold-transition login-page">
    <form id="form1" runat="server">
        <div class="login-box">
            <div class="login-logo">
                <b>Iniciar</b> sesión
            </div>
            <div class="card">
                <div class="card-body login-card-body">
                    <p class="login-box-msg">Ingresa tus credenciales</p>

                    <div class="input-group mb-3">
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Usuario"></asp:TextBox>
                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-user"></span>
                            </div>
                        </div>
                    </div>

                    <div class="input-group mb-3">
                        <asp:TextBox ID="txtContraseña" runat="server" CssClass="form-control" TextMode="Password" placeholder="Contraseña"></asp:TextBox>
                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-lock"></span>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-primary btn-block" Text="Ingresar" />
                        </div>
                    </div>

                    <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger small d-block mt-2"></asp:Label>
                </div>
            </div>
        </div>
    </form>

    <!-- jQuery -->
    <script src="/utilities/js/jquery.min.js"></script>
    <!-- Bootstrap JS -->
    <script src="/utilities/js/bootstrap.bundle.min.js"></script>
    <!-- AdminLTE JS -->
    <script src="/utilities/js/adminlte.min.js"></script>

    <!-- Moment.js (para formateo de fechas) -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.4/moment.min.js"></script>

    <!-- SweetAlert2 -->
    <script src="/utilities/plugins/sweetalert2/sweetalert2.min.js"></script>
    <!-- Toastr -->
    <script src="/utilities/plugins/toastr/toastr.min.js"></script>
    <!-- Select2 -->
    <script src="/utilities/plugins/select2/js/select2.full.min.js"></script>
    <!-- InputMask -->
    <script src="/utilities/plugins/inputmask/jquery.inputmask.min.js"></script>
    <!-- Tempusdominus Bootstrap 4 -->
    <script src="/utilities/plugins/moment/moment.min.js"></script>
    <script src="/utilities/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>

    <!-- Datatables -->
    <link rel="stylesheet" href="/utilities/plugins/datatables-test/datatables.min.css">
    <script src="/utilities/plugins/datatables-test/datatables.min.js"></script>
</body>

</html>
