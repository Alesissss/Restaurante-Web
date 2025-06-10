' Archivo: clsMantenimiento.vb (Versión Final, Segura y Profesional)
Imports System.Data
Imports System.Data.SqlClient

Public Class clsMantenimiento
    Private objCon As New clsConectaBD()

    Private Function CrearConexion() As SqlConnection
        Return New SqlConnection(objCon.gen_cad_cloud())
    End Function

    Private Sub AsignarParametros(ByRef cmd As SqlCommand, Optional parametros As Dictionary(Of String, Object) = Nothing)
        If parametros IsNot Nothing Then
            cmd.Parameters.Clear() ' Limpiar parámetros anteriores
            For Each p In parametros
                ' Usamos AddWithValue para manejar los tipos de datos automáticamente.
                ' Si el valor es Nothing, lo convertimos a DBNull.Value.
                cmd.Parameters.AddWithValue(p.Key, If(p.Value, DBNull.Value))
            Next
        End If
    End Sub

    ' Método para SELECT que devuelve una tabla de datos
    Public Function listarComando(ByVal sql As String, Optional parametros As Dictionary(Of String, Object) = Nothing) As DataTable
        Dim dt As New DataTable()
        ' 'Using' asegura que la conexión se cierre incluso si hay un error
        Using conn As SqlConnection = CrearConexion()
            Try
                conn.Open()
                ' Se crea un comando nuevo y limpio para esta operación específica
                Using cmd As New SqlCommand(sql, conn)
                    AsignarParametros(cmd, parametros)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            Catch ex As Exception
                ' Lanzamos el error original y detallado para un mejor diagnóstico
                Throw New Exception("Error en capa de datos (listarComando): " & ex.Message)
            End Try
        End Using
        Return dt
    End Function

    ' Método para INSERT, UPDATE, DELETE
    Public Function ejecutarComando(ByVal sql As String, Optional parametros As Dictionary(Of String, Object) = Nothing) As Boolean
        Dim filasAfectadas As Integer = 0
        Using conn As SqlConnection = CrearConexion()
            Try
                conn.Open()
                ' Se crea un comando nuevo y limpio para esta operación
                Using cmd As New SqlCommand(sql, conn)
                    AsignarParametros(cmd, parametros)
                    filasAfectadas = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                Throw New Exception("Error en capa de datos (ejecutarComando): " & ex.Message)
            End Try
        End Using
        Return (filasAfectadas > 0)
    End Function
End Class