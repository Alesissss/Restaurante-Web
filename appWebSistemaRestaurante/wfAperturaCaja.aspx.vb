Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Collections.Generic
Imports appWebSistemaRestaurante.srAperturaCajaLogica

Public Class wfAperturaCaja
    Inherits System.Web.UI.Page

    Dim logicaApertura As New wsAperturaCajaLogicaSoapClient()

    Private Shared ReadOnly denominaciones As New Dictionary(Of String, Decimal) From {
        {"Billete de S/200", 200}, {"Billete de S/100", 100}, {"Billete de S/50", 50}, {"Billete de S/20", 20},
        {"Billete de S/10", 10}, {"Moneda de S/5", 5}, {"Moneda de S/2", 2}, {"Moneda de S/1", 1},
        {"Moneda de 0.20", 0.2D}, {"Moneda de 0.10", 0.1D}
    }

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtFecha.Text = Date.Now.ToString("yyyy-MM-dd HH:mm")
            txtMoneda.Text = "Soles"
            txtUsuario.Text = Session("nombreUsuario")
            CargarCajeros()
            CargarAperturas()
            InicializarDenominaciones()
            CalcularTotales()
        End If
    End Sub
    Private Sub CargarAperturas()
        gvAperturas.DataSource = logicaApertura.ListarAperturas()
        gvAperturas.DataBind()
    End Sub
    Private Sub CargarCajeros()
        ddlCajero.DataSource = logicaApertura.ListarCajeros()
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
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            If String.IsNullOrEmpty(hfIdArqueo.Value) Then
                Dim idCajero = Integer.Parse(ddlCajero.SelectedValue)
                Dim nombreUsuario = "1"
                Dim fecha = DateTime.Now
                Dim montoBase = Decimal.Parse(txtMontoTotal.Text)

                ' 2) Creamos una lista de DetalleArqueoItem
                Dim listaDetalles As New List(Of DetalleArqueoItem)()

                For Each row As GridViewRow In gvDenominaciones.Rows
                    Dim txtCant As TextBox = CType(row.FindControl("txtCantidad"), TextBox)
                    Dim cantidad As Integer = 0
                    Integer.TryParse(txtCant.Text, cantidad)
                    Dim descripcion = row.Cells(0).Text
                    Dim valor = denominaciones(descripcion)
                    Dim subtotal = cantidad * valor

                    If subtotal > 0 Then
                        Dim item As New DetalleArqueoItem() With {
                        .descripcion = descripcion,
                        .monto = subtotal
                    }
                        listaDetalles.Add(item)
                    End If
                Next

                ' 3) Convertimos la lista a array
                Dim detallesArray() As DetalleArqueoItem = listaDetalles.ToArray()

                ' 4) Llamamos al servicio pasando el array
                logicaApertura.AbrirCaja(idCajero,
                                      nombreUsuario,
                                      fecha,
                                      montoBase,
                                      "Soles",
                                      "ABIERTO",
                                      detallesArray)

                ClientScript.RegisterStartupScript(Me.GetType(),
                "msg", "Swal.fire('Éxito','Caja aperturada correctamente','success');", True)
                CargarAperturas()
            End If
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(),
            "msg", $"Swal.fire('Error','{ex.Message}','error');", True)
        End Try
    End Sub

    Public Function NumeroATexto(ByVal numero As Decimal) As String
        Return "SOLES" ' Placeholder
    End Function

End Class
