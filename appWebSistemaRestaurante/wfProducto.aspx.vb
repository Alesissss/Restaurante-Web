Imports System.Web.Services
Imports System.Web.Script.Services
Imports System.Data
Imports libNegocio

Public Class wfProducto
    Inherits System.Web.UI.Page

    Shared objProducto As New clsProducto

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cargarTabla()
        End If
    End Sub

    Private Sub cargarTabla()
        Dim html As New StringBuilder()
        Dim dt As DataTable = objProducto.listarProductos()

        For Each row As DataRow In dt.Rows
            Dim id = row("idProducto").ToString()
            Dim nombre = row("nombre").ToString().Replace("'", "\'")
            Dim descripcion = row("descripcion").ToString().Replace("'", "\'")
            Dim precio = row("precio").ToString()
            Dim tipo = row("tipo_producto").ToString()
            Dim carta = row("carta").ToString()
            Dim estado = row("estado").ToString()
            Dim estadoBool = If(estado = "Activo", "true", "false")

            html.Append("<tr>")
            html.AppendFormat("<td>{0}</td>", id)
            html.AppendFormat("<td>{0}</td>", nombre)
            html.AppendFormat("<td>{0}</td>", descripcion)
            html.AppendFormat("<td>{0}</td>", precio)
            html.AppendFormat("<td>{0}</td>", tipo)
            html.AppendFormat("<td>{0}</td>", carta)
            html.AppendFormat("<td>{0}</td>", estado)
            html.Append("<td>")
            html.AppendFormat("<button class='btn btn-primary btn-sm' onclick=""fct_EditarProducto('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6})""><i class='fas fa-edit'></i></button> ", id, nombre, descripcion, precio, row("idTipo"), row("idCarta"), estadoBool)
            html.AppendFormat("<button class='btn btn-warning btn-sm' onclick=""fct_DarBajaProducto({0})""><i class='fas fa-arrow-down'></i></button> ", id)
            html.AppendFormat("<button class='btn btn-danger btn-sm' onclick=""fct_EliminarProducto({0})""><i class='fas fa-trash'></i></button>", id)
            html.Append("</td></tr>")
        Next

        tbody_Producto.InnerHtml = html.ToString()
        cargarCombos()
    End Sub

    Private Sub cargarCombos()
        ' Combo tipo producto
        Dim tipoProd As New clsTipoProducto()
        Dim dtTipo = tipoProd.listarTiposProducto()

        select_tipo.Items.Clear()
        For Each row As DataRow In dtTipo.Rows
            select_tipo.Items.Add(New ListItem(row("nombre").ToString(), row("idTipo").ToString()))
        Next

        ' Combo carta
        Dim carta As New clsCarta()
        Dim dtCarta = carta.listarCartas()

        select_carta.Items.Clear()
        For Each row As DataRow In dtCarta.Rows
            select_carta.Items.Add(New ListItem(row("nombre").ToString(), row("idCarta").ToString()))
        Next
    End Sub

    ' ---------------- WEB METHODS ----------------

    <WebMethod()>
    <ScriptMethod()>
    Public Shared Sub guardarProducto(id As Integer, nombre As String, descripcion As String, precio As Decimal, idTipo As Integer, idCarta As Integer, vigencia As Boolean)
        Dim obj As New clsProducto()
        If id = 0 Then
            id = obj.generarIDProducto()
            obj.guardarProducto(id, nombre, descripcion, precio, vigencia, idTipo, idCarta)
        End If
    End Sub

    <WebMethod()>
    <ScriptMethod()>
    Public Shared Sub modificarProducto(id As Integer, nombre As String, descripcion As String, precio As Decimal, idTipo As Integer, idCarta As Integer, vigencia As Boolean)
        Dim obj As New clsProducto()
        obj.modificarProducto(id, nombre, descripcion, precio, vigencia, idTipo, idCarta)
    End Sub

    <WebMethod()>
    <ScriptMethod()>
    Public Shared Sub eliminarProducto(id As Integer)
        Dim obj As New clsProducto()
        obj.eliminarProducto(id)
    End Sub

    <WebMethod()>
    <ScriptMethod()>
    Public Shared Sub darBajaProducto(id As Integer)
        Dim obj As New clsProducto()
        obj.darBajaProducto(id)
    End Sub
End Class
