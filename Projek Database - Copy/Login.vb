Imports System.Data.Odbc

Public Class login

    Private Sub FormMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Button transparan
        Button1.FlatStyle = FlatStyle.Flat
        Button1.FlatAppearance.BorderSize = 0

        Button1.BackColor = Color.Transparent
        Button1.ForeColor = Color.White

        Button1.FlatAppearance.MouseOverBackColor = Color.Transparent
        Button1.FlatAppearance.MouseDownBackColor = Color.Transparent

        For Each lbl As Label In Me.Controls.OfType(Of Label)()

            lbl.Parent = Me
            lbl.BackColor = Color.Transparent

        Next

        Button1.UseVisualStyleBackColor = False

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Try
            Call Koneksi()

            Dim cmd As New OdbcCommand("SELECT * FROM tbl_user WHERE user_id='" & TextBox1.Text & "' AND password='" & TextBox2.Text & "'", conn)

            Dim rd As OdbcDataReader = cmd.ExecuteReader()
           If rd.Read() Then

                Module1.LoginUserID = rd("user_id").ToString()

                MessageBox.Show("Selamat datang, login berhasil!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Dasbor.Show()
                Me.Hide()

            Else

                MessageBox.Show("ID user atau password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        TextBox2.UseSystemPasswordChar = Not CheckBox1.Checked
    End Sub

    Private Sub Label38_Click(sender As Object, e As EventArgs) Handles Label38.Click
        LupaPassword.Show()

    End Sub
End Class