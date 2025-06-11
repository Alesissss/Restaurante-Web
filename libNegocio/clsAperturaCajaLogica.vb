Imports System.Data
Imports System.Data.SqlClient
Imports libDatos

Public Class clsAperturaCajaLogica
    Private objUsuario As New clsUsuario
    Private objCaja As New clsCajero
    Private objConexion As New clsConectaBD

    Public Function ListarCajeros() As DataTable
        Return objCaja.listarCajeros()
    End Function

    Private Function ObtenerIdUsuario(nombreUsuario As String) As Integer
        Dim dt As DataTable = objUsuario.buscarUsuarioPorNombre(nombreUsuario)
        If dt.Rows.Count > 0 Then
            Return Convert.ToInt32(dt.Rows(0)("idUduario"))
        Else
            Throw New Exception("Usuario no encontrado.")
        End If
    End Function

    Public Function AbrirCaja(idCajero As Integer, nombreUsuario As String, fecha As DateTime, base As Decimal, moneda As String, estado As String, detalles As List(Of Tuple(Of String, Decimal))) As Integer
        Dim idGenerado As Integer = 0
        Dim idUsuario As Integer = ObtenerIdUsuario(nombreUsuario)
        Dim cadenaConexion As String = objConexion.gen_cad_cloud()

        Using conn As New SqlConnection(cadenaConexion)
            conn.Open()
            Dim trans = conn.BeginTransaction()

            Try
                Dim sqlArqueo As String = "INSERT INTO ArqueoCaja (idCajero, idUsuario, fechaApertura, base, moneda, estado) 
                                           VALUES (@idCajero, @idUsuario, @fechaApertura, @base, @moneda, @estado);
                                           SELECT SCOPE_IDENTITY();"

                Using cmd As New SqlCommand(sqlArqueo, conn, trans)
                    cmd.Parameters.AddWithValue("@idCajero", idCajero)
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario)
                    cmd.Parameters.AddWithValue("@fechaApertura", fecha)
                    cmd.Parameters.AddWithValue("@base", base)
                    cmd.Parameters.AddWithValue("@moneda", moneda)
                    cmd.Parameters.AddWithValue("@estado", estado)
                    idGenerado = Convert.ToInt32(cmd.ExecuteScalar())
                End Using

                For Each detalle In detalles
                    Dim sqlDetalle As String = "INSERT INTO DetalleArqueo (idArqueo, descripcion, monto) 
                                                VALUES (@idArqueo, @descripcion, @monto);"
                    Using cmd As New SqlCommand(sqlDetalle, conn, trans)
                        cmd.Parameters.AddWithValue("@idArqueo", idGenerado)
                        cmd.Parameters.AddWithValue("@descripcion", detalle.Item1)
                        cmd.Parameters.AddWithValue("@monto", detalle.Item2)
                        cmd.ExecuteNonQuery()
                    End Using
                Next

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                Throw New Exception("Error en transacción: " & ex.Message)
            End Try
        End Using

        Return idGenerado
    End Function

    Public Function ListarAperturas() As DataTable
        Dim sql As String = "
        SELECT A.idArqueo, C.nombre AS nombreCajero, A.fechaApertura, A.base, A.estado
        FROM ArqueoCaja A
        INNER JOIN Cajero C ON C.idCajero = A.idCajero
        ORDER BY A.fechaApertura DESC"
        Dim db As New clsMantenimiento()
        Return db.listarComando(sql)
    End Function
End Class
