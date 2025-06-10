' Archivo: wfPedidos.aspx.vb (Versión Final)
Imports System.Web.Services
Imports System.Data
Imports libNegocio

Public Class wfPedidos
    Inherits System.Web.UI.Page

    ' --- DTOs (Data Transfer Objects) ---
    Public Class DatosInicialesDTO
        Public Property Mesas As List(Of MesaDTO)
        Public Property Menu As List(Of ProductoDTO)
        Public Property Clientes As List(Of ClienteDTO)
        Public Property Cajeros As List(Of CajeroDTO)
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

    Public Class CajeroDTO
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

    ' --- WEBMETHODS ---
    <WebMethod(EnableSession:=True)>
    Public Shared Function GetDatosIniciales() As DatosInicialesDTO
        Dim datos As New DatosInicialesDTO()
        Try
            ' Cargar Mesas
            datos.Mesas = New List(Of MesaDTO)()
            Dim objMesa As New clsMesa()
            Dim dtMesas As DataTable = objMesa.listarMesas()
            For Each row As DataRow In dtMesas.Rows
                datos.Mesas.Add(New MesaDTO With {
                    .Id = CInt(row("idMesa")), .Numero = CInt(row("numero")),
                    .EsVigente = CBool(row("vigencia")), .EstaDisponible = CBool(row("estadoMesa"))
                })
            Next

            ' Cargar Menú
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

            ' Cargar Clientes
            datos.Clientes = New List(Of ClienteDTO)
            Dim objCliente As New clsCliente()
            Dim dtClientes As DataTable = objCliente.listarClientesVigentes()
            datos.Clientes.Add(New ClienteDTO With {.Id = 1, .Nombre = "Cliente Varios"}) ' Añadir cliente por defecto
            For Each row As DataRow In dtClientes.Rows
                datos.Clientes.Add(New ClienteDTO With {
                    .Id = CInt(row("idCliente")), .Nombre = $"{row("nombres")} {row("apellidos")}"
                })
            Next

            ' Cargar Cajeros
            datos.Cajeros = New List(Of CajeroDTO)
            Dim objCajero As New clsCajero()
            Dim dtCajeros As DataTable = objCajero.listarCajerosVigentes()
            For Each row As DataRow In dtCajeros.Rows
                datos.Cajeros.Add(New CajeroDTO With {
                    .Id = CInt(row("idCajero")), .Nombre = $"{row("nombres")} {row("apellidos")}"
                })
            Next

        Catch ex As Exception
            Throw New Exception("Error cargando datos iniciales: " & ex.Message)
        End Try
        Return datos
    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function GetPedidoPorMesa(idMesa As Integer) As PedidoDTO
        Dim objPedido As New clsPedido()
        Try
            Dim dtPedidoActivo As DataTable = objPedido.BuscarPedidoActivoPorMesa(idMesa)
            If dtPedidoActivo IsNot Nothing AndAlso dtPedidoActivo.Rows.Count > 0 Then
                Dim pedidoRow = dtPedidoActivo.Rows(0)
                Dim pedidoResult As New PedidoDTO With {
                    .IdPedido = CInt(pedidoRow("idPedido")), .IdMesa = CInt(pedidoRow("idMesa")),
                    .IdCliente = CInt(pedidoRow("idCliente")), .IdMesero = CInt(pedidoRow("idMesero")),
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

    <WebMethod(EnableSession:=True)>
    Public Shared Function RegistrarOModificarPedido(pedido As PedidoDTO) As String
        Dim objPedido As New clsPedido()
        Try
            If pedido.IdPedido = 0 Then
                objPedido.RegistrarPedido(pedido.IdCliente, pedido.IdMesero, pedido.IdMesa, pedido.Items)
            Else
                Throw New NotSupportedException("La modificación de pedidos aún no está implementada en esta versión.")
            End If
            Return "ok"
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function ProcesarPago(idPedido As Integer, idCajero As Integer) As String
        Dim objPedido As New clsPedido()
        Try
            objPedido.ProcesarPago(idPedido, idCajero)
            Return "ok"
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

End Class