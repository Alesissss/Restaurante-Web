Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports libDatos

Public Class clsPedido
    Private strSQL As String = ""
    Private objCliente As New clsCliente()
    Private objMesero As New clsMesero()
    Private objMesa As New clsMesa()
    Private objCajero As New clsCajero()
    Dim objMan As New clsMantenimiento()
    Dim objCon As New clsConectaBD()
    Dim dtPedido As New DataTable
    Public Function generarIDPedido() As Integer
        strSQL = "SELECT ISNULL(MAX(idPedido), 0) + 1 FROM PEDIDO"
        Try
            dtPedido = objMan.listarComando(strSQL)
            If dtPedido.Rows.Count > 0 Then
                Return Convert.ToInt32(dtPedido.Rows(0).Item(0))
            End If
        Catch ex As Exception
            Throw New Exception("Error al generar el ID de Pedido: " & ex.Message)
        End Try
        Return 0
    End Function
    Public Function calcularMonto(dgvDetalles As DataGridView) As Decimal
        Dim montoTotal As Single = 0
        For Each row As DataGridViewRow In dgvDetalles.Rows
            If Not row.IsNewRow Then
                montoTotal += Convert.ToSingle(row.Cells("Precio").Value) * Convert.ToInt32(row.Cells("Cantidad").Value)
            End If
        Next
        Return montoTotal
    End Function

    Public Function VerificarPedido(idPedido As Integer) As Boolean
        strSQL = "SELECT COUNT(*) FROM PEDIDO WHERE idPedido = @idPedido"
        Try
            Dim parametros As New Dictionary(Of String, Object) From {
                {"@idPedido", idPedido}
            }
            Dim dt As DataTable = objMan.listarComando(strSQL, parametros)
            If dt.Rows.Count > 0 Then
                Return Convert.ToInt32(dt.Rows(0).Item(0)) > 0
            End If
        Catch ex As Exception
            Throw New Exception("Error al verificar mesero: " & ex.Message)
        End Try
        Return False
    End Function

    Public Sub RegistrarPedidoTransaccional(idPedido As Integer, idCliente As Integer, idMesero As Integer, idMesa As Integer, dgvDetalles As DataGridView)
        If Not objCliente.VerificarCliente(idCliente) Then Throw New Exception("El cliente no existe.")
        If Not objMesero.VerificarMesero(idMesero) Then Throw New Exception("El mesero no existe.")
        If Not objMesa.VerificarMesa(idMesa) Then Throw New Exception("La mesa no existe o no se encuentra disponible.")

        Using conn As New SqlConnection("workstation id=BD_RESTAURANTE_ARAE.mssql.somee.com;packet size=4096;user id=Alesissss_SQLLogin_1;pwd=gxnvcpejup;data source=BD_RESTAURANTE_ARAE.mssql.somee.com;persist security info=False;initial catalog=BD_RESTAURANTE_ARAE;TrustServerCertificate=True; language=spanish")
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()
            Dim montoTotal = 0
            montoTotal = calcularMonto(dgvDetalles)
            Try
                ' Insertar Pedido
                strSQL = "INSERT INTO PEDIDO (idPedido, fecha, monto, estadoPedido, estadoPago, idCliente, idMesero, idMesa) 
                      VALUES (@idPedido, GETDATE(), @monto, '1', '1', @idCliente, @idMesero, @idMesa)"
                Using cmdPedido As New SqlCommand(strSQL, conn, transaction)
                    cmdPedido.Parameters.AddWithValue("@idPedido", idPedido)
                    cmdPedido.Parameters.AddWithValue("@monto", montoTotal)
                    cmdPedido.Parameters.AddWithValue("@idCliente", idCliente)
                    cmdPedido.Parameters.AddWithValue("@idMesero", idMesero)
                    cmdPedido.Parameters.AddWithValue("@idMesa", idMesa)
                    cmdPedido.ExecuteNonQuery()
                End Using

                ' Insertar Detalles
                strSQL = "INSERT INTO DETALLE_PEDIDO (idProducto, idPedido, cantidad, precioVenta) 
                      VALUES (@idProducto, @idPedido, @cantidad, @precioVenta)"
                For Each row As DataGridViewRow In dgvDetalles.Rows
                    If Not row.IsNewRow Then
                        Using cmdDetalle As New SqlCommand(strSQL, conn, transaction)
                            cmdDetalle.Parameters.AddWithValue("@idProducto", Convert.ToInt32(row.Cells("idProducto").Value))
                            cmdDetalle.Parameters.AddWithValue("@idPedido", idPedido)
                            cmdDetalle.Parameters.AddWithValue("@cantidad", Convert.ToInt32(row.Cells("Cantidad").Value))
                            cmdDetalle.Parameters.AddWithValue("@precioVenta", Convert.ToSingle(row.Cells("Precio").Value))
                            cmdDetalle.ExecuteNonQuery()
                        End Using
                    End If
                Next

                ' Cambiar estado de la mesa
                strSQL = "UPDATE MESA SET estado = 0 WHERE idMesa = @idMesa"
                Using cmdUpdateMesa As New SqlCommand(strSQL, conn, transaction)
                    cmdUpdateMesa.Parameters.AddWithValue("@idMesa", idMesa)
                    cmdUpdateMesa.ExecuteNonQuery()
                End Using

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw New Exception("Error al registrar pedido (se revirtió la transacción): " & ex.Message)
            End Try
        End Using
    End Sub
    Public Function listarPedidosVigentes() As DataTable
        strSQL = "SELECT idPedido, fecha, monto, estadoPedido, estadoPago,
                   idCliente, idMesero, idMesa,
                   (SELECT numero FROM MESA WHERE idMesa = ped.idMesa) AS numeroMesa
            FROM PEDIDO ped
            WHERE estadoPedido = 1"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar los pedidos vigentes: " & ex.Message)
        End Try
    End Function
    Public Sub PagarPedidoTransaccional(idPedido As Integer, idCajero As Integer, idCliente As Integer)
        If Not VerificarPedido(idPedido) Then Throw New Exception("El pedido no existe.")
        If Not objCajero.verificarCajero(idCajero) Then Throw New Exception("El cajero no existe.")
        If Not objCliente.VerificarCliente(idCliente) Then Throw New Exception("El cliente no existe.")

        Using conn As New SqlConnection(objCon.gen_cad_cloud())
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()
            Try
                ' Insertar Pedido
                strSQL = "UPDATE PEDIDO SET estadoPedido = 0, estadoPago = 0, idCliente = @idCliente, idCajero = @idCajero WHERE idPedido = @idPedido"
                Using cmdPedido As New SqlCommand(strSQL, conn, transaction)
                    cmdPedido.Parameters.AddWithValue("@idPedido", idPedido)
                    cmdPedido.Parameters.AddWithValue("@idCliente", idCliente)
                    cmdPedido.Parameters.AddWithValue("@idCajero", idCajero)
                    cmdPedido.ExecuteNonQuery()
                End Using

                ' Liberar mesa
                strSQL = "UPDATE MESA SET estado = 1 WHERE idMesa = (select idMesa from pedido where idPedido = @idPedido)"
                Using cmdMesa As New SqlCommand(strSQL, conn, transaction)
                    cmdMesa.Parameters.AddWithValue("@idPedido", idPedido)
                    cmdMesa.ExecuteNonQuery()
                End Using

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw New Exception("Error al pagar pedido (se revirtió la transacción): " & ex.Message)
            End Try
        End Using
    End Sub
    Public Function listarDetallesPedido(ByVal idPedido As Integer) As DataTable
        strSQL = "select p.idProducto, p.nombre, dped.precioVenta as precio, dped.cantidad, c.nombre as carta, (dped.precioVenta * dped.cantidad) as subtotal
                    FROM DETALLE_PEDIDO dped
                    INNER JOIN PRODUCTO p ON dped.idProducto = p.idProducto
                    INNER JOIN PEDIDO ped ON ped.idPedido = dped.idPedido
                    INNER JOIN CARTA c ON c.idCarta = p.idCarta
                    WHERE ped.estadoPedido = 1 AND ped.idPedido =" & idPedido
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar los detalles de pedido: " & ex.Message)
        End Try
    End Function
    Public Sub RegistrarPedidoDesdeWeb(idPedido As Integer, idCliente As Integer, idMesero As Integer, idMesa As Integer, detalles As DataTable)
        If Not objCliente.VerificarCliente(idCliente) Then Throw New Exception("Cliente inválido.")
        If Not objMesero.VerificarMesero(idMesero) Then Throw New Exception("Mesero inválido.")
        If Not objMesa.VerificarMesa(idMesa) Then Throw New Exception("Mesa no disponible.")

        Using conn As New SqlConnection(objCon.gen_cad_cloud())
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()

            Try
                Dim montoTotal As Decimal = detalles.AsEnumerable().Sum(Function(r) Convert.ToDecimal(r("precio")) * Convert.ToInt32(r("cantidad")))

                ' Insertar Pedido
                Dim strSQL = "INSERT INTO PEDIDO (idPedido, fecha, monto, estadoPedido, estadoPago, idCliente, idMesero, idMesa) 
                          VALUES (@idPedido, GETDATE(), @monto, 1, 1, @idCliente, @idMesero, @idMesa)"
                Using cmd As New SqlCommand(strSQL, conn, transaction)
                    cmd.Parameters.AddWithValue("@idPedido", idPedido)
                    cmd.Parameters.AddWithValue("@monto", montoTotal)
                    cmd.Parameters.AddWithValue("@idCliente", idCliente)
                    cmd.Parameters.AddWithValue("@idMesero", idMesero)
                    cmd.Parameters.AddWithValue("@idMesa", idMesa)
                    cmd.ExecuteNonQuery()
                End Using

                ' Insertar Detalles
                For Each row As DataRow In detalles.Rows
                    Dim strDet = "INSERT INTO DETALLE_PEDIDO (idProducto, idPedido, cantidad, precioVenta)
                              VALUES (@idProducto, @idPedido, @cantidad, @precio)"
                    Using cmdDet As New SqlCommand(strDet, conn, transaction)
                        cmdDet.Parameters.AddWithValue("@idProducto", Convert.ToInt32(row("idProducto")))
                        cmdDet.Parameters.AddWithValue("@idPedido", idPedido)
                        cmdDet.Parameters.AddWithValue("@cantidad", Convert.ToInt32(row("cantidad")))
                        cmdDet.Parameters.AddWithValue("@precio", Convert.ToDecimal(row("precio")))
                        cmdDet.ExecuteNonQuery()
                    End Using
                Next

                ' Actualizar estado de mesa
                Using cmdMesa As New SqlCommand("UPDATE MESA SET estado = 0 WHERE idMesa = @idMesa", conn, transaction)
                    cmdMesa.Parameters.AddWithValue("@idMesa", idMesa)
                    cmdMesa.ExecuteNonQuery()
                End Using

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw New Exception("Error al registrar pedido (WEB): " & ex.Message)
            End Try
        End Using
    End Sub

    Public Sub ModificarPedidoDesdeWeb(idPedido As Integer, idCliente As Integer, idMesero As Integer, idMesa As Integer, detalles As DataTable)
        Using conn As New SqlConnection(objCon.gen_cad_cloud())
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()
            Try
                ' Borrar detalles anteriores
                Using cmdDel As New SqlCommand("DELETE FROM DETALLE_PEDIDO WHERE idPedido = @idPedido", conn, transaction)
                    cmdDel.Parameters.AddWithValue("@idPedido", idPedido)
                    cmdDel.ExecuteNonQuery()
                End Using

                ' Recalcular monto
                Dim montoTotal As Decimal = detalles.AsEnumerable().Sum(Function(r) Convert.ToDecimal(r("precio")) * Convert.ToInt32(r("cantidad")))

                ' Actualizar pedido
                Dim sql = "UPDATE PEDIDO SET monto = @monto, idCliente = @idCliente, idMesero = @idMesero, idMesa = @idMesa WHERE idPedido = @idPedido"
                Using cmd As New SqlCommand(sql, conn, transaction)
                    cmd.Parameters.AddWithValue("@idPedido", idPedido)
                    cmd.Parameters.AddWithValue("@monto", montoTotal)
                    cmd.Parameters.AddWithValue("@idCliente", idCliente)
                    cmd.Parameters.AddWithValue("@idMesero", idMesero)
                    cmd.Parameters.AddWithValue("@idMesa", idMesa)
                    cmd.ExecuteNonQuery()
                End Using

                ' Insertar nuevos detalles
                For Each row As DataRow In detalles.Rows
                    Dim strDet = "INSERT INTO DETALLE_PEDIDO (idProducto, idPedido, cantidad, precioVenta)
                              VALUES (@idProducto, @idPedido, @cantidad, @precio)"
                    Using cmdDet As New SqlCommand(strDet, conn, transaction)
                        cmdDet.Parameters.AddWithValue("@idProducto", Convert.ToInt32(row("idProducto")))
                        cmdDet.Parameters.AddWithValue("@idPedido", idPedido)
                        cmdDet.Parameters.AddWithValue("@cantidad", Convert.ToInt32(row("cantidad")))
                        cmdDet.Parameters.AddWithValue("@precio", Convert.ToDecimal(row("precio")))
                        cmdDet.ExecuteNonQuery()
                    End Using
                Next

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw New Exception("Error al modificar pedido (WEB): " & ex.Message)
            End Try
        End Using
    End Sub


End Class