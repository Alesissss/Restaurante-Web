Imports libDatos
Public Class clsUsuario
    Dim objMan As New clsMantenimiento
    Dim strSQL As String = ""
    Dim dtUsuario As New DataTable

    Public Function iniciarSesion(usu As String, con As String) As Boolean
        strSQL = "select * from usuario where nombre='" & usu & "' and contrasena='" & con & "'"
        Try
            dtUsuario = objMan.listarComando(strSQL)
            If dtUsuario.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception("Error al ejecutar consulta!")
        End Try
    End Function

    Public Function obtenerPreguntaSec(usu As String) As String
        strSQL = "select preguntasecreta from usuario where nombre='" & usu & "'"
        Try
            dtUsuario = objMan.listarComando(strSQL)
            If dtUsuario.Rows.Count > 0 Then
                Return dtUsuario.Rows(0).Item(0)
            Else
                Return "USUARIO no existe!"
            End If
        Catch ex As Exception
            Throw New Exception("Error al ejecutar consulta!")
        End Try
    End Function

    Public Function validarRespuestaPregSec(usu As String, rpta As String) As Boolean
        strSQL = "select * from usuario where nombre='" & usu & "' and respuesta='" & rpta & "'"
        Try
            dtUsuario = objMan.listarComando(strSQL)
            If dtUsuario.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception("Error al ejecutar consulta!")
        End Try
    End Function

    Public Function obtenerNombreUsuario(usu As String) As String
        strSQL = "select nombresCompletos from usuario where nombre='" & usu & "'"
        Try
            dtUsuario = objMan.listarComando(strSQL)
            If dtUsuario.Rows.Count > 0 Then
                Return dtUsuario.Rows(0).Item(0)
            Else
                Return "USUARIO no existe!"
            End If
        Catch ex As Exception
            Throw New Exception("Error al ejecutar consulta!")
        End Try
    End Function
    Public Function generarIDUsuario() As Integer
        strSQL = "SELECT ISNULL(MAX(idUsuario), 0) + 1 FROM USUARIO"
        Try
            dtUsuario = objMan.listarComando(strSQL)
            If dtUsuario.Rows.Count > 0 Then
                Return Convert.ToInt32(dtUsuario.Rows(0).Item(0))
            End If
        Catch ex As Exception
            Throw New Exception("Error al generar el ID de usuario: " & ex.Message)
        End Try
        Return 0
    End Function

    Public Sub guardarUsuario(ByVal id As Integer, ByVal nom As String, ByVal contrasena As String, ByVal nombresCompletos As String, ByVal preguntaSecreta As String, ByVal respuesta As String, ByVal vig As Boolean, ByVal idTipoUsuario As Integer)
        strSQL = "INSERT INTO USUARIO (idUsuario, nombre, contrasena, nombresCompletos, preguntaSecreta, respuesta, vigencia, idTipoUsuario) VALUES (" &
             id & ", '" &
             nom.Replace("'", "''") & "', '" &
             contrasena.Replace("'", "''") & "', '" &
             nombresCompletos.Replace("'", "''") & "', '" &
             preguntaSecreta.Replace("'", "''") & "', '" &
             respuesta.Replace("'", "''") & "', " &
             IIf(vig, 1, 0) & ", " &
             idTipoUsuario & ")"
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al registrar el usuario: " & ex.Message)
        End Try
    End Sub

    Public Sub modificarUsuario(ByVal id As Integer, ByVal nom As String, ByVal contrasena As String, ByVal nombresCompletos As String, ByVal preguntaSecreta As String, ByVal respuesta As String, ByVal vig As Boolean, ByVal idTipoUsuario As Integer)
        strSQL = "UPDATE USUARIO SET " &
             "nombre = '" & nom.Replace("'", "''") & "', " &
             "contrasena = '" & contrasena.Replace("'", "''") & "', " &
             "nombresCompletos = '" & nombresCompletos.Replace("'", "''") & "', " &
             "preguntaSecreta = '" & preguntaSecreta.Replace("'", "''") & "', " &
             "respuesta = '" & respuesta.Replace("'", "''") & "', " &
             "vigencia = " & IIf(vig, 1, 0) & ", " &
             "idTipoUsuario = " & idTipoUsuario & " " &
             "WHERE idUsuario = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al modificar el usuario: " & ex.Message)
        End Try
    End Sub

    Public Sub eliminarUsuario(ByVal id As Integer)
        strSQL = "DELETE FROM USUARIO WHERE idUsuario = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al eliminar el usuario: " & ex.Message)
        End Try
    End Sub

    Public Sub darBajaUsuario(ByVal id As Integer)
        strSQL = "UPDATE USUARIO SET vigencia = 0 WHERE idUsuario = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de baja al usuario: " & ex.Message)
        End Try
    End Sub

    Public Sub darAltaUsuario(ByVal id As Integer)
        strSQL = "UPDATE USUARIO SET vigencia = 1 WHERE idUsuario = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de alta al usuario: " & ex.Message)
        End Try
    End Sub

    Public Function buscarUsuario(ByVal id As Integer) As DataTable
        strSQL = "SELECT * FROM USUARIO WHERE idUsuario = " & id
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar el usuario: " & ex.Message)
        End Try
    End Function

    Public Function buscarUsuarioPorNombre(ByVal nombres As String) As DataTable
        strSQL = "Select * From USUARIO Where nombre = '" & nombres & "'"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar el usuario: " & ex.Message)
        End Try
    End Function
    '
    Public Function listarUsuarios() As DataTable
        strSQL = "SELECT * FROM USUARIO"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar los usuarios: " & ex.Message)
        End Try
    End Function

End Class


