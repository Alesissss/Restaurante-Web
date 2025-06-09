Imports System.Web.Services
Imports System.Text
Imports System.Data
Imports libNegocio ' Asegúrate que este sea el nombre de tu proyecto de capa lógica

Public Class wfUsuario
    Inherits System.Web.UI.Page

    ' Instancias de las clases de negocio
    Private objUsuario As New clsUsuario()
    Private objTipoUsuario As New clsTipoUsuario()

    ' --- EVENTOS DE LA PÁGINA ---

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Estas funciones se ejecutan solo la primera vez que carga la página
            CargarTiposUsuario()
            ListarUsuarios()
        End If
    End Sub

    ' --- MÉTODOS PARA CARGAR DATOS EN LA PÁGINA ---

    Private Sub CargarTiposUsuario()
        Try
            ' Llena el DropDownList del modal con los tipos de usuario activos
            ddlTipoUsuario.DataSource = objTipoUsuario.listarTiposUsuario() ' Usamos el método que ya tienes
            ddlTipoUsuario.DataValueField = "idTipoUsuario"
            ddlTipoUsuario.DataTextField = "nombre"
            ddlTipoUsuario.DataBind()
        Catch ex As Exception
            ' Manejo de error (opcional: mostrar un mensaje en la página)
            ' lblError.Text = "Error al cargar tipos de usuario: " & ex.Message
        End Try
    End Sub

    Private Sub ListarUsuarios()
        Try
            Dim dtUsuarios As DataTable = objUsuario.listarUsuarios()
            Dim dtTipos As DataTable = objTipoUsuario.listarTiposUsuario()

            ' Para evitar consultar la BD por cada usuario, creamos un diccionario de búsqueda
            Dim dictTipos As New Dictionary(Of Integer, String)
            For Each row As DataRow In dtTipos.Rows
                dictTipos.Add(CInt(row("idTipoUsuario")), row("nombre").ToString())
            Next

            Dim sb As New StringBuilder()

            For Each row As DataRow In dtUsuarios.Rows
                Dim id As Integer = CInt(row("idUsuario"))
                Dim nombres As String = row("nombresCompletos").ToString()
                Dim nombreUsuario As String = row("nombre").ToString()
                Dim pregunta As String = row("preguntaSecreta").ToString()
                Dim respuesta As String = row("respuesta").ToString()
                Dim idTipo As Integer = CInt(row("idTipoUsuario"))
                Dim vigente As Boolean = CBool(row("vigencia"))

                ' Obtenemos el nombre del tipo de usuario desde el diccionario
                Dim nombreTipo As String = If(dictTipos.ContainsKey(idTipo), dictTipos(idTipo), "No asignado")

                ' Creamos las filas de la tabla
                sb.Append("<tr>")
                sb.Append($"<td>{id}</td>")
                sb.Append($"<td>{nombres}</td>")
                sb.Append($"<td>{nombreUsuario}</td>")
                sb.Append($"<td>{nombreTipo}</td>")

                ' Estado con una insignia (badge) de Bootstrap
                If vigente Then
                    sb.Append("<td><span class='badge badge-success'>Vigente</span></td>")
                Else
                    sb.Append("<td><span class='badge badge-danger'>No Vigente</span></td>")
                End If

                ' Botones de acciones
                sb.Append("<td>")
                ' Botón Editar: Pasamos todos los datos a la función JS, escapando las comillas
                sb.Append($"<button type='button' class='btn btn-primary btn-sm' onclick=""fct_EditarUsuario({id}, {idTipo}, '{nombres.Replace("'", "\'")}', '{nombreUsuario.Replace("'", "\'")}', '{pregunta.Replace("'", "\'")}', '{respuesta.Replace("'", "\'")}', {vigente.ToString().ToLower()})""><i class='fas fa-edit'></i></button> ")

                ' Botón Eliminar
                sb.Append($"<button type='button' class='btn btn-danger btn-sm' onclick='fct_EliminarUsuario({id})'><i class='fas fa-trash'></i></button> ")

                ' Botón Dar de Baja o Alta
                If vigente Then
                    sb.Append($"<button type='button' class='btn btn-warning btn-sm' onclick='fct_DarBajaUsuario({id})'><i class='fas fa-arrow-down'></i></button>")
                Else
                    ' Si no está vigente, podrías tener una función para reactivar
                    ' sb.Append($"<button type='button' class='btn btn-info btn-sm' onclick='fct_DarAltaUsuario({id})'><i class='fas fa-arrow-up'></i></button>")
                End If
                sb.Append("</td>")

                sb.Append("</tr>")
            Next

            ' Llenamos el cuerpo de la tabla
            tbody_Usuario.InnerHtml = sb.ToString()

        Catch ex As Exception
            ' Manejo de error
            tbody_Usuario.InnerHtml = $"<tr><td colspan='6'>Error al listar usuarios: {ex.Message}</td></tr>"
        End Try
    End Sub

    ' --- WEBMETHODS (Llamados desde JavaScript/AJAX) ---

    ' DTO (Data Transfer Object) para recibir los datos del usuario desde el modal
    Public Class UsuarioDTO
        Public Property id As Integer
        Public Property idTipoUsuario As Integer
        Public Property nombresCompletos As String
        Public Property nombre As String
        Public Property contrasena As String
        Public Property preguntaSecreta As String
        Public Property respuesta As String
        Public Property vigente As Boolean
    End Class

    <WebMethod>
    Public Shared Function GuardarUsuario(oUsuario As UsuarioDTO) As String
        Dim objU As New clsUsuario()
        Try
            If oUsuario.id = 0 Then ' Es un nuevo usuario
                Dim newId As Integer = objU.generarIDUsuario()
                objU.guardarUsuario(newId, oUsuario.nombre, oUsuario.contrasena, oUsuario.nombresCompletos, oUsuario.preguntaSecreta, oUsuario.respuesta, oUsuario.vigente, oUsuario.idTipoUsuario)
            Else ' Es una actualización
                ' Si la contraseña viene vacía, no la actualizamos.
                Dim passFinal As String = oUsuario.contrasena
                If String.IsNullOrEmpty(oUsuario.contrasena) Then
                    ' Obtenemos la contraseña actual para no sobreescribirla con un vacío
                    Dim dtActual As DataTable = objU.buscarUsuario(oUsuario.id)
                    If dtActual.Rows.Count > 0 Then
                        passFinal = dtActual.Rows(0)("contrasena").ToString()
                    End If
                End If
                objU.modificarUsuario(oUsuario.id, oUsuario.nombre, passFinal, oUsuario.nombresCompletos, oUsuario.preguntaSecreta, oUsuario.respuesta, oUsuario.vigente, oUsuario.idTipoUsuario)
            End If
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function EliminarUsuario(id As Integer) As String
        Dim objU As New clsUsuario()
        Try
            objU.eliminarUsuario(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarBajaUsuario(id As Integer) As String
        Dim objU As New clsUsuario()
        Try
            objU.darBajaUsuario(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

End Class