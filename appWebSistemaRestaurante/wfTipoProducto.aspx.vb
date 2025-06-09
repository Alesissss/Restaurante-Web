' wfTipoProducto.aspx.vb
Imports libNegocio

Public Class wfTipoProducto
    Inherits System.Web.UI.Page

    Dim objTipoProducto As New clsTipoProducto

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTabla()
        End If
    End Sub

    Private Sub CargarTabla()
        Dim lista As DataTable = objTipoProducto.listarTiposProducto()
        Dim html As New StringBuilder()

        For Each fila As DataRow In lista.Rows
            Dim id As String = fila("idTipo").ToString()
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
            html.AppendFormat("<button class='btn btn-primary btn-sm' onclick=""fct_EditarTipoProducto('{0}', '{1}', '{2}', {3})""><i class='fas fa-edit'></i></button> ", id, nombre, descripcion.Replace("'", "\'"), estadoBool)
            html.AppendFormat("<button class='btn btn-warning btn-sm' onclick=""fct_DarBajaTipoProducto('{0}')""><i class='fas fa-arrow-down'></i></button> ", id)
            html.AppendFormat("<button class='btn btn-danger btn-sm' onclick=""fct_EliminarTipoProducto('{0}')""><i class='fas fa-trash'></i></button>", id)
            html.Append("</td>")
            html.Append("</tr>")
        Next

        tbody_TipoProducto.InnerHtml = html.ToString()
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarTipoProducto(id As String, nombre As String, descripcion As String, vigente As Boolean) As String
        Try
            Dim obj As New clsTipoProducto
            If id = "" Then
                obj.guardarTipoProducto(obj.generarIDTipoProducto(), nombre, descripcion, vigente)
            Else
                obj.modificarTipoProducto(id, nombre, descripcion, vigente)
            End If
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarTipoProducto(id As String) As String
        Try
            Dim obj As New clsTipoProducto
            obj.eliminarTipoProducto(id)
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function DarBajaTipoProducto(id As String) As String
        Try
            Dim obj As New clsTipoProducto
            obj.darBajaTipoProducto(id)
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerTipoProducto(id As String) As Dictionary(Of String, Object)
        Dim obj As New clsTipoProducto
        Dim dt As DataTable = obj.buscarTipoProducto(id)

        If dt.Rows.Count > 0 Then
            Dim row = dt.Rows(0)
            Return New Dictionary(Of String, Object) From {
                {"idTipo", row("idTipo").ToString()},
                {"Nombre", row("nombre").ToString()},
                {"Descripcion", row("descripcion").ToString()},
                {"Vigencia", CBool(row("vigencia"))}
            }
        Else
            Throw New Exception("Tipo de producto no encontrado.")
        End If
    End Function

End Class
