Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class ValidationManager
    Public Shared Function camposLlenos(campos As Object()) As Boolean
        For Each campo In campos
            If String.IsNullOrWhiteSpace(campo) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function isNumber(cadena As String) As Boolean
        For Each c As Char In cadena
            If Not Char.IsDigit(c) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Class correo
        Public Shared Function esValido(correo As String) As Boolean
            Dim patron As String = "^[^@\s]+@[^@\s]+\.[^@\s]+$"
            Return Regex.IsMatch(correo, patron)
        End Function
    End Class

    Public Class dni
        Public Shared Function esValido(dni As String) As Boolean
            Dim patron As String = "^\d{8}$"
            Return Regex.IsMatch(dni, patron)
        End Function
    End Class

    Public Class telefono
        Public Shared Function esValido(dni As String) As Boolean
            Dim patron As String = "^\d{1,12}$"
            Return Regex.IsMatch(dni, patron)
        End Function
    End Class

    Public Class fecha
        Public Shared Function ValidarFechaNacimiento(datePicker As DateTimePicker) As Boolean
            Dim fechaNacimiento As DateTime = datePicker.Value
            Dim hoy As DateTime = DateTime.Today

            ' Validar que la fecha no sea futura
            If fechaNacimiento > hoy Then
                MessageBox.Show("La fecha de nacimiento no puede ser futura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            ' Calcular edad
            Dim edad As Integer = hoy.Year - fechaNacimiento.Year
            If (fechaNacimiento > hoy.AddYears(-edad)) Then
                edad -= 1
            End If

            ' Validar edad razonable (0 a 120 años)
            If edad < 0 Or edad > 120 Then
                MessageBox.Show("La edad no es válida. Debe estar entre 0 y 120 años.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            ' Si pasó todas las validaciones
            Return True
        End Function

    End Class

End Class
