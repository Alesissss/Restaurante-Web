Imports libDatos ' Asegúrate que clsMantenimiento esté accesible

Public Class clsCliente
    Private objMan As New clsMantenimiento()
    Private strSQL As String = ""
    Private dtCliente As New DataTable()

    Public Function generarIDCliente() As Integer
        strSQL = "SELECT ISNULL(MAX(idCliente), 0) + 1 FROM CLIENTE"
        Try
            dtCliente = objMan.listarComando(strSQL)
            If dtCliente.Rows.Count > 0 Then
                Return Convert.ToInt32(dtCliente.Rows(0).Item(0))
            End If
        Catch ex As Exception
            Throw New Exception("Error al generar el ID de Cliente: " & ex.Message)
        End Try
        Return 0 ' O manejar el error de forma diferente
    End Function

    Public Sub guardarCliente(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean)
        ' Limpiar teléfono si es nulo o vacío para evitar error en SQL si la columna no lo permite como ''
        Dim telefonoSQL As String = If(String.IsNullOrEmpty(tel), "NULL", "'" & tel.Replace("'", "''") & "'") ' Sanitizar apóstrofes

        strSQL = "INSERT INTO CLIENTE (idCliente, dniCliente, nombres, apellidos, telefono, correo, estado) VALUES (" &
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
            Throw New Exception("Error al registrar Cliente: " & ex.Message)
        End Try
    End Sub

    Public Sub modificarCliente(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean)
        Dim telefonoSQL As String = If(String.IsNullOrEmpty(tel), "NULL", "'" & tel.Replace("'", "''") & "'")

        strSQL = "UPDATE CLIENTE SET " &
                 "dniCliente = '" & dni.Replace("'", "''") & "', " &
                 "nombres = '" & nom.Replace("'", "''") & "', " &
                 "apellidos = '" & ape.Replace("'", "''") & "', " &
                 "telefono = " & telefonoSQL & ", " &
                 "correo = '" & cor.Replace("'", "''") & "', " &
                 "estado = " & IIf(est, 1, 0) & " " &
                 "WHERE idCliente = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al modificar Cliente: " & ex.Message)
        End Try
    End Sub

    Public Sub eliminarCliente(ByVal id As Integer)
        strSQL = "DELETE FROM CLIENTE WHERE idCliente = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al eliminar Cliente: " & ex.Message)
        End Try
    End Sub

    Public Sub darBajaCliente(ByVal id As Integer)
        strSQL = "UPDATE CLIENTE SET estado = 0 WHERE idCliente = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de baja a Cliente: " & ex.Message)
        End Try
    End Sub

    Public Sub darAltaCliente(ByVal id As Integer) ' Opcional, para reactivar
        strSQL = "UPDATE CLIENTE SET estado = 1 WHERE idCliente = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de alta a Cliente: " & ex.Message)
        End Try
    End Sub

    Public Function buscarCliente(ByVal id As Integer) As DataTable
        strSQL = "SELECT * FROM CLIENTE WHERE idCliente = " & id
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar Cliente: " & ex.Message)
        End Try
    End Function

    Public Function buscarClientePorDNI(ByVal dni As String) As DataTable
        strSQL = "SELECT * FROM CLIENTE WHERE dniCliente = '" & dni.Replace("'", "''") & "'"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar Cliente por DNI: " & ex.Message)
        End Try
    End Function

    Public Function listarClientes() As DataTable
        strSQL = "SELECT idCliente,dniCliente, nombres, apellidos, telefono, correo, " &
         "CASE estado " &
         "WHEN 1 THEN 'Activo' " &
         "WHEN 0 THEN 'Inactivo' " &
         "ELSE 'Desconocido' END AS estado " &
         "FROM CLIENTE"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar Clientes: " & ex.Message)
        End Try
    End Function
    Public Function listarClientesVigentes() As DataTable
        strSQL = "SELECT idCliente,dniCliente, nombres, apellidos, telefono, correo, " &
         "CASE estado " &
         "WHEN 1 THEN 'Activo' " &
         "WHEN 0 THEN 'Inactivo' " &
         "ELSE 'Desconocido' END AS estado " &
         "FROM CLIENTE WHERE estado = 1"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar Clientes: " & ex.Message)
        End Try
    End Function
    Public Function VerificarCliente(idCliente As Integer) As Boolean
        strSQL = "SELECT COUNT(*) FROM CLIENTE WHERE idCliente = @idCliente and estado = 1"
        Try
            Dim parametros As New Dictionary(Of String, Object) From {
                {"@idCliente", idCliente}
            }
            Dim dt As DataTable = objMan.listarComando(strSQL, parametros)
            If dt.Rows.Count > 0 Then
                Return Convert.ToInt32(dt.Rows(0).Item(0)) > 0
            End If
        Catch ex As Exception
            Throw New Exception("Error al verificar cliente: " & ex.Message)
        End Try
        Return False
    End Function
End Class