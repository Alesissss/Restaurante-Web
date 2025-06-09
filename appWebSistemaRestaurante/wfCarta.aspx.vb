Imports System.Web.Services
Imports System.Text
Imports System.Data
Imports libNegocio ' Asegúrate que este sea el nombre de tu proyecto de capa lógica

Public Class wfCarta
    Inherits System.Web.UI.Page

    Private objCarta As New clsCarta()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ListarCartas()
        End If
    End Sub

    Private Sub ListarCartas()
        Try
            Dim dt As DataTable = objCarta.listarCartas()
            Dim sb As New StringBuilder()

            For Each row As DataRow In dt.Rows
                Dim id As Integer = CInt(row("idCarta"))
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
                sb.Append($"<button type='button' class='btn btn-primary btn-sm' onclick=""fct_EditarCarta({id}, '{nombre.Replace("'", "\'")}', '{descripcion.Replace("'", "\'")}', '{estado}')""><i class='fas fa-edit'></i></button> ")
                sb.Append($"<button type='button' class='btn btn-danger btn-sm' onclick='fct_EliminarCarta({id})'><i class='fas fa-trash'></i></button> ")

                If esVigente Then
                    sb.Append($"<button type='button' class='btn btn-warning btn-sm' onclick='fct_DarBajaCarta({id})'><i class='fas fa-arrow-down'></i></button>")
                Else
                    sb.Append($"<button type='button' class='btn btn-info btn-sm' onclick='fct_DarAltaCarta({id})'><i class='fas fa-arrow-up'></i></button>")
                End If
                sb.Append("</td>")
                sb.Append("</tr>")
            Next

            tbody_Carta.InnerHtml = sb.ToString()

        Catch ex As Exception
            tbody_Carta.InnerHtml = $"<tr><td colspan='5'>Error al listar las cartas: {ex.Message}</td></tr>"
        End Try
    End Sub

    ' --- WEBMETHODS ---

    <WebMethod>
    Public Shared Function GuardarCarta(id As Integer, nombre As String, descripcion As String, vigente As Boolean) As String
        Dim objC As New clsCarta()
        Try
            If id = 0 Then
                Dim newId As Integer = objC.generarIDCarta()
                objC.guardarCarta(newId, nombre, descripcion, vigente)
            Else
                objC.modificarCarta(id, nombre, descripcion, vigente)
            End If
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function EliminarCarta(id As Integer) As String
        Dim objC As New clsCarta()
        Try
            ' Aquí podrías agregar una validación para no eliminar cartas en uso si fuera necesario
            objC.eliminarCarta(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarBajaCarta(id As Integer) As String
        Dim objC As New clsCarta()
        Try
            objC.darBajaCarta(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarAltaCarta(id As Integer) As String
        Dim objC As New clsCarta()
        Try
            objC.darAltaCarta(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

End Class