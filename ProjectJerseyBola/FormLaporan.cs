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
        string connectionString = @"Data Source=192.168.1.19\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True";

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
                // Buka Form Crystal Report lu
                FormCetakReport frmCetak = new FormCetakReport();
                frmCetak.ShowDialog();
            }
            else
            {
                MessageBox.Show("Datanya masih kosong bos, apa yang mau dicetak?", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}