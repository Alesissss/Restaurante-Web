' Archivo: wfPedidos.aspx.vb (Versión Final Completa)
Imports System.Web.Services
Imports System.Data
Imports libNegocio
Imports System.Web

Public Class wfPedidos
    Inherits System.Web.UI.Page

    Public Class PaginaPedidosDTO
        Public Property Mesas As List(Of MesaDTO)
        Public Property Menu As List(Of ProductoDTO)
        Public Property Clientes As List(Of ClienteDTO)
        Public Property Cajeros As List(Of MeseroDTO)
        Public Property Meseros As List(Of MeseroDTO)
        Public Property Historial As List(Of PedidoHistorialDTO)
    End Class

    Public Class MesaDTO
        Public Property Id As Integer
        Public Property Numero As Integer
        Public Property EsVigente As Boolean
        Public Property EstaDisponible As Boolean
    End Class

    Public Class ProductoDTO
        Public Property IdProducto As Integer
        Public Property Nombre As String
        Public Property Precio As Decimal
        Public Property Categoria As String
    End Class

    Public Class ClienteDTO
        Public Property Id As Integer
        Public Property Nombre As String
    End Class

    Public Class MeseroDTO
        Public Property Id As Integer
        Public Property Nombre As String
    End Class

    Public Class PedidoDTO
        Public Property IdPedido As Integer
        Public Property IdMesa As Integer
        Public Property IdCliente As Integer
        Public Property IdMesero As Integer
        Public Property Items As List(Of DetalleDTO)
    End Class

    Public Class PedidoHistorialDTO
        Public Property IdPedido As Integer
        Public Property Fecha As String
        Public Property Mesa As Integer
        Public Property Cliente As String
        Public Property Mesero As String
        Public Property Cajero As String
        Public Property Monto As Decimal
        Public Property Estado As String
    End Class

    <WebMethod()>
    Public Shared Function GetPaginaPedidos() As PaginaPedidosDTO
        Dim datos As New PaginaPedidosDTO()
        Try
            datos.Mesas = New List(Of MesaDTO)()
            Dim objMesa As New clsMesa()
            Dim dtMesas As DataTable = objMesa.listarMesas()
            For Each row As DataRow In dtMesas.Rows
                datos.Mesas.Add(New MesaDTO With {
                    .Id = CInt(row("idMesa")), .Numero = CInt(row("numero")),
                    .EsVigente = CBool(row("vigencia")), .EstaDisponible = CBool(row("estadoMesa"))
                })
            Next

            datos.Menu = New List(Of ProductoDTO)()
            Dim objProducto As New clsProducto()
            Dim dtProductos As DataTable = objProducto.listarProductos()
            For Each row As DataRow In dtProductos.Rows
                If row("estado").ToString().Equals("Activo", StringComparison.OrdinalIgnoreCase) Then
                    datos.Menu.Add(New ProductoDTO With {
                        .IdProducto = CInt(row("idProducto")), .Nombre = row("nombre").ToString(),
                        .Precio = CDec(row("precio")), .Categoria = row("tipo_producto").ToString()
                    })
                End If
            Next

            datos.Clientes = New List(Of ClienteDTO)
            Dim objCliente As New clsCliente()
            Dim dtClientes As DataTable = objCliente.listarClientesVigentes()
            For Each row As DataRow In dtClientes.Rows
                datos.Clientes.Add(New ClienteDTO With {
                    .Id = CInt(row("idCliente")), .Nombre = $"{row("nombres")} {row("apellidos")}"
                })
            Next

            Dim objMesero As New clsMesero()
            Dim dtMeseros As DataTable = objMesero.listarMesero()

            datos.Meseros = New List(Of MeseroDTO)
            For Each row As DataRow In dtMeseros.Rows
                If row("estado") = "Activo" Then
                    datos.Meseros.Add(New MeseroDTO With {.Id = CInt(row("idMesero")), .Nombre = row("nombres").ToString() & " " & row("apellidos").ToString()})
                End If
            Next

            Dim objCajero As New clsCajero()
            Dim dtCajeros As DataTable = objCajero.listarCajerosVigentes()

            datos.Cajeros = New List(Of MeseroDTO)
            For Each row As DataRow In dtCajeros.Rows
                datos.Cajeros.Add(New MeseroDTO With {.Id = CInt(row("idCajero")), .Nombre = row("nombres").ToString() & " " & row("apellidos").ToString()})
            Next

            datos.Historial = New List(Of PedidoHistorialDTO)
            Dim objPedido As New clsPedido()
            Dim dtHistorial As DataTable = objPedido.ListarHistorialDePedidos()
            For Each row As DataRow In dtHistorial.Rows
                datos.Historial.Add(New PedidoHistorialDTO With {
                    .IdPedido = CInt(row("idPedido")), .Fecha = Convert.ToDateTime(row("fecha")).ToString("g"),
                    .Mesa = CInt(row("Mesa")), .Cliente = row("Cliente").ToString(),
                    .Mesero = row("Mesero").ToString(), .Cajero = row("Cajero").ToString(),
                    .Monto = CDec(row("monto")), .Estado = row("Estado").ToString()
                })
            Next

        Catch ex As Exception
            Throw New Exception("Error cargando datos de página: " & ex.Message)
        End Try
        Return datos
    End Function

    <WebMethod()>
    Public Shared Function GetPedidoPorMesa(idMesa As Integer) As PedidoDTO
        Dim objPedido As New clsPedido()
        Try
            Dim dtPedidoActivo As DataTable = objPedido.BuscarPedidoActivoPorMesa(idMesa)
            If dtPedidoActivo IsNot Nothing AndAlso dtPedidoActivo.Rows.Count > 0 Then
                Dim pedidoRow = dtPedidoActivo.Rows(0)
                Dim pedidoResult As New PedidoDTO With {
                    .IdPedido = CInt(pedidoRow("idPedido")), .IdMesa = CInt(pedidoRow("idMesa")),
                    .IdCliente = CInt(If(IsDBNull(pedidoRow("idCliente")), -1, pedidoRow("idCliente"))), .IdMesero = CInt(pedidoRow("idMesero")),
                    .Items = New List(Of DetalleDTO)()
                }

                Dim dtDetalles As DataTable = objPedido.ListarDetallesPorId(pedidoResult.IdPedido)
                For Each detRow As DataRow In dtDetalles.Rows
                    pedidoResult.Items.Add(New DetalleDTO With {
                        .IdProducto = CInt(detRow("idProducto")), .Nombre = detRow("nombre").ToString(),
                        .Precio = CDec(detRow("precioVenta")), .Cantidad = CInt(detRow("cantidad"))
                    })
                Next
                Return pedidoResult
            End If
        Catch ex As Exception
            Throw New Exception("Error al obtener pedido por mesa: " & ex.Message)
        End Try
        Return Nothing
    End Function

    <WebMethod()>
    Public Shared Function RegistrarOModificarPedido(pedido As PedidoDTO) As String
        Dim objPedido As New clsPedido()
        Try
            If pedido.IdPedido = 0 Then
                objPedido.RegistrarPedido(pedido.IdCliente, pedido.IdMesero, pedido.IdMesa, pedido.Items)
            Else
                objPedido.ActualizarPedido(pedido.IdPedido, pedido.Items)
            End If
            Return "ok"
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

    <WebMethod()>
    Public Shared Function ProcesarPago(idPedido As Integer, idCajero As Integer, idCliente As Integer) As String
        Dim objPedido As New clsPedido()
        Try
            objPedido.ProcesarPago(idPedido, idCajero, idCliente)
            Return "ok"
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

End Class