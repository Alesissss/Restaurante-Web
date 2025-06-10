Imports System.Web.Services
Imports System.Text
Imports System.Data
Imports libNegocio ' Asegúrate que este sea el nombre de tu proyecto de capa lógica

Public Class wfProducto
    Inherits System.Web.UI.Page

    ' Instancias de las clases de negocio
    Private objProducto As New clsProducto()
    ' Asumo que estas clases existen en tu capa de lógica
    Private objTipoProducto As New clsTipoProducto()
    Private objCarta As New clsCarta()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Cargar los datos la primera vez que se abre la página
            CargarTiposProducto()
            CargarCartas()
            ListarProductos()
        End If
    End Sub

    ' --- MÉTODOS PARA CARGAR DATOS ---

    Private Sub CargarTiposProducto()
        Try
            ' Debes tener una clase clsTipoProducto con un método para listar
            ddlTipoProducto.DataSource = objTipoProducto.listarTiposProducto()
            ddlTipoProducto.DataValueField = "idTipo"
            ddlTipoProducto.DataTextField = "nombre"
            ddlTipoProducto.DataBind()
            ddlTipoProducto.Items.Insert(0, New ListItem("-- Seleccione Tipo --", "0"))
        Catch ex As Exception
            ' Manejo de errores
        End Try
    End Sub

    Private Sub CargarCartas()
        Try
            ddlCarta.DataSource = objCarta.listarCartas()
            ddlCarta.DataValueField = "idCarta"
            ddlCarta.DataTextField = "nombre"
            ddlCarta.DataBind()
            ddlCarta.Items.Insert(0, New ListItem("-- Seleccione Carta --", "0"))
        Catch ex As Exception
            ' Manejo de errores
        End Try
    End Sub

    Private Sub ListarProductos()
        Try
            ' El método listarProductos() ya trae los nombres de Tipo y Carta
            Dim dt As DataTable = objProducto.listarProductos()
            Dim sb As New StringBuilder()

            For Each row As DataRow In dt.Rows
                Dim id As Integer = CInt(row("idProducto"))
                Dim nombre As String = row("nombre").ToString()
                Dim descripcion As String = row("descripcion").ToString()
                Dim precio As Decimal = CDec(row("precio"))
                Dim idTipo As Integer = CInt(row("idTipo"))
                Dim idCarta As Integer = CInt(row("idCarta"))
                Dim nombreTipo As String = row("tipo_producto").ToString()
                Dim nombreCarta As String = row("carta").ToString()
                Dim estado As String = row("estado").ToString()
                Dim esVigente As Boolean = (estado.ToLower() = "activo")

                sb.Append("<tr>")
                sb.Append($"<td>{id}</td>")
                sb.Append($"<td>{nombre}</td>")
                sb.Append($"<td>S/ {precio:N2}</td>")
                sb.Append($"<td>{nombreTipo}</td>")
                sb.Append($"<td>{nombreCarta}</td>")

                If esVigente Then
                    sb.Append("<td><span class='badge badge-success'>Activo</span></td>")
                Else
                    sb.Append("<td><span class='badge badge-danger'>Inactivo</span></td>")
                End If

                ' Botones de acciones
                sb.Append("<td>")
                Dim p_nombre As String = nombre.Replace("'", "\'")
                Dim p_descripcion As String = descripcion.Replace("'", "\'")

                sb.Append($"<button type='button' class='btn btn-primary btn-sm' onclick=""fct_EditarProducto({id}, '{p_nombre}', '{p_descripcion}', {precio}, {idTipo}, {idCarta}, '{estado}')""><i class='fas fa-edit'></i></button> ")
                sb.Append($"<button type='button' class='btn btn-danger btn-sm' onclick='fct_EliminarProducto({id})'><i class='fas fa-trash'></i></button> ")

                If esVigente Then
                    sb.Append($"<button type='button' class='btn btn-warning btn-sm' onclick='fct_DarBajaProducto({id})'><i class='fas fa-arrow-down'></i></button>")
                Else
                    sb.Append($"<button type='button' class='btn btn-info btn-sm' onclick='fct_DarAltaProducto({id})'><i class='fas fa-arrow-up'></i></button>")
                End If
                sb.Append("</td>")
                sb.Append("</tr>")
            Next

            tbody_Producto.InnerHtml = sb.ToString()

        Catch ex As Exception
            tbody_Producto.InnerHtml = $"<tr><td colspan='7'>Error al listar productos: {ex.Message}</td></tr>"
        End Try
    End Sub

    ' --- WEBMETHODS ---

    <WebMethod>
    Public Shared Function GuardarProducto(id As Integer, nombre As String, descripcion As String, precio As Decimal, idTipo As Integer, idCarta As Integer, vigente As Boolean) As String
        Dim objP As New clsProducto()
        Try
            If id = 0 Then
                Dim newId As Integer = objP.generarIDProducto()
                objP.guardarProducto(newId, nombre, descripcion, precio, vigente, idTipo, idCarta)
            Else
                objP.modificarProducto(id, nombre, descripcion, precio, vigente, idTipo, idCarta)
            End If
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function EliminarProducto(id As Integer) As String
        Dim objP As New clsProducto()
        Try
            objP.eliminarProducto(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarBajaProducto(id As Integer) As String
        Dim objP As New clsProducto()
        Try
            objP.darBajaProducto(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Shared Function DarAltaProducto(id As Integer) As String
        Dim objP As New clsProducto()
        Try
            objP.darAltaProducto(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

End Class