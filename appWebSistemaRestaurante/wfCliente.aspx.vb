Imports System.Web.Services
Imports System.Web.Script.Services
Imports System.Data
Imports appWebSistemaRestaurante.clsCliente
Imports libNegocio

Public Class wfCliente
    Inherits System.Web.UI.Page

    Dim objCliente As New clsCliente

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            cargarTabla()
        End If
    End Sub

    Private Sub cargarTabla()
        Dim dt As DataTable = objCliente.listarClientes()
        Dim html As New StringBuilder()

        For Each row As DataRow In dt.Rows
            Dim id = row("idCliente").ToString()
            Dim dni = row("dniCliente").ToString()
            Dim nombres = row("nombres").ToString()
            Dim apellidos = row("apellidos").ToString()
            Dim telefono = row("telefono").ToString()
            Dim correo = row("correo").ToString()
            Dim estado = row("estado").ToString()
            Dim estadoBool = If(estado = "Activo", "true", "false")

            html.Append("<tr>")
            html.AppendFormat("<td>{0}</td>", id)
            html.AppendFormat("<td>{0}</td>", dni)
            html.AppendFormat("<td>{0}</td>", nombres)
            html.AppendFormat("<td>{0}</td>", apellidos)
            html.AppendFormat("<td>{0}</td>", telefono)
            html.AppendFormat("<td>{0}</td>", correo)
            html.AppendFormat("<td>{0}</td>", estado)
            html.Append("<td>")
            html.AppendFormat("<button class='btn btn-primary btn-sm' onclick=""fct_EditarCliente('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6})""><i class='fas fa-edit'></i></button> ",
                              id, dni, nombres.Replace("'", "\'"), apellidos.Replace("'", "\'"),
                              telefono.Replace("'", "\'"), correo.Replace("'", "\'"), estadoBool)
            html.AppendFormat("<button class='btn btn-warning btn-sm' onclick=""fct_DarBajaCliente({0})""><i class='fas fa-arrow-down'></i></button> ", id)
            html.AppendFormat("<button class='btn btn-danger btn-sm' onclick=""fct_EliminarCliente({0})""><i class='fas fa-trash'></i></button>", id)
            html.Append("</td>")
            html.Append("</tr>")
        Next

        tbody_Cliente.InnerHtml = html.ToString()
    End Sub

    ' ================== MÉTODOS AJAX =====================

    <WebMethod()>
    <ScriptMethod()>
    Public Shared Sub guardarCliente(id As Integer, dni As String, nom As String, ape As String, tel As String, cor As String, est As Boolean)
        Dim obj As New clsCliente
        Dim nuevoId = If(id = 0, obj.generarIDCliente(), id)
        obj.guardarCliente(nuevoId, dni, nom, ape, tel, cor, est)
    End Sub

    <WebMethod()>
    <ScriptMethod()>
    Public Shared Sub modificarCliente(id As Integer, dni As String, nom As String, ape As String, tel As String, cor As String, est As Boolean)
        Dim obj As New clsCliente
        obj.modificarCliente(id, dni, nom, ape, tel, cor, est)
    End Sub

    <WebMethod()>
    <ScriptMethod()>
    Public Shared Sub eliminarCliente(id As Integer)
        Dim obj As New clsCliente
        obj.eliminarCliente(id)
    End Sub

    <WebMethod()>
    <ScriptMethod()>
    Public Shared Sub darBajaCliente(id As Integer)
        Dim obj As New clsCliente
        obj.darBajaCliente(id)
    End Sub
End Class
