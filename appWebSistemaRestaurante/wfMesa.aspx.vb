Imports libNegocio

Public Class wfMesa
    Inherits System.Web.UI.Page

    Dim objMesa As New clsMesa

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then CargarTabla()
    End Sub

    Private Sub CargarTabla()
        Dim lista As DataTable = objMesa.listarMesas()
        Dim html As New StringBuilder()

        For Each fila As DataRow In lista.Rows
            Dim id = fila("idMesa").ToString()
            Dim numero = fila("numero").ToString()
            Dim capacidad = fila("capacidad").ToString()
            Dim estadoTexto = fila("estado").ToString()
            Dim estadoBool = If(estadoTexto = "Activo", "true", "false")

            html.Append("<tr>")
            html.AppendFormat("<td>{0}</td>", id)
            html.AppendFormat("<td>{0}</td>", numero)
            html.AppendFormat("<td>{0}</td>", capacidad)
            html.AppendFormat("<td>{0}</td>", estadoTexto)
            html.Append("<td>")
            html.AppendFormat("<button class='btn btn-primary btn-sm' onclick=""fct_EditarMesa('{0}', '{1}', '{2}', {3})""><i class='fas fa-edit'></i></button> ", id, numero, capacidad, estadoBool)
            html.AppendFormat("<button class='btn btn-warning btn-sm' onclick=""fct_DarBajaMesa('{0}')""><i class='fas fa-arrow-down'></i></button> ", id)
            html.AppendFormat("<button class='btn btn-danger btn-sm' onclick=""fct_EliminarMesa('{0}')""><i class='fas fa-trash'></i></button>", id)
            html.Append("</td>")
            html.Append("</tr>")
        Next

        tbody_Mesa.InnerHtml = html.ToString()
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarMesa(id As String, numero As String, capacidad As String, estado As Boolean) As String
        Try
            Dim obj As New clsMesa
            If id = "" Then
                obj.guardarMesa(obj.generarIDMesa(), CInt(numero), CByte(capacidad), estado)
            Else
                obj.modificarMesa(CInt(id), CInt(numero), CByte(capacidad), estado)
            End If
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarMesa(id As String) As String
        Try
            Dim obj As New clsMesa
            obj.eliminarMesa(CInt(id))
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function DarBajaMesa(id As String) As String
        Try
            Dim obj As New clsMesa
            obj.darBajaMesa(CInt(id))
            Return "success"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function
End Class
