' Archivo: clsPedido.vb (Versión Final)
Imports System.Data
Imports System.Data.SqlClient
Imports libDatos
Imports System.Linq

Public Class clsPedido
    Private objMan As New clsMantenimiento()
    Private objCon As New clsConectaBD()

    Public Function BuscarPedidoActivoPorMesa(ByVal idMesa As Integer) As DataTable
        Dim strSQL As String = "SELECT * FROM PEDIDO WHERE idMesa = @idMesa AND estadoPedido = 1"
        Dim parametros As New Dictionary(Of String, Object) From {{"@idMesa", idMesa}}
        Try
            Return objMan.listarComando(strSQL, parametros)
        Catch ex As Exception
            Throw New Exception("Error al buscar pedido activo por mesa: " & ex.Message)
        End Try
    End Function

    Public Function ListarDetallesPorId(ByVal idPedido As Integer) As DataTable
        Dim strSQL As String = "SELECT d.idProducto, p.nombre, d.cantidad, d.precioVenta FROM DETALLE_PEDIDO d " &
                             "INNER JOIN PRODUCTO p ON d.idProducto = p.idProducto " &
                             "WHERE d.idPedido = @idPedido"
        Dim parametros As New Dictionary(Of String, Object) From {{"@idPedido", idPedido}}
        Try
            Return objMan.listarComando(strSQL, parametros)
        Catch ex As Exception
            Throw New Exception("Error al listar detalles del pedido: " & ex.Message)
        End Try
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

                Dim sqlPedido As String = "INSERT INTO PEDIDO (idPedido, fecha, monto, estadoPedido, estadoPago, idCliente, idMesero, idMesa) " &
                                        "VALUES (@idPedido, GETDATE(), @monto, 1, 1, @idCliente, @idMesero, @idMesa)"
                Using cmd As New SqlCommand(sqlPedido, conn, transaction)
                    cmd.Parameters.AddWithValue("@idPedido", idPedido)
                    cmd.Parameters.AddWithValue("@monto", montoTotal)
                    cmd.Parameters.AddWithValue("@idCliente", idCliente)
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

    Public Sub ProcesarPago(ByVal idPedido As Integer, ByVal idCajero As Integer)
        Using conn As New SqlConnection(objCon.gen_cad_cloud())
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()
            Try
                Dim idMesa As Integer
                Using cmdMesaId As New SqlCommand("SELECT idMesa FROM PEDIDO WHERE idPedido = @idPedido", conn, transaction)
                    cmdMesaId.Parameters.AddWithValue("@idPedido", idPedido)
                    idMesa = CInt(cmdMesaId.ExecuteScalar())
                End Using

                Dim sqlPedido As String = "UPDATE PEDIDO SET estadoPago = 0, estadoPedido = 0, idCajero = @idCajero WHERE idPedido = @idPedido"
                Using cmd As New SqlCommand(sqlPedido, conn, transaction)
                    cmd.Parameters.AddWithValue("@idCajero", idCajero)
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
End Class

Public Class DetalleDTO
    Public Property IdProducto As Integer
    Public Property Nombre As String
    Public Property Precio As Decimal
    Public Property Cantidad As Integer
End Class