Imports System.Web.Services
Imports System.Text
Imports System.Data
Imports appWebSistemaRestaurante.srCajero ' Asegúrate que este sea el nombre de tu proyecto de capa lógica

Public Class wfCajero
    Inherits System.Web.UI.Page

    Private objCajero As New wsCajeroSoapClient()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ListarCajeros()
        End If
    End Sub

    Private Sub ListarCajeros()
        Try
            Dim dt As DataTable = objCajero.listarCajeros()
            Dim sb As New StringBuilder()

            For Each row As DataRow In dt.Rows
                Dim id As Integer = CInt(row("idCajero"))
                Dim dni As String = row("dniCajero").ToString()
                Dim nombres As String = row("nombres").ToString()
                Dim apellidos As String = row("apellidos").ToString()
                Dim telefono As String = row("telefono").ToString()
                Dim correo As String = row("correo").ToString()
                Dim estado As String = row("estado").ToString()
                Dim esActivo As Boolean = (estado.ToLower() = "activo")

                sb.Append("<tr>")
                sb.Append($"<td>{id}</td>")
                sb.Append($"<td>{dni}</td>")
                sb.Append($"<td>{nombres}</td>")
                sb.Append($"<td>{apellidos}</td>")
                sb.Append($"<td>{telefono}</td>")
                sb.Append($"<td>{correo}</td>")

                If esActivo Then
                    sb.Append("<td><span class='badge badge-success'>Activo</span></td>")
                Else
                    sb.Append("<td><span class='badge badge-danger'>Inactivo</span></td>")
                End If

                ' Botones de acciones
                sb.Append("<td>")
                ' Escapamos las comillas simples para evitar errores en JavaScript
                Dim p_nombres As String = nombres.Replace("'", "\'")
                Dim p_apellidos As String = apellidos.Replace("'", "\'")
                Dim p_correo As String = correo.Replace("'", "\'")

                sb.Append($"<button type='button' class='btn btn-primary btn-sm' onclick=""fct_EditarCajero({id}, '{dni}', '{p_nombres}', '{p_apellidos}', '{telefono}', '{p_correo}', '{estado}')""><i class='fas fa-edit'></i></button> ")
                sb.Append($"<button type='button' class='btn btn-danger btn-sm' onclick='fct_EliminarCajero({id})'><i class='fas fa-trash'></i></button> ")

                If esActivo Then
                    sb.Append($"<button type='button' class='btn btn-warning btn-sm' onclick='fct_DarBajaCajero({id})'><i class='fas fa-arrow-down'></i></button>")
                Else
                    sb.Append($"<button type='button' class='btn btn-info btn-sm' onclick='fct_DarAltaCajero({id})'><i class='fas fa-arrow-up'></i></button>")
                End If
                sb.Append("</td>")

                sb.Append("</tr>")
            Next

            tbody_Cajero.InnerHtml = sb.ToString()

        Catch ex As Exception
            tbody_Cajero.InnerHtml = $"<tr><td colspan='8'>Error al listar los cajeros: {ex.Message}</td></tr>"
        End Try
    End Sub

    ' --- WEBMETHODS ---

    <WebMethod>
    Public Shared Function GuardarCajero(id As Integer, dni As String, nombres As String, apellidos As String, telefono As String, correo As String, estado As Boolean) As String
        Dim objC As New wsCajeroSoapClient()
        Try
            ' Validación de DNI duplicado
            Dim dtExistente As DataTable = objC.buscarCajeroPorDNI(dni)
            If dtExistente.Rows.Count > 0 AndAlso CInt(dtExistente.Rows(0)("idCajero")) <> id Then
                Return "Ya existe un cajero con el DNI ingresado."
            End If

            If id = 0 Then
                Dim newId As Integer = objC.generarIDCajero()
                objC.guardarCajero(newId, dni, nombres, apellidos, telefono, correo, estado)
            Else
                objC.modificarCajero(id, dni, nombres, apellidos, telefono, correo, estado)
            End If
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function EliminarCajero(id As Integer) As String
        Dim objC As New wsCajeroSoapClient()
        Try
            objC.eliminarCajero(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarBajaCajero(id As Integer) As String
        Dim objC As New wsCajeroSoapClient()
        Try
            objC.darBajaCajero(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarAltaCajero(id As Integer) As String
        Dim objC As New wsCajeroSoapClient()
        Try
            objC.darAltaCajero(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

End Class