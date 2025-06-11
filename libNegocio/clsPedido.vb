' Archivo: clsPedido.vb (Versión Final Definitiva)
Imports System.Data
Imports System.Data.SqlClient
Imports libDatos
Imports System.Linq

Public Class clsPedido
    Private objMan As New clsMantenimiento()
    Private objCon As New clsConectaBD()
    Private strSQL As String = ""

    Public Function BuscarPedidoActivoPorMesa(ByVal idMesa As Integer) As DataTable
        strSQL = "SELECT * FROM PEDIDO WHERE idMesa = @idMesa AND estadoPedido = 1"
        Dim parametros As New Dictionary(Of String, Object) From {{"@idMesa", idMesa}}
        Return objMan.listarComando(strSQL, parametros)
    End Function

    Public Function ListarDetallesPorId(ByVal idPedido As Integer) As DataTable
        strSQL = "SELECT d.idProducto, p.nombre, d.cantidad, d.precioVenta FROM DETALLE_PEDIDO d " &
                 "INNER JOIN PRODUCTO p ON d.idProducto = p.idProducto " &
                 "WHERE d.idPedido = @idPedido"
        Dim parametros As New Dictionary(Of String, Object) From {{"@idPedido", idPedido}}
        Return objMan.listarComando(strSQL, parametros)
    End Function

    Public Function ListarHistorialDePedidos() As DataTable
        strSQL = "SELECT p.idPedido, p.fecha, p.monto, m.numero AS Mesa, " &
                 "ISNULL(c.nombres + ' ' + c.apellidos, 'Sin cliente') AS Cliente, " &
                 "ISNULL(mes.nombres + ' ' + mes.apellidos, 'Mesero Eliminado') AS Mesero, " &
                 "ISNULL(caj.nombres + ' ' + caj.apellidos, 'Cajero Eliminado') AS Cajero, " &
                 "CASE p.estadoPago WHEN 0 THEN 'Pagado' ELSE 'Pendiente' END AS Estado " &
                 "FROM PEDIDO p " &
                 "LEFT JOIN MESA m ON p.idMesa = m.idMesa " &
                 "LEFT JOIN CLIENTE c ON p.idCliente = c.idCliente " &
                 "LEFT JOIN MESERO mes ON p.idMesero = mes.idMesero " &
                 "LEFT JOIN CAJERO caj ON p.idCajero = caj.idCajero " &
                 "WHERE p.estadoPedido = 0 ORDER BY p.fecha DESC"
        Return objMan.listarComando(strSQL)
    End Function

    Public Sub RegistrarPedido(ByVal idCliente As Integer, ByVal idMesero As Integer, ByVal idMesa As Integer, ByVal detalles As List(Of DetalleDTO))
        Using conn As New SqlConnection(objCon.gen_cad_cloud())
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()
            Try
                Dim idPedido As Integer
                Using cmdId As New SqlCommand("SELECT ISNULL(MAX(idPedido), 0) + 1 FROM PEDIDO", conn, transaction)
                    idPedido = CInt(cmdId.ExecuteScalar())
                End Using

                Dim montoTotal As Decimal = detalles.Sum(Function(d) d.Precio * d.Cantidad)

                Dim sqlPedido As String = "INSERT INTO PEDIDO (idPedido, fecha, monto, estadoPedido, estadoPago, idMesero, idMesa) " &
                                        "VALUES (@idPedido, GETDATE(), @monto, 1, 1, @idMesero, @idMesa)"
                Using cmd As New SqlCommand(sqlPedido, conn, transaction)
                    cmd.Parameters.AddWithValue("@idPedido", idPedido)
                    cmd.Parameters.AddWithValue("@monto", montoTotal)
                    cmd.Parameters.AddWithValue("@idMesero", idMesero)
                    cmd.Parameters.AddWithValue("@idMesa", idMesa)
                    cmd.ExecuteNonQuery()
                End Using

                Dim sqlDetalle As String = "INSERT INTO DETALLE_PEDIDO (idPedido, idProducto, cantidad, precioVenta) VALUES (@idPedido, @idProducto, @cantidad, @precioVenta)"
                For Each item In detalles
                    Using cmdDet As New SqlCommand(sqlDetalle, conn, transaction)
                        cmdDet.Parameters.AddWithValue("@idPedido", idPedido)
                        cmdDet.Parameters.AddWithValue("@idProducto", item.IdProducto)
                        cmdDet.Parameters.AddWithValue("@cantidad", item.Cantidad)
                        cmdDet.Parameters.AddWithValue("@precioVenta", item.Precio)
                        cmdDet.ExecuteNonQuery()
                    End Using
                Next

                Using cmdMesa As New SqlCommand("UPDATE MESA SET estadoMesa = 0 WHERE idMesa = @idMesa", conn, transaction)
                    cmdMesa.Parameters.AddWithValue("@idMesa", idMesa)
                    cmdMesa.ExecuteNonQuery()
                End Using

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw New Exception("Error transaccional al registrar pedido: " & ex.Message)
            End Try
        End Using
    End Sub
    Public Sub ActualizarPedido(ByVal idPedido As Integer, ByVal nuevosDetalles As List(Of DetalleDTO))
        Using conn As New SqlConnection(objCon.gen_cad_cloud())
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()
            Try
                For Each item In nuevosDetalles
                    ' Verificar si ya existe el producto en el pedido
                    Dim existeDetalle As Boolean = False
                    Dim cantidadActual As Integer = 0

                    Using cmdCheck As New SqlCommand("SELECT cantidad FROM DETALLE_PEDIDO WHERE idPedido = @idPedido AND idProducto = @idProducto", conn, transaction)
                        cmdCheck.Parameters.AddWithValue("@idPedido", idPedido)
                        cmdCheck.Parameters.AddWithValue("@idProducto", item.IdProducto)
                        Dim result = cmdCheck.ExecuteScalar()
                        If result IsNot Nothing Then
                            existeDetalle = True
                            cantidadActual = Convert.ToInt32(result)
                        End If
                    End Using

                    If existeDetalle Then
                        ' Si ya existe, sumamos la cantidad (precio se mantiene igual)
                        Using cmdUpdate As New SqlCommand("UPDATE DETALLE_PEDIDO SET cantidad = @cantidad WHERE idPedido = @idPedido AND idProducto = @idProducto", conn, transaction)
                            cmdUpdate.Parameters.AddWithValue("@cantidad", cantidadActual + item.Cantidad)
                            cmdUpdate.Parameters.AddWithValue("@idPedido", idPedido)
                            cmdUpdate.Parameters.AddWithValue("@idProducto", item.IdProducto)
                            cmdUpdate.ExecuteNonQuery()
                        End Using
                    Else
                        ' Si no existe, insertamos normalmente
                        Using cmdInsert As New SqlCommand("INSERT INTO DETALLE_PEDIDO (idPedido, idProducto, cantidad, precioVenta) VALUES (@idPedido, @idProducto, @cantidad, @precioVenta)", conn, transaction)
                            cmdInsert.Parameters.AddWithValue("@idPedido", idPedido)
                            cmdInsert.Parameters.AddWithValue("@idProducto", item.IdProducto)
                            cmdInsert.Parameters.AddWithValue("@cantidad", item.Cantidad)
                            cmdInsert.Parameters.AddWithValue("@precioVenta", item.Precio)
                            cmdInsert.ExecuteNonQuery()
                        End Using
                    End If
                Next

                ' Recalcular monto total
                Dim montoTotalActualizado As Decimal
                Using cmdMonto As New SqlCommand("SELECT SUM(cantidad * precioVenta) FROM DETALLE_PEDIDO WHERE idPedido = @idPedido", conn, transaction)
                    cmdMonto.Parameters.AddWithValue("@idPedido", idPedido)
                    montoTotalActualizado = CDec(cmdMonto.ExecuteScalar())
                End Using

                ' Actualizar monto del pedido
                Using cmdUpdateMonto As New SqlCommand("UPDATE PEDIDO SET monto = @monto WHERE idPedido = @idPedido", conn, transaction)
                    cmdUpdateMonto.Parameters.AddWithValue("@monto", montoTotalActualizado)
                    cmdUpdateMonto.Parameters.AddWithValue("@idPedido", idPedido)
                    cmdUpdateMonto.ExecuteNonQuery()
                End Using

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw New Exception("Error transaccional al actualizar pedido: " & ex.Message)
            End Try
        End Using
    End Sub

    Public Sub ProcesarPago(ByVal idPedido As Integer, ByVal idCajero As Integer, ByVal idCliente As Integer)
        Using conn As New SqlConnection(objCon.gen_cad_cloud())
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()
            Try
                Dim idMesa As Integer
                Using cmdMesaId As New SqlCommand("SELECT idMesa FROM PEDIDO WHERE idPedido = @idPedido", conn, transaction)
                    cmdMesaId.Parameters.AddWithValue("@idPedido", idPedido)
                    idMesa = CInt(cmdMesaId.ExecuteScalar())
                End Using

                Dim sqlPedido As String = "UPDATE PEDIDO SET estadoPago = 0, estadoPedido = 0, idCajero = @idCajero, idCliente = @idCliente WHERE idPedido = @idPedido"
                Using cmd As New SqlCommand(sqlPedido, conn, transaction)
                    cmd.Parameters.AddWithValue("@idCajero", idCajero)
                    cmd.Parameters.AddWithValue("@idCliente", idCliente)
                    cmd.Parameters.AddWithValue("@idPedido", idPedido)
                    cmd.ExecuteNonQuery()
                End Using

                Using cmdMesa As New SqlCommand("UPDATE MESA SET estadoMesa = 1 WHERE idMesa = @idMesa", conn, transaction)
                    cmdMesa.Parameters.AddWithValue("@idMesa", idMesa)
                    cmdMesa.ExecuteNonQuery()
                End Using

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw New Exception("Error transaccional al procesar el pago: " & ex.Message)
            End Try
        End Using
    End Sub

    ' --- MÉTODOS DE REPORTE ---
    Public Function ContarPedidosHoy() As Integer
        strSQL = "SELECT COUNT(*) FROM PEDIDO WHERE CONVERT(DATE, fecha) = CONVERT(DATE, GETDATE())"
        Return Convert.ToInt32(objMan.listarComando(strSQL).Rows(0)(0))
    End Function
    Public Function ContarPedidosPendientes() As Integer
        strSQL = "SELECT COUNT(*) FROM PEDIDO WHERE estadoPedido = 1"
        Return Convert.ToInt32(objMan.listarComando(strSQL).Rows(0)(0))
    End Function
    Public Function ProductoMasVendido() As String
        strSQL = "SELECT TOP 1 p.nombre FROM DETALLE_PEDIDO dp INNER JOIN PRODUCTO p ON dp.idProducto = p.idProducto GROUP BY p.nombre ORDER BY SUM(dp.cantidad) DESC"
        Dim dt = objMan.listarComando(strSQL)
        Return If(dt.Rows.Count > 0, dt.Rows(0)("nombre").ToString(), "N/A")
    End Function
    Public Function VentasUltimos7Dias() As DataTable
        strSQL = "SELECT CONVERT(DATE, fecha) AS fecha, SUM(monto) AS totalVentas FROM PEDIDO WHERE fecha >= DATEADD(DAY, -7, GETDATE()) AND estadoPedido = 0 GROUP BY CONVERT(DATE, fecha) ORDER BY fecha ASC"
        Return objMan.listarComando(strSQL)
    End Function
    Public Function ContarPedidosPorEstado(estadoPago As Integer) As Integer
        strSQL = "SELECT COUNT(*) FROM PEDIDO WHERE estadoPago = " & estadoPago
        Return Convert.ToInt32(objMan.listarComando(strSQL).Rows(0)(0))
    End Function
    Public Function GetReporteDesempenoMeseros() As DataTable
        ' CORRECCIÓN: Ahora la consulta une con la tabla MESERO en lugar de USUARIO.
        ' y concatena los nombres y apellidos de esa tabla.
        strSQL = "SELECT (m.nombres + ' ' + m.apellidos) AS Empleado, COUNT(p.idPedido) AS CantidadPedidos, SUM(p.monto) AS TotalVendido " &
             "FROM PEDIDO p " &
             "INNER JOIN MESERO m ON p.idMesero = m.idMesero " &
             "WHERE p.estadoPedido = 0 " &
             "GROUP BY m.nombres, m.apellidos " &
             "ORDER BY TotalVendido DESC"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al generar reporte de meseros: " & ex.Message)
        End Try
    End Function

    Public Function GetReporteProductosVendidos() As DataTable
        ' Devuelve una lista de productos ordenados por la cantidad total vendida.
        strSQL = "SELECT TOP 10 p.nombre AS Producto, SUM(dp.cantidad) AS TotalCantidad " &
                 "FROM DETALLE_PEDIDO dp " &
                 "INNER JOIN PRODUCTO p ON dp.idProducto = p.idProducto " &
                 "GROUP BY p.nombre " &
                 "ORDER BY TotalCantidad DESC"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al generar reporte de productos: " & ex.Message)
        End Try
    End Function

    Public Function GetReporteVentasPorCategoria() As DataTable
        ' Devuelve el monto total de ventas agrupado por cada categoría de producto.
        strSQL = "SELECT tp.nombre AS Categoria, SUM(dp.cantidad * dp.precioVenta) AS MontoTotal " &
                 "FROM DETALLE_PEDIDO dp " &
                 "INNER JOIN PRODUCTO p ON dp.idProducto = p.idProducto " &
                 "INNER JOIN TIPO_PRODUCTO tp ON p.idTipo = tp.idTipo " &
                 "GROUP BY tp.nombre " &
                 "ORDER BY MontoTotal DESC"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al generar reporte de categorías: " & ex.Message)
        End Try
    End Function
End Class

Public Class DetalleDTO
    Public Property IdProducto As Integer
    Public Property Nombre As String
    Public Property Precio As Decimal
    Public Property Cantidad As Integer
End Class