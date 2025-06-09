Imports System.Data
Imports System.Data.SqlClient

Public Class clsMantenimiento
    Dim objConecta As New clsConectaBD
    Dim cm As New SqlCommand

    'Insert, update y delete
    Sub ejecutarComando(strConsulta As String)
        Try
            'objConecta.abrirconexionTrans()
            objConecta.abrirconexion()
            cm.Connection = objConecta.miConexion
            cm.CommandText = strConsulta
            'If transaccion = True Then
            '    cm.Transaction = tsql
            'End If
            cm.ExecuteNonQuery()
            'objConecta.cerrarconexionTrans()
            objConecta.cerrarconexion()
        Catch ex As Exception
            'objConecta.cancelarconexionTrans()
            Throw New Exception("Error al realizar la operación...")
        End Try
    End Sub

    'select
    Public Function listarComando(strConsulta As String) As DataTable
        Dim dt As New DataTable
        Dim da As SqlDataAdapter
        Try
            'objConecta.abrirconexionTrans()
            objConecta.abrirconexion()
            da = New SqlDataAdapter(strConsulta, objConecta.miConexion)
            'If transaccion = True Then
            '    da.SelectCommand.Transaction = tsql
            'End If
            da.Fill(dt)
            'objConecta.cerrarconexionTrans()
            objConecta.cerrarconexion()
            Return dt
        Catch ex As Exception
            objConecta.cancelarconexionTrans()
            Throw New Exception("Error al realizar la consulta...")
        End Try
        Return Nothing
    End Function

    Public Function listarComando(Optional strConsulta As String = "", Optional strConsulta1 As String = "") As DataSet
        Dim ds As New DataSet
        Dim da As SqlDataAdapter
        Try
            objConecta.abrirconexionTrans()
            da = New SqlDataAdapter(strConsulta, objConecta.miConexion)
            'If transaccion = True Then
            '    da.SelectCommand.Transaction = tsql
            'End If
            da.Fill(ds, "Tabla1")
            objConecta.cerrarconexionTrans()
            Return ds
        Catch ex As Exception
            objConecta.cancelarconexionTrans()
            Throw New Exception("Error al realizar la consulta...")
        End Try
        Return Nothing
    End Function
    Public Sub ejecutarComando(strConsulta As String, parametros As Dictionary(Of String, Object))
        Try
            objConecta.abrirconexion()
            cm.Connection = objConecta.miConexion
            cm.CommandText = strConsulta

            ' Añadir parámetros al comando
            For Each param In parametros
                cm.Parameters.AddWithValue(param.Key, param.Value)
            Next

            cm.ExecuteNonQuery()
            objConecta.cerrarconexion()
        Catch ex As Exception
            objConecta.cerrarconexion()
            Throw New Exception("Error al ejecutar el comando: " & ex.Message)
        End Try
    End Sub

    ' Método para listar datos (SELECT) con parámetros
    Public Function listarComando(strConsulta As String, parametros As Dictionary(Of String, Object)) As DataTable
        Dim dt As New DataTable()
        Dim da As SqlDataAdapter
        Try
            objConecta.abrirconexion()
            da = New SqlDataAdapter(strConsulta, objConecta.miConexion)

            ' Añadir parámetros al comando
            For Each param In parametros
                da.SelectCommand.Parameters.AddWithValue(param.Key, param.Value)
            Next

            da.Fill(dt)
            objConecta.cerrarconexion()
            Return dt
        Catch ex As Exception
            objConecta.cerrarconexion()
            Throw New Exception("Error al listar los datos: " & ex.Message)
        End Try
        Return Nothing
    End Function
End Class
