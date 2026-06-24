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

        // --- METHOD TAMPIL DATA (MENGGUNAKAN VIEW) ---
        void TampilDataLaporan()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();

                // view 
                string query = "SELECT * FROM vw_LaporanPenjualan ORDER BY [Waktu Transaksi] DESC";

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

        // --- POIN 3: TOMBOL CARI (DENGAN SQL INJECTION) ---
        private void btnCari_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();

                // Query digabung langsung pake tanda plus (+), nggak pake Parameter (@)
                string query = "SELECT * FROM vw_LaporanPenjualan WHERE Kasir = '" + txtCari.Text + "'";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvLaporan.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error pencarian: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void label8_Click(object sender, EventArgs e) { }

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
                            PdfPTable pdfTable = new PdfPTable(dgvLaporan.Columns.Count);
                            pdfTable.DefaultCell.Padding = 3;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn column in dgvLaporan.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                                pdfTable.AddCell(cell);
                            }

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

                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 0f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();

                                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                                Paragraph title = new Paragraph("LAPORAN PENJUALAN MINJI SPORT  \n\n", titleFont);
                                title.Alignment = Element.ALIGN_CENTER;
                                pdfDoc.Add(title);

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