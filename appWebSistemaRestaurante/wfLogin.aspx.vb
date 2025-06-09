Imports libNegocio

Public Class wfLogin
    Inherits System.Web.UI.Page
    Dim objUsuario As New clsUsuario
    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Try
            If objUsuario.iniciarSesion(txtUsuario.Text, txtContraseña.Text) Then
                ' ✅ Guardamos el usuario en sesión
                Session("usuario") = txtUsuario.Text.Trim()

                ' Redirigimos al inicio
                Response.Redirect("wfInicio.aspx")
            Else
                lblMensaje.Text = "Datos incorrectos, intente nuevamente!"
            End If
        Catch ex As Exception
            lblMensaje.Text = ex.Message
        End Try
    End Sub
End Class