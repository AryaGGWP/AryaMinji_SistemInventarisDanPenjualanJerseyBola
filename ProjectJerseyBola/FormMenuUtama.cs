using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectJerseyBola
{
    public partial class FormMenuUtama: Form
    {
        public FormMenuUtama()
        {
            InitializeComponent();
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnKelola_Click(object sender, EventArgs e)
        {
            FormKelolaJersey kelola = new FormKelolaJersey();
            kelola.Show(); // Tampilin form kelola
            this.Hide();   // Sembunyiin menu utama sementara
        }
    }
}
