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
            
            string connectionString = @"ata Source=IDEAPAD-ARYA\BANGDIO; Initial Catalog=DBAkademikADO; User ID=sa; Password=123; TrustServerCertificate=True";

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
    }
}
