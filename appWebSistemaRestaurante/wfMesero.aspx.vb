Imports libNegocio

Public Class wfMesero
    Inherits System.Web.UI.Page

    Dim objMesero As New clsMesero

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTabla()
        End If
    End Sub

    Private Sub CargarTabla()
        Dim lista As DataTable = objMesero.listarMesero()
        Dim html As New StringBuilder()

        For Each f As DataRow In lista.Rows
            html.Append("<tr>")
            html.AppendFormat("<td>{0}</td>", f("idMesero"))
            html.AppendFormat("<td>{0}</td>", f("dniMesero"))
            html.AppendFormat("<td>{0}</td>", f("nombres"))
            html.AppendFormat("<td>{0}</td>", f("apellidos"))
            html.AppendFormat("<td>{0}</td>", f("sexo"))
            html.AppendFormat("<td>{0:yyyy-MM-dd}</td>", f("fechaNac"))
            html.AppendFormat("<td>{0}</td>", f("telefono"))
            html.AppendFormat("<td>{0}</td>", f("correo"))
            html.AppendFormat("<td>{0}</td>", f("estado"))
            html.AppendFormat("<td><button class='btn btn-primary btn-sm' onclick=""fct_EditarMesero('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')""><i class='fas fa-edit'></i></button> ", f("idMesero"), f("dniMesero"), f("nombres").ToString().Replace("'", "\'"), f("apellidos").ToString().Replace("'", "\'"), f("sexo"), CDate(f("fechaNac")).ToString("yyyy-MM-dd"), f("direccion").ToString().Replace("'", "\'"), f("telefono"), f("correo"), f("estado"))
            html.AppendFormat("<button class='btn btn-warning btn-sm' onclick=""fct_DarBajaMesero('{0}')""><i class='fas fa-arrow-down'></i></button> ", f("idMesero"))
            html.AppendFormat("<button class='btn btn-danger btn-sm' onclick=""fct_EliminarMesero('{0}')""><i class='fas fa-trash'></i></button></td>", f("idMesero"))
            html.Append("</tr>")
        Next

        tbody_Mesero.InnerHtml = html.ToString()
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarMesero(id As String, dni As String, nom As String, ape As String, sex As String, fec As String, dir As String, tel As String, cor As String, est As Boolean) As String
        Dim obj As New clsMesero
        Try
            If id = "" Then
                obj.guardarMesero(obj.generarIDMesero(), dni, nom, ape, sex, Date.Parse(fec), dir, tel, cor, est)
            Else
                obj.modificarMesero(CInt(id), dni, nom, ape, sex, Date.Parse(fec), dir, tel, cor, est)
            End If
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarMesero(id As String) As String
        Dim obj As New clsMesero
        Try
            obj.eliminarMesero(CInt(id))
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function DarBajaMesero(id As String) As String
        Dim obj As New clsMesero
        Try
            obj.darBajaMesero(CInt(id))
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function
End Class
