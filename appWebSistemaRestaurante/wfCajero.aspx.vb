Imports System.Web.Services
Imports System.Web.Script.Services
Imports libNegocio

Partial Class wfCajero
    Inherits System.Web.UI.Page

    <WebMethod()>
    Public Shared Function GuardarCajero(id As String, dni As String, nom As String, ape As String, tel As String, cor As String, est As Boolean) As String
        Dim cls As New clsCajero()
        Try
            If String.IsNullOrEmpty(id) Then
                Dim nuevoId = cls.generarIDCajero()
                cls.guardarCajero(nuevoId, dni, nom, ape, tel, cor, est)
            Else
                cls.modificarCajero(Convert.ToInt32(id), dni, nom, ape, tel, cor, est)
            End If
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod()>
    Public Shared Function EliminarCajero(id As Integer) As String
        Dim cls As New clsCajero()
        Try
            cls.eliminarCajero(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod()>
    Public Shared Function DarBajaCajero(id As Integer) As String
        Dim cls As New clsCajero()
        Try
            cls.darBajaCajero(id)
            Return "success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
End Class
