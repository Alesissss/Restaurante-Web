Imports libDatos

Public Class clsCarta
    Private objMan As New clsMantenimiento()
    Private strSQL As String = ""
    Private dtCarta As New DataTable()

    Public Function generarIDCarta() As Integer
        strSQL = "SELECT ISNULL(MAX(idCarta), 0) + 1 FROM CARTA"
        Try
            dtCarta = objMan.listarComando(strSQL)
            If dtCarta.Rows.Count > 0 Then
                Return Convert.ToInt32(dtCarta.Rows(0).Item(0))
            End If
        Catch ex As Exception
            Throw New Exception("Error al generar el ID de Carta: " & ex.Message)
        End Try
        Return 0
    End Function

    Public Sub guardarCarta(ByVal id As Integer, ByVal nom As String, ByVal desc As String, ByVal vig As Boolean)
        Dim descripcionSQL As String = If(String.IsNullOrEmpty(desc), "NULL", "'" & desc.Replace("'", "''") & "'")

        strSQL = "INSERT INTO CARTA (idCarta, nombre, descripcion, vigencia) VALUES (" &
                 id & ", '" &
                 nom.Replace("'", "''") & "', " &
                 descripcionSQL & ", " &
                 IIf(vig, 1, 0) & ")"
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al registrar Carta: " & ex.Message)
        End Try
    End Sub

    Public Sub modificarCarta(ByVal id As Integer, ByVal nom As String, ByVal desc As String, ByVal vig As Boolean)
        Dim descripcionSQL As String = If(String.IsNullOrEmpty(desc), "NULL", "'" & desc.Replace("'", "''") & "'")

        strSQL = "UPDATE CARTA SET " &
                 "nombre = '" & nom.Replace("'", "''") & "', " &
                 "descripcion = " & descripcionSQL & ", " &
                 "vigencia = " & IIf(vig, 1, 0) & " " &
                 "WHERE idCarta = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al modificar Carta: " & ex.Message)
        End Try
    End Sub

    Public Sub eliminarCarta(ByVal id As Integer)
        strSQL = "DELETE FROM CARTA WHERE idCarta = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al eliminar Carta: " & ex.Message)
        End Try
    End Sub

    Public Sub darBajaCarta(ByVal id As Integer)
        strSQL = "UPDATE CARTA SET vigencia = 0 WHERE idCarta = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de baja a Carta: " & ex.Message)
        End Try
    End Sub

    Public Sub darAltaCarta(ByVal id As Integer)
        strSQL = "UPDATE CARTA SET vigencia = 1 WHERE idCarta = " & id
        Try
            objMan.ejecutarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al dar de alta a Carta: " & ex.Message)
        End Try
    End Sub

    Public Function buscarCarta(ByVal id As Integer) As DataTable
        strSQL = "SELECT * FROM CARTA WHERE idCarta = " & id
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar Carta: " & ex.Message)
        End Try
    End Function

    Public Function buscarCartaPorNombre(ByVal nombreBusqueda As String) As DataTable
        strSQL = "SELECT * FROM CARTA WHERE nombre = '" & nombreBusqueda.Replace("'", "''") & "'"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al buscar Carta por nombre: " & ex.Message)
        End Try
    End Function

    Public Function listarCartas() As DataTable
        strSQL = "SELECT idCarta,nombre, descripcion, " &
         "CASE vigencia " &
         "WHEN 1 THEN 'Activo' " &
         "WHEN 0 THEN 'Inactivo' " &
         "ELSE 'Desconocido' END AS estado " &
         "FROM CARTA"
        Try
            Return objMan.listarComando(strSQL)
        Catch ex As Exception
            Throw New Exception("Error al listar Cartas: " & ex.Message)
        End Try
    End Function

    Public Function VerificarCarta(ByVal idCartaVerificar As Integer) As Boolean
        strSQL = "SELECT COUNT(*) FROM CARTA WHERE idCarta = @idCarta"
        Try
            Dim parametros As New Dictionary(Of String, Object) From {
                {"@idCarta", idCartaVerificar}
            }
            Dim dt As DataTable = objMan.listarComando(strSQL, parametros)
            If dt.Rows.Count > 0 Then
                Return Convert.ToInt32(dt.Rows(0).Item(0)) > 0
            End If
        Catch ex As Exception
            Throw New Exception("Error al verificar carta: " & ex.Message)
        End Try
        Return False
    End Function
End Class