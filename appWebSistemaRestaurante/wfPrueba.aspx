<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPrueba.aspx.vb" Inherits="appWebSistemaRestaurante.wfPrueba" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            LOGIN DE PRUEBA<br />
            <br />
            <br />
            Usuario:&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
            <br />
            <br />
            Contraseña:
            <asp:TextBox ID="txtContraseña" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
            <br />
            <br />
            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
