using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO; 
using iTextSharp.text; 
using iTextSharp.text.pdf;
namespace ProjectJerseyBola
{
    public partial class FormLaporan : Form
    {
        string connectionString = @"Data Source=IDEAPAD-ARYA\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True";

        public FormLaporan()
        {
            InitializeComponent();
            TampilDataLaporan(); 
        }

        // --- METHOD TAMPIL DATA (DIBIKIN CANTIK PAKE JOIN) ---
        void TampilDataLaporan()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = @"SELECT 
                                    p.IDJual AS [ID Transaksi], 
                                    p.Tanggal AS [Waktu Transaksi], 
                                    j.NamaJersey AS [Nama Barang], 
                                    p.Jumlah AS [Qty], 
                                    p.TotalHarga AS [Total (Rp)], 
                                    a.Username AS [Kasir]
                                 FROM Penjualan p
                                 JOIN Jersey j ON p.KodeJersey = j.KodeJersey
                                 LEFT JOIN Admin a ON p.IDAdmin = a.IDAdmin
                                 ORDER BY p.Tanggal DESC"; 

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvLaporan.DataSource = dt;

                dgvLaporan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal nampilin laporan: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void label8_Click(object sender, EventArgs e){}

        // --- TOMBOL KEMBALI ---
        private void btnKembali_Click(object sender, EventArgs e)
        {
            FormMenuUtama menu = new FormMenuUtama();
            menu.Show();
            this.Close();
        }

        // --- TOMBOL CETAK PDF ---
        private void btnCetak_Click(object sender, EventArgs e)
        {
            if (dgvLaporan.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                string tanggalHariIni = DateTime.Now.ToString("dd-MM-yyyy");
                sfd.FileName = "Laporan Penjualan Minji Sport (" + tanggalHariIni + ").pdf";

                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException)
                        {
                            fileError = true;
                            MessageBox.Show("PDF-nya lagi kebuka di aplikasi lain cuy. Tutup dulu gih biar bisa ditimpa.");
                        }
                    }

                    if (!fileError)
                    {
                        try
                        {
                            // Bikin tabel PDF berdasarkan jumlah kolom di DGV
                            PdfPTable pdfTable = new PdfPTable(dgvLaporan.Columns.Count);
                            pdfTable.DefaultCell.Padding = 3;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            // 1. Masukin Header (Judul Kolom)
                            foreach (DataGridViewColumn column in dgvLaporan.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                                pdfTable.AddCell(cell);
                            }

                            // 2. Masukin Isi Datanya
                            foreach (DataGridViewRow row in dgvLaporan.Rows)
                            {
                                if (!row.IsNewRow) 
                                {
                                    foreach (DataGridViewCell cell in row.Cells)
                                    {
                                        pdfTable.AddCell(cell.Value?.ToString() ?? "");
                                    }
                                }
                            }

                            // 3. Proses bikin file PDF
                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 0f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();

                                // Bikin Judul Laporan
                                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                                Paragraph title = new Paragraph("LAPORAN PENJUALAN MINJI SPORT  \n\n", titleFont);
                                title.Alignment = Element.ALIGN_CENTER;
                                pdfDoc.Add(title);

                                // Masuk tabel
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Laporan Berhasil Diexport ke PDF!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error pas nyetak cuy: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Datanya masih kosong bos, apa yang mau dicetak?", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}