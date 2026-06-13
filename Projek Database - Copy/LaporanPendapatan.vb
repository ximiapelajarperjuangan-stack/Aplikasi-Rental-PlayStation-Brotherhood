Imports System.Data.Odbc
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel
Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting


Public Class LaporanPendapatan
    Private struk As String = ""

    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call Koneksi()

        DateTimePicker1.Format = DateTimePickerFormat.Short
        DateTimePicker2.Format = DateTimePickerFormat.Short
        TampilData()

        DateTimePicker1.Value = Today
        DateTimePicker2.Value = Today

        Label1.Text = "0"
        Label2.Text = "0 Jam"
        Label3.Text = "Rp 0"

    End Sub

    Sub TampilData()

        Try

            Dim sql As String

            sql = "SELECT transaksi_id AS 'Kode Transaksi', " &
                  "nama_pelanggan AS 'Nama Pelanggan', " &
                  "meja_id AS 'Nomor Meja', " &
                  "jam_mulai AS 'Jam Mulai', " &
                  "jam_selesai AS 'Jam Selesai', " &
                  "durasi AS 'Durasi', " &
                  "total_biaya AS 'Total Biaya', " &
                  "tanggal AS 'Tanggal' " &
                  "FROM tbl_transaksi " &
                  "WHERE tanggal BETWEEN ? AND ? " &
                  "ORDER BY tanggal DESC"

            Dim DA As New OdbcDataAdapter(sql, conn)

            DA.SelectCommand.Parameters.AddWithValue("", Format(DateTimePicker1.Value, "yyyy-MM-dd"))
            DA.SelectCommand.Parameters.AddWithValue("", Format(DateTimePicker2.Value, "yyyy-MM-dd"))

            Dim DS As New DataSet

            DA.Fill(DS, "laporan")
            DataGridView1.DataSource = DS.Tables("laporan")
            For Each row As DataGridViewRow In DataGridView1.Rows

                If Not row.IsNewRow Then

                    Dim durasi As String = row.Cells("Durasi").Value.ToString()

                    If durasi.Contains(".") Then
                        durasi = durasi.Split("."c)(0)
                    End If

                    row.Cells("Durasi").Value = durasi & " Jam"

                End If

            Next
            DataGridView1.Columns("Durasi").DefaultCellStyle.Format = "#,##0"
            DataGridView1.Columns("Total Biaya").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            HitungTotal()
            LoadGrafik()

            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DataGridView1.ReadOnly = True
            DataGridView1.AllowUserToAddRows = False

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Sub HitungTotal()

        Try

            Dim tgl1 As String =
                Format(DateTimePicker1.Value, "yyyy-MM-dd")

            Dim tgl2 As String =
                Format(DateTimePicker2.Value, "yyyy-MM-dd")

            'Total Transaksi
            Dim CMD1 As New OdbcCommand(
    "SELECT COUNT(*) FROM tbl_transaksi WHERE tanggal BETWEEN ? AND ?",
    conn)

            CMD1.Parameters.AddWithValue("", tgl1)
            CMD1.Parameters.AddWithValue("", tgl2)

            Label1.Text = CMD1.ExecuteScalar().ToString()

            'Total Jam Bermain
            Dim CMD2 As New OdbcCommand(
      "SELECT IFNULL(SUM(durasi),0) FROM tbl_transaksi WHERE tanggal BETWEEN ? AND ?",
      conn)

            CMD2.Parameters.AddWithValue("", tgl1)
            CMD2.Parameters.AddWithValue("", tgl2)

            Label2.Text =
                CMD2.ExecuteScalar().ToString() & " Jam"

            'Total Pendapatan
            Dim CMD3 As New OdbcCommand(
      "SELECT IFNULL(SUM(total_biaya),0) FROM tbl_transaksi WHERE tanggal BETWEEN ? AND ?",
      conn)

            CMD3.Parameters.AddWithValue("", tgl1)
            CMD3.Parameters.AddWithValue("", tgl2)

            Label3.Text =
                "Rp " &
                Format(CDbl(CMD3.ExecuteScalar()), "#,##0")

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Sub LoadGrafik()

        Try

            Chart1.Series.Clear()
            Chart1.ChartAreas.Clear()

            Chart1.ChartAreas.Add("Area1")

            Dim series1 As New System.Windows.Forms.DataVisualization.Charting.Series("Pendapatan")

            series1.ChartType = SeriesChartType.Column

            Chart1.Series.Add(series1)
            Chart1.Titles.Clear()
            Chart1.Titles.Add("Grafik Pendapatan")

            Chart1.ChartAreas("Area1").AxisX.Title = "Tanggal"
            Chart1.ChartAreas("Area1").AxisY.Title = "Pendapatan"

            Chart1.Legends.Clear()

            Dim sql As String =
         "SELECT tanggal, IFNULL(SUM(total_biaya),0) AS total " &
         "FROM tbl_transaksi " &
         "WHERE tanggal BETWEEN ? AND ? " &
         "GROUP BY tanggal " &
         "ORDER BY tanggal"

            Dim CMD As New OdbcCommand(sql, conn)

            CMD.Parameters.AddWithValue("",
                Format(DateTimePicker1.Value, "yyyy-MM-dd"))

            CMD.Parameters.AddWithValue("",
                Format(DateTimePicker2.Value, "yyyy-MM-dd"))

            Dim RD As OdbcDataReader = CMD.ExecuteReader()

            While RD.Read()

                Chart1.Series("Pendapatan").Points.AddXY(
                    Format(CDate(RD("tanggal")), "dd/MM"),
                    CDbl(RD("total")))

            End While

            RD.Close()

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Dim FJudul As New System.Drawing.Font("Arial", 18, FontStyle.Bold)
        Dim FSub As New System.Drawing.Font("Arial", 11, FontStyle.Bold)
        Dim FIsi As New System.Drawing.Font("Arial", 10)

        Dim y As Integer = 50

        e.Graphics.DrawString(
            "LAPORAN TRANSAKSI RENTAL PLAYSTATION",
            FJudul,
            Brushes.Black,
            180,
            y)

        y += 40

        e.Graphics.DrawString(
            "Periode : " &
            Format(DateTimePicker1.Value, "dd/MM/yyyy") &
            " s/d " &
            Format(DateTimePicker2.Value, "dd/MM/yyyy"),
            FIsi,
            Brushes.Black,
            50,
            y)

        y += 40
