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
            // 1. Cek dulu, jangan sampai kasir lupa ngisi tapi udah main klik aja
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Username dan Password nggak boleh kosong cuy!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Berhenti di sini, gak usah lanjut ke database
            }

            // 2. Siapin jembatan koneksi (Ganti tulisan NAMA_LAPTOP dan NAMA_DATABASE ya!)
            string connectionString = @"Data Source=NAMA_LAPTOP_LU;Initial Catalog=NAMA_DATABASE_LU;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                // 3. Bikin query SQL buat nyari akun di tabel Admin
                // Sesuai Class Diagram kita, nama tabelnya 'Admin', kolomnya 'Username' dan 'Password'
                string query = "SELECT COUNT(*) FROM Admin WHERE Username = @username AND Password = @password";
                SqlCommand cmd = new SqlCommand(query, conn);

                // Pakai parameter gini biar aman dari serangan Hacker (SQL Injection)
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                // 4. Eksekusi pencarian
                int result = (int)cmd.ExecuteScalar(); // Hasilnya bakal 1 kalau ketemu, 0 kalau nggak ada

                if (result > 0)
                {
                    MessageBox.Show("Login Berhasil! Selamat datang di BillingPlay.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // TODO: Nanti kodingan buat pindah ke Form Menu Utama / Dashboard ditaruh di sini
                    // FormMenuUtama menu = new FormMenuUtama();
                    // menu.Show();
                    // this.Hide();
                }
                else
                {
                    // Kalau result-nya 0, berarti username/password salah
                    MessageBox.Show("Username atau Password salah, coba ingat-ingat lagi!", "Gagal Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Waduh, ada error di database nih: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Pastikan gerbang selalu ditutup lagi, mau sukses atau error
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
