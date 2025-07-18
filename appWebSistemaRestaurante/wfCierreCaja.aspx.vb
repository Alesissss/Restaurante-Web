﻿Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Collections.Generic
Imports appWebSistemaRestaurante.srCierreCajaLogica

Public Class wfCierreCaja
    Inherits System.Web.UI.Page

    Dim logicaCierre As New wsCierreCajaLogicaSoapClient()

    Private Shared ReadOnly denominaciones As New Dictionary(Of String, Decimal) From {
        {"Billete de S/200", 200}, {"Billete de S/100", 100}, {"Billete de S/50", 50}, {"Billete de S/20", 20},
        {"Billete de S/10", 10}, {"Moneda de S/5", 5}, {"Moneda de S/2", 2}, {"Moneda de S/1", 1},
        {"Moneda de 0.20", 0.2D}, {"Moneda de 0.10", 0.1D}
    }

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtFechaCierre.Text = Date.Now.ToString("yyyy-MM-dd HH:mm")
            CargarCajeros()
            CargarCierres()
            InicializarDenominaciones()

        End If
    End Sub

    Private Sub CargarCierres()
        gvCierres.DataSource = logicaCierre.ListarCierres()
        gvCierres.DataBind()
    End Sub

    Private Sub CargarCajeros()
        ddlCajero.DataSource = logicaCierre.ListarCajeros()
        ddlCajero.DataTextField = "nombreCompleto"
        ddlCajero.DataValueField = "idCajero"
        ddlCajero.DataBind()
        ddlCajero.Items.Insert(0, New ListItem("-- Seleccione Cajero --", ""))
    End Sub

    Private Sub InicializarDenominaciones()
        Dim dt As New DataTable()
        dt.Columns.Add("Denominacion")
        dt.Columns.Add("Cantidad", GetType(Integer))
        dt.Columns.Add("Subtotal", GetType(Decimal))

        For Each kvp In denominaciones
            dt.Rows.Add(kvp.Key, 0, 0D)
        Next

        gvDenominaciones.DataSource = dt
        gvDenominaciones.DataBind()
    End Sub

    Protected Sub txtCantidad_TextChanged(sender As Object, e As EventArgs)
        CalcularTotales()
    End Sub

    Private Sub CalcularTotales()
        Dim total As Decimal = 0

        For Each row As GridViewRow In gvDenominaciones.Rows
            Dim txtCantidad As TextBox = CType(row.FindControl("txtCantidad"), TextBox)
            Dim cantidad As Integer = 0
            Integer.TryParse(txtCantidad.Text, cantidad)

            Dim denom As String = row.Cells(0).Text
            Dim valor As Decimal = denominaciones(denom)
            Dim subtotal As Decimal = cantidad * valor

            row.Cells(2).Text = subtotal.ToString("N2")
            total += subtotal
        Next

        txtMontoTotal.Text = total.ToString("N2")
        txtMontoTexto.Text = NumeroATexto(total)
    End Sub
    Protected Sub btnCerrarCaja_Click(sender As Object, e As EventArgs)
        Try
            Dim idCajero = Integer.Parse(ddlCajero.SelectedValue)
            Dim nombreUsuario = Session("nombreUsuario").ToString()
            Dim fecha = DateTime.Now
            Dim totalCierre = Decimal.Parse(txtMontoTotal.Text)

            ' 1) Creamos una lista de DetalleArqueoItem
            Dim listaDetalles As New List(Of DetalleArqueoItem)()

            For Each row As GridViewRow In gvDenominaciones.Rows
                Dim txtCantidad As TextBox = CType(row.FindControl("txtCantidad"), TextBox)
                Dim cantidad As Integer = 0
                Integer.TryParse(txtCantidad.Text, cantidad)

                Dim descripcion = row.Cells(0).Text
                Dim valor = denominaciones(descripcion)
                Dim subtotal = cantidad * valor

                If subtotal > 0 Then
                    listaDetalles.Add(
                    New DetalleArqueoItem() With {
                        .descripcion = descripcion,
                        .monto = subtotal
                    })
                End If
            Next

            ' 2) Convertimos la lista a array
            Dim detallesArray() As DetalleArqueoItem = listaDetalles.ToArray()

            ' 3) Invocamos al servicio pasando el array
            logicaCierre.CerrarCaja(
            idCajero,
            nombreUsuario,
            fecha,
            totalCierre,
            "Soles",
            detallesArray)

            ClientScript.RegisterStartupScript(
            Me.GetType(),
            "msg",
            "Swal.fire('Éxito','Caja cerrada correctamente','success');",
            True)

            CargarCierres()
        Catch ex As System.ServiceModel.FaultException
            ClientScript.RegisterStartupScript(
            Me.GetType(),
            "msg",
            $"Swal.fire('Error','Error en el servicio: {ex.Message}','error');",
            True)
        Catch ex As Exception
            ClientScript.RegisterStartupScript(
            Me.GetType(),
            "msg",
            $"Swal.fire('Error','{ex.Message}','error');",
            True)
        End Try
    End Sub

    Public Function NumeroATexto(ByVal numero As Decimal) As String
        Return "SOLES" ' Placeholder
    End Function

End Class
