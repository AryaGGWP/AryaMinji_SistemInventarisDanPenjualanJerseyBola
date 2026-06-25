using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // WAJIB ADA BUAT GRAFIK

namespace ProjectJerseyBola
{
    public partial class FormMenuUtama : Form
    {
        public static int idAdminAktif;

        // Koneksi Database
        string connectionString = @"Data Source=192.168.1.19\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True";

        public FormMenuUtama(int idTerima)
        {
            InitializeComponent();
            idAdminAktif = idTerima;
        }

        public FormMenuUtama()
        {
            InitializeComponent();
        }

        // --- METHOD BIKIN GRAFIK ---
        void LoadGrafik()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                // Panggil SP yang baru kita bikin di SSMS
                SqlCommand cmd = new SqlCommand("sp_ChartStokKlub", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // 1. Bersihin setingan default pabrik dari Chart-nya
                chart1.Series.Clear();
                chart1.Titles.Clear();

                // 2. Kasih Judul Grafiknya
                chart1.Titles.Add("Total Stok Jersey Berdasarkan Klub");
                chart1.Titles[0].Font = new System.Drawing.Font("Rockwell", 14, System.Drawing.FontStyle.Bold);

                // 3. Bikin Series (Batang Grafiknya)
                Series s = new Series("Stok Pcs");
                s.ChartType = SeriesChartType.Column; // Bentuk batang
                s.IsValueShownAsLabel = true; // Munculin angka di atas batang

                // 4. Looping masukin data dari database ke dalem grafik
                foreach (DataRow row in dt.Rows)
                {
                    s.Points.AddXY(row["Klub"].ToString(), Convert.ToInt32(row["TotalStok"]));
                }

                // 5. Masukin batang yang udah diisi data ke dalem Chart
                chart1.Series.Add(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load grafik cuy: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void FormMenuUtama_Load(object sender, EventArgs e)
        {
            // Panggil grafik otomatis pas menu utama muncul
            LoadGrafik();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult konfirmasi = MessageBox.Show("Logout dan kembali ke menu awal?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (konfirmasi == DialogResult.Yes)
            {
                this.Hide();

                FormLogin login = new FormLogin();
                login.Show();
            }
        }

        private void btnKelola_Click(object sender, EventArgs e)
        {
            FormKelolaJersey kelola = new FormKelolaJersey();
            kelola.Show();
            this.Hide();
        }

        private void btnStok_Click(object sender, EventArgs e)
        {
            FormCekStok cekStok = new FormCekStok();
            cekStok.Show();
            this.Hide();
        }

        private void btnTransaksi_Click(object sender, EventArgs e)
        {
            FormTransaksi jual = new FormTransaksi(idAdminAktif);
            jual.Show();
            this.Hide();
        }

        private void btnLaporan_Click(object sender, EventArgs e)
        {
            FormLaporan lap = new FormLaporan();
            lap.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}