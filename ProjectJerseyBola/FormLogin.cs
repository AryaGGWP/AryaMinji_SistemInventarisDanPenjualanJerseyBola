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
            // Validasi tetep aman punya lu
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
                // REVISI 1: Ganti COUNT(*) jadi IDAdmin
                string query = "SELECT IDAdmin FROM Admin WHERE Username = @username AND Password = @password";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                object result = cmd.ExecuteScalar();

                if (result != null) 
                {
                    int idAdminLogin = Convert.ToInt32(result);

                    MessageBox.Show("Selamat datang di Minji Sport.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FormMenuUtama menu = new FormMenuUtama(idAdminLogin);
                    menu.Show();
                    this.Hide();
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

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
