Imports System.Web.Services
Imports System.Text
Imports System.Data
Imports appWebSistemaRestaurante.srMesero ' Asegúrate que este sea el nombre de tu proyecto de capa lógica

Public Class wfMesero
    Inherits System.Web.UI.Page

    Private objMesero As New wsMeseroSoapClient()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ListarMeseros()
        End If
    End Sub

    Private Sub ListarMeseros()
        Try
            Dim dt As DataTable = objMesero.listarMesero()
            Dim sb As New StringBuilder()

            For Each row As DataRow In dt.Rows
                Dim id As Integer = CInt(row("idMesero"))
                Dim dni As String = row("dniMesero").ToString()
                Dim nombres As String = row("nombres").ToString()
                Dim apellidos As String = row("apellidos").ToString()
                Dim sexo As String = row("sexo").ToString()
                ' Formatear la fecha para que sea compatible con el input type="date"
                Dim fechaNac As String = Convert.ToDateTime(row("fechaNac")).ToString("yyyy-MM-dd")
                Dim direccion As String = row("direccion").ToString()
                Dim telefono As String = row("telefono").ToString()
                Dim correo As String = row("correo").ToString()
                Dim estado As String = row("estado").ToString()
                Dim esActivo As Boolean = (estado.ToLower() = "activo")

                sb.Append("<tr>")
                sb.Append($"<td>{id}</td>")
                sb.Append($"<td>{dni}</td>")
                sb.Append($"<td>{nombres} {apellidos}</td>")
                sb.Append($"<td>{telefono}</td>")
                sb.Append($"<td>{correo}</td>")

                If esActivo Then
                    sb.Append("<td><span class='badge badge-success'>Activo</span></td>")
                Else
                    sb.Append("<td><span class='badge badge-danger'>Inactivo</span></td>")
                End If

                ' Botones de acciones
                sb.Append("<td>")
                ' Escapamos las comillas simples para los parámetros de la función JS
                Dim p_nombres As String = nombres.Replace("'", "\'")
                Dim p_apellidos As String = apellidos.Replace("'", "\'")
                Dim p_direccion As String = direccion.Replace("'", "\'")
                Dim p_correo As String = correo.Replace("'", "\'")

                sb.Append($"<button type='button' class='btn btn-primary btn-sm' onclick=""fct_EditarMesero({id}, '{dni}', '{p_nombres}', '{p_apellidos}', '{sexo}', '{fechaNac}', '{p_direccion}', '{telefono}', '{p_correo}', '{estado}')""><i class='fas fa-edit'></i></button> ")
                sb.Append($"<button type='button' class='btn btn-danger btn-sm' onclick='fct_EliminarMesero({id})'><i class='fas fa-trash'></i></button> ")

                If esActivo Then
                    sb.Append($"<button type='button' class='btn btn-warning btn-sm' onclick='fct_DarBajaMesero({id})'><i class='fas fa-arrow-down'></i></button>")
                Else
                    sb.Append($"<button type='button' class='btn btn-info btn-sm' onclick='fct_DarAltaMesero({id})'><i class='fas fa-arrow-up'></i></button>")
                End If
                sb.Append("</td>")
                sb.Append("</tr>")
            Next

            tbody_Mesero.InnerHtml = sb.ToString()

        Catch ex As Exception
            tbody_Mesero.InnerHtml = $"<tr><td colspan='7'>Error al listar los meseros: {ex.Message}</td></tr>"
        End Try
    End Sub

    ' --- WEBMETHODS ---

    <WebMethod>
    Public Shared Function GuardarMesero(id As Integer, dni As String, nom As String, ape As String, sex As String, fec As String, dir As String, tel As String, cor As String, est As Boolean) As String
        Dim objM As New wsMeseroSoapClient()
        Try
            ' Convertir la fecha de string a Date
            Dim fechaNacDate As Date = Date.Parse(fec)

            If id = 0 Then
                Dim newId As Integer = objM.generarIDMesero()
                objM.guardarMesero(newId, dni, nom, ape, sex, fechaNacDate, dir, tel, cor, est)
            Else
                objM.modificarMesero(id, dni, nom, ape, sex, fechaNacDate, dir, tel, cor, est)
            End If
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function EliminarMesero(id As Integer) As String
        Dim objM As New wsMeseroSoapClient()
        Try
            objM.eliminarMesero(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarBajaMesero(id As Integer) As String
        Dim objM As New wsMeseroSoapClient()
        Try
            objM.darBajaMesero(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    ' Nota: Tu clase clsMesero no tenía este método, pero lo agrego para funcionalidad completa.
    ' Debes agregar el método 'darAltaMesero' a tu clase clsMesero.
    <WebMethod>
    Public Shared Function DarAltaMesero(id As Integer) As String
        Dim objM As New wsMeseroSoapClient()
        Try
            ' Ahora llamamos directamente al método de la clase de negocio
            objM.darAltaMesero(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

End Class