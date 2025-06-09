Imports libNegocio

Public Class wfTipoUsuario
    Inherits System.Web.UI.Page

    Dim objTipoUsuario As New clsTipoUsuario

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTabla()
        End If
    End Sub

    Private Sub CargarTabla()
        Dim lista As DataTable = objTipoUsuario.listarTiposUsuario()
        Dim html As New StringBuilder()

        For Each fila As DataRow In lista.Rows
            Dim id As String = fila("idTipoUsuario").ToString()
            Dim nombre As String = fila("nombre").ToString().Replace("'", "\'")
            Dim descripcion As String = fila("descripcion").ToString().Replace("'", "\'")
            Dim estadoTexto As String = fila("estado").ToString()
            Dim estadoBool As String = If(estadoTexto = "Activo", "true", "false")

            html.Append("<tr>")
            html.AppendFormat("<td>{0}</td>", id)
            html.AppendFormat("<td>{0}</td>", nombre)
            html.AppendFormat("<td>{0}</td>", descripcion)
            html.AppendFormat("<td>{0}</td>", estadoTexto)
            html.Append("<td>")
            html.AppendFormat("<button class='btn btn-primary btn-sm' onclick=""fct_EditarTipoUsuario('{0}', '{1}', '{2}', {3})""><i class='fas fa-edit'></i></button> ", id, nombre, descripcion, estadoBool)
            html.AppendFormat("<button class='btn btn-warning btn-sm' onclick=""fct_DarBajaTipoUsuario('{0}')""><i class='fas fa-arrow-down'></i></button> ", id)
            html.AppendFormat("<button class='btn btn-danger btn-sm' onclick=""fct_EliminarTipoUsuario('{0}')""><i class='fas fa-trash'></i></button>", id)
            html.Append("</td>")
            html.Append("</tr>")
        Next

        tbody_TipoUsuario.InnerHtml = html.ToString()
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarTipoUsuario(id As String, nombre As String, descripcion As String, vigente As Boolean) As String
        Try
            Dim obj As New clsTipoUsuario
            If id = "" Then
                obj.guardarTipoUsuario(obj.generarIDTipoUsuario(), nombre, descripcion, vigente)
            Else
                obj.modificarTipoUsuario(id, nombre, descripcion, vigente)
            End If
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarTipoUsuario(id As String) As String
        Try
            Dim obj As New clsTipoUsuario
            obj.eliminarTipoUsuario(id)
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function DarBajaTipoUsuario(id As String) As String
        Try
            Dim obj As New clsTipoUsuario
            obj.darBajaTipoUsuario(id)
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerTipoUsuario(id As String) As Dictionary(Of String, Object)
        Dim obj As New clsTipoUsuario
        Dim dt As DataTable = obj.buscarTipoUsuario(id)

        If dt.Rows.Count > 0 Then
            Dim row = dt.Rows(0)
            Return New Dictionary(Of String, Object) From {
            {"idTipoUsuario", row("idTipoUsuario").ToString()},
            {"Nombre", row("Nombre").ToString()},
            {"Descripcion", row("Descripcion").ToString()},
            {"Vigencia", CBool(row("Vigencia"))}
        }
        Else
            Throw New Exception("Tipo usuario no encontrado.")
        End If
    End Function

End Class
