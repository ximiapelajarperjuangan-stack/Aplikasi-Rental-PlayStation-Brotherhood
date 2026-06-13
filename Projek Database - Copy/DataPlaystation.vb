Imports System.Data
Imports System.Data.Odbc

Public Class DataPlaystation
    Public CMD As Odbc.OdbcCommand
    Public DR As Odbc.OdbcDataReader
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call Koneksi()

        TextBox1.ReadOnly = True

        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("Tersedia")
        ComboBox1.Items.Add("Error")


        TampilDataPS()

        For Each lbl As Label In Me.Controls.OfType(Of Label)()

            lbl.Parent = Me
            lbl.BackColor = Color.Transparent

        Next

    End Sub

    Sub TampilDataPS()

        Try

            Dim query As String = "SELECT m.meja_id AS 'No Meja', p.jenis_ps AS 'Jenis PS', m.status AS 'Status' FROM tbl_meja m INNER JOIN tbl_playstation p ON m.ps_id = p.ps_id ORDER BY m.meja_id"

            Dim DA As New OdbcDataAdapter(query, conn)

            Dim DS As New DataSet

            DA.Fill(DS)

            DataGridView1.DataSource = DS.Tables(0)

            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DataGridView1.ReadOnly = True
            DataGridView1.AllowUserToAddRows = False

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        If e.RowIndex >= 0 Then

            TextBox1.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString()

            ComboBox1.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value.ToString()

        End If

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)

        Try

            If TextBox1.Text = "" Then

                MessageBox.Show("Pilih meja terlebih dahulu")
                Exit Sub

            End If

            Dim statusLama As String = ""

            Dim CMDcek As New OdbcCommand("SELECT status FROM tbl_meja WHERE meja_id=?", conn)

            CMDcek.Parameters.AddWithValue("@meja", TextBox1.Text)

            statusLama = CMDcek.ExecuteScalar().ToString()

            If statusLama IsNot Nothing AndAlso statusLama.Trim().ToLower() = "digunakan" Then

                MessageBox.Show("Meja sedang digunakan dan tidak dapat diubah!")
                Exit Sub

            End If

            Dim CMD As New OdbcCommand("UPDATE tbl_meja SET status=? WHERE meja_id=?", conn)

            CMD.Parameters.AddWithValue("@status", ComboBox1.Text)
            CMD.Parameters.AddWithValue("@meja", TextBox1.Text)

            CMD.ExecuteNonQuery()

            MessageBox.Show("Status berhasil diperbarui")

            TampilDataPS()

            TextBox1.Clear()
            ComboBox1.SelectedIndex = -1

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Private Sub SetButtonTransparent(parent As Control)

        For Each ctrl As Control In parent.Controls

            If TypeOf ctrl Is Button Then

                Dim btn As Button = DirectCast(ctrl, Button)

                btn.FlatStyle = FlatStyle.Flat
                btn.FlatAppearance.BorderSize = 0

                btn.BackColor = Color.Transparent

                btn.FlatAppearance.MouseOverBackColor = Color.Transparent
                btn.FlatAppearance.MouseDownBackColor = Color.Transparent

            End If

            If ctrl.HasChildren Then

                SetButtonTransparent(ctrl)

            End If

        Next

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Dasbor.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs)
        Transaksi.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        LaporanPendapatan.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        AkunSaya.Show()
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

    Private Sub Button4_Click_1(sender As Object, e As EventArgs)
        Dasbor.Show()
        Me.Close()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)
        Transaksi.Show()
        Me.Close()
    End Sub

    Private Sub Button3_Click_2(sender As Object, e As EventArgs)
        LaporanPendapatan.Show()
        Me.Close()
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs)
        AkunSaya.Show()
        Me.Close()
    End Sub

    Public Sub RefreshData()
        TampilDataPS()
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Dasbor.Show()
        Me.Close()
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        Transaksi.Show()
        Me.Close()
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click
        LaporanPendapatan.Show()
        Me.Close()
    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        AkunSaya.Show()
        Me.Close()
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click
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

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        TambahkanPS.Show()
    End Sub

    Private Sub Button2_Click_2(sender As Object, e As EventArgs) Handles Button2.Click

        If TextBox1.Text = "" Then
            MessageBox.Show("Pilih data PlayStation yang akan dihapus!", "Peringatan")
            Exit Sub
        End If

        Dim jawab As DialogResult

        jawab = MessageBox.Show(
            "Yakin ingin menghapus data PlayStation ini?",
            "Konfirmasi Hapus",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If jawab = DialogResult.Yes Then

            Call Koneksi()

            CMD = New Odbc.OdbcCommand(
                "DELETE FROM playstation WHERE id_ps=?",
                Conn)

            CMD.Parameters.AddWithValue("@id", TextBox1.Text)

            CMD.ExecuteNonQuery()

            MessageBox.Show("Data berhasil dihapus!", "Informasi")

            'Kosongkan textbox
            TextBox1.Clear()

            'Refresh DataGridView jika ada prosedur tampil data
            Call TampilDataPS()

        End If

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button3_Click_3(sender As Object, e As EventArgs) Handles Button3.Click
        Try

            If TextBox1.Text = "" Then

                MessageBox.Show("Pilih meja terlebih dahulu")
                Exit Sub

            End If

            Dim statusLama As String = ""

            Dim CMDcek As New OdbcCommand("SELECT status FROM tbl_meja WHERE meja_id=?", conn)

            CMDcek.Parameters.AddWithValue("@meja", TextBox1.Text)

            statusLama = CMDcek.ExecuteScalar().ToString()

            If statusLama IsNot Nothing AndAlso statusLama.Trim().ToLower() = "digunakan" Then

                MessageBox.Show("Meja sedang digunakan dan tidak dapat diubah!")
                Exit Sub

            End If

            Dim CMD As New OdbcCommand("UPDATE tbl_meja SET status=? WHERE meja_id=?", conn)

            CMD.Parameters.AddWithValue("@status", ComboBox1.Text)
            CMD.Parameters.AddWithValue("@meja", TextBox1.Text)

            CMD.ExecuteNonQuery()

            MessageBox.Show("Status berhasil diperbarui")

            TampilDataPS()

            TextBox1.Clear()
            ComboBox1.SelectedIndex = -1

        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
    End Sub
End Class