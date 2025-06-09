Imports libDatos

Public Class clsCajero
    Private objMan As New clsMantenimiento()
    Private strSQL As String = ""
    Private dtCajero As New DataTable()

    Public Function generarIDCajero() As Integer
        strSQL = "SELECT ISNULL(MAX(idCajero), 0) + 1 FROM CAJERO"
        Try
            dtCajero = objMan.listarComando(strSQL)
            If dtCajero.Rows.Count > 0 Then
                Return Convert.ToInt32(dtCajero.Rows(0).Item(0))
            End If
        Catch ex As Exception
            Throw New Exception("Error al generar el ID de Cajero: " & ex.Message)
        End Try
        Return 0
    End Function

    Public Sub guardarCajero(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean)
        Dim telefonoSQL As String = If(String.IsNullOrEmpty(tel), "NULL", "'" & tel.Replace("'", "''") & "'")

        strSQL = "INSERT INTO CAJERO (idCajero, dniCajero, nombres, apellidos, telefono, correo, estado) VALUES (" &
                 id & ", '" &
                 dni.Replace("'", "''") & "', '" &
                 nom.Replace("'", "''") & "', '" &
                 ape.Replace("'", "''") & "', " &
                 telefonoSQL & ", '" &
                 cor.Replace("'", "''") & "', " &
                 IIf(est, 1, 0) & ")"
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al registrar Cajero: " & ex.Message)
        End Try
    End Sub

    Public Sub modificarCajero(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean)
        Dim telefonoSQL As String = If(String.IsNullOrEmpty(tel), "NULL", "'" & tel.Replace("'", "''") & "'")

        strSQL = "UPDATE CAJERO SET " &
                 "dniCajero = '" & dni.Replace("'", "''") & "', " &
                 "nombres = '" & nom.Replace("'", "''") & "', " &
                 "apellidos = '" & ape.Replace("'", "''") & "', " &
                 "telefono = " & telefonoSQL & ", " &
                 "correo = '" & cor.Replace("'", "''") & "', " &
                 "estado = " & IIf(est, 1, 0) & " " &
                 "WHERE idCajero = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al modificar Cajero: " & ex.Message)
        End Try
    End Sub

    Public Sub eliminarCajero(ByVal id As Integer)
        strSQL = "DELETE FROM CAJERO WHERE idCajero = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al eliminar Cajero: " & ex.Message)
        End Try
    End Sub

    Public Sub darBajaCajero(ByVal id As Integer)
        strSQL = "UPDATE CAJERO SET estado = 0 WHERE idCajero = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de baja a Cajero: " & ex.Message)
        End Try
    End Sub

    Public Sub darAltaCajero(ByVal id As Integer) ' Opcional
        strSQL = "UPDATE CAJERO SET estado = 1 WHERE idCajero = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de alta a Cajero: " & ex.Message)
        End Try
    End Sub

    Public Function buscarCajero(ByVal id As Integer) As DataTable
        strSQL = "SELECT * FROM CAJERO WHERE idCajero = " & id
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar Cajero: " & ex.Message)
        End Try
    End Function

    Public Function buscarCajeroPorDNI(ByVal dni As String) As DataTable
        strSQL = "SELECT * FROM CAJERO WHERE dniCajero = '" & dni.Replace("'", "''") & "'"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar Cajero por DNI: " & ex.Message)
        End Try
    End Function

    Public Function listarCajeros() As DataTable
        strSQL = "SELECT idCajero,dniCajero, nombres, apellidos, telefono, correo, " &
         "CASE estado " &
         "WHEN 1 THEN 'Activo' " &
         "WHEN 0 THEN 'Inactivo' " &
         "ELSE 'Desconocido' END AS estado " &
         "FROM CAJERO"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar Cajeros: " & ex.Message)
        End Try
    End Function
    Public Function listarCajerosVigentes() As DataTable
        strSQL = "SELECT idCajero,dniCajero, nombres, apellidos, telefono, correo, " &
         "CASE estado " &
         "WHEN 1 THEN 'Activo' " &
         "WHEN 0 THEN 'Inactivo' " &
         "ELSE 'Desconocido' END AS estado " &
         "FROM CAJERO WHERE estado = 1"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar Cajeros: " & ex.Message)
        End Try
    End Function
    Public Function VerificarCajero(idCajero As Integer) As Boolean
        strSQL = "SELECT COUNT(*) FROM CAJERO WHERE idCajero = @idCajero and estado = 1"
        Try
            Dim parametros As New Dictionary(Of String, Object) From {
                {"@idCajero", idCajero}
            }
            Dim dt As DataTable = objMan.listarComando(strSQL, parametros)
            If dt.Rows.Count > 0 Then
                Return Convert.ToInt32(dt.Rows(0).Item(0)) > 0
            End If
        Catch ex As Exception
            Throw New Exception("Error al verificar cajero: " & ex.Message)
        End Try
        Return False
    End Function
End Class