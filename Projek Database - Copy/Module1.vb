Imports System.Data.Odbc

Module Module1

    Public conn As New OdbcConnection("DSN=rental_playstation")
    Public LoginUserID As String

    Public Sub Koneksi()
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
    End Sub

End Module