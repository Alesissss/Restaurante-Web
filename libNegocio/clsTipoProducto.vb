Imports libDatos

Public Class clsTipoProducto
    Private objMan As New clsMantenimiento()
    Private strSQL As String = ""
    Private dtTipoProducto As New DataTable()

    Public Function generarIDTipoProducto() As Integer
        strSQL = "SELECT ISNULL(MAX(idTipo), 0) + 1 FROM TIPO_PRODUCTO"
        Try
            dtTipoProducto = objMan.listarComando(strSQL)
            If dtTipoProducto.Rows.Count > 0 Then
                Return Convert.ToInt32(dtTipoProducto.Rows(0).Item(0))
            End If
        Catch ex As Exception
            Throw New Exception("Error al generar el ID de Tipo de Producto: " & ex.Message)
        End Try
        Return 0
    End Function

    Public Sub guardarTipoProducto(ByVal id As Integer, ByVal nom As String, ByVal desc As String, ByVal vig As Boolean)
        Dim descripcionSQL As String = If(String.IsNullOrEmpty(desc), "NULL", "'" & desc.Replace("'", "''") & "'")

        strSQL = "INSERT INTO TIPO_PRODUCTO (idTipo, nombre, descripcion, vigencia) VALUES (" &
                 id & ", '" &
                 nom.Replace("'", "''") & "', " &
                 descripcionSQL & ", " &
                 IIf(vig, 1, 0) & ")"
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al registrar Tipo de Producto: " & ex.Message)
        End Try
    End Sub

    Public Sub modificarTipoProducto(ByVal id As Integer, ByVal nom As String, ByVal desc As String, ByVal vig As Boolean)
        Dim descripcionSQL As String = If(String.IsNullOrEmpty(desc), "NULL", "'" & desc.Replace("'", "''") & "'")

        strSQL = "UPDATE TIPO_PRODUCTO SET " &
                 "nombre = '" & nom.Replace("'", "''") & "', " &
                 "descripcion = " & descripcionSQL & ", " &
                 "vigencia = " & IIf(vig, 1, 0) & " " &
                 "WHERE idTipo = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al modificar Tipo de Producto: " & ex.Message)
        End Try
    End Sub

    Public Sub eliminarTipoProducto(ByVal id As Integer)
        strSQL = "DELETE FROM TIPO_PRODUCTO WHERE idTipo = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al eliminar Tipo de Producto: " & ex.Message)
        End Try
    End Sub

    Public Sub darBajaTipoProducto(ByVal id As Integer) ' Cambia vigencia a 0
        strSQL = "UPDATE TIPO_PRODUCTO SET vigencia = 0 WHERE idTipo = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de baja a Tipo de Producto: " & ex.Message)
        End Try
    End Sub

    Public Sub darAltaTipoProducto(ByVal id As Integer) ' Cambia vigencia a 1
        strSQL = "UPDATE TIPO_PRODUCTO SET vigencia = 1 WHERE idTipo = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de alta a Tipo de Producto: " & ex.Message)
        End Try
    End Sub

    Public Function buscarTipoProducto(ByVal id As Integer) As DataTable
        strSQL = "SELECT * FROM TIPO_PRODUCTO WHERE idTipo = " & id
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar Tipo de Producto: " & ex.Message)
        End Try
    End Function

    Public Function listarTiposProducto() As DataTable
        strSQL = "SELECT idTipo, nombre, descripcion, " &
         "CASE vigencia " &
         "WHEN 1 THEN 'Activo' " &
         "WHEN 0 THEN 'Inactivo' " &
         "ELSE 'Desconocido' END AS estado " &
         "FROM TIPO_PRODUCTO"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar Tipos de Producto: " & ex.Message)
        End Try
    End Function
End Class