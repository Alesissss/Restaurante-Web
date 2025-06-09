Imports libDatos
Public Class clsTipoUsuario
    Dim objMan As New clsMantenimiento
    Dim strSQL As String = ""
    Dim dtTipoUsuario As New DataTable
    Public Function generarIDTipoUsuario() As Integer
        strSQL = "SELECT ISNULL(MAX(idTipoUsuario), 0) + 1 FROM TIPO_USUARIO"
        Try
            dtTipoUsuario = objMan.listarComando(strSQL)
            If dtTipoUsuario.Rows.Count > 0 Then
                Return Convert.ToInt32(dtTipoUsuario.Rows(0).Item(0))
            End If
        Catch ex As Exception
            Throw New Exception("Error al generar el ID de Tipo de Usuario: " & ex.Message)
        End Try
        Return 0
    End Function

    Public Sub guardarTipoUsuario(ByVal id As Integer, ByVal nom As String, ByVal desc As String, ByVal vig As Boolean)
        Dim descripcionSQL As String = If(String.IsNullOrEmpty(desc), "NULL", "'" & desc.Replace("'", "''") & "'")

        strSQL = "INSERT INTO TIPO_USUARIO (idTipoUsuario, nombre, descripcion, vigencia) VALUES (" &
                 id & ", '" &
                 nom.Replace("'", "''") & "', " &
                 descripcionSQL & ", " &
                 IIf(vig, 1, 0) & ")"
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al registrar Tipo de Usuario: " & ex.Message)
        End Try
    End Sub

    Public Sub modificarTipoUsuario(ByVal id As Integer, ByVal nom As String, ByVal desc As String, ByVal vig As Boolean)
        Dim descripcionSQL As String = If(String.IsNullOrEmpty(desc), "NULL", "'" & desc.Replace("'", "''") & "'")

        strSQL = "UPDATE TIPO_USUARIO SET " &
                 "nombre = '" & nom.Replace("'", "''") & "', " &
                 "descripcion = " & descripcionSQL & ", " &
                 "vigencia = " & IIf(vig, 1, 0) & " " &
                 "WHERE idTipoUsuario = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al modificar Tipo de Usuario: " & ex.Message)
        End Try
    End Sub

    Public Sub eliminarTipoUsuario(ByVal id As Integer)
        strSQL = "DELETE FROM TIPO_USUARIO WHERE idTipoUsuario = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al eliminar Tipo de Usuario: " & ex.Message)
        End Try
    End Sub

    Public Sub darBajaTipoUsuario(ByVal id As Integer) ' Cambia vigencia a 0
        strSQL = "UPDATE TIPO_USUARIO SET vigencia = 0 WHERE idTipoUsuario = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de baja a Tipo de Usuario: " & ex.Message)
        End Try
    End Sub

    Public Sub darAltaTipoUsuario(ByVal id As Integer) ' Cambia vigencia a 1
        strSQL = "UPDATE TIPO_USUARIO SET vigencia = 1 WHERE idTipoUsuario = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de alta a Tipo de Usuario: " & ex.Message)
        End Try
    End Sub

    Public Function buscarTipoUsuario(ByVal id As Integer) As DataTable
        strSQL = "SELECT * FROM TIPO_USUARIO WHERE idTipoUsuario = " & id
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar Tipo de Usuario: " & ex.Message)
        End Try
    End Function

    Public Function listarTiposUsuario() As DataTable
        strSQL = "SELECT idTipoUsuario, nombre, descripcion, " &
         "CASE vigencia " &
         "WHEN 1 THEN 'Activo' " &
         "WHEN 0 THEN 'Inactivo' " &
         "ELSE 'Desconocido' END AS estado " &
         "FROM TIPO_USUARIO"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar Tipos de Usuario: " & ex.Message)
        End Try
    End Function
End Class


