Imports System.Data.Odbc

Public Class AkunSaya

    Private PathFoto As String = ""

    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call Koneksi()

        SetButtonTransparent(Me)

        LoadProfil()

        TextBox1.ReadOnly = True   'Nama
        TextBox2.ReadOnly = True   'Role
        TextBox3.ReadOnly = True   'Password
        TextBox4.ReadOnly = True   'No HP

        TextBox3.UseSystemPasswordChar = True

        Label16.Enabled = False
        For Each btn As Button In Me.Controls.OfType(Of Button)()

            btn.FlatStyle = FlatStyle.Flat
            btn.FlatAppearance.BorderSize = 0

            btn.BackColor = Color.Transparent
            btn.ForeColor = Color.Transparent

            btn.FlatAppearance.MouseOverBackColor = Color.Transparent
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent

            For Each lbl As Label In Me.Controls.OfType(Of Label)()

                lbl.Parent = Me
                lbl.BackColor = Color.Transparent

            Next

            btn.TabStop = False

        Next

    End Sub

    Sub LoadProfil()

        Try

            Dim CMD As New OdbcCommand(
                "SELECT * FROM tbl_user WHERE user_id=?",
                conn)

            CMD.Parameters.AddWithValue("", Module1.LoginUserID)

            Dim RD As OdbcDataReader
            RD = CMD.ExecuteReader()

            If RD.Read() Then

                TextBox1.Text = RD("nama").ToString()
                TextBox2.Text = RD("role").ToString()
                TextBox3.Text = RD("password").ToString()
                TextBox4.Text = RD("no_hp").ToString()

                If Not IsDBNull(RD("foto")) Then

                    PathFoto = RD("foto").ToString()

                    If IO.File.Exists(PathFoto) Then

                        PictureBox1.Image = Image.FromFile(PathFoto)

                    End If

                End If

            End If

            RD.Close()

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)

        TextBox1.ReadOnly = False
        TextBox2.ReadOnly = True
        TextBox3.ReadOnly = False
        TextBox4.ReadOnly = False

        Label16.Enabled = True

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs)

        Try

            Dim CMD As New OdbcCommand(
"UPDATE tbl_user SET nama=?, password=?, no_hp=?, foto=? WHERE user_id=?",
conn)

            CMD.Parameters.AddWithValue("", TextBox1.Text)
            CMD.Parameters.AddWithValue("", TextBox3.Text)
            CMD.Parameters.AddWithValue("", TextBox4.Text)
            CMD.Parameters.AddWithValue("", PathFoto)
            CMD.Parameters.AddWithValue("", Module1.LoginUserID)

            CMD.ExecuteNonQuery()

            TextBox1.ReadOnly = True
            TextBox3.ReadOnly = True
            TextBox4.ReadOnly = True

            Label16.Enabled = False

            MessageBox.Show("Profil berhasil diperbarui")

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs)

        If MessageBox.Show(
            "Yakin ingin logout?",
            "Konfirmasi",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) = DialogResult.Yes Then

            login.Show()

            Me.Close()

        End If

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs)

        Dim OFD As New OpenFileDialog

        OFD.Filter = "File Gambar|*.jpg;*.jpeg;*.png"

        If OFD.ShowDialog = DialogResult.OK Then

            PathFoto = OFD.FileName

            PictureBox1.Image = Image.FromFile(PathFoto)

        End If

    End Sub

    Private Sub SetButtonTransparent(parent As Control)

        For Each ctrl As Control In parent.Controls

            If TypeOf ctrl Is Button Then

                Dim btn As Button = DirectCast(ctrl, Button)

                btn.FlatStyle = FlatStyle.Flat
                btn.FlatAppearance.BorderSize = 0

                btn.UseVisualStyleBackColor = False
                btn.BackColor = Color.Transparent

                btn.FlatAppearance.MouseOverBackColor = Color.Transparent
                btn.FlatAppearance.MouseDownBackColor = Color.Transparent

            End If

            If ctrl.HasChildren Then
                SetButtonTransparent(ctrl)
            End If

        Next

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Dasbor.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        DataPlaystation.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Transaksi.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        LaporanPendapatan.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        If MessageBox.Show(
            "Yakin ingin logout?",
            "Konfirmasi",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) = DialogResult.Yes Then

            login.Show()

            Me.Close()

        End If
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs)
        Dasbor.Show()
        Me.Close()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        DataPlaystation.Show()
        Me.Close()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)
        Transaksi.Show()
        Me.Close()
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs)
        LaporanPendapatan.Show()
        Me.Close()
    End Sub

    Private Sub Button6_Click_1(sender As Object, e As EventArgs)
        If MessageBox.Show(
           "Yakin ingin logout?",
           "Konfirmasi",
           MessageBoxButtons.YesNo,
           MessageBoxIcon.Question) = DialogResult.Yes Then

            login.Show()

            Me.Close()

        End If
    End Sub

    Private Sub Label14_Click(sender As Object, e As EventArgs) Handles Label14.Click
        Dasbor.Show()
        Me.Close()
    End Sub

    Private Sub Label13_Click(sender As Object, e As EventArgs) Handles Label13.Click
        DataPlaystation.Show()
        Me.Close()
    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click
        Transaksi.Show()
        Me.Close()
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click
        LaporanPendapatan.Show()
        Me.Close()
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click
        If MessageBox.Show(
          "Yakin ingin logout?",
          "Konfirmasi",
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question) = DialogResult.Yes Then

            login.TextBox1.Text = ""
            login.TextBox2.Text = ""

            login.Show()
            Me.Close()

        End If
    End Sub

    Private Sub Label18_Click(sender As Object, e As EventArgs) Handles Label18.Click
        Dim OFD As New OpenFileDialog

        OFD.Filter = "File Gambar|*.jpg;*.jpeg;*.png"

        If OFD.ShowDialog = DialogResult.OK Then

            PathFoto = OFD.FileName

            PictureBox1.Image = Image.FromFile(PathFoto)

        End If
    End Sub

    Private Sub Label15_Click(sender As Object, e As EventArgs) Handles Label15.Click

        TextBox1.ReadOnly = False
        TextBox2.ReadOnly = True
        TextBox3.ReadOnly = False
        TextBox4.ReadOnly = False

        Label16.Enabled = True
    End Sub

    Private Sub Label16_Click(sender As Object, e As EventArgs) Handles Label16.Click

        Try

            Dim CMD As New OdbcCommand(
"UPDATE tbl_user SET nama=?, password=?, no_hp=?, foto=? WHERE user_id=?",
conn)

            CMD.Parameters.AddWithValue("", TextBox1.Text)
            CMD.Parameters.AddWithValue("", TextBox3.Text)
            CMD.Parameters.AddWithValue("", TextBox4.Text)
            CMD.Parameters.AddWithValue("", PathFoto)
            CMD.Parameters.AddWithValue("", Module1.LoginUserID)

            CMD.ExecuteNonQuery()

            TextBox1.ReadOnly = True
            TextBox3.ReadOnly = True
            TextBox4.ReadOnly = True

            Label16.Enabled = False

            MessageBox.Show("Profil berhasil diperbarui")

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try
    End Sub

    Private Sub Label17_Click(sender As Object, e As EventArgs) Handles Label17.Click

        If MessageBox.Show(
            "Yakin ingin logout?",
            "Konfirmasi",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) = DialogResult.Yes Then

            login.TextBox1.Text = ""
            login.TextBox2.Text = ""

            login.Show()
            Me.Close()

        End If

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        TextBox3.UseSystemPasswordChar = Not CheckBox1.Checked()
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub
End Class