using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectJerseyBola
{
    public partial class FormCetakReport : Form
    {
        string connectionString = @"Data Source=192.168.1.19\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True";

        public FormCetakReport()
        {
            InitializeComponent();
        }

        private void FormCetakReport_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                // Tarik data dari View persis kayak di FormLaporan
                string query = "SELECT * FROM vw_LaporanPenjualan ORDER BY [Waktu Transaksi] DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // MANGGIL FILE REPORT LU YANG NAMANYA "Penjualan"
                Penjualan rpt = new Penjualan();

                // Cekokin data dari tabel C# ke dalem Report
                rpt.SetDataSource(dt);

                // Tunjukin ke layar Viewer lu
                crystalReportViewer2.ReportSource = rpt;
                crystalReportViewer2.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aduh error narik report: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}   