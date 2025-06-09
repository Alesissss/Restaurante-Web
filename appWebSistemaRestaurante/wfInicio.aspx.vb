Public Class wfInicio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Si quieres, puedes validar la sesión aquí también.
        If Session("usuario") Is Nothing Then
            Response.Redirect("wfLogin.aspx")
        End If
    End Sub
End Class
