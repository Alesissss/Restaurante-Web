Imports libNegocio
Imports System.Security.Cryptography

Public Class Plantilla
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("usuario") Is Nothing Then
            Response.Redirect("wfLogin.aspx")
            Return
        End If

        Dim path As String = Request.Url.AbsolutePath.ToLower()

        If path.Contains("wfinicio.aspx") Then
            liInicio.Attributes("class") &= " menu-open"
            aInicio.Attributes("class") &= " active"
        End If

        ' ==== MANTENIMIENTOS ====
        If path.Contains("wfusuario.aspx") Then
            liUsuario.Attributes("class") &= " menu-open"
            aUsuario.Attributes("class") &= " active"
            aMantenimientos.Attributes("class") &= " active"
        ElseIf path.Contains("wftipousuario.aspx") Then
            liTipoUsuario.Attributes("class") &= " menu-open"
            aTipoUsuario.Attributes("class") &= " active"
            aMantenimientos.Attributes("class") &= " active"
        ElseIf path.Contains("wfcarta.aspx") Then
            liCarta.Attributes("class") &= " menu-open"
            aCarta.Attributes("class") &= " active"
            aMantenimientos.Attributes("class") &= " active"
        ElseIf path.Contains("wfmesa.aspx") Then
            liMesa.Attributes("class") &= " menu-open"
            aMesa.Attributes("class") &= " active"
            aMantenimientos.Attributes("class") &= " active"
        ElseIf path.Contains("wfmesero.aspx") Then
            liMesero.Attributes("class") &= " menu-open"
            aMesero.Attributes("class") &= " active"
            aMantenimientos.Attributes("class") &= " active"
        ElseIf path.Contains("wfcajero.aspx") Then
            liCajero.Attributes("class") &= " menu-open"
            aCajero.Attributes("class") &= " active"
            aMantenimientos.Attributes("class") &= " active"
        ElseIf path.Contains("wfproducto.aspx") Then
            liProducto.Attributes("class") &= " menu-open"
            aProducto.Attributes("class") &= " active"
            aMantenimientos.Attributes("class") &= " active"
        ElseIf path.Contains("wftipoproducto.aspx") Then
            liTipoProducto.Attributes("class") &= " menu-open"
            aTipoProducto.Attributes("class") &= " active"
            aMantenimientos.Attributes("class") &= " active"
        ElseIf path.Contains("wfcliente.aspx") Then
            liCliente.Attributes("class") &= " menu-open"
            aCliente.Attributes("class") &= " active"
            aMantenimientos.Attributes("class") &= " active"
        End If

        ' ==== OPERACIONES ====
        If path.Contains("wfpedidos.aspx") Then
            liPedidos.Attributes("class") &= " menu-open"
            aPedidos.Attributes("class") &= " active"
            aOperaciones.Attributes("class") &= " active"
        ElseIf path.Contains("wfaperturacaja.aspx") Then
            liApertura.Attributes("class") &= " menu-open"
            aApertura.Attributes("class") &= " active"
            aOperaciones.Attributes("class") &= " active"
        ElseIf path.Contains("wfcajacierre.aspx") Then
            liCierre.Attributes("class") &= " menu-open"
            aCierre.Attributes("class") &= " active"
            aOperaciones.Attributes("class") &= " active"
        End If

        ' ==== REPORTES ====
        If path.Contains("wfreporte1.aspx") Then
            liReporte1.Attributes("class") &= " menu-open"
            aReporte1.Attributes("class") &= " active"
            aReportes.Attributes("class") &= " active"
        ElseIf path.Contains("wfreporte2.aspx") Then
            liReporte2.Attributes("class") &= " menu-open"
            aReporte2.Attributes("class") &= " active"
            aReportes.Attributes("class") &= " active"
        ElseIf path.Contains("wfreporte3.aspx") Then
            liReporte3.Attributes("class") &= " menu-open"
            aReporte3.Attributes("class") &= " active"
            aReportes.Attributes("class") &= " active"
        End If
    End Sub
End Class