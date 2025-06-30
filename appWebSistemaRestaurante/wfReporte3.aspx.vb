Imports System.Web.Services
Imports System.Data
Imports appWebSistemaRestaurante.srPedido

Public Class wfReporte3
    Inherits System.Web.UI.Page

    Public Class DataPoint
        Public Property value As Decimal
        Public Property name As String
    End Class

    Public Class ReporteProductosDTO
        Public Property NombresProductos As List(Of String)
        Public Property CantidadesVendidas As List(Of Integer)
        Public Property VentasPorCategoria As List(Of DataPoint)
    End Class

    <WebMethod()>
    Public Shared Function CargarDatosReporte() As ReporteProductosDTO
        Dim datosReporte As New ReporteProductosDTO()
        datosReporte.NombresProductos = New List(Of String)()
        datosReporte.CantidadesVendidas = New List(Of Integer)()
        datosReporte.VentasPorCategoria = New List(Of DataPoint)()

        Dim objPedido As New wsPedidoSoapClient()
        Try
            ' Cargar Top 10 Productos
            Dim dtTop As DataTable = objPedido.GetReporteProductosVendidos()
            For Each row As DataRow In dtTop.Rows
                datosReporte.NombresProductos.Add(row("Producto").ToString())
                datosReporte.CantidadesVendidas.Add(CInt(row("TotalCantidad")))
            Next

            ' Cargar Ventas por Categoría
            Dim dtCat As DataTable = objPedido.GetReporteVentasPorCategoria()
            For Each row As DataRow In dtCat.Rows
                datosReporte.VentasPorCategoria.Add(New DataPoint With {
                    .name = row("Categoria").ToString(),
                    .value = CDec(row("MontoTotal"))
                })
            Next
        Catch ex As Exception
            ' Manejar o loggear el error
        End Try

        Return datosReporte
    End Function
End Class