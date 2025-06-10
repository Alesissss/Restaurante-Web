Imports System.Web.Script.Services
Imports System.Web.Services
Imports libNegocio
Imports Newtonsoft.Json

<ScriptService()>
Partial Class wfReporte1
    Inherits System.Web.UI.Page

    <WebMethod()>
    Public Shared Function GetIndicadores() As Object
        Dim obj As New clsPedido()
        Dim pedidosDia = obj.ContarPedidosHoy()
        Dim pendientes = obj.ContarPedidosPendientes()
        Dim productoTop = obj.ProductoMasVendido()
        Return New With {.pedidosDia = pedidosDia, .pedidosPendientes = pendientes, .productoMasVendido = productoTop}
    End Function

    <WebMethod()>
    Public Shared Function GetGraficoEstadoPedidos() As Object
        Dim obj As New clsPedido()
        Dim pagado = obj.ContarPedidosPorEstado(0)
        Dim pendiente = obj.ContarPedidosPorEstado(1)
        Return New With {.pagado = pagado, .pendiente = pendiente}
    End Function

    <WebMethod()>
    Public Shared Function GetGraficoVentasDiarias() As Object
        Dim obj As New clsPedido()
        Dim dt As DataTable = obj.VentasUltimos7Dias()
        Dim fechas As New List(Of String)
        Dim ventas As New List(Of Decimal)
        For Each row As DataRow In dt.Rows
            fechas.Add(Convert.ToDateTime(row("fecha")).ToString("dd/MM"))
            ventas.Add(Convert.ToDecimal(row("monto")))
        Next
        Return New With {.fechas = fechas, .ventas = ventas}
    End Function
End Class
