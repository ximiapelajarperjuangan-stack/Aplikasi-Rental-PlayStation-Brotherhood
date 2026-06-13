Imports System.Data.Odbc
Imports System.Drawing.Printing
Public Class Pembayaran
    Dim struk As String = ""
    Dim PD As New PrintDocument

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        RadioButton1.Checked = True

        TampilCash()

        Dim ukuranKertas As New PaperSize(
     "Thermal80",
     325,
     400)

        PrintDocument1.DefaultPageSettings.PaperSize = ukuranKertas

        PrintDocument1.DefaultPageSettings.Margins =
            New Margins(5, 5, 5, 5)

        SetButtonTransparent(Me)
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

    Sub TampilCash()

        Label26.Visible = True
        Label22.Visible = True

        Label27.Visible = True
        TextBox1.Visible = True

        Label28.Visible = True
        Label23.Visible = True

        Label31.Visible = False
        PictureBox1.Visible = False
        Label30.Visible = False
        Label32.Visible = False

    End Sub

    Sub TampilQRIS()

        Label26.Visible = False
        Label22.Visible = False

        Label27.Visible = False
        TextBox1.Visible = False

        Label28.Visible = False
        Label23.Visible = False

        Label31.Visible = True
        PictureBox1.Visible = True
        Label30.Visible = True
        Label32.Visible = True

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged

        If RadioButton1.Checked Then

            TampilCash()

        End If

    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged

        If RadioButton2.Checked Then

            TampilQRIS()

        End If

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Try

            Dim total As Double
            Dim bayar As Double

            total = Val(Label22.Text.Replace("Rp", "").Replace(".", "").Replace(",", ""))

            bayar = Val(TextBox1.Text)

            If bayar >= total Then

                Label23.Text = "Rp " & Format(bayar - total, "#,##0")

            Else

                Label23.Text = "Rp 0"

            End If

        Catch ex As Exception

            Label23.Text = "Rp 0"

        End Try

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)

        Me.Close()

        Transaksi.Show()

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs)
        '================ SIMPAN TRANSAKSI =================

        Try
            Dim query As String =
     "INSERT INTO tbl_transaksi " &
     "(transaksi_id,user_id,nama_pelanggan,meja_id,jam_mulai,jam_selesai,durasi,total_biaya,status,tanggal) " &
     "VALUES (?,?,?,?,?,?,?,?,?,?)"

            Dim CMD As New OdbcCommand(query, conn)
            CMD.Parameters.AddWithValue("", Transaksi.TextBox1.Text)

            CMD.Parameters.AddWithValue("", 1) 'sementara user_id

            CMD.Parameters.AddWithValue("", Transaksi.TextBox2.Text)

            CMD.Parameters.AddWithValue("", Val(Label15.Text))

            CMD.Parameters.AddWithValue("",
                Format(CDate(Label16.Text & " " & Label17.Text),
                "yyyy-MM-dd HH:mm:ss"))

            CMD.Parameters.AddWithValue("",
                Format(CDate(Label18.Text & " " & Label19.Text),
                "yyyy-MM-dd HH:mm:ss"))

            CMD.Parameters.AddWithValue("",
     Label20.Text.Replace(" Jam", ""))

            CMD.Parameters.AddWithValue("",
                Val(Label7.Text.Replace("Rp", "").Replace(".", "").Replace(",", "")))

            CMD.Parameters.AddWithValue("", "Berjalan")

            CMD.Parameters.AddWithValue("",
                Format(Today, "yyyy-MM-dd"))

            CMD.ExecuteNonQuery()

            '================ UPDATE STATUS MEJA =================
            Dim updateMeja As String =
            "UPDATE tbl_meja SET status='Digunakan' WHERE meja_id=?"

            Dim CMD2 As New OdbcCommand(updateMeja, conn)
            CMD2.Parameters.AddWithValue("", Val(Label15.Text))
            CMD2.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Gagal simpan transaksi : " & ex.Message)
            Exit Sub

        End Try

        '================ REFRESH SEMUA FORM =================
        Try
            Dasbor.RefreshDashboard()
        Catch
        End Try

        Try
            DataPlaystation.RefreshData()
        Catch
        End Try

        Try
            Transaksi.RefreshMeja()
        Catch
        End Try

        struk =
 "================================" & vbCrLf &
 "     BROTHERHOOD PLAYSTATION" & vbCrLf &
 "================================" & vbCrLf &
 vbCrLf &
 "Jenis PS    : " & Label14.Text & vbCrLf &
 "Nomor Meja  : " & Label15.Text & vbCrLf &
 "Tanggal     : " & Label16.Text & vbCrLf &
 "Jam Mulai   : " & Label17.Text & vbCrLf &
 "Jam Selesai : " & Label19.Text & vbCrLf &
 "Durasi      : " & Label20.Text & vbCrLf &
 vbCrLf &
 "--------------------------------" & vbCrLf &
 "TOTAL BAYAR : " & Label7.Text & vbCrLf &
 "METODE      : " &
 If(RadioButton1.Checked, "CASH", "QRIS") & vbCrLf &
 "--------------------------------" & vbCrLf &
 vbCrLf &
 "          TERIMA KASIH" & vbCrLf &
 "        SELAMAT BERMAIN" & vbCrLf &
 "================================"

        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.ShowDialog()

    End Sub

    Private Sub CetakStruk(sender As Object, e As PrintPageEventArgs)

        Dim y As Integer = 20

        e.Graphics.DrawString(
        "BROTHERHOOD PLAYSTATION",
        New Font("Arial", 12, FontStyle.Bold),
        Brushes.Black, 20, y)

        y += 30

        e.Graphics.DrawString(
        "Kode : " & Transaksi.TextBox1.Text,
        New Font("Arial", 10),
        Brushes.Black, 20, y)

        y += 20

        e.Graphics.DrawString(
        "Pelanggan : " & Transaksi.TextBox2.Text,
        New Font("Arial", 10),
        Brushes.Black, 20, y)

        y += 20

        e.Graphics.DrawString(
        "Jenis PS : " & Label14.Text,
        New Font("Arial", 10),
        Brushes.Black, 20, y)

        y += 20

        e.Graphics.DrawString(
        "Meja : " & Label15.Text,
        New Font("Arial", 10),
        Brushes.Black, 20, y)

        y += 20

        e.Graphics.DrawString(
        "Durasi : " & Label20.Text,
        New Font("Arial", 10),
        Brushes.Black, 20, y)

        y += 20

        e.Graphics.DrawString(
        "Total : " & Label7.Text,
        New Font("Arial", 10),
        Brushes.Black, 20, y)

        y += 30

        e.Graphics.DrawString(
        "Terima Kasih",
        New Font("Arial", 10),
        Brushes.Black, 20, y)

    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Dim f As New System.Drawing.Font("Consolas", 12, FontStyle.Regular)

        e.Graphics.DrawString(
            struk,
            f,
            Brushes.Black,
            10,
            10)

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

    Private Sub Label23_Click(sender As Object, e As EventArgs) Handles Label23.Click

    End Sub

    Private Sub Label39_Click(sender As Object, e As EventArgs) Handles Label39.Click
        Me.Close()
        Transaksi.Show()
    End Sub

    Private Sub Label40_Click(sender As Object, e As EventArgs) Handles Label40.Click
        '================ SIMPAN TRANSAKSI =================

        Try
            Dim query As String =
     "INSERT INTO tbl_transaksi " &
     "(transaksi_id,user_id,nama_pelanggan,meja_id,jam_mulai,jam_selesai,durasi,total_biaya,status,tanggal) " &
     "VALUES (?,?,?,?,?,?,?,?,?,?)"

            Dim CMD As New OdbcCommand(query, conn)
            CMD.Parameters.AddWithValue("", Transaksi.TextBox1.Text)

            CMD.Parameters.AddWithValue("", 1) 'sementara user_id

            CMD.Parameters.AddWithValue("", Transaksi.TextBox2.Text)

            CMD.Parameters.AddWithValue("", Val(Label15.Text))

            CMD.Parameters.AddWithValue("",
                Format(CDate(Label16.Text & " " & Label17.Text),
                "yyyy-MM-dd HH:mm:ss"))

            CMD.Parameters.AddWithValue("",
                Format(CDate(Label18.Text & " " & Label19.Text),
                "yyyy-MM-dd HH:mm:ss"))

            CMD.Parameters.AddWithValue("",
     Label20.Text.Replace(" Jam", ""))

            CMD.Parameters.AddWithValue("",
                Val(Label7.Text.Replace("Rp", "").Replace(".", "").Replace(",", "")))

            CMD.Parameters.AddWithValue("", "Berjalan")

            CMD.Parameters.AddWithValue("",
                Format(Today, "yyyy-MM-dd"))

            CMD.ExecuteNonQuery()

            '================ UPDATE STATUS MEJA =================
            Dim updateMeja As String =
            "UPDATE tbl_meja SET status='Digunakan' WHERE meja_id=?"

            Dim CMD2 As New OdbcCommand(updateMeja, conn)
            CMD2.Parameters.AddWithValue("", Val(Label15.Text))
            CMD2.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Gagal simpan transaksi : " & ex.Message)
            Exit Sub

        End Try

        '================ REFRESH SEMUA FORM =================
        Try
            Dasbor.RefreshDashboard()
        Catch
        End Try

        Try
            DataPlaystation.RefreshData()
        Catch
        End Try

        Try
            Transaksi.RefreshMeja()
        Catch
        End Try

        struk =
 "================================" & vbCrLf &
 "     BROTHERHOOD PLAYSTATION" & vbCrLf &
 "================================" & vbCrLf &
 vbCrLf &
 "Jenis PS    : " & Label14.Text & vbCrLf &
 "Nomor Meja  : " & Label15.Text & vbCrLf &
 "Tanggal     : " & Label16.Text & vbCrLf &
 "Jam Mulai   : " & Label17.Text & vbCrLf &
 "Jam Selesai : " & Label19.Text & vbCrLf &
 "Durasi      : " & Label20.Text & vbCrLf &
 vbCrLf &
 "--------------------------------" & vbCrLf &
 "TOTAL BAYAR : " & Label7.Text & vbCrLf &
 "METODE      : " &
 If(RadioButton1.Checked, "CASH", "QRIS") & vbCrLf &
 "--------------------------------" & vbCrLf &
 vbCrLf &
 "          TERIMA KASIH" & vbCrLf &
 "        SELAMAT BERMAIN" & vbCrLf &
 "================================"

        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub Label35_Click(sender As Object, e As EventArgs) Handles Label35.Click
        LaporanPendapatan.Show()
        Me.Close()
    End Sub

    Private Sub Label36_Click(sender As Object, e As EventArgs) Handles Label36.Click
        Transaksi.Show()
        Me.Close()
    End Sub

    Private Sub Label34_Click(sender As Object, e As EventArgs) Handles Label34.Click
        AkunSaya.Show()
        Me.Close()
    End Sub

    Private Sub Label33_Click(sender As Object, e As EventArgs) Handles Label33.Click
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

    Private Sub Label38_Click(sender As Object, e As EventArgs) Handles Label38.Click
        Dasbor.Show()
        Me.Close()
    End Sub

    Private Sub Label37_Click(sender As Object, e As EventArgs) Handles Label37.Click
        DataPlaystation.Show()
        Me.Close()
    End Sub
End Class