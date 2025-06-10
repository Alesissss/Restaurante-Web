Imports System.Web.Services
Imports System.Web.Script.Services
Imports Newtonsoft.Json
Imports libNegocio
Imports System.Web.Script.Serialization

Public Class wfPedidos
    Inherits System.Web.UI.Page

    Private objPedido As New clsPedido()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTabla()
        End If
    End Sub
    Private Sub CargarTabla()
        Dim dt As DataTable = objPedido.listarPedidosVigentes()
        Dim html As New StringBuilder()
        Dim serializer As New JavaScriptSerializer()

        For Each row As DataRow In dt.Rows
            Dim id = row("idPedido")
            Dim fecha = Convert.ToDateTime(row("fecha")).ToString("yyyy-MM-dd HH:mm")
            Dim monto = String.Format("{0:0.00}", row("monto"))
            Dim estadoPago = If(row("estadoPago") = 0, "Pagado", "Pendiente")
            Dim numeroMesa = row("numeroMesa")
            Dim idCliente = row("idCliente")
            Dim idMesero = row("idMesero")
            Dim idMesa = row("idMesa")

            ' Serializar detalles del pedido
            Dim detalles As DataTable = objPedido.listarDetallesPedido(id)
            Dim listaDetalles As New List(Of Object)
            For Each dr As DataRow In detalles.Rows
                listaDetalles.Add(New With {
                .idProducto = Convert.ToInt32(dr("idProducto")),
                .nombre = dr("nombre").ToString(),
                .precio = Convert.ToDecimal(dr("precio")),
                .cantidad = Convert.ToInt32(dr("cantidad")),
                .carta = dr("carta").ToString(),
                .subtotal = Convert.ToDecimal(dr("subtotal"))
            })
            Next
            Dim detallesJson = serializer.Serialize(listaDetalles).Replace("""", "'")

            html.Append("<tr>")
            html.AppendFormat("<td>{0}</td>", id)
            html.AppendFormat("<td>{0}</td>", fecha)
            html.AppendFormat("<td>S/. {0}</td>", monto)
            html.AppendFormat("<td>{0}</td>", estadoPago)
            html.AppendFormat("<td>{0}</td>", numeroMesa)

            ' Insertar botón de editar con JSON seguro
            html.Append("<td>")
            html.AppendFormat("<button type='button' class='btn btn-primary btn-sm' onclick=""fct_EditarPedido({0}, {1}, {2}, {3}, {4})""><i class='fas fa-edit'></i></button>", id, idCliente, idMesero, idMesa, detallesJson)
            html.Append("</td>")
            html.Append("</tr>")
        Next

        tbody_Pedido.InnerHtml = html.ToString()
    End Sub

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function registrarPedidoWeb(idCliente As Integer, idMesero As Integer, idMesa As Integer, detalles As Object) As String
        Dim objPedido As New clsPedido()
        Dim nuevoID = objPedido.generarIDPedido()

        Try
            ' Convertimos la lista dinámica a DataTable
            Dim json = JsonConvert.SerializeObject(detalles)
            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(json)

            objPedido.RegistrarPedidoDesdeWeb(nuevoID, idCliente, idMesero, idMesa, dt)
            Return "Pedido registrado correctamente"
        Catch ex As Exception
            Throw New Exception("Error al registrar el pedido: " & ex.Message)
        End Try
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function modificarPedidoWeb(idPedido As Integer, idCliente As Integer, idMesero As Integer, idMesa As Integer, detalles As Object) As String
        Dim objPedido As New clsPedido()
        Try
            Dim json = JsonConvert.SerializeObject(detalles)
            Dim dt As DataTable = JsonConvert.DeserializeObject(Of DataTable)(json)

            objPedido.ModificarPedidoDesdeWeb(idPedido, idCliente, idMesero, idMesa, dt)
            Return "Pedido modificado correctamente"
        Catch ex As Exception
            Throw New Exception("Error al modificar el pedido: " & ex.Message)
        End Try
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function obtenerDetalles(idPedido As Integer) As Object
        Dim objPedido As New clsPedido()
        Try
            Dim dt As DataTable = objPedido.listarDetallesPedido(idPedido)
            Return dt
        Catch ex As Exception
            Throw New Exception("Error al obtener detalles: " & ex.Message)
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function listarClientes() As List(Of Object)
        Dim cls As New clsCliente()
        Dim dt As DataTable = cls.listarClientesVigentes()
        Dim lista = New List(Of Object)

        For Each row As DataRow In dt.Rows
            lista.Add(New With {
            .id = Convert.ToInt32(row("idCliente")),
            .nombre = row("nombres").ToString() & " " & row("apellidos").ToString()
        })
        Next

        Return lista
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function listarMeseros() As List(Of Object)
        Dim cls As New clsMesero()
        Dim dt As DataTable = cls.listarMesero()
        Dim lista = New List(Of Object)

        For Each row As DataRow In dt.Rows
            lista.Add(New With {
            .id = Convert.ToInt32(row("idMesero")),
            .nombre = row("nombres").ToString() & " " & row("apellidos").ToString()
        })
        Next

        Return lista
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function listarMesasDisponibles() As List(Of Object)
        Dim cls As New clsMesa()
        Dim dt As DataTable = cls.listarMesas()
        Dim lista = New List(Of Object)

        For Each row As DataRow In dt.Rows
            lista.Add(New With {
                .id = Convert.ToInt32(row("idMesa")),
                .nombre = "Mesa " & row("numero").ToString()
            })
        Next

        Return lista
    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function listarProductosVigentes() As List(Of Object)
        Dim cls As New clsProducto()
        Dim dt As DataTable = cls.listarProductos()
        Dim lista = New List(Of Object)

        For Each row As DataRow In dt.Rows
            If row("estado") = "Activo" Then
                lista.Add(New With {
                    .id = Convert.ToInt32(row("idProducto")),
                    .nombre = row("nombre").ToString(),
                    .precio = Convert.ToDecimal(row("precio"))
                })
            End If
        Next

        Return lista
    End Function

End Class
