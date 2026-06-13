Imports System.Data
Imports System.Data.Odbc

Public Class Transaksi

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call Koneksi()

        SetButtonTransparent(Me)

        AutoKode()

        LoadMeja()

        Timer1.Enabled = False

        'Tanggal otomatis hari ini
        TextBox3.Text = Format(Now, "yyyy-MM-dd")
        TextBox4.Text = Format(Now, "yyyy-MM-dd")

        TextBox3.ReadOnly = True
        TextBox4.ReadOnly = True

        'Jam Mulai
        DateTimePicker2.Format = DateTimePickerFormat.Custom
        DateTimePicker2.CustomFormat = "HH:mm"
        DateTimePicker2.ShowUpDown = True

        'Jam Selesai
        DateTimePicker4.Format = DateTimePickerFormat.Custom
        DateTimePicker4.CustomFormat = "HH:mm"
        DateTimePicker4.ShowUpDown = True

        For Each lbl As Label In Me.Controls.OfType(Of Label)()

            lbl.Parent = Me
            lbl.BackColor = Color.Transparent

        Next

    End Sub

    Sub LoadMeja()

        ComboBox1.Items.Clear()

        Dim CMD As New OdbcCommand(
            "SELECT meja_id FROM tbl_meja WHERE status='Tersedia'",
            conn)

        Dim RD As OdbcDataReader

        RD = CMD.ExecuteReader()

        While RD.Read()

            ComboBox1.Items.Add(RD("meja_id").ToString())

        End While

        RD.Close()

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs)

        Try

            If TextBox1.Text = "" Or
               TextBox2.Text = "" Or
               ComboBox1.Text = "" Then

                MessageBox.Show("Lengkapi data terlebih dahulu")
                Exit Sub

            End If

            Dim query As String =
                "SELECT p.jenis_ps, p.tarif_per_jam " &
                "FROM tbl_meja m " &
                "INNER JOIN tbl_playstation p ON m.ps_id = p.ps_id " &
                "WHERE m.meja_id = ?"

            Dim CMD As New OdbcCommand(query, conn)

            CMD.Parameters.AddWithValue("@meja", ComboBox1.Text)

            Dim RD As OdbcDataReader

            RD = CMD.ExecuteReader()

            If RD.Read() Then

                Dim tarif As Double = CDbl(RD("tarif_per_jam"))
                Label21.Text = "Rp " & Format(tarif, "#,##0")

                'Jenis PS
                Label14.Text = RD("jenis_ps").ToString()

                'Nomor Meja
                Label15.Text = ComboBox1.Text

                'Tanggal & Jam
                Label16.Text = TextBox3.Text
                Label17.Text = DateTimePicker2.Value.ToString("HH:mm")

                Label18.Text = TextBox4.Text
                Label19.Text = DateTimePicker4.Value.ToString("HH:mm")

                'Hitung Durasi
                Dim jamMulai As DateTime = DateTimePicker2.Value
                Dim jamSelesai As DateTime = DateTimePicker4.Value

                'Buang detik
                jamMulai = New DateTime(
                    jamMulai.Year,
                    jamMulai.Month,
                    jamMulai.Day,
                    jamMulai.Hour,
                    jamMulai.Minute,
                    0)

                jamSelesai = New DateTime(
                    jamSelesai.Year,
                    jamSelesai.Month,
                    jamSelesai.Day,
                    jamSelesai.Hour,
                    jamSelesai.Minute,
                    0)

                Dim durasi As Integer = DateDiff(DateInterval.Hour, jamMulai, jamSelesai)

                If durasi <= 0 Then

                    MessageBox.Show("Jam selesai harus lebih besar dari jam mulai")
                    RD.Close()
                    Exit Sub

                End If

                Label20.Text = durasi & " Jam"

                Dim total As Double = durasi * tarif
                Label7.Text = "Rp " & Format(total, "#,##0")
                RD.Close()

                MessageBox.Show("Data berhasil diproses")

                Timer1.Start()

            Else

                RD.Close()

                MessageBox.Show("Data meja tidak ditemukan")

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Timer1.Stop()

        Pembayaran.Label14.Text = Label14.Text
        Pembayaran.Label15.Text = Label15.Text
        Pembayaran.Label16.Text = Label16.Text
        Pembayaran.Label17.Text = Label17.Text
        Pembayaran.Label18.Text = Label18.Text
        Pembayaran.Label19.Text = Label19.Text
        Pembayaran.Label20.Text = Label20.Text
        Pembayaran.Label21.Text = Label21.Text

        Pembayaran.Label7.Text = Label7.Text
        Pembayaran.Label22.Text = Label7.Text

        Pembayaran.Show()
        Me.Hide()

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)

        Bersih()

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

    Public Sub AutoKode()

        Call Koneksi()

        Dim tanggal As String = Format(Today, "yyyyMMdd")

        Dim query As String =
            "SELECT MAX(transaksi_id) FROM tbl_transaksi " &
            "WHERE transaksi_id LIKE 'TRX" & tanggal & "%'"

        Dim CMD As New OdbcCommand(query, conn)

        Dim hasil As Object = CMD.ExecuteScalar()

        If IsDBNull(hasil) Or hasil Is Nothing Then

            TextBox1.Text = "TRX" & tanggal & "001"

        Else

            Dim nomor As Integer

            nomor = Val(Microsoft.VisualBasic.Right(hasil.ToString(), 3)) + 1

            TextBox1.Text = "TRX" & tanggal & Format(nomor, "000")

        End If

    End Sub

    Public Sub Bersih()

        TextBox1.Clear()
        TextBox2.Clear()

        ComboBox1.SelectedIndex = -1

        TextBox3.Text = Format(Now, "yyyy-MM-dd")
        TextBox4.Text = Format(Now, "yyyy-MM-dd")

        Label14.Text = "-"
        Label15.Text = "-"

        Label16.Text = "-"
        Label17.Text = "-"

        Label18.Text = "-"
        Label19.Text = "-"

        Label20.Text = "-"

        Label21.Text = "-"

        Label7.Text = "Rp 0"

        AutoKode()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Dasbor.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        DataPlaystation.Show()
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

    Private Sub Button4_Click_1(sender As Object, e As EventArgs)
        Dasbor.Show()
        Me.Close()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        DataPlaystation.Show()
        Me.Close()
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs)
        LaporanPendapatan.Show()
        Me.Close()
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs)
        AkunSaya.Show()
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

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Public Sub RefreshMeja()
        LoadMeja()
    End Sub

    Private Sub Label22_Click(sender As Object, e As EventArgs) Handles Label22.Click
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

    Private Sub Label23_Click(sender As Object, e As EventArgs) Handles Label23.Click
        AkunSaya.Show()
        Me.Close()
    End Sub

    Private Sub Label24_Click(sender As Object, e As EventArgs) Handles Label24.Click
        LaporanPendapatan.Show()
        Me.Close()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label25_Click(sender As Object, e As EventArgs) Handles Label25.Click

    End Sub

    Private Sub Label26_Click(sender As Object, e As EventArgs) Handles Label26.Click
        DataPlaystation.Show()
        Me.Close()
    End Sub

    Private Sub Label27_Click(sender As Object, e As EventArgs) Handles Label27.Click
        Dasbor.Show()
        Me.Close()
    End Sub

    Private Sub Label28_Click(sender As Object, e As EventArgs) Handles Label28.Click
        Bersih()
    End Sub

    Private Sub Label29_Click(sender As Object, e As EventArgs) Handles Label29.Click

        Try

            If TextBox1.Text = "" Or
               TextBox2.Text = "" Or
               ComboBox1.Text = "" Then

                MessageBox.Show("Lengkapi data terlebih dahulu")
                Exit Sub

            End If

            Dim query As String =
                "SELECT p.jenis_ps, p.tarif_per_jam " &
                "FROM tbl_meja m " &
                "INNER JOIN tbl_playstation p ON m.ps_id = p.ps_id " &
                "WHERE m.meja_id = ?"

            Dim CMD As New OdbcCommand(query, conn)

            CMD.Parameters.AddWithValue("@meja", ComboBox1.Text)

            Dim RD As OdbcDataReader

            RD = CMD.ExecuteReader()

            If RD.Read() Then

                Dim tarif As Double = CDbl(RD("tarif_per_jam"))
                Label21.Text = "Rp " & Format(tarif, "#,##0")

                'Jenis PS
                Label14.Text = RD("jenis_ps").ToString()

                'Nomor Meja
                Label15.Text = ComboBox1.Text

                'Tanggal & Jam
                Label16.Text = TextBox3.Text
                Label17.Text = DateTimePicker2.Value.ToString("HH:mm")

                Label18.Text = TextBox4.Text
                Label19.Text = DateTimePicker4.Value.ToString("HH:mm")

                'Hitung Durasi
                Dim jamMulai As DateTime = DateTimePicker2.Value
                Dim jamSelesai As DateTime = DateTimePicker4.Value

                'Buang detik
                jamMulai = New DateTime(
                    jamMulai.Year,
                    jamMulai.Month,
                    jamMulai.Day,
                    jamMulai.Hour,
                    jamMulai.Minute,
                    0)

                jamSelesai = New DateTime(
                    jamSelesai.Year,
                    jamSelesai.Month,
                    jamSelesai.Day,
                    jamSelesai.Hour,
                    jamSelesai.Minute,
                    0)

                Dim durasi As Integer = DateDiff(DateInterval.Hour, jamMulai, jamSelesai)

                If durasi <= 0 Then

                    MessageBox.Show("Jam selesai harus lebih besar dari jam mulai")
                    RD.Close()
                    Exit Sub

                End If

                Label20.Text = durasi & " Jam"

                Dim total As Double = durasi * tarif
                Label7.Text = "Rp " & Format(total, "#,##0")
                RD.Close()

                MessageBox.Show("Data berhasil diproses")

                Timer1.Start()

            Else

                RD.Close()

                MessageBox.Show("Data meja tidak ditemukan")

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try
    End Sub
End Class