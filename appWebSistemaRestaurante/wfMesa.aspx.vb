' Archivo: wfMesa.aspx.vb
Imports System.Web.Services
Imports System.Text
Imports System.Data
Imports libNegocio

Public Class wfMesa
    Inherits System.Web.UI.Page

    Private objMesa As New clsMesa()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ListarMesas()
        End If
    End Sub

    Private Sub ListarMesas()
        Try
            Dim dt As DataTable = objMesa.listarMesas()
            Dim sb As New StringBuilder()

            If dt.Rows.Count = 0 Then
                sb.Append("<div class='col-12'><p class='text-center'>No hay mesas registradas.</p></div>")
            Else
                For Each row As DataRow In dt.Rows
                    Dim id As Integer = CInt(row("idMesa"))
                    Dim numero As Integer = CInt(row("numero"))
                    Dim capacidad As Byte = CByte(row("capacidad"))
                    Dim esVigente As Boolean = CBool(row("vigencia"))
                    Dim estaDisponible As Boolean = CBool(row("estadoMesa"))

                    Dim cardClass As String = If(esVigente, "border-primary", "border-secondary")
                    Dim iconClass As String = If(esVigente, "text-primary", "text-secondary")

                    sb.Append("<div class='col-lg-3 col-md-4 col-sm-6 mb-4'>")
                    sb.Append($"<div class='card card-mesa text-center {cardClass}'>")
                    sb.Append("<div class='card-body'>")
                    sb.Append($"<i class='fas fa-chair icono-mesa {iconClass}'></i>")
                    sb.Append($"<h5 class='card-title numero-mesa mt-2'>Mesa #{numero}</h5>")
                    sb.Append($"<p class='card-text capacidad-mesa mb-2'>Capacidad: {capacidad} personas</p>")

                    If esVigente Then
                        Dim estadoBadgeClass As String = If(estaDisponible, "success", "danger")
                        Dim estadoTexto As String = If(estaDisponible, "Disponible", "Ocupada")
                        sb.Append($"<span class='badge badge-pill badge-{estadoBadgeClass}'>{estadoTexto}</span>")
                    Else
                        sb.Append($"<span class='badge badge-pill badge-secondary'>Mantenimiento</span>")
                    End If

                    sb.Append("</div>")
                    sb.Append("<div class='card-footer'>")
                    sb.Append($"<button type='button' class='btn btn-sm btn-primary' onclick=""fct_EditarMesa({id}, {numero}, {capacidad}, {esVigente.ToString().ToLower()})""><i class='fas fa-edit'></i></button> ")
                    sb.Append($"<button type='button' class='btn btn-sm btn-{(If(esVigente, "warning", "info"))}' onclick=""fct_CambiarVigenciaMesa({id}, {(Not esVigente).ToString().ToLower()})""><i class='fas fa-toggle-on'></i></button> ")
                    sb.Append($"<button type='button' class='btn btn-sm btn-danger' onclick=""fct_EliminarMesa({id})""><i class='fas fa-trash'></i></button>")
                    sb.Append("</div>")
                    sb.Append("</div>")
                    sb.Append("</div>")
                Next
            End If

            cardContainer_Mesa.InnerHtml = sb.ToString()

        Catch ex As Exception
            cardContainer_Mesa.InnerHtml = $"<div class='col-12'><div class='alert alert-danger'>Error al listar mesas: {ex.Message}</div></div>"
        End Try
    End Sub

    ' --- WEBMETHODS ---

    <WebMethod>
    Public Shared Function GuardarMesa(id As Integer, numero As Integer, capacidad As Byte, vigencia As Boolean) As String
        Dim objM As New clsMesa()
        Try
            Dim dtExistente As DataTable = objM.listarMesas()
            For Each row As DataRow In dtExistente.Rows
                If CInt(row("numero")) = numero AndAlso CInt(row("idMesa")) <> id Then
                    Return "El número de mesa ya existe."
                End If
            Next

            If id = 0 Then
                Dim newId As Integer = objM.generarIDMesa()
                objM.guardarMesa(newId, numero, capacidad, vigencia)
            Else
                objM.modificarMesa(id, numero, capacidad, vigencia)
            End If
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function EliminarMesa(id As Integer) As String
        Dim objM As New clsMesa()
        Try
            objM.eliminarMesa(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function CambiarVigenciaMesa(id As Integer, nuevaVigencia As Boolean) As String
        Dim objM As New clsMesa()
        Try
            If nuevaVigencia Then
                objM.darAltaMesa(id)
            Else
                objM.darBajaMesa(id)
            End If
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

End Class