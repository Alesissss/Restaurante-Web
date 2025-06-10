' Archivo: clsMesa.vb
Imports libDatos

Public Class clsMesa
    Private objMan As New clsMantenimiento()
    Private strSQL As String = ""
    Private dtMesa As New DataTable()

    Public Function generarIDMesa() As Integer
        strSQL = "SELECT ISNULL(MAX(idMesa), 0) + 1 FROM MESA"
        Try
            dtMesa = objMan.listarComando(strSQL)
            If dtMesa.Rows.Count > 0 Then
                Return Convert.ToInt32(dtMesa.Rows(0).Item(0))
            End If
        Catch ex As Exception
            Throw New Exception("Error al generar el ID de Mesa: " & ex.Message)
        End Try
        Return 0
    End Function

    ' Maneja VIGENCIA. Deja que estadoMesa sea manejado por la BD (default) o por la lógica de pedidos.
    Public Sub guardarMesa(ByVal id As Integer, ByVal numero As Integer, ByVal capacidad As Byte, ByVal vig As Boolean)
        strSQL = "INSERT INTO MESA (idMesa, numero, capacidad, vigencia) VALUES (" &
                 id & ", " &
                 numero & ", " &
                 capacidad & ", " &
                 IIf(vig, 1, 0) & ")"
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al registrar Mesa: " & ex.Message)
        End Try
    End Sub

    ' Maneja VIGENCIA. No toca estadoMesa.
    Public Sub modificarMesa(ByVal id As Integer, ByVal numero As Integer, ByVal capacidad As Byte, ByVal vig As Boolean)
        strSQL = "UPDATE MESA SET " &
                 "numero = " & numero & ", " &
                 "capacidad = " & capacidad & ", " &
                 "vigencia = " & IIf(vig, 1, 0) & " " &
                 "WHERE idMesa = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al modificar Mesa: " & ex.Message)
        End Try
    End Sub

    Public Sub eliminarMesa(ByVal id As Integer)
        strSQL = "DELETE FROM MESA WHERE idMesa = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al eliminar Mesa: " & ex.Message)
        End Try
    End Sub

    Public Sub darBajaMesa(ByVal id As Integer)
        strSQL = "UPDATE MESA SET vigencia = 0 WHERE idMesa = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de baja a Mesa: " & ex.Message)
        End Try
    End Sub

    Public Sub darAltaMesa(ByVal id As Integer)
        strSQL = "UPDATE MESA SET vigencia = 1 WHERE idMesa = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de alta a Mesa: " & ex.Message)
        End Try
    End Sub

    ' Selecciona las dos columnas de estado para usarlas en la interfaz
    Public Function listarMesas() As DataTable
        strSQL = "SELECT idMesa, numero, capacidad, vigencia, estadoMesa FROM MESA"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar Mesas: " & ex.Message)
        End Try
    End Function

    Public Function buscarMesa(ByVal id As Integer) As DataTable
        strSQL = "SELECT * FROM MESA WHERE idMesa = " & id
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar Mesa: " & ex.Message)
        End Try
    End Function

    Public Function VerificarMesa(idMesa As Integer) As Boolean
        strSQL = "SELECT COUNT(*) FROM MESA WHERE idMesa = @idMesa AND estado=1"
        Try
            Dim parametros As New Dictionary(Of String, Object) From {
                {"@idMesa", idMesa}
            }
            Dim dt As DataTable = objMan.listarComando(strSQL, parametros)
            If dt.Rows.Count > 0 Then
                Return Convert.ToInt32(dt.Rows(0).Item(0)) > 0
            End If
        Catch ex As Exception
            Throw New Exception("Error al verificar mesa: " & ex.Message)
        End Try
        Return False
    End Function
End Class