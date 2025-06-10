Imports System.Web.Services
Imports System.Text
Imports System.Data
Imports libNegocio ' Asegúrate que este sea el nombre de tu proyecto de capa lógica

Public Class wfTipoProducto
    Inherits System.Web.UI.Page

    Private objTipoProducto As New clsTipoProducto()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ListarTiposProducto()
        End If
    End Sub

    Private Sub ListarTiposProducto()
        Try
            Dim dt As DataTable = objTipoProducto.listarTiposProducto()
            Dim sb As New StringBuilder()

            For Each row As DataRow In dt.Rows
                Dim id As Integer = CInt(row("idTipo"))
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
                sb.Append($"<button type='button' class='btn btn-primary btn-sm' onclick=""fct_EditarTipoProducto({id}, '{nombre.Replace("'", "\'")}', '{descripcion.Replace("'", "\'")}', '{estado}')""><i class='fas fa-edit'></i></button> ")
                sb.Append($"<button type='button' class='btn btn-danger btn-sm' onclick='fct_EliminarTipoProducto({id})'><i class='fas fa-trash'></i></button> ")

                If esVigente Then
                    sb.Append($"<button type='button' class='btn btn-warning btn-sm' onclick='fct_DarBajaTipoProducto({id})'><i class='fas fa-arrow-down'></i></button>")
                Else
                    sb.Append($"<button type='button' class='btn btn-info btn-sm' onclick='fct_DarAltaTipoProducto({id})'><i class='fas fa-arrow-up'></i></button>")
                End If
                sb.Append("</td>")
                sb.Append("</tr>")
            Next

            tbody_TipoProducto.InnerHtml = sb.ToString()

        Catch ex As Exception
            tbody_TipoProducto.InnerHtml = $"<tr><td colspan='5'>Error al listar los tipos de producto: {ex.Message}</td></tr>"
        End Try
    End Sub

    ' --- WEBMETHODS ---

    <WebMethod>
    Public Shared Function GuardarTipoProducto(id As Integer, nombre As String, descripcion As String, vigente As Boolean) As String
        Dim objTP As New clsTipoProducto()
        Try
            If id = 0 Then
                Dim newId As Integer = objTP.generarIDTipoProducto()
                objTP.guardarTipoProducto(newId, nombre, descripcion, vigente)
            Else
                objTP.modificarTipoProducto(id, nombre, descripcion, vigente)
            End If
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function EliminarTipoProducto(id As Integer) As String
        Dim objTP As New clsTipoProducto()
        Try
            ' NOTA: La eliminación fallará si hay productos que usan este tipo (debido a restricciones de clave foránea en la BD).
            ' Esto es un comportamiento deseado para mantener la integridad de los datos.
            objTP.eliminarTipoProducto(id)
            Return "success"
        Catch ex As Exception
            If ex.Message.Contains("FK_") Or ex.Message.Contains("conflicto") Then
                Return "No se puede eliminar: El tipo está siendo utilizado por uno o más productos."
            End If
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarBajaTipoProducto(id As Integer) As String
        Dim objTP As New clsTipoProducto()
        Try
            objTP.darBajaTipoProducto(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarAltaTipoProducto(id As Integer) As String
        Dim objTP As New clsTipoProducto()
        Try
            objTP.darAltaTipoProducto(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

End Class