e.Graphics.DrawLine(Pens.Black, 50, y, 780, y)

        y += 20

        e.Graphics.DrawString(
            "Total Transaksi : " & Label1.Text,
            FIsi,
            Brushes.Black,
            50,
            y)

        y += 25

        e.Graphics.DrawString(
            "Total Jam Bermain : " & Label2.Text,
            FIsi,
            Brushes.Black,
            50,
            y)

        y += 25

        e.Graphics.DrawString(
            "Total Pendapatan : " & Label3.Text,
            FIsi,
            Brushes.Black,
            50,
            y)

        y += 50

        e.Graphics.DrawString(
     "DETAIL TRANSAKSI",
     New System.Drawing.Font("Arial", 14, FontStyle.Bold),
     Brushes.Black,
     50,
     y)

        y += 35

        'HEADER TABEL
        e.Graphics.DrawString("Kode", FSub, Brushes.Black, 50, y)
        e.Graphics.DrawString("Pelanggan", FSub, Brushes.Black, 180, y)
        e.Graphics.DrawString("Meja", FSub, Brushes.Black, 350, y)
        e.Graphics.DrawString("Tanggal", FSub, Brushes.Black, 430, y)
        e.Graphics.DrawString("Durasi", FSub, Brushes.Black, 560, y)
        e.Graphics.DrawString("Total", FSub, Brushes.Black, 650, y)

        y += 25

        e.Graphics.DrawLine(Pens.Black, 50, y, 780, y)

        y += 10

        For Each row As DataGridViewRow In DataGridView1.Rows

            If Not row.IsNewRow Then

                e.Graphics.DrawString(
                    row.Cells(0).Value.ToString(),
                    FIsi,
                    Brushes.Black,
                    New RectangleF(50, y, 120, 20))

                e.Graphics.DrawString(
                    row.Cells(1).Value.ToString(),
                    FIsi,
                    Brushes.Black,
                    New RectangleF(180, y, 150, 20))

                e.Graphics.DrawString(
                    row.Cells(2).Value.ToString(),
                    FIsi,
                    Brushes.Black,
                    New RectangleF(350, y, 60, 20))

                e.Graphics.DrawString(
    Format(CDate(row.Cells(7).Value), "dd/MM/yyyy"),
    FIsi,
    Brushes.Black,
    New RectangleF(430, y, 110, 20))

                Dim durasi As Integer = CInt(Val(row.Cells(5).Value.ToString()))

                e.Graphics.DrawString(
                    durasi & " Jam",
                    FIsi,
                    Brushes.Black,
                    New RectangleF(560, y, 70, 20))

                e.Graphics.DrawString(
                    "Rp " & Format(CDbl(row.Cells(6).Value), "#,##0"),
                    FIsi,
                    Brushes.Black,
                    New RectangleF(650, y, 100, 20))

                y += 25

            End If

        Next

        y += 20

        e.Graphics.DrawLine(Pens.Black, 50, y, 780, y)

        y += 20

        e.Graphics.DrawString(
     "TOTAL PENDAPATAN : " & Label3.Text,
     New System.Drawing.Font("Arial", 12, FontStyle.Bold),
     Brushes.Black,
     450,
     y)

        y += 80

        e.Graphics.DrawString(
            "Admin Rental",
            FIsi,
            Brushes.Black,
            620,
            y)

        y += 60

        e.Graphics.DrawString(
            "(________________)",
            FIsi,
            Brushes.Black,
            580,
            y)

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Dasbor.Show()
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

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click
        DataPlaystation.Show()
        Me.Close()
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        Transaksi.Show()
        Me.Close()
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        AkunSaya.Show()
        Me.Close()
    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click
        TampilData()
    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs) Handles Label10.Click
        If DataGridView1.Rows.Count = 0 Then

            MessageBox.Show("Tidak ada data laporan")
            Exit Sub

        End If

        PrintPreviewDialog1.Document = PrintDocument1

        PrintPreviewDialog1.WindowState = FormWindowState.Maximized

        PrintPreviewDialog1.ShowDialog()
    End Sub
End Class