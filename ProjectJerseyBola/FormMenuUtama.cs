using System;
using System.Windows.Forms;

namespace ProjectJerseyBola
{
    public partial class FormMenuUtama : Form
    {
        public static int idAdminAktif;

        public FormMenuUtama(int idTerima)
        {
            InitializeComponent();
            idAdminAktif = idTerima;
        }
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
    }
}