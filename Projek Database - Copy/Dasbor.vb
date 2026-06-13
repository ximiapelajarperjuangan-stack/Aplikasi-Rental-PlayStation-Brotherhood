Imports System.Data.Odbc
Imports System.Collections.Generic

Public Class Dasbor
    Private lastPopup As DateTime = DateTime.MinValue
    Private notified10Min As New HashSet(Of String)
    Private notifiedExpired As New HashSet(Of String)
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call Koneksi()

        UpdateTransaksiExpired()

        LoadDashboard()
        LoadStatusMeja()
        LoadSisaWaktu()

        Timer1.Interval = 5000 'cek tiap 5 detik
        Timer1.Start()

        For Each btn As Button In Me.Controls.OfType(Of Button)()

            btn.FlatStyle = FlatStyle.Flat
            btn.FlatAppearance.BorderSize = 0

            btn.BackColor = Color.Transparent
            btn.ForeColor = SystemColors.ControlText

            btn.FlatAppearance.MouseOverBackColor = Color.Transparent
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent

            For Each lbl As Label In Me.Controls.OfType(Of Label)()

                lbl.Parent = Me
                lbl.BackColor = Color.Transparent

            Next

            btn.TabStop = False

        Next
    End Sub

    Private Sub LoadDashboard()

        Dim cmd As OdbcCommand

        ' Meja tersedia
        cmd = New OdbcCommand("SELECT COUNT(*) FROM tbl_meja WHERE status='Tersedia'", conn)
        Label1.Text = cmd.ExecuteScalar().ToString()

        ' Meja dipakai
        cmd = New OdbcCommand("SELECT COUNT(*) FROM tbl_meja WHERE status='Digunakan'", conn)
        Label5.Text = cmd.ExecuteScalar().ToString()

        ' Transaksi hari ini
        cmd = New OdbcCommand("SELECT COUNT(*) FROM tbl_transaksi WHERE tanggal=CURDATE()", conn)
        Label11.Text = cmd.ExecuteScalar().ToString()

        ' Pendapatan hari ini
        cmd = New OdbcCommand("SELECT IFNULL(SUM(total_biaya),0) FROM tbl_transaksi WHERE tanggal=CURDATE()", conn)
        Label12.Text = "Rp " & Format(CDbl(cmd.ExecuteScalar()), "#,##0")

    End Sub

    Private Sub LoadStatusMeja()

        Dim cmd As New OdbcCommand("SELECT meja_id,status FROM tbl_meja", conn)
        Dim rd As OdbcDataReader

        rd = cmd.ExecuteReader()

        While rd.Read()

            Select Case rd("meja_id").ToString

                Case "1"
                    SetStatus(Label13, rd("status").ToString)

                Case "2"
                    SetStatus(Label17, rd("status").ToString)

                Case "3"
                    SetStatus(Label18, rd("status").ToString)

                Case "4"
                    SetStatus(Label16, rd("status").ToString)

                Case "5"
                    SetStatus(Label19, rd("status").ToString)

                Case "6"
                    SetStatus(Label22, rd("status").ToString)

                Case "7"
                    SetStatus(Label20, rd("status").ToString)

                Case "8"
                    SetStatus(Label23, rd("status").ToString)

                Case "9"
                    SetStatus(Label24, rd("status").ToString)

                Case "10"
                    SetStatus(Label21, rd("status").ToString)

            End Select

        End While

        rd.Close()

    End Sub

    Private Sub SetStatus(lbl As Label, status As String)
       
        lbl.Text = status

        Select Case status

            Case "Tersedia"
                lbl.ForeColor = Color.Lime

            Case "Digunakan"
                lbl.ForeColor = Color.Orange

            Case "Error"
                lbl.ForeColor = Color.Red

        End Select

    End Sub
    Sub UpdateTransaksiExpired()

        Try

       Dim CMD As New OdbcCommand(
     "SELECT meja_id FROM tbl_transaksi " &
     "WHERE status='Berjalan' " &
     "AND jam_selesai <= NOW()", conn)

            Dim RD As OdbcDataReader = CMD.ExecuteReader()

            Dim daftarMeja As New List(Of String)

            While RD.Read()

                daftarMeja.Add(RD("meja_id").ToString())

            End While

            RD.Close()

            For Each meja As String In daftarMeja

            Dim CMD2 As New OdbcCommand(
    "UPDATE tbl_meja SET status='Tersedia' " &
    "WHERE meja_id=?", conn)

                CMD2.Parameters.AddWithValue("", meja)
                CMD2.ExecuteNonQuery()

            Dim CMD3 As New OdbcCommand(
    "UPDATE tbl_transaksi SET status='Selesai' " &
    "WHERE meja_id=? " &
    "AND status='Berjalan'", conn)

                CMD3.Parameters.AddWithValue("", meja)
                CMD3.ExecuteNonQuery()

            Next

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub
    Sub LoadSisaWaktu()

        Try

            Label25.Text = "-"
            Label14.Text = "-"
            Label10.Text = "-"
            Label15.Text = "-"
            Label3.Text = "-"
            Label28.Text = "-"
            Label30.Text = "-"
            Label29.Text = "-"
            Label27.Text = "-"
            Label26.Text = "-"

            Dim CMD As New OdbcCommand(
           "SELECT meja_id, jam_selesai FROM tbl_transaksi WHERE status='Berjalan'", conn)

            Dim RD As OdbcDataReader = CMD.ExecuteReader()

            While RD.Read()

                Dim meja As String = RD("meja_id").ToString()
                Dim jamSelesai As DateTime = CDate(RD("jam_selesai"))

                Dim sisa As TimeSpan = jamSelesai - DateTime.Now


                Dim teks As String

                If sisa.TotalSeconds > 0 Then

                    teks = sisa.Hours.ToString("00") & ":" &
                           sisa.Minutes.ToString("00") & ":" &
                           sisa.Seconds.ToString("00")

                Else

                    teks = "00:00:00"

                End If

                Select Case meja

                    Case "1"
                        Label25.Text = teks

                    Case "2"
                        Label14.Text = teks

                    Case "3"
                        Label10.Text = teks

                    Case "4"
                        Label15.Text = teks

                    Case "5"
                        Label3.Text = teks

                    Case "6"
                        Label28.Text = teks

                    Case "7"
                        Label30.Text = teks

                    Case "8"
                        Label29.Text = teks

                    Case "9"
                        Label27.Text = teks

                    Case "10"
                        Label26.Text = teks

                End Select

            End While

            RD.Close()

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs)
        login.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        DataPlaystation.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Transaksi.Show()
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

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        LaporanPendapatan.Show()
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

    Private Sub Button5_Click_1(sender As Object, e As EventArgs)
        AkunSaya.Show()
        Me.Close()
    End Sub

    Public Sub RefreshDashboard()
        LoadDashboard()
        LoadStatusMeja()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Try
        Dim CMD As New OdbcCommand(
    "SELECT transaksi_id, meja_id, jam_selesai FROM tbl_transaksi WHERE status='Berjalan'", conn)

            Dim RD As OdbcDataReader = CMD.ExecuteReader()

            Dim expiredMeja As New HashSet(Of String)

            While RD.Read()

                Dim jamSelesai As DateTime = CDate(RD("jam_selesai"))
                Dim meja As String = RD("meja_id").ToString()

                If DateTime.Now >= jamSelesai Then
                    expiredMeja.Add(meja)
                End If

            End While

            RD.Close()

            '================ UPDATE SETELAH LOOP =================
            For Each meja As String In expiredMeja

                'UPDATE MEJA
                Dim CMD2 As New OdbcCommand(
                "UPDATE tbl_meja SET status='Tersedia' WHERE meja_id=?", conn)

                CMD2.Parameters.AddWithValue("", meja)
                CMD2.ExecuteNonQuery()

                'UPDATE TRANSAKSI
                Dim CMD3 As New OdbcCommand(
                "UPDATE tbl_transaksi SET status='Selesai' WHERE meja_id=?", conn)

                CMD3.Parameters.AddWithValue("", meja)
                CMD3.ExecuteNonQuery()

            Next
            If expiredMeja.Count > 0 Then
                RefreshDashboard()
            End If

            LoadSisaWaktu()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        DataPlaystation.Show()
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

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub Label24_Click(sender As Object, e As EventArgs) Handles Label24.Click

    End Sub
End Class