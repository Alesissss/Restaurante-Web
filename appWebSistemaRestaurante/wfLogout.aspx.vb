Public Class wfLogout
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Session.Clear()
        Session.Abandon()
        Response.Redirect("wfLogin.aspx")
    End Sub

End Class