Imports System.Web.Services
Imports System.Data
Imports libNegocio

Public Class wfReporte2
    Inherits System.Web.UI.Page

    Public Class ReporteEmpleadosDTO
        Public Property NombresEmpleados As List(Of String)
        Public Property TotalesVenta As List(Of Decimal)
    End Class

    <WebMethod()>
    Public Shared Function CargarDatosReporte() As ReporteEmpleadosDTO
        Dim datosReporte As New ReporteEmpleadosDTO()
        datosReporte.NombresEmpleados = New List(Of String)()
        datosReporte.TotalesVenta = New List(Of Decimal)()

        Dim objPedido As New clsPedido()
        Try
            Dim dt As DataTable = objPedido.GetReporteDesempenoMeseros()
            For Each row As DataRow In dt.Rows
                datosReporte.NombresEmpleados.Add(row("Empleado").ToString())
                datosReporte.TotalesVenta.Add(CDec(row("TotalVendido")))
            Next
        Catch ex As Exception
            ' Manejar o loggear el error
        End Try

        Return datosReporte
    End Function
End Class