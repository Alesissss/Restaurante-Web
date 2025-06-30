Imports System.Data
Imports System.Data.SqlClient
Imports libDatos

Public Class clsCierreCajaLogica
    Private objUsuario As New clsUsuario
    Private objCaja As New clsCajero
    Private objConexion As New clsConectaBD

    ' Devuelve la lista de cajeros activos (para DropDownList)
    Public Function ListarCajeros() As DataTable
        Return objCaja.listarCajerosCompleto()
    End Function

    ' Obtiene el ID del usuario por su nombre de sesión
    Private Function ObtenerIdUsuario(nombreUsuario As String) As Integer
        Dim dt As DataTable = objUsuario.buscarUsuarioPorNombre(nombreUsuario)
        If dt.Rows.Count > 0 Then
            Return Convert.ToInt32(dt.Rows(0)("idUduario"))
        Else
            Throw New Exception("Usuario no encontrado.")
        End If
    End Function

    ' Registra un nuevo cierre de caja como nuevo arqueo, y actualiza el estado del cajero a CERRADO
    Public Function CerrarCaja(idCajero As Integer, nombreUsuario As String, fecha As DateTime, total As Decimal, moneda As String, detalles As List(Of Tuple(Of String, Decimal))) As Integer
        Dim idGenerado As Integer = 0
        Dim idUsuario As Integer = 1
        Dim cadenaConexion As String = objConexion.gen_cad_cloud()

        Using conn As New SqlConnection(cadenaConexion)
            conn.Open()
            Dim trans = conn.BeginTransaction()

            Try
                ' Insertar cierre como nuevo ArqueoCaja
                Dim sqlCierre As String = "INSERT INTO ArqueoCaja (idCajero, idUsuario, fechaApertura, base, moneda, estado) " &
                                          "VALUES (@idCajero, @idUsuario, @fecha, @base, @moneda, @estado); " &
                                          "SELECT SCOPE_IDENTITY();"

                Using cmd As New SqlCommand(sqlCierre, conn, trans)
                    cmd.Parameters.AddWithValue("@idCajero", idCajero)
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario)
                    cmd.Parameters.AddWithValue("@fecha", fecha)
                    cmd.Parameters.AddWithValue("@base", total)
                    cmd.Parameters.AddWithValue("@moneda", moneda)
                    cmd.Parameters.AddWithValue("@estado", "CERRADO")
                    idGenerado = Convert.ToInt32(cmd.ExecuteScalar())
                End Using

                ' Insertar detalles
                For Each detalle In detalles
                    Dim sqlDetalle As String = "INSERT INTO DetalleArqueo (idArqueo, descripcion, monto) VALUES (@idArqueo, @descripcion, @monto);"
                    Using cmd As New SqlCommand(sqlDetalle, conn, trans)
                        cmd.Parameters.AddWithValue("@idArqueo", idGenerado)
                        cmd.Parameters.AddWithValue("@descripcion", detalle.Item1)
                        cmd.Parameters.AddWithValue("@monto", detalle.Item2)
                        cmd.ExecuteNonQuery()
                    End Using
                Next

                ' Actualizar estado del cajero a CERRADO
                Dim sqlCajero As String = "UPDATE Cajero SET estado = 0 WHERE idCajero = @idCajero"
                Using cmd As New SqlCommand(sqlCajero, conn, trans)
                    cmd.Parameters.AddWithValue("@idCajero", idCajero)
                    cmd.ExecuteNonQuery()
                End Using

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                Throw New Exception("Error en transacción de cierre: " & ex.Message)
            End Try
        End Using

        Return idGenerado
    End Function

    ' Lista los cierres de caja
    Public Function ListarCierres() As DataTable
        Dim sql As String = "
        SELECT A.idArqueo, 
               (C.nombres + ' ' + C.apellidos) AS nombreCajero, 
               A.fechaApertura AS fechaCierre, 
               A.base AS totalCierre, 
               A.estado
        FROM ArqueoCaja A 
        INNER JOIN Cajero C ON C.idCajero = A.idCajero 
        WHERE A.estado = 'CERRADO'
        ORDER BY A.fechaApertura DESC"

        Dim db As New clsMantenimiento()
        Return db.listarComando(sql)
    End Function

End Class
