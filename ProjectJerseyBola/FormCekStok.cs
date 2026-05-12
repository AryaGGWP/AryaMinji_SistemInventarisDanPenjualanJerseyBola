using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectJerseyBola
{
    public partial class FormCekStok : Form
    {
        string connectionString = @"Data Source=IDEAPAD-ARYA\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True";

        public FormCekStok()
        {
            InitializeComponent();
            TampilkanData();
        }

        // --- METHOD TAMPIL DATA (Pake VIEW) ---
        void TampilkanData()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                // REVISI UCP 2: Panggil VIEW (vw_DataJersey)
                string query = "SELECT * FROM vw_DataJersey";
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

        // --- METHOD CARI DATA (Pake STORED PROCEDURE) ---
        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();

                // STORED PROCEDURE untuk Search
                SqlCommand cmd = new SqlCommand("sp_CariJersey", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Keyword", txtCari.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCekStok.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mencari data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}