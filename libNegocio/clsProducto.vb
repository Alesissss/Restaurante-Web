Imports libDatos
Public Class clsProducto
    Dim objMan As New clsMantenimiento
    Dim strSQL As String = ""
    Dim dtProducto As New DataTable
    Public Function generarIDProducto() As Integer
        strSQL = "SELECT ISNULL(MAX(idProducto), 0) + 1 FROM PRODUCTO"
        Try
            dtProducto = objMan.listarComando(strSQL)
            If dtProducto.Rows.Count > 0 Then
                Return Convert.ToInt32(dtProducto.Rows(0).Item(0))
            End If
        Catch ex As Exception
            Throw New Exception("Error al generar el ID de Producto: " & ex.Message)
        End Try
        Return 0
    End Function
    Public Sub guardarProducto(ByVal id As Integer, ByVal nom As String, ByVal desc As String, ByVal precio As Decimal, ByVal vig As Boolean, ByVal idTipo As Integer, ByVal idCarta As Integer)
        Dim descripcionSQL As String = If(String.IsNullOrEmpty(desc), "NULL", "'" & desc.Replace("'", "''") & "'")

        strSQL = "INSERT INTO PRODUCTO (idProducto, nombre, descripcion, precio, vigencia, idTipo, idCarta) VALUES (" &
             id & ", '" &
             nom.Replace("'", "''") & "', " &
             descripcionSQL & ", " &
             precio & ", " &
             IIf(vig, 1, 0) & ", " &
             idTipo & ", " &
             idCarta &
             ")"
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al registrar el producto: " & ex.Message)
        End Try
    End Sub
    Public Sub modificarProducto(ByVal id As Integer, ByVal nom As String, ByVal desc As String, ByVal precio As Decimal, ByVal vig As Boolean, ByVal idTipo As Integer, ByVal idCarta As Integer)
        Dim descripcionSQL As String = If(String.IsNullOrEmpty(desc), "NULL", "'" & desc.Replace("'", "''") & "'")

        strSQL = "UPDATE PRODUCTO SET " &
             "nombre = '" & nom.Replace("'", "''") & "', " &
             "descripcion = " & descripcionSQL & ", " &
             "precio = " & precio & ", " &
             "vigencia = " & IIf(vig, 1, 0) & ", " &
             "idTipo = " & idTipo & " " &
             "idCarta = " & idCarta & " " &
             "WHERE idProducto = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al modificar el producto: " & ex.Message)
        End Try
    End Sub

    Public Sub eliminarProducto(ByVal id As Integer)
        strSQL = "DELETE FROM PRODUCTO WHERE idProducto = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al eliminar el producto: " & ex.Message)
        End Try
    End Sub

    Public Sub darBajaProducto(ByVal id As Integer) ' Cambia vigencia a 0
        strSQL = "UPDATE PRODUCTO SET vigencia = 0 WHERE idProducto = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de baja al producto: " & ex.Message)
        End Try
    End Sub

    Public Sub darAltaProducto(ByVal id As Integer) ' Cambia vigencia a 1
        strSQL = "UPDATE PRODUCTO SET vigencia = 1 WHERE idProducto = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de alta al producto: " & ex.Message)
        End Try
    End Sub
    Public Function buscarProducto(ByVal id As Integer) As DataTable
        strSQL = "SELECT * FROM PRODUCTO WHERE idProducto = " & id
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar el producto: " & ex.Message)
        End Try
    End Function
    Public Function listarProductos() As DataTable
        strSQL = "SELECT p.idProducto, p.nombre, p.descripcion, p.precio, " &
             "tp.nombre AS tipo_producto, tp.idTipo, " &
             "c.nombre AS carta, c.idCarta, " &
             "CASE p.vigencia " &
             " WHEN 1 THEN 'Activo' " &
             " WHEN 0 THEN 'Inactivo' " &
             " ELSE 'Desconocido' END AS estado " &
             "FROM PRODUCTO p " &
             "LEFT JOIN TIPO_PRODUCTO tp ON p.idTipo = tp.idTipo " &
             "LEFT JOIN CARTA c ON c.idCarta = p.idCarta"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar los productos: " & ex.Message)
        End Try
    End Function

    Public Function ListarProductosPorIdCarta(ByVal idCartaBuscada As Integer) As DataTable
        strSQL = "SELECT tp.nombre as tipo_producto, p.nombre, p.descripcion, p.precio FROM PRODUCTO p LEFT JOIN TIPO_PRODUCTO tp ON p.idTipo = tp.idTipo WHERE idCarta = @IdDeLaCarta ORDER BY 1"
        Try
            Dim parametros As New Dictionary(Of String, Object) From {
            {"@IdDeLaCarta", idCartaBuscada}
        }

            Return objMan.listarComando(strSQL, parametros)

        Catch ex As Exception
            Dim mensajeError As String = "SQL: " & strSQL & vbCrLf &
                                   "Parámetros: idCarta=" & idCartaBuscada.ToString() & vbCrLf &
                                   "Error al listar los productos de la carta: " & ex.Message
            Throw New Exception(mensajeError)
        End Try
    End Function
End Class


