Imports System.Data.Odbc

Public Class TambahkanPS
    Public CMD As Odbc.OdbcCommand
    Public DR As Odbc.OdbcDataReader
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("PS3")
        ComboBox1.Items.Add("PS4")
        ComboBox1.Items.Add("PS5")

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Call Koneksi()

        If TextBox1.Text = "" Then
            MessageBox.Show("ID PlayStation harus diisi!", "Peringatan")
            TextBox1.Focus()
            Exit Sub
        End If

        If ComboBox1.Text = "" Then
            MessageBox.Show("Pilih jenis PlayStation!", "Peringatan")
            ComboBox1.Focus()
            Exit Sub
        End If

        CMD = New OdbcCommand(
            "INSERT INTO playstation (id_ps, jenis_ps, status) VALUES (?,?,?)",
            Conn)

        CMD.Parameters.AddWithValue("@id", TextBox1.Text)
        CMD.Parameters.AddWithValue("@jenis", ComboBox1.Text)
        CMD.Parameters.AddWithValue("@status", "Tersedia")

        CMD.ExecuteNonQuery()

        MessageBox.Show("Data PlayStation berhasil ditambahkan!", "Informasi")

        DataPlaystation.Show()
        Me.Close()

    End Sub
End Class