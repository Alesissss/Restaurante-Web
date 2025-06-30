Imports appWebSistemaRestaurante.srUsuario

Public Class wfPrueba
    Inherits System.Web.UI.Page
    Dim objUsuario As New wsUsuarioSoapClient()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Try
            If objUsuario.iniciarSesion(txtUsuario.Text, txtContraseña.Text) Then
                lblMensaje.Text = "Bienvenido al Sistema, yeeee!!!"
            Else
                lblMensaje.Text = "Datos incorrectos!"
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class