Imports System.Web.Services
Imports System.Text
Imports System.Data
Imports libNegocio ' Asegúrate que este sea el nombre de tu proyecto de capa lógica

Public Class wfTipoUsuario
    Inherits System.Web.UI.Page

    Private objTipoUsuario As New clsTipoUsuario()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ListarTiposUsuario()
        End If
    End Sub

    Private Sub ListarTiposUsuario()
        Try
            Dim dt As DataTable = objTipoUsuario.listarTiposUsuario()
            Dim sb As New StringBuilder()

            For Each row As DataRow In dt.Rows
                Dim id As Integer = CInt(row("idTipoUsuario"))
                Dim nombre As String = row("nombre").ToString()
                Dim descripcion As String = row("descripcion").ToString()
                Dim estado As String = row("estado").ToString()
                Dim esVigente As Boolean = (estado.ToLower() = "activo")

                sb.Append("<tr>")
                sb.Append($"<td>{id}</td>")
                sb.Append($"<td>{nombre}</td>")
                sb.Append($"<td>{descripcion}</td>")

                If esVigente Then
                    sb.Append("<td><span class='badge badge-success'>Activo</span></td>")
                Else
                    sb.Append("<td><span class='badge badge-danger'>Inactivo</span></td>")
                End If

                sb.Append("<td>")
                sb.Append($"<button type='button' class='btn btn-primary btn-sm' onclick=""fct_EditarTipoUsuario({id}, '{nombre.Replace("'", "\'")}', '{descripcion.Replace("'", "\'")}', '{estado}')""><i class='fas fa-edit'></i></button> ")
                sb.Append($"<button type='button' class='btn btn-danger btn-sm' onclick='fct_EliminarTipoUsuario({id})'><i class='fas fa-trash'></i></button> ")

                If esVigente Then
                    sb.Append($"<button type='button' class='btn btn-warning btn-sm' onclick='fct_DarBajaTipoUsuario({id})'><i class='fas fa-arrow-down'></i></button>")
                Else
                    sb.Append($"<button type='button' class='btn btn-info btn-sm' onclick='fct_DarAltaTipoUsuario({id})'><i class='fas fa-arrow-up'></i></button>")
                End If
                sb.Append("</td>")
                sb.Append("</tr>")
            Next

            tbody_TipoUsuario.InnerHtml = sb.ToString()

        Catch ex As Exception
            tbody_TipoUsuario.InnerHtml = $"<tr><td colspan='5'>Error al listar los tipos de usuario: {ex.Message}</td></tr>"
        End Try
    End Sub

    ' --- WEBMETHODS ---

    <WebMethod>
    Public Shared Function GuardarTipoUsuario(id As Integer, nombre As String, descripcion As String, vigente As Boolean) As String
        Dim objTU As New clsTipoUsuario()
        Try
            If id = 0 Then
                Dim newId As Integer = objTU.generarIDTipoUsuario()
                objTU.guardarTipoUsuario(newId, nombre, descripcion, vigente)
            Else
                objTU.modificarTipoUsuario(id, nombre, descripcion, vigente)
            End If
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function EliminarTipoUsuario(id As Integer) As String
        Dim objTU As New clsTipoUsuario()
        Try
            ' NOTA: La eliminación puede fallar si hay usuarios que usan este tipo (restricción de clave foránea).
            objTU.eliminarTipoUsuario(id)
            Return "success"
        Catch ex As Exception
            If ex.Message.ToLower().Contains("foreign key") Or ex.Message.ToLower().Contains("conflicto") Then
                Return "No se puede eliminar: El tipo está siendo utilizado por uno o más usuarios."
            End If
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarBajaTipoUsuario(id As Integer) As String
        Dim objTU As New clsTipoUsuario()
        Try
            objTU.darBajaTipoUsuario(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarAltaTipoUsuario(id As Integer) As String
        Dim objTU As New clsTipoUsuario()
        Try
            objTU.darAltaTipoUsuario(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

End Class