
Class MySqlCommand

    Private _qUERY As String
    Private _cONN As MySql.Data.MySqlClient.MySqlConnection

    Sub New(QUERY As String, CONN As MySql.Data.MySqlClient.MySqlConnection)
        ' TODO: Complete member initialization 
        _qUERY = QUERY
        _cONN = CONN
    End Sub

    Function ExecuteReader() As MySql.Data.MySqlClient.MySqlDataReader
        Throw New NotImplementedException
    End Function

End Class
