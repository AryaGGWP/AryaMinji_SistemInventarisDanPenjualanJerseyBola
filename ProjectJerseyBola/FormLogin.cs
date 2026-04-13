using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectJerseyBola
{
    public partial class FormLogin: Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCekKoneksi_Click(object sender, EventArgs e)
        {
            
            string connectionString = @"Data Source=IDEAPAD-ARYA\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True";

            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open(); // Coba buka gerbangnya
                MessageBox.Show("Mantap! Koneksi ke Database Berhasil!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close(); // Tutup lagi biar aman
            }
            catch (Exception ex)
            {
                // Kalau gagal, munculin error-nya
                MessageBox.Show("Waduh, gagal konek cuy: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Username dan Password nggak boleh kosong cuy!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            string connectionString = @"Data Source=IDEAPAD-ARYA\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Admin WHERE Username = @username AND Password = @password";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                int result = (int)cmd.ExecuteScalar();

                if (result > 0)
                {
                    MessageBox.Show("Selamat datang di Toko Jersey!!.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Username atau Password salah, coba ingat-ingat lagi!", "Gagal Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Waduh, ada error di database nih: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
