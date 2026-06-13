Imports System.Data.Odbc

Public Class LupaPassword
    Public CMD As Odbc.OdbcCommand
    Public DR As Odbc.OdbcDataReader

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Call Koneksi()

        'Cek data kosong
        If TextBox1.Text = "" Or
           TextBox2.Text = "" Or
           TextBox3.Text = "" Or
           TextBox4.Text = "" Or
           TextBox5.Text = "" Then

            MessageBox.Show("Semua data harus diisi!", "Peringatan")
            Exit Sub

        End If

        'Cek konfirmasi password
        If TextBox3.Text <> TextBox4.Text Then
            MessageBox.Show("Konfirmasi password tidak sesuai!", "Peringatan")
            Exit Sub
        End If

        'Verifikasi identitas pengguna
        CMD = New Odbc.OdbcCommand("SELECT * FROM user WHERE user_id=? AND nama=? AND no_hp=?", conn)

        CMD.Parameters.AddWithValue("@user_id", TextBox1.Text)
        CMD.Parameters.AddWithValue("@nama", TextBox2.Text)
        CMD.Parameters.AddWithValue("@no_hp", TextBox5.Text)

        DR = CMD.ExecuteReader()

        If DR.Read Then

            DR.Close()

            'Update password
            CMD = New Odbc.OdbcCommand("UPDATE user SET password=? WHERE user_id=?", conn)

            CMD.Parameters.AddWithValue("@password", TextBox3.Text)
            CMD.Parameters.AddWithValue("@user_id", TextBox1.Text)

            CMD.ExecuteNonQuery()

            MessageBox.Show("Password berhasil diubah!", "Informasi")

            login.TextBox1.Clear()
            login.TextBox2.Clear()
            login.TextBox1.Focus()

            login.Show()
            Me.Close()

        Else

            MessageBox.Show("ID Pengguna, Nama Lengkap, atau Nomor HP tidak sesuai!", "Gagal")

            DR.Close()

        End If

    End Sub

    Private Sub LupaPassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class