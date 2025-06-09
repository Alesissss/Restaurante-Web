'importaciones de espacios de nombre
'Imports System.Data 'ADO Net 
Imports System.Data.SqlClient

Public Class clsConectaBD
    'SqlConnectionStringBuilder: Permite armar la cadena de conexion 

    Private cn As SqlConnection

    Public Function gen_cad_cloud() As String
        Dim cad_con As New SqlConnectionStringBuilder
        cad_con.DataSource = "BD_RESTAURANTE_ARAE.mssql.somee.com"
        cad_con.InitialCatalog = "BD_RESTAURANTE_ARAE"
        cad_con.UserID = "Alesissss_SQLLogin_1"
        cad_con.Password = "gxnvcpejup"
        cad_con.IntegratedSecurity = False
        cad_con.PersistSecurityInfo = False
        cad_con.PacketSize = 4096
        cad_con.TrustServerCertificate = True
        cad_con("language") = "spanish"
        Return cad_con.ConnectionString
    End Function
    Public Function gen_cad_local() As String
        Dim cad_con As New SqlConnectionStringBuilder
        cad_con.DataSource = "localhost"
        cad_con.InitialCatalog = "BD_RESTAURANTE"
        cad_con.UserID = "sa"
        cad_con.Password = ""
        cad_con.IntegratedSecurity = True
        Return cad_con.ConnectionString
    End Function

    Sub New()
        cn = New SqlConnection
        cn.ConnectionString = gen_cad_cloud()
    End Sub

    Public Sub conectar()
        Try
            If cn.State = Data.ConnectionState.Closed Then
                cn.Open()
            End If
        Catch ex As Exception
            Throw New Exception("Error al conectar a BD")
        End Try
    End Sub

    Public Sub desconectar()
        Try
            If cn.State <> Data.ConnectionState.Closed Then
                cn.Close()
            End If
        Catch ex As Exception
            Throw New Exception("Error al desconectar a BD")
        End Try
    End Sub

    Public ReadOnly Property estadoCN() As String
        Get
            If cn.State = Data.ConnectionState.Open Then
                Return "BD está abierta."

            Else
                Return "BD está cerrada."
            End If
        End Get
    End Property

    Public ReadOnly Property miConexion() As SqlConnection
        Get
            Return cn
        End Get
    End Property

    Public ReadOnly Property Servidor() As String
        Get
            Return cn.DataSource.ToString
        End Get
    End Property

    Public Sub abrirconexion()
        Try
            'transaccion = False
            If cn.State <> Data.ConnectionState.Open Then ' SI EL ESTADO DE  LA CONEXION ES DIFERENTE DE ABIERTO ENTONCES ABRE LA CONEXION
                'cn.ConnectionString = gen_cad_local()
                cn.ConnectionString = gen_cad_cloud()
                cn.Open()
            End If
        Catch Ex As Exception
            Err.Raise(Err.Number, Err.Source, Err.Description)
        End Try
    End Sub

    Public Sub cerrarconexion()
        Try
            'transaccion = False
            If cn.State = Data.ConnectionState.Open Then
                cn.Close()
                cn.Dispose()
            End If
        Catch Ex As Exception
            Err.Raise(Err.Number, Err.Source, Err.Description)
        End Try
    End Sub

    Public Sub abrirconexionTrans()
        'Try
        '    If transaccion <> True Then
        '        abrirconexion()
        '        tsql = cn.BeginTransaction()
        '        transaccion = True
        '    End If
        'Catch ex As Exception
        '    Err.Raise(Err.Number, Err.Source, Err.Description)
        'End Try

    End Sub

    Public Sub cerrarconexionTrans()
        'Try
        '    If transaccion = True Then
        '        tsql.Commit()
        '        cerrarconexion()
        '        transaccion = False
        '    End If
        'Catch ex As Exception
        '    Err.Raise(Err.Number, Err.Source, Err.Description)
        'End Try
    End Sub

    Public Sub cancelarconexionTrans()
        'Try
        '    If transaccion = True Then
        '        tsql.Rollback()
        '        cerrarconexion()
        '        transaccion = False
        '    End If
        'Catch ex As Exception
        '    Err.Raise(Err.Number, Err.Source, Err.Description)
        'End Try
    End Sub
End Class
