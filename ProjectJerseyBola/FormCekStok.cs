using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectJerseyBola
{
    public partial class FormCekStok: Form
    {
        string connectionString = @"Data Source=IDEAPAD-ARYA\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True";

        public FormCekStok()
        {
            InitializeComponent();
            TampilkanData();
        }

        // --- METHOD TAMPIL DATA ---
        void TampilkanData()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                // Nampilin semua data stok jersey
                string query = "SELECT * FROM Jersey";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCekStok.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal nampilin stok: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void btnKembali_Click(object sender, EventArgs e)
        {
            FormMenuUtama menu = new FormMenuUtama();
            menu.Show();
            this.Close();
        }

        private void dgvCekStok_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                // Bisa nyari berdasarkan Kode, Nama, atau Klub
                string query = "SELECT * FROM Jersey WHERE NamaJersey LIKE '%" + txtCari.Text + "%' OR Klub LIKE '%" + txtCari.Text + "%' OR KodeJersey LIKE '%" + txtCari.Text + "%'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCekStok.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tidak ada jersey dalam stok." + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
