Imports appWebSistemaRestaurante.srUsuario
' LIBRERÍAS AÑADIDAS PARA RECAPTCHA
Imports System.Net
Imports System.IO

Public Class wfLogin
    Inherits System.Web.UI.Page
    Dim objUsuario As New wsUsuarioSoapClient()

    ' FUNCIÓN AÑADIDA PARA VALIDAR RECAPTCHA
    Public Function IsReCaptchaValid() As Boolean
        Dim result = False
        Dim captchaResponse = Request.Form("g-recaptcha-response")
        ' REEMPLAZA CON TU CLAVE SECRETA
        Dim secretKey = "6Lf0h20rAAAAAM3TZIVwijOPgJjcKkKEof8hWRhn"
        Dim apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret=" & secretKey & "&response=" & captchaResponse

        Try
            Using client As New WebClient()
                Dim jsonResult = client.DownloadString(apiUrl)
                ' La respuesta de Google es un texto JSON. Si contiene "success": true, es válido.
                If jsonResult IsNot Nothing AndAlso jsonResult.Contains("""success"": true") Then
                    result = True
                End If
            End Using
        Catch
            ' Si hay un error al contactar a Google, por seguridad se considera inválido.
            Return False
        End Try

        Return result
    End Function

    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Try
            ' PASO 1: VERIFICAR EL CAPTCHA
            If IsReCaptchaValid() Then
                ' PASO 2: SI EL CAPTCHA ES VÁLIDO, CONTINUAR CON EL LOGIN
                If objUsuario.iniciarSesion(txtUsuario.Text, txtContraseña.Text) Then
                    ' ✅ Guardamos el usuario en sesión
                    Session("usuario") = txtUsuario.Text.Trim()

                    ' Redirigimos al inicio
                    Response.Redirect("wfInicio.aspx")
                Else
                    lblMensaje.Text = "Datos incorrectos, intente nuevamente!"
                End If
            Else
                ' SI EL CAPTCHA NO ES VÁLIDO
                lblMensaje.Text = "Por favor, verifica que no eres un robot."
            End If

        Catch ex As Exception
            lblMensaje.Text = ex.Message
        End Try
    End Sub
End Class